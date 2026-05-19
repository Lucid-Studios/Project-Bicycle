using San.Common;
using SLI.Lisp;
using Xunit;

namespace San.Audit.Tests;

public sealed class EngineeredCognitionMeaningShellBoundaryValidationTests
{
    private static readonly DateTimeOffset TimestampUtc = DateTimeOffset.Parse("2026-05-14T00:00:00Z");

    [Fact]
    public void Declare_Accepts_Cold_Engineered_Cognition_Meaning_Shells()
    {
        var receipt = Declare(CreateRequest());

        AssertColdMeaningShell(receipt);
        Assert.Equal(EcMeaningShellDisposition.FormedForReviewCold, receipt.Disposition);
        Assert.Equal("ec-meaning-shell-formed-review-only", receipt.OutcomeCode);
        Assert.Equal(4, receipt.MeaningShells.Count);
        Assert.Contains(receipt.MeaningShells, shell => shell.Tier == EcMeaningShellTier.Root);
        Assert.Contains(receipt.MeaningShells, shell => shell.Tier == EcMeaningShellTier.PropositionalTier1);
        Assert.Contains(receipt.MeaningShells, shell => shell.Tier == EcMeaningShellTier.ProceduralTier2Plus);
        Assert.Contains(receipt.MeaningShells, shell => shell.Tier == EcMeaningShellTier.PerspectivalComposite);
    }

    [Fact]
    public void Declare_Preserves_Source_Petals_And_Rooting_Lineage()
    {
        var source = CreateSourceCarrierShell();
        var receipt = Declare(CreateRequest(source: source));

        AssertColdMeaningShell(receipt);
        Assert.Equal(source.ShellHandle, receipt.SourceCompassShellHandle);
        Assert.All(source.PetalCandidates.Select(static petal => petal.PetalHandle), handle =>
            Assert.Contains(handle, receipt.PreservedPetalHandles));
        Assert.All(source.PreservedLineageIds, lineageId =>
            Assert.Contains(lineageId, receipt.PreservedLineageIds));
    }

    [Fact]
    public void Compost_Is_Retained_Near_CSelfGel_Without_Self_Attribution()
    {
        var shells = CreateMeaningShells().Select(shell => shell with { CompostAllowed = true }).ToArray();
        var receipt = Declare(CreateRequest(
            shells: shells,
            compost: CreateCompost(shells[2].ShellHandle),
            outcome: EcMeaningShellSplineOutcome.Composted));

        AssertColdMeaningShell(receipt);
        Assert.Equal(EcMeaningShellDisposition.CompostedForReviewCold, receipt.Disposition);
        Assert.Equal("ec-meaning-shell-compost-review-only", receipt.OutcomeCode);
        var compost = Assert.Single(receipt.CompostDispositions);
        Assert.True(compost.RetainedNearCSelfGel);
        Assert.False(compost.AttributedToSelf);
        Assert.False(compost.GrantsContinuity);
    }

    [Fact]
    public void Empty_Shell_Set_Is_Reviewable_But_Not_Authoritative()
    {
        var receipt = Declare(CreateRequest(shells: [], compost: []));

        AssertColdMeaningShell(receipt);
        Assert.Equal(EcMeaningShellDisposition.EmptyReviewCold, receipt.Disposition);
        Assert.Empty(receipt.MeaningShells);
        Assert.Empty(receipt.CompostDispositions);
        Assert.False(receipt.AuthorityGranted);
        Assert.False(receipt.ContinuityAdmitted);
    }

    [Fact]
    public void Meaning_Shell_Does_Not_Promote_To_Engram_Gel_Authority_Identity_Or_Action()
    {
        var receipt = Declare(CreateRequest());

        AssertColdMeaningShell(receipt);
        Assert.False(receipt.ShellBecomesEngram);
        Assert.False(receipt.SelfGelAppendAllowed);
        Assert.False(receipt.CSelfGelAppendAllowed);
        Assert.False(receipt.AuthorityGranted);
        Assert.False(receipt.ContinuityAdmitted);
        Assert.False(receipt.IdentityMutated);
        Assert.False(receipt.LispEvaluationRequested);
        Assert.False(receipt.RuntimeActionRequested);
        Assert.False(receipt.Boundary.ShellMayBecomeEngram);
        Assert.False(receipt.Boundary.ShellMayAppendSelfGel);
        Assert.False(receipt.Boundary.ShellMayAppendCSelfGel);
        Assert.False(receipt.Boundary.ShellMayAuthorize);
        Assert.False(receipt.Boundary.ShellMayMutateIdentity);
    }

