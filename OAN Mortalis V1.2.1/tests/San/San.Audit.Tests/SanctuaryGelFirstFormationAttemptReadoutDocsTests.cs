using System.Text.RegularExpressions;
using Xunit;

namespace San.Audit.Tests;

public sealed class SanctuaryGelFirstFormationAttemptReadoutDocsTests
{
    [Fact]
    public void First_Formation_Attempt_Readout_Locks_Governing_Sentence_And_Chain()
    {
        var repoRoot = GetRepoRoot();
        var readoutPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "SANCTUARY_GEL_FIRST_FORMATION_ATTEMPT_READOUT.md");
        var readoutText = Normalize(File.ReadAllText(readoutPath));

        Assert.Contains("First `Sanctuary.GEL` formation attempt may report coherence across seated prerequisite bodies; it does not convert coherence into survivor admission, first-use admission, Sanctuary.Actual governance, runtime activation, or Cradle.GEL generation.", readoutText, StringComparison.Ordinal);
        Assert.Contains("GEL readiness -> GEL predicate prior -> localized pre-certification data pool -> localized formation floor -> Sanctuary.GEL regional substrate -> pre-governing standing -> first-use eligibility consideration -> first Sanctuary.GEL formation attempt", readoutText, StringComparison.Ordinal);
    }

    [Fact]
    public void First_Formation_Attempt_Readout_Defines_Dispositions_As_Readout_Only()
    {
        var repoRoot = GetRepoRoot();
        var readoutPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "SANCTUARY_GEL_FIRST_FORMATION_ATTEMPT_READOUT.md");
        var readoutText = Normalize(File.ReadAllText(readoutPath));

        Assert.Contains("`Ready` means the attempt coheres as an attempt only.", readoutText, StringComparison.Ordinal);
        Assert.Contains("`Held` means the attempt has recognizable represented structure, but one or more local, domain, Special Case, counsel, regional, or governance questions remain held.", readoutText, StringComparison.Ordinal);
        Assert.Contains("`Refused` means a missing prerequisite or overclaim prevents the attempt from standing.", readoutText, StringComparison.Ordinal);
        Assert.Contains("No disposition admits survivor standing, grants first use, selects a model, activates runtime, executes `SLI.Lisp`, invokes `RTME`, forms Sanctuary.Actual, or generates Cradle.GEL.", readoutText, StringComparison.Ordinal);
    }

    [Fact]
    public void First_Formation_Attempt_Readout_Preserves_Withheld_Horizons()
    {
        var repoRoot = GetRepoRoot();
        var readoutPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "SANCTUARY_GEL_FIRST_FORMATION_ATTEMPT_READOUT.md");
        var readoutText = Normalize(File.ReadAllText(readoutPath));

        Assert.Contains("The first-use admission and enactment split is the next passive seam above this readout.", readoutText, StringComparison.Ordinal);
        Assert.Contains("That split distinguishes first-use admission from first-use enactment.", readoutText, StringComparison.Ordinal);
        Assert.Contains("The governance approach path remains withheld.", readoutText, StringComparison.Ordinal);
        Assert.Contains("The recommended next seam after this readout is the first-use admission/enactment split, now seated separately as a passive boundary.", readoutText, StringComparison.Ordinal);
        Assert.Contains("This readout itself does not add that seam.", readoutText, StringComparison.Ordinal);
    }

    [Fact]
    public void Threaded_Docs_Preserve_Coherence_As_Non_Authority()
    {
        var repoRoot = GetRepoRoot();
        var docsRoot = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs");
        var attemptBoundary = Normalize(File.ReadAllText(Path.Combine(docsRoot, "SANCTUARY_GEL_FIRST_FORMATION_ATTEMPT_BOUNDARY.md")));
        var gelReadout = Normalize(File.ReadAllText(Path.Combine(docsRoot, "GEL_LINE_REGIONAL_SUBSTRATE_READOUT.md")));
        var firstUse = Normalize(File.ReadAllText(Path.Combine(docsRoot, "FIRST_USE_ELIGIBILITY_BOUNDARY.md")));
        var readiness = Normalize(File.ReadAllText(Path.Combine(docsRoot, "BUILD_READINESS.md")));

        Assert.Contains("consolidates what `Ready`, `Held`, and `Refused` can report without turning coherence into survivor admission, first-use admission, Sanctuary.Actual governance, runtime activation, or Cradle.GEL generation", attemptBoundary, StringComparison.Ordinal);
        Assert.Contains("The first formation-attempt readout consolidates what that assessment can report.", gelReadout, StringComparison.Ordinal);
        Assert.Contains("The first-use path now has a passive admission/enactment split.", gelReadout, StringComparison.Ordinal);
        Assert.Contains("does not convert eligibility or coherence into first-use admission, enactment, runtime, Sanctuary.Actual governance, or Cradle.GEL generation", firstUse, StringComparison.Ordinal);
        Assert.Contains("consolidates what formation-attempt coherence can report without converting coherence into survivor admission, first-use admission, Sanctuary.Actual governance, runtime activation, or Cradle.GEL generation", readiness, StringComparison.Ordinal);
    }

    [Fact]
    public void First_Formation_Attempt_Readout_Does_Not_Add_Mechanism_Files()
    {
        var repoRoot = GetRepoRoot();
        var lineRoot = Path.Combine(repoRoot, "OAN Mortalis V1.2.1");
        var sourceFiles = Directory
            .EnumerateFiles(Path.Combine(lineRoot, "src"), "*.cs", SearchOption.AllDirectories)
            .Where(path =>
                !path.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}", StringComparison.OrdinalIgnoreCase) &&
                !path.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}", StringComparison.OrdinalIgnoreCase))
            .ToArray();

        Assert.DoesNotContain(sourceFiles, path => Path.GetFileName(path).Contains("FirstFormationAttemptReadout", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(sourceFiles, path => Path.GetFileName(path).Contains("ModelSelection", StringComparison.OrdinalIgnoreCase));
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
