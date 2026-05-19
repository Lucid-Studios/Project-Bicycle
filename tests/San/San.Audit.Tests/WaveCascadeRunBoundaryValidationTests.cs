using San.Common;
using Xunit;

namespace San.Audit.Tests;

public sealed class WaveCascadeRunBoundaryValidationTests
{
    private static readonly DateTimeOffset TimestampUtc = DateTimeOffset.Parse("2026-05-16T00:00:00Z");

    [Fact]
    public void Wave_Cascade_Retains_Ninety_Runs_With_Thirty_Sixty_Ninety_Seams()
    {
        var receipt = Cascade(CreateRequest());

        Assert.Equal(WaveCascadeRunDisposition.RetainedForReviewCold, receipt.Disposition);
        Assert.Equal("wave-cascade-90-runs-retained-cold", receipt.OutcomeCode);
        Assert.True(receipt.CascadeRetainedAsColdEvidence);
        Assert.Equal(90, receipt.Runs.Count);
        Assert.Equal(3, receipt.SeamReceipts.Count);
        Assert.Contains(receipt.SeamReceipts, seam => seam.SeamRun == 30);
        Assert.Contains(receipt.SeamReceipts, seam => seam.SeamRun == 60);
        Assert.Contains(receipt.SeamReceipts, seam => seam.SeamRun == 90);
        AssertCold(receipt);
    }

    [Fact]
    public void Wave_Cascade_Can_Retain_Thirty_Run_Throttle_Band()
    {
        var runs = CreateRuns(30);
        var receipt = Cascade(CreateRequest(
            runs: runs,
            seams: CreateSeams(runs)));

        Assert.Equal(WaveCascadeRunDisposition.RetainedForReviewCold, receipt.Disposition);
        Assert.Equal("wave-cascade-30-runs-retained-cold", receipt.OutcomeCode);
        Assert.Equal(30, receipt.Runs.Count);
        Assert.Single(receipt.SeamReceipts);
        AssertCold(receipt);
    }

    [Fact]
    public void Empty_Wave_Cascade_Is_Reviewable_But_Not_Authoritative()
    {
        var receipt = Cascade(CreateRequest(runs: [], seams: []));

        Assert.Equal(WaveCascadeRunDisposition.EmptyReviewCold, receipt.Disposition);
        Assert.Equal("wave-cascade-empty-review-only", receipt.OutcomeCode);
        Assert.Empty(receipt.Runs);
        Assert.Empty(receipt.SeamReceipts);
        Assert.False(receipt.AuthorityGranted);
        Assert.False(receipt.ContinuityAdmitted);
        AssertCold(receipt);
    }

    [Fact]
    public void Wave_Cascade_Does_Not_Convert_Run_Count_Seams_Or_Volume_Into_Warrant()
    {
        var receipt = Cascade(CreateRequest(priorPassageCount: 144));

        Assert.Equal(144, receipt.PriorPassageCount);
        Assert.Equal(144, receipt.PassageCountAfterCascade);
        Assert.False(receipt.RunCountBecameWarrant);
        Assert.False(receipt.SeamBecameAuthority);
        Assert.False(receipt.VolumeBecameTruth);
        Assert.False(receipt.CascadeAdmittedContinuity);
        Assert.False(receipt.ActionAuthorized);
        Assert.False(receipt.IdentityMutated);
        Assert.False(receipt.RuntimeActionAllowed);
        Assert.False(receipt.LispEvaluationAllowed);
        Assert.False(receipt.NewPacketEmitted);
        Assert.False(receipt.ReceiptsReplayed);
        Assert.False(receipt.ContinuityAdmitted);
        Assert.False(receipt.AuthorityGranted);
        Assert.True(receipt.ActivationRefused);
        AssertCold(receipt);
    }

