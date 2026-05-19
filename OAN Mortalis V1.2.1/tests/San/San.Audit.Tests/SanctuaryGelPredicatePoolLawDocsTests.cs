using Xunit;

namespace San.Audit.Tests;

public sealed class SanctuaryGelPredicatePoolLawDocsTests
{
    [Fact]
    public void Predicate_Pool_Law_Names_The_Layer_Between_Data_Pool_And_First_Gel()
    {
        var repoRoot = GetRepoRoot();
        var lawPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "SANCTUARY_GEL_PREDICATE_POOL_LAW.md");
        var buildReadinessPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "BUILD_READINESS.md");

        var lawText = NormalizeWhitespace(File.ReadAllText(lawPath));
        var buildReadinessText = NormalizeWhitespace(File.ReadAllText(buildReadinessPath));

        Assert.Contains("the predicate pool is downstream of `SanctuaryGelFormationDataPool` and upstream of first `Sanctuary.GEL`.", lawText, StringComparison.Ordinal);
        Assert.Contains("bounded lab predicate inheritance", lawText, StringComparison.Ordinal);
        Assert.Contains("English-only", lawText, StringComparison.Ordinal);
        Assert.Contains("posture trust / authorization evidence footing response / disposition", lawText, StringComparison.Ordinal);
        Assert.Contains("not raw Atlas authority", lawText, StringComparison.Ordinal);
        Assert.Contains("not explicit bonded-validator posture", lawText, StringComparison.Ordinal);
        Assert.Contains("`SPC` belongs to later formal bonded-cognition activation.", lawText, StringComparison.Ordinal);
        Assert.Contains("SANCTUARY_GEL_PREDICATE_POOL_LAW.md", buildReadinessText, StringComparison.Ordinal);
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
