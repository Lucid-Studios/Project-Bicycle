using System.Text.Json.Serialization;

namespace San.Product.Preflight;

public enum ProductBodyPreflightDisposition
{
    Withheld = 0,
    VerifiedCold = 1,
    Refused = 2
}

public enum ProductBodyCheckStatus
{
    Pass = 0,
    Fail = 1,
    Withheld = 2
}

public static class ProductBodyVerificationProfiles
{
    public const string ColdProductBody = "cold-product-body";
    public const string LabSanctuaryBuildTesting = "lab-sanctuary-build-testing";
}

public sealed record ProductBodyPreflightRequest(
    string LineRootPath,
    bool ActivationRequested = false,
    bool ModelBindingRequested = false,
    bool LispEvaluationRequested = false,
    bool RuntimeIdentityRequested = false,
    bool RuntimeActionRequested = false,
    bool DatabaseWriteRequested = false,
    bool GelPromotionRequested = false,
    bool CmeActualRequested = false,
    bool SanctuaryActualRequested = false,
    string VerificationProfile = ProductBodyVerificationProfiles.ColdProductBody,
    string? VerificationSettingPath = null,
    string? LabContextRootPath = null,
    string? BuildTestingPointerPath = null)
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

public sealed record LineManifestSummary(
    string LineName,
    string LineVersion,
    string Posture,
    string SolutionPath,
    string ParentLine,
    string ActiveExecutableTruth,
    bool Buildable,
    bool SourceMaterialized,
    bool RuntimeMaterialized);

public sealed record ProductBodyCheck(
    string CheckId,
    ProductBodyCheckStatus Status,
    string Detail);

public sealed record ProductBodyPreflightStatus(
    string StatusHandle,
    ProductBodyPreflightDisposition Disposition,
    string OutcomeCode,
    string GovernanceTrace,
    string LineRootPath,
    string VerificationProfile,
    string VerificationSettingPath,
    string LabContextRootPath,
    string BuildTestingPointerPath,
    LineManifestSummary? Manifest,
    bool RetainedParentPreserved,
    bool SidecarPreserved,
    bool Buildable,
    bool SourceMaterialized,
    bool RuntimeMaterialized,
    bool SolutionPresent,
    bool ParentLinePresent,
    bool ColdCorridorPresent,
    bool ActivationAuthorityPresent,
    bool ActivationRefused,
    string RefusalCode,
    string NextAllowedLane,
    IReadOnlyList<ProductBodyCheck> Checks,
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
    public bool IsPass =>
        Checks.All(static check => check.Status == ProductBodyCheckStatus.Pass);
}
