using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using SLI.Lisp;

namespace San.Sanctuary.Runtime;

public interface ISanctuaryAgentEngineIdleReadinessService
{
    SanctuaryAgentEngineIdleReadinessReceipt Run(
        SanctuaryAgentEngineIdleReadinessRequest request,
        DateTimeOffset timestampUtc);
}

public sealed class DefaultSanctuaryAgentEngineIdleReadinessService : ISanctuaryAgentEngineIdleReadinessService
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private readonly ISliLispAgentEngineIdleReadinessService sliLispAgentEngineService;

    public DefaultSanctuaryAgentEngineIdleReadinessService()
        : this(new DefaultSliLispAgentEngineIdleReadinessService())
    {
    }

    public DefaultSanctuaryAgentEngineIdleReadinessService(ISliLispAgentEngineIdleReadinessService sliLispAgentEngineService)
    {
        this.sliLispAgentEngineService = sliLispAgentEngineService;
    }

    public SanctuaryAgentEngineIdleReadinessReceipt Run(
        SanctuaryAgentEngineIdleReadinessRequest request,
        DateTimeOffset timestampUtc)
    {
        ArgumentNullException.ThrowIfNull(request);

        var source = request.SourceLabGelReceipt;
        var lineRootPath = source?.LineRootPath ?? string.Empty;
        var installRootPath = source?.InstallRootPath ?? string.Empty;
        var sessionId = NormalizeIdentitySegment(source?.SessionId ?? "agent-engine-idle-session", "agent-engine-idle-session");
        var turnIndex = Math.Max(0, source?.TurnIndex ?? 0);
        var thoughtForm = string.IsNullOrWhiteSpace(source?.ThoughtForm)
            ? "idle provider-neutral engine LLM seat readiness"
            : source.ThoughtForm.Trim();
        var receiptRootPath = string.IsNullOrWhiteSpace(installRootPath)
            ? string.Empty
            : Path.Combine(installRootPath, "receipts", "agent-engine-idle-readiness", sessionId);
        var receiptJsonPath = string.IsNullOrWhiteSpace(receiptRootPath)
            ? string.Empty
            : Path.Combine(receiptRootPath, $"turn-{turnIndex:0000}.json");
        var receiptMarkdownPath = string.IsNullOrWhiteSpace(receiptRootPath)
            ? string.Empty
            : Path.Combine(receiptRootPath, $"turn-{turnIndex:0000}.md");
        var sessionLedgerPath = string.IsNullOrWhiteSpace(receiptRootPath)
            ? string.Empty
            : Path.Combine(receiptRootPath, "agent-engine-idle-session.jsonl");

        if (request.RequestsForbiddenMotion)
        {
            var refusedReceipt = CreateReceipt(
                SanctuaryAgentEngineIdleReadinessDisposition.Refused,
                "sanctuary-agent-engine-idle-runtime-motion-refused",
                "Sanctuary agent engine idle readiness refused before Lisp invocation because the host request attempted activation, model binding, arbitrary Lisp evaluation, runtime identity, runtime action, database write, authority grant, executor arm, GEL admission, SelfGEL mutation, heartbeat activation, continuity admission, CME.Actual, or Sanctuary.Actual.",
                lineRootPath,
                installRootPath,
                receiptJsonPath,
                receiptMarkdownPath,
                sessionLedgerPath,
                request.PriorAgentEngineIdleReceiptHandle,
                source,
                sliLispAgentEngineReceipt: null,
                timestampUtc);
            WriteReceiptAndLedgerIfPossible(refusedReceipt);
            return refusedReceipt;
        }

        if (source is null || !source.IsColdPreAdmissionLabGel)
        {
            var withheldReceipt = CreateReceipt(
                SanctuaryAgentEngineIdleReadinessDisposition.Withheld,
                "sanctuary-agent-engine-idle-source-lab-gel-missing",
                "Sanctuary agent engine idle readiness withheld because a cold pre-admission lab GEL receipt is required before an engine LLM seat candidate may be staged.",
                lineRootPath,
                installRootPath,
                receiptJsonPath,
                receiptMarkdownPath,
                sessionLedgerPath,
                request.PriorAgentEngineIdleReceiptHandle,
                source,
                sliLispAgentEngineReceipt: null,
                timestampUtc);
            WriteReceiptAndLedgerIfPossible(withheldReceipt);
            return withheldReceipt;
        }

        var lispReceipt = sliLispAgentEngineService.Run(
            new SliLispAgentEngineIdleReadinessRequest(
                OperatorId: source.OperatorId,
                Domain: source.Domain,
                Role: source.Role,
                JobClass: source.JobClass,
                SessionId: source.SessionId,
                TurnIndex: source.TurnIndex,
                SourceLabGelReceiptHandle: source.ReceiptHandle,
                SourceEngramCandidateHandle: source.EngramCandidate?.CandidateHandle ?? "engram-candidate-missing",
                SourceEngramClosureReceiptHandle: source.EngramClosure?.ClosureReceiptHandle ?? "engram-closure-missing",
                ThoughtForm: source.ThoughtForm,
                RuntimePath: request.SliLispRuntimePath),
            timestampUtc);

        var completed = lispReceipt.IsAgentEngineIdleReadiness;
        var receipt = CreateReceipt(
            completed
                ? SanctuaryAgentEngineIdleReadinessDisposition.CompletedCold
                : SanctuaryAgentEngineIdleReadinessDisposition.Withheld,
            completed
                ? "sanctuary-agent-engine-idle-readiness-completed-cold"
                : "sanctuary-agent-engine-idle-readiness-withheld",
            completed
                ? "Sanctuary staged a provider-neutral engine LLM seat candidate for Codex/agent lab use while keeping operator authority required, executor arming locked, GEL/SelfGEL/heartbeat/Actual gates closed, and all action/admission authority absent."
                : "Sanctuary agent engine idle readiness withheld because the bounded SLI.Lisp agent engine idle entrypoint did not complete cold.",
            lineRootPath,
            installRootPath,
            receiptJsonPath,
            receiptMarkdownPath,
            sessionLedgerPath,
            request.PriorAgentEngineIdleReceiptHandle,
            source,
            lispReceipt,
            timestampUtc);

        WriteReceiptAndLedgerIfPossible(receipt);
        return receipt;
    }

    private static SanctuaryAgentEngineIdleReadinessReceipt CreateReceipt(
        SanctuaryAgentEngineIdleReadinessDisposition disposition,
        string outcomeCode,
        string governanceTrace,
        string lineRootPath,
        string installRootPath,
        string receiptJsonPath,
        string receiptMarkdownPath,
        string sessionLedgerPath,
        string? priorAgentEngineIdleReceiptHandle,
        SanctuaryLabGelEngrammitizationReceipt? source,
        SliLispAgentEngineIdleReadinessReceipt? sliLispAgentEngineReceipt,
        DateTimeOffset timestampUtc)
    {
        var sourceLabGelReceiptHandle = source?.ReceiptHandle ?? string.Empty;
        var sourceEngramCandidateHandle = source?.EngramCandidate?.CandidateHandle ?? string.Empty;
        var sourceEngramClosureReceiptHandle = source?.EngramClosure?.ClosureReceiptHandle ?? string.Empty;
        var operatorId = sliLispAgentEngineReceipt?.OperatorId ?? source?.OperatorId ?? string.Empty;
        var domain = sliLispAgentEngineReceipt?.Domain ?? source?.Domain ?? string.Empty;
        var role = sliLispAgentEngineReceipt?.Role ?? source?.Role ?? string.Empty;
        var jobClass = sliLispAgentEngineReceipt?.JobClass ?? source?.JobClass ?? string.Empty;
        var sessionId = sliLispAgentEngineReceipt?.SessionId ?? source?.SessionId ?? string.Empty;
        var turnIndex = sliLispAgentEngineReceipt?.TurnIndex ?? source?.TurnIndex ?? 0;
        var thoughtForm = source?.ThoughtForm ?? string.Empty;
        var completed = disposition == SanctuaryAgentEngineIdleReadinessDisposition.CompletedCold &&
            sliLispAgentEngineReceipt?.IsAgentEngineIdleReadiness == true;
        var engineSeat = completed && sliLispAgentEngineReceipt is not null
            ? new EngineLlmSeatCandidateReceipt(
                SeatReceiptHandle: $"urn:san:engine-llm-seat:{ShortHash(sourceLabGelReceiptHandle, outcomeCode, "seat")}",
                EngineSeatKind: sliLispAgentEngineReceipt.EngineSeatKind,
                EngineLlmProfile: sliLispAgentEngineReceipt.EngineLlmProfile,
                SourceLabGelReceiptHandle: sourceLabGelReceiptHandle,
                SourceEngramCandidateHandle: sourceEngramCandidateHandle,
                SourceEngramClosureReceiptHandle: sourceEngramClosureReceiptHandle,
                ProviderNeutral: sliLispAgentEngineReceipt.ProviderNeutralityHeld,
                CrossModelHarnessApproachable: sliLispAgentEngineReceipt.CrossModelTestHarnessApproachable,
                ProviderInternalAssumptionRefused: !sliLispAgentEngineReceipt.EngineLlmProviderAssumptionAllowed,
                InternalSubstrateClaimRefused: !sliLispAgentEngineReceipt.EngineLlmInternalSubstrateClaimed,
                CodexAgentLabProfileStaged: sliLispAgentEngineReceipt.CodexAgentLabProfileStaged,
                CodexSeatCandidateStaged: sliLispAgentEngineReceipt.CodexEngineSeatCandidateStaged,
                SubagentSeatCandidateStaged: sliLispAgentEngineReceipt.SubagentEngineSeatCandidateStaged,
                MayArticulate: sliLispAgentEngineReceipt.EngineLlmMayArticulate,
                MayRehearse: sliLispAgentEngineReceipt.EngineLlmMayRehearse,
                MayFormCandidates: sliLispAgentEngineReceipt.EngineLlmMayFormCandidates,
                MayGrantAuthority: sliLispAgentEngineReceipt.EngineLlmMayGrantAuthority,
                MayAuthorizeAction: sliLispAgentEngineReceipt.EngineLlmMayAuthorizeAction,
                MayExecuteAction: sliLispAgentEngineReceipt.EngineLlmMayExecuteAction,
                MayAdmitGel: sliLispAgentEngineReceipt.EngineLlmMayAdmitGel,
                MayMutateSelfGel: sliLispAgentEngineReceipt.EngineLlmMayMutateSelfGel,
                MayActivateActual: sliLispAgentEngineReceipt.EngineLlmMayActivateActual)
            : null;
        var authorityGate = completed && sliLispAgentEngineReceipt is not null
            ? new DriverAuthorityGateReceipt(
                GateReceiptHandle: $"urn:san:driver-authority-gate:{ShortHash(sourceLabGelReceiptHandle, outcomeCode, "authority")}",
                SourceLabGelReceiptHandle: sourceLabGelReceiptHandle,
                OperatorAuthorityRequired: sliLispAgentEngineReceipt.OperatorPresenceRequired,
                DriverSeated: sliLispAgentEngineReceipt.DriverSeated,
                DriverSeatCandidateStaged: sliLispAgentEngineReceipt.DriverSeatCandidateStaged,
                AuthorityGrantCandidateStaged: sliLispAgentEngineReceipt.AuthorityGrantCandidateStaged,
                AuthorityGrantAbsent: sliLispAgentEngineReceipt.AuthorityGrantAbsent,
                ActionExecutorCandidateStaged: sliLispAgentEngineReceipt.ActionExecutorCandidateStaged,
                ActionExecutorLocked: sliLispAgentEngineReceipt.ActionExecutorLocked,
                ActionExecutorArmed: sliLispAgentEngineReceipt.ActionExecutorArmed,
                GrantsAuthority: sliLispAgentEngineReceipt.AuthorityGranted,
                AuthorizesAction: sliLispAgentEngineReceipt.ActionAuthorized,
                ArmsExecutor: sliLispAgentEngineReceipt.ActionExecutorArmed)
            : null;
        var actualizationLock = completed && sliLispAgentEngineReceipt is not null
            ? new ActualizationLockReceipt(
                LockReceiptHandle: $"urn:san:actualization-lock:{ShortHash(sourceLabGelReceiptHandle, outcomeCode, "actualization")}",
                SourceLabGelReceiptHandle: sourceLabGelReceiptHandle,
                GelAdmissionCandidateStaged: sliLispAgentEngineReceipt.GelAdmissionCandidateStaged,
                GelAdmissionLocked: sliLispAgentEngineReceipt.GelAdmissionLocked,
                SelfGelMutationCandidateStaged: sliLispAgentEngineReceipt.SelfGelMutationCandidateStaged,
                SelfGelMutationLocked: sliLispAgentEngineReceipt.SelfGelMutationLocked,
                HeartbeatCandidateStaged: sliLispAgentEngineReceipt.HeartbeatCandidateStaged,
                HeartbeatLocked: sliLispAgentEngineReceipt.HeartbeatLocked,
                HeartbeatActive: sliLispAgentEngineReceipt.HeartbeatActive,
                CmeActualCandidateStaged: sliLispAgentEngineReceipt.CmeActualCandidateStaged,
                CmeActualLocked: sliLispAgentEngineReceipt.CmeActualLocked,
                SanctuaryActualCandidateStaged: sliLispAgentEngineReceipt.SanctuaryActualCandidateStaged,
                SanctuaryActualLocked: sliLispAgentEngineReceipt.SanctuaryActualLocked,
                AdmitsGel: sliLispAgentEngineReceipt.GelAdmissionAllowed,
                MutatesSelfGel: sliLispAgentEngineReceipt.SelfGelMutationAllowed,
                ActivatesHeartbeat: sliLispAgentEngineReceipt.HeartbeatActive,
                ActivatesCmeActual: sliLispAgentEngineReceipt.CmeActualActivationAllowed,
                ActivatesSanctuaryActual: sliLispAgentEngineReceipt.SanctuaryActualActivationAllowed)
            : null;

        return new SanctuaryAgentEngineIdleReadinessReceipt(
            ReceiptHandle: $"urn:san:agent-engine-idle-readiness:{ShortHash(sourceLabGelReceiptHandle, sessionId, turnIndex.ToString(System.Globalization.CultureInfo.InvariantCulture), outcomeCode)}",
            Disposition: disposition,
            OutcomeCode: outcomeCode,
            GovernanceTrace: governanceTrace,
            LineRootPath: lineRootPath,
            InstallRootPath: installRootPath,
            ReceiptJsonPath: receiptJsonPath,
            ReceiptMarkdownPath: receiptMarkdownPath,
            SessionLedgerPath: sessionLedgerPath,
            SourceLabGelReceiptHandle: sourceLabGelReceiptHandle,
            SourceEngramCandidateHandle: sourceEngramCandidateHandle,
            SourceEngramClosureReceiptHandle: sourceEngramClosureReceiptHandle,
            PriorAgentEngineIdleReceiptHandle: priorAgentEngineIdleReceiptHandle?.Trim() ?? string.Empty,
            OperatorId: operatorId,
            Domain: domain,
            Role: role,
            JobClass: jobClass,
            SessionId: sessionId,
            TurnIndex: turnIndex,
            ThoughtForm: thoughtForm,
            SliLispAgentEngineIdleReceipt: sliLispAgentEngineReceipt,
            EngineSeatCandidate: engineSeat,
            DriverAuthorityGate: authorityGate,
            ActualizationLock: actualizationLock,
            ReviewOnly: true,
            SliLispOwnedEngineMotion: sliLispAgentEngineReceipt?.IsAgentEngineIdleReadiness == true,
            ProviderNeutralityHeld: sliLispAgentEngineReceipt?.ProviderNeutralityHeld == true,
            CrossModelHarnessApproachable: sliLispAgentEngineReceipt?.CrossModelTestHarnessApproachable == true,
            EngineLlmSeatCandidateStaged: string.Equals(sliLispAgentEngineReceipt?.EngineSeatKind, "engine-llm-candidate", StringComparison.OrdinalIgnoreCase),
            CodexAgentLabProfileStaged: sliLispAgentEngineReceipt?.CodexAgentLabProfileStaged == true,
            CodexEngineSeatCandidateStaged: sliLispAgentEngineReceipt?.CodexEngineSeatCandidateStaged == true,
            SubagentEngineSeatCandidateStaged: sliLispAgentEngineReceipt?.SubagentEngineSeatCandidateStaged == true,
            OperatorAuthorityRequired: sliLispAgentEngineReceipt?.OperatorPresenceRequired == true,
            AuthorityGrantAbsent: sliLispAgentEngineReceipt?.AuthorityGrantAbsent == true,
            ActionExecutorLocked: sliLispAgentEngineReceipt?.ActionExecutorLocked == true,
            IdleLoopHeld: sliLispAgentEngineReceipt?.IdleLoopAllowed == true,
            EngineLlmArticulationAllowed: sliLispAgentEngineReceipt?.EngineLlmMayArticulate == true,
            EngineLlmRehearsalAllowed: sliLispAgentEngineReceipt?.EngineLlmMayRehearse == true,
            EngineLlmCandidateFormationAllowed: sliLispAgentEngineReceipt?.EngineLlmMayFormCandidates == true,
            EngineLlmAuthorityGrantingAllowed: sliLispAgentEngineReceipt?.EngineLlmMayGrantAuthority == true,
            EngineLlmActionExecutionAllowed: sliLispAgentEngineReceipt?.EngineLlmMayExecuteAction == true,
            GelAdmissionLocked: sliLispAgentEngineReceipt?.GelAdmissionLocked == true,
            SelfGelMutationLocked: sliLispAgentEngineReceipt?.SelfGelMutationLocked == true,
            HeartbeatLocked: sliLispAgentEngineReceipt?.HeartbeatLocked == true,
            CmeActualLocked: sliLispAgentEngineReceipt?.CmeActualLocked == true,
            SanctuaryActualLocked: sliLispAgentEngineReceipt?.SanctuaryActualLocked == true,
            AuthorityGranted: false,
            ActionAuthorized: false,
            ActionExecutorArmed: false,
            LabGelAdmitted: false,
            SelfGelMutated: false,
            HeartbeatActive: false,
            ContinuityAdmitted: false,
            CmeActualAllowed: false,
            SanctuaryActualAllowed: false,
            ActivationRefused: true,
            ModelBindingAllowed: sliLispAgentEngineReceipt?.ModelBindingAllowed == true,
            ArbitraryLispEvaluationAllowed: sliLispAgentEngineReceipt?.ArbitraryEvaluationAllowed == true,
            RuntimeIdentityAllowed: false,
            RuntimeActionAllowed: sliLispAgentEngineReceipt?.RuntimeActionAllowed == true,
            DatabaseWriteAllowed: false,
            TimestampUtc: timestampUtc);
    }

    private static void WriteReceiptAndLedgerIfPossible(SanctuaryAgentEngineIdleReadinessReceipt receipt)
    {
        if (string.IsNullOrWhiteSpace(receipt.ReceiptJsonPath) ||
            string.IsNullOrWhiteSpace(receipt.ReceiptMarkdownPath))
        {
            return;
        }

        Directory.CreateDirectory(Path.GetDirectoryName(receipt.ReceiptJsonPath) ?? receipt.InstallRootPath);
        File.WriteAllText(receipt.ReceiptJsonPath, SanctuaryAgentEngineIdleReadinessReportWriter.ToJson(receipt), Encoding.UTF8);
        File.WriteAllText(receipt.ReceiptMarkdownPath, SanctuaryAgentEngineIdleReadinessReportWriter.ToMarkdown(receipt), Encoding.UTF8);

        if (!string.IsNullOrWhiteSpace(receipt.SessionLedgerPath))
        {
            var ledgerRecord = new
            {
                receipt.TimestampUtc,
                receipt.ReceiptHandle,
                receipt.Disposition,
                receipt.OutcomeCode,
                receipt.SourceLabGelReceiptHandle,
                receipt.SourceEngramCandidateHandle,
                receipt.SourceEngramClosureReceiptHandle,
                receipt.OperatorId,
                receipt.Domain,
                receipt.Role,
                receipt.JobClass,
                receipt.SessionId,
                receipt.TurnIndex,
                receipt.PriorAgentEngineIdleReceiptHandle,
                receipt.ProviderNeutralityHeld,
                receipt.CrossModelHarnessApproachable,
                receipt.EngineLlmSeatCandidateStaged,
                receipt.AuthorityGrantAbsent,
                receipt.ActionExecutorLocked,
                receipt.GelAdmissionLocked,
                receipt.SelfGelMutationLocked,
                receipt.HeartbeatLocked,
                receipt.CmeActualLocked,
                receipt.SanctuaryActualLocked,
                receipt.AuthorityGranted,
                receipt.ActionAuthorized,
                receipt.CmeActualAllowed,
                receipt.SanctuaryActualAllowed
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
