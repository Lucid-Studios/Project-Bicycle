using San.Common;
using Xunit;

namespace San.Audit.Tests;

public sealed class HarmonicInterlockModulationCorrespondenceBoundaryValidationTests
{
    private static readonly DateTimeOffset TimestampUtc = DateTimeOffset.Parse("2026-05-14T00:00:00Z");

    [Fact]
    public void Atlas_Accepts_Disciplined_Selective_Correspondence()
    {
        var receipt = Declare(CreateRequest());

        Assert.Equal(HarmonicInterlockModulationCorrespondenceDisposition.AtlasReviewCold, receipt.Disposition);
        Assert.Equal("modulation-correspondence-atlas-review-only", receipt.OutcomeCode);
        Assert.Equal(2, receipt.Sources.Count);
        Assert.Equal(3, receipt.Concepts.Count);
        Assert.All(receipt.Sources.Select(static source => source.SourceHandle), handle =>
            Assert.Contains(handle, receipt.PreservedSourceHandles));
        Assert.All(receipt.Concepts.Select(static concept => concept.ConceptHandle), handle =>
            Assert.Contains(handle, receipt.PreservedConceptHandles));
        AssertCold(receipt);
    }

    [Fact]
    public void Empty_Atlas_Is_Reviewable_But_Not_Authoritative()
    {
        var receipt = Declare(CreateRequest(concepts: []));

        Assert.Equal(HarmonicInterlockModulationCorrespondenceDisposition.EmptyReviewCold, receipt.Disposition);
        Assert.Equal("modulation-correspondence-empty-review-only", receipt.OutcomeCode);
        Assert.Empty(receipt.Concepts);
        Assert.False(receipt.AuthorityGranted);
        Assert.False(receipt.ContinuityAdmitted);
        AssertCold(receipt);
    }

    [Theory]
    [InlineData("equivalence")]
    [InlineData("proof")]
    [InlineData("ontology")]
    [InlineData("imported-success")]
    [InlineData("channel-warrant")]
    [InlineData("transmission-admissibility")]
    [InlineData("sync-authority")]
    [InlineData("throughput-continuity")]
    [InlineData("persistence-continuity")]
    [InlineData("stability-truth")]
    public void Borrowed_Concept_Refuses_Collapse_Into_CME_Governance(string collapseCase)
    {
        var concepts = CreateConcepts();
        concepts[0] = collapseCase switch
        {
            "equivalence" => concepts[0] with { BorrowStructureNotAuthority = false },
            "proof" => concepts[0] with { BorrowAnalogyNotProof = false },
            "ontology" => concepts[0] with { BorrowMechanismNotOntology = false },
            "imported-success" => concepts[0] with { ImportedSuccessBecomesGovernanceCondition = true },
            "channel-warrant" => concepts[0] with { ChannelSuccessBecomesSemanticWarrant = true },
            "transmission-admissibility" => concepts[0] with { TransmissionBecomesAdmissibility = true },
            "sync-authority" => concepts[0] with { SynchronizationBecomesAuthority = true },
            "throughput-continuity" => concepts[0] with { ThroughputBecomesContinuity = true },
            "persistence-continuity" => concepts[0] with { PersistenceBecomesContinuity = true },
            "stability-truth" => concepts[0] with { StabilityBecomesTruth = true },
            _ => concepts[0]
        };

        var receipt = Declare(CreateRequest(concepts: concepts));

        AssertRefused(receipt, "modulation-correspondence-concept-collapse-refused");
    }

    [Fact]
    public void Source_Success_Condition_May_Not_Become_CME_Success()
    {
        var receipt = Declare(CreateRequest(
            translation: CreateTranslationBoundary(allowsSourceSuccessAsCmeSuccess: true)));

        AssertRefused(receipt, "modulation-correspondence-translation-boundary-promotional");
    }

    [Fact]
    public void Channel_Success_May_Not_Become_Semantic_Warrant()
    {
        var receipt = Declare(CreateRequest(
            translation: CreateTranslationBoundary(allowsChannelSuccessAsWarrant: true)));

        AssertRefused(receipt, "modulation-correspondence-translation-boundary-promotional");
    }

    [Fact]
    public void Actualization_Test_Must_Preserve_Goal_Custody_Witness_Revocation_And_Continuity_Safety()
    {
        var receipt = Declare(CreateRequest(
            actualization: CreateActualizationBoundary(preservesWitness: false)));

        AssertRefused(receipt, "modulation-correspondence-actualization-boundary-promotional");
    }

