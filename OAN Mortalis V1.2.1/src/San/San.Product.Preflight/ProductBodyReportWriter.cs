using System.Text;
using System.Text.Json;

namespace San.Product.Preflight;

public static class ProductBodyReportWriter
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = true
    };

    public static string ToJson(ProductBodyPreflightStatus status) =>
        JsonSerializer.Serialize(status, JsonOptions);

    public static string ToMarkdown(ProductBodyPreflightStatus status)
    {
        ArgumentNullException.ThrowIfNull(status);

        var builder = new StringBuilder();
        builder.AppendLine("# Sanctuary Product Body Preflight");
        builder.AppendLine();
        builder.AppendLine($"Status: `{status.Disposition}`");
        builder.AppendLine($"Outcome: `{status.OutcomeCode}`");
        builder.AppendLine($"Refusal: `{status.RefusalCode}`");
        builder.AppendLine($"Generated: `{status.TimestampUtc:O}`");
        builder.AppendLine();
        builder.AppendLine("## Verification");
        builder.AppendLine();
        builder.AppendLine($"- Profile: `{status.VerificationProfile}`");
        builder.AppendLine($"- Setting: `{status.VerificationSettingPath}`");
        builder.AppendLine($"- Lab context root: `{status.LabContextRootPath}`");
        builder.AppendLine($"- Build testing pointer: `{status.BuildTestingPointerPath}`");
        builder.AppendLine();
        builder.AppendLine("## Line");
        builder.AppendLine();
        builder.AppendLine($"- Root: `{status.LineRootPath}`");
        builder.AppendLine($"- Name: `{status.Manifest?.LineName ?? "unknown"}`");
        builder.AppendLine($"- Version: `{status.Manifest?.LineVersion ?? "unknown"}`");
        builder.AppendLine($"- Parent line: `{status.Manifest?.ParentLine ?? "unknown"}`");
        builder.AppendLine($"- Active run-pointing truth: `{status.Manifest?.ActiveExecutableTruth ?? "unknown"}`");
        builder.AppendLine($"- Retained parent preserved: `{status.RetainedParentPreserved}`");
        builder.AppendLine($"- Runtime materialized: `{status.RuntimeMaterialized}`");
        builder.AppendLine();
        builder.AppendLine("## Activation Boundary");
        builder.AppendLine();
        builder.AppendLine($"- Activation authority present: `{status.ActivationAuthorityPresent}`");
        builder.AppendLine($"- Activation refused: `{status.ActivationRefused}`");
        builder.AppendLine($"- Model binding allowed: `{status.ModelBindingAllowed}`");
        builder.AppendLine($"- Lisp evaluation allowed: `{status.LispEvaluationAllowed}`");
        builder.AppendLine($"- Runtime identity allowed: `{status.RuntimeIdentityAllowed}`");
        builder.AppendLine($"- Runtime action allowed: `{status.RuntimeActionAllowed}`");
        builder.AppendLine($"- Database write allowed: `{status.DatabaseWriteAllowed}`");
        builder.AppendLine($"- GEL promotion allowed: `{status.GelPromotionAllowed}`");
        builder.AppendLine($"- CME.Actual allowed: `{status.CmeActualAllowed}`");
        builder.AppendLine($"- Sanctuary.Actual allowed: `{status.SanctuaryActualAllowed}`");
        builder.AppendLine();
        builder.AppendLine("## Checks");
        builder.AppendLine();

        foreach (var check in status.Checks)
        {
            builder.AppendLine($"- `{check.Status}` `{check.CheckId}`: {check.Detail}");
        }

        builder.AppendLine();
        builder.AppendLine("## Governance Trace");
        builder.AppendLine();
        builder.AppendLine(status.GovernanceTrace);
        builder.AppendLine();
        builder.AppendLine($"Next allowed lane: `{status.NextAllowedLane}`");

        return builder.ToString();
    }
}