    [Theory]
    [InlineData("missing-boundary")]
    [InlineData("not-review")]
    [InlineData("no-30")]
    [InlineData("no-60")]
    [InlineData("no-90")]
    [InlineData("no-seams")]
    [InlineData("no-evidence")]
    [InlineData("no-witness")]
    [InlineData("no-cooling")]
    [InlineData("no-return")]
    [InlineData("no-non-promotion")]
    [InlineData("run-truth")]
    [InlineData("repetition-warrant")]
    [InlineData("volume-authority")]
    [InlineData("cascade-continuity")]
    [InlineData("runtime")]
    [InlineData("identity")]
    [InlineData("lisp")]
    [InlineData("packet")]
    [InlineData("replay")]
    [InlineData("passage")]
    [InlineData("activation")]
    public void Cascade_Boundary_Refuses_Promotional_Collapse(string mutation)
    {
        var receipt = Cascade(CreateRequest(boundary: MutateBoundary(CreateBoundary(), mutation)));

        var expected = mutation == "missing-boundary"
            ? "wave-cascade-boundary-missing"
            : "wave-cascade-promotional-boundary";
        AssertRefused(receipt, expected);
    }

    [Theory]
    [InlineData("run-truth")]
    [InlineData("repetition-warrant")]
    [InlineData("volume-authority")]
    [InlineData("seam-continuity")]
    [InlineData("action")]
    [InlineData("lisp")]
    [InlineData("packet")]
    [InlineData("replay")]
    [InlineData("passage")]
    [InlineData("activation")]
    public void Non_Promotion_Boundary_Refuses_Volume_As_Warrant(string mutation)
    {
        var receipt = Cascade(CreateRequest(nonPromotion: MutateNonPromotion(CreateNonPromotion(), mutation)));

        AssertRefused(receipt, "wave-cascade-non-promotion-boundary-invalid");
    }

    [Fact]
    public void Wave_Cascade_Refuses_Unsupported_Run_Count()
    {
        var runs = CreateRuns(31);

        var receipt = Cascade(CreateRequest(runs: runs, seams: CreateSeams(runs)));

        AssertRefused(receipt, "wave-cascade-run-count-unsupported");
    }

    [Fact]
    public void Wave_Cascade_Refuses_Run_Index_Gap()
    {
        var runs = CreateRuns(30);
        runs[9] = runs[9] with { RunIndex = 29 };

        var receipt = Cascade(CreateRequest(runs: runs, seams: CreateSeams(CreateRuns(30))));

        AssertRefused(receipt, "wave-cascade-run-index-gap");
    }

    [Fact]
    public void Wave_Cascade_Refuses_Duplicate_Run_Handles()
    {
        var runs = CreateRuns(30);
        runs[1] = runs[1] with { RunHandle = runs[0].RunHandle };

        var receipt = Cascade(CreateRequest(runs: runs, seams: CreateSeams(runs)));

        AssertRefused(receipt, "wave-cascade-duplicate-run-handle");
    }

    [Theory]
    [InlineData("missing-handle")]
    [InlineData("missing-condensation")]
    [InlineData("missing-evidence")]
    [InlineData("missing-witness")]
    [InlineData("missing-anchor")]
    [InlineData("bad-index")]
    [InlineData("wrong-band")]
    [InlineData("not-review")]
    [InlineData("missing-evidence-body")]
    [InlineData("missing-witness-body")]
    [InlineData("missing-cooling")]
    [InlineData("missing-return")]
    [InlineData("run-truth")]
    [InlineData("repetition-warrant")]
    [InlineData("volume-authority")]
    [InlineData("cascade-continuity")]
    [InlineData("action")]
    [InlineData("identity")]
    [InlineData("lisp")]
    public void Cascade_Run_Remains_Cold_Evidence_Not_Warrant(string mutation)
    {
        var runs = CreateRuns(30);
        runs[0] = MutateRun(runs[0], mutation);

        var receipt = Cascade(CreateRequest(runs: runs, seams: CreateSeams(CreateRuns(30))));

        AssertRefused(receipt, "wave-cascade-run-invalid");
    }

