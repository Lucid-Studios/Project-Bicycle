namespace San.Common;

public static class SanctuaryLocalConversationWitnessStoreReferenceData
{
    public static IReadOnlyList<SanctuaryLocalConversationWitnessCapability> DefaultDeniedCapabilities { get; } =
        Enum.GetValues<SanctuaryLocalConversationWitnessCapability>();

    public static SanctuaryLocalConversationWitnessStoreRecord ReadyLocalOnlyWitnessStore { get; } = new(
        SourceEnactmentRef: FirstUseAdmissionEnactmentReferenceData.PreparedEnactmentReceipt.ReceiptHandle,
        Disposition: SanctuaryLocalConversationWitnessStoreDisposition.Ready,
        StoragePosture: SanctuaryLocalConversationWitnessStorePosture.LocalOnly,
        DefaultDeniedCapabilities: DefaultDeniedCapabilities,
        RefusalReasons: new[] { SanctuaryLocalConversationWitnessRefusalReason.None },
        RetentionPostureSummary: "Local-only UTF-8 witness and receipt retention posture is represented for site continuity only.",
        ConsentPostureSummary: "Retention posture is not research consent, training consent, provider access consent, or partner sharing consent.",
        NonFuelSummary: "The local store is not fuel. It is a governed witness body that may preserve bounded local UTF-8 witness and receipts for site continuity only; provider-visible access, research use, model training or improvement, partner or provider sharing, rehydration, GEL candidate generation, GEL survivor admission, RTME movement, and RTME activation remain denied.",
        WitnessRefs: new[]
        {
            "sanctuary-local-conversation-witness-store://ready-local-only",
            FirstUseAdmissionEnactmentReferenceData.PreparedEnactmentReceipt.ReceiptHandle
        });

    public static SanctuaryLocalConversationWitnessStoreRecord HeldQuarantineWitnessStore { get; } = new(
        SourceEnactmentRef: FirstUseAdmissionEnactmentReferenceData.HeldEnactmentReceipt.ReceiptHandle,
        Disposition: SanctuaryLocalConversationWitnessStoreDisposition.Held,
        StoragePosture: SanctuaryLocalConversationWitnessStorePosture.Quarantined,
        DefaultDeniedCapabilities: DefaultDeniedCapabilities,
        RefusalReasons: new[] { SanctuaryLocalConversationWitnessRefusalReason.None },
        RetentionPostureSummary: "Special Case, retention, consent, quarantine, or local review questions remain held before local witness-store posture can be ready.",
        ConsentPostureSummary: "Held quarantine posture does not widen into research consent, model memory, provider-visible access, training eligibility, rehydration, GEL admission, RTME movement, or runtime authority.",
        NonFuelSummary: "Held local conversation witness posture remains quarantined and all capabilities remain denied by default.",
        WitnessRefs: new[]
        {
            "sanctuary-local-conversation-witness-store://held-quarantine",
            FirstUseAdmissionEnactmentReferenceData.HeldEnactmentReceipt.ReceiptHandle
        });

    public static SanctuaryLocalConversationWitnessStoreRecord RefusedWitnessStore { get; } = new(
        SourceEnactmentRef: FirstUseAdmissionEnactmentReferenceData.RefusedEnactmentReceipt.ReceiptHandle,
        Disposition: SanctuaryLocalConversationWitnessStoreDisposition.Refused,
        StoragePosture: SanctuaryLocalConversationWitnessStorePosture.Withheld,
        DefaultDeniedCapabilities: DefaultDeniedCapabilities,
        RefusalReasons: new[]
        {
            SanctuaryLocalConversationWitnessRefusalReason.MissingFirstUseEnactment,
            SanctuaryLocalConversationWitnessRefusalReason.MissingRetentionPosture,
            SanctuaryLocalConversationWitnessRefusalReason.ResearchConsentOverclaimed,
            SanctuaryLocalConversationWitnessRefusalReason.ProviderAccessOverclaimed,
            SanctuaryLocalConversationWitnessRefusalReason.ModelMemoryOverclaimed,
            SanctuaryLocalConversationWitnessRefusalReason.TrainingUseOverclaimed,
            SanctuaryLocalConversationWitnessRefusalReason.GelAdmissionOverclaimed,
            SanctuaryLocalConversationWitnessRefusalReason.RtmeMovementOverclaimed,
            SanctuaryLocalConversationWitnessRefusalReason.RuntimeAuthorityOverclaimed
        },
        RetentionPostureSummary: "Local witness-store posture is refused because first-use enactment or retention footing is missing or overclaimed.",
        ConsentPostureSummary: "Refused witness-store posture does not create research consent, provider access, model memory, training eligibility, GEL survivor admission, RTME movement, or runtime authority.",
        NonFuelSummary: "Refused local conversation witness posture withholds the store and denies every capability; storage does not become fuel, profile, model memory, GEL admission, RTME movement, or provider-visible access.",
        WitnessRefs: new[]
        {
            "sanctuary-local-conversation-witness-store://refused",
            FirstUseAdmissionEnactmentReferenceData.RefusedEnactmentReceipt.ReceiptHandle
        });

    public static IReadOnlyList<SanctuaryLocalConversationWitnessStoreRecord> CanonicalRecords { get; } = new[]
    {
        ReadyLocalOnlyWitnessStore,
        HeldQuarantineWitnessStore,
        RefusedWitnessStore
    };

    public static SanctuaryLocalConversationWitnessStoreReceipt ReadyReceipt { get; } = Receipt(
        "sanctuary-local-conversation-witness-store-receipt://ready-local-only",
        ReadyLocalOnlyWitnessStore);

    public static SanctuaryLocalConversationWitnessStoreReceipt HeldReceipt { get; } = Receipt(
        "sanctuary-local-conversation-witness-store-receipt://held-quarantine",
        HeldQuarantineWitnessStore);

    public static SanctuaryLocalConversationWitnessStoreReceipt RefusedReceipt { get; } = Receipt(
        "sanctuary-local-conversation-witness-store-receipt://refused",
        RefusedWitnessStore);

    private static SanctuaryLocalConversationWitnessStoreReceipt Receipt(
        string receiptHandle,
        SanctuaryLocalConversationWitnessStoreRecord record)
    {
        return new(
            ReceiptHandle: receiptHandle,
            Disposition: record.Disposition,
            Summary: record.NonFuelSummary,
            WitnessRefs: record.WitnessRefs,
            TimestampUtc: DateTimeOffset.UnixEpoch);
    }
}
