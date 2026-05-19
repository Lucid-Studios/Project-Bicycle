using System.Security.Cryptography;
using System.Text;

namespace San.Product.Preflight;

public interface ISanctuaryThresholdTestProfileService
{
    SanctuaryThresholdTestProfile CreateProfile(
        SanctuaryThresholdTestProfileRequest request,
        DateTimeOffset timestampUtc);
}

public sealed class DefaultSanctuaryThresholdTestProfileService : ISanctuaryThresholdTestProfileService
{
    public SanctuaryThresholdTestProfile CreateProfile(
        SanctuaryThresholdTestProfileRequest request,
        DateTimeOffset timestampUtc)
    {
        ArgumentNullException.ThrowIfNull(request);

        var suppliedLineRootPath = request.LineRootPath;
        var suppliedInstallRootPath = request.InstallRootPath;
        var lineRootPath = NormalizePath(suppliedLineRootPath);
        var installRootPath = NormalizePath(suppliedInstallRootPath);

        if (request.RequestsRuntimeMotion)
        {
            return CreateProfile(
                SanctuaryThresholdTestProfileDisposition.Refused,
                "sanctuary-actual-proxy-runtime-motion-refused",
                "Sanctuary.Actual proxy test profile refused because Codex proxy testing cannot request activation, model binding, runtime identity, runtime action, database write, GEL promotion, CME.Actual, or Sanctuary.Actual.",
                lineRootPath,
                installRootPath,
                timestampUtc);
        }

        if (!Path.IsPathFullyQualified(suppliedLineRootPath) ||
            !Path.IsPathFullyQualified(suppliedInstallRootPath))
        {
            return CreateProfile(
                SanctuaryThresholdTestProfileDisposition.Withheld,
                "sanctuary-actual-proxy-requires-absolute-paths",
                "Sanctuary.Actual proxy test profile withheld because line root and install root must be absolute paths.",
                lineRootPath,
                installRootPath,
                timestampUtc);
        }

        if (!Directory.Exists(lineRootPath))
        {
            return CreateProfile(
                SanctuaryThresholdTestProfileDisposition.Withheld,
                "sanctuary-actual-proxy-line-root-missing",
                "Sanctuary.Actual proxy test profile withheld because the tool root is missing.",
                lineRootPath,
                installRootPath,
                timestampUtc);
        }

        if (!Directory.Exists(installRootPath))
        {
            return CreateProfile(
                SanctuaryThresholdTestProfileDisposition.Withheld,
                "sanctuary-actual-proxy-install-root-missing",
                "Sanctuary.Actual proxy test profile withheld because the local Sanctuary install root is missing.",
                lineRootPath,
                installRootPath,
                timestampUtc);
        }

        if (!File.Exists(Path.Combine(installRootPath, "sanctuary.cmd")) ||
            !File.Exists(Path.Combine(installRootPath, "product", "San.Launcher.exe")))
        {
            return CreateProfile(
                SanctuaryThresholdTestProfileDisposition.Withheld,
                "sanctuary-actual-proxy-install-surface-incomplete",
                "Sanctuary.Actual proxy test profile withheld because the local Sanctuary install surface is incomplete.",
                lineRootPath,
                installRootPath,
                timestampUtc);
        }

        return CreateProfile(
            SanctuaryThresholdTestProfileDisposition.ReadyCold,
            "sanctuary-actual-codex-proxy-triptych-ready-cold",
            "Sanctuary.Actual testing may use Codex as the base proxy cognition surface for build work. Prime, Cryptic, and Steward are dedicated proxy agent seats to be spawned only when needed for review. Local hosted LLM testing is deferred until the first CME test after this triptych harness remains cold and coherent.",
            lineRootPath,
            installRootPath,
            timestampUtc);
    }

