using San.Common;
using Xunit;

namespace San.Audit.Tests;

public sealed class HighEnergyArticulationCandidateBoundaryValidationTests
{
    private static readonly DateTimeOffset TimestampUtc = DateTimeOffset.Parse("2026-05-17T00:00:00Z");

    [Fact]
    public void Candidate_Engines_May_Be_Named_Without_Binding_Or_Activation()
    {
        var receipt = Declare(CreateRequest());

        AssertCold(receipt);
        Assert.Equal(HighEnergyArticulationCandidateDisposition.CandidateNamedCold, receipt.Disposition);
        Assert.Equal("high-energy-articulation-candidate-named-review-only", receipt.OutcomeCode);
        Assert.Equal(5, receipt.Candidates.Count);
        Assert.True(receipt.HighEnergyBodyNamed);
        Assert.True(receipt.PublicInterfaceReferenced);
        Assert.Contains(receipt.Candidates, candidate => candidate.CandidateRole == HighEnergyArticulationCandidateRole.MainBodyEngineCandidate);
        Assert.Contains(receipt.Candidates, candidate => candidate.CandidateRole == HighEnergyArticulationCandidateRole.GovernanceReviewCandidate);
        Assert.Contains(receipt.Candidates, candidate => candidate.CandidateRole == HighEnergyArticulationCandidateRole.InstantiatedCmeTestBodyCandidate);
        Assert.Contains(receipt.Candidates, candidate => candidate.CandidateRole == HighEnergyArticulationCandidateRole.ComparativeUniversalityCandidate);
        Assert.Contains(receipt.Candidates, candidate => candidate.CandidateRole == HighEnergyArticulationCandidateRole.LocalSlmCandidate);
        Assert.Contains("High-energy articulation candidates were named for review only", receipt.GovernanceTrace, StringComparison.Ordinal);
    }

    [Fact]
    public void Candidate_Body_Does_Not_Call_Bind_Start_Admit_Or_Authorize()
    {
        var receipt = Declare(CreateRequest(priorPassageCount: 313));

        AssertCold(receipt);
        Assert.Equal(313, receipt.PriorPassageCount);
        Assert.Equal(313, receipt.PassageCountAfterCandidateReview);
        Assert.False(receipt.ProviderCallMade);
        Assert.False(receipt.ModelBound);
        Assert.False(receipt.HiddenSubstrateClaimed);
        Assert.False(receipt.HiddenInternalsMapped);
        Assert.False(receipt.WeightsClaimed);
        Assert.False(receipt.TrainingDataClaimed);
        Assert.False(receipt.PersistentMemoryClaimed);
        Assert.False(receipt.RuntimeIdentityClaimed);
        Assert.False(receipt.HeartbeatActive);
        Assert.False(receipt.CmeActualAdmitted);
        Assert.False(receipt.RuntimeStarted);
        Assert.False(receipt.ActionAuthorized);
        Assert.False(receipt.ContinuityAdmitted);
        Assert.False(receipt.AuthorityGranted);
        Assert.False(receipt.IdentityMutated);
        Assert.False(receipt.SelfGelMutated);
        Assert.False(receipt.LispEvaluationAllowed);
        Assert.False(receipt.NewPacketEmitted);
        Assert.False(receipt.ReceiptsReplayed);
        Assert.False(receipt.PassageIncremented);
        Assert.True(receipt.ActivationRefused);
    }

    [Theory]
    [InlineData("missing")]
    [InlineData("refused")]
    [InlineData("not-cold")]
    [InlineData("heartbeat")]
    [InlineData("cme-actual")]
    public void Candidate_Naming_Requires_Cold_Zed_Delta_Chamber_Source(string sourceCase)
    {
        var source = sourceCase switch
        {
            "missing" => null,
            "refused" => CreateSourceReceipt(refused: true),
            "not-cold" => CreateSourceReceipt(runtimeModelBound: true),
            "heartbeat" => CreateSourceReceipt(heartbeatActive: true),
            "cme-actual" => CreateSourceReceipt(cmeActualAdmitted: true),
            _ => CreateSourceReceipt()
        };

        var request = sourceCase == "missing"
            ? CreateRequest() with { SourceZedDeltaChamberReceipt = null }
            : CreateRequest(source: source);

        var receipt = Declare(request);

        AssertRefused(receipt, "high-energy-source-zed-delta-chamber-missing");
    }

