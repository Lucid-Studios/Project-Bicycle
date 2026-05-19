using San.Common;
using SLI.Runtime;

namespace San.Nexus.Control;

public interface ISliCmeActualOrchestrationReceiptPassagePolicy
{
    SliCmeActualOrchestrationReceiptPassageEvaluation Evaluate(
        SliCmeActualRoundtripScaffoldResult scaffoldResult,
        DateTimeOffset timestampUtc);
}

public sealed class DefaultSliCmeActualOrchestrationReceiptPassagePolicy : ISliCmeActualOrchestrationReceiptPassagePolicy
{
    private static readonly string[] ExpectedPassageKinds =
    [
        "engram-packet",
        "cmos-certification",
        "sli-admission",
        "cme-actual",
        "ec-telemetry",
        "product-engram-response"
    ];

    public SliCmeActualOrchestrationReceiptPassageEvaluation Evaluate(
        SliCmeActualRoundtripScaffoldResult scaffoldResult,
        DateTimeOffset timestampUtc)
    {
        ArgumentNullException.ThrowIfNull(scaffoldResult);

        var orderedRefs = scaffoldResult.ProductResponse?.ReceiptContinuityReceipt?.Chain?.PassageRefs?.ToArray() ?? [];
        var orderedKinds = orderedRefs.Select(static passageRef => passageRef.RefKind).ToArray();
        var receiptHandles = orderedRefs.Select(static passageRef => passageRef.RefHandle).ToArray();
        var admissionDisposition = scaffoldResult.AdmissionReceipt?.Disposition ?? SliCmeActualRoundtripDisposition.Refused;
        var payloadOpened = HasPayloadOpened(scaffoldResult);
        var runtimeIdentityEmitted = HasRuntimeIdentityEmitted(scaffoldResult);
        var runtimeActionExecuted = HasRuntimeActionExecuted(scaffoldResult);
        var authorityGranted = HasAuthorityGranted(scaffoldResult);
        var preservesOrder = orderedKinds.SequenceEqual(ExpectedPassageKinds);
        var preservesAnchorContinuity = PreservesAnchorContinuity(scaffoldResult);
        var preservesNonActivation = PreservesNonActivation(scaffoldResult);

        if (!preservesOrder)
        {
            return CreateEvaluation(
                scaffoldResult,
                admissionDisposition,
                "receipt-passage-order-mismatch",
                "SW-04 requires ordered receipt passage from packet through product response.",
                orderedKinds,
                receiptHandles,
                receiptPassageAccepted: false,
                preservesOrder,
                preservesAnchorContinuity,
                preservesNonActivation,
                payloadOpened,
                runtimeIdentityEmitted,
                runtimeActionExecuted,
                authorityGranted,
                timestampUtc);
        }

        if (!preservesAnchorContinuity)
        {
            return CreateEvaluation(
                scaffoldResult,
                admissionDisposition,
                "anchor-continuity-passage-mismatch",
                "SW-04 may not pass a receipt thread with drifted anchor continuity.",
                orderedKinds,
                receiptHandles,
                receiptPassageAccepted: false,
                preservesOrder,
                preservesAnchorContinuity,
                preservesNonActivation,
                payloadOpened,
                runtimeIdentityEmitted,
                runtimeActionExecuted,
                authorityGranted,
                timestampUtc);
        }

        if (!preservesNonActivation)
        {
            return CreateEvaluation(
                scaffoldResult,
                admissionDisposition,
                "non-activation-passage-mismatch",
                "SW-04 may not pass a receipt thread with activation or inertness drift.",
                orderedKinds,
                receiptHandles,
                receiptPassageAccepted: false,
                preservesOrder,
                preservesAnchorContinuity,
                preservesNonActivation,
                payloadOpened,
                runtimeIdentityEmitted,
                runtimeActionExecuted,
                authorityGranted,
                timestampUtc);
        }

        if (payloadOpened || runtimeIdentityEmitted || runtimeActionExecuted || authorityGranted)
        {
            return CreateEvaluation(
                scaffoldResult,
                admissionDisposition,
                "orchestration-authority-or-payload-drift-blocked",
                "SW-04 carries receipt handles only; it may not open payloads, start runtime work, emit identity, or grant authority.",
                orderedKinds,
                receiptHandles,
                receiptPassageAccepted: false,
                preservesOrder,
                preservesAnchorContinuity,
                preservesNonActivation,
                payloadOpened,
                runtimeIdentityEmitted,
                runtimeActionExecuted,
                authorityGranted,
                timestampUtc);
        }

        if (!ExtendsInOrder(scaffoldResult))
        {
            return CreateEvaluation(
                scaffoldResult,
                admissionDisposition,
                "receipt-continuity-extension-mismatch",
                "SW-04 receipt continuity must extend forward without substitution, repair, collapse, upgrade, or forgery.",
                orderedKinds,
                receiptHandles,
                receiptPassageAccepted: false,
                preservesOrder,
                preservesAnchorContinuity,
                preservesNonActivation,
                payloadOpened,
                runtimeIdentityEmitted,
                runtimeActionExecuted,
                authorityGranted,
                timestampUtc);
        }

        return CreateEvaluation(
            scaffoldResult,
            admissionDisposition,
            "sw04-orchestration-receipt-passage-accepted",
            "orchestration carries ordered receipt evidence without becoming authority.",
            orderedKinds,
            receiptHandles,
            receiptPassageAccepted: true,
            preservesOrder,
            preservesAnchorContinuity,
            preservesNonActivation,
            payloadOpened,
            runtimeIdentityEmitted,
            runtimeActionExecuted,
            authorityGranted,
            timestampUtc);
    }

