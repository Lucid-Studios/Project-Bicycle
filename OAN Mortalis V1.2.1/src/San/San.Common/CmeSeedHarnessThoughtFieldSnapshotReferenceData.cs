namespace San.Common;

public static class CmeSeedHarnessThoughtFieldSnapshotReferenceData
{
    public static IReadOnlyList<string> FrozenThoughtFieldLadder { get; } = new[]
    {
        "documented data",
        "metadata-only proof posture",
        "governed inventory schema",
        "inventory evaluation posture",
        "claim-reading threshold",
        "first CME seed harness response lane"
    };

    public static IReadOnlyList<CmeSeedHarnessUiTemplateField> UiTemplateFields { get; } = new[]
    {
        Field(
            CmeSeedHarnessUiFieldKind.OperatorPrompt,
            "Operator prompt",
            "Future operator prompt field for the response lane.",
            "template-only-no-prompt-capture"),
        Field(
            CmeSeedHarnessUiFieldKind.SeedPostureSelector,
            "Seed posture",
            "Future selector for frozen thought-field seed posture.",
            "template-only-no-authority-selection"),
        Field(
            CmeSeedHarnessUiFieldKind.InventoryEvaluationRefs,
            "Inventory/evaluation refs",
            "Future logical refs for inventory evaluation posture.",
            "template-only-no-raw-data-ref-loading"),
        Field(
            CmeSeedHarnessUiFieldKind.ResponseMode,
            "Response mode",
            "Future response mode selection for readout, hold, or refusal posture.",
            "template-only-no-model-call"),
        Field(
            CmeSeedHarnessUiFieldKind.RefusalHoldReadoutLane,
            "Refusal/hold lane",
            "Future readout lane for refusals, holds, and non-authority summaries.",
            "template-only-no-runtime-effect")
    };

    public static IReadOnlyList<CmeSeedHarnessDeniedCapability> DefaultDeniedCapabilities { get; } =
        Enum.GetValues<CmeSeedHarnessDeniedCapability>();

    public static CmeSeedHarnessThoughtFieldSnapshotRecord FrozenSnapshot { get; } = new(
        Disposition: CmeSeedHarnessThoughtFieldSnapshotDisposition.FrozenForCodeFormation,
        SourceInventoryEvaluationReceiptRef: LabDataInventoryEvaluationPostureReferenceData.ReadableReceipt.ReceiptHandle,
        ThoughtFieldLadder: FrozenThoughtFieldLadder,
        UiTemplateFields: UiTemplateFields,
        DeniedCapabilities: DefaultDeniedCapabilities,
        RefusalReasons: new[] { CmeSeedHarnessRefusalReason.None },
        NonAuthoritySummary: "Frozen CME seed harness thought-field snapshot may seed a first callable response lane and UI template form. It denies raw Lab data ingestion, consent creation, model training, research use, provider visibility, model-context export, SLI.Lisp execution, RTME movement, Prime/Cryptic mutation, governing CME activation, Sanctuary.Actual formation, and runtime authority.",
        WitnessRefs: new[]
        {
            "cme-seed-harness-thought-field-snapshot-ref:frozen",
            LabDataInventoryEvaluationPostureReferenceData.ReadableReceipt.ReceiptHandle
        });

    public static CmeSeedHarnessThoughtFieldSnapshotRecord HeldForHarnessBuild { get; } = FrozenSnapshot with
    {
        Disposition = CmeSeedHarnessThoughtFieldSnapshotDisposition.HeldForHarnessBuild,
        SourceInventoryEvaluationReceiptRef = LabDataInventoryEvaluationPostureReferenceData.HeldReceipt.ReceiptHandle,
        RefusalReasons = new[] { CmeSeedHarnessRefusalReason.MissingInventoryEvaluationPosture },
        NonAuthoritySummary = "Held CME seed harness thought-field snapshot keeps harness build questions held while every denied capability remains denied.",
        WitnessRefs = new[]
        {
            "cme-seed-harness-thought-field-snapshot-ref:held-for-harness-build",
            LabDataInventoryEvaluationPostureReferenceData.HeldReceipt.ReceiptHandle
        }
    };

