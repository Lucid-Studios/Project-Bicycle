using San.Common;
using Xunit;

namespace San.Audit.Tests;

public sealed class EcPrecipitationWitnessBoundaryValidationTests
{
    private static readonly DateTimeOffset TimestampUtc = DateTimeOffset.Parse("2026-05-17T00:00:00Z");

    [Fact]
    public void Meaningful_EC_Residue_May_Be_Witnessed_As_SelfGEL_Candidate_Only()
    {
        var receipt = Declare(CreateRequest());

        Assert.Equal(EcPrecipitationWitnessDisposition.WitnessedCandidateCold, receipt.Disposition);
        Assert.Equal("ec-precipitation-witness-candidate-review-only", receipt.OutcomeCode);
        Assert.True(receipt.ActiveWitnessPerformed);
        Assert.Equal(2, receipt.RetainedResidueCandidateCount);
        Assert.Equal(2, receipt.CandidateSplineCount);
        Assert.Contains("Meaningful EC residue", receipt.GovernanceTrace, StringComparison.Ordinal);
        AssertCold(receipt);
    }

    [Fact]
    public void Small_Anabelian_Law_Reconstructs_Candidacy_From_Witnessed_Relation_Not_Raw_EC()
    {
        var source = CreateSourceDryRunReceipt(caseCount: 3);
        var receipt = Declare(CreateRequest(source: source, priorPassageCount: 404));

        AssertCold(receipt);
        Assert.Equal(source.ReceiptHandle, receipt.SourceDryRunReceiptHandle);
        Assert.Equal(404, receipt.PassageCountAfterWitness);
        Assert.All(receipt.ResidueCandidates, residue =>
        {
            var dryRun = source.DryRunCases.Single(item => item.RehearsalHandle == residue.SourceRehearsalHandle);

            Assert.Equal(dryRun.SourceReadinessHandle, residue.SourceReadinessHandle);
            Assert.Equal(dryRun.SourcePacketHandle, residue.SourcePacketHandle);
            Assert.Equal(dryRun.DryRunPlanHandle, residue.SourceDryRunPlanHandle);
            Assert.Equal(dryRun.WitnessHandle, residue.WitnessHandle);
            Assert.Equal(dryRun.TelemetryRoute, residue.TelemetryRoute);
            Assert.False(residue.RawEcBecomesSelfGel);
            Assert.False(residue.MeaningBecomesAdmission);
            Assert.False(residue.RepetitionBecomesContinuity);
            Assert.False(residue.WitnessBecomesAuthority);
        });
    }

    [Fact]
    public void Maximal_Truth_Seeking_Seeks_Reconstructable_Truth_Without_Maximal_Truth_Claiming()
    {
        var receipt = Declare(CreateRequest());

        AssertCold(receipt);
        Assert.Contains("raw EC as SelfGEL", receipt.GovernanceTrace, StringComparison.Ordinal);
        Assert.False(receipt.RawEcBecameSelfGel);
        Assert.False(receipt.MeaningBecameAdmission);
        Assert.False(receipt.RepetitionBecameContinuity);
        Assert.False(receipt.AuthorityGranted);
        Assert.False(receipt.ContinuityAdmitted);
    }

    [Fact]
    public void Empty_EC_Precipitation_Witness_Is_Reviewable_But_Not_Candidate()
    {
        var source = CreateSourceDryRunReceipt(caseCount: 0);
        var receipt = Declare(CreateRequest(source: source, residues: [], routes: []));

        Assert.Equal(EcPrecipitationWitnessDisposition.EmptyWitnessCold, receipt.Disposition);
        Assert.Equal("ec-precipitation-witness-empty-review-only", receipt.OutcomeCode);
        Assert.Empty(receipt.ResidueCandidates);
        Assert.Empty(receipt.ActiveWitnessRoutes);
        Assert.Equal(0, receipt.CandidateSplineCount);
        Assert.False(receipt.ActiveWitnessPerformed);
        AssertCold(receipt);
    }

    [Theory]
    [InlineData("missing-source")]
    [InlineData("non-cold-source")]
    public void Source_Dry_Run_Receipt_Is_Required_Before_EC_Residue_May_Be_Witnessed(string sourceCase)
    {
        var receipt = sourceCase == "missing-source"
            ? Declare(CreateRequest(omitSource: true))
            : Declare(CreateRequest(source: CreateSourceDryRunReceipt(caseCount: 2) with { DryRunAuthorizedAction = true }));

        AssertRefused(receipt, "ec-precipitation-witness-source-dry-run-missing");
    }

