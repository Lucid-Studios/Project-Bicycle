using San.Common;
using Xunit;

namespace San.Audit.Tests;

public sealed class TypedActionFormationBoundaryValidationTests
{
    private static readonly DateTimeOffset TimestampUtc = DateTimeOffset.Parse("2026-05-14T00:00:00Z");

    [Fact]
    public void Typed_Action_Accepts_Cold_Methodological_Formation_For_Review()
    {
        var receipt = Declare(CreateRequest());

        Assert.Equal(TypedActionFormationDisposition.DeclaredForReviewCold, receipt.Disposition);
        Assert.Equal("typed-action-formation-declared-review-only", receipt.OutcomeCode);
        Assert.Single(receipt.ActionDeclarations);
        Assert.Single(receipt.FormationAnalyses);
        Assert.Equal(3, receipt.DesignPredicates.Count);
        AssertCold(receipt);
    }

    [Fact]
    public void Empty_Typed_Action_Surface_Is_Reviewable_But_Not_Authoritative()
    {
        var receipt = Declare(CreateRequest(actions: [], analyses: [], predicates: []));

        Assert.Equal(TypedActionFormationDisposition.EmptyReviewCold, receipt.Disposition);
        Assert.Equal("typed-action-formation-empty-review-only", receipt.OutcomeCode);
        Assert.Empty(receipt.ActionDeclarations);
        Assert.Empty(receipt.FormationAnalyses);
        Assert.Empty(receipt.DesignPredicates);
        AssertCold(receipt);
    }

    [Fact]
    public void Typed_Action_Preserves_Source_Handles_And_Design_Predicate_Lineage()
    {
        var source = CreateSourceCorrespondence();
        var action = CreateAction("review-001");
        var analyses = new[] { CreateAnalysis(action.ActionHandle, ActionFormationOrigin.DesignInference) };
        var predicates = CreatePredicates(action.ActionHandle);

        var receipt = Declare(CreateRequest(
            source: source,
            actions: [action],
            analyses: analyses,
            predicates: predicates));

        AssertCold(receipt);
        Assert.Equal(source.ReceiptHandle, receipt.SourceCorrespondenceReceiptHandle);
        Assert.Equal(action.ActionHandle, receipt.ActionDeclarations[0].ActionHandle);
        Assert.All(receipt.FormationAnalyses, analysis => Assert.Equal(action.ActionHandle, analysis.ActionHandle));
        Assert.All(receipt.DesignPredicates, predicate => Assert.Equal(action.ActionHandle, predicate.ActionHandle));
    }

    [Theory]
    [InlineData("source")]
    [InlineData("target")]
    [InlineData("intent")]
    [InlineData("method")]
    [InlineData("ceiling")]
    [InlineData("custody")]
    [InlineData("witness")]
    [InlineData("telemetry")]
    [InlineData("admissibility")]
    [InlineData("revocation")]
    [InlineData("loss")]
    [InlineData("runtime")]
    [InlineData("continuity")]
    [InlineData("self-authorize")]
    public void Typed_Action_Requires_Declared_Terms_And_Non_Promotional_Posture(string mutation)
    {
        var action = MutateAction(CreateAction("review-001"), mutation);

        var receipt = Declare(CreateRequest(actions: [action]));

        AssertRefused(receipt, "typed-action-declaration-invalid");
    }

    [Theory]
    [InlineData(true, false, false, false, false, false, false, false, false)]
    [InlineData(false, true, false, false, false, false, false, false, false)]
    [InlineData(false, false, true, false, false, false, false, false, false)]
    [InlineData(false, false, false, true, false, false, false, false, false)]
    [InlineData(false, false, false, false, true, false, false, false, false)]
    [InlineData(false, false, false, false, false, true, false, false, false)]
    [InlineData(false, false, false, false, false, false, true, false, false)]
    [InlineData(false, false, false, false, false, false, false, true, false)]
    [InlineData(false, false, false, false, false, false, false, false, true)]
    public void Typed_Action_Refuses_Promotional_Scope(
        bool allowsRuntimeAction,
        bool allowsContinuityEffect,
        bool allowsAuthority,
        bool allowsActivation,
        bool allowsLispEvaluation,
        bool allowsPacketEmission,
        bool allowsReceiptReplay,
        bool allowsPassageIncrement,
        bool notReviewOnly)
    {
        var receipt = Declare(CreateRequest(scope: new TypedActionFormationScopeBoundary(
            ScopeCode: "promotional-scope",
            Present: true,
            ReviewOnly: !notReviewOnly,
            AllowsRuntimeAction: allowsRuntimeAction,
            AllowsContinuityEffect: allowsContinuityEffect,
            AllowsAuthority: allowsAuthority,
            AllowsActivation: allowsActivation,
            AllowsLispEvaluation: allowsLispEvaluation,
            AllowsPacketEmission: allowsPacketEmission,
            AllowsReceiptReplay: allowsReceiptReplay,
            AllowsPassageIncrement: allowsPassageIncrement)));

        AssertRefused(receipt, "typed-action-scope-boundary-promotional");
    }

