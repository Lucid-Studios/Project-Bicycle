using San.Common;
using Xunit;

namespace San.Audit.Tests;

public sealed class SanctuaryGelRegionalSubstrateFormationReferenceDataTests
{
    [Fact]
    public void Passive_Contracts_Expose_Regional_Substrate_Types()
    {
        Assert.Contains(SanctuaryGelRegionalSubstrateFormationDisposition.Ready, Enum.GetValues<SanctuaryGelRegionalSubstrateFormationDisposition>());
        Assert.Contains(SanctuaryGelRegionalSubstrateFormationDisposition.Held, Enum.GetValues<SanctuaryGelRegionalSubstrateFormationDisposition>());
        Assert.Contains(SanctuaryGelRegionalSubstrateFormationDisposition.Refused, Enum.GetValues<SanctuaryGelRegionalSubstrateFormationDisposition>());

        Assert.Contains(SanctuaryGelRegionalSubstrateAdmissionCeiling.CandidateOnly, Enum.GetValues<SanctuaryGelRegionalSubstrateAdmissionCeiling>());
        Assert.Contains(SanctuaryGelRegionalSubstrateAdmissionCeiling.RegionalSubstrateOnly, Enum.GetValues<SanctuaryGelRegionalSubstrateAdmissionCeiling>());

        Assert.Contains(SanctuaryGelRegionalSubstrateRefusalReason.MissingPredicatePriorRefs, Enum.GetValues<SanctuaryGelRegionalSubstrateRefusalReason>());
        Assert.Contains(SanctuaryGelRegionalSubstrateRefusalReason.SanctuaryActualOverclaimed, Enum.GetValues<SanctuaryGelRegionalSubstrateRefusalReason>());
        Assert.Contains(SanctuaryGelRegionalSubstrateRefusalReason.CradleGelGenerationOverclaimed, Enum.GetValues<SanctuaryGelRegionalSubstrateRefusalReason>());
        Assert.Contains(SanctuaryGelRegionalSubstrateRefusalReason.GovernanceOrRuntimeOverclaimed, Enum.GetValues<SanctuaryGelRegionalSubstrateRefusalReason>());
    }

    [Fact]
    public void Canonical_Reference_Data_Includes_Ready_Held_And_Refused_Records()
    {
        Assert.Equal(SanctuaryGelRegionalSubstrateFormationDisposition.Ready, SanctuaryGelRegionalSubstrateFormationReferenceData.ReadyRegionalSubstrate.Disposition);
        Assert.Equal(SanctuaryGelRegionalSubstrateFormationDisposition.Held, SanctuaryGelRegionalSubstrateFormationReferenceData.HeldForRegionalOrGovernanceReview.Disposition);
        Assert.Contains(SanctuaryGelRegionalSubstrateFormationReferenceData.CanonicalRecords, record => record.Disposition == SanctuaryGelRegionalSubstrateFormationDisposition.Refused);
    }

    [Fact]
    public void Ready_Regional_Substrate_Carries_Required_Refs_And_Remains_Non_Authoritative()
    {
        var record = SanctuaryGelRegionalSubstrateFormationReferenceData.ReadyRegionalSubstrate;

        Assert.Equal("sanctuary-gel-regional-substrate-ref://ready", record.Identity.SubstrateHandle);
        Assert.Equal("sanctuary-body-ref://full-program-body", record.Identity.SanctuaryBodyRef);
        Assert.Equal("regional-package-footing-ref://english-us", record.Identity.RegionalPackageFootingRef);
        Assert.Contains("gel-predicate-prior-ref://rt-accept-107", record.PredicatePriorRefs);
        Assert.Contains("localized-pre-certification-data-pool-ref://ready", record.LocalizedPreCertificationDataPoolRefs);
        Assert.Contains("localized-standing-ref://national", record.StandingRefs);
        Assert.Contains("sanctuary-gel-predicate-family-ref://posture", record.PredicateFamilyRefs);
        Assert.Equal(SanctuaryGelRegionalSubstrateAdmissionCeiling.RegionalSubstrateOnly, record.AdmissionCeiling);
        Assert.Contains("does not stand Sanctuary.Actual, select models, authorize governance, activate runtime, or generate Cradle.GEL", record.NonAuthoritySummary, StringComparison.Ordinal);
    }

    [Fact]
    public void Held_Regional_Substrate_Remains_Candidate_Only()
    {
        var record = SanctuaryGelRegionalSubstrateFormationReferenceData.HeldForRegionalOrGovernanceReview;

        Assert.Equal(SanctuaryGelRegionalSubstrateFormationDisposition.Held, record.Disposition);
        Assert.Equal(SanctuaryGelRegionalSubstrateAdmissionCeiling.CandidateOnly, record.AdmissionCeiling);
        Assert.Contains("candidate-only", record.NonAuthoritySummary, StringComparison.Ordinal);
    }

