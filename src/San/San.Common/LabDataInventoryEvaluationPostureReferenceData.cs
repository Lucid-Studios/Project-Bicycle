namespace San.Common;

public static class LabDataInventoryEvaluationPostureReferenceData
{
    public static IReadOnlyList<LabDataInventoryDeniedCapability> DefaultDeniedCapabilities { get; } =
        LabDataInventorySchemaReferenceData.DefaultDeniedCapabilities;

    public static LabDataInventoryEvaluationRecord ReadableInventoryOnlyEvaluation { get; } = new(
        SourceInventoryItemRef: LabDataInventorySchemaReferenceData.CompanyDataInventoryItem.InventoryItemId,
        Disposition: LabDataInventoryEvaluationDisposition.ReadableAsInventoryOnly,
        CompletenessPosture: "Inventory metadata fields are represented enough to read inventory posture only.",
        ConsistencyPosture: "Inventory posture is internally consistent as metadata-only readout; consistency is not research approval.",
        ScopePosture: "Allowed and forbidden use scopes are readable only and grant no use.",
        ConsentRequirementReadout: "Consent requirement is read only; no consent is collected.",
        RetentionDeletionReadout: "Retention and deletion posture is read only; no retention is activated.",
        VisibilityReadout: "Visibility posture is read only; provider visibility and model context remain denied.",
        SpecialCaseReadout: "Special Case posture is read only; no handling permission is granted.",
        DeniedCapabilities: DefaultDeniedCapabilities,
        RefusalReasons: new[] { LabDataInventoryEvaluationRefusalReason.None },
        NonAuthoritySummary: "Readable inventory-only evaluation reads metadata inventory posture for completeness, consistency, scope, consent requirement, retention/deletion, visibility, and Special Case posture. It denies ingestion, raw-content exposure, consent collection, research authorization, training eligibility, provider visibility, model-context export, surveillance or profiling, IP transfer, Special Case handling permission, runtime activation, and RTME movement.",
        WitnessRefs: new[]
        {
            "lab-data-inventory-evaluation-posture-ref:readable-inventory-only",
            LabDataInventorySchemaReferenceData.CompanyDataInventoryItem.InventoryItemId,
            LabDataInventorySchemaReferenceData.RepresentedSchemaReceipt.ReceiptHandle
        });

    public static LabDataInventoryEvaluationRecord HeldForMissingReviewPosture { get; } = new(
        SourceInventoryItemRef: LabDataInventorySchemaReferenceData.HeldForInventoryReview.InventoryItemId,
        Disposition: LabDataInventoryEvaluationDisposition.HeldForEvaluationReview,
        CompletenessPosture: "Inventory completeness remains held for review.",
        ConsistencyPosture: "Inventory consistency remains held for review.",
        ScopePosture: "Inventory scope remains held for review.",
        ConsentRequirementReadout: "Consent requirement readout remains held and does not collect consent.",
        RetentionDeletionReadout: "Retention/deletion readout remains held and does not activate retention.",
        VisibilityReadout: "Visibility readout remains held and creates no provider visibility or model context.",
        SpecialCaseReadout: "Special Case posture remains held and grants no handling permission.",
        DeniedCapabilities: DefaultDeniedCapabilities,
        RefusalReasons: new[]
        {
            LabDataInventoryEvaluationRefusalReason.MissingRetentionOrDeletionPosture,
            LabDataInventoryEvaluationRefusalReason.MissingVisibilityPosture,
            LabDataInventoryEvaluationRefusalReason.SpecialCasePostureMissing
        },
        NonAuthoritySummary: "Held inventory evaluation keeps completeness, consistency, scope, retention, visibility, or Special Case questions under review while every denied capability remains denied.",
        WitnessRefs: new[]
        {
            "lab-data-inventory-evaluation-posture-ref:held-for-missing-review-posture",
            LabDataInventorySchemaReferenceData.HeldForInventoryReview.InventoryItemId,
            LabDataInventorySchemaReferenceData.HeldSchemaReceipt.ReceiptHandle
        });

