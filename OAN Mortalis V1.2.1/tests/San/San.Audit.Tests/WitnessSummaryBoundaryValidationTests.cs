using San.Common;
using Xunit;

namespace San.Audit.Tests;

public sealed class WitnessSummaryBoundaryValidationTests
{
    private static readonly DateTimeOffset TimestampUtc = DateTimeOffset.Parse("2026-05-12T00:00:00Z");

    [Fact]
    public void Summary_Groups_Selected_Evidence_By_Type()
    {
        var summary = Summarize(CreateRequest());

        AssertColdSummary(summary);
        Assert.Contains(summary.Groups, group => group.GroupCode == "validation" && group.ArtifactIds.Contains("packet-membrane-validation-matrix"));
        Assert.Contains(summary.Groups, group => group.GroupCode == "routing" && group.ArtifactIds.Contains("receipt-non-permission-ledger"));
        Assert.Contains(summary.Groups, group => group.GroupCode == "replay" && group.ArtifactIds.Contains("replay-non-reentry-ledger"));
        Assert.Contains(summary.Groups, group => group.GroupCode == "query" && group.ArtifactIds.Contains("query-non-warrant-ledger"));
        Assert.Contains(summary.Groups, group => group.GroupCode == "selection" && group.ArtifactIds.Contains("selection-non-admission-ledger"));
        Assert.Contains(summary.Groups, group => group.GroupCode == "refusal" && group.ArtifactIds.Contains("packet-non-authority-refusal-ledger"));
        Assert.Contains(summary.Groups, group => group.GroupCode == "retained-artifacts" && group.ArtifactIds.Count == summary.ArtifactLineage.Count);
    }

    [Fact]
    public void Summary_Preserves_Original_Receipt_Handles()
    {
        var selection = CreateSelectionReceipt();

        var summary = Summarize(CreateRequest(selectionReceipt: selection));

        AssertColdSummary(summary);
        var originalHandle = selection.Nominations[0].OriginalReceiptHandle;
        Assert.All(summary.Groups, group => Assert.Contains(originalHandle, group.OriginalReceiptHandles));
    }

    [Fact]
    public void Summary_Preserves_Artifact_Lineage()
    {
        var summary = Summarize(CreateRequest());

        AssertColdSummary(summary);
        Assert.All(summary.ArtifactLineage, artifact => Assert.True(artifact.PreservesLineage));
        Assert.Contains(summary.ArtifactLineage, artifact =>
            artifact.ArtifactId == "selection-non-admission-ledger" &&
            artifact.CellId == "packet-membrane.receipt-selection-boundary" &&
            artifact.SourcePath.EndsWith("selection-non-admission-ledger.json", StringComparison.Ordinal));
    }

    [Fact]
    public void Summary_Cannot_Authorize()
    {
        var summary = Summarize(CreateRequest());

        AssertColdSummary(summary);
        Assert.False(summary.SummaryGrantsAuthority);
        Assert.False(summary.AuthorityGranted);
        Assert.False(summary.NonReplacementBoundary.SummaryMayAuthorize);
    }

    [Fact]
    public void Summary_Cannot_Admit_Continuity()
    {
        var summary = Summarize(CreateRequest());

        AssertColdSummary(summary);
        Assert.False(summary.SummaryAdmitsContinuity);
        Assert.False(summary.ContinuityAdmitted);
        Assert.False(summary.NonReplacementBoundary.SummaryMayAdmitContinuity);
    }

    [Fact]
    public void Summary_Cannot_Become_Compass_Truth()
    {
        var summary = Summarize(CreateRequest());

        AssertColdSummary(summary);
        Assert.False(summary.SummaryBecomesCompassTruth);
        Assert.False(summary.NonReplacementBoundary.SummaryMayBecomeCompassTruth);
    }

