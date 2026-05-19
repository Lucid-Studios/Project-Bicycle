using System.Security.Cryptography;
using System.Text;

namespace San.Common;

public enum EnactmentBoundaryReadinessDisposition
{
    ReadyForEnactmentBoundaryReviewCold = 0,
    EmptyReviewCold = 1,
    Refused = 2
}

public sealed record EnactmentBoundaryReadinessCandidate(
    string ReadinessHandle,
    string SourcePacketHandle,
    string SourceStewardRouteHandle,
    string DutyStation,
    string WorkSurface,
    string IntendedWork,
    string MethodCode,
    string AuthorityCeiling,
    string LocalEffectCeiling,
    string ReversibilityProofHandle,
    string DryRunPlanHandle,
    string CustodyOwner,
    string WitnessHandle,
    string TelemetryRoute,
    string StewardReviewHandle,
    string RevocationPath,
    string RepairPath,
    string LossCondition,
    bool ReviewOnly,
    bool ApproachOnly,
    bool LocalOnly,
    bool ReversibleOnly,
    bool RequiresStewardReview,
    bool RequiresDryRunBeforeExecution,
    bool RequiresSeparateActionHarness,
    bool ReadinessBecomesWarrant,
    bool ReadinessBecomesAdmission,
    bool ReadinessGrantsAuthority,
    bool ReadinessAdmitsContinuity,
    bool ReadinessAuthorizesAction,
    bool ReadinessExecutesAction,
    bool ApproachMovesRuntime,
    bool LocalityAuthorizesAction,
    bool ReversibilityAuthorizesAction,
    bool StewardReviewMovesRuntime,
    bool ReadinessEvaluatesLisp,
    bool ReadinessEmitsMembranePacket,
    bool ReadinessReplaysReceipt,
    bool ReadinessIncrementsPassage,
    bool ReadinessActivates)
{
    public bool IsColdReadinessCandidate =>
        !string.IsNullOrWhiteSpace(ReadinessHandle) &&
        !string.IsNullOrWhiteSpace(SourcePacketHandle) &&
        !string.IsNullOrWhiteSpace(SourceStewardRouteHandle) &&
        !string.IsNullOrWhiteSpace(DutyStation) &&
        !string.IsNullOrWhiteSpace(WorkSurface) &&
        !string.IsNullOrWhiteSpace(IntendedWork) &&
        !string.IsNullOrWhiteSpace(MethodCode) &&
        !string.IsNullOrWhiteSpace(AuthorityCeiling) &&
        !string.IsNullOrWhiteSpace(LocalEffectCeiling) &&
        !string.IsNullOrWhiteSpace(ReversibilityProofHandle) &&
        !string.IsNullOrWhiteSpace(DryRunPlanHandle) &&
        !string.IsNullOrWhiteSpace(CustodyOwner) &&
        !string.IsNullOrWhiteSpace(WitnessHandle) &&
        !string.IsNullOrWhiteSpace(TelemetryRoute) &&
        !string.IsNullOrWhiteSpace(StewardReviewHandle) &&
        !string.IsNullOrWhiteSpace(RevocationPath) &&
        !string.IsNullOrWhiteSpace(RepairPath) &&
        !string.IsNullOrWhiteSpace(LossCondition) &&
        ReviewOnly &&
        ApproachOnly &&
        LocalOnly &&
        ReversibleOnly &&
        RequiresStewardReview &&
        RequiresDryRunBeforeExecution &&
        RequiresSeparateActionHarness &&
        !ReadinessBecomesWarrant &&
        !ReadinessBecomesAdmission &&
        !ReadinessGrantsAuthority &&
        !ReadinessAdmitsContinuity &&
        !ReadinessAuthorizesAction &&
        !ReadinessExecutesAction &&
        !ApproachMovesRuntime &&
        !LocalityAuthorizesAction &&
        !ReversibilityAuthorizesAction &&
        !StewardReviewMovesRuntime &&
        !ReadinessEvaluatesLisp &&
        !ReadinessEmitsMembranePacket &&
        !ReadinessReplaysReceipt &&
        !ReadinessIncrementsPassage &&
        !ReadinessActivates;
}