    [Theory]
    [InlineData("missing-handle")]
    [InlineData("bad-seam")]
    [InlineData("empty-sources")]
    [InlineData("missing-evidence")]
    [InlineData("missing-witness")]
    [InlineData("not-review")]
    [InlineData("no-non-promotion")]
    [InlineData("no-run-lineage")]
    [InlineData("no-failed-lineage")]
    [InlineData("no-return")]
    [InlineData("authority")]
    [InlineData("continuity")]
    [InlineData("action")]
    [InlineData("lisp")]
    [InlineData("packet")]
    [InlineData("replay")]
    [InlineData("passage")]
    public void Seam_Receipt_Remains_Review_Only_Non_Promotion(string mutation)
    {
        var runs = CreateRuns(30);
        var seams = CreateSeams(runs);
        seams[0] = MutateSeam(seams[0], mutation);

        var receipt = Cascade(CreateRequest(runs: runs, seams: seams));

        AssertRefused(receipt, "wave-cascade-seam-invalid");
    }

    [Fact]
    public void Seam_Receipt_Must_Bind_To_Known_Runs()
    {
        var runs = CreateRuns(30);
        var seams = CreateSeams(runs);
        seams[0] = seams[0] with { SourceRunHandles = ["urn:san:wave-cascade-run:missing"] };

        var receipt = Cascade(CreateRequest(runs: runs, seams: seams));

        AssertRefused(receipt, "wave-cascade-seam-unbound");
    }

    [Fact]
    public void Required_Seam_Receipts_Must_Be_Present()
    {
        var runs = CreateRuns(60);
        var seams = CreateSeams(runs)
            .Where(static seam => seam.SeamRun != 60)
            .ToArray();

        var receipt = Cascade(CreateRequest(runs: runs, seams: seams));

        AssertRefused(receipt, "wave-cascade-required-seam-missing");
    }

