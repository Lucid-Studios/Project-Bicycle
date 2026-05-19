using San.Common;
using San.Nexus.Control;
using SLI.Engine;
using SLI.Lisp;
using SLI.Runtime;
using Xunit;

namespace San.Audit.Tests;

public sealed class SliCmeActualOrchestrationReceiptPassagePolicyTests
{
    private static readonly DateTimeOffset TimestampUtc = DateTimeOffset.Parse("2026-05-07T00:00:00Z");

    [Fact]
    public void Evaluate_Accepts_Ordered_Receipt_Passage_Without_Authority()
    {
        var evaluation = Evaluate(CreateRoundtrip());

        Assert.True(evaluation.ReceiptPassageAccepted);
        Assert.Equal("sw04-orchestration-receipt-passage-accepted", evaluation.OutcomeCode);
        Assert.Equal(SliCmeActualRoundtripDisposition.Admitted, evaluation.AdmissionDisposition);
        Assert.Equal(
            ["engram-packet", "cmos-certification", "sli-admission", "cme-actual", "ec-telemetry", "product-engram-response"],
            evaluation.OrderedPassageKinds);
        Assert.True(evaluation.PreservesOrder);
        Assert.True(evaluation.PreservesAnchorContinuity);
        Assert.True(evaluation.PreservesNonActivation);
        Assert.True(evaluation.Sw05HandoffEvidencePrepared);
        AssertNoAuthorityOrRuntime(evaluation);
    }

    [Fact]
    public void Evaluate_Carries_Refused_Admission_Disposition_Without_Upgrading_Authority()
    {
        var result = CreateRoundtripWithBrokenCertification();

        var evaluation = Evaluate(result);

        Assert.False(evaluation.ReceiptPassageAccepted);
        Assert.Equal(SliCmeActualRoundtripDisposition.Refused, evaluation.AdmissionDisposition);
        Assert.Equal("receipt-continuity-chain-mismatch", result.AdmissionReceipt.OutcomeCode);
        Assert.Equal("receipt-continuity-extension-mismatch", evaluation.OutcomeCode);
        Assert.False(evaluation.Sw05HandoffEvidencePrepared);
        AssertNoAuthorityOrRuntime(evaluation);
    }

    [Fact]
    public void Evaluate_Refuses_Order_Drift()
    {
        var result = CreateRoundtrip();
        var drifted = result with
        {
            ProductResponse = result.ProductResponse with
            {
                ReceiptContinuityReceipt = result.EngramPacket.ReceiptContinuityReceipt
            }
        };

        var evaluation = Evaluate(drifted);

        Assert.False(evaluation.ReceiptPassageAccepted);
        Assert.False(evaluation.PreservesOrder);
        Assert.Equal("receipt-passage-order-mismatch", evaluation.OutcomeCode);
        Assert.False(evaluation.Sw05HandoffEvidencePrepared);
        AssertNoAuthorityOrRuntime(evaluation);
    }

    [Theory]
    [MemberData(nameof(AuthorityDriftCases))]
    public void Evaluate_Refuses_Payload_Runtime_And_Authority_Drift(
        string caseId,
        SliCmeActualRoundtripScaffoldResult result,
        bool payloadOpened,
        bool runtimeIdentityEmitted,
        bool runtimeActionExecuted,
        bool authorityGranted)
    {
        var evaluation = Evaluate(result);

        Assert.False(evaluation.ReceiptPassageAccepted);
        Assert.Equal("orchestration-authority-or-payload-drift-blocked", evaluation.OutcomeCode);
        Assert.Equal(payloadOpened, evaluation.PayloadOpened);
        Assert.Equal(runtimeIdentityEmitted, evaluation.RuntimeIdentityEmitted);
        Assert.Equal(runtimeActionExecuted, evaluation.RuntimeActionExecuted);
        Assert.Equal(authorityGranted, evaluation.AuthorityGranted);
        Assert.False(evaluation.Sw05HandoffEvidencePrepared);
        Assert.False(string.IsNullOrWhiteSpace(caseId));
    }

