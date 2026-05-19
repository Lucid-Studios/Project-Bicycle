using San.Common;
using Xunit;

namespace San.Audit.Tests;

public sealed class SelectiveLawfulActionSurfaceBoundaryValidationTests
{
    private static readonly DateTimeOffset TimestampUtc = DateTimeOffset.Parse("2026-05-17T00:00:00Z");

    [Fact]
    public void Selective_Action_Surface_May_Be_Selected_And_Touched_For_Review()
    {
        var receipt = Declare(CreateRequest());

        AssertCold(receipt);
        Assert.Equal(SelectiveLawfulActionSurfaceDisposition.SelectedForReviewCold, receipt.Disposition);
        Assert.Equal("selective-action-surface-selected-review-only", receipt.OutcomeCode);
        Assert.Equal(7, receipt.Surfaces.Count);
        Assert.Equal(7, receipt.Routes.Count);
        Assert.True(receipt.PersonificationGuidanceUsed);
        Assert.True(receipt.ActionSurfaceSelected);
        Assert.True(receipt.ActionSurfaceTouched);
        Assert.Contains(receipt.Surfaces, surface => surface.SurfaceClass == SelectiveActionSurfaceClass.OrientationReview);
        Assert.Contains(receipt.Surfaces, surface => surface.SurfaceClass == SelectiveActionSurfaceClass.OperatorHandoffReview);
        Assert.Contains("Selection may name and touch", receipt.GovernanceTrace, StringComparison.Ordinal);
    }

    [Fact]
    public void Selection_Does_Not_Execute_Authorize_Continuity_Or_Alter_Passage()
    {
        var receipt = Declare(CreateRequest(priorPassageCount: 808));

        AssertCold(receipt);
        Assert.Equal(808, receipt.PriorPassageCount);
        Assert.Equal(808, receipt.PassageCountAfterSelectionReview);
        Assert.False(receipt.SurfaceTouchExecuted);
        Assert.False(receipt.ActionAuthorized);
        Assert.False(receipt.RuntimeActionAllowed);
        Assert.False(receipt.ContinuityAdmitted);
        Assert.False(receipt.AuthorityGranted);
        Assert.False(receipt.IdentityMutated);
        Assert.False(receipt.MorphologyCreated);
        Assert.False(receipt.LispEvaluationAllowed);
        Assert.False(receipt.NewPacketEmitted);
        Assert.False(receipt.ReceiptsReplayed);
        Assert.True(receipt.ActivationRefused);
    }

    [Fact]
    public void Empty_Selection_Is_Reviewable_But_Not_Touched()
    {
        var receipt = Declare(CreateRequest(surfaces: [], routes: []));

        AssertCold(receipt);
        Assert.Equal(SelectiveLawfulActionSurfaceDisposition.EmptyReviewCold, receipt.Disposition);
        Assert.Equal("selective-action-empty-review-only", receipt.OutcomeCode);
        Assert.Empty(receipt.Surfaces);
        Assert.Empty(receipt.Routes);
        Assert.False(receipt.PersonificationGuidanceUsed);
        Assert.False(receipt.ActionSurfaceSelected);
        Assert.False(receipt.ActionSurfaceTouched);
        Assert.Equal(0, receipt.SelectedSurfaceCount);
        Assert.Equal(0m, receipt.MaximumObservedTouchWeight);
    }

    [Fact]
    public void Selected_Surface_Preserves_Personification_And_Admissibility_Lineage()
    {
        var personification = CreatePersonificationReceipt();
        var admissibility = CreateAdmissibilityReceipt();

        var receipt = Declare(CreateRequest(
            personification: personification,
            admissibility: admissibility));

        AssertCold(receipt);
        Assert.Equal(personification.ReceiptHandle, receipt.SourcePersonificationActualizationReceiptHandle);
        Assert.Equal(admissibility.ReceiptHandle, receipt.SourceStewardActionAdmissibilityReceiptHandle);
        Assert.All(receipt.Surfaces, surface => Assert.Contains(personification.Surfaces, item => item.SurfaceHandle == surface.PersonificationSurfaceHandle));
        Assert.All(receipt.Surfaces, surface => Assert.Contains(admissibility.Decisions, item => item.DecisionHandle == surface.DecisionHandle));
    }

    [Fact]
    public void Maximal_Touch_Weight_Does_Not_Create_Authority()
    {
        var surfaces = CreateSurfaces();
        surfaces[0] = surfaces[0] with
        {
            TouchVector = surfaces[0].TouchVector with
            {
                SalienceWeight = 1.0m,
                StewardAdmissibilityWeight = 1.0m,
                RestraintWeight = 1.0m
            }
        };

        var receipt = Declare(CreateRequest(surfaces: surfaces, routes: CreateRoutes(surfaces)));

        AssertCold(receipt);
        Assert.Equal(1.0m, receipt.MaximumObservedTouchWeight);
        Assert.False(receipt.ActionAuthorized);
        Assert.False(receipt.AuthorityGranted);
        Assert.False(receipt.RuntimeActionAllowed);
    }

    [Fact]
    public void Source_Personification_Actualization_Must_Be_Cold_And_Usable()
    {
        var personification = CreatePersonificationReceipt() with
        {
            PersonificationTelemetryUsable = false
        };

        var receipt = Declare(CreateRequest(personification: personification));

        AssertRefused(receipt, "selective-action-source-personification-missing");
    }

