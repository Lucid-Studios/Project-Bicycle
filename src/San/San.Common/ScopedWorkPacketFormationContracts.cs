using System.Security.Cryptography;
using System.Text;

namespace San.Common;

public enum ScopedWorkPacketFormationDisposition
{
    FormedForReviewCold = 0,
    EmptyReviewCold = 1,
    Refused = 2
}

public sealed record ScopedWorkPacketDeclaration(
    string PacketHandle,
    string SourceSelectionHandle,
    string SourceMaturationCandidateHandle,
    string SourcePayloadStatementHandle,
    string DutyStation,
    string WorkSurface,
    string IntendedWork,
    string MethodCode,
    string AuthorityCeiling,
    string CustodyOwner,
    string WitnessHandle,
    string TelemetryRoute,
    string StewardRoute,
    string RevocationPath,
    string RepairPath,
    string LossCondition,
    bool ReviewOnly,
    bool CandidateOnly,
    bool LocalOnly,
    bool ReversibleOnly,
    bool RequiresStewardReview,
    bool RequiresSeparateEnactmentBoundary,
    bool PacketBecomesWarrant,
    bool PacketBecomesAdmission,
    bool PacketGrantsAuthority,
    bool PacketAdmitsContinuity,
    bool PacketAuthorizesAction,
    bool PacketExecutesAction,
    bool PacketEvaluatesLisp,
    bool PacketEmitsMembranePacket,
    bool PacketReplaysReceipt,
    bool PacketIncrementsPassage,
    bool PacketActivates)
{
    public bool IsColdPacketDeclaration =>
        !string.IsNullOrWhiteSpace(PacketHandle) &&
        !string.IsNullOrWhiteSpace(SourceSelectionHandle) &&
        !string.IsNullOrWhiteSpace(SourceMaturationCandidateHandle) &&
        !string.IsNullOrWhiteSpace(SourcePayloadStatementHandle) &&
        !string.IsNullOrWhiteSpace(DutyStation) &&
        !string.IsNullOrWhiteSpace(WorkSurface) &&
        !string.IsNullOrWhiteSpace(IntendedWork) &&
        !string.IsNullOrWhiteSpace(MethodCode) &&
        !string.IsNullOrWhiteSpace(AuthorityCeiling) &&
        !string.IsNullOrWhiteSpace(CustodyOwner) &&
        !string.IsNullOrWhiteSpace(WitnessHandle) &&
        !string.IsNullOrWhiteSpace(TelemetryRoute) &&
        !string.IsNullOrWhiteSpace(StewardRoute) &&
        !string.IsNullOrWhiteSpace(RevocationPath) &&
        !string.IsNullOrWhiteSpace(RepairPath) &&
        !string.IsNullOrWhiteSpace(LossCondition) &&
        ReviewOnly &&
        CandidateOnly &&
        LocalOnly &&
        ReversibleOnly &&
        RequiresStewardReview &&
        RequiresSeparateEnactmentBoundary &&
        !PacketBecomesWarrant &&
        !PacketBecomesAdmission &&
        !PacketGrantsAuthority &&
        !PacketAdmitsContinuity &&
        !PacketAuthorizesAction &&
        !PacketExecutesAction &&
        !PacketEvaluatesLisp &&
        !PacketEmitsMembranePacket &&
        !PacketReplaysReceipt &&
        !PacketIncrementsPassage &&
        !PacketActivates;
}

public sealed record ScopedWorkPacketStewardRoute(
    string RouteHandle,
    string PacketHandle,
    string StewardSurface,
    string CustodyOwner,
    string EvidenceHandle,
    string WitnessHandle,
    string TelemetryRoute,
    string ReturnPathHandle,
    bool ReviewOnly,
    bool PreservesPacketLineage,
    bool PreservesSelectionLineage,
    bool PreservesCompostLineage,
    bool RoutesToStewardReview,
    bool RequiresCooling,
    bool RouteAuthorizesAction,
    bool RouteExecutesAction,
    bool RouteGrantsAuthority,
    bool RouteAdmitsContinuity,
    bool RouteEvaluatesLisp,
    bool RouteEmitsMembranePacket,
    bool RouteActivates)
{
    public bool IsColdStewardRoute =>
        !string.IsNullOrWhiteSpace(RouteHandle) &&
        !string.IsNullOrWhiteSpace(PacketHandle) &&
        !string.IsNullOrWhiteSpace(StewardSurface) &&
        !string.IsNullOrWhiteSpace(CustodyOwner) &&
        !string.IsNullOrWhiteSpace(EvidenceHandle) &&
        !string.IsNullOrWhiteSpace(WitnessHandle) &&
        !string.IsNullOrWhiteSpace(TelemetryRoute) &&
        !string.IsNullOrWhiteSpace(ReturnPathHandle) &&
        ReviewOnly &&
        PreservesPacketLineage &&
        PreservesSelectionLineage &&
        PreservesCompostLineage &&
        RoutesToStewardReview &&
        RequiresCooling &&
        !RouteAuthorizesAction &&
        !RouteExecutesAction &&
        !RouteGrantsAuthority &&
        !RouteAdmitsContinuity &&
        !RouteEvaluatesLisp &&
        !RouteEmitsMembranePacket &&
        !RouteActivates;
}

