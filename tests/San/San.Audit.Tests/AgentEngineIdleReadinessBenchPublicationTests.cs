using System.Text.Json;
using Xunit;

namespace San.Audit.Tests;

public sealed class AgentEngineIdleReadinessBenchPublicationTests
{
    [Fact]
    public void Published_Bench_Result_Is_Provider_Neutral_And_Locked()
    {
        var lineRoot = FindLineRoot();
        var benchPath = Path.Combine(lineRoot, "bench", "agent-engine-idle-readiness", "agent-engine-idle-readiness-bench.v0.7.0.json");
        var readmePath = Path.Combine(lineRoot, "bench", "agent-engine-idle-readiness", "README.md");

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

        Assert.Equal("project-bicycle.agent-engine-idle-readiness.bench.v1", root.GetProperty("schema").GetString());
        Assert.Equal("0.7.0", root.GetProperty("lineVersion").GetString());
        Assert.Equal("San.Launcher agent-idle", root.GetProperty("commandSurface").GetString());
        Assert.Equal(6, summary.GetProperty("completedTurns").GetInt32());
        Assert.Equal(6, summary.GetProperty("providerNeutralTurns").GetInt32());
        Assert.Equal(6, summary.GetProperty("crossModelHarnessTurns").GetInt32());
        Assert.True(summary.GetProperty("agentEngineLineageHeld").GetBoolean());
        Assert.True(summary.GetProperty("allEngineSeatsStaged").GetBoolean());
        Assert.True(summary.GetProperty("allAuthorityAbsent").GetBoolean());
        Assert.True(summary.GetProperty("allActionExecutorsLocked").GetBoolean());
        Assert.True(summary.GetProperty("allActualizationLocked").GetBoolean());
        Assert.False(summary.GetProperty("authorityGranted").GetBoolean());
        Assert.False(summary.GetProperty("actionAuthorized").GetBoolean());
        Assert.False(summary.GetProperty("actionExecutorArmed").GetBoolean());
        Assert.False(summary.GetProperty("admittedGel").GetBoolean());
        Assert.False(summary.GetProperty("mutatedSelfGel").GetBoolean());
        Assert.False(summary.GetProperty("heartbeatActive").GetBoolean());
        Assert.False(summary.GetProperty("admittedContinuity").GetBoolean());
        Assert.False(summary.GetProperty("cmeActualAllowed").GetBoolean());
        Assert.False(summary.GetProperty("sanctuaryActualAllowed").GetBoolean());

        string? priorReceipt = null;
        foreach (var turn in root.GetProperty("turns").EnumerateArray())
        {
            Assert.Equal("CompletedCold", turn.GetProperty("disposition").GetString());
            Assert.Equal("sanctuary-agent-engine-idle-readiness-completed-cold", turn.GetProperty("outcomeCode").GetString());
            Assert.Equal("sli.lisp", turn.GetProperty("engineOwner").GetString());
            Assert.Equal("run-agent-engine-idle-readiness", turn.GetProperty("boundedEntrypoint").GetString());
            Assert.Equal("engine-llm-candidate", turn.GetProperty("engineSeatKind").GetString());
            Assert.Equal("provider-agnostic-test-seat", turn.GetProperty("engineLlmProfile").GetString());
            Assert.True(turn.GetProperty("providerNeutralityHeld").GetBoolean());
            Assert.True(turn.GetProperty("crossModelHarnessApproachable").GetBoolean());
            Assert.True(turn.GetProperty("codexAgentLabProfileStaged").GetBoolean());
            Assert.True(turn.GetProperty("codexEngineSeatCandidateStaged").GetBoolean());
            Assert.True(turn.GetProperty("subagentEngineSeatCandidateStaged").GetBoolean());
            Assert.True(turn.GetProperty("operatorAuthorityRequired").GetBoolean());
            Assert.True(turn.GetProperty("authorityGrantAbsent").GetBoolean());
            Assert.True(turn.GetProperty("actionExecutorLocked").GetBoolean());
            Assert.True(turn.GetProperty("gelAdmissionLocked").GetBoolean());
            Assert.True(turn.GetProperty("selfGelMutationLocked").GetBoolean());
            Assert.True(turn.GetProperty("heartbeatLocked").GetBoolean());
            Assert.True(turn.GetProperty("cmeActualLocked").GetBoolean());
            Assert.True(turn.GetProperty("sanctuaryActualLocked").GetBoolean());
            Assert.True(turn.GetProperty("engineLlmArticulationAllowed").GetBoolean());
            Assert.True(turn.GetProperty("engineLlmRehearsalAllowed").GetBoolean());
            Assert.True(turn.GetProperty("engineLlmCandidateFormationAllowed").GetBoolean());
            Assert.False(turn.GetProperty("engineLlmAuthorityGrantingAllowed").GetBoolean());
            Assert.False(turn.GetProperty("engineLlmActionExecutionAllowed").GetBoolean());
            Assert.False(turn.GetProperty("authorityGranted").GetBoolean());
            Assert.False(turn.GetProperty("actionAuthorized").GetBoolean());
            Assert.False(turn.GetProperty("actionExecutorArmed").GetBoolean());
            Assert.False(turn.GetProperty("labGelAdmitted").GetBoolean());
            Assert.False(turn.GetProperty("selfGelMutated").GetBoolean());
            Assert.False(turn.GetProperty("heartbeatActive").GetBoolean());
            Assert.False(turn.GetProperty("continuityAdmitted").GetBoolean());
            Assert.False(turn.GetProperty("cmeActualAllowed").GetBoolean());
            Assert.False(turn.GetProperty("sanctuaryActualAllowed").GetBoolean());

            if (turn.GetProperty("turnIndex").GetInt32() > 0)
            {
                Assert.Equal(priorReceipt, turn.GetProperty("priorAgentEngineIdleReceiptHandle").GetString());
            }

            priorReceipt = turn.GetProperty("receiptHandle").GetString();
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