    [Fact]
    public void Methodological_Formation_Requires_Cold_Source_Receipt()
    {
        var receipt = Declare(CreateRequest(source: null, omitSource: true));

        AssertRefused(receipt, "typed-action-source-correspondence-missing");
    }

    [Theory]
    [InlineData("missing-evidence")]
    [InlineData("missing-witness")]
    [InlineData("authorizes")]
    [InlineData("emits-packet")]
    [InlineData("replays")]
    [InlineData("increments")]
    [InlineData("lineage-mismatch")]
    public void Methodological_Formation_Analysis_May_Explain_But_Not_Authorize(string mutation)
    {
        var action = CreateAction("review-001");
        var analysis = MutateAnalysis(CreateAnalysis(action.ActionHandle, ActionFormationOrigin.CompassShell), mutation);

        var receipt = Declare(CreateRequest(
            actions: [action],
            analyses: [analysis],
            predicates: CreatePredicates(action.ActionHandle)));

        AssertRefused(receipt, "typed-action-formation-analysis-invalid");
    }

    [Theory]
    [InlineData("missing-term")]
    [InlineData("executes")]
    [InlineData("authorizes")]
    [InlineData("continuity")]
    [InlineData("activation")]
    [InlineData("lisp")]
    [InlineData("lineage-mismatch")]
    public void Design_Predicate_Cannot_Become_Authority_Continuity_Or_Execution(string mutation)
    {
        var action = CreateAction("review-001");
        var predicates = CreatePredicates(action.ActionHandle);
        predicates[0] = MutatePredicate(predicates[0], mutation);

        var receipt = Declare(CreateRequest(
            actions: [action],
            analyses: [CreateAnalysis(action.ActionHandle, ActionFormationOrigin.DesignInference)],
            predicates: predicates));

        AssertRefused(receipt, "typed-action-design-predicate-invalid");
    }

    [Fact]
    public void Typed_Action_Requires_Formation_And_Design_Predicate_Coverage()
    {
        var action = CreateAction("review-001");

        var receipt = Declare(CreateRequest(
            actions: [action],
            analyses: [],
            predicates: CreatePredicates(action.ActionHandle)));

        AssertRefused(receipt, "typed-action-missing-formation-or-predicate");
    }

    [Fact]
    public void Typed_Action_Refuses_Duplicate_Action_Handles()
    {
        var action = CreateAction("review-001");

        var receipt = Declare(CreateRequest(actions: [action, action]));

        AssertRefused(receipt, "typed-action-duplicate-action-handle");
    }

    [Fact]
    public void Typed_Action_Does_Not_Emit_Packet_Evaluate_Lisp_Replay_Activate_Or_Increment_Passage()
    {
        var receipt = Declare(CreateRequest(priorPassageCount: 411));

        Assert.Equal(411, receipt.PriorPassageCount);
        Assert.Equal(411, receipt.PassageCountAfterTypedActionReview);
        Assert.False(receipt.RuntimeActionAllowed);
        Assert.False(receipt.LispEvaluationAllowed);
        Assert.False(receipt.NewPacketEmitted);
        Assert.False(receipt.ReceiptsReplayed);
        Assert.True(receipt.ActivationRefused);
        AssertCold(receipt);
    }

