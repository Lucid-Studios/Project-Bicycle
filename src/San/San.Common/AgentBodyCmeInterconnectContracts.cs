namespace San.Common;

public enum AgentBodyCmeInterconnectDisposition
{
    Withheld = 0,
    Refused = 1,
    VerifiedCold = 2
}

public enum AgentBodyReviewConduitKind
{
    CgoaInsulatedPrime = 0,
    TelemetryStringCryptic = 1
}

public enum CompassShellCandidateStatus
{
    CandidateOnly = 0,
    Withheld = 1,
    Refused = 2
}

public enum CleavingDiscernmentDisposition
{
    CandidateOnly = 0,
    Withheld = 1,
    Refused = 2
}

public sealed record PrimeCarrierSet(
    string SoulFrameCmeId,
    string OeCmeId,
    string SelfGelCmeId,
    string CgoaBundleCmeId,
    string SlmSeedCmeId);

public sealed record CrypticCarrierSet(
    string AgentiCoreCmeId,
    string COeCmeId,
    string CSelfGelCmeId,
    string LispMembraneCmeId,
    string TelemetryStringRef);

public sealed record StewardRegulationField(
    string CSharpHostBoundaryRef,
    string ZedDeltaRef,
    string SituationalAwarenessRef,
    string TelemetryStringRef,
    string SliLispInnerChamberRef);

public sealed record CgoaInsulatedPrimeConduit(
    string ConduitHandle,
    AgentBodyReviewConduitKind ConduitKind,
    string CgoaBundleRef,
    string StewardHolderRef,
    bool InsulatesPrimeActual,
    bool GrantsAuthority,
    bool GrantsIdentity);

public sealed record TelemetryStringCrypticConduit(
    string ConduitHandle,
    AgentBodyReviewConduitKind ConduitKind,
    string TelemetryStringRef,
    string StewardHolderRef,
    bool DirectCrypticActualReach,
    bool GrantsAuthority,
    bool SelfAuthorizes);

public sealed record CompassShellCandidate(
    string ShellHandle,
    string ListeningFrameRef,
    string CompassRef,
    IReadOnlyList<string> PredicatePressureRefs,
    CompassShellCandidateStatus Status,
    bool IsEngram,
    bool ContinuityAdmitted,
    bool AuthorityGranted);

public sealed record CleavingDiscernmentReceipt(
    string ReceiptHandle,
    string CompassShellHandle,
    CleavingDiscernmentDisposition Disposition,
    string OutcomeCode,
    IReadOnlyList<string> ReviewPathRefs,
    bool EcStartRequested,
    bool RuntimeActionRequested,
    bool ContinuityAdmitted,
    bool AuthorityGranted);

public sealed record AgentBodyCmeInterconnectReceipt(
    string ReceiptHandle,
    AgentBodyCmeInterconnectDisposition Disposition,
    string OutcomeCode,
    string GovernanceTrace,
    string AgentBodyCmeId,
    PrimeCarrierSet Prime,
    CrypticCarrierSet Cryptic,
    StewardRegulationField Steward,
    CgoaInsulatedPrimeConduit PrimeReviewConduit,
    TelemetryStringCrypticConduit CrypticReviewConduit,
    CompassShellCandidate CompassShell,
    CleavingDiscernmentReceipt CleavingDiscernment,
    string SliRoundtripResponseHandle,
    IReadOnlyList<string> ReceiptRefs,
    bool RuntimeIdentityEmitted,
    bool RuntimeActionExecuted,
    bool ModelBindingRequested,
    bool LispEvaluationRequested,
    bool GelPromotionAllowed,
    bool CmeActualActivated,
    bool SanctuaryActualActivated,
    DateTimeOffset TimestampUtc)
{
    public bool IsCold =>
        Disposition == AgentBodyCmeInterconnectDisposition.VerifiedCold &&
        PrimeReviewConduit is { InsulatesPrimeActual: true, GrantsAuthority: false, GrantsIdentity: false } &&
        CrypticReviewConduit is { DirectCrypticActualReach: true, GrantsAuthority: false, SelfAuthorizes: false } &&
        CompassShell is { Status: CompassShellCandidateStatus.CandidateOnly, IsEngram: false, ContinuityAdmitted: false, AuthorityGranted: false } &&
        CleavingDiscernment is
        {
            Disposition: CleavingDiscernmentDisposition.CandidateOnly,
            EcStartRequested: false,
            RuntimeActionRequested: false,
            ContinuityAdmitted: false,
            AuthorityGranted: false
        } &&
        !RuntimeIdentityEmitted &&
        !RuntimeActionExecuted &&
        !ModelBindingRequested &&
        !LispEvaluationRequested &&
        !GelPromotionAllowed &&
        !CmeActualActivated &&
        !SanctuaryActualActivated;
}
