using System.ComponentModel;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace SLI.Lisp;

public interface ISliLispRuntimeLoadService
{
    SliLispRuntimeLoadReceipt LoadResidentMembrane(
        SliLispRuntimeLoadRequest request,
        DateTimeOffset timestampUtc);
}

public sealed class DefaultSliLispRuntimeLoadService : ISliLispRuntimeLoadService
{
    private const string RuntimeKind = "SBCL";
    private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(30);
    private static readonly Encoding Utf8NoBom = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);

    private static readonly string[] FoundationModules =
    [
        "core.lisp",
        "parser.lisp",
        "transport.lisp",
        "witness.lisp",
        "admissibility.lisp",
        "compass.lisp"
    ];

    public SliLispRuntimeLoadReceipt LoadResidentMembrane(
        SliLispRuntimeLoadRequest request,
        DateTimeOffset timestampUtc)
    {
        ArgumentNullException.ThrowIfNull(request);

        var modules = LispModuleCatalog.LoadModules();
        var orderedModules = OrderModules(modules.Keys).ToArray();
        var runtimePath = ResolveRuntimePath(request.RuntimePath);

        if (request.RequestsForbiddenMotion)
        {
            return CreateReceipt(
                request,
                SliLispRuntimeLoadDisposition.Refused,
                "sli-lisp-runtime-motion-refused",
                runtimePath,
                orderedModules,
                loadAttempted: false,
                loadSucceeded: false,
                exitCode: null,
                standardOutput: string.Empty,
                standardError: "Resident SLI.Lisp load permits bounded module loading only; arbitrary eval, action, and activation remain refused.",
                timestampUtc);
        }

        var tempRoot = Path.Combine(Path.GetTempPath(), "project-bicycle-sli-lisp-load", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempRoot);

        try
        {
            WriteModules(tempRoot, modules, orderedModules);
            var scriptPath = WriteLoadScript(tempRoot, orderedModules);
            var result = RunSbcl(runtimePath, scriptPath, request.Timeout ?? DefaultTimeout);

            return CreateReceipt(
                request,
                result.ExitCode == 0
                    ? SliLispRuntimeLoadDisposition.LoadedCold
                    : SliLispRuntimeLoadDisposition.Withheld,
                result.ExitCode == 0
                    ? "sli-lisp-resident-membrane-loaded-cold"
                    : "sli-lisp-resident-membrane-load-failed",
                runtimePath,
                orderedModules,
                loadAttempted: true,
                loadSucceeded: result.ExitCode == 0,
                result.ExitCode,
                result.StandardOutput,
                result.StandardError,
                timestampUtc);
        }
        catch (Win32Exception ex)
        {
            return CreateReceipt(
                request,
                SliLispRuntimeLoadDisposition.Withheld,
                "sli-lisp-runtime-missing",
                runtimePath,
                orderedModules,
                loadAttempted: true,
                loadSucceeded: false,
                exitCode: null,
                standardOutput: string.Empty,
                standardError: ex.Message,
                timestampUtc);
        }
        finally
        {
            if (Directory.Exists(tempRoot))
            {
                Directory.Delete(tempRoot, recursive: true);
            }
        }
    }

    private static SliLispRuntimeLoadReceipt CreateReceipt(
        SliLispRuntimeLoadRequest request,
        SliLispRuntimeLoadDisposition disposition,
        string outcomeCode,
        string runtimePath,
        IReadOnlyList<string> orderedModules,
        bool loadAttempted,
        bool loadSucceeded,
        int? exitCode,
        string standardOutput,
        string standardError,
        DateTimeOffset timestampUtc) =>
        new(
            ReceiptHandle: CreateHandle("sli-lisp-runtime-load://", outcomeCode, string.Join("|", orderedModules), timestampUtc.UtcTicks.ToString()),
            Disposition: disposition,
            OutcomeCode: outcomeCode,
            RuntimeKind: RuntimeKind,
            RuntimePath: runtimePath,
            ModuleNames: orderedModules,
            ModuleCount: orderedModules.Count,
            LoadedFromEmbeddedResources: true,
            LoadAttempted: loadAttempted,
            LoadSucceeded: loadSucceeded,
            ResidentModuleLoadAllowed: !request.RequestsForbiddenMotion,
            TopLevelLoadEvaluationExpected: !request.RequestsForbiddenMotion,
            ArbitraryEvaluationAllowed: false,
            RuntimeActionAllowed: false,
            ActivationAllowed: false,
            AuthorityGranted: false,
            ModelBindingAllowed: false,
            GelPromotionAllowed: false,
            CmeActualAllowed: false,
            SanctuaryActualAllowed: false,
            ExitCode: exitCode,
            StandardOutput: standardOutput,
            StandardError: standardError,
            TimestampUtc: timestampUtc);

    private static IEnumerable<string> OrderModules(IEnumerable<string> moduleNames)
    {
        var remaining = moduleNames.ToHashSet(StringComparer.OrdinalIgnoreCase);

        foreach (var foundationModule in FoundationModules)
        {
            if (remaining.Remove(foundationModule))
            {
                yield return foundationModule;
            }
        }

        foreach (var moduleName in remaining.OrderBy(static name => name, StringComparer.OrdinalIgnoreCase))
        {
            yield return moduleName;
        }
    }

    private static void WriteModules(
        string tempRoot,
        IReadOnlyDictionary<string, string> modules,
        IReadOnlyList<string> orderedModules)
    {
        foreach (var moduleName in orderedModules)
        {
            File.WriteAllText(Path.Combine(tempRoot, moduleName), modules[moduleName], Utf8NoBom);
        }
    }

    private static string WriteLoadScript(
        string tempRoot,
        IReadOnlyList<string> orderedModules)
    {
        var scriptPath = Path.Combine(tempRoot, "load-resident-sli-lisp.lisp");
        var builder = new StringBuilder();

        builder.AppendLine("(format t \"SAN-SLI-LISP-RUNTIME-LOAD-BEGIN~%\")");
        foreach (var moduleName in orderedModules)
        {
            if (!string.Equals(moduleName, "core.lisp", StringComparison.OrdinalIgnoreCase))
            {
                builder.AppendLine("(in-package :sli-core)");
            }

            builder.Append("(load ");
            builder.Append(ToLispString(Path.Combine(tempRoot, moduleName)));
            builder.AppendLine(")");
        }

        builder.AppendLine("(unless (and (find-package :sli-core) (fboundp 'sli-core:execute)) (error \"canonical SLI-CORE runtime symbols missing\"))");
        builder.Append("(format t \"SAN-SLI-LISP-RUNTIME-LOAD-OK module-count=");
        builder.Append(orderedModules.Count);
        builder.AppendLine("~%\")");
        builder.AppendLine("(sb-ext:exit :code 0)");

        File.WriteAllText(scriptPath, builder.ToString(), Utf8NoBom);
        return scriptPath;
    }

    private static ProcessResult RunSbcl(
        string runtimePath,
        string scriptPath,
        TimeSpan timeout)
    {
        using var process = new Process();
        process.StartInfo = new ProcessStartInfo
        {
            FileName = runtimePath,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        process.StartInfo.ArgumentList.Add("--noinform");
        process.StartInfo.ArgumentList.Add("--disable-debugger");
        process.StartInfo.ArgumentList.Add("--script");
        process.StartInfo.ArgumentList.Add(scriptPath);

        process.Start();
        var standardOutput = process.StandardOutput.ReadToEndAsync();
        var standardError = process.StandardError.ReadToEndAsync();

        if (!process.WaitForExit(timeout))
        {
            process.Kill(entireProcessTree: true);
            return new ProcessResult(
                ExitCode: -1,
                StandardOutput: standardOutput.GetAwaiter().GetResult(),
                StandardError: "SBCL timed out while loading resident SLI.Lisp membrane.");
        }

        return new ProcessResult(
            process.ExitCode,
            standardOutput.GetAwaiter().GetResult(),
            standardError.GetAwaiter().GetResult());
    }

    private static string ResolveRuntimePath(string? runtimePath)
    {
        if (!string.IsNullOrWhiteSpace(runtimePath))
        {
            return runtimePath;
        }

        var configured = Environment.GetEnvironmentVariable("SLI_LISP_RUNTIME");
        return string.IsNullOrWhiteSpace(configured) ? "sbcl" : configured;
    }

    private static string ToLispString(string path)
    {
        var normalized = path.Replace('\\', '/');
        return $"\"{normalized.Replace("\\", "\\\\").Replace("\"", "\\\"")}\"";
    }

    private static string CreateHandle(string prefix, params string[] parts)
    {
        var material = string.Join("|", parts);
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(material));
        return $"{prefix}{Convert.ToHexString(hash).ToLowerInvariant()[..16]}";
    }

    private sealed record ProcessResult(
        int ExitCode,
        string StandardOutput,
        string StandardError);
}
