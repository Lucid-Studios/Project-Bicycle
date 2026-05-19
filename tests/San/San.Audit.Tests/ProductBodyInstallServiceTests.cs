using San.Product.Preflight;
using Xunit;

namespace San.Audit.Tests;

public sealed class ProductBodyInstallServiceTests
{
    private static readonly DateTimeOffset TimestampUtc = DateTimeOffset.Parse("2026-05-12T00:00:00Z");

    [Fact]
    public void Install_Creates_Cold_Local_Tool_Surface_With_Receipts_And_Shims()
    {
        using var fixture = InstallFixture.Create();

        var receipt = new DefaultProductBodyInstallService().Install(
            fixture.CreateRequest(),
            TimestampUtc);

        Assert.Equal(ProductBodyInstallDisposition.InstalledCold, receipt.Disposition);
        Assert.Equal("local-sanctuary-install-verified-cold", receipt.OutcomeCode);
        Assert.True(receipt.IsColdInstall);
        Assert.True(receipt.ColdBuildToolSurfaceReady);
        Assert.True(receipt.ActivationRefused);
        Assert.False(receipt.ModelBindingAllowed);
        Assert.False(receipt.LispEvaluationAllowed);
        Assert.False(receipt.RuntimeIdentityAllowed);
        Assert.False(receipt.RuntimeActionAllowed);
        Assert.False(receipt.DatabaseWriteAllowed);
        Assert.False(receipt.GelPromotionAllowed);
        Assert.False(receipt.CmeActualAllowed);
        Assert.False(receipt.SanctuaryActualAllowed);
        Assert.True(File.Exists(receipt.ProductExecutablePath));
        Assert.True(File.Exists(receipt.CommandShimPath));
        Assert.True(File.Exists(receipt.PowerShellShimPath));
        Assert.True(File.Exists(receipt.PreflightReceiptJsonPath));
        Assert.True(File.Exists(receipt.PreflightReceiptMarkdownPath));
        Assert.True(File.Exists(Path.Combine(receipt.InstallRootPath, "SANCTUARY_INSTALL_RECEIPT.json")));
        Assert.True(File.Exists(Path.Combine(receipt.InstallRootPath, "SANCTUARY_INSTALL_RECEIPT.md")));
        Assert.True(File.Exists(Path.Combine(receipt.BuildInstallRootPath, "line-manifest.json")));
        Assert.True(File.Exists(Path.Combine(receipt.BuildInstallRootPath, "lab-sanctuary-verification-settings.json")));
        Assert.Contains("--line-root", File.ReadAllText(receipt.CommandShimPath), StringComparison.Ordinal);
        Assert.Contains("--report-dir", File.ReadAllText(receipt.PowerShellShimPath), StringComparison.Ordinal);
        Assert.Contains("standalone tool body", receipt.GovernanceTrace, StringComparison.Ordinal);
        Assert.DoesNotContain("current runnable truth", receipt.GovernanceTrace, StringComparison.Ordinal);
        Assert.Equal(2, receipt.CopiedProductFileCount);
    }

    [Fact]
    public void Install_Report_Distinguishes_Command_Executable_From_Runtime_Authority()
    {
        using var fixture = InstallFixture.Create();

        var receipt = new DefaultProductBodyInstallService().Install(
            fixture.CreateRequest(),
            TimestampUtc);

        var markdown = ProductBodyInstallReportWriter.ToMarkdown(receipt);

        Assert.Contains("Product command executable", markdown, StringComparison.Ordinal);
        Assert.DoesNotContain("Product executable:", markdown, StringComparison.Ordinal);
        Assert.Contains("Runtime action allowed: `False`", markdown, StringComparison.Ordinal);
        Assert.Contains("Sanctuary.Actual allowed: `False`", markdown, StringComparison.Ordinal);
        Assert.Contains("standalone tool body", markdown, StringComparison.Ordinal);
    }

    [Fact]
    public void Install_Withholds_When_Product_Launcher_Is_Missing()
    {
        using var fixture = InstallFixture.Create(createLauncher: false);

        var receipt = new DefaultProductBodyInstallService().Install(
            fixture.CreateRequest(),
            TimestampUtc);

        Assert.Equal(ProductBodyInstallDisposition.Withheld, receipt.Disposition);
        Assert.Equal("install-product-launcher-missing", receipt.OutcomeCode);
        Assert.False(receipt.IsColdInstall);
        Assert.False(File.Exists(receipt.ProductExecutablePath));
    }

