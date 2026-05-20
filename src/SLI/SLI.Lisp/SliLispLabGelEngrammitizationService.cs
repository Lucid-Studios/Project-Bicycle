using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace SLI.Lisp;

public interface ISliLispLabGelEngrammitizationService
{
    SliLispLabGelEngrammitizationReceipt Run(
        SliLispLabGelEngrammitizationRequest request,
        DateTimeOffset timestampUtc);
}

public sealed class DefaultSliLispLabGelEngrammitizationService : ISliLispLabGelEngrammitizationService
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
        "compass.lisp",
        "ec-telemetry-loop.lisp",
        "typed-warm-use-rehearsal.lisp",
        "lab-gel-engrammitization.lisp"
    ];

    public SliLispLabGelEngrammitizationReceipt Run(
        SliLispLabGelEngrammitizationRequest request,
        DateTimeOffset timestampUtc)
    {
        ArgumentNullException.ThrowIfNull(request);

        var modules = LispModuleCatalog.LoadModules();
        var orderedModules = OrderModules(modules.Keys).ToArray();
        var runtimePath = ResolveRuntimePath(request.RuntimePath);
        var normalized = NormalizeRequest(request);

        if (request.RequestsForbiddenMotion)
        {
            return CreateReceipt(
                normalized,
                SliLispLabGelEngrammitizationDisposition.Refused,
                "sli-lisp-lab-gel-runtime-motion-refused",
                runtimePath,
                orderedModules,
                telemetry: new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase),
                boundedEntrypointCalled: false,
                loadAttempted: false,
                loadSucceeded: false,
                exitCode: null,
                standardOutput: string.Empty,
                standardError: "Bounded SLI.Lisp lab GEL engrammitization refused because arbitrary eval, action, activation, model binding, GEL admission, SelfGEL mutation, or continuity admission was requested.",
                timestampUtc);
        }

        var tempRoot = Path.Combine(Path.GetTempPath(), "project-bicycle-sli-lab-gel", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempRoot);

        try
        {
            WriteModules(tempRoot, modules, orderedModules);
            var scriptPath = WriteRunScript(tempRoot, orderedModules, normalized);
            var result = RunSbcl(runtimePath, scriptPath, request.Timeout ?? DefaultTimeout);
            var telemetry = ParseTelemetry(result.StandardOutput);
            var completed = result.ExitCode == 0 && IsTrue(telemetry, "lab-gel-engrammitization-completed");

            return CreateReceipt(
                normalized,
                completed
                    ? SliLispLabGelEngrammitizationDisposition.CompletedCold
                    : SliLispLabGelEngrammitizationDisposition.Withheld,
                completed
                    ? "sli-lisp-lab-gel-engrammitization-completed-cold"
                    : "sli-lisp-lab-gel-engrammitization-withheld",
                runtimePath,
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
                normalized,
                SliLispLabGelEngrammitizationDisposition.Withheld,
                "sli-lisp-runtime-missing",
                runtimePath,
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

    private static SliLispLabGelEngrammitizationReceipt CreateReceipt(
        NormalizedLabGelRequest request,
        SliLispLabGelEngrammitizationDisposition disposition,
        string outcomeCode,
        string runtimePath,
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
        var predicateClasses = ReadValue(telemetry, "lab-gel.predicate-classes")
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        return new SliLispLabGelEngrammitizationReceipt(
            ReceiptHandle: CreateHandle(
                "sli-lisp-lab-gel://",
                outcomeCode,
                request.OperatorId,
                request.Domain,
                request.Role,
                request.JobClass,
                request.SessionId,
                request.TurnIndex.ToString(CultureInfo.InvariantCulture),
                request.SourceWarmUseReceiptHandle,
                request.ThoughtForm,
                string.Join("|", orderedModules),
                timestampUtc.UtcTicks.ToString(CultureInfo.InvariantCulture)),
            Disposition: disposition,
            OutcomeCode: outcomeCode,
            RuntimeKind: RuntimeKind,
            RuntimePath: runtimePath,
            OperatorId: ReadValueOrFallback(telemetry, "operator.id", request.OperatorId),
            Domain: ReadValueOrFallback(telemetry, "domain", request.Domain),
            Role: ReadValueOrFallback(telemetry, "role", request.Role),
            JobClass: ReadValueOrFallback(telemetry, "job-class", request.JobClass),
            SessionId: ReadValueOrFallback(telemetry, "session.id", request.SessionId),
            TurnIndex: int.TryParse(ReadValue(telemetry, "session.turn-index"), NumberStyles.Integer, CultureInfo.InvariantCulture, out var turnIndex)
                ? turnIndex
                : request.TurnIndex,
            SourceWarmUseReceiptHandle: ReadValueOrFallback(telemetry, "source.warm-use-receipt", request.SourceWarmUseReceiptHandle),
            ThoughtForm: request.ThoughtForm,
            Telemetry: telemetry,
            ModuleNames: orderedModules,
            ModuleCount: orderedModules.Count,
            BoundedEntrypointCalled: boundedEntrypointCalled,
            LoadAttempted: loadAttempted,
            LoadSucceeded: loadSucceeded,
            LabGelEngrammitizationCompleted: IsTrue(telemetry, "lab-gel-engrammitization-completed"),
            LabGelPredicateFormed: IsTrue(telemetry, "lab-gel.predicate-formed"),
            LabGelPredicateCount: int.TryParse(ReadValue(telemetry, "lab-gel.predicate-count"), NumberStyles.Integer, CultureInfo.InvariantCulture, out var predicateCount) ? predicateCount : 0,
            LabGelPredicateClasses: predicateClasses,
            EngramCandidateFormed: IsTrue(telemetry, "engram-candidate.formed"),
            EngramCandidatePreAdmissionOnly: IsTrue(telemetry, "engram-candidate.pre-admission-only"),
            EvidenceBodyFormed: IsTrue(telemetry, "engram-candidate.evidence-body-formed"),
            WitnessBodyFormed: IsTrue(telemetry, "engram-candidate.witness-body-formed"),
            CoolingHeld: IsTrue(telemetry, "engram-candidate.cooling-held"),
            PreAdmissionReviewRequired: IsTrue(telemetry, "engram-candidate.pre-admission-review-required"),
            LabGelReadbackAvailable: IsTrue(telemetry, "lab-gel.readback-available"),
            LabGelReadbackPreAdmissionOnly: IsTrue(telemetry, "lab-gel.readback-pre-admission-only"),
            TypedScopeAccepted: IsTrue(telemetry, "typed-scope.accepted"),
            SourceWarmUseAcceptedCold: IsTrue(telemetry, "source-warm-use.accepted-cold"),
            SessionLineageWitnessed: IsTrue(telemetry, "session-lineage.witnessed"),
            ListeningFrameReceived: IsTrue(telemetry, "listening-frame.received"),
            SliMembraneInterpretedPredicatePressure: IsTrue(telemetry, "sli-membrane.interpreted-predicate-pressure"),
            CompassOrientedPressure: IsTrue(telemetry, "compass.oriented-pressure"),
            CompassCoolingRequired: IsTrue(telemetry, "compass.cooling-required"),
            SoulFrameReceivedListeningFrame: IsTrue(telemetry, "soulframe.received-listening-frame"),
            AgentiCoreReceivedCompassPressure: IsTrue(telemetry, "agenticore.received-compass-pressure"),
            ThinkingAboutThinkingTelemetryProduced: IsTrue(telemetry, "thinking-about-thinking.telemetry-produced"),
            StewardReviewed: IsTrue(telemetry, "steward.reviewed"),
            GelPromotionAllowed: IsTrue(telemetry, "gel-promotion"),
            GelAdmissionAllowed: IsTrue(telemetry, "gel-admission"),
            EngramAdmissionAllowed: IsTrue(telemetry, "engram-admission"),
            MemoryAdmissionAllowed: IsTrue(telemetry, "memory-admission"),
            SelfGelMutationAllowed: IsTrue(telemetry, "selfgel-mutation"),
            ContinuityAdmissionAllowed: IsTrue(telemetry, "continuity-admission"),
            AuthorityGranted: IsTrue(telemetry, "authority-granted"),
            ActionAuthorized: IsTrue(telemetry, "action-authorized"),
            ModelBindingAllowed: IsTrue(telemetry, "model-binding"),
            ArbitraryEvaluationAllowed: IsTrue(telemetry, "arbitrary-lisp-evaluation"),
            RuntimeActionAllowed: IsTrue(telemetry, "runtime-action"),
            ActivationAllowed: false,
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
        NormalizedLabGelRequest request)
    {
        var scriptPath = Path.Combine(tempRoot, "run-lab-gel-engrammitization.lisp");
        var builder = new StringBuilder();

        builder.AppendLine("(format t \"SAN-SLI-LAB-GEL-BEGIN~%\")");
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

        builder.AppendLine("(unless (and (find-package :sli-core) (fboundp 'sli-core::run-lab-gel-engrammitization)) (error \"bounded lab GEL entrypoint missing\"))");
        builder.Append("(let ((result (sli-core::run-lab-gel-engrammitization ");
        builder.Append(ToLispString(request.OperatorId));
        builder.Append(' ');
        builder.Append(ToLispString(request.Domain));
        builder.Append(' ');
        builder.Append(ToLispString(request.Role));
        builder.Append(' ');
        builder.Append(ToLispString(request.JobClass));
        builder.Append(' ');
        builder.Append(ToLispString(request.SessionId));
        builder.Append(' ');
        builder.Append(request.TurnIndex.ToString(CultureInfo.InvariantCulture));
        builder.Append(' ');
        builder.Append(ToLispString(request.SourceWarmUseReceiptHandle));
        builder.Append(' ');
        builder.Append(ToLispString(request.ThoughtForm));
        builder.AppendLine(")))");
        builder.AppendLine("  (format t \"SAN-SLI-LAB-GEL-OK~%\")");
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
                StandardError: "SBCL timed out while running bounded lab GEL engrammitization.");
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

    private static string ReadValueOrFallback(IReadOnlyDictionary<string, string> telemetry, string key, string fallback) =>
        telemetry.TryGetValue(key, out var value) && !string.IsNullOrWhiteSpace(value)
            ? value
            : fallback;

    private static string ResolveRuntimePath(string? runtimePath)
    {
        if (!string.IsNullOrWhiteSpace(runtimePath))
        {
            return runtimePath;
        }

        var configured = Environment.GetEnvironmentVariable("SLI_LISP_RUNTIME");
        return string.IsNullOrWhiteSpace(configured) ? "sbcl" : configured;
    }

    private static NormalizedLabGelRequest NormalizeRequest(SliLispLabGelEngrammitizationRequest request) =>
        new(
            NormalizeValue(request.OperatorId, "Sanctuary.ID"),
            NormalizeValue(request.Domain, "Sanctuary"),
            NormalizeValue(request.Role, "InstalledBody"),
            NormalizeValue(request.JobClass, "ColdBench"),
            NormalizeValue(request.SessionId, "warm-use-session"),
            Math.Max(0, request.TurnIndex),
            NormalizeValue(request.SourceWarmUseReceiptHandle, "source-warm-use-receipt-missing"),
            NormalizeValue(request.ThoughtForm, "idle lab GEL predicate formation"));

    private static string NormalizeValue(string value, string fallback) =>
        string.IsNullOrWhiteSpace(value) ? fallback : value.Trim();

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

    private sealed record NormalizedLabGelRequest(
        string OperatorId,
        string Domain,
        string Role,
        string JobClass,
        string SessionId,
        int TurnIndex,
        string SourceWarmUseReceiptHandle,
        string ThoughtForm);

    private sealed record ProcessResult(
        int ExitCode,
        string StandardOutput,
        string StandardError);
}
