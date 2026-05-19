namespace San.Common;

public enum GelPredicatePriorFormalizationDisposition
{
    Ready = 0,
    Held = 1,
    Refused = 2
}

public enum GelPredicatePriorAdmissionCeiling
{
    CandidateOnly = 0
}

public enum GelPredicatePriorMutationPolicyPosture
{
    Withheld = 0,
    InvariantPreservingOnly = 1,
    ForkRequiredOnBodyChange = 2
}

public enum GelPredicatePriorTransportReceiptPosture
{
    NotEmitted = 0,
    FutureReceiptEligible = 1,
    RefusedAsActiveTransport = 2
}

public enum GelPredicatePriorFormalizationRefusalReason
{
    None = 0,
    MissingUtf8Witness = 1,
    MissingRootPredicate = 2,
    MissingSliConstructor = 3,
    MissingEngrammitizationFacingPosture = 4,
    MissingPredicateSurfaceReadiness = 5,
    SurvivorAdmissionOverclaimed = 6,
    FirstUseOverclaimed = 7,
    EcMutationOrTransportOverclaimed = 8,
    SliLispActivationOverclaimed = 9,
    RuntimeOrGovernanceOverclaimed = 10
}

public sealed record GelPredicatePriorUtf8Witness(
    string SourceTextRef,
    string EncodingState,
    string TokenSpanBounds,
    string LocalContextRef,
    string SourceWitnessRef,
    string AmbiguitySummary,
    string UnicodeSnapshotRef,
    IReadOnlyList<string> WitnessRefs);

public sealed record GelPredicatePriorRootPredicate(
    string RootPredicateRef,
    string RootCarrier,
    string SemanticFormationRef,
    string LineageSummary,
    IReadOnlyList<string> WitnessRefs);

public sealed record SliSymbolicConstructorAttachment(
    string PrefixSuper,
    string PrefixSub,
    string Body,
    string SuffixSuper,
    string SuffixSub,
    string StructuralSummary,
    IReadOnlyList<string> WitnessRefs);

public sealed record GelPredicatePriorFormalizationRecord(
    GelPredicatePriorFormalizationDisposition Disposition,
    string PredicatePriorRef,
    GelPredicatePriorUtf8Witness Utf8Witness,
    GelPredicatePriorRootPredicate RootPredicate,
    SliSymbolicConstructorAttachment Constructor,
    string GelPriorSummary,
    IReadOnlyList<string> InvariantSummaries,
    GelPredicatePriorMutationPolicyPosture MutationPolicyPosture,
    GelPredicatePriorTransportReceiptPosture TransportReceiptPosture,
    GelPredicatePriorAdmissionCeiling AdmissionCeiling,
    IReadOnlyList<GelPredicatePriorFormalizationRefusalReason> RefusalReasons,
    string NonAdmissionSummary,
    IReadOnlyList<string> WitnessRefs);

public sealed record GelPredicatePriorFormalizationReceipt(
    string ReceiptHandle,
    GelPredicatePriorFormalizationDisposition Disposition,
    string Summary,
    IReadOnlyList<string> WitnessRefs,
    DateTimeOffset TimestampUtc);
