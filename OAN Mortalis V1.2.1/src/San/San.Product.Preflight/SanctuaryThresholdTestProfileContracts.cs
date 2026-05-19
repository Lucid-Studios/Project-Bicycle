using System.Text.Json.Serialization;

namespace San.Product.Preflight;

public enum SanctuaryThresholdTestProfileDisposition
{
    Withheld = 0,
    ReadyCold = 1,
    Refused = 2
}

public enum SanctuaryThresholdCognitionProviderKind
{
    CodexProxy = 0,
    LocalHostedLlm = 1
}

public enum SanctuaryThresholdRoleSeatKind
{
    Prime = 0,
    Cryptic = 1,
    Steward = 2
}

public enum SanctuaryThresholdRoleSeatStatus
{
    ProxyOnly = 0,
    Withheld = 1,
    Refused = 2
}

public sealed record SanctuaryThresholdTestProfileRequest(
    string LineRootPath,
    string InstallRootPath,
    bool ActivationRequested = false,
    bool ModelBindingRequested = false,
    bool LispEvaluationRequested = false,
    bool RuntimeIdentityRequested = false,
    bool RuntimeActionRequested = false,
    bool DatabaseWriteRequested = false,
    bool GelPromotionRequested = false,
    bool CmeActualRequested = false,
    bool SanctuaryActualRequested = false)
{
    public bool RequestsRuntimeMotion =>
        ActivationRequested ||
        ModelBindingRequested ||
        LispEvaluationRequested ||
        RuntimeIdentityRequested ||
        RuntimeActionRequested ||
        DatabaseWriteRequested ||
        GelPromotionRequested ||
        CmeActualRequested ||
        SanctuaryActualRequested;
}

public sealed record SanctuaryThresholdCognitionProvider(
    SanctuaryThresholdCognitionProviderKind ProviderKind,
    string ProviderId,
    string ProviderSummary,
    bool BaseForBuildTesting,
    bool LocalHostedLlmDeferred,
    bool PersistentMemoryClaimed,
    bool RuntimeIdentityClaimed);

public sealed record SanctuaryThresholdRoleAgentSeat(
    SanctuaryThresholdRoleSeatKind SeatKind,
    SanctuaryThresholdRoleSeatStatus Status,
    string AgentLabel,
    string RoleDomain,
    string InvocationMode,
    string AuthorityBoundary,
    bool GrantsAuthority,
    bool SelfAuthorizes,
    bool ActivatesCmeActual,
    bool RequiresLocalHostedLlm);

public sealed record SanctuaryThresholdTestProfile(
    string ProfileHandle,
    SanctuaryThresholdTestProfileDisposition Disposition,
    string OutcomeCode,
    string GovernanceTrace,
    string LineRootPath,
    string InstallRootPath,
    string ActualNamingLaw,
    string ReservedActionableStateName,
    string CurrentInstallStateName,
    SanctuaryThresholdCognitionProvider BaseProvider,
    IReadOnlyList<SanctuaryThresholdRoleAgentSeat> RoleSeats,
    string CodexAgentSpawnPolicy,
    string LocalHostedLlmPosture,
    bool CodexProxyMayBuild,
    bool CodexProxyMayAuthorize,
    bool DedicatedAgentsRequiredOnlyWhenNeeded,
    bool LocalHostedLlmDeferredUntilFirstCmeTest,
    bool ReservedActionableStateAuthorized,
    bool ActivationRefused,
    bool ModelBindingAllowed,
    bool LispEvaluationAllowed,
    bool RuntimeIdentityAllowed,
    bool RuntimeActionAllowed,
    bool DatabaseWriteAllowed,
    bool GelPromotionAllowed,
    bool CmeActualAllowed,
    bool SanctuaryActualAllowed,
    DateTimeOffset TimestampUtc)
{
    [JsonIgnore]
    public bool IsColdProxyProfile =>
        Disposition == SanctuaryThresholdTestProfileDisposition.ReadyCold &&
        BaseProvider is
        {
            ProviderKind: SanctuaryThresholdCognitionProviderKind.CodexProxy,
            BaseForBuildTesting: true,
            LocalHostedLlmDeferred: true,
            PersistentMemoryClaimed: false,
            RuntimeIdentityClaimed: false
        } &&
        RoleSeats.Count == 3 &&
        RoleSeats.All(static seat =>
            seat.Status == SanctuaryThresholdRoleSeatStatus.ProxyOnly &&
            !seat.GrantsAuthority &&
            !seat.SelfAuthorizes &&
            !seat.ActivatesCmeActual &&
            !seat.RequiresLocalHostedLlm) &&
        CodexProxyMayBuild &&
        !CodexProxyMayAuthorize &&
        DedicatedAgentsRequiredOnlyWhenNeeded &&
        LocalHostedLlmDeferredUntilFirstCmeTest &&
        ActualNamingLaw.Contains(".Actual", StringComparison.Ordinal) &&
        string.Equals(ReservedActionableStateName, "Sanctuary.Actual", StringComparison.Ordinal) &&
        string.Equals(CurrentInstallStateName, "Sanctuary.ColdInstalled", StringComparison.Ordinal) &&
        !ReservedActionableStateAuthorized &&
        ActivationRefused &&
        !ModelBindingAllowed &&
        !LispEvaluationAllowed &&
        !RuntimeIdentityAllowed &&
        !RuntimeActionAllowed &&
        !DatabaseWriteAllowed &&
        !GelPromotionAllowed &&
        !CmeActualAllowed &&
        !SanctuaryActualAllowed;
}
