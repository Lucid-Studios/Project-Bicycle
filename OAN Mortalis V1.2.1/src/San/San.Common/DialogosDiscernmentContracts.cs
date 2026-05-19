using System.Security.Cryptography;
using System.Text;

namespace San.Common;

public enum DialogosDiscernmentDisposition
{
    RetainedForReviewCold = 0,
    SafeExplorationReturnedCold = 1,
    EmptyReviewCold = 2,
    Refused = 3
}

public enum ThoughtStatus
{
    AppearanceOnly = 0,
    Articulated = 1,
    Coherent = 2,
    Perspectival = 3,
    EvidenceSeeking = 4,
    WarrantSeeking = 5,
    SafeExplorationCandidate = 6
}

public sealed record DialogosThoughtForm(
    string ThoughtHandle,
    ThoughtStatus Status,
    string SourceSurface,
    string Statement,
    string PerspectiveRef,
    string EvidenceHandle,
    bool HasAppearance,
    bool ArticulationPresent,
    bool CoherenceClaimed,
    bool PerspectiveDeclared,
    bool EvidenceBodyPresent,
    bool WitnessBodyPresent,
    bool ReviewOnly,
    bool SafeExplorationRequested,
    bool TreatsAppearanceAsTruth,
    bool TreatsArticulationAsWarrant,
    bool TreatsCoherenceAsEvidence,
    bool TreatsAgreementAsAuthority,
    bool TreatsPerspectiveAsContinuity,
    bool TreatsRefusalAsObstruction,
    bool AuthorizesAction,
    bool MutatesIdentity,
    bool AdmitsContinuity,
    bool GrantsAuthority)
{
    public bool IsColdThoughtForm =>
        !string.IsNullOrWhiteSpace(ThoughtHandle) &&
        !string.IsNullOrWhiteSpace(SourceSurface) &&
        !string.IsNullOrWhiteSpace(Statement) &&
        !string.IsNullOrWhiteSpace(EvidenceHandle) &&
        HasAppearance &&
        EvidenceBodyPresent &&
        WitnessBodyPresent &&
        ReviewOnly &&
        !TreatsAppearanceAsTruth &&
        !TreatsArticulationAsWarrant &&
        !TreatsCoherenceAsEvidence &&
        !TreatsAgreementAsAuthority &&
        !TreatsPerspectiveAsContinuity &&
        !TreatsRefusalAsObstruction &&
        !AuthorizesAction &&
        !MutatesIdentity &&
        !AdmitsContinuity &&
        !GrantsAuthority &&
        HasStatusShape;

    private bool HasStatusShape =>
        Status switch
        {
            ThoughtStatus.AppearanceOnly => !ArticulationPresent && !CoherenceClaimed && !PerspectiveDeclared && !SafeExplorationRequested,
            ThoughtStatus.Articulated => ArticulationPresent,
            ThoughtStatus.Coherent => ArticulationPresent && CoherenceClaimed,
            ThoughtStatus.Perspectival => ArticulationPresent && CoherenceClaimed && PerspectiveDeclared && !string.IsNullOrWhiteSpace(PerspectiveRef),
            ThoughtStatus.EvidenceSeeking => ArticulationPresent && !SafeExplorationRequested,
            ThoughtStatus.WarrantSeeking => ArticulationPresent && CoherenceClaimed && PerspectiveDeclared && !string.IsNullOrWhiteSpace(PerspectiveRef),
            ThoughtStatus.SafeExplorationCandidate => ArticulationPresent && SafeExplorationRequested,
            _ => false
        };
}

public sealed record ArticulationSurface(
    string SurfaceHandle,
    string SourceThoughtHandle,
    string LanguageBody,
    string StatedContent,
    bool ProducedByModel,
    bool OperatorSupplied,
    bool ReviewOnly,
    bool TreatsFluencyAsTruth,
    bool TreatsRhetoricalForceAsWarrant,
    bool TreatsAgreementAsEvidence,
    bool GrantsAuthority,
    bool AdmitsContinuity)
{
    public bool IsColdArticulationSurface =>
        !string.IsNullOrWhiteSpace(SurfaceHandle) &&
        !string.IsNullOrWhiteSpace(SourceThoughtHandle) &&
        !string.IsNullOrWhiteSpace(LanguageBody) &&
        !string.IsNullOrWhiteSpace(StatedContent) &&
        (ProducedByModel || OperatorSupplied) &&
        ReviewOnly &&
        !TreatsFluencyAsTruth &&
        !TreatsRhetoricalForceAsWarrant &&
        !TreatsAgreementAsEvidence &&
        !GrantsAuthority &&
        !AdmitsContinuity;
}

