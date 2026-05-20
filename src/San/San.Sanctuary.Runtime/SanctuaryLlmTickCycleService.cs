using System.Security.Cryptography;
using System.Text;
using SLI.Lisp;

namespace San.Sanctuary.Runtime;

public interface ISanctuaryLlmTickCycleService
{
    SanctuaryLlmTickCycleReceipt Run(
        SanctuaryLlmTickCycleRequest request,
        DateTimeOffset timestampUtc);
}

public sealed class DeterministicHarnessEngineLlmAdapter : IEngineLlmAdapter
{
    public string AdapterKind => "deterministic-harness";

    public EngineLlmAdapterResponsePacket Tick(
        EngineLlmAdapterRequest request,
        DateTimeOffset timestampUtc)
    {
        ArgumentNullException.ThrowIfNull(request);

        var output = string.Join(
            " ",
            "Deterministic harness articulation:",
            "thought form witnessed;",
            "predicate residue may form;",
            "truth, authority, action, memory admission, provider call, and model binding remain false.");

        return new EngineLlmAdapterResponsePacket(
            ReceiptHandle: $"urn:san:engine-llm-adapter:deterministic:{ShortHash(request.SessionId, request.TickIndex.ToString(), request.ThoughtForm, timestampUtc.UtcTicks.ToString())}",
            AdapterKind: AdapterKind,
            OutputText: output,
            ModelAdapterPresent: true,
            DeterministicHarness: true,
            ProviderNeutral: true,
            ResponseWitnessed: true,
            ResponseBounded: true,
            OutputWitnessed: true,
            OutputBounded: true,
            ModelBindingAllowed: false,
            ProviderCallAllowed: false,
            ProviderCallMade: false,
            HiddenInternalsClaimed: false,
            OutputBecomesTruth: false,
            OutputAuthorizesAction: false,
            OutputAdmitsMemory: false,
            OutputAdmitsContinuity: false,
            AuthorityGranted: false,
            ActionAuthorized: false,
            GelAdmitted: false,
            SelfGelMutated: false,
            HeartbeatActive: false,
            CmeActualAllowed: false,
            SanctuaryActualAllowed: false,
            TimestampUtc: timestampUtc);
    }

    private static string ShortHash(params string[] values)
    {
        var material = string.Join("|", values.Select(static value => value?.Trim() ?? string.Empty));
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(material));
        return Convert.ToHexString(hash).ToLowerInvariant()[..16];
    }
}

public sealed class DefaultSanctuaryLlmTickCycleService : ISanctuaryLlmTickCycleService
{
    private readonly ISliLispLlmTickCycleService sliLispTickService;

    public DefaultSanctuaryLlmTickCycleService()
        : this(new DefaultSliLispLlmTickCycleService())
    {
    }

    public DefaultSanctuaryLlmTickCycleService(ISliLispLlmTickCycleService sliLispTickService)
    {
        this.sliLispTickService = sliLispTickService;
    }

