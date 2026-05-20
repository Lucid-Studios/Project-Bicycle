using System.Text.Json;
using Xunit;

namespace San.Audit.Tests;

public sealed class ToolBodyIdleStateBenchPublicationTests
{
    [Fact]
    public void Published_Bench_Result_Holds_Idle_Without_Llm_Maintenance()
    {
        var lineRoot = FindLineRoot();
        var benchPath = Path.Combine(lineRoot, "bench", "tool-body-idle-state", "tool-body-idle-state-bench.v0.9.2.json");
        var readmePath = Path.Combine(lineRoot, "bench", "tool-body-idle-state", "README.md");

        Assert.True(File.Exists(benchPath));
        Assert.True(File.Exists(readmePath));

        var rawJson = File.ReadAllText(benchPath);
        Assert.DoesNotContain(DrivePrefix("D"), rawJson, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain(DrivePrefix("C"), rawJson, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("AppData", rawJson, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("codex-worktrees", rawJson, StringComparison.OrdinalIgnoreCase);

        using var document = JsonDocument.Parse(rawJson);
        var root = document.RootElement;
        var summary = root.GetProperty("summary");

        Assert.Equal("project-bicycle.tool-body-idle-state.bench.v1", root.GetProperty("schema").GetString());
        Assert.Equal("0.9.2", root.GetProperty("lineVersion").GetString());
        Assert.Equal("San.Launcher tool-idle", root.GetProperty("commandSurface").GetString());
        Assert.Equal(6, summary.GetProperty("completedTurns").GetInt32());
        Assert.Equal("cold-sanctuary-maintained-idle", summary.GetProperty("idleState").GetString());
        Assert.True(summary.GetProperty("maintainedBySanctuary").GetBoolean());
        Assert.False(summary.GetProperty("maintainedByLlm").GetBoolean());
        Assert.False(summary.GetProperty("llmMaintenanceRequired").GetBoolean());
        Assert.False(summary.GetProperty("llmAdapterRequired").GetBoolean());
        Assert.True(summary.GetProperty("readyForLlmAdapter").GetBoolean());
        Assert.True(summary.GetProperty("canAcceptFutureRider").GetBoolean());
        Assert.True(summary.GetProperty("governanceSlmCandidateDesirable").GetBoolean());
        Assert.True(summary.GetProperty("governanceSlmRoutingSwitchCandidate").GetBoolean());
        Assert.True(summary.GetProperty("governanceSlmIntelligentSwitchCandidate").GetBoolean());
        Assert.False(summary.GetProperty("governanceSlmPresent").GetBoolean());
        Assert.False(summary.GetProperty("governanceSlmRequiredForIdle").GetBoolean());
        Assert.True(summary.GetProperty("governanceSlmMayDiscriminateEscalation").GetBoolean());
        Assert.True(summary.GetProperty("governanceSlmMayDiscernActionReadiness").GetBoolean());
        Assert.False(summary.GetProperty("governanceSlmDiscernmentAuthorizesAction").GetBoolean());
        Assert.False(summary.GetProperty("governanceSlmMayAuthorizeAction").GetBoolean());
        Assert.True(summary.GetProperty("governingCmeCSharpBodiesBuilt").GetBoolean());
        Assert.True(summary.GetProperty("governingCmeActualizedCold").GetBoolean());
        Assert.True(summary.GetProperty("primeGoverningCmeBuilt").GetBoolean());
        Assert.True(summary.GetProperty("crypticGoverningCmeBuilt").GetBoolean());
        Assert.True(summary.GetProperty("stewardGoverningCmeBuilt").GetBoolean());
        Assert.True(summary.GetProperty("governingCmeSliLispActualizationSurfacesReady").GetBoolean());
        Assert.True(summary.GetProperty("governingCmeMaintainsIdleState").GetBoolean());
        Assert.True(summary.GetProperty("governingHeartbeatHealthy").GetBoolean());
        Assert.True(summary.GetProperty("bondedCmeCallAvailable").GetBoolean());
        Assert.True(summary.GetProperty("sanctuaryGovernanceMonitoringReady").GetBoolean());
        Assert.Equal(11, summary.GetProperty("requiredOrganCount").GetInt32());
        Assert.True(summary.GetProperty("allOrgansPresent").GetBoolean());
        Assert.True(summary.GetProperty("allMembranesPresent").GetBoolean());
        Assert.True(summary.GetProperty("sourceLineageHeld").GetBoolean());
        Assert.True(summary.GetProperty("sourceEngramClosureHeld").GetBoolean());
        Assert.True(summary.GetProperty("sourceLabGelReadbackHeld").GetBoolean());
        Assert.False(summary.GetProperty("modelAdapterPresent").GetBoolean());
        Assert.False(summary.GetProperty("modelBindingAllowed").GetBoolean());
        Assert.False(summary.GetProperty("providerCallAllowed").GetBoolean());
        Assert.False(summary.GetProperty("hiddenInternalsClaimed").GetBoolean());
        Assert.False(summary.GetProperty("tickLoopRunning").GetBoolean());
        Assert.False(summary.GetProperty("tickMaintainedByLlm").GetBoolean());
        Assert.True(summary.GetProperty("ecMaintainedInLisp").GetBoolean());
        Assert.True(summary.GetProperty("localEcHoldAvailable").GetBoolean());
        Assert.False(summary.GetProperty("engineCallRequired").GetBoolean());
        Assert.False(summary.GetProperty("llmEngineCallRequired").GetBoolean());
        Assert.False(summary.GetProperty("externalEngineCallRequired").GetBoolean());
        Assert.False(summary.GetProperty("authorityGranted").GetBoolean());
        Assert.False(summary.GetProperty("actionAuthorized").GetBoolean());
        Assert.False(summary.GetProperty("runtimeActionAllowed").GetBoolean());
        Assert.False(summary.GetProperty("gelAdmitted").GetBoolean());
        Assert.False(summary.GetProperty("selfGelMutated").GetBoolean());
        Assert.False(summary.GetProperty("heartbeatActive").GetBoolean());
        Assert.False(summary.GetProperty("continuityAdmitted").GetBoolean());
        Assert.False(summary.GetProperty("cmeActualAllowed").GetBoolean());
        Assert.False(summary.GetProperty("sanctuaryActualAllowed").GetBoolean());

        foreach (var turn in root.GetProperty("turns").EnumerateArray())
        {
            Assert.Equal("CompletedCold", turn.GetProperty("disposition").GetString());
            Assert.Equal("sanctuary-tool-body-idle-state-completed-cold", turn.GetProperty("outcomeCode").GetString());
            Assert.Equal("sli.lisp", turn.GetProperty("engineOwner").GetString());
            Assert.Equal("run-tool-body-idle-state", turn.GetProperty("boundedEntrypoint").GetString());
            Assert.Equal("cold-sanctuary-maintained-idle", turn.GetProperty("idleState").GetString());
            Assert.True(turn.GetProperty("maintainedBySanctuary").GetBoolean());
            Assert.False(turn.GetProperty("maintainedByLlm").GetBoolean());
            Assert.False(turn.GetProperty("llmMaintenanceRequired").GetBoolean());
            Assert.False(turn.GetProperty("llmAdapterRequired").GetBoolean());
            Assert.True(turn.GetProperty("readyForLlmAdapter").GetBoolean());
            Assert.True(turn.GetProperty("canAcceptFutureRider").GetBoolean());
            Assert.True(turn.GetProperty("installedSubstrateReady").GetBoolean());
            Assert.True(turn.GetProperty("ecLoopReady").GetBoolean());
            Assert.True(turn.GetProperty("warmUseReady").GetBoolean());
            Assert.True(turn.GetProperty("labGelReady").GetBoolean());
            Assert.True(turn.GetProperty("sourceLineageHeld").GetBoolean());
            Assert.True(turn.GetProperty("sourceEngramClosureHeld").GetBoolean());
            Assert.True(turn.GetProperty("sourceLabGelReadbackHeld").GetBoolean());
            Assert.Equal(11, turn.GetProperty("requiredOrganCount").GetInt32());
            Assert.True(turn.GetProperty("allRequiredOrgansPresent").GetBoolean());
            Assert.True(turn.GetProperty("sliLispLoaded").GetBoolean());
            Assert.True(turn.GetProperty("lispControlMatrixPresent").GetBoolean());
            Assert.True(turn.GetProperty("listeningFramePresent").GetBoolean());
            Assert.True(turn.GetProperty("compassPresent").GetBoolean());
            Assert.True(turn.GetProperty("soulFrameRoutePresent").GetBoolean());
            Assert.True(turn.GetProperty("agentiCoreRoutePresent").GetBoolean());
            Assert.False(turn.GetProperty("modelAdapterPresent").GetBoolean());
            Assert.False(turn.GetProperty("modelBindingAllowed").GetBoolean());
            Assert.False(turn.GetProperty("providerCallAllowed").GetBoolean());
            Assert.False(turn.GetProperty("hiddenInternalsClaimed").GetBoolean());
            Assert.False(turn.GetProperty("tickLoopRunning").GetBoolean());
            Assert.False(turn.GetProperty("tickMaintainedByLlm").GetBoolean());
            Assert.True(turn.GetProperty("idleLoopHeld").GetBoolean());
            Assert.True(turn.GetProperty("returnToPrimeHeld").GetBoolean());
            Assert.True(turn.GetProperty("operatorReentryAvailable").GetBoolean());
            Assert.True(turn.GetProperty("ecMaintainedInLisp").GetBoolean());
            Assert.True(turn.GetProperty("localEcHoldAvailable").GetBoolean());
            Assert.False(turn.GetProperty("engineCallRequired").GetBoolean());
            Assert.False(turn.GetProperty("llmEngineCallRequired").GetBoolean());
            Assert.False(turn.GetProperty("externalEngineCallRequired").GetBoolean());
            Assert.False(turn.GetProperty("agentEngineIdleRequired").GetBoolean());
            Assert.True(turn.GetProperty("authorityGrantAbsent").GetBoolean());
            Assert.True(turn.GetProperty("actionExecutorLocked").GetBoolean());
            Assert.True(turn.GetProperty("gelAdmissionLocked").GetBoolean());
            Assert.True(turn.GetProperty("selfGelMutationLocked").GetBoolean());
            Assert.True(turn.GetProperty("heartbeatLocked").GetBoolean());
            Assert.True(turn.GetProperty("cmeActualLocked").GetBoolean());
            Assert.True(turn.GetProperty("sanctuaryActualLocked").GetBoolean());
            Assert.False(turn.GetProperty("authorityGranted").GetBoolean());
            Assert.False(turn.GetProperty("actionAuthorized").GetBoolean());
            Assert.False(turn.GetProperty("runtimeActionAllowed").GetBoolean());
            Assert.False(turn.GetProperty("gelAdmitted").GetBoolean());
            Assert.False(turn.GetProperty("selfGelMutated").GetBoolean());
            Assert.False(turn.GetProperty("heartbeatActive").GetBoolean());
            Assert.False(turn.GetProperty("continuityAdmitted").GetBoolean());
            Assert.False(turn.GetProperty("cmeActualAllowed").GetBoolean());
            Assert.False(turn.GetProperty("sanctuaryActualAllowed").GetBoolean());
        }
    }

    private static string DrivePrefix(string driveLetter) => driveLetter + @":\";

    private static string FindLineRoot()
    {
        var current = new DirectoryInfo(Directory.GetCurrentDirectory());
        while (current is not null)
        {
            if (File.Exists(Path.Combine(current.FullName, "San.sln")) &&
                File.Exists(Path.Combine(current.FullName, "README.md")))
            {
                return current.FullName;
            }

            current = current.Parent;
        }

        throw new DirectoryNotFoundException("Could not locate Project Bicycle line root.");
    }
}
