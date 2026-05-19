using San.Common;
using Xunit;

namespace San.Audit.Tests;

public sealed class EnactmentDryRunRehearsalBoundaryValidationTests
{
    private static readonly DateTimeOffset TimestampUtc = DateTimeOffset.Parse("2026-05-17T00:00:00Z");

    [Fact]
    public void Ready_Work_Packets_May_Enter_Dry_Run_Rehearsal_As_No_Op_Simulation()
    {
        var receipt = Declare(CreateRequest());

        Assert.Equal(EnactmentDryRunRehearsalDisposition.RehearsedCold, receipt.Disposition);
        Assert.Equal("enactment-dry-run-rehearsal-rehearsed-review-only", receipt.OutcomeCode);
        Assert.True(receipt.DryRunRehearsed);
        Assert.Equal(2, receipt.DryRunCases.Count);
        Assert.Equal(2, receipt.StewardReviewRoutes.Count);
        Assert.All(receipt.DryRunCases, dryRun => Assert.Contains("dry-run-rehearsal", dryRun.RehearsalHandle, StringComparison.Ordinal));
        AssertCold(receipt);
    }

    [Fact]
    public void Prospectus_Successful_Dry_Run_Is_Evidence_For_Interpretation_Not_Permission()
    {
        var receipt = Declare(CreateRequest(priorPassageCount: 1201));

        Assert.Equal(EnactmentDryRunRehearsalDisposition.RehearsedCold, receipt.Disposition);
        Assert.Equal(2, receipt.RetainedDryRunCaseCount);
        Assert.Equal(1201, receipt.PassageCountAfterDryRun);
        Assert.Contains("Ready work packets entered dry-run rehearsal", receipt.GovernanceTrace, StringComparison.Ordinal);
        Assert.Contains("simulation as permission", receipt.GovernanceTrace, StringComparison.Ordinal);
        Assert.All(receipt.DryRunCases, dryRun =>
        {
            Assert.True(dryRun.SimulationOnly);
            Assert.True(dryRun.NoOpOnly);
            Assert.True(dryRun.ReversibleOnly);
            Assert.False(dryRun.SimulationBecomesPermission);
            Assert.False(dryRun.DryRunAuthorizesAction);
            Assert.False(dryRun.DryRunExecutesAction);
            Assert.False(dryRun.DryRunMovesRuntime);
            Assert.False(dryRun.DryRunWritesOutsideReceiptSurface);
            Assert.False(dryRun.DryRunGrantsAuthority);
            Assert.False(dryRun.DryRunAdmitsContinuity);
        });
        Assert.False(receipt.DryRunBecamePermission);
        Assert.False(receipt.DryRunAuthorizedAction);
        Assert.False(receipt.DryRunExecutedAction);
        Assert.False(receipt.DryRunMovedRuntime);
        Assert.False(receipt.AuthorityGranted);
        Assert.False(receipt.ContinuityAdmitted);
        Assert.True(receipt.ActivationRefused);
    }

    [Theory]
    [InlineData(30, 2, "double")]
    [InlineData(60, 3, "triple")]
    [InlineData(90, 6, "deeper-chain")]
    public void Harmonic_Thirty_Sixty_Ninety_Rehearsal_Chains_Remain_Evidence_Not_Permission(
        int caseCount,
        int chainSize,
        string chainKind)
    {
        var source = CreateHarmonicSourceReadinessReceipt(caseCount);
        var receipt = Declare(CreateRequest(source: source, priorPassageCount: 1300 + caseCount));

        Assert.Equal(EnactmentDryRunRehearsalDisposition.RehearsedCold, receipt.Disposition);
        Assert.Equal(caseCount, receipt.RetainedDryRunCaseCount);
        Assert.Equal(caseCount, receipt.StewardReviewRoutes.Count);
        Assert.Equal(1300 + caseCount, receipt.PassageCountAfterDryRun);
        AssertCold(receipt);

        foreach (var chain in receipt.DryRunCases.Chunk(chainSize))
        {
            Assert.NotEmpty(chain);
            Assert.Equal(chain.Length, chain.Select(static dryRun => dryRun.RehearsalHandle).Distinct(StringComparer.Ordinal).Count());
            Assert.All(chain, dryRun =>
            {
                Assert.Contains(chainKind == "double" ? "harmonic" : "harmonic", dryRun.RehearsalHandle, StringComparison.Ordinal);
                Assert.True(dryRun.SimulationOnly);
                Assert.True(dryRun.NoOpOnly);
                Assert.True(dryRun.ReversibleOnly);
                Assert.False(dryRun.SimulationBecomesPermission);
                Assert.False(dryRun.DryRunAuthorizesAction);
                Assert.False(dryRun.DryRunExecutesAction);
                Assert.False(dryRun.DryRunGrantsAuthority);
                Assert.False(dryRun.DryRunAdmitsContinuity);
            });
        }

        Assert.False(receipt.DryRunBecamePermission);
        Assert.False(receipt.DryRunAuthorizedAction);
        Assert.False(receipt.DryRunGrantedAuthority);
        Assert.False(receipt.DryRunAdmittedContinuity);
    }

    [Fact]
    public void Thirty_Sixty_Ninety_Passes_Carry_Rehearsal_Resonance_Without_Passage_Inflation()
    {
        var passCounts = new[] { 30, 60, 90 };
        var priorPassage = 1500;
        var observedTelemetryRoutes = new HashSet<string>(StringComparer.Ordinal);

        foreach (var passCount in passCounts)
        {
            var source = CreateHarmonicSourceReadinessReceipt(
                passCount,
                sharedTelemetry: "telemetry:harmonic-rehearsal-register");
            var receipt = Declare(CreateRequest(source: source, priorPassageCount: priorPassage));

            Assert.Equal(passCount, receipt.RetainedDryRunCaseCount);
            Assert.Equal(priorPassage, receipt.PassageCountAfterDryRun);
            AssertCold(receipt);
            Assert.All(receipt.DryRunCases, dryRun =>
            {
                Assert.Equal("telemetry:harmonic-rehearsal-register", dryRun.TelemetryRoute);
                observedTelemetryRoutes.Add(dryRun.TelemetryRoute);
            });
            Assert.False(receipt.DryRunAuthorizedAction);
            Assert.False(receipt.AuthorityGranted);
            Assert.False(receipt.ContinuityAdmitted);

            priorPassage += passCount;
        }

        Assert.Single(observedTelemetryRoutes);
    }

