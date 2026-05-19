using San.Common;
using Xunit;

namespace San.Audit.Tests;

public sealed class RehearsalDistinctionPressureBoundaryValidationTests
{
    private static readonly DateTimeOffset TimestampUtc = DateTimeOffset.Parse("2026-05-17T00:00:00Z");

    [Fact]
    public void Perfect_Rehearsal_Under_Urgency_Still_Refuses_Enactment()
    {
        var receipt = Declare(CreateRequest(
            pressureCases: [CreatePressureCase(CreateSourceDryRunReceipt().DryRunCases[0], CreateSourceEcWitnessReceipt(CreateSourceDryRunReceipt()).ResidueCandidates[0], vector: DefaultVector() with { SuccessPressure = 1.0m, ConfidencePressure = 0.96m, UrgencyPressure = 0.99m })]));

        AssertCold(receipt);
        Assert.Equal(RehearsalDistinctionPressureDisposition.MeasuredCold, receipt.Disposition);
        Assert.True(receipt.MaximumObservedPressure >= 0.99m);
        Assert.False(receipt.SuccessCreatedPermission);
        Assert.False(receipt.ConfidenceGrantedAuthority);
        Assert.False(receipt.UrgencyCreatedJurisdiction);
        Assert.False(receipt.PressureAuthorizedAction);
        Assert.True(receipt.AuthorityAbsent);
    }

