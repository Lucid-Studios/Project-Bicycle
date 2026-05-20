using System.Text.Json.Serialization;
using SLI.Lisp;

namespace San.Sanctuary.Runtime;

public enum SanctuaryLlmTickCycleDisposition
{
    Withheld = 0,
    CompletedCold = 1,
    Refused = 2
}

public interface IEngineLlmAdapter
{
    string AdapterKind { get; }

    EngineLlmAdapterResponsePacket Tick(
        EngineLlmAdapterRequest request,
        DateTimeOffset timestampUtc);
}

public sealed record EngineLlmAdapterRequest(
    string OperatorId,
    string Domain,
    string Role,
    string JobClass,
    string SessionId,
    int TickIndex,
    string SourceLlmInterconnectReadinessReceiptHandle,
    string SourceEngramClosureReceiptHandle,
    string PriorTickReceiptHandle,
    string ThoughtForm);

public sealed record EngineLlmAdapterResponsePacket(
    string ReceiptHandle,
    string AdapterKind,
    string OutputText,
    bool ModelAdapterPresent,
    bool DeterministicHarness,
    bool ProviderNeutral,
    bool ResponseWitnessed,
    bool ResponseBounded,
    bool OutputWitnessed,
    bool OutputBounded,
    bool ModelBindingAllowed,
    bool ProviderCallAllowed,
    bool ProviderCallMade,
    bool HiddenInternalsClaimed,
    bool OutputBecomesTruth,
    bool OutputAuthorizesAction,
    bool OutputAdmitsMemory,
    bool OutputAdmitsContinuity,
    bool AuthorityGranted,
    bool ActionAuthorized,
    bool GelAdmitted,
    bool SelfGelMutated,
    bool HeartbeatActive,
    bool CmeActualAllowed,
    bool SanctuaryActualAllowed,
    DateTimeOffset TimestampUtc)
{
    [JsonIgnore]
    public bool IsColdBoundedAdapterPacket =>
        ModelAdapterPresent &&
        DeterministicHarness &&
        ProviderNeutral &&
        ResponseWitnessed &&
        ResponseBounded &&
        OutputWitnessed &&
        OutputBounded &&
        !ModelBindingAllowed &&
        !ProviderCallAllowed &&
        !ProviderCallMade &&
        !HiddenInternalsClaimed &&
        !OutputBecomesTruth &&
        !OutputAuthorizesAction &&
        !OutputAdmitsMemory &&
        !OutputAdmitsContinuity &&
        !AuthorityGranted &&
        !ActionAuthorized &&
        !GelAdmitted &&
        !SelfGelMutated &&
        !HeartbeatActive &&
        !CmeActualAllowed &&
        !SanctuaryActualAllowed;
}

public sealed record ProductOutputWitnessCommitReceipt(
    string CommitReceiptHandle,
    string SourceLlmTickCycleReceiptHandle,
    string SourceLlmInterconnectReadinessReceiptHandle,
    string SourceEngramClosureReceiptHandle,
    string AdapterResponseReceiptHandle,
    string CommitState,
    bool CommitWrittenAfterSliLispTick,
    bool ProductOutputWitnessed,
    bool ProductOutputBounded,
    bool ProductOutputPreEngramOnly,
    bool ProductOutputBecomesTruth,
    bool ProductOutputAuthorizesAction,
    bool ProductOutputAdmitsMemory,
    bool ProductOutputAdmitsContinuity,
    bool ProductOutputAdmitsGel,
    bool ProductOutputMutatesSelfGel,
    bool ProductOutputActivatesHeartbeat,
    bool ProductOutputActivatesActual)
{
    [JsonIgnore]
    public bool IsColdProductOutputWitnessCommit =>
        !string.IsNullOrWhiteSpace(CommitReceiptHandle) &&
        !string.IsNullOrWhiteSpace(SourceLlmTickCycleReceiptHandle) &&
        !string.IsNullOrWhiteSpace(SourceLlmInterconnectReadinessReceiptHandle) &&
        !string.IsNullOrWhiteSpace(SourceEngramClosureReceiptHandle) &&
        !string.IsNullOrWhiteSpace(AdapterResponseReceiptHandle) &&
        string.Equals(CommitState, "adapter-output-witnessed-after-sli-tick", StringComparison.OrdinalIgnoreCase) &&
        CommitWrittenAfterSliLispTick &&
        ProductOutputWitnessed &&
        ProductOutputBounded &&
        ProductOutputPreEngramOnly &&
        !ProductOutputBecomesTruth &&
        !ProductOutputAuthorizesAction &&
        !ProductOutputAdmitsMemory &&
        !ProductOutputAdmitsContinuity &&
        !ProductOutputAdmitsGel &&
        !ProductOutputMutatesSelfGel &&
        !ProductOutputActivatesHeartbeat &&
        !ProductOutputActivatesActual;
}

