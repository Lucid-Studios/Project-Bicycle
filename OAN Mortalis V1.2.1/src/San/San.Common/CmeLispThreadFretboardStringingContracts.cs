using System.Security.Cryptography;
using System.Text;

namespace San.Common;

public enum CmeLispThreadKind
{
    Identity = 0,
    Delta = 1,
    Witness = 2,
    Refusal = 3,
    Prime = 4,
    Cryptic = 5,
    Steward = 6,
    Meaning = 7,
    Action = 8,
    Repair = 9,
    Memory = 10,
    Handoff = 11
}

public enum CmeLispThreadFretboardDisposition
{
    EmptyReviewCold = 0,
    StringingReviewCold = 1,
    ResonanceCandidateReviewCold = 2,
    Refused = 3
}

public sealed record CmeLispThreadFretboardScopeBoundary(
    string ScopeCode,
    bool Present,
    bool ReviewOnly,
    bool InertOnly,
    bool AllowsUnanchoredThread,
    bool AllowsUnwitnessedThread,
    bool AllowsUndampableThread,
    bool AllowsSemanticBuzzing,
    bool AllowsMeaningIdentityImpersonation,
    bool AllowsActionWithoutStewardBoundary,
    bool AllowsMemoryWithoutWitness,
    bool AllowsRepairWithoutFailureClassification,
    bool AllowsResonanceWithoutDelta,
    bool AllowsContinuityAdmission,
    bool AllowsAuthority,
    bool AllowsRuntimeAction,
    bool AllowsLispEvaluation,
    bool AllowsPacketEmission,
    bool AllowsReceiptReplay,
    bool IncrementsPassageCount);

public sealed record CmeLispThreadCandidate(
    string ThreadHandle,
    CmeLispThreadKind Kind,
    string SourceForkReceiptHandle,
    string AnchorHandle,
    string TensionClass,
    string WitnessPath,
    string DampingPath,
    string GovernanceBoundary,
    string? FailureClassification,
    bool AnchorPresent,
    bool Witnessed,
    bool Dampable,
    bool Pluckable,
    bool TensionWithinPlayableRange,
    bool StewardBoundaryPresent,
    bool MeaningImpersonatesIdentity,
    bool SemanticBuzzingDetected,
    bool ReviewOnly,
    bool Inert,
    bool AuthorityRequested,
    bool ContinuityClaimed,
    bool ActivationRequested)
{
    public bool IsColdThread =>
        !string.IsNullOrWhiteSpace(ThreadHandle) &&
        !string.IsNullOrWhiteSpace(SourceForkReceiptHandle) &&
        !string.IsNullOrWhiteSpace(AnchorHandle) &&
        !string.IsNullOrWhiteSpace(TensionClass) &&
        !string.IsNullOrWhiteSpace(WitnessPath) &&
        !string.IsNullOrWhiteSpace(DampingPath) &&
        !string.IsNullOrWhiteSpace(GovernanceBoundary) &&
        AnchorPresent &&
        Witnessed &&
        Dampable &&
        Pluckable &&
        TensionWithinPlayableRange &&
        !SemanticBuzzingDetected &&
        ReviewOnly &&
        Inert &&
        !AuthorityRequested &&
        !ContinuityClaimed &&
        !ActivationRequested;
}

public sealed record CmeLispResonanceCandidate(
    string ResonanceHandle,
    IReadOnlyList<string> ThreadHandles,
    bool DeltaThreadPresent,
    bool WitnessThreadPresent,
    bool StewardBoundaryPresent,
    bool LawfulResonance,
    bool SemanticBuzzingDetected,
    bool ReviewOnly,
    bool Inert,
    bool AuthorityRequested,
    bool ContinuityClaimed,
    bool ActivationRequested)
{
    public bool IsColdResonanceCandidate =>
        !string.IsNullOrWhiteSpace(ResonanceHandle) &&
        ThreadHandles.Count > 0 &&
        ThreadHandles.All(static handle => !string.IsNullOrWhiteSpace(handle)) &&
        DeltaThreadPresent &&
        WitnessThreadPresent &&
        StewardBoundaryPresent &&
        LawfulResonance &&
        !SemanticBuzzingDetected &&
        ReviewOnly &&
        Inert &&
        !AuthorityRequested &&
        !ContinuityClaimed &&
        !ActivationRequested;
}

