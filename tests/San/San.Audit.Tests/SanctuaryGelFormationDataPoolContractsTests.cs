using San.Common;
using San.Nexus.Control;
using Xunit;

namespace San.Audit.Tests;

public sealed class SanctuaryGelFormationDataPoolContractsTests
{
    private readonly DefaultSanctuaryGelFormationDataPoolService _poolService = new();
    private readonly DefaultSanctuaryGelPredicatePoolService _predicatePoolService = new();
    private readonly DefaultSanctuaryGelFormationService _formationService = new();

    [Fact]
    public void EvaluateDataPool_Returns_Silence_When_Credential_Footing_Is_Missing()
    {
        var input = CreateValidPoolInputs();
        var credentialFooting = input.CredentialFooting with
        {
            CertifiedCommunicationBasis = false
        };

        var result = _poolService.EvaluateDataPool(
            credentialFooting,
            input.ChoiceMatrix,
            input.AgreementBundle,
            input.InstallIdentity,
            input.UsePosture,
            input.RegionalAdmission,
            input.PredicateInheritance,
            input.SiteBindingProfile);

        Assert.Equal(SanctuaryGelFormationDataPoolDisposition.Silence, result.Disposition);
        Assert.Null(result.DataPool);
        Assert.Equal("sanctuary-gel-formation-data-pool-silence", result.OutcomeCode);
    }

    [Fact]
    public void EvaluateDataPool_Returns_Silence_When_Universal_Authority_Is_Claimed()
    {
        var input = CreateValidPoolInputs();
        var predicateInheritance = input.PredicateInheritance with
        {
            UniversalAtlasAuthorityClaimed = true
        };

        var result = _poolService.EvaluateDataPool(
            input.CredentialFooting,
            input.ChoiceMatrix,
            input.AgreementBundle,
            input.InstallIdentity,
            input.UsePosture,
            input.RegionalAdmission,
            predicateInheritance,
            input.SiteBindingProfile);

        Assert.Equal(SanctuaryGelFormationDataPoolDisposition.Silence, result.Disposition);
        Assert.Null(result.DataPool);
        Assert.Equal("sanctuary-gel-formation-data-pool-silence", result.OutcomeCode);
    }

    [Fact]
    public void EvaluateDataPool_Returns_Refused_When_Install_Identity_Is_Missing()
    {
        var input = CreateValidPoolInputs();

        var result = _poolService.EvaluateDataPool(
            input.CredentialFooting,
            input.ChoiceMatrix,
            input.AgreementBundle,
            null,
            input.UsePosture,
            input.RegionalAdmission,
            input.PredicateInheritance,
            input.SiteBindingProfile);

        Assert.Equal(SanctuaryGelFormationDataPoolDisposition.Refused, result.Disposition);
        Assert.Null(result.DataPool);
        Assert.Equal("sanctuary-gel-formation-data-pool-refused", result.OutcomeCode);
    }

    [Fact]
    public void EvaluateDataPool_Returns_Refused_When_Regional_Package_Is_Not_Admitted()
    {
        var input = CreateValidPoolInputs();
        var refusedAdmission = input.RegionalAdmission with
        {
            Disposition = RegionalAtlasPackageDisposition.Refused,
            PackageIdentity = null
        };

        var result = _poolService.EvaluateDataPool(
            input.CredentialFooting,
            input.ChoiceMatrix,
            input.AgreementBundle,
            input.InstallIdentity,
            input.UsePosture,
            refusedAdmission,
            input.PredicateInheritance,
            input.SiteBindingProfile);

        Assert.Equal(SanctuaryGelFormationDataPoolDisposition.Refused, result.Disposition);
        Assert.Null(result.DataPool);
        Assert.Equal("sanctuary-gel-formation-data-pool-refused", result.OutcomeCode);
    }

    [Fact]
    public void EvaluateDataPool_Returns_Refused_When_Predicate_Inheritance_Is_Missing()
    {
        var input = CreateValidPoolInputs();
        var predicateInheritance = input.PredicateInheritance with
        {
            PredicateInheritanceWitness = string.Empty
        };

        var result = _poolService.EvaluateDataPool(
            input.CredentialFooting,
            input.ChoiceMatrix,
            input.AgreementBundle,
            input.InstallIdentity,
            input.UsePosture,
            input.RegionalAdmission,
            predicateInheritance,
            input.SiteBindingProfile);

        Assert.Equal(SanctuaryGelFormationDataPoolDisposition.Refused, result.Disposition);
        Assert.Null(result.DataPool);
        Assert.Equal("sanctuary-gel-formation-data-pool-refused", result.OutcomeCode);
    }

    [Fact]
    public void EvaluateDataPool_Returns_Ready_When_Bounded_Footing_Is_Complete()
    {
        var input = CreateValidPoolInputs();

        var result = _poolService.EvaluateDataPool(
            input.CredentialFooting,
            input.ChoiceMatrix,
            input.AgreementBundle,
            input.InstallIdentity,
            input.UsePosture,
            input.RegionalAdmission,
            input.PredicateInheritance,
            input.SiteBindingProfile);

        Assert.Equal(SanctuaryGelFormationDataPoolDisposition.Ready, result.Disposition);
        Assert.NotNull(result.DataPool);
        Assert.Equal("English", result.DataPool!.RegionalAtlasPackage.LanguageGroup);
        Assert.Equal(input.InstallIdentity.IdentitySetId, result.DataPool.InstallIdentity.IdentitySetId);
    }

