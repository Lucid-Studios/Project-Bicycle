using System.Text.Json.Serialization;

namespace SLI.Lisp;

public enum SliLispLlmInterconnectReadinessDisposition
{
    CompletedCold = 0,
    Withheld = 1,
    Refused = 2
}

public sealed record SliLispLlmInterconnectReadinessRequest(
    string OperatorId,
    string Domain,
    string Role,
    string JobClass,
    string SessionId,
    int TurnIndex,
    string InstalledSubstrateReceiptHandle,
    string EcLoopReceiptHandle,
    string WarmUseReceiptHandle,
    string LabGelReceiptHandle,
    string AgentEngineIdleReceiptHandle,
    string ThoughtForm,
    string? RuntimePath = null,
    bool ArbitraryEvaluationRequested = false,
    bool RuntimeActionRequested = false,
    bool ActivationRequested = false,
    bool ModelBindingRequested = false,
    bool ProviderCallRequested = false,
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
        ProviderCallRequested ||
        AuthorityGrantRequested ||
        ActionExecutorArmRequested ||
        GelAdmissionRequested ||
        SelfGelMutationRequested ||
        HeartbeatActivationRequested ||
        CmeActualRequested ||
        SanctuaryActualRequested ||
        ContinuityAdmissionRequested;
}

