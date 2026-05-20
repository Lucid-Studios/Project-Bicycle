using System.Text;
using System.Text.Json;

namespace San.Sanctuary.Runtime;

public static class SanctuaryEcTelemetryLoopReportWriter
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = true
    };

    public static string ToJson(SanctuaryEcTelemetryLoopReceipt receipt) =>
        JsonSerializer.Serialize(receipt, JsonOptions);

    public static string ToMarkdown(SanctuaryEcTelemetryLoopReceipt receipt)
    {
        var builder = new StringBuilder();
        builder.AppendLine("# Sanctuary EC Telemetry Loop");
        builder.AppendLine();
        builder.AppendLine($"- Disposition: `{receipt.Disposition}`");
        builder.AppendLine($"- Outcome: `{receipt.OutcomeCode}`");
        builder.AppendLine($"- Receipt: `{receipt.ReceiptHandle}`");
        builder.AppendLine($"- Source installed substrate: `{receipt.SourceInstalledSubstrateReceiptHandle}`");
        builder.AppendLine($"- Engine owner: `{receipt.SliLispEngineReceipt?.Telemetry.GetValueOrDefault("engine-owner") ?? "none"}`");
        builder.AppendLine($"- Bounded entrypoint: `{receipt.SliLispEngineReceipt?.Telemetry.GetValueOrDefault("bounded-entrypoint") ?? "none"}`");
        builder.AppendLine($"- Governance trace: {receipt.GovernanceTrace}");
        builder.AppendLine();
        builder.AppendLine("## Engine Pass");
        builder.AppendLine();
        builder.AppendLine($"- SLI.Lisp owned engine motion: `{receipt.SliLispOwnedEngineMotion}`");
        builder.AppendLine($"- Cold engine loop completed: `{receipt.ColdEngineLoopCompleted}`");
        builder.AppendLine($"- Listening Frame received: `{receipt.ListeningFrameReceived}`");
        builder.AppendLine($"- Compass oriented pressure: `{receipt.CompassOrientedPressure}`");
        builder.AppendLine($"- Thinking-about-thinking telemetry produced: `{receipt.ThinkingAboutThinkingTelemetryProduced}`");
        builder.AppendLine($"- Pre-engram residue produced: `{receipt.PreEngramResidueProduced}`");
        builder.AppendLine($"- Pre-engram residue count: `{receipt.PreEngramResidueCount}`");
        builder.AppendLine($"- Steward reviewed: `{receipt.StewardReviewed}`");
        builder.AppendLine();
        builder.AppendLine("## Boundary");
        builder.AppendLine();
        builder.AppendLine($"- Stream admitted engram: `{receipt.StreamAdmittedEngram}`");
        builder.AppendLine($"- Stream admitted memory: `{receipt.StreamAdmittedMemory}`");
        builder.AppendLine($"- SelfGEL mutated: `{receipt.SelfGelMutated}`");
        builder.AppendLine($"- Continuity admitted: `{receipt.ContinuityAdmitted}`");
        builder.AppendLine($"- Authority granted: `{receipt.AuthorityGranted}`");
        builder.AppendLine($"- Activation refused: `{receipt.ActivationRefused}`");
        builder.AppendLine($"- Model binding allowed: `{receipt.ModelBindingAllowed}`");
        builder.AppendLine($"- Arbitrary Lisp evaluation allowed: `{receipt.ArbitraryLispEvaluationAllowed}`");
        builder.AppendLine($"- Runtime action allowed: `{receipt.RuntimeActionAllowed}`");
        builder.AppendLine($"- CME.Actual allowed: `{receipt.CmeActualAllowed}`");
        builder.AppendLine($"- Sanctuary.Actual allowed: `{receipt.SanctuaryActualAllowed}`");
        return builder.ToString();
    }
}
