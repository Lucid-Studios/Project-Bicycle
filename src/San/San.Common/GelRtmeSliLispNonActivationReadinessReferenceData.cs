namespace San.Common;

public static class GelRtmeSliLispNonActivationReadinessReferenceData
{
    public static IReadOnlyList<GelRtmeSliLispDeniedCapability> DefaultDeniedCapabilities { get; } =
        Enum.GetValues<GelRtmeSliLispDeniedCapability>();

    public static GelRtmeSliLispNonActivationReadinessRecord ReadyNonActivationReadiness { get; } = new(
        SourceSanctuaryGelStandingRef: SanctuaryGelRegionalSubstrateFormationReferenceData.ReadyRegionalSubstrate.Identity.SubstrateHandle,
        SourceWitnessStoreRef: SanctuaryLocalConversationWitnessStoreReferenceData.ReadyReceipt.ReceiptHandle,
        Disposition: GelRtmeSliLispReadinessDisposition.Ready,
        ReadinessPosture: GelRtmeSliLispReadinessPosture.CandidateReadable,
        DeniedCapabilities: DefaultDeniedCapabilities,
        RefusalReasons: new[] { GelRtmeSliLispReadinessRefusalReason.None },
        NonActivationSummary: "Ready non-activation readiness means future movement candidacy is readable only. rtme_active=false; runtime_transaction_movement_allowed=false; always_on_authority_granted=false; direct_persistence_allowed=false; direct_prime_mutation_allowed=false. Nothing may move.",
        WitnessRefs: new[]
        {
            "gel-rtme-sli-lisp-non-activation-readiness://ready",
            SanctuaryGelRegionalSubstrateFormationReferenceData.ReadyRegionalSubstrate.Identity.SubstrateHandle,
            SanctuaryLocalConversationWitnessStoreReferenceData.ReadyReceipt.ReceiptHandle
        });

    public static GelRtmeSliLispNonActivationReadinessRecord HeldForExplicitGate { get; } = new(
        SourceSanctuaryGelStandingRef: SanctuaryGelRegionalSubstrateFormationReferenceData.HeldForRegionalOrGovernanceReview.Identity.SubstrateHandle,
        SourceWitnessStoreRef: SanctuaryLocalConversationWitnessStoreReferenceData.HeldReceipt.ReceiptHandle,
        Disposition: GelRtmeSliLispReadinessDisposition.Held,
        ReadinessPosture: GelRtmeSliLispReadinessPosture.HeldForGate,
        DeniedCapabilities: DefaultDeniedCapabilities,
        RefusalReasons: new[] { GelRtmeSliLispReadinessRefusalReason.None },
        NonActivationSummary: "Held non-activation readiness keeps explicit gate questions held while RTME activation, runtime transaction movement, always-on authority, direct persistence, direct Prime mutation, membrane bypass, SLI.Lisp execution, and survivor admission remain denied.",
        WitnessRefs: new[]
        {
            "gel-rtme-sli-lisp-non-activation-readiness://held-for-explicit-gate",
            SanctuaryGelRegionalSubstrateFormationReferenceData.HeldForRegionalOrGovernanceReview.Identity.SubstrateHandle,
            SanctuaryLocalConversationWitnessStoreReferenceData.HeldReceipt.ReceiptHandle
        });

    public static GelRtmeSliLispNonActivationReadinessRecord RefusedMovementOverclaim { get; } = new(
        SourceSanctuaryGelStandingRef: "missing-or-overclaimed",
        SourceWitnessStoreRef: SanctuaryLocalConversationWitnessStoreReferenceData.RefusedReceipt.ReceiptHandle,
        Disposition: GelRtmeSliLispReadinessDisposition.Refused,
        ReadinessPosture: GelRtmeSliLispReadinessPosture.MovementWithheld,
        DeniedCapabilities: DefaultDeniedCapabilities,
        RefusalReasons: new[]
        {
            GelRtmeSliLispReadinessRefusalReason.MissingSanctuaryGelStanding,
            GelRtmeSliLispReadinessRefusalReason.MissingWitnessStorePosture,
            GelRtmeSliLispReadinessRefusalReason.MovementOverclaimed,
            GelRtmeSliLispReadinessRefusalReason.PersistenceOverclaimed,
            GelRtmeSliLispReadinessRefusalReason.AuthorityOverclaimed,
            GelRtmeSliLispReadinessRefusalReason.SliLispExecutionOverclaimed,
            GelRtmeSliLispReadinessRefusalReason.RtmeActivationOverclaimed,
            GelRtmeSliLispReadinessRefusalReason.MembraneBypassOverclaimed,
            GelRtmeSliLispReadinessRefusalReason.SurvivorAdmissionOverclaimed
        },
        NonActivationSummary: "Refused non-activation readiness catches movement, persistence, authority, SLI.Lisp execution, RTME activation, membrane bypass, and survivor-admission overclaims. Nothing may move.",
        WitnessRefs: new[]
        {
            "gel-rtme-sli-lisp-non-activation-readiness://refused-movement-overclaim",
            SanctuaryLocalConversationWitnessStoreReferenceData.RefusedReceipt.ReceiptHandle
        });

    public static IReadOnlyList<GelRtmeSliLispNonActivationReadinessRecord> CanonicalRecords { get; } = new[]
    {
        ReadyNonActivationReadiness,
        HeldForExplicitGate,
        RefusedMovementOverclaim
    };

    public static GelRtmeSliLispNonActivationReadinessReceipt ReadyReceipt { get; } = Receipt(
        "gel-rtme-sli-lisp-non-activation-readiness-receipt://ready",
        ReadyNonActivationReadiness);

    public static GelRtmeSliLispNonActivationReadinessReceipt HeldReceipt { get; } = Receipt(
        "gel-rtme-sli-lisp-non-activation-readiness-receipt://held-for-explicit-gate",
        HeldForExplicitGate);

    public static GelRtmeSliLispNonActivationReadinessReceipt RefusedReceipt { get; } = Receipt(
        "gel-rtme-sli-lisp-non-activation-readiness-receipt://refused-movement-overclaim",
        RefusedMovementOverclaim);

    private static GelRtmeSliLispNonActivationReadinessReceipt Receipt(
        string receiptHandle,
        GelRtmeSliLispNonActivationReadinessRecord record)
    {
        return new(
            ReceiptHandle: receiptHandle,
            Disposition: record.Disposition,
            Summary: record.NonActivationSummary,
            WitnessRefs: record.WitnessRefs,
            TimestampUtc: DateTimeOffset.UnixEpoch);
    }
}
