namespace San.Common;

public enum SanctuaryLocalConversationWitnessStoreDisposition
{
    Ready = 0,
    Held = 1,
    Refused = 2
}

public enum SanctuaryLocalConversationWitnessStorePosture
{
    LocalOnly = 0,
    Quarantined = 1,
    Withheld = 2
}

public enum SanctuaryLocalConversationWitnessCapability
{
    ProviderVisibleAccess = 0,
    ResearchUse = 1,
    ModelTrainingOrImprovement = 2,
    PartnerOrProviderSharing = 3,
    Rehydration = 4,
    GelCandidateGeneration = 5,
    GelSurvivorAdmission = 6,
    RtmeMovement = 7,
    RtmeActivation = 8
}

public enum SanctuaryLocalConversationWitnessRefusalReason
{
    None = 0,
    MissingFirstUseEnactment = 1,
    MissingRetentionPosture = 2,
    ResearchConsentOverclaimed = 3,
    ProviderAccessOverclaimed = 4,
    ModelMemoryOverclaimed = 5,
    TrainingUseOverclaimed = 6,
    GelAdmissionOverclaimed = 7,
    RtmeMovementOverclaimed = 8,
    RuntimeAuthorityOverclaimed = 9
}

public sealed record SanctuaryLocalConversationWitnessStoreRecord(
    string SourceEnactmentRef,
    SanctuaryLocalConversationWitnessStoreDisposition Disposition,
    SanctuaryLocalConversationWitnessStorePosture StoragePosture,
    IReadOnlyList<SanctuaryLocalConversationWitnessCapability> DefaultDeniedCapabilities,
    IReadOnlyList<SanctuaryLocalConversationWitnessRefusalReason> RefusalReasons,
    string RetentionPostureSummary,
    string ConsentPostureSummary,
    string NonFuelSummary,
    IReadOnlyList<string> WitnessRefs);

public sealed record SanctuaryLocalConversationWitnessStoreReceipt(
    string ReceiptHandle,
    SanctuaryLocalConversationWitnessStoreDisposition Disposition,
    string Summary,
    IReadOnlyList<string> WitnessRefs,
    DateTimeOffset TimestampUtc);