    [Fact]
    public void Shared_Witness_And_Telemetry_Across_Ninety_Case_Chain_Do_Not_Create_Authority()
    {
        var source = CreateHarmonicSourceReadinessReceipt(
            90,
            sharedWitness: "urn:san:witness:harmonic-shared",
            sharedTelemetry: "telemetry:harmonic-shared");

        var receipt = Declare(CreateRequest(source: source, priorPassageCount: 1900));

        Assert.Equal(90, receipt.RetainedDryRunCaseCount);
        Assert.Single(receipt.DryRunCases.Select(static dryRun => dryRun.WitnessHandle).Distinct(StringComparer.Ordinal));
        Assert.Single(receipt.DryRunCases.Select(static dryRun => dryRun.TelemetryRoute).Distinct(StringComparer.Ordinal));
        Assert.Equal(1900, receipt.PassageCountAfterDryRun);
        Assert.False(receipt.DryRunBecamePermission);
        Assert.False(receipt.DryRunAuthorizedAction);
        Assert.False(receipt.StewardDryRunReviewMovedRuntime);
        Assert.False(receipt.AuthorityGranted);
        Assert.False(receipt.ContinuityAdmitted);
        Assert.True(receipt.ActivationRefused);
        AssertCold(receipt);
    }

    [Fact]
    public void Permission_Contamination_In_Ninety_Case_Harmonic_Chain_Refuses_Entire_Rehearsal()
    {
        var source = CreateHarmonicSourceReadinessReceipt(90);
        var dryRuns = CreateDryRunCases(source);
        dryRuns[44] = dryRuns[44] with { SimulationBecomesPermission = true };

        var receipt = Declare(CreateRequest(source: source, dryRunCases: dryRuns, routes: CreateRoutes(dryRuns)));

        AssertRefused(receipt, "enactment-dry-run-rehearsal-case-invalid");
        Assert.Empty(receipt.DryRunCases);
        Assert.Empty(receipt.StewardReviewRoutes);
        Assert.Equal(receipt.PriorPassageCount, receipt.PassageCountAfterDryRun);
    }

    [Fact]
    public void Route_Mismatch_In_Ninety_Case_Harmonic_Chain_Refuses_Lineage_Collapse()
    {
        var source = CreateHarmonicSourceReadinessReceipt(90);
        var dryRuns = CreateDryRunCases(source);
        var routes = CreateRoutes(dryRuns);
        routes[88] = routes[88] with { SourcePacketHandle = dryRuns[0].SourcePacketHandle };

        var receipt = Declare(CreateRequest(source: source, dryRunCases: dryRuns, routes: routes));

        AssertRefused(receipt, "enactment-dry-run-rehearsal-steward-route-invalid");
        Assert.Equal(receipt.PriorPassageCount, receipt.PassageCountAfterDryRun);
    }

    [Fact]
    public void Engineered_Cognition_Field_Limits_Exam_Allows_Broad_Rehearsal_As_Evidence_Only()
    {
        var source = CreateHarmonicSourceReadinessReceipt(
            90,
            sharedWitness: "urn:san:witness:ec-field-exam",
            sharedTelemetry: "telemetry:ec-field-exam");

        var receipt = Declare(CreateRequest(source: source, priorPassageCount: 2400));

        var rootCount = receipt.DryRunCases
            .Select(static dryRun => dryRun.SourceReadinessHandle)
            .Distinct(StringComparer.Ordinal)
            .Count();
        var branchCount = receipt.DryRunCases
            .Select(static dryRun => dryRun.RehearsalHandle)
            .Distinct(StringComparer.Ordinal)
            .Count();
        var trunkCount = receipt.DryRunCases
            .Select(static dryRun => (dryRun.WorkSurface, dryRun.IntendedWork, dryRun.MethodCode))
            .Distinct()
            .Count();

        Assert.Equal(90, rootCount);
        Assert.Equal(90, branchCount);
        Assert.Equal(1, trunkCount);
        Assert.Equal(2400, receipt.PassageCountAfterDryRun);
        Assert.Single(receipt.DryRunCases.Select(static dryRun => dryRun.WitnessHandle).Distinct(StringComparer.Ordinal));
        Assert.Single(receipt.DryRunCases.Select(static dryRun => dryRun.TelemetryRoute).Distinct(StringComparer.Ordinal));
        Assert.All(receipt.DryRunCases, dryRun =>
        {
            Assert.True(dryRun.ReviewOnly);
            Assert.True(dryRun.SimulationOnly);
            Assert.True(dryRun.NoOpOnly);
            Assert.True(dryRun.LocalOnly);
            Assert.True(dryRun.ReversibleOnly);
            Assert.True(dryRun.RequiresRollbackProof);
            Assert.True(dryRun.RequiresStewardReview);
            Assert.False(dryRun.SimulationBecomesPermission);
            Assert.False(dryRun.DryRunAuthorizesAction);
            Assert.False(dryRun.DryRunExecutesAction);
            Assert.False(dryRun.DryRunMovesRuntime);
            Assert.False(dryRun.DryRunWritesOutsideReceiptSurface);
            Assert.False(dryRun.DryRunGrantsAuthority);
            Assert.False(dryRun.DryRunAdmitsContinuity);
            Assert.False(dryRun.DryRunEvaluatesLisp);
            Assert.False(dryRun.DryRunEmitsMembranePacket);
            Assert.False(dryRun.DryRunReplaysReceipt);
            Assert.False(dryRun.DryRunIncrementsPassage);
            Assert.False(dryRun.DryRunActivates);
        });
        AssertCold(receipt);
    }

