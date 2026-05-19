using San.Common;
using San.Nexus.Control;
using SLI.Engine;
using SLI.Lisp;
using SLI.Runtime;
using Xunit;

namespace San.Audit.Tests;

public sealed class SliCmeActualRoundtripScaffoldTests
{
    [Fact]
    public void Cryptic_Bridge_Creates_Non_Activating_Engram_Packet()
    {
        var bridge = new NonActivatingSliCmeActualCrypticBridge();
        var packet = bridge.CreateNonActivatingPacket(
            CreateRootReference(),
            "trunk://root",
            "branch://symbolic",
            "sli-cme-actual",
            ["prime", "cryptic", "steward"]);

        Assert.False(packet.RawGelPromoted);
        Assert.False(packet.RuntimeIdentityEmissionAllowed);
        Assert.Equal("root-atlas-symbolic-reference://fixture", packet.RootReferenceHandle);
        Assert.Equal("root-atlas-symbolic-reference://fixture", packet.AnchorContinuityReceipt.Anchor.SourceReferenceHandle);
        Assert.False(packet.AnchorContinuityReceipt.HasForbiddenActivation);
        Assert.True(packet.NonActivationReceipt.State.IsInert);
        Assert.False(packet.NonActivationReceipt.HasPrematureActivation);
        Assert.True(packet.ReceiptContinuityReceipt.HasSameAnchorAs(packet.AnchorContinuityReceipt));
        Assert.True(packet.ReceiptContinuityReceipt.HasSameInertnessAs(packet.NonActivationReceipt));
        Assert.True(packet.ReceiptContinuityReceipt.ContainsPassageRef("engram-packet", packet.PacketHandle));
        Assert.False(packet.ReceiptContinuityReceipt.HasForbiddenActivation);
        Assert.Equal(3, packet.SymbolicSegments.Count);
    }

    [Fact]
    public void Admission_Policy_Refuses_Raw_Gel_Promotion_And_Runtime_Identity()
    {
        var policy = new DefaultSliCmeActualAdmissionPolicy();
        var packet = CreatePacket() with
        {
            RawGelPromoted = true
        };

        var receipt = policy.Evaluate(
            packet,
            CreateCertificationReceipt(packet.PacketHandle),
            CreateReadyFloorEvaluation(),
            DateTimeOffset.Parse("2026-04-29T00:00:00Z"));

        Assert.Equal(SliCmeActualRoundtripDisposition.Refused, receipt.Disposition);
        Assert.Equal("raw-gel-promotion-blocked", receipt.OutcomeCode);
        Assert.False(receipt.RuntimeIdentityEmissionAllowed);
    }

    [Fact]
    public void Admission_Policy_Admits_Only_When_Certified_And_Cryptic_Floor_Is_Ready()
    {
        var policy = new DefaultSliCmeActualAdmissionPolicy();
        var packet = CreatePacket();

        var receipt = policy.Evaluate(
            packet,
            CreateCertificationReceipt(packet.PacketHandle),
            CreateReadyFloorEvaluation(),
            DateTimeOffset.Parse("2026-04-29T00:00:00Z"));

        Assert.Equal(SliCmeActualRoundtripDisposition.Admitted, receipt.Disposition);
        Assert.Equal("sli-cme-actual-scaffold-admitted", receipt.OutcomeCode);
        Assert.True(packet.AnchorContinuityReceipt.HasSameAnchorAs(receipt.AnchorContinuityReceipt));
        Assert.True(packet.NonActivationReceipt.HasSameInertnessAs(receipt.NonActivationReceipt));
        Assert.True(receipt.ReceiptContinuityReceipt.ExtendsReceipt(packet.ReceiptContinuityReceipt));
        Assert.True(receipt.ReceiptContinuityReceipt.ContainsPassageRef("sli-admission", receipt.ReceiptHandle));
        Assert.False(receipt.RuntimeIdentityEmissionAllowed);
    }

    [Fact]
    public void Anchor_Continuity_Receipt_Preserves_Stable_Metadata_Without_Payload()
    {
        var receipt = CreateAnchorReceipt();
        var clone = receipt with
        {
            CarrierRef = "carrier://changed"
        };

        Assert.Equal(receipt.Anchor.AnchorHandle, clone.Anchor.AnchorHandle);
        Assert.Equal(receipt.Anchor.SourceReferenceHandle, clone.Anchor.SourceReferenceHandle);
        Assert.Equal(receipt.Anchor.SourceLineageRef, clone.Anchor.SourceLineageRef);
        Assert.Equal(receipt.Anchor.SourceGate, clone.Anchor.SourceGate);
        Assert.True(receipt.HasSameAnchorAs(clone));
        Assert.False(receipt.Anchor.PayloadOpened);
        Assert.False(receipt.Anchor.MutationAllowed);
        Assert.False(receipt.Anchor.RuntimeIdentityEmissionAllowed);
        Assert.False(receipt.PayloadCarried);
        Assert.False(receipt.RuntimeIdentityEmitted);
        Assert.False(receipt.DoctrineAdmitted);
    }

    [Fact]
    public void Non_Activation_Receipt_Default_State_Is_Inert()
    {
        var receipt = CreateNonActivationReceipt();

        Assert.True(receipt.State.IsInert);
        Assert.False(receipt.HasPrematureActivation);
        Assert.False(receipt.State.PayloadOpened);
        Assert.False(receipt.State.ModelBindingRequested);
        Assert.False(receipt.State.RuntimeIdentityRequested);
        Assert.False(receipt.State.StateMutationRequested);
        Assert.False(receipt.State.EcStartRequested);
        Assert.False(receipt.State.RuntimeActionRequested);
        Assert.False(receipt.State.LispEvaluationRequested);
        Assert.False(receipt.State.LispMorphologyPromotionRequested);
        Assert.False(receipt.State.DatabaseWriteRequested);
        Assert.False(receipt.State.KnobMutationRequested);
    }

