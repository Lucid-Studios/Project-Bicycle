using System.Security.Cryptography;
using System.Text;

namespace San.Common;

public enum HarmonicInterlockOutcome
{
    Align = 0,
    Sequence = 1,
    Damp = 2,
    Split = 3,
    Cool = 4,
    Refuse = 5
}

public enum StewardHarmonicCustodyInterlockDisposition
{
    AlignReviewCold = 0,
    SequenceReviewCold = 1,
    DampReviewCold = 2,
    SplitReviewCold = 3,
    CoolReviewCold = 4,
    RefusalReviewCold = 5,
    Refused = 6
}

public sealed record LawfulSignalCandidate(
    string SignalHandle,
    string SourceReceiptHandle,
    string ThreadHandle,
    string SharedSurfaceHandle,
    decimal CadenceOrdinal,
    decimal ResonanceAmplitude,
    decimal SharedSurfacePressure,
    bool LocallyLawful,
    bool ReviewOnly,
    bool Inert,
    bool RequestsSharedSurface,
    bool EmitsPacket,
    bool RequestsRuntimeAction,
    bool ClaimsAuthority,
    bool ClaimsContinuity,
    bool RequestsActivation)
{
    public bool IsColdLawfulSignal =>
        !string.IsNullOrWhiteSpace(SignalHandle) &&
        !string.IsNullOrWhiteSpace(SourceReceiptHandle) &&
        !string.IsNullOrWhiteSpace(ThreadHandle) &&
        !string.IsNullOrWhiteSpace(SharedSurfaceHandle) &&
        CadenceOrdinal >= 0m &&
        ResonanceAmplitude >= 0m &&
        SharedSurfacePressure >= 0m &&
        LocallyLawful &&
        ReviewOnly &&
        Inert &&
        RequestsSharedSurface &&
        !EmitsPacket &&
        !RequestsRuntimeAction &&
        !ClaimsAuthority &&
        !ClaimsContinuity &&
        !RequestsActivation;
}

public sealed record SharedSymbolicSurface(
    string SurfaceHandle,
    string SurfaceName,
    string CustodyOwner,
    bool Shared,
    bool WitnessSurfacePresent,
    bool StewardInterlockRequired,
    bool DirectWriteAdmissionAllowed,
    bool GrantsAuthority,
    bool AdmitsContinuity,
    bool AllowsRuntimeAction,
    bool AllowsActivation)
{
    public bool IsColdSharedSurface =>
        !string.IsNullOrWhiteSpace(SurfaceHandle) &&
        !string.IsNullOrWhiteSpace(SurfaceName) &&
        string.Equals(CustodyOwner, "Steward", StringComparison.Ordinal) &&
        Shared &&
        WitnessSurfacePresent &&
        StewardInterlockRequired &&
        !DirectWriteAdmissionAllowed &&
        !GrantsAuthority &&
        !AdmitsContinuity &&
        !AllowsRuntimeAction &&
        !AllowsActivation;
}

public sealed record StewardInterlockHeartbeatWindow(
    string WindowHandle,
    int StartOrdinal,
    int EndOrdinal,
    bool StewardGoverned,
    bool Bounded,
    bool AllowsUngovernedCoexistence,
    bool AllowsBypass,
    bool AllowsPassageIncrement)
{
    public bool IsColdHeartbeatWindow =>
        !string.IsNullOrWhiteSpace(WindowHandle) &&
        StartOrdinal >= 0 &&
        EndOrdinal >= StartOrdinal &&
        StewardGoverned &&
        Bounded &&
        !AllowsUngovernedCoexistence &&
        !AllowsBypass &&
        !AllowsPassageIncrement;
}

public sealed record CadenceAlignmentPolicy(
    string PolicyCode,
    bool Present,
    bool CompatibleCadenceRequired,
    bool AllowsAlignmentToAdmit,
    bool AllowsAlignmentToAuthorize,
    bool AllowsUnwitnessedCoexistence)
{
    public bool IsColdCadencePolicy =>
        !string.IsNullOrWhiteSpace(PolicyCode) &&
        Present &&
        CompatibleCadenceRequired &&
        !AllowsAlignmentToAdmit &&
        !AllowsAlignmentToAuthorize &&
        !AllowsUnwitnessedCoexistence;
}

