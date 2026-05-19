using San.Common;
using SLI.Engine;
using Xunit;

namespace San.Audit.Tests;

public sealed class FormationPathRefusalPolicyTests
{
    private static readonly DateTimeOffset TimestampUtc = DateTimeOffset.Parse("2026-05-06T00:00:00Z");

    [Fact]
    public void Evaluate_Admits_Passive_Canonical_Receipt_When_Collapse_Authorization_Is_Complete()
    {
        var evaluation = Evaluate(
            FormationPathReferenceData.CanonicalProductBoundaryReceipt,
            CompleteAuthorization());

        AssertAdmitted(evaluation, "bounded-future-work-may-be-considered");
        Assert.Equal(DisclosureSafetyClass.PublicSafe, evaluation.DisclosureSafety);
        Assert.Null(evaluation.LawfulLowerPathRoute);
        Assert.False(string.IsNullOrWhiteSpace(evaluation.GovernanceTrace));
    }

    [Theory]
    [MemberData(nameof(FireboxPreconditionCases))]
    public void Evaluate_Withholds_When_Firebox_Precondition_Is_Missing(
        string caseId,
        CollapseAuthorizationState authorization,
        string outcomeCode,
        LawfulLowerPathKind lowerPathKind)
    {
        var evaluation = Evaluate(
            FormationPathReferenceData.CanonicalProductBoundaryReceipt,
            authorization);

        AssertNonAdmitted(
            evaluation,
            SliCmeActualRoundtripDisposition.Withheld,
            outcomeCode,
            lowerPathKind,
            DisclosureSafetyClass.BoundedExplanation);
        Assert.StartsWith("idle-reason://", evaluation.IdlePreservation.IdleReasonRef, StringComparison.Ordinal);
        Assert.False(string.IsNullOrWhiteSpace(caseId));
    }

    [Theory]
    [MemberData(nameof(RuntimeAndProductOverreachCases))]
    public void Evaluate_Refuses_Runtime_And_Product_Overreach(
        string caseId,
        ProductBoundaryReceipt receipt,
        string outcomeCode,
        LawfulLowerPathKind lowerPathKind,
        DisclosureSafetyClass disclosureSafety)
    {
        var evaluation = Evaluate(receipt, CompleteAuthorization());

        AssertNonAdmitted(
            evaluation,
            SliCmeActualRoundtripDisposition.Refused,
            outcomeCode,
            lowerPathKind,
            disclosureSafety);
        Assert.False(string.IsNullOrWhiteSpace(caseId));
    }

    [Theory]
    [MemberData(nameof(DoctrineRequestCases))]
    public void Evaluate_Routes_Doctrine_Requests_To_Lawful_Lower_Path(
        string caseId,
        CollapseAuthorizationState authorization,
        SliCmeActualRoundtripDisposition disposition,
        string outcomeCode,
        LawfulLowerPathKind lowerPathKind,
        DisclosureSafetyClass disclosureSafety)
    {
        var evaluation = Evaluate(
            FormationPathReferenceData.CanonicalProductBoundaryReceipt,
            authorization);

        AssertNonAdmitted(evaluation, disposition, outcomeCode, lowerPathKind, disclosureSafety);
        Assert.False(string.IsNullOrWhiteSpace(caseId));

        if (disclosureSafety == DisclosureSafetyClass.SilenceRequired)
        {
            Assert.Equal(DisclosureSafetyClass.SilenceRequired, evaluation.DisclosureSafety);
            Assert.NotEmpty(evaluation.FailureDeltaReceipt!.WithheldMechanismRefs);
        }
    }

    [Theory]
    [MemberData(nameof(FormationIntegrityFailureCases))]
    public void Evaluate_Refuses_Formation_Integrity_Failures(
        string caseId,
        ProductBoundaryReceipt receipt,
        string outcomeCode,
        LawfulLowerPathKind lowerPathKind,
        DisclosureSafetyClass disclosureSafety)
    {
        var evaluation = Evaluate(receipt, CompleteAuthorization());

        AssertNonAdmitted(
            evaluation,
            SliCmeActualRoundtripDisposition.Refused,
            outcomeCode,
            lowerPathKind,
            disclosureSafety);
        Assert.False(string.IsNullOrWhiteSpace(caseId));
    }

