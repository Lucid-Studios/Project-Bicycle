using San.Common;
using Xunit;

namespace San.Audit.Tests;

public sealed class GoverningPrimeCrypticTemplateStructureReferenceDataTests
{
    [Fact]
    public void Passive_Contracts_Expose_Template_Offices_Predicate_Kinds_And_Denied_Capabilities()
    {
        Assert.Contains(GoverningPrimeCrypticTemplateDisposition.Ready, Enum.GetValues<GoverningPrimeCrypticTemplateDisposition>());
        Assert.Contains(GoverningPrimeCrypticTemplateDisposition.Held, Enum.GetValues<GoverningPrimeCrypticTemplateDisposition>());
        Assert.Contains(GoverningPrimeCrypticTemplateDisposition.Refused, Enum.GetValues<GoverningPrimeCrypticTemplateDisposition>());

        Assert.Contains(GoverningPrimeCrypticTemplateOffice.GoverningPrime, Enum.GetValues<GoverningPrimeCrypticTemplateOffice>());
        Assert.Contains(GoverningPrimeCrypticTemplateOffice.GoverningCryptic, Enum.GetValues<GoverningPrimeCrypticTemplateOffice>());
        Assert.Contains(GoverningPrimeCrypticTemplateOffice.PairedPrimeCrypticReceipt, Enum.GetValues<GoverningPrimeCrypticTemplateOffice>());
        Assert.Contains(GoverningPrimeCrypticTemplateOffice.UserDataPredicatePosture, Enum.GetValues<GoverningPrimeCrypticTemplateOffice>());

        Assert.Contains(GoverningPrimeCrypticUserDataPredicateKind.EntityPosture, Enum.GetValues<GoverningPrimeCrypticUserDataPredicateKind>());
        Assert.Contains(GoverningPrimeCrypticUserDataPredicateKind.AuthorityToBindPosture, Enum.GetValues<GoverningPrimeCrypticUserDataPredicateKind>());
        Assert.Contains(GoverningPrimeCrypticUserDataPredicateKind.ConsentScope, Enum.GetValues<GoverningPrimeCrypticUserDataPredicateKind>());
        Assert.Contains(GoverningPrimeCrypticUserDataPredicateKind.DisclosureRefs, Enum.GetValues<GoverningPrimeCrypticUserDataPredicateKind>());
        Assert.Contains(GoverningPrimeCrypticUserDataPredicateKind.LocalDataCategory, Enum.GetValues<GoverningPrimeCrypticUserDataPredicateKind>());
        Assert.Contains(GoverningPrimeCrypticUserDataPredicateKind.RetentionOptOut, Enum.GetValues<GoverningPrimeCrypticUserDataPredicateKind>());
        Assert.Contains(GoverningPrimeCrypticUserDataPredicateKind.ResearchSeparation, Enum.GetValues<GoverningPrimeCrypticUserDataPredicateKind>());
        Assert.Contains(GoverningPrimeCrypticUserDataPredicateKind.SpecialCaseQuarantine, Enum.GetValues<GoverningPrimeCrypticUserDataPredicateKind>());
        Assert.Contains(GoverningPrimeCrypticUserDataPredicateKind.IpAssetPosture, Enum.GetValues<GoverningPrimeCrypticUserDataPredicateKind>());
        Assert.Contains(GoverningPrimeCrypticUserDataPredicateKind.OperationalTelemetryPosture, Enum.GetValues<GoverningPrimeCrypticUserDataPredicateKind>());
        Assert.Contains(GoverningPrimeCrypticUserDataPredicateKind.NonAuthorityPosture, Enum.GetValues<GoverningPrimeCrypticUserDataPredicateKind>());

        Assert.Contains(GoverningPrimeCrypticTemplateDeniedCapability.DataCollection, Enum.GetValues<GoverningPrimeCrypticTemplateDeniedCapability>());
        Assert.Contains(GoverningPrimeCrypticTemplateDeniedCapability.ConsentCapture, Enum.GetValues<GoverningPrimeCrypticTemplateDeniedCapability>());
        Assert.Contains(GoverningPrimeCrypticTemplateDeniedCapability.Surveillance, Enum.GetValues<GoverningPrimeCrypticTemplateDeniedCapability>());
        Assert.Contains(GoverningPrimeCrypticTemplateDeniedCapability.Profiling, Enum.GetValues<GoverningPrimeCrypticTemplateDeniedCapability>());
        Assert.Contains(GoverningPrimeCrypticTemplateDeniedCapability.Training, Enum.GetValues<GoverningPrimeCrypticTemplateDeniedCapability>());
        Assert.Contains(GoverningPrimeCrypticTemplateDeniedCapability.ResearchUse, Enum.GetValues<GoverningPrimeCrypticTemplateDeniedCapability>());
        Assert.Contains(GoverningPrimeCrypticTemplateDeniedCapability.ProviderSync, Enum.GetValues<GoverningPrimeCrypticTemplateDeniedCapability>());
        Assert.Contains(GoverningPrimeCrypticTemplateDeniedCapability.CryptographicAuthority, Enum.GetValues<GoverningPrimeCrypticTemplateDeniedCapability>());
        Assert.Contains(GoverningPrimeCrypticTemplateDeniedCapability.EncryptionRuntime, Enum.GetValues<GoverningPrimeCrypticTemplateDeniedCapability>());
        Assert.Contains(GoverningPrimeCrypticTemplateDeniedCapability.PrimeMutation, Enum.GetValues<GoverningPrimeCrypticTemplateDeniedCapability>());
        Assert.Contains(GoverningPrimeCrypticTemplateDeniedCapability.CrypticMutation, Enum.GetValues<GoverningPrimeCrypticTemplateDeniedCapability>());
        Assert.Contains(GoverningPrimeCrypticTemplateDeniedCapability.GoverningCmeActivation, Enum.GetValues<GoverningPrimeCrypticTemplateDeniedCapability>());
        Assert.Contains(GoverningPrimeCrypticTemplateDeniedCapability.SliLispExecution, Enum.GetValues<GoverningPrimeCrypticTemplateDeniedCapability>());
        Assert.Contains(GoverningPrimeCrypticTemplateDeniedCapability.RtmeMovement, Enum.GetValues<GoverningPrimeCrypticTemplateDeniedCapability>());
        Assert.Contains(GoverningPrimeCrypticTemplateDeniedCapability.RuntimeControl, Enum.GetValues<GoverningPrimeCrypticTemplateDeniedCapability>());
    }

