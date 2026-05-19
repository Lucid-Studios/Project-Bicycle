namespace San.Common;

public enum LabDataInventoryEvaluationDisposition
{
    ReadableAsInventoryOnly = 0,
    HeldForEvaluationReview = 1,
    RefusedAsIngestibleOrActiveUse = 2
}

public enum LabDataInventoryEvaluationRefusalReason
{
    None = 0,
    MissingInventoryItem = 1,
    MissingLogicalSourceLabel = 2,
    MissingOwnerOrStewardPosture = 3,
    MissingAuthorityToInventoryPosture = 4,
    MissingConsentRequirement = 5,
    MissingAllowedOrForbiddenUseScope = 6,
    MissingRetentionOrDeletionPosture = 7,
    MissingVisibilityPosture = 8,
    SpecialCasePostureMissing = 9,
    IngestionOverclaimed = 10,
    RawContentValidationOverclaimed = 11,
    ConsentOverclaimed = 12,
    ResearchUseOverclaimed = 13,
    TrainingOverclaimed = 14,
    ProviderVisibilityOverclaimed = 15,
    ModelContextOverclaimed = 16,
    RuntimeOrRtmeOverclaimed = 17
}

public sealed record LabDataInventoryEvaluationRecord(
    string SourceInventoryItemRef,
    LabDataInventoryEvaluationDisposition Disposition,
    string CompletenessPosture,
    string ConsistencyPosture,
    string ScopePosture,
    string ConsentRequirementReadout,
    string RetentionDeletionReadout,
    string VisibilityReadout,
    string SpecialCaseReadout,
    IReadOnlyList<LabDataInventoryDeniedCapability> DeniedCapabilities,
    IReadOnlyList<LabDataInventoryEvaluationRefusalReason> RefusalReasons,
    string NonAuthoritySummary,
    IReadOnlyList<string> WitnessRefs);

public sealed record LabDataInventoryEvaluationReceipt(
    string ReceiptHandle,
    LabDataInventoryEvaluationDisposition Disposition,
    string Summary,
    IReadOnlyList<string> WitnessRefs,
    DateTimeOffset TimestampUtc);
