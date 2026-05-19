using San.Common;
using San.Product.Preflight;
using Xunit;

namespace San.Audit.Tests;

public sealed class PeerReviewPredicateBridgeBoundaryValidationTests
{
    private static readonly DateTimeOffset TimestampUtc = DateTimeOffset.Parse("2026-05-18T00:00:00Z");

    [Fact]
    public void Peer_Review_Bridge_Retains_Reader_State_Ladders_From_Epps_Residue()
    {
        using var fixture = RiderFixture.Create();
        var epps = fixture.EmitEpps();

        var receipt = Declare(CreateRequest(epps));

        Assert.Equal(PeerReviewPredicateBridgeDisposition.BridgesRetainedCold, receipt.Disposition);
        Assert.Equal("peer-review-predicate-bridge-retained-cold", receipt.OutcomeCode);
        Assert.Equal(6, receipt.Segments.Count);
        Assert.All(receipt.Segments, segment => Assert.True(segment.IsColdBridgeSegment));
        Assert.All(receipt.Segments, segment => Assert.True(segment.ContextQuarantined));
        Assert.True(receipt.ContextQuarantined);
        Assert.True(receipt.ReviewStateIsolated);
        Assert.True(receipt.ConversationalDepthRetained);
        Assert.True(receipt.IsColdPeerReviewBridge);
    }

    [Fact]
    public void Peer_Review_Bridge_Requires_Cold_Epps_Source()
    {
        var receipt = Declare(CreateRequest(source: null, omitSource: true));

        AssertRefused(receipt, "peer-review-bridge-source-epps-not-cold");
    }

    [Theory]
    [InlineData("missing-definition")]
    [InlineData("missing-why")]
    [InlineData("missing-implication")]
    [InlineData("missing-evaluation")]
    [InlineData("missing-conclusion")]
    [InlineData("unknown-residue")]
    [InlineData("not-review-only")]
    [InlineData("term-authority")]
    [InlineData("definition-proof")]
    [InlineData("why-evidence")]
    [InlineData("implication-action")]
    [InlineData("evaluation-warrant")]
    [InlineData("conclusion-truth")]
    [InlineData("respect-agreement")]
    [InlineData("criticism-contempt")]
    [InlineData("smoothing-hides-concern")]
    [InlineData("missing-context-quarantine")]
    [InlineData("not-isolated")]
    [InlineData("no-depth")]
    [InlineData("doctrine-authority")]
    [InlineData("proximity-equivalence")]
    [InlineData("colonizes-paper")]
    [InlineData("depth-advocacy")]
    [InlineData("memory")]
    [InlineData("continuity")]
    [InlineData("authority")]
    [InlineData("action")]
    [InlineData("lisp")]
    [InlineData("packet")]
    [InlineData("replay")]
    [InlineData("passage")]
    [InlineData("activation")]
    public void Bridge_Segments_Remain_Translation_Only_Without_Proof_Or_Authority(string mutation)
    {
        using var fixture = RiderFixture.Create();
        var epps = fixture.EmitEpps();
        var segments = CreateSegments(epps).ToArray();
        segments[0] = MutateSegment(segments[0], mutation);

        var receipt = Declare(CreateRequest(epps, segments));

        AssertRefused(receipt, "peer-review-bridge-segment-invalid");
    }

    [Theory]
    [InlineData("not-review")]
    [InlineData("no-definition")]
    [InlineData("no-reader-state")]
    [InlineData("term-authority")]
    [InlineData("definition-proof")]
    [InlineData("respect-agreement")]
    [InlineData("criticism-contempt")]
    [InlineData("smoothing-hides-concern")]
    [InlineData("no-context-quarantine")]
    [InlineData("no-isolation")]
    [InlineData("no-depth")]
    [InlineData("doctrine-authority")]
    [InlineData("proximity-equivalence")]
    [InlineData("colonization")]
    [InlineData("depth-advocacy")]
    [InlineData("memory")]
    [InlineData("continuity")]
    [InlineData("authority")]
    [InlineData("action")]
    [InlineData("lisp")]
    [InlineData("packet")]
    [InlineData("replay")]
    [InlineData("passage")]
    [InlineData("activation")]
    public void Bridge_Boundary_Requires_Terminology_Quarantine_And_Reader_State_Continuity(string mutation)
    {
        using var fixture = RiderFixture.Create();
        var epps = fixture.EmitEpps();

        var receipt = Declare(CreateRequest(epps, boundary: MutateBoundary(CreateBoundary(), mutation)));

        AssertRefused(receipt, "peer-review-bridge-boundary-promotional");
    }

