using San.Sanctuary.Runtime;
using Xunit;

namespace San.Audit.Tests;

public sealed class SanctuaryLlmInterconnectReadinessServiceTests
{
    private static readonly DateTimeOffset TimestampUtc = DateTimeOffset.Parse("2026-05-20T00:00:00Z");

    [Fact]
    public void Run_Verifies_Cold_Organs_Membranes_And_Engine_Seat_For_Future_Llm_Adapter()
    {
        using var fixture = LlmReadyFixture.Create();
        var installed = new DefaultSanctuaryInstalledSubstrateService().Install(fixture.CreateInstallRequest(), TimestampUtc);
        var ecLoop = new DefaultSanctuaryEcTelemetryLoopService().Run(
            new SanctuaryEcTelemetryLoopRequest(
                InstalledSubstrateReceipt: installed,
                ThoughtForm: "The tool body should be ready for a future LLM adapter."),
            TimestampUtc);
        var warmUse = new DefaultSanctuaryTypedWarmUseRehearsalService().Run(
            new SanctuaryTypedWarmUseRehearsalRequest(
                InstalledSubstrateReceipt: installed,
                SessionId: "llm-ready-session",
                TurnIndex: 1,
                ThoughtForm: "The tool body should be ready for a future LLM adapter."),
            TimestampUtc);
        var labGel = new DefaultSanctuaryLabGelEngrammitizationService().Run(
            new SanctuaryLabGelEngrammitizationRequest(SourceWarmUseReceipt: warmUse),
            TimestampUtc);
        var agentIdle = new DefaultSanctuaryAgentEngineIdleReadinessService().Run(
            new SanctuaryAgentEngineIdleReadinessRequest(SourceLabGelReceipt: labGel),
            TimestampUtc);

        var receipt = new DefaultSanctuaryLlmInterconnectReadinessService().Run(
            new SanctuaryLlmInterconnectReadinessRequest(
                InstalledSubstrateReceipt: installed,
                EcLoopReceipt: ecLoop,
                WarmUseReceipt: warmUse,
                LabGelReceipt: labGel,
                AgentEngineIdleReceipt: agentIdle),
            TimestampUtc);

        Assert.True(installed.IsColdInstalledSubstrate);
        Assert.True(ecLoop.IsColdEcTelemetryLoop);
        Assert.True(warmUse.IsTypedColdReadyWarmUse);
        Assert.True(labGel.IsColdPreAdmissionLabGel);
        Assert.True(agentIdle.IsColdAgentEngineIdleReadiness);
        Assert.Equal(SanctuaryLlmInterconnectReadinessDisposition.CompletedCold, receipt.Disposition);
        Assert.Equal("sanctuary-llm-interconnect-readiness-completed-cold", receipt.OutcomeCode);
        Assert.True(receipt.IsColdLlmInterconnectReady);
        Assert.True(receipt.SourceLineageHeld);
        Assert.Equal(SanctuaryInstalledSubstrateReceipt.ExpectedInstalledBodyCount, receipt.RequiredOrganCount);
        Assert.True(receipt.AllRequiredOrgansPresent);
        Assert.True(receipt.BaseBodiesPresent);
        Assert.True(receipt.CondensateBodiesPresent);
        Assert.True(receipt.RoleBodiesPresent);
        Assert.True(receipt.SliLispLoaded);
        Assert.True(receipt.LispControlMatrixPresent);
        Assert.True(receipt.ListeningFramePresent);
        Assert.True(receipt.CompassPresent);
        Assert.True(receipt.SoulFrameRoutePresent);
        Assert.True(receipt.AgentiCoreRoutePresent);
        Assert.True(receipt.ProviderNeutral);
        Assert.True(receipt.ReadyForLlmAdapter);
        Assert.False(receipt.ModelAdapterPresent);
        Assert.False(receipt.ModelBindingAllowed);
        Assert.False(receipt.ProviderCallAllowed);
        Assert.False(receipt.HiddenInternalsClaimed);
        Assert.True(receipt.EngineLlmSeatReady);
        Assert.True(receipt.EngineLlmMayArticulate);
        Assert.True(receipt.EngineLlmMayRehearse);
        Assert.True(receipt.EngineLlmMayFormCandidates);
        Assert.False(receipt.EngineLlmMayBindModel);
        Assert.False(receipt.EngineLlmMayCallProvider);
        Assert.True(receipt.AuthorityGrantAbsent);
        Assert.True(receipt.ActionExecutorLocked);
        Assert.True(receipt.GelAdmissionLocked);
        Assert.True(receipt.SelfGelMutationLocked);
        Assert.True(receipt.HeartbeatLocked);
        Assert.True(receipt.CmeActualLocked);
        Assert.True(receipt.SanctuaryActualLocked);
        Assert.False(receipt.AuthorityGranted);
        Assert.False(receipt.ActionAuthorized);
        Assert.False(receipt.RuntimeActionAllowed);
        Assert.False(receipt.GelAdmitted);
        Assert.False(receipt.SelfGelMutated);
        Assert.False(receipt.HeartbeatActive);
        Assert.False(receipt.ContinuityAdmitted);
        Assert.False(receipt.CmeActualAllowed);
        Assert.False(receipt.SanctuaryActualAllowed);
        Assert.True(File.Exists(receipt.ReceiptJsonPath));
        Assert.True(File.Exists(receipt.ReceiptMarkdownPath));
        Assert.Contains("Ready for LLM adapter: `True`", File.ReadAllText(receipt.ReceiptMarkdownPath), StringComparison.Ordinal);
    }

