namespace San.Common;

public enum SanctuaryGelPredicatePoolDisposition
{
    Ready = 0,
    Silence = 1,
    Refused = 2
}

public sealed record SanctuaryGelPredicatePoolIdentity(
    string PoolHandle,
    string EnvironmentHandle,
    string ReceiptHandle);

public sealed record SanctuaryGelPredicateCandidate(
    string CandidateHandle,
    SanctuaryGelPredicateFamily Family,
    SanctuaryGelPredicateCandidateKind Kind,
    string PredicateLabel,
    string PredicateSummary,
    bool GoverningSeatReady,
    IReadOnlyList<string> WitnessRefs);

public sealed record SanctuaryGelPredicatePool(
    SanctuaryGelPredicatePoolIdentity Identity,
    string PredicateLineageSummary,
    string ActiveLanguage,
    string Locale,
    string Jurisdiction,
    RegionalAtlasPackageIdentity RegionalAtlasPackage,
    string PredicateInheritanceWitness,
    IReadOnlyList<SanctuaryGelPredicateFamilySet> FamilySets,
    IReadOnlyList<SanctuaryGelPredicateCandidate> Candidates,
    string GoverningSeatPostureSummary,
    string? UsePostureRef,
    string? SiteBindingProfileRef);

public sealed record SanctuaryGelPredicatePoolReceipt(
    string ReceiptHandle,
    SanctuaryGelPredicatePoolDisposition Disposition,
    string Summary,
    IReadOnlyList<string> WitnessRefs,
    DateTimeOffset TimestampUtc);

public sealed record SanctuaryGelPredicatePoolAssessment(
    SanctuaryGelFormationDataPoolAssessment DataPoolAssessment,
    SanctuaryGelPredicatePoolDisposition Disposition,
    string OutcomeCode,
    string Summary,
    SanctuaryGelPredicatePool? PredicatePool,
    SanctuaryGelPredicatePoolReceipt Receipt);
