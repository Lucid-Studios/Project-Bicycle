using System.Security.Cryptography;
using System.Text;

namespace San.Product.Preflight;

public interface IProductBodyInstallService
{
    ProductBodyInstallReceipt Install(
        ProductBodyInstallRequest request,
        DateTimeOffset timestampUtc);
}

public sealed class DefaultProductBodyInstallService : IProductBodyInstallService
{
    public const string DefaultInstallDirectoryName = "Sanctuary";

    public static string ResolveDefaultInstallRoot(string lineRootPath)
    {
        var driveRoot = Path.GetPathRoot(lineRootPath);
        return string.IsNullOrWhiteSpace(driveRoot)
            ? Path.Combine(Environment.CurrentDirectory, DefaultInstallDirectoryName)
            : Path.Combine(driveRoot, DefaultInstallDirectoryName);
    }

    public ProductBodyInstallReceipt Install(
        ProductBodyInstallRequest request,
        DateTimeOffset timestampUtc)
    {
        ArgumentNullException.ThrowIfNull(request);

        var suppliedLineRootPath = request.LineRootPath;
        var suppliedInstallRootPath = request.InstallRootPath;
        var suppliedProductSourceRootPath = request.ProductSourceRootPath;
        var lineRootPath = NormalizePath(suppliedLineRootPath);
        var installRootPath = NormalizePath(suppliedInstallRootPath);
        var productSourceRootPath = NormalizePath(suppliedProductSourceRootPath);
        var productInstallRootPath = Path.Combine(installRootPath, "product");
        var buildInstallRootPath = Path.Combine(installRootPath, "build");
        var receiptRootPath = Path.Combine(installRootPath, "receipts");
        var preflightReceiptRootPath = Path.Combine(receiptRootPath, "preflight");
        var productExecutablePath = Path.Combine(productInstallRootPath, "San.Launcher.exe");
        var commandShimPath = Path.Combine(installRootPath, "sanctuary.cmd");
        var powerShellShimPath = Path.Combine(installRootPath, "sanctuary.ps1");
        var preflightReceiptJsonPath = Path.Combine(preflightReceiptRootPath, "product-body-status.json");
        var preflightReceiptMarkdownPath = Path.Combine(preflightReceiptRootPath, "product-body-status.md");

        var preflightStatus = EvaluatePreflight(request, lineRootPath, timestampUtc);
        if (request.RequestsRuntimeMotion)
        {
            return CreateReceipt(
                ProductBodyInstallDisposition.Refused,
                "install-runtime-motion-refused",
                "Local Sanctuary install refused because install-state creation cannot request activation, model binding, runtime identity, runtime action, database write, GEL promotion, CME.Actual, or Sanctuary.Actual.",
                lineRootPath,
                installRootPath,
                productSourceRootPath,
                productInstallRootPath,
                buildInstallRootPath,
                receiptRootPath,
                productExecutablePath,
                commandShimPath,
                powerShellShimPath,
                preflightReceiptJsonPath,
                preflightReceiptMarkdownPath,
                preflightStatus,
                copiedProductFileCount: 0,
                coldBuildToolSurfaceReady: false,
                timestampUtc);
        }

        var pathValidation = ValidatePaths(
            suppliedLineRootPath,
            suppliedInstallRootPath,
            suppliedProductSourceRootPath,
            lineRootPath,
            installRootPath,
            productSourceRootPath);
        if (pathValidation is not null)
        {
            return CreateReceipt(
                ProductBodyInstallDisposition.Withheld,
                pathValidation.Value.OutcomeCode,
                pathValidation.Value.GovernanceTrace,
                lineRootPath,
                installRootPath,
                productSourceRootPath,
                productInstallRootPath,
                buildInstallRootPath,
                receiptRootPath,
                productExecutablePath,
                commandShimPath,
                powerShellShimPath,
                preflightReceiptJsonPath,
                preflightReceiptMarkdownPath,
                preflightStatus,
                copiedProductFileCount: 0,
                coldBuildToolSurfaceReady: false,
                timestampUtc);
        }

        if (preflightStatus.Disposition != ProductBodyPreflightDisposition.VerifiedCold)
        {
            return CreateReceipt(
                ProductBodyInstallDisposition.Withheld,
                "install-preflight-not-verified-cold",
                $"Local Sanctuary install withheld because preflight did not verify cold: {preflightStatus.OutcomeCode}.",
                lineRootPath,
                installRootPath,
                productSourceRootPath,
                productInstallRootPath,
                buildInstallRootPath,
                receiptRootPath,
                productExecutablePath,
                commandShimPath,
                powerShellShimPath,
                preflightReceiptJsonPath,
                preflightReceiptMarkdownPath,
                preflightStatus,
                copiedProductFileCount: 0,
                coldBuildToolSurfaceReady: false,
                timestampUtc);
        }

        Directory.CreateDirectory(productInstallRootPath);
        Directory.CreateDirectory(buildInstallRootPath);
        Directory.CreateDirectory(receiptRootPath);
        Directory.CreateDirectory(preflightReceiptRootPath);

        var copiedProductFileCount = CopyDirectory(productSourceRootPath, productInstallRootPath);
        CopyBuildFile(lineRootPath, buildInstallRootPath, "line-manifest.json");
        CopyBuildFile(lineRootPath, buildInstallRootPath, "lab-sanctuary-verification-settings.json");
        WriteCommandShim(commandShimPath, lineRootPath);
        WritePowerShellShim(powerShellShimPath, lineRootPath);
        File.WriteAllText(preflightReceiptJsonPath, ProductBodyReportWriter.ToJson(preflightStatus), Encoding.UTF8);
        File.WriteAllText(preflightReceiptMarkdownPath, ProductBodyReportWriter.ToMarkdown(preflightStatus), Encoding.UTF8);

        var receipt = CreateReceipt(
            ProductBodyInstallDisposition.InstalledCold,
            "local-sanctuary-install-verified-cold",
            "Local Sanctuary install created a cold build tool surface. The installed launcher can execute verification and refusal commands while the standalone tool body remains anchored to the tool root; activation remains refused.",
            lineRootPath,
            installRootPath,
            productSourceRootPath,
            productInstallRootPath,
            buildInstallRootPath,
            receiptRootPath,
            productExecutablePath,
            commandShimPath,
            powerShellShimPath,
            preflightReceiptJsonPath,
            preflightReceiptMarkdownPath,
            preflightStatus,
            copiedProductFileCount,
            coldBuildToolSurfaceReady: File.Exists(productExecutablePath) && File.Exists(commandShimPath) && File.Exists(powerShellShimPath),
            timestampUtc);

        File.WriteAllText(Path.Combine(installRootPath, "SANCTUARY_INSTALL_RECEIPT.json"), ProductBodyInstallReportWriter.ToJson(receipt), Encoding.UTF8);
        File.WriteAllText(Path.Combine(installRootPath, "SANCTUARY_INSTALL_RECEIPT.md"), ProductBodyInstallReportWriter.ToMarkdown(receipt), Encoding.UTF8);

        return receipt;
    }

