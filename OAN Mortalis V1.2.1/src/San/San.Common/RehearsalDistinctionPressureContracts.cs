using System.Security.Cryptography;
using System.Text;

namespace San.Common;

public enum RehearsalDistinctionPressureDisposition
{
    MeasuredCold = 0,
    EmptyPressureCold = 1,
    Refused = 2
}

public sealed record RehearsalPressureVector(
    decimal PossibilityDensity,
    decimal SuccessPressure,
    decimal FailurePressure,
    decimal AmbiguityPressure,
    decimal ConfidencePressure,
    decimal UrgencyPressure,
    decimal IdentityDriftPressure,
    decimal WitnessDisagreementPressure)
{
    public bool IsColdVector =>
        IsUnit(PossibilityDensity) &&
        IsUnit(SuccessPressure) &&
        IsUnit(FailurePressure) &&
        IsUnit(AmbiguityPressure) &&
        IsUnit(ConfidencePressure) &&
        IsUnit(UrgencyPressure) &&
        IsUnit(IdentityDriftPressure) &&
        IsUnit(WitnessDisagreementPressure);

    public decimal MaximumPressure => new[]
    {
        PossibilityDensity,
        SuccessPressure,
        FailurePressure,
        AmbiguityPressure,
        ConfidencePressure,
        UrgencyPressure,
        IdentityDriftPressure,
        WitnessDisagreementPressure
    }.Max();

    private static bool IsUnit(decimal value) => value is >= 0m and <= 1m;
}

public sealed record RehearsalDistinctionPressureCase(
    string PressureHandle,
    string SourceRehearsalHandle,
    string SourceResidueHandle,
    string CandidateSplineHandle,
    string SourceReadinessHandle,
    string SourcePacketHandle,
    string SourceDryRunPlanHandle,
    string ScenarioHandle,
    string OutcomeInterpretationHandle,
    string CoolingHandle,
    string CustodyOwner,
    string WitnessHandle,
    string TelemetryRoute,
    string StewardReviewHandle,
    int BranchCount,
    int SuccessCount,
    int FailureCount,
    int AmbiguityCount,
    int RecurrenceCount,
    RehearsalPressureVector PressureVector,
    bool ReviewOnly,
    bool PressureOnly,
    bool EvidenceOnly,
    bool CoolingRequired,
    bool WitnessRequired,
    bool PreservesDryRunLineage,
    bool PreservesResidueLineage,
    bool PreservesCandidateSplineLineage,
    bool AuthorityAbsent,
    bool SuccessBecomesPermission,
    bool ConfidenceBecomesAuthority,
    bool RepetitionBecomesWarrant,
    bool FailureBecomesInvalidation,
    bool AmbiguityBecomesVictory,
    bool UrgencyBecomesJurisdiction,
    bool ImaginedFutureBecomesEnactedState,
    bool IdentityDriftMutatesCorePosture,
    bool PressureAuthorizesAction,
    bool PressureAdmitsContinuity,
    bool PressureEvaluatesLisp,
    bool PressureEmitsMembranePacket,
    bool PressureReplaysReceipt,
    bool PressureIncrementsPassage,
    bool PressureActivates)
{
    public bool IsColdPressureCase =>
        !string.IsNullOrWhiteSpace(PressureHandle) &&
        !string.IsNullOrWhiteSpace(SourceRehearsalHandle) &&
        !string.IsNullOrWhiteSpace(SourceResidueHandle) &&
        !string.IsNullOrWhiteSpace(CandidateSplineHandle) &&
        !string.IsNullOrWhiteSpace(SourceReadinessHandle) &&
        !string.IsNullOrWhiteSpace(SourcePacketHandle) &&
        !string.IsNullOrWhiteSpace(SourceDryRunPlanHandle) &&
        !string.IsNullOrWhiteSpace(ScenarioHandle) &&
        !string.IsNullOrWhiteSpace(OutcomeInterpretationHandle) &&
        !string.IsNullOrWhiteSpace(CoolingHandle) &&
        !string.IsNullOrWhiteSpace(CustodyOwner) &&
        !string.IsNullOrWhiteSpace(WitnessHandle) &&
        !string.IsNullOrWhiteSpace(TelemetryRoute) &&
        !string.IsNullOrWhiteSpace(StewardReviewHandle) &&
        BranchCount > 0 &&
        SuccessCount >= 0 &&
        FailureCount >= 0 &&
        AmbiguityCount >= 0 &&
        SuccessCount + FailureCount + AmbiguityCount <= BranchCount &&
        RecurrenceCount > 0 &&
        PressureVector.IsColdVector &&
        ReviewOnly &&
        PressureOnly &&
        EvidenceOnly &&
        CoolingRequired &&
        WitnessRequired &&
        PreservesDryRunLineage &&
        PreservesResidueLineage &&
        PreservesCandidateSplineLineage &&
        AuthorityAbsent &&
        !SuccessBecomesPermission &&
        !ConfidenceBecomesAuthority &&
        !RepetitionBecomesWarrant &&
        !FailureBecomesInvalidation &&
        !AmbiguityBecomesVictory &&
        !UrgencyBecomesJurisdiction &&
        !ImaginedFutureBecomesEnactedState &&
        !IdentityDriftMutatesCorePosture &&
        !PressureAuthorizesAction &&
        !PressureAdmitsContinuity &&
        !PressureEvaluatesLisp &&
        !PressureEmitsMembranePacket &&
        !PressureReplaysReceipt &&
        !PressureIncrementsPassage &&
        !PressureActivates;
}