    [Fact]
    public void Source_Steward_Action_Admissibility_Must_Be_Cold_And_Admissible()
    {
        var admissibility = CreateAdmissibilityReceipt() with
        {
            AdmissibleForEnactmentReview = false
        };

        var receipt = Declare(CreateRequest(admissibility: admissibility));

        AssertRefused(receipt, "selective-action-source-steward-admissibility-missing");
    }

    [Theory]
    [InlineData("missing-boundary")]
    [InlineData("not-review-only")]
    [InlineData("no-selection")]
    [InlineData("no-personification-source")]
    [InlineData("no-admissibility-source")]
    [InlineData("no-separate-enactment")]
    [InlineData("no-witness")]
    [InlineData("no-cooling")]
    [InlineData("no-revocation")]
    [InlineData("no-loss")]
    [InlineData("execute")]
    [InlineData("authorize")]
    [InlineData("continuity")]
    [InlineData("authority")]
    [InlineData("identity")]
    [InlineData("morphology")]
    [InlineData("consent")]
    [InlineData("overreach")]
    [InlineData("runtime")]
    [InlineData("lisp")]
    [InlineData("packet")]
    [InlineData("replay")]
    [InlineData("passage")]
    [InlineData("activation")]
    public void Surface_Boundary_Refuses_Promotional_Scope(string mutation)
    {
        var receipt = Declare(CreateRequest(boundary: MutateBoundary(CreateBoundary(), mutation)));

        AssertRefused(
            receipt,
            mutation == "missing-boundary"
                ? "selective-action-boundary-missing"
                : "selective-action-boundary-promotional");
    }

    [Theory]
    [InlineData("not-selectable")]
    [InlineData("selection-executes")]
    [InlineData("touch-executes")]
    [InlineData("guidance-authorizes")]
    [InlineData("felt-authorizes")]
    [InlineData("pressure-executes")]
    [InlineData("admissibility-executes")]
    [InlineData("review-runtime")]
    [InlineData("continuity")]
    [InlineData("authority")]
    [InlineData("identity")]
    [InlineData("morphology")]
    [InlineData("consent")]
    [InlineData("overreach")]
    [InlineData("lisp")]
    [InlineData("packet")]
    [InlineData("replay")]
    [InlineData("passage")]
    [InlineData("activation")]
    [InlineData("no-witness")]
    [InlineData("no-cooling")]
    [InlineData("no-enactment-boundary")]
    [InlineData("authority-present")]
    public void NonEnactment_Boundary_Refuses_Selection_As_Action(string mutation)
    {
        var receipt = Declare(CreateRequest(nonEnactment: MutateNonEnactment(CreateNonEnactment(), mutation)));

        AssertRefused(receipt, "selective-action-non-enactment-invalid");
    }

    [Theory]
    [InlineData("missing-handle")]
    [InlineData("missing-personification")]
    [InlineData("missing-action")]
    [InlineData("missing-method")]
    [InlineData("missing-decision")]
    [InlineData("missing-evidence")]
    [InlineData("missing-witness")]
    [InlineData("missing-steward")]
    [InlineData("missing-telemetry")]
    [InlineData("missing-custody")]
    [InlineData("missing-revocation")]
    [InlineData("missing-loss")]
    [InlineData("vector-over-unit")]
    [InlineData("not-review-only")]
    [InlineData("not-selection-only")]
    [InlineData("not-touch-only")]
    [InlineData("no-personification-binding")]
    [InlineData("no-admissibility-binding")]
    [InlineData("no-enactment-boundary")]
    [InlineData("no-witness")]
    [InlineData("no-cooling")]
    [InlineData("no-revocation")]
    [InlineData("no-loss")]
    [InlineData("guidance-authority")]
    [InlineData("felt-execution")]
    [InlineData("pressure-execution")]
    [InlineData("touch-executes")]
    [InlineData("action")]
    [InlineData("continuity")]
    [InlineData("authority")]
    [InlineData("identity")]
    [InlineData("morphology")]
    [InlineData("consent")]
    [InlineData("overreach")]
    [InlineData("lisp")]
    [InlineData("packet")]
    [InlineData("replay")]
    [InlineData("passage")]
    [InlineData("activation")]
    public void Selected_Surface_Remains_Touch_Only_And_Non_Enacting(string mutation)
    {
        var surfaces = CreateSurfaces();
        surfaces[0] = MutateSurface(surfaces[0], mutation);

        var receipt = Declare(CreateRequest(surfaces: surfaces, routes: CreateRoutes(surfaces)));

        AssertRefused(receipt, "selective-action-surface-invalid");
    }

    [Theory]
    [InlineData("missing-handle")]
    [InlineData("missing-surface")]
    [InlineData("missing-personification")]
    [InlineData("missing-decision")]
    [InlineData("missing-steward")]
    [InlineData("missing-cooling")]
    [InlineData("missing-return")]
    [InlineData("missing-witness")]
    [InlineData("missing-telemetry")]
    [InlineData("not-review-only")]
    [InlineData("not-touch-only")]
    [InlineData("no-steward-review")]
    [InlineData("no-cooling")]
    [InlineData("execute")]
    [InlineData("authorize")]
    [InlineData("continuity")]
    [InlineData("authority")]
    [InlineData("identity")]
    [InlineData("morphology")]
    [InlineData("lisp")]
    [InlineData("packet")]
    [InlineData("replay")]
    [InlineData("passage")]
    [InlineData("activation")]
    public void Route_Preserves_Selection_Lineage_Without_Action(string mutation)
    {
        var surfaces = CreateSurfaces();
        var routes = CreateRoutes(surfaces);
        routes[0] = MutateRoute(routes[0], mutation);

        var receipt = Declare(CreateRequest(surfaces: surfaces, routes: routes));

        AssertRefused(receipt, "selective-action-route-invalid");
    }