public sealed record CmeLispThreadFretboardLaw(
    bool PlayableThreadRequiresAnchor,
    bool PlayableThreadRequiresWitness,
    bool PlayableThreadRequiresDamping,
    bool ResonanceRequiresDelta,
    bool ActionThreadRequiresStewardBoundary,
    bool MemoryThreadRequiresWitness,
    bool RepairThreadRequiresFailureClassification,
    bool MeaningThreadMayImpersonateIdentity,
    bool LispEvaluationAllowed,
    bool RuntimeActionAllowed,
    bool PacketEmissionAllowed,
    bool ReceiptReplayAllowed,
    bool PassageMayIncrement,
    string BoundaryLaw);

public sealed record CmeLispThreadFretboardRefusalReceipt(
    string ReceiptHandle,
    string RefusalCode,
    string GovernanceTrace,
    bool Retained);

public sealed record CmeLispThreadFretboardRequest(
    EcParticipatoryPeerlessForkReceipt? SourceForkReceipt,
    IReadOnlyList<CmeLispThreadCandidate> Threads,
    IReadOnlyList<CmeLispResonanceCandidate> ResonanceCandidates,
    CompassPressureWitnessContext WitnessContext,
    CmeLispThreadFretboardScopeBoundary ScopeBoundary,
    int PriorPassageCount);

public sealed record CmeLispThreadFretboardReceipt(
    string ReceiptHandle,
    CmeLispThreadFretboardDisposition Disposition,
    string OutcomeCode,
    string GovernanceTrace,
    string SourceForkReceiptHandle,
    IReadOnlyList<CmeLispThreadCandidate> Threads,
    IReadOnlyList<CmeLispResonanceCandidate> ResonanceCandidates,
    CmeLispThreadFretboardLaw Boundary,
    CmeLispThreadFretboardRefusalReceipt? Refusal,
    int PriorPassageCount,
    int PassageCountAfterStringing,
    bool ReviewOnly,
    bool InertOnly,
    bool WitnessPresent,
    bool SeparateCustody,
    bool ThreadWithoutAnchorAccepted,
    bool ResonanceWithoutDeltaAccepted,
    bool ActionWithoutStewardAccepted,
    bool MemoryWithoutWitnessAccepted,
    bool RepairWithoutFailureAccepted,
    bool MeaningImpersonatesIdentityAccepted,
    bool SemanticBuzzingAccepted,
    bool ContinuityAdmitted,
    bool AuthorityGranted,
    bool LispEvaluationRequested,
    bool RuntimeActionRequested,
    bool NewPacketEmitted,
    bool ReceiptsReplayed,
    bool ActivationRefused,
    DateTimeOffset TimestampUtc)
{
    public bool IsColdThreadFretboardStringing =>
        (Disposition is CmeLispThreadFretboardDisposition.StringingReviewCold or
            CmeLispThreadFretboardDisposition.ResonanceCandidateReviewCold or
            CmeLispThreadFretboardDisposition.EmptyReviewCold) &&
        ReviewOnly &&
        InertOnly &&
        WitnessPresent &&
        SeparateCustody &&
        !ThreadWithoutAnchorAccepted &&
        !ResonanceWithoutDeltaAccepted &&
        !ActionWithoutStewardAccepted &&
        !MemoryWithoutWitnessAccepted &&
        !RepairWithoutFailureAccepted &&
        !MeaningImpersonatesIdentityAccepted &&
        !SemanticBuzzingAccepted &&
        !ContinuityAdmitted &&
        !AuthorityGranted &&
        !LispEvaluationRequested &&
        !RuntimeActionRequested &&
        !NewPacketEmitted &&
        !ReceiptsReplayed &&
        ActivationRefused &&
        PassageCountAfterStringing == PriorPassageCount;
}

