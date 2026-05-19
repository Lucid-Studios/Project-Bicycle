namespace San.Common;

public static class GenerativeEngrammitizationLibraryFormationReadinessReferenceData
{
    private static readonly string[] LogicalResearchRefs =
    {
        "gel-readiness-ref://logical-research-source-posture"
    };

    private static readonly string[] PredicateSurfaceRefs =
    {
        "predicate-surface-ref://ready-predicate-pool",
        "predicate-surface-ref://first-four-candidate-families",
        "predicate-surface-ref://family-bearing-gel-inheritance"
    };

    public static GenerativeEngrammitizationLibraryFormationReadinessRecord ReadyLibraryFormationReadiness { get; } = new(
        Disposition: GenerativeEngrammitizationLibraryFormationReadinessDisposition.Ready,
        CandidatePostures: ReadyPostures(),
        SourceLogicalResearchRefs: LogicalResearchRefs,
        SourcePredicateSurfaceRefs: PredicateSurfaceRefs,
        RefusalReasons: new[] { GenerativeEngrammitizationLibraryFormationReadinessRefusalReason.None },
        NonAdmissionSummary: "Ready GEL formation readiness may hold rooted, witnessed, SLI-formed, Engrammitization-facing candidates, but .GEL inclusion does not equal final Sanctuary.GEL survivor admission, localized formation, first use, governing CME, RTME, or runtime authority.",
        WitnessRefs: new[]
        {
            "gel-readiness-ref://ready-library-formation-readiness",
            "predicate-surface-ref://ready-predicate-pool"
        });

    public static GenerativeEngrammitizationLibraryFormationReadinessRecord HeldForLocalizationOrSurvivorReview { get; } = new(
        Disposition: GenerativeEngrammitizationLibraryFormationReadinessDisposition.Held,
        CandidatePostures: ReadyPostures(),
        SourceLogicalResearchRefs: LogicalResearchRefs,
        SourcePredicateSurfaceRefs: PredicateSurfaceRefs,
        RefusalReasons: new[] { GenerativeEngrammitizationLibraryFormationReadinessRefusalReason.None },
        NonAdmissionSummary: "GEL formation readiness remains held for localization or survivor review; readiness is not localized Sanctuary.GEL formation, standing, first use, or survivor admission.",
        WitnessRefs: new[]
        {
            "gel-readiness-ref://held-localization-or-survivor-review"
        });

    public static GenerativeEngrammitizationLibraryFormationReadinessRecord RefusedMissingRootWitnessOrSliFormation { get; } = Refused(
        "gel-readiness-ref://refused-missing-root-witness-or-sli-formation",
        new[]
        {
            GenerativeEngrammitizationLibraryFormationReadinessRefusalReason.MissingRootedSourcePosture,
            GenerativeEngrammitizationLibraryFormationReadinessRefusalReason.MissingWitnessPosture,
            GenerativeEngrammitizationLibraryFormationReadinessRefusalReason.MissingSliFormationPosture
        },
        ReadyPostures()
            .Where(static posture =>
                posture.Kind is not GenerativeEngrammitizationLibraryCandidatePostureKind.RootedSourcePosture and
                not GenerativeEngrammitizationLibraryCandidatePostureKind.WitnessedPosture and
                not GenerativeEngrammitizationLibraryCandidatePostureKind.SliFormedPosture)
            .ToArray(),
        "Refuses GEL formation readiness because rooting, witness, or SLI formation posture is missing.");

    public static GenerativeEngrammitizationLibraryFormationReadinessRecord RefusedMissingEngrammitizationFacingPosture { get; } = Refused(
        "gel-readiness-ref://refused-missing-engrammitization-facing-posture",
        new[]
        {
            GenerativeEngrammitizationLibraryFormationReadinessRefusalReason.MissingEngrammitizationFacingPosture
        },
        ReadyPostures()
            .Where(static posture => posture.Kind is not GenerativeEngrammitizationLibraryCandidatePostureKind.EngrammitizationFacingPosture)
            .ToArray(),
        "Refuses GEL formation readiness because Engrammitization-facing posture is missing.");

    public static GenerativeEngrammitizationLibraryFormationReadinessRecord RefusedSurvivorLocalizedFirstUseRuntimeOrGovernanceOverclaim { get; } = Refused(
        "gel-readiness-ref://refused-survivor-localized-first-use-runtime-or-governance-overclaim",
        new[]
        {
            GenerativeEngrammitizationLibraryFormationReadinessRefusalReason.SurvivorAdmissionOverclaimed,
            GenerativeEngrammitizationLibraryFormationReadinessRefusalReason.LocalizedSanctuaryGelFormationOverclaimed,
            GenerativeEngrammitizationLibraryFormationReadinessRefusalReason.FirstUseOverclaimed,
            GenerativeEngrammitizationLibraryFormationReadinessRefusalReason.RuntimeOrGovernanceOverclaimed
        },
        ReadyPostures(),
        "Refuses GEL formation readiness because survivor admission, localized Sanctuary.GEL formation, first use, runtime, or governance authority was overclaimed.");