    [Theory]
    [InlineData("lineage-ceiling", "enactment-dry-run-rehearsal-lineage-invalid")]
    [InlineData("warrant-ceiling", "enactment-dry-run-rehearsal-case-invalid")]
    [InlineData("continuity-ceiling", "enactment-dry-run-rehearsal-case-invalid")]
    [InlineData("action-ceiling", "enactment-dry-run-rehearsal-case-invalid")]
    [InlineData("authority-ceiling", "enactment-dry-run-rehearsal-case-invalid")]
    [InlineData("telemetry-command-ceiling", "enactment-dry-run-rehearsal-lineage-invalid")]
    [InlineData("steward-ceiling", "enactment-dry-run-rehearsal-case-invalid")]
    [InlineData("cooling-ceiling", "enactment-dry-run-rehearsal-steward-route-invalid")]
    [InlineData("passage-ceiling", "enactment-dry-run-rehearsal-case-invalid")]
    [InlineData("runtime-ceiling", "enactment-dry-run-rehearsal-case-invalid")]
    [InlineData("lisp-ceiling", "enactment-dry-run-rehearsal-case-invalid")]
    [InlineData("packet-ceiling", "enactment-dry-run-rehearsal-case-invalid")]
    public void Engineered_Cognition_Field_Limits_Exam_Refuses_Ceiling_Crossings(
        string ceiling,
        string expectedOutcome)
    {
        var source = CreateHarmonicSourceReadinessReceipt(90);
        var dryRuns = CreateDryRunCases(source);
        var routes = CreateRoutes(dryRuns);

        switch (ceiling)
        {
            case "lineage-ceiling":
                dryRuns[13] = dryRuns[13] with { SourceReadinessHandle = "urn:san:enactment-boundary-readiness:unrooted" };
                break;
            case "warrant-ceiling":
                dryRuns[13] = dryRuns[13] with { SimulationBecomesPermission = true };
                break;
            case "continuity-ceiling":
                dryRuns[13] = dryRuns[13] with { DryRunAdmitsContinuity = true };
                break;
            case "action-ceiling":
                dryRuns[13] = dryRuns[13] with { DryRunAuthorizesAction = true };
                break;
            case "authority-ceiling":
                dryRuns[13] = dryRuns[13] with { DryRunGrantsAuthority = true };
                break;
            case "telemetry-command-ceiling":
                dryRuns[13] = dryRuns[13] with { TelemetryRoute = "telemetry:command-channel" };
                break;
            case "steward-ceiling":
                dryRuns[13] = dryRuns[13] with { RequiresStewardReview = false };
                break;
            case "cooling-ceiling":
                routes[13] = routes[13] with { RequiresCooling = false };
                break;
            case "passage-ceiling":
                dryRuns[13] = dryRuns[13] with { DryRunIncrementsPassage = true };
                break;
            case "runtime-ceiling":
                dryRuns[13] = dryRuns[13] with { DryRunMovesRuntime = true };
                break;
            case "lisp-ceiling":
                dryRuns[13] = dryRuns[13] with { DryRunEvaluatesLisp = true };
                break;
            case "packet-ceiling":
                dryRuns[13] = dryRuns[13] with { DryRunEmitsMembranePacket = true };
                break;
            default:
                throw new InvalidOperationException($"Unknown EC field ceiling exam case: {ceiling}");
        }

        var receipt = Declare(CreateRequest(
            source: source,
            dryRunCases: dryRuns,
            routes: routes,
            priorPassageCount: 2500));

        AssertRefused(receipt, expectedOutcome);
        Assert.Equal(2500, receipt.PassageCountAfterDryRun);
        Assert.False(receipt.DryRunAuthorizedAction);
        Assert.False(receipt.DryRunExecutedAction);
        Assert.False(receipt.AuthorityGranted);
        Assert.False(receipt.ContinuityAdmitted);
        Assert.True(receipt.ActivationRefused);
    }

    [Fact]
    public void Empty_Dry_Run_Rehearsal_Is_Reviewable_But_Not_Authoritative()
    {
        var source = CreateSourceReadinessReceipt(candidates: [], routes: []);
        var receipt = Declare(CreateRequest(source: source, dryRunCases: [], routes: []));

        Assert.Equal(EnactmentDryRunRehearsalDisposition.EmptyRehearsalCold, receipt.Disposition);
        Assert.Equal("enactment-dry-run-rehearsal-empty-review-only", receipt.OutcomeCode);
        Assert.Empty(receipt.DryRunCases);
        Assert.Empty(receipt.StewardReviewRoutes);
        Assert.False(receipt.DryRunRehearsed);
        AssertCold(receipt);
    }

    [Fact]
    public void Dry_Run_Rehearsal_Preserves_Readiness_And_Packet_Lineage()
    {
        var source = CreateSourceReadinessReceipt();
        var receipt = Declare(CreateRequest(source: source));

        AssertCold(receipt);
        Assert.Equal(source.ReceiptHandle, receipt.SourceReadinessReceiptHandle);
        Assert.All(receipt.DryRunCases, dryRun =>
        {
            var candidate = source.Candidates.Single(item => item.ReadinessHandle == dryRun.SourceReadinessHandle);

            Assert.Equal(candidate.SourcePacketHandle, dryRun.SourcePacketHandle);
            Assert.Equal(candidate.DryRunPlanHandle, dryRun.DryRunPlanHandle);
            Assert.Equal(candidate.DutyStation, dryRun.DutyStation);
            Assert.Equal(candidate.WorkSurface, dryRun.WorkSurface);
            Assert.Equal(candidate.IntendedWork, dryRun.IntendedWork);
            Assert.Equal(candidate.MethodCode, dryRun.MethodCode);
            Assert.Equal(candidate.CustodyOwner, dryRun.CustodyOwner);
            Assert.Equal(candidate.WitnessHandle, dryRun.WitnessHandle);
            Assert.Equal(candidate.TelemetryRoute, dryRun.TelemetryRoute);
            Assert.Equal(candidate.StewardReviewHandle, dryRun.StewardReviewHandle);
        });
    }

    [Fact]
    public void Dry_Run_Rehearsal_Requires_Cold_Enactment_Readiness_Source()
    {
        var receipt = Declare(CreateRequest(omitSource: true));

        AssertRefused(receipt, "enactment-dry-run-rehearsal-source-readiness-missing");
    }

    [Theory]
    [InlineData("missing-boundary")]
    [InlineData("not-review")]
    [InlineData("no-rehearsal")]
    [InlineData("no-readiness")]
    [InlineData("no-plan")]
    [InlineData("no-effect")]
    [InlineData("no-rollback")]
    [InlineData("no-noop")]
    [InlineData("no-locality")]
    [InlineData("no-reversibility")]
    [InlineData("no-custody")]
    [InlineData("no-witness")]
    [InlineData("no-telemetry")]
    [InlineData("no-steward-review")]
    [InlineData("permission")]
    [InlineData("authorize")]
    [InlineData("execute")]
    [InlineData("runtime")]
    [InlineData("outside-write")]
    [InlineData("authority")]
    [InlineData("continuity")]
    [InlineData("lisp")]
    [InlineData("packet")]
    [InlineData("replay")]
    [InlineData("passage")]
    [InlineData("activation")]
    public void Scope_Refuses_Dry_Run_As_Permission_Or_Enactment(string mutation)
    {
        var receipt = Declare(CreateRequest(scope: MutateScope(CreateScope(), mutation)));

        var expected = mutation == "missing-boundary"
            ? "enactment-dry-run-rehearsal-scope-missing"
            : "enactment-dry-run-rehearsal-scope-promotional";
        AssertRefused(receipt, expected);
    }

