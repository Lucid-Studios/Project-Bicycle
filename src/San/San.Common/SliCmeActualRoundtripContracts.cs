namespace San.Common;

public enum SliCmeActualRoundtripDisposition
{
    Planned = 0,
    Withheld = 1,
    Refused = 2,
    Admitted = 3
}

public sealed record AnchorReference(
    string AnchorHandle,
    string SourceReferenceHandle,
    string SourceLineageRef,
    string SourceGate,
    bool PayloadOpened,
    bool MutationAllowed,
    bool RuntimeIdentityEmissionAllowed,
    IReadOnlyList<string> WitnessRefs)
{
    public bool HasForbiddenActivation =>
        PayloadOpened ||
        MutationAllowed ||
        RuntimeIdentityEmissionAllowed;
}

public sealed record AnchorContinuityReceipt(
    string ReceiptHandle,
    AnchorReference Anchor,
    string ContinuityGate,
    string CarrierRef,
    bool PayloadCarried,
    bool RuntimeIdentityEmitted,
    bool DoctrineAdmitted,
    IReadOnlyList<string> WitnessRefs)
{
    public bool HasForbiddenActivation =>
        Anchor is null ||
        Anchor.HasForbiddenActivation ||
        PayloadCarried ||
        RuntimeIdentityEmitted ||
        DoctrineAdmitted;

    public bool HasSameAnchorAs(AnchorContinuityReceipt? other) =>
        other is not null &&
        Anchor is not null &&
        other.Anchor is not null &&
        string.Equals(Anchor.AnchorHandle, other.Anchor.AnchorHandle, StringComparison.Ordinal) &&
        string.Equals(Anchor.SourceReferenceHandle, other.Anchor.SourceReferenceHandle, StringComparison.Ordinal) &&
        string.Equals(Anchor.SourceLineageRef, other.Anchor.SourceLineageRef, StringComparison.Ordinal) &&
        string.Equals(Anchor.SourceGate, other.Anchor.SourceGate, StringComparison.Ordinal) &&
        Anchor.PayloadOpened == other.Anchor.PayloadOpened &&
        Anchor.MutationAllowed == other.Anchor.MutationAllowed &&
        Anchor.RuntimeIdentityEmissionAllowed == other.Anchor.RuntimeIdentityEmissionAllowed &&
        WitnessRefs.SequenceEqual(other.WitnessRefs) &&
        Anchor.WitnessRefs.SequenceEqual(other.Anchor.WitnessRefs);
}

public sealed record NonActivationState(
    bool PayloadOpened,
    bool ModelBindingRequested,
    bool RuntimeIdentityRequested,
    bool StateMutationRequested,
    bool EcStartRequested,
    bool RuntimeActionRequested,
    bool LispEvaluationRequested,
    bool LispMorphologyPromotionRequested,
    bool DatabaseWriteRequested,
    bool KnobMutationRequested)
{
    public bool IsInert =>
        !PayloadOpened &&
        !ModelBindingRequested &&
        !RuntimeIdentityRequested &&
        !StateMutationRequested &&
        !EcStartRequested &&
        !RuntimeActionRequested &&
        !LispEvaluationRequested &&
        !LispMorphologyPromotionRequested &&
        !DatabaseWriteRequested &&
        !KnobMutationRequested;
}

public sealed record NonActivationReceipt(
    string ReceiptHandle,
    string InertnessGate,
    string CarrierRef,
    NonActivationState State,
    IReadOnlyList<string> WitnessRefs)
{
    public bool HasPrematureActivation =>
        State is null || !State.IsInert;

    public bool HasSameInertnessAs(NonActivationReceipt? other) =>
        other is not null &&
        State is not null &&
        other.State is not null &&
        string.Equals(InertnessGate, other.InertnessGate, StringComparison.Ordinal) &&
        State == other.State &&
        WitnessRefs.SequenceEqual(other.WitnessRefs);
}

