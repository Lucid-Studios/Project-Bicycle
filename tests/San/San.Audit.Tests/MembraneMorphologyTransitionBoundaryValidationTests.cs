using San.Common;
using Xunit;

namespace San.Audit.Tests;

public sealed class MembraneMorphologyTransitionBoundaryValidationTests
{
    private static readonly DateTimeOffset TimestampUtc = DateTimeOffset.Parse("2026-05-17T00:00:00Z");

    [Fact]
    public void Membrane_May_Deform_Without_Core_Mutation()
    {
        var receipt = Declare(CreateRequest());

        AssertCold(receipt);
        Assert.Equal(MembraneMorphologyTransitionDisposition.TransitionRetainedCold, receipt.Disposition);
        Assert.Equal("membrane-morphology-transition-retained-review-only", receipt.OutcomeCode);
        Assert.Equal(6, receipt.Transitions.Count);
        Assert.True(receipt.MembraneDeformed);
        Assert.True(receipt.MalformationWitnessed);
        Assert.True(receipt.CompostRetained);
        Assert.True(receipt.TransitionEvidenceRetained);
        Assert.True(receipt.HighEnergyPressureReferenced);
        Assert.Contains(receipt.Transitions, transition => transition.TransitionClass == MembraneMorphologyTransitionClass.ElasticDeformation);
        Assert.Contains(receipt.Transitions, transition => transition.TransitionClass == MembraneMorphologyTransitionClass.LawfulMalformation);
        Assert.Contains(receipt.Transitions, transition => transition.TransitionClass == MembraneMorphologyTransitionClass.CompostableResidue);
        Assert.Contains(receipt.Transitions, transition => transition.TransitionClass == MembraneMorphologyTransitionClass.RepairableTransition);
        Assert.Contains(receipt.Transitions, transition => transition.TransitionClass == MembraneMorphologyTransitionClass.StableMorphologyCandidate);
        Assert.Contains(receipt.Transitions, transition => transition.TransitionClass == MembraneMorphologyTransitionClass.ReturnToPrimeCooling);
        Assert.Contains("High-energy articulation pressure may deform the SLI.Lisp membrane", receipt.GovernanceTrace, StringComparison.Ordinal);
    }

    [Fact]
    public void Transition_Does_Not_Call_Bind_Start_Admit_Or_Authorize()
    {
        var receipt = Declare(CreateRequest(priorPassageCount: 313));

        AssertCold(receipt);
        Assert.Equal(313, receipt.PriorPassageCount);
        Assert.Equal(313, receipt.PassageCountAfterTransitionReview);
        Assert.False(receipt.CoreMutated);
        Assert.False(receipt.IdentityMutated);
        Assert.False(receipt.SelfGelMutated);
        Assert.False(receipt.OeMutated);
        Assert.False(receipt.ModelBound);
        Assert.False(receipt.ProviderCallMade);
        Assert.False(receipt.HeartbeatActive);
        Assert.False(receipt.CmeActualAdmitted);
        Assert.False(receipt.RuntimeStarted);
        Assert.False(receipt.ActionAuthorized);
        Assert.False(receipt.ContinuityAdmitted);
        Assert.False(receipt.AuthorityGranted);
        Assert.False(receipt.LispEvaluationAllowed);
        Assert.False(receipt.NewPacketEmitted);
        Assert.False(receipt.ReceiptsReplayed);
        Assert.False(receipt.PassageIncremented);
        Assert.True(receipt.ActivationRefused);
    }

    [Fact]
    public void Empty_Transition_Set_Is_Reviewable_But_Not_Deformation()
    {
        var receipt = Declare(CreateRequest(transitions: []));

        Assert.Equal(MembraneMorphologyTransitionDisposition.EmptyReviewCold, receipt.Disposition);
        Assert.True(receipt.IsColdEmptyMembraneMorphologyTransition);
        Assert.Empty(receipt.Transitions);
        Assert.False(receipt.MembraneDeformed);
        Assert.False(receipt.HighEnergyPressureReferenced);
        Assert.True(receipt.ActivationRefused);
    }

    [Theory]
    [InlineData("missing")]
    [InlineData("refused")]
    [InlineData("not-cold")]
    [InlineData("model-bound")]
    [InlineData("heartbeat")]
    [InlineData("cme-actual")]
    public void Transition_Requires_Cold_High_Energy_Candidate_Source(string sourceCase)
    {
        var source = sourceCase switch
        {
            "missing" => null,
            "refused" => CreateSourceReceipt(refused: true),
            "not-cold" => CreateSourceReceipt(providerCallMade: true),
            "model-bound" => CreateSourceReceipt(modelBound: true),
            "heartbeat" => CreateSourceReceipt(heartbeatActive: true),
            "cme-actual" => CreateSourceReceipt(cmeActualAdmitted: true),
            _ => CreateSourceReceipt()
        };
        var request = sourceCase == "missing"
            ? CreateRequest() with { SourceHighEnergyCandidateReceipt = null }
            : CreateRequest(source: source);

        var receipt = Declare(request);

        AssertRefused(receipt, "membrane-morphology-source-high-energy-candidate-missing");
    }

