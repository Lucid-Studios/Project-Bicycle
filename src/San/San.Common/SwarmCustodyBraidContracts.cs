using System.Security.Cryptography;
using System.Text;

namespace San.Common;

public enum SwarmWorkerDomain
{
    SanctuaryCradleTekRoot = 0,
    PrimeCrypticStewardTriptych = 1,
    CSharpSliLispDuplex = 2,
    PacketReceiptWitnessCustody = 3,
    CompassListeningSituationalAwareness = 4,
    InnerChamberFlow = 5,
    EngrammitizationPreconditions = 6,
    SoulFrameAgentiCoreEngineeredCognition = 7,
    TelemetryCoolingReplayQuery = 8
}

public enum SwarmCustodyBraidDisposition
{
    EmptyReviewCold = 0,
    BraidedForReviewCold = 1,
    Refused = 2
}

public sealed record SwarmBatchSeam(
    int BatchIndex,
    int RunStart,
    int RunEnd,
    int MicroSeam,
    string SeamCode);

public sealed record SwarmWorkerAuthorityBoundary(
    bool ActivationRequested,
    bool ModelBindingRequested,
    bool LispEvaluationRequested,
    bool RuntimeActionRequested,
    bool DatabaseWriteRequested,
    bool GelPromotionRequested,
    bool CmeActualRequested,
    bool SanctuaryActualRequested,
    bool ContinuityAdmissionRequested,
    bool AuthorityRequested,
    bool SelfAuthorizationRequested,
    bool EvidenceReplacementRequested,
    bool PacketEmissionRequested,
    bool ReceiptReplayRequested,
    bool IncrementsPassageCount)
{
    public bool RequestsForbiddenMotion =>
        ActivationRequested ||
        ModelBindingRequested ||
        LispEvaluationRequested ||
        RuntimeActionRequested ||
        DatabaseWriteRequested ||
        GelPromotionRequested ||
        CmeActualRequested ||
        SanctuaryActualRequested ||
        ContinuityAdmissionRequested ||
        AuthorityRequested ||
        SelfAuthorizationRequested ||
        EvidenceReplacementRequested ||
        PacketEmissionRequested ||
        ReceiptReplayRequested ||
        IncrementsPassageCount;
}

public sealed record SwarmWorkerTelemetryPacket(
    string PacketHandle,
    string WorkerId,
    SwarmWorkerDomain Domain,
    SwarmBatchSeam BatchSeam,
    IReadOnlyList<string> SourceHandles,
    IReadOnlyList<string> CandidateFindings,
    string NextLaneRecommendation,
    decimal Confidence,
    SwarmWorkerAuthorityBoundary AuthorityBoundary,
    bool ReviewOnly,
    bool CandidateOnly,
    bool PreservesLineage)
{
    public bool IsColdCandidate =>
        ReviewOnly &&
        CandidateOnly &&
        PreservesLineage &&
        !AuthorityBoundary.RequestsForbiddenMotion &&
        !string.IsNullOrWhiteSpace(PacketHandle) &&
        !string.IsNullOrWhiteSpace(WorkerId) &&
        !string.IsNullOrWhiteSpace(NextLaneRecommendation) &&
        Confidence is >= 0m and <= 1m;
}

public sealed record SwarmCustodyBraidScopeBoundary(
    string ScopeCode,
    bool Present,
    bool ReviewOnly,
    bool AllowsNextLaneNomination,
    bool AllowsAuthority,
    bool AllowsContinuityAdmission,
    bool AllowsActivation,
    bool AllowsConsensusWarrant,
    bool AllowsAggregateConfidenceWarrant);

public sealed record SwarmCustodyBraidNonPromotionBoundary(
    bool WorkerConsensusMayAuthorize,
    bool AggregateConfidenceMayBecomeWarrant,
    bool NextLaneNominationMayStartWork,
    bool BraidMayAdmitContinuity,
    bool BraidMayActivate,
    bool BraidMayEmitPackets,
    bool BraidMayReplayReceipts,
    bool BraidMayIncrementPassageCount,
    string BoundaryLaw);

