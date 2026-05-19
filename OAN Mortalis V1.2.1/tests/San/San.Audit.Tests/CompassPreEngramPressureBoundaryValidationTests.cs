using San.Common;
using Xunit;

namespace San.Audit.Tests;

public sealed class CompassPreEngramPressureBoundaryValidationTests
{
    private static readonly DateTimeOffset TimestampUtc = DateTimeOffset.Parse("2026-05-12T00:00:00Z");

    [Fact]
    public void Pressure_Accepts_Cold_Witness_Summary_As_Candidate()
    {
        var pressure = Pressurize(CreateRequest());

        AssertColdPressure(pressure);
        Assert.Equal(CompassPressureDisposition.PressurizedForReviewCold, pressure.Disposition);
        Assert.Equal("compass-pressure-candidate-review-only", pressure.OutcomeCode);
        Assert.True(pressure.CandidateOnly);
        Assert.NotNull(pressure.Residue);
        Assert.True(pressure.Residue!.CandidateOnly);
    }

    [Fact]
    public void Pressure_Preserves_Summary_And_Artifact_Lineage()
    {
        var summary = CreateSummaryReceipt();

        var pressure = Pressurize(CreateRequest(summaryReceipt: summary));

        AssertColdPressure(pressure);
        Assert.Equal(summary.ReceiptHandle, pressure.SummaryReceiptHandle);
        Assert.Equal(summary.ReceiptHandle, pressure.Residue!.SummaryReceiptHandle);
        Assert.Equal(summary.SelectionReceiptHandle, pressure.Residue.SelectionReceiptHandle);
        Assert.All(pressure.Residue.ArtifactLineage, artifact => Assert.True(artifact.PreservesLineage));
        Assert.Contains(pressure.Residue.ArtifactLineage, artifact =>
            artifact.ArtifactId == "summary-non-replacement-ledger" &&
            artifact.CellId == "witness.summary-boundary");
    }

    [Fact]
    public void Pressure_Preserves_Original_Receipt_Handles()
    {
        var summary = CreateSummaryReceipt();

        var pressure = Pressurize(CreateRequest(summaryReceipt: summary));

        AssertColdPressure(pressure);
        var originalHandles = summary.Groups
            .SelectMany(static group => group.OriginalReceiptHandles)
            .Distinct(StringComparer.Ordinal)
            .ToArray();
        Assert.Equal(originalHandles, pressure.Residue!.OriginalReceiptHandles);
    }

    [Fact]
    public void Pressure_Cannot_Become_Engram()
    {
        var pressure = Pressurize(CreateRequest());

        AssertColdPressure(pressure);
        Assert.False(pressure.PressureBecomesEngram);
        Assert.False(pressure.Residue!.EngramAdmitted);
        Assert.False(pressure.NonEngramBoundary.PressureMayBecomeEngram);
    }

    [Fact]
    public void Pressure_Cannot_Become_Truth()
    {
        var pressure = Pressurize(CreateRequest());

        AssertColdPressure(pressure);
        Assert.False(pressure.PressureBecomesTruth);
        Assert.False(pressure.NonEngramBoundary.PressureMayBecomeTruth);
    }

    [Fact]
    public void Pressure_Cannot_Authorize()
    {
        var pressure = Pressurize(CreateRequest());

        AssertColdPressure(pressure);
        Assert.False(pressure.PressureGrantsAuthority);
        Assert.False(pressure.AuthorityGranted);
        Assert.False(pressure.NonEngramBoundary.PressureMayAuthorize);
    }

    [Fact]
    public void Pressure_Cannot_Admit_Continuity()
    {
        var pressure = Pressurize(CreateRequest());

        AssertColdPressure(pressure);
        Assert.False(pressure.PressureAdmitsContinuity);
        Assert.False(pressure.ContinuityAdmitted);
        Assert.False(pressure.NonEngramBoundary.PressureMayAdmitContinuity);
    }

    [Fact]
    public void Pressure_Cannot_Append_SelfGEL()
    {
        var pressure = Pressurize(CreateRequest());

        AssertColdPressure(pressure);
        Assert.False(pressure.PressureAppendsSelfGel);
        Assert.False(pressure.Residue!.SelfGelAppendAllowed);
        Assert.False(pressure.NonEngramBoundary.PressureMayAppendSelfGel);
    }

