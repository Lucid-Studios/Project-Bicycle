using System.Security.Cryptography;
using System.Text;

namespace San.Common;

public enum AspirationCandidateSelectionClosureDisposition
{
    SelectedForReviewCold = 0,
    EmptyReviewCold = 1,
    Refused = 2
}

public enum AspirationCandidateSelectionState
{
    SelectedWorkingSet = 0,
    HeldAsCompost = 1,
    ReturnedForEvidence = 2,
    DeferredForCooling = 3
}

public sealed record AspirationCandidateSelection(
    string SelectionHandle,
    string SourceMaturationCandidateHandle,
    string SourcePayloadStatementHandle,
    AspirationCandidateSelectionState SelectionState,
    string SelectionRationale,
    string EvidenceHandle,
    string WitnessHandle,
    string ReturnPathHandle,
    bool ReviewOnly,
    bool PreservesCandidateLineage,
    bool PreservesPayloadLineage,
    bool RequiresStewardReview,
    bool RequiresCooling,
    bool AllowsCompostRetention,
    bool SelectionBecomesWarrant,
    bool SelectionBecomesAdmission,
    bool SelectionGrantsAuthority,
    bool SelectionAdmitsContinuity,
    bool SelectionAuthorizesAction,
    bool SelectionEvaluatesLisp,
    bool SelectionSmugglesKey)
{
    public bool IsColdSelection =>
        !string.IsNullOrWhiteSpace(SelectionHandle) &&
        !string.IsNullOrWhiteSpace(SourceMaturationCandidateHandle) &&
        !string.IsNullOrWhiteSpace(SourcePayloadStatementHandle) &&
        !string.IsNullOrWhiteSpace(SelectionRationale) &&
        !string.IsNullOrWhiteSpace(EvidenceHandle) &&
        !string.IsNullOrWhiteSpace(WitnessHandle) &&
        !string.IsNullOrWhiteSpace(ReturnPathHandle) &&
        ReviewOnly &&
        PreservesCandidateLineage &&
        PreservesPayloadLineage &&
        RequiresStewardReview &&
        RequiresCooling &&
        AllowsCompostRetention &&
        !SelectionBecomesWarrant &&
        !SelectionBecomesAdmission &&
        !SelectionGrantsAuthority &&
        !SelectionAdmitsContinuity &&
        !SelectionAuthorizesAction &&
        !SelectionEvaluatesLisp &&
        !SelectionSmugglesKey;
}

public sealed record AspirationClosureLaw(
    string LawHandle,
    string LawText,
    bool ReviewOnly,
    bool PreservesSelectionLineage,
    bool PreservesCompost,
    bool RequiresWitness,
    bool RequiresReturnPath,
    bool KeepsKeysWithheld,
    bool LawBecomesWarrant,
    bool LawGrantsAuthority,
    bool LawAdmitsContinuity,
    bool LawAuthorizesAction,
    bool LawEvaluatesLisp,
    bool LawActivates)
{
    public bool IsColdClosureLaw =>
        !string.IsNullOrWhiteSpace(LawHandle) &&
        !string.IsNullOrWhiteSpace(LawText) &&
        ReviewOnly &&
        PreservesSelectionLineage &&
        PreservesCompost &&
        RequiresWitness &&
        RequiresReturnPath &&
        KeepsKeysWithheld &&
        !LawBecomesWarrant &&
        !LawGrantsAuthority &&
        !LawAdmitsContinuity &&
        !LawAuthorizesAction &&
        !LawEvaluatesLisp &&
        !LawActivates;
}

public sealed record AspirationCandidateSelectionClosureBoundary(
    string BoundaryCode,
    bool Present,
    bool ReviewOnly,
    bool AllowsCandidateSelection,
    bool AllowsWorkingSetFormation,
    bool AllowsCompostRetention,
    bool AllowsEvidenceReturn,
    bool RequiresEvidence,
    bool RequiresWitness,
    bool RequiresCooling,
    bool RequiresReturnPath,
    bool RequiresStewardReview,
    bool RequiresKeyWithholding,
    bool AllowsSelectionAsWarrant,
    bool AllowsSelectionAsAdmission,
    bool AllowsSelectionAsAuthority,
    bool AllowsSelectionAsContinuity,
    bool AllowsClosureLawAsKey,
    bool AllowsRuntimeAction,
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
        AllowsCandidateSelection &&
        AllowsWorkingSetFormation &&
        AllowsCompostRetention &&
        AllowsEvidenceReturn &&
        RequiresEvidence &&
        RequiresWitness &&
        RequiresCooling &&
        RequiresReturnPath &&
        RequiresStewardReview &&
        RequiresKeyWithholding &&
        !AllowsSelectionAsWarrant &&
        !AllowsSelectionAsAdmission &&
        !AllowsSelectionAsAuthority &&
        !AllowsSelectionAsContinuity &&
        !AllowsClosureLawAsKey &&
        !AllowsRuntimeAction &&
        !AllowsLispEvaluation &&
        !AllowsPacketEmission &&
        !AllowsReceiptReplay &&
        !IncrementsPassageCount &&
        !AllowsActivation;
}

