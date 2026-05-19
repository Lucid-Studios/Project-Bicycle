namespace San.Common;

public enum LocalizedPreCertificationDataPoolDisposition
{
    Ready = 0,
    Held = 1,
    Refused = 2
}

public enum LocalizedPreCertificationDataPoolInputKind
{
    LabAssetCandidate = 0,
    RootAtlasRegionalPosture = 1,
    LegalAdminTemplateFamily = 2,
    NationalStanding = 3,
    RegionalStanding = 4,
    LocalStanding = 5,
    DataRightsPosture = 6,
    ResearchSeparationPosture = 7,
    SpecialCaseHold = 8,
    DomainHold = 9,
    NonAuthoritySummary = 10
}

public enum LocalizedPreCertificationDataPoolRefusalReason
{
    None = 0,
    MissingNationalStanding = 1,
    MissingRegionalStanding = 2,
    MissingLocalStanding = 3,
    ActiveLegalTermsOverclaimed = 4,
    CertificationOverclaimed = 5,
    ConsentRecordOverclaimed = 6,
    DisclosureIssuanceOverclaimed = 7,
    DomainAuthorizationOverclaimed = 8,
    FirstUseAdmissionOverclaimed = 9,
    RtmeOverclaimed = 10,
    GoverningCmeOverclaimed = 11,
    RuntimeAuthorityOverclaimed = 12
}

public sealed record LocalizedPreCertificationDataPoolInput(
    LocalizedPreCertificationDataPoolInputKind Kind,
    string InputRef,
    string Summary,
    IReadOnlyList<string> WitnessRefs);

public sealed record LocalizedPreCertificationDataPoolRecord(
    LocalizedPreCertificationDataPoolDisposition Disposition,
    IReadOnlyList<LocalizedPreCertificationDataPoolInput> Inputs,
    IReadOnlyList<string> SourceLocalizedStandingRefs,
    IReadOnlyList<string> SourceLegalAdminStagingRefs,
    IReadOnlyList<LocalizedPreCertificationDataPoolRefusalReason> RefusalReasons,
    string NonAuthoritySummary,
    IReadOnlyList<string> WitnessRefs);

public sealed record LocalizedPreCertificationDataPoolReceipt(
    string ReceiptHandle,
    LocalizedPreCertificationDataPoolDisposition Disposition,
    string Summary,
    IReadOnlyList<string> WitnessRefs,
    DateTimeOffset TimestampUtc);
