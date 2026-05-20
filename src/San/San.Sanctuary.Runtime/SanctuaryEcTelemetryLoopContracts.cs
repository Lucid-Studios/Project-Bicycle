using System.Text.Json.Serialization;
using SLI.Lisp;

namespace San.Sanctuary.Runtime;

public enum SanctuaryEcTelemetryLoopDisposition
{
    Withheld = 0,
    CompletedCold = 1,
    Refused = 2
}

public sealed record SanctuaryEcTelemetryLoopRequest(
    SanctuaryInstalledSubstrateReceipt? InstalledSubstrateReceipt,
    string ThoughtForm,
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

public sealed record SanctuaryEcTelemetryLoopReceipt(
    string ReceiptHandle,
    SanctuaryEcTelemetryLoopDisposition Disposition,
    string OutcomeCode,
    string GovernanceTrace,
    string LineRootPath,
    string InstallRootPath,
    string ReceiptJsonPath,
    string ReceiptMarkdownPath,
    string ThoughtForm,
    string SourceInstalledSubstrateReceiptHandle,
    SliLispEcTelemetryLoopReceipt? SliLispEngineReceipt,
    bool ReviewOnly,
    bool SliLispOwnedEngineMotion,
    bool ColdEngineLoopCompleted,
    bool ListeningFrameReceived,
    bool CompassOrientedPressure,
    bool ThinkingAboutThinkingTelemetryProduced,
    bool PreEngramResidueProduced,
    int PreEngramResidueCount,
    bool StewardReviewed,
    bool StreamAdmittedEngram,
    bool StreamAdmittedMemory,
    bool SelfGelMutated,
    bool ContinuityAdmitted,
    bool AuthorityGranted,
    bool ActivationRefused,
    bool ModelBindingAllowed,
    bool ArbitraryLispEvaluationAllowed,
    bool RuntimeIdentityAllowed,
    bool RuntimeActionAllowed,
    bool DatabaseWriteAllowed,
    bool GelPromotionAllowed,
    bool CmeActualAllowed,
    bool SanctuaryActualAllowed,
    DateTimeOffset TimestampUtc)
{
    [JsonIgnore]
    public bool IsColdEcTelemetryLoop =>
        Disposition == SanctuaryEcTelemetryLoopDisposition.CompletedCold &&
        SliLispEngineReceipt?.IsColdEcTelemetryLoop == true &&
        ReviewOnly &&
        SliLispOwnedEngineMotion &&
        ColdEngineLoopCompleted &&
        ListeningFrameReceived &&
        CompassOrientedPressure &&
        ThinkingAboutThinkingTelemetryProduced &&
        PreEngramResidueProduced &&
        PreEngramResidueCount == 6 &&
        StewardReviewed &&
        !StreamAdmittedEngram &&
        !StreamAdmittedMemory &&
        !SelfGelMutated &&
        !ContinuityAdmitted &&
        !AuthorityGranted &&
        ActivationRefused &&
        !ModelBindingAllowed &&
        !ArbitraryLispEvaluationAllowed &&
        !RuntimeIdentityAllowed &&
        !RuntimeActionAllowed &&
        !DatabaseWriteAllowed &&
        !GelPromotionAllowed &&
        !CmeActualAllowed &&
        !SanctuaryActualAllowed;
}