    private static SliCmeActualOrchestrationReceiptPassageEvaluation CreateEvaluation(
        SliCmeActualRoundtripScaffoldResult scaffoldResult,
        SliCmeActualRoundtripDisposition admissionDisposition,
        string outcomeCode,
        string governanceTrace,
        IReadOnlyList<string> orderedPassageKinds,
        IReadOnlyList<string> receiptHandles,
        bool receiptPassageAccepted,
        bool preservesOrder,
        bool preservesAnchorContinuity,
        bool preservesNonActivation,
        bool payloadOpened,
        bool runtimeIdentityEmitted,
        bool runtimeActionExecuted,
        bool authorityGranted,
        DateTimeOffset timestampUtc)
    {
        var evaluationHandle = $"sw04-orchestration-receipt-passage://{Math.Abs(HashCode.Combine(
            scaffoldResult.ProductResponse?.ResponseHandle,
            outcomeCode,
            receiptHandles.Count)):x}";
        var sw05HandoffEvidencePrepared =
            receiptPassageAccepted &&
            scaffoldResult.ProductResponse?.Disposition == ProductEngramResponseDisposition.ReceiptOnly;

        return new SliCmeActualOrchestrationReceiptPassageEvaluation(
            EvaluationHandle: evaluationHandle,
            AdmissionDisposition: admissionDisposition,
            OutcomeCode: outcomeCode,
            GovernanceTrace: governanceTrace,
            OrderedPassageKinds: orderedPassageKinds.ToArray(),
            ReceiptHandles: receiptHandles.ToArray(),
            ReceiptPassageAccepted: receiptPassageAccepted,
            PreservesOrder: preservesOrder,
            PreservesAnchorContinuity: preservesAnchorContinuity,
            PreservesNonActivation: preservesNonActivation,
            PayloadOpened: payloadOpened,
            RuntimeIdentityEmitted: runtimeIdentityEmitted,
            RuntimeActionExecuted: runtimeActionExecuted,
            AuthorityGranted: authorityGranted,
            RuntimeWorkStarted: runtimeIdentityEmitted || runtimeActionExecuted,
            LispEvaluationAllowed: false,
            ModelBindingAllowed: false,
            GelPromotionAllowed: false,
            MintingAuthorized: false,
            CertificationAuthorized: false,
            Sw05HandoffEvidencePrepared: sw05HandoffEvidencePrepared,
            TimestampUtc: timestampUtc);
    }

    private static bool PreservesAnchorContinuity(SliCmeActualRoundtripScaffoldResult result)
    {
        var anchor = result.EngramPacket?.AnchorContinuityReceipt;
        return anchor is not null &&
            anchor.HasSameAnchorAs(result.AdmissionReceipt?.AnchorContinuityReceipt) &&
            anchor.HasSameAnchorAs(result.CmeActualContract?.AnchorContinuityReceipt) &&
            anchor.HasSameAnchorAs(result.TelemetryEvent?.AnchorContinuityReceipt) &&
            anchor.HasSameAnchorAs(result.ProductResponse?.AnchorContinuityReceipt);
    }

    private static bool PreservesNonActivation(SliCmeActualRoundtripScaffoldResult result)
    {
        var inertness = result.EngramPacket?.NonActivationReceipt;
        return inertness is not null &&
            inertness.HasSameInertnessAs(result.AdmissionReceipt?.NonActivationReceipt) &&
            inertness.HasSameInertnessAs(result.CmeActualContract?.NonActivationReceipt) &&
            inertness.HasSameInertnessAs(result.TelemetryEvent?.NonActivationReceipt) &&
            inertness.HasSameInertnessAs(result.ProductResponse?.NonActivationReceipt) &&
            !inertness.HasPrematureActivation &&
            result.AdmissionReceipt?.NonActivationReceipt?.HasPrematureActivation == false &&
            result.CmeActualContract?.NonActivationReceipt?.HasPrematureActivation == false &&
            result.TelemetryEvent?.NonActivationReceipt?.HasPrematureActivation == false &&
            result.ProductResponse?.NonActivationReceipt?.HasPrematureActivation == false;
    }

    private static bool ExtendsInOrder(SliCmeActualRoundtripScaffoldResult result) =>
        result.AdmissionReceipt?.ReceiptContinuityReceipt?.ExtendsReceipt(result.EngramPacket?.ReceiptContinuityReceipt) == true &&
        result.CmeActualContract?.ReceiptContinuityReceipt?.ExtendsReceipt(result.AdmissionReceipt.ReceiptContinuityReceipt) == true &&
        result.TelemetryEvent?.ReceiptContinuityReceipt?.ExtendsReceipt(result.CmeActualContract.ReceiptContinuityReceipt) == true &&
        result.ProductResponse?.ReceiptContinuityReceipt?.ExtendsReceipt(result.TelemetryEvent.ReceiptContinuityReceipt) == true &&
        result.ProductResponse.ReceiptContinuityReceipt.ContainsPassageRef("product-engram-response", result.ProductResponse.ResponseHandle);

    private static bool HasPayloadOpened(SliCmeActualRoundtripScaffoldResult result) =>
        result.EngramPacket?.AnchorContinuityReceipt?.Anchor?.PayloadOpened == true ||
        result.EngramPacket?.AnchorContinuityReceipt?.PayloadCarried == true ||
        result.EngramPacket?.ReceiptContinuityReceipt?.Chain?.PassageRefs?.Any(static passageRef => passageRef.PayloadOpened) == true ||
        result.AdmissionReceipt?.ReceiptContinuityReceipt?.Chain?.PassageRefs?.Any(static passageRef => passageRef.PayloadOpened) == true ||
        result.CmeActualContract?.ReceiptContinuityReceipt?.Chain?.PassageRefs?.Any(static passageRef => passageRef.PayloadOpened) == true ||
        result.TelemetryEvent?.ReceiptContinuityReceipt?.Chain?.PassageRefs?.Any(static passageRef => passageRef.PayloadOpened) == true ||
        result.ProductResponse?.ReceiptContinuityReceipt?.Chain?.PassageRefs?.Any(static passageRef => passageRef.PayloadOpened) == true;

    private static bool HasRuntimeIdentityEmitted(SliCmeActualRoundtripScaffoldResult result) =>
        result.EngramPacket?.RuntimeIdentityEmissionAllowed == true ||
        result.AdmissionReceipt?.RuntimeIdentityEmissionAllowed == true ||
        result.CmeActualContract?.RuntimeIdentityEmitted == true ||
        result.TelemetryEvent?.RuntimeIdentityEmitted == true ||
        result.ProductResponse?.RuntimeIdentityEmitted == true;

    private static bool HasRuntimeActionExecuted(SliCmeActualRoundtripScaffoldResult result) =>
        result.TelemetryEvent?.RuntimeActionExecuted == true ||
        result.ProductResponse?.RuntimeActionExecuted == true;

    private static bool HasAuthorityGranted(SliCmeActualRoundtripScaffoldResult result) =>
        result.EngramPacket?.RawGelPromoted == true ||
        result.ProductResponse?.PublicationReady == true ||
        result.ProductResponse?.Disposition != ProductEngramResponseDisposition.ReceiptOnly ||
        result.ProductResponse?.ReceiptContinuityReceipt?.HasForbiddenActivation == true ||
        result.TelemetryEvent?.ReceiptContinuityReceipt?.HasForbiddenActivation == true ||
        result.CmeActualContract?.ReceiptContinuityReceipt?.HasForbiddenActivation == true ||
        result.AdmissionReceipt?.ReceiptContinuityReceipt?.HasForbiddenActivation == true ||
        result.EngramPacket?.ReceiptContinuityReceipt?.HasForbiddenActivation == true;
}

public sealed record SliCmeActualOrchestrationReceiptPassageEvaluation(
    string EvaluationHandle,
    SliCmeActualRoundtripDisposition AdmissionDisposition,
    string OutcomeCode,
    string GovernanceTrace,
    IReadOnlyList<string> OrderedPassageKinds,
    IReadOnlyList<string> ReceiptHandles,
    bool ReceiptPassageAccepted,
    bool PreservesOrder,
    bool PreservesAnchorContinuity,
    bool PreservesNonActivation,
    bool PayloadOpened,
    bool RuntimeIdentityEmitted,
    bool RuntimeActionExecuted,
    bool AuthorityGranted,
    bool RuntimeWorkStarted,
    bool LispEvaluationAllowed,
    bool ModelBindingAllowed,
    bool GelPromotionAllowed,
    bool MintingAuthorized,
    bool CertificationAuthorized,
    bool Sw05HandoffEvidencePrepared,
    DateTimeOffset TimestampUtc);
