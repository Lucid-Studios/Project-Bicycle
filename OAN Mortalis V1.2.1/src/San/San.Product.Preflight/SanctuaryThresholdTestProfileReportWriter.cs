using System.Text;
using System.Text.Json;

namespace San.Product.Preflight;

public static class SanctuaryThresholdTestProfileReportWriter
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = true
    };

    public static string ToJson(SanctuaryThresholdTestProfile profile) =>
        JsonSerializer.Serialize(profile, JsonOptions);

    public static string ToMarkdown(SanctuaryThresholdTestProfile profile)
    {
        ArgumentNullException.ThrowIfNull(profile);

        var builder = new StringBuilder();
        builder.AppendLine("# Sanctuary.Actual Codex Proxy Triptych Test Profile");
        builder.AppendLine();
        builder.AppendLine($"Status: `{profile.Disposition}`");
        builder.AppendLine($"Outcome: `{profile.OutcomeCode}`");
        builder.AppendLine($"Generated: `{profile.TimestampUtc:O}`");
        builder.AppendLine();
        builder.AppendLine("## Roots");
        builder.AppendLine();
        builder.AppendLine($"- Line root: `{profile.LineRootPath}`");
        builder.AppendLine($"- Install root: `{profile.InstallRootPath}`");
        builder.AppendLine($"- Actual naming law: {profile.ActualNamingLaw}");
        builder.AppendLine($"- Reserved actionable state: `{profile.ReservedActionableStateName}`");
        builder.AppendLine($"- Current install state: `{profile.CurrentInstallStateName}`");
        builder.AppendLine();
        builder.AppendLine("## Base Provider");
        builder.AppendLine();
        builder.AppendLine($"- Provider: `{profile.BaseProvider.ProviderKind}`");
        builder.AppendLine($"- Provider ID: `{profile.BaseProvider.ProviderId}`");
        builder.AppendLine($"- Build testing base: `{profile.BaseProvider.BaseForBuildTesting}`");
        builder.AppendLine($"- Local hosted LLM deferred: `{profile.BaseProvider.LocalHostedLlmDeferred}`");
        builder.AppendLine($"- Persistent memory claimed: `{profile.BaseProvider.PersistentMemoryClaimed}`");
        builder.AppendLine($"- Runtime identity claimed: `{profile.BaseProvider.RuntimeIdentityClaimed}`");
        builder.AppendLine();
        builder.AppendLine("## Proxy Role Seats");
        builder.AppendLine();

        foreach (var seat in profile.RoleSeats)
        {
            builder.AppendLine($"### {seat.SeatKind}");
            builder.AppendLine();
            builder.AppendLine($"- Status: `{seat.Status}`");
            builder.AppendLine($"- Label: `{seat.AgentLabel}`");
            builder.AppendLine($"- Domain: {seat.RoleDomain}");
            builder.AppendLine($"- Invocation: `{seat.InvocationMode}`");
            builder.AppendLine($"- Authority boundary: {seat.AuthorityBoundary}");
            builder.AppendLine($"- Grants authority: `{seat.GrantsAuthority}`");
            builder.AppendLine($"- Self-authorizes: `{seat.SelfAuthorizes}`");
            builder.AppendLine($"- Activates CME.Actual: `{seat.ActivatesCmeActual}`");
            builder.AppendLine();
        }

        builder.AppendLine("## Boundary");
        builder.AppendLine();
        builder.AppendLine($"- Codex may build: `{profile.CodexProxyMayBuild}`");
        builder.AppendLine($"- Codex may authorize: `{profile.CodexProxyMayAuthorize}`");
        builder.AppendLine($"- Dedicated agents required only when needed: `{profile.DedicatedAgentsRequiredOnlyWhenNeeded}`");
        builder.AppendLine($"- Local hosted LLM deferred until first CME test: `{profile.LocalHostedLlmDeferredUntilFirstCmeTest}`");
        builder.AppendLine($"- Reserved actionable state authorized: `{profile.ReservedActionableStateAuthorized}`");
        builder.AppendLine($"- Activation refused: `{profile.ActivationRefused}`");
        builder.AppendLine($"- Model binding allowed: `{profile.ModelBindingAllowed}`");
        builder.AppendLine($"- Lisp evaluation allowed: `{profile.LispEvaluationAllowed}`");
        builder.AppendLine($"- Runtime identity allowed: `{profile.RuntimeIdentityAllowed}`");
        builder.AppendLine($"- Runtime action allowed: `{profile.RuntimeActionAllowed}`");
        builder.AppendLine($"- Database write allowed: `{profile.DatabaseWriteAllowed}`");
        builder.AppendLine($"- GEL promotion allowed: `{profile.GelPromotionAllowed}`");
        builder.AppendLine($"- CME.Actual allowed: `{profile.CmeActualAllowed}`");
        builder.AppendLine($"- Sanctuary.Actual allowed: `{profile.SanctuaryActualAllowed}`");
        builder.AppendLine();
        builder.AppendLine("## Governance Trace");
        builder.AppendLine();
        builder.AppendLine(profile.GovernanceTrace);

        return builder.ToString();
    }
}
