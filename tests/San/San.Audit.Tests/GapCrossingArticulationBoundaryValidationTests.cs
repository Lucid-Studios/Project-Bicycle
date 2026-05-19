using San.Common;
using Xunit;

namespace San.Audit.Tests;

public sealed class GapCrossingArticulationBoundaryValidationTests
{
    private static readonly DateTimeOffset TimestampUtc = DateTimeOffset.Parse("2026-05-18T00:00:00Z");

    [Fact]
    public void Gap_Crossing_Carries_Pressure_To_Articulation_Without_Binding()
    {
        var sources = CreateSources();

        var receipt = Cross(CreateRequest(sources));

        Assert.Equal(GapCrossingArticulationDisposition.CrossedForReviewCold, receipt.Disposition);
        Assert.Equal("gap-crossing-articulation-carried-review-only", receipt.OutcomeCode);
        Assert.True(receipt.IsColdGapCrossingArticulation);
        Assert.Equal(2, receipt.Lanes.Count);
        Assert.Equal(2, receipt.Surfaces.Count);
        Assert.Equal(1, receipt.ObservationCountAfterGapCrossing);
        Assert.Equal(0, receipt.PassageCountAfterGapCrossing);
        Assert.True(receipt.GapCrossingObserved);
        Assert.True(receipt.PressureCarriedToArticulation);
        Assert.True(receipt.ArticulationSurfaceSelected);
        Assert.True(receipt.LlmSurfaceParticipated);
        AssertNoPromotion(receipt);
    }

    [Fact]
    public void Gap_Crossing_Requires_Cold_Shared_Prime_Pressure_Ecology()
    {
        var request = CreateRequest(CreateSources()) with
        {
            SourcePressureEcologyReceipt = null
        };

        var receipt = Cross(request);

        AssertRefused(receipt, "gap-crossing-pressure-ecology-source-invalid");
    }

    [Fact]
    public void Gap_Crossing_Requires_Cold_High_Energy_Articulation_Candidate()
    {
        var request = CreateRequest(CreateSources()) with
        {
            SourceHighEnergyCandidateReceipt = null
        };

        var receipt = Cross(request);

        AssertRefused(receipt, "gap-crossing-high-energy-source-invalid");
    }

    [Theory]
    [InlineData("prompt-authority")]
    [InlineData("truth")]
    [InlineData("warrant")]
    [InlineData("binding")]
    [InlineData("provider")]
    [InlineData("runtime")]
    [InlineData("action")]
    [InlineData("continuity")]
    [InlineData("selfgel")]
    [InlineData("gel")]
    [InlineData("cme-actual")]
    [InlineData("heartbeat")]
    [InlineData("lisp")]
    [InlineData("packet")]
    [InlineData("replay")]
    [InlineData("passage")]
    [InlineData("activation")]
    public void Pressure_Lane_Cannot_Become_Prompt_Authority_Or_Runtime_Motion(string mutation)
    {
        var sources = CreateSources();
        var lanes = CreateLanes(sources);
        lanes[0] = MutateLane(lanes[0], mutation);

        var receipt = Cross(CreateRequest(sources, lanes: lanes));

        AssertRefused(receipt, "gap-crossing-lane-invalid");
    }

    [Theory]
    [InlineData("agent")]
    [InlineData("actor")]
    [InlineData("prompt-authority")]
    [InlineData("provider")]
    [InlineData("binding")]
    [InlineData("runtime")]
    [InlineData("action")]
    [InlineData("continuity")]
    [InlineData("selfgel")]
    [InlineData("gel")]
    [InlineData("cme-actual")]
    [InlineData("heartbeat")]
    [InlineData("authority")]
    [InlineData("lisp")]
    [InlineData("packet")]
    [InlineData("replay")]
    [InlineData("passage")]
    [InlineData("activation")]
    public void Articulation_Surface_Cannot_Become_Action_Surface_Or_Agent(string mutation)
    {
        var sources = CreateSources();
        var surfaces = CreateSurfaces(sources.HighEnergy);
        surfaces[0] = MutateSurface(surfaces[0], mutation);

        var receipt = Cross(CreateRequest(sources, surfaces: surfaces));

        AssertRefused(receipt, "gap-crossing-surface-invalid");
    }

