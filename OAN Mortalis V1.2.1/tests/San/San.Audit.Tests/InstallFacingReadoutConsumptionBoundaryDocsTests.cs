using Xunit;

namespace San.Audit.Tests;

public sealed class InstallFacingReadoutConsumptionBoundaryDocsTests
{
    [Fact]
    public void Readout_Consumption_Note_States_Receiving_And_Witnessing_Only()
    {
        var repoRoot = GetRepoRoot();
        var lawPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "INSTALL_FACING_READOUT_CONSUMPTION_BOUNDARY.md");

        var lawText = NormalizeWhitespace(File.ReadAllText(lawPath));

        Assert.Contains("readout consumption receives and witnesses bounded outward readout; it does not shape, verify, admit, authorize, execute, or project.", lawText, StringComparison.Ordinal);
        Assert.Contains("Consumption is not readout generation. Consumption is not admissibility. Consumption is not runtime activation. Consumption is not operator realization.", lawText, StringComparison.Ordinal);
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
