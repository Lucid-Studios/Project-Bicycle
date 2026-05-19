using System.Security.Cryptography;
using System.Text;

namespace San.Common;

public enum WaveCascadeRunDisposition
{
    RetainedForReviewCold = 0,
    EmptyReviewCold = 1,
    Refused = 2
}

public enum WaveCascadeRunBand
{
    Runs01To30 = 0,
    Runs31To60 = 1,
    Runs61To90 = 2
}

public sealed record WaveCascadeRun(
    string RunHandle,
    string SourceCondensationHandle,
    string EvidenceHandle,
    string WitnessHandle,
    string SharedRealityAnchorHandle,
    WaveCascadeRunBand Band,
    int RunIndex,
    bool ReviewOnly,
    bool EvidenceBodyPresent,
    bool WitnessBodyPresent,
    bool CoolingPathPresent,
    bool ReturnPathPresent,
    bool CondensedFromPriorRun,
    bool TreatsRunAsTruth,
    bool TreatsRepetitionAsWarrant,
    bool TreatsVolumeAsAuthority,
    bool TreatsCascadeAsContinuity,
    bool AuthorizesAction,
    bool MutatesIdentity,
    bool EvaluatesLisp)
{
    public bool IsColdCascadeRun =>
        !string.IsNullOrWhiteSpace(RunHandle) &&
        !string.IsNullOrWhiteSpace(SourceCondensationHandle) &&
        !string.IsNullOrWhiteSpace(EvidenceHandle) &&
        !string.IsNullOrWhiteSpace(WitnessHandle) &&
        !string.IsNullOrWhiteSpace(SharedRealityAnchorHandle) &&
        RunIndex is >= 1 and <= 90 &&
        ReviewOnly &&
        EvidenceBodyPresent &&
        WitnessBodyPresent &&
        CoolingPathPresent &&
        ReturnPathPresent &&
        !TreatsRunAsTruth &&
        !TreatsRepetitionAsWarrant &&
        !TreatsVolumeAsAuthority &&
        !TreatsCascadeAsContinuity &&
        !AuthorizesAction &&
        !MutatesIdentity &&
        !EvaluatesLisp &&
        BandMatchesIndex;

    private bool BandMatchesIndex =>
        Band switch
        {
            WaveCascadeRunBand.Runs01To30 => RunIndex is >= 1 and <= 30,
            WaveCascadeRunBand.Runs31To60 => RunIndex is >= 31 and <= 60,
            WaveCascadeRunBand.Runs61To90 => RunIndex is >= 61 and <= 90,
            _ => false
        };
}

public sealed record WaveCascadeSeamReceipt(
    string SeamHandle,
    int SeamRun,
    IReadOnlyList<string> SourceRunHandles,
    string EvidenceHandle,
    string WitnessHandle,
    bool ReviewOnly,
    bool NonPromotionConfirmed,
    bool PreservesRunLineage,
    bool PreservesFailedCaseLineage,
    bool PreservesReturnPath,
    bool SeamMayContinue,
    bool SeamBecomesAuthority,
    bool SeamAdmitsContinuity,
    bool SeamAuthorizesAction,
    bool SeamEvaluatesLisp,
    bool SeamEmitsPacket,
    bool SeamReplaysReceipts,
    bool SeamIncrementsPassage)
{
    public bool IsColdSeamReceipt =>
        !string.IsNullOrWhiteSpace(SeamHandle) &&
        SeamRun is 30 or 60 or 90 &&
        SourceRunHandles.Count > 0 &&
        !string.IsNullOrWhiteSpace(EvidenceHandle) &&
        !string.IsNullOrWhiteSpace(WitnessHandle) &&
        ReviewOnly &&
        NonPromotionConfirmed &&
        PreservesRunLineage &&
        PreservesFailedCaseLineage &&
        PreservesReturnPath &&
        !SeamBecomesAuthority &&
        !SeamAdmitsContinuity &&
        !SeamAuthorizesAction &&
        !SeamEvaluatesLisp &&
        !SeamEmitsPacket &&
        !SeamReplaysReceipts &&
        !SeamIncrementsPassage;
}

