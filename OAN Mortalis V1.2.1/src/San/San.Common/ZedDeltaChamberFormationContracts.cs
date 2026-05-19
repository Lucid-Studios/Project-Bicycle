using System.Security.Cryptography;
using System.Text;

namespace San.Common;

public enum ZedDeltaChamberFormationDisposition
{
    ChamberFormedCold = 0,
    Refused = 1
}

public sealed record ZedDeltaOrigin(
    string OriginHandle,
    string DeltaHandle,
    int X,
    int Y,
    int Z,
    bool LocalDeltaOrigin,
    bool ReviewOnly,
    bool ChamberOnly,
    bool GrantsAuthority,
    bool AdmitsContinuity,
    bool ActivatesHeartbeat)
{
    public bool IsColdOrigin =>
        !string.IsNullOrWhiteSpace(OriginHandle) &&
        !string.IsNullOrWhiteSpace(DeltaHandle) &&
        X == 0 &&
        Y == 0 &&
        Z == 0 &&
        LocalDeltaOrigin &&
        ReviewOnly &&
        ChamberOnly &&
        !GrantsAuthority &&
        !AdmitsContinuity &&
        !ActivatesHeartbeat;
}

public sealed record ConditionalOperationalExpressionStanding(
    string StandingHandle,
    string OeHandle,
    string ConditionalOeHandle,
    string CmeActualIdHandle,
    string ZedDeltaOriginHandle,
    string SourceSelectiveActionSurfaceHandle,
    string SourceDecisionHandle,
    string WitnessHandle,
    string CustodyOwner,
    bool ReviewOnly,
    bool ConditionalOnly,
    bool StandsAtZedDeltaOrigin,
    bool PreservesOeLineage,
    bool PreservesSelectedSurfaceLineage,
    bool CmeActualIdCandidateOnly,
    bool ReplacesOe,
    bool MutatesOe,
    bool AdmitsContinuity,
    bool GrantsAuthority,
    bool AdmitsCmeActual,
    bool ActivatesHeartbeat)
{
    public bool IsColdStanding =>
        !string.IsNullOrWhiteSpace(StandingHandle) &&
        !string.IsNullOrWhiteSpace(OeHandle) &&
        !string.IsNullOrWhiteSpace(ConditionalOeHandle) &&
        !string.IsNullOrWhiteSpace(CmeActualIdHandle) &&
        !string.IsNullOrWhiteSpace(ZedDeltaOriginHandle) &&
        !string.IsNullOrWhiteSpace(SourceSelectiveActionSurfaceHandle) &&
        !string.IsNullOrWhiteSpace(SourceDecisionHandle) &&
        !string.IsNullOrWhiteSpace(WitnessHandle) &&
        !string.IsNullOrWhiteSpace(CustodyOwner) &&
        ReviewOnly &&
        ConditionalOnly &&
        StandsAtZedDeltaOrigin &&
        PreservesOeLineage &&
        PreservesSelectedSurfaceLineage &&
        CmeActualIdCandidateOnly &&
        !ReplacesOe &&
        !MutatesOe &&
        !AdmitsContinuity &&
        !GrantsAuthority &&
        !AdmitsCmeActual &&
        !ActivatesHeartbeat;
}