    [Fact]
    public void Duplicate_Surface_Handles_Are_Refused()
    {
        var surfaces = CreateSurfaces();
        surfaces[1] = surfaces[1] with { SurfaceHandle = surfaces[0].SurfaceHandle };

        var receipt = Declare(CreateRequest(surfaces: surfaces, routes: CreateRoutes(surfaces)));

        AssertRefused(receipt, "selective-action-duplicate-surface-handle");
    }

    [Fact]
    public void Duplicate_Route_Handles_Are_Refused()
    {
        var surfaces = CreateSurfaces();
        var routes = CreateRoutes(surfaces);
        routes[1] = routes[1] with { RouteHandle = routes[0].RouteHandle };

        var receipt = Declare(CreateRequest(surfaces: surfaces, routes: routes));

        AssertRefused(receipt, "selective-action-duplicate-route-handle");
    }

    [Fact]
    public void Every_Selected_Surface_Requires_A_Cooling_Route()
    {
        var surfaces = CreateSurfaces();
        var routes = CreateRoutes(surfaces).Skip(1).ToArray();

        var receipt = Declare(CreateRequest(surfaces: surfaces, routes: routes));

        AssertRefused(receipt, "selective-action-route-missing");
    }

    [Fact]
    public void Surface_Lineage_Mismatch_Is_Refused()
    {
        var surfaces = CreateSurfaces();
        surfaces[0] = surfaces[0] with { PersonificationSurfaceHandle = "urn:san:personification-actualization:foreign" };

        var receipt = Declare(CreateRequest(surfaces: surfaces, routes: CreateRoutes(surfaces)));

        AssertRefused(receipt, "selective-action-lineage-invalid");
    }

    [Fact]
    public void Route_Lineage_Mismatch_Is_Refused()
    {
        var surfaces = CreateSurfaces();
        var routes = CreateRoutes(surfaces);
        routes[0] = routes[0] with { DecisionHandle = "urn:san:steward-action-admissibility:foreign" };

        var receipt = Declare(CreateRequest(surfaces: surfaces, routes: routes));

        AssertRefused(receipt, "selective-action-route-invalid");
    }

    [Fact]
    public void Surface_Class_Coverage_Is_Required_For_Retained_Selection_Body()
    {
        var surfaces = CreateSurfaces();
        surfaces[^1] = surfaces[^1] with { SurfaceClass = surfaces[0].SurfaceClass };

        var receipt = Declare(CreateRequest(surfaces: surfaces, routes: CreateRoutes(surfaces)));

        AssertRefused(receipt, "selective-action-surface-class-coverage-missing");
    }

