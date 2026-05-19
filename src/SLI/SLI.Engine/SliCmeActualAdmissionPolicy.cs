using San.Common;

namespace SLI.Engine;

public interface ISliCmeActualAdmissionPolicy
{
    SliAdmissionReceipt Evaluate(
        EngramPacket packet,
        CMosCertificationReceipt certificationReceipt,
        CrypticFloorEvaluation floorEvaluation,
        DateTimeOffset timestampUtc);
}

public sealed class DefaultSliCmeActualAdmissionPolicy : ISliCmeActualAdmissionPolicy
{
    public SliAdmissionReceipt Evaluate(
        EngramPacket packet,
        CMosCertificationReceipt certificationReceipt,
        CrypticFloorEvaluation floorEvaluation,
        DateTimeOffset timestampUtc)
    {
        ArgumentNullException.ThrowIfNull(packet);
        ArgumentNullException.ThrowIfNull(certificationReceipt);
        ArgumentNullException.ThrowIfNull(floorEvaluation);

        if (packet.RawGelPromoted)
        {
            return Refuse(packet, certificationReceipt, "raw-gel-promotion-blocked", "raw GEL may not enter the scaffold roundtrip.", timestampUtc);
        }

        if (packet.RuntimeIdentityEmissionAllowed || certificationReceipt.RuntimeIdentityEmissionAllowed)
        {
            return Refuse(packet, certificationReceipt, "runtime-identity-emission-blocked", "runtime identity emission is withheld in the scaffold roundtrip.", timestampUtc);
        }

        if (packet.AnchorContinuityReceipt is null)
        {
            return Refuse(packet, certificationReceipt, "anchor-continuity-required", "packet anchor continuity receipt is required before SLI admission.", timestampUtc);
        }

        if (certificationReceipt.AnchorContinuityReceipt is null)
        {
            return Refuse(packet, certificationReceipt, "certification-anchor-continuity-required", "cMoS anchor continuity receipt is required before SLI admission.", timestampUtc);
        }

        if (packet.AnchorContinuityReceipt.HasForbiddenActivation ||
            certificationReceipt.AnchorContinuityReceipt.HasForbiddenActivation)
        {
            return Refuse(packet, certificationReceipt, "anchor-forbidden-activation-blocked", "anchor continuity may not carry payload, runtime identity, doctrine, or mutation.", timestampUtc);
        }

        if (!string.Equals(
                packet.RootReferenceHandle,
                packet.AnchorContinuityReceipt.Anchor.SourceReferenceHandle,
                StringComparison.Ordinal))
        {
            return Refuse(packet, certificationReceipt, "anchor-root-reference-mismatch", "packet root reference must match the preserved anchor source reference.", timestampUtc);
        }

        if (!packet.AnchorContinuityReceipt.HasSameAnchorAs(certificationReceipt.AnchorContinuityReceipt))
        {
            return Refuse(packet, certificationReceipt, "anchor-continuity-mismatch", "packet and cMoS receipts must preserve the same anchor.", timestampUtc);
        }

        if (packet.NonActivationReceipt is null)
        {
            return Refuse(packet, certificationReceipt, "non-activation-required", "packet non-activation receipt is required before scaffold admission.", timestampUtc);
        }

        if (certificationReceipt.NonActivationReceipt is null)
        {
            return Refuse(packet, certificationReceipt, "certification-non-activation-required", "cMoS non-activation receipt is required before scaffold admission.", timestampUtc);
        }

        if (packet.NonActivationReceipt.HasPrematureActivation ||
            certificationReceipt.NonActivationReceipt.HasPrematureActivation)
        {
            return Refuse(packet, certificationReceipt, "premature-activation-blocked", "scaffold objects may be shaped and receipted but not activated.", timestampUtc);
        }

        if (!packet.NonActivationReceipt.HasSameInertnessAs(certificationReceipt.NonActivationReceipt))
        {
            return Refuse(packet, certificationReceipt, "non-activation-state-mismatch", "packet and cMoS receipts must preserve the same inert state.", timestampUtc);
        }

        if (packet.ReceiptContinuityReceipt is null)
        {
            return Refuse(packet, certificationReceipt, "receipt-continuity-required", "packet receipt continuity chain is required before SLI admission.", timestampUtc);
        }

        if (certificationReceipt.ReceiptContinuityReceipt is null)
        {
            return Refuse(packet, certificationReceipt, "certification-receipt-continuity-required", "cMoS receipt continuity chain is required before SLI admission.", timestampUtc);
        }

        if (packet.ReceiptContinuityReceipt.HasForbiddenActivation ||
            certificationReceipt.ReceiptContinuityReceipt.HasForbiddenActivation)
        {
            return Refuse(packet, certificationReceipt, "receipt-continuity-forbidden-activation-blocked", "receipt continuity may not carry payload, runtime action, repair, substitution, collapse, upgrade, or forgery.", timestampUtc);
        }

        if (!packet.ReceiptContinuityReceipt.HasSameAnchorAs(packet.AnchorContinuityReceipt) ||
            !packet.ReceiptContinuityReceipt.HasSameInertnessAs(packet.NonActivationReceipt))
        {
            return Refuse(packet, certificationReceipt, "packet-receipt-continuity-root-mismatch", "packet receipt continuity must preserve packet anchor and inert state roots.", timestampUtc);
        }

        if (!certificationReceipt.ReceiptContinuityReceipt.HasSameAnchorAs(certificationReceipt.AnchorContinuityReceipt) ||
            !certificationReceipt.ReceiptContinuityReceipt.HasSameInertnessAs(certificationReceipt.NonActivationReceipt))
        {
            return Refuse(packet, certificationReceipt, "certification-receipt-continuity-root-mismatch", "cMoS receipt continuity must preserve certification anchor and inert state roots.", timestampUtc);
        }

        if (!packet.ReceiptContinuityReceipt.ContainsPassageRef("engram-packet", packet.PacketHandle))
        {
            return Refuse(packet, certificationReceipt, "receipt-continuity-packet-ref-missing", "packet receipt continuity must include the packet passage reference.", timestampUtc);
        }

        if (!certificationReceipt.ReceiptContinuityReceipt.ContainsPassageRef("cmos-certification", certificationReceipt.ReceiptHandle))
        {
            return Refuse(packet, certificationReceipt, "receipt-continuity-certification-ref-missing", "cMoS receipt continuity must include the certification passage reference.", timestampUtc);
        }

        if (!certificationReceipt.ReceiptContinuityReceipt.ExtendsReceipt(packet.ReceiptContinuityReceipt))
        {
            return Refuse(packet, certificationReceipt, "receipt-continuity-chain-mismatch", "cMoS receipt continuity must extend the packet receipt chain without repair or substitution.", timestampUtc);
        }

        if (!string.Equals(packet.PacketHandle, certificationReceipt.EngramPacketHandle, StringComparison.Ordinal))
        {
            return Refuse(packet, certificationReceipt, "certification-packet-mismatch", "cMoS receipt must certify the same engram packet.", timestampUtc);
        }

        if (string.IsNullOrWhiteSpace(certificationReceipt.IssuedRtmeHandle))
        {
            return Withhold(packet, certificationReceipt, "issued-rtme-handle-required", "issued RTME handle is required before SLI admission.", timestampUtc);
        }

        if (!floorEvaluation.PredicateLandingReady || floorEvaluation.Disposition != CrypticFloorDisposition.Ready)
        {
            return Withhold(packet, certificationReceipt, "cryptic-floor-not-ready", floorEvaluation.GovernanceTrace, timestampUtc);
        }

        return CreateReceipt(
            packet,
            certificationReceipt,
            SliCmeActualRoundtripDisposition.Admitted,
            "sli-cme-actual-scaffold-admitted",
            "scaffold-admission-preserves-non-activation-and-receipt-only-posture",
            timestampUtc);
    }

