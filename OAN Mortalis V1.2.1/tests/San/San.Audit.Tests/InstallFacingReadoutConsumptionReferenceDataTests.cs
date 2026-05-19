using San.Common;
using Xunit;

namespace San.Audit.Tests;

public sealed class InstallFacingReadoutConsumptionReferenceDataTests
{
    [Fact]
    public void Canonical_Consumption_Includes_Ready_Acknowledged_And_Held()
    {
        Assert.Equal(InstallFacingReadoutBundleDisposition.Ready, InstallFacingReadoutConsumptionReferenceData.ReadyAcknowledged.BundleDisposition);
        Assert.Equal(InstallFacingReadoutConsumptionDisposition.Acknowledged, InstallFacingReadoutConsumptionReferenceData.ReadyAcknowledged.ConsumptionDisposition);
        Assert.Equal(InstallFacingReadoutBundleDisposition.Ready, InstallFacingReadoutConsumptionReferenceData.ReadyHeld.BundleDisposition);
        Assert.Equal(InstallFacingReadoutConsumptionDisposition.Held, InstallFacingReadoutConsumptionReferenceData.ReadyHeld.ConsumptionDisposition);
    }

    [Fact]
    public void Silence_Is_Consumed_Only_As_Held()
    {
        var record = InstallFacingReadoutConsumptionReferenceData.SilenceHeld;

        Assert.Equal(InstallFacingReadoutBundleDisposition.Silence, record.BundleDisposition);
        Assert.Equal(InstallFacingReadoutConsumptionDisposition.Held, record.ConsumptionDisposition);
        Assert.Contains("without gaining extra teaching surface", record.Summary, StringComparison.Ordinal);
    }

    [Fact]
    public void Refused_Does_Not_Upgrade_To_Ready_Or_Authorized_Posture()
    {
        var record = InstallFacingReadoutConsumptionReferenceData.RefusedReception;

        Assert.Equal(InstallFacingReadoutBundleDisposition.Refused, record.BundleDisposition);
        Assert.Equal(InstallFacingReadoutConsumptionDisposition.Refused, record.ConsumptionDisposition);
        Assert.Contains("does not upgrade", record.Summary, StringComparison.Ordinal);
    }
}
