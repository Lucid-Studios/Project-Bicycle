using System.Text.RegularExpressions;
using Xunit;

namespace San.Audit.Tests;

public sealed class SanctuaryGelFirstFormationAttemptBoundaryDocsTests
{
    [Fact]
    public void First_Formation_Attempt_Note_Locks_Governing_Sentence_And_Ladder()
    {
        var repoRoot = GetRepoRoot();
        var lawPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "SANCTUARY_GEL_FIRST_FORMATION_ATTEMPT_BOUNDARY.md");
        var lawText = Normalize(File.ReadAllText(lawPath));

        Assert.Contains("First `Sanctuary.GEL` formation attempt gathers seated predicate priors, localized pre-certification inputs, represented National/Regional/Local standing, and regional substrate footing into one receipted formation attempt; it does not stand Sanctuary.Actual, admit survivor standing, grant first use, select models, activate runtime, or generate Cradle.GEL.", lawText, StringComparison.Ordinal);
        Assert.Contains("GEL readiness -> GEL predicate prior -> localized pre-certification data pool -> localized formation floor -> Sanctuary.GEL regional substrate -> pre-governing standing -> first-use eligibility consideration -> first Sanctuary.GEL formation attempt", lawText, StringComparison.Ordinal);
        Assert.Contains("An attempt is an assessment surface.", lawText, StringComparison.Ordinal);
        Assert.Contains("An attempt is not launch.", lawText, StringComparison.Ordinal);
        Assert.Contains("An attempt is not governance.", lawText, StringComparison.Ordinal);
        Assert.Contains("An attempt is not first use.", lawText, StringComparison.Ordinal);
    }

    [Fact]
    public void First_Formation_Attempt_Note_Names_Inputs_And_Non_Powers()
    {
        var repoRoot = GetRepoRoot();
        var lawPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "SANCTUARY_GEL_FIRST_FORMATION_ATTEMPT_BOUNDARY.md");
        var lawText = Normalize(File.ReadAllText(lawPath));

        Assert.Contains("ready `.GEL` predicate-prior records", lawText, StringComparison.Ordinal);
        Assert.Contains("localized pre-certification data pool records", lawText, StringComparison.Ordinal);
        Assert.Contains("represented National, Regional, and Local standing", lawText, StringComparison.Ordinal);
        Assert.Contains("`Sanctuary.GEL` regional substrate footing", lawText, StringComparison.Ordinal);
        Assert.Contains("Sanctuary pre-governing standing", lawText, StringComparison.Ordinal);
        Assert.Contains("first-use eligibility consideration", lawText, StringComparison.Ordinal);
        Assert.Contains("They do not become survivor admission, Sanctuary.Actual, Mother/Father governing `CME`, model selection, first-use admission, runtime, `RTME`, `SLI.Lisp` activation, or Cradle.GEL generation.", lawText, StringComparison.Ordinal);
    }

    [Fact]
    public void First_Formation_Attempt_Note_Allows_Passive_Evaluator_Only()
    {
        var repoRoot = GetRepoRoot();
        var lawPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "SANCTUARY_GEL_FIRST_FORMATION_ATTEMPT_BOUNDARY.md");
        var lawText = Normalize(File.ReadAllText(lawPath));

        Assert.Contains("accept passive prerequisite records", lawText, StringComparison.Ordinal);
        Assert.Contains("emit one receipted attempt record", lawText, StringComparison.Ordinal);
        Assert.Contains("persist state", lawText, StringComparison.Ordinal);
        Assert.Contains("execute runtime", lawText, StringComparison.Ordinal);
        Assert.Contains("activate `SLI.Lisp`", lawText, StringComparison.Ordinal);
        Assert.Contains("invoke `RTME`", lawText, StringComparison.Ordinal);
        Assert.Contains("select models", lawText, StringComparison.Ordinal);
        Assert.Contains("generate Cradle.GEL", lawText, StringComparison.Ordinal);
    }

    [Fact]
    public void Threaded_Docs_Preserve_Attempt_As_Non_Admission()
    {
        var repoRoot = GetRepoRoot();
        var docsRoot = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs");
        var readout = Normalize(File.ReadAllText(Path.Combine(docsRoot, "GEL_LINE_REGIONAL_SUBSTRATE_READOUT.md")));
        var regionalSubstrate = Normalize(File.ReadAllText(Path.Combine(docsRoot, "SANCTUARY_GEL_REGIONAL_SUBSTRATE_FORMATION_BOUNDARY.md")));
        var preGoverning = Normalize(File.ReadAllText(Path.Combine(docsRoot, "SANCTUARY_PRE_GOVERNING_STANDING_BOUNDARY.md")));
        var firstUse = Normalize(File.ReadAllText(Path.Combine(docsRoot, "FIRST_USE_ELIGIBILITY_BOUNDARY.md")));
        var readiness = Normalize(File.ReadAllText(Path.Combine(docsRoot, "BUILD_READINESS.md")));

        Assert.Contains("First `Sanctuary.GEL` formation attempt gathers the seated passive bodies into one receipted assessment.", readout, StringComparison.Ordinal);
        Assert.Contains("The `SANCTUARY_GEL_FIRST_FORMATION_ATTEMPT_BOUNDARY.md` note now defines the first receipted attempt above this substrate.", regionalSubstrate, StringComparison.Ordinal);
        Assert.Contains("The first `Sanctuary.GEL` formation attempt boundary may gather this pre-governing standing posture into a receipted attempt only.", preGoverning, StringComparison.Ordinal);
        Assert.Contains("may gather the eligibility record into an attempt, but it does not turn eligibility into first-use admission", firstUse, StringComparison.Ordinal);
        Assert.Contains("fixes one receipted passive assessment surface", readiness, StringComparison.Ordinal);
    }

    private static string Normalize(string text)
    {
        var withoutBlockQuoteMarkers = Regex.Replace(
            text,
            @"^\s*>\s?",
            string.Empty,
            RegexOptions.Multiline);
        return Regex.Replace(withoutBlockQuoteMarkers, "\\s+", " ").Trim();
    }

    private static string GetRepoRoot()
    {
        var current = new DirectoryInfo(AppContext.BaseDirectory);

        while (current is not null &&
               (!File.Exists(Path.Combine(current.FullName, "build.ps1")) ||
                !File.Exists(Path.Combine(current.FullName, "README.md"))))
        {
            current = current.Parent;
        }

        return current?.FullName ?? throw new InvalidOperationException("Unable to locate repository root.");
    }
}