public static class NonActivationReceipts
{
    public static NonActivationReceipt FromCarrier(
        string inertnessGate,
        string carrierRef,
        IReadOnlyList<string> witnessRefs)
    {
        if (string.IsNullOrWhiteSpace(inertnessGate) || string.IsNullOrWhiteSpace(carrierRef))
        {
            throw new InvalidOperationException("non-activation receipt requires a gate and carrier reference.");
        }

        var witnesses = witnessRefs?.ToArray() ?? [];
        var state = new NonActivationState(
            PayloadOpened: false,
            ModelBindingRequested: false,
            RuntimeIdentityRequested: false,
            StateMutationRequested: false,
            EcStartRequested: false,
            RuntimeActionRequested: false,
            LispEvaluationRequested: false,
            LispMorphologyPromotionRequested: false,
            DatabaseWriteRequested: false,
            KnobMutationRequested: false);

        return new NonActivationReceipt(
            ReceiptHandle: $"non-activation://{Math.Abs(HashCode.Combine(inertnessGate, carrierRef)):x}",
            InertnessGate: inertnessGate,
            CarrierRef: carrierRef,
            State: state,
            WitnessRefs: witnesses);
    }
}

public sealed record ReceiptContinuityRef(
    string RefKind,
    string RefHandle,
    string CarrierRef,
    string ContinuityGate,
    bool PayloadOpened,
    bool RuntimeIdentityEmitted,
    bool RuntimeActionExecuted,
    bool SilentRepairAttempted,
    bool ReceiptSubstitutionDetected,
    bool ReceiptCollapseDetected,
    bool ReceiptUpgradeAttempted,
    bool ForgedReceiptDetected,
    IReadOnlyList<string> WitnessRefs)
{
    public bool HasForbiddenActivation =>
        PayloadOpened ||
        RuntimeIdentityEmitted ||
        RuntimeActionExecuted ||
        SilentRepairAttempted ||
        ReceiptSubstitutionDetected ||
        ReceiptCollapseDetected ||
        ReceiptUpgradeAttempted ||
        ForgedReceiptDetected;

    public bool IsSameRefAs(ReceiptContinuityRef? other) =>
        other is not null &&
        string.Equals(RefKind, other.RefKind, StringComparison.Ordinal) &&
        string.Equals(RefHandle, other.RefHandle, StringComparison.Ordinal) &&
        string.Equals(CarrierRef, other.CarrierRef, StringComparison.Ordinal) &&
        string.Equals(ContinuityGate, other.ContinuityGate, StringComparison.Ordinal) &&
        PayloadOpened == other.PayloadOpened &&
        RuntimeIdentityEmitted == other.RuntimeIdentityEmitted &&
        RuntimeActionExecuted == other.RuntimeActionExecuted &&
        SilentRepairAttempted == other.SilentRepairAttempted &&
        ReceiptSubstitutionDetected == other.ReceiptSubstitutionDetected &&
        ReceiptCollapseDetected == other.ReceiptCollapseDetected &&
        ReceiptUpgradeAttempted == other.ReceiptUpgradeAttempted &&
        ForgedReceiptDetected == other.ForgedReceiptDetected &&
        WitnessRefs.SequenceEqual(other.WitnessRefs);
}

public sealed record ReceiptContinuityChain(
    string ChainHandle,
    IReadOnlyList<ReceiptContinuityRef> PassageRefs)
{
    public bool HasForbiddenActivation =>
        PassageRefs is null ||
        PassageRefs.Count == 0 ||
        PassageRefs.Any(static passageRef => passageRef is null || passageRef.HasForbiddenActivation);

    public bool ContainsRef(string refKind, string refHandle) =>
        PassageRefs.Any(passageRef =>
            string.Equals(passageRef.RefKind, refKind, StringComparison.Ordinal) &&
            string.Equals(passageRef.RefHandle, refHandle, StringComparison.Ordinal));

    public bool HasOrderedPrefix(ReceiptContinuityChain? prefix)
    {
        if (prefix?.PassageRefs is null || PassageRefs is null)
        {
            return false;
        }

        if (prefix.PassageRefs.Count > PassageRefs.Count)
        {
            return false;
        }

        for (var index = 0; index < prefix.PassageRefs.Count; index += 1)
        {
            if (!PassageRefs[index].IsSameRefAs(prefix.PassageRefs[index]))
            {
                return false;
            }
        }

        return true;
    }
}