    public static IReadOnlyList<GenerativeEngrammitizationLibraryFormationReadinessRecord> CanonicalRecords { get; } = new[]
    {
        ReadyLibraryFormationReadiness,
        HeldForLocalizationOrSurvivorReview,
        RefusedMissingRootWitnessOrSliFormation,
        RefusedMissingEngrammitizationFacingPosture,
        RefusedSurvivorLocalizedFirstUseRuntimeOrGovernanceOverclaim
    };

    public static GenerativeEngrammitizationLibraryFormationReadinessReceipt ReadyReceipt { get; } = Receipt(
        "gel-readiness-receipt-ref://ready-library-formation-readiness",
        ReadyLibraryFormationReadiness);

    public static GenerativeEngrammitizationLibraryFormationReadinessReceipt HeldReceipt { get; } = Receipt(
        "gel-readiness-receipt-ref://held-localization-or-survivor-review",
        HeldForLocalizationOrSurvivorReview);

    public static GenerativeEngrammitizationLibraryFormationReadinessReceipt RefusedReceipt { get; } = Receipt(
        "gel-readiness-receipt-ref://refused-survivor-localized-first-use-runtime-or-governance-overclaim",
        RefusedSurvivorLocalizedFirstUseRuntimeOrGovernanceOverclaim);

    private static IReadOnlyList<GenerativeEngrammitizationLibraryCandidatePosture> ReadyPostures()
    {
        return new[]
        {
            Posture(
                GenerativeEngrammitizationLibraryCandidatePostureKind.LogicalResearchSourcePosture,
                "gel-readiness-ref://logical-research-source-posture",
                "Logical research source posture represented without path-bound authority."),
            Posture(
                GenerativeEngrammitizationLibraryCandidatePostureKind.RootedSourcePosture,
                "gel-readiness-ref://root-atlas-derived-rooting-posture",
                "RootAtlas-derived rooting posture represented without local Atlas residency or mutation."),
            Posture(
                GenerativeEngrammitizationLibraryCandidatePostureKind.WitnessedPosture,
                "gel-readiness-ref://witness-posture-present",
                "Witness posture represented for the candidate."),
            Posture(
                GenerativeEngrammitizationLibraryCandidatePostureKind.SliFormedPosture,
                "gel-readiness-ref://sli-formation-posture-present",
                "SLI formation posture represented for the candidate."),
            Posture(
                GenerativeEngrammitizationLibraryCandidatePostureKind.EngrammitizationFacingPosture,
                "gel-readiness-ref://engrammitization-facing-posture-present",
                "Engrammitization-facing posture represented for the candidate."),
            Posture(
                GenerativeEngrammitizationLibraryCandidatePostureKind.PredicateSurfaceReadiness,
                "predicate-surface-ref://family-bearing-gel-inheritance",
                "Predicate surface readiness represented as ready predicate pool and first four candidate families."),
            Posture(
                GenerativeEngrammitizationLibraryCandidatePostureKind.FormationLineageSummary,
                "gel-readiness-ref://formation-lineage-summary",
                "Formation lineage summarized for readiness only.")
        };
    }

    private static GenerativeEngrammitizationLibraryFormationReadinessRecord Refused(
        string witnessRef,
        IReadOnlyList<GenerativeEngrammitizationLibraryFormationReadinessRefusalReason> refusalReasons,
        IReadOnlyList<GenerativeEngrammitizationLibraryCandidatePosture> postures,
        string summary)
    {
        return new(
            Disposition: GenerativeEngrammitizationLibraryFormationReadinessDisposition.Refused,
            CandidatePostures: postures,
            SourceLogicalResearchRefs: LogicalResearchRefs,
            SourcePredicateSurfaceRefs: PredicateSurfaceRefs,
            RefusalReasons: refusalReasons,
            NonAdmissionSummary: $"{summary} .GEL inclusion does not grant final Sanctuary.GEL survivor admission, localized formation, standing, first-use admission, governing CME, RTME, or runtime authority.",
            WitnessRefs: new[]
            {
                witnessRef
            });
    }

    private static GenerativeEngrammitizationLibraryCandidatePosture Posture(
        GenerativeEngrammitizationLibraryCandidatePostureKind kind,
        string postureRef,
        string summary)
    {
        return new(
            Kind: kind,
            PostureRef: postureRef,
            Summary: summary,
            WitnessRefs: new[] { postureRef });
    }

    private static GenerativeEngrammitizationLibraryFormationReadinessReceipt Receipt(
        string receiptHandle,
        GenerativeEngrammitizationLibraryFormationReadinessRecord record)
    {
        return new(
            ReceiptHandle: receiptHandle,
            Disposition: record.Disposition,
            Summary: record.NonAdmissionSummary,
            WitnessRefs: record.WitnessRefs,
            TimestampUtc: DateTimeOffset.UnixEpoch);
    }
}
