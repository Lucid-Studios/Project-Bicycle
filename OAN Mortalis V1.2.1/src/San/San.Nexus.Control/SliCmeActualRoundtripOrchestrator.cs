using San.Common;
using SLI.Engine;
using SLI.Lisp;
using SLI.Runtime;

namespace San.Nexus.Control;

public sealed record SliCmeActualRoundtripScaffoldResult(
    EngramPacket EngramPacket,
    SliAdmissionReceipt AdmissionReceipt,
    CmeActualInstanceContract CmeActualContract,
    EcDuplexTelemetryEventContract TelemetryEvent,
    ProductEngramResponseContract ProductResponse);

public interface ISliCmeActualRoundtripOrchestrator
{
    SliCmeActualRoundtripScaffoldResult CreateReceiptOnlyRoundtrip(
        RootAtlasSymbolicReference rootReference,
        PredicateLandingRequest landingRequest,
        CMosCertificationReceipt certificationReceipt,
        DateTimeOffset timestampUtc);
}

public sealed class SliCmeActualRoundtripOrchestrator : ISliCmeActualRoundtripOrchestrator
{
    private readonly ISliCmeActualCrypticBridge _crypticBridge;
    private readonly ICrypticFloorEvaluator _crypticFloorEvaluator;
    private readonly ISliCmeActualAdmissionPolicy _admissionPolicy;

    public SliCmeActualRoundtripOrchestrator(
        ISliCmeActualCrypticBridge crypticBridge,
        ICrypticFloorEvaluator crypticFloorEvaluator,
        ISliCmeActualAdmissionPolicy admissionPolicy)
    {
        _crypticBridge = crypticBridge;
        _crypticFloorEvaluator = crypticFloorEvaluator;
        _admissionPolicy = admissionPolicy;
    }

    public SliCmeActualRoundtripScaffoldResult CreateReceiptOnlyRoundtrip(
        RootAtlasSymbolicReference rootReference,
        PredicateLandingRequest landingRequest,
        CMosCertificationReceipt certificationReceipt,
        DateTimeOffset timestampUtc)
    {
        ArgumentNullException.ThrowIfNull(rootReference);
        ArgumentNullException.ThrowIfNull(landingRequest);
        ArgumentNullException.ThrowIfNull(certificationReceipt);

        var packet = _crypticBridge.CreateNonActivatingPacket(
            rootReference,
            trunkContractRef: "roundtrip://trunk/root-atlas-symbolic-reference",
            branchContractRef: "roundtrip://branch/sli-cme-actual-scaffold",
            predicateFamily: landingRequest.Envelope.Family.Value,
            symbolicSegments:
            [
                landingRequest.Envelope.Intent.Value,
                landingRequest.Envelope.TraceId
            ]);

        certificationReceipt = BindPlaceholderCertificationIfNeeded(packet, certificationReceipt);

        var floorEvaluation = _crypticFloorEvaluator.Evaluate(landingRequest);
        var admissionReceipt = _admissionPolicy.Evaluate(packet, certificationReceipt, floorEvaluation, timestampUtc);
        var cmeActualContractHandle = $"cme-actual-contract://{Math.Abs(HashCode.Combine(admissionReceipt.ReceiptHandle, packet.PacketHandle)):x}";
        var cmeReceiptContinuity = ReceiptContinuityReceipts.Extend(
            admissionReceipt.ReceiptContinuityReceipt,
            refKind: "cme-actual",
            refHandle: cmeActualContractHandle,
            carrierRef: admissionReceipt.ReceiptHandle,
            continuityGate: "cme-actual-receipt-continuity",
            anchorContinuityReceipt: admissionReceipt.AnchorContinuityReceipt,
            nonActivationReceipt: admissionReceipt.NonActivationReceipt,
            witnessRefs: rootReference.WitnessRefs);
        var cmeActualContract = new CmeActualInstanceContract(
            ContractHandle: cmeActualContractHandle,
            RegionalSanctuaryIdentityHandle: "urn:san:regional-sanctuary-identity:withheld-scaffold",
            SliAdmissionReceiptHandle: admissionReceipt.ReceiptHandle,
            AnchorContinuityReceipt: admissionReceipt.AnchorContinuityReceipt,
            NonActivationReceipt: admissionReceipt.NonActivationReceipt,
            ReceiptContinuityReceipt: cmeReceiptContinuity,
            Disposition: admissionReceipt.Disposition,
            RuntimeIdentityEmitted: false,
            WitnessRefs: rootReference.WitnessRefs);

        var telemetryEvent = EcDuplexRoundtripReceiptFactory.CreateReceiptOnlyTelemetryEvent(
            admissionReceipt,
            cmeActualContract,
            timestampUtc);

        var productResponse = EcDuplexRoundtripReceiptFactory.CreateReceiptOnlyProductResponse(
            telemetryEvent,
            [
                packet.PacketHandle,
                packet.AnchorContinuityReceipt.ReceiptHandle,
                packet.NonActivationReceipt.ReceiptHandle,
                packet.ReceiptContinuityReceipt.ReceiptHandle,
                certificationReceipt.ReceiptHandle,
                certificationReceipt.ReceiptContinuityReceipt.ReceiptHandle,
                admissionReceipt.ReceiptHandle,
                admissionReceipt.ReceiptContinuityReceipt.ReceiptHandle,
                cmeActualContract.ReceiptContinuityReceipt.ReceiptHandle,
                telemetryEvent.EventHandle
            ]);

        return new SliCmeActualRoundtripScaffoldResult(
            packet,
            admissionReceipt,
            cmeActualContract,
            telemetryEvent,
            productResponse);
    }

    private static CMosCertificationReceipt BindPlaceholderCertificationIfNeeded(
        EngramPacket packet,
        CMosCertificationReceipt certificationReceipt)
    {
        if (!string.Equals(certificationReceipt.EngramPacketHandle, "placeholder", StringComparison.Ordinal))
        {
            return certificationReceipt with
            {
                RuntimeIdentityEmissionAllowed = false
            };
        }

        return certificationReceipt with
        {
            EngramPacketHandle = packet.PacketHandle,
            ReceiptContinuityReceipt = ReceiptContinuityReceipts.Extend(
                packet.ReceiptContinuityReceipt,
                refKind: "cmos-certification",
                refHandle: certificationReceipt.ReceiptHandle,
                carrierRef: packet.PacketHandle,
                continuityGate: "nexus-placeholder-certification-binding",
                anchorContinuityReceipt: certificationReceipt.AnchorContinuityReceipt,
                nonActivationReceipt: certificationReceipt.NonActivationReceipt,
                witnessRefs: certificationReceipt.WitnessRefs),
            RuntimeIdentityEmissionAllowed = false
        };
    }
}