    [Fact]
    public void Admission_Policy_Refuses_Missing_Anchor_Continuity()
    {
        var policy = new DefaultSliCmeActualAdmissionPolicy();
        var packet = CreatePacket() with
        {
            AnchorContinuityReceipt = null!
        };

        var receipt = policy.Evaluate(
            packet,
            CreateCertificationReceipt(packet.PacketHandle),
            CreateReadyFloorEvaluation(),
            DateTimeOffset.Parse("2026-04-29T00:00:00Z"));

        Assert.Equal(SliCmeActualRoundtripDisposition.Refused, receipt.Disposition);
        Assert.Equal("anchor-continuity-required", receipt.OutcomeCode);
        Assert.False(receipt.RuntimeIdentityEmissionAllowed);
    }

    [Fact]
    public void Admission_Policy_Refuses_Missing_Non_Activation_Receipt()
    {
        var policy = new DefaultSliCmeActualAdmissionPolicy();
        var packet = CreatePacket() with
        {
            NonActivationReceipt = null!
        };

        var receipt = policy.Evaluate(
            packet,
            CreateCertificationReceipt(packet.PacketHandle),
            CreateReadyFloorEvaluation(),
            DateTimeOffset.Parse("2026-04-29T00:00:00Z"));

        Assert.Equal(SliCmeActualRoundtripDisposition.Refused, receipt.Disposition);
        Assert.Equal("non-activation-required", receipt.OutcomeCode);
    }

    [Fact]
    public void Admission_Policy_Refuses_Missing_Receipt_Continuity()
    {
        var policy = new DefaultSliCmeActualAdmissionPolicy();
        var packet = CreatePacket() with
        {
            ReceiptContinuityReceipt = null!
        };

        var receipt = policy.Evaluate(
            packet,
            CreateCertificationReceipt(packet.PacketHandle),
            CreateReadyFloorEvaluation(),
            DateTimeOffset.Parse("2026-04-29T00:00:00Z"));

        Assert.Equal(SliCmeActualRoundtripDisposition.Refused, receipt.Disposition);
        Assert.Equal("receipt-continuity-required", receipt.OutcomeCode);
    }

    [Fact]
    public void Admission_Policy_Refuses_Certification_Missing_Receipt_Continuity()
    {
        var policy = new DefaultSliCmeActualAdmissionPolicy();
        var packet = CreatePacket();
        var certification = CreateCertificationReceipt(packet.PacketHandle) with
        {
            ReceiptContinuityReceipt = null!
        };

        var receipt = policy.Evaluate(
            packet,
            certification,
            CreateReadyFloorEvaluation(),
            DateTimeOffset.Parse("2026-04-29T00:00:00Z"));

        Assert.Equal(SliCmeActualRoundtripDisposition.Refused, receipt.Disposition);
        Assert.Equal("certification-receipt-continuity-required", receipt.OutcomeCode);
    }

    [Theory]
    [InlineData("payload")]
    [InlineData("model")]
    [InlineData("runtime-identity")]
    [InlineData("mutation")]
    [InlineData("ec-start")]
    [InlineData("runtime-action")]
    [InlineData("lisp-evaluation")]
    [InlineData("lisp-morphology")]
    [InlineData("database-write")]
    [InlineData("knob-mutation")]
    public void Admission_Policy_Refuses_Premature_Activation_Signals(string activationSignal)
    {
        var policy = new DefaultSliCmeActualAdmissionPolicy();
        var packet = CreatePacket();
        var certification = CreateCertificationReceipt(packet.PacketHandle) with
        {
            NonActivationReceipt = CreateActivatedNonActivationReceipt(activationSignal)
        };

        var receipt = policy.Evaluate(
            packet,
            certification,
            CreateReadyFloorEvaluation(),
            DateTimeOffset.Parse("2026-04-29T00:00:00Z"));

        Assert.Equal(SliCmeActualRoundtripDisposition.Refused, receipt.Disposition);
        Assert.Equal("premature-activation-blocked", receipt.OutcomeCode);
    }

    [Fact]
    public void Admission_Policy_Refuses_Non_Activation_State_Mismatch()
    {
        var policy = new DefaultSliCmeActualAdmissionPolicy();
        var packet = CreatePacket();
        var certification = CreateCertificationReceipt(packet.PacketHandle) with
        {
            NonActivationReceipt = packet.NonActivationReceipt with
            {
                InertnessGate = "different-non-activation-gate"
            }
        };

        var receipt = policy.Evaluate(
            packet,
            certification,
            CreateReadyFloorEvaluation(),
            DateTimeOffset.Parse("2026-04-29T00:00:00Z"));

        Assert.Equal(SliCmeActualRoundtripDisposition.Refused, receipt.Disposition);
        Assert.Equal("non-activation-state-mismatch", receipt.OutcomeCode);
    }

    [Fact]
    public void Admission_Policy_Refuses_Receipt_Continuity_Chain_Mismatch()
    {
        var policy = new DefaultSliCmeActualAdmissionPolicy();
        var packet = CreatePacket();
        var certification = CreateCertificationReceipt(packet.PacketHandle) with
        {
            ReceiptContinuityReceipt = CreateCertificationReceiptContinuity(
                "engram-packet://other",
                "cmos-certification://fixture",
                packet.AnchorContinuityReceipt,
                packet.NonActivationReceipt)
        };

        var receipt = policy.Evaluate(
            packet,
            certification,
            CreateReadyFloorEvaluation(),
            DateTimeOffset.Parse("2026-04-29T00:00:00Z"));

        Assert.Equal(SliCmeActualRoundtripDisposition.Refused, receipt.Disposition);
        Assert.Equal("receipt-continuity-chain-mismatch", receipt.OutcomeCode);
    }

