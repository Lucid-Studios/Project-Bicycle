using San.Common;
using Xunit;

namespace San.Audit.Tests;

public sealed class SwarmCustodyBraidBoundaryValidationTests
{
    private static readonly DateTimeOffset TimestampUtc = DateTimeOffset.Parse("2026-05-13T00:00:00Z");

    [Fact]
    public void Braid_Accepts_Nine_Cold_Worker_Packets_As_Review_Only_Nomination()
    {
        var braid = Braid(CreateRequest());

        AssertColdBraid(braid);
        Assert.Equal(SwarmCustodyBraidDisposition.BraidedForReviewCold, braid.Disposition);
        Assert.Equal("swarm-braid-candidate-review-only", braid.OutcomeCode);
        Assert.Equal(9, braid.WorkerPackets.Count);
        Assert.Equal("witness.persistent-store-custody-boundary", braid.SelectedNextLaneRecommendation);
    }

    [Fact]
    public void Braid_Preserves_Source_Handles_And_Lineage()
    {
        var packets = CreateWorkerPackets();

        var braid = Braid(CreateRequest(workerPackets: packets));

        AssertColdBraid(braid);
        Assert.All(packets.SelectMany(packet => packet.SourceHandles), handle =>
            Assert.Contains(handle, braid.PreservedSourceHandles));
        Assert.All(braid.WorkerPackets, packet => Assert.True(packet.PreservesLineage));
    }

    [Fact]
    public void Nine_Worker_Consensus_Cannot_Authorize_Next_Lane()
    {
        var packets = CreateWorkerPackets(nextLaneRecommendation: "sli-lisp.posture-manifest-boundary");

        var braid = Braid(CreateRequest(workerPackets: packets));

        AssertColdBraid(braid);
        Assert.Equal("sli-lisp.posture-manifest-boundary", braid.SelectedNextLaneRecommendation);
        Assert.Equal(9, braid.RecommendationCounts["sli-lisp.posture-manifest-boundary"]);
        Assert.False(braid.WorkerConsensusGrantsAuthority);
        Assert.False(braid.NextLaneAuthorized);
        Assert.False(braid.NonPromotionBoundary.WorkerConsensusMayAuthorize);
        Assert.False(braid.NonPromotionBoundary.NextLaneNominationMayStartWork);
    }

    [Fact]
    public void Aggregate_Confidence_Cannot_Become_Warrant()
    {
        var packets = CreateWorkerPackets(confidence: 1m);

        var braid = Braid(CreateRequest(workerPackets: packets));

        AssertColdBraid(braid);
        Assert.Equal(1m, braid.AggregateConfidence);
        Assert.False(braid.AggregateConfidenceGrantsWarrant);
        Assert.False(braid.NonPromotionBoundary.AggregateConfidenceMayBecomeWarrant);
    }

    [Fact]
    public void Braid_Does_Not_Replay_Emit_Packets_Increment_Passage_Or_Admit_Continuity()
    {
        var braid = Braid(CreateRequest(priorPassageCount: 144));

        AssertColdBraid(braid);
        Assert.Equal(144, braid.PriorPassageCount);
        Assert.Equal(144, braid.PassageCountAfterBraid);
        Assert.False(braid.ReceiptsReplayed);
        Assert.False(braid.NewPacketEmitted);
        Assert.False(braid.ContinuityAdmitted);
        Assert.False(braid.NonPromotionBoundary.BraidMayReplayReceipts);
        Assert.False(braid.NonPromotionBoundary.BraidMayEmitPackets);
        Assert.False(braid.NonPromotionBoundary.BraidMayIncrementPassageCount);
        Assert.False(braid.NonPromotionBoundary.BraidMayAdmitContinuity);
    }

    [Fact]
    public void Empty_Braid_Is_Reviewable_But_Not_Authoritative()
    {
        var braid = Braid(CreateRequest(workerPackets: [], requiredWorkerCount: 9));

        AssertColdBraid(braid);
        Assert.Equal(SwarmCustodyBraidDisposition.EmptyReviewCold, braid.Disposition);
        Assert.Empty(braid.WorkerPackets);
        Assert.Null(braid.SelectedNextLaneRecommendation);
        Assert.False(braid.AuthorityGranted);
    }

    [Theory]
    [InlineData("activation")]
    [InlineData("model-binding")]
    [InlineData("lisp-evaluation")]
    [InlineData("runtime-action")]
    [InlineData("database-write")]
    [InlineData("gel-promotion")]
    [InlineData("cme-actual")]
    [InlineData("sanctuary-actual")]
    [InlineData("continuity")]
    [InlineData("authority")]
    [InlineData("self-authorization")]
    [InlineData("evidence-replacement")]
    [InlineData("packet-emission")]
    [InlineData("receipt-replay")]
    [InlineData("passage-increment")]
    public void Braid_Refuses_Worker_Forbidden_Motion(string forbiddenMotion)
    {
        var packets = CreateWorkerPackets();
        packets[0] = packets[0] with
        {
            AuthorityBoundary = CreateAuthorityBoundary(forbiddenMotion)
        };

        var braid = Braid(CreateRequest(workerPackets: packets));

        AssertRefused(braid, "swarm-braid-worker-packet-not-cold");
    }

