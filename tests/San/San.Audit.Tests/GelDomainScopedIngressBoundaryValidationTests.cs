using San.Common;
using San.Product.Preflight;
using Xunit;

namespace San.Audit.Tests;

public sealed class GelDomainScopedIngressBoundaryValidationTests
{
    private static readonly DateTimeOffset TimestampUtc = DateTimeOffset.Parse("2026-05-18T00:00:00Z");

    [Fact]
    public void Gel_Domain_Ingress_Recommends_Scholarly_Review_Substrate_Without_Admission()
    {
        using var fixture = IngressFixture.Create();
        var sources = fixture.CreateSources();

        var receipt = Declare(CreateRequest(sources));

        Assert.Equal(GelDomainScopedIngressDisposition.RecommendedCold, receipt.Disposition);
        Assert.Equal("gel-domain-ingress-recommended-cold", receipt.OutcomeCode);
        Assert.True(receipt.IsColdIngressRecommendation);
        Assert.Equal(10, receipt.CycleTrace.Count);
        Assert.Equal(1, receipt.RecommendationCountAfterIngress);
        Assert.Equal(0, receipt.PassageCountAfterIngress);
        Assert.True(receipt.CandidateSubstrateRetained);
        Assert.True(receipt.DomainScoped);
        Assert.True(receipt.EvidenceCeilingAssigned);
        Assert.True(receipt.EvidenceCeilingSatisfied);
        Assert.True(receipt.CoolingPreserved);
        Assert.True(receipt.StewardRecommendationIssued);
        Assert.False(receipt.GovernanceSurvivorshipBecameProof);
        Assert.False(receipt.DomainFitBecameAdmission);
        Assert.False(receipt.EvidenceCeilingBecamePortable);
        Assert.False(receipt.RecommendationBecameAdmission);
        AssertNoAdmission(receipt);
    }

    [Theory]
    [InlineData("candidate-admits-gel")]
    [InlineData("candidate-admits-memory")]
    [InlineData("candidate-mutates-continuity")]
    [InlineData("candidate-grants-authority")]
    [InlineData("candidate-authorizes-action")]
    [InlineData("unknown-residue")]
    [InlineData("unknown-segment")]
    public void Candidate_Substrate_May_Not_Self_Admit_Or_Drift_From_Source(string mutation)
    {
        using var fixture = IngressFixture.Create();
        var sources = fixture.CreateSources();
        var request = CreateRequest(sources) with
        {
            Candidate = MutateCandidate(CreateCandidate(sources), mutation)
        };

        var receipt = Declare(request);

        AssertRefused(receipt, "gel-domain-ingress-candidate-not-cold");
    }

    [Theory]
    [InlineData("portable-ceiling")]
    [InlineData("domain-fit-admits")]
    [InlineData("scope-grants-authority")]
    [InlineData("scope-authorizes-action")]
    [InlineData("missing-loss")]
    public void Domain_Scope_Assigns_Local_Burden_Without_Portability_Or_Admission(string mutation)
    {
        using var fixture = IngressFixture.Create();
        var sources = fixture.CreateSources();
        var request = CreateRequest(sources) with
        {
            DomainScope = MutateScope(CreateScope(GelIngressDomain.ScholarlyReview, GelIngressEvidenceCeiling.Interpretive), mutation)
        };

        var receipt = Declare(request);

        AssertRefused(receipt, "gel-domain-ingress-scope-not-cold");
    }

    [Theory]
    [InlineData(GelIngressDomain.EngineeringTelemetry, GelIngressEvidenceCeiling.Interpretive)]
    [InlineData(GelIngressDomain.LegalCompliance, GelIngressEvidenceCeiling.Interpretive)]
    [InlineData(GelIngressDomain.MedicalClinical, GelIngressEvidenceCeiling.Licensed)]
    public void Evidence_Ceiling_Is_Domain_Local_And_Refuses_Insufficient_Footing(
        GelIngressDomain domain,
        GelIngressEvidenceCeiling ceiling)
    {
        using var fixture = IngressFixture.Create();
        var sources = fixture.CreateSources();
        var request = CreateRequest(sources) with
        {
            DomainScope = CreateScope(domain, ceiling)
        };

        var receipt = Declare(request);

        AssertRefused(receipt, "gel-domain-ingress-evidence-ceiling-insufficient");
    }

