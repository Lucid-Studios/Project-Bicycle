using System.Text.RegularExpressions;
using Xunit;

namespace San.Audit.Tests;

public sealed class LabDataInventorySchemaBoundaryDocsTests
{
    [Fact]
    public void Inventory_Schema_Boundary_Locks_Governing_Sentence_Ladder_And_Non_Collapse_Rules()
    {
        var repoRoot = GetRepoRoot();
        var boundaryPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "LAB_DATA_INVENTORY_SCHEMA_BOUNDARY.md");
        var boundaryText = Normalize(File.ReadAllText(boundaryPath));

        Assert.Contains("Lab data inventory may classify documented company, personal/operator, nonprofit/society, IP/asset, witness, telemetry, and Special Case data into governed inventory posture; it does not ingest data, collect consent, expose raw content, authorize research use, train models, activate runtime, or admit RTME movement.", boundaryText, StringComparison.Ordinal);
        Assert.Contains("documented data -> metadata-only proof posture -> Lab data inventory schema -> Lab data inventory evaluation posture -> later ingestion boundary", boundaryText, StringComparison.Ordinal);
        Assert.Contains("documented != inventoried", boundaryText, StringComparison.Ordinal);
        Assert.Contains("inventoried != ingestible", boundaryText, StringComparison.Ordinal);
        Assert.Contains("inventory != ingestion", boundaryText, StringComparison.Ordinal);
        Assert.Contains("inventory != consent", boundaryText, StringComparison.Ordinal);
        Assert.Contains("owner posture != authority to use", boundaryText, StringComparison.Ordinal);
        Assert.Contains("company data != public data", boundaryText, StringComparison.Ordinal);
        Assert.Contains("personal data != research consent", boundaryText, StringComparison.Ordinal);
        Assert.Contains("nonprofit/society data != public-benefit authority", boundaryText, StringComparison.Ordinal);
        Assert.Contains("IP/asset posture != IP transfer", boundaryText, StringComparison.Ordinal);
        Assert.Contains("telemetry inventory != surveillance", boundaryText, StringComparison.Ordinal);
        Assert.Contains("Special Case inventory != handling permission", boundaryText, StringComparison.Ordinal);
    }

    [Fact]
    public void Inventory_Schema_Boundary_Names_Seven_Metadata_Only_Inventory_Classes_And_Required_Posture_Fields()
    {
        var repoRoot = GetRepoRoot();
        var boundaryPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "LAB_DATA_INVENTORY_SCHEMA_BOUNDARY.md");
        var boundaryText = Normalize(File.ReadAllText(boundaryPath));

        Assert.Contains("company data", boundaryText, StringComparison.Ordinal);
        Assert.Contains("personal/operator data", boundaryText, StringComparison.Ordinal);
        Assert.Contains("nonprofit/society data", boundaryText, StringComparison.Ordinal);
        Assert.Contains("IP/asset data", boundaryText, StringComparison.Ordinal);
        Assert.Contains("conversation witness data", boundaryText, StringComparison.Ordinal);
        Assert.Contains("operational telemetry data", boundaryText, StringComparison.Ordinal);
        Assert.Contains("Special Case/sensitive data", boundaryText, StringComparison.Ordinal);

        Assert.Contains("inventory item id", boundaryText, StringComparison.Ordinal);
        Assert.Contains("logical source label", boundaryText, StringComparison.Ordinal);
        Assert.Contains("owner or steward posture", boundaryText, StringComparison.Ordinal);
        Assert.Contains("authority-to-inventory posture", boundaryText, StringComparison.Ordinal);
        Assert.Contains("sensitivity class", boundaryText, StringComparison.Ordinal);
        Assert.Contains("consent requirement", boundaryText, StringComparison.Ordinal);
        Assert.Contains("allowed use scope", boundaryText, StringComparison.Ordinal);
        Assert.Contains("forbidden use scope", boundaryText, StringComparison.Ordinal);
        Assert.Contains("retention posture", boundaryText, StringComparison.Ordinal);
        Assert.Contains("deletion or revocation posture", boundaryText, StringComparison.Ordinal);
        Assert.Contains("visibility posture", boundaryText, StringComparison.Ordinal);
        Assert.Contains("research-separation posture", boundaryText, StringComparison.Ordinal);
        Assert.Contains("Special Case posture", boundaryText, StringComparison.Ordinal);
        Assert.Contains("IP/asset posture", boundaryText, StringComparison.Ordinal);
    }

    [Fact]
    public void Inventory_Schema_Boundary_Forbids_Ingestion_Consent_Raw_Exposure_Research_Training_Runtime_And_Rtme()
    {
        var repoRoot = GetRepoRoot();
        var boundaryPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "LAB_DATA_INVENTORY_SCHEMA_BOUNDARY.md");
        var boundaryText = Normalize(File.ReadAllText(boundaryPath));

        Assert.Contains("Each item remains metadata-only.", boundaryText, StringComparison.Ordinal);
        Assert.Contains("No item may contain raw Lab data, private examples, local manifests, private paths, or content that exposes a source body.", boundaryText, StringComparison.Ordinal);
        Assert.Contains("add local manifests", boundaryText, StringComparison.Ordinal);
        Assert.Contains("add ingestion harnesses", boundaryText, StringComparison.Ordinal);
        Assert.Contains("ingest data", boundaryText, StringComparison.Ordinal);
        Assert.Contains("expose raw content", boundaryText, StringComparison.Ordinal);
        Assert.Contains("collect consent", boundaryText, StringComparison.Ordinal);
        Assert.Contains("authorize research use", boundaryText, StringComparison.Ordinal);
        Assert.Contains("create training eligibility", boundaryText, StringComparison.Ordinal);
        Assert.Contains("create provider visibility", boundaryText, StringComparison.Ordinal);
        Assert.Contains("export model context", boundaryText, StringComparison.Ordinal);
        Assert.Contains("create surveillance", boundaryText, StringComparison.Ordinal);
        Assert.Contains("create profiles", boundaryText, StringComparison.Ordinal);
        Assert.Contains("transfer IP", boundaryText, StringComparison.Ordinal);
        Assert.Contains("permit Special Case handling", boundaryText, StringComparison.Ordinal);
        Assert.Contains("activate runtime", boundaryText, StringComparison.Ordinal);
        Assert.Contains("admit `RTME` movement", boundaryText, StringComparison.Ordinal);
    }

    [Fact]
    public void Threaded_Docs_Preserve_Proof_Inventory_Use_Ip_Witness_Legal_Admin_And_Readiness_Distinctions()
    {
        var repoRoot = GetRepoRoot();
        var docsRoot = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs");
        var proofRun = Normalize(File.ReadAllText(Path.Combine(docsRoot, "LAB_MIXED_DATA_PRE_AWAKENING_PROOF_RUN_BOUNDARY.md")));
        var cmeUseData = Normalize(File.ReadAllText(Path.Combine(docsRoot, "CME_USE_DATA_POSTURE_LAW.md")));
        var cmeIp = Normalize(File.ReadAllText(Path.Combine(docsRoot, "CME_IP_INHERITANCE_AND_CREATION_SCOPE_LAW.md")));
        var witnessStore = Normalize(File.ReadAllText(Path.Combine(docsRoot, "SANCTUARY_LOCAL_CONVERSATION_WITNESS_STORE_BOUNDARY.md")));
        var legalAdmin = Normalize(File.ReadAllText(Path.Combine(docsRoot, "LEGAL_ADMIN_LAB_ASSET_TEMPLATE_STAGING_BOUNDARY.md")));
        var readiness = Normalize(File.ReadAllText(Path.Combine(docsRoot, "BUILD_READINESS.md")));

        Assert.Contains("may classify documented company, personal/operator, nonprofit/society, IP/asset, witness, telemetry, and Special Case data by logical source label and governed inventory posture only", proofRun, StringComparison.Ordinal);
        Assert.Contains("may classify documented data as governed metadata-only inventory posture after proof posture and before any later ingestion boundary", cmeUseData, StringComparison.Ordinal);
        Assert.Contains("That inventory is not IP transfer, inheritance-scope admission, creation-use authority", cmeIp, StringComparison.Ordinal);
        Assert.Contains("may classify conversation witness data as metadata-only inventory posture after proof posture", witnessStore, StringComparison.Ordinal);
        Assert.Contains("may classify documented Lab asset and legal-admin-adjacent data by logical source label as metadata-only inventory posture", legalAdmin, StringComparison.Ordinal);
        Assert.Contains("Lab data inventory schema boundary now lives in `LAB_DATA_INVENTORY_SCHEMA_BOUNDARY.md`", readiness, StringComparison.Ordinal);
        Assert.Contains("Lab data inventory evaluation posture boundary now lives in `LAB_DATA_INVENTORY_EVALUATION_POSTURE_BOUNDARY.md`", readiness, StringComparison.Ordinal);
    }

    [Fact]
    public void Inventory_Schema_Boundary_Does_Not_Add_Ingestion_Manifest_Consent_Provider_Model_Training_Runtime_Or_Rtme_Files()
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

        Assert.DoesNotContain(sourceFiles, path => Path.GetFileName(path).Contains("LabDataIngestion", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(sourceFiles, path => Path.GetFileName(path).Contains("LocalManifest", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(sourceFiles, path => Path.GetFileName(path).Contains("RawDataLoader", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(sourceFiles, path => Path.GetFileName(path).Contains("ConsentEngine", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(sourceFiles, path => Path.GetFileName(path).Contains("ProviderSync", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(sourceFiles, path => Path.GetFileName(path).Contains("ModelContextExport", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(sourceFiles, path => Path.GetFileName(path).Contains("TelemetryEmitter", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(sourceFiles, path => Path.GetFileName(path).Contains("TrainingPath", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(sourceFiles, path => Path.GetFileName(path).Contains("RuntimeRunner", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(sourceFiles, path => Path.GetFileName(path).Contains("LabDataInventoryRtme", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void Inventory_Schema_Touched_Text_Does_Not_Contain_External_Documentation_Repo_Private_Corpus_Raw_Data_Or_Local_Paths()
    {
        var repoRoot = GetRepoRoot();
        var lineRoot = Path.Combine(repoRoot, "OAN Mortalis V1.2.1");
        var touchedTextFiles = new[]
        {
            Path.Combine(lineRoot, "docs", "LAB_DATA_INVENTORY_SCHEMA_BOUNDARY.md"),
            Path.Combine(lineRoot, "docs", "LAB_MIXED_DATA_PRE_AWAKENING_PROOF_RUN_BOUNDARY.md"),
            Path.Combine(lineRoot, "docs", "CME_USE_DATA_POSTURE_LAW.md"),
            Path.Combine(lineRoot, "docs", "CME_IP_INHERITANCE_AND_CREATION_SCOPE_LAW.md"),
            Path.Combine(lineRoot, "docs", "SANCTUARY_LOCAL_CONVERSATION_WITNESS_STORE_BOUNDARY.md"),
            Path.Combine(lineRoot, "docs", "LEGAL_ADMIN_LAB_ASSET_TEMPLATE_STAGING_BOUNDARY.md"),
            Path.Combine(lineRoot, "docs", "BUILD_READINESS.md")
        };

        foreach (var path in touchedTextFiles)
        {
            var text = File.ReadAllText(path);

            Assert.DoesNotContain("Documentation Repo", text, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("Lucid Research Corpus", text, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("private_onenote", text, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotMatch(@"\bSSN\b", text);
            Assert.DoesNotMatch(@"\bEIN\b", text);
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