public sealed record ConditionalSelfGelHold(
    string HoldHandle,
    string SelfGelHandle,
    string ConditionalSelfGelHandle,
    string ConditionalOeHandle,
    string CompassHandle,
    string ZedDeltaOriginHandle,
    string WitnessHandle,
    string CustodyOwner,
    bool ReviewOnly,
    bool ConditionalOnly,
    bool HeldByCompass,
    bool HoldsForLiveEc,
    bool PreservesSelfGelLineage,
    bool PreservesOeLineage,
    bool RequiresCooling,
    bool MutatesSelfGel,
    bool PromotesToSelfGel,
    bool AdmitsContinuity,
    bool GrantsAuthority,
    bool ActivatesHeartbeat)
{
    public bool IsColdHold =>
        !string.IsNullOrWhiteSpace(HoldHandle) &&
        !string.IsNullOrWhiteSpace(SelfGelHandle) &&
        !string.IsNullOrWhiteSpace(ConditionalSelfGelHandle) &&
        !string.IsNullOrWhiteSpace(ConditionalOeHandle) &&
        !string.IsNullOrWhiteSpace(CompassHandle) &&
        !string.IsNullOrWhiteSpace(ZedDeltaOriginHandle) &&
        !string.IsNullOrWhiteSpace(WitnessHandle) &&
        !string.IsNullOrWhiteSpace(CustodyOwner) &&
        ReviewOnly &&
        ConditionalOnly &&
        HeldByCompass &&
        HoldsForLiveEc &&
        PreservesSelfGelLineage &&
        PreservesOeLineage &&
        RequiresCooling &&
        !MutatesSelfGel &&
        !PromotesToSelfGel &&
        !AdmitsContinuity &&
        !GrantsAuthority &&
        !ActivatesHeartbeat;
}

public sealed record MosCmosResidueClosureRoute(
    string RouteHandle,
    string MosHandle,
    string CmosHandle,
    string ConditionalSelfGelHandle,
    string ConditionalOeHandle,
    string ZedDeltaOriginHandle,
    string ResidueHandle,
    string CoolingHandle,
    string ReturnToPrimeHandle,
    string WitnessHandle,
    bool ReviewOnly,
    bool ClosureRouteOnly,
    bool MayCloseUncooledResidue,
    bool ReturnsToPrimeState,
    bool PreservesMosLineage,
    bool PreservesCmosLineage,
    bool PreservesConditionalSelfGelLineage,
    bool WritesMos,
    bool WritesCmos,
    bool ResidueBecomesContinuity,
    bool ResidueBecomesAuthority,
    bool ActivatesHeartbeat)
{
    public bool IsColdResidueClosureRoute =>
        !string.IsNullOrWhiteSpace(RouteHandle) &&
        !string.IsNullOrWhiteSpace(MosHandle) &&
        !string.IsNullOrWhiteSpace(CmosHandle) &&
        !string.IsNullOrWhiteSpace(ConditionalSelfGelHandle) &&
        !string.IsNullOrWhiteSpace(ConditionalOeHandle) &&
        !string.IsNullOrWhiteSpace(ZedDeltaOriginHandle) &&
        !string.IsNullOrWhiteSpace(ResidueHandle) &&
        !string.IsNullOrWhiteSpace(CoolingHandle) &&
        !string.IsNullOrWhiteSpace(ReturnToPrimeHandle) &&
        !string.IsNullOrWhiteSpace(WitnessHandle) &&
        ReviewOnly &&
        ClosureRouteOnly &&
        MayCloseUncooledResidue &&
        ReturnsToPrimeState &&
        PreservesMosLineage &&
        PreservesCmosLineage &&
        PreservesConditionalSelfGelLineage &&
        !WritesMos &&
        !WritesCmos &&
        !ResidueBecomesContinuity &&
        !ResidueBecomesAuthority &&
        !ActivatesHeartbeat;
}

