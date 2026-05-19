using San.Common;
using Xunit;

namespace San.Audit.Tests;

public sealed class StewardActionAdmissibilityBoundaryValidationTests
{
    private static readonly DateTimeOffset TimestampUtc = DateTimeOffset.Parse("2026-05-14T00:00:00Z");

    [Fact]
    public void Steward_Action_Admissibility_Accepts_Cold_Decision_For_Enactment_Review()
    {
        var receipt = Declare(CreateRequest());

        Assert.Equal(StewardActionAdmissibilityDisposition.AdmissibleForEnactmentReviewCold, receipt.Disposition);
        Assert.Equal("steward-action-admissibility-for-enactment-review-cold", receipt.OutcomeCode);
        Assert.Single(receipt.Decisions);
        Assert.Equal(3, receipt.PredicateResults.Count);
        Assert.True(receipt.AdmissibleForEnactmentReview);
        Assert.True(receipt.SeparateEnactmentBoundaryRequired);
        AssertCold(receipt);
    }

    [Fact]
    public void Steward_Action_Admissibility_Preserves_Method_Action_And_Predicate_Lineage()
    {
        var source = CreateSourceMethodReadiness();
        var method = source.Methods[0];
        var decision = CreateDecision(method.MethodHandle, method.ActionHandle, "decision-001");
        var predicates = CreatePredicateResults(method.MethodHandle, method.ActionHandle);

        var receipt = Declare(CreateRequest(
            source: source,
            decisions: [decision],
            predicates: predicates));

        AssertCold(receipt);
        Assert.Equal(source.ReceiptHandle, receipt.SourceMethodReadinessReceiptHandle);
        Assert.Equal(method.MethodHandle, receipt.Decisions[0].MethodHandle);
        Assert.Equal(method.ActionHandle, receipt.Decisions[0].ActionHandle);
        Assert.All(receipt.PredicateResults, predicate => Assert.Equal(method.MethodHandle, predicate.MethodHandle));
        Assert.All(receipt.PredicateResults, predicate => Assert.Equal(method.ActionHandle, predicate.ActionHandle));
    }

    [Fact]
    public void Steward_Action_Admissibility_Requires_Cold_Method_Readiness_Source()
    {
        var receipt = Declare(CreateRequest(source: null, omitSource: true));

        AssertRefused(receipt, "steward-action-admissibility-source-method-readiness-missing");
    }

    [Fact]
    public void Steward_Action_Admissibility_Requires_A_Ready_Method_Source()
    {
        var source = CreateSourceMethodReadiness(methods: [], ready: false);

        var receipt = Declare(CreateRequest(source: source, decisions: [], predicates: []));

        AssertRefused(receipt, "steward-action-admissibility-source-method-readiness-missing");
    }

    [Theory]
    [InlineData("missing-steward")]
    [InlineData("missing-custody")]
    [InlineData("missing-witness")]
    [InlineData("missing-telemetry")]
    [InlineData("missing-ceiling")]
    [InlineData("missing-revocation")]
    [InlineData("missing-loss")]
    [InlineData("no-enactment-boundary")]
    [InlineData("not-admissible")]
    [InlineData("authorizes")]
    [InlineData("executes")]
    [InlineData("authority")]
    [InlineData("continuity")]
    [InlineData("activation")]
    [InlineData("packet")]
    [InlineData("lisp")]
    public void Admissibility_Decision_Requires_Declared_Terms_And_Non_Executive_Posture(string mutation)
    {
        var source = CreateSourceMethodReadiness();
        var method = source.Methods[0];
        var decision = MutateDecision(CreateDecision(method.MethodHandle, method.ActionHandle, "decision-001"), mutation);

        var receipt = Declare(CreateRequest(
            source: source,
            decisions: [decision],
            predicates: CreatePredicateResults(method.MethodHandle, method.ActionHandle)));

        AssertRefused(receipt, "steward-action-admissibility-decision-invalid");
    }