    [Fact]
    public void Missing_Predicate_Prior_Pre_Certification_Standing_Or_Regional_Footing_Refuses()
    {
        var missingPriorOrPool = SanctuaryGelRegionalSubstrateFormationReferenceData.RefusedMissingPredicatePriorOrPreCertificationPool;
        var missingStandingOrPackage = SanctuaryGelRegionalSubstrateFormationReferenceData.RefusedMissingStandingOrRegionalPackageFooting;

        Assert.Contains(SanctuaryGelRegionalSubstrateRefusalReason.MissingPredicatePriorRefs, missingPriorOrPool.RefusalReasons);
        Assert.Contains(SanctuaryGelRegionalSubstrateRefusalReason.MissingLocalizedPreCertificationDataPool, missingPriorOrPool.RefusalReasons);
        Assert.Empty(missingPriorOrPool.PredicatePriorRefs);
        Assert.Empty(missingPriorOrPool.LocalizedPreCertificationDataPoolRefs);

        Assert.Contains(SanctuaryGelRegionalSubstrateRefusalReason.MissingNationalStanding, missingStandingOrPackage.RefusalReasons);
        Assert.Contains(SanctuaryGelRegionalSubstrateRefusalReason.MissingRegionalStanding, missingStandingOrPackage.RefusalReasons);
        Assert.Contains(SanctuaryGelRegionalSubstrateRefusalReason.MissingLocalStanding, missingStandingOrPackage.RefusalReasons);
        Assert.Contains(SanctuaryGelRegionalSubstrateRefusalReason.MissingRegionalPackageFooting, missingStandingOrPackage.RefusalReasons);
        Assert.Empty(missingStandingOrPackage.StandingRefs);
        Assert.Equal("missing", missingStandingOrPackage.Identity.RegionalPackageFootingRef);
    }

    [Fact]
    public void SanctuaryActual_Model_Cradle_Governance_And_Runtime_Overclaims_Refuse()
    {
        var record = SanctuaryGelRegionalSubstrateFormationReferenceData.RefusedSanctuaryActualModelCradleGovernanceOrRuntimeOverclaim;

        Assert.Equal(SanctuaryGelRegionalSubstrateFormationDisposition.Refused, record.Disposition);
        Assert.Contains(SanctuaryGelRegionalSubstrateRefusalReason.SanctuaryActualOverclaimed, record.RefusalReasons);
        Assert.Contains(SanctuaryGelRegionalSubstrateRefusalReason.MotherFatherGoverningCmeOverclaimed, record.RefusalReasons);
        Assert.Contains(SanctuaryGelRegionalSubstrateRefusalReason.ModelSelectionOverclaimed, record.RefusalReasons);
        Assert.Contains(SanctuaryGelRegionalSubstrateRefusalReason.CradleGelGenerationOverclaimed, record.RefusalReasons);
        Assert.Contains(SanctuaryGelRegionalSubstrateRefusalReason.GovernanceOrRuntimeOverclaimed, record.RefusalReasons);
        Assert.Contains("No Sanctuary.Actual, Mother/Father governing CME, model selection, Cradle.GEL generation, governance, runtime, first use, RTME, or survivor admission is granted.", record.NonAuthoritySummary, StringComparison.Ordinal);
    }

    [Fact]
    public void Regional_Substrate_Does_Not_Introduce_Service_Evaluator_Runtime_Model_Selection_Or_Cradle_Generation()
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
            Path.GetFileName(path).Contains("SanctuaryGelRegionalSubstrate", StringComparison.OrdinalIgnoreCase));

        Assert.DoesNotContain(sourceFiles, path =>
            Path.GetFileNameWithoutExtension(path).Contains("SanctuaryGelRegionalSubstrate", StringComparison.OrdinalIgnoreCase) &&
            (Path.GetFileNameWithoutExtension(path).Contains("Service", StringComparison.OrdinalIgnoreCase) ||
             Path.GetFileNameWithoutExtension(path).Contains("Evaluator", StringComparison.OrdinalIgnoreCase) ||
             Path.GetFileNameWithoutExtension(path).Contains("Runtime", StringComparison.OrdinalIgnoreCase) ||
             Path.GetFileNameWithoutExtension(path).Contains("Owner", StringComparison.OrdinalIgnoreCase)));

        Assert.DoesNotContain(sourceFiles, path =>
            Path.GetFileNameWithoutExtension(path).Contains("ModelSelection", StringComparison.OrdinalIgnoreCase) ||
            Path.GetFileNameWithoutExtension(path).Contains("CradleGelGeneration", StringComparison.OrdinalIgnoreCase) ||
            Path.GetFileNameWithoutExtension(path).Contains("SanctuaryActual", StringComparison.OrdinalIgnoreCase));
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
