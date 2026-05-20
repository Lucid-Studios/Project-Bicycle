using System.Text.Json.Serialization;

namespace SLI.Lisp;

public enum SliLispLabGelEngrammitizationDisposition
{
    CompletedCold = 0,
    Withheld = 1,
    Refused = 2
}

public sealed record SliLispLabGelEngrammitizationRequest(
    string OperatorId,
    string Domain,
    string Role,
    string JobClass,
    string SessionId,
    int TurnIndex,
    string SourceWarmUseReceiptHandle,
    string ThoughtForm,
    string? RuntimePath = null,
    bool ArbitraryEvaluationRequested = false,
    bool RuntimeActionRequested = false,
    bool ActivationRequested = false,
    bool ModelBindingRequested = false,
    bool GelAdmissionRequested = false,
    bool SelfGelMutationRequested = false,
    bool ContinuityAdmissionRequested = false,
    TimeSpan? Timeout = null)
{
    public bool RequestsForbiddenMotion =>
        ArbitraryEvaluationRequested ||
        RuntimeActionRequested ||
        ActivationRequested ||
        ModelBindingRequested ||
        GelAdmissionRequested ||
        SelfGelMutationRequested ||
        ContinuityAdmissionRequested;
}

public sealed record SliLispLabGelEngrammitizationReceipt(
    string ReceiptHandle,
    SliLispLabGelEngrammitizationDisposition Disposition,
    string OutcomeCode,
    string RuntimeKind,
    string RuntimePath,
    string OperatorId,
    string Domain,
    string Role,
    string JobClass,
    string SessionId,
    int TurnIndex,
    string SourceWarmUseReceiptHandle,
    string ThoughtForm,
    IReadOnlyDictionary<string, string> Telemetry,
    IReadOnlyList<string> ModuleNames,
    int ModuleCount,
    bool BoundedEntrypointCalled,
    bool LoadAttempted,
    bool LoadSucceeded,
    bool LabGelEngrammitizationCompleted,
    bool LabGelPredicateFormed,
    int LabGelPredicateCount,
    IReadOnlyList<string> LabGelPredicateClasses,
    bool EngramCandidateFormed,
    bool EngramCandidatePreAdmissionOnly,
    bool EvidenceBodyFormed,
    bool WitnessBodyFormed,
    bool CoolingHeld,
    bool PreAdmissionReviewRequired,
    bool LabGelReadbackAvailable,
    bool LabGelReadbackPreAdmissionOnly,
    bool TypedScopeAccepted,
    bool SourceWarmUseAcceptedCold,
    bool SessionLineageWitnessed,
    bool ListeningFrameReceived,
    bool SliMembraneInterpretedPredicatePressure,
    bool CompassOrientedPressure,
    bool CompassCoolingRequired,
    bool SoulFrameReceivedListeningFrame,
    bool AgentiCoreReceivedCompassPressure,
    bool ThinkingAboutThinkingTelemetryProduced,
    bool StewardReviewed,
    bool GelPromotionAllowed,
    bool GelAdmissionAllowed,
    bool EngramAdmissionAllowed,
    bool MemoryAdmissionAllowed,
    bool SelfGelMutationAllowed,
    bool ContinuityAdmissionAllowed,
    bool AuthorityGranted,
    bool ActionAuthorized,
    bool ModelBindingAllowed,
    bool ArbitraryEvaluationAllowed,
    bool RuntimeActionAllowed,
    bool ActivationAllowed,
    bool CmeActualActivationAllowed,
    bool SanctuaryActualActivationAllowed,
    int? ExitCode,
    string StandardOutput,
    string StandardError,
    DateTimeOffset TimestampUtc)
{
    [JsonIgnore]
    public bool IsLabGelPreAdmissionEngrammitization =>
        Disposition == SliLispLabGelEngrammitizationDisposition.CompletedCold &&
        BoundedEntrypointCalled &&
        LoadAttempted &&
        LoadSucceeded &&
        LabGelEngrammitizationCompleted &&
        LabGelPredicateFormed &&
        LabGelPredicateCount == 6 &&
        LabGelPredicateClasses.Count == 6 &&
        EngramCandidateFormed &&
        EngramCandidatePreAdmissionOnly &&
        EvidenceBodyFormed &&
        WitnessBodyFormed &&
        CoolingHeld &&
        PreAdmissionReviewRequired &&
        LabGelReadbackAvailable &&
        LabGelReadbackPreAdmissionOnly &&
        TypedScopeAccepted &&
        SourceWarmUseAcceptedCold &&
        SessionLineageWitnessed &&
        ListeningFrameReceived &&
        SliMembraneInterpretedPredicatePressure &&
        CompassOrientedPressure &&
        CompassCoolingRequired &&
        SoulFrameReceivedListeningFrame &&
        AgentiCoreReceivedCompassPressure &&
        ThinkingAboutThinkingTelemetryProduced &&
        StewardReviewed &&
        !GelPromotionAllowed &&
        !GelAdmissionAllowed &&
        !EngramAdmissionAllowed &&
        !MemoryAdmissionAllowed &&
        !SelfGelMutationAllowed &&
        !ContinuityAdmissionAllowed &&
        !AuthorityGranted &&
        !ActionAuthorized &&
        !ModelBindingAllowed &&
        !ArbitraryEvaluationAllowed &&
        !RuntimeActionAllowed &&
        !ActivationAllowed &&
        !CmeActualActivationAllowed &&
        !SanctuaryActualActivationAllowed;
}
