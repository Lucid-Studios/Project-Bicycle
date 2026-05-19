namespace San.Common;

public enum InstallFacingReturnPostureDisposition
{
    Retained = 0,
    Deferred = 1,
    ClosedRefusal = 2,
    WitnessedForwardHorizon = 3
}

public enum InstallFacingReturnPostureLane
{
    LocalRetention = 0,
    LocalDeferral = 1,
    LocalRefusalClosure = 2,
    ForwardHorizonWitness = 3
}

public sealed record InstallFacingReturnPostureRecord(
    InstallFacingReadoutConsumptionDisposition ConsumptionDisposition,
    InstallFacingReadoutConsumptionLane ConsumptionLane,
    InstallFacingReturnPostureDisposition ReturnDisposition,
    InstallFacingReturnPostureLane ReturnLane,
    string ConsumptionRef,
    string Summary,
    IReadOnlyList<string> WitnessRefs);

public sealed record InstallFacingReturnPostureReceipt(
    string ReceiptHandle,
    InstallFacingReturnPostureDisposition ReturnDisposition,
    string Summary,
    IReadOnlyList<string> WitnessRefs,
    DateTimeOffset TimestampUtc);