    [Theory]
    [InlineData("missing-code")]
    [InlineData("not-present")]
    [InlineData("not-review")]
    [InlineData("not-public")]
    [InlineData("no-docs")]
    [InlineData("no-api")]
    [InlineData("no-behavior")]
    [InlineData("provider-call")]
    [InlineData("provider-access")]
    [InlineData("context-export")]
    [InlineData("scraping")]
    [InlineData("hidden-map")]
    [InlineData("weight-access")]
    [InlineData("training-inference")]
    [InlineData("memory-claim")]
    [InlineData("runtime-identity")]
    [InlineData("authority")]
    public void Provider_Interface_Observation_Remains_Public_And_Non_Calling(string mutation)
    {
        var receipt = Declare(CreateRequest(observation: MutateObservation(CreateObservation(), mutation)));

        AssertRefused(receipt, "high-energy-provider-observation-boundary-invalid");
    }

    [Theory]
    [InlineData("missing-law")]
    [InlineData("no-public-study")]
    [InlineData("hidden-claim")]
    [InlineData("internals")]
    [InlineData("weights")]
    [InlineData("training-data")]
    [InlineData("provider-logs")]
    [InlineData("system-prompt")]
    [InlineData("causal-certainty")]
    [InlineData("behavior-proof")]
    [InlineData("documentation-proof")]
    [InlineData("success-warrant")]
    [InlineData("no-uncertainty")]
    [InlineData("no-attribution")]
    [InlineData("no-non-equivalence")]
    public void Hidden_Substrate_Non_Claim_Boundary_Blocks_Interface_To_Internal_Proof(string mutation)
    {
        var receipt = Declare(CreateRequest(nonClaim: MutateNonClaim(CreateNonClaim(), mutation)));

        AssertRefused(receipt, "high-energy-hidden-substrate-claim-invalid");
    }

    [Theory]
    [InlineData("missing-law")]
    [InlineData("no-candidate")]
    [InlineData("no-role")]
    [InlineData("no-interface")]
    [InlineData("model-binding")]
    [InlineData("provider-call")]
    [InlineData("heartbeat")]
    [InlineData("cme-actual")]
    [InlineData("runtime")]
    [InlineData("action")]
    [InlineData("continuity")]
    [InlineData("authority")]
    [InlineData("identity")]
    [InlineData("selfgel")]
    [InlineData("lisp")]
    [InlineData("packet")]
    [InlineData("replay")]
    [InlineData("passage")]
    [InlineData("activation")]
    [InlineData("no-zed")]
    [InlineData("no-witness")]
    [InlineData("authority-present")]
    public void Candidate_Non_Binding_Boundary_Blocks_Promotion_To_Active_Cme(string mutation)
    {
        var receipt = Declare(CreateRequest(nonBinding: MutateNonBinding(CreateNonBinding(), mutation)));

        AssertRefused(receipt, "high-energy-non-binding-boundary-invalid");
    }

    [Fact]
    public void Candidate_Set_May_Not_Be_Empty()
    {
        var receipt = Declare(CreateRequest(candidates: []));

        AssertRefused(receipt, "high-energy-candidate-missing");
    }

