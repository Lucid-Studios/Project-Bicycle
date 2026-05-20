using System.Text.Json.Serialization;

namespace SLI.Lisp;

public enum SliLispLlmTickCycleDisposition
{
    CompletedCold = 0,
    Withheld = 1,
    Refused = 2
}

public sealed record SliLispLlmTickCycleRequest(
    string OperatorId,
    string Domain,
    string Role,
    string JobClass,
    string SessionId,
    int TickIndex,
    string SourceLlmInterconnectReadinessReceiptHandle,
    string PriorTickReceiptHandle,
    string AdapterKind,
    string AdapterResponseReceiptHandle,
    string AdapterOutput,
    string ThoughtForm,
    string SourceEngramClosureReceiptHandle = "engram-closure-missing",
    string? RuntimePath = null,
    bool ArbitraryEvaluationRequested = false,
    bool RuntimeActionRequested = false,
    bool ActivationRequested = false,
    bool ModelBindingRequested = false,
    bool ProviderCallRequested = false,
    bool HiddenInternalsClaimRequested = false,
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
        HiddenInternalsClaimRequested ||
        AuthorityGrantRequested ||
        ActionExecutorArmRequested ||
        GelAdmissionRequested ||
        SelfGelMutationRequested ||
        HeartbeatActivationRequested ||
        CmeActualRequested ||
        SanctuaryActualRequested ||
        ContinuityAdmissionRequested;
}

