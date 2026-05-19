using System.Text.RegularExpressions;
using Xunit;

namespace San.Audit.Tests;

public sealed class SanctuaryGelMosCmosSeedSubstrateBoundaryDocsTests
{
    [Fact]
    public void Seed_Substrate_Boundary_Locks_Governing_Sentence_Seed_Line_And_Non_Collapse_Rules()
    {
        var repoRoot = GetRepoRoot();
        var boundaryPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "SANCTUARY_GEL_TO_MOS_CMOS_SEED_SUBSTRATE_BOUNDARY.md");
        var boundaryText = Normalize(File.ReadAllText(boundaryPath));

        Assert.Contains("`Sanctuary.GEL` may predicate the paired Sanctuary.MoS and Sanctuary.cMoS seed substrate for future CME OE/SelfGEL formation; this does not activate governing CME, issue cryptographic authority, mutate Prime, execute SLI.Lisp, or grant runtime control.", boundaryText, StringComparison.Ordinal);
        Assert.Contains("Sanctuary.GEL -> Sanctuary.MoS seed telemetry posture -> Sanctuary.cMoS cryptic binder posture -> paired Prime/Cryptic seed substrate -> Nexus.Control-readable modulation posture -> later SLI.Lisp modulation surface -> later CME OE/SelfGEL formation", boundaryText, StringComparison.Ordinal);
        Assert.Contains("MoS substrate != governing CME", boundaryText, StringComparison.Ordinal);
        Assert.Contains("cMoS binder != active cryptographic key authority", boundaryText, StringComparison.Ordinal);
        Assert.Contains("Prime telemetry != runtime control", boundaryText, StringComparison.Ordinal);
        Assert.Contains("Cryptic telemetry != hidden mutation", boundaryText, StringComparison.Ordinal);
        Assert.Contains("Nexus-readable != Nexus-executable", boundaryText, StringComparison.Ordinal);
        Assert.Contains("SLI.Lisp modulation surface != SLI.Lisp execution", boundaryText, StringComparison.Ordinal);
        Assert.Contains("OE/SelfGEL readiness != CME formation", boundaryText, StringComparison.Ordinal);
    }

    [Fact]
    public void Seed_Substrate_Boundary_Preserves_Mos_Cmos_And_Handoff_Offices()
    {
        var repoRoot = GetRepoRoot();
        var boundaryPath = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs", "SANCTUARY_GEL_TO_MOS_CMOS_SEED_SUBSTRATE_BOUNDARY.md");
        var boundaryText = Normalize(File.ReadAllText(boundaryPath));

        Assert.Contains("It does not replace: - `MosStorageSeatRecord` - `CMosSurfaceRecord` - `GelCgelToMosHandoffReceipt`", boundaryText, StringComparison.Ordinal);
        Assert.Contains("They do not issue keys, activate encryption runtime, mutate Prime, mutate Cryptic material, execute Nexus, execute `SLI.Lisp`, or form `CME`", boundaryText, StringComparison.Ordinal);
        Assert.Contains("`Ready` means the paired seed substrate is readable as posture only.", boundaryText, StringComparison.Ordinal);
        Assert.Contains("`Held` means binder, telemetry, regional, or modulation questions remain held while every denied power remains denied.", boundaryText, StringComparison.Ordinal);
        Assert.Contains("`Refused` means a missing substrate, missing seed lane, or overclaim prevents the seed substrate from standing.", boundaryText, StringComparison.Ordinal);
    }

    [Fact]
    public void Threaded_Docs_Preserve_Substrate_Storage_Seat_Non_Activation_And_Cme_Distinctions()
    {
        var repoRoot = GetRepoRoot();
        var docsRoot = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "docs");
        var regionalSubstrate = Normalize(File.ReadAllText(Path.Combine(docsRoot, "SANCTUARY_GEL_REGIONAL_SUBSTRATE_FORMATION_BOUNDARY.md")));
        var mosSeat = Normalize(File.ReadAllText(Path.Combine(docsRoot, "SANCTUARY_MOS_STORAGE_SEAT_LAW.md")));
        var handoff = Normalize(File.ReadAllText(Path.Combine(docsRoot, "GEL_CGEL_TO_MOS_HANDOFF_AND_RECEIPT_LAW.md")));
        var stackMap = Normalize(File.ReadAllText(Path.Combine(docsRoot, "SANCTUARY_GEL_CGEL_MOS_GOVERNANCE_STACK_MAP.md")));
        var nonActivation = Normalize(File.ReadAllText(Path.Combine(docsRoot, "GEL_RTME_SLI_LISP_NON_ACTIVATION_READINESS_BOUNDARY.md")));
        var readout = Normalize(File.ReadAllText(Path.Combine(docsRoot, "GEL_LINE_REGIONAL_SUBSTRATE_READOUT.md")));
        var readiness = Normalize(File.ReadAllText(Path.Combine(docsRoot, "BUILD_READINESS.md")));

        Assert.Contains("may predicate paired Sanctuary.MoS and Sanctuary.cMoS seed substrate posture without activating governing CME", regionalSubstrate, StringComparison.Ordinal);
        Assert.Contains("does not replace `MosStorageSeatRecord`, `CMosSurfaceRecord`, or receipted GEL/cGEL-to-MoS handoff law", mosSeat, StringComparison.Ordinal);
        Assert.Contains("not the GEL/cGEL-to-MoS handoff receipt and does not create storage-seat standing", handoff, StringComparison.Ordinal);
        Assert.Contains("paired `Sanctuary.GEL -> Sanctuary.MoS/Sanctuary.cMoS` seed substrate posture", stackMap, StringComparison.Ordinal);
        Assert.Contains("Nexus-readable is not Nexus-executable and `SLI.Lisp` modulation surface is not `SLI.Lisp` execution", nonActivation, StringComparison.Ordinal);
        Assert.Contains("Sanctuary.GEL-to-MoS/cMoS seed substrate boundary now reads how `Sanctuary.GEL` may predicate paired `Sanctuary.MoS` seed telemetry posture", readout, StringComparison.Ordinal);
        Assert.Contains("paired Sanctuary.MoS seed telemetry and Sanctuary.cMoS cryptic binder posture as readable seed substrate only", readiness, StringComparison.Ordinal);
    }

    [Fact]
    public void Seed_Substrate_Boundary_Does_Not_Add_Service_Evaluator_Runtime_Key_Mutator_Executor_Or_Cme_Files()
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
            Path.GetFileName(path).Contains("MosCmosSeedSubstrate", StringComparison.OrdinalIgnoreCase));

        Assert.DoesNotContain(sourceFiles, path => Path.GetFileName(path).Contains("KeyGenerator", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(sourceFiles, path => Path.GetFileName(path).Contains("EncryptionRuntime", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(sourceFiles, path => Path.GetFileName(path).Contains("PrimeMutator", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(sourceFiles, path => Path.GetFileName(path).Contains("CrypticMutator", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(sourceFiles, path => Path.GetFileName(path).Contains("NexusExecutor", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(sourceFiles, path => Path.GetFileName(path).Contains("SliLispExecutor", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(sourceFiles, path => Path.GetFileName(path).Contains("CmeFormation", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(sourceFiles, path => Path.GetFileName(path).Contains("RuntimeControlSurface", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void Seed_Substrate_Touched_Text_Does_Not_Contain_External_Documentation_Repo_Or_Private_Corpus_Paths()
    {
        var repoRoot = GetRepoRoot();
        var lineRoot = Path.Combine(repoRoot, "OAN Mortalis V1.2.1");
        var touchedTextFiles = new[]
        {
            Path.Combine(lineRoot, "docs", "SANCTUARY_GEL_TO_MOS_CMOS_SEED_SUBSTRATE_BOUNDARY.md"),
            Path.Combine(lineRoot, "docs", "SANCTUARY_GEL_REGIONAL_SUBSTRATE_FORMATION_BOUNDARY.md"),
            Path.Combine(lineRoot, "docs", "SANCTUARY_MOS_STORAGE_SEAT_LAW.md"),
            Path.Combine(lineRoot, "docs", "GEL_CGEL_TO_MOS_HANDOFF_AND_RECEIPT_LAW.md"),
            Path.Combine(lineRoot, "docs", "SANCTUARY_GEL_CGEL_MOS_GOVERNANCE_STACK_MAP.md"),
            Path.Combine(lineRoot, "docs", "GEL_RTME_SLI_LISP_NON_ACTIVATION_READINESS_BOUNDARY.md"),
            Path.Combine(lineRoot, "docs", "GEL_LINE_REGIONAL_SUBSTRATE_READOUT.md"),
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
