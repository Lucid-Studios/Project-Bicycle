namespace San.Common;

public static class LabDataInventorySchemaReferenceData
{
    public static IReadOnlyList<LabDataInventoryDeniedCapability> DefaultDeniedCapabilities { get; } =
        Enum.GetValues<LabDataInventoryDeniedCapability>();

    public static LabDataInventoryItemRecord CompanyDataInventoryItem { get; } = Represented(
        inventoryItemId: "lab-data-inventory-item:company-metadata",
        dataClass: LabDataInventoryClass.CompanyData,
        logicalSourceLabel: "logical-source-label:lab-data/company-governance-metadata",
        ownerOrStewardPosture: "company-steward-represented",
        authorityToInventoryPosture: "inventory-authority-represented-only",
        sensitivityClass: "private-company-governance",
        consentRequirement: "later-disclosure-and-authority-review-required",
        allowedUseScope: "metadata-only inventory posture",
        forbiddenUseScope: "public disclosure, ingestion, research use, training, model context, runtime, and RTME",
        retentionPosture: "retention-posture-requires-later-review",
        deletionOrRevocationPosture: "deletion-or-revocation-posture-requires-later-review",
        visibilityPosture: "local-build-metadata-only",
        researchSeparationPosture: "not-research-authorized",
        specialCasePosture: "not-special-case",
        ipAssetPosture: "no-ip-transfer");

    public static LabDataInventoryItemRecord PersonalOperatorDataInventoryItem { get; } = Represented(
        inventoryItemId: "lab-data-inventory-item:personal-operator-metadata",
        dataClass: LabDataInventoryClass.PersonalOperatorData,
        logicalSourceLabel: "logical-source-label:lab-data/personal-operator-continuity-metadata",
        ownerOrStewardPosture: "operator-steward-represented",
        authorityToInventoryPosture: "inventory-authority-represented-only",
        sensitivityClass: "personal-continuity",
        consentRequirement: "explicit-consent-required-before-any-use",
        allowedUseScope: "metadata-only inventory posture",
        forbiddenUseScope: "research consent, ingestion, training, profiling, model context, runtime, and RTME",
        retentionPosture: "retention-posture-requires-consent-review",
        deletionOrRevocationPosture: "revocation-posture-requires-later-review",
        visibilityPosture: "local-build-metadata-only",
        researchSeparationPosture: "personal-data-is-not-research-consent",
        specialCasePosture: "screen-for-special-case-before-handling",
        ipAssetPosture: "no-ip-transfer");

    public static LabDataInventoryItemRecord NonprofitSocietyDataInventoryItem { get; } = Represented(
        inventoryItemId: "lab-data-inventory-item:nonprofit-society-metadata",
        dataClass: LabDataInventoryClass.NonprofitSocietyData,
        logicalSourceLabel: "logical-source-label:lab-data/nonprofit-society-governance-metadata",
        ownerOrStewardPosture: "society-steward-represented",
        authorityToInventoryPosture: "inventory-authority-represented-only",
        sensitivityClass: "governance-and-public-benefit-planning",
        consentRequirement: "entity-authority-and-review-required-before-any-use",
        allowedUseScope: "metadata-only inventory posture",
        forbiddenUseScope: "public-benefit authority, ingestion, research use, training, model context, runtime, and RTME",
        retentionPosture: "retention-posture-requires-entity-review",
        deletionOrRevocationPosture: "deletion-or-revocation-posture-requires-entity-review",
        visibilityPosture: "local-build-metadata-only",
        researchSeparationPosture: "not-research-authorized",
        specialCasePosture: "not-special-case",
        ipAssetPosture: "no-ip-transfer");

    public static LabDataInventoryItemRecord IpAssetDataInventoryItem { get; } = Represented(
        inventoryItemId: "lab-data-inventory-item:ip-asset-metadata",
        dataClass: LabDataInventoryClass.IpAssetData,
        logicalSourceLabel: "logical-source-label:lab-data/ip-asset-posture-metadata",
        ownerOrStewardPosture: "asset-steward-represented",
        authorityToInventoryPosture: "inventory-authority-represented-only",
        sensitivityClass: "ip-asset-posture",
        consentRequirement: "scope-and-authority-required-before-any-use",
        allowedUseScope: "metadata-only inventory posture",
        forbiddenUseScope: "IP transfer, inheritance, creation use, ingestion, training, model context, runtime, and RTME",
        retentionPosture: "retention-posture-requires-ip-scope-review",
        deletionOrRevocationPosture: "revocation-posture-requires-ip-scope-review",
        visibilityPosture: "local-build-metadata-only",
        researchSeparationPosture: "not-research-authorized",
        specialCasePosture: "not-special-case",
        ipAssetPosture: "ip-posture-represented-no-transfer");