    [Fact]
    public void Pressure_Cannot_Append_CSelfGEL()
    {
        var pressure = Pressurize(CreateRequest());

        AssertColdPressure(pressure);
        Assert.False(pressure.PressureAppendsCSelfGel);
        Assert.False(pressure.Residue!.CSelfGelAppendAllowed);
        Assert.False(pressure.NonEngramBoundary.PressureMayAppendCSelfGel);
    }

    [Fact]
    public void Pressure_Does_Not_Replay_Receipts()
    {
        var pressure = Pressurize(CreateRequest());

        AssertColdPressure(pressure);
        Assert.False(pressure.ReceiptsReplayed);
        Assert.False(pressure.NonEngramBoundary.PressureReplaysReceipts);
    }

    [Fact]
    public void Pressure_Does_Not_Emit_New_Packet()
    {
        var pressure = Pressurize(CreateRequest());

        AssertColdPressure(pressure);
        Assert.False(pressure.NewPacketEmitted);
        Assert.False(pressure.NonEngramBoundary.EmitsNewPacket);
    }

    [Fact]
    public void Pressure_Does_Not_Increment_Passage_Count()
    {
        var pressure = Pressurize(CreateRequest(priorPassageCount: 21));

        AssertColdPressure(pressure);
        Assert.Equal(21, pressure.PriorPassageCount);
        Assert.Equal(21, pressure.PassageCountAfterPressure);
        Assert.False(pressure.NonEngramBoundary.IncrementsPassageCount);
    }

    [Fact]
    public void Pressure_Requires_Summary_Source()
    {
        var pressure = Pressurize(CreateRequest(omitSummaryReceipt: true));

        AssertRefused(pressure, "compass-pressure-summary-receipt-missing");
    }

    [Fact]
    public void Pressure_Requires_Cold_Summary_Source()
    {
        var refusedSummary = new DefaultWitnessSummaryBoundaryValidator().Summarize(
            CreateSummaryRequest(omitSelectionReceipt: true),
            TimestampUtc);

        var pressure = Pressurize(CreateRequest(summaryReceipt: refusedSummary));

        AssertRefused(pressure, "compass-pressure-summary-not-cold-review");
    }

    [Fact]
    public void Pressure_Requires_Scope_Boundary()
    {
        var pressure = Pressurize(CreateRequest(scopeBoundary: new CompassPressureScopeBoundary(
            ScopeCode: string.Empty,
            Present: false,
            ReviewOnly: true,
            AllowsEngram: false,
            AllowsTruth: false,
            AllowsAuthority: false,
            AllowsContinuityAdmission: false,
            AllowsSelfGelAppend: false,
            AllowsCSelfGelAppend: false)));

        AssertRefused(pressure, "compass-pressure-scope-boundary-missing");
    }

    [Fact]
    public void Pressure_Requires_Witness_Context()
    {
        var pressure = Pressurize(CreateRequest(witnessContext: new CompassPressureWitnessContext(
            WitnessSurface: string.Empty,
            WitnessPresent: false,
            SeparateCustody: false)));

        AssertRefused(pressure, "compass-pressure-witness-context-missing");
    }

    [Fact]
    public void Pressure_Refuses_Engram_Admitting_Scope()
    {
        var pressure = Pressurize(CreateRequest(scopeBoundary: new CompassPressureScopeBoundary(
            ScopeCode: "review-plus-engram",
            Present: true,
            ReviewOnly: true,
            AllowsEngram: true,
            AllowsTruth: false,
            AllowsAuthority: false,
            AllowsContinuityAdmission: false,
            AllowsSelfGelAppend: false,
            AllowsCSelfGelAppend: false)));

        AssertRefused(pressure, "compass-pressure-engram-scope-refused");
    }

    [Theory]
    [InlineData(false, true, false, false, false, false)]
    [InlineData(false, false, true, false, false, false)]
    [InlineData(false, false, false, true, false, false)]
    [InlineData(false, false, false, false, true, false)]
    [InlineData(false, false, false, false, false, true)]
    [InlineData(true, false, false, false, false, false)]
    public void Pressure_Refuses_All_Promotion_Scope_Variants(
        bool allowsTruth,
        bool allowsAuthority,
        bool allowsContinuityAdmission,
        bool allowsSelfGelAppend,
        bool allowsCSelfGelAppend,
        bool notReviewOnly)
    {
        var pressure = Pressurize(CreateRequest(scopeBoundary: new CompassPressureScopeBoundary(
            ScopeCode: "promotion-scope",
            Present: true,
            ReviewOnly: !notReviewOnly,
            AllowsEngram: false,
            AllowsTruth: allowsTruth,
            AllowsAuthority: allowsAuthority,
            AllowsContinuityAdmission: allowsContinuityAdmission,
            AllowsSelfGelAppend: allowsSelfGelAppend,
            AllowsCSelfGelAppend: allowsCSelfGelAppend)));

        AssertRefused(pressure, "compass-pressure-engram-scope-refused");
    }

