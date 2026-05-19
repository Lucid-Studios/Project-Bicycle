using San.Common;
using Xunit;

namespace San.Audit.Tests;

public sealed class SanctuaryLocalConversationWitnessStoreReferenceDataTests
{
    [Fact]
    public void Passive_Contracts_Expose_Witness_Store_Posture_And_Refusal_Types()
    {
        Assert.Contains(SanctuaryLocalConversationWitnessStoreDisposition.Ready, Enum.GetValues<SanctuaryLocalConversationWitnessStoreDisposition>());
        Assert.Contains(SanctuaryLocalConversationWitnessStoreDisposition.Held, Enum.GetValues<SanctuaryLocalConversationWitnessStoreDisposition>());
        Assert.Contains(SanctuaryLocalConversationWitnessStoreDisposition.Refused, Enum.GetValues<SanctuaryLocalConversationWitnessStoreDisposition>());

        Assert.Contains(SanctuaryLocalConversationWitnessStorePosture.LocalOnly, Enum.GetValues<SanctuaryLocalConversationWitnessStorePosture>());
        Assert.Contains(SanctuaryLocalConversationWitnessStorePosture.Quarantined, Enum.GetValues<SanctuaryLocalConversationWitnessStorePosture>());
        Assert.Contains(SanctuaryLocalConversationWitnessStorePosture.Withheld, Enum.GetValues<SanctuaryLocalConversationWitnessStorePosture>());

        Assert.Contains(SanctuaryLocalConversationWitnessCapability.ProviderVisibleAccess, Enum.GetValues<SanctuaryLocalConversationWitnessCapability>());
        Assert.Contains(SanctuaryLocalConversationWitnessCapability.ResearchUse, Enum.GetValues<SanctuaryLocalConversationWitnessCapability>());
        Assert.Contains(SanctuaryLocalConversationWitnessCapability.ModelTrainingOrImprovement, Enum.GetValues<SanctuaryLocalConversationWitnessCapability>());
        Assert.Contains(SanctuaryLocalConversationWitnessCapability.Rehydration, Enum.GetValues<SanctuaryLocalConversationWitnessCapability>());
        Assert.Contains(SanctuaryLocalConversationWitnessCapability.GelSurvivorAdmission, Enum.GetValues<SanctuaryLocalConversationWitnessCapability>());
        Assert.Contains(SanctuaryLocalConversationWitnessCapability.RtmeMovement, Enum.GetValues<SanctuaryLocalConversationWitnessCapability>());
    }

    [Fact]
    public void Canonical_Reference_Data_Includes_Ready_Held_And_Refused_Records()
    {
        Assert.Contains(SanctuaryLocalConversationWitnessStoreReferenceData.CanonicalRecords, record => record.Disposition == SanctuaryLocalConversationWitnessStoreDisposition.Ready);
        Assert.Contains(SanctuaryLocalConversationWitnessStoreReferenceData.CanonicalRecords, record => record.Disposition == SanctuaryLocalConversationWitnessStoreDisposition.Held);
        Assert.Contains(SanctuaryLocalConversationWitnessStoreReferenceData.CanonicalRecords, record => record.Disposition == SanctuaryLocalConversationWitnessStoreDisposition.Refused);
    }

    [Fact]
    public void Ready_Witness_Store_Is_Local_Only_And_Denies_All_Capabilities()
    {
        var record = SanctuaryLocalConversationWitnessStoreReferenceData.ReadyLocalOnlyWitnessStore;

        Assert.Equal(FirstUseAdmissionEnactmentReferenceData.PreparedEnactmentReceipt.ReceiptHandle, record.SourceEnactmentRef);
        Assert.Equal(SanctuaryLocalConversationWitnessStorePosture.LocalOnly, record.StoragePosture);
        AssertDefaultDenied(record);
        Assert.Contains("The local store is not fuel", record.NonFuelSummary, StringComparison.Ordinal);
        Assert.Contains("Retention posture is not research consent", record.ConsentPostureSummary, StringComparison.Ordinal);
        Assert.Contains("provider-visible access", record.NonFuelSummary, StringComparison.Ordinal);
        Assert.Contains("RTME movement", record.NonFuelSummary, StringComparison.Ordinal);
    }

