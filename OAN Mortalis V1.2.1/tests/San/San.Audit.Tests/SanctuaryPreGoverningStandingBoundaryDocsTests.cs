using System.Text.RegularExpressions;
using Xunit;

namespace San.Audit.Tests;

public sealed class SanctuaryPreGoverningStandingBoundaryDocsTests
{
    [Fact]
    public void Standing_Boundary_Note_Names_Pre_Governing_Sanctuary_Standing()
    {
        var repoRoot = GetRepoRoot();
        var lawPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "SANCTUARY_PRE_GOVERNING_STANDING_BOUNDARY.md");
        var lawText = Normalize(File.ReadAllText(lawPath));

        Assert.Contains("Sanctuary pre-governing standing records that the install-facing body has enough bounded footing, disclosure posture, data-rights posture, and research separation to stand locally before governing CME exists.", lawText, StringComparison.Ordinal);
        Assert.Contains("Legal-admin research materials are template resources. They are not active legal terms.", lawText, StringComparison.Ordinal);
        Assert.Contains("Install is not research consent.", lawText, StringComparison.Ordinal);
    }

    [Fact]
    public void Standing_Boundary_Note_Refuses_Cme_Overclaim_And_External_Paths()
    {
        var repoRoot = GetRepoRoot();
        var lawPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "SANCTUARY_PRE_GOVERNING_STANDING_BOUNDARY.md");
        var lawText = Normalize(File.ReadAllText(lawPath));

        Assert.Contains("The Operator-`CME` bond is not:", lawText, StringComparison.Ordinal);
        Assert.Contains("domain authority", lawText, StringComparison.Ordinal);
        Assert.Contains("legal personhood", lawText, StringComparison.Ordinal);
        Assert.Contains("agency", lawText, StringComparison.Ordinal);
        Assert.Contains("fiduciary status", lawText, StringComparison.Ordinal);
        Assert.Contains("professional standing", lawText, StringComparison.Ordinal);
        Assert.Contains("liability shielding", lawText, StringComparison.Ordinal);
        Assert.Contains("Bonded `CME` data is continuity-bearing personal data before it is generic telemetry", lawText, StringComparison.Ordinal);
        Assert.Contains("Receipt telemetry is bounded and disclosed. It is not surveillance.", lawText, StringComparison.Ordinal);
        var driveSlashProbe = string.Concat("D:", "/");

        Assert.DoesNotContain("Documentation Repo", lawText, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain(driveSlashProbe, lawText, StringComparison.OrdinalIgnoreCase);
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
