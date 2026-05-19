using San.Common;
using SLI.Runtime;
using Xunit;

namespace San.Audit.Tests;

public sealed class GovernedReturnReceiptReplayPolicyTests
{
    private static readonly DateTimeOffset TimestampUtc = DateTimeOffset.Parse("2026-05-07T00:00:00Z");

    [Theory]
    [MemberData(nameof(ReturnFamilies))]
    public void All_Governed_Return_Families_Can_Be_Represented_For_Review(GovernedReturnReceiptFamily family)
    {
        var evaluation = Evaluate(CreateRequest(returnReceipt: CreateReturnReceipt(family)));

        Assert.Equal(GovernedReturnReplayReviewDisposition.ReviewAdmitted, evaluation.Disposition);
        Assert.Equal(family, evaluation.ReturnFamily);
        Assert.True(evaluation.ReviewOnly);
        AssertForbiddenRuntimeFlagsFalse(evaluation);
    }

    [Fact]
    public void Candidate_Only_Recomposition_Route_Admits_Review_Without_Landing()
    {
        var candidate = CreateCandidate(RecompositionCandidateDisposition.CandidateOnly);

        var evaluation = Evaluate(CreateRequest(recompositionCandidate: candidate));

        Assert.Equal(GovernedReturnReplayReviewDisposition.ReviewAdmitted, evaluation.Disposition);
        Assert.Equal("governed-return-replay-review-admitted", evaluation.OutcomeCode);
        Assert.Equal(candidate.CandidateId, evaluation.CandidateId);
        Assert.True(evaluation.ReviewOnly);
        AssertForbiddenRuntimeFlagsFalse(evaluation);
    }

    [Fact]
    public void Withheld_Candidate_Remains_Non_Executing()
    {
        var candidate = CreateCandidate(RecompositionCandidateDisposition.Withheld);

        var evaluation = Evaluate(CreateRequest(recompositionCandidate: candidate));

        Assert.Equal(GovernedReturnReplayReviewDisposition.ReviewAdmitted, evaluation.Disposition);
        Assert.True(evaluation.ReviewOnly);
        AssertForbiddenRuntimeFlagsFalse(evaluation);
    }

    [Theory]
    [InlineData("materialization", MaterializationEligibility.Restricted, PersistenceEligibility.Never, true, "recomposition-materialization-blocked")]
    [InlineData("persistence", MaterializationEligibility.No, PersistenceEligibility.AuditOnly, true, "recomposition-persistence-blocked")]
    [InlineData("missing-membrane-reentry", MaterializationEligibility.No, PersistenceEligibility.Never, false, "membrane-reentry-required")]
    public void Recomposition_Drift_Is_Refused(
        string caseId,
        MaterializationEligibility materialization,
        PersistenceEligibility persistence,
        bool requiresMembraneReentry,
        string outcomeCode)
    {
        var candidate = CreateCandidate(
            RecompositionCandidateDisposition.CandidateOnly,
            materialization,
            persistence,
            requiresMembraneReentry);

        var evaluation = Evaluate(CreateRequest(recompositionCandidate: candidate));

        Assert.Equal(GovernedReturnReplayReviewDisposition.ReviewRefused, evaluation.Disposition);
        Assert.Equal(outcomeCode, evaluation.OutcomeCode);
        Assert.False(string.IsNullOrWhiteSpace(caseId));
        Assert.False(evaluation.ReviewOnly);
        AssertForbiddenRuntimeFlagsFalse(evaluation);
    }

    [Fact]
    public void Field_Query_Review_Preserves_Passport_Truth_And_Authority_Ceiling()
    {
        var fieldQuery = CreateFieldQueryResult();

        var evaluation = Evaluate(CreateRequest(fieldQueryResult: fieldQuery));

        Assert.Equal(GovernedReturnReplayReviewDisposition.ReviewAdmitted, evaluation.Disposition);
        Assert.Equal(fieldQuery.Query.QueryId, evaluation.FieldQueryId);
        Assert.True(fieldQuery.TensionSummary.PassportTruthPreserved);
        Assert.True(fieldQuery.TensionSummary.AuthorityCeilingPreserved);
        Assert.True(fieldQuery.TensionSummary.MembraneReentryRequired);
        Assert.True(fieldQuery.MembraneReentryRequired);
        AssertForbiddenRuntimeFlagsFalse(evaluation);
    }

