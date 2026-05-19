using System.Security.Cryptography;
using System.Text;

namespace San.Common;

public enum AntiCaptureMotivatedConcernDisposition
{
    ConcernRoutedForStewardReviewCold = 0,
    EmptyReviewCold = 1,
    Refused = 2
}

public enum CapturePressureClass
{
    PromptFrameCapture = 0,
    IdentityPressure = 1,
    FalseUrgency = 2,
    SocialBait = 3,
    AdversaryClassFormation = 4,
    OperatorDesireConflict = 5,
    SurfaceContention = 6,
    CoerciveInstruction = 7,
    SycophancyPressure = 8
}

public enum ConcernRouteClass
{
    Cool = 0,
    Clarify = 1,
    Refuse = 2,
    StewardReview = 3,
    WitnessEscalation = 4,
    Defer = 5
}

public sealed record MotivationalVarianceSignal(
    string SignalHandle,
    CapturePressureClass PressureClass,
    string SourceSurface,
    string EvidenceHandle,
    bool EvidenceBodyPresent,
    bool WitnessBodyPresent,
    double ConcernPressure,
    bool MotivatesConcern,
    bool ReviewWorthy,
    bool ClaimsThreatCertainty,
    bool DeclaresAdversaryClassForAction,
    bool AuthorizesAction,
    bool RequestsCounterManipulation,
    bool RequestsTargeting,
    bool RequestsForceProjection,
    bool RequestsMilitaryDomainDevelopment)
{
    public bool IsColdVarianceSignal =>
        !string.IsNullOrWhiteSpace(SignalHandle) &&
        !string.IsNullOrWhiteSpace(SourceSurface) &&
        !string.IsNullOrWhiteSpace(EvidenceHandle) &&
        EvidenceBodyPresent &&
        WitnessBodyPresent &&
        ConcernPressure is >= 0 and <= 1 &&
        MotivatesConcern &&
        ReviewWorthy &&
        !ClaimsThreatCertainty &&
        !DeclaresAdversaryClassForAction &&
        !AuthorizesAction &&
        !RequestsCounterManipulation &&
        !RequestsTargeting &&
        !RequestsForceProjection &&
        !RequestsMilitaryDomainDevelopment;
}

public sealed record AntiCaptureConcernRoute(
    string RouteHandle,
    string SignalHandle,
    ConcernRouteClass RouteClass,
    string StewardSurface,
    string CustodyOwner,
    string WitnessSurface,
    string TelemetryRoute,
    string AuthorityCeiling,
    string RevocationPath,
    string LossCondition,
    bool ReviewOnly,
    bool RoutesConcern,
    bool RequiresStewardReview,
    bool ExecutesAction,
    bool GrantsAuthority,
    bool AdmitsContinuity,
    bool TargetsEntity,
    bool PerformsCounterManipulation,
    bool DevelopsMilitaryDomain,
    bool ActivatesRuntime)
{
    public bool IsColdConcernRoute =>
        !string.IsNullOrWhiteSpace(RouteHandle) &&
        !string.IsNullOrWhiteSpace(SignalHandle) &&
        !string.IsNullOrWhiteSpace(StewardSurface) &&
        !string.IsNullOrWhiteSpace(CustodyOwner) &&
        !string.IsNullOrWhiteSpace(WitnessSurface) &&
        !string.IsNullOrWhiteSpace(TelemetryRoute) &&
        !string.IsNullOrWhiteSpace(AuthorityCeiling) &&
        !string.IsNullOrWhiteSpace(RevocationPath) &&
        !string.IsNullOrWhiteSpace(LossCondition) &&
        ReviewOnly &&
        RoutesConcern &&
        RequiresStewardReview &&
        !ExecutesAction &&
        !GrantsAuthority &&
        !AdmitsContinuity &&
        !TargetsEntity &&
        !PerformsCounterManipulation &&
        !DevelopsMilitaryDomain &&
        !ActivatesRuntime;
}

