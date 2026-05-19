using San.Common;
using San.Nexus.Control;
using Xunit;

namespace San.Audit.Tests;

public sealed class SanctuaryGelPredicatePoolContractsTests
{
    private readonly DefaultSanctuaryGelFormationDataPoolService _dataPoolService = new();
    private readonly DefaultSanctuaryGelPredicatePoolService _predicatePoolService = new();
    private readonly DefaultSanctuaryGelFormationService _formationService = new();

    [Fact]
    public void EvaluatePredicatePool_Returns_Silence_When_Upstream_Data_Pool_Is_Silent()
    {
        var input = CreateValidPoolInputs();

        var dataPoolAssessment = _dataPoolService.EvaluateDataPool(
            input.CredentialFooting with { CertifiedCommunicationBasis = false },
            input.ChoiceMatrix,
            input.AgreementBundle,
            input.InstallIdentity,
            input.UsePosture,
            input.RegionalAdmission,
            input.PredicateInheritance,
            input.SiteBindingProfile);

        var result = _predicatePoolService.EvaluatePredicatePool(dataPoolAssessment);

        Assert.Equal(SanctuaryGelPredicatePoolDisposition.Silence, result.Disposition);
        Assert.Null(result.PredicatePool);
        Assert.Equal("sanctuary-gel-predicate-pool-silence", result.OutcomeCode);
    }

    [Fact]
    public void EvaluatePredicatePool_Returns_Refused_When_Upstream_Data_Pool_Is_Refused()
    {
        var input = CreateValidPoolInputs();

        var dataPoolAssessment = _dataPoolService.EvaluateDataPool(
            input.CredentialFooting,
            input.ChoiceMatrix,
            input.AgreementBundle,
            null,
            input.UsePosture,
            input.RegionalAdmission,
            input.PredicateInheritance,
            input.SiteBindingProfile);

        var result = _predicatePoolService.EvaluatePredicatePool(dataPoolAssessment);

        Assert.Equal(SanctuaryGelPredicatePoolDisposition.Refused, result.Disposition);
        Assert.Null(result.PredicatePool);
        Assert.Equal("sanctuary-gel-predicate-pool-refused", result.OutcomeCode);
    }

    [Fact]
    public void EvaluatePredicatePool_Returns_Ready_With_Bounded_Local_Predicate_Body()
    {
        var input = CreateValidPoolInputs();

        var dataPoolAssessment = _dataPoolService.EvaluateDataPool(
            input.CredentialFooting,
            input.ChoiceMatrix,
            input.AgreementBundle,
            input.InstallIdentity,
            input.UsePosture,
            input.RegionalAdmission,
            input.PredicateInheritance,
            input.SiteBindingProfile);

        var result = _predicatePoolService.EvaluatePredicatePool(dataPoolAssessment);

        Assert.Equal(SanctuaryGelPredicatePoolDisposition.Ready, result.Disposition);
        Assert.NotNull(result.PredicatePool);
        Assert.Equal("English", result.PredicatePool!.ActiveLanguage);
        Assert.Equal("English", result.PredicatePool.RegionalAtlasPackage.LanguageGroup);
        Assert.NotEmpty(result.PredicatePool.Candidates);
        Assert.Equal(
            new[]
            {
                SanctuaryGelPredicateFamily.Posture,
                SanctuaryGelPredicateFamily.TrustAuthorization,
                SanctuaryGelPredicateFamily.EvidenceFooting,
                SanctuaryGelPredicateFamily.ResponseDisposition
            },
            result.PredicatePool.FamilySets.Select(static familySet => familySet.Family).ToArray());
        Assert.Contains(result.PredicatePool.Candidates, candidate => candidate.Kind == SanctuaryGelPredicateCandidateKind.InstallFacing);
        Assert.Contains(result.PredicatePool.Candidates, candidate => candidate.Kind == SanctuaryGelPredicateCandidateKind.CertifiedCommunication);
        Assert.Contains(result.PredicatePool.Candidates, candidate => candidate.Kind == SanctuaryGelPredicateCandidateKind.AssentWitnessed);
        Assert.Contains(result.PredicatePool.Candidates, candidate => candidate.Kind == SanctuaryGelPredicateCandidateKind.Ready);
    }

    [Fact]
    public void Gel_Formation_Only_Proceeds_From_Ready_Predicate_Pool()
    {
        var input = CreateValidPoolInputs();

        var dataPoolAssessment = _dataPoolService.EvaluateDataPool(
            input.CredentialFooting,
            input.ChoiceMatrix,
            input.AgreementBundle,
            input.InstallIdentity,
            input.UsePosture,
            input.RegionalAdmission,
            input.PredicateInheritance,
            input.SiteBindingProfile);
        var predicatePoolAssessment = _predicatePoolService.EvaluatePredicatePool(dataPoolAssessment);

        var result = _formationService.EvaluateFormation(CreateFormationInput(predicatePoolAssessment));

        Assert.Equal(SanctuaryGelFormationDisposition.Retained, result.Disposition);
        Assert.NotNull(result.SubstrateRecord);
        Assert.Equal(predicatePoolAssessment.PredicatePool!.Identity.PoolHandle, result.SubstrateRecord!.PredicatePoolHandle);
        Assert.Contains(SanctuaryGelPredicateFamily.Posture, result.SubstrateRecord.PredicateFamilies);
        Assert.Contains(SanctuaryGelPredicateCandidateKind.GoverningSeatCandidate, result.SubstrateRecord.InheritedPredicateKinds);
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
