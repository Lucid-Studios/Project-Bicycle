using SLI.Lisp;
using Xunit;

namespace San.Audit.Tests;

public sealed class SliLispToolBodyIdleStateServiceTests
{
    private static readonly DateTimeOffset TimestampUtc = DateTimeOffset.Parse("2026-05-20T00:00:00Z");

    [Fact]
    public void Run_Uses_Bounded_Sli_Lisp_Entrypoint_For_Tool_Body_Idle_Without_Llm_Maintenance()
    {
        var receipt = new DefaultSliLispToolBodyIdleStateService().Run(
            new SliLispToolBodyIdleStateRequest(
                OperatorId: "YourNameHere.ID",
                Domain: "Civic",
                Role: "PaternalCareAssistance",
                JobClass: "Listening",
                SessionId: "tool-body-idle-session",
                TurnIndex: 1,
                InstalledSubstrateReceiptHandle: "urn:san:sanctuary-installed-substrate:source",
                EcLoopReceiptHandle: "urn:san:ec-telemetry-loop:source",
                WarmUseReceiptHandle: "urn:san:typed-warm-use:source",
                LabGelReceiptHandle: "urn:san:lab-gel-engrammitization:source",
                EngramCandidateHandle: "urn:san:engram-candidate:source",
                EngramClosureReceiptHandle: "urn:san:engram-closure:source",
                LabGelReadbackReceiptHandle: "urn:san:lab-gel-readback:source",
                ThoughtForm: "The tool body can idle without an LLM maintaining its state."),
            TimestampUtc);

        Assert.Equal(SliLispToolBodyIdleStateDisposition.CompletedCold, receipt.Disposition);
        Assert.Equal("sli-lisp-tool-body-idle-state-completed-cold", receipt.OutcomeCode);
        Assert.True(receipt.IsToolBodyIdleState);
        Assert.Equal("run-tool-body-idle-state", receipt.Telemetry["bounded-entrypoint"]);
        Assert.Equal("cold-sanctuary-maintained-idle", receipt.IdleState);
        Assert.True(receipt.MaintainedBySanctuary);
        Assert.False(receipt.MaintainedByLlm);
        Assert.False(receipt.LlmMaintenanceRequired);
        Assert.False(receipt.LlmAdapterRequired);
        Assert.True(receipt.ReadyForLlmAdapter);
        Assert.True(receipt.CanAcceptFutureRider);
        Assert.True(receipt.GovernanceSlmCandidateDesirable);
        Assert.True(receipt.GovernanceSlmRoutingSwitchCandidate);
        Assert.True(receipt.GovernanceSlmIntelligentSwitchCandidate);
        Assert.False(receipt.GovernanceSlmPresent);
        Assert.False(receipt.GovernanceSlmRequiredForIdle);
        Assert.True(receipt.GovernanceSlmMayDiscriminateEscalation);
        Assert.True(receipt.GovernanceSlmMayDiscernActionReadiness);
        Assert.False(receipt.GovernanceSlmDiscernmentAuthorizesAction);
        Assert.False(receipt.GovernanceSlmMayAuthorizeAction);
        Assert.False(receipt.ModelAdapterPresent);
        Assert.False(receipt.ModelBindingAllowed);
        Assert.False(receipt.ProviderCallAllowed);
        Assert.False(receipt.TickLoopRunning);
        Assert.False(receipt.TickMaintainedByLlm);
        Assert.True(receipt.IdleLoopHeld);
        Assert.True(receipt.ReturnToPrimeHeld);
        Assert.True(receipt.OperatorReentryAvailable);
        Assert.True(receipt.EcMaintainedInLisp);
        Assert.True(receipt.LocalEcHoldAvailable);
        Assert.False(receipt.EngineCallRequired);
        Assert.False(receipt.LlmEngineCallRequired);
        Assert.False(receipt.ExternalEngineCallRequired);
        Assert.Equal(11, receipt.OrganCount);
        Assert.True(receipt.AllRequiredOrgansPresent);
        Assert.True(receipt.GoverningCmeCSharpBodiesBuilt);
        Assert.True(receipt.GoverningCmeActualizedCold);
        Assert.True(receipt.PrimeGoverningCmeBuilt);
        Assert.True(receipt.CrypticGoverningCmeBuilt);
        Assert.True(receipt.StewardGoverningCmeBuilt);
        Assert.True(receipt.GoverningCmeSliLispActualizationSurfacesReady);
        Assert.True(receipt.GoverningCmeMaintainsIdleState);
        Assert.True(receipt.GoverningHeartbeatHealthy);
        Assert.True(receipt.BondedCmeCallAvailable);
        Assert.True(receipt.SanctuaryGovernanceMonitoringReady);
        Assert.True(receipt.EcLoopReady);
        Assert.True(receipt.TypedWarmUseReady);
        Assert.True(receipt.LabGelReady);
        Assert.False(receipt.AgentEngineIdleRequired);
        Assert.True(receipt.SourceLineageHeld);
        Assert.True(receipt.SourceEngramClosureAcceptedCold);
        Assert.True(receipt.SourceLabGelReadbackAcceptedCold);
        Assert.True(receipt.AuthorityGrantAbsent);
        Assert.True(receipt.ActionExecutorLocked);
        Assert.True(receipt.HeartbeatLocked);
        Assert.True(receipt.CmeActualLocked);
        Assert.True(receipt.SanctuaryActualLocked);
        Assert.False(receipt.AuthorityGranted);
        Assert.False(receipt.ActionAuthorized);
        Assert.False(receipt.HeartbeatActive);
        Assert.False(receipt.CmeActualActivationAllowed);
        Assert.False(receipt.SanctuaryActualActivationAllowed);
        Assert.Contains("SAN-SLI-TOOL-BODY-IDLE-BEGIN", receipt.StandardOutput, StringComparison.Ordinal);
        Assert.Contains("SAN-SLI-TOOL-BODY-IDLE-OK", receipt.StandardOutput, StringComparison.Ordinal);
    }

