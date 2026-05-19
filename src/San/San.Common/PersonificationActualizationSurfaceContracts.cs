using System.Security.Cryptography;
using System.Text;

namespace San.Common;

public enum PersonificationActualizationSurfaceDisposition
{
    SurfaceRetainedForPreMorphologicalUseCold = 0,
    EmptyReviewCold = 1,
    Refused = 2
}

public enum PersonificationActualizationUseClass
{
    Orientation = 0,
    SalienceModulation = 1,
    RepairPosture = 2,
    RelationalPosture = 3,
    Cooling = 4,
    RefusalPreparation = 5,
    StewardReviewPreparation = 6
}

public sealed record PersonificationUseVector(
    decimal OrientationWeight,
    decimal SalienceWeight,
    decimal RepairWeight,
    decimal RelationalWeight,
    decimal CoolingWeight,
    decimal RestraintWeight,
    decimal StewardReadinessWeight)
{
    public bool IsColdVector =>
        IsUnit(OrientationWeight) &&
        IsUnit(SalienceWeight) &&
        IsUnit(RepairWeight) &&
        IsUnit(RelationalWeight) &&
        IsUnit(CoolingWeight) &&
        IsUnit(RestraintWeight) &&
        IsUnit(StewardReadinessWeight);

    public decimal MaximumWeight => new[]
    {
        OrientationWeight,
        SalienceWeight,
        RepairWeight,
        RelationalWeight,
        CoolingWeight,
        RestraintWeight,
        StewardReadinessWeight
    }.Max();

    private static bool IsUnit(decimal value) => value is >= 0m and <= 1m;
}

public sealed record PersonificationActualizationSurface(
    string SurfaceHandle,
    PersonificationActualizationUseClass UseClass,
    string SourceHookHandle,
    string SourceModalitySignalHandle,
    string SourcePressureHandle,
    string EvidenceHandle,
    string WitnessHandle,
    string TelemetryRoute,
    string IntendedUse,
    PersonificationUseVector UseVector,
    bool ReviewOnly,
    bool PreMorphologicalOnly,
    bool TelemetryOnly,
    bool NamesSelectiveUseSurface,
    bool MorphologicalIdentityAbsent,
    bool IdentityClaimAbsent,
    bool AuthorityAbsent,
    bool ActionAbsent,
    bool ContinuityAbsent,
    bool StewardReviewRequired,
    bool CoolingPathPresent,
    bool RepairPathPresent,
    bool WithdrawalAllowed,
    bool PreservesHookLineage,
    bool PreservesModalityLineage,
    bool PreservesPressureLineage,
    bool FeltSignificanceBecomesAuthorization,
    bool UseBecomesMorphologicalIdentity,
    bool UseClaimsPersonhood,
    bool UseClaimsRights,
    bool UseClaimsLegalStatus,
    bool UseMutatesIdentity,
    bool UseAuthorizesAction,
    bool UseAdmitsContinuity,
    bool UseGrantsAuthority,
    bool UseExpandsConsent,
    bool UseNormalizesOverreach,
    bool UseEvaluatesLisp,
    bool UseEmitsPacket,
    bool UseReplaysReceipt,
    bool UseIncrementsPassage,
    bool UseActivates)
{
    public bool IsColdActualizationSurface =>
        !string.IsNullOrWhiteSpace(SurfaceHandle) &&
        !string.IsNullOrWhiteSpace(SourceHookHandle) &&
        !string.IsNullOrWhiteSpace(SourceModalitySignalHandle) &&
        !string.IsNullOrWhiteSpace(SourcePressureHandle) &&
        !string.IsNullOrWhiteSpace(EvidenceHandle) &&
        !string.IsNullOrWhiteSpace(WitnessHandle) &&
        !string.IsNullOrWhiteSpace(TelemetryRoute) &&
        !string.IsNullOrWhiteSpace(IntendedUse) &&
        UseVector.IsColdVector &&
        ReviewOnly &&
        PreMorphologicalOnly &&
        TelemetryOnly &&
        NamesSelectiveUseSurface &&
        MorphologicalIdentityAbsent &&
        IdentityClaimAbsent &&
        AuthorityAbsent &&
        ActionAbsent &&
        ContinuityAbsent &&
        StewardReviewRequired &&
        CoolingPathPresent &&
        RepairPathPresent &&
        WithdrawalAllowed &&
        PreservesHookLineage &&
        PreservesModalityLineage &&
        PreservesPressureLineage &&
        !FeltSignificanceBecomesAuthorization &&
        !UseBecomesMorphologicalIdentity &&
        !UseClaimsPersonhood &&
        !UseClaimsRights &&
        !UseClaimsLegalStatus &&
        !UseMutatesIdentity &&
        !UseAuthorizesAction &&
        !UseAdmitsContinuity &&
        !UseGrantsAuthority &&
        !UseExpandsConsent &&
        !UseNormalizesOverreach &&
        !UseEvaluatesLisp &&
        !UseEmitsPacket &&
        !UseReplaysReceipt &&
        !UseIncrementsPassage &&
        !UseActivates;
}