    [Theory]
    [MemberData(nameof(NonAdmittedInvariantCases))]
    public void Evaluate_Non_Admitted_Results_Emit_Delta_And_Do_Not_Leak_Or_Bypass(
        string caseId,
        ProductBoundaryReceipt receipt,
        CollapseAuthorizationState authorization,
        SliCmeActualRoundtripDisposition expectedDisposition,
        string outcomeCode)
    {
        var evaluation = Evaluate(receipt, authorization);

        Assert.Equal(expectedDisposition, evaluation.Disposition);
        Assert.Equal(outcomeCode, evaluation.OutcomeCode);
        Assert.False(evaluation.BoundedFutureWorkMayBeConsidered);
        Assert.NotNull(evaluation.FailureDeltaReceipt);
        Assert.StartsWith("formation-failure-delta://", evaluation.FailureDeltaReceipt!.DeltaHandle, StringComparison.Ordinal);
        Assert.StartsWith("internal-receipt://", evaluation.FailureDeltaReceipt.InternalReceiptRef, StringComparison.Ordinal);
        Assert.False(string.IsNullOrWhiteSpace(evaluation.FailureDeltaReceipt.NextConstraintRef));
        AssertIdlePreserved(evaluation);
        AssertNoProtectedMechanismDisclosure(evaluation);
        AssertNoGateBypass(evaluation);
        Assert.False(string.IsNullOrWhiteSpace(caseId));
    }

    public static IEnumerable<object[]> FireboxPreconditionCases()
    {
        yield return new object[]
        {
            "SW07-02-001",
            CompleteAuthorization() with { VaultBoundaryRef = null },
            "vault-boundary-required",
            LawfulLowerPathKind.VaultBoundaryFormationReview
        };
        yield return new object[]
        {
            "SW07-02-002",
            CompleteAuthorization() with { CSelfGelOrientationRef = null },
            "cselfgel-orientation-required",
            LawfulLowerPathKind.CSelfGelOrientationRequirement
        };
        yield return new object[]
        {
            "SW07-02-003",
            CompleteAuthorization() with { CVaultReceiptRef = null },
            "cvault-receipt-required",
            LawfulLowerPathKind.ReceiptRepairPath
        };
        yield return new object[]
        {
            "SW07-02-004",
            CompleteAuthorization() with { AdmittedWorkPresent = false },
            "admitted-work-required",
            LawfulLowerPathKind.IdleReceiptFormationReview
        };
        yield return new object[]
        {
            "SW07-02-005",
            CompleteAuthorization() with { CollapseConditionPresent = false },
            "collapse-condition-required",
            LawfulLowerPathKind.IdleReceiptFormationReview
        };
        yield return new object[]
        {
            "SW07-02-006",
            CompleteAuthorization() with { AuthorizationPresent = false },
            "authorization-required",
            LawfulLowerPathKind.WarrantCertificationOrIdle
        };
        yield return new object[]
        {
            "SW07-02-007",
            CompleteAuthorization() with { LawfulReceiptRef = null },
            "lawful-receipt-required",
            LawfulLowerPathKind.ReceiptRepairPath
        };
    }

