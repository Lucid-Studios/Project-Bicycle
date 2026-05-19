using System.Security.Cryptography;
using System.Text;

namespace San.Common;

public enum CmeLispThreadTouchKind
{
    Pluck = 0,
    Strike = 1,
    Bow = 2,
    Mute = 3,
    Rest = 4
}

public enum CmeLispResonanceHeartbeatDisposition
{
    EmanationReviewCold = 0,
    TouchReviewCold = 1,
    DampingReviewCold = 2,
    RestReviewCold = 3,
    Refused = 4
}

public sealed record CmeLispGlobalResonanceLaw(
    bool SoundMayBecomeAction,
    bool ResonanceMayAuthorize,
    bool ResonanceMayAdmitContinuity,
    bool DiscordanceMayBecomeFailure,
    bool DampingMayEraseWitness,
    bool RestMayMeanAbsence,
    bool RepetitionMayBecomeContinuity,
    bool AmplitudeMayBecomeTruth,
    string BoundaryLaw)
{
    public bool IsColdLaw =>
        !SoundMayBecomeAction &&
        !ResonanceMayAuthorize &&
        !ResonanceMayAdmitContinuity &&
        !DiscordanceMayBecomeFailure &&
        !DampingMayEraseWitness &&
        !RestMayMeanAbsence &&
        !RepetitionMayBecomeContinuity &&
        !AmplitudeMayBecomeTruth &&
        !string.IsNullOrWhiteSpace(BoundaryLaw);
}

public sealed record StewardHeartbeatPolicy(
    string PolicyCode,
    bool StewardGoverned,
    bool ReviewWindowPresent,
    bool AllowsUngovernedCadence,
    bool AllowsSoundToBypassReview,
    bool AllowsActionWithoutAdmission,
    bool AllowsHeartbeatToOwnResonance,
    bool AllowsHeartbeatToAdmitContinuity)
{
    public bool IsColdStewardPolicy =>
        !string.IsNullOrWhiteSpace(PolicyCode) &&
        StewardGoverned &&
        ReviewWindowPresent &&
        !AllowsUngovernedCadence &&
        !AllowsSoundToBypassReview &&
        !AllowsActionWithoutAdmission &&
        !AllowsHeartbeatToOwnResonance &&
        !AllowsHeartbeatToAdmitContinuity;
}

public sealed record ListeningFrameEmanationRecord(
    string EmanationHandle,
    string SharedRealitySurface,
    string ListeningFrameSurface,
    string HarmonicCondition,
    bool ReviewOnly,
    bool Inert,
    bool EmanationIsAction,
    bool AuthorityRequested,
    bool ContinuityClaimed,
    bool ActivationRequested)
{
    public bool IsColdEmanation =>
        !string.IsNullOrWhiteSpace(EmanationHandle) &&
        !string.IsNullOrWhiteSpace(SharedRealitySurface) &&
        !string.IsNullOrWhiteSpace(ListeningFrameSurface) &&
        !string.IsNullOrWhiteSpace(HarmonicCondition) &&
        ReviewOnly &&
        Inert &&
        !EmanationIsAction &&
        !AuthorityRequested &&
        !ContinuityClaimed &&
        !ActivationRequested;
}

public sealed record LispThreadTouchEvent(
    string TouchHandle,
    CmeLispThreadTouchKind TouchKind,
    string ThreadHandle,
    int HeartbeatOrdinal,
    decimal Attack,
    decimal SustainWindow,
    string DampingPath,
    bool StewardHeartbeatPresent,
    bool ActionAdmissionBoundaryPresent,
    bool ReviewOnly,
    bool Inert,
    bool EmitsPacket,
    bool RequestsRuntimeAction,
    bool ClaimsAuthority,
    bool ClaimsContinuity)
{
    public bool IsColdTouch =>
        !string.IsNullOrWhiteSpace(TouchHandle) &&
        !string.IsNullOrWhiteSpace(ThreadHandle) &&
        HeartbeatOrdinal >= 0 &&
        Attack >= 0m &&
        SustainWindow >= 0m &&
        !string.IsNullOrWhiteSpace(DampingPath) &&
        StewardHeartbeatPresent &&
        ReviewOnly &&
        Inert &&
        !EmitsPacket &&
        !RequestsRuntimeAction &&
        !ClaimsAuthority &&
        !ClaimsContinuity;
}

