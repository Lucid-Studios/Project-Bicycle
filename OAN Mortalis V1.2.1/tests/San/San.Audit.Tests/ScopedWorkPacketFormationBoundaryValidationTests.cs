using San.Common;
using Xunit;

namespace San.Audit.Tests;

public sealed class ScopedWorkPacketFormationBoundaryValidationTests
{
    private static readonly DateTimeOffset TimestampUtc = DateTimeOffset.Parse("2026-05-16T00:00:00Z");

    [Fact]
    public void Scoped_Work_Packets_Form_From_Selected_Working_Set_Without_Execution()
    {
        var receipt = Declare(CreateRequest());

        Assert.Equal(ScopedWorkPacketFormationDisposition.FormedForReviewCold, receipt.Disposition);
        Assert.Equal("scoped-work-packet-formed-review-only", receipt.OutcomeCode);
        Assert.True(receipt.WorkPacketFormedForReview);
        Assert.Equal(2, receipt.Packets.Count);
        Assert.Equal(2, receipt.StewardRoutes.Count);
        Assert.All(receipt.Packets, packet => Assert.Contains("selected", packet.SourceSelectionHandle, StringComparison.Ordinal));
        AssertCold(receipt);
    }

    [Fact]
    public void Empty_Work_Packet_Formation_Is_Reviewable_But_Not_Authoritative()
    {
        var source = CreateSourceSelectionReceipt(selections: [], closureLaws: []);
        var receipt = Declare(CreateRequest(source: source, packets: [], routes: []));

        Assert.Equal(ScopedWorkPacketFormationDisposition.EmptyReviewCold, receipt.Disposition);
        Assert.Equal("scoped-work-packet-empty-review-only", receipt.OutcomeCode);
        Assert.Empty(receipt.Packets);
        Assert.Empty(receipt.StewardRoutes);
        Assert.False(receipt.WorkPacketFormedForReview);
        AssertCold(receipt);
    }

    [Fact]
    public void Work_Packet_Formation_Preserves_Source_Selection_Lineage()
    {
        var source = CreateSourceSelectionReceipt();
        var receipt = Declare(CreateRequest(source: source));

        AssertCold(receipt);
        Assert.Equal(source.ReceiptHandle, receipt.SourceSelectionReceiptHandle);
        Assert.All(receipt.Packets, packet =>
        {
            var selection = source.Selections.Single(item => item.SelectionHandle == packet.SourceSelectionHandle);
            Assert.Equal(AspirationCandidateSelectionState.SelectedWorkingSet, selection.SelectionState);
            Assert.Equal(selection.SourceMaturationCandidateHandle, packet.SourceMaturationCandidateHandle);
            Assert.Equal(selection.SourcePayloadStatementHandle, packet.SourcePayloadStatementHandle);
        });
    }

