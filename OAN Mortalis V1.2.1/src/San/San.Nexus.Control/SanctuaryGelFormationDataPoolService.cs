using System.Security.Cryptography;
using System.Text;
using San.Common;

namespace San.Nexus.Control;

public interface ISanctuaryGelFormationDataPoolService
{
    SanctuaryGelFormationDataPoolAssessment EvaluateDataPool(
        SanctuaryGelFormationCredentialFooting credentialFooting,
        LocalizedInstallChoiceMatrix choiceMatrix,
        AgreementPredicateBundle agreementBundle,
        InstallIdentitySetCandidate? installIdentity,
        CoreCmeUsePostureRecord usePosture,
        RegionalAtlasPackageAdmissionAssessment regionalAtlasAdmission,
        SanctuaryGelFormationPredicateInheritance predicateInheritance,
        CradleTekSiteBindingProfile? siteBindingProfile = null);
}

public sealed class DefaultSanctuaryGelFormationDataPoolService : ISanctuaryGelFormationDataPoolService
{
    public SanctuaryGelFormationDataPoolAssessment EvaluateDataPool(
        SanctuaryGelFormationCredentialFooting credentialFooting,
        LocalizedInstallChoiceMatrix choiceMatrix,
        AgreementPredicateBundle agreementBundle,
        InstallIdentitySetCandidate? installIdentity,
        CoreCmeUsePostureRecord usePosture,
        RegionalAtlasPackageAdmissionAssessment regionalAtlasAdmission,
        SanctuaryGelFormationPredicateInheritance predicateInheritance,
        CradleTekSiteBindingProfile? siteBindingProfile = null)
    {
        ArgumentNullException.ThrowIfNull(credentialFooting);
        ArgumentNullException.ThrowIfNull(choiceMatrix);
        ArgumentNullException.ThrowIfNull(agreementBundle);
        ArgumentNullException.ThrowIfNull(usePosture);
        ArgumentNullException.ThrowIfNull(regionalAtlasAdmission);
        ArgumentNullException.ThrowIfNull(predicateInheritance);

        var witnessRefs = new List<string>
        {
            choiceMatrix.ChoiceMatrixId,
            agreementBundle.BundleId,
            usePosture.PostureId,
            regionalAtlasAdmission.Receipt.ReceiptHandle,
            predicateInheritance.PredicateInheritanceWitness
        };

        if (!string.IsNullOrWhiteSpace(installIdentity?.IdentitySetId))
        {
            witnessRefs.Add(installIdentity.IdentitySetId);
        }

        if (!string.IsNullOrWhiteSpace(siteBindingProfile?.SiteBindingProfileId))
        {
            witnessRefs.Add(siteBindingProfile.SiteBindingProfileId);
        }

        var untrustedCommunication =
            !credentialFooting.CertifiedCommunicationBasis ||
            string.IsNullOrWhiteSpace(credentialFooting.LicensingAgentId) ||
            string.IsNullOrWhiteSpace(credentialFooting.UserId) ||
            predicateInheritance.UniversalAtlasAuthorityClaimed;

        SanctuaryGelFormationDataPoolDisposition disposition;
        string outcomeCode;
        string summary;
        SanctuaryGelFormationDataPool? dataPool = null;

        if (untrustedCommunication)
        {
            disposition = SanctuaryGelFormationDataPoolDisposition.Silence;
            outcomeCode = "sanctuary-gel-formation-data-pool-silence";
            summary = "Formation data pool remained silent because certified communication footing was not established.";
        }
        else
        {
            var refused =
                installIdentity is null ||
                regionalAtlasAdmission.Disposition != RegionalAtlasPackageDisposition.Admitted ||
                regionalAtlasAdmission.PackageIdentity is null ||
                string.IsNullOrWhiteSpace(predicateInheritance.PredicateLineageSummary) ||
                string.IsNullOrWhiteSpace(predicateInheritance.PredicateInheritanceWitness) ||
                !agreementBundle.FullAssent;

            if (refused)
            {
                disposition = SanctuaryGelFormationDataPoolDisposition.Refused;
                outcomeCode = "sanctuary-gel-formation-data-pool-refused";
                summary = "Formation data pool refused because recognized install-lane footing was incomplete for first Sanctuary.GEL formation.";
            }
            else
            {
                var installIdentityValue = installIdentity;
                var packageIdentity = regionalAtlasAdmission.PackageIdentity;

                disposition = SanctuaryGelFormationDataPoolDisposition.Ready;
                outcomeCode = "sanctuary-gel-formation-data-pool-ready";
                summary = "Formation data pool is ready as the bounded install-side inheritance body for first Sanctuary.GEL formation.";
                dataPool = new SanctuaryGelFormationDataPool(
                    Identity: new SanctuaryGelFormationDataPoolIdentity(
                        PoolHandle: CreateHandle("sanctuary-gel-formation-data-pool://", choiceMatrix.ChoiceMatrixId, installIdentityValue!.IdentitySetId, packageIdentity!.PackageHandle),
                        EnvironmentHandle: installIdentityValue.IdentitySetId,
                        ReceiptHandle: CreateHandle("sanctuary-gel-formation-data-pool-receipt://", choiceMatrix.ChoiceMatrixId, installIdentityValue.IdentitySetId)),
                    CredentialFooting: credentialFooting,
                    ChoiceMatrix: choiceMatrix,
                    AgreementBundle: agreementBundle,
                    InstallIdentity: installIdentityValue,
                    UsePosture: usePosture,
                    RegionalAtlasPackage: packageIdentity,
                    PredicateInheritance: predicateInheritance,
                    SiteBindingProfile: siteBindingProfile);
            }
        }

        var receipt = new SanctuaryGelFormationDataPoolReceipt(
            ReceiptHandle: CreateHandle("sanctuary-gel-formation-data-pool-receipt://", choiceMatrix.ChoiceMatrixId, outcomeCode),
            Disposition: disposition,
            Summary: summary,
            WitnessRefs: witnessRefs,
            TimestampUtc: DateTimeOffset.UtcNow);

        return new SanctuaryGelFormationDataPoolAssessment(
            Disposition: disposition,
            OutcomeCode: outcomeCode,
            Summary: summary,
            DataPool: dataPool,
            Receipt: receipt);
    }

    private static string CreateHandle(string prefix, params string[] parts)
    {
        var material = string.Join("|", parts.Select(static part => part?.Trim() ?? string.Empty));
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(material));
        return $"{prefix}{Convert.ToHexString(hash).ToLowerInvariant()[..16]}";
    }
}