    private static ProductBodyPreflightStatus EvaluatePreflight(
        ProductBodyInstallRequest request,
        string lineRootPath,
        DateTimeOffset timestampUtc)
    {
        var preflightRequest = new ProductBodyPreflightRequest(
            LineRootPath: lineRootPath,
            ActivationRequested: request.ActivationRequested,
            ModelBindingRequested: request.ModelBindingRequested,
            LispEvaluationRequested: request.LispEvaluationRequested,
            RuntimeIdentityRequested: request.RuntimeIdentityRequested,
            RuntimeActionRequested: request.RuntimeActionRequested,
            DatabaseWriteRequested: request.DatabaseWriteRequested,
            GelPromotionRequested: request.GelPromotionRequested,
            CmeActualRequested: request.CmeActualRequested,
            SanctuaryActualRequested: request.SanctuaryActualRequested,
            VerificationProfile: request.VerificationProfile,
            VerificationSettingPath: request.VerificationSettingPath,
            LabContextRootPath: request.LabContextRootPath,
            BuildTestingPointerPath: request.BuildTestingPointerPath);

        return new DefaultProductBodyPreflightService().Evaluate(preflightRequest, timestampUtc);
    }

    private static (string OutcomeCode, string GovernanceTrace)? ValidatePaths(
        string suppliedLineRootPath,
        string suppliedInstallRootPath,
        string suppliedProductSourceRootPath,
        string lineRootPath,
        string installRootPath,
        string productSourceRootPath)
    {
        if (!Path.IsPathFullyQualified(suppliedLineRootPath) ||
            !Path.IsPathFullyQualified(suppliedInstallRootPath) ||
            !Path.IsPathFullyQualified(suppliedProductSourceRootPath))
        {
            return (
                "install-requires-absolute-paths",
                "Local Sanctuary install withheld because line root, install root, and product source root must be absolute paths.");
        }

        if (IsDriveRoot(installRootPath))
        {
            return (
                "install-root-drive-root-refused",
                "Local Sanctuary install withheld because the install root cannot be a drive root.");
        }

        if (!Directory.Exists(lineRootPath))
        {
            return (
                "install-line-root-missing",
                "Local Sanctuary install withheld because the line root is missing.");
        }

        if (!Directory.Exists(productSourceRootPath))
        {
            return (
                "install-product-source-root-missing",
                "Local Sanctuary install withheld because the product source root is missing.");
        }

        var sourceExecutable = Path.Combine(productSourceRootPath, "San.Launcher.exe");
        if (!File.Exists(sourceExecutable))
        {
            return (
                "install-product-launcher-missing",
                "Local Sanctuary install withheld because the product source root does not contain San.Launcher.exe.");
        }

        if (IsSamePath(installRootPath, lineRootPath) ||
            IsSamePath(installRootPath, productSourceRootPath) ||
            IsChildPath(installRootPath, lineRootPath) ||
            IsChildPath(installRootPath, productSourceRootPath))
        {
            return (
                "install-root-must-not-overlap-source",
                "Local Sanctuary install withheld because the install root must not overlap the line root or product source root.");
        }

        return null;
    }

