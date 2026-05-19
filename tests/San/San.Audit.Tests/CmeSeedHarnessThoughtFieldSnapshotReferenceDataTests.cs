using San.Common;
using Xunit;

namespace San.Audit.Tests;

public sealed class CmeSeedHarnessThoughtFieldSnapshotReferenceDataTests
{
    [Fact]
    public void Passive_Contracts_Expose_Snapshot_Ui_Response_Lane_And_Denials()
    {
        Assert.Contains(CmeSeedHarnessThoughtFieldSnapshotDisposition.FrozenForCodeFormation, Enum.GetValues<CmeSeedHarnessThoughtFieldSnapshotDisposition>());
        Assert.Contains(CmeSeedHarnessThoughtFieldSnapshotDisposition.HeldForHarnessBuild, Enum.GetValues<CmeSeedHarnessThoughtFieldSnapshotDisposition>());
        Assert.Contains(CmeSeedHarnessThoughtFieldSnapshotDisposition.RefusedAsActivationOverclaim, Enum.GetValues<CmeSeedHarnessThoughtFieldSnapshotDisposition>());

        Assert.Contains(CmeSeedHarnessUiFieldKind.OperatorPrompt, Enum.GetValues<CmeSeedHarnessUiFieldKind>());
        Assert.Contains(CmeSeedHarnessUiFieldKind.SeedPostureSelector, Enum.GetValues<CmeSeedHarnessUiFieldKind>());
        Assert.Contains(CmeSeedHarnessUiFieldKind.InventoryEvaluationRefs, Enum.GetValues<CmeSeedHarnessUiFieldKind>());
        Assert.Contains(CmeSeedHarnessUiFieldKind.ResponseMode, Enum.GetValues<CmeSeedHarnessUiFieldKind>());
        Assert.Contains(CmeSeedHarnessUiFieldKind.RefusalHoldReadoutLane, Enum.GetValues<CmeSeedHarnessUiFieldKind>());

        Assert.Contains(CmeSeedHarnessResponseLaneDisposition.SeededReadoutOnly, Enum.GetValues<CmeSeedHarnessResponseLaneDisposition>());
        Assert.Contains(CmeSeedHarnessDeniedCapability.RawLabDataIngestion, Enum.GetValues<CmeSeedHarnessDeniedCapability>());
        Assert.Contains(CmeSeedHarnessDeniedCapability.SliLispExecution, Enum.GetValues<CmeSeedHarnessDeniedCapability>());
        Assert.Contains(CmeSeedHarnessDeniedCapability.RtmeMovement, Enum.GetValues<CmeSeedHarnessDeniedCapability>());
        Assert.Contains(CmeSeedHarnessDeniedCapability.SanctuaryActualFormation, Enum.GetValues<CmeSeedHarnessDeniedCapability>());
        Assert.Contains(CmeSeedHarnessDeniedCapability.RuntimeAuthority, Enum.GetValues<CmeSeedHarnessDeniedCapability>());
    }

    [Fact]
    public void Frozen_Snapshot_Carries_Thought_Field_And_Denies_All_Forbidden_Capabilities()
    {
        var snapshot = CmeSeedHarnessThoughtFieldSnapshotReferenceData.FrozenSnapshot;

        Assert.Equal(CmeSeedHarnessThoughtFieldSnapshotDisposition.FrozenForCodeFormation, snapshot.Disposition);
        Assert.Equal(LabDataInventoryEvaluationPostureReferenceData.ReadableReceipt.ReceiptHandle, snapshot.SourceInventoryEvaluationReceiptRef);
        Assert.Contains("documented data", snapshot.ThoughtFieldLadder);
        Assert.Contains("metadata-only proof posture", snapshot.ThoughtFieldLadder);
        Assert.Contains("governed inventory schema", snapshot.ThoughtFieldLadder);
        Assert.Contains("inventory evaluation posture", snapshot.ThoughtFieldLadder);
        Assert.Contains("claim-reading threshold", snapshot.ThoughtFieldLadder);
        Assert.Contains("first CME seed harness response lane", snapshot.ThoughtFieldLadder);
        AssertDefaultDenied(snapshot.DeniedCapabilities);
        Assert.Contains(CmeSeedHarnessRefusalReason.None, snapshot.RefusalReasons);
        Assert.Contains("denies raw Lab data ingestion, consent creation, model training, research use, provider visibility, model-context export, SLI.Lisp execution, RTME movement, Prime/Cryptic mutation, governing CME activation, Sanctuary.Actual formation, and runtime authority", snapshot.NonAuthoritySummary, StringComparison.Ordinal);
    }

