using System.Text;
using System.Text.Json;

namespace San.Sanctuary.Runtime;

public static class SanctuaryLlmInterconnectReadinessReportWriter
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = true
    };

    public static string ToJson(SanctuaryLlmInterconnectReadinessReceipt receipt) =>
        JsonSerializer.Serialize(receipt, JsonOptions);

    public static string ToMarkdown(SanctuaryLlmInterconnectReadinessReceipt receipt)
    {
        var builder = new StringBuilder();
        builder.AppendLine("# Sanctuary LLM Interconnect Readiness");
        builder.AppendLine();
        builder.AppendLine($"- Disposition: `{receipt.Disposition}`");
        builder.AppendLine($"- Outcome: `{receipt.OutcomeCode}`");
        builder.AppendLine($"- Receipt: `{receipt.ReceiptHandle}`");
        builder.AppendLine($"- Bounded entrypoint: `{receipt.SliLispLlmInterconnectReceipt?.Telemetry.GetValueOrDefault("bounded-entrypoint") ?? "none"}`");
        builder.AppendLine($"- Governance trace: {receipt.GovernanceTrace}");
        builder.AppendLine();
        builder.AppendLine("## Source Chain");
        builder.AppendLine();
        builder.AppendLine($"- Installed substrate: `{receipt.InstalledSubstrateReady}`");
        builder.AppendLine($"- EC loop: `{receipt.EcLoopReady}`");
        builder.AppendLine($"- Typed warm-use: `{receipt.WarmUseReady}`");
        builder.AppendLine($"- Lab GEL: `{receipt.LabGelReady}`");
        builder.AppendLine($"- Agent engine idle: `{receipt.AgentEngineIdleReady}`");
        builder.AppendLine($"- Source lineage held: `{receipt.SourceLineageHeld}`");
        builder.AppendLine($"- Source engram candidate: `{receipt.SourceEngramCandidateHandle}`");
        builder.AppendLine($"- Source engram closure: `{receipt.SourceEngramClosureReceiptHandle}`");
        builder.AppendLine($"- Source lab GEL readback: `{receipt.SourceLabGelReadbackReceiptHandle}`");
        builder.AppendLine($"- Source engram closure held: `{receipt.SourceEngramClosureHeld}`");
        builder.AppendLine($"- Source lab GEL readback held: `{receipt.SourceLabGelReadbackHeld}`");
        builder.AppendLine();
        builder.AppendLine("## Organs And Membranes");
        builder.AppendLine();
        builder.AppendLine($"- Required organs: `{receipt.RequiredOrganCount}`");
        builder.AppendLine($"- All required organs present: `{receipt.AllRequiredOrgansPresent}`");
        builder.AppendLine($"- Base bodies present: `{receipt.BaseBodiesPresent}`");
        builder.AppendLine($"- Condensate bodies present: `{receipt.CondensateBodiesPresent}`");
        builder.AppendLine($"- Role bodies present: `{receipt.RoleBodiesPresent}`");
        builder.AppendLine($"- SLI.Lisp loaded: `{receipt.SliLispLoaded}`");
        builder.AppendLine($"- SLI.Lisp Prime present: `{receipt.SliLispPrimePresent}`");
        builder.AppendLine($"- SLI.Lisp Cryptic present: `{receipt.SliLispCrypticPresent}`");
        builder.AppendLine($"- Lisp Control Matrix present: `{receipt.LispControlMatrixPresent}`");
        builder.AppendLine($"- Listening Frame present: `{receipt.ListeningFramePresent}`");
        builder.AppendLine($"- Compass present: `{receipt.CompassPresent}`");
        builder.AppendLine($"- SoulFrame route present: `{receipt.SoulFrameRoutePresent}`");
        builder.AppendLine($"- AgentiCore route present: `{receipt.AgentiCoreRoutePresent}`");
        builder.AppendLine();
        builder.AppendLine("## Interconnect Boundary");
        builder.AppendLine();
        builder.AppendLine($"- Provider neutral: `{receipt.ProviderNeutral}`");
        builder.AppendLine($"- Ready for LLM adapter: `{receipt.ReadyForLlmAdapter}`");
        builder.AppendLine($"- Model adapter present: `{receipt.ModelAdapterPresent}`");
        builder.AppendLine($"- Model binding allowed: `{receipt.ModelBindingAllowed}`");
        builder.AppendLine($"- Provider call allowed: `{receipt.ProviderCallAllowed}`");
        builder.AppendLine($"- Hidden internals claimed: `{receipt.HiddenInternalsClaimed}`");
        builder.AppendLine($"- Engine LLM seat ready: `{receipt.EngineLlmSeatReady}`");
        builder.AppendLine($"- Engine LLM may articulate: `{receipt.EngineLlmMayArticulate}`");
        builder.AppendLine($"- Engine LLM may rehearse: `{receipt.EngineLlmMayRehearse}`");
        builder.AppendLine($"- Engine LLM may form candidates: `{receipt.EngineLlmMayFormCandidates}`");
        builder.AppendLine($"- Engine LLM may bind model: `{receipt.EngineLlmMayBindModel}`");
        builder.AppendLine($"- Engine LLM may call provider: `{receipt.EngineLlmMayCallProvider}`");
        builder.AppendLine();
        builder.AppendLine("## Locks");
        builder.AppendLine();
        builder.AppendLine($"- Authority grant absent: `{receipt.AuthorityGrantAbsent}`");
        builder.AppendLine($"- Action executor locked: `{receipt.ActionExecutorLocked}`");
        builder.AppendLine($"- GEL admission locked: `{receipt.GelAdmissionLocked}`");
        builder.AppendLine($"- SelfGEL mutation locked: `{receipt.SelfGelMutationLocked}`");
        builder.AppendLine($"- Heartbeat locked: `{receipt.HeartbeatLocked}`");
        builder.AppendLine($"- CME.Actual locked: `{receipt.CmeActualLocked}`");
        builder.AppendLine($"- Sanctuary.Actual locked: `{receipt.SanctuaryActualLocked}`");
        return builder.ToString();
    }
}
