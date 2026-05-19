using Xunit;

namespace San.Audit.Tests;

public sealed class SanctuaryGelFormationDataPoolLawDocsTests
{
    [Fact]
    public void Formation_Data_Pool_Law_Names_The_Immediate_Upstream_Body()
    {
        var repoRoot = GetRepoRoot();
        var lawPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "SANCTUARY_GEL_FORMATION_DATA_POOL_LAW.md");
        var buildReadinessPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "BUILD_READINESS.md");

        var lawText = NormalizeWhitespace(File.ReadAllText(lawPath));
        var buildReadinessText = NormalizeWhitespace(File.ReadAllText(buildReadinessPath));

        Assert.Contains("the formation data pool is the immediate upstream body for first `Sanctuary.GEL` formation.", lawText, StringComparison.Ordinal);
        Assert.Contains("credential footing", lawText, StringComparison.Ordinal);
        Assert.Contains("`CoreCmeUsePostureRecord`", lawText, StringComparison.Ordinal);
        Assert.Contains("admitted English regional package", lawText, StringComparison.Ordinal);
        Assert.Contains("bounded lab predicate inheritance", lawText, StringComparison.Ordinal);
        Assert.Contains("Future cryptic posture may inherit localized governing standing from this body.", lawText, StringComparison.Ordinal);
        Assert.Contains("uncertified or unauthorized communications may terminate in `Silence`", lawText, StringComparison.Ordinal);
        Assert.Contains("SANCTUARY_GEL_FORMATION_DATA_POOL_LAW.md", buildReadinessText, StringComparison.Ordinal);
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
