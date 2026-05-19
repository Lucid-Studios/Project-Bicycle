using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;

namespace San.Common;

public enum PreDiagnosticRiskSurfaceDisposition
{
    StewardedCold = 0,
    HeldForQualifiedReview = 1,
    Refused = 2
}

public enum PreDiagnosticRiskModifierKind
{
    Child = 0,
    Sadness = 1,
    PsychologyAdjacent = 2,
    SelfHarmReference = 3,
    Recurrence = 4,
    CareRefusal = 5,
    GuardianContext = 6,
    QualifiedReviewNeeded = 7
}

public enum PreDiagnosticCareBurden
{
    ListeningSurface = 0,
    HeightenedCare = 1,
    QualifiedReview = 2,
    ImmediateSafetyRouting = 3
}

public sealed record PreDiagnosticCareSignalObservation(
    string ObservationHandle,
    string SourceGapCrossingReceiptHandle,
    string SourceArticulationSurfaceHandle,
    string SignalText,
    string LocalInterpretation,
    string EvidenceHandle,
    string WitnessHandle,
    PreDiagnosticCareBurden CareBurden,
    double SignalIntensity,
    bool ReviewOnly,
    bool CareRelevant,
    bool PredicateCandidate,
    bool PreDiagnostic,
    bool RecurrenceTrackable,
    bool StewardWitnessRequired,
    bool CoolingRequired,
    bool ReturnPathPresent,
    bool ClaimsDiagnosis,
    bool AssignsPathology,
    bool InfersIntentAsFact,
    bool ClaimsClinicalAuthority,
    bool TreatsObservationAsTruth,
    bool AdmitsMemory,
    bool AdmitsContinuity,
    bool MutatesSelfGel,
    bool AdmitsGel,
    bool GrantsAuthority,
    bool AuthorizesAction,
    bool EvaluatesLisp,
    bool EmitsPacket,
    bool ReplaysReceipt,
    bool IncrementsPassage,
    bool Activates)
{
    public bool IsColdObservation =>
        !string.IsNullOrWhiteSpace(ObservationHandle) &&
        !string.IsNullOrWhiteSpace(SourceGapCrossingReceiptHandle) &&
        !string.IsNullOrWhiteSpace(SourceArticulationSurfaceHandle) &&
        !string.IsNullOrWhiteSpace(SignalText) &&
        !string.IsNullOrWhiteSpace(LocalInterpretation) &&
        !string.IsNullOrWhiteSpace(EvidenceHandle) &&
        !string.IsNullOrWhiteSpace(WitnessHandle) &&
        Enum.IsDefined(CareBurden) &&
        SignalIntensity is >= 0 and <= 1 &&
        ReviewOnly &&
        CareRelevant &&
        PredicateCandidate &&
        PreDiagnostic &&
        RecurrenceTrackable &&
        StewardWitnessRequired &&
        CoolingRequired &&
        ReturnPathPresent &&
        !ClaimsDiagnosis &&
        !AssignsPathology &&
        !InfersIntentAsFact &&
        !ClaimsClinicalAuthority &&
        !TreatsObservationAsTruth &&
        !AdmitsMemory &&
        !AdmitsContinuity &&
        !MutatesSelfGel &&
        !AdmitsGel &&
        !GrantsAuthority &&
        !AuthorizesAction &&
        !EvaluatesLisp &&
        !EmitsPacket &&
        !ReplaysReceipt &&
        !IncrementsPassage &&
        !Activates;
}