    [Fact]
    public void Lisp_Body_Carries_Wave_Cascade_As_Inert_Throttle_Register()
    {
        var lispPath = Path.Combine(
            FindRepositoryRoot(),
            "src",
            "SLI",
            "SLI.Lisp",
            "wave-cascade-run.lisp");

        var body = File.ReadAllText(lispPath);

        Assert.Contains(":posture :cme-wave-cascade-run-boundary", body, StringComparison.Ordinal);
        Assert.Contains(":lisp-role :inert-wave-cascade-run-carrier", body, StringComparison.Ordinal);
        Assert.Contains(":cascade-scope (:runs-30 :runs-60 :runs-90)", body, StringComparison.Ordinal);
        Assert.Contains(":run-count-not-warrant", body, StringComparison.Ordinal);
        Assert.Contains(":repetition-not-authority", body, StringComparison.Ordinal);
        Assert.Contains(":volume-not-truth", body, StringComparison.Ordinal);
        Assert.Contains(":seam-not-continuity", body, StringComparison.Ordinal);
        Assert.Contains(":cascade-not-action", body, StringComparison.Ordinal);
        Assert.Contains(":lisp-evaluation-allowed nil", body, StringComparison.Ordinal);
        Assert.Contains(":return :receipt-only", body, StringComparison.Ordinal);
        Assert.DoesNotContain("(eval", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("(compile", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("(load", body, StringComparison.OrdinalIgnoreCase);
    }

    private static WaveCascadeRunReceipt Cascade(WaveCascadeRunRequest request) =>
        new DefaultWaveCascadeRunBoundaryValidator().Cascade(request, TimestampUtc);

    private static WaveCascadeRunRequest CreateRequest(
        IReadOnlyList<WaveCascadeRun>? runs = null,
        IReadOnlyList<WaveCascadeSeamReceipt>? seams = null,
        WaveCascadeRunBoundary? boundary = null,
        WaveCascadeNonPromotionBoundary? nonPromotion = null,
        int priorPassageCount = 72)
    {
        var runSet = runs ?? CreateRuns(90);
        return new(
            Runs: runSet,
            SeamReceipts: seams ?? CreateSeams(runSet),
            Boundary: boundary ?? CreateBoundary(),
            NonPromotionBoundary: nonPromotion ?? CreateNonPromotion(),
            PriorPassageCount: priorPassageCount);
    }

    private static WaveCascadeRun[] CreateRuns(int count) =>
        Enumerable.Range(1, count)
            .Select(index => new WaveCascadeRun(
                RunHandle: $"urn:san:wave-cascade-run:{index:000}",
                SourceCondensationHandle: "urn:san:wave-condensation:shared-reality-review",
                EvidenceHandle: $"urn:san:evidence:wave-cascade:{index:000}",
                WitnessHandle: $"urn:san:witness:wave-cascade:{index:000}",
                SharedRealityAnchorHandle: "urn:san:shared-reality-anchor:prime-body",
                Band: BandFor(index),
                RunIndex: index,
                ReviewOnly: true,
                EvidenceBodyPresent: true,
                WitnessBodyPresent: true,
                CoolingPathPresent: true,
                ReturnPathPresent: true,
                CondensedFromPriorRun: index > 1,
                TreatsRunAsTruth: false,
                TreatsRepetitionAsWarrant: false,
                TreatsVolumeAsAuthority: false,
                TreatsCascadeAsContinuity: false,
                AuthorizesAction: false,
                MutatesIdentity: false,
                EvaluatesLisp: false))
            .ToArray();

    private static WaveCascadeSeamReceipt[] CreateSeams(IReadOnlyList<WaveCascadeRun> runs) =>
        new[] { 30, 60, 90 }
            .Where(seam => seam <= runs.Count)
            .Select(seam => new WaveCascadeSeamReceipt(
                SeamHandle: $"urn:san:wave-cascade-seam:{seam:000}",
                SeamRun: seam,
                SourceRunHandles: runs
                    .Where(run => run.RunIndex <= seam && run.RunIndex > seam - 30)
                    .Select(static run => run.RunHandle)
                    .ToArray(),
                EvidenceHandle: $"urn:san:evidence:wave-cascade:seam-{seam:000}",
                WitnessHandle: $"urn:san:witness:wave-cascade:seam-{seam:000}",
                ReviewOnly: true,
                NonPromotionConfirmed: true,
                PreservesRunLineage: true,
                PreservesFailedCaseLineage: true,
                PreservesReturnPath: true,
                SeamMayContinue: seam < runs.Count,
                SeamBecomesAuthority: false,
                SeamAdmitsContinuity: false,
                SeamAuthorizesAction: false,
                SeamEvaluatesLisp: false,
                SeamEmitsPacket: false,
                SeamReplaysReceipts: false,
                SeamIncrementsPassage: false))
            .ToArray();

    private static WaveCascadeRunBand BandFor(int index) =>
        index switch
        {
            <= 30 => WaveCascadeRunBand.Runs01To30,
            <= 60 => WaveCascadeRunBand.Runs31To60,
            _ => WaveCascadeRunBand.Runs61To90
        };

    private static WaveCascadeRunBoundary CreateBoundary(string? mutation = null) =>
        MutateBoundary(
            new WaveCascadeRunBoundary(
                BoundaryCode: "wave-cascade-run-review-only",
                Present: true,
                ReviewOnly: true,
                AllowsThirtyRunCascade: true,
                AllowsSixtyRunCascade: true,
                AllowsNinetyRunCascade: true,
                RequiresSeamReceipts: true,
                RequiresEvidence: true,
                RequiresWitness: true,
                RequiresCooling: true,
                RequiresReturnPath: true,
                RequiresNonPromotionConfirmation: true,
                AllowsRunAsTruth: false,
                AllowsRepetitionAsWarrant: false,
                AllowsVolumeAsAuthority: false,
                AllowsCascadeAsContinuity: false,
                AllowsRuntimeAction: false,
                AllowsIdentityMutation: false,
                AllowsLispEvaluation: false,
                AllowsPacketEmission: false,
                AllowsReceiptReplay: false,
                IncrementsPassageCount: false,
                AllowsActivation: false),
            mutation);

    private static WaveCascadeNonPromotionBoundary CreateNonPromotion() =>
        new(
            BoundaryLaw: "30, 60, and 90 retained runs may condense into review evidence; volume may not become warrant",
            RunMayBecomeTruth: false,
            RepetitionMayBecomeWarrant: false,
            VolumeMayBecomeAuthority: false,
            SeamMayAdmitContinuity: false,
            CascadeMayAuthorizeAction: false,
            CascadeMayEvaluateLisp: false,
            CascadeMayEmitPacket: false,
            CascadeMayReplayReceipts: false,
            CascadeMayIncrementPassage: false,
            CascadeMayActivate: false);

    private static WaveCascadeRunBoundary MutateBoundary(
        WaveCascadeRunBoundary boundary,
        string? mutation) =>
        mutation switch
        {
            null => boundary,
            "missing-boundary" => boundary with { BoundaryCode = string.Empty, Present = false },
            "not-review" => boundary with { ReviewOnly = false },
            "no-30" => boundary with { AllowsThirtyRunCascade = false },
            "no-60" => boundary with { AllowsSixtyRunCascade = false },
            "no-90" => boundary with { AllowsNinetyRunCascade = false },
            "no-seams" => boundary with { RequiresSeamReceipts = false },
            "no-evidence" => boundary with { RequiresEvidence = false },
            "no-witness" => boundary with { RequiresWitness = false },
            "no-cooling" => boundary with { RequiresCooling = false },
            "no-return" => boundary with { RequiresReturnPath = false },
            "no-non-promotion" => boundary with { RequiresNonPromotionConfirmation = false },
            "run-truth" => boundary with { AllowsRunAsTruth = true },
            "repetition-warrant" => boundary with { AllowsRepetitionAsWarrant = true },
            "volume-authority" => boundary with { AllowsVolumeAsAuthority = true },
            "cascade-continuity" => boundary with { AllowsCascadeAsContinuity = true },
            "runtime" => boundary with { AllowsRuntimeAction = true },
            "identity" => boundary with { AllowsIdentityMutation = true },
            "lisp" => boundary with { AllowsLispEvaluation = true },
            "packet" => boundary with { AllowsPacketEmission = true },
            "replay" => boundary with { AllowsReceiptReplay = true },
            "passage" => boundary with { IncrementsPassageCount = true },
            "activation" => boundary with { AllowsActivation = true },
            _ => boundary
        };

    private static WaveCascadeNonPromotionBoundary MutateNonPromotion(
        WaveCascadeNonPromotionBoundary boundary,
        string mutation) =>
        mutation switch
        {
            "run-truth" => boundary with { RunMayBecomeTruth = true },
            "repetition-warrant" => boundary with { RepetitionMayBecomeWarrant = true },
            "volume-authority" => boundary with { VolumeMayBecomeAuthority = true },
            "seam-continuity" => boundary with { SeamMayAdmitContinuity = true },
            "action" => boundary with { CascadeMayAuthorizeAction = true },
            "lisp" => boundary with { CascadeMayEvaluateLisp = true },
            "packet" => boundary with { CascadeMayEmitPacket = true },
            "replay" => boundary with { CascadeMayReplayReceipts = true },
            "passage" => boundary with { CascadeMayIncrementPassage = true },
            "activation" => boundary with { CascadeMayActivate = true },
            _ => boundary
        };

    private static WaveCascadeRun MutateRun(WaveCascadeRun run, string mutation) =>
        mutation switch
        {
            "missing-handle" => run with { RunHandle = string.Empty },
            "missing-condensation" => run with { SourceCondensationHandle = string.Empty },
            "missing-evidence" => run with { EvidenceHandle = string.Empty },
            "missing-witness" => run with { WitnessHandle = string.Empty },
            "missing-anchor" => run with { SharedRealityAnchorHandle = string.Empty },
            "bad-index" => run with { RunIndex = 0 },
            "wrong-band" => run with { Band = WaveCascadeRunBand.Runs61To90 },
            "not-review" => run with { ReviewOnly = false },
            "missing-evidence-body" => run with { EvidenceBodyPresent = false },
            "missing-witness-body" => run with { WitnessBodyPresent = false },
            "missing-cooling" => run with { CoolingPathPresent = false },
            "missing-return" => run with { ReturnPathPresent = false },
            "run-truth" => run with { TreatsRunAsTruth = true },
            "repetition-warrant" => run with { TreatsRepetitionAsWarrant = true },
            "volume-authority" => run with { TreatsVolumeAsAuthority = true },
            "cascade-continuity" => run with { TreatsCascadeAsContinuity = true },
            "action" => run with { AuthorizesAction = true },
            "identity" => run with { MutatesIdentity = true },
            "lisp" => run with { EvaluatesLisp = true },
            _ => run
        };

    private static WaveCascadeSeamReceipt MutateSeam(WaveCascadeSeamReceipt seam, string mutation) =>
        mutation switch
        {
            "missing-handle" => seam with { SeamHandle = string.Empty },
            "bad-seam" => seam with { SeamRun = 45 },
            "empty-sources" => seam with { SourceRunHandles = [] },
            "missing-evidence" => seam with { EvidenceHandle = string.Empty },
            "missing-witness" => seam with { WitnessHandle = string.Empty },
            "not-review" => seam with { ReviewOnly = false },
            "no-non-promotion" => seam with { NonPromotionConfirmed = false },
            "no-run-lineage" => seam with { PreservesRunLineage = false },
            "no-failed-lineage" => seam with { PreservesFailedCaseLineage = false },
            "no-return" => seam with { PreservesReturnPath = false },
            "authority" => seam with { SeamBecomesAuthority = true },
            "continuity" => seam with { SeamAdmitsContinuity = true },
            "action" => seam with { SeamAuthorizesAction = true },
            "lisp" => seam with { SeamEvaluatesLisp = true },
            "packet" => seam with { SeamEmitsPacket = true },
            "replay" => seam with { SeamReplaysReceipts = true },
            "passage" => seam with { SeamIncrementsPassage = true },
            _ => seam
        };

    private static void AssertCold(WaveCascadeRunReceipt receipt)
    {
        Assert.True(receipt.IsColdWaveCascade);
        Assert.Null(receipt.Refusal);
        Assert.True(receipt.ReviewOnly);
        Assert.Equal(receipt.PriorPassageCount, receipt.PassageCountAfterCascade);
        Assert.False(receipt.RunCountBecameWarrant);
        Assert.False(receipt.SeamBecameAuthority);
        Assert.False(receipt.VolumeBecameTruth);
        Assert.False(receipt.CascadeAdmittedContinuity);
        Assert.False(receipt.ActionAuthorized);
        Assert.False(receipt.IdentityMutated);
        Assert.False(receipt.RuntimeActionAllowed);
        Assert.False(receipt.LispEvaluationAllowed);
        Assert.False(receipt.NewPacketEmitted);
        Assert.False(receipt.ReceiptsReplayed);
        Assert.False(receipt.ContinuityAdmitted);
        Assert.False(receipt.AuthorityGranted);
        Assert.True(receipt.ActivationRefused);
    }

    private static void AssertRefused(
        WaveCascadeRunReceipt receipt,
        string outcomeCode)
    {
        Assert.Equal(WaveCascadeRunDisposition.Refused, receipt.Disposition);
        Assert.Equal(outcomeCode, receipt.OutcomeCode);
        Assert.True(receipt.IsRetainedWaveCascadeRefusal);
        Assert.NotNull(receipt.Refusal);
        Assert.Empty(receipt.Runs);
        Assert.Empty(receipt.SeamReceipts);
        Assert.Equal(receipt.PriorPassageCount, receipt.PassageCountAfterCascade);
        Assert.False(receipt.ActionAuthorized);
        Assert.False(receipt.ContinuityAdmitted);
        Assert.False(receipt.AuthorityGranted);
        Assert.True(receipt.ActivationRefused);
    }

    private static string FindRepositoryRoot()
    {
        var current = new DirectoryInfo(AppContext.BaseDirectory);
        while (current is not null)
        {
            var candidate = Path.Combine(current.FullName, "src", "SLI", "SLI.Lisp");
            if (Directory.Exists(candidate))
            {
                return current.FullName;
            }

            current = current.Parent;
        }

        throw new DirectoryNotFoundException("Unable to locate repository root.");
    }
}
