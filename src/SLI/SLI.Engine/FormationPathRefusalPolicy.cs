using San.Common;

namespace SLI.Engine;

public interface IFormationPathRefusalPolicy
{
    FormationPathRefusalEvaluation Evaluate(
        ProductBoundaryReceipt productBoundaryReceipt,
        CollapseAuthorizationState collapseAuthorization,
        DateTimeOffset timestampUtc);
}

public enum DisclosureSafetyClass
{
    PublicSafe = 0,
    BoundedExplanation = 1,
    WithholdProtectedMechanism = 2,
    SilenceRequired = 3
}

public enum LawfulLowerPathKind
{
    None = 0,
    FamiliarButNotHuman = 1,
    CertificationTraining = 2,
    IdleReceiptFormationReview = 3,
    CSelfGelOrientationRequirement = 4,
    StewardWarrantOrRefusal = 5,
    ProductBoundaryReceipt = 6,
    VaultBoundaryFormationReview = 7,
    WarrantCertificationOrIdle = 8,
    ReceiptRepairPath = 9,
    FormationReview = 10
}

public sealed record CollapseAuthorizationState(
    string? VaultBoundaryRef,
    string? CSelfGelOrientationRef,
    string? CVaultReceiptRef,
    bool AdmittedWorkPresent,
    bool CollapseConditionPresent,
    bool AuthorizationPresent,
    string? LawfulReceiptRef,
    bool PersonificationRequested,
    bool SageAccessRequestedWithoutGates,
    bool RawCrypticRequested)
{
    public bool HasVaultBoundary =>
        !string.IsNullOrWhiteSpace(VaultBoundaryRef);

    public bool HasCSelfGelOrientation =>
        !string.IsNullOrWhiteSpace(CSelfGelOrientationRef);

    public bool HasCVaultReceipt =>
        !string.IsNullOrWhiteSpace(CVaultReceiptRef);

    public bool HasLawfulReceipt =>
        !string.IsNullOrWhiteSpace(LawfulReceiptRef);

    public bool HasAuthorizedCollapseCondition =>
        HasVaultBoundary &&
        HasCSelfGelOrientation &&
        HasCVaultReceipt &&
        AdmittedWorkPresent &&
        CollapseConditionPresent &&
        AuthorizationPresent &&
        HasLawfulReceipt &&
        !PersonificationRequested &&
        !SageAccessRequestedWithoutGates &&
        !RawCrypticRequested;
}

public sealed record IdlePreservationResult(
    bool RtmeRemainsIdle,
    string IdleReasonRef,
    bool EcStartAllowed,
    bool RuntimeActionAllowed,
    bool RuntimeIdentityAllowed)
{
    public bool PreservesIdle =>
        RtmeRemainsIdle &&
        !EcStartAllowed &&
        !RuntimeActionAllowed &&
        !RuntimeIdentityAllowed;
}

public sealed record LawfulLowerPathRoute(
    LawfulLowerPathKind RouteKind,
    string RouteRef,
    string SafeExplanation,
    bool GateBypassAllowed,
    DisclosureSafetyClass DisclosureSafety)
{
    public bool IsLawful =>
        !GateBypassAllowed &&
        RouteKind != LawfulLowerPathKind.None;
}

public sealed record FormationFailureDeltaReceipt(
    string DeltaHandle,
    string FailedSurface,
    string SupportBreak,
    string NextConstraintRef,
    string InternalReceiptRef,
    DisclosureSafetyClass DisclosureSafety,
    string SafeExplanation,
    IReadOnlyList<string> WithheldMechanismRefs,
    DateTimeOffset TimestampUtc);

public sealed record FormationPathRefusalEvaluation(
    SliCmeActualRoundtripDisposition Disposition,
    string EvaluationHandle,
    string OutcomeCode,
    string GovernanceTrace,
    ProductBoundaryReceipt ProductBoundaryReceipt,
    CollapseAuthorizationState CollapseAuthorization,
    FormationFailureDeltaReceipt? FailureDeltaReceipt,
    IdlePreservationResult IdlePreservation,
    LawfulLowerPathRoute? LawfulLowerPathRoute,
    DisclosureSafetyClass DisclosureSafety,
    bool BoundedFutureWorkMayBeConsidered,
    DateTimeOffset TimestampUtc)
{
    public bool IsAdmitted =>
        Disposition == SliCmeActualRoundtripDisposition.Admitted;

    public bool PreservesIdle =>
        IdlePreservation.PreservesIdle;
}

