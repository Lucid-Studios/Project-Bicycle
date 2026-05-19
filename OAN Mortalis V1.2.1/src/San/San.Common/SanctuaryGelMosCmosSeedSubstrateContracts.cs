namespace San.Common;

public enum SanctuaryGelMosCmosSeedSubstrateDisposition
{
    Ready = 0,
    Held = 1,
    Refused = 2
}

public enum SanctuaryGelMosCmosSeedSubstrateLane
{
    PrimeMosSeed = 0,
    CrypticCmosSeed = 1,
    PairedBinderSpline = 2,
    NexusReadableModulation = 3
}

public enum SanctuaryGelMosCmosSeedSubstrateDeniedPower
{
    GoverningCme = 0,
    CryptographicKeyIssuance = 1,
    EncryptionRuntime = 2,
    PrimeMutation = 3,
    HiddenCrypticMutation = 4,
    NexusExecution = 5,
    SliLispExecution = 6,
    RuntimeControl = 7
}

public enum SanctuaryGelMosCmosSeedSubstrateRefusalReason
{
    None = 0,
    MissingSanctuaryGelSubstrate = 1,
    MissingPrimeMosSeed = 2,
    MissingCrypticCmosSeed = 3,
    MissingPairedBinderSpline = 4,
    MissingNexusReadableModulation = 5,
    GoverningCmeOverclaimed = 6,
    CryptographicAuthorityOverclaimed = 7,
    EncryptionRuntimeOverclaimed = 8,
    PrimeMutationOverclaimed = 9,
    HiddenCrypticMutationOverclaimed = 10,
    NexusExecutionOverclaimed = 11,
    SliLispExecutionOverclaimed = 12,
    RuntimeControlOverclaimed = 13,
    CmeFormationOverclaimed = 14
}

public sealed record SanctuaryGelMosCmosSeedSubstrateRecord(
    string SourceSanctuaryGelSubstrateRef,
    SanctuaryGelMosCmosSeedSubstrateDisposition Disposition,
    IReadOnlyList<SanctuaryGelMosCmosSeedSubstrateLane> Lanes,
    IReadOnlyList<SanctuaryGelMosCmosSeedSubstrateDeniedPower> DeniedPowers,
    IReadOnlyList<SanctuaryGelMosCmosSeedSubstrateRefusalReason> RefusalReasons,
    string PrimeMosSeedTelemetryPosture,
    string CrypticCmosBinderPosture,
    string PairedBinderSplinePosture,
    string NexusReadableModulationPosture,
    string NonAuthoritySummary,
    IReadOnlyList<string> WitnessRefs);

public sealed record SanctuaryGelMosCmosSeedSubstrateReceipt(
    string ReceiptHandle,
    SanctuaryGelMosCmosSeedSubstrateDisposition Disposition,
    string Summary,
    IReadOnlyList<string> WitnessRefs,
    DateTimeOffset TimestampUtc);