    public static CmeSeedHarnessThoughtFieldSnapshotRecord RefusedActivationOverclaim { get; } = new(
        Disposition: CmeSeedHarnessThoughtFieldSnapshotDisposition.RefusedAsActivationOverclaim,
        SourceInventoryEvaluationReceiptRef: LabDataInventoryEvaluationPostureReferenceData.RefusedReceipt.ReceiptHandle,
        ThoughtFieldLadder: FrozenThoughtFieldLadder,
        UiTemplateFields: UiTemplateFields,
        DeniedCapabilities: DefaultDeniedCapabilities,
        RefusalReasons: new[]
        {
            CmeSeedHarnessRefusalReason.RawDataIngestionOverclaimed,
            CmeSeedHarnessRefusalReason.ConsentOverclaimed,
            CmeSeedHarnessRefusalReason.LlmAuthorityOverclaimed,
            CmeSeedHarnessRefusalReason.SliLispOverclaimed,
            CmeSeedHarnessRefusalReason.RtmeOverclaimed,
            CmeSeedHarnessRefusalReason.PrimeCrypticMutationOverclaimed,
            CmeSeedHarnessRefusalReason.GoverningCmeActivationOverclaimed,
            CmeSeedHarnessRefusalReason.SanctuaryActualOverclaimed,
            CmeSeedHarnessRefusalReason.RuntimeAuthorityOverclaimed
        },
        NonAuthoritySummary: "Refused CME seed harness thought-field snapshot catches attempts to convert the first executable harness into raw data ingestion, consent, LLM authority, SLI.Lisp execution, RTME movement, Prime/Cryptic mutation, governing CME activation, Sanctuary.Actual formation, or runtime authority.",
        WitnessRefs: new[]
        {
            "cme-seed-harness-thought-field-snapshot-ref:refused-activation-overclaim",
            LabDataInventoryEvaluationPostureReferenceData.RefusedReceipt.ReceiptHandle
        });

    public static CmeSeedHarnessResponseLaneRecord SeededResponseLane { get; } = new(
        SourceThoughtFieldSnapshotRef: "cme-seed-harness-thought-field-snapshot-receipt:frozen",
        Disposition: CmeSeedHarnessResponseLaneDisposition.SeededReadoutOnly,
        ResponseMode: "deterministic-local-readout",
        InputPostureSummary: "Input posture is the frozen thought field and UI template form only; no operator prompt content or raw Lab data is collected.",
        OutputPostureSummary: "Output posture is a local readout lane only and does not claim activation, certification, survivor admission, or runtime authority.",
        DeniedCapabilities: DefaultDeniedCapabilities,
        RefusalReasons: new[] { CmeSeedHarnessRefusalReason.None },
        NonAuthoritySummary: "Seeded response lane may render the first CME harness posture as deterministic local readout only. It is not a model call, not SLI.Lisp, not RTME, not Prime/Cryptic mutation, not Sanctuary.Actual, and not runtime authority.",
        WitnessRefs: new[]
        {
            "cme-seed-harness-response-lane-ref:seeded-readout-only",
            "cme-seed-harness-thought-field-snapshot-receipt:frozen"
        });

    public static IReadOnlyList<CmeSeedHarnessThoughtFieldSnapshotRecord> CanonicalSnapshots { get; } = new[]
    {
        FrozenSnapshot,
        HeldForHarnessBuild,
        RefusedActivationOverclaim
    };

    public static CmeSeedHarnessThoughtFieldSnapshotReceipt FrozenReceipt { get; } = Receipt(
        "cme-seed-harness-thought-field-snapshot-receipt:frozen",
        FrozenSnapshot);

    public static CmeSeedHarnessThoughtFieldSnapshotReceipt HeldReceipt { get; } = Receipt(
        "cme-seed-harness-thought-field-snapshot-receipt:held-for-harness-build",
        HeldForHarnessBuild);

    public static CmeSeedHarnessThoughtFieldSnapshotReceipt RefusedReceipt { get; } = Receipt(
        "cme-seed-harness-thought-field-snapshot-receipt:refused-activation-overclaim",
        RefusedActivationOverclaim);

    private static CmeSeedHarnessUiTemplateField Field(
        CmeSeedHarnessUiFieldKind kind,
        string label,
        string summary,
        string nonCollectionPosture)
    {
        return new(
            Kind: kind,
            Label: label,
            Summary: summary,
            RequiredForTemplate: true,
            NonCollectionPosture: nonCollectionPosture);
    }

    private static CmeSeedHarnessThoughtFieldSnapshotReceipt Receipt(
        string receiptHandle,
        CmeSeedHarnessThoughtFieldSnapshotRecord record)
    {
        return new(
            ReceiptHandle: receiptHandle,
            Disposition: record.Disposition,
            Summary: record.NonAuthoritySummary,
            WitnessRefs: record.WitnessRefs,
            TimestampUtc: DateTimeOffset.UnixEpoch);
    }
}
