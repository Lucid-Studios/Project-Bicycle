namespace San.Common;

public enum SanctuaryGelFormationDataPoolDisposition
{
    Ready = 0,
    Silence = 1,
    Refused = 2
}

public sealed record SanctuaryGelFormationDataPoolIdentity(
    string PoolHandle,
    string EnvironmentHandle,
    string ReceiptHandle);

public sealed record SanctuaryGelFormationCredentialFooting(
    string LicensingAgentId,
    string UserId,
    bool CertifiedCommunicationBasis);

public sealed record SanctuaryGelFormationPredicateInheritance(
    string PredicateLineageSummary,
    string PredicateInheritanceWitness,
    bool UniversalAtlasAuthorityClaimed);

public sealed record SanctuaryGelFormationDataPool(
    SanctuaryGelFormationDataPoolIdentity Identity,
    SanctuaryGelFormationCredentialFooting CredentialFooting,
    LocalizedInstallChoiceMatrix ChoiceMatrix,
    AgreementPredicateBundle AgreementBundle,
    InstallIdentitySetCandidate InstallIdentity,
    CoreCmeUsePostureRecord UsePosture,
    RegionalAtlasPackageIdentity RegionalAtlasPackage,
    SanctuaryGelFormationPredicateInheritance PredicateInheritance,
    CradleTekSiteBindingProfile? SiteBindingProfile);

public sealed record SanctuaryGelFormationDataPoolReceipt(
    string ReceiptHandle,
    SanctuaryGelFormationDataPoolDisposition Disposition,
    string Summary,
    IReadOnlyList<string> WitnessRefs,
    DateTimeOffset TimestampUtc);

public sealed record SanctuaryGelFormationDataPoolAssessment(
    SanctuaryGelFormationDataPoolDisposition Disposition,
    string OutcomeCode,
    string Summary,
    SanctuaryGelFormationDataPool? DataPool,
    SanctuaryGelFormationDataPoolReceipt Receipt);