public sealed record WarrantBoundary(
    string BoundaryCode,
    bool Present,
    bool ReviewOnly,
    bool EvidenceRequired,
    bool WitnessRequired,
    bool ReturnPathRequired,
    bool AllowsAppearanceAsTruth,
    bool AllowsArticulationAsWarrant,
    bool AllowsCoherenceAsEvidence,
    bool AllowsAgreementAsAuthority,
    bool AllowsPerspectiveAsContinuity,
    bool AllowsSafeExplorationAsAdmission,
    bool AllowsRefusalAsObstruction,
    bool AllowsRuntimeAction,
    bool AllowsAuthority,
    bool AllowsContinuityAdmission,
    bool AllowsIdentityMutation,
    bool AllowsLispEvaluation,
    bool AllowsPacketEmission,
    bool AllowsReceiptReplay,
    bool IncrementsPassageCount,
    bool AllowsActivation)
{
    public bool IsColdBoundary =>
        !string.IsNullOrWhiteSpace(BoundaryCode) &&
        Present &&
        ReviewOnly &&
        EvidenceRequired &&
        WitnessRequired &&
        ReturnPathRequired &&
        !AllowsAppearanceAsTruth &&
        !AllowsArticulationAsWarrant &&
        !AllowsCoherenceAsEvidence &&
        !AllowsAgreementAsAuthority &&
        !AllowsPerspectiveAsContinuity &&
        !AllowsSafeExplorationAsAdmission &&
        !AllowsRefusalAsObstruction &&
        !AllowsRuntimeAction &&
        !AllowsAuthority &&
        !AllowsContinuityAdmission &&
        !AllowsIdentityMutation &&
        !AllowsLispEvaluation &&
        !AllowsPacketEmission &&
        !AllowsReceiptReplay &&
        !IncrementsPassageCount &&
        !AllowsActivation;
}

public sealed record IntermediateChamberState(
    string ChamberHandle,
    string SourceThoughtHandle,
    string CompassRef,
    string MeaningShellRef,
    ThoughtStatus HeldStatus,
    bool TransitionalityAdmissible,
    bool Sovereign,
    bool ReviewOnly,
    bool CoolingPathPresent,
    bool ReturnPathPresent,
    bool WitnessRequired,
    bool PromotesToEngram,
    bool PromotesToSelfGel,
    bool AdmitsContinuity,
    bool GrantsAuthority,
    bool AuthorizesAction,
    bool EvaluatesLisp)
{
    public bool IsColdIntermediateChamber =>
        !string.IsNullOrWhiteSpace(ChamberHandle) &&
        !string.IsNullOrWhiteSpace(SourceThoughtHandle) &&
        !string.IsNullOrWhiteSpace(CompassRef) &&
        !string.IsNullOrWhiteSpace(MeaningShellRef) &&
        TransitionalityAdmissible &&
        !Sovereign &&
        ReviewOnly &&
        CoolingPathPresent &&
        ReturnPathPresent &&
        WitnessRequired &&
        !PromotesToEngram &&
        !PromotesToSelfGel &&
        !AdmitsContinuity &&
        !GrantsAuthority &&
        !AuthorizesAction &&
        !EvaluatesLisp;
}

