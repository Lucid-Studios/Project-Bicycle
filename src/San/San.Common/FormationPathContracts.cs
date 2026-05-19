namespace San.Common;

public enum FormationPathStage
{
    PreEngram = 0,
    Engram = 1,
    PreproductionEngram = 2,
    ProductEngram = 3
}

public enum FormationEventKind
{
    Origin = 0,
    KnownInput = 1,
    Witness = 2,
    Cryptic = 3,
    OrderedSpine = 4,
    TypedEvent = 5,
    SplineWalk = 6,
    ShellLaw = 7,
    Residue = 8,
    Obstruction = 9,
    PreproductionIntersection = 10,
    ProductBoundary = 11
}

public enum FormationObstructionSeverity
{
    Informational = 0,
    ReviewRequired = 1,
    Blocking = 2
}

public sealed record FormationPathCoordinateFrame(
    string FrameHandle,
    string OriginRef,
    string DomainRef,
    string ShellRef,
    string OrdinalPlaneRef,
    string XAxisRef,
    string YAxisRef,
    string ZAxisRef,
    IReadOnlyList<string> WitnessRefs);

public sealed record TypedFormationEvent(
    string EventHandle,
    FormationPathStage Stage,
    FormationEventKind EventKind,
    string CarrierRef,
    string SourceRef,
    int SequenceOrdinal,
    NonActivationState NonActivation,
    IReadOnlyList<string> WitnessRefs)
{
    public bool HasForbiddenActivation =>
        NonActivation is null || !NonActivation.IsInert;
}

public sealed record FormationSplinePath(
    string PathHandle,
    FormationPathCoordinateFrame CoordinateFrame,
    IReadOnlyList<TypedFormationEvent> OrderedEvents,
    IReadOnlyList<string> AnchorRefs,
    string CurvatureSummaryRef,
    string InterlaceSummaryRef,
    NonActivationState NonActivation,
    IReadOnlyList<string> WitnessRefs)
{
    public bool HasForbiddenActivation =>
        NonActivation is null ||
        !NonActivation.IsInert ||
        CoordinateFrame is null ||
        OrderedEvents is null ||
        OrderedEvents.Count == 0 ||
        OrderedEvents.Any(static formationEvent => formationEvent is null || formationEvent.HasForbiddenActivation);
}

public sealed record FormationShellLawDescriptor(
    string ShellLawHandle,
    string ShellRef,
    string LocalLawRef,
    IReadOnlyList<string> InvariantRefs,
    IReadOnlyList<string> WithheldSurfaceRefs,
    string IuttPosture,
    string NonEquivalenceSummary,
    NonActivationState NonActivation,
    IReadOnlyList<string> WitnessRefs)
{
    public bool HasForbiddenActivation =>
        NonActivation is null || !NonActivation.IsInert;

    public bool HasForbiddenClaim =>
        string.IsNullOrWhiteSpace(IuttPosture) ||
        !IuttPosture.Contains("inspired", StringComparison.OrdinalIgnoreCase) ||
        IuttPosture.Contains("equivalent", StringComparison.OrdinalIgnoreCase) ||
        IuttPosture.Contains("proof", StringComparison.OrdinalIgnoreCase) ||
        IuttPosture.Contains("application", StringComparison.OrdinalIgnoreCase);
}

public sealed record FormationResidue(
    string ResidueHandle,
    string SourceEventRef,
    string DeltaRef,
    string ResidueClass,
    string NextConstraintRef,
    bool SilentRepairAttempted,
    NonActivationState NonActivation,
    IReadOnlyList<string> WitnessRefs)
{
    public bool HasForbiddenActivation =>
        NonActivation is null ||
        !NonActivation.IsInert ||
        SilentRepairAttempted;
}

public sealed record FormationObstruction(
    string ObstructionHandle,
    string SourceEventRef,
    string ObstructionClass,
    FormationObstructionSeverity Severity,
    string HaltReasonRef,
    string RequiredResolutionRef,
    bool SilentRepairAttempted,
    NonActivationState NonActivation,
    IReadOnlyList<string> WitnessRefs)
{
    public bool HasForbiddenActivation =>
        NonActivation is null ||
        !NonActivation.IsInert ||
        SilentRepairAttempted;

    public bool HasBlockingObstruction =>
        Severity == FormationObstructionSeverity.Blocking;
}

public sealed record PreproductionIntersection(
    string IntersectionHandle,
    FormationSplinePath SplinePath,
    FormationShellLawDescriptor ShellLaw,
    IReadOnlyList<FormationResidue> Residues,
    IReadOnlyList<FormationObstruction> Obstructions,
    string CandidateThoughtRef,
    bool OutputAuthorityGranted,
    bool RuntimeActionAuthorized,
    NonActivationState NonActivation,
    IReadOnlyList<string> WitnessRefs)
{
    public bool HasForbiddenActivation =>
        NonActivation is null ||
        !NonActivation.IsInert ||
        SplinePath is null ||
        SplinePath.HasForbiddenActivation ||
        ShellLaw is null ||
        ShellLaw.HasForbiddenActivation ||
        ShellLaw.HasForbiddenClaim ||
        Residues is null ||
        Residues.Any(static residue => residue is null || residue.HasForbiddenActivation) ||
        Obstructions is null ||
        Obstructions.Any(static obstruction => obstruction is null || obstruction.HasForbiddenActivation) ||
        OutputAuthorityGranted ||
        RuntimeActionAuthorized;

    public bool HasBlockingObstruction =>
        Obstructions is not null &&
        Obstructions.Any(static obstruction => obstruction is not null && obstruction.HasBlockingObstruction);
}

public sealed record ProductBoundaryReceipt(
    string ReceiptHandle,
    string ProductBoundaryRef,
    PreproductionIntersection Intersection,
    string BoundedProductRef,
    bool PublicOutputEmitted,
    bool CanonicalProductClaimed,
    bool RuntimeActionExecuted,
    bool RuntimeIdentityEmitted,
    NonActivationState NonActivation,
    IReadOnlyList<string> WitnessRefs)
{
    public bool HasForbiddenActivation =>
        NonActivation is null ||
        !NonActivation.IsInert ||
        Intersection is null ||
        Intersection.HasForbiddenActivation ||
        Intersection.HasBlockingObstruction ||
        PublicOutputEmitted ||
        CanonicalProductClaimed ||
        RuntimeActionExecuted ||
        RuntimeIdentityEmitted;
}
