namespace San.Common;

public enum InstallFacingReadoutConsumptionDisposition
{
    Acknowledged = 0,
    Held = 1,
    Refused = 2
}

public enum InstallFacingReadoutConsumptionLane
{
    ReadyReception = 0,
    SilentReception = 1,
    RefusalReception = 2,
    WitnessedRouting = 3
}

public sealed record InstallFacingReadoutConsumptionRecord(
    InstallFacingReadoutBundleDisposition BundleDisposition,
    InstallFacingReadoutConsumptionDisposition ConsumptionDisposition,
    InstallFacingReadoutConsumptionLane ConsumptionLane,
    string BundleRef,
    string Summary,
    IReadOnlyList<string> WitnessRefs);

public sealed record InstallFacingReadoutConsumptionReceipt(
    string ReceiptHandle,
    InstallFacingReadoutConsumptionDisposition ConsumptionDisposition,
    string Summary,
    IReadOnlyList<string> WitnessRefs,
    DateTimeOffset TimestampUtc);
