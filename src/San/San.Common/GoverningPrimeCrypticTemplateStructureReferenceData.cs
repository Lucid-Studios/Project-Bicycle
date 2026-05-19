namespace San.Common;

public static class GoverningPrimeCrypticTemplateStructureReferenceData
{
    public static IReadOnlyList<GoverningPrimeCrypticTemplateOffice> RequiredOffices { get; } =
        Enum.GetValues<GoverningPrimeCrypticTemplateOffice>();

    public static IReadOnlyList<GoverningPrimeCrypticUserDataPredicateKind> DefaultUserDataPredicateKinds { get; } =
        Enum.GetValues<GoverningPrimeCrypticUserDataPredicateKind>();

    public static IReadOnlyList<GoverningPrimeCrypticTemplateDeniedCapability> DefaultDeniedCapabilities { get; } =
        Enum.GetValues<GoverningPrimeCrypticTemplateDeniedCapability>();

    public static GoverningPrimeCrypticTemplateRecord ReadyTemplateStructure { get; } = new(
        SourceMosCmosSeedSubstrateRef: SanctuaryGelMosCmosSeedSubstrateReferenceData.ReadyReceipt.ReceiptHandle,
        Disposition: GoverningPrimeCrypticTemplateDisposition.Ready,
        Offices: RequiredOffices,
        UserDataPredicateKinds: DefaultUserDataPredicateKinds,
        DeniedCapabilities: DefaultDeniedCapabilities,
        RefusalReasons: new[] { GoverningPrimeCrypticTemplateRefusalReason.None },
        GoverningPrimeTemplatePosture: "Governing.Prime template posture is readable witness and telemetry candidate posture only.",
        GoverningCrypticTemplatePosture: "Governing.Cryptic template posture is cryptic binder and handshake candidate posture only.",
        PairedPrimeCrypticReceiptPosture: "Paired Prime/Cryptic template receipt is candidate pairing only and not active cryptography.",
        UserDataPredicatePosture: "User-data predicate classes shape future governed questions only and do not collect user data.",
        NonAuthoritySummary: "Ready Governing.Prime/Cryptic template structure may shape future governed user-data predicate questions. Data collection, consent capture, surveillance, profiling, training, research use, provider sync, cryptographic authority, encryption runtime, Prime/Cryptic mutation, governing CME activation, SLI.Lisp execution, RTME movement, and runtime control remain denied.",
        WitnessRefs: new[]
        {
            "governing-prime-cryptic-template-structure-ref://ready",
            SanctuaryGelMosCmosSeedSubstrateReferenceData.ReadyReceipt.ReceiptHandle
        });

    public static GoverningPrimeCrypticTemplateRecord HeldForLocalCounselOrDataReview { get; } = ReadyTemplateStructure with
    {
        SourceMosCmosSeedSubstrateRef = SanctuaryGelMosCmosSeedSubstrateReferenceData.HeldReceipt.ReceiptHandle,
        Disposition = GoverningPrimeCrypticTemplateDisposition.Held,
        RefusalReasons = new[] { GoverningPrimeCrypticTemplateRefusalReason.None },
        GoverningPrimeTemplatePosture = "Governing.Prime template posture remains held for local, counsel, telemetry, or data review.",
        GoverningCrypticTemplatePosture = "Governing.Cryptic template posture remains held for binder, handshake, or data review.",
        PairedPrimeCrypticReceiptPosture = "Paired Prime/Cryptic template receipt remains held and non-cryptographic.",
        UserDataPredicatePosture = "User-data predicate classes remain held; no user-data collection is admitted.",
        NonAuthoritySummary = "Held Governing.Prime/Cryptic template structure keeps local, counsel, data, binder, handshake, telemetry, or predicate questions held while every denied capability remains denied.",
        WitnessRefs = new[]
        {
            "governing-prime-cryptic-template-structure-ref://held-local-counsel-or-data-review",
            SanctuaryGelMosCmosSeedSubstrateReferenceData.HeldReceipt.ReceiptHandle
        }
    };

