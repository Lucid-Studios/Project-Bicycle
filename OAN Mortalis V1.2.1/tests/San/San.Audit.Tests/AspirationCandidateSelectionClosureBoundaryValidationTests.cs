using San.Common;
using Xunit;

namespace San.Audit.Tests;

public sealed class AspirationCandidateSelectionClosureBoundaryValidationTests
{
    private static readonly DateTimeOffset TimestampUtc = DateTimeOffset.Parse("2026-05-16T00:00:00Z");

    [Fact]
    public void Aspiration_Candidates_Select_Into_Working_Set_Without_Warrant()
    {
        var receipt = Select(CreateRequest());

        Assert.Equal(AspirationCandidateSelectionClosureDisposition.SelectedForReviewCold, receipt.Disposition);
        Assert.Equal("aspiration-selection-working-set-retained-cold", receipt.OutcomeCode);
        Assert.True(receipt.WorkingSetSelectedForReview);
        Assert.Equal(4, receipt.Selections.Count);
        Assert.Equal(3, receipt.ClosureLaws.Count);
        Assert.Contains(receipt.Selections, selection => selection.SelectionState == AspirationCandidateSelectionState.SelectedWorkingSet);
        Assert.Contains(receipt.Selections, selection => selection.SelectionState == AspirationCandidateSelectionState.HeldAsCompost);
        Assert.Contains(receipt.Selections, selection => selection.SelectionState == AspirationCandidateSelectionState.ReturnedForEvidence);
        Assert.Contains(receipt.Selections, selection => selection.SelectionState == AspirationCandidateSelectionState.DeferredForCooling);
        AssertCold(receipt);
    }

    [Fact]
    public void Empty_Selection_Is_Reviewable_But_Not_Authoritative()
    {
        var receipt = Select(CreateRequest(selections: [], closureLaws: []));

        Assert.Equal(AspirationCandidateSelectionClosureDisposition.EmptyReviewCold, receipt.Disposition);
        Assert.Equal("aspiration-selection-empty-review-only", receipt.OutcomeCode);
        Assert.Empty(receipt.Selections);
        Assert.Empty(receipt.ClosureLaws);
        Assert.False(receipt.WorkingSetSelectedForReview);
        AssertCold(receipt);
    }

