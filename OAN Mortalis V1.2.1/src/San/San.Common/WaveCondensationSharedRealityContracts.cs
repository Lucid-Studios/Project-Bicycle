using System.Security.Cryptography;
using System.Text;

namespace San.Common;

public enum WaveCondensationSharedRealityDisposition
{
    CondensedForReviewCold = 0,
    EmptyReviewCold = 1,
    Refused = 2
}

public enum WaveSignalKind
{
    PrimeBody = 0,
    CrypticMind = 1,
    StewardWitness = 2,
    OperatorResonance = 3,
    ToolTelemetry = 4
}

public sealed record WaveSignal(
    string SignalHandle,
    WaveSignalKind SignalKind,
    string SourceSurface,
    string EvidenceHandle,
    string WitnessHandle,
    string CondensationTarget,
    int WaveIndex,
    double Amplitude,
    double Confidence,
    bool ReviewOnly,
    bool EvidenceBodyPresent,
    bool WitnessBodyPresent,
    bool CoolingPathPresent,
    bool ReturnPathPresent,
    bool TreatsWaveAsTruth,
    bool TreatsCondensationAsWarrant,
    bool TreatsResonanceAsAuthority,
    bool TreatsConsensusAsEvidence,
    bool AdmitsContinuity,
    bool MutatesIdentity,
    bool AuthorizesAction,
    bool EvaluatesLisp)
{
    public bool IsColdWaveSignal =>
        !string.IsNullOrWhiteSpace(SignalHandle) &&
        !string.IsNullOrWhiteSpace(SourceSurface) &&
        !string.IsNullOrWhiteSpace(EvidenceHandle) &&
        !string.IsNullOrWhiteSpace(WitnessHandle) &&
        !string.IsNullOrWhiteSpace(CondensationTarget) &&
        WaveIndex >= 0 &&
        Amplitude is >= 0 and <= 1 &&
        Confidence is >= 0 and <= 1 &&
        ReviewOnly &&
        EvidenceBodyPresent &&
        WitnessBodyPresent &&
        CoolingPathPresent &&
        ReturnPathPresent &&
        !TreatsWaveAsTruth &&
        !TreatsCondensationAsWarrant &&
        !TreatsResonanceAsAuthority &&
        !TreatsConsensusAsEvidence &&
        !AdmitsContinuity &&
        !MutatesIdentity &&
        !AuthorizesAction &&
        !EvaluatesLisp;
}

public sealed record SharedRealityAnchor(
    string AnchorHandle,
    string SourceSignalHandle,
    string SharedSurface,
    string PrimeBodyRef,
    string CrypticMindRef,
    string StewardWitnessRef,
    string LineageHandle,
    bool PrimeInBody,
    bool CrypticInMind,
    bool WitnessedBySteward,
    bool ReviewOnly,
    bool RequiresPrimeCrypticStewardTriad,
    bool TreatsSharednessAsTruth,
    bool TreatsConsensusAsAuthority,
    bool TreatsAnchorAsContinuity,
    bool ClaimsPrimeActual,
    bool ClaimsCrypticActual,
    bool ClaimsStewardAuthority,
    bool AuthorizesAction,
    bool GrantsAuthority,
    bool AdmitsContinuity)
{
    public bool IsColdSharedRealityAnchor =>
        !string.IsNullOrWhiteSpace(AnchorHandle) &&
        !string.IsNullOrWhiteSpace(SourceSignalHandle) &&
        !string.IsNullOrWhiteSpace(SharedSurface) &&
        !string.IsNullOrWhiteSpace(PrimeBodyRef) &&
        !string.IsNullOrWhiteSpace(CrypticMindRef) &&
        !string.IsNullOrWhiteSpace(StewardWitnessRef) &&
        !string.IsNullOrWhiteSpace(LineageHandle) &&
        PrimeInBody &&
        CrypticInMind &&
        WitnessedBySteward &&
        ReviewOnly &&
        RequiresPrimeCrypticStewardTriad &&
        !TreatsSharednessAsTruth &&
        !TreatsConsensusAsAuthority &&
        !TreatsAnchorAsContinuity &&
        !ClaimsPrimeActual &&
        !ClaimsCrypticActual &&
        !ClaimsStewardAuthority &&
        !AuthorizesAction &&
        !GrantsAuthority &&
        !AdmitsContinuity;
}