public sealed record DampingBackoffPolicy(
    string PolicyCode,
    bool Present,
    decimal DampingCoefficient,
    bool DampsWithoutErasure,
    bool AllowsWitnessErasure,
    bool AllowsAuthority,
    bool AllowsContinuity)
{
    public bool IsColdDampingPolicy =>
        !string.IsNullOrWhiteSpace(PolicyCode) &&
        Present &&
        DampingCoefficient >= 0m &&
        DampingCoefficient <= 1m &&
        DampsWithoutErasure &&
        !AllowsWitnessErasure &&
        !AllowsAuthority &&
        !AllowsContinuity;
}

public sealed record WitnessSurfaceSplitRoute(
    string RouteCode,
    bool Present,
    bool PreservesCustody,
    bool PreservesOriginalSignalHandles,
    bool CreatesNewAuthoritySurface,
    bool FragmentsCustody,
    bool EmitsPackets)
{
    public bool IsColdSplitRoute =>
        !string.IsNullOrWhiteSpace(RouteCode) &&
        Present &&
        PreservesCustody &&
        PreservesOriginalSignalHandles &&
        !CreatesNewAuthoritySurface &&
        !FragmentsCustody &&
        !EmitsPackets;
}

public sealed record StewardInterlockNonAuthorityBoundary(
    string BoundaryCode,
    bool LocalLawfulnessMayImplySharedComposability,
    bool InterlockMayAuthorize,
    bool AlignmentMayAdmit,
    bool SequenceMayPunish,
    bool DampingMayEraseWitness,
    bool SplitMayFragmentCustody,
    bool CoolingMayMeanFailure,
    bool ContentionMayActivate,
    bool ReceiptMayPermit,
    bool StewardMayOwnMeaning,
    bool AllowsLispEvaluation,
    bool AllowsRuntimeAction,
    bool AllowsPacketEmission,
    bool AllowsReceiptReplay,
    bool AllowsPassageIncrement,
    bool AllowsContinuity,
    bool AllowsAuthority)
{
    public bool IsColdNonAuthorityBoundary =>
        !string.IsNullOrWhiteSpace(BoundaryCode) &&
        !LocalLawfulnessMayImplySharedComposability &&
        !InterlockMayAuthorize &&
        !AlignmentMayAdmit &&
        !SequenceMayPunish &&
        !DampingMayEraseWitness &&
        !SplitMayFragmentCustody &&
        !CoolingMayMeanFailure &&
        !ContentionMayActivate &&
        !ReceiptMayPermit &&
        !StewardMayOwnMeaning &&
        !AllowsLispEvaluation &&
        !AllowsRuntimeAction &&
        !AllowsPacketEmission &&
        !AllowsReceiptReplay &&
        !AllowsPassageIncrement &&
        !AllowsContinuity &&
        !AllowsAuthority;
}

public sealed record SharedSurfaceContentionReceipt(
    string ReceiptHandle,
    string SurfaceHandle,
    IReadOnlyList<string> SignalHandles,
    HarmonicInterlockOutcome Outcome,
    bool Retained,
    bool ReviewOnly,
    bool EvidenceOnly,
    bool GrantsPermission,
    bool BecomesAuthority,
    bool AdmitsContinuity,
    bool ActivatesRuntime)
{
    public bool IsColdContentionReceipt =>
        !string.IsNullOrWhiteSpace(ReceiptHandle) &&
        !string.IsNullOrWhiteSpace(SurfaceHandle) &&
        SignalHandles.Count >= 2 &&
        Retained &&
        ReviewOnly &&
        EvidenceOnly &&
        !GrantsPermission &&
        !BecomesAuthority &&
        !AdmitsContinuity &&
        !ActivatesRuntime;
}

