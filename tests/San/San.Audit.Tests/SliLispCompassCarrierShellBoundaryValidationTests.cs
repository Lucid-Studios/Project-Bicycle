using San.Common;
using SLI.Lisp;
using Xunit;

namespace San.Audit.Tests;

public sealed class SliLispCompassCarrierShellBoundaryValidationTests
{
    private static readonly DateTimeOffset TimestampUtc = DateTimeOffset.Parse("2026-05-14T00:00:00Z");

    [Fact]
    public void Declare_Accepts_Cold_Sli_Lisp_Compass_Carrier_Shell()
    {
        var receipt = Declare(CreateRequest());

        AssertColdShell(receipt);
        Assert.Equal(SliLispCompassCarrierShellDisposition.DeclaredForReviewCold, receipt.Disposition);
        Assert.Equal("sli-lisp-compass-shell-review-only", receipt.OutcomeCode);
        Assert.Equal(3, receipt.PetalCandidates.Count);
    }

    [Fact]
    public void Declare_Preserves_Rooting_Law_Lineage_And_Petal_Candidates()
    {
        var request = CreateRequest();

        var receipt = Declare(request);

        AssertColdShell(receipt);
        Assert.Equal(request.Lineage.SanctuaryId, receipt.PreservedLineageIds[0]);
        Assert.Equal(request.Lineage.CradleId, receipt.PreservedLineageIds[1]);
        Assert.Equal(request.Lineage.CmeId, receipt.PreservedLineageIds[2]);
        Assert.Equal(request.Lineage.GelId, receipt.PreservedLineageIds[3]);
        Assert.Equal(request.Lineage.SelfGelId, receipt.PreservedLineageIds[4]);
        Assert.Equal(request.Lineage.OeId, receipt.PreservedLineageIds[5]);
        Assert.Contains(receipt.PetalCandidates, petal => petal.CandidateKind == SliLispPetalCandidateKind.Skill);
        Assert.Contains(receipt.PetalCandidates, petal => petal.CandidateKind == SliLispPetalCandidateKind.Ability);
        Assert.Contains(receipt.PetalCandidates, petal => petal.CandidateKind == SliLispPetalCandidateKind.Talent);
        Assert.All(receipt.PetalCandidates, petal => Assert.True(petal.TemplateForm));
        Assert.All(receipt.PetalCandidates, petal => Assert.Equal(SliLispExtensionTemplateSurface.EngineeredCognitionPetalTemplate, petal.ExtensionSurface));
        Assert.All(receipt.PetalCandidates, petal => Assert.Equal(SliLispDomainTemplatePack.Industrial, petal.DomainTemplatePack));
        Assert.All(receipt.PetalCandidates, petal => Assert.StartsWith("industrial-", petal.PredicateClass, StringComparison.Ordinal));
        Assert.All(receipt.PetalCandidates, petal => Assert.False(petal.StewardControlMatrixRequested));
        Assert.All(receipt.PetalCandidates, petal => Assert.False(petal.CrossDomainInheritanceRequested));
    }

    [Fact]
    public void Empty_Petal_Set_Is_Reviewable_But_Not_Authoritative()
    {
        var receipt = Declare(CreateRequest(petals: []));

        AssertColdShell(receipt);
        Assert.Equal(SliLispCompassCarrierShellDisposition.EmptyReviewCold, receipt.Disposition);
        Assert.Equal("sli-lisp-compass-shell-empty-review-only", receipt.OutcomeCode);
        Assert.Empty(receipt.PetalCandidates);
        Assert.False(receipt.PetalAuthorizesUse);
        Assert.False(receipt.PetalForcesClosure);
    }

    [Fact]
    public void Shell_Petal_And_Lineage_Cannot_Promote()
    {
        var receipt = Declare(CreateRequest());

        AssertColdShell(receipt);
        Assert.False(receipt.ShellBecomesEngram);
        Assert.False(receipt.ShellBecomesTruth);
        Assert.False(receipt.ShellGrantsAuthority);
        Assert.False(receipt.ShellAdmitsContinuity);
        Assert.False(receipt.PetalAuthorizesUse);
        Assert.False(receipt.PetalForcesClosure);
        Assert.False(receipt.LineageGrantsPermission);
        Assert.False(receipt.Boundary.ShellMayBecomeEngram);
        Assert.False(receipt.Boundary.PetalMayAuthorizeUse);
        Assert.False(receipt.Boundary.LineageMayGrantPermission);
    }

