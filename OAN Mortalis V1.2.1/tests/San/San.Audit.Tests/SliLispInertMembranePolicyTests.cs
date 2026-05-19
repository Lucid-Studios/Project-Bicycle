using San.Common;
using San.Nexus.Control;
using SLI.Engine;
using SLI.Lisp;
using Xunit;

namespace San.Audit.Tests;

public sealed class SliLispInertMembranePolicyTests
{
    private static readonly DateTimeOffset TimestampUtc = DateTimeOffset.Parse("2026-05-07T00:00:00Z");

    [Fact]
    public void Evaluate_Shapes_Inert_Symbolic_Carrier_From_Accepted_SW04_Passage()
    {
        var evaluation = Evaluate(CreateAcceptedPassageEvaluation(), LoadLispModules());

        Assert.Equal(SliCmeActualRoundtripDisposition.Admitted, evaluation.Disposition);
        Assert.Equal("sw05-inert-lisp-symbolic-carrier-shaped", evaluation.OutcomeCode);
        Assert.NotNull(evaluation.Carrier);
        Assert.Equal(DefaultSliLispInertMembranePolicy.RoundtripModuleName, evaluation.Carrier!.SourceModuleName);
        Assert.Equal(DefaultSliLispInertMembranePolicy.NonActivationPosture, evaluation.Carrier.NonActivationPosture);
        Assert.Equal(DefaultSliLispInertMembranePolicy.ReceiptContinuityPosture, evaluation.Carrier.ReceiptContinuityPosture);
        Assert.Equal(DefaultSliLispInertMembranePolicy.ReturnPosture, evaluation.Carrier.ReturnPosture);
        Assert.NotEmpty(evaluation.Carrier.ReceiptHandles);
        Assert.Equal(
            ["engram-packet", "cmos-certification", "sli-admission", "cme-actual", "ec-telemetry", "product-engram-response"],
            evaluation.Carrier.OrderedPassageKinds);
        Assert.False(evaluation.Carrier.PayloadOpened);
        Assert.False(evaluation.Carrier.ExecutableLispCarried);
        Assert.False(evaluation.Carrier.GoaControlMatrixDisclosed);
        AssertForbiddenMotionFalse(evaluation);
    }

    [Fact]
    public void Evaluate_Refuses_Drifted_SW04_Passage_Without_Carrier()
    {
        var passage = CreateAcceptedPassageEvaluation() with
        {
            ReceiptPassageAccepted = false,
            OutcomeCode = "receipt-passage-order-mismatch",
            GovernanceTrace = "fixture drift"
        };

        var evaluation = Evaluate(passage, LoadLispModules());

        Assert.Equal(SliCmeActualRoundtripDisposition.Refused, evaluation.Disposition);
        Assert.Equal("sw04-passage-not-accepted", evaluation.OutcomeCode);
        Assert.Contains("receipt-passage-order-mismatch", evaluation.GovernanceTrace, StringComparison.Ordinal);
        Assert.Null(evaluation.Carrier);
        AssertForbiddenMotionFalse(evaluation);
    }

    [Fact]
    public void Evaluate_Withholds_When_Roundtrip_Lisp_Module_Is_Missing()
    {
        var modules = LoadLispModules()
            .Where(pair => !string.Equals(pair.Key, DefaultSliLispInertMembranePolicy.RoundtripModuleName, StringComparison.OrdinalIgnoreCase))
            .ToDictionary(StringComparer.OrdinalIgnoreCase);

        var evaluation = Evaluate(CreateAcceptedPassageEvaluation(), modules);

        Assert.Equal(SliCmeActualRoundtripDisposition.Withheld, evaluation.Disposition);
        Assert.Equal("sli-lisp-roundtrip-module-missing", evaluation.OutcomeCode);
        Assert.Null(evaluation.Carrier);
        AssertForbiddenMotionFalse(evaluation);
    }

