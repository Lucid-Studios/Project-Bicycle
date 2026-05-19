using San.Common;
using San.Nexus.Control;
using SLI.Engine;
using SLI.Lisp;
using Xunit;

namespace San.Audit.Tests;

public sealed class AgentBodyCmeInterconnectEvaluatorTests
{
    private static readonly DateTimeOffset TimestampUtc = DateTimeOffset.Parse("2026-05-12T00:00:00Z");

    [Fact]
    public void Evaluate_Verifies_Agent_Body_Cme_Cold_Interconnect_Topology()
    {
        var roundtrip = CreateRoundtrip();
        var receipt = Evaluate(roundtrip, LoadLispModules());

        Assert.Equal(AgentBodyCmeInterconnectDisposition.VerifiedCold, receipt.Disposition);
        Assert.Equal("agent-body-cme-cold-interconnect-verified", receipt.OutcomeCode);
        Assert.True(receipt.IsCold);
        Assert.Equal(roundtrip.ProductResponse.ResponseHandle, receipt.SliRoundtripResponseHandle);
        Assert.Equal(receipt.Prime.CgoaBundleCmeId, receipt.PrimeReviewConduit.CgoaBundleRef);
        Assert.Equal(AgentBodyReviewConduitKind.CgoaInsulatedPrime, receipt.PrimeReviewConduit.ConduitKind);
        Assert.True(receipt.PrimeReviewConduit.InsulatesPrimeActual);
        Assert.False(receipt.PrimeReviewConduit.GrantsAuthority);
        Assert.False(receipt.PrimeReviewConduit.GrantsIdentity);
        Assert.Equal(receipt.Cryptic.TelemetryStringRef, receipt.CrypticReviewConduit.TelemetryStringRef);
        Assert.Equal(receipt.Cryptic.TelemetryStringRef, receipt.Steward.TelemetryStringRef);
        Assert.Equal(AgentBodyReviewConduitKind.TelemetryStringCryptic, receipt.CrypticReviewConduit.ConduitKind);
        Assert.True(receipt.CrypticReviewConduit.DirectCrypticActualReach);
        Assert.False(receipt.CrypticReviewConduit.GrantsAuthority);
        Assert.False(receipt.CrypticReviewConduit.SelfAuthorizes);
        Assert.Equal(CompassShellCandidateStatus.CandidateOnly, receipt.CompassShell.Status);
        Assert.False(receipt.CompassShell.IsEngram);
        Assert.False(receipt.CompassShell.ContinuityAdmitted);
        Assert.False(receipt.CompassShell.AuthorityGranted);
        Assert.Equal(CleavingDiscernmentDisposition.CandidateOnly, receipt.CleavingDiscernment.Disposition);
        Assert.False(receipt.CleavingDiscernment.EcStartRequested);
        Assert.False(receipt.CleavingDiscernment.RuntimeActionRequested);
        AssertForbiddenMotionFalse(receipt);
    }

    [Fact]
    public void Evaluate_Withholds_When_Agent_Body_Lisp_Module_Is_Missing()
    {
        var modules = LoadLispModules()
            .Where(pair => !string.Equals(pair.Key, DefaultAgentBodyCmeInterconnectEvaluator.AgentBodyModuleName, StringComparison.OrdinalIgnoreCase))
            .ToDictionary(StringComparer.OrdinalIgnoreCase);

        var receipt = Evaluate(CreateRoundtrip(), modules);

        Assert.Equal(AgentBodyCmeInterconnectDisposition.Withheld, receipt.Disposition);
        Assert.Equal("agent-body-cme-lisp-module-missing", receipt.OutcomeCode);
        Assert.False(receipt.IsCold);
        Assert.Equal(CompassShellCandidateStatus.Withheld, receipt.CompassShell.Status);
        Assert.Equal(CleavingDiscernmentDisposition.Withheld, receipt.CleavingDiscernment.Disposition);
        AssertForbiddenMotionFalse(receipt);
    }

    [Fact]
    public void Evaluate_Withholds_When_Telemetry_Authority_Posture_Is_Altered()
    {
        var modules = LoadLispModules().ToDictionary(StringComparer.OrdinalIgnoreCase);
        modules[DefaultAgentBodyCmeInterconnectEvaluator.AgentBodyModuleName] =
            modules[DefaultAgentBodyCmeInterconnectEvaluator.AgentBodyModuleName]
                .Replace(
                    DefaultAgentBodyCmeInterconnectEvaluator.TelemetryAuthorityPosture,
                    ":telemetry-authority :authority",
                    StringComparison.Ordinal);

        var receipt = Evaluate(CreateRoundtrip(), modules);

        Assert.Equal(AgentBodyCmeInterconnectDisposition.Withheld, receipt.Disposition);
        Assert.Equal("agent-body-cme-lisp-posture-missing", receipt.OutcomeCode);
        Assert.Contains(DefaultAgentBodyCmeInterconnectEvaluator.TelemetryAuthorityPosture, receipt.GovernanceTrace, StringComparison.Ordinal);
        Assert.False(receipt.IsCold);
        AssertForbiddenMotionFalse(receipt);
    }

