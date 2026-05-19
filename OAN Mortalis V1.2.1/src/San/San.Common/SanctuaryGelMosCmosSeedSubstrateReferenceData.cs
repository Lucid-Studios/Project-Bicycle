namespace San.Common;

public static class SanctuaryGelMosCmosSeedSubstrateReferenceData
{
    public static IReadOnlyList<SanctuaryGelMosCmosSeedSubstrateLane> RequiredLanes { get; } =
        Enum.GetValues<SanctuaryGelMosCmosSeedSubstrateLane>();

    public static IReadOnlyList<SanctuaryGelMosCmosSeedSubstrateDeniedPower> DefaultDeniedPowers { get; } =
        Enum.GetValues<SanctuaryGelMosCmosSeedSubstrateDeniedPower>();

    public static SanctuaryGelMosCmosSeedSubstrateRecord ReadySeedSubstrate { get; } = new(
        SourceSanctuaryGelSubstrateRef: SanctuaryGelRegionalSubstrateFormationReferenceData.ReadyRegionalSubstrate.Identity.SubstrateHandle,
        Disposition: SanctuaryGelMosCmosSeedSubstrateDisposition.Ready,
        Lanes: RequiredLanes,
        DeniedPowers: DefaultDeniedPowers,
        RefusalReasons: new[] { SanctuaryGelMosCmosSeedSubstrateRefusalReason.None },
        PrimeMosSeedTelemetryPosture: "Sanctuary.MoS seed telemetry posture is readable as future Prime-side seed substrate only.",
        CrypticCmosBinderPosture: "Sanctuary.cMoS cryptic binder posture is readable as future paired binder posture only.",
        PairedBinderSplinePosture: "Prime/Cryptic seed substrate is paired as a binder spline without key issuance or encryption runtime.",
        NexusReadableModulationPosture: "Nexus.Control-readable modulation posture is readable only and remains non-executable.",
        NonAuthoritySummary: "Ready Sanctuary.GEL-to-MoS/cMoS seed substrate may predicate paired Sanctuary.MoS and Sanctuary.cMoS seed posture for future CME OE/SelfGEL formation. Governing CME, cryptographic key issuance, encryption runtime, Prime mutation, hidden Cryptic mutation, Nexus execution, SLI.Lisp execution, runtime control, and CME formation remain denied.",
        WitnessRefs: new[]
        {
            "sanctuary-gel-mos-cmos-seed-substrate-ref://ready",
            SanctuaryGelRegionalSubstrateFormationReferenceData.ReadyRegionalSubstrate.Identity.SubstrateHandle
        });

    public static SanctuaryGelMosCmosSeedSubstrateRecord HeldForBinderOrTelemetryReview { get; } = ReadySeedSubstrate with
    {
        SourceSanctuaryGelSubstrateRef = SanctuaryGelRegionalSubstrateFormationReferenceData.HeldForRegionalOrGovernanceReview.Identity.SubstrateHandle,
        Disposition = SanctuaryGelMosCmosSeedSubstrateDisposition.Held,
        RefusalReasons = new[] { SanctuaryGelMosCmosSeedSubstrateRefusalReason.None },
        PrimeMosSeedTelemetryPosture = "Sanctuary.MoS seed telemetry posture remains held for binder, regional, or telemetry review.",
        CrypticCmosBinderPosture = "Sanctuary.cMoS cryptic binder posture remains held and does not issue cryptographic authority.",
        PairedBinderSplinePosture = "Paired Prime/Cryptic binder spline remains held without mutation or runtime control.",
        NexusReadableModulationPosture = "Nexus.Control-readable modulation posture remains held and non-executable.",
        NonAuthoritySummary = "Held Sanctuary.GEL-to-MoS/cMoS seed substrate keeps binder, telemetry, regional, or modulation questions held while every denied power remains denied.",
        WitnessRefs = new[]
        {
            "sanctuary-gel-mos-cmos-seed-substrate-ref://held-binder-or-telemetry-review",
            SanctuaryGelRegionalSubstrateFormationReferenceData.HeldForRegionalOrGovernanceReview.Identity.SubstrateHandle
        }
    };