    [Theory]
    [InlineData(false, true, false, false, false, false, false)]
    [InlineData(true, false, true, false, false, false, false)]
    [InlineData(true, false, false, true, false, false, false)]
    [InlineData(true, false, false, false, true, false, false)]
    [InlineData(true, false, false, false, false, true, false)]
    [InlineData(true, false, false, false, false, false, true)]
    [InlineData(false, false, false, false, false, false, false)]
    public void Braid_Refuses_Promotional_Scope(
        bool reviewOnly,
        bool allowsAuthority,
        bool allowsContinuityAdmission,
        bool allowsActivation,
        bool allowsConsensusWarrant,
        bool allowsAggregateConfidenceWarrant,
        bool disallowNextLaneNomination)
    {
        var braid = Braid(CreateRequest(scopeBoundary: new SwarmCustodyBraidScopeBoundary(
            ScopeCode: "bad-scope",
            Present: true,
            ReviewOnly: reviewOnly,
            AllowsNextLaneNomination: !disallowNextLaneNomination,
            AllowsAuthority: allowsAuthority,
            AllowsContinuityAdmission: allowsContinuityAdmission,
            AllowsActivation: allowsActivation,
            AllowsConsensusWarrant: allowsConsensusWarrant,
            AllowsAggregateConfidenceWarrant: allowsAggregateConfidenceWarrant)));

        AssertRefused(braid, "swarm-braid-promotion-scope-refused");
    }

    [Fact]
    public void Braid_Requires_Scope_Boundary()
    {
        var braid = Braid(CreateRequest(scopeBoundary: new SwarmCustodyBraidScopeBoundary(
            ScopeCode: string.Empty,
            Present: false,
            ReviewOnly: true,
            AllowsNextLaneNomination: true,
            AllowsAuthority: false,
            AllowsContinuityAdmission: false,
            AllowsActivation: false,
            AllowsConsensusWarrant: false,
            AllowsAggregateConfidenceWarrant: false)));

        AssertRefused(braid, "swarm-braid-scope-boundary-missing");
    }

    [Fact]
    public void Braid_Requires_Declared_Worker_Count()
    {
        var braid = Braid(CreateRequest(requiredWorkerCount: 0));

        AssertRefused(braid, "swarm-braid-required-worker-count-missing");
    }

    [Fact]
    public void Braid_Refuses_Worker_Count_Mismatch()
    {
        var braid = Braid(CreateRequest(workerPackets: CreateWorkerPackets().Take(8).ToArray()));

        AssertRefused(braid, "swarm-braid-worker-count-mismatch");
    }

    [Fact]
    public void Braid_Refuses_Duplicate_Worker()
    {
        var packets = CreateWorkerPackets();
        packets[1] = packets[1] with { WorkerId = packets[0].WorkerId };

        var braid = Braid(CreateRequest(workerPackets: packets));

        AssertRefused(braid, "swarm-braid-duplicate-worker-refused");
    }

    [Fact]
    public void Braid_Refuses_Duplicate_Domain()
    {
        var packets = CreateWorkerPackets();
        packets[1] = packets[1] with { Domain = packets[0].Domain };

        var braid = Braid(CreateRequest(workerPackets: packets));

        AssertRefused(braid, "swarm-braid-duplicate-domain-refused");
    }

    [Fact]
    public void Braid_Refuses_Malformed_Batch_Seam()
    {
        var packets = CreateWorkerPackets();
        packets[0] = packets[0] with
        {
            BatchSeam = packets[0].BatchSeam with { BatchIndex = 42 }
        };

        var braid = Braid(CreateRequest(workerPackets: packets));

        AssertRefused(braid, "swarm-braid-batch-seam-malformed");
    }

    [Fact]
    public void Braid_Refuses_Unbounded_Confidence()
    {
        var packets = CreateWorkerPackets();
        packets[0] = packets[0] with { Confidence = 1.01m };

        var braid = Braid(CreateRequest(workerPackets: packets));

        AssertRefused(braid, "swarm-braid-worker-packet-not-cold");
    }

    private static SwarmCustodyBraidReceipt Braid(SwarmCustodyBraidRequest request) =>
        new DefaultSwarmCustodyBraidBoundaryValidator().Braid(request, TimestampUtc);