    private static SliAdmissionReceipt Refuse(
        EngramPacket packet,
        CMosCertificationReceipt certificationReceipt,
        string outcomeCode,
        string governanceTrace,
        DateTimeOffset timestampUtc) =>
        CreateReceipt(packet, certificationReceipt, SliCmeActualRoundtripDisposition.Refused, outcomeCode, governanceTrace, timestampUtc);

    private static SliAdmissionReceipt Withhold(
        EngramPacket packet,
        CMosCertificationReceipt certificationReceipt,
        string outcomeCode,
        string governanceTrace,
        DateTimeOffset timestampUtc) =>
        CreateReceipt(packet, certificationReceipt, SliCmeActualRoundtripDisposition.Withheld, outcomeCode, governanceTrace, timestampUtc);

    private static SliAdmissionReceipt CreateReceipt(
        EngramPacket packet,
        CMosCertificationReceipt certificationReceipt,
        SliCmeActualRoundtripDisposition disposition,
        string outcomeCode,
        string governanceTrace,
        DateTimeOffset timestampUtc)
    {
        var receiptHandle = CreateReceiptHandle(packet, certificationReceipt, outcomeCode);
        var anchorReceipt = SelectReceiptAnchor(packet, certificationReceipt);
        var nonActivationReceipt = SelectNonActivationReceipt(packet, certificationReceipt);

        return new SliAdmissionReceipt(
            Disposition: disposition,
            ReceiptHandle: receiptHandle,
            OutcomeCode: outcomeCode,
            GovernanceTrace: governanceTrace,
            EngramPacketHandle: packet.PacketHandle,
            CertificationReceiptHandle: certificationReceipt.ReceiptHandle,
            AnchorContinuityReceipt: anchorReceipt,
            NonActivationReceipt: nonActivationReceipt,
            ReceiptContinuityReceipt: CreateReceiptContinuity(packet, certificationReceipt, receiptHandle, anchorReceipt, nonActivationReceipt),
            RuntimeIdentityEmissionAllowed: false,
            TimestampUtc: timestampUtc);
    }