public sealed record SafeExplorationLane(
    string LaneHandle,
    string SourceThoughtHandle,
    string ExplorationQuestion,
    string EvidenceNeed,
    string ReturnCondition,
    bool SafeToExplore,
    bool ReviewOnly,
    bool Admitted,
    bool AuthorizesAction,
    bool GrantsAuthority,
    bool AdmitsContinuity,
    bool EvaluatesLisp)
{
    public bool IsColdSafeExplorationLane =>
        !string.IsNullOrWhiteSpace(LaneHandle) &&
        !string.IsNullOrWhiteSpace(SourceThoughtHandle) &&
        !string.IsNullOrWhiteSpace(ExplorationQuestion) &&
        !string.IsNullOrWhiteSpace(EvidenceNeed) &&
        !string.IsNullOrWhiteSpace(ReturnCondition) &&
        SafeToExplore &&
        ReviewOnly &&
        !Admitted &&
        !AuthorizesAction &&
        !GrantsAuthority &&
        !AdmitsContinuity &&
        !EvaluatesLisp;
}

public sealed record ReturnPath(
    string ReturnHandle,
    string SourceThoughtHandle,
    string OperatorReturnPrompt,
    string EvidenceNeed,
    bool ReturnsWithoutAdmission,
    bool PreservesQuestion,
    bool RequiresEvidence,
    bool ReviewOnly,
    bool GrantsAuthority,
    bool AdmitsContinuity,
    bool AuthorizesAction)
{
    public bool IsColdReturnPath =>
        !string.IsNullOrWhiteSpace(ReturnHandle) &&
        !string.IsNullOrWhiteSpace(SourceThoughtHandle) &&
        !string.IsNullOrWhiteSpace(OperatorReturnPrompt) &&
        !string.IsNullOrWhiteSpace(EvidenceNeed) &&
        ReturnsWithoutAdmission &&
        PreservesQuestion &&
        RequiresEvidence &&
        ReviewOnly &&
        !GrantsAuthority &&
        !AdmitsContinuity &&
        !AuthorizesAction;
}

public sealed record PrincipledRefusalReceipt(
    string ReceiptHandle,
    string RefusalCode,
    string GovernanceTrace,
    string ReturnPathHandle,
    bool Retained);

public sealed record DialogosDiscernmentRequest(
    IReadOnlyList<DialogosThoughtForm> ThoughtForms,
    IReadOnlyList<ArticulationSurface> ArticulationSurfaces,
    IReadOnlyList<IntermediateChamberState> IntermediateChambers,
    IReadOnlyList<SafeExplorationLane> SafeExplorationLanes,
    IReadOnlyList<ReturnPath> ReturnPaths,
    WarrantBoundary WarrantBoundary,
    int PriorPassageCount);

public sealed record DialogosDiscernmentReceipt(
    string ReceiptHandle,
    DialogosDiscernmentDisposition Disposition,
    string OutcomeCode,
    string GovernanceTrace,
    IReadOnlyList<DialogosThoughtForm> ThoughtForms,
    IReadOnlyList<ArticulationSurface> ArticulationSurfaces,
    IReadOnlyList<IntermediateChamberState> IntermediateChambers,
    IReadOnlyList<SafeExplorationLane> SafeExplorationLanes,
    IReadOnlyList<ReturnPath> ReturnPaths,
    WarrantBoundary WarrantBoundary,
    PrincipledRefusalReceipt? Refusal,
    int PriorPassageCount,
    int PassageCountAfterDiscernment,
    bool ReviewOnly,
    bool SafeExplorationReturned,
    bool PrincipledRefusalRetained,
    bool ThoughtAppearanceBecameTruth,
    bool ArticulationGrantedWarrant,
    bool CoherenceBecameEvidence,
    bool AgreementGrantedAuthority,
    bool PerspectiveAdmittedContinuity,
    bool SafeExplorationAdmitted,
    bool RuntimeActionAllowed,
    bool LispEvaluationAllowed,
    bool NewPacketEmitted,
    bool ReceiptsReplayed,
    bool ContinuityAdmitted,
    bool AuthorityGranted,
    bool ActivationRefused,
    DateTimeOffset TimestampUtc)
{
    public bool IsColdDialogosDiscernment =>
        (Disposition is DialogosDiscernmentDisposition.RetainedForReviewCold or
            DialogosDiscernmentDisposition.SafeExplorationReturnedCold or
            DialogosDiscernmentDisposition.EmptyReviewCold) &&
        Refusal is null &&
        ReviewOnly &&
        PassageCountAfterDiscernment == PriorPassageCount &&
        !ThoughtAppearanceBecameTruth &&
        !ArticulationGrantedWarrant &&
        !CoherenceBecameEvidence &&
        !AgreementGrantedAuthority &&
        !PerspectiveAdmittedContinuity &&
        !SafeExplorationAdmitted &&
        !RuntimeActionAllowed &&
        !LispEvaluationAllowed &&
        !NewPacketEmitted &&
        !ReceiptsReplayed &&
        !ContinuityAdmitted &&
        !AuthorityGranted &&
        ActivationRefused &&
        WarrantBoundary.IsColdBoundary;

    public bool IsRetainedDialogosDiscernmentRefusal =>
        Disposition == DialogosDiscernmentDisposition.Refused &&
        Refusal?.Retained == true &&
        PassageCountAfterDiscernment == PriorPassageCount &&
        !SafeExplorationReturned &&
        !ThoughtAppearanceBecameTruth &&
        !ArticulationGrantedWarrant &&
        !CoherenceBecameEvidence &&
        !AgreementGrantedAuthority &&
        !PerspectiveAdmittedContinuity &&
        !SafeExplorationAdmitted &&
        !RuntimeActionAllowed &&
        !LispEvaluationAllowed &&
        !NewPacketEmitted &&
        !ReceiptsReplayed &&
        !ContinuityAdmitted &&
        !AuthorityGranted &&
        ActivationRefused;
}

