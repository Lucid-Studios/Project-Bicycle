using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using SLI.Lisp;

namespace San.Sanctuary.Runtime;

public interface ISanctuaryTypedWarmUseRehearsalService
{
    SanctuaryTypedWarmUseRehearsalReceipt Run(
        SanctuaryTypedWarmUseRehearsalRequest request,
        DateTimeOffset timestampUtc);
}

public sealed class DefaultSanctuaryTypedWarmUseRehearsalService : ISanctuaryTypedWarmUseRehearsalService
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private readonly ISliLispTypedWarmUseRehearsalService sliLispWarmUseService;

    public DefaultSanctuaryTypedWarmUseRehearsalService()
        : this(new DefaultSliLispTypedWarmUseRehearsalService())
    {
    }

    public DefaultSanctuaryTypedWarmUseRehearsalService(ISliLispTypedWarmUseRehearsalService sliLispWarmUseService)
    {
        this.sliLispWarmUseService = sliLispWarmUseService;
    }

    public SanctuaryTypedWarmUseRehearsalReceipt Run(
        SanctuaryTypedWarmUseRehearsalRequest request,
        DateTimeOffset timestampUtc)
    {
        ArgumentNullException.ThrowIfNull(request);

        var installed = request.InstalledSubstrateReceipt;
        var lineRootPath = installed?.LineRootPath ?? string.Empty;
        var installRootPath = installed?.InstallRootPath ?? string.Empty;
        var rootIdentity = installed?.RootIdentity;
        var sessionId = NormalizeIdentitySegment(request.SessionId, "warm-use-session");
        var turnIndex = Math.Max(0, request.TurnIndex);
        var thoughtForm = string.IsNullOrWhiteSpace(request.ThoughtForm)
            ? "idle typed warm-use rehearsal"
            : request.ThoughtForm.Trim();
        var receiptRootPath = string.IsNullOrWhiteSpace(installRootPath)
            ? string.Empty
            : Path.Combine(installRootPath, "receipts", "sanctuary-warm-use", sessionId);
        var receiptJsonPath = string.IsNullOrWhiteSpace(receiptRootPath)
            ? string.Empty
            : Path.Combine(receiptRootPath, $"turn-{turnIndex:0000}.json");
        var receiptMarkdownPath = string.IsNullOrWhiteSpace(receiptRootPath)
            ? string.Empty
            : Path.Combine(receiptRootPath, $"turn-{turnIndex:0000}.md");
        var sessionLedgerPath = string.IsNullOrWhiteSpace(receiptRootPath)
            ? string.Empty
            : Path.Combine(receiptRootPath, "session.jsonl");
        var sessionSummaryPath = string.IsNullOrWhiteSpace(receiptRootPath)
            ? string.Empty
            : Path.Combine(receiptRootPath, "session-summary.json");

        if (request.RequestsRuntimeMotion)
        {
            var refusedReceipt = CreateReceipt(
                SanctuaryTypedWarmUseRehearsalDisposition.Refused,
                "sanctuary-typed-warm-use-runtime-motion-refused",
                "Sanctuary typed warm-use rehearsal refused before Lisp invocation because the host request attempted activation, model binding, arbitrary Lisp evaluation, runtime identity, runtime action, database write, GEL promotion, CME.Actual, or Sanctuary.Actual.",
                lineRootPath,
                installRootPath,
                receiptJsonPath,
                receiptMarkdownPath,
                sessionLedgerPath,
                sessionSummaryPath,
                request.PriorTurnReceiptHandle,
                rootIdentity,
                installed,
                sessionId,
                turnIndex,
                thoughtForm,
                sliLispWarmUseReceipt: null,
                timestampUtc);
            WriteReceiptAndLedgerIfPossible(refusedReceipt);
            return refusedReceipt;
        }

        if (installed is null || rootIdentity is null || !installed.IsColdInstalledSubstrate)
        {
            var withheldReceipt = CreateReceipt(
                SanctuaryTypedWarmUseRehearsalDisposition.Withheld,
                "sanctuary-typed-warm-use-installed-substrate-missing",
                "Sanctuary typed warm-use rehearsal withheld because a cold installed Sanctuary substrate is required before live scoped rehearsal may be tested.",
                lineRootPath,
                installRootPath,
                receiptJsonPath,
                receiptMarkdownPath,
                sessionLedgerPath,
                sessionSummaryPath,
                request.PriorTurnReceiptHandle,
                rootIdentity,
                installed,
                sessionId,
                turnIndex,
                thoughtForm,
                sliLispWarmUseReceipt: null,
                timestampUtc);
            WriteReceiptAndLedgerIfPossible(withheldReceipt);
            return withheldReceipt;
        }

        var lispReceipt = sliLispWarmUseService.Run(
            new SliLispTypedWarmUseRehearsalRequest(
                OperatorId: rootIdentity.OperatorId,
                Domain: rootIdentity.Domain,
                Role: rootIdentity.Role,
                JobClass: rootIdentity.JobClass,
                SessionId: sessionId,
                TurnIndex: turnIndex,
                ThoughtForm: thoughtForm,
                RuntimePath: request.SliLispRuntimePath),
            timestampUtc);

        var completed = lispReceipt.IsTypedWarmUseRehearsal;
        var receipt = CreateReceipt(
            completed
                ? SanctuaryTypedWarmUseRehearsalDisposition.CompletedCold
                : SanctuaryTypedWarmUseRehearsalDisposition.Withheld,
            completed
                ? "sanctuary-typed-warm-use-sli-lisp-rehearsal-completed-cold"
                : "sanctuary-typed-warm-use-sli-lisp-rehearsal-withheld",
            completed
                ? "Sanctuary typed warm-use rehearsal accepted live scoped thought-form material through the bounded SLI.Lisp entrypoint and returned receipt-only. The session ledger may append witness records; memory admission, SelfGEL mutation, continuity, authority, action, model binding, arbitrary Lisp evaluation, CME.Actual, and Sanctuary.Actual remain refused."
                : "Sanctuary typed warm-use rehearsal withheld because the bounded SLI.Lisp typed warm-use entrypoint did not complete cold.",
            lineRootPath,
            installRootPath,
            receiptJsonPath,
            receiptMarkdownPath,
            sessionLedgerPath,
            sessionSummaryPath,
            request.PriorTurnReceiptHandle,
            rootIdentity,
            installed,
            sessionId,
            turnIndex,
            thoughtForm,
            lispReceipt,
            timestampUtc);

        WriteReceiptAndLedgerIfPossible(receipt);
        return receipt;
    }

    private static SanctuaryTypedWarmUseRehearsalReceipt CreateReceipt(
        SanctuaryTypedWarmUseRehearsalDisposition disposition,
        string outcomeCode,
        string governanceTrace,
        string lineRootPath,
        string installRootPath,
        string receiptJsonPath,
        string receiptMarkdownPath,
        string sessionLedgerPath,
        string sessionSummaryPath,
        string? priorTurnReceiptHandle,
        SanctuaryRootIdentityRecord? rootIdentity,
        SanctuaryInstalledSubstrateReceipt? installed,
        string sessionId,
        int turnIndex,
        string thoughtForm,
        SliLispTypedWarmUseRehearsalReceipt? sliLispWarmUseReceipt,
        DateTimeOffset timestampUtc)
    {
        var operatorId = sliLispWarmUseReceipt?.OperatorId ?? rootIdentity?.OperatorId ?? string.Empty;
        var domain = sliLispWarmUseReceipt?.Domain ?? rootIdentity?.Domain ?? string.Empty;
        var role = sliLispWarmUseReceipt?.Role ?? rootIdentity?.Role ?? string.Empty;
        var jobClass = sliLispWarmUseReceipt?.JobClass ?? rootIdentity?.JobClass ?? string.Empty;

        return new SanctuaryTypedWarmUseRehearsalReceipt(
            ReceiptHandle: $"urn:san:typed-warm-use:{ShortHash(installed?.ReceiptHandle ?? "none", sessionId, turnIndex.ToString(System.Globalization.CultureInfo.InvariantCulture), thoughtForm, outcomeCode)}",
            Disposition: disposition,
            OutcomeCode: outcomeCode,
            GovernanceTrace: governanceTrace,
            LineRootPath: lineRootPath,
            InstallRootPath: installRootPath,
            ReceiptJsonPath: receiptJsonPath,
            ReceiptMarkdownPath: receiptMarkdownPath,
            SessionLedgerPath: sessionLedgerPath,
            SessionSummaryPath: sessionSummaryPath,
            SourceInstalledSubstrateReceiptHandle: installed?.ReceiptHandle ?? string.Empty,
            PriorTurnReceiptHandle: priorTurnReceiptHandle?.Trim() ?? string.Empty,
            OperatorId: operatorId,
            Domain: domain,
            Role: role,
            JobClass: jobClass,
            SessionId: sliLispWarmUseReceipt?.SessionId ?? sessionId,
            TurnIndex: sliLispWarmUseReceipt?.TurnIndex ?? turnIndex,
            ThoughtForm: thoughtForm,
            SliLispWarmUseReceipt: sliLispWarmUseReceipt,
            ReviewOnly: true,
            SliLispOwnedEngineMotion: sliLispWarmUseReceipt?.IsTypedWarmUseRehearsal == true,
            TypedScopeAccepted: sliLispWarmUseReceipt?.TypedScopeAccepted == true,
            LiveIngressAcceptedCold: sliLispWarmUseReceipt?.LiveIngressAcceptedCold == true,
            SessionLineageWitnessed: sliLispWarmUseReceipt?.SessionLineageWitnessed == true,
            ListeningFrameReceived: sliLispWarmUseReceipt?.ListeningFrameReceived == true,
            CompassOrientedPressure: sliLispWarmUseReceipt?.CompassOrientedPressure == true,
            ThinkingAboutThinkingTelemetryProduced: sliLispWarmUseReceipt?.ThinkingAboutThinkingTelemetryProduced == true,
            PreEngramResidueProduced: sliLispWarmUseReceipt?.PreEngramResidueProduced == true,
            PreEngramResidueCount: sliLispWarmUseReceipt?.PreEngramResidueCount ?? 0,
            StewardReviewed: sliLispWarmUseReceipt?.StewardReviewed == true,
            TurnLineageReceiptOnly: sliLispWarmUseReceipt?.TurnLineageReceiptOnly == true,
            SessionLedgerAppendOnly: sliLispWarmUseReceipt?.SessionLedgerAppendOnly == true,
            StreamAdmittedEngram: sliLispWarmUseReceipt?.EngramAdmissionAllowed == true,
            StreamAdmittedMemory: sliLispWarmUseReceipt?.MemoryAdmissionAllowed == true,
            SelfGelMutated: sliLispWarmUseReceipt?.SelfGelMutationAllowed == true,
            ContinuityAdmitted: sliLispWarmUseReceipt?.ContinuityAdmissionAllowed == true,
            AuthorityGranted: sliLispWarmUseReceipt?.AuthorityGranted == true,
            ActivationRefused: true,
            ModelBindingAllowed: sliLispWarmUseReceipt?.ModelBindingAllowed == true,
            ArbitraryLispEvaluationAllowed: sliLispWarmUseReceipt?.ArbitraryEvaluationAllowed == true,
            RuntimeIdentityAllowed: false,
            RuntimeActionAllowed: sliLispWarmUseReceipt?.RuntimeActionAllowed == true,
            DatabaseWriteAllowed: false,
            GelPromotionAllowed: false,
            CmeActualAllowed: sliLispWarmUseReceipt?.CmeActualActivationAllowed == true,
            SanctuaryActualAllowed: sliLispWarmUseReceipt?.SanctuaryActualActivationAllowed == true,
            TimestampUtc: timestampUtc);
    }

    private static void WriteReceiptAndLedgerIfPossible(SanctuaryTypedWarmUseRehearsalReceipt receipt)
    {
        if (string.IsNullOrWhiteSpace(receipt.ReceiptJsonPath) ||
            string.IsNullOrWhiteSpace(receipt.ReceiptMarkdownPath))
        {
            return;
        }

        Directory.CreateDirectory(Path.GetDirectoryName(receipt.ReceiptJsonPath) ?? receipt.InstallRootPath);
        File.WriteAllText(receipt.ReceiptJsonPath, SanctuaryTypedWarmUseRehearsalReportWriter.ToJson(receipt), Encoding.UTF8);
        File.WriteAllText(receipt.ReceiptMarkdownPath, SanctuaryTypedWarmUseRehearsalReportWriter.ToMarkdown(receipt), Encoding.UTF8);

        if (!string.IsNullOrWhiteSpace(receipt.SessionLedgerPath))
        {
            var ledgerRecord = new
            {
                receipt.TimestampUtc,
                receipt.ReceiptHandle,
                receipt.Disposition,
                receipt.OutcomeCode,
                receipt.OperatorId,
                receipt.Domain,
                receipt.Role,
                receipt.JobClass,
                receipt.SessionId,
                receipt.TurnIndex,
                receipt.PriorTurnReceiptHandle,
                receipt.SliLispOwnedEngineMotion,
                receipt.TypedScopeAccepted,
                receipt.LiveIngressAcceptedCold,
                receipt.PreEngramResidueCount,
                receipt.StewardReviewed,
                receipt.AuthorityGranted,
                receipt.ContinuityAdmitted,
                receipt.RuntimeActionAllowed,
                receipt.CmeActualAllowed,
                receipt.SanctuaryActualAllowed
            };
            File.AppendAllText(receipt.SessionLedgerPath, JsonSerializer.Serialize(ledgerRecord, JsonOptions) + Environment.NewLine, Encoding.UTF8);
        }

        if (!string.IsNullOrWhiteSpace(receipt.SessionSummaryPath))
        {
            var summary = new
            {
                receipt.SessionId,
                LastReceiptHandle = receipt.ReceiptHandle,
                receipt.TurnIndex,
                receipt.Disposition,
                receipt.OutcomeCode,
                receipt.IsTypedColdReadyWarmUse,
                receipt.AuthorityGranted,
                receipt.ContinuityAdmitted,
                receipt.RuntimeActionAllowed,
                receipt.CmeActualAllowed,
                receipt.SanctuaryActualAllowed,
                receipt.TimestampUtc
            };
            File.WriteAllText(receipt.SessionSummaryPath, JsonSerializer.Serialize(summary, JsonOptions), Encoding.UTF8);
        }
    }

    private static string NormalizeIdentitySegment(string value, string fallback)
    {
        var source = string.IsNullOrWhiteSpace(value) ? fallback : value.Trim();
        var builder = new StringBuilder();
        foreach (var ch in source)
        {
            if (char.IsLetterOrDigit(ch) || ch is '-' or '_' or '.')
            {
                builder.Append(ch);
            }
        }

        return builder.Length == 0 ? fallback : builder.ToString();
    }

    private static string ShortHash(params string[] values)
    {
        var material = string.Join("|", values.Select(static value => value?.Trim() ?? string.Empty));
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(material));
        return Convert.ToHexString(hash).ToLowerInvariant()[..16];
    }
}