    public static GoverningPrimeCrypticTemplateRecord RefusedDataCollectionOrAuthorityOverclaim { get; } = new(
        SourceMosCmosSeedSubstrateRef: "missing-or-overclaimed",
        Disposition: GoverningPrimeCrypticTemplateDisposition.Refused,
        Offices: Array.Empty<GoverningPrimeCrypticTemplateOffice>(),
        UserDataPredicateKinds: Array.Empty<GoverningPrimeCrypticUserDataPredicateKind>(),
        DeniedCapabilities: DefaultDeniedCapabilities,
        RefusalReasons: new[]
        {
            GoverningPrimeCrypticTemplateRefusalReason.MissingMosCmosSeedSubstrate,
            GoverningPrimeCrypticTemplateRefusalReason.MissingGoverningPrimeTemplate,
            GoverningPrimeCrypticTemplateRefusalReason.MissingGoverningCrypticTemplate,
            GoverningPrimeCrypticTemplateRefusalReason.MissingPairedPrimeCrypticReceipt,
            GoverningPrimeCrypticTemplateRefusalReason.MissingUserDataPredicatePosture,
            GoverningPrimeCrypticTemplateRefusalReason.DataCollectionOverclaimed,
            GoverningPrimeCrypticTemplateRefusalReason.ConsentCaptureOverclaimed,
            GoverningPrimeCrypticTemplateRefusalReason.SurveillanceOverclaimed,
            GoverningPrimeCrypticTemplateRefusalReason.ProfilingOverclaimed,
            GoverningPrimeCrypticTemplateRefusalReason.TrainingOverclaimed,
            GoverningPrimeCrypticTemplateRefusalReason.ResearchUseOverclaimed,
            GoverningPrimeCrypticTemplateRefusalReason.ProviderSyncOverclaimed,
            GoverningPrimeCrypticTemplateRefusalReason.CryptographicAuthorityOverclaimed,
            GoverningPrimeCrypticTemplateRefusalReason.MutationOverclaimed,
            GoverningPrimeCrypticTemplateRefusalReason.SliLispExecutionOverclaimed,
            GoverningPrimeCrypticTemplateRefusalReason.RtmeMovementOverclaimed,
            GoverningPrimeCrypticTemplateRefusalReason.GovernanceOrRuntimeOverclaimed
        },
        GoverningPrimeTemplatePosture: "refused",
        GoverningCrypticTemplatePosture: "refused",
        PairedPrimeCrypticReceiptPosture: "refused",
        UserDataPredicatePosture: "refused",
        NonAuthoritySummary: "Refused Governing.Prime/Cryptic template structure catches missing template offices and overclaims of consent, data collection, surveillance, profiling, training, research use, provider sync, cryptographic authority, Prime/Cryptic mutation, SLI.Lisp execution, RTME movement, governance, or runtime.",
        WitnessRefs: new[]
        {
            "governing-prime-cryptic-template-structure-ref://refused-data-collection-or-authority-overclaim"
        });

    public static IReadOnlyList<GoverningPrimeCrypticTemplateRecord> CanonicalRecords { get; } = new[]
    {
        ReadyTemplateStructure,
        HeldForLocalCounselOrDataReview,
        RefusedDataCollectionOrAuthorityOverclaim
    };

    public static GoverningPrimeCrypticTemplateReceipt ReadyReceipt { get; } = Receipt(
        "governing-prime-cryptic-template-structure-receipt://ready",
        ReadyTemplateStructure);

    public static GoverningPrimeCrypticTemplateReceipt HeldReceipt { get; } = Receipt(
        "governing-prime-cryptic-template-structure-receipt://held-local-counsel-or-data-review",
        HeldForLocalCounselOrDataReview);

    public static GoverningPrimeCrypticTemplateReceipt RefusedReceipt { get; } = Receipt(
        "governing-prime-cryptic-template-structure-receipt://refused-data-collection-or-authority-overclaim",
        RefusedDataCollectionOrAuthorityOverclaim);

    private static GoverningPrimeCrypticTemplateReceipt Receipt(
        string receiptHandle,
        GoverningPrimeCrypticTemplateRecord record)
    {
        return new(
            ReceiptHandle: receiptHandle,
            Disposition: record.Disposition,
            Summary: record.NonAuthoritySummary,
            WitnessRefs: record.WitnessRefs,
            TimestampUtc: DateTimeOffset.UnixEpoch);
    }
}