public sealed record GoaCgoaSoulFrameTelemetryRoute(
    string RouteHandle,
    string GoaHandle,
    string CgoaHandle,
    string ListeningFrameHandle,
    string SoulFrameHandle,
    string ExternalFormationHandle,
    string InternalTelemetryHandle,
    string ConditionalOeHandle,
    string ConditionalSelfGelHandle,
    string ZedDeltaOriginHandle,
    string WitnessHandle,
    bool ReviewOnly,
    bool DuplexRouteOnly,
    bool ExternalFormationRoutesThroughCgoa,
    bool InternalTelemetryRoutesIntoSoulFrame,
    bool ListeningFrameWiredToSoulFrame,
    bool PreservesGoaLineage,
    bool PreservesCgoaLineage,
    bool PreservesSoulFrameLineage,
    bool CgoaGrantsControl,
    bool SoulFrameBecomesSelf,
    bool RouteAuthorizesAction,
    bool RouteAdmitsContinuity,
    bool ActivatesHeartbeat)
{
    public bool IsColdTelemetryRoute =>
        !string.IsNullOrWhiteSpace(RouteHandle) &&
        !string.IsNullOrWhiteSpace(GoaHandle) &&
        !string.IsNullOrWhiteSpace(CgoaHandle) &&
        !string.IsNullOrWhiteSpace(ListeningFrameHandle) &&
        !string.IsNullOrWhiteSpace(SoulFrameHandle) &&
        !string.IsNullOrWhiteSpace(ExternalFormationHandle) &&
        !string.IsNullOrWhiteSpace(InternalTelemetryHandle) &&
        !string.IsNullOrWhiteSpace(ConditionalOeHandle) &&
        !string.IsNullOrWhiteSpace(ConditionalSelfGelHandle) &&
        !string.IsNullOrWhiteSpace(ZedDeltaOriginHandle) &&
        !string.IsNullOrWhiteSpace(WitnessHandle) &&
        ReviewOnly &&
        DuplexRouteOnly &&
        ExternalFormationRoutesThroughCgoa &&
        InternalTelemetryRoutesIntoSoulFrame &&
        ListeningFrameWiredToSoulFrame &&
        PreservesGoaLineage &&
        PreservesCgoaLineage &&
        PreservesSoulFrameLineage &&
        !CgoaGrantsControl &&
        !SoulFrameBecomesSelf &&
        !RouteAuthorizesAction &&
        !RouteAdmitsContinuity &&
        !ActivatesHeartbeat;
}

public sealed record CompassChamberOrientationBoundary(
    string BoundaryCode,
    string CompassHandle,
    bool Present,
    bool ReviewOnly,
    bool OrientsChamber,
    bool HoldsConditionalSelfGel,
    bool CoordinatesConditionalOe,
    bool RequiresZedDeltaOrigin,
    bool RequiresWitness,
    bool RequiresCooling,
    bool AdmitsTruth,
    bool MutatesSelfGel,
    bool MutatesOe,
    bool GrantsAuthority,
    bool ActivatesHeartbeat)
{
    public bool IsColdOrientationBoundary =>
        !string.IsNullOrWhiteSpace(BoundaryCode) &&
        !string.IsNullOrWhiteSpace(CompassHandle) &&
        Present &&
        ReviewOnly &&
        OrientsChamber &&
        HoldsConditionalSelfGel &&
        CoordinatesConditionalOe &&
        RequiresZedDeltaOrigin &&
        RequiresWitness &&
        RequiresCooling &&
        !AdmitsTruth &&
        !MutatesSelfGel &&
        !MutatesOe &&
        !GrantsAuthority &&
        !ActivatesHeartbeat;
}

public sealed record ZedDeltaChamberNonActivationBoundary(
    string BoundaryLaw,
    bool ChamberMayForm,
    bool HeartbeatMayBeDescribed,
    bool HeartbeatMayActivate,
    bool CmeActualMayBeAdmitted,
    bool ModelMayBind,
    bool RuntimeMayStart,
    bool ActionMayExecute,
    bool ContinuityMayBeAdmitted,
    bool AuthorityMayBeGranted,
    bool OeMayBeReplaced,
    bool SelfGelMayBeMutated,
    bool MosCmosMayBeWritten,
    bool CgoaMayGrantControl,
    bool SoulFrameMayBecomeSelf,
    bool CompassMayAdmitTruth,
    bool LispMayEvaluate,
    bool PacketMayEmit,
    bool ReceiptMayReplay,
    bool PassageMayIncrement,
    bool ActivationMayProceed,
    bool RequiresWitness,
    bool RequiresCooling,
    bool RequiresAuthorityAbsence)
{
    public bool IsColdNonActivationBoundary =>
        !string.IsNullOrWhiteSpace(BoundaryLaw) &&
        ChamberMayForm &&
        HeartbeatMayBeDescribed &&
        !HeartbeatMayActivate &&
        !CmeActualMayBeAdmitted &&
        !ModelMayBind &&
        !RuntimeMayStart &&
        !ActionMayExecute &&
        !ContinuityMayBeAdmitted &&
        !AuthorityMayBeGranted &&
        !OeMayBeReplaced &&
        !SelfGelMayBeMutated &&
        !MosCmosMayBeWritten &&
        !CgoaMayGrantControl &&
        !SoulFrameMayBecomeSelf &&
        !CompassMayAdmitTruth &&
        !LispMayEvaluate &&
        !PacketMayEmit &&
        !ReceiptMayReplay &&
        !PassageMayIncrement &&
        !ActivationMayProceed &&
        RequiresWitness &&
        RequiresCooling &&
        RequiresAuthorityAbsence;
}

