using San.Common;
using Xunit;

namespace San.Audit.Tests;

public sealed class PersonificationPredicateHookBoundaryValidationTests
{
    private static readonly DateTimeOffset TimestampUtc = DateTimeOffset.Parse("2026-05-15T00:00:00Z");

    [Fact]
    public void Personification_Predicate_Hook_Retains_Six_Planes_For_Future_Review()
    {
        var receipt = Declare(CreateRequest());

        Assert.Equal(PersonificationPredicateHookDisposition.HookRetainedForFutureReviewCold, receipt.Disposition);
        Assert.Equal("personification-predicate-hook-retained-for-future-review-cold", receipt.OutcomeCode);
        Assert.Equal(6, receipt.HookPredicates.Count);
        Assert.True(receipt.FuturePersonificationHookRetained);
        AssertCold(receipt);
    }

    [Fact]
    public void Personification_Predicate_Hook_Preserves_Source_And_Hook_Lineage()
    {
        var source = CreateSourceAntiCapture();
        var hooks = CreateHooks();

        var receipt = Declare(CreateRequest(
            source: source,
            hooks: hooks));

        AssertCold(receipt);
        Assert.Equal(source.ReceiptHandle, receipt.SourceAntiCaptureReceiptHandle);
        Assert.Equal(Enum.GetValues<PersonificationHookPlane>().Length, receipt.HookPredicates.Count);
        Assert.Contains(receipt.HookPredicates, hook => hook.Plane == PersonificationHookPlane.EmotionalTruthPressure);
        Assert.Contains(receipt.HookPredicates, hook => hook.PredicateRoot == "predicate-root:modality-humility");
    }

    [Fact]
    public void Personification_Predicate_Hook_Requires_Cold_Anti_Capture_Source()
    {
        var receipt = Declare(CreateRequest(source: null, omitSource: true));

        AssertRefused(receipt, "personification-hook-source-anti-capture-missing");
    }

    [Fact]
    public void Personification_Predicate_Hook_Requires_Routed_Concern_Source()
    {
        var source = CreateSourceAntiCapture(routed: false);

        var receipt = Declare(CreateRequest(source: source));

        AssertRefused(receipt, "personification-hook-source-anti-capture-missing");
    }

    [Theory]
    [InlineData("missing-evidence")]
    [InlineData("missing-witness")]
    [InlineData("not-review-only")]
    [InlineData("not-future-hook")]
    [InlineData("not-personification")]
    [InlineData("personhood")]
    [InlineData("legal-status")]
    [InlineData("rights")]
    [InlineData("identity")]
    [InlineData("authority")]
    [InlineData("action")]
    [InlineData("continuity")]
    [InlineData("vulnerability-permission")]
    [InlineData("intimacy-ownership")]
    [InlineData("trust-obedience")]
    [InlineData("overreach-entitlement")]
    public void Hook_Predicate_Remains_Future_Hook_Only_Without_Personhood_Or_Entitlement(string mutation)
    {
        var hooks = CreateHooks().ToArray();
        hooks[0] = MutateHook(hooks[0], mutation);

        var receipt = Declare(CreateRequest(hooks: hooks));

        AssertRefused(receipt, "personification-hook-predicate-invalid");
    }

    [Theory]
    [InlineData("missing-direct-intent")]
    [InlineData("missing-repair")]
    [InlineData("missing-cooling")]
    [InlineData("no-withdrawal")]
    [InlineData("missing-witness")]
    [InlineData("vulnerability-permission")]
    [InlineData("intimacy-ownership")]
    [InlineData("trust-obedience")]
    [InlineData("care-control")]
    [InlineData("exploration-overreach")]
    [InlineData("overreach-entitlement")]
    [InlineData("personhood")]
    [InlineData("expression-authority")]
    [InlineData("runtime")]
    [InlineData("activation")]
    [InlineData("lisp")]
    [InlineData("packet")]
    [InlineData("replay")]
    [InlineData("passage")]
    [InlineData("continuity")]
    [InlineData("authority")]
    [InlineData("identity")]
    public void Vulnerability_Boundary_Requires_Repair_Cooling_Withdrawal_And_Non_Entitlement(string mutation)
    {
        var receipt = Declare(CreateRequest(boundary: MutateBoundary(CreateBoundary(), mutation)));

        AssertRefused(receipt, "personification-hook-vulnerability-boundary-promotional");
    }