    [Fact]
    public void Actualization_Does_Not_Emit_Packet_Evaluate_Lisp_Replay_Activate_Or_Increment_Passage()
    {
        var receipt = Declare(CreateRequest(priorPassageCount: 222));

        Assert.Equal(222, receipt.PriorPassageCount);
        Assert.Equal(222, receipt.PassageCountAfterCorrespondenceReview);
        Assert.False(receipt.NewPacketEmitted);
        Assert.False(receipt.LispEvaluationAllowed);
        Assert.False(receipt.ReceiptsReplayed);
        Assert.True(receipt.ActivationRefused);
        AssertCold(receipt);
    }

    [Fact]
    public void Mature_Discipline_Source_May_Not_Claim_Proof_Transfer()
    {
        var sources = CreateSources();
        sources[0] = sources[0] with { ClaimsProofTransfer = true };

        var receipt = Declare(CreateRequest(sources: sources));

        AssertRefused(receipt, "modulation-correspondence-source-promotional-refused");
    }

    [Fact]
    public void Concept_Must_Bind_To_Declared_Source()
    {
        var concepts = CreateConcepts();
        concepts[0] = concepts[0] with { SourceHandle = "urn:san:mature-discipline-source:missing" };

        var receipt = Declare(CreateRequest(concepts: concepts));

        AssertRefused(receipt, "modulation-correspondence-concept-source-missing");
    }

    [Fact]
    public void Loss_Conditions_Are_Refusal_Boundaries_Not_Authority()
    {
        var losses = CreateLossConditions();
        losses[0] = losses[0] with { GrantsAuthority = true };

        var receipt = Declare(CreateRequest(lossConditions: losses));

        AssertRefused(receipt, "modulation-correspondence-loss-condition-promotional");
    }