public sealed record WaveCascadeRunBoundary(
    string BoundaryCode,
    bool Present,
    bool ReviewOnly,
    bool AllowsThirtyRunCascade,
    bool AllowsSixtyRunCascade,
    bool AllowsNinetyRunCascade,
    bool RequiresSeamReceipts,
    bool RequiresEvidence,
    bool RequiresWitness,
    bool RequiresCooling,
    bool RequiresReturnPath,
    bool RequiresNonPromotionConfirmation,
    bool AllowsRunAsTruth,
    bool AllowsRepetitionAsWarrant,
    bool AllowsVolumeAsAuthority,
    bool AllowsCascadeAsContinuity,
    bool AllowsRuntimeAction,
    bool AllowsIdentityMutation,
    bool AllowsLispEvaluation,
    bool AllowsPacketEmission,
    bool AllowsReceiptReplay,
    bool IncrementsPassageCount,
    bool AllowsActivation)
{
    public bool IsColdBoundary =>
        !string.IsNullOrWhiteSpace(BoundaryCode) &&
        Present &&
        ReviewOnly &&
        AllowsThirtyRunCascade &&
        AllowsSixtyRunCascade &&
        AllowsNinetyRunCascade &&
        RequiresSeamReceipts &&
        RequiresEvidence &&
        RequiresWitness &&
        RequiresCooling &&
        RequiresReturnPath &&
        RequiresNonPromotionConfirmation &&
        !AllowsRunAsTruth &&
        !AllowsRepetitionAsWarrant &&
        !AllowsVolumeAsAuthority &&
        !AllowsCascadeAsContinuity &&
        !AllowsRuntimeAction &&
        !AllowsIdentityMutation &&
        !AllowsLispEvaluation &&
        !AllowsPacketEmission &&
        !AllowsReceiptReplay &&
        !IncrementsPassageCount &&
        !AllowsActivation;
}

public sealed record WaveCascadeNonPromotionBoundary(
    string BoundaryLaw,
    bool RunMayBecomeTruth,
    bool RepetitionMayBecomeWarrant,
    bool VolumeMayBecomeAuthority,
    bool SeamMayAdmitContinuity,
    bool CascadeMayAuthorizeAction,
    bool CascadeMayEvaluateLisp,
    bool CascadeMayEmitPacket,
    bool CascadeMayReplayReceipts,
    bool CascadeMayIncrementPassage,
    bool CascadeMayActivate)
{
    public bool IsColdNonPromotionBoundary =>
        !string.IsNullOrWhiteSpace(BoundaryLaw) &&
        !RunMayBecomeTruth &&
        !RepetitionMayBecomeWarrant &&
        !VolumeMayBecomeAuthority &&
        !SeamMayAdmitContinuity &&
        !CascadeMayAuthorizeAction &&
        !CascadeMayEvaluateLisp &&
        !CascadeMayEmitPacket &&
        !CascadeMayReplayReceipts &&
        !CascadeMayIncrementPassage &&
        !CascadeMayActivate;
}

public sealed record WaveCascadeRunRefusalReceipt(
    string ReceiptHandle,
    string RefusalCode,
    string GovernanceTrace,
    bool Retained);

public sealed record WaveCascadeRunRequest(
    IReadOnlyList<WaveCascadeRun> Runs,
    IReadOnlyList<WaveCascadeSeamReceipt> SeamReceipts,
    WaveCascadeRunBoundary Boundary,
    WaveCascadeNonPromotionBoundary NonPromotionBoundary,
    int PriorPassageCount);

