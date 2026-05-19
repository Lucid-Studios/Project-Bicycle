using System.Security.Cryptography;
using System.Text;
using San.Common;

namespace SLI.Lisp;

public interface ISliCmeActualCrypticBridge
{
    EngramPacket CreateNonActivatingPacket(
        RootAtlasSymbolicReference rootReference,
        string trunkContractRef,
        string branchContractRef,
        string predicateFamily,
        IReadOnlyList<string> symbolicSegments);
}

public sealed class NonActivatingSliCmeActualCrypticBridge : ISliCmeActualCrypticBridge
{
    public EngramPacket CreateNonActivatingPacket(
        RootAtlasSymbolicReference rootReference,
        string trunkContractRef,
        string branchContractRef,
        string predicateFamily,
        IReadOnlyList<string> symbolicSegments)
    {
        ArgumentNullException.ThrowIfNull(rootReference);
        ArgumentNullException.ThrowIfNull(symbolicSegments);

        if (rootReference.SemanticPayloadOpened)
        {
            throw new InvalidOperationException("root atlas semantic payload must remain unopened for the scaffold roundtrip.");
        }

        if (rootReference.MutationAllowed)
        {
            throw new InvalidOperationException("root atlas mutation is not allowed for the scaffold roundtrip.");
        }

        if (string.IsNullOrWhiteSpace(rootReference.ReferenceHandle))
        {
            throw new InvalidOperationException("root atlas symbolic reference requires a handle.");
        }

        if (string.IsNullOrWhiteSpace(trunkContractRef) ||
            string.IsNullOrWhiteSpace(branchContractRef) ||
            string.IsNullOrWhiteSpace(predicateFamily))
        {
            throw new InvalidOperationException("engram packet requires trunk, branch, and predicate family references.");
        }

        if (symbolicSegments.Count == 0 || symbolicSegments.Any(string.IsNullOrWhiteSpace))
        {
            throw new InvalidOperationException("engram packet requires at least one non-empty symbolic segment.");
        }

        var anchorReceipt = AnchorContinuityReceipts.FromRootReference(
            rootReference,
            continuityGate: "sli-lisp-non-activating-anchor-preservation",
            carrierRef: rootReference.ReferenceHandle);
        var nonActivationReceipt = NonActivationReceipts.FromCarrier(
            inertnessGate: "sli-lisp-preserved-not-evaluated",
            carrierRef: rootReference.ReferenceHandle,
            witnessRefs: rootReference.WitnessRefs);
        var packetHandle = CreateHandle("engram-packet://", rootReference.ReferenceHandle, trunkContractRef, branchContractRef);
        var continuityReceipt = ReceiptContinuityReceipts.FromPacket(
            packetHandle,
            anchorReceipt,
            nonActivationReceipt,
            continuityGate: "sli-lisp-receipt-continuity-preserved-not-evaluated",
            witnessRefs: rootReference.WitnessRefs);

        return new EngramPacket(
            PacketHandle: packetHandle,
            RootReferenceHandle: rootReference.ReferenceHandle,
            AnchorContinuityReceipt: anchorReceipt,
            NonActivationReceipt: nonActivationReceipt,
            ReceiptContinuityReceipt: continuityReceipt,
            TrunkContractRef: trunkContractRef,
            BranchContractRef: branchContractRef,
            PredicateFamily: predicateFamily,
            SymbolicSegments: symbolicSegments.ToArray(),
            RawGelPromoted: false,
            RuntimeIdentityEmissionAllowed: false,
            WitnessRefs: rootReference.WitnessRefs);
    }

    private static string CreateHandle(string prefix, params string[] parts)
    {
        var material = string.Join("|", parts.Select(static part => part.Trim()));
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(material));
        return $"{prefix}{Convert.ToHexString(hash).ToLowerInvariant()[..16]}";
    }
}
