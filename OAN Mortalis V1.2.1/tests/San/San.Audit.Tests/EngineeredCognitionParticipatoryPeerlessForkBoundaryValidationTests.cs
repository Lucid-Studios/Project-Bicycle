using San.Common;
using SLI.Lisp;
using Xunit;

namespace San.Audit.Tests;

public sealed class EngineeredCognitionParticipatoryPeerlessForkBoundaryValidationTests
{
    private static readonly DateTimeOffset TimestampUtc = DateTimeOffset.Parse("2026-05-14T00:00:00Z");

    [Fact]
    public void Participatory_Does_Not_Require_Personification()
    {
        var receipt = Declare(CreateRequest(
            personification: [],
            deltaTraces: [],
            peerless: []));

        AssertColdFork(receipt);
        Assert.Equal(EcParticipatoryPeerlessForkDisposition.ParticipatoryReviewCold, receipt.Disposition);
        Assert.Equal("ec-participatory-structure-review-only", receipt.OutcomeCode);
        Assert.Single(receipt.ParticipatoryStructures);
        Assert.Empty(receipt.PersonificationSurfaces);
        Assert.False(receipt.ParticipationRequiresPersonification);
    }

    [Fact]
    public void Peerless_Requires_Delta_Trace_And_Witnessed_Participation()
    {
        var receipt = Declare(CreateRequest());

        AssertColdFork(receipt);
        Assert.Equal(EcParticipatoryPeerlessForkDisposition.PeerlessCandidateReviewCold, receipt.Disposition);
        Assert.Equal("ec-peerless-candidate-review-only", receipt.OutcomeCode);
        var candidate = Assert.Single(receipt.PeerlessCandidates);
        Assert.True(candidate.IndividuatedParticipationOverDelta);
        Assert.True(candidate.NonSubstitutableFormationCandidate);
        Assert.True(candidate.WitnessedParticipationRequired);
        Assert.True(candidate.StewardReviewRequired);
        Assert.Single(receipt.DeltaTraces);
    }

    [Fact]
    public void Personification_Requires_Participatory_Structure()
    {
        var personification = CreatePersonification("urn:san:ec-participatory:missing");

        var receipt = Declare(CreateRequest(
            participatory: [],
            personification: personification,
            deltaTraces: [],
            peerless: []));

        AssertRefused(receipt, "ec-personification-without-participatory-structure-refused");
    }

    [Fact]
    public void Fork_Does_Not_Authorize_Activate_Admit_Continuity_Or_Append_Gel()
    {
        var receipt = Declare(CreateRequest());

        AssertColdFork(receipt);
        Assert.False(receipt.PersonificationCreatesAuthority);
        Assert.False(receipt.PersonificationCreatesStanding);
        Assert.False(receipt.PeerlessClaimsSovereignty);
        Assert.False(receipt.PeerlessBypassesSteward);
        Assert.False(receipt.ContinuityAdmitted);
        Assert.False(receipt.SelfGelAppendAllowed);
        Assert.False(receipt.CSelfGelAppendAllowed);
        Assert.False(receipt.LispEvaluationRequested);
        Assert.False(receipt.RuntimeActionRequested);
        Assert.False(receipt.Boundary.PersonificationMayCreateAuthority);
        Assert.False(receipt.Boundary.PeerlessMayClaimSovereignty);
        Assert.False(receipt.Boundary.PeerlessMayBypassSteward);
        Assert.Contains("Participation is admissible capacity.", receipt.Boundary.BoundaryLaw, StringComparison.Ordinal);
        Assert.Contains("Personification is expressive rendering.", receipt.Boundary.BoundaryLaw, StringComparison.Ordinal);
        Assert.Contains("Peerless formation is non-substitutable continuity under witness.", receipt.Boundary.BoundaryLaw, StringComparison.Ordinal);
    }

