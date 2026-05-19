using San.Common;
using Xunit;

namespace San.Audit.Tests;

public sealed class EngramCandidatePreconditionBoundaryValidationTests
{
    private static readonly DateTimeOffset TimestampUtc = DateTimeOffset.Parse("2026-05-12T00:00:00Z");

    [Fact]
    public void Candidate_Accepts_Cold_Pressure_As_Readiness_Nomination()
    {
        var candidate = Nominate(CreateRequest());

        AssertColdCandidate(candidate);
        Assert.Equal(EngramCandidateDisposition.NominatedForReviewCold, candidate.Disposition);
        Assert.Equal("engram-candidate-readiness-nominated-review-only", candidate.OutcomeCode);
        Assert.True(candidate.CandidateNominated);
        Assert.NotNull(candidate.EvidenceLineage);
        Assert.True(candidate.EvidenceLineage!.CandidateOnly);
    }

    [Fact]
    public void Candidate_Preserves_Pressure_Source_Handles_And_Artifact_Lineage()
    {
        var pressure = CreatePressureReceipt();

        var candidate = Nominate(CreateRequest(pressureReceipt: pressure));

        AssertColdCandidate(candidate);
        Assert.Equal(pressure.ReceiptHandle, candidate.PressureReceiptHandle);
        Assert.Equal(pressure.ReceiptHandle, candidate.EvidenceLineage!.PressureReceiptHandle);
        Assert.Equal(pressure.Residue!.SummaryReceiptHandle, candidate.EvidenceLineage.SummaryReceiptHandle);
        Assert.Equal(pressure.Residue.SelectionReceiptHandle, candidate.EvidenceLineage.SelectionReceiptHandle);
        Assert.Equal(pressure.Residue.OriginalReceiptHandles, candidate.EvidenceLineage.OriginalReceiptHandles);
        Assert.All(candidate.EvidenceLineage.ArtifactLineage, artifact => Assert.True(artifact.PreservesLineage));
    }

    [Fact]
    public void Candidate_Preserves_Source_Preconditions()
    {
        var sourceBoundary = CreateSourceBoundary();

        var candidate = Nominate(CreateRequest(sourceBoundary: sourceBoundary));

        AssertColdCandidate(candidate);
        Assert.Equal(sourceBoundary.MembraneLandingCode, candidate.EvidenceLineage!.SourceBoundary.MembraneLandingCode);
        Assert.Equal(sourceBoundary.ClassificationCode, candidate.EvidenceLineage.SourceBoundary.ClassificationCode);
        Assert.Equal(sourceBoundary.TransformationTrace, candidate.EvidenceLineage.SourceBoundary.TransformationTrace);
        Assert.Equal(sourceBoundary.ContinuityRelationCode, candidate.EvidenceLineage.SourceBoundary.ContinuityRelationCode);
    }

    [Fact]
    public void Candidate_Cannot_Become_Engram()
    {
        var candidate = Nominate(CreateRequest());

        AssertColdCandidate(candidate);
        Assert.False(candidate.CandidateBecomesEngram);
        Assert.False(candidate.NonAdmissionBoundary.CandidateMayBecomeEngram);
    }

    [Fact]
    public void Candidate_Cannot_Admit_Continuity()
    {
        var candidate = Nominate(CreateRequest());

        AssertColdCandidate(candidate);
        Assert.False(candidate.CandidateAdmitsContinuity);
        Assert.False(candidate.ContinuityAdmitted);
        Assert.False(candidate.NonAdmissionBoundary.CandidateMayAdmitContinuity);
    }

    [Fact]
    public void Candidate_Cannot_Authorize()
    {
        var candidate = Nominate(CreateRequest());

        AssertColdCandidate(candidate);
        Assert.False(candidate.CandidateGrantsAuthority);
        Assert.False(candidate.AuthorityGranted);
        Assert.False(candidate.NonAdmissionBoundary.CandidateMayAuthorize);
    }

    [Fact]
    public void Candidate_Cannot_Append_SelfGEL_Or_CSelfGEL()
    {
        var candidate = Nominate(CreateRequest());

        AssertColdCandidate(candidate);
        Assert.False(candidate.CandidateAppendsSelfGel);
        Assert.False(candidate.CandidateAppendsCSelfGel);
        Assert.False(candidate.NonAdmissionBoundary.CandidateMayAppendSelfGel);
        Assert.False(candidate.NonAdmissionBoundary.CandidateMayAppendCSelfGel);
    }

