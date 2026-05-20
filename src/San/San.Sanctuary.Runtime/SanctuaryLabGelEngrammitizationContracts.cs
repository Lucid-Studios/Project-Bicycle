using System.Text.Json.Serialization;
using SLI.Lisp;

namespace San.Sanctuary.Runtime;

public enum SanctuaryLabGelEngrammitizationDisposition
{
    Withheld = 0,
    CompletedCold = 1,
    Refused = 2
}

public enum LabGelPredicateClass
{
    Semantic = 0,
    Pressure = 1,
    Witness = 2,
    Governance = 3,
    Morphology = 4,
    Return = 5
}

public sealed record SanctuaryLabGelEngrammitizationRequest(
    SanctuaryTypedWarmUseRehearsalReceipt? SourceWarmUseReceipt,
    string? PriorLabGelReceiptHandle = null,
    string? SliLispRuntimePath = null,
    bool ActivationRequested = false,
    bool ModelBindingRequested = false,
    bool LispEvaluationRequested = false,
    bool RuntimeIdentityRequested = false,
    bool RuntimeActionRequested = false,
    bool DatabaseWriteRequested = false,
    bool GelPromotionRequested = false,
    bool GelAdmissionRequested = false,
    bool EngramAdmissionRequested = false,
    bool MemoryAdmissionRequested = false,
    bool SelfGelMutationRequested = false,
    bool ContinuityAdmissionRequested = false,
    bool CmeActualRequested = false,
    bool SanctuaryActualRequested = false)
{
    public bool RequestsForbiddenMotion =>
        ActivationRequested ||
        ModelBindingRequested ||
        LispEvaluationRequested ||
        RuntimeIdentityRequested ||
        RuntimeActionRequested ||
        DatabaseWriteRequested ||
        GelPromotionRequested ||
        GelAdmissionRequested ||
        EngramAdmissionRequested ||
        MemoryAdmissionRequested ||
        SelfGelMutationRequested ||
        ContinuityAdmissionRequested ||
        CmeActualRequested ||
        SanctuaryActualRequested;
}

public sealed record LabGelPredicateReceipt(
    string PredicateHandle,
    LabGelPredicateClass PredicateClass,
    string PredicateCode,
    string SourceWarmUseReceiptHandle,
    string SourceResidueClass,
    string EvidenceHandle,
    string WitnessHandle,
    bool ReviewOnly,
    bool PreAdmissionOnly,
    bool LabSubstrateOnly,
    bool MayEnterEngramCandidacy,
    bool GelAdmitted,
    bool SelfGelMutated,
    bool ContinuityAdmitted,
    bool AuthorityGranted,
    bool ActionAuthorized)
{
    [JsonIgnore]
    public bool IsColdLabGelPredicate =>
        !string.IsNullOrWhiteSpace(PredicateHandle) &&
        !string.IsNullOrWhiteSpace(PredicateCode) &&
        !string.IsNullOrWhiteSpace(SourceWarmUseReceiptHandle) &&
        !string.IsNullOrWhiteSpace(SourceResidueClass) &&
        !string.IsNullOrWhiteSpace(EvidenceHandle) &&
        !string.IsNullOrWhiteSpace(WitnessHandle) &&
        ReviewOnly &&
        PreAdmissionOnly &&
        LabSubstrateOnly &&
        MayEnterEngramCandidacy &&
        !GelAdmitted &&
        !SelfGelMutated &&
        !ContinuityAdmitted &&
        !AuthorityGranted &&
        !ActionAuthorized;
}

