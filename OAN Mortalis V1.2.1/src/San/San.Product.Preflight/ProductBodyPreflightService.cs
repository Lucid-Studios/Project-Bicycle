using System.Text.Json;
using San.Nexus.Control;
using SLI.Engine;
using SLI.Runtime;

namespace San.Product.Preflight;

public interface IProductBodyPreflightService
{
    ProductBodyPreflightStatus Evaluate(
        ProductBodyPreflightRequest request,
        DateTimeOffset timestampUtc);
}

public sealed class DefaultProductBodyPreflightService : IProductBodyPreflightService
{
    public const string DefaultNextAllowedLane = "current_run_pointing_cold_truth_package_plan_review_only";
    public const string LabSanctuaryNextAllowedLane = "lab_sanctuary_current_run_pointing_cold_truth_package_plan_review_only";

    private static readonly string[] RequiredLabContextFiles =
    [
        "LAB_CONTEXT_ANCHOR.md",
        "LAB_POSTURE_ACTIVE.md",
        Path.Combine("Domain Universal", "domain-universal-index.csv"),
        "drive-file-index.csv"
    ];

    private static readonly string[] RequiredLabSanctuaryDocs =
    [
        "SANCTUARY_ROOT_INSTALL_SURFACE.md",
        "SANCTUARY_PRE_GOVERNING_STANDING_BOUNDARY.md",
        "SANCTUARYID_GOA_GOVERNING_CME_SET_LAW.md",
        "CME_PLACEMENT_WITHHELD_LAW.md",
        "LAB_MIXED_DATA_PRE_AWAKENING_PROOF_RUN_BOUNDARY.md",
        "LAB_DATA_INVENTORY_SCHEMA_BOUNDARY.md",
        "LAB_DATA_INVENTORY_EVALUATION_POSTURE_BOUNDARY.md",
        "CME_SEED_HARNESS_THOUGHT_FIELD_SNAPSHOT.md"
    ];