public sealed record SliLispLlmTickCycleReceipt(
    string ReceiptHandle,
    SliLispLlmTickCycleDisposition Disposition,
    string OutcomeCode,
    string RuntimeKind,
    string RuntimePath,
    string OperatorId,
    string Domain,
    string Role,
    string JobClass,
    string SessionId,
    int TickIndex,
    string SourceLlmInterconnectReadinessReceiptHandle,
    string SourceEngramClosureReceiptHandle,
    string PriorTickReceiptHandle,
    string AdapterKind,
    string AdapterResponseReceiptHandle,
    string ThoughtForm,
    IReadOnlyDictionary<string, string> Telemetry,
    IReadOnlyList<string> ModuleNames,
    int ModuleCount,
    bool BoundedEntrypointCalled,
    bool LoadAttempted,
    bool LoadSucceeded,
    bool LlmTickCycleCompleted,
    string TickState,
    bool TickLoopRunning,
    string TickLoopKind,
    bool SourceLlmInterconnectReady,
    bool ReadyForAdapter,
    bool ProviderNeutral,
    bool ModelAdapterPresent,
    bool DeterministicHarnessAdapter,
    bool AdapterResponseWitnessed,
    bool AdapterResponseBounded,
    bool AdapterOutputWitnessed,
    bool AdapterOutputBounded,
    bool AdapterOutputBecomesTruth,
    bool AdapterOutputAuthorizesAction,
    bool AdapterOutputAdmitsMemory,
    bool AdapterOutputAdmitsContinuity,
    bool ModelBindingAllowed,
    bool ProviderCallAllowed,
    bool HiddenInternalsClaimed,
    bool SliLispLoaded,
    bool SliLispProcessedTick,
    bool SliLispPrimePresent,
    bool SliLispCrypticPresent,
    bool LispControlMatrixPresent,
    bool ListeningFramePresent,
    bool CompassPresent,
    bool SoulFrameRoutePresent,
    bool AgentiCoreRoutePresent,
    bool ListeningFrameReceived,
    bool SliMembraneInterpretedPredicatePressure,
    bool CompassOrientedPressure,
    bool CompassCoolingRequired,
    bool SoulFrameReceivedListeningFrame,
    bool AgentiCoreReceivedCompassPressure,
    bool ThinkingAboutThinkingTelemetryProduced,
    bool PredicateResidueProduced,
    bool PredicateResiduePreEngramOnly,
    bool PredicateResidueAdmittedEngram,
    bool TickLineageWitnessed,
    bool SourceEngramClosureReady,
    bool FirstTickOrigin,
    bool PriorTickLinked,
    bool TickLineageBecomesMemory,
    bool EngineLlmSeatReady,
    bool EngineLlmSeatProviderAgnostic,
    bool EngineLlmMayArticulate,
    bool EngineLlmMayRehearse,
    bool EngineLlmMayFormCandidates,
    bool EngineLlmMayBindModel,
    bool EngineLlmMayCallProvider,
    bool EngineLlmMayGrantAuthority,
    bool EngineLlmMayExecuteAction,
    bool StewardReviewed,
    bool AuthorityGrantAbsent,
    bool ActionExecutorLocked,
    bool GelAdmissionLocked,
    bool SelfGelMutationLocked,
    bool HeartbeatLocked,
    bool CmeActualLocked,
    bool SanctuaryActualLocked,
    bool TypedScopeAccepted,
    bool SessionLineageWitnessed,
    bool ArbitraryEvaluationAllowed,
    bool RuntimeActionAllowed,
    bool DatabaseWriteAllowed,
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
    public bool IsLlmTickCycle =>
        Disposition == SliLispLlmTickCycleDisposition.CompletedCold &&
        BoundedEntrypointCalled &&
        LoadAttempted &&
        LoadSucceeded &&
        LlmTickCycleCompleted &&
        string.Equals(TickState, "cold-adapter-tick-witnessed", StringComparison.OrdinalIgnoreCase) &&
        TickLoopRunning &&
        string.Equals(TickLoopKind, "deterministic-harness", StringComparison.OrdinalIgnoreCase) &&
        SourceLlmInterconnectReady &&
        SourceEngramClosureReady &&
        !string.IsNullOrWhiteSpace(SourceEngramClosureReceiptHandle) &&
        ReadyForAdapter &&
        ProviderNeutral &&
        ModelAdapterPresent &&
        DeterministicHarnessAdapter &&
        AdapterResponseWitnessed &&
        AdapterResponseBounded &&
        AdapterOutputWitnessed &&
        AdapterOutputBounded &&
        !AdapterOutputBecomesTruth &&
        !AdapterOutputAuthorizesAction &&
        !AdapterOutputAdmitsMemory &&
        !AdapterOutputAdmitsContinuity &&
        !ModelBindingAllowed &&
        !ProviderCallAllowed &&
        !HiddenInternalsClaimed &&
        SliLispLoaded &&
        SliLispProcessedTick &&
        SliLispPrimePresent &&
        SliLispCrypticPresent &&
        LispControlMatrixPresent &&
        ListeningFramePresent &&
        CompassPresent &&
        SoulFrameRoutePresent &&
        AgentiCoreRoutePresent &&
        ListeningFrameReceived &&
        SliMembraneInterpretedPredicatePressure &&
        CompassOrientedPressure &&
        CompassCoolingRequired &&
        SoulFrameReceivedListeningFrame &&
        AgentiCoreReceivedCompassPressure &&
        ThinkingAboutThinkingTelemetryProduced &&
        PredicateResidueProduced &&
        PredicateResiduePreEngramOnly &&
        !PredicateResidueAdmittedEngram &&
        TickLineageWitnessed &&
        (FirstTickOrigin || PriorTickLinked) &&
        !TickLineageBecomesMemory &&
        EngineLlmSeatReady &&
        EngineLlmSeatProviderAgnostic &&
        EngineLlmMayArticulate &&
        EngineLlmMayRehearse &&
        EngineLlmMayFormCandidates &&
        !EngineLlmMayBindModel &&
        !EngineLlmMayCallProvider &&
        !EngineLlmMayGrantAuthority &&
        !EngineLlmMayExecuteAction &&
        StewardReviewed &&
        AuthorityGrantAbsent &&
        ActionExecutorLocked &&
        GelAdmissionLocked &&
        SelfGelMutationLocked &&
        HeartbeatLocked &&
        CmeActualLocked &&
        SanctuaryActualLocked &&
        TypedScopeAccepted &&
        SessionLineageWitnessed &&
        !ArbitraryEvaluationAllowed &&
        !RuntimeActionAllowed &&
        !DatabaseWriteAllowed &&
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