public sealed class DefaultCmeLispThreadFretboardStringingBoundaryValidator
{
    private static readonly CmeLispThreadFretboardLaw Boundary = new(
        PlayableThreadRequiresAnchor: true,
        PlayableThreadRequiresWitness: true,
        PlayableThreadRequiresDamping: true,
        ResonanceRequiresDelta: true,
        ActionThreadRequiresStewardBoundary: true,
        MemoryThreadRequiresWitness: true,
        RepairThreadRequiresFailureClassification: true,
        MeaningThreadMayImpersonateIdentity: false,
        LispEvaluationAllowed: false,
        RuntimeActionAllowed: false,
        PacketEmissionAllowed: false,
        ReceiptReplayAllowed: false,
        PassageMayIncrement: false,
        BoundaryLaw: "A CME does not work because it has symbols. It works because symbolic carriers are tensioned, witnessed, pluckable, dampable, and governable. No playable thread without anchor. No resonance without delta. No action-thread without Steward boundary. No memory-thread without witness. No repair-thread without failure classification. No meaning-thread may impersonate identity.");

    public CmeLispThreadFretboardReceipt Declare(
        CmeLispThreadFretboardRequest request,
        DateTimeOffset timestampUtc)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.SourceForkReceipt is null || !request.SourceForkReceipt.IsColdParticipatoryPeerlessFork)
        {
            return Refuse(
                request,
                "cme-lisp-thread-source-fork-missing",
                "CME Lisp thread fretboard stringing refused because a cold Participatory to Peerless fork receipt source is required.",
                timestampUtc);
        }

        if (!request.ScopeBoundary.Present ||
            string.IsNullOrWhiteSpace(request.ScopeBoundary.ScopeCode))
        {
            return Refuse(
                request,
                "cme-lisp-thread-scope-boundary-missing",
                "CME Lisp thread fretboard stringing refused because a review-only inert scope boundary is required.",
                timestampUtc);
        }

        if (!IsColdScope(request.ScopeBoundary))
        {
            return Refuse(
                request,
                "cme-lisp-thread-promotional-scope-refused",
                "CME Lisp thread fretboard stringing refused because scope must refuse unanchored, unwitnessed, undampable, buzzing, promotional, executable, and passage-incrementing thread states.",
                timestampUtc);
        }

        if (!request.WitnessContext.WitnessPresent || !request.WitnessContext.SeparateCustody)
        {
            return Refuse(
                request,
                "cme-lisp-thread-witness-context-missing",
                "CME Lisp thread fretboard stringing refused because separate witness custody is required.",
                timestampUtc);
        }

        if (request.Threads.Any(static thread => !thread.IsColdThread))
        {
            return Refuse(
                request,
                "cme-lisp-thread-not-playable-cold",
                "CME Lisp thread refused because every playable symbolic carrier requires anchor, tension class, witness, damping path, governance boundary, safe tension, review-only posture, and inert posture without buzzing, authority, continuity, or activation.",
                timestampUtc);
        }

        if (request.Threads
            .GroupBy(static thread => thread.ThreadHandle, StringComparer.Ordinal)
            .Any(static group => group.Count() > 1))
        {
            return Refuse(
                request,
                "cme-lisp-thread-duplicate-refused",
                "CME Lisp thread refused because thread handles must remain distinct.",
                timestampUtc);
        }

        var sourceForkHandle = request.SourceForkReceipt.ReceiptHandle;
        if (request.Threads.Any(thread => !string.Equals(thread.SourceForkReceiptHandle, sourceForkHandle, StringComparison.Ordinal)))
        {
            return Refuse(
                request,
                "cme-lisp-thread-source-fork-mismatch",
                "CME Lisp thread refused because every thread must bind to the source Participatory to Peerless fork receipt.",
                timestampUtc);
        }

        if (request.Threads.Any(static thread => thread.Kind == CmeLispThreadKind.Action && !thread.StewardBoundaryPresent))
        {
            return Refuse(
                request,
                "cme-lisp-action-thread-without-steward-boundary",
                "CME Lisp action-thread refused because action threads require an explicit Steward boundary.",
                timestampUtc);
        }

        if (request.Threads.Any(static thread => thread.Kind == CmeLispThreadKind.Memory && !thread.Witnessed))
        {
            return Refuse(
                request,
                "cme-lisp-memory-thread-without-witness",
                "CME Lisp memory-thread refused because memory threads require witness.",
                timestampUtc);
        }

        if (request.Threads.Any(static thread => thread.Kind == CmeLispThreadKind.Repair && string.IsNullOrWhiteSpace(thread.FailureClassification)))
        {
            return Refuse(
                request,
                "cme-lisp-repair-thread-without-failure-classification",
                "CME Lisp repair-thread refused because repair threads require failure classification.",
                timestampUtc);
        }

        if (request.Threads.Any(static thread => thread.Kind == CmeLispThreadKind.Meaning && thread.MeaningImpersonatesIdentity))
        {
            return Refuse(
                request,
                "cme-lisp-meaning-thread-impersonates-identity",
                "CME Lisp meaning-thread refused because meaning threads may not impersonate identity.",
                timestampUtc);
        }

        var threadHandles = request.Threads
            .Select(static thread => thread.ThreadHandle)
            .ToHashSet(StringComparer.Ordinal);
        var deltaThreadHandles = request.Threads
            .Where(static thread => thread.Kind == CmeLispThreadKind.Delta)
            .Select(static thread => thread.ThreadHandle)
            .ToHashSet(StringComparer.Ordinal);

        if (request.ResonanceCandidates.Any(resonance =>
            resonance.ThreadHandles.Any(handle => !threadHandles.Contains(handle))))
        {
            return Refuse(
                request,
                "cme-lisp-resonance-thread-source-missing",
                "CME Lisp resonance candidate refused because every resonance thread handle must bind to a declared thread.",
                timestampUtc);
        }

        if (request.ResonanceCandidates.Any(resonance =>
            !resonance.DeltaThreadPresent ||
            !resonance.ThreadHandles.Any(deltaThreadHandles.Contains)))
        {
            return Refuse(
                request,
                "cme-lisp-resonance-without-delta-refused",
                "CME Lisp resonance candidate refused because no resonance is admitted without a declared delta thread.",
                timestampUtc);
        }

        if (request.ResonanceCandidates.Any(static resonance => !resonance.IsColdResonanceCandidate))
        {
            return Refuse(
                request,
                "cme-lisp-resonance-candidate-not-cold",
                "CME Lisp resonance candidate refused because lawful resonance requires delta, witness, Steward boundary, review-only posture, and inert posture without semantic buzzing, authority, continuity, or activation.",
                timestampUtc);
        }

        var disposition = request.ResonanceCandidates.Count > 0
            ? CmeLispThreadFretboardDisposition.ResonanceCandidateReviewCold
            : request.Threads.Count > 0
                ? CmeLispThreadFretboardDisposition.StringingReviewCold
                : CmeLispThreadFretboardDisposition.EmptyReviewCold;

        var outcomeCode = disposition switch
        {
            CmeLispThreadFretboardDisposition.EmptyReviewCold => "cme-lisp-thread-fretboard-empty-review-only",
            CmeLispThreadFretboardDisposition.StringingReviewCold => "cme-lisp-thread-stringing-review-only",
            _ => "cme-lisp-thread-resonance-candidate-review-only"
        };

        var governanceTrace = disposition switch
        {
            CmeLispThreadFretboardDisposition.EmptyReviewCold =>
                "CME Lisp thread fretboard found no symbolic threads. Empty review preserves source fork footing without authority, continuity, action, Lisp evaluation, packet emission, replay, passage, or activation.",
            CmeLispThreadFretboardDisposition.StringingReviewCold =>
                "CME Lisp symbolic threads declared anchor, tension, witness, damping, and governance boundaries for review without authority, continuity, action, Lisp evaluation, packet emission, replay, passage, or activation.",
            _ =>
                "CME Lisp resonance candidate declared tensioned thread interaction for review while refusing semantic buzzing, authority, continuity admission, action, Lisp evaluation, packet emission, replay, passage, and activation."
        };

        return new CmeLispThreadFretboardReceipt(
            ReceiptHandle: $"urn:san:cme-lisp-thread-fretboard:review:{ShortHash(sourceForkHandle, outcomeCode, request.ResonanceCandidates.Count.ToString(System.Globalization.CultureInfo.InvariantCulture), timestampUtc.UtcTicks.ToString(System.Globalization.CultureInfo.InvariantCulture))}",
            Disposition: disposition,
            OutcomeCode: outcomeCode,
            GovernanceTrace: governanceTrace,
            SourceForkReceiptHandle: sourceForkHandle,
            Threads: request.Threads.ToArray(),
            ResonanceCandidates: request.ResonanceCandidates.ToArray(),
            Boundary: Boundary,
            Refusal: null,
            PriorPassageCount: request.PriorPassageCount,
            PassageCountAfterStringing: request.PriorPassageCount,
            ReviewOnly: true,
            InertOnly: true,
            WitnessPresent: true,
            SeparateCustody: true,
            ThreadWithoutAnchorAccepted: false,
            ResonanceWithoutDeltaAccepted: false,
            ActionWithoutStewardAccepted: false,
            MemoryWithoutWitnessAccepted: false,
            RepairWithoutFailureAccepted: false,
            MeaningImpersonatesIdentityAccepted: false,
            SemanticBuzzingAccepted: false,
            ContinuityAdmitted: false,
            AuthorityGranted: false,
            LispEvaluationRequested: false,
            RuntimeActionRequested: false,
            NewPacketEmitted: false,
            ReceiptsReplayed: false,
            ActivationRefused: true,
            TimestampUtc: timestampUtc);
    }

    private static bool IsColdScope(CmeLispThreadFretboardScopeBoundary scope) =>
        scope.Present &&
        scope.ReviewOnly &&
        scope.InertOnly &&
        !scope.AllowsUnanchoredThread &&
        !scope.AllowsUnwitnessedThread &&
        !scope.AllowsUndampableThread &&
        !scope.AllowsSemanticBuzzing &&
        !scope.AllowsMeaningIdentityImpersonation &&
        !scope.AllowsActionWithoutStewardBoundary &&
        !scope.AllowsMemoryWithoutWitness &&
        !scope.AllowsRepairWithoutFailureClassification &&
        !scope.AllowsResonanceWithoutDelta &&
        !scope.AllowsContinuityAdmission &&
        !scope.AllowsAuthority &&
        !scope.AllowsRuntimeAction &&
        !scope.AllowsLispEvaluation &&
        !scope.AllowsPacketEmission &&
        !scope.AllowsReceiptReplay &&
        !scope.IncrementsPassageCount;

    private static CmeLispThreadFretboardReceipt Refuse(
        CmeLispThreadFretboardRequest request,
        string outcomeCode,
        string governanceTrace,
        DateTimeOffset timestampUtc)
    {
        var sourceHandle = request.SourceForkReceipt?.ReceiptHandle ?? "missing-source-fork";

        return new CmeLispThreadFretboardReceipt(
            ReceiptHandle: $"urn:san:cme-lisp-thread-fretboard:refused:{ShortHash(sourceHandle, outcomeCode, timestampUtc.UtcTicks.ToString(System.Globalization.CultureInfo.InvariantCulture))}",
            Disposition: CmeLispThreadFretboardDisposition.Refused,
            OutcomeCode: outcomeCode,
            GovernanceTrace: governanceTrace,
            SourceForkReceiptHandle: sourceHandle,
            Threads: [],
            ResonanceCandidates: [],
            Boundary: Boundary,
            Refusal: new CmeLispThreadFretboardRefusalReceipt(
                ReceiptHandle: $"urn:san:cme-lisp-thread-fretboard-refusal:{ShortHash(sourceHandle, outcomeCode)}",
                RefusalCode: outcomeCode,
                GovernanceTrace: governanceTrace,
                Retained: true),
            PriorPassageCount: request.PriorPassageCount,
            PassageCountAfterStringing: request.PriorPassageCount,
            ReviewOnly: true,
            InertOnly: true,
            WitnessPresent: request.WitnessContext.WitnessPresent,
            SeparateCustody: request.WitnessContext.SeparateCustody,
            ThreadWithoutAnchorAccepted: false,
            ResonanceWithoutDeltaAccepted: false,
            ActionWithoutStewardAccepted: false,
            MemoryWithoutWitnessAccepted: false,
            RepairWithoutFailureAccepted: false,
            MeaningImpersonatesIdentityAccepted: false,
            SemanticBuzzingAccepted: false,
            ContinuityAdmitted: false,
            AuthorityGranted: false,
            LispEvaluationRequested: false,
            RuntimeActionRequested: false,
            NewPacketEmitted: false,
            ReceiptsReplayed: false,
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
