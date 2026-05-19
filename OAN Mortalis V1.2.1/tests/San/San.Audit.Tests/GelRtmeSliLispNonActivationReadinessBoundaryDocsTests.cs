using System.Text.RegularExpressions;
using Xunit;

namespace San.Audit.Tests;

public sealed class GelRtmeSliLispNonActivationReadinessBoundaryDocsTests
{
    [Fact]
    public void Non_Activation_Readiness_Boundary_Locks_Governing_Sentences()
    {
        var repoRoot = GetRepoRoot();
        var boundaryPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "GEL_RTME_SLI_LISP_NON_ACTIVATION_READINESS_BOUNDARY.md");
        var boundaryText = Normalize(File.ReadAllText(boundaryPath));

        Assert.Contains("`Sanctuary.GEL` rooted meaning may be read as future RTME movement candidate posture, but this boundary does not activate RTME, execute `SLI.Lisp`, persist state, mutate Prime, bypass membranes, or admit runtime transaction authority.", boundaryText, StringComparison.Ordinal);
        Assert.Contains("Nothing may move.", boundaryText, StringComparison.Ordinal);
    }

    [Fact]
    public void Non_Activation_Readiness_Boundary_Locks_Order_Lane_And_Invariants()
    {
        var repoRoot = GetRepoRoot();
        var boundaryPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "GEL_RTME_SLI_LISP_NON_ACTIVATION_READINESS_BOUNDARY.md");
        var boundaryText = Normalize(File.ReadAllText(boundaryPath));

        Assert.Contains("stand -> remain -> not move yet", boundaryText, StringComparison.Ordinal);
        Assert.Contains("Future RTME admission remains downstream.", boundaryText, StringComparison.Ordinal);
        Assert.Contains("RootAtlas -> SLI constructor -> .GEL predicate prior -> Engrammitization review -> Sanctuary.GEL standing -> future RTME admission -> SLI.Lisp symbolic transaction -> translational membrane -> bounded runtime effect", boundaryText, StringComparison.Ordinal);
        Assert.Contains("standing != movement", boundaryText, StringComparison.Ordinal);
        Assert.Contains("movement != persistence", boundaryText, StringComparison.Ordinal);
        Assert.Contains("availability != authority", boundaryText, StringComparison.Ordinal);
        Assert.Contains("transaction != survivor_admission", boundaryText, StringComparison.Ordinal);
    }

    [Fact]
    public void Non_Activation_Readiness_Boundary_Locks_Negative_Defaults_And_Non_Powers()
    {
        var repoRoot = GetRepoRoot();
        var boundaryPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "GEL_RTME_SLI_LISP_NON_ACTIVATION_READINESS_BOUNDARY.md");
        var boundaryText = Normalize(File.ReadAllText(boundaryPath));

        Assert.Contains("rtme_active: false", boundaryText, StringComparison.Ordinal);
        Assert.Contains("runtime_transaction_movement_allowed: false", boundaryText, StringComparison.Ordinal);
        Assert.Contains("always_on_authority_granted: false", boundaryText, StringComparison.Ordinal);
        Assert.Contains("direct_persistence_allowed: false", boundaryText, StringComparison.Ordinal);
        Assert.Contains("direct_prime_mutation_allowed: false", boundaryText, StringComparison.Ordinal);
        Assert.Contains("run an RTME runner", boundaryText, StringComparison.Ordinal);
        Assert.Contains("start an RTME listener", boundaryText, StringComparison.Ordinal);
        Assert.Contains("bypass membranes", boundaryText, StringComparison.Ordinal);
        Assert.Contains("admit survivor standing", boundaryText, StringComparison.Ordinal);
    }

    [Fact]
    public void Threaded_Docs_Place_Non_Activation_After_Witness_Store_And_Before_Rtme_Admission()
    {
        var repoRoot = GetRepoRoot();
        var docsRoot = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs");
        var witnessStore = Normalize(File.ReadAllText(Path.Combine(docsRoot, "SANCTUARY_LOCAL_CONVERSATION_WITNESS_STORE_BOUNDARY.md")));
        var firstUse = Normalize(File.ReadAllText(Path.Combine(docsRoot, "FIRST_USE_ADMISSION_AND_ENACTMENT_SPLIT_BOUNDARY.md")));
        var gelReadout = Normalize(File.ReadAllText(Path.Combine(docsRoot, "GEL_LINE_REGIONAL_SUBSTRATE_READOUT.md")));
        var rtmeSkeleton = Normalize(File.ReadAllText(Path.Combine(docsRoot, "SANCTUARYID_RTME_SKELETON.md")));
        var readiness = Normalize(File.ReadAllText(Path.Combine(docsRoot, "BUILD_READINESS.md")));

        Assert.Contains("may read future RTME movement candidacy, but nothing may move", witnessStore, StringComparison.Ordinal);
        Assert.Contains("may read future movement candidacy only after the witness store has answered what may remain", firstUse, StringComparison.Ordinal);
        Assert.Contains("GEL/RTME/SLI.Lisp non-activation readiness boundary now reads future movement candidacy while proving non-movement", gelReadout, StringComparison.Ordinal);
        Assert.Contains("proves that future movement candidacy may be read without activating this skeleton", rtmeSkeleton, StringComparison.Ordinal);
        Assert.Contains("future RTME movement candidacy as readable only", readiness, StringComparison.Ordinal);
    }

    [Fact]
    public void Non_Activation_Readiness_Does_Not_Add_Runner_Listener_Persistence_Prime_Or_Bypass_Files()
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
            Path.GetFileName(path).Contains("Rtme", StringComparison.OrdinalIgnoreCase) &&
            Path.GetFileName(path).Contains("Readiness", StringComparison.OrdinalIgnoreCase));

        Assert.DoesNotContain(sourceFiles, path => Path.GetFileName(path).Contains("RtmeRunner", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(sourceFiles, path => Path.GetFileName(path).Contains("RtmeListener", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(sourceFiles, path => Path.GetFileName(path).Contains("PersistenceWriter", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(sourceFiles, path => Path.GetFileName(path).Contains("PrimeMutator", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(sourceFiles, path => Path.GetFileName(path).Contains("MembraneBypass", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(sourceFiles, path => Path.GetFileName(path).Contains("AutoAdmission", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(sourceFiles, path => Path.GetFileName(path).Contains("SurvivorAdmissionService", StringComparison.OrdinalIgnoreCase));
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
