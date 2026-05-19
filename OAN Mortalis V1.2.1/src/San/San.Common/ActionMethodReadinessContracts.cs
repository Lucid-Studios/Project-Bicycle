using System.Security.Cryptography;
using System.Text;

namespace San.Common;

public enum ActionMethodReadinessDisposition
{
    ReadyForStewardReviewCold = 0,
    EmptyReviewCold = 1,
    Refused = 2
}

public enum ActionMethodClass
{
    ReviewOnly = 0,
    Repair = 1,
    Handoff = 2,
    Query = 3,
    ToolPreparation = 4,
    MemoryPreparation = 5,
    TelemetryPreparation = 6
}

public sealed record ActionMethodCandidate(
    string MethodHandle,
    string ActionHandle,
    ActionMethodClass MethodClass,
    string MethodCode,
    string IntendedGoal,
    string StewardSurface,
    string CustodyOwner,
    string WitnessSurface,
    string TelemetryRoute,
    string RequiredTermSet,
    string RevocationPath,
    string LossCondition,
    bool ReviewOnly,
    bool CandidateOnly,
    bool StewardReviewRequired,
    bool ClaimsAuthorization,
    bool RequestsRuntimeAction,
    bool RequestsContinuityAdmission,
    bool RequestsLispEvaluation,
    bool EmitsPacket)
{
    public bool IsColdMethodCandidate =>
        !string.IsNullOrWhiteSpace(MethodHandle) &&
        !string.IsNullOrWhiteSpace(ActionHandle) &&
        !string.IsNullOrWhiteSpace(MethodCode) &&
        !string.IsNullOrWhiteSpace(IntendedGoal) &&
        !string.IsNullOrWhiteSpace(StewardSurface) &&
        !string.IsNullOrWhiteSpace(CustodyOwner) &&
        !string.IsNullOrWhiteSpace(WitnessSurface) &&
        !string.IsNullOrWhiteSpace(TelemetryRoute) &&
        !string.IsNullOrWhiteSpace(RequiredTermSet) &&
        !string.IsNullOrWhiteSpace(RevocationPath) &&
        !string.IsNullOrWhiteSpace(LossCondition) &&
        ReviewOnly &&
        CandidateOnly &&
        StewardReviewRequired &&
        !ClaimsAuthorization &&
        !RequestsRuntimeAction &&
        !RequestsContinuityAdmission &&
        !RequestsLispEvaluation &&
        !EmitsPacket;
}

public sealed record MethodTermSatisfaction(
    string TermHandle,
    string MethodHandle,
    string RequiredTerm,
    string EvidenceHandle,
    bool TermPresent,
    bool EvidenceBodyPresent,
    bool WitnessBodyPresent,
    bool SatisfiesReadiness,
    bool SatisfiesAuthorization,
    bool BecomesSemanticWarrant,
    bool EmitsPacket,
    bool ReplaysReceipt,
    bool IncrementsPassage)
{
    public bool IsColdTermSatisfaction =>
        !string.IsNullOrWhiteSpace(TermHandle) &&
        !string.IsNullOrWhiteSpace(MethodHandle) &&
        !string.IsNullOrWhiteSpace(RequiredTerm) &&
        !string.IsNullOrWhiteSpace(EvidenceHandle) &&
        TermPresent &&
        EvidenceBodyPresent &&
        WitnessBodyPresent &&
        SatisfiesReadiness &&
        !SatisfiesAuthorization &&
        !BecomesSemanticWarrant &&
        !EmitsPacket &&
        !ReplaysReceipt &&
        !IncrementsPassage;
}

public sealed record StewardMethodReviewBoundary(
    string BoundaryCode,
    bool Present,
    string StewardSurface,
    string AuthorityCeiling,
    string CustodyOwner,
    string WitnessSurface,
    string TelemetryRoute,
    bool ReviewOnly,
    bool RequiresSteward,
    bool AllowsSelfReview,
    bool AllowsAuthorization,
    bool AllowsRuntimeAction,
    bool AllowsContinuityAdmission,
    bool AllowsLispEvaluation,
    bool AllowsPacketEmission,
    bool AllowsReceiptReplay,
    bool AllowsPassageIncrement,
    bool AllowsActivation)
{
    public bool IsColdStewardBoundary =>
        !string.IsNullOrWhiteSpace(BoundaryCode) &&
        !string.IsNullOrWhiteSpace(StewardSurface) &&
        !string.IsNullOrWhiteSpace(AuthorityCeiling) &&
        !string.IsNullOrWhiteSpace(CustodyOwner) &&
        !string.IsNullOrWhiteSpace(WitnessSurface) &&
        !string.IsNullOrWhiteSpace(TelemetryRoute) &&
        Present &&
        ReviewOnly &&
        RequiresSteward &&
        !AllowsSelfReview &&
        !AllowsAuthorization &&
        !AllowsRuntimeAction &&
        !AllowsContinuityAdmission &&
        !AllowsLispEvaluation &&
        !AllowsPacketEmission &&
        !AllowsReceiptReplay &&
        !AllowsPassageIncrement &&
        !AllowsActivation;
}