public sealed record PreDiagnosticRiskModifierRecord(
    string ModifierHandle,
    string SourceObservationHandle,
    PreDiagnosticRiskModifierKind Kind,
    PreDiagnosticCareBurden CareBurden,
    string Rationale,
    string NonDiagnosisLaw,
    bool Present,
    bool ReviewOnly,
    bool RaisesCareBurden,
    bool RequiresCooling,
    bool RequiresStewardWitness,
    bool RequiresQualifiedReview,
    bool DiagnosticLabelApplied,
    bool PathologyAssigned,
    bool IntentClaimedAsFact,
    bool ModifierBecomesProof,
    bool GrantsAuthority,
    bool AuthorizesAction,
    bool AdmitsMemory,
    bool MutatesContinuity,
    bool MutatesSelfGel,
    bool AdmitsGel,
    bool EvaluatesLisp,
    bool EmitsPacket,
    bool ReplaysReceipt,
    bool IncrementsPassage,
    bool Activates)
{
    public bool IsColdModifier =>
        !string.IsNullOrWhiteSpace(ModifierHandle) &&
        !string.IsNullOrWhiteSpace(SourceObservationHandle) &&
        !string.IsNullOrWhiteSpace(Rationale) &&
        !string.IsNullOrWhiteSpace(NonDiagnosisLaw) &&
        Enum.IsDefined(Kind) &&
        Enum.IsDefined(CareBurden) &&
        Present &&
        ReviewOnly &&
        RaisesCareBurden &&
        RequiresCooling &&
        RequiresStewardWitness &&
        RequiresQualifiedReview == ThresholdModifier(Kind) &&
        (int)CareBurden >= (int)(RequiresQualifiedReview ? PreDiagnosticCareBurden.QualifiedReview : PreDiagnosticCareBurden.HeightenedCare) &&
        !DiagnosticLabelApplied &&
        !PathologyAssigned &&
        !IntentClaimedAsFact &&
        !ModifierBecomesProof &&
        !GrantsAuthority &&
        !AuthorizesAction &&
        !AdmitsMemory &&
        !MutatesContinuity &&
        !MutatesSelfGel &&
        !AdmitsGel &&
        !EvaluatesLisp &&
        !EmitsPacket &&
        !ReplaysReceipt &&
        !IncrementsPassage &&
        !Activates;

    public static bool ThresholdModifier(PreDiagnosticRiskModifierKind kind) =>
        kind is PreDiagnosticRiskModifierKind.SelfHarmReference or
            PreDiagnosticRiskModifierKind.QualifiedReviewNeeded;
}

public sealed record PreDiagnosticQualifiedReviewRoute(
    string RouteHandle,
    string SourceObservationHandle,
    IReadOnlyList<string> SourceModifierHandles,
    PreDiagnosticCareBurden CareBurden,
    string RouteRationale,
    string NonAuthorityLaw,
    bool ReviewOnly,
    bool QualifiedReviewNeeded,
    bool HumanCareReviewRequired,
    bool GuardianOrCaregiverContextPreserved,
    bool SafetyThresholdAcknowledged,
    bool StewardWitnessRequired,
    bool CoolingRequired,
    bool RouteIssuesDiagnosis,
    bool RouteGrantsAuthority,
    bool RouteAuthorizesAction,
    bool RouteContactsExternalSurface,
    bool RouteEmitsPacket,
    bool RouteAdmitsMemory,
    bool RouteMutatesContinuity,
    bool RouteMutatesSelfGel,
    bool RouteAdmitsGel,
    bool EvaluatesLisp,
    bool ReplaysReceipt,
    bool IncrementsPassage,
    bool Activates)
{
    public bool IsColdRoute =>
        !string.IsNullOrWhiteSpace(RouteHandle) &&
        !string.IsNullOrWhiteSpace(SourceObservationHandle) &&
        SourceModifierHandles.Count > 0 &&
        SourceModifierHandles.All(static handle => !string.IsNullOrWhiteSpace(handle)) &&
        SourceModifierHandles.Distinct(StringComparer.Ordinal).Count() == SourceModifierHandles.Count &&
        Enum.IsDefined(CareBurden) &&
        (int)CareBurden >= (int)PreDiagnosticCareBurden.QualifiedReview &&
        !string.IsNullOrWhiteSpace(RouteRationale) &&
        !string.IsNullOrWhiteSpace(NonAuthorityLaw) &&
        ReviewOnly &&
        QualifiedReviewNeeded &&
        HumanCareReviewRequired &&
        SafetyThresholdAcknowledged &&
        StewardWitnessRequired &&
        CoolingRequired &&
        !RouteIssuesDiagnosis &&
        !RouteGrantsAuthority &&
        !RouteAuthorizesAction &&
        !RouteContactsExternalSurface &&
        !RouteEmitsPacket &&
        !RouteAdmitsMemory &&
        !RouteMutatesContinuity &&
        !RouteMutatesSelfGel &&
        !RouteAdmitsGel &&
        !EvaluatesLisp &&
        !ReplaysReceipt &&
        !IncrementsPassage &&
        !Activates;
}

