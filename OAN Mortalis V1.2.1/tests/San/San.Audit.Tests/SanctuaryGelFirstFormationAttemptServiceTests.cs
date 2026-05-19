using San.Common;
using San.Nexus.Control;
using Xunit;

namespace San.Audit.Tests;

public sealed class SanctuaryGelFirstFormationAttemptServiceTests
{
    private readonly DefaultSanctuaryGelFirstFormationAttemptService _service = new();

    [Fact]
    public void EvaluateFormationAttempt_Returns_Ready_When_All_Prerequisites_Are_Ready()
    {
        var result = _service.EvaluateFormationAttempt(SanctuaryGelFirstFormationAttemptReferenceData.ReadyInput);

        Assert.Equal(SanctuaryGelFirstFormationAttemptDisposition.Ready, result.Disposition);
        Assert.Equal("sanctuary-gel-first-formation-attempt-ready", result.OutcomeCode);
        Assert.Equal(SanctuaryGelFirstFormationAttemptDisposition.Ready, result.AttemptRecord.Disposition);
        Assert.Contains("gel-predicate-prior-ref://rt-accept-107", result.AttemptRecord.PredicatePriorRefs);
        Assert.Equal("sanctuary-gel-regional-substrate-ref://ready", result.AttemptRecord.RegionalSubstrateRef);
        Assert.Contains("does not stand Sanctuary.Actual", result.Summary, StringComparison.Ordinal);
        Assert.NotEmpty(result.Receipt.ReceiptHandle);
    }

    [Fact]
    public void EvaluateFormationAttempt_Returns_Held_When_Any_Upstream_Posture_Is_Held()
    {
        var input = SanctuaryGelFirstFormationAttemptReferenceData.ReadyInput with
        {
            RegionalSubstrate = SanctuaryGelRegionalSubstrateFormationReferenceData.HeldForRegionalOrGovernanceReview,
            FirstUseEligibility = FirstUseEligibilityReferenceData.HeldForReview
        };

        var result = _service.EvaluateFormationAttempt(input);

        Assert.Equal(SanctuaryGelFirstFormationAttemptDisposition.Held, result.Disposition);
        Assert.Equal("sanctuary-gel-first-formation-attempt-held", result.OutcomeCode);
        Assert.Contains("questions remain held", result.Summary, StringComparison.Ordinal);
    }

    [Fact]
    public void EvaluateFormationAttempt_Refuses_When_Predicate_Prior_Is_Missing()
    {
        var input = SanctuaryGelFirstFormationAttemptReferenceData.ReadyInput with
        {
            PredicatePriors = Array.Empty<GelPredicatePriorFormalizationRecord>()
        };

        var result = _service.EvaluateFormationAttempt(input);

        Assert.Equal(SanctuaryGelFirstFormationAttemptDisposition.Refused, result.Disposition);
        Assert.Equal("sanctuary-gel-first-formation-attempt-refused", result.OutcomeCode);
        Assert.Contains(SanctuaryGelFirstFormationAttemptRefusalReason.MissingPredicatePriors, result.AttemptRecord.RefusalReasons);
    }

    [Fact]
    public void EvaluateFormationAttempt_Refuses_When_Standing_Or_Regional_Substrate_Is_Missing()
    {
        var input = SanctuaryGelFirstFormationAttemptReferenceData.ReadyInput with
        {
            LocalizedFormation = LocalizedSanctuaryGelFormationReferenceData.RefusedMissingNationalStanding,
            RegionalSubstrate = SanctuaryGelRegionalSubstrateFormationReferenceData.RefusedMissingStandingOrRegionalPackageFooting
        };

        var result = _service.EvaluateFormationAttempt(input);

        Assert.Equal(SanctuaryGelFirstFormationAttemptDisposition.Refused, result.Disposition);
        Assert.Contains(SanctuaryGelFirstFormationAttemptRefusalReason.MissingLocalizedFormationFloor, result.AttemptRecord.RefusalReasons);
        Assert.Contains(SanctuaryGelFirstFormationAttemptRefusalReason.MissingNationalStanding, result.AttemptRecord.RefusalReasons);
        Assert.Contains(SanctuaryGelFirstFormationAttemptRefusalReason.MissingRegionalStanding, result.AttemptRecord.RefusalReasons);
        Assert.Contains(SanctuaryGelFirstFormationAttemptRefusalReason.MissingLocalStanding, result.AttemptRecord.RefusalReasons);
        Assert.Contains(SanctuaryGelFirstFormationAttemptRefusalReason.MissingRegionalSubstrateFooting, result.AttemptRecord.RefusalReasons);
    }

    [Fact]
    public void EvaluateFormationAttempt_Refuses_All_Authority_Overclaims()
    {
        var input = SanctuaryGelFirstFormationAttemptReferenceData.ReadyInput with
        {
            SanctuaryActualClaimed = true,
            SurvivorAdmissionClaimed = true,
            FirstUseAdmissionClaimed = true,
            ModelSelectionClaimed = true,
            RuntimeAuthorityClaimed = true,
            CradleGelGenerationClaimed = true,
            SliLispOrRtmeActivationClaimed = true
        };

        var result = _service.EvaluateFormationAttempt(input);

        Assert.Equal(SanctuaryGelFirstFormationAttemptDisposition.Refused, result.Disposition);
        Assert.Contains(SanctuaryGelFirstFormationAttemptRefusalReason.SanctuaryActualOverclaimed, result.AttemptRecord.RefusalReasons);
        Assert.Contains(SanctuaryGelFirstFormationAttemptRefusalReason.SurvivorAdmissionOverclaimed, result.AttemptRecord.RefusalReasons);
        Assert.Contains(SanctuaryGelFirstFormationAttemptRefusalReason.FirstUseAdmissionOverclaimed, result.AttemptRecord.RefusalReasons);
        Assert.Contains(SanctuaryGelFirstFormationAttemptRefusalReason.ModelSelectionOverclaimed, result.AttemptRecord.RefusalReasons);
        Assert.Contains(SanctuaryGelFirstFormationAttemptRefusalReason.RuntimeAuthorityOverclaimed, result.AttemptRecord.RefusalReasons);
        Assert.Contains(SanctuaryGelFirstFormationAttemptRefusalReason.CradleGelGenerationOverclaimed, result.AttemptRecord.RefusalReasons);
        Assert.Contains(SanctuaryGelFirstFormationAttemptRefusalReason.SliLispOrRtmeActivationOverclaimed, result.AttemptRecord.RefusalReasons);
    }
}
