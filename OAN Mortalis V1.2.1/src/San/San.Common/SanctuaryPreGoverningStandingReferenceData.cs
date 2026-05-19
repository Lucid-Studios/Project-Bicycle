namespace San.Common;

public static class SanctuaryPreGoverningStandingReferenceData
{
    private static readonly string[] ResearchSourceLabels =
    {
        "Research Legal-Admin Spine",
        "Sanctuary Pre-Local Certification Disclosure Boundary",
        "Operator-CME Bond Legal Form Options",
        "Domain-Specific CME Legal Standing Boundary",
        "CME Data Rights And Research Protection Boundary",
        "Personification Research Special Case Boundary",
        "Topical Access And Coverage Routing Boundary",
        "Trusted Failure And Receipt Telemetry Boundary"
    };

    public static SanctuaryPreGoverningStandingRecord ReadyStanding { get; } = new(
        SourceApproachRef: "install-facing-approach-boundary://forward-horizon-anchors",
        Disposition: SanctuaryPreGoverningStandingDisposition.Ready,
        DisclosurePosture: SanctuaryPreGoverningDisclosurePosture.TemplateResourceOnly,
        DataRightsPosture: "continuity-bearing-personal-data-before-generic-telemetry",
        ResearchSeparationPosture: "install-is-not-research-consent",
        DomainPosture: SanctuaryPreGoverningDomainPosture.GeneralContinuityOnly,
        CmePosture: SanctuaryPreGoverningCmePosture.PlacementWithheld,
        SpecialCasePosture: "special-cases-held-before-widening",
        LogicalResearchSourceLabels: ResearchSourceLabels,
        NonGrantSummary: "Standing Sanctuary is ready as a coded pre-governing posture; governing CME remains placement-withheld and non-governing.",
        WitnessRefs: new[]
        {
            "install-facing-approach-boundary://forward-horizon-anchors",
            "sanctuary-pre-governing-standing://ready"
        });

    public static SanctuaryPreGoverningStandingRecord HeldForSpecialCaseOrDomainReview { get; } = new(
        SourceApproachRef: "install-facing-approach-boundary://forward-horizon-anchors",
        Disposition: SanctuaryPreGoverningStandingDisposition.Held,
        DisclosurePosture: SanctuaryPreGoverningDisclosurePosture.TemplateResourceOnly,
        DataRightsPosture: "continuity-bearing-personal-data-before-generic-telemetry",
        ResearchSeparationPosture: "lab-counsel-domain-review-required-before-widening",
        DomainPosture: SanctuaryPreGoverningDomainPosture.SpecialCaseHeld,
        CmePosture: SanctuaryPreGoverningCmePosture.PlacementWithheld,
        SpecialCasePosture: "held-for-lab-counsel-domain-review",
        LogicalResearchSourceLabels: ResearchSourceLabels,
        NonGrantSummary: "Special Case or domain-sensitive posture is held for Lab, counsel, and domain review before standing can widen.",
        WitnessRefs: new[]
        {
            "install-facing-approach-boundary://forward-horizon-anchors",
            "sanctuary-pre-governing-standing://held-special-case-or-domain-review"
        });

    public static SanctuaryPreGoverningStandingRecord RefusedOverclaim { get; } = new(
        SourceApproachRef: "install-facing-approach-boundary://forward-horizon-anchors",
        Disposition: SanctuaryPreGoverningStandingDisposition.Refused,
        DisclosurePosture: SanctuaryPreGoverningDisclosurePosture.TemplateResourceOnly,
        DataRightsPosture: "refused-data-rights-overclaim",
        ResearchSeparationPosture: "refused-research-consent-overclaim",
        DomainPosture: SanctuaryPreGoverningDomainPosture.DomainAdmissionRequired,
        CmePosture: SanctuaryPreGoverningCmePosture.RefusedOverclaim,
        SpecialCasePosture: "refused-special-case-widening",
        LogicalResearchSourceLabels: ResearchSourceLabels,
        NonGrantSummary: "Refuses claims of certification, domain authority, research consent, CME legal personhood, active governing CME, RTME, or runtime authority.",
        WitnessRefs: new[]
        {
            "install-facing-approach-boundary://forward-horizon-anchors",
            "sanctuary-pre-governing-standing://refused-overclaim"
        });

    public static SanctuaryPreGoverningStandingReceipt ReadyStandingReceipt { get; } = new(
        ReceiptHandle: "sanctuary-pre-governing-standing-receipt://ready",
        Disposition: SanctuaryPreGoverningStandingDisposition.Ready,
        Summary: ReadyStanding.NonGrantSummary,
        WitnessRefs: ReadyStanding.WitnessRefs,
        TimestampUtc: DateTimeOffset.UnixEpoch);

    public static SanctuaryPreGoverningStandingReceipt HeldForSpecialCaseOrDomainReviewReceipt { get; } = new(
        ReceiptHandle: "sanctuary-pre-governing-standing-receipt://held-special-case-or-domain-review",
        Disposition: SanctuaryPreGoverningStandingDisposition.Held,
        Summary: HeldForSpecialCaseOrDomainReview.NonGrantSummary,
        WitnessRefs: HeldForSpecialCaseOrDomainReview.WitnessRefs,
        TimestampUtc: DateTimeOffset.UnixEpoch);

    public static SanctuaryPreGoverningStandingReceipt RefusedOverclaimReceipt { get; } = new(
        ReceiptHandle: "sanctuary-pre-governing-standing-receipt://refused-overclaim",
        Disposition: SanctuaryPreGoverningStandingDisposition.Refused,
        Summary: RefusedOverclaim.NonGrantSummary,
        WitnessRefs: RefusedOverclaim.WitnessRefs,
        TimestampUtc: DateTimeOffset.UnixEpoch);
}