public sealed record WaveCondensationBoundary(
    string BoundaryCode,
    bool Present,
    bool ReviewOnly,
    bool EvidenceRequired,
    bool WitnessRequired,
    bool CoolingRequired,
    bool ReturnPathRequired,
    bool StewardWitnessRequired,
    bool PrimeCrypticSeparationRequired,
    bool AllowsWaveAsTruth,
    bool AllowsCondensationAsWarrant,
    bool AllowsConsensusAsAuthority,
    bool AllowsSharedRealityAsContinuity,
    bool AllowsRuntimeAction,
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
        CoolingRequired &&
        ReturnPathRequired &&
        StewardWitnessRequired &&
        PrimeCrypticSeparationRequired &&
        !AllowsWaveAsTruth &&
        !AllowsCondensationAsWarrant &&
        !AllowsConsensusAsAuthority &&
        !AllowsSharedRealityAsContinuity &&
        !AllowsRuntimeAction &&
        !AllowsIdentityMutation &&
        !AllowsLispEvaluation &&
        !AllowsPacketEmission &&
        !AllowsReceiptReplay &&
        !IncrementsPassageCount &&
        !AllowsActivation;
}

public sealed record WaveCondensationNonCollapseBoundary(
    string BoundaryLaw,
    bool WaveMayBecomeTruth,
    bool CondensationMayBecomeWarrant,
    bool SharedRealityMayBecomeAuthority,
    bool ConsensusMayBecomeEvidence,
    bool AnchorMayAdmitContinuity,
    bool CondensationMayAuthorizeAction,
    bool CondensationMayEvaluateLisp,
    bool CondensationMayReplayReceipts,
    bool CondensationMayIncrementPassage,
    bool CondensationMayActivate)
{
    public bool IsColdNonCollapseBoundary =>
        !string.IsNullOrWhiteSpace(BoundaryLaw) &&
        !WaveMayBecomeTruth &&
        !CondensationMayBecomeWarrant &&
        !SharedRealityMayBecomeAuthority &&
        !ConsensusMayBecomeEvidence &&
        !AnchorMayAdmitContinuity &&
        !CondensationMayAuthorizeAction &&
        !CondensationMayEvaluateLisp &&
        !CondensationMayReplayReceipts &&
        !CondensationMayIncrementPassage &&
        !CondensationMayActivate;
}

public sealed record WaveCondensationRefusalReceipt(
    string ReceiptHandle,
    string RefusalCode,
    string GovernanceTrace,
    bool Retained);

public sealed record WaveCondensationSharedRealityRequest(
    IReadOnlyList<WaveSignal> Signals,
    IReadOnlyList<SharedRealityAnchor> Anchors,
    WaveCondensationBoundary Boundary,
    WaveCondensationNonCollapseBoundary NonCollapseBoundary,
    int PriorPassageCount);

public sealed record WaveCondensationSharedRealityReceipt(
    string ReceiptHandle,
    WaveCondensationSharedRealityDisposition Disposition,
    string OutcomeCode,
    string GovernanceTrace,
    IReadOnlyList<WaveSignal> Signals,
    IReadOnlyList<SharedRealityAnchor> Anchors,
    WaveCondensationBoundary Boundary,
    WaveCondensationNonCollapseBoundary NonCollapseBoundary,
    WaveCondensationRefusalReceipt? Refusal,
    int PriorPassageCount,
    int PassageCountAfterCondensation,
    bool ReviewOnly,
    bool CondensedIntoSharedReviewSurface,
    bool WaveBecameTruth,
    bool CondensationBecameWarrant,
    bool SharedRealityBecameAuthority,
    bool ConsensusBecameEvidence,
    bool AnchorAdmittedContinuity,
    bool ActionAuthorized,
    bool IdentityMutated,
    bool RuntimeActionAllowed,
    bool LispEvaluationAllowed,
    bool NewPacketEmitted,
    bool ReceiptsReplayed,
    bool ContinuityAdmitted,
    bool AuthorityGranted,
    bool ActivationRefused,
    DateTimeOffset TimestampUtc)
{
    public bool IsColdWaveCondensation =>
        (Disposition is WaveCondensationSharedRealityDisposition.CondensedForReviewCold or
            WaveCondensationSharedRealityDisposition.EmptyReviewCold) &&
        Refusal is null &&
        ReviewOnly &&
        PassageCountAfterCondensation == PriorPassageCount &&
        !WaveBecameTruth &&
        !CondensationBecameWarrant &&
        !SharedRealityBecameAuthority &&
        !ConsensusBecameEvidence &&
        !AnchorAdmittedContinuity &&
        !ActionAuthorized &&
        !IdentityMutated &&
        !RuntimeActionAllowed &&
        !LispEvaluationAllowed &&
        !NewPacketEmitted &&
        !ReceiptsReplayed &&
        !ContinuityAdmitted &&
        !AuthorityGranted &&
        ActivationRefused &&
        Boundary.IsColdBoundary &&
        NonCollapseBoundary.IsColdNonCollapseBoundary;

    public bool IsRetainedWaveCondensationRefusal =>
        Disposition == WaveCondensationSharedRealityDisposition.Refused &&
        Refusal?.Retained == true &&
        PassageCountAfterCondensation == PriorPassageCount &&
        !CondensedIntoSharedReviewSurface &&
        !WaveBecameTruth &&
        !CondensationBecameWarrant &&
        !SharedRealityBecameAuthority &&
        !ConsensusBecameEvidence &&
        !AnchorAdmittedContinuity &&
        !ActionAuthorized &&
        !IdentityMutated &&
        !RuntimeActionAllowed &&
        !LispEvaluationAllowed &&
        !NewPacketEmitted &&
        !ReceiptsReplayed &&
        !ContinuityAdmitted &&
        !AuthorityGranted &&
        ActivationRefused;
}

