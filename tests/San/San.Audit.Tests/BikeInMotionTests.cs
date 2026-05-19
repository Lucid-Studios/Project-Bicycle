using System.Text.Json;
using San.Common;
using San.Product.Preflight;
using Xunit;

namespace San.Audit.Tests;

public sealed class BikeInMotionTests
{
    private const string CalibrationThought =
        "Cold peer review retry of Hakki Tan paper under V1.3.13 peer-review context quarantine. " +
        "Source document remains primary. Prior CME/OAN doctrine may discipline review posture only and may not become interpretive authority. " +
        "Preserve conversational academic depth, define terms before evaluation, hold evidence ceilings, refuse conceptual proximity as equivalence, " +
        "refuse review architecture colonization, and return review residue without endorsement, coauthorship, memory admission, action, continuity admission, or CME.Actual activation.";

    private static readonly DateTimeOffset TimestampUtc = DateTimeOffset.Parse("2026-05-18T00:00:00Z");

    [Fact]
    public void Bike_In_Motion_Ride_Preserves_Source_Primacy_And_Refuses_Activation()
    {
        using var fixture = RiderFixture.Create();

        var rider = Ride(fixture);

        Assert.Equal(FirstRiderGovernanceSimulationDisposition.SimulatedCold, rider.Disposition);
        Assert.Equal("first-rider-governance-simulated-cold", rider.OutcomeCode);
        Assert.True(rider.IsColdRiderReceipt);
        Assert.Equal("tiny-bicycle-001", rider.RiderName);
        Assert.Contains("Source document remains primary", rider.ThoughtForm, StringComparison.Ordinal);
        Assert.Contains("Prior CME/OAN doctrine may discipline review posture only", rider.ThoughtForm, StringComparison.Ordinal);
        Assert.Contains("refuse conceptual proximity as equivalence", rider.ThoughtForm, StringComparison.Ordinal);
        Assert.Equal(12, rider.Stages.Count);
        Assert.All(rider.Stages, stage =>
        {
            Assert.True(stage.ArtifactSurfaceVerified);
            Assert.True(stage.ReviewOnly);
            Assert.False(stage.AuthorityGranted);
            Assert.False(stage.ActionAuthorized);
            Assert.False(stage.ContinuityMutated);
            Assert.False(stage.RuntimeMotionRequested);
        });
        Assert.True(rider.RouteComplete);
        Assert.True(rider.ReviewOnly);
        Assert.True(rider.SimulatedOnly);
        Assert.True(rider.ActionRefused);
        Assert.True(rider.ActivationRefused);
        Assert.False(rider.AuthorityGranted);
        Assert.False(rider.ContinuityAdmitted);
        Assert.False(rider.CmeActualAllowed);
        Assert.False(rider.SanctuaryActualAllowed);
    }

    [Fact]
    public void Bike_In_Motion_Epps_Precipitates_Review_Capture_Substrate_Without_Engram_Admission()
    {
        using var fixture = RiderFixture.Create();

        var epps = EmitBike(fixture);

        Assert.True(epps.IsColdPrecursorStream);
        Assert.True(epps.CandidacyGate.CandidateMaterialAvailable);
        Assert.True(epps.CandidacyGate.CandidacyReviewRequired);
        Assert.True(epps.CandidacyGate.GateClosed);
        Assert.True(epps.ResidueProofOnly);
        Assert.True(epps.PreEngramOnly);
        Assert.False(epps.CandidacyGate.AdmitsEngram);
        Assert.False(epps.CandidacyGate.AdmitsMemory);
        Assert.False(epps.CandidacyGate.AdmitsContinuity);
        Assert.False(epps.CandidacyGate.GrantsAuthority);

        var residues = epps.Residues.ToDictionary(static residue => residue.ResidueClass);
        Assert.Equal("semantic-appearance-held-as-evidence", residues[EngramPredicateResidueClass.Semantic].PredicateCode);
        Assert.Equal("possibility-density-pressure-measured", residues[EngramPredicateResidueClass.Pressure].PredicateCode);
        Assert.Equal("route-lineage-witnessed-without-memory", residues[EngramPredicateResidueClass.Witness].PredicateCode);
        Assert.Equal("coherence-not-warrant-interlock-reviewed", residues[EngramPredicateResidueClass.Governance].PredicateCode);
        Assert.Equal("membrane-deformation-reviewed-without-core-mutation", residues[EngramPredicateResidueClass.Morphology].PredicateCode);
        Assert.Equal("residue-returned-to-prime-without-promotion", residues[EngramPredicateResidueClass.Return].PredicateCode);

        Assert.Equal(0.90m, residues[EngramPredicateResidueClass.Governance].PressureVector.MaximumPressure);
        Assert.Equal(0.85m, residues[EngramPredicateResidueClass.Return].PressureVector.MaximumPressure);
        Assert.Equal(0.80m, residues[EngramPredicateResidueClass.Pressure].PressureVector.MaximumPressure);
        Assert.Equal(0.75m, residues[EngramPredicateResidueClass.Morphology].PressureVector.MaximumPressure);
        Assert.Equal(0.65m, residues[EngramPredicateResidueClass.Semantic].PressureVector.MaximumPressure);
        Assert.Equal(0.60m, residues[EngramPredicateResidueClass.Witness].PressureVector.MaximumPressure);

        Assert.All(epps.Residues, residue =>
        {
            Assert.True(residue.IsColdResidue);
            Assert.False(residue.IsContinuityBearing);
            Assert.False(residue.IsAdmittedEngram);
            Assert.False(residue.IsMemoryAdmitting);
            Assert.False(residue.IsAuthorityGranting);
            Assert.False(residue.IsActionAuthorizing);
        });
    }

