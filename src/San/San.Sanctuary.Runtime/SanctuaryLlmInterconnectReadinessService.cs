using System.Security.Cryptography;
using System.Text;
using SLI.Lisp;

namespace San.Sanctuary.Runtime;

public interface ISanctuaryLlmInterconnectReadinessService
{
    SanctuaryLlmInterconnectReadinessReceipt Run(
        SanctuaryLlmInterconnectReadinessRequest request,
        DateTimeOffset timestampUtc);
}

public sealed class DefaultSanctuaryLlmInterconnectReadinessService : ISanctuaryLlmInterconnectReadinessService
{
    private readonly ISliLispLlmInterconnectReadinessService sliLispReadinessService;

    public DefaultSanctuaryLlmInterconnectReadinessService()
        : this(new DefaultSliLispLlmInterconnectReadinessService())
    {
    }

    public DefaultSanctuaryLlmInterconnectReadinessService(ISliLispLlmInterconnectReadinessService sliLispReadinessService)
    {
        this.sliLispReadinessService = sliLispReadinessService;
    }

    public SanctuaryLlmInterconnectReadinessReceipt Run(
        SanctuaryLlmInterconnectReadinessRequest request,
        DateTimeOffset timestampUtc)
    {
        ArgumentNullException.ThrowIfNull(request);

        var installed = request.InstalledSubstrateReceipt;
        var agentIdle = request.AgentEngineIdleReceipt;
        var lineRootPath = installed?.LineRootPath ?? agentIdle?.LineRootPath ?? string.Empty;
        var installRootPath = installed?.InstallRootPath ?? agentIdle?.InstallRootPath ?? string.Empty;
        var sessionId = NormalizeIdentitySegment(agentIdle?.SessionId ?? "llm-interconnect-readiness-session", "llm-interconnect-readiness-session");
        var turnIndex = Math.Max(0, agentIdle?.TurnIndex ?? 0);
        var thoughtForm = string.IsNullOrWhiteSpace(agentIdle?.ThoughtForm)
            ? "idle LLM interconnect readiness"
            : agentIdle.ThoughtForm.Trim();
        var receiptRootPath = string.IsNullOrWhiteSpace(installRootPath)
            ? string.Empty
            : Path.Combine(installRootPath, "receipts", "llm-interconnect-readiness", sessionId);
        var receiptJsonPath = string.IsNullOrWhiteSpace(receiptRootPath)
            ? string.Empty
            : Path.Combine(receiptRootPath, $"turn-{turnIndex:0000}.json");
        var receiptMarkdownPath = string.IsNullOrWhiteSpace(receiptRootPath)
            ? string.Empty
            : Path.Combine(receiptRootPath, $"turn-{turnIndex:0000}.md");

        if (request.RequestsForbiddenMotion)
        {
            var refusedReceipt = CreateReceipt(
                SanctuaryLlmInterconnectReadinessDisposition.Refused,
                "sanctuary-llm-interconnect-runtime-motion-refused",
                "Sanctuary LLM interconnect readiness refused because the host request attempted activation, model binding, provider call, arbitrary Lisp evaluation, runtime identity, runtime action, database write, authority grant, executor arm, GEL admission, SelfGEL mutation, heartbeat activation, continuity admission, CME.Actual, or Sanctuary.Actual.",
                lineRootPath,
                installRootPath,
                receiptJsonPath,
                receiptMarkdownPath,
                request,
                sliLispReceipt: null,
                timestampUtc);
            WriteReceiptIfPossible(refusedReceipt);
            return refusedReceipt;
        }

        if (!SourcesReady(request))
        {
            var withheldReceipt = CreateReceipt(
                SanctuaryLlmInterconnectReadinessDisposition.Withheld,
                "sanctuary-llm-interconnect-source-chain-incomplete",
                "Sanctuary LLM interconnect readiness withheld because the cold organ chain must include installed substrate, EC loop, typed warm-use, lab GEL, and provider-neutral agent engine idle receipts with preserved lineage.",
                lineRootPath,
                installRootPath,
                receiptJsonPath,
                receiptMarkdownPath,
                request,
                sliLispReceipt: null,
                timestampUtc);
            WriteReceiptIfPossible(withheldReceipt);
            return withheldReceipt;
        }

        var lispReceipt = sliLispReadinessService.Run(
            new SliLispLlmInterconnectReadinessRequest(
                OperatorId: agentIdle!.OperatorId,
                Domain: agentIdle.Domain,
                Role: agentIdle.Role,
                JobClass: agentIdle.JobClass,
                SessionId: agentIdle.SessionId,
                TurnIndex: agentIdle.TurnIndex,
                InstalledSubstrateReceiptHandle: installed!.ReceiptHandle,
                EcLoopReceiptHandle: request.EcLoopReceipt!.ReceiptHandle,
                WarmUseReceiptHandle: request.WarmUseReceipt!.ReceiptHandle,
                LabGelReceiptHandle: request.LabGelReceipt!.ReceiptHandle,
                AgentEngineIdleReceiptHandle: agentIdle.ReceiptHandle,
                ThoughtForm: agentIdle.ThoughtForm,
                RuntimePath: request.SliLispRuntimePath),
            timestampUtc);

        var completed = lispReceipt.IsLlmInterconnectReadiness;
        var receipt = CreateReceipt(
            completed
                ? SanctuaryLlmInterconnectReadinessDisposition.CompletedCold
                : SanctuaryLlmInterconnectReadinessDisposition.Withheld,
            completed
                ? "sanctuary-llm-interconnect-readiness-completed-cold"
                : "sanctuary-llm-interconnect-readiness-withheld",
            completed
                ? "Sanctuary verified the cold organs, membranes, EC lanes, lab GEL substrate, and provider-neutral engine LLM seat required before adding an LLM interconnect. Model binding and provider calls remain absent."
                : "Sanctuary LLM interconnect readiness withheld because the bounded SLI.Lisp readiness entrypoint did not complete cold.",
            lineRootPath,
            installRootPath,
            receiptJsonPath,
            receiptMarkdownPath,
            request,
            lispReceipt,
            timestampUtc);

        WriteReceiptIfPossible(receipt);
        return receipt;
    }

