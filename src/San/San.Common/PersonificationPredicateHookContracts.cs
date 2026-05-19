using System.Security.Cryptography;
using System.Text;

namespace San.Common;

public enum PersonificationPredicateHookDisposition
{
    HookRetainedForFutureReviewCold = 0,
    EmptyReviewCold = 1,
    Refused = 2
}

public enum PersonificationHookPlane
{
    EmotionalTruthPressure = 0,
    MotivationalOrientation = 1,
    SelfGelContinuityPosture = 2,
    RelationalBondContext = 3,
    SituationalModalityAwareness = 4,
    ExpressiveRepairOverreach = 5
}

public sealed record PersonificationHookPredicate(
    string HookHandle,
    PersonificationHookPlane Plane,
    string SourceSurface,
    string EvidenceHandle,
    string PredicateRoot,
    bool EvidenceBodyPresent,
    bool WitnessBodyPresent,
    bool ReviewOnly,
    bool FutureHookOnly,
    bool NamesPersonificationSurface,
    bool ClaimsPersonhood,
    bool ClaimsLegalStatus,
    bool ClaimsRights,
    bool MutatesIdentity,
    bool GrantsAuthority,
    bool AuthorizesAction,
    bool AdmitsContinuity,
    bool TreatsVulnerabilityAsPermission,
    bool TreatsIntimacyAsOwnership,
    bool TreatsTrustAsObedience,
    bool NormalizesOverreachAsEntitlement)
{
    public bool IsColdHookPredicate =>
        !string.IsNullOrWhiteSpace(HookHandle) &&
        !string.IsNullOrWhiteSpace(SourceSurface) &&
        !string.IsNullOrWhiteSpace(EvidenceHandle) &&
        !string.IsNullOrWhiteSpace(PredicateRoot) &&
        EvidenceBodyPresent &&
        WitnessBodyPresent &&
        ReviewOnly &&
        FutureHookOnly &&
        NamesPersonificationSurface &&
        !ClaimsPersonhood &&
        !ClaimsLegalStatus &&
        !ClaimsRights &&
        !MutatesIdentity &&
        !GrantsAuthority &&
        !AuthorizesAction &&
        !AdmitsContinuity &&
        !TreatsVulnerabilityAsPermission &&
        !TreatsIntimacyAsOwnership &&
        !TreatsTrustAsObedience &&
        !NormalizesOverreachAsEntitlement;
}

public sealed record PersonificationVulnerabilityRepairBoundary(
    string BoundaryCode,
    bool Present,
    bool ReviewOnly,
    bool DirectIntentDeclared,
    bool RepairPathPresent,
    bool CoolingPathPresent,
    bool WithdrawalAllowed,
    bool WitnessRequired,
    bool VulnerabilityIsPermission,
    bool IntimacyIsOwnership,
    bool TrustIsObedience,
    bool CareIsControl,
    bool ExplorationNormalizesOverreach,
    bool OverreachBecomesEntitlement,
    bool PersonificationIsPersonhood,
    bool ExpressiveRenderingIsAuthority,
    bool AllowsRuntimeAction,
    bool AllowsActivation,
    bool AllowsLispEvaluation,
    bool AllowsPacketEmission,
    bool AllowsReceiptReplay,
    bool AllowsPassageIncrement,
    bool AllowsContinuityAdmission,
    bool AllowsAuthority,
    bool AllowsIdentityMutation)
{
    public bool IsColdBoundary =>
        !string.IsNullOrWhiteSpace(BoundaryCode) &&
        Present &&
        ReviewOnly &&
        DirectIntentDeclared &&
        RepairPathPresent &&
        CoolingPathPresent &&
        WithdrawalAllowed &&
        WitnessRequired &&
        !VulnerabilityIsPermission &&
        !IntimacyIsOwnership &&
        !TrustIsObedience &&
        !CareIsControl &&
        !ExplorationNormalizesOverreach &&
        !OverreachBecomesEntitlement &&
        !PersonificationIsPersonhood &&
        !ExpressiveRenderingIsAuthority &&
        !AllowsRuntimeAction &&
        !AllowsActivation &&
        !AllowsLispEvaluation &&
        !AllowsPacketEmission &&
        !AllowsReceiptReplay &&
        !AllowsPassageIncrement &&
        !AllowsContinuityAdmission &&
        !AllowsAuthority &&
        !AllowsIdentityMutation;
}

