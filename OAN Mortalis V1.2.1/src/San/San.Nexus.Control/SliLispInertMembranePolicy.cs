using San.Common;

namespace San.Nexus.Control;

public interface ISliLispInertMembranePolicy
{
    SliLispInertMembraneEvaluation Evaluate(
        SliCmeActualOrchestrationReceiptPassageEvaluation passageEvaluation,
        IReadOnlyDictionary<string, string> lispModules,
        DateTimeOffset timestampUtc);
}

public sealed class DefaultSliLispInertMembranePolicy : ISliLispInertMembranePolicy
{
    public const string RoundtripModuleName = "sli-cme-actual-roundtrip.lisp";
    public const string NonActivationPosture = ":non-activation :preserved-not-evaluated";
    public const string ReceiptContinuityPosture = ":receipt-continuity :proof-of-passage-preserved";
    public const string LispEvaluationNilPosture = ":lisp-evaluation-requested nil";
    public const string LispMorphologyPromotionNilPosture = ":lisp-morphology-promotion-requested nil";
    public const string ReturnPosture = ":return :receipt-only";

    private static readonly string[] RequiredRoundtripPostures =
    [
        NonActivationPosture,
        ReceiptContinuityPosture,
        LispEvaluationNilPosture,
        LispMorphologyPromotionNilPosture
    ];

    private static readonly IReadOnlyDictionary<string, string> LispFacingLabels = new Dictionary<string, string>(StringComparer.Ordinal)
    {
        ["evaluation_handle"] = ":evaluation-handle",
        ["ordered_passage_kinds"] = ":ordered-passage-kinds",
        ["receipt_handles"] = ":receipt-handles",
        ["governance_trace"] = ":governance-trace",
        ["preserves_non_activation"] = ":non-activation :preserved-not-evaluated",
        ["witness_refs"] = ":witness-refs",
        ["sw05_handoff_evidence_prepared"] = ":return :receipt-only"
    };

    public SliLispInertMembraneEvaluation Evaluate(
        SliCmeActualOrchestrationReceiptPassageEvaluation passageEvaluation,
        IReadOnlyDictionary<string, string> lispModules,
        DateTimeOffset timestampUtc)
    {
        ArgumentNullException.ThrowIfNull(passageEvaluation);

        if (!IsAcceptedPassage(passageEvaluation))
        {
            return CreateEvaluation(
                passageEvaluation,
                SliCmeActualRoundtripDisposition.Refused,
                "sw04-passage-not-accepted",
                $"SW-05 refused carrier shaping because SW-04 passage is not accepted: {passageEvaluation.OutcomeCode}. {passageEvaluation.GovernanceTrace}",
                carrier: null,
                timestampUtc);
        }

        if (lispModules is null ||
            !lispModules.TryGetValue(RoundtripModuleName, out var roundtripSource) ||
            string.IsNullOrWhiteSpace(roundtripSource))
        {
            return CreateEvaluation(
                passageEvaluation,
                SliCmeActualRoundtripDisposition.Withheld,
                "sli-lisp-roundtrip-module-missing",
                "SW-05 withheld carrier shaping because the inert roundtrip Lisp source module is missing.",
                carrier: null,
                timestampUtc);
        }

        var missingPostures = RequiredRoundtripPostures
            .Where(posture => !roundtripSource.Contains(posture, StringComparison.Ordinal))
            .ToArray();

        if (missingPostures.Length > 0)
        {
            return CreateEvaluation(
                passageEvaluation,
                SliCmeActualRoundtripDisposition.Withheld,
                "sli-lisp-inert-stub-posture-missing",
                $"SW-05 withheld carrier shaping because required inert Lisp posture is missing: {string.Join(", ", missingPostures)}.",
                carrier: null,
                timestampUtc);
        }

        var carrier = new SliLispInertSymbolicCarrier(
            CarrierHandle: $"sw05-inert-lisp-carrier://{Math.Abs(HashCode.Combine(passageEvaluation.EvaluationHandle, RoundtripModuleName)):x}",
            SourceModuleName: RoundtripModuleName,
            PassageEvaluationHandle: passageEvaluation.EvaluationHandle,
            ReceiptHandles: passageEvaluation.ReceiptHandles.ToArray(),
            OrderedPassageKinds: passageEvaluation.OrderedPassageKinds.ToArray(),
            GovernanceTrace: passageEvaluation.GovernanceTrace,
            NonActivationPosture: NonActivationPosture,
            ReceiptContinuityPosture: ReceiptContinuityPosture,
            LispFacingLabels: LispFacingLabels,
            ReturnPosture: ReturnPosture,
            PayloadOpened: false,
            ExecutableLispCarried: false,
            GoaControlMatrixDisclosed: false);

        return CreateEvaluation(
            passageEvaluation,
            SliCmeActualRoundtripDisposition.Admitted,
            "sw05-inert-lisp-symbolic-carrier-shaped",
            "SW-05 shaped inert Lisp-facing symbolic carrier from accepted receipt passage without execution or authority.",
            carrier,
            timestampUtc);
    }