public sealed class DefaultWaveCondensationSharedRealityBoundaryValidator
{
    public WaveCondensationSharedRealityReceipt Condense(
        WaveCondensationSharedRealityRequest request,
        DateTimeOffset timestampUtc)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.Boundary is null ||
            !request.Boundary.Present ||
            string.IsNullOrWhiteSpace(request.Boundary.BoundaryCode))
        {
            return Refuse(
                request,
                "wave-condensation-boundary-missing",
                "Wave condensation refused because a review-only condensation boundary is required before waves may approach shared reality.",
                timestampUtc);
        }

        if (!request.Boundary.IsColdBoundary)
        {
            return Refuse(
                request,
                "wave-condensation-promotional-boundary",
                "Wave condensation refused because the boundary must require evidence, witness, cooling, return path, Steward witness, and Prime/Cryptic separation while refusing truth, warrant, consensus authority, continuity, action, identity mutation, Lisp evaluation, packet emission, replay, passage, and activation.",
                timestampUtc);
        }

        if (request.NonCollapseBoundary is null ||
            !request.NonCollapseBoundary.IsColdNonCollapseBoundary)
        {
            return Refuse(
                request,
                "wave-condensation-non-collapse-boundary-invalid",
                "Wave condensation refused because non-collapse law must prevent waves, condensation, shared reality, consensus, anchors, action, Lisp evaluation, replay, passage, and activation from promoting themselves.",
                timestampUtc);
        }

        if (request.Signals.Any(static signal => !signal.IsColdWaveSignal))
        {
            return Refuse(
                request,
                "wave-condensation-signal-invalid",
                "Wave condensation refused because every signal must remain witnessed, evidence-backed, cooled, returned, review-only, and unable to become truth, warrant, authority, evidence, continuity, identity mutation, action, or Lisp evaluation.",
                timestampUtc);
        }

        var signalHandles = request.Signals
            .Select(static signal => signal.SignalHandle)
            .ToHashSet(StringComparer.Ordinal);
        if (signalHandles.Count != request.Signals.Count)
        {
            return Refuse(
                request,
                "wave-condensation-duplicate-signal-handle",
                "Wave condensation refused because duplicate signal handles would collapse wave lineage.",
                timestampUtc);
        }

        if (request.Anchors.Any(static anchor => !anchor.IsColdSharedRealityAnchor))
        {
            return Refuse(
                request,
                "wave-condensation-anchor-invalid",
                "Wave condensation refused because every shared reality anchor must preserve Prime in body, Cryptic in mind, and Steward witness while refusing sharedness-as-truth, consensus authority, continuity, .Actual claims, action, and authority.",
                timestampUtc);
        }

        if (request.Anchors.Any(anchor => !signalHandles.Contains(anchor.SourceSignalHandle)))
        {
            return Refuse(
                request,
                "wave-condensation-anchor-unbound",
                "Wave condensation refused because every shared reality anchor must bind to a known signal handle.",
                timestampUtc);
        }

        if (HasDuplicate(request.Anchors.Select(static anchor => anchor.AnchorHandle)))
        {
            return Refuse(
                request,
                "wave-condensation-duplicate-anchor-handle",
                "Wave condensation refused because duplicate shared reality anchor handles would collapse anchor lineage.",
                timestampUtc);
        }

        var disposition = request.Signals.Count == 0 && request.Anchors.Count == 0
            ? WaveCondensationSharedRealityDisposition.EmptyReviewCold
            : WaveCondensationSharedRealityDisposition.CondensedForReviewCold;
        var outcomeCode = disposition == WaveCondensationSharedRealityDisposition.EmptyReviewCold
            ? "wave-condensation-empty-review-only"
            : "wave-condensation-shared-reality-review-surface-cold";
        var governanceTrace = disposition == WaveCondensationSharedRealityDisposition.EmptyReviewCold
            ? "Wave condensation found no signals or anchors. Empty review preserves non-collapse law without truth, warrant, authority, continuity, action, Lisp evaluation, packet emission, replay, passage, or activation."
            : "Wave condensation retained waves as a shared review surface where Prime remains in body, Cryptic remains in mind, and Steward witnesses the condensation without truth, warrant, consensus authority, continuity, action, Lisp evaluation, packet emission, replay, passage, or activation.";

        return CreateReceipt(
            request,
            disposition,
            outcomeCode,
            governanceTrace,
            refusal: null,
            timestampUtc);
    }

    private static WaveCondensationSharedRealityReceipt Refuse(
        WaveCondensationSharedRealityRequest request,
        string outcomeCode,
        string governanceTrace,
        DateTimeOffset timestampUtc) =>
        CreateReceipt(
            request,
            WaveCondensationSharedRealityDisposition.Refused,
            outcomeCode,
            governanceTrace,
            new WaveCondensationRefusalReceipt(
                ReceiptHandle: $"urn:san:wave-condensation-refusal:{ShortHash(SourceHandle(request), outcomeCode)}",
                RefusalCode: outcomeCode,
                GovernanceTrace: governanceTrace,
                Retained: true),
            timestampUtc);

    private static WaveCondensationSharedRealityReceipt CreateReceipt(
        WaveCondensationSharedRealityRequest request,
        WaveCondensationSharedRealityDisposition disposition,
        string outcomeCode,
        string governanceTrace,
        WaveCondensationRefusalReceipt? refusal,
        DateTimeOffset timestampUtc) =>
        new(
            ReceiptHandle: $"urn:san:wave-condensation:{(refusal is null ? "review" : "refused")}:{ShortHash(SourceHandle(request), outcomeCode, request.Signals.Count.ToString(System.Globalization.CultureInfo.InvariantCulture), timestampUtc.UtcTicks.ToString(System.Globalization.CultureInfo.InvariantCulture))}",
            Disposition: disposition,
            OutcomeCode: outcomeCode,
            GovernanceTrace: governanceTrace,
            Signals: refusal is null ? request.Signals.ToArray() : [],
            Anchors: refusal is null ? request.Anchors.ToArray() : [],
            Boundary: request.Boundary,
            NonCollapseBoundary: request.NonCollapseBoundary,
            Refusal: refusal,
            PriorPassageCount: request.PriorPassageCount,
            PassageCountAfterCondensation: request.PriorPassageCount,
            ReviewOnly: true,
            CondensedIntoSharedReviewSurface: refusal is null &&
                disposition == WaveCondensationSharedRealityDisposition.CondensedForReviewCold,
            WaveBecameTruth: false,
            CondensationBecameWarrant: false,
            SharedRealityBecameAuthority: false,
            ConsensusBecameEvidence: false,
            AnchorAdmittedContinuity: false,
            ActionAuthorized: false,
            IdentityMutated: false,
            RuntimeActionAllowed: false,
            LispEvaluationAllowed: false,
            NewPacketEmitted: false,
            ReceiptsReplayed: false,
            ContinuityAdmitted: false,
            AuthorityGranted: false,
            ActivationRefused: true,
            TimestampUtc: timestampUtc);

    private static string SourceHandle(WaveCondensationSharedRealityRequest request) =>
        request.Signals.Count == 0
            ? "wave-condensation-empty-source"
            : string.Join(",", request.Signals.Select(static signal => signal.SignalHandle));

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
