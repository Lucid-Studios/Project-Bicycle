namespace San.Common;

public enum SanctuaryGelFirstFormationAttemptDisposition
{
    Ready = 0,
    Held = 1,
    Refused = 2
}

public enum SanctuaryGelFirstFormationAttemptRefusalReason
{
    None = 0,
    MissingPredicatePriors = 1,
    MissingLocalizedPreCertificationDataPool = 2,
    MissingLocalizedFormationFloor = 3,
    MissingNationalStanding = 4,
    MissingRegionalStanding = 5,
    MissingLocalStanding = 6,
    MissingRegionalSubstrateFooting = 7,
    MissingPreGoverningStanding = 8,
    MissingFirstUseEligibilityConsideration = 9,
    SanctuaryActualOverclaimed = 10,
    SurvivorAdmissionOverclaimed = 11,
    FirstUseAdmissionOverclaimed = 12,
    ModelSelectionOverclaimed = 13,
    RuntimeAuthorityOverclaimed = 14,
    CradleGelGenerationOverclaimed = 15,
    SliLispOrRtmeActivationOverclaimed = 16
}

public sealed record SanctuaryGelFirstFormationAttemptInput(
    IReadOnlyList<GelPredicatePriorFormalizationRecord> PredicatePriors,
    LocalizedPreCertificationDataPoolRecord? LocalizedPreCertificationDataPool,
    LocalizedSanctuaryGelFormationRecord? LocalizedFormation,
    SanctuaryGelRegionalSubstrateFormationRecord? RegionalSubstrate,
    SanctuaryPreGoverningStandingRecord? PreGoverningStanding,
    FirstUseEligibilityRecord? FirstUseEligibility,
    bool SanctuaryActualClaimed,
    bool SurvivorAdmissionClaimed,
    bool FirstUseAdmissionClaimed,
    bool ModelSelectionClaimed,
    bool RuntimeAuthorityClaimed,
    bool CradleGelGenerationClaimed,
    bool SliLispOrRtmeActivationClaimed);

public sealed record SanctuaryGelFirstFormationAttemptRecord(
    string AttemptHandle,
    SanctuaryGelFirstFormationAttemptDisposition Disposition,
    IReadOnlyList<string> PredicatePriorRefs,
    IReadOnlyList<string> LocalizedPreCertificationDataPoolRefs,
    IReadOnlyList<string> LocalizedFormationRefs,
    IReadOnlyList<string> StandingRefs,
    string RegionalSubstrateRef,
    string PreGoverningStandingRef,
    string FirstUseEligibilityRef,
    IReadOnlyList<SanctuaryGelFirstFormationAttemptRefusalReason> RefusalReasons,
    string NonAuthoritySummary,
    IReadOnlyList<string> WitnessRefs);

public sealed record SanctuaryGelFirstFormationAttemptReceipt(
    string ReceiptHandle,
    SanctuaryGelFirstFormationAttemptDisposition Disposition,
    string Summary,
    IReadOnlyList<string> WitnessRefs,
    DateTimeOffset TimestampUtc);

public sealed record SanctuaryGelFirstFormationAttemptAssessment(
    SanctuaryGelFirstFormationAttemptInput Input,
    SanctuaryGelFirstFormationAttemptDisposition Disposition,
    string OutcomeCode,
    string Summary,
    SanctuaryGelFirstFormationAttemptRecord AttemptRecord,
    SanctuaryGelFirstFormationAttemptReceipt Receipt);
