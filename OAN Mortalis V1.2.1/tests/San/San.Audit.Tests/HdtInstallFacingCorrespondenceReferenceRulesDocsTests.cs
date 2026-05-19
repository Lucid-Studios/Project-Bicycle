using Xunit;

namespace San.Audit.Tests;

public sealed class HdtInstallFacingCorrespondenceReferenceRulesDocsTests
{
    [Fact]
    public void Hdt_Rules_Note_Locks_Evidence_Only_Reference()
    {
        var repoRoot = GetRepoRoot();
        var lawPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "HDT_INSTALL_FACING_CORRESPONDENCE_REFERENCE_RULES.md");

        var lawText = NormalizeWhitespace(File.ReadAllText(lawPath));

        Assert.Contains("`HDT` may reference install-facing correspondence vocabulary for evidence and support.", lawText, StringComparison.Ordinal);
        Assert.Contains("`HDT` may not use that reference to claim truth, admissibility, certification, runtime standing, or operator authorization.", lawText, StringComparison.Ordinal);
        Assert.Contains("`V1.2.1` may not treat `HDT` reference as admissibility proof.", lawText, StringComparison.Ordinal);
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