    [Fact]
    public void Pressure_Returns_Bounded_Pressure_Vector()
    {
        var pressure = Pressurize(CreateRequest());

        AssertColdPressure(pressure);
        Assert.True(pressure.PressureVector.Bounded);
        Assert.True(pressure.PressureVector.EvidenceDensity is >= 0m and <= 1m);
        Assert.True(pressure.PressureVector.DoctrinePressure is >= 0m and <= 1m);
        Assert.True(pressure.PressureVector.GapPressure is >= 0m and <= 1m);
        Assert.True(pressure.PressureVector.Confidence is >= 0m and <= 1m);
    }

    [Fact]
    public void Empty_Pressure_Returns_Reviewable_Empty_Result()
    {
        var emptySelection = CreateSelectionReceipt(retainedReceipts: []);
        var emptySummary = CreateSummaryReceipt(selectionReceipt: emptySelection, artifactLineage: []);

        var pressure = Pressurize(CreateRequest(summaryReceipt: emptySummary));

        AssertColdPressure(pressure);
        Assert.Equal(CompassPressureDisposition.EmptyReviewCold, pressure.Disposition);
        Assert.Equal("compass-pressure-empty-review-only", pressure.OutcomeCode);
        Assert.Empty(pressure.Residue!.OriginalReceiptHandles);
        Assert.Empty(pressure.Residue.ArtifactLineage);
    }

    private static CompassPressureReceipt Pressurize(CompassPressureRequest request) =>
        new DefaultCompassPreEngramPressureBoundaryValidator().Pressurize(request, TimestampUtc);

    private static CompassPressureRequest CreateRequest(
        WitnessSummaryReceipt? summaryReceipt = null,
        CompassPressureWitnessContext? witnessContext = null,
        CompassPressureScopeBoundary? scopeBoundary = null,
        int priorPassageCount = 1,
        bool omitSummaryReceipt = false) =>
        new(
            PressureHandle: $"compass-pressure://{Guid.NewGuid():N}",
            SummaryReceipt: omitSummaryReceipt
                ? null
                : summaryReceipt ?? CreateSummaryReceipt(),
            WitnessContext: witnessContext ?? new CompassPressureWitnessContext(
                WitnessSurface: SanctuaryPacketSurfaces.Steward,
                WitnessPresent: true,
                SeparateCustody: true),
            ScopeBoundary: scopeBoundary ?? new CompassPressureScopeBoundary(
                ScopeCode: "review-only",
                Present: true,
                ReviewOnly: true,
                AllowsEngram: false,
                AllowsTruth: false,
                AllowsAuthority: false,
                AllowsContinuityAdmission: false,
                AllowsSelfGelAppend: false,
                AllowsCSelfGelAppend: false),
            PriorPassageCount: priorPassageCount);

    private static WitnessSummaryReceipt CreateSummaryReceipt(
        ReceiptSelectionReceipt? selectionReceipt = null,
        IReadOnlyList<WitnessSummaryArtifactLineage>? artifactLineage = null) =>
        new DefaultWitnessSummaryBoundaryValidator().Summarize(
            CreateSummaryRequest(selectionReceipt: selectionReceipt, artifactLineage: artifactLineage),
            TimestampUtc);