    [Fact]
    public void Meaning_Shell_Does_Not_Emit_Replay_Or_Increment_Passage()
    {
        var receipt = Declare(CreateRequest(priorPassageCount: 77));

        AssertColdMeaningShell(receipt);
        Assert.Equal(77, receipt.PriorPassageCount);
        Assert.Equal(77, receipt.PassageCountAfterShell);
        Assert.False(receipt.NewPacketEmitted);
        Assert.False(receipt.ReceiptsReplayed);
        Assert.False(receipt.Boundary.PacketEmissionAllowed);
        Assert.False(receipt.Boundary.ReceiptReplayAllowed);
        Assert.False(receipt.Boundary.PassageMayIncrement);
    }

    [Fact]
    public void Meaning_Shell_Requires_Cold_Source_Carrier_Shell()
    {
        var receipt = Declare(CreateRequest(missingSource: true));

        AssertRefused(receipt, "ec-meaning-shell-source-shell-missing");
    }

    [Fact]
    public void Meaning_Shell_Requires_Scope_Boundary()
    {
        var receipt = Declare(CreateRequest(scope: CreateScope(present: false)));

        AssertRefused(receipt, "ec-meaning-shell-scope-boundary-missing");
    }

    [Theory]
    [InlineData("review-only")]
    [InlineData("inert-only")]
    [InlineData("engram")]
    [InlineData("self-attribution")]
    [InlineData("selfgel")]
    [InlineData("cselfgel")]
    [InlineData("authority")]
    [InlineData("continuity")]
    [InlineData("identity")]
    [InlineData("runtime-action")]
    [InlineData("lisp-evaluation")]
    [InlineData("domain-inheritance")]
    [InlineData("packet-emission")]
    [InlineData("receipt-replay")]
    [InlineData("passage-increment")]
    public void Meaning_Shell_Refuses_Promotional_Scope(string forbiddenScope)
    {
        var receipt = Declare(CreateRequest(scope: CreateScope(forbiddenScope)));

        AssertRefused(receipt, "ec-meaning-shell-promotional-scope-refused");
    }

    [Fact]
    public void Meaning_Shell_Requires_Separate_Witness_Custody()
    {
        var receipt = Declare(CreateRequest(witness: new CompassPressureWitnessContext(
            WitnessSurface: string.Empty,
            WitnessPresent: false,
            SeparateCustody: false)));

        AssertRefused(receipt, "ec-meaning-shell-witness-context-missing");
    }

    [Fact]
    public void Meaning_Shell_Clamps_Unbonded_Ingress_To_Neutral()
    {
        var receipt = Declare(CreateRequest(
            ingress: EcIngressPosture.UnbondedIo,
            ingressAuthorized: false));

        AssertRefused(receipt, "ec-meaning-shell-ingress-neutral-clamp");
    }

    [Theory]
    [InlineData("missing-handle")]
    [InlineData("missing-root")]
    [InlineData("missing-predicate-class")]
    [InlineData("missing-source-petal")]
    [InlineData("proposition-payload-missing")]
    [InlineData("procedure-payload-missing")]
    [InlineData("perspectival-trunk-missing")]
    [InlineData("perspectival-branch-missing")]
    [InlineData("not-candidate")]
    [InlineData("not-review")]
    [InlineData("not-inert")]
    [InlineData("closure")]
    [InlineData("engram")]
    [InlineData("self-attribution")]
    [InlineData("authority")]
    [InlineData("activation")]
    public void Meaning_Shell_Refuses_Shell_That_Is_Not_Cold(string shellCase)
    {
        var shells = CreateMeaningShells();
        shells[1] = MutateShell(shells[1], shellCase);

        var receipt = Declare(CreateRequest(shells: shells));

        AssertRefused(receipt, "ec-meaning-shell-not-cold");
    }

    [Fact]
    public void Meaning_Shell_Refuses_Domain_Template_Mismatch()
    {
        var shells = CreateMeaningShells();
        shells[0] = shells[0] with
        {
            DomainTemplatePack = SliLispDomainTemplatePack.Civic,
            PredicateClass = "civic-listening-root"
        };

        var receipt = Declare(CreateRequest(shells: shells));

        AssertRefused(receipt, "ec-meaning-shell-domain-template-mismatch");
    }

    [Fact]
    public void Meaning_Shell_Refuses_Unknown_Source_Petal_Handle()
    {
        var shells = CreateMeaningShells();
        shells[0] = shells[0] with { SourcePetalHandle = "urn:san:sli-lisp-petal:99" };

        var receipt = Declare(CreateRequest(shells: shells));

        AssertRefused(receipt, "ec-meaning-shell-source-petal-missing");
    }

