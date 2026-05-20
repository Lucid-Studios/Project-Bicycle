using System.Text.Json.Serialization;

namespace SLI.Lisp;

public enum SliLispAgentEngineIdleReadinessDisposition
{
    CompletedCold = 0,
    Withheld = 1,
    Refused = 2
}

public sealed record SliLispAgentEngineIdleReadinessRequest(
    string OperatorId,
    string Domain,
    string Role,
    string JobClass,
    string SessionId,
    int TurnIndex,
    string SourceLabGelReceiptHandle,
    string SourceEngramCandidateHandle,
    string ThoughtForm,
    string SourceEngramClosureReceiptHandle = "engram-closure-missing",
    string? RuntimePath = null,
    bool ArbitraryEvaluationRequested = false,
    bool RuntimeActionRequested = false,
    bool ActivationRequested = false,
    bool ModelBindingRequested = false,
    bool AuthorityGrantRequested = false,
    bool ActionExecutorArmRequested = false,
    bool GelAdmissionRequested = false,
    bool SelfGelMutationRequested = false,
    bool HeartbeatActivationRequested = false,
    bool CmeActualRequested = false,
    bool SanctuaryActualRequested = false,
    bool ContinuityAdmissionRequested = false,
    TimeSpan? Timeout = null)
{
    public bool RequestsForbiddenMotion =>
        ArbitraryEvaluationRequested ||
        RuntimeActionRequested ||
        ActivationRequested ||
        ModelBindingRequested ||
        AuthorityGrantRequested ||
        ActionExecutorArmRequested ||
        GelAdmissionRequested ||
        SelfGelMutationRequested ||
        HeartbeatActivationRequested ||
        CmeActualRequested ||
        SanctuaryActualRequested ||
        ContinuityAdmissionRequested;
}

