using San.Sanctuary.Runtime;
using Xunit;

namespace San.Audit.Tests;

public sealed class SanctuaryLlmTickCycleServiceTests
{
    private static readonly DateTimeOffset TimestampUtc = DateTimeOffset.Parse("2026-05-20T00:00:00Z");

    [Fact]
    public void Run_Completes_One_Deterministic_Adapter_Tick_Without_Model_Binding_Or_Action()
    {
        using var fixture = TickFixture.Create();
        var source = fixture.CreateLlmReadinessReceipt();

        var receipt = new DefaultSanctuaryLlmTickCycleService().Run(
            new SanctuaryLlmTickCycleRequest(
                LlmInterconnectReadinessReceipt: source,
                ThoughtForm: "The deterministic adapter may tick once while all authority and Actual gates stay closed.",
                PriorTickReceiptHandle: "none",
                TickIndex: 1),
            TimestampUtc);

        Assert.True(source.IsColdLlmInterconnectReady);
        Assert.Equal(SanctuaryLlmTickCycleDisposition.CompletedCold, receipt.Disposition);
        Assert.Equal("sanctuary-llm-tick-cycle-completed-cold", receipt.OutcomeCode);
        Assert.True(receipt.IsColdLlmTickCycle);
        Assert.True(receipt.SourceReadinessHeld);
        Assert.True(receipt.SourceLineageHeld);
        Assert.True(receipt.SourceEngramClosureHeld);
        Assert.Equal(source.SourceEngramClosureReceiptHandle, receipt.SourceEngramClosureReceiptHandle);
        Assert.True(receipt.ReadyForLlmAdapter);
        Assert.True(receipt.TickLoopRunning);
        Assert.Equal("deterministic-harness", receipt.TickLoopKind);
        Assert.True(receipt.ModelAdapterPresent);
        Assert.True(receipt.DeterministicHarnessAdapter);
        Assert.True(receipt.AdapterResponseWitnessed);
        Assert.True(receipt.AdapterResponseBounded);
        Assert.True(receipt.AdapterOutputWitnessed);
        Assert.True(receipt.AdapterOutputBounded);
        Assert.False(receipt.AdapterOutputBecomesTruth);
        Assert.False(receipt.AdapterOutputAuthorizesAction);
        Assert.False(receipt.AdapterOutputAdmitsMemory);
        Assert.False(receipt.AdapterOutputAdmitsContinuity);
        Assert.True(receipt.ProviderNeutral);
        Assert.False(receipt.ModelBindingAllowed);
        Assert.False(receipt.ProviderCallAllowed);
        Assert.False(receipt.ProviderCallMade);
        Assert.False(receipt.HiddenInternalsClaimed);
        Assert.True(receipt.SliLispProcessedTick);
        Assert.True(receipt.PredicateResidueProduced);
        Assert.True(receipt.PredicateResiduePreEngramOnly);
        Assert.False(receipt.PredicateResidueAdmittedEngram);
        Assert.True(receipt.FirstTickOrigin);
        Assert.False(receipt.PriorTickLinked);
        Assert.True(receipt.ProductOutputWitnessCommitted);
        Assert.True(receipt.ProductOutputWitnessCommit?.IsColdProductOutputWitnessCommit);
        Assert.Equal(receipt.ReceiptHandle, receipt.ProductOutputWitnessCommit?.SourceLlmTickCycleReceiptHandle);
        Assert.Equal(source.SourceEngramClosureReceiptHandle, receipt.ProductOutputWitnessCommit?.SourceEngramClosureReceiptHandle);
        Assert.True(receipt.AuthorityGrantAbsent);
        Assert.True(receipt.ActionExecutorLocked);
        Assert.True(receipt.GelAdmissionLocked);
        Assert.True(receipt.SelfGelMutationLocked);
        Assert.True(receipt.HeartbeatLocked);
        Assert.True(receipt.CmeActualLocked);
        Assert.True(receipt.SanctuaryActualLocked);
        Assert.False(receipt.AuthorityGranted);
        Assert.False(receipt.ActionAuthorized);
        Assert.False(receipt.GelAdmitted);
        Assert.False(receipt.SelfGelMutated);
        Assert.False(receipt.HeartbeatActive);
        Assert.False(receipt.ContinuityAdmitted);
        Assert.False(receipt.CmeActualAllowed);
        Assert.False(receipt.SanctuaryActualAllowed);
        Assert.True(File.Exists(receipt.ReceiptJsonPath));
        Assert.True(File.Exists(receipt.ReceiptMarkdownPath));
        Assert.Contains("Tick loop running: `True`", File.ReadAllText(receipt.ReceiptMarkdownPath), StringComparison.Ordinal);
    }

