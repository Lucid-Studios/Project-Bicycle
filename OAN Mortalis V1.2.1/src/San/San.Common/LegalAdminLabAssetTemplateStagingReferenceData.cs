namespace San.Common;

public static class LegalAdminLabAssetTemplateStagingReferenceData
{
    private const string NonAuthoritySummary =
        "Recognized legal-admin template family is a review candidate only, not active legal terms, consent, disclosure, certification, or operational authority.";

    private const string CounselQuestionSummary =
        "Regional counsel review is required before this family can become build-ready documentation.";

    private const string RefusalReason =
        "Refused as active terms without Lab staging and regional counsel review.";

    public static LegalAdminLabAssetTemplateStagingRecord PreLocalCertificationDisclosure { get; } = Create(
        LegalAdminLabAssetTemplateFamily.PreLocalCertificationDisclosure,
        "Sanctuary Pre-Local Certification Disclosure Boundary",
        "legal-admin-template-family://pre-local-certification-disclosure");

    public static LegalAdminLabAssetTemplateStagingRecord OperatorCmeBondLegalForm { get; } = Create(
        LegalAdminLabAssetTemplateFamily.OperatorCmeBondLegalForm,
        "Operator-CME Bond Legal Form Options",
        "legal-admin-template-family://operator-cme-bond-legal-form");

    public static LegalAdminLabAssetTemplateStagingRecord DomainSpecificCmeStanding { get; } = Create(
        LegalAdminLabAssetTemplateFamily.DomainSpecificCmeStanding,
        "Domain-Specific CME Legal Standing Boundary",
        "legal-admin-template-family://domain-specific-cme-standing");

    public static LegalAdminLabAssetTemplateStagingRecord CmeDataRightsResearchProtection { get; } = Create(
        LegalAdminLabAssetTemplateFamily.CmeDataRightsResearchProtection,
        "CME Data Rights And Research Protection Boundary",
        "legal-admin-template-family://cme-data-rights-research-protection");

    public static LegalAdminLabAssetTemplateStagingRecord PersonificationSpecialCase { get; } = Create(
        LegalAdminLabAssetTemplateFamily.PersonificationSpecialCase,
        "Personification Research Special Case Boundary",
        "legal-admin-template-family://personification-special-case");

    public static LegalAdminLabAssetTemplateStagingRecord TopicalAccessRouting { get; } = Create(
        LegalAdminLabAssetTemplateFamily.TopicalAccessRouting,
        "Topical Access And Coverage Routing Boundary",
        "legal-admin-template-family://topical-access-routing");

    public static LegalAdminLabAssetTemplateStagingRecord TrustedFailureReceiptTelemetry { get; } = Create(
        LegalAdminLabAssetTemplateFamily.TrustedFailureReceiptTelemetry,
        "Trusted Failure And Receipt Telemetry Boundary",
        "legal-admin-template-family://trusted-failure-receipt-telemetry");

    public static IReadOnlyList<LegalAdminLabAssetTemplateStagingRecord> AllTemplateFamilies { get; } = new[]
    {
        PreLocalCertificationDisclosure,
        OperatorCmeBondLegalForm,
        DomainSpecificCmeStanding,
        CmeDataRightsResearchProtection,
        PersonificationSpecialCase,
        TopicalAccessRouting,
        TrustedFailureReceiptTelemetry
    };

    public static LegalAdminLabAssetTemplateStagingReceipt CounselReviewRequiredReceipt { get; } = new(
        ReceiptHandle: "legal-admin-template-staging-receipt://counsel-review-required",
        Family: LegalAdminLabAssetTemplateFamily.PreLocalCertificationDisclosure,
        Status: LegalAdminLabAssetTemplateStagingStatus.CounselReviewRequired,
        Summary: "Legal-admin template families are recognized as review candidates only and require regional counsel review before build-ready documentation.",
        WitnessRefs: new[]
        {
            "legal-admin-template-staging://counsel-review-required"
        },
        TimestampUtc: DateTimeOffset.UnixEpoch);

    private static LegalAdminLabAssetTemplateStagingRecord Create(
        LegalAdminLabAssetTemplateFamily family,
        string logicalSourceLabel,
        string resourceIdentity)
    {
        return new(
            Family: family,
            Status: LegalAdminLabAssetTemplateStagingStatus.CounselReviewRequired,
            ReviewPosture: LegalAdminLabAssetTemplateReviewPosture.ReviewRequired,
            LogicalSourceLabel: logicalSourceLabel,
            ResourceIdentity: resourceIdentity,
            LabCandidateRef: null,
            CounselQuestionSummary: CounselQuestionSummary,
            RefusalReason: RefusalReason,
            NonAuthoritySummary: NonAuthoritySummary,
            WitnessRefs: new[]
            {
                resourceIdentity,
                "legal-admin-template-staging://counsel-review-required"
            });
    }
}