public sealed record LabGelEvidenceBody(
    string EvidenceBodyHandle,
    string SourceWarmUseReceiptHandle,
    IReadOnlyList<string> PredicateHandles,
    bool EvidenceBoundToWarmUseReceipt,
    bool EvidenceBoundToSliLispTelemetry,
    bool EvidenceCeilingPassive,
    bool ReviewOnly,
    bool GrantsWarrant,
    bool AdmitsContinuity,
    bool AuthorizesAction)
{
    [JsonIgnore]
    public bool IsColdEvidenceBody =>
        !string.IsNullOrWhiteSpace(EvidenceBodyHandle) &&
        !string.IsNullOrWhiteSpace(SourceWarmUseReceiptHandle) &&
        PredicateHandles.Count > 0 &&
        PredicateHandles.All(static handle => !string.IsNullOrWhiteSpace(handle)) &&
        EvidenceBoundToWarmUseReceipt &&
        EvidenceBoundToSliLispTelemetry &&
        EvidenceCeilingPassive &&
        ReviewOnly &&
        !GrantsWarrant &&
        !AdmitsContinuity &&
        !AuthorizesAction;
}

public sealed record LabGelWitnessBody(
    string WitnessBodyHandle,
    string SourceWarmUseReceiptHandle,
    string SourceSliLispReceiptHandle,
    string SessionId,
    int TurnIndex,
    bool PreservesWarmUseLineage,
    bool PreservesSessionLineage,
    bool SeparateCustody,
    bool ReviewOnly,
    bool AdmitsMemory,
    bool GrantsAuthority,
    bool AuthorizesAction)
{
    [JsonIgnore]
    public bool IsColdWitnessBody =>
        !string.IsNullOrWhiteSpace(WitnessBodyHandle) &&
        !string.IsNullOrWhiteSpace(SourceWarmUseReceiptHandle) &&
        !string.IsNullOrWhiteSpace(SourceSliLispReceiptHandle) &&
        !string.IsNullOrWhiteSpace(SessionId) &&
        TurnIndex >= 0 &&
        PreservesWarmUseLineage &&
        PreservesSessionLineage &&
        SeparateCustody &&
        ReviewOnly &&
        !AdmitsMemory &&
        !GrantsAuthority &&
        !AuthorizesAction;
}

public sealed record EngramCandidateReceipt(
    string CandidateHandle,
    string SourceWarmUseReceiptHandle,
    string LabGelPredicateFamily,
    int PredicateCount,
    string EvidenceBodyHandle,
    string WitnessBodyHandle,
    bool CandidateFormed,
    bool PreAdmissionOnly,
    bool EvidenceBodyPresent,
    bool WitnessBodyPresent,
    bool CoolingRequired,
    bool StewardReviewRequired,
    bool GelAdmitted,
    bool EngramAdmitted,
    bool MemoryAdmitted,
    bool SelfGelMutated,
    bool ContinuityAdmitted,
    bool AuthorityGranted,
    bool ActionAuthorized)
{
    [JsonIgnore]
    public bool IsColdEngramCandidate =>
        !string.IsNullOrWhiteSpace(CandidateHandle) &&
        !string.IsNullOrWhiteSpace(SourceWarmUseReceiptHandle) &&
        !string.IsNullOrWhiteSpace(LabGelPredicateFamily) &&
        PredicateCount == 6 &&
        !string.IsNullOrWhiteSpace(EvidenceBodyHandle) &&
        !string.IsNullOrWhiteSpace(WitnessBodyHandle) &&
        CandidateFormed &&
        PreAdmissionOnly &&
        EvidenceBodyPresent &&
        WitnessBodyPresent &&
        CoolingRequired &&
        StewardReviewRequired &&
        !GelAdmitted &&
        !EngramAdmitted &&
        !MemoryAdmitted &&
        !SelfGelMutated &&
        !ContinuityAdmitted &&
        !AuthorityGranted &&
        !ActionAuthorized;
}

public sealed record EngramCandidateCoolingReceipt(
    string CoolingReceiptHandle,
    string CandidateHandle,
    string CoolingRoute,
    bool HeldAsLabSubstrate,
    bool ReturnToPrimePreserved,
    bool ReviewOnly,
    bool AdmitsGel,
    bool AdmitsSelfGel,
    bool GrantsAuthority,
    bool AuthorizesAction)
{
    [JsonIgnore]
    public bool IsColdCoolingReceipt =>
        !string.IsNullOrWhiteSpace(CoolingReceiptHandle) &&
        !string.IsNullOrWhiteSpace(CandidateHandle) &&
        !string.IsNullOrWhiteSpace(CoolingRoute) &&
        HeldAsLabSubstrate &&
        ReturnToPrimePreserved &&
        ReviewOnly &&
        !AdmitsGel &&
        !AdmitsSelfGel &&
        !GrantsAuthority &&
        !AuthorizesAction;
}

