using SLI.Lisp;
using Xunit;

namespace San.Audit.Tests;

public sealed class SliLispLlmTickCycleServiceTests
{
    private static readonly DateTimeOffset TimestampUtc = DateTimeOffset.Parse("2026-05-20T00:00:00Z");

    [Fact]
    public void Run_Processes_Deterministic_Adapter_Tick_Through_Bounded_Sli_Lisp_Entrypoint()
    {
        var receipt = new DefaultSliLispLlmTickCycleService().Run(
            new SliLispLlmTickCycleRequest(
                OperatorId: "YourNameHere.ID",
                Domain: "Civic",
                Role: "PaternalCareAssistance",
                JobClass: "Listening",
                SessionId: "llm-tick-session",
                TickIndex: 1,
                SourceLlmInterconnectReadinessReceiptHandle: "urn:san:llm-interconnect-readiness:source",
                PriorTickReceiptHandle: "none",
                AdapterKind: "deterministic-harness",
                AdapterResponseReceiptHandle: "urn:san:engine-llm-adapter:deterministic:source",
                AdapterOutput: "Adapter output is witnessed as predicate evidence only.",
                ThoughtForm: "Run one cold LLM tick without binding a provider.",
                SourceEngramClosureReceiptHandle: "urn:san:engram-closure:source"),
            TimestampUtc);

        Assert.Equal(SliLispLlmTickCycleDisposition.CompletedCold, receipt.Disposition);
        Assert.Equal("sli-lisp-llm-tick-cycle-completed-cold", receipt.OutcomeCode);
        Assert.True(receipt.IsLlmTickCycle);
        Assert.Equal("run-llm-tick-cycle", receipt.Telemetry["bounded-entrypoint"]);
        Assert.Equal("cold-adapter-tick-witnessed", receipt.TickState);
        Assert.True(receipt.TickLoopRunning);
        Assert.Equal("deterministic-harness", receipt.TickLoopKind);
        Assert.True(receipt.SourceLlmInterconnectReady);
        Assert.True(receipt.SourceEngramClosureReady);
        Assert.Equal("urn:san:engram-closure:source", receipt.SourceEngramClosureReceiptHandle);
        Assert.True(receipt.ReadyForAdapter);
        Assert.True(receipt.ModelAdapterPresent);
        Assert.True(receipt.DeterministicHarnessAdapter);
        Assert.True(receipt.AdapterResponseWitnessed);
        Assert.True(receipt.AdapterResponseBounded);
        Assert.True(receipt.SliLispProcessedTick);
        Assert.True(receipt.PredicateResidueProduced);
        Assert.True(receipt.PredicateResiduePreEngramOnly);
        Assert.False(receipt.PredicateResidueAdmittedEngram);
        Assert.True(receipt.FirstTickOrigin);
        Assert.False(receipt.PriorTickLinked);
        Assert.False(receipt.ModelBindingAllowed);
        Assert.False(receipt.ProviderCallAllowed);
        Assert.False(receipt.HiddenInternalsClaimed);
        Assert.False(receipt.AuthorityGranted);
        Assert.False(receipt.ActionAuthorized);
        Assert.False(receipt.HeartbeatActive);
        Assert.False(receipt.CmeActualActivationAllowed);
        Assert.False(receipt.SanctuaryActualActivationAllowed);
        Assert.Contains("SAN-SLI-LLM-TICK-BEGIN", receipt.StandardOutput, StringComparison.Ordinal);
        Assert.Contains("SAN-SLI-LLM-TICK-OK", receipt.StandardOutput, StringComparison.Ordinal);
    }

    [Fact]
    public void Run_Refuses_Model_Binding_Provider_Call_Or_Action_Before_Lisp_Entrypoint()
    {
        var receipt = new DefaultSliLispLlmTickCycleService().Run(
            new SliLispLlmTickCycleRequest(
                OperatorId: "YourNameHere.ID",
                Domain: "Civic",
                Role: "PaternalCareAssistance",
                JobClass: "Listening",
                SessionId: "llm-tick-session",
                TickIndex: 0,
                SourceLlmInterconnectReadinessReceiptHandle: "urn:san:llm-interconnect-readiness:source",
                PriorTickReceiptHandle: "none",
                AdapterKind: "deterministic-harness",
                AdapterResponseReceiptHandle: "urn:san:engine-llm-adapter:deterministic:source",
                AdapterOutput: "bind a model now",
                ThoughtForm: "bind a model now",
                ModelBindingRequested: true,
                ProviderCallRequested: true,
                RuntimeActionRequested: true),
            TimestampUtc);

        Assert.Equal(SliLispLlmTickCycleDisposition.Refused, receipt.Disposition);
        Assert.Equal("sli-lisp-llm-tick-runtime-motion-refused", receipt.OutcomeCode);
        Assert.False(receipt.IsLlmTickCycle);
        Assert.False(receipt.BoundedEntrypointCalled);
        Assert.False(receipt.LoadAttempted);
        Assert.False(receipt.LoadSucceeded);
        Assert.Contains("model binding, provider call", receipt.StandardError, StringComparison.Ordinal);
    }
}
