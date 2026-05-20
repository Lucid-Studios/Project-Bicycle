using San.Sanctuary.Runtime;
using SLI.Lisp;
using Xunit;

namespace San.Audit.Tests;

public sealed class SanctuaryInstalledSubstrateServiceTests
{
    private static readonly DateTimeOffset TimestampUtc = DateTimeOffset.Parse("2026-05-19T00:00:00Z");

    [Fact]
    public void Install_Composes_Sanctuary_Base_Condensate_And_Role_Bodies()
    {
        using var fixture = SanctuaryInstalledFixture.Create();
        var service = new DefaultSanctuaryInstalledSubstrateService(new FixedSliLispRuntimeLoadService(CreateLoadedLispReceipt()));

        var receipt = service.Install(fixture.CreateRequest(), TimestampUtc);

        Assert.Equal(SanctuaryInstalledSubstrateDisposition.InstalledCold, receipt.Disposition);
        Assert.Equal("sanctuary-installed-body-composed-cold", receipt.OutcomeCode);
        Assert.True(receipt.IsColdInstalledSubstrate);
        Assert.Equal(SanctuaryInstalledSubstrateReceipt.ExpectedInstalledBodyCount, receipt.Bodies.Count);
        Assert.True(receipt.BaseBodiesInstalled);
        Assert.True(receipt.CondensateBodiesInstalled);
        Assert.True(receipt.RoleBodiesInstalled);
        Assert.Equal("Sanctuary.ID", receipt.RootIdentity.SanctuaryId);
        Assert.Equal("Sanctuary.ID", receipt.RootIdentity.OperatorId);
        Assert.Equal("Sanctuary.CME.Actual", receipt.RootIdentity.ActualNameCandidate);
        Assert.Equal("OE.Sanctuary.ID", receipt.RootIdentity.OpalEngramRootId);
        Assert.Equal("SelfGEL.Sanctuary.ID", receipt.RootIdentity.SelfGelRootId);
        Assert.False(receipt.RootIdentity.HeartbeatActive);
        Assert.False(receipt.RootIdentity.GrantsAuthority);
        Assert.False(receipt.RootIdentity.AdmitsContinuity);
        AssertBody(receipt, SanctuaryInstalledBodyKind.Gel, "Sanctuary.GEL", ["SLI.Lisp.Prime"]);
        AssertBody(receipt, SanctuaryInstalledBodyKind.Goa, "Sanctuary.GoA", ["ListeningFrame", "ExternalFormation"]);
        AssertBody(receipt, SanctuaryInstalledBodyKind.Mos, "Sanctuary.MoS", ["OE.Sanctuary.ID", "SelfGEL.Sanctuary.ID"]);
        AssertBody(receipt, SanctuaryInstalledBodyKind.Vault, "Sanctuary.Vault", ["WitnessReceipts", "RefusalReceipts"]);
        AssertBody(receipt, SanctuaryInstalledBodyKind.CGel, "Sanctuary.cGEL", ["Sanctuary.GEL"]);
        AssertBody(receipt, SanctuaryInstalledBodyKind.CGoa, "Sanctuary.cGoA", ["Sanctuary.GoA", "SLI.Lisp.ControlMatrix"]);
        AssertBody(receipt, SanctuaryInstalledBodyKind.CMos, "Sanctuary.cMoS", ["Sanctuary.MoS", "SLI.Lisp.Cryptic"]);
        AssertBody(receipt, SanctuaryInstalledBodyKind.CVault, "Sanctuary.cVault", ["Sanctuary.Vault"]);
        AssertBody(receipt, SanctuaryInstalledBodyKind.Prime, "Prime", ["Sanctuary.GEL", "SLI.Lisp.Prime"]);
        AssertBody(receipt, SanctuaryInstalledBodyKind.Cryptic, "Cryptic", ["Sanctuary.cMoS", "SLI.Lisp.Cryptic"]);
        AssertBody(receipt, SanctuaryInstalledBodyKind.Steward, "Steward", ["Sanctuary.cGoA", "SLI.Lisp.ControlMatrix"]);
        AssertNoRuntimeMotion(receipt);
        Assert.True(File.Exists(receipt.ReceiptJsonPath));
        Assert.True(File.Exists(receipt.ReceiptMarkdownPath));
        Assert.Contains("Sanctuary.cGoA", File.ReadAllText(receipt.ReceiptJsonPath), StringComparison.Ordinal);
        Assert.Contains("Sanctuary Installed Body", File.ReadAllText(receipt.ReceiptMarkdownPath), StringComparison.Ordinal);
    }