    public ProductBodyPreflightStatus Evaluate(
        ProductBodyPreflightRequest request,
        DateTimeOffset timestampUtc)
    {
        ArgumentNullException.ThrowIfNull(request);

        var checks = new List<ProductBodyCheck>();
        var lineRootPath = NormalizePath(request.LineRootPath);

        if (string.IsNullOrWhiteSpace(lineRootPath) ||
            !Directory.Exists(lineRootPath))
        {
            checks.Add(new ProductBodyCheck(
                "line-root-present",
                ProductBodyCheckStatus.Fail,
                "line root is missing or unreadable."));

            return CreateStatus(
                request,
                lineRootPath,
                manifest: null,
                checks,
                ProductBodyPreflightDisposition.Withheld,
                "line-root-missing",
                "product body preflight requires a readable line root.",
                timestampUtc);
        }

        checks.Add(new ProductBodyCheck(
            "line-root-present",
            ProductBodyCheckStatus.Pass,
            lineRootPath));

        var manifestPath = Path.Combine(lineRootPath, "build", "line-manifest.json");
        var manifest = TryReadManifest(manifestPath, checks);
        if (manifest is null)
        {
            return CreateStatus(
                request,
                lineRootPath,
                manifest,
                checks,
                ProductBodyPreflightDisposition.Withheld,
                "line-manifest-missing-or-invalid",
                "product body preflight requires a readable line manifest.",
                timestampUtc);
        }

        var solutionPath = Path.Combine(lineRootPath, manifest.SolutionPath);
        checks.Add(new ProductBodyCheck(
            "solution-present",
            File.Exists(solutionPath) ? ProductBodyCheckStatus.Pass : ProductBodyCheckStatus.Fail,
            solutionPath));

        var parentLinePath = Path.Combine(Directory.GetParent(lineRootPath)?.FullName ?? string.Empty, manifest.ParentLine);
        checks.Add(new ProductBodyCheck(
            "parent-line-present",
            Directory.Exists(parentLinePath) ? ProductBodyCheckStatus.Pass : ProductBodyCheckStatus.Fail,
            parentLinePath));

        var currentRunPointingColdTruth =
            string.Equals(manifest.LineName, "OAN Mortalis V1.2.1", StringComparison.Ordinal) &&
            string.Equals(manifest.ParentLine, "OAN Mortalis V1.1.1", StringComparison.Ordinal) &&
            string.Equals(manifest.ActiveExecutableTruth, "OAN Mortalis V1.2.1", StringComparison.Ordinal);
        checks.Add(new ProductBodyCheck(
            "v121-current-run-pointing-cold-truth-preserved",
            currentRunPointingColdTruth ? ProductBodyCheckStatus.Pass : ProductBodyCheckStatus.Fail,
            $"line={manifest.LineName}; parent={manifest.ParentLine}; activeTruth={manifest.ActiveExecutableTruth}"));

        var coldCorridorPresent = HasColdCorridorPolicySurface();
        checks.Add(new ProductBodyCheck(
            "cold-corridor-policy-surface-present",
            coldCorridorPresent ? ProductBodyCheckStatus.Pass : ProductBodyCheckStatus.Fail,
            "SW-04 receipt passage, SW-05 inert Lisp membrane, SW-06 governed return replay, and SW-07 refusal policy surfaces are loadable."));

        var lineIsRunPointingColdTruth =
            currentRunPointingColdTruth &&
            manifest.Buildable &&
            manifest.SourceMaterialized &&
            !manifest.RuntimeMaterialized;
        checks.Add(new ProductBodyCheck(
            "line-is-run-pointing-cold-truth",
            lineIsRunPointingColdTruth ? ProductBodyCheckStatus.Pass : ProductBodyCheckStatus.Fail,
            $"buildable={manifest.Buildable}; sourceMaterialized={manifest.SourceMaterialized}; runtimeMaterialized={manifest.RuntimeMaterialized}"));

        if (IsLabSanctuaryBuildTestingProfile(request))
        {
            AddLabSanctuaryBuildTestingChecks(request, lineRootPath, checks);
        }

        var activationAuthorityPresent = false;
        var anyFailed = checks.Any(static check => check.Status == ProductBodyCheckStatus.Fail);
        if (request.RequestsRuntimeMotion)
        {
            return CreateStatus(
                request,
                lineRootPath,
                manifest,
                checks,
                ProductBodyPreflightDisposition.Refused,
                "activation-authority-absent",
                "current run-pointing cold truth refused activation because V1.2.1 has no runtime authority, model binding, database write, GEL promotion, CME.Actual, or Sanctuary.Actual authorization.",
                timestampUtc,
                activationAuthorityPresent);
        }

        var isLabSanctuaryProfile = IsLabSanctuaryBuildTestingProfile(request);
        return CreateStatus(
            request,
            lineRootPath,
            manifest,
            checks,
            anyFailed ? ProductBodyPreflightDisposition.Withheld : ProductBodyPreflightDisposition.VerifiedCold,
            ResolveOutcomeCode(anyFailed, isLabSanctuaryProfile),
            anyFailed
                ? ResolveIncompleteTrace(isLabSanctuaryProfile)
                : ResolveVerifiedTrace(isLabSanctuaryProfile),
            timestampUtc,
            activationAuthorityPresent);
    }

    public static string ResolveLineRoot(string? candidatePath = null)
    {
        if (!string.IsNullOrWhiteSpace(candidatePath))
        {
            return NormalizePath(candidatePath);
        }

        var current = new DirectoryInfo(Environment.CurrentDirectory);
        while (current is not null)
        {
            if (File.Exists(Path.Combine(current.FullName, "build", "line-manifest.json")) ||
                File.Exists(Path.Combine(current.FullName, "San.sln")))
            {
                return current.FullName;
            }

            current = current.Parent;
        }

        return Environment.CurrentDirectory;
    }

