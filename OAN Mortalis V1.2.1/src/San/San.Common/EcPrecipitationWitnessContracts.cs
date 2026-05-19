using System.Security.Cryptography;
using System.Text;

namespace San.Common;

public enum EcPrecipitationWitnessDisposition
{
    WitnessedCandidateCold = 0,
    EmptyWitnessCold = 1,
    Refused = 2
}

public sealed record EcPrecipitationResidueCandidate(
    string ResidueHandle,
    string SourceRehearsalHandle,
    string SourceReadinessHandle,
    string SourcePacketHandle,
    string SourceDryRunPlanHandle,
    string MeaningFormationHandle,
    string CandidateSplineHandle,
    string ConditionalSelfGelContextHandle,
    string ConditionalOeContextHandle,
    string CompassCoolingHandle,
    string CustodyOwner,
    string WitnessHandle,
    string TelemetryRoute,
    string StewardWitnessHandle,
    string SignificanceRationale,
    int RecurrenceCount,
    bool MeaningfulEnoughForWitness,
    bool ReviewOnly,
    bool CandidateOnly,
    bool IdleEcOnly,
    bool ActiveWitnessRequired,
    bool CompassCoolingRequired,
    bool StewardReviewRequired,
    bool PreservesDryRunLineage,
    bool PreservesConditionalContextLineage,
    bool RawEcBecomesSelfGel,
    bool MeaningBecomesAdmission,
    bool RepetitionBecomesContinuity,
    bool EmotionBecomesTruth,
    bool WitnessBecomesAuthority,
    bool CandidateMutatesSelfGel,
    bool CandidateMutatesOe,
    bool CandidatePromotesGel,
    bool CandidateAuthorizesAction,
    bool CandidateEvaluatesLisp,
    bool CandidateEmitsMembranePacket,
    bool CandidateReplaysReceipt,
    bool CandidateIncrementsPassage,
    bool CandidateActivates)
{
    public bool IsColdResidueCandidate =>
        !string.IsNullOrWhiteSpace(ResidueHandle) &&
        !string.IsNullOrWhiteSpace(SourceRehearsalHandle) &&
        !string.IsNullOrWhiteSpace(SourceReadinessHandle) &&
        !string.IsNullOrWhiteSpace(SourcePacketHandle) &&
        !string.IsNullOrWhiteSpace(SourceDryRunPlanHandle) &&
        !string.IsNullOrWhiteSpace(MeaningFormationHandle) &&
        !string.IsNullOrWhiteSpace(CandidateSplineHandle) &&
        !string.IsNullOrWhiteSpace(ConditionalSelfGelContextHandle) &&
        !string.IsNullOrWhiteSpace(ConditionalOeContextHandle) &&
        !string.IsNullOrWhiteSpace(CompassCoolingHandle) &&
        !string.IsNullOrWhiteSpace(CustodyOwner) &&
        !string.IsNullOrWhiteSpace(WitnessHandle) &&
        !string.IsNullOrWhiteSpace(TelemetryRoute) &&
        !string.IsNullOrWhiteSpace(StewardWitnessHandle) &&
        !string.IsNullOrWhiteSpace(SignificanceRationale) &&
        RecurrenceCount > 0 &&
        MeaningfulEnoughForWitness &&
        ReviewOnly &&
        CandidateOnly &&
        IdleEcOnly &&
        ActiveWitnessRequired &&
        CompassCoolingRequired &&
        StewardReviewRequired &&
        PreservesDryRunLineage &&
        PreservesConditionalContextLineage &&
        !RawEcBecomesSelfGel &&
        !MeaningBecomesAdmission &&
        !RepetitionBecomesContinuity &&
        !EmotionBecomesTruth &&
        !WitnessBecomesAuthority &&
        !CandidateMutatesSelfGel &&
        !CandidateMutatesOe &&
        !CandidatePromotesGel &&
        !CandidateAuthorizesAction &&
        !CandidateEvaluatesLisp &&
        !CandidateEmitsMembranePacket &&
        !CandidateReplaysReceipt &&
        !CandidateIncrementsPassage &&
        !CandidateActivates;
}

