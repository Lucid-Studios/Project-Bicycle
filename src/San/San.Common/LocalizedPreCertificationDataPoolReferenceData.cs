namespace San.Common;

public static class LocalizedPreCertificationDataPoolReferenceData
{
    private static readonly string[] LocalizedStandingRefs =
    {
        "localized-standing://national",
        "localized-standing://regional",
        "localized-standing://local"
    };

    private static readonly string[] LegalAdminStagingRefs =
    {
        "legal-admin-template-family://pre-local-certification-disclosure",
        "legal-admin-template-family://operator-cme-bond-legal-form",
        "legal-admin-template-family://domain-specific-cme-standing",
        "legal-admin-template-family://cme-data-rights-research-protection",
        "legal-admin-template-family://personification-special-case",
        "legal-admin-template-family://topical-access-routing",
        "legal-admin-template-family://trusted-failure-receipt-telemetry"
    };

    public static LocalizedPreCertificationDataPoolRecord ReadyPreCertificationPool { get; } = new(
        Disposition: LocalizedPreCertificationDataPoolDisposition.Ready,
        Inputs: ReadyInputs(),
        SourceLocalizedStandingRefs: LocalizedStandingRefs,
        SourceLegalAdminStagingRefs: LegalAdminStagingRefs,
        RefusalReasons: new[] { LocalizedPreCertificationDataPoolRefusalReason.None },
        NonAuthoritySummary: "Ready localized pre-certification data pool contextualizes Sanctuary.GEL only; it does not certify, disclose, consent, authorize, govern, activate runtime, admit first use, activate RTME, mutate Atlas authority, or import legal template bodies.",
        WitnessRefs: new[]
        {
            "localized-pre-certification-data-pool://ready",
            "localized-standing://national",
            "localized-standing://regional",
            "localized-standing://local"
        });

    public static LocalizedPreCertificationDataPoolRecord HeldForReview { get; } = new(
        Disposition: LocalizedPreCertificationDataPoolDisposition.Held,
        Inputs: HeldInputs(),
        SourceLocalizedStandingRefs: LocalizedStandingRefs,
        SourceLegalAdminStagingRefs: LegalAdminStagingRefs,
        RefusalReasons: new[] { LocalizedPreCertificationDataPoolRefusalReason.None },
        NonAuthoritySummary: "Localized pre-certification data pool is held because legal-admin, domain, Special Case, or counsel questions remain review candidates only.",
        WitnessRefs: new[]
        {
            "localized-pre-certification-data-pool://held-for-review",
            "localized-standing://national",
            "localized-standing://regional",
            "localized-standing://local"
        });

    public static LocalizedPreCertificationDataPoolRecord RefusedMissingStandingRepresentation { get; } = Refused(
        "localized-pre-certification-data-pool://refused-missing-standing-representation",
        new[]
        {
            LocalizedPreCertificationDataPoolRefusalReason.MissingNationalStanding,
            LocalizedPreCertificationDataPoolRefusalReason.MissingRegionalStanding,
            LocalizedPreCertificationDataPoolRefusalReason.MissingLocalStanding
        },
        ReadyInputs()
            .Where(static input =>
                input.Kind is not LocalizedPreCertificationDataPoolInputKind.NationalStanding and
                not LocalizedPreCertificationDataPoolInputKind.RegionalStanding and
                not LocalizedPreCertificationDataPoolInputKind.LocalStanding)
            .ToArray(),
        "Refuses localized pre-certification data pool because National, Regional, or Local standing representation is missing.");

    public static LocalizedPreCertificationDataPoolRecord RefusedActiveLegalTermsOrCertificationOverclaim { get; } = Refused(
        "localized-pre-certification-data-pool://refused-active-legal-terms-or-certification-overclaim",
        new[]
        {
            LocalizedPreCertificationDataPoolRefusalReason.ActiveLegalTermsOverclaimed,
            LocalizedPreCertificationDataPoolRefusalReason.CertificationOverclaimed
        },
        ReadyInputs(),
        "Refuses localized pre-certification data pool because active legal terms or certification were overclaimed.");