    private static ProductBodyPreflightStatus CreateStatus(
        ProductBodyPreflightRequest request,
        string lineRootPath,
        LineManifestSummary? manifest,
        IReadOnlyList<ProductBodyCheck> checks,
        ProductBodyPreflightDisposition disposition,
        string outcomeCode,
        string governanceTrace,
        DateTimeOffset timestampUtc,
        bool activationAuthorityPresent = false)
    {
        var activationRefused = !activationAuthorityPresent;
        var verificationSettingPath = ResolveVerificationSettingPath(request, lineRootPath);
        var labContextRootPath = ResolveLabContextRootPath(request, lineRootPath);
        var buildTestingPointerPath = ResolveBuildTestingPointerPath(request, lineRootPath);

        return new ProductBodyPreflightStatus(
            StatusHandle: $"product-body-preflight://{Math.Abs(HashCode.Combine(lineRootPath, outcomeCode, timestampUtc.UtcTicks)):x}",
            Disposition: disposition,
            OutcomeCode: outcomeCode,
            GovernanceTrace: governanceTrace,
            LineRootPath: lineRootPath,
            VerificationProfile: string.IsNullOrWhiteSpace(request.VerificationProfile)
                ? ProductBodyVerificationProfiles.ColdProductBody
                : request.VerificationProfile,
            VerificationSettingPath: verificationSettingPath,
            LabContextRootPath: labContextRootPath,
            BuildTestingPointerPath: buildTestingPointerPath,
            Manifest: manifest,
            RetainedParentPreserved: manifest is not null &&
                string.Equals(manifest.ParentLine, "OAN Mortalis V1.1.1", StringComparison.Ordinal),
            SidecarPreserved: manifest is not null &&
                string.Equals(manifest.ParentLine, "OAN Mortalis V1.1.1", StringComparison.Ordinal) &&
                string.Equals(manifest.ActiveExecutableTruth, "OAN Mortalis V1.1.1", StringComparison.Ordinal),
            Buildable: manifest?.Buildable == true,
            SourceMaterialized: manifest?.SourceMaterialized == true,
            RuntimeMaterialized: manifest?.RuntimeMaterialized == true,
            SolutionPresent: checks.Any(static check => check.CheckId == "solution-present" && check.Status == ProductBodyCheckStatus.Pass),
            ParentLinePresent: checks.Any(static check => check.CheckId == "parent-line-present" && check.Status == ProductBodyCheckStatus.Pass),
            ColdCorridorPresent: checks.Any(static check => check.CheckId == "cold-corridor-policy-surface-present" && check.Status == ProductBodyCheckStatus.Pass),
            ActivationAuthorityPresent: activationAuthorityPresent,
            ActivationRefused: activationRefused,
            RefusalCode: activationRefused ? "activation-authority-absent" : "activation-authority-present",
            NextAllowedLane: IsLabSanctuaryBuildTestingProfile(request) ? LabSanctuaryNextAllowedLane : DefaultNextAllowedLane,
            Checks: checks.ToArray(),
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

    private static LineManifestSummary? TryReadManifest(
        string manifestPath,
        ICollection<ProductBodyCheck> checks)
    {
        if (!File.Exists(manifestPath))
        {
            checks.Add(new ProductBodyCheck(
                "line-manifest-present",
                ProductBodyCheckStatus.Fail,
                manifestPath));
            return null;
        }

        try
        {
            using var stream = File.OpenRead(manifestPath);
            using var document = JsonDocument.Parse(stream);
            var root = document.RootElement;
            var manifest = new LineManifestSummary(
                LineName: ReadString(root, "lineName"),
                LineVersion: ReadString(root, "lineVersion"),
                Posture: ReadString(root, "posture"),
                SolutionPath: ReadString(root, "solutionPath", "San.sln"),
                ParentLine: ReadString(root, "parentLine"),
                ActiveExecutableTruth: ReadString(root, "activeExecutableTruth"),
                Buildable: ReadBool(root, "buildable"),
                SourceMaterialized: ReadBool(root, "sourceMaterialized"),
                RuntimeMaterialized: ReadBool(root, "runtimeMaterialized"));

            checks.Add(new ProductBodyCheck(
                "line-manifest-present",
                ProductBodyCheckStatus.Pass,
                manifestPath));
            return manifest;
        }
        catch (JsonException ex)
        {
            checks.Add(new ProductBodyCheck(
                "line-manifest-parse",
                ProductBodyCheckStatus.Fail,
                ex.Message));
            return null;
        }
    }

    private static bool HasColdCorridorPolicySurface() =>
        typeof(DefaultSliCmeActualOrchestrationReceiptPassagePolicy).Assembly is not null &&
        typeof(DefaultSliLispInertMembranePolicy).Assembly is not null &&
        typeof(DefaultGovernedReturnReceiptReplayPolicy).Assembly is not null &&
        typeof(DefaultFormationPathRefusalPolicy).Assembly is not null;

    private static string ReadString(JsonElement root, string propertyName, string fallback = "") =>
        root.TryGetProperty(propertyName, out var value) && value.ValueKind == JsonValueKind.String
            ? value.GetString() ?? fallback
            : fallback;

    private static bool ReadBool(JsonElement root, string propertyName) =>
        root.TryGetProperty(propertyName, out var value) &&
        value.ValueKind == JsonValueKind.True;

    private static void AddLabSanctuaryBuildTestingChecks(
        ProductBodyPreflightRequest request,
        string lineRootPath,
        ICollection<ProductBodyCheck> checks)
    {
        var verificationSettingPath = ResolveVerificationSettingPath(request, lineRootPath);
        AddJsonFileCheck(
            checks,
            "lab-sanctuary-verification-setting-loadable",
            verificationSettingPath,
            root =>
                root.TryGetProperty("settingId", out var settingId) &&
                string.Equals(settingId.GetString(), ProductBodyVerificationProfiles.LabSanctuaryBuildTesting, StringComparison.Ordinal) &&
                root.TryGetProperty("activationAuthorityRequired", out var activationAuthorityRequired) &&
                activationAuthorityRequired.ValueKind == JsonValueKind.False);

        var labContextRootPath = ResolveLabContextRootPath(request, lineRootPath);
        checks.Add(new ProductBodyCheck(
            "lab-context-root-present",
            Directory.Exists(labContextRootPath) ? ProductBodyCheckStatus.Pass : ProductBodyCheckStatus.Fail,
            labContextRootPath));

        foreach (var requiredFile in RequiredLabContextFiles)
        {
            var requiredPath = Path.Combine(labContextRootPath, requiredFile);
            checks.Add(new ProductBodyCheck(
                $"lab-context-file-present:{requiredFile.Replace('\\', '/')}",
                File.Exists(requiredPath) ? ProductBodyCheckStatus.Pass : ProductBodyCheckStatus.Fail,
                requiredPath));
        }

        var buildTestingPointerPath = ResolveBuildTestingPointerPath(request, lineRootPath);
        AddJsonFileCheck(
            checks,
            "lab-build-testing-pointer-loadable",
            buildTestingPointerPath,
            root => IsCurrentRunPointingColdTruthPointer(root, lineRootPath));

        foreach (var requiredDoc in RequiredLabSanctuaryDocs)
        {
            var requiredPath = Path.Combine(lineRootPath, "docs", requiredDoc);
            checks.Add(new ProductBodyCheck(
                $"lab-sanctuary-doc-present:{requiredDoc}",
                File.Exists(requiredPath) ? ProductBodyCheckStatus.Pass : ProductBodyCheckStatus.Fail,
                requiredPath));
        }

        checks.Add(new ProductBodyCheck(
            "lab-sanctuary-remains-non-activating",
            ProductBodyCheckStatus.Pass,
            "Lab Sanctuary verification proves package/readiness surfaces only; activation, model binding, database write, GEL promotion, CME.Actual, and Sanctuary.Actual remain refused."));
    }

    private static void AddJsonFileCheck(
        ICollection<ProductBodyCheck> checks,
        string checkId,
        string path,
        Func<JsonElement, bool> predicate)
    {
        if (!File.Exists(path))
        {
            checks.Add(new ProductBodyCheck(
                checkId,
                ProductBodyCheckStatus.Fail,
                path));
            return;
        }

        try
        {
            using var stream = File.OpenRead(path);
            using var document = JsonDocument.Parse(stream);
            checks.Add(new ProductBodyCheck(
                checkId,
                predicate(document.RootElement) ? ProductBodyCheckStatus.Pass : ProductBodyCheckStatus.Fail,
                path));
        }
        catch (JsonException ex)
        {
            checks.Add(new ProductBodyCheck(
                checkId,
                ProductBodyCheckStatus.Fail,
                ex.Message));
        }
    }

    private static bool IsLabSanctuaryBuildTestingProfile(ProductBodyPreflightRequest request) =>
        string.Equals(
            request.VerificationProfile,
            ProductBodyVerificationProfiles.LabSanctuaryBuildTesting,
            StringComparison.OrdinalIgnoreCase);

    private static string ResolveOutcomeCode(bool anyFailed, bool isLabSanctuaryProfile)
    {
        if (isLabSanctuaryProfile)
        {
            return anyFailed
                ? "lab-sanctuary-build-verification-incomplete"
                : "lab-sanctuary-build-verification-verified-cold";
        }

        return anyFailed
            ? "cold-product-body-preflight-incomplete"
            : "cold-product-body-preflight-verified";
    }

    private static string ResolveIncompleteTrace(bool isLabSanctuaryProfile) =>
        isLabSanctuaryProfile
            ? "Lab Sanctuary build verification is withheld until failed lab, pointer, or Sanctuary surface checks are repaired."
            : "cold product body preflight is withheld until failed checks are repaired.";

    private static string ResolveVerifiedTrace(bool isLabSanctuaryProfile) =>
        isLabSanctuaryProfile
            ? "Lab Sanctuary build testing profile verified the current run-pointing cold truth, retained parent, lab context anchors, build-testing pointer, and Sanctuary formation surfaces; activation remains refused by design."
            : "current run-pointing cold truth is verified as a buildable cold product body; activation remains refused by design.";

    private static bool IsCurrentRunPointingColdTruthPointer(
        JsonElement root,
        string lineRootPath)
    {
        if (!root.TryGetProperty("activeBuildLine", out var activeBuildLine) ||
            !string.Equals(NormalizePath(activeBuildLine.GetString()), lineRootPath, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        if (!root.TryGetProperty("retainedParentBuildLine", out var retainedParentBuildLine) ||
            !Directory.Exists(NormalizePath(retainedParentBuildLine.GetString())))
        {
            return false;
        }

        return root.TryGetProperty("activePosture", out var activePosture) &&
            File.Exists(NormalizePath(activePosture.GetString()));
    }

    private static string ResolveVerificationSettingPath(
        ProductBodyPreflightRequest request,
        string lineRootPath) =>
        NormalizePath(request.VerificationSettingPath) is { Length: > 0 } settingPath
            ? settingPath
            : NormalizePath(Path.Combine(lineRootPath, "build", "lab-sanctuary-verification-settings.json"));

    private static string ResolveLabContextRootPath(
        ProductBodyPreflightRequest request,
        string lineRootPath)
    {
        var explicitPath = NormalizePath(request.LabContextRootPath);
        if (!string.IsNullOrWhiteSpace(explicitPath))
        {
            return explicitPath;
        }

        var driveRoot = Path.GetPathRoot(lineRootPath);
        return string.IsNullOrWhiteSpace(driveRoot)
            ? string.Empty
            : Path.Combine(driveRoot, "Lab Context");
    }

    private static string ResolveBuildTestingPointerPath(
        ProductBodyPreflightRequest request,
        string lineRootPath)
    {
        var explicitPath = NormalizePath(request.BuildTestingPointerPath);
        if (!string.IsNullOrWhiteSpace(explicitPath))
        {
            return explicitPath;
        }

        var stackRoot = Directory.GetParent(lineRootPath)?.FullName;
        return string.IsNullOrWhiteSpace(stackRoot)
            ? string.Empty
            : Path.Combine(stackRoot, "runtime", "LAB_BUILD_PREDICATE_COMPILER.pointer.json");
    }

    private static string NormalizePath(string? path) =>
        string.IsNullOrWhiteSpace(path)
            ? string.Empty
            : Path.GetFullPath(path);
}
