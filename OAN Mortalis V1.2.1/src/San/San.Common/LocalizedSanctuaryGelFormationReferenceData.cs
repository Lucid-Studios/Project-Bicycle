namespace San.Common;

public static class LocalizedSanctuaryGelFormationReferenceData
{
    public static LocalizedStandingRepresentation NationalStanding { get; } = new(
        Layer: LocalizedStandingRepresentationLayer.National,
        RepresentationRef: "localized-standing://national",
        Summary: "National standing represented as broad civic and legal frame.",
        WitnessRefs: new[]
        {
            "localized-standing://national"
        });

    public static LocalizedStandingRepresentation RegionalStanding { get; } = new(
        Layer: LocalizedStandingRepresentationLayer.Regional,
        RepresentationRef: "localized-standing://regional",
        Summary: "Regional standing represented as localized state, province, territory, institutional, cultural, regulatory, and counsel-relevant frame.",
        WitnessRefs: new[]
        {
            "localized-standing://regional"
        });

    public static LocalizedStandingRepresentation LocalStanding { get; } = new(
        Layer: LocalizedStandingRepresentationLayer.Local,
        RepresentationRef: "localized-standing://local",
        Summary: "Local standing represented as site, operator context, data posture, disclosure posture, and permitted environment.",
        WitnessRefs: new[]
        {
            "localized-standing://local"
        });

    public static IReadOnlyList<LocalizedStandingRepresentation> AllStandingRepresentations { get; } = new[]
    {
        NationalStanding,
        RegionalStanding,
        LocalStanding
    };

    public static LocalizedSanctuaryGelFormationRecord ReadyLocalizedFormation { get; } = new(
        Disposition: LocalizedSanctuaryGelFormationDisposition.Ready,
        StandingRepresentations: AllStandingRepresentations,
        SourceGelFormationRef: "sanctuary-gel-formation://first-localized",
        PredicateFootingRef: "sanctuary-gel-predicate-pool://first-family-bearing-pool",
        DataRightsPosture: "continuity-bearing-personal-data-before-generic-telemetry",
        LegalAdminStagingPosture: "legal-admin-template-families-review-candidates-only",
        ContinuityDataPosture: "bounded-local-continuity-substrate",
        RefusalReason: LocalizedSanctuaryGelFormationRefusalReason.None,
        NonAuthoritySummary: "Ready localized Sanctuary.GEL formation remains pre-governing and grants no first use, governance, RTME, counsel-reviewed disclosure, consent, domain authority, or runtime authority.",
        WitnessRefs: new[]
        {
            "localized-standing://national",
            "localized-standing://regional",
            "localized-standing://local",
            "localized-sanctuary-gel-formation://ready"
        });

    public static LocalizedSanctuaryGelFormationRecord HeldForContextReview { get; } = new(
        Disposition: LocalizedSanctuaryGelFormationDisposition.Held,
        StandingRepresentations: AllStandingRepresentations,
        SourceGelFormationRef: "sanctuary-gel-formation://first-localized",
        PredicateFootingRef: "sanctuary-gel-predicate-pool://first-family-bearing-pool",
        DataRightsPosture: "continuity-bearing-personal-data-before-generic-telemetry",
        LegalAdminStagingPosture: "legal-admin-template-families-review-candidates-only",
        ContinuityDataPosture: "held-local-domain-or-special-case-context",
        RefusalReason: LocalizedSanctuaryGelFormationRefusalReason.None,
        NonAuthoritySummary: "Localized formation is held for local, domain, or Special Case review before first-use eligibility may be considered.",
        WitnessRefs: new[]
        {
            "localized-standing://national",
            "localized-standing://regional",
            "localized-standing://local",
            "localized-sanctuary-gel-formation://held-context-review"
        });

    public static LocalizedSanctuaryGelFormationRecord RefusedMissingNationalStanding { get; } = Refused(
        refusalReason: LocalizedSanctuaryGelFormationRefusalReason.MissingNationalStanding,
        standingRepresentations: new[] { RegionalStanding, LocalStanding },
        witnessRef: "localized-sanctuary-gel-formation://refused-missing-national",
        summary: "Refuses localized Sanctuary.GEL formation because National standing is not represented.");

