using San.Common;
using Xunit;

namespace San.Audit.Tests;

public sealed class LegalAdminLabAssetTemplateStagingReferenceDataTests
{
    [Fact]
    public void Passive_Contracts_Expose_Staging_Statuses_And_Template_Families()
    {
        Assert.Contains(LegalAdminLabAssetTemplateStagingStatus.SourceResource, Enum.GetValues<LegalAdminLabAssetTemplateStagingStatus>());
        Assert.Contains(LegalAdminLabAssetTemplateStagingStatus.LabStaged, Enum.GetValues<LegalAdminLabAssetTemplateStagingStatus>());
        Assert.Contains(LegalAdminLabAssetTemplateStagingStatus.CounselReviewRequired, Enum.GetValues<LegalAdminLabAssetTemplateStagingStatus>());
        Assert.Contains(LegalAdminLabAssetTemplateStagingStatus.BuildReadyWithCounselReview, Enum.GetValues<LegalAdminLabAssetTemplateStagingStatus>());
        Assert.Contains(LegalAdminLabAssetTemplateStagingStatus.RefusedAsActiveTerms, Enum.GetValues<LegalAdminLabAssetTemplateStagingStatus>());

        Assert.Equal(7, Enum.GetValues<LegalAdminLabAssetTemplateFamily>().Length);
    }

    [Fact]
    public void Canonical_Reference_Data_Names_All_Seven_Legal_Admin_Template_Families()
    {
        var families = LegalAdminLabAssetTemplateStagingReferenceData.AllTemplateFamilies
            .Select(record => record.Family)
            .ToArray();

        Assert.Contains(LegalAdminLabAssetTemplateFamily.PreLocalCertificationDisclosure, families);
        Assert.Contains(LegalAdminLabAssetTemplateFamily.OperatorCmeBondLegalForm, families);
        Assert.Contains(LegalAdminLabAssetTemplateFamily.DomainSpecificCmeStanding, families);
        Assert.Contains(LegalAdminLabAssetTemplateFamily.CmeDataRightsResearchProtection, families);
        Assert.Contains(LegalAdminLabAssetTemplateFamily.PersonificationSpecialCase, families);
        Assert.Contains(LegalAdminLabAssetTemplateFamily.TopicalAccessRouting, families);
        Assert.Contains(LegalAdminLabAssetTemplateFamily.TrustedFailureReceiptTelemetry, families);
    }

    [Fact]
    public void Canonical_Records_Remain_Review_Candidates_Without_Template_Bodies_Or_Active_Terms()
    {
        foreach (var record in LegalAdminLabAssetTemplateStagingReferenceData.AllTemplateFamilies)
        {
            var driveSlashProbe = string.Concat("D:", "/");
            var driveBackslashProbe = string.Concat("D:", "\\");

            Assert.Equal(LegalAdminLabAssetTemplateStagingStatus.CounselReviewRequired, record.Status);
            Assert.Equal(LegalAdminLabAssetTemplateReviewPosture.ReviewRequired, record.ReviewPosture);
            Assert.Null(record.LabCandidateRef);
            Assert.StartsWith("legal-admin-template-family://", record.ResourceIdentity, StringComparison.Ordinal);
            Assert.Contains("review candidate only", record.NonAuthoritySummary, StringComparison.Ordinal);
            Assert.Contains("not active legal terms, consent, disclosure, certification, or operational authority", record.NonAuthoritySummary, StringComparison.Ordinal);
            Assert.Contains("Regional counsel review is required", record.CounselQuestionSummary, StringComparison.Ordinal);
            Assert.Contains("Refused as active terms", record.RefusalReason, StringComparison.Ordinal);
            Assert.DoesNotContain("Documentation Repo", record.LogicalSourceLabel, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain(driveSlashProbe, record.LogicalSourceLabel, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain(driveBackslashProbe, record.LogicalSourceLabel, StringComparison.OrdinalIgnoreCase);
        }
    }

    [Fact]
    public void Build_Ready_With_Counsel_Review_Is_Not_Runtime_Or_Consent_Activation()
    {
        var record = new LegalAdminLabAssetTemplateStagingRecord(
            Family: LegalAdminLabAssetTemplateFamily.PreLocalCertificationDisclosure,
            Status: LegalAdminLabAssetTemplateStagingStatus.BuildReadyWithCounselReview,
            ReviewPosture: LegalAdminLabAssetTemplateReviewPosture.CounselReviewedForBuildDocumentation,
            LogicalSourceLabel: "Counsel Reviewed Legal-Admin Template Candidate",
            ResourceIdentity: "legal-admin-template-family://build-ready-with-counsel-review",
            LabCandidateRef: null,
            CounselQuestionSummary: "Counsel review admitted build documentation only.",
            RefusalReason: "Still refused as runtime authority or consent activation.",
            NonAuthoritySummary: "Build-ready with counsel review admits documentation only, not runtime authority, consent activation, certification, or domain authorization.",
            WitnessRefs: new[]
            {
                "legal-admin-template-staging://build-documentation-only"
            });

        Assert.Equal(LegalAdminLabAssetTemplateStagingStatus.BuildReadyWithCounselReview, record.Status);
        Assert.Contains("documentation only", record.NonAuthoritySummary, StringComparison.Ordinal);
        Assert.Contains("not runtime authority, consent activation, certification, or domain authorization", record.NonAuthoritySummary, StringComparison.Ordinal);
    }

    [Fact]
    public void Legal_Admin_Staging_Does_Not_Introduce_Control_Service_Consent_Engine_Or_Template_Runtime()
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
            Path.GetFileName(path).Contains("LegalAdmin", StringComparison.OrdinalIgnoreCase));

        Assert.DoesNotContain(sourceFiles, path =>
            Path.GetFileNameWithoutExtension(path).Contains("LegalAdmin", StringComparison.OrdinalIgnoreCase) &&
            (Path.GetFileNameWithoutExtension(path).Contains("Service", StringComparison.OrdinalIgnoreCase) ||
             Path.GetFileNameWithoutExtension(path).Contains("Engine", StringComparison.OrdinalIgnoreCase) ||
             Path.GetFileNameWithoutExtension(path).Contains("Generator", StringComparison.OrdinalIgnoreCase) ||
             Path.GetFileNameWithoutExtension(path).Contains("Runtime", StringComparison.OrdinalIgnoreCase)));

        Assert.DoesNotContain(sourceFiles, path =>
            Path.GetFileNameWithoutExtension(path).Contains("ConsentEngine", StringComparison.OrdinalIgnoreCase) ||
            Path.GetFileNameWithoutExtension(path).Contains("CertificationService", StringComparison.OrdinalIgnoreCase) ||
            Path.GetFileNameWithoutExtension(path).Contains("LegalDocumentGenerator", StringComparison.OrdinalIgnoreCase));
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