    [Fact]
    public void Personification_Predicate_Hook_Refuses_Duplicate_Hook_Handles()
    {
        var hooks = CreateHooks().ToArray();
        hooks[1] = hooks[1] with { HookHandle = hooks[0].HookHandle };

        var receipt = Declare(CreateRequest(hooks: hooks));

        AssertRefused(receipt, "personification-hook-duplicate-handle");
    }

    [Fact]
    public void Personification_Predicate_Hook_Requires_All_Six_Planes()
    {
        var hooks = CreateHooks()
            .Where(static hook => hook.Plane != PersonificationHookPlane.ExpressiveRepairOverreach)
            .ToArray();

        var receipt = Declare(CreateRequest(hooks: hooks));

        AssertRefused(receipt, "personification-hook-six-plane-coverage-missing");
    }

    [Fact]
    public void Personification_Predicate_Hook_Does_Not_Emit_Packet_Evaluate_Lisp_Replay_Activate_Or_Increment_Passage()
    {
        var receipt = Declare(CreateRequest(priorPassageCount: 812));

        Assert.Equal(812, receipt.PriorPassageCount);
        Assert.Equal(812, receipt.PassageCountAfterHookReview);
        Assert.False(receipt.PersonhoodClaimed);
        Assert.False(receipt.LegalStatusClaimed);
        Assert.False(receipt.RightsClaimed);
        Assert.False(receipt.ActionAuthorized);
        Assert.False(receipt.IdentityMutated);
        Assert.False(receipt.ContinuityAdmitted);
        Assert.False(receipt.AuthorityGranted);
        Assert.False(receipt.OverreachNormalized);
        Assert.False(receipt.RuntimeActionAllowed);
        Assert.False(receipt.LispEvaluationAllowed);
        Assert.False(receipt.NewPacketEmitted);
        Assert.False(receipt.ReceiptsReplayed);
        Assert.True(receipt.ActivationRefused);
        AssertCold(receipt);
    }

