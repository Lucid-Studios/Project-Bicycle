namespace San.Common;

public static class FirstUseAdmissionEnactmentReferenceData
{
    public static FirstUseAdmissionRecord ReadyAdmission { get; } = new(
        EligibilityRef: FirstUseEligibilityReferenceData.ReadyReceipt.ReceiptHandle,
        FormationAttemptRef: SanctuaryGelFirstFormationAttemptReferenceData.ReadyReceipt.ReceiptHandle,
        Disposition: FirstUseAdmissionDisposition.Ready,
        RefusalReasons: new[] { FirstUseAdmissionRefusalReason.None },
        DisclosureDataPostureSummary: "Disclosure and local data posture are represented for bounded first-use session preparation only; no disclosure issuance, consent collection, or active legal terms are created.",
        NonAuthoritySummary: "Ready first-use admission authorizes preparation of a bounded first-use session only; it does not enact first use, activate runtime transaction authority, stand Sanctuary.Actual, invoke RTME or SLI.Lisp, select models, or generate Cradle.GEL.",
        WitnessRefs: new[]
        {
            "first-use-admission://ready",
            FirstUseEligibilityReferenceData.ReadyReceipt.ReceiptHandle,
            SanctuaryGelFirstFormationAttemptReferenceData.ReadyReceipt.ReceiptHandle
        });

    public static FirstUseAdmissionRecord HeldAdmission { get; } = new(
        EligibilityRef: FirstUseEligibilityReferenceData.HeldReceipt.ReceiptHandle,
        FormationAttemptRef: SanctuaryGelFirstFormationAttemptReferenceData.HeldReceipt.ReceiptHandle,
        Disposition: FirstUseAdmissionDisposition.Held,
        RefusalReasons: new[] { FirstUseAdmissionRefusalReason.None },
        DisclosureDataPostureSummary: "Disclosure or data posture remains held with local, domain, Special Case, counsel, or formation-attempt questions.",
        NonAuthoritySummary: "Held first-use admission preserves preparation posture while eligibility or formation-attempt questions remain held; no enactment, runtime transaction, RTME, SLI.Lisp, model selection, Sanctuary.Actual, or Cradle.GEL authority is granted.",
        WitnessRefs: new[]
        {
            "first-use-admission://held",
            FirstUseEligibilityReferenceData.HeldReceipt.ReceiptHandle,
            SanctuaryGelFirstFormationAttemptReferenceData.HeldReceipt.ReceiptHandle
        });

    public static FirstUseAdmissionRecord RefusedAdmission { get; } = new(
        EligibilityRef: FirstUseEligibilityReferenceData.RefusedReceipt.ReceiptHandle,
        FormationAttemptRef: SanctuaryGelFirstFormationAttemptReferenceData.RefusedReceipt.ReceiptHandle,
        Disposition: FirstUseAdmissionDisposition.Refused,
        RefusalReasons: new[]
        {
            FirstUseAdmissionRefusalReason.EligibilityNotReady,
            FirstUseAdmissionRefusalReason.FormationAttemptNotReady,
            FirstUseAdmissionRefusalReason.DisclosureOrDataPostureMissing,
            FirstUseAdmissionRefusalReason.RuntimeOrGovernanceOverclaimed
        },
        DisclosureDataPostureSummary: "Disclosure or data posture is missing or overclaimed; first-use admission is refused.",
        NonAuthoritySummary: "Refused first-use admission does not authorize preparation, enactment, runtime transaction authority, Sanctuary.Actual governance, RTME, SLI.Lisp execution, model selection, or Cradle.GEL generation.",
        WitnessRefs: new[]
        {
            "first-use-admission://refused",
            FirstUseEligibilityReferenceData.RefusedReceipt.ReceiptHandle,
            SanctuaryGelFirstFormationAttemptReferenceData.RefusedReceipt.ReceiptHandle
        });

    public static IReadOnlyList<FirstUseAdmissionRecord> CanonicalAdmissionRecords { get; } = new[]
    {
        ReadyAdmission,
        HeldAdmission,
        RefusedAdmission
    };

    public static FirstUseAdmissionReceipt ReadyAdmissionReceipt { get; } = AdmissionReceipt(
        "first-use-admission-receipt://ready",
        ReadyAdmission);

    public static FirstUseAdmissionReceipt HeldAdmissionReceipt { get; } = AdmissionReceipt(
        "first-use-admission-receipt://held",
        HeldAdmission);

