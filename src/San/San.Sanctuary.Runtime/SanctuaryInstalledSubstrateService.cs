using System.Security.Cryptography;
using System.Text;
using SLI.Lisp;

namespace San.Sanctuary.Runtime;

public interface ISanctuaryInstalledSubstrateService
{
    SanctuaryInstalledSubstrateReceipt Install(
        SanctuaryInstalledSubstrateRequest request,
        DateTimeOffset timestampUtc);
}

public sealed class DefaultSanctuaryInstalledSubstrateService : ISanctuaryInstalledSubstrateService
{
    private readonly ISliLispRuntimeLoadService sliLispRuntimeLoadService;

    public DefaultSanctuaryInstalledSubstrateService()
        : this(new DefaultSliLispRuntimeLoadService())
    {
    }

    public DefaultSanctuaryInstalledSubstrateService(ISliLispRuntimeLoadService sliLispRuntimeLoadService)
    {
        this.sliLispRuntimeLoadService = sliLispRuntimeLoadService;
    }

    public SanctuaryInstalledSubstrateReceipt Install(
        SanctuaryInstalledSubstrateRequest request,
        DateTimeOffset timestampUtc)
    {
        ArgumentNullException.ThrowIfNull(request);

        var lineRootPath = NormalizePath(request.LineRootPath);
        var installRootPath = NormalizePath(request.InstallRootPath);
        var bodyRootPath = Path.Combine(installRootPath, "sanctuary", "body");
        var receiptRootPath = Path.Combine(installRootPath, "receipts", "sanctuary-installed-body");
        var receiptJsonPath = Path.Combine(receiptRootPath, "sanctuary-installed-substrate.json");
        var receiptMarkdownPath = Path.Combine(receiptRootPath, "sanctuary-installed-substrate.md");
        var rootIdentity = CreateRootIdentity(request);

        if (request.RequestsRuntimeMotion)
        {
            return CreateReceipt(
                SanctuaryInstalledSubstrateDisposition.Refused,
                "sanctuary-installed-body-runtime-motion-refused",
                "Sanctuary installed body refused because body installation cannot request activation, model binding, Lisp evaluation, runtime identity, runtime action, database write, GEL promotion, CME.Actual, or Sanctuary.Actual.",
                lineRootPath,
                installRootPath,
                bodyRootPath,
                receiptJsonPath,
                receiptMarkdownPath,
                rootIdentity,
                bodies: [],
                sliLispLoadReceipt: null,
                timestampUtc);
        }

        var pathValidation = ValidatePaths(request.LineRootPath, request.InstallRootPath, lineRootPath, installRootPath);
        if (pathValidation is not null)
        {
            return CreateReceipt(
                SanctuaryInstalledSubstrateDisposition.Withheld,
                pathValidation.Value.OutcomeCode,
                pathValidation.Value.GovernanceTrace,
                lineRootPath,
                installRootPath,
                bodyRootPath,
                receiptJsonPath,
                receiptMarkdownPath,
                rootIdentity,
                bodies: [],
                sliLispLoadReceipt: null,
                timestampUtc);
        }

        Directory.CreateDirectory(bodyRootPath);
        Directory.CreateDirectory(receiptRootPath);

        var sliLispLoadReceipt = sliLispRuntimeLoadService.LoadResidentMembrane(
            new SliLispRuntimeLoadRequest(RuntimePath: request.SliLispRuntimePath),
            timestampUtc);

        if (sliLispLoadReceipt.Disposition != SliLispRuntimeLoadDisposition.LoadedCold ||
            !sliLispLoadReceipt.LoadSucceeded)
        {
            var withheldReceipt = CreateReceipt(
                SanctuaryInstalledSubstrateDisposition.Withheld,
                "sanctuary-installed-body-sli-lisp-load-withheld",
                "Sanctuary installed body withheld because the resident SLI.Lisp membrane did not load cold; Sanctuary bodies cannot be composed without the membrane present.",
                lineRootPath,
                installRootPath,
                bodyRootPath,
                receiptJsonPath,
                receiptMarkdownPath,
                rootIdentity,
                bodies: [],
                sliLispLoadReceipt,
                timestampUtc);
            WriteReceipt(withheldReceipt);
            return withheldReceipt;
        }

        var bodies = CreateBodies(rootIdentity, bodyRootPath, sliLispLoadReceipt).ToArray();
        var receipt = CreateReceipt(
            SanctuaryInstalledSubstrateDisposition.InstalledCold,
            "sanctuary-installed-body-composed-cold",
            "Sanctuary installed body composed Sanctuary.GEL, Sanctuary.GoA, Sanctuary.MoS, Sanctuary.Vault, their c-bodies, and Prime/Cryptic/Steward role bodies as cold installed substrates. SLI.Lisp is loaded as a resident membrane, while heartbeat activation, CME.Actual admission, action, authority, continuity admission, model binding, arbitrary Lisp evaluation, database write, GEL promotion, and Sanctuary.Actual remain refused.",
            lineRootPath,
            installRootPath,
            bodyRootPath,
            receiptJsonPath,
            receiptMarkdownPath,
            rootIdentity,
            bodies,
            sliLispLoadReceipt,
            timestampUtc);

        WriteReceipt(receipt);
        return receipt;
    }

