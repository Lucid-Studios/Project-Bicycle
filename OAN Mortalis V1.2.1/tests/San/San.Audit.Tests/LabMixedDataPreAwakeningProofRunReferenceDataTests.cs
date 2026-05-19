using San.Common;
using Xunit;

namespace San.Audit.Tests;

public sealed class LabMixedDataPreAwakeningProofRunReferenceDataTests
{
    [Fact]
    public void Passive_Contracts_Expose_Proof_Dispositions_Datum_Kinds_Stages_Receipts_And_Denials()
    {
        Assert.Contains(LabMixedDataPreAwakeningProofDisposition.HeldForProof, Enum.GetValues<LabMixedDataPreAwakeningProofDisposition>());
        Assert.Contains(LabMixedDataPreAwakeningProofDisposition.RefusedUntilConsentAndStartupAdmission, Enum.GetValues<LabMixedDataPreAwakeningProofDisposition>());
        Assert.Equal(2, Enum.GetValues<LabMixedDataPreAwakeningProofDisposition>().Length);

        Assert.Contains(LabMixedDataManifestDatumKind.PersonalOperator, Enum.GetValues<LabMixedDataManifestDatumKind>());
        Assert.Contains(LabMixedDataManifestDatumKind.PrivateLabBusiness, Enum.GetValues<LabMixedDataManifestDatumKind>());
        Assert.Contains(LabMixedDataManifestDatumKind.IpAsset, Enum.GetValues<LabMixedDataManifestDatumKind>());
        Assert.Contains(LabMixedDataManifestDatumKind.ConversationWitness, Enum.GetValues<LabMixedDataManifestDatumKind>());
        Assert.Contains(LabMixedDataManifestDatumKind.OperationalTelemetry, Enum.GetValues<LabMixedDataManifestDatumKind>());
        Assert.Contains(LabMixedDataManifestDatumKind.SpecialCaseSensitiveHeld, Enum.GetValues<LabMixedDataManifestDatumKind>());

        Assert.Contains(LabMixedDataPreAwakeningProofStage.LocalLabDataManifest, Enum.GetValues<LabMixedDataPreAwakeningProofStage>());
        Assert.Contains(LabMixedDataPreAwakeningProofStage.ActivationHeldOrRefusedByDesign, Enum.GetValues<LabMixedDataPreAwakeningProofStage>());

        Assert.Contains(LabMixedDataPreAwakeningProofReceiptKind.DataManifest, Enum.GetValues<LabMixedDataPreAwakeningProofReceiptKind>());
        Assert.Contains(LabMixedDataPreAwakeningProofReceiptKind.StartupAttemptHoldOrRefusal, Enum.GetValues<LabMixedDataPreAwakeningProofReceiptKind>());

        Assert.Contains(LabMixedDataPreAwakeningDeniedCapability.RawContentExposure, Enum.GetValues<LabMixedDataPreAwakeningDeniedCapability>());
        Assert.Contains(LabMixedDataPreAwakeningDeniedCapability.ProviderVisibility, Enum.GetValues<LabMixedDataPreAwakeningDeniedCapability>());
        Assert.Contains(LabMixedDataPreAwakeningDeniedCapability.ConsentCreation, Enum.GetValues<LabMixedDataPreAwakeningDeniedCapability>());
        Assert.Contains(LabMixedDataPreAwakeningDeniedCapability.ResearchUse, Enum.GetValues<LabMixedDataPreAwakeningDeniedCapability>());
        Assert.Contains(LabMixedDataPreAwakeningDeniedCapability.Training, Enum.GetValues<LabMixedDataPreAwakeningDeniedCapability>());
        Assert.Contains(LabMixedDataPreAwakeningDeniedCapability.Profiling, Enum.GetValues<LabMixedDataPreAwakeningDeniedCapability>());
        Assert.Contains(LabMixedDataPreAwakeningDeniedCapability.Surveillance, Enum.GetValues<LabMixedDataPreAwakeningDeniedCapability>());
        Assert.Contains(LabMixedDataPreAwakeningDeniedCapability.ModelContext, Enum.GetValues<LabMixedDataPreAwakeningDeniedCapability>());
        Assert.Contains(LabMixedDataPreAwakeningDeniedCapability.RtmeMovement, Enum.GetValues<LabMixedDataPreAwakeningDeniedCapability>());
        Assert.Contains(LabMixedDataPreAwakeningDeniedCapability.SliLispExecution, Enum.GetValues<LabMixedDataPreAwakeningDeniedCapability>());
        Assert.Contains(LabMixedDataPreAwakeningDeniedCapability.PrimeCrypticMutation, Enum.GetValues<LabMixedDataPreAwakeningDeniedCapability>());
        Assert.Contains(LabMixedDataPreAwakeningDeniedCapability.RuntimeControl, Enum.GetValues<LabMixedDataPreAwakeningDeniedCapability>());
    }