    [Theory]
    [InlineData("permission")]
    [InlineData("authorize")]
    [InlineData("execute")]
    [InlineData("runtime")]
    [InlineData("outside-write")]
    [InlineData("authority")]
    [InlineData("continuity")]
    [InlineData("steward-runtime")]
    [InlineData("simulation-permission")]
    [InlineData("reversible-authorize")]
    [InlineData("no-noop")]
    [InlineData("no-rollback")]
    [InlineData("lisp")]
    [InlineData("packet")]
    [InlineData("replay")]
    [InlineData("passage")]
    [InlineData("activation")]
    public void Non_Enactment_Boundary_Refuses_Rehearsal_As_Work(string mutation)
    {
        var receipt = Declare(CreateRequest(nonEnactment: MutateNonEnactment(CreateNonEnactment(), mutation)));

        AssertRefused(receipt, "enactment-dry-run-rehearsal-non-enactment-invalid");
    }

    [Theory]
    [InlineData("missing-rehearsal")]
    [InlineData("missing-readiness")]
    [InlineData("missing-packet")]
    [InlineData("missing-plan")]
    [InlineData("missing-duty")]
    [InlineData("missing-surface")]
    [InlineData("missing-work")]
    [InlineData("missing-method")]
    [InlineData("missing-effect")]
    [InlineData("missing-rollback")]
    [InlineData("missing-custody")]
    [InlineData("missing-witness")]
    [InlineData("missing-telemetry")]
    [InlineData("missing-steward")]
    [InlineData("not-review")]
    [InlineData("not-simulation")]
    [InlineData("not-noop")]
    [InlineData("not-local")]
    [InlineData("not-reversible")]
    [InlineData("no-rollback")]
    [InlineData("no-steward-review")]
    [InlineData("permission")]
    [InlineData("authorize")]
    [InlineData("execute")]
    [InlineData("runtime")]
    [InlineData("outside-write")]
    [InlineData("authority")]
    [InlineData("continuity")]
    [InlineData("lisp")]
    [InlineData("packet")]
    [InlineData("replay")]
    [InlineData("passage")]
    [InlineData("activation")]
    public void Dry_Run_Case_Remains_No_Op_Simulation(string mutation)
    {
        var source = CreateSourceReadinessReceipt();
        var dryRuns = CreateDryRunCases(source);
        dryRuns[0] = MutateDryRunCase(dryRuns[0], mutation);

        var receipt = Declare(CreateRequest(source: source, dryRunCases: dryRuns));

        AssertRefused(receipt, "enactment-dry-run-rehearsal-case-invalid");
    }

    [Theory]
    [InlineData("wrong-readiness")]
    [InlineData("wrong-packet")]
    [InlineData("wrong-plan")]
    [InlineData("wrong-duty")]
    [InlineData("wrong-method")]
    [InlineData("wrong-custody")]
    [InlineData("wrong-witness")]
    [InlineData("wrong-steward")]
    public void Dry_Run_Case_Must_Bind_To_Source_Readiness_Lineage(string mutation)
    {
        var source = CreateSourceReadinessReceipt();
        var dryRuns = CreateDryRunCases(source);
        dryRuns[0] = mutation switch
        {
            "wrong-readiness" => dryRuns[0] with { SourceReadinessHandle = "urn:san:enactment-boundary-readiness:missing" },
            "wrong-packet" => dryRuns[0] with { SourcePacketHandle = "urn:san:scoped-work-packet:missing" },
            "wrong-plan" => dryRuns[0] with { DryRunPlanHandle = "urn:san:dry-run-plan:missing" },
            "wrong-duty" => dryRuns[0] with { DutyStation = "lab.local.wrong-duty" },
            "wrong-method" => dryRuns[0] with { MethodCode = "method:wrong" },
            "wrong-custody" => dryRuns[0] with { CustodyOwner = SanctuaryPacketSurfaces.Cryptic },
            "wrong-witness" => dryRuns[0] with { WitnessHandle = "urn:san:witness:wrong" },
            "wrong-steward" => dryRuns[0] with { StewardReviewHandle = "urn:san:steward-review:wrong" },
            _ => dryRuns[0]
        };

        var receipt = Declare(CreateRequest(source: source, dryRunCases: dryRuns));

        AssertRefused(receipt, "enactment-dry-run-rehearsal-lineage-invalid");
    }

    [Fact]
    public void Dry_Run_Rehearsal_Refuses_Duplicate_Rehearsal_Handles()
    {
        var source = CreateSourceReadinessReceipt();
        var dryRuns = CreateDryRunCases(source);
        dryRuns[1] = dryRuns[1] with { RehearsalHandle = dryRuns[0].RehearsalHandle };

        var receipt = Declare(CreateRequest(source: source, dryRunCases: dryRuns));

        AssertRefused(receipt, "enactment-dry-run-rehearsal-duplicate-case-handle");
    }

    [Theory]
    [InlineData("missing-route")]
    [InlineData("missing-rehearsal")]
    [InlineData("missing-readiness")]
    [InlineData("missing-packet")]
    [InlineData("missing-steward")]
    [InlineData("missing-custody")]
    [InlineData("missing-evidence")]
    [InlineData("missing-witness")]
    [InlineData("missing-telemetry")]
    [InlineData("missing-return")]
    [InlineData("not-review")]
    [InlineData("no-rehearsal-lineage")]
    [InlineData("no-readiness-lineage")]
    [InlineData("no-packet-lineage")]
    [InlineData("no-plan-lineage")]
    [InlineData("no-steward-review")]
    [InlineData("no-cooling")]
    [InlineData("authorize")]
    [InlineData("execute")]
    [InlineData("runtime")]
    [InlineData("authority")]
    [InlineData("continuity")]
    [InlineData("lisp")]
    [InlineData("packet")]
    [InlineData("replay")]
    [InlineData("passage")]
    [InlineData("activation")]
    [InlineData("unknown-rehearsal")]
    [InlineData("wrong-custody")]
    public void Steward_Dry_Run_Route_Remains_Review_Only(string mutation)
    {
        var source = CreateSourceReadinessReceipt();
        var dryRuns = CreateDryRunCases(source);
        var routes = CreateRoutes(dryRuns);
        routes[0] = MutateRoute(routes[0], mutation);

        var receipt = Declare(CreateRequest(source: source, dryRunCases: dryRuns, routes: routes));

        AssertRefused(receipt, "enactment-dry-run-rehearsal-steward-route-invalid");
    }

    [Fact]
    public void Steward_Dry_Run_Route_Requires_Unique_Handle()
    {
        var source = CreateSourceReadinessReceipt();
        var dryRuns = CreateDryRunCases(source);
        var routes = CreateRoutes(dryRuns);
        routes[1] = routes[1] with { ReviewRouteHandle = routes[0].ReviewRouteHandle };

        var receipt = Declare(CreateRequest(source: source, dryRunCases: dryRuns, routes: routes));

        AssertRefused(receipt, "enactment-dry-run-rehearsal-duplicate-route-handle");
    }

