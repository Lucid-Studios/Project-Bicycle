using System.Text.RegularExpressions;
using Xunit;

namespace San.Audit.Tests;

public sealed class LocalizedSanctuaryGelFormationBoundaryDocsTests
{
    [Fact]
    public void Localized_Formation_Note_Locks_Governing_Sentences_And_Spine()
    {
        var repoRoot = GetRepoRoot();
        var lawPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "LOCALIZED_SANCTUARY_GEL_FORMATION_BOUNDARY.md");
        var lawText = Normalize(File.ReadAllText(lawPath));

        Assert.Contains("First `Sanctuary.GEL` formation is not an automatic install artifact; it is a localized continuity-substrate formation event that may occur only after National, Regional, and Local standing have been represented.", lawText, StringComparison.Ordinal);
        Assert.Contains("Formation answers whether the bounded local substrate exists. Standing answers what the formed substrate is allowed to mean.", lawText, StringComparison.Ordinal);
        Assert.Contains("National Standing -> Regional Standing -> Local Standing -> First Sanctuary.GEL Formation -> Sanctuary Pre-Governing Standing -> First-Use Eligibility Consideration -> later Governing CME Formation", lawText, StringComparison.Ordinal);
    }

    [Fact]
    public void Localized_Formation_Note_Refuses_Governance_First_Use_Consent_Domain_Rtme_And_Runtime()
    {
        var repoRoot = GetRepoRoot();
        var lawPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "LOCALIZED_SANCTUARY_GEL_FORMATION_BOUNDARY.md");
        var lawText = Normalize(File.ReadAllText(lawPath));

        Assert.Contains("govern", lawText, StringComparison.Ordinal);
        Assert.Contains("certify", lawText, StringComparison.Ordinal);
        Assert.Contains("authorize domains", lawText, StringComparison.Ordinal);
        Assert.Contains("activate `RTME`", lawText, StringComparison.Ordinal);
        Assert.Contains("perform counsel-reviewed disclosure", lawText, StringComparison.Ordinal);
        Assert.Contains("mint consent", lawText, StringComparison.Ordinal);
        Assert.Contains("admit `CME` personhood", lawText, StringComparison.Ordinal);
        Assert.Contains("claim first use", lawText, StringComparison.Ordinal);
        Assert.Contains("claim runtime authority", lawText, StringComparison.Ordinal);
    }

    [Fact]
    public void Threaded_Docs_Preserve_Localized_Formation_And_Pre_Governing_Standing_Distinction()
    {
        var repoRoot = GetRepoRoot();
        var docsRoot = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs");

        var firstFormation = Normalize(File.ReadAllText(Path.Combine(docsRoot, "FIRST_RUNTIME_ADMITTED_SANCTUARY_GEL_FORMATION_LAW.md")));
        var standingLadder = Normalize(File.ReadAllText(Path.Combine(docsRoot, "INSTALL_TO_FIRST_USE_STANDING_LADDER.md")));
        var preGoverning = Normalize(File.ReadAllText(Path.Combine(docsRoot, "SANCTUARY_PRE_GOVERNING_STANDING_BOUNDARY.md")));
        var readiness = Normalize(File.ReadAllText(Path.Combine(docsRoot, "BUILD_READINESS.md")));

        Assert.Contains("First `.GEL` is formed under represented standing, not merely from available install materials.", firstFormation, StringComparison.Ordinal);
        Assert.Contains("National, Regional, and Local standing must be represented", firstFormation, StringComparison.Ordinal);
        Assert.Contains("formation answers whether the bounded local substrate exists", standingLadder, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("what the formed substrate is allowed to mean", standingLadder, StringComparison.Ordinal);
        Assert.Contains("Formation answers whether the bounded local substrate exists; this standing boundary answers what that substrate is allowed to mean before governing `CME`.", preGoverning, StringComparison.Ordinal);
        Assert.Contains("localized first `.GEL` formation now requires represented National, Regional, and Local standing", readiness, StringComparison.Ordinal);
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
