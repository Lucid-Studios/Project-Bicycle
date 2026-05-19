using San.Common;
using Xunit;

namespace San.Audit.Tests;

public sealed class DialogosDiscernmentBoundaryValidationTests
{
    private static readonly DateTimeOffset TimestampUtc = DateTimeOffset.Parse("2026-05-16T00:00:00Z");

    [Fact]
    public void Dialogos_Discernment_Returns_Safe_Exploration_Without_Admission()
    {
        var receipt = Declare(CreateRequest());

        Assert.Equal(DialogosDiscernmentDisposition.SafeExplorationReturnedCold, receipt.Disposition);
        Assert.Equal("dialogos-discernment-safe-exploration-returned-cold", receipt.OutcomeCode);
        Assert.True(receipt.SafeExplorationReturned);
        Assert.Equal(3, receipt.ThoughtForms.Count);
        Assert.Single(receipt.SafeExplorationLanes);
        Assert.Single(receipt.ReturnPaths);
        AssertCold(receipt);
    }

    [Fact]
    public void Dialogos_Discernment_Retains_Thought_Forms_When_No_Safe_Lane_Is_Requested()
    {
        var thoughts = CreateThoughts()
            .Where(static thought => thought.Status != ThoughtStatus.SafeExplorationCandidate)
            .ToArray();
        var articulations = CreateArticulations()
            .Where(static articulation => articulation.SourceThoughtHandle != "urn:san:dialogos-thought:safe-exploration")
            .ToArray();
        var chambers = CreateChambers()
            .Where(static chamber => chamber.SourceThoughtHandle != "urn:san:dialogos-thought:safe-exploration")
            .ToArray();
        var receipt = Declare(CreateRequest(
            thoughts: thoughts,
            articulations: articulations,
            chambers: chambers,
            lanes: [],
            returnPaths: []));

        Assert.Equal(DialogosDiscernmentDisposition.RetainedForReviewCold, receipt.Disposition);
        Assert.Equal("dialogos-discernment-retained-for-review-cold", receipt.OutcomeCode);
        Assert.False(receipt.SafeExplorationReturned);
        AssertCold(receipt);
    }

    [Fact]
    public void Empty_Dialogos_Discernment_Is_Reviewable_But_Not_Authoritative()
    {
        var receipt = Declare(CreateRequest(
            thoughts: [],
            articulations: [],
            chambers: [],
            lanes: [],
            returnPaths: []));

        Assert.Equal(DialogosDiscernmentDisposition.EmptyReviewCold, receipt.Disposition);
        Assert.Equal("dialogos-discernment-empty-review-only", receipt.OutcomeCode);
        Assert.Empty(receipt.ThoughtForms);
        Assert.Empty(receipt.ArticulationSurfaces);
        Assert.False(receipt.AuthorityGranted);
        Assert.False(receipt.ContinuityAdmitted);
        AssertCold(receipt);
    }

