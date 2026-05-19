using System.Text;
using System.Text.Json;

namespace San.Product.Preflight;

public static class FirstRiderGovernanceSimulationReportWriter
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = true
    };

    public static string ToJson(FirstRiderGovernanceSimulationReceipt receipt) =>
        JsonSerializer.Serialize(receipt, JsonOptions);

    public static string ToMarkdown(FirstRiderGovernanceSimulationReceipt receipt)
    {
        ArgumentNullException.ThrowIfNull(receipt);

        var builder = new StringBuilder();
        builder.AppendLine("# First Rider Governance Simulation Receipt");
        builder.AppendLine();
        builder.AppendLine($"Status: `{receipt.Disposition}`");
        builder.AppendLine($"Outcome: `{receipt.OutcomeCode}`");
        builder.AppendLine($"Generated: `{receipt.TimestampUtc:O}`");
        builder.AppendLine($"Rider: `{receipt.RiderName}`");
        builder.AppendLine();
        builder.AppendLine("## Roots");
        builder.AppendLine();
        builder.AppendLine($"- Line root: `{receipt.LineRootPath}`");
        builder.AppendLine($"- Install root: `{receipt.InstallRootPath}`");
        builder.AppendLine($"- Thought form: {receipt.ThoughtForm}");
        builder.AppendLine();
        builder.AppendLine("## Boundary");
        builder.AppendLine();
        builder.AppendLine($"- Route complete: `{receipt.RouteComplete}`");
        builder.AppendLine($"- Review only: `{receipt.ReviewOnly}`");
        builder.AppendLine($"- Simulated only: `{receipt.SimulatedOnly}`");
        builder.AppendLine($"- Artifact body verified: `{receipt.ArtifactBodyVerified}`");
        builder.AppendLine($"- Action refused: `{receipt.ActionRefused}`");
        builder.AppendLine($"- Authority granted: `{receipt.AuthorityGranted}`");
        builder.AppendLine($"- Continuity admitted: `{receipt.ContinuityAdmitted}`");
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

        if (receipt.MissingArtifacts.Count > 0)
        {
            builder.AppendLine("## Missing Artifacts");
            builder.AppendLine();

            foreach (var artifact in receipt.MissingArtifacts)
            {
                builder.AppendLine($"- `{artifact}`");
            }

            builder.AppendLine();
        }

        builder.AppendLine("## Route");
        builder.AppendLine();

        foreach (var stage in receipt.Stages)
        {
            builder.AppendLine($"### {stage.StageName}");
            builder.AppendLine();
            builder.AppendLine($"- Stage ID: `{stage.StageId}`");
            builder.AppendLine($"- Boundary cell: `{stage.BoundaryCellId}`");
            builder.AppendLine($"- Result: `{stage.Result}`");
            builder.AppendLine($"- Artifact surface verified: `{stage.ArtifactSurfaceVerified}`");
            builder.AppendLine($"- Review only: `{stage.ReviewOnly}`");
            builder.AppendLine($"- Authority granted: `{stage.AuthorityGranted}`");
            builder.AppendLine($"- Action authorized: `{stage.ActionAuthorized}`");
            builder.AppendLine($"- Continuity mutated: `{stage.ContinuityMutated}`");
            builder.AppendLine($"- Runtime motion requested: `{stage.RuntimeMotionRequested}`");
            builder.AppendLine($"- Function: {stage.GovernanceFunction}");
            builder.AppendLine("- Required artifacts:");

            foreach (var artifact in stage.RequiredArtifacts)
            {
                builder.AppendLine($"  - `{artifact}`");
            }

            if (stage.MissingArtifacts.Count > 0)
            {
                builder.AppendLine("- Missing artifacts:");

                foreach (var artifact in stage.MissingArtifacts)
                {
                    builder.AppendLine($"  - `{artifact}`");
                }
            }

            builder.AppendLine();
        }

        builder.AppendLine("## Governance Trace");
        builder.AppendLine();
        builder.AppendLine(receipt.GovernanceTrace);

        return builder.ToString();
    }
}