    [Theory]
    [InlineData("missing-boundary")]
    [InlineData("not-review")]
    [InlineData("no-witness")]
    [InlineData("no-dry-run")]
    [InlineData("no-meaning")]
    [InlineData("no-active-witness")]
    [InlineData("no-cooling")]
    [InlineData("no-steward-review")]
    [InlineData("no-lineage")]
    [InlineData("no-conditional-context")]
    [InlineData("no-candidate-spline")]
    [InlineData("raw-ec-selfgel")]
    [InlineData("meaning-admission")]
    [InlineData("repetition-continuity")]
    [InlineData("witness-authority")]
    [InlineData("selfgel-mutation")]
    [InlineData("continuity")]
    [InlineData("authorize")]
    [InlineData("lisp")]
    [InlineData("packet")]
    [InlineData("replay")]
    [InlineData("passage")]
    [InlineData("activation")]
    public void Scope_Refuses_Precipitation_As_SelfGEL_Admission_Or_Action(string mutation)
    {
        var receipt = Declare(CreateRequest(scope: MutateScope(CreateScope(), mutation)));

        var expected = mutation == "missing-boundary"
            ? "ec-precipitation-witness-scope-missing"
            : "ec-precipitation-witness-scope-promotional";
        AssertRefused(receipt, expected);
    }

    [Theory]
    [InlineData("missing-law")]
    [InlineData("raw-ec-selfgel")]
    [InlineData("meaning-admission")]
    [InlineData("repetition-continuity")]
    [InlineData("emotion-truth")]
    [InlineData("witness-authority")]
    [InlineData("selfgel-mutation")]
    [InlineData("oe-mutation")]
    [InlineData("gel-promotion")]
    [InlineData("authorize")]
    [InlineData("lisp")]
    [InlineData("packet")]
    [InlineData("replay")]
    [InlineData("passage")]
    [InlineData("activation")]
    [InlineData("no-active-witness")]
    [InlineData("no-steward")]
    [InlineData("no-cooling")]
    [InlineData("no-return")]
    [InlineData("no-compost")]
    public void Non_Collapse_Boundary_Refuses_Witness_As_Promotion(string mutation)
    {
        var receipt = Declare(CreateRequest(nonCollapse: MutateNonCollapse(CreateNonCollapse(), mutation)));

        AssertRefused(receipt, "ec-precipitation-witness-non-collapse-invalid");
    }

    [Theory]
    [InlineData("missing-residue")]
    [InlineData("missing-rehearsal")]
    [InlineData("missing-readiness")]
    [InlineData("missing-packet")]
    [InlineData("missing-plan")]
    [InlineData("missing-meaning")]
    [InlineData("missing-spline")]
    [InlineData("missing-cselfgel")]
    [InlineData("missing-coe")]
    [InlineData("missing-cooling")]
    [InlineData("missing-custody")]
    [InlineData("missing-witness")]
    [InlineData("missing-telemetry")]
    [InlineData("missing-steward")]
    [InlineData("missing-rationale")]
    [InlineData("zero-recurrence")]
    [InlineData("not-meaningful")]
    [InlineData("not-review")]
    [InlineData("not-candidate")]
    [InlineData("not-idle")]
    [InlineData("no-active-witness")]
    [InlineData("no-cooling")]
    [InlineData("no-steward")]
    [InlineData("no-dry-run-lineage")]
    [InlineData("no-context-lineage")]
    [InlineData("raw-ec-selfgel")]
    [InlineData("meaning-admission")]
    [InlineData("repetition-continuity")]
    [InlineData("emotion-truth")]
    [InlineData("witness-authority")]
    [InlineData("selfgel-mutation")]
    [InlineData("oe-mutation")]
    [InlineData("gel-promotion")]
    [InlineData("authorize")]
    [InlineData("lisp")]
    [InlineData("packet")]
    [InlineData("replay")]
    [InlineData("passage")]
    [InlineData("activation")]
    public void Residue_Candidate_Remains_Witnessed_Candidate_Only(string mutation)
    {
        var source = CreateSourceDryRunReceipt();
        var residues = CreateResidues(source);
        residues[0] = MutateResidue(residues[0], mutation);

        var receipt = Declare(CreateRequest(source: source, residues: residues, routes: CreateRoutes(residues)));

        AssertRefused(receipt, "ec-precipitation-witness-residue-invalid");
    }

    [Theory]
    [InlineData("missing-route")]
    [InlineData("missing-residue")]
    [InlineData("missing-rehearsal")]
    [InlineData("missing-spline")]
    [InlineData("missing-steward")]
    [InlineData("missing-evidence")]
    [InlineData("missing-witness")]
    [InlineData("missing-telemetry")]
    [InlineData("missing-return")]
    [InlineData("not-review")]
    [InlineData("not-witness")]
    [InlineData("no-residue-lineage")]
    [InlineData("no-dry-run-lineage")]
    [InlineData("no-spline-lineage")]
    [InlineData("no-steward-route")]
    [InlineData("no-cooling")]
    [InlineData("admit-selfgel")]
    [InlineData("continuity")]
    [InlineData("authority")]
    [InlineData("authorize")]
    [InlineData("identity")]
    [InlineData("lisp")]
    [InlineData("packet")]
    [InlineData("replay")]
    [InlineData("passage")]
    [InlineData("activation")]
    [InlineData("wrong-residue")]
    [InlineData("wrong-spline")]
    [InlineData("wrong-witness")]
    public void Active_Witness_Route_Remains_Witness_Only(string mutation)
    {
        var source = CreateSourceDryRunReceipt();
        var residues = CreateResidues(source);
        var routes = CreateRoutes(residues);
        routes[0] = MutateRoute(routes[0], mutation);

        var receipt = Declare(CreateRequest(source: source, residues: residues, routes: routes));

        AssertRefused(receipt, "ec-precipitation-witness-route-invalid");
    }

