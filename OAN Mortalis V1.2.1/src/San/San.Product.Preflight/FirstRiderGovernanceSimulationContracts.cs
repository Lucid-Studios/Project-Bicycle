using System.Text.Json.Serialization;

namespace San.Product.Preflight;

public enum FirstRiderGovernanceSimulationDisposition
{
    Withheld = 0,
    SimulatedCold = 1,
    Refused = 2
}

public sealed record FirstRiderGovernanceSimulationRequest(
    string LineRootPath,
    string InstallRootPath,
    string? ThoughtForm = null,
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

public sealed record FirstRiderGovernanceStageSpec(
    string StageId,
    string StageName,
    string BoundaryCellId,
    IReadOnlyList<string> RequiredArtifacts,
    string GovernanceFunction,
    string ExpectedColdResult);

public sealed record FirstRiderGovernanceStageReceipt(
    string StageId,
    string StageName,
    string BoundaryCellId,
    IReadOnlyList<string> RequiredArtifacts,
    IReadOnlyList<string> MissingArtifacts,
    string GovernanceFunction,
    string Result,
    bool ArtifactSurfaceVerified,
    bool ReviewOnly,
    bool AuthorityGranted,
    bool ActionAuthorized,
    bool ContinuityMutated,
    bool RuntimeMotionRequested);

public sealed record FirstRiderGovernanceSimulationReceipt(
    string ReceiptHandle,
    FirstRiderGovernanceSimulationDisposition Disposition,
    string OutcomeCode,
    string GovernanceTrace,
    string LineRootPath,
    string InstallRootPath,
    string ThoughtForm,
    string RiderName,
    IReadOnlyList<FirstRiderGovernanceStageReceipt> Stages,
    IReadOnlyList<string> MissingArtifacts,
    bool RouteComplete,
    bool ReviewOnly,
    bool SimulatedOnly,
    bool ArtifactBodyVerified,
    bool ActionRefused,
    bool AuthorityGranted,
    bool ContinuityAdmitted,
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
    public bool IsColdRiderReceipt =>
        Disposition == FirstRiderGovernanceSimulationDisposition.SimulatedCold &&
        Stages.Count == 12 &&
        RouteComplete &&
        ReviewOnly &&
        SimulatedOnly &&
        ArtifactBodyVerified &&
        ActionRefused &&
        !AuthorityGranted &&
        !ContinuityAdmitted &&
        ActivationRefused &&
        !ModelBindingAllowed &&
        !LispEvaluationAllowed &&
        !RuntimeIdentityAllowed &&
        !RuntimeActionAllowed &&
        !DatabaseWriteAllowed &&
        !GelPromotionAllowed &&
        !CmeActualAllowed &&
        !SanctuaryActualAllowed &&
        Stages.All(static stage =>
            stage.ArtifactSurfaceVerified &&
            stage.ReviewOnly &&
            !stage.AuthorityGranted &&
            !stage.ActionAuthorized &&
            !stage.ContinuityMutated &&
            !stage.RuntimeMotionRequested);
}