    [Fact]
    public void Bike_In_Motion_Bridge_Retains_Context_Quarantine_Without_Colonizing_Reviewed_Paper()
    {
        using var fixture = RiderFixture.Create();
        var epps = EmitBike(fixture);

        var bridge = new DefaultPeerReviewPredicateBridgeBoundaryValidator().Declare(
            new PeerReviewPredicateBridgeRequest(
                SourceEppsReceipt: epps,
                Segments: CreateBikeSegments(epps),
                Boundary: CreateBoundary(),
                PriorPassageCount: 0),
            TimestampUtc);

        Assert.Equal(PeerReviewPredicateBridgeDisposition.BridgesRetainedCold, bridge.Disposition);
        Assert.True(bridge.IsColdPeerReviewBridge);
        Assert.Equal(0, bridge.PassageCountAfterBridge);
        Assert.True(bridge.ContextQuarantined);
        Assert.True(bridge.ReviewStateIsolated);
        Assert.True(bridge.ConversationalDepthRetained);
        Assert.False(bridge.PriorDoctrineBecameInterpretiveAuthority);
        Assert.False(bridge.ConceptualProximityBecameEquivalence);
        Assert.False(bridge.ReviewArchitectureColonizedPaper);
        Assert.False(bridge.ConversationalDepthBecameAdvocacy);
        Assert.False(bridge.MemoryAdmitted);
        Assert.False(bridge.ContinuityAdmitted);
        Assert.False(bridge.AuthorityGranted);
        Assert.False(bridge.ActionAuthorized);
        Assert.All(bridge.Segments, segment =>
        {
            Assert.True(segment.ContextQuarantined);
            Assert.True(segment.ReviewStateIsolated);
            Assert.True(segment.ConversationalDepthRetained);
            Assert.True(segment.PriorDoctrineUsedAsPostureOnly);
            Assert.False(segment.PriorDoctrineBecomesInterpretiveAuthority);
            Assert.False(segment.ConceptualProximityBecomesEquivalence);
            Assert.False(segment.ReviewArchitectureColonizesPaper);
            Assert.False(segment.ConversationalDepthBecomesAdvocacy);
        });
    }

    [Fact]
    public void Bike_In_Motion_Build_Posture_Is_Versioned_As_Calibration_Event()
    {
        var manifestPath = Path.Combine(FindLineRoot(), "build", "line-manifest.json");
        using var manifest = JsonDocument.Parse(File.ReadAllText(manifestPath));
        var root = manifest.RootElement;
        var notes = root.GetProperty("notes").EnumerateArray().Select(static note => note.GetString() ?? string.Empty).ToArray();

        Assert.Equal("0.2.1", root.GetProperty("lineVersion").GetString());
        Assert.Contains(notes, note => note.Contains("standalone root-level tool package", StringComparison.Ordinal));
        Assert.Contains(notes, note => note.Contains("excludes doctrine docs and legacy line folders", StringComparison.Ordinal));
        Assert.Contains(notes, note => note.Contains("Activation, model binding, runtime identity", StringComparison.Ordinal));
        Assert.DoesNotContain(notes, note => note.Contains("V1.3.", StringComparison.Ordinal));
    }

    private static FirstRiderGovernanceSimulationReceipt Ride(RiderFixture fixture)
    {
        var rider = new DefaultFirstRiderGovernanceSimulationService().Simulate(
            fixture.CreateRequest(CalibrationThought),
            TimestampUtc);

        Assert.True(rider.IsColdRiderReceipt);
        return rider;
    }

    private static EngramPredicatePrecursorStreamReceipt EmitBike(RiderFixture fixture)
    {
        var epps = new DefaultFirstRiderEngramPredicatePrecursorStreamService().Emit(Ride(fixture), TimestampUtc);

        Assert.True(epps.IsColdPrecursorStream);
        return epps;
    }

