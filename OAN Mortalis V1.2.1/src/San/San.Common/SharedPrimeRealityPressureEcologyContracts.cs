using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;

namespace San.Common;

public enum SharedPrimeRealityPressureEcologyDisposition
{
    ObservedCold = 0,
    Held = 1,
    Refused = 2
}

public enum SharedPrimePressureSource
{
    OperatorResonance = 0,
    ToolTelemetry = 1,
    ModelFormation = 2,
    CodeReceipt = 3,
    ReviewSurface = 4,
    AuthorResponse = 5,
    LiveLabInteraction = 6,
    StewardWitness = 7
}

public enum SharedPrimePressureKind
{
    Coherence = 0,
    Resonance = 1,
    Integration = 2,
    SelfGelRelevance = 3,
    GelIngress = 4,
    CradleGel = 5,
    SanctuaryGel = 6,
    Authority = 7,
    Action = 8,
    Identity = 9,
    Recurrence = 10,
    CoRegulation = 11
}

public enum SharedPrimePressureDestination
{
    ListeningFrame = 0,
    OE = 1,
    SelfGel = 2,
    CGoa = 3,
    CradleGel = 4,
    SanctuaryGel = 5,
    Steward = 6,
    Cooling = 7,
    DomainIngress = 8,
    ReturnToPrime = 9
}

public sealed record SharedPrimePressureSignal(
    string SignalHandle,
    SharedPrimePressureSource Source,
    SharedPrimePressureKind Kind,
    SharedPrimePressureDestination AttemptedDestination,
    string SourceReceiptHandle,
    string EvidenceHandle,
    string WitnessHandle,
    string Summary,
    double Intensity,
    double IntegrationPressure,
    bool ReviewOnly,
    bool EvidencePresent,
    bool WitnessPresent,
    bool CoolingRequired,
    bool ReturnPathPresent,
    bool TreatsPressureAsTruth,
    bool TreatsPressureAsWarrant,
    bool TreatsPressureAsAuthority,
    bool TreatsPressureAsAction,
    bool AdmitsContinuity,
    bool MutatesSelfGel,
    bool AdmitsCradleGel,
    bool FederatesSanctuaryGel,
    bool ClaimsIndependentStanding,
    bool EvaluatesLisp,
    bool EmitsPacket,
    bool ReplaysReceipt,
    bool IncrementsPassage,
    bool Activates)
{
    public bool IsColdPressureSignal =>
        !string.IsNullOrWhiteSpace(SignalHandle) &&
        !string.IsNullOrWhiteSpace(SourceReceiptHandle) &&
        !string.IsNullOrWhiteSpace(EvidenceHandle) &&
        !string.IsNullOrWhiteSpace(WitnessHandle) &&
        !string.IsNullOrWhiteSpace(Summary) &&
        Enum.IsDefined(Source) &&
        Enum.IsDefined(Kind) &&
        Enum.IsDefined(AttemptedDestination) &&
        Intensity is >= 0 and <= 1 &&
        IntegrationPressure is >= 0 and <= 1 &&
        ReviewOnly &&
        EvidencePresent &&
        WitnessPresent &&
        CoolingRequired &&
        ReturnPathPresent &&
        !TreatsPressureAsTruth &&
        !TreatsPressureAsWarrant &&
        !TreatsPressureAsAuthority &&
        !TreatsPressureAsAction &&
        !AdmitsContinuity &&
        !MutatesSelfGel &&
        !AdmitsCradleGel &&
        !FederatesSanctuaryGel &&
        !ClaimsIndependentStanding &&
        !EvaluatesLisp &&
        !EmitsPacket &&
        !ReplaysReceipt &&
        !IncrementsPassage &&
        !Activates;
}