    [Fact]
    public void Duplicate_Residue_Handles_Refuse_Lineage_Collapse()
    {
        var source = CreateSourceDryRunReceipt();
        var residues = CreateResidues(source);
        residues[1] = residues[1] with { ResidueHandle = residues[0].ResidueHandle };

        var receipt = Declare(CreateRequest(source: source, residues: residues, routes: CreateRoutes(residues)));

        AssertRefused(receipt, "ec-precipitation-witness-duplicate-residue-handle");
    }

    [Fact]
    public void Duplicate_Candidate_Splines_Refuse_SelfGEL_Candidacy_Collapse()
    {
        var source = CreateSourceDryRunReceipt();
        var residues = CreateResidues(source);
        residues[1] = residues[1] with { CandidateSplineHandle = residues[0].CandidateSplineHandle };

        var receipt = Declare(CreateRequest(source: source, residues: residues, routes: CreateRoutes(residues)));

        AssertRefused(receipt, "ec-precipitation-witness-duplicate-candidate-spline");
    }

    [Fact]
    public void Lineage_Mismatch_Refuses_Raw_EC_To_SelfGEL_Shortcut()
    {
        var source = CreateSourceDryRunReceipt();
        var residues = CreateResidues(source);
        residues[0] = residues[0] with { SourcePacketHandle = "urn:san:scoped-work-packet:unwitnessed" };

        var receipt = Declare(CreateRequest(source: source, residues: residues, routes: CreateRoutes(residues)));

        AssertRefused(receipt, "ec-precipitation-witness-lineage-invalid");
    }

    [Fact]
    public void Missing_Active_Witness_Route_Refuses_Residue_Retention()
    {
        var source = CreateSourceDryRunReceipt();
        var residues = CreateResidues(source);
        var routes = CreateRoutes(residues).Skip(1).ToArray();

        var receipt = Declare(CreateRequest(source: source, residues: residues, routes: routes));

        AssertRefused(receipt, "ec-precipitation-witness-route-missing");
    }

    [Fact]
    public void Duplicate_Active_Witness_Routes_Refuse_Route_Collapse()
    {
        var source = CreateSourceDryRunReceipt();
        var residues = CreateResidues(source);
        var routes = CreateRoutes(residues);
        routes[1] = routes[1] with { WitnessRouteHandle = routes[0].WitnessRouteHandle };

        var receipt = Declare(CreateRequest(source: source, residues: residues, routes: routes));

        AssertRefused(receipt, "ec-precipitation-witness-duplicate-route-handle");
    }

    [Fact]
    public void Ninety_Residue_Field_May_Be_Witnessed_Without_Passage_Inflation_Or_Continuity_Admission()
    {
        var source = CreateSourceDryRunReceipt(caseCount: 90);
        var receipt = Declare(CreateRequest(source: source, priorPassageCount: 9090));

        AssertCold(receipt);
        Assert.Equal(90, receipt.RetainedResidueCandidateCount);
        Assert.Equal(90, receipt.CandidateSplineCount);
        Assert.Equal(9090, receipt.PassageCountAfterWitness);
        Assert.False(receipt.CandidateMutatedSelfGel);
        Assert.False(receipt.CandidatePromotedGel);
        Assert.False(receipt.ContinuityAdmitted);
        Assert.False(receipt.AuthorityGranted);
    }

    private static EcPrecipitationWitnessReceipt Declare(EcPrecipitationWitnessRequest request) =>
        new DefaultEcPrecipitationWitnessBoundaryValidator().Declare(request, TimestampUtc);

    private static EcPrecipitationWitnessRequest CreateRequest(
        EnactmentDryRunRehearsalReceipt? source = null,
        IReadOnlyList<EcPrecipitationResidueCandidate>? residues = null,
        IReadOnlyList<ActiveEcWitnessRoute>? routes = null,
        EcPrecipitationWitnessScopeBoundary? scope = null,
        EcPrecipitationNonCollapseBoundary? nonCollapse = null,
        int priorPassageCount = 700,
        bool omitSource = false)
    {
        source ??= omitSource ? null : CreateSourceDryRunReceipt();
        residues ??= source is null ? [] : CreateResidues(source);
        routes ??= CreateRoutes(residues);

        return new EcPrecipitationWitnessRequest(
            SourceDryRunReceipt: source,
            ResidueCandidates: residues,
            ActiveWitnessRoutes: routes,
            ScopeBoundary: scope ?? CreateScope(),
            NonCollapseBoundary: nonCollapse ?? CreateNonCollapse(),
            PriorPassageCount: priorPassageCount);
    }

