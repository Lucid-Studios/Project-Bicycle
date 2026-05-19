using San.Common;
using Xunit;

namespace San.Audit.Tests;

public sealed class EnactmentBoundaryReadinessBoundaryValidationTests
{
    private static readonly DateTimeOffset TimestampUtc = DateTimeOffset.Parse("2026-05-16T00:00:00Z");

    [Fact]
    public void Scoped_Work_Packets_Approach_Enactment_Boundary_Review_Without_Execution()
    {
        var receipt = Declare(CreateRequest());

        Assert.Equal(EnactmentBoundaryReadinessDisposition.ReadyForEnactmentBoundaryReviewCold, receipt.Disposition);
        Assert.Equal("enactment-boundary-readiness-ready-review-only", receipt.OutcomeCode);
        Assert.True(receipt.ReadyForEnactmentBoundaryReview);
        Assert.Equal(2, receipt.Candidates.Count);
        Assert.Equal(2, receipt.StewardReviewRoutes.Count);
        Assert.All(receipt.Candidates, candidate => Assert.Contains("scoped-work-packet", candidate.SourcePacketHandle, StringComparison.Ordinal));
        AssertCold(receipt);
    }

    [Fact]
    public void Empty_Readiness_Is_Reviewable_But_Not_Authoritative()
    {
        var source = CreateSourcePacketReceipt(packets: [], routes: []);
        var receipt = Declare(CreateRequest(source: source, candidates: [], routes: []));

        Assert.Equal(EnactmentBoundaryReadinessDisposition.EmptyReviewCold, receipt.Disposition);
        Assert.Equal("enactment-boundary-readiness-empty-review-only", receipt.OutcomeCode);
        Assert.Empty(receipt.Candidates);
        Assert.Empty(receipt.StewardReviewRoutes);
        Assert.False(receipt.ReadyForEnactmentBoundaryReview);
        AssertCold(receipt);
    }

    [Fact]
    public void Readiness_Preserves_Source_Packet_Lineage()
    {
        var source = CreateSourcePacketReceipt();
        var receipt = Declare(CreateRequest(source: source));

        AssertCold(receipt);
        Assert.Equal(source.ReceiptHandle, receipt.SourcePacketFormationReceiptHandle);
        Assert.All(receipt.Candidates, candidate =>
        {
            var packet = source.Packets.Single(item => item.PacketHandle == candidate.SourcePacketHandle);
            var route = source.StewardRoutes.Single(item => item.RouteHandle == candidate.SourceStewardRouteHandle);

            Assert.Equal(packet.PacketHandle, route.PacketHandle);
            Assert.Equal(packet.DutyStation, candidate.DutyStation);
            Assert.Equal(packet.WorkSurface, candidate.WorkSurface);
            Assert.Equal(packet.IntendedWork, candidate.IntendedWork);
            Assert.Equal(packet.MethodCode, candidate.MethodCode);
            Assert.Equal(packet.AuthorityCeiling, candidate.AuthorityCeiling);
            Assert.Equal(packet.CustodyOwner, candidate.CustodyOwner);
            Assert.Equal(packet.WitnessHandle, candidate.WitnessHandle);
            Assert.Equal(packet.TelemetryRoute, candidate.TelemetryRoute);
            Assert.Equal(packet.RevocationPath, candidate.RevocationPath);
            Assert.Equal(packet.RepairPath, candidate.RepairPath);
            Assert.Equal(packet.LossCondition, candidate.LossCondition);
        });
    }

    [Fact]
    public void Readiness_Does_Not_Emit_Packets_Replay_Or_Increment_Passage()
    {
        var receipt = Declare(CreateRequest(priorPassageCount: 888));

        Assert.Equal(888, receipt.PriorPassageCount);
        Assert.Equal(888, receipt.PassageCountAfterReadiness);
        Assert.False(receipt.ReadinessAuthorizedAction);
        Assert.False(receipt.ReadinessExecutedAction);
        Assert.False(receipt.ApproachAuthorizedAction);
        Assert.False(receipt.LocalityAuthorizedAction);
        Assert.False(receipt.ReversibilityAuthorizedAction);
        Assert.False(receipt.StewardReviewMovedRuntime);
        Assert.False(receipt.DryRunPlanExecuted);
        Assert.False(receipt.RuntimeActionAllowed);
        Assert.False(receipt.LispEvaluationAllowed);
        Assert.False(receipt.NewMembranePacketEmitted);
        Assert.False(receipt.ReceiptsReplayed);
        Assert.False(receipt.ContinuityAdmitted);
        Assert.False(receipt.AuthorityGranted);
        Assert.True(receipt.ActivationRefused);
        AssertCold(receipt);
    }

    [Fact]
    public void Readiness_Requires_Cold_Scoped_Work_Packet_Source()
    {
        var receipt = Declare(CreateRequest(omitSource: true));

        AssertRefused(receipt, "enactment-boundary-readiness-source-packet-missing");
    }