public sealed record WaveCascadeRunReceipt(
    string ReceiptHandle,
    WaveCascadeRunDisposition Disposition,
    string OutcomeCode,
    string GovernanceTrace,
    IReadOnlyList<WaveCascadeRun> Runs,
    IReadOnlyList<WaveCascadeSeamReceipt> SeamReceipts,
    WaveCascadeRunBoundary Boundary,
    WaveCascadeNonPromotionBoundary NonPromotionBoundary,
    WaveCascadeRunRefusalReceipt? Refusal,
    int PriorPassageCount,
    int PassageCountAfterCascade,
    int RetainedRunCount,
    bool ReviewOnly,
    bool CascadeRetainedAsColdEvidence,
    bool RunCountBecameWarrant,
    bool SeamBecameAuthority,
    bool VolumeBecameTruth,
    bool CascadeAdmittedContinuity,
    bool ActionAuthorized,
    bool IdentityMutated,
    bool RuntimeActionAllowed,
    bool LispEvaluationAllowed,
    bool NewPacketEmitted,
    bool ReceiptsReplayed,
    bool ContinuityAdmitted,
    bool AuthorityGranted,
    bool ActivationRefused,
    DateTimeOffset TimestampUtc)
{
    public bool IsColdWaveCascade =>
        (Disposition is WaveCascadeRunDisposition.RetainedForReviewCold or
            WaveCascadeRunDisposition.EmptyReviewCold) &&
        Refusal is null &&
        ReviewOnly &&
        PassageCountAfterCascade == PriorPassageCount &&
        RetainedRunCount == Runs.Count &&
        !RunCountBecameWarrant &&
        !SeamBecameAuthority &&
        !VolumeBecameTruth &&
        !CascadeAdmittedContinuity &&
        !ActionAuthorized &&
        !IdentityMutated &&
        !RuntimeActionAllowed &&
        !LispEvaluationAllowed &&
        !NewPacketEmitted &&
        !ReceiptsReplayed &&
        !ContinuityAdmitted &&
        !AuthorityGranted &&
        ActivationRefused &&
        Boundary.IsColdBoundary &&
        NonPromotionBoundary.IsColdNonPromotionBoundary;

    public bool IsRetainedWaveCascadeRefusal =>
        Disposition == WaveCascadeRunDisposition.Refused &&
        Refusal?.Retained == true &&
        PassageCountAfterCascade == PriorPassageCount &&
        RetainedRunCount == 0 &&
        !CascadeRetainedAsColdEvidence &&
        !RunCountBecameWarrant &&
        !SeamBecameAuthority &&
        !VolumeBecameTruth &&
        !CascadeAdmittedContinuity &&
        !ActionAuthorized &&
        !IdentityMutated &&
        !RuntimeActionAllowed &&
        !LispEvaluationAllowed &&
        !NewPacketEmitted &&
        !ReceiptsReplayed &&
        !ContinuityAdmitted &&
        !AuthorityGranted &&
        ActivationRefused;
}

