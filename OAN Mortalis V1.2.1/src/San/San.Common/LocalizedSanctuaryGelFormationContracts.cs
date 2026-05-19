namespace San.Common;

public enum LocalizedSanctuaryGelFormationDisposition
{
    Ready = 0,
    Held = 1,
    Refused = 2
}

public enum LocalizedStandingRepresentationLayer
{
    National = 0,
    Regional = 1,
    Local = 2
}

public enum LocalizedSanctuaryGelFormationRefusalReason
{
    None = 0,
    MissingNationalStanding = 1,
    MissingRegionalStanding = 2,
    MissingLocalStanding = 3,
    OverclaimsGovernance = 4
}

public sealed record LocalizedStandingRepresentation(
    LocalizedStandingRepresentationLayer Layer,
    string RepresentationRef,
    string Summary,
    IReadOnlyList<string> WitnessRefs);

public sealed record LocalizedSanctuaryGelFormationRecord(
    LocalizedSanctuaryGelFormationDisposition Disposition,
    IReadOnlyList<LocalizedStandingRepresentation> StandingRepresentations,
    string SourceGelFormationRef,
    string PredicateFootingRef,
    string DataRightsPosture,
    string LegalAdminStagingPosture,
    string ContinuityDataPosture,
    LocalizedSanctuaryGelFormationRefusalReason RefusalReason,
    string NonAuthoritySummary,
    IReadOnlyList<string> WitnessRefs);

public sealed record LocalizedSanctuaryGelFormationReceipt(
    string ReceiptHandle,
    LocalizedSanctuaryGelFormationDisposition Disposition,
    string Summary,
    IReadOnlyList<string> WitnessRefs,
    DateTimeOffset TimestampUtc);
