using San.Common;

namespace SLI.Runtime;

public interface IGovernedReturnReceiptReplayPolicy
{
    GovernedReturnReceiptReplayEvaluation Evaluate(
        GovernedReturnReceiptReplayRequest request,
        DateTimeOffset timestampUtc);
}

public sealed class DefaultGovernedReturnReceiptReplayPolicy : IGovernedReturnReceiptReplayPolicy
{
    public GovernedReturnReceiptReplayEvaluation Evaluate(
        GovernedReturnReceiptReplayRequest request,
        DateTimeOffset timestampUtc)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.ReturnReceipt is null)
        {
            return CreateEvaluation(
                request,
                GovernedReturnReplayReviewDisposition.ReviewWithheld,
                "governed-return-receipt-required",
                "SW-06 withheld replay review because no governed return receipt was presented.",
                timestampUtc);
        }

        if (HasRequestedMotion(request))
        {
            return CreateEvaluation(
                request,
                GovernedReturnReplayReviewDisposition.ReviewRefused,
                "runtime-landing-or-execution-requested",
                "SW-06 refused replay review because requested motion attempted runtime landing, execution, persistence, authority widening, or GEL promotion.",
                timestampUtc);
        }

        if (request.RecompositionCandidate is { } candidate)
        {
            if (candidate.Disposition is not RecompositionCandidateDisposition.CandidateOnly and not RecompositionCandidateDisposition.Withheld)
            {
                return CreateEvaluation(
                    request,
                    GovernedReturnReplayReviewDisposition.ReviewRefused,
                    "recomposition-disposition-blocked",
                    "SW-06 refused replay review because recomposition posture was not candidate-only or withheld.",
                    timestampUtc);
            }

            if (candidate.MaterializationEligibility != MaterializationEligibility.No)
            {
                return CreateEvaluation(
                    request,
                    GovernedReturnReplayReviewDisposition.ReviewRefused,
                    "recomposition-materialization-blocked",
                    "SW-06 refused replay review because recomposition carried materialization eligibility.",
                    timestampUtc);
            }

            if (candidate.PersistenceEligibility != PersistenceEligibility.Never)
            {
                return CreateEvaluation(
                    request,
                    GovernedReturnReplayReviewDisposition.ReviewRefused,
                    "recomposition-persistence-blocked",
                    "SW-06 refused replay review because recomposition carried persistence eligibility.",
                    timestampUtc);
            }

            if (!candidate.RequiresMembraneReentry)
            {
                return CreateEvaluation(
                    request,
                    GovernedReturnReplayReviewDisposition.ReviewRefused,
                    "membrane-reentry-required",
                    "SW-06 refused replay review because recomposition attempted to bypass membrane re-entry.",
                    timestampUtc);
            }
        }

        if (request.FieldQueryResult is { } fieldQueryResult &&
            (!fieldQueryResult.TensionSummary.PassportTruthPreserved ||
             !fieldQueryResult.TensionSummary.AuthorityCeilingPreserved ||
             !fieldQueryResult.TensionSummary.MembraneReentryRequired ||
             !fieldQueryResult.MembraneReentryRequired))
        {
            return CreateEvaluation(
                request,
                GovernedReturnReplayReviewDisposition.ReviewRefused,
                "field-query-passport-or-authority-drift-blocked",
                "SW-06 refused replay review because field query posture did not preserve passport truth, authority ceiling, and membrane re-entry.",
                timestampUtc);
        }

        if (request.EscalationTransitionDecision?.Disposition == EscalationTransitionDisposition.Denied)
        {
            return CreateEvaluation(
                request,
                GovernedReturnReplayReviewDisposition.ReviewRefused,
                "escalation-transition-denied",
                "SW-06 refused replay review because the escalation transition was denied by bounded state grammar.",
                timestampUtc);
        }

        if (RequiresHitlWitness(request.EscalationPacket) &&
            request.HitlWitnessToken?.AuthorizedExit != HitlHoldExitRoute.GovernedReturn)
        {
            return CreateEvaluation(
                request,
                GovernedReturnReplayReviewDisposition.ReviewWithheld,
                "hitl-governed-return-witness-token-required",
                "SW-06 withheld replay review because HITL hold release requires a witness token authorizing GovernedReturn.",
                timestampUtc);
        }

        return CreateEvaluation(
            request,
            GovernedReturnReplayReviewDisposition.ReviewAdmitted,
            "governed-return-replay-review-admitted",
            "SW-06 admitted governed return replay review as representation only; no runtime landing, replay execution, persistence, identity, action, or authority widening is allowed.",
            timestampUtc);
    }

    private static bool HasRequestedMotion(GovernedReturnReceiptReplayRequest request) =>
        request.RuntimeLandingRequested ||
        request.ReplayExecutionRequested ||
        request.DbWriteRequested ||
        request.EcStartRequested ||
        request.RuntimeIdentityRequested ||
        request.RuntimeActionRequested ||
        request.RecompositionExecutionRequested ||
        request.AuthorityWideningRequested ||
        request.GelPromotionRequested;

    private static bool RequiresHitlWitness(SliEscalationPacket? escalationPacket) =>
        escalationPacket?.State == SliEscalationState.HitlHold ||
        escalationPacket?.HitlRequired == true;

    private static GovernedReturnReceiptReplayEvaluation CreateEvaluation(
        GovernedReturnReceiptReplayRequest request,
        GovernedReturnReplayReviewDisposition disposition,
        string outcomeCode,
        string governanceTrace,
        DateTimeOffset timestampUtc) =>
        new(
            EvaluationHandle: $"sw06-governed-return-replay-review://{Math.Abs(HashCode.Combine(request.ReturnReceipt?.ReceiptId, request.RecompositionCandidate?.CandidateId, outcomeCode)):x}",
            Disposition: disposition,
            OutcomeCode: outcomeCode,
            GovernanceTrace: governanceTrace,
            ReturnReceiptId: request.ReturnReceipt?.ReceiptId,
            ReturnFamily: request.ReturnReceipt?.ReturnFamily,
            CandidateId: request.RecompositionCandidate?.CandidateId,
            FieldQueryId: request.FieldQueryResult?.Query.QueryId,
            EscalationTraceId: request.EscalationPacket?.TraceId,
            HitlWitnessTokenId: request.HitlWitnessToken?.TokenId,
            ReviewOnly: disposition == GovernedReturnReplayReviewDisposition.ReviewAdmitted,
            RuntimeLandingAllowed: false,
            ReplayExecutionAllowed: false,
            RuntimeIdentityEmitted: false,
            RuntimeActionExecuted: false,
            DbWriteAllowed: false,
            EcStartAllowed: false,
            RecompositionExecutionAllowed: false,
            AuthorityWideningAllowed: false,
            GelPromotionAllowed: false,
            TimestampUtc: timestampUtc);
}

