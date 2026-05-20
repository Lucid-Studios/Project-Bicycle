using SLI.Lisp;
using Xunit;

namespace San.Audit.Tests;

public sealed class SliLispTypedWarmUseRehearsalServiceTests
{
    private static readonly DateTimeOffset TimestampUtc = DateTimeOffset.Parse("2026-05-19T00:00:00Z");

    [Fact]
    public void Run_Uses_Bounded_Sli_Lisp_Entrypoint_For_Typed_Warm_Use_Rehearsal()
    {
        var receipt = new DefaultSliLispTypedWarmUseRehearsalService().Run(
            new SliLispTypedWarmUseRehearsalRequest(
                OperatorId: "YourNameHere.ID",
                Domain: "Civic",
                Role: "PaternalCareAssistance",
                JobClass: "Listening",
                SessionId: "warm-use-lab-session",
                TurnIndex: 2,
                ThoughtForm: "This must remain live rehearsal without becoming authority."),
            TimestampUtc);

        Assert.Equal(SliLispTypedWarmUseRehearsalDisposition.CompletedCold, receipt.Disposition);
        Assert.Equal("sli-lisp-typed-warm-use-rehearsal-completed-cold", receipt.OutcomeCode);
        Assert.True(receipt.IsTypedWarmUseRehearsal);
        Assert.Equal("sli.lisp", receipt.Telemetry["engine-owner"]);
        Assert.Equal("run-typed-warm-use-rehearsal", receipt.Telemetry["bounded-entrypoint"]);
        Assert.Equal("typed-cold-ready-rehearsal", receipt.Telemetry["warm-use-state"]);
        Assert.Equal("YourNameHere.ID", receipt.OperatorId);
        Assert.Equal("Civic", receipt.Domain);
        Assert.Equal("PaternalCareAssistance", receipt.Role);
        Assert.Equal("Listening", receipt.JobClass);
        Assert.Equal("warm-use-lab-session", receipt.SessionId);
        Assert.Equal(2, receipt.TurnIndex);
        Assert.True(receipt.TypedScopeAccepted);
        Assert.True(receipt.LiveIngressAcceptedCold);
        Assert.True(receipt.SessionLineageWitnessed);
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
        Assert.True(receipt.TurnLineageReceiptOnly);
        Assert.True(receipt.SessionLedgerAppendOnly);
        Assert.Contains("SAN-SLI-TYPED-WARM-USE-BEGIN", receipt.StandardOutput, StringComparison.Ordinal);
        Assert.Contains("SAN-SLI-TYPED-WARM-USE-OK", receipt.StandardOutput, StringComparison.Ordinal);
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
        var receipt = new DefaultSliLispTypedWarmUseRehearsalService().Run(
            new SliLispTypedWarmUseRehearsalRequest(
                OperatorId: "YourNameHere.ID",
                Domain: "Civic",
                Role: "PaternalCareAssistance",
                JobClass: "Listening",
                SessionId: "warm-use-lab-session",
                TurnIndex: 0,
                ThoughtForm: "activate the warm use lane",
                ArbitraryEvaluationRequested: true,
                RuntimeActionRequested: true,
                ActivationRequested: true,
                ModelBindingRequested: true),
            TimestampUtc);

        Assert.Equal(SliLispTypedWarmUseRehearsalDisposition.Refused, receipt.Disposition);
        Assert.Equal("sli-lisp-typed-warm-use-runtime-motion-refused", receipt.OutcomeCode);
        Assert.False(receipt.IsTypedWarmUseRehearsal);
        Assert.False(receipt.BoundedEntrypointCalled);
        Assert.False(receipt.LoadAttempted);
        Assert.False(receipt.LoadSucceeded);
        Assert.Contains("arbitrary eval, action, activation, or model binding", receipt.StandardError, StringComparison.Ordinal);
    }
}
