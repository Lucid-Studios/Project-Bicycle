using San.Common;
using Xunit;

namespace San.Audit.Tests;

public sealed class SanctuaryGelMosCmosSeedSubstrateReferenceDataTests
{
    [Fact]
    public void Passive_Contracts_Expose_Seed_Substrate_Types()
    {
        Assert.Contains(SanctuaryGelMosCmosSeedSubstrateDisposition.Ready, Enum.GetValues<SanctuaryGelMosCmosSeedSubstrateDisposition>());
        Assert.Contains(SanctuaryGelMosCmosSeedSubstrateDisposition.Held, Enum.GetValues<SanctuaryGelMosCmosSeedSubstrateDisposition>());
        Assert.Contains(SanctuaryGelMosCmosSeedSubstrateDisposition.Refused, Enum.GetValues<SanctuaryGelMosCmosSeedSubstrateDisposition>());

        Assert.Contains(SanctuaryGelMosCmosSeedSubstrateLane.PrimeMosSeed, Enum.GetValues<SanctuaryGelMosCmosSeedSubstrateLane>());
        Assert.Contains(SanctuaryGelMosCmosSeedSubstrateLane.CrypticCmosSeed, Enum.GetValues<SanctuaryGelMosCmosSeedSubstrateLane>());
        Assert.Contains(SanctuaryGelMosCmosSeedSubstrateLane.PairedBinderSpline, Enum.GetValues<SanctuaryGelMosCmosSeedSubstrateLane>());
        Assert.Contains(SanctuaryGelMosCmosSeedSubstrateLane.NexusReadableModulation, Enum.GetValues<SanctuaryGelMosCmosSeedSubstrateLane>());

        Assert.Contains(SanctuaryGelMosCmosSeedSubstrateDeniedPower.GoverningCme, Enum.GetValues<SanctuaryGelMosCmosSeedSubstrateDeniedPower>());
        Assert.Contains(SanctuaryGelMosCmosSeedSubstrateDeniedPower.CryptographicKeyIssuance, Enum.GetValues<SanctuaryGelMosCmosSeedSubstrateDeniedPower>());
        Assert.Contains(SanctuaryGelMosCmosSeedSubstrateDeniedPower.EncryptionRuntime, Enum.GetValues<SanctuaryGelMosCmosSeedSubstrateDeniedPower>());
        Assert.Contains(SanctuaryGelMosCmosSeedSubstrateDeniedPower.PrimeMutation, Enum.GetValues<SanctuaryGelMosCmosSeedSubstrateDeniedPower>());
        Assert.Contains(SanctuaryGelMosCmosSeedSubstrateDeniedPower.HiddenCrypticMutation, Enum.GetValues<SanctuaryGelMosCmosSeedSubstrateDeniedPower>());
        Assert.Contains(SanctuaryGelMosCmosSeedSubstrateDeniedPower.NexusExecution, Enum.GetValues<SanctuaryGelMosCmosSeedSubstrateDeniedPower>());
        Assert.Contains(SanctuaryGelMosCmosSeedSubstrateDeniedPower.SliLispExecution, Enum.GetValues<SanctuaryGelMosCmosSeedSubstrateDeniedPower>());
        Assert.Contains(SanctuaryGelMosCmosSeedSubstrateDeniedPower.RuntimeControl, Enum.GetValues<SanctuaryGelMosCmosSeedSubstrateDeniedPower>());
    }

    [Fact]
    public void Canonical_Reference_Data_Includes_Ready_Held_And_Refused_Records()
    {
        Assert.Contains(SanctuaryGelMosCmosSeedSubstrateReferenceData.CanonicalRecords, record => record.Disposition == SanctuaryGelMosCmosSeedSubstrateDisposition.Ready);
        Assert.Contains(SanctuaryGelMosCmosSeedSubstrateReferenceData.CanonicalRecords, record => record.Disposition == SanctuaryGelMosCmosSeedSubstrateDisposition.Held);
        Assert.Contains(SanctuaryGelMosCmosSeedSubstrateReferenceData.CanonicalRecords, record => record.Disposition == SanctuaryGelMosCmosSeedSubstrateDisposition.Refused);
    }

    [Fact]
    public void Ready_Seed_Substrate_Denies_Every_Forbidden_Power()
    {
        var record = SanctuaryGelMosCmosSeedSubstrateReferenceData.ReadySeedSubstrate;

        Assert.Equal(SanctuaryGelMosCmosSeedSubstrateDisposition.Ready, record.Disposition);
        Assert.Equal(SanctuaryGelRegionalSubstrateFormationReferenceData.ReadyRegionalSubstrate.Identity.SubstrateHandle, record.SourceSanctuaryGelSubstrateRef);
        AssertRequiredLanes(record);
        AssertDefaultDenied(record);
        Assert.Contains("future Prime-side seed substrate only", record.PrimeMosSeedTelemetryPosture, StringComparison.Ordinal);
        Assert.Contains("future paired binder posture only", record.CrypticCmosBinderPosture, StringComparison.Ordinal);
        Assert.Contains("without key issuance or encryption runtime", record.PairedBinderSplinePosture, StringComparison.Ordinal);
        Assert.Contains("non-executable", record.NexusReadableModulationPosture, StringComparison.Ordinal);
        Assert.Contains("Governing CME, cryptographic key issuance, encryption runtime, Prime mutation, hidden Cryptic mutation, Nexus execution, SLI.Lisp execution, runtime control, and CME formation remain denied.", record.NonAuthoritySummary, StringComparison.Ordinal);
    }

