using San.Common;
using Xunit;

namespace San.Audit.Tests;

public sealed class ReceiptQueryBoundaryValidationTests
{
    private static readonly DateTimeOffset TimestampUtc = DateTimeOffset.Parse("2026-05-12T00:00:00Z");

    [Fact]
    public void Query_Returns_Matching_Receipts_Only()
    {
        var target = CreateRoutingReceipt(CreatePacket(packetHandle: "packet://target"));
        var other = CreateRoutingReceipt(CreatePacket(packetHandle: "packet://other"));

        var query = Query(CreateRequest(
            retainedReceipts: [target, other],
            filter: new ReceiptQueryFilter(
                PacketHandle: target.PacketHandle,
                Disposition: null,
                OutcomeCode: null)));

        AssertColdQuery(query);
        Assert.Equal(ReceiptQueryDisposition.LocatedForReviewCold, query.Disposition);
        Assert.Single(query.Evidence);
        Assert.Equal(target.ReceiptHandle, query.Evidence[0].OriginalReceiptHandle);
        Assert.Equal(target.PacketHandle, query.Evidence[0].OriginalPacketHandle);
    }

    [Fact]
    public void Query_Requires_Scope_Boundary()
    {
        var query = Query(CreateRequest(scopeBoundary: new ReceiptQueryScopeBoundary(
            ScopeCode: string.Empty,
            Present: false,
            ReviewOnly: true,
            AllowsWarrant: false)));

        AssertRefused(query, "receipt-query-scope-boundary-missing");
    }

    [Fact]
    public void Query_Summary_Cannot_Authorize()
    {
        var query = Query(CreateRequest());

        AssertColdQuery(query);
        Assert.False(query.QuerySummaryGrantsAuthority);
        Assert.False(query.AuthorityGranted);
        Assert.False(query.NonWarrantBoundary.QuerySummaryMayAuthorize);
    }

    [Fact]
    public void Query_Summary_Cannot_Admit_Continuity()
    {
        var query = Query(CreateRequest());

        AssertColdQuery(query);
        Assert.False(query.QuerySummaryAdmitsContinuity);
        Assert.False(query.ContinuityAdmitted);
        Assert.False(query.NonWarrantBoundary.QuerySummaryMayAdmitContinuity);
    }

    [Fact]
    public void Query_Does_Not_Replay_Receipts()
    {
        var query = Query(CreateRequest());

        AssertColdQuery(query);
        Assert.False(query.ReceiptsReplayed);
        Assert.False(query.NonWarrantBoundary.QueryReplaysReceipts);
    }

    [Fact]
    public void Query_Does_Not_Increment_Passage_Count()
    {
        var query = Query(CreateRequest(priorPassageCount: 11));

        AssertColdQuery(query);
        Assert.Equal(11, query.PriorPassageCount);
        Assert.Equal(11, query.PassageCountAfterQuery);
        Assert.False(query.NonWarrantBoundary.IncrementsPassageCount);
    }

    [Fact]
    public void Query_Result_Preserves_Original_Receipt_Handles()
    {
        var first = CreateRoutingReceipt(CreatePacket(packetHandle: "packet://first"));
        var second = CreateRoutingReceipt(CreatePacket(packetHandle: "packet://second"));

        var query = Query(CreateRequest(retainedReceipts: [first, second]));

        AssertColdQuery(query);
        Assert.Equal([first.ReceiptHandle, second.ReceiptHandle], query.Evidence.Select(static evidence => evidence.OriginalReceiptHandle));
        Assert.All(query.Evidence, evidence => Assert.True(evidence.PreservesOriginalReceiptHandle));
    }

    [Fact]
    public void Empty_Query_Returns_Reviewable_Empty_Result()
    {
        var query = Query(CreateRequest(filter: new ReceiptQueryFilter(
            PacketHandle: "packet://missing",
            Disposition: null,
            OutcomeCode: null)));

        AssertColdQuery(query);
        Assert.Equal(ReceiptQueryDisposition.EmptyReviewCold, query.Disposition);
        Assert.Equal("receipt-query-empty-review-only", query.OutcomeCode);
        Assert.Empty(query.Evidence);
        Assert.Equal(0, query.AggregateCount);
    }