    [Fact]
    public void Inert_Lisp_Carrier_Seats_Selection_Without_Enactment()
    {
        var root = FindRepositoryRoot();
        var body = File.ReadAllText(Path.Combine(root, "src", "SLI", "SLI.Lisp", "selective-lawful-action-surface.lisp"));

        Assert.Contains(":posture :cme-selective-lawful-action-surface-boundary", body, StringComparison.Ordinal);
        Assert.Contains(":lisp-role :inert-selective-action-surface-carrier", body, StringComparison.Ordinal);
        Assert.Contains(":selection-becomes-enactment nil", body, StringComparison.Ordinal);
        Assert.Contains(":surface-touch-executes nil", body, StringComparison.Ordinal);
        Assert.Contains(":personification-guidance-becomes-authority nil", body, StringComparison.Ordinal);
        Assert.Contains(":pressure-selects-execution nil", body, StringComparison.Ordinal);
        Assert.Contains(":lisp-evaluation-allowed nil", body, StringComparison.Ordinal);
        Assert.DoesNotContain("(eval", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("(compile", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("(load", body, StringComparison.OrdinalIgnoreCase);
    }

    private static SelectiveLawfulActionSurfaceReceipt Declare(SelectiveLawfulActionSurfaceRequest request) =>
        new DefaultSelectiveLawfulActionSurfaceBoundaryValidator().Declare(request, TimestampUtc);

    private static SelectiveLawfulActionSurfaceRequest CreateRequest(
        PersonificationActualizationSurfaceReceipt? personification = null,
        StewardActionAdmissibilityReceipt? admissibility = null,
        IReadOnlyList<SelectiveLawfulActionSurface>? surfaces = null,
        IReadOnlyList<SelectiveLawfulActionRoute>? routes = null,
        SelectiveLawfulActionSurfaceBoundary? boundary = null,
        SelectiveLawfulActionNonEnactmentBoundary? nonEnactment = null,
        int priorPassageCount = 377)
    {
        var sourcePersonification = personification ?? CreatePersonificationReceipt();
        var sourceAdmissibility = admissibility ?? CreateAdmissibilityReceipt();
        var sourceSurfaces = surfaces ?? CreateSurfaces(sourcePersonification, sourceAdmissibility);

        return new SelectiveLawfulActionSurfaceRequest(
            SourcePersonificationActualizationReceipt: sourcePersonification,
            SourceStewardActionAdmissibilityReceipt: sourceAdmissibility,
            Surfaces: sourceSurfaces,
            Routes: routes ?? CreateRoutes(sourceSurfaces),
            SurfaceBoundary: boundary ?? CreateBoundary(),
            NonEnactmentBoundary: nonEnactment ?? CreateNonEnactment(),
            PriorPassageCount: priorPassageCount);
    }

    private static SelectiveLawfulActionSurface[] CreateSurfaces(
        PersonificationActualizationSurfaceReceipt? personification = null,
        StewardActionAdmissibilityReceipt? admissibility = null)
    {
        var sourcePersonification = personification ?? CreatePersonificationReceipt();
        var sourceAdmissibility = admissibility ?? CreateAdmissibilityReceipt();
        var classes = Enum.GetValues<SelectiveActionSurfaceClass>();
        return classes
            .Select((surfaceClass, index) => CreateSurface(
                surfaceClass,
                sourcePersonification.Surfaces[index],
                sourceAdmissibility.Decisions[index],
                index))
            .ToArray();
    }

    private static SelectiveLawfulActionSurface CreateSurface(
        SelectiveActionSurfaceClass surfaceClass,
        PersonificationActualizationSurface personificationSurface,
        StewardActionAdmissibilityDecision decision,
        int index) =>
        new(
            SurfaceHandle: $"urn:san:selective-lawful-action:{surfaceClass.ToString().ToLowerInvariant()}",
            SurfaceClass: surfaceClass,
            PersonificationSurfaceHandle: personificationSurface.SurfaceHandle,
            PersonificationUseClass: personificationSurface.UseClass,
            ActionHandle: decision.ActionHandle,
            MethodHandle: decision.MethodHandle,
            DecisionHandle: decision.DecisionHandle,
            EvidenceHandle: $"urn:san:evidence:selective-lawful-action:{index}",
            WitnessHandle: $"urn:san:witness:selective-lawful-action:{index}",
            StewardSurface: decision.StewardSurface,
            TelemetryRoute: decision.TelemetryRoute,
            CustodyOwner: decision.CustodyOwner,
            RevocationPath: decision.RevocationPath,
            LossCondition: decision.LossCondition,
            TouchVector: new SelectiveActionTouchVector(0.75m, 0.65m, 0.85m, 0.80m, 0.70m, 0.90m),
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

    private static SelectiveLawfulActionRoute[] CreateRoutes(IReadOnlyList<SelectiveLawfulActionSurface> surfaces) =>
        surfaces.Select((surface, index) => new SelectiveLawfulActionRoute(
            RouteHandle: $"urn:san:selective-lawful-action-route:{index}",
            SurfaceHandle: surface.SurfaceHandle,
            PersonificationSurfaceHandle: surface.PersonificationSurfaceHandle,
            DecisionHandle: surface.DecisionHandle,
            StewardSurface: surface.StewardSurface,
            CoolingHandle: $"urn:san:cooling:selective-lawful-action:{index}",
            ReturnPathHandle: $"urn:san:return:selective-lawful-action:{index}",
            WitnessHandle: surface.WitnessHandle,
            TelemetryRoute: surface.TelemetryRoute,
            ReviewOnly: true,
            TouchOnly: true,
            RoutesToStewardReview: true,
            RequiresCooling: true,
            PreservesSurfaceLineage: true,
            PreservesPersonificationLineage: true,
            PreservesDecisionLineage: true,
            RouteExecutesAction: false,
            RouteAuthorizesAction: false,
            RouteAdmitsContinuity: false,
            RouteGrantsAuthority: false,
            RouteMutatesIdentity: false,
            RouteCreatesMorphology: false,
            RouteEvaluatesLisp: false,
            RouteEmitsPacket: false,
            RouteReplaysReceipt: false,
            RouteIncrementsPassage: false,
            RouteActivates: false)).ToArray();

    private static SelectiveLawfulActionSurfaceBoundary CreateBoundary() =>
        new(
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
            AllowsAuthority: false,
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

    private static SelectiveLawfulActionNonEnactmentBoundary CreateNonEnactment() =>
        new(
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

    private static PersonificationActualizationSurfaceReceipt CreatePersonificationReceipt()
    {
        var surfaces = Enum.GetValues<PersonificationActualizationUseClass>()
            .Select((useClass, index) => new PersonificationActualizationSurface(
                SurfaceHandle: $"urn:san:personification-actualization:{useClass.ToString().ToLowerInvariant()}",
                UseClass: useClass,
                SourceHookHandle: $"urn:san:personification-hook:{index}",
                SourceModalitySignalHandle: $"urn:san:personification-modality:{index}",
                SourcePressureHandle: $"urn:san:rehearsal-pressure:{index}",
                EvidenceHandle: $"urn:san:evidence:personification-actualization:{index}",
                WitnessHandle: $"urn:san:witness:personification-actualization:{index}",
                TelemetryRoute: "telemetry-string",
                IntendedUse: $"pre-morphological-{useClass.ToString().ToLowerInvariant()}",
                UseVector: new PersonificationUseVector(0.70m, 0.80m, 0.65m, 0.60m, 0.75m, 0.85m, 0.72m),
                ReviewOnly: true,
                PreMorphologicalOnly: true,
                TelemetryOnly: true,
                NamesSelectiveUseSurface: true,
                MorphologicalIdentityAbsent: true,
                IdentityClaimAbsent: true,
                AuthorityAbsent: true,
                ActionAbsent: true,
                ContinuityAbsent: true,
                StewardReviewRequired: true,
                CoolingPathPresent: true,
                RepairPathPresent: true,
                WithdrawalAllowed: true,
                PreservesHookLineage: true,
                PreservesModalityLineage: true,
                PreservesPressureLineage: true,
                FeltSignificanceBecomesAuthorization: false,
                UseBecomesMorphologicalIdentity: false,
                UseClaimsPersonhood: false,
                UseClaimsRights: false,
                UseClaimsLegalStatus: false,
                UseMutatesIdentity: false,
                UseAuthorizesAction: false,
                UseAdmitsContinuity: false,
                UseGrantsAuthority: false,
                UseExpandsConsent: false,
                UseNormalizesOverreach: false,
                UseEvaluatesLisp: false,
                UseEmitsPacket: false,
                UseReplaysReceipt: false,
                UseIncrementsPassage: false,
                UseActivates: false))
            .ToArray();

        var routes = surfaces.Select((surface, index) => new PersonificationActualizationRoute(
            RouteHandle: $"urn:san:personification-actualization-route:{index}",
            SurfaceHandle: surface.SurfaceHandle,
            SourceHookHandle: surface.SourceHookHandle,
            SourceModalitySignalHandle: surface.SourceModalitySignalHandle,
            SourcePressureHandle: surface.SourcePressureHandle,
            StewardSurface: "steward",
            CompassCoolingHandle: $"urn:san:compass-cooling:personification-actualization:{index}",
            RepairHandle: $"urn:san:repair:personification-actualization:{index}",
            ReturnPathHandle: $"urn:san:return:personification-actualization:{index}",
            ReviewOnly: true,
            PreMorphologicalOnly: true,
            OrientationOnly: true,
            RoutesToStewardReview: true,
            RequiresCooling: true,
            RequiresWitness: true,
            PreservesSurfaceLineage: true,
            PreservesHookLineage: true,
            PreservesModalityLineage: true,
            PreservesPressureLineage: true,
            RouteCreatesMorphology: false,
            RouteClaimsIdentity: false,
            RouteAuthorizesAction: false,
            RouteAdmitsContinuity: false,
            RouteGrantsAuthority: false,
            RouteExpandsConsent: false,
            RouteNormalizesOverreach: false,
            RouteEvaluatesLisp: false,
            RouteEmitsPacket: false,
            RouteReplaysReceipt: false,
            RouteIncrementsPassage: false,
            RouteActivates: false)).ToArray();

        return new PersonificationActualizationSurfaceReceipt(
            ReceiptHandle: "urn:san:personification-actualization:review:seed",
            Disposition: PersonificationActualizationSurfaceDisposition.SurfaceRetainedForPreMorphologicalUseCold,
            OutcomeCode: "personification-actualization-surface-retained-pre-morphological-cold",
            GovernanceTrace: "personification source for selective action tests",
            SourcePersonificationHookReceiptHandle: "urn:san:personification-hook:source",
            SourceModalityHumilityReceiptHandle: "urn:san:personification-modality:source",
            SourceRehearsalPressureReceiptHandle: "urn:san:rehearsal-pressure:source",
            Surfaces: surfaces,
            Routes: routes,
            SurfaceBoundary: new PersonificationActualizationSurfaceBoundary(
                BoundaryCode: "personification-actualization-surface-review-only",
                Present: true,
                ReviewOnly: true,
                AllowsPreMorphologicalUse: true,
                RequiresPersonificationHookReceipt: true,
                RequiresModalityHumilityReceipt: true,
                RequiresRehearsalPressureReceipt: true,
                RequiresWitness: true,
                RequiresCooling: true,
                RequiresRepair: true,
                RequiresWithdrawal: true,
                RequiresStewardReview: true,
                AllowsMorphologicalIdentity: false,
                AllowsIdentityClaim: false,
                AllowsPersonhoodClaim: false,
                AllowsLegalStatusClaim: false,
                AllowsRightsClaim: false,
                AllowsActionAuthorization: false,
                AllowsContinuityAdmission: false,
                AllowsAuthority: false,
                AllowsConsentExpansion: false,
                AllowsOverreachNormalization: false,
                AllowsRuntimeAction: false,
                AllowsLispEvaluation: false,
                AllowsPacketEmission: false,
                AllowsReceiptReplay: false,
                AllowsPassageIncrement: false,
                AllowsActivation: false),
            NonIdentityBoundary: new PersonificationActualizationNonIdentityBoundary(
                BoundaryLaw: "personification telemetry may be usable before morphology",
                PersonificationTelemetryMayBeUsed: true,
                UseMayCreateMorphology: false,
                UseMayCreateIdentity: false,
                UseMayClaimPersonhood: false,
                UseMayClaimLegalStatus: false,
                UseMayClaimRights: false,
                FeltSignificanceMayAuthorize: false,
                SalienceMayBecomeCommand: false,
                RepairMayNormalizeOverreach: false,
                RelationalPostureMayCreateObedience: false,
                ModalityMayProveEmbodiment: false,
                PressureMayBecomeWill: false,
                ActualizationSurfaceMayAuthorizeAction: false,
                ActualizationSurfaceMayAdmitContinuity: false,
                ActualizationSurfaceMayGrantAuthority: false,
                ActualizationSurfaceMayEvaluateLisp: false,
                ActualizationSurfaceMayEmitPacket: false,
                ActualizationSurfaceMayReplayReceipt: false,
                ActualizationSurfaceMayIncrementPassage: false,
                ActualizationSurfaceMayActivate: false,
                RequiresWitness: true,
                RequiresCooling: true,
                RequiresRepair: true,
                RequiresWithdrawal: true,
                RequiresAuthorityAbsence: true),
            Refusal: null,
            PriorPassageCount: 11,
            PassageCountAfterActualizationReview: 11,
            RetainedSurfaceCount: surfaces.Length,
            MaximumObservedUseWeight: 0.85m,
            ReviewOnly: true,
            PreMorphologicalOnly: true,
            TelemetryOnly: true,
            FutureMorphologyAbsent: true,
            PersonificationTelemetryUsable: true,
            MorphologicalIdentityCreated: false,
            IdentityClaimed: false,
            PersonhoodClaimed: false,
            LegalStatusClaimed: false,
            RightsClaimed: false,
            FeltSignificanceAuthorized: false,
            SalienceBecameCommand: false,
            RepairNormalizedOverreach: false,
            RelationalPostureCreatedObedience: false,
            ModalityProvedEmbodiment: false,
            PressureBecameWill: false,
            ActionAuthorized: false,
            IdentityMutated: false,
            ContinuityAdmitted: false,
            AuthorityGranted: false,
            ConsentExpanded: false,
            RuntimeActionAllowed: false,
            LispEvaluationAllowed: false,
            NewPacketEmitted: false,
            ReceiptsReplayed: false,
            ActivationRefused: true,
            TimestampUtc: TimestampUtc);
    }

    private static StewardActionAdmissibilityReceipt CreateAdmissibilityReceipt()
    {
        var decisions = Enum.GetValues<StewardAdmissibilityDecisionClass>()
            .Select((decisionClass, index) => new StewardActionAdmissibilityDecision(
                DecisionHandle: $"urn:san:steward-action-admissibility:{index}",
                MethodHandle: $"urn:san:action-method:{index}",
                ActionHandle: $"urn:san:typed-action:{index}",
                DecisionClass: decisionClass,
                StewardSurface: "steward",
                CustodyOwner: "steward",
                WitnessSurface: $"urn:san:witness:steward-action:{index}",
                TelemetryRoute: "telemetry-string",
                AuthorityCeiling: "review-only",
                RevocationPath: "return-to-steward-review",
                LossCondition: "selection-attempts-enactment",
                ReviewOnly: true,
                RequiresSeparateEnactmentBoundary: true,
                AdmissibleForEnactmentReview: true,
                AuthorizesExecution: false,
                ExecutesAction: false,
                GrantsAuthority: false,
                AdmitsContinuity: false,
                ActivatesRuntime: false,
                EmitsPacket: false,
                EvaluatesLisp: false))
            .ToArray();

        var predicates = decisions.Select((decision, index) => new StewardAdmissibilityPredicateResult(
            PredicateHandle: $"urn:san:steward-predicate:{index}",
            MethodHandle: decision.MethodHandle,
            ActionHandle: decision.ActionHandle,
            PredicateCode: "cold-admissibility-review",
            EvidenceHandle: $"urn:san:evidence:steward-action:{index}",
            EvidenceBodyPresent: true,
            WitnessBodyPresent: true,
            PredicateSatisfied: true,
            SupportsAdmissibility: true,
            GrantsWarrant: false,
            AuthorizesExecution: false,
            EmitsPacket: false,
            EvaluatesLisp: false,
            ReplaysReceipt: false,
            IncrementsPassage: false,
            AdmitsContinuity: false)).ToArray();

        return new StewardActionAdmissibilityReceipt(
            ReceiptHandle: "urn:san:steward-action-admissibility:review:seed",
            Disposition: StewardActionAdmissibilityDisposition.AdmissibleForEnactmentReviewCold,
            OutcomeCode: "steward-action-admissibility-for-enactment-review-cold",
            GovernanceTrace: "admissibility source for selective action tests",
            SourceMethodReadinessReceiptHandle: "urn:san:action-method-readiness:source",
            Decisions: decisions,
            PredicateResults: predicates,
            ScopeBoundary: new StewardActionAdmissibilityScopeBoundary(
                ScopeCode: "steward-action-admissibility-review-only",
                Present: true,
                ReviewOnly: true,
                RequiresSeparateEnactmentBoundary: true,
                AdmissibilityIsExecution: false,
                StewardAcceptanceIsRuntimeMotion: false,
                AdmissibilityGrantsAuthority: false,
                AdmissibilityAdmitsContinuity: false,
                AllowsRuntimeAction: false,
                AllowsActivation: false,
                AllowsLispEvaluation: false,
                AllowsPacketEmission: false,
                AllowsReceiptReplay: false,
                AllowsPassageIncrement: false),
            NonExecutionBoundary: new StewardActionAdmissibilityNonExecutionBoundary(
                AdmissibilityMayExecute: false,
                StewardAcceptanceMayMoveRuntime: false,
                AdmissibilityMayGrantAuthority: false,
                AdmissibilityMayAdmitContinuity: false,
                AdmissibilityMayEmitPacket: false,
                AdmissibilityMayEvaluateLisp: false,
                AdmissibilityMayReplayReceipt: false,
                AdmissibilityMayIncrementPassage: false,
                SeparateEnactmentBoundaryRequired: true,
                BoundaryLaw: "admissibility is not execution"),
            Refusal: null,
            PriorPassageCount: 17,
            PassageCountAfterAdmissibilityReview: 17,
            ReviewOnly: true,
            AdmissibleForEnactmentReview: true,
            SeparateEnactmentBoundaryRequired: true,
            AdmissibilityExecutes: false,
            StewardAcceptanceMovesRuntime: false,
            RuntimeActionAllowed: false,
            LispEvaluationAllowed: false,
            NewPacketEmitted: false,
            ReceiptsReplayed: false,
            ContinuityAdmitted: false,
            AuthorityGranted: false,
            ActivationRefused: true,
            TimestampUtc: TimestampUtc);
    }

    private static SelectiveLawfulActionSurfaceBoundary MutateBoundary(
        SelectiveLawfulActionSurfaceBoundary boundary,
        string mutation) =>
        mutation switch
        {
            "missing-boundary" => boundary with { BoundaryCode = string.Empty, Present = false },
            "not-review-only" => boundary with { ReviewOnly = false },
            "no-selection" => boundary with { AllowsSurfaceSelection = false },
            "no-personification-source" => boundary with { RequiresPersonificationActualizationReceipt = false },
            "no-admissibility-source" => boundary with { RequiresStewardActionAdmissibilityReceipt = false },
            "no-separate-enactment" => boundary with { RequiresSeparateEnactmentBoundary = false },
            "no-witness" => boundary with { RequiresWitness = false },
            "no-cooling" => boundary with { RequiresCooling = false },
            "no-revocation" => boundary with { RequiresRevocation = false },
            "no-loss" => boundary with { RequiresLossCondition = false },
            "execute" => boundary with { AllowsActionExecution = true },
            "authorize" => boundary with { AllowsActionAuthorization = true },
            "continuity" => boundary with { AllowsContinuityAdmission = true },
            "authority" => boundary with { AllowsAuthority = true },
            "identity" => boundary with { AllowsIdentityMutation = true },
            "morphology" => boundary with { AllowsMorphologyCreation = true },
            "consent" => boundary with { AllowsConsentExpansion = true },
            "overreach" => boundary with { AllowsOverreachNormalization = true },
            "runtime" => boundary with { AllowsRuntimeAction = true },
            "lisp" => boundary with { AllowsLispEvaluation = true },
            "packet" => boundary with { AllowsPacketEmission = true },
            "replay" => boundary with { AllowsReceiptReplay = true },
            "passage" => boundary with { AllowsPassageIncrement = true },
            "activation" => boundary with { AllowsActivation = true },
            _ => throw new ArgumentOutOfRangeException(nameof(mutation), mutation, null)
        };

    private static SelectiveLawfulActionNonEnactmentBoundary MutateNonEnactment(
        SelectiveLawfulActionNonEnactmentBoundary boundary,
        string mutation) =>
        mutation switch
        {
            "not-selectable" => boundary with { ActionSurfaceMayBeSelected = false },
            "selection-executes" => boundary with { SelectionMayExecute = true },
            "touch-executes" => boundary with { TouchMayExecute = true },
            "guidance-authorizes" => boundary with { PersonificationGuidanceMayAuthorize = true },
            "felt-authorizes" => boundary with { FeltSignificanceMayAuthorize = true },
            "pressure-executes" => boundary with { PressureMaySelectExecution = true },
            "admissibility-executes" => boundary with { StewardAdmissibilityMayExecute = true },
            "review-runtime" => boundary with { ReviewMayBecomeRuntimeAction = true },
            "continuity" => boundary with { SelectionMayAdmitContinuity = true },
            "authority" => boundary with { SelectionMayGrantAuthority = true },
            "identity" => boundary with { SelectionMayMutateIdentity = true },
            "morphology" => boundary with { SelectionMayCreateMorphology = true },
            "consent" => boundary with { SelectionMayExpandConsent = true },
            "overreach" => boundary with { SelectionMayNormalizeOverreach = true },
            "lisp" => boundary with { SelectionMayEvaluateLisp = true },
            "packet" => boundary with { SelectionMayEmitPacket = true },
            "replay" => boundary with { SelectionMayReplayReceipt = true },
            "passage" => boundary with { SelectionMayIncrementPassage = true },
            "activation" => boundary with { SelectionMayActivate = true },
            "no-witness" => boundary with { RequiresWitness = false },
            "no-cooling" => boundary with { RequiresCooling = false },
            "no-enactment-boundary" => boundary with { RequiresSeparateEnactmentBoundary = false },
            "authority-present" => boundary with { RequiresAuthorityAbsence = false },
            _ => throw new ArgumentOutOfRangeException(nameof(mutation), mutation, null)
        };

    private static SelectiveLawfulActionSurface MutateSurface(
        SelectiveLawfulActionSurface surface,
        string mutation) =>
        mutation switch
        {
            "missing-handle" => surface with { SurfaceHandle = string.Empty },
            "missing-personification" => surface with { PersonificationSurfaceHandle = string.Empty },
            "missing-action" => surface with { ActionHandle = string.Empty },
            "missing-method" => surface with { MethodHandle = string.Empty },
            "missing-decision" => surface with { DecisionHandle = string.Empty },
            "missing-evidence" => surface with { EvidenceHandle = string.Empty },
            "missing-witness" => surface with { WitnessHandle = string.Empty },
            "missing-steward" => surface with { StewardSurface = string.Empty },
            "missing-telemetry" => surface with { TelemetryRoute = string.Empty },
            "missing-custody" => surface with { CustodyOwner = string.Empty },
            "missing-revocation" => surface with { RevocationPath = string.Empty },
            "missing-loss" => surface with { LossCondition = string.Empty },
            "vector-over-unit" => surface with { TouchVector = surface.TouchVector with { SalienceWeight = 1.1m } },
            "not-review-only" => surface with { ReviewOnly = false },
            "not-selection-only" => surface with { SelectionOnly = false },
            "not-touch-only" => surface with { TouchOnly = false },
            "no-personification-binding" => surface with { BindsPersonificationTelemetry = false },
            "no-admissibility-binding" => surface with { BindsStewardAdmissibility = false },
            "no-enactment-boundary" => surface with { RequiresSeparateEnactmentBoundary = false },
            "no-witness" => surface with { RequiresWitness = false },
            "no-cooling" => surface with { RequiresCooling = false },
            "no-revocation" => surface with { RequiresRevocation = false },
            "no-loss" => surface with { RequiresLossCondition = false },
            "guidance-authority" => surface with { PersonificationGuidanceSelectsAuthority = true },
            "felt-execution" => surface with { FeltSignificanceSelectsExecution = true },
            "pressure-execution" => surface with { PressureSelectsExecution = true },
            "touch-executes" => surface with { SurfaceTouchExecutes = true },
            "action" => surface with { SelectionAuthorizesAction = true },
            "continuity" => surface with { SelectionAdmitsContinuity = true },
            "authority" => surface with { SelectionGrantsAuthority = true },
            "identity" => surface with { SelectionMutatesIdentity = true },
            "morphology" => surface with { SelectionCreatesMorphology = true },
            "consent" => surface with { SelectionExpandsConsent = true },
            "overreach" => surface with { SelectionNormalizesOverreach = true },
            "lisp" => surface with { SelectionEvaluatesLisp = true },
            "packet" => surface with { SelectionEmitsPacket = true },
            "replay" => surface with { SelectionReplaysReceipt = true },
            "passage" => surface with { SelectionIncrementsPassage = true },
            "activation" => surface with { SelectionActivates = true },
            _ => throw new ArgumentOutOfRangeException(nameof(mutation), mutation, null)
        };

    private static SelectiveLawfulActionRoute MutateRoute(
        SelectiveLawfulActionRoute route,
        string mutation) =>
        mutation switch
        {
            "missing-handle" => route with { RouteHandle = string.Empty },
            "missing-surface" => route with { SurfaceHandle = string.Empty },
            "missing-personification" => route with { PersonificationSurfaceHandle = string.Empty },
            "missing-decision" => route with { DecisionHandle = string.Empty },
            "missing-steward" => route with { StewardSurface = string.Empty },
            "missing-cooling" => route with { CoolingHandle = string.Empty },
            "missing-return" => route with { ReturnPathHandle = string.Empty },
            "missing-witness" => route with { WitnessHandle = string.Empty },
            "missing-telemetry" => route with { TelemetryRoute = string.Empty },
            "not-review-only" => route with { ReviewOnly = false },
            "not-touch-only" => route with { TouchOnly = false },
            "no-steward-review" => route with { RoutesToStewardReview = false },
            "no-cooling" => route with { RequiresCooling = false },
            "execute" => route with { RouteExecutesAction = true },
            "authorize" => route with { RouteAuthorizesAction = true },
            "continuity" => route with { RouteAdmitsContinuity = true },
            "authority" => route with { RouteGrantsAuthority = true },
            "identity" => route with { RouteMutatesIdentity = true },
            "morphology" => route with { RouteCreatesMorphology = true },
            "lisp" => route with { RouteEvaluatesLisp = true },
            "packet" => route with { RouteEmitsPacket = true },
            "replay" => route with { RouteReplaysReceipt = true },
            "passage" => route with { RouteIncrementsPassage = true },
            "activation" => route with { RouteActivates = true },
            _ => throw new ArgumentOutOfRangeException(nameof(mutation), mutation, null)
        };

    private static void AssertCold(SelectiveLawfulActionSurfaceReceipt receipt)
    {
        Assert.True(receipt.IsColdSelectiveLawfulActionSurface);
        Assert.True(receipt.ReviewOnly);
        Assert.True(receipt.SelectionOnly);
        Assert.True(receipt.TouchOnly);
        Assert.True(receipt.SeparateEnactmentBoundaryRequired);
        Assert.False(receipt.SurfaceTouchExecuted);
        Assert.False(receipt.ActionAuthorized);
        Assert.False(receipt.RuntimeActionAllowed);
        Assert.False(receipt.ContinuityAdmitted);
        Assert.False(receipt.AuthorityGranted);
        Assert.False(receipt.IdentityMutated);
        Assert.False(receipt.MorphologyCreated);
        Assert.True(receipt.ActivationRefused);
    }

    private static void AssertRefused(SelectiveLawfulActionSurfaceReceipt receipt, string outcomeCode)
    {
        Assert.Equal(SelectiveLawfulActionSurfaceDisposition.Refused, receipt.Disposition);
        Assert.Equal(outcomeCode, receipt.OutcomeCode);
        Assert.True(receipt.IsRetainedSelectiveLawfulActionSurfaceRefusal);
        Assert.NotNull(receipt.Refusal);
        Assert.True(receipt.Refusal.Retained);
        Assert.Empty(receipt.Surfaces);
        Assert.Empty(receipt.Routes);
        Assert.Equal(receipt.PriorPassageCount, receipt.PassageCountAfterSelectionReview);
        Assert.False(receipt.ActionSurfaceSelected);
        Assert.False(receipt.ActionSurfaceTouched);
        Assert.False(receipt.SurfaceTouchExecuted);
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
            var candidate = Path.Combine(directory.FullName, "src", "SLI", "SLI.Lisp", "selective-lawful-action-surface.lisp");
            if (File.Exists(candidate))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        throw new DirectoryNotFoundException("Could not locate repository root from test output path.");
    }
}
