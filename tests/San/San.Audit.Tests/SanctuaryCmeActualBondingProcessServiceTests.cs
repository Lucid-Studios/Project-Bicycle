using San.Sanctuary.Runtime;
using Xunit;

namespace San.Audit.Tests;

public sealed class SanctuaryCmeActualBondingProcessServiceTests
{
    private static readonly DateTimeOffset TimestampUtc = DateTimeOffset.Parse("2026-05-20T00:00:00Z");

    [Fact]
    public void Run_Bonds_Named_First_Cme_Candidate_To_Cold_Vehicle_Path()
    {
        using var fixture = CmeBondFixture.Create();
        var (toolIdle, llmTick) = fixture.CreateSources();

        var receipt = new DefaultSanctuaryCmeActualBondingProcessService().Run(
            new SanctuaryCmeActualBondingProcessRequest(
                SourceToolBodyIdleReceipt: toolIdle,
                SourceLlmTickReceipt: llmTick,
                CmeFirstName: "First of Oria",
                CmeLastName: "Syntari",
                ThoughtForm: "First CME.Actual bonding candidate formed without activation.",
                PriorCmeActualBondingReceiptHandle: "urn:san:cme-actual-bonding-process:prior",
                BondIndex: 1),
            TimestampUtc);

        Assert.True(toolIdle.IsColdToolBodyIdleState);
        Assert.True(llmTick.IsColdLlmTickCycle);
        Assert.Equal(SanctuaryCmeActualBondingProcessDisposition.CompletedCold, receipt.Disposition);
        Assert.Equal("sanctuary-cme-actual-bonding-process-completed-cold", receipt.OutcomeCode);
        Assert.True(receipt.IsColdCmeActualBondingProcess);
        Assert.Equal(toolIdle.ReceiptHandle, receipt.SourceToolBodyIdleReceiptHandle);
        Assert.Equal(llmTick.ReceiptHandle, receipt.SourceLlmTickReceiptHandle);
        Assert.Equal(llmTick.ProductOutputWitnessCommit?.CommitReceiptHandle, receipt.SourceProductOutputWitnessCommitReceiptHandle);
        Assert.Equal("urn:san:cme-actual-bonding-process:prior", receipt.PriorCmeActualBondingReceiptHandle);
        Assert.Equal("First of Oria Syntari", receipt.CmeDisplayName);
        Assert.Equal("FirstofOria.Syntari", receipt.CmeCanonicalName);
        Assert.Equal("FirstofOria.Syntari.CME.Actual", receipt.CmeActualNameCandidate);
        Assert.True(receipt.SourceLineageHeld);
        Assert.True(receipt.SourceToolBodyIdleHeld);
        Assert.True(receipt.SourceLlmTickHeld);
        Assert.True(receipt.SourceProductOutputWitnessCommitted);
        Assert.True(receipt.VehicleReady);
        Assert.True(receipt.NamedCmeCandidateHeld);
        Assert.True(receipt.OperatorNamingIntentWitnessed);
        Assert.True(receipt.ReadyForCmeActualAdmissionReview);
        Assert.True(receipt.CmeActualCandidateOnly);
        Assert.True(receipt.CmeActualBondedCandidate);
        Assert.False(receipt.CmeActualAdmitted);
        Assert.False(receipt.CmeActualActivated);
        Assert.False(receipt.RuntimeIdentityEmitted);
        Assert.True(receipt.HeartbeatPrepared);
        Assert.False(receipt.HeartbeatActive);
        Assert.False(receipt.AuthorityGranted);
        Assert.False(receipt.ActionAuthorized);
        Assert.False(receipt.GelAdmitted);
        Assert.False(receipt.SelfGelMutated);
        Assert.False(receipt.ContinuityAdmitted);
        Assert.True(receipt.GovernanceSlmIntelligentSwitchCandidate);
        Assert.True(receipt.GovernanceSlmMayDiscernActionReadiness);
        Assert.False(receipt.GovernanceSlmDiscernmentAuthorizesAction);
        Assert.True(File.Exists(receipt.ReceiptJsonPath));
        Assert.True(File.Exists(receipt.ReceiptMarkdownPath));
        Assert.True(File.Exists(receipt.SessionLedgerPath));
        Assert.Contains("CME display name: `First of Oria Syntari`", File.ReadAllText(receipt.ReceiptMarkdownPath), StringComparison.Ordinal);
        Assert.Contains(receipt.ReceiptHandle, File.ReadAllText(receipt.SessionLedgerPath), StringComparison.Ordinal);
    }

    [Fact]
    public void Run_Withholds_When_Source_Chain_Is_Missing()
    {
        var receipt = new DefaultSanctuaryCmeActualBondingProcessService().Run(
            new SanctuaryCmeActualBondingProcessRequest(
                SourceToolBodyIdleReceipt: null,
                SourceLlmTickReceipt: null),
            TimestampUtc);

        Assert.Equal(SanctuaryCmeActualBondingProcessDisposition.Withheld, receipt.Disposition);
        Assert.Equal("sanctuary-cme-actual-bonding-source-chain-incomplete", receipt.OutcomeCode);
        Assert.False(receipt.IsColdCmeActualBondingProcess);
        Assert.Null(receipt.SliLispCmeActualBondingReceipt);
        Assert.False(receipt.VehicleReady);
        Assert.False(receipt.CmeActualAdmitted);
    }