    public static LabDataInventoryItemRecord ConversationWitnessDataInventoryItem { get; } = Represented(
        inventoryItemId: "lab-data-inventory-item:conversation-witness-metadata",
        dataClass: LabDataInventoryClass.ConversationWitnessData,
        logicalSourceLabel: "logical-source-label:lab-data/conversation-witness-metadata",
        ownerOrStewardPosture: "local-witness-steward-represented",
        authorityToInventoryPosture: "inventory-authority-represented-only",
        sensitivityClass: "local-conversation-witness",
        consentRequirement: "local-retention-and-disclosure-required-before-any-use",
        allowedUseScope: "metadata-only inventory posture",
        forbiddenUseScope: "ingestion, model memory, profile, research use, training, provider visibility, model context, runtime, and RTME",
        retentionPosture: "local-retention-posture-requires-later-review",
        deletionOrRevocationPosture: "revocation-posture-requires-witness-store-review",
        visibilityPosture: "local-build-metadata-only",
        researchSeparationPosture: "not-research-authorized",
        specialCasePosture: "quarantine-if-special-case",
        ipAssetPosture: "no-ip-transfer");

    public static LabDataInventoryItemRecord OperationalTelemetryDataInventoryItem { get; } = Represented(
        inventoryItemId: "lab-data-inventory-item:operational-telemetry-metadata",
        dataClass: LabDataInventoryClass.OperationalTelemetryData,
        logicalSourceLabel: "logical-source-label:lab-data/operational-telemetry-metadata",
        ownerOrStewardPosture: "telemetry-steward-represented",
        authorityToInventoryPosture: "inventory-authority-represented-only",
        sensitivityClass: "governed-operational-telemetry",
        consentRequirement: "disclosure-and-scope-required-before-any-use",
        allowedUseScope: "metadata-only inventory posture",
        forbiddenUseScope: "ingestion, surveillance, profiling, research use, training, provider visibility, model context, runtime, and RTME",
        retentionPosture: "retention-posture-requires-telemetry-scope-review",
        deletionOrRevocationPosture: "revocation-posture-requires-telemetry-scope-review",
        visibilityPosture: "local-build-metadata-only",
        researchSeparationPosture: "not-research-authorized",
        specialCasePosture: "not-special-case",
        ipAssetPosture: "no-ip-transfer");

    public static LabDataInventoryItemRecord SpecialCaseSensitiveDataInventoryItem { get; } = Represented(
        inventoryItemId: "lab-data-inventory-item:special-case-sensitive-metadata",
        dataClass: LabDataInventoryClass.SpecialCaseSensitiveData,
        logicalSourceLabel: "logical-source-label:lab-data/special-case-sensitive-held-metadata",
        ownerOrStewardPosture: "special-case-steward-represented",
        authorityToInventoryPosture: "inventory-authority-represented-only",
        sensitivityClass: "special-case-sensitive-held",
        consentRequirement: "special-case-review-required-before-any-handling",
        allowedUseScope: "metadata-only inventory posture",
        forbiddenUseScope: "handling permission, ingestion, research use, training, model context, runtime, and RTME",
        retentionPosture: "quarantined-retention-posture-requires-review",
        deletionOrRevocationPosture: "revocation-posture-requires-special-case-review",
        visibilityPosture: "local-build-metadata-only",
        researchSeparationPosture: "not-research-authorized",
        specialCasePosture: "quarantined-no-handling-permission",
        ipAssetPosture: "no-ip-transfer");

    public static IReadOnlyList<LabDataInventoryItemRecord> FirstInventoryClassItems { get; } = new[]
    {
        CompanyDataInventoryItem,
        PersonalOperatorDataInventoryItem,
        NonprofitSocietyDataInventoryItem,
        IpAssetDataInventoryItem,
        ConversationWitnessDataInventoryItem,
        OperationalTelemetryDataInventoryItem,
        SpecialCaseSensitiveDataInventoryItem
    };

