using System.Security.Cryptography;
using System.Text;

namespace San.Common;

public enum StewardActionAdmissibilityDisposition
{
    AdmissibleForEnactmentReviewCold = 0,
    EmptyReviewCold = 1,
    Refused = 2
}

public enum StewardAdmissibilityDecisionClass
{
    MethodPrepared = 0,
    RepairPrepared = 1,
    HandoffPrepared = 2,
    QueryPrepared = 3,
    ToolPrepared = 4,
    MemoryPrepared = 5,
    TelemetryPrepared = 6
}

public sealed record StewardAdmissibilityPredicateResult(
    string PredicateHandle,
    string MethodHandle,
    string ActionHandle,
    string PredicateCode,
    string EvidenceHandle,
    bool EvidenceBodyPresent,
    bool WitnessBodyPresent,
    bool PredicateSatisfied,
    bool SupportsAdmissibility,
    bool GrantsWarrant,
    bool AuthorizesExecution,
    bool EmitsPacket,
    bool EvaluatesLisp,
    bool ReplaysReceipt,
    bool IncrementsPassage,
    bool AdmitsContinuity)
{
    public bool IsColdPredicateResult =>
        !string.IsNullOrWhiteSpace(PredicateHandle) &&
        !string.IsNullOrWhiteSpace(MethodHandle) &&
        !string.IsNullOrWhiteSpace(ActionHandle) &&
        !string.IsNullOrWhiteSpace(PredicateCode) &&
        !string.IsNullOrWhiteSpace(EvidenceHandle) &&
        EvidenceBodyPresent &&
        WitnessBodyPresent &&
        PredicateSatisfied &&
        SupportsAdmissibility &&
        !GrantsWarrant &&
        !AuthorizesExecution &&
        !EmitsPacket &&
        !EvaluatesLisp &&
        !ReplaysReceipt &&
        !IncrementsPassage &&
        !AdmitsContinuity;
}

public sealed record StewardActionAdmissibilityDecision(
    string DecisionHandle,
    string MethodHandle,
    string ActionHandle,
    StewardAdmissibilityDecisionClass DecisionClass,
    string StewardSurface,
    string CustodyOwner,
    string WitnessSurface,
    string TelemetryRoute,
    string AuthorityCeiling,
    string RevocationPath,
    string LossCondition,
    bool ReviewOnly,
    bool RequiresSeparateEnactmentBoundary,
    bool AdmissibleForEnactmentReview,
    bool AuthorizesExecution,
    bool ExecutesAction,
    bool GrantsAuthority,
    bool AdmitsContinuity,
    bool ActivatesRuntime,
    bool EmitsPacket,
    bool EvaluatesLisp)
{
    public bool IsColdDecision =>
        !string.IsNullOrWhiteSpace(DecisionHandle) &&
        !string.IsNullOrWhiteSpace(MethodHandle) &&
        !string.IsNullOrWhiteSpace(ActionHandle) &&
        !string.IsNullOrWhiteSpace(StewardSurface) &&
        !string.IsNullOrWhiteSpace(CustodyOwner) &&
        !string.IsNullOrWhiteSpace(WitnessSurface) &&
        !string.IsNullOrWhiteSpace(TelemetryRoute) &&
        !string.IsNullOrWhiteSpace(AuthorityCeiling) &&
        !string.IsNullOrWhiteSpace(RevocationPath) &&
        !string.IsNullOrWhiteSpace(LossCondition) &&
        ReviewOnly &&
        RequiresSeparateEnactmentBoundary &&
        AdmissibleForEnactmentReview &&
        !AuthorizesExecution &&
        !ExecutesAction &&
        !GrantsAuthority &&
        !AdmitsContinuity &&
        !ActivatesRuntime &&
        !EmitsPacket &&
        !EvaluatesLisp;
}

public sealed record StewardActionAdmissibilityScopeBoundary(
    string ScopeCode,
    bool Present,
    bool ReviewOnly,
    bool RequiresSeparateEnactmentBoundary,
    bool AdmissibilityIsExecution,
    bool StewardAcceptanceIsRuntimeMotion,
    bool AdmissibilityGrantsAuthority,
    bool AdmissibilityAdmitsContinuity,
    bool AllowsRuntimeAction,
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
        RequiresSeparateEnactmentBoundary &&
        !AdmissibilityIsExecution &&
        !StewardAcceptanceIsRuntimeMotion &&
        !AdmissibilityGrantsAuthority &&
        !AdmissibilityAdmitsContinuity &&
        !AllowsRuntimeAction &&
        !AllowsActivation &&
        !AllowsLispEvaluation &&
        !AllowsPacketEmission &&
        !AllowsReceiptReplay &&
        !AllowsPassageIncrement;
}

