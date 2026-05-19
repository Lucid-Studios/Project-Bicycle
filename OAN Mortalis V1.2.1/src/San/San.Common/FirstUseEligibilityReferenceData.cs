namespace San.Common;

public static class FirstUseEligibilityReferenceData
{
    public static FirstUseEligibilityRecord ReadyForConsideration { get; } = new(
        Disposition: FirstUseEligibilityDisposition.ReadyForConsideration,
        Postures: ReadyPostures(),
        SourceLocalizedFormationRef: "localized-sanctuary-gel-formation://ready",
        SourcePreGoverningStandingRef: "sanctuary-pre-governing-standing://ready",
        RefusalReasons: new[] { FirstUseEligibilityRefusalReason.None },
        NonPermissionSummary: "First use may be considered because localized formation, predicate-surface readiness, pre-governing standing, disclosure, data, retention, opt-out, research separation, Special Case hold, domain hold, counsel-review state, and non-authority posture are represented or lawfully held; this is not first-use permission.",
        WitnessRefs: new[]
        {
            "localized-sanctuary-gel-formation://ready",
            "sanctuary-pre-governing-standing://ready",
            "sanctuary-gel-predicate-pool://first-family-bearing-pool",
            "first-use-eligibility://ready-for-consideration"
        });

    public static FirstUseEligibilityRecord HeldForReview { get; } = new(
        Disposition: FirstUseEligibilityDisposition.Held,
        Postures: HeldPostures(),
        SourceLocalizedFormationRef: "localized-sanctuary-gel-formation://held-context-review",
        SourcePreGoverningStandingRef: "sanctuary-pre-governing-standing://held-special-case-or-domain-review",
        RefusalReasons: new[] { FirstUseEligibilityRefusalReason.None },
        NonPermissionSummary: "First-use eligibility is held because local, domain, Special Case, or counsel questions remain held before first-use admission may be considered.",
        WitnessRefs: new[]
        {
            "localized-sanctuary-gel-formation://held-context-review",
            "sanctuary-pre-governing-standing://held-special-case-or-domain-review",
            "first-use-eligibility://held-for-review"
        });

    public static FirstUseEligibilityRecord RefusedMissingPredicateSurfaceReadiness { get; } = Refused(
        "first-use-eligibility://refused-missing-predicate-surface-readiness",
        new[] { FirstUseEligibilityRefusalReason.MissingPredicateSurfaceReadiness },
        Replace(
            ReadyPostures(),
            Missing(FirstUseEligibilityPostureKind.PredicateSurfaceReadiness, "Predicate-surface readiness is missing; first-use eligibility cannot be considered.")),
        "Refuses first-use eligibility consideration because predicate-surface readiness is missing.");

    public static FirstUseEligibilityRecord RefusedMissingDisclosureOrDataPosture { get; } = Refused(
        "first-use-eligibility://refused-missing-disclosure-or-data-posture",
        new[]
        {
            FirstUseEligibilityRefusalReason.MissingDisclosurePosture,
            FirstUseEligibilityRefusalReason.MissingLocalDataPosture
        },
        Replace(
            ReadyPostures(),
            Missing(FirstUseEligibilityPostureKind.Disclosure, "Disclosure posture is missing."),
            Missing(FirstUseEligibilityPostureKind.LocalData, "Local data posture is missing.")),
        "Refuses first-use eligibility consideration because disclosure or local data posture is missing.");

    public static FirstUseEligibilityRecord RefusedMissingRetentionOrOptOut { get; } = Refused(
        "first-use-eligibility://refused-missing-retention-or-opt-out",
        new[] { FirstUseEligibilityRefusalReason.MissingRetentionOrOptOutPosture },
        Replace(
            ReadyPostures(),
            Missing(FirstUseEligibilityPostureKind.Retention, "Retention posture is missing."),
            Missing(FirstUseEligibilityPostureKind.OptOut, "Opt-out posture is missing.")),
        "Refuses first-use eligibility consideration because retention or opt-out posture is missing.");