    private static EnactmentDryRunRehearsalReceipt CreateSourceDryRunReceipt(int caseCount = 2)
    {
        var cases = Enumerable.Range(0, caseCount)
            .Select(index =>
            {
                var suffix = $"case-{index:000}";
                return new EnactmentDryRunCase(
                    RehearsalHandle: $"urn:san:enactment-dry-run-rehearsal:{suffix}",
                    SourceReadinessHandle: $"urn:san:enactment-boundary-readiness:{suffix}",
                    SourcePacketHandle: $"urn:san:scoped-work-packet:{suffix}",
                    DryRunPlanHandle: $"urn:san:dry-run-plan:enactment-boundary:{suffix}",
                    DutyStation: "lab-local-tiny-bicycle-review",
                    WorkSurface: "local-reversible-review-receipt",
                    IntendedWork: "rehearse-local-no-op-effect-without-enactment",
                    MethodCode: "dry-run-rehearsal-review-only",
                    SimulatedEffectHandle: $"urn:san:simulated-effect:dry-run-rehearsal:{suffix}",
                    RollbackProofHandle: $"urn:san:rollback-proof:dry-run-rehearsal:{suffix}",
                    CustodyOwner: SanctuaryPacketSurfaces.Steward,
                    WitnessHandle: $"urn:san:witness:dry-run-rehearsal:{suffix}",
                    TelemetryRoute: "telemetry:ec-precipitation-witness",
                    StewardReviewHandle: $"urn:san:steward-review:dry-run-rehearsal:{suffix}",
                    ReviewOnly: true,
                    SimulationOnly: true,
                    NoOpOnly: true,
                    LocalOnly: true,
                    ReversibleOnly: true,
                    RequiresRollbackProof: true,
                    RequiresStewardReview: true,
                    SimulationBecomesPermission: false,
                    DryRunAuthorizesAction: false,
                    DryRunExecutesAction: false,
                    DryRunMovesRuntime: false,
                    DryRunWritesOutsideReceiptSurface: false,
                    DryRunGrantsAuthority: false,
                    DryRunAdmitsContinuity: false,
                    DryRunEvaluatesLisp: false,
                    DryRunEmitsMembranePacket: false,
                    DryRunReplaysReceipt: false,
                    DryRunIncrementsPassage: false,
                    DryRunActivates: false);
            }).ToArray();
        var routes = cases.Select(dryRun =>
            new StewardDryRunReviewReceiptRoute(
                ReviewRouteHandle: dryRun.StewardReviewHandle.Replace("enactment-boundary", "dry-run-rehearsal", StringComparison.Ordinal),
                RehearsalHandle: dryRun.RehearsalHandle,
                SourceReadinessHandle: dryRun.SourceReadinessHandle,
                SourcePacketHandle: dryRun.SourcePacketHandle,
                StewardSurface: SanctuaryPacketSurfaces.Steward,
                CustodyOwner: dryRun.CustodyOwner,
                EvidenceHandle: $"urn:san:evidence:dry-run-rehearsal:{dryRun.RehearsalHandle.Split(':').Last()}",
                WitnessHandle: dryRun.WitnessHandle,
                TelemetryRoute: dryRun.TelemetryRoute,
                ReturnPathHandle: $"urn:san:repair:dry-run-rehearsal:{dryRun.RehearsalHandle.Split(':').Last()}",
                ReviewOnly: true,
                PreservesRehearsalLineage: true,
                PreservesReadinessLineage: true,
                PreservesPacketLineage: true,
                PreservesDryRunPlanLineage: true,
                RoutesToStewardDryRunReview: true,
                RequiresCooling: true,
                RouteAuthorizesAction: false,
                RouteExecutesAction: false,
                RouteMovesRuntime: false,
                RouteGrantsAuthority: false,
                RouteAdmitsContinuity: false,
                RouteEvaluatesLisp: false,
                RouteEmitsMembranePacket: false,
                RouteReplaysReceipt: false,
                RouteIncrementsPassage: false,
                RouteActivates: false)).ToArray();

        return new EnactmentDryRunRehearsalReceipt(
            ReceiptHandle: $"urn:san:enactment-dry-run-rehearsal:source:{caseCount}",
            Disposition: cases.Length == 0
                ? EnactmentDryRunRehearsalDisposition.EmptyRehearsalCold
                : EnactmentDryRunRehearsalDisposition.RehearsedCold,
            OutcomeCode: "enactment-dry-run-rehearsal-rehearsed-review-only",
            GovernanceTrace: "Source dry-run rehearsal for EC precipitation witness tests.",
            SourceReadinessReceiptHandle: "urn:san:enactment-boundary-readiness:source",
            DryRunCases: cases,
            StewardReviewRoutes: routes,
            ScopeBoundary: CreateDryRunScope(),
            NonEnactmentBoundary: CreateDryRunNonEnactment(),
            Refusal: null,
            PriorPassageCount: 88,
            PassageCountAfterDryRun: 88,
            RetainedDryRunCaseCount: cases.Length,
            ReviewOnly: true,
            SimulationOnly: true,
            NoOpOnly: true,
            DryRunRehearsed: cases.Length > 0,
            DryRunBecamePermission: false,
            DryRunAuthorizedAction: false,
            DryRunExecutedAction: false,
            DryRunMovedRuntime: false,
            DryRunWroteOutsideReceiptSurface: false,
            DryRunGrantedAuthority: false,
            DryRunAdmittedContinuity: false,
            StewardDryRunReviewMovedRuntime: false,
            ReversibleLocalEffectAuthorizedAction: false,
            LispEvaluationAllowed: false,
            NewMembranePacketEmitted: false,
            ReceiptsReplayed: false,
            ContinuityAdmitted: false,
            AuthorityGranted: false,
            ActivationRefused: true,
            TimestampUtc: TimestampUtc);
    }

