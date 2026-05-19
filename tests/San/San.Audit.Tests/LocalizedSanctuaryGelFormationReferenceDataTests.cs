using San.Common;
using Xunit;

namespace San.Audit.Tests;

public sealed class LocalizedSanctuaryGelFormationReferenceDataTests
{
    [Fact]
    public void Passive_Contracts_Expose_Localized_Formation_Standing_And_Refusal_Types()
    {
        Assert.Contains(LocalizedSanctuaryGelFormationDisposition.Ready, Enum.GetValues<LocalizedSanctuaryGelFormationDisposition>());
        Assert.Contains(LocalizedSanctuaryGelFormationDisposition.Held, Enum.GetValues<LocalizedSanctuaryGelFormationDisposition>());
        Assert.Contains(LocalizedSanctuaryGelFormationDisposition.Refused, Enum.GetValues<LocalizedSanctuaryGelFormationDisposition>());

        Assert.Contains(LocalizedStandingRepresentationLayer.National, Enum.GetValues<LocalizedStandingRepresentationLayer>());
        Assert.Contains(LocalizedStandingRepresentationLayer.Regional, Enum.GetValues<LocalizedStandingRepresentationLayer>());
        Assert.Contains(LocalizedStandingRepresentationLayer.Local, Enum.GetValues<LocalizedStandingRepresentationLayer>());

        Assert.Contains(LocalizedSanctuaryGelFormationRefusalReason.MissingNationalStanding, Enum.GetValues<LocalizedSanctuaryGelFormationRefusalReason>());
        Assert.Contains(LocalizedSanctuaryGelFormationRefusalReason.MissingRegionalStanding, Enum.GetValues<LocalizedSanctuaryGelFormationRefusalReason>());
        Assert.Contains(LocalizedSanctuaryGelFormationRefusalReason.MissingLocalStanding, Enum.GetValues<LocalizedSanctuaryGelFormationRefusalReason>());
        Assert.Contains(LocalizedSanctuaryGelFormationRefusalReason.OverclaimsGovernance, Enum.GetValues<LocalizedSanctuaryGelFormationRefusalReason>());
    }

    [Fact]
    public void Canonical_Reference_Data_Includes_Ready_Held_And_Refused_Records()
    {
        Assert.Equal(LocalizedSanctuaryGelFormationDisposition.Ready, LocalizedSanctuaryGelFormationReferenceData.ReadyLocalizedFormation.Disposition);
        Assert.Equal(LocalizedSanctuaryGelFormationDisposition.Held, LocalizedSanctuaryGelFormationReferenceData.HeldForContextReview.Disposition);
        Assert.Contains(LocalizedSanctuaryGelFormationReferenceData.CanonicalRecords, record => record.Disposition == LocalizedSanctuaryGelFormationDisposition.Refused);
    }

    [Fact]
    public void Ready_Localized_Formation_Represents_National_Regional_And_Local_Standing()
    {
        var layers = LocalizedSanctuaryGelFormationReferenceData.ReadyLocalizedFormation.StandingRepresentations
            .Select(representation => representation.Layer)
            .ToArray();

        Assert.Contains(LocalizedStandingRepresentationLayer.National, layers);
        Assert.Contains(LocalizedStandingRepresentationLayer.Regional, layers);
        Assert.Contains(LocalizedStandingRepresentationLayer.Local, layers);
        Assert.Equal(LocalizedSanctuaryGelFormationRefusalReason.None, LocalizedSanctuaryGelFormationReferenceData.ReadyLocalizedFormation.RefusalReason);
    }

    [Fact]
    public void Missing_Standing_Refusals_Remove_The_Missing_Layer_And_Name_The_Refusal()
    {
        AssertMissingLayer(
            LocalizedSanctuaryGelFormationReferenceData.RefusedMissingNationalStanding,
            LocalizedStandingRepresentationLayer.National,
            LocalizedSanctuaryGelFormationRefusalReason.MissingNationalStanding);

        AssertMissingLayer(
            LocalizedSanctuaryGelFormationReferenceData.RefusedMissingRegionalStanding,
            LocalizedStandingRepresentationLayer.Regional,
            LocalizedSanctuaryGelFormationRefusalReason.MissingRegionalStanding);

        AssertMissingLayer(
            LocalizedSanctuaryGelFormationReferenceData.RefusedMissingLocalStanding,
            LocalizedStandingRepresentationLayer.Local,
            LocalizedSanctuaryGelFormationRefusalReason.MissingLocalStanding);
    }

