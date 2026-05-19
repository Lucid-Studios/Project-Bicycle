using San.Common;
using Xunit;

namespace San.Audit.Tests;

public sealed class LocalizedPreCertificationDataPoolReferenceDataTests
{
    [Fact]
    public void Passive_Contracts_Expose_Pre_Certification_Input_And_Refusal_Types()
    {
        Assert.Contains(LocalizedPreCertificationDataPoolDisposition.Ready, Enum.GetValues<LocalizedPreCertificationDataPoolDisposition>());
        Assert.Contains(LocalizedPreCertificationDataPoolDisposition.Held, Enum.GetValues<LocalizedPreCertificationDataPoolDisposition>());
        Assert.Contains(LocalizedPreCertificationDataPoolDisposition.Refused, Enum.GetValues<LocalizedPreCertificationDataPoolDisposition>());

        Assert.Contains(LocalizedPreCertificationDataPoolInputKind.LabAssetCandidate, Enum.GetValues<LocalizedPreCertificationDataPoolInputKind>());
        Assert.Contains(LocalizedPreCertificationDataPoolInputKind.RootAtlasRegionalPosture, Enum.GetValues<LocalizedPreCertificationDataPoolInputKind>());
        Assert.Contains(LocalizedPreCertificationDataPoolInputKind.LegalAdminTemplateFamily, Enum.GetValues<LocalizedPreCertificationDataPoolInputKind>());
        Assert.Contains(LocalizedPreCertificationDataPoolInputKind.NationalStanding, Enum.GetValues<LocalizedPreCertificationDataPoolInputKind>());
        Assert.Contains(LocalizedPreCertificationDataPoolInputKind.NonAuthoritySummary, Enum.GetValues<LocalizedPreCertificationDataPoolInputKind>());

        Assert.Contains(LocalizedPreCertificationDataPoolRefusalReason.ActiveLegalTermsOverclaimed, Enum.GetValues<LocalizedPreCertificationDataPoolRefusalReason>());
        Assert.Contains(LocalizedPreCertificationDataPoolRefusalReason.FirstUseAdmissionOverclaimed, Enum.GetValues<LocalizedPreCertificationDataPoolRefusalReason>());
        Assert.Contains(LocalizedPreCertificationDataPoolRefusalReason.RuntimeAuthorityOverclaimed, Enum.GetValues<LocalizedPreCertificationDataPoolRefusalReason>());
    }

    [Fact]
    public void Canonical_Reference_Data_Includes_Ready_Held_And_Refused_Records()
    {
        Assert.Equal(LocalizedPreCertificationDataPoolDisposition.Ready, LocalizedPreCertificationDataPoolReferenceData.ReadyPreCertificationPool.Disposition);
        Assert.Equal(LocalizedPreCertificationDataPoolDisposition.Held, LocalizedPreCertificationDataPoolReferenceData.HeldForReview.Disposition);
        Assert.Contains(LocalizedPreCertificationDataPoolReferenceData.CanonicalRecords, record => record.Disposition == LocalizedPreCertificationDataPoolDisposition.Refused);
    }

    [Fact]
    public void Ready_Pre_Certification_Pool_Includes_All_Input_Kinds_And_Remains_Non_Authoritative()
    {
        var record = LocalizedPreCertificationDataPoolReferenceData.ReadyPreCertificationPool;
        var inputKinds = record.Inputs.Select(static input => input.Kind).ToArray();

        foreach (var kind in Enum.GetValues<LocalizedPreCertificationDataPoolInputKind>())
        {
            Assert.Contains(kind, inputKinds);
        }

        Assert.Contains("localized-standing://national", record.SourceLocalizedStandingRefs);
        Assert.Contains("legal-admin-template-family://trusted-failure-receipt-telemetry", record.SourceLegalAdminStagingRefs);
        Assert.Contains("does not certify, disclose, consent, authorize, govern, activate runtime", record.NonAuthoritySummary, StringComparison.Ordinal);
        Assert.Contains("mutate Atlas authority", record.NonAuthoritySummary, StringComparison.Ordinal);
        Assert.Contains("legal template bodies", record.NonAuthoritySummary, StringComparison.Ordinal);
    }