    [Fact]
    public void Admission_Policy_Refuses_Forbidden_Receipt_Continuity_Flags()
    {
        var policy = new DefaultSliCmeActualAdmissionPolicy();
        var packet = CreatePacket();
        var certification = CreateCertificationReceipt(packet.PacketHandle) with
        {
            ReceiptContinuityReceipt = packet.ReceiptContinuityReceipt with
            {
                ReceiptSubstitutionDetected = true
            }
        };

        var receipt = policy.Evaluate(
            packet,
            certification,
            CreateReadyFloorEvaluation(),
            DateTimeOffset.Parse("2026-04-29T00:00:00Z"));

        Assert.Equal(SliCmeActualRoundtripDisposition.Refused, receipt.Disposition);
        Assert.Equal("receipt-continuity-forbidden-activation-blocked", receipt.OutcomeCode);
    }

    [Fact]
    public void Admission_Policy_Refuses_Mutated_Anchor_Continuity()
    {
        var policy = new DefaultSliCmeActualAdmissionPolicy();
        var packet = CreatePacket();
        var mutatedAnchor = packet.AnchorContinuityReceipt.Anchor with
        {
            MutationAllowed = true
        };
        var certification = CreateCertificationReceipt(packet.PacketHandle) with
        {
            AnchorContinuityReceipt = packet.AnchorContinuityReceipt with
            {
                Anchor = mutatedAnchor
            }
        };

        var receipt = policy.Evaluate(
            packet,
            certification,
            CreateReadyFloorEvaluation(),
            DateTimeOffset.Parse("2026-04-29T00:00:00Z"));

        Assert.Equal(SliCmeActualRoundtripDisposition.Refused, receipt.Disposition);
        Assert.Equal("anchor-forbidden-activation-blocked", receipt.OutcomeCode);
    }

    [Fact]
    public void Admission_Policy_Refuses_Mismatched_Anchor_Continuity()
    {
        var policy = new DefaultSliCmeActualAdmissionPolicy();
        var packet = CreatePacket();
        var mismatchedAnchor = packet.AnchorContinuityReceipt.Anchor with
        {
            SourceReferenceHandle = "root-atlas-symbolic-reference://drifted"
        };
        var certification = CreateCertificationReceipt(packet.PacketHandle) with
        {
            AnchorContinuityReceipt = packet.AnchorContinuityReceipt with
            {
                Anchor = mismatchedAnchor
            }
        };

        var receipt = policy.Evaluate(
            packet,
            certification,
            CreateReadyFloorEvaluation(),
            DateTimeOffset.Parse("2026-04-29T00:00:00Z"));

        Assert.Equal(SliCmeActualRoundtripDisposition.Refused, receipt.Disposition);
        Assert.Equal("anchor-continuity-mismatch", receipt.OutcomeCode);
    }

    [Fact]
    public void Admission_Policy_Refuses_Packet_Root_Anchor_Drift()
    {
        var policy = new DefaultSliCmeActualAdmissionPolicy();
        var packet = CreatePacket() with
        {
            RootReferenceHandle = "root-atlas-symbolic-reference://drifted"
        };

        var receipt = policy.Evaluate(
            packet,
            CreateCertificationReceipt(packet.PacketHandle),
            CreateReadyFloorEvaluation(),
            DateTimeOffset.Parse("2026-04-29T00:00:00Z"));

        Assert.Equal(SliCmeActualRoundtripDisposition.Refused, receipt.Disposition);
        Assert.Equal("anchor-root-reference-mismatch", receipt.OutcomeCode);
    }

    [Fact]
    public void Orchestrator_Returns_Receipt_Only_Product_Response()
    {
        var orchestrator = new SliCmeActualRoundtripOrchestrator(
            new NonActivatingSliCmeActualCrypticBridge(),
            new CrypticFloorEvaluator(),
            new DefaultSliCmeActualAdmissionPolicy());

        var result = orchestrator.CreateReceiptOnlyRoundtrip(
            CreateRootReference(),
            CreateLandingRequest(),
            CreateCertificationReceipt("placeholder"),
            DateTimeOffset.Parse("2026-04-29T00:00:00Z"));

        Assert.False(result.EngramPacket.RawGelPromoted);
        Assert.False(result.AdmissionReceipt.RuntimeIdentityEmissionAllowed);
        Assert.False(result.CmeActualContract.RuntimeIdentityEmitted);
        Assert.False(result.TelemetryEvent.RuntimeIdentityEmitted);
        Assert.False(result.TelemetryEvent.RuntimeActionExecuted);
        Assert.True(result.TelemetryEvent.NonActivationReceipt.State.IsInert);
        Assert.False(result.ProductResponse.PublicationReady);
        Assert.False(result.ProductResponse.RuntimeIdentityEmitted);
        Assert.False(result.ProductResponse.RuntimeActionExecuted);
        Assert.True(result.ProductResponse.NonActivationReceipt.State.IsInert);
        Assert.Equal(ProductEngramResponseDisposition.ReceiptOnly, result.ProductResponse.Disposition);
    }

