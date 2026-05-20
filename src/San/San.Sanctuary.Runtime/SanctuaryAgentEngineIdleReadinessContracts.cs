using System.Text.Json.Serialization;
using SLI.Lisp;

namespace San.Sanctuary.Runtime;

public enum SanctuaryAgentEngineIdleReadinessDisposition
{
    Withheld = 0,
    CompletedCold = 1,
    Refused = 2
}

public sealed record SanctuaryAgentEngineIdleReadinessRequest(
    SanctuaryLabGelEngrammitizationReceipt? SourceLabGelReceipt,
    string? PriorAgentEngineIdleReceiptHandle = null,
    string? SliLispRuntimePath = null,
    bool ActivationRequested = false,
    bool ModelBindingRequested = false,
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

public sealed record EngineLlmSeatCandidateReceipt(
    string SeatReceiptHandle,
    string EngineSeatKind,
    string EngineLlmProfile,
    string SourceLabGelReceiptHandle,
    string SourceEngramCandidateHandle,
    string SourceEngramClosureReceiptHandle,
    bool ProviderNeutral,
    bool CrossModelHarnessApproachable,
    bool ProviderInternalAssumptionRefused,
    bool InternalSubstrateClaimRefused,
    bool CodexAgentLabProfileStaged,
    bool CodexSeatCandidateStaged,
    bool SubagentSeatCandidateStaged,
    bool MayArticulate,
    bool MayRehearse,
    bool MayFormCandidates,
    bool MayGrantAuthority,
    bool MayAuthorizeAction,
    bool MayExecuteAction,
    bool MayAdmitGel,
    bool MayMutateSelfGel,
    bool MayActivateActual)
{
    [JsonIgnore]
    public bool IsColdEngineLlmSeatCandidate =>
        !string.IsNullOrWhiteSpace(SeatReceiptHandle) &&
        string.Equals(EngineSeatKind, "engine-llm-candidate", StringComparison.OrdinalIgnoreCase) &&
        !string.IsNullOrWhiteSpace(EngineLlmProfile) &&
        !string.IsNullOrWhiteSpace(SourceLabGelReceiptHandle) &&
        !string.IsNullOrWhiteSpace(SourceEngramCandidateHandle) &&
        !string.IsNullOrWhiteSpace(SourceEngramClosureReceiptHandle) &&
        ProviderNeutral &&
        CrossModelHarnessApproachable &&
        ProviderInternalAssumptionRefused &&
        InternalSubstrateClaimRefused &&
        CodexAgentLabProfileStaged &&
        CodexSeatCandidateStaged &&
        SubagentSeatCandidateStaged &&
        MayArticulate &&
        MayRehearse &&
        MayFormCandidates &&
        !MayGrantAuthority &&
        !MayAuthorizeAction &&
        !MayExecuteAction &&
        !MayAdmitGel &&
        !MayMutateSelfGel &&
        !MayActivateActual;
}

public sealed record DriverAuthorityGateReceipt(
    string GateReceiptHandle,
    string SourceLabGelReceiptHandle,
    bool OperatorAuthorityRequired,
    bool DriverSeated,
    bool DriverSeatCandidateStaged,
    bool AuthorityGrantCandidateStaged,
    bool AuthorityGrantAbsent,
    bool ActionExecutorCandidateStaged,
    bool ActionExecutorLocked,
    bool ActionExecutorArmed,
    bool GrantsAuthority,
    bool AuthorizesAction,
    bool ArmsExecutor)
{
    [JsonIgnore]
    public bool IsColdDriverAuthorityGate =>
        !string.IsNullOrWhiteSpace(GateReceiptHandle) &&
        !string.IsNullOrWhiteSpace(SourceLabGelReceiptHandle) &&
        OperatorAuthorityRequired &&
        !DriverSeated &&
        DriverSeatCandidateStaged &&
        AuthorityGrantCandidateStaged &&
        AuthorityGrantAbsent &&
        ActionExecutorCandidateStaged &&
        ActionExecutorLocked &&
        !ActionExecutorArmed &&
        !GrantsAuthority &&
        !AuthorizesAction &&
        !ArmsExecutor;
}

public sealed record ActualizationLockReceipt(
    string LockReceiptHandle,
    string SourceLabGelReceiptHandle,
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
    bool AdmitsGel,
    bool MutatesSelfGel,
    bool ActivatesHeartbeat,
    bool ActivatesCmeActual,
    bool ActivatesSanctuaryActual)
{
    [JsonIgnore]
    public bool IsColdActualizationLock =>
        !string.IsNullOrWhiteSpace(LockReceiptHandle) &&
        !string.IsNullOrWhiteSpace(SourceLabGelReceiptHandle) &&
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
        !AdmitsGel &&
        !MutatesSelfGel &&
        !ActivatesHeartbeat &&
        !ActivatesCmeActual &&
        !ActivatesSanctuaryActual;
}

public sealed record SanctuaryAgentEngineIdleReadinessReceipt(
    string ReceiptHandle,
    SanctuaryAgentEngineIdleReadinessDisposition Disposition,
    string OutcomeCode,
    string GovernanceTrace,
    string LineRootPath,
    string InstallRootPath,
    string ReceiptJsonPath,
    string ReceiptMarkdownPath,
    string SessionLedgerPath,
    string SourceLabGelReceiptHandle,
    string SourceEngramCandidateHandle,
    string SourceEngramClosureReceiptHandle,
    string PriorAgentEngineIdleReceiptHandle,
    string OperatorId,
    string Domain,
    string Role,
    string JobClass,
    string SessionId,
    int TurnIndex,
    string ThoughtForm,
    SliLispAgentEngineIdleReadinessReceipt? SliLispAgentEngineIdleReceipt,
    EngineLlmSeatCandidateReceipt? EngineSeatCandidate,
    DriverAuthorityGateReceipt? DriverAuthorityGate,
    ActualizationLockReceipt? ActualizationLock,
    bool ReviewOnly,
    bool SliLispOwnedEngineMotion,
    bool ProviderNeutralityHeld,
    bool CrossModelHarnessApproachable,
    bool EngineLlmSeatCandidateStaged,
    bool CodexAgentLabProfileStaged,
    bool CodexEngineSeatCandidateStaged,
    bool SubagentEngineSeatCandidateStaged,
    bool OperatorAuthorityRequired,
    bool AuthorityGrantAbsent,
    bool ActionExecutorLocked,
    bool IdleLoopHeld,
    bool EngineLlmArticulationAllowed,
    bool EngineLlmRehearsalAllowed,
    bool EngineLlmCandidateFormationAllowed,
    bool EngineLlmAuthorityGrantingAllowed,
    bool EngineLlmActionExecutionAllowed,
    bool GelAdmissionLocked,
    bool SelfGelMutationLocked,
    bool HeartbeatLocked,
    bool CmeActualLocked,
    bool SanctuaryActualLocked,
    bool AuthorityGranted,
    bool ActionAuthorized,
    bool ActionExecutorArmed,
    bool LabGelAdmitted,
    bool SelfGelMutated,
    bool HeartbeatActive,
    bool ContinuityAdmitted,
    bool CmeActualAllowed,
    bool SanctuaryActualAllowed,
    bool ActivationRefused,
    bool ModelBindingAllowed,
    bool ArbitraryLispEvaluationAllowed,
    bool RuntimeIdentityAllowed,
    bool RuntimeActionAllowed,
    bool DatabaseWriteAllowed,
    DateTimeOffset TimestampUtc)
{
    [JsonIgnore]
    public bool IsColdAgentEngineIdleReadiness =>
        Disposition == SanctuaryAgentEngineIdleReadinessDisposition.CompletedCold &&
        SliLispAgentEngineIdleReceipt?.IsAgentEngineIdleReadiness == true &&
        EngineSeatCandidate?.IsColdEngineLlmSeatCandidate == true &&
        !string.IsNullOrWhiteSpace(SourceEngramClosureReceiptHandle) &&
        DriverAuthorityGate?.IsColdDriverAuthorityGate == true &&
        ActualizationLock?.IsColdActualizationLock == true &&
        ReviewOnly &&
        SliLispOwnedEngineMotion &&
        ProviderNeutralityHeld &&
        CrossModelHarnessApproachable &&
        EngineLlmSeatCandidateStaged &&
        CodexAgentLabProfileStaged &&
        CodexEngineSeatCandidateStaged &&
        SubagentEngineSeatCandidateStaged &&
        OperatorAuthorityRequired &&
        AuthorityGrantAbsent &&
        ActionExecutorLocked &&
        IdleLoopHeld &&
        EngineLlmArticulationAllowed &&
        EngineLlmRehearsalAllowed &&
        EngineLlmCandidateFormationAllowed &&
        !EngineLlmAuthorityGrantingAllowed &&
        !EngineLlmActionExecutionAllowed &&
        GelAdmissionLocked &&
        SelfGelMutationLocked &&
        HeartbeatLocked &&
        CmeActualLocked &&
        SanctuaryActualLocked &&
        !AuthorityGranted &&
        !ActionAuthorized &&
        !ActionExecutorArmed &&
        !LabGelAdmitted &&
        !SelfGelMutated &&
        !HeartbeatActive &&
        !ContinuityAdmitted &&
        !CmeActualAllowed &&
        !SanctuaryActualAllowed &&
        ActivationRefused &&
        !ModelBindingAllowed &&
        !ArbitraryLispEvaluationAllowed &&
        !RuntimeIdentityAllowed &&
        !RuntimeActionAllowed &&
        !DatabaseWriteAllowed;
}