    [Fact]
    public void Run_Withholds_When_Source_Chain_Is_Missing()
    {
        var receipt = new DefaultSanctuaryLlmInterconnectReadinessService().Run(
            new SanctuaryLlmInterconnectReadinessRequest(
                InstalledSubstrateReceipt: null,
                EcLoopReceipt: null,
                WarmUseReceipt: null,
                LabGelReceipt: null,
                AgentEngineIdleReceipt: null),
            TimestampUtc);

        Assert.Equal(SanctuaryLlmInterconnectReadinessDisposition.Withheld, receipt.Disposition);
        Assert.Equal("sanctuary-llm-interconnect-source-chain-incomplete", receipt.OutcomeCode);
        Assert.False(receipt.IsColdLlmInterconnectReady);
        Assert.Null(receipt.SliLispLlmInterconnectReceipt);
        Assert.False(receipt.ReadyForLlmAdapter);
        Assert.False(receipt.ModelBindingAllowed);
    }

    [Fact]
    public void Run_Refuses_Model_Binding_Provider_Call_Or_Action_Before_Sli_Lisp()
    {
        var receipt = new DefaultSanctuaryLlmInterconnectReadinessService().Run(
            new SanctuaryLlmInterconnectReadinessRequest(
                InstalledSubstrateReceipt: null,
                EcLoopReceipt: null,
                WarmUseReceipt: null,
                LabGelReceipt: null,
                AgentEngineIdleReceipt: null,
                ModelBindingRequested: true,
                ProviderCallRequested: true,
                RuntimeActionRequested: true),
            TimestampUtc);

        Assert.Equal(SanctuaryLlmInterconnectReadinessDisposition.Refused, receipt.Disposition);
        Assert.Equal("sanctuary-llm-interconnect-runtime-motion-refused", receipt.OutcomeCode);
        Assert.False(receipt.IsColdLlmInterconnectReady);
        Assert.Null(receipt.SliLispLlmInterconnectReceipt);
        Assert.False(receipt.ModelBindingAllowed);
        Assert.False(receipt.ProviderCallAllowed);
        Assert.False(receipt.RuntimeActionAllowed);
    }

    private sealed class LlmReadyFixture : IDisposable
    {
        private LlmReadyFixture(string rootPath)
        {
            RootPath = rootPath;
            LineRootPath = Path.Combine(rootPath, "line-root");
            InstallRootPath = Path.Combine(rootPath, "Sanctuary");
        }

        private string RootPath { get; }

        private string LineRootPath { get; }

        private string InstallRootPath { get; }

        public static LlmReadyFixture Create()
        {
            var fixture = new LlmReadyFixture(Path.Combine(Path.GetTempPath(), $"sanctuary-llm-ready-tests-{Guid.NewGuid():N}"));
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