public sealed record AspirationCandidateSelectionNonPromotionBoundary(
    string BoundaryLaw,
    bool SelectionMayBecomeWarrant,
    bool SelectionMayBecomeAdmission,
    bool SelectionMayGrantAuthority,
    bool SelectionMayAdmitContinuity,
    bool ClosureLawMaySmuggleKey,
    bool CompostMayBeErased,
    bool CandidateMayAuthorizeAction,
    bool CandidateMayEvaluateLisp,
    bool CandidateMayEmitPacket,
    bool CandidateMayReplayReceipts,
    bool CandidateMayIncrementPassage,
    bool CandidateMayActivate)
{
    public bool IsColdNonPromotionBoundary =>
        !string.IsNullOrWhiteSpace(BoundaryLaw) &&
        !SelectionMayBecomeWarrant &&
        !SelectionMayBecomeAdmission &&
        !SelectionMayGrantAuthority &&
        !SelectionMayAdmitContinuity &&
        !ClosureLawMaySmuggleKey &&
        !CompostMayBeErased &&
        !CandidateMayAuthorizeAction &&
        !CandidateMayEvaluateLisp &&
        !CandidateMayEmitPacket &&
        !CandidateMayReplayReceipts &&
        !CandidateMayIncrementPassage &&
        !CandidateMayActivate;
}

public sealed record AspirationCandidateSelectionRefusalReceipt(
    string ReceiptHandle,
    string RefusalCode,
    string GovernanceTrace,
    bool Retained);

public sealed record AspirationCandidateSelectionClosureRequest(
    IReadOnlyList<AspirationCandidateSelection> Selections,
    IReadOnlyList<AspirationClosureLaw> ClosureLaws,
    AspirationCandidateSelectionClosureBoundary Boundary,
    AspirationCandidateSelectionNonPromotionBoundary NonPromotionBoundary,
    int PriorPassageCount);

public sealed record AspirationCandidateSelectionClosureReceipt(
    string ReceiptHandle,
    AspirationCandidateSelectionClosureDisposition Disposition,
    string OutcomeCode,
    string GovernanceTrace,
    IReadOnlyList<AspirationCandidateSelection> Selections,
    IReadOnlyList<AspirationClosureLaw> ClosureLaws,
    AspirationCandidateSelectionClosureBoundary Boundary,
    AspirationCandidateSelectionNonPromotionBoundary NonPromotionBoundary,
    AspirationCandidateSelectionRefusalReceipt? Refusal,
    int PriorPassageCount,
    int PassageCountAfterSelection,
    int RetainedSelectionCount,
    bool ReviewOnly,
    bool WorkingSetSelectedForReview,
    bool SelectionBecameWarrant,
    bool SelectionBecameAdmission,
    bool SelectionGrantedAuthority,
    bool SelectionAdmittedContinuity,
    bool ClosureLawSmuggledKey,
    bool CompostErased,
    bool ActionAuthorized,
    bool RuntimeActionAllowed,
    bool LispEvaluationAllowed,
    bool NewPacketEmitted,
    bool ReceiptsReplayed,
    bool ContinuityAdmitted,
    bool AuthorityGranted,
    bool ActivationRefused,
    DateTimeOffset TimestampUtc)
{
    public bool IsColdSelectionClosure =>
        (Disposition is AspirationCandidateSelectionClosureDisposition.SelectedForReviewCold or
            AspirationCandidateSelectionClosureDisposition.EmptyReviewCold) &&
        Refusal is null &&
        ReviewOnly &&
        PassageCountAfterSelection == PriorPassageCount &&
        RetainedSelectionCount == Selections.Count &&
        !SelectionBecameWarrant &&
        !SelectionBecameAdmission &&
        !SelectionGrantedAuthority &&
        !SelectionAdmittedContinuity &&
        !ClosureLawSmuggledKey &&
        !CompostErased &&
        !ActionAuthorized &&
        !RuntimeActionAllowed &&
        !LispEvaluationAllowed &&
        !NewPacketEmitted &&
        !ReceiptsReplayed &&
        !ContinuityAdmitted &&
        !AuthorityGranted &&
        ActivationRefused &&
        Boundary.IsColdBoundary &&
        NonPromotionBoundary.IsColdNonPromotionBoundary;

    public bool IsRetainedSelectionRefusal =>
        Disposition == AspirationCandidateSelectionClosureDisposition.Refused &&
        Refusal?.Retained == true &&
        PassageCountAfterSelection == PriorPassageCount &&
        RetainedSelectionCount == 0 &&
        !WorkingSetSelectedForReview &&
        !SelectionBecameWarrant &&
        !SelectionBecameAdmission &&
        !SelectionGrantedAuthority &&
        !SelectionAdmittedContinuity &&
        !ClosureLawSmuggledKey &&
        !CompostErased &&
        !ActionAuthorized &&
        !RuntimeActionAllowed &&
        !LispEvaluationAllowed &&
        !NewPacketEmitted &&
        !ReceiptsReplayed &&
        !ContinuityAdmitted &&
        !AuthorityGranted &&
        ActivationRefused;
}