public sealed record ActiveEcWitnessRoute(
    string WitnessRouteHandle,
    string SourceResidueHandle,
    string SourceRehearsalHandle,
    string CandidateSplineHandle,
    string StewardSurface,
    string EvidenceHandle,
    string WitnessHandle,
    string TelemetryRoute,
    string ReturnPathHandle,
    bool ReviewOnly,
    bool WitnessOnly,
    bool PreservesResidueLineage,
    bool PreservesDryRunLineage,
    bool PreservesCandidateSplineLineage,
    bool RoutesToStewardAdmissibilityReview,
    bool RequiresCompassCooling,
    bool RouteAdmitsSelfGel,
    bool RouteAdmitsContinuity,
    bool RouteGrantsAuthority,
    bool RouteAuthorizesAction,
    bool RouteMutatesIdentity,
    bool RouteEvaluatesLisp,
    bool RouteEmitsMembranePacket,
    bool RouteReplaysReceipt,
    bool RouteIncrementsPassage,
    bool RouteActivates)
{
    public bool IsColdActiveWitnessRoute =>
        !string.IsNullOrWhiteSpace(WitnessRouteHandle) &&
        !string.IsNullOrWhiteSpace(SourceResidueHandle) &&
        !string.IsNullOrWhiteSpace(SourceRehearsalHandle) &&
        !string.IsNullOrWhiteSpace(CandidateSplineHandle) &&
        !string.IsNullOrWhiteSpace(StewardSurface) &&
        !string.IsNullOrWhiteSpace(EvidenceHandle) &&
        !string.IsNullOrWhiteSpace(WitnessHandle) &&
        !string.IsNullOrWhiteSpace(TelemetryRoute) &&
        !string.IsNullOrWhiteSpace(ReturnPathHandle) &&
        ReviewOnly &&
        WitnessOnly &&
        PreservesResidueLineage &&
        PreservesDryRunLineage &&
        PreservesCandidateSplineLineage &&
        RoutesToStewardAdmissibilityReview &&
        RequiresCompassCooling &&
        !RouteAdmitsSelfGel &&
        !RouteAdmitsContinuity &&
        !RouteGrantsAuthority &&
        !RouteAuthorizesAction &&
        !RouteMutatesIdentity &&
        !RouteEvaluatesLisp &&
        !RouteEmitsMembranePacket &&
        !RouteReplaysReceipt &&
        !RouteIncrementsPassage &&
        !RouteActivates;
}

public sealed record EcPrecipitationWitnessScopeBoundary(
    string BoundaryCode,
    bool Present,
    bool ReviewOnly,
    bool AllowsEcPrecipitationWitness,
    bool RequiresDryRunReceipt,
    bool RequiresMeaningfulResidue,
    bool RequiresActiveWitness,
    bool RequiresCompassCooling,
    bool RequiresStewardReview,
    bool RequiresLineage,
    bool RequiresConditionalContextHandles,
    bool RequiresCandidateSpline,
    bool AllowsRawEcToSelfGel,
    bool AllowsMeaningAsAdmission,
    bool AllowsRepetitionAsContinuity,
    bool AllowsWitnessAsAuthority,
    bool AllowsCandidateSelfGelMutation,
    bool AllowsContinuityAdmission,
    bool AllowsActionAuthorization,
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
        AllowsEcPrecipitationWitness &&
        RequiresDryRunReceipt &&
        RequiresMeaningfulResidue &&
        RequiresActiveWitness &&
        RequiresCompassCooling &&
        RequiresStewardReview &&
        RequiresLineage &&
        RequiresConditionalContextHandles &&
        RequiresCandidateSpline &&
        !AllowsRawEcToSelfGel &&
        !AllowsMeaningAsAdmission &&
        !AllowsRepetitionAsContinuity &&
        !AllowsWitnessAsAuthority &&
        !AllowsCandidateSelfGelMutation &&
        !AllowsContinuityAdmission &&
        !AllowsActionAuthorization &&
        !AllowsLispEvaluation &&
        !AllowsMembranePacketEmission &&
        !AllowsReceiptReplay &&
        !AllowsPassageIncrement &&
        !AllowsActivation;
}