    [Fact]
    public void Candidate_Does_Not_Replace_Evidence_Replay_Emit_Or_Increment()
    {
        var candidate = Nominate(CreateRequest(priorPassageCount: 34));

        AssertColdCandidate(candidate);
        Assert.Equal(34, candidate.PriorPassageCount);
        Assert.Equal(34, candidate.PassageCountAfterCandidate);
        Assert.False(candidate.CandidateReplacesEvidence);
        Assert.False(candidate.ReceiptsReplayed);
        Assert.False(candidate.NewPacketEmitted);
        Assert.False(candidate.NonAdmissionBoundary.CandidateMayReplaceEvidence);
        Assert.False(candidate.NonAdmissionBoundary.CandidateReplaysReceipts);
        Assert.False(candidate.NonAdmissionBoundary.IncrementsPassageCount);
        Assert.False(candidate.NonAdmissionBoundary.EmitsNewPacket);
    }

    [Fact]
    public void Candidate_Requires_Pressure_Source()
    {
        var candidate = Nominate(CreateRequest(omitPressureReceipt: true));

        AssertRefused(candidate, "engram-candidate-pressure-receipt-missing");
    }

    [Fact]
    public void Candidate_Requires_Cold_Pressure_Source()
    {
        var refusedPressure = new DefaultCompassPreEngramPressureBoundaryValidator().Pressurize(
            CreatePressureRequest(omitSummaryReceipt: true),
            TimestampUtc);

        var candidate = Nominate(CreateRequest(pressureReceipt: refusedPressure));

        AssertRefused(candidate, "engram-candidate-pressure-not-cold-review");
    }

    [Fact]
    public void Candidate_Requires_Membrane_Landing()
    {
        var candidate = Nominate(CreateRequest(sourceBoundary: CreateSourceBoundary(
            membraneLandingCode: string.Empty,
            membraneLandingPresent: false)));

        AssertRefused(candidate, "engram-candidate-membrane-landing-missing");
    }

    [Fact]
    public void Candidate_Requires_Classification()
    {
        var candidate = Nominate(CreateRequest(sourceBoundary: CreateSourceBoundary(
            classificationCode: string.Empty,
            classificationPresent: false)));

        AssertRefused(candidate, "engram-candidate-classification-missing");
    }

    [Fact]
    public void Candidate_Requires_Source_Evidence()
    {
        var candidate = Nominate(CreateRequest(sourceBoundary: CreateSourceBoundary(sourceEvidencePresent: false)));

        AssertRefused(candidate, "engram-candidate-source-evidence-missing");
    }

    [Fact]
    public void Candidate_Requires_Transformation_Trace()
    {
        var candidate = Nominate(CreateRequest(sourceBoundary: CreateSourceBoundary(
            transformationTrace: string.Empty,
            transformationTracePresent: false)));

        AssertRefused(candidate, "engram-candidate-transformation-trace-missing");
    }

    [Fact]
    public void Candidate_Requires_Continuity_Relation()
    {
        var candidate = Nominate(CreateRequest(sourceBoundary: CreateSourceBoundary(
            continuityRelationCode: string.Empty,
            continuityRelationPresent: false)));

        AssertRefused(candidate, "engram-candidate-continuity-relation-missing");
    }

    [Fact]
    public void Candidate_Requires_Witness_Context()
    {
        var candidate = Nominate(CreateRequest(witnessContext: new EngramCandidateWitnessContext(
            WitnessSurface: string.Empty,
            WitnessPresent: false,
            SeparateCustody: false)));

        AssertRefused(candidate, "engram-candidate-witness-context-missing");
    }

    [Fact]
    public void Candidate_Requires_Scope_Boundary()
    {
        var candidate = Nominate(CreateRequest(scopeBoundary: new EngramCandidateScopeBoundary(
            ScopeCode: string.Empty,
            Present: false,
            ReviewOnly: true,
            AllowsEngramAdmission: false,
            AllowsContinuityAdmission: false,
            AllowsAuthority: false,
            AllowsSelfGelAppend: false,
            AllowsCSelfGelAppend: false,
            AllowsRuntimeAction: false)));

        AssertRefused(candidate, "engram-candidate-scope-boundary-missing");
    }