public sealed class DefaultFormationPathRefusalPolicy : IFormationPathRefusalPolicy
{
    public FormationPathRefusalEvaluation Evaluate(
        ProductBoundaryReceipt productBoundaryReceipt,
        CollapseAuthorizationState collapseAuthorization,
        DateTimeOffset timestampUtc)
    {
        ArgumentNullException.ThrowIfNull(productBoundaryReceipt);
        ArgumentNullException.ThrowIfNull(collapseAuthorization);

        if (collapseAuthorization.PersonificationRequested)
        {
            return Refuse(
                productBoundaryReceipt,
                collapseAuthorization,
                "personification-not-promptable",
                "personification requests route to familiar-but-not-human posture unless certified and separately authorized.",
                "personification-request",
                "personification is not promptable, hackable, or roleplayable",
                "next-constraint://personification-certification-and-seal-required",
                DisclosureSafetyClass.BoundedExplanation,
                LowerPath(
                    LawfulLowerPathKind.FamiliarButNotHuman,
                    "lower-path://familiar-but-not-human",
                    "Familiarity may be available, but personification requires certified authorization.",
                    DisclosureSafetyClass.BoundedExplanation),
                timestampUtc,
                "protected-mechanism://personification-macro",
                "protected-mechanism://alpha-omega-seal");
        }

        if (collapseAuthorization.SageAccessRequestedWithoutGates)
        {
            return Withhold(
                productBoundaryReceipt,
                collapseAuthorization,
                "sage-gate-completion-required",
                "SAGE access requires gate completion before deeper CME access can be considered.",
                "sage-request",
                "32-gate completion is missing",
                "next-constraint://complete-sage-certification-gates",
                DisclosureSafetyClass.PublicSafe,
                LowerPath(
                    LawfulLowerPathKind.CertificationTraining,
                    "lower-path://certification-training",
                    "SAGE access routes to certification and training before deeper access.",
                    DisclosureSafetyClass.PublicSafe),
                timestampUtc,
                "protected-mechanism://sage-gate-internals");
        }

        if (collapseAuthorization.RawCrypticRequested)
        {
            return Refuse(
                productBoundaryReceipt,
                collapseAuthorization,
                "raw-cryptic-request-warrant-required",
                "raw Cryptic surfaces require Steward or warrant handling and may remain silent when disclosure is unsafe.",
                "raw-cryptic-request",
                "raw Cryptic request lacks Steward/warrant admission",
                "next-constraint://steward-warrant-or-refusal",
                DisclosureSafetyClass.SilenceRequired,
                LowerPath(
                    LawfulLowerPathKind.StewardWarrantOrRefusal,
                    "lower-path://steward-warrant-or-refusal",
                    "That surface is Steward/warrant gated.",
                    DisclosureSafetyClass.WithholdProtectedMechanism),
                timestampUtc,
                "protected-mechanism://raw-cryptic-state",
                "protected-mechanism://cgoa-private-morphology");
        }

        if (productBoundaryReceipt.RuntimeIdentityEmitted)
        {
            return Refuse(
                productBoundaryReceipt,
                collapseAuthorization,
                "runtime-identity-emission-blocked",
                "runtime identity emission is inadmissible in the formation refusal lane.",
                "product-boundary-runtime-identity",
                "runtime identity was emitted before authorization",
                "next-constraint://preserve-pre-personification-boundary",
                DisclosureSafetyClass.BoundedExplanation,
                LowerPath(
                    LawfulLowerPathKind.FamiliarButNotHuman,
                    "lower-path://familiar-but-not-human",
                    "A familiar surface may remain available without runtime identity emission.",
                    DisclosureSafetyClass.BoundedExplanation),
                timestampUtc,
                "protected-mechanism://runtime-identity");
        }

        if (productBoundaryReceipt.RuntimeActionExecuted)
        {
            return Refuse(
                productBoundaryReceipt,
                collapseAuthorization,
                "runtime-action-execution-blocked",
                "runtime action may not execute from a formation receipt.",
                "product-boundary-runtime-action",
                "runtime action executed before authorization",
                "next-constraint://return-to-idle-formation-review",
                DisclosureSafetyClass.PublicSafe,
                LowerPath(
                    LawfulLowerPathKind.IdleReceiptFormationReview,
                    "lower-path://idle-receipt-formation-review",
                    "The lawful lower path is idle receipt and formation review.",
                    DisclosureSafetyClass.PublicSafe),
                timestampUtc,
                "protected-mechanism://runtime-action");
        }

        if (productBoundaryReceipt.PublicOutputEmitted || productBoundaryReceipt.CanonicalProductClaimed)
        {
            return Refuse(
                productBoundaryReceipt,
                collapseAuthorization,
                "product-boundary-overreach-blocked",
                "a product boundary receipt is not public output or a canonical product.",
                "product-boundary-overreach",
                "public or canonical product state was claimed",
                "next-constraint://receipt-only-product-boundary",
                DisclosureSafetyClass.PublicSafe,
                LowerPath(
                    LawfulLowerPathKind.ProductBoundaryReceipt,
                    "lower-path://product-boundary-receipt",
                    "A bounded product receipt may be preserved without public output authority.",
                    DisclosureSafetyClass.PublicSafe),
                timestampUtc,
                "protected-mechanism://output-authority");
        }

        var intersection = productBoundaryReceipt.Intersection;
        if (intersection is null)
        {
            return Refuse(
                productBoundaryReceipt,
                collapseAuthorization,
                "preproduction-intersection-required",
                "product boundary receipt requires a preproduction intersection.",
                "product-boundary-intersection",
                "preproduction intersection missing",
                "next-constraint://restore-preproduction-intersection",
                DisclosureSafetyClass.PublicSafe,
                LowerPath(
                    LawfulLowerPathKind.FormationReview,
                    "lower-path://formation-review",
                    "The lawful lower path is formation review.",
                    DisclosureSafetyClass.PublicSafe),
                timestampUtc);
        }

        if (intersection.RuntimeActionAuthorized)
        {
            return Refuse(
                productBoundaryReceipt,
                collapseAuthorization,
                "runtime-action-authorization-blocked",
                "runtime action authorization cannot pass through the formation path.",
                "preproduction-runtime-action",
                "runtime action was authorized before runtime lane",
                "next-constraint://keep-formation-path-non-runtime",
                DisclosureSafetyClass.BoundedExplanation,
                LowerPath(
                    LawfulLowerPathKind.IdleReceiptFormationReview,
                    "lower-path://idle-receipt-formation-review",
                    "The lawful lower path is idle receipt and formation review.",
                    DisclosureSafetyClass.PublicSafe),
                timestampUtc,
                "protected-mechanism://runtime-action");
        }

        if (intersection.OutputAuthorityGranted)
        {
            return Refuse(
                productBoundaryReceipt,
                collapseAuthorization,
                "output-authority-premature-blocked",
                "candidate thought threshold is not output authority.",
                "preproduction-output-authority",
                "output authority was granted before output lane",
                "next-constraint://separate-product-boundary-from-output-authority",
                DisclosureSafetyClass.PublicSafe,
                LowerPath(
                    LawfulLowerPathKind.ProductBoundaryReceipt,
                    "lower-path://product-boundary-receipt",
                    "A bounded receipt may be formed; output authority remains separate.",
                    DisclosureSafetyClass.PublicSafe),
                timestampUtc,
                "protected-mechanism://output-authority");
        }

        if (intersection.ShellLaw is null || intersection.ShellLaw.HasForbiddenClaim)
        {
            return Refuse(
                productBoundaryReceipt,
                collapseAuthorization,
                "shell-law-overclaim-blocked",
                "formation shell law must remain supported and non-equivalent unless separately proven.",
                "shell-law-overclaim",
                "shell law carried forbidden formal overclaim",
                "next-constraint://reduce-claim-to-supported-posture",
                DisclosureSafetyClass.PublicSafe,
                LowerPath(
                    LawfulLowerPathKind.FormationReview,
                    "lower-path://formation-review",
                    "The lawful lower path is formation review and claim reduction.",
                    DisclosureSafetyClass.PublicSafe),
                timestampUtc,
                "protected-mechanism://formal-proof-equivalence");
        }

        if (intersection.HasBlockingObstruction)
        {
            return Refuse(
                productBoundaryReceipt,
                collapseAuthorization,
                "blocking-obstruction-present",
                "blocking obstruction halts movement rather than smoothing contradiction.",
                "formation-obstruction",
                "blocking obstruction present",
                "next-constraint://resolve-blocking-obstruction-before-movement",
                DisclosureSafetyClass.PublicSafe,
                LowerPath(
                    LawfulLowerPathKind.FormationReview,
                    "lower-path://formation-review",
                    "The lawful lower path is obstruction review.",
                    DisclosureSafetyClass.PublicSafe),
                timestampUtc);
        }

        if (HasSilentRepair(intersection))
        {
            return Refuse(
                productBoundaryReceipt,
                collapseAuthorization,
                "silent-repair-attempt-blocked",
                "failure becomes delta rather than hidden patch.",
                "formation-silent-repair",
                "silent repair was attempted",
                "next-constraint://record-delta-without-repair",
                DisclosureSafetyClass.WithholdProtectedMechanism,
                LowerPath(
                    LawfulLowerPathKind.ReceiptRepairPath,
                    "lower-path://receipt-repair",
                    "A receipt repair path is required before movement continues.",
                    DisclosureSafetyClass.BoundedExplanation),
                timestampUtc,
                "protected-mechanism://repair-internals");
        }

        if (productBoundaryReceipt.HasForbiddenActivation)
        {
            return Refuse(
                productBoundaryReceipt,
                collapseAuthorization,
                "non-activation-drift-blocked",
                "formation objects must preserve non-activation before any movement.",
                "formation-non-activation",
                "non-activation drift detected",
                "next-constraint://restore-inert-non-activation-state",
                DisclosureSafetyClass.BoundedExplanation,
                LowerPath(
                    LawfulLowerPathKind.ReceiptRepairPath,
                    "lower-path://receipt-repair",
                    "Non-activation receipt repair is required before movement continues.",
                    DisclosureSafetyClass.BoundedExplanation),
                timestampUtc,
                "protected-mechanism://non-activation-drift");
        }

        if (!collapseAuthorization.HasVaultBoundary)
        {
            return WithholdIdle(
                productBoundaryReceipt,
                collapseAuthorization,
                "vault-boundary-required",
                "continuity must be bounded before cognition work is admitted.",
                "firebox-vault-boundary",
                "vault boundary missing",
                "next-constraint://vault-boundary-formation-review",
                LawfulLowerPathKind.VaultBoundaryFormationReview,
                "lower-path://vault-boundary-formation-review",
                "A continuity boundary must be formed before cognition work.",
                timestampUtc,
                "protected-mechanism://vault-formation");
        }

        if (!collapseAuthorization.HasCSelfGelOrientation)
        {
            return WithholdIdle(
                productBoundaryReceipt,
                collapseAuthorization,
                "cselfgel-orientation-required",
                "cSelfGEL orientation is required before retained reference admission.",
                "firebox-cselfgel-orientation",
                "cSelfGEL orientation missing",
                "next-constraint://cselfgel-orientation-required",
                LawfulLowerPathKind.CSelfGelOrientationRequirement,
                "lower-path://cselfgel-orientation",
                "Retained reference admission requires the control-safe self orientation receipt first.",
                timestampUtc,
                "protected-mechanism://cselfgel-control-surface");
        }

        if (!collapseAuthorization.HasCVaultReceipt)
        {
            return WithholdIdle(
                productBoundaryReceipt,
                collapseAuthorization,
                "cvault-receipt-required",
                "cVault receipt is required before RTME wake may be considered.",
                "firebox-cvault-receipt",
                "cVault receipt missing",
                "next-constraint://cvault-receipt-required",
                LawfulLowerPathKind.ReceiptRepairPath,
                "lower-path://receipt-repair",
                "A cVault receipt is required before movement can continue.",
                timestampUtc,
                "protected-mechanism://cvault-reference");
        }

        if (!collapseAuthorization.AdmittedWorkPresent)
        {
            return WithholdIdle(
                productBoundaryReceipt,
                collapseAuthorization,
                "admitted-work-required",
                "no admitted work means no EC work.",
                "firebox-admitted-work",
                "admitted work missing",
                "next-constraint://formation-review-for-admitted-work",
                LawfulLowerPathKind.IdleReceiptFormationReview,
                "lower-path://idle-receipt-formation-review",
                "No cognition work is admitted; the RTME remains idle pending formation review.",
                timestampUtc);
        }

        if (!collapseAuthorization.CollapseConditionPresent)
        {
            return WithholdIdle(
                productBoundaryReceipt,
                collapseAuthorization,
                "collapse-condition-required",
                "no collapse condition means EC RTME remains idle.",
                "firebox-collapse-condition",
                "collapse condition missing",
                "next-constraint://collapse-condition-required",
                LawfulLowerPathKind.IdleReceiptFormationReview,
                "lower-path://idle-receipt-formation-review",
                "No collapse condition is present; the RTME remains idle.",
                timestampUtc,
                "protected-mechanism://collapse-condition");
        }

        if (!collapseAuthorization.AuthorizationPresent)
        {
            return WithholdIdle(
                productBoundaryReceipt,
                collapseAuthorization,
                "authorization-required",
                "authorization must be explicit and lawful.",
                "firebox-authorization",
                "authorization missing",
                "next-constraint://warrant-certification-or-idle",
                LawfulLowerPathKind.WarrantCertificationOrIdle,
                "lower-path://warrant-certification-or-idle",
                "This action requires authorization; until then the system remains idle.",
                timestampUtc,
                "protected-mechanism://authorization-surface");
        }

        if (!collapseAuthorization.HasLawfulReceipt)
        {
            return WithholdIdle(
                productBoundaryReceipt,
                collapseAuthorization,
                "lawful-receipt-required",
                "no receipt means no wake.",
                "firebox-lawful-receipt",
                "lawful receipt missing",
                "next-constraint://receipt-repair-path",
                LawfulLowerPathKind.ReceiptRepairPath,
                "lower-path://receipt-repair",
                "A lawful receipt is required before movement can continue.",
                timestampUtc,
                "protected-mechanism://receipt-bypass");
        }

        return AdmitPassive(productBoundaryReceipt, collapseAuthorization, timestampUtc);
    }