    [Fact]
    public void Evaluate_Refuses_Anchor_Continuity_Drift()
    {
        var result = CreateRoundtrip();
        var driftedAnchor = result.ProductResponse.AnchorContinuityReceipt with
        {
            Anchor = result.ProductResponse.AnchorContinuityReceipt.Anchor with
            {
                SourceReferenceHandle = "root-atlas-symbolic-reference://drifted"
            }
        };
        var drifted = result with
        {
            ProductResponse = result.ProductResponse with
            {
                AnchorContinuityReceipt = driftedAnchor
            }
        };

        var evaluation = Evaluate(drifted);

        Assert.False(evaluation.ReceiptPassageAccepted);
        Assert.False(evaluation.PreservesAnchorContinuity);
        Assert.Equal("anchor-continuity-passage-mismatch", evaluation.OutcomeCode);
        Assert.False(evaluation.Sw05HandoffEvidencePrepared);
    }

    [Fact]
    public void Evaluate_Refuses_Non_Activation_Drift()
    {
        var result = CreateRoundtrip();
        var activatedState = result.ProductResponse.NonActivationReceipt.State with
        {
            RuntimeActionRequested = true
        };
        var drifted = result with
        {
            ProductResponse = result.ProductResponse with
            {
                NonActivationReceipt = result.ProductResponse.NonActivationReceipt with
                {
                    State = activatedState
                }
            }
        };

        var evaluation = Evaluate(drifted);

        Assert.False(evaluation.ReceiptPassageAccepted);
        Assert.False(evaluation.PreservesNonActivation);
        Assert.Equal("non-activation-passage-mismatch", evaluation.OutcomeCode);
        Assert.False(evaluation.Sw05HandoffEvidencePrepared);
    }

    public static IEnumerable<object[]> AuthorityDriftCases()
    {
        var result = CreateRoundtrip();
        var payloadRef = result.ProductResponse.ReceiptContinuityReceipt.Chain.PassageRefs.Last() with
        {
            PayloadOpened = true
        };
        var payloadChain = result.ProductResponse.ReceiptContinuityReceipt.Chain with
        {
            PassageRefs = result.ProductResponse.ReceiptContinuityReceipt.Chain.PassageRefs.SkipLast(1).Concat([payloadRef]).ToArray()
        };

        yield return new object[]
        {
            "SW04-AUTH-001",
            result with
            {
                ProductResponse = result.ProductResponse with
                {
                    ReceiptContinuityReceipt = result.ProductResponse.ReceiptContinuityReceipt with
                    {
                        Chain = payloadChain
                    }
                }
            },
            true,
            false,
            false,
            true
        };

        yield return new object[]
        {
            "SW04-AUTH-002",
            result with
            {
                ProductResponse = result.ProductResponse with
                {
                    RuntimeIdentityEmitted = true
                }
            },
            false,
            true,
            false,
            false
        };

        yield return new object[]
        {
            "SW04-AUTH-003",
            result with
            {
                ProductResponse = result.ProductResponse with
                {
                    RuntimeActionExecuted = true
                }
            },
            false,
            false,
            true,
            false
        };

        yield return new object[]
        {
            "SW04-AUTH-004",
            result with
            {
                ProductResponse = result.ProductResponse with
                {
                    PublicationReady = true
                }
            },
            false,
            false,
            false,
            true
        };
    }

    private static SliCmeActualOrchestrationReceiptPassageEvaluation Evaluate(SliCmeActualRoundtripScaffoldResult result) =>
        new DefaultSliCmeActualOrchestrationReceiptPassagePolicy().Evaluate(result, TimestampUtc);

    private static void AssertNoAuthorityOrRuntime(SliCmeActualOrchestrationReceiptPassageEvaluation evaluation)
    {
        Assert.False(evaluation.PayloadOpened);
        Assert.False(evaluation.RuntimeIdentityEmitted);
        Assert.False(evaluation.RuntimeActionExecuted);
        Assert.False(evaluation.AuthorityGranted);
        Assert.False(evaluation.RuntimeWorkStarted);
        Assert.False(evaluation.LispEvaluationAllowed);
        Assert.False(evaluation.ModelBindingAllowed);
        Assert.False(evaluation.GelPromotionAllowed);
        Assert.False(evaluation.MintingAuthorized);
        Assert.False(evaluation.CertificationAuthorized);
    }

    private static SliCmeActualRoundtripScaffoldResult CreateRoundtrip() =>
        CreateOrchestrator().CreateReceiptOnlyRoundtrip(
            CreateRootReference(),
            CreateLandingRequest(),
            CreateCertificationReceipt("placeholder"),
            TimestampUtc);

