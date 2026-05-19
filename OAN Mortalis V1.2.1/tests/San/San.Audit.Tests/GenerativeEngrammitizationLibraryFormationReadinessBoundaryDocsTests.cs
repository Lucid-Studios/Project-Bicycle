using System.Text.RegularExpressions;
using Xunit;

namespace San.Audit.Tests;

public sealed class GenerativeEngrammitizationLibraryFormationReadinessBoundaryDocsTests
{
    [Fact]
    public void Gel_Readiness_Note_Locks_Governing_Sentence_And_Ladder()
    {
        var repoRoot = GetRepoRoot();
        var lawPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "GENERATIVE_ENGRAMMITIZATION_LIBRARY_FORMATION_READINESS_BOUNDARY.md");
        var lawText = Normalize(File.ReadAllText(lawPath));

        Assert.Contains("`GEL` means `Generative Engrammitization Library` in this boundary.", lawText, StringComparison.Ordinal);
        Assert.Contains("`.GEL` inclusion does not equal final `Sanctuary.GEL` survivor admission.", lawText, StringComparison.Ordinal);
        Assert.Contains("GEL formation readiness -> first localized Sanctuary.GEL formation -> Sanctuary pre-governing standing -> first-use eligibility consideration -> later survivor/admission/governing work", lawText, StringComparison.Ordinal);
        Assert.Contains("The `GEL` readiness body is upstream of localized `Sanctuary.GEL` formation.", lawText, StringComparison.Ordinal);
        Assert.Contains("It is not survivor admission.", lawText, StringComparison.Ordinal);
    }

    [Fact]
    public void Gel_Readiness_Note_Names_Candidate_Posture_Without_Survivor_Admission()
    {
        var repoRoot = GetRepoRoot();
        var lawPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "GENERATIVE_ENGRAMMITIZATION_LIBRARY_FORMATION_READINESS_BOUNDARY.md");
        var lawText = Normalize(File.ReadAllText(lawPath));

        Assert.Contains("may hold rooted, witnessed, `SLI`-formed, Engrammitization-facing candidates", lawText, StringComparison.Ordinal);
        Assert.Contains("logical research source posture", lawText, StringComparison.Ordinal);
        Assert.Contains("predicate-surface readiness", lawText, StringComparison.Ordinal);
        Assert.Contains("readiness posture only", lawText, StringComparison.Ordinal);
        Assert.Contains("They do not become legal objects, consent-bearing records, user-facing artifacts, localized Sanctuary survivor records, governing `CME`, `RTME`, or runtime authority.", lawText, StringComparison.Ordinal);
    }

    [Fact]
    public void Gel_Readiness_Note_Requires_Localized_Pre_Certification_And_Standing_Before_Localized_Formation()
    {
        var repoRoot = GetRepoRoot();
        var lawPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "GENERATIVE_ENGRAMMITIZATION_LIBRARY_FORMATION_READINESS_BOUNDARY.md");
        var lawText = Normalize(File.ReadAllText(lawPath));

        Assert.Contains("only after localized pre-certification data pool posture and represented National, Regional, and Local standing requirements are present", lawText, StringComparison.Ordinal);
        Assert.Contains("A candidate may be ready for generative formation and still fail", lawText, StringComparison.Ordinal);
        Assert.Contains("localized `Sanctuary.GEL` formation", lawText, StringComparison.Ordinal);
        Assert.Contains("first-use eligibility consideration", lawText, StringComparison.Ordinal);
        Assert.Contains("survivor admission", lawText, StringComparison.Ordinal);
        Assert.Contains("governing `CME` formation", lawText, StringComparison.Ordinal);
    }

    [Fact]
    public void Gel_Readiness_Note_Refuses_Runtime_Governance_Promotion_And_Validator_Exposure()
    {
        var repoRoot = GetRepoRoot();
        var lawPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "GENERATIVE_ENGRAMMITIZATION_LIBRARY_FORMATION_READINESS_BOUNDARY.md");
        var lawText = Normalize(File.ReadAllText(lawPath));

        Assert.Contains("admit final `Sanctuary.GEL` survivor standing", lawText, StringComparison.Ordinal);
        Assert.Contains("create localized `Sanctuary.GEL` formation by itself", lawText, StringComparison.Ordinal);
        Assert.Contains("grant first-use eligibility or first-use admission", lawText, StringComparison.Ordinal);
        Assert.Contains("activate `RTME`", lawText, StringComparison.Ordinal);
        Assert.Contains("create runtime authority", lawText, StringComparison.Ordinal);
        Assert.Contains("mutate Atlas authority", lawText, StringComparison.Ordinal);
        Assert.Contains("promote predicates", lawText, StringComparison.Ordinal);
        Assert.Contains("expose `SPC` validator posture", lawText, StringComparison.Ordinal);
    }

    [Fact]
    public void Threaded_Docs_Preserve_Readiness_Formation_And_Survivor_Admission_Distinctions()
    {
        var repoRoot = GetRepoRoot();
        var docsRoot = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs");

        var firstFormation = Normalize(File.ReadAllText(Path.Combine(docsRoot, "FIRST_RUNTIME_ADMITTED_SANCTUARY_GEL_FORMATION_LAW.md")));
        var localizedFormation = Normalize(File.ReadAllText(Path.Combine(docsRoot, "LOCALIZED_SANCTUARY_GEL_FORMATION_BOUNDARY.md")));
        var firstUse = Normalize(File.ReadAllText(Path.Combine(docsRoot, "FIRST_USE_ELIGIBILITY_BOUNDARY.md")));
        var standingLadder = Normalize(File.ReadAllText(Path.Combine(docsRoot, "INSTALL_TO_FIRST_USE_STANDING_LADDER.md")));
        var readiness = Normalize(File.ReadAllText(Path.Combine(docsRoot, "BUILD_READINESS.md")));

        Assert.Contains("`.GEL` inclusion does not equal final `Sanctuary.GEL` survivor admission.", firstFormation, StringComparison.Ordinal);
        Assert.Contains("may draw from rooted, witnessed, `SLI`-formed, Engrammitization-facing `.GEL` readiness only after localized pre-certification data pool posture and National, Regional, and Local standing are represented", localizedFormation, StringComparison.Ordinal);
        Assert.Contains("does not turn `GEL` formation readiness into survivor admission", firstUse, StringComparison.Ordinal);
        Assert.Contains("`Generative Engrammitization Library` candidate posture beneath localized `.GEL` formation", standingLadder, StringComparison.Ordinal);
        Assert.Contains("fixes `.GEL` inclusion as library-level candidate readiness rather than final `Sanctuary.GEL` survivor admission", readiness, StringComparison.Ordinal);
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