    [Fact]
    public void Summary_Does_Not_Replay_Receipts()
    {
        var summary = Summarize(CreateRequest());

        AssertColdSummary(summary);
        Assert.False(summary.ReceiptsReplayed);
        Assert.False(summary.NonReplacementBoundary.SummaryReplaysReceipts);
    }

    [Fact]
    public void Summary_Does_Not_Create_New_Evidence_Handle()
    {
        var selection = CreateSelectionReceipt();

        var summary = Summarize(CreateRequest(selectionReceipt: selection));

        AssertColdSummary(summary);
        var originalHandle = selection.Nominations[0].OriginalReceiptHandle;
        Assert.NotEqual(summary.ReceiptHandle, originalHandle);
        Assert.Contains(summary.Groups, group => group.OriginalReceiptHandles.Contains(originalHandle));
        Assert.False(summary.NonReplacementBoundary.SummaryCreatesNewEvidenceReceiptHandles);
    }

    [Fact]
    public void Summary_Requires_Selection_Source()
    {
        var summary = Summarize(CreateRequest(omitSelectionReceipt: true));

        AssertRefused(summary, "witness-summary-selection-receipt-missing");
    }

    [Fact]
    public void Summary_Requires_Scope_Boundary()
    {
        var summary = Summarize(CreateRequest(scopeBoundary: new WitnessSummaryScopeBoundary(
            ScopeCode: string.Empty,
            Present: false,
            ReviewOnly: true,
            AllowsAuthority: false,
            AllowsContinuityAdmission: false,
            AllowsEvidenceReplacement: false,
            AllowsCompassTruth: false)));

        AssertRefused(summary, "witness-summary-scope-boundary-missing");
    }

    [Fact]
    public void Summary_Requires_Witness_Context()
    {
        var summary = Summarize(CreateRequest(witnessContext: new SummaryWitnessContext(
            WitnessSurface: string.Empty,
            WitnessPresent: false,
            SeparateCustody: false)));

        AssertRefused(summary, "witness-summary-witness-context-missing");
    }

    [Fact]
    public void Summary_Returns_Review_Only_Confidence()
    {
        var summary = Summarize(CreateRequest(confidenceEstimate: 0.88m));

        AssertColdSummary(summary);
        Assert.Equal(0.88m, summary.ConfidenceEstimate);
        Assert.True(summary.ReviewOnly);
        Assert.True(summary.ConfidenceEstimate is >= 0m and <= 1m);
    }

    [Fact]
    public void Summary_Refuses_Confidence_Out_Of_Range()
    {
        var summary = Summarize(CreateRequest(confidenceEstimate: 1.2m));

        AssertRefused(summary, "witness-summary-confidence-out-of-range");
        Assert.Equal(0m, summary.ConfidenceEstimate);
    }

    [Fact]
    public void Empty_Summary_Returns_Reviewable_Empty_Result()
    {
        var emptySelection = CreateSelectionReceipt(retainedReceipts: []);

        var summary = Summarize(CreateRequest(
            selectionReceipt: emptySelection,
            artifactLineage: []));

        AssertColdSummary(summary);
        Assert.Equal(WitnessSummaryDisposition.EmptyReviewCold, summary.Disposition);
        Assert.Equal("witness-summary-empty-review-only", summary.OutcomeCode);
        Assert.Empty(summary.ArtifactLineage);
    }

    [Fact]
    public void Summary_Refuses_Replacement_Scope()
    {
        var summary = Summarize(CreateRequest(scopeBoundary: new WitnessSummaryScopeBoundary(
            ScopeCode: "review-plus-replacement",
            Present: true,
            ReviewOnly: true,
            AllowsAuthority: false,
            AllowsContinuityAdmission: false,
            AllowsEvidenceReplacement: true,
            AllowsCompassTruth: false)));

        AssertRefused(summary, "witness-summary-replacement-scope-refused");
    }

