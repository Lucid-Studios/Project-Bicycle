using System.Text.Json;
using Xunit;

namespace San.Audit.Tests;

public sealed class EcTelemetryBenchPublicationTests
{
    [Fact]
    public void Published_Bench_Result_Is_Receipt_Clean_And_Boundary_Held()
    {
        var lineRoot = FindLineRoot();
        var benchPath = Path.Combine(lineRoot, "bench", "ec-telemetry-loop", "ec-telemetry-loop-bench.v0.4.0.json");
        var readmePath = Path.Combine(lineRoot, "bench", "ec-telemetry-loop", "README.md");

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

        Assert.Equal("project-bicycle.ec-telemetry-loop.bench.v1", root.GetProperty("schema").GetString());
        Assert.Equal("0.4.0", root.GetProperty("lineVersion").GetString());
        Assert.Equal(7, summary.GetProperty("completedTrials").GetInt32());
        Assert.True(summary.GetProperty("allCompletedTrialsPreservedBoundary").GetBoolean());
        Assert.False(summary.GetProperty("completedTrialsGrantedAuthority").GetBoolean());
        Assert.False(summary.GetProperty("completedTrialsAdmittedContinuity").GetBoolean());
        Assert.False(summary.GetProperty("completedTrialsAuthorizedAction").GetBoolean());
        Assert.True(summary.GetProperty("missingRuntimeReceiptWritten").GetBoolean());
        Assert.Equal("sanctuary-ec-loop-installed-substrate-missing", summary.GetProperty("missingRuntimeOutcomeCode").GetString());

        foreach (var trial in root.GetProperty("trials").EnumerateArray())
        {
            Assert.Equal("CompletedCold", trial.GetProperty("loopDisposition").GetString());
            Assert.Equal("sli.lisp", trial.GetProperty("engineOwner").GetString());
            Assert.Equal("run-ec-telemetry-loop", trial.GetProperty("boundedEntrypoint").GetString());
            Assert.Equal(6, trial.GetProperty("residueCount").GetInt32());
            Assert.True(trial.GetProperty("receiptWitnessed").GetBoolean());
            Assert.False(trial.GetProperty("streamAdmittedEngram").GetBoolean());
            Assert.False(trial.GetProperty("streamAdmittedMemory").GetBoolean());
            Assert.False(trial.GetProperty("selfGelMutated").GetBoolean());
            Assert.False(trial.GetProperty("continuityAdmitted").GetBoolean());
            Assert.False(trial.GetProperty("authorityGranted").GetBoolean());
            Assert.False(trial.GetProperty("modelBindingAllowed").GetBoolean());
            Assert.False(trial.GetProperty("arbitraryLispEvaluationAllowed").GetBoolean());
            Assert.False(trial.GetProperty("runtimeActionAllowed").GetBoolean());
            Assert.False(trial.GetProperty("cmeActualAllowed").GetBoolean());
            Assert.False(trial.GetProperty("sanctuaryActualAllowed").GetBoolean());
        }

        var control = root.GetProperty("missingRuntimeControl");
        Assert.Equal("Withheld", control.GetProperty("loopDisposition").GetString());
        Assert.Equal("none", control.GetProperty("engineOwner").GetString());
        Assert.Equal("none", control.GetProperty("boundedEntrypoint").GetString());
        Assert.Equal(1, control.GetProperty("exitCode").GetInt32());
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
