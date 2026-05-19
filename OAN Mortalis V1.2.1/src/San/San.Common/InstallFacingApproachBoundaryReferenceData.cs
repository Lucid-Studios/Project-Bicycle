namespace San.Common;

public static class InstallFacingApproachBoundaryReferenceData
{
    public static InstallFacingApproachBoundaryRecord RetainedControlSurface { get; } = new(
        SourceReturnDisposition: InstallFacingReturnPostureDisposition.Retained,
        SourceReturnLane: InstallFacingReturnPostureLane.LocalRetention,
        ApproachEligibilities: new[]
        {
            InstallFacingApproachEligibility.ControlSurfaceEligible
        },
        FutureControlSurfaceClass: InstallFacingFutureControlSurfaceClass.LocalRetentionControl,
        FutureTelemetryAnchorClass: InstallFacingFutureTelemetryAnchorClass.None,
        FuturePredicateTemplateAnchorClass: InstallFacingFuturePredicateTemplateAnchorClass.None,
        SourceReturnRef: "install-facing-return-posture://ready-retained",
        Summary: "Retained return posture is eligible for future local retention control-surface reference only.",
        WitnessRefs: new[]
        {
            "install-facing-return-posture://ready-retained",
            "install-facing-approach-boundary://retained-control"
        });

    public static InstallFacingApproachBoundaryRecord DeferredHoldControlSurface { get; } = new(
        SourceReturnDisposition: InstallFacingReturnPostureDisposition.Deferred,
        SourceReturnLane: InstallFacingReturnPostureLane.LocalDeferral,
        ApproachEligibilities: new[]
        {
            InstallFacingApproachEligibility.ControlSurfaceEligible
        },
        FutureControlSurfaceClass: InstallFacingFutureControlSurfaceClass.DeferredHoldControl,
        FutureTelemetryAnchorClass: InstallFacingFutureTelemetryAnchorClass.None,
        FuturePredicateTemplateAnchorClass: InstallFacingFuturePredicateTemplateAnchorClass.None,
        SourceReturnRef: "install-facing-return-posture://ready-deferred",
        Summary: "Deferred return posture is eligible for future deferred-hold control-surface reference only.",
        WitnessRefs: new[]
        {
            "install-facing-return-posture://ready-deferred",
            "install-facing-approach-boundary://deferred-control"
        });

    public static InstallFacingApproachBoundaryRecord ClosedRefusalControlSurface { get; } = new(
        SourceReturnDisposition: InstallFacingReturnPostureDisposition.ClosedRefusal,
        SourceReturnLane: InstallFacingReturnPostureLane.LocalRefusalClosure,
        ApproachEligibilities: new[]
        {
            InstallFacingApproachEligibility.ControlSurfaceEligible
        },
        FutureControlSurfaceClass: InstallFacingFutureControlSurfaceClass.RefusalClosureControl,
        FutureTelemetryAnchorClass: InstallFacingFutureTelemetryAnchorClass.None,
        FuturePredicateTemplateAnchorClass: InstallFacingFuturePredicateTemplateAnchorClass.None,
        SourceReturnRef: "install-facing-return-posture://refused-closed",
        Summary: "Closed refusal return posture is eligible for future refusal-closure control-surface reference only.",
        WitnessRefs: new[]
        {
            "install-facing-return-posture://refused-closed",
            "install-facing-approach-boundary://closed-refusal-control"
        });

    public static InstallFacingApproachBoundaryRecord WitnessedForwardHorizonAnchors { get; } = new(
        SourceReturnDisposition: InstallFacingReturnPostureDisposition.WitnessedForwardHorizon,
        SourceReturnLane: InstallFacingReturnPostureLane.ForwardHorizonWitness,
        ApproachEligibilities: new[]
        {
            InstallFacingApproachEligibility.ControlSurfaceEligible,
            InstallFacingApproachEligibility.TelemetryAnchorEligible,
            InstallFacingApproachEligibility.PredicateTemplateAnchorEligible
        },
        FutureControlSurfaceClass: InstallFacingFutureControlSurfaceClass.ForwardHorizonControl,
        FutureTelemetryAnchorClass: InstallFacingFutureTelemetryAnchorClass.ForwardHorizonTelemetryPoint,
        FuturePredicateTemplateAnchorClass: InstallFacingFuturePredicateTemplateAnchorClass.ForwardHorizonPredicateTemplate,
        SourceReturnRef: "install-facing-return-posture://ready-forward-horizon",
        Summary: "Witnessed forward horizon is only a future anchor and not active SLI.Lisp telemetry, template generation, handoff, RTME approach, or pre-certification.",
        WitnessRefs: new[]
        {
            "install-facing-return-posture://ready-forward-horizon",
            "install-facing-approach-boundary://forward-horizon-anchors"
        });
}