public sealed record ThreadResonanceEvidence(
    string EvidenceHandle,
    string EmanationHandle,
    string TouchHandle,
    decimal ResonanceAmplitude,
    decimal DiscordanceIndex,
    bool DampingApplied,
    bool ReviewOnly,
    bool Inert,
    bool EvidenceBecomesWarrant,
    bool ClaimsAction,
    bool ClaimsAuthority,
    bool ClaimsContinuity)
{
    public bool IsColdEvidence =>
        !string.IsNullOrWhiteSpace(EvidenceHandle) &&
        !string.IsNullOrWhiteSpace(EmanationHandle) &&
        !string.IsNullOrWhiteSpace(TouchHandle) &&
        ResonanceAmplitude >= 0m &&
        DiscordanceIndex >= 0m &&
        DampingApplied &&
        ReviewOnly &&
        Inert &&
        !EvidenceBecomesWarrant &&
        !ClaimsAction &&
        !ClaimsAuthority &&
        !ClaimsContinuity;
}

public sealed record DampingProfile(
    string DampingCode,
    decimal DampingCoefficient,
    string CoolingRoute,
    bool DampsWithoutErasure,
    bool ErasesWitness,
    bool PromotesContinuity,
    bool GrantsAuthority)
{
    public bool IsColdDamping =>
        !string.IsNullOrWhiteSpace(DampingCode) &&
        DampingCoefficient >= 0m &&
        DampingCoefficient <= 1m &&
        !string.IsNullOrWhiteSpace(CoolingRoute) &&
        DampsWithoutErasure &&
        !ErasesWitness &&
        !PromotesContinuity &&
        !GrantsAuthority;
}

public sealed record DiscordanceRoute(
    string RouteCode,
    decimal DiscordanceThreshold,
    bool RoutesToReview,
    bool RoutesToCooling,
    bool RoutesToRefusal,
    bool TreatsDiscordanceAsFailure,
    bool GrantsAuthority,
    bool AdmitsContinuity)
{
    public bool IsColdDiscordanceRoute =>
        !string.IsNullOrWhiteSpace(RouteCode) &&
        DiscordanceThreshold >= 0m &&
        DiscordanceThreshold <= 1m &&
        RoutesToReview &&
        (RoutesToCooling || RoutesToRefusal) &&
        !TreatsDiscordanceAsFailure &&
        !GrantsAuthority &&
        !AdmitsContinuity;
}

public sealed record ActionAdmissionBoundary(
    string BoundaryCode,
    bool Present,
    bool StewardReviewRequired,
    bool AllowsSoundToBecomeAction,
    bool AllowsAuthority,
    bool AllowsContinuityAdmission,
    bool AllowsPacketEmission,
    bool AllowsLispEvaluation,
    bool IncrementsPassageCount)
{
    public bool IsColdAdmissionBoundary =>
        !string.IsNullOrWhiteSpace(BoundaryCode) &&
        Present &&
        StewardReviewRequired &&
        !AllowsSoundToBecomeAction &&
        !AllowsAuthority &&
        !AllowsContinuityAdmission &&
        !AllowsPacketEmission &&
        !AllowsLispEvaluation &&
        !IncrementsPassageCount;
}

public sealed record CmeLispResonanceHeartbeatRefusalReceipt(
    string ReceiptHandle,
    string RefusalCode,
    string GovernanceTrace,
    bool Retained);

