using San.Product.Preflight;
using Xunit;

namespace San.Audit.Tests;

public sealed class SpiralBuildAutomationServiceTests
{
    private static readonly DateTimeOffset TimestampUtc = DateTimeOffset.Parse("2026-05-12T00:00:00Z");

    [Fact]
    public void CreateReceipt_Selects_Next_Adjacent_Cell_From_Cold_Installed_Surface()
    {
        using var fixture = SpiralFixture.Create(includePreflight: true, includeTriptychProfile: true);

        var receipt = new DefaultSpiralBuildAutomationService().CreateReceipt(
            fixture.CreateRequest(),
            TimestampUtc);

        Assert.Equal(SpiralBuildAutomationDisposition.ReadyCold, receipt.Disposition);
        Assert.Equal("spiral-build-next-adjacent-cell-selected", receipt.OutcomeCode);
        Assert.True(receipt.IsColdAutomationReady);
        Assert.NotNull(receipt.NextCell);
        Assert.Equal("full-body.layer-map", receipt.NextCell!.CellId);
        Assert.Equal(SpiralBuildPhase.FullBodyPass, receipt.NextCell.Phase);
        Assert.Contains("full-body.install-anchor", receipt.NextCell.AdjacentTo);
        Assert.Contains("full-body.codex-proxy-triptych", receipt.NextCell.AdjacentTo);
        Assert.Contains(receipt.Cells, cell => cell.CellId == "full-body.install-anchor" && cell.Status == SpiralBuildCellStatus.VerifiedCold);
        Assert.Contains(receipt.Cells, cell => cell.CellId == "full-body.codex-proxy-triptych" && cell.Status == SpiralBuildCellStatus.VerifiedCold);
        Assert.False(receipt.HitlRequired);
        Assert.True(receipt.AutomationMayContinue);
        Assert.Contains("telemetry attempts authority", receipt.AutomationStopConditions);
        AssertForbiddenMotionFalse(receipt);
    }

    [Fact]
    public void CreateReceipt_Selects_Profile_Cell_When_Triptych_Profile_Is_Not_Yet_Emitted()
    {
        using var fixture = SpiralFixture.Create(includePreflight: true, includeTriptychProfile: false);

        var receipt = new DefaultSpiralBuildAutomationService().CreateReceipt(
            fixture.CreateRequest(),
            TimestampUtc);

        Assert.Equal(SpiralBuildAutomationDisposition.ReadyCold, receipt.Disposition);
        Assert.Equal("full-body.codex-proxy-triptych", receipt.NextCell?.CellId);
        Assert.True(receipt.IsColdAutomationReady);
    }

    [Fact]
    public void CreateReceipt_Withholds_When_Install_Surface_Is_Missing()
    {
        using var fixture = SpiralFixture.Create(createInstallSurface: false);

        var receipt = new DefaultSpiralBuildAutomationService().CreateReceipt(
            fixture.CreateRequest(),
            TimestampUtc);

        Assert.Equal(SpiralBuildAutomationDisposition.Withheld, receipt.Disposition);
        Assert.Equal("spiral-build-install-surface-missing", receipt.OutcomeCode);
        Assert.Null(receipt.NextCell);
        Assert.False(receipt.AutomationMayContinue);
        Assert.True(receipt.HitlRequired);
    }

    [Fact]
    public void CreateReceipt_Refuses_Runtime_Motion_Request()
    {
        using var fixture = SpiralFixture.Create();
        var request = fixture.CreateRequest() with
        {
            SanctuaryActualRequested = true
        };

        var receipt = new DefaultSpiralBuildAutomationService().CreateReceipt(request, TimestampUtc);

        Assert.Equal(SpiralBuildAutomationDisposition.Refused, receipt.Disposition);
        Assert.Equal("spiral-build-runtime-motion-refused", receipt.OutcomeCode);
        Assert.Null(receipt.NextCell);
        Assert.False(receipt.AutomationMayContinue);
        Assert.True(receipt.HitlRequired);
        Assert.False(receipt.SanctuaryActualAllowed);
    }

    private static void AssertForbiddenMotionFalse(SpiralBuildAutomationReceipt receipt)
    {
        Assert.True(receipt.ActivationRefused);
        Assert.False(receipt.ModelBindingAllowed);
        Assert.False(receipt.LispEvaluationAllowed);
        Assert.False(receipt.RuntimeIdentityAllowed);
        Assert.False(receipt.RuntimeActionAllowed);
        Assert.False(receipt.DatabaseWriteAllowed);
        Assert.False(receipt.GelPromotionAllowed);
        Assert.False(receipt.CmeActualAllowed);
        Assert.False(receipt.SanctuaryActualAllowed);
    }

    private sealed class SpiralFixture : IDisposable
    {
        private SpiralFixture(string rootPath)
        {
            RootPath = rootPath;
            LineRootPath = Path.Combine(rootPath, "line-root");
            InstallRootPath = Path.Combine(rootPath, "Sanctuary");
        }

        public string RootPath { get; }

        public string LineRootPath { get; }

        public string InstallRootPath { get; }

        public static SpiralFixture Create(
            bool createInstallSurface = true,
            bool includePreflight = false,
            bool includeTriptychProfile = false)
        {
            var fixture = new SpiralFixture(Path.Combine(Path.GetTempPath(), $"san-spiral-tests-{Guid.NewGuid():N}"));
            Directory.CreateDirectory(fixture.LineRootPath);

            if (createInstallSurface)
            {
                Directory.CreateDirectory(Path.Combine(fixture.InstallRootPath, "product"));
                File.WriteAllText(Path.Combine(fixture.InstallRootPath, "sanctuary.cmd"), string.Empty);
                File.WriteAllText(Path.Combine(fixture.InstallRootPath, "product", "San.Launcher.exe"), string.Empty);
            }

            if (includePreflight)
            {
                Directory.CreateDirectory(Path.Combine(fixture.InstallRootPath, "receipts", "preflight"));
                File.WriteAllText(Path.Combine(fixture.InstallRootPath, "receipts", "preflight", "product-body-status.json"), "{}");
            }

            if (includeTriptychProfile)
            {
                Directory.CreateDirectory(Path.Combine(fixture.InstallRootPath, "receipts", "sanctuary-actual-test-profile"));
                File.WriteAllText(Path.Combine(fixture.InstallRootPath, "receipts", "sanctuary-actual-test-profile", "sanctuary-actual-test-profile.json"), "{}");
            }

            return fixture;
        }

        public SpiralBuildAutomationRequest CreateRequest() =>
            new(
                LineRootPath: LineRootPath,
                InstallRootPath: InstallRootPath);

        public void Dispose()
        {
            if (Directory.Exists(RootPath))
            {
                Directory.Delete(RootPath, recursive: true);
            }
        }
    }
}
