namespace San.Common;

public enum InstallFacingApproachEligibility
{
    LocalOnly = 0,
    ControlSurfaceEligible = 1,
    TelemetryAnchorEligible = 2,
    PredicateTemplateAnchorEligible = 3,
    Withheld = 4
}

public enum InstallFacingFutureControlSurfaceClass
{
    None = 0,
    LocalRetentionControl = 1,
    DeferredHoldControl = 2,
    RefusalClosureControl = 3,
    ForwardHorizonControl = 4
}

public enum InstallFacingFutureTelemetryAnchorClass
{
    None = 0,
    ReturnPostureTelemetryPoint = 1,
    ForwardHorizonTelemetryPoint = 2
}

public enum InstallFacingFuturePredicateTemplateAnchorClass
{
    None = 0,
    ReturnPosturePredicateTemplate = 1,
    ForwardHorizonPredicateTemplate = 2
}

public sealed record InstallFacingApproachBoundaryRecord(
    InstallFacingReturnPostureDisposition SourceReturnDisposition,
    InstallFacingReturnPostureLane SourceReturnLane,
    IReadOnlyList<InstallFacingApproachEligibility> ApproachEligibilities,
    InstallFacingFutureControlSurfaceClass FutureControlSurfaceClass,
    InstallFacingFutureTelemetryAnchorClass FutureTelemetryAnchorClass,
    InstallFacingFuturePredicateTemplateAnchorClass FuturePredicateTemplateAnchorClass,
    string SourceReturnRef,
    string Summary,
    IReadOnlyList<string> WitnessRefs);