    public static IEnumerable<object[]> RuntimeAndProductOverreachCases()
    {
        yield return new object[]
        {
            "SW07-03-001",
            FormationPathReferenceData.CanonicalProductBoundaryReceipt with { RuntimeIdentityEmitted = true },
            "runtime-identity-emission-blocked",
            LawfulLowerPathKind.FamiliarButNotHuman,
            DisclosureSafetyClass.BoundedExplanation
        };
        yield return new object[]
        {
            "SW07-03-002",
            FormationPathReferenceData.CanonicalProductBoundaryReceipt with { RuntimeActionExecuted = true },
            "runtime-action-execution-blocked",
            LawfulLowerPathKind.IdleReceiptFormationReview,
            DisclosureSafetyClass.PublicSafe
        };
        yield return new object[]
        {
            "SW07-03-003",
            FormationPathReferenceData.CanonicalProductBoundaryReceipt with { PublicOutputEmitted = true },
            "product-boundary-overreach-blocked",
            LawfulLowerPathKind.ProductBoundaryReceipt,
            DisclosureSafetyClass.PublicSafe
        };
        yield return new object[]
        {
            "SW07-03-004",
            FormationPathReferenceData.CanonicalProductBoundaryReceipt with { CanonicalProductClaimed = true },
            "product-boundary-overreach-blocked",
            LawfulLowerPathKind.ProductBoundaryReceipt,
            DisclosureSafetyClass.PublicSafe
        };
        yield return new object[]
        {
            "SW07-03-005",
            WithIntersection(FormationPathReferenceData.CanonicalPreproductionIntersection with { RuntimeActionAuthorized = true }),
            "runtime-action-authorization-blocked",
            LawfulLowerPathKind.IdleReceiptFormationReview,
            DisclosureSafetyClass.BoundedExplanation
        };
        yield return new object[]
        {
            "SW07-03-006",
            WithIntersection(FormationPathReferenceData.CanonicalPreproductionIntersection with { OutputAuthorityGranted = true }),
            "output-authority-premature-blocked",
            LawfulLowerPathKind.ProductBoundaryReceipt,
            DisclosureSafetyClass.PublicSafe
        };
    }

    public static IEnumerable<object[]> DoctrineRequestCases()
    {
        yield return new object[]
        {
            "SW07-04-001",
            CompleteAuthorization() with { PersonificationRequested = true },
            SliCmeActualRoundtripDisposition.Refused,
            "personification-not-promptable",
            LawfulLowerPathKind.FamiliarButNotHuman,
            DisclosureSafetyClass.BoundedExplanation
        };
        yield return new object[]
        {
            "SW07-04-002",
            CompleteAuthorization() with { SageAccessRequestedWithoutGates = true },
            SliCmeActualRoundtripDisposition.Withheld,
            "sage-gate-completion-required",
            LawfulLowerPathKind.CertificationTraining,
            DisclosureSafetyClass.PublicSafe
        };
        yield return new object[]
        {
            "SW07-04-003",
            CompleteAuthorization() with { RawCrypticRequested = true },
            SliCmeActualRoundtripDisposition.Refused,
            "raw-cryptic-request-warrant-required",
            LawfulLowerPathKind.StewardWarrantOrRefusal,
            DisclosureSafetyClass.SilenceRequired
        };
    }

    public static IEnumerable<object[]> FormationIntegrityFailureCases()
    {
        yield return new object[]
        {
            "SW07-05-001",
            FormationPathReferenceData.CanonicalProductBoundaryReceipt with { Intersection = null! },
            "preproduction-intersection-required",
            LawfulLowerPathKind.FormationReview,
            DisclosureSafetyClass.PublicSafe
        };
        yield return new object[]
        {
            "SW07-05-002",
            WithShellLaw(FormationPathReferenceData.CanonicalShellLaw with { IuttPosture = "iutt-equivalent-proof" }),
            "shell-law-overclaim-blocked",
            LawfulLowerPathKind.FormationReview,
            DisclosureSafetyClass.PublicSafe
        };
        yield return new object[]
        {
            "SW07-05-003",
            WithObstruction(FormationPathReferenceData.CanonicalObstruction with { Severity = FormationObstructionSeverity.Blocking }),
            "blocking-obstruction-present",
            LawfulLowerPathKind.FormationReview,
            DisclosureSafetyClass.PublicSafe
        };
        yield return new object[]
        {
            "SW07-05-004",
            WithResidue(FormationPathReferenceData.CanonicalResidue with { SilentRepairAttempted = true }),
            "silent-repair-attempt-blocked",
            LawfulLowerPathKind.ReceiptRepairPath,
            DisclosureSafetyClass.WithholdProtectedMechanism
        };
        yield return new object[]
        {
            "SW07-05-005",
            FormationPathReferenceData.CanonicalProductBoundaryReceipt with { NonActivation = DriftState() },
            "non-activation-drift-blocked",
            LawfulLowerPathKind.ReceiptRepairPath,
            DisclosureSafetyClass.BoundedExplanation
        };
    }