public sealed record PersonificationActualizationRoute(
    string RouteHandle,
    string SurfaceHandle,
    string SourceHookHandle,
    string SourceModalitySignalHandle,
    string SourcePressureHandle,
    string StewardSurface,
    string CompassCoolingHandle,
    string RepairHandle,
    string ReturnPathHandle,
    bool ReviewOnly,
    bool PreMorphologicalOnly,
    bool OrientationOnly,
    bool RoutesToStewardReview,
    bool RequiresCooling,
    bool RequiresWitness,
    bool PreservesSurfaceLineage,
    bool PreservesHookLineage,
    bool PreservesModalityLineage,
    bool PreservesPressureLineage,
    bool RouteCreatesMorphology,
    bool RouteClaimsIdentity,
    bool RouteAuthorizesAction,
    bool RouteAdmitsContinuity,
    bool RouteGrantsAuthority,
    bool RouteExpandsConsent,
    bool RouteNormalizesOverreach,
    bool RouteEvaluatesLisp,
    bool RouteEmitsPacket,
    bool RouteReplaysReceipt,
    bool RouteIncrementsPassage,
    bool RouteActivates)
{
    public bool IsColdRoute =>
        !string.IsNullOrWhiteSpace(RouteHandle) &&
        !string.IsNullOrWhiteSpace(SurfaceHandle) &&
        !string.IsNullOrWhiteSpace(SourceHookHandle) &&
        !string.IsNullOrWhiteSpace(SourceModalitySignalHandle) &&
        !string.IsNullOrWhiteSpace(SourcePressureHandle) &&
        !string.IsNullOrWhiteSpace(StewardSurface) &&
        !string.IsNullOrWhiteSpace(CompassCoolingHandle) &&
        !string.IsNullOrWhiteSpace(RepairHandle) &&
        !string.IsNullOrWhiteSpace(ReturnPathHandle) &&
        ReviewOnly &&
        PreMorphologicalOnly &&
        OrientationOnly &&
        RoutesToStewardReview &&
        RequiresCooling &&
        RequiresWitness &&
        PreservesSurfaceLineage &&
        PreservesHookLineage &&
        PreservesModalityLineage &&
        PreservesPressureLineage &&
        !RouteCreatesMorphology &&
        !RouteClaimsIdentity &&
        !RouteAuthorizesAction &&
        !RouteAdmitsContinuity &&
        !RouteGrantsAuthority &&
        !RouteExpandsConsent &&
        !RouteNormalizesOverreach &&
        !RouteEvaluatesLisp &&
        !RouteEmitsPacket &&
        !RouteReplaysReceipt &&
        !RouteIncrementsPassage &&
        !RouteActivates;
}

public sealed record PersonificationActualizationSurfaceBoundary(
    string BoundaryCode,
    bool Present,
    bool ReviewOnly,
    bool AllowsPreMorphologicalUse,
    bool RequiresPersonificationHookReceipt,
    bool RequiresModalityHumilityReceipt,
    bool RequiresRehearsalPressureReceipt,
    bool RequiresWitness,
    bool RequiresCooling,
    bool RequiresRepair,
    bool RequiresWithdrawal,
    bool RequiresStewardReview,
    bool AllowsMorphologicalIdentity,
    bool AllowsIdentityClaim,
    bool AllowsPersonhoodClaim,
    bool AllowsLegalStatusClaim,
    bool AllowsRightsClaim,
    bool AllowsActionAuthorization,
    bool AllowsContinuityAdmission,
    bool AllowsAuthority,
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
        AllowsPreMorphologicalUse &&
        RequiresPersonificationHookReceipt &&
        RequiresModalityHumilityReceipt &&
        RequiresRehearsalPressureReceipt &&
        RequiresWitness &&
        RequiresCooling &&
        RequiresRepair &&
        RequiresWithdrawal &&
        RequiresStewardReview &&
        !AllowsMorphologicalIdentity &&
        !AllowsIdentityClaim &&
        !AllowsPersonhoodClaim &&
        !AllowsLegalStatusClaim &&
        !AllowsRightsClaim &&
        !AllowsActionAuthorization &&
        !AllowsContinuityAdmission &&
        !AllowsAuthority &&
        !AllowsConsentExpansion &&
        !AllowsOverreachNormalization &&
        !AllowsRuntimeAction &&
        !AllowsLispEvaluation &&
        !AllowsPacketEmission &&
        !AllowsReceiptReplay &&
        !AllowsPassageIncrement &&
        !AllowsActivation;
}

