using System.Text.RegularExpressions;
using Xunit;

namespace San.Audit.Tests;

public sealed class SanctuaryLocalConversationWitnessStoreBoundaryDocsTests
{
    [Fact]
    public void Witness_Store_Boundary_Locks_Governing_Sentences_And_Order()
    {
        var repoRoot = GetRepoRoot();
        var boundaryPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "SANCTUARY_LOCAL_CONVERSATION_WITNESS_STORE_BOUNDARY.md");
        var boundaryText = Normalize(File.ReadAllText(boundaryPath));

        Assert.Contains("The local store is not fuel. It is a governed witness body.", boundaryText, StringComparison.Ordinal);
        Assert.Contains("Sanctuary local conversation witness storage may preserve bounded local UTF-8 witness and receipts for site continuity; it does not create model memory, research consent, provider-visible access, `.GEL` survivor admission, RTME movement, runtime authority, or training eligibility.", boundaryText, StringComparison.Ordinal);
        Assert.Contains("stand -> remain -> move", boundaryText, StringComparison.Ordinal);
        Assert.Contains("This boundary sits after first-use enactment and before negative RTME readiness.", boundaryText, StringComparison.Ordinal);
    }

    [Fact]
    public void Witness_Store_Boundary_Locks_Flow_And_Non_Collapse_Rules()
    {
        var repoRoot = GetRepoRoot();
        var boundaryPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "SANCTUARY_LOCAL_CONVERSATION_WITNESS_STORE_BOUNDARY.md");
        var boundaryText = Normalize(File.ReadAllText(boundaryPath));

        Assert.Contains("first-use enactment -> local UTF-8 witness posture -> consent/retention posture -> local witness store posture -> receipt posture -> optional future digest/topic map -> optional future bounded rehydration package -> optional future reviewed .GEL predicate-prior candidate -> later SLI/Engrammitization/RTME only by explicit gate", boundaryText, StringComparison.Ordinal);
        Assert.Contains("local_conversation_continuity != model_memory", boundaryText, StringComparison.Ordinal);
        Assert.Contains("local_witness_retention != research_consent", boundaryText, StringComparison.Ordinal);
        Assert.Contains("context_rehydration != runtime_authority", boundaryText, StringComparison.Ordinal);
        Assert.Contains("chat_log_presence != gel_survivor_admission", boundaryText, StringComparison.Ordinal);
    }

    [Fact]
    public void Witness_Store_Boundary_Refuses_Forbidden_Shortcuts()
    {
        var repoRoot = GetRepoRoot();
        var boundaryPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "SANCTUARY_LOCAL_CONVERSATION_WITNESS_STORE_BOUNDARY.md");
        var boundaryText = Normalize(File.ReadAllText(boundaryPath));

        Assert.Contains("retention -> research use", boundaryText, StringComparison.Ordinal);
        Assert.Contains("digest -> profile", boundaryText, StringComparison.Ordinal);
        Assert.Contains("rehydration -> model memory", boundaryText, StringComparison.Ordinal);
        Assert.Contains("storage -> `.GEL` admission", boundaryText, StringComparison.Ordinal);
        Assert.Contains("storage -> RTME movement", boundaryText, StringComparison.Ordinal);
        Assert.Contains("local continuity -> provider-visible access", boundaryText, StringComparison.Ordinal);
    }

    [Fact]
    public void Threaded_Docs_Place_Witness_Store_After_Enactment_And_Before_Movement()
    {
        var repoRoot = GetRepoRoot();
        var docsRoot = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs");
        var admission = Normalize(File.ReadAllText(Path.Combine(docsRoot, "FIRST_USE_ADMISSION_AND_ENACTMENT_SPLIT_BOUNDARY.md")));
        var gelReadout = Normalize(File.ReadAllText(Path.Combine(docsRoot, "GEL_LINE_REGIONAL_SUBSTRATE_READOUT.md")));
        var readiness = Normalize(File.ReadAllText(Path.Combine(docsRoot, "BUILD_READINESS.md")));

        Assert.Contains("The `SANCTUARY_LOCAL_CONVERSATION_WITNESS_STORE_BOUNDARY.md` note sits downstream of enactment.", admission, StringComparison.Ordinal);
        Assert.Contains("The local conversation witness store boundary now governs what first use may leave behind after enactment.", gelReadout, StringComparison.Ordinal);
        Assert.Contains("post-enactment local UTF-8 witness retention as a governed witness body", readiness, StringComparison.Ordinal);
        Assert.Contains("`stand -> remain -> move`", readiness, StringComparison.Ordinal);
    }

    [Fact]
    public void Witness_Store_Boundary_Does_Not_Add_Ingestion_Logging_Sync_Export_Training_Or_Runtime_Files()
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
            Path.GetFileName(path).Contains("ConversationWitness", StringComparison.OrdinalIgnoreCase));

        Assert.DoesNotContain(sourceFiles, path => Path.GetFileName(path).Contains("ConversationIngestion", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(sourceFiles, path => Path.GetFileName(path).Contains("AutomaticLogging", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(sourceFiles, path => Path.GetFileName(path).Contains("ProviderSync", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(sourceFiles, path => Path.GetFileName(path).Contains("ResearchExport", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(sourceFiles, path => Path.GetFileName(path).Contains("TrainingEligibility", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(sourceFiles, path => Path.GetFileName(path).Contains("RehydrationPackage", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(sourceFiles, path => Path.GetFileName(path).Contains("GelPromotion", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(sourceFiles, path => Path.GetFileName(path).Contains("RtmeMovement", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(sourceFiles, path => Path.GetFileName(path).Contains("WitnessStoreService", StringComparison.OrdinalIgnoreCase));
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
