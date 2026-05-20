using System.Text;
using System.Text.Json;

namespace San.Sanctuary.Runtime;

public static class SanctuaryLlmTickCycleReportWriter
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = true
    };

    public static string ToJson(SanctuaryLlmTickCycleReceipt receipt) =>
        JsonSerializer.Serialize(receipt, JsonOptions);

    public static string ToMarkdown(SanctuaryLlmTickCycleReceipt receipt)
    {
        var builder = new StringBuilder();
        builder.AppendLine("# Sanctuary LLM Tick Cycle");
        builder.AppendLine();
        builder.AppendLine($"- Disposition: `{receipt.Disposition}`");
        builder.AppendLine($"- Outcome: `{receipt.OutcomeCode}`");
        builder.AppendLine($"- Receipt: `{receipt.ReceiptHandle}`");
        builder.AppendLine($"- Bounded entrypoint: `{receipt.SliLispLlmTickReceipt?.Telemetry.GetValueOrDefault("bounded-entrypoint") ?? "none"}`");
        builder.AppendLine($"- Governance trace: {receipt.GovernanceTrace}");
        builder.AppendLine();
        builder.AppendLine("## Source");
        builder.AppendLine();
        builder.AppendLine($"- LLM interconnect readiness held: `{receipt.SourceReadinessHeld}`");
        builder.AppendLine($"- Source lineage held: `{receipt.SourceLineageHeld}`");
        builder.AppendLine($"- Source readiness receipt: `{receipt.SourceLlmInterconnectReadinessReceiptHandle}`");
        builder.AppendLine($"- Source engram closure: `{receipt.SourceEngramClosureReceiptHandle}`");
        builder.AppendLine($"- Source engram closure held: `{receipt.SourceEngramClosureHeld}`");
        builder.AppendLine($"- Prior tick receipt: `{receipt.PriorTickReceiptHandle}`");
        builder.AppendLine();
        builder.AppendLine("## Tick");
        builder.AppendLine();
        builder.AppendLine($"- Tick index: `{receipt.TickIndex}`");
        builder.AppendLine($"- Tick loop running: `{receipt.TickLoopRunning}`");
        builder.AppendLine($"- Tick loop kind: `{receipt.TickLoopKind}`");
        builder.AppendLine($"- Ready for LLM adapter: `{receipt.ReadyForLlmAdapter}`");
        builder.AppendLine($"- Model adapter present: `{receipt.ModelAdapterPresent}`");
        builder.AppendLine($"- Deterministic harness adapter: `{receipt.DeterministicHarnessAdapter}`");
        builder.AppendLine($"- Adapter response witnessed: `{receipt.AdapterResponseWitnessed}`");
        builder.AppendLine($"- Adapter response bounded: `{receipt.AdapterResponseBounded}`");
        builder.AppendLine($"- Adapter output witnessed: `{receipt.AdapterOutputWitnessed}`");
        builder.AppendLine($"- Adapter output bounded: `{receipt.AdapterOutputBounded}`");
        builder.AppendLine($"- Adapter output becomes truth: `{receipt.AdapterOutputBecomesTruth}`");
        builder.AppendLine($"- Adapter output authorizes action: `{receipt.AdapterOutputAuthorizesAction}`");
        builder.AppendLine($"- Adapter output admits memory: `{receipt.AdapterOutputAdmitsMemory}`");
        builder.AppendLine($"- Adapter output admits continuity: `{receipt.AdapterOutputAdmitsContinuity}`");
        builder.AppendLine();
        builder.AppendLine("## Membrane");
        builder.AppendLine();
        builder.AppendLine($"- SLI.Lisp processed tick: `{receipt.SliLispProcessedTick}`");
        builder.AppendLine($"- Listening Frame received: `{receipt.ListeningFrameReceived}`");
        builder.AppendLine($"- Compass oriented pressure: `{receipt.CompassOrientedPressure}`");
        builder.AppendLine($"- Compass cooling required: `{receipt.CompassCoolingRequired}`");
        builder.AppendLine($"- SoulFrame received Listening Frame: `{receipt.SoulFrameReceivedListeningFrame}`");
        builder.AppendLine($"- AgentiCore received Compass pressure: `{receipt.AgentiCoreReceivedCompassPressure}`");
        builder.AppendLine($"- Thinking telemetry produced: `{receipt.ThinkingAboutThinkingTelemetryProduced}`");
        builder.AppendLine($"- Predicate residue produced: `{receipt.PredicateResidueProduced}`");
        builder.AppendLine($"- Predicate residue pre-engram only: `{receipt.PredicateResiduePreEngramOnly}`");
        builder.AppendLine($"- Predicate residue admitted engram: `{receipt.PredicateResidueAdmittedEngram}`");
        builder.AppendLine($"- Tick lineage witnessed: `{receipt.TickLineageWitnessed}`");
        builder.AppendLine($"- First tick origin: `{receipt.FirstTickOrigin}`");
        builder.AppendLine($"- Prior tick linked: `{receipt.PriorTickLinked}`");
        builder.AppendLine($"- Product output witness committed: `{receipt.ProductOutputWitnessCommitted}`");
        builder.AppendLine();
        builder.AppendLine("## Locks");
        builder.AppendLine();
        builder.AppendLine($"- Provider neutral: `{receipt.ProviderNeutral}`");
        builder.AppendLine($"- Model binding allowed: `{receipt.ModelBindingAllowed}`");
        builder.AppendLine($"- Provider call allowed: `{receipt.ProviderCallAllowed}`");
        builder.AppendLine($"- Provider call made: `{receipt.ProviderCallMade}`");
        builder.AppendLine($"- Hidden internals claimed: `{receipt.HiddenInternalsClaimed}`");
        builder.AppendLine($"- Authority grant absent: `{receipt.AuthorityGrantAbsent}`");
        builder.AppendLine($"- Action executor locked: `{receipt.ActionExecutorLocked}`");
        builder.AppendLine($"- GEL admission locked: `{receipt.GelAdmissionLocked}`");
        builder.AppendLine($"- SelfGEL mutation locked: `{receipt.SelfGelMutationLocked}`");
        builder.AppendLine($"- Heartbeat locked: `{receipt.HeartbeatLocked}`");
        builder.AppendLine($"- CME.Actual locked: `{receipt.CmeActualLocked}`");
        builder.AppendLine($"- Sanctuary.Actual locked: `{receipt.SanctuaryActualLocked}`");
        builder.AppendLine($"- Authority granted: `{receipt.AuthorityGranted}`");
        builder.AppendLine($"- Action authorized: `{receipt.ActionAuthorized}`");
        builder.AppendLine($"- CME.Actual allowed: `{receipt.CmeActualAllowed}`");
        builder.AppendLine($"- Sanctuary.Actual allowed: `{receipt.SanctuaryActualAllowed}`");
        return builder.ToString();
    }
}