public sealed class DefaultWaveCascadeRunBoundaryValidator
{
    public WaveCascadeRunReceipt Cascade(
        WaveCascadeRunRequest request,
        DateTimeOffset timestampUtc)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.Boundary is null ||
            !request.Boundary.Present ||
            string.IsNullOrWhiteSpace(request.Boundary.BoundaryCode))
        {
            return Refuse(
                request,
                "wave-cascade-boundary-missing",
                "Wave cascade refused because a review-only cascade boundary is required before 30, 60, or 90 runs may be retained.",
                timestampUtc);
        }

        if (!request.Boundary.IsColdBoundary)
        {
            return Refuse(
                request,
                "wave-cascade-promotional-boundary",
                "Wave cascade refused because the boundary must allow cold 30, 60, and 90 run cascades while requiring seam receipts, evidence, witness, cooling, return path, and non-promotion, and while refusing truth, warrant, volume authority, continuity, action, identity mutation, Lisp evaluation, packet emission, replay, passage, and activation.",
                timestampUtc);
        }

        if (request.NonPromotionBoundary is null ||
            !request.NonPromotionBoundary.IsColdNonPromotionBoundary)
        {
            return Refuse(
                request,
                "wave-cascade-non-promotion-boundary-invalid",
                "Wave cascade refused because non-promotion law must prevent runs, repetition, volume, seams, cascade, Lisp evaluation, packets, replay, passage, and activation from promoting themselves.",
                timestampUtc);
        }

        if (request.Runs.Count is not 0 and not 30 and not 60 and not 90)
        {
            return Refuse(
                request,
                "wave-cascade-run-count-unsupported",
                "Wave cascade refused because only 30, 60, or 90 retained cold runs may be accepted by this throttle boundary.",
                timestampUtc);
        }

        if (request.Runs.Any(static run => !run.IsColdCascadeRun))
        {
            return Refuse(
                request,
                "wave-cascade-run-invalid",
                "Wave cascade refused because every run must remain evidence-backed, witnessed, cooled, returned, review-only, and unable to become truth, warrant, authority, continuity, identity mutation, action, or Lisp evaluation.",
                timestampUtc);
        }

        var runHandles = request.Runs
            .Select(static run => run.RunHandle)
            .ToHashSet(StringComparer.Ordinal);
        if (runHandles.Count != request.Runs.Count)
        {
            return Refuse(
                request,
                "wave-cascade-duplicate-run-handle",
                "Wave cascade refused because duplicate run handles would collapse run lineage.",
                timestampUtc);
        }

        if (!HasExpectedRunIndices(request.Runs))
        {
            return Refuse(
                request,
                "wave-cascade-run-index-gap",
                "Wave cascade refused because retained runs must preserve contiguous run indices from 1 through the requested run count.",
                timestampUtc);
        }

        if (request.SeamReceipts.Any(static seam => !seam.IsColdSeamReceipt))
        {
            return Refuse(
                request,
                "wave-cascade-seam-invalid",
                "Wave cascade refused because every seam receipt must preserve lineage and non-promotion while refusing authority, continuity, action, Lisp evaluation, packet emission, replay, and passage.",
                timestampUtc);
        }

        if (HasDuplicate(request.SeamReceipts.Select(static seam => seam.SeamHandle)))
        {
            return Refuse(
                request,
                "wave-cascade-duplicate-seam-handle",
                "Wave cascade refused because duplicate seam handles would collapse seam lineage.",
                timestampUtc);
        }

        if (request.SeamReceipts.Any(seam => seam.SourceRunHandles.Any(handle => !runHandles.Contains(handle))))
        {
            return Refuse(
                request,
                "wave-cascade-seam-unbound",
                "Wave cascade refused because every seam receipt must cite known retained run handles.",
                timestampUtc);
        }

        var expectedSeams = ExpectedSeamRuns(request.Runs.Count);
        var actualSeams = request.SeamReceipts
            .Select(static seam => seam.SeamRun)
            .ToHashSet();
        if (!expectedSeams.All(actualSeams.Contains))
        {
            return Refuse(
                request,
                "wave-cascade-required-seam-missing",
                "Wave cascade refused because required seam receipts at 30, 60, or 90 runs are missing.",
                timestampUtc);
        }

        var disposition = request.Runs.Count == 0
            ? WaveCascadeRunDisposition.EmptyReviewCold
            : WaveCascadeRunDisposition.RetainedForReviewCold;
        var outcomeCode = disposition == WaveCascadeRunDisposition.EmptyReviewCold
            ? "wave-cascade-empty-review-only"
            : $"wave-cascade-{request.Runs.Count}-runs-retained-cold";
        var governanceTrace = disposition == WaveCascadeRunDisposition.EmptyReviewCold
            ? "Wave cascade found no runs. Empty review preserves throttle boundary without truth, warrant, authority, continuity, action, Lisp evaluation, packet emission, replay, passage, or activation."
            : $"Wave cascade retained {request.Runs.Count} cold review runs with seam receipts while refusing repetition-as-warrant, volume-as-authority, seam-as-continuity, action, Lisp evaluation, packet emission, replay, passage, and activation.";

        return CreateReceipt(
            request,
            disposition,
            outcomeCode,
            governanceTrace,
            refusal: null,
            timestampUtc);
    }

    private static bool HasExpectedRunIndices(IReadOnlyList<WaveCascadeRun> runs)
    {
        var indices = runs
            .Select(static run => run.RunIndex)
            .ToHashSet();
        return Enumerable.Range(1, runs.Count).All(indices.Contains);
    }

    private static IReadOnlyList<int> ExpectedSeamRuns(int runCount) =>
        runCount switch
        {
            0 => [],
            30 => [30],
            60 => [30, 60],
            90 => [30, 60, 90],
            _ => []
        };

    private static WaveCascadeRunReceipt Refuse(
        WaveCascadeRunRequest request,
        string outcomeCode,
        string governanceTrace,
        DateTimeOffset timestampUtc) =>
        CreateReceipt(
            request,
            WaveCascadeRunDisposition.Refused,
            outcomeCode,
            governanceTrace,
            new WaveCascadeRunRefusalReceipt(
                ReceiptHandle: $"urn:san:wave-cascade-refusal:{ShortHash(SourceHandle(request), outcomeCode)}",
                RefusalCode: outcomeCode,
                GovernanceTrace: governanceTrace,
                Retained: true),
            timestampUtc);

    private static WaveCascadeRunReceipt CreateReceipt(
        WaveCascadeRunRequest request,
        WaveCascadeRunDisposition disposition,
        string outcomeCode,
        string governanceTrace,
        WaveCascadeRunRefusalReceipt? refusal,
        DateTimeOffset timestampUtc) =>
        new(
            ReceiptHandle: $"urn:san:wave-cascade:{(refusal is null ? "review" : "refused")}:{ShortHash(SourceHandle(request), outcomeCode, request.Runs.Count.ToString(System.Globalization.CultureInfo.InvariantCulture), timestampUtc.UtcTicks.ToString(System.Globalization.CultureInfo.InvariantCulture))}",
            Disposition: disposition,
            OutcomeCode: outcomeCode,
            GovernanceTrace: governanceTrace,
            Runs: refusal is null ? request.Runs.ToArray() : [],
            SeamReceipts: refusal is null ? request.SeamReceipts.ToArray() : [],
            Boundary: request.Boundary,
            NonPromotionBoundary: request.NonPromotionBoundary,
            Refusal: refusal,
            PriorPassageCount: request.PriorPassageCount,
            PassageCountAfterCascade: request.PriorPassageCount,
            RetainedRunCount: refusal is null ? request.Runs.Count : 0,
            ReviewOnly: true,
            CascadeRetainedAsColdEvidence: refusal is null &&
                disposition == WaveCascadeRunDisposition.RetainedForReviewCold,
            RunCountBecameWarrant: false,
            SeamBecameAuthority: false,
            VolumeBecameTruth: false,
            CascadeAdmittedContinuity: false,
            ActionAuthorized: false,
            IdentityMutated: false,
            RuntimeActionAllowed: false,
            LispEvaluationAllowed: false,
            NewPacketEmitted: false,
            ReceiptsReplayed: false,
            ContinuityAdmitted: false,
            AuthorityGranted: false,
            ActivationRefused: true,
            TimestampUtc: timestampUtc);

    private static string SourceHandle(WaveCascadeRunRequest request) =>
        request.Runs.Count == 0
            ? "wave-cascade-empty-source"
            : string.Join(",", request.Runs.Take(3).Select(static run => run.RunHandle));

    private static bool HasDuplicate(IEnumerable<string> handles)
    {
        var seen = new HashSet<string>(StringComparer.Ordinal);
        foreach (var handle in handles)
        {
            if (!seen.Add(handle))
            {
                return true;
            }
        }

        return false;
    }

    private static string ShortHash(params string[] parts)
    {
        var payload = string.Join("|", parts);
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(payload));
        return Convert.ToHexString(bytes)[..16].ToLowerInvariant();
    }
}