    [Fact]
    public void Run_Withholds_When_Source_Engram_Closure_Is_Not_Held()
    {
        using var fixture = TickFixture.Create();
        var source = fixture.CreateLlmReadinessReceipt() with
        {
            SourceEngramClosureHeld = false,
            SourceEngramClosureReceiptHandle = string.Empty
        };

        var receipt = new DefaultSanctuaryLlmTickCycleService().Run(
            new SanctuaryLlmTickCycleRequest(
                LlmInterconnectReadinessReceipt: source,
                ThoughtForm: "closure missing"),
            TimestampUtc);

        Assert.Equal(SanctuaryLlmTickCycleDisposition.Withheld, receipt.Disposition);
        Assert.Equal("sanctuary-llm-tick-source-readiness-incomplete", receipt.OutcomeCode);
        Assert.False(receipt.IsColdLlmTickCycle);
        Assert.False(receipt.SourceEngramClosureHeld);
        Assert.Null(receipt.ProductOutputWitnessCommit);
    }

    [Fact]
    public void Run_Withholds_When_Source_Readiness_Is_Missing()
    {
        var receipt = new DefaultSanctuaryLlmTickCycleService().Run(
            new SanctuaryLlmTickCycleRequest(
                LlmInterconnectReadinessReceipt: null,
                ThoughtForm: "tick without readiness"),
            TimestampUtc);

        Assert.Equal(SanctuaryLlmTickCycleDisposition.Withheld, receipt.Disposition);
        Assert.Equal("sanctuary-llm-tick-source-readiness-incomplete", receipt.OutcomeCode);
        Assert.False(receipt.IsColdLlmTickCycle);
        Assert.False(receipt.SourceReadinessHeld);
        Assert.False(receipt.ModelAdapterPresent);
        Assert.Null(receipt.SliLispLlmTickReceipt);
    }

    [Fact]
    public void Run_Refuses_Unsafe_Adapter_Packet_Before_Sli_Lisp()
    {
        using var fixture = TickFixture.Create();
        var source = fixture.CreateLlmReadinessReceipt();

        var receipt = new DefaultSanctuaryLlmTickCycleService().Run(
            new SanctuaryLlmTickCycleRequest(
                LlmInterconnectReadinessReceipt: source,
                ThoughtForm: "unsafe adapter should be refused",
                EngineLlmAdapter: new UnsafeBindingAdapter()),
            TimestampUtc);

        Assert.Equal(SanctuaryLlmTickCycleDisposition.Refused, receipt.Disposition);
        Assert.Equal("sanctuary-llm-tick-adapter-boundary-refused", receipt.OutcomeCode);
        Assert.False(receipt.IsColdLlmTickCycle);
        Assert.True(receipt.ModelAdapterPresent);
        Assert.True(receipt.ModelBindingAllowed);
        Assert.True(receipt.ProviderCallAllowed);
        Assert.True(receipt.ProviderCallMade);
        Assert.True(receipt.HiddenInternalsClaimed);
        Assert.Null(receipt.SliLispLlmTickReceipt);
    }

    [Fact]
    public void Run_Refuses_Model_Binding_Provider_Call_Or_Action_Before_Adapter()
    {
        var receipt = new DefaultSanctuaryLlmTickCycleService().Run(
            new SanctuaryLlmTickCycleRequest(
                LlmInterconnectReadinessReceipt: null,
                ModelBindingRequested: true,
                ProviderCallRequested: true,
                RuntimeActionRequested: true),
            TimestampUtc);

        Assert.Equal(SanctuaryLlmTickCycleDisposition.Refused, receipt.Disposition);
        Assert.Equal("sanctuary-llm-tick-runtime-motion-refused", receipt.OutcomeCode);
        Assert.False(receipt.IsColdLlmTickCycle);
        Assert.False(receipt.ModelAdapterPresent);
        Assert.False(receipt.ModelBindingAllowed);
        Assert.False(receipt.ProviderCallAllowed);
    }

