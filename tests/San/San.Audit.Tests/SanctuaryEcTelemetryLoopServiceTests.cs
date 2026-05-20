using San.Sanctuary.Runtime;
using SLI.Lisp;
using Xunit;

namespace San.Audit.Tests;

public sealed class SanctuaryEcTelemetryLoopServiceTests
{
    private static readonly DateTimeOffset TimestampUtc = DateTimeOffset.Parse("2026-05-19T00:00:00Z");

    [Fact]
    public void Run_Wraps_Sli_Lisp_Owned_Ec_Motion_Without_CSharp_Claiming_Engine_Ownership()
    {
        using var fixture = SanctuaryInstalledFixture.Create();
        var installed = new DefaultSanctuaryInstalledSubstrateService(new FixedSliLispRuntimeLoadService(CreateLoadedRuntimeReceipt()))
            .Install(fixture.CreateInstallRequest(), TimestampUtc);
        var service = new DefaultSanctuaryEcTelemetryLoopService(new FixedSliLispEcTelemetryLoopService(CreateLoadedEcReceipt()));

        var receipt = service.Run(
            new SanctuaryEcTelemetryLoopRequest(
                InstalledSubstrateReceipt: installed,
                ThoughtForm: "Listen to the predicate pressure and return without admission."),
            TimestampUtc);

        Assert.Equal(SanctuaryEcTelemetryLoopDisposition.CompletedCold, receipt.Disposition);
        Assert.Equal("sanctuary-ec-loop-sli-lisp-engine-completed-cold", receipt.OutcomeCode);
        Assert.True(receipt.IsColdEcTelemetryLoop);
        Assert.True(receipt.SliLispOwnedEngineMotion);
        Assert.Equal("sli.lisp", receipt.SliLispEngineReceipt!.Telemetry["engine-owner"]);
        Assert.Equal("run-ec-telemetry-loop", receipt.SliLispEngineReceipt.Telemetry["bounded-entrypoint"]);
        Assert.True(receipt.ListeningFrameReceived);
        Assert.True(receipt.CompassOrientedPressure);
        Assert.True(receipt.ThinkingAboutThinkingTelemetryProduced);
        Assert.True(receipt.PreEngramResidueProduced);
        Assert.Equal(6, receipt.PreEngramResidueCount);
        Assert.True(receipt.StewardReviewed);
        Assert.False(receipt.StreamAdmittedEngram);
        Assert.False(receipt.StreamAdmittedMemory);
        Assert.False(receipt.SelfGelMutated);
        Assert.False(receipt.ContinuityAdmitted);
        Assert.False(receipt.AuthorityGranted);
        Assert.False(receipt.ModelBindingAllowed);
        Assert.False(receipt.ArbitraryLispEvaluationAllowed);
        Assert.False(receipt.RuntimeActionAllowed);
        Assert.False(receipt.CmeActualAllowed);
        Assert.False(receipt.SanctuaryActualAllowed);
        Assert.True(File.Exists(receipt.ReceiptJsonPath));
        Assert.True(File.Exists(receipt.ReceiptMarkdownPath));
        Assert.Contains("SLI.Lisp owned engine motion: `True`", File.ReadAllText(receipt.ReceiptMarkdownPath), StringComparison.Ordinal);
    }

    [Fact]
    public void Run_Withholds_Without_Cold_Installed_Substrate()
    {
        var service = new DefaultSanctuaryEcTelemetryLoopService(new FixedSliLispEcTelemetryLoopService(CreateLoadedEcReceipt()));

        var receipt = service.Run(
            new SanctuaryEcTelemetryLoopRequest(
                InstalledSubstrateReceipt: null,
                ThoughtForm: "no socket"),
            TimestampUtc);

        Assert.Equal(SanctuaryEcTelemetryLoopDisposition.Withheld, receipt.Disposition);
        Assert.Equal("sanctuary-ec-loop-installed-substrate-missing", receipt.OutcomeCode);
        Assert.False(receipt.IsColdEcTelemetryLoop);
        Assert.Null(receipt.SliLispEngineReceipt);
        Assert.False(receipt.SliLispOwnedEngineMotion);
    }

