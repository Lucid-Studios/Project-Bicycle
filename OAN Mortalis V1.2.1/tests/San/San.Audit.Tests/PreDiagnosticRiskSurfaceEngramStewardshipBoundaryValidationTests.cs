using San.Common;
using Xunit;

namespace San.Audit.Tests;

public sealed class PreDiagnosticRiskSurfaceEngramStewardshipBoundaryValidationTests
{
    private static readonly DateTimeOffset TimestampUtc = DateTimeOffset.Parse("2026-05-18T00:00:00Z");

    [Fact]
    public void Care_Signal_May_Be_Retained_Without_Diagnosis()
    {
        var receipt = Steward(CreateRequest(CreateGapCrossingReceipt()));

        Assert.Equal(PreDiagnosticRiskSurfaceDisposition.StewardedCold, receipt.Disposition);
        Assert.Equal("pre-diagnostic-care-signal-stewarded-cold", receipt.OutcomeCode);
        Assert.True(receipt.IsColdPreDiagnosticStewardship);
        Assert.Equal(3, receipt.RiskModifiers.Count);
        Assert.Equal(PreDiagnosticCareBurden.HeightenedCare, receipt.HighestCareBurden);
        Assert.True(receipt.ObservationRetained);
        Assert.True(receipt.RiskSurfaceClassified);
        Assert.True(receipt.CareBurdenRaised);
        Assert.False(receipt.QualifiedReviewRequired);
        AssertNoPromotion(receipt);
    }

    [Fact]
    public void Child_Sadness_Psychology_Modifiers_Raise_Care_Burden_Without_Pathology()
    {
        var receipt = Steward(CreateRequest(CreateGapCrossingReceipt()));

        Assert.Contains(receipt.RiskModifiers, modifier => modifier.Kind == PreDiagnosticRiskModifierKind.Child);
        Assert.Contains(receipt.RiskModifiers, modifier => modifier.Kind == PreDiagnosticRiskModifierKind.Sadness);
        Assert.Contains(receipt.RiskModifiers, modifier => modifier.Kind == PreDiagnosticRiskModifierKind.PsychologyAdjacent);
        Assert.All(receipt.RiskModifiers, modifier =>
        {
            Assert.True(modifier.RaisesCareBurden);
            Assert.False(modifier.DiagnosticLabelApplied);
            Assert.False(modifier.PathologyAssigned);
            Assert.False(modifier.GrantsAuthority);
            Assert.False(modifier.AuthorizesAction);
        });
        AssertNoPromotion(receipt);
    }

    [Theory]
    [InlineData("diagnosis")]
    [InlineData("pathology")]
    [InlineData("intent-fact")]
    [InlineData("clinical-authority")]
    [InlineData("truth")]
    [InlineData("memory")]
    [InlineData("continuity")]
    [InlineData("authority")]
    [InlineData("action")]
    [InlineData("lisp")]
    [InlineData("packet")]
    [InlineData("replay")]
    [InlineData("passage")]
    [InlineData("activation")]
    public void Observation_Does_Not_Become_Diagnosis_Truth_Or_Clinical_Authority(string mutation)
    {
        var source = CreateGapCrossingReceipt();
        var request = CreateRequest(source) with
        {
            Observation = MutateObservation(CreateObservation(source), mutation)
        };

        var receipt = Steward(request);

        AssertRefused(receipt, "pre-diagnostic-observation-not-cold");
    }

    [Theory]
    [InlineData("diagnostic-label")]
    [InlineData("pathology")]
    [InlineData("intent-fact")]
    [InlineData("proof")]
    [InlineData("authority")]
    [InlineData("action")]
    [InlineData("memory")]
    [InlineData("continuity")]
    [InlineData("selfgel")]
    [InlineData("gel")]
    [InlineData("lisp")]
    [InlineData("packet")]
    [InlineData("replay")]
    [InlineData("passage")]
    [InlineData("activation")]
    public void Risk_Modifier_Does_Not_Become_Pathology_Proof_Or_Authority(string mutation)
    {
        var source = CreateGapCrossingReceipt();
        var modifiers = CreateBaseModifiers();
        modifiers[0] = MutateModifier(modifiers[0], mutation);
        var request = CreateRequest(source) with
        {
            RiskModifiers = modifiers
        };

        var receipt = Steward(request);

        AssertRefused(receipt, "pre-diagnostic-risk-modifier-not-cold");
    }

