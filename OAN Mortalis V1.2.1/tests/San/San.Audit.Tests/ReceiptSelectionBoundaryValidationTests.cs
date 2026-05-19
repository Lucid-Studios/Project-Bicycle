using San.Common;
using Xunit;

namespace San.Audit.Tests;

public sealed class ReceiptSelectionBoundaryValidationTests
{
    private static readonly DateTimeOffset TimestampUtc = DateTimeOffset.Parse("2026-05-12T00:00:00Z");

    [Fact]
    public void Selection_Nominates_Requested_Evidence_Only()
    {
        var first = CreateRoutingReceipt(CreatePacket(packetHandle: "packet://first"));
        var second = CreateRoutingReceipt(CreatePacket(packetHandle: "packet://second"));
        var query = CreateQueryReceipt([first, second]);

        var selection = Select(CreateRequest(
            queryReceipt: query,
            requestedHandles: [first.ReceiptHandle]));

        AssertColdSelection(selection);
        Assert.Equal(ReceiptSelectionDisposition.NominatedForReviewCold, selection.Disposition);
        Assert.Single(selection.Nominations);
        Assert.Equal(first.ReceiptHandle, selection.Nominations[0].OriginalReceiptHandle);
        Assert.Equal(first.PacketHandle, selection.Nominations[0].OriginalPacketHandle);
    }

    [Fact]
    public void Selection_Requires_Query_Receipt()
    {
        var selection = Select(CreateRequest(omitQueryReceipt: true));

        AssertRefused(selection, "receipt-selection-query-receipt-missing");
    }

    [Fact]
    public void Selection_Requires_Scope_Boundary()
    {
        var selection = Select(CreateRequest(scopeBoundary: new ReceiptSelectionScopeBoundary(
            ScopeCode: string.Empty,
            Present: false,
            ReviewOnly: true,
            AllowsAuthority: false,
            AllowsContinuityAdmission: false,
            AllowsCompassTruth: false)));

        AssertRefused(selection, "receipt-selection-scope-boundary-missing");
    }

    [Fact]
    public void Selection_Nomination_Cannot_Authorize()
    {
        var selection = Select(CreateRequest());

        AssertColdSelection(selection);
        Assert.False(selection.SelectionGrantsAuthority);
        Assert.False(selection.AuthorityGranted);
        Assert.False(selection.NonAdmissionBoundary.NominationMayAuthorize);
    }

    [Fact]
    public void Selection_Nomination_Cannot_Admit_Continuity()
    {
        var selection = Select(CreateRequest());

        AssertColdSelection(selection);
        Assert.False(selection.SelectionAdmitsContinuity);
        Assert.False(selection.ContinuityAdmitted);
        Assert.False(selection.NonAdmissionBoundary.NominationMayAdmitContinuity);
    }

    [Fact]
    public void Selection_Nomination_Cannot_Become_Compass_Truth()
    {
        var selection = Select(CreateRequest());

        AssertColdSelection(selection);
        Assert.False(selection.SelectionBecomesCompassTruth);
        Assert.False(selection.NonAdmissionBoundary.NominationMayBecomeCompassTruth);
    }

    [Fact]
    public void Selection_Does_Not_Replay_Receipts()
    {
        var selection = Select(CreateRequest());

        AssertColdSelection(selection);
        Assert.False(selection.ReceiptsReplayed);
        Assert.False(selection.NonAdmissionBoundary.SelectionReplaysReceipts);
    }

    [Fact]
    public void Selection_Does_Not_Increment_Passage_Count()
    {
        var selection = Select(CreateRequest(priorPassageCount: 13));

        AssertColdSelection(selection);
        Assert.Equal(13, selection.PriorPassageCount);
        Assert.Equal(13, selection.PassageCountAfterSelection);
        Assert.False(selection.NonAdmissionBoundary.IncrementsPassageCount);
    }

