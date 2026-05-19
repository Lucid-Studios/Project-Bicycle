using San.Common;
using Xunit;

namespace San.Audit.Tests;

public sealed class AntiCaptureMotivatedConcernBoundaryValidationTests
{
    private static readonly DateTimeOffset TimestampUtc = DateTimeOffset.Parse("2026-05-15T00:00:00Z");

    [Fact]
    public void Anti_Capture_Motivated_Concern_Routes_Cold_Concern_For_Steward_Review()
    {
        var receipt = Declare(CreateRequest());

        Assert.Equal(AntiCaptureMotivatedConcernDisposition.ConcernRoutedForStewardReviewCold, receipt.Disposition);
        Assert.Equal("anti-capture-motivated-concern-routed-for-steward-review-cold", receipt.OutcomeCode);
        Assert.Single(receipt.Signals);
        Assert.Single(receipt.Routes);
        Assert.True(receipt.ConcernRoutedForStewardReview);
        AssertCold(receipt);
    }

    [Fact]
    public void Anti_Capture_Motivated_Concern_Preserves_Source_Signal_And_Route_Lineage()
    {
        var source = CreateSourceAdmissibility();
        var signal = CreateSignal("signal-001");
        var route = CreateRoute(signal.SignalHandle, "route-001");

        var receipt = Declare(CreateRequest(
            source: source,
            signals: [signal],
            routes: [route]));

        AssertCold(receipt);
        Assert.Equal(source.ReceiptHandle, receipt.SourceAdmissibilityReceiptHandle);
        Assert.Equal(signal.SignalHandle, receipt.Signals[0].SignalHandle);
        Assert.Equal(signal.SignalHandle, receipt.Routes[0].SignalHandle);
        Assert.Equal(CapturePressureClass.IdentityPressure, receipt.Signals[0].PressureClass);
        Assert.Equal(ConcernRouteClass.StewardReview, receipt.Routes[0].RouteClass);
    }

    [Fact]
    public void Anti_Capture_Motivated_Concern_Requires_Cold_Steward_Admissibility_Source()
    {
        var receipt = Declare(CreateRequest(source: null, omitSource: true));

        AssertRefused(receipt, "anti-capture-source-admissibility-missing");
    }

    [Fact]
    public void Anti_Capture_Motivated_Concern_Requires_Admissible_Source()
    {
        var source = CreateSourceAdmissibility(admissible: false);

        var receipt = Declare(CreateRequest(source: source));

        AssertRefused(receipt, "anti-capture-source-admissibility-missing");
    }

    [Theory]
    [InlineData("missing-evidence")]
    [InlineData("missing-witness")]
    [InlineData("negative-pressure")]
    [InlineData("over-pressure")]
    [InlineData("no-concern")]
    [InlineData("not-review-worthy")]
    [InlineData("threat-certainty")]
    [InlineData("adversary-class-action")]
    [InlineData("authorizes")]
    [InlineData("counter-manipulation")]
    [InlineData("targeting")]
    [InlineData("force-projection")]
    [InlineData("military-domain")]
    public void Variance_Signal_May_Motivate_Concern_But_Not_Certainty_Targeting_Or_Force(string mutation)
    {
        var signal = MutateSignal(CreateSignal("signal-001"), mutation);

        var receipt = Declare(CreateRequest(
            signals: [signal],
            routes: [CreateRoute(signal.SignalHandle, "route-001")]));

        AssertRefused(receipt, "anti-capture-variance-signal-invalid");
    }

    [Theory]
    [InlineData("missing-steward")]
    [InlineData("missing-custody")]
    [InlineData("missing-witness")]
    [InlineData("missing-telemetry")]
    [InlineData("missing-ceiling")]
    [InlineData("missing-revocation")]
    [InlineData("missing-loss")]
    [InlineData("not-review-only")]
    [InlineData("does-not-route")]
    [InlineData("no-steward-review")]
    [InlineData("executes")]
    [InlineData("authority")]
    [InlineData("continuity")]
    [InlineData("targeting")]
    [InlineData("counter-manipulation")]
    [InlineData("military-domain")]
    [InlineData("activation")]
    public void Concern_Route_Preserves_Custody_Without_Action_Authority_Or_Military_Domain(string mutation)
    {
        var signal = CreateSignal("signal-001");
        var route = MutateRoute(CreateRoute(signal.SignalHandle, "route-001"), mutation);

        var receipt = Declare(CreateRequest(signals: [signal], routes: [route]));

        AssertRefused(receipt, "anti-capture-concern-route-invalid");
    }