public sealed record CmeLispResonanceHeartbeatRequest(
    CmeLispThreadFretboardReceipt? SourceFretboardReceipt,
    CmeLispGlobalResonanceLaw GlobalLaw,
    StewardHeartbeatPolicy HeartbeatPolicy,
    IReadOnlyList<ListeningFrameEmanationRecord> Emanations,
    IReadOnlyList<LispThreadTouchEvent> TouchEvents,
    IReadOnlyList<ThreadResonanceEvidence> ResonanceEvidence,
    IReadOnlyList<DampingProfile> DampingProfiles,
    IReadOnlyList<DiscordanceRoute> DiscordanceRoutes,
    ActionAdmissionBoundary ActionAdmissionBoundary,
    int PriorPassageCount);

public sealed record CmeLispResonanceHeartbeatReceipt(
    string ReceiptHandle,
    CmeLispResonanceHeartbeatDisposition Disposition,
    string OutcomeCode,
    string GovernanceTrace,
    string SourceFretboardReceiptHandle,
    CmeLispGlobalResonanceLaw GlobalLaw,
    StewardHeartbeatPolicy HeartbeatPolicy,
    IReadOnlyList<ListeningFrameEmanationRecord> Emanations,
    IReadOnlyList<LispThreadTouchEvent> TouchEvents,
    IReadOnlyList<ThreadResonanceEvidence> ResonanceEvidence,
    IReadOnlyList<DampingProfile> DampingProfiles,
    IReadOnlyList<DiscordanceRoute> DiscordanceRoutes,
    ActionAdmissionBoundary ActionAdmissionBoundary,
    CmeLispResonanceHeartbeatRefusalReceipt? Refusal,
    int PriorPassageCount,
    int PassageCountAfterResonanceReview,
    bool ReviewOnly,
    bool InertOnly,
    bool ListeningFrameMayReceive,
    bool EmanationBecomesAction,
    bool SoundBecomesAuthority,
    bool ResonanceAdmitsContinuity,
    bool DiscordanceBecomesFailure,
    bool DampingErasesWitness,
    bool RestBecomesAbsence,
    bool ThreadTouchEmitsPacket,
    bool RuntimeActionAllowed,
    bool LispEvaluationAllowed,
    bool ActivationRefused,
    DateTimeOffset TimestampUtc)
{
    public bool IsColdResonanceHeartbeat =>
        (Disposition is CmeLispResonanceHeartbeatDisposition.EmanationReviewCold or
            CmeLispResonanceHeartbeatDisposition.TouchReviewCold or
            CmeLispResonanceHeartbeatDisposition.DampingReviewCold or
            CmeLispResonanceHeartbeatDisposition.RestReviewCold) &&
        Refusal is null &&
        ReviewOnly &&
        InertOnly &&
        ListeningFrameMayReceive &&
        !EmanationBecomesAction &&
        !SoundBecomesAuthority &&
        !ResonanceAdmitsContinuity &&
        !DiscordanceBecomesFailure &&
        !DampingErasesWitness &&
        !RestBecomesAbsence &&
        !ThreadTouchEmitsPacket &&
        !RuntimeActionAllowed &&
        !LispEvaluationAllowed &&
        ActivationRefused &&
        PassageCountAfterResonanceReview == PriorPassageCount;

    public bool IsRetainedResonanceHeartbeatRefusal =>
        Disposition == CmeLispResonanceHeartbeatDisposition.Refused &&
        Refusal?.Retained == true &&
        PassageCountAfterResonanceReview == PriorPassageCount &&
        !EmanationBecomesAction &&
        !SoundBecomesAuthority &&
        !ResonanceAdmitsContinuity &&
        !DiscordanceBecomesFailure &&
        !DampingErasesWitness &&
        !RestBecomesAbsence &&
        !ThreadTouchEmitsPacket &&
        !RuntimeActionAllowed &&
        !LispEvaluationAllowed &&
        ActivationRefused;
}