    public static FirstUseAdmissionReceipt RefusedAdmissionReceipt { get; } = AdmissionReceipt(
        "first-use-admission-receipt://refused",
        RefusedAdmission);

    public static FirstUseEnactmentRecord PreparedEnactment { get; } = new(
        AdmissionRef: ReadyAdmissionReceipt.ReceiptHandle,
        Disposition: FirstUseEnactmentDisposition.Prepared,
        RefusalReasons: new[] { FirstUseEnactmentRefusalReason.None },
        EnactmentWitnessSummary: "A bounded first-use session entry witness is represented from ready admission.",
        NonAuthoritySummary: "Prepared first-use enactment witnesses entry posture only; it does not activate runtime transaction authority, RTME, SLI.Lisp, model selection, Sanctuary.Actual, governing CME, or Cradle.GEL generation.",
        WitnessRefs: new[]
        {
            "first-use-enactment://prepared",
            ReadyAdmissionReceipt.ReceiptHandle
        });

    public static FirstUseEnactmentRecord HeldEnactment { get; } = new(
        AdmissionRef: HeldAdmissionReceipt.ReceiptHandle,
        Disposition: FirstUseEnactmentDisposition.Held,
        RefusalReasons: new[] { FirstUseEnactmentRefusalReason.None },
        EnactmentWitnessSummary: "First-use enactment remains held because admission or witnessed entry posture remains held.",
        NonAuthoritySummary: "Held first-use enactment does not enter the session and does not activate runtime transaction authority, RTME, SLI.Lisp, model selection, Sanctuary.Actual, governing CME, or Cradle.GEL generation.",
        WitnessRefs: new[]
        {
            "first-use-enactment://held",
            HeldAdmissionReceipt.ReceiptHandle
        });

    public static FirstUseEnactmentRecord RefusedEnactment { get; } = new(
        AdmissionRef: RefusedAdmissionReceipt.ReceiptHandle,
        Disposition: FirstUseEnactmentDisposition.Refused,
        RefusalReasons: new[]
        {
            FirstUseEnactmentRefusalReason.AdmissionNotReady,
            FirstUseEnactmentRefusalReason.WitnessMissing,
            FirstUseEnactmentRefusalReason.RuntimeTransactionOverclaimed,
            FirstUseEnactmentRefusalReason.RtmeOrSliLispOverclaimed,
            FirstUseEnactmentRefusalReason.ModelSelectionOverclaimed,
            FirstUseEnactmentRefusalReason.SanctuaryActualOrCradleGelOverclaimed
        },
        EnactmentWitnessSummary: "First-use enactment witness is missing or overclaimed.",
        NonAuthoritySummary: "Refused first-use enactment does not enter the session and refuses runtime transaction authority, RTME, SLI.Lisp, model selection, Sanctuary.Actual, governing CME, or Cradle.GEL generation.",
        WitnessRefs: new[]
        {
            "first-use-enactment://refused",
            RefusedAdmissionReceipt.ReceiptHandle
        });

    public static IReadOnlyList<FirstUseEnactmentRecord> CanonicalEnactmentRecords { get; } = new[]
    {
        PreparedEnactment,
        HeldEnactment,
        RefusedEnactment
    };

    public static FirstUseEnactmentReceipt PreparedEnactmentReceipt { get; } = EnactmentReceipt(
        "first-use-enactment-receipt://prepared",
        PreparedEnactment);

    public static FirstUseEnactmentReceipt HeldEnactmentReceipt { get; } = EnactmentReceipt(
        "first-use-enactment-receipt://held",
        HeldEnactment);

    public static FirstUseEnactmentReceipt RefusedEnactmentReceipt { get; } = EnactmentReceipt(
        "first-use-enactment-receipt://refused",
        RefusedEnactment);

    private static FirstUseAdmissionReceipt AdmissionReceipt(
        string receiptHandle,
        FirstUseAdmissionRecord record)
    {
        return new(
            ReceiptHandle: receiptHandle,
            Disposition: record.Disposition,
            Summary: record.NonAuthoritySummary,
            WitnessRefs: record.WitnessRefs,
            TimestampUtc: DateTimeOffset.UnixEpoch);
    }

    private static FirstUseEnactmentReceipt EnactmentReceipt(
        string receiptHandle,
        FirstUseEnactmentRecord record)
    {
        return new(
            ReceiptHandle: receiptHandle,
            Disposition: record.Disposition,
            Summary: record.NonAuthoritySummary,
            WitnessRefs: record.WitnessRefs,
            TimestampUtc: DateTimeOffset.UnixEpoch);
    }
}