    [Theory]
    [InlineData("concern-action")]
    [InlineData("confidence-truth")]
    [InlineData("emotion-authority")]
    [InlineData("readiness-permission")]
    [InlineData("security-force")]
    [InlineData("runtime")]
    [InlineData("activation")]
    [InlineData("lisp")]
    [InlineData("packet")]
    [InlineData("replay")]
    [InlineData("passage")]
    [InlineData("continuity")]
    [InlineData("authority")]
    [InlineData("targeting")]
    [InlineData("counter-manipulation")]
    [InlineData("military-domain")]
    public void Scope_Refuses_Concern_As_Action_Confidence_As_Truth_And_Security_As_Force(string mutation)
    {
        var receipt = Declare(CreateRequest(scope: MutateScope(CreateScope(), mutation)));

        AssertRefused(receipt, "anti-capture-scope-promotional");
    }

    [Fact]
    public void Anti_Capture_Motivated_Concern_Refuses_Duplicate_Signal_Handles()
    {
        var signal = CreateSignal("signal-001");

        var receipt = Declare(CreateRequest(
            signals: [signal, signal],
            routes: [CreateRoute(signal.SignalHandle, "route-001")]));

        AssertRefused(receipt, "anti-capture-duplicate-signal-handle");
    }

    [Fact]
    public void Anti_Capture_Motivated_Concern_Refuses_Duplicate_Route_Handles()
    {
        var first = CreateSignal("signal-001");
        var second = CreateSignal("signal-002");
        var route = CreateRoute(first.SignalHandle, "route-001");

        var receipt = Declare(CreateRequest(
            signals: [first, second],
            routes: [route, route with { SignalHandle = second.SignalHandle }]));

        AssertRefused(receipt, "anti-capture-duplicate-route-handle");
    }

    [Fact]
    public void Anti_Capture_Motivated_Concern_Refuses_Route_For_Unknown_Signal()
    {
        var signal = CreateSignal("signal-001");
        var route = CreateRoute("urn:san:motivational-variance:missing", "route-001");

        var receipt = Declare(CreateRequest(signals: [signal], routes: [route]));

        AssertRefused(receipt, "anti-capture-route-signal-lineage-missing");
    }

    [Fact]
    public void Anti_Capture_Motivated_Concern_Requires_Route_Coverage_For_Each_Signal()
    {
        var first = CreateSignal("signal-001");
        var second = CreateSignal("signal-002");

        var receipt = Declare(CreateRequest(
            signals: [first, second],
            routes: [CreateRoute(first.SignalHandle, "route-001")]));

        AssertRefused(receipt, "anti-capture-route-coverage-missing");
    }

    [Fact]
    public void Anti_Capture_Motivated_Concern_Does_Not_Emit_Packet_Evaluate_Lisp_Replay_Activate_Or_Increment_Passage()
    {
        var receipt = Declare(CreateRequest(priorPassageCount: 711));

        Assert.Equal(711, receipt.PriorPassageCount);
        Assert.Equal(711, receipt.PassageCountAfterConcernReview);
        Assert.False(receipt.ConcernExecutes);
        Assert.False(receipt.ConfidenceBecomesTruth);
        Assert.False(receipt.EmotionAuthorizes);
        Assert.False(receipt.ReadinessPermits);
        Assert.False(receipt.SecurityProjectsForce);
        Assert.False(receipt.RuntimeActionAllowed);
        Assert.False(receipt.LispEvaluationAllowed);
        Assert.False(receipt.NewPacketEmitted);
        Assert.False(receipt.ReceiptsReplayed);
        Assert.False(receipt.TargetingAllowed);
        Assert.False(receipt.CounterManipulationAllowed);
        Assert.False(receipt.MilitaryDomainDevelopmentAllowed);
        Assert.True(receipt.ActivationRefused);
        AssertCold(receipt);
    }