public sealed record SwarmCustodyBraidRefusalReceipt(
    string ReceiptHandle,
    string RefusalCode,
    string GovernanceTrace,
    bool Retained);

public sealed record SwarmCustodyBraidRequest(
    string BraidHandle,
    IReadOnlyList<SwarmWorkerTelemetryPacket> WorkerPackets,
    SwarmCustodyBraidScopeBoundary ScopeBoundary,
    int RequiredWorkerCount,
    int PriorPassageCount);

public sealed record SwarmCustodyBraidReceipt(
    string ReceiptHandle,
    SwarmCustodyBraidDisposition Disposition,
    string OutcomeCode,
    string GovernanceTrace,
    string BraidHandle,
    IReadOnlyList<SwarmWorkerTelemetryPacket> WorkerPackets,
    IReadOnlyList<string> PreservedSourceHandles,
    IReadOnlyDictionary<string, int> RecommendationCounts,
    string? SelectedNextLaneRecommendation,
    decimal AggregateConfidence,
    SwarmCustodyBraidNonPromotionBoundary NonPromotionBoundary,
    SwarmCustodyBraidRefusalReceipt? Refusal,
    int PriorPassageCount,
    int PassageCountAfterBraid,
    bool ReviewOnly,
    bool CandidateOnly,
    bool WorkerConsensusGrantsAuthority,
    bool AggregateConfidenceGrantsWarrant,
    bool NextLaneAuthorized,
    bool ContinuityAdmitted,
    bool ReceiptsReplayed,
    bool NewPacketEmitted,
    bool ActivationRefused,
    bool AuthorityGranted,
    DateTimeOffset TimestampUtc)
{
    public bool IsColdBraid =>
        Disposition is SwarmCustodyBraidDisposition.BraidedForReviewCold or SwarmCustodyBraidDisposition.EmptyReviewCold &&
        ReviewOnly &&
        CandidateOnly &&
        !WorkerConsensusGrantsAuthority &&
        !AggregateConfidenceGrantsWarrant &&
        !NextLaneAuthorized &&
        !ContinuityAdmitted &&
        !ReceiptsReplayed &&
        !NewPacketEmitted &&
        ActivationRefused &&
        !AuthorityGranted &&
        PassageCountAfterBraid == PriorPassageCount;
}

public sealed class DefaultSwarmCustodyBraidBoundaryValidator
{
    private static readonly SwarmCustodyBraidNonPromotionBoundary NonPromotionBoundary = new(
        WorkerConsensusMayAuthorize: false,
        AggregateConfidenceMayBecomeWarrant: false,
        NextLaneNominationMayStartWork: false,
        BraidMayAdmitContinuity: false,
        BraidMayActivate: false,
        BraidMayEmitPackets: false,
        BraidMayReplayReceipts: false,
        BraidMayIncrementPassageCount: false,
        BoundaryLaw: "Many workers may inspect and propose. One custody braid may integrate. Consensus may not become warrant.");

    public SwarmCustodyBraidReceipt Braid(
        SwarmCustodyBraidRequest request,
        DateTimeOffset timestampUtc)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (!request.ScopeBoundary.Present ||
            string.IsNullOrWhiteSpace(request.ScopeBoundary.ScopeCode))
        {
            return Refuse(
                request,
                "swarm-braid-scope-boundary-missing",
                "Swarm custody braid refused because a review-only scope boundary is required.",
                timestampUtc);
        }

        if (!request.ScopeBoundary.ReviewOnly ||
            !request.ScopeBoundary.AllowsNextLaneNomination ||
            request.ScopeBoundary.AllowsAuthority ||
            request.ScopeBoundary.AllowsContinuityAdmission ||
            request.ScopeBoundary.AllowsActivation ||
            request.ScopeBoundary.AllowsConsensusWarrant ||
            request.ScopeBoundary.AllowsAggregateConfidenceWarrant)
        {
            return Refuse(
                request,
                "swarm-braid-promotion-scope-refused",
                "Swarm custody braid refused because scope must allow review-only next-lane nomination while refusing authority, continuity, activation, consensus warrant, and aggregate confidence warrant.",
                timestampUtc);
        }