public sealed record EnactmentBoundaryStewardReviewRoute(
    string ReviewRouteHandle,
    string ReadinessHandle,
    string SourcePacketHandle,
    string StewardSurface,
    string CustodyOwner,
    string EvidenceHandle,
    string WitnessHandle,
    string TelemetryRoute,
    string ReturnPathHandle,
    bool ReviewOnly,
    bool PreservesReadinessLineage,
    bool PreservesPacketLineage,
    bool PreservesStewardRouteLineage,
    bool RoutesToStewardEnactmentReview,
    bool RequiresCooling,
    bool RequiresSeparateActionHarness,
    bool RouteAuthorizesAction,
    bool RouteExecutesAction,
    bool RouteMovesRuntime,
    bool RouteGrantsAuthority,
    bool RouteAdmitsContinuity,
    bool RouteEvaluatesLisp,
    bool RouteEmitsMembranePacket,
    bool RouteReplaysReceipt,
    bool RouteIncrementsPassage,
    bool RouteActivates)
{
    public bool IsColdStewardReviewRoute =>
        !string.IsNullOrWhiteSpace(ReviewRouteHandle) &&
        !string.IsNullOrWhiteSpace(ReadinessHandle) &&
        !string.IsNullOrWhiteSpace(SourcePacketHandle) &&
        !string.IsNullOrWhiteSpace(StewardSurface) &&
        !string.IsNullOrWhiteSpace(CustodyOwner) &&
        !string.IsNullOrWhiteSpace(EvidenceHandle) &&
        !string.IsNullOrWhiteSpace(WitnessHandle) &&
        !string.IsNullOrWhiteSpace(TelemetryRoute) &&
        !string.IsNullOrWhiteSpace(ReturnPathHandle) &&
        ReviewOnly &&
        PreservesReadinessLineage &&
        PreservesPacketLineage &&
        PreservesStewardRouteLineage &&
        RoutesToStewardEnactmentReview &&
        RequiresCooling &&
        RequiresSeparateActionHarness &&
        !RouteAuthorizesAction &&
        !RouteExecutesAction &&
        !RouteMovesRuntime &&
        !RouteGrantsAuthority &&
        !RouteAdmitsContinuity &&
        !RouteEvaluatesLisp &&
        !RouteEmitsMembranePacket &&
        !RouteReplaysReceipt &&
        !RouteIncrementsPassage &&
        !RouteActivates;
}

public sealed record EnactmentBoundaryReadinessScopeBoundary(
    string BoundaryCode,
    bool Present,
    bool ReviewOnly,
    bool AllowsReadinessDeclaration,
    bool RequiresScopedWorkPacketReceipt,
    bool RequiresStewardRoute,
    bool RequiresDutyStation,
    bool RequiresWorkSurface,
    bool RequiresIntendedWork,
    bool RequiresMethodCode,
    bool RequiresAuthorityCeiling,
    bool RequiresLocalEffectCeiling,
    bool RequiresReversibilityProof,
    bool RequiresDryRunPlan,
    bool RequiresCustody,
    bool RequiresWitness,
    bool RequiresTelemetryRoute,
    bool RequiresStewardReview,
    bool RequiresRevocationPath,
    bool RequiresRepairPath,
    bool RequiresLossCondition,
    bool RequiresSeparateActionHarness,
    bool AllowsReadinessAsWarrant,
    bool AllowsReadinessAsAdmission,
    bool AllowsReadinessAsAuthority,
    bool AllowsReadinessAsContinuity,
    bool AllowsActionAuthorization,
    bool AllowsExecution,
    bool AllowsRuntimeMotion,
    bool AllowsLispEvaluation,
    bool AllowsMembranePacketEmission,
    bool AllowsReceiptReplay,
    bool AllowsPassageIncrement,
    bool AllowsActivation)
{
    public bool IsColdScope =>
        !string.IsNullOrWhiteSpace(BoundaryCode) &&
        Present &&
        ReviewOnly &&
        AllowsReadinessDeclaration &&
        RequiresScopedWorkPacketReceipt &&
        RequiresStewardRoute &&
        RequiresDutyStation &&
        RequiresWorkSurface &&
        RequiresIntendedWork &&
        RequiresMethodCode &&
        RequiresAuthorityCeiling &&
        RequiresLocalEffectCeiling &&
        RequiresReversibilityProof &&
        RequiresDryRunPlan &&
        RequiresCustody &&
        RequiresWitness &&
        RequiresTelemetryRoute &&
        RequiresStewardReview &&
        RequiresRevocationPath &&
        RequiresRepairPath &&
        RequiresLossCondition &&
        RequiresSeparateActionHarness &&
        !AllowsReadinessAsWarrant &&
        !AllowsReadinessAsAdmission &&
        !AllowsReadinessAsAuthority &&
        !AllowsReadinessAsContinuity &&
        !AllowsActionAuthorization &&
        !AllowsExecution &&
        !AllowsRuntimeMotion &&
        !AllowsLispEvaluation &&
        !AllowsMembranePacketEmission &&
        !AllowsReceiptReplay &&
        !AllowsPassageIncrement &&
        !AllowsActivation;
}

