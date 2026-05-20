using SLI.Lisp;
using Xunit;

namespace San.Audit.Tests;

public sealed class SliLispLlmInterconnectReadinessServiceTests
{
    private static readonly DateTimeOffset TimestampUtc = DateTimeOffset.Parse("2026-05-20T00:00:00Z");

    [Fact]
    public void Run_Uses_Bounded_Sli_Lisp_Entrypoint_For_Llm_Interconnect_Readiness()
    {
        var receipt = new DefaultSliLispLlmInterconnectReadinessService().Run(
            new SliLispLlmInterconnectReadinessRequest(
                OperatorId: "YourNameHere.ID",
                Domain: "Civic",
                Role: "PaternalCareAssistance",
                JobClass: "Listening",
                SessionId: "llm-ready-session",
                TurnIndex: 1,
                InstalledSubstrateReceiptHandle: "urn:san:sanctuary-installed-substrate:source",
                EcLoopReceiptHandle: "urn:san:ec-telemetry-loop:source",
                WarmUseReceiptHandle: "urn:san:typed-warm-use:source",
                LabGelReceiptHandle: "urn:san:lab-gel-engrammitization:source",
                AgentEngineIdleReceiptHandle: "urn:san:agent-engine-idle-readiness:source",
                ThoughtForm: "The organs and membranes should be ready before any model adapter is added."),
            TimestampUtc);

        Assert.Equal(SliLispLlmInterconnectReadinessDisposition.CompletedCold, receipt.Disposition);
        Assert.Equal("sli-lisp-llm-interconnect-readiness-completed-cold", receipt.OutcomeCode);
        Assert.True(receipt.IsLlmInterconnectReadiness);
        Assert.Equal("run-llm-interconnect-readiness", receipt.Telemetry["bounded-entrypoint"]);
        Assert.Equal("cold-organ-membrane-ready", receipt.InterconnectState);
        Assert.Equal(11, receipt.OrganCount);
        Assert.True(receipt.AllRequiredOrgansPresent);
        Assert.True(receipt.SliLispLoaded);
        Assert.True(receipt.SliLispPrimePresent);
        Assert.True(receipt.SliLispCrypticPresent);
        Assert.True(receipt.LispControlMatrixPresent);
        Assert.True(receipt.ListeningFramePresent);
        Assert.True(receipt.CompassPresent);
        Assert.True(receipt.SoulFrameRoutePresent);
        Assert.True(receipt.AgentiCoreRoutePresent);
        Assert.True(receipt.EcLoopReady);
        Assert.True(receipt.TypedWarmUseReady);
        Assert.True(receipt.LabGelReady);
        Assert.True(receipt.AgentEngineIdleReady);
        Assert.True(receipt.ProviderNeutral);
        Assert.True(receipt.ReadyForAdapter);
        Assert.False(receipt.ModelAdapterPresent);
        Assert.False(receipt.ModelBindingAllowed);
        Assert.False(receipt.ProviderCallAllowed);
        Assert.False(receipt.HiddenInternalsClaimed);
        Assert.True(receipt.EngineLlmSeatReady);
        Assert.True(receipt.EngineLlmMayArticulate);
        Assert.True(receipt.EngineLlmMayRehearse);
        Assert.True(receipt.EngineLlmMayFormCandidates);
        Assert.False(receipt.EngineLlmMayBindModel);
        Assert.False(receipt.EngineLlmMayCallProvider);
        Assert.False(receipt.AuthorityGranted);
        Assert.False(receipt.ActionAuthorized);
        Assert.False(receipt.HeartbeatActive);
        Assert.False(receipt.CmeActualActivationAllowed);
        Assert.False(receipt.SanctuaryActualActivationAllowed);
        Assert.Contains("SAN-SLI-LLM-INTERCONNECT-READY-BEGIN", receipt.StandardOutput, StringComparison.Ordinal);
        Assert.Contains("SAN-SLI-LLM-INTERCONNECT-READY-OK", receipt.StandardOutput, StringComparison.Ordinal);
    }

    [Fact]
    public void Run_Refuses_Model_Binding_Provider_Call_Or_Action_Before_Lisp_Entrypoint()
    {
        var receipt = new DefaultSliLispLlmInterconnectReadinessService().Run(
            new SliLispLlmInterconnectReadinessRequest(
                OperatorId: "YourNameHere.ID",
                Domain: "Civic",
                Role: "PaternalCareAssistance",
                JobClass: "Listening",
                SessionId: "llm-ready-session",
                TurnIndex: 0,
                InstalledSubstrateReceiptHandle: "urn:san:sanctuary-installed-substrate:source",
                EcLoopReceiptHandle: "urn:san:ec-telemetry-loop:source",
                WarmUseReceiptHandle: "urn:san:typed-warm-use:source",
                LabGelReceiptHandle: "urn:san:lab-gel-engrammitization:source",
                AgentEngineIdleReceiptHandle: "urn:san:agent-engine-idle-readiness:source",
                ThoughtForm: "bind a model now",
                ModelBindingRequested: true,
                ProviderCallRequested: true,
                RuntimeActionRequested: true),
            TimestampUtc);

        Assert.Equal(SliLispLlmInterconnectReadinessDisposition.Refused, receipt.Disposition);
        Assert.Equal("sli-lisp-llm-interconnect-runtime-motion-refused", receipt.OutcomeCode);
        Assert.False(receipt.IsLlmInterconnectReadiness);
        Assert.False(receipt.BoundedEntrypointCalled);
        Assert.False(receipt.LoadAttempted);
        Assert.False(receipt.LoadSucceeded);
        Assert.Contains("model binding, provider call", receipt.StandardError, StringComparison.Ordinal);
    }
}