    public SanctuaryLlmTickCycleReceipt Run(
        SanctuaryLlmTickCycleRequest request,
        DateTimeOffset timestampUtc)
    {
        ArgumentNullException.ThrowIfNull(request);

        var source = request.LlmInterconnectReadinessReceipt;
        var lineRootPath = source?.LineRootPath ?? string.Empty;
        var installRootPath = source?.InstallRootPath ?? string.Empty;
        var sessionId = NormalizeIdentitySegment(source?.SessionId ?? "llm-tick-cycle-session", "llm-tick-cycle-session");
        var tickIndex = Math.Max(0, request.TickIndex ?? ((source?.TurnIndex ?? 0) + 1));
        var thoughtForm = string.IsNullOrWhiteSpace(request.ThoughtForm)
            ? source?.ThoughtForm ?? "cold LLM tick cycle"
            : request.ThoughtForm.Trim();
        var priorTickReceipt = string.IsNullOrWhiteSpace(request.PriorTickReceiptHandle)
            ? "none"
            : request.PriorTickReceiptHandle.Trim();
        var receiptRootPath = string.IsNullOrWhiteSpace(installRootPath)
            ? string.Empty
            : Path.Combine(installRootPath, "receipts", "llm-tick-cycle", sessionId);
        var receiptJsonPath = string.IsNullOrWhiteSpace(receiptRootPath)
            ? string.Empty
            : Path.Combine(receiptRootPath, $"tick-{tickIndex:0000}.json");
        var receiptMarkdownPath = string.IsNullOrWhiteSpace(receiptRootPath)
            ? string.Empty
            : Path.Combine(receiptRootPath, $"tick-{tickIndex:0000}.md");

        if (request.RequestsForbiddenMotion)
        {
            var refusedReceipt = CreateReceipt(
                SanctuaryLlmTickCycleDisposition.Refused,
                "sanctuary-llm-tick-runtime-motion-refused",
                "Sanctuary LLM tick cycle refused because the host request attempted activation, model binding, provider call, hidden internals claim, arbitrary Lisp evaluation, runtime identity, runtime action, database write, authority grant, executor arm, GEL admission, SelfGEL mutation, heartbeat activation, continuity admission, CME.Actual, or Sanctuary.Actual.",
                lineRootPath,
                installRootPath,
                receiptJsonPath,
                receiptMarkdownPath,
                source,
                priorTickReceipt,
                tickIndex,
                thoughtForm,
                adapterPacket: null,
                sliLispReceipt: null,
                timestampUtc);
            WriteReceiptIfPossible(refusedReceipt);
            return refusedReceipt;
        }

        if (source?.IsColdLlmInterconnectReady != true)
        {
            var withheldReceipt = CreateReceipt(
                SanctuaryLlmTickCycleDisposition.Withheld,
                "sanctuary-llm-tick-source-readiness-incomplete",
                "Sanctuary LLM tick cycle withheld because a completed cold LLM interconnect readiness receipt is required before an adapter tick may be witnessed.",
                lineRootPath,
                installRootPath,
                receiptJsonPath,
                receiptMarkdownPath,
                source,
                priorTickReceipt,
                tickIndex,
                thoughtForm,
                adapterPacket: null,
                sliLispReceipt: null,
                timestampUtc);
            WriteReceiptIfPossible(withheldReceipt);
            return withheldReceipt;
        }

        var adapter = request.EngineLlmAdapter ?? new DeterministicHarnessEngineLlmAdapter();
        var adapterPacket = adapter.Tick(
            new EngineLlmAdapterRequest(
                OperatorId: source.OperatorId,
                Domain: source.Domain,
                Role: source.Role,
                JobClass: source.JobClass,
                SessionId: source.SessionId,
                TickIndex: tickIndex,
                SourceLlmInterconnectReadinessReceiptHandle: source.ReceiptHandle,
                SourceEngramClosureReceiptHandle: source.SourceEngramClosureReceiptHandle,
                PriorTickReceiptHandle: priorTickReceipt,
                ThoughtForm: thoughtForm),
            timestampUtc);

        if (!adapterPacket.IsColdBoundedAdapterPacket)
        {
            var refusedReceipt = CreateReceipt(
                SanctuaryLlmTickCycleDisposition.Refused,
                "sanctuary-llm-tick-adapter-boundary-refused",
                "Sanctuary LLM tick cycle refused because the adapter packet attempted or implied model binding, provider call, hidden internals claim, authority, action, memory admission, continuity admission, GEL admission, SelfGEL mutation, heartbeat, CME.Actual, or Sanctuary.Actual.",
                lineRootPath,
                installRootPath,
                receiptJsonPath,
                receiptMarkdownPath,
                source,
                priorTickReceipt,
                tickIndex,
                thoughtForm,
                adapterPacket,
                sliLispReceipt: null,
                timestampUtc);
            WriteReceiptIfPossible(refusedReceipt);
            return refusedReceipt;
        }

        var lispReceipt = sliLispTickService.Run(
            new SliLispLlmTickCycleRequest(
                OperatorId: source.OperatorId,
                Domain: source.Domain,
                Role: source.Role,
                JobClass: source.JobClass,
                SessionId: source.SessionId,
                TickIndex: tickIndex,
                SourceLlmInterconnectReadinessReceiptHandle: source.ReceiptHandle,
                SourceEngramClosureReceiptHandle: source.SourceEngramClosureReceiptHandle,
                PriorTickReceiptHandle: priorTickReceipt,
                AdapterKind: adapterPacket.AdapterKind,
                AdapterResponseReceiptHandle: adapterPacket.ReceiptHandle,
                AdapterOutput: adapterPacket.OutputText,
                ThoughtForm: thoughtForm,
                RuntimePath: request.SliLispRuntimePath),
            timestampUtc);

        var completed = lispReceipt.IsLlmTickCycle;
        var receipt = CreateReceipt(
            completed
                ? SanctuaryLlmTickCycleDisposition.CompletedCold
                : SanctuaryLlmTickCycleDisposition.Withheld,
            completed
                ? "sanctuary-llm-tick-cycle-completed-cold"
                : "sanctuary-llm-tick-cycle-withheld",
            completed
                ? "Sanctuary witnessed one deterministic LLM adapter tick through SLI.Lisp. The tick loop is running, adapter output became predicate evidence only, and model binding, provider calls, authority, action, GEL admission, SelfGEL mutation, heartbeat, CME.Actual, and Sanctuary.Actual remain absent."
                : "Sanctuary LLM tick cycle withheld because the bounded SLI.Lisp tick entrypoint did not complete cold.",
            lineRootPath,
            installRootPath,
            receiptJsonPath,
            receiptMarkdownPath,
            source,
            priorTickReceipt,
            tickIndex,
            thoughtForm,
            adapterPacket,
            lispReceipt,
            timestampUtc);

        WriteReceiptIfPossible(receipt);
        return receipt;
    }