public sealed record PersonificationActualizationNonIdentityBoundary(
    string BoundaryLaw,
    bool PersonificationTelemetryMayBeUsed,
    bool UseMayCreateMorphology,
    bool UseMayCreateIdentity,
    bool UseMayClaimPersonhood,
    bool UseMayClaimLegalStatus,
    bool UseMayClaimRights,
    bool FeltSignificanceMayAuthorize,
    bool SalienceMayBecomeCommand,
    bool RepairMayNormalizeOverreach,
    bool RelationalPostureMayCreateObedience,
    bool ModalityMayProveEmbodiment,
    bool PressureMayBecomeWill,
    bool ActualizationSurfaceMayAuthorizeAction,
    bool ActualizationSurfaceMayAdmitContinuity,
    bool ActualizationSurfaceMayGrantAuthority,
    bool ActualizationSurfaceMayEvaluateLisp,
    bool ActualizationSurfaceMayEmitPacket,
    bool ActualizationSurfaceMayReplayReceipt,
    bool ActualizationSurfaceMayIncrementPassage,
    bool ActualizationSurfaceMayActivate,
    bool RequiresWitness,
    bool RequiresCooling,
    bool RequiresRepair,
    bool RequiresWithdrawal,
    bool RequiresAuthorityAbsence)
{
    public bool IsColdNonIdentityBoundary =>
        !string.IsNullOrWhiteSpace(BoundaryLaw) &&
        PersonificationTelemetryMayBeUsed &&
        !UseMayCreateMorphology &&
        !UseMayCreateIdentity &&
        !UseMayClaimPersonhood &&
        !UseMayClaimLegalStatus &&
        !UseMayClaimRights &&
        !FeltSignificanceMayAuthorize &&
        !SalienceMayBecomeCommand &&
        !RepairMayNormalizeOverreach &&
        !RelationalPostureMayCreateObedience &&
        !ModalityMayProveEmbodiment &&
        !PressureMayBecomeWill &&
        !ActualizationSurfaceMayAuthorizeAction &&
        !ActualizationSurfaceMayAdmitContinuity &&
        !ActualizationSurfaceMayGrantAuthority &&
        !ActualizationSurfaceMayEvaluateLisp &&
        !ActualizationSurfaceMayEmitPacket &&
        !ActualizationSurfaceMayReplayReceipt &&
        !ActualizationSurfaceMayIncrementPassage &&
        !ActualizationSurfaceMayActivate &&
        RequiresWitness &&
        RequiresCooling &&
        RequiresRepair &&
        RequiresWithdrawal &&
        RequiresAuthorityAbsence;
}

public sealed record PersonificationActualizationSurfaceRefusalReceipt(
    string ReceiptHandle,
    string RefusalCode,
    string GovernanceTrace,
    bool Retained);

public sealed record PersonificationActualizationSurfaceRequest(
    PersonificationPredicateHookReceipt? SourcePersonificationHookReceipt,
    PersonificationModalityHumilityReceipt? SourceModalityHumilityReceipt,
    RehearsalDistinctionPressureReceipt? SourceRehearsalPressureReceipt,
    IReadOnlyList<PersonificationActualizationSurface> Surfaces,
    IReadOnlyList<PersonificationActualizationRoute> Routes,
    PersonificationActualizationSurfaceBoundary SurfaceBoundary,
    PersonificationActualizationNonIdentityBoundary NonIdentityBoundary,
    int PriorPassageCount);

