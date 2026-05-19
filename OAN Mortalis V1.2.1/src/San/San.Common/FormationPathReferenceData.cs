namespace San.Common;

public static class FormationPathReferenceData
{
    public static NonActivationState InertState { get; } = new(
        PayloadOpened: false,
        ModelBindingRequested: false,
        RuntimeIdentityRequested: false,
        StateMutationRequested: false,
        EcStartRequested: false,
        RuntimeActionRequested: false,
        LispEvaluationRequested: false,
        LispMorphologyPromotionRequested: false,
        DatabaseWriteRequested: false,
        KnobMutationRequested: false);

    public static FormationPathCoordinateFrame CanonicalCoordinateFrame { get; } = new(
        FrameHandle: "formation-frame://zed-delta-shell",
        OriginRef: "formation-origin://zed-of-delta",
        DomainRef: "formation-domain://spiders-web-v121",
        ShellRef: "formation-shell://dodecahedral-local-law",
        OrdinalPlaneRef: "formation-plane://x-y-z",
        XAxisRef: "formation-axis://x-known-to-surface",
        YAxisRef: "formation-axis://y-witness-spine",
        ZAxisRef: "formation-axis://z-depth-to-shell",
        WitnessRefs: new[]
        {
            "witness-ref://spiders-web-sw02-coordinate-frame"
        });

    public static IReadOnlyList<TypedFormationEvent> CanonicalTypedEvents { get; } = new[]
    {
        new TypedFormationEvent(
            EventHandle: "formation-event://origin-zed",
            Stage: FormationPathStage.PreEngram,
            EventKind: FormationEventKind.Origin,
            CarrierRef: "carrier-ref://zed-of-delta",
            SourceRef: "source-ref://zero-assumption-origin",
            SequenceOrdinal: 0,
            NonActivation: InertState,
            WitnessRefs: new[] { "witness-ref://origin-zed" }),
        new TypedFormationEvent(
            EventHandle: "formation-event://known-light-cone",
            Stage: FormationPathStage.PreEngram,
            EventKind: FormationEventKind.KnownInput,
            CarrierRef: "carrier-ref://light-cone-of-reason",
            SourceRef: "source-ref://known-input",
            SequenceOrdinal: 1,
            NonActivation: InertState,
            WitnessRefs: new[] { "witness-ref://known-light-cone" }),
        new TypedFormationEvent(
            EventHandle: "formation-event://witness-spine",
            Stage: FormationPathStage.Engram,
            EventKind: FormationEventKind.Witness,
            CarrierRef: "carrier-ref://ordered-event-spine",
            SourceRef: "source-ref://witnessed-event",
            SequenceOrdinal: 2,
            NonActivation: InertState,
            WitnessRefs: new[] { "witness-ref://witness-spine" }),
        new TypedFormationEvent(
            EventHandle: "formation-event://cryptic-gate",
            Stage: FormationPathStage.PreproductionEngram,
            EventKind: FormationEventKind.Cryptic,
            CarrierRef: "carrier-ref://cryptic-withheld-surface",
            SourceRef: "source-ref://protected-or-unknown",
            SequenceOrdinal: 3,
            NonActivation: InertState,
            WitnessRefs: new[] { "witness-ref://cryptic-gate" }),
        new TypedFormationEvent(
            EventHandle: "formation-event://product-boundary",
            Stage: FormationPathStage.ProductEngram,
            EventKind: FormationEventKind.ProductBoundary,
            CarrierRef: "carrier-ref://bounded-product-receipt",
            SourceRef: "source-ref://shell-cleaving-boundary",
            SequenceOrdinal: 4,
            NonActivation: InertState,
            WitnessRefs: new[] { "witness-ref://product-boundary" })
    };

    public static FormationSplinePath CanonicalSplinePath { get; } = new(
        PathHandle: "formation-spline://canonical-inert-path",
        CoordinateFrame: CanonicalCoordinateFrame,
        OrderedEvents: CanonicalTypedEvents,
        AnchorRefs: new[]
        {
            "anchor-ref://zed-origin",
            "anchor-ref://witness-spine",
            "anchor-ref://shell-intersection"
        },
        CurvatureSummaryRef: "curvature-summary-ref://synthetic-spline-walk",
        InterlaceSummaryRef: "interlace-summary-ref://typed-event-cubes",
        NonActivation: InertState,
        WitnessRefs: new[]
        {
            "witness-ref://canonical-spline-path"
        });