    [Fact]
    public void Run_Refuses_Llm_Maintenance_Tick_Model_Binding_Or_Action_Before_Lisp_Entrypoint()
    {
        var receipt = new DefaultSliLispToolBodyIdleStateService().Run(
            new SliLispToolBodyIdleStateRequest(
                OperatorId: "YourNameHere.ID",
                Domain: "Civic",
                Role: "PaternalCareAssistance",
                JobClass: "Listening",
                SessionId: "tool-body-idle-session",
                TurnIndex: 0,
                InstalledSubstrateReceiptHandle: "urn:san:sanctuary-installed-substrate:source",
                EcLoopReceiptHandle: "urn:san:ec-telemetry-loop:source",
                WarmUseReceiptHandle: "urn:san:typed-warm-use:source",
                LabGelReceiptHandle: "urn:san:lab-gel-engrammitization:source",
                EngramCandidateHandle: "urn:san:engram-candidate:source",
                EngramClosureReceiptHandle: "urn:san:engram-closure:source",
                LabGelReadbackReceiptHandle: "urn:san:lab-gel-readback:source",
                ThoughtForm: "let the model maintain idle",
                LlmMaintenanceRequested: true,
                TickLoopRequested: true,
                ModelBindingRequested: true,
                RuntimeActionRequested: true),
            TimestampUtc);

        Assert.Equal(SliLispToolBodyIdleStateDisposition.Refused, receipt.Disposition);
        Assert.Equal("sli-lisp-tool-body-idle-runtime-motion-refused", receipt.OutcomeCode);
        Assert.False(receipt.IsToolBodyIdleState);
        Assert.False(receipt.BoundedEntrypointCalled);
        Assert.False(receipt.LoadAttempted);
        Assert.False(receipt.LoadSucceeded);
        Assert.Contains("LLM maintenance, tick loop", receipt.StandardError, StringComparison.Ordinal);
    }
}