    public static LabDataInventoryEvaluationRecord RefusedIngestionOrUseOverclaim { get; } = new(
        SourceInventoryItemRef: LabDataInventorySchemaReferenceData.RefusedAsIngestibleOrActiveUse.InventoryItemId,
        Disposition: LabDataInventoryEvaluationDisposition.RefusedAsIngestibleOrActiveUse,
        CompletenessPosture: "refused",
        ConsistencyPosture: "refused",
        ScopePosture: "refused",
        ConsentRequirementReadout: "refused",
        RetentionDeletionReadout: "refused",
        VisibilityReadout: "refused",
        SpecialCaseReadout: "refused",
        DeniedCapabilities: DefaultDeniedCapabilities,
        RefusalReasons: new[]
        {
            LabDataInventoryEvaluationRefusalReason.MissingInventoryItem,
            LabDataInventoryEvaluationRefusalReason.MissingLogicalSourceLabel,
            LabDataInventoryEvaluationRefusalReason.MissingOwnerOrStewardPosture,
            LabDataInventoryEvaluationRefusalReason.MissingAuthorityToInventoryPosture,
            LabDataInventoryEvaluationRefusalReason.MissingConsentRequirement,
            LabDataInventoryEvaluationRefusalReason.MissingAllowedOrForbiddenUseScope,
            LabDataInventoryEvaluationRefusalReason.IngestionOverclaimed,
            LabDataInventoryEvaluationRefusalReason.RawContentValidationOverclaimed,
            LabDataInventoryEvaluationRefusalReason.ConsentOverclaimed,
            LabDataInventoryEvaluationRefusalReason.ResearchUseOverclaimed,
            LabDataInventoryEvaluationRefusalReason.TrainingOverclaimed,
            LabDataInventoryEvaluationRefusalReason.ProviderVisibilityOverclaimed,
            LabDataInventoryEvaluationRefusalReason.ModelContextOverclaimed,
            LabDataInventoryEvaluationRefusalReason.RuntimeOrRtmeOverclaimed
        },
        NonAuthoritySummary: "Refused inventory evaluation catches missing inventory posture and overclaims of ingestion, raw content validation, consent, research use, training, provider visibility, model context, runtime activation, or RTME movement.",
        WitnessRefs: new[]
        {
            "lab-data-inventory-evaluation-posture-ref:refused-ingestion-or-use-overclaim",
            LabDataInventorySchemaReferenceData.RefusedAsIngestibleOrActiveUse.InventoryItemId,
            LabDataInventorySchemaReferenceData.RefusedSchemaReceipt.ReceiptHandle
        });

    public static IReadOnlyList<LabDataInventoryEvaluationRecord> CanonicalRecords { get; } = new[]
    {
        ReadableInventoryOnlyEvaluation,
        HeldForMissingReviewPosture,
        RefusedIngestionOrUseOverclaim
    };

    public static LabDataInventoryEvaluationReceipt ReadableReceipt { get; } = Receipt(
        "lab-data-inventory-evaluation-posture-receipt:readable-inventory-only",
        ReadableInventoryOnlyEvaluation);

    public static LabDataInventoryEvaluationReceipt HeldReceipt { get; } = Receipt(
        "lab-data-inventory-evaluation-posture-receipt:held-for-missing-review-posture",
        HeldForMissingReviewPosture);

    public static LabDataInventoryEvaluationReceipt RefusedReceipt { get; } = Receipt(
        "lab-data-inventory-evaluation-posture-receipt:refused-ingestion-or-use-overclaim",
        RefusedIngestionOrUseOverclaim);

    private static LabDataInventoryEvaluationReceipt Receipt(
        string receiptHandle,
        LabDataInventoryEvaluationRecord record)
    {
        return new(
            ReceiptHandle: receiptHandle,
            Disposition: record.Disposition,
            Summary: record.NonAuthoritySummary,
            WitnessRefs: record.WitnessRefs,
            TimestampUtc: DateTimeOffset.UnixEpoch);
    }
}