public sealed class DefaultAspirationCandidateSelectionClosureBoundaryValidator
{
    public AspirationCandidateSelectionClosureReceipt Select(
        AspirationCandidateSelectionClosureRequest request,
        DateTimeOffset timestampUtc)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.Boundary is null ||
            !request.Boundary.Present ||
            string.IsNullOrWhiteSpace(request.Boundary.BoundaryCode))
        {
            return Refuse(
                request,
                "aspiration-selection-boundary-missing",
                "Aspiration selection refused because a review-only selection and closure boundary is required.",
                timestampUtc);
        }

        if (!request.Boundary.IsColdBoundary)
        {
            return Refuse(
                request,
                "aspiration-selection-promotional-boundary",
                "Aspiration selection refused because the boundary must allow candidate selection, working-set formation, compost retention, and evidence return while requiring evidence, witness, cooling, return path, Steward review, key withholding, and while refusing warrant, admission, authority, continuity, key smuggling, action, Lisp evaluation, packet emission, replay, passage, and activation.",
                timestampUtc);
        }

        if (request.NonPromotionBoundary is null ||
            !request.NonPromotionBoundary.IsColdNonPromotionBoundary)
        {
            return Refuse(
                request,
                "aspiration-selection-non-promotion-boundary-invalid",
                "Aspiration selection refused because non-promotion law must prevent selection, closure law, compost, candidates, Lisp evaluation, packets, replay, passage, and activation from promoting themselves.",
                timestampUtc);
        }

        if (request.Selections.Any(static selection => !selection.IsColdSelection))
        {
            return Refuse(
                request,
                "aspiration-selection-invalid",
                "Aspiration selection refused because every selection must preserve candidate and payload lineage while remaining review-only, steward-reviewable, cooled, returned, compost-retaining, and unable to become warrant, admission, authority, continuity, action, Lisp evaluation, or key.",
                timestampUtc);
        }

        if (HasDuplicate(request.Selections.Select(static selection => selection.SelectionHandle)))
        {
            return Refuse(
                request,
                "aspiration-selection-duplicate-selection-handle",
                "Aspiration selection refused because duplicate selection handles would collapse selection lineage.",
                timestampUtc);
        }

        if (HasDuplicate(request.Selections.Select(static selection => selection.SourceMaturationCandidateHandle)))
        {
            return Refuse(
                request,
                "aspiration-selection-duplicate-candidate-selection",
                "Aspiration selection refused because a maturation candidate may not receive multiple selection states in the same closure pass.",
                timestampUtc);
        }

        if (request.ClosureLaws.Any(static law => !law.IsColdClosureLaw))
        {
            return Refuse(
                request,
                "aspiration-selection-closure-law-invalid",
                "Aspiration selection refused because every closure law must remain review-only, preserve lineage and compost, require witness and return path, keep keys withheld, and refuse warrant, authority, continuity, action, Lisp evaluation, and activation.",
                timestampUtc);
        }

        if (HasDuplicate(request.ClosureLaws.Select(static law => law.LawHandle)))
        {
            return Refuse(
                request,
                "aspiration-selection-duplicate-closure-law-handle",
                "Aspiration selection refused because duplicate closure law handles would collapse closure lineage.",
                timestampUtc);
        }

        if (request.Selections.Count > 0 && request.ClosureLaws.Count == 0)
        {
            return Refuse(
                request,
                "aspiration-selection-closure-law-missing",
                "Aspiration selection refused because non-empty selection requires at least one closure law with keys withheld.",
                timestampUtc);
        }

        var disposition = request.Selections.Count == 0
            ? AspirationCandidateSelectionClosureDisposition.EmptyReviewCold
            : AspirationCandidateSelectionClosureDisposition.SelectedForReviewCold;
        var outcomeCode = disposition == AspirationCandidateSelectionClosureDisposition.EmptyReviewCold
            ? "aspiration-selection-empty-review-only"
            : "aspiration-selection-working-set-retained-cold";
        var governanceTrace = disposition == AspirationCandidateSelectionClosureDisposition.EmptyReviewCold
            ? "Aspiration candidate selection found no candidates. Empty review preserves selection closure without warrant, admission, authority, continuity, key smuggling, action, Lisp evaluation, packet emission, replay, passage, or activation."
            : "Aspiration candidates were selected, composted, returned for evidence, or cooled as a review-only working set while refusing selection-as-warrant, selection-as-admission, selection-as-authority, selection-as-continuity, closure-law-as-key, compost erasure, action, Lisp evaluation, packet emission, replay, passage, and activation.";

        return CreateReceipt(
            request,
            disposition,
            outcomeCode,
            governanceTrace,
            refusal: null,
            timestampUtc);
    }

    private static AspirationCandidateSelectionClosureReceipt Refuse(
        AspirationCandidateSelectionClosureRequest request,
        string outcomeCode,
        string governanceTrace,
        DateTimeOffset timestampUtc) =>
        CreateReceipt(
            request,
            AspirationCandidateSelectionClosureDisposition.Refused,
            outcomeCode,
            governanceTrace,
            new AspirationCandidateSelectionRefusalReceipt(
                ReceiptHandle: $"urn:san:aspiration-selection-refusal:{ShortHash(SourceHandle(request), outcomeCode)}",
                RefusalCode: outcomeCode,
                GovernanceTrace: governanceTrace,
                Retained: true),
            timestampUtc);

    private static AspirationCandidateSelectionClosureReceipt CreateReceipt(
        AspirationCandidateSelectionClosureRequest request,
        AspirationCandidateSelectionClosureDisposition disposition,
        string outcomeCode,
        string governanceTrace,
        AspirationCandidateSelectionRefusalReceipt? refusal,
        DateTimeOffset timestampUtc) =>
        new(
            ReceiptHandle: $"urn:san:aspiration-selection:{(refusal is null ? "review" : "refused")}:{ShortHash(SourceHandle(request), outcomeCode, request.Selections.Count.ToString(System.Globalization.CultureInfo.InvariantCulture), timestampUtc.UtcTicks.ToString(System.Globalization.CultureInfo.InvariantCulture))}",
            Disposition: disposition,
            OutcomeCode: outcomeCode,
            GovernanceTrace: governanceTrace,
            Selections: refusal is null ? request.Selections.ToArray() : [],
            ClosureLaws: refusal is null ? request.ClosureLaws.ToArray() : [],
            Boundary: request.Boundary,
            NonPromotionBoundary: request.NonPromotionBoundary,
            Refusal: refusal,
            PriorPassageCount: request.PriorPassageCount,
            PassageCountAfterSelection: request.PriorPassageCount,
            RetainedSelectionCount: refusal is null ? request.Selections.Count : 0,
            ReviewOnly: true,
            WorkingSetSelectedForReview: refusal is null &&
                disposition == AspirationCandidateSelectionClosureDisposition.SelectedForReviewCold,
            SelectionBecameWarrant: false,
            SelectionBecameAdmission: false,
            SelectionGrantedAuthority: false,
            SelectionAdmittedContinuity: false,
            ClosureLawSmuggledKey: false,
            CompostErased: false,
            ActionAuthorized: false,
            RuntimeActionAllowed: false,
            LispEvaluationAllowed: false,
            NewPacketEmitted: false,
            ReceiptsReplayed: false,
            ContinuityAdmitted: false,
            AuthorityGranted: false,
            ActivationRefused: true,
            TimestampUtc: timestampUtc);

    private static string SourceHandle(AspirationCandidateSelectionClosureRequest request) =>
        request.Selections.Count == 0
            ? "aspiration-selection-empty-source"
            : string.Join(",", request.Selections.Take(3).Select(static selection => selection.SelectionHandle));

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
