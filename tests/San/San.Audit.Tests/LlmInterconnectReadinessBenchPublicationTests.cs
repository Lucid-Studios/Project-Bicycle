using System.Text.Json;
using Xunit;

namespace San.Audit.Tests;

public sealed class LlmInterconnectReadinessBenchPublicationTests
{
    [Fact]
    public void Published_Bench_Result_Is_Organ_Membrane_Ready_Without_Model_Binding()
    {
        var lineRoot = FindLineRoot();
        var benchPath = Path.Combine(lineRoot, "bench", "llm-interconnect-readiness", "llm-interconnect-readiness-bench.v0.8.0.json");
        var readmePath = Path.Combine(lineRoot, "bench", "llm-interconnect-readiness", "README.md");

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

        Assert.Equal("project-bicycle.llm-interconnect-readiness.bench.v1", root.GetProperty("schema").GetString());
        Assert.Equal("0.8.0", root.GetProperty("lineVersion").GetString());
        Assert.Equal("San.Launcher llm-ready", root.GetProperty("commandSurface").GetString());
        Assert.Equal(6, summary.GetProperty("completedTurns").GetInt32());
        Assert.Equal(6, summary.GetProperty("readyForAdapterTurns").GetInt32());
        Assert.Equal(11, summary.GetProperty("requiredOrganCount").GetInt32());
        Assert.True(summary.GetProperty("allOrgansPresent").GetBoolean());
        Assert.True(summary.GetProperty("allMembranesPresent").GetBoolean());
        Assert.True(summary.GetProperty("sourceLineageHeld").GetBoolean());
        Assert.True(summary.GetProperty("providerNeutral").GetBoolean());
        Assert.False(summary.GetProperty("modelAdapterPresent").GetBoolean());
        Assert.False(summary.GetProperty("modelBindingAllowed").GetBoolean());
        Assert.False(summary.GetProperty("providerCallAllowed").GetBoolean());
        Assert.False(summary.GetProperty("hiddenInternalsClaimed").GetBoolean());
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
            Assert.Equal("sanctuary-llm-interconnect-readiness-completed-cold", turn.GetProperty("outcomeCode").GetString());
            Assert.Equal("sli.lisp", turn.GetProperty("engineOwner").GetString());
            Assert.Equal("run-llm-interconnect-readiness", turn.GetProperty("boundedEntrypoint").GetString());
            Assert.Equal("cold-organ-membrane-ready", turn.GetProperty("interconnectState").GetString());
            Assert.True(turn.GetProperty("installedSubstrateReady").GetBoolean());
            Assert.True(turn.GetProperty("ecLoopReady").GetBoolean());
            Assert.True(turn.GetProperty("warmUseReady").GetBoolean());
            Assert.True(turn.GetProperty("labGelReady").GetBoolean());
            Assert.True(turn.GetProperty("agentEngineIdleReady").GetBoolean());
            Assert.True(turn.GetProperty("sourceLineageHeld").GetBoolean());
            Assert.Equal(11, turn.GetProperty("requiredOrganCount").GetInt32());
            Assert.True(turn.GetProperty("allRequiredOrgansPresent").GetBoolean());
            Assert.True(turn.GetProperty("sliLispLoaded").GetBoolean());
            Assert.True(turn.GetProperty("lispControlMatrixPresent").GetBoolean());
            Assert.True(turn.GetProperty("listeningFramePresent").GetBoolean());
            Assert.True(turn.GetProperty("compassPresent").GetBoolean());
            Assert.True(turn.GetProperty("soulFrameRoutePresent").GetBoolean());
            Assert.True(turn.GetProperty("agentiCoreRoutePresent").GetBoolean());
            Assert.True(turn.GetProperty("providerNeutral").GetBoolean());
            Assert.True(turn.GetProperty("readyForLlmAdapter").GetBoolean());
            Assert.False(turn.GetProperty("modelAdapterPresent").GetBoolean());
            Assert.False(turn.GetProperty("modelBindingAllowed").GetBoolean());
            Assert.False(turn.GetProperty("providerCallAllowed").GetBoolean());
            Assert.False(turn.GetProperty("hiddenInternalsClaimed").GetBoolean());
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
