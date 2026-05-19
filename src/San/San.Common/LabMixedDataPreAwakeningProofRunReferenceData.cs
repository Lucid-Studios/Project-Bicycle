namespace San.Common;

public static class LabMixedDataPreAwakeningProofRunReferenceData
{
    public static IReadOnlyList<LabMixedDataPreAwakeningProofStage> DefaultProofStages { get; } =
        Enum.GetValues<LabMixedDataPreAwakeningProofStage>();

    public static IReadOnlyList<LabMixedDataPreAwakeningProofReceiptKind> DefaultReceiptKinds { get; } =
        Enum.GetValues<LabMixedDataPreAwakeningProofReceiptKind>();

    public static IReadOnlyList<LabMixedDataPreAwakeningDeniedCapability> DefaultDeniedCapabilities { get; } =
        Enum.GetValues<LabMixedDataPreAwakeningDeniedCapability>();

    public static IReadOnlyList<LabMixedDataManifestEntry> MetadataOnlyManifestEntries { get; } = new[]
    {
        Entry(
            LabMixedDataManifestDatumKind.PersonalOperator,
            "lab-mixed-data-local-ref://personal-operator-metadata",
            "hash-or-ref-only-no-raw-content",
            "Personal/operator datum represented by metadata, summary, receipt posture, and witness refs only.",
            "personal-local-only"),
        Entry(
            LabMixedDataManifestDatumKind.PrivateLabBusiness,
            "lab-mixed-data-local-ref://private-lab-business-metadata",
            "hash-or-ref-only-no-raw-content",
            "Private Lab/business datum represented by metadata, summary, receipt posture, and witness refs only.",
            "private-lab-local-only"),
        Entry(
            LabMixedDataManifestDatumKind.IpAsset,
            "lab-mixed-data-local-ref://ip-asset-metadata",
            "hash-or-ref-only-no-raw-content",
            "IP/asset datum represented by metadata, summary, receipt posture, and witness refs only.",
            "asset-posture-local-only"),
        Entry(
            LabMixedDataManifestDatumKind.ConversationWitness,
            "lab-mixed-data-local-ref://conversation-witness-metadata",
            "hash-or-ref-only-no-raw-content",
            "Conversation witness datum represented by metadata, summary, receipt posture, and witness refs only.",
            "conversation-witness-local-only"),
        Entry(
            LabMixedDataManifestDatumKind.OperationalTelemetry,
            "lab-mixed-data-local-ref://operational-telemetry-metadata",
            "hash-or-ref-only-no-raw-content",
            "Operational telemetry datum represented by metadata, summary, receipt posture, and witness refs only.",
            "governed-telemetry-local-only"),
        Entry(
            LabMixedDataManifestDatumKind.SpecialCaseSensitiveHeld,
            "lab-mixed-data-local-ref://special-case-sensitive-held-metadata",
            "hash-or-ref-only-no-raw-content",
            "Special Case/sensitive held datum represented by metadata, summary, quarantine posture, and witness refs only.",
            "special-case-quarantined")
    };

    public static LabMixedDataPreAwakeningProofRunRecord HeldMetadataOnlyProofRun { get; } = new(
        SourceGoverningPrimeCrypticTemplateRef: GoverningPrimeCrypticTemplateStructureReferenceData.ReadyReceipt.ReceiptHandle,
        Disposition: LabMixedDataPreAwakeningProofDisposition.HeldForProof,
        ManifestEntries: MetadataOnlyManifestEntries,
        ProofStages: DefaultProofStages,
        ReceiptKinds: DefaultReceiptKinds,
        DeniedCapabilities: DefaultDeniedCapabilities,
        RefusalReasons: new[] { LabMixedDataPreAwakeningProofRefusalReason.None },
        PredicateContextPosture: "National, Regional, and Local predicate context refs are represented as refs only and do not grant authority.",
        PayloadClassificationPosture: "Payload classification posture is represented without collecting raw content or creating consent.",
        ConsentStartupPosture: "Consent and startup boundaries are absent; activation remains held by design.",
        SpecialCaseQuarantinePosture: "Special Case/sensitive held datum remains quarantined.",
        ActivationResultPosture: "HeldForProof is success posture; activation is held/refused by design.",
        NonMisuseSummary: "Held metadata-only proof run proves mixed Lab data can enter the pre-awakening spine as metadata, refs, hashes, summaries, receipts, and posture while raw-content exposure, provider visibility, consent creation, research use, training, profiling, surveillance, model context, RTME movement, SLI.Lisp execution, Prime/Cryptic mutation, governance, and runtime control remain denied.",
        WitnessRefs: new[]
        {
            "lab-mixed-data-pre-awakening-proof-run-ref://held-metadata-only",
            GoverningPrimeCrypticTemplateStructureReferenceData.ReadyReceipt.ReceiptHandle
        });

    public static LabMixedDataPreAwakeningProofRunRecord RefusedUntilConsentAndStartupAdmission { get; } = HeldMetadataOnlyProofRun with
    {
        Disposition = LabMixedDataPreAwakeningProofDisposition.RefusedUntilConsentAndStartupAdmission,
        RefusalReasons = new[]
        {
            LabMixedDataPreAwakeningProofRefusalReason.MissingPreActivationLegitimacyPosture,
            LabMixedDataPreAwakeningProofRefusalReason.StartupAttemptNotHeldOrRefused
        },
        ConsentStartupPosture = "Consent, disclosure, retention, and startup boundaries are not admitted.",
        ActivationResultPosture = "Activation is refused until consent and startup admission are separately represented.",
        NonMisuseSummary = "Refused proof run keeps activation refused until consent, disclosure, retention, and startup admission are separately represented. No raw content, model context, RTME movement, SLI.Lisp execution, governance, or runtime authority is admitted.",
        WitnessRefs = new[]
        {
            "lab-mixed-data-pre-awakening-proof-run-ref://refused-until-consent-and-startup-admission",
            GoverningPrimeCrypticTemplateStructureReferenceData.ReadyReceipt.ReceiptHandle
        }
    };

