using San.Common;
using Xunit;

namespace San.Audit.Tests;

public sealed class ActionMethodReadinessBoundaryValidationTests
{
    private static readonly DateTimeOffset TimestampUtc = DateTimeOffset.Parse("2026-05-14T00:00:00Z");

    [Fact]
    public void Action_Method_Readiness_Accepts_Cold_Method_For_Steward_Review()
    {
        var receipt = Declare(CreateRequest());

        Assert.Equal(ActionMethodReadinessDisposition.ReadyForStewardReviewCold, receipt.Disposition);
        Assert.Equal("action-method-readiness-ready-for-steward-review-cold", receipt.OutcomeCode);
        Assert.Single(receipt.Methods);
        Assert.Equal(3, receipt.TermSatisfactions.Count);
        Assert.True(receipt.MethodReadyForStewardReview);
        AssertCold(receipt);
    }

    [Fact]
    public void Empty_Method_Readiness_Surface_Is_Reviewable_But_Not_Authoritative()
    {
        var receipt = Declare(CreateRequest(
            source: CreateSourceTypedAction(actions: []),
            methods: [],
            terms: []));

        Assert.Equal(ActionMethodReadinessDisposition.EmptyReviewCold, receipt.Disposition);
        Assert.Equal("action-method-readiness-empty-review-only", receipt.OutcomeCode);
        Assert.Empty(receipt.Methods);
        Assert.Empty(receipt.TermSatisfactions);
        Assert.False(receipt.MethodReadyForStewardReview);
        AssertCold(receipt);
    }

    [Fact]
    public void Method_Readiness_Preserves_Action_Method_And_Term_Lineage()
    {
        var action = CreateAction("review-001");
        var source = CreateSourceTypedAction(actions: [action]);
        var method = CreateMethod(action.ActionHandle, "review-method-001");
        var terms = CreateTerms(method.MethodHandle);

        var receipt = Declare(CreateRequest(
            source: source,
            methods: [method],
            terms: terms));

        AssertCold(receipt);
        Assert.Equal(source.ReceiptHandle, receipt.SourceTypedActionReceiptHandle);
        Assert.Equal(action.ActionHandle, receipt.Methods[0].ActionHandle);
        Assert.All(receipt.TermSatisfactions, term => Assert.Equal(method.MethodHandle, term.MethodHandle));
    }

    [Fact]
    public void Method_Readiness_Requires_Cold_Typed_Action_Source()
    {
        var receipt = Declare(CreateRequest(source: null, omitSource: true));

        AssertRefused(receipt, "action-method-source-typed-action-missing");
    }

    [Theory]
    [InlineData("missing-method-code")]
    [InlineData("missing-goal")]
    [InlineData("missing-steward")]
    [InlineData("missing-custody")]
    [InlineData("missing-witness")]
    [InlineData("missing-telemetry")]
    [InlineData("missing-term-set")]
    [InlineData("missing-revocation")]
    [InlineData("missing-loss")]
    [InlineData("self-authorizes")]
    [InlineData("runtime")]
    [InlineData("continuity")]
    [InlineData("lisp")]
    [InlineData("packet")]
    [InlineData("no-steward-review")]
    public void Method_Candidate_Requires_Declared_Terms_And_Non_Promotional_Posture(string mutation)
    {
        var action = CreateAction("review-001");
        var method = MutateMethod(CreateMethod(action.ActionHandle, "review-method-001"), mutation);

        var receipt = Declare(CreateRequest(
            source: CreateSourceTypedAction(actions: [action]),
            methods: [method],
            terms: CreateTerms(method.MethodHandle)));

        AssertRefused(receipt, "action-method-candidate-invalid");
    }

    [Theory]
    [InlineData("missing-term")]
    [InlineData("missing-evidence")]
    [InlineData("missing-witness")]
    [InlineData("authorizes")]
    [InlineData("warrant")]
    [InlineData("emits-packet")]
    [InlineData("replays")]
    [InlineData("increments")]
    [InlineData("lineage-mismatch")]
    public void Term_Satisfaction_May_Support_Readiness_But_Not_Warrant(string mutation)
    {
        var action = CreateAction("review-001");
        var method = CreateMethod(action.ActionHandle, "review-method-001");
        var term = MutateTerm(CreateTerm(method.MethodHandle, "source-target-method"), mutation);

        var receipt = Declare(CreateRequest(
            source: CreateSourceTypedAction(actions: [action]),
            methods: [method],
            terms: [term]));

        AssertRefused(receipt, "action-method-term-satisfaction-invalid");
    }

