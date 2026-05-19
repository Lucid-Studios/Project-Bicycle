using System.Text.RegularExpressions;
using Xunit;

namespace San.Audit.Tests;

public sealed class LabDataInventoryEvaluationPostureBoundaryDocsTests
{
    [Fact]
    public void Evaluation_Posture_Boundary_Locks_Governing_Sentences_Ladder_And_Non_Collapse_Rules()
    {
        var repoRoot = GetRepoRoot();
        var boundaryPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "LAB_DATA_INVENTORY_EVALUATION_POSTURE_BOUNDARY.md");
        var boundaryText = Normalize(File.ReadAllText(boundaryPath));

        Assert.Contains("Lab data inventory evaluation may read metadata-only inventory posture for completeness, consistency, scope, and refusal conditions; it does not ingest data, validate raw content, collect consent, authorize use, approve research, train models, activate runtime, or admit RTME movement.", boundaryText, StringComparison.Ordinal);
        Assert.Contains("Evaluation reads inventory posture only; it does not make inventoried data ingestible.", boundaryText, StringComparison.Ordinal);
        Assert.Contains("documented data -> metadata-only proof posture -> Lab data inventory schema -> Lab data inventory evaluation posture -> later ingestion boundary", boundaryText, StringComparison.Ordinal);
        Assert.Contains("evaluation != ingestion", boundaryText, StringComparison.Ordinal);
        Assert.Contains("evaluation != consent", boundaryText, StringComparison.Ordinal);
        Assert.Contains("evaluation != authority to use", boundaryText, StringComparison.Ordinal);
        Assert.Contains("complete inventory != admissible data", boundaryText, StringComparison.Ordinal);
        Assert.Contains("consistent posture != research approval", boundaryText, StringComparison.Ordinal);
        Assert.Contains("owner/steward match != binding authority", boundaryText, StringComparison.Ordinal);
        Assert.Contains("allowed-use readout != use grant", boundaryText, StringComparison.Ordinal);
        Assert.Contains("retention readout != retention activation", boundaryText, StringComparison.Ordinal);
        Assert.Contains("Special Case evaluation != handling permission", boundaryText, StringComparison.Ordinal);
        Assert.Contains("RTME refusal != RTME readiness", boundaryText, StringComparison.Ordinal);
    }

    [Fact]
    public void Evaluation_Posture_Boundary_Names_Readouts_And_Results_As_Readout_Only()
    {
        var repoRoot = GetRepoRoot();
        var boundaryPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "LAB_DATA_INVENTORY_EVALUATION_POSTURE_BOUNDARY.md");
        var boundaryText = Normalize(File.ReadAllText(boundaryPath));

        Assert.Contains("source inventory item ref", boundaryText, StringComparison.Ordinal);
        Assert.Contains("completeness posture", boundaryText, StringComparison.Ordinal);
        Assert.Contains("consistency posture", boundaryText, StringComparison.Ordinal);
        Assert.Contains("scope posture", boundaryText, StringComparison.Ordinal);
        Assert.Contains("consent requirement readout", boundaryText, StringComparison.Ordinal);
        Assert.Contains("retention/deletion readout", boundaryText, StringComparison.Ordinal);
        Assert.Contains("visibility readout", boundaryText, StringComparison.Ordinal);
        Assert.Contains("Special Case readout", boundaryText, StringComparison.Ordinal);
        Assert.Contains("`ReadableAsInventoryOnly` means inventory posture can be read as metadata-only posture", boundaryText, StringComparison.Ordinal);
        Assert.Contains("`HeldForEvaluationReview` means one or more completeness, consistency, scope, retention, visibility, or Special Case questions remain held", boundaryText, StringComparison.Ordinal);
        Assert.Contains("`RefusedAsIngestibleOrActiveUse` means an inventory item is missing required posture or overclaims ingestion, raw validation, consent, research use, training, provider visibility, model context, runtime, or `RTME`.", boundaryText, StringComparison.Ordinal);
        Assert.Contains("No result authorizes ingestion, use, consent, research approval, training, model context, provider visibility, runtime activation, or `RTME` movement.", boundaryText, StringComparison.Ordinal);
    }

    [Fact]
    public void Threaded_Docs_Preserve_Schema_Proof_Data_Witness_And_Readiness_Distinctions()
    {
        var repoRoot = GetRepoRoot();
        var docsRoot = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs");
        var schema = Normalize(File.ReadAllText(Path.Combine(docsRoot, "LAB_DATA_INVENTORY_SCHEMA_BOUNDARY.md")));
        var proofRun = Normalize(File.ReadAllText(Path.Combine(docsRoot, "LAB_MIXED_DATA_PRE_AWAKENING_PROOF_RUN_BOUNDARY.md")));
        var cmeUseData = Normalize(File.ReadAllText(Path.Combine(docsRoot, "CME_USE_DATA_POSTURE_LAW.md")));
        var witnessStore = Normalize(File.ReadAllText(Path.Combine(docsRoot, "SANCTUARY_LOCAL_CONVERSATION_WITNESS_STORE_BOUNDARY.md")));
        var readiness = Normalize(File.ReadAllText(Path.Combine(docsRoot, "BUILD_READINESS.md")));

        Assert.Contains("may read this inventory posture for completeness, consistency, scope, and refusal conditions", schema, StringComparison.Ordinal);
        Assert.Contains("may then read inventory posture for completeness, consistency, scope, and refusal conditions as inventory-only readout", proofRun, StringComparison.Ordinal);
        Assert.Contains("Evaluation does not change `CoreCmeUsePostureRecord`, make inventoried data ingestible, authorize use, approve research, collect consent, train models", cmeUseData, StringComparison.Ordinal);
        Assert.Contains("may read conversation witness inventory posture for completeness, consistency, scope, retention/deletion, visibility, and Special Case refusal conditions only", witnessStore, StringComparison.Ordinal);
        Assert.Contains("passive completeness, consistency, scope, and refusal readout over metadata-only inventory posture", readiness, StringComparison.Ordinal);
    }

    [Fact]
    public void Evaluation_Posture_Boundary_Does_Not_Add_Active_Evaluator_Ingestion_Raw_Loader_Consent_Provider_Model_Runtime_Or_Rtme_Files()
    {
        var repoRoot = GetRepoRoot();
        var lineRoot = Path.Combine(repoRoot, "OAN Mortalis V1.2.1");
        var sourceFiles = Directory
            .EnumerateFiles(Path.Combine(lineRoot, "src"), "*.cs", SearchOption.AllDirectories)
            .Where(path =>
                !path.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}", StringComparison.OrdinalIgnoreCase) &&
                !path.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}", StringComparison.OrdinalIgnoreCase))
            .ToArray();

        Assert.DoesNotContain(sourceFiles, path =>
            path.Contains($"{Path.DirectorySeparatorChar}San.Nexus.Control{Path.DirectorySeparatorChar}", StringComparison.OrdinalIgnoreCase) &&
            Path.GetFileName(path).Contains("LabDataInventory", StringComparison.OrdinalIgnoreCase));

        Assert.DoesNotContain(sourceFiles, path => Path.GetFileName(path).Contains("LabDataInventoryEvaluator", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(sourceFiles, path => Path.GetFileName(path).Contains("LabDataInventoryService", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(sourceFiles, path => Path.GetFileName(path).Contains("LabDataIngestion", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(sourceFiles, path => Path.GetFileName(path).Contains("RawDataLoader", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(sourceFiles, path => Path.GetFileName(path).Contains("ConsentEngine", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(sourceFiles, path => Path.GetFileName(path).Contains("ProviderSync", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(sourceFiles, path => Path.GetFileName(path).Contains("ModelContextExport", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(sourceFiles, path => Path.GetFileName(path).Contains("RuntimeRunner", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(sourceFiles, path => Path.GetFileName(path).Contains("LabDataInventoryRtme", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void Evaluation_Posture_Touched_Text_Does_Not_Contain_Private_Paths_Raw_Local_Data_Or_Local_Manifests()
    {
        var repoRoot = GetRepoRoot();
        var lineRoot = Path.Combine(repoRoot, "OAN Mortalis V1.2.1");
        var touchedTextFiles = new[]
        {
            Path.Combine(lineRoot, "docs", "LAB_DATA_INVENTORY_EVALUATION_POSTURE_BOUNDARY.md"),
            Path.Combine(lineRoot, "docs", "LAB_DATA_INVENTORY_SCHEMA_BOUNDARY.md"),
            Path.Combine(lineRoot, "docs", "LAB_MIXED_DATA_PRE_AWAKENING_PROOF_RUN_BOUNDARY.md"),
            Path.Combine(lineRoot, "docs", "CME_USE_DATA_POSTURE_LAW.md"),
            Path.Combine(lineRoot, "docs", "SANCTUARY_LOCAL_CONVERSATION_WITNESS_STORE_BOUNDARY.md"),
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