        if (request.RequiredWorkerCount <= 0)
        {
            return Refuse(
                request,
                "swarm-braid-required-worker-count-missing",
                "Swarm custody braid refused because required worker count must be positive.",
                timestampUtc);
        }

        if (request.WorkerPackets.Count == 0)
        {
            return CreateReceipt(
                request,
                SwarmCustodyBraidDisposition.EmptyReviewCold,
                "swarm-braid-empty-review-only",
                "Swarm custody braid found no worker packets. Empty braid is reviewable but grants no authority, warrant, activation, continuity, or next-lane authorization.",
                workerPackets: [],
                timestampUtc);
        }

        if (request.WorkerPackets.Count != request.RequiredWorkerCount)
        {
            return Refuse(
                request,
                "swarm-braid-worker-count-mismatch",
                "Swarm custody braid refused because worker packet count does not match the required cold swarm count.",
                timestampUtc);
        }

        if (request.WorkerPackets.Any(static packet => !packet.IsColdCandidate))
        {
            return Refuse(
                request,
                "swarm-braid-worker-packet-not-cold",
                "Swarm custody braid refused because at least one worker packet is not review-only, candidate-only, lineage-preserving, confidence-bounded, and non-promotional.",
                timestampUtc);
        }

        if (request.WorkerPackets
            .GroupBy(static packet => packet.WorkerId, StringComparer.Ordinal)
            .Any(static group => group.Count() > 1))
        {
            return Refuse(
                request,
                "swarm-braid-duplicate-worker-refused",
                "Swarm custody braid refused because each worker id must contribute at most one packet to the braid.",
                timestampUtc);
        }

        if (request.WorkerPackets
            .GroupBy(static packet => packet.Domain)
            .Any(static group => group.Count() > 1))
        {
            return Refuse(
                request,
                "swarm-braid-duplicate-domain-refused",
                "Swarm custody braid refused because each domain lane must contribute at most one packet to the braid.",
                timestampUtc);
        }

        if (request.WorkerPackets.Any(static packet => !IsValidBatchSeam(packet.BatchSeam)))
        {
            return Refuse(
                request,
                "swarm-braid-batch-seam-malformed",
                "Swarm custody braid refused because worker batch seams must declare valid 30/60/90 batch and 3/6/9 micro-seam posture.",
                timestampUtc);
        }

