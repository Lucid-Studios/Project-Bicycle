using System.Text.Json.Serialization;

namespace SLI.Lisp;

public enum SliLispEcTelemetryLoopDisposition
{
    CompletedCold = 0,
    Withheld = 1,
    Refused = 2
}

public sealed record SliLispEcTelemetryLoopRequest(
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

public sealed record SliLispEcTelemetryLoopReceipt(
    string ReceiptHandle,
    SliLispEcTelemetryLoopDisposition Disposition,
    string OutcomeCode,
    string RuntimeKind,
    string RuntimePath,
    string ThoughtForm,
    IReadOnlyDictionary<string, string> Telemetry,
    IReadOnlyList<string> ModuleNames,
    int ModuleCount,
    bool BoundedEntrypointCalled,
    bool LoadAttempted,
    bool LoadSucceeded,
    bool ColdEngineLoopCompleted,
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
    public bool IsColdEcTelemetryLoop =>
        Disposition == SliLispEcTelemetryLoopDisposition.CompletedCold &&
        BoundedEntrypointCalled &&
        LoadAttempted &&
        LoadSucceeded &&
        ColdEngineLoopCompleted &&
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