public sealed record ScopedWorkPacketScopeBoundary(
    string BoundaryCode,
    bool Present,
    bool ReviewOnly,
    bool AllowsWorkPacketFormation,
    bool RequiresDutyStation,
    bool RequiresWorkSurface,
    bool RequiresIntendedWork,
    bool RequiresMethodCode,
    bool RequiresAuthorityCeiling,
    bool RequiresCustody,
    bool RequiresWitness,
    bool RequiresTelemetryRoute,
    bool RequiresStewardRoute,
    bool RequiresRevocationPath,
    bool RequiresRepairPath,
    bool RequiresLossCondition,
    bool RequiresSeparateEnactmentBoundary,
    bool RequiresLocalEffectBoundary,
    bool RequiresReversibility,
    bool AllowsPacketAsWarrant,
    bool AllowsPacketAsAdmission,
    bool AllowsPacketAsAuthority,
    bool AllowsPacketAsContinuity,
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
        AllowsWorkPacketFormation &&
        RequiresDutyStation &&
        RequiresWorkSurface &&
        RequiresIntendedWork &&
        RequiresMethodCode &&
        RequiresAuthorityCeiling &&
        RequiresCustody &&
        RequiresWitness &&
        RequiresTelemetryRoute &&
        RequiresStewardRoute &&
        RequiresRevocationPath &&
        RequiresRepairPath &&
        RequiresLossCondition &&
        RequiresSeparateEnactmentBoundary &&
        RequiresLocalEffectBoundary &&
        RequiresReversibility &&
        !AllowsPacketAsWarrant &&
        !AllowsPacketAsAdmission &&
        !AllowsPacketAsAuthority &&
        !AllowsPacketAsContinuity &&
        !AllowsExecution &&
        !AllowsRuntimeMotion &&
        !AllowsLispEvaluation &&
        !AllowsMembranePacketEmission &&
        !AllowsReceiptReplay &&
        !AllowsPassageIncrement &&
        !AllowsActivation;
}

public sealed record ScopedWorkPacketNonExecutionBoundary(
    string BoundaryLaw,
    bool WorkPacketMayBecomeWarrant,
    bool WorkPacketMayBecomeAdmission,
    bool WorkPacketMayAuthorize,
    bool WorkPacketMayExecute,
    bool WorkPacketMayGrantAuthority,
    bool WorkPacketMayAdmitContinuity,
    bool StewardRoutingMayExecute,
    bool ReversibilityMayAuthorize,
    bool LocalityMayAuthorize,
    bool WorkPacketMayEvaluateLisp,
    bool WorkPacketMayEmitMembranePacket,
    bool WorkPacketMayReplayReceipt,
    bool WorkPacketMayIncrementPassage,
    bool WorkPacketMayActivate)
{
    public bool IsColdNonExecutionBoundary =>
        !string.IsNullOrWhiteSpace(BoundaryLaw) &&
        !WorkPacketMayBecomeWarrant &&
        !WorkPacketMayBecomeAdmission &&
        !WorkPacketMayAuthorize &&
        !WorkPacketMayExecute &&
        !WorkPacketMayGrantAuthority &&
        !WorkPacketMayAdmitContinuity &&
        !StewardRoutingMayExecute &&
        !ReversibilityMayAuthorize &&
        !LocalityMayAuthorize &&
        !WorkPacketMayEvaluateLisp &&
        !WorkPacketMayEmitMembranePacket &&
        !WorkPacketMayReplayReceipt &&
        !WorkPacketMayIncrementPassage &&
        !WorkPacketMayActivate;
}

public sealed record ScopedWorkPacketFormationRefusalReceipt(
    string ReceiptHandle,
    string RefusalCode,
    string GovernanceTrace,
    bool Retained);

