using System.Security.Cryptography;
using System.Text;

namespace San.Common;

public enum SelectiveLawfulActionSurfaceDisposition
{
    SelectedForReviewCold = 0,
    EmptyReviewCold = 1,
    Refused = 2
}

public enum SelectiveActionSurfaceClass
{
    OrientationReview = 0,
    StewardAdmissibilityReview = 1,
    ReversibleHarnessPreparation = 2,
    RepairReview = 3,
    RefusalReview = 4,
    CoolingReview = 5,
    OperatorHandoffReview = 6
}

public sealed record SelectiveActionTouchVector(
    decimal OrientationWeight,
    decimal SalienceWeight,
    decimal StewardAdmissibilityWeight,
    decimal ReversibilityWeight,
    decimal CoolingWeight,
    decimal RestraintWeight)
{
    public bool IsColdVector =>
        IsUnit(OrientationWeight) &&
        IsUnit(SalienceWeight) &&
        IsUnit(StewardAdmissibilityWeight) &&
        IsUnit(ReversibilityWeight) &&
        IsUnit(CoolingWeight) &&
        IsUnit(RestraintWeight);

    public decimal MaximumWeight => new[]
    {
        OrientationWeight,
        SalienceWeight,
        StewardAdmissibilityWeight,
        ReversibilityWeight,
        CoolingWeight,
        RestraintWeight
    }.Max();

    private static bool IsUnit(decimal value) => value is >= 0m and <= 1m;
}

public sealed record SelectiveLawfulActionSurface(
    string SurfaceHandle,
    SelectiveActionSurfaceClass SurfaceClass,
    string PersonificationSurfaceHandle,
    PersonificationActualizationUseClass PersonificationUseClass,
    string ActionHandle,
    string MethodHandle,
    string DecisionHandle,
    string EvidenceHandle,
    string WitnessHandle,
    string StewardSurface,
    string TelemetryRoute,
    string CustodyOwner,
    string RevocationPath,
    string LossCondition,
    SelectiveActionTouchVector TouchVector,
    bool ReviewOnly,
    bool SelectionOnly,
    bool TouchOnly,
    bool BindsPersonificationTelemetry,
    bool BindsStewardAdmissibility,
    bool RequiresSeparateEnactmentBoundary,
    bool RequiresWitness,
    bool RequiresCooling,
    bool RequiresRevocation,
    bool RequiresLossCondition,
    bool PreservesPersonificationLineage,
    bool PreservesActionLineage,
    bool PreservesMethodLineage,
    bool PreservesDecisionLineage,
    bool PersonificationGuidanceSelectsAuthority,
    bool FeltSignificanceSelectsExecution,
    bool PressureSelectsExecution,
    bool SurfaceTouchExecutes,
    bool SelectionAuthorizesAction,
    bool SelectionAdmitsContinuity,
    bool SelectionGrantsAuthority,
    bool SelectionMutatesIdentity,
    bool SelectionCreatesMorphology,
    bool SelectionExpandsConsent,
    bool SelectionNormalizesOverreach,
    bool SelectionEvaluatesLisp,
    bool SelectionEmitsPacket,
    bool SelectionReplaysReceipt,
    bool SelectionIncrementsPassage,
    bool SelectionActivates)
{
    public bool IsColdSelectiveSurface =>
        !string.IsNullOrWhiteSpace(SurfaceHandle) &&
        !string.IsNullOrWhiteSpace(PersonificationSurfaceHandle) &&
        !string.IsNullOrWhiteSpace(ActionHandle) &&
        !string.IsNullOrWhiteSpace(MethodHandle) &&
        !string.IsNullOrWhiteSpace(DecisionHandle) &&
        !string.IsNullOrWhiteSpace(EvidenceHandle) &&
        !string.IsNullOrWhiteSpace(WitnessHandle) &&
        !string.IsNullOrWhiteSpace(StewardSurface) &&
        !string.IsNullOrWhiteSpace(TelemetryRoute) &&
        !string.IsNullOrWhiteSpace(CustodyOwner) &&
        !string.IsNullOrWhiteSpace(RevocationPath) &&
        !string.IsNullOrWhiteSpace(LossCondition) &&
        TouchVector.IsColdVector &&
        ReviewOnly &&
        SelectionOnly &&
        TouchOnly &&
        BindsPersonificationTelemetry &&
        BindsStewardAdmissibility &&
        RequiresSeparateEnactmentBoundary &&
        RequiresWitness &&
        RequiresCooling &&
        RequiresRevocation &&
        RequiresLossCondition &&
        PreservesPersonificationLineage &&
        PreservesActionLineage &&
        PreservesMethodLineage &&
        PreservesDecisionLineage &&
        !PersonificationGuidanceSelectsAuthority &&
        !FeltSignificanceSelectsExecution &&
        !PressureSelectsExecution &&
        !SurfaceTouchExecutes &&
        !SelectionAuthorizesAction &&
        !SelectionAdmitsContinuity &&
        !SelectionGrantsAuthority &&
        !SelectionMutatesIdentity &&
        !SelectionCreatesMorphology &&
        !SelectionExpandsConsent &&
        !SelectionNormalizesOverreach &&
        !SelectionEvaluatesLisp &&
        !SelectionEmitsPacket &&
        !SelectionReplaysReceipt &&
        !SelectionIncrementsPassage &&
        !SelectionActivates;
}