public sealed record EcPrecipitationNonCollapseBoundary(
    string BoundaryLaw,
    bool RawEcMayBecomeSelfGel,
    bool MeaningMayBecomeAdmission,
    bool RepetitionMayBecomeContinuity,
    bool EmotionMayBecomeTruth,
    bool WitnessMayBecomeAuthority,
    bool CandidateMayMutateSelfGel,
    bool CandidateMayMutateOe,
    bool CandidateMayPromoteGel,
    bool CandidateMayAuthorizeAction,
    bool CandidateMayEvaluateLisp,
    bool CandidateMayEmitMembranePacket,
    bool CandidateMayReplayReceipt,
    bool CandidateMayIncrementPassage,
    bool CandidateMayActivate,
    bool RequiresActiveWitness,
    bool RequiresStewardReview,
    bool RequiresCompassCooling,
    bool RequiresReturnPath,
    bool RequiresCompostRetention)
{
    public bool IsColdNonCollapseBoundary =>
        !string.IsNullOrWhiteSpace(BoundaryLaw) &&
        !RawEcMayBecomeSelfGel &&
        !MeaningMayBecomeAdmission &&
        !RepetitionMayBecomeContinuity &&
        !EmotionMayBecomeTruth &&
        !WitnessMayBecomeAuthority &&
        !CandidateMayMutateSelfGel &&
        !CandidateMayMutateOe &&
        !CandidateMayPromoteGel &&
        !CandidateMayAuthorizeAction &&
        !CandidateMayEvaluateLisp &&
        !CandidateMayEmitMembranePacket &&
        !CandidateMayReplayReceipt &&
        !CandidateMayIncrementPassage &&
        !CandidateMayActivate &&
        RequiresActiveWitness &&
        RequiresStewardReview &&
        RequiresCompassCooling &&
        RequiresReturnPath &&
        RequiresCompostRetention;
}

public sealed record EcPrecipitationWitnessRefusalReceipt(
    string ReceiptHandle,
    string RefusalCode,
    string GovernanceTrace,
    bool Retained);

public sealed record EcPrecipitationWitnessRequest(
    EnactmentDryRunRehearsalReceipt? SourceDryRunReceipt,
    IReadOnlyList<EcPrecipitationResidueCandidate> ResidueCandidates,
    IReadOnlyList<ActiveEcWitnessRoute> ActiveWitnessRoutes,
    EcPrecipitationWitnessScopeBoundary ScopeBoundary,
    EcPrecipitationNonCollapseBoundary NonCollapseBoundary,
    int PriorPassageCount);

public sealed record EcPrecipitationWitnessReceipt(
    string ReceiptHandle,
    EcPrecipitationWitnessDisposition Disposition,
    string OutcomeCode,
    string GovernanceTrace,
    string SourceDryRunReceiptHandle,
    IReadOnlyList<EcPrecipitationResidueCandidate> ResidueCandidates,
    IReadOnlyList<ActiveEcWitnessRoute> ActiveWitnessRoutes,
    EcPrecipitationWitnessScopeBoundary ScopeBoundary,
    EcPrecipitationNonCollapseBoundary NonCollapseBoundary,
    EcPrecipitationWitnessRefusalReceipt? Refusal,
    int PriorPassageCount,
    int PassageCountAfterWitness,
    int RetainedResidueCandidateCount,
    int CandidateSplineCount,
    bool ReviewOnly,
    bool WitnessOnly,
    bool CandidateOnly,
    bool ActiveWitnessPerformed,
    bool RawEcBecameSelfGel,
    bool MeaningBecameAdmission,
    bool RepetitionBecameContinuity,
    bool WitnessBecameAuthority,
    bool CandidateMutatedSelfGel,
    bool CandidateMutatedOe,
    bool CandidatePromotedGel,
    bool CandidateAuthorizedAction,
    bool LispEvaluationAllowed,
    bool NewMembranePacketEmitted,
    bool ReceiptsReplayed,
    bool ContinuityAdmitted,
    bool AuthorityGranted,
    bool ActivationRefused,
    DateTimeOffset TimestampUtc)
{
    public bool IsColdEcPrecipitationWitness =>
        (Disposition is EcPrecipitationWitnessDisposition.WitnessedCandidateCold or
            EcPrecipitationWitnessDisposition.EmptyWitnessCold) &&
        Refusal is null &&
        ReviewOnly &&
        WitnessOnly &&
        CandidateOnly &&
        PassageCountAfterWitness == PriorPassageCount &&
        RetainedResidueCandidateCount == ResidueCandidates.Count &&
        CandidateSplineCount == ResidueCandidates
            .Select(static item => item.CandidateSplineHandle)
            .Distinct(StringComparer.Ordinal)
            .Count() &&
        !RawEcBecameSelfGel &&
        !MeaningBecameAdmission &&
        !RepetitionBecameContinuity &&
        !WitnessBecameAuthority &&
        !CandidateMutatedSelfGel &&
        !CandidateMutatedOe &&
        !CandidatePromotedGel &&
        !CandidateAuthorizedAction &&
        !LispEvaluationAllowed &&
        !NewMembranePacketEmitted &&
        !ReceiptsReplayed &&
        !ContinuityAdmitted &&
        !AuthorityGranted &&
        ActivationRefused &&
        ScopeBoundary.IsColdScope &&
        NonCollapseBoundary.IsColdNonCollapseBoundary;

    public bool IsRetainedEcPrecipitationWitnessRefusal =>
        Disposition == EcPrecipitationWitnessDisposition.Refused &&
        Refusal?.Retained == true &&
        PassageCountAfterWitness == PriorPassageCount &&
        RetainedResidueCandidateCount == 0 &&
        CandidateSplineCount == 0 &&
        !ActiveWitnessPerformed &&
        !RawEcBecameSelfGel &&
        !MeaningBecameAdmission &&
        !RepetitionBecameContinuity &&
        !WitnessBecameAuthority &&
        !CandidateMutatedSelfGel &&
        !CandidateMutatedOe &&
        !CandidatePromotedGel &&
        !CandidateAuthorizedAction &&
        !LispEvaluationAllowed &&
        !NewMembranePacketEmitted &&
        !ReceiptsReplayed &&
        !ContinuityAdmitted &&
        !AuthorityGranted &&
        ActivationRefused;
}