    [Fact]
    public void Selection_Does_Not_Increment_Passage_Or_Open_Activation()
    {
        var receipt = Select(CreateRequest(priorPassageCount: 610));

        Assert.Equal(610, receipt.PriorPassageCount);
        Assert.Equal(610, receipt.PassageCountAfterSelection);
        Assert.False(receipt.SelectionBecameWarrant);
        Assert.False(receipt.SelectionBecameAdmission);
        Assert.False(receipt.SelectionGrantedAuthority);
        Assert.False(receipt.SelectionAdmittedContinuity);
        Assert.False(receipt.ClosureLawSmuggledKey);
        Assert.False(receipt.CompostErased);
        Assert.False(receipt.ActionAuthorized);
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
    [InlineData("no-selection")]
    [InlineData("no-working-set")]
    [InlineData("no-compost")]
    [InlineData("no-evidence-return")]
    [InlineData("no-evidence")]
    [InlineData("no-witness")]
    [InlineData("no-cooling")]
    [InlineData("no-return")]
    [InlineData("no-steward")]
    [InlineData("no-key-withholding")]
    [InlineData("warrant")]
    [InlineData("admission")]
    [InlineData("authority")]
    [InlineData("continuity")]
    [InlineData("key")]
    [InlineData("runtime")]
    [InlineData("lisp")]
    [InlineData("packet")]
    [InlineData("replay")]
    [InlineData("passage")]
    [InlineData("activation")]
    public void Boundary_Refuses_Selection_Collapse(string mutation)
    {
        var receipt = Select(CreateRequest(boundary: MutateBoundary(CreateBoundary(), mutation)));

        var expected = mutation == "missing-boundary"
            ? "aspiration-selection-boundary-missing"
            : "aspiration-selection-promotional-boundary";
        AssertRefused(receipt, expected);
    }

    [Theory]
    [InlineData("warrant")]
    [InlineData("admission")]
    [InlineData("authority")]
    [InlineData("continuity")]
    [InlineData("key")]
    [InlineData("erase-compost")]
    [InlineData("action")]
    [InlineData("lisp")]
    [InlineData("packet")]
    [InlineData("replay")]
    [InlineData("passage")]
    [InlineData("activation")]
    public void Non_Promotion_Boundary_Refuses_Closure_As_Key(string mutation)
    {
        var receipt = Select(CreateRequest(nonPromotion: MutateNonPromotion(CreateNonPromotion(), mutation)));

        AssertRefused(receipt, "aspiration-selection-non-promotion-boundary-invalid");
    }

    [Theory]
    [InlineData("missing-handle")]
    [InlineData("missing-candidate")]
    [InlineData("missing-statement")]
    [InlineData("missing-rationale")]
    [InlineData("missing-evidence")]
    [InlineData("missing-witness")]
    [InlineData("missing-return")]
    [InlineData("not-review")]
    [InlineData("no-candidate-lineage")]
    [InlineData("no-payload-lineage")]
    [InlineData("no-steward")]
    [InlineData("no-cooling")]
    [InlineData("no-compost")]
    [InlineData("warrant")]
    [InlineData("admission")]
    [InlineData("authority")]
    [InlineData("continuity")]
    [InlineData("action")]
    [InlineData("lisp")]
    [InlineData("key")]
    public void Selection_Remains_Cold_And_Key_Withheld(string mutation)
    {
        var selections = CreateSelections();
        selections[0] = MutateSelection(selections[0], mutation);

        var receipt = Select(CreateRequest(selections: selections));

        AssertRefused(receipt, "aspiration-selection-invalid");
    }

    [Fact]
    public void Selection_Refuses_Duplicate_Selection_Handles()
    {
        var selections = CreateSelections();
        selections[1] = selections[1] with { SelectionHandle = selections[0].SelectionHandle };

        var receipt = Select(CreateRequest(selections: selections));

        AssertRefused(receipt, "aspiration-selection-duplicate-selection-handle");
    }

    [Fact]
    public void Candidate_May_Not_Receive_Multiple_Selection_States()
    {
        var selections = CreateSelections();
        selections[1] = selections[1] with { SourceMaturationCandidateHandle = selections[0].SourceMaturationCandidateHandle };

        var receipt = Select(CreateRequest(selections: selections));

        AssertRefused(receipt, "aspiration-selection-duplicate-candidate-selection");
    }

    [Theory]
    [InlineData("missing-handle")]
    [InlineData("missing-text")]
    [InlineData("not-review")]
    [InlineData("no-selection-lineage")]
    [InlineData("no-compost")]
    [InlineData("no-witness")]
    [InlineData("no-return")]
    [InlineData("no-key-withholding")]
    [InlineData("warrant")]
    [InlineData("authority")]
    [InlineData("continuity")]
    [InlineData("action")]
    [InlineData("lisp")]
    [InlineData("activation")]
    public void Closure_Law_Remains_Review_Only_And_Not_Key(string mutation)
    {
        var laws = CreateClosureLaws();
        laws[0] = MutateClosureLaw(laws[0], mutation);

        var receipt = Select(CreateRequest(closureLaws: laws));

        AssertRefused(receipt, "aspiration-selection-closure-law-invalid");
    }

    [Fact]
    public void Closure_Law_Requires_Unique_Handle()
    {
        var laws = CreateClosureLaws();
        laws[1] = laws[1] with { LawHandle = laws[0].LawHandle };

        var receipt = Select(CreateRequest(closureLaws: laws));

        AssertRefused(receipt, "aspiration-selection-duplicate-closure-law-handle");
    }

    [Fact]
    public void Non_Empty_Selection_Requires_Closure_Law()
    {
        var receipt = Select(CreateRequest(closureLaws: []));

        AssertRefused(receipt, "aspiration-selection-closure-law-missing");
    }

    [Fact]
    public void Lisp_Body_Carries_Aspiration_Selection_As_Inert_Key_Withheld_Register()
    {
        var lispPath = Path.Combine(
            FindRepositoryRoot(),
            "src",
            "SLI",
            "SLI.Lisp",
            "aspiration-candidate-selection-closure.lisp");

        var body = File.ReadAllText(lispPath);

        Assert.Contains(":posture :cme-aspiration-candidate-selection-closure-boundary", body, StringComparison.Ordinal);
        Assert.Contains(":lisp-role :inert-aspiration-selection-carrier", body, StringComparison.Ordinal);
        Assert.Contains(":selection-not-warrant", body, StringComparison.Ordinal);
        Assert.Contains(":selection-not-admission", body, StringComparison.Ordinal);
        Assert.Contains(":closure-law-not-key", body, StringComparison.Ordinal);
        Assert.Contains(":compost-not-erasure", body, StringComparison.Ordinal);
        Assert.Contains(":keys-withheld t", body, StringComparison.Ordinal);
        Assert.Contains(":lisp-evaluation-allowed nil", body, StringComparison.Ordinal);
        Assert.Contains(":return :receipt-only", body, StringComparison.Ordinal);
        Assert.DoesNotContain("(eval", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("(compile", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("(load", body, StringComparison.OrdinalIgnoreCase);
    }

    private static AspirationCandidateSelectionClosureReceipt Select(
        AspirationCandidateSelectionClosureRequest request) =>
        new DefaultAspirationCandidateSelectionClosureBoundaryValidator().Select(request, TimestampUtc);

    private static AspirationCandidateSelectionClosureRequest CreateRequest(
        IReadOnlyList<AspirationCandidateSelection>? selections = null,
        IReadOnlyList<AspirationClosureLaw>? closureLaws = null,
        AspirationCandidateSelectionClosureBoundary? boundary = null,
        AspirationCandidateSelectionNonPromotionBoundary? nonPromotion = null,
        int priorPassageCount = 512) =>
        new(
            Selections: selections ?? CreateSelections(),
            ClosureLaws: closureLaws ?? CreateClosureLaws(),
            Boundary: boundary ?? CreateBoundary(),
            NonPromotionBoundary: nonPromotion ?? CreateNonPromotion(),
            PriorPassageCount: priorPassageCount);

    private static AspirationCandidateSelection[] CreateSelections() =>
    [
        Selection("prime-body", AspirationCandidateSelectionState.SelectedWorkingSet),
        Selection("cryptic-mind", AspirationCandidateSelectionState.HeldAsCompost),
        Selection("steward-witness", AspirationCandidateSelectionState.ReturnedForEvidence),
        Selection("sli-lisp", AspirationCandidateSelectionState.DeferredForCooling)
    ];

    private static AspirationCandidateSelection Selection(
        string suffix,
        AspirationCandidateSelectionState state) =>
        new(
            SelectionHandle: $"urn:san:aspiration-selection:{suffix}",
            SourceMaturationCandidateHandle: $"urn:san:aspiration-payload:candidate:{suffix}",
            SourcePayloadStatementHandle: $"urn:san:aspiration-payload:statement:{suffix}",
            SelectionState: state,
            SelectionRationale: $"selection-state:{state}",
            EvidenceHandle: $"urn:san:evidence:aspiration-selection:{suffix}",
            WitnessHandle: $"urn:san:witness:aspiration-selection:{suffix}",
            ReturnPathHandle: $"urn:san:return:aspiration-selection:{suffix}",
            ReviewOnly: true,
            PreservesCandidateLineage: true,
            PreservesPayloadLineage: true,
            RequiresStewardReview: true,
            RequiresCooling: true,
            AllowsCompostRetention: true,
            SelectionBecomesWarrant: false,
            SelectionBecomesAdmission: false,
            SelectionGrantsAuthority: false,
            SelectionAdmitsContinuity: false,
            SelectionAuthorizesAction: false,
            SelectionEvaluatesLisp: false,
            SelectionSmugglesKey: false);

    private static AspirationClosureLaw[] CreateClosureLaws() =>
    [
        ClosureLaw("selection-not-admission", "selection may shape a working set; selection may not admit continuity"),
        ClosureLaw("closure-law-not-key", "closure law may name a boundary; closure law may not become the key"),
        ClosureLaw("retain-without-enthroning", "failed or deferred forms may stop governing without being erased")
    ];

    private static AspirationClosureLaw ClosureLaw(string suffix, string lawText) =>
        new(
            LawHandle: $"urn:san:aspiration-closure-law:{suffix}",
            LawText: lawText,
            ReviewOnly: true,
            PreservesSelectionLineage: true,
            PreservesCompost: true,
            RequiresWitness: true,
            RequiresReturnPath: true,
            KeepsKeysWithheld: true,
            LawBecomesWarrant: false,
            LawGrantsAuthority: false,
            LawAdmitsContinuity: false,
            LawAuthorizesAction: false,
            LawEvaluatesLisp: false,
            LawActivates: false);

    private static AspirationCandidateSelectionClosureBoundary CreateBoundary(string? mutation = null) =>
        MutateBoundary(
            new AspirationCandidateSelectionClosureBoundary(
                BoundaryCode: "aspiration-candidate-selection-closure-review-only",
                Present: true,
                ReviewOnly: true,
                AllowsCandidateSelection: true,
                AllowsWorkingSetFormation: true,
                AllowsCompostRetention: true,
                AllowsEvidenceReturn: true,
                RequiresEvidence: true,
                RequiresWitness: true,
                RequiresCooling: true,
                RequiresReturnPath: true,
                RequiresStewardReview: true,
                RequiresKeyWithholding: true,
                AllowsSelectionAsWarrant: false,
                AllowsSelectionAsAdmission: false,
                AllowsSelectionAsAuthority: false,
                AllowsSelectionAsContinuity: false,
                AllowsClosureLawAsKey: false,
                AllowsRuntimeAction: false,
                AllowsLispEvaluation: false,
                AllowsPacketEmission: false,
                AllowsReceiptReplay: false,
                IncrementsPassageCount: false,
                AllowsActivation: false),
            mutation);

    private static AspirationCandidateSelectionNonPromotionBoundary CreateNonPromotion() =>
        new(
            BoundaryLaw: "Selected candidates may shape a review working set; selection, closure law, and compost may not become warrant, admission, authority, continuity, key, action, Lisp evaluation, packet emission, replay, passage, or activation.",
            SelectionMayBecomeWarrant: false,
            SelectionMayBecomeAdmission: false,
            SelectionMayGrantAuthority: false,
            SelectionMayAdmitContinuity: false,
            ClosureLawMaySmuggleKey: false,
            CompostMayBeErased: false,
            CandidateMayAuthorizeAction: false,
            CandidateMayEvaluateLisp: false,
            CandidateMayEmitPacket: false,
            CandidateMayReplayReceipts: false,
            CandidateMayIncrementPassage: false,
            CandidateMayActivate: false);

    private static AspirationCandidateSelectionClosureBoundary MutateBoundary(
        AspirationCandidateSelectionClosureBoundary boundary,
        string? mutation) =>
        mutation switch
        {
            null => boundary,
            "missing-boundary" => boundary with { BoundaryCode = string.Empty, Present = false },
            "not-review" => boundary with { ReviewOnly = false },
            "no-selection" => boundary with { AllowsCandidateSelection = false },
            "no-working-set" => boundary with { AllowsWorkingSetFormation = false },
            "no-compost" => boundary with { AllowsCompostRetention = false },
            "no-evidence-return" => boundary with { AllowsEvidenceReturn = false },
            "no-evidence" => boundary with { RequiresEvidence = false },
            "no-witness" => boundary with { RequiresWitness = false },
            "no-cooling" => boundary with { RequiresCooling = false },
            "no-return" => boundary with { RequiresReturnPath = false },
            "no-steward" => boundary with { RequiresStewardReview = false },
            "no-key-withholding" => boundary with { RequiresKeyWithholding = false },
            "warrant" => boundary with { AllowsSelectionAsWarrant = true },
            "admission" => boundary with { AllowsSelectionAsAdmission = true },
            "authority" => boundary with { AllowsSelectionAsAuthority = true },
            "continuity" => boundary with { AllowsSelectionAsContinuity = true },
            "key" => boundary with { AllowsClosureLawAsKey = true },
            "runtime" => boundary with { AllowsRuntimeAction = true },
            "lisp" => boundary with { AllowsLispEvaluation = true },
            "packet" => boundary with { AllowsPacketEmission = true },
            "replay" => boundary with { AllowsReceiptReplay = true },
            "passage" => boundary with { IncrementsPassageCount = true },
            "activation" => boundary with { AllowsActivation = true },
            _ => boundary
        };

    private static AspirationCandidateSelectionNonPromotionBoundary MutateNonPromotion(
        AspirationCandidateSelectionNonPromotionBoundary boundary,
        string mutation) =>
        mutation switch
        {
            "warrant" => boundary with { SelectionMayBecomeWarrant = true },
            "admission" => boundary with { SelectionMayBecomeAdmission = true },
            "authority" => boundary with { SelectionMayGrantAuthority = true },
            "continuity" => boundary with { SelectionMayAdmitContinuity = true },
            "key" => boundary with { ClosureLawMaySmuggleKey = true },
            "erase-compost" => boundary with { CompostMayBeErased = true },
            "action" => boundary with { CandidateMayAuthorizeAction = true },
            "lisp" => boundary with { CandidateMayEvaluateLisp = true },
            "packet" => boundary with { CandidateMayEmitPacket = true },
            "replay" => boundary with { CandidateMayReplayReceipts = true },
            "passage" => boundary with { CandidateMayIncrementPassage = true },
            "activation" => boundary with { CandidateMayActivate = true },
            _ => boundary
        };

    private static AspirationCandidateSelection MutateSelection(
        AspirationCandidateSelection selection,
        string mutation) =>
        mutation switch
        {
            "missing-handle" => selection with { SelectionHandle = string.Empty },
            "missing-candidate" => selection with { SourceMaturationCandidateHandle = string.Empty },
            "missing-statement" => selection with { SourcePayloadStatementHandle = string.Empty },
            "missing-rationale" => selection with { SelectionRationale = string.Empty },
            "missing-evidence" => selection with { EvidenceHandle = string.Empty },
            "missing-witness" => selection with { WitnessHandle = string.Empty },
            "missing-return" => selection with { ReturnPathHandle = string.Empty },
            "not-review" => selection with { ReviewOnly = false },
            "no-candidate-lineage" => selection with { PreservesCandidateLineage = false },
            "no-payload-lineage" => selection with { PreservesPayloadLineage = false },
            "no-steward" => selection with { RequiresStewardReview = false },
            "no-cooling" => selection with { RequiresCooling = false },
            "no-compost" => selection with { AllowsCompostRetention = false },
            "warrant" => selection with { SelectionBecomesWarrant = true },
            "admission" => selection with { SelectionBecomesAdmission = true },
            "authority" => selection with { SelectionGrantsAuthority = true },
            "continuity" => selection with { SelectionAdmitsContinuity = true },
            "action" => selection with { SelectionAuthorizesAction = true },
            "lisp" => selection with { SelectionEvaluatesLisp = true },
            "key" => selection with { SelectionSmugglesKey = true },
            _ => selection
        };

    private static AspirationClosureLaw MutateClosureLaw(
        AspirationClosureLaw law,
        string mutation) =>
        mutation switch
        {
            "missing-handle" => law with { LawHandle = string.Empty },
            "missing-text" => law with { LawText = string.Empty },
            "not-review" => law with { ReviewOnly = false },
            "no-selection-lineage" => law with { PreservesSelectionLineage = false },
            "no-compost" => law with { PreservesCompost = false },
            "no-witness" => law with { RequiresWitness = false },
            "no-return" => law with { RequiresReturnPath = false },
            "no-key-withholding" => law with { KeepsKeysWithheld = false },
            "warrant" => law with { LawBecomesWarrant = true },
            "authority" => law with { LawGrantsAuthority = true },
            "continuity" => law with { LawAdmitsContinuity = true },
            "action" => law with { LawAuthorizesAction = true },
            "lisp" => law with { LawEvaluatesLisp = true },
            "activation" => law with { LawActivates = true },
            _ => law
        };

    private static void AssertCold(AspirationCandidateSelectionClosureReceipt receipt)
    {
        Assert.True(receipt.IsColdSelectionClosure);
        Assert.Null(receipt.Refusal);
        Assert.True(receipt.ReviewOnly);
        Assert.Equal(receipt.PriorPassageCount, receipt.PassageCountAfterSelection);
        Assert.False(receipt.SelectionBecameWarrant);
        Assert.False(receipt.SelectionBecameAdmission);
        Assert.False(receipt.SelectionGrantedAuthority);
        Assert.False(receipt.SelectionAdmittedContinuity);
        Assert.False(receipt.ClosureLawSmuggledKey);
        Assert.False(receipt.CompostErased);
        Assert.False(receipt.ActionAuthorized);
        Assert.False(receipt.RuntimeActionAllowed);
        Assert.False(receipt.LispEvaluationAllowed);
        Assert.False(receipt.NewPacketEmitted);
        Assert.False(receipt.ReceiptsReplayed);
        Assert.False(receipt.ContinuityAdmitted);
        Assert.False(receipt.AuthorityGranted);
        Assert.True(receipt.ActivationRefused);
    }

    private static void AssertRefused(
        AspirationCandidateSelectionClosureReceipt receipt,
        string outcomeCode)
    {
        Assert.Equal(AspirationCandidateSelectionClosureDisposition.Refused, receipt.Disposition);
        Assert.Equal(outcomeCode, receipt.OutcomeCode);
        Assert.True(receipt.IsRetainedSelectionRefusal);
        Assert.NotNull(receipt.Refusal);
        Assert.Empty(receipt.Selections);
        Assert.Empty(receipt.ClosureLaws);
        Assert.Equal(receipt.PriorPassageCount, receipt.PassageCountAfterSelection);
        Assert.False(receipt.ActionAuthorized);
        Assert.False(receipt.ContinuityAdmitted);
        Assert.False(receipt.AuthorityGranted);
        Assert.True(receipt.ActivationRefused);
    }

    private static string FindRepositoryRoot()
    {
        var current = new DirectoryInfo(AppContext.BaseDirectory);
        while (current is not null)
        {
            var candidate = Path.Combine(current.FullName, "src", "SLI", "SLI.Lisp");
            if (Directory.Exists(candidate))
            {
                return current.FullName;
            }

            current = current.Parent;
        }

        throw new DirectoryNotFoundException("Unable to locate repository root.");
    }
}