    [Fact]
    public void Install_Withholds_When_Install_Root_Overlaps_Line_Root()
    {
        using var fixture = InstallFixture.Create();
        var request = fixture.CreateRequest(installRootPath: Path.Combine(fixture.LineRootPath, "nested-install"));

        var receipt = new DefaultProductBodyInstallService().Install(request, TimestampUtc);

        Assert.Equal(ProductBodyInstallDisposition.Withheld, receipt.Disposition);
        Assert.Equal("install-root-must-not-overlap-source", receipt.OutcomeCode);
        Assert.False(receipt.IsColdInstall);
    }

    [Fact]
    public void Install_Refuses_Runtime_Motion_Request()
    {
        using var fixture = InstallFixture.Create();
        var request = fixture.CreateRequest() with
        {
            RuntimeActionRequested = true
        };

        var receipt = new DefaultProductBodyInstallService().Install(request, TimestampUtc);

        Assert.Equal(ProductBodyInstallDisposition.Refused, receipt.Disposition);
        Assert.Equal("install-runtime-motion-refused", receipt.OutcomeCode);
        Assert.False(receipt.IsColdInstall);
        Assert.False(receipt.RuntimeActionAllowed);
        Assert.True(receipt.ActivationRefused);
    }

    private sealed class InstallFixture : IDisposable
    {
        private InstallFixture(string rootPath)
        {
            RootPath = rootPath;
            LineRootPath = Path.Combine(rootPath, "tool-root");
            ProductSourceRootPath = Path.Combine(rootPath, "product-source");
            InstallRootPath = Path.Combine(rootPath, "Sanctuary");
        }

        public string RootPath { get; }

        public string LineRootPath { get; }

        public string ProductSourceRootPath { get; }

        public string InstallRootPath { get; }

        public static InstallFixture Create(bool createLauncher = true)
        {
            var fixture = new InstallFixture(Path.Combine(Path.GetTempPath(), $"san-install-tests-{Guid.NewGuid():N}"));
            Directory.CreateDirectory(fixture.LineRootPath);
            Directory.CreateDirectory(Path.Combine(fixture.LineRootPath, "build"));
            Directory.CreateDirectory(fixture.ProductSourceRootPath);
            File.WriteAllText(Path.Combine(fixture.LineRootPath, "San.sln"), string.Empty);
            File.WriteAllText(
                Path.Combine(fixture.LineRootPath, "build", "line-manifest.json"),
                """
                {
                  "lineName": "Project Bicycle",
                  "lineVersion": "0.2.1",
                  "posture": "standalone-tool-body",
                  "solutionPath": "San.sln",
                  "parentLine": "",
                  "activeExecutableTruth": "Project Bicycle",
                  "buildable": true,
                  "sourceMaterialized": true,
                  "runtimeMaterialized": false
                }
                """);
            File.WriteAllText(
                Path.Combine(fixture.LineRootPath, "build", "lab-sanctuary-verification-settings.json"),
                """
                {
                  "settingId": "lab-sanctuary-build-testing",
                  "activationAuthorityRequired": false
                }
                """);

            if (createLauncher)
            {
                File.WriteAllText(Path.Combine(fixture.ProductSourceRootPath, "San.Launcher.exe"), string.Empty);
                File.WriteAllText(Path.Combine(fixture.ProductSourceRootPath, "San.Launcher.dll"), string.Empty);
            }

            return fixture;
        }

        public ProductBodyInstallRequest CreateRequest(string? installRootPath = null) =>
            new(
                LineRootPath: LineRootPath,
                InstallRootPath: installRootPath ?? InstallRootPath,
                ProductSourceRootPath: ProductSourceRootPath,
                VerificationProfile: ProductBodyVerificationProfiles.ColdProductBody);

        public void Dispose()
        {
            if (Directory.Exists(RootPath))
            {
                Directory.Delete(RootPath, recursive: true);
            }
        }
    }
}