    [Fact]
    public void Ui_Template_Fields_Are_Template_Only_And_Non_Collecting()
    {
        var fields = CmeSeedHarnessThoughtFieldSnapshotReferenceData.UiTemplateFields;

        Assert.Equal(5, fields.Count);
        Assert.Contains(fields, field => field.Kind == CmeSeedHarnessUiFieldKind.OperatorPrompt);
        Assert.Contains(fields, field => field.Kind == CmeSeedHarnessUiFieldKind.SeedPostureSelector);
        Assert.Contains(fields, field => field.Kind == CmeSeedHarnessUiFieldKind.InventoryEvaluationRefs);
        Assert.Contains(fields, field => field.Kind == CmeSeedHarnessUiFieldKind.ResponseMode);
        Assert.Contains(fields, field => field.Kind == CmeSeedHarnessUiFieldKind.RefusalHoldReadoutLane);

        foreach (var field in fields)
        {
            Assert.True(field.RequiredForTemplate);
            Assert.Contains("template-only", field.NonCollectionPosture, StringComparison.Ordinal);
        }
    }

    [Fact]
    public void Response_Lane_Is_Seeded_Readout_Only_Not_Model_Call_Or_Runtime()
    {
        var lane = CmeSeedHarnessThoughtFieldSnapshotReferenceData.SeededResponseLane;

        Assert.Equal(CmeSeedHarnessResponseLaneDisposition.SeededReadoutOnly, lane.Disposition);
        Assert.Equal("deterministic-local-readout", lane.ResponseMode);
        AssertDefaultDenied(lane.DeniedCapabilities);
        Assert.Contains("no operator prompt content or raw Lab data is collected", lane.InputPostureSummary, StringComparison.Ordinal);
        Assert.Contains("does not claim activation, certification, survivor admission, or runtime authority", lane.OutputPostureSummary, StringComparison.Ordinal);
        Assert.Contains("not a model call, not SLI.Lisp, not RTME, not Prime/Cryptic mutation, not Sanctuary.Actual, and not runtime authority", lane.NonAuthoritySummary, StringComparison.Ordinal);
    }

    [Fact]
    public void Held_And_Refused_Snapshots_Preserve_Denied_Capabilities()
    {
        var held = CmeSeedHarnessThoughtFieldSnapshotReferenceData.HeldForHarnessBuild;
        var refused = CmeSeedHarnessThoughtFieldSnapshotReferenceData.RefusedActivationOverclaim;

        Assert.Equal(CmeSeedHarnessThoughtFieldSnapshotDisposition.HeldForHarnessBuild, held.Disposition);
        AssertDefaultDenied(held.DeniedCapabilities);
        Assert.Contains(CmeSeedHarnessRefusalReason.MissingInventoryEvaluationPosture, held.RefusalReasons);

        Assert.Equal(CmeSeedHarnessThoughtFieldSnapshotDisposition.RefusedAsActivationOverclaim, refused.Disposition);
        AssertDefaultDenied(refused.DeniedCapabilities);
        Assert.Contains(CmeSeedHarnessRefusalReason.RawDataIngestionOverclaimed, refused.RefusalReasons);
        Assert.Contains(CmeSeedHarnessRefusalReason.ConsentOverclaimed, refused.RefusalReasons);
        Assert.Contains(CmeSeedHarnessRefusalReason.LlmAuthorityOverclaimed, refused.RefusalReasons);
        Assert.Contains(CmeSeedHarnessRefusalReason.SliLispOverclaimed, refused.RefusalReasons);
        Assert.Contains(CmeSeedHarnessRefusalReason.RtmeOverclaimed, refused.RefusalReasons);
        Assert.Contains(CmeSeedHarnessRefusalReason.PrimeCrypticMutationOverclaimed, refused.RefusalReasons);
        Assert.Contains(CmeSeedHarnessRefusalReason.GoverningCmeActivationOverclaimed, refused.RefusalReasons);
        Assert.Contains(CmeSeedHarnessRefusalReason.SanctuaryActualOverclaimed, refused.RefusalReasons);
        Assert.Contains(CmeSeedHarnessRefusalReason.RuntimeAuthorityOverclaimed, refused.RefusalReasons);
    }

    [Fact]
    public void Snapshot_Types_Do_Not_Expose_Service_Executor_Runtime_Llm_Provider_Or_Rtme_Owners()
    {
        var harnessTypes = typeof(CmeSeedHarnessThoughtFieldSnapshotRecord)
            .Assembly
            .GetTypes()
            .Where(static type =>
                type.Namespace == "San.Common" &&
                type.Name.Contains("CmeSeedHarness", StringComparison.Ordinal))
            .Select(static type => type.Name)
            .ToArray();

        Assert.DoesNotContain(harnessTypes, name => name.Contains("Service", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(harnessTypes, name => name.Contains("Executor", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(harnessTypes, name => name.Contains("RuntimeOwner", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(harnessTypes, name => name.Contains("ProviderClient", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(harnessTypes, name => name.Contains("RtmeRunner", StringComparison.OrdinalIgnoreCase));
    }

    private static void AssertDefaultDenied(IReadOnlyList<CmeSeedHarnessDeniedCapability> deniedCapabilities)
    {
        var allCapabilities = Enum.GetValues<CmeSeedHarnessDeniedCapability>();

        Assert.Equal(allCapabilities.Length, deniedCapabilities.Count);

        foreach (var capability in allCapabilities)
        {
            Assert.Contains(capability, deniedCapabilities);
        }
    }
}