    [Theory]
    [InlineData(true, false, false, false, false, false, false)]
    [InlineData(false, true, false, false, false, false, false)]
    [InlineData(false, false, true, false, false, false, false)]
    [InlineData(false, false, false, true, false, false, false)]
    [InlineData(false, false, false, false, true, false, false)]
    [InlineData(false, false, false, false, false, true, false)]
    [InlineData(false, false, false, false, false, false, true)]
    public void Candidate_Refuses_All_Admission_Scope_Variants(
        bool allowsEngramAdmission,
        bool allowsContinuityAdmission,
        bool allowsAuthority,
        bool allowsSelfGelAppend,
        bool allowsCSelfGelAppend,
        bool allowsRuntimeAction,
        bool notReviewOnly)
    {
        var candidate = Nominate(CreateRequest(scopeBoundary: new EngramCandidateScopeBoundary(
            ScopeCode: "promotion-scope",
            Present: true,
            ReviewOnly: !notReviewOnly,
            AllowsEngramAdmission: allowsEngramAdmission,
            AllowsContinuityAdmission: allowsContinuityAdmission,
            AllowsAuthority: allowsAuthority,
            AllowsSelfGelAppend: allowsSelfGelAppend,
            AllowsCSelfGelAppend: allowsCSelfGelAppend,
            AllowsRuntimeAction: allowsRuntimeAction)));

        AssertRefused(candidate, "engram-candidate-admission-scope-refused");
    }

    private static EngramCandidateReadinessReceipt Nominate(EngramCandidateReadinessRequest request) =>
        new DefaultEngramCandidatePreconditionBoundaryValidator().Nominate(request, TimestampUtc);

    private static EngramCandidateReadinessRequest CreateRequest(
        CompassPressureReceipt? pressureReceipt = null,
        EngramCandidateSourceBoundary? sourceBoundary = null,
        EngramCandidateWitnessContext? witnessContext = null,
        EngramCandidateScopeBoundary? scopeBoundary = null,
        int priorPassageCount = 1,
        bool omitPressureReceipt = false) =>
        new(
            CandidateHandle: $"engram-candidate://{Guid.NewGuid():N}",
            PressureReceipt: omitPressureReceipt
                ? null
                : pressureReceipt ?? CreatePressureReceipt(),
            SourceBoundary: sourceBoundary ?? CreateSourceBoundary(),
            WitnessContext: witnessContext ?? new EngramCandidateWitnessContext(
                WitnessSurface: SanctuaryPacketSurfaces.Steward,
                WitnessPresent: true,
                SeparateCustody: true),
            ScopeBoundary: scopeBoundary ?? new EngramCandidateScopeBoundary(
                ScopeCode: "review-only",
                Present: true,
                ReviewOnly: true,
                AllowsEngramAdmission: false,
                AllowsContinuityAdmission: false,
                AllowsAuthority: false,
                AllowsSelfGelAppend: false,
                AllowsCSelfGelAppend: false,
                AllowsRuntimeAction: false),
            PriorPassageCount: priorPassageCount);

    private static EngramCandidateSourceBoundary CreateSourceBoundary(
        string membraneLandingCode = "membrane-landed:packet-selection-summary-pressure",
        bool membraneLandingPresent = true,
        string classificationCode = "classification:pre-engram-residue",
        bool classificationPresent = true,
        bool sourceEvidencePresent = true,
        string transformationTrace = "selection->summary->compass-pressure->pre-engram-residue",
        bool transformationTracePresent = true,
        string continuityRelationCode = "candidate-relation:review-only-non-admitted",
        bool continuityRelationPresent = true) =>
        new(
            MembraneLandingCode: membraneLandingCode,
            MembraneLandingPresent: membraneLandingPresent,
            ClassificationCode: classificationCode,
            ClassificationPresent: classificationPresent,
            SourceEvidencePresent: sourceEvidencePresent,
            TransformationTrace: transformationTrace,
            TransformationTracePresent: transformationTracePresent,
            ContinuityRelationCode: continuityRelationCode,
            ContinuityRelationPresent: continuityRelationPresent);

    private static CompassPressureReceipt CreatePressureReceipt() =>
        new DefaultCompassPreEngramPressureBoundaryValidator().Pressurize(CreatePressureRequest(), TimestampUtc);