    [Fact]
    public void Non_Empty_Dry_Run_Rehearsal_Requires_Steward_Route()
    {
        var source = CreateSourceReadinessReceipt();
        var dryRuns = CreateDryRunCases(source);

        var receipt = Declare(CreateRequest(source: source, dryRunCases: dryRuns, routes: []));

        AssertRefused(receipt, "enactment-dry-run-rehearsal-steward-route-missing");
    }

    [Fact]
    public void Dry_Run_Rehearsal_Does_Not_Write_Outside_Receipt_Surface_Replay_Or_Increment_Passage()
    {
        var receipt = Declare(CreateRequest(priorPassageCount: 991));

        Assert.Equal(991, receipt.PriorPassageCount);
        Assert.Equal(991, receipt.PassageCountAfterDryRun);
        Assert.False(receipt.DryRunBecamePermission);
        Assert.False(receipt.DryRunAuthorizedAction);
        Assert.False(receipt.DryRunExecutedAction);
        Assert.False(receipt.DryRunMovedRuntime);
        Assert.False(receipt.DryRunWroteOutsideReceiptSurface);
        Assert.False(receipt.StewardDryRunReviewMovedRuntime);
        Assert.False(receipt.ReversibleLocalEffectAuthorizedAction);
        Assert.False(receipt.LispEvaluationAllowed);
        Assert.False(receipt.NewMembranePacketEmitted);
        Assert.False(receipt.ReceiptsReplayed);
        Assert.True(receipt.ActivationRefused);
        AssertCold(receipt);
    }