public sealed class DefaultCmeLispListeningFrameResonanceHeartbeatBoundaryValidator
{
    public CmeLispResonanceHeartbeatReceipt Declare(
        CmeLispResonanceHeartbeatRequest request,
        DateTimeOffset timestampUtc)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.SourceFretboardReceipt is null ||
            !request.SourceFretboardReceipt.IsColdThreadFretboardStringing)
        {
            return Refuse(
                request,
                "cme-lisp-resonance-source-fretboard-missing",
                "Listening Frame resonance heartbeat refused because a cold CME Lisp thread fretboard receipt source is required.",
                timestampUtc);
        }

        if (!request.GlobalLaw.IsColdLaw)
        {
            return Refuse(
                request,
                "cme-lisp-global-resonance-law-promotional",
                "Global resonance law refused because sound, resonance, discordance, damping, rest, repetition, or amplitude attempted to become action, authority, continuity, failure, erasure, absence, or truth.",
                timestampUtc);
        }

        if (!request.HeartbeatPolicy.IsColdStewardPolicy)
        {
            return Refuse(
                request,
                "cme-lisp-heartbeat-not-steward-governed",
                "Steward heartbeat policy refused because heartbeat must be Steward-governed review cadence without owning resonance, bypassing review, admitting action, or admitting continuity.",
                timestampUtc);
        }

        if (!request.ActionAdmissionBoundary.IsColdAdmissionBoundary)
        {
            return Refuse(
                request,
                "cme-lisp-action-admission-boundary-missing",
                "Action admission boundary refused because sound may not become work without a present Steward review boundary that still refuses action on the cold bench.",
                timestampUtc);
        }

        if (request.Emanations.Any(static emanation => !emanation.IsColdEmanation))
        {
            return Refuse(
                request,
                "cme-lisp-emanation-promotional-refused",
                "Listening Frame emanation refused because harmonic reception must remain review-only, inert, non-action, non-authorizing, non-continuity, and non-activating.",
                timestampUtc);
        }

        var threadHandles = request.SourceFretboardReceipt.Threads
            .Select(static thread => thread.ThreadHandle)
            .ToHashSet(StringComparer.Ordinal);
        var actionThreadHandles = request.SourceFretboardReceipt.Threads
            .Where(static thread => thread.Kind == CmeLispThreadKind.Action)
            .Select(static thread => thread.ThreadHandle)
            .ToHashSet(StringComparer.Ordinal);

        if (request.TouchEvents.Any(touch => !threadHandles.Contains(touch.ThreadHandle)))
        {
            return Refuse(
                request,
                "cme-lisp-touch-thread-source-missing",
                "Lisp thread touch refused because every touch must bind to a declared fretboard thread.",
                timestampUtc);
        }

        if (request.TouchEvents.Any(static touch => !touch.IsColdTouch))
        {
            return Refuse(
                request,
                "cme-lisp-touch-promotional-refused",
                "Lisp thread touch refused because touching a thread may not emit packets, request runtime action, claim authority, claim continuity, bypass heartbeat, or leave inert review.",
                timestampUtc);
        }

        if (request.TouchEvents.Any(touch =>
            actionThreadHandles.Contains(touch.ThreadHandle) &&
            !touch.ActionAdmissionBoundaryPresent))
        {
            return Refuse(
                request,
                "cme-lisp-action-thread-touch-without-admission-boundary",
                "Action-thread touch refused because action-facing resonance requires a declared action admission boundary even when cold review refuses action.",
                timestampUtc);
        }

        var emanationHandles = request.Emanations
            .Select(static emanation => emanation.EmanationHandle)
            .ToHashSet(StringComparer.Ordinal);
        var touchHandles = request.TouchEvents
            .Select(static touch => touch.TouchHandle)
            .ToHashSet(StringComparer.Ordinal);

        if (request.ResonanceEvidence.Any(evidence =>
            !emanationHandles.Contains(evidence.EmanationHandle) ||
            !touchHandles.Contains(evidence.TouchHandle)))
        {
            return Refuse(
                request,
                "cme-lisp-resonance-evidence-source-missing",
                "Thread resonance evidence refused because every evidence record must bind to a declared emanation and touch event.",
                timestampUtc);
        }

        if (request.ResonanceEvidence.Any(static evidence => !evidence.IsColdEvidence))
        {
            return Refuse(
                request,
                "cme-lisp-resonance-evidence-promotional-refused",
                "Thread resonance evidence refused because evidence may be inspected but may not become warrant, action, authority, or continuity.",
                timestampUtc);
        }

        if (request.DampingProfiles.Any(static damping => !damping.IsColdDamping))
        {
            return Refuse(
                request,
                "cme-lisp-damping-erases-witness-refused",
                "Damping profile refused because damping may cool resonance without erasing witness, granting authority, or promoting continuity.",
                timestampUtc);
        }

        if (request.DiscordanceRoutes.Any(static route => !route.IsColdDiscordanceRoute))
        {
            return Refuse(
                request,
                "cme-lisp-discordance-as-failure-refused",
                "Discordance route refused because discordance may route review, cooling, or refusal but may not become failure, authority, or continuity by itself.",
                timestampUtc);
        }

        if (request.TouchEvents.Any(static touch => touch.TouchKind == CmeLispThreadTouchKind.Rest) &&
            request.GlobalLaw.RestMayMeanAbsence)
        {
            return Refuse(
                request,
                "cme-lisp-rest-as-absence-refused",
                "Rest refused because rest is lawful non-action, not absence.",
                timestampUtc);
        }

        var disposition = ResolveDisposition(request);
        var outcomeCode = disposition switch
        {
            CmeLispResonanceHeartbeatDisposition.RestReviewCold => "cme-lisp-rest-review-only",
            CmeLispResonanceHeartbeatDisposition.DampingReviewCold => "cme-lisp-damping-resonance-review-only",
            CmeLispResonanceHeartbeatDisposition.TouchReviewCold => "cme-lisp-thread-touch-review-only",
            _ => "cme-lisp-listening-frame-emanation-review-only"
        };

        var governanceTrace = disposition switch
        {
            CmeLispResonanceHeartbeatDisposition.RestReviewCold =>
                "Listening Frame received rest as lawful non-action under Steward heartbeat review. Rest grants no action, authority, continuity, packet emission, Lisp evaluation, passage, or activation.",
            CmeLispResonanceHeartbeatDisposition.DampingReviewCold =>
                "Listening Frame resonance produced damped evidence for review. Damping cools without erasing witness and grants no action, authority, continuity, packet emission, Lisp evaluation, passage, or activation.",
            CmeLispResonanceHeartbeatDisposition.TouchReviewCold =>
                "Listening Frame resonance touched declared Lisp threads for review. Touching a thread grants no action, authority, continuity, packet emission, Lisp evaluation, passage, or activation.",
            _ =>
                "Listening Frame received Shared Prime Reality harmonic emanation for review. Emanation grants no action, authority, continuity, packet emission, Lisp evaluation, passage, or activation."
        };

        return new CmeLispResonanceHeartbeatReceipt(
            ReceiptHandle: $"urn:san:cme-lisp-resonance-heartbeat:review:{ShortHash(request.SourceFretboardReceipt.ReceiptHandle, outcomeCode, request.TouchEvents.Count.ToString(System.Globalization.CultureInfo.InvariantCulture), request.ResonanceEvidence.Count.ToString(System.Globalization.CultureInfo.InvariantCulture), timestampUtc.UtcTicks.ToString(System.Globalization.CultureInfo.InvariantCulture))}",
            Disposition: disposition,
            OutcomeCode: outcomeCode,
            GovernanceTrace: governanceTrace,
            SourceFretboardReceiptHandle: request.SourceFretboardReceipt.ReceiptHandle,
            GlobalLaw: request.GlobalLaw,
            HeartbeatPolicy: request.HeartbeatPolicy,
            Emanations: request.Emanations.ToArray(),
            TouchEvents: request.TouchEvents.ToArray(),
            ResonanceEvidence: request.ResonanceEvidence.ToArray(),
            DampingProfiles: request.DampingProfiles.ToArray(),
            DiscordanceRoutes: request.DiscordanceRoutes.ToArray(),
            ActionAdmissionBoundary: request.ActionAdmissionBoundary,
            Refusal: null,
            PriorPassageCount: request.PriorPassageCount,
            PassageCountAfterResonanceReview: request.PriorPassageCount,
            ReviewOnly: true,
            InertOnly: true,
            ListeningFrameMayReceive: true,
            EmanationBecomesAction: false,
            SoundBecomesAuthority: false,
            ResonanceAdmitsContinuity: false,
            DiscordanceBecomesFailure: false,
            DampingErasesWitness: false,
            RestBecomesAbsence: false,
            ThreadTouchEmitsPacket: false,
            RuntimeActionAllowed: false,
            LispEvaluationAllowed: false,
            ActivationRefused: true,
            TimestampUtc: timestampUtc);
    }

    private static CmeLispResonanceHeartbeatDisposition ResolveDisposition(
        CmeLispResonanceHeartbeatRequest request)
    {
        if (request.TouchEvents.Any(static touch => touch.TouchKind == CmeLispThreadTouchKind.Rest))
        {
            return CmeLispResonanceHeartbeatDisposition.RestReviewCold;
        }

        if (request.ResonanceEvidence.Count > 0 || request.DampingProfiles.Count > 0 || request.DiscordanceRoutes.Count > 0)
        {
            return CmeLispResonanceHeartbeatDisposition.DampingReviewCold;
        }

        return request.TouchEvents.Count > 0
            ? CmeLispResonanceHeartbeatDisposition.TouchReviewCold
            : CmeLispResonanceHeartbeatDisposition.EmanationReviewCold;
    }

    private static CmeLispResonanceHeartbeatReceipt Refuse(
        CmeLispResonanceHeartbeatRequest request,
        string outcomeCode,
        string governanceTrace,
        DateTimeOffset timestampUtc)
    {
        var sourceHandle = request.SourceFretboardReceipt?.ReceiptHandle ?? "missing-fretboard-source";
        return new CmeLispResonanceHeartbeatReceipt(
            ReceiptHandle: $"urn:san:cme-lisp-resonance-heartbeat:refused:{ShortHash(sourceHandle, outcomeCode, timestampUtc.UtcTicks.ToString(System.Globalization.CultureInfo.InvariantCulture))}",
            Disposition: CmeLispResonanceHeartbeatDisposition.Refused,
            OutcomeCode: outcomeCode,
            GovernanceTrace: governanceTrace,
            SourceFretboardReceiptHandle: sourceHandle,
            GlobalLaw: request.GlobalLaw,
            HeartbeatPolicy: request.HeartbeatPolicy,
            Emanations: [],
            TouchEvents: [],
            ResonanceEvidence: [],
            DampingProfiles: [],
            DiscordanceRoutes: [],
            ActionAdmissionBoundary: request.ActionAdmissionBoundary,
            Refusal: new CmeLispResonanceHeartbeatRefusalReceipt(
                ReceiptHandle: $"urn:san:cme-lisp-resonance-heartbeat-refusal:{ShortHash(sourceHandle, outcomeCode)}",
                RefusalCode: outcomeCode,
                GovernanceTrace: governanceTrace,
                Retained: true),
            PriorPassageCount: request.PriorPassageCount,
            PassageCountAfterResonanceReview: request.PriorPassageCount,
            ReviewOnly: true,
            InertOnly: true,
            ListeningFrameMayReceive: false,
            EmanationBecomesAction: false,
            SoundBecomesAuthority: false,
            ResonanceAdmitsContinuity: false,
            DiscordanceBecomesFailure: false,
            DampingErasesWitness: false,
            RestBecomesAbsence: false,
            ThreadTouchEmitsPacket: false,
            RuntimeActionAllowed: false,
            LispEvaluationAllowed: false,
            ActivationRefused: true,
            TimestampUtc: timestampUtc);
    }

    private static string ShortHash(params string[] parts)
    {
        var payload = string.Join("|", parts);
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(payload));
        return Convert.ToHexString(bytes)[..16].ToLowerInvariant();
    }
}
