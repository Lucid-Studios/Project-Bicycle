using San.Common;
using Xunit;

namespace San.Audit.Tests;

public sealed class SanctuaryGelFirstFormationAttemptReferenceDataTests
{
    [Fact]
    public void Passive_Contracts_Expose_Attempt_Disposition_And_Refusal_Types()
    {
        Assert.Contains(SanctuaryGelFirstFormationAttemptDisposition.Ready, Enum.GetValues<SanctuaryGelFirstFormationAttemptDisposition>());
        Assert.Contains(SanctuaryGelFirstFormationAttemptDisposition.Held, Enum.GetValues<SanctuaryGelFirstFormationAttemptDisposition>());
        Assert.Contains(SanctuaryGelFirstFormationAttemptDisposition.Refused, Enum.GetValues<SanctuaryGelFirstFormationAttemptDisposition>());

        Assert.Contains(SanctuaryGelFirstFormationAttemptRefusalReason.MissingPredicatePriors, Enum.GetValues<SanctuaryGelFirstFormationAttemptRefusalReason>());
        Assert.Contains(SanctuaryGelFirstFormationAttemptRefusalReason.MissingLocalizedPreCertificationDataPool, Enum.GetValues<SanctuaryGelFirstFormationAttemptRefusalReason>());
        Assert.Contains(SanctuaryGelFirstFormationAttemptRefusalReason.MissingRegionalSubstrateFooting, Enum.GetValues<SanctuaryGelFirstFormationAttemptRefusalReason>());
        Assert.Contains(SanctuaryGelFirstFormationAttemptRefusalReason.SanctuaryActualOverclaimed, Enum.GetValues<SanctuaryGelFirstFormationAttemptRefusalReason>());
        Assert.Contains(SanctuaryGelFirstFormationAttemptRefusalReason.CradleGelGenerationOverclaimed, Enum.GetValues<SanctuaryGelFirstFormationAttemptRefusalReason>());
        Assert.Contains(SanctuaryGelFirstFormationAttemptRefusalReason.SliLispOrRtmeActivationOverclaimed, Enum.GetValues<SanctuaryGelFirstFormationAttemptRefusalReason>());
    }

    [Fact]
    public void Canonical_Reference_Data_Includes_Ready_Held_And_Refused_Attempts()
    {
        Assert.Equal(SanctuaryGelFirstFormationAttemptDisposition.Ready, SanctuaryGelFirstFormationAttemptReferenceData.ReadyAttempt.Disposition);
        Assert.Equal(SanctuaryGelFirstFormationAttemptDisposition.Held, SanctuaryGelFirstFormationAttemptReferenceData.HeldAttempt.Disposition);
        Assert.Contains(SanctuaryGelFirstFormationAttemptReferenceData.CanonicalRecords, record => record.Disposition == SanctuaryGelFirstFormationAttemptDisposition.Refused);
    }

    [Fact]
    public void Ready_Attempt_Carries_Prerequisite_Refs_And_Remains_Non_Authoritative()
    {
        var record = SanctuaryGelFirstFormationAttemptReferenceData.ReadyAttempt;

        Assert.Contains("gel-predicate-prior-ref://rt-accept-107", record.PredicatePriorRefs);
        Assert.Contains("localized-pre-certification-data-pool-ref://ready", record.LocalizedPreCertificationDataPoolRefs);
        Assert.Contains("localized-standing://national", record.StandingRefs);
        Assert.Equal("sanctuary-gel-regional-substrate-ref://ready", record.RegionalSubstrateRef);
        Assert.Contains("ready-for-consideration", record.FirstUseEligibilityRef, StringComparison.Ordinal);
        Assert.Contains("does not stand Sanctuary.Actual, admit survivor standing, grant first use, select models, activate runtime, invoke SLI.Lisp or RTME, or generate Cradle.GEL", record.NonAuthoritySummary, StringComparison.Ordinal);
    }

    [Fact]
    public void Refused_Attempts_Name_Missing_Prerequisites_And_Authority_Overclaims()
    {
        var missing = SanctuaryGelFirstFormationAttemptReferenceData.RefusedMissingPrerequisites;
        var overclaim = SanctuaryGelFirstFormationAttemptReferenceData.RefusedAuthorityOverclaim;

        Assert.Contains(SanctuaryGelFirstFormationAttemptRefusalReason.MissingPredicatePriors, missing.RefusalReasons);
        Assert.Contains(SanctuaryGelFirstFormationAttemptRefusalReason.MissingLocalizedPreCertificationDataPool, missing.RefusalReasons);
        Assert.Contains(SanctuaryGelFirstFormationAttemptRefusalReason.MissingRegionalSubstrateFooting, missing.RefusalReasons);
        Assert.Empty(missing.PredicatePriorRefs);
        Assert.Empty(missing.LocalizedPreCertificationDataPoolRefs);

        Assert.Contains(SanctuaryGelFirstFormationAttemptRefusalReason.SanctuaryActualOverclaimed, overclaim.RefusalReasons);
        Assert.Contains(SanctuaryGelFirstFormationAttemptRefusalReason.SurvivorAdmissionOverclaimed, overclaim.RefusalReasons);
        Assert.Contains(SanctuaryGelFirstFormationAttemptRefusalReason.FirstUseAdmissionOverclaimed, overclaim.RefusalReasons);
        Assert.Contains(SanctuaryGelFirstFormationAttemptRefusalReason.ModelSelectionOverclaimed, overclaim.RefusalReasons);
        Assert.Contains(SanctuaryGelFirstFormationAttemptRefusalReason.RuntimeAuthorityOverclaimed, overclaim.RefusalReasons);
        Assert.Contains(SanctuaryGelFirstFormationAttemptRefusalReason.CradleGelGenerationOverclaimed, overclaim.RefusalReasons);
        Assert.Contains(SanctuaryGelFirstFormationAttemptRefusalReason.SliLispOrRtmeActivationOverclaimed, overclaim.RefusalReasons);
    }
}
