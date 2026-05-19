namespace San.Common;

public enum GelRtmeSliLispReadinessDisposition
{
    Ready = 0,
    Held = 1,
    Refused = 2
}

public enum GelRtmeSliLispReadinessPosture
{
    CandidateReadable = 0,
    HeldForGate = 1,
    MovementWithheld = 2
}

public enum GelRtmeSliLispDeniedCapability
{
    RtmeActivation = 0,
    RuntimeTransactionMovement = 1,
    AlwaysOnAuthority = 2,
    DirectPersistence = 3,
    DirectPrimeMutation = 4,
    MembraneBypass = 5,
    SliLispExecution = 6,
    SurvivorAdmission = 7
}

public enum GelRtmeSliLispReadinessRefusalReason
{
    None = 0,
    MissingSanctuaryGelStanding = 1,
    MissingWitnessStorePosture = 2,
    MovementOverclaimed = 3,
    PersistenceOverclaimed = 4,
    AuthorityOverclaimed = 5,
    SliLispExecutionOverclaimed = 6,
    RtmeActivationOverclaimed = 7,
    MembraneBypassOverclaimed = 8,
    SurvivorAdmissionOverclaimed = 9
}

public sealed record GelRtmeSliLispNonActivationReadinessRecord(
    string SourceSanctuaryGelStandingRef,
    string SourceWitnessStoreRef,
    GelRtmeSliLispReadinessDisposition Disposition,
    GelRtmeSliLispReadinessPosture ReadinessPosture,
    IReadOnlyList<GelRtmeSliLispDeniedCapability> DeniedCapabilities,
    IReadOnlyList<GelRtmeSliLispReadinessRefusalReason> RefusalReasons,
    string NonActivationSummary,
    IReadOnlyList<string> WitnessRefs);

public sealed record GelRtmeSliLispNonActivationReadinessReceipt(
    string ReceiptHandle,
    GelRtmeSliLispReadinessDisposition Disposition,
    string Summary,
    IReadOnlyList<string> WitnessRefs,
    DateTimeOffset TimestampUtc);
