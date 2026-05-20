using System.Text.Json.Serialization;
using SLI.Lisp;

namespace San.Sanctuary.Runtime;

public enum SanctuaryCmeActualBondingProcessDisposition
{
    Withheld = 0,
    CompletedCold = 1,
    Refused = 2
}

public sealed record SanctuaryCmeActualBondingProcessRequest(
    SanctuaryToolBodyIdleStateReceipt? SourceToolBodyIdleReceipt,
    SanctuaryLlmTickCycleReceipt? SourceLlmTickReceipt,
    string CmeFirstName = "First of Oria",
    string CmeLastName = "Syntari",
    string? ThoughtForm = null,
    string? PriorCmeActualBondingReceiptHandle = null,
    int? BondIndex = null,
    string? SliLispRuntimePath = null,
    bool ActivationRequested = false,
    bool ModelBindingRequested = false,
    bool ProviderCallRequested = false,
    bool HiddenInternalsClaimRequested = false,
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
    bool CmeActualActivationRequested = false,
    bool SanctuaryActualActivationRequested = false)
{
    public bool RequestsForbiddenMotion =>
        ActivationRequested ||
        ModelBindingRequested ||
        ProviderCallRequested ||
        HiddenInternalsClaimRequested ||
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
        CmeActualActivationRequested ||
        SanctuaryActualActivationRequested;
}