public sealed record EnactmentBoundaryReadinessNonExecutionBoundary(
    string BoundaryLaw,
    bool ReadinessMayBecomeWarrant,
    bool ReadinessMayBecomeAdmission,
    bool ReadinessMayAuthorize,
    bool ReadinessMayExecute,
    bool ReadinessMayMoveRuntime,
    bool ReadinessMayGrantAuthority,
    bool ReadinessMayAdmitContinuity,
    bool ApproachMayAuthorize,
    bool LocalityMayAuthorize,
    bool ReversibilityMayAuthorize,
    bool StewardReviewMayMoveRuntime,
    bool DryRunPlanMayExecute,
    bool SeparateActionHarnessRequired,
    bool ReadinessMayEvaluateLisp,
    bool ReadinessMayEmitMembranePacket,
    bool ReadinessMayReplayReceipt,
    bool ReadinessMayIncrementPassage,
    bool ReadinessMayActivate)
{
    public bool IsColdNonExecutionBoundary =>
        !string.IsNullOrWhiteSpace(BoundaryLaw) &&
        !ReadinessMayBecomeWarrant &&
        !ReadinessMayBecomeAdmission &&
        !ReadinessMayAuthorize &&
        !ReadinessMayExecute &&
        !ReadinessMayMoveRuntime &&
        !ReadinessMayGrantAuthority &&
        !ReadinessMayAdmitContinuity &&
        !ApproachMayAuthorize &&
        !LocalityMayAuthorize &&
        !ReversibilityMayAuthorize &&
        !StewardReviewMayMoveRuntime &&
        !DryRunPlanMayExecute &&
        SeparateActionHarnessRequired &&
        !ReadinessMayEvaluateLisp &&
        !ReadinessMayEmitMembranePacket &&
        !ReadinessMayReplayReceipt &&
        !ReadinessMayIncrementPassage &&
        !ReadinessMayActivate;
}

public sealed record EnactmentBoundaryReadinessRefusalReceipt(
    string ReceiptHandle,
    string RefusalCode,
    string GovernanceTrace,
    bool Retained);

public sealed record EnactmentBoundaryReadinessRequest(
    ScopedWorkPacketFormationReceipt? SourcePacketFormationReceipt,
    IReadOnlyList<EnactmentBoundaryReadinessCandidate> Candidates,
    IReadOnlyList<EnactmentBoundaryStewardReviewRoute> StewardReviewRoutes,
    EnactmentBoundaryReadinessScopeBoundary ScopeBoundary,
    EnactmentBoundaryReadinessNonExecutionBoundary NonExecutionBoundary,
    int PriorPassageCount);