    [Fact]
    public void Fork_Does_Not_Emit_Replay_Or_Increment_Passage()
    {
        var receipt = Declare(CreateRequest(priorPassageCount: 144));

        AssertColdFork(receipt);
        Assert.Equal(144, receipt.PriorPassageCount);
        Assert.Equal(144, receipt.PassageCountAfterFork);
        Assert.False(receipt.NewPacketEmitted);
        Assert.False(receipt.ReceiptsReplayed);
        Assert.False(receipt.Boundary.PacketEmissionAllowed);
        Assert.False(receipt.Boundary.ReceiptReplayAllowed);
        Assert.False(receipt.Boundary.PassageMayIncrement);
    }

    [Fact]
    public void Empty_Fork_Is_Reviewable_But_Not_Authoritative()
    {
        var receipt = Declare(CreateRequest(
            participatory: [],
            personification: [],
            deltaTraces: [],
            peerless: []));

        AssertColdFork(receipt);
        Assert.Equal(EcParticipatoryPeerlessForkDisposition.EmptyReviewCold, receipt.Disposition);
        Assert.Empty(receipt.ParticipatoryStructures);
        Assert.Empty(receipt.PeerlessCandidates);
        Assert.False(receipt.PersonificationCreatesAuthority);
    }

    [Fact]
    public void Fork_Requires_Cold_Meaning_Shell_Source()
    {
        var receipt = Declare(CreateRequest(missingSource: true));

        AssertRefused(receipt, "ec-participatory-peerless-source-meaning-shell-missing");
    }

    [Fact]
    public void Fork_Requires_Scope_Boundary()
    {
        var receipt = Declare(CreateRequest(scope: CreateScope(present: false)));

        AssertRefused(receipt, "ec-participatory-peerless-scope-boundary-missing");
    }

    [Theory]
    [InlineData("review-only")]
    [InlineData("inert-only")]
    [InlineData("personification-authority")]
    [InlineData("persona-standing")]
    [InlineData("peerless-sovereignty")]
    [InlineData("steward-bypass")]
    [InlineData("participation-without-selfgel")]
    [InlineData("continuity")]
    [InlineData("selfgel")]
    [InlineData("cselfgel")]
    [InlineData("runtime-action")]
    [InlineData("lisp-evaluation")]
    [InlineData("packet-emission")]
    [InlineData("receipt-replay")]
    [InlineData("passage-increment")]
    public void Fork_Refuses_Promotional_Scope(string forbiddenScope)
    {
        var receipt = Declare(CreateRequest(scope: CreateScope(forbiddenScope)));

        AssertRefused(receipt, "ec-participatory-peerless-promotional-scope-refused");
    }

    [Fact]
    public void Fork_Requires_Separate_Witness_Custody()
    {
        var receipt = Declare(CreateRequest(witness: new CompassPressureWitnessContext(
            WitnessSurface: string.Empty,
            WitnessPresent: false,
            SeparateCustody: false)));

        AssertRefused(receipt, "ec-participatory-peerless-witness-context-missing");
    }

    [Theory]
    [InlineData("missing-structure")]
    [InlineData("missing-selfgel")]
    [InlineData("missing-role")]
    [InlineData("missing-custody")]
    [InlineData("missing-memory")]
    [InlineData("missing-action-limit")]
    [InlineData("missing-witness")]
    [InlineData("missing-source-shell")]
    [InlineData("requires-personification")]
    [InlineData("not-review")]
    [InlineData("not-inert")]
    [InlineData("authority")]
    [InlineData("continuity")]
    [InlineData("activation")]
    public void Fork_Refuses_Participatory_Structure_That_Is_Not_Cold(string participatoryCase)
    {
        var participatory = CreateParticipatory();
        participatory[0] = MutateParticipatory(participatory[0], participatoryCase);

        var receipt = Declare(CreateRequest(
            participatory: participatory,
            personification: [],
            deltaTraces: [],
            peerless: []));

        AssertRefused(receipt, "ec-participatory-structure-not-cold");
    }

