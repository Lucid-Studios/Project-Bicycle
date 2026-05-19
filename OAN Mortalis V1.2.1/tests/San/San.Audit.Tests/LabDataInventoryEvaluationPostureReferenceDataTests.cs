using San.Common;
using Xunit;

namespace San.Audit.Tests;

public sealed class LabDataInventoryEvaluationPostureReferenceDataTests
{
    [Fact]
    public void Passive_Contracts_Expose_Evaluation_Dispositions_And_Refusal_Reasons()
    {
        Assert.Contains(LabDataInventoryEvaluationDisposition.ReadableAsInventoryOnly, Enum.GetValues<LabDataInventoryEvaluationDisposition>());
        Assert.Contains(LabDataInventoryEvaluationDisposition.HeldForEvaluationReview, Enum.GetValues<LabDataInventoryEvaluationDisposition>());
        Assert.Contains(LabDataInventoryEvaluationDisposition.RefusedAsIngestibleOrActiveUse, Enum.GetValues<LabDataInventoryEvaluationDisposition>());

        Assert.Contains(LabDataInventoryEvaluationRefusalReason.MissingInventoryItem, Enum.GetValues<LabDataInventoryEvaluationRefusalReason>());
        Assert.Contains(LabDataInventoryEvaluationRefusalReason.MissingLogicalSourceLabel, Enum.GetValues<LabDataInventoryEvaluationRefusalReason>());
        Assert.Contains(LabDataInventoryEvaluationRefusalReason.MissingOwnerOrStewardPosture, Enum.GetValues<LabDataInventoryEvaluationRefusalReason>());
        Assert.Contains(LabDataInventoryEvaluationRefusalReason.MissingAuthorityToInventoryPosture, Enum.GetValues<LabDataInventoryEvaluationRefusalReason>());
        Assert.Contains(LabDataInventoryEvaluationRefusalReason.MissingConsentRequirement, Enum.GetValues<LabDataInventoryEvaluationRefusalReason>());
        Assert.Contains(LabDataInventoryEvaluationRefusalReason.MissingAllowedOrForbiddenUseScope, Enum.GetValues<LabDataInventoryEvaluationRefusalReason>());
        Assert.Contains(LabDataInventoryEvaluationRefusalReason.MissingRetentionOrDeletionPosture, Enum.GetValues<LabDataInventoryEvaluationRefusalReason>());
        Assert.Contains(LabDataInventoryEvaluationRefusalReason.MissingVisibilityPosture, Enum.GetValues<LabDataInventoryEvaluationRefusalReason>());
        Assert.Contains(LabDataInventoryEvaluationRefusalReason.SpecialCasePostureMissing, Enum.GetValues<LabDataInventoryEvaluationRefusalReason>());
        Assert.Contains(LabDataInventoryEvaluationRefusalReason.IngestionOverclaimed, Enum.GetValues<LabDataInventoryEvaluationRefusalReason>());
        Assert.Contains(LabDataInventoryEvaluationRefusalReason.RawContentValidationOverclaimed, Enum.GetValues<LabDataInventoryEvaluationRefusalReason>());
        Assert.Contains(LabDataInventoryEvaluationRefusalReason.ConsentOverclaimed, Enum.GetValues<LabDataInventoryEvaluationRefusalReason>());
        Assert.Contains(LabDataInventoryEvaluationRefusalReason.ResearchUseOverclaimed, Enum.GetValues<LabDataInventoryEvaluationRefusalReason>());
        Assert.Contains(LabDataInventoryEvaluationRefusalReason.TrainingOverclaimed, Enum.GetValues<LabDataInventoryEvaluationRefusalReason>());
        Assert.Contains(LabDataInventoryEvaluationRefusalReason.ProviderVisibilityOverclaimed, Enum.GetValues<LabDataInventoryEvaluationRefusalReason>());
        Assert.Contains(LabDataInventoryEvaluationRefusalReason.ModelContextOverclaimed, Enum.GetValues<LabDataInventoryEvaluationRefusalReason>());
        Assert.Contains(LabDataInventoryEvaluationRefusalReason.RuntimeOrRtmeOverclaimed, Enum.GetValues<LabDataInventoryEvaluationRefusalReason>());
    }