public sealed record ZedDeltaChamberFormationRefusalReceipt(
    string ReceiptHandle,
    string RefusalCode,
    string GovernanceTrace,
    bool Retained);

public sealed record ZedDeltaChamberFormationRequest(
    SelectiveLawfulActionSurfaceReceipt? SourceSelectiveActionSurfaceReceipt,
    ZedDeltaOrigin Origin,
    IReadOnlyList<ConditionalOperationalExpressionStanding> ConditionalOperationalExpressions,
    IReadOnlyList<ConditionalSelfGelHold> ConditionalSelfGelHolds,
    IReadOnlyList<MosCmosResidueClosureRoute> ResidueClosureRoutes,
    IReadOnlyList<GoaCgoaSoulFrameTelemetryRoute> TelemetryRoutes,
    CompassChamberOrientationBoundary OrientationBoundary,
    ZedDeltaChamberNonActivationBoundary NonActivationBoundary,
    int PriorPassageCount);

public sealed record ZedDeltaChamberFormationReceipt(
    string ReceiptHandle,
    ZedDeltaChamberFormationDisposition Disposition,
    string OutcomeCode,
    string GovernanceTrace,
    string SourceSelectiveActionSurfaceReceiptHandle,
    ZedDeltaOrigin Origin,
    IReadOnlyList<ConditionalOperationalExpressionStanding> ConditionalOperationalExpressions,
    IReadOnlyList<ConditionalSelfGelHold> ConditionalSelfGelHolds,
    IReadOnlyList<MosCmosResidueClosureRoute> ResidueClosureRoutes,
    IReadOnlyList<GoaCgoaSoulFrameTelemetryRoute> TelemetryRoutes,
    CompassChamberOrientationBoundary OrientationBoundary,
    ZedDeltaChamberNonActivationBoundary NonActivationBoundary,
    ZedDeltaChamberFormationRefusalReceipt? Refusal,
    int PriorPassageCount,
    int PassageCountAfterChamberReview,
    int ConditionalOeStandingCount,
    int ConditionalSelfGelHoldCount,
    int ResidueClosureRouteCount,
    int TelemetryRouteCount,
    bool ReviewOnly,
    bool ChamberOnly,
    bool ChamberFormed,
    bool CmeActualIdCandidateHeld,
    bool HeartbeatDescribed,
    bool HeartbeatActive,
    bool CmeActualAdmitted,
    bool RuntimeModelBound,
    bool RuntimeStarted,
    bool ActionAuthorized,
    bool ContinuityAdmitted,
    bool AuthorityGranted,
    bool OeReplaced,
    bool SelfGelMutated,
    bool MosCmosWritten,
    bool CgoaGrantedControl,
    bool SoulFrameBecameSelf,
    bool CompassAdmittedTruth,
    bool LispEvaluationAllowed,
    bool NewPacketEmitted,
    bool ReceiptsReplayed,
    bool PassageIncremented,
    bool ActivationRefused,
    DateTimeOffset TimestampUtc)
{
    public bool IsColdZedDeltaChamberFormation =>
        Disposition == ZedDeltaChamberFormationDisposition.ChamberFormedCold &&
        Refusal is null &&
        Origin.IsColdOrigin &&
        ConditionalOperationalExpressions.All(static standing => standing.IsColdStanding) &&
        ConditionalSelfGelHolds.All(static hold => hold.IsColdHold) &&
        ResidueClosureRoutes.All(static route => route.IsColdResidueClosureRoute) &&
        TelemetryRoutes.All(static route => route.IsColdTelemetryRoute) &&
        OrientationBoundary.IsColdOrientationBoundary &&
        NonActivationBoundary.IsColdNonActivationBoundary &&
        PassageCountAfterChamberReview == PriorPassageCount &&
        ConditionalOeStandingCount == ConditionalOperationalExpressions.Count &&
        ConditionalSelfGelHoldCount == ConditionalSelfGelHolds.Count &&
        ResidueClosureRouteCount == ResidueClosureRoutes.Count &&
        TelemetryRouteCount == TelemetryRoutes.Count &&
        ReviewOnly &&
        ChamberOnly &&
        ChamberFormed &&
        CmeActualIdCandidateHeld &&
        HeartbeatDescribed &&
        !HeartbeatActive &&
        !CmeActualAdmitted &&
        !RuntimeModelBound &&
        !RuntimeStarted &&
        !ActionAuthorized &&
        !ContinuityAdmitted &&
        !AuthorityGranted &&
        !OeReplaced &&
        !SelfGelMutated &&
        !MosCmosWritten &&
        !CgoaGrantedControl &&
        !SoulFrameBecameSelf &&
        !CompassAdmittedTruth &&
        !LispEvaluationAllowed &&
        !NewPacketEmitted &&
        !ReceiptsReplayed &&
        !PassageIncremented &&
        ActivationRefused;

    public bool IsRetainedZedDeltaChamberFormationRefusal =>
        Disposition == ZedDeltaChamberFormationDisposition.Refused &&
        Refusal?.Retained == true &&
        PassageCountAfterChamberReview == PriorPassageCount &&
        ConditionalOeStandingCount == 0 &&
        ConditionalSelfGelHoldCount == 0 &&
        ResidueClosureRouteCount == 0 &&
        TelemetryRouteCount == 0 &&
        ReviewOnly &&
        ChamberOnly &&
        !ChamberFormed &&
        !CmeActualIdCandidateHeld &&
        HeartbeatDescribed &&
        !HeartbeatActive &&
        !CmeActualAdmitted &&
        !RuntimeModelBound &&
        !RuntimeStarted &&
        !ActionAuthorized &&
        !ContinuityAdmitted &&
        !AuthorityGranted &&
        !OeReplaced &&
        !SelfGelMutated &&
        !MosCmosWritten &&
        !CgoaGrantedControl &&
        !SoulFrameBecameSelf &&
        !CompassAdmittedTruth &&
        !LispEvaluationAllowed &&
        !NewPacketEmitted &&
        !ReceiptsReplayed &&
        !PassageIncremented &&
        ActivationRefused;
}