    public static FirstUseEligibilityRecord RefusedSpecialCaseOrDomainNotHeld { get; } = Refused(
        "first-use-eligibility://refused-special-case-or-domain-not-held",
        new[]
        {
            FirstUseEligibilityRefusalReason.SpecialCaseNotHeld,
            FirstUseEligibilityRefusalReason.DomainUseNotHeld
        },
        Replace(
            ReadyPostures(),
            RefusedPosture(FirstUseEligibilityPostureKind.SpecialCaseHold, "Special Cases are not lawfully held."),
            RefusedPosture(FirstUseEligibilityPostureKind.DomainHold, "Domain-sensitive uses are not lawfully held.")),
        "Refuses first-use eligibility consideration because Special Cases or domain-sensitive uses are not held.");

    public static FirstUseEligibilityRecord RefusedResearchSeparationMissing { get; } = Refused(
        "first-use-eligibility://refused-research-separation-missing",
        new[] { FirstUseEligibilityRefusalReason.ResearchSeparationMissing },
        Replace(
            ReadyPostures(),
            Missing(FirstUseEligibilityPostureKind.ResearchSeparation, "Research separation is missing.")),
        "Refuses first-use eligibility consideration because research separation is missing.");

    public static FirstUseEligibilityRecord RefusedCounselRuntimeOrGovernanceOverclaim { get; } = Refused(
        "first-use-eligibility://refused-counsel-runtime-or-governance-overclaim",
        new[]
        {
            FirstUseEligibilityRefusalReason.CounselReviewOverclaimed,
            FirstUseEligibilityRefusalReason.RuntimeOrGovernanceOverclaimed
        },
        Replace(
            ReadyPostures(),
            RefusedPosture(FirstUseEligibilityPostureKind.CounselReview, "Counsel-review state is overclaimed."),
            RefusedPosture(FirstUseEligibilityPostureKind.NonAuthority, "Runtime, governance, RTME, domain authority, or first-use permission is overclaimed.")),
        "Refuses first-use eligibility consideration because counsel review, runtime, governance, RTME, domain authority, or first-use permission is overclaimed.");

    public static IReadOnlyList<FirstUseEligibilityRecord> CanonicalRecords { get; } = new[]
    {
        ReadyForConsideration,
        HeldForReview,
        RefusedMissingPredicateSurfaceReadiness,
        RefusedMissingDisclosureOrDataPosture,
        RefusedMissingRetentionOrOptOut,
        RefusedSpecialCaseOrDomainNotHeld,
        RefusedResearchSeparationMissing,
        RefusedCounselRuntimeOrGovernanceOverclaim
    };

    public static FirstUseEligibilityReceipt ReadyReceipt { get; } = Receipt(
        "first-use-eligibility-receipt://ready-for-consideration",
        ReadyForConsideration);

    public static FirstUseEligibilityReceipt HeldReceipt { get; } = Receipt(
        "first-use-eligibility-receipt://held-for-review",
        HeldForReview);

    public static FirstUseEligibilityReceipt RefusedReceipt { get; } = Receipt(
        "first-use-eligibility-receipt://refused-counsel-runtime-or-governance-overclaim",
        RefusedCounselRuntimeOrGovernanceOverclaim);

    private static IReadOnlyList<FirstUseEligibilityPosture> ReadyPostures()
    {
        return new[]
        {
            Represented(FirstUseEligibilityPostureKind.LocalizedGelFormation, "Localized Sanctuary.GEL formation is represented."),
            Represented(FirstUseEligibilityPostureKind.PredicateSurfaceReadiness, "Ready predicate pool, four predicate families, and family-bearing GEL inheritance are represented without SPC, validator exposure, predicate promotion, Atlas mutation, or runtime reasoning."),
            Represented(FirstUseEligibilityPostureKind.PreGoverningStanding, "Sanctuary pre-governing standing is represented."),
            Represented(FirstUseEligibilityPostureKind.Disclosure, "Disclosure posture is represented without activating disclosure surfaces."),
            Represented(FirstUseEligibilityPostureKind.LocalData, "Local data posture is represented."),
            Represented(FirstUseEligibilityPostureKind.Retention, "Retention posture is represented."),
            Represented(FirstUseEligibilityPostureKind.OptOut, "Opt-out posture is represented."),
            Represented(FirstUseEligibilityPostureKind.ResearchSeparation, "Research separation is explicit."),
            Held(FirstUseEligibilityPostureKind.SpecialCaseHold, "Special Cases remain held."),
            Held(FirstUseEligibilityPostureKind.DomainHold, "Domain-sensitive uses remain held."),
            Represented(FirstUseEligibilityPostureKind.CounselReview, "Counsel-review state is represented without claiming counsel approval."),
            Represented(FirstUseEligibilityPostureKind.NonAuthority, "No first use, runtime, governance, RTME, domain authority, counsel approval, or active legal terms are granted.")
        };
    }