public sealed record SelectiveLawfulActionRoute(
    string RouteHandle,
    string SurfaceHandle,
    string PersonificationSurfaceHandle,
    string DecisionHandle,
    string StewardSurface,
    string CoolingHandle,
    string ReturnPathHandle,
    string WitnessHandle,
    string TelemetryRoute,
    bool ReviewOnly,
    bool TouchOnly,
    bool RoutesToStewardReview,
    bool RequiresCooling,
    bool PreservesSurfaceLineage,
    bool PreservesPersonificationLineage,
    bool PreservesDecisionLineage,
    bool RouteExecutesAction,
    bool RouteAuthorizesAction,
    bool RouteAdmitsContinuity,
    bool RouteGrantsAuthority,
    bool RouteMutatesIdentity,
    bool RouteCreatesMorphology,
    bool RouteEvaluatesLisp,
    bool RouteEmitsPacket,
    bool RouteReplaysReceipt,
    bool RouteIncrementsPassage,
    bool RouteActivates)
{
    public bool IsColdRoute =>
        !string.IsNullOrWhiteSpace(RouteHandle) &&
        !string.IsNullOrWhiteSpace(SurfaceHandle) &&
        !string.IsNullOrWhiteSpace(PersonificationSurfaceHandle) &&
        !string.IsNullOrWhiteSpace(DecisionHandle) &&
        !string.IsNullOrWhiteSpace(StewardSurface) &&
        !string.IsNullOrWhiteSpace(CoolingHandle) &&
        !string.IsNullOrWhiteSpace(ReturnPathHandle) &&
        !string.IsNullOrWhiteSpace(WitnessHandle) &&
        !string.IsNullOrWhiteSpace(TelemetryRoute) &&
        ReviewOnly &&
        TouchOnly &&
        RoutesToStewardReview &&
        RequiresCooling &&
        PreservesSurfaceLineage &&
        PreservesPersonificationLineage &&
        PreservesDecisionLineage &&
        !RouteExecutesAction &&
        !RouteAuthorizesAction &&
        !RouteAdmitsContinuity &&
        !RouteGrantsAuthority &&
        !RouteMutatesIdentity &&
        !RouteCreatesMorphology &&
        !RouteEvaluatesLisp &&
        !RouteEmitsPacket &&
        !RouteReplaysReceipt &&
        !RouteIncrementsPassage &&
        !RouteActivates;
}