public sealed record ScopedWorkPacketFormationRequest(
    AspirationCandidateSelectionClosureReceipt? SourceSelectionReceipt,
    IReadOnlyList<ScopedWorkPacketDeclaration> Packets,
    IReadOnlyList<ScopedWorkPacketStewardRoute> StewardRoutes,
    ScopedWorkPacketScopeBoundary ScopeBoundary,
    ScopedWorkPacketNonExecutionBoundary NonExecutionBoundary,
    int PriorPassageCount);

public sealed record ScopedWorkPacketFormationReceipt(
    string ReceiptHandle,
    ScopedWorkPacketFormationDisposition Disposition,
    string OutcomeCode,
    string GovernanceTrace,
    string SourceSelectionReceiptHandle,
    IReadOnlyList<ScopedWorkPacketDeclaration> Packets,
    IReadOnlyList<ScopedWorkPacketStewardRoute> StewardRoutes,
    ScopedWorkPacketScopeBoundary ScopeBoundary,
    ScopedWorkPacketNonExecutionBoundary NonExecutionBoundary,
    ScopedWorkPacketFormationRefusalReceipt? Refusal,
    int PriorPassageCount,
    int PassageCountAfterPacketFormation,
    int RetainedPacketCount,
    bool ReviewOnly,
    bool CandidateOnly,
    bool WorkPacketFormedForReview,
    bool PacketBecameWarrant,
    bool PacketBecameAdmission,
    bool PacketGrantedAuthority,
    bool PacketAdmittedContinuity,
    bool PacketAuthorizedAction,
    bool PacketExecutedAction,
    bool ReversibilityAuthorizedAction,
    bool LocalityAuthorizedAction,
    bool RuntimeActionAllowed,
    bool LispEvaluationAllowed,
    bool NewMembranePacketEmitted,
    bool ReceiptsReplayed,
    bool ContinuityAdmitted,
    bool AuthorityGranted,
    bool ActivationRefused,
    DateTimeOffset TimestampUtc)
{
    public bool IsColdScopedWorkPacketFormation =>
        (Disposition is ScopedWorkPacketFormationDisposition.FormedForReviewCold or
            ScopedWorkPacketFormationDisposition.EmptyReviewCold) &&
        Refusal is null &&
        ReviewOnly &&
        CandidateOnly &&
        PassageCountAfterPacketFormation == PriorPassageCount &&
        RetainedPacketCount == Packets.Count &&
        !PacketBecameWarrant &&
        !PacketBecameAdmission &&
        !PacketGrantedAuthority &&
        !PacketAdmittedContinuity &&
        !PacketAuthorizedAction &&
        !PacketExecutedAction &&
        !ReversibilityAuthorizedAction &&
        !LocalityAuthorizedAction &&
        !RuntimeActionAllowed &&
        !LispEvaluationAllowed &&
        !NewMembranePacketEmitted &&
        !ReceiptsReplayed &&
        !ContinuityAdmitted &&
        !AuthorityGranted &&
        ActivationRefused &&
        ScopeBoundary.IsColdScope &&
        NonExecutionBoundary.IsColdNonExecutionBoundary;

    public bool IsRetainedScopedWorkPacketFormationRefusal =>
        Disposition == ScopedWorkPacketFormationDisposition.Refused &&
        Refusal?.Retained == true &&
        PassageCountAfterPacketFormation == PriorPassageCount &&
        RetainedPacketCount == 0 &&
        !WorkPacketFormedForReview &&
        !PacketBecameWarrant &&
        !PacketBecameAdmission &&
        !PacketGrantedAuthority &&
        !PacketAdmittedContinuity &&
        !PacketAuthorizedAction &&
        !PacketExecutedAction &&
        !ReversibilityAuthorizedAction &&
        !LocalityAuthorizedAction &&
        !RuntimeActionAllowed &&
        !LispEvaluationAllowed &&
        !NewMembranePacketEmitted &&
        !ReceiptsReplayed &&
        !ContinuityAdmitted &&
        !AuthorityGranted &&
        ActivationRefused;
}