    private static IReadOnlyList<FirstUseEligibilityPosture> HeldPostures()
    {
        return Replace(
            ReadyPostures(),
            Held(FirstUseEligibilityPostureKind.LocalData, "Local data posture remains held for review."),
            Held(FirstUseEligibilityPostureKind.SpecialCaseHold, "Special Case posture remains held for review."),
            Held(FirstUseEligibilityPostureKind.DomainHold, "Domain-sensitive use posture remains held for review."),
            Held(FirstUseEligibilityPostureKind.CounselReview, "Counsel-review questions remain held without claiming approval."));
    }

    private static FirstUseEligibilityRecord Refused(
        string witnessRef,
        IReadOnlyList<FirstUseEligibilityRefusalReason> refusalReasons,
        IReadOnlyList<FirstUseEligibilityPosture> postures,
        string summary)
    {
        return new(
            Disposition: FirstUseEligibilityDisposition.Refused,
            Postures: postures,
            SourceLocalizedFormationRef: "localized-sanctuary-gel-formation://ready",
            SourcePreGoverningStandingRef: "sanctuary-pre-governing-standing://ready",
            RefusalReasons: refusalReasons,
            NonPermissionSummary: $"{summary} No first-use permission, runtime, governing CME, RTME, counsel approval, domain authority, or consent activation is granted.",
            WitnessRefs: new[]
            {
                witnessRef
            });
    }

    private static FirstUseEligibilityPosture Represented(
        FirstUseEligibilityPostureKind kind,
        string summary)
    {
        return new(
            Kind: kind,
            State: FirstUseEligibilityPostureState.Represented,
            Summary: summary,
            WitnessRefs: new[] { $"first-use-eligibility-posture://{kind.ToString().ToLowerInvariant()}" });
    }

    private static FirstUseEligibilityPosture Held(
        FirstUseEligibilityPostureKind kind,
        string summary)
    {
        return new(
            Kind: kind,
            State: FirstUseEligibilityPostureState.Held,
            Summary: summary,
            WitnessRefs: new[] { $"first-use-eligibility-posture://{kind.ToString().ToLowerInvariant()}" });
    }

    private static FirstUseEligibilityPosture Missing(
        FirstUseEligibilityPostureKind kind,
        string summary)
    {
        return new(
            Kind: kind,
            State: FirstUseEligibilityPostureState.Missing,
            Summary: summary,
            WitnessRefs: new[] { $"first-use-eligibility-posture://{kind.ToString().ToLowerInvariant()}/missing" });
    }

    private static FirstUseEligibilityPosture RefusedPosture(
        FirstUseEligibilityPostureKind kind,
        string summary)
    {
        return new(
            Kind: kind,
            State: FirstUseEligibilityPostureState.Refused,
            Summary: summary,
            WitnessRefs: new[] { $"first-use-eligibility-posture://{kind.ToString().ToLowerInvariant()}/refused" });
    }

    private static IReadOnlyList<FirstUseEligibilityPosture> Replace(
        IReadOnlyList<FirstUseEligibilityPosture> postures,
        params FirstUseEligibilityPosture[] replacements)
    {
        var replacementsByKind = replacements.ToDictionary(static posture => posture.Kind);
        return postures
            .Select(posture => replacementsByKind.TryGetValue(posture.Kind, out var replacement)
                ? replacement
                : posture)
            .ToArray();
    }

    private static FirstUseEligibilityReceipt Receipt(
        string receiptHandle,
        FirstUseEligibilityRecord record)
    {
        return new(
            ReceiptHandle: receiptHandle,
            Disposition: record.Disposition,
            Summary: record.NonPermissionSummary,
            WitnessRefs: record.WitnessRefs,
            TimestampUtc: DateTimeOffset.UnixEpoch);
    }
}
