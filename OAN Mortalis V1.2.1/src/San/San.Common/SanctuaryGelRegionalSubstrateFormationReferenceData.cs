namespace San.Common;

public static class SanctuaryGelRegionalSubstrateFormationReferenceData
{
    private static readonly string[] ReadyPredicatePriorRefs =
    {
        "gel-predicate-prior-ref://rt-accept-107"
    };

    private static readonly string[] ReadyPredicatePoolRefs =
    {
        "sanctuary-gel-predicate-pool-ref://first-family-bearing-pool"
    };

    private static readonly string[] ReadyPredicateFamilyRefs =
    {
        "sanctuary-gel-predicate-family-ref://posture",
        "sanctuary-gel-predicate-family-ref://trust-authorization",
        "sanctuary-gel-predicate-family-ref://evidence-footing",
        "sanctuary-gel-predicate-family-ref://response-disposition"
    };

    private static readonly string[] ReadyPreCertificationPoolRefs =
    {
        "localized-pre-certification-data-pool-ref://ready"
    };

    private static readonly string[] ReadyStandingRefs =
    {
        "localized-standing-ref://national",
        "localized-standing-ref://regional",
        "localized-standing-ref://local"
    };

    public static SanctuaryGelRegionalSubstrateFormationRecord ReadyRegionalSubstrate { get; } = new(
        Disposition: SanctuaryGelRegionalSubstrateFormationDisposition.Ready,
        Identity: ReadyIdentity(),
        PredicatePriorRefs: ReadyPredicatePriorRefs,
        PredicatePoolRefs: ReadyPredicatePoolRefs,
        PredicateFamilyRefs: ReadyPredicateFamilyRefs,
        LocalizedPreCertificationDataPoolRefs: ReadyPreCertificationPoolRefs,
        StandingRefs: ReadyStandingRefs,
        DataRightsPosture: "continuity-bearing-personal-data-before-generic-telemetry",
        ResearchSeparationPosture: "install-is-not-research-consent",
        SpecialCaseHoldPosture: "special-case-held-before-widening",
        DomainHoldPosture: "domain-sensitive-use-held-before-separate-admission",
        AdmissionCeiling: SanctuaryGelRegionalSubstrateAdmissionCeiling.RegionalSubstrateOnly,
        RefusalReasons: new[] { SanctuaryGelRegionalSubstrateRefusalReason.None },
        NonAuthoritySummary: "Ready Sanctuary.GEL regional substrate forms the bounded regional GEL body required before Sanctuary.Actual governing CME or later Cradle.GEL generation may be considered; it does not stand Sanctuary.Actual, select models, authorize governance, activate runtime, or generate Cradle.GEL.",
        WitnessRefs: new[]
        {
            "sanctuary-gel-regional-substrate-ref://ready",
            "gel-predicate-prior-ref://rt-accept-107",
            "localized-pre-certification-data-pool-ref://ready",
            "localized-standing-ref://national",
            "localized-standing-ref://regional",
            "localized-standing-ref://local"
        });

    public static SanctuaryGelRegionalSubstrateFormationRecord HeldForRegionalOrGovernanceReview { get; } = ReadyRegionalSubstrate with
    {
        Disposition = SanctuaryGelRegionalSubstrateFormationDisposition.Held,
        Identity = ReadyIdentity() with
        {
            SubstrateHandle = "sanctuary-gel-regional-substrate-ref://held-regional-or-governance-review",
            Summary = "Held Sanctuary.GEL regional substrate remains under regional, domain, Special Case, or governance review."
        },
        AdmissionCeiling = SanctuaryGelRegionalSubstrateAdmissionCeiling.CandidateOnly,
        NonAuthoritySummary = "Held Sanctuary.GEL regional substrate remains candidate-only while regional, domain, Special Case, or governance questions remain unresolved.",
        WitnessRefs = new[]
        {
            "sanctuary-gel-regional-substrate-ref://held-regional-or-governance-review"
        }
    };