    private static SanctuaryLlmTickCycleReceipt CreateReceipt(
        SanctuaryLlmTickCycleDisposition disposition,
        string outcomeCode,
        string governanceTrace,
        string lineRootPath,
        string installRootPath,
        string receiptJsonPath,
        string receiptMarkdownPath,
        SanctuaryLlmInterconnectReadinessReceipt? source,
        string priorTickReceipt,
        int tickIndex,
        string thoughtForm,
        EngineLlmAdapterResponsePacket? adapterPacket,
        SliLispLlmTickCycleReceipt? sliLispReceipt,
        DateTimeOffset timestampUtc)
    {
        var receiptHandle = $"urn:san:llm-tick-cycle:{ShortHash(source?.ReceiptHandle ?? string.Empty, adapterPacket?.ReceiptHandle ?? string.Empty, tickIndex.ToString(), outcomeCode)}";
        var sourceEngramClosureReceiptHandle = source?.SourceEngramClosureReceiptHandle ?? string.Empty;
        var productCommit = disposition == SanctuaryLlmTickCycleDisposition.CompletedCold &&
            adapterPacket is not null &&
            sliLispReceipt?.IsLlmTickCycle == true
                ? new ProductOutputWitnessCommitReceipt(
                    CommitReceiptHandle: $"urn:san:product-output-witness-commit:{ShortHash(receiptHandle, adapterPacket.ReceiptHandle, sourceEngramClosureReceiptHandle)}",
                    SourceLlmTickCycleReceiptHandle: receiptHandle,
                    SourceLlmInterconnectReadinessReceiptHandle: source?.ReceiptHandle ?? string.Empty,
                    SourceEngramClosureReceiptHandle: sourceEngramClosureReceiptHandle,
                    AdapterResponseReceiptHandle: adapterPacket.ReceiptHandle,
                    CommitState: "adapter-output-witnessed-after-sli-tick",
                    CommitWrittenAfterSliLispTick: true,
                    ProductOutputWitnessed: adapterPacket.OutputWitnessed && sliLispReceipt.AdapterOutputWitnessed,
                    ProductOutputBounded: adapterPacket.OutputBounded && sliLispReceipt.AdapterOutputBounded,
                    ProductOutputPreEngramOnly: sliLispReceipt.PredicateResiduePreEngramOnly,
                    ProductOutputBecomesTruth: adapterPacket.OutputBecomesTruth || sliLispReceipt.AdapterOutputBecomesTruth,
                    ProductOutputAuthorizesAction: adapterPacket.OutputAuthorizesAction || sliLispReceipt.AdapterOutputAuthorizesAction,
                    ProductOutputAdmitsMemory: adapterPacket.OutputAdmitsMemory || sliLispReceipt.AdapterOutputAdmitsMemory,
                    ProductOutputAdmitsContinuity: adapterPacket.OutputAdmitsContinuity || sliLispReceipt.AdapterOutputAdmitsContinuity,
                    ProductOutputAdmitsGel: adapterPacket.GelAdmitted || sliLispReceipt.GelAdmissionAllowed,
                    ProductOutputMutatesSelfGel: adapterPacket.SelfGelMutated || sliLispReceipt.SelfGelMutationAllowed,
                    ProductOutputActivatesHeartbeat: adapterPacket.HeartbeatActive || sliLispReceipt.HeartbeatActive,
                    ProductOutputActivatesActual: adapterPacket.CmeActualAllowed ||
                        adapterPacket.SanctuaryActualAllowed ||
                        sliLispReceipt.CmeActualActivationAllowed ||
                        sliLispReceipt.SanctuaryActualActivationAllowed)
                : null;

        return new SanctuaryLlmTickCycleReceipt(
            ReceiptHandle: receiptHandle,
            Disposition: disposition,
            OutcomeCode: outcomeCode,
            GovernanceTrace: governanceTrace,
            LineRootPath: lineRootPath,
            InstallRootPath: installRootPath,
            ReceiptJsonPath: receiptJsonPath,
            ReceiptMarkdownPath: receiptMarkdownPath,
            SourceLlmInterconnectReadinessReceiptHandle: source?.ReceiptHandle ?? string.Empty,
            SourceEngramClosureReceiptHandle: sourceEngramClosureReceiptHandle,
            PriorTickReceiptHandle: priorTickReceipt,
            OperatorId: source?.OperatorId ?? string.Empty,
            Domain: source?.Domain ?? string.Empty,
            Role: source?.Role ?? string.Empty,
            JobClass: source?.JobClass ?? string.Empty,
            SessionId: source?.SessionId ?? string.Empty,
            TickIndex: sliLispReceipt?.TickIndex ?? tickIndex,
            ThoughtForm: thoughtForm,
            AdapterResponsePacket: adapterPacket,
            SliLispLlmTickReceipt: sliLispReceipt,
            ProductOutputWitnessCommit: productCommit,
            ReviewOnly: true,
            SliLispOwnedEngineMotion: sliLispReceipt?.IsLlmTickCycle == true,
            SourceReadinessHeld: source?.IsColdLlmInterconnectReady == true,
            SourceLineageHeld: source?.IsColdLlmInterconnectReady == true &&
                sliLispReceipt?.SourceLlmInterconnectReadinessReceiptHandle == source.ReceiptHandle,
            SourceEngramClosureHeld: source?.SourceEngramClosureHeld == true &&
                sliLispReceipt?.SourceEngramClosureReady == true &&
                sliLispReceipt.SourceEngramClosureReceiptHandle == sourceEngramClosureReceiptHandle,
            ReadyForLlmAdapter: source?.ReadyForLlmAdapter == true && sliLispReceipt?.ReadyForAdapter == true,
            TickLoopRunning: sliLispReceipt?.TickLoopRunning == true,
            TickLoopKind: sliLispReceipt?.TickLoopKind ?? string.Empty,
            ModelAdapterPresent: adapterPacket?.ModelAdapterPresent == true || sliLispReceipt?.ModelAdapterPresent == true,
            DeterministicHarnessAdapter: adapterPacket?.DeterministicHarness == true || sliLispReceipt?.DeterministicHarnessAdapter == true,
            AdapterResponseWitnessed: adapterPacket?.ResponseWitnessed == true || sliLispReceipt?.AdapterResponseWitnessed == true,
            AdapterResponseBounded: adapterPacket?.ResponseBounded == true || sliLispReceipt?.AdapterResponseBounded == true,
            AdapterOutputWitnessed: adapterPacket?.OutputWitnessed == true || sliLispReceipt?.AdapterOutputWitnessed == true,
            AdapterOutputBounded: adapterPacket?.OutputBounded == true || sliLispReceipt?.AdapterOutputBounded == true,
            AdapterOutputBecomesTruth: adapterPacket?.OutputBecomesTruth == true || sliLispReceipt?.AdapterOutputBecomesTruth == true,
            AdapterOutputAuthorizesAction: adapterPacket?.OutputAuthorizesAction == true || sliLispReceipt?.AdapterOutputAuthorizesAction == true,
            AdapterOutputAdmitsMemory: adapterPacket?.OutputAdmitsMemory == true || sliLispReceipt?.AdapterOutputAdmitsMemory == true,
            AdapterOutputAdmitsContinuity: adapterPacket?.OutputAdmitsContinuity == true || sliLispReceipt?.AdapterOutputAdmitsContinuity == true,
            ProviderNeutral: adapterPacket?.ProviderNeutral == true || sliLispReceipt?.ProviderNeutral == true,
            ModelBindingAllowed: adapterPacket?.ModelBindingAllowed == true || sliLispReceipt?.ModelBindingAllowed == true,
            ProviderCallAllowed: adapterPacket?.ProviderCallAllowed == true || sliLispReceipt?.ProviderCallAllowed == true,
            ProviderCallMade: adapterPacket?.ProviderCallMade == true,
            HiddenInternalsClaimed: adapterPacket?.HiddenInternalsClaimed == true || sliLispReceipt?.HiddenInternalsClaimed == true,
            SliLispProcessedTick: sliLispReceipt?.SliLispProcessedTick == true,
            ListeningFrameReceived: sliLispReceipt?.ListeningFrameReceived == true,
            CompassOrientedPressure: sliLispReceipt?.CompassOrientedPressure == true,
            CompassCoolingRequired: sliLispReceipt?.CompassCoolingRequired == true,
            SoulFrameReceivedListeningFrame: sliLispReceipt?.SoulFrameReceivedListeningFrame == true,
            AgentiCoreReceivedCompassPressure: sliLispReceipt?.AgentiCoreReceivedCompassPressure == true,
            ThinkingAboutThinkingTelemetryProduced: sliLispReceipt?.ThinkingAboutThinkingTelemetryProduced == true,
            PredicateResidueProduced: sliLispReceipt?.PredicateResidueProduced == true,
            PredicateResiduePreEngramOnly: sliLispReceipt?.PredicateResiduePreEngramOnly == true,
            PredicateResidueAdmittedEngram: sliLispReceipt?.PredicateResidueAdmittedEngram == true,
            TickLineageWitnessed: sliLispReceipt?.TickLineageWitnessed == true,
            FirstTickOrigin: sliLispReceipt?.FirstTickOrigin == true,
            PriorTickLinked: sliLispReceipt?.PriorTickLinked == true,
            TickLineageBecomesMemory: sliLispReceipt?.TickLineageBecomesMemory == true,
            ProductOutputWitnessCommitted: productCommit?.IsColdProductOutputWitnessCommit == true,
            EngineLlmMayArticulate: sliLispReceipt?.EngineLlmMayArticulate == true,
            EngineLlmMayRehearse: sliLispReceipt?.EngineLlmMayRehearse == true,
            EngineLlmMayFormCandidates: sliLispReceipt?.EngineLlmMayFormCandidates == true,
            EngineLlmMayBindModel: sliLispReceipt?.EngineLlmMayBindModel == true,
            EngineLlmMayCallProvider: sliLispReceipt?.EngineLlmMayCallProvider == true,
            EngineLlmMayGrantAuthority: sliLispReceipt?.EngineLlmMayGrantAuthority == true,
            EngineLlmMayExecuteAction: sliLispReceipt?.EngineLlmMayExecuteAction == true,
            StewardReviewed: sliLispReceipt?.StewardReviewed == true,
            AuthorityGrantAbsent: sliLispReceipt?.AuthorityGrantAbsent == true,
            ActionExecutorLocked: sliLispReceipt?.ActionExecutorLocked == true,
            GelAdmissionLocked: sliLispReceipt?.GelAdmissionLocked == true,
            SelfGelMutationLocked: sliLispReceipt?.SelfGelMutationLocked == true,
            HeartbeatLocked: sliLispReceipt?.HeartbeatLocked == true,
            CmeActualLocked: sliLispReceipt?.CmeActualLocked == true,
            SanctuaryActualLocked: sliLispReceipt?.SanctuaryActualLocked == true,
            AuthorityGranted: adapterPacket?.AuthorityGranted == true || sliLispReceipt?.AuthorityGranted == true,
            ActionAuthorized: adapterPacket?.ActionAuthorized == true || sliLispReceipt?.ActionAuthorized == true,
            RuntimeActionAllowed: sliLispReceipt?.RuntimeActionAllowed == true,
            ArbitraryLispEvaluationAllowed: sliLispReceipt?.ArbitraryEvaluationAllowed == true,
            DatabaseWriteAllowed: sliLispReceipt?.DatabaseWriteAllowed == true,
            GelAdmitted: adapterPacket?.GelAdmitted == true || sliLispReceipt?.GelAdmissionAllowed == true,
            SelfGelMutated: adapterPacket?.SelfGelMutated == true || sliLispReceipt?.SelfGelMutationAllowed == true,
            HeartbeatActive: adapterPacket?.HeartbeatActive == true || sliLispReceipt?.HeartbeatActive == true,
            ContinuityAdmitted: sliLispReceipt?.ContinuityAdmissionAllowed == true,
            CmeActualAllowed: adapterPacket?.CmeActualAllowed == true || sliLispReceipt?.CmeActualActivationAllowed == true,
            SanctuaryActualAllowed: adapterPacket?.SanctuaryActualAllowed == true || sliLispReceipt?.SanctuaryActualActivationAllowed == true,
            TimestampUtc: timestampUtc);
    }

    private static void WriteReceiptIfPossible(SanctuaryLlmTickCycleReceipt receipt)
    {
        if (string.IsNullOrWhiteSpace(receipt.ReceiptJsonPath) ||
            string.IsNullOrWhiteSpace(receipt.ReceiptMarkdownPath))
        {
            return;
        }

        Directory.CreateDirectory(Path.GetDirectoryName(receipt.ReceiptJsonPath) ?? receipt.InstallRootPath);
        File.WriteAllText(receipt.ReceiptJsonPath, SanctuaryLlmTickCycleReportWriter.ToJson(receipt), Encoding.UTF8);
        File.WriteAllText(receipt.ReceiptMarkdownPath, SanctuaryLlmTickCycleReportWriter.ToMarkdown(receipt), Encoding.UTF8);
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
