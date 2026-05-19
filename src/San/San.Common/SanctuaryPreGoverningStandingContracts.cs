namespace San.Common;

public enum SanctuaryPreGoverningStandingDisposition
{
    Ready = 0,
    Held = 1,
    Refused = 2
}

public enum SanctuaryPreGoverningDisclosurePosture
{
    TemplateResourceOnly = 0,
    LabStaged = 1,
    RegionalCounselReviewed = 2
}

public enum SanctuaryPreGoverningDomainPosture
{
    GeneralContinuityOnly = 0,
    DomainAdmissionRequired = 1,
    SpecialCaseHeld = 2
}

public enum SanctuaryPreGoverningCmePosture
{
    PlacementWithheld = 0,
    NonGoverning = 1,
    RefusedOverclaim = 2
}

public sealed record SanctuaryPreGoverningStandingRecord(
    string SourceApproachRef,
    SanctuaryPreGoverningStandingDisposition Disposition,
    SanctuaryPreGoverningDisclosurePosture DisclosurePosture,
    string DataRightsPosture,
    string ResearchSeparationPosture,
    SanctuaryPreGoverningDomainPosture DomainPosture,
    SanctuaryPreGoverningCmePosture CmePosture,
    string SpecialCasePosture,
    IReadOnlyList<string> LogicalResearchSourceLabels,
    string NonGrantSummary,
    IReadOnlyList<string> WitnessRefs);

public sealed record SanctuaryPreGoverningStandingReceipt(
    string ReceiptHandle,
    SanctuaryPreGoverningStandingDisposition Disposition,
    string Summary,
    IReadOnlyList<string> WitnessRefs,
    DateTimeOffset TimestampUtc);