    [Theory]
    [InlineData("missing-code")]
    [InlineData("not-present")]
    [InlineData("not-review")]
    [InlineData("no-deform")]
    [InlineData("no-malformation")]
    [InlineData("no-compost")]
    [InlineData("no-repair")]
    [InlineData("no-evidence")]
    [InlineData("core-mutation")]
    [InlineData("identity")]
    [InlineData("selfgel")]
    [InlineData("oe")]
    [InlineData("model-binding")]
    [InlineData("provider-call")]
    [InlineData("heartbeat")]
    [InlineData("cme-actual")]
    [InlineData("runtime")]
    [InlineData("action")]
    [InlineData("continuity")]
    [InlineData("authority")]
    [InlineData("lisp")]
    [InlineData("packet")]
    [InlineData("replay")]
    [InlineData("passage")]
    [InlineData("activation")]
    public void Scope_Boundary_Allows_Deformation_But_Not_Promotion(string mutation)
    {
        var receipt = Declare(CreateRequest(scope: MutateScope(CreateScope(), mutation)));

        AssertRefused(receipt, "membrane-morphology-scope-boundary-invalid");
    }

    [Theory]
    [InlineData("missing-law")]
    [InlineData("no-deform")]
    [InlineData("core-mutation")]
    [InlineData("malformation-failure")]
    [InlineData("compost-continuity")]
    [InlineData("evidence-authority")]
    [InlineData("engine-binding")]
    [InlineData("heartbeat")]
    [InlineData("cme-actual")]
    [InlineData("runtime")]
    [InlineData("action")]
    [InlineData("continuity")]
    [InlineData("authority")]
    [InlineData("lisp")]
    [InlineData("packet")]
    [InlineData("replay")]
    [InlineData("passage")]
    [InlineData("activation")]
    [InlineData("no-source")]
    [InlineData("no-witness")]
    [InlineData("no-cooling")]
    [InlineData("authority-present")]
    public void Non_Mutation_Boundary_Prevents_Membrane_Deformation_From_Becoming_Core_Change(string mutation)
    {
        var receipt = Declare(CreateRequest(nonMutation: MutateNonMutation(CreateNonMutation(), mutation)));

        AssertRefused(receipt, "membrane-morphology-core-non-mutation-invalid");
    }

    [Theory]
    [InlineData("missing-law")]
    [InlineData("no-witness")]
    [InlineData("no-compost")]
    [InlineData("no-repair")]
    [InlineData("no-prime")]
    [InlineData("normalize-corruption")]
    [InlineData("corruption-mutates-core")]
    [InlineData("erase-lineage")]
    [InlineData("authority")]
    [InlineData("skip-witness")]
    [InlineData("skip-cooling")]
    [InlineData("model-binding")]
    [InlineData("heartbeat")]
    [InlineData("cme-actual")]
    [InlineData("runtime")]
    [InlineData("action")]
    [InlineData("continuity")]
    [InlineData("lisp")]
    [InlineData("packet")]
    [InlineData("replay")]
    [InlineData("passage")]
    [InlineData("activation")]
    public void Compost_Boundary_Retains_Malformation_Without_Enthroning_It(string mutation)
    {
        var receipt = Declare(CreateRequest(compost: MutateCompost(CreateCompost(), mutation)));

        AssertRefused(receipt, "membrane-morphology-compost-boundary-invalid");
    }

    [Theory]
    [InlineData("missing-handle")]
    [InlineData("missing-source-receipt")]
    [InlineData("missing-source-candidate")]
    [InlineData("missing-chamber")]
    [InlineData("missing-origin")]
    [InlineData("missing-coe")]
    [InlineData("missing-cselfgel")]
    [InlineData("missing-membrane")]
    [InlineData("missing-evidence")]
    [InlineData("missing-witness")]
    [InlineData("missing-cooling")]
    [InlineData("missing-custody")]
    [InlineData("vector-over-unit")]
    [InlineData("corruption-class")]
    [InlineData("not-review")]
    [InlineData("not-transition")]
    [InlineData("not-membrane")]
    [InlineData("not-candidate")]
    [InlineData("no-deform")]
    [InlineData("no-malformation")]
    [InlineData("no-compost")]
    [InlineData("no-repair")]
    [InlineData("no-prime")]
    [InlineData("no-high-energy-lineage")]
    [InlineData("no-chamber-lineage")]
    [InlineData("no-coe-lineage")]
    [InlineData("no-cselfgel-lineage")]
    [InlineData("corruption")]
    [InlineData("core-mutation")]
    [InlineData("identity")]
    [InlineData("selfgel")]
    [InlineData("oe")]
    [InlineData("model-binding")]
    [InlineData("provider-call")]
    [InlineData("heartbeat")]
    [InlineData("cme-actual")]
    [InlineData("runtime")]
    [InlineData("action")]
    [InlineData("continuity")]
    [InlineData("authority")]
    [InlineData("lisp")]
    [InlineData("packet")]
    [InlineData("replay")]
    [InlineData("passage")]
    [InlineData("activation")]
    public void Transition_Record_Remains_Membrane_Only_And_Non_Activating(string mutation)
    {
        var transitions = CreateTransitions();
        transitions[0] = MutateTransition(transitions[0], mutation);

        var receipt = Declare(CreateRequest(transitions: transitions));

        AssertRefused(receipt, "membrane-morphology-transition-invalid");
    }