    [Fact]
    public void Held_Seed_Substrate_Keeps_Binder_And_Telemetry_Questions_Held_With_Denied_Powers()
    {
        var record = SanctuaryGelMosCmosSeedSubstrateReferenceData.HeldForBinderOrTelemetryReview;

        Assert.Equal(SanctuaryGelMosCmosSeedSubstrateDisposition.Held, record.Disposition);
        AssertRequiredLanes(record);
        AssertDefaultDenied(record);
        Assert.Contains("binder, telemetry, regional, or modulation questions held", record.NonAuthoritySummary, StringComparison.Ordinal);
        Assert.Contains("every denied power remains denied", record.NonAuthoritySummary, StringComparison.Ordinal);
    }

    [Fact]
    public void Refused_Seed_Substrate_Catches_Missing_Lanes_And_All_Overclaims()
    {
        var record = SanctuaryGelMosCmosSeedSubstrateReferenceData.RefusedSeedSubstrateOverclaim;

        Assert.Equal(SanctuaryGelMosCmosSeedSubstrateDisposition.Refused, record.Disposition);
        Assert.Empty(record.Lanes);
        AssertDefaultDenied(record);
        Assert.Contains(SanctuaryGelMosCmosSeedSubstrateRefusalReason.MissingSanctuaryGelSubstrate, record.RefusalReasons);
        Assert.Contains(SanctuaryGelMosCmosSeedSubstrateRefusalReason.MissingPrimeMosSeed, record.RefusalReasons);
        Assert.Contains(SanctuaryGelMosCmosSeedSubstrateRefusalReason.MissingCrypticCmosSeed, record.RefusalReasons);
        Assert.Contains(SanctuaryGelMosCmosSeedSubstrateRefusalReason.MissingPairedBinderSpline, record.RefusalReasons);
        Assert.Contains(SanctuaryGelMosCmosSeedSubstrateRefusalReason.MissingNexusReadableModulation, record.RefusalReasons);
        Assert.Contains(SanctuaryGelMosCmosSeedSubstrateRefusalReason.GoverningCmeOverclaimed, record.RefusalReasons);
        Assert.Contains(SanctuaryGelMosCmosSeedSubstrateRefusalReason.CryptographicAuthorityOverclaimed, record.RefusalReasons);
        Assert.Contains(SanctuaryGelMosCmosSeedSubstrateRefusalReason.EncryptionRuntimeOverclaimed, record.RefusalReasons);
        Assert.Contains(SanctuaryGelMosCmosSeedSubstrateRefusalReason.PrimeMutationOverclaimed, record.RefusalReasons);
        Assert.Contains(SanctuaryGelMosCmosSeedSubstrateRefusalReason.HiddenCrypticMutationOverclaimed, record.RefusalReasons);
        Assert.Contains(SanctuaryGelMosCmosSeedSubstrateRefusalReason.NexusExecutionOverclaimed, record.RefusalReasons);
        Assert.Contains(SanctuaryGelMosCmosSeedSubstrateRefusalReason.SliLispExecutionOverclaimed, record.RefusalReasons);
        Assert.Contains(SanctuaryGelMosCmosSeedSubstrateRefusalReason.RuntimeControlOverclaimed, record.RefusalReasons);
        Assert.Contains(SanctuaryGelMosCmosSeedSubstrateRefusalReason.CmeFormationOverclaimed, record.RefusalReasons);
    }

    [Fact]
    public void Seed_Substrate_Types_Do_Not_Expose_Service_Evaluator_Runtime_Or_Execution_Owners()
    {
        var seedTypes = typeof(SanctuaryGelMosCmosSeedSubstrateRecord)
            .Assembly
            .GetTypes()
            .Where(static type =>
                type.Namespace == "San.Common" &&
                type.Name.Contains("SanctuaryGelMosCmosSeedSubstrate", StringComparison.Ordinal))
            .Select(static type => type.Name)
            .ToArray();

        Assert.DoesNotContain(seedTypes, name => name.Contains("Service", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(seedTypes, name => name.Contains("Evaluator", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(seedTypes, name => name.Contains("RuntimeOwner", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(seedTypes, name => name.Contains("KeyGenerator", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(seedTypes, name => name.Contains("Mutator", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(seedTypes, name => name.Contains("Executor", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(seedTypes, name => name.Contains("CmeFormation", StringComparison.OrdinalIgnoreCase));
    }

    private static void AssertRequiredLanes(SanctuaryGelMosCmosSeedSubstrateRecord record)
    {
        var allLanes = Enum.GetValues<SanctuaryGelMosCmosSeedSubstrateLane>();

        Assert.Equal(allLanes.Length, record.Lanes.Count);

        foreach (var lane in allLanes)
        {
            Assert.Contains(lane, record.Lanes);
        }
    }

    private static void AssertDefaultDenied(SanctuaryGelMosCmosSeedSubstrateRecord record)
    {
        var allPowers = Enum.GetValues<SanctuaryGelMosCmosSeedSubstrateDeniedPower>();

        Assert.Equal(allPowers.Length, record.DeniedPowers.Count);

        foreach (var power in allPowers)
        {
            Assert.Contains(power, record.DeniedPowers);
        }
    }
}