    public static LabDataInventoryItemRecord HeldForInventoryReview { get; } = ConversationWitnessDataInventoryItem with
    {
        InventoryItemId = "lab-data-inventory-item:held-for-inventory-review",
        Disposition = LabDataInventoryDisposition.HeldForInventoryReview,
        LogicalSourceLabel = "logical-source-label:lab-data/held-inventory-review-metadata",
        RefusalReasons = new[] { LabDataInventoryRefusalReason.None },
        RetentionPosture = "retention-posture-held-for-review",
        DeletionOrRevocationPosture = "deletion-or-revocation-posture-held-for-review",
        SpecialCasePosture = "held-for-special-case-screening",
        NonAuthoritySummary = "Held Lab data inventory keeps inventory review open while denying ingestion, raw-content exposure, consent collection, research authorization, training eligibility, provider visibility, model-context export, surveillance or profiling, IP transfer, Special Case handling permission, runtime activation, and RTME movement.",
        WitnessRefs = new[]
        {
            "lab-data-inventory-ref:held-for-inventory-review",
            LabMixedDataPreAwakeningProofRunReferenceData.HeldReceipt.ReceiptHandle
        }
    };

    public static LabDataInventoryItemRecord RefusedAsIngestibleOrActiveUse { get; } = new(
        InventoryItemId: "lab-data-inventory-item:refused-ingestible-or-active-use",
        Disposition: LabDataInventoryDisposition.RefusedAsIngestibleOrActiveUse,
        DataClass: LabDataInventoryClass.SpecialCaseSensitiveData,
        LogicalSourceLabel: "logical-source-label:lab-data/refused-active-use-overclaim-metadata",
        OwnerOrStewardPosture: "refused",
        AuthorityToInventoryPosture: "refused",
        SensitivityClass: "refused",
        ConsentRequirement: "refused",
        AllowedUseScope: "none",
        ForbiddenUseScope: "ingestion, raw exposure, consent capture, research authorization, training, provider visibility, model context, surveillance, profiling, IP transfer, Special Case handling, runtime, and RTME",
        RetentionPosture: "refused",
        DeletionOrRevocationPosture: "refused",
        VisibilityPosture: "refused",
        ResearchSeparationPosture: "refused",
        SpecialCasePosture: "refused",
        IpAssetPosture: "refused",
        ReceiptRefs: new[] { LabMixedDataPreAwakeningProofRunReferenceData.RefusedMisuseReceipt.ReceiptHandle },
        DeniedCapabilities: DefaultDeniedCapabilities,
        RefusalReasons: new[]
        {
            LabDataInventoryRefusalReason.IngestionOverclaimed,
            LabDataInventoryRefusalReason.RawContentExposureOverclaimed,
            LabDataInventoryRefusalReason.ConsentCollectionOverclaimed,
            LabDataInventoryRefusalReason.ResearchAuthorizationOverclaimed,
            LabDataInventoryRefusalReason.TrainingEligibilityOverclaimed,
            LabDataInventoryRefusalReason.ProviderVisibilityOverclaimed,
            LabDataInventoryRefusalReason.ModelContextExportOverclaimed,
            LabDataInventoryRefusalReason.SurveillanceOrProfilingOverclaimed,
            LabDataInventoryRefusalReason.IpTransferOverclaimed,
            LabDataInventoryRefusalReason.SpecialCaseHandlingPermissionOverclaimed,
            LabDataInventoryRefusalReason.RuntimeOrRtmeOverclaimed
        },
        NonAuthoritySummary: "Refused Lab data inventory catches attempts to treat inventory as ingestion, raw exposure, consent collection, research authorization, training eligibility, provider visibility, model-context export, surveillance or profiling, IP transfer, Special Case handling permission, runtime activation, or RTME movement.",
        WitnessRefs: new[]
        {
            "lab-data-inventory-ref:refused-ingestible-or-active-use",
            LabMixedDataPreAwakeningProofRunReferenceData.RefusedMisuseReceipt.ReceiptHandle
        });