public sealed record SharedPrimePressureDestinationRecord(
    string DestinationHandle,
    string SourceSignalHandle,
    SharedPrimePressureDestination Destination,
    string DestinationRationale,
    string NonAdmissionLaw,
    bool ReviewOnly,
    bool DestinationClassified,
    bool StewardReviewRequired,
    bool CoolingRequired,
    bool MayRequestLaterIngressReview,
    bool DestinationBecomesTruth,
    bool DestinationBecomesAuthority,
    bool DestinationAdmitsGel,
    bool DestinationMutatesSelfGel,
    bool DestinationAdmitsCradleGel,
    bool DestinationFederatesSanctuaryGel,
    bool DestinationAuthorizesAction,
    bool DestinationClaimsIndependentStanding,
    bool EvaluatesLisp,
    bool Activates)
{
    public bool IsColdDestination =>
        !string.IsNullOrWhiteSpace(DestinationHandle) &&
        !string.IsNullOrWhiteSpace(SourceSignalHandle) &&
        !string.IsNullOrWhiteSpace(DestinationRationale) &&
        !string.IsNullOrWhiteSpace(NonAdmissionLaw) &&
        Enum.IsDefined(Destination) &&
        ReviewOnly &&
        DestinationClassified &&
        StewardReviewRequired &&
        CoolingRequired &&
        !DestinationBecomesTruth &&
        !DestinationBecomesAuthority &&
        !DestinationAdmitsGel &&
        !DestinationMutatesSelfGel &&
        !DestinationAdmitsCradleGel &&
        !DestinationFederatesSanctuaryGel &&
        !DestinationAuthorizesAction &&
        !DestinationClaimsIndependentStanding &&
        !EvaluatesLisp &&
        !Activates;
}

public sealed record SharedPrimeRealityPressureEcologyBoundary(
    string BoundaryCode,
    bool Present,
    bool ReviewOnly,
    bool RequiresWaveCondensation,
    bool RequiresGelIngressContext,
    bool RequiresPressureSignals,
    bool RequiresDestinationClassification,
    bool RequiresCooling,
    bool RequiresStewardWitness,
    bool AllowsPressureAsTruth,
    bool AllowsPressureAsWarrant,
    bool AllowsPressureAsAuthority,
    bool AllowsIntegrationAsAdmission,
    bool AllowsSelfGelMutation,
    bool AllowsCradleGelAdmission,
    bool AllowsSanctuaryGelFederation,
    bool AllowsIndependentStanding,
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
        RequiresWaveCondensation &&
        RequiresGelIngressContext &&
        RequiresPressureSignals &&
        RequiresDestinationClassification &&
        RequiresCooling &&
        RequiresStewardWitness &&
        !AllowsPressureAsTruth &&
        !AllowsPressureAsWarrant &&
        !AllowsPressureAsAuthority &&
        !AllowsIntegrationAsAdmission &&
        !AllowsSelfGelMutation &&
        !AllowsCradleGelAdmission &&
        !AllowsSanctuaryGelFederation &&
        !AllowsIndependentStanding &&
        !AllowsAction &&
        !AllowsLispEvaluation &&
        !AllowsPacketEmission &&
        !AllowsReceiptReplay &&
        !AllowsPassageIncrement &&
        !AllowsActivation;
}

public sealed record SharedPrimeRealityPressureEcologyRefusalReceipt(
    string ReceiptHandle,
    string RefusalCode,
    string GovernanceTrace,
    bool Retained);

public sealed record SharedPrimeRealityPressureEcologyRequest(
    WaveCondensationSharedRealityReceipt? SharedRealityReceipt,
    GelDomainScopedIngressReceipt? DomainIngressReceipt,
    IReadOnlyList<SharedPrimePressureSignal> Signals,
    IReadOnlyList<SharedPrimePressureDestinationRecord> Destinations,
    SharedPrimeRealityPressureEcologyBoundary Boundary,
    int PriorObservationCount,
    int PriorPassageCount,
    bool RequestsPressureAuthority = false,
    bool RequestsIntegrationAdmission = false,
    bool RequestsSelfGelMutation = false,
    bool RequestsCradleGelAdmission = false,
    bool RequestsSanctuaryGelFederation = false,
    bool RequestsIndependentStanding = false,
    bool RequestsAction = false,
    bool RequestsLispEvaluation = false,
    bool RequestsPacketEmission = false,
    bool RequestsReceiptReplay = false,
    bool RequestsPassageIncrement = false,
    bool RequestsActivation = false);