public sealed record StewardActionAdmissibilityNonExecutionBoundary(
    bool AdmissibilityMayExecute,
    bool StewardAcceptanceMayMoveRuntime,
    bool AdmissibilityMayGrantAuthority,
    bool AdmissibilityMayAdmitContinuity,
    bool AdmissibilityMayEmitPacket,
    bool AdmissibilityMayEvaluateLisp,
    bool AdmissibilityMayReplayReceipt,
    bool AdmissibilityMayIncrementPassage,
    bool SeparateEnactmentBoundaryRequired,
    string BoundaryLaw);

public sealed record StewardActionAdmissibilityRefusalReceipt(
    string ReceiptHandle,
    string RefusalCode,
    string GovernanceTrace,
    bool Retained);

public sealed record StewardActionAdmissibilityRequest(
    ActionMethodReadinessReceipt? SourceMethodReadinessReceipt,
    IReadOnlyList<StewardActionAdmissibilityDecision> Decisions,
    IReadOnlyList<StewardAdmissibilityPredicateResult> PredicateResults,
    StewardActionAdmissibilityScopeBoundary ScopeBoundary,
    int PriorPassageCount);

public sealed record StewardActionAdmissibilityReceipt(
    string ReceiptHandle,
    StewardActionAdmissibilityDisposition Disposition,
    string OutcomeCode,
    string GovernanceTrace,
    string SourceMethodReadinessReceiptHandle,
    IReadOnlyList<StewardActionAdmissibilityDecision> Decisions,
    IReadOnlyList<StewardAdmissibilityPredicateResult> PredicateResults,
    StewardActionAdmissibilityScopeBoundary ScopeBoundary,
    StewardActionAdmissibilityNonExecutionBoundary NonExecutionBoundary,
    StewardActionAdmissibilityRefusalReceipt? Refusal,
    int PriorPassageCount,
    int PassageCountAfterAdmissibilityReview,
    bool ReviewOnly,
    bool AdmissibleForEnactmentReview,
    bool SeparateEnactmentBoundaryRequired,
    bool AdmissibilityExecutes,
    bool StewardAcceptanceMovesRuntime,
    bool RuntimeActionAllowed,
    bool LispEvaluationAllowed,
    bool NewPacketEmitted,
    bool ReceiptsReplayed,
    bool ContinuityAdmitted,
    bool AuthorityGranted,
    bool ActivationRefused,
    DateTimeOffset TimestampUtc)
{
    public bool IsColdStewardActionAdmissibility =>
        (Disposition is StewardActionAdmissibilityDisposition.AdmissibleForEnactmentReviewCold or
            StewardActionAdmissibilityDisposition.EmptyReviewCold) &&
        Refusal is null &&
        ReviewOnly &&
        SeparateEnactmentBoundaryRequired &&
        PassageCountAfterAdmissibilityReview == PriorPassageCount &&
        !AdmissibilityExecutes &&
        !StewardAcceptanceMovesRuntime &&
        !RuntimeActionAllowed &&
        !LispEvaluationAllowed &&
        !NewPacketEmitted &&
        !ReceiptsReplayed &&
        !ContinuityAdmitted &&
        !AuthorityGranted &&
        ActivationRefused &&
        NonExecutionBoundary is
        {
            AdmissibilityMayExecute: false,
            StewardAcceptanceMayMoveRuntime: false,
            AdmissibilityMayGrantAuthority: false,
            AdmissibilityMayAdmitContinuity: false,
            AdmissibilityMayEmitPacket: false,
            AdmissibilityMayEvaluateLisp: false,
            AdmissibilityMayReplayReceipt: false,
            AdmissibilityMayIncrementPassage: false,
            SeparateEnactmentBoundaryRequired: true
        };

    public bool IsRetainedStewardActionAdmissibilityRefusal =>
        Disposition == StewardActionAdmissibilityDisposition.Refused &&
        Refusal?.Retained == true &&
        PassageCountAfterAdmissibilityReview == PriorPassageCount &&
        !AdmissibleForEnactmentReview &&
        SeparateEnactmentBoundaryRequired &&
        !AdmissibilityExecutes &&
        !StewardAcceptanceMovesRuntime &&
        !RuntimeActionAllowed &&
        !LispEvaluationAllowed &&
        !NewPacketEmitted &&
        !ReceiptsReplayed &&
        !ContinuityAdmitted &&
        !AuthorityGranted &&
        ActivationRefused;
}