    [Fact]
    public void Canonical_Reference_Data_Includes_Readable_Held_And_Refused_Evaluations()
    {
        Assert.Contains(LabDataInventoryEvaluationPostureReferenceData.CanonicalRecords, record => record.Disposition == LabDataInventoryEvaluationDisposition.ReadableAsInventoryOnly);
        Assert.Contains(LabDataInventoryEvaluationPostureReferenceData.CanonicalRecords, record => record.Disposition == LabDataInventoryEvaluationDisposition.HeldForEvaluationReview);
        Assert.Contains(LabDataInventoryEvaluationPostureReferenceData.CanonicalRecords, record => record.Disposition == LabDataInventoryEvaluationDisposition.RefusedAsIngestibleOrActiveUse);
    }

    [Fact]
    public void Readable_Evaluation_Reads_Inventory_Only_And_Denies_All_Active_Capabilities()
    {
        var record = LabDataInventoryEvaluationPostureReferenceData.ReadableInventoryOnlyEvaluation;

        Assert.Equal(LabDataInventoryEvaluationDisposition.ReadableAsInventoryOnly, record.Disposition);
        Assert.Equal(LabDataInventorySchemaReferenceData.CompanyDataInventoryItem.InventoryItemId, record.SourceInventoryItemRef);
        AssertDefaultDenied(record);
        Assert.Contains(LabDataInventoryEvaluationRefusalReason.None, record.RefusalReasons);
        Assert.Contains("represented enough to read inventory posture only", record.CompletenessPosture, StringComparison.Ordinal);
        Assert.Contains("consistency is not research approval", record.ConsistencyPosture, StringComparison.Ordinal);
        Assert.Contains("grant no use", record.ScopePosture, StringComparison.Ordinal);
        Assert.Contains("no consent is collected", record.ConsentRequirementReadout, StringComparison.Ordinal);
        Assert.Contains("no retention is activated", record.RetentionDeletionReadout, StringComparison.Ordinal);
        Assert.Contains("provider visibility and model context remain denied", record.VisibilityReadout, StringComparison.Ordinal);
        Assert.Contains("no handling permission is granted", record.SpecialCaseReadout, StringComparison.Ordinal);
        Assert.Contains("denies ingestion, raw-content exposure, consent collection, research authorization, training eligibility, provider visibility, model-context export, surveillance or profiling, IP transfer, Special Case handling permission, runtime activation, and RTME movement", record.NonAuthoritySummary, StringComparison.Ordinal);
    }

    [Fact]
    public void Held_Evaluation_Keeps_Review_Open_And_Preserves_Denied_Capabilities()
    {
        var record = LabDataInventoryEvaluationPostureReferenceData.HeldForMissingReviewPosture;

        Assert.Equal(LabDataInventoryEvaluationDisposition.HeldForEvaluationReview, record.Disposition);
        Assert.Equal(LabDataInventorySchemaReferenceData.HeldForInventoryReview.InventoryItemId, record.SourceInventoryItemRef);
        AssertDefaultDenied(record);
        Assert.Contains(LabDataInventoryEvaluationRefusalReason.MissingRetentionOrDeletionPosture, record.RefusalReasons);
        Assert.Contains(LabDataInventoryEvaluationRefusalReason.MissingVisibilityPosture, record.RefusalReasons);
        Assert.Contains(LabDataInventoryEvaluationRefusalReason.SpecialCasePostureMissing, record.RefusalReasons);
        Assert.Contains("every denied capability remains denied", record.NonAuthoritySummary, StringComparison.Ordinal);
    }

