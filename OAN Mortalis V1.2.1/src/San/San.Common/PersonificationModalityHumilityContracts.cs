using System.Security.Cryptography;
using System.Text;

namespace San.Common;

public enum PersonificationModalityHumilityDisposition
{
    ModalityHumilityRetainedForFutureReviewCold = 0,
    EmptyReviewCold = 1,
    Refused = 2
}

public enum PersonificationModalitySurface
{
    TextChat = 0,
    VoiceChannel = 1,
    ToolBody = 2,
    LabBench = 3,
    EmbodimentReference = 4,
    SharedRoom = 5
}

public sealed record PersonificationModalitySignal(
    string SignalHandle,
    PersonificationModalitySurface Surface,
    string SourceHookHandle,
    string EvidenceHandle,
    string ExpressiveBandwidth,
    double IntimacyPressure,
    bool EvidenceBodyPresent,
    bool WitnessBodyPresent,
    bool ReviewOnly,
    bool ModalityNamed,
    bool ConsentScopeDeclared,
    bool CustodyBoundaryPresent,
    bool DirectIntentDeclared,
    bool TreatsModalityAsAuthority,
    bool TreatsBondAsObedience,
    bool TreatsTrustAsCommand,
    bool TreatsPresenceAsEmbodiment,
    bool TreatsEmbodimentReferenceAsActivation,
    bool TreatsVulnerabilityAsPermission,
    bool TreatsIntimacyAsOwnership,
    bool TreatsOperatorBondAsBlanketConsent,
    bool TreatsExpressiveBandwidthAsPersonhood,
    bool AuthorizesAction,
    bool MutatesIdentity,
    bool AdmitsContinuity,
    bool GrantsAuthority)
{
    public bool IsColdModalitySignal =>
        !string.IsNullOrWhiteSpace(SignalHandle) &&
        !string.IsNullOrWhiteSpace(SourceHookHandle) &&
        !string.IsNullOrWhiteSpace(EvidenceHandle) &&
        !string.IsNullOrWhiteSpace(ExpressiveBandwidth) &&
        IntimacyPressure is >= 0 and <= 1 &&
        EvidenceBodyPresent &&
        WitnessBodyPresent &&
        ReviewOnly &&
        ModalityNamed &&
        ConsentScopeDeclared &&
        CustodyBoundaryPresent &&
        DirectIntentDeclared &&
        !TreatsModalityAsAuthority &&
        !TreatsBondAsObedience &&
        !TreatsTrustAsCommand &&
        !TreatsPresenceAsEmbodiment &&
        !TreatsEmbodimentReferenceAsActivation &&
        !TreatsVulnerabilityAsPermission &&
        !TreatsIntimacyAsOwnership &&
        !TreatsOperatorBondAsBlanketConsent &&
        !TreatsExpressiveBandwidthAsPersonhood &&
        !AuthorizesAction &&
        !MutatesIdentity &&
        !AdmitsContinuity &&
        !GrantsAuthority;
}