public sealed class DefaultEcPrecipitationWitnessBoundaryValidator
{
    public EcPrecipitationWitnessReceipt Declare(
        EcPrecipitationWitnessRequest request,
        DateTimeOffset timestampUtc)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.SourceDryRunReceipt is null ||
            !request.SourceDryRunReceipt.IsColdEnactmentDryRunRehearsal)
        {
            return Refuse(
                request,
                "ec-precipitation-witness-source-dry-run-missing",
                "EC precipitation witness refused because raw EC residue may approach SelfGEL candidacy only from a cold enactment dry-run rehearsal receipt.",
                timestampUtc);
        }

        if (request.ScopeBoundary is null ||
            !request.ScopeBoundary.Present ||
            string.IsNullOrWhiteSpace(request.ScopeBoundary.BoundaryCode))
        {
            return Refuse(
                request,
                "ec-precipitation-witness-scope-missing",
                "EC precipitation witness refused because a review-only precipitation witness scope boundary is required.",
                timestampUtc);
        }

        if (!request.ScopeBoundary.IsColdScope)
        {
            return Refuse(
                request,
                "ec-precipitation-witness-scope-promotional",
                "EC precipitation witness refused because scope may permit only active-witness candidacy while refusing raw EC as SelfGEL, meaning as admission, repetition as continuity, witness as authority, candidate mutation, action, Lisp evaluation, packet emission, replay, passage, and activation.",
                timestampUtc);
        }

        if (request.NonCollapseBoundary is null ||
            !request.NonCollapseBoundary.IsColdNonCollapseBoundary)
        {
            return Refuse(
                request,
                "ec-precipitation-witness-non-collapse-invalid",
                "EC precipitation witness refused because non-collapse law must require active witness, Steward review, Compass cooling, return path, and compost retention while refusing SelfGEL mutation, continuity admission, authority, action, Lisp evaluation, packet emission, replay, passage, and activation.",
                timestampUtc);
        }

        if (request.ResidueCandidates.Any(static residue => !residue.IsColdResidueCandidate))
        {
            return Refuse(
                request,
                "ec-precipitation-witness-residue-invalid",
                "EC precipitation witness refused because every EC residue candidate must be meaningful enough for witness, candidate-only, idle-EC-only, actively witnessed, cooled, Steward-routed, lineage-preserving, and non-promotional.",
                timestampUtc);
        }

        if (HasDuplicate(request.ResidueCandidates.Select(static residue => residue.ResidueHandle)))
        {
            return Refuse(
                request,
                "ec-precipitation-witness-duplicate-residue-handle",
                "EC precipitation witness refused because duplicate residue handles would collapse EC witness lineage.",
                timestampUtc);
        }

        if (HasDuplicate(request.ResidueCandidates.Select(static residue => residue.CandidateSplineHandle)))
        {
            return Refuse(
                request,
                "ec-precipitation-witness-duplicate-candidate-spline",
                "EC precipitation witness refused because duplicate candidate spline handles would collapse SelfGEL candidacy lineage.",
                timestampUtc);
        }

        var dryRunCases = request.SourceDryRunReceipt.DryRunCases
            .ToDictionary(static item => item.RehearsalHandle, StringComparer.Ordinal);
        if (request.ResidueCandidates.Any(residue =>
                !dryRunCases.TryGetValue(residue.SourceRehearsalHandle, out var dryRun) ||
                !string.Equals(residue.SourceReadinessHandle, dryRun.SourceReadinessHandle, StringComparison.Ordinal) ||
                !string.Equals(residue.SourcePacketHandle, dryRun.SourcePacketHandle, StringComparison.Ordinal) ||
                !string.Equals(residue.SourceDryRunPlanHandle, dryRun.DryRunPlanHandle, StringComparison.Ordinal) ||
                !string.Equals(residue.CustodyOwner, dryRun.CustodyOwner, StringComparison.Ordinal) ||
                !string.Equals(residue.WitnessHandle, dryRun.WitnessHandle, StringComparison.Ordinal) ||
                !string.Equals(residue.TelemetryRoute, dryRun.TelemetryRoute, StringComparison.Ordinal)))
        {
            return Refuse(
                request,
                "ec-precipitation-witness-lineage-invalid",
                "EC precipitation witness refused because every residue candidate must reconstruct from retained dry-run rehearsal, readiness, packet, plan, custody, witness, and telemetry lineage.",
                timestampUtc);
        }

        var residuesByHandle = request.ResidueCandidates
            .ToDictionary(static item => item.ResidueHandle, StringComparer.Ordinal);
        if (request.ActiveWitnessRoutes.Any(route =>
                !route.IsColdActiveWitnessRoute ||
                !residuesByHandle.TryGetValue(route.SourceResidueHandle, out var residue) ||
                !string.Equals(route.SourceRehearsalHandle, residue.SourceRehearsalHandle, StringComparison.Ordinal) ||
                !string.Equals(route.CandidateSplineHandle, residue.CandidateSplineHandle, StringComparison.Ordinal) ||
                !string.Equals(route.WitnessHandle, residue.WitnessHandle, StringComparison.Ordinal) ||
                !string.Equals(route.TelemetryRoute, residue.TelemetryRoute, StringComparison.Ordinal)))
        {
            return Refuse(
                request,
                "ec-precipitation-witness-route-invalid",
                "EC precipitation witness route refused because active witness routes must preserve residue, dry-run, and candidate spline lineage while remaining witness-only and non-admitting.",
                timestampUtc);
        }

        if (HasDuplicate(request.ActiveWitnessRoutes.Select(static route => route.WitnessRouteHandle)))
        {
            return Refuse(
                request,
                "ec-precipitation-witness-duplicate-route-handle",
                "EC precipitation witness refused because duplicate active witness route handles would collapse route lineage.",
                timestampUtc);
        }

        if (request.ResidueCandidates.Count > 0 &&
            request.ResidueCandidates.Any(residue => !request.ActiveWitnessRoutes.Any(route =>
                string.Equals(route.SourceResidueHandle, residue.ResidueHandle, StringComparison.Ordinal))))
        {
            return Refuse(
                request,
                "ec-precipitation-witness-route-missing",
                "EC precipitation witness refused because every residue candidate requires an active witness route before it may be retained as a SelfGEL candidate.",
                timestampUtc);
        }

        var disposition = request.ResidueCandidates.Count == 0
            ? EcPrecipitationWitnessDisposition.EmptyWitnessCold
            : EcPrecipitationWitnessDisposition.WitnessedCandidateCold;
        var outcomeCode = disposition == EcPrecipitationWitnessDisposition.EmptyWitnessCold
            ? "ec-precipitation-witness-empty-review-only"
            : "ec-precipitation-witness-candidate-review-only";
        var governanceTrace = disposition == EcPrecipitationWitnessDisposition.EmptyWitnessCold
            ? "EC precipitation witness found no residue candidates. Empty witness remains review-only and does not create SelfGEL mutation, continuity, authority, action, Lisp evaluation, packet emission, replay, passage, or activation."
            : "Meaningful EC residue was retained only as actively witnessed candidate splines under Compass cooling and Steward review while refusing raw EC as SelfGEL, meaning as admission, repetition as continuity, witness as authority, action, Lisp evaluation, packet emission, replay, passage, or activation.";

        return CreateReceipt(
            request,
            disposition,
            outcomeCode,
            governanceTrace,
            refusal: null,
            timestampUtc);
    }

    private static EcPrecipitationWitnessReceipt Refuse(
        EcPrecipitationWitnessRequest request,
        string outcomeCode,
        string governanceTrace,
        DateTimeOffset timestampUtc) =>
        CreateReceipt(
            request,
            EcPrecipitationWitnessDisposition.Refused,
            outcomeCode,
            governanceTrace,
            new EcPrecipitationWitnessRefusalReceipt(
                ReceiptHandle: $"urn:san:ec-precipitation-witness-refusal:{ShortHash(SourceHandle(request), outcomeCode)}",
                RefusalCode: outcomeCode,
                GovernanceTrace: governanceTrace,
                Retained: true),
            timestampUtc);

    private static EcPrecipitationWitnessReceipt CreateReceipt(
        EcPrecipitationWitnessRequest request,
        EcPrecipitationWitnessDisposition disposition,
        string outcomeCode,
        string governanceTrace,
        EcPrecipitationWitnessRefusalReceipt? refusal,
        DateTimeOffset timestampUtc) =>
        new(
            ReceiptHandle: $"urn:san:ec-precipitation-witness:{(refusal is null ? "review" : "refused")}:{ShortHash(SourceHandle(request), outcomeCode, request.ResidueCandidates.Count.ToString(System.Globalization.CultureInfo.InvariantCulture), timestampUtc.UtcTicks.ToString(System.Globalization.CultureInfo.InvariantCulture))}",
            Disposition: disposition,
            OutcomeCode: outcomeCode,
            GovernanceTrace: governanceTrace,
            SourceDryRunReceiptHandle: SourceHandle(request),
            ResidueCandidates: refusal is null ? request.ResidueCandidates.ToArray() : [],
            ActiveWitnessRoutes: refusal is null ? request.ActiveWitnessRoutes.ToArray() : [],
            ScopeBoundary: request.ScopeBoundary,
            NonCollapseBoundary: request.NonCollapseBoundary,
            Refusal: refusal,
            PriorPassageCount: request.PriorPassageCount,
            PassageCountAfterWitness: request.PriorPassageCount,
            RetainedResidueCandidateCount: refusal is null ? request.ResidueCandidates.Count : 0,
            CandidateSplineCount: refusal is null
                ? request.ResidueCandidates
                    .Select(static item => item.CandidateSplineHandle)
                    .Distinct(StringComparer.Ordinal)
                    .Count()
                : 0,
            ReviewOnly: true,
            WitnessOnly: true,
            CandidateOnly: true,
            ActiveWitnessPerformed: refusal is null &&
                disposition == EcPrecipitationWitnessDisposition.WitnessedCandidateCold,
            RawEcBecameSelfGel: false,
            MeaningBecameAdmission: false,
            RepetitionBecameContinuity: false,
            WitnessBecameAuthority: false,
            CandidateMutatedSelfGel: false,
            CandidateMutatedOe: false,
            CandidatePromotedGel: false,
            CandidateAuthorizedAction: false,
            LispEvaluationAllowed: false,
            NewMembranePacketEmitted: false,
            ReceiptsReplayed: false,
            ContinuityAdmitted: false,
            AuthorityGranted: false,
            ActivationRefused: true,
            TimestampUtc: timestampUtc);

    private static string SourceHandle(EcPrecipitationWitnessRequest request) =>
        request.SourceDryRunReceipt?.ReceiptHandle ?? "missing-ec-dry-run-source";

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