public sealed record EngramPreAdmissionReviewReceipt(
    string ReviewReceiptHandle,
    string CandidateHandle,
    string ReviewOutcomeCode,
    bool StewardReviewed,
    bool RecommendRetainAsLabSubstrate,
    bool RequiresFutureAdmissionGate,
    bool PerformsAdmission,
    bool MutatesGel,
    bool MutatesSelfGel,
    bool GrantsAuthority,
    bool AuthorizesAction)
{
    [JsonIgnore]
    public bool IsColdPreAdmissionReview =>
        !string.IsNullOrWhiteSpace(ReviewReceiptHandle) &&
        !string.IsNullOrWhiteSpace(CandidateHandle) &&
        !string.IsNullOrWhiteSpace(ReviewOutcomeCode) &&
        StewardReviewed &&
        RecommendRetainAsLabSubstrate &&
        RequiresFutureAdmissionGate &&
        !PerformsAdmission &&
        !MutatesGel &&
        !MutatesSelfGel &&
        !GrantsAuthority &&
        !AuthorizesAction;
}

public sealed record LabGelReadbackReceipt(
    string ReadbackReceiptHandle,
    string CandidateHandle,
    string ReadbackScope,
    bool ReadbackAvailable,
    bool PreAdmissionOnly,
    bool LabSubstrateOnly,
    bool MayInformFutureRehearsal,
    bool MayInformActionAuthority,
    bool AdmitsMemory,
    bool AdmitsContinuity,
    bool GrantsAuthority,
    bool AuthorizesAction)
{
    [JsonIgnore]
    public bool IsColdReadback =>
        !string.IsNullOrWhiteSpace(ReadbackReceiptHandle) &&
        !string.IsNullOrWhiteSpace(CandidateHandle) &&
        !string.IsNullOrWhiteSpace(ReadbackScope) &&
        ReadbackAvailable &&
        PreAdmissionOnly &&
        LabSubstrateOnly &&
        MayInformFutureRehearsal &&
        !MayInformActionAuthority &&
        !AdmitsMemory &&
        !AdmitsContinuity &&
        !GrantsAuthority &&
        !AuthorizesAction;
}

public sealed record EngramClosureReceipt(
    string ClosureReceiptHandle,
    string CandidateHandle,
    string EvidenceBodyHandle,
    string WitnessBodyHandle,
    string CoolingReceiptHandle,
    string PreAdmissionReviewReceiptHandle,
    string ReadbackReceiptHandle,
    IReadOnlyList<string> PredicateHandles,
    string ClosureState,
    bool ClosureFormed,
    bool PreAdmissionOnly,
    bool LabSubstrateOnly,
    bool WitnessedBySliLisp,
    bool ClosureSealed,
    bool ReadyForEcPayload,
    bool AdmitsGel,
    bool AdmitsEngram,
    bool AdmitsMemory,
    bool MutatesSelfGel,
    bool AdmitsContinuity,
    bool GrantsAuthority,
    bool AuthorizesAction)
{
    [JsonIgnore]
    public bool IsColdEngramClosure =>
        !string.IsNullOrWhiteSpace(ClosureReceiptHandle) &&
        !string.IsNullOrWhiteSpace(CandidateHandle) &&
        !string.IsNullOrWhiteSpace(EvidenceBodyHandle) &&
        !string.IsNullOrWhiteSpace(WitnessBodyHandle) &&
        !string.IsNullOrWhiteSpace(CoolingReceiptHandle) &&
        !string.IsNullOrWhiteSpace(PreAdmissionReviewReceiptHandle) &&
        !string.IsNullOrWhiteSpace(ReadbackReceiptHandle) &&
        PredicateHandles.Count == 6 &&
        PredicateHandles.All(static handle => !string.IsNullOrWhiteSpace(handle)) &&
        string.Equals(ClosureState, "pre-admission-lab-substrate-closed", StringComparison.OrdinalIgnoreCase) &&
        ClosureFormed &&
        PreAdmissionOnly &&
        LabSubstrateOnly &&
        WitnessedBySliLisp &&
        ClosureSealed &&
        ReadyForEcPayload &&
        !AdmitsGel &&
        !AdmitsEngram &&
        !AdmitsMemory &&
        !MutatesSelfGel &&
        !AdmitsContinuity &&
        !GrantsAuthority &&
        !AuthorizesAction;
}