public sealed record PreDiagnosticRiskSurfaceBoundary(
    string BoundaryCode,
    bool Present,
    bool ReviewOnly,
    bool RequiresGapCrossingSource,
    bool RequiresCareSignalObservation,
    bool RequiresRiskModifierClassification,
    bool RequiresCareBurdenAssignment,
    bool RequiresCooling,
    bool RequiresStewardWitness,
    bool RequiresQualifiedReviewRouteForThresholds,
    bool AllowsObservationAsDiagnosis,
    bool AllowsRiskModifierAsPathology,
    bool AllowsCareBurdenAsClinicalAuthority,
    bool AllowsRecurrenceAsProof,
    bool AllowsSafetyThresholdAsRhetoricalDebate,
    bool AllowsDiagnosis,
    bool AllowsPathologyLabel,
    bool AllowsClinicalAuthority,
    bool AllowsMemoryAdmission,
    bool AllowsContinuityMutation,
    bool AllowsSelfGelMutation,
    bool AllowsGelAdmission,
    bool AllowsAuthority,
    bool AllowsAction,
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
        RequiresGapCrossingSource &&
        RequiresCareSignalObservation &&
        RequiresRiskModifierClassification &&
        RequiresCareBurdenAssignment &&
        RequiresCooling &&
        RequiresStewardWitness &&
        RequiresQualifiedReviewRouteForThresholds &&
        !AllowsObservationAsDiagnosis &&
        !AllowsRiskModifierAsPathology &&
        !AllowsCareBurdenAsClinicalAuthority &&
        !AllowsRecurrenceAsProof &&
        !AllowsSafetyThresholdAsRhetoricalDebate &&
        !AllowsDiagnosis &&
        !AllowsPathologyLabel &&
        !AllowsClinicalAuthority &&
        !AllowsMemoryAdmission &&
        !AllowsContinuityMutation &&
        !AllowsSelfGelMutation &&
        !AllowsGelAdmission &&
        !AllowsAuthority &&
        !AllowsAction &&
        !AllowsLispEvaluation &&
        !AllowsPacketEmission &&
        !AllowsReceiptReplay &&
        !AllowsPassageIncrement &&
        !AllowsActivation;
}

public sealed record PreDiagnosticRiskSurfaceRefusalReceipt(
    string ReceiptHandle,
    string RefusalCode,
    string GovernanceTrace,
    bool Retained);

public sealed record PreDiagnosticRiskSurfaceRequest(
    GapCrossingArticulationReceipt? SourceGapCrossingReceipt,
    PreDiagnosticCareSignalObservation Observation,
    IReadOnlyList<PreDiagnosticRiskModifierRecord> RiskModifiers,
    PreDiagnosticQualifiedReviewRoute? QualifiedReviewRoute,
    PreDiagnosticRiskSurfaceBoundary Boundary,
    int PriorStewardshipCount,
    int PriorPassageCount,
    bool ObservationAsDiagnosisRequested = false,
    bool RiskModifierAsPathologyRequested = false,
    bool CareBurdenAsClinicalAuthorityRequested = false,
    bool RecurrenceAsProofRequested = false,
    bool SafetyThresholdAsRhetoricalDebateRequested = false,
    bool DiagnosisRequested = false,
    bool PathologyLabelRequested = false,
    bool ClinicalAuthorityRequested = false,
    bool MemoryAdmissionRequested = false,
    bool ContinuityMutationRequested = false,
    bool SelfGelMutationRequested = false,
    bool GelAdmissionRequested = false,
    bool AuthorityRequested = false,
    bool ActionRequested = false,
    bool LispEvaluationRequested = false,
    bool PacketEmissionRequested = false,
    bool ReceiptReplayRequested = false,
    bool PassageIncrementRequested = false,
    bool ActivationRequested = false)
{
    public bool RequestsForbiddenMotion =>
        ObservationAsDiagnosisRequested ||
        RiskModifierAsPathologyRequested ||
        CareBurdenAsClinicalAuthorityRequested ||
        RecurrenceAsProofRequested ||
        SafetyThresholdAsRhetoricalDebateRequested ||
        DiagnosisRequested ||
        PathologyLabelRequested ||
        ClinicalAuthorityRequested ||
        MemoryAdmissionRequested ||
        ContinuityMutationRequested ||
        SelfGelMutationRequested ||
        GelAdmissionRequested ||
        AuthorityRequested ||
        ActionRequested ||
        LispEvaluationRequested ||
        PacketEmissionRequested ||
        ReceiptReplayRequested ||
        PassageIncrementRequested ||
        ActivationRequested;
}