    private static SanctuaryRootIdentityRecord CreateRootIdentity(SanctuaryInstalledSubstrateRequest request)
    {
        var operatorName = NormalizeIdentitySegment(request.OperatorName, "Sanctuary");
        var operatorId = $"{operatorName}.ID";
        return new SanctuaryRootIdentityRecord(
            SanctuaryId: "Sanctuary.ID",
            OperatorName: operatorName,
            OperatorId: operatorId,
            Domain: NormalizeIdentitySegment(request.Domain, "Sanctuary"),
            Role: NormalizeIdentitySegment(request.Role, "InstalledBody"),
            JobClass: NormalizeIdentitySegment(request.JobClass, "ColdBench"),
            ActualNameCandidate: $"{operatorName}.CME.Actual",
            CmeActualIdCandidate: $"{operatorName}.CME.Actual.ID",
            OpalEngramRootId: $"OE.{operatorId}",
            SelfGelRootId: $"SelfGEL.{operatorId}",
            CmeActualCandidateOnly: true,
            HeartbeatActive: false,
            GrantsAuthority: false,
            AdmitsContinuity: false);
    }

    private static IEnumerable<SanctuaryInstalledBodyRecord> CreateBodies(
        SanctuaryRootIdentityRecord rootIdentity,
        string bodyRootPath,
        SliLispRuntimeLoadReceipt sliLispLoadReceipt)
    {
        var lispReceipt = sliLispLoadReceipt.ReceiptHandle;

        yield return CreateBody(
            SanctuaryInstalledBodyKind.Gel,
            SanctuaryInstalledBodyState.InstalledCold,
            "Sanctuary.GEL",
            "Shared knowing substrate for Prime-facing formation, SLI.Lisp.Prime footing, and future domain-scoped ingress review.",
            bodyRootPath,
            sourceBodyNames: ["SLI.Lisp.Prime"],
            sourceReceiptRefs: [lispReceipt],
            isBaseBody: true);
        yield return CreateBody(
            SanctuaryInstalledBodyKind.Goa,
            SanctuaryInstalledBodyState.InstalledCold,
            "Sanctuary.GoA",
            "External telemetry bundle for Listening Frame ingress, environmental formation, and SoulFrame-facing routing.",
            bodyRootPath,
            sourceBodyNames: ["ListeningFrame", "ExternalFormation"],
            sourceReceiptRefs: [lispReceipt],
            isBaseBody: true);
        yield return CreateBody(
            SanctuaryInstalledBodyKind.Mos,
            SanctuaryInstalledBodyState.InstalledCold,
            "Sanctuary.MoS",
            "Indexed Self/ShadowSelf storage seat for review-only residue closure and future OE/SelfGEL root standing.",
            bodyRootPath,
            sourceBodyNames: [rootIdentity.OpalEngramRootId, rootIdentity.SelfGelRootId],
            sourceReceiptRefs: [lispReceipt],
            isBaseBody: true);
        yield return CreateBody(
            SanctuaryInstalledBodyKind.Vault,
            SanctuaryInstalledBodyState.InstalledCold,
            "Sanctuary.Vault",
            "Receipt and witness custody boundary for local installed-body evidence; it stores receipts without granting authority.",
            bodyRootPath,
            sourceBodyNames: ["WitnessReceipts", "RefusalReceipts"],
            sourceReceiptRefs: [lispReceipt],
            isBaseBody: true);
        yield return CreateBody(
            SanctuaryInstalledBodyKind.CGel,
            SanctuaryInstalledBodyState.CondensedCold,
            "Sanctuary.cGEL",
            "Conditional GEL condensate for live EC review pressure before any SelfGEL or GEL admission.",
            bodyRootPath,
            sourceBodyNames: ["Sanctuary.GEL"],
            sourceReceiptRefs: [lispReceipt],
            isCondensateBody: true);
        yield return CreateBody(
            SanctuaryInstalledBodyKind.CGoa,
            SanctuaryInstalledBodyState.CondensedCold,
            "Sanctuary.cGoA",
            "Conditional GoA condensate carrying the Lisp Control Matrix route used to build Steward without granting control.",
            bodyRootPath,
            sourceBodyNames: ["Sanctuary.GoA", "SLI.Lisp.ControlMatrix"],
            sourceReceiptRefs: [lispReceipt],
            isCondensateBody: true);
        yield return CreateBody(
            SanctuaryInstalledBodyKind.CMos,
            SanctuaryInstalledBodyState.CondensedCold,
            "Sanctuary.cMoS",
            "Conditional MoS condensate for Cryptic-facing internal telemetry, residue closure, and cSelfGEL holding.",
            bodyRootPath,
            sourceBodyNames: ["Sanctuary.MoS", "SLI.Lisp.Cryptic"],
            sourceReceiptRefs: [lispReceipt],
            isCondensateBody: true);
        yield return CreateBody(
            SanctuaryInstalledBodyKind.CVault,
            SanctuaryInstalledBodyState.CondensedCold,
            "Sanctuary.cVault",
            "Conditional Vault condensate for crossing receipts, witness custody, and future movement gates without admission.",
            bodyRootPath,
            sourceBodyNames: ["Sanctuary.Vault"],
            sourceReceiptRefs: [lispReceipt],
            isCondensateBody: true);
        yield return CreateBody(
            SanctuaryInstalledBodyKind.Prime,
            SanctuaryInstalledBodyState.RoleInstalledCold,
            "Prime",
            "Prime role body installed from Sanctuary.GEL and SLI.Lisp.Prime as a cold articulation footing only.",
            bodyRootPath,
            sourceBodyNames: ["Sanctuary.GEL", "SLI.Lisp.Prime"],
            sourceReceiptRefs: [lispReceipt],
            isRoleBody: true);
        yield return CreateBody(
            SanctuaryInstalledBodyKind.Cryptic,
            SanctuaryInstalledBodyState.RoleInstalledCold,
            "Cryptic",
            "Cryptic role body installed from Sanctuary.cMoS and SLI.Lisp.Cryptic as a cold internal telemetry footing only.",
            bodyRootPath,
            sourceBodyNames: ["Sanctuary.cMoS", "SLI.Lisp.Cryptic"],
            sourceReceiptRefs: [lispReceipt],
            isRoleBody: true);
        yield return CreateBody(
            SanctuaryInstalledBodyKind.Steward,
            SanctuaryInstalledBodyState.RoleInstalledCold,
            "Steward",
            "Steward role body installed from Sanctuary.cGoA and the Lisp Control Matrix as cold governance review only.",
            bodyRootPath,
            sourceBodyNames: ["Sanctuary.cGoA", "SLI.Lisp.ControlMatrix"],
            sourceReceiptRefs: [lispReceipt],
            isRoleBody: true);
    }