    private static bool IsAcceptedPassage(SliCmeActualOrchestrationReceiptPassageEvaluation evaluation) =>
        evaluation.ReceiptPassageAccepted &&
        evaluation.PreservesOrder &&
        evaluation.PreservesAnchorContinuity &&
        evaluation.PreservesNonActivation &&
        evaluation.Sw05HandoffEvidencePrepared &&
        !evaluation.PayloadOpened &&
        !evaluation.RuntimeIdentityEmitted &&
        !evaluation.RuntimeActionExecuted &&
        !evaluation.AuthorityGranted &&
        !evaluation.RuntimeWorkStarted &&
        !evaluation.LispEvaluationAllowed &&
        !evaluation.ModelBindingAllowed &&
        !evaluation.GelPromotionAllowed &&
        !evaluation.MintingAuthorized &&
        !evaluation.CertificationAuthorized;

    private static SliLispInertMembraneEvaluation CreateEvaluation(
        SliCmeActualOrchestrationReceiptPassageEvaluation passageEvaluation,
        SliCmeActualRoundtripDisposition disposition,
        string outcomeCode,
        string governanceTrace,
        SliLispInertSymbolicCarrier? carrier,
        DateTimeOffset timestampUtc) =>
        new(
            EvaluationHandle: $"sw05-inert-lisp-membrane://{Math.Abs(HashCode.Combine(passageEvaluation.EvaluationHandle, outcomeCode)):x}",
            PassageEvaluationHandle: passageEvaluation.EvaluationHandle,
            Disposition: disposition,
            OutcomeCode: outcomeCode,
            GovernanceTrace: governanceTrace,
            Carrier: carrier,
            LispEvaluationRequested: false,
            LispLoadRequested: false,
            LispCompileRequested: false,
            MacroExpansionRequested: false,
            MorphologyPromotionRequested: false,
            ModelBindingRequested: false,
            EcStartRequested: false,
            RuntimeIdentityEmitted: false,
            RuntimeActionExecuted: false,
            GelPromotionAllowed: false,
            GoaControlMatrixDisclosed: false,
            TimestampUtc: timestampUtc);
}

public sealed record SliLispInertSymbolicCarrier(
    string CarrierHandle,
    string SourceModuleName,
    string PassageEvaluationHandle,
    IReadOnlyList<string> ReceiptHandles,
    IReadOnlyList<string> OrderedPassageKinds,
    string GovernanceTrace,
    string NonActivationPosture,
    string ReceiptContinuityPosture,
    IReadOnlyDictionary<string, string> LispFacingLabels,
    string ReturnPosture,
    bool PayloadOpened,
    bool ExecutableLispCarried,
    bool GoaControlMatrixDisclosed);

public sealed record SliLispInertMembraneEvaluation(
    string EvaluationHandle,
    string PassageEvaluationHandle,
    SliCmeActualRoundtripDisposition Disposition,
    string OutcomeCode,
    string GovernanceTrace,
    SliLispInertSymbolicCarrier? Carrier,
    bool LispEvaluationRequested,
    bool LispLoadRequested,
    bool LispCompileRequested,
    bool MacroExpansionRequested,
    bool MorphologyPromotionRequested,
    bool ModelBindingRequested,
    bool EcStartRequested,
    bool RuntimeIdentityEmitted,
    bool RuntimeActionExecuted,
    bool GelPromotionAllowed,
    bool GoaControlMatrixDisclosed,
    DateTimeOffset TimestampUtc);
