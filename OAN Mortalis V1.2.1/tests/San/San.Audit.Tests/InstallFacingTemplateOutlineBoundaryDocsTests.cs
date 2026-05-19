using Xunit;

namespace San.Audit.Tests;

public sealed class InstallFacingTemplateOutlineBoundaryDocsTests
{
    [Fact]
    public void Template_Outline_Note_States_Templating_Is_Future_Facing_And_Doctrine_Only()
    {
        var repoRoot = GetRepoRoot();
        var lawPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "INSTALL_FACING_TEMPLATE_OUTLINE_BOUNDARY.md");

        var lawText = NormalizeWhitespace(File.ReadAllText(lawPath));

        Assert.Contains("templating is a later downstream presentation and packaging concern.", lawText, StringComparison.Ordinal);
        Assert.Contains("local install-facing templating in `V1.2.1` is not yet an active office.", lawText, StringComparison.Ordinal);
        Assert.Contains("No contracts, template data files, services, generators, or `HDT` integration code are introduced by this step.", lawText, StringComparison.Ordinal);
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