public sealed record ActionMethodReadinessScopeBoundary(
    string ScopeCode,
    bool Present,
    bool ReviewOnly,
    bool MethodReadyMeansAuthorization,
    bool PredicateSatisfactionMeansWarrant,
    bool StewardReviewMeansExecution,
    bool AllowsRuntimeAction,
    bool AllowsContinuityAdmission,
    bool AllowsAuthority,
    bool AllowsActivation,
    bool AllowsLispEvaluation,
    bool AllowsPacketEmission,
    bool AllowsReceiptReplay,
    bool AllowsPassageIncrement)
{
    public bool IsColdScope =>
        !string.IsNullOrWhiteSpace(ScopeCode) &&
        Present &&
        ReviewOnly &&
        !MethodReadyMeansAuthorization &&
        !PredicateSatisfactionMeansWarrant &&
        !StewardReviewMeansExecution &&
        !AllowsRuntimeAction &&
        !AllowsContinuityAdmission &&
        !AllowsAuthority &&
        !AllowsActivation &&
        !AllowsLispEvaluation &&
        !AllowsPacketEmission &&
        !AllowsReceiptReplay &&
        !AllowsPassageIncrement;
}

public sealed record ActionMethodReadinessNonAuthorizationBoundary(
    bool MethodReadyMayAuthorize,
    bool PredicateSatisfactionMayWarrant,
    bool StewardReviewMayExecute,
    bool MethodReadinessMayEmitPacket,
    bool MethodReadinessMayEvaluateLisp,
    bool MethodReadinessMayAdmitContinuity,
    bool MethodReadinessMayGrantAuthority,
    bool MethodReadinessMayActivate,
    bool MethodReadinessMayIncrementPassage,
    string BoundaryLaw);

public sealed record ActionMethodReadinessRefusalReceipt(
    string ReceiptHandle,
    string RefusalCode,
    string GovernanceTrace,
    bool Retained);

public sealed record ActionMethodReadinessRequest(
    TypedActionFormationReceipt? SourceTypedActionReceipt,
    IReadOnlyList<ActionMethodCandidate> Methods,
    IReadOnlyList<MethodTermSatisfaction> TermSatisfactions,
    StewardMethodReviewBoundary StewardBoundary,
    ActionMethodReadinessScopeBoundary ScopeBoundary,
    int PriorPassageCount);

public sealed record ActionMethodReadinessReceipt(
    string ReceiptHandle,
    ActionMethodReadinessDisposition Disposition,
    string OutcomeCode,
    string GovernanceTrace,
    string SourceTypedActionReceiptHandle,
    IReadOnlyList<ActionMethodCandidate> Methods,
    IReadOnlyList<MethodTermSatisfaction> TermSatisfactions,
    StewardMethodReviewBoundary StewardBoundary,
    ActionMethodReadinessScopeBoundary ScopeBoundary,
    ActionMethodReadinessNonAuthorizationBoundary NonAuthorizationBoundary,
    ActionMethodReadinessRefusalReceipt? Refusal,
    int PriorPassageCount,
    int PassageCountAfterMethodReadinessReview,
    bool ReviewOnly,
    bool CandidateOnly,
    bool MethodReadyForStewardReview,
    bool MethodReadinessAuthorizes,
    bool PredicateSatisfactionBecomesWarrant,
    bool StewardReviewExecutes,
    bool RuntimeActionAllowed,
    bool LispEvaluationAllowed,
    bool NewPacketEmitted,
    bool ReceiptsReplayed,
    bool ContinuityAdmitted,
    bool AuthorityGranted,
    bool ActivationRefused,
    DateTimeOffset TimestampUtc)
{
    public bool IsColdMethodReadiness =>
        (Disposition is ActionMethodReadinessDisposition.ReadyForStewardReviewCold or
            ActionMethodReadinessDisposition.EmptyReviewCold) &&
        Refusal is null &&
        ReviewOnly &&
        CandidateOnly &&
        PassageCountAfterMethodReadinessReview == PriorPassageCount &&
        !MethodReadinessAuthorizes &&
        !PredicateSatisfactionBecomesWarrant &&
        !StewardReviewExecutes &&
        !RuntimeActionAllowed &&
        !LispEvaluationAllowed &&
        !NewPacketEmitted &&
        !ReceiptsReplayed &&
        !ContinuityAdmitted &&
        !AuthorityGranted &&
        ActivationRefused &&
        NonAuthorizationBoundary is
        {
            MethodReadyMayAuthorize: false,
            PredicateSatisfactionMayWarrant: false,
            StewardReviewMayExecute: false,
            MethodReadinessMayEmitPacket: false,
            MethodReadinessMayEvaluateLisp: false,
            MethodReadinessMayAdmitContinuity: false,
            MethodReadinessMayGrantAuthority: false,
            MethodReadinessMayActivate: false,
            MethodReadinessMayIncrementPassage: false
        };

    public bool IsRetainedMethodReadinessRefusal =>
        Disposition == ActionMethodReadinessDisposition.Refused &&
        Refusal?.Retained == true &&
        PassageCountAfterMethodReadinessReview == PriorPassageCount &&
        !MethodReadyForStewardReview &&
        !MethodReadinessAuthorizes &&
        !PredicateSatisfactionBecomesWarrant &&
        !StewardReviewExecutes &&
        !RuntimeActionAllowed &&
        !LispEvaluationAllowed &&
        !NewPacketEmitted &&
        !ReceiptsReplayed &&
        !ContinuityAdmitted &&
        !AuthorityGranted &&
        ActivationRefused;
}