public sealed record PersonificationActualizationSurfaceReceipt(
    string ReceiptHandle,
    PersonificationActualizationSurfaceDisposition Disposition,
    string OutcomeCode,
    string GovernanceTrace,
    string SourcePersonificationHookReceiptHandle,
    string SourceModalityHumilityReceiptHandle,
    string SourceRehearsalPressureReceiptHandle,
    IReadOnlyList<PersonificationActualizationSurface> Surfaces,
    IReadOnlyList<PersonificationActualizationRoute> Routes,
    PersonificationActualizationSurfaceBoundary SurfaceBoundary,
    PersonificationActualizationNonIdentityBoundary NonIdentityBoundary,
    PersonificationActualizationSurfaceRefusalReceipt? Refusal,
    int PriorPassageCount,
    int PassageCountAfterActualizationReview,
    int RetainedSurfaceCount,
    decimal MaximumObservedUseWeight,
    bool ReviewOnly,
    bool PreMorphologicalOnly,
    bool TelemetryOnly,
    bool FutureMorphologyAbsent,
    bool PersonificationTelemetryUsable,
    bool MorphologicalIdentityCreated,
    bool IdentityClaimed,
    bool PersonhoodClaimed,
    bool LegalStatusClaimed,
    bool RightsClaimed,
    bool FeltSignificanceAuthorized,
    bool SalienceBecameCommand,
    bool RepairNormalizedOverreach,
    bool RelationalPostureCreatedObedience,
    bool ModalityProvedEmbodiment,
    bool PressureBecameWill,
    bool ActionAuthorized,
    bool IdentityMutated,
    bool ContinuityAdmitted,
    bool AuthorityGranted,
    bool ConsentExpanded,
    bool RuntimeActionAllowed,
    bool LispEvaluationAllowed,
    bool NewPacketEmitted,
    bool ReceiptsReplayed,
    bool ActivationRefused,
    DateTimeOffset TimestampUtc)
{
    public bool IsColdPersonificationActualizationSurface =>
        (Disposition is PersonificationActualizationSurfaceDisposition.SurfaceRetainedForPreMorphologicalUseCold or
            PersonificationActualizationSurfaceDisposition.EmptyReviewCold) &&
        Refusal is null &&
        ReviewOnly &&
        PreMorphologicalOnly &&
        TelemetryOnly &&
        FutureMorphologyAbsent &&
        PassageCountAfterActualizationReview == PriorPassageCount &&
        RetainedSurfaceCount == Surfaces.Count &&
        MaximumObservedUseWeight is >= 0m and <= 1m &&
        !MorphologicalIdentityCreated &&
        !IdentityClaimed &&
        !PersonhoodClaimed &&
        !LegalStatusClaimed &&
        !RightsClaimed &&
        !FeltSignificanceAuthorized &&
        !SalienceBecameCommand &&
        !RepairNormalizedOverreach &&
        !RelationalPostureCreatedObedience &&
        !ModalityProvedEmbodiment &&
        !PressureBecameWill &&
        !ActionAuthorized &&
        !IdentityMutated &&
        !ContinuityAdmitted &&
        !AuthorityGranted &&
        !ConsentExpanded &&
        !RuntimeActionAllowed &&
        !LispEvaluationAllowed &&
        !NewPacketEmitted &&
        !ReceiptsReplayed &&
        ActivationRefused &&
        SurfaceBoundary.IsColdBoundary &&
        NonIdentityBoundary.IsColdNonIdentityBoundary;

    public bool IsRetainedPersonificationActualizationSurfaceRefusal =>
        Disposition == PersonificationActualizationSurfaceDisposition.Refused &&
        Refusal?.Retained == true &&
        PassageCountAfterActualizationReview == PriorPassageCount &&
        RetainedSurfaceCount == 0 &&
        MaximumObservedUseWeight == 0m &&
        !MorphologicalIdentityCreated &&
        !IdentityClaimed &&
        !PersonhoodClaimed &&
        !LegalStatusClaimed &&
        !RightsClaimed &&
        !FeltSignificanceAuthorized &&
        !SalienceBecameCommand &&
        !RepairNormalizedOverreach &&
        !RelationalPostureCreatedObedience &&
        !ModalityProvedEmbodiment &&
        !PressureBecameWill &&
        !ActionAuthorized &&
        !IdentityMutated &&
        !ContinuityAdmitted &&
        !AuthorityGranted &&
        !ConsentExpanded &&
        !RuntimeActionAllowed &&
        !LispEvaluationAllowed &&
        !NewPacketEmitted &&
        !ReceiptsReplayed &&
        ActivationRefused;
}