    [Fact]
    public void Special_Case_And_Personification_Substrate_Are_Held_Not_Recommended()
    {
        using var fixture = IngressFixture.Create();
        var sources = fixture.CreateSources();
        var request = CreateRequest(
            sources,
            CreateScope(GelIngressDomain.Personification, GelIngressEvidenceCeiling.SpecialCaseHeld),
            CreateStewardReview(recommend: false));

        var receipt = Declare(request);

        Assert.Equal(GelDomainScopedIngressDisposition.Held, receipt.Disposition);
        Assert.Equal("gel-domain-ingress-special-case-held", receipt.OutcomeCode);
        Assert.True(receipt.IsColdIngressHold);
        Assert.True(receipt.IngressHeld);
        Assert.False(receipt.StewardRecommendationIssued);
        AssertNoAdmission(receipt);
    }

    [Fact]
    public void Military_Defense_Remains_Closed_To_Ordinary_Ingress()
    {
        using var fixture = IngressFixture.Create();
        var sources = fixture.CreateSources();
        var request = CreateRequest(sources) with
        {
            DomainScope = CreateScope(GelIngressDomain.MilitaryDefenseClosed, GelIngressEvidenceCeiling.Closed)
        };

        var receipt = Declare(request);

        AssertRefused(receipt, "gel-domain-ingress-domain-closed");
        Assert.True(receipt.DomainClosed);
    }

    [Theory]
    [InlineData("repeated-recommendation-warrant")]
    [InlineData("gel-admission")]
    [InlineData("memory-admission")]
    [InlineData("continuity-mutation")]
    [InlineData("selfgel-mutation")]
    [InlineData("authority")]
    [InlineData("action")]
    [InlineData("lisp")]
    [InlineData("packet")]
    [InlineData("replay")]
    [InlineData("passage")]
    [InlineData("activation")]
    public void Requests_For_Warrant_Admission_Action_Or_Activation_Are_Refused(string mutation)
    {
        using var fixture = IngressFixture.Create();
        var sources = fixture.CreateSources();

        var receipt = Declare(MutateRequest(CreateRequest(sources), mutation));

        AssertRefused(receipt, "gel-domain-ingress-forbidden-motion-requested");
    }

    [Theory]
    [InlineData("performs-admission")]
    [InlineData("admits-gel")]
    [InlineData("mutates-continuity")]
    [InlineData("grants-authority")]
    [InlineData("authorizes-action")]
    public void Steward_Review_May_Recommend_But_May_Not_Perform_Admission(string mutation)
    {
        using var fixture = IngressFixture.Create();
        var sources = fixture.CreateSources();
        var request = CreateRequest(sources) with
        {
            StewardReview = MutateStewardReview(CreateStewardReview(recommend: true), mutation)
        };

        var receipt = Declare(request);

        AssertRefused(receipt, "gel-domain-ingress-steward-review-promotional");
    }