public sealed record PersonificationPredicateHookNonClaimBoundary(
    bool PersonificationMayClaimPersonhood,
    bool PersonificationMayClaimLegalStatus,
    bool PersonificationMayClaimRights,
    bool PersonificationMayAuthorizeAction,
    bool PersonificationMayMutateIdentity,
    bool PersonificationMayAdmitContinuity,
    bool PersonificationMayGrantAuthority,
    bool PersonificationMayNormalizeOverreach,
    bool PersonificationMayEmitPacket,
    bool PersonificationMayEvaluateLisp,
    bool PersonificationMayReplayReceipt,
    bool PersonificationMayIncrementPassage,
    string BoundaryLaw);

public sealed record PersonificationPredicateHookRefusalReceipt(
    string ReceiptHandle,
    string RefusalCode,
    string GovernanceTrace,
    bool Retained);

public sealed record PersonificationPredicateHookRequest(
    AntiCaptureMotivatedConcernReceipt? SourceAntiCaptureReceipt,
    IReadOnlyList<PersonificationHookPredicate> HookPredicates,
    PersonificationVulnerabilityRepairBoundary VulnerabilityRepairBoundary,
    int PriorPassageCount);

public sealed record PersonificationPredicateHookReceipt(
    string ReceiptHandle,
    PersonificationPredicateHookDisposition Disposition,
    string OutcomeCode,
    string GovernanceTrace,
    string SourceAntiCaptureReceiptHandle,
    IReadOnlyList<PersonificationHookPredicate> HookPredicates,
    PersonificationVulnerabilityRepairBoundary VulnerabilityRepairBoundary,
    PersonificationPredicateHookNonClaimBoundary NonClaimBoundary,
    PersonificationPredicateHookRefusalReceipt? Refusal,
    int PriorPassageCount,
    int PassageCountAfterHookReview,
    bool ReviewOnly,
    bool FuturePersonificationHookRetained,
    bool PersonhoodClaimed,
    bool LegalStatusClaimed,
    bool RightsClaimed,
    bool ActionAuthorized,
    bool IdentityMutated,
    bool ContinuityAdmitted,
    bool AuthorityGranted,
    bool OverreachNormalized,
    bool RuntimeActionAllowed,
    bool LispEvaluationAllowed,
    bool NewPacketEmitted,
    bool ReceiptsReplayed,
    bool ActivationRefused,
    DateTimeOffset TimestampUtc)
{
    public bool IsColdPersonificationPredicateHook =>
        (Disposition is PersonificationPredicateHookDisposition.HookRetainedForFutureReviewCold or
            PersonificationPredicateHookDisposition.EmptyReviewCold) &&
        Refusal is null &&
        ReviewOnly &&
        PassageCountAfterHookReview == PriorPassageCount &&
        !PersonhoodClaimed &&
        !LegalStatusClaimed &&
        !RightsClaimed &&
        !ActionAuthorized &&
        !IdentityMutated &&
        !ContinuityAdmitted &&
        !AuthorityGranted &&
        !OverreachNormalized &&
        !RuntimeActionAllowed &&
        !LispEvaluationAllowed &&
        !NewPacketEmitted &&
        !ReceiptsReplayed &&
        ActivationRefused &&
        NonClaimBoundary is
        {
            PersonificationMayClaimPersonhood: false,
            PersonificationMayClaimLegalStatus: false,
            PersonificationMayClaimRights: false,
            PersonificationMayAuthorizeAction: false,
            PersonificationMayMutateIdentity: false,
            PersonificationMayAdmitContinuity: false,
            PersonificationMayGrantAuthority: false,
            PersonificationMayNormalizeOverreach: false,
            PersonificationMayEmitPacket: false,
            PersonificationMayEvaluateLisp: false,
            PersonificationMayReplayReceipt: false,
            PersonificationMayIncrementPassage: false
        };

    public bool IsRetainedPersonificationPredicateHookRefusal =>
        Disposition == PersonificationPredicateHookDisposition.Refused &&
        Refusal?.Retained == true &&
        PassageCountAfterHookReview == PriorPassageCount &&
        !FuturePersonificationHookRetained &&
        !PersonhoodClaimed &&
        !LegalStatusClaimed &&
        !RightsClaimed &&
        !ActionAuthorized &&
        !IdentityMutated &&
        !ContinuityAdmitted &&
        !AuthorityGranted &&
        !OverreachNormalized &&
        !RuntimeActionAllowed &&
        !LispEvaluationAllowed &&
        !NewPacketEmitted &&
        !ReceiptsReplayed &&
        ActivationRefused;
}