    public static LocalizedSanctuaryGelFormationRecord RefusedMissingRegionalStanding { get; } = Refused(
        refusalReason: LocalizedSanctuaryGelFormationRefusalReason.MissingRegionalStanding,
        standingRepresentations: new[] { NationalStanding, LocalStanding },
        witnessRef: "localized-sanctuary-gel-formation://refused-missing-regional",
        summary: "Refuses localized Sanctuary.GEL formation because Regional standing is not represented.");

    public static LocalizedSanctuaryGelFormationRecord RefusedMissingLocalStanding { get; } = Refused(
        refusalReason: LocalizedSanctuaryGelFormationRefusalReason.MissingLocalStanding,
        standingRepresentations: new[] { NationalStanding, RegionalStanding },
        witnessRef: "localized-sanctuary-gel-formation://refused-missing-local",
        summary: "Refuses localized Sanctuary.GEL formation because Local standing is not represented.");

    public static LocalizedSanctuaryGelFormationRecord RefusedGovernanceOverclaim { get; } = Refused(
        refusalReason: LocalizedSanctuaryGelFormationRefusalReason.OverclaimsGovernance,
        standingRepresentations: AllStandingRepresentations,
        witnessRef: "localized-sanctuary-gel-formation://refused-governance-overclaim",
        summary: "Refuses localized Sanctuary.GEL formation because formation overclaims governance, CME personhood, first use, RTME, domain authority, or runtime authority.");

    public static IReadOnlyList<LocalizedSanctuaryGelFormationRecord> CanonicalRecords { get; } = new[]
    {
        ReadyLocalizedFormation,
        HeldForContextReview,
        RefusedMissingNationalStanding,
        RefusedMissingRegionalStanding,
        RefusedMissingLocalStanding,
        RefusedGovernanceOverclaim
    };

    public static LocalizedSanctuaryGelFormationReceipt ReadyReceipt { get; } = Receipt(
        "localized-sanctuary-gel-formation-receipt://ready",
        ReadyLocalizedFormation);

    public static LocalizedSanctuaryGelFormationReceipt HeldReceipt { get; } = Receipt(
        "localized-sanctuary-gel-formation-receipt://held-context-review",
        HeldForContextReview);

    public static LocalizedSanctuaryGelFormationReceipt RefusedGovernanceOverclaimReceipt { get; } = Receipt(
        "localized-sanctuary-gel-formation-receipt://refused-governance-overclaim",
        RefusedGovernanceOverclaim);

    private static LocalizedSanctuaryGelFormationRecord Refused(
        LocalizedSanctuaryGelFormationRefusalReason refusalReason,
        IReadOnlyList<LocalizedStandingRepresentation> standingRepresentations,
        string witnessRef,
        string summary)
    {
        return new(
            Disposition: LocalizedSanctuaryGelFormationDisposition.Refused,
            StandingRepresentations: standingRepresentations,
            SourceGelFormationRef: "sanctuary-gel-formation://first-localized",
            PredicateFootingRef: "sanctuary-gel-predicate-pool://first-family-bearing-pool",
            DataRightsPosture: "withheld-until-localized-standing-represented",
            LegalAdminStagingPosture: "withheld-as-active-terms",
            ContinuityDataPosture: "withheld-localized-continuity-substrate",
            RefusalReason: refusalReason,
            NonAuthoritySummary: $"{summary} No first use, governance, RTME, counsel-reviewed disclosure, consent, domain authority, or runtime authority is granted.",
            WitnessRefs: new[]
            {
                witnessRef
            });
    }

    private static LocalizedSanctuaryGelFormationReceipt Receipt(
        string receiptHandle,
        LocalizedSanctuaryGelFormationRecord record)
    {
        return new(
            ReceiptHandle: receiptHandle,
            Disposition: record.Disposition,
            Summary: record.NonAuthoritySummary,
            WitnessRefs: record.WitnessRefs,
            TimestampUtc: DateTimeOffset.UnixEpoch);
    }
}