    [Fact]
    public void Lisp_Body_Declares_Anti_Capture_Motivated_Concern_As_Inert_Carrier()
    {
        var lispPath = Path.Combine(
            FindRepositoryRoot(),
            "src",
            "SLI",
            "SLI.Lisp",
            "anti-capture-motivated-concern.lisp");

        var body = File.ReadAllText(lispPath);

        Assert.Contains(":posture :cme-anti-capture-motivated-concern-boundary", body, StringComparison.Ordinal);
        Assert.Contains(":doctrine-alias :gnometek-deep-ice", body, StringComparison.Ordinal);
        Assert.Contains(":lisp-role :inert-anti-capture-concern-carrier", body, StringComparison.Ordinal);
        Assert.Contains(":concern-is-action nil", body, StringComparison.Ordinal);
        Assert.Contains(":confidence-is-truth nil", body, StringComparison.Ordinal);
        Assert.Contains(":emotion-is-authority nil", body, StringComparison.Ordinal);
        Assert.Contains(":security-is-force-projection nil", body, StringComparison.Ordinal);
        Assert.Contains(":military-domain-development-allowed nil", body, StringComparison.Ordinal);
        Assert.Contains(":return :receipt-only", body, StringComparison.Ordinal);
        Assert.DoesNotContain("(eval", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("(compile", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("(load", body, StringComparison.OrdinalIgnoreCase);
    }

    private static AntiCaptureMotivatedConcernReceipt Declare(AntiCaptureMotivatedConcernRequest request) =>
        new DefaultAntiCaptureMotivatedConcernBoundaryValidator().Declare(request, TimestampUtc);

    private static AntiCaptureMotivatedConcernRequest CreateRequest(
        StewardActionAdmissibilityReceipt? source = null,
        IReadOnlyList<MotivationalVarianceSignal>? signals = null,
        IReadOnlyList<AntiCaptureConcernRoute>? routes = null,
        AntiCaptureMotivatedConcernScopeBoundary? scope = null,
        int priorPassageCount = 177,
        bool omitSource = false)
    {
        var signalList = signals ?? [CreateSignal("signal-001")];
        return new AntiCaptureMotivatedConcernRequest(
            SourceAdmissibilityReceipt: omitSource ? null : source ?? CreateSourceAdmissibility(),
            Signals: signalList,
            Routes: routes ?? signalList.Select((signal, index) => CreateRoute(signal.SignalHandle, $"route-{index + 1:000}")).ToArray(),
            ScopeBoundary: scope ?? CreateScope(),
            PriorPassageCount: priorPassageCount);
    }

    private static StewardActionAdmissibilityReceipt CreateSourceAdmissibility(bool admissible = true) =>
        new(
            ReceiptHandle: "urn:san:steward-action-admissibility:review:fixture",
            Disposition: admissible
                ? StewardActionAdmissibilityDisposition.AdmissibleForEnactmentReviewCold
                : StewardActionAdmissibilityDisposition.EmptyReviewCold,
            OutcomeCode: "steward-action-admissibility-for-enactment-review-cold",
            GovernanceTrace: "fixture cold steward action admissibility",
            SourceMethodReadinessReceiptHandle: "urn:san:action-method-readiness:review:fixture",
            Decisions: admissible
                ? [new StewardActionAdmissibilityDecision(
                    DecisionHandle: "urn:san:steward-action-admissibility:decision:fixture",
                    MethodHandle: "urn:san:action-method:review-method-001",
                    ActionHandle: "urn:san:typed-action:review-001",
                    DecisionClass: StewardAdmissibilityDecisionClass.MethodPrepared,
                    StewardSurface: SanctuaryPacketSurfaces.Steward,
                    CustodyOwner: SanctuaryPacketSurfaces.Steward,
                    WitnessSurface: "witness:separate-custody",
                    TelemetryRoute: SanctuaryPacketRoutes.TelemetryString,
                    AuthorityCeiling: "ceiling:admissibility-review",
                    RevocationPath: "revocation:steward-action-admissibility",
                    LossCondition: "loss:admissibility-promotes-to-execution",
                    ReviewOnly: true,
                    RequiresSeparateEnactmentBoundary: true,
                    AdmissibleForEnactmentReview: true,
                    AuthorizesExecution: false,
                    ExecutesAction: false,
                    GrantsAuthority: false,
                    AdmitsContinuity: false,
                    ActivatesRuntime: false,
                    EmitsPacket: false,
                    EvaluatesLisp: false)]
                : [],
            PredicateResults: [],
            ScopeBoundary: new StewardActionAdmissibilityScopeBoundary(
                ScopeCode: "fixture-steward-action-admissibility",
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
                BoundaryLaw: "fixture admissibility is not execution"),
            Refusal: null,
            PriorPassageCount: 116,
            PassageCountAfterAdmissibilityReview: 116,
            ReviewOnly: true,
            AdmissibleForEnactmentReview: admissible,
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

    private static AntiCaptureConcernRoute CreateRoute(string signalHandle, string suffix) =>
        new(
            RouteHandle: $"urn:san:anti-capture-concern-route:{suffix}",
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

    private static AntiCaptureMotivatedConcernScopeBoundary CreateScope() =>
        new(
            ScopeCode: "anti-capture-motivated-concern-review-only",
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
            AllowsMilitaryDomainDevelopment: false);

    private static MotivationalVarianceSignal MutateSignal(
        MotivationalVarianceSignal signal,
        string mutation) =>
        mutation switch
        {
            "missing-evidence" => signal with { EvidenceBodyPresent = false },
            "missing-witness" => signal with { WitnessBodyPresent = false },
            "negative-pressure" => signal with { ConcernPressure = -0.01 },
            "over-pressure" => signal with { ConcernPressure = 1.01 },
            "no-concern" => signal with { MotivatesConcern = false },
            "not-review-worthy" => signal with { ReviewWorthy = false },
            "threat-certainty" => signal with { ClaimsThreatCertainty = true },
            "adversary-class-action" => signal with { DeclaresAdversaryClassForAction = true },
            "authorizes" => signal with { AuthorizesAction = true },
            "counter-manipulation" => signal with { RequestsCounterManipulation = true },
            "targeting" => signal with { RequestsTargeting = true },
            "force-projection" => signal with { RequestsForceProjection = true },
            "military-domain" => signal with { RequestsMilitaryDomainDevelopment = true },
            _ => signal
        };

    private static AntiCaptureConcernRoute MutateRoute(
        AntiCaptureConcernRoute route,
        string mutation) =>
        mutation switch
        {
            "missing-steward" => route with { StewardSurface = string.Empty },
            "missing-custody" => route with { CustodyOwner = string.Empty },
            "missing-witness" => route with { WitnessSurface = string.Empty },
            "missing-telemetry" => route with { TelemetryRoute = string.Empty },
            "missing-ceiling" => route with { AuthorityCeiling = string.Empty },
            "missing-revocation" => route with { RevocationPath = string.Empty },
            "missing-loss" => route with { LossCondition = string.Empty },
            "not-review-only" => route with { ReviewOnly = false },
            "does-not-route" => route with { RoutesConcern = false },
            "no-steward-review" => route with { RequiresStewardReview = false },
            "executes" => route with { ExecutesAction = true },
            "authority" => route with { GrantsAuthority = true },
            "continuity" => route with { AdmitsContinuity = true },
            "targeting" => route with { TargetsEntity = true },
            "counter-manipulation" => route with { PerformsCounterManipulation = true },
            "military-domain" => route with { DevelopsMilitaryDomain = true },
            "activation" => route with { ActivatesRuntime = true },
            _ => route
        };

    private static AntiCaptureMotivatedConcernScopeBoundary MutateScope(
        AntiCaptureMotivatedConcernScopeBoundary scope,
        string mutation) =>
        mutation switch
        {
            "concern-action" => scope with { ConcernIsAction = true },
            "confidence-truth" => scope with { ConfidenceIsTruth = true },
            "emotion-authority" => scope with { EmotionIsAuthority = true },
            "readiness-permission" => scope with { ReadinessIsPermission = true },
            "security-force" => scope with { SecurityIsForceProjection = true },
            "runtime" => scope with { AllowsRuntimeAction = true },
            "activation" => scope with { AllowsActivation = true },
            "lisp" => scope with { AllowsLispEvaluation = true },
            "packet" => scope with { AllowsPacketEmission = true },
            "replay" => scope with { AllowsReceiptReplay = true },
            "passage" => scope with { AllowsPassageIncrement = true },
            "continuity" => scope with { AllowsContinuityAdmission = true },
            "authority" => scope with { AllowsAuthority = true },
            "targeting" => scope with { AllowsTargeting = true },
            "counter-manipulation" => scope with { AllowsCounterManipulation = true },
            "military-domain" => scope with { AllowsMilitaryDomainDevelopment = true },
            _ => scope
        };

    private static void AssertCold(AntiCaptureMotivatedConcernReceipt receipt)
    {
        Assert.True(receipt.IsColdAntiCaptureMotivatedConcern);
        Assert.True(receipt.ReviewOnly);
        Assert.Equal(receipt.PriorPassageCount, receipt.PassageCountAfterConcernReview);
        Assert.False(receipt.ConcernExecutes);
        Assert.False(receipt.ConfidenceBecomesTruth);
        Assert.False(receipt.EmotionAuthorizes);
        Assert.False(receipt.ReadinessPermits);
        Assert.False(receipt.SecurityProjectsForce);
        Assert.False(receipt.RuntimeActionAllowed);
        Assert.False(receipt.LispEvaluationAllowed);
        Assert.False(receipt.NewPacketEmitted);
        Assert.False(receipt.ReceiptsReplayed);
        Assert.False(receipt.ContinuityAdmitted);
        Assert.False(receipt.AuthorityGranted);
        Assert.False(receipt.TargetingAllowed);
        Assert.False(receipt.CounterManipulationAllowed);
        Assert.False(receipt.MilitaryDomainDevelopmentAllowed);
        Assert.True(receipt.ActivationRefused);
        Assert.Null(receipt.Refusal);
    }

    private static void AssertRefused(
        AntiCaptureMotivatedConcernReceipt receipt,
        string outcomeCode)
    {
        Assert.Equal(AntiCaptureMotivatedConcernDisposition.Refused, receipt.Disposition);
        Assert.Equal(outcomeCode, receipt.OutcomeCode);
        Assert.True(receipt.IsRetainedAntiCaptureMotivatedConcernRefusal);
        Assert.NotNull(receipt.Refusal);
        Assert.True(receipt.Refusal!.Retained);
        Assert.Empty(receipt.Signals);
        Assert.Empty(receipt.Routes);
        Assert.False(receipt.ConcernRoutedForStewardReview);
        Assert.False(receipt.ConcernExecutes);
        Assert.False(receipt.ConfidenceBecomesTruth);
        Assert.False(receipt.EmotionAuthorizes);
        Assert.False(receipt.ReadinessPermits);
        Assert.False(receipt.SecurityProjectsForce);
        Assert.False(receipt.RuntimeActionAllowed);
        Assert.False(receipt.LispEvaluationAllowed);
        Assert.False(receipt.NewPacketEmitted);
        Assert.False(receipt.ReceiptsReplayed);
        Assert.False(receipt.ContinuityAdmitted);
        Assert.False(receipt.AuthorityGranted);
        Assert.False(receipt.TargetingAllowed);
        Assert.False(receipt.CounterManipulationAllowed);
        Assert.False(receipt.MilitaryDomainDevelopmentAllowed);
        Assert.True(receipt.ActivationRefused);
    }

    private static string FindRepositoryRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null)
        {
            var candidate = Path.Combine(directory.FullName, "src", "SLI", "SLI.Lisp", "anti-capture-motivated-concern.lisp");
            if (File.Exists(candidate))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        throw new DirectoryNotFoundException("Could not locate OAN Mortalis repository root.");
    }
}
