namespace San.Common;

public enum GenerativeEngrammitizationLibraryFormationReadinessDisposition
{
    Ready = 0,
    Held = 1,
    Refused = 2
}

public enum GenerativeEngrammitizationLibraryCandidatePostureKind
{
    LogicalResearchSourcePosture = 0,
    RootedSourcePosture = 1,
    WitnessedPosture = 2,
    SliFormedPosture = 3,
    EngrammitizationFacingPosture = 4,
    PredicateSurfaceReadiness = 5,
    FormationLineageSummary = 6
}

public enum GenerativeEngrammitizationLibraryFormationReadinessRefusalReason
{
    None = 0,
    MissingRootedSourcePosture = 1,
    MissingWitnessPosture = 2,
    MissingSliFormationPosture = 3,
    MissingEngrammitizationFacingPosture = 4,
    SurvivorAdmissionOverclaimed = 5,
    LocalizedSanctuaryGelFormationOverclaimed = 6,
    FirstUseOverclaimed = 7,
    RuntimeOrGovernanceOverclaimed = 8
}

public sealed record GenerativeEngrammitizationLibraryCandidatePosture(
    GenerativeEngrammitizationLibraryCandidatePostureKind Kind,
    string PostureRef,
    string Summary,
    IReadOnlyList<string> WitnessRefs);

public sealed record GenerativeEngrammitizationLibraryFormationReadinessRecord(
    GenerativeEngrammitizationLibraryFormationReadinessDisposition Disposition,
    IReadOnlyList<GenerativeEngrammitizationLibraryCandidatePosture> CandidatePostures,
    IReadOnlyList<string> SourceLogicalResearchRefs,
    IReadOnlyList<string> SourcePredicateSurfaceRefs,
    IReadOnlyList<GenerativeEngrammitizationLibraryFormationReadinessRefusalReason> RefusalReasons,
    string NonAdmissionSummary,
    IReadOnlyList<string> WitnessRefs);

public sealed record GenerativeEngrammitizationLibraryFormationReadinessReceipt(
    string ReceiptHandle,
    GenerativeEngrammitizationLibraryFormationReadinessDisposition Disposition,
    string Summary,
    IReadOnlyList<string> WitnessRefs,
    DateTimeOffset TimestampUtc);
