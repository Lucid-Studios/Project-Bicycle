using System.Text.RegularExpressions;
using Xunit;

namespace San.Audit.Tests;

public sealed class FirstUseAdmissionEnactmentBoundaryDocsTests
{
    [Fact]
    public void Admission_Enactment_Boundary_Locks_Governing_Sentence_Ladder_And_Invariants()
    {
        var repoRoot = GetRepoRoot();
        var boundaryPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "FIRST_USE_ADMISSION_AND_ENACTMENT_SPLIT_BOUNDARY.md");
        var boundaryText = Normalize(File.ReadAllText(boundaryPath));

        Assert.Contains("First-use admission may authorize preparation of a bounded first-use session, but first-use enactment is the separate witnessed event of entering that session; neither admission nor enactment activates runtime transaction authority, Sanctuary.Actual governance, RTME, SLI.Lisp execution, model selection, or Cradle.GEL generation.", boundaryText, StringComparison.Ordinal);
        Assert.Contains("first-use eligibility consideration -> first-use admission -> first-use enactment -> later negative RTME readiness boundary -> much later RTME admission", boundaryText, StringComparison.Ordinal);
        Assert.Contains("eligibility != admission", boundaryText, StringComparison.Ordinal);
        Assert.Contains("admission != enactment", boundaryText, StringComparison.Ordinal);
        Assert.Contains("enactment != runtime transaction", boundaryText, StringComparison.Ordinal);
        Assert.Contains("first use != governance", boundaryText, StringComparison.Ordinal);
    }

    [Fact]
    public void Admission_And_Enactment_Are_Separate_Passive_Offices()
    {
        var repoRoot = GetRepoRoot();
        var boundaryPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "FIRST_USE_ADMISSION_AND_ENACTMENT_SPLIT_BOUNDARY.md");
        var boundaryText = Normalize(File.ReadAllText(boundaryPath));

        Assert.Contains("First-use admission is a passive preparation office.", boundaryText, StringComparison.Ordinal);
        Assert.Contains("First-use enactment is a passive witnessed-entry office.", boundaryText, StringComparison.Ordinal);
        Assert.Contains("Admission does not enter the session.", boundaryText, StringComparison.Ordinal);
        Assert.Contains("Enactment witnesses entry into the bounded first-use session.", boundaryText, StringComparison.Ordinal);
        Assert.Contains("Enactment does not activate runtime transaction authority, `RTME`, `SLI.Lisp`, model selection, Sanctuary.Actual, governing `CME`, or Cradle.GEL.", boundaryText, StringComparison.Ordinal);
    }

    [Fact]
    public void Threaded_Docs_Preserve_Eligibility_Admission_Enactment_And_Runtime_Distinctions()
    {
        var repoRoot = GetRepoRoot();
        var docsRoot = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs");
        var eligibility = Normalize(File.ReadAllText(Path.Combine(docsRoot, "FIRST_USE_ELIGIBILITY_BOUNDARY.md")));
        var attemptReadout = Normalize(File.ReadAllText(Path.Combine(docsRoot, "SANCTUARY_GEL_FIRST_FORMATION_ATTEMPT_READOUT.md")));
        var gelReadout = Normalize(File.ReadAllText(Path.Combine(docsRoot, "GEL_LINE_REGIONAL_SUBSTRATE_READOUT.md")));
        var readiness = Normalize(File.ReadAllText(Path.Combine(docsRoot, "BUILD_READINESS.md")));

        Assert.Contains("eligibility does not create admission and admission does not create enactment", eligibility, StringComparison.Ordinal);
        Assert.Contains("The first-use admission and enactment split is the next passive seam above this readout.", attemptReadout, StringComparison.Ordinal);
        Assert.Contains("Admission may prepare a bounded first-use session, and enactment may witness entry into that session, but neither one is runtime transaction authority", gelReadout, StringComparison.Ordinal);
        Assert.Contains("the first-use admission and enactment split boundary now lives in `FIRST_USE_ADMISSION_AND_ENACTMENT_SPLIT_BOUNDARY.md`", readiness, StringComparison.Ordinal);
        Assert.Contains("`eligibility != admission`, `admission != enactment`, `enactment != runtime transaction`, and `first use != governance`", readiness, StringComparison.Ordinal);
    }

    [Fact]
    public void Admission_Enactment_Boundary_Does_Not_Add_Runtime_Or_Governance_Mechanism()
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
            Path.GetFileName(path).Contains("FirstUseAdmission", StringComparison.OrdinalIgnoreCase));

        Assert.DoesNotContain(sourceFiles, path => Path.GetFileName(path).Contains("FirstUseAdmissionEnactmentService", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(sourceFiles, path => Path.GetFileName(path).Contains("FirstUseAdmissionEnactmentEvaluator", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(sourceFiles, path => Path.GetFileName(path).Contains("FirstUseRuntime", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(sourceFiles, path => Path.GetFileName(path).Contains("ModelSelection", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(sourceFiles, path => Path.GetFileName(path).Contains("CradleGelGeneration", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(sourceFiles, path => Path.GetFileName(path).Contains("SanctuaryActual", StringComparison.OrdinalIgnoreCase));
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