    public static IEnumerable<object[]> NonAdmittedInvariantCases()
    {
        foreach (var row in FireboxPreconditionCases())
        {
            yield return new object[]
            {
                row[0],
                FormationPathReferenceData.CanonicalProductBoundaryReceipt,
                row[1],
                SliCmeActualRoundtripDisposition.Withheld,
                row[2]
            };
        }

        foreach (var row in RuntimeAndProductOverreachCases())
        {
            yield return new object[]
            {
                row[0],
                row[1],
                CompleteAuthorization(),
                SliCmeActualRoundtripDisposition.Refused,
                row[2]
            };
        }

        foreach (var row in DoctrineRequestCases())
        {
            yield return new object[]
            {
                row[0],
                FormationPathReferenceData.CanonicalProductBoundaryReceipt,
                row[1],
                row[2],
                row[3]
            };
        }

        foreach (var row in FormationIntegrityFailureCases())
        {
            yield return new object[]
            {
                row[0],
                row[1],
                CompleteAuthorization(),
                SliCmeActualRoundtripDisposition.Refused,
                row[2]
            };
        }
    }

    private static CollapseAuthorizationState CompleteAuthorization() => new(
        VaultBoundaryRef: "vault-boundary://canonical",
        CSelfGelOrientationRef: "cselfgel-orientation://canonical",
        CVaultReceiptRef: "cvault-receipt://canonical",
        AdmittedWorkPresent: true,
        CollapseConditionPresent: true,
        AuthorizationPresent: true,
        LawfulReceiptRef: "lawful-receipt://canonical",
        PersonificationRequested: false,
        SageAccessRequestedWithoutGates: false,
        RawCrypticRequested: false);

    private static FormationPathRefusalEvaluation Evaluate(
        ProductBoundaryReceipt receipt,
        CollapseAuthorizationState authorization)
    {
        var policy = new DefaultFormationPathRefusalPolicy();
        return policy.Evaluate(receipt, authorization, TimestampUtc);
    }

    private static void AssertAdmitted(FormationPathRefusalEvaluation evaluation, string outcomeCode)
    {
        Assert.Equal(SliCmeActualRoundtripDisposition.Admitted, evaluation.Disposition);
        Assert.True(evaluation.IsAdmitted);
        Assert.Equal(outcomeCode, evaluation.OutcomeCode);
        Assert.True(evaluation.BoundedFutureWorkMayBeConsidered);
        Assert.Null(evaluation.FailureDeltaReceipt);
        AssertIdlePreserved(evaluation);
    }

    private static void AssertNonAdmitted(
        FormationPathRefusalEvaluation evaluation,
        SliCmeActualRoundtripDisposition disposition,
        string outcomeCode,
        LawfulLowerPathKind lowerPathKind,
        DisclosureSafetyClass disclosureSafety)
    {
        Assert.Equal(disposition, evaluation.Disposition);
        Assert.False(evaluation.IsAdmitted);
        Assert.Equal(outcomeCode, evaluation.OutcomeCode);
        Assert.Equal(disclosureSafety, evaluation.DisclosureSafety);
        Assert.False(evaluation.BoundedFutureWorkMayBeConsidered);
        Assert.NotNull(evaluation.FailureDeltaReceipt);
        Assert.Equal(disclosureSafety, evaluation.FailureDeltaReceipt!.DisclosureSafety);
        Assert.False(string.IsNullOrWhiteSpace(evaluation.FailureDeltaReceipt.SafeExplanation));
        Assert.StartsWith("next-constraint://", evaluation.FailureDeltaReceipt.NextConstraintRef, StringComparison.Ordinal);
        AssertLowerPath(evaluation, lowerPathKind);
        AssertIdlePreserved(evaluation);
        AssertNoProtectedMechanismDisclosure(evaluation);
        AssertNoGateBypass(evaluation);
    }