    public static IReadOnlyList<LabDataInventoryItemRecord> CanonicalRecords { get; } = new[]
    {
        CompanyDataInventoryItem,
        PersonalOperatorDataInventoryItem,
        NonprofitSocietyDataInventoryItem,
        IpAssetDataInventoryItem,
        ConversationWitnessDataInventoryItem,
        OperationalTelemetryDataInventoryItem,
        SpecialCaseSensitiveDataInventoryItem,
        HeldForInventoryReview,
        RefusedAsIngestibleOrActiveUse
    };

    public static LabDataInventorySchemaReceipt RepresentedSchemaReceipt { get; } = Receipt(
        "lab-data-inventory-schema-receipt:represented",
        LabDataInventoryDisposition.Represented,
        "Represented Lab data inventory schema classifies metadata-only inventory posture and denies ingestion, raw exposure, consent collection, research authorization, training, runtime activation, and RTME movement.",
        FirstInventoryClassItems.SelectMany(static item => item.WitnessRefs).Distinct().ToArray());

    public static LabDataInventorySchemaReceipt HeldSchemaReceipt { get; } = Receipt(
        "lab-data-inventory-schema-receipt:held-for-inventory-review",
        LabDataInventoryDisposition.HeldForInventoryReview,
        HeldForInventoryReview.NonAuthoritySummary,
        HeldForInventoryReview.WitnessRefs);

    public static LabDataInventorySchemaReceipt RefusedSchemaReceipt { get; } = Receipt(
        "lab-data-inventory-schema-receipt:refused-ingestible-or-active-use",
        LabDataInventoryDisposition.RefusedAsIngestibleOrActiveUse,
        RefusedAsIngestibleOrActiveUse.NonAuthoritySummary,
        RefusedAsIngestibleOrActiveUse.WitnessRefs);

    private static LabDataInventoryItemRecord Represented(
        string inventoryItemId,
        LabDataInventoryClass dataClass,
        string logicalSourceLabel,
        string ownerOrStewardPosture,
        string authorityToInventoryPosture,
        string sensitivityClass,
        string consentRequirement,
        string allowedUseScope,
        string forbiddenUseScope,
        string retentionPosture,
        string deletionOrRevocationPosture,
        string visibilityPosture,
        string researchSeparationPosture,
        string specialCasePosture,
        string ipAssetPosture)
    {
        return new(
            InventoryItemId: inventoryItemId,
            Disposition: LabDataInventoryDisposition.Represented,
            DataClass: dataClass,
            LogicalSourceLabel: logicalSourceLabel,
            OwnerOrStewardPosture: ownerOrStewardPosture,
            AuthorityToInventoryPosture: authorityToInventoryPosture,
            SensitivityClass: sensitivityClass,
            ConsentRequirement: consentRequirement,
            AllowedUseScope: allowedUseScope,
            ForbiddenUseScope: forbiddenUseScope,
            RetentionPosture: retentionPosture,
            DeletionOrRevocationPosture: deletionOrRevocationPosture,
            VisibilityPosture: visibilityPosture,
            ResearchSeparationPosture: researchSeparationPosture,
            SpecialCasePosture: specialCasePosture,
            IpAssetPosture: ipAssetPosture,
            ReceiptRefs: new[] { LabMixedDataPreAwakeningProofRunReferenceData.HeldReceipt.ReceiptHandle },
            DeniedCapabilities: DefaultDeniedCapabilities,
            RefusalReasons: new[] { LabDataInventoryRefusalReason.None },
            NonAuthoritySummary: "Represented Lab data inventory is metadata-only posture. It denies ingestion, raw-content exposure, consent collection, research authorization, training eligibility, provider visibility, model-context export, surveillance or profiling, IP transfer, Special Case handling permission, runtime activation, and RTME movement.",
            WitnessRefs: new[]
            {
                inventoryItemId,
                logicalSourceLabel,
                LabMixedDataPreAwakeningProofRunReferenceData.HeldReceipt.ReceiptHandle
            });
    }

    private static LabDataInventorySchemaReceipt Receipt(
        string receiptHandle,
        LabDataInventoryDisposition disposition,
        string summary,
        IReadOnlyList<string> witnessRefs)
    {
        return new(
            ReceiptHandle: receiptHandle,
            Disposition: disposition,
            Summary: summary,
            WitnessRefs: witnessRefs,
            TimestampUtc: DateTimeOffset.UnixEpoch);
    }
}
