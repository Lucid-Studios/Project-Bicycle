namespace San.Common;

public enum CmeSeedHarnessThoughtFieldSnapshotDisposition
{
    FrozenForCodeFormation = 0,
    HeldForHarnessBuild = 1,
    RefusedAsActivationOverclaim = 2
}

public enum CmeSeedHarnessUiFieldKind
{
    OperatorPrompt = 0,
    SeedPostureSelector = 1,
    InventoryEvaluationRefs = 2,
    ResponseMode = 3,
    RefusalHoldReadoutLane = 4
}

public enum CmeSeedHarnessDeniedCapability
{
    RawLabDataIngestion = 0,
    ConsentCreation = 1,
    ModelTraining = 2,
    ResearchUse = 3,
    ProviderVisibility = 4,
    ModelContextExport = 5,
    SliLispExecution = 6,
    RtmeMovement = 7,
    PrimeCrypticMutation = 8,
    GoverningCmeActivation = 9,
    SanctuaryActualFormation = 10,
    RuntimeAuthority = 11
}

public enum CmeSeedHarnessResponseLaneDisposition
{
    SeededReadoutOnly = 0,
    HeldForHumanReview = 1,
    RefusedForAuthorityOverclaim = 2
}

public enum CmeSeedHarnessRefusalReason
{
    None = 0,
    MissingThoughtFieldSnapshot = 1,
    MissingInventoryEvaluationPosture = 2,
    RawDataIngestionOverclaimed = 3,
    ConsentOverclaimed = 4,
    LlmAuthorityOverclaimed = 5,
    SliLispOverclaimed = 6,
    RtmeOverclaimed = 7,
    PrimeCrypticMutationOverclaimed = 8,
    GoverningCmeActivationOverclaimed = 9,
    SanctuaryActualOverclaimed = 10,
    RuntimeAuthorityOverclaimed = 11
}

public sealed record CmeSeedHarnessUiTemplateField(
    CmeSeedHarnessUiFieldKind Kind,
    string Label,
    string Summary,
    bool RequiredForTemplate,
    string NonCollectionPosture);

public sealed record CmeSeedHarnessThoughtFieldSnapshotRecord(
    CmeSeedHarnessThoughtFieldSnapshotDisposition Disposition,
    string SourceInventoryEvaluationReceiptRef,
    IReadOnlyList<string> ThoughtFieldLadder,
    IReadOnlyList<CmeSeedHarnessUiTemplateField> UiTemplateFields,
    IReadOnlyList<CmeSeedHarnessDeniedCapability> DeniedCapabilities,
    IReadOnlyList<CmeSeedHarnessRefusalReason> RefusalReasons,
    string NonAuthoritySummary,
    IReadOnlyList<string> WitnessRefs);

public sealed record CmeSeedHarnessResponseLaneRecord(
    string SourceThoughtFieldSnapshotRef,
    CmeSeedHarnessResponseLaneDisposition Disposition,
    string ResponseMode,
    string InputPostureSummary,
    string OutputPostureSummary,
    IReadOnlyList<CmeSeedHarnessDeniedCapability> DeniedCapabilities,
    IReadOnlyList<CmeSeedHarnessRefusalReason> RefusalReasons,
    string NonAuthoritySummary,
    IReadOnlyList<string> WitnessRefs);

public sealed record CmeSeedHarnessThoughtFieldSnapshotReceipt(
    string ReceiptHandle,
    CmeSeedHarnessThoughtFieldSnapshotDisposition Disposition,
    string Summary,
    IReadOnlyList<string> WitnessRefs,
    DateTimeOffset TimestampUtc);
