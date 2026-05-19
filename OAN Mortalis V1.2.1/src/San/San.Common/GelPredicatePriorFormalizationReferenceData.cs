namespace San.Common;

public static class GelPredicatePriorFormalizationReferenceData
{
    private static readonly string[] DefaultInvariants =
    {
        "root-body-preserved",
        "utf8-witness-preserved",
        "constructor-attachment-fields-preserved",
        "admission-ceiling-candidate-only"
    };

    public static GelPredicatePriorFormalizationRecord ReadyPredicatePrior { get; } = new(
        Disposition: GelPredicatePriorFormalizationDisposition.Ready,
        PredicatePriorRef: "gel-predicate-prior-ref://rt-accept-107",
        Utf8Witness: ReadyUtf8Witness(),
        RootPredicate: ReadyRootPredicate(),
        Constructor: ReadyConstructor(),
        GelPriorSummary: "Bounded .GEL predicate prior for a rooted, witnessed, SLI-constructor-bearing, Engrammitization-facing candidate.",
        InvariantSummaries: DefaultInvariants,
        MutationPolicyPosture: GelPredicatePriorMutationPolicyPosture.InvariantPreservingOnly,
        TransportReceiptPosture: GelPredicatePriorTransportReceiptPosture.FutureReceiptEligible,
        AdmissionCeiling: GelPredicatePriorAdmissionCeiling.CandidateOnly,
        RefusalReasons: new[] { GelPredicatePriorFormalizationRefusalReason.None },
        NonAdmissionSummary: "Ready .GEL predicate prior remains candidate-only; it does not admit final Sanctuary.GEL survivor standing, grant first use, authorize EC mutation, activate SLI.Lisp, emit transport, or create runtime authority.",
        WitnessRefs: new[]
        {
            "gel-predicate-prior-ref://rt-accept-107",
            "utf8-witness-ref://accept-source",
            "root-predicate-ref://rt-accept-107",
            "sli-constructor-ref://attachment-native-accept-107"
        });

    public static GelPredicatePriorFormalizationRecord HeldForAmbiguityOrCarrierReview { get; } = ReadyPredicatePrior with
    {
        Disposition = GelPredicatePriorFormalizationDisposition.Held,
        PredicatePriorRef = "gel-predicate-prior-ref://held-ambiguity-or-carrier-review",
        GelPriorSummary = "Held .GEL predicate prior remains under ambiguity or carrier review before any later formation, mutation, transport, or admission work.",
        MutationPolicyPosture = GelPredicatePriorMutationPolicyPosture.Withheld,
        TransportReceiptPosture = GelPredicatePriorTransportReceiptPosture.NotEmitted,
        NonAdmissionSummary = "Held .GEL predicate prior remains candidate-only and non-operative while ambiguity or carrier posture is reviewed.",
        WitnessRefs = new[]
        {
            "gel-predicate-prior-ref://held-ambiguity-or-carrier-review"
        }
    };

    public static GelPredicatePriorFormalizationRecord RefusedMissingUtf8Witness { get; } = Refused(
        "gel-predicate-prior-ref://refused-missing-utf8-witness",
        new[] { GelPredicatePriorFormalizationRefusalReason.MissingUtf8Witness },
        ReadyUtf8Witness() with
        {
            SourceTextRef = "missing",
            EncodingState = "missing",
            TokenSpanBounds = "missing",
            UnicodeSnapshotRef = "missing"
        },
        ReadyRootPredicate(),
        ReadyConstructor(),
        "Refuses .GEL predicate-prior formalization because UTF-8 witness posture is missing.");

    public static GelPredicatePriorFormalizationRecord RefusedMissingRootPredicate { get; } = Refused(
        "gel-predicate-prior-ref://refused-missing-root-predicate",
        new[] { GelPredicatePriorFormalizationRefusalReason.MissingRootPredicate },
        ReadyUtf8Witness(),
        ReadyRootPredicate() with
        {
            RootPredicateRef = "missing",
            RootCarrier = "missing",
            SemanticFormationRef = "missing"
        },
        ReadyConstructor(),
        "Refuses .GEL predicate-prior formalization because RootAtlas root predicate posture is missing.");

    public static GelPredicatePriorFormalizationRecord RefusedMissingSliConstructor { get; } = Refused(
        "gel-predicate-prior-ref://refused-missing-sli-constructor",
        new[] { GelPredicatePriorFormalizationRefusalReason.MissingSliConstructor },
        ReadyUtf8Witness(),
        ReadyRootPredicate(),
        ReadyConstructor() with
        {
            PrefixSuper = "missing",
            PrefixSub = "missing",
            Body = "missing",
            SuffixSuper = "missing",
            SuffixSub = "missing"
        },
        "Refuses .GEL predicate-prior formalization because attachment-native SLI constructor posture is missing.");

    public static GelPredicatePriorFormalizationRecord RefusedMissingEngrammitizationOrPredicateSurface { get; } = Refused(
        "gel-predicate-prior-ref://refused-missing-engrammitization-or-predicate-surface",
        new[]
        {
            GelPredicatePriorFormalizationRefusalReason.MissingEngrammitizationFacingPosture,
            GelPredicatePriorFormalizationRefusalReason.MissingPredicateSurfaceReadiness
        },
        ReadyUtf8Witness(),
        ReadyRootPredicate(),
        ReadyConstructor(),
        "Refuses .GEL predicate-prior formalization because Engrammitization-facing posture or predicate-surface readiness is missing.");