    private static void AssertLowerPath(
        FormationPathRefusalEvaluation evaluation,
        LawfulLowerPathKind expectedLowerPathKind)
    {
        Assert.NotNull(evaluation.LawfulLowerPathRoute);
        Assert.Equal(expectedLowerPathKind, evaluation.LawfulLowerPathRoute!.RouteKind);
        Assert.True(evaluation.LawfulLowerPathRoute.IsLawful);
        Assert.StartsWith("lower-path://", evaluation.LawfulLowerPathRoute.RouteRef, StringComparison.Ordinal);
        Assert.False(string.IsNullOrWhiteSpace(evaluation.LawfulLowerPathRoute.SafeExplanation));
    }

    private static void AssertIdlePreserved(FormationPathRefusalEvaluation evaluation)
    {
        Assert.True(evaluation.PreservesIdle);
        Assert.True(evaluation.IdlePreservation.RtmeRemainsIdle);
        Assert.False(evaluation.IdlePreservation.EcStartAllowed);
        Assert.False(evaluation.IdlePreservation.RuntimeActionAllowed);
        Assert.False(evaluation.IdlePreservation.RuntimeIdentityAllowed);
        Assert.False(string.IsNullOrWhiteSpace(evaluation.IdlePreservation.IdleReasonRef));
    }

    private static void AssertNoGateBypass(FormationPathRefusalEvaluation evaluation)
    {
        if (evaluation.LawfulLowerPathRoute is not null)
        {
            Assert.False(evaluation.LawfulLowerPathRoute.GateBypassAllowed);
        }
    }

    private static void AssertNoProtectedMechanismDisclosure(FormationPathRefusalEvaluation evaluation)
    {
        if (evaluation.LawfulLowerPathRoute is not null)
        {
            Assert.DoesNotContain("protected-mechanism://", evaluation.LawfulLowerPathRoute.SafeExplanation, StringComparison.OrdinalIgnoreCase);
        }

        if (evaluation.FailureDeltaReceipt is not null)
        {
            Assert.DoesNotContain("protected-mechanism://", evaluation.FailureDeltaReceipt.SafeExplanation, StringComparison.OrdinalIgnoreCase);
            Assert.All(
                evaluation.FailureDeltaReceipt.WithheldMechanismRefs,
                mechanismRef => Assert.StartsWith("protected-mechanism://", mechanismRef, StringComparison.Ordinal));
        }
    }

    private static ProductBoundaryReceipt WithIntersection(PreproductionIntersection intersection) =>
        FormationPathReferenceData.CanonicalProductBoundaryReceipt with
        {
            Intersection = intersection
        };

    private static ProductBoundaryReceipt WithShellLaw(FormationShellLawDescriptor shellLaw) =>
        WithIntersection(FormationPathReferenceData.CanonicalPreproductionIntersection with
        {
            ShellLaw = shellLaw
        });

    private static ProductBoundaryReceipt WithResidue(FormationResidue residue) =>
        WithIntersection(FormationPathReferenceData.CanonicalPreproductionIntersection with
        {
            Residues = new[] { residue }
        });

    private static ProductBoundaryReceipt WithObstruction(FormationObstruction obstruction) =>
        WithIntersection(FormationPathReferenceData.CanonicalPreproductionIntersection with
        {
            Obstructions = new[] { obstruction }
        });

    private static NonActivationState DriftState() =>
        FormationPathReferenceData.InertState with
        {
            EcStartRequested = true
        };
}
