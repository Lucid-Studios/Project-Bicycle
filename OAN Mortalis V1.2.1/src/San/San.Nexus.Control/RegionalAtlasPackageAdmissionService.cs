using System.Security.Cryptography;
using System.Text;
using San.Common;

namespace San.Nexus.Control;

public interface IRegionalAtlasPackageAdmissionService
{
    RegionalAtlasPackageAdmissionAssessment EvaluateAdmission(RegionalAtlasPackageAdmissionInput input);
}

public sealed class DefaultRegionalAtlasPackageAdmissionService : IRegionalAtlasPackageAdmissionService
{
    public RegionalAtlasPackageAdmissionAssessment EvaluateAdmission(RegionalAtlasPackageAdmissionInput input)
    {
        ArgumentNullException.ThrowIfNull(input);
        ArgumentNullException.ThrowIfNull(input.ChoiceMatrix);
        ArgumentNullException.ThrowIfNull(input.Selection);

        var witnessRefs = new List<string>
        {
            input.ChoiceMatrix.ChoiceMatrixId,
            input.SignedPayloadLineage,
            input.PackageWitness,
            input.VerificationWitness
        };

        if (input.InstallIdentity is not null)
        {
            witnessRefs.Add(input.InstallIdentity.IdentitySetId);
        }

        var assentComplete = input.ChoiceMatrix.AgreementAssentStates.Count > 0 &&
            input.ChoiceMatrix.AgreementAssentStates.Values.All(static state => state == AgreementAssentState.Assented);

        var englishPackageRequested =
            input.Selection.RequestedPackageKind == RegionalAtlasPackageKind.EnglishRegionalAtlasPackage &&
            string.Equals(input.Selection.RequestedLanguageGroup, "English", StringComparison.Ordinal) &&
            string.Equals(input.ChoiceMatrix.LanguageDataset.ActiveLanguage, "English", StringComparison.Ordinal);

        var installIdentityPresent =
            input.InstallIdentity is not null &&
            !string.IsNullOrWhiteSpace(input.InstallIdentity.IdentitySetId) &&
            string.Equals(input.InstallIdentity.LanguageDatasetId, input.ChoiceMatrix.LanguageDataset.DatasetId, StringComparison.Ordinal) &&
            string.Equals(input.InstallIdentity.Locale, input.ChoiceMatrix.LanguageDataset.Locale, StringComparison.Ordinal) &&
            string.Equals(input.InstallIdentity.Jurisdiction, input.ChoiceMatrix.LanguageDataset.Jurisdiction, StringComparison.Ordinal);

        var refused =
            !assentComplete ||
            !installIdentityPresent ||
            string.IsNullOrWhiteSpace(input.SignedPayloadLineage) ||
            string.IsNullOrWhiteSpace(input.PackageWitness) ||
            string.IsNullOrWhiteSpace(input.VerificationWitness) ||
            string.IsNullOrWhiteSpace(input.Selection.RequestedLocale) ||
            string.IsNullOrWhiteSpace(input.Selection.RequestedJurisdiction) ||
            input.UniversalAtlasAuthorityClaimed ||
            !englishPackageRequested;

        RegionalAtlasPackageIdentity? packageIdentity = null;
        RegionalAtlasPackageDisposition disposition;
        string outcomeCode;
        string summary;

        if (refused)
        {
            disposition = RegionalAtlasPackageDisposition.Refused;
            outcomeCode = "regional-atlas-package-admission-refused";
            summary = "Regional Atlas package admission refused because localized assent footing, package witness, or English-only bounded package constraints were not satisfied.";
        }
        else
        {
            disposition = RegionalAtlasPackageDisposition.Admitted;
            outcomeCode = "regional-atlas-package-admission-admitted";
            summary = "Regional Atlas package admission admitted the bounded English package as local install footing before first Sanctuary.GEL formation.";
            packageIdentity = new RegionalAtlasPackageIdentity(
                PackageHandle: CreateHandle("regional-atlas-package://", input.ChoiceMatrix.ChoiceMatrixId, input.Selection.RequestedLanguageGroup, input.SignedPayloadLineage),
                PackageKind: input.Selection.RequestedPackageKind,
                LanguageGroup: input.Selection.RequestedLanguageGroup,
                Locale: input.Selection.RequestedLocale,
                SignedPayloadLineage: input.SignedPayloadLineage);
        }

        var receipt = new RegionalAtlasPackageAdmissionReceipt(
            ReceiptHandle: CreateHandle("regional-atlas-package-receipt://", input.ChoiceMatrix.ChoiceMatrixId, outcomeCode),
            Disposition: disposition,
            Summary: summary,
            WitnessRefs: witnessRefs,
            TimestampUtc: DateTimeOffset.UtcNow);

        return new RegionalAtlasPackageAdmissionAssessment(
            Input: input,
            Disposition: disposition,
            OutcomeCode: outcomeCode,
            Summary: summary,
            PackageIdentity: packageIdentity,
            Receipt: receipt);
    }

    private static string CreateHandle(string prefix, params string[] parts)
    {
        var material = string.Join("|", parts.Select(static part => part?.Trim() ?? string.Empty));
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(material));
        return $"{prefix}{Convert.ToHexString(hash).ToLowerInvariant()[..16]}";
    }
}