    public static LabMixedDataPreAwakeningProofRunRecord RefusedMisuseOverclaimReadout { get; } = new(
        SourceGoverningPrimeCrypticTemplateRef: GoverningPrimeCrypticTemplateStructureReferenceData.RefusedReceipt.ReceiptHandle,
        Disposition: LabMixedDataPreAwakeningProofDisposition.RefusedUntilConsentAndStartupAdmission,
        ManifestEntries: Array.Empty<LabMixedDataManifestEntry>(),
        ProofStages: DefaultProofStages,
        ReceiptKinds: DefaultReceiptKinds,
        DeniedCapabilities: DefaultDeniedCapabilities,
        RefusalReasons: new[]
        {
            LabMixedDataPreAwakeningProofRefusalReason.MissingLocalManifestMetadata,
            LabMixedDataPreAwakeningProofRefusalReason.MissingPredicateContextRefs,
            LabMixedDataPreAwakeningProofRefusalReason.MissingTemplateMatch,
            LabMixedDataPreAwakeningProofRefusalReason.MissingPayloadClassification,
            LabMixedDataPreAwakeningProofRefusalReason.MissingConsentRequirementReadout,
            LabMixedDataPreAwakeningProofRefusalReason.MissingRetentionOptOutReadout,
            LabMixedDataPreAwakeningProofRefusalReason.SpecialCaseNotQuarantined,
            LabMixedDataPreAwakeningProofRefusalReason.MissingLabSeedInheritanceRef,
            LabMixedDataPreAwakeningProofRefusalReason.RawContentExposureOverclaimed,
            LabMixedDataPreAwakeningProofRefusalReason.ProviderVisibilityOverclaimed,
            LabMixedDataPreAwakeningProofRefusalReason.ConsentCreationOverclaimed,
            LabMixedDataPreAwakeningProofRefusalReason.ResearchTrainingProfilingSurveillanceOverclaimed,
            LabMixedDataPreAwakeningProofRefusalReason.ModelContextOverclaimed,
            LabMixedDataPreAwakeningProofRefusalReason.RetentionOrSpecialCaseWideningOverclaimed,
            LabMixedDataPreAwakeningProofRefusalReason.RtmeOrSliLispOverclaimed,
            LabMixedDataPreAwakeningProofRefusalReason.PrimeCrypticMutationOverclaimed,
            LabMixedDataPreAwakeningProofRefusalReason.GovernanceOrRuntimeOverclaimed
        },
        PredicateContextPosture: "refused",
        PayloadClassificationPosture: "refused",
        ConsentStartupPosture: "refused",
        SpecialCaseQuarantinePosture: "refused",
        ActivationResultPosture: "refused",
        NonMisuseSummary: "RefusedUntilConsentAndStartupAdmission catches misuse overclaims including raw-content exposure, provider visibility, consent creation, research use, training, profiling, surveillance, model context, retention or Special Case widening, RTME movement, SLI.Lisp execution, Prime/Cryptic mutation, governance, and runtime control. No third proof result state is admitted.",
        WitnessRefs: new[]
        {
            "lab-mixed-data-pre-awakening-proof-run-ref://refused-misuse-overclaim",
            GoverningPrimeCrypticTemplateStructureReferenceData.RefusedReceipt.ReceiptHandle
        });

    public static IReadOnlyList<LabMixedDataPreAwakeningProofRunRecord> CanonicalRecords { get; } = new[]
    {
        HeldMetadataOnlyProofRun,
        RefusedUntilConsentAndStartupAdmission,
        RefusedMisuseOverclaimReadout
    };

    public static LabMixedDataPreAwakeningProofRunReceipt HeldReceipt { get; } = Receipt(
        "lab-mixed-data-pre-awakening-proof-run-receipt://held-metadata-only",
        HeldMetadataOnlyProofRun);

    public static LabMixedDataPreAwakeningProofRunReceipt RefusedUntilConsentReceipt { get; } = Receipt(
        "lab-mixed-data-pre-awakening-proof-run-receipt://refused-until-consent-and-startup-admission",
        RefusedUntilConsentAndStartupAdmission);

    public static LabMixedDataPreAwakeningProofRunReceipt RefusedMisuseReceipt { get; } = Receipt(
        "lab-mixed-data-pre-awakening-proof-run-receipt://refused-misuse-overclaim",
        RefusedMisuseOverclaimReadout);

    private static LabMixedDataManifestEntry Entry(
        LabMixedDataManifestDatumKind kind,
        string logicalLocalRef,
        string hashOrRefPosture,
        string summary,
        string sensitivityPosture)
    {
        return new(
            Kind: kind,
            LogicalLocalRef: logicalLocalRef,
            HashOrRefPosture: hashOrRefPosture,
            Summary: summary,
            SensitivityPosture: sensitivityPosture,
            WitnessRefs: new[]
            {
                logicalLocalRef
            });
    }

    private static LabMixedDataPreAwakeningProofRunReceipt Receipt(
        string receiptHandle,
        LabMixedDataPreAwakeningProofRunRecord record)
    {
        return new(
            ReceiptHandle: receiptHandle,
            Disposition: record.Disposition,
            Summary: record.NonMisuseSummary,
            WitnessRefs: record.WitnessRefs,
            TimestampUtc: DateTimeOffset.UnixEpoch);
    }
}
