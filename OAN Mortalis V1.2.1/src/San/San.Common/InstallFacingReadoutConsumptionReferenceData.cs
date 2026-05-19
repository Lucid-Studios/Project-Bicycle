namespace San.Common;

public static class InstallFacingReadoutConsumptionReferenceData
{
    public static InstallFacingReadoutConsumptionRecord ReadyAcknowledged { get; } = new(
        BundleDisposition: InstallFacingReadoutBundleDisposition.Ready,
        ConsumptionDisposition: InstallFacingReadoutConsumptionDisposition.Acknowledged,
        ConsumptionLane: InstallFacingReadoutConsumptionLane.ReadyReception,
        BundleRef: "install-facing-readout://ready",
        Summary: "Ready readout bundle is acknowledged as bounded outward footing without authority upgrade.",
        WitnessRefs: new[]
        {
            "install-facing-readout://ready",
            "install-facing-readout-consumption://ready-acknowledged"
        });

    public static InstallFacingReadoutConsumptionRecord ReadyHeld { get; } = new(
        BundleDisposition: InstallFacingReadoutBundleDisposition.Ready,
        ConsumptionDisposition: InstallFacingReadoutConsumptionDisposition.Held,
        ConsumptionLane: InstallFacingReadoutConsumptionLane.WitnessedRouting,
        BundleRef: "install-facing-readout://ready",
        Summary: "Ready readout bundle is held in witnessed routing posture without runtime activation.",
        WitnessRefs: new[]
        {
            "install-facing-readout://ready",
            "install-facing-readout-consumption://ready-held"
        });

    public static InstallFacingReadoutConsumptionRecord SilenceHeld { get; } = new(
        BundleDisposition: InstallFacingReadoutBundleDisposition.Silence,
        ConsumptionDisposition: InstallFacingReadoutConsumptionDisposition.Held,
        ConsumptionLane: InstallFacingReadoutConsumptionLane.SilentReception,
        BundleRef: "install-facing-readout://silence",
        Summary: "Silent readout bundle is held without gaining extra teaching surface.",
        WitnessRefs: new[]
        {
            "install-facing-readout://silence",
            "install-facing-readout-consumption://silence-held"
        });

    public static InstallFacingReadoutConsumptionRecord RefusedReception { get; } = new(
        BundleDisposition: InstallFacingReadoutBundleDisposition.Refused,
        ConsumptionDisposition: InstallFacingReadoutConsumptionDisposition.Refused,
        ConsumptionLane: InstallFacingReadoutConsumptionLane.RefusalReception,
        BundleRef: "install-facing-readout://refused",
        Summary: "Refused readout bundle remains refused in consumption and does not upgrade into readiness or authorization.",
        WitnessRefs: new[]
        {
            "install-facing-readout://refused",
            "install-facing-readout-consumption://refused"
        });

    public static InstallFacingReadoutConsumptionReceipt ReadyAcknowledgedReceipt { get; } = new(
        ReceiptHandle: "install-facing-readout-consumption-receipt://ready-acknowledged",
        ConsumptionDisposition: InstallFacingReadoutConsumptionDisposition.Acknowledged,
        Summary: ReadyAcknowledged.Summary,
        WitnessRefs: ReadyAcknowledged.WitnessRefs,
        TimestampUtc: DateTimeOffset.UnixEpoch);

    public static InstallFacingReadoutConsumptionReceipt ReadyHeldReceipt { get; } = new(
        ReceiptHandle: "install-facing-readout-consumption-receipt://ready-held",
        ConsumptionDisposition: InstallFacingReadoutConsumptionDisposition.Held,
        Summary: ReadyHeld.Summary,
        WitnessRefs: ReadyHeld.WitnessRefs,
        TimestampUtc: DateTimeOffset.UnixEpoch);

    public static InstallFacingReadoutConsumptionReceipt SilenceHeldReceipt { get; } = new(
        ReceiptHandle: "install-facing-readout-consumption-receipt://silence-held",
        ConsumptionDisposition: InstallFacingReadoutConsumptionDisposition.Held,
        Summary: SilenceHeld.Summary,
        WitnessRefs: SilenceHeld.WitnessRefs,
        TimestampUtc: DateTimeOffset.UnixEpoch);

    public static InstallFacingReadoutConsumptionReceipt RefusedReceipt { get; } = new(
        ReceiptHandle: "install-facing-readout-consumption-receipt://refused",
        ConsumptionDisposition: InstallFacingReadoutConsumptionDisposition.Refused,
        Summary: RefusedReception.Summary,
        WitnessRefs: RefusedReception.WitnessRefs,
        TimestampUtc: DateTimeOffset.UnixEpoch);
}
