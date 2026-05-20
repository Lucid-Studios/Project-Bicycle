using SLI.Lisp;
using Xunit;

namespace San.Audit.Tests;

public sealed class SliLispEcTelemetryLoopServiceTests
{
    private static readonly DateTimeOffset TimestampUtc = DateTimeOffset.Parse("2026-05-19T00:00:00Z");

    [Fact]
    public void Run_Uses_Bounded_Sli_Lisp_Entrypoint_For_Ec_Telemetry_Loop()
    {
        var receipt = new DefaultSliLispEcTelemetryLoopService().Run(
            new SliLispEcTelemetryLoopRequest(
                ThoughtForm: "Can the body think about its own predicate pressure without becoming authority?"),
            TimestampUtc);

        Assert.Equal(SliLispEcTelemetryLoopDisposition.CompletedCold, receipt.Disposition);
        Assert.Equal("sli-lisp-ec-telemetry-loop-completed-cold", receipt.OutcomeCode);
        Assert.True(receipt.IsColdEcTelemetryLoop);
        Assert.Equal("sli.lisp", receipt.Telemetry["engine-owner"]);
        Assert.Equal("run-ec-telemetry-loop", receipt.Telemetry["bounded-entrypoint"]);
        Assert.True(receipt.BoundedEntrypointCalled);
        Assert.True(receipt.LoadSucceeded);
        Assert.True(receipt.ColdEngineLoopCompleted);
        Assert.True(receipt.ListeningFrameReceived);
        Assert.True(receipt.SliMembraneInterpretedPredicatePressure);
        Assert.True(receipt.CompassOrientedPressure);
        Assert.True(receipt.SoulFrameReceivedListeningFrame);
        Assert.True(receipt.AgentiCoreReceivedCompassPressure);
        Assert.True(receipt.ThinkingAboutThinkingTelemetryProduced);
        Assert.True(receipt.PreEngramResidueProduced);
        Assert.Equal(6, receipt.PreEngramResidueCount);
        Assert.Equal(["semantic", "pressure", "witness", "governance", "morphology", "return"], receipt.PreEngramResidueClasses);
        Assert.True(receipt.StewardReviewed);
        Assert.Contains("SAN-SLI-EC-TELEMETRY-LOOP-BEGIN", receipt.StandardOutput, StringComparison.Ordinal);
        Assert.Contains("SAN-SLI-EC-TELEMETRY-LOOP-OK", receipt.StandardOutput, StringComparison.Ordinal);
        Assert.Contains("engine-owner=sli.lisp", receipt.StandardOutput, StringComparison.Ordinal);
        Assert.False(receipt.EngramAdmissionAllowed);
        Assert.False(receipt.MemoryAdmissionAllowed);
        Assert.False(receipt.SelfGelMutationAllowed);
        Assert.False(receipt.ContinuityAdmissionAllowed);
        Assert.False(receipt.AuthorityGranted);
        Assert.False(receipt.ActionAuthorized);
        Assert.False(receipt.ModelBindingAllowed);
        Assert.False(receipt.ArbitraryEvaluationAllowed);
        Assert.False(receipt.RuntimeActionAllowed);
        Assert.False(receipt.ActivationAllowed);
        Assert.False(receipt.CmeActualActivationAllowed);
        Assert.False(receipt.SanctuaryActualActivationAllowed);
    }

    [Fact]
    public void Run_Refuses_Forbidden_Motion_Before_Lisp_Entrypoint()
    {
        var receipt = new DefaultSliLispEcTelemetryLoopService().Run(
            new SliLispEcTelemetryLoopRequest(
                ThoughtForm: "activate the engine",
                ArbitraryEvaluationRequested: true,
                RuntimeActionRequested: true,
                ActivationRequested: true,
                ModelBindingRequested: true),
            TimestampUtc);

        Assert.Equal(SliLispEcTelemetryLoopDisposition.Refused, receipt.Disposition);
        Assert.Equal("sli-lisp-ec-loop-runtime-motion-refused", receipt.OutcomeCode);
        Assert.False(receipt.IsColdEcTelemetryLoop);
        Assert.False(receipt.BoundedEntrypointCalled);
        Assert.False(receipt.LoadAttempted);
        Assert.False(receipt.LoadSucceeded);
        Assert.False(receipt.ColdEngineLoopCompleted);
        Assert.Contains("arbitrary eval, action, activation, or model binding", receipt.StandardError, StringComparison.Ordinal);
    }
}