    [Fact]
    public void Gel_Formation_Remains_Silent_When_Data_Pool_Is_Silent()
    {
        var input = CreateValidPoolInputs();

        var poolAssessment = _poolService.EvaluateDataPool(
            input.CredentialFooting with { CertifiedCommunicationBasis = false },
            input.ChoiceMatrix,
            input.AgreementBundle,
            input.InstallIdentity,
            input.UsePosture,
            input.RegionalAdmission,
            input.PredicateInheritance,
            input.SiteBindingProfile);

        var predicatePoolAssessment = _predicatePoolService.EvaluatePredicatePool(poolAssessment);
        var result = _formationService.EvaluateFormation(CreateFormationInput(predicatePoolAssessment));

        Assert.Equal(SanctuaryGelFormationDisposition.Refused, result.Disposition);
        Assert.Equal("sanctuary-gel-formation-silence", result.OutcomeCode);
        Assert.Null(result.SubstrateRecord);
    }

    [Fact]
    public void Gel_Formation_Refuses_To_Proceed_When_Data_Pool_Is_Refused()
    {
        var input = CreateValidPoolInputs();

        var poolAssessment = _poolService.EvaluateDataPool(
            input.CredentialFooting,
            input.ChoiceMatrix,
            input.AgreementBundle,
            null,
            input.UsePosture,
            input.RegionalAdmission,
            input.PredicateInheritance,
            input.SiteBindingProfile);

        var predicatePoolAssessment = _predicatePoolService.EvaluatePredicatePool(poolAssessment);
        var result = _formationService.EvaluateFormation(CreateFormationInput(predicatePoolAssessment));

        Assert.Equal(SanctuaryGelFormationDisposition.Refused, result.Disposition);
        Assert.Equal("sanctuary-gel-formation-refused", result.OutcomeCode);
        Assert.Null(result.SubstrateRecord);
    }

    [Fact]
    public void Gel_Formation_Retains_Only_When_Data_Pool_Is_Ready()
    {
        var input = CreateValidPoolInputs();

        var poolAssessment = _poolService.EvaluateDataPool(
            input.CredentialFooting,
            input.ChoiceMatrix,
            input.AgreementBundle,
            input.InstallIdentity,
            input.UsePosture,
            input.RegionalAdmission,
            input.PredicateInheritance,
            input.SiteBindingProfile);

        var predicatePoolAssessment = _predicatePoolService.EvaluatePredicatePool(poolAssessment);
        var result = _formationService.EvaluateFormation(CreateFormationInput(predicatePoolAssessment));

        Assert.Equal(SanctuaryGelFormationDisposition.Retained, result.Disposition);
        Assert.Equal("sanctuary-gel-formation-retained", result.OutcomeCode);
        Assert.NotNull(result.SubstrateRecord);
    }

    private static SanctuaryGelFormationInput CreateFormationInput(
        SanctuaryGelPredicatePoolAssessment predicatePoolAssessment)
    {
        return new SanctuaryGelFormationInput(
            PredicatePoolAssessment: predicatePoolAssessment,
            DerivedPayloadLineage: "payload://sanctuary-gel/bootstrap/first-formation",
            SymbolicAnchorSummary: "bounded-anchor-summary://first-formation",
            SymbolicTransformWitness: "transform-witness://iutt-bounded-order",
            EngrammatizationWitness: "engrammitization-witness://first-local-pass",
            SubstrateIdentity: new SanctuaryGelSubstrateIdentity(
                SubstrateHandle: "sanctuary-gel://env/local/0001",
                EnvironmentHandle: "environment://local/install/0001",
                FormationReceiptHandle: "formation-receipt://local/install/0001"),
            RawRootAtlasResidencyClaimed: false,
            LabSideTemplatingAuthorityClaimed: false,
            PublicProjectionRequested: false);
    }