public sealed record ReceiptContinuityReceipt(
    string ReceiptHandle,
    ReceiptContinuityChain Chain,
    AnchorContinuityReceipt AnchorContinuityReceipt,
    NonActivationReceipt NonActivationReceipt,
    string ContinuityGate,
    bool SilentRepairAttempted,
    bool ReceiptSubstitutionDetected,
    bool ReceiptCollapseDetected,
    bool ReceiptUpgradeAttempted,
    bool ForgedReceiptDetected,
    IReadOnlyList<string> WitnessRefs)
{
    public bool HasForbiddenActivation =>
        Chain is null ||
        Chain.HasForbiddenActivation ||
        AnchorContinuityReceipt is null ||
        AnchorContinuityReceipt.HasForbiddenActivation ||
        NonActivationReceipt is null ||
        NonActivationReceipt.HasPrematureActivation ||
        SilentRepairAttempted ||
        ReceiptSubstitutionDetected ||
        ReceiptCollapseDetected ||
        ReceiptUpgradeAttempted ||
        ForgedReceiptDetected;

    public bool HasSameAnchorAs(AnchorContinuityReceipt? other) =>
        AnchorContinuityReceipt.HasSameAnchorAs(other);

    public bool HasSameInertnessAs(NonActivationReceipt? other) =>
        NonActivationReceipt.HasSameInertnessAs(other);

    public bool HasSameReceiptRootsAs(ReceiptContinuityReceipt? other) =>
        other is not null &&
        HasSameAnchorAs(other.AnchorContinuityReceipt) &&
        HasSameInertnessAs(other.NonActivationReceipt);

    public bool ExtendsReceipt(ReceiptContinuityReceipt? priorReceipt) =>
        priorReceipt is not null &&
        HasSameReceiptRootsAs(priorReceipt) &&
        Chain.HasOrderedPrefix(priorReceipt.Chain);

    public bool ContainsPassageRef(string refKind, string refHandle) =>
        Chain.ContainsRef(refKind, refHandle);
}

public static class ReceiptContinuityReceipts
{
    public static ReceiptContinuityReceipt FromPacket(
        string packetHandle,
        AnchorContinuityReceipt anchorContinuityReceipt,
        NonActivationReceipt nonActivationReceipt,
        string continuityGate,
        IReadOnlyList<string> witnessRefs) =>
        FromRefs(
            [CreateRef("engram-packet", packetHandle, packetHandle, continuityGate, witnessRefs)],
            anchorContinuityReceipt,
            nonActivationReceipt,
            continuityGate,
            witnessRefs);

    public static ReceiptContinuityReceipt Extend(
        ReceiptContinuityReceipt priorReceipt,
        string refKind,
        string refHandle,
        string carrierRef,
        string continuityGate,
        AnchorContinuityReceipt anchorContinuityReceipt,
        NonActivationReceipt nonActivationReceipt,
        IReadOnlyList<string> witnessRefs)
    {
        ArgumentNullException.ThrowIfNull(priorReceipt);

        var refs = priorReceipt.Chain.PassageRefs
            .Concat([CreateRef(refKind, refHandle, carrierRef, continuityGate, witnessRefs)])
            .ToArray();

        return FromRefs(refs, anchorContinuityReceipt, nonActivationReceipt, continuityGate, witnessRefs);
    }