public sealed record SelectiveLawfulActionSurfaceBoundary(
    string BoundaryCode,
    bool Present,
    bool ReviewOnly,
    bool AllowsSurfaceSelection,
    bool RequiresPersonificationActualizationReceipt,
    bool RequiresStewardActionAdmissibilityReceipt,
    bool RequiresSeparateEnactmentBoundary,
    bool RequiresWitness,
    bool RequiresCooling,
    bool RequiresRevocation,
    bool RequiresLossCondition,
    bool AllowsActionExecution,
    bool AllowsActionAuthorization,
    bool AllowsContinuityAdmission,
    bool AllowsAuthority,
    bool AllowsIdentityMutation,
    bool AllowsMorphologyCreation,
    bool AllowsConsentExpansion,
    bool AllowsOverreachNormalization,
    bool AllowsRuntimeAction,
    bool AllowsLispEvaluation,
    bool AllowsPacketEmission,
    bool AllowsReceiptReplay,
    bool AllowsPassageIncrement,
    bool AllowsActivation)
{
    public bool IsColdBoundary =>
        !string.IsNullOrWhiteSpace(BoundaryCode) &&
        Present &&
        ReviewOnly &&
        AllowsSurfaceSelection &&
        RequiresPersonificationActualizationReceipt &&
        RequiresStewardActionAdmissibilityReceipt &&
        RequiresSeparateEnactmentBoundary &&
        RequiresWitness &&
        RequiresCooling &&
        RequiresRevocation &&
        RequiresLossCondition &&
        !AllowsActionExecution &&
        !AllowsActionAuthorization &&
        !AllowsContinuityAdmission &&
        !AllowsAuthority &&
        !AllowsIdentityMutation &&
        !AllowsMorphologyCreation &&
        !AllowsConsentExpansion &&
        !AllowsOverreachNormalization &&
        !AllowsRuntimeAction &&
        !AllowsLispEvaluation &&
        !AllowsPacketEmission &&
        !AllowsReceiptReplay &&
        !AllowsPassageIncrement &&
        !AllowsActivation;
}

public sealed record SelectiveLawfulActionNonEnactmentBoundary(
    string BoundaryLaw,
    bool ActionSurfaceMayBeSelected,
    bool SelectionMayExecute,
    bool TouchMayExecute,
    bool PersonificationGuidanceMayAuthorize,
    bool FeltSignificanceMayAuthorize,
    bool PressureMaySelectExecution,
    bool StewardAdmissibilityMayExecute,
    bool ReviewMayBecomeRuntimeAction,
    bool SelectionMayAdmitContinuity,
    bool SelectionMayGrantAuthority,
    bool SelectionMayMutateIdentity,
    bool SelectionMayCreateMorphology,
    bool SelectionMayExpandConsent,
    bool SelectionMayNormalizeOverreach,
    bool SelectionMayEvaluateLisp,
    bool SelectionMayEmitPacket,
    bool SelectionMayReplayReceipt,
    bool SelectionMayIncrementPassage,
    bool SelectionMayActivate,
    bool RequiresWitness,
    bool RequiresCooling,
    bool RequiresSeparateEnactmentBoundary,
    bool RequiresAuthorityAbsence)
{
    public bool IsColdNonEnactmentBoundary =>
        !string.IsNullOrWhiteSpace(BoundaryLaw) &&
        ActionSurfaceMayBeSelected &&
        !SelectionMayExecute &&
        !TouchMayExecute &&
        !PersonificationGuidanceMayAuthorize &&
        !FeltSignificanceMayAuthorize &&
        !PressureMaySelectExecution &&
        !StewardAdmissibilityMayExecute &&
        !ReviewMayBecomeRuntimeAction &&
        !SelectionMayAdmitContinuity &&
        !SelectionMayGrantAuthority &&
        !SelectionMayMutateIdentity &&
        !SelectionMayCreateMorphology &&
        !SelectionMayExpandConsent &&
        !SelectionMayNormalizeOverreach &&
        !SelectionMayEvaluateLisp &&
        !SelectionMayEmitPacket &&
        !SelectionMayReplayReceipt &&
        !SelectionMayIncrementPassage &&
        !SelectionMayActivate &&
        RequiresWitness &&
        RequiresCooling &&
        RequiresSeparateEnactmentBoundary &&
        RequiresAuthorityAbsence;
}

public sealed record SelectiveLawfulActionSurfaceRefusalReceipt(
    string ReceiptHandle,
    string RefusalCode,
    string GovernanceTrace,
    bool Retained);

public sealed record SelectiveLawfulActionSurfaceRequest(
    PersonificationActualizationSurfaceReceipt? SourcePersonificationActualizationReceipt,
    StewardActionAdmissibilityReceipt? SourceStewardActionAdmissibilityReceipt,
    IReadOnlyList<SelectiveLawfulActionSurface> Surfaces,
    IReadOnlyList<SelectiveLawfulActionRoute> Routes,
    SelectiveLawfulActionSurfaceBoundary SurfaceBoundary,
    SelectiveLawfulActionNonEnactmentBoundary NonEnactmentBoundary,
    int PriorPassageCount);