    public static SanctuaryGelRegionalSubstrateFormationRecord RefusedMissingPredicatePriorOrPreCertificationPool { get; } = Refused(
        "sanctuary-gel-regional-substrate-ref://refused-missing-predicate-prior-or-pre-certification-pool",
        new[]
        {
            SanctuaryGelRegionalSubstrateRefusalReason.MissingPredicatePriorRefs,
            SanctuaryGelRegionalSubstrateRefusalReason.MissingLocalizedPreCertificationDataPool
        },
        predicatePriorRefs: Array.Empty<string>(),
        preCertificationPoolRefs: Array.Empty<string>(),
        standingRefs: ReadyStandingRefs,
        regionalPackageFootingRef: "regional-package-footing-ref://english-us",
        "Refuses Sanctuary.GEL regional substrate formation because predicate-prior refs or localized pre-certification data pool refs are missing.");

    public static SanctuaryGelRegionalSubstrateFormationRecord RefusedMissingStandingOrRegionalPackageFooting { get; } = Refused(
        "sanctuary-gel-regional-substrate-ref://refused-missing-standing-or-regional-package-footing",
        new[]
        {
            SanctuaryGelRegionalSubstrateRefusalReason.MissingNationalStanding,
            SanctuaryGelRegionalSubstrateRefusalReason.MissingRegionalStanding,
            SanctuaryGelRegionalSubstrateRefusalReason.MissingLocalStanding,
            SanctuaryGelRegionalSubstrateRefusalReason.MissingRegionalPackageFooting
        },
        predicatePriorRefs: ReadyPredicatePriorRefs,
        preCertificationPoolRefs: ReadyPreCertificationPoolRefs,
        standingRefs: Array.Empty<string>(),
        regionalPackageFootingRef: "missing",
        "Refuses Sanctuary.GEL regional substrate formation because National, Regional, Local, or regional package footing is missing.");

    public static SanctuaryGelRegionalSubstrateFormationRecord RefusedAdmissionCeilingWidened { get; } = Refused(
        "sanctuary-gel-regional-substrate-ref://refused-admission-ceiling-widened",
        new[]
        {
            SanctuaryGelRegionalSubstrateRefusalReason.AdmissionCeilingWidened
        },
        predicatePriorRefs: ReadyPredicatePriorRefs,
        preCertificationPoolRefs: ReadyPreCertificationPoolRefs,
        standingRefs: ReadyStandingRefs,
        regionalPackageFootingRef: "regional-package-footing-ref://english-us",
        "Refuses Sanctuary.GEL regional substrate formation because the admission ceiling was widened beyond regional substrate posture.");

    public static SanctuaryGelRegionalSubstrateFormationRecord RefusedSanctuaryActualModelCradleGovernanceOrRuntimeOverclaim { get; } = Refused(
        "sanctuary-gel-regional-substrate-ref://refused-sanctuary-actual-model-cradle-governance-or-runtime-overclaim",
        new[]
        {
            SanctuaryGelRegionalSubstrateRefusalReason.SanctuaryActualOverclaimed,
            SanctuaryGelRegionalSubstrateRefusalReason.MotherFatherGoverningCmeOverclaimed,
            SanctuaryGelRegionalSubstrateRefusalReason.ModelSelectionOverclaimed,
            SanctuaryGelRegionalSubstrateRefusalReason.CradleGelGenerationOverclaimed,
            SanctuaryGelRegionalSubstrateRefusalReason.GovernanceOrRuntimeOverclaimed
        },
        predicatePriorRefs: ReadyPredicatePriorRefs,
        preCertificationPoolRefs: ReadyPreCertificationPoolRefs,
        standingRefs: ReadyStandingRefs,
        regionalPackageFootingRef: "regional-package-footing-ref://english-us",
        "Refuses Sanctuary.GEL regional substrate formation because Sanctuary.Actual, Mother/Father governing CME, model selection, Cradle.GEL generation, governance, or runtime authority was overclaimed.");