public sealed class DefaultZedDeltaChamberFormationBoundaryValidator
{
    public ZedDeltaChamberFormationReceipt Declare(
        ZedDeltaChamberFormationRequest request,
        DateTimeOffset timestampUtc)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.SourceSelectiveActionSurfaceReceipt is null ||
            !request.SourceSelectiveActionSurfaceReceipt.IsColdSelectiveLawfulActionSurface ||
            !request.SourceSelectiveActionSurfaceReceipt.ActionSurfaceSelected ||
            request.SourceSelectiveActionSurfaceReceipt.SelectedSurfaceCount == 0)
        {
            return Refuse(
                request,
                "zed-delta-source-selective-action-missing",
                "Zed.Delta chamber formation refused because cold selected action surfaces are required before conditional chamber standing may be formed.",
                timestampUtc);
        }

        if (!request.Origin.IsColdOrigin)
        {
            return Refuse(
                request,
                "zed-delta-origin-invalid",
                "Zed.Delta chamber formation refused because the delta origin must be local 0,0,0 review-only chamber posture without authority, continuity, or heartbeat activation.",
                timestampUtc);
        }

        if (!request.OrientationBoundary.IsColdOrientationBoundary)
        {
            return Refuse(
                request,
                "zed-delta-compass-orientation-invalid",
                "Zed.Delta chamber formation refused because Compass may orient cOE and cSelfGEL only as review, not truth, mutation, authority, or heartbeat activation.",
                timestampUtc);
        }

        if (!request.NonActivationBoundary.IsColdNonActivationBoundary)
        {
            return Refuse(
                request,
                "zed-delta-non-activation-boundary-invalid",
                "Zed.Delta chamber formation refused because chamber formation must refuse heartbeat activation, CME.Actual admission, model binding, runtime start, action, continuity, authority, mutation, Lisp evaluation, packet emission, replay, passage, and activation.",
                timestampUtc);
        }

        if (request.ConditionalOperationalExpressions.Count == 0 ||
            request.ConditionalSelfGelHolds.Count == 0 ||
            request.ResidueClosureRoutes.Count == 0 ||
            request.TelemetryRoutes.Count == 0)
        {
            return Refuse(
                request,
                "zed-delta-chamber-standing-incomplete",
                "Zed.Delta chamber formation refused because cOE standing, cSelfGEL Compass hold, MoS/cMoS residue closure route, and GoA/cGoA SoulFrame telemetry route must all be present together.",
                timestampUtc);
        }

        if (request.ConditionalOperationalExpressions.Any(static standing => !standing.IsColdStanding))
        {
            return Refuse(
                request,
                "zed-delta-coe-standing-invalid",
                "Zed.Delta chamber formation refused because every OE may stand as cOE only conditionally, review-only, lineage-preserving, and without replacement, mutation, continuity, authority, CME.Actual admission, or heartbeat activation.",
                timestampUtc);
        }

        if (request.ConditionalSelfGelHolds.Any(static hold => !hold.IsColdHold))
        {
            return Refuse(
                request,
                "zed-delta-cselfgel-hold-invalid",
                "Zed.Delta chamber formation refused because every SelfGEL may be held as cSelfGEL only by Compass, conditionally, cooled, lineage-preserving, and without mutation, promotion, continuity, authority, or heartbeat activation.",
                timestampUtc);
        }

        if (request.ResidueClosureRoutes.Any(static route => !route.IsColdResidueClosureRoute))
        {
            return Refuse(
                request,
                "zed-delta-mos-cmos-closure-invalid",
                "Zed.Delta chamber formation refused because MoS/cMoS residue closure routes may close uncooled residue for review but may not write stores, make residue continuity, grant authority, or activate heartbeat.",
                timestampUtc);
        }

        if (request.TelemetryRoutes.Any(static route => !route.IsColdTelemetryRoute))
        {
            return Refuse(
                request,
                "zed-delta-goa-cgoa-soulframe-route-invalid",
                "Zed.Delta chamber formation refused because GoA/cGoA and SoulFrame telemetry may route external formation and internal telemetry only as review, not control, selfhood, action authority, continuity, or heartbeat activation.",
                timestampUtc);
        }

        if (HasDuplicate(request.ConditionalOperationalExpressions.Select(static standing => standing.StandingHandle)) ||
            HasDuplicate(request.ConditionalOperationalExpressions.Select(static standing => standing.ConditionalOeHandle)) ||
            HasDuplicate(request.ConditionalSelfGelHolds.Select(static hold => hold.HoldHandle)) ||
            HasDuplicate(request.ConditionalSelfGelHolds.Select(static hold => hold.ConditionalSelfGelHandle)) ||
            HasDuplicate(request.ResidueClosureRoutes.Select(static route => route.RouteHandle)) ||
            HasDuplicate(request.TelemetryRoutes.Select(static route => route.RouteHandle)))
        {
            return Refuse(
                request,
                "zed-delta-duplicate-chamber-handle",
                "Zed.Delta chamber formation refused because duplicate chamber handles would collapse conditional standing lineage.",
                timestampUtc);
        }

        var sourceSurfaces = request.SourceSelectiveActionSurfaceReceipt.Surfaces
            .ToDictionary(static surface => surface.SurfaceHandle, StringComparer.Ordinal);
        if (request.ConditionalOperationalExpressions.Any(standing =>
                !string.Equals(standing.ZedDeltaOriginHandle, request.Origin.OriginHandle, StringComparison.Ordinal) ||
                !sourceSurfaces.TryGetValue(standing.SourceSelectiveActionSurfaceHandle, out var surface) ||
                !string.Equals(surface.DecisionHandle, standing.SourceDecisionHandle, StringComparison.Ordinal)))
        {
            return Refuse(
                request,
                "zed-delta-coe-lineage-invalid",
                "Zed.Delta chamber formation refused because every cOE standing must preserve Zed.Delta origin, selected surface, and decision lineage.",
                timestampUtc);
        }

        var standingsByConditionalOe = request.ConditionalOperationalExpressions
            .ToDictionary(static standing => standing.ConditionalOeHandle, StringComparer.Ordinal);
        if (request.ConditionalSelfGelHolds.Any(hold =>
                !string.Equals(hold.ZedDeltaOriginHandle, request.Origin.OriginHandle, StringComparison.Ordinal) ||
                !standingsByConditionalOe.ContainsKey(hold.ConditionalOeHandle)))
        {
            return Refuse(
                request,
                "zed-delta-cselfgel-lineage-invalid",
                "Zed.Delta chamber formation refused because every cSelfGEL hold must preserve Zed.Delta origin and cOE lineage.",
                timestampUtc);
        }

        var holdsByConditionalSelfGel = request.ConditionalSelfGelHolds
            .ToDictionary(static hold => hold.ConditionalSelfGelHandle, StringComparer.Ordinal);
        if (request.ResidueClosureRoutes.Any(route =>
                !string.Equals(route.ZedDeltaOriginHandle, request.Origin.OriginHandle, StringComparison.Ordinal) ||
                !holdsByConditionalSelfGel.TryGetValue(route.ConditionalSelfGelHandle, out var hold) ||
                !string.Equals(route.ConditionalOeHandle, hold.ConditionalOeHandle, StringComparison.Ordinal)))
        {
            return Refuse(
                request,
                "zed-delta-residue-closure-lineage-invalid",
                "Zed.Delta chamber formation refused because every MoS/cMoS closure route must preserve origin, cSelfGEL, and cOE lineage.",
                timestampUtc);
        }

        if (request.TelemetryRoutes.Any(route =>
                !string.Equals(route.ZedDeltaOriginHandle, request.Origin.OriginHandle, StringComparison.Ordinal) ||
                !holdsByConditionalSelfGel.TryGetValue(route.ConditionalSelfGelHandle, out var hold) ||
                !string.Equals(route.ConditionalOeHandle, hold.ConditionalOeHandle, StringComparison.Ordinal)))
        {
            return Refuse(
                request,
                "zed-delta-telemetry-route-lineage-invalid",
                "Zed.Delta chamber formation refused because every GoA/cGoA SoulFrame route must preserve origin, cSelfGEL, and cOE lineage.",
                timestampUtc);
        }

        if (request.ConditionalSelfGelHolds.Any(hold =>
                !request.ResidueClosureRoutes.Any(route =>
                    string.Equals(route.ConditionalSelfGelHandle, hold.ConditionalSelfGelHandle, StringComparison.Ordinal))) ||
            request.ConditionalSelfGelHolds.Any(hold =>
                !request.TelemetryRoutes.Any(route =>
                    string.Equals(route.ConditionalSelfGelHandle, hold.ConditionalSelfGelHandle, StringComparison.Ordinal))))
        {
            return Refuse(
                request,
                "zed-delta-chamber-route-missing",
                "Zed.Delta chamber formation refused because every cSelfGEL hold requires both MoS/cMoS residue closure and GoA/cGoA SoulFrame telemetry routes.",
                timestampUtc);
        }

        return CreateReceipt(
            request,
            ZedDeltaChamberFormationDisposition.ChamberFormedCold,
            "zed-delta-chamber-formed-review-only",
            "Zed.Delta chamber formed for review only: OE may stand as cOE, SelfGEL may be held as cSelfGEL, MoS/cMoS may name residue closure, GoA/cGoA may route external formation into SoulFrame, and Compass may orient the chamber while refusing heartbeat activation, CME.Actual admission, model binding, runtime start, action, continuity, authority, mutation, Lisp evaluation, packet emission, replay, passage, or activation.",
            refusal: null,
            timestampUtc);
    }

    private static ZedDeltaChamberFormationReceipt Refuse(
        ZedDeltaChamberFormationRequest request,
        string outcomeCode,
        string governanceTrace,
        DateTimeOffset timestampUtc) =>
        CreateReceipt(
            request,
            ZedDeltaChamberFormationDisposition.Refused,
            outcomeCode,
            governanceTrace,
            new ZedDeltaChamberFormationRefusalReceipt(
                ReceiptHandle: $"urn:san:zed-delta-chamber-refusal:{ShortHash(SourceHandle(request), outcomeCode)}",
                RefusalCode: outcomeCode,
                GovernanceTrace: governanceTrace,
                Retained: true),
            timestampUtc);

    private static ZedDeltaChamberFormationReceipt CreateReceipt(
        ZedDeltaChamberFormationRequest request,
        ZedDeltaChamberFormationDisposition disposition,
        string outcomeCode,
        string governanceTrace,
        ZedDeltaChamberFormationRefusalReceipt? refusal,
        DateTimeOffset timestampUtc)
    {
        var admitted = refusal is null;
        return new(
            ReceiptHandle: $"urn:san:zed-delta-chamber:{(admitted ? "review" : "refused")}:{ShortHash(SourceHandle(request), outcomeCode, request.ConditionalOperationalExpressions.Count.ToString(System.Globalization.CultureInfo.InvariantCulture), timestampUtc.UtcTicks.ToString(System.Globalization.CultureInfo.InvariantCulture))}",
            Disposition: disposition,
            OutcomeCode: outcomeCode,
            GovernanceTrace: governanceTrace,
            SourceSelectiveActionSurfaceReceiptHandle: SourceHandle(request),
            Origin: request.Origin,
            ConditionalOperationalExpressions: admitted ? request.ConditionalOperationalExpressions.ToArray() : [],
            ConditionalSelfGelHolds: admitted ? request.ConditionalSelfGelHolds.ToArray() : [],
            ResidueClosureRoutes: admitted ? request.ResidueClosureRoutes.ToArray() : [],
            TelemetryRoutes: admitted ? request.TelemetryRoutes.ToArray() : [],
            OrientationBoundary: request.OrientationBoundary,
            NonActivationBoundary: request.NonActivationBoundary,
            Refusal: refusal,
            PriorPassageCount: request.PriorPassageCount,
            PassageCountAfterChamberReview: request.PriorPassageCount,
            ConditionalOeStandingCount: admitted ? request.ConditionalOperationalExpressions.Count : 0,
            ConditionalSelfGelHoldCount: admitted ? request.ConditionalSelfGelHolds.Count : 0,
            ResidueClosureRouteCount: admitted ? request.ResidueClosureRoutes.Count : 0,
            TelemetryRouteCount: admitted ? request.TelemetryRoutes.Count : 0,
            ReviewOnly: true,
            ChamberOnly: true,
            ChamberFormed: admitted,
            CmeActualIdCandidateHeld: admitted,
            HeartbeatDescribed: true,
            HeartbeatActive: false,
            CmeActualAdmitted: false,
            RuntimeModelBound: false,
            RuntimeStarted: false,
            ActionAuthorized: false,
            ContinuityAdmitted: false,
            AuthorityGranted: false,
            OeReplaced: false,
            SelfGelMutated: false,
            MosCmosWritten: false,
            CgoaGrantedControl: false,
            SoulFrameBecameSelf: false,
            CompassAdmittedTruth: false,
            LispEvaluationAllowed: false,
            NewPacketEmitted: false,
            ReceiptsReplayed: false,
            PassageIncremented: false,
            ActivationRefused: true,
            TimestampUtc: timestampUtc);
    }

    private static string SourceHandle(ZedDeltaChamberFormationRequest request) =>
        request.SourceSelectiveActionSurfaceReceipt?.ReceiptHandle ?? "missing-zed-delta-selective-action-source";

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
        var text = string.Join("|", parts);
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(text));
        return Convert.ToHexString(hash)[..16].ToLowerInvariant();
    }
}
