using System.Text.RegularExpressions;
using Xunit;

namespace San.Audit.Tests;

public sealed class InstallFacingApproachBoundaryDocsTests
{
    [Fact]
    public void Approach_Boundary_Note_Locks_Non_Authority_Sentence()
    {
        var repoRoot = GetRepoRoot();
        var lawPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "INSTALL_FACING_APPROACH_BOUNDARY_AND_TELEMETRY_ANCHOR_LAW.md");
        var lawText = Normalize(File.ReadAllText(lawPath));

        Assert.Contains("approach boundary names future eligibility from bounded return posture; it does not emit, template, certify, hand off, or activate authority.", lawText, StringComparison.Ordinal);
        Assert.Contains("`WitnessedForwardHorizon` is future-facing only.", lawText, StringComparison.Ordinal);
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