    [Fact]
    public void Work_Packet_Formation_Does_Not_Emit_Membrane_Packet_Replay_Or_Increment_Passage()
    {
        var receipt = Declare(CreateRequest(priorPassageCount: 777));

        Assert.Equal(777, receipt.PriorPassageCount);
        Assert.Equal(777, receipt.PassageCountAfterPacketFormation);
        Assert.False(receipt.PacketAuthorizedAction);
        Assert.False(receipt.PacketExecutedAction);
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
    public void Work_Packet_Formation_Requires_Cold_Source_Selection_Receipt()
    {
        var receipt = Declare(CreateRequest(omitSource: true));

        AssertRefused(receipt, "scoped-work-packet-source-selection-missing");
    }

    [Theory]
    [InlineData("missing-boundary")]
    [InlineData("not-review")]
    [InlineData("no-formation")]
    [InlineData("no-duty")]
    [InlineData("no-surface")]
    [InlineData("no-work")]
    [InlineData("no-method")]
    [InlineData("no-ceiling")]
    [InlineData("no-custody")]
    [InlineData("no-witness")]
    [InlineData("no-telemetry")]
    [InlineData("no-steward")]
    [InlineData("no-revocation")]
    [InlineData("no-repair")]
    [InlineData("no-loss")]
    [InlineData("no-enactment-boundary")]
    [InlineData("no-local")]
    [InlineData("no-reversible")]
    [InlineData("warrant")]
    [InlineData("admission")]
    [InlineData("authority")]
    [InlineData("continuity")]
    [InlineData("execution")]
    [InlineData("runtime")]
    [InlineData("lisp")]
    [InlineData("packet")]
    [InlineData("replay")]
    [InlineData("passage")]
    [InlineData("activation")]
    public void Scope_Boundary_Refuses_Work_Packet_Collapse(string mutation)
    {
        var receipt = Declare(CreateRequest(scope: MutateScope(CreateScope(), mutation)));

        var expected = mutation == "missing-boundary"
            ? "scoped-work-packet-boundary-missing"
            : "scoped-work-packet-boundary-promotional";
        AssertRefused(receipt, expected);
    }

    [Theory]
    [InlineData("warrant")]
    [InlineData("admission")]
    [InlineData("authorize")]
    [InlineData("execute")]
    [InlineData("authority")]
    [InlineData("continuity")]
    [InlineData("route-executes")]
    [InlineData("reversibility-authorizes")]
    [InlineData("locality-authorizes")]
    [InlineData("lisp")]
    [InlineData("packet")]
    [InlineData("replay")]
    [InlineData("passage")]
    [InlineData("activation")]
    public void Non_Execution_Boundary_Refuses_Work_Packet_As_Authority(string mutation)
    {
        var receipt = Declare(CreateRequest(nonExecution: MutateNonExecution(CreateNonExecution(), mutation)));

        AssertRefused(receipt, "scoped-work-packet-non-execution-boundary-invalid");
    }

    [Theory]
    [InlineData("missing-packet")]
    [InlineData("missing-selection")]
    [InlineData("missing-candidate")]
    [InlineData("missing-statement")]
    [InlineData("missing-duty")]
    [InlineData("missing-surface")]
    [InlineData("missing-work")]
    [InlineData("missing-method")]
    [InlineData("missing-ceiling")]
    [InlineData("missing-custody")]
    [InlineData("missing-witness")]
    [InlineData("missing-telemetry")]
    [InlineData("missing-steward")]
    [InlineData("missing-revocation")]
    [InlineData("missing-repair")]
    [InlineData("missing-loss")]
    [InlineData("not-review")]
    [InlineData("not-candidate")]
    [InlineData("not-local")]
    [InlineData("not-reversible")]
    [InlineData("no-steward-review")]
    [InlineData("no-enactment-boundary")]
    [InlineData("warrant")]
    [InlineData("admission")]
    [InlineData("authority")]
    [InlineData("continuity")]
    [InlineData("authorize")]
    [InlineData("execute")]
    [InlineData("lisp")]
    [InlineData("packet")]
    [InlineData("replay")]
    [InlineData("passage")]
    [InlineData("activation")]
    public void Work_Packet_Declaration_Remains_Candidate_Only(string mutation)
    {
        var source = CreateSourceSelectionReceipt();
        var packets = CreatePackets(source);
        packets[0] = MutatePacket(packets[0], mutation);

        var receipt = Declare(CreateRequest(source: source, packets: packets));

        AssertRefused(receipt, "scoped-work-packet-invalid");
    }

    [Theory]
    [InlineData("wrong-selection")]
    [InlineData("wrong-candidate")]
    [InlineData("wrong-statement")]
    [InlineData("compost-selection")]
    public void Work_Packet_Must_Bind_To_Selected_Working_Set_Lineage(string mutation)
    {
        var source = CreateSourceSelectionReceipt();
        var packets = CreatePackets(source);
        packets[0] = mutation switch
        {
            "wrong-selection" => packets[0] with { SourceSelectionHandle = "urn:san:aspiration-selection:missing" },
            "wrong-candidate" => packets[0] with { SourceMaturationCandidateHandle = "urn:san:aspiration-payload:candidate:wrong" },
            "wrong-statement" => packets[0] with { SourcePayloadStatementHandle = "urn:san:aspiration-payload:statement:wrong" },
            "compost-selection" => packets[0] with { SourceSelectionHandle = "urn:san:aspiration-selection:compost" },
            _ => packets[0]
        };

        var receipt = Declare(CreateRequest(source: source, packets: packets));

        AssertRefused(receipt, "scoped-work-packet-selection-lineage-invalid");
    }

    [Fact]
    public void Work_Packet_Refuses_Duplicate_Packet_Handles()
    {
        var source = CreateSourceSelectionReceipt();
        var packets = CreatePackets(source);
        packets[1] = packets[1] with { PacketHandle = packets[0].PacketHandle };

        var receipt = Declare(CreateRequest(source: source, packets: packets));

        AssertRefused(receipt, "scoped-work-packet-duplicate-packet-handle");
    }

    [Theory]
    [InlineData("missing-route")]
    [InlineData("missing-packet")]
    [InlineData("missing-steward")]
    [InlineData("missing-custody")]
    [InlineData("missing-evidence")]
    [InlineData("missing-witness")]
    [InlineData("missing-telemetry")]
    [InlineData("missing-return")]
    [InlineData("not-review")]
    [InlineData("no-packet-lineage")]
    [InlineData("no-selection-lineage")]
    [InlineData("no-compost-lineage")]
    [InlineData("no-steward-review")]
    [InlineData("no-cooling")]
    [InlineData("authorize")]
    [InlineData("execute")]
    [InlineData("authority")]
    [InlineData("continuity")]
    [InlineData("lisp")]
    [InlineData("packet")]
    [InlineData("activation")]
    [InlineData("unknown-packet")]
    public void Steward_Route_Remains_Review_Only(string mutation)
    {
        var source = CreateSourceSelectionReceipt();
        var packets = CreatePackets(source);
        var routes = CreateRoutes(packets);
        routes[0] = MutateRoute(routes[0], mutation);

        var receipt = Declare(CreateRequest(source: source, packets: packets, routes: routes));

        AssertRefused(receipt, "scoped-work-packet-steward-route-invalid");
    }

    [Fact]
    public void Steward_Route_Requires_Unique_Handle()
    {
        var source = CreateSourceSelectionReceipt();
        var packets = CreatePackets(source);
        var routes = CreateRoutes(packets);
        routes[1] = routes[1] with { RouteHandle = routes[0].RouteHandle };

        var receipt = Declare(CreateRequest(source: source, packets: packets, routes: routes));

        AssertRefused(receipt, "scoped-work-packet-duplicate-route-handle");
    }

    [Fact]
    public void Non_Empty_Work_Packet_Requires_Steward_Route()
    {
        var source = CreateSourceSelectionReceipt();
        var packets = CreatePackets(source);

        var receipt = Declare(CreateRequest(source: source, packets: packets, routes: []));

        AssertRefused(receipt, "scoped-work-packet-steward-route-missing");
    }

    [Fact]
    public void Lisp_Body_Carries_Scoped_Work_Packet_As_Inert_Non_Execution_Register()
    {
        var lispPath = Path.Combine(
            FindRepositoryRoot(),
            "src",
            "SLI",
            "SLI.Lisp",
            "scoped-work-packet-formation.lisp");

        var body = File.ReadAllText(lispPath);

        Assert.Contains(":posture :cme-scoped-work-packet-formation-boundary", body, StringComparison.Ordinal);
        Assert.Contains(":lisp-role :inert-scoped-work-packet-carrier", body, StringComparison.Ordinal);
        Assert.Contains(":work-packet-may-authorize nil", body, StringComparison.Ordinal);
        Assert.Contains(":work-packet-may-execute nil", body, StringComparison.Ordinal);
        Assert.Contains(":separate-enactment-boundary-required t", body, StringComparison.Ordinal);
        Assert.Contains(":reversibility-may-authorize nil", body, StringComparison.Ordinal);
        Assert.Contains(":lisp-evaluation-allowed nil", body, StringComparison.Ordinal);
        Assert.Contains(":return :receipt-only", body, StringComparison.Ordinal);
        Assert.DoesNotContain("(eval", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("(compile", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("(load", body, StringComparison.OrdinalIgnoreCase);
    }

    private static ScopedWorkPacketFormationReceipt Declare(ScopedWorkPacketFormationRequest request) =>
        new DefaultScopedWorkPacketFormationBoundaryValidator().Declare(request, TimestampUtc);

    private static ScopedWorkPacketFormationRequest CreateRequest(
        AspirationCandidateSelectionClosureReceipt? source = null,
        IReadOnlyList<ScopedWorkPacketDeclaration>? packets = null,
        IReadOnlyList<ScopedWorkPacketStewardRoute>? routes = null,
        ScopedWorkPacketScopeBoundary? scope = null,
        ScopedWorkPacketNonExecutionBoundary? nonExecution = null,
        int priorPassageCount = 640,
        bool omitSource = false)
    {
        var sourceReceipt = omitSource ? null : source ?? CreateSourceSelectionReceipt();
        var defaultSource = sourceReceipt ?? CreateSourceSelectionReceipt();
        var packetSet = packets ?? CreatePackets(defaultSource);
        return new(
            SourceSelectionReceipt: sourceReceipt,
            Packets: packetSet,
            StewardRoutes: routes ?? CreateRoutes(packetSet),
            ScopeBoundary: scope ?? CreateScope(),
            NonExecutionBoundary: nonExecution ?? CreateNonExecution(),
            PriorPassageCount: priorPassageCount);
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
        ClosureLaw("selection-not-packet-authority", "selection may seed a scoped packet; selection may not authorize work"),
        ClosureLaw("packet-not-enactment", "work packet may name a duty station; work packet may not enact it")
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
            .Select(selection => Packet(selection))
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

    private static ScopedWorkPacketStewardRoute[] CreateRoutes(IReadOnlyList<ScopedWorkPacketDeclaration> packets) =>
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

    private static ScopedWorkPacketScopeBoundary CreateScope() =>
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

    private static ScopedWorkPacketNonExecutionBoundary CreateNonExecution() =>
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

    private static ScopedWorkPacketScopeBoundary MutateScope(
        ScopedWorkPacketScopeBoundary scope,
        string mutation) =>
        mutation switch
        {
            "missing-boundary" => scope with { BoundaryCode = string.Empty, Present = false },
            "not-review" => scope with { ReviewOnly = false },
            "no-formation" => scope with { AllowsWorkPacketFormation = false },
            "no-duty" => scope with { RequiresDutyStation = false },
            "no-surface" => scope with { RequiresWorkSurface = false },
            "no-work" => scope with { RequiresIntendedWork = false },
            "no-method" => scope with { RequiresMethodCode = false },
            "no-ceiling" => scope with { RequiresAuthorityCeiling = false },
            "no-custody" => scope with { RequiresCustody = false },
            "no-witness" => scope with { RequiresWitness = false },
            "no-telemetry" => scope with { RequiresTelemetryRoute = false },
            "no-steward" => scope with { RequiresStewardRoute = false },
            "no-revocation" => scope with { RequiresRevocationPath = false },
            "no-repair" => scope with { RequiresRepairPath = false },
            "no-loss" => scope with { RequiresLossCondition = false },
            "no-enactment-boundary" => scope with { RequiresSeparateEnactmentBoundary = false },
            "no-local" => scope with { RequiresLocalEffectBoundary = false },
            "no-reversible" => scope with { RequiresReversibility = false },
            "warrant" => scope with { AllowsPacketAsWarrant = true },
            "admission" => scope with { AllowsPacketAsAdmission = true },
            "authority" => scope with { AllowsPacketAsAuthority = true },
            "continuity" => scope with { AllowsPacketAsContinuity = true },
            "execution" => scope with { AllowsExecution = true },
            "runtime" => scope with { AllowsRuntimeMotion = true },
            "lisp" => scope with { AllowsLispEvaluation = true },
            "packet" => scope with { AllowsMembranePacketEmission = true },
            "replay" => scope with { AllowsReceiptReplay = true },
            "passage" => scope with { AllowsPassageIncrement = true },
            "activation" => scope with { AllowsActivation = true },
            _ => scope
        };

    private static ScopedWorkPacketNonExecutionBoundary MutateNonExecution(
        ScopedWorkPacketNonExecutionBoundary boundary,
        string mutation) =>
        mutation switch
        {
            "warrant" => boundary with { WorkPacketMayBecomeWarrant = true },
            "admission" => boundary with { WorkPacketMayBecomeAdmission = true },
            "authorize" => boundary with { WorkPacketMayAuthorize = true },
            "execute" => boundary with { WorkPacketMayExecute = true },
            "authority" => boundary with { WorkPacketMayGrantAuthority = true },
            "continuity" => boundary with { WorkPacketMayAdmitContinuity = true },
            "route-executes" => boundary with { StewardRoutingMayExecute = true },
            "reversibility-authorizes" => boundary with { ReversibilityMayAuthorize = true },
            "locality-authorizes" => boundary with { LocalityMayAuthorize = true },
            "lisp" => boundary with { WorkPacketMayEvaluateLisp = true },
            "packet" => boundary with { WorkPacketMayEmitMembranePacket = true },
            "replay" => boundary with { WorkPacketMayReplayReceipt = true },
            "passage" => boundary with { WorkPacketMayIncrementPassage = true },
            "activation" => boundary with { WorkPacketMayActivate = true },
            _ => boundary
        };

    private static ScopedWorkPacketDeclaration MutatePacket(
        ScopedWorkPacketDeclaration packet,
        string mutation) =>
        mutation switch
        {
            "missing-packet" => packet with { PacketHandle = string.Empty },
            "missing-selection" => packet with { SourceSelectionHandle = string.Empty },
            "missing-candidate" => packet with { SourceMaturationCandidateHandle = string.Empty },
            "missing-statement" => packet with { SourcePayloadStatementHandle = string.Empty },
            "missing-duty" => packet with { DutyStation = string.Empty },
            "missing-surface" => packet with { WorkSurface = string.Empty },
            "missing-work" => packet with { IntendedWork = string.Empty },
            "missing-method" => packet with { MethodCode = string.Empty },
            "missing-ceiling" => packet with { AuthorityCeiling = string.Empty },
            "missing-custody" => packet with { CustodyOwner = string.Empty },
            "missing-witness" => packet with { WitnessHandle = string.Empty },
            "missing-telemetry" => packet with { TelemetryRoute = string.Empty },
            "missing-steward" => packet with { StewardRoute = string.Empty },
            "missing-revocation" => packet with { RevocationPath = string.Empty },
            "missing-repair" => packet with { RepairPath = string.Empty },
            "missing-loss" => packet with { LossCondition = string.Empty },
            "not-review" => packet with { ReviewOnly = false },
            "not-candidate" => packet with { CandidateOnly = false },
            "not-local" => packet with { LocalOnly = false },
            "not-reversible" => packet with { ReversibleOnly = false },
            "no-steward-review" => packet with { RequiresStewardReview = false },
            "no-enactment-boundary" => packet with { RequiresSeparateEnactmentBoundary = false },
            "warrant" => packet with { PacketBecomesWarrant = true },
            "admission" => packet with { PacketBecomesAdmission = true },
            "authority" => packet with { PacketGrantsAuthority = true },
            "continuity" => packet with { PacketAdmitsContinuity = true },
            "authorize" => packet with { PacketAuthorizesAction = true },
            "execute" => packet with { PacketExecutesAction = true },
            "lisp" => packet with { PacketEvaluatesLisp = true },
            "packet" => packet with { PacketEmitsMembranePacket = true },
            "replay" => packet with { PacketReplaysReceipt = true },
            "passage" => packet with { PacketIncrementsPassage = true },
            "activation" => packet with { PacketActivates = true },
            _ => packet
        };

    private static ScopedWorkPacketStewardRoute MutateRoute(
        ScopedWorkPacketStewardRoute route,
        string mutation) =>
        mutation switch
        {
            "missing-route" => route with { RouteHandle = string.Empty },
            "missing-packet" => route with { PacketHandle = string.Empty },
            "missing-steward" => route with { StewardSurface = string.Empty },
            "missing-custody" => route with { CustodyOwner = string.Empty },
            "missing-evidence" => route with { EvidenceHandle = string.Empty },
            "missing-witness" => route with { WitnessHandle = string.Empty },
            "missing-telemetry" => route with { TelemetryRoute = string.Empty },
            "missing-return" => route with { ReturnPathHandle = string.Empty },
            "not-review" => route with { ReviewOnly = false },
            "no-packet-lineage" => route with { PreservesPacketLineage = false },
            "no-selection-lineage" => route with { PreservesSelectionLineage = false },
            "no-compost-lineage" => route with { PreservesCompostLineage = false },
            "no-steward-review" => route with { RoutesToStewardReview = false },
            "no-cooling" => route with { RequiresCooling = false },
            "authorize" => route with { RouteAuthorizesAction = true },
            "execute" => route with { RouteExecutesAction = true },
            "authority" => route with { RouteGrantsAuthority = true },
            "continuity" => route with { RouteAdmitsContinuity = true },
            "lisp" => route with { RouteEvaluatesLisp = true },
            "packet" => route with { RouteEmitsMembranePacket = true },
            "activation" => route with { RouteActivates = true },
            "unknown-packet" => route with { PacketHandle = "urn:san:scoped-work-packet:missing" },
            _ => route
        };

    private static void AssertCold(ScopedWorkPacketFormationReceipt receipt)
    {
        Assert.True(receipt.IsColdScopedWorkPacketFormation);
        Assert.Null(receipt.Refusal);
        Assert.True(receipt.ReviewOnly);
        Assert.True(receipt.CandidateOnly);
        Assert.Equal(receipt.PriorPassageCount, receipt.PassageCountAfterPacketFormation);
        Assert.False(receipt.PacketBecameWarrant);
        Assert.False(receipt.PacketBecameAdmission);
        Assert.False(receipt.PacketGrantedAuthority);
        Assert.False(receipt.PacketAdmittedContinuity);
        Assert.False(receipt.PacketAuthorizedAction);
        Assert.False(receipt.PacketExecutedAction);
        Assert.False(receipt.ReversibilityAuthorizedAction);
        Assert.False(receipt.LocalityAuthorizedAction);
        Assert.False(receipt.RuntimeActionAllowed);
        Assert.False(receipt.LispEvaluationAllowed);
        Assert.False(receipt.NewMembranePacketEmitted);
        Assert.False(receipt.ReceiptsReplayed);
        Assert.False(receipt.ContinuityAdmitted);
        Assert.False(receipt.AuthorityGranted);
        Assert.True(receipt.ActivationRefused);
    }

    private static void AssertRefused(
        ScopedWorkPacketFormationReceipt receipt,
        string outcomeCode)
    {
        Assert.Equal(ScopedWorkPacketFormationDisposition.Refused, receipt.Disposition);
        Assert.Equal(outcomeCode, receipt.OutcomeCode);
        Assert.True(receipt.IsRetainedScopedWorkPacketFormationRefusal);
        Assert.NotNull(receipt.Refusal);
        Assert.Empty(receipt.Packets);
        Assert.Empty(receipt.StewardRoutes);
        Assert.Equal(receipt.PriorPassageCount, receipt.PassageCountAfterPacketFormation);
        Assert.False(receipt.PacketAuthorizedAction);
        Assert.False(receipt.PacketExecutedAction);
        Assert.False(receipt.RuntimeActionAllowed);
        Assert.False(receipt.LispEvaluationAllowed);
        Assert.False(receipt.NewMembranePacketEmitted);
        Assert.False(receipt.ContinuityAdmitted);
        Assert.False(receipt.AuthorityGranted);
        Assert.True(receipt.ActivationRefused);
    }

    private static string FindRepositoryRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null)
        {
            var candidate = Path.Combine(directory.FullName, "src", "SLI", "SLI.Lisp", "scoped-work-packet-formation.lisp");
            if (File.Exists(candidate))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        throw new DirectoryNotFoundException("Could not locate OAN Mortalis repository root.");
    }
}
