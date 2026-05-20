using System.Text;
using System.Text.Json;

namespace San.Sanctuary.Runtime;

public static class SanctuaryAgentEngineIdleReadinessReportWriter
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = true
    };

    public static string ToJson(SanctuaryAgentEngineIdleReadinessReceipt receipt) =>
        JsonSerializer.Serialize(receipt, JsonOptions);

    public static string ToMarkdown(SanctuaryAgentEngineIdleReadinessReceipt receipt)
    {
        var builder = new StringBuilder();
        builder.AppendLine("# Sanctuary Agent Engine Idle Readiness");
        builder.AppendLine();
        builder.AppendLine($"- Disposition: `{receipt.Disposition}`");
        builder.AppendLine($"- Outcome: `{receipt.OutcomeCode}`");
        builder.AppendLine($"- Receipt: `{receipt.ReceiptHandle}`");
        builder.AppendLine($"- Source lab GEL receipt: `{receipt.SourceLabGelReceiptHandle}`");
        builder.AppendLine($"- Source engram candidate: `{receipt.SourceEngramCandidateHandle}`");
        builder.AppendLine($"- Source engram closure: `{receipt.SourceEngramClosureReceiptHandle}`");
        builder.AppendLine($"- Prior agent engine idle receipt: `{(string.IsNullOrWhiteSpace(receipt.PriorAgentEngineIdleReceiptHandle) ? "none" : receipt.PriorAgentEngineIdleReceiptHandle)}`");
        builder.AppendLine($"- Engine owner: `{receipt.SliLispAgentEngineIdleReceipt?.Telemetry.GetValueOrDefault("engine-owner") ?? "none"}`");
        builder.AppendLine($"- Bounded entrypoint: `{receipt.SliLispAgentEngineIdleReceipt?.Telemetry.GetValueOrDefault("bounded-entrypoint") ?? "none"}`");
        builder.AppendLine($"- Engine LLM profile: `{receipt.EngineSeatCandidate?.EngineLlmProfile ?? "none"}`");
        builder.AppendLine($"- Governance trace: {receipt.GovernanceTrace}");
        builder.AppendLine();
        builder.AppendLine("## Scope");
        builder.AppendLine();
        builder.AppendLine($"- Operator ID: `{receipt.OperatorId}`");
        builder.AppendLine($"- Domain: `{receipt.Domain}`");
        builder.AppendLine($"- Role: `{receipt.Role}`");
        builder.AppendLine($"- Job class: `{receipt.JobClass}`");
        builder.AppendLine($"- Session ID: `{receipt.SessionId}`");
        builder.AppendLine($"- Turn index: `{receipt.TurnIndex}`");
        builder.AppendLine();
        builder.AppendLine("## Engine Seat");
        builder.AppendLine();
        builder.AppendLine($"- Provider neutrality held: `{receipt.ProviderNeutralityHeld}`");
        builder.AppendLine($"- Cross-model harness approachable: `{receipt.CrossModelHarnessApproachable}`");
        builder.AppendLine($"- Engine LLM seat candidate staged: `{receipt.EngineLlmSeatCandidateStaged}`");
        builder.AppendLine($"- Codex/agent lab profile staged: `{receipt.CodexAgentLabProfileStaged}`");
        builder.AppendLine($"- Codex engine seat candidate staged: `{receipt.CodexEngineSeatCandidateStaged}`");
        builder.AppendLine($"- Subagent engine seat candidate staged: `{receipt.SubagentEngineSeatCandidateStaged}`");
        builder.AppendLine($"- May articulate: `{receipt.EngineLlmArticulationAllowed}`");
        builder.AppendLine($"- May rehearse: `{receipt.EngineLlmRehearsalAllowed}`");
        builder.AppendLine($"- May form candidates: `{receipt.EngineLlmCandidateFormationAllowed}`");
        builder.AppendLine();
        builder.AppendLine("## Authority And Actualization Locks");
        builder.AppendLine();
        builder.AppendLine($"- Operator authority required: `{receipt.OperatorAuthorityRequired}`");
        builder.AppendLine($"- Authority grant absent: `{receipt.AuthorityGrantAbsent}`");
        builder.AppendLine($"- Action executor locked: `{receipt.ActionExecutorLocked}`");
        builder.AppendLine($"- GEL admission locked: `{receipt.GelAdmissionLocked}`");
        builder.AppendLine($"- SelfGEL mutation locked: `{receipt.SelfGelMutationLocked}`");
        builder.AppendLine($"- Heartbeat locked: `{receipt.HeartbeatLocked}`");
        builder.AppendLine($"- CME.Actual locked: `{receipt.CmeActualLocked}`");
        builder.AppendLine($"- Sanctuary.Actual locked: `{receipt.SanctuaryActualLocked}`");
        builder.AppendLine();
        builder.AppendLine("## Boundary");
        builder.AppendLine();
        builder.AppendLine($"- Authority granted: `{receipt.AuthorityGranted}`");
        builder.AppendLine($"- Action authorized: `{receipt.ActionAuthorized}`");
        builder.AppendLine($"- Action executor armed: `{receipt.ActionExecutorArmed}`");
        builder.AppendLine($"- Lab GEL admitted: `{receipt.LabGelAdmitted}`");
        builder.AppendLine($"- SelfGEL mutated: `{receipt.SelfGelMutated}`");
        builder.AppendLine($"- Heartbeat active: `{receipt.HeartbeatActive}`");
        builder.AppendLine($"- Continuity admitted: `{receipt.ContinuityAdmitted}`");
        builder.AppendLine($"- CME.Actual allowed: `{receipt.CmeActualAllowed}`");
        builder.AppendLine($"- Sanctuary.Actual allowed: `{receipt.SanctuaryActualAllowed}`");
        builder.AppendLine($"- Model binding allowed: `{receipt.ModelBindingAllowed}`");
        builder.AppendLine($"- Arbitrary Lisp evaluation allowed: `{receipt.ArbitraryLispEvaluationAllowed}`");
        builder.AppendLine($"- Runtime action allowed: `{receipt.RuntimeActionAllowed}`");
        return builder.ToString();
    }
}
