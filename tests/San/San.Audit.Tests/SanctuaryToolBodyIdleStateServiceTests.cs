using San.Sanctuary.Runtime;
using Xunit;

namespace San.Audit.Tests;

public sealed class SanctuaryToolBodyIdleStateServiceTests
{
    private static readonly DateTimeOffset TimestampUtc = DateTimeOffset.Parse("2026-05-20T00:00:00Z");

    [Fact]
    public void Run_Holds_Tool_Body_Idle_Without_Llm_Maintenance()
    {
        using var fixture = ToolIdleFixture.Create();
        var installed = new DefaultSanctuaryInstalledSubstrateService().Install(fixture.CreateInstallRequest(), TimestampUtc);
        var ecLoop = new DefaultSanctuaryEcTelemetryLoopService().Run(
            new SanctuaryEcTelemetryLoopRequest(
                InstalledSubstrateReceipt: installed,
                ThoughtForm: "The Sanctuary body can idle without model maintenance."),
            TimestampUtc);
        var warmUse = new DefaultSanctuaryTypedWarmUseRehearsalService().Run(
            new SanctuaryTypedWarmUseRehearsalRequest(
                InstalledSubstrateReceipt: installed,
                SessionId: "tool-body-idle-session",
                TurnIndex: 1,
                ThoughtForm: "The Sanctuary body can idle without model maintenance."),
            TimestampUtc);
        var labGel = new DefaultSanctuaryLabGelEngrammitizationService().Run(
            new SanctuaryLabGelEngrammitizationRequest(SourceWarmUseReceipt: warmUse),
            TimestampUtc);

        var receipt = new DefaultSanctuaryToolBodyIdleStateService().Run(
            new SanctuaryToolBodyIdleStateRequest(
                InstalledSubstrateReceipt: installed,
                EcLoopReceipt: ecLoop,
                WarmUseReceipt: warmUse,
                LabGelReceipt: labGel,
                PriorToolBodyIdleReceiptHandle: "urn:san:tool-body-idle-state:prior"),
            TimestampUtc);

        Assert.True(installed.IsColdInstalledSubstrate);
        Assert.True(ecLoop.IsColdEcTelemetryLoop);
        Assert.True(warmUse.IsTypedColdReadyWarmUse);
        Assert.True(labGel.IsColdPreAdmissionLabGel);
        Assert.Equal(SanctuaryToolBodyIdleStateDisposition.CompletedCold, receipt.Disposition);
        Assert.Equal("sanctuary-tool-body-idle-state-completed-cold", receipt.OutcomeCode);
        Assert.True(receipt.IsColdToolBodyIdleState);
        Assert.Equal(installed.ReceiptHandle, receipt.SourceInstalledSubstrateReceiptHandle);
        Assert.Equal(ecLoop.ReceiptHandle, receipt.SourceEcLoopReceiptHandle);
        Assert.Equal(warmUse.ReceiptHandle, receipt.SourceWarmUseReceiptHandle);
        Assert.Equal(labGel.ReceiptHandle, receipt.SourceLabGelReceiptHandle);
        Assert.Equal(labGel.EngramClosure?.ClosureReceiptHandle, receipt.SourceEngramClosureReceiptHandle);
        Assert.Equal(labGel.ReadbackReceipt?.ReadbackReceiptHandle, receipt.SourceLabGelReadbackReceiptHandle);
        Assert.Equal("urn:san:tool-body-idle-state:prior", receipt.PriorToolBodyIdleReceiptHandle);
        Assert.True(receipt.SourceLineageHeld);
        Assert.True(receipt.SourceEngramClosureHeld);
        Assert.True(receipt.SourceLabGelReadbackHeld);
        Assert.Equal(SanctuaryInstalledSubstrateReceipt.ExpectedInstalledBodyCount, receipt.RequiredOrganCount);
        Assert.True(receipt.AllRequiredOrgansPresent);
        Assert.True(receipt.BaseBodiesPresent);
        Assert.True(receipt.CondensateBodiesPresent);
        Assert.True(receipt.RoleBodiesPresent);
        Assert.True(receipt.GoverningCmeCSharpBodiesBuilt);
        Assert.True(receipt.GoverningCmeActualizedCold);
        Assert.True(receipt.PrimeGoverningCmeBuilt);
        Assert.True(receipt.CrypticGoverningCmeBuilt);
        Assert.True(receipt.StewardGoverningCmeBuilt);
        Assert.True(receipt.GoverningCmeSliLispActualizationSurfacesReady);
        Assert.True(receipt.GoverningCmeMaintainsIdleState);
        Assert.True(receipt.GoverningHeartbeatHealthy);
        Assert.True(receipt.BondedCmeCallAvailable);
        Assert.True(receipt.SanctuaryGovernanceMonitoringReady);
        Assert.True(receipt.SliLispLoaded);
        Assert.True(receipt.LispControlMatrixPresent);
        Assert.True(receipt.ListeningFramePresent);
        Assert.True(receipt.CompassPresent);
        Assert.True(receipt.SoulFrameRoutePresent);
        Assert.True(receipt.AgentiCoreRoutePresent);
        Assert.Equal("cold-sanctuary-maintained-idle", receipt.IdleState);
        Assert.True(receipt.MaintainedBySanctuary);
        Assert.False(receipt.MaintainedByLlm);
        Assert.False(receipt.LlmMaintenanceRequired);
        Assert.False(receipt.LlmAdapterRequired);
        Assert.True(receipt.ReadyForLlmAdapter);
        Assert.True(receipt.CanAcceptFutureRider);
        Assert.True(receipt.GovernanceSlmCandidateDesirable);
        Assert.True(receipt.GovernanceSlmRoutingSwitchCandidate);
        Assert.True(receipt.GovernanceSlmIntelligentSwitchCandidate);
        Assert.False(receipt.GovernanceSlmPresent);
        Assert.False(receipt.GovernanceSlmRequiredForIdle);
        Assert.True(receipt.GovernanceSlmMayDiscriminateEscalation);
        Assert.True(receipt.GovernanceSlmMayDiscernActionReadiness);
        Assert.False(receipt.GovernanceSlmDiscernmentAuthorizesAction);
        Assert.False(receipt.GovernanceSlmMayAuthorizeAction);
        Assert.False(receipt.ModelAdapterPresent);
        Assert.False(receipt.ModelBindingAllowed);
        Assert.False(receipt.ProviderCallAllowed);
        Assert.False(receipt.TickLoopRunning);
        Assert.False(receipt.TickMaintainedByLlm);
        Assert.True(receipt.IdleLoopHeld);
        Assert.True(receipt.ReturnToPrimeHeld);
        Assert.True(receipt.OperatorReentryAvailable);
        Assert.True(receipt.EcMaintainedInLisp);
        Assert.True(receipt.LocalEcHoldAvailable);
        Assert.False(receipt.EngineCallRequired);
        Assert.False(receipt.LlmEngineCallRequired);
        Assert.False(receipt.ExternalEngineCallRequired);
        Assert.False(receipt.AgentEngineIdleRequired);
        Assert.True(receipt.AuthorityGrantAbsent);
        Assert.True(receipt.ActionExecutorLocked);
        Assert.True(receipt.GelAdmissionLocked);
        Assert.True(receipt.SelfGelMutationLocked);
        Assert.True(receipt.HeartbeatLocked);
        Assert.True(receipt.CmeActualLocked);
        Assert.True(receipt.SanctuaryActualLocked);
        Assert.False(receipt.AuthorityGranted);
        Assert.False(receipt.ActionAuthorized);
        Assert.False(receipt.HeartbeatActive);
        Assert.False(receipt.CmeActualAllowed);
        Assert.False(receipt.SanctuaryActualAllowed);
        Assert.True(File.Exists(receipt.ReceiptJsonPath));
        Assert.True(File.Exists(receipt.ReceiptMarkdownPath));
        Assert.True(File.Exists(receipt.SessionLedgerPath));
        Assert.Contains("Maintained by LLM: `False`", File.ReadAllText(receipt.ReceiptMarkdownPath), StringComparison.Ordinal);
        Assert.Contains(receipt.ReceiptHandle, File.ReadAllText(receipt.SessionLedgerPath), StringComparison.Ordinal);
    }