    private static ReceiptContinuityReceipt FromRefs(
        IReadOnlyList<ReceiptContinuityRef> passageRefs,
        AnchorContinuityReceipt anchorContinuityReceipt,
        NonActivationReceipt nonActivationReceipt,
        string continuityGate,
        IReadOnlyList<string> witnessRefs)
    {
        ArgumentNullException.ThrowIfNull(anchorContinuityReceipt);
        ArgumentNullException.ThrowIfNull(nonActivationReceipt);
        ArgumentNullException.ThrowIfNull(passageRefs);

        if (passageRefs.Count == 0)
        {
            throw new InvalidOperationException("receipt continuity requires at least one passage reference.");
        }

        if (string.IsNullOrWhiteSpace(continuityGate))
        {
            throw new InvalidOperationException("receipt continuity requires a gate.");
        }

        var witnesses = witnessRefs?.ToArray() ?? [];
        var chainMaterial = string.Join("|", passageRefs.Select(static passageRef => $"{passageRef.RefKind}:{passageRef.RefHandle}"));
        var chain = new ReceiptContinuityChain(
            ChainHandle: $"receipt-continuity-chain://{Math.Abs(HashCode.Combine(chainMaterial, continuityGate)):x}",
            PassageRefs: passageRefs.ToArray());

        return new ReceiptContinuityReceipt(
            ReceiptHandle: $"receipt-continuity://{Math.Abs(HashCode.Combine(chain.ChainHandle, anchorContinuityReceipt.ReceiptHandle, nonActivationReceipt.ReceiptHandle)):x}",
            Chain: chain,
            AnchorContinuityReceipt: anchorContinuityReceipt,
            NonActivationReceipt: nonActivationReceipt,
            ContinuityGate: continuityGate,
            SilentRepairAttempted: false,
            ReceiptSubstitutionDetected: false,
            ReceiptCollapseDetected: false,
            ReceiptUpgradeAttempted: false,
            ForgedReceiptDetected: false,
            WitnessRefs: witnesses);
    }

    private static ReceiptContinuityRef CreateRef(
        string refKind,
        string refHandle,
        string carrierRef,
        string continuityGate,
        IReadOnlyList<string> witnessRefs)
    {
        if (string.IsNullOrWhiteSpace(refKind) ||
            string.IsNullOrWhiteSpace(refHandle) ||
            string.IsNullOrWhiteSpace(carrierRef) ||
            string.IsNullOrWhiteSpace(continuityGate))
        {
            throw new InvalidOperationException("receipt continuity reference requires kind, handle, carrier, and gate.");
        }

        return new ReceiptContinuityRef(
            RefKind: refKind,
            RefHandle: refHandle,
            CarrierRef: carrierRef,
            ContinuityGate: continuityGate,
            PayloadOpened: false,
            RuntimeIdentityEmitted: false,
            RuntimeActionExecuted: false,
            SilentRepairAttempted: false,
            ReceiptSubstitutionDetected: false,
            ReceiptCollapseDetected: false,
            ReceiptUpgradeAttempted: false,
            ForgedReceiptDetected: false,
            WitnessRefs: witnessRefs?.ToArray() ?? []);
    }
}

public static class AnchorContinuityReceipts
{
    public static AnchorContinuityReceipt FromRootReference(
        RootAtlasSymbolicReference rootReference,
        string continuityGate,
        string carrierRef)
    {
        ArgumentNullException.ThrowIfNull(rootReference);

        if (string.IsNullOrWhiteSpace(rootReference.ReferenceHandle))
        {
            throw new InvalidOperationException("anchor reference requires a source handle.");
        }

        if (rootReference.SemanticPayloadOpened)
        {
            throw new InvalidOperationException("anchor reference may not carry an opened semantic payload.");
        }

        if (rootReference.MutationAllowed)
        {
            throw new InvalidOperationException("anchor reference may not permit source mutation.");
        }

        if (string.IsNullOrWhiteSpace(continuityGate) || string.IsNullOrWhiteSpace(carrierRef))
        {
            throw new InvalidOperationException("anchor continuity requires a gate and carrier reference.");
        }

        var witnessRefs = rootReference.WitnessRefs.ToArray();
        var anchor = new AnchorReference(
            AnchorHandle: $"anchor://{rootReference.ReferenceHandle.Trim()}",
            SourceReferenceHandle: rootReference.ReferenceHandle,
            SourceLineageRef: rootReference.AtlasLineageRef,
            SourceGate: rootReference.SourcePosture,
            PayloadOpened: rootReference.SemanticPayloadOpened,
            MutationAllowed: rootReference.MutationAllowed,
            RuntimeIdentityEmissionAllowed: false,
            WitnessRefs: witnessRefs);

        return new AnchorContinuityReceipt(
            ReceiptHandle: $"anchor-continuity://{Math.Abs(HashCode.Combine(anchor.AnchorHandle, continuityGate, carrierRef)):x}",
            Anchor: anchor,
            ContinuityGate: continuityGate,
            CarrierRef: carrierRef,
            PayloadCarried: false,
            RuntimeIdentityEmitted: false,
            DoctrineAdmitted: false,
            WitnessRefs: witnessRefs);
    }
}

