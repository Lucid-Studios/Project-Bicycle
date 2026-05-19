using San.Common;
using Xunit;

namespace San.Audit.Tests;

public sealed class GenerativeEngrammitizationLibraryFormationReadinessReferenceDataTests
{
    [Fact]
    public void Passive_Contracts_Expose_Gel_Readiness_Posture_And_Refusal_Types()
    {
        Assert.Contains(GenerativeEngrammitizationLibraryFormationReadinessDisposition.Ready, Enum.GetValues<GenerativeEngrammitizationLibraryFormationReadinessDisposition>());
        Assert.Contains(GenerativeEngrammitizationLibraryFormationReadinessDisposition.Held, Enum.GetValues<GenerativeEngrammitizationLibraryFormationReadinessDisposition>());
        Assert.Contains(GenerativeEngrammitizationLibraryFormationReadinessDisposition.Refused, Enum.GetValues<GenerativeEngrammitizationLibraryFormationReadinessDisposition>());

        Assert.Contains(GenerativeEngrammitizationLibraryCandidatePostureKind.RootedSourcePosture, Enum.GetValues<GenerativeEngrammitizationLibraryCandidatePostureKind>());
        Assert.Contains(GenerativeEngrammitizationLibraryCandidatePostureKind.WitnessedPosture, Enum.GetValues<GenerativeEngrammitizationLibraryCandidatePostureKind>());
        Assert.Contains(GenerativeEngrammitizationLibraryCandidatePostureKind.SliFormedPosture, Enum.GetValues<GenerativeEngrammitizationLibraryCandidatePostureKind>());
        Assert.Contains(GenerativeEngrammitizationLibraryCandidatePostureKind.EngrammitizationFacingPosture, Enum.GetValues<GenerativeEngrammitizationLibraryCandidatePostureKind>());
        Assert.Contains(GenerativeEngrammitizationLibraryCandidatePostureKind.PredicateSurfaceReadiness, Enum.GetValues<GenerativeEngrammitizationLibraryCandidatePostureKind>());

        Assert.Contains(GenerativeEngrammitizationLibraryFormationReadinessRefusalReason.SurvivorAdmissionOverclaimed, Enum.GetValues<GenerativeEngrammitizationLibraryFormationReadinessRefusalReason>());
        Assert.Contains(GenerativeEngrammitizationLibraryFormationReadinessRefusalReason.LocalizedSanctuaryGelFormationOverclaimed, Enum.GetValues<GenerativeEngrammitizationLibraryFormationReadinessRefusalReason>());
        Assert.Contains(GenerativeEngrammitizationLibraryFormationReadinessRefusalReason.RuntimeOrGovernanceOverclaimed, Enum.GetValues<GenerativeEngrammitizationLibraryFormationReadinessRefusalReason>());
    }

    [Fact]
    public void Canonical_Reference_Data_Includes_Ready_Held_And_Refused_Records()
    {
        Assert.Equal(GenerativeEngrammitizationLibraryFormationReadinessDisposition.Ready, GenerativeEngrammitizationLibraryFormationReadinessReferenceData.ReadyLibraryFormationReadiness.Disposition);
        Assert.Equal(GenerativeEngrammitizationLibraryFormationReadinessDisposition.Held, GenerativeEngrammitizationLibraryFormationReadinessReferenceData.HeldForLocalizationOrSurvivorReview.Disposition);
        Assert.Contains(GenerativeEngrammitizationLibraryFormationReadinessReferenceData.CanonicalRecords, record => record.Disposition == GenerativeEngrammitizationLibraryFormationReadinessDisposition.Refused);
    }

    [Fact]
    public void Ready_Readiness_Includes_All_Candidate_Postures_And_Remains_Non_Admission()
    {
        var record = GenerativeEngrammitizationLibraryFormationReadinessReferenceData.ReadyLibraryFormationReadiness;
        var postureKinds = record.CandidatePostures.Select(static posture => posture.Kind).ToArray();

        foreach (var kind in Enum.GetValues<GenerativeEngrammitizationLibraryCandidatePostureKind>())
        {
            Assert.Contains(kind, postureKinds);
        }

        Assert.Contains("gel-readiness-ref://logical-research-source-posture", record.SourceLogicalResearchRefs);
        Assert.Contains("predicate-surface-ref://family-bearing-gel-inheritance", record.SourcePredicateSurfaceRefs);
        Assert.Contains(".GEL inclusion does not equal final Sanctuary.GEL survivor admission", record.NonAdmissionSummary, StringComparison.Ordinal);
        Assert.Contains("localized formation, first use, governing CME, RTME, or runtime authority", record.NonAdmissionSummary, StringComparison.Ordinal);
    }

