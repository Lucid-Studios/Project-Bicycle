using San.Common;
using Xunit;

namespace San.Audit.Tests;

public sealed class InstallFacingReadoutBundleReferenceDataTests
{
    [Fact]
    public void Ready_Bundle_Covers_Posture_Trust_Evidence_And_Response()
    {
        var bundle = InstallFacingReadoutBundleReferenceData.Ready;

        Assert.Equal(InstallFacingReadoutBundleDisposition.Ready, bundle.Disposition);
        Assert.Contains(bundle.Sections, section => section.SectionKind == InstallFacingReadoutSectionKind.Posture);
        Assert.Contains(bundle.Sections, section => section.SectionKind == InstallFacingReadoutSectionKind.TrustAuthorization);
        Assert.Contains(bundle.Sections, section => section.SectionKind == InstallFacingReadoutSectionKind.EvidenceFooting);
        Assert.Contains(bundle.Sections, section => section.SectionKind == InstallFacingReadoutSectionKind.ResponseDisposition);
    }

    [Fact]
    public void Silence_Bundle_Carries_Bounded_Silent_Response_Only()
    {
        var bundle = InstallFacingReadoutBundleReferenceData.Silence;

        Assert.Equal(InstallFacingReadoutBundleDisposition.Silence, bundle.Disposition);
        Assert.Single(bundle.Sections);
        Assert.Equal(InstallFacingReadoutSectionKind.ResponseDisposition, bundle.Sections[0].SectionKind);
        Assert.Single(bundle.Sections[0].Entries);
        Assert.Equal(SanctuaryGelPredicateCandidateKind.Silence, bundle.Sections[0].Entries[0].PredicateCandidateKind);
        Assert.False(bundle.Sections[0].Entries[0].OperatorVisible);
    }

    [Fact]
    public void Refused_Bundle_Carries_Bounded_Refusal_Response_Only()
    {
        var bundle = InstallFacingReadoutBundleReferenceData.Refused;

        Assert.Equal(InstallFacingReadoutBundleDisposition.Refused, bundle.Disposition);
        Assert.Single(bundle.Sections);
        Assert.Equal(InstallFacingReadoutSectionKind.ResponseDisposition, bundle.Sections[0].SectionKind);
        Assert.Single(bundle.Sections[0].Entries);
        Assert.Equal(SanctuaryGelPredicateCandidateKind.Refused, bundle.Sections[0].Entries[0].PredicateCandidateKind);
    }
}