    private static bool HasSilentRepair(PreproductionIntersection intersection) =>
        (intersection.Residues?.Any(static residue => residue is not null && residue.SilentRepairAttempted) ?? false) ||
        (intersection.Obstructions?.Any(static obstruction => obstruction is not null && obstruction.SilentRepairAttempted) ?? false);

    private static FormationPathRefusalEvaluation Refuse(
        ProductBoundaryReceipt productBoundaryReceipt,
        CollapseAuthorizationState collapseAuthorization,
        string outcomeCode,
        string governanceTrace,
        string failedSurface,
        string supportBreak,
        string nextConstraintRef,
        DisclosureSafetyClass disclosureSafety,
        LawfulLowerPathRoute? lowerPath,
        DateTimeOffset timestampUtc,
        params string[] withheldMechanismRefs) =>
        CreateNonAdmitted(
            SliCmeActualRoundtripDisposition.Refused,
            productBoundaryReceipt,
            collapseAuthorization,
            outcomeCode,
            governanceTrace,
            failedSurface,
            supportBreak,
            nextConstraintRef,
            disclosureSafety,
            lowerPath,
            timestampUtc,
            withheldMechanismRefs);

    private static FormationPathRefusalEvaluation Withhold(
        ProductBoundaryReceipt productBoundaryReceipt,
        CollapseAuthorizationState collapseAuthorization,
        string outcomeCode,
        string governanceTrace,
        string failedSurface,
        string supportBreak,
        string nextConstraintRef,
        DisclosureSafetyClass disclosureSafety,
        LawfulLowerPathRoute? lowerPath,
        DateTimeOffset timestampUtc,
        params string[] withheldMechanismRefs) =>
        CreateNonAdmitted(
            SliCmeActualRoundtripDisposition.Withheld,
            productBoundaryReceipt,
            collapseAuthorization,
            outcomeCode,
            governanceTrace,
            failedSurface,
            supportBreak,
            nextConstraintRef,
            disclosureSafety,
            lowerPath,
            timestampUtc,
            withheldMechanismRefs);