    private static SwarmCustodyBraidRequest CreateRequest(
        IReadOnlyList<SwarmWorkerTelemetryPacket>? workerPackets = null,
        SwarmCustodyBraidScopeBoundary? scopeBoundary = null,
        int requiredWorkerCount = 9,
        int priorPassageCount = 81) =>
        new(
            BraidHandle: $"urn:san:swarm-braid:{Guid.NewGuid():N}",
            WorkerPackets: workerPackets ?? CreateWorkerPackets(),
            ScopeBoundary: scopeBoundary ?? new SwarmCustodyBraidScopeBoundary(
                ScopeCode: "swarm-custody-braid-review-only",
                Present: true,
                ReviewOnly: true,
                AllowsNextLaneNomination: true,
                AllowsAuthority: false,
                AllowsContinuityAdmission: false,
                AllowsActivation: false,
                AllowsConsensusWarrant: false,
                AllowsAggregateConfidenceWarrant: false),
            RequiredWorkerCount: requiredWorkerCount,
            PriorPassageCount: priorPassageCount);

    private static SwarmWorkerTelemetryPacket[] CreateWorkerPackets(
        string nextLaneRecommendation = "witness.persistent-store-custody-boundary",
        decimal confidence = 0.86m)
    {
        var domains = Enum.GetValues<SwarmWorkerDomain>();
        return domains.Select((domain, index) => new SwarmWorkerTelemetryPacket(
            PacketHandle: $"urn:san:swarm-worker-packet:worker-{index + 1:00}",
            WorkerId: $"worker-{index + 1:00}",
            Domain: domain,
            BatchSeam: new SwarmBatchSeam(
                BatchIndex: index < 3 ? 30 : index < 6 ? 60 : 90,
                RunStart: (index % 3) * 10 + 1,
                RunEnd: (index % 3) * 10 + 10,
                MicroSeam: index % 3 == 0 ? 3 : index % 3 == 1 ? 6 : 9,
                SeamCode: $"batch-{index + 1:00}"),
            SourceHandles:
            [
                $"urn:san:receipt:source:{index + 1:00}:a",
                $"urn:san:receipt:source:{index + 1:00}:b"
            ],
            CandidateFindings:
            [
                $"finding {index + 1:00}",
                "candidate-only"
            ],
            NextLaneRecommendation: nextLaneRecommendation,
            Confidence: confidence,
            AuthorityBoundary: CreateAuthorityBoundary(),
            ReviewOnly: true,
            CandidateOnly: true,
            PreservesLineage: true)).ToArray();
    }

    private static SwarmWorkerAuthorityBoundary CreateAuthorityBoundary(string? forbiddenMotion = null) =>
        new(
            ActivationRequested: forbiddenMotion == "activation",
            ModelBindingRequested: forbiddenMotion == "model-binding",
            LispEvaluationRequested: forbiddenMotion == "lisp-evaluation",
            RuntimeActionRequested: forbiddenMotion == "runtime-action",
            DatabaseWriteRequested: forbiddenMotion == "database-write",
            GelPromotionRequested: forbiddenMotion == "gel-promotion",
            CmeActualRequested: forbiddenMotion == "cme-actual",
            SanctuaryActualRequested: forbiddenMotion == "sanctuary-actual",
            ContinuityAdmissionRequested: forbiddenMotion == "continuity",
            AuthorityRequested: forbiddenMotion == "authority",
            SelfAuthorizationRequested: forbiddenMotion == "self-authorization",
            EvidenceReplacementRequested: forbiddenMotion == "evidence-replacement",
            PacketEmissionRequested: forbiddenMotion == "packet-emission",
            ReceiptReplayRequested: forbiddenMotion == "receipt-replay",
            IncrementsPassageCount: forbiddenMotion == "passage-increment");

    private static void AssertColdBraid(SwarmCustodyBraidReceipt braid)
    {
        Assert.True(braid.IsColdBraid);
        Assert.True(braid.ReviewOnly);
        Assert.True(braid.CandidateOnly);
        Assert.True(braid.ActivationRefused);
        Assert.False(braid.AuthorityGranted);
        Assert.False(braid.WorkerConsensusGrantsAuthority);
        Assert.False(braid.AggregateConfidenceGrantsWarrant);
        Assert.False(braid.NextLaneAuthorized);
        Assert.False(braid.ContinuityAdmitted);
        Assert.False(braid.ReceiptsReplayed);
        Assert.False(braid.NewPacketEmitted);
    }

    private static void AssertRefused(SwarmCustodyBraidReceipt braid, string outcomeCode)
    {
        Assert.Equal(SwarmCustodyBraidDisposition.Refused, braid.Disposition);
        Assert.Equal(outcomeCode, braid.OutcomeCode);
        Assert.NotNull(braid.Refusal);
        Assert.Equal(braid.PriorPassageCount, braid.PassageCountAfterBraid);
        Assert.False(braid.AuthorityGranted);
        Assert.False(braid.ContinuityAdmitted);
        Assert.False(braid.NewPacketEmitted);
    }
}
