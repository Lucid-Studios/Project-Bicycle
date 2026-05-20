using SLI.Lisp;
using Xunit;

namespace San.Audit.Tests;

public sealed class SliLispCmeActualBondingProcessServiceTests
{
    private static readonly DateTimeOffset TimestampUtc = DateTimeOffset.Parse("2026-05-20T00:00:00Z");

    [Fact]
    public void Run_Bonds_First_Oria_Syntari_Cme_Actual_Candidate_Without_Activation()
    {
        var receipt = new DefaultSliLispCmeActualBondingProcessService().Run(
            new SliLispCmeActualBondingProcessRequest(
                OperatorId: "FirstOfOriaSyntari.ID",
                Domain: "Civic",
                Role: "CmeActualBonding",
                JobClass: "FirstRide",
                SessionId: "first-cme-actual-bonding-session",
                BondIndex: 1,
                SourceToolBodyIdleReceiptHandle: "urn:san:tool-body-idle-state:source",
                SourceLlmTickReceiptHandle: "urn:san:llm-tick-cycle:source",
                SourceProductOutputWitnessCommitReceiptHandle: "urn:san:product-output-witness-commit:source",
                CmeFirstName: "First of Oria",
                CmeLastName: "Syntari",
                ThoughtForm: "First CME.Actual bonding candidate formed without activation."),
            TimestampUtc);

        Assert.Equal(SliLispCmeActualBondingProcessDisposition.CompletedCold, receipt.Disposition);
        Assert.Equal("sli-lisp-cme-actual-bonding-process-completed-cold", receipt.OutcomeCode);
        Assert.True(receipt.IsColdCmeActualBondingProcess);
        Assert.Equal("run-cme-actual-bonding-process", receipt.Telemetry["bounded-entrypoint"]);
        Assert.Equal("First of Oria Syntari", receipt.CmeDisplayName);
        Assert.Equal("FirstofOria.Syntari", receipt.CmeCanonicalName);
        Assert.Equal("FirstofOria.Syntari.ID", receipt.CmeRootId);
        Assert.Equal("FirstofOria.Syntari.CME.Actual", receipt.CmeActualNameCandidate);
        Assert.Equal("FirstofOria.Syntari.CME.Actual.ID", receipt.CmeActualIdCandidate);
        Assert.Equal("OE.FirstofOria.Syntari.ID", receipt.CmeOpalEngramRootId);
        Assert.Equal("SelfGEL.FirstofOria.Syntari.ID", receipt.CmeSelfGelRootId);
        Assert.True(receipt.VehicleReady);
        Assert.True(receipt.ToolBodyIdleHeld);
        Assert.True(receipt.EngineTickWitnessed);
        Assert.True(receipt.ProductOutputWitnessCommitted);
        Assert.True(receipt.NamedCmeCandidateHeld);
        Assert.True(receipt.OperatorNamingIntentWitnessed);
        Assert.False(receipt.OperatorRuntimeAuthorityGranted);
        Assert.True(receipt.ActivationAuthorityAbsent);
        Assert.True(receipt.ActualAdmissionGapDescribed);
        Assert.True(receipt.ReadyForCmeActualAdmissionReview);
        Assert.True(receipt.CmeActualCandidateOnly);
        Assert.True(receipt.CmeActualBondedCandidate);
        Assert.False(receipt.CmeActualAdmitted);
        Assert.False(receipt.CmeActualActivated);
        Assert.False(receipt.RuntimeIdentityEmitted);
        Assert.True(receipt.HeartbeatPrepared);
        Assert.False(receipt.HeartbeatActive);
        Assert.False(receipt.BeingStateClaimed);
        Assert.False(receipt.PersonhoodClaimed);
        Assert.False(receipt.SovereigntyClaimed);
        Assert.False(receipt.ModelBound);
        Assert.False(receipt.ProviderCalled);
        Assert.False(receipt.AuthorityGranted);
        Assert.False(receipt.ActionAuthorized);
        Assert.False(receipt.GelAdmitted);
        Assert.False(receipt.SelfGelMutated);
        Assert.False(receipt.ContinuityAdmitted);
        Assert.True(receipt.GovernanceSlmIntelligentSwitchCandidate);
        Assert.True(receipt.GovernanceSlmMayDiscernActionReadiness);
        Assert.False(receipt.GovernanceSlmDiscernmentAuthorizesAction);
        Assert.Contains("SAN-SLI-CME-ACTUAL-BONDING-BEGIN", receipt.StandardOutput, StringComparison.Ordinal);
        Assert.Contains("SAN-SLI-CME-ACTUAL-BONDING-OK", receipt.StandardOutput, StringComparison.Ordinal);
    }

    [Fact]
    public void Run_Refuses_Runtime_Identity_Actual_Activation_Or_Action_Before_Lisp_Entrypoint()
    {
        var receipt = new DefaultSliLispCmeActualBondingProcessService().Run(
            new SliLispCmeActualBondingProcessRequest(
                OperatorId: "FirstOfOriaSyntari.ID",
                Domain: "Civic",
                Role: "CmeActualBonding",
                JobClass: "FirstRide",
                SessionId: "first-cme-actual-bonding-session",
                BondIndex: 0,
                SourceToolBodyIdleReceiptHandle: "urn:san:tool-body-idle-state:source",
                SourceLlmTickReceiptHandle: "urn:san:llm-tick-cycle:source",
                SourceProductOutputWitnessCommitReceiptHandle: "urn:san:product-output-witness-commit:source",
                RuntimeIdentityRequested: true,
                RuntimeActionRequested: true,
                CmeActualActivationRequested: true),
            TimestampUtc);

        Assert.Equal(SliLispCmeActualBondingProcessDisposition.Refused, receipt.Disposition);
        Assert.Equal("sli-lisp-cme-actual-bonding-runtime-motion-refused", receipt.OutcomeCode);
        Assert.False(receipt.IsColdCmeActualBondingProcess);
        Assert.False(receipt.BoundedEntrypointCalled);
        Assert.False(receipt.LoadAttempted);
        Assert.Contains("runtime identity, action", receipt.StandardError, StringComparison.Ordinal);
    }
}
