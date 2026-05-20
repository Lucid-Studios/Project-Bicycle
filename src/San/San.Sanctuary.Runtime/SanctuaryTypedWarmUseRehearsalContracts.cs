using System.Text.Json.Serialization;
using SLI.Lisp;

namespace San.Sanctuary.Runtime;

public enum SanctuaryTypedWarmUseRehearsalDisposition
{
    Withheld = 0,
    CompletedCold = 1,
    Refused = 2
}

public sealed record SanctuaryTypedWarmUseRehearsalRequest(
    SanctuaryInstalledSubstrateReceipt? InstalledSubstrateReceipt,
    string SessionId,
    int TurnIndex,
    string ThoughtForm,
    string? PriorTurnReceiptHandle = null,
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

public sealed record SanctuaryTypedWarmUseRehearsalReceipt(
    string ReceiptHandle,
    SanctuaryTypedWarmUseRehearsalDisposition Disposition,
    string OutcomeCode,
    string GovernanceTrace,
    string LineRootPath,
    string InstallRootPath,
    string ReceiptJsonPath,
    string ReceiptMarkdownPath,
    string SessionLedgerPath,
    string SessionSummaryPath,
    string SourceInstalledSubstrateReceiptHandle,
    string PriorTurnReceiptHandle,
    string OperatorId,
    string Domain,
    string Role,
    string JobClass,
    string SessionId,
    int TurnIndex,
    string ThoughtForm,
    SliLispTypedWarmUseRehearsalReceipt? SliLispWarmUseReceipt,
    bool ReviewOnly,
    bool SliLispOwnedEngineMotion,
    bool TypedScopeAccepted,
    bool LiveIngressAcceptedCold,
    bool SessionLineageWitnessed,
    bool ListeningFrameReceived,
    bool CompassOrientedPressure,
    bool ThinkingAboutThinkingTelemetryProduced,
    bool PreEngramResidueProduced,
    int PreEngramResidueCount,
    bool StewardReviewed,
    bool TurnLineageReceiptOnly,
    bool SessionLedgerAppendOnly,
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
    public bool IsTypedColdReadyWarmUse =>
        Disposition == SanctuaryTypedWarmUseRehearsalDisposition.CompletedCold &&
        SliLispWarmUseReceipt?.IsTypedWarmUseRehearsal == true &&
        ReviewOnly &&
        SliLispOwnedEngineMotion &&
        TypedScopeAccepted &&
        LiveIngressAcceptedCold &&
        SessionLineageWitnessed &&
        ListeningFrameReceived &&
        CompassOrientedPressure &&
        ThinkingAboutThinkingTelemetryProduced &&
        PreEngramResidueProduced &&
        PreEngramResidueCount == 6 &&
        StewardReviewed &&
        TurnLineageReceiptOnly &&
        SessionLedgerAppendOnly &&
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