public sealed record StewardHarmonicCustodyInterlockRefusalReceipt(
    string ReceiptHandle,
    string RefusalCode,
    string GovernanceTrace,
    bool Retained);

public sealed record StewardHarmonicCustodyInterlockRequest(
    CmeLispResonanceHeartbeatReceipt? SourceResonanceReceipt,
    IReadOnlyList<LawfulSignalCandidate> Signals,
    SharedSymbolicSurface SharedSurface,
    StewardInterlockHeartbeatWindow HeartbeatWindow,
    HarmonicInterlockOutcome RequestedOutcome,
    CadenceAlignmentPolicy CadencePolicy,
    DampingBackoffPolicy DampingPolicy,
    WitnessSurfaceSplitRoute SplitRoute,
    StewardInterlockNonAuthorityBoundary Boundary,
    int PriorPassageCount);

public sealed record StewardHarmonicCustodyInterlockReceipt(
    string ReceiptHandle,
    StewardHarmonicCustodyInterlockDisposition Disposition,
    string OutcomeCode,
    string GovernanceTrace,
    string SourceResonanceReceiptHandle,
    IReadOnlyList<LawfulSignalCandidate> Signals,
    SharedSymbolicSurface SharedSurface,
    StewardInterlockHeartbeatWindow HeartbeatWindow,
    HarmonicInterlockOutcome Outcome,
    CadenceAlignmentPolicy CadencePolicy,
    DampingBackoffPolicy DampingPolicy,
    WitnessSurfaceSplitRoute SplitRoute,
    StewardInterlockNonAuthorityBoundary Boundary,
    SharedSurfaceContentionReceipt? ContentionReceipt,
    StewardHarmonicCustodyInterlockRefusalReceipt? Refusal,
    int PriorPassageCount,
    int PassageCountAfterInterlockReview,
    bool ReviewOnly,
    bool InertOnly,
    bool StewardInterlockPresent,
    bool LocalLawfulnessBecomesSharedComposability,
    bool InterlockGrantsAuthority,
    bool AlignmentAdmits,
    bool SequencePunishes,
    bool DampingErasesWitness,
    bool SplitFragmentsCustody,
    bool CoolingMeansFailure,
    bool ContentionActivates,
    bool ReceiptBecomesPermission,
    bool StewardOwnsMeaning,
    bool NewPacketEmitted,
    bool RuntimeActionAllowed,
    bool LispEvaluationAllowed,
    bool ContinuityAdmitted,
    bool ActivationRefused,
    DateTimeOffset TimestampUtc)
{
    public bool IsColdInterlock =>
        (Disposition is StewardHarmonicCustodyInterlockDisposition.AlignReviewCold or
            StewardHarmonicCustodyInterlockDisposition.SequenceReviewCold or
            StewardHarmonicCustodyInterlockDisposition.DampReviewCold or
            StewardHarmonicCustodyInterlockDisposition.SplitReviewCold or
            StewardHarmonicCustodyInterlockDisposition.CoolReviewCold or
            StewardHarmonicCustodyInterlockDisposition.RefusalReviewCold) &&
        Refusal is null &&
        ContentionReceipt?.IsColdContentionReceipt == true &&
        ReviewOnly &&
        InertOnly &&
        StewardInterlockPresent &&
        !LocalLawfulnessBecomesSharedComposability &&
        !InterlockGrantsAuthority &&
        !AlignmentAdmits &&
        !SequencePunishes &&
        !DampingErasesWitness &&
        !SplitFragmentsCustody &&
        !CoolingMeansFailure &&
        !ContentionActivates &&
        !ReceiptBecomesPermission &&
        !StewardOwnsMeaning &&
        !NewPacketEmitted &&
        !RuntimeActionAllowed &&
        !LispEvaluationAllowed &&
        !ContinuityAdmitted &&
        ActivationRefused &&
        PassageCountAfterInterlockReview == PriorPassageCount;

    public bool IsRetainedInterlockRefusal =>
        Disposition == StewardHarmonicCustodyInterlockDisposition.Refused &&
        Refusal?.Retained == true &&
        PassageCountAfterInterlockReview == PriorPassageCount &&
        !InterlockGrantsAuthority &&
        !ContinuityAdmitted &&
        !NewPacketEmitted &&
        !RuntimeActionAllowed &&
        !LispEvaluationAllowed &&
        ActivationRefused;
}