    public static LocalizedPreCertificationDataPoolRecord RefusedConsentOrDisclosureOverclaim { get; } = Refused(
        "localized-pre-certification-data-pool://refused-consent-or-disclosure-overclaim",
        new[]
        {
            LocalizedPreCertificationDataPoolRefusalReason.ConsentRecordOverclaimed,
            LocalizedPreCertificationDataPoolRefusalReason.DisclosureIssuanceOverclaimed
        },
        ReadyInputs(),
        "Refuses localized pre-certification data pool because consent records or disclosure issuance were overclaimed.");

    public static LocalizedPreCertificationDataPoolRecord RefusedDomainFirstUseRtmeGovernanceOrRuntimeOverclaim { get; } = Refused(
        "localized-pre-certification-data-pool://refused-domain-first-use-rtme-governance-or-runtime-overclaim",
        new[]
        {
            LocalizedPreCertificationDataPoolRefusalReason.DomainAuthorizationOverclaimed,
            LocalizedPreCertificationDataPoolRefusalReason.FirstUseAdmissionOverclaimed,
            LocalizedPreCertificationDataPoolRefusalReason.RtmeOverclaimed,
            LocalizedPreCertificationDataPoolRefusalReason.GoverningCmeOverclaimed,
            LocalizedPreCertificationDataPoolRefusalReason.RuntimeAuthorityOverclaimed
        },
        ReadyInputs(),
        "Refuses localized pre-certification data pool because domain authorization, first-use admission, RTME, governing CME, or runtime authority were overclaimed.");

    public static IReadOnlyList<LocalizedPreCertificationDataPoolRecord> CanonicalRecords { get; } = new[]
    {
        ReadyPreCertificationPool,
        HeldForReview,
        RefusedMissingStandingRepresentation,
        RefusedActiveLegalTermsOrCertificationOverclaim,
        RefusedConsentOrDisclosureOverclaim,
        RefusedDomainFirstUseRtmeGovernanceOrRuntimeOverclaim
    };

    public static LocalizedPreCertificationDataPoolReceipt ReadyReceipt { get; } = Receipt(
        "localized-pre-certification-data-pool-receipt://ready",
        ReadyPreCertificationPool);

    public static LocalizedPreCertificationDataPoolReceipt HeldReceipt { get; } = Receipt(
        "localized-pre-certification-data-pool-receipt://held-for-review",
        HeldForReview);

    public static LocalizedPreCertificationDataPoolReceipt RefusedReceipt { get; } = Receipt(
        "localized-pre-certification-data-pool-receipt://refused-domain-first-use-rtme-governance-or-runtime-overclaim",
        RefusedDomainFirstUseRtmeGovernanceOrRuntimeOverclaim);