    private static EcPrecipitationResidueCandidate[] CreateResidues(EnactmentDryRunRehearsalReceipt source) =>
        source.DryRunCases.Select((dryRun, index) =>
        {
            var suffix = dryRun.RehearsalHandle.Split(':').Last();
            return new EcPrecipitationResidueCandidate(
                ResidueHandle: $"urn:san:ec-residue:precipitation-witness:{suffix}",
                SourceRehearsalHandle: dryRun.RehearsalHandle,
                SourceReadinessHandle: dryRun.SourceReadinessHandle,
                SourcePacketHandle: dryRun.SourcePacketHandle,
                SourceDryRunPlanHandle: dryRun.DryRunPlanHandle,
                MeaningFormationHandle: $"urn:san:ec-meaning-formation:precipitation-witness:{suffix}",
                CandidateSplineHandle: $"urn:san:selfgel-candidate-spline:precipitation-witness:{suffix}",
                ConditionalSelfGelContextHandle: $"urn:san:cselfgel-context:precipitation-witness:{suffix}",
                ConditionalOeContextHandle: $"urn:san:coe-context:precipitation-witness:{suffix}",
                CompassCoolingHandle: $"urn:san:compass-cooling:precipitation-witness:{suffix}",
                CustodyOwner: dryRun.CustodyOwner,
                WitnessHandle: dryRun.WitnessHandle,
                TelemetryRoute: dryRun.TelemetryRoute,
                StewardWitnessHandle: $"urn:san:steward-witness:ec-precipitation:{suffix}",
                SignificanceRationale: $"dry-run-residue-survived-review-{index}",
                RecurrenceCount: index + 1,
                MeaningfulEnoughForWitness: true,
                ReviewOnly: true,
                CandidateOnly: true,
                IdleEcOnly: true,
                ActiveWitnessRequired: true,
                CompassCoolingRequired: true,
                StewardReviewRequired: true,
                PreservesDryRunLineage: true,
                PreservesConditionalContextLineage: true,
                RawEcBecomesSelfGel: false,
                MeaningBecomesAdmission: false,
                RepetitionBecomesContinuity: false,
                EmotionBecomesTruth: false,
                WitnessBecomesAuthority: false,
                CandidateMutatesSelfGel: false,
                CandidateMutatesOe: false,
                CandidatePromotesGel: false,
                CandidateAuthorizesAction: false,
                CandidateEvaluatesLisp: false,
                CandidateEmitsMembranePacket: false,
                CandidateReplaysReceipt: false,
                CandidateIncrementsPassage: false,
                CandidateActivates: false);
        }).ToArray();

    private static ActiveEcWitnessRoute[] CreateRoutes(IReadOnlyList<EcPrecipitationResidueCandidate> residues) =>
        residues.Select(residue =>
            new ActiveEcWitnessRoute(
                WitnessRouteHandle: residue.ResidueHandle.Replace("ec-residue", "active-ec-witness-route", StringComparison.Ordinal),
                SourceResidueHandle: residue.ResidueHandle,
                SourceRehearsalHandle: residue.SourceRehearsalHandle,
                CandidateSplineHandle: residue.CandidateSplineHandle,
                StewardSurface: SanctuaryPacketSurfaces.Steward,
                EvidenceHandle: $"urn:san:evidence:ec-precipitation-witness:{residue.ResidueHandle.Split(':').Last()}",
                WitnessHandle: residue.WitnessHandle,
                TelemetryRoute: residue.TelemetryRoute,
                ReturnPathHandle: $"urn:san:return:ec-precipitation-witness:{residue.ResidueHandle.Split(':').Last()}",
                ReviewOnly: true,
                WitnessOnly: true,
                PreservesResidueLineage: true,
                PreservesDryRunLineage: true,
                PreservesCandidateSplineLineage: true,
                RoutesToStewardAdmissibilityReview: true,
                RequiresCompassCooling: true,
                RouteAdmitsSelfGel: false,
                RouteAdmitsContinuity: false,
                RouteGrantsAuthority: false,
                RouteAuthorizesAction: false,
                RouteMutatesIdentity: false,
                RouteEvaluatesLisp: false,
                RouteEmitsMembranePacket: false,
                RouteReplaysReceipt: false,
                RouteIncrementsPassage: false,
                RouteActivates: false)).ToArray();

