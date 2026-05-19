using San.Product.Preflight;
using Xunit;

namespace San.Audit.Tests;

public sealed class SanctuaryThresholdTestProfileServiceTests
{
    private static readonly DateTimeOffset TimestampUtc = DateTimeOffset.Parse("2026-05-12T00:00:00Z");

    [Fact]
    public void CreateProfile_Returns_Cold_Codex_Proxy_Triptych_When_Install_Surface_Is_Ready()
    {
        using var fixture = ProfileFixture.Create();

        var profile = new DefaultSanctuaryThresholdTestProfileService().CreateProfile(
            fixture.CreateRequest(),
            TimestampUtc);

        Assert.Equal(SanctuaryThresholdTestProfileDisposition.ReadyCold, profile.Disposition);
        Assert.Equal("sanctuary-actual-codex-proxy-triptych-ready-cold", profile.OutcomeCode);
        Assert.True(profile.IsColdProxyProfile);
        Assert.Contains(".Actual names an authorized running/actionable body state", profile.ActualNamingLaw, StringComparison.Ordinal);
        Assert.Equal("Sanctuary.Actual", profile.ReservedActionableStateName);
        Assert.Equal("Sanctuary.ColdInstalled", profile.CurrentInstallStateName);
        Assert.False(profile.ReservedActionableStateAuthorized);
        Assert.Equal(SanctuaryThresholdCognitionProviderKind.CodexProxy, profile.BaseProvider.ProviderKind);
        Assert.True(profile.BaseProvider.BaseForBuildTesting);
        Assert.True(profile.BaseProvider.LocalHostedLlmDeferred);
        Assert.False(profile.BaseProvider.PersistentMemoryClaimed);
        Assert.False(profile.BaseProvider.RuntimeIdentityClaimed);
        Assert.Contains(profile.RoleSeats, seat => seat.SeatKind == SanctuaryThresholdRoleSeatKind.Prime);
        Assert.Contains(profile.RoleSeats, seat => seat.SeatKind == SanctuaryThresholdRoleSeatKind.Cryptic);
        Assert.Contains(profile.RoleSeats, seat => seat.SeatKind == SanctuaryThresholdRoleSeatKind.Steward);
        Assert.All(profile.RoleSeats, seat =>
        {
            Assert.Equal(SanctuaryThresholdRoleSeatStatus.ProxyOnly, seat.Status);
            Assert.Equal("spawn-explicit-when-needed", seat.InvocationMode);
            Assert.False(seat.GrantsAuthority);
            Assert.False(seat.SelfAuthorizes);
            Assert.False(seat.ActivatesCmeActual);
            Assert.False(seat.RequiresLocalHostedLlm);
        });
        Assert.True(profile.CodexProxyMayBuild);
        Assert.False(profile.CodexProxyMayAuthorize);
        Assert.True(profile.LocalHostedLlmDeferredUntilFirstCmeTest);
        AssertForbiddenMotionFalse(profile);
    }

    [Fact]
    public void CreateProfile_Withholds_When_Install_Surface_Is_Incomplete()
    {
        using var fixture = ProfileFixture.Create(createLauncher: false);

        var profile = new DefaultSanctuaryThresholdTestProfileService().CreateProfile(
            fixture.CreateRequest(),
            TimestampUtc);

        Assert.Equal(SanctuaryThresholdTestProfileDisposition.Withheld, profile.Disposition);
        Assert.Equal("sanctuary-actual-proxy-install-surface-incomplete", profile.OutcomeCode);
        Assert.False(profile.IsColdProxyProfile);
        Assert.All(profile.RoleSeats, seat => Assert.Equal(SanctuaryThresholdRoleSeatStatus.Withheld, seat.Status));
        AssertForbiddenMotionFalse(profile);
    }

    [Fact]
    public void CreateProfile_Refuses_When_Runtime_Motion_Is_Requested()
    {
        using var fixture = ProfileFixture.Create();
        var request = fixture.CreateRequest() with
        {
            CmeActualRequested = true
        };

        var profile = new DefaultSanctuaryThresholdTestProfileService().CreateProfile(request, TimestampUtc);

        Assert.Equal(SanctuaryThresholdTestProfileDisposition.Refused, profile.Disposition);
        Assert.Equal("sanctuary-actual-proxy-runtime-motion-refused", profile.OutcomeCode);
        Assert.False(profile.IsColdProxyProfile);
        Assert.False(profile.CmeActualAllowed);
        Assert.True(profile.ActivationRefused);
        Assert.All(profile.RoleSeats, seat => Assert.Equal(SanctuaryThresholdRoleSeatStatus.Refused, seat.Status));
    }

    private static void AssertForbiddenMotionFalse(SanctuaryThresholdTestProfile profile)
    {
        Assert.True(profile.ActivationRefused);
        Assert.False(profile.ModelBindingAllowed);
        Assert.False(profile.LispEvaluationAllowed);
        Assert.False(profile.RuntimeIdentityAllowed);
        Assert.False(profile.RuntimeActionAllowed);
        Assert.False(profile.DatabaseWriteAllowed);
        Assert.False(profile.GelPromotionAllowed);
        Assert.False(profile.CmeActualAllowed);
        Assert.False(profile.SanctuaryActualAllowed);
    }

    private sealed class ProfileFixture : IDisposable
    {
        private ProfileFixture(string rootPath)
        {
            RootPath = rootPath;
            LineRootPath = Path.Combine(rootPath, "line-root");
            InstallRootPath = Path.Combine(rootPath, "Sanctuary");
        }

        public string RootPath { get; }

        public string LineRootPath { get; }

        public string InstallRootPath { get; }

        public static ProfileFixture Create(bool createLauncher = true)
        {
            var fixture = new ProfileFixture(Path.Combine(Path.GetTempPath(), $"san-profile-tests-{Guid.NewGuid():N}"));
            Directory.CreateDirectory(fixture.LineRootPath);
            Directory.CreateDirectory(Path.Combine(fixture.InstallRootPath, "product"));
            File.WriteAllText(Path.Combine(fixture.InstallRootPath, "sanctuary.cmd"), string.Empty);

            if (createLauncher)
            {
                File.WriteAllText(Path.Combine(fixture.InstallRootPath, "product", "San.Launcher.exe"), string.Empty);
            }

            return fixture;
        }

        public SanctuaryThresholdTestProfileRequest CreateRequest() =>
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
