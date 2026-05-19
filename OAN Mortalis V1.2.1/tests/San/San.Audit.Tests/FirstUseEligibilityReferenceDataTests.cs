using San.Common;
using Xunit;

namespace San.Audit.Tests;

public sealed class FirstUseEligibilityReferenceDataTests
{
    [Fact]
    public void Passive_Contracts_Expose_First_Use_Eligibility_Posture_And_Refusal_Types()
    {
        Assert.Contains(FirstUseEligibilityDisposition.ReadyForConsideration, Enum.GetValues<FirstUseEligibilityDisposition>());
        Assert.Contains(FirstUseEligibilityDisposition.Held, Enum.GetValues<FirstUseEligibilityDisposition>());
        Assert.Contains(FirstUseEligibilityDisposition.Refused, Enum.GetValues<FirstUseEligibilityDisposition>());

        Assert.Contains(FirstUseEligibilityPostureKind.PredicateSurfaceReadiness, Enum.GetValues<FirstUseEligibilityPostureKind>());
        Assert.Contains(FirstUseEligibilityPostureKind.Disclosure, Enum.GetValues<FirstUseEligibilityPostureKind>());
        Assert.Contains(FirstUseEligibilityPostureKind.LocalData, Enum.GetValues<FirstUseEligibilityPostureKind>());
        Assert.Contains(FirstUseEligibilityPostureKind.OptOut, Enum.GetValues<FirstUseEligibilityPostureKind>());
        Assert.Contains(FirstUseEligibilityPostureKind.NonAuthority, Enum.GetValues<FirstUseEligibilityPostureKind>());

        Assert.Contains(FirstUseEligibilityRefusalReason.MissingPredicateSurfaceReadiness, Enum.GetValues<FirstUseEligibilityRefusalReason>());
        Assert.Contains(FirstUseEligibilityRefusalReason.CounselReviewOverclaimed, Enum.GetValues<FirstUseEligibilityRefusalReason>());
        Assert.Contains(FirstUseEligibilityRefusalReason.RuntimeOrGovernanceOverclaimed, Enum.GetValues<FirstUseEligibilityRefusalReason>());
    }

    [Fact]
    public void Canonical_Reference_Data_Includes_Ready_Held_And_Refused_Records()
    {
        Assert.Equal(FirstUseEligibilityDisposition.ReadyForConsideration, FirstUseEligibilityReferenceData.ReadyForConsideration.Disposition);
        Assert.Equal(FirstUseEligibilityDisposition.Held, FirstUseEligibilityReferenceData.HeldForReview.Disposition);
        Assert.Contains(FirstUseEligibilityReferenceData.CanonicalRecords, record => record.Disposition == FirstUseEligibilityDisposition.Refused);
    }

    [Fact]
    public void Ready_For_Consideration_Includes_All_Required_Postures_Without_Permission()
    {
        var record = FirstUseEligibilityReferenceData.ReadyForConsideration;
        var postures = record.Postures.ToDictionary(static posture => posture.Kind);

        foreach (var kind in Enum.GetValues<FirstUseEligibilityPostureKind>())
        {
            Assert.Contains(kind, postures.Keys);
        }

        Assert.Equal(FirstUseEligibilityPostureState.Represented, postures[FirstUseEligibilityPostureKind.PredicateSurfaceReadiness].State);
        Assert.Equal(FirstUseEligibilityPostureState.Held, postures[FirstUseEligibilityPostureKind.SpecialCaseHold].State);
        Assert.Equal(FirstUseEligibilityPostureState.Held, postures[FirstUseEligibilityPostureKind.DomainHold].State);
        Assert.Contains("not first-use permission", record.NonPermissionSummary, StringComparison.Ordinal);
    }

    [Fact]
    public void Missing_Predicate_Surface_Readiness_Refuses_Eligibility_Consideration()
    {
        var record = FirstUseEligibilityReferenceData.RefusedMissingPredicateSurfaceReadiness;
        var predicatePosture = record.Postures.Single(static posture => posture.Kind == FirstUseEligibilityPostureKind.PredicateSurfaceReadiness);

        Assert.Equal(FirstUseEligibilityDisposition.Refused, record.Disposition);
        Assert.Contains(FirstUseEligibilityRefusalReason.MissingPredicateSurfaceReadiness, record.RefusalReasons);
        Assert.Equal(FirstUseEligibilityPostureState.Missing, predicatePosture.State);
        Assert.Contains("predicate-surface readiness is missing", record.NonPermissionSummary, StringComparison.Ordinal);
    }