public sealed record GovernedReturnReceiptReplayRequest(
    GovernedReturnReceipt? ReturnReceipt,
    FieldQueryResult? FieldQueryResult,
    RecompositionCandidate? RecompositionCandidate,
    RecompositionCandidateEvaluationDecision? RecompositionEvaluationDecision,
    SliEscalationPacket? EscalationPacket,
    SliEscalationTransitionDecision? EscalationTransitionDecision,
    HitlHoldWitnessToken? HitlWitnessToken,
    bool RuntimeLandingRequested,
    bool ReplayExecutionRequested,
    bool DbWriteRequested,
    bool EcStartRequested,
    bool RuntimeIdentityRequested,
    bool RuntimeActionRequested,
    bool RecompositionExecutionRequested,
    bool AuthorityWideningRequested,
    bool GelPromotionRequested);

public sealed record GovernedReturnReceiptReplayEvaluation(
    string EvaluationHandle,
    GovernedReturnReplayReviewDisposition Disposition,
    string OutcomeCode,
    string GovernanceTrace,
    string? ReturnReceiptId,
    GovernedReturnReceiptFamily? ReturnFamily,
    string? CandidateId,
    string? FieldQueryId,
    string? EscalationTraceId,
    string? HitlWitnessTokenId,
    bool ReviewOnly,
    bool RuntimeLandingAllowed,
    bool ReplayExecutionAllowed,
    bool RuntimeIdentityEmitted,
    bool RuntimeActionExecuted,
    bool DbWriteAllowed,
    bool EcStartAllowed,
    bool RecompositionExecutionAllowed,
    bool AuthorityWideningAllowed,
    bool GelPromotionAllowed,
    DateTimeOffset TimestampUtc);

public enum GovernedReturnReplayReviewDisposition
{
    ReviewWithheld = 0,
    ReviewRefused = 1,
    ReviewAdmitted = 2
}
