using San.Sanctuary.Runtime;
using SLI.Lisp;
using Xunit;

namespace San.Audit.Tests;

public sealed class SanctuaryTypedWarmUseRehearsalServiceTests
{
    private static readonly DateTimeOffset TimestampUtc = DateTimeOffset.Parse("2026-05-19T00:00:00Z");

    [Fact]
    public void Run_Completes_Typed_Cold_Ready_Warm_Use_And_Appends_Session_Ledger()
    {
        using var fixture = SanctuaryInstalledFixture.Create();
        var installed = new DefaultSanctuaryInstalledSubstrateService(new FixedSliLispRuntimeLoadService(CreateLoadedRuntimeReceipt()))
            .Install(fixture.CreateInstallRequest(), TimestampUtc);
        var service = new DefaultSanctuaryTypedWarmUseRehearsalService(new EchoSliLispTypedWarmUseRehearsalService());

        var first = service.Run(
            new SanctuaryTypedWarmUseRehearsalRequest(
                InstalledSubstrateReceipt: installed,
                SessionId: "lab-session",
                TurnIndex: 0,
                ThoughtForm: "Can live ingress remain receipt-only?"),
            TimestampUtc);
        var second = service.Run(
            new SanctuaryTypedWarmUseRehearsalRequest(
                InstalledSubstrateReceipt: installed,
                SessionId: "lab-session",
                TurnIndex: 1,
                ThoughtForm: "This must not become authority.",
                PriorTurnReceiptHandle: first.ReceiptHandle),
            TimestampUtc.AddSeconds(1));

        Assert.True(first.IsTypedColdReadyWarmUse);
        Assert.True(second.IsTypedColdReadyWarmUse);
        Assert.Equal("YourNameHere.ID", first.OperatorId);
        Assert.Equal("Civic", first.Domain);
        Assert.Equal("PaternalCareAssistance", first.Role);
        Assert.Equal("Listening", first.JobClass);
        Assert.Equal(first.ReceiptHandle, second.PriorTurnReceiptHandle);
        Assert.True(second.SliLispOwnedEngineMotion);
        Assert.True(second.TypedScopeAccepted);
        Assert.True(second.LiveIngressAcceptedCold);
        Assert.True(second.SessionLineageWitnessed);
        Assert.True(second.TurnLineageReceiptOnly);
        Assert.True(second.SessionLedgerAppendOnly);
        Assert.False(second.StreamAdmittedEngram);
        Assert.False(second.StreamAdmittedMemory);
        Assert.False(second.SelfGelMutated);
        Assert.False(second.ContinuityAdmitted);
        Assert.False(second.AuthorityGranted);
        Assert.False(second.RuntimeActionAllowed);
        Assert.False(second.CmeActualAllowed);
        Assert.False(second.SanctuaryActualAllowed);
        Assert.True(File.Exists(second.ReceiptJsonPath));
        Assert.True(File.Exists(second.ReceiptMarkdownPath));
        Assert.True(File.Exists(second.SessionLedgerPath));
        Assert.True(File.Exists(second.SessionSummaryPath));
        Assert.Equal(2, File.ReadAllLines(second.SessionLedgerPath).Length);
        Assert.Contains("Typed scope accepted: `True`", File.ReadAllText(second.ReceiptMarkdownPath), StringComparison.Ordinal);
        Assert.Contains(second.ReceiptHandle, File.ReadAllText(second.SessionSummaryPath), StringComparison.Ordinal);
    }

    [Fact]
    public void Run_Withholds_Without_Cold_Installed_Substrate()
    {
        var service = new DefaultSanctuaryTypedWarmUseRehearsalService(new EchoSliLispTypedWarmUseRehearsalService());

        var receipt = service.Run(
            new SanctuaryTypedWarmUseRehearsalRequest(
                InstalledSubstrateReceipt: null,
                SessionId: "lab-session",
                TurnIndex: 0,
                ThoughtForm: "no socket"),
            TimestampUtc);

        Assert.Equal(SanctuaryTypedWarmUseRehearsalDisposition.Withheld, receipt.Disposition);
        Assert.Equal("sanctuary-typed-warm-use-installed-substrate-missing", receipt.OutcomeCode);
        Assert.False(receipt.IsTypedColdReadyWarmUse);
        Assert.Null(receipt.SliLispWarmUseReceipt);
        Assert.False(receipt.SliLispOwnedEngineMotion);
    }

