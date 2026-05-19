using System.Text.Json.Serialization;

namespace San.Product.Preflight;

public enum SpiralBuildStepDisposition
{
    Withheld = 0,
    ExecutedCold = 1,
    Refused = 2,
    Complete = 3
}

public sealed record SpiralBuildStepRequest(
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

public sealed record SpiralBuildArtifactRecord(
    string ArtifactId,
    string JsonPath,
    string MarkdownPath,
    string Summary);

public sealed record SpiralBuildStepReceipt(
    string ReceiptHandle,
    SpiralBuildStepDisposition Disposition,
    string OutcomeCode,
    string GovernanceTrace,
    string LineRootPath,
    string InstallRootPath,
    string? ExecutedCellId,
    IReadOnlyList<string> ExecutedCellIds,
    string? NextCellBeforeExecution,
    string? NextCellAfterExecution,
    IReadOnlyList<SpiralBuildArtifactRecord> Artifacts,
    bool AutomationMayContinue,
    bool HitlRequired,
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
    public bool IsColdStep =>
        Disposition is SpiralBuildStepDisposition.ExecutedCold or SpiralBuildStepDisposition.Complete &&
        !string.IsNullOrWhiteSpace(ExecutedCellId) &&
        ExecutedCellIds.Count > 0 &&
        Artifacts.Count > 0 &&
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