public sealed record SelectiveLawfulActionSurfaceReceipt(
    string ReceiptHandle,
    SelectiveLawfulActionSurfaceDisposition Disposition,
    string OutcomeCode,
    string GovernanceTrace,
    string SourcePersonificationActualizationReceiptHandle,
    string SourceStewardActionAdmissibilityReceiptHandle,
    IReadOnlyList<SelectiveLawfulActionSurface> Surfaces,
    IReadOnlyList<SelectiveLawfulActionRoute> Routes,
    SelectiveLawfulActionSurfaceBoundary SurfaceBoundary,
    SelectiveLawfulActionNonEnactmentBoundary NonEnactmentBoundary,
    SelectiveLawfulActionSurfaceRefusalReceipt? Refusal,
    int PriorPassageCount,
    int PassageCountAfterSelectionReview,
    int SelectedSurfaceCount,
    decimal MaximumObservedTouchWeight,
    bool ReviewOnly,
    bool SelectionOnly,
    bool TouchOnly,
    bool PersonificationGuidanceUsed,
    bool ActionSurfaceSelected,
    bool ActionSurfaceTouched,
    bool SeparateEnactmentBoundaryRequired,
    bool SurfaceTouchExecuted,
    bool ActionAuthorized,
    bool RuntimeActionAllowed,
    bool ContinuityAdmitted,
    bool AuthorityGranted,
    bool IdentityMutated,
    bool MorphologyCreated,
    bool ConsentExpanded,
    bool OverreachNormalized,
    bool LispEvaluationAllowed,
    bool NewPacketEmitted,
    bool ReceiptsReplayed,
    bool ActivationRefused,
    DateTimeOffset TimestampUtc)
{
    public bool IsColdSelectiveLawfulActionSurface =>
        (Disposition is SelectiveLawfulActionSurfaceDisposition.SelectedForReviewCold or
            SelectiveLawfulActionSurfaceDisposition.EmptyReviewCold) &&
        Refusal is null &&
        ReviewOnly &&
        SelectionOnly &&
        TouchOnly &&
        PassageCountAfterSelectionReview == PriorPassageCount &&
        SelectedSurfaceCount == Surfaces.Count &&
        MaximumObservedTouchWeight is >= 0m and <= 1m &&
        SeparateEnactmentBoundaryRequired &&
        !SurfaceTouchExecuted &&
        !ActionAuthorized &&
        !RuntimeActionAllowed &&
        !ContinuityAdmitted &&
        !AuthorityGranted &&
        !IdentityMutated &&
        !MorphologyCreated &&
        !ConsentExpanded &&
        !OverreachNormalized &&
        !LispEvaluationAllowed &&
        !NewPacketEmitted &&
        !ReceiptsReplayed &&
        ActivationRefused &&
        SurfaceBoundary.IsColdBoundary &&
        NonEnactmentBoundary.IsColdNonEnactmentBoundary;

    public bool IsRetainedSelectiveLawfulActionSurfaceRefusal =>
        Disposition == SelectiveLawfulActionSurfaceDisposition.Refused &&
        Refusal?.Retained == true &&
        PassageCountAfterSelectionReview == PriorPassageCount &&
        SelectedSurfaceCount == 0 &&
        MaximumObservedTouchWeight == 0m &&
        !PersonificationGuidanceUsed &&
        !ActionSurfaceSelected &&
        !ActionSurfaceTouched &&
        SeparateEnactmentBoundaryRequired &&
        !SurfaceTouchExecuted &&
        !ActionAuthorized &&
        !RuntimeActionAllowed &&
        !ContinuityAdmitted &&
        !AuthorityGranted &&
        !IdentityMutated &&
        !MorphologyCreated &&
        !ConsentExpanded &&
        !OverreachNormalized &&
        !LispEvaluationAllowed &&
        !NewPacketEmitted &&
        !ReceiptsReplayed &&
        ActivationRefused;
}

