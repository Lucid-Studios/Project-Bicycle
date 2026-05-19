namespace San.Common;

public enum FirstUseEligibilityDisposition
{
    ReadyForConsideration = 0,
    Held = 1,
    Refused = 2
}

public enum FirstUseEligibilityPostureKind
{
    LocalizedGelFormation = 0,
    PredicateSurfaceReadiness = 1,
    PreGoverningStanding = 2,
    Disclosure = 3,
    LocalData = 4,
    Retention = 5,
    OptOut = 6,
    ResearchSeparation = 7,
    SpecialCaseHold = 8,
    DomainHold = 9,
    CounselReview = 10,
    NonAuthority = 11
}

public enum FirstUseEligibilityPostureState
{
    Represented = 0,
    Held = 1,
    Missing = 2,
    Refused = 3
}

public enum FirstUseEligibilityRefusalReason
{
    None = 0,
    MissingLocalizedGelFormation = 1,
    MissingPredicateSurfaceReadiness = 2,
    MissingDisclosurePosture = 3,
    MissingLocalDataPosture = 4,
    MissingRetentionOrOptOutPosture = 5,
    SpecialCaseNotHeld = 6,
    DomainUseNotHeld = 7,
    ResearchSeparationMissing = 8,
    CounselReviewOverclaimed = 9,
    RuntimeOrGovernanceOverclaimed = 10
}

public sealed record FirstUseEligibilityPosture(
    FirstUseEligibilityPostureKind Kind,
    FirstUseEligibilityPostureState State,
    string Summary,
    IReadOnlyList<string> WitnessRefs);

public sealed record FirstUseEligibilityRecord(
    FirstUseEligibilityDisposition Disposition,
    IReadOnlyList<FirstUseEligibilityPosture> Postures,
    string SourceLocalizedFormationRef,
    string SourcePreGoverningStandingRef,
    IReadOnlyList<FirstUseEligibilityRefusalReason> RefusalReasons,
    string NonPermissionSummary,
    IReadOnlyList<string> WitnessRefs);

public sealed record FirstUseEligibilityReceipt(
    string ReceiptHandle,
    FirstUseEligibilityDisposition Disposition,
    string Summary,
    IReadOnlyList<string> WitnessRefs,
    DateTimeOffset TimestampUtc);
