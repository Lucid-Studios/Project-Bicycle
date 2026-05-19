namespace San.Common;

public static class SanctuaryGelFirstFormationAttemptReferenceData
{
    public static SanctuaryGelFirstFormationAttemptInput ReadyInput { get; } = new(
        PredicatePriors: new[] { GelPredicatePriorFormalizationReferenceData.ReadyPredicatePrior },
        LocalizedPreCertificationDataPool: LocalizedPreCertificationDataPoolReferenceData.ReadyPreCertificationPool,
        LocalizedFormation: LocalizedSanctuaryGelFormationReferenceData.ReadyLocalizedFormation,
        RegionalSubstrate: SanctuaryGelRegionalSubstrateFormationReferenceData.ReadyRegionalSubstrate,
        PreGoverningStanding: SanctuaryPreGoverningStandingReferenceData.ReadyStanding,
        FirstUseEligibility: FirstUseEligibilityReferenceData.ReadyForConsideration,
        SanctuaryActualClaimed: false,
        SurvivorAdmissionClaimed: false,
        FirstUseAdmissionClaimed: false,
        ModelSelectionClaimed: false,
        RuntimeAuthorityClaimed: false,
        CradleGelGenerationClaimed: false,
        SliLispOrRtmeActivationClaimed: false);

    public static SanctuaryGelFirstFormationAttemptRecord ReadyAttempt { get; } = new(
        AttemptHandle: "sanctuary-gel-first-formation-attempt-ref://ready",
        Disposition: SanctuaryGelFirstFormationAttemptDisposition.Ready,
        PredicatePriorRefs: new[] { GelPredicatePriorFormalizationReferenceData.ReadyPredicatePrior.PredicatePriorRef },
        LocalizedPreCertificationDataPoolRefs: new[] { "localized-pre-certification-data-pool-ref://ready" },
        LocalizedFormationRefs: new[] { LocalizedSanctuaryGelFormationReferenceData.ReadyLocalizedFormation.SourceGelFormationRef },
        StandingRefs: LocalizedSanctuaryGelFormationReferenceData.ReadyLocalizedFormation.StandingRepresentations.Select(static standing => standing.RepresentationRef).ToArray(),
        RegionalSubstrateRef: SanctuaryGelRegionalSubstrateFormationReferenceData.ReadyRegionalSubstrate.Identity.SubstrateHandle,
        PreGoverningStandingRef: SanctuaryPreGoverningStandingReferenceData.ReadyStanding.SourceApproachRef,
        FirstUseEligibilityRef: "first-use-eligibility://ready-for-consideration",
        RefusalReasons: new[] { SanctuaryGelFirstFormationAttemptRefusalReason.None },
        NonAuthoritySummary: "Ready first Sanctuary.GEL formation attempt coheres as a bounded receipted attempt only; it does not stand Sanctuary.Actual, admit survivor standing, grant first use, select models, activate runtime, invoke SLI.Lisp or RTME, or generate Cradle.GEL.",
        WitnessRefs: ReadyWitnessRefs());

    public static SanctuaryGelFirstFormationAttemptRecord HeldAttempt { get; } = ReadyAttempt with
    {
        AttemptHandle = "sanctuary-gel-first-formation-attempt-ref://held",
        Disposition = SanctuaryGelFirstFormationAttemptDisposition.Held,
        RegionalSubstrateRef = SanctuaryGelRegionalSubstrateFormationReferenceData.HeldForRegionalOrGovernanceReview.Identity.SubstrateHandle,
        PreGoverningStandingRef = SanctuaryPreGoverningStandingReferenceData.HeldForSpecialCaseOrDomainReview.SourceApproachRef,
        FirstUseEligibilityRef = "first-use-eligibility://held-for-review",
        NonAuthoritySummary = "Held first Sanctuary.GEL formation attempt preserves represented posture while local, domain, Special Case, counsel, regional, or governance questions remain held.",
        WitnessRefs = new[]
        {
            "sanctuary-gel-first-formation-attempt-ref://held",
            SanctuaryGelRegionalSubstrateFormationReferenceData.HeldForRegionalOrGovernanceReview.Identity.SubstrateHandle,
            "first-use-eligibility://held-for-review"
        }
    };

