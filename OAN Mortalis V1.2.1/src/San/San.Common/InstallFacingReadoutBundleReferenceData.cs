namespace San.Common;

public static class InstallFacingReadoutBundleReferenceData
{
    public static InstallFacingReadoutBundle Ready { get; } = CreateReadyBundle();

    public static InstallFacingReadoutBundle Silence { get; } = new(
        Disposition: InstallFacingReadoutBundleDisposition.Silence,
        Sections: new[]
        {
            new InstallFacingReadoutSection(
                SectionKind: InstallFacingReadoutSectionKind.ResponseDisposition,
                Entries: new[]
                {
                    CreateEntry(SanctuaryGelPredicateCandidateKind.Silence)
                })
        },
        CorrespondenceRefs: new[]
        {
            nameof(SanctuaryGelPredicateCandidateKind.Silence)
        },
        WitnessRefs: new[]
        {
            "readout-bundle://silence"
        });

    public static InstallFacingReadoutBundle Refused { get; } = new(
        Disposition: InstallFacingReadoutBundleDisposition.Refused,
        Sections: new[]
        {
            new InstallFacingReadoutSection(
                SectionKind: InstallFacingReadoutSectionKind.ResponseDisposition,
                Entries: new[]
                {
                    CreateEntry(SanctuaryGelPredicateCandidateKind.Refused)
                })
        },
        CorrespondenceRefs: new[]
        {
            nameof(SanctuaryGelPredicateCandidateKind.Refused)
        },
        WitnessRefs: new[]
        {
            "readout-bundle://refused"
        });

    public static InstallFacingReadoutReceipt ReadyReceipt { get; } = new(
        ReceiptHandle: "install-facing-readout-receipt://ready",
        Disposition: InstallFacingReadoutBundleDisposition.Ready,
        Summary: "Ready readout bundle preserves posture, trust, evidence, and response wording in bounded outward form.",
        WitnessRefs: new[] { "readout-bundle://ready" },
        TimestampUtc: DateTimeOffset.UnixEpoch);

    public static InstallFacingReadoutReceipt SilenceReceipt { get; } = new(
        ReceiptHandle: "install-facing-readout-receipt://silence",
        Disposition: InstallFacingReadoutBundleDisposition.Silence,
        Summary: "Silence readout bundle preserves non-teaching silent response footing only.",
        WitnessRefs: new[] { "readout-bundle://silence" },
        TimestampUtc: DateTimeOffset.UnixEpoch);

    public static InstallFacingReadoutReceipt RefusedReceipt { get; } = new(
        ReceiptHandle: "install-facing-readout-receipt://refused",
        Disposition: InstallFacingReadoutBundleDisposition.Refused,
        Summary: "Refused readout bundle preserves bounded refusal footing only.",
        WitnessRefs: new[] { "readout-bundle://refused" },
        TimestampUtc: DateTimeOffset.UnixEpoch);

    private static InstallFacingReadoutBundle CreateReadyBundle()
    {
        var postureEntries = new[]
        {
            CreateEntry(SanctuaryGelPredicateCandidateKind.InstallFacing),
            CreateEntry(SanctuaryGelPredicateCandidateKind.ConversationalMovement),
            CreateEntry(SanctuaryGelPredicateCandidateKind.GoverningSeatCandidate),
            CreateEntry(SanctuaryGelPredicateCandidateKind.ResearchAttached)
        };
        var trustEntries = new[]
        {
            CreateEntry(SanctuaryGelPredicateCandidateKind.CertifiedCommunication),
            CreateEntry(SanctuaryGelPredicateCandidateKind.RegionalPackageAdmitted),
            CreateEntry(SanctuaryGelPredicateCandidateKind.UniversalAtlasAuthorityWithheld)
        };
        var evidenceEntries = new[]
        {
            CreateEntry(SanctuaryGelPredicateCandidateKind.AssentWitnessed),
            CreateEntry(SanctuaryGelPredicateCandidateKind.PackageWitnessed),
            CreateEntry(SanctuaryGelPredicateCandidateKind.PredicateInheritanceWitnessed)
        };
        var responseEntries = new[]
        {
            CreateEntry(SanctuaryGelPredicateCandidateKind.Ready)
        };

        return new InstallFacingReadoutBundle(
            Disposition: InstallFacingReadoutBundleDisposition.Ready,
            Sections: new[]
            {
                new InstallFacingReadoutSection(InstallFacingReadoutSectionKind.Posture, postureEntries),
                new InstallFacingReadoutSection(InstallFacingReadoutSectionKind.TrustAuthorization, trustEntries),
                new InstallFacingReadoutSection(InstallFacingReadoutSectionKind.EvidenceFooting, evidenceEntries),
                new InstallFacingReadoutSection(InstallFacingReadoutSectionKind.ResponseDisposition, responseEntries)
            },
            CorrespondenceRefs: postureEntries.Concat(trustEntries).Concat(evidenceEntries).Concat(responseEntries)
                .Select(static entry => entry.PredicateCandidateKind.ToString())
                .ToArray(),
            WitnessRefs: new[]
            {
                "readout-bundle://ready"
            });
    }

    private static InstallFacingReadoutEntry CreateEntry(SanctuaryGelPredicateCandidateKind kind)
    {
        var correspondence = InstallFacingPredicatePostureCorrespondenceReferenceData.Canonical.Correspondences
            .Single(entry => entry.PredicateCandidateKind == kind);

        return new InstallFacingReadoutEntry(
            SectionKind: (InstallFacingReadoutSectionKind)correspondence.Lane,
            PredicateFamily: correspondence.PredicateFamily,
            PredicateCandidateKind: correspondence.PredicateCandidateKind,
            Phrase: correspondence.InstallFacingPhrase,
            Summary: correspondence.InstallFacingSummary,
            OperatorVisible: correspondence.OperatorVisible,
            CertifiedLaneOnly: correspondence.CertifiedLaneOnly);
    }
}