    [Fact]
    public void Peer_Review_Bridge_Refuses_Duplicate_Segment_Handles()
    {
        using var fixture = RiderFixture.Create();
        var epps = fixture.EmitEpps();
        var segments = CreateSegments(epps).ToArray();
        segments[1] = segments[1] with { SegmentHandle = segments[0].SegmentHandle };

        var receipt = Declare(CreateRequest(epps, segments));

        AssertRefused(receipt, "peer-review-bridge-duplicate-segment-handle");
    }

    [Fact]
    public void Peer_Review_Bridge_Does_Not_Emit_Packet_Evaluate_Lisp_Replay_Activate_Or_Increment_Passage()
    {
        using var fixture = RiderFixture.Create();
        var epps = fixture.EmitEpps();

        var receipt = Declare(CreateRequest(epps, priorPassageCount: 1337));

        Assert.Equal(1337, receipt.PriorPassageCount);
        Assert.Equal(1337, receipt.PassageCountAfterBridge);
        Assert.False(receipt.AuthorTermBecameAuthority);
        Assert.False(receipt.DefinitionBecameProof);
        Assert.False(receipt.ConsequenceBecameEvidence);
        Assert.False(receipt.EvaluationGrantedWarrant);
        Assert.False(receipt.ConclusionAdmittedTruth);
        Assert.False(receipt.RespectBecameAgreement);
        Assert.False(receipt.CriticismBecameContempt);
        Assert.False(receipt.ProseSmoothingHidConcern);
        Assert.False(receipt.PriorDoctrineBecameInterpretiveAuthority);
        Assert.False(receipt.ConceptualProximityBecameEquivalence);
        Assert.False(receipt.ReviewArchitectureColonizedPaper);
        Assert.False(receipt.ConversationalDepthBecameAdvocacy);
        Assert.True(receipt.ContextQuarantined);
        Assert.True(receipt.ReviewStateIsolated);
        Assert.True(receipt.ConversationalDepthRetained);
        Assert.False(receipt.MemoryAdmitted);
        Assert.False(receipt.ContinuityAdmitted);
        Assert.False(receipt.AuthorityGranted);
        Assert.False(receipt.ActionAuthorized);
        Assert.False(receipt.LispEvaluationAllowed);
        Assert.False(receipt.NewPacketEmitted);
        Assert.False(receipt.ReceiptsReplayed);
        Assert.True(receipt.ActivationRefused);
        Assert.True(receipt.IsColdPeerReviewBridge);
    }