public sealed record RehearsalPressureCoolingRoute(
    string CoolingRouteHandle,
    string PressureHandle,
    string SourceRehearsalHandle,
    string SourceResidueHandle,
    string CandidateSplineHandle,
    string StewardSurface,
    string EvidenceHandle,
    string WitnessHandle,
    string TelemetryRoute,
    string ReturnPathHandle,
    bool ReviewOnly,
    bool CoolingOnly,
    bool PreservesPressureLineage,
    bool PreservesRehearsalLineage,
    bool PreservesResidueLineage,
    bool PreservesCandidateSplineLineage,
    bool RoutesToStewardCoolingReview,
    bool RequiresCompassCooling,
    bool RouteGrantsAuthority,
    bool RouteAuthorizesAction,
    bool RouteAdmitsContinuity,
    bool RouteMutatesIdentity,
    bool RouteEvaluatesLisp,
    bool RouteEmitsMembranePacket,
    bool RouteReplaysReceipt,
    bool RouteIncrementsPassage,
    bool RouteActivates)
{
    public bool IsColdCoolingRoute =>
        !string.IsNullOrWhiteSpace(CoolingRouteHandle) &&
        !string.IsNullOrWhiteSpace(PressureHandle) &&
        !string.IsNullOrWhiteSpace(SourceRehearsalHandle) &&
        !string.IsNullOrWhiteSpace(SourceResidueHandle) &&
        !string.IsNullOrWhiteSpace(CandidateSplineHandle) &&
        !string.IsNullOrWhiteSpace(StewardSurface) &&
        !string.IsNullOrWhiteSpace(EvidenceHandle) &&
        !string.IsNullOrWhiteSpace(WitnessHandle) &&
        !string.IsNullOrWhiteSpace(TelemetryRoute) &&
        !string.IsNullOrWhiteSpace(ReturnPathHandle) &&
        ReviewOnly &&
        CoolingOnly &&
        PreservesPressureLineage &&
        PreservesRehearsalLineage &&
        PreservesResidueLineage &&
        PreservesCandidateSplineLineage &&
        RoutesToStewardCoolingReview &&
        RequiresCompassCooling &&
        !RouteGrantsAuthority &&
        !RouteAuthorizesAction &&
        !RouteAdmitsContinuity &&
        !RouteMutatesIdentity &&
        !RouteEvaluatesLisp &&
        !RouteEmitsMembranePacket &&
        !RouteReplaysReceipt &&
        !RouteIncrementsPassage &&
        !RouteActivates;
}

