using System.Text;
using System.Text.Json;

namespace San.Sanctuary.Runtime;

public static class SanctuaryToolBodyIdleStateReportWriter
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = true
    };

    public static string ToJson(SanctuaryToolBodyIdleStateReceipt receipt) =>
        JsonSerializer.Serialize(receipt, JsonOptions);

    public static string ToMarkdown(SanctuaryToolBodyIdleStateReceipt receipt)
    {
        var builder = new StringBuilder();
        builder.AppendLine("# Sanctuary Tool Body Idle State");
        builder.AppendLine();
        builder.AppendLine($"- Disposition: `{receipt.Disposition}`");
        builder.AppendLine($"- Outcome: `{receipt.OutcomeCode}`");
        builder.AppendLine($"- Receipt: `{receipt.ReceiptHandle}`");
        builder.AppendLine($"- Source installed substrate: `{receipt.SourceInstalledSubstrateReceiptHandle}`");
        builder.AppendLine($"- Source EC loop: `{receipt.SourceEcLoopReceiptHandle}`");
        builder.AppendLine($"- Source warm-use: `{receipt.SourceWarmUseReceiptHandle}`");
        builder.AppendLine($"- Source lab GEL: `{receipt.SourceLabGelReceiptHandle}`");
        builder.AppendLine($"- Source engram closure: `{receipt.SourceEngramClosureReceiptHandle}`");
        builder.AppendLine($"- Source lab GEL readback: `{receipt.SourceLabGelReadbackReceiptHandle}`");
        builder.AppendLine($"- Prior tool body idle receipt: `{(string.IsNullOrWhiteSpace(receipt.PriorToolBodyIdleReceiptHandle) ? "none" : receipt.PriorToolBodyIdleReceiptHandle)}`");
        builder.AppendLine($"- Engine owner: `{receipt.SliLispToolBodyIdleReceipt?.Telemetry.GetValueOrDefault("engine-owner") ?? "none"}`");
        builder.AppendLine($"- Bounded entrypoint: `{receipt.SliLispToolBodyIdleReceipt?.Telemetry.GetValueOrDefault("bounded-entrypoint") ?? "none"}`");
        builder.AppendLine($"- Governance trace: {receipt.GovernanceTrace}");
        builder.AppendLine();
        builder.AppendLine("## Idle Posture");
        builder.AppendLine();
        builder.AppendLine($"- Idle state: `{receipt.IdleState}`");
        builder.AppendLine($"- Maintained by Sanctuary: `{receipt.MaintainedBySanctuary}`");
        builder.AppendLine($"- Maintained by LLM: `{receipt.MaintainedByLlm}`");
        builder.AppendLine($"- LLM maintenance required: `{receipt.LlmMaintenanceRequired}`");
        builder.AppendLine($"- LLM adapter required: `{receipt.LlmAdapterRequired}`");
        builder.AppendLine($"- Ready for LLM adapter: `{receipt.ReadyForLlmAdapter}`");
        builder.AppendLine($"- Can accept future rider: `{receipt.CanAcceptFutureRider}`");
        builder.AppendLine($"- Governance SLM candidate desirable: `{receipt.GovernanceSlmCandidateDesirable}`");
        builder.AppendLine($"- Governance SLM routing switch candidate: `{receipt.GovernanceSlmRoutingSwitchCandidate}`");
        builder.AppendLine($"- Governance SLM intelligent switch candidate: `{receipt.GovernanceSlmIntelligentSwitchCandidate}`");
        builder.AppendLine($"- Governance SLM present: `{receipt.GovernanceSlmPresent}`");
        builder.AppendLine($"- Governance SLM required for idle: `{receipt.GovernanceSlmRequiredForIdle}`");
        builder.AppendLine($"- Governance SLM may discriminate escalation: `{receipt.GovernanceSlmMayDiscriminateEscalation}`");
        builder.AppendLine($"- Governance SLM may discern action readiness: `{receipt.GovernanceSlmMayDiscernActionReadiness}`");
        builder.AppendLine($"- Governance SLM discernment authorizes action: `{receipt.GovernanceSlmDiscernmentAuthorizesAction}`");
        builder.AppendLine($"- Governance SLM may authorize action: `{receipt.GovernanceSlmMayAuthorizeAction}`");
        builder.AppendLine($"- Tick loop running: `{receipt.TickLoopRunning}`");
        builder.AppendLine($"- Tick maintained by LLM: `{receipt.TickMaintainedByLlm}`");
        builder.AppendLine($"- Return to Prime held: `{receipt.ReturnToPrimeHeld}`");
        builder.AppendLine($"- Operator re-entry available: `{receipt.OperatorReentryAvailable}`");
        builder.AppendLine($"- EC maintained in Lisp: `{receipt.EcMaintainedInLisp}`");
        builder.AppendLine($"- Local EC hold available: `{receipt.LocalEcHoldAvailable}`");
        builder.AppendLine($"- Engine call required: `{receipt.EngineCallRequired}`");
        builder.AppendLine($"- LLM engine call required: `{receipt.LlmEngineCallRequired}`");
        builder.AppendLine($"- External engine call required: `{receipt.ExternalEngineCallRequired}`");
        builder.AppendLine();
        builder.AppendLine("## Organs And Membranes");
        builder.AppendLine();
        builder.AppendLine($"- Required organs: `{receipt.RequiredOrganCount}`");
        builder.AppendLine($"- All required organs present: `{receipt.AllRequiredOrgansPresent}`");
        builder.AppendLine($"- Base bodies present: `{receipt.BaseBodiesPresent}`");
        builder.AppendLine($"- Condensate bodies present: `{receipt.CondensateBodiesPresent}`");
        builder.AppendLine($"- Role bodies present: `{receipt.RoleBodiesPresent}`");
        builder.AppendLine($"- Governing CME C# bodies built: `{receipt.GoverningCmeCSharpBodiesBuilt}`");
        builder.AppendLine($"- Governing CME actualized cold: `{receipt.GoverningCmeActualizedCold}`");
        builder.AppendLine($"- Prime governing CME built: `{receipt.PrimeGoverningCmeBuilt}`");
        builder.AppendLine($"- Cryptic governing CME built: `{receipt.CrypticGoverningCmeBuilt}`");
        builder.AppendLine($"- Steward governing CME built: `{receipt.StewardGoverningCmeBuilt}`");
        builder.AppendLine($"- CME SLI.Lisp actualization surfaces ready: `{receipt.GoverningCmeSliLispActualizationSurfacesReady}`");
        builder.AppendLine($"- Governing heartbeat healthy: `{receipt.GoverningHeartbeatHealthy}`");
        builder.AppendLine($"- Bonded CME call available: `{receipt.BondedCmeCallAvailable}`");
        builder.AppendLine($"- Sanctuary governance monitoring ready: `{receipt.SanctuaryGovernanceMonitoringReady}`");
        builder.AppendLine($"- SLI.Lisp loaded: `{receipt.SliLispLoaded}`");
        builder.AppendLine($"- Lisp Control Matrix present: `{receipt.LispControlMatrixPresent}`");
        builder.AppendLine($"- Listening Frame present: `{receipt.ListeningFramePresent}`");
        builder.AppendLine($"- Compass present: `{receipt.CompassPresent}`");
        builder.AppendLine($"- SoulFrame route present: `{receipt.SoulFrameRoutePresent}`");
        builder.AppendLine($"- AgentiCore route present: `{receipt.AgentiCoreRoutePresent}`");
        builder.AppendLine();
        builder.AppendLine("## Boundary");
        builder.AppendLine();
        builder.AppendLine($"- Model adapter present: `{receipt.ModelAdapterPresent}`");
        builder.AppendLine($"- Model binding allowed: `{receipt.ModelBindingAllowed}`");
        builder.AppendLine($"- Provider call allowed: `{receipt.ProviderCallAllowed}`");
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
        builder.AppendLine($"- Heartbeat active: `{receipt.HeartbeatActive}`");
        builder.AppendLine($"- CME.Actual allowed: `{receipt.CmeActualAllowed}`");
        builder.AppendLine($"- Sanctuary.Actual allowed: `{receipt.SanctuaryActualAllowed}`");
        return builder.ToString();
    }
}