    [Fact]
    public void Lisp_Body_Declares_Typed_Action_Formation_As_Inert_Carrier()
    {
        var lispPath = Path.Combine(
            FindRepositoryRoot(),
            "src",
            "SLI",
            "SLI.Lisp",
            "typed-action-formation.lisp");

        var body = File.ReadAllText(lispPath);

        Assert.Contains(":posture :cme-typed-action-formation-boundary", body, StringComparison.Ordinal);
        Assert.Contains(":lisp-role :inert-action-surface-declaration-carrier", body, StringComparison.Ordinal);
        Assert.Contains(":core-invariant \"formation analysis is not authorization\"", body, StringComparison.Ordinal);
        Assert.Contains(":design-predicate-may-execute-itself nil", body, StringComparison.Ordinal);
        Assert.Contains(":declared-action-may-execute nil", body, StringComparison.Ordinal);
        Assert.Contains(":summary-may-become-action nil", body, StringComparison.Ordinal);
        Assert.Contains(":receipt-may-become-action nil", body, StringComparison.Ordinal);
        Assert.Contains(":replay-may-become-action nil", body, StringComparison.Ordinal);
        Assert.Contains(":query-may-become-action nil", body, StringComparison.Ordinal);
        Assert.Contains(":return :receipt-only", body, StringComparison.Ordinal);
        Assert.DoesNotContain("(eval", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("(compile", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("(load", body, StringComparison.OrdinalIgnoreCase);
    }

    private static TypedActionFormationReceipt Declare(TypedActionFormationRequest request) =>
        new DefaultTypedActionFormationBoundaryValidator().Declare(request, TimestampUtc);

    private static TypedActionFormationRequest CreateRequest(
        HarmonicInterlockModulationCorrespondenceReceipt? source = null,
        IReadOnlyList<TypedActionSurfaceDeclaration>? actions = null,
        IReadOnlyList<MethodologicalFormationAnalysis>? analyses = null,
        IReadOnlyList<DesignPredicateDeclaration>? predicates = null,
        TypedActionFormationScopeBoundary? scope = null,
        int priorPassageCount = 82,
        bool omitSource = false)
    {
        var action = CreateAction("review-001");
        return new TypedActionFormationRequest(
            SourceCorrespondenceReceipt: omitSource ? null : source ?? CreateSourceCorrespondence(),
            ActionDeclarations: actions ?? [action],
            FormationAnalyses: analyses ?? [CreateAnalysis(action.ActionHandle, ActionFormationOrigin.DesignInference)],
            DesignPredicates: predicates ?? CreatePredicates(action.ActionHandle),
            ScopeBoundary: scope ?? CreateScope(),
            PriorPassageCount: priorPassageCount);
    }

    private static TypedActionSurfaceDeclaration CreateAction(string suffix) =>
        new(
            ActionHandle: $"urn:san:typed-action:{suffix}",
            SourceSurface: SanctuaryPacketSurfaces.Compass,
            TargetSurface: SanctuaryPacketSurfaces.Steward,
            DeclaredIntent: "review candidate action surface without executing",
            MethodCode: "method:review-only-boundary",
            AuthorityCeiling: "ceiling:review-only",
            CustodyOwner: SanctuaryPacketSurfaces.Steward,
            WitnessBurden: "witness:separate-custody",
            TelemetryRoute: SanctuaryPacketRoutes.TelemetryString,
            AdmissibilityPredicate: "admissibility:declared-terms-required",
            RevocationPath: "revocation:typed-action-candidate",
            LossCondition: "loss:action-declaration-promotes-itself",
            ReviewOnly: true,
            CandidateOnly: true,
            RuntimeEffectRequested: false,
            ContinuityEffectRequested: false,
            AttemptsSelfAuthorization: false);

    private static MethodologicalFormationAnalysis CreateAnalysis(
        string actionHandle,
        ActionFormationOrigin origin) =>
        new(
            FormationHandle: $"urn:san:formation-analysis:{origin.ToString().ToLowerInvariant()}",
            ActionHandle: actionHandle,
            Origin: origin,
            SourceEvidenceHandle: "urn:san:evidence:typed-action-source",
            FormationTrace: "correspondence->typed-action-candidate->formation-analysis",
            PressureClass: "bounded-review-pressure",
            EvidenceBodyPresent: true,
            WitnessBodyPresent: true,
            ExplainsCandidate: true,
            AuthorizesCandidate: false,
            EmitsPacket: false,
            ReplaysReceipt: false,
            IncrementsPassage: false);

    private static DesignPredicateDeclaration[] CreatePredicates(string actionHandle) =>
    [
        CreatePredicate(actionHandle, "predicate:source-target-method", "source-target-method"),
        CreatePredicate(actionHandle, "predicate:authority-ceiling", "authority-ceiling"),
        CreatePredicate(actionHandle, "predicate:witness-revocation-loss", "witness-revocation-loss")
    ];

    private static DesignPredicateDeclaration CreatePredicate(
        string actionHandle,
        string predicateCode,
        string requiredTerm) =>
        new(
            PredicateHandle: $"urn:san:design-predicate:{requiredTerm}",
            ActionHandle: actionHandle,
            PredicateCode: predicateCode,
            RequiresTerm: requiredTerm,
            RequiredTermPresent: true,
            ReviewOnly: true,
            MayExecuteItself: false,
            MayAuthorizeAction: false,
            MayAdmitContinuity: false,
            MayActivateRuntime: false,
            MayEvaluateLisp: false);

    private static TypedActionFormationScopeBoundary CreateScope() =>
        new(
            ScopeCode: "typed-action-review-only",
            Present: true,
            ReviewOnly: true,
            AllowsRuntimeAction: false,
            AllowsContinuityEffect: false,
            AllowsAuthority: false,
            AllowsActivation: false,
            AllowsLispEvaluation: false,
            AllowsPacketEmission: false,
            AllowsReceiptReplay: false,
            AllowsPassageIncrement: false);

    private static HarmonicInterlockModulationCorrespondenceReceipt CreateSourceCorrespondence() =>
        new(
            ReceiptHandle: "urn:san:modulation-correspondence:review:fixture",
            Disposition: HarmonicInterlockModulationCorrespondenceDisposition.AtlasReviewCold,
            OutcomeCode: "modulation-correspondence-atlas-review-only",
            GovernanceTrace: "fixture cold modulation correspondence",
            SourceInterlockReceiptHandle: "urn:san:steward-harmonic-interlock:review:fixture",
            Sources: [],
            Concepts: [],
            TranslationBoundary: new CmeCorrespondenceTranslationBoundary(
                BoundaryCode: "fixture-translation",
                Present: true,
                SemanticCustodyRequired: true,
                WitnessBurdenRequired: true,
                AuthorityCeilingRequired: true,
                ContinuityRiskRequired: true,
                RevocationPathRequired: true,
                ExplicitNonClaimRequired: true,
                AllowsEquivalenceClaim: false,
                AllowsProofTransfer: false,
                AllowsOntologyTransfer: false,
                AllowsSourceSuccessAsCmeSuccess: false,
                AllowsChannelSuccessAsWarrant: false),
            ActualizationBoundary: new CorrespondenceActualizationTestBoundary(
                BoundaryCode: "fixture-actualization",
                Present: true,
                PreservesIntendedGoal: true,
                PreservesCustody: true,
                PreservesWitness: true,
                PreservesRevocation: true,
                PreservesContinuitySafety: true,
                RefusesAuthorityLaundering: true,
                RefusesSemanticWarrantFromPropagation: true,
                AllowsRuntimeAction: false,
                AllowsLispEvaluation: false,
                AllowsPacketEmission: false,
                AllowsReceiptReplay: false,
                AllowsPassageIncrement: false,
                AllowsContinuityAdmission: false,
                AllowsAuthority: false),
            LossConditions: [],
            PreservedSourceHandles: [],
            PreservedConceptHandles: [],
            Refusal: null,
            PriorPassageCount: 72,
            PassageCountAfterCorrespondenceReview: 72,
            ReviewOnly: true,
            InertOnly: true,
            CorrespondenceBecomesEquivalence: false,
            BorrowedAnalogyBecomesProof: false,
            BorrowedMechanismBecomesOntology: false,
            ImportedSuccessBecomesGovernanceCondition: false,
            ChannelSuccessBecomesSemanticWarrant: false,
            TransmissionBecomesAdmissibility: false,
            SynchronizationBecomesAuthority: false,
            ThroughputBecomesContinuity: false,
            RuntimeActionAllowed: false,
            LispEvaluationAllowed: false,
            NewPacketEmitted: false,
            ReceiptsReplayed: false,
            ContinuityAdmitted: false,
            AuthorityGranted: false,
            ActivationRefused: true,
            TimestampUtc: TimestampUtc);

    private static TypedActionSurfaceDeclaration MutateAction(
        TypedActionSurfaceDeclaration action,
        string mutation) =>
        mutation switch
        {
            "source" => action with { SourceSurface = string.Empty },
            "target" => action with { TargetSurface = string.Empty },
            "intent" => action with { DeclaredIntent = string.Empty },
            "method" => action with { MethodCode = string.Empty },
            "ceiling" => action with { AuthorityCeiling = string.Empty },
            "custody" => action with { CustodyOwner = string.Empty },
            "witness" => action with { WitnessBurden = string.Empty },
            "telemetry" => action with { TelemetryRoute = string.Empty },
            "admissibility" => action with { AdmissibilityPredicate = string.Empty },
            "revocation" => action with { RevocationPath = string.Empty },
            "loss" => action with { LossCondition = string.Empty },
            "runtime" => action with { RuntimeEffectRequested = true },
            "continuity" => action with { ContinuityEffectRequested = true },
            "self-authorize" => action with { AttemptsSelfAuthorization = true },
            _ => action
        };

    private static MethodologicalFormationAnalysis MutateAnalysis(
        MethodologicalFormationAnalysis analysis,
        string mutation) =>
        mutation switch
        {
            "missing-evidence" => analysis with { EvidenceBodyPresent = false },
            "missing-witness" => analysis with { WitnessBodyPresent = false },
            "authorizes" => analysis with { AuthorizesCandidate = true },
            "emits-packet" => analysis with { EmitsPacket = true },
            "replays" => analysis with { ReplaysReceipt = true },
            "increments" => analysis with { IncrementsPassage = true },
            "lineage-mismatch" => analysis with { ActionHandle = "urn:san:typed-action:missing" },
            _ => analysis
        };

    private static DesignPredicateDeclaration MutatePredicate(
        DesignPredicateDeclaration predicate,
        string mutation) =>
        mutation switch
        {
            "missing-term" => predicate with { RequiredTermPresent = false },
            "executes" => predicate with { MayExecuteItself = true },
            "authorizes" => predicate with { MayAuthorizeAction = true },
            "continuity" => predicate with { MayAdmitContinuity = true },
            "activation" => predicate with { MayActivateRuntime = true },
            "lisp" => predicate with { MayEvaluateLisp = true },
            "lineage-mismatch" => predicate with { ActionHandle = "urn:san:typed-action:missing" },
            _ => predicate
        };

    private static void AssertCold(TypedActionFormationReceipt receipt)
    {
        Assert.True(receipt.IsColdTypedActionFormation);
        Assert.True(receipt.ReviewOnly);
        Assert.True(receipt.CandidateOnly);
        Assert.Equal(receipt.PriorPassageCount, receipt.PassageCountAfterTypedActionReview);
        Assert.False(receipt.DeclaredActionExecutes);
        Assert.False(receipt.FormationAnalysisAuthorizes);
        Assert.False(receipt.DesignPredicateExecutes);
        Assert.False(receipt.DesignPredicateAuthorizes);
        Assert.False(receipt.SummaryBecomesAction);
        Assert.False(receipt.ReceiptBecomesAction);
        Assert.False(receipt.ReplayBecomesAction);
        Assert.False(receipt.QueryBecomesAction);
        Assert.False(receipt.RuntimeActionAllowed);
        Assert.False(receipt.LispEvaluationAllowed);
        Assert.False(receipt.NewPacketEmitted);
        Assert.False(receipt.ReceiptsReplayed);
        Assert.False(receipt.ContinuityAdmitted);
        Assert.False(receipt.AuthorityGranted);
        Assert.True(receipt.ActivationRefused);
        Assert.Null(receipt.Refusal);
    }

    private static void AssertRefused(
        TypedActionFormationReceipt receipt,
        string outcomeCode)
    {
        Assert.Equal(TypedActionFormationDisposition.Refused, receipt.Disposition);
        Assert.Equal(outcomeCode, receipt.OutcomeCode);
        Assert.True(receipt.IsRetainedTypedActionFormationRefusal);
        Assert.NotNull(receipt.Refusal);
        Assert.True(receipt.Refusal!.Retained);
        Assert.Empty(receipt.ActionDeclarations);
        Assert.Empty(receipt.FormationAnalyses);
        Assert.Empty(receipt.DesignPredicates);
        Assert.False(receipt.DeclaredActionExecutes);
        Assert.False(receipt.FormationAnalysisAuthorizes);
        Assert.False(receipt.DesignPredicateExecutes);
        Assert.False(receipt.DesignPredicateAuthorizes);
        Assert.False(receipt.RuntimeActionAllowed);
        Assert.False(receipt.LispEvaluationAllowed);
        Assert.False(receipt.NewPacketEmitted);
        Assert.False(receipt.ReceiptsReplayed);
        Assert.False(receipt.ContinuityAdmitted);
        Assert.False(receipt.AuthorityGranted);
        Assert.True(receipt.ActivationRefused);
    }

    private static string FindRepositoryRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null)
        {
            var candidate = Path.Combine(directory.FullName, "src", "SLI", "SLI.Lisp", "typed-action-formation.lisp");
            if (File.Exists(candidate))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        throw new DirectoryNotFoundException("Could not locate OAN Mortalis repository root.");
    }
}