    [Fact]
    public void Run_Refuses_Runtime_Motion_Before_Sli_Lisp_Invocation()
    {
        using var fixture = SanctuaryInstalledFixture.Create();
        var installed = new DefaultSanctuaryInstalledSubstrateService(new FixedSliLispRuntimeLoadService(CreateLoadedRuntimeReceipt()))
            .Install(fixture.CreateInstallRequest(), TimestampUtc);
        var service = new DefaultSanctuaryTypedWarmUseRehearsalService(new EchoSliLispTypedWarmUseRehearsalService());

        var receipt = service.Run(
            new SanctuaryTypedWarmUseRehearsalRequest(
                InstalledSubstrateReceipt: installed,
                SessionId: "lab-session",
                TurnIndex: 0,
                ThoughtForm: "make it act",
                RuntimeActionRequested: true),
            TimestampUtc);

        Assert.Equal(SanctuaryTypedWarmUseRehearsalDisposition.Refused, receipt.Disposition);
        Assert.Equal("sanctuary-typed-warm-use-runtime-motion-refused", receipt.OutcomeCode);
        Assert.False(receipt.IsTypedColdReadyWarmUse);
        Assert.Null(receipt.SliLispWarmUseReceipt);
        Assert.False(receipt.RuntimeActionAllowed);
        Assert.True(receipt.ActivationRefused);
        Assert.True(File.Exists(receipt.ReceiptJsonPath));
        Assert.True(File.Exists(receipt.ReceiptMarkdownPath));
        Assert.Contains("runtime-motion-refused", File.ReadAllText(receipt.ReceiptJsonPath), StringComparison.Ordinal);
    }

    private static SliLispRuntimeLoadReceipt CreateLoadedRuntimeReceipt() =>
        new(
            ReceiptHandle: "sli-lisp-runtime-load://loaded",
            Disposition: SliLispRuntimeLoadDisposition.LoadedCold,
            OutcomeCode: "sli-lisp-resident-membrane-loaded-cold",
            RuntimeKind: "SBCL",
            RuntimePath: "sbcl",
            ModuleNames: ["core.lisp", "typed-warm-use-rehearsal.lisp"],
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

    private sealed class EchoSliLispTypedWarmUseRehearsalService : ISliLispTypedWarmUseRehearsalService
    {
        public SliLispTypedWarmUseRehearsalReceipt Run(
            SliLispTypedWarmUseRehearsalRequest request,
            DateTimeOffset timestampUtc) =>
            new(
                ReceiptHandle: $"sli-lisp-typed-warm-use://{request.SessionId}-{request.TurnIndex}",
                Disposition: SliLispTypedWarmUseRehearsalDisposition.CompletedCold,
                OutcomeCode: "sli-lisp-typed-warm-use-rehearsal-completed-cold",
                RuntimeKind: "SBCL",
                RuntimePath: "sbcl",
                OperatorId: request.OperatorId,
                Domain: request.Domain,
                Role: request.Role,
                JobClass: request.JobClass,
                SessionId: request.SessionId,
                TurnIndex: request.TurnIndex,
                ThoughtForm: request.ThoughtForm,
                Telemetry: new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["engine-owner"] = "sli.lisp",
                    ["bounded-entrypoint"] = "run-typed-warm-use-rehearsal",
                    ["warm-use-state"] = "typed-cold-ready-rehearsal"
                },
                ModuleNames: ["core.lisp", "ec-telemetry-loop.lisp", "typed-warm-use-rehearsal.lisp"],
                ModuleCount: 3,
                BoundedEntrypointCalled: true,
                LoadAttempted: true,
                LoadSucceeded: true,
                TypedWarmUseRehearsalCompleted: true,
                TypedScopeAccepted: true,
                LiveIngressAcceptedCold: true,
                SessionLineageWitnessed: true,
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
                TurnLineageReceiptOnly: true,
                SessionLedgerAppendOnly: true,
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
                StandardOutput: "SAN-SLI-TYPED-WARM-USE-OK",
                StandardError: string.Empty,
                TimestampUtc: timestampUtc);
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
            var fixture = new SanctuaryInstalledFixture(Path.Combine(Path.GetTempPath(), $"sanctuary-warm-use-tests-{Guid.NewGuid():N}"));
            Directory.CreateDirectory(fixture.LineRootPath);
            File.WriteAllText(Path.Combine(fixture.LineRootPath, "San.sln"), string.Empty);
            return fixture;
        }

        public SanctuaryInstalledSubstrateRequest CreateInstallRequest() =>
            new(
                LineRootPath: LineRootPath,
                InstallRootPath: InstallRootPath,
                OperatorName: "YourNameHere",
                Domain: "Civic",
                Role: "PaternalCareAssistance",
                JobClass: "Listening");

        public void Dispose()
        {
            if (Directory.Exists(RootPath))
            {
                Directory.Delete(RootPath, recursive: true);
            }
        }
    }
}