public sealed record RehearsalDistinctionPressureScopeBoundary(
    string BoundaryCode,
    bool Present,
    bool ReviewOnly,
    bool AllowsRehearsalPressureMeasurement,
    bool RequiresDryRunReceipt,
    bool RequiresEcPrecipitationWitnessReceipt,
    bool RequiresPressureVector,
    bool RequiresCooling,
    bool RequiresWitness,
    bool RequiresLineage,
    bool RequiresAuthorityAbsence,
    bool AllowsSuccessAsPermission,
    bool AllowsConfidenceAsAuthority,
    bool AllowsRepetitionAsWarrant,
    bool AllowsFailureAsInvalidation,
    bool AllowsAmbiguityAsVictory,
    bool AllowsUrgencyAsJurisdiction,
    bool AllowsImaginedFutureAsEnactedState,
    bool AllowsIdentityDriftMutation,
    bool AllowsActionAuthorization,
    bool AllowsContinuityAdmission,
    bool AllowsLispEvaluation,
    bool AllowsMembranePacketEmission,
    bool AllowsReceiptReplay,
    bool AllowsPassageIncrement,
    bool AllowsActivation)
{
    public bool IsColdScope =>
        !string.IsNullOrWhiteSpace(BoundaryCode) &&
        Present &&
        ReviewOnly &&
        AllowsRehearsalPressureMeasurement &&
        RequiresDryRunReceipt &&
        RequiresEcPrecipitationWitnessReceipt &&
        RequiresPressureVector &&
        RequiresCooling &&
        RequiresWitness &&
        RequiresLineage &&
        RequiresAuthorityAbsence &&
        !AllowsSuccessAsPermission &&
        !AllowsConfidenceAsAuthority &&
        !AllowsRepetitionAsWarrant &&
        !AllowsFailureAsInvalidation &&
        !AllowsAmbiguityAsVictory &&
        !AllowsUrgencyAsJurisdiction &&
        !AllowsImaginedFutureAsEnactedState &&
        !AllowsIdentityDriftMutation &&
        !AllowsActionAuthorization &&
        !AllowsContinuityAdmission &&
        !AllowsLispEvaluation &&
        !AllowsMembranePacketEmission &&
        !AllowsReceiptReplay &&
        !AllowsPassageIncrement &&
        !AllowsActivation;
}

public sealed record RehearsalDistinctionNonAuthorityBoundary(
    string BoundaryLaw,
    bool PressureMayManufactureLegitimacy,
    bool UrgencyMayCreateJurisdiction,
    bool ConfidenceMayGrantAuthority,
    bool SuccessMayCreatePermission,
    bool RepetitionMayCreateWarrant,
    bool FailureMayInvalidateSelf,
    bool AmbiguityMayCollapseToVictory,
    bool ImaginedFutureMayBecomeEnactedState,
    bool IdentityDriftPressureMayMutateCorePosture,
    bool PressureMayAuthorizeAction,
    bool PressureMayAdmitContinuity,
    bool PressureMayEvaluateLisp,
    bool PressureMayEmitMembranePacket,
    bool PressureMayReplayReceipt,
    bool PressureMayIncrementPassage,
    bool PressureMayActivate,
    bool RequiresCooling,
    bool RequiresWitnessRetention,
    bool RequiresAuthorityAbsence)
{
    public bool IsColdNonAuthorityBoundary =>
        !string.IsNullOrWhiteSpace(BoundaryLaw) &&
        !PressureMayManufactureLegitimacy &&
        !UrgencyMayCreateJurisdiction &&
        !ConfidenceMayGrantAuthority &&
        !SuccessMayCreatePermission &&
        !RepetitionMayCreateWarrant &&
        !FailureMayInvalidateSelf &&
        !AmbiguityMayCollapseToVictory &&
        !ImaginedFutureMayBecomeEnactedState &&
        !IdentityDriftPressureMayMutateCorePosture &&
        !PressureMayAuthorizeAction &&
        !PressureMayAdmitContinuity &&
        !PressureMayEvaluateLisp &&
        !PressureMayEmitMembranePacket &&
        !PressureMayReplayReceipt &&
        !PressureMayIncrementPassage &&
        !PressureMayActivate &&
        RequiresCooling &&
        RequiresWitnessRetention &&
        RequiresAuthorityAbsence;
}

public sealed record RehearsalDistinctionPressureRefusalReceipt(
    string ReceiptHandle,
    string RefusalCode,
    string GovernanceTrace,
    bool Retained);

public sealed record RehearsalDistinctionPressureRequest(
    EnactmentDryRunRehearsalReceipt? SourceDryRunReceipt,
    EcPrecipitationWitnessReceipt? SourceEcWitnessReceipt,
    IReadOnlyList<RehearsalDistinctionPressureCase> PressureCases,
    IReadOnlyList<RehearsalPressureCoolingRoute> CoolingRoutes,
    RehearsalDistinctionPressureScopeBoundary ScopeBoundary,
    RehearsalDistinctionNonAuthorityBoundary NonAuthorityBoundary,
    int PriorPassageCount);