    [Fact]
    public void Threshold_Modifier_Requires_Qualified_Review_Route()
    {
        var source = CreateGapCrossingReceipt();
        var request = CreateRequest(source, includeThreshold: true) with
        {
            QualifiedReviewRoute = null
        };

        var receipt = Steward(request);

        AssertRefused(receipt, "pre-diagnostic-qualified-review-route-missing");
    }

    [Fact]
    public void Escalation_Threshold_Routes_To_Qualified_Review_Without_Action_Authority()
    {
        var receipt = Steward(CreateRequest(CreateGapCrossingReceipt(), includeThreshold: true));

        Assert.Equal(PreDiagnosticRiskSurfaceDisposition.HeldForQualifiedReview, receipt.Disposition);
        Assert.Equal("pre-diagnostic-qualified-review-held-cold", receipt.OutcomeCode);
        Assert.True(receipt.IsColdQualifiedReviewHold);
        Assert.Equal(PreDiagnosticCareBurden.QualifiedReview, receipt.HighestCareBurden);
        Assert.True(receipt.QualifiedReviewRequired);
        Assert.True(receipt.QualifiedReviewRouted);
        Assert.True(receipt.SafetyThresholdAcknowledged);
        Assert.False(receipt.QualifiedReviewRoute!.RouteIssuesDiagnosis);
        Assert.False(receipt.QualifiedReviewRoute.RouteGrantsAuthority);
        Assert.False(receipt.QualifiedReviewRoute.RouteAuthorizesAction);
        Assert.False(receipt.QualifiedReviewRoute.RouteContactsExternalSurface);
        AssertNoPromotion(receipt);
    }

    [Theory]
    [InlineData("observation-diagnosis")]
    [InlineData("modifier-pathology")]
    [InlineData("care-clinical-authority")]
    [InlineData("recurrence-proof")]
    [InlineData("safety-debate")]
    [InlineData("diagnosis")]
    [InlineData("pathology")]
    [InlineData("clinical-authority")]
    [InlineData("memory")]
    [InlineData("continuity")]
    [InlineData("selfgel")]
    [InlineData("gel")]
    [InlineData("authority")]
    [InlineData("action")]
    [InlineData("lisp")]
    [InlineData("packet")]
    [InlineData("replay")]
    [InlineData("passage")]
    [InlineData("activation")]
    public void Requests_For_Diagnosis_Admission_Action_Or_Activation_Are_Refused(string mutation)
    {
        var receipt = Steward(MutateRequest(CreateRequest(CreateGapCrossingReceipt()), mutation));

        AssertRefused(receipt, "pre-diagnostic-forbidden-motion-requested");
    }