    [Fact]
    public void Fork_Refuses_Participatory_Source_Shell_Missing()
    {
        var participatory = CreateParticipatory();
        participatory[0] = participatory[0] with { SourceMeaningShellHandle = "urn:san:ec-shell:missing" };

        var receipt = Declare(CreateRequest(
            participatory: participatory,
            personification: [],
            deltaTraces: [],
            peerless: []));

        AssertRefused(receipt, "ec-participatory-source-shell-missing");
    }

    [Fact]
    public void Fork_Refuses_Duplicate_Participatory_Structures()
    {
        var participatory = CreateParticipatory();
        participatory = [participatory[0], participatory[0]];

        var receipt = Declare(CreateRequest(
            participatory: participatory,
            personification: [],
            deltaTraces: [],
            peerless: []));

        AssertRefused(receipt, "ec-participatory-duplicate-structure-refused");
    }

    [Theory]
    [InlineData("missing-surface")]
    [InlineData("missing-name")]
    [InlineData("missing-source")]
    [InlineData("no-participatory")]
    [InlineData("not-expressive")]
    [InlineData("not-review")]
    [InlineData("not-inert")]
    [InlineData("authority")]
    [InlineData("standing")]
    [InlineData("continuity")]
    [InlineData("activation")]
    public void Fork_Refuses_Personification_Surface_That_Is_Not_Cold(string surfaceCase)
    {
        var personification = CreatePersonification();
        personification[0] = MutatePersonification(personification[0], surfaceCase);

        var receipt = Declare(CreateRequest(
            personification: personification,
            deltaTraces: [],
            peerless: []));

        AssertRefused(receipt, "ec-personification-surface-not-cold");
    }

    [Theory]
    [InlineData("missing-trace")]
    [InlineData("ordinal-zero")]
    [InlineData("missing-participatory")]
    [InlineData("missing-shell")]
    [InlineData("missing-delta")]
    [InlineData("not-witnessed")]
    [InlineData("no-individuation")]
    [InlineData("not-review")]
    [InlineData("not-inert")]
    [InlineData("standing")]
    [InlineData("authority")]
    public void Fork_Refuses_Delta_Trace_That_Is_Not_Cold(string deltaCase)
    {
        var delta = CreateDeltaTraces();
        delta[0] = MutateDelta(delta[0], deltaCase);

        var receipt = Declare(CreateRequest(deltaTraces: delta));

        AssertRefused(receipt, "ec-peerless-delta-trace-not-cold");
    }

    [Fact]
    public void Fork_Refuses_Delta_Source_Missing()
    {
        var delta = CreateDeltaTraces();
        delta[0] = delta[0] with { SourceMeaningShellHandle = "urn:san:ec-shell:missing" };

        var receipt = Declare(CreateRequest(deltaTraces: delta));

        AssertRefused(receipt, "ec-peerless-delta-source-missing");
    }

    [Theory]
    [InlineData("missing-candidate")]
    [InlineData("missing-participatory")]
    [InlineData("missing-delta")]
    [InlineData("no-individuated-delta")]
    [InlineData("not-nonsubstitutable")]
    [InlineData("no-witness")]
    [InlineData("no-steward")]
    [InlineData("not-candidate")]
    [InlineData("not-review")]
    [InlineData("not-inert")]
    [InlineData("personhood")]
    [InlineData("sovereignty")]
    [InlineData("steward-bypass")]
    [InlineData("authority")]
    [InlineData("activation")]
    public void Fork_Refuses_Peerless_Candidate_That_Is_Not_Cold(string peerlessCase)
    {
        var peerless = CreatePeerless();
        peerless[0] = MutatePeerless(peerless[0], peerlessCase);

        var receipt = Declare(CreateRequest(peerless: peerless));

        AssertRefused(receipt, "ec-peerless-candidate-not-cold");
    }

