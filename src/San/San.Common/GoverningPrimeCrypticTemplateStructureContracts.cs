namespace San.Common;

public enum GoverningPrimeCrypticTemplateDisposition
{
    Ready = 0,
    Held = 1,
    Refused = 2
}

public enum GoverningPrimeCrypticTemplateOffice
{
    GoverningPrime = 0,
    GoverningCryptic = 1,
    PairedPrimeCrypticReceipt = 2,
    UserDataPredicatePosture = 3
}

public enum GoverningPrimeCrypticUserDataPredicateKind
{
    EntityPosture = 0,
    AuthorityToBindPosture = 1,
    ConsentScope = 2,
    DisclosureRefs = 3,
    LocalDataCategory = 4,
    RetentionOptOut = 5,
    ResearchSeparation = 6,
    SpecialCaseQuarantine = 7,
    IpAssetPosture = 8,
    OperationalTelemetryPosture = 9,
    NonAuthorityPosture = 10
}

public enum GoverningPrimeCrypticTemplateDeniedCapability
{
    DataCollection = 0,
    ConsentCapture = 1,
    Surveillance = 2,
    Profiling = 3,
    Training = 4,
    ResearchUse = 5,
    ProviderSync = 6,
    CryptographicAuthority = 7,
    EncryptionRuntime = 8,
    PrimeMutation = 9,
    CrypticMutation = 10,
    GoverningCmeActivation = 11,
    SliLispExecution = 12,
    RtmeMovement = 13,
    RuntimeControl = 14
}

public enum GoverningPrimeCrypticTemplateRefusalReason
{
    None = 0,
    MissingMosCmosSeedSubstrate = 1,
    MissingGoverningPrimeTemplate = 2,
    MissingGoverningCrypticTemplate = 3,
    MissingPairedPrimeCrypticReceipt = 4,
    MissingUserDataPredicatePosture = 5,
    DataCollectionOverclaimed = 6,
    ConsentCaptureOverclaimed = 7,
    SurveillanceOverclaimed = 8,
    ProfilingOverclaimed = 9,
    TrainingOverclaimed = 10,
    ResearchUseOverclaimed = 11,
    ProviderSyncOverclaimed = 12,
    CryptographicAuthorityOverclaimed = 13,
    MutationOverclaimed = 14,
    SliLispExecutionOverclaimed = 15,
    RtmeMovementOverclaimed = 16,
    GovernanceOrRuntimeOverclaimed = 17
}

public sealed record GoverningPrimeCrypticTemplateRecord(
    string SourceMosCmosSeedSubstrateRef,
    GoverningPrimeCrypticTemplateDisposition Disposition,
    IReadOnlyList<GoverningPrimeCrypticTemplateOffice> Offices,
    IReadOnlyList<GoverningPrimeCrypticUserDataPredicateKind> UserDataPredicateKinds,
    IReadOnlyList<GoverningPrimeCrypticTemplateDeniedCapability> DeniedCapabilities,
    IReadOnlyList<GoverningPrimeCrypticTemplateRefusalReason> RefusalReasons,
    string GoverningPrimeTemplatePosture,
    string GoverningCrypticTemplatePosture,
    string PairedPrimeCrypticReceiptPosture,
    string UserDataPredicatePosture,
    string NonAuthoritySummary,
    IReadOnlyList<string> WitnessRefs);

public sealed record GoverningPrimeCrypticTemplateReceipt(
    string ReceiptHandle,
    GoverningPrimeCrypticTemplateDisposition Disposition,
    string Summary,
    IReadOnlyList<string> WitnessRefs,
    DateTimeOffset TimestampUtc);
