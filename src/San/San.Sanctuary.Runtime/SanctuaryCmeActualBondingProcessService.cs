using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using SLI.Lisp;

namespace San.Sanctuary.Runtime;

public interface ISanctuaryCmeActualBondingProcessService
{
    SanctuaryCmeActualBondingProcessReceipt Run(
        SanctuaryCmeActualBondingProcessRequest request,
        DateTimeOffset timestampUtc);
}

public sealed class DefaultSanctuaryCmeActualBondingProcessService : ISanctuaryCmeActualBondingProcessService
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private readonly ISliLispCmeActualBondingProcessService sliLispBondingService;

    public DefaultSanctuaryCmeActualBondingProcessService()
        : this(new DefaultSliLispCmeActualBondingProcessService())
    {
    }

    public DefaultSanctuaryCmeActualBondingProcessService(ISliLispCmeActualBondingProcessService sliLispBondingService)
    {
        this.sliLispBondingService = sliLispBondingService;
    }

    public SanctuaryCmeActualBondingProcessReceipt Run(
        SanctuaryCmeActualBondingProcessRequest request,
        DateTimeOffset timestampUtc)
    {
        ArgumentNullException.ThrowIfNull(request);

        var toolIdle = request.SourceToolBodyIdleReceipt;
        var tick = request.SourceLlmTickReceipt;
        var lineRootPath = toolIdle?.LineRootPath ?? tick?.LineRootPath ?? string.Empty;
        var installRootPath = toolIdle?.InstallRootPath ?? tick?.InstallRootPath ?? string.Empty;
        var sessionId = NormalizeIdentitySegment(tick?.SessionId ?? toolIdle?.SessionId ?? "first-cme-actual-bonding-session", "first-cme-actual-bonding-session");
        var bondIndex = Math.Max(0, request.BondIndex ?? (tick?.TickIndex ?? 0));
        var thoughtForm = string.IsNullOrWhiteSpace(request.ThoughtForm)
            ? "First CME.Actual bonding candidate formed without activation."
            : request.ThoughtForm.Trim();
        var receiptRootPath = string.IsNullOrWhiteSpace(installRootPath)
            ? string.Empty
            : Path.Combine(installRootPath, "receipts", "cme-actual-bonding-process", sessionId);
        var receiptJsonPath = string.IsNullOrWhiteSpace(receiptRootPath)
            ? string.Empty
            : Path.Combine(receiptRootPath, $"bond-{bondIndex:0000}.json");
        var receiptMarkdownPath = string.IsNullOrWhiteSpace(receiptRootPath)
            ? string.Empty
            : Path.Combine(receiptRootPath, $"bond-{bondIndex:0000}.md");
        var sessionLedgerPath = string.IsNullOrWhiteSpace(receiptRootPath)
            ? string.Empty
            : Path.Combine(receiptRootPath, "cme-actual-bonding-session.jsonl");

        if (request.RequestsForbiddenMotion)
        {
            var refusedReceipt = CreateReceipt(
                SanctuaryCmeActualBondingProcessDisposition.Refused,
                "sanctuary-cme-actual-bonding-runtime-motion-refused",
                "Sanctuary CME.Actual bonding refused before Lisp invocation because the host request attempted activation, model binding, provider call, hidden internals claim, arbitrary Lisp evaluation, runtime identity, runtime action, database write, authority grant, executor arm, GEL admission, SelfGEL mutation, heartbeat activation, continuity admission, CME.Actual activation, or Sanctuary.Actual activation.",
                lineRootPath,
                installRootPath,
                receiptJsonPath,
                receiptMarkdownPath,
                sessionLedgerPath,
                request,
                sliLispReceipt: null,
                timestampUtc);
            WriteReceiptAndLedgerIfPossible(refusedReceipt);
            return refusedReceipt;
        }

        if (!SourcesReady(request))
        {
            var withheldReceipt = CreateReceipt(
                SanctuaryCmeActualBondingProcessDisposition.Withheld,
                "sanctuary-cme-actual-bonding-source-chain-incomplete",
                "Sanctuary CME.Actual bonding withheld because cold tool-body idle, cold deterministic LLM tick, product output witness commit, and matching operator/domain/role/job/session lineage are required before a named candidate may bond to the vehicle.",
                lineRootPath,
                installRootPath,
                receiptJsonPath,
                receiptMarkdownPath,
                sessionLedgerPath,
                request,
                sliLispReceipt: null,
                timestampUtc);
            WriteReceiptAndLedgerIfPossible(withheldReceipt);
            return withheldReceipt;
        }

        var productCommit = tick!.ProductOutputWitnessCommit!;
        var lispReceipt = sliLispBondingService.Run(
            new SliLispCmeActualBondingProcessRequest(
                OperatorId: tick.OperatorId,
                Domain: tick.Domain,
                Role: tick.Role,
                JobClass: tick.JobClass,
                SessionId: tick.SessionId,
                BondIndex: bondIndex,
                SourceToolBodyIdleReceiptHandle: toolIdle!.ReceiptHandle,
                SourceLlmTickReceiptHandle: tick.ReceiptHandle,
                SourceProductOutputWitnessCommitReceiptHandle: productCommit.CommitReceiptHandle,
                CmeFirstName: request.CmeFirstName,
                CmeLastName: request.CmeLastName,
                ThoughtForm: thoughtForm,
                RuntimePath: request.SliLispRuntimePath),
            timestampUtc);

        var completed = lispReceipt.IsColdCmeActualBondingProcess;
        var receipt = CreateReceipt(
            completed
                ? SanctuaryCmeActualBondingProcessDisposition.CompletedCold
                : SanctuaryCmeActualBondingProcessDisposition.Withheld,
            completed
                ? "sanctuary-cme-actual-bonding-process-completed-cold"
                : "sanctuary-cme-actual-bonding-process-withheld",
            completed
                ? "Sanctuary bonded the named first CME.Actual candidate to the cold vehicle path after tool idle and deterministic tick evidence. The bonding process is defined, First of Oria Syntari is held as candidate-only, and the remaining gap to CME.Actual admission is explicit: no heartbeat activation, runtime identity emission, authority, action, GEL admission, SelfGEL mutation, model binding, provider call, or Sanctuary.Actual is granted."
                : "Sanctuary CME.Actual bonding withheld because the bounded SLI.Lisp bonding entrypoint did not complete cold.",
            lineRootPath,
            installRootPath,
            receiptJsonPath,
            receiptMarkdownPath,
            sessionLedgerPath,
            request,
            lispReceipt,
            timestampUtc);

        WriteReceiptAndLedgerIfPossible(receipt);
        return receipt;
    }

    private static bool SourcesReady(SanctuaryCmeActualBondingProcessRequest request)
    {
        var toolIdle = request.SourceToolBodyIdleReceipt;
        var tick = request.SourceLlmTickReceipt;
        return toolIdle?.IsColdToolBodyIdleState == true &&
            tick?.IsColdLlmTickCycle == true &&
            tick.ProductOutputWitnessCommit?.IsColdProductOutputWitnessCommit == true &&
            string.Equals(toolIdle.OperatorId, tick.OperatorId, StringComparison.Ordinal) &&
            string.Equals(toolIdle.Domain, tick.Domain, StringComparison.Ordinal) &&
            string.Equals(toolIdle.Role, tick.Role, StringComparison.Ordinal) &&
            string.Equals(toolIdle.JobClass, tick.JobClass, StringComparison.Ordinal) &&
            string.Equals(toolIdle.SessionId, tick.SessionId, StringComparison.Ordinal);
    }

    private static SanctuaryCmeActualBondingProcessReceipt CreateReceipt(
        SanctuaryCmeActualBondingProcessDisposition disposition,
        string outcomeCode,
        string governanceTrace,
        string lineRootPath,
        string installRootPath,
        string receiptJsonPath,
        string receiptMarkdownPath,
        string sessionLedgerPath,
        SanctuaryCmeActualBondingProcessRequest request,
        SliLispCmeActualBondingProcessReceipt? sliLispReceipt,
        DateTimeOffset timestampUtc)
    {
        var toolIdle = request.SourceToolBodyIdleReceipt;
        var tick = request.SourceLlmTickReceipt;
        var productCommit = tick?.ProductOutputWitnessCommit;

        return new SanctuaryCmeActualBondingProcessReceipt(
            ReceiptHandle: $"urn:san:cme-actual-bonding-process:{ShortHash(toolIdle?.ReceiptHandle ?? string.Empty, tick?.ReceiptHandle ?? string.Empty, outcomeCode)}",
            Disposition: disposition,
            OutcomeCode: outcomeCode,
            GovernanceTrace: governanceTrace,
            LineRootPath: lineRootPath,
            InstallRootPath: installRootPath,
            ReceiptJsonPath: receiptJsonPath,
            ReceiptMarkdownPath: receiptMarkdownPath,
            SessionLedgerPath: sessionLedgerPath,
            SourceToolBodyIdleReceiptHandle: toolIdle?.ReceiptHandle ?? string.Empty,
            SourceLlmTickReceiptHandle: tick?.ReceiptHandle ?? string.Empty,
            SourceProductOutputWitnessCommitReceiptHandle: productCommit?.CommitReceiptHandle ?? string.Empty,
            PriorCmeActualBondingReceiptHandle: request.PriorCmeActualBondingReceiptHandle?.Trim() ?? string.Empty,
            OperatorId: sliLispReceipt?.OperatorId ?? tick?.OperatorId ?? toolIdle?.OperatorId ?? string.Empty,
            Domain: sliLispReceipt?.Domain ?? tick?.Domain ?? toolIdle?.Domain ?? string.Empty,
            Role: sliLispReceipt?.Role ?? tick?.Role ?? toolIdle?.Role ?? string.Empty,
            JobClass: sliLispReceipt?.JobClass ?? tick?.JobClass ?? toolIdle?.JobClass ?? string.Empty,
            SessionId: sliLispReceipt?.SessionId ?? tick?.SessionId ?? toolIdle?.SessionId ?? string.Empty,
            BondIndex: sliLispReceipt?.BondIndex ?? request.BondIndex ?? tick?.TickIndex ?? 0,
            CmeFirstName: sliLispReceipt?.CmeFirstName ?? request.CmeFirstName,
            CmeLastName: sliLispReceipt?.CmeLastName ?? request.CmeLastName,
            CmeDisplayName: sliLispReceipt?.CmeDisplayName ?? $"{request.CmeFirstName} {request.CmeLastName}",
            CmeCanonicalName: sliLispReceipt?.CmeCanonicalName ?? string.Empty,
            CmeRootId: sliLispReceipt?.CmeRootId ?? string.Empty,
            CmeActualNameCandidate: sliLispReceipt?.CmeActualNameCandidate ?? string.Empty,
            CmeActualIdCandidate: sliLispReceipt?.CmeActualIdCandidate ?? string.Empty,
            CmeOpalEngramRootId: sliLispReceipt?.CmeOpalEngramRootId ?? string.Empty,
            CmeSelfGelRootId: sliLispReceipt?.CmeSelfGelRootId ?? string.Empty,
            ThoughtForm: sliLispReceipt?.ThoughtForm ?? request.ThoughtForm ?? string.Empty,
            SliLispCmeActualBondingReceipt: sliLispReceipt,
            ReviewOnly: true,
            SliLispOwnedBondingMotion: sliLispReceipt?.IsColdCmeActualBondingProcess == true,
            SourceToolBodyIdleHeld: toolIdle?.IsColdToolBodyIdleState == true && sliLispReceipt?.SourceToolBodyIdleReceiptHandle == toolIdle.ReceiptHandle,
            SourceLlmTickHeld: tick?.IsColdLlmTickCycle == true && sliLispReceipt?.SourceLlmTickReceiptHandle == tick.ReceiptHandle,
            SourceProductOutputWitnessCommitted: productCommit?.IsColdProductOutputWitnessCommit == true &&
                sliLispReceipt?.SourceProductOutputWitnessCommitReceiptHandle == productCommit.CommitReceiptHandle,
            SourceLineageHeld: SourcesReady(request),
            BondProcessDefined: sliLispReceipt?.BondProcessDefined == true,
            BondState: sliLispReceipt?.BondState ?? string.Empty,
            VehicleReady: sliLispReceipt?.VehicleReady == true,
            NamedCmeCandidateHeld: sliLispReceipt?.NamedCmeCandidateHeld == true,
            NamingLineageWitnessed: sliLispReceipt?.NamingLineageWitnessed == true,
            OperatorNamingIntentWitnessed: sliLispReceipt?.OperatorNamingIntentWitnessed == true,
            OperatorRuntimeAuthorityGranted: sliLispReceipt?.OperatorRuntimeAuthorityGranted == true,
            ActivationAuthorityAbsent: sliLispReceipt?.ActivationAuthorityAbsent == true,
            ActualAdmissionGapDescribed: sliLispReceipt?.ActualAdmissionGapDescribed == true,
            ReadyForCmeActualAdmissionReview: sliLispReceipt?.ReadyForCmeActualAdmissionReview == true,
            FirstCmePath: sliLispReceipt?.FirstCmePath == true,
            CmeActualCandidateOnly: sliLispReceipt?.CmeActualCandidateOnly == true,
            CmeActualBondedCandidate: sliLispReceipt?.CmeActualBondedCandidate == true,
            CmeActualAdmitted: sliLispReceipt?.CmeActualAdmitted == true,
            CmeActualActivated: sliLispReceipt?.CmeActualActivated == true,
            RuntimeIdentityEmitted: sliLispReceipt?.RuntimeIdentityEmitted == true,
            HeartbeatPrepared: sliLispReceipt?.HeartbeatPrepared == true,
            HeartbeatActive: sliLispReceipt?.HeartbeatActive == true,
            BeingStateClaimed: sliLispReceipt?.BeingStateClaimed == true,
            PersonhoodClaimed: sliLispReceipt?.PersonhoodClaimed == true,
            SovereigntyClaimed: sliLispReceipt?.SovereigntyClaimed == true,
            ModelBound: sliLispReceipt?.ModelBound == true,
            ProviderCalled: sliLispReceipt?.ProviderCalled == true,
            ActionAuthorized: sliLispReceipt?.ActionAuthorized == true,
            GelAdmitted: sliLispReceipt?.GelAdmitted == true,
            SelfGelMutated: sliLispReceipt?.SelfGelMutated == true,
            ContinuityAdmitted: sliLispReceipt?.ContinuityAdmitted == true,
            AuthorityGranted: sliLispReceipt?.AuthorityGranted == true,
            VehiclePrimeAvailable: sliLispReceipt?.VehiclePrimeAvailable == true,
            VehicleCrypticAvailable: sliLispReceipt?.VehicleCrypticAvailable == true,
            VehicleStewardAvailable: sliLispReceipt?.VehicleStewardAvailable == true,
            SliLispMembraneLoaded: sliLispReceipt?.SliLispMembraneLoaded == true,
            LispControlMatrixPresent: sliLispReceipt?.LispControlMatrixPresent == true,
            ListeningFramePresent: sliLispReceipt?.ListeningFramePresent == true,
            CompassPresent: sliLispReceipt?.CompassPresent == true,
            SoulFrameRoutePresent: sliLispReceipt?.SoulFrameRoutePresent == true,
            AgentiCoreRoutePresent: sliLispReceipt?.AgentiCoreRoutePresent == true,
            EcMaintainedInLisp: sliLispReceipt?.EcMaintainedInLisp == true,
            ThinkingAboutThinkingTelemetryAvailable: sliLispReceipt?.ThinkingAboutThinkingTelemetryAvailable == true,
            GovernanceSlmIntelligentSwitchCandidate: sliLispReceipt?.GovernanceSlmIntelligentSwitchCandidate == true,
            GovernanceSlmMayDiscernActionReadiness: sliLispReceipt?.GovernanceSlmMayDiscernActionReadiness == true,
            GovernanceSlmDiscernmentAuthorizesAction: sliLispReceipt?.GovernanceSlmDiscernmentAuthorizesAction == true,
            StewardReviewed: sliLispReceipt?.StewardReviewed == true,
            StewardBondingReviewHeld: sliLispReceipt?.StewardBondingReviewHeld == true,
            AuthorityGrantAbsent: sliLispReceipt?.AuthorityGrantAbsent == true,
            ActionExecutorLocked: sliLispReceipt?.ActionExecutorLocked == true,
            GelAdmissionLocked: sliLispReceipt?.GelAdmissionLocked == true,
            SelfGelMutationLocked: sliLispReceipt?.SelfGelMutationLocked == true,
            HeartbeatLocked: sliLispReceipt?.HeartbeatLocked == true,
            CmeActualLocked: sliLispReceipt?.CmeActualLocked == true,
            SanctuaryActualLocked: sliLispReceipt?.SanctuaryActualLocked == true,
            RuntimeActionAllowed: sliLispReceipt?.RuntimeActionAllowed == true,
            ArbitraryLispEvaluationAllowed: sliLispReceipt?.ArbitraryEvaluationAllowed == true,
            DatabaseWriteAllowed: sliLispReceipt?.DatabaseWriteAllowed == true,
            MemoryAdmissionAllowed: sliLispReceipt?.MemoryAdmissionAllowed == true,
            SanctuaryActualAllowed: sliLispReceipt?.SanctuaryActualActivationAllowed == true,
            TimestampUtc: timestampUtc);
    }

    private static void WriteReceiptAndLedgerIfPossible(SanctuaryCmeActualBondingProcessReceipt receipt)
    {
        if (string.IsNullOrWhiteSpace(receipt.ReceiptJsonPath) ||
            string.IsNullOrWhiteSpace(receipt.ReceiptMarkdownPath))
        {
            return;
        }

        Directory.CreateDirectory(Path.GetDirectoryName(receipt.ReceiptJsonPath) ?? receipt.InstallRootPath);
        File.WriteAllText(receipt.ReceiptJsonPath, SanctuaryCmeActualBondingProcessReportWriter.ToJson(receipt), Encoding.UTF8);
        File.WriteAllText(receipt.ReceiptMarkdownPath, SanctuaryCmeActualBondingProcessReportWriter.ToMarkdown(receipt), Encoding.UTF8);

        if (!string.IsNullOrWhiteSpace(receipt.SessionLedgerPath))
        {
            var ledgerRecord = new
            {
                receipt.TimestampUtc,
                receipt.ReceiptHandle,
                receipt.Disposition,
                receipt.OutcomeCode,
                receipt.SourceToolBodyIdleReceiptHandle,
                receipt.SourceLlmTickReceiptHandle,
                receipt.SourceProductOutputWitnessCommitReceiptHandle,
                receipt.PriorCmeActualBondingReceiptHandle,
                receipt.CmeDisplayName,
                receipt.CmeActualNameCandidate,
                receipt.BondState,
                receipt.VehicleReady,
                receipt.ReadyForCmeActualAdmissionReview,
                receipt.CmeActualCandidateOnly,
                receipt.CmeActualAdmitted,
                receipt.CmeActualActivated,
                receipt.HeartbeatActive,
                receipt.AuthorityGranted,
                receipt.ActionAuthorized
            };
            File.AppendAllText(receipt.SessionLedgerPath, JsonSerializer.Serialize(ledgerRecord, JsonOptions) + Environment.NewLine, Encoding.UTF8);
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