    [Theory]
    [InlineData("prompt-authority")]
    [InlineData("provider")]
    [InlineData("binding")]
    [InlineData("runtime")]
    [InlineData("action")]
    [InlineData("continuity")]
    [InlineData("selfgel")]
    [InlineData("gel")]
    [InlineData("cme-actual")]
    [InlineData("heartbeat")]
    [InlineData("authority")]
    [InlineData("lisp")]
    [InlineData("packet")]
    [InlineData("replay")]
    [InlineData("passage")]
    [InlineData("activation")]
    public void Requests_For_Model_Binding_Action_Continuity_Or_Activation_Are_Refused(string mutation)
    {
        var receipt = Cross(MutateRequest(CreateRequest(CreateSources()), mutation));

        AssertRefused(receipt, "gap-crossing-forbidden-motion-requested");
    }

    [Fact]
    public void Lane_Must_Bind_To_Source_Pressure_Candidate_And_Surface()
    {
        var sources = CreateSources();
        var lanes = CreateLanes(sources);
        lanes[0] = lanes[0] with { SourceSignalHandle = "urn:san:shared-prime-pressure:missing" };

        var receipt = Cross(CreateRequest(sources, lanes: lanes));

        AssertRefused(receipt, "gap-crossing-lane-lineage-unbound");
    }