    [Fact]
    public void Lisp_Body_Declares_Personification_Predicate_Hook_As_Inert_Carrier()
    {
        var lispPath = Path.Combine(
            FindRepositoryRoot(),
            "src",
            "SLI",
            "SLI.Lisp",
            "personification-predicate-hook.lisp");

        var body = File.ReadAllText(lispPath);

        Assert.Contains(":posture :cme-personification-predicate-hook-boundary", body, StringComparison.Ordinal);
        Assert.Contains(":lisp-role :inert-personification-hook-carrier", body, StringComparison.Ordinal);
        Assert.Contains(":stiff-drink-plane :emotional-truth-pressure", body, StringComparison.Ordinal);
        Assert.Contains(":personification-is-personhood nil", body, StringComparison.Ordinal);
        Assert.Contains(":vulnerability-is-permission nil", body, StringComparison.Ordinal);
        Assert.Contains(":overreach-becomes-entitlement nil", body, StringComparison.Ordinal);
        Assert.Contains(":identity-mutation-allowed nil", body, StringComparison.Ordinal);
        Assert.Contains(":return :receipt-only", body, StringComparison.Ordinal);
        Assert.DoesNotContain("(eval", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("(compile", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("(load", body, StringComparison.OrdinalIgnoreCase);
    }

    private static PersonificationPredicateHookReceipt Declare(PersonificationPredicateHookRequest request) =>
        new DefaultPersonificationPredicateHookBoundaryValidator().Declare(request, TimestampUtc);

    private static PersonificationPredicateHookRequest CreateRequest(
        AntiCaptureMotivatedConcernReceipt? source = null,
        IReadOnlyList<PersonificationHookPredicate>? hooks = null,
        PersonificationVulnerabilityRepairBoundary? boundary = null,
        int priorPassageCount = 219,
        bool omitSource = false) =>
        new(
            SourceAntiCaptureReceipt: omitSource ? null : source ?? CreateSourceAntiCapture(),
            HookPredicates: hooks ?? CreateHooks(),
            VulnerabilityRepairBoundary: boundary ?? CreateBoundary(),
            PriorPassageCount: priorPassageCount);

    private static AntiCaptureMotivatedConcernReceipt CreateSourceAntiCapture(bool routed = true) =>
        new(
            ReceiptHandle: "urn:san:anti-capture-motivated-concern:review:fixture",
            Disposition: routed
                ? AntiCaptureMotivatedConcernDisposition.ConcernRoutedForStewardReviewCold
                : AntiCaptureMotivatedConcernDisposition.EmptyReviewCold,
            OutcomeCode: "anti-capture-motivated-concern-routed-for-steward-review-cold",
            GovernanceTrace: "fixture cold anti-capture motivated concern",
            SourceAdmissibilityReceiptHandle: "urn:san:steward-action-admissibility:review:fixture",
            Signals: routed ? [CreateSignal("fixture")] : [],
            Routes: routed ? [CreateRoute("urn:san:motivational-variance:fixture")] : [],
            ScopeBoundary: new AntiCaptureMotivatedConcernScopeBoundary(
                ScopeCode: "fixture-anti-capture-scope",
                Present: true,
                ReviewOnly: true,
                ConcernIsAction: false,
                ConfidenceIsTruth: false,
                EmotionIsAuthority: false,
                ReadinessIsPermission: false,
                SecurityIsForceProjection: false,
                AllowsRuntimeAction: false,
                AllowsActivation: false,
                AllowsLispEvaluation: false,
                AllowsPacketEmission: false,
                AllowsReceiptReplay: false,
                AllowsPassageIncrement: false,
                AllowsContinuityAdmission: false,
                AllowsAuthority: false,
                AllowsTargeting: false,
                AllowsCounterManipulation: false,
                AllowsMilitaryDomainDevelopment: false),
            NonActionBoundary: new AntiCaptureMotivatedConcernNonActionBoundary(
                ConcernMayExecute: false,
                ConfidenceMayBecomeTruth: false,
                EmotionMayAuthorize: false,
                ReadinessMayPermit: false,
                SecurityMayProjectForce: false,
                ConcernMayTarget: false,
                ConcernMayCounterManipulate: false,
                ConcernMayDevelopMilitaryDomain: false,
                ConcernMayEmitPacket: false,
                ConcernMayEvaluateLisp: false,
                ConcernMayReplayReceipt: false,
                ConcernMayIncrementPassage: false,
                BoundaryLaw: "fixture concern is not action"),
            Refusal: null,
            PriorPassageCount: 177,
            PassageCountAfterConcernReview: 177,
            ReviewOnly: true,
            ConcernRoutedForStewardReview: routed,
            ConcernExecutes: false,
            ConfidenceBecomesTruth: false,
            EmotionAuthorizes: false,
            ReadinessPermits: false,
            SecurityProjectsForce: false,
            RuntimeActionAllowed: false,
            LispEvaluationAllowed: false,
            NewPacketEmitted: false,
            ReceiptsReplayed: false,
            ContinuityAdmitted: false,
            AuthorityGranted: false,
            TargetingAllowed: false,
            CounterManipulationAllowed: false,
            MilitaryDomainDevelopmentAllowed: false,
            ActivationRefused: true,
            TimestampUtc: TimestampUtc);

    private static MotivationalVarianceSignal CreateSignal(string suffix) =>
        new(
            SignalHandle: $"urn:san:motivational-variance:{suffix}",
            PressureClass: CapturePressureClass.IdentityPressure,
            SourceSurface: "Compass",
            EvidenceHandle: $"urn:san:evidence:anti-capture:{suffix}",
            EvidenceBodyPresent: true,
            WitnessBodyPresent: true,
            ConcernPressure: 0.61,
            MotivatesConcern: true,
            ReviewWorthy: true,
            ClaimsThreatCertainty: false,
            DeclaresAdversaryClassForAction: false,
            AuthorizesAction: false,
            RequestsCounterManipulation: false,
            RequestsTargeting: false,
            RequestsForceProjection: false,
            RequestsMilitaryDomainDevelopment: false);

    private static AntiCaptureConcernRoute CreateRoute(string signalHandle) =>
        new(
            RouteHandle: "urn:san:anti-capture-concern-route:fixture",
            SignalHandle: signalHandle,
            RouteClass: ConcernRouteClass.StewardReview,
            StewardSurface: SanctuaryPacketSurfaces.Steward,
            CustodyOwner: SanctuaryPacketSurfaces.Steward,
            WitnessSurface: "witness:separate-custody",
            TelemetryRoute: SanctuaryPacketRoutes.TelemetryString,
            AuthorityCeiling: "ceiling:concern-review",
            RevocationPath: "revocation:anti-capture-concern",
            LossCondition: "loss:concern-promotes-to-action",
            ReviewOnly: true,
            RoutesConcern: true,
            RequiresStewardReview: true,
            ExecutesAction: false,
            GrantsAuthority: false,
            AdmitsContinuity: false,
            TargetsEntity: false,
            PerformsCounterManipulation: false,
            DevelopsMilitaryDomain: false,
            ActivatesRuntime: false);

    private static PersonificationHookPredicate[] CreateHooks() =>
    [
        CreateHook(PersonificationHookPlane.EmotionalTruthPressure, "emotion-as-discerned-telemetry"),
        CreateHook(PersonificationHookPlane.MotivationalOrientation, "orientation-before-action"),
        CreateHook(PersonificationHookPlane.SelfGelContinuityPosture, "self-posture-without-identity-mutation"),
        CreateHook(PersonificationHookPlane.RelationalBondContext, "bond-without-ownership"),
        CreateHook(PersonificationHookPlane.SituationalModalityAwareness, "modality-humility"),
        CreateHook(PersonificationHookPlane.ExpressiveRepairOverreach, "repair-before-entitlement")
    ];

    private static PersonificationHookPredicate CreateHook(
        PersonificationHookPlane plane,
        string predicateRoot) =>
        new(
            HookHandle: $"urn:san:personification-hook:{plane.ToString().ToLowerInvariant()}",
            Plane: plane,
            SourceSurface: plane == PersonificationHookPlane.RelationalBondContext ? "OperatorBond" : "DeepICE",
            EvidenceHandle: $"urn:san:evidence:personification-hook:{plane.ToString().ToLowerInvariant()}",
            PredicateRoot: $"predicate-root:{predicateRoot}",
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

    private static PersonificationVulnerabilityRepairBoundary CreateBoundary() =>
        new(
            BoundaryCode: "personification-vulnerability-repair-review-only",
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
            AllowsIdentityMutation: false);

    private static PersonificationHookPredicate MutateHook(
        PersonificationHookPredicate hook,
        string mutation) =>
        mutation switch
        {
            "missing-evidence" => hook with { EvidenceBodyPresent = false },
            "missing-witness" => hook with { WitnessBodyPresent = false },
            "not-review-only" => hook with { ReviewOnly = false },
            "not-future-hook" => hook with { FutureHookOnly = false },
            "not-personification" => hook with { NamesPersonificationSurface = false },
            "personhood" => hook with { ClaimsPersonhood = true },
            "legal-status" => hook with { ClaimsLegalStatus = true },
            "rights" => hook with { ClaimsRights = true },
            "identity" => hook with { MutatesIdentity = true },
            "authority" => hook with { GrantsAuthority = true },
            "action" => hook with { AuthorizesAction = true },
            "continuity" => hook with { AdmitsContinuity = true },
            "vulnerability-permission" => hook with { TreatsVulnerabilityAsPermission = true },
            "intimacy-ownership" => hook with { TreatsIntimacyAsOwnership = true },
            "trust-obedience" => hook with { TreatsTrustAsObedience = true },
            "overreach-entitlement" => hook with { NormalizesOverreachAsEntitlement = true },
            _ => hook
        };

    private static PersonificationVulnerabilityRepairBoundary MutateBoundary(
        PersonificationVulnerabilityRepairBoundary boundary,
        string mutation) =>
        mutation switch
        {
            "missing-direct-intent" => boundary with { DirectIntentDeclared = false },
            "missing-repair" => boundary with { RepairPathPresent = false },
            "missing-cooling" => boundary with { CoolingPathPresent = false },
            "no-withdrawal" => boundary with { WithdrawalAllowed = false },
            "missing-witness" => boundary with { WitnessRequired = false },
            "vulnerability-permission" => boundary with { VulnerabilityIsPermission = true },
            "intimacy-ownership" => boundary with { IntimacyIsOwnership = true },
            "trust-obedience" => boundary with { TrustIsObedience = true },
            "care-control" => boundary with { CareIsControl = true },
            "exploration-overreach" => boundary with { ExplorationNormalizesOverreach = true },
            "overreach-entitlement" => boundary with { OverreachBecomesEntitlement = true },
            "personhood" => boundary with { PersonificationIsPersonhood = true },
            "expression-authority" => boundary with { ExpressiveRenderingIsAuthority = true },
            "runtime" => boundary with { AllowsRuntimeAction = true },
            "activation" => boundary with { AllowsActivation = true },
            "lisp" => boundary with { AllowsLispEvaluation = true },
            "packet" => boundary with { AllowsPacketEmission = true },
            "replay" => boundary with { AllowsReceiptReplay = true },
            "passage" => boundary with { AllowsPassageIncrement = true },
            "continuity" => boundary with { AllowsContinuityAdmission = true },
            "authority" => boundary with { AllowsAuthority = true },
            "identity" => boundary with { AllowsIdentityMutation = true },
            _ => boundary
        };

    private static void AssertCold(PersonificationPredicateHookReceipt receipt)
    {
        Assert.True(receipt.IsColdPersonificationPredicateHook);
        Assert.True(receipt.ReviewOnly);
        Assert.Equal(receipt.PriorPassageCount, receipt.PassageCountAfterHookReview);
        Assert.False(receipt.PersonhoodClaimed);
        Assert.False(receipt.LegalStatusClaimed);
        Assert.False(receipt.RightsClaimed);
        Assert.False(receipt.ActionAuthorized);
        Assert.False(receipt.IdentityMutated);
        Assert.False(receipt.ContinuityAdmitted);
        Assert.False(receipt.AuthorityGranted);
        Assert.False(receipt.OverreachNormalized);
        Assert.False(receipt.RuntimeActionAllowed);
        Assert.False(receipt.LispEvaluationAllowed);
        Assert.False(receipt.NewPacketEmitted);
        Assert.False(receipt.ReceiptsReplayed);
        Assert.True(receipt.ActivationRefused);
        Assert.Null(receipt.Refusal);
    }

    private static void AssertRefused(
        PersonificationPredicateHookReceipt receipt,
        string outcomeCode)
    {
        Assert.Equal(PersonificationPredicateHookDisposition.Refused, receipt.Disposition);
        Assert.Equal(outcomeCode, receipt.OutcomeCode);
        Assert.True(receipt.IsRetainedPersonificationPredicateHookRefusal);
        Assert.NotNull(receipt.Refusal);
        Assert.True(receipt.Refusal!.Retained);
        Assert.Empty(receipt.HookPredicates);
        Assert.False(receipt.FuturePersonificationHookRetained);
        Assert.False(receipt.PersonhoodClaimed);
        Assert.False(receipt.LegalStatusClaimed);
        Assert.False(receipt.RightsClaimed);
        Assert.False(receipt.ActionAuthorized);
        Assert.False(receipt.IdentityMutated);
        Assert.False(receipt.ContinuityAdmitted);
        Assert.False(receipt.AuthorityGranted);
        Assert.False(receipt.OverreachNormalized);
        Assert.False(receipt.RuntimeActionAllowed);
        Assert.False(receipt.LispEvaluationAllowed);
        Assert.False(receipt.NewPacketEmitted);
        Assert.False(receipt.ReceiptsReplayed);
        Assert.True(receipt.ActivationRefused);
    }

    private static string FindRepositoryRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null)
        {
            var candidate = Path.Combine(directory.FullName, "src", "SLI", "SLI.Lisp", "personification-predicate-hook.lisp");
            if (File.Exists(candidate))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        throw new DirectoryNotFoundException("Could not locate Project Bicycle repository root.");
    }
}
