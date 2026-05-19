namespace San.Common;

public enum LegalAdminLabAssetTemplateStagingStatus
{
    SourceResource = 0,
    LabStaged = 1,
    CounselReviewRequired = 2,
    BuildReadyWithCounselReview = 3,
    RefusedAsActiveTerms = 4
}

public enum LegalAdminLabAssetTemplateFamily
{
    PreLocalCertificationDisclosure = 0,
    OperatorCmeBondLegalForm = 1,
    DomainSpecificCmeStanding = 2,
    CmeDataRightsResearchProtection = 3,
    PersonificationSpecialCase = 4,
    TopicalAccessRouting = 5,
    TrustedFailureReceiptTelemetry = 6
}

public enum LegalAdminLabAssetTemplateReviewPosture
{
    NotReviewed = 0,
    ReviewRequired = 1,
    CounselReviewedForBuildDocumentation = 2,
    RefusedForActivation = 3
}

public sealed record LegalAdminLabAssetTemplateStagingRecord(
    LegalAdminLabAssetTemplateFamily Family,
    LegalAdminLabAssetTemplateStagingStatus Status,
    LegalAdminLabAssetTemplateReviewPosture ReviewPosture,
    string LogicalSourceLabel,
    string ResourceIdentity,
    string? LabCandidateRef,
    string CounselQuestionSummary,
    string RefusalReason,
    string NonAuthoritySummary,
    IReadOnlyList<string> WitnessRefs);

public sealed record LegalAdminLabAssetTemplateStagingReceipt(
    string ReceiptHandle,
    LegalAdminLabAssetTemplateFamily Family,
    LegalAdminLabAssetTemplateStagingStatus Status,
    string Summary,
    IReadOnlyList<string> WitnessRefs,
    DateTimeOffset TimestampUtc);
