using System.Text;
using System.Text.Json;

namespace San.Sanctuary.Runtime;

public static class SanctuaryCmeActualBondingProcessReportWriter
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = true
    };

    public static string ToJson(SanctuaryCmeActualBondingProcessReceipt receipt) =>
        JsonSerializer.Serialize(receipt, JsonOptions);

    public static string ToMarkdown(SanctuaryCmeActualBondingProcessReceipt receipt)
    {
        var builder = new StringBuilder();
        builder.AppendLine("# CME.Actual Bonding Process");
        builder.AppendLine();
        builder.AppendLine($"- Disposition: `{receipt.Disposition}`");
        builder.AppendLine($"- Outcome: `{receipt.OutcomeCode}`");
        builder.AppendLine($"- Receipt: `{receipt.ReceiptHandle}`");
        builder.AppendLine($"- Source tool idle: `{receipt.SourceToolBodyIdleReceiptHandle}`");
        builder.AppendLine($"- Source LLM tick: `{receipt.SourceLlmTickReceiptHandle}`");
        builder.AppendLine($"- Source product output witness commit: `{receipt.SourceProductOutputWitnessCommitReceiptHandle}`");
        builder.AppendLine($"- Prior bonding receipt: `{(string.IsNullOrWhiteSpace(receipt.PriorCmeActualBondingReceiptHandle) ? "none" : receipt.PriorCmeActualBondingReceiptHandle)}`");
        builder.AppendLine($"- Engine owner: `{receipt.SliLispCmeActualBondingReceipt?.Telemetry.GetValueOrDefault("engine-owner") ?? "none"}`");
        builder.AppendLine($"- Bounded entrypoint: `{receipt.SliLispCmeActualBondingReceipt?.Telemetry.GetValueOrDefault("bounded-entrypoint") ?? "none"}`");
        builder.AppendLine($"- Governance trace: {receipt.GovernanceTrace}");
        builder.AppendLine();
        builder.AppendLine("## Named Candidate");
        builder.AppendLine();
        builder.AppendLine($"- CME display name: `{receipt.CmeDisplayName}`");
        builder.AppendLine($"- CME canonical name: `{receipt.CmeCanonicalName}`");
        builder.AppendLine($"- CME root ID: `{receipt.CmeRootId}`");
        builder.AppendLine($"- CME.Actual name candidate: `{receipt.CmeActualNameCandidate}`");
        builder.AppendLine($"- CME.Actual ID candidate: `{receipt.CmeActualIdCandidate}`");
        builder.AppendLine($"- OE root: `{receipt.CmeOpalEngramRootId}`");
        builder.AppendLine($"- SelfGEL root: `{receipt.CmeSelfGelRootId}`");
        builder.AppendLine();
        builder.AppendLine("## Bond");
        builder.AppendLine();
        builder.AppendLine($"- Bond state: `{receipt.BondState}`");
        builder.AppendLine($"- Bond process defined: `{receipt.BondProcessDefined}`");
        builder.AppendLine($"- Vehicle ready: `{receipt.VehicleReady}`");
        builder.AppendLine($"- Tool body idle held: `{receipt.SourceToolBodyIdleHeld}`");
        builder.AppendLine($"- Engine tick witnessed: `{receipt.SourceLlmTickHeld}`");
        builder.AppendLine($"- Product output witness committed: `{receipt.SourceProductOutputWitnessCommitted}`");
        builder.AppendLine($"- Named CME candidate held: `{receipt.NamedCmeCandidateHeld}`");
        builder.AppendLine($"- Operator naming intent witnessed: `{receipt.OperatorNamingIntentWitnessed}`");
        builder.AppendLine($"- Operator runtime authority granted: `{receipt.OperatorRuntimeAuthorityGranted}`");
        builder.AppendLine($"- Activation authority absent: `{receipt.ActivationAuthorityAbsent}`");
        builder.AppendLine($"- Actual admission gap described: `{receipt.ActualAdmissionGapDescribed}`");
        builder.AppendLine($"- Ready for CME.Actual admission review: `{receipt.ReadyForCmeActualAdmissionReview}`");
        builder.AppendLine();
        builder.AppendLine("## Vehicle");
        builder.AppendLine();
        builder.AppendLine($"- Prime available: `{receipt.VehiclePrimeAvailable}`");
        builder.AppendLine($"- Cryptic available: `{receipt.VehicleCrypticAvailable}`");
        builder.AppendLine($"- Steward available: `{receipt.VehicleStewardAvailable}`");
        builder.AppendLine($"- SLI.Lisp membrane loaded: `{receipt.SliLispMembraneLoaded}`");
        builder.AppendLine($"- Lisp Control Matrix present: `{receipt.LispControlMatrixPresent}`");
        builder.AppendLine($"- Listening Frame present: `{receipt.ListeningFramePresent}`");
        builder.AppendLine($"- Compass present: `{receipt.CompassPresent}`");
        builder.AppendLine($"- SoulFrame route present: `{receipt.SoulFrameRoutePresent}`");
        builder.AppendLine($"- AgentiCore route present: `{receipt.AgentiCoreRoutePresent}`");
        builder.AppendLine($"- EC maintained in Lisp: `{receipt.EcMaintainedInLisp}`");
        builder.AppendLine($"- Thinking-about-thinking telemetry available: `{receipt.ThinkingAboutThinkingTelemetryAvailable}`");
        builder.AppendLine($"- Governance SLM intelligent switch candidate: `{receipt.GovernanceSlmIntelligentSwitchCandidate}`");
        builder.AppendLine($"- Governance SLM may discern action readiness: `{receipt.GovernanceSlmMayDiscernActionReadiness}`");
        builder.AppendLine($"- Governance SLM discernment authorizes action: `{receipt.GovernanceSlmDiscernmentAuthorizesAction}`");
        builder.AppendLine();
        builder.AppendLine("## Boundary");
        builder.AppendLine();
        builder.AppendLine($"- CME.Actual candidate only: `{receipt.CmeActualCandidateOnly}`");
        builder.AppendLine($"- CME.Actual bonded candidate: `{receipt.CmeActualBondedCandidate}`");
        builder.AppendLine($"- CME.Actual admitted: `{receipt.CmeActualAdmitted}`");
        builder.AppendLine($"- CME.Actual activated: `{receipt.CmeActualActivated}`");
        builder.AppendLine($"- Runtime identity emitted: `{receipt.RuntimeIdentityEmitted}`");
        builder.AppendLine($"- Heartbeat prepared: `{receipt.HeartbeatPrepared}`");
        builder.AppendLine($"- Heartbeat active: `{receipt.HeartbeatActive}`");
        builder.AppendLine($"- Model bound: `{receipt.ModelBound}`");
        builder.AppendLine($"- Provider called: `{receipt.ProviderCalled}`");
        builder.AppendLine($"- Authority granted: `{receipt.AuthorityGranted}`");
        builder.AppendLine($"- Action authorized: `{receipt.ActionAuthorized}`");
        builder.AppendLine($"- GEL admitted: `{receipt.GelAdmitted}`");
        builder.AppendLine($"- SelfGEL mutated: `{receipt.SelfGelMutated}`");
        builder.AppendLine($"- Continuity admitted: `{receipt.ContinuityAdmitted}`");
        builder.AppendLine($"- Sanctuary.Actual allowed: `{receipt.SanctuaryActualAllowed}`");
        return builder.ToString();
    }
}
