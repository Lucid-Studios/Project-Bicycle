namespace San.Common;

public enum SanctuaryGelPredicateFamily
{
    Posture = 0,
    TrustAuthorization = 1,
    EvidenceFooting = 2,
    ResponseDisposition = 3
}

public enum SanctuaryGelPredicateCandidateKind
{
    InstallFacing = 0,
    ConversationalMovement = 1,
    GoverningSeatCandidate = 2,
    ResearchAttached = 3,
    CertifiedCommunication = 4,
    RegionalPackageAdmitted = 5,
    UniversalAtlasAuthorityWithheld = 6,
    AssentWitnessed = 7,
    PackageWitnessed = 8,
    PredicateInheritanceWitnessed = 9,
    Ready = 10,
    Silence = 11,
    Refused = 12
}

public sealed record SanctuaryGelPredicateFamilySet(
    SanctuaryGelPredicateFamily Family,
    IReadOnlyList<SanctuaryGelPredicateCandidateKind> CandidateKinds);
