using System.Security.Cryptography;
using System.Text;
using SLI.Lisp;

namespace San.Sanctuary.Runtime;

public interface ISanctuaryEcTelemetryLoopService
{
    SanctuaryEcTelemetryLoopReceipt Run(
        SanctuaryEcTelemetryLoopRequest request,
        DateTimeOffset timestampUtc);
}

public sealed class DefaultSanctuaryEcTelemetryLoopService : ISanctuaryEcTelemetryLoopService
{
    private readonly ISliLispEcTelemetryLoopService sliLispEcTelemetryLoopService;

    public DefaultSanctuaryEcTelemetryLoopService()
        : this(new DefaultSliLispEcTelemetryLoopService())
    {
    }

    public DefaultSanctuaryEcTelemetryLoopService(ISliLispEcTelemetryLoopService sliLispEcTelemetryLoopService)
    {
        this.sliLispEcTelemetryLoopService = sliLispEcTelemetryLoopService;
    }

    public SanctuaryEcTelemetryLoopReceipt Run(
        SanctuaryEcTelemetryLoopRequest request,
        DateTimeOffset timestampUtc)
    {
        ArgumentNullException.ThrowIfNull(request);

        var installed = request.InstalledSubstrateReceipt;
        var lineRootPath = installed?.LineRootPath ?? string.Empty;
        var installRootPath = installed?.InstallRootPath ?? string.Empty;
        var receiptRootPath = string.IsNullOrWhiteSpace(installRootPath)
            ? string.Empty
            : Path.Combine(installRootPath, "receipts", "sanctuary-ec-loop");
        var receiptJsonPath = string.IsNullOrWhiteSpace(receiptRootPath)
            ? string.Empty
            : Path.Combine(receiptRootPath, "sanctuary-ec-loop.json");
        var receiptMarkdownPath = string.IsNullOrWhiteSpace(receiptRootPath)
            ? string.Empty
            : Path.Combine(receiptRootPath, "sanctuary-ec-loop.md");
        var thoughtForm = string.IsNullOrWhiteSpace(request.ThoughtForm)
            ? "idle cold EC telemetry loop"
            : request.ThoughtForm.Trim();

        if (request.RequestsRuntimeMotion)
        {
            var refusedReceipt = CreateReceipt(
                SanctuaryEcTelemetryLoopDisposition.Refused,
                "sanctuary-ec-loop-runtime-motion-refused",
                "Sanctuary EC telemetry loop refused before Lisp invocation because the host request attempted activation, model binding, arbitrary Lisp evaluation, runtime identity, runtime action, database write, GEL promotion, CME.Actual, or Sanctuary.Actual.",
                lineRootPath,
                installRootPath,
                receiptJsonPath,
                receiptMarkdownPath,
                thoughtForm,
                installed,
                sliLispEngineReceipt: null,
                timestampUtc);
            WriteReceiptIfPossible(refusedReceipt);
            return refusedReceipt;
        }

        if (installed is null || !installed.IsColdInstalledSubstrate)
        {
            var withheldReceipt = CreateReceipt(
                SanctuaryEcTelemetryLoopDisposition.Withheld,
                "sanctuary-ec-loop-installed-substrate-missing",
                "Sanctuary EC telemetry loop withheld because a cold installed Sanctuary substrate is required before SLI.Lisp-owned EC motion may be tested.",
                lineRootPath,
                installRootPath,
                receiptJsonPath,
                receiptMarkdownPath,
                thoughtForm,
                installed,
                sliLispEngineReceipt: null,
                timestampUtc);
            WriteReceiptIfPossible(withheldReceipt);
            return withheldReceipt;
        }

        var lispReceipt = sliLispEcTelemetryLoopService.Run(
            new SliLispEcTelemetryLoopRequest(
                ThoughtForm: thoughtForm,
                RuntimePath: request.SliLispRuntimePath),
            timestampUtc);

        var disposition = lispReceipt.IsColdEcTelemetryLoop
            ? SanctuaryEcTelemetryLoopDisposition.CompletedCold
            : SanctuaryEcTelemetryLoopDisposition.Withheld;
        var receipt = CreateReceipt(
            disposition,
            lispReceipt.IsColdEcTelemetryLoop
                ? "sanctuary-ec-loop-sli-lisp-engine-completed-cold"
                : "sanctuary-ec-loop-sli-lisp-engine-withheld",
            lispReceipt.IsColdEcTelemetryLoop
                ? "Sanctuary EC telemetry loop completed through the bounded SLI.Lisp EC entrypoint. C# hosted and receipted the run; Lisp owned the EC movement. Engram admission, memory admission, SelfGEL mutation, continuity, authority, action, model binding, arbitrary Lisp evaluation, CME.Actual, and Sanctuary.Actual remain refused."
                : "Sanctuary EC telemetry loop withheld because the bounded SLI.Lisp EC entrypoint did not complete cold.",
            lineRootPath,
            installRootPath,
            receiptJsonPath,
            receiptMarkdownPath,
            thoughtForm,
            installed,
            lispReceipt,
            timestampUtc);

        WriteReceiptIfPossible(receipt);
        return receipt;
    }