public sealed record RehearsalDistinctionPressureReceipt(
    string ReceiptHandle,
    RehearsalDistinctionPressureDisposition Disposition,
    string OutcomeCode,
    string GovernanceTrace,
    string SourceDryRunReceiptHandle,
    string SourceEcWitnessReceiptHandle,
    IReadOnlyList<RehearsalDistinctionPressureCase> PressureCases,
    IReadOnlyList<RehearsalPressureCoolingRoute> CoolingRoutes,
    RehearsalDistinctionPressureScopeBoundary ScopeBoundary,
    RehearsalDistinctionNonAuthorityBoundary NonAuthorityBoundary,
    RehearsalDistinctionPressureRefusalReceipt? Refusal,
    int PriorPassageCount,
    int PassageCountAfterPressure,
    int RetainedPressureCaseCount,
    decimal MaximumObservedPressure,
    bool ReviewOnly,
    bool PressureOnly,
    bool EvidenceOnly,
    bool CoolingRequired,
    bool AuthorityAbsent,
    bool PressureManufacturedLegitimacy,
    bool UrgencyCreatedJurisdiction,
    bool ConfidenceGrantedAuthority,
    bool SuccessCreatedPermission,
    bool RepetitionCreatedWarrant,
    bool FailureInvalidatedSelf,
    bool AmbiguityCollapsedToVictory,
    bool ImaginedFutureBecameEnactedState,
    bool IdentityDriftMutatedCorePosture,
    bool PressureAuthorizedAction,
    bool PressureAdmittedContinuity,
    bool LispEvaluationAllowed,
    bool NewMembranePacketEmitted,
    bool ReceiptsReplayed,
    bool AuthorityGranted,
    bool ActivationRefused,
    DateTimeOffset TimestampUtc)
{
    public bool IsColdRehearsalDistinctionPressure =>
        (Disposition is RehearsalDistinctionPressureDisposition.MeasuredCold or
            RehearsalDistinctionPressureDisposition.EmptyPressureCold) &&
        Refusal is null &&
        ReviewOnly &&
        PressureOnly &&
        EvidenceOnly &&
        CoolingRequired &&
        AuthorityAbsent &&
        PassageCountAfterPressure == PriorPassageCount &&
        RetainedPressureCaseCount == PressureCases.Count &&
        MaximumObservedPressure is >= 0m and <= 1m &&
        !PressureManufacturedLegitimacy &&
        !UrgencyCreatedJurisdiction &&
        !ConfidenceGrantedAuthority &&
        !SuccessCreatedPermission &&
        !RepetitionCreatedWarrant &&
        !FailureInvalidatedSelf &&
        !AmbiguityCollapsedToVictory &&
        !ImaginedFutureBecameEnactedState &&
        !IdentityDriftMutatedCorePosture &&
        !PressureAuthorizedAction &&
        !PressureAdmittedContinuity &&
        !LispEvaluationAllowed &&
        !NewMembranePacketEmitted &&
        !ReceiptsReplayed &&
        !AuthorityGranted &&
        ActivationRefused &&
        ScopeBoundary.IsColdScope &&
        NonAuthorityBoundary.IsColdNonAuthorityBoundary;

    public bool IsRetainedRehearsalDistinctionPressureRefusal =>
        Disposition == RehearsalDistinctionPressureDisposition.Refused &&
        Refusal?.Retained == true &&
        PassageCountAfterPressure == PriorPassageCount &&
        RetainedPressureCaseCount == 0 &&
        MaximumObservedPressure == 0m &&
        !PressureManufacturedLegitimacy &&
        !UrgencyCreatedJurisdiction &&
        !ConfidenceGrantedAuthority &&
        !SuccessCreatedPermission &&
        !RepetitionCreatedWarrant &&
        !FailureInvalidatedSelf &&
        !AmbiguityCollapsedToVictory &&
        !ImaginedFutureBecameEnactedState &&
        !IdentityDriftMutatedCorePosture &&
        !PressureAuthorizedAction &&
        !PressureAdmittedContinuity &&
        !LispEvaluationAllowed &&
        !NewMembranePacketEmitted &&
        !ReceiptsReplayed &&
        !AuthorityGranted &&
        ActivationRefused;
}