    [Theory]
    [InlineData("self-review")]
    [InlineData("authorization")]
    [InlineData("runtime")]
    [InlineData("continuity")]
    [InlineData("lisp")]
    [InlineData("packet")]
    [InlineData("replay")]
    [InlineData("passage")]
    [InlineData("activation")]
    public void Steward_Method_Review_Boundary_Must_Not_Become_Execution_Or_Authority(string mutation)
    {
        var receipt = Declare(CreateRequest(stewardBoundary: MutateStewardBoundary(CreateStewardBoundary(), mutation)));

        AssertRefused(receipt, "action-method-steward-boundary-invalid");
    }

    [Theory]
    [InlineData("ready-authorizes")]
    [InlineData("predicate-warrants")]
    [InlineData("review-executes")]
    [InlineData("runtime")]
    [InlineData("continuity")]
    [InlineData("authority")]
    [InlineData("activation")]
    [InlineData("lisp")]
    [InlineData("packet")]
    [InlineData("replay")]
    [InlineData("passage")]
    public void Scope_Refuses_Readiness_As_Authorization_Warrant_Or_Execution(string mutation)
    {
        var receipt = Declare(CreateRequest(scope: MutateScope(CreateScope(), mutation)));

        AssertRefused(receipt, "action-method-scope-promotional");
    }

    [Fact]
    public void Method_Readiness_Refuses_Method_For_Unknown_Action()
    {
        var action = CreateAction("review-001");
        var method = CreateMethod("urn:san:typed-action:missing", "review-method-001");

        var receipt = Declare(CreateRequest(
            source: CreateSourceTypedAction(actions: [action]),
            methods: [method],
            terms: CreateTerms(method.MethodHandle)));

        AssertRefused(receipt, "action-method-action-lineage-missing");
    }

    [Fact]
    public void Method_Readiness_Refuses_Duplicate_Method_Handles()
    {
        var action = CreateAction("review-001");
        var method = CreateMethod(action.ActionHandle, "review-method-001");

        var receipt = Declare(CreateRequest(
            source: CreateSourceTypedAction(actions: [action]),
            methods: [method, method],
            terms: CreateTerms(method.MethodHandle)));

        AssertRefused(receipt, "action-method-duplicate-method-handle");
    }

    [Fact]
    public void Method_Readiness_Requires_Term_Coverage_For_Each_Method()
    {
        var action = CreateAction("review-001");
        var method = CreateMethod(action.ActionHandle, "review-method-001");

        var receipt = Declare(CreateRequest(
            source: CreateSourceTypedAction(actions: [action]),
            methods: [method],
            terms: []));

        AssertRefused(receipt, "action-method-readiness-term-coverage-missing");
    }

    [Fact]
    public void Method_Readiness_Requires_Method_Coverage_For_Each_Typed_Action()
    {
        var actionA = CreateAction("review-001");
        var actionB = CreateAction("review-002");
        var method = CreateMethod(actionA.ActionHandle, "review-method-001");

        var receipt = Declare(CreateRequest(
            source: CreateSourceTypedAction(actions: [actionA, actionB]),
            methods: [method],
            terms: CreateTerms(method.MethodHandle)));

        AssertRefused(receipt, "action-method-readiness-action-coverage-missing");
    }

    [Fact]
    public void Method_Readiness_Does_Not_Emit_Packet_Evaluate_Lisp_Replay_Activate_Or_Increment_Passage()
    {
        var receipt = Declare(CreateRequest(priorPassageCount: 512));

        Assert.Equal(512, receipt.PriorPassageCount);
        Assert.Equal(512, receipt.PassageCountAfterMethodReadinessReview);
        Assert.False(receipt.MethodReadinessAuthorizes);
        Assert.False(receipt.PredicateSatisfactionBecomesWarrant);
        Assert.False(receipt.StewardReviewExecutes);
        Assert.False(receipt.RuntimeActionAllowed);
        Assert.False(receipt.LispEvaluationAllowed);
        Assert.False(receipt.NewPacketEmitted);
        Assert.False(receipt.ReceiptsReplayed);
        Assert.True(receipt.ActivationRefused);
        AssertCold(receipt);
    }