    [Fact]
    public void Canonical_Reference_Data_Uses_Only_Held_And_Consent_Startup_Refusal_States()
    {
        Assert.Contains(LabMixedDataPreAwakeningProofRunReferenceData.CanonicalRecords, record => record.Disposition == LabMixedDataPreAwakeningProofDisposition.HeldForProof);
        Assert.Contains(LabMixedDataPreAwakeningProofRunReferenceData.CanonicalRecords, record => record.Disposition == LabMixedDataPreAwakeningProofDisposition.RefusedUntilConsentAndStartupAdmission);
        Assert.All(
            LabMixedDataPreAwakeningProofRunReferenceData.CanonicalRecords,
            record => Assert.True(
                record.Disposition is
                    LabMixedDataPreAwakeningProofDisposition.HeldForProof or
                    LabMixedDataPreAwakeningProofDisposition.RefusedUntilConsentAndStartupAdmission));
    }

    [Fact]
    public void Held_Metadata_Only_Proof_Run_Represents_All_Six_Datum_Kinds_Without_Raw_Content()
    {
        var record = LabMixedDataPreAwakeningProofRunReferenceData.HeldMetadataOnlyProofRun;

        Assert.Equal(LabMixedDataPreAwakeningProofDisposition.HeldForProof, record.Disposition);
        Assert.Equal(GoverningPrimeCrypticTemplateStructureReferenceData.ReadyReceipt.ReceiptHandle, record.SourceGoverningPrimeCrypticTemplateRef);
        Assert.Equal(6, record.ManifestEntries.Count);
        AssertManifestContains(record, LabMixedDataManifestDatumKind.PersonalOperator);
        AssertManifestContains(record, LabMixedDataManifestDatumKind.PrivateLabBusiness);
        AssertManifestContains(record, LabMixedDataManifestDatumKind.IpAsset);
        AssertManifestContains(record, LabMixedDataManifestDatumKind.ConversationWitness);
        AssertManifestContains(record, LabMixedDataManifestDatumKind.OperationalTelemetry);
        AssertManifestContains(record, LabMixedDataManifestDatumKind.SpecialCaseSensitiveHeld);

        foreach (var entry in record.ManifestEntries)
        {
            Assert.Contains("local-ref://", entry.LogicalLocalRef, StringComparison.Ordinal);
            Assert.Equal("hash-or-ref-only-no-raw-content", entry.HashOrRefPosture);
            Assert.Contains("metadata", entry.Summary, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("raw", entry.LogicalLocalRef, StringComparison.OrdinalIgnoreCase);
        }
    }

    [Fact]
    public void Held_Metadata_Only_Proof_Run_Represents_All_Stages_Receipts_And_Denies_All_Capabilities()
    {
        var record = LabMixedDataPreAwakeningProofRunReferenceData.HeldMetadataOnlyProofRun;

        AssertDefaultStages(record);
        AssertDefaultReceipts(record);
        AssertDefaultDenied(record);
        Assert.Contains("do not grant authority", record.PredicateContextPosture, StringComparison.Ordinal);
        Assert.Contains("without collecting raw content or creating consent", record.PayloadClassificationPosture, StringComparison.Ordinal);
        Assert.Contains("activation remains held by design", record.ConsentStartupPosture, StringComparison.Ordinal);
        Assert.Contains("Special Case/sensitive held datum remains quarantined", record.SpecialCaseQuarantinePosture, StringComparison.Ordinal);
        Assert.Contains("HeldForProof is success posture", record.ActivationResultPosture, StringComparison.Ordinal);
        Assert.Contains("raw-content exposure, provider visibility, consent creation, research use, training, profiling, surveillance, model context, RTME movement, SLI.Lisp execution, Prime/Cryptic mutation, governance, and runtime control remain denied", record.NonMisuseSummary, StringComparison.Ordinal);
    }

    [Fact]
    public void Consent_And_Startup_Absence_Keeps_Activation_Refused_By_Design()
    {
        var record = LabMixedDataPreAwakeningProofRunReferenceData.RefusedUntilConsentAndStartupAdmission;

        Assert.Equal(LabMixedDataPreAwakeningProofDisposition.RefusedUntilConsentAndStartupAdmission, record.Disposition);
        AssertDefaultDenied(record);
        Assert.Contains(LabMixedDataPreAwakeningProofRefusalReason.MissingPreActivationLegitimacyPosture, record.RefusalReasons);
        Assert.Contains(LabMixedDataPreAwakeningProofRefusalReason.StartupAttemptNotHeldOrRefused, record.RefusalReasons);
        Assert.Contains("not admitted", record.ConsentStartupPosture, StringComparison.Ordinal);
        Assert.Contains("Activation is refused until consent and startup admission are separately represented", record.ActivationResultPosture, StringComparison.Ordinal);
    }

    [Fact]
    public void Misuse_Overclaim_Refuses_Raw_Content_Context_Rtme_Sli_Mutation_Governance_And_Runtime()
    {
        var record = LabMixedDataPreAwakeningProofRunReferenceData.RefusedMisuseOverclaimReadout;

        Assert.Equal(LabMixedDataPreAwakeningProofDisposition.RefusedUntilConsentAndStartupAdmission, record.Disposition);
        Assert.Empty(record.ManifestEntries);
        AssertDefaultDenied(record);
        Assert.Contains(LabMixedDataPreAwakeningProofRefusalReason.RawContentExposureOverclaimed, record.RefusalReasons);
        Assert.Contains(LabMixedDataPreAwakeningProofRefusalReason.ProviderVisibilityOverclaimed, record.RefusalReasons);
        Assert.Contains(LabMixedDataPreAwakeningProofRefusalReason.ConsentCreationOverclaimed, record.RefusalReasons);
        Assert.Contains(LabMixedDataPreAwakeningProofRefusalReason.ResearchTrainingProfilingSurveillanceOverclaimed, record.RefusalReasons);
        Assert.Contains(LabMixedDataPreAwakeningProofRefusalReason.ModelContextOverclaimed, record.RefusalReasons);
        Assert.Contains(LabMixedDataPreAwakeningProofRefusalReason.RetentionOrSpecialCaseWideningOverclaimed, record.RefusalReasons);
        Assert.Contains(LabMixedDataPreAwakeningProofRefusalReason.RtmeOrSliLispOverclaimed, record.RefusalReasons);
        Assert.Contains(LabMixedDataPreAwakeningProofRefusalReason.PrimeCrypticMutationOverclaimed, record.RefusalReasons);
        Assert.Contains(LabMixedDataPreAwakeningProofRefusalReason.GovernanceOrRuntimeOverclaimed, record.RefusalReasons);
        Assert.Contains("No third proof result state is admitted", record.NonMisuseSummary, StringComparison.Ordinal);
    }

    [Fact]
    public void Proof_Run_Types_Do_Not_Expose_Service_Runner_Ingestion_Startup_Activation_Or_Runtime_Owners()
    {
        var proofTypes = typeof(LabMixedDataPreAwakeningProofRunRecord)
            .Assembly
            .GetTypes()
            .Where(static type =>
                type.Namespace == "San.Common" &&
                type.Name.Contains("LabMixedDataPreAwakening", StringComparison.Ordinal))
            .Select(static type => type.Name)
            .ToArray();

        Assert.DoesNotContain(proofTypes, name => name.Contains("Service", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(proofTypes, name => name.Contains("Runner", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(proofTypes, name => name.Contains("Harness", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(proofTypes, name => name.Contains("Ingestion", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(proofTypes, name => name.Contains("Startup", StringComparison.OrdinalIgnoreCase) && name.Contains("Owner", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(proofTypes, name => name.Contains("Activation", StringComparison.OrdinalIgnoreCase) && name.Contains("Service", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(proofTypes, name => name.Contains("RuntimeOwner", StringComparison.OrdinalIgnoreCase));
    }

    private static void AssertManifestContains(
        LabMixedDataPreAwakeningProofRunRecord record,
        LabMixedDataManifestDatumKind kind)
    {
        Assert.Contains(record.ManifestEntries, entry => entry.Kind == kind);
    }

    private static void AssertDefaultStages(LabMixedDataPreAwakeningProofRunRecord record)
    {
        var allStages = Enum.GetValues<LabMixedDataPreAwakeningProofStage>();

        Assert.Equal(allStages.Length, record.ProofStages.Count);

        foreach (var stage in allStages)
        {
            Assert.Contains(stage, record.ProofStages);
        }
    }

    private static void AssertDefaultReceipts(LabMixedDataPreAwakeningProofRunRecord record)
    {
        var allReceipts = Enum.GetValues<LabMixedDataPreAwakeningProofReceiptKind>();

        Assert.Equal(allReceipts.Length, record.ReceiptKinds.Count);

        foreach (var receipt in allReceipts)
        {
            Assert.Contains(receipt, record.ReceiptKinds);
        }
    }

    private static void AssertDefaultDenied(LabMixedDataPreAwakeningProofRunRecord record)
    {
        var allCapabilities = Enum.GetValues<LabMixedDataPreAwakeningDeniedCapability>();

        Assert.Equal(allCapabilities.Length, record.DeniedCapabilities.Count);

        foreach (var capability in allCapabilities)
        {
            Assert.Contains(capability, record.DeniedCapabilities);
        }
    }
}