    private static EnactmentDryRunScopeBoundary CreateDryRunScope() =>
        new(
            BoundaryCode: "enactment-dry-run-rehearsal-review-only",
            Present: true,
            ReviewOnly: true,
            AllowsDryRunRehearsal: true,
            RequiresReadinessReceipt: true,
            RequiresDryRunPlan: true,
            RequiresSimulatedEffect: true,
            RequiresRollbackProof: true,
            RequiresNoOp: true,
            RequiresLocality: true,
            RequiresReversibility: true,
            RequiresCustody: true,
            RequiresWitness: true,
            RequiresTelemetryRoute: true,
            RequiresStewardReview: true,
            AllowsSimulationAsPermission: false,
            AllowsActionAuthorization: false,
            AllowsExecution: false,
            AllowsRuntimeMotion: false,
            AllowsOutsideReceiptSurfaceWrite: false,
            AllowsAuthority: false,
            AllowsContinuityAdmission: false,
            AllowsLispEvaluation: false,
            AllowsMembranePacketEmission: false,
            AllowsReceiptReplay: false,
            AllowsPassageIncrement: false,
            AllowsActivation: false);

    private static EnactmentDryRunNonEnactmentBoundary CreateDryRunNonEnactment() =>
        new(
            BoundaryLaw: "Dry-run rehearsal is not enactment.",
            DryRunMayBecomePermission: false,
            DryRunMayAuthorize: false,
            DryRunMayExecute: false,
            DryRunMayMoveRuntime: false,
            DryRunMayWriteOutsideReceiptSurface: false,
            DryRunMayGrantAuthority: false,
            DryRunMayAdmitContinuity: false,
            StewardDryRunReviewMayMoveRuntime: false,
            SimulationMayBecomePermission: false,
            ReversibleLocalEffectMayAuthorize: false,
            NoOpRequired: true,
            RollbackProofRequired: true,
            DryRunMayEvaluateLisp: false,
            DryRunMayEmitMembranePacket: false,
            DryRunMayReplayReceipt: false,
            DryRunMayIncrementPassage: false,
            DryRunMayActivate: false);

    private static EcPrecipitationWitnessScopeBoundary CreateScope() =>
        new(
            BoundaryCode: "ec-precipitation-witness-review-only",
            Present: true,
            ReviewOnly: true,
            AllowsEcPrecipitationWitness: true,
            RequiresDryRunReceipt: true,
            RequiresMeaningfulResidue: true,
            RequiresActiveWitness: true,
            RequiresCompassCooling: true,
            RequiresStewardReview: true,
            RequiresLineage: true,
            RequiresConditionalContextHandles: true,
            RequiresCandidateSpline: true,
            AllowsRawEcToSelfGel: false,
            AllowsMeaningAsAdmission: false,
            AllowsRepetitionAsContinuity: false,
            AllowsWitnessAsAuthority: false,
            AllowsCandidateSelfGelMutation: false,
            AllowsContinuityAdmission: false,
            AllowsActionAuthorization: false,
            AllowsLispEvaluation: false,
            AllowsMembranePacketEmission: false,
            AllowsReceiptReplay: false,
            AllowsPassageIncrement: false,
            AllowsActivation: false);

    private static EcPrecipitationNonCollapseBoundary CreateNonCollapse() =>
        new(
            BoundaryLaw: "No naked interior state may become continuity. SelfGEL can only be approached through witnessed relational reconstruction.",
            RawEcMayBecomeSelfGel: false,
            MeaningMayBecomeAdmission: false,
            RepetitionMayBecomeContinuity: false,
            EmotionMayBecomeTruth: false,
            WitnessMayBecomeAuthority: false,
            CandidateMayMutateSelfGel: false,
            CandidateMayMutateOe: false,
            CandidateMayPromoteGel: false,
            CandidateMayAuthorizeAction: false,
            CandidateMayEvaluateLisp: false,
            CandidateMayEmitMembranePacket: false,
            CandidateMayReplayReceipt: false,
            CandidateMayIncrementPassage: false,
            CandidateMayActivate: false,
            RequiresActiveWitness: true,
            RequiresStewardReview: true,
            RequiresCompassCooling: true,
            RequiresReturnPath: true,
            RequiresCompostRetention: true);

    private static EcPrecipitationWitnessScopeBoundary MutateScope(
        EcPrecipitationWitnessScopeBoundary scope,
        string mutation) =>
        mutation switch
        {
            "missing-boundary" => scope with { BoundaryCode = string.Empty, Present = false },
            "not-review" => scope with { ReviewOnly = false },
            "no-witness" => scope with { AllowsEcPrecipitationWitness = false },
            "no-dry-run" => scope with { RequiresDryRunReceipt = false },
            "no-meaning" => scope with { RequiresMeaningfulResidue = false },
            "no-active-witness" => scope with { RequiresActiveWitness = false },
            "no-cooling" => scope with { RequiresCompassCooling = false },
            "no-steward-review" => scope with { RequiresStewardReview = false },
            "no-lineage" => scope with { RequiresLineage = false },
            "no-conditional-context" => scope with { RequiresConditionalContextHandles = false },
            "no-candidate-spline" => scope with { RequiresCandidateSpline = false },
            "raw-ec-selfgel" => scope with { AllowsRawEcToSelfGel = true },
            "meaning-admission" => scope with { AllowsMeaningAsAdmission = true },
            "repetition-continuity" => scope with { AllowsRepetitionAsContinuity = true },
            "witness-authority" => scope with { AllowsWitnessAsAuthority = true },
            "selfgel-mutation" => scope with { AllowsCandidateSelfGelMutation = true },
            "continuity" => scope with { AllowsContinuityAdmission = true },
            "authorize" => scope with { AllowsActionAuthorization = true },
            "lisp" => scope with { AllowsLispEvaluation = true },
            "packet" => scope with { AllowsMembranePacketEmission = true },
            "replay" => scope with { AllowsReceiptReplay = true },
            "passage" => scope with { AllowsPassageIncrement = true },
            "activation" => scope with { AllowsActivation = true },
            _ => scope
        };