    [Fact]
    public void Evaluate_Withholds_When_Required_Inert_Stub_Posture_Is_Missing()
    {
        var modules = LoadLispModules().ToDictionary(StringComparer.OrdinalIgnoreCase);
        modules[DefaultSliLispInertMembranePolicy.RoundtripModuleName] =
            modules[DefaultSliLispInertMembranePolicy.RoundtripModuleName]
                .Replace(DefaultSliLispInertMembranePolicy.LispEvaluationNilPosture, ":lisp-evaluation-requested t", StringComparison.Ordinal);

        var evaluation = Evaluate(CreateAcceptedPassageEvaluation(), modules);

        Assert.Equal(SliCmeActualRoundtripDisposition.Withheld, evaluation.Disposition);
        Assert.Equal("sli-lisp-inert-stub-posture-missing", evaluation.OutcomeCode);
        Assert.Contains(DefaultSliLispInertMembranePolicy.LispEvaluationNilPosture, evaluation.GovernanceTrace, StringComparison.Ordinal);
        Assert.Null(evaluation.Carrier);
        AssertForbiddenMotionFalse(evaluation);
    }

    [Fact]
    public void Carrier_Contains_Label_Alignment_Only()
    {
        var evaluation = Evaluate(CreateAcceptedPassageEvaluation(), LoadLispModules());
        var carrier = evaluation.Carrier!;

        Assert.Equal(":evaluation-handle", carrier.LispFacingLabels["evaluation_handle"]);
        Assert.Equal(":ordered-passage-kinds", carrier.LispFacingLabels["ordered_passage_kinds"]);
        Assert.Equal(":receipt-handles", carrier.LispFacingLabels["receipt_handles"]);
        Assert.Equal(":governance-trace", carrier.LispFacingLabels["governance_trace"]);
        Assert.Equal(DefaultSliLispInertMembranePolicy.NonActivationPosture, carrier.LispFacingLabels["preserves_non_activation"]);
        Assert.Equal(":witness-refs", carrier.LispFacingLabels["witness_refs"]);
        Assert.Equal(DefaultSliLispInertMembranePolicy.ReturnPosture, carrier.LispFacingLabels["sw05_handoff_evidence_prepared"]);
        Assert.DoesNotContain("payload", carrier.LispFacingLabels.Values, StringComparer.OrdinalIgnoreCase);
        Assert.DoesNotContain("eval", carrier.LispFacingLabels.Values, StringComparer.OrdinalIgnoreCase);
        Assert.DoesNotContain("goa-control-matrix", carrier.LispFacingLabels.Values, StringComparer.OrdinalIgnoreCase);
    }

    [Fact]
    public void Evaluate_Does_Not_Load_Compile_Evaluate_Or_Disclose_Control_Matrix()
    {
        var admitted = Evaluate(CreateAcceptedPassageEvaluation(), LoadLispModules());
        var refused = Evaluate(
            CreateAcceptedPassageEvaluation() with { Sw05HandoffEvidencePrepared = false },
            LoadLispModules());
        var withheld = Evaluate(CreateAcceptedPassageEvaluation(), new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase));