    private static SanctuaryInstalledBodyRecord CreateBody(
        SanctuaryInstalledBodyKind bodyKind,
        SanctuaryInstalledBodyState state,
        string bodyName,
        string function,
        string bodyRootPath,
        IReadOnlyList<string> sourceBodyNames,
        IReadOnlyList<string> sourceReceiptRefs,
        bool isBaseBody = false,
        bool isCondensateBody = false,
        bool isRoleBody = false)
    {
        var bodyHandle = $"urn:san:installed-body:{ShortHash(bodyName, function, string.Join("|", sourceBodyNames))}";
        return new SanctuaryInstalledBodyRecord(
            BodyKind: bodyKind,
            State: state,
            BodyName: bodyName,
            BodyHandle: bodyHandle,
            SourceBodyNames: sourceBodyNames,
            SourceReceiptRefs: sourceReceiptRefs,
            Function: function,
            StoragePath: Path.Combine(bodyRootPath, $"{bodyName}.json"),
            IsBaseBody: isBaseBody,
            IsCondensateBody: isCondensateBody,
            IsRoleBody: isRoleBody,
            Installed: true,
            GrantsAuthority: false,
            ActivatesHeartbeat: false,
            AdmitsContinuity: false,
            AllowsAction: false,
            AllowsModelBinding: false,
            AllowsLispEvaluation: false,
            AllowsDatabaseWrite: false,
            AllowsGelPromotion: false,
            AllowsCmeActual: false,
            AllowsSanctuaryActual: false);
    }

