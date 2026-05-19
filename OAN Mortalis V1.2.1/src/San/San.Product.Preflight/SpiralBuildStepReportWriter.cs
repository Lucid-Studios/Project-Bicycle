using System.Text;
using System.Text.Json;

namespace San.Product.Preflight;

public static class SpiralBuildStepReportWriter
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = true
    };

    public static string ToJson(SpiralBuildStepReceipt receipt) =>
        JsonSerializer.Serialize(receipt, JsonOptions);

    public static string ToMarkdown(SpiralBuildStepReceipt receipt)
    {
        ArgumentNullException.ThrowIfNull(receipt);

        var builder = new StringBuilder();
        builder.AppendLine("# Sanctuary Instrument Body Spiral Build Step");
        builder.AppendLine();
        builder.AppendLine($"Status: `{receipt.Disposition}`");
        builder.AppendLine($"Outcome: `{receipt.OutcomeCode}`");
        builder.AppendLine($"Generated: `{receipt.TimestampUtc:O}`");
        builder.AppendLine();
        builder.AppendLine("## Roots");
        builder.AppendLine();
        builder.AppendLine($"- Line root: `{receipt.LineRootPath}`");
        builder.AppendLine($"- Install root: `{receipt.InstallRootPath}`");
        builder.AppendLine();
        builder.AppendLine("## Walk");
        builder.AppendLine();
        builder.AppendLine($"- First adjacent cell: `{receipt.NextCellBeforeExecution ?? "none"}`");
        builder.AppendLine($"- Last executed cell: `{receipt.ExecutedCellId ?? "none"}`");
        builder.AppendLine($"- Next adjacent cell after walk: `{receipt.NextCellAfterExecution ?? "none"}`");
        builder.AppendLine($"- Automation may continue: `{receipt.AutomationMayContinue}`");
        builder.AppendLine($"- HITL required: `{receipt.HitlRequired}`");
        builder.AppendLine();
        builder.AppendLine("## Executed Cells");
        builder.AppendLine();

        if (receipt.ExecutedCellIds.Count == 0)
        {
            builder.AppendLine("No cold cells were executed.");
        }
        else
        {
            foreach (var cellId in receipt.ExecutedCellIds)
            {
                builder.AppendLine($"- `{cellId}`");
            }
        }

        builder.AppendLine();
        builder.AppendLine("## Artifacts");
        builder.AppendLine();

        foreach (var artifact in receipt.Artifacts)
        {
            builder.AppendLine($"- `{artifact.ArtifactId}`");
            builder.AppendLine($"  - JSON: `{artifact.JsonPath}`");
            builder.AppendLine($"  - Markdown: `{artifact.MarkdownPath}`");
            builder.AppendLine($"  - Summary: {artifact.Summary}");
        }

        builder.AppendLine();
        builder.AppendLine("## Boundary");
        builder.AppendLine();
        builder.AppendLine($"- Activation refused: `{receipt.ActivationRefused}`");
        builder.AppendLine($"- Model binding allowed: `{receipt.ModelBindingAllowed}`");
        builder.AppendLine($"- Lisp evaluation allowed: `{receipt.LispEvaluationAllowed}`");
        builder.AppendLine($"- Runtime identity allowed: `{receipt.RuntimeIdentityAllowed}`");
        builder.AppendLine($"- Runtime action allowed: `{receipt.RuntimeActionAllowed}`");
        builder.AppendLine($"- Database write allowed: `{receipt.DatabaseWriteAllowed}`");
        builder.AppendLine($"- GEL promotion allowed: `{receipt.GelPromotionAllowed}`");
        builder.AppendLine($"- CME.Actual allowed: `{receipt.CmeActualAllowed}`");
        builder.AppendLine($"- Sanctuary.Actual allowed: `{receipt.SanctuaryActualAllowed}`");
        builder.AppendLine();
        builder.AppendLine("## Governance Trace");
        builder.AppendLine();
        builder.AppendLine(receipt.GovernanceTrace);

        return builder.ToString();
    }
}