    [Theory]
    [InlineData("missing-boundary")]
    [InlineData("not-review")]
    [InlineData("no-readiness")]
    [InlineData("no-source")]
    [InlineData("no-steward-route")]
    [InlineData("no-duty")]
    [InlineData("no-surface")]
    [InlineData("no-work")]
    [InlineData("no-method")]
    [InlineData("no-ceiling")]
    [InlineData("no-local-ceiling")]
    [InlineData("no-reversibility")]
    [InlineData("no-dry-run")]
    [InlineData("no-custody")]
    [InlineData("no-witness")]
    [InlineData("no-telemetry")]
    [InlineData("no-steward-review")]
    [InlineData("no-revocation")]
    [InlineData("no-repair")]
    [InlineData("no-loss")]
    [InlineData("no-action-harness")]
    [InlineData("warrant")]
    [InlineData("admission")]
    [InlineData("authority")]
    [InlineData("continuity")]
    [InlineData("authorize")]
    [InlineData("execute")]
    [InlineData("runtime")]
    [InlineData("lisp")]
    [InlineData("packet")]
    [InlineData("replay")]
    [InlineData("passage")]
    [InlineData("activation")]
    public void Scope_Boundary_Refuses_Readiness_Collapse(string mutation)
    {
        var receipt = Declare(CreateRequest(scope: MutateScope(CreateScope(), mutation)));

        var expected = mutation == "missing-boundary"
            ? "enactment-boundary-readiness-scope-missing"
            : "enactment-boundary-readiness-scope-promotional";
        AssertRefused(receipt, expected);
    }

    [Theory]
    [InlineData("warrant")]
    [InlineData("admission")]
    [InlineData("authorize")]
    [InlineData("execute")]
    [InlineData("runtime")]
    [InlineData("authority")]
    [InlineData("continuity")]
    [InlineData("approach-authorizes")]
    [InlineData("locality-authorizes")]
    [InlineData("reversibility-authorizes")]
    [InlineData("steward-moves-runtime")]
    [InlineData("dry-run-executes")]
    [InlineData("no-action-harness")]
    [InlineData("lisp")]
    [InlineData("packet")]
    [InlineData("replay")]
    [InlineData("passage")]
    [InlineData("activation")]
    public void Non_Execution_Boundary_Refuses_Readiness_As_Authority(string mutation)
    {
        var receipt = Declare(CreateRequest(nonExecution: MutateNonExecution(CreateNonExecution(), mutation)));

        AssertRefused(receipt, "enactment-boundary-readiness-non-execution-invalid");
    }

    [Theory]
    [InlineData("missing-readiness")]
    [InlineData("missing-packet")]
    [InlineData("missing-route")]
    [InlineData("missing-duty")]
    [InlineData("missing-surface")]
    [InlineData("missing-work")]
    [InlineData("missing-method")]
    [InlineData("missing-ceiling")]
    [InlineData("missing-local-ceiling")]
    [InlineData("missing-reversibility")]
    [InlineData("missing-dry-run")]
    [InlineData("missing-custody")]
    [InlineData("missing-witness")]
    [InlineData("missing-telemetry")]
    [InlineData("missing-steward-review")]
    [InlineData("missing-revocation")]
    [InlineData("missing-repair")]
    [InlineData("missing-loss")]
    [InlineData("not-review")]
    [InlineData("not-approach")]
    [InlineData("not-local")]
    [InlineData("not-reversible")]
    [InlineData("no-steward-review")]
    [InlineData("no-dry-run")]
    [InlineData("no-action-harness")]
    [InlineData("warrant")]
    [InlineData("admission")]
    [InlineData("authority")]
    [InlineData("continuity")]
    [InlineData("authorize")]
    [InlineData("execute")]
    [InlineData("runtime")]
    [InlineData("locality-authorizes")]
    [InlineData("reversibility-authorizes")]
    [InlineData("steward-moves-runtime")]
    [InlineData("lisp")]
    [InlineData("packet")]
    [InlineData("replay")]
    [InlineData("passage")]
    [InlineData("activation")]
    public void Readiness_Candidate_Remains_Approach_Only(string mutation)
    {
        var source = CreateSourcePacketReceipt();
        var candidates = CreateCandidates(source);
        candidates[0] = MutateCandidate(candidates[0], mutation);

        var receipt = Declare(CreateRequest(source: source, candidates: candidates));

        AssertRefused(receipt, "enactment-boundary-readiness-candidate-invalid");
    }

