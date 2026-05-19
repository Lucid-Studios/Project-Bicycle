using System.Text.RegularExpressions;
using Xunit;

namespace San.Audit.Tests;

public sealed class GelLineRegionalSubstrateReadoutDocsTests
{
    [Fact]
    public void Gel_Line_Readout_Locks_Governing_Sentence_And_Current_Line()
    {
        var repoRoot = GetRepoRoot();
        var readoutPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "GEL_LINE_REGIONAL_SUBSTRATE_READOUT.md");
        var readoutText = Normalize(File.ReadAllText(readoutPath));

        Assert.Contains("`Sanctuary.GEL` is a prerequisite regional substrate for later Sanctuary.Actual and Cradle.GEL work; it is not itself a governing actor, runtime body, model-selection surface, or CradleTek-local GEL.", readoutText, StringComparison.Ordinal);
        Assert.Contains("GEL readiness -> GEL predicate prior -> Sanctuary.GEL regional substrate -> Sanctuary pre-governing standing -> first-use eligibility", readoutText, StringComparison.Ordinal);
        Assert.Contains("local conversation witness store boundary", readoutText, StringComparison.Ordinal);
        Assert.Contains("much later RTME admission -> later Sanctuary.Actual Mother/Father governing CME -> later Cradle.GEL generation", readoutText, StringComparison.Ordinal);
    }

    [Fact]
    public void Gel_Line_Readout_Names_Seated_Layers_From_Witness_To_First_Use_Eligibility()
    {
        var repoRoot = GetRepoRoot();
        var readoutPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "GEL_LINE_REGIONAL_SUBSTRATE_READOUT.md");
        var readoutText = Normalize(File.ReadAllText(readoutPath));

        Assert.Contains("UTF-8 witness remains the first lawful witness field.", readoutText, StringComparison.Ordinal);
        Assert.Contains("RootAtlas provides root predicate identity.", readoutText, StringComparison.Ordinal);
        Assert.Contains("`SLI` constructors are structured and attachment-native.", readoutText, StringComparison.Ordinal);
        Assert.Contains("`GEL = Generative Engrammitization Library` readiness may hold rooted, witnessed, `SLI`-formed, Engrammitization-facing candidates.", readoutText, StringComparison.Ordinal);
        Assert.Contains("`.GEL` predicate-prior formalization records structured candidate-only priors.", readoutText, StringComparison.Ordinal);
        Assert.Contains("The localized pre-certification data pool is representational and non-authorizing.", readoutText, StringComparison.Ordinal);
        Assert.Contains("Localized `Sanctuary.GEL` formation remains the constitutional floor.", readoutText, StringComparison.Ordinal);
        Assert.Contains("`Sanctuary.GEL` regional substrate formation names the specific regional/root local GEL substrate for Sanctuary.", readoutText, StringComparison.Ordinal);
        Assert.Contains("Sanctuary pre-governing standing admits local meaning without governance.", readoutText, StringComparison.Ordinal);
        Assert.Contains("First-use eligibility only permits consideration.", readoutText, StringComparison.Ordinal);
    }

    [Fact]
    public void Gel_Line_Readout_Refuses_Collapse_And_Keeps_Horizons_Withheld()
    {
        var repoRoot = GetRepoRoot();
        var readoutPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "GEL_LINE_REGIONAL_SUBSTRATE_READOUT.md");
        var readoutText = Normalize(File.ReadAllText(readoutPath));

        Assert.Contains("treating RootAtlas grounding as local Atlas authority", readoutText, StringComparison.Ordinal);
        Assert.Contains("treating predicate-prior formalization as survivor admission", readoutText, StringComparison.Ordinal);
        Assert.Contains("treating `Sanctuary.GEL` regional substrate as Sanctuary.Actual", readoutText, StringComparison.Ordinal);
        Assert.Contains("treating first-use eligibility as admission or enactment", readoutText, StringComparison.Ordinal);
        Assert.Contains("treating first-use admission as enactment", readoutText, StringComparison.Ordinal);
        Assert.Contains("treating first-use enactment as runtime transaction authority", readoutText, StringComparison.Ordinal);
        Assert.Contains("treating local conversation continuity as model memory", readoutText, StringComparison.Ordinal);
        Assert.Contains("treating local witness retention as research consent", readoutText, StringComparison.Ordinal);
        Assert.Contains("treating context rehydration as runtime authority", readoutText, StringComparison.Ordinal);
        Assert.Contains("treating chat log presence as `.GEL` survivor admission", readoutText, StringComparison.Ordinal);
        Assert.Contains("Two upward paths remain bounded.", readoutText, StringComparison.Ordinal);
        Assert.Contains("The first-use path now has a passive admission/enactment split.", readoutText, StringComparison.Ordinal);
        Assert.Contains("The governance approach path would define how a standing `Sanctuary.GEL` regional substrate may approach Sanctuary.Actual Mother/Father governing `CME` formation.", readoutText, StringComparison.Ordinal);
        Assert.Contains("This readout chooses neither path.", readoutText, StringComparison.Ordinal);
    }

    [Fact]
    public void Threaded_Docs_Name_Readout_As_Non_Authoritative()
    {
        var repoRoot = GetRepoRoot();
        var docsRoot = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs");
        var regionalSubstrate = Normalize(File.ReadAllText(Path.Combine(docsRoot, "SANCTUARY_GEL_REGIONAL_SUBSTRATE_FORMATION_BOUNDARY.md")));
        var readiness = Normalize(File.ReadAllText(Path.Combine(docsRoot, "BUILD_READINESS.md")));

        Assert.Contains("reads the current `.GEL` line end to end without adding authority, first-use admission, Sanctuary.Actual approach, model selection, runtime, or Cradle.GEL generation", regionalSubstrate, StringComparison.Ordinal);
        Assert.Contains("consolidates the current line from UTF-8 witness through first-use eligibility without adding first-use admission, Sanctuary.Actual approach, model selection, runtime, or Cradle.GEL generation", readiness, StringComparison.Ordinal);
        Assert.Contains("while keeping first-use admission/enactment and Sanctuary.Actual approach withheld", readiness, StringComparison.Ordinal);
    }

    [Fact]
    public void Gel_Line_Readout_Does_Not_Add_Contracts_Services_Or_Runtime_Files()
    {
        var repoRoot = GetRepoRoot();
        var lineRoot = Path.Combine(repoRoot, "OAN Mortalis V1.2.1");
        var srcRoot = Path.Combine(lineRoot, "src");

        var sourceFiles = Directory
            .EnumerateFiles(srcRoot, "*.cs", SearchOption.AllDirectories)
            .Where(path =>
                !path.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}", StringComparison.OrdinalIgnoreCase) &&
                !path.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}", StringComparison.OrdinalIgnoreCase))
            .ToArray();

        Assert.DoesNotContain(sourceFiles, path => Path.GetFileName(path).Contains("GelLineRegionalSubstrateReadout", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(sourceFiles, path => Path.GetFileName(path).Contains("SanctuaryActual", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(sourceFiles, path => Path.GetFileName(path).Contains("CradleGelGeneration", StringComparison.OrdinalIgnoreCase));
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
