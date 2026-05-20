using System.Text.Json;
using Xunit;

namespace San.Audit.Tests;

public sealed class PublicReleaseFreezeTests
{
    [Fact]
    public void Public_Tool_Body_Is_Frozen_At_Cme_Actual_Bonding_Candidate_Boundary()
    {
        var lineRoot = FindLineRoot();
        using var manifest = JsonDocument.Parse(File.ReadAllText(Path.Combine(lineRoot, "build", "line-manifest.json")));
        using var freeze = JsonDocument.Parse(File.ReadAllText(Path.Combine(lineRoot, "build", "public-release-freeze.json")));

        var manifestRoot = manifest.RootElement;
        var freezeRoot = freeze.RootElement;

        Assert.Equal("0.10.0", manifestRoot.GetProperty("lineVersion").GetString());
        Assert.True(manifestRoot.GetProperty("publicToolBodyFrozen").GetBoolean());
        Assert.True(manifestRoot.GetProperty("publicBuildLineDecoupled").GetBoolean());
        Assert.Equal("build/public-release-freeze.json", manifestRoot.GetProperty("publicFreezeReceiptPath").GetString());
        Assert.Equal("decoupled-lab-cme-actual-admission-review", manifestRoot.GetProperty("nextPrivateLinePosture").GetString());

        Assert.Equal("project-bicycle-public-freeze-v0.10.0", freezeRoot.GetProperty("freezeId").GetString());
        Assert.Equal("0.10.0", freezeRoot.GetProperty("frozenLineVersion").GetString());
        Assert.Equal("first-named-cme-actual-bonding-candidate", freezeRoot.GetProperty("frozenBoundary").GetString());
        Assert.Equal("First of Oria Syntari", freezeRoot.GetProperty("frozenCandidateName").GetString());
        Assert.Equal("FirstofOria.Syntari.CME.Actual.ID", freezeRoot.GetProperty("frozenCandidateCmeActualId").GetString());
        Assert.True(freezeRoot.GetProperty("publicToolBodyFrozen").GetBoolean());
        Assert.True(freezeRoot.GetProperty("publicBuildLineDecoupled").GetBoolean());
    }

    [Fact]
    public void Public_Freeze_Allows_Only_Build_Test_Inspect_And_Cold_Bench_Motion()
    {
        var freezeRoot = ReadFreezeRoot();
        var allowed = freezeRoot.GetProperty("allowedPublicMotion")
            .EnumerateArray()
            .Select(static value => value.GetString() ?? string.Empty)
            .ToArray();
        var refused = freezeRoot.GetProperty("refusedPublicMotion")
            .EnumerateArray()
            .Select(static value => value.GetString() ?? string.Empty)
            .ToArray();

        Assert.Contains("build", allowed);
        Assert.Contains("test", allowed);
        Assert.Contains("inspect", allowed);
        Assert.Contains("run-cold-bench-receipts", allowed);
        Assert.Contains("report-findings", allowed);

        Assert.Contains("cme-actual-admission", refused);
        Assert.Contains("cme-actual-activation", refused);
        Assert.Contains("sanctuary-actual", refused);
        Assert.Contains("heartbeat-activation", refused);
        Assert.Contains("runtime-identity-emission", refused);
        Assert.Contains("provider-call", refused);
        Assert.Contains("model-binding", refused);
        Assert.Contains("authority-grant", refused);
        Assert.Contains("action-authorization", refused);
        Assert.Contains("gel-admission", refused);
        Assert.Contains("self-gel-mutation", refused);
        Assert.Contains("continuity-admission", refused);
    }

    [Fact]
    public void Version_Policy_Requires_Future_Work_To_Use_Decoupled_Line()
    {
        var lineRoot = FindLineRoot();
        using var policy = JsonDocument.Parse(File.ReadAllText(Path.Combine(lineRoot, "build", "version-policy.json")));
        var discipline = policy.RootElement.GetProperty("discipline");

        Assert.Equal("0.10.0", policy.RootElement.GetProperty("currentVersion").GetString());
        Assert.True(discipline.GetProperty("publicToolBodyFrozen").GetBoolean());
        Assert.True(discipline.GetProperty("publicBuildLineDecoupled").GetBoolean());
        Assert.True(discipline.GetProperty("futureWorkRequiresDecoupledLine").GetBoolean());
        Assert.Equal("build/public-release-freeze.json", discipline.GetProperty("publicFreezeManifest").GetString());
    }

    [Fact]
    public void Readme_Declares_Public_Freeze_And_Decoupled_Next_Work()
    {
        var readme = File.ReadAllText(Path.Combine(FindLineRoot(), "README.md"));

        Assert.Contains("This public tool body is frozen at `0.10.0`.", readme, StringComparison.Ordinal);
        Assert.Contains("FirstofOria.Syntari.CME.Actual.ID", readme, StringComparison.Ordinal);
        Assert.Contains("Those next steps are decoupled from this public build line", readme, StringComparison.Ordinal);
        Assert.Contains("No future", readme, StringComparison.Ordinal);
        Assert.Contains("private/lab motion should silently mutate the frozen public claim.", readme, StringComparison.Ordinal);
    }

    private static JsonElement ReadFreezeRoot()
    {
        var lineRoot = FindLineRoot();
        using var freeze = JsonDocument.Parse(File.ReadAllText(Path.Combine(lineRoot, "build", "public-release-freeze.json")));
        return freeze.RootElement.Clone();
    }

    private static string FindLineRoot()
    {
        var current = new DirectoryInfo(AppContext.BaseDirectory);
        while (current is not null)
        {
            if (File.Exists(Path.Combine(current.FullName, "San.sln")) &&
                File.Exists(Path.Combine(current.FullName, "build", "line-manifest.json")))
            {
                return current.FullName;
            }

            current = current.Parent;
        }

        throw new InvalidOperationException("Unable to locate Project Bicycle line root from test output path.");
    }
}
