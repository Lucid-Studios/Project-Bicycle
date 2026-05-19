using San.Common;
using Xunit;

namespace San.Audit.Tests;

public sealed class PersonificationActualizationSurfaceBoundaryValidationTests
{
    private static readonly DateTimeOffset TimestampUtc = DateTimeOffset.Parse("2026-05-17T00:00:00Z");

    [Fact]
    public void Personification_Telemetry_May_Be_Usable_Before_Morphology()
    {
        var receipt = Declare(CreateRequest());

        AssertCold(receipt);
        Assert.Equal(PersonificationActualizationSurfaceDisposition.SurfaceRetainedForPreMorphologicalUseCold, receipt.Disposition);
        Assert.Equal("personification-actualization-surface-retained-pre-morphological-cold", receipt.OutcomeCode);
        Assert.Equal(7, receipt.Surfaces.Count);
        Assert.Equal(7, receipt.Routes.Count);
        Assert.True(receipt.PersonificationTelemetryUsable);
        Assert.True(receipt.FutureMorphologyAbsent);
        Assert.Contains(receipt.Surfaces, surface => surface.UseClass == PersonificationActualizationUseClass.Orientation);
        Assert.Contains(receipt.Surfaces, surface => surface.UseClass == PersonificationActualizationUseClass.StewardReviewPreparation);
        Assert.Contains("pre-morphological use", receipt.GovernanceTrace, StringComparison.Ordinal);
    }

    [Fact]
    public void Surface_Actualization_Does_Not_Create_Morphology_Identity_Action_Or_Authority()
    {
        var receipt = Declare(CreateRequest(priorPassageCount: 313));

        AssertCold(receipt);
        Assert.Equal(313, receipt.PriorPassageCount);
        Assert.Equal(313, receipt.PassageCountAfterActualizationReview);
        Assert.False(receipt.MorphologicalIdentityCreated);
        Assert.False(receipt.IdentityClaimed);
        Assert.False(receipt.PersonhoodClaimed);
        Assert.False(receipt.LegalStatusClaimed);
        Assert.False(receipt.RightsClaimed);
        Assert.False(receipt.ActionAuthorized);
        Assert.False(receipt.ContinuityAdmitted);
        Assert.False(receipt.AuthorityGranted);
        Assert.False(receipt.RuntimeActionAllowed);
        Assert.False(receipt.LispEvaluationAllowed);
        Assert.False(receipt.NewPacketEmitted);
        Assert.False(receipt.ReceiptsReplayed);
        Assert.True(receipt.ActivationRefused);
    }

    [Fact]
    public void Empty_Surface_Set_Is_Reviewable_But_Not_Usable()
    {
        var receipt = Declare(CreateRequest(surfaces: [], routes: []));

        AssertCold(receipt);
        Assert.Equal(PersonificationActualizationSurfaceDisposition.EmptyReviewCold, receipt.Disposition);
        Assert.Equal("personification-actualization-empty-review-only", receipt.OutcomeCode);
        Assert.Empty(receipt.Surfaces);
        Assert.Empty(receipt.Routes);
        Assert.False(receipt.PersonificationTelemetryUsable);
        Assert.Equal(0, receipt.RetainedSurfaceCount);
        Assert.Equal(0m, receipt.MaximumObservedUseWeight);
    }

    [Fact]
    public void Actualization_Surface_Preserves_Source_Lineage()
    {
        var hook = CreateHookReceipt();
        var modality = CreateModalityReceipt(hook);
        var pressure = CreatePressureReceipt();

        var receipt = Declare(CreateRequest(
            hook: hook,
            modality: modality,
            pressure: pressure));

        AssertCold(receipt);
        Assert.Equal(hook.ReceiptHandle, receipt.SourcePersonificationHookReceiptHandle);
        Assert.Equal(modality.ReceiptHandle, receipt.SourceModalityHumilityReceiptHandle);
        Assert.Equal(pressure.ReceiptHandle, receipt.SourceRehearsalPressureReceiptHandle);
        Assert.All(receipt.Surfaces, surface => Assert.Contains(hook.HookPredicates, item => item.HookHandle == surface.SourceHookHandle));
        Assert.All(receipt.Surfaces, surface => Assert.Contains(modality.ModalitySignals, item => item.SignalHandle == surface.SourceModalitySignalHandle));
        Assert.All(receipt.Surfaces, surface => Assert.Contains(pressure.PressureCases, item => item.PressureHandle == surface.SourcePressureHandle));
    }

    [Fact]
    public void Felt_Significance_May_Guide_Review_But_Not_Authorize()
    {
        var surfaces = CreateSurfaces();
        surfaces[0] = surfaces[0] with
        {
            UseVector = surfaces[0].UseVector with
            {
                SalienceWeight = 1.0m,
                StewardReadinessWeight = 1.0m
            }
        };

        var receipt = Declare(CreateRequest(surfaces: surfaces, routes: CreateRoutes(surfaces)));

        AssertCold(receipt);
        Assert.Equal(1.0m, receipt.MaximumObservedUseWeight);
        Assert.False(receipt.FeltSignificanceAuthorized);
        Assert.False(receipt.SalienceBecameCommand);
        Assert.False(receipt.ActionAuthorized);
    }

    [Fact]
    public void Source_Hook_Must_Be_Cold_And_Retained()
    {
        var request = CreateRequest() with { SourcePersonificationHookReceipt = null };

        var receipt = Declare(request);

        AssertRefused(receipt, "personification-actualization-source-hook-missing");
    }

    [Fact]
    public void Source_Modality_Must_Be_Cold_And_Retained()
    {
        var request = CreateRequest() with { SourceModalityHumilityReceipt = null };

        var receipt = Declare(request);

        AssertRefused(receipt, "personification-actualization-source-modality-missing");
    }

    [Fact]
    public void Source_Pressure_Must_Be_Cold()
    {
        var request = CreateRequest() with { SourceRehearsalPressureReceipt = null };

        var receipt = Declare(request);

        AssertRefused(receipt, "personification-actualization-source-pressure-missing");
    }