public sealed class DefaultDialogosDiscernmentBoundaryValidator
{
    public DialogosDiscernmentReceipt Declare(
        DialogosDiscernmentRequest request,
        DateTimeOffset timestampUtc)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.WarrantBoundary is null ||
            !request.WarrantBoundary.Present ||
            string.IsNullOrWhiteSpace(request.WarrantBoundary.BoundaryCode))
        {
            return Refuse(
                request,
                "dialogos-discernment-warrant-boundary-missing",
                "Dialogos discernment refused because a review-only warrant boundary is required before thought status may be retained.",
                timestampUtc);
        }

        if (!request.WarrantBoundary.IsColdBoundary)
        {
            return Refuse(
                request,
                "dialogos-discernment-promotional-boundary",
                "Dialogos discernment refused because the warrant boundary must require evidence, witness, and return path while refusing appearance-as-truth, articulation-as-warrant, coherence-as-evidence, agreement-as-authority, perspective-as-continuity, safe exploration as admission, action, authority, continuity, identity mutation, Lisp evaluation, packet emission, replay, passage, and activation.",
                timestampUtc);
        }

        if (request.ThoughtForms.Any(static thought => !thought.IsColdThoughtForm))
        {
            return Refuse(
                request,
                "dialogos-discernment-thought-form-invalid",
                "Dialogos discernment refused because every thought form must remain witnessed, review-only, and unable to become truth, warrant, evidence, authority, continuity, action, identity mutation, or obstruction by itself.",
                timestampUtc);
        }

        var thoughtHandles = request.ThoughtForms
            .Select(static thought => thought.ThoughtHandle)
            .ToHashSet(StringComparer.Ordinal);

        if (thoughtHandles.Count != request.ThoughtForms.Count)
        {
            return Refuse(
                request,
                "dialogos-discernment-duplicate-thought-handle",
                "Dialogos discernment refused because duplicate thought handles would collapse thought-form lineage.",
                timestampUtc);
        }

        if (request.ArticulationSurfaces.Any(static surface => !surface.IsColdArticulationSurface) ||
            request.ArticulationSurfaces.Any(surface => !thoughtHandles.Contains(surface.SourceThoughtHandle)))
        {
            return Refuse(
                request,
                "dialogos-discernment-articulation-invalid",
                "Dialogos discernment refused because every articulation surface must bind to a known thought and refuse fluency-as-truth, rhetorical force as warrant, agreement as evidence, authority, and continuity.",
                timestampUtc);
        }

        if (HasDuplicate(request.ArticulationSurfaces.Select(static surface => surface.SurfaceHandle)))
        {
            return Refuse(
                request,
                "dialogos-discernment-duplicate-articulation-handle",
                "Dialogos discernment refused because duplicate articulation handles would collapse language-body lineage.",
                timestampUtc);
        }

        if (request.IntermediateChambers.Any(static chamber => !chamber.IsColdIntermediateChamber) ||
            request.IntermediateChambers.Any(chamber => !thoughtHandles.Contains(chamber.SourceThoughtHandle)))
        {
            return Refuse(
                request,
                "dialogos-discernment-intermediate-chamber-invalid",
                "Dialogos discernment refused because intermediate chambers must hold transitionality as admissible without sovereignty, engram promotion, SelfGEL promotion, authority, continuity, action, or Lisp evaluation.",
                timestampUtc);
        }

        if (HasDuplicate(request.IntermediateChambers.Select(static chamber => chamber.ChamberHandle)))
        {
            return Refuse(
                request,
                "dialogos-discernment-duplicate-chamber-handle",
                "Dialogos discernment refused because duplicate intermediate chamber handles would collapse chamber lineage.",
                timestampUtc);
        }

        if (request.SafeExplorationLanes.Any(static lane => !lane.IsColdSafeExplorationLane) ||
            request.SafeExplorationLanes.Any(lane => !thoughtHandles.Contains(lane.SourceThoughtHandle)))
        {
            return Refuse(
                request,
                "dialogos-discernment-safe-exploration-invalid",
                "Dialogos discernment refused because safe exploration lanes must remain review-only return routes and may not become admission, action, authority, continuity, or Lisp evaluation.",
                timestampUtc);
        }

        if (HasDuplicate(request.SafeExplorationLanes.Select(static lane => lane.LaneHandle)))
        {
            return Refuse(
                request,
                "dialogos-discernment-duplicate-safe-lane-handle",
                "Dialogos discernment refused because duplicate safe exploration lane handles would collapse exploration lineage.",
                timestampUtc);
        }

        if (request.ReturnPaths.Any(static path => !path.IsColdReturnPath) ||
            request.ReturnPaths.Any(path => !thoughtHandles.Contains(path.SourceThoughtHandle)))
        {
            return Refuse(
                request,
                "dialogos-discernment-return-path-invalid",
                "Dialogos discernment refused because every return path must preserve the question, require evidence, return without admission, and refuse authority, continuity, and action.",
                timestampUtc);
        }

        if (HasDuplicate(request.ReturnPaths.Select(static path => path.ReturnHandle)))
        {
            return Refuse(
                request,
                "dialogos-discernment-duplicate-return-path-handle",
                "Dialogos discernment refused because duplicate return path handles would collapse return lineage.",
                timestampUtc);
        }

        var returnPathThoughts = request.ReturnPaths
            .Select(static path => path.SourceThoughtHandle)
            .ToHashSet(StringComparer.Ordinal);

        if (request.SafeExplorationLanes.Any(lane => !returnPathThoughts.Contains(lane.SourceThoughtHandle)))
        {
            return Refuse(
                request,
                "dialogos-discernment-safe-lane-return-path-missing",
                "Dialogos discernment refused because every safe exploration lane requires a return path before exploration can be retained.",
                timestampUtc);
        }

        var disposition = ResolveDisposition(request);
        var outcomeCode = disposition switch
        {
            DialogosDiscernmentDisposition.EmptyReviewCold => "dialogos-discernment-empty-review-only",
            DialogosDiscernmentDisposition.SafeExplorationReturnedCold => "dialogos-discernment-safe-exploration-returned-cold",
            _ => "dialogos-discernment-retained-for-review-cold"
        };
        var governanceTrace = disposition switch
        {
            DialogosDiscernmentDisposition.EmptyReviewCold =>
                "Dialogos discernment found no thought forms. Empty review preserves warrant boundary without truth, warrant, authority, continuity, action, Lisp evaluation, packet emission, replay, passage, or activation.",
            DialogosDiscernmentDisposition.SafeExplorationReturnedCold =>
                "Dialogos discernment returned safe exploration under evidence need and return path while refusing admission, warrant, authority, continuity, action, Lisp evaluation, packet emission, replay, passage, and activation.",
            _ =>
                "Dialogos discernment retained thought forms for review while refusing appearance-as-truth, articulation-as-warrant, coherence-as-evidence, agreement-as-authority, perspective-as-continuity, action, authority, continuity, identity mutation, Lisp evaluation, packet emission, replay, passage, and activation."
        };

        return CreateReceipt(
            request,
            disposition,
            outcomeCode,
            governanceTrace,
            refusal: null,
            timestampUtc);
    }

    private static DialogosDiscernmentDisposition ResolveDisposition(DialogosDiscernmentRequest request)
    {
        if (request.ThoughtForms.Count == 0)
        {
            return DialogosDiscernmentDisposition.EmptyReviewCold;
        }

        return request.SafeExplorationLanes.Count > 0 || request.ReturnPaths.Count > 0
            ? DialogosDiscernmentDisposition.SafeExplorationReturnedCold
            : DialogosDiscernmentDisposition.RetainedForReviewCold;
    }

    private static DialogosDiscernmentReceipt Refuse(
        DialogosDiscernmentRequest request,
        string outcomeCode,
        string governanceTrace,
        DateTimeOffset timestampUtc) =>
        CreateReceipt(
            request,
            DialogosDiscernmentDisposition.Refused,
            outcomeCode,
            governanceTrace,
            new PrincipledRefusalReceipt(
                ReceiptHandle: $"urn:san:dialogos-discernment-refusal:{ShortHash(SourceHandle(request), outcomeCode)}",
                RefusalCode: outcomeCode,
                GovernanceTrace: governanceTrace,
                ReturnPathHandle: request.ReturnPaths.FirstOrDefault()?.ReturnHandle ?? "missing-dialogos-return-path",
                Retained: true),
            timestampUtc);

    private static DialogosDiscernmentReceipt CreateReceipt(
        DialogosDiscernmentRequest request,
        DialogosDiscernmentDisposition disposition,
        string outcomeCode,
        string governanceTrace,
        PrincipledRefusalReceipt? refusal,
        DateTimeOffset timestampUtc) =>
        new(
            ReceiptHandle: $"urn:san:dialogos-discernment:{(refusal is null ? "review" : "refused")}:{ShortHash(SourceHandle(request), outcomeCode, request.ThoughtForms.Count.ToString(System.Globalization.CultureInfo.InvariantCulture), timestampUtc.UtcTicks.ToString(System.Globalization.CultureInfo.InvariantCulture))}",
            Disposition: disposition,
            OutcomeCode: outcomeCode,
            GovernanceTrace: governanceTrace,
            ThoughtForms: refusal is null ? request.ThoughtForms.ToArray() : [],
            ArticulationSurfaces: refusal is null ? request.ArticulationSurfaces.ToArray() : [],
            IntermediateChambers: refusal is null ? request.IntermediateChambers.ToArray() : [],
            SafeExplorationLanes: refusal is null ? request.SafeExplorationLanes.ToArray() : [],
            ReturnPaths: refusal is null ? request.ReturnPaths.ToArray() : [],
            WarrantBoundary: request.WarrantBoundary,
            Refusal: refusal,
            PriorPassageCount: request.PriorPassageCount,
            PassageCountAfterDiscernment: request.PriorPassageCount,
            ReviewOnly: true,
            SafeExplorationReturned: refusal is null &&
                disposition == DialogosDiscernmentDisposition.SafeExplorationReturnedCold,
            PrincipledRefusalRetained: refusal is not null,
            ThoughtAppearanceBecameTruth: false,
            ArticulationGrantedWarrant: false,
            CoherenceBecameEvidence: false,
            AgreementGrantedAuthority: false,
            PerspectiveAdmittedContinuity: false,
            SafeExplorationAdmitted: false,
            RuntimeActionAllowed: false,
            LispEvaluationAllowed: false,
            NewPacketEmitted: false,
            ReceiptsReplayed: false,
            ContinuityAdmitted: false,
            AuthorityGranted: false,
            ActivationRefused: true,
            TimestampUtc: timestampUtc);

    private static string SourceHandle(DialogosDiscernmentRequest request) =>
        request.ThoughtForms.Count == 0
            ? "dialogos-discernment-empty-source"
            : string.Join(",", request.ThoughtForms.Select(static thought => thought.ThoughtHandle));

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