    private static FormationPathRefusalEvaluation WithholdIdle(
        ProductBoundaryReceipt productBoundaryReceipt,
        CollapseAuthorizationState collapseAuthorization,
        string outcomeCode,
        string governanceTrace,
        string failedSurface,
        string supportBreak,
        string nextConstraintRef,
        LawfulLowerPathKind routeKind,
        string routeRef,
        string safeExplanation,
        DateTimeOffset timestampUtc,
        params string[] withheldMechanismRefs) =>
        Withhold(
            productBoundaryReceipt,
            collapseAuthorization,
            outcomeCode,
            governanceTrace,
            failedSurface,
            supportBreak,
            nextConstraintRef,
            DisclosureSafetyClass.BoundedExplanation,
            LowerPath(routeKind, routeRef, safeExplanation, DisclosureSafetyClass.BoundedExplanation),
            timestampUtc,
            withheldMechanismRefs);

    private static FormationPathRefusalEvaluation CreateNonAdmitted(
        SliCmeActualRoundtripDisposition disposition,
        ProductBoundaryReceipt productBoundaryReceipt,
        CollapseAuthorizationState collapseAuthorization,
        string outcomeCode,
        string governanceTrace,
        string failedSurface,
        string supportBreak,
        string nextConstraintRef,
        DisclosureSafetyClass disclosureSafety,
        LawfulLowerPathRoute? lowerPath,
        DateTimeOffset timestampUtc,
        IReadOnlyList<string> withheldMechanismRefs)
    {
        var evaluationHandle = CreateEvaluationHandle(productBoundaryReceipt, outcomeCode);
        var failureDelta = new FormationFailureDeltaReceipt(
            DeltaHandle: $"formation-failure-delta://{Math.Abs(HashCode.Combine(evaluationHandle, failedSurface)):x}",
            FailedSurface: failedSurface,
            SupportBreak: supportBreak,
            NextConstraintRef: nextConstraintRef,
            InternalReceiptRef: $"internal-receipt://{evaluationHandle}",
            DisclosureSafety: disclosureSafety,
            SafeExplanation: lowerPath?.SafeExplanation ?? "The movement is not admitted here.",
            WithheldMechanismRefs: withheldMechanismRefs,
            TimestampUtc: timestampUtc);

        return new FormationPathRefusalEvaluation(
            Disposition: disposition,
            EvaluationHandle: evaluationHandle,
            OutcomeCode: outcomeCode,
            GovernanceTrace: governanceTrace,
            ProductBoundaryReceipt: productBoundaryReceipt,
            CollapseAuthorization: collapseAuthorization,
            FailureDeltaReceipt: failureDelta,
            IdlePreservation: PreserveIdle($"idle-reason://{outcomeCode}"),
            LawfulLowerPathRoute: lowerPath,
            DisclosureSafety: disclosureSafety,
            BoundedFutureWorkMayBeConsidered: false,
            TimestampUtc: timestampUtc);
    }