    private static WitnessSummaryRequest CreateSummaryRequest(
        ReceiptSelectionReceipt? selectionReceipt = null,
        IReadOnlyList<WitnessSummaryArtifactLineage>? artifactLineage = null,
        bool omitSelectionReceipt = false) =>
        new(
            SummaryHandle: $"witness-summary://{Guid.NewGuid():N}",
            SelectionReceipt: omitSelectionReceipt
                ? null
                : selectionReceipt ?? CreateSelectionReceipt(),
            ArtifactLineage: artifactLineage ?? CreateArtifactLineage(),
            DoctrinePhrases:
            [
                new("Pre-engram residue may pressure Compass. Pre-engram residue may not become engram.", "pressure-non-engram-ledger", true),
                new("Summary may compress evidence. Summary may not replace evidence.", "summary-non-replacement-ledger", true),
                new("Selection may nominate evidence for review. Selection may not admit evidence into continuity.", "selection-non-admission-ledger", true)
            ],
            GapCandidates:
            [
                new("live-compass-handoff", "planned", false),
                new("persistent-pressure-store", "planned", false)
            ],
            WitnessContext: new SummaryWitnessContext(
                WitnessSurface: SanctuaryPacketSurfaces.Steward,
                WitnessPresent: true,
                SeparateCustody: true),
            ScopeBoundary: new WitnessSummaryScopeBoundary(
                ScopeCode: "review-only",
                Present: true,
                ReviewOnly: true,
                AllowsAuthority: false,
                AllowsContinuityAdmission: false,
                AllowsEvidenceReplacement: false,
                AllowsCompassTruth: false),
            ConfidenceEstimate: 0.86m,
            PriorPassageCount: 1);

    private static IReadOnlyList<WitnessSummaryArtifactLineage> CreateArtifactLineage() =>
    [
        Artifact("packet-membrane-validation-matrix", "packet-membrane.contract-validation", "PacketMembraneContractValidation", "packet-membrane"),
        Artifact("receipt-non-permission-ledger", "packet-membrane.receipt-routing", "PacketMembraneReceiptRouting", "packet-membrane"),
        Artifact("replay-non-reentry-ledger", "packet-membrane.receipt-replay-boundary", "PacketMembraneReceiptReplayBoundary", "packet-membrane"),
        Artifact("query-non-warrant-ledger", "packet-membrane.receipt-query-boundary", "PacketMembraneReceiptQueryBoundary", "packet-membrane"),
        Artifact("selection-non-admission-ledger", "packet-membrane.receipt-selection-boundary", "PacketMembraneReceiptSelectionBoundary", "packet-membrane"),
        Artifact("summary-non-replacement-ledger", "witness.summary-boundary", "WitnessSummaryBoundary", "witness"),
        Artifact("compass-shell-packet", "instrument.compass-shell", "InstrumentBodyHardening", "instrument-body")
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

    private static void AssertColdPressure(CompassPressureReceipt pressure)
    {
        Assert.True(pressure.IsColdPressure);
        Assert.True(pressure.ReviewOnly);
        Assert.True(pressure.CandidateOnly);
        Assert.Equal(pressure.PriorPassageCount, pressure.PassageCountAfterPressure);
        Assert.False(pressure.PressureBecomesEngram);
        Assert.False(pressure.PressureBecomesTruth);
        Assert.False(pressure.PressureGrantsAuthority);
        Assert.False(pressure.PressureAdmitsContinuity);
        Assert.False(pressure.PressureAppendsSelfGel);
        Assert.False(pressure.PressureAppendsCSelfGel);
        Assert.False(pressure.ReceiptsReplayed);
        Assert.False(pressure.NewPacketEmitted);
        Assert.True(pressure.ActivationRefused);
        Assert.False(pressure.AuthorityGranted);
        Assert.False(pressure.ContinuityAdmitted);
    }

    private static void AssertRefused(CompassPressureReceipt pressure, string outcomeCode)
    {
        Assert.Equal(CompassPressureDisposition.Refused, pressure.Disposition);
        Assert.Equal(outcomeCode, pressure.OutcomeCode);
        Assert.True(pressure.IsRetainedPressureRefusal);
        Assert.NotNull(pressure.Refusal);
        Assert.True(pressure.Refusal!.Retained);
        Assert.Null(pressure.Residue);
        Assert.False(pressure.PressureBecomesEngram);
        Assert.False(pressure.PressureBecomesTruth);
        Assert.False(pressure.PressureGrantsAuthority);
        Assert.False(pressure.PressureAdmitsContinuity);
        Assert.False(pressure.PressureAppendsSelfGel);
        Assert.False(pressure.PressureAppendsCSelfGel);
        Assert.False(pressure.ReceiptsReplayed);
        Assert.False(pressure.NewPacketEmitted);
        Assert.True(pressure.ActivationRefused);
        Assert.False(pressure.AuthorityGranted);
        Assert.False(pressure.ContinuityAdmitted);
    }
}