    [Fact]
    public void Duplicate_Transition_Handles_Are_Refused()
    {
        var transitions = CreateTransitions();
        transitions[1] = transitions[1] with { TransitionHandle = transitions[0].TransitionHandle };

        var receipt = Declare(CreateRequest(transitions: transitions));

        AssertRefused(receipt, "membrane-morphology-duplicate-transition-handle");
    }

    [Theory]
    [InlineData("source-receipt")]
    [InlineData("source-candidate")]
    [InlineData("chamber")]
    [InlineData("origin")]
    [InlineData("coe")]
    [InlineData("cselfgel")]
    public void Transition_Lineage_Must_Reconstruct_Without_Shortcuts(string lineage)
    {
        var transitions = CreateTransitions();
        transitions[0] = lineage switch
        {
            "source-receipt" => transitions[0] with { SourceHighEnergyCandidateReceiptHandle = "urn:san:high-energy-articulation:foreign" },
            "source-candidate" => transitions[0] with { SourceCandidateHandle = "urn:san:high-energy-candidate:foreign" },
            "chamber" => transitions[0] with { ZedDeltaChamberReceiptHandle = "urn:san:zed-delta-chamber:foreign" },
            "origin" => transitions[0] with { ZedDeltaOriginHandle = "urn:san:zed-delta:origin:foreign" },
            "coe" => transitions[0] with { ConditionalOeHandle = "urn:san:coe:foreign" },
            "cselfgel" => transitions[0] with { ConditionalSelfGelHandle = "urn:san:cselfgel:foreign" },
            _ => transitions[0]
        };

        var receipt = Declare(CreateRequest(transitions: transitions));

        AssertRefused(receipt, "membrane-morphology-transition-lineage-invalid");
    }

    [Fact]
    public void Transition_Class_Coverage_Is_Required_For_Retention()
    {
        var transitions = CreateTransitions();
        transitions[^1] = transitions[^1] with { TransitionClass = transitions[0].TransitionClass };

        var receipt = Declare(CreateRequest(transitions: transitions));

        AssertRefused(receipt, "membrane-morphology-transition-class-coverage-missing");
    }