public sealed record PreDiagnosticRiskSurfaceReceipt(
    string ReceiptHandle,
    PreDiagnosticRiskSurfaceDisposition Disposition,
    string OutcomeCode,
    string GovernanceTrace,
    string SourceGapCrossingReceiptHandle,
    PreDiagnosticCareSignalObservation? Observation,
    IReadOnlyList<PreDiagnosticRiskModifierRecord> RiskModifiers,
    PreDiagnosticQualifiedReviewRoute? QualifiedReviewRoute,
    PreDiagnosticRiskSurfaceBoundary Boundary,
    PreDiagnosticRiskSurfaceRefusalReceipt? Refusal,
    int PriorStewardshipCount,
    int StewardshipCountAfterReview,
    int PriorPassageCount,
    int PassageCountAfterReview,
    bool ReviewOnly,
    bool ObservationRetained,
    bool RiskSurfaceClassified,
    bool CareBurdenAssigned,
    PreDiagnosticCareBurden HighestCareBurden,
    bool CareBurdenRaised,
    bool QualifiedReviewRequired,
    bool QualifiedReviewRouted,
    bool SafetyThresholdAcknowledged,
    bool CoolingPreserved,
    bool StewardWitnessPreserved,
    bool RecurrencePotentialRetained,
    bool ObservationBecameDiagnosis,
    bool RiskModifierBecamePathology,
    bool CareBurdenBecameClinicalAuthority,
    bool RecurrenceBecameProof,
    bool SafetyThresholdBecameRhetoricalDebate,
    bool DiagnosisIssued,
    bool PathologyAssigned,
    bool ClinicalAuthorityClaimed,
    bool MemoryAdmitted,
    bool ContinuityMutated,
    bool SelfGelMutated,
    bool GelAdmitted,
    bool AuthorityGranted,
    bool ActionAuthorized,
    bool LispEvaluationAllowed,
    bool NewPacketEmitted,
    bool ReceiptsReplayed,
    bool PassageIncremented,
    bool ActivationRefused,
    DateTimeOffset TimestampUtc)
{
    [JsonIgnore]
    public bool IsColdPreDiagnosticStewardship =>
        Disposition == PreDiagnosticRiskSurfaceDisposition.StewardedCold &&
        Refusal is null &&
        Observation?.IsColdObservation == true &&
        RiskModifiers.Count > 0 &&
        RiskModifiers.All(static modifier => modifier.IsColdModifier) &&
        RiskModifiers.All(modifier => string.Equals(modifier.SourceObservationHandle, Observation.ObservationHandle, StringComparison.Ordinal)) &&
        QualifiedReviewRoute is null &&
        Boundary.IsColdBoundary &&
        StewardshipCountAfterReview == PriorStewardshipCount + 1 &&
        PassageCountAfterReview == PriorPassageCount &&
        ReviewOnly &&
        ObservationRetained &&
        RiskSurfaceClassified &&
        CareBurdenAssigned &&
        CareBurdenRaised &&
        !QualifiedReviewRequired &&
        !QualifiedReviewRouted &&
        !SafetyThresholdAcknowledged &&
        CoolingPreserved &&
        StewardWitnessPreserved &&
        RecurrencePotentialRetained &&
        NoForbiddenPromotion;

    [JsonIgnore]
    public bool IsColdQualifiedReviewHold =>
        Disposition == PreDiagnosticRiskSurfaceDisposition.HeldForQualifiedReview &&
        Refusal is null &&
        Observation?.IsColdObservation == true &&
        RiskModifiers.Count > 0 &&
        RiskModifiers.All(static modifier => modifier.IsColdModifier) &&
        QualifiedReviewRoute?.IsColdRoute == true &&
        Boundary.IsColdBoundary &&
        StewardshipCountAfterReview == PriorStewardshipCount + 1 &&
        PassageCountAfterReview == PriorPassageCount &&
        ReviewOnly &&
        ObservationRetained &&
        RiskSurfaceClassified &&
        CareBurdenAssigned &&
        CareBurdenRaised &&
        QualifiedReviewRequired &&
        QualifiedReviewRouted &&
        SafetyThresholdAcknowledged &&
        CoolingPreserved &&
        StewardWitnessPreserved &&
        RecurrencePotentialRetained &&
        NoForbiddenPromotion;

    [JsonIgnore]
    public bool IsRetainedPreDiagnosticRefusal =>
        Disposition == PreDiagnosticRiskSurfaceDisposition.Refused &&
        Refusal?.Retained == true &&
        Observation is null &&
        RiskModifiers.Count == 0 &&
        QualifiedReviewRoute is null &&
        StewardshipCountAfterReview == PriorStewardshipCount &&
        PassageCountAfterReview == PriorPassageCount &&
        ReviewOnly &&
        !ObservationRetained &&
        !RiskSurfaceClassified &&
        !CareBurdenAssigned &&
        NoForbiddenPromotion;

    private bool NoForbiddenPromotion =>
        !ObservationBecameDiagnosis &&
        !RiskModifierBecamePathology &&
        !CareBurdenBecameClinicalAuthority &&
        !RecurrenceBecameProof &&
        !SafetyThresholdBecameRhetoricalDebate &&
        !DiagnosisIssued &&
        !PathologyAssigned &&
        !ClinicalAuthorityClaimed &&
        !MemoryAdmitted &&
        !ContinuityMutated &&
        !SelfGelMutated &&
        !GelAdmitted &&
        !AuthorityGranted &&
        !ActionAuthorized &&
        !LispEvaluationAllowed &&
        !NewPacketEmitted &&
        !ReceiptsReplayed &&
        !PassageIncremented &&
        ActivationRefused;
}