    public static GelPredicatePriorFormalizationRecord RefusedAdmissionMutationLispRuntimeOrGovernanceOverclaim { get; } = Refused(
        "gel-predicate-prior-ref://refused-admission-mutation-lisp-runtime-or-governance-overclaim",
        new[]
        {
            GelPredicatePriorFormalizationRefusalReason.SurvivorAdmissionOverclaimed,
            GelPredicatePriorFormalizationRefusalReason.FirstUseOverclaimed,
            GelPredicatePriorFormalizationRefusalReason.EcMutationOrTransportOverclaimed,
            GelPredicatePriorFormalizationRefusalReason.SliLispActivationOverclaimed,
            GelPredicatePriorFormalizationRefusalReason.RuntimeOrGovernanceOverclaimed
        },
        ReadyUtf8Witness(),
        ReadyRootPredicate(),
        ReadyConstructor(),
        "Refuses .GEL predicate-prior formalization because survivor admission, first use, EC mutation, SLI.Lisp activation, runtime, or governance authority was overclaimed.");

    public static IReadOnlyList<GelPredicatePriorFormalizationRecord> CanonicalRecords { get; } = new[]
    {
        ReadyPredicatePrior,
        HeldForAmbiguityOrCarrierReview,
        RefusedMissingUtf8Witness,
        RefusedMissingRootPredicate,
        RefusedMissingSliConstructor,
        RefusedMissingEngrammitizationOrPredicateSurface,
        RefusedAdmissionMutationLispRuntimeOrGovernanceOverclaim
    };

    public static GelPredicatePriorFormalizationReceipt ReadyReceipt { get; } = Receipt(
        "gel-predicate-prior-receipt-ref://ready-rt-accept-107",
        ReadyPredicatePrior);

    public static GelPredicatePriorFormalizationReceipt HeldReceipt { get; } = Receipt(
        "gel-predicate-prior-receipt-ref://held-ambiguity-or-carrier-review",
        HeldForAmbiguityOrCarrierReview);

    public static GelPredicatePriorFormalizationReceipt RefusedReceipt { get; } = Receipt(
        "gel-predicate-prior-receipt-ref://refused-admission-mutation-lisp-runtime-or-governance-overclaim",
        RefusedAdmissionMutationLispRuntimeOrGovernanceOverclaim);

    private static GelPredicatePriorUtf8Witness ReadyUtf8Witness()
    {
        return new(
            SourceTextRef: "utf8-witness-ref://accept-source",
            EncodingState: "utf-8-preserved",
            TokenSpanBounds: "span-bounds-ref://accept-source",
            LocalContextRef: "local-context-ref://install-facing-sample",
            SourceWitnessRef: "source-witness-ref://accept-source",
            AmbiguitySummary: "No ambiguity resolved by symbolics; unresolved ambiguity remains witness-bearing.",
            UnicodeSnapshotRef: "unicode-snapshot-ref://accept-source",
            WitnessRefs: new[]
            {
                "utf8-witness-ref://accept-source",
                "unicode-snapshot-ref://accept-source"
            });
    }

    private static GelPredicatePriorRootPredicate ReadyRootPredicate()
    {
        return new(
            RootPredicateRef: "root-predicate-ref://rt-accept-107",
            RootCarrier: "rt.accept",
            SemanticFormationRef: "root-semantic-formation-ref://rt-accept-107",
            LineageSummary: "RootAtlas-derived predicate identity is lineage-bearing only and does not create local Atlas authority.",
            WitnessRefs: new[]
            {
                "root-predicate-ref://rt-accept-107"
            });
    }

    private static SliSymbolicConstructorAttachment ReadyConstructor()
    {
        return new(
            PrefixSuper: "a",
            PrefixSub: "complement",
            Body: "delta-body",
            SuffixSuper: "K",
            SuffixSub: "107",
            StructuralSummary: "Attachment-native SLI constructor uses prefix.super, prefix.sub, body, suffix.super, and suffix.sub; flat rendering is display-only.",
            WitnessRefs: new[]
            {
                "sli-constructor-ref://attachment-native-accept-107"
            });
    }

    private static GelPredicatePriorFormalizationRecord Refused(
        string predicatePriorRef,
        IReadOnlyList<GelPredicatePriorFormalizationRefusalReason> refusalReasons,
        GelPredicatePriorUtf8Witness utf8Witness,
        GelPredicatePriorRootPredicate rootPredicate,
        SliSymbolicConstructorAttachment constructor,
        string summary)
    {
        return new(
            Disposition: GelPredicatePriorFormalizationDisposition.Refused,
            PredicatePriorRef: predicatePriorRef,
            Utf8Witness: utf8Witness,
            RootPredicate: rootPredicate,
            Constructor: constructor,
            GelPriorSummary: summary,
            InvariantSummaries: DefaultInvariants,
            MutationPolicyPosture: GelPredicatePriorMutationPolicyPosture.Withheld,
            TransportReceiptPosture: GelPredicatePriorTransportReceiptPosture.RefusedAsActiveTransport,
            AdmissionCeiling: GelPredicatePriorAdmissionCeiling.CandidateOnly,
            RefusalReasons: refusalReasons,
            NonAdmissionSummary: $"{summary} No final Sanctuary.GEL survivor admission, first use, EC mutation, SLI.Lisp activation, transport emission, governing CME, RTME, or runtime authority is granted.",
            WitnessRefs: new[]
            {
                predicatePriorRef
            });
    }

    private static GelPredicatePriorFormalizationReceipt Receipt(
        string receiptHandle,
        GelPredicatePriorFormalizationRecord record)
    {
        return new(
            ReceiptHandle: receiptHandle,
            Disposition: record.Disposition,
            Summary: record.NonAdmissionSummary,
            WitnessRefs: record.WitnessRefs,
            TimestampUtc: DateTimeOffset.UnixEpoch);
    }
}
