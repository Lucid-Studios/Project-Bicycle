namespace San.Common;

public enum SanctuaryGelRegionalSubstrateFormationDisposition
{
    Ready = 0,
    Held = 1,
    Refused = 2
}

public enum SanctuaryGelRegionalSubstrateRefusalReason
{
    None = 0,
    MissingPredicatePriorRefs = 1,
    MissingLocalizedPreCertificationDataPool = 2,
    MissingNationalStanding = 3,
    MissingRegionalStanding = 4,
    MissingLocalStanding = 5,
    MissingRegionalPackageFooting = 6,
    AdmissionCeilingWidened = 7,
    SanctuaryActualOverclaimed = 8,
    MotherFatherGoverningCmeOverclaimed = 9,
    ModelSelectionOverclaimed = 10,
    CradleGelGenerationOverclaimed = 11,
    GovernanceOrRuntimeOverclaimed = 12
}

public enum SanctuaryGelRegionalSubstrateAdmissionCeiling
{
    CandidateOnly = 0,
    RegionalSubstrateOnly = 1
}

public sealed record SanctuaryGelRegionalSubstrateIdentity(
    string SubstrateHandle,
    string SanctuaryBodyRef,
    string RegionRef,
    string RegionalPackageFootingRef,
    string Summary,
    IReadOnlyList<string> WitnessRefs);

public sealed record SanctuaryGelRegionalSubstrateFormationRecord(
    SanctuaryGelRegionalSubstrateFormationDisposition Disposition,
    SanctuaryGelRegionalSubstrateIdentity Identity,
    IReadOnlyList<string> PredicatePriorRefs,
    IReadOnlyList<string> PredicatePoolRefs,
    IReadOnlyList<string> PredicateFamilyRefs,
    IReadOnlyList<string> LocalizedPreCertificationDataPoolRefs,
    IReadOnlyList<string> StandingRefs,
    string DataRightsPosture,
    string ResearchSeparationPosture,
    string SpecialCaseHoldPosture,
    string DomainHoldPosture,
    SanctuaryGelRegionalSubstrateAdmissionCeiling AdmissionCeiling,
    IReadOnlyList<SanctuaryGelRegionalSubstrateRefusalReason> RefusalReasons,
    string NonAuthoritySummary,
    IReadOnlyList<string> WitnessRefs);

public sealed record SanctuaryGelRegionalSubstrateFormationReceipt(
    string ReceiptHandle,
    SanctuaryGelRegionalSubstrateFormationDisposition Disposition,
    string Summary,
    IReadOnlyList<string> WitnessRefs,
    DateTimeOffset TimestampUtc);