    [Fact]
    public void Missing_National_Regional_Or_Local_Standing_Refuses_The_Pool()
    {
        var record = LocalizedPreCertificationDataPoolReferenceData.RefusedMissingStandingRepresentation;
        var inputKinds = record.Inputs.Select(static input => input.Kind).ToArray();

        Assert.Equal(LocalizedPreCertificationDataPoolDisposition.Refused, record.Disposition);
        Assert.Contains(LocalizedPreCertificationDataPoolRefusalReason.MissingNationalStanding, record.RefusalReasons);
        Assert.Contains(LocalizedPreCertificationDataPoolRefusalReason.MissingRegionalStanding, record.RefusalReasons);
        Assert.Contains(LocalizedPreCertificationDataPoolRefusalReason.MissingLocalStanding, record.RefusalReasons);
        Assert.DoesNotContain(LocalizedPreCertificationDataPoolInputKind.NationalStanding, inputKinds);
        Assert.DoesNotContain(LocalizedPreCertificationDataPoolInputKind.RegionalStanding, inputKinds);
        Assert.DoesNotContain(LocalizedPreCertificationDataPoolInputKind.LocalStanding, inputKinds);
    }

    [Fact]
    public void Legal_Certification_Consent_Disclosure_Domain_First_Use_Rtme_Governance_And_Runtime_Overclaims_Refuse()
    {
        var legalOrCert = LocalizedPreCertificationDataPoolReferenceData.RefusedActiveLegalTermsOrCertificationOverclaim;
        var consentOrDisclosure = LocalizedPreCertificationDataPoolReferenceData.RefusedConsentOrDisclosureOverclaim;
        var domainRuntime = LocalizedPreCertificationDataPoolReferenceData.RefusedDomainFirstUseRtmeGovernanceOrRuntimeOverclaim;

        Assert.Contains(LocalizedPreCertificationDataPoolRefusalReason.ActiveLegalTermsOverclaimed, legalOrCert.RefusalReasons);
        Assert.Contains(LocalizedPreCertificationDataPoolRefusalReason.CertificationOverclaimed, legalOrCert.RefusalReasons);
        Assert.Contains(LocalizedPreCertificationDataPoolRefusalReason.ConsentRecordOverclaimed, consentOrDisclosure.RefusalReasons);
        Assert.Contains(LocalizedPreCertificationDataPoolRefusalReason.DisclosureIssuanceOverclaimed, consentOrDisclosure.RefusalReasons);
        Assert.Contains(LocalizedPreCertificationDataPoolRefusalReason.DomainAuthorizationOverclaimed, domainRuntime.RefusalReasons);
        Assert.Contains(LocalizedPreCertificationDataPoolRefusalReason.FirstUseAdmissionOverclaimed, domainRuntime.RefusalReasons);
        Assert.Contains(LocalizedPreCertificationDataPoolRefusalReason.RtmeOverclaimed, domainRuntime.RefusalReasons);
        Assert.Contains(LocalizedPreCertificationDataPoolRefusalReason.GoverningCmeOverclaimed, domainRuntime.RefusalReasons);
        Assert.Contains(LocalizedPreCertificationDataPoolRefusalReason.RuntimeAuthorityOverclaimed, domainRuntime.RefusalReasons);
        Assert.Contains("No certification, disclosure, consent, authorization, governance, RTME, first-use admission, governing CME, Atlas mutation, legal template body, or runtime authority is granted.", domainRuntime.NonAuthoritySummary, StringComparison.Ordinal);
    }

    [Fact]
    public void Pre_Certification_Data_Pool_Does_Not_Introduce_Service_Evaluator_Runtime_Consent_Disclosure_Or_Domain_Authorization()
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
            Path.GetFileName(path).Contains("PreCertificationDataPool", StringComparison.OrdinalIgnoreCase));

        Assert.DoesNotContain(sourceFiles, path =>
            Path.GetFileNameWithoutExtension(path).Contains("PreCertificationDataPool", StringComparison.OrdinalIgnoreCase) &&
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