        AssertForbiddenMotionFalse(admitted);
        AssertForbiddenMotionFalse(refused);
        AssertForbiddenMotionFalse(withheld);
    }

    private static SliLispInertMembraneEvaluation Evaluate(
        SliCmeActualOrchestrationReceiptPassageEvaluation passageEvaluation,
        IReadOnlyDictionary<string, string> lispModules) =>
        new DefaultSliLispInertMembranePolicy().Evaluate(passageEvaluation, lispModules, TimestampUtc);

    private static IReadOnlyDictionary<string, string> LoadLispModules() =>
        new GovernedCrypticLispBundleService().LoadModules();

    private static SliCmeActualOrchestrationReceiptPassageEvaluation CreateAcceptedPassageEvaluation()
    {
        var roundtrip = CreateOrchestrator().CreateReceiptOnlyRoundtrip(
            CreateRootReference(),
            CreateLandingRequest(),
            CreateCertificationReceipt("placeholder", CreateRootReference()),
            TimestampUtc);

        return new DefaultSliCmeActualOrchestrationReceiptPassagePolicy().Evaluate(roundtrip, TimestampUtc);
    }

    private static SliCmeActualRoundtripOrchestrator CreateOrchestrator() =>
        new(
            new NonActivatingSliCmeActualCrypticBridge(),
            new CrypticFloorEvaluator(),
            new DefaultSliCmeActualAdmissionPolicy());

    private static RootAtlasSymbolicReference CreateRootReference() =>
        new(
            ReferenceHandle: "root-atlas-symbolic-reference://sw05-fixture",
            AtlasLineageRef: "root-atlas-lineage://sw05-metadata-only",
            SymbolicEntryKey: "symbolic-entry://sw05-fixture",
            SourcePosture: "prime-symbolic-metadata-only",
            SemanticPayloadOpened: false,
            MutationAllowed: false,
            WitnessRefs: ["witness://sw05-root-atlas"]);

    private static AnchorContinuityReceipt CreateAnchorReceipt(RootAtlasSymbolicReference rootReference) =>
        AnchorContinuityReceipts.FromRootReference(
            rootReference,
            continuityGate: "sw05-anchor-preservation",
            carrierRef: rootReference.ReferenceHandle);

    private static NonActivationReceipt CreateNonActivationReceipt(RootAtlasSymbolicReference rootReference) =>
        NonActivationReceipts.FromCarrier(
            inertnessGate: "sli-lisp-preserved-not-evaluated",
            carrierRef: rootReference.ReferenceHandle,
            witnessRefs: rootReference.WitnessRefs);

    private static CMosCertificationReceipt CreateCertificationReceipt(string packetHandle, RootAtlasSymbolicReference rootReference)
    {
        var receiptHandle = "cmos-certification://sw05-fixture";
        var anchorReceipt = CreateAnchorReceipt(rootReference);
        var inertReceipt = CreateNonActivationReceipt(rootReference);

        return new(
            ReceiptHandle: receiptHandle,
            EngramPacketHandle: packetHandle,
            AnchorContinuityReceipt: anchorReceipt,
            NonActivationReceipt: inertReceipt,
            ReceiptContinuityReceipt: ReceiptContinuityReceipts.Extend(
                ReceiptContinuityReceipts.FromPacket(
                    packetHandle,
                    anchorReceipt,
                    inertReceipt,
                    continuityGate: "sw05-packet-receipt-continuity",
                    witnessRefs: ["witness://sw05-packet"]),
                refKind: "cmos-certification",
                refHandle: receiptHandle,
                carrierRef: packetHandle,
                continuityGate: "sw05-certification-receipt-continuity",
                anchorContinuityReceipt: anchorReceipt,
                nonActivationReceipt: inertReceipt,
                witnessRefs: ["witness://sw05-cmos"]),
            IssuedRtmeHandle: "issued-rtme://sw05-fixture",
            CertificationPosture: "certified-for-scaffold-only",
            RuntimeIdentityEmissionAllowed: false,
            WitnessRefs: ["witness://sw05-cmos"],
            TimestampUtc: TimestampUtc);
    }

    private static PredicateLandingRequest CreateLandingRequest() =>
        new(
            Envelope: new SymbolicEnvelope(
                Origin: "root-atlas-symbolic-reference://sw05-fixture",
                Family: new SymbolicProductFamily("sli-cme-actual"),
                ProductClass: SymbolicProductClass.CandidateProduct,
                Intent: new SymbolicIntent("sw05-inert-membrane-fixture"),
                Admissibility: AdmissibilityStatus.Admissible,
                ContradictionState: ContradictionState.None,
                MaterializationEligibility: MaterializationEligibility.Restricted,
                PersistenceEligibility: PersistenceEligibility.AuditOnly,
                TraceId: "trace://sw05-fixture"),
            MembraneDecision: MembraneDecision.Accept,
            SanctuaryGelHandle: "sanctuary-gel://sw05-fixture",
            IssuedRtmeHandle: "issued-rtme://sw05-fixture",
            RouteHandle: "route://sw05-fixture",
            RouteKind: PredicateLandingRouteKind.BoundedEcTransit);

    private static void AssertForbiddenMotionFalse(SliLispInertMembraneEvaluation evaluation)
    {
        Assert.False(evaluation.LispEvaluationRequested);
        Assert.False(evaluation.LispLoadRequested);
        Assert.False(evaluation.LispCompileRequested);
        Assert.False(evaluation.MacroExpansionRequested);
        Assert.False(evaluation.MorphologyPromotionRequested);
        Assert.False(evaluation.ModelBindingRequested);
        Assert.False(evaluation.EcStartRequested);
        Assert.False(evaluation.RuntimeIdentityEmitted);
        Assert.False(evaluation.RuntimeActionExecuted);
        Assert.False(evaluation.GelPromotionAllowed);
        Assert.False(evaluation.GoaControlMatrixDisclosed);
    }
}