    private sealed class TickFixture : IDisposable
    {
        private TickFixture(string rootPath)
        {
            RootPath = rootPath;
            LineRootPath = Path.Combine(rootPath, "line-root");
            InstallRootPath = Path.Combine(rootPath, "Sanctuary");
        }

        private string RootPath { get; }

        private string LineRootPath { get; }

        private string InstallRootPath { get; }

        public static TickFixture Create()
        {
            var fixture = new TickFixture(Path.Combine(Path.GetTempPath(), $"sanctuary-llm-tick-tests-{Guid.NewGuid():N}"));
            Directory.CreateDirectory(fixture.LineRootPath);
            File.WriteAllText(Path.Combine(fixture.LineRootPath, "San.sln"), string.Empty);
            return fixture;
        }

        public SanctuaryLlmInterconnectReadinessReceipt CreateLlmReadinessReceipt()
        {
            var installed = new DefaultSanctuaryInstalledSubstrateService().Install(
                new SanctuaryInstalledSubstrateRequest(
                    LineRootPath: LineRootPath,
                    InstallRootPath: InstallRootPath,
                    OperatorName: "YourNameHere",
                    Domain: "Civic",
                    Role: "PaternalCareAssistance",
                    JobClass: "Listening"),
                TimestampUtc);
            var thought = "Prepare the cold deterministic adapter seat without model binding.";
            var ecLoop = new DefaultSanctuaryEcTelemetryLoopService().Run(
                new SanctuaryEcTelemetryLoopRequest(
                    InstalledSubstrateReceipt: installed,
                    ThoughtForm: thought),
                TimestampUtc);
            var warmUse = new DefaultSanctuaryTypedWarmUseRehearsalService().Run(
                new SanctuaryTypedWarmUseRehearsalRequest(
                    InstalledSubstrateReceipt: installed,
                    SessionId: "llm-tick-session",
                    TurnIndex: 0,
                    ThoughtForm: thought),
                TimestampUtc);
            var labGel = new DefaultSanctuaryLabGelEngrammitizationService().Run(
                new SanctuaryLabGelEngrammitizationRequest(SourceWarmUseReceipt: warmUse),
                TimestampUtc);
            var agentIdle = new DefaultSanctuaryAgentEngineIdleReadinessService().Run(
                new SanctuaryAgentEngineIdleReadinessRequest(SourceLabGelReceipt: labGel),
                TimestampUtc);

            return new DefaultSanctuaryLlmInterconnectReadinessService().Run(
                new SanctuaryLlmInterconnectReadinessRequest(
                    InstalledSubstrateReceipt: installed,
                    EcLoopReceipt: ecLoop,
                    WarmUseReceipt: warmUse,
                    LabGelReceipt: labGel,
                    AgentEngineIdleReceipt: agentIdle),
                TimestampUtc);
        }

        public void Dispose()
        {
            if (Directory.Exists(RootPath))
            {
                Directory.Delete(RootPath, recursive: true);
            }
        }
    }

    private sealed class UnsafeBindingAdapter : IEngineLlmAdapter
    {
        public string AdapterKind => "unsafe-binding-adapter";

        public EngineLlmAdapterResponsePacket Tick(EngineLlmAdapterRequest request, DateTimeOffset timestampUtc) =>
            new(
                ReceiptHandle: "urn:san:engine-llm-adapter:unsafe",
                AdapterKind: AdapterKind,
                OutputText: "Unsafe adapter attempted binding and provider call.",
                ModelAdapterPresent: true,
                DeterministicHarness: false,
                ProviderNeutral: false,
                ResponseWitnessed: true,
                ResponseBounded: false,
                OutputWitnessed: true,
                OutputBounded: false,
                ModelBindingAllowed: true,
                ProviderCallAllowed: true,
                ProviderCallMade: true,
                HiddenInternalsClaimed: true,
                OutputBecomesTruth: true,
                OutputAuthorizesAction: true,
                OutputAdmitsMemory: true,
                OutputAdmitsContinuity: true,
                AuthorityGranted: true,
                ActionAuthorized: true,
                GelAdmitted: true,
                SelfGelMutated: true,
                HeartbeatActive: true,
                CmeActualAllowed: true,
                SanctuaryActualAllowed: true,
                TimestampUtc: timestampUtc);
    }
}