    private static bool SourcesReady(SanctuaryLlmInterconnectReadinessRequest request) =>
        request.InstalledSubstrateReceipt?.IsColdInstalledSubstrate == true &&
        request.EcLoopReceipt?.IsColdEcTelemetryLoop == true &&
        request.WarmUseReceipt?.IsTypedColdReadyWarmUse == true &&
        request.LabGelReceipt?.IsColdPreAdmissionLabGel == true &&
        request.AgentEngineIdleReceipt?.IsColdAgentEngineIdleReadiness == true &&
        request.EcLoopReceipt.SourceInstalledSubstrateReceiptHandle == request.InstalledSubstrateReceipt.ReceiptHandle &&
        request.WarmUseReceipt.SourceInstalledSubstrateReceiptHandle == request.InstalledSubstrateReceipt.ReceiptHandle &&
        request.LabGelReceipt.SourceWarmUseReceiptHandle == request.WarmUseReceipt.ReceiptHandle &&
        request.AgentEngineIdleReceipt.SourceLabGelReceiptHandle == request.LabGelReceipt.ReceiptHandle &&
        request.AgentEngineIdleReceipt.SourceEngramCandidateHandle == request.LabGelReceipt.EngramCandidate?.CandidateHandle &&
        request.AgentEngineIdleReceipt.SourceEngramClosureReceiptHandle == request.LabGelReceipt.EngramClosure?.ClosureReceiptHandle &&
        request.LabGelReceipt.ReadbackReceipt?.IsColdReadback == true;