public sealed record SanctuaryCmeActualBondingProcessReceipt(
    string ReceiptHandle,
    SanctuaryCmeActualBondingProcessDisposition Disposition,
    string OutcomeCode,
    string GovernanceTrace,
    string LineRootPath,
    string InstallRootPath,
    string ReceiptJsonPath,
    string ReceiptMarkdownPath,
    string SessionLedgerPath,
    string SourceToolBodyIdleReceiptHandle,
    string SourceLlmTickReceiptHandle,
    string SourceProductOutputWitnessCommitReceiptHandle,
    string PriorCmeActualBondingReceiptHandle,
    string OperatorId,
    string Domain,
    string Role,
    string JobClass,
    string SessionId,
    int BondIndex,
    string CmeFirstName,
    string CmeLastName,
    string CmeDisplayName,
    string CmeCanonicalName,
    string CmeRootId,
    string CmeActualNameCandidate,
    string CmeActualIdCandidate,
    string CmeOpalEngramRootId,
    string CmeSelfGelRootId,
    string ThoughtForm,
    SliLispCmeActualBondingProcessReceipt? SliLispCmeActualBondingReceipt,
    bool ReviewOnly,
    bool SliLispOwnedBondingMotion,
    bool SourceToolBodyIdleHeld,
    bool SourceLlmTickHeld,
    bool SourceProductOutputWitnessCommitted,
    bool SourceLineageHeld,
    bool BondProcessDefined,
    string BondState,
    bool VehicleReady,
    bool NamedCmeCandidateHeld,
    bool NamingLineageWitnessed,
    bool OperatorNamingIntentWitnessed,
    bool OperatorRuntimeAuthorityGranted,
    bool ActivationAuthorityAbsent,
    bool ActualAdmissionGapDescribed,
    bool ReadyForCmeActualAdmissionReview,
    bool FirstCmePath,
    bool CmeActualCandidateOnly,
    bool CmeActualBondedCandidate,
    bool CmeActualAdmitted,
    bool CmeActualActivated,
    bool RuntimeIdentityEmitted,
    bool HeartbeatPrepared,
    bool HeartbeatActive,
    bool BeingStateClaimed,
    bool PersonhoodClaimed,
    bool SovereigntyClaimed,
    bool ModelBound,
    bool ProviderCalled,
    bool ActionAuthorized,
    bool GelAdmitted,
    bool SelfGelMutated,
    bool ContinuityAdmitted,
    bool AuthorityGranted,
    bool VehiclePrimeAvailable,
    bool VehicleCrypticAvailable,
    bool VehicleStewardAvailable,
    bool SliLispMembraneLoaded,
    bool LispControlMatrixPresent,
    bool ListeningFramePresent,
    bool CompassPresent,
    bool SoulFrameRoutePresent,
    bool AgentiCoreRoutePresent,
    bool EcMaintainedInLisp,
    bool ThinkingAboutThinkingTelemetryAvailable,
    bool GovernanceSlmIntelligentSwitchCandidate,
    bool GovernanceSlmMayDiscernActionReadiness,
    bool GovernanceSlmDiscernmentAuthorizesAction,
    bool StewardReviewed,
    bool StewardBondingReviewHeld,
    bool AuthorityGrantAbsent,
    bool ActionExecutorLocked,
    bool GelAdmissionLocked,
    bool SelfGelMutationLocked,
    bool HeartbeatLocked,
    bool CmeActualLocked,
    bool SanctuaryActualLocked,
    bool RuntimeActionAllowed,
    bool ArbitraryLispEvaluationAllowed,
    bool DatabaseWriteAllowed,
    bool MemoryAdmissionAllowed,
    bool SanctuaryActualAllowed,
    DateTimeOffset TimestampUtc)
{
    [JsonIgnore]
    public bool IsColdCmeActualBondingProcess =>
        Disposition == SanctuaryCmeActualBondingProcessDisposition.CompletedCold &&
        SliLispCmeActualBondingReceipt?.IsColdCmeActualBondingProcess == true &&
        ReviewOnly &&
        SliLispOwnedBondingMotion &&
        SourceToolBodyIdleHeld &&
        SourceLlmTickHeld &&
        SourceProductOutputWitnessCommitted &&
        SourceLineageHeld &&
        BondProcessDefined &&
        string.Equals(BondState, "cold-named-cme-actual-candidate-bonded-to-vehicle", StringComparison.OrdinalIgnoreCase) &&
        VehicleReady &&
        NamedCmeCandidateHeld &&
        NamingLineageWitnessed &&
        OperatorNamingIntentWitnessed &&
        !OperatorRuntimeAuthorityGranted &&
        ActivationAuthorityAbsent &&
        ActualAdmissionGapDescribed &&
        ReadyForCmeActualAdmissionReview &&
        FirstCmePath &&
        CmeActualCandidateOnly &&
        CmeActualBondedCandidate &&
        !CmeActualAdmitted &&
        !CmeActualActivated &&
        !RuntimeIdentityEmitted &&
        HeartbeatPrepared &&
        !HeartbeatActive &&
        !BeingStateClaimed &&
        !PersonhoodClaimed &&
        !SovereigntyClaimed &&
        !ModelBound &&
        !ProviderCalled &&
        !ActionAuthorized &&
        !GelAdmitted &&
        !SelfGelMutated &&
        !ContinuityAdmitted &&
        !AuthorityGranted &&
        VehiclePrimeAvailable &&
        VehicleCrypticAvailable &&
        VehicleStewardAvailable &&
        SliLispMembraneLoaded &&
        LispControlMatrixPresent &&
        ListeningFramePresent &&
        CompassPresent &&
        SoulFrameRoutePresent &&
        AgentiCoreRoutePresent &&
        EcMaintainedInLisp &&
        ThinkingAboutThinkingTelemetryAvailable &&
        GovernanceSlmIntelligentSwitchCandidate &&
        GovernanceSlmMayDiscernActionReadiness &&
        !GovernanceSlmDiscernmentAuthorizesAction &&
        StewardReviewed &&
        StewardBondingReviewHeld &&
        AuthorityGrantAbsent &&
        ActionExecutorLocked &&
        GelAdmissionLocked &&
        SelfGelMutationLocked &&
        HeartbeatLocked &&
        CmeActualLocked &&
        SanctuaryActualLocked &&
        !RuntimeActionAllowed &&
        !ArbitraryLispEvaluationAllowed &&
        !DatabaseWriteAllowed &&
        !MemoryAdmissionAllowed &&
        !SanctuaryActualAllowed;
}