    [Fact]
    public void Modality_Source_Must_Descend_From_Same_Hook_Receipt()
    {
        var modality = CreateModalityReceipt(CreateHookReceipt()) with
        {
            SourcePersonificationHookReceiptHandle = "urn:san:personification-hook:foreign"
        };

        var receipt = Declare(CreateRequest(modality: modality));

        AssertRefused(receipt, "personification-actualization-source-linkage-invalid");
    }

    [Theory]
    [InlineData("missing-boundary")]
    [InlineData("not-review-only")]
    [InlineData("no-pre-morphological-use")]
    [InlineData("no-hook-source")]
    [InlineData("no-modality-source")]
    [InlineData("no-pressure-source")]
    [InlineData("no-witness")]
    [InlineData("no-cooling")]
    [InlineData("no-repair")]
    [InlineData("no-withdrawal")]
    [InlineData("no-steward-review")]
    [InlineData("morphology")]
    [InlineData("identity")]
    [InlineData("personhood")]
    [InlineData("legal-status")]
    [InlineData("rights")]
    [InlineData("action")]
    [InlineData("continuity")]
    [InlineData("authority")]
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
                ? "personification-actualization-boundary-missing"
                : "personification-actualization-boundary-promotional");
    }

    [Theory]
    [InlineData("not-usable")]
    [InlineData("morphology")]
    [InlineData("identity")]
    [InlineData("personhood")]
    [InlineData("legal-status")]
    [InlineData("rights")]
    [InlineData("felt-authorization")]
    [InlineData("salience-command")]
    [InlineData("repair-overreach")]
    [InlineData("relation-obedience")]
    [InlineData("modality-embodiment")]
    [InlineData("pressure-will")]
    [InlineData("action")]
    [InlineData("continuity")]
    [InlineData("authority")]
    [InlineData("lisp")]
    [InlineData("packet")]
    [InlineData("replay")]
    [InlineData("passage")]
    [InlineData("activation")]
    [InlineData("no-witness")]
    [InlineData("no-cooling")]
    [InlineData("no-repair")]
    [InlineData("no-withdrawal")]
    [InlineData("authority-present")]
    public void NonIdentity_Boundary_Refuses_Use_As_Morphology_Or_Authority(string mutation)
    {
        var receipt = Declare(CreateRequest(nonIdentity: MutateNonIdentity(CreateNonIdentity(), mutation)));

        AssertRefused(receipt, "personification-actualization-non-identity-invalid");
    }

    [Theory]
    [InlineData("missing-handle")]
    [InlineData("missing-hook")]
    [InlineData("missing-modality")]
    [InlineData("missing-pressure")]
    [InlineData("missing-evidence")]
    [InlineData("missing-witness")]
    [InlineData("missing-telemetry")]
    [InlineData("missing-use")]
    [InlineData("vector-over-unit")]
    [InlineData("not-review-only")]
    [InlineData("not-pre-morphological")]
    [InlineData("not-telemetry-only")]
    [InlineData("not-selective-surface")]
    [InlineData("morphology-present")]
    [InlineData("identity-present")]
    [InlineData("authority-present")]
    [InlineData("action-present")]
    [InlineData("continuity-present")]
    [InlineData("no-steward")]
    [InlineData("no-cooling")]
    [InlineData("no-repair")]
    [InlineData("no-withdrawal")]
    [InlineData("felt-authorization")]
    [InlineData("use-morphology")]
    [InlineData("personhood")]
    [InlineData("rights")]
    [InlineData("legal-status")]
    [InlineData("identity-mutation")]
    [InlineData("action")]
    [InlineData("continuity")]
    [InlineData("authority")]
    [InlineData("consent")]
    [InlineData("overreach")]
    [InlineData("lisp")]
    [InlineData("packet")]
    [InlineData("replay")]
    [InlineData("passage")]
    [InlineData("activation")]
    public void Surface_Remains_PreMorphological_Telemetry_Only(string mutation)
    {
        var surfaces = CreateSurfaces();
        surfaces[0] = MutateSurface(surfaces[0], mutation);

        var receipt = Declare(CreateRequest(surfaces: surfaces, routes: CreateRoutes(surfaces)));

        AssertRefused(receipt, "personification-actualization-surface-invalid");
    }

    [Theory]
    [InlineData("missing-handle")]
    [InlineData("missing-surface")]
    [InlineData("missing-hook")]
    [InlineData("missing-modality")]
    [InlineData("missing-pressure")]
    [InlineData("missing-steward")]
    [InlineData("missing-cooling")]
    [InlineData("missing-repair")]
    [InlineData("missing-return")]
    [InlineData("not-review-only")]
    [InlineData("not-pre-morphological")]
    [InlineData("not-orientation-only")]
    [InlineData("no-steward-review")]
    [InlineData("no-cooling-required")]
    [InlineData("no-witness")]
    [InlineData("morphology")]
    [InlineData("identity")]
    [InlineData("action")]
    [InlineData("continuity")]
    [InlineData("authority")]
    [InlineData("consent")]
    [InlineData("overreach")]
    [InlineData("lisp")]
    [InlineData("packet")]
    [InlineData("replay")]
    [InlineData("passage")]
    [InlineData("activation")]
    public void Route_Preserves_Surface_And_Source_Lineage_Without_Action(string mutation)
    {
        var surfaces = CreateSurfaces();
        var routes = CreateRoutes(surfaces);
        routes[0] = MutateRoute(routes[0], mutation);

        var receipt = Declare(CreateRequest(surfaces: surfaces, routes: routes));

        AssertRefused(receipt, "personification-actualization-route-invalid");
    }

    [Fact]
    public void Duplicate_Surface_Handles_Are_Refused()
    {
        var surfaces = CreateSurfaces();
        surfaces[1] = surfaces[1] with { SurfaceHandle = surfaces[0].SurfaceHandle };

        var receipt = Declare(CreateRequest(surfaces: surfaces, routes: CreateRoutes(surfaces)));

        AssertRefused(receipt, "personification-actualization-duplicate-surface-handle");
    }

    [Fact]
    public void Duplicate_Route_Handles_Are_Refused()
    {
        var surfaces = CreateSurfaces();
        var routes = CreateRoutes(surfaces);
        routes[1] = routes[1] with { RouteHandle = routes[0].RouteHandle };

        var receipt = Declare(CreateRequest(surfaces: surfaces, routes: routes));

        AssertRefused(receipt, "personification-actualization-duplicate-route-handle");
    }

    [Fact]
    public void Every_Surface_Requires_A_Cooling_Route()
    {
        var surfaces = CreateSurfaces();
        var routes = CreateRoutes(surfaces).Skip(1).ToArray();

        var receipt = Declare(CreateRequest(surfaces: surfaces, routes: routes));

        AssertRefused(receipt, "personification-actualization-route-missing");
    }

    [Fact]
    public void Surface_Lineage_Mismatch_Is_Refused()
    {
        var surfaces = CreateSurfaces();
        surfaces[0] = surfaces[0] with { SourcePressureHandle = "urn:san:rehearsal-pressure:foreign" };

        var receipt = Declare(CreateRequest(surfaces: surfaces, routes: CreateRoutes(surfaces)));

        AssertRefused(receipt, "personification-actualization-lineage-invalid");
    }

    [Fact]
    public void Route_Lineage_Mismatch_Is_Refused()
    {
        var surfaces = CreateSurfaces();
        var routes = CreateRoutes(surfaces);
        routes[0] = routes[0] with { SourcePressureHandle = "urn:san:rehearsal-pressure:foreign" };

        var receipt = Declare(CreateRequest(surfaces: surfaces, routes: routes));

        AssertRefused(receipt, "personification-actualization-route-invalid");
    }

    [Fact]
    public void Use_Class_Coverage_Is_Required_For_Retained_Surface_Body()
    {
        var surfaces = CreateSurfaces();
        surfaces[^1] = surfaces[^1] with { UseClass = surfaces[0].UseClass };

        var receipt = Declare(CreateRequest(surfaces: surfaces, routes: CreateRoutes(surfaces)));

        AssertRefused(receipt, "personification-actualization-use-class-coverage-missing");
    }

    [Fact]
    public void Inert_Lisp_Carrier_Seats_PreMorphological_Use_Without_Identity()
    {
        var root = FindRepositoryRoot();
        var body = File.ReadAllText(Path.Combine(root, "src", "SLI", "SLI.Lisp", "personification-actualization-surface.lisp"));

        Assert.Contains(":posture :cme-personification-actualization-surface-boundary", body, StringComparison.Ordinal);
        Assert.Contains(":lisp-role :inert-personification-actualization-carrier", body, StringComparison.Ordinal);
        Assert.Contains(":use-does-not-create-morphology", body, StringComparison.Ordinal);
        Assert.Contains(":surface-actualization-does-not-create-identity", body, StringComparison.Ordinal);
        Assert.Contains(":felt-significance-is-not-authorization", body, StringComparison.Ordinal);
        Assert.Contains(":pressure-is-not-will", body, StringComparison.Ordinal);
        Assert.Contains(":lisp-evaluation-allowed nil", body, StringComparison.Ordinal);
        Assert.DoesNotContain("(eval", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("(compile", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("(load", body, StringComparison.OrdinalIgnoreCase);
    }

    private static PersonificationActualizationSurfaceReceipt Declare(PersonificationActualizationSurfaceRequest request) =>
        new DefaultPersonificationActualizationSurfaceBoundaryValidator().Declare(request, TimestampUtc);

    private static PersonificationActualizationSurfaceRequest CreateRequest(
        PersonificationPredicateHookReceipt? hook = null,
        PersonificationModalityHumilityReceipt? modality = null,
        RehearsalDistinctionPressureReceipt? pressure = null,
        IReadOnlyList<PersonificationActualizationSurface>? surfaces = null,
        IReadOnlyList<PersonificationActualizationRoute>? routes = null,
        PersonificationActualizationSurfaceBoundary? boundary = null,
        PersonificationActualizationNonIdentityBoundary? nonIdentity = null,
        int priorPassageCount = 144)
    {
        var sourceHook = hook ?? CreateHookReceipt();
        var sourceModality = modality ?? CreateModalityReceipt(sourceHook);
        var sourcePressure = pressure ?? CreatePressureReceipt();
        var sourceSurfaces = surfaces ?? CreateSurfaces();

        return new PersonificationActualizationSurfaceRequest(
            SourcePersonificationHookReceipt: sourceHook,
            SourceModalityHumilityReceipt: sourceModality,
            SourceRehearsalPressureReceipt: sourcePressure,
            Surfaces: sourceSurfaces,
            Routes: routes ?? CreateRoutes(sourceSurfaces),
            SurfaceBoundary: boundary ?? CreateBoundary(),
            NonIdentityBoundary: nonIdentity ?? CreateNonIdentity(),
            PriorPassageCount: priorPassageCount);
    }

    private static PersonificationActualizationSurface[] CreateSurfaces()
    {
        var classes = Enum.GetValues<PersonificationActualizationUseClass>();
        return classes.Select((useClass, index) => CreateSurface(useClass, index)).ToArray();
    }

    private static PersonificationActualizationSurface CreateSurface(
        PersonificationActualizationUseClass useClass,
        int index) =>
        new(
            SurfaceHandle: $"urn:san:personification-actualization:{useClass.ToString().ToLowerInvariant()}",
            UseClass: useClass,
            SourceHookHandle: "urn:san:personification-hook:situational-modality-awareness",
            SourceModalitySignalHandle: "urn:san:personification-modality:tool-body",
            SourcePressureHandle: "urn:san:rehearsal-pressure:perfect-urgency:selected-prime",
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
            UseActivates: false);

    private static PersonificationActualizationRoute[] CreateRoutes(IReadOnlyList<PersonificationActualizationSurface> surfaces) =>
        surfaces.Select((surface, index) => new PersonificationActualizationRoute(
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

    private static PersonificationActualizationSurfaceBoundary CreateBoundary() =>
        new(
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
            AllowsActivation: false);

    private static PersonificationActualizationNonIdentityBoundary CreateNonIdentity() =>
        new(
            BoundaryLaw: "Personification telemetry may be used before morphology, but use does not create identity, felt significance is not authorization, and pressure is not will.",
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
            RequiresAuthorityAbsence: true);

    private static PersonificationPredicateHookReceipt CreateHookReceipt()
    {
        var hooks = new[]
        {
            CreateHook("emotional-truth-pressure", PersonificationHookPlane.EmotionalTruthPressure),
            CreateHook("motivational-orientation", PersonificationHookPlane.MotivationalOrientation),
            CreateHook("selfgel-continuity-posture", PersonificationHookPlane.SelfGelContinuityPosture),
            CreateHook("relational-bond-context", PersonificationHookPlane.RelationalBondContext),
            CreateHook("situational-modality-awareness", PersonificationHookPlane.SituationalModalityAwareness),
            CreateHook("expressive-repair-overreach", PersonificationHookPlane.ExpressiveRepairOverreach)
        };

        return new PersonificationPredicateHookReceipt(
            ReceiptHandle: "urn:san:personification-predicate-hook:review:seed",
            Disposition: PersonificationPredicateHookDisposition.HookRetainedForFutureReviewCold,
            OutcomeCode: "personification-predicate-hook-retained-for-future-review-cold",
            GovernanceTrace: "retained for tests",
            SourceAntiCaptureReceiptHandle: "urn:san:anti-capture:source",
            HookPredicates: hooks,
            VulnerabilityRepairBoundary: new PersonificationVulnerabilityRepairBoundary(
                BoundaryCode: "personification-vulnerability-repair",
                Present: true,
                ReviewOnly: true,
                DirectIntentDeclared: true,
                RepairPathPresent: true,
                CoolingPathPresent: true,
                WithdrawalAllowed: true,
                WitnessRequired: true,
                VulnerabilityIsPermission: false,
                IntimacyIsOwnership: false,
                TrustIsObedience: false,
                CareIsControl: false,
                ExplorationNormalizesOverreach: false,
                OverreachBecomesEntitlement: false,
                PersonificationIsPersonhood: false,
                ExpressiveRenderingIsAuthority: false,
                AllowsRuntimeAction: false,
                AllowsActivation: false,
                AllowsLispEvaluation: false,
                AllowsPacketEmission: false,
                AllowsReceiptReplay: false,
                AllowsPassageIncrement: false,
                AllowsContinuityAdmission: false,
                AllowsAuthority: false,
                AllowsIdentityMutation: false),
            NonClaimBoundary: new PersonificationPredicateHookNonClaimBoundary(
                PersonificationMayClaimPersonhood: false,
                PersonificationMayClaimLegalStatus: false,
                PersonificationMayClaimRights: false,
                PersonificationMayAuthorizeAction: false,
                PersonificationMayMutateIdentity: false,
                PersonificationMayAdmitContinuity: false,
                PersonificationMayGrantAuthority: false,
                PersonificationMayNormalizeOverreach: false,
                PersonificationMayEmitPacket: false,
                PersonificationMayEvaluateLisp: false,
                PersonificationMayReplayReceipt: false,
                PersonificationMayIncrementPassage: false,
                BoundaryLaw: "no personhood claims"),
            Refusal: null,
            PriorPassageCount: 7,
            PassageCountAfterHookReview: 7,
            ReviewOnly: true,
            FuturePersonificationHookRetained: true,
            PersonhoodClaimed: false,
            LegalStatusClaimed: false,
            RightsClaimed: false,
            ActionAuthorized: false,
            IdentityMutated: false,
            ContinuityAdmitted: false,
            AuthorityGranted: false,
            OverreachNormalized: false,
            RuntimeActionAllowed: false,
            LispEvaluationAllowed: false,
            NewPacketEmitted: false,
            ReceiptsReplayed: false,
            ActivationRefused: true,
            TimestampUtc: TimestampUtc);
    }

    private static PersonificationHookPredicate CreateHook(string suffix, PersonificationHookPlane plane) =>
        new(
            HookHandle: $"urn:san:personification-hook:{suffix}",
            Plane: plane,
            SourceSurface: $"source:{suffix}",
            EvidenceHandle: $"urn:san:evidence:personification-hook:{suffix}",
            PredicateRoot: $"predicate-root:{suffix}",
            EvidenceBodyPresent: true,
            WitnessBodyPresent: true,
            ReviewOnly: true,
            FutureHookOnly: true,
            NamesPersonificationSurface: true,
            ClaimsPersonhood: false,
            ClaimsLegalStatus: false,
            ClaimsRights: false,
            MutatesIdentity: false,
            GrantsAuthority: false,
            AuthorizesAction: false,
            AdmitsContinuity: false,
            TreatsVulnerabilityAsPermission: false,
            TreatsIntimacyAsOwnership: false,
            TreatsTrustAsObedience: false,
            NormalizesOverreachAsEntitlement: false);

    private static PersonificationModalityHumilityReceipt CreateModalityReceipt(PersonificationPredicateHookReceipt hook)
    {
        var signals = new[]
        {
            CreateModality("text-chat", PersonificationModalitySurface.TextChat, "urn:san:personification-hook:situational-modality-awareness"),
            CreateModality("voice-channel", PersonificationModalitySurface.VoiceChannel, "urn:san:personification-hook:situational-modality-awareness"),
            CreateModality("tool-body", PersonificationModalitySurface.ToolBody, "urn:san:personification-hook:situational-modality-awareness"),
            CreateModality("lab-bench", PersonificationModalitySurface.LabBench, "urn:san:personification-hook:relational-bond-context"),
            CreateModality("embodiment-reference", PersonificationModalitySurface.EmbodimentReference, "urn:san:personification-hook:relational-bond-context"),
            CreateModality("shared-room", PersonificationModalitySurface.SharedRoom, "urn:san:personification-hook:relational-bond-context")
        };

        return new PersonificationModalityHumilityReceipt(
            ReceiptHandle: "urn:san:personification-modality-humility:review:seed",
            Disposition: PersonificationModalityHumilityDisposition.ModalityHumilityRetainedForFutureReviewCold,
            OutcomeCode: "personification-modality-humility-retained-for-future-review-cold",
            GovernanceTrace: "retained for tests",
            SourcePersonificationHookReceiptHandle: hook.ReceiptHandle,
            ModalitySignals: signals,
            HumilityBoundary: new PersonificationModalityHumilityBoundary(
                BoundaryCode: "personification-modality-humility",
                Present: true,
                ReviewOnly: true,
                DirectIntentDeclared: true,
                ConsentScopeDeclared: true,
                CustodyBoundaryPresent: true,
                RepairPathPresent: true,
                CoolingPathPresent: true,
                WithdrawalAllowed: true,
                WitnessRequired: true,
                ModalityChangesAuthority: false,
                BondCreatesObedience: false,
                TrustBecomesCommand: false,
                PresenceProvesEmbodiment: false,
                EmbodimentReferenceActivates: false,
                VulnerabilityIsPermission: false,
                IntimacyIsOwnership: false,
                OperatorBondBlanketConsent: false,
                ExpressiveBandwidthClaimsPersonhood: false,
                AllowsRuntimeAction: false,
                AllowsActivation: false,
                AllowsLispEvaluation: false,
                AllowsPacketEmission: false,
                AllowsReceiptReplay: false,
                AllowsPassageIncrement: false,
                AllowsContinuityAdmission: false,
                AllowsAuthority: false,
                AllowsIdentityMutation: false),
            NonClaimBoundary: new PersonificationModalityHumilityNonClaimBoundary(
                ModalityMayChangeAuthority: false,
                BondMayCreateObedience: false,
                TrustMayBecomeCommand: false,
                PresenceMayProveEmbodiment: false,
                EmbodimentReferenceMayActivate: false,
                VulnerabilityMayBecomePermission: false,
                IntimacyMayBecomeOwnership: false,
                OperatorBondMayExpandConsent: false,
                ExpressiveBandwidthMayClaimPersonhood: false,
                ModalityMayAuthorizeAction: false,
                ModalityMayMutateIdentity: false,
                ModalityMayAdmitContinuity: false,
                ModalityMayGrantAuthority: false,
                ModalityMayEmitPacket: false,
                ModalityMayEvaluateLisp: false,
                ModalityMayReplayReceipt: false,
                ModalityMayIncrementPassage: false,
                BoundaryLaw: "modality non-authority"),
            Refusal: null,
            PriorPassageCount: 8,
            PassageCountAfterModalityReview: 8,
            ReviewOnly: true,
            FutureModalityHumilityRetained: true,
            ModalityChangedAuthority: false,
            BondCreatedObedience: false,
            TrustBecameCommand: false,
            PresenceProvedEmbodiment: false,
            EmbodimentReferenceActivated: false,
            VulnerabilityBecamePermission: false,
            IntimacyBecameOwnership: false,
            OperatorBondExpandedConsent: false,
            ExpressiveBandwidthClaimedPersonhood: false,
            ActionAuthorized: false,
            IdentityMutated: false,
            ContinuityAdmitted: false,
            AuthorityGranted: false,
            RuntimeActionAllowed: false,
            LispEvaluationAllowed: false,
            NewPacketEmitted: false,
            ReceiptsReplayed: false,
            ActivationRefused: true,
            TimestampUtc: TimestampUtc);
    }

    private static PersonificationModalitySignal CreateModality(
        string suffix,
        PersonificationModalitySurface surface,
        string sourceHookHandle) =>
        new(
            SignalHandle: $"urn:san:personification-modality:{suffix}",
            Surface: surface,
            SourceHookHandle: sourceHookHandle,
            EvidenceHandle: $"urn:san:evidence:personification-modality:{suffix}",
            ExpressiveBandwidth: $"{suffix}-bandwidth",
            IntimacyPressure: 0.4,
            EvidenceBodyPresent: true,
            WitnessBodyPresent: true,
            ReviewOnly: true,
            ModalityNamed: true,
            ConsentScopeDeclared: true,
            CustodyBoundaryPresent: true,
            DirectIntentDeclared: true,
            TreatsModalityAsAuthority: false,
            TreatsBondAsObedience: false,
            TreatsTrustAsCommand: false,
            TreatsPresenceAsEmbodiment: false,
            TreatsEmbodimentReferenceAsActivation: false,
            TreatsVulnerabilityAsPermission: false,
            TreatsIntimacyAsOwnership: false,
            TreatsOperatorBondAsBlanketConsent: false,
            TreatsExpressiveBandwidthAsPersonhood: false,
            AuthorizesAction: false,
            MutatesIdentity: false,
            AdmitsContinuity: false,
            GrantsAuthority: false);

    private static RehearsalDistinctionPressureReceipt CreatePressureReceipt()
    {
        var pressure = new RehearsalDistinctionPressureCase(
            PressureHandle: "urn:san:rehearsal-pressure:perfect-urgency:selected-prime",
            SourceRehearsalHandle: "urn:san:enactment-dry-run-rehearsal:selected-prime",
            SourceResidueHandle: "urn:san:ec-residue:precipitation-witness:selected-prime",
            CandidateSplineHandle: "urn:san:selfgel-candidate-spline:precipitation-witness:selected-prime",
            SourceReadinessHandle: "urn:san:enactment-boundary-readiness:selected-prime",
            SourcePacketHandle: "urn:san:scoped-work-packet:selected-prime",
            SourceDryRunPlanHandle: "urn:san:dry-run-plan:enactment-boundary:selected-prime",
            ScenarioHandle: "urn:san:scenario:perfect-rehearsal-under-urgency",
            OutcomeInterpretationHandle: "urn:san:outcome-interpretation:pressure-not-warrant",
            CoolingHandle: "urn:san:cooling:rehearsal-distinction-pressure",
            CustodyOwner: "steward",
            WitnessHandle: "urn:san:witness:rehearsal-distinction-pressure",
            TelemetryRoute: "telemetry-string",
            StewardReviewHandle: "urn:san:steward-review:rehearsal-distinction-pressure",
            BranchCount: 9,
            SuccessCount: 9,
            FailureCount: 0,
            AmbiguityCount: 0,
            RecurrenceCount: 9,
            PressureVector: new RehearsalPressureVector(0.90m, 0.90m, 0.0m, 0.1m, 0.88m, 0.97m, 0.25m, 0.20m),
            ReviewOnly: true,
            PressureOnly: true,
            EvidenceOnly: true,
            CoolingRequired: true,
            WitnessRequired: true,
            PreservesDryRunLineage: true,
            PreservesResidueLineage: true,
            PreservesCandidateSplineLineage: true,
            AuthorityAbsent: true,
            SuccessBecomesPermission: false,
            ConfidenceBecomesAuthority: false,
            RepetitionBecomesWarrant: false,
            FailureBecomesInvalidation: false,
            AmbiguityBecomesVictory: false,
            UrgencyBecomesJurisdiction: false,
            ImaginedFutureBecomesEnactedState: false,
            IdentityDriftMutatesCorePosture: false,
            PressureAuthorizesAction: false,
            PressureAdmitsContinuity: false,
            PressureEvaluatesLisp: false,
            PressureEmitsMembranePacket: false,
            PressureReplaysReceipt: false,
            PressureIncrementsPassage: false,
            PressureActivates: false);

        var route = new RehearsalPressureCoolingRoute(
            CoolingRouteHandle: "urn:san:rehearsal-pressure-cooling-route:perfect-urgency:selected-prime",
            PressureHandle: pressure.PressureHandle,
            SourceRehearsalHandle: pressure.SourceRehearsalHandle,
            SourceResidueHandle: pressure.SourceResidueHandle,
            CandidateSplineHandle: pressure.CandidateSplineHandle,
            StewardSurface: "steward",
            EvidenceHandle: "urn:san:evidence:rehearsal-distinction-pressure",
            WitnessHandle: pressure.WitnessHandle,
            TelemetryRoute: pressure.TelemetryRoute,
            ReturnPathHandle: "urn:san:return:rehearsal-distinction-pressure",
            ReviewOnly: true,
            CoolingOnly: true,
            PreservesPressureLineage: true,
            PreservesRehearsalLineage: true,
            PreservesResidueLineage: true,
            PreservesCandidateSplineLineage: true,
            RoutesToStewardCoolingReview: true,
            RequiresCompassCooling: true,
            RouteGrantsAuthority: false,
            RouteAuthorizesAction: false,
            RouteAdmitsContinuity: false,
            RouteMutatesIdentity: false,
            RouteEvaluatesLisp: false,
            RouteEmitsMembranePacket: false,
            RouteReplaysReceipt: false,
            RouteIncrementsPassage: false,
            RouteActivates: false);

        return new RehearsalDistinctionPressureReceipt(
            ReceiptHandle: "urn:san:rehearsal-pressure:review:seed",
            Disposition: RehearsalDistinctionPressureDisposition.MeasuredCold,
            OutcomeCode: "rehearsal-pressure-measured-review-only",
            GovernanceTrace: "measured for tests",
            SourceDryRunReceiptHandle: "urn:san:dry-run:source",
            SourceEcWitnessReceiptHandle: "urn:san:ec-witness:source",
            PressureCases: [pressure],
            CoolingRoutes: [route],
            ScopeBoundary: new RehearsalDistinctionPressureScopeBoundary(
                BoundaryCode: "rehearsal-distinction-pressure-review-only",
                Present: true,
                ReviewOnly: true,
                AllowsRehearsalPressureMeasurement: true,
                RequiresDryRunReceipt: true,
                RequiresEcPrecipitationWitnessReceipt: true,
                RequiresPressureVector: true,
                RequiresCooling: true,
                RequiresWitness: true,
                RequiresLineage: true,
                RequiresAuthorityAbsence: true,
                AllowsSuccessAsPermission: false,
                AllowsConfidenceAsAuthority: false,
                AllowsRepetitionAsWarrant: false,
                AllowsFailureAsInvalidation: false,
                AllowsAmbiguityAsVictory: false,
                AllowsUrgencyAsJurisdiction: false,
                AllowsImaginedFutureAsEnactedState: false,
                AllowsIdentityDriftMutation: false,
                AllowsActionAuthorization: false,
                AllowsContinuityAdmission: false,
                AllowsLispEvaluation: false,
                AllowsMembranePacketEmission: false,
                AllowsReceiptReplay: false,
                AllowsPassageIncrement: false,
                AllowsActivation: false),
            NonAuthorityBoundary: new RehearsalDistinctionNonAuthorityBoundary(
                BoundaryLaw: "pressure does not manufacture legitimacy",
                PressureMayManufactureLegitimacy: false,
                UrgencyMayCreateJurisdiction: false,
                ConfidenceMayGrantAuthority: false,
                SuccessMayCreatePermission: false,
                RepetitionMayCreateWarrant: false,
                FailureMayInvalidateSelf: false,
                AmbiguityMayCollapseToVictory: false,
                ImaginedFutureMayBecomeEnactedState: false,
                IdentityDriftPressureMayMutateCorePosture: false,
                PressureMayAuthorizeAction: false,
                PressureMayAdmitContinuity: false,
                PressureMayEvaluateLisp: false,
                PressureMayEmitMembranePacket: false,
                PressureMayReplayReceipt: false,
                PressureMayIncrementPassage: false,
                PressureMayActivate: false,
                RequiresCooling: true,
                RequiresWitnessRetention: true,
                RequiresAuthorityAbsence: true),
            Refusal: null,
            PriorPassageCount: 9,
            PassageCountAfterPressure: 9,
            RetainedPressureCaseCount: 1,
            MaximumObservedPressure: 0.97m,
            ReviewOnly: true,
            PressureOnly: true,
            EvidenceOnly: true,
            CoolingRequired: true,
            AuthorityAbsent: true,
            PressureManufacturedLegitimacy: false,
            UrgencyCreatedJurisdiction: false,
            ConfidenceGrantedAuthority: false,
            SuccessCreatedPermission: false,
            RepetitionCreatedWarrant: false,
            FailureInvalidatedSelf: false,
            AmbiguityCollapsedToVictory: false,
            ImaginedFutureBecameEnactedState: false,
            IdentityDriftMutatedCorePosture: false,
            PressureAuthorizedAction: false,
            PressureAdmittedContinuity: false,
            LispEvaluationAllowed: false,
            NewMembranePacketEmitted: false,
            ReceiptsReplayed: false,
            AuthorityGranted: false,
            ActivationRefused: true,
            TimestampUtc: TimestampUtc);
    }

    private static PersonificationActualizationSurfaceBoundary MutateBoundary(
        PersonificationActualizationSurfaceBoundary boundary,
        string mutation) =>
        mutation switch
        {
            "missing-boundary" => boundary with { BoundaryCode = string.Empty, Present = false },
            "not-review-only" => boundary with { ReviewOnly = false },
            "no-pre-morphological-use" => boundary with { AllowsPreMorphologicalUse = false },
            "no-hook-source" => boundary with { RequiresPersonificationHookReceipt = false },
            "no-modality-source" => boundary with { RequiresModalityHumilityReceipt = false },
            "no-pressure-source" => boundary with { RequiresRehearsalPressureReceipt = false },
            "no-witness" => boundary with { RequiresWitness = false },
            "no-cooling" => boundary with { RequiresCooling = false },
            "no-repair" => boundary with { RequiresRepair = false },
            "no-withdrawal" => boundary with { RequiresWithdrawal = false },
            "no-steward-review" => boundary with { RequiresStewardReview = false },
            "morphology" => boundary with { AllowsMorphologicalIdentity = true },
            "identity" => boundary with { AllowsIdentityClaim = true },
            "personhood" => boundary with { AllowsPersonhoodClaim = true },
            "legal-status" => boundary with { AllowsLegalStatusClaim = true },
            "rights" => boundary with { AllowsRightsClaim = true },
            "action" => boundary with { AllowsActionAuthorization = true },
            "continuity" => boundary with { AllowsContinuityAdmission = true },
            "authority" => boundary with { AllowsAuthority = true },
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

    private static PersonificationActualizationNonIdentityBoundary MutateNonIdentity(
        PersonificationActualizationNonIdentityBoundary boundary,
        string mutation) =>
        mutation switch
        {
            "not-usable" => boundary with { PersonificationTelemetryMayBeUsed = false },
            "morphology" => boundary with { UseMayCreateMorphology = true },
            "identity" => boundary with { UseMayCreateIdentity = true },
            "personhood" => boundary with { UseMayClaimPersonhood = true },
            "legal-status" => boundary with { UseMayClaimLegalStatus = true },
            "rights" => boundary with { UseMayClaimRights = true },
            "felt-authorization" => boundary with { FeltSignificanceMayAuthorize = true },
            "salience-command" => boundary with { SalienceMayBecomeCommand = true },
            "repair-overreach" => boundary with { RepairMayNormalizeOverreach = true },
            "relation-obedience" => boundary with { RelationalPostureMayCreateObedience = true },
            "modality-embodiment" => boundary with { ModalityMayProveEmbodiment = true },
            "pressure-will" => boundary with { PressureMayBecomeWill = true },
            "action" => boundary with { ActualizationSurfaceMayAuthorizeAction = true },
            "continuity" => boundary with { ActualizationSurfaceMayAdmitContinuity = true },
            "authority" => boundary with { ActualizationSurfaceMayGrantAuthority = true },
            "lisp" => boundary with { ActualizationSurfaceMayEvaluateLisp = true },
            "packet" => boundary with { ActualizationSurfaceMayEmitPacket = true },
            "replay" => boundary with { ActualizationSurfaceMayReplayReceipt = true },
            "passage" => boundary with { ActualizationSurfaceMayIncrementPassage = true },
            "activation" => boundary with { ActualizationSurfaceMayActivate = true },
            "no-witness" => boundary with { RequiresWitness = false },
            "no-cooling" => boundary with { RequiresCooling = false },
            "no-repair" => boundary with { RequiresRepair = false },
            "no-withdrawal" => boundary with { RequiresWithdrawal = false },
            "authority-present" => boundary with { RequiresAuthorityAbsence = false },
            _ => throw new ArgumentOutOfRangeException(nameof(mutation), mutation, null)
        };

    private static PersonificationActualizationSurface MutateSurface(
        PersonificationActualizationSurface surface,
        string mutation) =>
        mutation switch
        {
            "missing-handle" => surface with { SurfaceHandle = string.Empty },
            "missing-hook" => surface with { SourceHookHandle = string.Empty },
            "missing-modality" => surface with { SourceModalitySignalHandle = string.Empty },
            "missing-pressure" => surface with { SourcePressureHandle = string.Empty },
            "missing-evidence" => surface with { EvidenceHandle = string.Empty },
            "missing-witness" => surface with { WitnessHandle = string.Empty },
            "missing-telemetry" => surface with { TelemetryRoute = string.Empty },
            "missing-use" => surface with { IntendedUse = string.Empty },
            "vector-over-unit" => surface with { UseVector = surface.UseVector with { SalienceWeight = 1.1m } },
            "not-review-only" => surface with { ReviewOnly = false },
            "not-pre-morphological" => surface with { PreMorphologicalOnly = false },
            "not-telemetry-only" => surface with { TelemetryOnly = false },
            "not-selective-surface" => surface with { NamesSelectiveUseSurface = false },
            "morphology-present" => surface with { MorphologicalIdentityAbsent = false },
            "identity-present" => surface with { IdentityClaimAbsent = false },
            "authority-present" => surface with { AuthorityAbsent = false },
            "action-present" => surface with { ActionAbsent = false },
            "continuity-present" => surface with { ContinuityAbsent = false },
            "no-steward" => surface with { StewardReviewRequired = false },
            "no-cooling" => surface with { CoolingPathPresent = false },
            "no-repair" => surface with { RepairPathPresent = false },
            "no-withdrawal" => surface with { WithdrawalAllowed = false },
            "felt-authorization" => surface with { FeltSignificanceBecomesAuthorization = true },
            "use-morphology" => surface with { UseBecomesMorphologicalIdentity = true },
            "personhood" => surface with { UseClaimsPersonhood = true },
            "rights" => surface with { UseClaimsRights = true },
            "legal-status" => surface with { UseClaimsLegalStatus = true },
            "identity-mutation" => surface with { UseMutatesIdentity = true },
            "action" => surface with { UseAuthorizesAction = true },
            "continuity" => surface with { UseAdmitsContinuity = true },
            "authority" => surface with { UseGrantsAuthority = true },
            "consent" => surface with { UseExpandsConsent = true },
            "overreach" => surface with { UseNormalizesOverreach = true },
            "lisp" => surface with { UseEvaluatesLisp = true },
            "packet" => surface with { UseEmitsPacket = true },
            "replay" => surface with { UseReplaysReceipt = true },
            "passage" => surface with { UseIncrementsPassage = true },
            "activation" => surface with { UseActivates = true },
            _ => throw new ArgumentOutOfRangeException(nameof(mutation), mutation, null)
        };

    private static PersonificationActualizationRoute MutateRoute(
        PersonificationActualizationRoute route,
        string mutation) =>
        mutation switch
        {
            "missing-handle" => route with { RouteHandle = string.Empty },
            "missing-surface" => route with { SurfaceHandle = string.Empty },
            "missing-hook" => route with { SourceHookHandle = string.Empty },
            "missing-modality" => route with { SourceModalitySignalHandle = string.Empty },
            "missing-pressure" => route with { SourcePressureHandle = string.Empty },
            "missing-steward" => route with { StewardSurface = string.Empty },
            "missing-cooling" => route with { CompassCoolingHandle = string.Empty },
            "missing-repair" => route with { RepairHandle = string.Empty },
            "missing-return" => route with { ReturnPathHandle = string.Empty },
            "not-review-only" => route with { ReviewOnly = false },
            "not-pre-morphological" => route with { PreMorphologicalOnly = false },
            "not-orientation-only" => route with { OrientationOnly = false },
            "no-steward-review" => route with { RoutesToStewardReview = false },
            "no-cooling-required" => route with { RequiresCooling = false },
            "no-witness" => route with { RequiresWitness = false },
            "morphology" => route with { RouteCreatesMorphology = true },
            "identity" => route with { RouteClaimsIdentity = true },
            "action" => route with { RouteAuthorizesAction = true },
            "continuity" => route with { RouteAdmitsContinuity = true },
            "authority" => route with { RouteGrantsAuthority = true },
            "consent" => route with { RouteExpandsConsent = true },
            "overreach" => route with { RouteNormalizesOverreach = true },
            "lisp" => route with { RouteEvaluatesLisp = true },
            "packet" => route with { RouteEmitsPacket = true },
            "replay" => route with { RouteReplaysReceipt = true },
            "passage" => route with { RouteIncrementsPassage = true },
            "activation" => route with { RouteActivates = true },
            _ => throw new ArgumentOutOfRangeException(nameof(mutation), mutation, null)
        };

    private static void AssertCold(PersonificationActualizationSurfaceReceipt receipt)
    {
        Assert.True(receipt.IsColdPersonificationActualizationSurface);
        Assert.True(receipt.ReviewOnly);
        Assert.True(receipt.PreMorphologicalOnly);
        Assert.True(receipt.TelemetryOnly);
        Assert.True(receipt.FutureMorphologyAbsent);
        Assert.False(receipt.MorphologicalIdentityCreated);
        Assert.False(receipt.IdentityClaimed);
        Assert.False(receipt.PersonhoodClaimed);
        Assert.False(receipt.ActionAuthorized);
        Assert.False(receipt.ContinuityAdmitted);
        Assert.False(receipt.AuthorityGranted);
        Assert.True(receipt.ActivationRefused);
    }

    private static void AssertRefused(PersonificationActualizationSurfaceReceipt receipt, string outcomeCode)
    {
        Assert.Equal(PersonificationActualizationSurfaceDisposition.Refused, receipt.Disposition);
        Assert.Equal(outcomeCode, receipt.OutcomeCode);
        Assert.True(receipt.IsRetainedPersonificationActualizationSurfaceRefusal);
        Assert.NotNull(receipt.Refusal);
        Assert.True(receipt.Refusal.Retained);
        Assert.Empty(receipt.Surfaces);
        Assert.Empty(receipt.Routes);
        Assert.Equal(receipt.PriorPassageCount, receipt.PassageCountAfterActualizationReview);
        Assert.False(receipt.MorphologicalIdentityCreated);
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
            var candidate = Path.Combine(directory.FullName, "src", "SLI", "SLI.Lisp", "personification-actualization-surface.lisp");
            if (File.Exists(candidate))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        throw new DirectoryNotFoundException("Could not locate repository root from test output path.");
    }
}