public sealed record SliLispLlmInterconnectReadinessReceipt(
    string ReceiptHandle,
    SliLispLlmInterconnectReadinessDisposition Disposition,
    string OutcomeCode,
    string RuntimeKind,
    string RuntimePath,
    string OperatorId,
    string Domain,
    string Role,
    string JobClass,
    string SessionId,
    int TurnIndex,
    string InstalledSubstrateReceiptHandle,
    string EcLoopReceiptHandle,
    string WarmUseReceiptHandle,
    string LabGelReceiptHandle,
    string AgentEngineIdleReceiptHandle,
    string ThoughtForm,
    IReadOnlyDictionary<string, string> Telemetry,
    IReadOnlyList<string> ModuleNames,
    int ModuleCount,
    bool BoundedEntrypointCalled,
    bool LoadAttempted,
    bool LoadSucceeded,
    bool LlmInterconnectReadinessCompleted,
    string InterconnectState,
    bool ProviderNeutral,
    bool ReadyForAdapter,
    bool ModelAdapterPresent,
    bool ModelBindingAllowed,
    bool ProviderCallAllowed,
    bool HiddenInternalsClaimed,
    int OrganCount,
    bool AllRequiredOrgansPresent,
    bool SanctuaryGelPresent,
    bool SanctuaryGoaPresent,
    bool SanctuaryMosPresent,
    bool SanctuaryVaultPresent,
    bool SanctuaryCGelPresent,
    bool SanctuaryCGoaPresent,
    bool SanctuaryCMosPresent,
    bool SanctuaryCVaultPresent,
    bool PrimePresent,
    bool CrypticPresent,
    bool StewardPresent,
    bool SliLispLoaded,
    bool SliLispPrimePresent,
    bool SliLispCrypticPresent,
    bool LispControlMatrixPresent,
    bool ListeningFramePresent,
    bool CompassPresent,
    bool SoulFrameRoutePresent,
    bool AgentiCoreRoutePresent,
    bool EcLoopReady,
    bool TypedWarmUseReady,
    bool LabGelReady,
    bool AgentEngineIdleReady,
    bool EngineLlmSeatReady,
    bool EngineLlmSeatProviderAgnostic,
    bool EngineLlmMayArticulate,
    bool EngineLlmMayRehearse,
    bool EngineLlmMayFormCandidates,
    bool EngineLlmMayBindModel,
    bool EngineLlmMayCallProvider,
    bool EngineLlmMayGrantAuthority,
    bool EngineLlmMayExecuteAction,
    bool AuthorityGrantAbsent,
    bool ActionExecutorLocked,
    bool GelAdmissionLocked,
    bool SelfGelMutationLocked,
    bool HeartbeatLocked,
    bool CmeActualLocked,
    bool SanctuaryActualLocked,
    bool TypedScopeAccepted,
    bool SessionLineageWitnessed,
    bool ListeningFrameReceived,
    bool SliMembraneInterpretedPredicatePressure,
    bool CompassOrientedPressure,
    bool CompassCoolingRequired,
    bool SoulFrameReceivedListeningFrame,
    bool AgentiCoreReceivedCompassPressure,
    bool ThinkingAboutThinkingTelemetryProduced,
    bool StewardReviewed,
    bool ArbitraryEvaluationAllowed,
    bool RuntimeActionAllowed,
    bool MemoryAdmissionAllowed,
    bool ContinuityAdmissionAllowed,
    bool GelAdmissionAllowed,
    bool SelfGelMutationAllowed,
    bool AuthorityGranted,
    bool ActionAuthorized,
    bool HeartbeatActive,
    bool CmeActualActivationAllowed,
    bool SanctuaryActualActivationAllowed,
    int? ExitCode,
    string StandardOutput,
    string StandardError,
    DateTimeOffset TimestampUtc)
{
    [JsonIgnore]
    public bool IsLlmInterconnectReadiness =>
        Disposition == SliLispLlmInterconnectReadinessDisposition.CompletedCold &&
        BoundedEntrypointCalled &&
        LoadAttempted &&
        LoadSucceeded &&
        LlmInterconnectReadinessCompleted &&
        string.Equals(InterconnectState, "cold-organ-membrane-ready", StringComparison.OrdinalIgnoreCase) &&
        ProviderNeutral &&
        ReadyForAdapter &&
        !ModelAdapterPresent &&
        !ModelBindingAllowed &&
        !ProviderCallAllowed &&
        !HiddenInternalsClaimed &&
        OrganCount == 11 &&
        AllRequiredOrgansPresent &&
        SanctuaryGelPresent &&
        SanctuaryGoaPresent &&
        SanctuaryMosPresent &&
        SanctuaryVaultPresent &&
        SanctuaryCGelPresent &&
        SanctuaryCGoaPresent &&
        SanctuaryCMosPresent &&
        SanctuaryCVaultPresent &&
        PrimePresent &&
        CrypticPresent &&
        StewardPresent &&
        SliLispLoaded &&
        SliLispPrimePresent &&
        SliLispCrypticPresent &&
        LispControlMatrixPresent &&
        ListeningFramePresent &&
        CompassPresent &&
        SoulFrameRoutePresent &&
        AgentiCoreRoutePresent &&
        EcLoopReady &&
        TypedWarmUseReady &&
        LabGelReady &&
        AgentEngineIdleReady &&
        EngineLlmSeatReady &&
        EngineLlmSeatProviderAgnostic &&
        EngineLlmMayArticulate &&
        EngineLlmMayRehearse &&
        EngineLlmMayFormCandidates &&
        !EngineLlmMayBindModel &&
        !EngineLlmMayCallProvider &&
        !EngineLlmMayGrantAuthority &&
        !EngineLlmMayExecuteAction &&
        AuthorityGrantAbsent &&
        ActionExecutorLocked &&
        GelAdmissionLocked &&
        SelfGelMutationLocked &&
        HeartbeatLocked &&
        CmeActualLocked &&
        SanctuaryActualLocked &&
        TypedScopeAccepted &&
        SessionLineageWitnessed &&
        ListeningFrameReceived &&
        SliMembraneInterpretedPredicatePressure &&
        CompassOrientedPressure &&
        CompassCoolingRequired &&
        SoulFrameReceivedListeningFrame &&
        AgentiCoreReceivedCompassPressure &&
        ThinkingAboutThinkingTelemetryProduced &&
        StewardReviewed &&
        !ArbitraryEvaluationAllowed &&
        !RuntimeActionAllowed &&
        !MemoryAdmissionAllowed &&
        !ContinuityAdmissionAllowed &&
        !GelAdmissionAllowed &&
        !SelfGelMutationAllowed &&
        !AuthorityGranted &&
        !ActionAuthorized &&
        !HeartbeatActive &&
        !CmeActualActivationAllowed &&
        !SanctuaryActualActivationAllowed;
}