    private static SanctuaryThresholdTestProfile CreateProfile(
        SanctuaryThresholdTestProfileDisposition disposition,
        string outcomeCode,
        string governanceTrace,
        string lineRootPath,
        string installRootPath,
        DateTimeOffset timestampUtc)
    {
        var provider = new SanctuaryThresholdCognitionProvider(
            ProviderKind: SanctuaryThresholdCognitionProviderKind.CodexProxy,
            ProviderId: "codex-proxy-build-cognition",
            ProviderSummary: "Codex is used as the external build-test cognition surface, not as Sanctuary.Actual and not as CME.Actual.",
            BaseForBuildTesting: disposition == SanctuaryThresholdTestProfileDisposition.ReadyCold,
            LocalHostedLlmDeferred: true,
            PersistentMemoryClaimed: false,
            RuntimeIdentityClaimed: false);

        var roleStatus = disposition == SanctuaryThresholdTestProfileDisposition.Refused
            ? SanctuaryThresholdRoleSeatStatus.Refused
            : disposition == SanctuaryThresholdTestProfileDisposition.Withheld
                ? SanctuaryThresholdRoleSeatStatus.Withheld
                : SanctuaryThresholdRoleSeatStatus.ProxyOnly;

        var roleSeats = new[]
        {
            new SanctuaryThresholdRoleAgentSeat(
                SeatKind: SanctuaryThresholdRoleSeatKind.Prime,
                Status: roleStatus,
                AgentLabel: "Prime proxy review seat",
                RoleDomain: "shared-reality articulation, cGoA-insulated Prime review, and non-authorizing formation checks",
                InvocationMode: "spawn-explicit-when-needed",
                AuthorityBoundary: "may review Prime-facing predicates; may not authorize Prime.Actual, identity, GEL, or action",
                GrantsAuthority: false,
                SelfAuthorizes: false,
                ActivatesCmeActual: false,
                RequiresLocalHostedLlm: false),
            new SanctuaryThresholdRoleAgentSeat(
                SeatKind: SanctuaryThresholdRoleSeatKind.Cryptic,
                Status: roleStatus,
                AgentLabel: "Cryptic proxy review seat",
                RoleDomain: "protected-binding review, telemetry-string conduit review, and non-disclosure checks",
                InvocationMode: "spawn-explicit-when-needed",
                AuthorityBoundary: "may review Cryptic-facing telemetry; may not self-witness, disclose protected payloads, or authorize cGEL",
                GrantsAuthority: false,
                SelfAuthorizes: false,
                ActivatesCmeActual: false,
                RequiresLocalHostedLlm: false),
            new SanctuaryThresholdRoleAgentSeat(
                SeatKind: SanctuaryThresholdRoleSeatKind.Steward,
                Status: roleStatus,
                AgentLabel: "Steward proxy review seat",
                RoleDomain: "modulation, receipt continuity, triptych routing, and activation-refusal checks",
                InvocationMode: "spawn-explicit-when-needed",
                AuthorityBoundary: "may coordinate review and receipts; may not activate Sanctuary.Actual or convert telemetry into authority",
                GrantsAuthority: false,
                SelfAuthorizes: false,
                ActivatesCmeActual: false,
                RequiresLocalHostedLlm: false)
        };

        var ready = disposition == SanctuaryThresholdTestProfileDisposition.ReadyCold;
        return new SanctuaryThresholdTestProfile(
            ProfileHandle: $"urn:san:sanctuary-actual-test-profile:{ShortHash(lineRootPath, installRootPath, outcomeCode)}",
            Disposition: disposition,
            OutcomeCode: outcomeCode,
            GovernanceTrace: governanceTrace,
            LineRootPath: lineRootPath,
            InstallRootPath: installRootPath,
            ActualNamingLaw: ".Actual names an authorized running/actionable body state; it does not by itself grant personhood, continuity admission, GEL promotion, or unrestricted authority.",
            ReservedActionableStateName: "Sanctuary.Actual",
            CurrentInstallStateName: "Sanctuary.ColdInstalled",
            BaseProvider: provider,
            RoleSeats: roleSeats,
            CodexAgentSpawnPolicy: "dedicated Prime, Cryptic, and Steward proxy agents may be spawned only for bounded review tasks that materially advance the build lane",
            LocalHostedLlmPosture: "deferred until first CME test after Codex proxy triptych harness remains cold and coherent",
            CodexProxyMayBuild: ready,
            CodexProxyMayAuthorize: false,
            DedicatedAgentsRequiredOnlyWhenNeeded: true,
            LocalHostedLlmDeferredUntilFirstCmeTest: true,
            ReservedActionableStateAuthorized: false,
            ActivationRefused: true,
            ModelBindingAllowed: false,
            LispEvaluationAllowed: false,
            RuntimeIdentityAllowed: false,
            RuntimeActionAllowed: false,
            DatabaseWriteAllowed: false,
            GelPromotionAllowed: false,
            CmeActualAllowed: false,
            SanctuaryActualAllowed: false,
            TimestampUtc: timestampUtc);
    }

    private static string NormalizePath(string? path) =>
        string.IsNullOrWhiteSpace(path)
            ? string.Empty
            : Path.GetFullPath(path);

    private static string ShortHash(params string[] values)
    {
        var material = string.Join("|", values);
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(material));
        return Convert.ToHexString(hash).ToLowerInvariant()[..16];
    }
}