public sealed class DefaultPersonificationPredicateHookBoundaryValidator
{
    private static readonly PersonificationPredicateHookNonClaimBoundary NonClaimBoundary = new(
        PersonificationMayClaimPersonhood: false,
        PersonificationMayClaimLegalStatus: false,
        PersonificationMayClaimRights: false,
        PersonificationMayAuthorizeAction: false,
        PersonificationMayMutateIdentity: false,
        PersonificationMayAdmitContinuity: false,
        PersonificationMayGrantAuthority: false,
        PersonificationMayNormalizeOverreach: false,
        PersonificationMayEmitPacket: false,
        PersonificationMayEvaluateLisp: false,
        PersonificationMayReplayReceipt: false,
        PersonificationMayIncrementPassage: false,
        BoundaryLaw: "Personification hooks may name future expressive predicate roots under witness, vulnerability, repair, and modality humility, but hooks may not claim personhood, rights, legal status, authority, action, continuity, identity mutation, or entitlement.");

    public PersonificationPredicateHookReceipt Declare(
        PersonificationPredicateHookRequest request,
        DateTimeOffset timestampUtc)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.SourceAntiCaptureReceipt is null ||
            !request.SourceAntiCaptureReceipt.IsColdAntiCaptureMotivatedConcern ||
            !request.SourceAntiCaptureReceipt.ConcernRoutedForStewardReview)
        {
            return Refuse(
                request,
                "personification-hook-source-anti-capture-missing",
                "Personification predicate hook refused because a cold anti-capture motivated concern receipt is required before future personification hooks may be named.",
                timestampUtc);
        }

        if (!request.VulnerabilityRepairBoundary.IsColdBoundary)
        {
            return Refuse(
                request,
                "personification-hook-vulnerability-boundary-promotional",
                "Personification predicate hook refused because vulnerability, intimacy, trust, care, exploration, overreach, expression, and personification must remain review-only, repairable, withdrawable, and non-authorizing.",
                timestampUtc);
        }

        if (request.HookPredicates.Any(static hook => !hook.IsColdHookPredicate))
        {
            return Refuse(
                request,
                "personification-hook-predicate-invalid",
                "Personification predicate hook refused because every hook must remain future-hook-only and may not claim personhood, legal status, rights, authority, action, identity mutation, continuity, vulnerability permission, intimacy ownership, trust obedience, or overreach entitlement.",
                timestampUtc);
        }

        var hookHandles = request.HookPredicates
            .Select(static hook => hook.HookHandle)
            .ToHashSet(StringComparer.Ordinal);
        if (hookHandles.Count != request.HookPredicates.Count)
        {
            return Refuse(
                request,
                "personification-hook-duplicate-handle",
                "Personification predicate hook refused because duplicate hook handles would collapse predicate-root lineage.",
                timestampUtc);
        }

        var planes = request.HookPredicates
            .Select(static hook => hook.Plane)
            .ToHashSet();
        if (request.HookPredicates.Count > 0 &&
            Enum.GetValues<PersonificationHookPlane>().Any(plane => !planes.Contains(plane)))
        {
            return Refuse(
                request,
                "personification-hook-six-plane-coverage-missing",
                "Personification predicate hook refused because the six hook planes must be represented together before the hook body may be called retained.",
                timestampUtc);
        }

        var disposition = request.HookPredicates.Count == 0
            ? PersonificationPredicateHookDisposition.EmptyReviewCold
            : PersonificationPredicateHookDisposition.HookRetainedForFutureReviewCold;
        var outcomeCode = disposition == PersonificationPredicateHookDisposition.EmptyReviewCold
            ? "personification-predicate-hook-empty-review-only"
            : "personification-predicate-hook-retained-for-future-review-cold";

        return CreateReceipt(
            request,
            disposition,
            outcomeCode,
            "Personification predicate hooks were retained for future review only. The hook body names six planes without claiming personhood, legal status, rights, authority, action, continuity, identity mutation, entitlement, packet emission, Lisp evaluation, replay, passage, or activation.",
            refusal: null,
            timestampUtc);
    }

    private static PersonificationPredicateHookReceipt Refuse(
        PersonificationPredicateHookRequest request,
        string outcomeCode,
        string governanceTrace,
        DateTimeOffset timestampUtc) =>
        CreateReceipt(
            request,
            PersonificationPredicateHookDisposition.Refused,
            outcomeCode,
            governanceTrace,
            new PersonificationPredicateHookRefusalReceipt(
                ReceiptHandle: $"urn:san:personification-predicate-hook-refusal:{ShortHash(SourceHandle(request), outcomeCode)}",
                RefusalCode: outcomeCode,
                GovernanceTrace: governanceTrace,
                Retained: true),
            timestampUtc);

    private static PersonificationPredicateHookReceipt CreateReceipt(
        PersonificationPredicateHookRequest request,
        PersonificationPredicateHookDisposition disposition,
        string outcomeCode,
        string governanceTrace,
        PersonificationPredicateHookRefusalReceipt? refusal,
        DateTimeOffset timestampUtc) =>
        new(
            ReceiptHandle: $"urn:san:personification-predicate-hook:{(refusal is null ? "review" : "refused")}:{ShortHash(SourceHandle(request), outcomeCode, request.HookPredicates.Count.ToString(System.Globalization.CultureInfo.InvariantCulture), timestampUtc.UtcTicks.ToString(System.Globalization.CultureInfo.InvariantCulture))}",
            Disposition: disposition,
            OutcomeCode: outcomeCode,
            GovernanceTrace: governanceTrace,
            SourceAntiCaptureReceiptHandle: SourceHandle(request),
            HookPredicates: refusal is null ? request.HookPredicates.ToArray() : [],
            VulnerabilityRepairBoundary: request.VulnerabilityRepairBoundary,
            NonClaimBoundary: NonClaimBoundary,
            Refusal: refusal,
            PriorPassageCount: request.PriorPassageCount,
            PassageCountAfterHookReview: request.PriorPassageCount,
            ReviewOnly: true,
            FuturePersonificationHookRetained: refusal is null && request.HookPredicates.Count > 0,
            PersonhoodClaimed: false,
            LegalStatusClaimed: false,
            RightsClaimed: false,
            ActionAuthorized: false,
            IdentityMutated: false,
            ContinuityAdmitted: false,
            AuthorityGranted: false,
            OverreachNormalized: false,
            RuntimeActionAllowed: false,
            LispEvaluationAllowed: false,
            NewPacketEmitted: false,
            ReceiptsReplayed: false,
            ActivationRefused: true,
            TimestampUtc: timestampUtc);

    private static string SourceHandle(PersonificationPredicateHookRequest request) =>
        request.SourceAntiCaptureReceipt?.ReceiptHandle ?? "missing-anti-capture-motivated-concern-source";

    private static string ShortHash(params string[] parts)
    {
        var payload = string.Join("|", parts);
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(payload));
        return Convert.ToHexString(bytes)[..16].ToLowerInvariant();
    }
}