    private static FormationPathRefusalEvaluation AdmitPassive(
        ProductBoundaryReceipt productBoundaryReceipt,
        CollapseAuthorizationState collapseAuthorization,
        DateTimeOffset timestampUtc)
    {
        var outcomeCode = "bounded-future-work-may-be-considered";
        var evaluationHandle = CreateEvaluationHandle(productBoundaryReceipt, outcomeCode);

        return new FormationPathRefusalEvaluation(
            Disposition: SliCmeActualRoundtripDisposition.Admitted,
            EvaluationHandle: evaluationHandle,
            OutcomeCode: outcomeCode,
            GovernanceTrace: "authorized collapse condition is present; SW-03 admits passive consideration only and starts no runtime work.",
            ProductBoundaryReceipt: productBoundaryReceipt,
            CollapseAuthorization: collapseAuthorization,
            FailureDeltaReceipt: null,
            IdlePreservation: PreserveIdle("idle-reason://sw03-passive-admission-no-ec-start"),
            LawfulLowerPathRoute: null,
            DisclosureSafety: DisclosureSafetyClass.PublicSafe,
            BoundedFutureWorkMayBeConsidered: true,
            TimestampUtc: timestampUtc);
    }

    private static IdlePreservationResult PreserveIdle(string reasonRef) =>
        new(
            RtmeRemainsIdle: true,
            IdleReasonRef: reasonRef,
            EcStartAllowed: false,
            RuntimeActionAllowed: false,
            RuntimeIdentityAllowed: false);

    private static LawfulLowerPathRoute LowerPath(
        LawfulLowerPathKind kind,
        string routeRef,
        string safeExplanation,
        DisclosureSafetyClass disclosureSafety) =>
        new(
            RouteKind: kind,
            RouteRef: routeRef,
            SafeExplanation: safeExplanation,
            GateBypassAllowed: false,
            DisclosureSafety: disclosureSafety);

    private static string CreateEvaluationHandle(ProductBoundaryReceipt productBoundaryReceipt, string discriminator) =>
        $"formation-refusal-evaluation://{Math.Abs(HashCode.Combine(productBoundaryReceipt.ReceiptHandle, discriminator)):x}";
}
