using System.Text.RegularExpressions;
using Xunit;

namespace San.Audit.Tests;

public sealed class GelPredicatePriorFormalizationBoundaryDocsTests
{
    [Fact]
    public void Predicate_Prior_Formalization_Note_Locks_Governing_Sentence_And_Ladder()
    {
        var repoRoot = GetRepoRoot();
        var lawPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "GEL_PREDICATE_PRIOR_FORMALIZATION_BOUNDARY.md");
        var lawText = Normalize(File.ReadAllText(lawPath));

        Assert.Contains("`.GEL` predicate-prior formalization records a rooted, witnessed, SLI-constructor-bearing, Engrammitization-facing candidate as a bounded library prior; it does not admit final `Sanctuary.GEL` survivor standing, grant first use, authorize EC mutation, activate `SLI.Lisp`, or create runtime authority.", lawText, StringComparison.Ordinal);
        Assert.Contains("raw data -> UTF-8 witness -> RootAtlas root predicate -> SLI symbolic constructor -> .GEL predicate prior -> Engrammitization carrier review -> later SLI.Lisp mutation / transport / EC operations -> later Sanctuary.GEL survivor admission if warranted", lawText, StringComparison.Ordinal);
        Assert.Contains("downstream of `GEL` formation readiness and upstream of localized `Sanctuary.GEL` formation", lawText, StringComparison.Ordinal);
        Assert.Contains("Formalization is not survivor admission.", lawText, StringComparison.Ordinal);
        Assert.Contains("Formalization is not first-use permission.", lawText, StringComparison.Ordinal);
    }

    [Fact]
    public void Predicate_Prior_Formalization_Note_Names_Layered_Object_And_Utf8_Witness_Rule()
    {
        var repoRoot = GetRepoRoot();
        var lawPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "GEL_PREDICATE_PRIOR_FORMALIZATION_BOUNDARY.md");
        var lawText = Normalize(File.ReadAllText(lawPath));

        Assert.Contains("predicate_object:", lawText, StringComparison.Ordinal);
        Assert.Contains("witness root constructor gel_prior invariants mutation_policy transport_receipts admission_ceiling: candidate_only", lawText, StringComparison.Ordinal);
        Assert.Contains("UTF-8 is the first lawful witness field.", lawText, StringComparison.Ordinal);
        Assert.Contains("preserve source text, encoding state, token or span bounds, local context, source witness, ambiguity, and Unicode snapshot posture", lawText, StringComparison.Ordinal);
        Assert.Contains("Symbolics may compress and deepen the object, but they may not replace the witness.", lawText, StringComparison.Ordinal);
    }

    [Fact]
    public void Predicate_Prior_Formalization_Note_Keeps_Root_Constructor_And_Prior_Non_Authoritative()
    {
        var repoRoot = GetRepoRoot();
        var lawPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "GEL_PREDICATE_PRIOR_FORMALIZATION_BOUNDARY.md");
        var lawText = Normalize(File.ReadAllText(lawPath));

        Assert.Contains("RootAtlas grounding gives root predicate identity.", lawText, StringComparison.Ordinal);
        Assert.Contains("It does not create local RootAtlas authority, Atlas mutation, domain authority, or universal source sovereignty.", lawText, StringComparison.Ordinal);
        Assert.Contains("The `SLI` symbolic constructor is attachment-native and structured.", lawText, StringComparison.Ordinal);
        Assert.Contains("prefix.super", lawText, StringComparison.Ordinal);
        Assert.Contains("prefix.sub", lawText, StringComparison.Ordinal);
        Assert.Contains("suffix.super", lawText, StringComparison.Ordinal);
        Assert.Contains("suffix.sub", lawText, StringComparison.Ordinal);
        Assert.Contains("A flat rendering is only a lossy debug or display compromise.", lawText, StringComparison.Ordinal);
        Assert.Contains("`.GEL` stores predicate priors.", lawText, StringComparison.Ordinal);
        Assert.Contains("It does not store final survivor admission.", lawText, StringComparison.Ordinal);
    }

    [Fact]
    public void Predicate_Prior_Formalization_Note_Refuses_Mutation_Transport_Lisp_Runtime_And_Survivor_Admission()
    {
        var repoRoot = GetRepoRoot();
        var lawPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "GEL_PREDICATE_PRIOR_FORMALIZATION_BOUNDARY.md");
        var lawText = Normalize(File.ReadAllText(lawPath));

        Assert.Contains("This boundary does not perform mutation, transport, EC operations, `SLI.Lisp` execution, or admission.", lawText, StringComparison.Ordinal);
        Assert.Contains("admit final `Sanctuary.GEL` survivor standing", lawText, StringComparison.Ordinal);
        Assert.Contains("authorize EC mutation", lawText, StringComparison.Ordinal);
        Assert.Contains("activate `SLI.Lisp`", lawText, StringComparison.Ordinal);
        Assert.Contains("emit transport", lawText, StringComparison.Ordinal);
        Assert.Contains("create runtime authority", lawText, StringComparison.Ordinal);
        Assert.Contains("expose `SPC` validator posture", lawText, StringComparison.Ordinal);
    }

    [Fact]
    public void Threaded_Docs_Preserve_Readiness_Prior_Formalization_And_Localized_Formation_Distinctions()
    {
        var repoRoot = GetRepoRoot();
        var docsRoot = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs");

        var readiness = Normalize(File.ReadAllText(Path.Combine(docsRoot, "GENERATIVE_ENGRAMMITIZATION_LIBRARY_FORMATION_READINESS_BOUNDARY.md")));
        var firstFormation = Normalize(File.ReadAllText(Path.Combine(docsRoot, "FIRST_RUNTIME_ADMITTED_SANCTUARY_GEL_FORMATION_LAW.md")));
        var localizedFormation = Normalize(File.ReadAllText(Path.Combine(docsRoot, "LOCALIZED_SANCTUARY_GEL_FORMATION_BOUNDARY.md")));
        var firstUse = Normalize(File.ReadAllText(Path.Combine(docsRoot, "FIRST_USE_ELIGIBILITY_BOUNDARY.md")));
        var standingLadder = Normalize(File.ReadAllText(Path.Combine(docsRoot, "INSTALL_TO_FIRST_USE_STANDING_LADDER.md")));
        var buildReadiness = Normalize(File.ReadAllText(Path.Combine(docsRoot, "BUILD_READINESS.md")));

        Assert.Contains("The predicate-prior formalization boundary now sits immediately above this readiness body and below localized `Sanctuary.GEL` formation.", readiness, StringComparison.Ordinal);
        Assert.Contains("The `.GEL` predicate-prior formalization boundary now adds the structured prior distinction", firstFormation, StringComparison.Ordinal);
        Assert.Contains("The `.GEL` predicate-prior formalization boundary sits between readiness and localized formation.", localizedFormation, StringComparison.Ordinal);
        Assert.Contains("Predicate-prior formalization does not grant first-use permission.", firstUse, StringComparison.Ordinal);
        Assert.Contains("The `.GEL` predicate-prior formalization body now names the structured candidate-only prior beneath localized `.GEL` formation.", standingLadder, StringComparison.Ordinal);
        Assert.Contains("fixes a structured candidate-only library prior", buildReadiness, StringComparison.Ordinal);
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