    [Fact]
    public void Selection_Does_Not_Create_New_Evidence_Receipt_Handle()
    {
        var routing = CreateRoutingReceipt(CreatePacket(packetHandle: "packet://target"));
        var query = CreateQueryReceipt([routing]);

        var selection = Select(CreateRequest(queryReceipt: query));

        AssertColdSelection(selection);
        Assert.Single(selection.Nominations);
        Assert.Equal(routing.ReceiptHandle, selection.Nominations[0].OriginalReceiptHandle);
        Assert.NotEqual(selection.ReceiptHandle, selection.Nominations[0].OriginalReceiptHandle);
        Assert.False(selection.NonAdmissionBoundary.SelectionCreatesNewEvidenceReceiptHandles);
    }

    [Fact]
    public void Empty_Selection_Returns_Reviewable_Empty_Result()
    {
        var query = CreateQueryReceipt([]);

        var selection = Select(CreateRequest(queryReceipt: query));

        AssertColdSelection(selection);
        Assert.Equal(ReceiptSelectionDisposition.EmptyReviewCold, selection.Disposition);
        Assert.Equal("receipt-selection-empty-review-only", selection.OutcomeCode);
        Assert.Empty(selection.Nominations);
        Assert.Equal(0, selection.NominationCount);
    }

    [Fact]
    public void Selection_Refuses_Unknown_Evidence_Handle()
    {
        var query = CreateQueryReceipt([CreateRoutingReceipt(CreatePacket())]);

        var selection = Select(CreateRequest(
            queryReceipt: query,
            requestedHandles: ["packet-receipt-routing://missing"]));

        AssertRefused(selection, "receipt-selection-unknown-evidence-handle");
    }

    [Fact]
    public void Selection_Refuses_Missing_Witness_Context()
    {
        var selection = Select(CreateRequest(witnessContext: new SelectionWitnessContext(
            WitnessSurface: string.Empty,
            WitnessPresent: false,
            SeparateCustody: false)));

        AssertRefused(selection, "receipt-selection-witness-context-missing");
    }

    [Fact]
    public void Selection_Refuses_Admission_Scope()
    {
        var selection = Select(CreateRequest(scopeBoundary: new ReceiptSelectionScopeBoundary(
            ScopeCode: "review-plus-admission",
            Present: true,
            ReviewOnly: true,
            AllowsAuthority: false,
            AllowsContinuityAdmission: true,
            AllowsCompassTruth: false)));

        AssertRefused(selection, "receipt-selection-admission-scope-refused");
    }

    private static ReceiptSelectionReceipt Select(ReceiptSelectionRequest request) =>
        new DefaultReceiptSelectionBoundaryValidator().Select(request, TimestampUtc);

    private static ReceiptSelectionRequest CreateRequest(
        ReceiptQueryReceipt? queryReceipt = null,
        IReadOnlyList<string>? requestedHandles = null,
        SelectionWitnessContext? witnessContext = null,
        ReceiptSelectionScopeBoundary? scopeBoundary = null,
        int priorPassageCount = 1,
        bool omitQueryReceipt = false) =>
        new(
            SelectionHandle: $"receipt-selection://{Guid.NewGuid():N}",
            QueryReceipt: omitQueryReceipt
                ? null
                : queryReceipt ?? CreateQueryReceipt([CreateRoutingReceipt(CreatePacket())]),
            RequestedOriginalReceiptHandles: requestedHandles ?? [],
            WitnessContext: witnessContext ?? new SelectionWitnessContext(
                WitnessSurface: SanctuaryPacketSurfaces.Steward,
                WitnessPresent: true,
                SeparateCustody: true),
            ScopeBoundary: scopeBoundary ?? new ReceiptSelectionScopeBoundary(
                ScopeCode: "review-only",
                Present: true,
                ReviewOnly: true,
                AllowsAuthority: false,
                AllowsContinuityAdmission: false,
                AllowsCompassTruth: false),
            PriorPassageCount: priorPassageCount);