    public static FormationShellLawDescriptor CanonicalShellLaw { get; } = new(
        ShellLawHandle: "formation-shell-law://dodecahedral-local-law",
        ShellRef: "formation-shell://dodecahedral-local-law",
        LocalLawRef: "local-law-ref://domain-separated-transport-invariant-continuity",
        InvariantRefs: new[]
        {
            "invariant-ref://origin-preserved",
            "invariant-ref://event-order-preserved",
            "invariant-ref://non-activation-preserved",
            "invariant-ref://product-boundary-not-output-authority"
        },
        WithheldSurfaceRefs: new[]
        {
            "withheld-ref://private-control-weights",
            "withheld-ref://runtime-activation-paths",
            "withheld-ref://lisp-morphology"
        },
        IuttPosture: "iutt-inspired-analogical-only",
        NonEquivalenceSummary: "Formation shell law is inspired by disciplined separation and transport, but does not claim IUTT equivalence, proof, application, or endorsement.",
        NonActivation: InertState,
        WitnessRefs: new[]
        {
            "witness-ref://canonical-shell-law"
        });

    public static FormationResidue CanonicalResidue { get; } = new(
        ResidueHandle: "formation-residue://failure-delta-next-constraint",
        SourceEventRef: "formation-event://cryptic-gate",
        DeltaRef: "delta-ref://withheld-surface-to-public-carrier",
        ResidueClass: "withheld_surface_residue",
        NextConstraintRef: "next-constraint-ref://keep-public-carrier-inert",
        SilentRepairAttempted: false,
        NonActivation: InertState,
        WitnessRefs: new[]
        {
            "witness-ref://canonical-residue"
        });

    public static FormationObstruction CanonicalObstruction { get; } = new(
        ObstructionHandle: "formation-obstruction://none",
        SourceEventRef: "formation-event://product-boundary",
        ObstructionClass: "none",
        Severity: FormationObstructionSeverity.Informational,
        HaltReasonRef: "halt-ref://none",
        RequiredResolutionRef: "resolution-ref://none",
        SilentRepairAttempted: false,
        NonActivation: InertState,
        WitnessRefs: new[]
        {
            "witness-ref://canonical-obstruction"
        });

    public static PreproductionIntersection CanonicalPreproductionIntersection { get; } = new(
        IntersectionHandle: "formation-intersection://candidate-thought-threshold",
        SplinePath: CanonicalSplinePath,
        ShellLaw: CanonicalShellLaw,
        Residues: new[] { CanonicalResidue },
        Obstructions: new[] { CanonicalObstruction },
        CandidateThoughtRef: "candidate-thought-ref://preproduction-only",
        OutputAuthorityGranted: false,
        RuntimeActionAuthorized: false,
        NonActivation: InertState,
        WitnessRefs: new[]
        {
            "witness-ref://canonical-preproduction-intersection"
        });

    public static ProductBoundaryReceipt CanonicalProductBoundaryReceipt { get; } = new(
        ReceiptHandle: "product-boundary-receipt://canonical-inert",
        ProductBoundaryRef: "product-boundary-ref://bounded-receipt-only",
        Intersection: CanonicalPreproductionIntersection,
        BoundedProductRef: "bounded-product-ref://receipt-only",
        PublicOutputEmitted: false,
        CanonicalProductClaimed: false,
        RuntimeActionExecuted: false,
        RuntimeIdentityEmitted: false,
        NonActivation: InertState,
        WitnessRefs: new[]
        {
            "witness-ref://canonical-product-boundary"
        });

    public static IReadOnlyList<object> CanonicalFormationObjects { get; } = new object[]
    {
        CanonicalCoordinateFrame,
        CanonicalSplinePath,
        CanonicalShellLaw,
        CanonicalResidue,
        CanonicalObstruction,
        CanonicalPreproductionIntersection,
        CanonicalProductBoundaryReceipt
    };
}