    [Fact]
    public void Evaluate_Refuses_When_Runtime_Action_Drifts_Into_Product_Response()
    {
        var roundtrip = CreateRoundtrip();
        var drifted = roundtrip with
        {
            ProductResponse = roundtrip.ProductResponse with
            {
                RuntimeActionExecuted = true
            }
        };

        var receipt = Evaluate(drifted, LoadLispModules());

        Assert.Equal(AgentBodyCmeInterconnectDisposition.Refused, receipt.Disposition);
        Assert.Equal("agent-body-activation-drift-blocked", receipt.OutcomeCode);
        Assert.Equal(CompassShellCandidateStatus.Refused, receipt.CompassShell.Status);
        Assert.Equal(CleavingDiscernmentDisposition.Refused, receipt.CleavingDiscernment.Disposition);
        AssertForbiddenMotionFalse(receipt);
    }

    private static AgentBodyCmeInterconnectReceipt Evaluate(
        SliCmeActualRoundtripScaffoldResult roundtrip,
        IReadOnlyDictionary<string, string> lispModules) =>
        new DefaultAgentBodyCmeInterconnectEvaluator().Evaluate(roundtrip, lispModules, TimestampUtc);

    private static IReadOnlyDictionary<string, string> LoadLispModules() =>
        new GovernedCrypticLispBundleService().LoadModules();

    private static SliCmeActualRoundtripScaffoldResult CreateRoundtrip()
    {
        var root = CreateRootReference();
        return new SliCmeActualRoundtripOrchestrator(
            new NonActivatingSliCmeActualCrypticBridge(),
            new CrypticFloorEvaluator(),
            new DefaultSliCmeActualAdmissionPolicy())
            .CreateReceiptOnlyRoundtrip(
                root,
                CreateLandingRequest(),
                CreateCertificationReceipt("placeholder", root),
                TimestampUtc);
    }

    private static RootAtlasSymbolicReference CreateRootReference() =>
        new(
            ReferenceHandle: "root-atlas-symbolic-reference://agent-body-fixture",
            AtlasLineageRef: "root-atlas-lineage://agent-body-metadata-only",
            SymbolicEntryKey: "symbolic-entry://agent-body-fixture",
            SourcePosture: "prime-symbolic-metadata-only",
            SemanticPayloadOpened: false,
            MutationAllowed: false,
            WitnessRefs: ["witness://agent-body-root-atlas"]);

    private static AnchorContinuityReceipt CreateAnchorReceipt(RootAtlasSymbolicReference rootReference) =>
        AnchorContinuityReceipts.FromRootReference(
            rootReference,
            continuityGate: "agent-body-anchor-preservation",
            carrierRef: rootReference.ReferenceHandle);

    private static NonActivationReceipt CreateNonActivationReceipt(RootAtlasSymbolicReference rootReference) =>
        NonActivationReceipts.FromCarrier(
            inertnessGate: "sli-lisp-preserved-not-evaluated",
            carrierRef: rootReference.ReferenceHandle,
            witnessRefs: rootReference.WitnessRefs);

    private static CMosCertificationReceipt CreateCertificationReceipt(string packetHandle, RootAtlasSymbolicReference rootReference)
    {
        var receiptHandle = "cmos-certification://agent-body-fixture";
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
                    continuityGate: "agent-body-packet-receipt-continuity",
                    witnessRefs: ["witness://agent-body-packet"]),
                refKind: "cmos-certification",
                refHandle: receiptHandle,
                carrierRef: packetHandle,
                continuityGate: "agent-body-certification-receipt-continuity",
                anchorContinuityReceipt: anchorReceipt,
                nonActivationReceipt: inertReceipt,
                witnessRefs: ["witness://agent-body-cmos"]),
            IssuedRtmeHandle: "issued-rtme://agent-body-fixture",
            CertificationPosture: "certified-for-scaffold-only",
            RuntimeIdentityEmissionAllowed: false,
            WitnessRefs: ["witness://agent-body-cmos"],
            TimestampUtc: TimestampUtc);
    }

    private static PredicateLandingRequest CreateLandingRequest() =>
        new(
            Envelope: new SymbolicEnvelope(
                Origin: "root-atlas-symbolic-reference://agent-body-fixture",
                Family: new SymbolicProductFamily("sli-cme-actual"),
                ProductClass: SymbolicProductClass.CandidateProduct,
                Intent: new SymbolicIntent("agent-body-cme-interconnect-fixture"),
                Admissibility: AdmissibilityStatus.Admissible,
                ContradictionState: ContradictionState.None,
                MaterializationEligibility: MaterializationEligibility.Restricted,
                PersistenceEligibility: PersistenceEligibility.AuditOnly,
                TraceId: "trace://agent-body-fixture"),
            MembraneDecision: MembraneDecision.Accept,
            SanctuaryGelHandle: "sanctuary-gel://agent-body-fixture",
            IssuedRtmeHandle: "issued-rtme://agent-body-fixture",
            RouteHandle: "route://agent-body-fixture",
            RouteKind: PredicateLandingRouteKind.BoundedEcTransit);

    private static void AssertForbiddenMotionFalse(AgentBodyCmeInterconnectReceipt receipt)
    {
        Assert.False(receipt.RuntimeIdentityEmitted);
        Assert.False(receipt.RuntimeActionExecuted);
        Assert.False(receipt.ModelBindingRequested);
        Assert.False(receipt.LispEvaluationRequested);
        Assert.False(receipt.GelPromotionAllowed);
        Assert.False(receipt.CmeActualActivated);
        Assert.False(receipt.SanctuaryActualActivated);
    }
}
