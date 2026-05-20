using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace SLI.Lisp;

public interface ISliLispEcTelemetryLoopService
{
    SliLispEcTelemetryLoopReceipt Run(
        SliLispEcTelemetryLoopRequest request,
        DateTimeOffset timestampUtc);
}

public sealed class DefaultSliLispEcTelemetryLoopService : ISliLispEcTelemetryLoopService
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

    public SliLispEcTelemetryLoopReceipt Run(
        SliLispEcTelemetryLoopRequest request,
        DateTimeOffset timestampUtc)
    {
        ArgumentNullException.ThrowIfNull(request);

        var modules = LispModuleCatalog.LoadModules();
        var orderedModules = OrderModules(modules.Keys).ToArray();
        var runtimePath = ResolveRuntimePath(request.RuntimePath);
        var thoughtForm = string.IsNullOrWhiteSpace(request.ThoughtForm)
            ? "idle cold EC telemetry loop"
            : request.ThoughtForm.Trim();

        if (request.RequestsForbiddenMotion)
        {
            return CreateReceipt(
                request,
                SliLispEcTelemetryLoopDisposition.Refused,
                "sli-lisp-ec-loop-runtime-motion-refused",
                runtimePath,
                thoughtForm,
                orderedModules,
                telemetry: new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase),
                boundedEntrypointCalled: false,
                loadAttempted: false,
                loadSucceeded: false,
                exitCode: null,
                standardOutput: string.Empty,
                standardError: "Bounded SLI.Lisp EC loop refused because arbitrary eval, action, activation, or model binding was requested.",
                timestampUtc);
        }

        var tempRoot = Path.Combine(Path.GetTempPath(), "project-bicycle-sli-ec-loop", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempRoot);

        try
        {
            WriteModules(tempRoot, modules, orderedModules);
            var scriptPath = WriteRunScript(tempRoot, orderedModules, thoughtForm);
            var result = RunSbcl(runtimePath, scriptPath, request.Timeout ?? DefaultTimeout);
            var telemetry = ParseTelemetry(result.StandardOutput);
            var completed = result.ExitCode == 0 && IsTrue(telemetry, "cold-engine-loop-completed");

            return CreateReceipt(
                request,
                completed
                    ? SliLispEcTelemetryLoopDisposition.CompletedCold
                    : SliLispEcTelemetryLoopDisposition.Withheld,
                completed
                    ? "sli-lisp-ec-telemetry-loop-completed-cold"
                    : "sli-lisp-ec-telemetry-loop-withheld",
                runtimePath,
                thoughtForm,
                orderedModules,
                telemetry,
                boundedEntrypointCalled: completed,
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
                SliLispEcTelemetryLoopDisposition.Withheld,
                "sli-lisp-runtime-missing",
                runtimePath,
                thoughtForm,
                orderedModules,
                telemetry: new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase),
                boundedEntrypointCalled: false,
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

    private static SliLispEcTelemetryLoopReceipt CreateReceipt(
        SliLispEcTelemetryLoopRequest request,
        SliLispEcTelemetryLoopDisposition disposition,
        string outcomeCode,
        string runtimePath,
        string thoughtForm,
        IReadOnlyList<string> orderedModules,
        IReadOnlyDictionary<string, string> telemetry,
        bool boundedEntrypointCalled,
        bool loadAttempted,
        bool loadSucceeded,
        int? exitCode,
        string standardOutput,
        string standardError,
        DateTimeOffset timestampUtc)
    {
        var residueClasses = ReadValue(telemetry, "pre-engram.residue-classes")
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        return new SliLispEcTelemetryLoopReceipt(
            ReceiptHandle: CreateHandle("sli-lisp-ec-loop://", outcomeCode, thoughtForm, string.Join("|", orderedModules), timestampUtc.UtcTicks.ToString(CultureInfo.InvariantCulture)),
            Disposition: disposition,
            OutcomeCode: outcomeCode,
            RuntimeKind: RuntimeKind,
            RuntimePath: runtimePath,
            ThoughtForm: thoughtForm,
            Telemetry: telemetry,
            ModuleNames: orderedModules,
            ModuleCount: orderedModules.Count,
            BoundedEntrypointCalled: boundedEntrypointCalled,
            LoadAttempted: loadAttempted,
            LoadSucceeded: loadSucceeded,
            ColdEngineLoopCompleted: IsTrue(telemetry, "cold-engine-loop-completed"),
            ListeningFrameReceived: IsTrue(telemetry, "listening-frame.received"),
            SliMembraneInterpretedPredicatePressure: IsTrue(telemetry, "sli-membrane.interpreted-predicate-pressure"),
            CompassOrientedPressure: IsTrue(telemetry, "compass.oriented-pressure"),
            CompassCoolingRequired: IsTrue(telemetry, "compass.cooling-required"),
            SoulFrameReceivedListeningFrame: IsTrue(telemetry, "soulframe.received-listening-frame"),
            AgentiCoreReceivedCompassPressure: IsTrue(telemetry, "agenticore.received-compass-pressure"),
            ThinkingAboutThinkingTelemetryProduced: IsTrue(telemetry, "thinking-about-thinking.telemetry-produced"),
            PreEngramResidueProduced: IsTrue(telemetry, "pre-engram.residue-produced"),
            PreEngramResidueCount: int.TryParse(ReadValue(telemetry, "pre-engram.residue-count"), NumberStyles.Integer, CultureInfo.InvariantCulture, out var residueCount) ? residueCount : 0,
            PreEngramResidueClasses: residueClasses,
            StewardReviewed: IsTrue(telemetry, "steward.reviewed"),
            EngramAdmissionAllowed: IsTrue(telemetry, "engram-admission"),
            MemoryAdmissionAllowed: IsTrue(telemetry, "memory-admission"),
            SelfGelMutationAllowed: IsTrue(telemetry, "selfgel-mutation"),
            ContinuityAdmissionAllowed: IsTrue(telemetry, "continuity-admission"),
            AuthorityGranted: IsTrue(telemetry, "authority-granted"),
            ActionAuthorized: IsTrue(telemetry, "action-authorized"),
            ModelBindingAllowed: IsTrue(telemetry, "model-binding"),
            ArbitraryEvaluationAllowed: IsTrue(telemetry, "arbitrary-lisp-evaluation"),
            RuntimeActionAllowed: false,
            ActivationAllowed: request.ActivationRequested,
            CmeActualActivationAllowed: IsTrue(telemetry, "cme-actual-activation"),
            SanctuaryActualActivationAllowed: IsTrue(telemetry, "sanctuary-actual-activation"),
            ExitCode: exitCode,
            StandardOutput: standardOutput,
            StandardError: standardError,
            TimestampUtc: timestampUtc);
    }

    private static string WriteRunScript(
        string tempRoot,
        IReadOnlyList<string> orderedModules,
        string thoughtForm)
    {
        var scriptPath = Path.Combine(tempRoot, "run-ec-telemetry-loop.lisp");
        var builder = new StringBuilder();

        builder.AppendLine("(format t \"SAN-SLI-EC-TELEMETRY-LOOP-BEGIN~%\")");
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

        builder.AppendLine("(unless (and (find-package :sli-core) (fboundp 'sli-core::run-ec-telemetry-loop)) (error \"bounded EC telemetry entrypoint missing\"))");
        builder.Append("(let ((result (sli-core::run-ec-telemetry-loop ");
        builder.Append(ToLispString(thoughtForm));
        builder.AppendLine(")))");
        builder.AppendLine("  (format t \"SAN-SLI-EC-TELEMETRY-LOOP-OK~%\")");
        builder.AppendLine("  (dolist (line result) (format t \"~a~%\" line)))");
        builder.AppendLine("(sb-ext:exit :code 0)");

        File.WriteAllText(scriptPath, builder.ToString(), Utf8NoBom);
        return scriptPath;
    }

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
                StandardError: "SBCL timed out while running bounded EC telemetry loop.");
        }

        return new ProcessResult(
            process.ExitCode,
            standardOutput.GetAwaiter().GetResult(),
            standardError.GetAwaiter().GetResult());
    }

    private static IReadOnlyDictionary<string, string> ParseTelemetry(string standardOutput)
    {
        var telemetry = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (var line in standardOutput.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            var equalsIndex = line.IndexOf('=', StringComparison.Ordinal);
            if (equalsIndex <= 0)
            {
                continue;
            }

            telemetry[line[..equalsIndex]] = line[(equalsIndex + 1)..];
        }

        return telemetry;
    }

    private static bool IsTrue(IReadOnlyDictionary<string, string> telemetry, string key) =>
        telemetry.TryGetValue(key, out var value) &&
        string.Equals(value, "true", StringComparison.OrdinalIgnoreCase);

    private static string ReadValue(IReadOnlyDictionary<string, string> telemetry, string key) =>
        telemetry.TryGetValue(key, out var value) ? value : string.Empty;

    private static string ResolveRuntimePath(string? runtimePath)
    {
        if (!string.IsNullOrWhiteSpace(runtimePath))
        {
            return runtimePath;
        }

        var configured = Environment.GetEnvironmentVariable("SLI_LISP_RUNTIME");
        return string.IsNullOrWhiteSpace(configured) ? "sbcl" : configured;
    }

    private static string ToLispString(string value)
    {
        var normalized = value.Replace('\\', '/');
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
