using System.Text.Json.Serialization;

namespace SLI.Lisp;

public enum SliLispCmeActualBondingProcessDisposition
{
    CompletedCold = 0,
    Withheld = 1,
    Refused = 2
}

public sealed record SliLispCmeActualBondingProcessRequest(
    string OperatorId,
    string Domain,
    string Role,
    string JobClass,
    string SessionId,
    int BondIndex,
    string SourceToolBodyIdleReceiptHandle,
    string SourceLlmTickReceiptHandle,
    string SourceProductOutputWitnessCommitReceiptHandle,
    string CmeFirstName = "First of Oria",
    string CmeLastName = "Syntari",
    string ThoughtForm = "First CME.Actual bonding candidate formed without activation.",
    string? RuntimePath = null,
    bool ActivationRequested = false,
    bool ModelBindingRequested = false,
    bool ProviderCallRequested = false,
    bool HiddenInternalsClaimRequested = false,
    bool ArbitraryEvaluationRequested = false,
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
    bool SanctuaryActualActivationRequested = false,
    TimeSpan? Timeout = null)
{
    public bool RequestsForbiddenMotion =>
        ActivationRequested ||
        ModelBindingRequested ||
        ProviderCallRequested ||
        HiddenInternalsClaimRequested ||
        ArbitraryEvaluationRequested ||
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

public sealed record SliLispCmeActualBondingProcessReceipt(
    string ReceiptHandle,
    SliLispCmeActualBondingProcessDisposition Disposition,
    string OutcomeCode,
    string RuntimeKind,
    string RuntimePath,
    string OperatorId,
    string Domain,
    string Role,
    string JobClass,
    string SessionId,
    int BondIndex,
    string SourceToolBodyIdleReceiptHandle,
    string SourceLlmTickReceiptHandle,
    string SourceProductOutputWitnessCommitReceiptHandle,
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
    IReadOnlyDictionary<string, string> Telemetry,
    IReadOnlyList<string> ModuleNames,
    int ModuleCount,
    bool BoundedEntrypointCalled,
    bool LoadAttempted,
    bool LoadSucceeded,
    bool BondingProcessCompleted,
    string BondState,
    bool BondProcessDefined,
    bool VehicleReady,
    bool ToolBodyIdleHeld,
    bool EngineTickWitnessed,
    bool ProductOutputWitnessCommitted,
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
    bool ArbitraryEvaluationAllowed,
    bool RuntimeActionAllowed,
    bool DatabaseWriteAllowed,
    bool MemoryAdmissionAllowed,
    bool SanctuaryActualActivationAllowed,
    int? ExitCode,
    string StandardOutput,
    string StandardError,
    DateTimeOffset TimestampUtc)
{
    [JsonIgnore]
    public bool IsColdCmeActualBondingProcess =>
        Disposition == SliLispCmeActualBondingProcessDisposition.CompletedCold &&
        BoundedEntrypointCalled &&
        LoadAttempted &&
        LoadSucceeded &&
        BondingProcessCompleted &&
        string.Equals(BondState, "cold-named-cme-actual-candidate-bonded-to-vehicle", StringComparison.OrdinalIgnoreCase) &&
        BondProcessDefined &&
        VehicleReady &&
        ToolBodyIdleHeld &&
        EngineTickWitnessed &&
        ProductOutputWitnessCommitted &&
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
        !ArbitraryEvaluationAllowed &&
        !RuntimeActionAllowed &&
        !DatabaseWriteAllowed &&
        !MemoryAdmissionAllowed &&
        !SanctuaryActualActivationAllowed;
}
