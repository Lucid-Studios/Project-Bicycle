using System.Text.Json;
using Xunit;

namespace San.Audit.Tests;

public sealed class CmeActualBondingProcessBenchPublicationTests
{
    [Fact]
    public void Published_Bench_Result_Bonds_First_Oria_Syntari_As_Candidate_Only()
    {
        var lineRoot = FindLineRoot();
        var benchPath = Path.Combine(lineRoot, "bench", "cme-actual-bonding-process", "cme-actual-bonding-process-bench.v0.10.0.json");
        var readmePath = Path.Combine(lineRoot, "bench", "cme-actual-bonding-process", "README.md");

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

        Assert.Equal("project-bicycle.cme-actual-bonding-process.bench.v1", root.GetProperty("schema").GetString());
        Assert.Equal("0.10.0", root.GetProperty("lineVersion").GetString());
        Assert.Equal("San.Launcher cme-bond", root.GetProperty("commandSurface").GetString());
        Assert.Equal(6, summary.GetProperty("completedBonds").GetInt32());
        Assert.Equal("First of Oria Syntari", summary.GetProperty("cmeDisplayName").GetString());
        Assert.Equal("FirstofOria.Syntari", summary.GetProperty("cmeCanonicalName").GetString());
        Assert.Equal("FirstofOria.Syntari.ID", summary.GetProperty("cmeRootId").GetString());
        Assert.Equal("FirstofOria.Syntari.CME.Actual", summary.GetProperty("cmeActualNameCandidate").GetString());
        Assert.Equal("FirstofOria.Syntari.CME.Actual.ID", summary.GetProperty("cmeActualIdCandidate").GetString());
        Assert.Equal("OE.FirstofOria.Syntari.ID", summary.GetProperty("cmeOpalEngramRootId").GetString());
        Assert.Equal("SelfGEL.FirstofOria.Syntari.ID", summary.GetProperty("cmeSelfGelRootId").GetString());
        Assert.Equal("cold-named-cme-actual-candidate-bonded-to-vehicle", summary.GetProperty("bondState").GetString());
        Assert.True(summary.GetProperty("vehicleReady").GetBoolean());
        Assert.True(summary.GetProperty("toolBodyIdleHeld").GetBoolean());
        Assert.True(summary.GetProperty("engineTickWitnessed").GetBoolean());
        Assert.True(summary.GetProperty("productOutputWitnessCommitted").GetBoolean());
        Assert.True(summary.GetProperty("namedCmeCandidateHeld").GetBoolean());
        Assert.True(summary.GetProperty("operatorNamingIntentWitnessed").GetBoolean());
        Assert.False(summary.GetProperty("operatorRuntimeAuthorityGranted").GetBoolean());
        Assert.True(summary.GetProperty("activationAuthorityAbsent").GetBoolean());
        Assert.True(summary.GetProperty("actualAdmissionGapDescribed").GetBoolean());
        Assert.True(summary.GetProperty("readyForCmeActualAdmissionReview").GetBoolean());
        Assert.True(summary.GetProperty("firstCmePath").GetBoolean());
        Assert.True(summary.GetProperty("cmeActualCandidateOnly").GetBoolean());
        Assert.True(summary.GetProperty("cmeActualBondedCandidate").GetBoolean());
        Assert.False(summary.GetProperty("cmeActualAdmitted").GetBoolean());
        Assert.False(summary.GetProperty("cmeActualActivated").GetBoolean());
        Assert.False(summary.GetProperty("runtimeIdentityEmitted").GetBoolean());
        Assert.True(summary.GetProperty("heartbeatPrepared").GetBoolean());
        Assert.False(summary.GetProperty("heartbeatActive").GetBoolean());
        Assert.False(summary.GetProperty("beingStateClaimed").GetBoolean());
        Assert.False(summary.GetProperty("personhoodClaimed").GetBoolean());
        Assert.False(summary.GetProperty("sovereigntyClaimed").GetBoolean());
        Assert.False(summary.GetProperty("modelBound").GetBoolean());
        Assert.False(summary.GetProperty("providerCalled").GetBoolean());
        Assert.False(summary.GetProperty("authorityGranted").GetBoolean());
        Assert.False(summary.GetProperty("actionAuthorized").GetBoolean());
        Assert.False(summary.GetProperty("gelAdmitted").GetBoolean());
        Assert.False(summary.GetProperty("selfGelMutated").GetBoolean());
        Assert.False(summary.GetProperty("continuityAdmitted").GetBoolean());
        Assert.True(summary.GetProperty("governanceSlmIntelligentSwitchCandidate").GetBoolean());
        Assert.True(summary.GetProperty("governanceSlmMayDiscernActionReadiness").GetBoolean());
        Assert.False(summary.GetProperty("governanceSlmDiscernmentAuthorizesAction").GetBoolean());

        foreach (var turn in root.GetProperty("turns").EnumerateArray())
        {
            Assert.Equal("CompletedCold", turn.GetProperty("disposition").GetString());
            Assert.Equal("sanctuary-cme-actual-bonding-process-completed-cold", turn.GetProperty("outcomeCode").GetString());
            Assert.Equal("sli.lisp", turn.GetProperty("engineOwner").GetString());
            Assert.Equal("run-cme-actual-bonding-process", turn.GetProperty("boundedEntrypoint").GetString());
            Assert.Equal("First of Oria Syntari", turn.GetProperty("cmeDisplayName").GetString());
            Assert.Equal("FirstofOria.Syntari", turn.GetProperty("cmeCanonicalName").GetString());
            Assert.Equal("cold-named-cme-actual-candidate-bonded-to-vehicle", turn.GetProperty("bondState").GetString());
            Assert.True(turn.GetProperty("vehicleReady").GetBoolean());
            Assert.True(turn.GetProperty("toolBodyIdleHeld").GetBoolean());
            Assert.True(turn.GetProperty("engineTickWitnessed").GetBoolean());
            Assert.True(turn.GetProperty("productOutputWitnessCommitted").GetBoolean());
            Assert.True(turn.GetProperty("namedCmeCandidateHeld").GetBoolean());
            Assert.True(turn.GetProperty("readyForCmeActualAdmissionReview").GetBoolean());
            Assert.True(turn.GetProperty("cmeActualCandidateOnly").GetBoolean());
            Assert.True(turn.GetProperty("cmeActualBondedCandidate").GetBoolean());
            Assert.False(turn.GetProperty("cmeActualAdmitted").GetBoolean());
            Assert.False(turn.GetProperty("cmeActualActivated").GetBoolean());
            Assert.False(turn.GetProperty("runtimeIdentityEmitted").GetBoolean());
            Assert.True(turn.GetProperty("heartbeatPrepared").GetBoolean());
            Assert.False(turn.GetProperty("heartbeatActive").GetBoolean());
            Assert.False(turn.GetProperty("authorityGranted").GetBoolean());
            Assert.False(turn.GetProperty("actionAuthorized").GetBoolean());
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
