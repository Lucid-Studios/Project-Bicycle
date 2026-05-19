using System.Text.RegularExpressions;
using Xunit;

namespace San.Audit.Tests;

public sealed class GoverningPrimeCrypticTemplateStructureBoundaryDocsTests
{
    [Fact]
    public void Template_Structure_Boundary_Locks_Governing_Sentences_And_Line()
    {
        var repoRoot = GetRepoRoot();
        var boundaryPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "GOVERNING_PRIME_CRYPTIC_TEMPLATE_STRUCTURE_BOUNDARY.md");
        var boundaryText = Normalize(File.ReadAllText(boundaryPath));

        Assert.Contains("Governing.Prime and Governing.Cryptic template structure may define candidate witness, telemetry, binder, handshake, and pairing posture for future governed user-data predicate questions; it does not collect user data, create consent, profile users, train models, issue cryptographic authority, activate governing CME, execute SLI.Lisp, admit RTME movement, or grant runtime control.", boundaryText, StringComparison.Ordinal);
        Assert.Contains("User data collection may be predicated by template posture only; no user data may be collected until a later consent, disclosure, retention, and startup boundary separately admits it.", boundaryText, StringComparison.Ordinal);
        Assert.Contains("Sanctuary.GEL to MoS/cMoS seed substrate -> Governing.Prime template posture -> Governing.Cryptic template posture -> paired Prime/Cryptic template receipt -> user-data predicate classes -> later consent/startup conditions -> later governed CME activation attempt", boundaryText, StringComparison.Ordinal);
    }

    [Fact]
    public void Template_Structure_Boundary_Names_Offices_And_User_Data_Predicate_Classes()
    {
        var repoRoot = GetRepoRoot();
        var boundaryPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "GOVERNING_PRIME_CRYPTIC_TEMPLATE_STRUCTURE_BOUNDARY.md");
        var boundaryText = Normalize(File.ReadAllText(boundaryPath));

        Assert.Contains("`Governing.Prime` is readable witness and telemetry template posture.", boundaryText, StringComparison.Ordinal);
        Assert.Contains("`Governing.Cryptic` is cryptic binder and handshake template posture.", boundaryText, StringComparison.Ordinal);
        Assert.Contains("Paired Prime/Cryptic template posture is a receipted candidate pairing.", boundaryText, StringComparison.Ordinal);
        Assert.Contains("entity posture", boundaryText, StringComparison.Ordinal);
        Assert.Contains("authority-to-bind posture", boundaryText, StringComparison.Ordinal);
        Assert.Contains("consent scope", boundaryText, StringComparison.Ordinal);
        Assert.Contains("local data category", boundaryText, StringComparison.Ordinal);
        Assert.Contains("retention/opt-out", boundaryText, StringComparison.Ordinal);
        Assert.Contains("research separation", boundaryText, StringComparison.Ordinal);
        Assert.Contains("Special Case quarantine", boundaryText, StringComparison.Ordinal);
        Assert.Contains("IP/asset posture", boundaryText, StringComparison.Ordinal);
        Assert.Contains("operational telemetry posture", boundaryText, StringComparison.Ordinal);
        Assert.Contains("non-authority posture", boundaryText, StringComparison.Ordinal);
    }

    [Fact]
    public void Template_Structure_Boundary_Refuses_Collection_Consent_Profile_Training_Crypto_Rtme_And_Runtime()
    {
        var repoRoot = GetRepoRoot();
        var boundaryPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "GOVERNING_PRIME_CRYPTIC_TEMPLATE_STRUCTURE_BOUNDARY.md");
        var boundaryText = Normalize(File.ReadAllText(boundaryPath));

        Assert.Contains("template posture != user-data collection", boundaryText, StringComparison.Ordinal);
        Assert.Contains("predicate class != consent", boundaryText, StringComparison.Ordinal);
        Assert.Contains("witness telemetry != surveillance", boundaryText, StringComparison.Ordinal);
        Assert.Contains("receipt != profile", boundaryText, StringComparison.Ordinal);
        Assert.Contains("retention predicate != training", boundaryText, StringComparison.Ordinal);
        Assert.Contains("Prime template != runtime control", boundaryText, StringComparison.Ordinal);
        Assert.Contains("Cryptic template != active cryptography", boundaryText, StringComparison.Ordinal);
        Assert.Contains("paired template receipt != key authority", boundaryText, StringComparison.Ordinal);
        Assert.Contains("user-data predicate != research use", boundaryText, StringComparison.Ordinal);
        Assert.Contains("governed question shape != governed CME activation", boundaryText, StringComparison.Ordinal);
        Assert.Contains("No user data may be collected", boundaryText, StringComparison.Ordinal);
    }

    [Fact]
    public void Threaded_Docs_Preserve_Seed_Template_Data_Consent_And_Activation_Distinctions()
    {
        var repoRoot = GetRepoRoot();
        var docsRoot = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs");
        var seed = Normalize(File.ReadAllText(Path.Combine(docsRoot, "SANCTUARY_GEL_TO_MOS_CMOS_SEED_SUBSTRATE_BOUNDARY.md")));
        var cmeUseData = Normalize(File.ReadAllText(Path.Combine(docsRoot, "CME_USE_DATA_POSTURE_LAW.md")));
        var witnessStore = Normalize(File.ReadAllText(Path.Combine(docsRoot, "SANCTUARY_LOCAL_CONVERSATION_WITNESS_STORE_BOUNDARY.md")));
        var goa = Normalize(File.ReadAllText(Path.Combine(docsRoot, "SANCTUARYID_GOA_GOVERNING_CME_SET_LAW.md")));
        var oeSelfGel = Normalize(File.ReadAllText(Path.Combine(docsRoot, "OE_AND_SELFGEL_STRUCTURAL_STANDING_LAW.md")));
        var readiness = Normalize(File.ReadAllText(Path.Combine(docsRoot, "BUILD_READINESS.md")));

        Assert.Contains("sits above this seed substrate as passive template posture", seed, StringComparison.Ordinal);
        Assert.Contains("predicate classes do not collect user data, create consent, admit research use, profile users, train models", cmeUseData, StringComparison.Ordinal);
        Assert.Contains("question-shaping templates only; they do not collect conversation data, create consent, widen retention, generate profiles", witnessStore, StringComparison.Ordinal);
        Assert.Contains("template posture remains below active governing `CME` office and does not activate the `SanctuaryID.GoA` governing set", goa, StringComparison.Ordinal);
        Assert.Contains("may shape future OE/SelfGEL user-data predicate questions, but it does not seat OE or SelfGEL as active governing life and does not form `CME`", oeSelfGel, StringComparison.Ordinal);
        Assert.Contains("candidate witness, telemetry, binder, handshake, pairing, and user-data predicate posture without collecting user data", readiness, StringComparison.Ordinal);
    }

    [Fact]
    public void Template_Structure_Boundary_Does_Not_Add_Active_Data_Consent_Key_Execution_Or_Runtime_Files()
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
            Path.GetFileName(path).Contains("PrimeCrypticTemplate", StringComparison.OrdinalIgnoreCase));

        Assert.DoesNotContain(sourceFiles, path => Path.GetFileName(path).Contains("ConsentEngine", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(sourceFiles, path => Path.GetFileName(path).Contains("DataCollector", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(sourceFiles, path => Path.GetFileName(path).Contains("TelemetryEmitter", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(sourceFiles, path => Path.GetFileName(path).Contains("ProfileBuilder", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(sourceFiles, path => Path.GetFileName(path).Contains("TrainingPath", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(sourceFiles, path => Path.GetFileName(path).Contains("KeyGenerator", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(sourceFiles, path => Path.GetFileName(path).Contains("EncryptionRuntime", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(sourceFiles, path => Path.GetFileName(path).Contains("StartupRunner", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(sourceFiles, path => Path.GetFileName(path).Contains("SliLispExecutor", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(sourceFiles, path => Path.GetFileName(path).Contains("RtmeMovement", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(sourceFiles, path => Path.GetFileName(path).Contains("GovernedCmeActivation", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void Template_Structure_Touched_Text_Does_Not_Contain_External_Documentation_Repo_Or_Private_Corpus_Paths()
    {
        var repoRoot = GetRepoRoot();
        var lineRoot = Path.Combine(repoRoot, "OAN Mortalis V1.2.1");
        var touchedTextFiles = new[]
        {
            Path.Combine(lineRoot, "docs", "GOVERNING_PRIME_CRYPTIC_TEMPLATE_STRUCTURE_BOUNDARY.md"),
            Path.Combine(lineRoot, "docs", "SANCTUARY_GEL_TO_MOS_CMOS_SEED_SUBSTRATE_BOUNDARY.md"),
            Path.Combine(lineRoot, "docs", "CME_USE_DATA_POSTURE_LAW.md"),
            Path.Combine(lineRoot, "docs", "SANCTUARY_LOCAL_CONVERSATION_WITNESS_STORE_BOUNDARY.md"),
            Path.Combine(lineRoot, "docs", "SANCTUARYID_GOA_GOVERNING_CME_SET_LAW.md"),
            Path.Combine(lineRoot, "docs", "OE_AND_SELFGEL_STRUCTURAL_STANDING_LAW.md"),
            Path.Combine(lineRoot, "docs", "BUILD_READINESS.md")
        };

        foreach (var path in touchedTextFiles)
        {
            var text = File.ReadAllText(path);

            Assert.DoesNotContain("Documentation Repo", text, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("Lucid Research Corpus", text, StringComparison.OrdinalIgnoreCase);
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