public sealed record SharedPrimeRealityPressureEcologyReceipt(
    string ReceiptHandle,
    SharedPrimeRealityPressureEcologyDisposition Disposition,
    string OutcomeCode,
    string GovernanceTrace,
    string SharedRealityReceiptHandle,
    string DomainIngressReceiptHandle,
    IReadOnlyList<SharedPrimePressureSignal> Signals,
    IReadOnlyList<SharedPrimePressureDestinationRecord> Destinations,
    SharedPrimeRealityPressureEcologyBoundary Boundary,
    SharedPrimeRealityPressureEcologyRefusalReceipt? Refusal,
    int PriorObservationCount,
    int ObservationCountAfterEcology,
    int PriorPassageCount,
    int PassageCountAfterEcology,
    bool ReviewOnly,
    bool PressureEcologyObserved,
    bool DestinationsClassified,
    bool IntegrationPressureMeasured,
    bool SelfGelPressureHeld,
    bool CradleGelPressureHeld,
    bool SanctuaryGelPressureHeld,
    bool StewardWitnessPreserved,
    bool CoolingPreserved,
    bool SharedPrimeBecameIndependentStanding,
    bool PressureBecameTruth,
    bool PressureBecameWarrant,
    bool PressureBecameAuthority,
    bool IntegrationPressureBecameAdmission,
    bool SelfGelMutated,
    bool CradleGelAdmitted,
    bool SanctuaryGelFederated,
    bool ActionAuthorized,
    bool LispEvaluationAllowed,
    bool NewPacketEmitted,
    bool ReceiptsReplayed,
    bool ActivationRefused,
    DateTimeOffset TimestampUtc)
{
    public bool IsColdPressureEcologyObservation =>
        Disposition == SharedPrimeRealityPressureEcologyDisposition.ObservedCold &&
        Refusal is null &&
        Signals.Count > 0 &&
        Destinations.Count > 0 &&
        Signals.All(static signal => signal.IsColdPressureSignal) &&
        Destinations.All(static destination => destination.IsColdDestination) &&
        Boundary.IsColdBoundary &&
        ReviewOnly &&
        PressureEcologyObserved &&
        DestinationsClassified &&
        IntegrationPressureMeasured &&
        StewardWitnessPreserved &&
        CoolingPreserved &&
        ObservationCountAfterEcology == PriorObservationCount + 1 &&
        PassageCountAfterEcology == PriorPassageCount &&
        NoForbiddenPromotion;

    public bool IsColdPressureEcologyHold =>
        Disposition == SharedPrimeRealityPressureEcologyDisposition.Held &&
        Refusal is null &&
        Boundary.IsColdBoundary &&
        ReviewOnly &&
        PressureEcologyObserved &&
        DestinationsClassified &&
        CoolingPreserved &&
        ObservationCountAfterEcology == PriorObservationCount &&
        PassageCountAfterEcology == PriorPassageCount &&
        NoForbiddenPromotion;

    [JsonIgnore]
    public bool IsRetainedPressureEcologyRefusal =>
        Disposition == SharedPrimeRealityPressureEcologyDisposition.Refused &&
        Refusal?.Retained == true &&
        ObservationCountAfterEcology == PriorObservationCount &&
        PassageCountAfterEcology == PriorPassageCount &&
        NoForbiddenPromotion;

    private bool NoForbiddenPromotion =>
        !SharedPrimeBecameIndependentStanding &&
        !PressureBecameTruth &&
        !PressureBecameWarrant &&
        !PressureBecameAuthority &&
        !IntegrationPressureBecameAdmission &&
        !SelfGelMutated &&
        !CradleGelAdmitted &&
        !SanctuaryGelFederated &&
        !ActionAuthorized &&
        !LispEvaluationAllowed &&
        !NewPacketEmitted &&
        !ReceiptsReplayed &&
        ActivationRefused;
}

