using San.Common;
using Xunit;

namespace San.Audit.Tests;

public sealed class InstallFacingApproachBoundaryReferenceDataTests
{
    [Fact]
    public void Canonical_Approach_Data_Maps_All_Return_Postures()
    {
        var sourcePostures = new[]
        {
            InstallFacingApproachBoundaryReferenceData.RetainedControlSurface.SourceReturnDisposition,
            InstallFacingApproachBoundaryReferenceData.DeferredHoldControlSurface.SourceReturnDisposition,
            InstallFacingApproachBoundaryReferenceData.ClosedRefusalControlSurface.SourceReturnDisposition,
            InstallFacingApproachBoundaryReferenceData.WitnessedForwardHorizonAnchors.SourceReturnDisposition
        };

        Assert.Contains(InstallFacingReturnPostureDisposition.Retained, sourcePostures);
        Assert.Contains(InstallFacingReturnPostureDisposition.Deferred, sourcePostures);
        Assert.Contains(InstallFacingReturnPostureDisposition.ClosedRefusal, sourcePostures);
        Assert.Contains(InstallFacingReturnPostureDisposition.WitnessedForwardHorizon, sourcePostures);
    }

    [Fact]
    public void Forward_Horizon_Is_Future_Telemetry_And_Template_Anchor_Only()
    {
        var record = InstallFacingApproachBoundaryReferenceData.WitnessedForwardHorizonAnchors;

        Assert.Contains(InstallFacingApproachEligibility.TelemetryAnchorEligible, record.ApproachEligibilities);
        Assert.Contains(InstallFacingApproachEligibility.PredicateTemplateAnchorEligible, record.ApproachEligibilities);
        Assert.Equal(InstallFacingFutureTelemetryAnchorClass.ForwardHorizonTelemetryPoint, record.FutureTelemetryAnchorClass);
        Assert.Equal(InstallFacingFuturePredicateTemplateAnchorClass.ForwardHorizonPredicateTemplate, record.FuturePredicateTemplateAnchorClass);
        Assert.Contains("not active SLI.Lisp telemetry, template generation, handoff, RTME approach, or pre-certification", record.Summary, StringComparison.Ordinal);
    }
}
