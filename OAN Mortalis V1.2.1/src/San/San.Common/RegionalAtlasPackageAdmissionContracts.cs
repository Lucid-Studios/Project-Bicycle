namespace San.Common;

public enum RegionalAtlasPackageDisposition
{
    Admitted = 0,
    Refused = 1
}

public enum RegionalAtlasPackageKind
{
    EnglishRegionalAtlasPackage = 0
}

public sealed record RegionalAtlasPackageIdentity(
    string PackageHandle,
    RegionalAtlasPackageKind PackageKind,
    string LanguageGroup,
    string Locale,
    string SignedPayloadLineage);

public sealed record RegionalAtlasPackageSelection(
    RegionalAtlasPackageKind RequestedPackageKind,
    string RequestedLanguageGroup,
    string RequestedLocale,
    string RequestedJurisdiction);

public sealed record RegionalAtlasPackageAdmissionInput(
    LocalizedInstallChoiceMatrix ChoiceMatrix,
    InstallIdentitySetCandidate? InstallIdentity,
    RegionalAtlasPackageSelection Selection,
    string SignedPayloadLineage,
    string PackageWitness,
    string VerificationWitness,
    bool UniversalAtlasAuthorityClaimed);

public sealed record RegionalAtlasPackageAdmissionReceipt(
    string ReceiptHandle,
    RegionalAtlasPackageDisposition Disposition,
    string Summary,
    IReadOnlyList<string> WitnessRefs,
    DateTimeOffset TimestampUtc);

public sealed record RegionalAtlasPackageAdmissionAssessment(
    RegionalAtlasPackageAdmissionInput Input,
    RegionalAtlasPackageDisposition Disposition,
    string OutcomeCode,
    string Summary,
    RegionalAtlasPackageIdentity? PackageIdentity,
    RegionalAtlasPackageAdmissionReceipt Receipt);