    [Fact]
    public void Canonical_Reference_Data_Includes_Ready_Held_And_Refused_Records()
    {
        Assert.Contains(GoverningPrimeCrypticTemplateStructureReferenceData.CanonicalRecords, record => record.Disposition == GoverningPrimeCrypticTemplateDisposition.Ready);
        Assert.Contains(GoverningPrimeCrypticTemplateStructureReferenceData.CanonicalRecords, record => record.Disposition == GoverningPrimeCrypticTemplateDisposition.Held);
        Assert.Contains(GoverningPrimeCrypticTemplateStructureReferenceData.CanonicalRecords, record => record.Disposition == GoverningPrimeCrypticTemplateDisposition.Refused);
    }

    [Fact]
    public void Ready_Template_Structure_Denies_Every_Forbidden_Capability()
    {
        var record = GoverningPrimeCrypticTemplateStructureReferenceData.ReadyTemplateStructure;

        Assert.Equal(GoverningPrimeCrypticTemplateDisposition.Ready, record.Disposition);
        Assert.Equal(SanctuaryGelMosCmosSeedSubstrateReferenceData.ReadyReceipt.ReceiptHandle, record.SourceMosCmosSeedSubstrateRef);
        AssertRequiredOffices(record);
        AssertPredicateKinds(record);
        AssertDefaultDenied(record);
        Assert.Contains("readable witness and telemetry candidate posture only", record.GoverningPrimeTemplatePosture, StringComparison.Ordinal);
        Assert.Contains("cryptic binder and handshake candidate posture only", record.GoverningCrypticTemplatePosture, StringComparison.Ordinal);
        Assert.Contains("candidate pairing only and not active cryptography", record.PairedPrimeCrypticReceiptPosture, StringComparison.Ordinal);
        Assert.Contains("do not collect user data", record.UserDataPredicatePosture, StringComparison.Ordinal);
        Assert.Contains("Data collection, consent capture, surveillance, profiling, training, research use, provider sync, cryptographic authority, encryption runtime, Prime/Cryptic mutation, governing CME activation, SLI.Lisp execution, RTME movement, and runtime control remain denied.", record.NonAuthoritySummary, StringComparison.Ordinal);
    }

    [Fact]
    public void Held_Template_Structure_Keeps_Local_Counsel_And_Data_Questions_Held_With_Denied_Capabilities()
    {
        var record = GoverningPrimeCrypticTemplateStructureReferenceData.HeldForLocalCounselOrDataReview;

        Assert.Equal(GoverningPrimeCrypticTemplateDisposition.Held, record.Disposition);
        AssertRequiredOffices(record);
        AssertPredicateKinds(record);
        AssertDefaultDenied(record);
        Assert.Contains("local, counsel, data, binder, handshake, telemetry, or predicate questions held", record.NonAuthoritySummary, StringComparison.Ordinal);
        Assert.Contains("every denied capability remains denied", record.NonAuthoritySummary, StringComparison.Ordinal);
    }