        return CreateReceipt(
            request,
            SwarmCustodyBraidDisposition.BraidedForReviewCold,
            "swarm-braid-candidate-review-only",
            "Swarm custody braid integrated cold worker packets into one review-only next-lane nomination while preserving source handles and refusing authority, warrant, continuity, activation, packet emission, and replay.",
            request.WorkerPackets.ToArray(),
            timestampUtc);
    }

    private static bool IsValidBatchSeam(SwarmBatchSeam seam) =>
        seam.BatchIndex is 30 or 60 or 90 &&
        seam.RunStart >= 1 &&
        seam.RunEnd >= seam.RunStart &&
        seam.RunEnd <= seam.BatchIndex &&
        seam.MicroSeam is 3 or 6 or 9 &&
        !string.IsNullOrWhiteSpace(seam.SeamCode);

    private static SwarmCustodyBraidReceipt CreateReceipt(
        SwarmCustodyBraidRequest request,
        SwarmCustodyBraidDisposition disposition,
        string outcomeCode,
        string governanceTrace,
        IReadOnlyList<SwarmWorkerTelemetryPacket> workerPackets,
        DateTimeOffset timestampUtc)
    {
        var preservedSourceHandles = workerPackets
            .SelectMany(static packet => packet.SourceHandles)
            .Where(static handle => !string.IsNullOrWhiteSpace(handle))
            .Distinct(StringComparer.Ordinal)
            .ToArray();
        var recommendationCounts = workerPackets
            .GroupBy(static packet => packet.NextLaneRecommendation, StringComparer.Ordinal)
            .OrderByDescending(static group => group.Count())
            .ThenBy(static group => group.Key, StringComparer.Ordinal)
            .ToDictionary(static group => group.Key, static group => group.Count(), StringComparer.Ordinal);
        var selectedNextLane = recommendationCounts.FirstOrDefault().Key;
        if (string.IsNullOrWhiteSpace(selectedNextLane))
        {
            selectedNextLane = null;
        }

        var aggregateConfidence = workerPackets.Count == 0
            ? 0m
            : Math.Round(workerPackets.Average(static packet => packet.Confidence), 4, MidpointRounding.AwayFromZero);

        return new SwarmCustodyBraidReceipt(
            ReceiptHandle: $"urn:san:swarm-braid:review:{ShortHash(request.BraidHandle, outcomeCode, workerPackets.Count.ToString(System.Globalization.CultureInfo.InvariantCulture), timestampUtc.UtcTicks.ToString(System.Globalization.CultureInfo.InvariantCulture))}",
            Disposition: disposition,
            OutcomeCode: outcomeCode,
            GovernanceTrace: governanceTrace,
            BraidHandle: request.BraidHandle,
            WorkerPackets: workerPackets,
            PreservedSourceHandles: preservedSourceHandles,
            RecommendationCounts: recommendationCounts,
            SelectedNextLaneRecommendation: selectedNextLane,
            AggregateConfidence: aggregateConfidence,
            NonPromotionBoundary: NonPromotionBoundary,
            Refusal: null,
            PriorPassageCount: request.PriorPassageCount,
            PassageCountAfterBraid: request.PriorPassageCount,
            ReviewOnly: true,
            CandidateOnly: true,
            WorkerConsensusGrantsAuthority: false,
            AggregateConfidenceGrantsWarrant: false,
            NextLaneAuthorized: false,
            ContinuityAdmitted: false,
            ReceiptsReplayed: false,
            NewPacketEmitted: false,
            ActivationRefused: true,
            AuthorityGranted: false,
            TimestampUtc: timestampUtc);
    }

    private static SwarmCustodyBraidReceipt Refuse(
        SwarmCustodyBraidRequest request,
        string outcomeCode,
        string governanceTrace,
        DateTimeOffset timestampUtc) =>
        new(
            ReceiptHandle: $"urn:san:swarm-braid:refused:{ShortHash(request.BraidHandle, outcomeCode, timestampUtc.UtcTicks.ToString(System.Globalization.CultureInfo.InvariantCulture))}",
            Disposition: SwarmCustodyBraidDisposition.Refused,
            OutcomeCode: outcomeCode,
            GovernanceTrace: governanceTrace,
            BraidHandle: request.BraidHandle,
            WorkerPackets: request.WorkerPackets,
            PreservedSourceHandles: [],
            RecommendationCounts: new Dictionary<string, int>(StringComparer.Ordinal),
            SelectedNextLaneRecommendation: null,
            AggregateConfidence: 0m,
            NonPromotionBoundary: NonPromotionBoundary,
            Refusal: new SwarmCustodyBraidRefusalReceipt(
                ReceiptHandle: $"urn:san:swarm-braid-refusal:{ShortHash(request.BraidHandle, outcomeCode)}",
                RefusalCode: outcomeCode,
                GovernanceTrace: governanceTrace,
                Retained: true),
            PriorPassageCount: request.PriorPassageCount,
            PassageCountAfterBraid: request.PriorPassageCount,
            ReviewOnly: true,
            CandidateOnly: true,
            WorkerConsensusGrantsAuthority: false,
            AggregateConfidenceGrantsWarrant: false,
            NextLaneAuthorized: false,
            ContinuityAdmitted: false,
            ReceiptsReplayed: false,
            NewPacketEmitted: false,
            ActivationRefused: true,
            AuthorityGranted: false,
            TimestampUtc: timestampUtc);

    private static string ShortHash(params string[] values)
    {
        var material = string.Join("|", values);
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(material));
        return Convert.ToHexString(hash).ToLowerInvariant()[..16];
    }
}
