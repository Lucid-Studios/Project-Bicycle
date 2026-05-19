using Xunit;

namespace San.Audit.Tests;

public sealed class RegionalAtlasPackageAdmissionLawDocsTests
{
    [Fact]
    public void Regional_Atlas_Admission_Law_Places_English_Package_Before_Gel_Formation()
    {
        var repoRoot = GetRepoRoot();
        var lawPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "REGIONAL_ATLAS_PACKAGE_ADMISSION_BEFORE_GEL_FORMATION_LAW.md");
        var buildReadinessPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "BUILD_READINESS.md");
        var gelFormationLawPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "FIRST_RUNTIME_ADMITTED_SANCTUARY_GEL_FORMATION_LAW.md");

        var lawText = NormalizeWhitespace(File.ReadAllText(lawPath));
        var buildReadinessText = NormalizeWhitespace(File.ReadAllText(buildReadinessPath));
        var gelFormationLawText = NormalizeWhitespace(File.ReadAllText(gelFormationLawPath));

        Assert.Contains("regional Atlas package admission occurs after localized assent footing and before first runtime-admitted `Sanctuary.GEL` formation.", lawText, StringComparison.Ordinal);
        Assert.Contains("The first admitted package in this phase is: - `EnglishRegionalAtlasPackage`", lawText, StringComparison.Ordinal);
        Assert.Contains("local telemetry and local symbolic footing may bind only to the admitted regional package", lawText, StringComparison.Ordinal);
        Assert.Contains("universal Atlas authority remains lab-side", lawText, StringComparison.Ordinal);
        Assert.Contains("REGIONAL_ATLAS_PACKAGE_ADMISSION_BEFORE_GEL_FORMATION_LAW.md", buildReadinessText, StringComparison.Ordinal);
        Assert.Contains("regional package", gelFormationLawText, StringComparison.Ordinal);
        Assert.Contains("admitted English regional package basis", gelFormationLawText, StringComparison.Ordinal);
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