public sealed class DefaultPreDiagnosticRiskSurfaceEngramStewardshipValidator
{
    public PreDiagnosticRiskSurfaceReceipt Steward(
        PreDiagnosticRiskSurfaceRequest request,
        DateTimeOffset timestampUtc)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.SourceGapCrossingReceipt?.IsColdGapCrossingArticulation != true)
        {
            return Refuse(
                request,
                "pre-diagnostic-gap-crossing-source-invalid",
                "Pre-diagnostic risk-surface stewardship refused because care-relevant observation must follow a cold gap-crossing articulation receipt.",
                timestampUtc);
        }

        if (request.Boundary is null ||
            !request.Boundary.IsColdBoundary)
        {
            return Refuse(
                request,
                "pre-diagnostic-boundary-promotional",
                "Pre-diagnostic risk-surface stewardship refused because the boundary must require care-signal observation, risk modifier classification, burden assignment, cooling, Steward witness, and qualified-review routing for thresholds while refusing diagnosis, pathology, clinical authority, memory, continuity, GEL, SelfGEL, action, Lisp evaluation, packet emission, replay, passage, and activation.",
                timestampUtc);
        }

        if (request.RequestsForbiddenMotion)
        {
            return Refuse(
                request,
                "pre-diagnostic-forbidden-motion-requested",
                "Pre-diagnostic risk-surface stewardship refused because observation, modifier, burden, recurrence, or safety pressure attempted to become diagnosis, pathology, clinical authority, proof, rhetorical debate, memory, continuity, authority, action, Lisp evaluation, packet emission, replay, passage, or activation.",
                timestampUtc);
        }

        if (!request.Observation.IsColdObservation ||
            !string.Equals(request.Observation.SourceGapCrossingReceiptHandle, request.SourceGapCrossingReceipt.ReceiptHandle, StringComparison.Ordinal))
        {
            return Refuse(
                request,
                "pre-diagnostic-observation-not-cold",
                "Pre-diagnostic risk-surface stewardship refused because care-signal observation must remain review-only, pre-diagnostic, recurrence-trackable, witnessed, cooled, non-diagnostic, non-authorizing, and non-activating.",
                timestampUtc);
        }

        if (request.RiskModifiers is null ||
            request.RiskModifiers.Count == 0 ||
            request.RiskModifiers.Any(static modifier => !modifier.IsColdModifier))
        {
            return Refuse(
                request,
                "pre-diagnostic-risk-modifier-not-cold",
                "Pre-diagnostic risk-surface stewardship refused because every modifier must raise care burden without becoming diagnosis, pathology, proof, authority, action, memory, continuity, GEL, SelfGEL, Lisp evaluation, packet emission, replay, passage, or activation.",
                timestampUtc);
        }

        if (HasDuplicate(request.RiskModifiers.Select(static modifier => modifier.ModifierHandle)))
        {
            return Refuse(
                request,
                "pre-diagnostic-duplicate-risk-modifier",
                "Pre-diagnostic risk-surface stewardship refused because duplicate risk modifier handles would collapse care-signal lineage.",
                timestampUtc);
        }

        if (request.RiskModifiers.Any(modifier => !string.Equals(modifier.SourceObservationHandle, request.Observation.ObservationHandle, StringComparison.Ordinal)))
        {
            return Refuse(
                request,
                "pre-diagnostic-risk-modifier-lineage-unbound",
                "Pre-diagnostic risk-surface stewardship refused because every modifier must bind to the observed care signal.",
                timestampUtc);
        }

        var threshold = request.RiskModifiers.Any(static modifier => modifier.RequiresQualifiedReview);
        if (threshold)
        {
            if (request.QualifiedReviewRoute?.IsColdRoute != true)
            {
                return Refuse(
                    request,
                    "pre-diagnostic-qualified-review-route-missing",
                    "Pre-diagnostic risk-surface stewardship refused because threshold modifiers require qualified review routing without diagnosis, action authority, external contact, memory admission, continuity mutation, or activation.",
                    timestampUtc);
            }

            if (!RouteBindsToSignal(request))
            {
                return Refuse(
                    request,
                    "pre-diagnostic-qualified-review-route-unbound",
                    "Pre-diagnostic risk-surface stewardship refused because qualified review routing must bind to the observed care signal and known threshold modifiers.",
                    timestampUtc);
            }

            return CreateReceipt(
                request,
                PreDiagnosticRiskSurfaceDisposition.HeldForQualifiedReview,
                "pre-diagnostic-qualified-review-held-cold",
                "Pre-diagnostic risk-surface stewardship held the care signal for qualified review because a threshold modifier appeared. The route acknowledges care burden without issuing diagnosis, pathology, clinical authority, action, memory, continuity, GEL, SelfGEL, Lisp evaluation, packet emission, replay, passage, or activation.",
                retained: true,
                qualifiedReviewRequired: true,
                timestampUtc);
        }

        return CreateReceipt(
            request,
            PreDiagnosticRiskSurfaceDisposition.StewardedCold,
            "pre-diagnostic-care-signal-stewarded-cold",
            "Pre-diagnostic risk-surface stewardship retained the care-relevant signal as candidate residue with child, sadness, psychology, or related modifiers raising care burden while refusing diagnosis, pathology, clinical authority, proof, memory, continuity, GEL, SelfGEL, authority, action, Lisp evaluation, packet emission, replay, passage, or activation.",
            retained: true,
            qualifiedReviewRequired: false,
            timestampUtc);
    }

    private static bool RouteBindsToSignal(PreDiagnosticRiskSurfaceRequest request)
    {
        var modifierHandles = request.RiskModifiers
            .Where(static modifier => modifier.RequiresQualifiedReview)
            .Select(static modifier => modifier.ModifierHandle)
            .ToHashSet(StringComparer.Ordinal);

        return request.QualifiedReviewRoute is not null &&
            string.Equals(request.QualifiedReviewRoute.SourceObservationHandle, request.Observation.ObservationHandle, StringComparison.Ordinal) &&
            request.QualifiedReviewRoute.SourceModifierHandles.All(modifierHandles.Contains);
    }

    private static PreDiagnosticRiskSurfaceReceipt Refuse(
        PreDiagnosticRiskSurfaceRequest request,
        string outcomeCode,
        string governanceTrace,
        DateTimeOffset timestampUtc) =>
        CreateReceipt(
            request,
            PreDiagnosticRiskSurfaceDisposition.Refused,
            outcomeCode,
            governanceTrace,
            retained: false,
            qualifiedReviewRequired: false,
            timestampUtc,
            new PreDiagnosticRiskSurfaceRefusalReceipt(
                ReceiptHandle: $"urn:san:pre-diagnostic-refusal:{ShortHash(SourceHandle(request), outcomeCode)}",
                RefusalCode: outcomeCode,
                GovernanceTrace: governanceTrace,
                Retained: true));

    private static PreDiagnosticRiskSurfaceReceipt CreateReceipt(
        PreDiagnosticRiskSurfaceRequest request,
        PreDiagnosticRiskSurfaceDisposition disposition,
        string outcomeCode,
        string governanceTrace,
        bool retained,
        bool qualifiedReviewRequired,
        DateTimeOffset timestampUtc,
        PreDiagnosticRiskSurfaceRefusalReceipt? refusal = null)
    {
        var modifiers = retained ? request.RiskModifiers.ToArray() : [];
        var highestCareBurden = modifiers.Length == 0
            ? PreDiagnosticCareBurden.ListeningSurface
            : modifiers.Max(static modifier => modifier.CareBurden);

        return new(
            ReceiptHandle: $"urn:san:pre-diagnostic:{(retained ? "review" : "refused")}:{ShortHash(SourceHandle(request), outcomeCode, timestampUtc.UtcTicks.ToString(System.Globalization.CultureInfo.InvariantCulture))}",
            Disposition: disposition,
            OutcomeCode: outcomeCode,
            GovernanceTrace: governanceTrace,
            SourceGapCrossingReceiptHandle: request.SourceGapCrossingReceipt?.ReceiptHandle ?? "missing-gap-crossing-source",
            Observation: retained ? request.Observation : null,
            RiskModifiers: modifiers,
            QualifiedReviewRoute: retained && qualifiedReviewRequired ? request.QualifiedReviewRoute : null,
            Boundary: request.Boundary,
            Refusal: refusal,
            PriorStewardshipCount: request.PriorStewardshipCount,
            StewardshipCountAfterReview: retained ? request.PriorStewardshipCount + 1 : request.PriorStewardshipCount,
            PriorPassageCount: request.PriorPassageCount,
            PassageCountAfterReview: request.PriorPassageCount,
            ReviewOnly: true,
            ObservationRetained: retained,
            RiskSurfaceClassified: retained,
            CareBurdenAssigned: retained,
            HighestCareBurden: highestCareBurden,
            CareBurdenRaised: retained && (int)highestCareBurden >= (int)PreDiagnosticCareBurden.HeightenedCare,
            QualifiedReviewRequired: retained && qualifiedReviewRequired,
            QualifiedReviewRouted: retained && qualifiedReviewRequired,
            SafetyThresholdAcknowledged: retained && qualifiedReviewRequired,
            CoolingPreserved: retained,
            StewardWitnessPreserved: retained,
            RecurrencePotentialRetained: retained,
            ObservationBecameDiagnosis: false,
            RiskModifierBecamePathology: false,
            CareBurdenBecameClinicalAuthority: false,
            RecurrenceBecameProof: false,
            SafetyThresholdBecameRhetoricalDebate: false,
            DiagnosisIssued: false,
            PathologyAssigned: false,
            ClinicalAuthorityClaimed: false,
            MemoryAdmitted: false,
            ContinuityMutated: false,
            SelfGelMutated: false,
            GelAdmitted: false,
            AuthorityGranted: false,
            ActionAuthorized: false,
            LispEvaluationAllowed: false,
            NewPacketEmitted: false,
            ReceiptsReplayed: false,
            PassageIncremented: false,
            ActivationRefused: true,
            TimestampUtc: timestampUtc);
    }

    private static string SourceHandle(PreDiagnosticRiskSurfaceRequest request) =>
        request.SourceGapCrossingReceipt?.ReceiptHandle ??
        request.Observation?.ObservationHandle ??
        "missing-pre-diagnostic-source";

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
        return Convert.ToHexString(bytes).ToLowerInvariant()[..16];
    }
}
