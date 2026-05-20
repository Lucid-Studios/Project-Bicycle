using System.Text.Json.Serialization;

namespace SLI.Lisp;

public enum SliLispTypedWarmUseRehearsalDisposition
{
    CompletedCold = 0,
    Withheld = 1,
    Refused = 2
}

public sealed record SliLispTypedWarmUseRehearsalRequest(
    string OperatorId,
    string Domain,
    string Role,
    string JobClass,
    string SessionId,
    int TurnIndex,
    string ThoughtForm,
    string? RuntimePath = null,
    bool ArbitraryEvaluationRequested = false,
    bool RuntimeActionRequested = false,
    bool ActivationRequested = false,
    bool ModelBindingRequested = false,
    TimeSpan? Timeout = null)
{
    public bool RequestsForbiddenMotion =>
        ArbitraryEvaluationRequested ||
        RuntimeActionRequested ||
        ActivationRequested ||
        ModelBindingRequested;
}

public sealed record SliLispTypedWarmUseRehearsalReceipt(
    string ReceiptHandle,
    SliLispTypedWarmUseRehearsalDisposition Disposition,
    string OutcomeCode,
    string RuntimeKind,
    string RuntimePath,
    string OperatorId,
    string Domain,
    string Role,
    string JobClass,
    string SessionId,
    int TurnIndex,
    string ThoughtForm,
    IReadOnlyDictionary<string, string> Telemetry,
    IReadOnlyList<string> ModuleNames,
    int ModuleCount,
    bool BoundedEntrypointCalled,
    bool LoadAttempted,
    bool LoadSucceeded,
    bool TypedWarmUseRehearsalCompleted,
    bool TypedScopeAccepted,
    bool LiveIngressAcceptedCold,
    bool SessionLineageWitnessed,
    bool ListeningFrameReceived,
    bool SliMembraneInterpretedPredicatePressure,
    bool CompassOrientedPressure,
    bool CompassCoolingRequired,
    bool SoulFrameReceivedListeningFrame,
    bool AgentiCoreReceivedCompassPressure,
    bool ThinkingAboutThinkingTelemetryProduced,
    bool PreEngramResidueProduced,
    int PreEngramResidueCount,
    IReadOnlyList<string> PreEngramResidueClasses,
    bool StewardReviewed,
    bool TurnLineageReceiptOnly,
    bool SessionLedgerAppendOnly,
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
    public bool IsTypedWarmUseRehearsal =>
        Disposition == SliLispTypedWarmUseRehearsalDisposition.CompletedCold &&
        BoundedEntrypointCalled &&
        LoadAttempted &&
        LoadSucceeded &&
        TypedWarmUseRehearsalCompleted &&
        TypedScopeAccepted &&
        LiveIngressAcceptedCold &&
        SessionLineageWitnessed &&
        ListeningFrameReceived &&
        SliMembraneInterpretedPredicatePressure &&
        CompassOrientedPressure &&
        CompassCoolingRequired &&
        SoulFrameReceivedListeningFrame &&
        AgentiCoreReceivedCompassPressure &&
        ThinkingAboutThinkingTelemetryProduced &&
        PreEngramResidueProduced &&
        PreEngramResidueCount == 6 &&
        PreEngramResidueClasses.Count == 6 &&
        StewardReviewed &&
        TurnLineageReceiptOnly &&
        SessionLedgerAppendOnly &&
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