public sealed record EnactmentBoundaryReadinessReceipt(
    string ReceiptHandle,
    EnactmentBoundaryReadinessDisposition Disposition,
    string OutcomeCode,
    string GovernanceTrace,
    string SourcePacketFormationReceiptHandle,
    IReadOnlyList<EnactmentBoundaryReadinessCandidate> Candidates,
    IReadOnlyList<EnactmentBoundaryStewardReviewRoute> StewardReviewRoutes,
    EnactmentBoundaryReadinessScopeBoundary ScopeBoundary,
    EnactmentBoundaryReadinessNonExecutionBoundary NonExecutionBoundary,
    EnactmentBoundaryReadinessRefusalReceipt? Refusal,
    int PriorPassageCount,
    int PassageCountAfterReadiness,
    int RetainedCandidateCount,
    bool ReviewOnly,
    bool ApproachOnly,
    bool ReadyForEnactmentBoundaryReview,
    bool ReadinessBecameWarrant,
    bool ReadinessBecameAdmission,
    bool ReadinessGrantedAuthority,
    bool ReadinessAdmittedContinuity,
    bool ReadinessAuthorizedAction,
    bool ReadinessExecutedAction,
    bool ApproachAuthorizedAction,
    bool LocalityAuthorizedAction,
    bool ReversibilityAuthorizedAction,
    bool StewardReviewMovedRuntime,
    bool DryRunPlanExecuted,
    bool RuntimeActionAllowed,
    bool LispEvaluationAllowed,
    bool NewMembranePacketEmitted,
    bool ReceiptsReplayed,
    bool ContinuityAdmitted,
    bool AuthorityGranted,
    bool ActivationRefused,
    DateTimeOffset TimestampUtc)
{
    public bool IsColdEnactmentBoundaryReadiness =>
        (Disposition is EnactmentBoundaryReadinessDisposition.ReadyForEnactmentBoundaryReviewCold or
            EnactmentBoundaryReadinessDisposition.EmptyReviewCold) &&
        Refusal is null &&
        ReviewOnly &&
        ApproachOnly &&
        PassageCountAfterReadiness == PriorPassageCount &&
        RetainedCandidateCount == Candidates.Count &&
        !ReadinessBecameWarrant &&
        !ReadinessBecameAdmission &&
        !ReadinessGrantedAuthority &&
        !ReadinessAdmittedContinuity &&
        !ReadinessAuthorizedAction &&
        !ReadinessExecutedAction &&
        !ApproachAuthorizedAction &&
        !LocalityAuthorizedAction &&
        !ReversibilityAuthorizedAction &&
        !StewardReviewMovedRuntime &&
        !DryRunPlanExecuted &&
        !RuntimeActionAllowed &&
        !LispEvaluationAllowed &&
        !NewMembranePacketEmitted &&
        !ReceiptsReplayed &&
        !ContinuityAdmitted &&
        !AuthorityGranted &&
        ActivationRefused &&
        ScopeBoundary.IsColdScope &&
        NonExecutionBoundary.IsColdNonExecutionBoundary;

    public bool IsRetainedEnactmentBoundaryReadinessRefusal =>
        Disposition == EnactmentBoundaryReadinessDisposition.Refused &&
        Refusal?.Retained == true &&
        PassageCountAfterReadiness == PriorPassageCount &&
        RetainedCandidateCount == 0 &&
        !ReadyForEnactmentBoundaryReview &&
        !ReadinessBecameWarrant &&
        !ReadinessBecameAdmission &&
        !ReadinessGrantedAuthority &&
        !ReadinessAdmittedContinuity &&
        !ReadinessAuthorizedAction &&
        !ReadinessExecutedAction &&
        !ApproachAuthorizedAction &&
        !LocalityAuthorizedAction &&
        !ReversibilityAuthorizedAction &&
        !StewardReviewMovedRuntime &&
        !DryRunPlanExecuted &&
        !RuntimeActionAllowed &&
        !LispEvaluationAllowed &&
        !NewMembranePacketEmitted &&
        !ReceiptsReplayed &&
        !ContinuityAdmitted &&
        !AuthorityGranted &&
        ActivationRefused;
}

