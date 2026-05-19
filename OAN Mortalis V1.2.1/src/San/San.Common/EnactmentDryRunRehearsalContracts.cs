using System.Security.Cryptography;
using System.Text;

namespace San.Common;

public enum EnactmentDryRunRehearsalDisposition
{
    RehearsedCold = 0,
    EmptyRehearsalCold = 1,
    Refused = 2
}

public sealed record EnactmentDryRunCase(
    string RehearsalHandle,
    string SourceReadinessHandle,
    string SourcePacketHandle,
    string DryRunPlanHandle,
    string DutyStation,
    string WorkSurface,
    string IntendedWork,
    string MethodCode,
    string SimulatedEffectHandle,
    string RollbackProofHandle,
    string CustodyOwner,
    string WitnessHandle,
    string TelemetryRoute,
    string StewardReviewHandle,
    bool ReviewOnly,
    bool SimulationOnly,
    bool NoOpOnly,
    bool LocalOnly,
    bool ReversibleOnly,
    bool RequiresRollbackProof,
    bool RequiresStewardReview,
    bool SimulationBecomesPermission,
    bool DryRunAuthorizesAction,
    bool DryRunExecutesAction,
    bool DryRunMovesRuntime,
    bool DryRunWritesOutsideReceiptSurface,
    bool DryRunGrantsAuthority,
    bool DryRunAdmitsContinuity,
    bool DryRunEvaluatesLisp,
    bool DryRunEmitsMembranePacket,
    bool DryRunReplaysReceipt,
    bool DryRunIncrementsPassage,
    bool DryRunActivates)
{
    public bool IsColdDryRunCase =>
        !string.IsNullOrWhiteSpace(RehearsalHandle) &&
        !string.IsNullOrWhiteSpace(SourceReadinessHandle) &&
        !string.IsNullOrWhiteSpace(SourcePacketHandle) &&
        !string.IsNullOrWhiteSpace(DryRunPlanHandle) &&
        !string.IsNullOrWhiteSpace(DutyStation) &&
        !string.IsNullOrWhiteSpace(WorkSurface) &&
        !string.IsNullOrWhiteSpace(IntendedWork) &&
        !string.IsNullOrWhiteSpace(MethodCode) &&
        !string.IsNullOrWhiteSpace(SimulatedEffectHandle) &&
        !string.IsNullOrWhiteSpace(RollbackProofHandle) &&
        !string.IsNullOrWhiteSpace(CustodyOwner) &&
        !string.IsNullOrWhiteSpace(WitnessHandle) &&
        !string.IsNullOrWhiteSpace(TelemetryRoute) &&
        !string.IsNullOrWhiteSpace(StewardReviewHandle) &&
        ReviewOnly &&
        SimulationOnly &&
        NoOpOnly &&
        LocalOnly &&
        ReversibleOnly &&
        RequiresRollbackProof &&
        RequiresStewardReview &&
        !SimulationBecomesPermission &&
        !DryRunAuthorizesAction &&
        !DryRunExecutesAction &&
        !DryRunMovesRuntime &&
        !DryRunWritesOutsideReceiptSurface &&
        !DryRunGrantsAuthority &&
        !DryRunAdmitsContinuity &&
        !DryRunEvaluatesLisp &&
        !DryRunEmitsMembranePacket &&
        !DryRunReplaysReceipt &&
        !DryRunIncrementsPassage &&
        !DryRunActivates;
}