    [Fact]
    public void Held_Witness_Store_Quarantines_Special_Case_And_Retention_Questions()
    {
        var record = SanctuaryLocalConversationWitnessStoreReferenceData.HeldQuarantineWitnessStore;

        Assert.Equal(SanctuaryLocalConversationWitnessStoreDisposition.Held, record.Disposition);
        Assert.Equal(SanctuaryLocalConversationWitnessStorePosture.Quarantined, record.StoragePosture);
        AssertDefaultDenied(record);
        Assert.Contains("Special Case", record.RetentionPostureSummary, StringComparison.Ordinal);
        Assert.Contains("quarantine", record.RetentionPostureSummary, StringComparison.Ordinal);
        Assert.Contains("all capabilities remain denied", record.NonFuelSummary, StringComparison.Ordinal);
    }

    [Fact]
    public void Refused_Witness_Store_Names_Missing_And_Overclaim_Refusals()
    {
        var record = SanctuaryLocalConversationWitnessStoreReferenceData.RefusedWitnessStore;

        Assert.Equal(SanctuaryLocalConversationWitnessStoreDisposition.Refused, record.Disposition);
        Assert.Equal(SanctuaryLocalConversationWitnessStorePosture.Withheld, record.StoragePosture);
        AssertDefaultDenied(record);
        Assert.Contains(SanctuaryLocalConversationWitnessRefusalReason.MissingFirstUseEnactment, record.RefusalReasons);
        Assert.Contains(SanctuaryLocalConversationWitnessRefusalReason.MissingRetentionPosture, record.RefusalReasons);
        Assert.Contains(SanctuaryLocalConversationWitnessRefusalReason.ResearchConsentOverclaimed, record.RefusalReasons);
        Assert.Contains(SanctuaryLocalConversationWitnessRefusalReason.ProviderAccessOverclaimed, record.RefusalReasons);
        Assert.Contains(SanctuaryLocalConversationWitnessRefusalReason.ModelMemoryOverclaimed, record.RefusalReasons);
        Assert.Contains(SanctuaryLocalConversationWitnessRefusalReason.TrainingUseOverclaimed, record.RefusalReasons);
        Assert.Contains(SanctuaryLocalConversationWitnessRefusalReason.GelAdmissionOverclaimed, record.RefusalReasons);
        Assert.Contains(SanctuaryLocalConversationWitnessRefusalReason.RtmeMovementOverclaimed, record.RefusalReasons);
        Assert.Contains(SanctuaryLocalConversationWitnessRefusalReason.RuntimeAuthorityOverclaimed, record.RefusalReasons);
    }

    [Fact]
    public void Reference_Data_Does_Not_Expose_Operational_Store_Owners()
    {
        var contractAssemblyTypes = typeof(SanctuaryLocalConversationWitnessStoreRecord)
            .Assembly
            .GetTypes()
            .Where(static type => type.Namespace == "San.Common")
            .Select(static type => type.Name)
            .ToArray();

        Assert.DoesNotContain(contractAssemblyTypes, name => name.Contains("ConversationIngestion", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(contractAssemblyTypes, name => name.Contains("AutomaticLogging", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(contractAssemblyTypes, name => name.Contains("ProviderSync", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(contractAssemblyTypes, name => name.Contains("ResearchExport", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(contractAssemblyTypes, name => name.Contains("TrainingEligibility", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(contractAssemblyTypes, name => name.Contains("RehydrationPackage", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(contractAssemblyTypes, name => name.Contains("GelPromotion", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(contractAssemblyTypes, name => name.Contains("WitnessStoreService", StringComparison.OrdinalIgnoreCase));
    }

    private static void AssertDefaultDenied(SanctuaryLocalConversationWitnessStoreRecord record)
    {
        var allCapabilities = Enum.GetValues<SanctuaryLocalConversationWitnessCapability>();

        Assert.Equal(allCapabilities.Length, record.DefaultDeniedCapabilities.Count);

        foreach (var capability in allCapabilities)
        {
            Assert.Contains(capability, record.DefaultDeniedCapabilities);
        }
    }
}
