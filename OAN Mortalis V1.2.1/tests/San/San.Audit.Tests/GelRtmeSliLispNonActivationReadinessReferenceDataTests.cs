using San.Common;
using Xunit;

namespace San.Audit.Tests;

public sealed class GelRtmeSliLispNonActivationReadinessReferenceDataTests
{
    [Fact]
    public void Passive_Contracts_Expose_Readiness_Posture_Denied_Capability_And_Refusal_Types()
    {
        Assert.Contains(GelRtmeSliLispReadinessDisposition.Ready, Enum.GetValues<GelRtmeSliLispReadinessDisposition>());
        Assert.Contains(GelRtmeSliLispReadinessDisposition.Held, Enum.GetValues<GelRtmeSliLispReadinessDisposition>());
        Assert.Contains(GelRtmeSliLispReadinessDisposition.Refused, Enum.GetValues<GelRtmeSliLispReadinessDisposition>());

        Assert.Contains(GelRtmeSliLispReadinessPosture.CandidateReadable, Enum.GetValues<GelRtmeSliLispReadinessPosture>());
        Assert.Contains(GelRtmeSliLispReadinessPosture.HeldForGate, Enum.GetValues<GelRtmeSliLispReadinessPosture>());
        Assert.Contains(GelRtmeSliLispReadinessPosture.MovementWithheld, Enum.GetValues<GelRtmeSliLispReadinessPosture>());

        Assert.Contains(GelRtmeSliLispDeniedCapability.RtmeActivation, Enum.GetValues<GelRtmeSliLispDeniedCapability>());
        Assert.Contains(GelRtmeSliLispDeniedCapability.RuntimeTransactionMovement, Enum.GetValues<GelRtmeSliLispDeniedCapability>());
        Assert.Contains(GelRtmeSliLispDeniedCapability.AlwaysOnAuthority, Enum.GetValues<GelRtmeSliLispDeniedCapability>());
        Assert.Contains(GelRtmeSliLispDeniedCapability.DirectPersistence, Enum.GetValues<GelRtmeSliLispDeniedCapability>());
        Assert.Contains(GelRtmeSliLispDeniedCapability.DirectPrimeMutation, Enum.GetValues<GelRtmeSliLispDeniedCapability>());
        Assert.Contains(GelRtmeSliLispDeniedCapability.MembraneBypass, Enum.GetValues<GelRtmeSliLispDeniedCapability>());
        Assert.Contains(GelRtmeSliLispDeniedCapability.SliLispExecution, Enum.GetValues<GelRtmeSliLispDeniedCapability>());
        Assert.Contains(GelRtmeSliLispDeniedCapability.SurvivorAdmission, Enum.GetValues<GelRtmeSliLispDeniedCapability>());
    }

    [Fact]
    public void Canonical_Reference_Data_Includes_Ready_Held_And_Refused_Records()
    {
        Assert.Contains(GelRtmeSliLispNonActivationReadinessReferenceData.CanonicalRecords, record => record.Disposition == GelRtmeSliLispReadinessDisposition.Ready);
        Assert.Contains(GelRtmeSliLispNonActivationReadinessReferenceData.CanonicalRecords, record => record.Disposition == GelRtmeSliLispReadinessDisposition.Held);
        Assert.Contains(GelRtmeSliLispNonActivationReadinessReferenceData.CanonicalRecords, record => record.Disposition == GelRtmeSliLispReadinessDisposition.Refused);
    }

    [Fact]
    public void Ready_Non_Activation_Readiness_Reads_Candidacy_Only_And_Denies_All_Capabilities()
    {
        var record = GelRtmeSliLispNonActivationReadinessReferenceData.ReadyNonActivationReadiness;

        Assert.Equal(GelRtmeSliLispReadinessDisposition.Ready, record.Disposition);
        Assert.Equal(GelRtmeSliLispReadinessPosture.CandidateReadable, record.ReadinessPosture);
        AssertDefaultDenied(record);
        Assert.Contains("future movement candidacy is readable only", record.NonActivationSummary, StringComparison.Ordinal);
        Assert.Contains("rtme_active=false", record.NonActivationSummary, StringComparison.Ordinal);
        Assert.Contains("runtime_transaction_movement_allowed=false", record.NonActivationSummary, StringComparison.Ordinal);
        Assert.Contains("always_on_authority_granted=false", record.NonActivationSummary, StringComparison.Ordinal);
        Assert.Contains("direct_persistence_allowed=false", record.NonActivationSummary, StringComparison.Ordinal);
        Assert.Contains("direct_prime_mutation_allowed=false", record.NonActivationSummary, StringComparison.Ordinal);
        Assert.Contains("Nothing may move", record.NonActivationSummary, StringComparison.Ordinal);
    }