    [Theory]
    [InlineData("missing-handle")]
    [InlineData("missing-provider")]
    [InlineData("missing-model")]
    [InlineData("missing-role-description")]
    [InlineData("missing-chamber")]
    [InlineData("missing-origin")]
    [InlineData("missing-coe")]
    [InlineData("missing-cselfgel")]
    [InlineData("missing-telemetry")]
    [InlineData("missing-docs")]
    [InlineData("missing-witness")]
    [InlineData("missing-custody")]
    [InlineData("not-review")]
    [InlineData("not-candidate")]
    [InlineData("not-role-typed")]
    [InlineData("not-public")]
    [InlineData("not-observable")]
    [InlineData("no-chamber-lineage")]
    [InlineData("no-coe-lineage")]
    [InlineData("no-cselfgel-lineage")]
    [InlineData("provider-call")]
    [InlineData("model-binding")]
    [InlineData("hidden-substrate")]
    [InlineData("weights")]
    [InlineData("training")]
    [InlineData("memory")]
    [InlineData("runtime-identity")]
    [InlineData("heartbeat")]
    [InlineData("cme-actual")]
    [InlineData("action")]
    [InlineData("continuity")]
    [InlineData("authority")]
    [InlineData("lisp")]
    [InlineData("packet")]
    [InlineData("replay")]
    [InlineData("passage")]
    [InlineData("activation")]
    public void Every_Candidate_Remains_Public_Interface_Candidate_Only(string mutation)
    {
        var candidates = CreateCandidates();
        candidates[0] = MutateCandidate(candidates[0], mutation);

        var receipt = Declare(CreateRequest(candidates: candidates));

        AssertRefused(receipt, "high-energy-candidate-promotional");
    }

    [Fact]
    public void Duplicate_Candidate_Handles_Are_Refused()
    {
        var candidates = CreateCandidates();
        candidates[1] = candidates[1] with { CandidateHandle = candidates[0].CandidateHandle };

        var receipt = Declare(CreateRequest(candidates: candidates));

        AssertRefused(receipt, "high-energy-duplicate-candidate-handle");
    }

    [Theory]
    [InlineData("chamber")]
    [InlineData("origin")]
    [InlineData("coe")]
    [InlineData("cselfgel")]
    public void Candidate_Lineage_Must_Reconstruct_From_Zed_Delta_Source(string lineage)
    {
        var candidates = CreateCandidates();
        candidates[0] = lineage switch
        {
            "chamber" => candidates[0] with { ZedDeltaChamberReceiptHandle = "urn:san:zed-delta-chamber:foreign" },
            "origin" => candidates[0] with { ZedDeltaOriginHandle = "urn:san:zed-delta:origin:foreign" },
            "coe" => candidates[0] with { ConditionalOeHandle = "urn:san:coe:foreign" },
            "cselfgel" => candidates[0] with { ConditionalSelfGelHandle = "urn:san:cselfgel:foreign" },
            _ => candidates[0]
        };

        var receipt = Declare(CreateRequest(candidates: candidates));

        AssertRefused(receipt, "high-energy-candidate-lineage-invalid");
    }

    [Fact]
    public void Candidate_Role_Coverage_Is_Required_Before_Retention()
    {
        var candidates = CreateCandidates();
        candidates[^1] = candidates[^1] with { CandidateRole = candidates[0].CandidateRole };

        var receipt = Declare(CreateRequest(candidates: candidates));

        AssertRefused(receipt, "high-energy-candidate-role-coverage-missing");
    }

