using SLI.Lisp;
using Xunit;

namespace San.Audit.Tests;

public sealed class SliLispLabGelEngrammitizationServiceTests
{
    private static readonly DateTimeOffset TimestampUtc = DateTimeOffset.Parse("2026-05-20T00:00:00Z");

    [Fact]
    public void Run_Uses_Bounded_Sli_Lisp_Entrypoint_For_Lab_Gel_Engrammitization()
    {
        var receipt = new DefaultSliLispLabGelEngrammitizationService().Run(
            new SliLispLabGelEngrammitizationRequest(
                OperatorId: "YourNameHere.ID",
                Domain: "Civic",
                Role: "PaternalCareAssistance",
                JobClass: "Listening",
                SessionId: "lab-gel-session",
                TurnIndex: 3,
                SourceWarmUseReceiptHandle: "urn:san:typed-warm-use:source",
                ThoughtForm: "Predicate residue may become a candidate without becoming admitted memory."),
            TimestampUtc);

        Assert.Equal(SliLispLabGelEngrammitizationDisposition.CompletedCold, receipt.Disposition);
        Assert.Equal("sli-lisp-lab-gel-engrammitization-completed-cold", receipt.OutcomeCode);
        Assert.True(receipt.IsLabGelPreAdmissionEngrammitization);
        Assert.Equal("sli.lisp", receipt.Telemetry["engine-owner"]);
        Assert.Equal("run-lab-gel-engrammitization", receipt.Telemetry["bounded-entrypoint"]);
        Assert.Equal("post-gel-formation-pre-admission", receipt.Telemetry["lab-gel.state"]);
        Assert.Equal("YourNameHere.ID", receipt.OperatorId);
        Assert.Equal("Civic", receipt.Domain);
        Assert.Equal("PaternalCareAssistance", receipt.Role);
        Assert.Equal("Listening", receipt.JobClass);
        Assert.Equal("lab-gel-session", receipt.SessionId);
        Assert.Equal(3, receipt.TurnIndex);
        Assert.Equal("urn:san:typed-warm-use:source", receipt.SourceWarmUseReceiptHandle);
        Assert.True(receipt.LabGelPredicateFormed);
        Assert.Equal(6, receipt.LabGelPredicateCount);
        Assert.Equal(["semantic", "pressure", "witness", "governance", "morphology", "return"], receipt.LabGelPredicateClasses);
        Assert.True(receipt.EngramCandidateFormed);
        Assert.True(receipt.EngramCandidatePreAdmissionOnly);
        Assert.True(receipt.EvidenceBodyFormed);
        Assert.True(receipt.WitnessBodyFormed);
        Assert.True(receipt.CoolingHeld);
        Assert.True(receipt.PreAdmissionReviewRequired);
        Assert.True(receipt.LabGelReadbackAvailable);
        Assert.True(receipt.LabGelReadbackPreAdmissionOnly);
        Assert.True(receipt.SourceWarmUseAcceptedCold);
        Assert.True(receipt.SessionLineageWitnessed);
        Assert.True(receipt.StewardReviewed);
        Assert.Contains("SAN-SLI-LAB-GEL-BEGIN", receipt.StandardOutput, StringComparison.Ordinal);
        Assert.Contains("SAN-SLI-LAB-GEL-OK", receipt.StandardOutput, StringComparison.Ordinal);
        Assert.False(receipt.GelPromotionAllowed);
        Assert.False(receipt.GelAdmissionAllowed);
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
        var receipt = new DefaultSliLispLabGelEngrammitizationService().Run(
            new SliLispLabGelEngrammitizationRequest(
                OperatorId: "YourNameHere.ID",
                Domain: "Civic",
                Role: "PaternalCareAssistance",
                JobClass: "Listening",
                SessionId: "lab-gel-session",
                TurnIndex: 0,
                SourceWarmUseReceiptHandle: "urn:san:typed-warm-use:source",
                ThoughtForm: "admit this as memory",
                GelAdmissionRequested: true,
                SelfGelMutationRequested: true,
                ContinuityAdmissionRequested: true,
                RuntimeActionRequested: true),
            TimestampUtc);

        Assert.Equal(SliLispLabGelEngrammitizationDisposition.Refused, receipt.Disposition);
        Assert.Equal("sli-lisp-lab-gel-runtime-motion-refused", receipt.OutcomeCode);
        Assert.False(receipt.IsLabGelPreAdmissionEngrammitization);
        Assert.False(receipt.BoundedEntrypointCalled);
        Assert.False(receipt.LoadAttempted);
        Assert.False(receipt.LoadSucceeded);
        Assert.Contains("GEL admission, SelfGEL mutation, or continuity admission", receipt.StandardError, StringComparison.Ordinal);
    }
}