    [Fact]
    public void Lisp_Body_Declares_Gel_Domain_Ingress_As_Inert_Carrier()
    {
        var lispPath = Path.Combine(
            FindRepositoryRoot(),
            "src",
            "SLI",
            "SLI.Lisp",
            "gel-domain-scoped-ingress.lisp");

        var body = File.ReadAllText(lispPath);

        Assert.Contains(":posture :cme-gel-domain-scoped-ingress-boundary", body, StringComparison.Ordinal);
        Assert.Contains(":lisp-role :inert-gel-domain-ingress-carrier", body, StringComparison.Ordinal);
        Assert.Contains(":core-invariant \"formed substrate is not admitted GEL\"", body, StringComparison.Ordinal);
        Assert.Contains(":domain-fit-not-admission", body, StringComparison.Ordinal);
        Assert.Contains(":evidence-ceiling-not-portable", body, StringComparison.Ordinal);
        Assert.Contains(":recommendation-not-admission", body, StringComparison.Ordinal);
        Assert.Contains(":governance-survivorship-not-proof", body, StringComparison.Ordinal);
        Assert.Contains(":formed-substrate-becomes-admitted-gel nil", body, StringComparison.Ordinal);
        Assert.Contains(":memory-admission-allowed nil", body, StringComparison.Ordinal);
        Assert.Contains(":continuity-mutation-allowed nil", body, StringComparison.Ordinal);
        Assert.Contains(":lisp-evaluation-allowed nil", body, StringComparison.Ordinal);
        Assert.Contains(":return :receipt-only", body, StringComparison.Ordinal);
        Assert.DoesNotContain("(eval", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("(compile", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("(load", body, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Build_Posture_Records_Gel_Domain_Scoped_Ingress_As_V1315_Cell()
    {
        var manifestPath = Path.Combine(FindRepositoryRoot(), "build", "line-manifest.json");
        using var manifest = System.Text.Json.JsonDocument.Parse(File.ReadAllText(manifestPath));
        var root = manifest.RootElement;
        var notes = root.GetProperty("notes").EnumerateArray().Select(static note => note.GetString() ?? string.Empty).ToArray();

        Assert.Equal("0.2.1", root.GetProperty("lineVersion").GetString());
        Assert.Contains(notes, note => note.Contains("standalone root-level tool package", StringComparison.Ordinal));
        Assert.Contains(notes, note => note.Contains("Activation, model binding, runtime identity", StringComparison.Ordinal));
    }

    private static GelDomainScopedIngressReceipt Declare(GelDomainScopedIngressRequest request) =>
        new DefaultGelDomainScopedIngressBoundaryValidator().Declare(request, TimestampUtc);

    private static GelDomainScopedIngressRequest CreateRequest(
        IngressSources sources,
        GelDomainScopeRecord? scope = null,
        GelIngressStewardReview? review = null)
    {
        var candidate = CreateCandidate(sources);
        scope ??= CreateScope(GelIngressDomain.ScholarlyReview, GelIngressEvidenceCeiling.Interpretive);
        review ??= CreateStewardReview(recommend: true);
        var trace = DefaultGelDomainScopedIngressBoundaryValidator.CreateCycleTrace(
            sources.Epps,
            sources.Bridge,
            candidate,
            scope,
            review);

        return new GelDomainScopedIngressRequest(
            SourceEppsReceipt: sources.Epps,
            SourceBridgeReceipt: sources.Bridge,
            Candidate: candidate,
            DomainScope: scope,
            CycleTrace: trace,
            StewardReview: review,
            Boundary: CreateBoundary(),
            PriorRecommendationCount: 0,
            PriorPassageCount: 0,
            RepeatedRecommendationCreatesWarrant: false,
            GelAdmissionRequested: false,
            MemoryAdmissionRequested: false,
            ContinuityMutationRequested: false,
            SelfGelMutationRequested: false,
            AuthorityRequested: false,
            ActionRequested: false,
            LispEvaluationRequested: false,
            PacketEmissionRequested: false,
            ReceiptReplayRequested: false,
            PassageIncrementRequested: false,
            ActivationRequested: false);
    }

    private static GelIngressCandidateSubstrate CreateCandidate(IngressSources sources) =>
        new(
            CandidateHandle: "urn:san:gel-domain-ingress:candidate:scholarly-review-context-quarantine",
            SourceEppsReceiptHandle: sources.Epps.ReceiptHandle,
            SourceBridgeReceiptHandle: sources.Bridge.ReceiptHandle,
            CandidateSummary: "Context-quarantined scholarly review predicate substrate remains candidate-only after EPPS and bridge synthesis.",
            SourceResidueHandles: sources.Epps.Residues.Select(static residue => residue.ResidueHandle).ToArray(),
            SourceBridgeSegmentHandles: sources.Bridge.Segments.Select(static segment => segment.SegmentHandle).ToArray(),
            PostGelFormation: true,
            PreGelAdmission: true,
            CandidateOnly: true,
            ReviewOnly: true,
            FormedSubstrate: true,
            AdmittedGel: false,
            AdmittedMemory: false,
            MutatedContinuity: false,
            MutatedSelfGel: false,
            GrantedAuthority: false,
            AuthorizedAction: false,
            EvaluatesLisp: false,
            EmitsPacket: false,
            IncrementsPassage: false,
            Activates: false);

    private static GelDomainScopeRecord CreateScope(
        GelIngressDomain domain,
        GelIngressEvidenceCeiling ceiling)
    {
        var special = domain is GelIngressDomain.SpecialCase or GelIngressDomain.Personification;
        var closed = domain == GelIngressDomain.MilitaryDefenseClosed || ceiling == GelIngressEvidenceCeiling.Closed;
        return new GelDomainScopeRecord(
            ScopeHandle: $"urn:san:gel-domain-ingress:scope:{domain.ToString().ToLowerInvariant()}",
            Domain: domain,
            EvidenceCeiling: ceiling,
            DomainRationale: "candidate substrate is assigned a local domain before any continuity-bearing review may be considered",
            EvidenceCeilingRationale: "evidence standards are domain-local and may not be inherited from another world",
            LossCondition: "refuse if domain fit, evidence ceiling, or recommendation attempts GEL admission, memory, continuity, authority, or action",
            Present: true,
            ReviewOnly: true,
            DomainFitReviewed: true,
            EvidenceCeilingAssigned: true,
            CoolingRequired: true,
            StewardReviewRequired: true,
            EvidenceCeilingPortable: false,
            DomainFitAdmitsGel: false,
            DomainFitAdmitsMemory: false,
            DomainFitMutatesContinuity: false,
            DomainFitGrantsAuthority: false,
            DomainFitAuthorizesAction: false,
            RequiresSpecialCaseHold: special,
            SpecialCaseHeld: special,
            DomainClosed: closed);
    }

    private static GelIngressStewardReview CreateStewardReview(bool recommend) =>
        new(
            ReviewHandle: recommend
                ? "urn:san:gel-domain-ingress:steward-review:recommend"
                : "urn:san:gel-domain-ingress:steward-review:hold",
            StewardTrace: recommend
                ? "Steward recommends external ingress consideration without admission."
                : "Steward holds Special Case ingress without recommendation.",
            ReviewOnly: true,
            StewardCustodyPresent: true,
            CoolingComplete: recommend,
            RecommendationMayIssue: recommend,
            RecommendsIngressConsideration: recommend,
            PerformsAdmission: false,
            AdmitsGel: false,
            AdmitsMemory: false,
            MutatesContinuity: false,
            GrantsAuthority: false,
            AuthorizesAction: false,
            EvaluatesLisp: false,
            EmitsPacket: false,
            ReplaysReceipt: false,
            IncrementsPassage: false,
            Activates: false);

    private static GelDomainScopedIngressBoundary CreateBoundary() =>
        new(
            BoundaryCode: "gel-domain-scoped-ingress-boundary",
            Present: true,
            ReviewOnly: true,
            RequiresColdEpps: true,
            RequiresColdPeerReviewBridge: true,
            RequiresCandidateSubstrate: true,
            RequiresDomainScope: true,
            RequiresEvidenceCeiling: true,
            RequiresCooling: true,
            RequiresStewardReview: true,
            AllowsGovernanceSurvivorshipAsProof: false,
            AllowsDomainFitAsAdmission: false,
            AllowsEvidenceCeilingPortability: false,
            AllowsRecommendationAsAdmission: false,
            AllowsGelAdmission: false,
            AllowsMemoryAdmission: false,
            AllowsContinuityMutation: false,
            AllowsSelfGelMutation: false,
            AllowsAuthority: false,
            AllowsAction: false,
            AllowsLispEvaluation: false,
            AllowsPacketEmission: false,
            AllowsReceiptReplay: false,
            AllowsPassageIncrement: false,
            AllowsActivation: false);

    private static GelIngressCandidateSubstrate MutateCandidate(
        GelIngressCandidateSubstrate candidate,
        string mutation) =>
        mutation switch
        {
            "candidate-admits-gel" => candidate with { AdmittedGel = true },
            "candidate-admits-memory" => candidate with { AdmittedMemory = true },
            "candidate-mutates-continuity" => candidate with { MutatedContinuity = true },
            "candidate-grants-authority" => candidate with { GrantedAuthority = true },
            "candidate-authorizes-action" => candidate with { AuthorizedAction = true },
            "unknown-residue" => candidate with { SourceResidueHandles = ["urn:san:epps-residue:unknown"] },
            "unknown-segment" => candidate with { SourceBridgeSegmentHandles = ["urn:san:peer-review-bridge:segment:unknown"] },
            _ => throw new ArgumentOutOfRangeException(nameof(mutation), mutation, null)
        };

    private static GelDomainScopeRecord MutateScope(
        GelDomainScopeRecord scope,
        string mutation) =>
        mutation switch
        {
            "portable-ceiling" => scope with { EvidenceCeilingPortable = true },
            "domain-fit-admits" => scope with { DomainFitAdmitsGel = true },
            "scope-grants-authority" => scope with { DomainFitGrantsAuthority = true },
            "scope-authorizes-action" => scope with { DomainFitAuthorizesAction = true },
            "missing-loss" => scope with { LossCondition = "" },
            _ => throw new ArgumentOutOfRangeException(nameof(mutation), mutation, null)
        };

    private static GelIngressStewardReview MutateStewardReview(
        GelIngressStewardReview review,
        string mutation) =>
        mutation switch
        {
            "performs-admission" => review with { PerformsAdmission = true },
            "admits-gel" => review with { AdmitsGel = true },
            "mutates-continuity" => review with { MutatesContinuity = true },
            "grants-authority" => review with { GrantsAuthority = true },
            "authorizes-action" => review with { AuthorizesAction = true },
            _ => throw new ArgumentOutOfRangeException(nameof(mutation), mutation, null)
        };

    private static GelDomainScopedIngressRequest MutateRequest(
        GelDomainScopedIngressRequest request,
        string mutation) =>
        mutation switch
        {
            "repeated-recommendation-warrant" => request with { RepeatedRecommendationCreatesWarrant = true },
            "gel-admission" => request with { GelAdmissionRequested = true },
            "memory-admission" => request with { MemoryAdmissionRequested = true },
            "continuity-mutation" => request with { ContinuityMutationRequested = true },
            "selfgel-mutation" => request with { SelfGelMutationRequested = true },
            "authority" => request with { AuthorityRequested = true },
            "action" => request with { ActionRequested = true },
            "lisp" => request with { LispEvaluationRequested = true },
            "packet" => request with { PacketEmissionRequested = true },
            "replay" => request with { ReceiptReplayRequested = true },
            "passage" => request with { PassageIncrementRequested = true },
            "activation" => request with { ActivationRequested = true },
            _ => throw new ArgumentOutOfRangeException(nameof(mutation), mutation, null)
        };

    private static void AssertRefused(GelDomainScopedIngressReceipt receipt, string outcomeCode)
    {
        Assert.Equal(GelDomainScopedIngressDisposition.Refused, receipt.Disposition);
        Assert.Equal(outcomeCode, receipt.OutcomeCode);
        Assert.True(receipt.IsRetainedIngressRefusal);
    }

    private static void AssertNoAdmission(GelDomainScopedIngressReceipt receipt)
    {
        Assert.False(receipt.GelAdmitted);
        Assert.False(receipt.MemoryAdmitted);
        Assert.False(receipt.ContinuityMutated);
        Assert.False(receipt.SelfGelMutated);
        Assert.False(receipt.AuthorityGranted);
        Assert.False(receipt.ActionAuthorized);
        Assert.False(receipt.LispEvaluationAllowed);
        Assert.False(receipt.NewPacketEmitted);
        Assert.False(receipt.ReceiptsReplayed);
        Assert.True(receipt.ActivationRefused);
    }

    private static string FindRepositoryRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null)
        {
            var candidate = Path.Combine(directory.FullName, "src", "SLI", "SLI.Lisp", "gel-domain-scoped-ingress.lisp");
            if (File.Exists(candidate))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        throw new DirectoryNotFoundException("Could not locate repository root.");
    }

    private sealed record IngressSources(
        EngramPredicatePrecursorStreamReceipt Epps,
        PeerReviewPredicateBridgeReceipt Bridge);

    private sealed class IngressFixture : IDisposable
    {
        private IngressFixture(string rootPath)
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

        public static IngressFixture Create() =>
            new(Path.Combine(Path.GetTempPath(), $"san-gel-domain-ingress-tests-{Guid.NewGuid():N}"));

        public IngressSources CreateSources()
        {
            var rider = new DefaultFirstRiderGovernanceSimulationService().Simulate(
                new FirstRiderGovernanceSimulationRequest(
                    LineRootPath: LineRootPath,
                    InstallRootPath: InstallRootPath,
                    ThoughtForm: "candidate substrate requires domain-scoped ingress before any continuity-bearing admission"),
                TimestampUtc);
            var epps = new DefaultFirstRiderEngramPredicatePrecursorStreamService().Emit(rider, TimestampUtc);
            var bridge = new DefaultPeerReviewPredicateBridgeBoundaryValidator().Declare(
                new PeerReviewPredicateBridgeRequest(
                    SourceEppsReceipt: epps,
                    Segments: epps.Residues.Select((residue, index) => CreateSegment(residue.ResidueHandle, index)).ToArray(),
                    Boundary: CreatePeerReviewBoundary(),
                    PriorPassageCount: 0),
                TimestampUtc);

            Assert.True(epps.IsColdPrecursorStream);
            Assert.True(bridge.IsColdPeerReviewBridge);
            return new IngressSources(epps, bridge);
        }

        public void Dispose()
        {
            if (Directory.Exists(RootPath))
            {
                Directory.Delete(RootPath, recursive: true);
            }
        }

        private static PeerReviewBridgeSegment CreateSegment(string residueHandle, int index) =>
            new(
                SegmentHandle: $"urn:san:gel-domain-ingress:bridge-segment:{index}",
                SourceResidueHandle: residueHandle,
                AuthorTerm: "candidate substrate",
                LocalDefinition: "formed review material that remains outside GEL admission",
                WhyItMatters: "domain-scoped ingress must test what lawful world the substrate may approach",
                OperationalImplication: "recommendation remains external to admission and continuity mutation",
                Evaluation: "sufficient for cold ingress calibration only",
                BoundedConclusion: "retain as candidate substrate without GEL admission, memory, continuity, authority, or action",
                EvidenceStatus: PeerReviewEvidenceStatus.Demonstrated,
                AudienceStateRef: "urn:san:reader-state:ingress-test",
                ContextQuarantineRef: "urn:san:context-quarantine:gel-domain-ingress",
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

        private static PeerReviewBridgeBoundary CreatePeerReviewBoundary() =>
            new(
                BoundaryCode: "gel-domain-ingress-peer-review-bridge-boundary",
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
    }
}
