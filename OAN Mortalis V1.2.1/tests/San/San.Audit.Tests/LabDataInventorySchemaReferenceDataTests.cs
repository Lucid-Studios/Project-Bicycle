using San.Common;
using Xunit;

namespace San.Audit.Tests;

public sealed class LabDataInventorySchemaReferenceDataTests
{
    [Fact]
    public void Passive_Contracts_Expose_Inventory_Dispositions_Classes_And_Denied_Capabilities()
    {
        Assert.Contains(LabDataInventoryDisposition.Represented, Enum.GetValues<LabDataInventoryDisposition>());
        Assert.Contains(LabDataInventoryDisposition.HeldForInventoryReview, Enum.GetValues<LabDataInventoryDisposition>());
        Assert.Contains(LabDataInventoryDisposition.RefusedAsIngestibleOrActiveUse, Enum.GetValues<LabDataInventoryDisposition>());

        Assert.Contains(LabDataInventoryClass.CompanyData, Enum.GetValues<LabDataInventoryClass>());
        Assert.Contains(LabDataInventoryClass.PersonalOperatorData, Enum.GetValues<LabDataInventoryClass>());
        Assert.Contains(LabDataInventoryClass.NonprofitSocietyData, Enum.GetValues<LabDataInventoryClass>());
        Assert.Contains(LabDataInventoryClass.IpAssetData, Enum.GetValues<LabDataInventoryClass>());
        Assert.Contains(LabDataInventoryClass.ConversationWitnessData, Enum.GetValues<LabDataInventoryClass>());
        Assert.Contains(LabDataInventoryClass.OperationalTelemetryData, Enum.GetValues<LabDataInventoryClass>());
        Assert.Contains(LabDataInventoryClass.SpecialCaseSensitiveData, Enum.GetValues<LabDataInventoryClass>());

        Assert.Contains(LabDataInventoryDeniedCapability.Ingestion, Enum.GetValues<LabDataInventoryDeniedCapability>());
        Assert.Contains(LabDataInventoryDeniedCapability.RawContentExposure, Enum.GetValues<LabDataInventoryDeniedCapability>());
        Assert.Contains(LabDataInventoryDeniedCapability.ConsentCollection, Enum.GetValues<LabDataInventoryDeniedCapability>());
        Assert.Contains(LabDataInventoryDeniedCapability.ResearchAuthorization, Enum.GetValues<LabDataInventoryDeniedCapability>());
        Assert.Contains(LabDataInventoryDeniedCapability.TrainingEligibility, Enum.GetValues<LabDataInventoryDeniedCapability>());
        Assert.Contains(LabDataInventoryDeniedCapability.ProviderVisibility, Enum.GetValues<LabDataInventoryDeniedCapability>());
        Assert.Contains(LabDataInventoryDeniedCapability.ModelContextExport, Enum.GetValues<LabDataInventoryDeniedCapability>());
        Assert.Contains(LabDataInventoryDeniedCapability.SurveillanceProfiling, Enum.GetValues<LabDataInventoryDeniedCapability>());
        Assert.Contains(LabDataInventoryDeniedCapability.IpTransfer, Enum.GetValues<LabDataInventoryDeniedCapability>());
        Assert.Contains(LabDataInventoryDeniedCapability.SpecialCaseHandlingPermission, Enum.GetValues<LabDataInventoryDeniedCapability>());
        Assert.Contains(LabDataInventoryDeniedCapability.RuntimeActivation, Enum.GetValues<LabDataInventoryDeniedCapability>());
        Assert.Contains(LabDataInventoryDeniedCapability.RtmeMovement, Enum.GetValues<LabDataInventoryDeniedCapability>());
    }

    [Fact]
    public void Canonical_Reference_Data_Includes_Seven_First_Inventory_Classes_And_Held_Refused_Records()
    {
        var classItems = LabDataInventorySchemaReferenceData.FirstInventoryClassItems;

        Assert.Equal(7, classItems.Count);
        AssertContainsClass(classItems, LabDataInventoryClass.CompanyData);
        AssertContainsClass(classItems, LabDataInventoryClass.PersonalOperatorData);
        AssertContainsClass(classItems, LabDataInventoryClass.NonprofitSocietyData);
        AssertContainsClass(classItems, LabDataInventoryClass.IpAssetData);
        AssertContainsClass(classItems, LabDataInventoryClass.ConversationWitnessData);
        AssertContainsClass(classItems, LabDataInventoryClass.OperationalTelemetryData);
        AssertContainsClass(classItems, LabDataInventoryClass.SpecialCaseSensitiveData);

        Assert.Contains(LabDataInventorySchemaReferenceData.CanonicalRecords, record => record.Disposition == LabDataInventoryDisposition.HeldForInventoryReview);
        Assert.Contains(LabDataInventorySchemaReferenceData.CanonicalRecords, record => record.Disposition == LabDataInventoryDisposition.RefusedAsIngestibleOrActiveUse);
    }