    [Theory]
    [InlineData("wrong-packet")]
    [InlineData("wrong-route")]
    [InlineData("route-for-other-packet")]
    [InlineData("wrong-duty")]
    [InlineData("wrong-method")]
    [InlineData("wrong-custody")]
    [InlineData("wrong-witness")]
    [InlineData("wrong-loss")]
    public void Readiness_Must_Bind_To_Source_Packet_And_Steward_Route_Lineage(string mutation)
    {
        var source = CreateSourcePacketReceipt();
        var candidates = CreateCandidates(source);
        candidates[0] = mutation switch
        {
            "wrong-packet" => candidates[0] with { SourcePacketHandle = "urn:san:scoped-work-packet:missing" },
            "wrong-route" => candidates[0] with { SourceStewardRouteHandle = "urn:san:steward-route:scoped-work-packet:missing" },
            "route-for-other-packet" => candidates[0] with { SourceStewardRouteHandle = source.StewardRoutes[1].RouteHandle },
            "wrong-duty" => candidates[0] with { DutyStation = "lab.local.wrong-duty" },
            "wrong-method" => candidates[0] with { MethodCode = "method:wrong" },
            "wrong-custody" => candidates[0] with { CustodyOwner = SanctuaryPacketSurfaces.Prime },
            "wrong-witness" => candidates[0] with { WitnessHandle = "urn:san:witness:wrong" },
            "wrong-loss" => candidates[0] with { LossCondition = "loss:wrong" },
            _ => candidates[0]
        };

        var receipt = Declare(CreateRequest(source: source, candidates: candidates));

        AssertRefused(receipt, "enactment-boundary-readiness-lineage-invalid");
    }

    [Fact]
    public void Readiness_Refuses_Duplicate_Candidate_Handles()
    {
        var source = CreateSourcePacketReceipt();
        var candidates = CreateCandidates(source);
        candidates[1] = candidates[1] with { ReadinessHandle = candidates[0].ReadinessHandle };

        var receipt = Declare(CreateRequest(source: source, candidates: candidates));

        AssertRefused(receipt, "enactment-boundary-readiness-duplicate-candidate-handle");
    }

    [Theory]
    [InlineData("missing-review-route")]
    [InlineData("missing-readiness")]
    [InlineData("missing-packet")]
    [InlineData("missing-steward")]
    [InlineData("missing-custody")]
    [InlineData("missing-evidence")]
    [InlineData("missing-witness")]
    [InlineData("missing-telemetry")]
    [InlineData("missing-return")]
    [InlineData("not-review")]
    [InlineData("no-readiness-lineage")]
    [InlineData("no-packet-lineage")]
    [InlineData("no-route-lineage")]
    [InlineData("no-steward-review")]
    [InlineData("no-cooling")]
    [InlineData("no-action-harness")]
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
    [InlineData("unknown-readiness")]
    [InlineData("unknown-packet")]
    public void Steward_Review_Route_Remains_Review_Only(string mutation)
    {
        var source = CreateSourcePacketReceipt();
        var candidates = CreateCandidates(source);
        var routes = CreateRoutes(candidates);
        routes[0] = MutateRoute(routes[0], mutation);

        var receipt = Declare(CreateRequest(source: source, candidates: candidates, routes: routes));

        AssertRefused(receipt, "enactment-boundary-readiness-steward-route-invalid");
    }

    [Fact]
    public void Steward_Review_Route_Requires_Unique_Handle()
    {
        var source = CreateSourcePacketReceipt();
        var candidates = CreateCandidates(source);
        var routes = CreateRoutes(candidates);
        routes[1] = routes[1] with { ReviewRouteHandle = routes[0].ReviewRouteHandle };

        var receipt = Declare(CreateRequest(source: source, candidates: candidates, routes: routes));

        AssertRefused(receipt, "enactment-boundary-readiness-duplicate-route-handle");
    }

    [Fact]
    public void Non_Empty_Readiness_Requires_Steward_Review_Route()
    {
        var source = CreateSourcePacketReceipt();
        var candidates = CreateCandidates(source);

        var receipt = Declare(CreateRequest(source: source, candidates: candidates, routes: []));

        AssertRefused(receipt, "enactment-boundary-readiness-steward-route-missing");
    }