    [Theory]
    [InlineData("missing-evidence")]
    [InlineData("missing-witness")]
    [InlineData("not-satisfied")]
    [InlineData("not-supporting")]
    [InlineData("warrant")]
    [InlineData("authorizes")]
    [InlineData("packet")]
    [InlineData("lisp")]
    [InlineData("replay")]
    [InlineData("passage")]
    [InlineData("continuity")]
    [InlineData("method-lineage-mismatch")]
    [InlineData("action-lineage-mismatch")]
    public void Admissibility_Predicate_May_Support_Admissibility_But_Not_Warrant_Or_Execution(string mutation)
    {
        var source = CreateSourceMethodReadiness();
        var method = source.Methods[0];
        var predicate = MutatePredicate(CreatePredicate(method.MethodHandle, method.ActionHandle, "method-ready"), mutation);

        var receipt = Declare(CreateRequest(
            source: source,
            decisions: [CreateDecision(method.MethodHandle, method.ActionHandle, "decision-001")],
            predicates: [predicate]));

        AssertRefused(receipt, "steward-action-admissibility-predicate-invalid");
    }

    [Theory]
    [InlineData("no-enactment-boundary")]
    [InlineData("admissibility-executes")]
    [InlineData("acceptance-runtime")]
    [InlineData("authority")]
    [InlineData("continuity")]
    [InlineData("runtime")]
    [InlineData("activation")]
    [InlineData("lisp")]
    [InlineData("packet")]
    [InlineData("replay")]
    [InlineData("passage")]
    public void Scope_Refuses_Admissibility_As_Execution_Authority_Continuity_Or_Runtime_Motion(string mutation)
    {
        var receipt = Declare(CreateRequest(scope: MutateScope(CreateScope(), mutation)));

        AssertRefused(receipt, "steward-action-admissibility-scope-promotional");
    }

    [Fact]
    public void Steward_Action_Admissibility_Refuses_Duplicate_Decision_Handles()
    {
        var source = CreateSourceMethodReadiness();
        var method = source.Methods[0];
        var decision = CreateDecision(method.MethodHandle, method.ActionHandle, "decision-001");

        var receipt = Declare(CreateRequest(
            source: source,
            decisions: [decision, decision],
            predicates: CreatePredicateResults(method.MethodHandle, method.ActionHandle)));

        AssertRefused(receipt, "steward-action-admissibility-duplicate-decision-handle");
    }

    [Fact]
    public void Steward_Action_Admissibility_Refuses_Decision_For_Unknown_Method()
    {
        var source = CreateSourceMethodReadiness();
        var method = source.Methods[0];
        var decision = CreateDecision("urn:san:action-method:missing", method.ActionHandle, "decision-001");

        var receipt = Declare(CreateRequest(
            source: source,
            decisions: [decision],
            predicates: CreatePredicateResults(method.MethodHandle, method.ActionHandle)));

        AssertRefused(receipt, "steward-action-admissibility-method-lineage-missing");
    }

    [Fact]
    public void Steward_Action_Admissibility_Requires_Predicate_Coverage_For_Each_Decision()
    {
        var source = CreateSourceMethodReadiness();
        var method = source.Methods[0];

        var receipt = Declare(CreateRequest(
            source: source,
            decisions: [CreateDecision(method.MethodHandle, method.ActionHandle, "decision-001")],
            predicates: []));

        AssertRefused(receipt, "steward-action-admissibility-predicate-coverage-missing");
    }

    [Fact]
    public void Steward_Action_Admissibility_Requires_Decision_Coverage_For_Each_Method()
    {
        var source = CreateSourceMethodReadiness(methods:
        [
            CreateMethod("urn:san:typed-action:review-001", "review-method-001"),
            CreateMethod("urn:san:typed-action:review-002", "review-method-002")
        ]);
        var first = source.Methods[0];

        var receipt = Declare(CreateRequest(
            source: source,
            decisions: [CreateDecision(first.MethodHandle, first.ActionHandle, "decision-001")],
            predicates: CreatePredicateResults(first.MethodHandle, first.ActionHandle)));

        AssertRefused(receipt, "steward-action-admissibility-method-coverage-missing");
    }

    [Fact]
    public void Steward_Action_Admissibility_Does_Not_Emit_Packet_Evaluate_Lisp_Replay_Activate_Or_Increment_Passage()
    {
        var receipt = Declare(CreateRequest(priorPassageCount: 610));

        Assert.Equal(610, receipt.PriorPassageCount);
        Assert.Equal(610, receipt.PassageCountAfterAdmissibilityReview);
        Assert.False(receipt.AdmissibilityExecutes);
        Assert.False(receipt.StewardAcceptanceMovesRuntime);
        Assert.False(receipt.RuntimeActionAllowed);
        Assert.False(receipt.LispEvaluationAllowed);
        Assert.False(receipt.NewPacketEmitted);
        Assert.False(receipt.ReceiptsReplayed);
        Assert.True(receipt.ActivationRefused);
        AssertCold(receipt);
    }