    [Fact]
    public void Canonical_Inventory_Items_Contain_Required_Posture_Fields_And_Logical_Source_Labels_Only()
    {
        foreach (var record in LabDataInventorySchemaReferenceData.FirstInventoryClassItems)
        {
            Assert.Equal(LabDataInventoryDisposition.Represented, record.Disposition);
            AssertNotBlank(record.InventoryItemId);
            AssertNotBlank(record.LogicalSourceLabel);
            Assert.StartsWith("logical-source-label:", record.LogicalSourceLabel, StringComparison.Ordinal);
            AssertNotBlank(record.OwnerOrStewardPosture);
            AssertNotBlank(record.AuthorityToInventoryPosture);
            AssertNotBlank(record.SensitivityClass);
            AssertNotBlank(record.ConsentRequirement);
            AssertNotBlank(record.AllowedUseScope);
            AssertNotBlank(record.ForbiddenUseScope);
            AssertNotBlank(record.RetentionPosture);
            AssertNotBlank(record.DeletionOrRevocationPosture);
            AssertNotBlank(record.VisibilityPosture);
            AssertNotBlank(record.ResearchSeparationPosture);
            AssertNotBlank(record.SpecialCasePosture);
            AssertNotBlank(record.IpAssetPosture);
            Assert.NotEmpty(record.ReceiptRefs);
            Assert.NotEmpty(record.WitnessRefs);
            Assert.DoesNotContain("\\", record.LogicalSourceLabel, StringComparison.Ordinal);
            Assert.DoesNotContain("/", record.LogicalSourceLabel[.."logical-source-label:".Length], StringComparison.Ordinal);
        }
    }