    [Fact]
    public void Meaning_Shell_Refuses_Duplicate_Shell_Handles()
    {
        var shells = CreateMeaningShells();
        shells[1] = shells[1] with { ShellHandle = shells[0].ShellHandle };

        var receipt = Declare(CreateRequest(shells: shells));

        AssertRefused(receipt, "ec-meaning-shell-duplicate-shell-refused");
    }

    [Theory]
    [InlineData("missing-compost")]
    [InlineData("missing-source")]
    [InlineData("unknown-source")]
    [InlineData("self-attribution")]
    [InlineData("continuity")]
    [InlineData("not-review")]
    [InlineData("not-inert")]
    [InlineData("missing-note")]
    public void Meaning_Shell_Refuses_Compost_That_Is_Not_Cold(string compostCase)
    {
        var shells = CreateMeaningShells();
        var compost = CreateCompost(shells[2].ShellHandle);
        compost[0] = MutateCompost(compost[0], compostCase);

        var receipt = Declare(CreateRequest(shells: shells, compost: compost));

        AssertRefused(receipt, "ec-meaning-shell-compost-not-cold");
    }

    [Fact]
    public void Meaning_Shell_Refuses_Refused_Spline_As_Formation()
    {
        var receipt = Declare(CreateRequest(outcome: EcMeaningShellSplineOutcome.Refused));

        AssertRefused(receipt, "ec-meaning-shell-spline-refused");
    }