    [Fact]
    public void Missing_Root_Witness_Or_Sli_Formation_Refuses_Readiness()
    {
        var record = GenerativeEngrammitizationLibraryFormationReadinessReferenceData.RefusedMissingRootWitnessOrSliFormation;
        var postureKinds = record.CandidatePostures.Select(static posture => posture.Kind).ToArray();

        Assert.Equal(GenerativeEngrammitizationLibraryFormationReadinessDisposition.Refused, record.Disposition);
        Assert.Contains(GenerativeEngrammitizationLibraryFormationReadinessRefusalReason.MissingRootedSourcePosture, record.RefusalReasons);
        Assert.Contains(GenerativeEngrammitizationLibraryFormationReadinessRefusalReason.MissingWitnessPosture, record.RefusalReasons);
        Assert.Contains(GenerativeEngrammitizationLibraryFormationReadinessRefusalReason.MissingSliFormationPosture, record.RefusalReasons);
        Assert.DoesNotContain(GenerativeEngrammitizationLibraryCandidatePostureKind.RootedSourcePosture, postureKinds);
        Assert.DoesNotContain(GenerativeEngrammitizationLibraryCandidatePostureKind.WitnessedPosture, postureKinds);
        Assert.DoesNotContain(GenerativeEngrammitizationLibraryCandidatePostureKind.SliFormedPosture, postureKinds);
    }

    [Fact]
    public void Missing_Engrammitization_Facing_Posture_Refuses_Readiness()
    {
        var record = GenerativeEngrammitizationLibraryFormationReadinessReferenceData.RefusedMissingEngrammitizationFacingPosture;
        var postureKinds = record.CandidatePostures.Select(static posture => posture.Kind).ToArray();

        Assert.Equal(GenerativeEngrammitizationLibraryFormationReadinessDisposition.Refused, record.Disposition);
        Assert.Contains(GenerativeEngrammitizationLibraryFormationReadinessRefusalReason.MissingEngrammitizationFacingPosture, record.RefusalReasons);
        Assert.DoesNotContain(GenerativeEngrammitizationLibraryCandidatePostureKind.EngrammitizationFacingPosture, postureKinds);
    }

    [Fact]
    public void Survivor_Localized_First_Use_Runtime_And_Governance_Overclaims_Refuse()
    {
        var record = GenerativeEngrammitizationLibraryFormationReadinessReferenceData.RefusedSurvivorLocalizedFirstUseRuntimeOrGovernanceOverclaim;

        Assert.Equal(GenerativeEngrammitizationLibraryFormationReadinessDisposition.Refused, record.Disposition);
        Assert.Contains(GenerativeEngrammitizationLibraryFormationReadinessRefusalReason.SurvivorAdmissionOverclaimed, record.RefusalReasons);
        Assert.Contains(GenerativeEngrammitizationLibraryFormationReadinessRefusalReason.LocalizedSanctuaryGelFormationOverclaimed, record.RefusalReasons);
        Assert.Contains(GenerativeEngrammitizationLibraryFormationReadinessRefusalReason.FirstUseOverclaimed, record.RefusalReasons);
        Assert.Contains(GenerativeEngrammitizationLibraryFormationReadinessRefusalReason.RuntimeOrGovernanceOverclaimed, record.RefusalReasons);
        Assert.Contains(".GEL inclusion does not grant final Sanctuary.GEL survivor admission, localized formation, standing, first-use admission, governing CME, RTME, or runtime authority.", record.NonAdmissionSummary, StringComparison.Ordinal);
    }

    [Fact]
    public void Gel_Readiness_Does_Not_Introduce_Service_Evaluator_Runtime_Or_Admission_Owner()
    {
        var repoRoot = GetRepoRoot();
        var srcRoot = Path.Combine(repoRoot, "src");

        var sourceFiles = Directory
            .EnumerateFiles(srcRoot, "*.cs", SearchOption.AllDirectories)
            .Where(path =>
                !path.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}", StringComparison.OrdinalIgnoreCase) &&
                !path.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}", StringComparison.OrdinalIgnoreCase))
            .ToArray();

        Assert.DoesNotContain(sourceFiles, path =>
            path.Contains($"{Path.DirectorySeparatorChar}San.Nexus.Control{Path.DirectorySeparatorChar}", StringComparison.OrdinalIgnoreCase) &&
            Path.GetFileName(path).Contains("GenerativeEngrammitizationLibraryFormationReadiness", StringComparison.OrdinalIgnoreCase));

        Assert.DoesNotContain(sourceFiles, path =>
            Path.GetFileNameWithoutExtension(path).Contains("GenerativeEngrammitizationLibraryFormationReadiness", StringComparison.OrdinalIgnoreCase) &&
            (Path.GetFileNameWithoutExtension(path).Contains("Service", StringComparison.OrdinalIgnoreCase) ||
             Path.GetFileNameWithoutExtension(path).Contains("Evaluator", StringComparison.OrdinalIgnoreCase) ||
             Path.GetFileNameWithoutExtension(path).Contains("Runtime", StringComparison.OrdinalIgnoreCase) ||
             Path.GetFileNameWithoutExtension(path).Contains("Owner", StringComparison.OrdinalIgnoreCase) ||
             Path.GetFileNameWithoutExtension(path).Contains("Admission", StringComparison.OrdinalIgnoreCase)));
    }

    private static string GetRepoRoot()
    {
        var current = new DirectoryInfo(AppContext.BaseDirectory);

        while (current is not null &&
               (!File.Exists(Path.Combine(current.FullName, "build.ps1")) ||
                !File.Exists(Path.Combine(current.FullName, "San.sln"))))
        {
            current = current.Parent;
        }

        return current?.FullName ?? throw new InvalidOperationException("Unable to locate repository root.");
    }
}