    [Fact]
    public void Held_Non_Activation_Readiness_Keeps_Gates_Held_And_Denied()
    {
        var record = GelRtmeSliLispNonActivationReadinessReferenceData.HeldForExplicitGate;

        Assert.Equal(GelRtmeSliLispReadinessDisposition.Held, record.Disposition);
        Assert.Equal(GelRtmeSliLispReadinessPosture.HeldForGate, record.ReadinessPosture);
        AssertDefaultDenied(record);
        Assert.Contains("explicit gate questions held", record.NonActivationSummary, StringComparison.Ordinal);
        Assert.Contains("remain denied", record.NonActivationSummary, StringComparison.Ordinal);
    }

    [Fact]
    public void Refused_Non_Activation_Readiness_Catches_Movement_And_Authority_Overclaims()
    {
        var record = GelRtmeSliLispNonActivationReadinessReferenceData.RefusedMovementOverclaim;

        Assert.Equal(GelRtmeSliLispReadinessDisposition.Refused, record.Disposition);
        Assert.Equal(GelRtmeSliLispReadinessPosture.MovementWithheld, record.ReadinessPosture);
        AssertDefaultDenied(record);
        Assert.Contains(GelRtmeSliLispReadinessRefusalReason.MovementOverclaimed, record.RefusalReasons);
        Assert.Contains(GelRtmeSliLispReadinessRefusalReason.PersistenceOverclaimed, record.RefusalReasons);
        Assert.Contains(GelRtmeSliLispReadinessRefusalReason.AuthorityOverclaimed, record.RefusalReasons);
        Assert.Contains(GelRtmeSliLispReadinessRefusalReason.SliLispExecutionOverclaimed, record.RefusalReasons);
        Assert.Contains(GelRtmeSliLispReadinessRefusalReason.RtmeActivationOverclaimed, record.RefusalReasons);
        Assert.Contains(GelRtmeSliLispReadinessRefusalReason.MembraneBypassOverclaimed, record.RefusalReasons);
        Assert.Contains(GelRtmeSliLispReadinessRefusalReason.SurvivorAdmissionOverclaimed, record.RefusalReasons);
    }

    [Fact]
    public void Reference_Data_Does_Not_Expose_Operational_Movement_Owners()
    {
        var contractAssemblyTypes = typeof(GelRtmeSliLispNonActivationReadinessRecord)
            .Assembly
            .GetTypes()
            .Where(static type => type.Namespace == "San.Common")
            .Select(static type => type.Name)
            .ToArray();

        Assert.DoesNotContain(contractAssemblyTypes, name => name.Contains("RtmeRunner", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(contractAssemblyTypes, name => name.Contains("RtmeListener", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(contractAssemblyTypes, name => name.Contains("PersistenceWriter", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(contractAssemblyTypes, name => name.Contains("PrimeMutator", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(contractAssemblyTypes, name => name.Contains("MembraneBypass", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(contractAssemblyTypes, name => name.Contains("AutoAdmission", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(contractAssemblyTypes, name => name.Contains("SurvivorAdmissionService", StringComparison.OrdinalIgnoreCase));
    }

    private static void AssertDefaultDenied(GelRtmeSliLispNonActivationReadinessRecord record)
    {
        var allCapabilities = Enum.GetValues<GelRtmeSliLispDeniedCapability>();

        Assert.Equal(allCapabilities.Length, record.DeniedCapabilities.Count);

        foreach (var capability in allCapabilities)
        {
            Assert.Contains(capability, record.DeniedCapabilities);
        }
    }
}