    [Fact]
    public void Missing_Disclosure_Data_Retention_Or_Opt_Out_Refuses_Eligibility_Consideration()
    {
        var disclosureData = FirstUseEligibilityReferenceData.RefusedMissingDisclosureOrDataPosture;
        var retentionOptOut = FirstUseEligibilityReferenceData.RefusedMissingRetentionOrOptOut;

        Assert.Contains(FirstUseEligibilityRefusalReason.MissingDisclosurePosture, disclosureData.RefusalReasons);
        Assert.Contains(FirstUseEligibilityRefusalReason.MissingLocalDataPosture, disclosureData.RefusalReasons);
        Assert.Contains(FirstUseEligibilityRefusalReason.MissingRetentionOrOptOutPosture, retentionOptOut.RefusalReasons);
        Assert.Equal(FirstUseEligibilityPostureState.Missing, disclosureData.Postures.Single(static posture => posture.Kind == FirstUseEligibilityPostureKind.Disclosure).State);
        Assert.Equal(FirstUseEligibilityPostureState.Missing, disclosureData.Postures.Single(static posture => posture.Kind == FirstUseEligibilityPostureKind.LocalData).State);
        Assert.Equal(FirstUseEligibilityPostureState.Missing, retentionOptOut.Postures.Single(static posture => posture.Kind == FirstUseEligibilityPostureKind.Retention).State);
        Assert.Equal(FirstUseEligibilityPostureState.Missing, retentionOptOut.Postures.Single(static posture => posture.Kind == FirstUseEligibilityPostureKind.OptOut).State);
    }

    [Fact]
    public void Special_Case_Domain_Research_Counsel_Runtime_And_Governance_Refusals_Are_Explicit()
    {
        var specialDomain = FirstUseEligibilityReferenceData.RefusedSpecialCaseOrDomainNotHeld;
        var research = FirstUseEligibilityReferenceData.RefusedResearchSeparationMissing;
        var overclaim = FirstUseEligibilityReferenceData.RefusedCounselRuntimeOrGovernanceOverclaim;

        Assert.Contains(FirstUseEligibilityRefusalReason.SpecialCaseNotHeld, specialDomain.RefusalReasons);
        Assert.Contains(FirstUseEligibilityRefusalReason.DomainUseNotHeld, specialDomain.RefusalReasons);
        Assert.Contains(FirstUseEligibilityRefusalReason.ResearchSeparationMissing, research.RefusalReasons);
        Assert.Contains(FirstUseEligibilityRefusalReason.CounselReviewOverclaimed, overclaim.RefusalReasons);
        Assert.Contains(FirstUseEligibilityRefusalReason.RuntimeOrGovernanceOverclaimed, overclaim.RefusalReasons);
        Assert.Contains("RTME", overclaim.NonPermissionSummary, StringComparison.Ordinal);
        Assert.Contains("domain authority", overclaim.NonPermissionSummary, StringComparison.Ordinal);
    }

    [Fact]
    public void First_Use_Eligibility_Does_Not_Introduce_Service_Evaluator_Runtime_Consent_Disclosure_Or_Domain_Authorization()
    {
        var repoRoot = GetRepoRoot();
        var srcRoot = Path.Combine(repoRoot, "OAN Mortalis V1.2.1", "src");

        var sourceFiles = Directory
            .EnumerateFiles(srcRoot, "*.cs", SearchOption.AllDirectories)
            .Where(path =>
                !path.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}", StringComparison.OrdinalIgnoreCase) &&
                !path.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}", StringComparison.OrdinalIgnoreCase))
            .ToArray();

        Assert.DoesNotContain(sourceFiles, path =>
            path.Contains($"{Path.DirectorySeparatorChar}San.Nexus.Control{Path.DirectorySeparatorChar}", StringComparison.OrdinalIgnoreCase) &&
            Path.GetFileName(path).Contains("FirstUseEligibility", StringComparison.OrdinalIgnoreCase));

        Assert.DoesNotContain(sourceFiles, path =>
            Path.GetFileNameWithoutExtension(path).Contains("FirstUseEligibility", StringComparison.OrdinalIgnoreCase) &&
            (Path.GetFileNameWithoutExtension(path).Contains("Service", StringComparison.OrdinalIgnoreCase) ||
             Path.GetFileNameWithoutExtension(path).Contains("Evaluator", StringComparison.OrdinalIgnoreCase) ||
             Path.GetFileNameWithoutExtension(path).Contains("Runtime", StringComparison.OrdinalIgnoreCase)));

        Assert.DoesNotContain(sourceFiles, path =>
            Path.GetFileNameWithoutExtension(path).Contains("ConsentEngine", StringComparison.OrdinalIgnoreCase) ||
            Path.GetFileNameWithoutExtension(path).Contains("DisclosureGenerator", StringComparison.OrdinalIgnoreCase) ||
            Path.GetFileNameWithoutExtension(path).Contains("LegalDocumentGenerator", StringComparison.OrdinalIgnoreCase) ||
            Path.GetFileNameWithoutExtension(path).Contains("DomainAuthorization", StringComparison.OrdinalIgnoreCase));
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
