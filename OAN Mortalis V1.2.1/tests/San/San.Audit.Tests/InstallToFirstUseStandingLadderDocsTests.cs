using System.Text.RegularExpressions;
using Xunit;

namespace San.Audit.Tests;

public sealed class InstallToFirstUseStandingLadderDocsTests
{
    [Fact]
    public void Standing_Ladder_Note_Locks_Governing_Sentence_And_Names_Seven_Standings()
    {
        var repoRoot = GetRepoRoot();
        var lawPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "INSTALL_TO_FIRST_USE_STANDING_LADDER.md");
        var lawText = Normalize(File.ReadAllText(lawPath));

        Assert.Contains("install-to-first-use standing is a lattice of bounded standings; no single standing grants first use, runtime authority, research consent, domain authority, active legal terms, or governing CME.", lawText, StringComparison.Ordinal);
        Assert.Contains("Install assent standing", lawText, StringComparison.Ordinal);
        Assert.Contains("`.GEL` formation footing standing", lawText, StringComparison.Ordinal);
        Assert.Contains("Install-facing expression standing", lawText, StringComparison.Ordinal);
        Assert.Contains("Readout reception and return standing", lawText, StringComparison.Ordinal);
        Assert.Contains("Approach anchor standing", lawText, StringComparison.Ordinal);
        Assert.Contains("Sanctuary pre-governing standing", lawText, StringComparison.Ordinal);
        Assert.Contains("Legal-admin Lab asset template staging standing", lawText, StringComparison.Ordinal);
    }

    [Fact]
    public void Standing_Ladder_Note_Separates_Standing_Kinds_And_Withholds_First_Use_Permission()
    {
        var repoRoot = GetRepoRoot();
        var lawPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "INSTALL_TO_FIRST_USE_STANDING_LADDER.md");
        var lawText = Normalize(File.ReadAllText(lawPath));

        Assert.Contains("Computational standing names what is typed, formed, witnessed, receipted, or referenced.", lawText, StringComparison.Ordinal);
        Assert.Contains("Legal-admin standing names disclosure, data-rights, retention, counsel-review, and consent-separation posture.", lawText, StringComparison.Ordinal);
        Assert.Contains("Operator-facing standing names correspondence, readout, reception, and bounded return.", lawText, StringComparison.Ordinal);
        Assert.Contains("Research standing remains separated from install and consent", lawText, StringComparison.Ordinal);
        Assert.Contains("Domain standing remains held unless separately admitted", lawText, StringComparison.Ordinal);
        Assert.Contains("Runtime standing remains withheld.", lawText, StringComparison.Ordinal);
        Assert.Contains("First use may be considered is future eligibility language, not permission.", lawText, StringComparison.Ordinal);
    }

    [Fact]
    public void Threaded_Docs_Preserve_Approach_Pre_Governing_Staging_And_First_Use_Distinctions()
    {
        var repoRoot = GetRepoRoot();
        var docsRoot = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs");

        var approach = Normalize(File.ReadAllText(Path.Combine(docsRoot, "INSTALL_FACING_APPROACH_BOUNDARY_AND_TELEMETRY_ANCHOR_LAW.md")));
        var standing = Normalize(File.ReadAllText(Path.Combine(docsRoot, "SANCTUARY_PRE_GOVERNING_STANDING_BOUNDARY.md")));
        var staging = Normalize(File.ReadAllText(Path.Combine(docsRoot, "LEGAL_ADMIN_LAB_ASSET_TEMPLATE_STAGING_BOUNDARY.md")));
        var readiness = Normalize(File.ReadAllText(Path.Combine(docsRoot, "BUILD_READINESS.md")));

        Assert.Contains("approach anchors as one bounded standing inside a larger lattice", approach, StringComparison.Ordinal);
        Assert.Contains("not allow approach anchors to grant first use", approach, StringComparison.Ordinal);
        Assert.Contains("Sanctuary pre-governing standing only", standing, StringComparison.Ordinal);
        Assert.Contains("not first-use permission", standing, StringComparison.Ordinal);
        Assert.Contains("legal-admin Lab asset template staging standing only", staging, StringComparison.Ordinal);
        Assert.Contains("not first-use permission", staging, StringComparison.Ordinal);
        Assert.Contains("reads current install, formation, expression, return, approach, pre-governing, and legal-admin staging postures as a bounded lattice rather than a first-use grant", readiness, StringComparison.Ordinal);
    }

    [Fact]
    public void Standing_Ladder_Does_Not_Add_First_Use_Runtime_Surface_Or_External_Paths()
    {
        var repoRoot = GetRepoRoot();
        var lineRoot = Path.Combine(repoRoot, "OAN Mortalis V1.2.1");
        var srcRoot = Path.Combine(lineRoot, "src");
        var docsRoot = Path.Combine(lineRoot, "docs");

        var sourceFiles = Directory
            .EnumerateFiles(srcRoot, "*.cs", SearchOption.AllDirectories)
            .Where(path =>
                !path.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}", StringComparison.OrdinalIgnoreCase) &&
                !path.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}", StringComparison.OrdinalIgnoreCase))
            .ToArray();

        Assert.DoesNotContain(sourceFiles, path =>
            Path.GetFileNameWithoutExtension(path).Contains("InstallToFirstUse", StringComparison.OrdinalIgnoreCase) ||
            (Path.GetFileNameWithoutExtension(path).Contains("FirstUse", StringComparison.OrdinalIgnoreCase) &&
             (Path.GetFileNameWithoutExtension(path).Contains("Service", StringComparison.OrdinalIgnoreCase) ||
              Path.GetFileNameWithoutExtension(path).Contains("Evaluator", StringComparison.OrdinalIgnoreCase) ||
              Path.GetFileNameWithoutExtension(path).Contains("Runtime", StringComparison.OrdinalIgnoreCase))) ||
            Path.GetFileNameWithoutExtension(path).Contains("ConsentEngine", StringComparison.OrdinalIgnoreCase) ||
            Path.GetFileNameWithoutExtension(path).Contains("LegalDocumentGenerator", StringComparison.OrdinalIgnoreCase));

        var scannedFiles = Directory
            .EnumerateFiles(docsRoot, "*.md", SearchOption.AllDirectories)
            .Concat(sourceFiles)
            .ToArray();

        foreach (var path in scannedFiles)
        {
            var text = File.ReadAllText(path);
            var driveSlashProbe = string.Concat("D:", "/");
            var driveBackslashProbe = string.Concat("D:", "\\");

            Assert.DoesNotContain("Documentation Repo", text, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain(driveSlashProbe, text, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain(driveBackslashProbe, text, StringComparison.OrdinalIgnoreCase);
        }
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
