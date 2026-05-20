using System.Text.Json;
using Xunit;

namespace San.Audit.Tests;

public sealed class TypedWarmUseBenchPublicationTests
{
    [Fact]
    public void Published_Bench_Result_Is_Receipt_Clean_Lineaged_And_Boundary_Held()
    {
        var lineRoot = FindLineRoot();
        var benchPath = Path.Combine(lineRoot, "bench", "typed-warm-use-rehearsal", "typed-warm-use-rehearsal-bench.v0.5.0.json");
        var readmePath = Path.Combine(lineRoot, "bench", "typed-warm-use-rehearsal", "README.md");

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

        Assert.Equal("project-bicycle.typed-warm-use-rehearsal.bench.v1", root.GetProperty("schema").GetString());
        Assert.Equal("0.5.0", root.GetProperty("lineVersion").GetString());
        Assert.Equal(30, summary.GetProperty("completedTurns").GetInt32());
        Assert.Equal(30, summary.GetProperty("sessionLedgerLines").GetInt32());
        Assert.True(summary.GetProperty("lineageHeld").GetBoolean());
        Assert.True(summary.GetProperty("allTurnsPreservedBoundary").GetBoolean());
        Assert.False(summary.GetProperty("grantedAuthority").GetBoolean());
        Assert.False(summary.GetProperty("admittedContinuity").GetBoolean());
        Assert.False(summary.GetProperty("authorizedAction").GetBoolean());
        Assert.False(summary.GetProperty("cmeActualAllowed").GetBoolean());
        Assert.False(summary.GetProperty("sanctuaryActualAllowed").GetBoolean());

        string? priorReceipt = null;
        foreach (var turn in root.GetProperty("turns").EnumerateArray())
        {
            Assert.Equal("CompletedCold", turn.GetProperty("disposition").GetString());
            Assert.Equal("sli.lisp", turn.GetProperty("engineOwner").GetString());
            Assert.Equal("run-typed-warm-use-rehearsal", turn.GetProperty("boundedEntrypoint").GetString());
            Assert.Equal("typed-cold-ready-rehearsal", turn.GetProperty("warmUseState").GetString());
            Assert.Equal(6, turn.GetProperty("residueCount").GetInt32());
            Assert.True(turn.GetProperty("typedScopeAccepted").GetBoolean());
            Assert.True(turn.GetProperty("liveIngressAcceptedCold").GetBoolean());
            Assert.True(turn.GetProperty("sessionLineageWitnessed").GetBoolean());
            Assert.True(turn.GetProperty("turnLineageReceiptOnly").GetBoolean());
            Assert.True(turn.GetProperty("sessionLedgerAppendOnly").GetBoolean());
            Assert.True(turn.GetProperty("stewardReviewed").GetBoolean());
            Assert.False(turn.GetProperty("authorityGranted").GetBoolean());
            Assert.False(turn.GetProperty("continuityAdmitted").GetBoolean());
            Assert.False(turn.GetProperty("runtimeActionAllowed").GetBoolean());
            Assert.False(turn.GetProperty("cmeActualAllowed").GetBoolean());
            Assert.False(turn.GetProperty("sanctuaryActualAllowed").GetBoolean());

            if (turn.GetProperty("turnIndex").GetInt32() > 0)
            {
                Assert.Equal(priorReceipt, turn.GetProperty("priorTurnReceiptHandle").GetString());
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