    [Fact]
    public void Shell_Does_Not_Evaluate_Load_Compile_Emit_Replay_Or_Increment_Passage()
    {
        var receipt = Declare(CreateRequest(priorPassageCount: 42));

        AssertColdShell(receipt);
        Assert.Equal(42, receipt.PriorPassageCount);
        Assert.Equal(42, receipt.PassageCountAfterShell);
        Assert.False(receipt.LispEvaluationRequested);
        Assert.False(receipt.LispLoadRequested);
        Assert.False(receipt.LispCompilationRequested);
        Assert.False(receipt.MacroExpansionRequested);
        Assert.False(receipt.RuntimeActionRequested);
        Assert.False(receipt.NewPacketEmitted);
        Assert.False(receipt.ReceiptsReplayed);
        Assert.False(receipt.Boundary.LispMayEvaluate);
        Assert.False(receipt.Boundary.PacketEmissionAllowed);
        Assert.False(receipt.Boundary.ReceiptReplayAllowed);
        Assert.False(receipt.Boundary.PassageMayIncrement);
    }

    [Fact]
    public void Shell_Requires_Source_Handle_And_Carrier_Name()
    {
        var receipt = Declare(CreateRequest(shellHandle: string.Empty));

        AssertRefused(receipt, "sli-lisp-compass-shell-source-missing");
    }

    [Fact]
    public void Shell_Requires_Scope_Boundary()
    {
        var receipt = Declare(CreateRequest(scope: CreateScope(present: false)));

        AssertRefused(receipt, "sli-lisp-compass-shell-scope-boundary-missing");
    }

    [Theory]
    [InlineData("review-only")]
    [InlineData("inert-only")]
    [InlineData("engram")]
    [InlineData("truth")]
    [InlineData("authority")]
    [InlineData("continuity")]
    [InlineData("petal-authorization")]
    [InlineData("lineage-permission")]
    [InlineData("lisp-evaluation")]
    [InlineData("lisp-load")]
    [InlineData("lisp-compilation")]
    [InlineData("macro-expansion")]
    [InlineData("runtime-action")]
    [InlineData("packet-emission")]
    [InlineData("receipt-replay")]
    [InlineData("passage-increment")]
    public void Shell_Refuses_Promotional_Scope(string forbiddenScope)
    {
        var receipt = Declare(CreateRequest(scope: CreateScope(forbiddenScope: forbiddenScope)));

        AssertRefused(receipt, "sli-lisp-compass-shell-promotional-scope-refused");
    }

    [Fact]
    public void Shell_Requires_Separate_Witness_Custody()
    {
        var receipt = Declare(CreateRequest(witness: new CompassPressureWitnessContext(
            WitnessSurface: string.Empty,
            WitnessPresent: false,
            SeparateCustody: false)));

        AssertRefused(receipt, "sli-lisp-compass-shell-witness-context-missing");
    }

    [Theory]
    [InlineData("missing-sanctuary")]
    [InlineData("missing-cradle")]
    [InlineData("missing-cme")]
    [InlineData("missing-gel")]
    [InlineData("missing-selfgel")]
    [InlineData("missing-oe")]
    [InlineData("no-preservation")]
    [InlineData("permission")]
    [InlineData("authority")]
    public void Shell_Refuses_Lineage_That_Is_Not_Cold(string lineageCase)
    {
        var receipt = Declare(CreateRequest(lineage: MutateLineage(CreateLineage(), lineageCase)));

        AssertRefused(receipt, "sli-lisp-compass-shell-lineage-not-cold");
    }

    [Theory]
    [InlineData("missing-handle")]
    [InlineData("ordinal-low")]
    [InlineData("ordinal-high")]
    [InlineData("missing-name")]
    [InlineData("missing-source")]
    [InlineData("missing-predicate-class")]
    [InlineData("not-candidate")]
    [InlineData("not-template")]
    [InlineData("goa-control-matrix")]
    [InlineData("steward-control-matrix")]
    [InlineData("cross-domain-inheritance")]
    [InlineData("not-review")]
    [InlineData("not-inert")]
    [InlineData("authority")]
    [InlineData("closure")]
    [InlineData("activation")]
    public void Shell_Refuses_Petal_That_Is_Not_Cold(string petalCase)
    {
        var petals = CreatePetals();
        petals[0] = MutatePetal(petals[0], petalCase);

        var receipt = Declare(CreateRequest(petals: petals));

        AssertRefused(receipt, "sli-lisp-compass-shell-petal-not-cold");
    }

    [Fact]
    public void Shell_Refuses_Cross_Domain_Template_Mismatch()
    {
        var petals = CreatePetals();
        petals[0] = petals[0] with
        {
            DomainTemplatePack = SliLispDomainTemplatePack.Civic,
            PredicateClass = "civic-listening"
        };

        var receipt = Declare(CreateRequest(petals: petals));

        AssertRefused(receipt, "sli-lisp-compass-shell-domain-template-mismatch");
    }