    [Fact]
    public void Field_Query_Drift_Is_Refused()
    {
        var fieldQuery = CreateFieldQueryResult(passportTruthPreserved: false);

        var evaluation = Evaluate(CreateRequest(fieldQueryResult: fieldQuery));

        Assert.Equal(GovernedReturnReplayReviewDisposition.ReviewRefused, evaluation.Disposition);
        Assert.Equal("field-query-passport-or-authority-drift-blocked", evaluation.OutcomeCode);
        AssertForbiddenRuntimeFlagsFalse(evaluation);
    }

    [Fact]
    public void Denied_Escalation_Transition_Is_Refused()
    {
        var decision = new SliEscalationTransitionDecision(
            Disposition: EscalationTransitionDisposition.Denied,
            OutcomeCode: "transition-not-permitted",
            GovernanceTrace: "bounded state grammar denied transition",
            TimestampUtc: TimestampUtc);

        var evaluation = Evaluate(CreateRequest(escalationTransitionDecision: decision));

        Assert.Equal(GovernedReturnReplayReviewDisposition.ReviewRefused, evaluation.Disposition);
        Assert.Equal("escalation-transition-denied", evaluation.OutcomeCode);
        AssertForbiddenRuntimeFlagsFalse(evaluation);
    }

    [Fact]
    public void Hitl_Hold_Without_Witness_Token_Is_Withheld()
    {
        var escalation = CreateEscalationPacket(SliEscalationState.HitlHold, hitlRequired: true);

        var evaluation = Evaluate(CreateRequest(escalationPacket: escalation));

        Assert.Equal(GovernedReturnReplayReviewDisposition.ReviewWithheld, evaluation.Disposition);
        Assert.Equal("hitl-governed-return-witness-token-required", evaluation.OutcomeCode);
        Assert.False(evaluation.ReviewOnly);
        AssertForbiddenRuntimeFlagsFalse(evaluation);
    }

    [Fact]
    public void Hitl_Hold_With_Governed_Return_Witness_Token_Admits_Review_Only()
    {
        var escalation = CreateEscalationPacket(SliEscalationState.HitlHold, hitlRequired: true);
        var token = CreateHitlWitnessToken(HitlHoldExitRoute.GovernedReturn);

        var evaluation = Evaluate(CreateRequest(escalationPacket: escalation, hitlWitnessToken: token));

        Assert.Equal(GovernedReturnReplayReviewDisposition.ReviewAdmitted, evaluation.Disposition);
        Assert.Equal(token.TokenId, evaluation.HitlWitnessTokenId);
        Assert.True(evaluation.ReviewOnly);
        AssertForbiddenRuntimeFlagsFalse(evaluation);
    }

    [Fact]
    public void Hitl_Hold_With_Non_Governed_Return_Token_Is_Withheld()
    {
        var escalation = CreateEscalationPacket(SliEscalationState.HitlHold, hitlRequired: true);
        var token = CreateHitlWitnessToken(HitlHoldExitRoute.Refusal);

        var evaluation = Evaluate(CreateRequest(escalationPacket: escalation, hitlWitnessToken: token));

        Assert.Equal(GovernedReturnReplayReviewDisposition.ReviewWithheld, evaluation.Disposition);
        Assert.Equal("hitl-governed-return-witness-token-required", evaluation.OutcomeCode);
        Assert.False(evaluation.ReviewOnly);
        AssertForbiddenRuntimeFlagsFalse(evaluation);
    }

