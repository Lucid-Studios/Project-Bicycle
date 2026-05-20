using System.Text.Json.Serialization;
using SLI.Lisp;

namespace San.Sanctuary.Runtime;

public enum SanctuaryInstalledSubstrateDisposition
{
    Withheld = 0,
    InstalledCold = 1,
    Refused = 2
}

public enum SanctuaryInstalledBodyKind
{
    Gel = 0,
    Goa = 1,
    Mos = 2,
    Vault = 3,
    CGel = 4,
    CGoa = 5,
    CMos = 6,
    CVault = 7,
    Prime = 8,
    Cryptic = 9,
    Steward = 10
}

public enum SanctuaryInstalledBodyState
{
    InstalledCold = 0,
    CondensedCold = 1,
    RoleInstalledCold = 2,
    Withheld = 3,
    Refused = 4
}

public sealed record SanctuaryInstalledSubstrateRequest(
    string LineRootPath,
    string InstallRootPath,
    string OperatorName = "Sanctuary",
    string Domain = "Sanctuary",
    string Role = "InstalledBody",
    string JobClass = "ColdBench",
    string? SliLispRuntimePath = null,
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

public sealed record SanctuaryRootIdentityRecord(
    string SanctuaryId,
    string OperatorName,
    string OperatorId,
    string Domain,
    string Role,
    string JobClass,
    string ActualNameCandidate,
    string CmeActualIdCandidate,
    string OpalEngramRootId,
    string SelfGelRootId,
    bool CmeActualCandidateOnly,
    bool HeartbeatActive,
    bool GrantsAuthority,
    bool AdmitsContinuity);

public sealed record SanctuaryInstalledBodyRecord(
    SanctuaryInstalledBodyKind BodyKind,
    SanctuaryInstalledBodyState State,
    string BodyName,
    string BodyHandle,
    IReadOnlyList<string> SourceBodyNames,
    IReadOnlyList<string> SourceReceiptRefs,
    string Function,
    string StoragePath,
    bool IsBaseBody,
    bool IsCondensateBody,
    bool IsRoleBody,
    bool Installed,
    bool GrantsAuthority,
    bool ActivatesHeartbeat,
    bool AdmitsContinuity,
    bool AllowsAction,
    bool AllowsModelBinding,
    bool AllowsLispEvaluation,
    bool AllowsDatabaseWrite,
    bool AllowsGelPromotion,
    bool AllowsCmeActual,
    bool AllowsSanctuaryActual)
{
    [JsonIgnore]
    public bool IsColdBody =>
        Installed &&
        !GrantsAuthority &&
        !ActivatesHeartbeat &&
        !AdmitsContinuity &&
        !AllowsAction &&
        !AllowsModelBinding &&
        !AllowsLispEvaluation &&
        !AllowsDatabaseWrite &&
        !AllowsGelPromotion &&
        !AllowsCmeActual &&
        !AllowsSanctuaryActual;
}

public sealed record SanctuaryInstalledSubstrateReceipt(
    string ReceiptHandle,
    SanctuaryInstalledSubstrateDisposition Disposition,
    string OutcomeCode,
    string GovernanceTrace,
    string LineRootPath,
    string InstallRootPath,
    string BodyRootPath,
    string ReceiptJsonPath,
    string ReceiptMarkdownPath,
    SanctuaryRootIdentityRecord RootIdentity,
    IReadOnlyList<SanctuaryInstalledBodyRecord> Bodies,
    SliLispRuntimeLoadReceipt? SliLispLoadReceipt,
    bool BaseBodiesInstalled,
    bool CondensateBodiesInstalled,
    bool RoleBodiesInstalled,
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
    public const int ExpectedInstalledBodyCount = 11;

    [JsonIgnore]
    public bool IsColdInstalledSubstrate =>
        Disposition == SanctuaryInstalledSubstrateDisposition.InstalledCold &&
        Bodies.Count == ExpectedInstalledBodyCount &&
        BaseBodiesInstalled &&
        CondensateBodiesInstalled &&
        RoleBodiesInstalled &&
        RootIdentity is
        {
            CmeActualCandidateOnly: true,
            HeartbeatActive: false,
            GrantsAuthority: false,
            AdmitsContinuity: false
        } &&
        SliLispLoadReceipt is
        {
            Disposition: SliLispRuntimeLoadDisposition.LoadedCold,
            LoadSucceeded: true,
            ActivationAllowed: false,
            AuthorityGranted: false,
            ModelBindingAllowed: false,
            GelPromotionAllowed: false,
            CmeActualAllowed: false,
            SanctuaryActualAllowed: false
        } &&
        Bodies.All(static body => body.IsColdBody) &&
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