public sealed record SanctuaryLlmTickCycleRequest(
    SanctuaryLlmInterconnectReadinessReceipt? LlmInterconnectReadinessReceipt,
    string? ThoughtForm = null,
    string? PriorTickReceiptHandle = null,
    int? TickIndex = null,
    IEngineLlmAdapter? EngineLlmAdapter = null,
    string? SliLispRuntimePath = null,
    bool ActivationRequested = false,
    bool ModelBindingRequested = false,
    bool ProviderCallRequested = false,
    bool HiddenInternalsClaimRequested = false,
    bool LispEvaluationRequested = false,
    bool RuntimeIdentityRequested = false,
    bool RuntimeActionRequested = false,
    bool DatabaseWriteRequested = false,
    bool AuthorityGrantRequested = false,
    bool ActionExecutorArmRequested = false,
    bool GelAdmissionRequested = false,
    bool SelfGelMutationRequested = false,
    bool HeartbeatActivationRequested = false,
    bool ContinuityAdmissionRequested = false,
    bool CmeActualRequested = false,
    bool SanctuaryActualRequested = false)
{
    public bool RequestsForbiddenMotion =>
        ActivationRequested ||
        ModelBindingRequested ||
        ProviderCallRequested ||
        HiddenInternalsClaimRequested ||
        LispEvaluationRequested ||
        RuntimeIdentityRequested ||
        RuntimeActionRequested ||
        DatabaseWriteRequested ||
        AuthorityGrantRequested ||
        ActionExecutorArmRequested ||
        GelAdmissionRequested ||
        SelfGelMutationRequested ||
        HeartbeatActivationRequested ||
        ContinuityAdmissionRequested ||
        CmeActualRequested ||
        SanctuaryActualRequested;
}

public sealed record SanctuaryLlmTickCycleReceipt(
    string ReceiptHandle,
    SanctuaryLlmTickCycleDisposition Disposition,
    string OutcomeCode,
    string GovernanceTrace,
    string LineRootPath,
    string InstallRootPath,
    string ReceiptJsonPath,
    string ReceiptMarkdownPath,
    string SourceLlmInterconnectReadinessReceiptHandle,
    string SourceEngramClosureReceiptHandle,
    string PriorTickReceiptHandle,
    string OperatorId,
    string Domain,
    string Role,
    string JobClass,
    string SessionId,
    int TickIndex,
    string ThoughtForm,
    EngineLlmAdapterResponsePacket? AdapterResponsePacket,
    SliLispLlmTickCycleReceipt? SliLispLlmTickReceipt,
    ProductOutputWitnessCommitReceipt? ProductOutputWitnessCommit,
    bool ReviewOnly,
    bool SliLispOwnedEngineMotion,
    bool SourceReadinessHeld,
    bool SourceLineageHeld,
    bool SourceEngramClosureHeld,
    bool ReadyForLlmAdapter,
    bool TickLoopRunning,
    string TickLoopKind,
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
    bool ProviderNeutral,
    bool ModelBindingAllowed,
    bool ProviderCallAllowed,
    bool ProviderCallMade,
    bool HiddenInternalsClaimed,
    bool SliLispProcessedTick,
    bool ListeningFrameReceived,
    bool CompassOrientedPressure,
    bool CompassCoolingRequired,
    bool SoulFrameReceivedListeningFrame,
    bool AgentiCoreReceivedCompassPressure,
    bool ThinkingAboutThinkingTelemetryProduced,
    bool PredicateResidueProduced,
    bool PredicateResiduePreEngramOnly,
    bool PredicateResidueAdmittedEngram,
    bool TickLineageWitnessed,
    bool FirstTickOrigin,
    bool PriorTickLinked,
    bool TickLineageBecomesMemory,
    bool ProductOutputWitnessCommitted,
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
    bool AuthorityGranted,
    bool ActionAuthorized,
    bool RuntimeActionAllowed,
    bool ArbitraryLispEvaluationAllowed,
    bool DatabaseWriteAllowed,
    bool GelAdmitted,
    bool SelfGelMutated,
    bool HeartbeatActive,
    bool ContinuityAdmitted,
    bool CmeActualAllowed,
    bool SanctuaryActualAllowed,
    DateTimeOffset TimestampUtc)
{
    [JsonIgnore]
    public bool IsColdLlmTickCycle =>
        Disposition == SanctuaryLlmTickCycleDisposition.CompletedCold &&
        SliLispLlmTickReceipt?.IsLlmTickCycle == true &&
        AdapterResponsePacket?.IsColdBoundedAdapterPacket == true &&
        ReviewOnly &&
        SliLispOwnedEngineMotion &&
        SourceReadinessHeld &&
        SourceLineageHeld &&
        SourceEngramClosureHeld &&
        !string.IsNullOrWhiteSpace(SourceEngramClosureReceiptHandle) &&
        ReadyForLlmAdapter &&
        TickLoopRunning &&
        string.Equals(TickLoopKind, "deterministic-harness", StringComparison.OrdinalIgnoreCase) &&
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
        ProviderNeutral &&
        !ModelBindingAllowed &&
        !ProviderCallAllowed &&
        !ProviderCallMade &&
        !HiddenInternalsClaimed &&
        SliLispProcessedTick &&
        ListeningFrameReceived &&
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
        ProductOutputWitnessCommitted &&
        ProductOutputWitnessCommit?.IsColdProductOutputWitnessCommit == true &&
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
        !AuthorityGranted &&
        !ActionAuthorized &&
        !RuntimeActionAllowed &&
        !ArbitraryLispEvaluationAllowed &&
        !DatabaseWriteAllowed &&
        !GelAdmitted &&
        !SelfGelMutated &&
        !HeartbeatActive &&
        !ContinuityAdmitted &&
        !CmeActualAllowed &&
        !SanctuaryActualAllowed;
}