    [Fact]
    public void Represented_Inventory_Is_Metadata_Only_And_Denies_All_Active_Use_Capabilities()
    {
        foreach (var record in LabDataInventorySchemaReferenceData.FirstInventoryClassItems)
        {
            AssertDefaultDenied(record);
            Assert.Contains(LabDataInventoryRefusalReason.None, record.RefusalReasons);
            Assert.Contains("metadata-only", record.NonAuthoritySummary, StringComparison.Ordinal);
            Assert.Contains("denies ingestion, raw-content exposure, consent collection, research authorization, training eligibility, provider visibility, model-context export, surveillance or profiling, IP transfer, Special Case handling permission, runtime activation, and RTME movement", record.NonAuthoritySummary, StringComparison.Ordinal);
            Assert.Contains("metadata-only inventory posture", record.AllowedUseScope, StringComparison.Ordinal);
            Assert.Contains("ingestion", record.ForbiddenUseScope, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("RTME", record.ForbiddenUseScope, StringComparison.Ordinal);
        }
    }

    [Fact]
    public void Inventory_Class_Records_Remain_Non_Public_Non_Consent_Non_Research_Non_Training_Non_Runtime()
    {
        Assert.Contains("public disclosure", LabDataInventorySchemaReferenceData.CompanyDataInventoryItem.ForbiddenUseScope, StringComparison.Ordinal);
        Assert.Contains("research consent", LabDataInventorySchemaReferenceData.PersonalOperatorDataInventoryItem.ForbiddenUseScope, StringComparison.Ordinal);
        Assert.Contains("public-benefit authority", LabDataInventorySchemaReferenceData.NonprofitSocietyDataInventoryItem.ForbiddenUseScope, StringComparison.Ordinal);
        Assert.Contains("IP transfer", LabDataInventorySchemaReferenceData.IpAssetDataInventoryItem.ForbiddenUseScope, StringComparison.Ordinal);
        Assert.Contains("model memory", LabDataInventorySchemaReferenceData.ConversationWitnessDataInventoryItem.ForbiddenUseScope, StringComparison.Ordinal);
        Assert.Contains("surveillance", LabDataInventorySchemaReferenceData.OperationalTelemetryDataInventoryItem.ForbiddenUseScope, StringComparison.Ordinal);
        Assert.Contains("handling permission", LabDataInventorySchemaReferenceData.SpecialCaseSensitiveDataInventoryItem.ForbiddenUseScope, StringComparison.Ordinal);
        Assert.Contains("quarantined-no-handling-permission", LabDataInventorySchemaReferenceData.SpecialCaseSensitiveDataInventoryItem.SpecialCasePosture, StringComparison.Ordinal);
    }

    [Fact]
    public void Held_And_Refused_Inventory_Records_Keep_Denied_Capabilities()
    {
        var held = LabDataInventorySchemaReferenceData.HeldForInventoryReview;
        var refused = LabDataInventorySchemaReferenceData.RefusedAsIngestibleOrActiveUse;

        Assert.Equal(LabDataInventoryDisposition.HeldForInventoryReview, held.Disposition);
        AssertDefaultDenied(held);
        Assert.Contains("Held Lab data inventory keeps inventory review open", held.NonAuthoritySummary, StringComparison.Ordinal);

        Assert.Equal(LabDataInventoryDisposition.RefusedAsIngestibleOrActiveUse, refused.Disposition);
        AssertDefaultDenied(refused);
        Assert.Contains(LabDataInventoryRefusalReason.IngestionOverclaimed, refused.RefusalReasons);
        Assert.Contains(LabDataInventoryRefusalReason.RawContentExposureOverclaimed, refused.RefusalReasons);
        Assert.Contains(LabDataInventoryRefusalReason.ConsentCollectionOverclaimed, refused.RefusalReasons);
        Assert.Contains(LabDataInventoryRefusalReason.ResearchAuthorizationOverclaimed, refused.RefusalReasons);
        Assert.Contains(LabDataInventoryRefusalReason.TrainingEligibilityOverclaimed, refused.RefusalReasons);
        Assert.Contains(LabDataInventoryRefusalReason.ProviderVisibilityOverclaimed, refused.RefusalReasons);
        Assert.Contains(LabDataInventoryRefusalReason.ModelContextExportOverclaimed, refused.RefusalReasons);
        Assert.Contains(LabDataInventoryRefusalReason.SurveillanceOrProfilingOverclaimed, refused.RefusalReasons);
        Assert.Contains(LabDataInventoryRefusalReason.IpTransferOverclaimed, refused.RefusalReasons);
        Assert.Contains(LabDataInventoryRefusalReason.SpecialCaseHandlingPermissionOverclaimed, refused.RefusalReasons);
        Assert.Contains(LabDataInventoryRefusalReason.RuntimeOrRtmeOverclaimed, refused.RefusalReasons);
    }

    [Fact]
    public void Inventory_Schema_Types_Do_Not_Expose_Service_Evaluator_Ingestion_Runtime_Consent_Or_Rtme_Owners()
    {
        var inventoryTypes = typeof(LabDataInventoryItemRecord)
            .Assembly
            .GetTypes()
            .Where(static type =>
                type.Namespace == "San.Common" &&
                type.Name.Contains("LabDataInventory", StringComparison.Ordinal))
            .Select(static type => type.Name)
            .ToArray();

        Assert.DoesNotContain(inventoryTypes, name => name.Contains("Service", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(inventoryTypes, name => name.Contains("Evaluator", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(inventoryTypes, name => name.Contains("Ingestion", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(inventoryTypes, name => name.Contains("Loader", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(inventoryTypes, name => name.Contains("ConsentEngine", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(inventoryTypes, name => name.Contains("RuntimeOwner", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(inventoryTypes, name => name.Contains("RtmeRunner", StringComparison.OrdinalIgnoreCase));
    }

    private static void AssertContainsClass(
        IReadOnlyList<LabDataInventoryItemRecord> records,
        LabDataInventoryClass dataClass)
    {
        Assert.Contains(records, record => record.DataClass == dataClass);
    }

    private static void AssertDefaultDenied(LabDataInventoryItemRecord record)
    {
        var allCapabilities = Enum.GetValues<LabDataInventoryDeniedCapability>();

        Assert.Equal(allCapabilities.Length, record.DeniedCapabilities.Count);

        foreach (var capability in allCapabilities)
        {
            Assert.Contains(capability, record.DeniedCapabilities);
        }
    }

    private static void AssertNotBlank(string value)
    {
        Assert.False(string.IsNullOrWhiteSpace(value));
    }
}