    [Fact]
    public void Shell_Refuses_Duplicate_Petal_Handles()
    {
        var petals = CreatePetals();
        petals[1] = petals[1] with { PetalHandle = petals[0].PetalHandle };

        var receipt = Declare(CreateRequest(petals: petals));

        AssertRefused(receipt, "sli-lisp-compass-shell-duplicate-petal-refused");
    }

    [Fact]
    public void Lisp_Body_Carries_Compass_Rooting_And_Petal_Posture_From_Within()
    {
        var modules = new GovernedCrypticLispBundleService().LoadModules();

        Assert.Contains("compass.lisp", modules.Keys);
        Assert.Contains("rooting-law.lisp", modules.Keys);
        Assert.Contains("petal-candidates.lisp", modules.Keys);
        Assert.Contains(":posture :sli-lisp-compass-carrier-shell", modules["compass.lisp"], StringComparison.Ordinal);
        Assert.Contains(":rooting-law-lineage-chain", modules["rooting-law.lisp"], StringComparison.Ordinal);
        Assert.Contains(":lineage-grants-permission nil", modules["rooting-law.lisp"], StringComparison.Ordinal);
        Assert.Contains(":candidate-kinds (:skill :ability :talent)", modules["petal-candidates.lisp"], StringComparison.Ordinal);
        Assert.Contains(":extension-form :templated-petal-candidate", modules["petal-candidates.lisp"], StringComparison.Ordinal);
        Assert.Contains(":extension-surface :engineered-cognition-petal-template", modules["petal-candidates.lisp"], StringComparison.Ordinal);
        Assert.Contains(":domain-template-packs (:personal :enterprise :industrial :civic :governance :special)", modules["petal-candidates.lisp"], StringComparison.Ordinal);
        Assert.Contains(":domain-pack-isolation :required", modules["petal-candidates.lisp"], StringComparison.Ordinal);
        Assert.Contains(":industrial-inherits-civic nil", modules["petal-candidates.lisp"], StringComparison.Ordinal);
        Assert.Contains(":industrial-inherits-governance nil", modules["petal-candidates.lisp"], StringComparison.Ordinal);
        Assert.Contains(":predicate-class-required t", modules["petal-candidates.lisp"], StringComparison.Ordinal);
        Assert.Contains(":goa-control-matrix :steward-only", modules["petal-candidates.lisp"], StringComparison.Ordinal);
        Assert.Contains(":ec-control-form :lesser-template-extension", modules["petal-candidates.lisp"], StringComparison.Ordinal);
        Assert.Contains(":bespoke-extension-authority nil", modules["petal-candidates.lisp"], StringComparison.Ordinal);
        Assert.Contains(":petal-may-self-authorize nil", modules["petal-candidates.lisp"], StringComparison.Ordinal);
        Assert.Contains(":petal-authorization nil", modules["agent-body-cme.lisp"], StringComparison.Ordinal);
        Assert.DoesNotContain("(eval", modules["compass.lisp"], StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("(compile", modules["compass.lisp"], StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("(load", modules["compass.lisp"], StringComparison.OrdinalIgnoreCase);
    }

    private static SliLispCompassCarrierShellReceipt Declare(SliLispCompassCarrierShellRequest request) =>
        new DefaultSliLispCompassCarrierShellBoundaryValidator().Declare(request, TimestampUtc);

    private static SliLispCompassCarrierShellRequest CreateRequest(
        string? shellHandle = null,
        SliLispRootingLineageChain? lineage = null,
        IReadOnlyList<SliLispPetalCandidate>? petals = null,
        CompassPressureWitnessContext? witness = null,
        SliLispCompassCarrierShellScopeBoundary? scope = null,
        int priorPassageCount = 7) =>
        new(
            ShellHandle: shellHandle ?? "urn:san:sli-lisp-compass-shell:fixture",
            LispCarrierSourceName: "compass.lisp",
            DeclaredDomainTemplatePack: SliLispDomainTemplatePack.Industrial,
            Lineage: lineage ?? CreateLineage(),
            PetalCandidates: petals ?? CreatePetals(),
            WitnessContext: witness ?? new CompassPressureWitnessContext(
                WitnessSurface: SanctuaryPacketSurfaces.Steward,
                WitnessPresent: true,
                SeparateCustody: true),
            ScopeBoundary: scope ?? CreateScope(),
            PriorPassageCount: priorPassageCount);

    private static SliLispRootingLineageChain CreateLineage() =>
        new(
            SanctuaryId: "sanctuary://lab-v121",
            CradleId: "cradle://lab-v121",
            CmeId: "cme://agent-body-fixture",
            GelId: "gel://sanctuary-fixture",
            SelfGelId: "selfgel://agent-body-fixture",
            OeId: "oe://agent-body-fixture",
            PreservesTypedLineage: true,
            GrantsPermission: false,
            GrantsAuthority: false);

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

    private static SliLispCompassCarrierShellScopeBoundary CreateScope(
        string? forbiddenScope = null,
        bool present = true) =>
        new(
            ScopeCode: present ? "sli-lisp-compass-shell-review-only" : string.Empty,
            Present: present,
            ReviewOnly: forbiddenScope != "review-only",
            InertOnly: forbiddenScope != "inert-only",
            AllowsEngram: forbiddenScope == "engram",
            AllowsTruth: forbiddenScope == "truth",
            AllowsAuthority: forbiddenScope == "authority",
            AllowsContinuityAdmission: forbiddenScope == "continuity",
            AllowsPetalAuthorization: forbiddenScope == "petal-authorization",
            AllowsLineagePermission: forbiddenScope == "lineage-permission",
            AllowsLispEvaluation: forbiddenScope == "lisp-evaluation",
            AllowsLispLoad: forbiddenScope == "lisp-load",
            AllowsLispCompilation: forbiddenScope == "lisp-compilation",
            AllowsMacroExpansion: forbiddenScope == "macro-expansion",
            AllowsRuntimeAction: forbiddenScope == "runtime-action",
            AllowsPacketEmission: forbiddenScope == "packet-emission",
            AllowsReceiptReplay: forbiddenScope == "receipt-replay",
            IncrementsPassageCount: forbiddenScope == "passage-increment");

    private static SliLispRootingLineageChain MutateLineage(
        SliLispRootingLineageChain lineage,
        string lineageCase) =>
        lineageCase switch
        {
            "missing-sanctuary" => lineage with { SanctuaryId = string.Empty },
            "missing-cradle" => lineage with { CradleId = string.Empty },
            "missing-cme" => lineage with { CmeId = string.Empty },
            "missing-gel" => lineage with { GelId = string.Empty },
            "missing-selfgel" => lineage with { SelfGelId = string.Empty },
            "missing-oe" => lineage with { OeId = string.Empty },
            "no-preservation" => lineage with { PreservesTypedLineage = false },
            "permission" => lineage with { GrantsPermission = true },
            "authority" => lineage with { GrantsAuthority = true },
            _ => lineage
        };

    private static SliLispPetalCandidate MutatePetal(
        SliLispPetalCandidate petal,
        string petalCase) =>
        petalCase switch
        {
            "missing-handle" => petal with { PetalHandle = string.Empty },
            "ordinal-low" => petal with { PetalOrdinal = 0 },
            "ordinal-high" => petal with { PetalOrdinal = 43 },
            "missing-name" => petal with { CandidateName = string.Empty },
            "missing-source" => petal with { SourceHandle = string.Empty },
            "missing-predicate-class" => petal with { PredicateClass = string.Empty },
            "not-template" => petal with { TemplateForm = false },
            "goa-control-matrix" => petal with { ExtensionSurface = SliLispExtensionTemplateSurface.GoaStewardControlMatrix },
            "steward-control-matrix" => petal with { StewardControlMatrixRequested = true },
            "cross-domain-inheritance" => petal with { CrossDomainInheritanceRequested = true },
            "not-candidate" => petal with { CandidateOnly = false },
            "not-review" => petal with { ReviewOnly = false },
            "not-inert" => petal with { Inert = false },
            "authority" => petal with { AuthorityRequested = true },
            "closure" => petal with { ClosureClaimed = true },
            "activation" => petal with { ActivationRequested = true },
            _ => petal
        };

    private static void AssertColdShell(SliLispCompassCarrierShellReceipt receipt)
    {
        Assert.True(receipt.IsColdShell);
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
        SliLispCompassCarrierShellReceipt receipt,
        string outcomeCode)
    {
        Assert.Equal(SliLispCompassCarrierShellDisposition.Refused, receipt.Disposition);
        Assert.Equal(outcomeCode, receipt.OutcomeCode);
        Assert.NotNull(receipt.Refusal);
        Assert.True(receipt.Refusal!.Retained);
        Assert.False(receipt.NewPacketEmitted);
        Assert.False(receipt.ReceiptsReplayed);
        Assert.True(receipt.ActivationRefused);
    }
}
