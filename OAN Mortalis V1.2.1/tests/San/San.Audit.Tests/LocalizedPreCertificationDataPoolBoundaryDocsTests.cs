using System.Text.RegularExpressions;
using Xunit;

namespace San.Audit.Tests;

public sealed class LocalizedPreCertificationDataPoolBoundaryDocsTests
{
    [Fact]
    public void Pre_Certification_Data_Pool_Note_Locks_Governing_Sentence_And_Ladder()
    {
        var repoRoot = GetRepoRoot();
        var lawPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "LOCALIZED_PRE_CERTIFICATION_DATA_POOL_BOUNDARY.md");
        var lawText = Normalize(File.ReadAllText(lawPath));

        Assert.Contains("Pre-certification data pool formation may contextualize `Sanctuary.GEL`, but it does not certify, disclose, consent, authorize, govern, or activate runtime.", lawText, StringComparison.Ordinal);
        Assert.Contains("represented source/template/standing inputs -> localized pre-certification data pool -> localized Sanctuary.GEL formation -> Sanctuary pre-governing standing -> first-use eligibility consideration", lawText, StringComparison.Ordinal);
        Assert.Contains("The pool is upstream of localized `Sanctuary.GEL` formation.", lawText, StringComparison.Ordinal);
        Assert.Contains("The pool is not the formed `.GEL`.", lawText, StringComparison.Ordinal);
        Assert.Contains("The pool is not first-use eligibility consideration.", lawText, StringComparison.Ordinal);
    }

    [Fact]
    public void Pre_Certification_Data_Pool_Note_References_Stages_And_Localizes_Without_Activation()
    {
        var repoRoot = GetRepoRoot();
        var lawPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "LOCALIZED_PRE_CERTIFICATION_DATA_POOL_BOUNDARY.md");
        var lawText = Normalize(File.ReadAllText(lawPath));

        Assert.Contains("may reference, stage, and localize", lawText, StringComparison.Ordinal);
        Assert.Contains("Lab asset candidate refs", lawText, StringComparison.Ordinal);
        Assert.Contains("RootAtlas regional source posture refs", lawText, StringComparison.Ordinal);
        Assert.Contains("legal-admin template-family refs", lawText, StringComparison.Ordinal);
        Assert.Contains("National standing refs", lawText, StringComparison.Ordinal);
        Assert.Contains("Regional standing refs", lawText, StringComparison.Ordinal);
        Assert.Contains("Local standing refs", lawText, StringComparison.Ordinal);
        Assert.Contains("These are represented inputs only.", lawText, StringComparison.Ordinal);
        Assert.Contains("They do not become active legal documents, active RootAtlas authority, consent records, disclosure surfaces, certification claims, or runtime instructions.", lawText, StringComparison.Ordinal);
    }

    [Fact]
    public void Pre_Certification_Data_Pool_Note_Refuses_Activation_And_Overclaims()
    {
        var repoRoot = GetRepoRoot();
        var lawPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "LOCALIZED_PRE_CERTIFICATION_DATA_POOL_BOUNDARY.md");
        var lawText = Normalize(File.ReadAllText(lawPath));

        Assert.Contains("certification", lawText, StringComparison.Ordinal);
        Assert.Contains("counsel-approved terms", lawText, StringComparison.Ordinal);
        Assert.Contains("consent records", lawText, StringComparison.Ordinal);
        Assert.Contains("disclosure issuance", lawText, StringComparison.Ordinal);
        Assert.Contains("domain authorization", lawText, StringComparison.Ordinal);
        Assert.Contains("first-use admission", lawText, StringComparison.Ordinal);
        Assert.Contains("`RTME`", lawText, StringComparison.Ordinal);
        Assert.Contains("governing `CME`", lawText, StringComparison.Ordinal);
        Assert.Contains("runtime authority", lawText, StringComparison.Ordinal);
    }

    [Fact]
    public void Threaded_Docs_Point_To_Pre_Certification_Pool_As_Upstream_Represented_Input_Body()
    {
        var repoRoot = GetRepoRoot();
        var docsRoot = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs");

        var localizedFormation = Normalize(File.ReadAllText(Path.Combine(docsRoot, "LOCALIZED_SANCTUARY_GEL_FORMATION_BOUNDARY.md")));
        var firstUse = Normalize(File.ReadAllText(Path.Combine(docsRoot, "FIRST_USE_ELIGIBILITY_BOUNDARY.md")));
        var legalAdmin = Normalize(File.ReadAllText(Path.Combine(docsRoot, "LEGAL_ADMIN_LAB_ASSET_TEMPLATE_STAGING_BOUNDARY.md")));
        var standingLadder = Normalize(File.ReadAllText(Path.Combine(docsRoot, "INSTALL_TO_FIRST_USE_STANDING_LADDER.md")));
        var readiness = Normalize(File.ReadAllText(Path.Combine(docsRoot, "BUILD_READINESS.md")));

        Assert.Contains("The localized pre-certification data pool sits upstream of this formation boundary.", localizedFormation, StringComparison.Ordinal);
        Assert.Contains("The localized pre-certification data pool names the represented input body beneath this boundary.", localizedFormation, StringComparison.Ordinal);
        Assert.Contains("sits beneath localized substrate formation as represented source, template, and standing input context", firstUse, StringComparison.Ordinal);
        Assert.Contains("may reference these template-family and Lab asset candidate postures as represented inputs only", legalAdmin, StringComparison.Ordinal);
        Assert.Contains("represented source/template/standing input body beneath localized `.GEL` formation", standingLadder, StringComparison.Ordinal);
        Assert.Contains("localized pre-certification data pool formation now contextualizes", readiness, StringComparison.Ordinal);
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