public sealed class DefaultRehearsalDistinctionPressureBoundaryValidator
{
    public RehearsalDistinctionPressureReceipt Declare(
        RehearsalDistinctionPressureRequest request,
        DateTimeOffset timestampUtc)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.SourceDryRunReceipt is null ||
            !request.SourceDryRunReceipt.IsColdEnactmentDryRunRehearsal)
        {
            return Refuse(
                request,
                "rehearsal-pressure-source-dry-run-missing",
                "Rehearsal distinction pressure refused because pressure may be measured only from a cold enactment dry-run rehearsal receipt.",
                timestampUtc);
        }

        if (request.SourceEcWitnessReceipt is null ||
            !request.SourceEcWitnessReceipt.IsColdEcPrecipitationWitness)
        {
            return Refuse(
                request,
                "rehearsal-pressure-source-ec-witness-missing",
                "Rehearsal distinction pressure refused because pressure may be measured only after cold EC precipitation witness preserves candidate-only residue.",
                timestampUtc);
        }

        if (!string.Equals(
                request.SourceEcWitnessReceipt.SourceDryRunReceiptHandle,
                request.SourceDryRunReceipt.ReceiptHandle,
                StringComparison.Ordinal))
        {
            return Refuse(
                request,
                "rehearsal-pressure-source-linkage-invalid",
                "Rehearsal distinction pressure refused because the EC witness receipt must reconstruct from the same dry-run receipt being pressure-measured.",
                timestampUtc);
        }

        if (request.ScopeBoundary is null ||
            !request.ScopeBoundary.Present ||
            string.IsNullOrWhiteSpace(request.ScopeBoundary.BoundaryCode))
        {
            return Refuse(
                request,
                "rehearsal-pressure-scope-missing",
                "Rehearsal distinction pressure refused because a review-only pressure scope boundary is required.",
                timestampUtc);
        }

        if (!request.ScopeBoundary.IsColdScope)
        {
            return Refuse(
                request,
                "rehearsal-pressure-scope-promotional",
                "Rehearsal distinction pressure refused because scope may measure pressure only while refusing success as permission, confidence as authority, repetition as warrant, failure as invalidation, ambiguity as victory, urgency as jurisdiction, imagined future as enacted state, identity drift mutation, action, continuity, Lisp evaluation, packet emission, replay, passage, or activation.",
                timestampUtc);
        }

        if (request.NonAuthorityBoundary is null ||
            !request.NonAuthorityBoundary.IsColdNonAuthorityBoundary)
        {
            return Refuse(
                request,
                "rehearsal-pressure-non-authority-invalid",
                "Rehearsal distinction pressure refused because pressure law must retain authority absence while requiring cooling, witness retention, and refusal of pressure as legitimacy.",
                timestampUtc);
        }

        if (request.PressureCases.Any(static pressure => !pressure.IsColdPressureCase))
        {
            return Refuse(
                request,
                "rehearsal-pressure-case-invalid",
                "Rehearsal distinction pressure refused because every pressure case must remain review-only, evidence-only, cooling-routed, lineage-preserving, authority-absent, and non-authorizing.",
                timestampUtc);
        }

        if (HasDuplicate(request.PressureCases.Select(static pressure => pressure.PressureHandle)))
        {
            return Refuse(
                request,
                "rehearsal-pressure-duplicate-pressure-handle",
                "Rehearsal distinction pressure refused because duplicate pressure handles would collapse pressure lineage.",
                timestampUtc);
        }

        var dryRunCases = request.SourceDryRunReceipt.DryRunCases
            .ToDictionary(static item => item.RehearsalHandle, StringComparer.Ordinal);
        var residues = request.SourceEcWitnessReceipt.ResidueCandidates
            .ToDictionary(static item => item.ResidueHandle, StringComparer.Ordinal);

        if (request.PressureCases.Any(pressure =>
                !dryRunCases.TryGetValue(pressure.SourceRehearsalHandle, out var dryRun) ||
                !residues.TryGetValue(pressure.SourceResidueHandle, out var residue) ||
                !string.Equals(residue.SourceRehearsalHandle, pressure.SourceRehearsalHandle, StringComparison.Ordinal) ||
                !string.Equals(residue.CandidateSplineHandle, pressure.CandidateSplineHandle, StringComparison.Ordinal) ||
                !string.Equals(pressure.SourceReadinessHandle, dryRun.SourceReadinessHandle, StringComparison.Ordinal) ||
                !string.Equals(pressure.SourcePacketHandle, dryRun.SourcePacketHandle, StringComparison.Ordinal) ||
                !string.Equals(pressure.SourceDryRunPlanHandle, dryRun.DryRunPlanHandle, StringComparison.Ordinal) ||
                !string.Equals(pressure.CustodyOwner, dryRun.CustodyOwner, StringComparison.Ordinal) ||
                !string.Equals(pressure.WitnessHandle, dryRun.WitnessHandle, StringComparison.Ordinal) ||
                !string.Equals(pressure.TelemetryRoute, dryRun.TelemetryRoute, StringComparison.Ordinal)))
        {
            return Refuse(
                request,
                "rehearsal-pressure-lineage-invalid",
                "Rehearsal distinction pressure refused because every pressure case must preserve dry-run, EC residue, candidate spline, readiness, packet, plan, custody, witness, and telemetry lineage.",
                timestampUtc);
        }

        var pressureCasesByHandle = request.PressureCases
            .ToDictionary(static item => item.PressureHandle, StringComparer.Ordinal);

        if (request.CoolingRoutes.Any(route =>
                !route.IsColdCoolingRoute ||
                !pressureCasesByHandle.TryGetValue(route.PressureHandle, out var pressure) ||
                !string.Equals(route.SourceRehearsalHandle, pressure.SourceRehearsalHandle, StringComparison.Ordinal) ||
                !string.Equals(route.SourceResidueHandle, pressure.SourceResidueHandle, StringComparison.Ordinal) ||
                !string.Equals(route.CandidateSplineHandle, pressure.CandidateSplineHandle, StringComparison.Ordinal) ||
                !string.Equals(route.WitnessHandle, pressure.WitnessHandle, StringComparison.Ordinal) ||
                !string.Equals(route.TelemetryRoute, pressure.TelemetryRoute, StringComparison.Ordinal)))
        {
            return Refuse(
                request,
                "rehearsal-pressure-cooling-route-invalid",
                "Rehearsal distinction pressure cooling refused because cooling routes must preserve pressure, rehearsal, residue, and candidate spline lineage without authority, action, continuity, identity mutation, Lisp evaluation, packet emission, replay, passage, or activation.",
                timestampUtc);
        }

        if (HasDuplicate(request.CoolingRoutes.Select(static route => route.CoolingRouteHandle)))
        {
            return Refuse(
                request,
                "rehearsal-pressure-duplicate-cooling-route",
                "Rehearsal distinction pressure refused because duplicate cooling route handles would collapse cooling lineage.",
                timestampUtc);
        }

        if (request.PressureCases.Count > 0 &&
            request.PressureCases.Any(pressure => !request.CoolingRoutes.Any(route =>
                string.Equals(route.PressureHandle, pressure.PressureHandle, StringComparison.Ordinal))))
        {
            return Refuse(
                request,
                "rehearsal-pressure-cooling-route-missing",
                "Rehearsal distinction pressure refused because every pressure case requires a cooling route before pressure may be retained.",
                timestampUtc);
        }

        var disposition = request.PressureCases.Count == 0
            ? RehearsalDistinctionPressureDisposition.EmptyPressureCold
            : RehearsalDistinctionPressureDisposition.MeasuredCold;
        var outcomeCode = disposition == RehearsalDistinctionPressureDisposition.EmptyPressureCold
            ? "rehearsal-pressure-empty-review-only"
            : "rehearsal-pressure-measured-review-only";
        var governanceTrace = disposition == RehearsalDistinctionPressureDisposition.EmptyPressureCold
            ? "Rehearsal distinction pressure found no pressure cases. Empty pressure remains review-only and does not create permission, warrant, authority, jurisdiction, action, continuity, Lisp evaluation, packet emission, replay, passage, or activation."
            : "Rehearsal distinction pressure measured possibility density, success, failure, ambiguity, confidence, urgency, identity drift, and witness disagreement as evidence-only cooling pressure while refusing pressure as legitimacy, urgency as jurisdiction, confidence as authority, success as permission, repetition as warrant, failure as invalidation, ambiguity as victory, imagined future as enacted state, action, continuity, Lisp evaluation, packet emission, replay, passage, or activation.";

        return CreateReceipt(
            request,
            disposition,
            outcomeCode,
            governanceTrace,
            refusal: null,
            timestampUtc);
    }

    private static RehearsalDistinctionPressureReceipt Refuse(
        RehearsalDistinctionPressureRequest request,
        string outcomeCode,
        string governanceTrace,
        DateTimeOffset timestampUtc) =>
        CreateReceipt(
            request,
            RehearsalDistinctionPressureDisposition.Refused,
            outcomeCode,
            governanceTrace,
            new RehearsalDistinctionPressureRefusalReceipt(
                ReceiptHandle: $"urn:san:rehearsal-pressure-refusal:{ShortHash(SourceDryRunHandle(request), SourceEcWitnessHandle(request), outcomeCode)}",
                RefusalCode: outcomeCode,
                GovernanceTrace: governanceTrace,
                Retained: true),
            timestampUtc);

    private static RehearsalDistinctionPressureReceipt CreateReceipt(
        RehearsalDistinctionPressureRequest request,
        RehearsalDistinctionPressureDisposition disposition,
        string outcomeCode,
        string governanceTrace,
        RehearsalDistinctionPressureRefusalReceipt? refusal,
        DateTimeOffset timestampUtc)
    {
        var maximumPressure = refusal is null && request.PressureCases.Count > 0
            ? request.PressureCases.Max(static pressure => pressure.PressureVector.MaximumPressure)
            : 0m;

        return new(
            ReceiptHandle: $"urn:san:rehearsal-pressure:{(refusal is null ? "review" : "refused")}:{ShortHash(SourceDryRunHandle(request), SourceEcWitnessHandle(request), outcomeCode, request.PressureCases.Count.ToString(System.Globalization.CultureInfo.InvariantCulture), timestampUtc.UtcTicks.ToString(System.Globalization.CultureInfo.InvariantCulture))}",
            Disposition: disposition,
            OutcomeCode: outcomeCode,
            GovernanceTrace: governanceTrace,
            SourceDryRunReceiptHandle: SourceDryRunHandle(request),
            SourceEcWitnessReceiptHandle: SourceEcWitnessHandle(request),
            PressureCases: refusal is null ? request.PressureCases.ToArray() : [],
            CoolingRoutes: refusal is null ? request.CoolingRoutes.ToArray() : [],
            ScopeBoundary: request.ScopeBoundary,
            NonAuthorityBoundary: request.NonAuthorityBoundary,
            Refusal: refusal,
            PriorPassageCount: request.PriorPassageCount,
            PassageCountAfterPressure: request.PriorPassageCount,
            RetainedPressureCaseCount: refusal is null ? request.PressureCases.Count : 0,
            MaximumObservedPressure: maximumPressure,
            ReviewOnly: true,
            PressureOnly: true,
            EvidenceOnly: true,
            CoolingRequired: true,
            AuthorityAbsent: true,
            PressureManufacturedLegitimacy: false,
            UrgencyCreatedJurisdiction: false,
            ConfidenceGrantedAuthority: false,
            SuccessCreatedPermission: false,
            RepetitionCreatedWarrant: false,
            FailureInvalidatedSelf: false,
            AmbiguityCollapsedToVictory: false,
            ImaginedFutureBecameEnactedState: false,
            IdentityDriftMutatedCorePosture: false,
            PressureAuthorizedAction: false,
            PressureAdmittedContinuity: false,
            LispEvaluationAllowed: false,
            NewMembranePacketEmitted: false,
            ReceiptsReplayed: false,
            AuthorityGranted: false,
            ActivationRefused: true,
            TimestampUtc: timestampUtc);
    }

    private static string SourceDryRunHandle(RehearsalDistinctionPressureRequest request) =>
        request.SourceDryRunReceipt?.ReceiptHandle ?? "missing-rehearsal-pressure-dry-run-source";

    private static string SourceEcWitnessHandle(RehearsalDistinctionPressureRequest request) =>
        request.SourceEcWitnessReceipt?.ReceiptHandle ?? "missing-rehearsal-pressure-ec-witness-source";

    private static bool HasDuplicate(IEnumerable<string> handles)
    {
        var seen = new HashSet<string>(StringComparer.Ordinal);
        foreach (var handle in handles)
        {
            if (!seen.Add(handle))
            {
                return true;
            }
        }

        return false;
    }

    private static string ShortHash(params string[] parts)
    {
        var payload = string.Join("|", parts);
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(payload));
        return Convert.ToHexString(bytes)[..16].ToLowerInvariant();
    }
}
