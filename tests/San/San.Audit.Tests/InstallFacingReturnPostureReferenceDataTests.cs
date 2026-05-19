using San.Common;
using Xunit;

namespace San.Audit.Tests;

public sealed class InstallFacingReturnPostureReferenceDataTests
{
    [Fact]
    public void Canonical_Return_Posture_Includes_Retained_Deferred_And_Horizon()
    {
        Assert.Equal(InstallFacingReturnPostureDisposition.Retained, InstallFacingReturnPostureReferenceData.ReadyAcknowledgedRetained.ReturnDisposition);
        Assert.Equal(InstallFacingReturnPostureDisposition.Deferred, InstallFacingReturnPostureReferenceData.ReadyHeldDeferred.ReturnDisposition);
        Assert.Equal(InstallFacingReturnPostureDisposition.WitnessedForwardHorizon, InstallFacingReturnPostureReferenceData.ReadyHeldForwardHorizon.ReturnDisposition);
    }

    [Fact]
    public void Silence_Remains_Deferred_Without_Extra_Teaching_Surface()
    {
        var record = InstallFacingReturnPostureReferenceData.SilenceHeldDeferred;

        Assert.Equal(InstallFacingReturnPostureDisposition.Deferred, record.ReturnDisposition);
        Assert.Contains("without gaining extra teaching surface", record.Summary, StringComparison.Ordinal);
    }

    [Fact]
    public void Closed_Refusal_Does_Not_Reopen_Readiness_Or_Authorization()
    {
        var record = InstallFacingReturnPostureReferenceData.RefusedClosed;

        Assert.Equal(InstallFacingReturnPostureDisposition.ClosedRefusal, record.ReturnDisposition);
        Assert.Contains("does not reopen readiness or authorization", record.Summary, StringComparison.Ordinal);
    }
}
