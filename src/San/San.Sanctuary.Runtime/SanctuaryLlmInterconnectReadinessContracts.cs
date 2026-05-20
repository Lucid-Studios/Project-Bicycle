using System.Text.Json.Serialization;
using SLI.Lisp;

namespace San.Sanctuary.Runtime;

public enum SanctuaryLlmInterconnectReadinessDisposition
{
    Withheld = 0,
    CompletedCold = 1,
    Refused = 2
}

public sealed record SanctuaryLlmInterconnectReadinessRequest(
    SanctuaryInstalledSubstrateReceipt? InstalledSubstrateReceipt,
    SanctuaryEcTelemetryLoopReceipt? EcLoopReceipt,
    SanctuaryTypedWarmUseRehearsalReceipt? WarmUseReceipt,
    SanctuaryLabGelEngrammitizationReceipt? LabGelReceipt,
    SanctuaryAgentEngineIdleReadinessReceipt? AgentEngineIdleReceipt,
    string? SliLispRuntimePath = null,
    bool ActivationRequested = false,
    bool ModelBindingRequested = false,
    bool ProviderCallRequested = false,
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

public sealed record SanctuaryLlmInterconnectReadinessReceipt(
    string ReceiptHandle,
    SanctuaryLlmInterconnectReadinessDisposition Disposition,
    string OutcomeCode,
    string GovernanceTrace,
    string LineRootPath,
    string InstallRootPath,
    string ReceiptJsonPath,
    string ReceiptMarkdownPath,
    string SourceInstalledSubstrateReceiptHandle,
    string SourceEcLoopReceiptHandle,
    string SourceWarmUseReceiptHandle,
    string SourceLabGelReceiptHandle,
    string SourceAgentEngineIdleReceiptHandle,
    string SourceEngramCandidateHandle,
    string SourceEngramClosureReceiptHandle,
    string SourceLabGelReadbackReceiptHandle,
    string OperatorId,
    string Domain,
    string Role,
    string JobClass,
    string SessionId,
    int TurnIndex,
    string ThoughtForm,
    SliLispLlmInterconnectReadinessReceipt? SliLispLlmInterconnectReceipt,
    bool ReviewOnly,
    bool SliLispOwnedEngineMotion,
    bool InstalledSubstrateReady,
    bool EcLoopReady,
    bool WarmUseReady,
    bool LabGelReady,
    bool AgentEngineIdleReady,
    bool SourceLineageHeld,
    bool SourceEngramClosureHeld,
    bool SourceLabGelReadbackHeld,
    int RequiredOrganCount,
    bool AllRequiredOrgansPresent,
    bool BaseBodiesPresent,
    bool CondensateBodiesPresent,
    bool RoleBodiesPresent,
    bool SliLispLoaded,
    bool SliLispPrimePresent,
    bool SliLispCrypticPresent,
    bool LispControlMatrixPresent,
    bool ListeningFramePresent,
    bool CompassPresent,
    bool SoulFrameRoutePresent,
    bool AgentiCoreRoutePresent,
    bool ProviderNeutral,
    bool ReadyForLlmAdapter,
    bool ModelAdapterPresent,
    bool ModelBindingAllowed,
    bool ProviderCallAllowed,
    bool HiddenInternalsClaimed,
    bool EngineLlmSeatReady,
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
    public bool IsColdLlmInterconnectReady =>
        Disposition == SanctuaryLlmInterconnectReadinessDisposition.CompletedCold &&
        SliLispLlmInterconnectReceipt?.IsLlmInterconnectReadiness == true &&
        ReviewOnly &&
        SliLispOwnedEngineMotion &&
        InstalledSubstrateReady &&
        EcLoopReady &&
        WarmUseReady &&
        LabGelReady &&
        AgentEngineIdleReady &&
        SourceLineageHeld &&
        SourceEngramClosureHeld &&
        SourceLabGelReadbackHeld &&
        !string.IsNullOrWhiteSpace(SourceEngramCandidateHandle) &&
        !string.IsNullOrWhiteSpace(SourceEngramClosureReceiptHandle) &&
        !string.IsNullOrWhiteSpace(SourceLabGelReadbackReceiptHandle) &&
        RequiredOrganCount == SanctuaryInstalledSubstrateReceipt.ExpectedInstalledBodyCount &&
        AllRequiredOrgansPresent &&
        BaseBodiesPresent &&
        CondensateBodiesPresent &&
        RoleBodiesPresent &&
        SliLispLoaded &&
        SliLispPrimePresent &&
        SliLispCrypticPresent &&
        LispControlMatrixPresent &&
        ListeningFramePresent &&
        CompassPresent &&
        SoulFrameRoutePresent &&
        AgentiCoreRoutePresent &&
        ProviderNeutral &&
        ReadyForLlmAdapter &&
        !ModelAdapterPresent &&
        !ModelBindingAllowed &&
        !ProviderCallAllowed &&
        !HiddenInternalsClaimed &&
        EngineLlmSeatReady &&
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