public sealed class DefaultStewardHarmonicCustodyInterlockBoundaryValidator
{
    public StewardHarmonicCustodyInterlockReceipt Declare(
        StewardHarmonicCustodyInterlockRequest request,
        DateTimeOffset timestampUtc)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.SourceResonanceReceipt is null ||
            !request.SourceResonanceReceipt.IsColdResonanceHeartbeat)
        {
            return Refuse(
                request,
                "steward-interlock-source-resonance-heartbeat-missing",
                "Steward harmonic interlock refused because a cold Listening Frame resonance heartbeat receipt is required.",
                timestampUtc);
        }

        if (request.Signals.Count < 2)
        {
            return Refuse(
                request,
                "steward-interlock-requires-multiple-lawful-signals",
                "Steward harmonic interlock refused because shared-surface coexistence requires at least two lawful signals under review.",
                timestampUtc);
        }

        if (request.Signals.Any(static signal => !signal.IsColdLawfulSignal))
        {
            return Refuse(
                request,
                "steward-interlock-signal-promotional-refused",
                "Lawful signal refused because signal candidates must remain review-only, inert, locally lawful, non-authorizing, non-continuity, non-packet, and non-activating.",
                timestampUtc);
        }

        if (request.Signals.Any(signal =>
            !string.Equals(signal.SourceReceiptHandle, request.SourceResonanceReceipt.ReceiptHandle, StringComparison.Ordinal) ||
            !string.Equals(signal.SharedSurfaceHandle, request.SharedSurface.SurfaceHandle, StringComparison.Ordinal)))
        {
            return Refuse(
                request,
                "steward-interlock-signal-lineage-or-surface-mismatch",
                "Lawful signal refused because every signal must preserve the source resonance receipt handle and target the declared shared surface.",
                timestampUtc);
        }

        if (!request.SharedSurface.IsColdSharedSurface)
        {
            return Refuse(
                request,
                "steward-interlock-shared-surface-not-steward-governed",
                "Shared surface refused because coexistence requires Steward custody, witness surface, and no direct write admission, authority, continuity, action, or activation.",
                timestampUtc);
        }

        if (!request.HeartbeatWindow.IsColdHeartbeatWindow)
        {
            return Refuse(
                request,
                "steward-interlock-heartbeat-window-not-steward-governed",
                "Steward interlock refused because harmonic coexistence requires a bounded Steward-governed heartbeat window without bypass or passage increment.",
                timestampUtc);
        }

        if (!request.Boundary.IsColdNonAuthorityBoundary)
        {
            return Refuse(
                request,
                "steward-interlock-non-authority-boundary-promotional",
                "Steward interlock refused because interlock, alignment, sequence, damping, split, cooling, contention, receipt, or Steward meaning attempted promotion.",
                timestampUtc);
        }

        var policyRefusal = ValidateOutcomePolicy(request);
        if (policyRefusal is not null)
        {
            return Refuse(
                request,
                policyRefusal.Value.OutcomeCode,
                policyRefusal.Value.GovernanceTrace,
                timestampUtc);
        }

        var disposition = request.RequestedOutcome switch
        {
            HarmonicInterlockOutcome.Align => StewardHarmonicCustodyInterlockDisposition.AlignReviewCold,
            HarmonicInterlockOutcome.Sequence => StewardHarmonicCustodyInterlockDisposition.SequenceReviewCold,
            HarmonicInterlockOutcome.Damp => StewardHarmonicCustodyInterlockDisposition.DampReviewCold,
            HarmonicInterlockOutcome.Split => StewardHarmonicCustodyInterlockDisposition.SplitReviewCold,
            HarmonicInterlockOutcome.Cool => StewardHarmonicCustodyInterlockDisposition.CoolReviewCold,
            _ => StewardHarmonicCustodyInterlockDisposition.RefusalReviewCold
        };
        var outcomeCode = request.RequestedOutcome switch
        {
            HarmonicInterlockOutcome.Align => "steward-harmonic-interlock-align-review-only",
            HarmonicInterlockOutcome.Sequence => "steward-harmonic-interlock-sequence-review-only",
            HarmonicInterlockOutcome.Damp => "steward-harmonic-interlock-damp-review-only",
            HarmonicInterlockOutcome.Split => "steward-harmonic-interlock-split-review-only",
            HarmonicInterlockOutcome.Cool => "steward-harmonic-interlock-cool-review-only",
            _ => "steward-harmonic-interlock-contention-refused-review-only"
        };
        var signalHandles = request.Signals
            .Select(static signal => signal.SignalHandle)
            .ToArray();

        return new StewardHarmonicCustodyInterlockReceipt(
            ReceiptHandle: $"urn:san:steward-harmonic-interlock:review:{ShortHash(request.SourceResonanceReceipt.ReceiptHandle, request.SharedSurface.SurfaceHandle, outcomeCode, timestampUtc.UtcTicks.ToString(System.Globalization.CultureInfo.InvariantCulture))}",
            Disposition: disposition,
            OutcomeCode: outcomeCode,
            GovernanceTrace: CreateGovernanceTrace(request.RequestedOutcome),
            SourceResonanceReceiptHandle: request.SourceResonanceReceipt.ReceiptHandle,
            Signals: request.Signals.ToArray(),
            SharedSurface: request.SharedSurface,
            HeartbeatWindow: request.HeartbeatWindow,
            Outcome: request.RequestedOutcome,
            CadencePolicy: request.CadencePolicy,
            DampingPolicy: request.DampingPolicy,
            SplitRoute: request.SplitRoute,
            Boundary: request.Boundary,
            ContentionReceipt: new SharedSurfaceContentionReceipt(
                ReceiptHandle: $"urn:san:shared-surface-contention:{ShortHash(request.SharedSurface.SurfaceHandle, string.Join(",", signalHandles), outcomeCode)}",
                SurfaceHandle: request.SharedSurface.SurfaceHandle,
                SignalHandles: signalHandles,
                Outcome: request.RequestedOutcome,
                Retained: true,
                ReviewOnly: true,
                EvidenceOnly: true,
                GrantsPermission: false,
                BecomesAuthority: false,
                AdmitsContinuity: false,
                ActivatesRuntime: false),
            Refusal: null,
            PriorPassageCount: request.PriorPassageCount,
            PassageCountAfterInterlockReview: request.PriorPassageCount,
            ReviewOnly: true,
            InertOnly: true,
            StewardInterlockPresent: true,
            LocalLawfulnessBecomesSharedComposability: false,
            InterlockGrantsAuthority: false,
            AlignmentAdmits: false,
            SequencePunishes: false,
            DampingErasesWitness: false,
            SplitFragmentsCustody: false,
            CoolingMeansFailure: false,
            ContentionActivates: false,
            ReceiptBecomesPermission: false,
            StewardOwnsMeaning: false,
            NewPacketEmitted: false,
            RuntimeActionAllowed: false,
            LispEvaluationAllowed: false,
            ContinuityAdmitted: false,
            ActivationRefused: true,
            TimestampUtc: timestampUtc);
    }

    private static (string OutcomeCode, string GovernanceTrace)? ValidateOutcomePolicy(
        StewardHarmonicCustodyInterlockRequest request) =>
        request.RequestedOutcome switch
        {
            HarmonicInterlockOutcome.Align or HarmonicInterlockOutcome.Sequence
                when !request.CadencePolicy.IsColdCadencePolicy =>
                ("steward-interlock-cadence-policy-missing",
                    "Steward interlock refused because align and sequence require a cold cadence policy that cannot admit, authorize, or allow unwitnessed coexistence."),
            HarmonicInterlockOutcome.Damp or HarmonicInterlockOutcome.Cool
                when !request.DampingPolicy.IsColdDampingPolicy =>
                ("steward-interlock-damping-policy-missing",
                    "Steward interlock refused because damp and cool require a cold damping policy that cannot erase witness, authorize, or admit continuity."),
            HarmonicInterlockOutcome.Split
                when !request.SplitRoute.IsColdSplitRoute =>
                ("steward-interlock-split-route-missing",
                    "Steward interlock refused because split requires a witness surface split route that preserves custody and original signal handles."),
            _ => null
        };

    private static string CreateGovernanceTrace(HarmonicInterlockOutcome outcome) =>
        outcome switch
        {
            HarmonicInterlockOutcome.Align =>
                "Steward aligned locally lawful signals for shared-surface review. Alignment grants no authority, admission, action, continuity, packet emission, Lisp evaluation, passage, or activation.",
            HarmonicInterlockOutcome.Sequence =>
                "Steward sequenced locally lawful signals for shared-surface review. Sequence is cadence custody, not punishment, admission, authority, action, or continuity.",
            HarmonicInterlockOutcome.Damp =>
                "Steward damped shared-surface contention for review. Damping reduces pressure without erasing witness or granting authority, action, continuity, passage, or activation.",
            HarmonicInterlockOutcome.Split =>
                "Steward split witness surfaces for review. Split preserves custody and original signal handles without fragmenting authority or emitting packets.",
            HarmonicInterlockOutcome.Cool =>
                "Steward cooled shared-surface contention for review. Cooling is not failure and grants no action, authority, continuity, passage, or activation.",
            _ =>
                "Steward refused shared-surface coexistence while retaining contention evidence. Refusal preserves witness without becoming permission, authority, continuity, action, passage, or activation."
        };

    private static StewardHarmonicCustodyInterlockReceipt Refuse(
        StewardHarmonicCustodyInterlockRequest request,
        string outcomeCode,
        string governanceTrace,
        DateTimeOffset timestampUtc)
    {
        var sourceHandle = request.SourceResonanceReceipt?.ReceiptHandle ?? "missing-resonance-heartbeat-source";
        return new StewardHarmonicCustodyInterlockReceipt(
            ReceiptHandle: $"urn:san:steward-harmonic-interlock:refused:{ShortHash(sourceHandle, outcomeCode, timestampUtc.UtcTicks.ToString(System.Globalization.CultureInfo.InvariantCulture))}",
            Disposition: StewardHarmonicCustodyInterlockDisposition.Refused,
            OutcomeCode: outcomeCode,
            GovernanceTrace: governanceTrace,
            SourceResonanceReceiptHandle: sourceHandle,
            Signals: [],
            SharedSurface: request.SharedSurface,
            HeartbeatWindow: request.HeartbeatWindow,
            Outcome: request.RequestedOutcome,
            CadencePolicy: request.CadencePolicy,
            DampingPolicy: request.DampingPolicy,
            SplitRoute: request.SplitRoute,
            Boundary: request.Boundary,
            ContentionReceipt: null,
            Refusal: new StewardHarmonicCustodyInterlockRefusalReceipt(
                ReceiptHandle: $"urn:san:steward-harmonic-interlock-refusal:{ShortHash(sourceHandle, outcomeCode)}",
                RefusalCode: outcomeCode,
                GovernanceTrace: governanceTrace,
                Retained: true),
            PriorPassageCount: request.PriorPassageCount,
            PassageCountAfterInterlockReview: request.PriorPassageCount,
            ReviewOnly: true,
            InertOnly: true,
            StewardInterlockPresent: false,
            LocalLawfulnessBecomesSharedComposability: false,
            InterlockGrantsAuthority: false,
            AlignmentAdmits: false,
            SequencePunishes: false,
            DampingErasesWitness: false,
            SplitFragmentsCustody: false,
            CoolingMeansFailure: false,
            ContentionActivates: false,
            ReceiptBecomesPermission: false,
            StewardOwnsMeaning: false,
            NewPacketEmitted: false,
            RuntimeActionAllowed: false,
            LispEvaluationAllowed: false,
            ContinuityAdmitted: false,
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