    [Fact]
    public void Install_Roots_Custom_Operator_Name_As_CmeActual_Id_And_Oe_SelfGel()
    {
        using var fixture = SanctuaryInstalledFixture.Create();
        var service = new DefaultSanctuaryInstalledSubstrateService(new FixedSliLispRuntimeLoadService(CreateLoadedLispReceipt()));

        var receipt = service.Install(
            fixture.CreateRequest(operatorName: "Your Name Here", domain: "Civic", role: "Paternal Care Assistance", jobClass: "Listening"),
            TimestampUtc);

        Assert.Equal(SanctuaryInstalledSubstrateDisposition.InstalledCold, receipt.Disposition);
        Assert.True(receipt.IsColdInstalledSubstrate);
        Assert.Equal("YourNameHere.ID", receipt.RootIdentity.OperatorId);
        Assert.Equal("YourNameHere.CME.Actual", receipt.RootIdentity.ActualNameCandidate);
        Assert.Equal("YourNameHere.CME.Actual.ID", receipt.RootIdentity.CmeActualIdCandidate);
        Assert.Equal("OE.YourNameHere.ID", receipt.RootIdentity.OpalEngramRootId);
        Assert.Equal("SelfGEL.YourNameHere.ID", receipt.RootIdentity.SelfGelRootId);
        Assert.Equal("Civic", receipt.RootIdentity.Domain);
        Assert.Equal("PaternalCareAssistance", receipt.RootIdentity.Role);
        Assert.Equal("Listening", receipt.RootIdentity.JobClass);
        Assert.False(receipt.CmeActualAllowed);
        Assert.False(receipt.SanctuaryActualAllowed);
    }

    [Fact]
    public void Install_Withholds_When_Sli_Lisp_Membrane_Does_Not_Load()
    {
        using var fixture = SanctuaryInstalledFixture.Create();
        var service = new DefaultSanctuaryInstalledSubstrateService(new FixedSliLispRuntimeLoadService(CreateWithheldLispReceipt()));

        var receipt = service.Install(fixture.CreateRequest(), TimestampUtc);

        Assert.Equal(SanctuaryInstalledSubstrateDisposition.Withheld, receipt.Disposition);
        Assert.Equal("sanctuary-installed-body-sli-lisp-load-withheld", receipt.OutcomeCode);
        Assert.False(receipt.IsColdInstalledSubstrate);
        Assert.Empty(receipt.Bodies);
        Assert.False(receipt.BaseBodiesInstalled);
        Assert.False(receipt.CondensateBodiesInstalled);
        Assert.False(receipt.RoleBodiesInstalled);
        Assert.NotNull(receipt.SliLispLoadReceipt);
        Assert.False(receipt.SliLispLoadReceipt!.LoadSucceeded);
        Assert.True(File.Exists(receipt.ReceiptJsonPath));
        Assert.True(File.Exists(receipt.ReceiptMarkdownPath));
    }

    [Fact]
    public void Install_Refuses_When_Runtime_Motion_Is_Requested()
    {
        using var fixture = SanctuaryInstalledFixture.Create();
        var service = new DefaultSanctuaryInstalledSubstrateService(new FixedSliLispRuntimeLoadService(CreateLoadedLispReceipt()));

        var receipt = service.Install(
            fixture.CreateRequest() with
            {
                CmeActualRequested = true,
                RuntimeActionRequested = true
            },
            TimestampUtc);

        Assert.Equal(SanctuaryInstalledSubstrateDisposition.Refused, receipt.Disposition);
        Assert.Equal("sanctuary-installed-body-runtime-motion-refused", receipt.OutcomeCode);
        Assert.False(receipt.IsColdInstalledSubstrate);
        Assert.Empty(receipt.Bodies);
        Assert.Null(receipt.SliLispLoadReceipt);
        AssertNoRuntimeMotion(receipt);
    }

    [Fact]
    public void Report_States_Installed_Bodies_Without_Activation()
    {
        using var fixture = SanctuaryInstalledFixture.Create();
        var service = new DefaultSanctuaryInstalledSubstrateService(new FixedSliLispRuntimeLoadService(CreateLoadedLispReceipt()));

        var receipt = service.Install(fixture.CreateRequest(), TimestampUtc);
        var report = SanctuaryInstalledSubstrateReportWriter.ToMarkdown(receipt);

        Assert.Contains("Sanctuary.GEL", report, StringComparison.Ordinal);
        Assert.Contains("Sanctuary.cGoA", report, StringComparison.Ordinal);
        Assert.Contains("Steward", report, StringComparison.Ordinal);
        Assert.Contains("CME.Actual allowed: `False`", report, StringComparison.Ordinal);
        Assert.Contains("Activation refused: `True`", report, StringComparison.Ordinal);
        Assert.Contains("Lisp evaluation allowed: `False`", report, StringComparison.Ordinal);
    }