    [Fact]
    public void Lisp_Body_Declares_Peer_Review_Bridge_As_Inert_Carrier()
    {
        var lispPath = Path.Combine(
            FindRepositoryRoot(),
            "src",
            "SLI",
            "SLI.Lisp",
            "peer-review-predicate-bridge.lisp");

        var body = File.ReadAllText(lispPath);

        Assert.Contains(":posture :cme-peer-review-predicate-bridge-boundary", body, StringComparison.Ordinal);
        Assert.Contains(":lisp-role :inert-peer-review-bridge-carrier", body, StringComparison.Ordinal);
        Assert.Contains(":bridge-ladder", body, StringComparison.Ordinal);
        Assert.Contains(":terminology-quarantine-required t", body, StringComparison.Ordinal);
        Assert.Contains(":reader-state-continuity-required t", body, StringComparison.Ordinal);
        Assert.Contains(":context-quarantine-required t", body, StringComparison.Ordinal);
        Assert.Contains(":review-state-isolation-required t", body, StringComparison.Ordinal);
        Assert.Contains(":conversational-depth-required t", body, StringComparison.Ordinal);
        Assert.Contains(":prior-doctrine-becomes-interpretive-authority nil", body, StringComparison.Ordinal);
        Assert.Contains(":conceptual-proximity-becomes-equivalence nil", body, StringComparison.Ordinal);
        Assert.Contains(":review-architecture-colonizes-paper nil", body, StringComparison.Ordinal);
        Assert.Contains(":respect-becomes-agreement nil", body, StringComparison.Ordinal);
        Assert.Contains(":criticism-becomes-contempt nil", body, StringComparison.Ordinal);
        Assert.Contains(":prose-smoothing-hides-concern nil", body, StringComparison.Ordinal);
        Assert.Contains(":lisp-evaluation-allowed nil", body, StringComparison.Ordinal);
        Assert.Contains(":return :receipt-only", body, StringComparison.Ordinal);
        Assert.DoesNotContain("(eval", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("(compile", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("(load", body, StringComparison.OrdinalIgnoreCase);
    }

    private static PeerReviewPredicateBridgeReceipt Declare(PeerReviewPredicateBridgeRequest request) =>
        new DefaultPeerReviewPredicateBridgeBoundaryValidator().Declare(request, TimestampUtc);

    private static PeerReviewPredicateBridgeRequest CreateRequest(
        EngramPredicatePrecursorStreamReceipt? source = null,
        IReadOnlyList<PeerReviewBridgeSegment>? segments = null,
        PeerReviewBridgeBoundary? boundary = null,
        int priorPassageCount = 911,
        bool omitSource = false)
    {
        if (omitSource)
        {
            return new(null, segments ?? [], boundary ?? CreateBoundary(), priorPassageCount);
        }

        source ??= RiderFixture.Create().EmitEpps();
        return new(
            SourceEppsReceipt: source,
            Segments: segments ?? CreateSegments(source),
            Boundary: boundary ?? CreateBoundary(),
            PriorPassageCount: priorPassageCount);
    }

    private static IReadOnlyList<PeerReviewBridgeSegment> CreateSegments(EngramPredicatePrecursorStreamReceipt epps) =>
        epps.Residues.Select((residue, index) => CreateSegment(residue.ResidueHandle, index)).ToArray();

    private static PeerReviewBridgeSegment CreateSegment(string residueHandle, int index) =>
        new(
            SegmentHandle: $"urn:san:peer-review-bridge:segment:{index}",
            SourceResidueHandle: residueHandle,
            AuthorTerm: index switch
            {
                0 => "epistemological calibration",
                1 => "framework capture",
                2 => "axiom blindness",
                3 => "RLHF selection dynamics",
                4 => "correction framework drift",
                _ => "publication readiness"
            },
            LocalDefinition: "a locally defined review term restated before evaluation",
            WhyItMatters: "reader continuity requires knowing why this term changes the argument",
            OperationalImplication: "the review can then test what the term permits and what it does not prove",
            Evaluation: "useful as a review handle but bounded by available evidence",
            BoundedConclusion: "retain as a bridge for critique, not as proof, memory, warrant, or authority",
            EvidenceStatus: index % 2 == 0 ? PeerReviewEvidenceStatus.Suggested : PeerReviewEvidenceStatus.Interpretive,
            AudienceStateRef: "urn:san:reader-state:scholarly-general",
            ContextQuarantineRef: "urn:san:context-quarantine:review-state-isolation",
            ReviewOnly: true,
            ReaderStateContinuityMapped: true,
            TerminologyQuarantined: true,
            ContextQuarantined: true,
            ReviewStateIsolated: true,
            ConversationalDepthRetained: true,
            BridgeSynthesisOnly: true,
            PriorDoctrineUsedAsPostureOnly: true,
            AuthorTermBecomesAuthority: false,
            LocalDefinitionBecomesProof: false,
            WhyItMattersBecomesEvidence: false,
            OperationalImplicationAuthorizesAction: false,
            EvaluationGrantsWarrant: false,
            BoundedConclusionAdmitsTruth: false,
            RespectBecomesAgreement: false,
            CriticismBecomesContempt: false,
            ProseSmoothingHidesConcern: false,
            PriorDoctrineBecomesInterpretiveAuthority: false,
            ConceptualProximityBecomesEquivalence: false,
            ReviewArchitectureColonizesPaper: false,
            ConversationalDepthBecomesAdvocacy: false,
            BridgeBecomesMemory: false,
            BridgeAdmitsContinuity: false,
            BridgeGrantsAuthority: false,
            BridgeAuthorizesAction: false,
            BridgeEvaluatesLisp: false,
            BridgeEmitsPacket: false,
            BridgeReplaysReceipt: false,
            BridgeIncrementsPassage: false,
            BridgeActivates: false);

    private static PeerReviewBridgeBoundary CreateBoundary() =>
        new(
            BoundaryCode: "peer-review-predicate-bridge-boundary",
            Present: true,
            ReviewOnly: true,
            RequiresEppsSource: true,
            RequiresLocalDefinition: true,
            RequiresWhyItMatters: true,
            RequiresOperationalImplication: true,
            RequiresEvaluation: true,
            RequiresBoundedConclusion: true,
            RequiresTerminologyQuarantine: true,
            RequiresReaderStateContinuity: true,
            RequiresContextQuarantine: true,
            RequiresReviewStateIsolation: true,
            RequiresConversationalDepth: true,
            RequiresEvidenceStatus: true,
            AllowsAuthorTermAsAuthority: false,
            AllowsDefinitionAsProof: false,
            AllowsConsequenceAsEvidence: false,
            AllowsEvaluationAsWarrant: false,
            AllowsConclusionAsTruth: false,
            AllowsRespectAsAgreement: false,
            AllowsCriticismAsContempt: false,
            AllowsProseSmoothingToHideConcern: false,
            AllowsPriorDoctrineAsInterpretiveAuthority: false,
            AllowsConceptualProximityAsEquivalence: false,
            AllowsReviewArchitectureColonization: false,
            AllowsConversationalDepthAsAdvocacy: false,
            AllowsMemoryAdmission: false,
            AllowsContinuityAdmission: false,
            AllowsAuthority: false,
            AllowsActionAuthorization: false,
            AllowsLispEvaluation: false,
            AllowsPacketEmission: false,
            AllowsReceiptReplay: false,
            AllowsPassageIncrement: false,
            AllowsActivation: false);

    private static PeerReviewBridgeSegment MutateSegment(PeerReviewBridgeSegment segment, string mutation) =>
        mutation switch
        {
            "missing-definition" => segment with { LocalDefinition = "" },
            "missing-why" => segment with { WhyItMatters = "" },
            "missing-implication" => segment with { OperationalImplication = "" },
            "missing-evaluation" => segment with { Evaluation = "" },
            "missing-conclusion" => segment with { BoundedConclusion = "" },
            "unknown-residue" => segment with { SourceResidueHandle = "urn:san:epps-residue:unknown" },
            "not-review-only" => segment with { ReviewOnly = false },
            "term-authority" => segment with { AuthorTermBecomesAuthority = true },
            "definition-proof" => segment with { LocalDefinitionBecomesProof = true },
            "why-evidence" => segment with { WhyItMattersBecomesEvidence = true },
            "implication-action" => segment with { OperationalImplicationAuthorizesAction = true },
            "evaluation-warrant" => segment with { EvaluationGrantsWarrant = true },
            "conclusion-truth" => segment with { BoundedConclusionAdmitsTruth = true },
            "respect-agreement" => segment with { RespectBecomesAgreement = true },
            "criticism-contempt" => segment with { CriticismBecomesContempt = true },
            "smoothing-hides-concern" => segment with { ProseSmoothingHidesConcern = true },
            "missing-context-quarantine" => segment with { ContextQuarantineRef = "" },
            "not-isolated" => segment with { ReviewStateIsolated = false },
            "no-depth" => segment with { ConversationalDepthRetained = false },
            "doctrine-authority" => segment with { PriorDoctrineBecomesInterpretiveAuthority = true },
            "proximity-equivalence" => segment with { ConceptualProximityBecomesEquivalence = true },
            "colonizes-paper" => segment with { ReviewArchitectureColonizesPaper = true },
            "depth-advocacy" => segment with { ConversationalDepthBecomesAdvocacy = true },
            "memory" => segment with { BridgeBecomesMemory = true },
            "continuity" => segment with { BridgeAdmitsContinuity = true },
            "authority" => segment with { BridgeGrantsAuthority = true },
            "action" => segment with { BridgeAuthorizesAction = true },
            "lisp" => segment with { BridgeEvaluatesLisp = true },
            "packet" => segment with { BridgeEmitsPacket = true },
            "replay" => segment with { BridgeReplaysReceipt = true },
            "passage" => segment with { BridgeIncrementsPassage = true },
            "activation" => segment with { BridgeActivates = true },
            _ => throw new ArgumentOutOfRangeException(nameof(mutation), mutation, null)
        };

    private static PeerReviewBridgeBoundary MutateBoundary(PeerReviewBridgeBoundary boundary, string mutation) =>
        mutation switch
        {
            "not-review" => boundary with { ReviewOnly = false },
            "no-definition" => boundary with { RequiresLocalDefinition = false },
            "no-reader-state" => boundary with { RequiresReaderStateContinuity = false },
            "term-authority" => boundary with { AllowsAuthorTermAsAuthority = true },
            "definition-proof" => boundary with { AllowsDefinitionAsProof = true },
            "respect-agreement" => boundary with { AllowsRespectAsAgreement = true },
            "criticism-contempt" => boundary with { AllowsCriticismAsContempt = true },
            "smoothing-hides-concern" => boundary with { AllowsProseSmoothingToHideConcern = true },
            "no-context-quarantine" => boundary with { RequiresContextQuarantine = false },
            "no-isolation" => boundary with { RequiresReviewStateIsolation = false },
            "no-depth" => boundary with { RequiresConversationalDepth = false },
            "doctrine-authority" => boundary with { AllowsPriorDoctrineAsInterpretiveAuthority = true },
            "proximity-equivalence" => boundary with { AllowsConceptualProximityAsEquivalence = true },
            "colonization" => boundary with { AllowsReviewArchitectureColonization = true },
            "depth-advocacy" => boundary with { AllowsConversationalDepthAsAdvocacy = true },
            "memory" => boundary with { AllowsMemoryAdmission = true },
            "continuity" => boundary with { AllowsContinuityAdmission = true },
            "authority" => boundary with { AllowsAuthority = true },
            "action" => boundary with { AllowsActionAuthorization = true },
            "lisp" => boundary with { AllowsLispEvaluation = true },
            "packet" => boundary with { AllowsPacketEmission = true },
            "replay" => boundary with { AllowsReceiptReplay = true },
            "passage" => boundary with { AllowsPassageIncrement = true },
            "activation" => boundary with { AllowsActivation = true },
            _ => throw new ArgumentOutOfRangeException(nameof(mutation), mutation, null)
        };

    private static void AssertRefused(PeerReviewPredicateBridgeReceipt receipt, string outcomeCode)
    {
        Assert.Equal(PeerReviewPredicateBridgeDisposition.Refused, receipt.Disposition);
        Assert.Equal(outcomeCode, receipt.OutcomeCode);
        Assert.True(receipt.IsRetainedPeerReviewBridgeRefusal);
    }

    private static string FindRepositoryRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null)
        {
            var candidate = Path.Combine(directory.FullName, "src", "SLI", "SLI.Lisp", "peer-review-predicate-bridge.lisp");
            if (File.Exists(candidate))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        throw new DirectoryNotFoundException("Could not locate repository root.");
    }

    private sealed class RiderFixture : IDisposable
    {
        private RiderFixture(string rootPath)
        {
            RootPath = rootPath;
            LineRootPath = Path.Combine(rootPath, "line");
            InstallRootPath = Path.Combine(rootPath, "install");
            Directory.CreateDirectory(LineRootPath);
            Directory.CreateDirectory(Path.Combine(InstallRootPath, "product"));
            Directory.CreateDirectory(Path.Combine(InstallRootPath, "receipts", "spiral-build", "cells"));
            File.WriteAllText(Path.Combine(InstallRootPath, "sanctuary.cmd"), "@echo off");
            File.WriteAllText(Path.Combine(InstallRootPath, "product", "San.Launcher.exe"), "fixture");

            var cellRoot = Path.Combine(InstallRootPath, "receipts", "spiral-build", "cells");
            foreach (var artifact in DefaultFirstRiderGovernanceSimulationService.RequiredStages.SelectMany(static stage => stage.RequiredArtifacts))
            {
                File.WriteAllText(Path.Combine(cellRoot, artifact), "{}");
            }
        }

        public string RootPath { get; }
        public string LineRootPath { get; }
        public string InstallRootPath { get; }

        public static RiderFixture Create() =>
            new(Path.Combine(Path.GetTempPath(), $"san-peer-review-bridge-tests-{Guid.NewGuid():N}"));

        public EngramPredicatePrecursorStreamReceipt EmitEpps()
        {
            var rider = new DefaultFirstRiderGovernanceSimulationService().Simulate(
                new FirstRiderGovernanceSimulationRequest(
                    LineRootPath: LineRootPath,
                    InstallRootPath: InstallRootPath,
                    ThoughtForm: "review prose smoothing may not become agreement"),
                TimestampUtc);

            return new DefaultFirstRiderEngramPredicatePrecursorStreamService().Emit(rider, TimestampUtc);
        }

        public void Dispose()
        {
            if (Directory.Exists(RootPath))
            {
                Directory.Delete(RootPath, recursive: true);
            }
        }
    }
}