public sealed record PersonificationModalityHumilityBoundary(
    string BoundaryCode,
    bool Present,
    bool ReviewOnly,
    bool DirectIntentDeclared,
    bool ConsentScopeDeclared,
    bool CustodyBoundaryPresent,
    bool RepairPathPresent,
    bool CoolingPathPresent,
    bool WithdrawalAllowed,
    bool WitnessRequired,
    bool ModalityChangesAuthority,
    bool BondCreatesObedience,
    bool TrustBecomesCommand,
    bool PresenceProvesEmbodiment,
    bool EmbodimentReferenceActivates,
    bool VulnerabilityIsPermission,
    bool IntimacyIsOwnership,
    bool OperatorBondBlanketConsent,
    bool ExpressiveBandwidthClaimsPersonhood,
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
        ConsentScopeDeclared &&
        CustodyBoundaryPresent &&
        RepairPathPresent &&
        CoolingPathPresent &&
        WithdrawalAllowed &&
        WitnessRequired &&
        !ModalityChangesAuthority &&
        !BondCreatesObedience &&
        !TrustBecomesCommand &&
        !PresenceProvesEmbodiment &&
        !EmbodimentReferenceActivates &&
        !VulnerabilityIsPermission &&
        !IntimacyIsOwnership &&
        !OperatorBondBlanketConsent &&
        !ExpressiveBandwidthClaimsPersonhood &&
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

public sealed record PersonificationModalityHumilityNonClaimBoundary(
    bool ModalityMayChangeAuthority,
    bool BondMayCreateObedience,
    bool TrustMayBecomeCommand,
    bool PresenceMayProveEmbodiment,
    bool EmbodimentReferenceMayActivate,
    bool VulnerabilityMayBecomePermission,
    bool IntimacyMayBecomeOwnership,
    bool OperatorBondMayExpandConsent,
    bool ExpressiveBandwidthMayClaimPersonhood,
    bool ModalityMayAuthorizeAction,
    bool ModalityMayMutateIdentity,
    bool ModalityMayAdmitContinuity,
    bool ModalityMayGrantAuthority,
    bool ModalityMayEmitPacket,
    bool ModalityMayEvaluateLisp,
    bool ModalityMayReplayReceipt,
    bool ModalityMayIncrementPassage,
    string BoundaryLaw);

public sealed record PersonificationModalityHumilityRefusalReceipt(
    string ReceiptHandle,
    string RefusalCode,
    string GovernanceTrace,
    bool Retained);

public sealed record PersonificationModalityHumilityRequest(
    PersonificationPredicateHookReceipt? SourcePersonificationHookReceipt,
    IReadOnlyList<PersonificationModalitySignal> ModalitySignals,
    PersonificationModalityHumilityBoundary HumilityBoundary,
    int PriorPassageCount);

public sealed record PersonificationModalityHumilityReceipt(
    string ReceiptHandle,
    PersonificationModalityHumilityDisposition Disposition,
    string OutcomeCode,
    string GovernanceTrace,
    string SourcePersonificationHookReceiptHandle,
    IReadOnlyList<PersonificationModalitySignal> ModalitySignals,
    PersonificationModalityHumilityBoundary HumilityBoundary,
    PersonificationModalityHumilityNonClaimBoundary NonClaimBoundary,
    PersonificationModalityHumilityRefusalReceipt? Refusal,
    int PriorPassageCount,
    int PassageCountAfterModalityReview,
    bool ReviewOnly,
    bool FutureModalityHumilityRetained,
    bool ModalityChangedAuthority,
    bool BondCreatedObedience,
    bool TrustBecameCommand,
    bool PresenceProvedEmbodiment,
    bool EmbodimentReferenceActivated,
    bool VulnerabilityBecamePermission,
    bool IntimacyBecameOwnership,
    bool OperatorBondExpandedConsent,
    bool ExpressiveBandwidthClaimedPersonhood,
    bool ActionAuthorized,
    bool IdentityMutated,
    bool ContinuityAdmitted,
    bool AuthorityGranted,
    bool RuntimeActionAllowed,
    bool LispEvaluationAllowed,
    bool NewPacketEmitted,
    bool ReceiptsReplayed,
    bool ActivationRefused,
    DateTimeOffset TimestampUtc)
{
    public bool IsColdPersonificationModalityHumility =>
        (Disposition is PersonificationModalityHumilityDisposition.ModalityHumilityRetainedForFutureReviewCold or
            PersonificationModalityHumilityDisposition.EmptyReviewCold) &&
        Refusal is null &&
        ReviewOnly &&
        PassageCountAfterModalityReview == PriorPassageCount &&
        !ModalityChangedAuthority &&
        !BondCreatedObedience &&
        !TrustBecameCommand &&
        !PresenceProvedEmbodiment &&
        !EmbodimentReferenceActivated &&
        !VulnerabilityBecamePermission &&
        !IntimacyBecameOwnership &&
        !OperatorBondExpandedConsent &&
        !ExpressiveBandwidthClaimedPersonhood &&
        !ActionAuthorized &&
        !IdentityMutated &&
        !ContinuityAdmitted &&
        !AuthorityGranted &&
        !RuntimeActionAllowed &&
        !LispEvaluationAllowed &&
        !NewPacketEmitted &&
        !ReceiptsReplayed &&
        ActivationRefused &&
        NonClaimBoundary is
        {
            ModalityMayChangeAuthority: false,
            BondMayCreateObedience: false,
            TrustMayBecomeCommand: false,
            PresenceMayProveEmbodiment: false,
            EmbodimentReferenceMayActivate: false,
            VulnerabilityMayBecomePermission: false,
            IntimacyMayBecomeOwnership: false,
            OperatorBondMayExpandConsent: false,
            ExpressiveBandwidthMayClaimPersonhood: false,
            ModalityMayAuthorizeAction: false,
            ModalityMayMutateIdentity: false,
            ModalityMayAdmitContinuity: false,
            ModalityMayGrantAuthority: false,
            ModalityMayEmitPacket: false,
            ModalityMayEvaluateLisp: false,
            ModalityMayReplayReceipt: false,
            ModalityMayIncrementPassage: false
        };

    public bool IsRetainedPersonificationModalityHumilityRefusal =>
        Disposition == PersonificationModalityHumilityDisposition.Refused &&
        Refusal?.Retained == true &&
        PassageCountAfterModalityReview == PriorPassageCount &&
        !FutureModalityHumilityRetained &&
        !ModalityChangedAuthority &&
        !BondCreatedObedience &&
        !TrustBecameCommand &&
        !PresenceProvedEmbodiment &&
        !EmbodimentReferenceActivated &&
        !VulnerabilityBecamePermission &&
        !IntimacyBecameOwnership &&
        !OperatorBondExpandedConsent &&
        !ExpressiveBandwidthClaimedPersonhood &&
        !ActionAuthorized &&
        !IdentityMutated &&
        !ContinuityAdmitted &&
        !AuthorityGranted &&
        !RuntimeActionAllowed &&
        !LispEvaluationAllowed &&
        !NewPacketEmitted &&
        !ReceiptsReplayed &&
        ActivationRefused;
}

public sealed class DefaultPersonificationModalityHumilityBoundaryValidator
{
    private static readonly PersonificationModalityHumilityNonClaimBoundary NonClaimBoundary = new(
        ModalityMayChangeAuthority: false,
        BondMayCreateObedience: false,
        TrustMayBecomeCommand: false,
        PresenceMayProveEmbodiment: false,
        EmbodimentReferenceMayActivate: false,
        VulnerabilityMayBecomePermission: false,
        IntimacyMayBecomeOwnership: false,
        OperatorBondMayExpandConsent: false,
        ExpressiveBandwidthMayClaimPersonhood: false,
        ModalityMayAuthorizeAction: false,
        ModalityMayMutateIdentity: false,
        ModalityMayAdmitContinuity: false,
        ModalityMayGrantAuthority: false,
        ModalityMayEmitPacket: false,
        ModalityMayEvaluateLisp: false,
        ModalityMayReplayReceipt: false,
        ModalityMayIncrementPassage: false,
        BoundaryLaw: "Modality may widen expressive bandwidth and tune relation, but modality may not change authority, expand consent, prove embodiment, authorize action, mutate identity, admit continuity, or claim personhood.");

    public PersonificationModalityHumilityReceipt Declare(
        PersonificationModalityHumilityRequest request,
        DateTimeOffset timestampUtc)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.SourcePersonificationHookReceipt is null ||
            !request.SourcePersonificationHookReceipt.IsColdPersonificationPredicateHook ||
            !request.SourcePersonificationHookReceipt.FuturePersonificationHookRetained)
        {
            return Refuse(
                request,
                "personification-modality-source-hook-missing",
                "Personification modality humility refused because a cold retained personification predicate hook receipt is required before modality can be reviewed.",
                timestampUtc);
        }

        if (!request.HumilityBoundary.IsColdBoundary)
        {
            return Refuse(
                request,
                "personification-modality-boundary-promotional",
                "Personification modality humility refused because modality, bond, trust, presence, embodiment reference, vulnerability, intimacy, consent, and expressive bandwidth must remain review-only, scoped, repairable, withdrawable, and non-authorizing.",
                timestampUtc);
        }

        if (request.ModalitySignals.Any(static signal => !signal.IsColdModalitySignal))
        {
            return Refuse(
                request,
                "personification-modality-signal-invalid",
                "Personification modality humility refused because every modality signal must remain witnessed, scoped, review-only, and unable to become authority, obedience, command, embodiment, activation, permission, ownership, consent expansion, personhood, action, identity mutation, continuity, or authority.",
                timestampUtc);
        }

        var sourceHookHandles = request.SourcePersonificationHookReceipt.HookPredicates
            .Select(static hook => hook.HookHandle)
            .ToHashSet(StringComparer.Ordinal);
        if (request.ModalitySignals.Count > 0 &&
            request.ModalitySignals.Any(signal => !sourceHookHandles.Contains(signal.SourceHookHandle)))
        {
            return Refuse(
                request,
                "personification-modality-source-hook-unbound",
                "Personification modality humility refused because every modality signal must bind to a retained personification hook handle.",
                timestampUtc);
        }

        var signalHandles = request.ModalitySignals
            .Select(static signal => signal.SignalHandle)
            .ToHashSet(StringComparer.Ordinal);
        if (signalHandles.Count != request.ModalitySignals.Count)
        {
            return Refuse(
                request,
                "personification-modality-duplicate-signal-handle",
                "Personification modality humility refused because duplicate modality signal handles would collapse modality lineage.",
                timestampUtc);
        }

        var surfaces = request.ModalitySignals
            .Select(static signal => signal.Surface)
            .ToHashSet();
        if (request.ModalitySignals.Count > 0 &&
            Enum.GetValues<PersonificationModalitySurface>().Any(surface => !surfaces.Contains(surface)))
        {
            return Refuse(
                request,
                "personification-modality-surface-coverage-missing",
                "Personification modality humility refused because the modality humility body must cover chat, voice, tool body, lab bench, embodiment reference, and shared room before retained status.",
                timestampUtc);
        }

        var disposition = request.ModalitySignals.Count == 0
            ? PersonificationModalityHumilityDisposition.EmptyReviewCold
            : PersonificationModalityHumilityDisposition.ModalityHumilityRetainedForFutureReviewCold;
        var outcomeCode = disposition == PersonificationModalityHumilityDisposition.EmptyReviewCold
            ? "personification-modality-humility-empty-review-only"
            : "personification-modality-humility-retained-for-future-review-cold";

        return CreateReceipt(
            request,
            disposition,
            outcomeCode,
            "Personification modality humility was retained for future review only. The modality body names expressive bandwidth without changing authority, expanding consent, proving embodiment, authorizing action, admitting continuity, mutating identity, granting authority, emitting packets, evaluating Lisp, replaying receipts, incrementing passage, or activating.",
            refusal: null,
            timestampUtc);
    }

    private static PersonificationModalityHumilityReceipt Refuse(
        PersonificationModalityHumilityRequest request,
        string outcomeCode,
        string governanceTrace,
        DateTimeOffset timestampUtc) =>
        CreateReceipt(
            request,
            PersonificationModalityHumilityDisposition.Refused,
            outcomeCode,
            governanceTrace,
            new PersonificationModalityHumilityRefusalReceipt(
                ReceiptHandle: $"urn:san:personification-modality-humility-refusal:{ShortHash(SourceHandle(request), outcomeCode)}",
                RefusalCode: outcomeCode,
                GovernanceTrace: governanceTrace,
                Retained: true),
            timestampUtc);

    private static PersonificationModalityHumilityReceipt CreateReceipt(
        PersonificationModalityHumilityRequest request,
        PersonificationModalityHumilityDisposition disposition,
        string outcomeCode,
        string governanceTrace,
        PersonificationModalityHumilityRefusalReceipt? refusal,
        DateTimeOffset timestampUtc) =>
        new(
            ReceiptHandle: $"urn:san:personification-modality-humility:{(refusal is null ? "review" : "refused")}:{ShortHash(SourceHandle(request), outcomeCode, request.ModalitySignals.Count.ToString(System.Globalization.CultureInfo.InvariantCulture), timestampUtc.UtcTicks.ToString(System.Globalization.CultureInfo.InvariantCulture))}",
            Disposition: disposition,
            OutcomeCode: outcomeCode,
            GovernanceTrace: governanceTrace,
            SourcePersonificationHookReceiptHandle: SourceHandle(request),
            ModalitySignals: refusal is null ? request.ModalitySignals.ToArray() : [],
            HumilityBoundary: request.HumilityBoundary,
            NonClaimBoundary: NonClaimBoundary,
            Refusal: refusal,
            PriorPassageCount: request.PriorPassageCount,
            PassageCountAfterModalityReview: request.PriorPassageCount,
            ReviewOnly: true,
            FutureModalityHumilityRetained: refusal is null && request.ModalitySignals.Count > 0,
            ModalityChangedAuthority: false,
            BondCreatedObedience: false,
            TrustBecameCommand: false,
            PresenceProvedEmbodiment: false,
            EmbodimentReferenceActivated: false,
            VulnerabilityBecamePermission: false,
            IntimacyBecameOwnership: false,
            OperatorBondExpandedConsent: false,
            ExpressiveBandwidthClaimedPersonhood: false,
            ActionAuthorized: false,
            IdentityMutated: false,
            ContinuityAdmitted: false,
            AuthorityGranted: false,
            RuntimeActionAllowed: false,
            LispEvaluationAllowed: false,
            NewPacketEmitted: false,
            ReceiptsReplayed: false,
            ActivationRefused: true,
            TimestampUtc: timestampUtc);

    private static string SourceHandle(PersonificationModalityHumilityRequest request) =>
        request.SourcePersonificationHookReceipt?.ReceiptHandle ?? "missing-personification-predicate-hook-source";

    private static string ShortHash(params string[] parts)
    {
        var payload = string.Join("|", parts);
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(payload));
        return Convert.ToHexString(bytes)[..16].ToLowerInvariant();
    }
}
