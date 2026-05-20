using System.Text.Json.Serialization;
using SLI.Lisp;

namespace San.Sanctuary.Runtime;

public enum SanctuaryToolBodyIdleStateDisposition
{
    Withheld = 0,
    CompletedCold = 1,
    Refused = 2
}

public sealed record SanctuaryToolBodyIdleStateRequest(
    SanctuaryInstalledSubstrateReceipt? InstalledSubstrateReceipt,
    SanctuaryEcTelemetryLoopReceipt? EcLoopReceipt,
    SanctuaryTypedWarmUseRehearsalReceipt? WarmUseReceipt,
    SanctuaryLabGelEngrammitizationReceipt? LabGelReceipt,
    string? PriorToolBodyIdleReceiptHandle = null,
    string? SliLispRuntimePath = null,
    bool ActivationRequested = false,
    bool ModelBindingRequested = false,
    bool ProviderCallRequested = false,
    bool LlmMaintenanceRequested = false,
    bool TickLoopRequested = false,
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
        LlmMaintenanceRequested ||
        TickLoopRequested ||
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

public sealed record SanctuaryToolBodyIdleStateReceipt(
    string ReceiptHandle,
    SanctuaryToolBodyIdleStateDisposition Disposition,
    string OutcomeCode,
    string GovernanceTrace,
    string LineRootPath,
    string InstallRootPath,
    string ReceiptJsonPath,
    string ReceiptMarkdownPath,
    string SessionLedgerPath,
    string SourceInstalledSubstrateReceiptHandle,
    string SourceEcLoopReceiptHandle,
    string SourceWarmUseReceiptHandle,
    string SourceLabGelReceiptHandle,
    string SourceEngramCandidateHandle,
    string SourceEngramClosureReceiptHandle,
    string SourceLabGelReadbackReceiptHandle,
    string PriorToolBodyIdleReceiptHandle,
    string OperatorId,
    string Domain,
    string Role,
    string JobClass,
    string SessionId,
    int TurnIndex,
    string ThoughtForm,
    SliLispToolBodyIdleStateReceipt? SliLispToolBodyIdleReceipt,
    bool ReviewOnly,
    bool SliLispOwnedIdleMotion,
    bool InstalledSubstrateReady,
    bool EcLoopReady,
    bool WarmUseReady,
    bool LabGelReady,
    bool SourceLineageHeld,
    bool SourceEngramClosureHeld,
    bool SourceLabGelReadbackHeld,
    int RequiredOrganCount,
    bool AllRequiredOrgansPresent,
    bool BaseBodiesPresent,
    bool CondensateBodiesPresent,
    bool RoleBodiesPresent,
    bool GoverningCmeCSharpBodiesBuilt,
    bool GoverningCmeActualizedCold,
    bool PrimeGoverningCmeBuilt,
    bool CrypticGoverningCmeBuilt,
    bool StewardGoverningCmeBuilt,
    bool GoverningCmeSliLispActualizationSurfacesReady,
    bool GoverningCmeMaintainsIdleState,
    bool GoverningHeartbeatHealthy,
    bool BondedCmeCallAvailable,
    bool SanctuaryGovernanceMonitoringReady,
    bool SliLispLoaded,
    bool SliLispPrimePresent,
    bool SliLispCrypticPresent,
    bool LispControlMatrixPresent,
    bool ListeningFramePresent,
    bool CompassPresent,
    bool SoulFrameRoutePresent,
    bool AgentiCoreRoutePresent,
    string IdleState,
    bool ToolBodyIdleStateHeld,
    bool MaintainedBySanctuary,
    bool MaintainedByLlm,
    bool LlmMaintenanceRequired,
    bool LlmAdapterRequired,
    bool ReadyForLlmAdapter,
    bool CanAcceptFutureRider,
    bool GovernanceSlmCandidateDesirable,
    bool GovernanceSlmRoutingSwitchCandidate,
    bool GovernanceSlmIntelligentSwitchCandidate,
    bool GovernanceSlmPresent,
    bool GovernanceSlmRequiredForIdle,
    bool GovernanceSlmMayDiscriminateEscalation,
    bool GovernanceSlmMayDiscernActionReadiness,
    bool GovernanceSlmDiscernmentAuthorizesAction,
    bool GovernanceSlmMayAuthorizeAction,
    bool ModelAdapterPresent,
    bool ModelBindingAllowed,
    bool ProviderCallAllowed,
    bool HiddenInternalsClaimed,
    bool TickLoopRunning,
    bool TickMaintainedByLlm,
    bool IdleLoopHeld,
    bool ReturnToPrimeHeld,
    bool OperatorReentryAvailable,
    bool EcMaintainedInLisp,
    bool LocalEcHoldAvailable,
    bool EngineCallRequired,
    bool LlmEngineCallRequired,
    bool ExternalEngineCallRequired,
    bool AgentEngineIdleRequired,
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
    public bool IsColdToolBodyIdleState =>
        Disposition == SanctuaryToolBodyIdleStateDisposition.CompletedCold &&
        SliLispToolBodyIdleReceipt?.IsToolBodyIdleState == true &&
        ReviewOnly &&
        SliLispOwnedIdleMotion &&
        InstalledSubstrateReady &&
        EcLoopReady &&
        WarmUseReady &&
        LabGelReady &&
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
        GoverningCmeCSharpBodiesBuilt &&
        GoverningCmeActualizedCold &&
        PrimeGoverningCmeBuilt &&
        CrypticGoverningCmeBuilt &&
        StewardGoverningCmeBuilt &&
        GoverningCmeSliLispActualizationSurfacesReady &&
        GoverningCmeMaintainsIdleState &&
        GoverningHeartbeatHealthy &&
        BondedCmeCallAvailable &&
        SanctuaryGovernanceMonitoringReady &&
        SliLispLoaded &&
        SliLispPrimePresent &&
        SliLispCrypticPresent &&
        LispControlMatrixPresent &&
        ListeningFramePresent &&
        CompassPresent &&
        SoulFrameRoutePresent &&
        AgentiCoreRoutePresent &&
        string.Equals(IdleState, "cold-sanctuary-maintained-idle", StringComparison.OrdinalIgnoreCase) &&
        ToolBodyIdleStateHeld &&
        MaintainedBySanctuary &&
        !MaintainedByLlm &&
        !LlmMaintenanceRequired &&
        !LlmAdapterRequired &&
        ReadyForLlmAdapter &&
        CanAcceptFutureRider &&
        GovernanceSlmCandidateDesirable &&
        GovernanceSlmRoutingSwitchCandidate &&
        GovernanceSlmIntelligentSwitchCandidate &&
        !GovernanceSlmPresent &&
        !GovernanceSlmRequiredForIdle &&
        GovernanceSlmMayDiscriminateEscalation &&
        GovernanceSlmMayDiscernActionReadiness &&
        !GovernanceSlmDiscernmentAuthorizesAction &&
        !GovernanceSlmMayAuthorizeAction &&
        !ModelAdapterPresent &&
        !ModelBindingAllowed &&
        !ProviderCallAllowed &&
        !HiddenInternalsClaimed &&
        !TickLoopRunning &&
        !TickMaintainedByLlm &&
        IdleLoopHeld &&
        ReturnToPrimeHeld &&
        OperatorReentryAvailable &&
        EcMaintainedInLisp &&
        LocalEcHoldAvailable &&
        !EngineCallRequired &&
        !LlmEngineCallRequired &&
        !ExternalEngineCallRequired &&
        !AgentEngineIdleRequired &&
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