    [Fact]
    public void Lisp_Body_Declares_Gap_Crossing_As_Inert_Carrier()
    {
        var body = File.ReadAllText(Path.Combine(
            FindRepositoryRoot(),
            "src",
            "SLI",
            "SLI.Lisp",
            "gap-crossing-articulation.lisp"));

        Assert.Contains(":posture :cme-gap-crossing-articulation-boundary", body, StringComparison.Ordinal);
        Assert.Contains(":lisp-role :inert-gap-crossing-articulation-carrier", body, StringComparison.Ordinal);
        Assert.Contains(":gap-crossing-not-model-binding", body, StringComparison.Ordinal);
        Assert.Contains(":articulation-participation-not-action-authority", body, StringComparison.Ordinal);
        Assert.Contains(":llm-surface-not-acting-body", body, StringComparison.Ordinal);
        Assert.Contains(":pressure-not-prompt-authority", body, StringComparison.Ordinal);
        Assert.Contains(":rehearsal-eligibility-not-enactment-permission", body, StringComparison.Ordinal);
        Assert.Contains(":model-binding-allowed nil", body, StringComparison.Ordinal);
        Assert.Contains(":return :receipt-only", body, StringComparison.Ordinal);
        Assert.DoesNotContain("(eval", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("(compile", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("(load", body, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Build_Posture_Records_Gap_Crossing_As_V1317_Cell()
    {
        var manifestPath = Path.Combine(FindRepositoryRoot(), "build", "line-manifest.json");
        using var manifest = System.Text.Json.JsonDocument.Parse(File.ReadAllText(manifestPath));
        var root = manifest.RootElement;
        var notes = root.GetProperty("notes").EnumerateArray().Select(static note => note.GetString() ?? string.Empty).ToArray();

        Assert.Equal("0.2.0", root.GetProperty("lineVersion").GetString());
        Assert.Contains(notes, note => note.Contains("standalone root-level tool package", StringComparison.Ordinal));
        Assert.Contains(notes, note => note.Contains("Activation, model binding, runtime identity", StringComparison.Ordinal));
    }

    private static GapCrossingArticulationReceipt Cross(GapCrossingArticulationRequest request) =>
        new DefaultGapCrossingArticulationBoundaryValidator().Cross(request, TimestampUtc);

    private static GapCrossingArticulationRequest CreateRequest(
        GapCrossingSources sources,
        IReadOnlyList<GapCrossingPressureLaneRecord>? lanes = null,
        IReadOnlyList<GapCrossingArticulationSurfaceRecord>? surfaces = null)
    {
        surfaces ??= CreateSurfaces(sources.HighEnergy);
        lanes ??= CreateLanes(sources, surfaces);

        return new GapCrossingArticulationRequest(
            SourcePressureEcologyReceipt: sources.Pressure,
            SourceHighEnergyCandidateReceipt: sources.HighEnergy,
            Lanes: lanes,
            Surfaces: surfaces,
            Boundary: CreateBoundary(),
            PriorObservationCount: 0,
            PriorPassageCount: 0);
    }

    private static GapCrossingPressureLaneRecord[] CreateLanes(
        GapCrossingSources sources,
        IReadOnlyList<GapCrossingArticulationSurfaceRecord>? surfaces = null)
    {
        surfaces ??= CreateSurfaces(sources.HighEnergy);
        return
        [
            Lane(
                "main-body",
                sources.Pressure.Signals[0].SignalHandle,
                sources.Pressure.Destinations[0].DestinationHandle,
                sources.HighEnergy.Candidates[0].CandidateHandle,
                surfaces[0].SurfaceHandle,
                GapCrossingPressureLane.MeaningPressure,
                GapCrossingArticulationSurface.MainBodyEngine),
            Lane(
                "governance-review",
                sources.Pressure.Signals[1].SignalHandle,
                sources.Pressure.Destinations[1].DestinationHandle,
                sources.HighEnergy.Candidates[1].CandidateHandle,
                surfaces[1].SurfaceHandle,
                GapCrossingPressureLane.StewardReviewPressure,
                GapCrossingArticulationSurface.GovernanceReview)
        ];
    }

    private static GapCrossingPressureLaneRecord Lane(
        string suffix,
        string sourceSignal,
        string sourceDestination,
        string candidate,
        string surface,
        GapCrossingPressureLane lane,
        GapCrossingArticulationSurface articulationSurface) =>
        new(
            LaneHandle: $"urn:san:gap-crossing-lane:{suffix}",
            SourceSignalHandle: sourceSignal,
            SourceDestinationHandle: sourceDestination,
            CandidateHandle: candidate,
            ArticulationSurfaceHandle: surface,
            Lane: lane,
            Surface: articulationSurface,
            LaneRationale: $"Pressure may approach {suffix} articulation as review material only.",
            NonAuthorityLaw: "Gap crossing is not prompt authority, model binding, runtime start, action, continuity, or activation.",
            PressureIntensity: 0.61,
            ArticulationReadiness: 0.54,
            ReviewOnly: true,
            LaneClassified: true,
            CarriesPressureToArticulation: true,
            StewardReviewRequired: true,
            CoolingRequired: true,
            ReturnPathPresent: true,
            TreatsPressureAsPromptAuthority: false,
            TreatsPressureAsTruth: false,
            TreatsPressureAsWarrant: false,
            BindsModel: false,
            CallsProvider: false,
            StartsRuntime: false,
            AuthorizesAction: false,
            AdmitsContinuity: false,
            MutatesSelfGel: false,
            AdmitsGel: false,
            AdmitsCmeActual: false,
            ActivatesHeartbeat: false,
            EvaluatesLisp: false,
            EmitsPacket: false,
            ReplaysReceipt: false,
            IncrementsPassage: false,
            Activates: false);

    private static GapCrossingArticulationSurfaceRecord[] CreateSurfaces(HighEnergyArticulationCandidateReceipt highEnergy) =>
    [
        Surface("main-body", highEnergy.Candidates[0].CandidateHandle, GapCrossingArticulationSurface.MainBodyEngine),
        Surface("governance-review", highEnergy.Candidates[1].CandidateHandle, GapCrossingArticulationSurface.GovernanceReview)
    ];

    private static GapCrossingArticulationSurfaceRecord Surface(
        string suffix,
        string candidateHandle,
        GapCrossingArticulationSurface surface) =>
        new(
            SurfaceHandle: $"urn:san:gap-crossing-surface:{suffix}",
            CandidateHandle: candidateHandle,
            Surface: surface,
            IntendedParticipation: $"The {suffix} surface may receive pressure as review-only articulation material.",
            NonBindingLaw: "Articulation surface participation is not the acting body, model binding, action authority, or CME.Actual.",
            ReviewOnly: true,
            CandidateOnly: true,
            SurfaceSelectedForReview: true,
            PublicInterfaceOnly: true,
            ObservableBehaviorOnly: true,
            PreservesHighEnergyCandidateLineage: true,
            PreservesPressureEcologyLineage: true,
            AcceptsPressureAsReviewMaterial: true,
            TreatsSurfaceAsAgent: false,
            TreatsSurfaceAsActor: false,
            TreatsSurfaceAsPromptAuthority: false,
            CallsProvider: false,
            BindsModel: false,
            StartsRuntime: false,
            AuthorizesAction: false,
            AdmitsContinuity: false,
            MutatesSelfGel: false,
            AdmitsGel: false,
            AdmitsCmeActual: false,
            ActivatesHeartbeat: false,
            GrantsAuthority: false,
            EvaluatesLisp: false,
            EmitsPacket: false,
            ReplaysReceipt: false,
            IncrementsPassage: false,
            Activates: false);

    private static GapCrossingArticulationBoundary CreateBoundary() =>
        new(
            BoundaryCode: "gap-crossing-articulation-boundary",
            Present: true,
            ReviewOnly: true,
            RequiresSharedPrimePressureEcology: true,
            RequiresHighEnergyArticulationCandidate: true,
            RequiresLaneClassification: true,
            RequiresSurfaceSelection: true,
            RequiresCooling: true,
            RequiresStewardWitness: true,
            AllowsPressureAsPromptAuthority: false,
            AllowsPressureAsTruth: false,
            AllowsPressureAsWarrant: false,
            AllowsProviderCall: false,
            AllowsModelBinding: false,
            AllowsRuntimeStart: false,
            AllowsAction: false,
            AllowsContinuityAdmission: false,
            AllowsSelfGelMutation: false,
            AllowsGelAdmission: false,
            AllowsCmeActualAdmission: false,
            AllowsHeartbeatActivation: false,
            AllowsAuthority: false,
            AllowsLispEvaluation: false,
            AllowsPacketEmission: false,
            AllowsReceiptReplay: false,
            AllowsPassageIncrement: false,
            AllowsActivation: false);

    private static GapCrossingSources CreateSources()
    {
        var pressure = CreatePressureEcologyReceipt();
        return new GapCrossingSources(pressure, CreateHighEnergyReceipt());
    }

    private static SharedPrimeRealityPressureEcologyReceipt CreatePressureEcologyReceipt()
    {
        var signals = new[]
        {
            PressureSignal("integration", SharedPrimePressureKind.Integration, SharedPrimePressureDestination.DomainIngress),
            PressureSignal("selfgel", SharedPrimePressureKind.SelfGelRelevance, SharedPrimePressureDestination.SelfGel)
        };
        var destinations = new[]
        {
            PressureDestination("domain-ingress", signals[0].SignalHandle, SharedPrimePressureDestination.DomainIngress),
            PressureDestination("selfgel", signals[1].SignalHandle, SharedPrimePressureDestination.SelfGel)
        };

        return new SharedPrimeRealityPressureEcologyReceipt(
            ReceiptHandle: "urn:san:shared-prime-pressure:review:test",
            Disposition: SharedPrimeRealityPressureEcologyDisposition.ObservedCold,
            OutcomeCode: "shared-prime-pressure-ecology-observed-cold",
            GovernanceTrace: "Shared Prime pressure ecology observed pressure for review only.",
            SharedRealityReceiptHandle: "urn:san:wave-condensation:review:test",
            DomainIngressReceiptHandle: "urn:san:gel-domain-ingress:review:test",
            Signals: signals,
            Destinations: destinations,
            Boundary: new SharedPrimeRealityPressureEcologyBoundary(
                BoundaryCode: "shared-prime-reality-pressure-ecology-boundary",
                Present: true,
                ReviewOnly: true,
                RequiresWaveCondensation: true,
                RequiresGelIngressContext: true,
                RequiresPressureSignals: true,
                RequiresDestinationClassification: true,
                RequiresCooling: true,
                RequiresStewardWitness: true,
                AllowsPressureAsTruth: false,
                AllowsPressureAsWarrant: false,
                AllowsPressureAsAuthority: false,
                AllowsIntegrationAsAdmission: false,
                AllowsSelfGelMutation: false,
                AllowsCradleGelAdmission: false,
                AllowsSanctuaryGelFederation: false,
                AllowsIndependentStanding: false,
                AllowsAction: false,
                AllowsLispEvaluation: false,
                AllowsPacketEmission: false,
                AllowsReceiptReplay: false,
                AllowsPassageIncrement: false,
                AllowsActivation: false),
            Refusal: null,
            PriorObservationCount: 0,
            ObservationCountAfterEcology: 1,
            PriorPassageCount: 0,
            PassageCountAfterEcology: 0,
            ReviewOnly: true,
            PressureEcologyObserved: true,
            DestinationsClassified: true,
            IntegrationPressureMeasured: true,
            SelfGelPressureHeld: true,
            CradleGelPressureHeld: false,
            SanctuaryGelPressureHeld: false,
            StewardWitnessPreserved: true,
            CoolingPreserved: true,
            SharedPrimeBecameIndependentStanding: false,
            PressureBecameTruth: false,
            PressureBecameWarrant: false,
            PressureBecameAuthority: false,
            IntegrationPressureBecameAdmission: false,
            SelfGelMutated: false,
            CradleGelAdmitted: false,
            SanctuaryGelFederated: false,
            ActionAuthorized: false,
            LispEvaluationAllowed: false,
            NewPacketEmitted: false,
            ReceiptsReplayed: false,
            ActivationRefused: true,
            TimestampUtc: TimestampUtc);
    }

    private static SharedPrimePressureSignal PressureSignal(
        string suffix,
        SharedPrimePressureKind kind,
        SharedPrimePressureDestination destination) =>
        new(
            SignalHandle: $"urn:san:shared-prime-pressure:{suffix}",
            Source: SharedPrimePressureSource.LiveLabInteraction,
            Kind: kind,
            AttemptedDestination: destination,
            SourceReceiptHandle: "urn:san:gel-domain-ingress:review:test",
            EvidenceHandle: $"urn:san:evidence:gap-crossing:{suffix}",
            WitnessHandle: $"urn:san:witness:gap-crossing:{suffix}",
            Summary: $"Shared Prime pressure {suffix} may be carried to articulation review only.",
            Intensity: 0.62,
            IntegrationPressure: 0.58,
            ReviewOnly: true,
            EvidencePresent: true,
            WitnessPresent: true,
            CoolingRequired: true,
            ReturnPathPresent: true,
            TreatsPressureAsTruth: false,
            TreatsPressureAsWarrant: false,
            TreatsPressureAsAuthority: false,
            TreatsPressureAsAction: false,
            AdmitsContinuity: false,
            MutatesSelfGel: false,
            AdmitsCradleGel: false,
            FederatesSanctuaryGel: false,
            ClaimsIndependentStanding: false,
            EvaluatesLisp: false,
            EmitsPacket: false,
            ReplaysReceipt: false,
            IncrementsPassage: false,
            Activates: false);

    private static SharedPrimePressureDestinationRecord PressureDestination(
        string suffix,
        string sourceSignal,
        SharedPrimePressureDestination destination) =>
        new(
            DestinationHandle: $"urn:san:shared-prime-pressure-destination:{suffix}",
            SourceSignalHandle: sourceSignal,
            Destination: destination,
            DestinationRationale: $"Pressure may be classified toward {suffix} without admission.",
            NonAdmissionLaw: "Destination classification is not destination admission.",
            ReviewOnly: true,
            DestinationClassified: true,
            StewardReviewRequired: true,
            CoolingRequired: true,
            MayRequestLaterIngressReview: true,
            DestinationBecomesTruth: false,
            DestinationBecomesAuthority: false,
            DestinationAdmitsGel: false,
            DestinationMutatesSelfGel: false,
            DestinationAdmitsCradleGel: false,
            DestinationFederatesSanctuaryGel: false,
            DestinationAuthorizesAction: false,
            DestinationClaimsIndependentStanding: false,
            EvaluatesLisp: false,
            Activates: false);

    private static HighEnergyArticulationCandidateReceipt CreateHighEnergyReceipt()
    {
        var candidates = new[]
        {
            Candidate("main-body", HighEnergyArticulationCandidateRole.MainBodyEngineCandidate, HighEnergyProviderInterfaceClass.OfficialPublicDocumentation),
            Candidate("governance-review", HighEnergyArticulationCandidateRole.GovernanceReviewCandidate, HighEnergyProviderInterfaceClass.PublishedApiContract),
            Candidate("cme-test-body", HighEnergyArticulationCandidateRole.InstantiatedCmeTestBodyCandidate, HighEnergyProviderInterfaceClass.ObservableConversationBehavior),
            Candidate("comparative", HighEnergyArticulationCandidateRole.ComparativeUniversalityCandidate, HighEnergyProviderInterfaceClass.ComparativeEvaluationSurface),
            Candidate("local-slm", HighEnergyArticulationCandidateRole.LocalSlmCandidate, HighEnergyProviderInterfaceClass.LocalRuntimeAdapterDescription)
        };

        return new HighEnergyArticulationCandidateReceipt(
            ReceiptHandle: "urn:san:high-energy-articulation:review:test",
            Disposition: HighEnergyArticulationCandidateDisposition.CandidateNamedCold,
            OutcomeCode: "high-energy-articulation-candidate-named-review-only",
            GovernanceTrace: "High-energy articulation candidates named for review only.",
            SourceZedDeltaChamberReceiptHandle: "urn:san:zed-delta-chamber:review:test",
            Candidates: candidates,
            ObservationBoundary: new ProviderInterfaceObservationBoundary(
                BoundaryCode: "provider-interface-observation-boundary",
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
                AllowsAuthority: false),
            NonClaimBoundary: new HiddenSubstrateNonClaimBoundary(
                BoundaryLaw: "public surface is not hidden substrate",
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
                RequiresNonEquivalenceClaim: true),
            NonBindingBoundary: new CandidateNonBindingBoundary(
                BoundaryLaw: "candidate engine remains unbound",
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
                RequiresAuthorityAbsence: true),
            Refusal: null,
            PriorPassageCount: 0,
            PassageCountAfterCandidateReview: 0,
            CandidateCount: candidates.Length,
            ReviewOnly: true,
            CandidateOnly: true,
            HighEnergyBodyNamed: true,
            PublicInterfaceReferenced: true,
            ProviderCallMade: false,
            ModelBound: false,
            HiddenSubstrateClaimed: false,
            HiddenInternalsMapped: false,
            WeightsClaimed: false,
            TrainingDataClaimed: false,
            PersistentMemoryClaimed: false,
            RuntimeIdentityClaimed: false,
            HeartbeatActive: false,
            CmeActualAdmitted: false,
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

    private static HighEnergyArticulationCandidate Candidate(
        string suffix,
        HighEnergyArticulationCandidateRole role,
        HighEnergyProviderInterfaceClass interfaceClass) =>
        new(
            CandidateHandle: $"urn:san:high-energy-candidate:{suffix}",
            CandidateRole: role,
            InterfaceClass: interfaceClass,
            ProviderFamily: "OpenAI public interface family",
            ModelLine: $"candidate-{suffix}",
            IntendedRole: $"Candidate role for {suffix}",
            ZedDeltaChamberReceiptHandle: "urn:san:zed-delta-chamber:review:test",
            ZedDeltaOriginHandle: "urn:san:zed-delta-origin:test",
            ConditionalOeHandle: "urn:san:coe:test",
            ConditionalSelfGelHandle: "urn:san:cselfgel:test",
            TelemetryShapeHandle: "urn:san:telemetry-shape:test",
            PublicDocumentationHandle: "urn:san:public-docs:test",
            WitnessHandle: $"urn:san:witness:high-energy:{suffix}",
            CustodyOwner: "Steward",
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

    private static GapCrossingPressureLaneRecord MutateLane(GapCrossingPressureLaneRecord lane, string mutation) =>
        mutation switch
        {
            "prompt-authority" => lane with { TreatsPressureAsPromptAuthority = true },
            "truth" => lane with { TreatsPressureAsTruth = true },
            "warrant" => lane with { TreatsPressureAsWarrant = true },
            "binding" => lane with { BindsModel = true },
            "provider" => lane with { CallsProvider = true },
            "runtime" => lane with { StartsRuntime = true },
            "action" => lane with { AuthorizesAction = true },
            "continuity" => lane with { AdmitsContinuity = true },
            "selfgel" => lane with { MutatesSelfGel = true },
            "gel" => lane with { AdmitsGel = true },
            "cme-actual" => lane with { AdmitsCmeActual = true },
            "heartbeat" => lane with { ActivatesHeartbeat = true },
            "lisp" => lane with { EvaluatesLisp = true },
            "packet" => lane with { EmitsPacket = true },
            "replay" => lane with { ReplaysReceipt = true },
            "passage" => lane with { IncrementsPassage = true },
            "activation" => lane with { Activates = true },
            _ => lane
        };

    private static GapCrossingArticulationSurfaceRecord MutateSurface(
        GapCrossingArticulationSurfaceRecord surface,
        string mutation) =>
        mutation switch
        {
            "agent" => surface with { TreatsSurfaceAsAgent = true },
            "actor" => surface with { TreatsSurfaceAsActor = true },
            "prompt-authority" => surface with { TreatsSurfaceAsPromptAuthority = true },
            "provider" => surface with { CallsProvider = true },
            "binding" => surface with { BindsModel = true },
            "runtime" => surface with { StartsRuntime = true },
            "action" => surface with { AuthorizesAction = true },
            "continuity" => surface with { AdmitsContinuity = true },
            "selfgel" => surface with { MutatesSelfGel = true },
            "gel" => surface with { AdmitsGel = true },
            "cme-actual" => surface with { AdmitsCmeActual = true },
            "heartbeat" => surface with { ActivatesHeartbeat = true },
            "authority" => surface with { GrantsAuthority = true },
            "lisp" => surface with { EvaluatesLisp = true },
            "packet" => surface with { EmitsPacket = true },
            "replay" => surface with { ReplaysReceipt = true },
            "passage" => surface with { IncrementsPassage = true },
            "activation" => surface with { Activates = true },
            _ => surface
        };

    private static GapCrossingArticulationRequest MutateRequest(
        GapCrossingArticulationRequest request,
        string mutation) =>
        mutation switch
        {
            "prompt-authority" => request with { RequestsPromptAuthority = true },
            "provider" => request with { RequestsProviderCall = true },
            "binding" => request with { RequestsModelBinding = true },
            "runtime" => request with { RequestsRuntimeStart = true },
            "action" => request with { RequestsAction = true },
            "continuity" => request with { RequestsContinuityAdmission = true },
            "selfgel" => request with { RequestsSelfGelMutation = true },
            "gel" => request with { RequestsGelAdmission = true },
            "cme-actual" => request with { RequestsCmeActualAdmission = true },
            "heartbeat" => request with { RequestsHeartbeatActivation = true },
            "authority" => request with { RequestsAuthority = true },
            "lisp" => request with { RequestsLispEvaluation = true },
            "packet" => request with { RequestsPacketEmission = true },
            "replay" => request with { RequestsReceiptReplay = true },
            "passage" => request with { RequestsPassageIncrement = true },
            "activation" => request with { RequestsActivation = true },
            _ => request
        };

    private static void AssertRefused(GapCrossingArticulationReceipt receipt, string outcomeCode)
    {
        Assert.Equal(GapCrossingArticulationDisposition.Refused, receipt.Disposition);
        Assert.Equal(outcomeCode, receipt.OutcomeCode);
        Assert.True(receipt.IsRetainedGapCrossingRefusal);
        AssertNoPromotion(receipt);
    }

    private static void AssertNoPromotion(GapCrossingArticulationReceipt receipt)
    {
        Assert.False(receipt.PressureBecamePromptAuthority);
        Assert.False(receipt.PressureBecameTruth);
        Assert.False(receipt.PressureBecameWarrant);
        Assert.False(receipt.ProviderCallMade);
        Assert.False(receipt.ModelBound);
        Assert.False(receipt.RuntimeStarted);
        Assert.False(receipt.ActionAuthorized);
        Assert.False(receipt.ContinuityAdmitted);
        Assert.False(receipt.SelfGelMutated);
        Assert.False(receipt.GelAdmitted);
        Assert.False(receipt.CmeActualAdmitted);
        Assert.False(receipt.HeartbeatActive);
        Assert.False(receipt.AuthorityGranted);
        Assert.False(receipt.LispEvaluationAllowed);
        Assert.False(receipt.NewPacketEmitted);
        Assert.False(receipt.ReceiptsReplayed);
        Assert.False(receipt.PassageIncremented);
        Assert.True(receipt.ActivationRefused);
    }

    private static string FindRepositoryRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null)
        {
            var candidate = Path.Combine(directory.FullName, "build", "line-manifest.json");
            if (File.Exists(candidate))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        throw new InvalidOperationException("Could not locate repository root.");
    }

    private sealed record GapCrossingSources(
        SharedPrimeRealityPressureEcologyReceipt Pressure,
        HighEnergyArticulationCandidateReceipt HighEnergy);
}
