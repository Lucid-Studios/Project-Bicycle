using System.Text.RegularExpressions;
using Xunit;

namespace San.Audit.Tests;

public sealed class LegalAdminLabAssetTemplateStagingBoundaryDocsTests
{
    [Fact]
    public void Staging_Boundary_Note_Locks_Review_Candidate_Sentence_And_Ladder()
    {
        var repoRoot = GetRepoRoot();
        var lawPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "LEGAL_ADMIN_LAB_ASSET_TEMPLATE_STAGING_BOUNDARY.md");
        var lawText = Normalize(File.ReadAllText(lawPath));

        Assert.Contains("Lab-staged legal-admin templates are review candidates, not active legal terms, consent records, disclosure surfaces, certification claims, or operational authority.", lawText, StringComparison.Ordinal);
        Assert.Contains("Research legal-admin source body", lawText, StringComparison.Ordinal);
        Assert.Contains("Lab asset template staging", lawText, StringComparison.Ordinal);
        Assert.Contains("Regional counsel review", lawText, StringComparison.Ordinal);
        Assert.Contains("Build-ready documentation", lawText, StringComparison.Ordinal);
    }

    [Fact]
    public void Staging_Boundary_Note_Refuses_Active_Legal_Consent_Certification_And_Runtime_Authority()
    {
        var repoRoot = GetRepoRoot();
        var lawPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "LEGAL_ADMIN_LAB_ASSET_TEMPLATE_STAGING_BOUNDARY.md");
        var lawText = Normalize(File.ReadAllText(lawPath));

        Assert.Contains("They are not active template bodies.", lawText, StringComparison.Ordinal);
        Assert.Contains("They are not legal terms.", lawText, StringComparison.Ordinal);
        Assert.Contains("They are not consent records.", lawText, StringComparison.Ordinal);
        Assert.Contains("They are not certification claims.", lawText, StringComparison.Ordinal);
        Assert.Contains("does not mean runtime authority, consent activation, certification, operator authorization, domain authority, or `RTME` approach.", lawText, StringComparison.Ordinal);
        Assert.Contains("copy legal-admin research documents into Build as active documents", lawText, StringComparison.Ordinal);
    }

    [Fact]
    public void Staging_Boundary_And_Threaded_Docs_Do_Not_Expose_External_Documentation_Paths()
    {
        var repoRoot = GetRepoRoot();
        var lineRoot = Path.Combine(repoRoot, "OAN Mortalis V1.2.1");
        var scannedRoots = new[]
        {
            Path.Combine(lineRoot, "docs"),
            Path.Combine(lineRoot, "src")
        };

        var trackedLikeFiles = scannedRoots
            .SelectMany(root => Directory.EnumerateFiles(root, "*.*", SearchOption.AllDirectories))
            .Where(path =>
                (path.EndsWith(".md", StringComparison.OrdinalIgnoreCase) ||
                 path.EndsWith(".cs", StringComparison.OrdinalIgnoreCase)) &&
                !path.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}", StringComparison.OrdinalIgnoreCase) &&
                !path.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}", StringComparison.OrdinalIgnoreCase))
            .ToArray();

        foreach (var path in trackedLikeFiles)
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