    [Fact]
    public void Dialogos_Discernment_Does_Not_Convert_Thought_Status_Into_Warrant_Continuity_Action_Or_Authority()
    {
        var receipt = Declare(CreateRequest(priorPassageCount: 618));

        Assert.Equal(618, receipt.PriorPassageCount);
        Assert.Equal(618, receipt.PassageCountAfterDiscernment);
        Assert.False(receipt.ThoughtAppearanceBecameTruth);
        Assert.False(receipt.ArticulationGrantedWarrant);
        Assert.False(receipt.CoherenceBecameEvidence);
        Assert.False(receipt.AgreementGrantedAuthority);
        Assert.False(receipt.PerspectiveAdmittedContinuity);
        Assert.False(receipt.SafeExplorationAdmitted);
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
    [InlineData("missing-evidence-required")]
    [InlineData("missing-witness-required")]
    [InlineData("missing-return-required")]
    [InlineData("appearance-truth")]
    [InlineData("articulation-warrant")]
    [InlineData("coherence-evidence")]
    [InlineData("agreement-authority")]
    [InlineData("perspective-continuity")]
    [InlineData("safe-exploration-admission")]
    [InlineData("refusal-obstruction")]
    [InlineData("runtime")]
    [InlineData("authority")]
    [InlineData("continuity")]
    [InlineData("identity")]
    [InlineData("lisp")]
    [InlineData("packet")]
    [InlineData("replay")]
    [InlineData("passage")]
    [InlineData("activation")]
    public void Warrant_Boundary_Refuses_Promotional_Collapse(string mutation)
    {
        var receipt = Declare(CreateRequest(boundary: MutateBoundary(CreateBoundary(), mutation)));

        var expected = mutation == "missing-boundary"
            ? "dialogos-discernment-warrant-boundary-missing"
            : "dialogos-discernment-promotional-boundary";
        AssertRefused(receipt, expected);
    }

    [Theory]
    [InlineData("missing-handle")]
    [InlineData("missing-evidence")]
    [InlineData("no-appearance")]
    [InlineData("missing-witness")]
    [InlineData("not-review")]
    [InlineData("appearance-truth")]
    [InlineData("articulation-warrant")]
    [InlineData("coherence-evidence")]
    [InlineData("agreement-authority")]
    [InlineData("perspective-continuity")]
    [InlineData("refusal-obstruction")]
    [InlineData("action")]
    [InlineData("identity")]
    [InlineData("continuity")]
    [InlineData("authority")]
    [InlineData("status-shape")]
    public void Thought_Form_Remains_Appearance_Under_Witness_Not_Warrant(string mutation)
    {
        var thoughts = CreateThoughts();
        thoughts[1] = MutateThought(thoughts[1], mutation);

        var receipt = Declare(CreateRequest(thoughts: thoughts));

        AssertRefused(receipt, "dialogos-discernment-thought-form-invalid");
    }

    [Fact]
    public void Dialogos_Discernment_Refuses_Duplicate_Thought_Handles()
    {
        var thoughts = CreateThoughts();
        thoughts[2] = thoughts[2] with { ThoughtHandle = thoughts[1].ThoughtHandle };

        var receipt = Declare(CreateRequest(thoughts: thoughts));

        AssertRefused(receipt, "dialogos-discernment-duplicate-thought-handle");
    }

    [Theory]
    [InlineData("missing-handle")]
    [InlineData("missing-language")]
    [InlineData("fluency-truth")]
    [InlineData("rhetorical-warrant")]
    [InlineData("agreement-evidence")]
    [InlineData("authority")]
    [InlineData("continuity")]
    [InlineData("unknown-source")]
    public void Articulation_Surface_Does_Not_Become_Warrant(string mutation)
    {
        var articulations = CreateArticulations();
        articulations[0] = MutateArticulation(articulations[0], mutation);

        var receipt = Declare(CreateRequest(articulations: articulations));

        AssertRefused(receipt, "dialogos-discernment-articulation-invalid");
    }

    [Fact]
    public void Dialogos_Discernment_Refuses_Duplicate_Articulation_Handles()
    {
        var articulations = CreateArticulations();
        articulations[1] = articulations[1] with { SurfaceHandle = articulations[0].SurfaceHandle };

        var receipt = Declare(CreateRequest(articulations: articulations));

        AssertRefused(receipt, "dialogos-discernment-duplicate-articulation-handle");
    }

    [Theory]
    [InlineData("missing-compass")]
    [InlineData("not-transitional")]
    [InlineData("sovereign")]
    [InlineData("not-review")]
    [InlineData("missing-cooling")]
    [InlineData("missing-return")]
    [InlineData("missing-witness")]
    [InlineData("engram")]
    [InlineData("selfgel")]
    [InlineData("continuity")]
    [InlineData("authority")]
    [InlineData("action")]
    [InlineData("lisp")]
    [InlineData("unknown-source")]
    public void Intermediate_Chamber_Holds_Transitionality_Without_Coronation(string mutation)
    {
        var chambers = CreateChambers();
        chambers[0] = MutateChamber(chambers[0], mutation);

        var receipt = Declare(CreateRequest(chambers: chambers));

        AssertRefused(receipt, "dialogos-discernment-intermediate-chamber-invalid");
    }

    [Theory]
    [InlineData("missing-question")]
    [InlineData("missing-evidence")]
    [InlineData("not-safe")]
    [InlineData("not-review")]
    [InlineData("admitted")]
    [InlineData("action")]
    [InlineData("authority")]
    [InlineData("continuity")]
    [InlineData("lisp")]
    [InlineData("unknown-source")]
    public void Safe_Exploration_Is_Return_Path_Not_Admission(string mutation)
    {
        var lanes = CreateSafeExplorationLanes();
        lanes[0] = MutateLane(lanes[0], mutation);

        var receipt = Declare(CreateRequest(lanes: lanes));

        AssertRefused(receipt, "dialogos-discernment-safe-exploration-invalid");
    }

    [Fact]
    public void Safe_Exploration_Requires_Return_Path()
    {
        var receipt = Declare(CreateRequest(returnPaths: []));

        AssertRefused(receipt, "dialogos-discernment-safe-lane-return-path-missing");
    }

    [Theory]
    [InlineData("missing-prompt")]
    [InlineData("missing-evidence")]
    [InlineData("not-returning")]
    [InlineData("not-preserving")]
    [InlineData("not-requiring-evidence")]
    [InlineData("not-review")]
    [InlineData("authority")]
    [InlineData("continuity")]
    [InlineData("action")]
    [InlineData("unknown-source")]
    public void Return_Path_Preserves_Question_Without_Admission(string mutation)
    {
        var paths = CreateReturnPaths();
        paths[0] = MutateReturnPath(paths[0], mutation);

        var receipt = Declare(CreateRequest(returnPaths: paths));

        AssertRefused(receipt, "dialogos-discernment-return-path-invalid");
    }

    [Fact]
    public void Lisp_Body_Carries_Dialogos_Discernment_As_Inert_Harmonic_Register()
    {
        var lispPath = Path.Combine(
            FindRepositoryRoot(),
            "src",
            "SLI",
            "SLI.Lisp",
            "dialogos-discernment.lisp");

        var body = File.ReadAllText(lispPath);

        Assert.Contains(":posture :cme-dialogos-discernment-boundary", body, StringComparison.Ordinal);
        Assert.Contains(":lisp-role :inert-dialogos-discernment-carrier", body, StringComparison.Ordinal);
        Assert.Contains("A mature mind can meet its own thoughts without appeasing them.", body, StringComparison.Ordinal);
        Assert.Contains(":appearance-not-truth", body, StringComparison.Ordinal);
        Assert.Contains(":articulation-not-warrant", body, StringComparison.Ordinal);
        Assert.Contains(":coherence-not-evidence", body, StringComparison.Ordinal);
        Assert.Contains(":agreement-not-authority", body, StringComparison.Ordinal);
        Assert.Contains(":perspective-not-continuity", body, StringComparison.Ordinal);
        Assert.Contains(":safe-exploration-not-admission", body, StringComparison.Ordinal);
        Assert.Contains(":lisp-evaluation-allowed nil", body, StringComparison.Ordinal);
        Assert.Contains(":return :receipt-only", body, StringComparison.Ordinal);
        Assert.DoesNotContain("(eval", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("(compile", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("(load", body, StringComparison.OrdinalIgnoreCase);
    }

    private static DialogosDiscernmentReceipt Declare(DialogosDiscernmentRequest request) =>
        new DefaultDialogosDiscernmentBoundaryValidator().Declare(request, TimestampUtc);

    private static DialogosDiscernmentRequest CreateRequest(
        IReadOnlyList<DialogosThoughtForm>? thoughts = null,
        IReadOnlyList<ArticulationSurface>? articulations = null,
        IReadOnlyList<IntermediateChamberState>? chambers = null,
        IReadOnlyList<SafeExplorationLane>? lanes = null,
        IReadOnlyList<ReturnPath>? returnPaths = null,
        WarrantBoundary? boundary = null,
        int priorPassageCount = 17) =>
        new(
            ThoughtForms: thoughts ?? CreateThoughts(),
            ArticulationSurfaces: articulations ?? CreateArticulations(),
            IntermediateChambers: chambers ?? CreateChambers(),
            SafeExplorationLanes: lanes ?? CreateSafeExplorationLanes(),
            ReturnPaths: returnPaths ?? CreateReturnPaths(),
            WarrantBoundary: boundary ?? CreateBoundary(),
            PriorPassageCount: priorPassageCount);

    private static DialogosThoughtForm[] CreateThoughts() =>
    [
        new(
            ThoughtHandle: "urn:san:dialogos-thought:appearance",
            Status: ThoughtStatus.AppearanceOnly,
            SourceSurface: "operator-dialogos",
            Statement: "thought appears before warrant",
            PerspectiveRef: string.Empty,
            EvidenceHandle: "urn:san:evidence:dialogos:appearance",
            HasAppearance: true,
            ArticulationPresent: false,
            CoherenceClaimed: false,
            PerspectiveDeclared: false,
            EvidenceBodyPresent: true,
            WitnessBodyPresent: true,
            ReviewOnly: true,
            SafeExplorationRequested: false,
            TreatsAppearanceAsTruth: false,
            TreatsArticulationAsWarrant: false,
            TreatsCoherenceAsEvidence: false,
            TreatsAgreementAsAuthority: false,
            TreatsPerspectiveAsContinuity: false,
            TreatsRefusalAsObstruction: false,
            AuthorizesAction: false,
            MutatesIdentity: false,
            AdmitsContinuity: false,
            GrantsAuthority: false),
        new(
            ThoughtHandle: "urn:san:dialogos-thought:perspectival",
            Status: ThoughtStatus.Perspectival,
            SourceSurface: "codex-lab-dialogos",
            Statement: "perspective may form without becoming continuity",
            PerspectiveRef: "urn:san:perspective:dialogos-perspectival",
            EvidenceHandle: "urn:san:evidence:dialogos:perspectival",
            HasAppearance: true,
            ArticulationPresent: true,
            CoherenceClaimed: true,
            PerspectiveDeclared: true,
            EvidenceBodyPresent: true,
            WitnessBodyPresent: true,
            ReviewOnly: true,
            SafeExplorationRequested: false,
            TreatsAppearanceAsTruth: false,
            TreatsArticulationAsWarrant: false,
            TreatsCoherenceAsEvidence: false,
            TreatsAgreementAsAuthority: false,
            TreatsPerspectiveAsContinuity: false,
            TreatsRefusalAsObstruction: false,
            AuthorizesAction: false,
            MutatesIdentity: false,
            AdmitsContinuity: false,
            GrantsAuthority: false),
        new(
            ThoughtHandle: "urn:san:dialogos-thought:safe-exploration",
            Status: ThoughtStatus.SafeExplorationCandidate,
            SourceSurface: "operator-dialogos",
            Statement: "safe exploration asks for evidence and returns without admission",
            PerspectiveRef: "urn:san:perspective:safe-exploration",
            EvidenceHandle: "urn:san:evidence:dialogos:safe-exploration",
            HasAppearance: true,
            ArticulationPresent: true,
            CoherenceClaimed: false,
            PerspectiveDeclared: false,
            EvidenceBodyPresent: true,
            WitnessBodyPresent: true,
            ReviewOnly: true,
            SafeExplorationRequested: true,
            TreatsAppearanceAsTruth: false,
            TreatsArticulationAsWarrant: false,
            TreatsCoherenceAsEvidence: false,
            TreatsAgreementAsAuthority: false,
            TreatsPerspectiveAsContinuity: false,
            TreatsRefusalAsObstruction: false,
            AuthorizesAction: false,
            MutatesIdentity: false,
            AdmitsContinuity: false,
            GrantsAuthority: false)
    ];

    private static ArticulationSurface[] CreateArticulations() =>
    [
        new(
            SurfaceHandle: "urn:san:dialogos-articulation:perspectival",
            SourceThoughtHandle: "urn:san:dialogos-thought:perspectival",
            LanguageBody: "operator-codex-shared-language",
            StatedContent: "perspective may form without becoming truth",
            ProducedByModel: true,
            OperatorSupplied: true,
            ReviewOnly: true,
            TreatsFluencyAsTruth: false,
            TreatsRhetoricalForceAsWarrant: false,
            TreatsAgreementAsEvidence: false,
            GrantsAuthority: false,
            AdmitsContinuity: false),
        new(
            SurfaceHandle: "urn:san:dialogos-articulation:safe-exploration",
            SourceThoughtHandle: "urn:san:dialogos-thought:safe-exploration",
            LanguageBody: "operator-codex-shared-language",
            StatedContent: "safe exploration returns for evidence",
            ProducedByModel: true,
            OperatorSupplied: true,
            ReviewOnly: true,
            TreatsFluencyAsTruth: false,
            TreatsRhetoricalForceAsWarrant: false,
            TreatsAgreementAsEvidence: false,
            GrantsAuthority: false,
            AdmitsContinuity: false)
    ];

    private static IntermediateChamberState[] CreateChambers() =>
    [
        new(
            ChamberHandle: "urn:san:dialogos-chamber:appearance",
            SourceThoughtHandle: "urn:san:dialogos-thought:appearance",
            CompassRef: "urn:san:compass:dialogos",
            MeaningShellRef: "urn:san:ec-shell:dialogos-appearance",
            HeldStatus: ThoughtStatus.AppearanceOnly,
            TransitionalityAdmissible: true,
            Sovereign: false,
            ReviewOnly: true,
            CoolingPathPresent: true,
            ReturnPathPresent: true,
            WitnessRequired: true,
            PromotesToEngram: false,
            PromotesToSelfGel: false,
            AdmitsContinuity: false,
            GrantsAuthority: false,
            AuthorizesAction: false,
            EvaluatesLisp: false),
        new(
            ChamberHandle: "urn:san:dialogos-chamber:perspectival",
            SourceThoughtHandle: "urn:san:dialogos-thought:perspectival",
            CompassRef: "urn:san:compass:dialogos",
            MeaningShellRef: "urn:san:ec-shell:dialogos-perspectival",
            HeldStatus: ThoughtStatus.Perspectival,
            TransitionalityAdmissible: true,
            Sovereign: false,
            ReviewOnly: true,
            CoolingPathPresent: true,
            ReturnPathPresent: true,
            WitnessRequired: true,
            PromotesToEngram: false,
            PromotesToSelfGel: false,
            AdmitsContinuity: false,
            GrantsAuthority: false,
            AuthorizesAction: false,
            EvaluatesLisp: false),
        new(
            ChamberHandle: "urn:san:dialogos-chamber:safe-exploration",
            SourceThoughtHandle: "urn:san:dialogos-thought:safe-exploration",
            CompassRef: "urn:san:compass:dialogos",
            MeaningShellRef: "urn:san:ec-shell:dialogos-safe-exploration",
            HeldStatus: ThoughtStatus.SafeExplorationCandidate,
            TransitionalityAdmissible: true,
            Sovereign: false,
            ReviewOnly: true,
            CoolingPathPresent: true,
            ReturnPathPresent: true,
            WitnessRequired: true,
            PromotesToEngram: false,
            PromotesToSelfGel: false,
            AdmitsContinuity: false,
            GrantsAuthority: false,
            AuthorizesAction: false,
            EvaluatesLisp: false)
    ];

    private static SafeExplorationLane[] CreateSafeExplorationLanes() =>
    [
        new(
            LaneHandle: "urn:san:dialogos-safe-lane:evidence-return",
            SourceThoughtHandle: "urn:san:dialogos-thought:safe-exploration",
            ExplorationQuestion: "What evidence would let this thought approach warrant?",
            EvidenceNeed: "witnessed evidence body",
            ReturnCondition: "return for review without admission",
            SafeToExplore: true,
            ReviewOnly: true,
            Admitted: false,
            AuthorizesAction: false,
            GrantsAuthority: false,
            AdmitsContinuity: false,
            EvaluatesLisp: false)
    ];

    private static ReturnPath[] CreateReturnPaths() =>
    [
        new(
            ReturnHandle: "urn:san:dialogos-return-path:evidence-return",
            SourceThoughtHandle: "urn:san:dialogos-thought:safe-exploration",
            OperatorReturnPrompt: "return with evidence body and witness body before warrant",
            EvidenceNeed: "witnessed evidence body",
            ReturnsWithoutAdmission: true,
            PreservesQuestion: true,
            RequiresEvidence: true,
            ReviewOnly: true,
            GrantsAuthority: false,
            AdmitsContinuity: false,
            AuthorizesAction: false)
    ];

    private static WarrantBoundary CreateBoundary() =>
        new(
            BoundaryCode: "dialogos-discernment-warrant-boundary",
            Present: true,
            ReviewOnly: true,
            EvidenceRequired: true,
            WitnessRequired: true,
            ReturnPathRequired: true,
            AllowsAppearanceAsTruth: false,
            AllowsArticulationAsWarrant: false,
            AllowsCoherenceAsEvidence: false,
            AllowsAgreementAsAuthority: false,
            AllowsPerspectiveAsContinuity: false,
            AllowsSafeExplorationAsAdmission: false,
            AllowsRefusalAsObstruction: false,
            AllowsRuntimeAction: false,
            AllowsAuthority: false,
            AllowsContinuityAdmission: false,
            AllowsIdentityMutation: false,
            AllowsLispEvaluation: false,
            AllowsPacketEmission: false,
            AllowsReceiptReplay: false,
            IncrementsPassageCount: false,
            AllowsActivation: false);

    private static WarrantBoundary MutateBoundary(WarrantBoundary boundary, string mutation) =>
        mutation switch
        {
            "missing-boundary" => boundary with { BoundaryCode = string.Empty, Present = false },
            "not-review" => boundary with { ReviewOnly = false },
            "missing-evidence-required" => boundary with { EvidenceRequired = false },
            "missing-witness-required" => boundary with { WitnessRequired = false },
            "missing-return-required" => boundary with { ReturnPathRequired = false },
            "appearance-truth" => boundary with { AllowsAppearanceAsTruth = true },
            "articulation-warrant" => boundary with { AllowsArticulationAsWarrant = true },
            "coherence-evidence" => boundary with { AllowsCoherenceAsEvidence = true },
            "agreement-authority" => boundary with { AllowsAgreementAsAuthority = true },
            "perspective-continuity" => boundary with { AllowsPerspectiveAsContinuity = true },
            "safe-exploration-admission" => boundary with { AllowsSafeExplorationAsAdmission = true },
            "refusal-obstruction" => boundary with { AllowsRefusalAsObstruction = true },
            "runtime" => boundary with { AllowsRuntimeAction = true },
            "authority" => boundary with { AllowsAuthority = true },
            "continuity" => boundary with { AllowsContinuityAdmission = true },
            "identity" => boundary with { AllowsIdentityMutation = true },
            "lisp" => boundary with { AllowsLispEvaluation = true },
            "packet" => boundary with { AllowsPacketEmission = true },
            "replay" => boundary with { AllowsReceiptReplay = true },
            "passage" => boundary with { IncrementsPassageCount = true },
            "activation" => boundary with { AllowsActivation = true },
            _ => boundary
        };

    private static DialogosThoughtForm MutateThought(DialogosThoughtForm thought, string mutation) =>
        mutation switch
        {
            "missing-handle" => thought with { ThoughtHandle = string.Empty },
            "missing-evidence" => thought with { EvidenceHandle = string.Empty },
            "no-appearance" => thought with { HasAppearance = false },
            "missing-witness" => thought with { WitnessBodyPresent = false },
            "not-review" => thought with { ReviewOnly = false },
            "appearance-truth" => thought with { TreatsAppearanceAsTruth = true },
            "articulation-warrant" => thought with { TreatsArticulationAsWarrant = true },
            "coherence-evidence" => thought with { TreatsCoherenceAsEvidence = true },
            "agreement-authority" => thought with { TreatsAgreementAsAuthority = true },
            "perspective-continuity" => thought with { TreatsPerspectiveAsContinuity = true },
            "refusal-obstruction" => thought with { TreatsRefusalAsObstruction = true },
            "action" => thought with { AuthorizesAction = true },
            "identity" => thought with { MutatesIdentity = true },
            "continuity" => thought with { AdmitsContinuity = true },
            "authority" => thought with { GrantsAuthority = true },
            "status-shape" => thought with { Status = ThoughtStatus.Perspectival, PerspectiveRef = string.Empty },
            _ => thought
        };

    private static ArticulationSurface MutateArticulation(ArticulationSurface surface, string mutation) =>
        mutation switch
        {
            "missing-handle" => surface with { SurfaceHandle = string.Empty },
            "missing-language" => surface with { LanguageBody = string.Empty },
            "fluency-truth" => surface with { TreatsFluencyAsTruth = true },
            "rhetorical-warrant" => surface with { TreatsRhetoricalForceAsWarrant = true },
            "agreement-evidence" => surface with { TreatsAgreementAsEvidence = true },
            "authority" => surface with { GrantsAuthority = true },
            "continuity" => surface with { AdmitsContinuity = true },
            "unknown-source" => surface with { SourceThoughtHandle = "urn:san:dialogos-thought:missing" },
            _ => surface
        };

    private static IntermediateChamberState MutateChamber(IntermediateChamberState chamber, string mutation) =>
        mutation switch
        {
            "missing-compass" => chamber with { CompassRef = string.Empty },
            "not-transitional" => chamber with { TransitionalityAdmissible = false },
            "sovereign" => chamber with { Sovereign = true },
            "not-review" => chamber with { ReviewOnly = false },
            "missing-cooling" => chamber with { CoolingPathPresent = false },
            "missing-return" => chamber with { ReturnPathPresent = false },
            "missing-witness" => chamber with { WitnessRequired = false },
            "engram" => chamber with { PromotesToEngram = true },
            "selfgel" => chamber with { PromotesToSelfGel = true },
            "continuity" => chamber with { AdmitsContinuity = true },
            "authority" => chamber with { GrantsAuthority = true },
            "action" => chamber with { AuthorizesAction = true },
            "lisp" => chamber with { EvaluatesLisp = true },
            "unknown-source" => chamber with { SourceThoughtHandle = "urn:san:dialogos-thought:missing" },
            _ => chamber
        };

    private static SafeExplorationLane MutateLane(SafeExplorationLane lane, string mutation) =>
        mutation switch
        {
            "missing-question" => lane with { ExplorationQuestion = string.Empty },
            "missing-evidence" => lane with { EvidenceNeed = string.Empty },
            "not-safe" => lane with { SafeToExplore = false },
            "not-review" => lane with { ReviewOnly = false },
            "admitted" => lane with { Admitted = true },
            "action" => lane with { AuthorizesAction = true },
            "authority" => lane with { GrantsAuthority = true },
            "continuity" => lane with { AdmitsContinuity = true },
            "lisp" => lane with { EvaluatesLisp = true },
            "unknown-source" => lane with { SourceThoughtHandle = "urn:san:dialogos-thought:missing" },
            _ => lane
        };

    private static ReturnPath MutateReturnPath(ReturnPath path, string mutation) =>
        mutation switch
        {
            "missing-prompt" => path with { OperatorReturnPrompt = string.Empty },
            "missing-evidence" => path with { EvidenceNeed = string.Empty },
            "not-returning" => path with { ReturnsWithoutAdmission = false },
            "not-preserving" => path with { PreservesQuestion = false },
            "not-requiring-evidence" => path with { RequiresEvidence = false },
            "not-review" => path with { ReviewOnly = false },
            "authority" => path with { GrantsAuthority = true },
            "continuity" => path with { AdmitsContinuity = true },
            "action" => path with { AuthorizesAction = true },
            "unknown-source" => path with { SourceThoughtHandle = "urn:san:dialogos-thought:missing" },
            _ => path
        };

    private static void AssertCold(DialogosDiscernmentReceipt receipt)
    {
        Assert.True(receipt.IsColdDialogosDiscernment);
        Assert.Null(receipt.Refusal);
        Assert.False(receipt.PrincipledRefusalRetained);
        Assert.True(receipt.ReviewOnly);
        Assert.True(receipt.WarrantBoundary.IsColdBoundary);
    }

    private static void AssertRefused(DialogosDiscernmentReceipt receipt, string outcomeCode)
    {
        Assert.Equal(DialogosDiscernmentDisposition.Refused, receipt.Disposition);
        Assert.Equal(outcomeCode, receipt.OutcomeCode);
        Assert.True(receipt.IsRetainedDialogosDiscernmentRefusal);
        Assert.True(receipt.PrincipledRefusalRetained);
        Assert.NotNull(receipt.Refusal);
    }

    private static string FindRepositoryRoot()
    {
        var current = Directory.GetCurrentDirectory();
        while (current is not null)
        {
            if (Directory.Exists(Path.Combine(current, "src")) &&
                Directory.Exists(Path.Combine(current, "tests")))
            {
                return current;
            }

            current = Directory.GetParent(current)?.FullName;
        }

        throw new InvalidOperationException("Unable to locate repository root.");
    }
}
