using San.Common;
using San.Nexus.Control;
using Xunit;

namespace San.Audit.Tests;

public sealed class SanctuaryGelFormationContractsTests
{
    private readonly DefaultSanctuaryGelFormationService _service = new();
    private readonly DefaultSanctuaryGelFormationDataPoolService _poolService = new();
    private readonly DefaultSanctuaryGelPredicatePoolService _predicatePoolService = new();

    [Fact]
    public void EvaluateFormation_Refuses_When_Tripartite_Witness_Is_Missing()
    {
        var input = CreateValidInput() with
        {
            EngrammatizationWitness = string.Empty
        };

        var result = _service.EvaluateFormation(input);

        Assert.Equal(SanctuaryGelFormationDisposition.Refused, result.Disposition);
        Assert.Null(result.SubstrateRecord);
        Assert.Equal("sanctuary-gel-formation-refused", result.OutcomeCode);
    }

    [Fact]
    public void EvaluateFormation_Refuses_When_Raw_Source_Authority_Is_Claimed()
    {
        var input = CreateValidInput() with
        {
            RawRootAtlasResidencyClaimed = true
        };

        var result = _service.EvaluateFormation(input);

        Assert.Equal(SanctuaryGelFormationDisposition.Refused, result.Disposition);
        Assert.Null(result.SubstrateRecord);
        Assert.Equal("sanctuary-gel-formation-refused", result.OutcomeCode);
    }

    [Fact]
    public void EvaluateFormation_Retains_First_Local_Substrate_Record_When_Bounded_Input_Is_Sufficient()
    {
        var input = CreateValidInput();

        var result = _service.EvaluateFormation(input);

        Assert.Equal(SanctuaryGelFormationDisposition.Retained, result.Disposition);
        Assert.NotNull(result.SubstrateRecord);
        Assert.Equal(SanctuaryGelFormationDisposition.Retained, result.SubstrateRecord!.State);
        Assert.True(result.SubstrateRecord.Retained);
        Assert.True(result.SubstrateRecord.RestCapable);
        Assert.Equal(input.SubstrateIdentity.SubstrateHandle, result.SubstrateRecord.Identity.SubstrateHandle);
        Assert.Equal(input.DerivedPayloadLineage, result.SubstrateRecord.DerivedPayloadLineage);
        Assert.Equal(input.SymbolicAnchorSummary, result.SubstrateRecord.SymbolicAnchorSummary);
        Assert.Equal(input.PredicatePoolAssessment.PredicatePool!.Identity.PoolHandle, result.SubstrateRecord.PredicatePoolHandle);
        Assert.Contains(SanctuaryGelPredicateFamily.Posture, result.SubstrateRecord.PredicateFamilies);
        Assert.Contains(SanctuaryGelPredicateFamily.TrustAuthorization, result.SubstrateRecord.PredicateFamilies);
        Assert.Contains(SanctuaryGelPredicateCandidateKind.Ready, result.SubstrateRecord.InheritedPredicateKinds);
    }

    private static SanctuaryGelFormationInput CreateValidInput()
    {
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
            AgreementTemplateRefs: Enum.GetValues<AgreementPredicateKind>().ToDictionary(kind => kind, kind => $"template://{kind}"),
            AgreementAssentStates: Enum.GetValues<AgreementPredicateKind>().ToDictionary(kind => kind, _ => AgreementAssentState.Assented),
            AgreementWitnessRefs: Enum.GetValues<AgreementPredicateKind>().ToDictionary(
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
            Predicates: Enum.GetValues<AgreementPredicateKind>().Select(
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
            AssentWitnessByLane: Enum.GetValues<AgreementPredicateKind>().ToDictionary(kind => kind.ToString(), kind => $"witness://{kind}"),
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

        var regionalAdmission = new RegionalAtlasPackageAdmissionAssessment(
            Input: new RegionalAtlasPackageAdmissionInput(
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
                UniversalAtlasAuthorityClaimed: false),
            Disposition: RegionalAtlasPackageDisposition.Admitted,
            OutcomeCode: "regional-atlas-package-admission-admitted",
            Summary: "admitted",
            PackageIdentity: new RegionalAtlasPackageIdentity(
                PackageHandle: "regional-atlas-package://english/0001",
                PackageKind: RegionalAtlasPackageKind.EnglishRegionalAtlasPackage,
                LanguageGroup: "English",
                Locale: "en-US",
                SignedPayloadLineage: "payload://atlas-package/english/0001"),
            Receipt: new RegionalAtlasPackageAdmissionReceipt(
                ReceiptHandle: "regional-atlas-package-receipt://english/0001",
                Disposition: RegionalAtlasPackageDisposition.Admitted,
                Summary: "admitted",
                WitnessRefs: new[] { "atlas-package-witness://english/0001" },
                TimestampUtc: DateTimeOffset.UtcNow));

        var dataPoolAssessment = new DefaultSanctuaryGelFormationDataPoolService().EvaluateDataPool(
            new SanctuaryGelFormationCredentialFooting(
                LicensingAgentId: choiceMatrix.LicensingAgentId,
                UserId: choiceMatrix.UserId,
                CertifiedCommunicationBasis: true),
            choiceMatrix,
            agreementBundle,
            installIdentity,
            usePosture,
            regionalAdmission,
            new SanctuaryGelFormationPredicateInheritance(
                PredicateLineageSummary: "predicate-lineage://eng/install/0001",
                PredicateInheritanceWitness: "predicate-inheritance-witness://eng/install/0001",
                UniversalAtlasAuthorityClaimed: false),
            null);
        var predicatePoolAssessment = new DefaultSanctuaryGelPredicatePoolService().EvaluatePredicatePool(dataPoolAssessment);

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
}