    [Fact]
    public void Refused_Template_Structure_Catches_Missing_Offices_And_Overclaims()
    {
        var record = GoverningPrimeCrypticTemplateStructureReferenceData.RefusedDataCollectionOrAuthorityOverclaim;

        Assert.Equal(GoverningPrimeCrypticTemplateDisposition.Refused, record.Disposition);
        Assert.Empty(record.Offices);
        Assert.Empty(record.UserDataPredicateKinds);
        AssertDefaultDenied(record);
        Assert.Contains(GoverningPrimeCrypticTemplateRefusalReason.MissingMosCmosSeedSubstrate, record.RefusalReasons);
        Assert.Contains(GoverningPrimeCrypticTemplateRefusalReason.MissingGoverningPrimeTemplate, record.RefusalReasons);
        Assert.Contains(GoverningPrimeCrypticTemplateRefusalReason.MissingGoverningCrypticTemplate, record.RefusalReasons);
        Assert.Contains(GoverningPrimeCrypticTemplateRefusalReason.MissingPairedPrimeCrypticReceipt, record.RefusalReasons);
        Assert.Contains(GoverningPrimeCrypticTemplateRefusalReason.MissingUserDataPredicatePosture, record.RefusalReasons);
        Assert.Contains(GoverningPrimeCrypticTemplateRefusalReason.DataCollectionOverclaimed, record.RefusalReasons);
        Assert.Contains(GoverningPrimeCrypticTemplateRefusalReason.ConsentCaptureOverclaimed, record.RefusalReasons);
        Assert.Contains(GoverningPrimeCrypticTemplateRefusalReason.SurveillanceOverclaimed, record.RefusalReasons);
        Assert.Contains(GoverningPrimeCrypticTemplateRefusalReason.ProfilingOverclaimed, record.RefusalReasons);
        Assert.Contains(GoverningPrimeCrypticTemplateRefusalReason.TrainingOverclaimed, record.RefusalReasons);
        Assert.Contains(GoverningPrimeCrypticTemplateRefusalReason.ResearchUseOverclaimed, record.RefusalReasons);
        Assert.Contains(GoverningPrimeCrypticTemplateRefusalReason.ProviderSyncOverclaimed, record.RefusalReasons);
        Assert.Contains(GoverningPrimeCrypticTemplateRefusalReason.CryptographicAuthorityOverclaimed, record.RefusalReasons);
        Assert.Contains(GoverningPrimeCrypticTemplateRefusalReason.MutationOverclaimed, record.RefusalReasons);
        Assert.Contains(GoverningPrimeCrypticTemplateRefusalReason.SliLispExecutionOverclaimed, record.RefusalReasons);
        Assert.Contains(GoverningPrimeCrypticTemplateRefusalReason.RtmeMovementOverclaimed, record.RefusalReasons);
        Assert.Contains(GoverningPrimeCrypticTemplateRefusalReason.GovernanceOrRuntimeOverclaimed, record.RefusalReasons);
    }

    [Fact]
    public void Template_Structure_Types_Do_Not_Expose_Service_Evaluator_Runtime_Data_Consent_Key_Or_Execution_Owners()
    {
        var templateTypes = typeof(GoverningPrimeCrypticTemplateRecord)
            .Assembly
            .GetTypes()
            .Where(static type =>
                type.Namespace == "San.Common" &&
                type.Name.Contains("GoverningPrimeCrypticTemplate", StringComparison.Ordinal))
            .Select(static type => type.Name)
            .ToArray();

        Assert.DoesNotContain(templateTypes, name => name.Contains("Service", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(templateTypes, name => name.Contains("Evaluator", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(templateTypes, name => name.Contains("RuntimeOwner", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(templateTypes, name => name.Contains("DataCollector", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(templateTypes, name => name.Contains("ConsentEngine", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(templateTypes, name => name.Contains("KeyGenerator", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(templateTypes, name => name.Contains("Executor", StringComparison.OrdinalIgnoreCase));
    }

    private static void AssertRequiredOffices(GoverningPrimeCrypticTemplateRecord record)
    {
        var allOffices = Enum.GetValues<GoverningPrimeCrypticTemplateOffice>();

        Assert.Equal(allOffices.Length, record.Offices.Count);

        foreach (var office in allOffices)
        {
            Assert.Contains(office, record.Offices);
        }
    }

    private static void AssertPredicateKinds(GoverningPrimeCrypticTemplateRecord record)
    {
        var allPredicateKinds = Enum.GetValues<GoverningPrimeCrypticUserDataPredicateKind>();

        Assert.Equal(allPredicateKinds.Length, record.UserDataPredicateKinds.Count);

        foreach (var predicateKind in allPredicateKinds)
        {
            Assert.Contains(predicateKind, record.UserDataPredicateKinds);
        }
    }

    private static void AssertDefaultDenied(GoverningPrimeCrypticTemplateRecord record)
    {
        var allCapabilities = Enum.GetValues<GoverningPrimeCrypticTemplateDeniedCapability>();

        Assert.Equal(allCapabilities.Length, record.DeniedCapabilities.Count);

        foreach (var capability in allCapabilities)
        {
            Assert.Contains(capability, record.DeniedCapabilities);
        }
    }
}
