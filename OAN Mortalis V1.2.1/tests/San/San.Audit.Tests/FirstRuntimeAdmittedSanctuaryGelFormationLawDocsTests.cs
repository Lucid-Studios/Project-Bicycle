using Xunit;

namespace San.Audit.Tests;

public sealed class FirstRuntimeAdmittedSanctuaryGelFormationLawDocsTests
{
    [Fact]
    public void First_Runtime_Admitted_Sanctuary_Gel_Formation_Law_Preserves_Tripartite_Order()
    {
        var repoRoot = GetRepoRoot();
        var lawPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "FIRST_RUNTIME_ADMITTED_SANCTUARY_GEL_FORMATION_LAW.md");
        var buildReadinessPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "BUILD_READINESS.md");

        var lawText = NormalizeWhitespace(File.ReadAllText(lawPath));
        var buildReadinessText = NormalizeWhitespace(File.ReadAllText(buildReadinessPath));

        Assert.Contains("The tripartite is `RootAtlas -> symbolic law body -> engrammitization`.", lawText, StringComparison.Ordinal);
        Assert.Contains("`.GEL` is the first local formed product body of that tripartite.", lawText, StringComparison.Ordinal);
        Assert.Contains("`SLI.Lisp` is the cryptic working medium", lawText, StringComparison.Ordinal);
        Assert.Contains("The first lawful `.GEL` body is a `RuntimeAdmittedSubstrate`", lawText, StringComparison.Ordinal);
        Assert.Contains("Return to `Prime` and later human-facing products are downstream of formed `.GEL`", lawText, StringComparison.Ordinal);
        Assert.Contains("FIRST_RUNTIME_ADMITTED_SANCTUARY_GEL_FORMATION_LAW.md", buildReadinessText, StringComparison.Ordinal);
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