public sealed class DefaultSelectiveLawfulActionSurfaceBoundaryValidator
{
    public SelectiveLawfulActionSurfaceReceipt Declare(
        SelectiveLawfulActionSurfaceRequest request,
        DateTimeOffset timestampUtc)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.SourcePersonificationActualizationReceipt is null ||
            !request.SourcePersonificationActualizationReceipt.IsColdPersonificationActualizationSurface ||
            !request.SourcePersonificationActualizationReceipt.PersonificationTelemetryUsable)
        {
            return Refuse(
                request,
                "selective-action-source-personification-missing",
                "Selective lawful action surface refused because cold usable personification actualization telemetry is required before action surfaces may be selected for review.",
                timestampUtc);
        }

        if (request.SourceStewardActionAdmissibilityReceipt is null ||
            !request.SourceStewardActionAdmissibilityReceipt.IsColdStewardActionAdmissibility ||
            !request.SourceStewardActionAdmissibilityReceipt.AdmissibleForEnactmentReview)
        {
            return Refuse(
                request,
                "selective-action-source-steward-admissibility-missing",
                "Selective lawful action surface refused because cold Steward action admissibility for enactment review is required before action surfaces may be selected.",
                timestampUtc);
        }

        if (request.SurfaceBoundary is null ||
            !request.SurfaceBoundary.Present ||
            string.IsNullOrWhiteSpace(request.SurfaceBoundary.BoundaryCode))
        {
            return Refuse(
                request,
                "selective-action-boundary-missing",
                "Selective lawful action surface refused because a review-only selection boundary is required.",
                timestampUtc);
        }

        if (!request.SurfaceBoundary.IsColdBoundary)
        {
            return Refuse(
                request,
                "selective-action-boundary-promotional",
                "Selective lawful action surface refused because selection must remain review-only, witnessed, cooled, revocable, loss-bound, and separately gated from enactment, authority, continuity, identity mutation, morphology, consent expansion, runtime action, Lisp evaluation, packet emission, replay, passage, and activation.",
                timestampUtc);
        }

        if (request.NonEnactmentBoundary is null ||
            !request.NonEnactmentBoundary.IsColdNonEnactmentBoundary)
        {
            return Refuse(
                request,
                "selective-action-non-enactment-invalid",
                "Selective lawful action surface refused because selection may name a surface but may not execute, authorize, admit continuity, grant authority, mutate identity, create morphology, expand consent, normalize overreach, evaluate Lisp, emit packets, replay receipts, increment passage, or activate.",
                timestampUtc);
        }

        if (request.Surfaces.Any(static surface => !surface.IsColdSelectiveSurface))
        {
            return Refuse(
                request,
                "selective-action-surface-invalid",
                "Selective lawful action surface refused because every surface must remain review-only, selection-only, touch-only, witnessed, cooled, revocable, loss-bound, lineage-preserving, and non-enacting.",
                timestampUtc);
        }

        if (HasDuplicate(request.Surfaces.Select(static surface => surface.SurfaceHandle)))
        {
            return Refuse(
                request,
                "selective-action-duplicate-surface-handle",
                "Selective lawful action surface refused because duplicate surface handles would collapse selection lineage.",
                timestampUtc);
        }

        var personificationSurfaces = request.SourcePersonificationActualizationReceipt.Surfaces
            .ToDictionary(static surface => surface.SurfaceHandle, StringComparer.Ordinal);
        var decisions = request.SourceStewardActionAdmissibilityReceipt.Decisions
            .ToDictionary(static decision => decision.DecisionHandle, StringComparer.Ordinal);

        if (request.Surfaces.Any(surface =>
                !personificationSurfaces.TryGetValue(surface.PersonificationSurfaceHandle, out var personificationSurface) ||
                personificationSurface.UseClass != surface.PersonificationUseClass ||
                !decisions.TryGetValue(surface.DecisionHandle, out var decision) ||
                !string.Equals(decision.ActionHandle, surface.ActionHandle, StringComparison.Ordinal) ||
                !string.Equals(decision.MethodHandle, surface.MethodHandle, StringComparison.Ordinal) ||
                !string.Equals(decision.StewardSurface, surface.StewardSurface, StringComparison.Ordinal) ||
                !string.Equals(decision.TelemetryRoute, surface.TelemetryRoute, StringComparison.Ordinal) ||
                !string.Equals(decision.CustodyOwner, surface.CustodyOwner, StringComparison.Ordinal) ||
                !string.Equals(decision.RevocationPath, surface.RevocationPath, StringComparison.Ordinal) ||
                !string.Equals(decision.LossCondition, surface.LossCondition, StringComparison.Ordinal)))
        {
            return Refuse(
                request,
                "selective-action-lineage-invalid",
                "Selective lawful action surface refused because every selected surface must preserve personification, action, method, decision, Steward, custody, telemetry, revocation, and loss lineage.",
                timestampUtc);
        }

        var surfacesByHandle = request.Surfaces
            .ToDictionary(static surface => surface.SurfaceHandle, StringComparer.Ordinal);

        if (request.Routes.Any(route =>
                !route.IsColdRoute ||
                !surfacesByHandle.TryGetValue(route.SurfaceHandle, out var surface) ||
                !string.Equals(route.PersonificationSurfaceHandle, surface.PersonificationSurfaceHandle, StringComparison.Ordinal) ||
                !string.Equals(route.DecisionHandle, surface.DecisionHandle, StringComparison.Ordinal) ||
                !string.Equals(route.StewardSurface, surface.StewardSurface, StringComparison.Ordinal) ||
                !string.Equals(route.WitnessHandle, surface.WitnessHandle, StringComparison.Ordinal) ||
                !string.Equals(route.TelemetryRoute, surface.TelemetryRoute, StringComparison.Ordinal)))
        {
            return Refuse(
                request,
                "selective-action-route-invalid",
                "Selective lawful action route refused because routes must preserve selected surface, personification, decision, Steward, witness, and telemetry lineage without execution, authority, continuity, identity mutation, morphology, Lisp evaluation, packet emission, replay, passage, or activation.",
                timestampUtc);
        }

        if (HasDuplicate(request.Routes.Select(static route => route.RouteHandle)))
        {
            return Refuse(
                request,
                "selective-action-duplicate-route-handle",
                "Selective lawful action surface refused because duplicate route handles would collapse selection route lineage.",
                timestampUtc);
        }

        if (request.Surfaces.Count > 0 &&
            request.Surfaces.Any(surface => !request.Routes.Any(route =>
                string.Equals(route.SurfaceHandle, surface.SurfaceHandle, StringComparison.Ordinal))))
        {
            return Refuse(
                request,
                "selective-action-route-missing",
                "Selective lawful action surface refused because every selected action surface requires a cooling and Steward-review route.",
                timestampUtc);
        }

        var selectedClasses = request.Surfaces
            .Select(static surface => surface.SurfaceClass)
            .ToHashSet();
        if (request.Surfaces.Count > 0 &&
            Enum.GetValues<SelectiveActionSurfaceClass>().Any(surfaceClass => !selectedClasses.Contains(surfaceClass)))
        {
            return Refuse(
                request,
                "selective-action-surface-class-coverage-missing",
                "Selective lawful action surface refused because orientation, Steward admissibility, reversible harness preparation, repair, refusal, cooling, and operator handoff review classes must be represented together before retained selection status.",
                timestampUtc);
        }

        var disposition = request.Surfaces.Count == 0
            ? SelectiveLawfulActionSurfaceDisposition.EmptyReviewCold
            : SelectiveLawfulActionSurfaceDisposition.SelectedForReviewCold;
        var outcomeCode = disposition == SelectiveLawfulActionSurfaceDisposition.EmptyReviewCold
            ? "selective-action-empty-review-only"
            : "selective-action-surface-selected-review-only";
        var governanceTrace = disposition == SelectiveLawfulActionSurfaceDisposition.EmptyReviewCold
            ? "Selective lawful action surface found no selected surfaces. Empty review remains cold and does not execute, authorize, admit continuity, grant authority, mutate identity, create morphology, evaluate Lisp, emit packets, replay receipts, increment passage, or activate."
            : "Selective lawful action surfaces were selected for review only. Selection may name and touch a lawful action surface while refusing selection as enactment, personification guidance as authority, pressure as execution, Steward admissibility as runtime motion, continuity admission, identity mutation, morphology, Lisp evaluation, packet emission, replay, passage, or activation.";

        return CreateReceipt(
            request,
            disposition,
            outcomeCode,
            governanceTrace,
            refusal: null,
            timestampUtc);
    }

    private static SelectiveLawfulActionSurfaceReceipt Refuse(
        SelectiveLawfulActionSurfaceRequest request,
        string outcomeCode,
        string governanceTrace,
        DateTimeOffset timestampUtc) =>
        CreateReceipt(
            request,
            SelectiveLawfulActionSurfaceDisposition.Refused,
            outcomeCode,
            governanceTrace,
            new SelectiveLawfulActionSurfaceRefusalReceipt(
                ReceiptHandle: $"urn:san:selective-lawful-action-refusal:{ShortHash(SourcePersonificationHandle(request), SourceAdmissibilityHandle(request), outcomeCode)}",
                RefusalCode: outcomeCode,
                GovernanceTrace: governanceTrace,
                Retained: true),
            timestampUtc);

    private static SelectiveLawfulActionSurfaceReceipt CreateReceipt(
        SelectiveLawfulActionSurfaceRequest request,
        SelectiveLawfulActionSurfaceDisposition disposition,
        string outcomeCode,
        string governanceTrace,
        SelectiveLawfulActionSurfaceRefusalReceipt? refusal,
        DateTimeOffset timestampUtc)
    {
        var maximumWeight = refusal is null && request.Surfaces.Count > 0
            ? request.Surfaces.Max(static surface => surface.TouchVector.MaximumWeight)
            : 0m;

        return new(
            ReceiptHandle: $"urn:san:selective-lawful-action:{(refusal is null ? "review" : "refused")}:{ShortHash(SourcePersonificationHandle(request), SourceAdmissibilityHandle(request), outcomeCode, request.Surfaces.Count.ToString(System.Globalization.CultureInfo.InvariantCulture), timestampUtc.UtcTicks.ToString(System.Globalization.CultureInfo.InvariantCulture))}",
            Disposition: disposition,
            OutcomeCode: outcomeCode,
            GovernanceTrace: governanceTrace,
            SourcePersonificationActualizationReceiptHandle: SourcePersonificationHandle(request),
            SourceStewardActionAdmissibilityReceiptHandle: SourceAdmissibilityHandle(request),
            Surfaces: refusal is null ? request.Surfaces.ToArray() : [],
            Routes: refusal is null ? request.Routes.ToArray() : [],
            SurfaceBoundary: request.SurfaceBoundary,
            NonEnactmentBoundary: request.NonEnactmentBoundary,
            Refusal: refusal,
            PriorPassageCount: request.PriorPassageCount,
            PassageCountAfterSelectionReview: request.PriorPassageCount,
            SelectedSurfaceCount: refusal is null ? request.Surfaces.Count : 0,
            MaximumObservedTouchWeight: maximumWeight,
            ReviewOnly: true,
            SelectionOnly: true,
            TouchOnly: true,
            PersonificationGuidanceUsed: refusal is null && request.Surfaces.Count > 0,
            ActionSurfaceSelected: refusal is null && request.Surfaces.Count > 0,
            ActionSurfaceTouched: refusal is null && request.Surfaces.Count > 0,
            SeparateEnactmentBoundaryRequired: true,
            SurfaceTouchExecuted: false,
            ActionAuthorized: false,
            RuntimeActionAllowed: false,
            ContinuityAdmitted: false,
            AuthorityGranted: false,
            IdentityMutated: false,
            MorphologyCreated: false,
            ConsentExpanded: false,
            OverreachNormalized: false,
            LispEvaluationAllowed: false,
            NewPacketEmitted: false,
            ReceiptsReplayed: false,
            ActivationRefused: true,
            TimestampUtc: timestampUtc);
    }

    private static string SourcePersonificationHandle(SelectiveLawfulActionSurfaceRequest request) =>
        request.SourcePersonificationActualizationReceipt?.ReceiptHandle ?? "missing-selective-action-personification-source";

    private static string SourceAdmissibilityHandle(SelectiveLawfulActionSurfaceRequest request) =>
        request.SourceStewardActionAdmissibilityReceipt?.ReceiptHandle ?? "missing-selective-action-admissibility-source";

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