    [Fact]
    public void Refused_Evaluation_Catches_Missing_Posture_And_Active_Use_Overclaims()
    {
        var record = LabDataInventoryEvaluationPostureReferenceData.RefusedIngestionOrUseOverclaim;

        Assert.Equal(LabDataInventoryEvaluationDisposition.RefusedAsIngestibleOrActiveUse, record.Disposition);
        Assert.Equal(LabDataInventorySchemaReferenceData.RefusedAsIngestibleOrActiveUse.InventoryItemId, record.SourceInventoryItemRef);
        AssertDefaultDenied(record);
        Assert.Contains(LabDataInventoryEvaluationRefusalReason.MissingInventoryItem, record.RefusalReasons);
        Assert.Contains(LabDataInventoryEvaluationRefusalReason.MissingLogicalSourceLabel, record.RefusalReasons);
        Assert.Contains(LabDataInventoryEvaluationRefusalReason.MissingOwnerOrStewardPosture, record.RefusalReasons);
        Assert.Contains(LabDataInventoryEvaluationRefusalReason.MissingAuthorityToInventoryPosture, record.RefusalReasons);
        Assert.Contains(LabDataInventoryEvaluationRefusalReason.MissingConsentRequirement, record.RefusalReasons);
        Assert.Contains(LabDataInventoryEvaluationRefusalReason.MissingAllowedOrForbiddenUseScope, record.RefusalReasons);
        Assert.Contains(LabDataInventoryEvaluationRefusalReason.IngestionOverclaimed, record.RefusalReasons);
        Assert.Contains(LabDataInventoryEvaluationRefusalReason.RawContentValidationOverclaimed, record.RefusalReasons);
        Assert.Contains(LabDataInventoryEvaluationRefusalReason.ConsentOverclaimed, record.RefusalReasons);
        Assert.Contains(LabDataInventoryEvaluationRefusalReason.ResearchUseOverclaimed, record.RefusalReasons);
        Assert.Contains(LabDataInventoryEvaluationRefusalReason.TrainingOverclaimed, record.RefusalReasons);
        Assert.Contains(LabDataInventoryEvaluationRefusalReason.ProviderVisibilityOverclaimed, record.RefusalReasons);
        Assert.Contains(LabDataInventoryEvaluationRefusalReason.ModelContextOverclaimed, record.RefusalReasons);
        Assert.Contains(LabDataInventoryEvaluationRefusalReason.RuntimeOrRtmeOverclaimed, record.RefusalReasons);
        Assert.Contains("overclaims of ingestion, raw content validation, consent, research use, training, provider visibility, model context, runtime activation, or RTME movement", record.NonAuthoritySummary, StringComparison.Ordinal);
    }

    [Fact]
    public void Evaluation_Posture_Receipts_Carry_Readout_Dispositions_Only()
    {
        Assert.Equal(LabDataInventoryEvaluationDisposition.ReadableAsInventoryOnly, LabDataInventoryEvaluationPostureReferenceData.ReadableReceipt.Disposition);
        Assert.Equal(LabDataInventoryEvaluationDisposition.HeldForEvaluationReview, LabDataInventoryEvaluationPostureReferenceData.HeldReceipt.Disposition);
        Assert.Equal(LabDataInventoryEvaluationDisposition.RefusedAsIngestibleOrActiveUse, LabDataInventoryEvaluationPostureReferenceData.RefusedReceipt.Disposition);
        Assert.Contains("readable-inventory-only", LabDataInventoryEvaluationPostureReferenceData.ReadableReceipt.ReceiptHandle, StringComparison.Ordinal);
        Assert.Contains("held-for-missing-review-posture", LabDataInventoryEvaluationPostureReferenceData.HeldReceipt.ReceiptHandle, StringComparison.Ordinal);
        Assert.Contains("refused-ingestion-or-use-overclaim", LabDataInventoryEvaluationPostureReferenceData.RefusedReceipt.ReceiptHandle, StringComparison.Ordinal);
    }

    [Fact]
    public void Evaluation_Posture_Types_Do_Not_Expose_Service_Evaluator_Ingestion_Runtime_Consent_Or_Rtme_Owners()
    {
        var evaluationTypes = typeof(LabDataInventoryEvaluationRecord)
            .Assembly
            .GetTypes()
            .Where(static type =>
                type.Namespace == "San.Common" &&
                type.Name.Contains("LabDataInventoryEvaluation", StringComparison.Ordinal))
            .Select(static type => type.Name)
            .ToArray();

        Assert.DoesNotContain(evaluationTypes, name => name.Contains("Service", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(evaluationTypes, name => name.Contains("Evaluator", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(evaluationTypes, name => name.Contains("Ingestion", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(evaluationTypes, name => name.Contains("Loader", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(evaluationTypes, name => name.Contains("ConsentEngine", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(evaluationTypes, name => name.Contains("RuntimeOwner", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(evaluationTypes, name => name.Contains("RtmeRunner", StringComparison.OrdinalIgnoreCase));
    }

    private static void AssertDefaultDenied(LabDataInventoryEvaluationRecord record)
    {
        var allCapabilities = Enum.GetValues<LabDataInventoryDeniedCapability>();

        Assert.Equal(allCapabilities.Length, record.DeniedCapabilities.Count);

        foreach (var capability in allCapabilities)
        {
            Assert.Contains(capability, record.DeniedCapabilities);
        }
    }
}