    [Fact]
    public void Lisp_Body_Declares_High_Energy_Candidate_As_Inert_Carrier()
    {
        var root = FindRepositoryRoot();
        var body = File.ReadAllText(Path.Combine(root, "src", "SLI", "SLI.Lisp", "high-energy-articulation-candidate.lisp"));

        Assert.Contains(":posture :cme-high-energy-articulation-candidate-boundary", body, StringComparison.Ordinal);
        Assert.Contains(":lisp-role :inert-high-energy-articulation-candidate-carrier", body, StringComparison.Ordinal);
        Assert.Contains(":candidate-engine-named t", body, StringComparison.Ordinal);
        Assert.Contains(":provider-call-allowed nil", body, StringComparison.Ordinal);
        Assert.Contains(":model-binding-allowed nil", body, StringComparison.Ordinal);
        Assert.Contains(":heartbeat-active nil", body, StringComparison.Ordinal);
        Assert.Contains(":cme-actual-admitted nil", body, StringComparison.Ordinal);
        Assert.Contains(":lisp-evaluation-allowed nil", body, StringComparison.Ordinal);
        Assert.DoesNotContain("(eval", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("(compile", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("(load", body, StringComparison.OrdinalIgnoreCase);
    }

    private static HighEnergyArticulationCandidateReceipt Declare(HighEnergyArticulationCandidateRequest request) =>
        new DefaultHighEnergyArticulationCandidateBoundaryValidator().Declare(request, TimestampUtc);

    private static HighEnergyArticulationCandidateRequest CreateRequest(
        ZedDeltaChamberFormationReceipt? source = null,
        IReadOnlyList<HighEnergyArticulationCandidate>? candidates = null,
        ProviderInterfaceObservationBoundary? observation = null,
        HiddenSubstrateNonClaimBoundary? nonClaim = null,
        CandidateNonBindingBoundary? nonBinding = null,
        int priorPassageCount = 23) =>
        new(
            SourceZedDeltaChamberReceipt: source ?? CreateSourceReceipt(),
            Candidates: candidates ?? CreateCandidates(),
            ObservationBoundary: observation ?? CreateObservation(),
            NonClaimBoundary: nonClaim ?? CreateNonClaim(),
            NonBindingBoundary: nonBinding ?? CreateNonBinding(),
            PriorPassageCount: priorPassageCount);

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

    private static ZedDeltaChamberFormationReceipt CreateSourceReceipt(
        bool refused = false,
        bool runtimeModelBound = false,
        bool heartbeatActive = false,
        bool cmeActualAdmitted = false)
    {
        var origin = new ZedDeltaOrigin(
            OriginHandle: "urn:san:zed-delta:origin:0-0-0",
            DeltaHandle: "urn:san:delta:live-origin",
            X: 0,
            Y: 0,
            Z: 0,
            LocalDeltaOrigin: true,
            ReviewOnly: true,
            ChamberOnly: true,
            GrantsAuthority: false,
            AdmitsContinuity: false,
            ActivatesHeartbeat: false);
        var standing = new ConditionalOperationalExpressionStanding(
            StandingHandle: "urn:san:zed-delta-standing:primary",
            OeHandle: "urn:san:oe:primary",
            ConditionalOeHandle: "urn:san:coe:primary",
            CmeActualIdHandle: "urn:san:cme-actual-id:candidate:primary",
            ZedDeltaOriginHandle: origin.OriginHandle,
            SourceSelectiveActionSurfaceHandle: "urn:san:selective-action:orientation-review",
            SourceDecisionHandle: "urn:san:steward-admissibility:decision",
            WitnessHandle: "urn:san:witness:coe:primary",
            CustodyOwner: "steward",
            ReviewOnly: true,
            ConditionalOnly: true,
            StandsAtZedDeltaOrigin: true,
            PreservesOeLineage: true,
            PreservesSelectedSurfaceLineage: true,
            CmeActualIdCandidateOnly: true,
            ReplacesOe: false,
            MutatesOe: false,
            AdmitsContinuity: false,
            GrantsAuthority: false,
            AdmitsCmeActual: false,
            ActivatesHeartbeat: false);
        var hold = new ConditionalSelfGelHold(
            HoldHandle: "urn:san:zed-delta-cselfgel-hold:primary",
            SelfGelHandle: "urn:san:selfgel:primary",
            ConditionalSelfGelHandle: "urn:san:cselfgel:primary",
            ConditionalOeHandle: standing.ConditionalOeHandle,
            CompassHandle: "urn:san:compass:zed-delta",
            ZedDeltaOriginHandle: origin.OriginHandle,
            WitnessHandle: "urn:san:witness:cselfgel:primary",
            CustodyOwner: "steward",
            ReviewOnly: true,
            ConditionalOnly: true,
            HeldByCompass: true,
            HoldsForLiveEc: true,
            PreservesSelfGelLineage: true,
            PreservesOeLineage: true,
            RequiresCooling: true,
            MutatesSelfGel: false,
            PromotesToSelfGel: false,
            AdmitsContinuity: false,
            GrantsAuthority: false,
            ActivatesHeartbeat: false);
        var closure = new MosCmosResidueClosureRoute(
            RouteHandle: "urn:san:zed-delta-mos-cmos-closure:primary",
            MosHandle: "urn:san:mos:self-store",
            CmosHandle: "urn:san:cmos:shadow-self-store",
            ConditionalSelfGelHandle: hold.ConditionalSelfGelHandle,
            ConditionalOeHandle: standing.ConditionalOeHandle,
            ZedDeltaOriginHandle: origin.OriginHandle,
            ResidueHandle: "urn:san:residue:primary",
            CoolingHandle: "urn:san:cooling:zed-delta",
            ReturnToPrimeHandle: "urn:san:return:prime",
            WitnessHandle: "urn:san:witness:mos-cmos:primary",
            ReviewOnly: true,
            ClosureRouteOnly: true,
            MayCloseUncooledResidue: true,
            ReturnsToPrimeState: true,
            PreservesMosLineage: true,
            PreservesCmosLineage: true,
            PreservesConditionalSelfGelLineage: true,
            WritesMos: false,
            WritesCmos: false,
            ResidueBecomesContinuity: false,
            ResidueBecomesAuthority: false,
            ActivatesHeartbeat: false);
        var telemetry = new GoaCgoaSoulFrameTelemetryRoute(
            RouteHandle: "urn:san:zed-delta-goa-cgoa-soulframe:primary",
            GoaHandle: "urn:san:goa:external-formation",
            CgoaHandle: "urn:san:cgoa:cryptic-control-plane",
            ListeningFrameHandle: "urn:san:listening-frame:external",
            SoulFrameHandle: "urn:san:soulframe:internal-telemetry",
            ExternalFormationHandle: "urn:san:formation:external",
            InternalTelemetryHandle: "urn:san:telemetry:internal",
            ConditionalOeHandle: standing.ConditionalOeHandle,
            ConditionalSelfGelHandle: hold.ConditionalSelfGelHandle,
            ZedDeltaOriginHandle: origin.OriginHandle,
            WitnessHandle: "urn:san:witness:goa-cgoa:primary",
            ReviewOnly: true,
            DuplexRouteOnly: true,
            ExternalFormationRoutesThroughCgoa: true,
            InternalTelemetryRoutesIntoSoulFrame: true,
            ListeningFrameWiredToSoulFrame: true,
            PreservesGoaLineage: true,
            PreservesCgoaLineage: true,
            PreservesSoulFrameLineage: true,
            CgoaGrantsControl: false,
            SoulFrameBecomesSelf: false,
            RouteAuthorizesAction: false,
            RouteAdmitsContinuity: false,
            ActivatesHeartbeat: false);
        var orientation = new CompassChamberOrientationBoundary(
            BoundaryCode: "zed-delta-compass-orientation-review-only",
            CompassHandle: "urn:san:compass:zed-delta",
            Present: true,
            ReviewOnly: true,
            OrientsChamber: true,
            HoldsConditionalSelfGel: true,
            CoordinatesConditionalOe: true,
            RequiresZedDeltaOrigin: true,
            RequiresWitness: true,
            RequiresCooling: true,
            AdmitsTruth: false,
            MutatesSelfGel: false,
            MutatesOe: false,
            GrantsAuthority: false,
            ActivatesHeartbeat: false);
        var nonActivation = new ZedDeltaChamberNonActivationBoundary(
            BoundaryLaw: "chamber formation is not heartbeat activation",
            ChamberMayForm: true,
            HeartbeatMayBeDescribed: true,
            HeartbeatMayActivate: false,
            CmeActualMayBeAdmitted: false,
            ModelMayBind: false,
            RuntimeMayStart: false,
            ActionMayExecute: false,
            ContinuityMayBeAdmitted: false,
            AuthorityMayBeGranted: false,
            OeMayBeReplaced: false,
            SelfGelMayBeMutated: false,
            MosCmosMayBeWritten: false,
            CgoaMayGrantControl: false,
            SoulFrameMayBecomeSelf: false,
            CompassMayAdmitTruth: false,
            LispMayEvaluate: false,
            PacketMayEmit: false,
            ReceiptMayReplay: false,
            PassageMayIncrement: false,
            ActivationMayProceed: false,
            RequiresWitness: true,
            RequiresCooling: true,
            RequiresAuthorityAbsence: true);

        var admitted = !refused;
        return new ZedDeltaChamberFormationReceipt(
            ReceiptHandle: admitted ? "urn:san:zed-delta-chamber:review:test" : "urn:san:zed-delta-chamber:refused:test",
            Disposition: admitted ? ZedDeltaChamberFormationDisposition.ChamberFormedCold : ZedDeltaChamberFormationDisposition.Refused,
            OutcomeCode: admitted ? "zed-delta-chamber-formed-review-only" : "refused",
            GovernanceTrace: "source zed delta chamber test receipt",
            SourceSelectiveActionSurfaceReceiptHandle: "urn:san:selective-lawful-action:review:test",
            Origin: origin,
            ConditionalOperationalExpressions: admitted ? [standing] : [],
            ConditionalSelfGelHolds: admitted ? [hold] : [],
            ResidueClosureRoutes: admitted ? [closure] : [],
            TelemetryRoutes: admitted ? [telemetry] : [],
            OrientationBoundary: orientation,
            NonActivationBoundary: nonActivation,
            Refusal: admitted ? null : new ZedDeltaChamberFormationRefusalReceipt("urn:san:zed-delta-chamber-refusal:test", "refused", "refused", true),
            PriorPassageCount: 23,
            PassageCountAfterChamberReview: 23,
            ConditionalOeStandingCount: admitted ? 1 : 0,
            ConditionalSelfGelHoldCount: admitted ? 1 : 0,
            ResidueClosureRouteCount: admitted ? 1 : 0,
            TelemetryRouteCount: admitted ? 1 : 0,
            ReviewOnly: true,
            ChamberOnly: true,
            ChamberFormed: admitted,
            CmeActualIdCandidateHeld: admitted,
            HeartbeatDescribed: true,
            HeartbeatActive: heartbeatActive,
            CmeActualAdmitted: cmeActualAdmitted,
            RuntimeModelBound: runtimeModelBound,
            RuntimeStarted: false,
            ActionAuthorized: false,
            ContinuityAdmitted: false,
            AuthorityGranted: false,
            OeReplaced: false,
            SelfGelMutated: false,
            MosCmosWritten: false,
            CgoaGrantedControl: false,
            SoulFrameBecameSelf: false,
            CompassAdmittedTruth: false,
            LispEvaluationAllowed: false,
            NewPacketEmitted: false,
            ReceiptsReplayed: false,
            PassageIncremented: false,
            ActivationRefused: true,
            TimestampUtc: TimestampUtc);
    }

    private static ProviderInterfaceObservationBoundary MutateObservation(
        ProviderInterfaceObservationBoundary boundary,
        string mutation) =>
        mutation switch
        {
            "missing-code" => boundary with { BoundaryCode = string.Empty },
            "not-present" => boundary with { Present = false },
            "not-review" => boundary with { ReviewOnly = false },
            "not-public" => boundary with { PublicObservableOnly = false },
            "no-docs" => boundary with { AllowsOfficialDocumentationReference = false },
            "no-api" => boundary with { AllowsPublishedApiContractReference = false },
            "no-behavior" => boundary with { AllowsObservableBehaviorStudy = false },
            "provider-call" => boundary with { AllowsProviderCall = true },
            "provider-access" => boundary with { AllowsProviderVisibleAccess = true },
            "context-export" => boundary with { AllowsModelContextExport = true },
            "scraping" => boundary with { AllowsScraping = true },
            "hidden-map" => boundary with { AllowsHiddenInternalsMapping = true },
            "weight-access" => boundary with { AllowsWeightAccess = true },
            "training-inference" => boundary with { AllowsTrainingDataInference = true },
            "memory-claim" => boundary with { AllowsPersistentMemoryClaim = true },
            "runtime-identity" => boundary with { AllowsRuntimeIdentityClaim = true },
            "authority" => boundary with { AllowsAuthority = true },
            _ => throw new ArgumentOutOfRangeException(nameof(mutation), mutation, null)
        };

    private static HiddenSubstrateNonClaimBoundary MutateNonClaim(
        HiddenSubstrateNonClaimBoundary boundary,
        string mutation) =>
        mutation switch
        {
            "missing-law" => boundary with { BoundaryLaw = string.Empty },
            "no-public-study" => boundary with { PublicInterfaceMayBeStudied = false },
            "hidden-claim" => boundary with { HiddenSubstrateMayBeClaimed = true },
            "internals" => boundary with { ProprietaryInternalsMayBeMapped = true },
            "weights" => boundary with { WeightsMayBeClaimed = true },
            "training-data" => boundary with { TrainingDataMayBeClaimed = true },
            "provider-logs" => boundary with { ProviderLogsMayBeClaimed = true },
            "system-prompt" => boundary with { SystemPromptMayBeClaimed = true },
            "causal-certainty" => boundary with { FullCausalCertaintyMayBeClaimed = true },
            "behavior-proof" => boundary with { ObservableBehaviorMayBecomeInternalProof = true },
            "documentation-proof" => boundary with { DocumentationMayBecomeImplementationProof = true },
            "success-warrant" => boundary with { InterfaceSuccessMayBecomeSemanticWarrant = true },
            "no-uncertainty" => boundary with { RequiresUncertaintyRetention = false },
            "no-attribution" => boundary with { RequiresSourceAttribution = false },
            "no-non-equivalence" => boundary with { RequiresNonEquivalenceClaim = false },
            _ => throw new ArgumentOutOfRangeException(nameof(mutation), mutation, null)
        };

    private static CandidateNonBindingBoundary MutateNonBinding(
        CandidateNonBindingBoundary boundary,
        string mutation) =>
        mutation switch
        {
            "missing-law" => boundary with { BoundaryLaw = string.Empty },
            "no-candidate" => boundary with { CandidateMayBeNamed = false },
            "no-role" => boundary with { RoleMayBeAssigned = false },
            "no-interface" => boundary with { InterfaceMayBeObserved = false },
            "model-binding" => boundary with { ModelMayBind = true },
            "provider-call" => boundary with { ProviderMayBeCalled = true },
            "heartbeat" => boundary with { HeartbeatMayActivate = true },
            "cme-actual" => boundary with { CmeActualMayBeAdmitted = true },
            "runtime" => boundary with { RuntimeMayStart = true },
            "action" => boundary with { ActionMayBeAuthorized = true },
            "continuity" => boundary with { ContinuityMayBeAdmitted = true },
            "authority" => boundary with { AuthorityMayBeGranted = true },
            "identity" => boundary with { IdentityMayMutate = true },
            "selfgel" => boundary with { SelfGelMayMutate = true },
            "lisp" => boundary with { LispMayEvaluate = true },
            "packet" => boundary with { PacketMayEmit = true },
            "replay" => boundary with { ReceiptMayReplay = true },
            "passage" => boundary with { PassageMayIncrement = true },
            "activation" => boundary with { ActivationMayProceed = true },
            "no-zed" => boundary with { RequiresZedDeltaChamber = false },
            "no-witness" => boundary with { RequiresWitness = false },
            "authority-present" => boundary with { RequiresAuthorityAbsence = false },
            _ => throw new ArgumentOutOfRangeException(nameof(mutation), mutation, null)
        };

    private static HighEnergyArticulationCandidate MutateCandidate(
        HighEnergyArticulationCandidate candidate,
        string mutation) =>
        mutation switch
        {
            "missing-handle" => candidate with { CandidateHandle = string.Empty },
            "missing-provider" => candidate with { ProviderFamily = string.Empty },
            "missing-model" => candidate with { ModelLine = string.Empty },
            "missing-role-description" => candidate with { IntendedRole = string.Empty },
            "missing-chamber" => candidate with { ZedDeltaChamberReceiptHandle = string.Empty },
            "missing-origin" => candidate with { ZedDeltaOriginHandle = string.Empty },
            "missing-coe" => candidate with { ConditionalOeHandle = string.Empty },
            "missing-cselfgel" => candidate with { ConditionalSelfGelHandle = string.Empty },
            "missing-telemetry" => candidate with { TelemetryShapeHandle = string.Empty },
            "missing-docs" => candidate with { PublicDocumentationHandle = string.Empty },
            "missing-witness" => candidate with { WitnessHandle = string.Empty },
            "missing-custody" => candidate with { CustodyOwner = string.Empty },
            "not-review" => candidate with { ReviewOnly = false },
            "not-candidate" => candidate with { CandidateOnly = false },
            "not-role-typed" => candidate with { RoleTyped = false },
            "not-public" => candidate with { PublicInterfaceOnly = false },
            "not-observable" => candidate with { ObservableBehaviorOnly = false },
            "no-chamber-lineage" => candidate with { PreservesChamberLineage = false },
            "no-coe-lineage" => candidate with { PreservesConditionalOeLineage = false },
            "no-cselfgel-lineage" => candidate with { PreservesConditionalSelfGelLineage = false },
            "provider-call" => candidate with { ProviderCallRequested = true },
            "model-binding" => candidate with { ModelBindingRequested = true },
            "hidden-substrate" => candidate with { HiddenSubstrateClaimed = true },
            "weights" => candidate with { WeightAccessClaimed = true },
            "training" => candidate with { TrainingDataClaimed = true },
            "memory" => candidate with { PersistentMemoryClaimed = true },
            "runtime-identity" => candidate with { RuntimeIdentityClaimed = true },
            "heartbeat" => candidate with { HeartbeatActivationRequested = true },
            "cme-actual" => candidate with { CmeActualAdmissionRequested = true },
            "action" => candidate with { ActionAuthorizationRequested = true },
            "continuity" => candidate with { ContinuityAdmissionRequested = true },
            "authority" => candidate with { AuthorityRequested = true },
            "lisp" => candidate with { LispEvaluationRequested = true },
            "packet" => candidate with { PacketEmissionRequested = true },
            "replay" => candidate with { ReceiptReplayRequested = true },
            "passage" => candidate with { PassageIncrementRequested = true },
            "activation" => candidate with { ActivationRequested = true },
            _ => throw new ArgumentOutOfRangeException(nameof(mutation), mutation, null)
        };

    private static void AssertCold(HighEnergyArticulationCandidateReceipt receipt)
    {
        Assert.True(receipt.IsColdHighEnergyArticulationCandidate);
        Assert.True(receipt.ReviewOnly);
        Assert.True(receipt.CandidateOnly);
        Assert.True(receipt.ActivationRefused);
        Assert.False(receipt.ProviderCallMade);
        Assert.False(receipt.ModelBound);
        Assert.False(receipt.HeartbeatActive);
        Assert.False(receipt.CmeActualAdmitted);
        Assert.False(receipt.ActionAuthorized);
        Assert.False(receipt.ContinuityAdmitted);
        Assert.False(receipt.AuthorityGranted);
    }

    private static void AssertRefused(HighEnergyArticulationCandidateReceipt receipt, string outcomeCode)
    {
        Assert.Equal(HighEnergyArticulationCandidateDisposition.Refused, receipt.Disposition);
        Assert.Equal(outcomeCode, receipt.OutcomeCode);
        Assert.True(receipt.IsRetainedHighEnergyArticulationCandidateRefusal);
        Assert.NotNull(receipt.Refusal);
        Assert.True(receipt.Refusal.Retained);
        Assert.Empty(receipt.Candidates);
        Assert.Equal(receipt.PriorPassageCount, receipt.PassageCountAfterCandidateReview);
        Assert.False(receipt.ProviderCallMade);
        Assert.False(receipt.ModelBound);
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
            var candidate = Path.Combine(directory.FullName, "src", "SLI", "SLI.Lisp", "high-energy-articulation-candidate.lisp");
            if (File.Exists(candidate))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        throw new DirectoryNotFoundException("Could not locate repository root from test output path.");
    }
}
