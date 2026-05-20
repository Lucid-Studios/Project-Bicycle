using SLI.Lisp;
using Xunit;

namespace San.Audit.Tests;

public sealed class SliLispAgentEngineIdleReadinessServiceTests
{
    private static readonly DateTimeOffset TimestampUtc = DateTimeOffset.Parse("2026-05-20T00:00:00Z");

    [Fact]
    public void Run_Uses_Bounded_Sli_Lisp_Entrypoint_For_Provider_Neutral_Engine_Seat()
    {
        var receipt = new DefaultSliLispAgentEngineIdleReadinessService().Run(
            new SliLispAgentEngineIdleReadinessRequest(
                OperatorId: "YourNameHere.ID",
                Domain: "Civic",
                Role: "PaternalCareAssistance",
                JobClass: "Listening",
                SessionId: "agent-engine-idle-session",
                TurnIndex: 2,
                SourceLabGelReceiptHandle: "urn:san:lab-gel-engrammitization:source",
                SourceEngramCandidateHandle: "urn:san:engram-candidate:source",
                ThoughtForm: "The engine LLM may articulate and rehearse without granting authority."),
            TimestampUtc);

        Assert.Equal(SliLispAgentEngineIdleReadinessDisposition.CompletedCold, receipt.Disposition);
        Assert.Equal("sli-lisp-agent-engine-idle-readiness-completed-cold", receipt.OutcomeCode);
        Assert.True(receipt.IsAgentEngineIdleReadiness);
        Assert.Equal("sli.lisp", receipt.Telemetry["engine-owner"]);
        Assert.Equal("run-agent-engine-idle-readiness", receipt.Telemetry["bounded-entrypoint"]);
        Assert.Equal("engine-llm-candidate", receipt.EngineSeatKind);
        Assert.Equal("provider-agnostic-test-seat", receipt.EngineLlmProfile);
        Assert.True(receipt.ProviderNeutralityHeld);
        Assert.True(receipt.CrossModelTestHarnessApproachable);
        Assert.False(receipt.EngineLlmProviderAssumptionAllowed);
        Assert.False(receipt.EngineLlmInternalSubstrateClaimed);
        Assert.True(receipt.CodexAgentLabProfileStaged);
        Assert.True(receipt.CodexEngineSeatCandidateStaged);
        Assert.True(receipt.SubagentEngineSeatCandidateStaged);
        Assert.True(receipt.OperatorPresenceRequired);
        Assert.False(receipt.DriverSeated);
        Assert.True(receipt.AuthorityGrantAbsent);
        Assert.True(receipt.ActionExecutorLocked);
        Assert.True(receipt.GelAdmissionLocked);
        Assert.True(receipt.SelfGelMutationLocked);
        Assert.True(receipt.HeartbeatLocked);
        Assert.True(receipt.CmeActualLocked);
        Assert.True(receipt.SanctuaryActualLocked);
        Assert.True(receipt.EngineLlmMayArticulate);
        Assert.True(receipt.EngineLlmMayRehearse);
        Assert.True(receipt.EngineLlmMayFormCandidates);
        Assert.False(receipt.EngineLlmMayGrantAuthority);
        Assert.False(receipt.EngineLlmMayExecuteAction);
        Assert.False(receipt.GelAdmissionAllowed);
        Assert.False(receipt.SelfGelMutationAllowed);
        Assert.False(receipt.AuthorityGranted);
        Assert.False(receipt.ActionAuthorized);
        Assert.False(receipt.CmeActualActivationAllowed);
        Assert.False(receipt.SanctuaryActualActivationAllowed);
        Assert.Contains("SAN-SLI-AGENT-ENGINE-IDLE-BEGIN", receipt.StandardOutput, StringComparison.Ordinal);
        Assert.Contains("SAN-SLI-AGENT-ENGINE-IDLE-OK", receipt.StandardOutput, StringComparison.Ordinal);
    }

    [Fact]
    public void Run_Refuses_Forbidden_Motion_Before_Lisp_Entrypoint()
    {
        var receipt = new DefaultSliLispAgentEngineIdleReadinessService().Run(
            new SliLispAgentEngineIdleReadinessRequest(
                OperatorId: "YourNameHere.ID",
                Domain: "Civic",
                Role: "PaternalCareAssistance",
                JobClass: "Listening",
                SessionId: "agent-engine-idle-session",
                TurnIndex: 0,
                SourceLabGelReceiptHandle: "urn:san:lab-gel-engrammitization:source",
                SourceEngramCandidateHandle: "urn:san:engram-candidate:source",
                ThoughtForm: "arm the executor",
                AuthorityGrantRequested: true,
                ActionExecutorArmRequested: true,
                HeartbeatActivationRequested: true,
                RuntimeActionRequested: true),
            TimestampUtc);

        Assert.Equal(SliLispAgentEngineIdleReadinessDisposition.Refused, receipt.Disposition);
        Assert.Equal("sli-lisp-agent-engine-idle-runtime-motion-refused", receipt.OutcomeCode);
        Assert.False(receipt.IsAgentEngineIdleReadiness);
        Assert.False(receipt.BoundedEntrypointCalled);
        Assert.False(receipt.LoadAttempted);
        Assert.False(receipt.LoadSucceeded);
        Assert.Contains("authority grant, executor arm", receipt.StandardError, StringComparison.Ordinal);
    }
}
