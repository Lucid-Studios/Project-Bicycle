namespace San.Common;

public enum FirstUseAdmissionDisposition
{
    Ready = 0,
    Held = 1,
    Refused = 2
}

public enum FirstUseEnactmentDisposition
{
    Prepared = 0,
    Held = 1,
    Refused = 2
}

public enum FirstUseAdmissionRefusalReason
{
    None = 0,
    MissingEligibility = 1,
    MissingFormationAttempt = 2,
    EligibilityNotReady = 3,
    FormationAttemptNotReady = 4,
    DisclosureOrDataPostureMissing = 5,
    RuntimeOrGovernanceOverclaimed = 6
}

public enum FirstUseEnactmentRefusalReason
{
    None = 0,
    MissingAdmission = 1,
    AdmissionNotReady = 2,
    WitnessMissing = 3,
    RuntimeTransactionOverclaimed = 4,
    RtmeOrSliLispOverclaimed = 5,
    ModelSelectionOverclaimed = 6,
    SanctuaryActualOrCradleGelOverclaimed = 7
}

public sealed record FirstUseAdmissionRecord(
    string EligibilityRef,
    string FormationAttemptRef,
    FirstUseAdmissionDisposition Disposition,
    IReadOnlyList<FirstUseAdmissionRefusalReason> RefusalReasons,
    string DisclosureDataPostureSummary,
    string NonAuthoritySummary,
    IReadOnlyList<string> WitnessRefs);

public sealed record FirstUseAdmissionReceipt(
    string ReceiptHandle,
    FirstUseAdmissionDisposition Disposition,
    string Summary,
    IReadOnlyList<string> WitnessRefs,
    DateTimeOffset TimestampUtc);

public sealed record FirstUseEnactmentRecord(
    string AdmissionRef,
    FirstUseEnactmentDisposition Disposition,
    IReadOnlyList<FirstUseEnactmentRefusalReason> RefusalReasons,
    string EnactmentWitnessSummary,
    string NonAuthoritySummary,
    IReadOnlyList<string> WitnessRefs);

public sealed record FirstUseEnactmentReceipt(
    string ReceiptHandle,
    FirstUseEnactmentDisposition Disposition,
    string Summary,
    IReadOnlyList<string> WitnessRefs,
    DateTimeOffset TimestampUtc);
