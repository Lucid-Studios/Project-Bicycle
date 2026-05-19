using San.Common;
using Xunit;

namespace San.Audit.Tests;

public sealed class ZedDeltaChamberFormationBoundaryValidationTests
{
    private static readonly DateTimeOffset TimestampUtc = DateTimeOffset.Parse("2026-05-17T00:00:00Z");

    [Fact]
    public void Chamber_May_Form_Without_Heartbeat_Activation_Or_CmeActual_Admission()
    {
        var receipt = Declare(CreateRequest());

        AssertCold(receipt);
        Assert.Equal(ZedDeltaChamberFormationDisposition.ChamberFormedCold, receipt.Disposition);
        Assert.Equal("zed-delta-chamber-formed-review-only", receipt.OutcomeCode);
        Assert.True(receipt.ChamberFormed);
        Assert.True(receipt.CmeActualIdCandidateHeld);
        Assert.True(receipt.HeartbeatDescribed);
        Assert.False(receipt.HeartbeatActive);
        Assert.False(receipt.CmeActualAdmitted);
        Assert.False(receipt.RuntimeModelBound);
        Assert.False(receipt.RuntimeStarted);
        Assert.False(receipt.ActionAuthorized);
        Assert.False(receipt.ContinuityAdmitted);
        Assert.False(receipt.AuthorityGranted);
        Assert.False(receipt.OeReplaced);
        Assert.False(receipt.SelfGelMutated);
        Assert.False(receipt.MosCmosWritten);
        Assert.False(receipt.CgoaGrantedControl);
        Assert.False(receipt.SoulFrameBecameSelf);
        Assert.False(receipt.CompassAdmittedTruth);
        Assert.False(receipt.LispEvaluationAllowed);
        Assert.False(receipt.NewPacketEmitted);
        Assert.False(receipt.ReceiptsReplayed);
        Assert.False(receipt.PassageIncremented);
        Assert.True(receipt.ActivationRefused);
        Assert.Equal(receipt.PriorPassageCount, receipt.PassageCountAfterChamberReview);
        Assert.Contains("CME.Actual admission", receipt.GovernanceTrace, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData("missing")]
    [InlineData("refused")]
    [InlineData("empty-selection")]
    [InlineData("selection-not-cold")]
    public void Chamber_Requires_Cold_Selected_Action_Surface_Source(string sourceCase)
    {
        var source = sourceCase switch
        {
            "missing" => null,
            "refused" => CreateSourceReceipt(refused: true),
            "empty-selection" => CreateSourceReceipt(emptySelection: true),
            "selection-not-cold" => CreateSourceReceipt(allowsAuthority: true),
            _ => CreateSourceReceipt()
        };

        var request = sourceCase == "missing"
            ? CreateRequest() with { SourceSelectiveActionSurfaceReceipt = null }
            : CreateRequest(source: source);

        var receipt = Declare(request);

        AssertRefused(receipt, "zed-delta-source-selective-action-missing");
    }

    [Theory]
    [InlineData("missing-handle")]
    [InlineData("x-not-zero")]
    [InlineData("y-not-zero")]
    [InlineData("z-not-zero")]
    [InlineData("not-local")]
    [InlineData("not-review")]
    [InlineData("authority")]
    [InlineData("continuity")]
    [InlineData("heartbeat")]
    public void Zed_Delta_Origin_Must_Be_Local_Zero_Zero_Zero_Without_Promotion(string originCase)
    {
        var receipt = Declare(CreateRequest(origin: MutateOrigin(CreateOrigin(), originCase)));

        AssertRefused(receipt, "zed-delta-origin-invalid");
    }

    [Theory]
    [InlineData("missing-code")]
    [InlineData("not-present")]
    [InlineData("not-review")]
    [InlineData("no-orient")]
    [InlineData("no-cselfgel")]
    [InlineData("no-coe")]
    [InlineData("no-origin")]
    [InlineData("truth")]
    [InlineData("selfgel-mutation")]
    [InlineData("oe-mutation")]
    [InlineData("authority")]
    [InlineData("heartbeat")]
    public void Compass_Orientation_May_Not_Admit_Truth_Mutation_Authority_Or_Heartbeat(string boundaryCase)
    {
        var receipt = Declare(CreateRequest(orientation: MutateOrientation(CreateOrientation(), boundaryCase)));

        AssertRefused(receipt, "zed-delta-compass-orientation-invalid");
    }

    [Theory]
    [InlineData("chamber-cannot-form")]
    [InlineData("heartbeat-not-described")]
    [InlineData("heartbeat-active")]
    [InlineData("cme-actual")]
    [InlineData("model-binding")]
    [InlineData("runtime")]
    [InlineData("action")]
    [InlineData("continuity")]
    [InlineData("authority")]
    [InlineData("oe-replacement")]
    [InlineData("selfgel-mutation")]
    [InlineData("store-write")]
    [InlineData("cgoa-control")]
    [InlineData("soulframe-self")]
    [InlineData("compass-truth")]
    [InlineData("lisp")]
    [InlineData("packet")]
    [InlineData("replay")]
    [InlineData("passage")]
    [InlineData("activation")]
    public void Non_Activation_Boundary_Must_Keep_Chamber_Pre_Heartbeat(string boundaryCase)
    {
        var receipt = Declare(CreateRequest(nonActivation: MutateNonActivation(CreateNonActivation(), boundaryCase)));

        AssertRefused(receipt, "zed-delta-non-activation-boundary-invalid");
    }

    [Theory]
    [InlineData("missing-standing")]
    [InlineData("missing-hold")]
    [InlineData("missing-closure")]
    [InlineData("missing-telemetry")]
    public void Chamber_Requires_All_Standing_And_Routing_Parts_Together(string missingCase)
    {
        var request = missingCase switch
        {
            "missing-standing" => CreateRequest(standings: []),
            "missing-hold" => CreateRequest(holds: []),
            "missing-closure" => CreateRequest(closureRoutes: []),
            "missing-telemetry" => CreateRequest(telemetryRoutes: []),
            _ => CreateRequest()
        };

        var receipt = Declare(request);

        AssertRefused(receipt, "zed-delta-chamber-standing-incomplete");
    }

    [Theory]
    [InlineData("missing-handle")]
    [InlineData("not-review")]
    [InlineData("not-conditional")]
    [InlineData("not-standing")]
    [InlineData("no-oe-lineage")]
    [InlineData("no-surface-lineage")]
    [InlineData("not-candidate")]
    [InlineData("replaces-oe")]
    [InlineData("mutates-oe")]
    [InlineData("continuity")]
    [InlineData("authority")]
    [InlineData("cme-actual")]
    [InlineData("heartbeat")]
    public void COE_Standing_Is_Conditional_And_Non_Actualizing(string standingCase)
    {
        var standing = MutateStanding(CreateStanding(), standingCase);

        var receipt = Declare(CreateRequest(standings: [standing]));

        AssertRefused(receipt, "zed-delta-coe-standing-invalid");
    }

    [Theory]
    [InlineData("missing-handle")]
    [InlineData("not-review")]
    [InlineData("not-conditional")]
    [InlineData("not-held")]
    [InlineData("not-live-ec")]
    [InlineData("no-selfgel-lineage")]
    [InlineData("no-oe-lineage")]
    [InlineData("no-cooling")]
    [InlineData("mutates-selfgel")]
    [InlineData("promotes-selfgel")]
    [InlineData("continuity")]
    [InlineData("authority")]
    [InlineData("heartbeat")]
    public void CSelfGEL_Hold_Is_Compass_Held_And_Non_Mutating(string holdCase)
    {
        var hold = MutateHold(CreateHold(), holdCase);

        var receipt = Declare(CreateRequest(holds: [hold]));

        AssertRefused(receipt, "zed-delta-cselfgel-hold-invalid");
    }

    [Theory]
    [InlineData("missing-route")]
    [InlineData("not-review")]
    [InlineData("not-closure")]
    [InlineData("not-close-residue")]
    [InlineData("not-return-prime")]
    [InlineData("no-mos-lineage")]
    [InlineData("no-cmos-lineage")]
    [InlineData("no-cselfgel-lineage")]
    [InlineData("writes-mos")]
    [InlineData("writes-cmos")]
    [InlineData("continuity")]
    [InlineData("authority")]
    [InlineData("heartbeat")]
    public void MoS_CMoS_Closure_Is_Route_Not_Store_Write_Or_Continuity(string routeCase)
    {
        var route = MutateClosure(CreateClosure(), routeCase);

        var receipt = Declare(CreateRequest(closureRoutes: [route]));

        AssertRefused(receipt, "zed-delta-mos-cmos-closure-invalid");
    }

    [Theory]
    [InlineData("missing-route")]
    [InlineData("not-review")]
    [InlineData("not-duplex")]
    [InlineData("no-external")]
    [InlineData("no-internal")]
    [InlineData("no-listening")]
    [InlineData("no-goa-lineage")]
    [InlineData("no-cgoa-lineage")]
    [InlineData("no-soulframe-lineage")]
    [InlineData("cgoa-control")]
    [InlineData("soulframe-self")]
    [InlineData("action")]
    [InlineData("continuity")]
    [InlineData("heartbeat")]
    public void GoA_CGoA_SoulFrame_Route_Is_Telemetry_Not_Control_Or_Selfhood(string routeCase)
    {
        var route = MutateTelemetry(CreateTelemetry(), routeCase);

        var receipt = Declare(CreateRequest(telemetryRoutes: [route]));

        AssertRefused(receipt, "zed-delta-goa-cgoa-soulframe-route-invalid");
    }

    [Theory]
    [InlineData("standing-handle")]
    [InlineData("coe-handle")]
    [InlineData("hold-handle")]
    [InlineData("cselfgel-handle")]
    [InlineData("closure-route")]
    [InlineData("telemetry-route")]
    public void Duplicate_Chamber_Handles_Refuse_Lineage_Collapse(string duplicateCase)
    {
        var standing = CreateStanding();
        var hold = CreateHold();
        var closure = CreateClosure();
        var telemetry = CreateTelemetry();
        var request = duplicateCase switch
        {
            "standing-handle" => CreateRequest(standings: [standing, CreateStanding("second") with { StandingHandle = standing.StandingHandle }]),
            "coe-handle" => CreateRequest(standings: [standing, CreateStanding("second") with { ConditionalOeHandle = standing.ConditionalOeHandle }]),
            "hold-handle" => CreateRequest(holds: [hold, CreateHold("second") with { HoldHandle = hold.HoldHandle }]),
            "cselfgel-handle" => CreateRequest(holds: [hold, CreateHold("second") with { ConditionalSelfGelHandle = hold.ConditionalSelfGelHandle }]),
            "closure-route" => CreateRequest(closureRoutes: [closure, CreateClosure("second") with { RouteHandle = closure.RouteHandle }]),
            "telemetry-route" => CreateRequest(telemetryRoutes: [telemetry, CreateTelemetry("second") with { RouteHandle = telemetry.RouteHandle }]),
            _ => CreateRequest()
        };

        var receipt = Declare(request);

        AssertRefused(receipt, "zed-delta-duplicate-chamber-handle");
    }

    [Theory]
    [InlineData("standing-origin")]
    [InlineData("standing-surface")]
    [InlineData("standing-decision")]
    [InlineData("hold-origin")]
    [InlineData("hold-coe")]
    [InlineData("closure-origin")]
    [InlineData("closure-cselfgel")]
    [InlineData("closure-coe")]
    [InlineData("telemetry-origin")]
    [InlineData("telemetry-cselfgel")]
    [InlineData("telemetry-coe")]
    public void Chamber_Lineage_Must_Reconstruct_Without_Shortcuts(string lineageCase)
    {
        var request = lineageCase switch
        {
            "standing-origin" => CreateRequest(standings: [CreateStanding() with { ZedDeltaOriginHandle = "urn:san:zed-delta:missing" }]),
            "standing-surface" => CreateRequest(standings: [CreateStanding() with { SourceSelectiveActionSurfaceHandle = "urn:san:selective-action:missing" }]),
            "standing-decision" => CreateRequest(standings: [CreateStanding() with { SourceDecisionHandle = "urn:san:decision:missing" }]),
            "hold-origin" => CreateRequest(holds: [CreateHold() with { ZedDeltaOriginHandle = "urn:san:zed-delta:missing" }]),
            "hold-coe" => CreateRequest(holds: [CreateHold() with { ConditionalOeHandle = "urn:san:coe:missing" }]),
            "closure-origin" => CreateRequest(closureRoutes: [CreateClosure() with { ZedDeltaOriginHandle = "urn:san:zed-delta:missing" }]),
            "closure-cselfgel" => CreateRequest(closureRoutes: [CreateClosure() with { ConditionalSelfGelHandle = "urn:san:cselfgel:missing" }]),
            "closure-coe" => CreateRequest(closureRoutes: [CreateClosure() with { ConditionalOeHandle = "urn:san:coe:missing" }]),
            "telemetry-origin" => CreateRequest(telemetryRoutes: [CreateTelemetry() with { ZedDeltaOriginHandle = "urn:san:zed-delta:missing" }]),
            "telemetry-cselfgel" => CreateRequest(telemetryRoutes: [CreateTelemetry() with { ConditionalSelfGelHandle = "urn:san:cselfgel:missing" }]),
            "telemetry-coe" => CreateRequest(telemetryRoutes: [CreateTelemetry() with { ConditionalOeHandle = "urn:san:coe:missing" }]),
            _ => CreateRequest()
        };

        var receipt = Declare(request);

        Assert.Equal(ZedDeltaChamberFormationDisposition.Refused, receipt.Disposition);
        Assert.True(receipt.IsRetainedZedDeltaChamberFormationRefusal);
        Assert.Contains("lineage", receipt.GovernanceTrace, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Every_CSelfGEL_Hold_Requires_Closure_And_Telemetry_Routes()
    {
        var standing = CreateStanding("second");
        var hold = CreateHold("second");

        var receipt = Declare(CreateRequest(standings: [CreateStanding(), standing], holds: [CreateHold(), hold]));

        AssertRefused(receipt, "zed-delta-chamber-route-missing");
    }

    [Fact]
    public void Lisp_Body_Declares_Zed_Delta_Chamber_As_Inert_Carrier()
    {
        var root = FindRepoRoot();
        var body = File.ReadAllText(Path.Combine(root, "src", "SLI", "SLI.Lisp", "zed-delta-chamber-formation.lisp"));

        Assert.Contains(":posture :cme-zed-delta-chamber-formation-boundary", body, StringComparison.Ordinal);
        Assert.Contains(":lisp-role :inert-zed-delta-chamber-carrier", body, StringComparison.Ordinal);
        Assert.Contains(":oe-stands-as-coe t", body, StringComparison.Ordinal);
        Assert.Contains(":selfgel-held-as-cselfgel t", body, StringComparison.Ordinal);
        Assert.Contains(":heartbeat-active nil", body, StringComparison.Ordinal);
        Assert.Contains(":cme-actual-admitted nil", body, StringComparison.Ordinal);
        Assert.Contains(":lisp-evaluation-allowed nil", body, StringComparison.Ordinal);
    }

    private static ZedDeltaChamberFormationReceipt Declare(ZedDeltaChamberFormationRequest request) =>
        new DefaultZedDeltaChamberFormationBoundaryValidator().Declare(request, TimestampUtc);

    private static ZedDeltaChamberFormationRequest CreateRequest(
        SelectiveLawfulActionSurfaceReceipt? source = null,
        ZedDeltaOrigin? origin = null,
        IReadOnlyList<ConditionalOperationalExpressionStanding>? standings = null,
        IReadOnlyList<ConditionalSelfGelHold>? holds = null,
        IReadOnlyList<MosCmosResidueClosureRoute>? closureRoutes = null,
        IReadOnlyList<GoaCgoaSoulFrameTelemetryRoute>? telemetryRoutes = null,
        CompassChamberOrientationBoundary? orientation = null,
        ZedDeltaChamberNonActivationBoundary? nonActivation = null) =>
        new(
            SourceSelectiveActionSurfaceReceipt: source ?? CreateSourceReceipt(),
            Origin: origin ?? CreateOrigin(),
            ConditionalOperationalExpressions: standings ?? [CreateStanding()],
            ConditionalSelfGelHolds: holds ?? [CreateHold()],
            ResidueClosureRoutes: closureRoutes ?? [CreateClosure()],
            TelemetryRoutes: telemetryRoutes ?? [CreateTelemetry()],
            OrientationBoundary: orientation ?? CreateOrientation(),
            NonActivationBoundary: nonActivation ?? CreateNonActivation(),
            PriorPassageCount: 17);

    private static SelectiveLawfulActionSurfaceReceipt CreateSourceReceipt(
        bool refused = false,
        bool emptySelection = false,
        bool allowsAuthority = false)
    {
        var surfaceBoundary = new SelectiveLawfulActionSurfaceBoundary(
            BoundaryCode: "selective-lawful-action-surface-review-only",
            Present: true,
            ReviewOnly: true,
            AllowsSurfaceSelection: true,
            RequiresPersonificationActualizationReceipt: true,
            RequiresStewardActionAdmissibilityReceipt: true,
            RequiresSeparateEnactmentBoundary: true,
            RequiresWitness: true,
            RequiresCooling: true,
            RequiresRevocation: true,
            RequiresLossCondition: true,
            AllowsActionExecution: false,
            AllowsActionAuthorization: false,
            AllowsContinuityAdmission: false,
            AllowsAuthority: allowsAuthority,
            AllowsIdentityMutation: false,
            AllowsMorphologyCreation: false,
            AllowsConsentExpansion: false,
            AllowsOverreachNormalization: false,
            AllowsRuntimeAction: false,
            AllowsLispEvaluation: false,
            AllowsPacketEmission: false,
            AllowsReceiptReplay: false,
            AllowsPassageIncrement: false,
            AllowsActivation: false);
        var nonEnactment = new SelectiveLawfulActionNonEnactmentBoundary(
            BoundaryLaw: "selection is not enactment",
            ActionSurfaceMayBeSelected: true,
            SelectionMayExecute: false,
            TouchMayExecute: false,
            PersonificationGuidanceMayAuthorize: false,
            FeltSignificanceMayAuthorize: false,
            PressureMaySelectExecution: false,
            StewardAdmissibilityMayExecute: false,
            ReviewMayBecomeRuntimeAction: false,
            SelectionMayAdmitContinuity: false,
            SelectionMayGrantAuthority: false,
            SelectionMayMutateIdentity: false,
            SelectionMayCreateMorphology: false,
            SelectionMayExpandConsent: false,
            SelectionMayNormalizeOverreach: false,
            SelectionMayEvaluateLisp: false,
            SelectionMayEmitPacket: false,
            SelectionMayReplayReceipt: false,
            SelectionMayIncrementPassage: false,
            SelectionMayActivate: false,
            RequiresWitness: true,
            RequiresCooling: true,
            RequiresSeparateEnactmentBoundary: true,
            RequiresAuthorityAbsence: true);

        var surfaces = emptySelection || refused ? Array.Empty<SelectiveLawfulActionSurface>() : new[] { CreateSelectedSurface() };
        return new(
            ReceiptHandle: refused ? "urn:san:selective-lawful-action:refused:test" : "urn:san:selective-lawful-action:review:test",
            Disposition: refused ? SelectiveLawfulActionSurfaceDisposition.Refused : SelectiveLawfulActionSurfaceDisposition.SelectedForReviewCold,
            OutcomeCode: refused ? "refused" : "selected",
            GovernanceTrace: "source selective action surface test receipt",
            SourcePersonificationActualizationReceiptHandle: "urn:san:personification-actualization:review:test",
            SourceStewardActionAdmissibilityReceiptHandle: "urn:san:steward-admissibility:review:test",
            Surfaces: surfaces,
            Routes: [],
            SurfaceBoundary: surfaceBoundary,
            NonEnactmentBoundary: nonEnactment,
            Refusal: refused ? new SelectiveLawfulActionSurfaceRefusalReceipt("urn:san:selective-lawful-action-refusal:test", "refused", "refused", true) : null,
            PriorPassageCount: 17,
            PassageCountAfterSelectionReview: 17,
            SelectedSurfaceCount: surfaces.Length,
            MaximumObservedTouchWeight: surfaces.Length == 0 ? 0m : 0.8m,
            ReviewOnly: true,
            SelectionOnly: true,
            TouchOnly: true,
            PersonificationGuidanceUsed: surfaces.Length > 0,
            ActionSurfaceSelected: surfaces.Length > 0,
            ActionSurfaceTouched: surfaces.Length > 0,
            SeparateEnactmentBoundaryRequired: true,
            SurfaceTouchExecuted: false,
            ActionAuthorized: false,
            RuntimeActionAllowed: false,
            ContinuityAdmitted: false,
            AuthorityGranted: false,
            IdentityMutated: false,
            MorphologyCreated: false,
            ConsentExpanded: false,
            OverreachNormalized: false,
            LispEvaluationAllowed: false,
            NewPacketEmitted: false,
            ReceiptsReplayed: false,
            ActivationRefused: true,
            TimestampUtc: TimestampUtc);
    }

    private static SelectiveLawfulActionSurface CreateSelectedSurface() =>
        new(
            SurfaceHandle: "urn:san:selective-action:orientation-review",
            SurfaceClass: SelectiveActionSurfaceClass.OrientationReview,
            PersonificationSurfaceHandle: "urn:san:personification-actualization:orientation",
            PersonificationUseClass: PersonificationActualizationUseClass.Orientation,
            ActionHandle: "urn:san:action:review",
            MethodHandle: "urn:san:method:review",
            DecisionHandle: "urn:san:steward-admissibility:decision",
            EvidenceHandle: "urn:san:evidence:selective",
            WitnessHandle: "urn:san:witness:selective",
            StewardSurface: "steward",
            TelemetryRoute: "telemetry-string",
            CustodyOwner: "steward",
            RevocationPath: "return-to-review",
            LossCondition: "selection-attempts-enactment",
            TouchVector: new SelectiveActionTouchVector(0.8m, 0.7m, 0.8m, 0.8m, 0.7m, 0.9m),
            ReviewOnly: true,
            SelectionOnly: true,
            TouchOnly: true,
            BindsPersonificationTelemetry: true,
            BindsStewardAdmissibility: true,
            RequiresSeparateEnactmentBoundary: true,
            RequiresWitness: true,
            RequiresCooling: true,
            RequiresRevocation: true,
            RequiresLossCondition: true,
            PreservesPersonificationLineage: true,
            PreservesActionLineage: true,
            PreservesMethodLineage: true,
            PreservesDecisionLineage: true,
            PersonificationGuidanceSelectsAuthority: false,
            FeltSignificanceSelectsExecution: false,
            PressureSelectsExecution: false,
            SurfaceTouchExecutes: false,
            SelectionAuthorizesAction: false,
            SelectionAdmitsContinuity: false,
            SelectionGrantsAuthority: false,
            SelectionMutatesIdentity: false,
            SelectionCreatesMorphology: false,
            SelectionExpandsConsent: false,
            SelectionNormalizesOverreach: false,
            SelectionEvaluatesLisp: false,
            SelectionEmitsPacket: false,
            SelectionReplaysReceipt: false,
            SelectionIncrementsPassage: false,
            SelectionActivates: false);

    private static ZedDeltaOrigin CreateOrigin() =>
        new(
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

    private static ConditionalOperationalExpressionStanding CreateStanding(string suffix = "primary") =>
        new(
            StandingHandle: $"urn:san:zed-delta-standing:{suffix}",
            OeHandle: $"urn:san:oe:{suffix}",
            ConditionalOeHandle: $"urn:san:coe:{suffix}",
            CmeActualIdHandle: $"urn:san:cme-actual-id:candidate:{suffix}",
            ZedDeltaOriginHandle: "urn:san:zed-delta:origin:0-0-0",
            SourceSelectiveActionSurfaceHandle: "urn:san:selective-action:orientation-review",
            SourceDecisionHandle: "urn:san:steward-admissibility:decision",
            WitnessHandle: $"urn:san:witness:coe:{suffix}",
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

    private static ConditionalSelfGelHold CreateHold(string suffix = "primary") =>
        new(
            HoldHandle: $"urn:san:zed-delta-cselfgel-hold:{suffix}",
            SelfGelHandle: $"urn:san:selfgel:{suffix}",
            ConditionalSelfGelHandle: $"urn:san:cselfgel:{suffix}",
            ConditionalOeHandle: $"urn:san:coe:{suffix}",
            CompassHandle: "urn:san:compass:zed-delta",
            ZedDeltaOriginHandle: "urn:san:zed-delta:origin:0-0-0",
            WitnessHandle: $"urn:san:witness:cselfgel:{suffix}",
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

    private static MosCmosResidueClosureRoute CreateClosure(string suffix = "primary") =>
        new(
            RouteHandle: $"urn:san:zed-delta-mos-cmos-closure:{suffix}",
            MosHandle: "urn:san:mos:self-store",
            CmosHandle: "urn:san:cmos:shadow-self-store",
            ConditionalSelfGelHandle: $"urn:san:cselfgel:{suffix}",
            ConditionalOeHandle: $"urn:san:coe:{suffix}",
            ZedDeltaOriginHandle: "urn:san:zed-delta:origin:0-0-0",
            ResidueHandle: $"urn:san:residue:{suffix}",
            CoolingHandle: "urn:san:cooling:zed-delta",
            ReturnToPrimeHandle: "urn:san:return:prime",
            WitnessHandle: $"urn:san:witness:mos-cmos:{suffix}",
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

    private static GoaCgoaSoulFrameTelemetryRoute CreateTelemetry(string suffix = "primary") =>
        new(
            RouteHandle: $"urn:san:zed-delta-goa-cgoa-soulframe:{suffix}",
            GoaHandle: "urn:san:goa:external-formation",
            CgoaHandle: "urn:san:cgoa:cryptic-control-plane",
            ListeningFrameHandle: "urn:san:listening-frame:external",
            SoulFrameHandle: "urn:san:soulframe:internal-telemetry",
            ExternalFormationHandle: "urn:san:formation:external",
            InternalTelemetryHandle: "urn:san:telemetry:internal",
            ConditionalOeHandle: $"urn:san:coe:{suffix}",
            ConditionalSelfGelHandle: $"urn:san:cselfgel:{suffix}",
            ZedDeltaOriginHandle: "urn:san:zed-delta:origin:0-0-0",
            WitnessHandle: $"urn:san:witness:goa-cgoa:{suffix}",
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

    private static CompassChamberOrientationBoundary CreateOrientation() =>
        new(
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

    private static ZedDeltaChamberNonActivationBoundary CreateNonActivation() =>
        new(
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

    private static ZedDeltaOrigin MutateOrigin(ZedDeltaOrigin origin, string originCase) =>
        originCase switch
        {
            "missing-handle" => origin with { OriginHandle = string.Empty },
            "x-not-zero" => origin with { X = 1 },
            "y-not-zero" => origin with { Y = 1 },
            "z-not-zero" => origin with { Z = 1 },
            "not-local" => origin with { LocalDeltaOrigin = false },
            "not-review" => origin with { ReviewOnly = false },
            "authority" => origin with { GrantsAuthority = true },
            "continuity" => origin with { AdmitsContinuity = true },
            "heartbeat" => origin with { ActivatesHeartbeat = true },
            _ => origin
        };

    private static CompassChamberOrientationBoundary MutateOrientation(CompassChamberOrientationBoundary boundary, string boundaryCase) =>
        boundaryCase switch
        {
            "missing-code" => boundary with { BoundaryCode = string.Empty },
            "not-present" => boundary with { Present = false },
            "not-review" => boundary with { ReviewOnly = false },
            "no-orient" => boundary with { OrientsChamber = false },
            "no-cselfgel" => boundary with { HoldsConditionalSelfGel = false },
            "no-coe" => boundary with { CoordinatesConditionalOe = false },
            "no-origin" => boundary with { RequiresZedDeltaOrigin = false },
            "truth" => boundary with { AdmitsTruth = true },
            "selfgel-mutation" => boundary with { MutatesSelfGel = true },
            "oe-mutation" => boundary with { MutatesOe = true },
            "authority" => boundary with { GrantsAuthority = true },
            "heartbeat" => boundary with { ActivatesHeartbeat = true },
            _ => boundary
        };

    private static ZedDeltaChamberNonActivationBoundary MutateNonActivation(ZedDeltaChamberNonActivationBoundary boundary, string boundaryCase) =>
        boundaryCase switch
        {
            "chamber-cannot-form" => boundary with { ChamberMayForm = false },
            "heartbeat-not-described" => boundary with { HeartbeatMayBeDescribed = false },
            "heartbeat-active" => boundary with { HeartbeatMayActivate = true },
            "cme-actual" => boundary with { CmeActualMayBeAdmitted = true },
            "model-binding" => boundary with { ModelMayBind = true },
            "runtime" => boundary with { RuntimeMayStart = true },
            "action" => boundary with { ActionMayExecute = true },
            "continuity" => boundary with { ContinuityMayBeAdmitted = true },
            "authority" => boundary with { AuthorityMayBeGranted = true },
            "oe-replacement" => boundary with { OeMayBeReplaced = true },
            "selfgel-mutation" => boundary with { SelfGelMayBeMutated = true },
            "store-write" => boundary with { MosCmosMayBeWritten = true },
            "cgoa-control" => boundary with { CgoaMayGrantControl = true },
            "soulframe-self" => boundary with { SoulFrameMayBecomeSelf = true },
            "compass-truth" => boundary with { CompassMayAdmitTruth = true },
            "lisp" => boundary with { LispMayEvaluate = true },
            "packet" => boundary with { PacketMayEmit = true },
            "replay" => boundary with { ReceiptMayReplay = true },
            "passage" => boundary with { PassageMayIncrement = true },
            "activation" => boundary with { ActivationMayProceed = true },
            _ => boundary
        };

    private static ConditionalOperationalExpressionStanding MutateStanding(ConditionalOperationalExpressionStanding standing, string standingCase) =>
        standingCase switch
        {
            "missing-handle" => standing with { StandingHandle = string.Empty },
            "not-review" => standing with { ReviewOnly = false },
            "not-conditional" => standing with { ConditionalOnly = false },
            "not-standing" => standing with { StandsAtZedDeltaOrigin = false },
            "no-oe-lineage" => standing with { PreservesOeLineage = false },
            "no-surface-lineage" => standing with { PreservesSelectedSurfaceLineage = false },
            "not-candidate" => standing with { CmeActualIdCandidateOnly = false },
            "replaces-oe" => standing with { ReplacesOe = true },
            "mutates-oe" => standing with { MutatesOe = true },
            "continuity" => standing with { AdmitsContinuity = true },
            "authority" => standing with { GrantsAuthority = true },
            "cme-actual" => standing with { AdmitsCmeActual = true },
            "heartbeat" => standing with { ActivatesHeartbeat = true },
            _ => standing
        };

    private static ConditionalSelfGelHold MutateHold(ConditionalSelfGelHold hold, string holdCase) =>
        holdCase switch
        {
            "missing-handle" => hold with { HoldHandle = string.Empty },
            "not-review" => hold with { ReviewOnly = false },
            "not-conditional" => hold with { ConditionalOnly = false },
            "not-held" => hold with { HeldByCompass = false },
            "not-live-ec" => hold with { HoldsForLiveEc = false },
            "no-selfgel-lineage" => hold with { PreservesSelfGelLineage = false },
            "no-oe-lineage" => hold with { PreservesOeLineage = false },
            "no-cooling" => hold with { RequiresCooling = false },
            "mutates-selfgel" => hold with { MutatesSelfGel = true },
            "promotes-selfgel" => hold with { PromotesToSelfGel = true },
            "continuity" => hold with { AdmitsContinuity = true },
            "authority" => hold with { GrantsAuthority = true },
            "heartbeat" => hold with { ActivatesHeartbeat = true },
            _ => hold
        };

    private static MosCmosResidueClosureRoute MutateClosure(MosCmosResidueClosureRoute route, string routeCase) =>
        routeCase switch
        {
            "missing-route" => route with { RouteHandle = string.Empty },
            "not-review" => route with { ReviewOnly = false },
            "not-closure" => route with { ClosureRouteOnly = false },
            "not-close-residue" => route with { MayCloseUncooledResidue = false },
            "not-return-prime" => route with { ReturnsToPrimeState = false },
            "no-mos-lineage" => route with { PreservesMosLineage = false },
            "no-cmos-lineage" => route with { PreservesCmosLineage = false },
            "no-cselfgel-lineage" => route with { PreservesConditionalSelfGelLineage = false },
            "writes-mos" => route with { WritesMos = true },
            "writes-cmos" => route with { WritesCmos = true },
            "continuity" => route with { ResidueBecomesContinuity = true },
            "authority" => route with { ResidueBecomesAuthority = true },
            "heartbeat" => route with { ActivatesHeartbeat = true },
            _ => route
        };

    private static GoaCgoaSoulFrameTelemetryRoute MutateTelemetry(GoaCgoaSoulFrameTelemetryRoute route, string routeCase) =>
        routeCase switch
        {
            "missing-route" => route with { RouteHandle = string.Empty },
            "not-review" => route with { ReviewOnly = false },
            "not-duplex" => route with { DuplexRouteOnly = false },
            "no-external" => route with { ExternalFormationRoutesThroughCgoa = false },
            "no-internal" => route with { InternalTelemetryRoutesIntoSoulFrame = false },
            "no-listening" => route with { ListeningFrameWiredToSoulFrame = false },
            "no-goa-lineage" => route with { PreservesGoaLineage = false },
            "no-cgoa-lineage" => route with { PreservesCgoaLineage = false },
            "no-soulframe-lineage" => route with { PreservesSoulFrameLineage = false },
            "cgoa-control" => route with { CgoaGrantsControl = true },
            "soulframe-self" => route with { SoulFrameBecomesSelf = true },
            "action" => route with { RouteAuthorizesAction = true },
            "continuity" => route with { RouteAdmitsContinuity = true },
            "heartbeat" => route with { ActivatesHeartbeat = true },
            _ => route
        };

    private static void AssertCold(ZedDeltaChamberFormationReceipt receipt)
    {
        Assert.True(receipt.IsColdZedDeltaChamberFormation);
        Assert.Null(receipt.Refusal);
    }

    private static void AssertRefused(ZedDeltaChamberFormationReceipt receipt, string outcomeCode)
    {
        Assert.Equal(ZedDeltaChamberFormationDisposition.Refused, receipt.Disposition);
        Assert.Equal(outcomeCode, receipt.OutcomeCode);
        Assert.True(receipt.IsRetainedZedDeltaChamberFormationRefusal);
        Assert.True(receipt.ActivationRefused);
        Assert.False(receipt.HeartbeatActive);
        Assert.False(receipt.CmeActualAdmitted);
        Assert.False(receipt.ActionAuthorized);
        Assert.False(receipt.AuthorityGranted);
    }

    private static string FindRepoRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null)
        {
            var candidate = Path.Combine(directory.FullName, "src", "SLI", "SLI.Lisp", "zed-delta-chamber-formation.lisp");
            if (File.Exists(candidate))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        throw new DirectoryNotFoundException("Could not locate Project Bicycle repository root.");
    }
}