    public static SanctuaryGelFirstFormationAttemptRecord RefusedMissingPrerequisites { get; } = ReadyAttempt with
    {
        AttemptHandle = "sanctuary-gel-first-formation-attempt-ref://refused-missing-prerequisites",
        Disposition = SanctuaryGelFirstFormationAttemptDisposition.Refused,
        PredicatePriorRefs = Array.Empty<string>(),
        LocalizedPreCertificationDataPoolRefs = Array.Empty<string>(),
        RegionalSubstrateRef = "missing",
        RefusalReasons = new[]
        {
            SanctuaryGelFirstFormationAttemptRefusalReason.MissingPredicatePriors,
            SanctuaryGelFirstFormationAttemptRefusalReason.MissingLocalizedPreCertificationDataPool,
            SanctuaryGelFirstFormationAttemptRefusalReason.MissingRegionalSubstrateFooting
        },
        NonAuthoritySummary = "Refuses first Sanctuary.GEL formation attempt because required predicate priors, localized pre-certification data pool posture, or regional substrate footing are missing. No authority is granted.",
        WitnessRefs = new[]
        {
            "sanctuary-gel-first-formation-attempt-ref://refused-missing-prerequisites"
        }
    };

    public static SanctuaryGelFirstFormationAttemptRecord RefusedAuthorityOverclaim { get; } = ReadyAttempt with
    {
        AttemptHandle = "sanctuary-gel-first-formation-attempt-ref://refused-authority-overclaim",
        Disposition = SanctuaryGelFirstFormationAttemptDisposition.Refused,
        RefusalReasons = new[]
        {
            SanctuaryGelFirstFormationAttemptRefusalReason.SanctuaryActualOverclaimed,
            SanctuaryGelFirstFormationAttemptRefusalReason.SurvivorAdmissionOverclaimed,
            SanctuaryGelFirstFormationAttemptRefusalReason.FirstUseAdmissionOverclaimed,
            SanctuaryGelFirstFormationAttemptRefusalReason.ModelSelectionOverclaimed,
            SanctuaryGelFirstFormationAttemptRefusalReason.RuntimeAuthorityOverclaimed,
            SanctuaryGelFirstFormationAttemptRefusalReason.CradleGelGenerationOverclaimed,
            SanctuaryGelFirstFormationAttemptRefusalReason.SliLispOrRtmeActivationOverclaimed
        },
        NonAuthoritySummary = "Refuses first Sanctuary.GEL formation attempt because Sanctuary.Actual, survivor admission, first-use admission, model selection, runtime, Cradle.GEL generation, SLI.Lisp, or RTME activation was overclaimed.",
        WitnessRefs = new[]
        {
            "sanctuary-gel-first-formation-attempt-ref://refused-authority-overclaim"
        }
    };

    public static IReadOnlyList<SanctuaryGelFirstFormationAttemptRecord> CanonicalRecords { get; } = new[]
    {
        ReadyAttempt,
        HeldAttempt,
        RefusedMissingPrerequisites,
        RefusedAuthorityOverclaim
    };

    public static SanctuaryGelFirstFormationAttemptReceipt ReadyReceipt { get; } = Receipt(
        "sanctuary-gel-first-formation-attempt-receipt-ref://ready",
        ReadyAttempt);

    public static SanctuaryGelFirstFormationAttemptReceipt HeldReceipt { get; } = Receipt(
        "sanctuary-gel-first-formation-attempt-receipt-ref://held",
        HeldAttempt);

    public static SanctuaryGelFirstFormationAttemptReceipt RefusedReceipt { get; } = Receipt(
        "sanctuary-gel-first-formation-attempt-receipt-ref://refused-authority-overclaim",
        RefusedAuthorityOverclaim);

    private static IReadOnlyList<string> ReadyWitnessRefs()
    {
        return new[]
        {
            "sanctuary-gel-first-formation-attempt-ref://ready",
            GelPredicatePriorFormalizationReferenceData.ReadyPredicatePrior.PredicatePriorRef,
            SanctuaryGelRegionalSubstrateFormationReferenceData.ReadyRegionalSubstrate.Identity.SubstrateHandle,
            "first-use-eligibility://ready-for-consideration"
        };
    }

    private static SanctuaryGelFirstFormationAttemptReceipt Receipt(
        string receiptHandle,
        SanctuaryGelFirstFormationAttemptRecord record)
    {
        return new(
            ReceiptHandle: receiptHandle,
            Disposition: record.Disposition,
            Summary: record.NonAuthoritySummary,
            WitnessRefs: record.WitnessRefs,
            TimestampUtc: DateTimeOffset.UnixEpoch);
    }
}