    [Theory]
    [MemberData(nameof(RequestedMotionCases))]
    public void Requested_Runtime_Motion_Is_Refused(string requestedMotion)
    {
        var evaluation = Evaluate(CreateRequestWithRequestedMotion(requestedMotion));

        Assert.Equal(GovernedReturnReplayReviewDisposition.ReviewRefused, evaluation.Disposition);
        Assert.Equal("runtime-landing-or-execution-requested", evaluation.OutcomeCode);
        Assert.False(evaluation.ReviewOnly);
        AssertForbiddenRuntimeFlagsFalse(evaluation);
    }

    [Theory]
    [MemberData(nameof(RepresentativeOutcomes))]
    public void Forbidden_Runtime_Output_Flags_Remain_False_For_All_Outcomes(
        string caseId,
        GovernedReturnReceiptReplayRequest request,
        GovernedReturnReplayReviewDisposition expectedDisposition)
    {
        var evaluation = Evaluate(request);

        Assert.Equal(expectedDisposition, evaluation.Disposition);
        Assert.False(string.IsNullOrWhiteSpace(caseId));
        AssertForbiddenRuntimeFlagsFalse(evaluation);
    }

    public static IEnumerable<object[]> ReturnFamilies() =>
        Enum.GetValues<GovernedReturnReceiptFamily>()
            .Select(static family => new object[] { family });

    public static IEnumerable<object[]> RequestedMotionCases()
    {
        yield return ["runtime-landing"];
        yield return ["replay-execution"];
        yield return ["db-write"];
        yield return ["ec-start"];
        yield return ["runtime-identity"];
        yield return ["runtime-action"];
        yield return ["recomposition-execution"];
        yield return ["authority-widening"];
        yield return ["gel-promotion"];
    }

    public static IEnumerable<object[]> RepresentativeOutcomes()
    {
        yield return
        [
            "admitted",
            CreateRequest(),
            GovernedReturnReplayReviewDisposition.ReviewAdmitted
        ];

        yield return
        [
            "withheld",
            CreateRequest(
                escalationPacket: CreateEscalationPacket(SliEscalationState.HitlHold, hitlRequired: true)),
            GovernedReturnReplayReviewDisposition.ReviewWithheld
        ];

        yield return
        [
            "refused",
            CreateRequest(
                recompositionCandidate: CreateCandidate(
                    RecompositionCandidateDisposition.CandidateOnly,
                    MaterializationEligibility.Yes)),
            GovernedReturnReplayReviewDisposition.ReviewRefused
        ];
    }

    private static GovernedReturnReceiptReplayEvaluation Evaluate(GovernedReturnReceiptReplayRequest request) =>
        new DefaultGovernedReturnReceiptReplayPolicy().Evaluate(request, TimestampUtc);

    private static GovernedReturnReceiptReplayRequest CreateRequest(
        GovernedReturnReceipt? returnReceipt = null,
        FieldQueryResult? fieldQueryResult = null,
        RecompositionCandidate? recompositionCandidate = null,
        RecompositionCandidateEvaluationDecision? recompositionEvaluationDecision = null,
        SliEscalationPacket? escalationPacket = null,
        SliEscalationTransitionDecision? escalationTransitionDecision = null,
        HitlHoldWitnessToken? hitlWitnessToken = null,
        bool runtimeLandingRequested = false,
        bool replayExecutionRequested = false,
        bool dbWriteRequested = false,
        bool ecStartRequested = false,
        bool runtimeIdentityRequested = false,
        bool runtimeActionRequested = false,
        bool recompositionExecutionRequested = false,
        bool authorityWideningRequested = false,
        bool gelPromotionRequested = false) =>
        new(
            ReturnReceipt: returnReceipt ?? CreateReturnReceipt(),
            FieldQueryResult: fieldQueryResult,
            RecompositionCandidate: recompositionCandidate,
            RecompositionEvaluationDecision: recompositionEvaluationDecision,
            EscalationPacket: escalationPacket,
            EscalationTransitionDecision: escalationTransitionDecision,
            HitlWitnessToken: hitlWitnessToken,
            RuntimeLandingRequested: runtimeLandingRequested,
            ReplayExecutionRequested: replayExecutionRequested,
            DbWriteRequested: dbWriteRequested,
            EcStartRequested: ecStartRequested,
            RuntimeIdentityRequested: runtimeIdentityRequested,
            RuntimeActionRequested: runtimeActionRequested,
            RecompositionExecutionRequested: recompositionExecutionRequested,
            AuthorityWideningRequested: authorityWideningRequested,
            GelPromotionRequested: gelPromotionRequested);

