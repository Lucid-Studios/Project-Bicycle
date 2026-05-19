namespace San.Product.Preflight;

public enum ProductBodyInstallDisposition
{
    Withheld = 0,
    InstalledCold = 1,
    Refused = 2
}

public sealed record ProductBodyInstallRequest(
    string LineRootPath,
    string InstallRootPath,
    string ProductSourceRootPath,
    string VerificationProfile = ProductBodyVerificationProfiles.LabSanctuaryBuildTesting,
    string? VerificationSettingPath = null,
    string? LabContextRootPath = null,
    string? BuildTestingPointerPath = null,
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

public sealed record ProductBodyInstallReceipt(
    string ReceiptHandle,
    ProductBodyInstallDisposition Disposition,
    string OutcomeCode,
    string GovernanceTrace,
    string LineRootPath,
    string InstallRootPath,
    string ProductSourceRootPath,
    string ProductInstallRootPath,
    string BuildInstallRootPath,
    string ReceiptRootPath,
    string ProductExecutablePath,
    string CommandShimPath,
    string PowerShellShimPath,
    string PreflightReceiptJsonPath,
    string PreflightReceiptMarkdownPath,
    ProductBodyPreflightStatus PreflightStatus,
    int CopiedProductFileCount,
    bool ColdBuildToolSurfaceReady,
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
    public bool IsColdInstall =>
        Disposition == ProductBodyInstallDisposition.InstalledCold &&
        ColdBuildToolSurfaceReady &&
        ActivationRefused &&
        !ModelBindingAllowed &&
        !LispEvaluationAllowed &&
        !RuntimeIdentityAllowed &&
        !RuntimeActionAllowed &&
        !DatabaseWriteAllowed &&
        !GelPromotionAllowed &&
        !CmeActualAllowed &&
        !SanctuaryActualAllowed &&
        PreflightStatus.Disposition == ProductBodyPreflightDisposition.VerifiedCold;
}