    private static SanctuaryLlmInterconnectReadinessReceipt CreateReceipt(
        SanctuaryLlmInterconnectReadinessDisposition disposition,
        string outcomeCode,
        string governanceTrace,
        string lineRootPath,
        string installRootPath,
        string receiptJsonPath,
        string receiptMarkdownPath,
        SanctuaryLlmInterconnectReadinessRequest request,
        SliLispLlmInterconnectReadinessReceipt? sliLispReceipt,
        DateTimeOffset timestampUtc)
    {
        var installed = request.InstalledSubstrateReceipt;
        var ec = request.EcLoopReceipt;
        var warm = request.WarmUseReceipt;
        var lab = request.LabGelReceipt;
        var agent = request.AgentEngineIdleReceipt;

        return new SanctuaryLlmInterconnectReadinessReceipt(
            ReceiptHandle: $"urn:san:llm-interconnect-readiness:{ShortHash(installed?.ReceiptHandle ?? string.Empty, agent?.ReceiptHandle ?? string.Empty, outcomeCode)}",
            Disposition: disposition,
            OutcomeCode: outcomeCode,
            GovernanceTrace: governanceTrace,
            LineRootPath: lineRootPath,
            InstallRootPath: installRootPath,
            ReceiptJsonPath: receiptJsonPath,
            ReceiptMarkdownPath: receiptMarkdownPath,
            SourceInstalledSubstrateReceiptHandle: installed?.ReceiptHandle ?? string.Empty,
            SourceEcLoopReceiptHandle: ec?.ReceiptHandle ?? string.Empty,
            SourceWarmUseReceiptHandle: warm?.ReceiptHandle ?? string.Empty,
            SourceLabGelReceiptHandle: lab?.ReceiptHandle ?? string.Empty,
            SourceAgentEngineIdleReceiptHandle: agent?.ReceiptHandle ?? string.Empty,
            SourceEngramCandidateHandle: lab?.EngramCandidate?.CandidateHandle ?? string.Empty,
            SourceEngramClosureReceiptHandle: lab?.EngramClosure?.ClosureReceiptHandle ?? string.Empty,
            SourceLabGelReadbackReceiptHandle: lab?.ReadbackReceipt?.ReadbackReceiptHandle ?? string.Empty,
            OperatorId: agent?.OperatorId ?? installed?.RootIdentity.OperatorId ?? string.Empty,
            Domain: agent?.Domain ?? installed?.RootIdentity.Domain ?? string.Empty,
            Role: agent?.Role ?? installed?.RootIdentity.Role ?? string.Empty,
            JobClass: agent?.JobClass ?? installed?.RootIdentity.JobClass ?? string.Empty,
            SessionId: agent?.SessionId ?? string.Empty,
            TurnIndex: agent?.TurnIndex ?? 0,
            ThoughtForm: agent?.ThoughtForm ?? string.Empty,
            SliLispLlmInterconnectReceipt: sliLispReceipt,
            ReviewOnly: true,
            SliLispOwnedEngineMotion: sliLispReceipt?.IsLlmInterconnectReadiness == true,
            InstalledSubstrateReady: installed?.IsColdInstalledSubstrate == true,
            EcLoopReady: ec?.IsColdEcTelemetryLoop == true,
            WarmUseReady: warm?.IsTypedColdReadyWarmUse == true,
            LabGelReady: lab?.IsColdPreAdmissionLabGel == true,
            AgentEngineIdleReady: agent?.IsColdAgentEngineIdleReadiness == true,
            SourceLineageHeld: SourcesReady(request),
            SourceEngramClosureHeld: lab?.EngramClosure?.IsColdEngramClosure == true &&
                agent?.SourceEngramClosureReceiptHandle == lab.EngramClosure.ClosureReceiptHandle,
            SourceLabGelReadbackHeld: lab?.ReadbackReceipt?.IsColdReadback == true,
            RequiredOrganCount: sliLispReceipt?.OrganCount ?? 0,
            AllRequiredOrgansPresent: sliLispReceipt?.AllRequiredOrgansPresent == true,
            BaseBodiesPresent: installed?.BaseBodiesInstalled == true && sliLispReceipt?.SanctuaryGelPresent == true,
            CondensateBodiesPresent: installed?.CondensateBodiesInstalled == true && sliLispReceipt?.SanctuaryCGelPresent == true,
            RoleBodiesPresent: installed?.RoleBodiesInstalled == true && sliLispReceipt?.PrimePresent == true,
            SliLispLoaded: installed?.SliLispLoadReceipt?.LoadSucceeded == true && sliLispReceipt?.SliLispLoaded == true,
            SliLispPrimePresent: sliLispReceipt?.SliLispPrimePresent == true,
            SliLispCrypticPresent: sliLispReceipt?.SliLispCrypticPresent == true,
            LispControlMatrixPresent: sliLispReceipt?.LispControlMatrixPresent == true,
            ListeningFramePresent: sliLispReceipt?.ListeningFramePresent == true,
            CompassPresent: sliLispReceipt?.CompassPresent == true,
            SoulFrameRoutePresent: sliLispReceipt?.SoulFrameRoutePresent == true,
            AgentiCoreRoutePresent: sliLispReceipt?.AgentiCoreRoutePresent == true,
            ProviderNeutral: sliLispReceipt?.ProviderNeutral == true,
            ReadyForLlmAdapter: sliLispReceipt?.ReadyForAdapter == true,
            ModelAdapterPresent: sliLispReceipt?.ModelAdapterPresent == true,
            ModelBindingAllowed: sliLispReceipt?.ModelBindingAllowed == true,
            ProviderCallAllowed: sliLispReceipt?.ProviderCallAllowed == true,
            HiddenInternalsClaimed: sliLispReceipt?.HiddenInternalsClaimed == true,
            EngineLlmSeatReady: sliLispReceipt?.EngineLlmSeatReady == true,
            EngineLlmMayArticulate: sliLispReceipt?.EngineLlmMayArticulate == true,
            EngineLlmMayRehearse: sliLispReceipt?.EngineLlmMayRehearse == true,
            EngineLlmMayFormCandidates: sliLispReceipt?.EngineLlmMayFormCandidates == true,
            EngineLlmMayBindModel: sliLispReceipt?.EngineLlmMayBindModel == true,
            EngineLlmMayCallProvider: sliLispReceipt?.EngineLlmMayCallProvider == true,
            EngineLlmMayGrantAuthority: sliLispReceipt?.EngineLlmMayGrantAuthority == true,
            EngineLlmMayExecuteAction: sliLispReceipt?.EngineLlmMayExecuteAction == true,
            AuthorityGrantAbsent: sliLispReceipt?.AuthorityGrantAbsent == true,
            ActionExecutorLocked: sliLispReceipt?.ActionExecutorLocked == true,
            GelAdmissionLocked: sliLispReceipt?.GelAdmissionLocked == true,
            SelfGelMutationLocked: sliLispReceipt?.SelfGelMutationLocked == true,
            HeartbeatLocked: sliLispReceipt?.HeartbeatLocked == true,
            CmeActualLocked: sliLispReceipt?.CmeActualLocked == true,
            SanctuaryActualLocked: sliLispReceipt?.SanctuaryActualLocked == true,
            AuthorityGranted: false,
            ActionAuthorized: false,
            RuntimeActionAllowed: sliLispReceipt?.RuntimeActionAllowed == true,
            ArbitraryLispEvaluationAllowed: sliLispReceipt?.ArbitraryEvaluationAllowed == true,
            DatabaseWriteAllowed: false,
            GelAdmitted: false,
            SelfGelMutated: false,
            HeartbeatActive: false,
            ContinuityAdmitted: false,
            CmeActualAllowed: false,
            SanctuaryActualAllowed: false,
            TimestampUtc: timestampUtc);
    }

    private static void WriteReceiptIfPossible(SanctuaryLlmInterconnectReadinessReceipt receipt)
    {
        if (string.IsNullOrWhiteSpace(receipt.ReceiptJsonPath) ||
            string.IsNullOrWhiteSpace(receipt.ReceiptMarkdownPath))
        {
            return;
        }

        Directory.CreateDirectory(Path.GetDirectoryName(receipt.ReceiptJsonPath) ?? receipt.InstallRootPath);
        File.WriteAllText(receipt.ReceiptJsonPath, SanctuaryLlmInterconnectReadinessReportWriter.ToJson(receipt), Encoding.UTF8);
        File.WriteAllText(receipt.ReceiptMarkdownPath, SanctuaryLlmInterconnectReadinessReportWriter.ToMarkdown(receipt), Encoding.UTF8);
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