    private static GovernedReturnReceiptReplayRequest CreateRequestWithRequestedMotion(string requestedMotion) =>
        requestedMotion switch
        {
            "runtime-landing" => CreateRequest(runtimeLandingRequested: true),
            "replay-execution" => CreateRequest(replayExecutionRequested: true),
            "db-write" => CreateRequest(dbWriteRequested: true),
            "ec-start" => CreateRequest(ecStartRequested: true),
            "runtime-identity" => CreateRequest(runtimeIdentityRequested: true),
            "runtime-action" => CreateRequest(runtimeActionRequested: true),
            "recomposition-execution" => CreateRequest(recompositionExecutionRequested: true),
            "authority-widening" => CreateRequest(authorityWideningRequested: true),
            "gel-promotion" => CreateRequest(gelPromotionRequested: true),
            _ => throw new ArgumentOutOfRangeException(nameof(requestedMotion), requestedMotion, "unsupported requested motion fixture.")
        };

    private static GovernedReturnReceipt CreateReturnReceipt(
        GovernedReturnReceiptFamily family = GovernedReturnReceiptFamily.PermissionReceipt) =>
        new(
            ReceiptId: $"governed-return://sw06/{family}",
            ReturnFamily: family,
            SourceReviewJurisdiction: "local-cradle",
            SourceStateLineage: "state-lineage://sw06-fixture",
            TraceId: "trace://sw06-governed-return",
            AdmissibilityPosture: AdmissibilityStatus.Admissible,
            AuthorityCeiling: "review-only",
            BurdenOfReturn: "represent-only",
            RequiredAcknowledgement: GovernedReturnAcknowledgementBurden.AcknowledgeOnly,
            ExpiryOrRepresentWindow: "single-review-window",
            LocalTransformabilityRule: GovernedReturnTransformabilityRule.RenderOnly,
            TimestampUtc: TimestampUtc);

    private static RecompositionCandidate CreateCandidate(
        RecompositionCandidateDisposition disposition,
        MaterializationEligibility materialization = MaterializationEligibility.No,
        PersistenceEligibility persistence = PersistenceEligibility.Never,
        bool requiresMembraneReentry = true) =>
        new(
            CandidateId: $"recomposition-candidate://sw06/{disposition}/{materialization}/{persistence}/{requiresMembraneReentry}",
            QueryId: "field-query://sw06",
            CandidateClass: RecompositionCandidateClass.FieldRecall,
            Disposition: disposition,
            Sources: disposition == RecompositionCandidateDisposition.CandidateOnly
                ? [CreateProvenance()]
                : [],
            TensionSummary: CreateTensionSummary(),
            Admissibility: AdmissibilityStatus.Pending,
            ContradictionState: ContradictionState.None,
            MaterializationEligibility: materialization,
            PersistenceEligibility: persistence,
            RequiresMembraneReentry: requiresMembraneReentry,
            CreatedAtUtc: TimestampUtc);

    private static RecompositionCandidateProvenance CreateProvenance() =>
        new(
            ProductId: "product://sw06",
            ReceiptId: "receipt://sw06-source",
            WitnessSnapshotId: "witness://sw06-source",
            SourceTraceId: "trace://sw06-source",
            Lane: MembraneDispatchLane.Accepted,
            Family: new SymbolicProductFamily("sw06-governed-return"),
            ProductClass: SymbolicProductClass.CandidateProduct,
            Admissibility: AdmissibilityStatus.Admissible,
            ContradictionState: ContradictionState.None,
            ReceivedAtUtc: TimestampUtc);

