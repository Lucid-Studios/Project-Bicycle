namespace San.Common;

public enum SanctuaryGelFormationDisposition
{
    Hydrated = 0,
    Admitted = 1,
    Engrammatized = 2,
    Retained = 3,
    Resting = 4,
    Refused = 5
}

public sealed record SanctuaryGelSubstrateIdentity(
    string SubstrateHandle,
    string EnvironmentHandle,
    string FormationReceiptHandle);

public sealed record SanctuaryGelFormationInput(
    SanctuaryGelPredicatePoolAssessment PredicatePoolAssessment,
    string DerivedPayloadLineage,
    string SymbolicAnchorSummary,
    string SymbolicTransformWitness,
    string EngrammatizationWitness,
    SanctuaryGelSubstrateIdentity SubstrateIdentity,
    bool RawRootAtlasResidencyClaimed,
    bool LabSideTemplatingAuthorityClaimed,
    bool PublicProjectionRequested);

public sealed record SanctuaryGelSubstrateRecord(
    SanctuaryGelSubstrateIdentity Identity,
    SanctuaryGelFormationDisposition State,
    string DerivedPayloadLineage,
    string SymbolicAnchorSummary,
    string PredicatePoolHandle,
    IReadOnlyList<SanctuaryGelPredicateFamily> PredicateFamilies,
    IReadOnlyList<SanctuaryGelPredicateCandidateKind> InheritedPredicateKinds,
    bool Retained,
    bool RestCapable,
    IReadOnlyList<string> WitnessRefs);

public sealed record SanctuaryGelFormationReceipt(
    string ReceiptHandle,
    SanctuaryGelFormationDisposition Disposition,
    string Summary,
    IReadOnlyList<string> WitnessRefs,
    DateTimeOffset TimestampUtc);

public sealed record SanctuaryGelFormationAssessment(
    SanctuaryGelFormationInput Input,
    SanctuaryGelFormationDisposition Disposition,
    string OutcomeCode,
    string Summary,
    SanctuaryGelSubstrateRecord? SubstrateRecord,
    SanctuaryGelFormationReceipt Receipt);