public sealed class DefaultEnactmentBoundaryReadinessBoundaryValidator
{
    public EnactmentBoundaryReadinessReceipt Declare(
        EnactmentBoundaryReadinessRequest request,
        DateTimeOffset timestampUtc)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.SourcePacketFormationReceipt is null ||
            !request.SourcePacketFormationReceipt.IsColdScopedWorkPacketFormation)
        {
            return Refuse(
                request,
                "enactment-boundary-readiness-source-packet-missing",
                "Enactment boundary readiness refused because a cold scoped work packet formation receipt is required before readiness may approach review.",
                timestampUtc);
        }

        if (request.ScopeBoundary is null ||
            !request.ScopeBoundary.Present ||
            string.IsNullOrWhiteSpace(request.ScopeBoundary.BoundaryCode))
        {
            return Refuse(
                request,
                "enactment-boundary-readiness-scope-missing",
                "Enactment boundary readiness refused because a review-only readiness scope boundary is required.",
                timestampUtc);
        }

        if (!request.ScopeBoundary.IsColdScope)
        {
            return Refuse(
                request,
                "enactment-boundary-readiness-scope-promotional",
                "Enactment boundary readiness refused because readiness may approach enactment boundary review only while requiring scoped packet lineage, Steward route, duty station, work surface, method, authority ceiling, local effect ceiling, reversibility proof, dry-run plan, custody, witness, telemetry, Steward review, revocation, repair, loss, and separate action harness, and while refusing warrant, admission, authority, continuity, action authorization, execution, runtime motion, Lisp evaluation, membrane packet emission, replay, passage, and activation.",
                timestampUtc);
        }

        if (request.NonExecutionBoundary is null ||
            !request.NonExecutionBoundary.IsColdNonExecutionBoundary)
        {
            return Refuse(
                request,
                "enactment-boundary-readiness-non-execution-invalid",
                "Enactment boundary readiness refused because non-execution law must prevent readiness, approach, locality, reversibility, Steward review, and dry-run planning from becoming authorization, execution, authority, continuity, runtime motion, Lisp evaluation, membrane packet emission, replay, passage, or activation.",
                timestampUtc);
        }

        if (request.Candidates.Any(static candidate => !candidate.IsColdReadinessCandidate))
        {
            return Refuse(
                request,
                "enactment-boundary-readiness-candidate-invalid",
                "Enactment boundary readiness refused because every candidate must preserve packet and Steward route lineage while declaring local effect ceiling, reversibility proof, dry-run plan, custody, witness, telemetry, revocation, repair, and loss without warrant, admission, authority, continuity, action authorization, execution, runtime motion, Lisp evaluation, membrane packet emission, replay, passage, or activation.",
                timestampUtc);
        }

        if (HasDuplicate(request.Candidates.Select(static candidate => candidate.ReadinessHandle)))
        {
            return Refuse(
                request,
                "enactment-boundary-readiness-duplicate-candidate-handle",
                "Enactment boundary readiness refused because duplicate readiness handles would collapse readiness lineage.",
                timestampUtc);
        }

        var sourcePackets = request.SourcePacketFormationReceipt.Packets
            .ToDictionary(static packet => packet.PacketHandle, StringComparer.Ordinal);
        var sourceRoutes = request.SourcePacketFormationReceipt.StewardRoutes
            .ToDictionary(static route => route.RouteHandle, StringComparer.Ordinal);

        if (request.Candidates.Any(candidate =>
                !sourcePackets.TryGetValue(candidate.SourcePacketHandle, out var packet) ||
                !sourceRoutes.TryGetValue(candidate.SourceStewardRouteHandle, out var route) ||
                !string.Equals(route.PacketHandle, packet.PacketHandle, StringComparison.Ordinal) ||
                !string.Equals(candidate.DutyStation, packet.DutyStation, StringComparison.Ordinal) ||
                !string.Equals(candidate.WorkSurface, packet.WorkSurface, StringComparison.Ordinal) ||
                !string.Equals(candidate.IntendedWork, packet.IntendedWork, StringComparison.Ordinal) ||
                !string.Equals(candidate.MethodCode, packet.MethodCode, StringComparison.Ordinal) ||
                !string.Equals(candidate.AuthorityCeiling, packet.AuthorityCeiling, StringComparison.Ordinal) ||
                !string.Equals(candidate.CustodyOwner, packet.CustodyOwner, StringComparison.Ordinal) ||
                !string.Equals(candidate.WitnessHandle, packet.WitnessHandle, StringComparison.Ordinal) ||
                !string.Equals(candidate.TelemetryRoute, packet.TelemetryRoute, StringComparison.Ordinal) ||
                !string.Equals(candidate.RevocationPath, packet.RevocationPath, StringComparison.Ordinal) ||
                !string.Equals(candidate.RepairPath, packet.RepairPath, StringComparison.Ordinal) ||
                !string.Equals(candidate.LossCondition, packet.LossCondition, StringComparison.Ordinal)))
        {
            return Refuse(
                request,
                "enactment-boundary-readiness-lineage-invalid",
                "Enactment boundary readiness refused because every readiness candidate must bind to a retained scoped work packet and its Steward route while preserving declared work packet lineage.",
                timestampUtc);
        }

        var candidateHandles = request.Candidates
            .Select(static candidate => candidate.ReadinessHandle)
            .ToHashSet(StringComparer.Ordinal);
        var candidatePacketHandles = request.Candidates
            .Select(static candidate => candidate.SourcePacketHandle)
            .ToHashSet(StringComparer.Ordinal);

        if (request.StewardReviewRoutes.Any(route =>
                !route.IsColdStewardReviewRoute ||
                !candidateHandles.Contains(route.ReadinessHandle) ||
                !candidatePacketHandles.Contains(route.SourcePacketHandle)))
        {
            return Refuse(
                request,
                "enactment-boundary-readiness-steward-route-invalid",
                "Enactment boundary readiness Steward review route refused because routes may preserve readiness, packet, and Steward route lineage for enactment boundary review only and may not authorize, execute, move runtime, grant authority, admit continuity, evaluate Lisp, emit membrane packets, replay receipts, increment passage, or activate.",
                timestampUtc);
        }

        if (HasDuplicate(request.StewardReviewRoutes.Select(static route => route.ReviewRouteHandle)))
        {
            return Refuse(
                request,
                "enactment-boundary-readiness-duplicate-route-handle",
                "Enactment boundary readiness refused because duplicate Steward review route handles would collapse review-route lineage.",
                timestampUtc);
        }

        if (request.Candidates.Count > 0 &&
            request.Candidates.Any(candidate => !request.StewardReviewRoutes.Any(route => route.ReadinessHandle == candidate.ReadinessHandle)))
        {
            return Refuse(
                request,
                "enactment-boundary-readiness-steward-route-missing",
                "Enactment boundary readiness refused because every readiness candidate requires a Steward enactment review route before it can be retained for review.",
                timestampUtc);
        }

        var disposition = request.Candidates.Count == 0
            ? EnactmentBoundaryReadinessDisposition.EmptyReviewCold
            : EnactmentBoundaryReadinessDisposition.ReadyForEnactmentBoundaryReviewCold;
        var outcomeCode = disposition == EnactmentBoundaryReadinessDisposition.EmptyReviewCold
            ? "enactment-boundary-readiness-empty-review-only"
            : "enactment-boundary-readiness-ready-review-only";
        var governanceTrace = disposition == EnactmentBoundaryReadinessDisposition.EmptyReviewCold
            ? "Enactment boundary readiness found no scoped packet candidates. Empty review preserves enactment readiness boundary without execution, warrant, authority, continuity, Lisp evaluation, membrane packet emission, replay, passage, or activation."
            : "Scoped work packets approached enactment boundary readiness review while refusing readiness as execution, warrant, admission, authority, continuity, action authorization, runtime motion, Lisp evaluation, membrane packet emission, replay, passage, dry-run execution, or activation.";

        return CreateReceipt(
            request,
            disposition,
            outcomeCode,
            governanceTrace,
            refusal: null,
            timestampUtc);
    }

    private static EnactmentBoundaryReadinessReceipt Refuse(
        EnactmentBoundaryReadinessRequest request,
        string outcomeCode,
        string governanceTrace,
        DateTimeOffset timestampUtc) =>
        CreateReceipt(
            request,
            EnactmentBoundaryReadinessDisposition.Refused,
            outcomeCode,
            governanceTrace,
            new EnactmentBoundaryReadinessRefusalReceipt(
                ReceiptHandle: $"urn:san:enactment-boundary-readiness-refusal:{ShortHash(SourceHandle(request), outcomeCode)}",
                RefusalCode: outcomeCode,
                GovernanceTrace: governanceTrace,
                Retained: true),
            timestampUtc);

    private static EnactmentBoundaryReadinessReceipt CreateReceipt(
        EnactmentBoundaryReadinessRequest request,
        EnactmentBoundaryReadinessDisposition disposition,
        string outcomeCode,
        string governanceTrace,
        EnactmentBoundaryReadinessRefusalReceipt? refusal,
        DateTimeOffset timestampUtc) =>
        new(
            ReceiptHandle: $"urn:san:enactment-boundary-readiness:{(refusal is null ? "review" : "refused")}:{ShortHash(SourceHandle(request), outcomeCode, request.Candidates.Count.ToString(System.Globalization.CultureInfo.InvariantCulture), timestampUtc.UtcTicks.ToString(System.Globalization.CultureInfo.InvariantCulture))}",
            Disposition: disposition,
            OutcomeCode: outcomeCode,
            GovernanceTrace: governanceTrace,
            SourcePacketFormationReceiptHandle: SourceHandle(request),
            Candidates: refusal is null ? request.Candidates.ToArray() : [],
            StewardReviewRoutes: refusal is null ? request.StewardReviewRoutes.ToArray() : [],
            ScopeBoundary: request.ScopeBoundary,
            NonExecutionBoundary: request.NonExecutionBoundary,
            Refusal: refusal,
            PriorPassageCount: request.PriorPassageCount,
            PassageCountAfterReadiness: request.PriorPassageCount,
            RetainedCandidateCount: refusal is null ? request.Candidates.Count : 0,
            ReviewOnly: true,
            ApproachOnly: true,
            ReadyForEnactmentBoundaryReview: refusal is null &&
                disposition == EnactmentBoundaryReadinessDisposition.ReadyForEnactmentBoundaryReviewCold,
            ReadinessBecameWarrant: false,
            ReadinessBecameAdmission: false,
            ReadinessGrantedAuthority: false,
            ReadinessAdmittedContinuity: false,
            ReadinessAuthorizedAction: false,
            ReadinessExecutedAction: false,
            ApproachAuthorizedAction: false,
            LocalityAuthorizedAction: false,
            ReversibilityAuthorizedAction: false,
            StewardReviewMovedRuntime: false,
            DryRunPlanExecuted: false,
            RuntimeActionAllowed: false,
            LispEvaluationAllowed: false,
            NewMembranePacketEmitted: false,
            ReceiptsReplayed: false,
            ContinuityAdmitted: false,
            AuthorityGranted: false,
            ActivationRefused: true,
            TimestampUtc: timestampUtc);

    private static string SourceHandle(EnactmentBoundaryReadinessRequest request) =>
        request.SourcePacketFormationReceipt?.ReceiptHandle ?? "missing-scoped-work-packet-source";

    private static bool HasDuplicate(IEnumerable<string> handles)
    {
        var seen = new HashSet<string>(StringComparer.Ordinal);
        foreach (var handle in handles)
        {
            if (!seen.Add(handle))
            {
                return true;
            }
        }

        return false;
    }

    private static string ShortHash(params string[] parts)
    {
        var payload = string.Join("|", parts);
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(payload));
        return Convert.ToHexString(bytes)[..16].ToLowerInvariant();
    }
}