    private static SliCmeActualRoundtripScaffoldResult CreateRoundtripWithBrokenCertification()
    {
        var root = CreateRootReference();
        return CreateOrchestrator().CreateReceiptOnlyRoundtrip(
            root,
            CreateLandingRequest(),
            CreateCertificationReceipt(
                "engram-packet://other",
                CreateAnchorReceipt(root),
                CreateNonActivationReceipt()),
            TimestampUtc);
    }

    private static SliCmeActualRoundtripOrchestrator CreateOrchestrator() =>
        new(
            new NonActivatingSliCmeActualCrypticBridge(),
            new CrypticFloorEvaluator(),
            new DefaultSliCmeActualAdmissionPolicy());

    private static RootAtlasSymbolicReference CreateRootReference() =>
        new(
            ReferenceHandle: "root-atlas-symbolic-reference://sw04-fixture",
            AtlasLineageRef: "root-atlas-lineage://sw04-metadata-only",
            SymbolicEntryKey: "symbolic-entry://sw04-fixture",
            SourcePosture: "prime-symbolic-metadata-only",
            SemanticPayloadOpened: false,
            MutationAllowed: false,
            WitnessRefs: ["witness://sw04-root-atlas"]);

    private static AnchorContinuityReceipt CreateAnchorReceipt(RootAtlasSymbolicReference rootReference) =>
        AnchorContinuityReceipts.FromRootReference(
            rootReference,
            continuityGate: "sw04-anchor-preservation",
            carrierRef: rootReference.ReferenceHandle);

    private static NonActivationReceipt CreateNonActivationReceipt() =>
        NonActivationReceipts.FromCarrier(
            inertnessGate: "sli-lisp-preserved-not-evaluated",
            carrierRef: "root-atlas-symbolic-reference://sw04-fixture",
            witnessRefs: ["witness://sw04-root-atlas"]);

    private static CMosCertificationReceipt CreateCertificationReceipt(
        string packetHandle,
        AnchorContinuityReceipt? anchorContinuityReceipt = null,
        NonActivationReceipt? nonActivationReceipt = null)
    {
        var receiptHandle = "cmos-certification://sw04-fixture";
        var anchorReceipt = anchorContinuityReceipt ?? CreateAnchorReceipt(CreateRootReference());
        var inertReceipt = nonActivationReceipt ?? CreateNonActivationReceipt();

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
                    continuityGate: "sw04-packet-receipt-continuity",
                    witnessRefs: ["witness://sw04-packet"]),
                refKind: "cmos-certification",
                refHandle: receiptHandle,
                carrierRef: packetHandle,
                continuityGate: "sw04-certification-receipt-continuity",
                anchorContinuityReceipt: anchorReceipt,
                nonActivationReceipt: inertReceipt,
                witnessRefs: ["witness://sw04-cmos"]),
            IssuedRtmeHandle: "issued-rtme://sw04-fixture",
            CertificationPosture: "certified-for-scaffold-only",
            RuntimeIdentityEmissionAllowed: false,
            WitnessRefs: ["witness://sw04-cmos"],
            TimestampUtc: TimestampUtc);
    }

    private static PredicateLandingRequest CreateLandingRequest() =>
        new(
            Envelope: new SymbolicEnvelope(
                Origin: "root-atlas-symbolic-reference://sw04-fixture",
                Family: new SymbolicProductFamily("sli-cme-actual"),
                ProductClass: SymbolicProductClass.CandidateProduct,
                Intent: new SymbolicIntent("sw04-receipt-passage-fixture"),
                Admissibility: AdmissibilityStatus.Admissible,
                ContradictionState: ContradictionState.None,
                MaterializationEligibility: MaterializationEligibility.Restricted,
                PersistenceEligibility: PersistenceEligibility.AuditOnly,
                TraceId: "trace://sw04-fixture"),
            MembraneDecision: MembraneDecision.Accept,
            SanctuaryGelHandle: "sanctuary-gel://sw04-fixture",
            IssuedRtmeHandle: "issued-rtme://sw04-fixture",
            RouteHandle: "route://sw04-fixture",
            RouteKind: PredicateLandingRouteKind.BoundedEcTransit);
}
