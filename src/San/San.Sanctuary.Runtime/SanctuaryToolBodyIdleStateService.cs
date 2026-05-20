using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using SLI.Lisp;

namespace San.Sanctuary.Runtime;

public interface ISanctuaryToolBodyIdleStateService
{
    SanctuaryToolBodyIdleStateReceipt Run(
        SanctuaryToolBodyIdleStateRequest request,
        DateTimeOffset timestampUtc);
}

public sealed class DefaultSanctuaryToolBodyIdleStateService : ISanctuaryToolBodyIdleStateService
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private readonly ISliLispToolBodyIdleStateService sliLispToolBodyIdleService;

    public DefaultSanctuaryToolBodyIdleStateService()
        : this(new DefaultSliLispToolBodyIdleStateService())
    {
    }

    public DefaultSanctuaryToolBodyIdleStateService(ISliLispToolBodyIdleStateService sliLispToolBodyIdleService)
    {
        this.sliLispToolBodyIdleService = sliLispToolBodyIdleService;
    }

    public SanctuaryToolBodyIdleStateReceipt Run(
        SanctuaryToolBodyIdleStateRequest request,
        DateTimeOffset timestampUtc)
    {
        ArgumentNullException.ThrowIfNull(request);

        var installed = request.InstalledSubstrateReceipt;
        var warm = request.WarmUseReceipt;
        var lineRootPath = installed?.LineRootPath ?? warm?.LineRootPath ?? string.Empty;
        var installRootPath = installed?.InstallRootPath ?? warm?.InstallRootPath ?? string.Empty;
        var sessionId = NormalizeIdentitySegment(warm?.SessionId ?? "tool-body-idle-session", "tool-body-idle-session");
        var turnIndex = Math.Max(0, warm?.TurnIndex ?? 0);
        var thoughtForm = string.IsNullOrWhiteSpace(warm?.ThoughtForm)
            ? "cold tool body idle without LLM maintenance"
            : warm.ThoughtForm.Trim();
        var receiptRootPath = string.IsNullOrWhiteSpace(installRootPath)
            ? string.Empty
            : Path.Combine(installRootPath, "receipts", "tool-body-idle-state", sessionId);
        var receiptJsonPath = string.IsNullOrWhiteSpace(receiptRootPath)
            ? string.Empty
            : Path.Combine(receiptRootPath, $"turn-{turnIndex:0000}.json");
        var receiptMarkdownPath = string.IsNullOrWhiteSpace(receiptRootPath)
            ? string.Empty
            : Path.Combine(receiptRootPath, $"turn-{turnIndex:0000}.md");
        var sessionLedgerPath = string.IsNullOrWhiteSpace(receiptRootPath)
            ? string.Empty
            : Path.Combine(receiptRootPath, "tool-body-idle-session.jsonl");

        if (request.RequestsForbiddenMotion)
        {
            var refusedReceipt = CreateReceipt(
                SanctuaryToolBodyIdleStateDisposition.Refused,
                "sanctuary-tool-body-idle-runtime-motion-refused",
                "Sanctuary tool body idle refused before Lisp invocation because the host request attempted activation, model binding, provider call, LLM maintenance, tick loop, arbitrary Lisp evaluation, runtime identity, runtime action, database write, authority grant, executor arm, GEL admission, SelfGEL mutation, heartbeat activation, continuity admission, CME.Actual, or Sanctuary.Actual.",
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
                SanctuaryToolBodyIdleStateDisposition.Withheld,
                "sanctuary-tool-body-idle-source-chain-incomplete",
                "Sanctuary tool body idle withheld because the cold chain must include installed substrate, EC loop, typed warm-use, lab GEL, pre-admission engram closure, and lab GEL readback receipts with preserved lineage.",
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

        var lab = request.LabGelReceipt!;
        var lispReceipt = sliLispToolBodyIdleService.Run(
            new SliLispToolBodyIdleStateRequest(
                OperatorId: warm!.OperatorId,
                Domain: warm.Domain,
                Role: warm.Role,
                JobClass: warm.JobClass,
                SessionId: warm.SessionId,
                TurnIndex: warm.TurnIndex,
                InstalledSubstrateReceiptHandle: installed!.ReceiptHandle,
                EcLoopReceiptHandle: request.EcLoopReceipt!.ReceiptHandle,
                WarmUseReceiptHandle: warm.ReceiptHandle,
                LabGelReceiptHandle: lab.ReceiptHandle,
                EngramCandidateHandle: lab.EngramCandidate?.CandidateHandle ?? "engram-candidate-missing",
                EngramClosureReceiptHandle: lab.EngramClosure?.ClosureReceiptHandle ?? "engram-closure-missing",
                LabGelReadbackReceiptHandle: lab.ReadbackReceipt?.ReadbackReceiptHandle ?? "lab-gel-readback-missing",
                ThoughtForm: warm.ThoughtForm,
                RuntimePath: request.SliLispRuntimePath),
            timestampUtc);

        var completed = lispReceipt.IsToolBodyIdleState;
        var receipt = CreateReceipt(
            completed
                ? SanctuaryToolBodyIdleStateDisposition.CompletedCold
                : SanctuaryToolBodyIdleStateDisposition.Withheld,
            completed
                ? "sanctuary-tool-body-idle-state-completed-cold"
                : "sanctuary-tool-body-idle-state-withheld",
            completed
                ? "Sanctuary held the installed tool body in a cold idle state maintained by Sanctuary membranes and pre-admission closure telemetry, without LLM maintenance, model adapter, provider call, tick loop, authority, action, GEL/SelfGEL mutation, heartbeat, CME.Actual, or Sanctuary.Actual."
                : "Sanctuary tool body idle withheld because the bounded SLI.Lisp tool body idle entrypoint did not complete cold.",
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

    private static bool SourcesReady(SanctuaryToolBodyIdleStateRequest request) =>
        request.InstalledSubstrateReceipt?.IsColdInstalledSubstrate == true &&
        request.EcLoopReceipt?.IsColdEcTelemetryLoop == true &&
        request.WarmUseReceipt?.IsTypedColdReadyWarmUse == true &&
        request.LabGelReceipt?.IsColdPreAdmissionLabGel == true &&
        request.EcLoopReceipt.SourceInstalledSubstrateReceiptHandle == request.InstalledSubstrateReceipt.ReceiptHandle &&
        request.WarmUseReceipt.SourceInstalledSubstrateReceiptHandle == request.InstalledSubstrateReceipt.ReceiptHandle &&
        request.LabGelReceipt.SourceWarmUseReceiptHandle == request.WarmUseReceipt.ReceiptHandle &&
        request.LabGelReceipt.EngramClosure?.IsColdEngramClosure == true &&
        request.LabGelReceipt.ReadbackReceipt?.IsColdReadback == true;

    private static SanctuaryToolBodyIdleStateReceipt CreateReceipt(
        SanctuaryToolBodyIdleStateDisposition disposition,
        string outcomeCode,
        string governanceTrace,
        string lineRootPath,
        string installRootPath,
        string receiptJsonPath,
        string receiptMarkdownPath,
        string sessionLedgerPath,
        SanctuaryToolBodyIdleStateRequest request,
        SliLispToolBodyIdleStateReceipt? sliLispReceipt,
        DateTimeOffset timestampUtc)
    {
        var installed = request.InstalledSubstrateReceipt;
        var ec = request.EcLoopReceipt;
        var warm = request.WarmUseReceipt;
        var lab = request.LabGelReceipt;

        return new SanctuaryToolBodyIdleStateReceipt(
            ReceiptHandle: $"urn:san:tool-body-idle-state:{ShortHash(installed?.ReceiptHandle ?? string.Empty, lab?.ReceiptHandle ?? string.Empty, outcomeCode)}",
            Disposition: disposition,
            OutcomeCode: outcomeCode,
            GovernanceTrace: governanceTrace,
            LineRootPath: lineRootPath,
            InstallRootPath: installRootPath,
            ReceiptJsonPath: receiptJsonPath,
            ReceiptMarkdownPath: receiptMarkdownPath,
            SessionLedgerPath: sessionLedgerPath,
            SourceInstalledSubstrateReceiptHandle: installed?.ReceiptHandle ?? string.Empty,
            SourceEcLoopReceiptHandle: ec?.ReceiptHandle ?? string.Empty,
            SourceWarmUseReceiptHandle: warm?.ReceiptHandle ?? string.Empty,
            SourceLabGelReceiptHandle: lab?.ReceiptHandle ?? string.Empty,
            SourceEngramCandidateHandle: lab?.EngramCandidate?.CandidateHandle ?? string.Empty,
            SourceEngramClosureReceiptHandle: lab?.EngramClosure?.ClosureReceiptHandle ?? string.Empty,
            SourceLabGelReadbackReceiptHandle: lab?.ReadbackReceipt?.ReadbackReceiptHandle ?? string.Empty,
            PriorToolBodyIdleReceiptHandle: request.PriorToolBodyIdleReceiptHandle?.Trim() ?? string.Empty,
            OperatorId: sliLispReceipt?.OperatorId ?? warm?.OperatorId ?? installed?.RootIdentity.OperatorId ?? string.Empty,
            Domain: sliLispReceipt?.Domain ?? warm?.Domain ?? installed?.RootIdentity.Domain ?? string.Empty,
            Role: sliLispReceipt?.Role ?? warm?.Role ?? installed?.RootIdentity.Role ?? string.Empty,
            JobClass: sliLispReceipt?.JobClass ?? warm?.JobClass ?? installed?.RootIdentity.JobClass ?? string.Empty,
            SessionId: sliLispReceipt?.SessionId ?? warm?.SessionId ?? string.Empty,
            TurnIndex: sliLispReceipt?.TurnIndex ?? warm?.TurnIndex ?? 0,
            ThoughtForm: warm?.ThoughtForm ?? string.Empty,
            SliLispToolBodyIdleReceipt: sliLispReceipt,
            ReviewOnly: true,
            SliLispOwnedIdleMotion: sliLispReceipt?.IsToolBodyIdleState == true,
            InstalledSubstrateReady: installed?.IsColdInstalledSubstrate == true,
            EcLoopReady: ec?.IsColdEcTelemetryLoop == true,
            WarmUseReady: warm?.IsTypedColdReadyWarmUse == true,
            LabGelReady: lab?.IsColdPreAdmissionLabGel == true,
            SourceLineageHeld: SourcesReady(request),
            SourceEngramClosureHeld: lab?.EngramClosure?.IsColdEngramClosure == true &&
                sliLispReceipt?.EngramClosureReceiptHandle == lab.EngramClosure.ClosureReceiptHandle,
            SourceLabGelReadbackHeld: lab?.ReadbackReceipt?.IsColdReadback == true &&
                sliLispReceipt?.LabGelReadbackReceiptHandle == lab.ReadbackReceipt.ReadbackReceiptHandle,
            RequiredOrganCount: sliLispReceipt?.OrganCount ?? 0,
            AllRequiredOrgansPresent: sliLispReceipt?.AllRequiredOrgansPresent == true,
            BaseBodiesPresent: installed?.BaseBodiesInstalled == true &&
                sliLispReceipt?.SanctuaryGelPresent == true &&
                sliLispReceipt.SanctuaryGoaPresent &&
                sliLispReceipt.SanctuaryMosPresent &&
                sliLispReceipt.SanctuaryVaultPresent,
            CondensateBodiesPresent: installed?.CondensateBodiesInstalled == true &&
                sliLispReceipt?.SanctuaryCGelPresent == true &&
                sliLispReceipt.SanctuaryCGoaPresent &&
                sliLispReceipt.SanctuaryCMosPresent &&
                sliLispReceipt.SanctuaryCVaultPresent,
            RoleBodiesPresent: installed?.RoleBodiesInstalled == true &&
                sliLispReceipt?.PrimePresent == true &&
                sliLispReceipt.CrypticPresent &&
                sliLispReceipt.StewardPresent,
            GoverningCmeCSharpBodiesBuilt: sliLispReceipt?.GoverningCmeCSharpBodiesBuilt == true,
            GoverningCmeActualizedCold: sliLispReceipt?.GoverningCmeActualizedCold == true,
            PrimeGoverningCmeBuilt: sliLispReceipt?.PrimeGoverningCmeBuilt == true,
            CrypticGoverningCmeBuilt: sliLispReceipt?.CrypticGoverningCmeBuilt == true,
            StewardGoverningCmeBuilt: sliLispReceipt?.StewardGoverningCmeBuilt == true,
            GoverningCmeSliLispActualizationSurfacesReady: sliLispReceipt?.GoverningCmeSliLispActualizationSurfacesReady == true,
            GoverningCmeMaintainsIdleState: sliLispReceipt?.GoverningCmeMaintainsIdleState == true,
            GoverningHeartbeatHealthy: sliLispReceipt?.GoverningHeartbeatHealthy == true,
            BondedCmeCallAvailable: sliLispReceipt?.BondedCmeCallAvailable == true,
            SanctuaryGovernanceMonitoringReady: sliLispReceipt?.SanctuaryGovernanceMonitoringReady == true,
            SliLispLoaded: installed?.SliLispLoadReceipt?.LoadSucceeded == true && sliLispReceipt?.SliLispLoaded == true,
            SliLispPrimePresent: sliLispReceipt?.SliLispPrimePresent == true,
            SliLispCrypticPresent: sliLispReceipt?.SliLispCrypticPresent == true,
            LispControlMatrixPresent: sliLispReceipt?.LispControlMatrixPresent == true,
            ListeningFramePresent: sliLispReceipt?.ListeningFramePresent == true,
            CompassPresent: sliLispReceipt?.CompassPresent == true,
            SoulFrameRoutePresent: sliLispReceipt?.SoulFrameRoutePresent == true,
            AgentiCoreRoutePresent: sliLispReceipt?.AgentiCoreRoutePresent == true,
            IdleState: sliLispReceipt?.IdleState ?? string.Empty,
            ToolBodyIdleStateHeld: sliLispReceipt?.ToolBodyIdleStateCompleted == true,
            MaintainedBySanctuary: sliLispReceipt?.MaintainedBySanctuary == true,
            MaintainedByLlm: sliLispReceipt?.MaintainedByLlm == true,
            LlmMaintenanceRequired: sliLispReceipt?.LlmMaintenanceRequired == true,
            LlmAdapterRequired: sliLispReceipt?.LlmAdapterRequired == true,
            ReadyForLlmAdapter: sliLispReceipt?.ReadyForLlmAdapter == true,
            CanAcceptFutureRider: sliLispReceipt?.CanAcceptFutureRider == true,
            GovernanceSlmCandidateDesirable: sliLispReceipt?.GovernanceSlmCandidateDesirable == true,
            GovernanceSlmRoutingSwitchCandidate: sliLispReceipt?.GovernanceSlmRoutingSwitchCandidate == true,
            GovernanceSlmIntelligentSwitchCandidate: sliLispReceipt?.GovernanceSlmIntelligentSwitchCandidate == true,
            GovernanceSlmPresent: sliLispReceipt?.GovernanceSlmPresent == true,
            GovernanceSlmRequiredForIdle: sliLispReceipt?.GovernanceSlmRequiredForIdle == true,
            GovernanceSlmMayDiscriminateEscalation: sliLispReceipt?.GovernanceSlmMayDiscriminateEscalation == true,
            GovernanceSlmMayDiscernActionReadiness: sliLispReceipt?.GovernanceSlmMayDiscernActionReadiness == true,
            GovernanceSlmDiscernmentAuthorizesAction: sliLispReceipt?.GovernanceSlmDiscernmentAuthorizesAction == true,
            GovernanceSlmMayAuthorizeAction: sliLispReceipt?.GovernanceSlmMayAuthorizeAction == true,
            ModelAdapterPresent: sliLispReceipt?.ModelAdapterPresent == true,
            ModelBindingAllowed: sliLispReceipt?.ModelBindingAllowed == true,
            ProviderCallAllowed: sliLispReceipt?.ProviderCallAllowed == true,
            HiddenInternalsClaimed: sliLispReceipt?.HiddenInternalsClaimed == true,
            TickLoopRunning: sliLispReceipt?.TickLoopRunning == true,
            TickMaintainedByLlm: sliLispReceipt?.TickMaintainedByLlm == true,
            IdleLoopHeld: sliLispReceipt?.IdleLoopHeld == true,
            ReturnToPrimeHeld: sliLispReceipt?.ReturnToPrimeHeld == true,
            OperatorReentryAvailable: sliLispReceipt?.OperatorReentryAvailable == true,
            EcMaintainedInLisp: sliLispReceipt?.EcMaintainedInLisp == true,
            LocalEcHoldAvailable: sliLispReceipt?.LocalEcHoldAvailable == true,
            EngineCallRequired: sliLispReceipt?.EngineCallRequired == true,
            LlmEngineCallRequired: sliLispReceipt?.LlmEngineCallRequired == true,
            ExternalEngineCallRequired: sliLispReceipt?.ExternalEngineCallRequired == true,
            AgentEngineIdleRequired: sliLispReceipt?.AgentEngineIdleRequired == true,
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

    private static void WriteReceiptAndLedgerIfPossible(SanctuaryToolBodyIdleStateReceipt receipt)
    {
        if (string.IsNullOrWhiteSpace(receipt.ReceiptJsonPath) ||
            string.IsNullOrWhiteSpace(receipt.ReceiptMarkdownPath))
        {
            return;
        }

        Directory.CreateDirectory(Path.GetDirectoryName(receipt.ReceiptJsonPath) ?? receipt.InstallRootPath);
        File.WriteAllText(receipt.ReceiptJsonPath, SanctuaryToolBodyIdleStateReportWriter.ToJson(receipt), Encoding.UTF8);
        File.WriteAllText(receipt.ReceiptMarkdownPath, SanctuaryToolBodyIdleStateReportWriter.ToMarkdown(receipt), Encoding.UTF8);

        if (!string.IsNullOrWhiteSpace(receipt.SessionLedgerPath))
        {
            var ledgerRecord = new
            {
                receipt.TimestampUtc,
                receipt.ReceiptHandle,
                receipt.Disposition,
                receipt.OutcomeCode,
                receipt.SourceInstalledSubstrateReceiptHandle,
                receipt.SourceEcLoopReceiptHandle,
                receipt.SourceWarmUseReceiptHandle,
                receipt.SourceLabGelReceiptHandle,
                receipt.SourceEngramClosureReceiptHandle,
                receipt.PriorToolBodyIdleReceiptHandle,
                receipt.IdleState,
                receipt.MaintainedBySanctuary,
                receipt.MaintainedByLlm,
                receipt.LlmMaintenanceRequired,
                receipt.ReadyForLlmAdapter,
                receipt.ModelAdapterPresent,
                receipt.ModelBindingAllowed,
                receipt.ProviderCallAllowed,
                receipt.TickLoopRunning,
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