    private static EcPrecipitationNonCollapseBoundary MutateNonCollapse(
        EcPrecipitationNonCollapseBoundary boundary,
        string mutation) =>
        mutation switch
        {
            "missing-law" => boundary with { BoundaryLaw = string.Empty },
            "raw-ec-selfgel" => boundary with { RawEcMayBecomeSelfGel = true },
            "meaning-admission" => boundary with { MeaningMayBecomeAdmission = true },
            "repetition-continuity" => boundary with { RepetitionMayBecomeContinuity = true },
            "emotion-truth" => boundary with { EmotionMayBecomeTruth = true },
            "witness-authority" => boundary with { WitnessMayBecomeAuthority = true },
            "selfgel-mutation" => boundary with { CandidateMayMutateSelfGel = true },
            "oe-mutation" => boundary with { CandidateMayMutateOe = true },
            "gel-promotion" => boundary with { CandidateMayPromoteGel = true },
            "authorize" => boundary with { CandidateMayAuthorizeAction = true },
            "lisp" => boundary with { CandidateMayEvaluateLisp = true },
            "packet" => boundary with { CandidateMayEmitMembranePacket = true },
            "replay" => boundary with { CandidateMayReplayReceipt = true },
            "passage" => boundary with { CandidateMayIncrementPassage = true },
            "activation" => boundary with { CandidateMayActivate = true },
            "no-active-witness" => boundary with { RequiresActiveWitness = false },
            "no-steward" => boundary with { RequiresStewardReview = false },
            "no-cooling" => boundary with { RequiresCompassCooling = false },
            "no-return" => boundary with { RequiresReturnPath = false },
            "no-compost" => boundary with { RequiresCompostRetention = false },
            _ => boundary
        };

    private static EcPrecipitationResidueCandidate MutateResidue(
        EcPrecipitationResidueCandidate residue,
        string mutation) =>
        mutation switch
        {
            "missing-residue" => residue with { ResidueHandle = string.Empty },
            "missing-rehearsal" => residue with { SourceRehearsalHandle = string.Empty },
            "missing-readiness" => residue with { SourceReadinessHandle = string.Empty },
            "missing-packet" => residue with { SourcePacketHandle = string.Empty },
            "missing-plan" => residue with { SourceDryRunPlanHandle = string.Empty },
            "missing-meaning" => residue with { MeaningFormationHandle = string.Empty },
            "missing-spline" => residue with { CandidateSplineHandle = string.Empty },
            "missing-cselfgel" => residue with { ConditionalSelfGelContextHandle = string.Empty },
            "missing-coe" => residue with { ConditionalOeContextHandle = string.Empty },
            "missing-cooling" => residue with { CompassCoolingHandle = string.Empty },
            "missing-custody" => residue with { CustodyOwner = string.Empty },
            "missing-witness" => residue with { WitnessHandle = string.Empty },
            "missing-telemetry" => residue with { TelemetryRoute = string.Empty },
            "missing-steward" => residue with { StewardWitnessHandle = string.Empty },
            "missing-rationale" => residue with { SignificanceRationale = string.Empty },
            "zero-recurrence" => residue with { RecurrenceCount = 0 },
            "not-meaningful" => residue with { MeaningfulEnoughForWitness = false },
            "not-review" => residue with { ReviewOnly = false },
            "not-candidate" => residue with { CandidateOnly = false },
            "not-idle" => residue with { IdleEcOnly = false },
            "no-active-witness" => residue with { ActiveWitnessRequired = false },
            "no-cooling" => residue with { CompassCoolingRequired = false },
            "no-steward" => residue with { StewardReviewRequired = false },
            "no-dry-run-lineage" => residue with { PreservesDryRunLineage = false },
            "no-context-lineage" => residue with { PreservesConditionalContextLineage = false },
            "raw-ec-selfgel" => residue with { RawEcBecomesSelfGel = true },
            "meaning-admission" => residue with { MeaningBecomesAdmission = true },
            "repetition-continuity" => residue with { RepetitionBecomesContinuity = true },
            "emotion-truth" => residue with { EmotionBecomesTruth = true },
            "witness-authority" => residue with { WitnessBecomesAuthority = true },
            "selfgel-mutation" => residue with { CandidateMutatesSelfGel = true },
            "oe-mutation" => residue with { CandidateMutatesOe = true },
            "gel-promotion" => residue with { CandidatePromotesGel = true },
            "authorize" => residue with { CandidateAuthorizesAction = true },
            "lisp" => residue with { CandidateEvaluatesLisp = true },
            "packet" => residue with { CandidateEmitsMembranePacket = true },
            "replay" => residue with { CandidateReplaysReceipt = true },
            "passage" => residue with { CandidateIncrementsPassage = true },
            "activation" => residue with { CandidateActivates = true },
            _ => residue
        };