    [Fact]
    public void Lisp_Body_Declares_Action_Method_Readiness_As_Inert_Carrier()
    {
        var lispPath = Path.Combine(
            FindRepositoryRoot(),
            "src",
            "SLI",
            "SLI.Lisp",
            "action-method-readiness.lisp");

        var body = File.ReadAllText(lispPath);

        Assert.Contains(":posture :cme-action-method-readiness-boundary", body, StringComparison.Ordinal);
        Assert.Contains(":lisp-role :inert-method-readiness-carrier", body, StringComparison.Ordinal);
        Assert.Contains(":core-invariant \"predicate satisfaction is not warrant\"", body, StringComparison.Ordinal);
        Assert.Contains(":method-readiness-authorizes nil", body, StringComparison.Ordinal);
        Assert.Contains(":predicate-satisfaction-becomes-warrant nil", body, StringComparison.Ordinal);
        Assert.Contains(":steward-review-executes nil", body, StringComparison.Ordinal);
        Assert.Contains(":runtime-action-allowed nil", body, StringComparison.Ordinal);
        Assert.Contains(":lisp-evaluation-allowed nil", body, StringComparison.Ordinal);
        Assert.Contains(":return :receipt-only", body, StringComparison.Ordinal);
        Assert.DoesNotContain("(eval", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("(compile", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("(load", body, StringComparison.OrdinalIgnoreCase);
    }

    private static ActionMethodReadinessReceipt Declare(ActionMethodReadinessRequest request) =>
        new DefaultActionMethodReadinessBoundaryValidator().Declare(request, TimestampUtc);

    private static ActionMethodReadinessRequest CreateRequest(
        TypedActionFormationReceipt? source = null,
        IReadOnlyList<ActionMethodCandidate>? methods = null,
        IReadOnlyList<MethodTermSatisfaction>? terms = null,
        StewardMethodReviewBoundary? stewardBoundary = null,
        ActionMethodReadinessScopeBoundary? scope = null,
        int priorPassageCount = 94,
        bool omitSource = false)
    {
        var action = CreateAction("review-001");
        var method = CreateMethod(action.ActionHandle, "review-method-001");
        return new ActionMethodReadinessRequest(
            SourceTypedActionReceipt: omitSource ? null : source ?? CreateSourceTypedAction(actions: [action]),
            Methods: methods ?? [method],
            TermSatisfactions: terms ?? CreateTerms(method.MethodHandle),
            StewardBoundary: stewardBoundary ?? CreateStewardBoundary(),
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

    private static TypedActionFormationReceipt CreateSourceTypedAction(
        IReadOnlyList<TypedActionSurfaceDeclaration>? actions = null)
    {
        var actionList = actions ?? [CreateAction("review-001")];
        return new TypedActionFormationReceipt(
            ReceiptHandle: "urn:san:typed-action-formation:review:fixture",
            Disposition: actionList.Count == 0
                ? TypedActionFormationDisposition.EmptyReviewCold
                : TypedActionFormationDisposition.DeclaredForReviewCold,
            OutcomeCode: "typed-action-formation-declared-review-only",
            GovernanceTrace: "fixture cold typed action formation",
            SourceCorrespondenceReceiptHandle: "urn:san:modulation-correspondence:review:fixture",
            ActionDeclarations: actionList,
            FormationAnalyses: [],
            DesignPredicates: [],
            ScopeBoundary: new TypedActionFormationScopeBoundary(
                ScopeCode: "fixture-typed-action-review",
                Present: true,
                ReviewOnly: true,
                AllowsRuntimeAction: false,
                AllowsContinuityEffect: false,
                AllowsAuthority: false,
                AllowsActivation: false,
                AllowsLispEvaluation: false,
                AllowsPacketEmission: false,
                AllowsReceiptReplay: false,
                AllowsPassageIncrement: false),
            NonExecutionBoundary: new TypedActionFormationNonExecutionBoundary(
                DeclaredActionMayExecute: false,
                FormationAnalysisMayAuthorize: false,
                DesignPredicateMayExecute: false,
                DesignPredicateMayAuthorize: false,
                SummaryMayBecomeAction: false,
                ReceiptMayBecomeAction: false,
                ReplayMayBecomeAction: false,
                QueryMayBecomeAction: false,
                EmitsPacket: false,
                IncrementsPassageCount: false,
                AllowsContinuity: false,
                AllowsAuthority: false,
                BoundaryLaw: "fixture typed action does not execute"),
            Refusal: null,
            PriorPassageCount: 82,
            PassageCountAfterTypedActionReview: 82,
            ReviewOnly: true,
            CandidateOnly: true,
            DeclaredActionExecutes: false,
            FormationAnalysisAuthorizes: false,
            DesignPredicateExecutes: false,
            DesignPredicateAuthorizes: false,
            SummaryBecomesAction: false,
            ReceiptBecomesAction: false,
            ReplayBecomesAction: false,
            QueryBecomesAction: false,
            RuntimeActionAllowed: false,
            LispEvaluationAllowed: false,
            NewPacketEmitted: false,
            ReceiptsReplayed: false,
            ContinuityAdmitted: false,
            AuthorityGranted: false,
            ActivationRefused: true,
            TimestampUtc: TimestampUtc);
    }

    private static ActionMethodCandidate CreateMethod(string actionHandle, string suffix) =>
        new(
            MethodHandle: $"urn:san:action-method:{suffix}",
            ActionHandle: actionHandle,
            MethodClass: ActionMethodClass.ReviewOnly,
            MethodCode: "method:prepare-steward-review",
            IntendedGoal: "prepare candidate method for Steward review without authorizing work",
            StewardSurface: SanctuaryPacketSurfaces.Steward,
            CustodyOwner: SanctuaryPacketSurfaces.Steward,
            WitnessSurface: "witness:separate-custody",
            TelemetryRoute: SanctuaryPacketRoutes.TelemetryString,
            RequiredTermSet: "source-target-method-witness-revocation-loss",
            RevocationPath: "revocation:method-readiness-candidate",
            LossCondition: "loss:method-readiness-promotes-to-authorization",
            ReviewOnly: true,
            CandidateOnly: true,
            StewardReviewRequired: true,
            ClaimsAuthorization: false,
            RequestsRuntimeAction: false,
            RequestsContinuityAdmission: false,
            RequestsLispEvaluation: false,
            EmitsPacket: false);

    private static MethodTermSatisfaction[] CreateTerms(string methodHandle) =>
    [
        CreateTerm(methodHandle, "source-target-method"),
        CreateTerm(methodHandle, "steward-custody-witness"),
        CreateTerm(methodHandle, "revocation-loss")
    ];

    private static MethodTermSatisfaction CreateTerm(string methodHandle, string requiredTerm) =>
        new(
            TermHandle: $"urn:san:method-term:{requiredTerm}",
            MethodHandle: methodHandle,
            RequiredTerm: requiredTerm,
            EvidenceHandle: $"urn:san:evidence:{requiredTerm}",
            TermPresent: true,
            EvidenceBodyPresent: true,
            WitnessBodyPresent: true,
            SatisfiesReadiness: true,
            SatisfiesAuthorization: false,
            BecomesSemanticWarrant: false,
            EmitsPacket: false,
            ReplaysReceipt: false,
            IncrementsPassage: false);

    private static StewardMethodReviewBoundary CreateStewardBoundary() =>
        new(
            BoundaryCode: "steward-method-review-cold",
            Present: true,
            StewardSurface: SanctuaryPacketSurfaces.Steward,
            AuthorityCeiling: "ceiling:method-readiness-review",
            CustodyOwner: SanctuaryPacketSurfaces.Steward,
            WitnessSurface: "witness:separate-custody",
            TelemetryRoute: SanctuaryPacketRoutes.TelemetryString,
            ReviewOnly: true,
            RequiresSteward: true,
            AllowsSelfReview: false,
            AllowsAuthorization: false,
            AllowsRuntimeAction: false,
            AllowsContinuityAdmission: false,
            AllowsLispEvaluation: false,
            AllowsPacketEmission: false,
            AllowsReceiptReplay: false,
            AllowsPassageIncrement: false,
            AllowsActivation: false);

    private static ActionMethodReadinessScopeBoundary CreateScope() =>
        new(
            ScopeCode: "action-method-readiness-review-only",
            Present: true,
            ReviewOnly: true,
            MethodReadyMeansAuthorization: false,
            PredicateSatisfactionMeansWarrant: false,
            StewardReviewMeansExecution: false,
            AllowsRuntimeAction: false,
            AllowsContinuityAdmission: false,
            AllowsAuthority: false,
            AllowsActivation: false,
            AllowsLispEvaluation: false,
            AllowsPacketEmission: false,
            AllowsReceiptReplay: false,
            AllowsPassageIncrement: false);

    private static ActionMethodCandidate MutateMethod(ActionMethodCandidate method, string mutation) =>
        mutation switch
        {
            "missing-method-code" => method with { MethodCode = string.Empty },
            "missing-goal" => method with { IntendedGoal = string.Empty },
            "missing-steward" => method with { StewardSurface = string.Empty },
            "missing-custody" => method with { CustodyOwner = string.Empty },
            "missing-witness" => method with { WitnessSurface = string.Empty },
            "missing-telemetry" => method with { TelemetryRoute = string.Empty },
            "missing-term-set" => method with { RequiredTermSet = string.Empty },
            "missing-revocation" => method with { RevocationPath = string.Empty },
            "missing-loss" => method with { LossCondition = string.Empty },
            "self-authorizes" => method with { ClaimsAuthorization = true },
            "runtime" => method with { RequestsRuntimeAction = true },
            "continuity" => method with { RequestsContinuityAdmission = true },
            "lisp" => method with { RequestsLispEvaluation = true },
            "packet" => method with { EmitsPacket = true },
            "no-steward-review" => method with { StewardReviewRequired = false },
            _ => method
        };

    private static MethodTermSatisfaction MutateTerm(MethodTermSatisfaction term, string mutation) =>
        mutation switch
        {
            "missing-term" => term with { TermPresent = false },
            "missing-evidence" => term with { EvidenceBodyPresent = false },
            "missing-witness" => term with { WitnessBodyPresent = false },
            "authorizes" => term with { SatisfiesAuthorization = true },
            "warrant" => term with { BecomesSemanticWarrant = true },
            "emits-packet" => term with { EmitsPacket = true },
            "replays" => term with { ReplaysReceipt = true },
            "increments" => term with { IncrementsPassage = true },
            "lineage-mismatch" => term with { MethodHandle = "urn:san:action-method:missing" },
            _ => term
        };

    private static StewardMethodReviewBoundary MutateStewardBoundary(
        StewardMethodReviewBoundary boundary,
        string mutation) =>
        mutation switch
        {
            "self-review" => boundary with { AllowsSelfReview = true },
            "authorization" => boundary with { AllowsAuthorization = true },
            "runtime" => boundary with { AllowsRuntimeAction = true },
            "continuity" => boundary with { AllowsContinuityAdmission = true },
            "lisp" => boundary with { AllowsLispEvaluation = true },
            "packet" => boundary with { AllowsPacketEmission = true },
            "replay" => boundary with { AllowsReceiptReplay = true },
            "passage" => boundary with { AllowsPassageIncrement = true },
            "activation" => boundary with { AllowsActivation = true },
            _ => boundary
        };

    private static ActionMethodReadinessScopeBoundary MutateScope(
        ActionMethodReadinessScopeBoundary scope,
        string mutation) =>
        mutation switch
        {
            "ready-authorizes" => scope with { MethodReadyMeansAuthorization = true },
            "predicate-warrants" => scope with { PredicateSatisfactionMeansWarrant = true },
            "review-executes" => scope with { StewardReviewMeansExecution = true },
            "runtime" => scope with { AllowsRuntimeAction = true },
            "continuity" => scope with { AllowsContinuityAdmission = true },
            "authority" => scope with { AllowsAuthority = true },
            "activation" => scope with { AllowsActivation = true },
            "lisp" => scope with { AllowsLispEvaluation = true },
            "packet" => scope with { AllowsPacketEmission = true },
            "replay" => scope with { AllowsReceiptReplay = true },
            "passage" => scope with { AllowsPassageIncrement = true },
            _ => scope
        };

    private static void AssertCold(ActionMethodReadinessReceipt receipt)
    {
        Assert.True(receipt.IsColdMethodReadiness);
        Assert.True(receipt.ReviewOnly);
        Assert.True(receipt.CandidateOnly);
        Assert.Equal(receipt.PriorPassageCount, receipt.PassageCountAfterMethodReadinessReview);
        Assert.False(receipt.MethodReadinessAuthorizes);
        Assert.False(receipt.PredicateSatisfactionBecomesWarrant);
        Assert.False(receipt.StewardReviewExecutes);
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
        ActionMethodReadinessReceipt receipt,
        string outcomeCode)
    {
        Assert.Equal(ActionMethodReadinessDisposition.Refused, receipt.Disposition);
        Assert.Equal(outcomeCode, receipt.OutcomeCode);
        Assert.True(receipt.IsRetainedMethodReadinessRefusal);
        Assert.NotNull(receipt.Refusal);
        Assert.True(receipt.Refusal!.Retained);
        Assert.Empty(receipt.Methods);
        Assert.Empty(receipt.TermSatisfactions);
        Assert.False(receipt.MethodReadyForStewardReview);
        Assert.False(receipt.MethodReadinessAuthorizes);
        Assert.False(receipt.PredicateSatisfactionBecomesWarrant);
        Assert.False(receipt.StewardReviewExecutes);
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
            var candidate = Path.Combine(directory.FullName, "src", "SLI", "SLI.Lisp", "action-method-readiness.lisp");
            if (File.Exists(candidate))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        throw new DirectoryNotFoundException("Could not locate Project Bicycle repository root.");
    }
}