    private static FieldQueryResult CreateFieldQueryResult(
        bool passportTruthPreserved = true,
        bool authorityCeilingPreserved = true,
        bool membraneReentryRequired = true) =>
        new(
            Query: new FieldQuery(
                QueryId: "field-query://sw06",
                RequestedByTraceId: "trace://sw06-field-query",
                Axes: [FieldQueryAxis.Family],
                Family: new SymbolicProductFamily("sw06-governed-return"),
                ProductClass: null,
                Intent: null,
                Admissibility: null,
                ContradictionState: null,
                LaneScope: null,
                Origin: null,
                TraceLineagePrefix: null,
                TemporalWindow: null,
                RequestedAtUtc: TimestampUtc),
            Matches: [],
            TensionSummary: CreateTensionSummary(
                passportTruthPreserved,
                authorityCeilingPreserved,
                membraneReentryRequired),
            MembraneReentryRequired: membraneReentryRequired,
            EvaluatedAtUtc: TimestampUtc);

    private static QueryTensionSummary CreateTensionSummary(
        bool passportTruthPreserved = true,
        bool authorityCeilingPreserved = true,
        bool membraneReentryRequired = true) =>
        new(
            ActiveAxes: [FieldQueryAxis.Family],
            TensionState: QueryTensionState.Stable,
            SourceCount: 1,
            MatchCount: 1,
            WithheldCount: 0,
            PassportTruthPreserved: passportTruthPreserved,
            AuthorityCeilingPreserved: authorityCeilingPreserved,
            MembraneReentryRequired: membraneReentryRequired,
            Notes:
            [
                new QueryTensionNote(
                    Code: QueryTensionNoteCodes.PassportTruthPreserved,
                    Message: "passport truth preserved for SW-06 replay review fixture."),
                new QueryTensionNote(
                    Code: QueryTensionNoteCodes.AuthorityCeilingPreserved,
                    Message: "authority ceiling preserved for SW-06 replay review fixture.")
            ]);

    private static SliEscalationPacket CreateEscalationPacket(
        SliEscalationState state,
        bool hitlRequired) =>
        new(
            TraceId: "trace://sw06-escalation",
            State: state,
            Jurisdiction: SliEscalationJurisdiction.Steward,
            Admissibility: AdmissibilityStatus.Pending,
            BurdenOfReview: "governed-return-review",
            HitlRequired: hitlRequired,
            TimestampUtc: TimestampUtc);

    private static HitlHoldWitnessToken CreateHitlWitnessToken(HitlHoldExitRoute authorizedExit) =>
        new(
            TokenId: "hitl-token://sw06-governed-return",
            TokenClass: HitlHoldWitnessTokenClass.RepresentToken,
            WitnessRole: HitlHoldWitnessRole.ReviewWitness,
            IssuerSurface: "Steward",
            IssuerJurisdiction: "local-cradle",
            HoldTraceId: "trace://sw06-escalation",
            StateLineage: "state-lineage://sw06-hitl",
            AuthorizedExit: authorizedExit,
            ExpiryOrReuseRule: "single-use",
            IssuedAtUtc: TimestampUtc);

    private static void AssertForbiddenRuntimeFlagsFalse(GovernedReturnReceiptReplayEvaluation evaluation)
    {
        Assert.False(evaluation.RuntimeLandingAllowed);
        Assert.False(evaluation.ReplayExecutionAllowed);
        Assert.False(evaluation.RuntimeIdentityEmitted);
        Assert.False(evaluation.RuntimeActionExecuted);
        Assert.False(evaluation.DbWriteAllowed);
        Assert.False(evaluation.EcStartAllowed);
        Assert.False(evaluation.RecompositionExecutionAllowed);
        Assert.False(evaluation.AuthorityWideningAllowed);
        Assert.False(evaluation.GelPromotionAllowed);
    }
}