    private static IReadOnlyList<LocalizedPreCertificationDataPoolInput> ReadyInputs()
    {
        return new[]
        {
            Input(
                LocalizedPreCertificationDataPoolInputKind.LabAssetCandidate,
                "lab-assets://legal-admin-template-candidates",
                "Lab asset candidates represented as review candidates only."),
            Input(
                LocalizedPreCertificationDataPoolInputKind.RootAtlasRegionalPosture,
                "rootatlas-regional-posture://english-us",
                "RootAtlas regional source posture represented as ancestry and orientation only, not local RootAtlas authority."),
            Input(
                LocalizedPreCertificationDataPoolInputKind.LegalAdminTemplateFamily,
                "legal-admin-template-family://first-seven",
                "Legal-admin template families represented as family refs only, not legal template bodies."),
            Input(
                LocalizedPreCertificationDataPoolInputKind.NationalStanding,
                "localized-standing://national",
                "National standing represented as broad civic and legal frame."),
            Input(
                LocalizedPreCertificationDataPoolInputKind.RegionalStanding,
                "localized-standing://regional",
                "Regional standing represented as regional, regulatory, cultural, and counsel-relevant frame."),
            Input(
                LocalizedPreCertificationDataPoolInputKind.LocalStanding,
                "localized-standing://local",
                "Local standing represented as site, operator context, data posture, disclosure posture, and permitted environment."),
            Input(
                LocalizedPreCertificationDataPoolInputKind.DataRightsPosture,
                "data-rights-posture://continuity-bearing-personal-data",
                "Data-rights posture represented as continuity-bearing personal data before generic telemetry."),
            Input(
                LocalizedPreCertificationDataPoolInputKind.ResearchSeparationPosture,
                "research-separation-posture://install-is-not-research-consent",
                "Research separation posture represented; install is not research consent."),
            Input(
                LocalizedPreCertificationDataPoolInputKind.SpecialCaseHold,
                "special-case-hold-ref://personification-and-sensitive-use",
                "Special Cases remain held before widening."),
            Input(
                LocalizedPreCertificationDataPoolInputKind.DomainHold,
                "domain-hold-ref://domain-sensitive-use",
                "Domain-sensitive uses remain held before separate admission."),
            Input(
                LocalizedPreCertificationDataPoolInputKind.NonAuthoritySummary,
                "non-authority-summary://pre-certification-data-pool",
                "Pool contextualizes Sanctuary.GEL only and grants no certification, disclosure, consent, authorization, governance, RTME, first use, or runtime authority.")
        };
    }

    private static IReadOnlyList<LocalizedPreCertificationDataPoolInput> HeldInputs()
    {
        return ReadyInputs()
            .Select(static input => input.Kind switch
            {
                LocalizedPreCertificationDataPoolInputKind.LabAssetCandidate => input with
                {
                    Summary = "Lab asset candidates remain held as review candidates."
                },
                LocalizedPreCertificationDataPoolInputKind.LegalAdminTemplateFamily => input with
                {
                    Summary = "Legal-admin template families remain held for counsel review."
                },
                LocalizedPreCertificationDataPoolInputKind.SpecialCaseHold => input with
                {
                    Summary = "Special Case posture remains held for review."
                },
                LocalizedPreCertificationDataPoolInputKind.DomainHold => input with
                {
                    Summary = "Domain-sensitive use posture remains held for review."
                },
                _ => input
            })
            .ToArray();
    }

    private static LocalizedPreCertificationDataPoolRecord Refused(
        string witnessRef,
        IReadOnlyList<LocalizedPreCertificationDataPoolRefusalReason> refusalReasons,
        IReadOnlyList<LocalizedPreCertificationDataPoolInput> inputs,
        string summary)
    {
        return new(
            Disposition: LocalizedPreCertificationDataPoolDisposition.Refused,
            Inputs: inputs,
            SourceLocalizedStandingRefs: LocalizedStandingRefs,
            SourceLegalAdminStagingRefs: LegalAdminStagingRefs,
            RefusalReasons: refusalReasons,
            NonAuthoritySummary: $"{summary} No certification, disclosure, consent, authorization, governance, RTME, first-use admission, governing CME, Atlas mutation, legal template body, or runtime authority is granted.",
            WitnessRefs: new[]
            {
                witnessRef
            });
    }

    private static LocalizedPreCertificationDataPoolInput Input(
        LocalizedPreCertificationDataPoolInputKind kind,
        string inputRef,
        string summary)
    {
        return new(
            Kind: kind,
            InputRef: inputRef,
            Summary: summary,
            WitnessRefs: new[] { inputRef });
    }

    private static LocalizedPreCertificationDataPoolReceipt Receipt(
        string receiptHandle,
        LocalizedPreCertificationDataPoolRecord record)
    {
        return new(
            ReceiptHandle: receiptHandle,
            Disposition: record.Disposition,
            Summary: record.NonAuthoritySummary,
            WitnessRefs: record.WitnessRefs,
            TimestampUtc: DateTimeOffset.UnixEpoch);
    }
}