    public static SanctuaryGelMosCmosSeedSubstrateRecord RefusedSeedSubstrateOverclaim { get; } = new(
        SourceSanctuaryGelSubstrateRef: "missing-or-overclaimed",
        Disposition: SanctuaryGelMosCmosSeedSubstrateDisposition.Refused,
        Lanes: Array.Empty<SanctuaryGelMosCmosSeedSubstrateLane>(),
        DeniedPowers: DefaultDeniedPowers,
        RefusalReasons: new[]
        {
            SanctuaryGelMosCmosSeedSubstrateRefusalReason.MissingSanctuaryGelSubstrate,
            SanctuaryGelMosCmosSeedSubstrateRefusalReason.MissingPrimeMosSeed,
            SanctuaryGelMosCmosSeedSubstrateRefusalReason.MissingCrypticCmosSeed,
            SanctuaryGelMosCmosSeedSubstrateRefusalReason.MissingPairedBinderSpline,
            SanctuaryGelMosCmosSeedSubstrateRefusalReason.MissingNexusReadableModulation,
            SanctuaryGelMosCmosSeedSubstrateRefusalReason.GoverningCmeOverclaimed,
            SanctuaryGelMosCmosSeedSubstrateRefusalReason.CryptographicAuthorityOverclaimed,
            SanctuaryGelMosCmosSeedSubstrateRefusalReason.EncryptionRuntimeOverclaimed,
            SanctuaryGelMosCmosSeedSubstrateRefusalReason.PrimeMutationOverclaimed,
            SanctuaryGelMosCmosSeedSubstrateRefusalReason.HiddenCrypticMutationOverclaimed,
            SanctuaryGelMosCmosSeedSubstrateRefusalReason.NexusExecutionOverclaimed,
            SanctuaryGelMosCmosSeedSubstrateRefusalReason.SliLispExecutionOverclaimed,
            SanctuaryGelMosCmosSeedSubstrateRefusalReason.RuntimeControlOverclaimed,
            SanctuaryGelMosCmosSeedSubstrateRefusalReason.CmeFormationOverclaimed
        },
        PrimeMosSeedTelemetryPosture: "refused",
        CrypticCmosBinderPosture: "refused",
        PairedBinderSplinePosture: "refused",
        NexusReadableModulationPosture: "refused",
        NonAuthoritySummary: "Refused Sanctuary.GEL-to-MoS/cMoS seed substrate catches missing seed lanes and overclaims of governing CME, cryptographic authority, encryption runtime, Prime mutation, hidden Cryptic mutation, Nexus execution, SLI.Lisp execution, runtime control, or CME formation.",
        WitnessRefs: new[]
        {
            "sanctuary-gel-mos-cmos-seed-substrate-ref://refused-overclaim"
        });

    public static IReadOnlyList<SanctuaryGelMosCmosSeedSubstrateRecord> CanonicalRecords { get; } = new[]
    {
        ReadySeedSubstrate,
        HeldForBinderOrTelemetryReview,
        RefusedSeedSubstrateOverclaim
    };

    public static SanctuaryGelMosCmosSeedSubstrateReceipt ReadyReceipt { get; } = Receipt(
        "sanctuary-gel-mos-cmos-seed-substrate-receipt://ready",
        ReadySeedSubstrate);

    public static SanctuaryGelMosCmosSeedSubstrateReceipt HeldReceipt { get; } = Receipt(
        "sanctuary-gel-mos-cmos-seed-substrate-receipt://held-binder-or-telemetry-review",
        HeldForBinderOrTelemetryReview);

    public static SanctuaryGelMosCmosSeedSubstrateReceipt RefusedReceipt { get; } = Receipt(
        "sanctuary-gel-mos-cmos-seed-substrate-receipt://refused-overclaim",
        RefusedSeedSubstrateOverclaim);

    private static SanctuaryGelMosCmosSeedSubstrateReceipt Receipt(
        string receiptHandle,
        SanctuaryGelMosCmosSeedSubstrateRecord record)
    {
        return new(
            ReceiptHandle: receiptHandle,
            Disposition: record.Disposition,
            Summary: record.NonAuthoritySummary,
            WitnessRefs: record.WitnessRefs,
            TimestampUtc: DateTimeOffset.UnixEpoch);
    }
}