    [Fact]
    public void Run_Withholds_When_Source_Chain_Is_Missing()
    {
        var receipt = new DefaultSanctuaryToolBodyIdleStateService().Run(
            new SanctuaryToolBodyIdleStateRequest(
                InstalledSubstrateReceipt: null,
                EcLoopReceipt: null,
                WarmUseReceipt: null,
                LabGelReceipt: null),
            TimestampUtc);

        Assert.Equal(SanctuaryToolBodyIdleStateDisposition.Withheld, receipt.Disposition);
        Assert.Equal("sanctuary-tool-body-idle-source-chain-incomplete", receipt.OutcomeCode);
        Assert.False(receipt.IsColdToolBodyIdleState);
        Assert.Null(receipt.SliLispToolBodyIdleReceipt);
        Assert.False(receipt.MaintainedBySanctuary);
        Assert.False(receipt.MaintainedByLlm);
        Assert.False(receipt.ModelBindingAllowed);
    }

    [Fact]
    public void Run_Refuses_Llm_Maintenance_Tick_Model_Binding_Or_Action_Before_Sli_Lisp()
    {
        var receipt = new DefaultSanctuaryToolBodyIdleStateService().Run(
            new SanctuaryToolBodyIdleStateRequest(
                InstalledSubstrateReceipt: null,
                EcLoopReceipt: null,
                WarmUseReceipt: null,
                LabGelReceipt: null,
                LlmMaintenanceRequested: true,
                TickLoopRequested: true,
                ModelBindingRequested: true,
                RuntimeActionRequested: true),
            TimestampUtc);

        Assert.Equal(SanctuaryToolBodyIdleStateDisposition.Refused, receipt.Disposition);
        Assert.Equal("sanctuary-tool-body-idle-runtime-motion-refused", receipt.OutcomeCode);
        Assert.False(receipt.IsColdToolBodyIdleState);
        Assert.Null(receipt.SliLispToolBodyIdleReceipt);
        Assert.False(receipt.ModelBindingAllowed);
        Assert.False(receipt.ProviderCallAllowed);
        Assert.False(receipt.RuntimeActionAllowed);
    }

    private sealed class ToolIdleFixture : IDisposable
    {
        private ToolIdleFixture(string rootPath)
        {
            RootPath = rootPath;
            LineRootPath = Path.Combine(rootPath, "line-root");
            InstallRootPath = Path.Combine(rootPath, "Sanctuary");
        }

        private string RootPath { get; }

        private string LineRootPath { get; }

        private string InstallRootPath { get; }

        public static ToolIdleFixture Create()
        {
            var fixture = new ToolIdleFixture(Path.Combine(Path.GetTempPath(), $"sanctuary-tool-idle-tests-{Guid.NewGuid():N}"));
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