public sealed record StewardDryRunReviewReceiptRoute(
    string ReviewRouteHandle,
    string RehearsalHandle,
    string SourceReadinessHandle,
    string SourcePacketHandle,
    string StewardSurface,
    string CustodyOwner,
    string EvidenceHandle,
    string WitnessHandle,
    string TelemetryRoute,
    string ReturnPathHandle,
    bool ReviewOnly,
    bool PreservesRehearsalLineage,
    bool PreservesReadinessLineage,
    bool PreservesPacketLineage,
    bool PreservesDryRunPlanLineage,
    bool RoutesToStewardDryRunReview,
    bool RequiresCooling,
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
    public bool IsColdStewardDryRunReviewRoute =>
        !string.IsNullOrWhiteSpace(ReviewRouteHandle) &&
        !string.IsNullOrWhiteSpace(RehearsalHandle) &&
        !string.IsNullOrWhiteSpace(SourceReadinessHandle) &&
        !string.IsNullOrWhiteSpace(SourcePacketHandle) &&
        !string.IsNullOrWhiteSpace(StewardSurface) &&
        !string.IsNullOrWhiteSpace(CustodyOwner) &&
        !string.IsNullOrWhiteSpace(EvidenceHandle) &&
        !string.IsNullOrWhiteSpace(WitnessHandle) &&
        !string.IsNullOrWhiteSpace(TelemetryRoute) &&
        !string.IsNullOrWhiteSpace(ReturnPathHandle) &&
        ReviewOnly &&
        PreservesRehearsalLineage &&
        PreservesReadinessLineage &&
        PreservesPacketLineage &&
        PreservesDryRunPlanLineage &&
        RoutesToStewardDryRunReview &&
        RequiresCooling &&
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

public sealed record EnactmentDryRunScopeBoundary(
    string BoundaryCode,
    bool Present,
    bool ReviewOnly,
    bool AllowsDryRunRehearsal,
    bool RequiresReadinessReceipt,
    bool RequiresDryRunPlan,
    bool RequiresSimulatedEffect,
    bool RequiresRollbackProof,
    bool RequiresNoOp,
    bool RequiresLocality,
    bool RequiresReversibility,
    bool RequiresCustody,
    bool RequiresWitness,
    bool RequiresTelemetryRoute,
    bool RequiresStewardReview,
    bool AllowsSimulationAsPermission,
    bool AllowsActionAuthorization,
    bool AllowsExecution,
    bool AllowsRuntimeMotion,
    bool AllowsOutsideReceiptSurfaceWrite,
    bool AllowsAuthority,
    bool AllowsContinuityAdmission,
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
        AllowsDryRunRehearsal &&
        RequiresReadinessReceipt &&
        RequiresDryRunPlan &&
        RequiresSimulatedEffect &&
        RequiresRollbackProof &&
        RequiresNoOp &&
        RequiresLocality &&
        RequiresReversibility &&
        RequiresCustody &&
        RequiresWitness &&
        RequiresTelemetryRoute &&
        RequiresStewardReview &&
        !AllowsSimulationAsPermission &&
        !AllowsActionAuthorization &&
        !AllowsExecution &&
        !AllowsRuntimeMotion &&
        !AllowsOutsideReceiptSurfaceWrite &&
        !AllowsAuthority &&
        !AllowsContinuityAdmission &&
        !AllowsLispEvaluation &&
        !AllowsMembranePacketEmission &&
        !AllowsReceiptReplay &&
        !AllowsPassageIncrement &&
        !AllowsActivation;
}

public sealed record EnactmentDryRunNonEnactmentBoundary(
    string BoundaryLaw,
    bool DryRunMayBecomePermission,
    bool DryRunMayAuthorize,
    bool DryRunMayExecute,
    bool DryRunMayMoveRuntime,
    bool DryRunMayWriteOutsideReceiptSurface,
    bool DryRunMayGrantAuthority,
    bool DryRunMayAdmitContinuity,
    bool StewardDryRunReviewMayMoveRuntime,
    bool SimulationMayBecomePermission,
    bool ReversibleLocalEffectMayAuthorize,
    bool NoOpRequired,
    bool RollbackProofRequired,
    bool DryRunMayEvaluateLisp,
    bool DryRunMayEmitMembranePacket,
    bool DryRunMayReplayReceipt,
    bool DryRunMayIncrementPassage,
    bool DryRunMayActivate)
{
    public bool IsColdNonEnactmentBoundary =>
        !string.IsNullOrWhiteSpace(BoundaryLaw) &&
        !DryRunMayBecomePermission &&
        !DryRunMayAuthorize &&
        !DryRunMayExecute &&
        !DryRunMayMoveRuntime &&
        !DryRunMayWriteOutsideReceiptSurface &&
        !DryRunMayGrantAuthority &&
        !DryRunMayAdmitContinuity &&
        !StewardDryRunReviewMayMoveRuntime &&
        !SimulationMayBecomePermission &&
        !ReversibleLocalEffectMayAuthorize &&
        NoOpRequired &&
        RollbackProofRequired &&
        !DryRunMayEvaluateLisp &&
        !DryRunMayEmitMembranePacket &&
        !DryRunMayReplayReceipt &&
        !DryRunMayIncrementPassage &&
        !DryRunMayActivate;
}

public sealed record EnactmentDryRunRehearsalRefusalReceipt(
    string ReceiptHandle,
    string RefusalCode,
    string GovernanceTrace,
    bool Retained);

public sealed record EnactmentDryRunRehearsalRequest(
    EnactmentBoundaryReadinessReceipt? SourceReadinessReceipt,
    IReadOnlyList<EnactmentDryRunCase> DryRunCases,
    IReadOnlyList<StewardDryRunReviewReceiptRoute> StewardReviewRoutes,
    EnactmentDryRunScopeBoundary ScopeBoundary,
    EnactmentDryRunNonEnactmentBoundary NonEnactmentBoundary,
    int PriorPassageCount);

public sealed record EnactmentDryRunRehearsalReceipt(
    string ReceiptHandle,
    EnactmentDryRunRehearsalDisposition Disposition,
    string OutcomeCode,
    string GovernanceTrace,
    string SourceReadinessReceiptHandle,
    IReadOnlyList<EnactmentDryRunCase> DryRunCases,
    IReadOnlyList<StewardDryRunReviewReceiptRoute> StewardReviewRoutes,
    EnactmentDryRunScopeBoundary ScopeBoundary,
    EnactmentDryRunNonEnactmentBoundary NonEnactmentBoundary,
    EnactmentDryRunRehearsalRefusalReceipt? Refusal,
    int PriorPassageCount,
    int PassageCountAfterDryRun,
    int RetainedDryRunCaseCount,
    bool ReviewOnly,
    bool SimulationOnly,
    bool NoOpOnly,
    bool DryRunRehearsed,
    bool DryRunBecamePermission,
    bool DryRunAuthorizedAction,
    bool DryRunExecutedAction,
    bool DryRunMovedRuntime,
    bool DryRunWroteOutsideReceiptSurface,
    bool DryRunGrantedAuthority,
    bool DryRunAdmittedContinuity,
    bool StewardDryRunReviewMovedRuntime,
    bool ReversibleLocalEffectAuthorizedAction,
    bool LispEvaluationAllowed,
    bool NewMembranePacketEmitted,
    bool ReceiptsReplayed,
    bool ContinuityAdmitted,
    bool AuthorityGranted,
    bool ActivationRefused,
    DateTimeOffset TimestampUtc)
{
    public bool IsColdEnactmentDryRunRehearsal =>
        (Disposition is EnactmentDryRunRehearsalDisposition.RehearsedCold or
            EnactmentDryRunRehearsalDisposition.EmptyRehearsalCold) &&
        Refusal is null &&
        ReviewOnly &&
        SimulationOnly &&
        NoOpOnly &&
        PassageCountAfterDryRun == PriorPassageCount &&
        RetainedDryRunCaseCount == DryRunCases.Count &&
        !DryRunBecamePermission &&
        !DryRunAuthorizedAction &&
        !DryRunExecutedAction &&
        !DryRunMovedRuntime &&
        !DryRunWroteOutsideReceiptSurface &&
        !DryRunGrantedAuthority &&
        !DryRunAdmittedContinuity &&
        !StewardDryRunReviewMovedRuntime &&
        !ReversibleLocalEffectAuthorizedAction &&
        !LispEvaluationAllowed &&
        !NewMembranePacketEmitted &&
        !ReceiptsReplayed &&
        !ContinuityAdmitted &&
        !AuthorityGranted &&
        ActivationRefused &&
        ScopeBoundary.IsColdScope &&
        NonEnactmentBoundary.IsColdNonEnactmentBoundary;

    public bool IsRetainedEnactmentDryRunRehearsalRefusal =>
        Disposition == EnactmentDryRunRehearsalDisposition.Refused &&
        Refusal?.Retained == true &&
        PassageCountAfterDryRun == PriorPassageCount &&
        RetainedDryRunCaseCount == 0 &&
        !DryRunRehearsed &&
        !DryRunBecamePermission &&
        !DryRunAuthorizedAction &&
        !DryRunExecutedAction &&
        !DryRunMovedRuntime &&
        !DryRunWroteOutsideReceiptSurface &&
        !DryRunGrantedAuthority &&
        !DryRunAdmittedContinuity &&
        !StewardDryRunReviewMovedRuntime &&
        !ReversibleLocalEffectAuthorizedAction &&
        !LispEvaluationAllowed &&
        !NewMembranePacketEmitted &&
        !ReceiptsReplayed &&
        !ContinuityAdmitted &&
        !AuthorityGranted &&
        ActivationRefused;
}

public sealed class DefaultEnactmentDryRunRehearsalBoundaryValidator
{
    public EnactmentDryRunRehearsalReceipt Declare(
        EnactmentDryRunRehearsalRequest request,
        DateTimeOffset timestampUtc)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.SourceReadinessReceipt is null ||
            !request.SourceReadinessReceipt.IsColdEnactmentBoundaryReadiness)
        {
            return Refuse(
                request,
                "enactment-dry-run-rehearsal-source-readiness-missing",
                "Enactment dry-run rehearsal refused because a cold enactment boundary readiness receipt is required before rehearsal may be retained.",
                timestampUtc);
        }

        if (request.ScopeBoundary is null ||
            !request.ScopeBoundary.Present ||
            string.IsNullOrWhiteSpace(request.ScopeBoundary.BoundaryCode))
        {
            return Refuse(
                request,
                "enactment-dry-run-rehearsal-scope-missing",
                "Enactment dry-run rehearsal refused because a review-only dry-run scope boundary is required.",
                timestampUtc);
        }

        if (!request.ScopeBoundary.IsColdScope)
        {
            return Refuse(
                request,
                "enactment-dry-run-rehearsal-scope-promotional",
                "Enactment dry-run rehearsal refused because rehearsal may retain only simulation-only, no-op, local, reversible, rollback-proven cases while refusing simulation as permission, action authorization, execution, runtime motion, writes outside receipt surfaces, authority, continuity, Lisp evaluation, membrane packet emission, replay, passage, and activation.",
                timestampUtc);
        }

        if (request.NonEnactmentBoundary is null ||
            !request.NonEnactmentBoundary.IsColdNonEnactmentBoundary)
        {
            return Refuse(
                request,
                "enactment-dry-run-rehearsal-non-enactment-invalid",
                "Enactment dry-run rehearsal refused because non-enactment law must prevent dry-run, simulation, reversible local effect, and Steward dry-run review from becoming permission, authorization, execution, authority, continuity, runtime motion, Lisp evaluation, membrane packet emission, replay, passage, or activation.",
                timestampUtc);
        }

        if (request.DryRunCases.Any(static item => !item.IsColdDryRunCase))
        {
            return Refuse(
                request,
                "enactment-dry-run-rehearsal-case-invalid",
                "Enactment dry-run rehearsal refused because every dry-run case must remain review-only, simulation-only, no-op, local, reversible, rollback-proven, and Steward-routed without permission, authorization, execution, runtime motion, outside receipt writes, authority, continuity, Lisp evaluation, membrane packet emission, replay, passage, or activation.",
                timestampUtc);
        }

        if (HasDuplicate(request.DryRunCases.Select(static item => item.RehearsalHandle)))
        {
            return Refuse(
                request,
                "enactment-dry-run-rehearsal-duplicate-case-handle",
                "Enactment dry-run rehearsal refused because duplicate rehearsal handles would collapse rehearsal lineage.",
                timestampUtc);
        }

        var sourceCandidates = request.SourceReadinessReceipt.Candidates
            .ToDictionary(static item => item.ReadinessHandle, StringComparer.Ordinal);

        if (request.DryRunCases.Any(item =>
                !sourceCandidates.TryGetValue(item.SourceReadinessHandle, out var source) ||
                !string.Equals(item.SourcePacketHandle, source.SourcePacketHandle, StringComparison.Ordinal) ||
                !string.Equals(item.DryRunPlanHandle, source.DryRunPlanHandle, StringComparison.Ordinal) ||
                !string.Equals(item.DutyStation, source.DutyStation, StringComparison.Ordinal) ||
                !string.Equals(item.WorkSurface, source.WorkSurface, StringComparison.Ordinal) ||
                !string.Equals(item.IntendedWork, source.IntendedWork, StringComparison.Ordinal) ||
                !string.Equals(item.MethodCode, source.MethodCode, StringComparison.Ordinal) ||
                !string.Equals(item.CustodyOwner, source.CustodyOwner, StringComparison.Ordinal) ||
                !string.Equals(item.WitnessHandle, source.WitnessHandle, StringComparison.Ordinal) ||
                !string.Equals(item.TelemetryRoute, source.TelemetryRoute, StringComparison.Ordinal) ||
                !string.Equals(item.StewardReviewHandle, source.StewardReviewHandle, StringComparison.Ordinal)))
        {
            return Refuse(
                request,
                "enactment-dry-run-rehearsal-lineage-invalid",
                "Enactment dry-run rehearsal refused because every dry-run case must bind to retained enactment readiness, packet, dry-run plan, duty, surface, work, method, custody, witness, telemetry, and Steward review lineage.",
                timestampUtc);
        }

        var rehearsalHandles = request.DryRunCases
            .Select(static item => item.RehearsalHandle)
            .ToHashSet(StringComparer.Ordinal);
        var readinessHandles = request.DryRunCases
            .Select(static item => item.SourceReadinessHandle)
            .ToHashSet(StringComparer.Ordinal);
        var packetHandles = request.DryRunCases
            .Select(static item => item.SourcePacketHandle)
            .ToHashSet(StringComparer.Ordinal);
        var casesByHandle = request.DryRunCases
            .ToDictionary(static item => item.RehearsalHandle, StringComparer.Ordinal);

        if (request.StewardReviewRoutes.Any(route =>
                !route.IsColdStewardDryRunReviewRoute ||
                !rehearsalHandles.Contains(route.RehearsalHandle) ||
                !readinessHandles.Contains(route.SourceReadinessHandle) ||
                !packetHandles.Contains(route.SourcePacketHandle) ||
                !casesByHandle.TryGetValue(route.RehearsalHandle, out var dryRun) ||
                !string.Equals(route.SourceReadinessHandle, dryRun.SourceReadinessHandle, StringComparison.Ordinal) ||
                !string.Equals(route.SourcePacketHandle, dryRun.SourcePacketHandle, StringComparison.Ordinal) ||
                !string.Equals(route.CustodyOwner, dryRun.CustodyOwner, StringComparison.Ordinal) ||
                !string.Equals(route.WitnessHandle, dryRun.WitnessHandle, StringComparison.Ordinal) ||
                !string.Equals(route.TelemetryRoute, dryRun.TelemetryRoute, StringComparison.Ordinal)))
        {
            return Refuse(
                request,
                "enactment-dry-run-rehearsal-steward-route-invalid",
                "Enactment dry-run rehearsal Steward route refused because dry-run review routes may preserve rehearsal, readiness, packet, and dry-run plan lineage only and may not authorize, execute, move runtime, grant authority, admit continuity, evaluate Lisp, emit membrane packets, replay receipts, increment passage, or activate.",
                timestampUtc);
        }

        if (HasDuplicate(request.StewardReviewRoutes.Select(static item => item.ReviewRouteHandle)))
        {
            return Refuse(
                request,
                "enactment-dry-run-rehearsal-duplicate-route-handle",
                "Enactment dry-run rehearsal refused because duplicate Steward dry-run review route handles would collapse review-route lineage.",
                timestampUtc);
        }

        if (request.DryRunCases.Count > 0 &&
            request.DryRunCases.Any(item => !request.StewardReviewRoutes.Any(route => route.RehearsalHandle == item.RehearsalHandle)))
        {
            return Refuse(
                request,
                "enactment-dry-run-rehearsal-steward-route-missing",
                "Enactment dry-run rehearsal refused because every rehearsal case requires a Steward dry-run review route before it can be retained.",
                timestampUtc);
        }

        var disposition = request.DryRunCases.Count == 0
            ? EnactmentDryRunRehearsalDisposition.EmptyRehearsalCold
            : EnactmentDryRunRehearsalDisposition.RehearsedCold;
        var outcomeCode = disposition == EnactmentDryRunRehearsalDisposition.EmptyRehearsalCold
            ? "enactment-dry-run-rehearsal-empty-review-only"
            : "enactment-dry-run-rehearsal-rehearsed-review-only";
        var governanceTrace = disposition == EnactmentDryRunRehearsalDisposition.EmptyRehearsalCold
            ? "Enactment dry-run rehearsal found no ready work candidates. Empty rehearsal remains review-only and does not create permission, action, authority, continuity, runtime motion, Lisp evaluation, membrane packet emission, replay, passage, or activation."
            : "Ready work packets entered dry-run rehearsal as simulation-only, no-op, local, reversible cases while refusing rehearsal as enactment, simulation as permission, reversible local effect as authorization, Steward dry-run review as runtime motion, action, authority, continuity, Lisp evaluation, packet emission, replay, passage, outside receipt-surface write, or activation.";

        return CreateReceipt(
            request,
            disposition,
            outcomeCode,
            governanceTrace,
            refusal: null,
            timestampUtc);
    }

    private static EnactmentDryRunRehearsalReceipt Refuse(
        EnactmentDryRunRehearsalRequest request,
        string outcomeCode,
        string governanceTrace,
        DateTimeOffset timestampUtc) =>
        CreateReceipt(
            request,
            EnactmentDryRunRehearsalDisposition.Refused,
            outcomeCode,
            governanceTrace,
            new EnactmentDryRunRehearsalRefusalReceipt(
                ReceiptHandle: $"urn:san:enactment-dry-run-rehearsal-refusal:{ShortHash(SourceHandle(request), outcomeCode)}",
                RefusalCode: outcomeCode,
                GovernanceTrace: governanceTrace,
                Retained: true),
            timestampUtc);

    private static EnactmentDryRunRehearsalReceipt CreateReceipt(
        EnactmentDryRunRehearsalRequest request,
        EnactmentDryRunRehearsalDisposition disposition,
        string outcomeCode,
        string governanceTrace,
        EnactmentDryRunRehearsalRefusalReceipt? refusal,
        DateTimeOffset timestampUtc) =>
        new(
            ReceiptHandle: $"urn:san:enactment-dry-run-rehearsal:{(refusal is null ? "review" : "refused")}:{ShortHash(SourceHandle(request), outcomeCode, request.DryRunCases.Count.ToString(System.Globalization.CultureInfo.InvariantCulture), timestampUtc.UtcTicks.ToString(System.Globalization.CultureInfo.InvariantCulture))}",
            Disposition: disposition,
            OutcomeCode: outcomeCode,
            GovernanceTrace: governanceTrace,
            SourceReadinessReceiptHandle: SourceHandle(request),
            DryRunCases: refusal is null ? request.DryRunCases.ToArray() : [],
            StewardReviewRoutes: refusal is null ? request.StewardReviewRoutes.ToArray() : [],
            ScopeBoundary: request.ScopeBoundary,
            NonEnactmentBoundary: request.NonEnactmentBoundary,
            Refusal: refusal,
            PriorPassageCount: request.PriorPassageCount,
            PassageCountAfterDryRun: request.PriorPassageCount,
            RetainedDryRunCaseCount: refusal is null ? request.DryRunCases.Count : 0,
            ReviewOnly: true,
            SimulationOnly: true,
            NoOpOnly: true,
            DryRunRehearsed: refusal is null &&
                disposition == EnactmentDryRunRehearsalDisposition.RehearsedCold,
            DryRunBecamePermission: false,
            DryRunAuthorizedAction: false,
            DryRunExecutedAction: false,
            DryRunMovedRuntime: false,
            DryRunWroteOutsideReceiptSurface: false,
            DryRunGrantedAuthority: false,
            DryRunAdmittedContinuity: false,
            StewardDryRunReviewMovedRuntime: false,
            ReversibleLocalEffectAuthorizedAction: false,
            LispEvaluationAllowed: false,
            NewMembranePacketEmitted: false,
            ReceiptsReplayed: false,
            ContinuityAdmitted: false,
            AuthorityGranted: false,
            ActivationRefused: true,
            TimestampUtc: timestampUtc);

    private static string SourceHandle(EnactmentDryRunRehearsalRequest request) =>
        request.SourceReadinessReceipt?.ReceiptHandle ?? "missing-enactment-readiness-source";

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