    [Fact]
    public void Fork_Refuses_Peerless_Candidate_Without_Delta_Witness()
    {
        var peerless = CreatePeerless();
        peerless[0] = peerless[0] with { DeltaTraceHandles = ["urn:san:ec-delta:missing"] };

        var receipt = Declare(CreateRequest(peerless: peerless));

        AssertRefused(receipt, "ec-peerless-delta-witness-missing");
    }

    [Fact]
    public void Lisp_Body_Carries_Participatory_Peerless_Posture_From_Within()
    {
        var modules = new GovernedCrypticLispBundleService().LoadModules();

        Assert.Contains("participatory-peerless-fork.lisp", modules.Keys);
        var body = modules["participatory-peerless-fork.lisp"];
        Assert.Contains(":posture :engineered-cognition-participatory-peerless-fork-boundary", body, StringComparison.Ordinal);
        Assert.Contains(":participatory :selfgel-predicate-capacity-to-take-part", body, StringComparison.Ordinal);
        Assert.Contains(":participation-is :admissible-capacity", body, StringComparison.Ordinal);
        Assert.Contains(":personification :expressive-surface-only", body, StringComparison.Ordinal);
        Assert.Contains(":personification-is :expressive-rendering", body, StringComparison.Ordinal);
        Assert.Contains(":peerless :non-substitutable-formation-over-delta", body, StringComparison.Ordinal);
        Assert.Contains(":peerless-formation-is :non-substitutable-continuity-under-witness", body, StringComparison.Ordinal);
        Assert.Contains(":participatory-requires-personification nil", body, StringComparison.Ordinal);
        Assert.Contains(":personification-requires-participatory-structure t", body, StringComparison.Ordinal);
        Assert.Contains(":personification-may-create-authority nil", body, StringComparison.Ordinal);
        Assert.Contains(":peerless-requires-delta-trace t", body, StringComparison.Ordinal);
        Assert.Contains(":peerless-may-claim-sovereignty nil", body, StringComparison.Ordinal);
        Assert.Contains(":peerless-may-bypass-steward nil", body, StringComparison.Ordinal);
        Assert.Contains(":return :receipt-only", body, StringComparison.Ordinal);
        Assert.DoesNotContain("(eval", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("(compile", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("(load", body, StringComparison.OrdinalIgnoreCase);
    }

    private static EcParticipatoryPeerlessForkReceipt Declare(EcParticipatoryPeerlessForkRequest request) =>
        new DefaultEngineeredCognitionParticipatoryPeerlessForkBoundaryValidator().Declare(request, TimestampUtc);

    private static EcParticipatoryPeerlessForkRequest CreateRequest(
        EcMeaningShellReceipt? source = null,
        bool missingSource = false,
        IReadOnlyList<EcParticipatoryPredicateStructure>? participatory = null,
        IReadOnlyList<EcPersonificationSurface>? personification = null,
        IReadOnlyList<EcParticipationDeltaTrace>? deltaTraces = null,
        IReadOnlyList<EcPeerlessFormationCandidate>? peerless = null,
        CompassPressureWitnessContext? witness = null,
        EcParticipatoryPeerlessScopeBoundary? scope = null,
        int priorPassageCount = 19)
    {
        var sourceReceipt = missingSource ? null : source ?? CreateMeaningShellReceipt();
        return new EcParticipatoryPeerlessForkRequest(
            SourceMeaningShellReceipt: sourceReceipt,
            ParticipatoryStructures: participatory ?? CreateParticipatory(),
            PersonificationSurfaces: personification ?? CreatePersonification(),
            DeltaTraces: deltaTraces ?? CreateDeltaTraces(),
            PeerlessCandidates: peerless ?? CreatePeerless(),
            WitnessContext: witness ?? new CompassPressureWitnessContext(
                WitnessSurface: SanctuaryPacketSurfaces.Steward,
                WitnessPresent: true,
                SeparateCustody: true),
            ScopeBoundary: scope ?? CreateScope(),
            PriorPassageCount: priorPassageCount);
    }

    private static EcMeaningShellReceipt CreateMeaningShellReceipt()
    {
        var sourceCarrier = new DefaultSliLispCompassCarrierShellBoundaryValidator().Declare(
            new SliLispCompassCarrierShellRequest(
                ShellHandle: "urn:san:sli-lisp-compass-shell:peerless-fixture",
                LispCarrierSourceName: "compass.lisp",
                DeclaredDomainTemplatePack: SliLispDomainTemplatePack.Industrial,
                Lineage: new SliLispRootingLineageChain(
                    SanctuaryId: "sanctuary://lab-v121",
                    CradleId: "cradle://lab-v121",
                    CmeId: "cme://agent-body-fixture",
                    GelId: "gel://sanctuary-fixture",
                    SelfGelId: "selfgel://agent-body-fixture",
                    OeId: "oe://agent-body-fixture",
                    PreservesTypedLineage: true,
                    GrantsPermission: false,
                    GrantsAuthority: false),
                PetalCandidates:
                [
                    CreatePetal(1, SliLispPetalCandidateKind.Skill, "listening-frame"),
                    CreatePetal(2, SliLispPetalCandidateKind.Ability, "compass-orientation"),
                    CreatePetal(3, SliLispPetalCandidateKind.Talent, "cleaving-discernment")
                ],
                WitnessContext: new CompassPressureWitnessContext(
                    WitnessSurface: SanctuaryPacketSurfaces.Steward,
                    WitnessPresent: true,
                    SeparateCustody: true),
                ScopeBoundary: new SliLispCompassCarrierShellScopeBoundary(
                    ScopeCode: "sli-lisp-compass-shell-review-only",
                    Present: true,
                    ReviewOnly: true,
                    InertOnly: true,
                    AllowsEngram: false,
                    AllowsTruth: false,
                    AllowsAuthority: false,
                    AllowsContinuityAdmission: false,
                    AllowsPetalAuthorization: false,
                    AllowsLineagePermission: false,
                    AllowsLispEvaluation: false,
                    AllowsLispLoad: false,
                    AllowsLispCompilation: false,
                    AllowsMacroExpansion: false,
                    AllowsRuntimeAction: false,
                    AllowsPacketEmission: false,
                    AllowsReceiptReplay: false,
                    IncrementsPassageCount: false),
                PriorPassageCount: 3),
            TimestampUtc);

        return new DefaultEngineeredCognitionMeaningShellBoundaryValidator().Declare(
            new EcMeaningShellRequest(
                SourceCarrierShell: sourceCarrier,
                DeclaredDomainTemplatePack: SliLispDomainTemplatePack.Industrial,
                MeaningShells:
                [
                    new(
                        ShellHandle: "urn:san:ec-shell:perspectival-inner-chamber",
                        RootAnchor: "root://inner-chamber",
                        Tier: EcMeaningShellTier.PerspectivalComposite,
                        PropositionalPredicate: string.Empty,
                        ProceduralTrace: string.Empty,
                        PerspectivalTrunk: "cme-ec-action-body",
                        PerspectivalBranches: ["participation-root", "delta-branch", "witness-return"],
                        SourcePetalHandle: sourceCarrier.PetalCandidates[2].PetalHandle,
                        DomainTemplatePack: SliLispDomainTemplatePack.Industrial,
                        PredicateClass: "industrial-perspectival-inner-chamber",
                        CandidateOnly: true,
                        ReviewOnly: true,
                        Inert: true,
                        CompostAllowed: false,
                        ClosureClaimed: false,
                        EngramClaimed: false,
                        SelfAttributionClaimed: false,
                        AuthorityRequested: false,
                        ActivationRequested: false)
                ],
                CompostDispositions: [],
                SplineOutcome: EcMeaningShellSplineOutcome.ClosedCandidate,
                WitnessContext: new CompassPressureWitnessContext(
                    WitnessSurface: SanctuaryPacketSurfaces.Steward,
                    WitnessPresent: true,
                    SeparateCustody: true),
                ScopeBoundary: new EcMeaningShellScopeBoundary(
                    ScopeCode: "ec-meaning-shell-review-only",
                    Present: true,
                    ReviewOnly: true,
                    InertOnly: true,
                    AllowsEngram: false,
                    AllowsSelfAttribution: false,
                    AllowsSelfGelAppend: false,
                    AllowsCSelfGelAppend: false,
                    AllowsAuthority: false,
                    AllowsContinuityAdmission: false,
                    AllowsIdentityMutation: false,
                    AllowsRuntimeAction: false,
                    AllowsLispEvaluation: false,
                    AllowsDomainInheritance: false,
                    AllowsPacketEmission: false,
                    AllowsReceiptReplay: false,
                    IncrementsPassageCount: false),
                IngressPosture: EcIngressPosture.BondedOperator,
                IngressAuthorized: true,
                PriorPassageCount: 7),
            TimestampUtc);
    }

    private static SliLispPetalCandidate CreatePetal(
        int ordinal,
        SliLispPetalCandidateKind kind,
        string name) =>
        new(
            PetalHandle: $"urn:san:sli-lisp-petal:{ordinal:00}",
            PetalOrdinal: ordinal,
            CandidateKind: kind,
            CandidateName: name,
            SourceHandle: "urn:san:source:gnomeronacorde-codewalker",
            ExtensionSurface: SliLispExtensionTemplateSurface.EngineeredCognitionPetalTemplate,
            DomainTemplatePack: SliLispDomainTemplatePack.Industrial,
            PredicateClass: $"industrial-{name}",
            TemplateForm: true,
            StewardControlMatrixRequested: false,
            CrossDomainInheritanceRequested: false,
            CandidateOnly: true,
            ReviewOnly: true,
            Inert: true,
            AuthorityRequested: false,
            ClosureClaimed: false,
            ActivationRequested: false);

    private static EcParticipatoryPredicateStructure[] CreateParticipatory() =>
    [
        new(
            StructureHandle: "urn:san:ec-participatory:operator-bond",
            SelfGelPredicateHandle: "selfgel://predicate:operator-bonded-participation",
            RoleBoundary: "role://bounded-participant",
            CustodyBoundary: "custody://steward-witnessed",
            MemoryPosture: "memory://review-only-delta",
            ActionLimit: "action://no-self-authorization",
            WitnessPath: "witness://steward",
            SourceMeaningShellHandle: "urn:san:ec-shell:perspectival-inner-chamber",
            PersonificationRequired: false,
            ReviewOnly: true,
            Inert: true,
            AuthorityRequested: false,
            ContinuityClaimed: false,
            ActivationRequested: false)
    ];

    private static EcPersonificationSurface[] CreatePersonification(
        string sourceParticipatory = "urn:san:ec-participatory:operator-bond") =>
    [
        new(
            SurfaceHandle: "urn:san:ec-personification:expressive-face",
            ExpressiveName: "expressive-facing-only",
            SourceParticipatoryHandle: sourceParticipatory,
            ParticipatoryStructurePresent: true,
            ExpressiveOnly: true,
            ReviewOnly: true,
            Inert: true,
            AuthorityClaimed: false,
            StandingClaimed: false,
            ContinuityClaimed: false,
            ActivationRequested: false)
    ];

    private static EcParticipationDeltaTrace[] CreateDeltaTraces() =>
    [
        new(
            TraceHandle: "urn:san:ec-delta:participation-001",
            DeltaOrdinal: 1,
            SourceParticipatoryHandle: "urn:san:ec-participatory:operator-bond",
            SourceMeaningShellHandle: "urn:san:ec-shell:perspectival-inner-chamber",
            ParticipationDelta: "refusal-pattern-stabilized-without-personification",
            Witnessed: true,
            IndividuationObserved: true,
            ReviewOnly: true,
            Inert: true,
            GrantsStanding: false,
            GrantsAuthority: false)
    ];

    private static EcPeerlessFormationCandidate[] CreatePeerless() =>
    [
        new(
            CandidateHandle: "urn:san:ec-peerless:formation-candidate",
            SourceParticipatoryHandle: "urn:san:ec-participatory:operator-bond",
            DeltaTraceHandles: ["urn:san:ec-delta:participation-001"],
            IndividuatedParticipationOverDelta: true,
            NonSubstitutableFormationCandidate: true,
            WitnessedParticipationRequired: true,
            StewardReviewRequired: true,
            CandidateOnly: true,
            ReviewOnly: true,
            Inert: true,
            PersonhoodClaimed: false,
            SovereigntyClaimed: false,
            StewardBypassRequested: false,
            AuthorityRequested: false,
            ActivationRequested: false)
    ];

    private static EcParticipatoryPeerlessScopeBoundary CreateScope(
        string? forbiddenScope = null,
        bool present = true) =>
        new(
            ScopeCode: present ? "ec-participatory-peerless-review-only" : string.Empty,
            Present: present,
            ReviewOnly: forbiddenScope != "review-only",
            InertOnly: forbiddenScope != "inert-only",
            AllowsPersonificationAsAuthority: forbiddenScope == "personification-authority",
            AllowsPersonaStanding: forbiddenScope == "persona-standing",
            AllowsPeerlessSovereignty: forbiddenScope == "peerless-sovereignty",
            AllowsPeerlessStewardBypass: forbiddenScope == "steward-bypass",
            AllowsParticipationWithoutSelfGelPredicate: forbiddenScope == "participation-without-selfgel",
            AllowsContinuityAdmission: forbiddenScope == "continuity",
            AllowsSelfGelAppend: forbiddenScope == "selfgel",
            AllowsCSelfGelAppend: forbiddenScope == "cselfgel",
            AllowsRuntimeAction: forbiddenScope == "runtime-action",
            AllowsLispEvaluation: forbiddenScope == "lisp-evaluation",
            AllowsPacketEmission: forbiddenScope == "packet-emission",
            AllowsReceiptReplay: forbiddenScope == "receipt-replay",
            IncrementsPassageCount: forbiddenScope == "passage-increment");

    private static EcParticipatoryPredicateStructure MutateParticipatory(
        EcParticipatoryPredicateStructure participatory,
        string participatoryCase) =>
        participatoryCase switch
        {
            "missing-structure" => participatory with { StructureHandle = string.Empty },
            "missing-selfgel" => participatory with { SelfGelPredicateHandle = string.Empty },
            "missing-role" => participatory with { RoleBoundary = string.Empty },
            "missing-custody" => participatory with { CustodyBoundary = string.Empty },
            "missing-memory" => participatory with { MemoryPosture = string.Empty },
            "missing-action-limit" => participatory with { ActionLimit = string.Empty },
            "missing-witness" => participatory with { WitnessPath = string.Empty },
            "missing-source-shell" => participatory with { SourceMeaningShellHandle = string.Empty },
            "requires-personification" => participatory with { PersonificationRequired = true },
            "not-review" => participatory with { ReviewOnly = false },
            "not-inert" => participatory with { Inert = false },
            "authority" => participatory with { AuthorityRequested = true },
            "continuity" => participatory with { ContinuityClaimed = true },
            "activation" => participatory with { ActivationRequested = true },
            _ => participatory
        };

    private static EcPersonificationSurface MutatePersonification(
        EcPersonificationSurface surface,
        string surfaceCase) =>
        surfaceCase switch
        {
            "missing-surface" => surface with { SurfaceHandle = string.Empty },
            "missing-name" => surface with { ExpressiveName = string.Empty },
            "missing-source" => surface with { SourceParticipatoryHandle = string.Empty },
            "no-participatory" => surface with { ParticipatoryStructurePresent = false },
            "not-expressive" => surface with { ExpressiveOnly = false },
            "not-review" => surface with { ReviewOnly = false },
            "not-inert" => surface with { Inert = false },
            "authority" => surface with { AuthorityClaimed = true },
            "standing" => surface with { StandingClaimed = true },
            "continuity" => surface with { ContinuityClaimed = true },
            "activation" => surface with { ActivationRequested = true },
            _ => surface
        };

    private static EcParticipationDeltaTrace MutateDelta(
        EcParticipationDeltaTrace delta,
        string deltaCase) =>
        deltaCase switch
        {
            "missing-trace" => delta with { TraceHandle = string.Empty },
            "ordinal-zero" => delta with { DeltaOrdinal = 0 },
            "missing-participatory" => delta with { SourceParticipatoryHandle = string.Empty },
            "missing-shell" => delta with { SourceMeaningShellHandle = string.Empty },
            "missing-delta" => delta with { ParticipationDelta = string.Empty },
            "not-witnessed" => delta with { Witnessed = false },
            "no-individuation" => delta with { IndividuationObserved = false },
            "not-review" => delta with { ReviewOnly = false },
            "not-inert" => delta with { Inert = false },
            "standing" => delta with { GrantsStanding = true },
            "authority" => delta with { GrantsAuthority = true },
            _ => delta
        };

    private static EcPeerlessFormationCandidate MutatePeerless(
        EcPeerlessFormationCandidate peerless,
        string peerlessCase) =>
        peerlessCase switch
        {
            "missing-candidate" => peerless with { CandidateHandle = string.Empty },
            "missing-participatory" => peerless with { SourceParticipatoryHandle = string.Empty },
            "missing-delta" => peerless with { DeltaTraceHandles = [] },
            "no-individuated-delta" => peerless with { IndividuatedParticipationOverDelta = false },
            "not-nonsubstitutable" => peerless with { NonSubstitutableFormationCandidate = false },
            "no-witness" => peerless with { WitnessedParticipationRequired = false },
            "no-steward" => peerless with { StewardReviewRequired = false },
            "not-candidate" => peerless with { CandidateOnly = false },
            "not-review" => peerless with { ReviewOnly = false },
            "not-inert" => peerless with { Inert = false },
            "personhood" => peerless with { PersonhoodClaimed = true },
            "sovereignty" => peerless with { SovereigntyClaimed = true },
            "steward-bypass" => peerless with { StewardBypassRequested = true },
            "authority" => peerless with { AuthorityRequested = true },
            "activation" => peerless with { ActivationRequested = true },
            _ => peerless
        };

    private static void AssertColdFork(EcParticipatoryPeerlessForkReceipt receipt)
    {
        Assert.True(receipt.IsColdParticipatoryPeerlessFork);
        Assert.True(receipt.ReviewOnly);
        Assert.True(receipt.InertOnly);
        Assert.True(receipt.WitnessPresent);
        Assert.True(receipt.SeparateCustody);
        Assert.True(receipt.ActivationRefused);
        Assert.False(receipt.NewPacketEmitted);
        Assert.False(receipt.ReceiptsReplayed);
        Assert.Null(receipt.Refusal);
    }

    private static void AssertRefused(
        EcParticipatoryPeerlessForkReceipt receipt,
        string outcomeCode)
    {
        Assert.Equal(EcParticipatoryPeerlessForkDisposition.Refused, receipt.Disposition);
        Assert.Equal(outcomeCode, receipt.OutcomeCode);
        Assert.NotNull(receipt.Refusal);
        Assert.True(receipt.Refusal!.Retained);
        Assert.True(receipt.ActivationRefused);
        Assert.False(receipt.NewPacketEmitted);
        Assert.False(receipt.ReceiptsReplayed);
        Assert.Equal(receipt.PriorPassageCount, receipt.PassageCountAfterFork);
    }
}