    private static void AssertBody(
        SanctuaryInstalledSubstrateReceipt receipt,
        SanctuaryInstalledBodyKind bodyKind,
        string bodyName,
        IReadOnlyList<string> sourceBodyNames)
    {
        var body = Assert.Single(receipt.Bodies, candidate => candidate.BodyKind == bodyKind);
        Assert.Equal(bodyName, body.BodyName);
        Assert.Equal(sourceBodyNames, body.SourceBodyNames);
        Assert.True(body.IsColdBody);
    }

    private static void AssertNoRuntimeMotion(SanctuaryInstalledSubstrateReceipt receipt)
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

    private static SliLispRuntimeLoadReceipt CreateLoadedLispReceipt() =>
        new(
            ReceiptHandle: "sli-lisp-runtime-load://loaded",
            Disposition: SliLispRuntimeLoadDisposition.LoadedCold,
            OutcomeCode: "sli-lisp-resident-membrane-loaded-cold",
            RuntimeKind: "SBCL",
            RuntimePath: "sbcl",
            ModuleNames: ["core.lisp", "parser.lisp", "compass.lisp"],
            ModuleCount: 3,
            LoadedFromEmbeddedResources: true,
            LoadAttempted: true,
            LoadSucceeded: true,
            ResidentModuleLoadAllowed: true,
            TopLevelLoadEvaluationExpected: true,
            ArbitraryEvaluationAllowed: false,
            RuntimeActionAllowed: false,
            ActivationAllowed: false,
            AuthorityGranted: false,
            ModelBindingAllowed: false,
            GelPromotionAllowed: false,
            CmeActualAllowed: false,
            SanctuaryActualAllowed: false,
            ExitCode: 0,
            StandardOutput: "SAN-SLI-LISP-RUNTIME-LOAD-OK",
            StandardError: string.Empty,
            TimestampUtc: TimestampUtc);

    private static SliLispRuntimeLoadReceipt CreateWithheldLispReceipt() =>
        CreateLoadedLispReceipt() with
        {
            Disposition = SliLispRuntimeLoadDisposition.Withheld,
            OutcomeCode = "sli-lisp-resident-membrane-load-failed",
            LoadSucceeded = false,
            ExitCode = 1,
            StandardOutput = string.Empty,
            StandardError = "load failed"
        };

    private sealed class FixedSliLispRuntimeLoadService : ISliLispRuntimeLoadService
    {
        private readonly SliLispRuntimeLoadReceipt receipt;

        public FixedSliLispRuntimeLoadService(SliLispRuntimeLoadReceipt receipt)
        {
            this.receipt = receipt;
        }

        public SliLispRuntimeLoadReceipt LoadResidentMembrane(
            SliLispRuntimeLoadRequest request,
            DateTimeOffset timestampUtc) =>
            receipt;
    }

    private sealed class SanctuaryInstalledFixture : IDisposable
    {
        private SanctuaryInstalledFixture(string rootPath)
        {
            RootPath = rootPath;
            LineRootPath = Path.Combine(rootPath, "line-root");
            InstallRootPath = Path.Combine(rootPath, "Sanctuary");
        }

        public string RootPath { get; }

        public string LineRootPath { get; }

        public string InstallRootPath { get; }

        public static SanctuaryInstalledFixture Create()
        {
            var fixture = new SanctuaryInstalledFixture(Path.Combine(Path.GetTempPath(), $"sanctuary-installed-body-tests-{Guid.NewGuid():N}"));
            Directory.CreateDirectory(fixture.LineRootPath);
            File.WriteAllText(Path.Combine(fixture.LineRootPath, "San.sln"), string.Empty);
            return fixture;
        }

        public SanctuaryInstalledSubstrateRequest CreateRequest(
            string? operatorName = null,
            string? domain = null,
            string? role = null,
            string? jobClass = null) =>
            new(
                LineRootPath: LineRootPath,
                InstallRootPath: InstallRootPath,
                OperatorName: operatorName ?? "Sanctuary",
                Domain: domain ?? "Sanctuary",
                Role: role ?? "InstalledBody",
                JobClass: jobClass ?? "ColdBench");

        public void Dispose()
        {
            if (Directory.Exists(RootPath))
            {
                Directory.Delete(RootPath, recursive: true);
            }
        }
    }
}
