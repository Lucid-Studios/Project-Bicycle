using Xunit;

namespace San.Audit.Tests;

public sealed class SanctuaryGelFirstPredicateCandidateFamiliesLawDocsTests
{
    [Fact]
    public void First_Predicate_Candidate_Families_Law_Names_The_Four_Families()
    {
        var repoRoot = GetRepoRoot();
        var lawPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "SANCTUARY_GEL_FIRST_PREDICATE_CANDIDATE_FAMILIES_LAW.md");
        var buildReadinessPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "BUILD_READINESS.md");

        var lawText = NormalizeWhitespace(File.ReadAllText(lawPath));
        var buildReadinessText = NormalizeWhitespace(File.ReadAllText(buildReadinessPath));

        Assert.Contains("the predicate pool now carries typed predicate candidate families.", lawText, StringComparison.Ordinal);
        Assert.Contains("`PosturePredicate`", lawText, StringComparison.Ordinal);
        Assert.Contains("`TrustAuthorizationPredicate`", lawText, StringComparison.Ordinal);
        Assert.Contains("`EvidenceFootingPredicate`", lawText, StringComparison.Ordinal);
        Assert.Contains("`ResponseDispositionPredicate`", lawText, StringComparison.Ordinal);
        Assert.Contains("they remain below explicit `SPC` and validator exposure.", lawText, StringComparison.Ordinal);
        Assert.Contains("SANCTUARY_GEL_FIRST_PREDICATE_CANDIDATE_FAMILIES_LAW.md", buildReadinessText, StringComparison.Ordinal);
    }

    private static string NormalizeWhitespace(string value)
    {
        var withoutBlockQuoteMarkers = System.Text.RegularExpressions.Regex.Replace(
            value,
            @"^\s*>\s?",
            string.Empty,
            System.Text.RegularExpressions.RegexOptions.Multiline);
        return System.Text.RegularExpressions.Regex.Replace(withoutBlockQuoteMarkers, "\\s+", " ").Trim();
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
