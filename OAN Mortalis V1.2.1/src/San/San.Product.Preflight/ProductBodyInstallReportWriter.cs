using System.Text;
using System.Text.Json;

namespace San.Product.Preflight;

public static class ProductBodyInstallReportWriter
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = true
    };

    public static string ToJson(ProductBodyInstallReceipt receipt) =>
        JsonSerializer.Serialize(receipt, JsonOptions);

    public static string ToMarkdown(ProductBodyInstallReceipt receipt)
    {
        ArgumentNullException.ThrowIfNull(receipt);

        var builder = new StringBuilder();
        builder.AppendLine("# Sanctuary Local Install Receipt");
        builder.AppendLine();
        builder.AppendLine($"Status: `{receipt.Disposition}`");
        builder.AppendLine($"Outcome: `{receipt.OutcomeCode}`");
        builder.AppendLine($"Generated: `{receipt.TimestampUtc:O}`");
        builder.AppendLine();
        builder.AppendLine("## Roots");
        builder.AppendLine();
        builder.AppendLine($"- Line root: `{receipt.LineRootPath}`");
        builder.AppendLine($"- Install root: `{receipt.InstallRootPath}`");
        builder.AppendLine($"- Product source root: `{receipt.ProductSourceRootPath}`");
        builder.AppendLine($"- Product install root: `{receipt.ProductInstallRootPath}`");
        builder.AppendLine($"- Build install root: `{receipt.BuildInstallRootPath}`");
        builder.AppendLine($"- Receipt root: `{receipt.ReceiptRootPath}`");
        builder.AppendLine();
        builder.AppendLine("## Commands");
        builder.AppendLine();
        builder.AppendLine($"- Product command executable: `{receipt.ProductExecutablePath}`");
        builder.AppendLine($"- Command shim: `{receipt.CommandShimPath}`");
        builder.AppendLine($"- PowerShell shim: `{receipt.PowerShellShimPath}`");
        builder.AppendLine();
        builder.AppendLine("## Verification");
        builder.AppendLine();
        builder.AppendLine($"- Preflight status: `{receipt.PreflightStatus.Disposition}`");
        builder.AppendLine($"- Preflight outcome: `{receipt.PreflightStatus.OutcomeCode}`");
        builder.AppendLine($"- Preflight JSON: `{receipt.PreflightReceiptJsonPath}`");
        builder.AppendLine($"- Preflight Markdown: `{receipt.PreflightReceiptMarkdownPath}`");
        builder.AppendLine($"- Copied product files: `{receipt.CopiedProductFileCount}`");
        builder.AppendLine($"- Cold build tool surface ready: `{receipt.ColdBuildToolSurfaceReady}`");
        builder.AppendLine();
        builder.AppendLine("## Activation Boundary");
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