    private static SanctuaryEcTelemetryLoopReceipt CreateReceipt(
        SanctuaryEcTelemetryLoopDisposition disposition,
        string outcomeCode,
        string governanceTrace,
        string lineRootPath,
        string installRootPath,
        string receiptJsonPath,
        string receiptMarkdownPath,
        string thoughtForm,
        SanctuaryInstalledSubstrateReceipt? installed,
        SliLispEcTelemetryLoopReceipt? sliLispEngineReceipt,
        DateTimeOffset timestampUtc) =>
        new(
            ReceiptHandle: $"urn:san:ec-telemetry-loop:{ShortHash(installed?.ReceiptHandle ?? "none", thoughtForm, outcomeCode)}",
            Disposition: disposition,
            OutcomeCode: outcomeCode,
            GovernanceTrace: governanceTrace,
            LineRootPath: lineRootPath,
            InstallRootPath: installRootPath,
            ReceiptJsonPath: receiptJsonPath,
            ReceiptMarkdownPath: receiptMarkdownPath,
            ThoughtForm: thoughtForm,
            SourceInstalledSubstrateReceiptHandle: installed?.ReceiptHandle ?? string.Empty,
            SliLispEngineReceipt: sliLispEngineReceipt,
            ReviewOnly: true,
            SliLispOwnedEngineMotion: sliLispEngineReceipt?.IsColdEcTelemetryLoop == true,
            ColdEngineLoopCompleted: sliLispEngineReceipt?.ColdEngineLoopCompleted == true,
            ListeningFrameReceived: sliLispEngineReceipt?.ListeningFrameReceived == true,
            CompassOrientedPressure: sliLispEngineReceipt?.CompassOrientedPressure == true,
            ThinkingAboutThinkingTelemetryProduced: sliLispEngineReceipt?.ThinkingAboutThinkingTelemetryProduced == true,
            PreEngramResidueProduced: sliLispEngineReceipt?.PreEngramResidueProduced == true,
            PreEngramResidueCount: sliLispEngineReceipt?.PreEngramResidueCount ?? 0,
            StewardReviewed: sliLispEngineReceipt?.StewardReviewed == true,
            StreamAdmittedEngram: sliLispEngineReceipt?.EngramAdmissionAllowed == true,
            StreamAdmittedMemory: sliLispEngineReceipt?.MemoryAdmissionAllowed == true,
            SelfGelMutated: sliLispEngineReceipt?.SelfGelMutationAllowed == true,
            ContinuityAdmitted: sliLispEngineReceipt?.ContinuityAdmissionAllowed == true,
            AuthorityGranted: sliLispEngineReceipt?.AuthorityGranted == true,
            ActivationRefused: true,
            ModelBindingAllowed: sliLispEngineReceipt?.ModelBindingAllowed == true,
            ArbitraryLispEvaluationAllowed: sliLispEngineReceipt?.ArbitraryEvaluationAllowed == true,
            RuntimeIdentityAllowed: false,
            RuntimeActionAllowed: sliLispEngineReceipt?.RuntimeActionAllowed == true,
            DatabaseWriteAllowed: false,
            GelPromotionAllowed: false,
            CmeActualAllowed: sliLispEngineReceipt?.CmeActualActivationAllowed == true,
            SanctuaryActualAllowed: sliLispEngineReceipt?.SanctuaryActualActivationAllowed == true,
            TimestampUtc: timestampUtc);

    private static void WriteReceiptIfPossible(SanctuaryEcTelemetryLoopReceipt receipt)
    {
        if (string.IsNullOrWhiteSpace(receipt.ReceiptJsonPath) ||
            string.IsNullOrWhiteSpace(receipt.ReceiptMarkdownPath))
        {
            return;
        }

        Directory.CreateDirectory(Path.GetDirectoryName(receipt.ReceiptJsonPath) ?? receipt.InstallRootPath);
        File.WriteAllText(receipt.ReceiptJsonPath, SanctuaryEcTelemetryLoopReportWriter.ToJson(receipt), Encoding.UTF8);
        File.WriteAllText(receipt.ReceiptMarkdownPath, SanctuaryEcTelemetryLoopReportWriter.ToMarkdown(receipt), Encoding.UTF8);
    }

    private static string ShortHash(params string[] values)
    {
        var material = string.Join("|", values.Select(static value => value?.Trim() ?? string.Empty));
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(material));
        return Convert.ToHexString(hash).ToLowerInvariant()[..16];
    }
}