    [Fact]
    public void Run_Refuses_Runtime_Motion_Before_Sli_Lisp_Invocation()
    {
        using var fixture = SanctuaryInstalledFixture.Create();
        var installed = new DefaultSanctuaryInstalledSubstrateService(new FixedSliLispRuntimeLoadService(CreateLoadedRuntimeReceipt()))
            .Install(fixture.CreateInstallRequest(), TimestampUtc);
        var service = new DefaultSanctuaryEcTelemetryLoopService(new FixedSliLispEcTelemetryLoopService(CreateLoadedEcReceipt()));

        var receipt = service.Run(
            new SanctuaryEcTelemetryLoopRequest(
                InstalledSubstrateReceipt: installed,
                ThoughtForm: "make it act",
                RuntimeActionRequested: true),
            TimestampUtc);

        Assert.Equal(SanctuaryEcTelemetryLoopDisposition.Refused, receipt.Disposition);
        Assert.Equal("sanctuary-ec-loop-runtime-motion-refused", receipt.OutcomeCode);
        Assert.False(receipt.IsColdEcTelemetryLoop);
        Assert.Null(receipt.SliLispEngineReceipt);
        Assert.False(receipt.RuntimeActionAllowed);
        Assert.True(receipt.ActivationRefused);
        Assert.True(File.Exists(receipt.ReceiptJsonPath));
        Assert.True(File.Exists(receipt.ReceiptMarkdownPath));
        Assert.Contains("runtime-motion-refused", File.ReadAllText(receipt.ReceiptJsonPath), StringComparison.Ordinal);
    }

    private static SliLispEcTelemetryLoopReceipt CreateLoadedEcReceipt() =>
        new(
            ReceiptHandle: "sli-lisp-ec-loop://loaded",
            Disposition: SliLispEcTelemetryLoopDisposition.CompletedCold,
            OutcomeCode: "sli-lisp-ec-telemetry-loop-completed-cold",
            RuntimeKind: "SBCL",
            RuntimePath: "sbcl",
            ThoughtForm: "Listen to the predicate pressure and return without admission.",
            Telemetry: new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["engine-owner"] = "sli.lisp",
                ["bounded-entrypoint"] = "run-ec-telemetry-loop"
            },
            ModuleNames: ["core.lisp", "ec-telemetry-loop.lisp"],
            ModuleCount: 2,
            BoundedEntrypointCalled: true,
            LoadAttempted: true,
            LoadSucceeded: true,
            ColdEngineLoopCompleted: true,
            ListeningFrameReceived: true,
            SliMembraneInterpretedPredicatePressure: true,
            CompassOrientedPressure: true,
            CompassCoolingRequired: true,
            SoulFrameReceivedListeningFrame: true,
            AgentiCoreReceivedCompassPressure: true,
            ThinkingAboutThinkingTelemetryProduced: true,
            PreEngramResidueProduced: true,
            PreEngramResidueCount: 6,
            PreEngramResidueClasses: ["semantic", "pressure", "witness", "governance", "morphology", "return"],
            StewardReviewed: true,
            EngramAdmissionAllowed: false,
            MemoryAdmissionAllowed: false,
            SelfGelMutationAllowed: false,
            ContinuityAdmissionAllowed: false,
            AuthorityGranted: false,
            ActionAuthorized: false,
            ModelBindingAllowed: false,
            ArbitraryEvaluationAllowed: false,
            RuntimeActionAllowed: false,
            ActivationAllowed: false,
            CmeActualActivationAllowed: false,
            SanctuaryActualActivationAllowed: false,
            ExitCode: 0,
            StandardOutput: "SAN-SLI-EC-TELEMETRY-LOOP-OK",
            StandardError: string.Empty,
            TimestampUtc: TimestampUtc);

    private static SliLispRuntimeLoadReceipt CreateLoadedRuntimeReceipt() =>
        new(
            ReceiptHandle: "sli-lisp-runtime-load://loaded",
            Disposition: SliLispRuntimeLoadDisposition.LoadedCold,
            OutcomeCode: "sli-lisp-resident-membrane-loaded-cold",
            RuntimeKind: "SBCL",
            RuntimePath: "sbcl",
            ModuleNames: ["core.lisp", "ec-telemetry-loop.lisp"],
            ModuleCount: 2,
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

    private sealed class FixedSliLispEcTelemetryLoopService : ISliLispEcTelemetryLoopService
    {
        private readonly SliLispEcTelemetryLoopReceipt receipt;

        public FixedSliLispEcTelemetryLoopService(SliLispEcTelemetryLoopReceipt receipt)
        {
            this.receipt = receipt;
        }

        public SliLispEcTelemetryLoopReceipt Run(
            SliLispEcTelemetryLoopRequest request,
            DateTimeOffset timestampUtc) =>
            receipt;
    }

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
            var fixture = new SanctuaryInstalledFixture(Path.Combine(Path.GetTempPath(), $"sanctuary-ec-loop-tests-{Guid.NewGuid():N}"));
            Directory.CreateDirectory(fixture.LineRootPath);
            File.WriteAllText(Path.Combine(fixture.LineRootPath, "San.sln"), string.Empty);
            return fixture;
        }

        public SanctuaryInstalledSubstrateRequest CreateInstallRequest() =>
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
