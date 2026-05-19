using San.Common;
using San.Nexus.Control;
using Xunit;

namespace San.Audit.Tests;

public sealed class RegionalAtlasPackageAdmissionContractsTests
{
    private readonly DefaultRegionalAtlasPackageAdmissionService _service = new();

    [Fact]
    public void EvaluateAdmission_Refuses_When_Assent_Footing_Is_Incomplete()
    {
        var input = CreateValidInput() with
        {
            ChoiceMatrix = CreateChoiceMatrix(AgreementAssentState.Acknowledged)
        };

        var result = _service.EvaluateAdmission(input);

        Assert.Equal(RegionalAtlasPackageDisposition.Refused, result.Disposition);
        Assert.Null(result.PackageIdentity);
        Assert.Equal("regional-atlas-package-admission-refused", result.OutcomeCode);
    }

    [Fact]
    public void EvaluateAdmission_Refuses_When_Package_Witness_Is_Missing()
    {
        var input = CreateValidInput() with
        {
            PackageWitness = string.Empty
        };

        var result = _service.EvaluateAdmission(input);

        Assert.Equal(RegionalAtlasPackageDisposition.Refused, result.Disposition);
        Assert.Null(result.PackageIdentity);
        Assert.Equal("regional-atlas-package-admission-refused", result.OutcomeCode);
    }

    [Fact]
    public void EvaluateAdmission_Refuses_When_Universal_Authority_Is_Claimed()
    {
        var input = CreateValidInput() with
        {
            UniversalAtlasAuthorityClaimed = true
        };

        var result = _service.EvaluateAdmission(input);

        Assert.Equal(RegionalAtlasPackageDisposition.Refused, result.Disposition);
        Assert.Null(result.PackageIdentity);
        Assert.Equal("regional-atlas-package-admission-refused", result.OutcomeCode);
    }

    [Fact]
    public void EvaluateAdmission_Refuses_When_Non_English_Package_Is_Requested()
    {
        var input = CreateValidInput() with
        {
            Selection = new RegionalAtlasPackageSelection(
                RequestedPackageKind: RegionalAtlasPackageKind.EnglishRegionalAtlasPackage,
                RequestedLanguageGroup: "French",
                RequestedLocale: "fr-FR",
                RequestedJurisdiction: "FR")
        };

        var result = _service.EvaluateAdmission(input);

        Assert.Equal(RegionalAtlasPackageDisposition.Refused, result.Disposition);
        Assert.Null(result.PackageIdentity);
        Assert.Equal("regional-atlas-package-admission-refused", result.OutcomeCode);
    }

    [Fact]
    public void EvaluateAdmission_Admits_Bounded_English_Package_When_Footing_Is_Complete()
    {
        var input = CreateValidInput();

        var result = _service.EvaluateAdmission(input);

        Assert.Equal(RegionalAtlasPackageDisposition.Admitted, result.Disposition);
        Assert.NotNull(result.PackageIdentity);
        Assert.Equal(RegionalAtlasPackageKind.EnglishRegionalAtlasPackage, result.PackageIdentity!.PackageKind);
        Assert.Equal("English", result.PackageIdentity.LanguageGroup);
        Assert.Equal("en-US", result.PackageIdentity.Locale);
        Assert.Equal(input.SignedPayloadLineage, result.PackageIdentity.SignedPayloadLineage);
        Assert.Equal("regional-atlas-package-admission-admitted", result.OutcomeCode);
    }

    private static RegionalAtlasPackageAdmissionInput CreateValidInput()
    {
        var choiceMatrix = CreateChoiceMatrix(AgreementAssentState.Assented);

        return new RegionalAtlasPackageAdmissionInput(
            ChoiceMatrix: choiceMatrix,
            InstallIdentity: new InstallIdentitySetCandidate(
                IdentitySetId: "install-identity://0001",
                BundleId: "agreement-bundle://0001",
                LicensingAgentId: "licensing-agent://lab",
                UserId: "user://0001",
                LanguageDatasetId: choiceMatrix.LanguageDataset.DatasetId,
                Locale: choiceMatrix.LanguageDataset.Locale,
                Jurisdiction: choiceMatrix.LanguageDataset.Jurisdiction,
                AssentWitnessByLane: new Dictionary<string, string>
                {
                    ["service-license-predicate"] = "witness://service-license",
                    ["terms-of-service-predicate"] = "witness://terms",
                    ["bonded-operator-predicate"] = "witness://bonded-operator",
                    ["cme-lab-notice-predicate"] = "witness://lab-notice",
                    ["research-data-practice-predicate"] = "witness://data-practice",
                    ["access-attachment-profile-predicate"] = "witness://attachment-profile"
                },
                AgreementTemplateLineage: new[]
                {
                    "agreement-template://eng/0001"
                },
                CmeLabNoticeLineageRef: "agreement-template://eng/lab-notice"),
            Selection: new RegionalAtlasPackageSelection(
                RequestedPackageKind: RegionalAtlasPackageKind.EnglishRegionalAtlasPackage,
                RequestedLanguageGroup: "English",
                RequestedLocale: "en-US",
                RequestedJurisdiction: "US"),
            SignedPayloadLineage: "payload://atlas-package/english/0001",
            PackageWitness: "atlas-package-witness://english/0001",
            VerificationWitness: "atlas-package-verification://english/0001",
            UniversalAtlasAuthorityClaimed: false);
    }

    private static LocalizedInstallChoiceMatrix CreateChoiceMatrix(AgreementAssentState assentState)
    {
        var assentStates = Enum.GetValues<AgreementPredicateKind>()
            .ToDictionary(kind => kind, _ => assentState);

        var witnessRefs = Enum.GetValues<AgreementPredicateKind>()
            .ToDictionary(
                kind => kind,
                kind => (IReadOnlyList<string>)new[]
                {
                    $"witness://{kind}"
                });

        var templateRefs = Enum.GetValues<AgreementPredicateKind>()
            .ToDictionary(kind => kind, kind => $"template://{kind}");

        return new LocalizedInstallChoiceMatrix(
            ChoiceMatrixId: "choice-matrix://0001",
            LanguageDataset: new LanguageSelectionDataset(
                DatasetId: "langset://eng/0001",
                ActiveLanguage: "English",
                Locale: "en-US",
                Jurisdiction: "US",
                LegalFormFamily: "common-law",
                NamingConventions: new[] { "given-family" },
                ScriptExpectations: new[] { "Latin" },
                ReservedPredicateBindings: new[] { "service-license-predicate" },
                LocalLexemeAllowances: new[] { "consent" },
                UnresolvedLexemeLanes: Array.Empty<string>(),
                DatasetPosture: "localized-install"),
            FootingSection: new LocalizedFootingSection(
                FootingSectionId: "footing://0001",
                WhoRef: "who://user",
                WhatRef: "what://install",
                WhenRef: "when://now",
                WhereRef: "where://region-us",
                WhyRef: "why://install",
                HowRef: "how://localized-assent",
                BiographicalScope: "local-user",
                LocalScope: "regional-english",
                GovernmentalScope: "us",
                LegalPosture: "install-assent",
                LocalizationAuthorizationState: "authorized"),
            LicensingAgentId: "licensing-agent://lab",
            UserId: "user://0001",
            AgreementTemplateLineage: new[]
            {
                "agreement-template://eng/0001"
            },
            AgreementTemplateRefs: templateRefs,
            AgreementAssentStates: assentStates,
            AgreementWitnessRefs: witnessRefs);
    }
}