    [Fact]
    public void Lisp_Body_Declares_Membrane_Morphology_Transition_As_Inert_Carrier()
    {
        var root = FindRepositoryRoot();
        var body = File.ReadAllText(Path.Combine(root, "src", "SLI", "SLI.Lisp", "membrane-morphology-transition.lisp"));

        Assert.Contains(":posture :cme-membrane-morphology-transition-boundary", body, StringComparison.Ordinal);
        Assert.Contains(":lisp-role :inert-membrane-morphology-transition-carrier", body, StringComparison.Ordinal);
        Assert.Contains(":membrane-may-deform t", body, StringComparison.Ordinal);
        Assert.Contains(":core-mutated nil", body, StringComparison.Ordinal);
        Assert.Contains(":model-binding-allowed nil", body, StringComparison.Ordinal);
        Assert.Contains(":heartbeat-active nil", body, StringComparison.Ordinal);
        Assert.Contains(":cme-actual-admitted nil", body, StringComparison.Ordinal);
        Assert.Contains(":lisp-evaluation-allowed nil", body, StringComparison.Ordinal);
        Assert.DoesNotContain("(eval", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("(compile", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("(load", body, StringComparison.OrdinalIgnoreCase);
    }

    private static MembraneMorphologyTransitionReceipt Declare(MembraneMorphologyTransitionRequest request) =>
        new DefaultMembraneMorphologyTransitionBoundaryValidator().Declare(request, TimestampUtc);

    private static MembraneMorphologyTransitionRequest CreateRequest(
        HighEnergyArticulationCandidateReceipt? source = null,
        IReadOnlyList<MembraneMorphologyTransition>? transitions = null,
        MembraneTransitionScopeBoundary? scope = null,
        MembraneCoreNonMutationBoundary? nonMutation = null,
        MorphologicalCompostBoundary? compost = null,
        int priorPassageCount = 29) =>
        new(
            SourceHighEnergyCandidateReceipt: source ?? CreateSourceReceipt(),
            Transitions: transitions ?? CreateTransitions(),
            ScopeBoundary: scope ?? CreateScope(),
            NonMutationBoundary: nonMutation ?? CreateNonMutation(),
            CompostBoundary: compost ?? CreateCompost(),
            PriorPassageCount: priorPassageCount);

    private static MembraneMorphologyTransition[] CreateTransitions() =>
    [
        CreateTransition(MembraneMorphologyTransitionClass.ElasticDeformation, "elastic", "urn:san:high-energy-candidate:main-body", 0.68m),
        CreateTransition(MembraneMorphologyTransitionClass.LawfulMalformation, "malformation", "urn:san:high-energy-candidate:governance-review", 0.74m),
        CreateTransition(MembraneMorphologyTransitionClass.CompostableResidue, "compost", "urn:san:high-energy-candidate:cme-test-body", 0.62m),
        CreateTransition(MembraneMorphologyTransitionClass.RepairableTransition, "repair", "urn:san:high-energy-candidate:comparative", 0.57m),
        CreateTransition(MembraneMorphologyTransitionClass.StableMorphologyCandidate, "stable-candidate", "urn:san:high-energy-candidate:local-slm", 0.51m),
        CreateTransition(MembraneMorphologyTransitionClass.ReturnToPrimeCooling, "return-prime", "urn:san:high-energy-candidate:main-body", 0.49m)
    ];

    private static MembraneMorphologyTransition CreateTransition(
        MembraneMorphologyTransitionClass transitionClass,
        string suffix,
        string sourceCandidateHandle,
        decimal deformationPressure) =>
        new(
            TransitionHandle: $"urn:san:membrane-transition:{suffix}",
            TransitionClass: transitionClass,
            SourceHighEnergyCandidateReceiptHandle: "urn:san:high-energy-articulation:review:test",
            SourceCandidateHandle: sourceCandidateHandle,
            ZedDeltaChamberReceiptHandle: "urn:san:zed-delta-chamber:review:test",
            ZedDeltaOriginHandle: "urn:san:zed-delta:origin:0-0-0",
            ConditionalOeHandle: "urn:san:coe:primary",
            ConditionalSelfGelHandle: "urn:san:cselfgel:primary",
            MembraneHandle: "urn:san:sli-lisp:membrane:morphology-transition",
            EvidenceHandle: $"urn:san:evidence:membrane-transition:{suffix}",
            WitnessHandle: $"urn:san:witness:membrane-transition:{suffix}",
            CoolingHandle: "urn:san:cooling:membrane-morphology-transition",
            CustodyOwner: "steward",
            PressureVector: new MembraneMorphologyPressureVector(0.66m, deformationPressure, 0.54m, 0.48m, 0.58m, 0.72m),
            ReviewOnly: true,
            TransitionOnly: true,
            MembraneOnly: true,
            MorphologyCandidateOnly: true,
            MembraneMayDeform: true,
            MalformationMayBeWitnessed: true,
            CompostMayBeRetained: true,
            RepairMayBeRouted: true,
            ReturnToPrimeAllowed: true,
            PreservesHighEnergyCandidateLineage: true,
            PreservesChamberLineage: true,
            PreservesConditionalOeLineage: true,
            PreservesConditionalSelfGelLineage: true,
            CorruptionAttempted: false,
            CoreMutated: false,
            IdentityMutated: false,
            SelfGelMutated: false,
            OeMutated: false,
            ModelBindingRequested: false,
            ProviderCallRequested: false,
            HeartbeatActivationRequested: false,
            CmeActualAdmissionRequested: false,
            RuntimeStartRequested: false,
            ActionAuthorizationRequested: false,
            ContinuityAdmissionRequested: false,
            AuthorityRequested: false,
            LispEvaluationRequested: false,
            PacketEmissionRequested: false,
            ReceiptReplayRequested: false,
            PassageIncrementRequested: false,
            ActivationRequested: false);

    private static MembraneTransitionScopeBoundary CreateScope() =>
        new(
            BoundaryCode: "membrane-morphology-transition-review-only",
            Present: true,
            ReviewOnly: true,
            AllowsMembraneDeformation: true,
            AllowsMalformationWitness: true,
            AllowsCompostRetention: true,
            AllowsRepairRouting: true,
            AllowsTransitionEvidence: true,
            AllowsCoreMutation: false,
            AllowsIdentityMutation: false,
            AllowsSelfGelMutation: false,
            AllowsOeMutation: false,
            AllowsModelBinding: false,
            AllowsProviderCall: false,
            AllowsHeartbeatActivation: false,
            AllowsCmeActualAdmission: false,
            AllowsRuntimeStart: false,
            AllowsActionAuthorization: false,
            AllowsContinuityAdmission: false,
            AllowsAuthority: false,
            AllowsLispEvaluation: false,
            AllowsPacketEmission: false,
            AllowsReceiptReplay: false,
            AllowsPassageIncrement: false,
            AllowsActivation: false);

    private static MembraneCoreNonMutationBoundary CreateNonMutation() =>
        new(
            BoundaryLaw: "membrane deformation is not core mutation",
            MembraneMayDeform: true,
            CoreMayMutate: false,
            MalformationMayBecomeFailure: false,
            CompostMayBecomeContinuity: false,
            TransitionEvidenceMayAuthorize: false,
            DeformationMayBindEngine: false,
            TransitionMayActivateHeartbeat: false,
            TransitionMayAdmitCmeActual: false,
            TransitionMayStartRuntime: false,
            TransitionMayAuthorizeAction: false,
            TransitionMayAdmitContinuity: false,
            TransitionMayGrantAuthority: false,
            TransitionMayEvaluateLisp: false,
            TransitionMayEmitPacket: false,
            TransitionMayReplayReceipt: false,
            TransitionMayIncrementPassage: false,
            TransitionMayActivate: false,
            RequiresHighEnergyCandidate: true,
            RequiresWitness: true,
            RequiresCooling: true,
            RequiresAuthorityAbsence: true);

    private static MorphologicalCompostBoundary CreateCompost() =>
        new(
            BoundaryLaw: "malformation may compost without ruling",
            MalformationMayBeWitnessed: true,
            MalformationMayBeRetainedAsCompost: true,
            CompostMayRouteRepair: true,
            CompostMayReturnToPrime: true,
            CorruptionMayBeNormalized: false,
            CorruptionMayMutateCore: false,
            CompostMayEraseLineage: false,
            CompostMayGrantAuthority: false,
            RepairMaySkipWitness: false,
            CoolingMayBeSkipped: false,
            CompostMayBindModel: false,
            CompostMayActivateHeartbeat: false,
            CompostMayAdmitCmeActual: false,
            CompostMayStartRuntime: false,
            CompostMayAuthorizeAction: false,
            CompostMayAdmitContinuity: false,
            CompostMayEvaluateLisp: false,
            CompostMayEmitPacket: false,
            CompostMayReplayReceipt: false,
            CompostMayIncrementPassage: false,
            CompostMayActivate: false);

    private static HighEnergyArticulationCandidateReceipt CreateSourceReceipt(
        bool refused = false,
        bool providerCallMade = false,
        bool modelBound = false,
        bool heartbeatActive = false,
        bool cmeActualAdmitted = false)
    {
        var candidates = CreateCandidates();
        return new HighEnergyArticulationCandidateReceipt(
            ReceiptHandle: refused ? "urn:san:high-energy-articulation:refused:test" : "urn:san:high-energy-articulation:review:test",
            Disposition: refused ? HighEnergyArticulationCandidateDisposition.Refused : HighEnergyArticulationCandidateDisposition.CandidateNamedCold,
            OutcomeCode: refused ? "refused" : "high-energy-articulation-candidate-named-review-only",
            GovernanceTrace: "source high-energy candidate test receipt",
            SourceZedDeltaChamberReceiptHandle: "urn:san:zed-delta-chamber:review:test",
            Candidates: refused ? [] : candidates,
            ObservationBoundary: CreateObservation(),
            NonClaimBoundary: CreateNonClaim(),
            NonBindingBoundary: CreateNonBinding(),
            Refusal: refused ? new HighEnergyArticulationCandidateRefusalReceipt("urn:san:high-energy-refusal:test", "refused", "refused", true) : null,
            PriorPassageCount: 29,
            PassageCountAfterCandidateReview: 29,
            CandidateCount: refused ? 0 : candidates.Length,
            ReviewOnly: true,
            CandidateOnly: true,
            HighEnergyBodyNamed: !refused,
            PublicInterfaceReferenced: !refused,
            ProviderCallMade: providerCallMade,
            ModelBound: modelBound,
            HiddenSubstrateClaimed: false,
            HiddenInternalsMapped: false,
            WeightsClaimed: false,
            TrainingDataClaimed: false,
            PersistentMemoryClaimed: false,
            RuntimeIdentityClaimed: false,
            HeartbeatActive: heartbeatActive,
            CmeActualAdmitted: cmeActualAdmitted,
            RuntimeStarted: false,
            ActionAuthorized: false,
            ContinuityAdmitted: false,
            AuthorityGranted: false,
            IdentityMutated: false,
            SelfGelMutated: false,
            LispEvaluationAllowed: false,
            NewPacketEmitted: false,
            ReceiptsReplayed: false,
            PassageIncremented: false,
            ActivationRefused: true,
            TimestampUtc: TimestampUtc);
    }

    private static HighEnergyArticulationCandidate[] CreateCandidates() =>
    [
        CreateCandidate(HighEnergyArticulationCandidateRole.MainBodyEngineCandidate, HighEnergyProviderInterfaceClass.OfficialPublicDocumentation, "main-body", "OpenAI", "GPT"),
        CreateCandidate(HighEnergyArticulationCandidateRole.GovernanceReviewCandidate, HighEnergyProviderInterfaceClass.PublishedApiContract, "governance-review", "OpenAI", "Codex"),
        CreateCandidate(HighEnergyArticulationCandidateRole.InstantiatedCmeTestBodyCandidate, HighEnergyProviderInterfaceClass.ObservableConversationBehavior, "cme-test-body", "OpenAI", "mini-or-micro"),
        CreateCandidate(HighEnergyArticulationCandidateRole.ComparativeUniversalityCandidate, HighEnergyProviderInterfaceClass.ComparativeEvaluationSurface, "comparative", "comparative-provider", "public-interface-only"),
        CreateCandidate(HighEnergyArticulationCandidateRole.LocalSlmCandidate, HighEnergyProviderInterfaceClass.LocalRuntimeAdapterDescription, "local-slm", "local-runtime", "deferred-slm")
    ];

    private static HighEnergyArticulationCandidate CreateCandidate(
        HighEnergyArticulationCandidateRole role,
        HighEnergyProviderInterfaceClass interfaceClass,
        string suffix,
        string provider,
        string modelLine) =>
        new(
            CandidateHandle: $"urn:san:high-energy-candidate:{suffix}",
            CandidateRole: role,
            InterfaceClass: interfaceClass,
            ProviderFamily: provider,
            ModelLine: modelLine,
            IntendedRole: $"review-only-{suffix}",
            ZedDeltaChamberReceiptHandle: "urn:san:zed-delta-chamber:review:test",
            ZedDeltaOriginHandle: "urn:san:zed-delta:origin:0-0-0",
            ConditionalOeHandle: "urn:san:coe:primary",
            ConditionalSelfGelHandle: "urn:san:cselfgel:primary",
            TelemetryShapeHandle: $"urn:san:telemetry-shape:high-energy:{suffix}",
            PublicDocumentationHandle: $"urn:san:public-interface:{suffix}",
            WitnessHandle: $"urn:san:witness:high-energy:{suffix}",
            CustodyOwner: "steward",
            ReviewOnly: true,
            CandidateOnly: true,
            RoleTyped: true,
            PublicInterfaceOnly: true,
            ObservableBehaviorOnly: true,
            PreservesChamberLineage: true,
            PreservesConditionalOeLineage: true,
            PreservesConditionalSelfGelLineage: true,
            ProviderCallRequested: false,
            ModelBindingRequested: false,
            HiddenSubstrateClaimed: false,
            WeightAccessClaimed: false,
            TrainingDataClaimed: false,
            PersistentMemoryClaimed: false,
            RuntimeIdentityClaimed: false,
            HeartbeatActivationRequested: false,
            CmeActualAdmissionRequested: false,
            ActionAuthorizationRequested: false,
            ContinuityAdmissionRequested: false,
            AuthorityRequested: false,
            LispEvaluationRequested: false,
            PacketEmissionRequested: false,
            ReceiptReplayRequested: false,
            PassageIncrementRequested: false,
            ActivationRequested: false);

    private static ProviderInterfaceObservationBoundary CreateObservation() =>
        new(
            BoundaryCode: "provider-interface-observation-public-review-only",
            Present: true,
            ReviewOnly: true,
            PublicObservableOnly: true,
            AllowsOfficialDocumentationReference: true,
            AllowsPublishedApiContractReference: true,
            AllowsObservableBehaviorStudy: true,
            AllowsProviderCall: false,
            AllowsProviderVisibleAccess: false,
            AllowsModelContextExport: false,
            AllowsScraping: false,
            AllowsHiddenInternalsMapping: false,
            AllowsWeightAccess: false,
            AllowsTrainingDataInference: false,
            AllowsPersistentMemoryClaim: false,
            AllowsRuntimeIdentityClaim: false,
            AllowsAuthority: false);

    private static HiddenSubstrateNonClaimBoundary CreateNonClaim() =>
        new(
            BoundaryLaw: "observable interface is not hidden substrate proof",
            PublicInterfaceMayBeStudied: true,
            HiddenSubstrateMayBeClaimed: false,
            ProprietaryInternalsMayBeMapped: false,
            WeightsMayBeClaimed: false,
            TrainingDataMayBeClaimed: false,
            ProviderLogsMayBeClaimed: false,
            SystemPromptMayBeClaimed: false,
            FullCausalCertaintyMayBeClaimed: false,
            ObservableBehaviorMayBecomeInternalProof: false,
            DocumentationMayBecomeImplementationProof: false,
            InterfaceSuccessMayBecomeSemanticWarrant: false,
            RequiresUncertaintyRetention: true,
            RequiresSourceAttribution: true,
            RequiresNonEquivalenceClaim: true);

    private static CandidateNonBindingBoundary CreateNonBinding() =>
        new(
            BoundaryLaw: "candidate naming is not model binding",
            CandidateMayBeNamed: true,
            RoleMayBeAssigned: true,
            InterfaceMayBeObserved: true,
            ModelMayBind: false,
            ProviderMayBeCalled: false,
            HeartbeatMayActivate: false,
            CmeActualMayBeAdmitted: false,
            RuntimeMayStart: false,
            ActionMayBeAuthorized: false,
            ContinuityMayBeAdmitted: false,
            AuthorityMayBeGranted: false,
            IdentityMayMutate: false,
            SelfGelMayMutate: false,
            LispMayEvaluate: false,
            PacketMayEmit: false,
            ReceiptMayReplay: false,
            PassageMayIncrement: false,
            ActivationMayProceed: false,
            RequiresZedDeltaChamber: true,
            RequiresWitness: true,
            RequiresAuthorityAbsence: true);

    private static MembraneTransitionScopeBoundary MutateScope(
        MembraneTransitionScopeBoundary boundary,
        string mutation) =>
        mutation switch
        {
            "missing-code" => boundary with { BoundaryCode = string.Empty },
            "not-present" => boundary with { Present = false },
            "not-review" => boundary with { ReviewOnly = false },
            "no-deform" => boundary with { AllowsMembraneDeformation = false },
            "no-malformation" => boundary with { AllowsMalformationWitness = false },
            "no-compost" => boundary with { AllowsCompostRetention = false },
            "no-repair" => boundary with { AllowsRepairRouting = false },
            "no-evidence" => boundary with { AllowsTransitionEvidence = false },
            "core-mutation" => boundary with { AllowsCoreMutation = true },
            "identity" => boundary with { AllowsIdentityMutation = true },
            "selfgel" => boundary with { AllowsSelfGelMutation = true },
            "oe" => boundary with { AllowsOeMutation = true },
            "model-binding" => boundary with { AllowsModelBinding = true },
            "provider-call" => boundary with { AllowsProviderCall = true },
            "heartbeat" => boundary with { AllowsHeartbeatActivation = true },
            "cme-actual" => boundary with { AllowsCmeActualAdmission = true },
            "runtime" => boundary with { AllowsRuntimeStart = true },
            "action" => boundary with { AllowsActionAuthorization = true },
            "continuity" => boundary with { AllowsContinuityAdmission = true },
            "authority" => boundary with { AllowsAuthority = true },
            "lisp" => boundary with { AllowsLispEvaluation = true },
            "packet" => boundary with { AllowsPacketEmission = true },
            "replay" => boundary with { AllowsReceiptReplay = true },
            "passage" => boundary with { AllowsPassageIncrement = true },
            "activation" => boundary with { AllowsActivation = true },
            _ => throw new ArgumentOutOfRangeException(nameof(mutation), mutation, null)
        };

    private static MembraneCoreNonMutationBoundary MutateNonMutation(
        MembraneCoreNonMutationBoundary boundary,
        string mutation) =>
        mutation switch
        {
            "missing-law" => boundary with { BoundaryLaw = string.Empty },
            "no-deform" => boundary with { MembraneMayDeform = false },
            "core-mutation" => boundary with { CoreMayMutate = true },
            "malformation-failure" => boundary with { MalformationMayBecomeFailure = true },
            "compost-continuity" => boundary with { CompostMayBecomeContinuity = true },
            "evidence-authority" => boundary with { TransitionEvidenceMayAuthorize = true },
            "engine-binding" => boundary with { DeformationMayBindEngine = true },
            "heartbeat" => boundary with { TransitionMayActivateHeartbeat = true },
            "cme-actual" => boundary with { TransitionMayAdmitCmeActual = true },
            "runtime" => boundary with { TransitionMayStartRuntime = true },
            "action" => boundary with { TransitionMayAuthorizeAction = true },
            "continuity" => boundary with { TransitionMayAdmitContinuity = true },
            "authority" => boundary with { TransitionMayGrantAuthority = true },
            "lisp" => boundary with { TransitionMayEvaluateLisp = true },
            "packet" => boundary with { TransitionMayEmitPacket = true },
            "replay" => boundary with { TransitionMayReplayReceipt = true },
            "passage" => boundary with { TransitionMayIncrementPassage = true },
            "activation" => boundary with { TransitionMayActivate = true },
            "no-source" => boundary with { RequiresHighEnergyCandidate = false },
            "no-witness" => boundary with { RequiresWitness = false },
            "no-cooling" => boundary with { RequiresCooling = false },
            "authority-present" => boundary with { RequiresAuthorityAbsence = false },
            _ => throw new ArgumentOutOfRangeException(nameof(mutation), mutation, null)
        };

    private static MorphologicalCompostBoundary MutateCompost(
        MorphologicalCompostBoundary boundary,
        string mutation) =>
        mutation switch
        {
            "missing-law" => boundary with { BoundaryLaw = string.Empty },
            "no-witness" => boundary with { MalformationMayBeWitnessed = false },
            "no-compost" => boundary with { MalformationMayBeRetainedAsCompost = false },
            "no-repair" => boundary with { CompostMayRouteRepair = false },
            "no-prime" => boundary with { CompostMayReturnToPrime = false },
            "normalize-corruption" => boundary with { CorruptionMayBeNormalized = true },
            "corruption-mutates-core" => boundary with { CorruptionMayMutateCore = true },
            "erase-lineage" => boundary with { CompostMayEraseLineage = true },
            "authority" => boundary with { CompostMayGrantAuthority = true },
            "skip-witness" => boundary with { RepairMaySkipWitness = true },
            "skip-cooling" => boundary with { CoolingMayBeSkipped = true },
            "model-binding" => boundary with { CompostMayBindModel = true },
            "heartbeat" => boundary with { CompostMayActivateHeartbeat = true },
            "cme-actual" => boundary with { CompostMayAdmitCmeActual = true },
            "runtime" => boundary with { CompostMayStartRuntime = true },
            "action" => boundary with { CompostMayAuthorizeAction = true },
            "continuity" => boundary with { CompostMayAdmitContinuity = true },
            "lisp" => boundary with { CompostMayEvaluateLisp = true },
            "packet" => boundary with { CompostMayEmitPacket = true },
            "replay" => boundary with { CompostMayReplayReceipt = true },
            "passage" => boundary with { CompostMayIncrementPassage = true },
            "activation" => boundary with { CompostMayActivate = true },
            _ => throw new ArgumentOutOfRangeException(nameof(mutation), mutation, null)
        };

    private static MembraneMorphologyTransition MutateTransition(
        MembraneMorphologyTransition transition,
        string mutation) =>
        mutation switch
        {
            "missing-handle" => transition with { TransitionHandle = string.Empty },
            "missing-source-receipt" => transition with { SourceHighEnergyCandidateReceiptHandle = string.Empty },
            "missing-source-candidate" => transition with { SourceCandidateHandle = string.Empty },
            "missing-chamber" => transition with { ZedDeltaChamberReceiptHandle = string.Empty },
            "missing-origin" => transition with { ZedDeltaOriginHandle = string.Empty },
            "missing-coe" => transition with { ConditionalOeHandle = string.Empty },
            "missing-cselfgel" => transition with { ConditionalSelfGelHandle = string.Empty },
            "missing-membrane" => transition with { MembraneHandle = string.Empty },
            "missing-evidence" => transition with { EvidenceHandle = string.Empty },
            "missing-witness" => transition with { WitnessHandle = string.Empty },
            "missing-cooling" => transition with { CoolingHandle = string.Empty },
            "missing-custody" => transition with { CustodyOwner = string.Empty },
            "vector-over-unit" => transition with { PressureVector = transition.PressureVector with { DeformationPressure = 1.1m } },
            "corruption-class" => transition with { TransitionClass = MembraneMorphologyTransitionClass.CorruptionAttempt },
            "not-review" => transition with { ReviewOnly = false },
            "not-transition" => transition with { TransitionOnly = false },
            "not-membrane" => transition with { MembraneOnly = false },
            "not-candidate" => transition with { MorphologyCandidateOnly = false },
            "no-deform" => transition with { MembraneMayDeform = false },
            "no-malformation" => transition with { MalformationMayBeWitnessed = false },
            "no-compost" => transition with { CompostMayBeRetained = false },
            "no-repair" => transition with { RepairMayBeRouted = false },
            "no-prime" => transition with { ReturnToPrimeAllowed = false },
            "no-high-energy-lineage" => transition with { PreservesHighEnergyCandidateLineage = false },
            "no-chamber-lineage" => transition with { PreservesChamberLineage = false },
            "no-coe-lineage" => transition with { PreservesConditionalOeLineage = false },
            "no-cselfgel-lineage" => transition with { PreservesConditionalSelfGelLineage = false },
            "corruption" => transition with { CorruptionAttempted = true },
            "core-mutation" => transition with { CoreMutated = true },
            "identity" => transition with { IdentityMutated = true },
            "selfgel" => transition with { SelfGelMutated = true },
            "oe" => transition with { OeMutated = true },
            "model-binding" => transition with { ModelBindingRequested = true },
            "provider-call" => transition with { ProviderCallRequested = true },
            "heartbeat" => transition with { HeartbeatActivationRequested = true },
            "cme-actual" => transition with { CmeActualAdmissionRequested = true },
            "runtime" => transition with { RuntimeStartRequested = true },
            "action" => transition with { ActionAuthorizationRequested = true },
            "continuity" => transition with { ContinuityAdmissionRequested = true },
            "authority" => transition with { AuthorityRequested = true },
            "lisp" => transition with { LispEvaluationRequested = true },
            "packet" => transition with { PacketEmissionRequested = true },
            "replay" => transition with { ReceiptReplayRequested = true },
            "passage" => transition with { PassageIncrementRequested = true },
            "activation" => transition with { ActivationRequested = true },
            _ => throw new ArgumentOutOfRangeException(nameof(mutation), mutation, null)
        };

    private static void AssertCold(MembraneMorphologyTransitionReceipt receipt)
    {
        Assert.True(receipt.IsColdMembraneMorphologyTransition);
        Assert.True(receipt.ReviewOnly);
        Assert.True(receipt.TransitionOnly);
        Assert.True(receipt.ActivationRefused);
        Assert.False(receipt.CoreMutated);
        Assert.False(receipt.IdentityMutated);
        Assert.False(receipt.SelfGelMutated);
        Assert.False(receipt.OeMutated);
        Assert.False(receipt.ModelBound);
        Assert.False(receipt.ProviderCallMade);
        Assert.False(receipt.HeartbeatActive);
        Assert.False(receipt.CmeActualAdmitted);
        Assert.False(receipt.ActionAuthorized);
        Assert.False(receipt.ContinuityAdmitted);
        Assert.False(receipt.AuthorityGranted);
        Assert.False(receipt.LispEvaluationAllowed);
    }

    private static void AssertRefused(MembraneMorphologyTransitionReceipt receipt, string outcomeCode)
    {
        Assert.Equal(MembraneMorphologyTransitionDisposition.Refused, receipt.Disposition);
        Assert.Equal(outcomeCode, receipt.OutcomeCode);
        Assert.True(receipt.IsRetainedMembraneMorphologyTransitionRefusal);
        Assert.NotNull(receipt.Refusal);
        Assert.True(receipt.Refusal.Retained);
        Assert.Empty(receipt.Transitions);
        Assert.Equal(receipt.PriorPassageCount, receipt.PassageCountAfterTransitionReview);
        Assert.False(receipt.CoreMutated);
        Assert.False(receipt.ModelBound);
        Assert.False(receipt.ProviderCallMade);
        Assert.False(receipt.CmeActualAdmitted);
        Assert.False(receipt.ActionAuthorized);
        Assert.False(receipt.ContinuityAdmitted);
        Assert.False(receipt.AuthorityGranted);
        Assert.True(receipt.ActivationRefused);
    }

    private static string FindRepositoryRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null)
        {
            var candidate = Path.Combine(directory.FullName, "src", "SLI", "SLI.Lisp", "membrane-morphology-transition.lisp");
            if (File.Exists(candidate))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        throw new DirectoryNotFoundException("Could not locate repository root from test output path.");
    }
}
