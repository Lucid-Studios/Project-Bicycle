using San.Common;
using Xunit;

namespace San.Audit.Tests;

public sealed class FirstUseAdmissionEnactmentReferenceDataTests
{
    [Fact]
    public void Passive_Contracts_Expose_Admission_And_Enactment_Types()
    {
        Assert.Contains(FirstUseAdmissionDisposition.Ready, Enum.GetValues<FirstUseAdmissionDisposition>());
        Assert.Contains(FirstUseAdmissionDisposition.Held, Enum.GetValues<FirstUseAdmissionDisposition>());
        Assert.Contains(FirstUseAdmissionDisposition.Refused, Enum.GetValues<FirstUseAdmissionDisposition>());

        Assert.Contains(FirstUseEnactmentDisposition.Prepared, Enum.GetValues<FirstUseEnactmentDisposition>());
        Assert.Contains(FirstUseEnactmentDisposition.Held, Enum.GetValues<FirstUseEnactmentDisposition>());
        Assert.Contains(FirstUseEnactmentDisposition.Refused, Enum.GetValues<FirstUseEnactmentDisposition>());

        Assert.Contains(FirstUseAdmissionRefusalReason.EligibilityNotReady, Enum.GetValues<FirstUseAdmissionRefusalReason>());
        Assert.Contains(FirstUseAdmissionRefusalReason.RuntimeOrGovernanceOverclaimed, Enum.GetValues<FirstUseAdmissionRefusalReason>());
        Assert.Contains(FirstUseEnactmentRefusalReason.RtmeOrSliLispOverclaimed, Enum.GetValues<FirstUseEnactmentRefusalReason>());
        Assert.Contains(FirstUseEnactmentRefusalReason.SanctuaryActualOrCradleGelOverclaimed, Enum.GetValues<FirstUseEnactmentRefusalReason>());
    }

    [Fact]
    public void Canonical_Reference_Data_Includes_All_Admission_And_Enactment_Postures()
    {
        Assert.Contains(FirstUseAdmissionEnactmentReferenceData.CanonicalAdmissionRecords, record => record.Disposition == FirstUseAdmissionDisposition.Ready);
        Assert.Contains(FirstUseAdmissionEnactmentReferenceData.CanonicalAdmissionRecords, record => record.Disposition == FirstUseAdmissionDisposition.Held);
        Assert.Contains(FirstUseAdmissionEnactmentReferenceData.CanonicalAdmissionRecords, record => record.Disposition == FirstUseAdmissionDisposition.Refused);

        Assert.Contains(FirstUseAdmissionEnactmentReferenceData.CanonicalEnactmentRecords, record => record.Disposition == FirstUseEnactmentDisposition.Prepared);
        Assert.Contains(FirstUseAdmissionEnactmentReferenceData.CanonicalEnactmentRecords, record => record.Disposition == FirstUseEnactmentDisposition.Held);
        Assert.Contains(FirstUseAdmissionEnactmentReferenceData.CanonicalEnactmentRecords, record => record.Disposition == FirstUseEnactmentDisposition.Refused);
    }

    [Fact]
    public void Ready_Admission_Prepares_Session_Only()
    {
        var admission = FirstUseAdmissionEnactmentReferenceData.ReadyAdmission;

        Assert.Equal(FirstUseAdmissionDisposition.Ready, admission.Disposition);
        Assert.Equal(FirstUseEligibilityReferenceData.ReadyReceipt.ReceiptHandle, admission.EligibilityRef);
        Assert.Equal(SanctuaryGelFirstFormationAttemptReferenceData.ReadyReceipt.ReceiptHandle, admission.FormationAttemptRef);
        Assert.Contains("preparation of a bounded first-use session only", admission.NonAuthoritySummary, StringComparison.Ordinal);
        Assert.Contains("does not enact first use", admission.NonAuthoritySummary, StringComparison.Ordinal);
        Assert.Contains("RTME", admission.NonAuthoritySummary, StringComparison.Ordinal);
        Assert.Contains("Cradle.GEL", admission.NonAuthoritySummary, StringComparison.Ordinal);
    }

    [Fact]
    public void Prepared_Enactment_Witnesses_Entry_Only()
    {
        var enactment = FirstUseAdmissionEnactmentReferenceData.PreparedEnactment;

        Assert.Equal(FirstUseEnactmentDisposition.Prepared, enactment.Disposition);
        Assert.Equal(FirstUseAdmissionEnactmentReferenceData.ReadyAdmissionReceipt.ReceiptHandle, enactment.AdmissionRef);
        Assert.Contains("witnesses entry posture only", enactment.NonAuthoritySummary, StringComparison.Ordinal);
        Assert.Contains("does not activate runtime transaction authority", enactment.NonAuthoritySummary, StringComparison.Ordinal);
        Assert.Contains("RTME", enactment.NonAuthoritySummary, StringComparison.Ordinal);
        Assert.Contains("SLI.Lisp", enactment.NonAuthoritySummary, StringComparison.Ordinal);
        Assert.Contains("Sanctuary.Actual", enactment.NonAuthoritySummary, StringComparison.Ordinal);
    }

    [Fact]
    public void Refused_Admission_And_Enactment_Preserve_Non_Authority()
    {
        var admission = FirstUseAdmissionEnactmentReferenceData.RefusedAdmission;
        var enactment = FirstUseAdmissionEnactmentReferenceData.RefusedEnactment;

        Assert.Equal(FirstUseAdmissionDisposition.Refused, admission.Disposition);
        Assert.Contains(FirstUseAdmissionRefusalReason.EligibilityNotReady, admission.RefusalReasons);
        Assert.Contains(FirstUseAdmissionRefusalReason.FormationAttemptNotReady, admission.RefusalReasons);
        Assert.Contains(FirstUseAdmissionRefusalReason.RuntimeOrGovernanceOverclaimed, admission.RefusalReasons);

        Assert.Equal(FirstUseEnactmentDisposition.Refused, enactment.Disposition);
        Assert.Contains(FirstUseEnactmentRefusalReason.AdmissionNotReady, enactment.RefusalReasons);
        Assert.Contains(FirstUseEnactmentRefusalReason.RuntimeTransactionOverclaimed, enactment.RefusalReasons);
        Assert.Contains(FirstUseEnactmentRefusalReason.RtmeOrSliLispOverclaimed, enactment.RefusalReasons);
        Assert.Contains(FirstUseEnactmentRefusalReason.ModelSelectionOverclaimed, enactment.RefusalReasons);
        Assert.Contains(FirstUseEnactmentRefusalReason.SanctuaryActualOrCradleGelOverclaimed, enactment.RefusalReasons);
    }

    [Fact]
    public void Reference_Data_Does_Not_Expose_Service_Evaluator_Runtime_Or_Model_Selection()
    {
        var contractAssemblyTypes = typeof(FirstUseAdmissionRecord)
            .Assembly
            .GetTypes()
            .Where(static type => type.Namespace == "San.Common")
            .Select(static type => type.Name)
            .ToArray();

        Assert.DoesNotContain(contractAssemblyTypes, name => name.Contains("FirstUseAdmissionService", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(contractAssemblyTypes, name => name.Contains("FirstUseAdmissionEvaluator", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(contractAssemblyTypes, name => name.Contains("FirstUseRuntime", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(contractAssemblyTypes, name => name.Contains("ModelSelection", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(contractAssemblyTypes, name => name.Contains("CradleGelGeneration", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(contractAssemblyTypes, name => name.Contains("SanctuaryActual", StringComparison.OrdinalIgnoreCase));
    }
}
