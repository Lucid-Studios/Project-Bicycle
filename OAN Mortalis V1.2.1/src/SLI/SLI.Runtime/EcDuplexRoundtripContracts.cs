using San.Common;

namespace SLI.Runtime;

public enum EcDuplexRoundtripEventKind
{
    ScaffoldReceipt = 0,
    AdmissionObserved = 1,
    ResponseFormed = 2
}

public enum ProductEngramResponseDisposition
{
    ReceiptOnly = 0,
    Withheld = 1,
    Refused = 2
}

public sealed record EcDuplexTelemetryEventContract(
    string EventHandle,
    EcDuplexRoundtripEventKind EventKind,
    string AdmissionReceiptHandle,
    string CmeActualContractHandle,
    AnchorContinuityReceipt AnchorContinuityReceipt,
    NonActivationReceipt NonActivationReceipt,
    ReceiptContinuityReceipt ReceiptContinuityReceipt,
    string GovernanceTrace,
    bool RuntimeIdentityEmitted,
    bool RuntimeActionExecuted,
    DateTimeOffset TimestampUtc);

public sealed record ProductEngramResponseContract(
    string ResponseHandle,
    ProductEngramResponseDisposition Disposition,
    string TelemetryEventHandle,
    string ResponseSummary,
    IReadOnlyList<string> ReceiptRefs,
    AnchorContinuityReceipt AnchorContinuityReceipt,
    NonActivationReceipt NonActivationReceipt,
    ReceiptContinuityReceipt ReceiptContinuityReceipt,
    bool PublicationReady,
    bool RuntimeIdentityEmitted,
    bool RuntimeActionExecuted);

public static class EcDuplexRoundtripReceiptFactory
{
    public static EcDuplexTelemetryEventContract CreateReceiptOnlyTelemetryEvent(
        SliAdmissionReceipt admissionReceipt,
        CmeActualInstanceContract cmeActualContract,
        DateTimeOffset timestampUtc)
    {
        ArgumentNullException.ThrowIfNull(admissionReceipt);
        ArgumentNullException.ThrowIfNull(cmeActualContract);

        if (!admissionReceipt.AnchorContinuityReceipt.HasSameAnchorAs(cmeActualContract.AnchorContinuityReceipt))
        {
            throw new InvalidOperationException("EC telemetry requires preserved anchor continuity between SLI admission and CME.Actual contract.");
        }

        if (!admissionReceipt.NonActivationReceipt.HasSameInertnessAs(cmeActualContract.NonActivationReceipt))
        {
            throw new InvalidOperationException("EC telemetry requires preserved non-activation state between SLI admission and CME.Actual contract.");
        }

        if (!cmeActualContract.ReceiptContinuityReceipt.ExtendsReceipt(admissionReceipt.ReceiptContinuityReceipt))
        {
            throw new InvalidOperationException("EC telemetry requires CME.Actual receipt continuity to extend SLI admission continuity.");
        }

        if (admissionReceipt.NonActivationReceipt.HasPrematureActivation)
        {
            throw new InvalidOperationException("EC telemetry may only carry inert scaffold receipts.");
        }

        var eventHandle = $"ec-duplex-event://{Math.Abs(HashCode.Combine(admissionReceipt.ReceiptHandle, cmeActualContract.ContractHandle)):x}";
        var receiptContinuity = ReceiptContinuityReceipts.Extend(
            cmeActualContract.ReceiptContinuityReceipt,
            refKind: "ec-telemetry",
            refHandle: eventHandle,
            carrierRef: cmeActualContract.ContractHandle,
            continuityGate: "ec-duplex-receipt-continuity",
            anchorContinuityReceipt: cmeActualContract.AnchorContinuityReceipt,
            nonActivationReceipt: cmeActualContract.NonActivationReceipt,
            witnessRefs: [eventHandle]);

        return new EcDuplexTelemetryEventContract(
            EventHandle: eventHandle,
            EventKind: EcDuplexRoundtripEventKind.ScaffoldReceipt,
            AdmissionReceiptHandle: admissionReceipt.ReceiptHandle,
            CmeActualContractHandle: cmeActualContract.ContractHandle,
            AnchorContinuityReceipt: admissionReceipt.AnchorContinuityReceipt,
            NonActivationReceipt: admissionReceipt.NonActivationReceipt,
            ReceiptContinuityReceipt: receiptContinuity,
            GovernanceTrace: "ec-duplex-scaffold-records-receipt-only-non-activation",
            RuntimeIdentityEmitted: false,
            RuntimeActionExecuted: false,
            TimestampUtc: timestampUtc);
    }

    public static ProductEngramResponseContract CreateReceiptOnlyProductResponse(
        EcDuplexTelemetryEventContract telemetryEvent,
        IReadOnlyList<string> receiptRefs)
    {
        ArgumentNullException.ThrowIfNull(telemetryEvent);
        ArgumentNullException.ThrowIfNull(receiptRefs);

        if (!telemetryEvent.ReceiptContinuityReceipt.ContainsPassageRef("ec-telemetry", telemetryEvent.EventHandle))
        {
            throw new InvalidOperationException("product response requires telemetry receipt continuity before response formation.");
        }

        var responseHandle = $"product-engram-response://{Math.Abs(HashCode.Combine(telemetryEvent.EventHandle, receiptRefs.Count)):x}";
        var receiptContinuity = ReceiptContinuityReceipts.Extend(
            telemetryEvent.ReceiptContinuityReceipt,
            refKind: "product-engram-response",
            refHandle: responseHandle,
            carrierRef: telemetryEvent.EventHandle,
            continuityGate: "product-engram-response-receipt-continuity",
            anchorContinuityReceipt: telemetryEvent.AnchorContinuityReceipt,
            nonActivationReceipt: telemetryEvent.NonActivationReceipt,
            witnessRefs: receiptRefs);

        return new ProductEngramResponseContract(
            ResponseHandle: responseHandle,
            Disposition: ProductEngramResponseDisposition.ReceiptOnly,
            TelemetryEventHandle: telemetryEvent.EventHandle,
            ResponseSummary: "product engram response withheld to receipt-only scaffold posture",
            ReceiptRefs: receiptRefs.Concat([receiptContinuity.ReceiptHandle]).ToArray(),
            AnchorContinuityReceipt: telemetryEvent.AnchorContinuityReceipt,
            NonActivationReceipt: telemetryEvent.NonActivationReceipt,
            ReceiptContinuityReceipt: receiptContinuity,
            PublicationReady: false,
            RuntimeIdentityEmitted: false,
            RuntimeActionExecuted: false);
    }
}