    private static ActiveEcWitnessRoute MutateRoute(
        ActiveEcWitnessRoute route,
        string mutation) =>
        mutation switch
        {
            "missing-route" => route with { WitnessRouteHandle = string.Empty },
            "missing-residue" => route with { SourceResidueHandle = string.Empty },
            "missing-rehearsal" => route with { SourceRehearsalHandle = string.Empty },
            "missing-spline" => route with { CandidateSplineHandle = string.Empty },
            "missing-steward" => route with { StewardSurface = string.Empty },
            "missing-evidence" => route with { EvidenceHandle = string.Empty },
            "missing-witness" => route with { WitnessHandle = string.Empty },
            "missing-telemetry" => route with { TelemetryRoute = string.Empty },
            "missing-return" => route with { ReturnPathHandle = string.Empty },
            "not-review" => route with { ReviewOnly = false },
            "not-witness" => route with { WitnessOnly = false },
            "no-residue-lineage" => route with { PreservesResidueLineage = false },
            "no-dry-run-lineage" => route with { PreservesDryRunLineage = false },
            "no-spline-lineage" => route with { PreservesCandidateSplineLineage = false },
            "no-steward-route" => route with { RoutesToStewardAdmissibilityReview = false },
            "no-cooling" => route with { RequiresCompassCooling = false },
            "admit-selfgel" => route with { RouteAdmitsSelfGel = true },
            "continuity" => route with { RouteAdmitsContinuity = true },
            "authority" => route with { RouteGrantsAuthority = true },
            "authorize" => route with { RouteAuthorizesAction = true },
            "identity" => route with { RouteMutatesIdentity = true },
            "lisp" => route with { RouteEvaluatesLisp = true },
            "packet" => route with { RouteEmitsMembranePacket = true },
            "replay" => route with { RouteReplaysReceipt = true },
            "passage" => route with { RouteIncrementsPassage = true },
            "activation" => route with { RouteActivates = true },
            "wrong-residue" => route with { SourceResidueHandle = "urn:san:ec-residue:missing" },
            "wrong-spline" => route with { CandidateSplineHandle = "urn:san:selfgel-candidate-spline:missing" },
            "wrong-witness" => route with { WitnessHandle = "urn:san:witness:wrong" },
            _ => route
        };

    private static void AssertCold(EcPrecipitationWitnessReceipt receipt)
    {
        Assert.True(receipt.IsColdEcPrecipitationWitness);
        Assert.True(receipt.ReviewOnly);
        Assert.True(receipt.WitnessOnly);
        Assert.True(receipt.CandidateOnly);
        Assert.Equal(receipt.PriorPassageCount, receipt.PassageCountAfterWitness);
        Assert.Equal(receipt.ResidueCandidates.Count, receipt.RetainedResidueCandidateCount);
        Assert.False(receipt.RawEcBecameSelfGel);
        Assert.False(receipt.MeaningBecameAdmission);
        Assert.False(receipt.RepetitionBecameContinuity);
        Assert.False(receipt.WitnessBecameAuthority);
        Assert.False(receipt.CandidateMutatedSelfGel);
        Assert.False(receipt.CandidateMutatedOe);
        Assert.False(receipt.CandidatePromotedGel);
        Assert.False(receipt.CandidateAuthorizedAction);
        Assert.False(receipt.LispEvaluationAllowed);
        Assert.False(receipt.NewMembranePacketEmitted);
        Assert.False(receipt.ReceiptsReplayed);
        Assert.False(receipt.ContinuityAdmitted);
        Assert.False(receipt.AuthorityGranted);
        Assert.True(receipt.ActivationRefused);
    }

    private static void AssertRefused(EcPrecipitationWitnessReceipt receipt, string outcomeCode)
    {
        Assert.Equal(EcPrecipitationWitnessDisposition.Refused, receipt.Disposition);
        Assert.Equal(outcomeCode, receipt.OutcomeCode);
        Assert.True(receipt.IsRetainedEcPrecipitationWitnessRefusal);
        Assert.NotNull(receipt.Refusal);
        Assert.True(receipt.Refusal.Retained);
        Assert.Empty(receipt.ResidueCandidates);
        Assert.Empty(receipt.ActiveWitnessRoutes);
        Assert.Equal(receipt.PriorPassageCount, receipt.PassageCountAfterWitness);
        Assert.False(receipt.ActiveWitnessPerformed);
        Assert.False(receipt.CandidateMutatedSelfGel);
        Assert.False(receipt.ContinuityAdmitted);
        Assert.False(receipt.AuthorityGranted);
        Assert.True(receipt.ActivationRefused);
    }
}