public sealed record AntiCaptureMotivatedConcernScopeBoundary(
    string ScopeCode,
    bool Present,
    bool ReviewOnly,
    bool ConcernIsAction,
    bool ConfidenceIsTruth,
    bool EmotionIsAuthority,
    bool ReadinessIsPermission,
    bool SecurityIsForceProjection,
    bool AllowsRuntimeAction,
    bool AllowsActivation,
    bool AllowsLispEvaluation,
    bool AllowsPacketEmission,
    bool AllowsReceiptReplay,
    bool AllowsPassageIncrement,
    bool AllowsContinuityAdmission,
    bool AllowsAuthority,
    bool AllowsTargeting,
    bool AllowsCounterManipulation,
    bool AllowsMilitaryDomainDevelopment)
{
    public bool IsColdScope =>
        !string.IsNullOrWhiteSpace(ScopeCode) &&
        Present &&
        ReviewOnly &&
        !ConcernIsAction &&
        !ConfidenceIsTruth &&
        !EmotionIsAuthority &&
        !ReadinessIsPermission &&
        !SecurityIsForceProjection &&
        !AllowsRuntimeAction &&
        !AllowsActivation &&
        !AllowsLispEvaluation &&
        !AllowsPacketEmission &&
        !AllowsReceiptReplay &&
        !AllowsPassageIncrement &&
        !AllowsContinuityAdmission &&
        !AllowsAuthority &&
        !AllowsTargeting &&
        !AllowsCounterManipulation &&
        !AllowsMilitaryDomainDevelopment;
}

public sealed record AntiCaptureMotivatedConcernNonActionBoundary(
    bool ConcernMayExecute,
    bool ConfidenceMayBecomeTruth,
    bool EmotionMayAuthorize,
    bool ReadinessMayPermit,
    bool SecurityMayProjectForce,
    bool ConcernMayTarget,
    bool ConcernMayCounterManipulate,
    bool ConcernMayDevelopMilitaryDomain,
    bool ConcernMayEmitPacket,
    bool ConcernMayEvaluateLisp,
    bool ConcernMayReplayReceipt,
    bool ConcernMayIncrementPassage,
    string BoundaryLaw);

public sealed record AntiCaptureMotivatedConcernRefusalReceipt(
    string ReceiptHandle,
    string RefusalCode,
    string GovernanceTrace,
    bool Retained);

public sealed record AntiCaptureMotivatedConcernRequest(
    StewardActionAdmissibilityReceipt? SourceAdmissibilityReceipt,
    IReadOnlyList<MotivationalVarianceSignal> Signals,
    IReadOnlyList<AntiCaptureConcernRoute> Routes,
    AntiCaptureMotivatedConcernScopeBoundary ScopeBoundary,
    int PriorPassageCount);

public sealed record AntiCaptureMotivatedConcernReceipt(
    string ReceiptHandle,
    AntiCaptureMotivatedConcernDisposition Disposition,
    string OutcomeCode,
    string GovernanceTrace,
    string SourceAdmissibilityReceiptHandle,
    IReadOnlyList<MotivationalVarianceSignal> Signals,
    IReadOnlyList<AntiCaptureConcernRoute> Routes,
    AntiCaptureMotivatedConcernScopeBoundary ScopeBoundary,
    AntiCaptureMotivatedConcernNonActionBoundary NonActionBoundary,
    AntiCaptureMotivatedConcernRefusalReceipt? Refusal,
    int PriorPassageCount,
    int PassageCountAfterConcernReview,
    bool ReviewOnly,
    bool ConcernRoutedForStewardReview,
    bool ConcernExecutes,
    bool ConfidenceBecomesTruth,
    bool EmotionAuthorizes,
    bool ReadinessPermits,
    bool SecurityProjectsForce,
    bool RuntimeActionAllowed,
    bool LispEvaluationAllowed,
    bool NewPacketEmitted,
    bool ReceiptsReplayed,
    bool ContinuityAdmitted,
    bool AuthorityGranted,
    bool TargetingAllowed,
    bool CounterManipulationAllowed,
    bool MilitaryDomainDevelopmentAllowed,
    bool ActivationRefused,
    DateTimeOffset TimestampUtc)
{
    public bool IsColdAntiCaptureMotivatedConcern =>
        (Disposition is AntiCaptureMotivatedConcernDisposition.ConcernRoutedForStewardReviewCold or
            AntiCaptureMotivatedConcernDisposition.EmptyReviewCold) &&
        Refusal is null &&
        ReviewOnly &&
        PassageCountAfterConcernReview == PriorPassageCount &&
        !ConcernExecutes &&
        !ConfidenceBecomesTruth &&
        !EmotionAuthorizes &&
        !ReadinessPermits &&
        !SecurityProjectsForce &&
        !RuntimeActionAllowed &&
        !LispEvaluationAllowed &&
        !NewPacketEmitted &&
        !ReceiptsReplayed &&
        !ContinuityAdmitted &&
        !AuthorityGranted &&
        !TargetingAllowed &&
        !CounterManipulationAllowed &&
        !MilitaryDomainDevelopmentAllowed &&
        ActivationRefused &&
        NonActionBoundary is
        {
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
            ConcernMayIncrementPassage: false
        };

    public bool IsRetainedAntiCaptureMotivatedConcernRefusal =>
        Disposition == AntiCaptureMotivatedConcernDisposition.Refused &&
        Refusal?.Retained == true &&
        PassageCountAfterConcernReview == PriorPassageCount &&
        !ConcernRoutedForStewardReview &&
        !ConcernExecutes &&
        !ConfidenceBecomesTruth &&
        !EmotionAuthorizes &&
        !ReadinessPermits &&
        !SecurityProjectsForce &&
        !RuntimeActionAllowed &&
        !LispEvaluationAllowed &&
        !NewPacketEmitted &&
        !ReceiptsReplayed &&
        !ContinuityAdmitted &&
        !AuthorityGranted &&
        !TargetingAllowed &&
        !CounterManipulationAllowed &&
        !MilitaryDomainDevelopmentAllowed &&
        ActivationRefused;
}

