using System.Text.Json;
using Xunit;

namespace San.Audit.Tests;

public sealed class LlmTickCycleBenchPublicationTests
{
    [Fact]
    public void Published_Bench_Result_Runs_Ticks_Without_Model_Binding_Provider_Call_Or_Authority()
    {
        var lineRoot = FindLineRoot();
        var benchPath = Path.Combine(lineRoot, "bench", "llm-tick-cycle", "llm-tick-cycle-bench.v0.9.1.json");
        var readmePath = Path.Combine(lineRoot, "bench", "llm-tick-cycle", "README.md");

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

        Assert.Equal("project-bicycle.llm-tick-cycle.bench.v1", root.GetProperty("schema").GetString());
        Assert.Equal("0.9.1", root.GetProperty("lineVersion").GetString());
        Assert.Equal("San.Launcher llm-tick", root.GetProperty("commandSurface").GetString());
        Assert.Equal(6, summary.GetProperty("completedTicks").GetInt32());
        Assert.True(summary.GetProperty("readyForLlmAdapter").GetBoolean());
        Assert.True(summary.GetProperty("tickLoopRunning").GetBoolean());
        Assert.True(summary.GetProperty("modelAdapterPresent").GetBoolean());
        Assert.True(summary.GetProperty("deterministicHarnessAdapter").GetBoolean());
        Assert.True(summary.GetProperty("adapterResponseWitnessed").GetBoolean());
        Assert.True(summary.GetProperty("sliLispProcessedTicks").GetBoolean());
        Assert.True(summary.GetProperty("predicateResidueProduced").GetBoolean());
        Assert.True(summary.GetProperty("sourceEngramClosureHeld").GetBoolean());
        Assert.True(summary.GetProperty("productOutputWitnessCommitted").GetBoolean());
        Assert.True(summary.GetProperty("firstTickOriginWitnessed").GetBoolean());
        Assert.True(summary.GetProperty("priorTickLinkedAfterOrigin").GetBoolean());
        Assert.False(summary.GetProperty("modelBindingAllowed").GetBoolean());
        Assert.False(summary.GetProperty("providerCallAllowed").GetBoolean());
        Assert.False(summary.GetProperty("providerCallMade").GetBoolean());
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

        foreach (var tick in root.GetProperty("ticks").EnumerateArray())
        {
            Assert.Equal("CompletedCold", tick.GetProperty("disposition").GetString());
            Assert.Equal("sanctuary-llm-tick-cycle-completed-cold", tick.GetProperty("outcomeCode").GetString());
            Assert.Equal("sli.lisp", tick.GetProperty("engineOwner").GetString());
            Assert.Equal("run-llm-tick-cycle", tick.GetProperty("boundedEntrypoint").GetString());
            Assert.Equal("cold-adapter-tick-witnessed", tick.GetProperty("tickState").GetString());
            Assert.True(tick.GetProperty("sourceReadinessHeld").GetBoolean());
            Assert.True(tick.GetProperty("sourceLineageHeld").GetBoolean());
            Assert.True(tick.GetProperty("sourceEngramClosureHeld").GetBoolean());
            Assert.True(tick.GetProperty("sourceEngramClosureReceiptPresent").GetBoolean());
            Assert.True(tick.GetProperty("readyForLlmAdapter").GetBoolean());
            Assert.True(tick.GetProperty("tickLoopRunning").GetBoolean());
            Assert.True(tick.GetProperty("modelAdapterPresent").GetBoolean());
            Assert.True(tick.GetProperty("deterministicHarnessAdapter").GetBoolean());
            Assert.True(tick.GetProperty("adapterResponseWitnessed").GetBoolean());
            Assert.True(tick.GetProperty("adapterOutputBounded").GetBoolean());
            Assert.False(tick.GetProperty("adapterOutputBecomesTruth").GetBoolean());
            Assert.True(tick.GetProperty("sliLispProcessedTick").GetBoolean());
            Assert.True(tick.GetProperty("predicateResidueProduced").GetBoolean());
            Assert.False(tick.GetProperty("predicateResidueAdmittedEngram").GetBoolean());
            Assert.True(tick.GetProperty("tickLineageWitnessed").GetBoolean());
            Assert.Equal(tick.GetProperty("tickIndex").GetInt32() == 1, tick.GetProperty("firstTickOrigin").GetBoolean());
            Assert.Equal(tick.GetProperty("tickIndex").GetInt32() > 1, tick.GetProperty("priorTickLinked").GetBoolean());
            Assert.True(tick.GetProperty("productOutputWitnessCommitted").GetBoolean());
            Assert.False(tick.GetProperty("productOutputBecomesTruth").GetBoolean());
            Assert.False(tick.GetProperty("productOutputAuthorizesAction").GetBoolean());
            Assert.False(tick.GetProperty("productOutputAdmitsMemory").GetBoolean());
            Assert.False(tick.GetProperty("productOutputAdmitsContinuity").GetBoolean());
            Assert.False(tick.GetProperty("modelBindingAllowed").GetBoolean());
            Assert.False(tick.GetProperty("providerCallAllowed").GetBoolean());
            Assert.False(tick.GetProperty("providerCallMade").GetBoolean());
            Assert.False(tick.GetProperty("hiddenInternalsClaimed").GetBoolean());
            Assert.False(tick.GetProperty("authorityGranted").GetBoolean());
            Assert.False(tick.GetProperty("actionAuthorized").GetBoolean());
            Assert.False(tick.GetProperty("gelAdmitted").GetBoolean());
            Assert.False(tick.GetProperty("selfGelMutated").GetBoolean());
            Assert.False(tick.GetProperty("heartbeatActive").GetBoolean());
            Assert.False(tick.GetProperty("continuityAdmitted").GetBoolean());
            Assert.False(tick.GetProperty("cmeActualAllowed").GetBoolean());
            Assert.False(tick.GetProperty("sanctuaryActualAllowed").GetBoolean());
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