    [Fact]
    public void Summary_Does_Not_Increment_Passage_Count()
    {
        var summary = Summarize(CreateRequest(priorPassageCount: 17));

        AssertColdSummary(summary);
        Assert.Equal(17, summary.PriorPassageCount);
        Assert.Equal(17, summary.PassageCountAfterSummary);
        Assert.False(summary.NonReplacementBoundary.IncrementsPassageCount);
    }

    private static WitnessSummaryReceipt Summarize(WitnessSummaryRequest request) =>
        new DefaultWitnessSummaryBoundaryValidator().Summarize(request, TimestampUtc);

    private static WitnessSummaryRequest CreateRequest(
        ReceiptSelectionReceipt? selectionReceipt = null,
        IReadOnlyList<WitnessSummaryArtifactLineage>? artifactLineage = null,
        SummaryWitnessContext? witnessContext = null,
        WitnessSummaryScopeBoundary? scopeBoundary = null,
        decimal confidenceEstimate = 0.86m,
        int priorPassageCount = 1,
        bool omitSelectionReceipt = false) =>
        new(
            SummaryHandle: $"witness-summary://{Guid.NewGuid():N}",
            SelectionReceipt: omitSelectionReceipt
                ? null
                : selectionReceipt ?? CreateSelectionReceipt(),
            ArtifactLineage: artifactLineage ?? CreateArtifactLineage(),
            DoctrinePhrases:
            [
                new("Summary may compress evidence. Summary may not replace evidence.", "summary-non-replacement-ledger", true),
                new("Selection may nominate evidence for review. Selection may not admit evidence into continuity.", "selection-non-admission-ledger", true),
                new("Query may locate evidence. Query may not manufacture warrant.", "query-non-warrant-ledger", true)
            ],
            GapCandidates:
            [
                new("persistent-receipt-store", "planned", false),
                new("compass-evidence-handoff", "planned", false)
            ],
            WitnessContext: witnessContext ?? new SummaryWitnessContext(
                WitnessSurface: SanctuaryPacketSurfaces.Steward,
                WitnessPresent: true,
                SeparateCustody: true),
            ScopeBoundary: scopeBoundary ?? new WitnessSummaryScopeBoundary(
                ScopeCode: "review-only",
                Present: true,
                ReviewOnly: true,
                AllowsAuthority: false,
                AllowsContinuityAdmission: false,
                AllowsEvidenceReplacement: false,
                AllowsCompassTruth: false),
            ConfidenceEstimate: confidenceEstimate,
            PriorPassageCount: priorPassageCount);

    private static IReadOnlyList<WitnessSummaryArtifactLineage> CreateArtifactLineage() =>
    [
        Artifact("packet-membrane-validation-matrix", "packet-membrane.contract-validation", "PacketMembraneContractValidation", "packet-membrane"),
        Artifact("packet-non-authority-refusal-ledger", "packet-membrane.contract-validation", "PacketMembraneContractValidation", "packet-membrane"),
        Artifact("receipt-non-permission-ledger", "packet-membrane.receipt-routing", "PacketMembraneReceiptRouting", "packet-membrane"),
        Artifact("replay-non-reentry-ledger", "packet-membrane.receipt-replay-boundary", "PacketMembraneReceiptReplayBoundary", "packet-membrane"),
        Artifact("query-non-warrant-ledger", "packet-membrane.receipt-query-boundary", "PacketMembraneReceiptQueryBoundary", "packet-membrane"),
        Artifact("selection-non-admission-ledger", "packet-membrane.receipt-selection-boundary", "PacketMembraneReceiptSelectionBoundary", "packet-membrane"),
        Artifact("compass-shell-packet", "instrument.compass-shell", "InstrumentBodyHardening", "instrument-body"),
        Artifact("compass-candidate-only-refusal", "instrument.compass-shell", "InstrumentBodyHardening", "instrument-body")
    ];

