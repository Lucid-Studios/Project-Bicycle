using System.Text.Json.Serialization;

namespace SLI.Lisp;

public enum SliLispToolBodyIdleStateDisposition
{
    CompletedCold = 0,
    Withheld = 1,
    Refused = 2
}

public sealed record SliLispToolBodyIdleStateRequest(
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
    string EngramCandidateHandle,
    string EngramClosureReceiptHandle,
    string LabGelReadbackReceiptHandle,
    string ThoughtForm,
    string? RuntimePath = null,
    bool ArbitraryEvaluationRequested = false,
    bool RuntimeActionRequested = false,
    bool ActivationRequested = false,
    bool ModelBindingRequested = false,
    bool ProviderCallRequested = false,
    bool LlmMaintenanceRequested = false,
    bool TickLoopRequested = false,
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
        LlmMaintenanceRequested ||
        TickLoopRequested ||
        AuthorityGrantRequested ||
        ActionExecutorArmRequested ||
        GelAdmissionRequested ||
        SelfGelMutationRequested ||
        HeartbeatActivationRequested ||
        CmeActualRequested ||
        SanctuaryActualRequested ||
        ContinuityAdmissionRequested;
}

public sealed record SliLispToolBodyIdleStateReceipt(
    string ReceiptHandle,
    SliLispToolBodyIdleStateDisposition Disposition,
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
    string EngramCandidateHandle,
    string EngramClosureReceiptHandle,
    string LabGelReadbackReceiptHandle,
    string ThoughtForm,
    IReadOnlyDictionary<string, string> Telemetry,
    IReadOnlyList<string> ModuleNames,
    int ModuleCount,
    bool BoundedEntrypointCalled,
    bool LoadAttempted,
    bool LoadSucceeded,
    bool ToolBodyIdleStateCompleted,
    string IdleState,
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
    bool EcLoopReady,
    bool TypedWarmUseReady,
    bool LabGelReady,
    bool AgentEngineIdleRequired,
    bool SourceLineageHeld,
    bool SourceEngramClosureAcceptedCold,
    bool SourceLabGelReadbackAcceptedCold,
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
    public bool IsToolBodyIdleState =>
        Disposition == SliLispToolBodyIdleStateDisposition.CompletedCold &&
        BoundedEntrypointCalled &&
        LoadAttempted &&
        LoadSucceeded &&
        ToolBodyIdleStateCompleted &&
        string.Equals(IdleState, "cold-sanctuary-maintained-idle", StringComparison.OrdinalIgnoreCase) &&
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
        EcLoopReady &&
        TypedWarmUseReady &&
        LabGelReady &&
        !AgentEngineIdleRequired &&
        SourceLineageHeld &&
        SourceEngramClosureAcceptedCold &&
        SourceLabGelReadbackAcceptedCold &&
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
