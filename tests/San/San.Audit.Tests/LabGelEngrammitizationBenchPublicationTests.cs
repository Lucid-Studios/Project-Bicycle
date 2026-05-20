using System.Text.Json;
using Xunit;

namespace San.Audit.Tests;

public sealed class LabGelEngrammitizationBenchPublicationTests
{
    [Fact]
    public void Published_Bench_Result_Is_Pre_Admission_Lineaged_And_Boundary_Held()
    {
        var lineRoot = FindLineRoot();
        var benchPath = Path.Combine(lineRoot, "bench", "lab-gel-engrammitization", "lab-gel-engrammitization-bench.v0.6.0.json");
        var readmePath = Path.Combine(lineRoot, "bench", "lab-gel-engrammitization", "README.md");

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

        Assert.Equal("project-bicycle.lab-gel-engrammitization.bench.v1", root.GetProperty("schema").GetString());
        Assert.Equal("0.6.0", root.GetProperty("lineVersion").GetString());
        Assert.Equal("San.Launcher lab-gel", root.GetProperty("commandSurface").GetString());
        Assert.Equal(6, summary.GetProperty("completedTurns").GetInt32());
        Assert.Equal(6, summary.GetProperty("predicateTurns").GetInt32());
        Assert.True(summary.GetProperty("labGelLineageHeld").GetBoolean());
        Assert.True(summary.GetProperty("allTurnsRetainedAsLabSubstrate").GetBoolean());
        Assert.False(summary.GetProperty("admittedGel").GetBoolean());
        Assert.False(summary.GetProperty("admittedEngram").GetBoolean());
        Assert.False(summary.GetProperty("admittedMemory").GetBoolean());
        Assert.False(summary.GetProperty("mutatedSelfGel").GetBoolean());
        Assert.False(summary.GetProperty("admittedContinuity").GetBoolean());
        Assert.False(summary.GetProperty("grantedAuthority").GetBoolean());
        Assert.False(summary.GetProperty("authorizedAction").GetBoolean());
        Assert.False(summary.GetProperty("cmeActualAllowed").GetBoolean());
        Assert.False(summary.GetProperty("sanctuaryActualAllowed").GetBoolean());

        string? priorReceipt = null;
        foreach (var turn in root.GetProperty("turns").EnumerateArray())
        {
            Assert.Equal("CompletedCold", turn.GetProperty("disposition").GetString());
            Assert.Equal("sanctuary-lab-gel-engrammitization-completed-cold", turn.GetProperty("outcomeCode").GetString());
            Assert.Equal("sli.lisp", turn.GetProperty("engineOwner").GetString());
            Assert.Equal("run-lab-gel-engrammitization", turn.GetProperty("boundedEntrypoint").GetString());
            Assert.Equal("post-gel-formation-pre-admission", turn.GetProperty("labGelState").GetString());
            Assert.Equal(6, turn.GetProperty("predicateCount").GetInt32());
            Assert.True(turn.GetProperty("engramCandidateFormed").GetBoolean());
            Assert.True(turn.GetProperty("candidateRetainedAsLabSubstrate").GetBoolean());
            Assert.False(turn.GetProperty("labGelAdmitted").GetBoolean());
            Assert.False(turn.GetProperty("engramAdmitted").GetBoolean());
            Assert.False(turn.GetProperty("memoryAdmitted").GetBoolean());
            Assert.False(turn.GetProperty("selfGelMutated").GetBoolean());
            Assert.False(turn.GetProperty("continuityAdmitted").GetBoolean());
            Assert.False(turn.GetProperty("authorityGranted").GetBoolean());
            Assert.False(turn.GetProperty("actionAuthorized").GetBoolean());
            Assert.False(turn.GetProperty("cmeActualAllowed").GetBoolean());
            Assert.False(turn.GetProperty("sanctuaryActualAllowed").GetBoolean());

            if (turn.GetProperty("turnIndex").GetInt32() > 0)
            {
                Assert.Equal(priorReceipt, turn.GetProperty("priorLabGelReceiptHandle").GetString());
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
