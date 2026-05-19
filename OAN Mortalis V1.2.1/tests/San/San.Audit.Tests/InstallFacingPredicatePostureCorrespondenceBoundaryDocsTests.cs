using Xunit;

namespace San.Audit.Tests;

public sealed class InstallFacingPredicatePostureCorrespondenceBoundaryDocsTests
{
    [Fact]
    public void Correspondence_Boundary_Names_The_Practical_Ladder_And_Hdt_Boundary()
    {
        var repoRoot = GetRepoRoot();
        var lawPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "INSTALL_FACING_PREDICATE_POSTURE_CORRESPONDENCE_BOUNDARY.md");
        var buildReadinessPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "BUILD_READINESS.md");

        var lawText = NormalizeWhitespace(File.ReadAllText(lawPath));
        var buildReadinessText = NormalizeWhitespace(File.ReadAllText(buildReadinessPath));

        Assert.Contains("`RootAtlas` says what is rooted. `SLI` says what is formed. `HDT` says how formed objects may be viewed, related, compared, and explored. later `Engram` says what may survive into carried formation.", lawText, StringComparison.Ordinal);
        Assert.Contains("`HDT` is a governed field and artifact lane", lawText, StringComparison.Ordinal);
        Assert.Contains("`HDT` does not mint: truth admissibility runtime standing", lawText, StringComparison.Ordinal);
        Assert.Contains("this correspondence boundary does not: type payloads template payloads verify payloads certify payloads project artifact surfaces compare temporal stacks perform support inspection create runtime authority", lawText, StringComparison.Ordinal);
        Assert.Contains("INSTALL_FACING_PREDICATE_POSTURE_CORRESPONDENCE_BOUNDARY.md", buildReadinessText, StringComparison.Ordinal);
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