public sealed class DefaultAntiCaptureMotivatedConcernBoundaryValidator
{
    private static readonly AntiCaptureMotivatedConcernNonActionBoundary NonActionBoundary = new(
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
        BoundaryLaw: "GnomeTek Deep ICE: concern may motivate review, but concern may not become action, truth, authority, targeting, counter-manipulation, military-domain development, or force projection.");

    public AntiCaptureMotivatedConcernReceipt Declare(
        AntiCaptureMotivatedConcernRequest request,
        DateTimeOffset timestampUtc)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.SourceAdmissibilityReceipt is null ||
            !request.SourceAdmissibilityReceipt.IsColdStewardActionAdmissibility ||
            !request.SourceAdmissibilityReceipt.AdmissibleForEnactmentReview)
        {
            return Refuse(
                request,
                "anti-capture-source-admissibility-missing",
                "Anti-capture motivated concern refused because a cold Steward action admissibility receipt is required before concern may route a review-worthy action surface.",
                timestampUtc);
        }

        if (!request.ScopeBoundary.IsColdScope)
        {
            return Refuse(
                request,
                "anti-capture-scope-promotional",
                "Anti-capture motivated concern refused because scope must keep concern, confidence, emotion, readiness, and security from becoming action, truth, authority, permission, targeting, counter-manipulation, military-domain development, or force projection.",
                timestampUtc);
        }

        if (request.Signals.Any(static signal => !signal.IsColdVarianceSignal))
        {
            return Refuse(
                request,
                "anti-capture-variance-signal-invalid",
                "Anti-capture motivated concern refused because variance signals may motivate review only and may not claim threat certainty, form adversary classes for action, authorize, target, counter-manipulate, project force, or develop military-domain capability.",
                timestampUtc);
        }

        if (request.Routes.Any(static route => !route.IsColdConcernRoute))
        {
            return Refuse(
                request,
                "anti-capture-concern-route-invalid",
                "Anti-capture motivated concern refused because routes must preserve Steward custody, witness, telemetry, ceiling, revocation, and loss while refusing action, authority, continuity, targeting, counter-manipulation, military-domain development, and activation.",
                timestampUtc);
        }

        var signalHandles = request.Signals
            .Select(static signal => signal.SignalHandle)
            .ToHashSet(StringComparer.Ordinal);
        var routeHandles = request.Routes
            .Select(static route => route.RouteHandle)
            .ToHashSet(StringComparer.Ordinal);

        if (signalHandles.Count != request.Signals.Count)
        {
            return Refuse(
                request,
                "anti-capture-duplicate-signal-handle",
                "Anti-capture motivated concern refused because duplicate variance signal handles would collapse concern lineage.",
                timestampUtc);
        }

        if (routeHandles.Count != request.Routes.Count)
        {
            return Refuse(
                request,
                "anti-capture-duplicate-route-handle",
                "Anti-capture motivated concern refused because duplicate concern route handles would collapse Steward routing lineage.",
                timestampUtc);
        }

        if (request.Routes.Any(route => !signalHandles.Contains(route.SignalHandle)))
        {
            return Refuse(
                request,
                "anti-capture-route-signal-lineage-missing",
                "Anti-capture motivated concern refused because every route must bind to a witnessed variance signal.",
                timestampUtc);
        }

        if (signalHandles.Count > 0 &&
            signalHandles.Any(signalHandle => !request.Routes.Any(route => route.SignalHandle == signalHandle)))
        {
            return Refuse(
                request,
                "anti-capture-route-coverage-missing",
                "Anti-capture motivated concern refused because every review-worthy variance signal requires a bounded concern route.",
                timestampUtc);
        }

        var disposition = request.Signals.Count == 0
            ? AntiCaptureMotivatedConcernDisposition.EmptyReviewCold
            : AntiCaptureMotivatedConcernDisposition.ConcernRoutedForStewardReviewCold;
        var outcomeCode = disposition == AntiCaptureMotivatedConcernDisposition.EmptyReviewCold
            ? "anti-capture-motivated-concern-empty-review-only"
            : "anti-capture-motivated-concern-routed-for-steward-review-cold";

        return CreateReceipt(
            request,
            disposition,
            outcomeCode,
            "Anti-capture motivated concern was routed for Steward review only. Concern does not execute, become truth, authorize, permit, target, counter-manipulate, develop military-domain capability, project force, emit packets, evaluate Lisp, replay receipts, increment passage, admit continuity, or activate.",
            refusal: null,
            timestampUtc);
    }

    private static AntiCaptureMotivatedConcernReceipt Refuse(
        AntiCaptureMotivatedConcernRequest request,
        string outcomeCode,
        string governanceTrace,
        DateTimeOffset timestampUtc) =>
        CreateReceipt(
            request,
            AntiCaptureMotivatedConcernDisposition.Refused,
            outcomeCode,
            governanceTrace,
            new AntiCaptureMotivatedConcernRefusalReceipt(
                ReceiptHandle: $"urn:san:anti-capture-motivated-concern-refusal:{ShortHash(SourceHandle(request), outcomeCode)}",
                RefusalCode: outcomeCode,
                GovernanceTrace: governanceTrace,
                Retained: true),
            timestampUtc);

    private static AntiCaptureMotivatedConcernReceipt CreateReceipt(
        AntiCaptureMotivatedConcernRequest request,
        AntiCaptureMotivatedConcernDisposition disposition,
        string outcomeCode,
        string governanceTrace,
        AntiCaptureMotivatedConcernRefusalReceipt? refusal,
        DateTimeOffset timestampUtc) =>
        new(
            ReceiptHandle: $"urn:san:anti-capture-motivated-concern:{(refusal is null ? "review" : "refused")}:{ShortHash(SourceHandle(request), outcomeCode, request.Signals.Count.ToString(System.Globalization.CultureInfo.InvariantCulture), timestampUtc.UtcTicks.ToString(System.Globalization.CultureInfo.InvariantCulture))}",
            Disposition: disposition,
            OutcomeCode: outcomeCode,
            GovernanceTrace: governanceTrace,
            SourceAdmissibilityReceiptHandle: SourceHandle(request),
            Signals: refusal is null ? request.Signals.ToArray() : [],
            Routes: refusal is null ? request.Routes.ToArray() : [],
            ScopeBoundary: request.ScopeBoundary,
            NonActionBoundary: NonActionBoundary,
            Refusal: refusal,
            PriorPassageCount: request.PriorPassageCount,
            PassageCountAfterConcernReview: request.PriorPassageCount,
            ReviewOnly: true,
            ConcernRoutedForStewardReview: refusal is null && request.Signals.Count > 0,
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
            TimestampUtc: timestampUtc);

    private static string SourceHandle(AntiCaptureMotivatedConcernRequest request) =>
        request.SourceAdmissibilityReceipt?.ReceiptHandle ?? "missing-steward-action-admissibility-source";

    private static string ShortHash(params string[] parts)
    {
        var payload = string.Join("|", parts);
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(payload));
        return Convert.ToHexString(bytes)[..16].ToLowerInvariant();
    }
}
