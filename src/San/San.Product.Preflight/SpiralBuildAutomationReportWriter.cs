using System.Text;
using System.Text.Json;

namespace San.Product.Preflight;

public static class SpiralBuildAutomationReportWriter
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = true
    };

    public static string ToJson(SpiralBuildAutomationReceipt receipt) =>
        JsonSerializer.Serialize(receipt, JsonOptions);

    public static string ToMarkdown(SpiralBuildAutomationReceipt receipt)
    {
        ArgumentNullException.ThrowIfNull(receipt);

        var builder = new StringBuilder();
        builder.AppendLine("# Sanctuary Instrument Body Spiral Build Automation");
        builder.AppendLine();
        builder.AppendLine($"Status: `{receipt.Disposition}`");
        builder.AppendLine($"Outcome: `{receipt.OutcomeCode}`");
        builder.AppendLine($"Generated: `{receipt.TimestampUtc:O}`");
        builder.AppendLine();
        builder.AppendLine("## Law");
        builder.AppendLine();
        builder.AppendLine(receipt.BuildLaw);
        builder.AppendLine();
        builder.AppendLine("## Roots");
        builder.AppendLine();
        builder.AppendLine($"- Line root: `{receipt.LineRootPath}`");
        builder.AppendLine($"- Install root: `{receipt.InstallRootPath}`");
        builder.AppendLine();
        builder.AppendLine("## Phases");
        builder.AppendLine();

        foreach (var phase in receipt.Phases)
        {
            builder.AppendLine($"- {phase}");
        }

        builder.AppendLine();
        builder.AppendLine("## Next Cell");
        builder.AppendLine();

        if (receipt.NextCell is null)
        {
            builder.AppendLine("No adjacent next cell selected.");
        }
        else
        {
            WriteCell(builder, receipt.NextCell);
        }

        builder.AppendLine();
        builder.AppendLine("## Body Map");
        builder.AppendLine();

        foreach (var cell in receipt.Cells)
        {
            WriteCell(builder, cell);
            builder.AppendLine();
        }

        builder.AppendLine("## Automation Boundary");
        builder.AppendLine();
        builder.AppendLine($"- Automation may continue: `{receipt.AutomationMayContinue}`");
        builder.AppendLine($"- HITL required: `{receipt.HitlRequired}`");
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
        builder.AppendLine("## HITL Stop Conditions");
        builder.AppendLine();

        foreach (var condition in receipt.AutomationStopConditions)
        {
            builder.AppendLine($"- {condition}");
        }

        builder.AppendLine();
        builder.AppendLine("## Governance Trace");
        builder.AppendLine();
        builder.AppendLine(receipt.GovernanceTrace);

        return builder.ToString();
    }

    private static void WriteCell(StringBuilder builder, SpiralBuildCellRecord cell)
    {
        builder.AppendLine($"### `{cell.CellId}`");
        builder.AppendLine();
        builder.AppendLine($"- Phase: `{cell.Phase}`");
        builder.AppendLine($"- Layer: `{cell.Layer}`");
        builder.AppendLine($"- Name: {cell.CellName}");
        builder.AppendLine($"- Status: `{cell.Status}`");
        builder.AppendLine($"- Adjacent to: `{string.Join(", ", cell.AdjacentTo)}`");
        builder.AppendLine($"- Required artifacts: `{string.Join(", ", cell.RequiredArtifacts)}`");
        builder.AppendLine($"- Next action: {cell.NextAction}");
        builder.AppendLine($"- HITL required: `{cell.HitlRequired}`");
    }
}