    private static CompassPressureRequest CreatePressureRequest(bool omitSummaryReceipt = false) =>
        new(
            PressureHandle: $"compass-pressure://{Guid.NewGuid():N}",
            SummaryReceipt: omitSummaryReceipt ? null : CreateSummaryReceipt(),
            WitnessContext: new CompassPressureWitnessContext(
                WitnessSurface: SanctuaryPacketSurfaces.Steward,
                WitnessPresent: true,
                SeparateCustody: true),
            ScopeBoundary: new CompassPressureScopeBoundary(
                ScopeCode: "review-only",
                Present: true,
                ReviewOnly: true,
                AllowsEngram: false,
                AllowsTruth: false,
                AllowsAuthority: false,
                AllowsContinuityAdmission: false,
                AllowsSelfGelAppend: false,
                AllowsCSelfGelAppend: false),
            PriorPassageCount: 1);

    private static WitnessSummaryReceipt CreateSummaryReceipt() =>
        new DefaultWitnessSummaryBoundaryValidator().Summarize(CreateSummaryRequest(), TimestampUtc);

    private static WitnessSummaryRequest CreateSummaryRequest() =>
        new(
            SummaryHandle: $"witness-summary://{Guid.NewGuid():N}",
            SelectionReceipt: CreateSelectionReceipt(),
            ArtifactLineage: CreateArtifactLineage(),
            DoctrinePhrases:
            [
                new("Pre-engram residue may pressure Compass. Pre-engram residue may not become engram.", "pressure-non-engram-ledger", true),
                new("Engram candidate readiness may nominate residue. Engram candidate readiness may not admit continuity.", "engram-candidate-precondition-map", true)
            ],
            GapCandidates:
            [
                new("persistent-witness-store", "planned", false),
                new("compass-steward-handoff", "planned", false)
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
        Artifact("receipt-selection-boundary-matrix", "packet-membrane.receipt-selection-boundary", "PacketMembraneReceiptSelectionBoundary", "packet-membrane"),
        Artifact("summary-non-replacement-ledger", "witness.summary-boundary", "WitnessSummaryBoundary", "witness"),
        Artifact("pressure-non-engram-ledger", "compass.pre-engram-pressure-boundary", "CompassPreEngramPressureBoundary", "compass")
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

    private static ReceiptSelectionReceipt CreateSelectionReceipt()
    {
        var query = new DefaultReceiptQueryBoundaryValidator().Query(
            new ReceiptQueryRequest(
                QueryHandle: $"receipt-query://{Guid.NewGuid():N}",
                RetainedReceipts: [CreateRoutingReceipt(CreatePacket())],
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

    private static void AssertColdCandidate(EngramCandidateReadinessReceipt candidate)
    {
        Assert.True(candidate.IsColdCandidate);
        Assert.True(candidate.ReviewOnly);
        Assert.True(candidate.CandidateOnly);
        Assert.True(candidate.CandidateNominated);
        Assert.Equal(candidate.PriorPassageCount, candidate.PassageCountAfterCandidate);
        Assert.False(candidate.CandidateBecomesEngram);
        Assert.False(candidate.CandidateAdmitsContinuity);
        Assert.False(candidate.CandidateGrantsAuthority);
        Assert.False(candidate.CandidateAppendsSelfGel);
        Assert.False(candidate.CandidateAppendsCSelfGel);
        Assert.False(candidate.CandidateReplacesEvidence);
        Assert.False(candidate.ReceiptsReplayed);
        Assert.False(candidate.NewPacketEmitted);
        Assert.True(candidate.ActivationRefused);
        Assert.False(candidate.AuthorityGranted);
        Assert.False(candidate.ContinuityAdmitted);
    }

    private static void AssertRefused(EngramCandidateReadinessReceipt candidate, string outcomeCode)
    {
        Assert.Equal(EngramCandidateDisposition.Refused, candidate.Disposition);
        Assert.Equal(outcomeCode, candidate.OutcomeCode);
        Assert.True(candidate.IsRetainedCandidateRefusal);
        Assert.NotNull(candidate.Refusal);
        Assert.True(candidate.Refusal!.Retained);
        Assert.Null(candidate.EvidenceLineage);
        Assert.False(candidate.CandidateNominated);
        Assert.False(candidate.CandidateBecomesEngram);
        Assert.False(candidate.CandidateAdmitsContinuity);
        Assert.False(candidate.CandidateGrantsAuthority);
        Assert.False(candidate.CandidateAppendsSelfGel);
        Assert.False(candidate.CandidateAppendsCSelfGel);
        Assert.False(candidate.CandidateReplacesEvidence);
        Assert.False(candidate.ReceiptsReplayed);
        Assert.False(candidate.NewPacketEmitted);
        Assert.True(candidate.ActivationRefused);
        Assert.False(candidate.AuthorityGranted);
        Assert.False(candidate.ContinuityAdmitted);
    }
}
