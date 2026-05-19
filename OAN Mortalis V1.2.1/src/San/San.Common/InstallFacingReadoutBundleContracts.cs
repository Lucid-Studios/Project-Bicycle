namespace San.Common;

public enum InstallFacingReadoutBundleDisposition
{
    Ready = 0,
    Silence = 1,
    Refused = 2
}

public enum InstallFacingReadoutSectionKind
{
    Posture = 0,
    TrustAuthorization = 1,
    EvidenceFooting = 2,
    ResponseDisposition = 3
}

public sealed record InstallFacingReadoutEntry(
    InstallFacingReadoutSectionKind SectionKind,
    SanctuaryGelPredicateFamily PredicateFamily,
    SanctuaryGelPredicateCandidateKind PredicateCandidateKind,
    string Phrase,
    string Summary,
    bool OperatorVisible,
    bool CertifiedLaneOnly);

public sealed record InstallFacingReadoutSection(
    InstallFacingReadoutSectionKind SectionKind,
    IReadOnlyList<InstallFacingReadoutEntry> Entries);

public sealed record InstallFacingReadoutBundle(
    InstallFacingReadoutBundleDisposition Disposition,
    IReadOnlyList<InstallFacingReadoutSection> Sections,
    IReadOnlyList<string> CorrespondenceRefs,
    IReadOnlyList<string> WitnessRefs);

public sealed record InstallFacingReadoutReceipt(
    string ReceiptHandle,
    InstallFacingReadoutBundleDisposition Disposition,
    string Summary,
    IReadOnlyList<string> WitnessRefs,
    DateTimeOffset TimestampUtc);
