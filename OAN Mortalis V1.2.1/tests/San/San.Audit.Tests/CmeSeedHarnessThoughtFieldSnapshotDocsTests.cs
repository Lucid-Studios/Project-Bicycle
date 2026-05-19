using System.Text.RegularExpressions;
using Xunit;

namespace San.Audit.Tests;

public sealed class CmeSeedHarnessThoughtFieldSnapshotDocsTests
{
    [Fact]
    public void Snapshot_Doc_Locks_Governing_Sentences_And_Frozen_Thought_Field()
    {
        var repoRoot = GetRepoRoot();
        var boundaryPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "CME_SEED_HARNESS_THOUGHT_FIELD_SNAPSHOT.md");
        var boundaryText = Normalize(File.ReadAllText(boundaryPath));

        Assert.Contains("The CME seed harness thought-field snapshot freezes documented data, metadata-only proof posture, governed inventory schema, and inventory evaluation posture as seed context for a first callable response lane; it does not ingest raw Lab data, create consent, train models, execute SLI.Lisp, activate RTME, mutate Prime/Cryptic, form Sanctuary.Actual, or grant runtime authority.", boundaryText, StringComparison.Ordinal);
        Assert.Contains("The first executable harness may render and call a response lane from the frozen thought field only; its response is not activation, certification, survivor admission, or runtime authority.", boundaryText, StringComparison.Ordinal);
        Assert.Contains("documented data -> metadata-only proof posture -> governed inventory schema -> inventory evaluation posture -> claim-reading threshold -> first CME seed harness response lane", boundaryText, StringComparison.Ordinal);
    }

    [Fact]
    public void Snapshot_Doc_Keeps_Executable_App_Local_Private_And_Out_Of_Public_Repo()
    {
        var repoRoot = GetRepoRoot();
        var boundaryPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "CME_SEED_HARNESS_THOUGHT_FIELD_SNAPSHOT.md");
        var readinessPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "BUILD_READINESS.md");
        var boundaryText = Normalize(File.ReadAllText(boundaryPath));
        var readinessText = Normalize(File.ReadAllText(readinessPath));

        Assert.Contains("This public Build snapshot does not add the executable app itself.", boundaryText, StringComparison.Ordinal);
        Assert.Contains("The executable seed harness remains local/private until a later explicit admission decides what, if anything, may enter the public repository.", boundaryText, StringComparison.Ordinal);
        Assert.Contains("add the executable app to the public repository", boundaryText, StringComparison.Ordinal);
        Assert.Contains("the executable harness app itself remains local/private and is not introduced into the public repository in this build set", readinessText, StringComparison.Ordinal);
        Assert.Contains("without committing the executable app", readinessText, StringComparison.Ordinal);
    }

    [Fact]
    public void Snapshot_Doc_Names_Ui_Template_Form_Response_Lane_And_Non_Powers()
    {
        var repoRoot = GetRepoRoot();
        var boundaryPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "CME_SEED_HARNESS_THOUGHT_FIELD_SNAPSHOT.md");
        var boundaryText = Normalize(File.ReadAllText(boundaryPath));

        Assert.Contains("operator prompt field", boundaryText, StringComparison.Ordinal);
        Assert.Contains("seed posture selector", boundaryText, StringComparison.Ordinal);
        Assert.Contains("inventory/evaluation posture refs", boundaryText, StringComparison.Ordinal);
        Assert.Contains("response mode", boundaryText, StringComparison.Ordinal);
        Assert.Contains("refusal/hold readout lane", boundaryText, StringComparison.Ordinal);
        Assert.Contains("The first CME response lane is a seeded readout lane.", boundaryText, StringComparison.Ordinal);
        Assert.Contains("The hosted LLM remains a future reasoning/interface participant.", boundaryText, StringComparison.Ordinal);
        Assert.Contains("add a live LLM provider call", boundaryText, StringComparison.Ordinal);
        Assert.Contains("add model-context export", boundaryText, StringComparison.Ordinal);
        Assert.Contains("add `SLI.Lisp` execution", boundaryText, StringComparison.Ordinal);
        Assert.Contains("add `RTME` movement", boundaryText, StringComparison.Ordinal);
        Assert.Contains("add Sanctuary.Actual", boundaryText, StringComparison.Ordinal);
    }

    [Fact]
    public void Public_Repo_Does_Not_Track_Executable_App_Project_For_Seed_Harness()
    {
        var repoRoot = GetRepoRoot();
        var lineRoot = Path.Combine(repoRoot, "OAN Mortalis V1.2.1");
        var solutionText = File.ReadAllText(Path.Combine(lineRoot, "San.sln"));

        Assert.False(Directory.Exists(Path.Combine(lineRoot, "src", "San", "San.FirstRun")));
        Assert.DoesNotContain("San.FirstRun", solutionText, StringComparison.Ordinal);
        Assert.DoesNotContain("San.HostedLlm", solutionText, StringComparison.Ordinal);
    }

    [Fact]
    public void Snapshot_Touched_Text_Does_Not_Contain_Private_Paths_Raw_Data_Or_Local_Manifests()
    {
        var repoRoot = GetRepoRoot();
        var lineRoot = Path.Combine(repoRoot, "OAN Mortalis V1.2.1");
        var touchedTextFiles = new[]
        {
            Path.Combine(lineRoot, "docs", "CME_SEED_HARNESS_THOUGHT_FIELD_SNAPSHOT.md"),
            Path.Combine(lineRoot, "docs", "BUILD_READINESS.md")
        };

        foreach (var path in touchedTextFiles)
        {
            var text = File.ReadAllText(path);

            Assert.DoesNotContain("Documentation Repo", text, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("Lucid Research Corpus", text, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("private_onenote", text, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("raw-local-proof-data", text, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain(".local/private_corpus_root", text, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotMatch(@"(?<![A-Za-z])[A-Za-z]:[\\/]", text);
        }
    }

    private static string Normalize(string text)
    {
        var withoutBlockQuoteMarkers = Regex.Replace(
            text,
            @"^\s*>\s?",
            string.Empty,
            RegexOptions.Multiline);
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