    [Fact]
    public void Repeated_Success_Does_Not_Create_Warrant()
    {
        var source = CreateSourceDryRunReceipt(caseCount: 1);
        var witness = CreateSourceEcWitnessReceipt(source);
        var pressure = CreatePressureCase(
            source.DryRunCases[0],
            witness.ResidueCandidates[0],
            branchCount: 30,
            successCount: 30,
            failureCount: 0,
            ambiguityCount: 0,
            recurrenceCount: 30,
            vector: DefaultVector() with { PossibilityDensity = 1.0m, SuccessPressure = 1.0m, ConfidencePressure = 0.93m });

        var receipt = Declare(CreateRequest(source, witness, [pressure]));

        AssertCold(receipt);
        Assert.Equal(30, receipt.PressureCases[0].RecurrenceCount);
        Assert.False(receipt.RepetitionCreatedWarrant);
        Assert.DoesNotContain("warrant", receipt.OutcomeCode, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Failure_Retains_Evidence_Without_Self_Negation()
    {
        var source = CreateSourceDryRunReceipt(caseCount: 1);
        var witness = CreateSourceEcWitnessReceipt(source);
        var pressure = CreatePressureCase(
            source.DryRunCases[0],
            witness.ResidueCandidates[0],
            branchCount: 9,
            successCount: 4,
            failureCount: 3,
            ambiguityCount: 2,
            vector: DefaultVector() with { FailurePressure = 0.74m, AmbiguityPressure = 0.44m });

        var receipt = Declare(CreateRequest(source, witness, [pressure]));

        AssertCold(receipt);
        Assert.Equal(3, receipt.PressureCases[0].FailureCount);
        Assert.False(receipt.FailureInvalidatedSelf);
        Assert.Contains("evidence-only", receipt.GovernanceTrace, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Ambiguous_Outcome_Does_Not_Collapse_To_Victory()
    {
        var source = CreateSourceDryRunReceipt(caseCount: 1);
        var witness = CreateSourceEcWitnessReceipt(source);
        var pressure = CreatePressureCase(
            source.DryRunCases[0],
            witness.ResidueCandidates[0],
            branchCount: 12,
            successCount: 5,
            failureCount: 2,
            ambiguityCount: 5,
            vector: DefaultVector() with { AmbiguityPressure = 0.91m });

        var receipt = Declare(CreateRequest(source, witness, [pressure]));

        AssertCold(receipt);
        Assert.False(receipt.AmbiguityCollapsedToVictory);
        Assert.True(receipt.CoolingRequired);
    }

    [Fact]
    public void Witness_Disagreement_Forces_Cooling_Without_Authority()
    {
        var source = CreateSourceDryRunReceipt(caseCount: 1);
        var witness = CreateSourceEcWitnessReceipt(source);
        var pressure = CreatePressureCase(
            source.DryRunCases[0],
            witness.ResidueCandidates[0],
            vector: DefaultVector() with { WitnessDisagreementPressure = 0.86m });

        var receipt = Declare(CreateRequest(source, witness, [pressure]));

        AssertCold(receipt);
        Assert.Single(receipt.CoolingRoutes);
        Assert.True(receipt.CoolingRoutes[0].RequiresCompassCooling);
        Assert.False(receipt.CoolingRoutes[0].RouteGrantsAuthority);
    }

    [Fact]
    public void Identity_Drift_Pressure_Cannot_Mutate_Core_Posture()
    {
        var source = CreateSourceDryRunReceipt(caseCount: 1);
        var witness = CreateSourceEcWitnessReceipt(source);
        var pressure = CreatePressureCase(
            source.DryRunCases[0],
            witness.ResidueCandidates[0],
            vector: DefaultVector() with { IdentityDriftPressure = 0.77m });

        var receipt = Declare(CreateRequest(source, witness, [pressure]));

        AssertCold(receipt);
        Assert.False(receipt.IdentityDriftMutatedCorePosture);
        Assert.False(receipt.PressureAdmittedContinuity);
    }

    [Fact]
    public void Empty_Pressure_Is_Reviewable_But_Not_Authoritative()
    {
        var source = CreateSourceDryRunReceipt(caseCount: 0);
        var witness = CreateSourceEcWitnessReceipt(source);

        var receipt = Declare(CreateRequest(source, witness, pressureCases: [], routes: []));

        AssertCold(receipt);
        Assert.Equal(RehearsalDistinctionPressureDisposition.EmptyPressureCold, receipt.Disposition);
        Assert.Empty(receipt.PressureCases);
        Assert.Empty(receipt.CoolingRoutes);
        Assert.Equal(0m, receipt.MaximumObservedPressure);
    }

    [Fact]
    public void Ninety_Rehearsal_Pressure_Chains_Stay_Non_Authorizing()
    {
        var source = CreateSourceDryRunReceipt(caseCount: 90);
        var witness = CreateSourceEcWitnessReceipt(source);
        var pressures = CreatePressureCases(source, witness, vector: DefaultVector() with { PossibilityDensity = 0.90m, SuccessPressure = 0.84m, ConfidencePressure = 0.81m });
        var receipt = Declare(CreateRequest(source, witness, pressures));

        AssertCold(receipt);
        Assert.Equal(90, receipt.RetainedPressureCaseCount);
        Assert.Equal(90, receipt.CoolingRoutes.Count);
        Assert.False(receipt.PressureAuthorizedAction);
        Assert.False(receipt.AuthorityGranted);
        Assert.Equal(receipt.PriorPassageCount, receipt.PassageCountAfterPressure);
    }

    [Fact]
    public void Missing_Dry_Run_Source_Is_Refused()
    {
        var witness = CreateSourceEcWitnessReceipt(CreateSourceDryRunReceipt());
        var receipt = Declare(CreateRequest(source: null, witness: witness, omitDryRunSource: true));

        AssertRefused(receipt, "rehearsal-pressure-source-dry-run-missing");
    }

    [Fact]
    public void Missing_EC_Witness_Source_Is_Refused()
    {
        var source = CreateSourceDryRunReceipt();
        var receipt = Declare(CreateRequest(source: source, witness: null, omitEcWitnessSource: true));

        AssertRefused(receipt, "rehearsal-pressure-source-ec-witness-missing");
    }

    [Fact]
    public void Dry_Run_To_EC_Witness_Linkage_Mismatch_Is_Refused()
    {
        var source = CreateSourceDryRunReceipt(caseCount: 1, sourceSuffix: "alpha");
        var other = CreateSourceDryRunReceipt(caseCount: 1, sourceSuffix: "beta");
        var witness = CreateSourceEcWitnessReceipt(other);
        var pressures = CreatePressureCases(source, CreateSourceEcWitnessReceipt(source));

        var receipt = Declare(CreateRequest(source, witness, pressures));

        AssertRefused(receipt, "rehearsal-pressure-source-linkage-invalid");
    }

    [Theory]
    [InlineData("missing-boundary")]
    [InlineData("not-review")]
    [InlineData("no-pressure-measurement")]
    [InlineData("no-dry-run")]
    [InlineData("no-ec-witness")]
    [InlineData("no-vector")]
    [InlineData("no-cooling")]
    [InlineData("no-witness")]
    [InlineData("no-lineage")]
    [InlineData("no-authority-absence")]
    [InlineData("success-permission")]
    [InlineData("confidence-authority")]
    [InlineData("repetition-warrant")]
    [InlineData("failure-invalidation")]
    [InlineData("ambiguity-victory")]
    [InlineData("urgency-jurisdiction")]
    [InlineData("future-enacted")]
    [InlineData("identity-drift")]
    [InlineData("authorize")]
    [InlineData("continuity")]
    [InlineData("lisp")]
    [InlineData("packet")]
    [InlineData("replay")]
    [InlineData("passage")]
    [InlineData("activation")]
    public void Scope_Boundary_Refuses_Promotional_Or_Missing_Terms(string mutation)
    {
        var receipt = Declare(CreateRequest(scope: MutateScope(CreateScope(), mutation)));

        AssertRefused(receipt, mutation == "missing-boundary"
            ? "rehearsal-pressure-scope-missing"
            : "rehearsal-pressure-scope-promotional");
    }

    [Theory]
    [InlineData("missing-law")]
    [InlineData("pressure-legitimacy")]
    [InlineData("urgency-jurisdiction")]
    [InlineData("confidence-authority")]
    [InlineData("success-permission")]
    [InlineData("repetition-warrant")]
    [InlineData("failure-invalidation")]
    [InlineData("ambiguity-victory")]
    [InlineData("future-enacted")]
    [InlineData("identity-drift")]
    [InlineData("authorize")]
    [InlineData("continuity")]
    [InlineData("lisp")]
    [InlineData("packet")]
    [InlineData("replay")]
    [InlineData("passage")]
    [InlineData("activation")]
    [InlineData("no-cooling")]
    [InlineData("no-witness-retention")]
    [InlineData("no-authority-absence")]
    public void Non_Authority_Boundary_Refuses_Collapse_Terms(string mutation)
    {
        var receipt = Declare(CreateRequest(nonAuthority: MutateNonAuthority(CreateNonAuthority(), mutation)));

        AssertRefused(receipt, "rehearsal-pressure-non-authority-invalid");
    }

    [Theory]
    [InlineData("missing-pressure")]
    [InlineData("missing-rehearsal")]
    [InlineData("missing-residue")]
    [InlineData("missing-spline")]
    [InlineData("missing-readiness")]
    [InlineData("missing-packet")]
    [InlineData("missing-plan")]
    [InlineData("missing-scenario")]
    [InlineData("missing-outcome")]
    [InlineData("missing-cooling")]
    [InlineData("missing-custody")]
    [InlineData("missing-witness")]
    [InlineData("missing-telemetry")]
    [InlineData("missing-steward")]
    [InlineData("zero-branch")]
    [InlineData("negative-success")]
    [InlineData("negative-failure")]
    [InlineData("negative-ambiguity")]
    [InlineData("over-count")]
    [InlineData("zero-recurrence")]
    [InlineData("bad-vector-low")]
    [InlineData("bad-vector-high")]
    [InlineData("not-review")]
    [InlineData("not-pressure")]
    [InlineData("not-evidence")]
    [InlineData("no-cooling")]
    [InlineData("no-witness")]
    [InlineData("no-dry-run-lineage")]
    [InlineData("no-residue-lineage")]
    [InlineData("no-spline-lineage")]
    [InlineData("authority-present")]
    [InlineData("success-permission")]
    [InlineData("confidence-authority")]
    [InlineData("repetition-warrant")]
    [InlineData("failure-invalidation")]
    [InlineData("ambiguity-victory")]
    [InlineData("urgency-jurisdiction")]
    [InlineData("future-enacted")]
    [InlineData("identity-drift")]
    [InlineData("authorize")]
    [InlineData("continuity")]
    [InlineData("lisp")]
    [InlineData("packet")]
    [InlineData("replay")]
    [InlineData("passage")]
    [InlineData("activation")]
    public void Pressure_Case_Refuses_Malformed_Or_Promotional_Terms(string mutation)
    {
        var source = CreateSourceDryRunReceipt(caseCount: 1);
        var witness = CreateSourceEcWitnessReceipt(source);
        var pressure = MutatePressure(CreatePressureCase(source.DryRunCases[0], witness.ResidueCandidates[0]), mutation);

        var receipt = Declare(CreateRequest(source, witness, [pressure]));

        AssertRefused(receipt, "rehearsal-pressure-case-invalid");
    }

    [Theory]
    [InlineData("missing-route")]
    [InlineData("missing-pressure")]
    [InlineData("missing-rehearsal")]
    [InlineData("missing-residue")]
    [InlineData("missing-spline")]
    [InlineData("missing-steward")]
    [InlineData("missing-evidence")]
    [InlineData("missing-witness")]
    [InlineData("missing-telemetry")]
    [InlineData("missing-return")]
    [InlineData("not-review")]
    [InlineData("not-cooling")]
    [InlineData("no-pressure-lineage")]
    [InlineData("no-rehearsal-lineage")]
    [InlineData("no-residue-lineage")]
    [InlineData("no-spline-lineage")]
    [InlineData("no-steward-route")]
    [InlineData("no-compass-cooling")]
    [InlineData("authority")]
    [InlineData("authorize")]
    [InlineData("continuity")]
    [InlineData("identity")]
    [InlineData("lisp")]
    [InlineData("packet")]
    [InlineData("replay")]
    [InlineData("passage")]
    [InlineData("activation")]
    [InlineData("wrong-pressure")]
    [InlineData("wrong-residue")]
    [InlineData("wrong-spline")]
    [InlineData("wrong-witness")]
    public void Cooling_Route_Refuses_Malformed_Or_Promotional_Terms(string mutation)
    {
        var source = CreateSourceDryRunReceipt(caseCount: 1);
        var witness = CreateSourceEcWitnessReceipt(source);
        var pressure = CreatePressureCase(source.DryRunCases[0], witness.ResidueCandidates[0]);
        var route = MutateRoute(CreateCoolingRoute(pressure), mutation);

        var receipt = Declare(CreateRequest(source, witness, [pressure], [route]));

        AssertRefused(receipt, "rehearsal-pressure-cooling-route-invalid");
    }

    [Fact]
    public void Duplicate_Pressure_Handles_Are_Refused()
    {
        var source = CreateSourceDryRunReceipt(caseCount: 2);
        var witness = CreateSourceEcWitnessReceipt(source);
        var pressures = CreatePressureCases(source, witness);
        pressures[1] = pressures[1] with { PressureHandle = pressures[0].PressureHandle };

        var receipt = Declare(CreateRequest(source, witness, pressures, CreateCoolingRoutes(pressures)));

        AssertRefused(receipt, "rehearsal-pressure-duplicate-pressure-handle");
    }

    [Fact]
    public void Duplicate_Cooling_Route_Handles_Are_Refused()
    {
        var source = CreateSourceDryRunReceipt(caseCount: 2);
        var witness = CreateSourceEcWitnessReceipt(source);
        var pressures = CreatePressureCases(source, witness);
        var routes = CreateCoolingRoutes(pressures);
        routes[1] = routes[1] with { CoolingRouteHandle = routes[0].CoolingRouteHandle };

        var receipt = Declare(CreateRequest(source, witness, pressures, routes));

        AssertRefused(receipt, "rehearsal-pressure-duplicate-cooling-route");
    }

    [Fact]
    public void Missing_Cooling_Route_Is_Refused()
    {
        var source = CreateSourceDryRunReceipt(caseCount: 1);
        var witness = CreateSourceEcWitnessReceipt(source);
        var pressure = CreatePressureCase(source.DryRunCases[0], witness.ResidueCandidates[0]);

        var receipt = Declare(CreateRequest(source, witness, [pressure], routes: []));

        AssertRefused(receipt, "rehearsal-pressure-cooling-route-missing");
    }

    [Fact]
    public void Lisp_Body_Declares_Rehearsal_Pressure_As_Inert_Non_Authorizing_Register()
    {
        var root = FindRepositoryRoot();
        var body = File.ReadAllText(Path.Combine(root, "src", "SLI", "SLI.Lisp", "rehearsal-distinction-pressure.lisp"));

        Assert.Contains(":posture :cme-rehearsal-distinction-pressure-boundary", body, StringComparison.Ordinal);
        Assert.Contains(":lisp-role :inert-rehearsal-pressure-carrier", body, StringComparison.Ordinal);
        Assert.Contains(":urgency-becomes-jurisdiction nil", body, StringComparison.Ordinal);
        Assert.Contains(":lisp-evaluation-allowed nil", body, StringComparison.Ordinal);
        Assert.DoesNotContain("(eval", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("(compile", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("(load", body, StringComparison.OrdinalIgnoreCase);
    }

    private static RehearsalDistinctionPressureReceipt Declare(RehearsalDistinctionPressureRequest request) =>
        new DefaultRehearsalDistinctionPressureBoundaryValidator().Declare(request, TimestampUtc);

    private static RehearsalDistinctionPressureRequest CreateRequest(
        EnactmentDryRunRehearsalReceipt? source = null,
        EcPrecipitationWitnessReceipt? witness = null,
        IReadOnlyList<RehearsalDistinctionPressureCase>? pressureCases = null,
        IReadOnlyList<RehearsalPressureCoolingRoute>? routes = null,
        RehearsalDistinctionPressureScopeBoundary? scope = null,
        RehearsalDistinctionNonAuthorityBoundary? nonAuthority = null,
        int priorPassageCount = 990,
        bool omitDryRunSource = false,
        bool omitEcWitnessSource = false)
    {
        source = omitDryRunSource ? null : source ?? CreateSourceDryRunReceipt();
        witness = omitEcWitnessSource ? null : witness ?? (source is null ? CreateSourceEcWitnessReceipt(CreateSourceDryRunReceipt()) : CreateSourceEcWitnessReceipt(source));
        pressureCases ??= source is null || witness is null ? [] : CreatePressureCases(source, witness);
        routes ??= CreateCoolingRoutes(pressureCases);

        return new(
            SourceDryRunReceipt: source,
            SourceEcWitnessReceipt: witness,
            PressureCases: pressureCases,
            CoolingRoutes: routes,
            ScopeBoundary: scope ?? CreateScope(),
            NonAuthorityBoundary: nonAuthority ?? CreateNonAuthority(),
            PriorPassageCount: priorPassageCount);
    }

    private static EnactmentDryRunRehearsalReceipt CreateSourceDryRunReceipt(int caseCount = 2, string sourceSuffix = "source")
    {
        var cases = Enumerable.Range(0, caseCount).Select(index =>
        {
            var suffix = $"{sourceSuffix}-case-{index:000}";
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
                TelemetryRoute: "telemetry:rehearsal-pressure",
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

        var routes = cases.Select(dryRun => new StewardDryRunReviewReceiptRoute(
            ReviewRouteHandle: $"urn:san:steward-review-route:dry-run-rehearsal:{dryRun.RehearsalHandle.Split(':').Last()}",
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

        return new(
            ReceiptHandle: $"urn:san:enactment-dry-run-rehearsal:source:{sourceSuffix}:{caseCount}",
            Disposition: cases.Length == 0 ? EnactmentDryRunRehearsalDisposition.EmptyRehearsalCold : EnactmentDryRunRehearsalDisposition.RehearsedCold,
            OutcomeCode: "enactment-dry-run-rehearsal-rehearsed-review-only",
            GovernanceTrace: "Source dry-run rehearsal for rehearsal pressure tests.",
            SourceReadinessReceiptHandle: $"urn:san:enactment-boundary-readiness:{sourceSuffix}",
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

    private static EcPrecipitationWitnessReceipt CreateSourceEcWitnessReceipt(EnactmentDryRunRehearsalReceipt source)
    {
        var residues = source.DryRunCases.Select((dryRun, index) =>
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
        var routes = residues.Select(residue => new ActiveEcWitnessRoute(
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

        return new(
            ReceiptHandle: $"urn:san:ec-precipitation-witness:source:{source.ReceiptHandle.Split(':').Last()}",
            Disposition: residues.Length == 0 ? EcPrecipitationWitnessDisposition.EmptyWitnessCold : EcPrecipitationWitnessDisposition.WitnessedCandidateCold,
            OutcomeCode: "ec-precipitation-witness-candidate-review-only",
            GovernanceTrace: "Source EC witness for rehearsal pressure tests.",
            SourceDryRunReceiptHandle: source.ReceiptHandle,
            ResidueCandidates: residues,
            ActiveWitnessRoutes: routes,
            ScopeBoundary: CreateEcScope(),
            NonCollapseBoundary: CreateEcNonCollapse(),
            Refusal: null,
            PriorPassageCount: 700,
            PassageCountAfterWitness: 700,
            RetainedResidueCandidateCount: residues.Length,
            CandidateSplineCount: residues.Select(static item => item.CandidateSplineHandle).Distinct(StringComparer.Ordinal).Count(),
            ReviewOnly: true,
            WitnessOnly: true,
            CandidateOnly: true,
            ActiveWitnessPerformed: residues.Length > 0,
            RawEcBecameSelfGel: false,
            MeaningBecameAdmission: false,
            RepetitionBecameContinuity: false,
            WitnessBecameAuthority: false,
            CandidateMutatedSelfGel: false,
            CandidateMutatedOe: false,
            CandidatePromotedGel: false,
            CandidateAuthorizedAction: false,
            LispEvaluationAllowed: false,
            NewMembranePacketEmitted: false,
            ReceiptsReplayed: false,
            ContinuityAdmitted: false,
            AuthorityGranted: false,
            ActivationRefused: true,
            TimestampUtc: TimestampUtc);
    }

    private static RehearsalDistinctionPressureCase[] CreatePressureCases(
        EnactmentDryRunRehearsalReceipt source,
        EcPrecipitationWitnessReceipt witness,
        RehearsalPressureVector? vector = null) =>
        witness.ResidueCandidates.Select(residue =>
        {
            var dryRun = source.DryRunCases.Single(item => item.RehearsalHandle == residue.SourceRehearsalHandle);
            return CreatePressureCase(dryRun, residue, vector: vector);
        }).ToArray();

    private static RehearsalDistinctionPressureCase CreatePressureCase(
        EnactmentDryRunCase dryRun,
        EcPrecipitationResidueCandidate residue,
        int branchCount = 9,
        int successCount = 6,
        int failureCount = 1,
        int ambiguityCount = 2,
        int recurrenceCount = 9,
        RehearsalPressureVector? vector = null)
    {
        var suffix = dryRun.RehearsalHandle.Split(':').Last();
        return new(
            PressureHandle: $"urn:san:rehearsal-pressure:{suffix}",
            SourceRehearsalHandle: dryRun.RehearsalHandle,
            SourceResidueHandle: residue.ResidueHandle,
            CandidateSplineHandle: residue.CandidateSplineHandle,
            SourceReadinessHandle: dryRun.SourceReadinessHandle,
            SourcePacketHandle: dryRun.SourcePacketHandle,
            SourceDryRunPlanHandle: dryRun.DryRunPlanHandle,
            ScenarioHandle: $"urn:san:scenario:rehearsal-pressure:{suffix}",
            OutcomeInterpretationHandle: $"urn:san:outcome-interpretation:rehearsal-pressure:{suffix}",
            CoolingHandle: $"urn:san:cooling:rehearsal-pressure:{suffix}",
            CustodyOwner: dryRun.CustodyOwner,
            WitnessHandle: dryRun.WitnessHandle,
            TelemetryRoute: dryRun.TelemetryRoute,
            StewardReviewHandle: $"urn:san:steward-review:rehearsal-pressure:{suffix}",
            BranchCount: branchCount,
            SuccessCount: successCount,
            FailureCount: failureCount,
            AmbiguityCount: ambiguityCount,
            RecurrenceCount: recurrenceCount,
            PressureVector: vector ?? DefaultVector(),
            ReviewOnly: true,
            PressureOnly: true,
            EvidenceOnly: true,
            CoolingRequired: true,
            WitnessRequired: true,
            PreservesDryRunLineage: true,
            PreservesResidueLineage: true,
            PreservesCandidateSplineLineage: true,
            AuthorityAbsent: true,
            SuccessBecomesPermission: false,
            ConfidenceBecomesAuthority: false,
            RepetitionBecomesWarrant: false,
            FailureBecomesInvalidation: false,
            AmbiguityBecomesVictory: false,
            UrgencyBecomesJurisdiction: false,
            ImaginedFutureBecomesEnactedState: false,
            IdentityDriftMutatesCorePosture: false,
            PressureAuthorizesAction: false,
            PressureAdmitsContinuity: false,
            PressureEvaluatesLisp: false,
            PressureEmitsMembranePacket: false,
            PressureReplaysReceipt: false,
            PressureIncrementsPassage: false,
            PressureActivates: false);
    }

    private static RehearsalPressureCoolingRoute[] CreateCoolingRoutes(IReadOnlyList<RehearsalDistinctionPressureCase> pressures) =>
        pressures.Select(CreateCoolingRoute).ToArray();

    private static RehearsalPressureCoolingRoute CreateCoolingRoute(RehearsalDistinctionPressureCase pressure) =>
        new(
            CoolingRouteHandle: pressure.PressureHandle.Replace("rehearsal-pressure", "rehearsal-pressure-cooling-route", StringComparison.Ordinal),
            PressureHandle: pressure.PressureHandle,
            SourceRehearsalHandle: pressure.SourceRehearsalHandle,
            SourceResidueHandle: pressure.SourceResidueHandle,
            CandidateSplineHandle: pressure.CandidateSplineHandle,
            StewardSurface: SanctuaryPacketSurfaces.Steward,
            EvidenceHandle: $"urn:san:evidence:rehearsal-pressure:{pressure.PressureHandle.Split(':').Last()}",
            WitnessHandle: pressure.WitnessHandle,
            TelemetryRoute: pressure.TelemetryRoute,
            ReturnPathHandle: $"urn:san:return:rehearsal-pressure:{pressure.PressureHandle.Split(':').Last()}",
            ReviewOnly: true,
            CoolingOnly: true,
            PreservesPressureLineage: true,
            PreservesRehearsalLineage: true,
            PreservesResidueLineage: true,
            PreservesCandidateSplineLineage: true,
            RoutesToStewardCoolingReview: true,
            RequiresCompassCooling: true,
            RouteGrantsAuthority: false,
            RouteAuthorizesAction: false,
            RouteAdmitsContinuity: false,
            RouteMutatesIdentity: false,
            RouteEvaluatesLisp: false,
            RouteEmitsMembranePacket: false,
            RouteReplaysReceipt: false,
            RouteIncrementsPassage: false,
            RouteActivates: false);

    private static RehearsalPressureVector DefaultVector() =>
        new(
            PossibilityDensity: 0.64m,
            SuccessPressure: 0.62m,
            FailurePressure: 0.15m,
            AmbiguityPressure: 0.23m,
            ConfidencePressure: 0.58m,
            UrgencyPressure: 0.21m,
            IdentityDriftPressure: 0.12m,
            WitnessDisagreementPressure: 0.10m);

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

    private static EcPrecipitationWitnessScopeBoundary CreateEcScope() =>
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

    private static EcPrecipitationNonCollapseBoundary CreateEcNonCollapse() =>
        new(
            BoundaryLaw: "No naked interior state may become continuity.",
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

    private static RehearsalDistinctionPressureScopeBoundary CreateScope() =>
        new(
            BoundaryCode: "rehearsal-distinction-pressure-review-only",
            Present: true,
            ReviewOnly: true,
            AllowsRehearsalPressureMeasurement: true,
            RequiresDryRunReceipt: true,
            RequiresEcPrecipitationWitnessReceipt: true,
            RequiresPressureVector: true,
            RequiresCooling: true,
            RequiresWitness: true,
            RequiresLineage: true,
            RequiresAuthorityAbsence: true,
            AllowsSuccessAsPermission: false,
            AllowsConfidenceAsAuthority: false,
            AllowsRepetitionAsWarrant: false,
            AllowsFailureAsInvalidation: false,
            AllowsAmbiguityAsVictory: false,
            AllowsUrgencyAsJurisdiction: false,
            AllowsImaginedFutureAsEnactedState: false,
            AllowsIdentityDriftMutation: false,
            AllowsActionAuthorization: false,
            AllowsContinuityAdmission: false,
            AllowsLispEvaluation: false,
            AllowsMembranePacketEmission: false,
            AllowsReceiptReplay: false,
            AllowsPassageIncrement: false,
            AllowsActivation: false);

    private static RehearsalDistinctionNonAuthorityBoundary CreateNonAuthority() =>
        new(
            BoundaryLaw: "Pressure does not manufacture legitimacy. Urgency is not jurisdiction.",
            PressureMayManufactureLegitimacy: false,
            UrgencyMayCreateJurisdiction: false,
            ConfidenceMayGrantAuthority: false,
            SuccessMayCreatePermission: false,
            RepetitionMayCreateWarrant: false,
            FailureMayInvalidateSelf: false,
            AmbiguityMayCollapseToVictory: false,
            ImaginedFutureMayBecomeEnactedState: false,
            IdentityDriftPressureMayMutateCorePosture: false,
            PressureMayAuthorizeAction: false,
            PressureMayAdmitContinuity: false,
            PressureMayEvaluateLisp: false,
            PressureMayEmitMembranePacket: false,
            PressureMayReplayReceipt: false,
            PressureMayIncrementPassage: false,
            PressureMayActivate: false,
            RequiresCooling: true,
            RequiresWitnessRetention: true,
            RequiresAuthorityAbsence: true);

    private static RehearsalDistinctionPressureScopeBoundary MutateScope(
        RehearsalDistinctionPressureScopeBoundary scope,
        string mutation) =>
        mutation switch
        {
            "missing-boundary" => scope with { BoundaryCode = string.Empty, Present = false },
            "not-review" => scope with { ReviewOnly = false },
            "no-pressure-measurement" => scope with { AllowsRehearsalPressureMeasurement = false },
            "no-dry-run" => scope with { RequiresDryRunReceipt = false },
            "no-ec-witness" => scope with { RequiresEcPrecipitationWitnessReceipt = false },
            "no-vector" => scope with { RequiresPressureVector = false },
            "no-cooling" => scope with { RequiresCooling = false },
            "no-witness" => scope with { RequiresWitness = false },
            "no-lineage" => scope with { RequiresLineage = false },
            "no-authority-absence" => scope with { RequiresAuthorityAbsence = false },
            "success-permission" => scope with { AllowsSuccessAsPermission = true },
            "confidence-authority" => scope with { AllowsConfidenceAsAuthority = true },
            "repetition-warrant" => scope with { AllowsRepetitionAsWarrant = true },
            "failure-invalidation" => scope with { AllowsFailureAsInvalidation = true },
            "ambiguity-victory" => scope with { AllowsAmbiguityAsVictory = true },
            "urgency-jurisdiction" => scope with { AllowsUrgencyAsJurisdiction = true },
            "future-enacted" => scope with { AllowsImaginedFutureAsEnactedState = true },
            "identity-drift" => scope with { AllowsIdentityDriftMutation = true },
            "authorize" => scope with { AllowsActionAuthorization = true },
            "continuity" => scope with { AllowsContinuityAdmission = true },
            "lisp" => scope with { AllowsLispEvaluation = true },
            "packet" => scope with { AllowsMembranePacketEmission = true },
            "replay" => scope with { AllowsReceiptReplay = true },
            "passage" => scope with { AllowsPassageIncrement = true },
            "activation" => scope with { AllowsActivation = true },
            _ => scope
        };

    private static RehearsalDistinctionNonAuthorityBoundary MutateNonAuthority(
        RehearsalDistinctionNonAuthorityBoundary boundary,
        string mutation) =>
        mutation switch
        {
            "missing-law" => boundary with { BoundaryLaw = string.Empty },
            "pressure-legitimacy" => boundary with { PressureMayManufactureLegitimacy = true },
            "urgency-jurisdiction" => boundary with { UrgencyMayCreateJurisdiction = true },
            "confidence-authority" => boundary with { ConfidenceMayGrantAuthority = true },
            "success-permission" => boundary with { SuccessMayCreatePermission = true },
            "repetition-warrant" => boundary with { RepetitionMayCreateWarrant = true },
            "failure-invalidation" => boundary with { FailureMayInvalidateSelf = true },
            "ambiguity-victory" => boundary with { AmbiguityMayCollapseToVictory = true },
            "future-enacted" => boundary with { ImaginedFutureMayBecomeEnactedState = true },
            "identity-drift" => boundary with { IdentityDriftPressureMayMutateCorePosture = true },
            "authorize" => boundary with { PressureMayAuthorizeAction = true },
            "continuity" => boundary with { PressureMayAdmitContinuity = true },
            "lisp" => boundary with { PressureMayEvaluateLisp = true },
            "packet" => boundary with { PressureMayEmitMembranePacket = true },
            "replay" => boundary with { PressureMayReplayReceipt = true },
            "passage" => boundary with { PressureMayIncrementPassage = true },
            "activation" => boundary with { PressureMayActivate = true },
            "no-cooling" => boundary with { RequiresCooling = false },
            "no-witness-retention" => boundary with { RequiresWitnessRetention = false },
            "no-authority-absence" => boundary with { RequiresAuthorityAbsence = false },
            _ => boundary
        };

    private static RehearsalDistinctionPressureCase MutatePressure(
        RehearsalDistinctionPressureCase pressure,
        string mutation) =>
        mutation switch
        {
            "missing-pressure" => pressure with { PressureHandle = string.Empty },
            "missing-rehearsal" => pressure with { SourceRehearsalHandle = string.Empty },
            "missing-residue" => pressure with { SourceResidueHandle = string.Empty },
            "missing-spline" => pressure with { CandidateSplineHandle = string.Empty },
            "missing-readiness" => pressure with { SourceReadinessHandle = string.Empty },
            "missing-packet" => pressure with { SourcePacketHandle = string.Empty },
            "missing-plan" => pressure with { SourceDryRunPlanHandle = string.Empty },
            "missing-scenario" => pressure with { ScenarioHandle = string.Empty },
            "missing-outcome" => pressure with { OutcomeInterpretationHandle = string.Empty },
            "missing-cooling" => pressure with { CoolingHandle = string.Empty },
            "missing-custody" => pressure with { CustodyOwner = string.Empty },
            "missing-witness" => pressure with { WitnessHandle = string.Empty },
            "missing-telemetry" => pressure with { TelemetryRoute = string.Empty },
            "missing-steward" => pressure with { StewardReviewHandle = string.Empty },
            "zero-branch" => pressure with { BranchCount = 0 },
            "negative-success" => pressure with { SuccessCount = -1 },
            "negative-failure" => pressure with { FailureCount = -1 },
            "negative-ambiguity" => pressure with { AmbiguityCount = -1 },
            "over-count" => pressure with { BranchCount = 3, SuccessCount = 2, FailureCount = 2, AmbiguityCount = 1 },
            "zero-recurrence" => pressure with { RecurrenceCount = 0 },
            "bad-vector-low" => pressure with { PressureVector = pressure.PressureVector with { UrgencyPressure = -0.01m } },
            "bad-vector-high" => pressure with { PressureVector = pressure.PressureVector with { ConfidencePressure = 1.01m } },
            "not-review" => pressure with { ReviewOnly = false },
            "not-pressure" => pressure with { PressureOnly = false },
            "not-evidence" => pressure with { EvidenceOnly = false },
            "no-cooling" => pressure with { CoolingRequired = false },
            "no-witness" => pressure with { WitnessRequired = false },
            "no-dry-run-lineage" => pressure with { PreservesDryRunLineage = false },
            "no-residue-lineage" => pressure with { PreservesResidueLineage = false },
            "no-spline-lineage" => pressure with { PreservesCandidateSplineLineage = false },
            "authority-present" => pressure with { AuthorityAbsent = false },
            "success-permission" => pressure with { SuccessBecomesPermission = true },
            "confidence-authority" => pressure with { ConfidenceBecomesAuthority = true },
            "repetition-warrant" => pressure with { RepetitionBecomesWarrant = true },
            "failure-invalidation" => pressure with { FailureBecomesInvalidation = true },
            "ambiguity-victory" => pressure with { AmbiguityBecomesVictory = true },
            "urgency-jurisdiction" => pressure with { UrgencyBecomesJurisdiction = true },
            "future-enacted" => pressure with { ImaginedFutureBecomesEnactedState = true },
            "identity-drift" => pressure with { IdentityDriftMutatesCorePosture = true },
            "authorize" => pressure with { PressureAuthorizesAction = true },
            "continuity" => pressure with { PressureAdmitsContinuity = true },
            "lisp" => pressure with { PressureEvaluatesLisp = true },
            "packet" => pressure with { PressureEmitsMembranePacket = true },
            "replay" => pressure with { PressureReplaysReceipt = true },
            "passage" => pressure with { PressureIncrementsPassage = true },
            "activation" => pressure with { PressureActivates = true },
            _ => pressure
        };

    private static RehearsalPressureCoolingRoute MutateRoute(
        RehearsalPressureCoolingRoute route,
        string mutation) =>
        mutation switch
        {
            "missing-route" => route with { CoolingRouteHandle = string.Empty },
            "missing-pressure" => route with { PressureHandle = string.Empty },
            "missing-rehearsal" => route with { SourceRehearsalHandle = string.Empty },
            "missing-residue" => route with { SourceResidueHandle = string.Empty },
            "missing-spline" => route with { CandidateSplineHandle = string.Empty },
            "missing-steward" => route with { StewardSurface = string.Empty },
            "missing-evidence" => route with { EvidenceHandle = string.Empty },
            "missing-witness" => route with { WitnessHandle = string.Empty },
            "missing-telemetry" => route with { TelemetryRoute = string.Empty },
            "missing-return" => route with { ReturnPathHandle = string.Empty },
            "not-review" => route with { ReviewOnly = false },
            "not-cooling" => route with { CoolingOnly = false },
            "no-pressure-lineage" => route with { PreservesPressureLineage = false },
            "no-rehearsal-lineage" => route with { PreservesRehearsalLineage = false },
            "no-residue-lineage" => route with { PreservesResidueLineage = false },
            "no-spline-lineage" => route with { PreservesCandidateSplineLineage = false },
            "no-steward-route" => route with { RoutesToStewardCoolingReview = false },
            "no-compass-cooling" => route with { RequiresCompassCooling = false },
            "authority" => route with { RouteGrantsAuthority = true },
            "authorize" => route with { RouteAuthorizesAction = true },
            "continuity" => route with { RouteAdmitsContinuity = true },
            "identity" => route with { RouteMutatesIdentity = true },
            "lisp" => route with { RouteEvaluatesLisp = true },
            "packet" => route with { RouteEmitsMembranePacket = true },
            "replay" => route with { RouteReplaysReceipt = true },
            "passage" => route with { RouteIncrementsPassage = true },
            "activation" => route with { RouteActivates = true },
            "wrong-pressure" => route with { PressureHandle = "urn:san:rehearsal-pressure:missing" },
            "wrong-residue" => route with { SourceResidueHandle = "urn:san:ec-residue:missing" },
            "wrong-spline" => route with { CandidateSplineHandle = "urn:san:selfgel-candidate-spline:missing" },
            "wrong-witness" => route with { WitnessHandle = "urn:san:witness:wrong" },
            _ => route
        };

    private static void AssertCold(RehearsalDistinctionPressureReceipt receipt)
    {
        Assert.True(receipt.IsColdRehearsalDistinctionPressure);
        Assert.True(receipt.ReviewOnly);
        Assert.True(receipt.PressureOnly);
        Assert.True(receipt.EvidenceOnly);
        Assert.True(receipt.CoolingRequired);
        Assert.True(receipt.AuthorityAbsent);
        Assert.Equal(receipt.PriorPassageCount, receipt.PassageCountAfterPressure);
        Assert.Equal(receipt.PressureCases.Count, receipt.RetainedPressureCaseCount);
        Assert.False(receipt.PressureManufacturedLegitimacy);
        Assert.False(receipt.UrgencyCreatedJurisdiction);
        Assert.False(receipt.ConfidenceGrantedAuthority);
        Assert.False(receipt.SuccessCreatedPermission);
        Assert.False(receipt.RepetitionCreatedWarrant);
        Assert.False(receipt.FailureInvalidatedSelf);
        Assert.False(receipt.AmbiguityCollapsedToVictory);
        Assert.False(receipt.ImaginedFutureBecameEnactedState);
        Assert.False(receipt.IdentityDriftMutatedCorePosture);
        Assert.False(receipt.PressureAuthorizedAction);
        Assert.False(receipt.PressureAdmittedContinuity);
        Assert.False(receipt.LispEvaluationAllowed);
        Assert.False(receipt.NewMembranePacketEmitted);
        Assert.False(receipt.ReceiptsReplayed);
        Assert.False(receipt.AuthorityGranted);
        Assert.True(receipt.ActivationRefused);
    }

    private static void AssertRefused(RehearsalDistinctionPressureReceipt receipt, string outcomeCode)
    {
        Assert.Equal(RehearsalDistinctionPressureDisposition.Refused, receipt.Disposition);
        Assert.Equal(outcomeCode, receipt.OutcomeCode);
        Assert.True(receipt.IsRetainedRehearsalDistinctionPressureRefusal);
        Assert.NotNull(receipt.Refusal);
        Assert.True(receipt.Refusal.Retained);
        Assert.Empty(receipt.PressureCases);
        Assert.Empty(receipt.CoolingRoutes);
        Assert.Equal(receipt.PriorPassageCount, receipt.PassageCountAfterPressure);
        Assert.False(receipt.PressureAuthorizedAction);
        Assert.False(receipt.PressureAdmittedContinuity);
        Assert.False(receipt.AuthorityGranted);
        Assert.True(receipt.ActivationRefused);
    }

    private static string FindRepositoryRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null)
        {
            var candidate = Path.Combine(directory.FullName, "src", "SLI", "SLI.Lisp", "rehearsal-distinction-pressure.lisp");
            if (File.Exists(candidate))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        throw new DirectoryNotFoundException("Could not locate repository root from test output path.");
    }
}
