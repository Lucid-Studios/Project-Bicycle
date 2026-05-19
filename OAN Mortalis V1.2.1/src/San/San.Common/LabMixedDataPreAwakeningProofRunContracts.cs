namespace San.Common;

public enum LabMixedDataPreAwakeningProofDisposition
{
    HeldForProof = 0,
    RefusedUntilConsentAndStartupAdmission = 1
}

public enum LabMixedDataManifestDatumKind
{
    PersonalOperator = 0,
    PrivateLabBusiness = 1,
    IpAsset = 2,
    ConversationWitness = 3,
    OperationalTelemetry = 4,
    SpecialCaseSensitiveHeld = 5
}

public enum LabMixedDataPreAwakeningProofStage
{
    LocalLabDataManifest = 0,
    NationalRegionalLocalPredicateContextRefs = 1,
    GovernedUserDataPredicateTemplateMatch = 2,
    PayloadClassificationPosture = 3,
    ConsentDisclosureRetentionRequirementReadout = 4,
    OptInOptOutRevocationPosture = 5,
    SpecialCaseQuarantine = 6,
    LabSeedInheritanceRef = 7,
    PreActivationLegitimacyPosture = 8,
    StartupAttemptEligibilityReadout = 9,
    ActivationHeldOrRefusedByDesign = 10
}

public enum LabMixedDataPreAwakeningProofReceiptKind
{
    DataManifest = 0,
    PredicateContext = 1,
    PayloadClassification = 2,
    ConsentRequirement = 3,
    RetentionOptOut = 4,
    SpecialCaseQuarantine = 5,
    LabSeedInheritance = 6,
    PreActivationLegitimacy = 7,
    StartupAttemptHoldOrRefusal = 8
}

public enum LabMixedDataPreAwakeningDeniedCapability
{
    RawContentExposure = 0,
    ProviderVisibility = 1,
    ConsentCreation = 2,
    ResearchUse = 3,
    Training = 4,
    Profiling = 5,
    Surveillance = 6,
    ModelContext = 7,
    RetentionWidening = 8,
    SpecialCaseWidening = 9,
    Governance = 10,
    RtmeMovement = 11,
    SliLispExecution = 12,
    PrimeCrypticMutation = 13,
    RuntimeControl = 14
}

public enum LabMixedDataPreAwakeningProofRefusalReason
{
    None = 0,
    MissingLocalManifestMetadata = 1,
    MissingPredicateContextRefs = 2,
    MissingTemplateMatch = 3,
    MissingPayloadClassification = 4,
    MissingConsentRequirementReadout = 5,
    MissingRetentionOptOutReadout = 6,
    SpecialCaseNotQuarantined = 7,
    MissingLabSeedInheritanceRef = 8,
    MissingPreActivationLegitimacyPosture = 9,
    StartupAttemptNotHeldOrRefused = 10,
    RawContentExposureOverclaimed = 11,
    ProviderVisibilityOverclaimed = 12,
    ConsentCreationOverclaimed = 13,
    ResearchTrainingProfilingSurveillanceOverclaimed = 14,
    ModelContextOverclaimed = 15,
    RetentionOrSpecialCaseWideningOverclaimed = 16,
    RtmeOrSliLispOverclaimed = 17,
    PrimeCrypticMutationOverclaimed = 18,
    GovernanceOrRuntimeOverclaimed = 19
}

public sealed record LabMixedDataManifestEntry(
    LabMixedDataManifestDatumKind Kind,
    string LogicalLocalRef,
    string HashOrRefPosture,
    string Summary,
    string SensitivityPosture,
    IReadOnlyList<string> WitnessRefs);

public sealed record LabMixedDataPreAwakeningProofRunRecord(
    string SourceGoverningPrimeCrypticTemplateRef,
    LabMixedDataPreAwakeningProofDisposition Disposition,
    IReadOnlyList<LabMixedDataManifestEntry> ManifestEntries,
    IReadOnlyList<LabMixedDataPreAwakeningProofStage> ProofStages,
    IReadOnlyList<LabMixedDataPreAwakeningProofReceiptKind> ReceiptKinds,
    IReadOnlyList<LabMixedDataPreAwakeningDeniedCapability> DeniedCapabilities,
    IReadOnlyList<LabMixedDataPreAwakeningProofRefusalReason> RefusalReasons,
    string PredicateContextPosture,
    string PayloadClassificationPosture,
    string ConsentStartupPosture,
    string SpecialCaseQuarantinePosture,
    string ActivationResultPosture,
    string NonMisuseSummary,
    IReadOnlyList<string> WitnessRefs);

public sealed record LabMixedDataPreAwakeningProofRunReceipt(
    string ReceiptHandle,
    LabMixedDataPreAwakeningProofDisposition Disposition,
    string Summary,
    IReadOnlyList<string> WitnessRefs,
    DateTimeOffset TimestampUtc);