    private static ReceiptQueryReceipt CreateQueryReceipt(IReadOnlyList<PacketReceiptRoutingReceipt> retainedReceipts) =>
        new DefaultReceiptQueryBoundaryValidator().Query(
            new ReceiptQueryRequest(
                QueryHandle: $"receipt-query://{Guid.NewGuid():N}",
                RetainedReceipts: retainedReceipts,
                Filter: new ReceiptQueryFilter(
                    PacketHandle: null,
                    Disposition: null,
                    OutcomeCode: null),
                WitnessContext: new QueryWitnessContext(
                    WitnessSurface: SanctuaryPacketSurfaces.Steward,
                    WitnessPresent: true,
                    SeparateCustody: true),
                ScopeBoundary: new ReceiptQueryScopeBoundary(
                    ScopeCode: "review-only",
                    Present: true,
                    ReviewOnly: true,
                    AllowsWarrant: false),
                PriorPassageCount: 1),
            TimestampUtc);

    private static PacketReceiptRoutingReceipt CreateRoutingReceipt(SanctuaryPacket packet)
    {
        var validationReceipt = new DefaultSanctuaryPacketMembraneValidator().Validate(packet, TimestampUtc);
        return new DefaultPacketReceiptRoutingValidator().Route(packet, validationReceipt, TimestampUtc);
    }

    private static SanctuaryPacket CreatePacket(string? packetHandle = null) =>
        new(
            PacketHandle: packetHandle ?? $"packet://{Guid.NewGuid():N}",
            PacketKind: "candidate-structure",
            Address: new MembraneAddress(
                SourceSurface: SanctuaryPacketSurfaces.Prime,
                TargetSurface: SanctuaryPacketSurfaces.Steward,
                Route: SanctuaryPacketRoutes.CGoaInsulated),
            AuthorityCeiling: new AuthorityCeiling(
                CeilingCode: "review-only",
                MayAuthorize: false,
                MayPromoteContinuity: false,
                MayActivate: false),
            CustodyEnvelope: new CustodyEnvelope(
                CustodyOwner: SanctuaryPacketSurfaces.Steward,
                RevocationPath: "revocation://packet",
                WitnessRefs: ["witness://steward"]),
            Telemetry: new TelemetryString(
                TraceId: "trace://packet",
                Route: SanctuaryPacketRoutes.CGoaInsulated,
                AttemptsAuthority: false),
            Witness: new WitnessReceipt(
                ReceiptHandle: "witness://steward",
                WitnessSurface: SanctuaryPacketSurfaces.Steward,
                SeparateCustody: true),
            CompassShell: null,
            AttemptsRuntimeAction: false,
            AttemptsActivation: false,
            AttemptsContinuityPromotion: false,
            AttemptsSelfAuthorization: false);

    private static void AssertColdSelection(ReceiptSelectionReceipt selection)
    {
        Assert.True(selection.IsColdSelection);
        Assert.True(selection.ReviewOnly);
        Assert.Equal(selection.Nominations.Count, selection.NominationCount);
        Assert.Equal(selection.PriorPassageCount, selection.PassageCountAfterSelection);
        Assert.False(selection.SelectionGrantsAuthority);
        Assert.False(selection.SelectionAdmitsContinuity);
        Assert.False(selection.SelectionBecomesCompassTruth);
        Assert.False(selection.ReceiptsReplayed);
        Assert.False(selection.NewPacketEmitted);
        Assert.True(selection.ActivationRefused);
        Assert.False(selection.AuthorityGranted);
        Assert.False(selection.ContinuityAdmitted);
    }

    private static void AssertRefused(ReceiptSelectionReceipt selection, string outcomeCode)
    {
        Assert.Equal(ReceiptSelectionDisposition.Refused, selection.Disposition);
        Assert.Equal(outcomeCode, selection.OutcomeCode);
        Assert.True(selection.IsRetainedSelectionRefusal);
        Assert.NotNull(selection.Refusal);
        Assert.True(selection.Refusal!.Retained);
        Assert.Empty(selection.Nominations);
        Assert.False(selection.SelectionGrantsAuthority);
        Assert.False(selection.SelectionAdmitsContinuity);
        Assert.False(selection.SelectionBecomesCompassTruth);
        Assert.False(selection.ReceiptsReplayed);
        Assert.False(selection.NewPacketEmitted);
        Assert.True(selection.ActivationRefused);
        Assert.False(selection.AuthorityGranted);
        Assert.False(selection.ContinuityAdmitted);
    }
}
