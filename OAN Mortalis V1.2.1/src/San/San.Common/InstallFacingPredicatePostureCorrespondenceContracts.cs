namespace San.Common;

public enum InstallFacingPredicatePostureLane
{
    Posture = 0,
    TrustAuthorization = 1,
    EvidenceFooting = 2,
    ResponseDisposition = 3
}

public enum InstallFacingPayloadMeaningKind
{
    InstallFacingPostureMeaning = 0,
    TrustAuthorizationMeaning = 1,
    EvidenceFootingMeaning = 2,
    ResponseDispositionMeaning = 3
}

public sealed record InstallFacingPredicatePostureCorrespondence(
    InstallFacingPayloadMeaningKind PayloadMeaningKind,
    InstallFacingPredicatePostureLane Lane,
    SanctuaryGelPredicateFamily PredicateFamily,
    SanctuaryGelPredicateCandidateKind PredicateCandidateKind,
    string InstallFacingPhrase,
    string InstallFacingSummary,
    bool OperatorVisible,
    bool CertifiedLaneOnly,
    bool HdtSupportEligible);

public sealed record InstallFacingPredicatePostureCorrespondenceSet(
    IReadOnlyList<InstallFacingPredicatePostureCorrespondence> Correspondences);