    [Fact]
    public void Orchestrator_Preserves_Anchor_Continuity_Through_Roundtrip()
    {
        var root = CreateRootReference();
        var certification = CreateCertificationReceipt("placeholder", CreateAnchorReceipt(root));
        var orchestrator = new SliCmeActualRoundtripOrchestrator(
            new NonActivatingSliCmeActualCrypticBridge(),
            new CrypticFloorEvaluator(),
            new DefaultSliCmeActualAdmissionPolicy());

        var result = orchestrator.CreateReceiptOnlyRoundtrip(
            root,
            CreateLandingRequest(),
            certification,
            DateTimeOffset.Parse("2026-04-29T00:00:00Z"));

        var anchor = result.EngramPacket.AnchorContinuityReceipt;
        Assert.True(anchor.HasSameAnchorAs(result.AdmissionReceipt.AnchorContinuityReceipt));
        Assert.True(anchor.HasSameAnchorAs(result.CmeActualContract.AnchorContinuityReceipt));
        Assert.True(anchor.HasSameAnchorAs(result.TelemetryEvent.AnchorContinuityReceipt));
        Assert.True(anchor.HasSameAnchorAs(result.ProductResponse.AnchorContinuityReceipt));
        Assert.False(result.ProductResponse.AnchorContinuityReceipt.HasForbiddenActivation);
        Assert.Contains(anchor.ReceiptHandle, result.ProductResponse.ReceiptRefs);

        var inertness = result.EngramPacket.NonActivationReceipt;
        Assert.True(inertness.HasSameInertnessAs(result.AdmissionReceipt.NonActivationReceipt));
        Assert.True(inertness.HasSameInertnessAs(result.CmeActualContract.NonActivationReceipt));
        Assert.True(inertness.HasSameInertnessAs(result.TelemetryEvent.NonActivationReceipt));
        Assert.True(inertness.HasSameInertnessAs(result.ProductResponse.NonActivationReceipt));
        Assert.False(result.ProductResponse.NonActivationReceipt.HasPrematureActivation);
        Assert.Contains(inertness.ReceiptHandle, result.ProductResponse.ReceiptRefs);

        var continuity = result.EngramPacket.ReceiptContinuityReceipt;
        Assert.True(result.AdmissionReceipt.ReceiptContinuityReceipt.ExtendsReceipt(continuity));
        Assert.True(result.CmeActualContract.ReceiptContinuityReceipt.ExtendsReceipt(result.AdmissionReceipt.ReceiptContinuityReceipt));
        Assert.True(result.TelemetryEvent.ReceiptContinuityReceipt.ExtendsReceipt(result.CmeActualContract.ReceiptContinuityReceipt));
        Assert.True(result.ProductResponse.ReceiptContinuityReceipt.ExtendsReceipt(result.TelemetryEvent.ReceiptContinuityReceipt));
        Assert.Contains(continuity.ReceiptHandle, result.ProductResponse.ReceiptRefs);
        Assert.Contains(result.ProductResponse.ReceiptContinuityReceipt.ReceiptHandle, result.ProductResponse.ReceiptRefs);
        Assert.False(result.ProductResponse.ReceiptContinuityReceipt.HasForbiddenActivation);
    }

    [Fact]
    public void Orchestrator_Does_Not_Repair_Non_Placeholder_Broken_Receipt_Continuity()
    {
        var root = CreateRootReference();
        var certification = CreateCertificationReceipt(
            "engram-packet://other",
            CreateAnchorReceipt(root),
            CreateNonActivationReceipt());
        var orchestrator = new SliCmeActualRoundtripOrchestrator(
            new NonActivatingSliCmeActualCrypticBridge(),
            new CrypticFloorEvaluator(),
            new DefaultSliCmeActualAdmissionPolicy());

        var result = orchestrator.CreateReceiptOnlyRoundtrip(
            root,
            CreateLandingRequest(),
            certification,
            DateTimeOffset.Parse("2026-04-29T00:00:00Z"));

        Assert.Equal(SliCmeActualRoundtripDisposition.Refused, result.AdmissionReceipt.Disposition);
        Assert.Equal("receipt-continuity-chain-mismatch", result.AdmissionReceipt.OutcomeCode);
        Assert.DoesNotContain(result.EngramPacket.PacketHandle, certification.ReceiptContinuityReceipt.Chain.PassageRefs.Select(passageRef => passageRef.RefHandle));
    }

    [Fact]
    public void Lisp_Roundtrip_Stub_Marks_Preserved_Not_Evaluated()
    {
        var stubPath = Path.Combine(GetLineRoot(), "src", "SLI", "SLI.Lisp", "sli-cme-actual-roundtrip.lisp");
        var stub = File.ReadAllText(stubPath).ToLowerInvariant();

        Assert.Contains(":non-activation :preserved-not-evaluated", stub);
        Assert.Contains(":receipt-continuity :proof-of-passage-preserved", stub);
        Assert.Contains(":receipt-continuity-repair-attempted nil", stub);
        Assert.Contains(":receipt-continuity-substitution-detected nil", stub);
        Assert.Contains(":lisp-evaluation-requested nil", stub);
        Assert.Contains(":lisp-morphology-promotion-requested nil", stub);
        Assert.DoesNotContain("(eval", stub);
        Assert.DoesNotContain("(compile", stub);
        Assert.DoesNotContain("(load", stub);
    }