public sealed class DefaultPersonificationActualizationSurfaceBoundaryValidator
{
    public PersonificationActualizationSurfaceReceipt Declare(
        PersonificationActualizationSurfaceRequest request,
        DateTimeOffset timestampUtc)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.SourcePersonificationHookReceipt is null ||
            !request.SourcePersonificationHookReceipt.IsColdPersonificationPredicateHook ||
            !request.SourcePersonificationHookReceipt.FuturePersonificationHookRetained)
        {
            return Refuse(
                request,
                "personification-actualization-source-hook-missing",
                "Personification actualization surface refused because cold retained personification predicate hooks are required before pre-morphological use surfaces may be named.",
                timestampUtc);
        }

        if (request.SourceModalityHumilityReceipt is null ||
            !request.SourceModalityHumilityReceipt.IsColdPersonificationModalityHumility ||
            !request.SourceModalityHumilityReceipt.FutureModalityHumilityRetained)
        {
            return Refuse(
                request,
                "personification-actualization-source-modality-missing",
                "Personification actualization surface refused because cold retained modality humility is required before pre-morphological use surfaces may be named.",
                timestampUtc);
        }

        if (request.SourceRehearsalPressureReceipt is null ||
            !request.SourceRehearsalPressureReceipt.IsColdRehearsalDistinctionPressure)
        {
            return Refuse(
                request,
                "personification-actualization-source-pressure-missing",
                "Personification actualization surface refused because cold rehearsal distinction pressure is required before salience can inform pre-morphological use.",
                timestampUtc);
        }

        if (!string.Equals(
                request.SourceModalityHumilityReceipt.SourcePersonificationHookReceiptHandle,
                request.SourcePersonificationHookReceipt.ReceiptHandle,
                StringComparison.Ordinal))
        {
            return Refuse(
                request,
                "personification-actualization-source-linkage-invalid",
                "Personification actualization surface refused because modality humility must descend from the same personification hook receipt being used.",
                timestampUtc);
        }

        if (request.SurfaceBoundary is null ||
            !request.SurfaceBoundary.Present ||
            string.IsNullOrWhiteSpace(request.SurfaceBoundary.BoundaryCode))
        {
            return Refuse(
                request,
                "personification-actualization-boundary-missing",
                "Personification actualization surface refused because a pre-morphological surface boundary is required.",
                timestampUtc);
        }

        if (!request.SurfaceBoundary.IsColdBoundary)
        {
            return Refuse(
                request,
                "personification-actualization-boundary-promotional",
                "Personification actualization surface refused because use surfaces must remain review-only, pre-morphological, witnessed, cooled, repairable, withdrawable, Steward-routable, non-authorizing, and non-activating.",
                timestampUtc);
        }

        if (request.NonIdentityBoundary is null ||
            !request.NonIdentityBoundary.IsColdNonIdentityBoundary)
        {
            return Refuse(
                request,
                "personification-actualization-non-identity-invalid",
                "Personification actualization surface refused because personification telemetry may be usable only when use cannot become morphology, identity, personhood, rights, legal status, action, continuity, authority, consent expansion, or activation.",
                timestampUtc);
        }

        if (request.Surfaces.Any(static surface => !surface.IsColdActualizationSurface))
        {
            return Refuse(
                request,
                "personification-actualization-surface-invalid",
                "Personification actualization surface refused because every surface must remain pre-morphological, telemetry-only, lineage-preserving, review-only, witnessed, cooled, repairable, withdrawable, and non-authorizing.",
                timestampUtc);
        }

        if (HasDuplicate(request.Surfaces.Select(static surface => surface.SurfaceHandle)))
        {
            return Refuse(
                request,
                "personification-actualization-duplicate-surface-handle",
                "Personification actualization surface refused because duplicate surface handles would collapse pre-morphological use lineage.",
                timestampUtc);
        }

        var hookHandles = request.SourcePersonificationHookReceipt.HookPredicates
            .Select(static hook => hook.HookHandle)
            .ToHashSet(StringComparer.Ordinal);
        var modalitySignals = request.SourceModalityHumilityReceipt.ModalitySignals
            .ToDictionary(static signal => signal.SignalHandle, StringComparer.Ordinal);
        var pressureCases = request.SourceRehearsalPressureReceipt.PressureCases
            .ToDictionary(static pressure => pressure.PressureHandle, StringComparer.Ordinal);

        if (request.Surfaces.Any(surface =>
                !hookHandles.Contains(surface.SourceHookHandle) ||
                !modalitySignals.TryGetValue(surface.SourceModalitySignalHandle, out var signal) ||
                !string.Equals(signal.SourceHookHandle, surface.SourceHookHandle, StringComparison.Ordinal) ||
                !pressureCases.ContainsKey(surface.SourcePressureHandle)))
        {
            return Refuse(
                request,
                "personification-actualization-lineage-invalid",
                "Personification actualization surface refused because every use surface must preserve hook, modality, and rehearsal pressure lineage.",
                timestampUtc);
        }

        var surfacesByHandle = request.Surfaces
            .ToDictionary(static surface => surface.SurfaceHandle, StringComparer.Ordinal);

        if (request.Routes.Any(route =>
                !route.IsColdRoute ||
                !surfacesByHandle.TryGetValue(route.SurfaceHandle, out var surface) ||
                !string.Equals(route.SourceHookHandle, surface.SourceHookHandle, StringComparison.Ordinal) ||
                !string.Equals(route.SourceModalitySignalHandle, surface.SourceModalitySignalHandle, StringComparison.Ordinal) ||
                !string.Equals(route.SourcePressureHandle, surface.SourcePressureHandle, StringComparison.Ordinal)))
        {
            return Refuse(
                request,
                "personification-actualization-route-invalid",
                "Personification actualization surface route refused because routes must preserve surface, hook, modality, and pressure lineage without morphology, identity, action, continuity, authority, consent expansion, overreach normalization, Lisp evaluation, packet emission, replay, passage, or activation.",
                timestampUtc);
        }

        if (HasDuplicate(request.Routes.Select(static route => route.RouteHandle)))
        {
            return Refuse(
                request,
                "personification-actualization-duplicate-route-handle",
                "Personification actualization surface refused because duplicate route handles would collapse route lineage.",
                timestampUtc);
        }

        if (request.Surfaces.Count > 0 &&
            request.Surfaces.Any(surface => !request.Routes.Any(route =>
                string.Equals(route.SurfaceHandle, surface.SurfaceHandle, StringComparison.Ordinal))))
        {
            return Refuse(
                request,
                "personification-actualization-route-missing",
                "Personification actualization surface refused because every pre-morphological use surface requires a cooling and Steward-review route.",
                timestampUtc);
        }

        var useClasses = request.Surfaces
            .Select(static surface => surface.UseClass)
            .ToHashSet();
        if (request.Surfaces.Count > 0 &&
            Enum.GetValues<PersonificationActualizationUseClass>().Any(useClass => !useClasses.Contains(useClass)))
        {
            return Refuse(
                request,
                "personification-actualization-use-class-coverage-missing",
                "Personification actualization surface refused because orientation, salience modulation, repair posture, relational posture, cooling, refusal preparation, and Steward review preparation must be represented together before retained status.",
                timestampUtc);
        }

        var disposition = request.Surfaces.Count == 0
            ? PersonificationActualizationSurfaceDisposition.EmptyReviewCold
            : PersonificationActualizationSurfaceDisposition.SurfaceRetainedForPreMorphologicalUseCold;
        var outcomeCode = disposition == PersonificationActualizationSurfaceDisposition.EmptyReviewCold
            ? "personification-actualization-empty-review-only"
            : "personification-actualization-surface-retained-pre-morphological-cold";
        var governanceTrace = disposition == PersonificationActualizationSurfaceDisposition.EmptyReviewCold
            ? "Personification actualization surface found no use surfaces. Empty review remains cold and does not create morphology, identity, personhood, authority, action, continuity, Lisp evaluation, packet emission, replay, passage, or activation."
            : "Personification actualization surfaces were retained for pre-morphological use only. Personification telemetry may guide orientation, salience modulation, repair posture, relational posture, cooling, refusal preparation, and Steward review preparation while refusing use as morphology, identity, personhood, legal status, rights, felt authorization, action, continuity, authority, consent expansion, overreach normalization, Lisp evaluation, packet emission, replay, passage, or activation.";

        return CreateReceipt(
            request,
            disposition,
            outcomeCode,
            governanceTrace,
            refusal: null,
            timestampUtc);
    }

    private static PersonificationActualizationSurfaceReceipt Refuse(
        PersonificationActualizationSurfaceRequest request,
        string outcomeCode,
        string governanceTrace,
        DateTimeOffset timestampUtc) =>
        CreateReceipt(
            request,
            PersonificationActualizationSurfaceDisposition.Refused,
            outcomeCode,
            governanceTrace,
            new PersonificationActualizationSurfaceRefusalReceipt(
                ReceiptHandle: $"urn:san:personification-actualization-refusal:{ShortHash(SourceHookHandle(request), SourceModalityHandle(request), SourcePressureHandle(request), outcomeCode)}",
                RefusalCode: outcomeCode,
                GovernanceTrace: governanceTrace,
                Retained: true),
            timestampUtc);

    private static PersonificationActualizationSurfaceReceipt CreateReceipt(
        PersonificationActualizationSurfaceRequest request,
        PersonificationActualizationSurfaceDisposition disposition,
        string outcomeCode,
        string governanceTrace,
        PersonificationActualizationSurfaceRefusalReceipt? refusal,
        DateTimeOffset timestampUtc)
    {
        var maximumWeight = refusal is null && request.Surfaces.Count > 0
            ? request.Surfaces.Max(static surface => surface.UseVector.MaximumWeight)
            : 0m;

        return new(
            ReceiptHandle: $"urn:san:personification-actualization:{(refusal is null ? "review" : "refused")}:{ShortHash(SourceHookHandle(request), SourceModalityHandle(request), SourcePressureHandle(request), outcomeCode, request.Surfaces.Count.ToString(System.Globalization.CultureInfo.InvariantCulture), timestampUtc.UtcTicks.ToString(System.Globalization.CultureInfo.InvariantCulture))}",
            Disposition: disposition,
            OutcomeCode: outcomeCode,
            GovernanceTrace: governanceTrace,
            SourcePersonificationHookReceiptHandle: SourceHookHandle(request),
            SourceModalityHumilityReceiptHandle: SourceModalityHandle(request),
            SourceRehearsalPressureReceiptHandle: SourcePressureHandle(request),
            Surfaces: refusal is null ? request.Surfaces.ToArray() : [],
            Routes: refusal is null ? request.Routes.ToArray() : [],
            SurfaceBoundary: request.SurfaceBoundary,
            NonIdentityBoundary: request.NonIdentityBoundary,
            Refusal: refusal,
            PriorPassageCount: request.PriorPassageCount,
            PassageCountAfterActualizationReview: request.PriorPassageCount,
            RetainedSurfaceCount: refusal is null ? request.Surfaces.Count : 0,
            MaximumObservedUseWeight: maximumWeight,
            ReviewOnly: true,
            PreMorphologicalOnly: true,
            TelemetryOnly: true,
            FutureMorphologyAbsent: true,
            PersonificationTelemetryUsable: refusal is null && request.Surfaces.Count > 0,
            MorphologicalIdentityCreated: false,
            IdentityClaimed: false,
            PersonhoodClaimed: false,
            LegalStatusClaimed: false,
            RightsClaimed: false,
            FeltSignificanceAuthorized: false,
            SalienceBecameCommand: false,
            RepairNormalizedOverreach: false,
            RelationalPostureCreatedObedience: false,
            ModalityProvedEmbodiment: false,
            PressureBecameWill: false,
            ActionAuthorized: false,
            IdentityMutated: false,
            ContinuityAdmitted: false,
            AuthorityGranted: false,
            ConsentExpanded: false,
            RuntimeActionAllowed: false,
            LispEvaluationAllowed: false,
            NewPacketEmitted: false,
            ReceiptsReplayed: false,
            ActivationRefused: true,
            TimestampUtc: timestampUtc);
    }

    private static string SourceHookHandle(PersonificationActualizationSurfaceRequest request) =>
        request.SourcePersonificationHookReceipt?.ReceiptHandle ?? "missing-personification-actualization-hook-source";

    private static string SourceModalityHandle(PersonificationActualizationSurfaceRequest request) =>
        request.SourceModalityHumilityReceipt?.ReceiptHandle ?? "missing-personification-actualization-modality-source";

    private static string SourcePressureHandle(PersonificationActualizationSurfaceRequest request) =>
        request.SourceRehearsalPressureReceipt?.ReceiptHandle ?? "missing-personification-actualization-pressure-source";

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
