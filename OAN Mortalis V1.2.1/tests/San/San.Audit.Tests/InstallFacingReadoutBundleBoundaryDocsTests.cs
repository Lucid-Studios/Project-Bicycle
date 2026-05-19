using Xunit;

namespace San.Audit.Tests;

public sealed class InstallFacingReadoutBundleBoundaryDocsTests
{
    [Fact]
    public void Readout_Bundle_Note_States_Bounded_Outward_Expression_Only()
    {
        var repoRoot = GetRepoRoot();
        var lawPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "INSTALL_FACING_READOUT_BUNDLE_BOUNDARY.md");

        var lawText = NormalizeWhitespace(File.ReadAllText(lawPath));

        Assert.Contains("a readout bundle is a bounded outward expression of already-lawful install footing composed from seated correspondence vocabulary; it does not shape, verify, admit, authorize, or project.", lawText, StringComparison.Ordinal);
        Assert.Contains("Readout is not shaping. Readout is not admissibility. Readout is not artifact projection. Readout is not templating authority. Readout is not runtime authority.", lawText, StringComparison.Ordinal);
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