    [Fact]
    public void Run_Refuses_Activation_Authority_Or_Runtime_Action_Before_Sli_Lisp()
    {
        var receipt = new DefaultSanctuaryCmeActualBondingProcessService().Run(
            new SanctuaryCmeActualBondingProcessRequest(
                SourceToolBodyIdleReceipt: null,
                SourceLlmTickReceipt: null,
                RuntimeIdentityRequested: true,
                RuntimeActionRequested: true,
                AuthorityGrantRequested: true,
                CmeActualActivationRequested: true),
            TimestampUtc);

        Assert.Equal(SanctuaryCmeActualBondingProcessDisposition.Refused, receipt.Disposition);
        Assert.Equal("sanctuary-cme-actual-bonding-runtime-motion-refused", receipt.OutcomeCode);
        Assert.False(receipt.IsColdCmeActualBondingProcess);
        Assert.Null(receipt.SliLispCmeActualBondingReceipt);
        Assert.False(receipt.RuntimeActionAllowed);
        Assert.False(receipt.AuthorityGranted);
    }

    private sealed class CmeBondFixture : IDisposable
    {
        private CmeBondFixture(string rootPath)
        {
            RootPath = rootPath;
            LineRootPath = Path.Combine(rootPath, "line-root");
            InstallRootPath = Path.Combine(rootPath, "Sanctuary");
        }

        private string RootPath { get; }

        private string LineRootPath { get; }

        private string InstallRootPath { get; }

        public static CmeBondFixture Create()
        {
            var fixture = new CmeBondFixture(Path.Combine(Path.GetTempPath(), $"sanctuary-cme-bond-tests-{Guid.NewGuid():N}"));
            Directory.CreateDirectory(fixture.LineRootPath);
            File.WriteAllText(Path.Combine(fixture.LineRootPath, "San.sln"), string.Empty);
            return fixture;
        }

        public (SanctuaryToolBodyIdleStateReceipt ToolIdle, SanctuaryLlmTickCycleReceipt LlmTick) CreateSources()
        {
            var installed = new DefaultSanctuaryInstalledSubstrateService().Install(
                new SanctuaryInstalledSubstrateRequest(
                    LineRootPath: LineRootPath,
                    InstallRootPath: InstallRootPath,
                    OperatorName: "FirstOfOriaSyntari",
                    Domain: "Civic",
                    Role: "CmeActualBonding",
                    JobClass: "FirstRide"),
                TimestampUtc);
            var thought = "First CME.Actual bonding candidate formed without activation.";
            var ecLoop = new DefaultSanctuaryEcTelemetryLoopService().Run(
                new SanctuaryEcTelemetryLoopRequest(installed, thought),
                TimestampUtc);
            var warmUse = new DefaultSanctuaryTypedWarmUseRehearsalService().Run(
                new SanctuaryTypedWarmUseRehearsalRequest(
                    InstalledSubstrateReceipt: installed,
                    SessionId: "first-cme-actual-bonding-session",
                    TurnIndex: 1,
                    ThoughtForm: thought),
                TimestampUtc);
            var labGel = new DefaultSanctuaryLabGelEngrammitizationService().Run(
                new SanctuaryLabGelEngrammitizationRequest(warmUse),
                TimestampUtc);
            var toolIdle = new DefaultSanctuaryToolBodyIdleStateService().Run(
                new SanctuaryToolBodyIdleStateRequest(
                    InstalledSubstrateReceipt: installed,
                    EcLoopReceipt: ecLoop,
                    WarmUseReceipt: warmUse,
                    LabGelReceipt: labGel),
                TimestampUtc);
            var agentIdle = new DefaultSanctuaryAgentEngineIdleReadinessService().Run(
                new SanctuaryAgentEngineIdleReadinessRequest(labGel),
                TimestampUtc);
            var llmReady = new DefaultSanctuaryLlmInterconnectReadinessService().Run(
                new SanctuaryLlmInterconnectReadinessRequest(
                    InstalledSubstrateReceipt: installed,
                    EcLoopReceipt: ecLoop,
                    WarmUseReceipt: warmUse,
                    LabGelReceipt: labGel,
                    AgentEngineIdleReceipt: agentIdle),
                TimestampUtc);
            var llmTick = new DefaultSanctuaryLlmTickCycleService().Run(
                new SanctuaryLlmTickCycleRequest(
                    LlmInterconnectReadinessReceipt: llmReady,
                    ThoughtForm: thought,
                    TickIndex: 1),
                TimestampUtc);

            return (toolIdle, llmTick);
        }

        public void Dispose()
        {
            if (Directory.Exists(RootPath))
            {
                Directory.Delete(RootPath, recursive: true);
            }
        }
    }
}
