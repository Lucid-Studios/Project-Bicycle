using San.Common;
using Xunit;

namespace San.Audit.Tests;

public sealed class InstallFacingPredicatePostureCorrespondenceReferenceDataTests
{
    [Fact]
    public void Canonical_Vocabulary_Covers_All_First_Families_And_Current_Kinds()
    {
        var set = InstallFacingPredicatePostureCorrespondenceReferenceData.Canonical;

        Assert.Contains(set.Correspondences, entry => entry.PredicateCandidateKind == SanctuaryGelPredicateCandidateKind.InstallFacing);
        Assert.Contains(set.Correspondences, entry => entry.PredicateCandidateKind == SanctuaryGelPredicateCandidateKind.ConversationalMovement);
        Assert.Contains(set.Correspondences, entry => entry.PredicateCandidateKind == SanctuaryGelPredicateCandidateKind.GoverningSeatCandidate);
        Assert.Contains(set.Correspondences, entry => entry.PredicateCandidateKind == SanctuaryGelPredicateCandidateKind.ResearchAttached);
        Assert.Contains(set.Correspondences, entry => entry.PredicateCandidateKind == SanctuaryGelPredicateCandidateKind.CertifiedCommunication);
        Assert.Contains(set.Correspondences, entry => entry.PredicateCandidateKind == SanctuaryGelPredicateCandidateKind.RegionalPackageAdmitted);
        Assert.Contains(set.Correspondences, entry => entry.PredicateCandidateKind == SanctuaryGelPredicateCandidateKind.UniversalAtlasAuthorityWithheld);
        Assert.Contains(set.Correspondences, entry => entry.PredicateCandidateKind == SanctuaryGelPredicateCandidateKind.AssentWitnessed);
        Assert.Contains(set.Correspondences, entry => entry.PredicateCandidateKind == SanctuaryGelPredicateCandidateKind.PackageWitnessed);
        Assert.Contains(set.Correspondences, entry => entry.PredicateCandidateKind == SanctuaryGelPredicateCandidateKind.PredicateInheritanceWitnessed);
        Assert.Contains(set.Correspondences, entry => entry.PredicateCandidateKind == SanctuaryGelPredicateCandidateKind.Ready);
        Assert.Contains(set.Correspondences, entry => entry.PredicateCandidateKind == SanctuaryGelPredicateCandidateKind.Silence);
        Assert.Contains(set.Correspondences, entry => entry.PredicateCandidateKind == SanctuaryGelPredicateCandidateKind.Refused);
    }

    [Fact]
    public void Canonical_Vocabulary_Remains_Reference_Only()
    {
        var set = InstallFacingPredicatePostureCorrespondenceReferenceData.Canonical;

        Assert.All(
            set.Correspondences,
            entry =>
            {
                Assert.False(string.IsNullOrWhiteSpace(entry.InstallFacingPhrase));
                Assert.False(string.IsNullOrWhiteSpace(entry.InstallFacingSummary));
            });
    }
}