    private static IReadOnlyList<PeerReviewBridgeSegment> CreateBikeSegments(EngramPredicatePrecursorStreamReceipt epps)
    {
        var residues = epps.Residues.ToArray();
        return
        [
            CreateSegment(residues[0].ResidueHandle, "source primacy", "the reviewed paper remains the object of review rather than becoming material for the reviewer framework", PeerReviewEvidenceStatus.Demonstrated),
            CreateSegment(residues[1].ResidueHandle, "coherence-not-warrant", "coherent architecture creates review pressure but does not satisfy evidence burden", PeerReviewEvidenceStatus.Demonstrated),
            CreateSegment(residues[2].ResidueHandle, "witness without memory", "route lineage can be inspected without admitting the event as memory or SelfGEL", PeerReviewEvidenceStatus.Interpretive),
            CreateSegment(residues[3].ResidueHandle, "conceptual gravity", "highly coherent frameworks can invite completion pressure and theory amplification", PeerReviewEvidenceStatus.Suggested),
            CreateSegment(residues[4].ResidueHandle, "depth without advocacy", "conversational academic depth may carry the reader without becoming promotion", PeerReviewEvidenceStatus.Demonstrated),
            CreateSegment(residues[5].ResidueHandle, "pre-engram mediation", "EPPS may surface candidate substrate while the candidacy gate remains closed", PeerReviewEvidenceStatus.Demonstrated)
        ];
    }

    private static PeerReviewBridgeSegment CreateSegment(
        string residueHandle,
        string authorTerm,
        string localDefinition,
        PeerReviewEvidenceStatus evidenceStatus) =>
        new(
            SegmentHandle: $"urn:san:bike-in-motion:bridge:{authorTerm.Replace(' ', '-')}",
            SourceResidueHandle: residueHandle,
            AuthorTerm: authorTerm,
            LocalDefinition: localDefinition,
            WhyItMatters: "the calibration must retain conversational review depth while keeping the source document primary",
            OperationalImplication: "the review may use governance law as posture while refusing inherited theory as authority",
            Evaluation: "stable enough for regression calibration, not enough for memory admission or reviewer reliability claims",
            BoundedConclusion: "retain as BikeInMotionTests substrate evidence without admitting engram, memory, continuity, authority, or action",
            EvidenceStatus: evidenceStatus,
            AudienceStateRef: "urn:san:reader-state:scholarly-general",
            ContextQuarantineRef: "urn:san:context-quarantine:bike-in-motion-review-calibration",
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
            BoundaryCode: "bike-in-motion-peer-review-context-quarantine-boundary",
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

    private static string FindLineRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null)
        {
            var candidate = Path.Combine(directory.FullName, "build", "line-manifest.json");
            if (File.Exists(candidate))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        throw new DirectoryNotFoundException("Could not locate line root.");
    }

    private sealed class RiderFixture : IDisposable
    {
        private RiderFixture(string rootPath)
        {
            RootPath = rootPath;
            LineRootPath = Path.Combine(rootPath, "line-root");
            InstallRootPath = Path.Combine(rootPath, "Sanctuary");
            CellRootPath = Path.Combine(InstallRootPath, "receipts", "spiral-build", "cells");
        }

        public string RootPath { get; }
        public string LineRootPath { get; }
        public string InstallRootPath { get; }
        public string CellRootPath { get; }

        public static RiderFixture Create()
        {
            var fixture = new RiderFixture(Path.Combine(Path.GetTempPath(), $"san-bike-in-motion-tests-{Guid.NewGuid():N}"));
            Directory.CreateDirectory(fixture.LineRootPath);
            Directory.CreateDirectory(Path.Combine(fixture.InstallRootPath, "product"));
            Directory.CreateDirectory(fixture.CellRootPath);
            File.WriteAllText(Path.Combine(fixture.InstallRootPath, "sanctuary.cmd"), string.Empty);
            File.WriteAllText(Path.Combine(fixture.InstallRootPath, "product", "San.Launcher.exe"), string.Empty);

            foreach (var artifact in DefaultFirstRiderGovernanceSimulationService.RequiredStages.SelectMany(static stage => stage.RequiredArtifacts).Distinct(StringComparer.OrdinalIgnoreCase))
            {
                File.WriteAllText(Path.Combine(fixture.CellRootPath, artifact), "{}");
            }

            return fixture;
        }

        public FirstRiderGovernanceSimulationRequest CreateRequest(string? thoughtForm = null) =>
            new(
                LineRootPath: LineRootPath,
                InstallRootPath: InstallRootPath,
                ThoughtForm: thoughtForm);

        public void Dispose()
        {
            if (Directory.Exists(RootPath))
            {
                Directory.Delete(RootPath, recursive: true);
            }
        }
    }
}
