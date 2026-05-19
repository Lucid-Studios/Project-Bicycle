using System.Text.RegularExpressions;
using Xunit;

namespace San.Audit.Tests;

public sealed class SanctuaryGelRegionalSubstrateFormationBoundaryDocsTests
{
    [Fact]
    public void Regional_Substrate_Note_Locks_Governing_Sentence_And_Hierarchy()
    {
        var repoRoot = GetRepoRoot();
        var lawPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "SANCTUARY_GEL_REGIONAL_SUBSTRATE_FORMATION_BOUNDARY.md");
        var lawText = Normalize(File.ReadAllText(lawPath));

        Assert.Contains("`Sanctuary.GEL` regional substrate formation records the bounded regional GEL body required before Sanctuary.Actual governing CME and later Cradle.GEL generation may be considered; it does not stand Sanctuary.Actual, select models, authorize governance, activate runtime, or generate Cradle.GEL.", lawText, StringComparison.Ordinal);
        Assert.Contains("Sanctuary Body = the full program body", lawText, StringComparison.Ordinal);
        Assert.Contains("Sanctuary.GEL = regional/root local GEL substrate for Sanctuary", lawText, StringComparison.Ordinal);
        Assert.Contains("Sanctuary.Actual Mother/Father governing CME = later governing formation that requires Sanctuary.GEL substrate", lawText, StringComparison.Ordinal);
        Assert.Contains("Cradle.GEL = later CradleTek-local GEL generated downstream from standing Sanctuary governance", lawText, StringComparison.Ordinal);
    }

    [Fact]
    public void Regional_Substrate_Note_Names_Ladder_And_Preserves_Localized_Floor()
    {
        var repoRoot = GetRepoRoot();
        var lawPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "SANCTUARY_GEL_REGIONAL_SUBSTRATE_FORMATION_BOUNDARY.md");
        var lawText = Normalize(File.ReadAllText(lawPath));

        Assert.Contains("GEL predicate-prior formalization -> localized pre-certification data pool -> Sanctuary.GEL regional substrate formation -> Sanctuary pre-governing standing -> first-use eligibility consideration -> later Sanctuary.Actual Mother/Father governing CME -> later Cradle.GEL generation", lawText, StringComparison.Ordinal);
        Assert.Contains("The localized `Sanctuary.GEL` formation boundary remains the constitutional floor.", lawText, StringComparison.Ordinal);
        Assert.Contains("This boundary names the specific body that is forming inside that floor: `Sanctuary.GEL` as regional/root substrate.", lawText, StringComparison.Ordinal);
    }

    [Fact]
    public void Regional_Substrate_Note_Requires_Inputs_Without_Activating_Downstream_Authority()
    {
        var repoRoot = GetRepoRoot();
        var lawPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "SANCTUARY_GEL_REGIONAL_SUBSTRATE_FORMATION_BOUNDARY.md");
        var lawText = Normalize(File.ReadAllText(lawPath));

        Assert.Contains("ready `.GEL` predicate-prior refs", lawText, StringComparison.Ordinal);
        Assert.Contains("localized pre-certification data pool refs", lawText, StringComparison.Ordinal);
        Assert.Contains("National standing refs", lawText, StringComparison.Ordinal);
        Assert.Contains("Regional standing refs", lawText, StringComparison.Ordinal);
        Assert.Contains("Local standing refs", lawText, StringComparison.Ordinal);
        Assert.Contains("regional package footing", lawText, StringComparison.Ordinal);
        Assert.Contains("They do not become Sanctuary.Actual, Mother/Father governing `CME`, model selection, Cradle.GEL, runtime authority, domain authority, or final survivor admission.", lawText, StringComparison.Ordinal);
    }

    [Fact]
    public void Regional_Substrate_Note_Blocks_SanctuaryActual_CradleGel_Model_Runtime_And_Governance()
    {
        var repoRoot = GetRepoRoot();
        var lawPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "SANCTUARY_GEL_REGIONAL_SUBSTRATE_FORMATION_BOUNDARY.md");
        var lawText = Normalize(File.ReadAllText(lawPath));

        Assert.Contains("Sanctuary.Actual Mother/Father governing `CME` may not be approached as a standing body until `Sanctuary.GEL` regional substrate formation is represented.", lawText, StringComparison.Ordinal);
        Assert.Contains("Cradle.GEL generation may not be approached until later standing Sanctuary governance exists.", lawText, StringComparison.Ordinal);
        Assert.Contains("Cradle.GEL must not be generated directly from install materials", lawText, StringComparison.Ordinal);
        Assert.Contains("stand Sanctuary.Actual", lawText, StringComparison.Ordinal);
        Assert.Contains("stand Mother/Father governing `CME`", lawText, StringComparison.Ordinal);
        Assert.Contains("select models", lawText, StringComparison.Ordinal);
        Assert.Contains("generate Cradle.GEL", lawText, StringComparison.Ordinal);
        Assert.Contains("activate runtime", lawText, StringComparison.Ordinal);
    }

    [Fact]
    public void Threaded_Docs_Preserve_Regional_Substrate_Distinction()
    {
        var repoRoot = GetRepoRoot();
        var docsRoot = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs");

        var localizedFormation = Normalize(File.ReadAllText(Path.Combine(docsRoot, "LOCALIZED_SANCTUARY_GEL_FORMATION_BOUNDARY.md")));
        var preGoverning = Normalize(File.ReadAllText(Path.Combine(docsRoot, "SANCTUARY_PRE_GOVERNING_STANDING_BOUNDARY.md")));
        var firstUse = Normalize(File.ReadAllText(Path.Combine(docsRoot, "FIRST_USE_ELIGIBILITY_BOUNDARY.md")));
        var standingLadder = Normalize(File.ReadAllText(Path.Combine(docsRoot, "INSTALL_TO_FIRST_USE_STANDING_LADDER.md")));
        var readiness = Normalize(File.ReadAllText(Path.Combine(docsRoot, "BUILD_READINESS.md")));

        Assert.Contains("The `Sanctuary.GEL` regional substrate formation boundary names the specific body formed inside this localized floor", localizedFormation, StringComparison.Ordinal);
        Assert.Contains("may recognize that regional substrate without standing Sanctuary.Actual", preGoverning, StringComparison.Ordinal);
        Assert.Contains("It is prerequisite context, not first-use admission, Sanctuary.Actual, model selection, or Cradle.GEL generation.", firstUse, StringComparison.Ordinal);
        Assert.Contains("The `Sanctuary.GEL` regional substrate formation body now names the specific regional/root local GEL substrate for Sanctuary beneath pre-governing standing.", standingLadder, StringComparison.Ordinal);
        Assert.Contains("fixes the bounded regional/root GEL substrate required before Sanctuary.Actual governing `CME` or later Cradle.GEL generation may be considered", readiness, StringComparison.Ordinal);
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