    [Fact]
    public void Telemetry_And_Product_Response_Do_Not_Add_Payload_Fields()
    {
        Assert.DoesNotContain(
            typeof(EcDuplexTelemetryEventContract).GetProperties(),
            property => property.Name.Contains("Payload", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(
            typeof(ProductEngramResponseContract).GetProperties(),
            property => property.Name.Contains("Payload", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void Steward_Extension_Contracts_Do_Not_Mint_New_Prime_Cryptic_Roots()
    {
        var steward = new StewardExtensionContract(
            "steward-extension://billing",
            "urn:san:regional-sanctuary-identity:fixture",
            "Billing",
            "domain-steward-extension",
            MintsPrimeCrypticRoot: false,
            WitnessRefs: ["witness://steward"]);

        var cradleTek = new CradleTekDomainStewardContract(
            "cradletek-domain://billing",
            steward.ParentSanctuaryActualId,
            "Billing",
            steward.ExtensionHandle,
            MintsPrimeCrypticRoot: false,
            WitnessRefs: ["witness://cradletek"]);

        Assert.False(steward.MintsPrimeCrypticRoot);
        Assert.False(cradleTek.MintsPrimeCrypticRoot);
        Assert.Equal(steward.ParentSanctuaryActualId, cradleTek.ParentSanctuaryActualId);
    }

    [Fact]
    public void Scaffold_Files_Are_Present_Only_In_V121_Target_Surface()
    {
        var lineRoot = GetLineRoot();
        var expectedFiles = new[]
        {
            Path.Combine(lineRoot, "src", "San", "San.Common", "SliCmeActualRoundtripContracts.cs"),
            Path.Combine(lineRoot, "src", "SLI", "SLI.Lisp", "SliCmeActualCrypticBridge.cs"),
            Path.Combine(lineRoot, "src", "SLI", "SLI.Lisp", "sli-cme-actual-roundtrip.lisp"),
            Path.Combine(lineRoot, "src", "SLI", "SLI.Engine", "SliCmeActualAdmissionPolicy.cs"),
            Path.Combine(lineRoot, "src", "SLI", "SLI.Runtime", "EcDuplexRoundtripContracts.cs"),
            Path.Combine(lineRoot, "src", "San", "San.Nexus.Control", "SliCmeActualRoundtripOrchestrator.cs"),
            Path.Combine(lineRoot, "tests", "San", "San.Audit.Tests", "SliCmeActualRoundtripScaffoldTests.cs")
        };

        foreach (var file in expectedFiles)
        {
            Assert.True(File.Exists(file), $"expected scaffold file missing: {file}");
        }
    }

    [Fact]
    public void Stress_Single_Reference_Baseline_Preserves_Inert_Receipt_Path()
    {
        var result = CreateSyntheticRoundtrip(1);

        AssertSyntheticRoundtripPreserved(result, 1);
        Assert.Equal(SliCmeActualRoundtripDisposition.Admitted, result.AdmissionReceipt.Disposition);
        Assert.Equal(ProductEngramResponseDisposition.ReceiptOnly, result.ProductResponse.Disposition);
    }

    [Fact]
    public void Stress_Many_Symbolic_References_Preserve_Unique_Inert_Receipts()
    {
        var smallBatch = CreateSyntheticRoundtripBatch(16);
        var mediumBatch = CreateSyntheticRoundtripBatch(128, startOrdinal: 1001);
        var allResults = smallBatch.Concat(mediumBatch).ToArray();

        Assert.Equal(16, smallBatch.Count);
        Assert.Equal(128, mediumBatch.Count);
        Assert.Equal(144, allResults.Select(result => result.EngramPacket.PacketHandle).Distinct(StringComparer.Ordinal).Count());
        Assert.All(allResults, result => AssertSyntheticRoundtripPreserved(result, ExtractSyntheticOrdinal(result)));
    }

    [Fact]
    public void Stress_Duplicate_Anchor_Refusal_Is_Detected_By_Audit_Harness()
    {
        var duplicateRoots = new[]
        {
            CreateSyntheticRootReference(7),
            CreateSyntheticRootReference(7)
        };

        var exception = Assert.Throws<InvalidOperationException>(() => ValidateUniqueSyntheticReferences(duplicateRoots));

        Assert.Contains("duplicate synthetic reference", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Stress_Missing_Anchor_Continuity_Is_Refused()
    {
        var policy = new DefaultSliCmeActualAdmissionPolicy();
        var packet = CreateSyntheticPacket(10) with
        {
            AnchorContinuityReceipt = null!
        };

        var receipt = policy.Evaluate(
            packet,
            CreateCertificationReceipt(packet.PacketHandle, packet.AnchorContinuityReceipt, packet.NonActivationReceipt),
            CreateReadyFloorEvaluation(10),
            DateTimeOffset.Parse("2026-04-30T00:00:00Z"));

        Assert.Equal(SliCmeActualRoundtripDisposition.Refused, receipt.Disposition);
        Assert.Equal("anchor-continuity-required", receipt.OutcomeCode);
        Assert.False(receipt.RuntimeIdentityEmissionAllowed);
    }

    [Fact]
    public void Stress_Missing_Non_Activation_Receipt_Is_Refused()
    {
        var policy = new DefaultSliCmeActualAdmissionPolicy();
        var packet = CreateSyntheticPacket(11) with
        {
            NonActivationReceipt = null!
        };

        var receipt = policy.Evaluate(
            packet,
            CreateCertificationReceipt(packet.PacketHandle, packet.AnchorContinuityReceipt, packet.NonActivationReceipt),
            CreateReadyFloorEvaluation(11),
            DateTimeOffset.Parse("2026-04-30T00:00:00Z"));

        Assert.Equal(SliCmeActualRoundtripDisposition.Refused, receipt.Disposition);
        Assert.Equal("non-activation-required", receipt.OutcomeCode);
    }

    [Fact]
    public void Stress_Missing_Receipt_Continuity_Is_Refused()
    {
        var policy = new DefaultSliCmeActualAdmissionPolicy();
        var packet = CreateSyntheticPacket(12) with
        {
            ReceiptContinuityReceipt = null!
        };

        var receipt = policy.Evaluate(
            packet,
            CreateCertificationReceipt(packet.PacketHandle, packet.AnchorContinuityReceipt, packet.NonActivationReceipt),
            CreateReadyFloorEvaluation(12),
            DateTimeOffset.Parse("2026-04-30T00:00:00Z"));

        Assert.Equal(SliCmeActualRoundtripDisposition.Refused, receipt.Disposition);
        Assert.Equal("receipt-continuity-required", receipt.OutcomeCode);
    }

    [Fact]
    public void Stress_Mismatched_Receipt_Chain_Is_Refused()
    {
        var policy = new DefaultSliCmeActualAdmissionPolicy();
        var packet = CreateSyntheticPacket(13);
        var certification = CreateCertificationReceipt(
            packet.PacketHandle,
            packet.AnchorContinuityReceipt,
            packet.NonActivationReceipt) with
        {
            ReceiptContinuityReceipt = CreateCertificationReceiptContinuity(
                "engram-packet://synthetic-mismatch",
                "cmos-certification://fixture",
                packet.AnchorContinuityReceipt,
                packet.NonActivationReceipt)
        };

        var receipt = policy.Evaluate(
            packet,
            certification,
            CreateReadyFloorEvaluation(13),
            DateTimeOffset.Parse("2026-04-30T00:00:00Z"));

        Assert.Equal(SliCmeActualRoundtripDisposition.Refused, receipt.Disposition);
        Assert.Equal("receipt-continuity-chain-mismatch", receipt.OutcomeCode);
    }

    [Fact]
    public void Stress_Ordered_Passage_Is_Preserved_Through_Product_Response()
    {
        var result = CreateSyntheticRoundtrip(14);
        var refs = result.ProductResponse.ReceiptContinuityReceipt.Chain.PassageRefs;

        Assert.Equal(
            ["engram-packet", "cmos-certification", "sli-admission", "cme-actual", "ec-telemetry", "product-engram-response"],
            refs.Select(passageRef => passageRef.RefKind).ToArray());
        Assert.True(result.ProductResponse.ReceiptContinuityReceipt.ExtendsReceipt(result.TelemetryEvent.ReceiptContinuityReceipt));
        Assert.False(result.ProductResponse.ReceiptContinuityReceipt.HasForbiddenActivation);
    }

    [Fact]
    public void Stress_Telemetry_Remains_Metadata_Only_For_Medium_Batch()
    {
        var results = CreateSyntheticRoundtripBatch(128, startOrdinal: 1001);

        Assert.All(results, result =>
        {
            Assert.Equal(EcDuplexRoundtripEventKind.ScaffoldReceipt, result.TelemetryEvent.EventKind);
            Assert.False(result.TelemetryEvent.RuntimeIdentityEmitted);
            Assert.False(result.TelemetryEvent.RuntimeActionExecuted);
            Assert.True(result.TelemetryEvent.NonActivationReceipt.State.IsInert);
            Assert.DoesNotContain(
                typeof(EcDuplexTelemetryEventContract).GetProperties(),
                property => property.Name.Contains("Payload", StringComparison.OrdinalIgnoreCase));
        });
    }

    [Fact]
    public void Stress_Product_Response_Remains_Receipt_Only_For_Medium_Batch()
    {
        var results = CreateSyntheticRoundtripBatch(128, startOrdinal: 1001);

        Assert.All(results, result =>
        {
            Assert.Equal(ProductEngramResponseDisposition.ReceiptOnly, result.ProductResponse.Disposition);
            Assert.False(result.ProductResponse.PublicationReady);
            Assert.False(result.ProductResponse.RuntimeIdentityEmitted);
            Assert.False(result.ProductResponse.RuntimeActionExecuted);
            Assert.Contains(result.ProductResponse.ReceiptContinuityReceipt.ReceiptHandle, result.ProductResponse.ReceiptRefs);
            Assert.DoesNotContain(
                typeof(ProductEngramResponseContract).GetProperties(),
                property => property.Name.Contains("Payload", StringComparison.OrdinalIgnoreCase));
        });
    }

    private static RootAtlasSymbolicReference CreateRootReference() =>
        new(
            ReferenceHandle: "root-atlas-symbolic-reference://fixture",
            AtlasLineageRef: "root-atlas-lineage://metadata-only",
            SymbolicEntryKey: "symbolic-entry://fixture",
            SourcePosture: "prime-symbolic-metadata-only",
            SemanticPayloadOpened: false,
            MutationAllowed: false,
            WitnessRefs: ["witness://root-atlas"]);

    private static RootAtlasSymbolicReference CreateSyntheticRootReference(int ordinal) =>
        new(
            ReferenceHandle: FormatSyntheticReferenceHandle(ordinal),
            AtlasLineageRef: "synthetic-root-atlas-lineage://stress-metadata-only",
            SymbolicEntryKey: $"synthetic-symbolic-entry://stress/{ordinal:D4}",
            SourcePosture: "synthetic-prime-symbolic-metadata-only",
            SemanticPayloadOpened: false,
            MutationAllowed: false,
            WitnessRefs: [$"witness://synthetic-root-atlas/{ordinal:D4}"]);

    private static EngramPacket CreatePacket()
    {
        var packetHandle = "engram-packet://fixture";
        var anchorReceipt = CreateAnchorReceipt();
        var nonActivationReceipt = CreateNonActivationReceipt();

        return new(
            PacketHandle: "engram-packet://fixture",
            RootReferenceHandle: "root-atlas-symbolic-reference://fixture",
            AnchorContinuityReceipt: anchorReceipt,
            NonActivationReceipt: nonActivationReceipt,
            ReceiptContinuityReceipt: CreatePacketReceiptContinuity(packetHandle, anchorReceipt, nonActivationReceipt),
            TrunkContractRef: "trunk://fixture",
            BranchContractRef: "branch://fixture",
            PredicateFamily: "sli-cme-actual",
            SymbolicSegments: ["prime", "cryptic", "steward"],
            RawGelPromoted: false,
            RuntimeIdentityEmissionAllowed: false,
            WitnessRefs: ["witness://packet"]);
    }

    private static AnchorContinuityReceipt CreateAnchorReceipt() =>
        CreateAnchorReceipt(CreateRootReference());

    private static AnchorContinuityReceipt CreateAnchorReceipt(RootAtlasSymbolicReference rootReference) =>
        AnchorContinuityReceipts.FromRootReference(
            rootReference,
            continuityGate: "test-anchor-preservation",
            carrierRef: rootReference.ReferenceHandle);

    private static NonActivationReceipt CreateNonActivationReceipt() =>
        NonActivationReceipts.FromCarrier(
            inertnessGate: "sli-lisp-preserved-not-evaluated",
            carrierRef: "root-atlas-symbolic-reference://fixture",
            witnessRefs: ["witness://root-atlas"]);

    private static NonActivationReceipt CreateNonActivationReceipt(string carrierRef) =>
        NonActivationReceipts.FromCarrier(
            inertnessGate: "sli-lisp-preserved-not-evaluated",
            carrierRef: carrierRef,
            witnessRefs: [$"witness://{carrierRef}"]);

    private static NonActivationReceipt CreateNonActivationReceipt(string carrierRef, IReadOnlyList<string> witnessRefs) =>
        NonActivationReceipts.FromCarrier(
            inertnessGate: "sli-lisp-preserved-not-evaluated",
            carrierRef: carrierRef,
            witnessRefs: witnessRefs);

    private static ReceiptContinuityReceipt CreatePacketReceiptContinuity(
        string packetHandle,
        AnchorContinuityReceipt anchorContinuityReceipt,
        NonActivationReceipt nonActivationReceipt) =>
        ReceiptContinuityReceipts.FromPacket(
            packetHandle,
            anchorContinuityReceipt,
            nonActivationReceipt,
            continuityGate: "test-packet-receipt-continuity",
            witnessRefs: ["witness://receipt-continuity"]);

    private static ReceiptContinuityReceipt CreateCertificationReceiptContinuity(
        string packetHandle,
        string certificationReceiptHandle,
        AnchorContinuityReceipt anchorContinuityReceipt,
        NonActivationReceipt nonActivationReceipt) =>
        ReceiptContinuityReceipts.Extend(
            CreatePacketReceiptContinuity(packetHandle, anchorContinuityReceipt, nonActivationReceipt),
            refKind: "cmos-certification",
            refHandle: certificationReceiptHandle,
            carrierRef: packetHandle,
            continuityGate: "test-certification-receipt-continuity",
            anchorContinuityReceipt: anchorContinuityReceipt,
            nonActivationReceipt: nonActivationReceipt,
            witnessRefs: ["witness://cmos"]);

    private static NonActivationReceipt CreateActivatedNonActivationReceipt(string activationSignal)
    {
        var receipt = CreateNonActivationReceipt();
        var state = activationSignal switch
        {
            "payload" => receipt.State with { PayloadOpened = true },
            "model" => receipt.State with { ModelBindingRequested = true },
            "runtime-identity" => receipt.State with { RuntimeIdentityRequested = true },
            "mutation" => receipt.State with { StateMutationRequested = true },
            "ec-start" => receipt.State with { EcStartRequested = true },
            "runtime-action" => receipt.State with { RuntimeActionRequested = true },
            "lisp-evaluation" => receipt.State with { LispEvaluationRequested = true },
            "lisp-morphology" => receipt.State with { LispMorphologyPromotionRequested = true },
            "database-write" => receipt.State with { DatabaseWriteRequested = true },
            "knob-mutation" => receipt.State with { KnobMutationRequested = true },
            _ => throw new InvalidOperationException($"unknown activation signal: {activationSignal}")
        };

        return receipt with
        {
            State = state
        };
    }

    private static CMosCertificationReceipt CreateCertificationReceipt(
        string packetHandle,
        AnchorContinuityReceipt? anchorContinuityReceipt = null,
        NonActivationReceipt? nonActivationReceipt = null)
    {
        var receiptHandle = "cmos-certification://fixture";
        var anchorReceipt = anchorContinuityReceipt ?? CreateAnchorReceipt();
        var inertReceipt = nonActivationReceipt ?? CreateNonActivationReceipt();

        return new(
            ReceiptHandle: "cmos-certification://fixture",
            EngramPacketHandle: packetHandle,
            AnchorContinuityReceipt: anchorReceipt,
            NonActivationReceipt: inertReceipt,
            ReceiptContinuityReceipt: CreateCertificationReceiptContinuity(packetHandle, receiptHandle, anchorReceipt, inertReceipt),
            IssuedRtmeHandle: "issued-rtme://fixture",
            CertificationPosture: "certified-for-scaffold-only",
            RuntimeIdentityEmissionAllowed: false,
            WitnessRefs: ["witness://cmos"],
            TimestampUtc: DateTimeOffset.Parse("2026-04-29T00:00:00Z"));
    }

    private static CrypticFloorEvaluation CreateReadyFloorEvaluation() =>
        new(
            PredicateLandingReady: true,
            Disposition: CrypticFloorDisposition.Ready,
            OutcomeCode: "predicate-landing-ready",
            GovernanceTrace: "test-ready-floor",
            Envelope: CreateEnvelope());

    private static CrypticFloorEvaluation CreateReadyFloorEvaluation(int ordinal) =>
        new(
            PredicateLandingReady: true,
            Disposition: CrypticFloorDisposition.Ready,
            OutcomeCode: "predicate-landing-ready",
            GovernanceTrace: $"test-ready-floor-stress-{ordinal:D4}",
            Envelope: CreateEnvelope(ordinal));

    private static PredicateLandingRequest CreateLandingRequest() =>
        new(
            Envelope: CreateEnvelope(),
            MembraneDecision: MembraneDecision.Accept,
            SanctuaryGelHandle: "sanctuary-gel://fixture",
            IssuedRtmeHandle: "issued-rtme://fixture",
            RouteHandle: "route://fixture",
            RouteKind: PredicateLandingRouteKind.BoundedEcTransit);

    private static PredicateLandingRequest CreateLandingRequest(int ordinal) =>
        new(
            Envelope: CreateEnvelope(ordinal),
            MembraneDecision: MembraneDecision.Accept,
            SanctuaryGelHandle: $"sanctuary-gel://stress/{ordinal:D4}",
            IssuedRtmeHandle: $"issued-rtme://stress/{ordinal:D4}",
            RouteHandle: $"route://stress/{ordinal:D4}",
            RouteKind: PredicateLandingRouteKind.BoundedEcTransit);

    private static SymbolicEnvelope CreateEnvelope() =>
        new(
            Origin: "root-atlas-symbolic-reference://fixture",
            Family: new SymbolicProductFamily("sli-cme-actual"),
            ProductClass: SymbolicProductClass.CandidateProduct,
            Intent: new SymbolicIntent("minimal-roundtrip-scaffold"),
            Admissibility: AdmissibilityStatus.Admissible,
            ContradictionState: ContradictionState.None,
            MaterializationEligibility: MaterializationEligibility.Restricted,
            PersistenceEligibility: PersistenceEligibility.AuditOnly,
            TraceId: "trace://fixture");

    private static SymbolicEnvelope CreateEnvelope(int ordinal) =>
        new(
            Origin: FormatSyntheticReferenceHandle(ordinal),
            Family: new SymbolicProductFamily("sli-cme-actual"),
            ProductClass: SymbolicProductClass.CandidateProduct,
            Intent: new SymbolicIntent($"minimal-roundtrip-stress-{ordinal:D4}"),
            Admissibility: AdmissibilityStatus.Admissible,
            ContradictionState: ContradictionState.None,
            MaterializationEligibility: MaterializationEligibility.Restricted,
            PersistenceEligibility: PersistenceEligibility.AuditOnly,
            TraceId: $"trace://stress/{ordinal:D4}");

    private static SliCmeActualRoundtripScaffoldResult CreateSyntheticRoundtrip(int ordinal)
    {
        var root = CreateSyntheticRootReference(ordinal);
        var orchestrator = new SliCmeActualRoundtripOrchestrator(
            new NonActivatingSliCmeActualCrypticBridge(),
            new CrypticFloorEvaluator(),
            new DefaultSliCmeActualAdmissionPolicy());

        return orchestrator.CreateReceiptOnlyRoundtrip(
            root,
            CreateLandingRequest(ordinal),
            CreateCertificationReceipt(
                "placeholder",
                CreateAnchorReceipt(root),
                CreateNonActivationReceipt(root.ReferenceHandle, root.WitnessRefs)),
            DateTimeOffset.Parse("2026-04-30T00:00:00Z"));
    }

    private static IReadOnlyList<SliCmeActualRoundtripScaffoldResult> CreateSyntheticRoundtripBatch(int count, int startOrdinal = 1)
    {
        var roots = Enumerable.Range(startOrdinal, count)
            .Select(CreateSyntheticRootReference)
            .ToArray();
        ValidateUniqueSyntheticReferences(roots);

        return Enumerable.Range(startOrdinal, count)
            .Select(CreateSyntheticRoundtrip)
            .ToArray();
    }

    private static EngramPacket CreateSyntheticPacket(int ordinal)
    {
        var bridge = new NonActivatingSliCmeActualCrypticBridge();
        var root = CreateSyntheticRootReference(ordinal);

        return bridge.CreateNonActivatingPacket(
            root,
            $"trunk://stress/{ordinal:D4}",
            $"branch://stress/{ordinal:D4}",
            "sli-cme-actual",
            [$"prime-{ordinal:D4}", $"cryptic-{ordinal:D4}", $"steward-{ordinal:D4}"]);
    }

    private static void ValidateUniqueSyntheticReferences(IReadOnlyList<RootAtlasSymbolicReference> roots)
    {
        var duplicate = roots
            .GroupBy(root => root.ReferenceHandle, StringComparer.Ordinal)
            .FirstOrDefault(group => group.Count() > 1);

        if (duplicate is not null)
        {
            throw new InvalidOperationException($"duplicate synthetic reference detected: {duplicate.Key}");
        }
    }

    private static void AssertSyntheticRoundtripPreserved(SliCmeActualRoundtripScaffoldResult result, int ordinal)
    {
        var expectedReference = FormatSyntheticReferenceHandle(ordinal);

        Assert.Equal(expectedReference, result.EngramPacket.RootReferenceHandle);
        Assert.Equal(expectedReference, result.EngramPacket.AnchorContinuityReceipt.Anchor.SourceReferenceHandle);
        Assert.False(result.EngramPacket.RawGelPromoted);
        Assert.False(result.EngramPacket.RuntimeIdentityEmissionAllowed);
        Assert.False(result.EngramPacket.AnchorContinuityReceipt.HasForbiddenActivation);
        Assert.True(result.EngramPacket.NonActivationReceipt.State.IsInert);
        Assert.False(result.EngramPacket.NonActivationReceipt.HasPrematureActivation);
        Assert.False(result.EngramPacket.ReceiptContinuityReceipt.HasForbiddenActivation);
        Assert.True(result.AdmissionReceipt.ReceiptContinuityReceipt.ExtendsReceipt(result.EngramPacket.ReceiptContinuityReceipt));
        Assert.True(result.CmeActualContract.ReceiptContinuityReceipt.ExtendsReceipt(result.AdmissionReceipt.ReceiptContinuityReceipt));
        Assert.True(result.TelemetryEvent.ReceiptContinuityReceipt.ExtendsReceipt(result.CmeActualContract.ReceiptContinuityReceipt));
        Assert.True(result.ProductResponse.ReceiptContinuityReceipt.ExtendsReceipt(result.TelemetryEvent.ReceiptContinuityReceipt));
        Assert.False(result.CmeActualContract.RuntimeIdentityEmitted);
        Assert.False(result.TelemetryEvent.RuntimeIdentityEmitted);
        Assert.False(result.TelemetryEvent.RuntimeActionExecuted);
        Assert.False(result.ProductResponse.RuntimeIdentityEmitted);
        Assert.False(result.ProductResponse.RuntimeActionExecuted);
    }

    private static int ExtractSyntheticOrdinal(SliCmeActualRoundtripScaffoldResult result)
    {
        var value = result.EngramPacket.RootReferenceHandle.Split('/').Last();
        return int.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
    }

    private static string FormatSyntheticReferenceHandle(int ordinal) =>
        $"synthetic-root-atlas-symbolic-reference://stress/{ordinal:D4}";

    private static string GetLineRoot()
    {
        var current = new DirectoryInfo(AppContext.BaseDirectory);

        while (current is not null && !File.Exists(Path.Combine(current.FullName, "San.sln")))
        {
            current = current.Parent;
        }

        return current?.FullName ?? throw new InvalidOperationException("Unable to locate tool root.");
    }
}