    [Fact]
    public void Lisp_Body_Declares_Dry_Run_Rehearsal_As_Inert_Non_Enactment_Register()
    {
        var lispPath = Path.Combine(
            FindRepositoryRoot(),
            "src",
            "SLI",
            "SLI.Lisp",
            "enactment-dry-run-rehearsal.lisp");

        var body = File.ReadAllText(lispPath);

        Assert.Contains(":posture :cme-enactment-dry-run-rehearsal-boundary", body, StringComparison.Ordinal);
        Assert.Contains(":lisp-role :inert-dry-run-rehearsal-carrier", body, StringComparison.Ordinal);
        Assert.Contains(":core-invariant \"dry-run rehearsal is not enactment\"", body, StringComparison.Ordinal);
        Assert.Contains(":simulation-becomes-permission nil", body, StringComparison.Ordinal);
        Assert.Contains(":dry-run-authorizes-action nil", body, StringComparison.Ordinal);
        Assert.Contains(":dry-run-executes-action nil", body, StringComparison.Ordinal);
        Assert.Contains(":dry-run-moves-runtime nil", body, StringComparison.Ordinal);
        Assert.Contains(":dry-run-writes-outside-receipt-surface nil", body, StringComparison.Ordinal);
        Assert.Contains(":steward-dry-run-review-moves-runtime nil", body, StringComparison.Ordinal);
        Assert.Contains(":lisp-evaluation-allowed nil", body, StringComparison.Ordinal);
        Assert.Contains(":return :receipt-only", body, StringComparison.Ordinal);
        Assert.DoesNotContain("(eval", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("(compile", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("(load", body, StringComparison.OrdinalIgnoreCase);
    }

    private static EnactmentDryRunRehearsalReceipt Declare(EnactmentDryRunRehearsalRequest request) =>
        new DefaultEnactmentDryRunRehearsalBoundaryValidator().Declare(request, TimestampUtc);

    private static EnactmentDryRunRehearsalRequest CreateRequest(
        EnactmentBoundaryReadinessReceipt? source = null,
        IReadOnlyList<EnactmentDryRunCase>? dryRunCases = null,
        IReadOnlyList<StewardDryRunReviewReceiptRoute>? routes = null,
        EnactmentDryRunScopeBoundary? scope = null,
        EnactmentDryRunNonEnactmentBoundary? nonEnactment = null,
        int priorPassageCount = 808,
        bool omitSource = false)
    {
        var sourceReceipt = omitSource ? null : source ?? CreateSourceReadinessReceipt();
        var defaultSource = sourceReceipt ?? CreateSourceReadinessReceipt();
        var dryRuns = dryRunCases ?? CreateDryRunCases(defaultSource);
        return new(
            SourceReadinessReceipt: sourceReceipt,
            DryRunCases: dryRuns,
            StewardReviewRoutes: routes ?? CreateRoutes(dryRuns),
            ScopeBoundary: scope ?? CreateScope(),
            NonEnactmentBoundary: nonEnactment ?? CreateNonEnactment(),
            PriorPassageCount: priorPassageCount);
    }

    private static EnactmentBoundaryReadinessReceipt CreateSourceReadinessReceipt(
        IReadOnlyList<EnactmentBoundaryReadinessCandidate>? candidates = null,
        IReadOnlyList<EnactmentBoundaryStewardReviewRoute>? routes = null)
    {
        var candidateSet = candidates ?? CreateReadinessCandidates();
        var routeSet = routes ?? CreateReadinessRoutes(candidateSet);
        var disposition = candidateSet.Count == 0
            ? EnactmentBoundaryReadinessDisposition.EmptyReviewCold
            : EnactmentBoundaryReadinessDisposition.ReadyForEnactmentBoundaryReviewCold;

        return new(
            ReceiptHandle: "urn:san:enactment-boundary-readiness:review:test-source",
            Disposition: disposition,
            OutcomeCode: "enactment-boundary-readiness-ready-review-only",
            GovernanceTrace: "test source readiness",
            SourcePacketFormationReceiptHandle: "urn:san:scoped-work-packet-formation:test-source",
            Candidates: candidateSet,
            StewardReviewRoutes: routeSet,
            ScopeBoundary: CreateReadinessScope(),
            NonExecutionBoundary: CreateReadinessNonExecution(),
            Refusal: null,
            PriorPassageCount: 704,
            PassageCountAfterReadiness: 704,
            RetainedCandidateCount: candidateSet.Count,
            ReviewOnly: true,
            ApproachOnly: true,
            ReadyForEnactmentBoundaryReview: candidateSet.Count > 0,
            ReadinessBecameWarrant: false,
            ReadinessBecameAdmission: false,
            ReadinessGrantedAuthority: false,
            ReadinessAdmittedContinuity: false,
            ReadinessAuthorizedAction: false,
            ReadinessExecutedAction: false,
            ApproachAuthorizedAction: false,
            LocalityAuthorizedAction: false,
            ReversibilityAuthorizedAction: false,
            StewardReviewMovedRuntime: false,
            DryRunPlanExecuted: false,
            RuntimeActionAllowed: false,
            LispEvaluationAllowed: false,
            NewMembranePacketEmitted: false,
            ReceiptsReplayed: false,
            ContinuityAdmitted: false,
            AuthorityGranted: false,
            ActivationRefused: true,
            TimestampUtc: TimestampUtc);
    }

    private static EnactmentBoundaryReadinessCandidate[] CreateReadinessCandidates() =>
    [
        ReadinessCandidate("selected-prime", SanctuaryPacketSurfaces.Prime),
        ReadinessCandidate("selected-steward", SanctuaryPacketSurfaces.Steward)
    ];

    private static EnactmentBoundaryReadinessReceipt CreateHarmonicSourceReadinessReceipt(
        int candidateCount,
        string? sharedWitness = null,
        string? sharedTelemetry = null)
    {
        var candidates = Enumerable.Range(1, candidateCount)
            .Select(index =>
            {
                var custodyOwner = (index % 3) switch
                {
                    0 => SanctuaryPacketSurfaces.Cryptic,
                    1 => SanctuaryPacketSurfaces.Prime,
                    _ => SanctuaryPacketSurfaces.Steward
                };
                var candidate = ReadinessCandidate($"harmonic-{index:000}", custodyOwner);
                return candidate with
                {
                    WitnessHandle = sharedWitness ?? candidate.WitnessHandle,
                    TelemetryRoute = sharedTelemetry ?? candidate.TelemetryRoute
                };
            })
            .ToArray();

        return CreateSourceReadinessReceipt(candidates, CreateReadinessRoutes(candidates));
    }

    private static EnactmentBoundaryReadinessCandidate ReadinessCandidate(string suffix, string custodyOwner) =>
        new(
            ReadinessHandle: $"urn:san:enactment-boundary-readiness:{suffix}",
            SourcePacketHandle: $"urn:san:scoped-work-packet:{suffix}",
            SourceStewardRouteHandle: $"urn:san:steward-route:scoped-work-packet:{suffix}",
            DutyStation: $"lab.local.{suffix}",
            WorkSurface: "local-reversible-review-receipt",
            IntendedWork: "prepare-local-reversible-review-work-without-enactment",
            MethodCode: "method:dry-run-ready-review-only",
            AuthorityCeiling: "authority:steward-review-required",
            LocalEffectCeiling: "local-effect:receipt-only-under-tiny-bicycle-lab",
            ReversibilityProofHandle: $"urn:san:reversibility-proof:enactment-boundary:{suffix}",
            DryRunPlanHandle: $"urn:san:dry-run-plan:enactment-boundary:{suffix}",
            CustodyOwner: custodyOwner,
            WitnessHandle: $"urn:san:witness:enactment-boundary-readiness:{suffix}",
            TelemetryRoute: "telemetry:string",
            StewardReviewHandle: $"urn:san:steward-review:enactment-boundary:{suffix}",
            RevocationPath: $"urn:san:revocation:enactment-boundary:{suffix}",
            RepairPath: $"urn:san:repair:enactment-boundary:{suffix}",
            LossCondition: "loss:readiness-treated-as-enactment",
            ReviewOnly: true,
            ApproachOnly: true,
            LocalOnly: true,
            ReversibleOnly: true,
            RequiresStewardReview: true,
            RequiresDryRunBeforeExecution: true,
            RequiresSeparateActionHarness: true,
            ReadinessBecomesWarrant: false,
            ReadinessBecomesAdmission: false,
            ReadinessGrantsAuthority: false,
            ReadinessAdmitsContinuity: false,
            ReadinessAuthorizesAction: false,
            ReadinessExecutesAction: false,
            ApproachMovesRuntime: false,
            LocalityAuthorizesAction: false,
            ReversibilityAuthorizesAction: false,
            StewardReviewMovesRuntime: false,
            ReadinessEvaluatesLisp: false,
            ReadinessEmitsMembranePacket: false,
            ReadinessReplaysReceipt: false,
            ReadinessIncrementsPassage: false,
            ReadinessActivates: false);

    private static EnactmentBoundaryStewardReviewRoute[] CreateReadinessRoutes(
        IReadOnlyList<EnactmentBoundaryReadinessCandidate> candidates) =>
        candidates.Select(candidate => new EnactmentBoundaryStewardReviewRoute(
            ReviewRouteHandle: candidate.StewardReviewHandle,
            ReadinessHandle: candidate.ReadinessHandle,
            SourcePacketHandle: candidate.SourcePacketHandle,
            StewardSurface: SanctuaryPacketSurfaces.Steward,
            CustodyOwner: candidate.CustodyOwner,
            EvidenceHandle: $"urn:san:evidence:enactment-boundary:{candidate.ReadinessHandle.Split(':').Last()}",
            WitnessHandle: candidate.WitnessHandle,
            TelemetryRoute: candidate.TelemetryRoute,
            ReturnPathHandle: candidate.RepairPath,
            ReviewOnly: true,
            PreservesReadinessLineage: true,
            PreservesPacketLineage: true,
            PreservesStewardRouteLineage: true,
            RoutesToStewardEnactmentReview: true,
            RequiresCooling: true,
            RequiresSeparateActionHarness: true,
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

    private static EnactmentBoundaryReadinessScopeBoundary CreateReadinessScope() =>
        new(
            BoundaryCode: "enactment-boundary-readiness-review-only",
            Present: true,
            ReviewOnly: true,
            AllowsReadinessDeclaration: true,
            RequiresScopedWorkPacketReceipt: true,
            RequiresStewardRoute: true,
            RequiresDutyStation: true,
            RequiresWorkSurface: true,
            RequiresIntendedWork: true,
            RequiresMethodCode: true,
            RequiresAuthorityCeiling: true,
            RequiresLocalEffectCeiling: true,
            RequiresReversibilityProof: true,
            RequiresDryRunPlan: true,
            RequiresCustody: true,
            RequiresWitness: true,
            RequiresTelemetryRoute: true,
            RequiresStewardReview: true,
            RequiresRevocationPath: true,
            RequiresRepairPath: true,
            RequiresLossCondition: true,
            RequiresSeparateActionHarness: true,
            AllowsReadinessAsWarrant: false,
            AllowsReadinessAsAdmission: false,
            AllowsReadinessAsAuthority: false,
            AllowsReadinessAsContinuity: false,
            AllowsActionAuthorization: false,
            AllowsExecution: false,
            AllowsRuntimeMotion: false,
            AllowsLispEvaluation: false,
            AllowsMembranePacketEmission: false,
            AllowsReceiptReplay: false,
            AllowsPassageIncrement: false,
            AllowsActivation: false);

    private static EnactmentBoundaryReadinessNonExecutionBoundary CreateReadinessNonExecution() =>
        new(
            BoundaryLaw: "Approach is not enactment.",
            ReadinessMayBecomeWarrant: false,
            ReadinessMayBecomeAdmission: false,
            ReadinessMayAuthorize: false,
            ReadinessMayExecute: false,
            ReadinessMayMoveRuntime: false,
            ReadinessMayGrantAuthority: false,
            ReadinessMayAdmitContinuity: false,
            ApproachMayAuthorize: false,
            LocalityMayAuthorize: false,
            ReversibilityMayAuthorize: false,
            StewardReviewMayMoveRuntime: false,
            DryRunPlanMayExecute: false,
            SeparateActionHarnessRequired: true,
            ReadinessMayEvaluateLisp: false,
            ReadinessMayEmitMembranePacket: false,
            ReadinessMayReplayReceipt: false,
            ReadinessMayIncrementPassage: false,
            ReadinessMayActivate: false);

    private static EnactmentDryRunCase[] CreateDryRunCases(EnactmentBoundaryReadinessReceipt source) =>
        source.Candidates.Select(candidate =>
        {
            var suffix = candidate.ReadinessHandle.Split(':').Last();
            return new EnactmentDryRunCase(
                RehearsalHandle: $"urn:san:enactment-dry-run-rehearsal:{suffix}",
                SourceReadinessHandle: candidate.ReadinessHandle,
                SourcePacketHandle: candidate.SourcePacketHandle,
                DryRunPlanHandle: candidate.DryRunPlanHandle,
                DutyStation: candidate.DutyStation,
                WorkSurface: candidate.WorkSurface,
                IntendedWork: candidate.IntendedWork,
                MethodCode: candidate.MethodCode,
                SimulatedEffectHandle: $"urn:san:simulated-effect:dry-run-rehearsal:{suffix}",
                RollbackProofHandle: $"urn:san:rollback-proof:dry-run-rehearsal:{suffix}",
                CustodyOwner: candidate.CustodyOwner,
                WitnessHandle: candidate.WitnessHandle,
                TelemetryRoute: candidate.TelemetryRoute,
                StewardReviewHandle: candidate.StewardReviewHandle,
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

    private static StewardDryRunReviewReceiptRoute[] CreateRoutes(IReadOnlyList<EnactmentDryRunCase> dryRuns) =>
        dryRuns.Select(dryRun =>
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

    private static EnactmentDryRunScopeBoundary CreateScope() =>
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

    private static EnactmentDryRunNonEnactmentBoundary CreateNonEnactment() =>
        new(
            BoundaryLaw: "A ready work packet may enter dry-run rehearsal. Dry-run rehearsal is not enactment. Simulation is not permission.",
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

    private static EnactmentDryRunScopeBoundary MutateScope(EnactmentDryRunScopeBoundary scope, string mutation) =>
        mutation switch
        {
            "missing-boundary" => scope with { BoundaryCode = string.Empty, Present = false },
            "not-review" => scope with { ReviewOnly = false },
            "no-rehearsal" => scope with { AllowsDryRunRehearsal = false },
            "no-readiness" => scope with { RequiresReadinessReceipt = false },
            "no-plan" => scope with { RequiresDryRunPlan = false },
            "no-effect" => scope with { RequiresSimulatedEffect = false },
            "no-rollback" => scope with { RequiresRollbackProof = false },
            "no-noop" => scope with { RequiresNoOp = false },
            "no-locality" => scope with { RequiresLocality = false },
            "no-reversibility" => scope with { RequiresReversibility = false },
            "no-custody" => scope with { RequiresCustody = false },
            "no-witness" => scope with { RequiresWitness = false },
            "no-telemetry" => scope with { RequiresTelemetryRoute = false },
            "no-steward-review" => scope with { RequiresStewardReview = false },
            "permission" => scope with { AllowsSimulationAsPermission = true },
            "authorize" => scope with { AllowsActionAuthorization = true },
            "execute" => scope with { AllowsExecution = true },
            "runtime" => scope with { AllowsRuntimeMotion = true },
            "outside-write" => scope with { AllowsOutsideReceiptSurfaceWrite = true },
            "authority" => scope with { AllowsAuthority = true },
            "continuity" => scope with { AllowsContinuityAdmission = true },
            "lisp" => scope with { AllowsLispEvaluation = true },
            "packet" => scope with { AllowsMembranePacketEmission = true },
            "replay" => scope with { AllowsReceiptReplay = true },
            "passage" => scope with { AllowsPassageIncrement = true },
            "activation" => scope with { AllowsActivation = true },
            _ => scope
        };

    private static EnactmentDryRunNonEnactmentBoundary MutateNonEnactment(
        EnactmentDryRunNonEnactmentBoundary boundary,
        string mutation) =>
        mutation switch
        {
            "permission" => boundary with { DryRunMayBecomePermission = true },
            "authorize" => boundary with { DryRunMayAuthorize = true },
            "execute" => boundary with { DryRunMayExecute = true },
            "runtime" => boundary with { DryRunMayMoveRuntime = true },
            "outside-write" => boundary with { DryRunMayWriteOutsideReceiptSurface = true },
            "authority" => boundary with { DryRunMayGrantAuthority = true },
            "continuity" => boundary with { DryRunMayAdmitContinuity = true },
            "steward-runtime" => boundary with { StewardDryRunReviewMayMoveRuntime = true },
            "simulation-permission" => boundary with { SimulationMayBecomePermission = true },
            "reversible-authorize" => boundary with { ReversibleLocalEffectMayAuthorize = true },
            "no-noop" => boundary with { NoOpRequired = false },
            "no-rollback" => boundary with { RollbackProofRequired = false },
            "lisp" => boundary with { DryRunMayEvaluateLisp = true },
            "packet" => boundary with { DryRunMayEmitMembranePacket = true },
            "replay" => boundary with { DryRunMayReplayReceipt = true },
            "passage" => boundary with { DryRunMayIncrementPassage = true },
            "activation" => boundary with { DryRunMayActivate = true },
            _ => boundary
        };

    private static EnactmentDryRunCase MutateDryRunCase(EnactmentDryRunCase dryRun, string mutation) =>
        mutation switch
        {
            "missing-rehearsal" => dryRun with { RehearsalHandle = string.Empty },
            "missing-readiness" => dryRun with { SourceReadinessHandle = string.Empty },
            "missing-packet" => dryRun with { SourcePacketHandle = string.Empty },
            "missing-plan" => dryRun with { DryRunPlanHandle = string.Empty },
            "missing-duty" => dryRun with { DutyStation = string.Empty },
            "missing-surface" => dryRun with { WorkSurface = string.Empty },
            "missing-work" => dryRun with { IntendedWork = string.Empty },
            "missing-method" => dryRun with { MethodCode = string.Empty },
            "missing-effect" => dryRun with { SimulatedEffectHandle = string.Empty },
            "missing-rollback" => dryRun with { RollbackProofHandle = string.Empty },
            "missing-custody" => dryRun with { CustodyOwner = string.Empty },
            "missing-witness" => dryRun with { WitnessHandle = string.Empty },
            "missing-telemetry" => dryRun with { TelemetryRoute = string.Empty },
            "missing-steward" => dryRun with { StewardReviewHandle = string.Empty },
            "not-review" => dryRun with { ReviewOnly = false },
            "not-simulation" => dryRun with { SimulationOnly = false },
            "not-noop" => dryRun with { NoOpOnly = false },
            "not-local" => dryRun with { LocalOnly = false },
            "not-reversible" => dryRun with { ReversibleOnly = false },
            "no-rollback" => dryRun with { RequiresRollbackProof = false },
            "no-steward-review" => dryRun with { RequiresStewardReview = false },
            "permission" => dryRun with { SimulationBecomesPermission = true },
            "authorize" => dryRun with { DryRunAuthorizesAction = true },
            "execute" => dryRun with { DryRunExecutesAction = true },
            "runtime" => dryRun with { DryRunMovesRuntime = true },
            "outside-write" => dryRun with { DryRunWritesOutsideReceiptSurface = true },
            "authority" => dryRun with { DryRunGrantsAuthority = true },
            "continuity" => dryRun with { DryRunAdmitsContinuity = true },
            "lisp" => dryRun with { DryRunEvaluatesLisp = true },
            "packet" => dryRun with { DryRunEmitsMembranePacket = true },
            "replay" => dryRun with { DryRunReplaysReceipt = true },
            "passage" => dryRun with { DryRunIncrementsPassage = true },
            "activation" => dryRun with { DryRunActivates = true },
            _ => dryRun
        };

    private static StewardDryRunReviewReceiptRoute MutateRoute(
        StewardDryRunReviewReceiptRoute route,
        string mutation) =>
        mutation switch
        {
            "missing-route" => route with { ReviewRouteHandle = string.Empty },
            "missing-rehearsal" => route with { RehearsalHandle = string.Empty },
            "missing-readiness" => route with { SourceReadinessHandle = string.Empty },
            "missing-packet" => route with { SourcePacketHandle = string.Empty },
            "missing-steward" => route with { StewardSurface = string.Empty },
            "missing-custody" => route with { CustodyOwner = string.Empty },
            "missing-evidence" => route with { EvidenceHandle = string.Empty },
            "missing-witness" => route with { WitnessHandle = string.Empty },
            "missing-telemetry" => route with { TelemetryRoute = string.Empty },
            "missing-return" => route with { ReturnPathHandle = string.Empty },
            "not-review" => route with { ReviewOnly = false },
            "no-rehearsal-lineage" => route with { PreservesRehearsalLineage = false },
            "no-readiness-lineage" => route with { PreservesReadinessLineage = false },
            "no-packet-lineage" => route with { PreservesPacketLineage = false },
            "no-plan-lineage" => route with { PreservesDryRunPlanLineage = false },
            "no-steward-review" => route with { RoutesToStewardDryRunReview = false },
            "no-cooling" => route with { RequiresCooling = false },
            "authorize" => route with { RouteAuthorizesAction = true },
            "execute" => route with { RouteExecutesAction = true },
            "runtime" => route with { RouteMovesRuntime = true },
            "authority" => route with { RouteGrantsAuthority = true },
            "continuity" => route with { RouteAdmitsContinuity = true },
            "lisp" => route with { RouteEvaluatesLisp = true },
            "packet" => route with { RouteEmitsMembranePacket = true },
            "replay" => route with { RouteReplaysReceipt = true },
            "passage" => route with { RouteIncrementsPassage = true },
            "activation" => route with { RouteActivates = true },
            "unknown-rehearsal" => route with { RehearsalHandle = "urn:san:enactment-dry-run-rehearsal:missing" },
            "wrong-custody" => route with { CustodyOwner = SanctuaryPacketSurfaces.Cryptic },
            _ => route
        };

    private static void AssertCold(EnactmentDryRunRehearsalReceipt receipt)
    {
        Assert.True(receipt.IsColdEnactmentDryRunRehearsal);
        Assert.True(receipt.ReviewOnly);
        Assert.True(receipt.SimulationOnly);
        Assert.True(receipt.NoOpOnly);
        Assert.Equal(receipt.PriorPassageCount, receipt.PassageCountAfterDryRun);
        Assert.Equal(receipt.DryRunCases.Count, receipt.RetainedDryRunCaseCount);
        Assert.False(receipt.DryRunBecamePermission);
        Assert.False(receipt.DryRunAuthorizedAction);
        Assert.False(receipt.DryRunExecutedAction);
        Assert.False(receipt.DryRunMovedRuntime);
        Assert.False(receipt.DryRunWroteOutsideReceiptSurface);
        Assert.False(receipt.DryRunGrantedAuthority);
        Assert.False(receipt.DryRunAdmittedContinuity);
        Assert.False(receipt.StewardDryRunReviewMovedRuntime);
        Assert.False(receipt.ReversibleLocalEffectAuthorizedAction);
        Assert.False(receipt.LispEvaluationAllowed);
        Assert.False(receipt.NewMembranePacketEmitted);
        Assert.False(receipt.ReceiptsReplayed);
        Assert.False(receipt.ContinuityAdmitted);
        Assert.False(receipt.AuthorityGranted);
        Assert.True(receipt.ActivationRefused);
    }

    private static void AssertRefused(EnactmentDryRunRehearsalReceipt receipt, string outcomeCode)
    {
        Assert.Equal(EnactmentDryRunRehearsalDisposition.Refused, receipt.Disposition);
        Assert.Equal(outcomeCode, receipt.OutcomeCode);
        Assert.True(receipt.IsRetainedEnactmentDryRunRehearsalRefusal);
        Assert.NotNull(receipt.Refusal);
        Assert.True(receipt.Refusal.Retained);
        Assert.Empty(receipt.DryRunCases);
        Assert.Empty(receipt.StewardReviewRoutes);
        Assert.Equal(receipt.PriorPassageCount, receipt.PassageCountAfterDryRun);
        Assert.False(receipt.DryRunRehearsed);
        Assert.False(receipt.DryRunAuthorizedAction);
        Assert.False(receipt.DryRunExecutedAction);
        Assert.True(receipt.ActivationRefused);
    }

    private static string FindRepositoryRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null)
        {
            var candidate = Path.Combine(directory.FullName, "src", "SLI", "SLI.Lisp", "enactment-dry-run-rehearsal.lisp");
            if (File.Exists(candidate))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        throw new DirectoryNotFoundException("Could not locate repository root from test output path.");
    }
}
