using System.Text;
using System.Text.Json;

namespace San.Sanctuary.Runtime;

public static class SanctuaryInstalledSubstrateReportWriter
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = true
    };

    public static string ToJson(SanctuaryInstalledSubstrateReceipt receipt) =>
        JsonSerializer.Serialize(receipt, JsonOptions);

    public static string ToMarkdown(SanctuaryInstalledSubstrateReceipt receipt)
    {
        var builder = new StringBuilder();
        builder.AppendLine("# Sanctuary Installed Body");
        builder.AppendLine();
        builder.AppendLine($"- Disposition: `{receipt.Disposition}`");
        builder.AppendLine($"- Outcome: `{receipt.OutcomeCode}`");
        builder.AppendLine($"- Receipt: `{receipt.ReceiptHandle}`");
        builder.AppendLine($"- Line root: `{receipt.LineRootPath}`");
        builder.AppendLine($"- Install root: `{receipt.InstallRootPath}`");
        builder.AppendLine($"- Body root: `{receipt.BodyRootPath}`");
        builder.AppendLine($"- Governance trace: {receipt.GovernanceTrace}");
        builder.AppendLine();
        builder.AppendLine("## Root Identity");
        builder.AppendLine();
        builder.AppendLine($"- Sanctuary ID: `{receipt.RootIdentity.SanctuaryId}`");
        builder.AppendLine($"- Operator ID: `{receipt.RootIdentity.OperatorId}`");
        builder.AppendLine($"- Actual candidate: `{receipt.RootIdentity.ActualNameCandidate}`");
        builder.AppendLine($"- CME.Actual ID candidate: `{receipt.RootIdentity.CmeActualIdCandidate}`");
        builder.AppendLine($"- OE root: `{receipt.RootIdentity.OpalEngramRootId}`");
        builder.AppendLine($"- SelfGEL root: `{receipt.RootIdentity.SelfGelRootId}`");
        builder.AppendLine($"- Domain / role / job: `{receipt.RootIdentity.Domain}` / `{receipt.RootIdentity.Role}` / `{receipt.RootIdentity.JobClass}`");
        builder.AppendLine();
        builder.AppendLine("## SLI.Lisp Membrane");
        builder.AppendLine();
        if (receipt.SliLispLoadReceipt is null)
        {
            builder.AppendLine("- Load receipt: `none`");
        }
        else
        {
            builder.AppendLine($"- Disposition: `{receipt.SliLispLoadReceipt.Disposition}`");
            builder.AppendLine($"- Outcome: `{receipt.SliLispLoadReceipt.OutcomeCode}`");
            builder.AppendLine($"- Runtime: `{receipt.SliLispLoadReceipt.RuntimeKind}`");
            builder.AppendLine($"- Modules: `{receipt.SliLispLoadReceipt.ModuleCount}`");
            builder.AppendLine($"- Load succeeded: `{receipt.SliLispLoadReceipt.LoadSucceeded}`");
        }

        builder.AppendLine();
        builder.AppendLine("## Bodies");
        builder.AppendLine();
        foreach (var body in receipt.Bodies)
        {
            builder.AppendLine($"- `{body.BodyName}` (`{body.BodyKind}`): {body.Function}");
            builder.AppendLine($"  - Sources: `{string.Join("`, `", body.SourceBodyNames)}`");
            builder.AppendLine($"  - State: `{body.State}`");
            builder.AppendLine($"  - Authority: `{body.GrantsAuthority}`; heartbeat: `{body.ActivatesHeartbeat}`; continuity: `{body.AdmitsContinuity}`");
        }

        builder.AppendLine();
        builder.AppendLine("## Boundary");
        builder.AppendLine();
        builder.AppendLine($"- Base bodies installed: `{receipt.BaseBodiesInstalled}`");
        builder.AppendLine($"- Condensate bodies installed: `{receipt.CondensateBodiesInstalled}`");
        builder.AppendLine($"- Role bodies installed: `{receipt.RoleBodiesInstalled}`");
        builder.AppendLine($"- Activation refused: `{receipt.ActivationRefused}`");
        builder.AppendLine($"- Model binding allowed: `{receipt.ModelBindingAllowed}`");
        builder.AppendLine($"- Lisp evaluation allowed: `{receipt.LispEvaluationAllowed}`");
        builder.AppendLine($"- Runtime identity allowed: `{receipt.RuntimeIdentityAllowed}`");
        builder.AppendLine($"- Runtime action allowed: `{receipt.RuntimeActionAllowed}`");
        builder.AppendLine($"- Database write allowed: `{receipt.DatabaseWriteAllowed}`");
        builder.AppendLine($"- GEL promotion allowed: `{receipt.GelPromotionAllowed}`");
        builder.AppendLine($"- CME.Actual allowed: `{receipt.CmeActualAllowed}`");
        builder.AppendLine($"- Sanctuary.Actual allowed: `{receipt.SanctuaryActualAllowed}`");
        return builder.ToString();
    }
}