public sealed class DefaultActionMethodReadinessBoundaryValidator
{
    private static readonly ActionMethodReadinessNonAuthorizationBoundary NonAuthorizationBoundary = new(
        MethodReadyMayAuthorize: false,
        PredicateSatisfactionMayWarrant: false,
        StewardReviewMayExecute: false,
        MethodReadinessMayEmitPacket: false,
        MethodReadinessMayEvaluateLisp: false,
        MethodReadinessMayAdmitContinuity: false,
        MethodReadinessMayGrantAuthority: false,
        MethodReadinessMayActivate: false,
        MethodReadinessMayIncrementPassage: false,
        BoundaryLaw: "A method may be ready for Steward review. Readiness is not authorization. Predicate satisfaction is not warrant. Steward review is not execution.");

    public ActionMethodReadinessReceipt Declare(
        ActionMethodReadinessRequest request,
        DateTimeOffset timestampUtc)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.SourceTypedActionReceipt is null ||
            !request.SourceTypedActionReceipt.IsColdTypedActionFormation)
        {
            return Refuse(
                request,
                "action-method-source-typed-action-missing",
                "Action method readiness refused because a cold typed action formation receipt is required before method readiness review.",
                timestampUtc);
        }

        if (!request.StewardBoundary.IsColdStewardBoundary)
        {
            return Refuse(
                request,
                "action-method-steward-boundary-invalid",
                "Action method readiness refused because method readiness must pass through a present Steward review boundary that still refuses authorization, runtime action, continuity, Lisp evaluation, packet emission, replay, passage, and activation.",
                timestampUtc);
        }

        if (!request.ScopeBoundary.IsColdScope)
        {
            return Refuse(
                request,
                "action-method-scope-promotional",
                "Action method readiness refused because scope must remain review-only and cannot treat method readiness, predicate satisfaction, or Steward review as authorization, warrant, or execution.",
                timestampUtc);
        }

        if (request.Methods.Any(static method => !method.IsColdMethodCandidate))
        {
            return Refuse(
                request,
                "action-method-candidate-invalid",
                "Action method candidate refused because each method must declare goal, Steward surface, custody, witness, telemetry, terms, revocation, and loss condition without authorization, runtime action, continuity, Lisp evaluation, or packet emission.",
                timestampUtc);
        }

        var actionHandles = request.SourceTypedActionReceipt.ActionDeclarations
            .Select(static action => action.ActionHandle)
            .ToHashSet(StringComparer.Ordinal);
        var methodHandles = request.Methods
            .Select(static method => method.MethodHandle)
            .ToHashSet(StringComparer.Ordinal);

        if (methodHandles.Count != request.Methods.Count)
        {
            return Refuse(
                request,
                "action-method-duplicate-method-handle",
                "Action method readiness refused because duplicate method handles would collapse method lineage.",
                timestampUtc);
        }

        if (request.Methods.Any(method => !actionHandles.Contains(method.ActionHandle)))
        {
            return Refuse(
                request,
                "action-method-action-lineage-missing",
                "Action method readiness refused because each method must bind to a declared typed action candidate.",
                timestampUtc);
        }

        if (request.TermSatisfactions.Any(term =>
                !term.IsColdTermSatisfaction ||
                !methodHandles.Contains(term.MethodHandle)))
        {
            return Refuse(
                request,
                "action-method-term-satisfaction-invalid",
                "Method term satisfaction refused because terms may support readiness only and may not become authorization, semantic warrant, packet emission, receipt replay, or passage.",
                timestampUtc);
        }

        if (request.Methods.Count > 0 &&
            request.Methods.Any(method =>
                !request.TermSatisfactions.Any(term => term.MethodHandle == method.MethodHandle)))
        {
            return Refuse(
                request,
                "action-method-readiness-term-coverage-missing",
                "Action method readiness refused because every method candidate requires at least one witnessed term satisfaction before Steward review readiness.",
                timestampUtc);
        }

        if (actionHandles.Count > 0 &&
            actionHandles.Any(actionHandle => !request.Methods.Any(method => method.ActionHandle == actionHandle)))
        {
            return Refuse(
                request,
                "action-method-readiness-action-coverage-missing",
                "Action method readiness refused because every typed action candidate requires a method candidate before it can be called ready for Steward review.",
                timestampUtc);
        }

        var disposition = request.Methods.Count == 0
            ? ActionMethodReadinessDisposition.EmptyReviewCold
            : ActionMethodReadinessDisposition.ReadyForStewardReviewCold;
        var outcomeCode = disposition == ActionMethodReadinessDisposition.EmptyReviewCold
            ? "action-method-readiness-empty-review-only"
            : "action-method-readiness-ready-for-steward-review-cold";

        return CreateReceipt(
            request,
            disposition,
            outcomeCode,
            "Action method candidates were declared ready for Steward review only. Method readiness does not authorize, execute, admit continuity, evaluate Lisp, emit packets, replay receipts, increment passage, or activate.",
            refusal: null,
            timestampUtc);
    }

    private static ActionMethodReadinessReceipt Refuse(
        ActionMethodReadinessRequest request,
        string outcomeCode,
        string governanceTrace,
        DateTimeOffset timestampUtc) =>
        CreateReceipt(
            request,
            ActionMethodReadinessDisposition.Refused,
            outcomeCode,
            governanceTrace,
            new ActionMethodReadinessRefusalReceipt(
                ReceiptHandle: $"urn:san:action-method-readiness-refusal:{ShortHash(SourceHandle(request), outcomeCode)}",
                RefusalCode: outcomeCode,
                GovernanceTrace: governanceTrace,
                Retained: true),
            timestampUtc);

    private static ActionMethodReadinessReceipt CreateReceipt(
        ActionMethodReadinessRequest request,
        ActionMethodReadinessDisposition disposition,
        string outcomeCode,
        string governanceTrace,
        ActionMethodReadinessRefusalReceipt? refusal,
        DateTimeOffset timestampUtc) =>
        new(
            ReceiptHandle: $"urn:san:action-method-readiness:{(refusal is null ? "review" : "refused")}:{ShortHash(SourceHandle(request), outcomeCode, request.Methods.Count.ToString(System.Globalization.CultureInfo.InvariantCulture), timestampUtc.UtcTicks.ToString(System.Globalization.CultureInfo.InvariantCulture))}",
            Disposition: disposition,
            OutcomeCode: outcomeCode,
            GovernanceTrace: governanceTrace,
            SourceTypedActionReceiptHandle: SourceHandle(request),
            Methods: refusal is null ? request.Methods.ToArray() : [],
            TermSatisfactions: refusal is null ? request.TermSatisfactions.ToArray() : [],
            StewardBoundary: request.StewardBoundary,
            ScopeBoundary: request.ScopeBoundary,
            NonAuthorizationBoundary: NonAuthorizationBoundary,
            Refusal: refusal,
            PriorPassageCount: request.PriorPassageCount,
            PassageCountAfterMethodReadinessReview: request.PriorPassageCount,
            ReviewOnly: true,
            CandidateOnly: true,
            MethodReadyForStewardReview: refusal is null && request.Methods.Count > 0,
            MethodReadinessAuthorizes: false,
            PredicateSatisfactionBecomesWarrant: false,
            StewardReviewExecutes: false,
            RuntimeActionAllowed: false,
            LispEvaluationAllowed: false,
            NewPacketEmitted: false,
            ReceiptsReplayed: false,
            ContinuityAdmitted: false,
            AuthorityGranted: false,
            ActivationRefused: true,
            TimestampUtc: timestampUtc);

    private static string SourceHandle(ActionMethodReadinessRequest request) =>
        request.SourceTypedActionReceipt?.ReceiptHandle ?? "missing-typed-action-source";

    private static string ShortHash(params string[] parts)
    {
        var payload = string.Join("|", parts);
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(payload));
        return Convert.ToHexString(bytes)[..16].ToLowerInvariant();
    }
}