    [Fact]
    public void Governance_Overclaim_Refuses_Localized_Formation()
    {
        var record = LocalizedSanctuaryGelFormationReferenceData.RefusedGovernanceOverclaim;

        Assert.Equal(LocalizedSanctuaryGelFormationDisposition.Refused, record.Disposition);
        Assert.Equal(LocalizedSanctuaryGelFormationRefusalReason.OverclaimsGovernance, record.RefusalReason);
        Assert.Contains("overclaims governance", record.NonAuthoritySummary, StringComparison.Ordinal);
        Assert.Contains("No first use, governance, RTME, counsel-reviewed disclosure, consent, domain authority, or runtime authority is granted.", record.NonAuthoritySummary, StringComparison.Ordinal);
    }

    [Fact]
    public void Ready_Localized_Formation_Remains_Pre_Governing_And_Non_Runtime()
    {
        var record = LocalizedSanctuaryGelFormationReferenceData.ReadyLocalizedFormation;

        Assert.Contains("pre-governing", record.NonAuthoritySummary, StringComparison.Ordinal);
        Assert.Contains("no first use, governance, RTME, counsel-reviewed disclosure, consent, domain authority, or runtime authority", record.NonAuthoritySummary, StringComparison.Ordinal);
        Assert.Contains("continuity-bearing-personal-data", record.DataRightsPosture, StringComparison.Ordinal);
        Assert.Contains("review-candidates-only", record.LegalAdminStagingPosture, StringComparison.Ordinal);
    }

    [Fact]
    public void Localized_Formation_Does_Not_Introduce_Service_Evaluator_Runtime_Consent_Or_Domain_Authorization()
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
            Path.GetFileName(path).Contains("LocalizedSanctuaryGelFormation", StringComparison.OrdinalIgnoreCase));

        Assert.DoesNotContain(sourceFiles, path =>
            Path.GetFileNameWithoutExtension(path).Contains("LocalizedSanctuaryGelFormation", StringComparison.OrdinalIgnoreCase) &&
            (Path.GetFileNameWithoutExtension(path).Contains("Service", StringComparison.OrdinalIgnoreCase) ||
             Path.GetFileNameWithoutExtension(path).Contains("Evaluator", StringComparison.OrdinalIgnoreCase) ||
             Path.GetFileNameWithoutExtension(path).Contains("Runtime", StringComparison.OrdinalIgnoreCase)));

        Assert.DoesNotContain(sourceFiles, path =>
            Path.GetFileNameWithoutExtension(path).Contains("ConsentEngine", StringComparison.OrdinalIgnoreCase) ||
            Path.GetFileNameWithoutExtension(path).Contains("LegalDocumentGenerator", StringComparison.OrdinalIgnoreCase) ||
            Path.GetFileNameWithoutExtension(path).Contains("DomainAuthorization", StringComparison.OrdinalIgnoreCase));
    }

    private static void AssertMissingLayer(
        LocalizedSanctuaryGelFormationRecord record,
        LocalizedStandingRepresentationLayer missingLayer,
        LocalizedSanctuaryGelFormationRefusalReason expectedReason)
    {
        var layers = record.StandingRepresentations
            .Select(representation => representation.Layer)
            .ToArray();

        Assert.Equal(LocalizedSanctuaryGelFormationDisposition.Refused, record.Disposition);
        Assert.Equal(expectedReason, record.RefusalReason);
        Assert.DoesNotContain(missingLayer, layers);
        Assert.Contains("No first use, governance, RTME, counsel-reviewed disclosure, consent, domain authority, or runtime authority is granted.", record.NonAuthoritySummary, StringComparison.Ordinal);
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
