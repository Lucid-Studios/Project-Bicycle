namespace San.Common;

public static class InstallFacingReturnPostureReferenceData
{
    public static InstallFacingReturnPostureRecord ReadyAcknowledgedRetained { get; } = new(
        ConsumptionDisposition: InstallFacingReadoutConsumptionDisposition.Acknowledged,
        ConsumptionLane: InstallFacingReadoutConsumptionLane.ReadyReception,
        ReturnDisposition: InstallFacingReturnPostureDisposition.Retained,
        ReturnLane: InstallFacingReturnPostureLane.LocalRetention,
        ConsumptionRef: "install-facing-readout-consumption://ready-acknowledged",
        Summary: "Bounded local return is retained without widening authority.",
        WitnessRefs: new[]
        {
            "install-facing-readout-consumption://ready-acknowledged",
            "install-facing-return-posture://ready-retained"
        });

    public static InstallFacingReturnPostureRecord ReadyHeldDeferred { get; } = new(
        ConsumptionDisposition: InstallFacingReadoutConsumptionDisposition.Held,
        ConsumptionLane: InstallFacingReadoutConsumptionLane.WitnessedRouting,
        ReturnDisposition: InstallFacingReturnPostureDisposition.Deferred,
        ReturnLane: InstallFacingReturnPostureLane.LocalDeferral,
        ConsumptionRef: "install-facing-readout-consumption://ready-held",
        Summary: "Bounded local return is deferred without extra teaching surface or activation.",
        WitnessRefs: new[]
        {
            "install-facing-readout-consumption://ready-held",
            "install-facing-return-posture://ready-deferred"
        });

    public static InstallFacingReturnPostureRecord ReadyHeldForwardHorizon { get; } = new(
        ConsumptionDisposition: InstallFacingReadoutConsumptionDisposition.Held,
        ConsumptionLane: InstallFacingReadoutConsumptionLane.WitnessedRouting,
        ReturnDisposition: InstallFacingReturnPostureDisposition.WitnessedForwardHorizon,
        ReturnLane: InstallFacingReturnPostureLane.ForwardHorizonWitness,
        ConsumptionRef: "install-facing-readout-consumption://ready-held",
        Summary: "Later horizon remains witnessed without naming handoff, RTME, or pre-certification.",
        WitnessRefs: new[]
        {
            "install-facing-readout-consumption://ready-held",
            "install-facing-return-posture://ready-forward-horizon"
        });

    public static InstallFacingReturnPostureRecord SilenceHeldDeferred { get; } = new(
        ConsumptionDisposition: InstallFacingReadoutConsumptionDisposition.Held,
        ConsumptionLane: InstallFacingReadoutConsumptionLane.SilentReception,
        ReturnDisposition: InstallFacingReturnPostureDisposition.Deferred,
        ReturnLane: InstallFacingReturnPostureLane.LocalDeferral,
        ConsumptionRef: "install-facing-readout-consumption://silence-held",
        Summary: "Silent return remains deferred without gaining extra teaching surface or activation.",
        WitnessRefs: new[]
        {
            "install-facing-readout-consumption://silence-held",
            "install-facing-return-posture://silence-deferred"
        });

    public static InstallFacingReturnPostureRecord RefusedClosed { get; } = new(
        ConsumptionDisposition: InstallFacingReadoutConsumptionDisposition.Refused,
        ConsumptionLane: InstallFacingReadoutConsumptionLane.RefusalReception,
        ReturnDisposition: InstallFacingReturnPostureDisposition.ClosedRefusal,
        ReturnLane: InstallFacingReturnPostureLane.LocalRefusalClosure,
        ConsumptionRef: "install-facing-readout-consumption://refused",
        Summary: "Refusal closes locally and does not reopen readiness or authorization.",
        WitnessRefs: new[]
        {
            "install-facing-readout-consumption://refused",
            "install-facing-return-posture://refused-closed"
        });

    public static InstallFacingReturnPostureReceipt ReadyAcknowledgedRetainedReceipt { get; } = new(
        ReceiptHandle: "install-facing-return-posture-receipt://ready-retained",
        ReturnDisposition: InstallFacingReturnPostureDisposition.Retained,
        Summary: ReadyAcknowledgedRetained.Summary,
        WitnessRefs: ReadyAcknowledgedRetained.WitnessRefs,
        TimestampUtc: DateTimeOffset.UnixEpoch);

    public static InstallFacingReturnPostureReceipt ReadyHeldDeferredReceipt { get; } = new(
        ReceiptHandle: "install-facing-return-posture-receipt://ready-deferred",
        ReturnDisposition: InstallFacingReturnPostureDisposition.Deferred,
        Summary: ReadyHeldDeferred.Summary,
        WitnessRefs: ReadyHeldDeferred.WitnessRefs,
        TimestampUtc: DateTimeOffset.UnixEpoch);

    public static InstallFacingReturnPostureReceipt ReadyHeldForwardHorizonReceipt { get; } = new(
        ReceiptHandle: "install-facing-return-posture-receipt://ready-forward-horizon",
        ReturnDisposition: InstallFacingReturnPostureDisposition.WitnessedForwardHorizon,
        Summary: ReadyHeldForwardHorizon.Summary,
        WitnessRefs: ReadyHeldForwardHorizon.WitnessRefs,
        TimestampUtc: DateTimeOffset.UnixEpoch);

    public static InstallFacingReturnPostureReceipt SilenceHeldDeferredReceipt { get; } = new(
        ReceiptHandle: "install-facing-return-posture-receipt://silence-deferred",
        ReturnDisposition: InstallFacingReturnPostureDisposition.Deferred,
        Summary: SilenceHeldDeferred.Summary,
        WitnessRefs: SilenceHeldDeferred.WitnessRefs,
        TimestampUtc: DateTimeOffset.UnixEpoch);

    public static InstallFacingReturnPostureReceipt RefusedClosedReceipt { get; } = new(
        ReceiptHandle: "install-facing-return-posture-receipt://refused-closed",
        ReturnDisposition: InstallFacingReturnPostureDisposition.ClosedRefusal,
        Summary: RefusedClosed.Summary,
        WitnessRefs: RefusedClosed.WitnessRefs,
        TimestampUtc: DateTimeOffset.UnixEpoch);
}