    private static string CreateReceiptHandle(EngramPacket packet, CMosCertificationReceipt certificationReceipt, string discriminator) =>
        $"sli-admission-receipt://{Math.Abs(HashCode.Combine(packet.PacketHandle, certificationReceipt.ReceiptHandle, discriminator)):x}";

    private static AnchorContinuityReceipt SelectReceiptAnchor(EngramPacket packet, CMosCertificationReceipt certificationReceipt) =>
        packet.AnchorContinuityReceipt ?? certificationReceipt.AnchorContinuityReceipt!;

    private static NonActivationReceipt SelectNonActivationReceipt(EngramPacket packet, CMosCertificationReceipt certificationReceipt) =>
        packet.NonActivationReceipt ?? certificationReceipt.NonActivationReceipt!;

    private static ReceiptContinuityReceipt CreateReceiptContinuity(
        EngramPacket packet,
        CMosCertificationReceipt certificationReceipt,
        string receiptHandle,
        AnchorContinuityReceipt anchorReceipt,
        NonActivationReceipt nonActivationReceipt)
    {
        var priorReceipt = certificationReceipt.ReceiptContinuityReceipt ?? packet.ReceiptContinuityReceipt;
        return priorReceipt is null
            ? null!
            : ReceiptContinuityReceipts.Extend(
                priorReceipt,
                refKind: "sli-admission",
                refHandle: receiptHandle,
                carrierRef: packet.PacketHandle,
                continuityGate: "sli-admission-receipt-continuity",
                anchorContinuityReceipt: anchorReceipt,
                nonActivationReceipt: nonActivationReceipt,
                witnessRefs: [receiptHandle]);
    }
}