    public static IReadOnlyList<SanctuaryGelRegionalSubstrateFormationRecord> CanonicalRecords { get; } = new[]
    {
        ReadyRegionalSubstrate,
        HeldForRegionalOrGovernanceReview,
        RefusedMissingPredicatePriorOrPreCertificationPool,
        RefusedMissingStandingOrRegionalPackageFooting,
        RefusedAdmissionCeilingWidened,
        RefusedSanctuaryActualModelCradleGovernanceOrRuntimeOverclaim
    };

    public static SanctuaryGelRegionalSubstrateFormationReceipt ReadyReceipt { get; } = Receipt(
        "sanctuary-gel-regional-substrate-receipt-ref://ready",
        ReadyRegionalSubstrate);

    public static SanctuaryGelRegionalSubstrateFormationReceipt HeldReceipt { get; } = Receipt(
        "sanctuary-gel-regional-substrate-receipt-ref://held-regional-or-governance-review",
        HeldForRegionalOrGovernanceReview);

    public static SanctuaryGelRegionalSubstrateFormationReceipt RefusedReceipt { get; } = Receipt(
        "sanctuary-gel-regional-substrate-receipt-ref://refused-sanctuary-actual-model-cradle-governance-or-runtime-overclaim",
        RefusedSanctuaryActualModelCradleGovernanceOrRuntimeOverclaim);

    private static SanctuaryGelRegionalSubstrateIdentity ReadyIdentity()
    {
        return new(
            SubstrateHandle: "sanctuary-gel-regional-substrate-ref://ready",
            SanctuaryBodyRef: "sanctuary-body-ref://full-program-body",
            RegionRef: "regional-footing-ref://english-us",
            RegionalPackageFootingRef: "regional-package-footing-ref://english-us",
            Summary: "Sanctuary.GEL regional/root local GEL substrate for Sanctuary.",
            WitnessRefs: new[]
            {
                "sanctuary-gel-regional-substrate-ref://ready",
                "regional-package-footing-ref://english-us"
            });
    }

    private static SanctuaryGelRegionalSubstrateFormationRecord Refused(
        string substrateHandle,
        IReadOnlyList<SanctuaryGelRegionalSubstrateRefusalReason> refusalReasons,
        IReadOnlyList<string> predicatePriorRefs,
        IReadOnlyList<string> preCertificationPoolRefs,
        IReadOnlyList<string> standingRefs,
        string regionalPackageFootingRef,
        string summary)
    {
        return new(
            Disposition: SanctuaryGelRegionalSubstrateFormationDisposition.Refused,
            Identity: ReadyIdentity() with
            {
                SubstrateHandle = substrateHandle,
                RegionalPackageFootingRef = regionalPackageFootingRef,
                Summary = summary
            },
            PredicatePriorRefs: predicatePriorRefs,
            PredicatePoolRefs: ReadyPredicatePoolRefs,
            PredicateFamilyRefs: ReadyPredicateFamilyRefs,
            LocalizedPreCertificationDataPoolRefs: preCertificationPoolRefs,
            StandingRefs: standingRefs,
            DataRightsPosture: "withheld-until-regional-substrate-footing-represented",
            ResearchSeparationPosture: "withheld-until-regional-substrate-footing-represented",
            SpecialCaseHoldPosture: "withheld-until-regional-substrate-footing-represented",
            DomainHoldPosture: "withheld-until-regional-substrate-footing-represented",
            AdmissionCeiling: SanctuaryGelRegionalSubstrateAdmissionCeiling.CandidateOnly,
            RefusalReasons: refusalReasons,
            NonAuthoritySummary: $"{summary} No Sanctuary.Actual, Mother/Father governing CME, model selection, Cradle.GEL generation, governance, runtime, first use, RTME, or survivor admission is granted.",
            WitnessRefs: new[]
            {
                substrateHandle
            });
    }

    private static SanctuaryGelRegionalSubstrateFormationReceipt Receipt(
        string receiptHandle,
        SanctuaryGelRegionalSubstrateFormationRecord record)
    {
        return new(
            ReceiptHandle: receiptHandle,
            Disposition: record.Disposition,
            Summary: record.NonAuthoritySummary,
            WitnessRefs: record.WitnessRefs,
            TimestampUtc: DateTimeOffset.UnixEpoch);
    }
}