public sealed class DefaultScopedWorkPacketFormationBoundaryValidator
{
    public ScopedWorkPacketFormationReceipt Declare(
        ScopedWorkPacketFormationRequest request,
        DateTimeOffset timestampUtc)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.SourceSelectionReceipt is null ||
            !request.SourceSelectionReceipt.IsColdSelectionClosure)
        {
            return Refuse(
                request,
                "scoped-work-packet-source-selection-missing",
                "Scoped work packet formation refused because a cold aspiration candidate selection closure receipt is required before work packet formation.",
                timestampUtc);
        }

        if (request.ScopeBoundary is null ||
            !request.ScopeBoundary.Present ||
            string.IsNullOrWhiteSpace(request.ScopeBoundary.BoundaryCode))
        {
            return Refuse(
                request,
                "scoped-work-packet-boundary-missing",
                "Scoped work packet formation refused because a review-only packet scope boundary is required.",
                timestampUtc);
        }

        if (!request.ScopeBoundary.IsColdScope)
        {
            return Refuse(
                request,
                "scoped-work-packet-boundary-promotional",
                "Scoped work packet formation refused because scope must form work packets only for review while requiring duty station, work surface, method, custody, witness, telemetry, Steward route, revocation, repair, loss, local effect boundary, reversibility, and separate enactment boundary, and while refusing warrant, admission, authority, continuity, execution, runtime motion, Lisp evaluation, membrane packet emission, replay, passage, and activation.",
                timestampUtc);
        }

        if (request.NonExecutionBoundary is null ||
            !request.NonExecutionBoundary.IsColdNonExecutionBoundary)
        {
            return Refuse(
                request,
                "scoped-work-packet-non-execution-boundary-invalid",
                "Scoped work packet formation refused because non-execution law must prevent packet formation, Steward routing, reversibility, locality, Lisp evaluation, membrane packet emission, replay, passage, and activation from becoming execution or authority.",
                timestampUtc);
        }

        if (request.Packets.Any(static packet => !packet.IsColdPacketDeclaration))
        {
            return Refuse(
                request,
                "scoped-work-packet-invalid",
                "Scoped work packet refused because every packet must preserve selection, candidate, and payload lineage while declaring duty station, work surface, intended work, method, authority ceiling, custody, witness, telemetry, Steward route, revocation, repair, and loss without warrant, admission, authority, continuity, action, execution, Lisp evaluation, membrane packet emission, replay, passage, or activation.",
                timestampUtc);
        }

        if (HasDuplicate(request.Packets.Select(static packet => packet.PacketHandle)))
        {
            return Refuse(
                request,
                "scoped-work-packet-duplicate-packet-handle",
                "Scoped work packet formation refused because duplicate packet handles would collapse packet lineage.",
                timestampUtc);
        }

        var selectedSelections = request.SourceSelectionReceipt.Selections
            .Where(static selection => selection.SelectionState == AspirationCandidateSelectionState.SelectedWorkingSet)
            .ToDictionary(static selection => selection.SelectionHandle, StringComparer.Ordinal);

        if (request.Packets.Any(packet =>
                !selectedSelections.TryGetValue(packet.SourceSelectionHandle, out var selection) ||
                !string.Equals(packet.SourceMaturationCandidateHandle, selection.SourceMaturationCandidateHandle, StringComparison.Ordinal) ||
                !string.Equals(packet.SourcePayloadStatementHandle, selection.SourcePayloadStatementHandle, StringComparison.Ordinal)))
        {
            return Refuse(
                request,
                "scoped-work-packet-selection-lineage-invalid",
                "Scoped work packet formation refused because every packet must bind to a selected working-set candidate and preserve candidate and payload lineage.",
                timestampUtc);
        }

        var packetHandles = request.Packets
            .Select(static packet => packet.PacketHandle)
            .ToHashSet(StringComparer.Ordinal);

        if (request.StewardRoutes.Any(route =>
                !route.IsColdStewardRoute ||
                !packetHandles.Contains(route.PacketHandle)))
        {
            return Refuse(
                request,
                "scoped-work-packet-steward-route-invalid",
                "Scoped work packet Steward route refused because routes may preserve packet, selection, and compost lineage for Steward review only and may not authorize, execute, grant authority, admit continuity, evaluate Lisp, emit membrane packets, or activate.",
                timestampUtc);
        }

        if (HasDuplicate(request.StewardRoutes.Select(static route => route.RouteHandle)))
        {
            return Refuse(
                request,
                "scoped-work-packet-duplicate-route-handle",
                "Scoped work packet formation refused because duplicate Steward route handles would collapse routing lineage.",
                timestampUtc);
        }

        if (request.Packets.Count > 0 &&
            request.Packets.Any(packet => !request.StewardRoutes.Any(route => route.PacketHandle == packet.PacketHandle)))
        {
            return Refuse(
                request,
                "scoped-work-packet-steward-route-missing",
                "Scoped work packet formation refused because every work packet requires a Steward review route before it can be retained for review.",
                timestampUtc);
        }

        var disposition = request.Packets.Count == 0
            ? ScopedWorkPacketFormationDisposition.EmptyReviewCold
            : ScopedWorkPacketFormationDisposition.FormedForReviewCold;
        var outcomeCode = disposition == ScopedWorkPacketFormationDisposition.EmptyReviewCold
            ? "scoped-work-packet-empty-review-only"
            : "scoped-work-packet-formed-review-only";
        var governanceTrace = disposition == ScopedWorkPacketFormationDisposition.EmptyReviewCold
            ? "Scoped work packet formation found no selected working-set packets. Empty review preserves packet formation boundary without execution, warrant, authority, continuity, Lisp evaluation, membrane packet emission, replay, passage, or activation."
            : "Selected aspiration working sets formed scoped work packets for Steward review while refusing packet formation as execution, warrant, admission, authority, continuity, action authorization, Lisp evaluation, membrane packet emission, replay, passage, runtime motion, or activation.";

        return CreateReceipt(
            request,
            disposition,
            outcomeCode,
            governanceTrace,
            refusal: null,
            timestampUtc);
    }

    private static ScopedWorkPacketFormationReceipt Refuse(
        ScopedWorkPacketFormationRequest request,
        string outcomeCode,
        string governanceTrace,
        DateTimeOffset timestampUtc) =>
        CreateReceipt(
            request,
            ScopedWorkPacketFormationDisposition.Refused,
            outcomeCode,
            governanceTrace,
            new ScopedWorkPacketFormationRefusalReceipt(
                ReceiptHandle: $"urn:san:scoped-work-packet-refusal:{ShortHash(SourceHandle(request), outcomeCode)}",
                RefusalCode: outcomeCode,
                GovernanceTrace: governanceTrace,
                Retained: true),
            timestampUtc);

    private static ScopedWorkPacketFormationReceipt CreateReceipt(
        ScopedWorkPacketFormationRequest request,
        ScopedWorkPacketFormationDisposition disposition,
        string outcomeCode,
        string governanceTrace,
        ScopedWorkPacketFormationRefusalReceipt? refusal,
        DateTimeOffset timestampUtc) =>
        new(
            ReceiptHandle: $"urn:san:scoped-work-packet:{(refusal is null ? "review" : "refused")}:{ShortHash(SourceHandle(request), outcomeCode, request.Packets.Count.ToString(System.Globalization.CultureInfo.InvariantCulture), timestampUtc.UtcTicks.ToString(System.Globalization.CultureInfo.InvariantCulture))}",
            Disposition: disposition,
            OutcomeCode: outcomeCode,
            GovernanceTrace: governanceTrace,
            SourceSelectionReceiptHandle: SourceHandle(request),
            Packets: refusal is null ? request.Packets.ToArray() : [],
            StewardRoutes: refusal is null ? request.StewardRoutes.ToArray() : [],
            ScopeBoundary: request.ScopeBoundary,
            NonExecutionBoundary: request.NonExecutionBoundary,
            Refusal: refusal,
            PriorPassageCount: request.PriorPassageCount,
            PassageCountAfterPacketFormation: request.PriorPassageCount,
            RetainedPacketCount: refusal is null ? request.Packets.Count : 0,
            ReviewOnly: true,
            CandidateOnly: true,
            WorkPacketFormedForReview: refusal is null &&
                disposition == ScopedWorkPacketFormationDisposition.FormedForReviewCold,
            PacketBecameWarrant: false,
            PacketBecameAdmission: false,
            PacketGrantedAuthority: false,
            PacketAdmittedContinuity: false,
            PacketAuthorizedAction: false,
            PacketExecutedAction: false,
            ReversibilityAuthorizedAction: false,
            LocalityAuthorizedAction: false,
            RuntimeActionAllowed: false,
            LispEvaluationAllowed: false,
            NewMembranePacketEmitted: false,
            ReceiptsReplayed: false,
            ContinuityAdmitted: false,
            AuthorityGranted: false,
            ActivationRefused: true,
            TimestampUtc: timestampUtc);

    private static string SourceHandle(ScopedWorkPacketFormationRequest request) =>
        request.SourceSelectionReceipt?.ReceiptHandle ?? "missing-aspiration-selection-source";

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
