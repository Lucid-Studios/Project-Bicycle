namespace San.Common;

public enum LabDataInventoryDisposition
{
    Represented = 0,
    HeldForInventoryReview = 1,
    RefusedAsIngestibleOrActiveUse = 2
}

public enum LabDataInventoryClass
{
    CompanyData = 0,
    PersonalOperatorData = 1,
    NonprofitSocietyData = 2,
    IpAssetData = 3,
    ConversationWitnessData = 4,
    OperationalTelemetryData = 5,
    SpecialCaseSensitiveData = 6
}

public enum LabDataInventoryDeniedCapability
{
    Ingestion = 0,
    RawContentExposure = 1,
    ConsentCollection = 2,
    ResearchAuthorization = 3,
    TrainingEligibility = 4,
    ProviderVisibility = 5,
    ModelContextExport = 6,
    SurveillanceProfiling = 7,
    IpTransfer = 8,
    SpecialCaseHandlingPermission = 9,
    RuntimeActivation = 10,
    RtmeMovement = 11
}

public enum LabDataInventoryRefusalReason
{
    None = 0,
    MissingInventoryItemId = 1,
    MissingLogicalSourceLabel = 2,
    MissingOwnerOrStewardPosture = 3,
    MissingAuthorityToInventoryPosture = 4,
    MissingSensitivityClass = 5,
    MissingConsentRequirement = 6,
    MissingUseScope = 7,
    MissingRetentionOrDeletionPosture = 8,
    MissingVisibilityPosture = 9,
    IngestionOverclaimed = 10,
    RawContentExposureOverclaimed = 11,
    ConsentCollectionOverclaimed = 12,
    ResearchAuthorizationOverclaimed = 13,
    TrainingEligibilityOverclaimed = 14,
    ProviderVisibilityOverclaimed = 15,
    ModelContextExportOverclaimed = 16,
    SurveillanceOrProfilingOverclaimed = 17,
    IpTransferOverclaimed = 18,
    SpecialCaseHandlingPermissionOverclaimed = 19,
    RuntimeOrRtmeOverclaimed = 20
}

public sealed record LabDataInventoryItemRecord(
    string InventoryItemId,
    LabDataInventoryDisposition Disposition,
    LabDataInventoryClass DataClass,
    string LogicalSourceLabel,
    string OwnerOrStewardPosture,
    string AuthorityToInventoryPosture,
    string SensitivityClass,
    string ConsentRequirement,
    string AllowedUseScope,
    string ForbiddenUseScope,
    string RetentionPosture,
    string DeletionOrRevocationPosture,
    string VisibilityPosture,
    string ResearchSeparationPosture,
    string SpecialCasePosture,
    string IpAssetPosture,
    IReadOnlyList<string> ReceiptRefs,
    IReadOnlyList<LabDataInventoryDeniedCapability> DeniedCapabilities,
    IReadOnlyList<LabDataInventoryRefusalReason> RefusalReasons,
    string NonAuthoritySummary,
    IReadOnlyList<string> WitnessRefs);

public sealed record LabDataInventorySchemaReceipt(
    string ReceiptHandle,
    LabDataInventoryDisposition Disposition,
    string Summary,
    IReadOnlyList<string> WitnessRefs,
    DateTimeOffset TimestampUtc);