    [Fact]
    public void Query_Aggregate_Count_Cannot_Authorize()
    {
        var first = CreateRoutingReceipt(CreatePacket(packetHandle: "packet://first"));
        var second = CreateRoutingReceipt(CreatePacket(packetHandle: "packet://second"));

        var query = Query(CreateRequest(retainedReceipts: [first, second]));

        AssertColdQuery(query);
        Assert.Equal(2, query.AggregateCount);
        Assert.False(query.NonWarrantBoundary.AggregateCountMayAuthorize);
        Assert.False(query.AuthorityGranted);
    }

    [Fact]
    public void Query_Filter_Does_Not_Create_New_Receipt_Handle()
    {
        var target = CreateRoutingReceipt(CreatePacket(packetHandle: "packet://target"));

        var query = Query(CreateRequest(
            retainedReceipts: [target],
            filter: new ReceiptQueryFilter(
                PacketHandle: target.PacketHandle,
                Disposition: target.Disposition,
                OutcomeCode: null)));

        AssertColdQuery(query);
        Assert.Single(query.Evidence);
        Assert.Equal(target.ReceiptHandle, query.Evidence[0].OriginalReceiptHandle);
        Assert.NotEqual(query.ReceiptHandle, query.Evidence[0].OriginalReceiptHandle);
        Assert.False(query.NonWarrantBoundary.QueryCreatesNewEvidenceReceiptHandles);
    }

    [Fact]
    public void Query_Refuses_Missing_Witness_Context()
    {
        var query = Query(CreateRequest(witnessContext: new QueryWitnessContext(
            WitnessSurface: string.Empty,
            WitnessPresent: false,
            SeparateCustody: false)));

        AssertRefused(query, "receipt-query-witness-context-missing");
    }

    private static ReceiptQueryReceipt Query(ReceiptQueryRequest request) =>
        new DefaultReceiptQueryBoundaryValidator().Query(request, TimestampUtc);

    private static ReceiptQueryRequest CreateRequest(
        IReadOnlyList<PacketReceiptRoutingReceipt>? retainedReceipts = null,
        ReceiptQueryFilter? filter = null,
        QueryWitnessContext? witnessContext = null,
        ReceiptQueryScopeBoundary? scopeBoundary = null,
        int priorPassageCount = 1) =>
        new(
            QueryHandle: $"receipt-query://{Guid.NewGuid():N}",
            RetainedReceipts: retainedReceipts ?? [CreateRoutingReceipt(CreatePacket())],
            Filter: filter ?? new ReceiptQueryFilter(
                PacketHandle: null,
                Disposition: null,
                OutcomeCode: null),
            WitnessContext: witnessContext ?? new QueryWitnessContext(
                WitnessSurface: SanctuaryPacketSurfaces.Steward,
                WitnessPresent: true,
                SeparateCustody: true),
            ScopeBoundary: scopeBoundary ?? new ReceiptQueryScopeBoundary(
                ScopeCode: "review-only",
                Present: true,
                ReviewOnly: true,
                AllowsWarrant: false),
            PriorPassageCount: priorPassageCount);

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

    private static void AssertColdQuery(ReceiptQueryReceipt query)
    {
        Assert.True(query.IsColdQuery);
        Assert.True(query.ReviewOnly);
        Assert.Equal(query.Evidence.Count, query.AggregateCount);
        Assert.Equal(query.PriorPassageCount, query.PassageCountAfterQuery);
        Assert.False(query.QuerySummaryGrantsAuthority);
        Assert.False(query.QuerySummaryAdmitsContinuity);
        Assert.False(query.ReceiptsReplayed);
        Assert.False(query.NewPacketEmitted);
        Assert.True(query.ActivationRefused);
        Assert.False(query.AuthorityGranted);
        Assert.False(query.ContinuityAdmitted);
    }

    private static void AssertRefused(ReceiptQueryReceipt query, string outcomeCode)
    {
        Assert.Equal(ReceiptQueryDisposition.Refused, query.Disposition);
        Assert.Equal(outcomeCode, query.OutcomeCode);
        Assert.True(query.IsRetainedQueryRefusal);
        Assert.NotNull(query.Refusal);
        Assert.True(query.Refusal!.Retained);
        Assert.Empty(query.Evidence);
        Assert.False(query.QuerySummaryGrantsAuthority);
        Assert.False(query.QuerySummaryAdmitsContinuity);
        Assert.False(query.ReceiptsReplayed);
        Assert.False(query.NewPacketEmitted);
        Assert.True(query.ActivationRefused);
        Assert.False(query.AuthorityGranted);
        Assert.False(query.ContinuityAdmitted);
    }
}
