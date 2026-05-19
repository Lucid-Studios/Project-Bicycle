using System.Text.RegularExpressions;
using Xunit;

namespace San.Audit.Tests;

public sealed class FirstUseEligibilityBoundaryDocsTests
{
    [Fact]
    public void First_Use_Eligibility_Note_Locks_Governing_Sentence_And_Ladder()
    {
        var repoRoot = GetRepoRoot();
        var lawPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "FIRST_USE_ELIGIBILITY_BOUNDARY.md");
        var lawText = Normalize(File.ReadAllText(lawPath));

        Assert.Contains("First use may be considered only after a localized `Sanctuary.GEL` substrate has formed, Sanctuary pre-governing standing has been admitted, and required predicate-surface, disclosure, data, retention, opt-out, research-separation, Special Case hold, domain hold, counsel-review, and non-authority postures have been represented.", lawText, StringComparison.Ordinal);
        Assert.Contains("materials available -> localized substrate formed -> pre-governing standing admitted -> first-use eligibility may be considered -> later first-use admission -> later governing CME formation", lawText, StringComparison.Ordinal);
        Assert.Contains("Eligibility consideration is not first-use admission.", lawText, StringComparison.Ordinal);
        Assert.Contains("Eligibility consideration is not runtime activation.", lawText, StringComparison.Ordinal);
        Assert.Contains("Eligibility consideration is not governing `CME` formation.", lawText, StringComparison.Ordinal);
    }

    [Fact]
    public void First_Use_Eligibility_Note_Requires_Predicate_Surface_Readiness()
    {
        var repoRoot = GetRepoRoot();
        var lawPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "FIRST_USE_ELIGIBILITY_BOUNDARY.md");
        var lawText = Normalize(File.ReadAllText(lawPath));

        Assert.Contains("Predicate-surface readiness is required before first-use eligibility may be considered.", lawText, StringComparison.Ordinal);
        Assert.Contains("the `Sanctuary.GEL` predicate pool is `Ready`", lawText, StringComparison.Ordinal);
        Assert.Contains("posture, trust/authorization, evidence footing, and response/disposition predicate families are present", lawText, StringComparison.Ordinal);
        Assert.Contains("first `.GEL` substrate records family-bearing predicate inheritance", lawText, StringComparison.Ordinal);
        Assert.Contains("explicit `SPC` remains withheld", lawText, StringComparison.Ordinal);
        Assert.Contains("validator exposure remains withheld", lawText, StringComparison.Ordinal);
        Assert.Contains("predicate promotion remains withheld", lawText, StringComparison.Ordinal);
        Assert.Contains("Atlas mutation remains withheld", lawText, StringComparison.Ordinal);
        Assert.Contains("runtime reasoning remains withheld", lawText, StringComparison.Ordinal);
        Assert.Contains("Missing predicate-surface readiness refuses first-use eligibility consideration.", lawText, StringComparison.Ordinal);
    }

    [Fact]
    public void First_Use_Eligibility_Note_Requires_Legal_Admin_Data_Hold_And_Non_Authority_Postures()
    {
        var repoRoot = GetRepoRoot();
        var lawPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "FIRST_USE_ELIGIBILITY_BOUNDARY.md");
        var lawText = Normalize(File.ReadAllText(lawPath));

        Assert.Contains("disclosure posture", lawText, StringComparison.Ordinal);
        Assert.Contains("local data posture", lawText, StringComparison.Ordinal);
        Assert.Contains("retention posture", lawText, StringComparison.Ordinal);
        Assert.Contains("opt-out posture", lawText, StringComparison.Ordinal);
        Assert.Contains("Research separation must be explicit.", lawText, StringComparison.Ordinal);
        Assert.Contains("Special Cases must remain held.", lawText, StringComparison.Ordinal);
        Assert.Contains("Domain-sensitive uses must remain held.", lawText, StringComparison.Ordinal);
        Assert.Contains("Counsel-review state may be represented, but counsel review must not be overclaimed.", lawText, StringComparison.Ordinal);
        Assert.Contains("admit first use", lawText, StringComparison.Ordinal);
    }

    [Fact]
    public void Threaded_Docs_Preserve_First_Use_Eligibility_As_Consideration_Only()
    {
        var repoRoot = GetRepoRoot();
        var docsRoot = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs");

        var localizedFormation = Normalize(File.ReadAllText(Path.Combine(docsRoot, "LOCALIZED_SANCTUARY_GEL_FORMATION_BOUNDARY.md")));
        var preGoverning = Normalize(File.ReadAllText(Path.Combine(docsRoot, "SANCTUARY_PRE_GOVERNING_STANDING_BOUNDARY.md")));
        var standingLadder = Normalize(File.ReadAllText(Path.Combine(docsRoot, "INSTALL_TO_FIRST_USE_STANDING_LADDER.md")));
        var readiness = Normalize(File.ReadAllText(Path.Combine(docsRoot, "BUILD_READINESS.md")));

        Assert.Contains("may ask whether localized formation and predicate surfaces are ready enough for first use to be considered", localizedFormation, StringComparison.Ordinal);
        Assert.Contains("it may not admit first use or define active `.GEL` formation procedure", localizedFormation, StringComparison.Ordinal);
        Assert.Contains("The first-use eligibility boundary sits above this pre-governing standing posture", preGoverning, StringComparison.Ordinal);
        Assert.Contains("predicate-surface, disclosure, data, retention, opt-out, research-separation, Special Case hold, domain hold, counsel-review, and non-authority postures", preGoverning, StringComparison.Ordinal);
        Assert.Contains("That membrane now names predicate-surface readiness", standingLadder, StringComparison.Ordinal);
        Assert.Contains("even localized formation and pre-governing standing do not grant first use", standingLadder, StringComparison.Ordinal);
        Assert.Contains("first-use eligibility consideration now requires localized formation", readiness, StringComparison.Ordinal);
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
