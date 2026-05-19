using San.Common;
using Xunit;

namespace San.Audit.Tests;

public sealed class SanctuaryPreGoverningStandingReferenceDataTests
{
    [Fact]
    public void Canonical_Pre_Governing_Standing_Includes_Ready_Held_And_Refused()
    {
        Assert.Equal(SanctuaryPreGoverningStandingDisposition.Ready, SanctuaryPreGoverningStandingReferenceData.ReadyStanding.Disposition);
        Assert.Equal(SanctuaryPreGoverningStandingDisposition.Held, SanctuaryPreGoverningStandingReferenceData.HeldForSpecialCaseOrDomainReview.Disposition);
        Assert.Equal(SanctuaryPreGoverningStandingDisposition.Refused, SanctuaryPreGoverningStandingReferenceData.RefusedOverclaim.Disposition);
    }

    [Fact]
    public void Ready_Standing_Remains_Template_Resource_Only_And_Cme_Placement_Withheld()
    {
        var record = SanctuaryPreGoverningStandingReferenceData.ReadyStanding;

        Assert.Equal(SanctuaryPreGoverningDisclosurePosture.TemplateResourceOnly, record.DisclosurePosture);
        Assert.Equal(SanctuaryPreGoverningCmePosture.PlacementWithheld, record.CmePosture);
        Assert.Contains("non-governing", record.NonGrantSummary, StringComparison.Ordinal);
        Assert.Contains("install-is-not-research-consent", record.ResearchSeparationPosture, StringComparison.Ordinal);
    }

    [Fact]
    public void Refused_Standing_Rejects_Certification_Domain_Research_Cme_Rtme_And_Runtime_Overclaims()
    {
        var record = SanctuaryPreGoverningStandingReferenceData.RefusedOverclaim;

        Assert.Equal(SanctuaryPreGoverningCmePosture.RefusedOverclaim, record.CmePosture);
        Assert.Contains("certification", record.NonGrantSummary, StringComparison.Ordinal);
        Assert.Contains("domain authority", record.NonGrantSummary, StringComparison.Ordinal);
        Assert.Contains("research consent", record.NonGrantSummary, StringComparison.Ordinal);
        Assert.Contains("CME legal personhood", record.NonGrantSummary, StringComparison.Ordinal);
        Assert.Contains("RTME", record.NonGrantSummary, StringComparison.Ordinal);
        Assert.Contains("runtime authority", record.NonGrantSummary, StringComparison.Ordinal);
    }
}