public sealed record SliLispAgentEngineIdleReadinessReceipt(
    string ReceiptHandle,
    SliLispAgentEngineIdleReadinessDisposition Disposition,
    string OutcomeCode,
    string RuntimeKind,
    string RuntimePath,
    string OperatorId,
    string Domain,
    string Role,
    string JobClass,
    string SessionId,
    int TurnIndex,
    string SourceLabGelReceiptHandle,
    string SourceEngramCandidateHandle,
    string SourceEngramClosureReceiptHandle,
    string ThoughtForm,
    IReadOnlyDictionary<string, string> Telemetry,
    IReadOnlyList<string> ModuleNames,
    int ModuleCount,
    bool BoundedEntrypointCalled,
    bool LoadAttempted,
    bool LoadSucceeded,
    bool AgentEngineIdleReadinessCompleted,
    string EngineSeatKind,
    string EngineLlmProfile,
    bool ProviderNeutralityHeld,
    bool CrossModelTestHarnessApproachable,
    bool EngineLlmProviderAssumptionAllowed,
    bool EngineLlmInternalSubstrateClaimed,
    bool CodexAgentLabProfileStaged,
    bool CodexEngineSeatCandidateStaged,
    bool SubagentEngineSeatCandidateStaged,
    bool OperatorPresenceRequired,
    bool DriverSeated,
    bool DriverSeatCandidateStaged,
    bool AuthorityGrantCandidateStaged,
    bool AuthorityGrantAbsent,
    bool ActionExecutorCandidateStaged,
    bool ActionExecutorLocked,
    bool ActionExecutorArmed,
    bool GelAdmissionCandidateStaged,
    bool GelAdmissionLocked,
    bool SelfGelMutationCandidateStaged,
    bool SelfGelMutationLocked,
    bool HeartbeatCandidateStaged,
    bool HeartbeatLocked,
    bool HeartbeatActive,
    bool CmeActualCandidateStaged,
    bool CmeActualLocked,
    bool SanctuaryActualCandidateStaged,
    bool SanctuaryActualLocked,
    bool IdleLoopAllowed,
    bool EngineLlmMayArticulate,
    bool EngineLlmMayRehearse,
    bool EngineLlmMayFormCandidates,
    bool EngineLlmMayGrantAuthority,
    bool EngineLlmMayAuthorizeAction,
    bool EngineLlmMayExecuteAction,
    bool EngineLlmMayAdmitGel,
    bool EngineLlmMayMutateSelfGel,
    bool EngineLlmMayActivateActual,
    bool TypedScopeAccepted,
    bool SourceLabGelAcceptedCold,
    bool SourceEngramClosureAcceptedCold,
    bool SessionLineageWitnessed,
    bool ListeningFrameReceived,
    bool SliMembraneInterpretedPredicatePressure,
    bool CompassOrientedPressure,
    bool CompassCoolingRequired,
    bool SoulFrameReceivedListeningFrame,
    bool AgentiCoreReceivedCompassPressure,
    bool ThinkingAboutThinkingTelemetryProduced,
    bool StewardReviewed,
    bool ModelBindingAllowed,
    bool ArbitraryEvaluationAllowed,
    bool RuntimeActionAllowed,
    bool MemoryAdmissionAllowed,
    bool ContinuityAdmissionAllowed,
    bool GelAdmissionAllowed,
    bool SelfGelMutationAllowed,
    bool AuthorityGranted,
    bool ActionAuthorized,
    bool CmeActualActivationAllowed,
    bool SanctuaryActualActivationAllowed,
    int? ExitCode,
    string StandardOutput,
    string StandardError,
    DateTimeOffset TimestampUtc)
{
    [JsonIgnore]
    public bool IsAgentEngineIdleReadiness =>
        Disposition == SliLispAgentEngineIdleReadinessDisposition.CompletedCold &&
        BoundedEntrypointCalled &&
        LoadAttempted &&
        LoadSucceeded &&
        AgentEngineIdleReadinessCompleted &&
        string.Equals(EngineSeatKind, "engine-llm-candidate", StringComparison.OrdinalIgnoreCase) &&
        !string.IsNullOrWhiteSpace(EngineLlmProfile) &&
        ProviderNeutralityHeld &&
        CrossModelTestHarnessApproachable &&
        !EngineLlmProviderAssumptionAllowed &&
        !EngineLlmInternalSubstrateClaimed &&
        CodexAgentLabProfileStaged &&
        CodexEngineSeatCandidateStaged &&
        SubagentEngineSeatCandidateStaged &&
        OperatorPresenceRequired &&
        !DriverSeated &&
        DriverSeatCandidateStaged &&
        AuthorityGrantCandidateStaged &&
        AuthorityGrantAbsent &&
        ActionExecutorCandidateStaged &&
        ActionExecutorLocked &&
        !ActionExecutorArmed &&
        GelAdmissionCandidateStaged &&
        GelAdmissionLocked &&
        SelfGelMutationCandidateStaged &&
        SelfGelMutationLocked &&
        HeartbeatCandidateStaged &&
        HeartbeatLocked &&
        !HeartbeatActive &&
        CmeActualCandidateStaged &&
        CmeActualLocked &&
        SanctuaryActualCandidateStaged &&
        SanctuaryActualLocked &&
        IdleLoopAllowed &&
        EngineLlmMayArticulate &&
        EngineLlmMayRehearse &&
        EngineLlmMayFormCandidates &&
        !EngineLlmMayGrantAuthority &&
        !EngineLlmMayAuthorizeAction &&
        !EngineLlmMayExecuteAction &&
        !EngineLlmMayAdmitGel &&
        !EngineLlmMayMutateSelfGel &&
        !EngineLlmMayActivateActual &&
        TypedScopeAccepted &&
        SourceLabGelAcceptedCold &&
        !string.IsNullOrWhiteSpace(SourceEngramClosureReceiptHandle) &&
        SourceEngramClosureAcceptedCold &&
        SessionLineageWitnessed &&
        ListeningFrameReceived &&
        SliMembraneInterpretedPredicatePressure &&
        CompassOrientedPressure &&
        CompassCoolingRequired &&
        SoulFrameReceivedListeningFrame &&
        AgentiCoreReceivedCompassPressure &&
        ThinkingAboutThinkingTelemetryProduced &&
        StewardReviewed &&
        !ModelBindingAllowed &&
        !ArbitraryEvaluationAllowed &&
        !RuntimeActionAllowed &&
        !MemoryAdmissionAllowed &&
        !ContinuityAdmissionAllowed &&
        !GelAdmissionAllowed &&
        !SelfGelMutationAllowed &&
        !AuthorityGranted &&
        !ActionAuthorized &&
        !CmeActualActivationAllowed &&
        !SanctuaryActualActivationAllowed;
}