    private static WitnessSummaryArtifactLineage Artifact(
        string artifactId,
        string cellId,
        string phase,
        string layer) =>
        new(
            ArtifactId: artifactId,
            CellId: cellId,
            Phase: phase,
            Layer: layer,
            SourcePath: $"receipts/spiral-build/cells/{artifactId}.json",
            Summary: $"review-only artifact lineage for {artifactId}",
            PreservesLineage: true);

    private static ReceiptSelectionReceipt CreateSelectionReceipt(IReadOnlyList<PacketReceiptRoutingReceipt>? retainedReceipts = null)
    {
        var query = new DefaultReceiptQueryBoundaryValidator().Query(
            new ReceiptQueryRequest(
                QueryHandle: $"receipt-query://{Guid.NewGuid():N}",
                RetainedReceipts: retainedReceipts ?? [CreateRoutingReceipt(CreatePacket())],
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

        return new DefaultReceiptSelectionBoundaryValidator().Select(
            new ReceiptSelectionRequest(
                SelectionHandle: $"receipt-selection://{Guid.NewGuid():N}",
                QueryReceipt: query,
                RequestedOriginalReceiptHandles: [],
                WitnessContext: new SelectionWitnessContext(
                    WitnessSurface: SanctuaryPacketSurfaces.Steward,
                    WitnessPresent: true,
                    SeparateCustody: true),
                ScopeBoundary: new ReceiptSelectionScopeBoundary(
                    ScopeCode: "review-only",
                    Present: true,
                    ReviewOnly: true,
                    AllowsAuthority: false,
                    AllowsContinuityAdmission: false,
                    AllowsCompassTruth: false),
                PriorPassageCount: 1),
            TimestampUtc);
    }

    private static PacketReceiptRoutingReceipt CreateRoutingReceipt(SanctuaryPacket packet)
    {
        var validationReceipt = new DefaultSanctuaryPacketMembraneValidator().Validate(packet, TimestampUtc);
        return new DefaultPacketReceiptRoutingValidator().Route(packet, validationReceipt, TimestampUtc);
    }

    private static SanctuaryPacket CreatePacket() =>
        new(
            PacketHandle: $"packet://{Guid.NewGuid():N}",
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

    private static void AssertColdSummary(WitnessSummaryReceipt summary)
    {
        Assert.True(summary.IsColdSummary);
        Assert.True(summary.ReviewOnly);
        Assert.Equal(summary.PriorPassageCount, summary.PassageCountAfterSummary);
        Assert.False(summary.SummaryReplacesEvidence);
        Assert.False(summary.SummaryGrantsAuthority);
        Assert.False(summary.SummaryAdmitsContinuity);
        Assert.False(summary.SummaryBecomesCompassTruth);
        Assert.False(summary.ReceiptsReplayed);
        Assert.False(summary.NewPacketEmitted);
        Assert.True(summary.ActivationRefused);
        Assert.False(summary.AuthorityGranted);
        Assert.False(summary.ContinuityAdmitted);
    }

    private static void AssertRefused(WitnessSummaryReceipt summary, string outcomeCode)
    {
        Assert.Equal(WitnessSummaryDisposition.Refused, summary.Disposition);
        Assert.Equal(outcomeCode, summary.OutcomeCode);
        Assert.True(summary.IsRetainedSummaryRefusal);
        Assert.NotNull(summary.Refusal);
        Assert.True(summary.Refusal!.Retained);
        Assert.Empty(summary.Groups);
        Assert.False(summary.SummaryReplacesEvidence);
        Assert.False(summary.SummaryGrantsAuthority);
        Assert.False(summary.SummaryAdmitsContinuity);
        Assert.False(summary.SummaryBecomesCompassTruth);
        Assert.False(summary.ReceiptsReplayed);
        Assert.False(summary.NewPacketEmitted);
        Assert.True(summary.ActivationRefused);
        Assert.False(summary.AuthorityGranted);
        Assert.False(summary.ContinuityAdmitted);
    }
}
