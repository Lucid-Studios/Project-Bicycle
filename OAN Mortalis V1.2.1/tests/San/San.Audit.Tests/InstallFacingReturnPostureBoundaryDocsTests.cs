using System.Text.RegularExpressions;
using Xunit;

namespace San.Audit.Tests;

public sealed class InstallFacingReturnPostureBoundaryDocsTests
{
    [Fact]
    public void Return_Posture_Note_States_Bounded_Local_Consequence_Only()
    {
        var repoRoot = GetRepoRoot();
        var lawPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "INSTALL_FACING_RETURN_POSTURE_BOUNDARY.md");
        var lawText = Normalize(File.ReadAllText(lawPath));

        Assert.Contains("install-facing return posture records the bounded local consequence of readout consumption; it does not activate, admit, certify, execute, or hand off authority.", lawText, StringComparison.Ordinal);
        Assert.Contains("`WitnessedForwardHorizon` is not `RTME` approach. `WitnessedForwardHorizon` is not pre-certification.", lawText, StringComparison.Ordinal);
    }

    private static string Normalize(string text)
    {
        var withoutBlockQuoteMarkers = text.Replace("> ", string.Empty, StringComparison.Ordinal);
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