    private static (
        SanctuaryGelFormationCredentialFooting CredentialFooting,
        LocalizedInstallChoiceMatrix ChoiceMatrix,
        AgreementPredicateBundle AgreementBundle,
        InstallIdentitySetCandidate InstallIdentity,
        CoreCmeUsePostureRecord UsePosture,
        RegionalAtlasPackageAdmissionAssessment RegionalAdmission,
        SanctuaryGelFormationPredicateInheritance PredicateInheritance,
        CradleTekSiteBindingProfile SiteBindingProfile) CreateValidPoolInputs()
    {
        var agreementKinds = Enum.GetValues<AgreementPredicateKind>();

        var choiceMatrix = new LocalizedInstallChoiceMatrix(
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
            AgreementTemplateLineage: new[] { "agreement-template://eng/0001" },
            AgreementTemplateRefs: agreementKinds.ToDictionary(kind => kind, kind => $"template://{kind}"),
            AgreementAssentStates: agreementKinds.ToDictionary(kind => kind, _ => AgreementAssentState.Assented),
            AgreementWitnessRefs: agreementKinds.ToDictionary(
                kind => kind,
                kind => (IReadOnlyList<string>)new[] { $"witness://{kind}" }));

        var agreementBundle = new AgreementPredicateBundle(
            BundleId: "agreement-bundle://0001",
            ChoiceMatrixId: choiceMatrix.ChoiceMatrixId,
            LicensingAgentId: choiceMatrix.LicensingAgentId,
            UserId: choiceMatrix.UserId,
            LanguageDatasetId: choiceMatrix.LanguageDataset.DatasetId,
            Locale: choiceMatrix.LanguageDataset.Locale,
            Jurisdiction: choiceMatrix.LanguageDataset.Jurisdiction,
            AgreementTemplateLineage: choiceMatrix.AgreementTemplateLineage,
            Predicates: agreementKinds.Select(
                kind => new AgreementPredicateRecord(
                    PredicateKind: kind,
                    PredicateLane: kind.ToString(),
                    AssentState: AgreementAssentState.Assented,
                    TemplateRef: choiceMatrix.AgreementTemplateRefs[kind],
                    WitnessRefs: choiceMatrix.AgreementWitnessRefs[kind],
                    FormationTrace: $"trace://{kind}")).ToArray(),
            FullAssent: true);

        var installIdentity = new InstallIdentitySetCandidate(
            IdentitySetId: "install-identity://0001",
            BundleId: agreementBundle.BundleId,
            LicensingAgentId: choiceMatrix.LicensingAgentId,
            UserId: choiceMatrix.UserId,
            LanguageDatasetId: choiceMatrix.LanguageDataset.DatasetId,
            Locale: choiceMatrix.LanguageDataset.Locale,
            Jurisdiction: choiceMatrix.LanguageDataset.Jurisdiction,
            AssentWitnessByLane: agreementKinds.ToDictionary(kind => kind.ToString(), kind => $"witness://{kind}"),
            AgreementTemplateLineage: choiceMatrix.AgreementTemplateLineage,
            CmeLabNoticeLineageRef: "agreement-template://eng/lab-notice");

        var usePosture = new CoreCmeUsePostureRecord(
            PostureId: "use-posture://0001",
            AttachmentProfile: "research-attached-default",
            CmeLabProductStanding: "lab-product",
            CmeLabTestStanding: "test-standing",
            DataPracticePosture: "agency-plus-research-context",
            LanguageDatasetId: choiceMatrix.LanguageDataset.DatasetId,
            ActiveLanguage: choiceMatrix.LanguageDataset.ActiveLanguage,
            Locale: choiceMatrix.LanguageDataset.Locale,
            Jurisdiction: choiceMatrix.LanguageDataset.Jurisdiction,
            AgreementLineageRefs: choiceMatrix.AgreementTemplateLineage);

        var regionalAdmissionInput = new RegionalAtlasPackageAdmissionInput(
            ChoiceMatrix: choiceMatrix,
            InstallIdentity: installIdentity,
            Selection: new RegionalAtlasPackageSelection(
                RequestedPackageKind: RegionalAtlasPackageKind.EnglishRegionalAtlasPackage,
                RequestedLanguageGroup: "English",
                RequestedLocale: "en-US",
                RequestedJurisdiction: "US"),
            SignedPayloadLineage: "payload://atlas-package/english/0001",
            PackageWitness: "atlas-package-witness://english/0001",
            VerificationWitness: "atlas-package-verification://english/0001",
            UniversalAtlasAuthorityClaimed: false);

        var regionalAdmission = new DefaultRegionalAtlasPackageAdmissionService().EvaluateAdmission(regionalAdmissionInput);

        var predicateInheritance = new SanctuaryGelFormationPredicateInheritance(
            PredicateLineageSummary: "predicate-lineage://eng/install/0001",
            PredicateInheritanceWitness: "predicate-inheritance-witness://eng/install/0001",
            UniversalAtlasAuthorityClaimed: false);

        var siteBindingProfile = new CradleTekSiteBindingProfile(
            SiteBindingProfileId: "site-binding://0001",
            SiteClass: CradleTekSiteClass.PersonalPc,
            SiteSummary: "personal-pc",
            SanctuaryHostFooting: "co-resident",
            CradleTekHostFooting: "local",
            JurisdictionProfile: "US",
            LocalizationProfile: "en-US",
            WitnessRefs: new[] { "site-witness://0001" });

        var credentialFooting = new SanctuaryGelFormationCredentialFooting(
            LicensingAgentId: choiceMatrix.LicensingAgentId,
            UserId: choiceMatrix.UserId,
            CertifiedCommunicationBasis: true);

        return (
            credentialFooting,
            choiceMatrix,
            agreementBundle,
            installIdentity,
            usePosture,
            regionalAdmission,
            predicateInheritance,
            siteBindingProfile);
    }
}