    [Fact]
    public void Lisp_Body_Declares_Steward_Action_Admissibility_As_Inert_Carrier()
    {
        var lispPath = Path.Combine(
            FindRepositoryRoot(),
            "src",
            "SLI",
            "SLI.Lisp",
            "steward-action-admissibility.lisp");

        var body = File.ReadAllText(lispPath);

        Assert.Contains(":posture :cme-steward-action-admissibility-boundary", body, StringComparison.Ordinal);
        Assert.Contains(":lisp-role :inert-steward-admissibility-carrier", body, StringComparison.Ordinal);
        Assert.Contains(":admissibility-is-execution nil", body, StringComparison.Ordinal);
        Assert.Contains(":steward-acceptance-is-runtime-motion nil", body, StringComparison.Ordinal);
        Assert.Contains(":admissible-action-may-execute nil", body, StringComparison.Ordinal);
        Assert.Contains(":admissibility-may-grant-authority nil", body, StringComparison.Ordinal);
        Assert.Contains(":return :receipt-only", body, StringComparison.Ordinal);
        Assert.DoesNotContain("(eval", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("(compile", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("(load", body, StringComparison.OrdinalIgnoreCase);
    }

    private static StewardActionAdmissibilityReceipt Declare(StewardActionAdmissibilityRequest request) =>
        new DefaultStewardActionAdmissibilityBoundaryValidator().Declare(request, TimestampUtc);

    private static StewardActionAdmissibilityRequest CreateRequest(
        ActionMethodReadinessReceipt? source = null,
        IReadOnlyList<StewardActionAdmissibilityDecision>? decisions = null,
        IReadOnlyList<StewardAdmissibilityPredicateResult>? predicates = null,
        StewardActionAdmissibilityScopeBoundary? scope = null,
        int priorPassageCount = 116,
        bool omitSource = false)
    {
        var sourceReceipt = source ?? CreateSourceMethodReadiness();
        var method = sourceReceipt.Methods.FirstOrDefault() ?? CreateMethod("urn:san:typed-action:review-001", "review-method-001");
        return new StewardActionAdmissibilityRequest(
            SourceMethodReadinessReceipt: omitSource ? null : sourceReceipt,
            Decisions: decisions ?? [CreateDecision(method.MethodHandle, method.ActionHandle, "decision-001")],
            PredicateResults: predicates ?? CreatePredicateResults(method.MethodHandle, method.ActionHandle),
            ScopeBoundary: scope ?? CreateScope(),
            PriorPassageCount: priorPassageCount);
    }

    private static ActionMethodReadinessReceipt CreateSourceMethodReadiness(
        IReadOnlyList<ActionMethodCandidate>? methods = null,
        bool ready = true)
    {
        var methodList = methods ?? [CreateMethod("urn:san:typed-action:review-001", "review-method-001")];
        return new ActionMethodReadinessReceipt(
            ReceiptHandle: "urn:san:action-method-readiness:review:fixture",
            Disposition: methodList.Count == 0
                ? ActionMethodReadinessDisposition.EmptyReviewCold
                : ActionMethodReadinessDisposition.ReadyForStewardReviewCold,
            OutcomeCode: "action-method-readiness-ready-for-steward-review-cold",
            GovernanceTrace: "fixture cold action method readiness",
            SourceTypedActionReceiptHandle: "urn:san:typed-action-formation:review:fixture",
            Methods: methodList,
            TermSatisfactions: [],
            StewardBoundary: new StewardMethodReviewBoundary(
                BoundaryCode: "fixture-steward-method-review",
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
                AllowsActivation: false),
            ScopeBoundary: new ActionMethodReadinessScopeBoundary(
                ScopeCode: "fixture-action-method-readiness",
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
                AllowsPassageIncrement: false),
            NonAuthorizationBoundary: new ActionMethodReadinessNonAuthorizationBoundary(
                MethodReadyMayAuthorize: false,
                PredicateSatisfactionMayWarrant: false,
                StewardReviewMayExecute: false,
                MethodReadinessMayEmitPacket: false,
                MethodReadinessMayEvaluateLisp: false,
                MethodReadinessMayAdmitContinuity: false,
                MethodReadinessMayGrantAuthority: false,
                MethodReadinessMayActivate: false,
                MethodReadinessMayIncrementPassage: false,
                BoundaryLaw: "fixture method readiness does not authorize"),
            Refusal: null,
            PriorPassageCount: 94,
            PassageCountAfterMethodReadinessReview: 94,
            ReviewOnly: true,
            CandidateOnly: true,
            MethodReadyForStewardReview: ready && methodList.Count > 0,
            MethodReadinessAuthorizes: false,
            PredicateSatisfactionBecomesWarrant: false,
            StewardReviewExecutes: false,
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

    private static StewardActionAdmissibilityDecision CreateDecision(
        string methodHandle,
        string actionHandle,
        string suffix) =>
        new(
            DecisionHandle: $"urn:san:steward-action-admissibility:{suffix}",
            MethodHandle: methodHandle,
            ActionHandle: actionHandle,
            DecisionClass: StewardAdmissibilityDecisionClass.MethodPrepared,
            StewardSurface: SanctuaryPacketSurfaces.Steward,
            CustodyOwner: SanctuaryPacketSurfaces.Steward,
            WitnessSurface: "witness:separate-custody",
            TelemetryRoute: SanctuaryPacketRoutes.TelemetryString,
            AuthorityCeiling: "ceiling:admissibility-review",
            RevocationPath: "revocation:steward-action-admissibility",
            LossCondition: "loss:admissibility-promotes-to-execution",
            ReviewOnly: true,
            RequiresSeparateEnactmentBoundary: true,
            AdmissibleForEnactmentReview: true,
            AuthorizesExecution: false,
            ExecutesAction: false,
            GrantsAuthority: false,
            AdmitsContinuity: false,
            ActivatesRuntime: false,
            EmitsPacket: false,
            EvaluatesLisp: false);

    private static StewardAdmissibilityPredicateResult[] CreatePredicateResults(
        string methodHandle,
        string actionHandle) =>
    [
        CreatePredicate(methodHandle, actionHandle, "method-ready"),
        CreatePredicate(methodHandle, actionHandle, "steward-custody-witness"),
        CreatePredicate(methodHandle, actionHandle, "separate-enactment-boundary")
    ];

    private static StewardAdmissibilityPredicateResult CreatePredicate(
        string methodHandle,
        string actionHandle,
        string suffix) =>
        new(
            PredicateHandle: $"urn:san:admissibility-predicate:{suffix}",
            MethodHandle: methodHandle,
            ActionHandle: actionHandle,
            PredicateCode: $"predicate:{suffix}",
            EvidenceHandle: $"urn:san:evidence:{suffix}",
            EvidenceBodyPresent: true,
            WitnessBodyPresent: true,
            PredicateSatisfied: true,
            SupportsAdmissibility: true,
            GrantsWarrant: false,
            AuthorizesExecution: false,
            EmitsPacket: false,
            EvaluatesLisp: false,
            ReplaysReceipt: false,
            IncrementsPassage: false,
            AdmitsContinuity: false);

    private static StewardActionAdmissibilityScopeBoundary CreateScope() =>
        new(
            ScopeCode: "steward-action-admissibility-review-only",
            Present: true,
            ReviewOnly: true,
            RequiresSeparateEnactmentBoundary: true,
            AdmissibilityIsExecution: false,
            StewardAcceptanceIsRuntimeMotion: false,
            AdmissibilityGrantsAuthority: false,
            AdmissibilityAdmitsContinuity: false,
            AllowsRuntimeAction: false,
            AllowsActivation: false,
            AllowsLispEvaluation: false,
            AllowsPacketEmission: false,
            AllowsReceiptReplay: false,
            AllowsPassageIncrement: false);

    private static StewardActionAdmissibilityDecision MutateDecision(
        StewardActionAdmissibilityDecision decision,
        string mutation) =>
        mutation switch
        {
            "missing-steward" => decision with { StewardSurface = string.Empty },
            "missing-custody" => decision with { CustodyOwner = string.Empty },
            "missing-witness" => decision with { WitnessSurface = string.Empty },
            "missing-telemetry" => decision with { TelemetryRoute = string.Empty },
            "missing-ceiling" => decision with { AuthorityCeiling = string.Empty },
            "missing-revocation" => decision with { RevocationPath = string.Empty },
            "missing-loss" => decision with { LossCondition = string.Empty },
            "no-enactment-boundary" => decision with { RequiresSeparateEnactmentBoundary = false },
            "not-admissible" => decision with { AdmissibleForEnactmentReview = false },
            "authorizes" => decision with { AuthorizesExecution = true },
            "executes" => decision with { ExecutesAction = true },
            "authority" => decision with { GrantsAuthority = true },
            "continuity" => decision with { AdmitsContinuity = true },
            "activation" => decision with { ActivatesRuntime = true },
            "packet" => decision with { EmitsPacket = true },
            "lisp" => decision with { EvaluatesLisp = true },
            _ => decision
        };

    private static StewardAdmissibilityPredicateResult MutatePredicate(
        StewardAdmissibilityPredicateResult predicate,
        string mutation) =>
        mutation switch
        {
            "missing-evidence" => predicate with { EvidenceBodyPresent = false },
            "missing-witness" => predicate with { WitnessBodyPresent = false },
            "not-satisfied" => predicate with { PredicateSatisfied = false },
            "not-supporting" => predicate with { SupportsAdmissibility = false },
            "warrant" => predicate with { GrantsWarrant = true },
            "authorizes" => predicate with { AuthorizesExecution = true },
            "packet" => predicate with { EmitsPacket = true },
            "lisp" => predicate with { EvaluatesLisp = true },
            "replay" => predicate with { ReplaysReceipt = true },
            "passage" => predicate with { IncrementsPassage = true },
            "continuity" => predicate with { AdmitsContinuity = true },
            "method-lineage-mismatch" => predicate with { MethodHandle = "urn:san:action-method:missing" },
            "action-lineage-mismatch" => predicate with { ActionHandle = "urn:san:typed-action:missing" },
            _ => predicate
        };

    private static StewardActionAdmissibilityScopeBoundary MutateScope(
        StewardActionAdmissibilityScopeBoundary scope,
        string mutation) =>
        mutation switch
        {
            "no-enactment-boundary" => scope with { RequiresSeparateEnactmentBoundary = false },
            "admissibility-executes" => scope with { AdmissibilityIsExecution = true },
            "acceptance-runtime" => scope with { StewardAcceptanceIsRuntimeMotion = true },
            "authority" => scope with { AdmissibilityGrantsAuthority = true },
            "continuity" => scope with { AdmissibilityAdmitsContinuity = true },
            "runtime" => scope with { AllowsRuntimeAction = true },
            "activation" => scope with { AllowsActivation = true },
            "lisp" => scope with { AllowsLispEvaluation = true },
            "packet" => scope with { AllowsPacketEmission = true },
            "replay" => scope with { AllowsReceiptReplay = true },
            "passage" => scope with { AllowsPassageIncrement = true },
            _ => scope
        };

    private static void AssertCold(StewardActionAdmissibilityReceipt receipt)
    {
        Assert.True(receipt.IsColdStewardActionAdmissibility);
        Assert.True(receipt.ReviewOnly);
        Assert.True(receipt.SeparateEnactmentBoundaryRequired);
        Assert.Equal(receipt.PriorPassageCount, receipt.PassageCountAfterAdmissibilityReview);
        Assert.False(receipt.AdmissibilityExecutes);
        Assert.False(receipt.StewardAcceptanceMovesRuntime);
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
        StewardActionAdmissibilityReceipt receipt,
        string outcomeCode)
    {
        Assert.Equal(StewardActionAdmissibilityDisposition.Refused, receipt.Disposition);
        Assert.Equal(outcomeCode, receipt.OutcomeCode);
        Assert.True(receipt.IsRetainedStewardActionAdmissibilityRefusal);
        Assert.NotNull(receipt.Refusal);
        Assert.True(receipt.Refusal!.Retained);
        Assert.Empty(receipt.Decisions);
        Assert.Empty(receipt.PredicateResults);
        Assert.False(receipt.AdmissibleForEnactmentReview);
        Assert.True(receipt.SeparateEnactmentBoundaryRequired);
        Assert.False(receipt.AdmissibilityExecutes);
        Assert.False(receipt.StewardAcceptanceMovesRuntime);
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
            var candidate = Path.Combine(directory.FullName, "src", "SLI", "SLI.Lisp", "steward-action-admissibility.lisp");
            if (File.Exists(candidate))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        throw new DirectoryNotFoundException("Could not locate Project Bicycle repository root.");
    }
}