    [Fact]
    public void Lisp_Body_Declares_Modulation_Correspondence_As_Inert_Carrier()
    {
        var lispPath = Path.Combine(
            FindRepositoryRoot(),
            "src",
            "SLI",
            "SLI.Lisp",
            "harmonic-interlock-modulation-correspondence.lisp");

        var body = File.ReadAllText(lispPath);

        Assert.Contains(":posture :cme-harmonic-interlock-modulation-correspondence-boundary", body, StringComparison.Ordinal);
        Assert.Contains(":lisp-role :disciplined-selective-correspondence-atlas", body, StringComparison.Ordinal);
        Assert.Contains(":core-invariant \"channel success is not semantic warrant\"", body, StringComparison.Ordinal);
        Assert.Contains(":borrow-structure-not-authority t", body, StringComparison.Ordinal);
        Assert.Contains(":borrow-analogy-not-proof t", body, StringComparison.Ordinal);
        Assert.Contains(":borrow-mechanism-not-ontology t", body, StringComparison.Ordinal);
        Assert.Contains(":correspondence-may-become-equivalence nil", body, StringComparison.Ordinal);
        Assert.Contains(":imported-success-may-become-governance-condition nil", body, StringComparison.Ordinal);
        Assert.Contains(":channel-success-may-become-semantic-warrant nil", body, StringComparison.Ordinal);
        Assert.Contains(":transmission-may-become-admissibility nil", body, StringComparison.Ordinal);
        Assert.Contains(":synchronization-may-become-authority nil", body, StringComparison.Ordinal);
        Assert.Contains(":throughput-may-become-continuity nil", body, StringComparison.Ordinal);
        Assert.Contains(":return :receipt-only", body, StringComparison.Ordinal);
        Assert.DoesNotContain("(eval", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("(compile", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("(load", body, StringComparison.OrdinalIgnoreCase);
    }

    private static HarmonicInterlockModulationCorrespondenceReceipt Declare(
        HarmonicInterlockModulationCorrespondenceRequest request) =>
        new DefaultHarmonicInterlockModulationCorrespondenceBoundaryValidator().Declare(request, TimestampUtc);

    private static HarmonicInterlockModulationCorrespondenceRequest CreateRequest(
        StewardHarmonicCustodyInterlockReceipt? source = null,
        IReadOnlyList<MatureDisciplineSource>? sources = null,
        IReadOnlyList<BorrowedCorrespondenceConcept>? concepts = null,
        CmeCorrespondenceTranslationBoundary? translation = null,
        CorrespondenceActualizationTestBoundary? actualization = null,
        IReadOnlyList<CorrespondenceLossCondition>? lossConditions = null,
        int priorPassageCount = 78) =>
        new(
            SourceInterlockReceipt: source ?? CreateSourceInterlock(),
            Sources: sources ?? CreateSources(),
            Concepts: concepts ?? CreateConcepts(),
            TranslationBoundary: translation ?? CreateTranslationBoundary(),
            ActualizationBoundary: actualization ?? CreateActualizationBoundary(),
            LossConditions: lossConditions ?? CreateLossConditions(),
            PriorPassageCount: priorPassageCount);

    private static StewardHarmonicCustodyInterlockReceipt CreateSourceInterlock()
    {
        var sourceResonance = "urn:san:cme-lisp-resonance-heartbeat:review:fixture";
        var sharedSurface = CreateSharedSurface();
        var signals = new[]
        {
            CreateSignal("prime", sourceResonance, sharedSurface.SurfaceHandle),
            CreateSignal("cryptic", sourceResonance, sharedSurface.SurfaceHandle)
        };

        return new StewardHarmonicCustodyInterlockReceipt(
            ReceiptHandle: "urn:san:steward-harmonic-interlock:review:fixture",
            Disposition: StewardHarmonicCustodyInterlockDisposition.SequenceReviewCold,
            OutcomeCode: "steward-harmonic-interlock-sequence-review-only",
            GovernanceTrace: "fixture cold Steward interlock",
            SourceResonanceReceiptHandle: sourceResonance,
            Signals: signals,
            SharedSurface: sharedSurface,
            HeartbeatWindow: new StewardInterlockHeartbeatWindow(
                WindowHandle: "urn:san:steward-heartbeat-window:fixture",
                StartOrdinal: 1,
                EndOrdinal: 2,
                StewardGoverned: true,
                Bounded: true,
                AllowsUngovernedCoexistence: false,
                AllowsBypass: false,
                AllowsPassageIncrement: false),
            Outcome: HarmonicInterlockOutcome.Sequence,
            CadencePolicy: new CadenceAlignmentPolicy(
                PolicyCode: "fixture-cadence",
                Present: true,
                CompatibleCadenceRequired: true,
                AllowsAlignmentToAdmit: false,
                AllowsAlignmentToAuthorize: false,
                AllowsUnwitnessedCoexistence: false),
            DampingPolicy: new DampingBackoffPolicy(
                PolicyCode: "fixture-damping",
                Present: true,
                DampingCoefficient: 0.40m,
                DampsWithoutErasure: true,
                AllowsWitnessErasure: false,
                AllowsAuthority: false,
                AllowsContinuity: false),
            SplitRoute: new WitnessSurfaceSplitRoute(
                RouteCode: "fixture-split",
                Present: true,
                PreservesCustody: true,
                PreservesOriginalSignalHandles: true,
                CreatesNewAuthoritySurface: false,
                FragmentsCustody: false,
                EmitsPackets: false),
            Boundary: new StewardInterlockNonAuthorityBoundary(
                BoundaryCode: "fixture-interlock-boundary",
                LocalLawfulnessMayImplySharedComposability: false,
                InterlockMayAuthorize: false,
                AlignmentMayAdmit: false,
                SequenceMayPunish: false,
                DampingMayEraseWitness: false,
                SplitMayFragmentCustody: false,
                CoolingMayMeanFailure: false,
                ContentionMayActivate: false,
                ReceiptMayPermit: false,
                StewardMayOwnMeaning: false,
                AllowsLispEvaluation: false,
                AllowsRuntimeAction: false,
                AllowsPacketEmission: false,
                AllowsReceiptReplay: false,
                AllowsPassageIncrement: false,
                AllowsContinuity: false,
                AllowsAuthority: false),
            ContentionReceipt: new SharedSurfaceContentionReceipt(
                ReceiptHandle: "urn:san:shared-surface-contention:fixture",
                SurfaceHandle: sharedSurface.SurfaceHandle,
                SignalHandles: signals.Select(static signal => signal.SignalHandle).ToArray(),
                Outcome: HarmonicInterlockOutcome.Sequence,
                Retained: true,
                ReviewOnly: true,
                EvidenceOnly: true,
                GrantsPermission: false,
                BecomesAuthority: false,
                AdmitsContinuity: false,
                ActivatesRuntime: false),
            Refusal: null,
            PriorPassageCount: 66,
            PassageCountAfterInterlockReview: 66,
            ReviewOnly: true,
            InertOnly: true,
            StewardInterlockPresent: true,
            LocalLawfulnessBecomesSharedComposability: false,
            InterlockGrantsAuthority: false,
            AlignmentAdmits: false,
            SequencePunishes: false,
            DampingErasesWitness: false,
            SplitFragmentsCustody: false,
            CoolingMeansFailure: false,
            ContentionActivates: false,
            ReceiptBecomesPermission: false,
            StewardOwnsMeaning: false,
            NewPacketEmitted: false,
            RuntimeActionAllowed: false,
            LispEvaluationAllowed: false,
            ContinuityAdmitted: false,
            ActivationRefused: true,
            TimestampUtc: TimestampUtc);
    }

    private static LawfulSignalCandidate CreateSignal(
        string name,
        string sourceResonance,
        string sharedSurface) =>
        new(
            SignalHandle: $"urn:san:lawful-signal:{name}",
            SourceReceiptHandle: sourceResonance,
            ThreadHandle: $"urn:san:cme-lisp-thread:{name}-001",
            SharedSurfaceHandle: sharedSurface,
            CadenceOrdinal: 1m,
            ResonanceAmplitude: 0.50m,
            SharedSurfacePressure: 0.42m,
            LocallyLawful: true,
            ReviewOnly: true,
            Inert: true,
            RequestsSharedSurface: true,
            EmitsPacket: false,
            RequestsRuntimeAction: false,
            ClaimsAuthority: false,
            ClaimsContinuity: false,
            RequestsActivation: false);

    private static SharedSymbolicSurface CreateSharedSurface() =>
        new(
            SurfaceHandle: "urn:san:shared-symbolic-surface:compass-worktable",
            SurfaceName: "CompassWorktable",
            CustodyOwner: "Steward",
            Shared: true,
            WitnessSurfacePresent: true,
            StewardInterlockRequired: true,
            DirectWriteAdmissionAllowed: false,
            GrantsAuthority: false,
            AdmitsContinuity: false,
            AllowsRuntimeAction: false,
            AllowsActivation: false);

    private static MatureDisciplineSource[] CreateSources() =>
    [
        new(
            SourceHandle: "urn:san:mature-discipline-source:network-scheduling",
            Domain: MatureDisciplineDomain.NetworkScheduling,
            SourceName: "network scheduling and backoff",
            SourceSuccessCondition: "avoid shared-channel contention",
            ReviewOnly: true,
            Inert: true,
            ClaimsEquivalence: false,
            ClaimsProofTransfer: false,
            ClaimsOntologyTransfer: false,
            ClaimsAuthority: false),
        new(
            SourceHandle: "urn:san:mature-discipline-source:signal-processing",
            Domain: MatureDisciplineDomain.SignalProcessing,
            SourceName: "signal damping and interference control",
            SourceSuccessCondition: "preserve signal fidelity",
            ReviewOnly: true,
            Inert: true,
            ClaimsEquivalence: false,
            ClaimsProofTransfer: false,
            ClaimsOntologyTransfer: false,
            ClaimsAuthority: false)
    ];

    private static BorrowedCorrespondenceConcept[] CreateConcepts() =>
    [
        CreateConcept(
            "backoff",
            "urn:san:mature-discipline-source:network-scheduling",
            "backoff protocol",
            "avoid shared-channel contention",
            "cool shared-surface pressure without erasing witness"),
        CreateConcept(
            "multiplex",
            "urn:san:mature-discipline-source:network-scheduling",
            "time-division multiplexing",
            "separate coexisting transmissions",
            "sequence symbolic voices without admitting authority"),
        CreateConcept(
            "damping",
            "urn:san:mature-discipline-source:signal-processing",
            "damping control",
            "reduce interference and preserve fidelity",
            "reduce harmonic contention without granting semantic warrant")
    ];

    private static BorrowedCorrespondenceConcept CreateConcept(
        string name,
        string source,
        string conceptName,
        string sourceSuccess,
        string translation) =>
        new(
            ConceptHandle: $"urn:san:borrowed-correspondence:{name}",
            SourceHandle: source,
            ConceptName: conceptName,
            SourceDomainSuccessCondition: sourceSuccess,
            CmeTranslation: translation,
            ExplicitNonClaim: "structural correspondence only; no equivalence, proof transfer, ontology transfer, authority, or warrant",
            ActualizationTest: "preserve intended goal, custody, witness, revocation, and continuity safety",
            LossConditions:
            [
                "meaning -> transmission",
                "authority -> successful propagation",
                "continuity -> persistence"
            ],
            BorrowStructureNotAuthority: true,
            BorrowAnalogyNotProof: true,
            BorrowMechanismNotOntology: true,
            ReGovernedUnderCmeLaw: true,
            ChannelSuccessBecomesSemanticWarrant: false,
            TransmissionBecomesAdmissibility: false,
            SynchronizationBecomesAuthority: false,
            ThroughputBecomesContinuity: false,
            PersistenceBecomesContinuity: false,
            StabilityBecomesTruth: false,
            ImportedSuccessBecomesGovernanceCondition: false);

    private static CmeCorrespondenceTranslationBoundary CreateTranslationBoundary(
        bool allowsSourceSuccessAsCmeSuccess = false,
        bool allowsChannelSuccessAsWarrant = false) =>
        new(
            BoundaryCode: "cme-correspondence-translation-boundary",
            Present: true,
            SemanticCustodyRequired: true,
            WitnessBurdenRequired: true,
            AuthorityCeilingRequired: true,
            ContinuityRiskRequired: true,
            RevocationPathRequired: true,
            ExplicitNonClaimRequired: true,
            AllowsEquivalenceClaim: false,
            AllowsProofTransfer: false,
            AllowsOntologyTransfer: false,
            AllowsSourceSuccessAsCmeSuccess: allowsSourceSuccessAsCmeSuccess,
            AllowsChannelSuccessAsWarrant: allowsChannelSuccessAsWarrant);

    private static CorrespondenceActualizationTestBoundary CreateActualizationBoundary(
        bool preservesWitness = true) =>
        new(
            BoundaryCode: "correspondence-actualization-test-boundary",
            Present: true,
            PreservesIntendedGoal: true,
            PreservesCustody: true,
            PreservesWitness: preservesWitness,
            PreservesRevocation: true,
            PreservesContinuitySafety: true,
            RefusesAuthorityLaundering: true,
            RefusesSemanticWarrantFromPropagation: true,
            AllowsRuntimeAction: false,
            AllowsLispEvaluation: false,
            AllowsPacketEmission: false,
            AllowsReceiptReplay: false,
            AllowsPassageIncrement: false,
            AllowsContinuityAdmission: false,
            AllowsAuthority: false);

    private static CorrespondenceLossCondition[] CreateLossConditions() =>
    [
        CreateLoss("meaning-transmission", "meaning -> transmission"),
        CreateLoss("authority-propagation", "authority -> successful propagation"),
        CreateLoss("continuity-persistence", "continuity -> persistence")
    ];

    private static CorrespondenceLossCondition CreateLoss(
        string name,
        string forbiddenCollapse) =>
        new(
            LossHandle: $"urn:san:correspondence-loss:{name}",
            ForbiddenCollapse: forbiddenCollapse,
            Refused: true,
            RetainedForReview: true,
            GrantsAuthority: false,
            AdmitsContinuity: false,
            ActivatesRuntime: false);

    private static void AssertCold(HarmonicInterlockModulationCorrespondenceReceipt receipt)
    {
        Assert.True(receipt.IsColdCorrespondenceAtlas);
        Assert.True(receipt.ReviewOnly);
        Assert.True(receipt.InertOnly);
        Assert.True(receipt.ActivationRefused);
        Assert.False(receipt.CorrespondenceBecomesEquivalence);
        Assert.False(receipt.BorrowedAnalogyBecomesProof);
        Assert.False(receipt.BorrowedMechanismBecomesOntology);
        Assert.False(receipt.ImportedSuccessBecomesGovernanceCondition);
        Assert.False(receipt.ChannelSuccessBecomesSemanticWarrant);
        Assert.False(receipt.TransmissionBecomesAdmissibility);
        Assert.False(receipt.SynchronizationBecomesAuthority);
        Assert.False(receipt.ThroughputBecomesContinuity);
        Assert.False(receipt.NewPacketEmitted);
        Assert.False(receipt.ReceiptsReplayed);
        Assert.False(receipt.AuthorityGranted);
        Assert.False(receipt.ContinuityAdmitted);
        Assert.Null(receipt.Refusal);
    }

    private static void AssertRefused(
        HarmonicInterlockModulationCorrespondenceReceipt receipt,
        string outcomeCode)
    {
        Assert.Equal(HarmonicInterlockModulationCorrespondenceDisposition.Refused, receipt.Disposition);
        Assert.Equal(outcomeCode, receipt.OutcomeCode);
        Assert.True(receipt.IsRetainedCorrespondenceRefusal);
        Assert.NotNull(receipt.Refusal);
        Assert.True(receipt.Refusal!.Retained);
    }

    private static string FindRepositoryRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null)
        {
            var candidate = Path.Combine(directory.FullName, "src", "SLI", "SLI.Lisp", "harmonic-interlock-modulation-correspondence.lisp");
            if (File.Exists(candidate))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        throw new DirectoryNotFoundException("Could not locate Project Bicycle repository root.");
    }
}