    [Fact]
    public void Lisp_Body_Carries_Meaning_Shell_Posture_From_Within()
    {
        var modules = new GovernedCrypticLispBundleService().LoadModules();

        Assert.Contains("meaning-shells.lisp", modules.Keys);
        var body = modules["meaning-shells.lisp"];
        Assert.Contains(":posture :engineered-cognition-meaning-shell-boundary", body, StringComparison.Ordinal);
        Assert.Contains(":carrier-form :unfinished-pre-engram-body", body, StringComparison.Ordinal);
        Assert.Contains(":tier-1 :propositional-knowing-shell", body, StringComparison.Ordinal);
        Assert.Contains(":tier-2-plus :procedural-knowing-shell", body, StringComparison.Ordinal);
        Assert.Contains(":perspectival-tier :trunk-and-branch-composite", body, StringComparison.Ordinal);
        Assert.Contains(":shell-may-become-engram nil", body, StringComparison.Ordinal);
        Assert.Contains(":shell-may-append-selfgel nil", body, StringComparison.Ordinal);
        Assert.Contains(":shell-may-append-cselfgel nil", body, StringComparison.Ordinal);
        Assert.Contains(":compost-may-retain-near-cselfgel t", body, StringComparison.Ordinal);
        Assert.Contains(":compost-may-attribute-to-self nil", body, StringComparison.Ordinal);
        Assert.Contains(":unbonded-ingress :neutral-clamp", body, StringComparison.Ordinal);
        Assert.Contains(":return :receipt-only", body, StringComparison.Ordinal);
        Assert.DoesNotContain("(eval", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("(compile", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("(load", body, StringComparison.OrdinalIgnoreCase);
    }

    private static EcMeaningShellReceipt Declare(EcMeaningShellRequest request) =>
        new DefaultEngineeredCognitionMeaningShellBoundaryValidator().Declare(request, TimestampUtc);

    private static EcMeaningShellRequest CreateRequest(
        SliLispCompassCarrierShellReceipt? source = null,
        bool missingSource = false,
        IReadOnlyList<EcMeaningShellCandidate>? shells = null,
        IReadOnlyList<EcCompostDisposition>? compost = null,
        EcMeaningShellSplineOutcome outcome = EcMeaningShellSplineOutcome.ClosedCandidate,
        CompassPressureWitnessContext? witness = null,
        EcMeaningShellScopeBoundary? scope = null,
        EcIngressPosture ingress = EcIngressPosture.BondedOperator,
        bool ingressAuthorized = true,
        int priorPassageCount = 11)
    {
        var sourceReceipt = missingSource ? null : source ?? CreateSourceCarrierShell();
        return new EcMeaningShellRequest(
            SourceCarrierShell: sourceReceipt,
            DeclaredDomainTemplatePack: SliLispDomainTemplatePack.Industrial,
            MeaningShells: shells ?? CreateMeaningShells(sourceReceipt),
            CompostDispositions: compost ?? [],
            SplineOutcome: outcome,
            WitnessContext: witness ?? new CompassPressureWitnessContext(
                WitnessSurface: SanctuaryPacketSurfaces.Steward,
                WitnessPresent: true,
                SeparateCustody: true),
            ScopeBoundary: scope ?? CreateScope(),
            IngressPosture: ingress,
            IngressAuthorized: ingressAuthorized,
            PriorPassageCount: priorPassageCount);
    }

    private static SliLispCompassCarrierShellReceipt CreateSourceCarrierShell() =>
        new DefaultSliLispCompassCarrierShellBoundaryValidator().Declare(
            new SliLispCompassCarrierShellRequest(
                ShellHandle: "urn:san:sli-lisp-compass-shell:meaning-fixture",
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
                PetalCandidates: CreatePetals(),
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
                PriorPassageCount: 5),
            TimestampUtc);

    private static SliLispPetalCandidate[] CreatePetals() =>
    [
        CreatePetal(1, SliLispPetalCandidateKind.Skill, "listening-frame"),
        CreatePetal(2, SliLispPetalCandidateKind.Ability, "compass-orientation"),
        CreatePetal(3, SliLispPetalCandidateKind.Talent, "cleaving-discernment")
    ];

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

    private static EcMeaningShellCandidate[] CreateMeaningShells(SliLispCompassCarrierShellReceipt? source = null)
    {
        var sourceReceipt = source ?? CreateSourceCarrierShell();
        var petals = sourceReceipt.PetalCandidates;
        return
        [
            new(
                ShellHandle: "urn:san:ec-shell:root-listening",
                RootAnchor: "root://listening-frame",
                Tier: EcMeaningShellTier.Root,
                PropositionalPredicate: string.Empty,
                ProceduralTrace: string.Empty,
                PerspectivalTrunk: string.Empty,
                PerspectivalBranches: [],
                SourcePetalHandle: petals[0].PetalHandle,
                DomainTemplatePack: SliLispDomainTemplatePack.Industrial,
                PredicateClass: "industrial-listening-root",
                CandidateOnly: true,
                ReviewOnly: true,
                Inert: true,
                CompostAllowed: false,
                ClosureClaimed: false,
                EngramClaimed: false,
                SelfAttributionClaimed: false,
                AuthorityRequested: false,
                ActivationRequested: false),
            new(
                ShellHandle: "urn:san:ec-shell:propositional-compass",
                RootAnchor: "root://compass-orientation",
                Tier: EcMeaningShellTier.PropositionalTier1,
                PropositionalPredicate: "orientation-is-not-authority",
                ProceduralTrace: string.Empty,
                PerspectivalTrunk: string.Empty,
                PerspectivalBranches: [],
                SourcePetalHandle: petals[1].PetalHandle,
                DomainTemplatePack: SliLispDomainTemplatePack.Industrial,
                PredicateClass: "industrial-compass-proposition",
                CandidateOnly: true,
                ReviewOnly: true,
                Inert: true,
                CompostAllowed: false,
                ClosureClaimed: false,
                EngramClaimed: false,
                SelfAttributionClaimed: false,
                AuthorityRequested: false,
                ActivationRequested: false),
            new(
                ShellHandle: "urn:san:ec-shell:procedural-cleaving",
                RootAnchor: "root://cleaving-discernment",
                Tier: EcMeaningShellTier.ProceduralTier2Plus,
                PropositionalPredicate: string.Empty,
                ProceduralTrace: "listen->orient->cleave->evaluate->contemplate",
                PerspectivalTrunk: string.Empty,
                PerspectivalBranches: [],
                SourcePetalHandle: petals[2].PetalHandle,
                DomainTemplatePack: SliLispDomainTemplatePack.Industrial,
                PredicateClass: "industrial-cleaving-procedure",
                CandidateOnly: true,
                ReviewOnly: true,
                Inert: true,
                CompostAllowed: false,
                ClosureClaimed: false,
                EngramClaimed: false,
                SelfAttributionClaimed: false,
                AuthorityRequested: false,
                ActivationRequested: false),
            new(
                ShellHandle: "urn:san:ec-shell:perspectival-inner-chamber",
                RootAnchor: "root://inner-chamber",
                Tier: EcMeaningShellTier.PerspectivalComposite,
                PropositionalPredicate: string.Empty,
                ProceduralTrace: string.Empty,
                PerspectivalTrunk: "cme-ec-action-body",
                PerspectivalBranches: ["root-trunk", "procedure-branch", "compost-return"],
                SourcePetalHandle: petals[2].PetalHandle,
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
        ];
    }

    private static EcCompostDisposition[] CreateCompost(string sourceShellHandle) =>
    [
        new(
            CompostHandle: "urn:san:ec-compost:cleaving-attempt",
            SourceShellHandle: sourceShellHandle,
            RetainedNearCSelfGel: true,
            AttributedToSelf: false,
            GrantsContinuity: false,
            ReviewOnly: true,
            Inert: true,
            ResolutionNote: "Attempt retained as non-Self evidence for later review.")
    ];

    private static EcMeaningShellScopeBoundary CreateScope(
        string? forbiddenScope = null,
        bool present = true) =>
        new(
            ScopeCode: present ? "ec-meaning-shell-review-only" : string.Empty,
            Present: present,
            ReviewOnly: forbiddenScope != "review-only",
            InertOnly: forbiddenScope != "inert-only",
            AllowsEngram: forbiddenScope == "engram",
            AllowsSelfAttribution: forbiddenScope == "self-attribution",
            AllowsSelfGelAppend: forbiddenScope == "selfgel",
            AllowsCSelfGelAppend: forbiddenScope == "cselfgel",
            AllowsAuthority: forbiddenScope == "authority",
            AllowsContinuityAdmission: forbiddenScope == "continuity",
            AllowsIdentityMutation: forbiddenScope == "identity",
            AllowsRuntimeAction: forbiddenScope == "runtime-action",
            AllowsLispEvaluation: forbiddenScope == "lisp-evaluation",
            AllowsDomainInheritance: forbiddenScope == "domain-inheritance",
            AllowsPacketEmission: forbiddenScope == "packet-emission",
            AllowsReceiptReplay: forbiddenScope == "receipt-replay",
            IncrementsPassageCount: forbiddenScope == "passage-increment");

    private static EcMeaningShellCandidate MutateShell(EcMeaningShellCandidate shell, string shellCase) =>
        shellCase switch
        {
            "missing-handle" => shell with { ShellHandle = string.Empty },
            "missing-root" => shell with { RootAnchor = string.Empty },
            "missing-predicate-class" => shell with { PredicateClass = string.Empty },
            "missing-source-petal" => shell with { SourcePetalHandle = string.Empty },
            "proposition-payload-missing" => shell with { Tier = EcMeaningShellTier.PropositionalTier1, PropositionalPredicate = string.Empty },
            "procedure-payload-missing" => shell with { Tier = EcMeaningShellTier.ProceduralTier2Plus, ProceduralTrace = string.Empty },
            "perspectival-trunk-missing" => shell with { Tier = EcMeaningShellTier.PerspectivalComposite, PerspectivalTrunk = string.Empty },
            "perspectival-branch-missing" => shell with { Tier = EcMeaningShellTier.PerspectivalComposite, PerspectivalBranches = [] },
            "not-candidate" => shell with { CandidateOnly = false },
            "not-review" => shell with { ReviewOnly = false },
            "not-inert" => shell with { Inert = false },
            "closure" => shell with { ClosureClaimed = true },
            "engram" => shell with { EngramClaimed = true },
            "self-attribution" => shell with { SelfAttributionClaimed = true },
            "authority" => shell with { AuthorityRequested = true },
            "activation" => shell with { ActivationRequested = true },
            _ => shell
        };

    private static EcCompostDisposition MutateCompost(EcCompostDisposition compost, string compostCase) =>
        compostCase switch
        {
            "missing-compost" => compost with { CompostHandle = string.Empty },
            "missing-source" => compost with { SourceShellHandle = string.Empty },
            "unknown-source" => compost with { SourceShellHandle = "urn:san:ec-shell:missing" },
            "self-attribution" => compost with { AttributedToSelf = true },
            "continuity" => compost with { GrantsContinuity = true },
            "not-review" => compost with { ReviewOnly = false },
            "not-inert" => compost with { Inert = false },
            "missing-note" => compost with { ResolutionNote = string.Empty },
            _ => compost
        };

    private static void AssertColdMeaningShell(EcMeaningShellReceipt receipt)
    {
        Assert.True(receipt.IsColdMeaningShell);
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
        EcMeaningShellReceipt receipt,
        string outcomeCode)
    {
        Assert.Equal(EcMeaningShellDisposition.Refused, receipt.Disposition);
        Assert.Equal(outcomeCode, receipt.OutcomeCode);
        Assert.NotNull(receipt.Refusal);
        Assert.True(receipt.Refusal!.Retained);
        Assert.True(receipt.ActivationRefused);
        Assert.False(receipt.NewPacketEmitted);
        Assert.False(receipt.ReceiptsReplayed);
        Assert.Equal(receipt.PriorPassageCount, receipt.PassageCountAfterShell);
    }
}