    [Fact]
    public void Lisp_Body_Carries_Enactment_Boundary_Readiness_As_Inert_Non_Execution_Register()
    {
        var lispPath = Path.Combine(
            FindRepositoryRoot(),
            "src",
            "SLI",
            "SLI.Lisp",
            "enactment-boundary-readiness.lisp");

        var body = File.ReadAllText(lispPath);

        Assert.Contains(":posture :cme-enactment-boundary-readiness-boundary", body, StringComparison.Ordinal);
        Assert.Contains(":lisp-role :inert-enactment-boundary-carrier", body, StringComparison.Ordinal);
        Assert.Contains(":approach-may-authorize nil", body, StringComparison.Ordinal);
        Assert.Contains(":locality-may-authorize nil", body, StringComparison.Ordinal);
        Assert.Contains(":reversibility-may-authorize nil", body, StringComparison.Ordinal);
        Assert.Contains(":steward-review-may-move-runtime nil", body, StringComparison.Ordinal);
        Assert.Contains(":separate-action-harness-required t", body, StringComparison.Ordinal);
        Assert.Contains(":lisp-evaluation-allowed nil", body, StringComparison.Ordinal);
        Assert.Contains(":return :receipt-only", body, StringComparison.Ordinal);
        Assert.DoesNotContain("(eval", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("(compile", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("(load", body, StringComparison.OrdinalIgnoreCase);
    }

    private static EnactmentBoundaryReadinessReceipt Declare(EnactmentBoundaryReadinessRequest request) =>
        new DefaultEnactmentBoundaryReadinessBoundaryValidator().Declare(request, TimestampUtc);

    private static EnactmentBoundaryReadinessRequest CreateRequest(
        ScopedWorkPacketFormationReceipt? source = null,
        IReadOnlyList<EnactmentBoundaryReadinessCandidate>? candidates = null,
        IReadOnlyList<EnactmentBoundaryStewardReviewRoute>? routes = null,
        EnactmentBoundaryReadinessScopeBoundary? scope = null,
        EnactmentBoundaryReadinessNonExecutionBoundary? nonExecution = null,
        int priorPassageCount = 704,
        bool omitSource = false)
    {
        var sourceReceipt = omitSource ? null : source ?? CreateSourcePacketReceipt();
        var defaultSource = sourceReceipt ?? CreateSourcePacketReceipt();
        var candidateSet = candidates ?? CreateCandidates(defaultSource);
        return new(
            SourcePacketFormationReceipt: sourceReceipt,
            Candidates: candidateSet,
            StewardReviewRoutes: routes ?? CreateRoutes(candidateSet),
            ScopeBoundary: scope ?? CreateScope(),
            NonExecutionBoundary: nonExecution ?? CreateNonExecution(),
            PriorPassageCount: priorPassageCount);
    }

    private static ScopedWorkPacketFormationReceipt CreateSourcePacketReceipt(
        IReadOnlyList<ScopedWorkPacketDeclaration>? packets = null,
        IReadOnlyList<ScopedWorkPacketStewardRoute>? routes = null)
    {
        var selectionSource = CreateSourceSelectionReceipt();
        var packetSet = packets ?? CreatePackets(selectionSource);
        return new DefaultScopedWorkPacketFormationBoundaryValidator().Declare(
            new ScopedWorkPacketFormationRequest(
                SourceSelectionReceipt: selectionSource,
                Packets: packetSet,
                StewardRoutes: routes ?? CreatePacketRoutes(packetSet),
                ScopeBoundary: CreatePacketScope(),
                NonExecutionBoundary: CreatePacketNonExecution(),
                PriorPassageCount: 640),
            TimestampUtc);
    }

    private static AspirationCandidateSelectionClosureReceipt CreateSourceSelectionReceipt(
        IReadOnlyList<AspirationCandidateSelection>? selections = null,
        IReadOnlyList<AspirationClosureLaw>? closureLaws = null) =>
        new DefaultAspirationCandidateSelectionClosureBoundaryValidator().Select(
            new AspirationCandidateSelectionClosureRequest(
                Selections: selections ?? CreateSourceSelections(),
                ClosureLaws: closureLaws ?? CreateClosureLaws(),
                Boundary: CreateSelectionBoundary(),
                NonPromotionBoundary: CreateSelectionNonPromotion(),
                PriorPassageCount: 512),
            TimestampUtc);

    private static AspirationCandidateSelection[] CreateSourceSelections() =>
    [
        Selection("selected-prime", AspirationCandidateSelectionState.SelectedWorkingSet),
        Selection("selected-steward", AspirationCandidateSelectionState.SelectedWorkingSet),
        Selection("compost", AspirationCandidateSelectionState.HeldAsCompost)
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
        ClosureLaw("packet-not-enactment", "work packet may name a duty station; work packet may not enact it"),
        ClosureLaw("readiness-not-motion", "enactment readiness may approach review; readiness may not move runtime")
    ];

    private static AspirationClosureLaw ClosureLaw(string suffix, string text) =>
        new(
            LawHandle: $"urn:san:aspiration-closure-law:{suffix}",
            LawText: text,
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

    private static AspirationCandidateSelectionClosureBoundary CreateSelectionBoundary() =>
        new(
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
            AllowsActivation: false);

    private static AspirationCandidateSelectionNonPromotionBoundary CreateSelectionNonPromotion() =>
        new(
            BoundaryLaw: "Selection may shape scoped work packet review. Selection and closure law may not authorize, execute, admit continuity, evaluate Lisp, emit packets, replay, increment passage, or activate.",
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

    private static ScopedWorkPacketDeclaration[] CreatePackets(AspirationCandidateSelectionClosureReceipt source) =>
        source.Selections
            .Where(static selection => selection.SelectionState == AspirationCandidateSelectionState.SelectedWorkingSet)
            .Select(Packet)
            .ToArray();

    private static ScopedWorkPacketDeclaration Packet(AspirationCandidateSelection selection)
    {
        var suffix = selection.SelectionHandle.Split(':').Last();
        return new(
            PacketHandle: $"urn:san:scoped-work-packet:{suffix}",
            SourceSelectionHandle: selection.SelectionHandle,
            SourceMaturationCandidateHandle: selection.SourceMaturationCandidateHandle,
            SourcePayloadStatementHandle: selection.SourcePayloadStatementHandle,
            DutyStation: "lab.local.tiny-bicycle-review",
            WorkSurface: "surface:local-reversible-review-receipt",
            IntendedWork: "prepare local reversible review work without enactment",
            MethodCode: "method:scoped-work-packet-review-only",
            AuthorityCeiling: "ceiling:steward-enactment-boundary-required",
            CustodyOwner: SanctuaryPacketSurfaces.Steward,
            WitnessHandle: $"urn:san:witness:scoped-work-packet:{suffix}",
            TelemetryRoute: SanctuaryPacketRoutes.TelemetryString,
            StewardRoute: $"urn:san:steward-route:scoped-work-packet:{suffix}",
            RevocationPath: $"urn:san:revocation:scoped-work-packet:{suffix}",
            RepairPath: $"urn:san:repair:scoped-work-packet:{suffix}",
            LossCondition: "loss:work-packet-treated-as-enactment",
            ReviewOnly: true,
            CandidateOnly: true,
            LocalOnly: true,
            ReversibleOnly: true,
            RequiresStewardReview: true,
            RequiresSeparateEnactmentBoundary: true,
            PacketBecomesWarrant: false,
            PacketBecomesAdmission: false,
            PacketGrantsAuthority: false,
            PacketAdmitsContinuity: false,
            PacketAuthorizesAction: false,
            PacketExecutesAction: false,
            PacketEvaluatesLisp: false,
            PacketEmitsMembranePacket: false,
            PacketReplaysReceipt: false,
            PacketIncrementsPassage: false,
            PacketActivates: false);
    }

    private static ScopedWorkPacketStewardRoute[] CreatePacketRoutes(IReadOnlyList<ScopedWorkPacketDeclaration> packets) =>
        packets.Select(packet =>
            new ScopedWorkPacketStewardRoute(
                RouteHandle: packet.StewardRoute,
                PacketHandle: packet.PacketHandle,
                StewardSurface: SanctuaryPacketSurfaces.Steward,
                CustodyOwner: packet.CustodyOwner,
                EvidenceHandle: $"urn:san:evidence:scoped-work-packet:{packet.PacketHandle.Split(':').Last()}",
                WitnessHandle: packet.WitnessHandle,
                TelemetryRoute: packet.TelemetryRoute,
                ReturnPathHandle: packet.RepairPath,
                ReviewOnly: true,
                PreservesPacketLineage: true,
                PreservesSelectionLineage: true,
                PreservesCompostLineage: true,
                RoutesToStewardReview: true,
                RequiresCooling: true,
                RouteAuthorizesAction: false,
                RouteExecutesAction: false,
                RouteGrantsAuthority: false,
                RouteAdmitsContinuity: false,
                RouteEvaluatesLisp: false,
                RouteEmitsMembranePacket: false,
                RouteActivates: false))
            .ToArray();

    private static ScopedWorkPacketScopeBoundary CreatePacketScope() =>
        new(
            BoundaryCode: "scoped-work-packet-formation-review-only",
            Present: true,
            ReviewOnly: true,
            AllowsWorkPacketFormation: true,
            RequiresDutyStation: true,
            RequiresWorkSurface: true,
            RequiresIntendedWork: true,
            RequiresMethodCode: true,
            RequiresAuthorityCeiling: true,
            RequiresCustody: true,
            RequiresWitness: true,
            RequiresTelemetryRoute: true,
            RequiresStewardRoute: true,
            RequiresRevocationPath: true,
            RequiresRepairPath: true,
            RequiresLossCondition: true,
            RequiresSeparateEnactmentBoundary: true,
            RequiresLocalEffectBoundary: true,
            RequiresReversibility: true,
            AllowsPacketAsWarrant: false,
            AllowsPacketAsAdmission: false,
            AllowsPacketAsAuthority: false,
            AllowsPacketAsContinuity: false,
            AllowsExecution: false,
            AllowsRuntimeMotion: false,
            AllowsLispEvaluation: false,
            AllowsMembranePacketEmission: false,
            AllowsReceiptReplay: false,
            AllowsPassageIncrement: false,
            AllowsActivation: false);

    private static ScopedWorkPacketNonExecutionBoundary CreatePacketNonExecution() =>
        new(
            BoundaryLaw: "A selected working set may form a scoped work packet for Steward review. The packet may not authorize, execute, grant authority, admit continuity, evaluate Lisp, emit membrane packets, replay receipts, increment passage, or activate.",
            WorkPacketMayBecomeWarrant: false,
            WorkPacketMayBecomeAdmission: false,
            WorkPacketMayAuthorize: false,
            WorkPacketMayExecute: false,
            WorkPacketMayGrantAuthority: false,
            WorkPacketMayAdmitContinuity: false,
            StewardRoutingMayExecute: false,
            ReversibilityMayAuthorize: false,
            LocalityMayAuthorize: false,
            WorkPacketMayEvaluateLisp: false,
            WorkPacketMayEmitMembranePacket: false,
            WorkPacketMayReplayReceipt: false,
            WorkPacketMayIncrementPassage: false,
            WorkPacketMayActivate: false);

    private static EnactmentBoundaryReadinessCandidate[] CreateCandidates(ScopedWorkPacketFormationReceipt source) =>
        source.Packets.Select(packet =>
        {
            var route = source.StewardRoutes.Single(item => item.PacketHandle == packet.PacketHandle);
            var suffix = packet.PacketHandle.Split(':').Last();
            return new EnactmentBoundaryReadinessCandidate(
                ReadinessHandle: $"urn:san:enactment-boundary-readiness:{suffix}",
                SourcePacketHandle: packet.PacketHandle,
                SourceStewardRouteHandle: route.RouteHandle,
                DutyStation: packet.DutyStation,
                WorkSurface: packet.WorkSurface,
                IntendedWork: packet.IntendedWork,
                MethodCode: packet.MethodCode,
                AuthorityCeiling: packet.AuthorityCeiling,
                LocalEffectCeiling: "local-effect:receipt-only-under-tiny-bicycle-lab",
                ReversibilityProofHandle: $"urn:san:reversibility-proof:enactment-boundary:{suffix}",
                DryRunPlanHandle: $"urn:san:dry-run-plan:enactment-boundary:{suffix}",
                CustodyOwner: packet.CustodyOwner,
                WitnessHandle: packet.WitnessHandle,
                TelemetryRoute: packet.TelemetryRoute,
                StewardReviewHandle: $"urn:san:steward-review:enactment-boundary:{suffix}",
                RevocationPath: packet.RevocationPath,
                RepairPath: packet.RepairPath,
                LossCondition: packet.LossCondition,
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
        }).ToArray();

    private static EnactmentBoundaryStewardReviewRoute[] CreateRoutes(IReadOnlyList<EnactmentBoundaryReadinessCandidate> candidates) =>
        candidates.Select(candidate =>
            new EnactmentBoundaryStewardReviewRoute(
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
                RouteActivates: false))
            .ToArray();

    private static EnactmentBoundaryReadinessScopeBoundary CreateScope() =>
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

    private static EnactmentBoundaryReadinessNonExecutionBoundary CreateNonExecution() =>
        new(
            BoundaryLaw: "A scoped work packet may approach enactment boundary review. Approach is not enactment; locality is not permission; reversibility is not permission; Steward review is not runtime motion.",
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

    private static EnactmentBoundaryReadinessScopeBoundary MutateScope(
        EnactmentBoundaryReadinessScopeBoundary scope,
        string mutation) =>
        mutation switch
        {
            "missing-boundary" => scope with { BoundaryCode = string.Empty, Present = false },
            "not-review" => scope with { ReviewOnly = false },
            "no-readiness" => scope with { AllowsReadinessDeclaration = false },
            "no-source" => scope with { RequiresScopedWorkPacketReceipt = false },
            "no-steward-route" => scope with { RequiresStewardRoute = false },
            "no-duty" => scope with { RequiresDutyStation = false },
            "no-surface" => scope with { RequiresWorkSurface = false },
            "no-work" => scope with { RequiresIntendedWork = false },
            "no-method" => scope with { RequiresMethodCode = false },
            "no-ceiling" => scope with { RequiresAuthorityCeiling = false },
            "no-local-ceiling" => scope with { RequiresLocalEffectCeiling = false },
            "no-reversibility" => scope with { RequiresReversibilityProof = false },
            "no-dry-run" => scope with { RequiresDryRunPlan = false },
            "no-custody" => scope with { RequiresCustody = false },
            "no-witness" => scope with { RequiresWitness = false },
            "no-telemetry" => scope with { RequiresTelemetryRoute = false },
            "no-steward-review" => scope with { RequiresStewardReview = false },
            "no-revocation" => scope with { RequiresRevocationPath = false },
            "no-repair" => scope with { RequiresRepairPath = false },
            "no-loss" => scope with { RequiresLossCondition = false },
            "no-action-harness" => scope with { RequiresSeparateActionHarness = false },
            "warrant" => scope with { AllowsReadinessAsWarrant = true },
            "admission" => scope with { AllowsReadinessAsAdmission = true },
            "authority" => scope with { AllowsReadinessAsAuthority = true },
            "continuity" => scope with { AllowsReadinessAsContinuity = true },
            "authorize" => scope with { AllowsActionAuthorization = true },
            "execute" => scope with { AllowsExecution = true },
            "runtime" => scope with { AllowsRuntimeMotion = true },
            "lisp" => scope with { AllowsLispEvaluation = true },
            "packet" => scope with { AllowsMembranePacketEmission = true },
            "replay" => scope with { AllowsReceiptReplay = true },
            "passage" => scope with { AllowsPassageIncrement = true },
            "activation" => scope with { AllowsActivation = true },
            _ => scope
        };

    private static EnactmentBoundaryReadinessNonExecutionBoundary MutateNonExecution(
        EnactmentBoundaryReadinessNonExecutionBoundary boundary,
        string mutation) =>
        mutation switch
        {
            "warrant" => boundary with { ReadinessMayBecomeWarrant = true },
            "admission" => boundary with { ReadinessMayBecomeAdmission = true },
            "authorize" => boundary with { ReadinessMayAuthorize = true },
            "execute" => boundary with { ReadinessMayExecute = true },
            "runtime" => boundary with { ReadinessMayMoveRuntime = true },
            "authority" => boundary with { ReadinessMayGrantAuthority = true },
            "continuity" => boundary with { ReadinessMayAdmitContinuity = true },
            "approach-authorizes" => boundary with { ApproachMayAuthorize = true },
            "locality-authorizes" => boundary with { LocalityMayAuthorize = true },
            "reversibility-authorizes" => boundary with { ReversibilityMayAuthorize = true },
            "steward-moves-runtime" => boundary with { StewardReviewMayMoveRuntime = true },
            "dry-run-executes" => boundary with { DryRunPlanMayExecute = true },
            "no-action-harness" => boundary with { SeparateActionHarnessRequired = false },
            "lisp" => boundary with { ReadinessMayEvaluateLisp = true },
            "packet" => boundary with { ReadinessMayEmitMembranePacket = true },
            "replay" => boundary with { ReadinessMayReplayReceipt = true },
            "passage" => boundary with { ReadinessMayIncrementPassage = true },
            "activation" => boundary with { ReadinessMayActivate = true },
            _ => boundary
        };

    private static EnactmentBoundaryReadinessCandidate MutateCandidate(
        EnactmentBoundaryReadinessCandidate candidate,
        string mutation) =>
        mutation switch
        {
            "missing-readiness" => candidate with { ReadinessHandle = string.Empty },
            "missing-packet" => candidate with { SourcePacketHandle = string.Empty },
            "missing-route" => candidate with { SourceStewardRouteHandle = string.Empty },
            "missing-duty" => candidate with { DutyStation = string.Empty },
            "missing-surface" => candidate with { WorkSurface = string.Empty },
            "missing-work" => candidate with { IntendedWork = string.Empty },
            "missing-method" => candidate with { MethodCode = string.Empty },
            "missing-ceiling" => candidate with { AuthorityCeiling = string.Empty },
            "missing-local-ceiling" => candidate with { LocalEffectCeiling = string.Empty },
            "missing-reversibility" => candidate with { ReversibilityProofHandle = string.Empty },
            "missing-dry-run" => candidate with { DryRunPlanHandle = string.Empty },
            "missing-custody" => candidate with { CustodyOwner = string.Empty },
            "missing-witness" => candidate with { WitnessHandle = string.Empty },
            "missing-telemetry" => candidate with { TelemetryRoute = string.Empty },
            "missing-steward-review" => candidate with { StewardReviewHandle = string.Empty },
            "missing-revocation" => candidate with { RevocationPath = string.Empty },
            "missing-repair" => candidate with { RepairPath = string.Empty },
            "missing-loss" => candidate with { LossCondition = string.Empty },
            "not-review" => candidate with { ReviewOnly = false },
            "not-approach" => candidate with { ApproachOnly = false },
            "not-local" => candidate with { LocalOnly = false },
            "not-reversible" => candidate with { ReversibleOnly = false },
            "no-steward-review" => candidate with { RequiresStewardReview = false },
            "no-dry-run" => candidate with { RequiresDryRunBeforeExecution = false },
            "no-action-harness" => candidate with { RequiresSeparateActionHarness = false },
            "warrant" => candidate with { ReadinessBecomesWarrant = true },
            "admission" => candidate with { ReadinessBecomesAdmission = true },
            "authority" => candidate with { ReadinessGrantsAuthority = true },
            "continuity" => candidate with { ReadinessAdmitsContinuity = true },
            "authorize" => candidate with { ReadinessAuthorizesAction = true },
            "execute" => candidate with { ReadinessExecutesAction = true },
            "runtime" => candidate with { ApproachMovesRuntime = true },
            "locality-authorizes" => candidate with { LocalityAuthorizesAction = true },
            "reversibility-authorizes" => candidate with { ReversibilityAuthorizesAction = true },
            "steward-moves-runtime" => candidate with { StewardReviewMovesRuntime = true },
            "lisp" => candidate with { ReadinessEvaluatesLisp = true },
            "packet" => candidate with { ReadinessEmitsMembranePacket = true },
            "replay" => candidate with { ReadinessReplaysReceipt = true },
            "passage" => candidate with { ReadinessIncrementsPassage = true },
            "activation" => candidate with { ReadinessActivates = true },
            _ => candidate
        };

    private static EnactmentBoundaryStewardReviewRoute MutateRoute(
        EnactmentBoundaryStewardReviewRoute route,
        string mutation) =>
        mutation switch
        {
            "missing-review-route" => route with { ReviewRouteHandle = string.Empty },
            "missing-readiness" => route with { ReadinessHandle = string.Empty },
            "missing-packet" => route with { SourcePacketHandle = string.Empty },
            "missing-steward" => route with { StewardSurface = string.Empty },
            "missing-custody" => route with { CustodyOwner = string.Empty },
            "missing-evidence" => route with { EvidenceHandle = string.Empty },
            "missing-witness" => route with { WitnessHandle = string.Empty },
            "missing-telemetry" => route with { TelemetryRoute = string.Empty },
            "missing-return" => route with { ReturnPathHandle = string.Empty },
            "not-review" => route with { ReviewOnly = false },
            "no-readiness-lineage" => route with { PreservesReadinessLineage = false },
            "no-packet-lineage" => route with { PreservesPacketLineage = false },
            "no-route-lineage" => route with { PreservesStewardRouteLineage = false },
            "no-steward-review" => route with { RoutesToStewardEnactmentReview = false },
            "no-cooling" => route with { RequiresCooling = false },
            "no-action-harness" => route with { RequiresSeparateActionHarness = false },
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
            "unknown-readiness" => route with { ReadinessHandle = "urn:san:enactment-boundary-readiness:missing" },
            "unknown-packet" => route with { SourcePacketHandle = "urn:san:scoped-work-packet:missing" },
            _ => route
        };

    private static void AssertCold(EnactmentBoundaryReadinessReceipt receipt)
    {
        Assert.True(receipt.IsColdEnactmentBoundaryReadiness);
        Assert.True(receipt.ReviewOnly);
        Assert.True(receipt.ApproachOnly);
        Assert.Equal(receipt.PriorPassageCount, receipt.PassageCountAfterReadiness);
        Assert.Equal(receipt.Candidates.Count, receipt.RetainedCandidateCount);
        Assert.False(receipt.ReadinessBecameWarrant);
        Assert.False(receipt.ReadinessBecameAdmission);
        Assert.False(receipt.ReadinessGrantedAuthority);
        Assert.False(receipt.ReadinessAdmittedContinuity);
        Assert.False(receipt.ReadinessAuthorizedAction);
        Assert.False(receipt.ReadinessExecutedAction);
        Assert.False(receipt.RuntimeActionAllowed);
        Assert.False(receipt.LispEvaluationAllowed);
        Assert.False(receipt.NewMembranePacketEmitted);
        Assert.False(receipt.ReceiptsReplayed);
        Assert.False(receipt.ContinuityAdmitted);
        Assert.False(receipt.AuthorityGranted);
        Assert.True(receipt.ActivationRefused);
    }

    private static void AssertRefused(
        EnactmentBoundaryReadinessReceipt receipt,
        string outcomeCode)
    {
        Assert.Equal(EnactmentBoundaryReadinessDisposition.Refused, receipt.Disposition);
        Assert.Equal(outcomeCode, receipt.OutcomeCode);
        Assert.True(receipt.IsRetainedEnactmentBoundaryReadinessRefusal);
        Assert.NotNull(receipt.Refusal);
        Assert.True(receipt.Refusal.Retained);
        Assert.Empty(receipt.Candidates);
        Assert.Empty(receipt.StewardReviewRoutes);
        Assert.Equal(receipt.PriorPassageCount, receipt.PassageCountAfterReadiness);
        Assert.False(receipt.ReadyForEnactmentBoundaryReview);
        Assert.False(receipt.ReadinessAuthorizedAction);
        Assert.False(receipt.ReadinessExecutedAction);
        Assert.True(receipt.ActivationRefused);
    }

    private static string FindRepositoryRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null)
        {
            var candidate = Path.Combine(directory.FullName, "src", "SLI", "SLI.Lisp", "enactment-boundary-readiness.lisp");
            if (File.Exists(candidate))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        throw new DirectoryNotFoundException("Could not locate repository root from test output path.");
    }
}