public sealed record RootAtlasSymbolicReference(
    string ReferenceHandle,
    string AtlasLineageRef,
    string SymbolicEntryKey,
    string SourcePosture,
    bool SemanticPayloadOpened,
    bool MutationAllowed,
    IReadOnlyList<string> WitnessRefs);

public sealed record EngramPacket(
    string PacketHandle,
    string RootReferenceHandle,
    AnchorContinuityReceipt AnchorContinuityReceipt,
    NonActivationReceipt NonActivationReceipt,
    ReceiptContinuityReceipt ReceiptContinuityReceipt,
    string TrunkContractRef,
    string BranchContractRef,
    string PredicateFamily,
    IReadOnlyList<string> SymbolicSegments,
    bool RawGelPromoted,
    bool RuntimeIdentityEmissionAllowed,
    IReadOnlyList<string> WitnessRefs);

public sealed record CMosCertificationReceipt(
    string ReceiptHandle,
    string EngramPacketHandle,
    AnchorContinuityReceipt AnchorContinuityReceipt,
    NonActivationReceipt NonActivationReceipt,
    ReceiptContinuityReceipt ReceiptContinuityReceipt,
    string IssuedRtmeHandle,
    string CertificationPosture,
    bool RuntimeIdentityEmissionAllowed,
    IReadOnlyList<string> WitnessRefs,
    DateTimeOffset TimestampUtc);

public sealed record SliAdmissionReceipt(
    string ReceiptHandle,
    SliCmeActualRoundtripDisposition Disposition,
    string OutcomeCode,
    string GovernanceTrace,
    string EngramPacketHandle,
    string CertificationReceiptHandle,
    AnchorContinuityReceipt AnchorContinuityReceipt,
    NonActivationReceipt NonActivationReceipt,
    ReceiptContinuityReceipt ReceiptContinuityReceipt,
    bool RuntimeIdentityEmissionAllowed,
    DateTimeOffset TimestampUtc);

public sealed record RegionalSanctuaryIdentityContract(
    string ContractHandle,
    string RegionalNamespace,
    string SanctuaryIdentityHandle,
    string PrimeCrypticBindingHandle,
    bool RuntimeIdentityEmissionAllowed,
    bool PrimeCrypticRootMintedByServiceExtension,
    IReadOnlyList<string> WitnessRefs);

public sealed record CmeActualInstanceContract(
    string ContractHandle,
    string RegionalSanctuaryIdentityHandle,
    string SliAdmissionReceiptHandle,
    AnchorContinuityReceipt AnchorContinuityReceipt,
    NonActivationReceipt NonActivationReceipt,
    ReceiptContinuityReceipt ReceiptContinuityReceipt,
    SliCmeActualRoundtripDisposition Disposition,
    bool RuntimeIdentityEmitted,
    IReadOnlyList<string> WitnessRefs);

public sealed record StewardExtensionContract(
    string ExtensionHandle,
    string ParentSanctuaryActualId,
    string StewardDomainName,
    string ExtensionPosture,
    bool MintsPrimeCrypticRoot,
    IReadOnlyList<string> WitnessRefs);

public sealed record CradleTekDomainStewardContract(
    string DomainHandle,
    string ParentSanctuaryActualId,
    string DomainName,
    string StewardExtensionHandle,
    bool MintsPrimeCrypticRoot,
    IReadOnlyList<string> WitnessRefs);