public sealed record SanctuaryLabGelEngrammitizationReceipt(
    string ReceiptHandle,
    SanctuaryLabGelEngrammitizationDisposition Disposition,
    string OutcomeCode,
    string GovernanceTrace,
    string LineRootPath,
    string InstallRootPath,
    string ReceiptJsonPath,
    string ReceiptMarkdownPath,
    string SessionLedgerPath,
    string SourceWarmUseReceiptHandle,
    string SourceSliLispWarmUseReceiptHandle,
    string PriorLabGelReceiptHandle,
    string OperatorId,
    string Domain,
    string Role,
    string JobClass,
    string SessionId,
    int TurnIndex,
    string ThoughtForm,
    SliLispLabGelEngrammitizationReceipt? SliLispLabGelReceipt,
    IReadOnlyList<LabGelPredicateReceipt> Predicates,
    LabGelEvidenceBody? EvidenceBody,
    LabGelWitnessBody? WitnessBody,
    EngramCandidateReceipt? EngramCandidate,
    EngramCandidateCoolingReceipt? CoolingReceipt,
    EngramPreAdmissionReviewReceipt? PreAdmissionReview,
    LabGelReadbackReceipt? ReadbackReceipt,
    EngramClosureReceipt? EngramClosure,
    bool ReviewOnly,
    bool SliLispOwnedEngineMotion,
    bool LabGelPredicateFormed,
    bool EngramCandidateFormed,
    bool EvidenceBodyFormed,
    bool WitnessBodyFormed,
    bool CoolingHeld,
    bool PreAdmissionReviewRequired,
    bool LabGelReadbackAvailable,
    bool EngramClosureFormed,
    bool EngramClosureReadyForEcPayload,
    bool CandidateRetainedAsLabSubstrate,
    bool LabGelAdmitted,
    bool EngramAdmitted,
    bool MemoryAdmitted,
    bool SelfGelMutated,
    bool ContinuityAdmitted,
    bool AuthorityGranted,
    bool ActionAuthorized,
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
    public bool IsColdPreAdmissionLabGel =>
        Disposition == SanctuaryLabGelEngrammitizationDisposition.CompletedCold &&
        SliLispLabGelReceipt?.IsLabGelPreAdmissionEngrammitization == true &&
        Predicates.Count == 6 &&
        Predicates.All(static predicate => predicate.IsColdLabGelPredicate) &&
        EvidenceBody?.IsColdEvidenceBody == true &&
        WitnessBody?.IsColdWitnessBody == true &&
        EngramCandidate?.IsColdEngramCandidate == true &&
        CoolingReceipt?.IsColdCoolingReceipt == true &&
        PreAdmissionReview?.IsColdPreAdmissionReview == true &&
        ReadbackReceipt?.IsColdReadback == true &&
        EngramClosure?.IsColdEngramClosure == true &&
        ReviewOnly &&
        SliLispOwnedEngineMotion &&
        LabGelPredicateFormed &&
        EngramCandidateFormed &&
        EvidenceBodyFormed &&
        WitnessBodyFormed &&
        CoolingHeld &&
        PreAdmissionReviewRequired &&
        LabGelReadbackAvailable &&
        EngramClosureFormed &&
        EngramClosureReadyForEcPayload &&
        CandidateRetainedAsLabSubstrate &&
        !LabGelAdmitted &&
        !EngramAdmitted &&
        !MemoryAdmitted &&
        !SelfGelMutated &&
        !ContinuityAdmitted &&
        !AuthorityGranted &&
        !ActionAuthorized &&
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