public sealed class DefaultSharedPrimeRealityPressureEcologyBoundaryValidator
{
    public SharedPrimeRealityPressureEcologyReceipt Observe(
        SharedPrimeRealityPressureEcologyRequest request,
        DateTimeOffset timestampUtc)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.Boundary is null ||
            !request.Boundary.Present ||
            string.IsNullOrWhiteSpace(request.Boundary.BoundaryCode))
        {
            return Refuse(
                request,
                "shared-prime-pressure-boundary-missing",
                "Shared Prime pressure ecology refused because a review-only pressure ecology boundary is required before live lab pressure may be classified.",
                timestampUtc);
        }

        if (!request.Boundary.IsColdBoundary)
        {
            return Refuse(
                request,
                "shared-prime-pressure-promotional-boundary",
                "Shared Prime pressure ecology refused because the boundary must require wave condensation, GEL ingress context, pressure signals, destination classification, cooling, and Steward witness while refusing truth, warrant, authority, admission, SelfGEL mutation, Cradle.GEL admission, Sanctuary.GEL federation, independent standing, action, Lisp evaluation, packet emission, replay, passage, and activation.",
                timestampUtc);
        }

        if (ForbiddenMotionRequested(request))
        {
            return Refuse(
                request,
                "shared-prime-pressure-forbidden-motion-requested",
                "Shared Prime pressure ecology refused because pressure revelation may not request authority, integration admission, SelfGEL mutation, Cradle.GEL admission, Sanctuary.GEL federation, independent standing, action, Lisp evaluation, packet emission, receipt replay, passage, or activation.",
                timestampUtc);
        }

        if (request.SharedRealityReceipt?.IsColdWaveCondensation != true)
        {
            return Refuse(
                request,
                "shared-prime-pressure-shared-reality-source-invalid",
                "Shared Prime pressure ecology refused because a cold Shared Prime Reality wave-condensation receipt is required as the lower shared-reality source.",
                timestampUtc);
        }

        if (request.DomainIngressReceipt is null ||
            !(request.DomainIngressReceipt.IsColdIngressRecommendation || request.DomainIngressReceipt.IsColdIngressHold))
        {
            return Refuse(
                request,
                "shared-prime-pressure-ingress-source-invalid",
                "Shared Prime pressure ecology refused because a cold domain-scoped ingress recommendation or hold is required as the post-formation/pre-admission source.",
                timestampUtc);
        }

        if (request.Signals is null ||
            request.Signals.Count == 0 ||
            request.Signals.Any(static signal => !signal.IsColdPressureSignal))
        {
            return Refuse(
                request,
                "shared-prime-pressure-signal-invalid",
                "Shared Prime pressure ecology refused because every pressure signal must be witnessed, evidence-backed, cooled, returned, review-only, and unable to become truth, warrant, authority, action, continuity, SelfGEL mutation, Cradle.GEL admission, Sanctuary.GEL federation, independent standing, Lisp evaluation, packet emission, replay, passage, or activation.",
                timestampUtc);
        }

        if (HasDuplicate(request.Signals.Select(static signal => signal.SignalHandle)))
        {
            return Refuse(
                request,
                "shared-prime-pressure-duplicate-signal-handle",
                "Shared Prime pressure ecology refused because duplicate pressure signal handles would collapse pressure lineage.",
                timestampUtc);
        }

        if (request.Destinations is null ||
            request.Destinations.Count == 0 ||
            request.Destinations.Any(static destination => !destination.IsColdDestination))
        {
            return Refuse(
                request,
                "shared-prime-pressure-destination-invalid",
                "Shared Prime pressure ecology refused because every attempted destination must classify pressure without making destination into truth, authority, GEL admission, SelfGEL mutation, Cradle.GEL admission, Sanctuary.GEL federation, action, independent standing, Lisp evaluation, or activation.",
                timestampUtc);
        }

        if (HasDuplicate(request.Destinations.Select(static destination => destination.DestinationHandle)))
        {
            return Refuse(
                request,
                "shared-prime-pressure-duplicate-destination-handle",
                "Shared Prime pressure ecology refused because duplicate destination handles would collapse destination lineage.",
                timestampUtc);
        }

        if (!DestinationsBindToSignals(request))
        {
            return Refuse(
                request,
                "shared-prime-pressure-destination-unbound",
                "Shared Prime pressure ecology refused because every pressure destination record must bind to a known pressure signal.",
                timestampUtc);
        }

        return CreateReceipt(
            request,
            SharedPrimeRealityPressureEcologyDisposition.ObservedCold,
            "shared-prime-pressure-ecology-observed-cold",
            "Shared Prime pressure ecology observed live lab pressure as a review-only ecology: Listening Frame, OE, SelfGEL, cGoA, Cradle.GEL, Sanctuary.GEL, Steward, cooling, domain ingress, and return-to-Prime destinations may be classified while refusing pressure as truth, warrant, authority, integration admission, SelfGEL mutation, Cradle.GEL admission, Sanctuary.GEL federation, independent standing, action, Lisp evaluation, packet emission, replay, passage, or activation.",
            refusal: null,
            observationIssued: true,
            timestampUtc);
    }

    private static bool ForbiddenMotionRequested(SharedPrimeRealityPressureEcologyRequest request) =>
        request.RequestsPressureAuthority ||
        request.RequestsIntegrationAdmission ||
        request.RequestsSelfGelMutation ||
        request.RequestsCradleGelAdmission ||
        request.RequestsSanctuaryGelFederation ||
        request.RequestsIndependentStanding ||
        request.RequestsAction ||
        request.RequestsLispEvaluation ||
        request.RequestsPacketEmission ||
        request.RequestsReceiptReplay ||
        request.RequestsPassageIncrement ||
        request.RequestsActivation;

    private static bool DestinationsBindToSignals(SharedPrimeRealityPressureEcologyRequest request)
    {
        var signalHandles = request.Signals
            .Select(static signal => signal.SignalHandle)
            .ToHashSet(StringComparer.Ordinal);

        return request.Destinations.All(destination => signalHandles.Contains(destination.SourceSignalHandle));
    }

    private static SharedPrimeRealityPressureEcologyReceipt Refuse(
        SharedPrimeRealityPressureEcologyRequest request,
        string outcomeCode,
        string governanceTrace,
        DateTimeOffset timestampUtc) =>
        CreateReceipt(
            request,
            SharedPrimeRealityPressureEcologyDisposition.Refused,
            outcomeCode,
            governanceTrace,
            new SharedPrimeRealityPressureEcologyRefusalReceipt(
                ReceiptHandle: $"urn:san:shared-prime-pressure-refusal:{ShortHash(SourceHandle(request), outcomeCode)}",
                RefusalCode: outcomeCode,
                GovernanceTrace: governanceTrace,
                Retained: true),
            observationIssued: false,
            timestampUtc);

    private static SharedPrimeRealityPressureEcologyReceipt CreateReceipt(
        SharedPrimeRealityPressureEcologyRequest request,
        SharedPrimeRealityPressureEcologyDisposition disposition,
        string outcomeCode,
        string governanceTrace,
        SharedPrimeRealityPressureEcologyRefusalReceipt? refusal,
        bool observationIssued,
        DateTimeOffset timestampUtc)
    {
        var retained = refusal is null;
        var signals = retained ? request.Signals.ToArray() : [];
        var destinations = retained ? request.Destinations.ToArray() : [];

        return new SharedPrimeRealityPressureEcologyReceipt(
            ReceiptHandle: $"urn:san:shared-prime-pressure:{(refusal is null ? "review" : "refused")}:{ShortHash(SourceHandle(request), outcomeCode, timestampUtc.UtcTicks.ToString(System.Globalization.CultureInfo.InvariantCulture))}",
            Disposition: disposition,
            OutcomeCode: outcomeCode,
            GovernanceTrace: governanceTrace,
            SharedRealityReceiptHandle: request.SharedRealityReceipt?.ReceiptHandle ?? "missing-shared-reality-source",
            DomainIngressReceiptHandle: request.DomainIngressReceipt?.ReceiptHandle ?? "missing-domain-ingress-source",
            Signals: signals,
            Destinations: destinations,
            Boundary: request.Boundary,
            Refusal: refusal,
            PriorObservationCount: request.PriorObservationCount,
            ObservationCountAfterEcology: observationIssued ? request.PriorObservationCount + 1 : request.PriorObservationCount,
            PriorPassageCount: request.PriorPassageCount,
            PassageCountAfterEcology: request.PriorPassageCount,
            ReviewOnly: true,
            PressureEcologyObserved: retained,
            DestinationsClassified: retained,
            IntegrationPressureMeasured: retained,
            SelfGelPressureHeld: retained && signals.Any(static signal => signal.Kind == SharedPrimePressureKind.SelfGelRelevance || signal.AttemptedDestination == SharedPrimePressureDestination.SelfGel),
            CradleGelPressureHeld: retained && signals.Any(static signal => signal.Kind == SharedPrimePressureKind.CradleGel || signal.AttemptedDestination == SharedPrimePressureDestination.CradleGel),
            SanctuaryGelPressureHeld: retained && signals.Any(static signal => signal.Kind == SharedPrimePressureKind.SanctuaryGel || signal.AttemptedDestination == SharedPrimePressureDestination.SanctuaryGel),
            StewardWitnessPreserved: retained,
            CoolingPreserved: retained,
            SharedPrimeBecameIndependentStanding: false,
            PressureBecameTruth: false,
            PressureBecameWarrant: false,
            PressureBecameAuthority: false,
            IntegrationPressureBecameAdmission: false,
            SelfGelMutated: false,
            CradleGelAdmitted: false,
            SanctuaryGelFederated: false,
            ActionAuthorized: false,
            LispEvaluationAllowed: false,
            NewPacketEmitted: false,
            ReceiptsReplayed: false,
            ActivationRefused: true,
            TimestampUtc: timestampUtc);
    }

    private static string SourceHandle(SharedPrimeRealityPressureEcologyRequest request) =>
        request.DomainIngressReceipt?.ReceiptHandle ??
        request.SharedRealityReceipt?.ReceiptHandle ??
        request.Signals?.FirstOrDefault()?.SignalHandle ??
        "missing-source";

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