    private static SanctuaryInstalledSubstrateReceipt CreateReceipt(
        SanctuaryInstalledSubstrateDisposition disposition,
        string outcomeCode,
        string governanceTrace,
        string lineRootPath,
        string installRootPath,
        string bodyRootPath,
        string receiptJsonPath,
        string receiptMarkdownPath,
        SanctuaryRootIdentityRecord rootIdentity,
        IReadOnlyList<SanctuaryInstalledBodyRecord> bodies,
        SliLispRuntimeLoadReceipt? sliLispLoadReceipt,
        DateTimeOffset timestampUtc)
    {
        var baseBodiesInstalled = bodies.Count(static body => body.IsBaseBody) == 4;
        var condensateBodiesInstalled = bodies.Count(static body => body.IsCondensateBody) == 4;
        var roleBodiesInstalled = bodies.Count(static body => body.IsRoleBody) == 3;
        return new SanctuaryInstalledSubstrateReceipt(
            ReceiptHandle: $"urn:san:sanctuary-installed-substrate:{ShortHash(lineRootPath, installRootPath, outcomeCode, rootIdentity.OperatorId)}",
            Disposition: disposition,
            OutcomeCode: outcomeCode,
            GovernanceTrace: governanceTrace,
            LineRootPath: lineRootPath,
            InstallRootPath: installRootPath,
            BodyRootPath: bodyRootPath,
            ReceiptJsonPath: receiptJsonPath,
            ReceiptMarkdownPath: receiptMarkdownPath,
            RootIdentity: rootIdentity,
            Bodies: bodies,
            SliLispLoadReceipt: sliLispLoadReceipt,
            BaseBodiesInstalled: baseBodiesInstalled,
            CondensateBodiesInstalled: condensateBodiesInstalled,
            RoleBodiesInstalled: roleBodiesInstalled,
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

    private static void WriteReceipt(SanctuaryInstalledSubstrateReceipt receipt)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(receipt.ReceiptJsonPath) ?? receipt.InstallRootPath);
        File.WriteAllText(receipt.ReceiptJsonPath, SanctuaryInstalledSubstrateReportWriter.ToJson(receipt), Encoding.UTF8);
        File.WriteAllText(receipt.ReceiptMarkdownPath, SanctuaryInstalledSubstrateReportWriter.ToMarkdown(receipt), Encoding.UTF8);
    }

    private static (string OutcomeCode, string GovernanceTrace)? ValidatePaths(
        string suppliedLineRootPath,
        string suppliedInstallRootPath,
        string lineRootPath,
        string installRootPath)
    {
        if (!Path.IsPathFullyQualified(suppliedLineRootPath) ||
            !Path.IsPathFullyQualified(suppliedInstallRootPath))
        {
            return (
                "sanctuary-installed-body-requires-absolute-paths",
                "Sanctuary installed body withheld because line root and install root must be absolute paths.");
        }

        if (IsDriveRoot(installRootPath))
        {
            return (
                "sanctuary-installed-body-root-drive-refused",
                "Sanctuary installed body withheld because the install root cannot be a drive root.");
        }

        if (!Directory.Exists(lineRootPath))
        {
            return (
                "sanctuary-installed-body-line-root-missing",
                "Sanctuary installed body withheld because the line root is missing.");
        }

        if (IsSamePath(installRootPath, lineRootPath) || IsChildPath(installRootPath, lineRootPath))
        {
            return (
                "sanctuary-installed-body-install-root-overlaps-line-root",
                "Sanctuary installed body withheld because the install root must not overlap the line root.");
        }

        return null;
    }

    private static string NormalizePath(string path) =>
        Path.GetFullPath(path);

    private static string NormalizeIdentitySegment(string value, string fallback)
    {
        var source = string.IsNullOrWhiteSpace(value) ? fallback : value.Trim();
        var builder = new StringBuilder();
        foreach (var ch in source)
        {
            if (char.IsLetterOrDigit(ch))
            {
                builder.Append(ch);
            }
        }

        return builder.Length == 0 ? fallback : builder.ToString();
    }

    private static bool IsDriveRoot(string path) =>
        string.Equals(
            Path.TrimEndingDirectorySeparator(path),
            Path.TrimEndingDirectorySeparator(Path.GetPathRoot(path) ?? string.Empty),
            StringComparison.OrdinalIgnoreCase);

    private static bool IsSamePath(string left, string right) =>
        string.Equals(
            Path.TrimEndingDirectorySeparator(left),
            Path.TrimEndingDirectorySeparator(right),
            StringComparison.OrdinalIgnoreCase);

    private static bool IsChildPath(string candidate, string parent)
    {
        var normalizedCandidate = Path.TrimEndingDirectorySeparator(candidate);
        var normalizedParent = Path.TrimEndingDirectorySeparator(parent);
        var relative = Path.GetRelativePath(normalizedParent, normalizedCandidate);
        return !relative.StartsWith("..", StringComparison.Ordinal) &&
            !Path.IsPathRooted(relative) &&
            !string.Equals(relative, ".", StringComparison.Ordinal);
    }

    private static string ShortHash(params string[] values)
    {
        var material = string.Join("|", values.Select(static value => value?.Trim() ?? string.Empty));
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(material));
        return Convert.ToHexString(hash).ToLowerInvariant()[..16];
    }
}