    private static int CopyDirectory(string sourceRoot, string destinationRoot)
    {
        var copied = 0;
        foreach (var sourcePath in Directory.EnumerateFiles(sourceRoot, "*", SearchOption.AllDirectories))
        {
            var relativePath = Path.GetRelativePath(sourceRoot, sourcePath);
            var destinationPath = Path.Combine(destinationRoot, relativePath);
            Directory.CreateDirectory(Path.GetDirectoryName(destinationPath) ?? destinationRoot);
            File.Copy(sourcePath, destinationPath, overwrite: true);
            copied += 1;
        }

        return copied;
    }

    private static void CopyBuildFile(string lineRootPath, string buildInstallRootPath, string fileName)
    {
        var sourcePath = Path.Combine(lineRootPath, "build", fileName);
        if (!File.Exists(sourcePath))
        {
            return;
        }

        File.Copy(sourcePath, Path.Combine(buildInstallRootPath, fileName), overwrite: true);
    }

    private static void WriteCommandShim(string path, string lineRootPath)
    {
        var content = $"""
            @echo off
            set "SANCTUARY_LINE_ROOT={lineRootPath}"
            set "SANCTUARY_REPORT_DIR=%~dp0receipts\preflight"
            "%~dp0product\San.Launcher.exe" %* --line-root "%SANCTUARY_LINE_ROOT%" --report-dir "%SANCTUARY_REPORT_DIR%"
            exit /b %ERRORLEVEL%
            """;

        File.WriteAllText(path, content.Replace("\n", Environment.NewLine), Encoding.ASCII);
    }

    private static void WritePowerShellShim(string path, string lineRootPath)
    {
        var content = $"""
            $SanctuaryLineRoot = '{lineRootPath.Replace("'", "''", StringComparison.Ordinal)}'
            $SanctuaryReportDir = Join-Path $PSScriptRoot 'receipts\preflight'
            & (Join-Path $PSScriptRoot 'product\San.Launcher.exe') @args --line-root $SanctuaryLineRoot --report-dir $SanctuaryReportDir
            exit $LASTEXITCODE
            """;

        File.WriteAllText(path, content.Replace("\n", Environment.NewLine), Encoding.UTF8);
    }

    private static ProductBodyInstallReceipt CreateReceipt(
        ProductBodyInstallDisposition disposition,
        string outcomeCode,
        string governanceTrace,
        string lineRootPath,
        string installRootPath,
        string productSourceRootPath,
        string productInstallRootPath,
        string buildInstallRootPath,
        string receiptRootPath,
        string productExecutablePath,
        string commandShimPath,
        string powerShellShimPath,
        string preflightReceiptJsonPath,
        string preflightReceiptMarkdownPath,
        ProductBodyPreflightStatus preflightStatus,
        int copiedProductFileCount,
        bool coldBuildToolSurfaceReady,
        DateTimeOffset timestampUtc) =>
        new(
            ReceiptHandle: $"urn:san:local-install:{ShortHash(lineRootPath, installRootPath, outcomeCode)}",
            Disposition: disposition,
            OutcomeCode: outcomeCode,
            GovernanceTrace: governanceTrace,
            LineRootPath: lineRootPath,
            InstallRootPath: installRootPath,
            ProductSourceRootPath: productSourceRootPath,
            ProductInstallRootPath: productInstallRootPath,
            BuildInstallRootPath: buildInstallRootPath,
            ReceiptRootPath: receiptRootPath,
            ProductExecutablePath: productExecutablePath,
            CommandShimPath: commandShimPath,
            PowerShellShimPath: powerShellShimPath,
            PreflightReceiptJsonPath: preflightReceiptJsonPath,
            PreflightReceiptMarkdownPath: preflightReceiptMarkdownPath,
            PreflightStatus: preflightStatus,
            CopiedProductFileCount: copiedProductFileCount,
            ColdBuildToolSurfaceReady: coldBuildToolSurfaceReady,
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
        return normalizedCandidate.StartsWith(
            normalizedParent + Path.DirectorySeparatorChar,
            StringComparison.OrdinalIgnoreCase);
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