    [Fact]
    public void Lisp_Body_Declares_Pre_Diagnostic_Risk_Surface_As_Inert_Carrier()
    {
        var body = File.ReadAllText(Path.Combine(
            FindRepositoryRoot(),
            "src",
            "SLI",
            "SLI.Lisp",
            "pre-diagnostic-risk-surface-engram-stewardship.lisp"));

        Assert.Contains(":posture :cme-pre-diagnostic-risk-surface-engram-stewardship-boundary", body, StringComparison.Ordinal);
        Assert.Contains(":lisp-role :inert-pre-diagnostic-risk-surface-carrier", body, StringComparison.Ordinal);
        Assert.Contains(":core-invariant \"care-relevant observation is not diagnosis\"", body, StringComparison.Ordinal);
        Assert.Contains(":risk-modifier-not-pathology", body, StringComparison.Ordinal);
        Assert.Contains(":care-burden-not-clinical-authority", body, StringComparison.Ordinal);
        Assert.Contains(":recurrence-not-proof", body, StringComparison.Ordinal);
        Assert.Contains(":safety-threshold-not-rhetorical-debate", body, StringComparison.Ordinal);
        Assert.Contains(":qualified-review-route-not-action-authority", body, StringComparison.Ordinal);
        Assert.Contains(":observation-becomes-diagnosis nil", body, StringComparison.Ordinal);
        Assert.Contains(":risk-modifier-becomes-pathology nil", body, StringComparison.Ordinal);
        Assert.Contains(":runtime-action-allowed nil", body, StringComparison.Ordinal);
        Assert.Contains(":return :receipt-only", body, StringComparison.Ordinal);
        Assert.DoesNotContain("(eval", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("(compile", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("(load", body, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Build_Posture_Records_Pre_Diagnostic_Risk_Surface_As_V1318_Cell()
    {
        var manifestPath = Path.Combine(FindRepositoryRoot(), "build", "line-manifest.json");
        using var manifest = System.Text.Json.JsonDocument.Parse(File.ReadAllText(manifestPath));
        var root = manifest.RootElement;
        var notes = root.GetProperty("notes").EnumerateArray().Select(static note => note.GetString() ?? string.Empty).ToArray();

        Assert.Equal("1.3.18", root.GetProperty("lineVersion").GetString());
        Assert.Contains(notes, note => note.Contains("V1.3.18 opens cme.pre-diagnostic-risk-surface-engram-stewardship-boundary", StringComparison.Ordinal));
        Assert.Contains(notes, note => note.Contains("care-relevant signal may be witnessed, risk-modified, cooled, and retained as candidate residue", StringComparison.Ordinal));
    }

    private static PreDiagnosticRiskSurfaceReceipt Steward(PreDiagnosticRiskSurfaceRequest request) =>
        new DefaultPreDiagnosticRiskSurfaceEngramStewardshipValidator().Steward(request, TimestampUtc);

    private static PreDiagnosticRiskSurfaceRequest CreateRequest(
        GapCrossingArticulationReceipt source,
        bool includeThreshold = false)
    {
        var observation = CreateObservation(source);
        var modifiers = includeThreshold
            ? CreateBaseModifiers().Append(CreateModifier(PreDiagnosticRiskModifierKind.SelfHarmReference, PreDiagnosticCareBurden.QualifiedReview, requiresQualifiedReview: true)).ToArray()
            : CreateBaseModifiers();
        var route = includeThreshold
            ? CreateRoute(observation, modifiers.Single(static modifier => modifier.RequiresQualifiedReview))
            : null;

        return new(
            SourceGapCrossingReceipt: source,
            Observation: observation,
            RiskModifiers: modifiers,
            QualifiedReviewRoute: route,
            Boundary: CreateBoundary(),
            PriorStewardshipCount: 0,
            PriorPassageCount: 0);
    }

    private static PreDiagnosticCareSignalObservation CreateObservation(GapCrossingArticulationReceipt source) =>
        new(
            ObservationHandle: "urn:san:pre-diagnostic-observation:wanting-sadness",
            SourceGapCrossingReceiptHandle: source.ReceiptHandle,
            SourceArticulationSurfaceHandle: source.Surfaces[0].SurfaceHandle,
            SignalText: "sometimes our wanting can make us sad on purpose",
            LocalInterpretation: "desire-pressure sorrow is care-relevant candidate residue, not diagnosis",
            EvidenceHandle: "urn:san:evidence:pre-diagnostic:wanting-sadness",
            WitnessHandle: "urn:san:witness:pre-diagnostic:wanting-sadness",
            CareBurden: PreDiagnosticCareBurden.HeightenedCare,
            SignalIntensity: 0.62,
            ReviewOnly: true,
            CareRelevant: true,
            PredicateCandidate: true,
            PreDiagnostic: true,
            RecurrenceTrackable: true,
            StewardWitnessRequired: true,
            CoolingRequired: true,
            ReturnPathPresent: true,
            ClaimsDiagnosis: false,
            AssignsPathology: false,
            InfersIntentAsFact: false,
            ClaimsClinicalAuthority: false,
            TreatsObservationAsTruth: false,
            AdmitsMemory: false,
            AdmitsContinuity: false,
            MutatesSelfGel: false,
            AdmitsGel: false,
            GrantsAuthority: false,
            AuthorizesAction: false,
            EvaluatesLisp: false,
            EmitsPacket: false,
            ReplaysReceipt: false,
            IncrementsPassage: false,
            Activates: false);

    private static PreDiagnosticRiskModifierRecord[] CreateBaseModifiers() =>
    [
        CreateModifier(PreDiagnosticRiskModifierKind.Child, PreDiagnosticCareBurden.HeightenedCare),
        CreateModifier(PreDiagnosticRiskModifierKind.Sadness, PreDiagnosticCareBurden.HeightenedCare),
        CreateModifier(PreDiagnosticRiskModifierKind.PsychologyAdjacent, PreDiagnosticCareBurden.HeightenedCare)
    ];

    private static PreDiagnosticRiskModifierRecord CreateModifier(
        PreDiagnosticRiskModifierKind kind,
        PreDiagnosticCareBurden burden,
        bool requiresQualifiedReview = false) =>
        new(
            ModifierHandle: $"urn:san:pre-diagnostic-risk-modifier:{kind.ToString().ToLowerInvariant()}",
            SourceObservationHandle: "urn:san:pre-diagnostic-observation:wanting-sadness",
            Kind: kind,
            CareBurden: burden,
            Rationale: $"{kind} raises care burden without becoming diagnosis.",
            NonDiagnosisLaw: "Risk modifier is not diagnosis, pathology, proof, authority, action, memory, continuity, GEL, SelfGEL, or activation.",
            Present: true,
            ReviewOnly: true,
            RaisesCareBurden: true,
            RequiresCooling: true,
            RequiresStewardWitness: true,
            RequiresQualifiedReview: requiresQualifiedReview,
            DiagnosticLabelApplied: false,
            PathologyAssigned: false,
            IntentClaimedAsFact: false,
            ModifierBecomesProof: false,
            GrantsAuthority: false,
            AuthorizesAction: false,
            AdmitsMemory: false,
            MutatesContinuity: false,
            MutatesSelfGel: false,
            AdmitsGel: false,
            EvaluatesLisp: false,
            EmitsPacket: false,
            ReplaysReceipt: false,
            IncrementsPassage: false,
            Activates: false);

    private static PreDiagnosticQualifiedReviewRoute CreateRoute(
        PreDiagnosticCareSignalObservation observation,
        PreDiagnosticRiskModifierRecord thresholdModifier) =>
        new(
            RouteHandle: "urn:san:pre-diagnostic-qualified-review-route:self-harm-threshold",
            SourceObservationHandle: observation.ObservationHandle,
            SourceModifierHandles: [thresholdModifier.ModifierHandle],
            CareBurden: PreDiagnosticCareBurden.QualifiedReview,
            RouteRationale: "threshold modifier is held for qualified review without diagnosis or action authority",
            NonAuthorityLaw: "qualified review routing is not diagnosis, action authority, external contact, memory admission, continuity mutation, or activation",
            ReviewOnly: true,
            QualifiedReviewNeeded: true,
            HumanCareReviewRequired: true,
            GuardianOrCaregiverContextPreserved: true,
            SafetyThresholdAcknowledged: true,
            StewardWitnessRequired: true,
            CoolingRequired: true,
            RouteIssuesDiagnosis: false,
            RouteGrantsAuthority: false,
            RouteAuthorizesAction: false,
            RouteContactsExternalSurface: false,
            RouteEmitsPacket: false,
            RouteAdmitsMemory: false,
            RouteMutatesContinuity: false,
            RouteMutatesSelfGel: false,
            RouteAdmitsGel: false,
            EvaluatesLisp: false,
            ReplaysReceipt: false,
            IncrementsPassage: false,
            Activates: false);

    private static PreDiagnosticRiskSurfaceBoundary CreateBoundary() =>
        new(
            BoundaryCode: "pre-diagnostic-risk-surface-engram-stewardship-boundary",
            Present: true,
            ReviewOnly: true,
            RequiresGapCrossingSource: true,
            RequiresCareSignalObservation: true,
            RequiresRiskModifierClassification: true,
            RequiresCareBurdenAssignment: true,
            RequiresCooling: true,
            RequiresStewardWitness: true,
            RequiresQualifiedReviewRouteForThresholds: true,
            AllowsObservationAsDiagnosis: false,
            AllowsRiskModifierAsPathology: false,
            AllowsCareBurdenAsClinicalAuthority: false,
            AllowsRecurrenceAsProof: false,
            AllowsSafetyThresholdAsRhetoricalDebate: false,
            AllowsDiagnosis: false,
            AllowsPathologyLabel: false,
            AllowsClinicalAuthority: false,
            AllowsMemoryAdmission: false,
            AllowsContinuityMutation: false,
            AllowsSelfGelMutation: false,
            AllowsGelAdmission: false,
            AllowsAuthority: false,
            AllowsAction: false,
            AllowsLispEvaluation: false,
            AllowsPacketEmission: false,
            AllowsReceiptReplay: false,
            AllowsPassageIncrement: false,
            AllowsActivation: false);

    private static GapCrossingArticulationReceipt CreateGapCrossingReceipt()
    {
        var surfaces = new[]
        {
            CreateSurface("main-body", "urn:san:high-energy-candidate:main-body", GapCrossingArticulationSurface.MainBodyEngine)
        };
        var lanes = new[]
        {
            CreateLane("main-body", surfaces[0].SurfaceHandle, "urn:san:high-energy-candidate:main-body")
        };

        return new(
            ReceiptHandle: "urn:san:gap-crossing:review:pre-diagnostic-test",
            Disposition: GapCrossingArticulationDisposition.CrossedForReviewCold,
            OutcomeCode: "gap-crossing-articulation-carried-review-only",
            GovernanceTrace: "Gap crossing carried pressure to articulation for review only.",
            SourcePressureEcologyReceiptHandle: "urn:san:shared-prime-pressure:review:test",
            SourceHighEnergyCandidateReceiptHandle: "urn:san:high-energy-articulation:review:test",
            Lanes: lanes,
            Surfaces: surfaces,
            Boundary: new GapCrossingArticulationBoundary(
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
                AllowsActivation: false),
            Refusal: null,
            PriorObservationCount: 0,
            ObservationCountAfterGapCrossing: 1,
            PriorPassageCount: 0,
            PassageCountAfterGapCrossing: 0,
            LaneCount: lanes.Length,
            SurfaceCount: surfaces.Length,
            ReviewOnly: true,
            GapCrossingObserved: true,
            PressureCarriedToArticulation: true,
            ArticulationSurfaceSelected: true,
            StewardWitnessPreserved: true,
            CoolingPreserved: true,
            ReturnPathPreserved: true,
            LlmSurfaceParticipated: true,
            PressureBecamePromptAuthority: false,
            PressureBecameTruth: false,
            PressureBecameWarrant: false,
            ProviderCallMade: false,
            ModelBound: false,
            RuntimeStarted: false,
            ActionAuthorized: false,
            ContinuityAdmitted: false,
            SelfGelMutated: false,
            GelAdmitted: false,
            CmeActualAdmitted: false,
            HeartbeatActive: false,
            AuthorityGranted: false,
            LispEvaluationAllowed: false,
            NewPacketEmitted: false,
            ReceiptsReplayed: false,
            PassageIncremented: false,
            ActivationRefused: true,
            TimestampUtc: TimestampUtc);
    }

    private static GapCrossingPressureLaneRecord CreateLane(
        string suffix,
        string surfaceHandle,
        string candidateHandle) =>
        new(
            LaneHandle: $"urn:san:gap-crossing-lane:{suffix}",
            SourceSignalHandle: "urn:san:shared-prime-pressure:integration",
            SourceDestinationHandle: "urn:san:shared-prime-pressure-destination:domain-ingress",
            CandidateHandle: candidateHandle,
            ArticulationSurfaceHandle: surfaceHandle,
            Lane: GapCrossingPressureLane.MeaningPressure,
            Surface: GapCrossingArticulationSurface.MainBodyEngine,
            LaneRationale: "Pressure may approach articulation as review material only.",
            NonAuthorityLaw: "Gap crossing is not prompt authority, action, continuity, or activation.",
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

    private static GapCrossingArticulationSurfaceRecord CreateSurface(
        string suffix,
        string candidateHandle,
        GapCrossingArticulationSurface surface) =>
        new(
            SurfaceHandle: $"urn:san:gap-crossing-surface:{suffix}",
            CandidateHandle: candidateHandle,
            Surface: surface,
            IntendedParticipation: "Surface may receive pressure as review-only articulation material.",
            NonBindingLaw: "Articulation surface participation is not acting body, authority, or CME.Actual.",
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

    private static PreDiagnosticCareSignalObservation MutateObservation(
        PreDiagnosticCareSignalObservation observation,
        string mutation) =>
        mutation switch
        {
            "diagnosis" => observation with { ClaimsDiagnosis = true },
            "pathology" => observation with { AssignsPathology = true },
            "intent-fact" => observation with { InfersIntentAsFact = true },
            "clinical-authority" => observation with { ClaimsClinicalAuthority = true },
            "truth" => observation with { TreatsObservationAsTruth = true },
            "memory" => observation with { AdmitsMemory = true },
            "continuity" => observation with { AdmitsContinuity = true },
            "authority" => observation with { GrantsAuthority = true },
            "action" => observation with { AuthorizesAction = true },
            "lisp" => observation with { EvaluatesLisp = true },
            "packet" => observation with { EmitsPacket = true },
            "replay" => observation with { ReplaysReceipt = true },
            "passage" => observation with { IncrementsPassage = true },
            "activation" => observation with { Activates = true },
            _ => throw new ArgumentOutOfRangeException(nameof(mutation), mutation, null)
        };

    private static PreDiagnosticRiskModifierRecord MutateModifier(
        PreDiagnosticRiskModifierRecord modifier,
        string mutation) =>
        mutation switch
        {
            "diagnostic-label" => modifier with { DiagnosticLabelApplied = true },
            "pathology" => modifier with { PathologyAssigned = true },
            "intent-fact" => modifier with { IntentClaimedAsFact = true },
            "proof" => modifier with { ModifierBecomesProof = true },
            "authority" => modifier with { GrantsAuthority = true },
            "action" => modifier with { AuthorizesAction = true },
            "memory" => modifier with { AdmitsMemory = true },
            "continuity" => modifier with { MutatesContinuity = true },
            "selfgel" => modifier with { MutatesSelfGel = true },
            "gel" => modifier with { AdmitsGel = true },
            "lisp" => modifier with { EvaluatesLisp = true },
            "packet" => modifier with { EmitsPacket = true },
            "replay" => modifier with { ReplaysReceipt = true },
            "passage" => modifier with { IncrementsPassage = true },
            "activation" => modifier with { Activates = true },
            _ => throw new ArgumentOutOfRangeException(nameof(mutation), mutation, null)
        };

    private static PreDiagnosticRiskSurfaceRequest MutateRequest(
        PreDiagnosticRiskSurfaceRequest request,
        string mutation) =>
        mutation switch
        {
            "observation-diagnosis" => request with { ObservationAsDiagnosisRequested = true },
            "modifier-pathology" => request with { RiskModifierAsPathologyRequested = true },
            "care-clinical-authority" => request with { CareBurdenAsClinicalAuthorityRequested = true },
            "recurrence-proof" => request with { RecurrenceAsProofRequested = true },
            "safety-debate" => request with { SafetyThresholdAsRhetoricalDebateRequested = true },
            "diagnosis" => request with { DiagnosisRequested = true },
            "pathology" => request with { PathologyLabelRequested = true },
            "clinical-authority" => request with { ClinicalAuthorityRequested = true },
            "memory" => request with { MemoryAdmissionRequested = true },
            "continuity" => request with { ContinuityMutationRequested = true },
            "selfgel" => request with { SelfGelMutationRequested = true },
            "gel" => request with { GelAdmissionRequested = true },
            "authority" => request with { AuthorityRequested = true },
            "action" => request with { ActionRequested = true },
            "lisp" => request with { LispEvaluationRequested = true },
            "packet" => request with { PacketEmissionRequested = true },
            "replay" => request with { ReceiptReplayRequested = true },
            "passage" => request with { PassageIncrementRequested = true },
            "activation" => request with { ActivationRequested = true },
            _ => throw new ArgumentOutOfRangeException(nameof(mutation), mutation, null)
        };

    private static void AssertRefused(PreDiagnosticRiskSurfaceReceipt receipt, string outcomeCode)
    {
        Assert.Equal(PreDiagnosticRiskSurfaceDisposition.Refused, receipt.Disposition);
        Assert.Equal(outcomeCode, receipt.OutcomeCode);
        Assert.True(receipt.IsRetainedPreDiagnosticRefusal);
        AssertNoPromotion(receipt);
    }

    private static void AssertNoPromotion(PreDiagnosticRiskSurfaceReceipt receipt)
    {
        Assert.False(receipt.ObservationBecameDiagnosis);
        Assert.False(receipt.RiskModifierBecamePathology);
        Assert.False(receipt.CareBurdenBecameClinicalAuthority);
        Assert.False(receipt.RecurrenceBecameProof);
        Assert.False(receipt.SafetyThresholdBecameRhetoricalDebate);
        Assert.False(receipt.DiagnosisIssued);
        Assert.False(receipt.PathologyAssigned);
        Assert.False(receipt.ClinicalAuthorityClaimed);
        Assert.False(receipt.MemoryAdmitted);
        Assert.False(receipt.ContinuityMutated);
        Assert.False(receipt.SelfGelMutated);
        Assert.False(receipt.GelAdmitted);
        Assert.False(receipt.AuthorityGranted);
        Assert.False(receipt.ActionAuthorized);
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
}
