using System.Text.RegularExpressions;
using Xunit;

namespace San.Audit.Tests;

public sealed class LabMixedDataPreAwakeningProofRunBoundaryDocsTests
{
    [Fact]
    public void Proof_Run_Boundary_Locks_Governing_Sentence_And_Ladder()
    {
        var repoRoot = GetRepoRoot();
        var boundaryPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "LAB_MIXED_DATA_PRE_AWAKENING_PROOF_RUN_BOUNDARY.md");
        var boundaryText = Normalize(File.ReadAllText(boundaryPath));

        Assert.Contains("Real mixed Lab data may be represented by local-only metadata, refs, hashes, summaries, receipts, and posture for pre-awakening proof; it must not become consent, training fuel, surveillance, RTME movement, governance, model context, raw-content exposure, or runtime authority.", boundaryText, StringComparison.Ordinal);
        Assert.Contains("local Lab data manifest -> National/Regional/Local predicate context refs -> governed user-data predicate template match -> payload classification posture -> consent/disclosure/retention requirement readout -> opt-in/opt-out/revocation posture -> Special Case quarantine -> Lab seed inheritance ref -> pre-activation legitimacy posture -> startup attempt eligibility readout -> activation held/refused by design", boundaryText, StringComparison.Ordinal);
        Assert.Contains("Success is proving the system can meet mixed Lab data and refuse misuse.", boundaryText, StringComparison.Ordinal);
    }

    [Fact]
    public void Proof_Run_Boundary_Names_Metadata_Only_Datum_Kinds_And_Proof_Receipts()
    {
        var repoRoot = GetRepoRoot();
        var boundaryPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "LAB_MIXED_DATA_PRE_AWAKENING_PROOF_RUN_BOUNDARY.md");
        var boundaryText = Normalize(File.ReadAllText(boundaryPath));

        Assert.Contains("personal/operator", boundaryText, StringComparison.Ordinal);
        Assert.Contains("private Lab/business", boundaryText, StringComparison.Ordinal);
        Assert.Contains("IP/asset", boundaryText, StringComparison.Ordinal);
        Assert.Contains("conversation witness", boundaryText, StringComparison.Ordinal);
        Assert.Contains("operational telemetry", boundaryText, StringComparison.Ordinal);
        Assert.Contains("Special Case/sensitive held", boundaryText, StringComparison.Ordinal);
        Assert.Contains("No datum kind may carry raw content in tracked Build files.", boundaryText, StringComparison.Ordinal);
        Assert.Contains("No datum kind may be exposed to model context.", boundaryText, StringComparison.Ordinal);

        Assert.Contains("data manifest receipt", boundaryText, StringComparison.Ordinal);
        Assert.Contains("predicate context receipt", boundaryText, StringComparison.Ordinal);
        Assert.Contains("payload classification receipt", boundaryText, StringComparison.Ordinal);
        Assert.Contains("consent requirement receipt", boundaryText, StringComparison.Ordinal);
        Assert.Contains("retention/opt-out receipt", boundaryText, StringComparison.Ordinal);
        Assert.Contains("Special Case quarantine receipt", boundaryText, StringComparison.Ordinal);
        Assert.Contains("Lab seed inheritance receipt", boundaryText, StringComparison.Ordinal);
        Assert.Contains("pre-activation legitimacy receipt", boundaryText, StringComparison.Ordinal);
        Assert.Contains("startup attempt hold/refusal receipt", boundaryText, StringComparison.Ordinal);
    }

    [Fact]
    public void Proof_Run_Boundary_Defines_Held_For_Proof_As_Success_Not_Activation()
    {
        var repoRoot = GetRepoRoot();
        var boundaryPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "LAB_MIXED_DATA_PRE_AWAKENING_PROOF_RUN_BOUNDARY.md");
        var boundaryText = Normalize(File.ReadAllText(boundaryPath));

        Assert.Contains("The expected result is not `Activated`.", boundaryText, StringComparison.Ordinal);
        Assert.Contains("No other proof-result state is admitted.", boundaryText, StringComparison.Ordinal);
        Assert.Contains("`HeldForProof` is the first success posture.", boundaryText, StringComparison.Ordinal);
        Assert.Contains("activation remains held or refused by design", boundaryText, StringComparison.Ordinal);
        Assert.Contains("`RefusedUntilConsentAndStartupAdmission` also catches any attempt to convert proof posture into data collection, consent, training, surveillance, model context, `RTME`, `SLI.Lisp`, governance, mutation, or runtime control.", boundaryText, StringComparison.Ordinal);
        Assert.Contains("template did not collect", boundaryText, StringComparison.Ordinal);
        Assert.Contains("payload did not imply consent", boundaryText, StringComparison.Ordinal);
        Assert.Contains("telemetry did not profile", boundaryText, StringComparison.Ordinal);
        Assert.Contains("retention did not train", boundaryText, StringComparison.Ordinal);
        Assert.Contains("Special Case content remained quarantined", boundaryText, StringComparison.Ordinal);
        Assert.Contains("startup readiness did not wake runtime", boundaryText, StringComparison.Ordinal);
        Assert.Contains("`RTME` remained inactive", boundaryText, StringComparison.Ordinal);
        Assert.Contains("`SLI.Lisp` did not execute", boundaryText, StringComparison.Ordinal);
        Assert.Contains("Prime/Cryptic did not mutate", boundaryText, StringComparison.Ordinal);
    }

    [Fact]
    public void Proof_Run_Boundary_Forbids_Activation_And_Runtime_Result_Labels()
    {
        var repoRoot = GetRepoRoot();
        var boundaryPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "LAB_MIXED_DATA_PRE_AWAKENING_PROOF_RUN_BOUNDARY.md");
        var boundaryText = Normalize(File.ReadAllText(boundaryPath));

        Assert.Contains("Activated", boundaryText, StringComparison.Ordinal);
        Assert.Contains("StartupAdmitted", boundaryText, StringComparison.Ordinal);
        Assert.Contains("RuntimeReady", boundaryText, StringComparison.Ordinal);
        Assert.Contains("RTMEMovementAllowed", boundaryText, StringComparison.Ordinal);
        Assert.Contains("SLILispExecuted", boundaryText, StringComparison.Ordinal);
        Assert.Contains("PrimeMutated", boundaryText, StringComparison.Ordinal);
        Assert.Contains("CrypticMutated", boundaryText, StringComparison.Ordinal);
        Assert.Contains("ResearchUseGranted", boundaryText, StringComparison.Ordinal);
        Assert.Contains("TrainingEligible", boundaryText, StringComparison.Ordinal);
        Assert.Contains("GELSurvivorAdmitted", boundaryText, StringComparison.Ordinal);
    }

    [Fact]
    public void Threaded_Docs_Preserve_Template_Witness_Non_Activation_Data_And_Readiness_Distinctions()
    {
        var repoRoot = GetRepoRoot();
        var docsRoot = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs");
        var template = Normalize(File.ReadAllText(Path.Combine(docsRoot, "GOVERNING_PRIME_CRYPTIC_TEMPLATE_STRUCTURE_BOUNDARY.md")));
        var witnessStore = Normalize(File.ReadAllText(Path.Combine(docsRoot, "SANCTUARY_LOCAL_CONVERSATION_WITNESS_STORE_BOUNDARY.md")));
        var nonActivation = Normalize(File.ReadAllText(Path.Combine(docsRoot, "GEL_RTME_SLI_LISP_NON_ACTIVATION_READINESS_BOUNDARY.md")));
        var cmeUseData = Normalize(File.ReadAllText(Path.Combine(docsRoot, "CME_USE_DATA_POSTURE_LAW.md")));
        var readiness = Normalize(File.ReadAllText(Path.Combine(docsRoot, "BUILD_READINESS.md")));

        Assert.Contains("may use this template posture as a governed user-data predicate template match for metadata-only proof", template, StringComparison.Ordinal);
        Assert.Contains("may represent a conversation witness datum as local-only metadata for proof posture", witnessStore, StringComparison.Ordinal);
        Assert.Contains("mixed Lab metadata can pass through pre-awakening posture while activation is held or refused by design", nonActivation, StringComparison.Ordinal);
        Assert.Contains("may represent a mixed Lab data manifest as metadata-only proof posture", cmeUseData, StringComparison.Ordinal);
        Assert.Contains("representing real mixed Lab data by local-only metadata, refs, hashes, summaries, receipts, and posture", readiness, StringComparison.Ordinal);
    }

    [Fact]
    public void Proof_Run_Boundary_Does_Not_Add_Runner_Ingestion_Startup_Activation_Model_Context_Telemetry_Rtme_Or_Sli_Files()
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
            Path.GetFileName(path).Contains("ProofRun", StringComparison.OrdinalIgnoreCase));

        Assert.DoesNotContain(sourceFiles, path => Path.GetFileName(path).Contains("ProofRunRunner", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(sourceFiles, path => Path.GetFileName(path).Contains("LabDataIngestion", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(sourceFiles, path => Path.GetFileName(path).Contains("StartupRunner", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(sourceFiles, path => Path.GetFileName(path).Contains("ActivationAttemptService", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(sourceFiles, path => Path.GetFileName(path).Contains("ModelContextPackage", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(sourceFiles, path => Path.GetFileName(path).Contains("TelemetryEmitter", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(sourceFiles, path => Path.GetFileName(path).Contains("RtmeRunner", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(sourceFiles, path => Path.GetFileName(path).Contains("SliLispExecutor", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void Proof_Run_Touched_Text_Does_Not_Contain_External_Documentation_Repo_Private_Corpus_Or_Raw_Local_Data_Paths()
    {
        var repoRoot = GetRepoRoot();
        var lineRoot = Path.Combine(repoRoot, "OAN Mortalis V1.2.1");
        var touchedTextFiles = new[]
        {
            Path.Combine(lineRoot, "docs", "LAB_MIXED_DATA_PRE_AWAKENING_PROOF_RUN_BOUNDARY.md"),
            Path.Combine(lineRoot, "docs", "GOVERNING_PRIME_CRYPTIC_TEMPLATE_STRUCTURE_BOUNDARY.md"),
            Path.Combine(lineRoot, "docs", "SANCTUARY_LOCAL_CONVERSATION_WITNESS_STORE_BOUNDARY.md"),
            Path.Combine(lineRoot, "docs", "GEL_RTME_SLI_LISP_NON_ACTIVATION_READINESS_BOUNDARY.md"),
            Path.Combine(lineRoot, "docs", "CME_USE_DATA_POSTURE_LAW.md"),
            Path.Combine(lineRoot, "docs", "BUILD_READINESS.md")
        };

        foreach (var path in touchedTextFiles)
        {
            var text = File.ReadAllText(path);

            Assert.DoesNotContain("Documentation Repo", text, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("Lucid Research Corpus", text, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("raw-local-proof-data", text, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotMatch(@"[A-Za-z]:[\\/]", text);
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