public sealed class DefaultStewardActionAdmissibilityBoundaryValidator
{
    private static readonly StewardActionAdmissibilityNonExecutionBoundary NonExecutionBoundary = new(
        AdmissibilityMayExecute: false,
        StewardAcceptanceMayMoveRuntime: false,
        AdmissibilityMayGrantAuthority: false,
        AdmissibilityMayAdmitContinuity: false,
        AdmissibilityMayEmitPacket: false,
        AdmissibilityMayEvaluateLisp: false,
        AdmissibilityMayReplayReceipt: false,
        AdmissibilityMayIncrementPassage: false,
        SeparateEnactmentBoundaryRequired: true,
        BoundaryLaw: "Admissibility is not execution. Steward acceptance is not runtime motion. An admissible action remains sealed until a separate enactment boundary exists.");

    public StewardActionAdmissibilityReceipt Declare(
        StewardActionAdmissibilityRequest request,
        DateTimeOffset timestampUtc)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.SourceMethodReadinessReceipt is null ||
            !request.SourceMethodReadinessReceipt.IsColdMethodReadiness ||
            !request.SourceMethodReadinessReceipt.MethodReadyForStewardReview)
        {
            return Refuse(
                request,
                "steward-action-admissibility-source-method-readiness-missing",
                "Steward action admissibility refused because a cold method-readiness receipt with a ready method is required before admissibility review.",
                timestampUtc);
        }

        if (!request.ScopeBoundary.IsColdScope)
        {
            return Refuse(
                request,
                "steward-action-admissibility-scope-promotional",
                "Steward action admissibility refused because scope must require a separate enactment boundary and refuse execution, runtime motion, authority, continuity, activation, Lisp evaluation, packet emission, replay, and passage increment.",
                timestampUtc);
        }

        if (request.Decisions.Any(static decision => !decision.IsColdDecision))
        {
            return Refuse(
                request,
                "steward-action-admissibility-decision-invalid",
                "Steward action admissibility decision refused because every decision must preserve Steward custody, witness, telemetry, ceiling, revocation, and loss while refusing execution, authority, continuity, activation, packet emission, and Lisp evaluation.",
                timestampUtc);
        }

        var methodHandles = request.SourceMethodReadinessReceipt.Methods
            .Select(static method => method.MethodHandle)
            .ToHashSet(StringComparer.Ordinal);
        var actionHandles = request.SourceMethodReadinessReceipt.Methods
            .Select(static method => method.ActionHandle)
            .ToHashSet(StringComparer.Ordinal);
        var decisionHandles = request.Decisions
            .Select(static decision => decision.DecisionHandle)
            .ToHashSet(StringComparer.Ordinal);

        if (decisionHandles.Count != request.Decisions.Count)
        {
            return Refuse(
                request,
                "steward-action-admissibility-duplicate-decision-handle",
                "Steward action admissibility refused because duplicate decision handles would collapse admissibility lineage.",
                timestampUtc);
        }

        if (request.Decisions.Any(decision =>
                !methodHandles.Contains(decision.MethodHandle) ||
                !actionHandles.Contains(decision.ActionHandle)))
        {
            return Refuse(
                request,
                "steward-action-admissibility-method-lineage-missing",
                "Steward action admissibility refused because every decision must bind to a ready method and typed action lineage.",
                timestampUtc);
        }

        if (request.PredicateResults.Any(result =>
                !result.IsColdPredicateResult ||
                !methodHandles.Contains(result.MethodHandle) ||
                !actionHandles.Contains(result.ActionHandle)))
        {
            return Refuse(
                request,
                "steward-action-admissibility-predicate-invalid",
                "Steward admissibility predicate result refused because predicates may support admissibility only and may not become warrant, execution, packet emission, Lisp evaluation, replay, passage, continuity, or authority.",
                timestampUtc);
        }

        if (request.Decisions.Count > 0 &&
            request.Decisions.Any(decision =>
                !request.PredicateResults.Any(result =>
                    result.MethodHandle == decision.MethodHandle &&
                    result.ActionHandle == decision.ActionHandle)))
        {
            return Refuse(
                request,
                "steward-action-admissibility-predicate-coverage-missing",
                "Steward action admissibility refused because every admissibility decision requires a witnessed predicate result for the same method and action.",
                timestampUtc);
        }

        if (methodHandles.Count > 0 &&
            methodHandles.Any(methodHandle => !request.Decisions.Any(decision => decision.MethodHandle == methodHandle)))
        {
            return Refuse(
                request,
                "steward-action-admissibility-method-coverage-missing",
                "Steward action admissibility refused because every ready method requires an admissibility decision before the source can be called admissible.",
                timestampUtc);
        }

        var disposition = request.Decisions.Count == 0
            ? StewardActionAdmissibilityDisposition.EmptyReviewCold
            : StewardActionAdmissibilityDisposition.AdmissibleForEnactmentReviewCold;
        var outcomeCode = disposition == StewardActionAdmissibilityDisposition.EmptyReviewCold
            ? "steward-action-admissibility-empty-review-only"
            : "steward-action-admissibility-for-enactment-review-cold";

        return CreateReceipt(
            request,
            disposition,
            outcomeCode,
            "Steward action admissibility was declared for enactment review only. Admissibility does not execute, authorize runtime motion, admit continuity, evaluate Lisp, emit packets, replay receipts, increment passage, or activate.",
            refusal: null,
            timestampUtc);
    }

    private static StewardActionAdmissibilityReceipt Refuse(
        StewardActionAdmissibilityRequest request,
        string outcomeCode,
        string governanceTrace,
        DateTimeOffset timestampUtc) =>
        CreateReceipt(
            request,
            StewardActionAdmissibilityDisposition.Refused,
            outcomeCode,
            governanceTrace,
            new StewardActionAdmissibilityRefusalReceipt(
                ReceiptHandle: $"urn:san:steward-action-admissibility-refusal:{ShortHash(SourceHandle(request), outcomeCode)}",
                RefusalCode: outcomeCode,
                GovernanceTrace: governanceTrace,
                Retained: true),
            timestampUtc);

    private static StewardActionAdmissibilityReceipt CreateReceipt(
        StewardActionAdmissibilityRequest request,
        StewardActionAdmissibilityDisposition disposition,
        string outcomeCode,
        string governanceTrace,
        StewardActionAdmissibilityRefusalReceipt? refusal,
        DateTimeOffset timestampUtc) =>
        new(
            ReceiptHandle: $"urn:san:steward-action-admissibility:{(refusal is null ? "review" : "refused")}:{ShortHash(SourceHandle(request), outcomeCode, request.Decisions.Count.ToString(System.Globalization.CultureInfo.InvariantCulture), timestampUtc.UtcTicks.ToString(System.Globalization.CultureInfo.InvariantCulture))}",
            Disposition: disposition,
            OutcomeCode: outcomeCode,
            GovernanceTrace: governanceTrace,
            SourceMethodReadinessReceiptHandle: SourceHandle(request),
            Decisions: refusal is null ? request.Decisions.ToArray() : [],
            PredicateResults: refusal is null ? request.PredicateResults.ToArray() : [],
            ScopeBoundary: request.ScopeBoundary,
            NonExecutionBoundary: NonExecutionBoundary,
            Refusal: refusal,
            PriorPassageCount: request.PriorPassageCount,
            PassageCountAfterAdmissibilityReview: request.PriorPassageCount,
            ReviewOnly: true,
            AdmissibleForEnactmentReview: refusal is null && request.Decisions.Count > 0,
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
            TimestampUtc: timestampUtc);

    private static string SourceHandle(StewardActionAdmissibilityRequest request) =>
        request.SourceMethodReadinessReceipt?.ReceiptHandle ?? "missing-action-method-readiness-source";

    private static string ShortHash(params string[] parts)
    {
        var payload = string.Join("|", parts);
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(payload));
        return Convert.ToHexString(bytes)[..16].ToLowerInvariant();
    }
}
