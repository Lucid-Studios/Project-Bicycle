using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace SLI.Lisp;

public interface ISliLispLlmTickCycleService
{
    SliLispLlmTickCycleReceipt Run(
        SliLispLlmTickCycleRequest request,
        DateTimeOffset timestampUtc);
}

public sealed class DefaultSliLispLlmTickCycleService : ISliLispLlmTickCycleService
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
        "lab-gel-engrammitization.lisp",
        "agent-engine-idle-readiness.lisp",
        "llm-interconnect-readiness.lisp",
        "llm-tick-cycle.lisp"
    ];

    public SliLispLlmTickCycleReceipt Run(
        SliLispLlmTickCycleRequest request,
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
                SliLispLlmTickCycleDisposition.Refused,
                "sli-lisp-llm-tick-runtime-motion-refused",
                runtimePath,
                orderedModules,
                telemetry: new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase),
                boundedEntrypointCalled: false,
                loadAttempted: false,
                loadSucceeded: false,
                exitCode: null,
                standardOutput: string.Empty,
                standardError: "Bounded SLI.Lisp LLM tick cycle refused because arbitrary eval, action, activation, model binding, provider call, hidden internals claim, authority grant, executor arm, GEL admission, SelfGEL mutation, heartbeat activation, Actual activation, or continuity admission was requested.",
                timestampUtc);
        }

        var tempRoot = Path.Combine(Path.GetTempPath(), "project-bicycle-sli-llm-tick", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempRoot);

        try
        {
            WriteModules(tempRoot, modules, orderedModules);
            var scriptPath = WriteRunScript(tempRoot, orderedModules, normalized);
            var result = RunSbcl(runtimePath, scriptPath, request.Timeout ?? DefaultTimeout);
            var telemetry = ParseTelemetry(result.StandardOutput);
            var completed = result.ExitCode == 0 && IsTrue(telemetry, "llm-tick-cycle-completed");

            return CreateReceipt(
                normalized,
                completed
                    ? SliLispLlmTickCycleDisposition.CompletedCold
                    : SliLispLlmTickCycleDisposition.Withheld,
                completed
                    ? "sli-lisp-llm-tick-cycle-completed-cold"
                    : "sli-lisp-llm-tick-cycle-withheld",
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
                SliLispLlmTickCycleDisposition.Withheld,
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

    private static SliLispLlmTickCycleReceipt CreateReceipt(
        NormalizedLlmTickCycleRequest request,
        SliLispLlmTickCycleDisposition disposition,
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
        DateTimeOffset timestampUtc) =>
        new(
            ReceiptHandle: CreateHandle(
                "sli-lisp-llm-tick://",
                outcomeCode,
                request.OperatorId,
                request.Domain,
                request.Role,
                request.JobClass,
                request.SessionId,
                request.TickIndex.ToString(CultureInfo.InvariantCulture),
                request.SourceLlmInterconnectReadinessReceiptHandle,
                request.SourceEngramClosureReceiptHandle,
                request.PriorTickReceiptHandle,
                request.AdapterKind,
                request.AdapterResponseReceiptHandle,
                request.AdapterOutput,
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
            TickIndex: int.TryParse(ReadValue(telemetry, "tick.index"), NumberStyles.Integer, CultureInfo.InvariantCulture, out var tickIndex)
                ? tickIndex
                : request.TickIndex,
            SourceLlmInterconnectReadinessReceiptHandle: ReadValueOrFallback(telemetry, "source.llm-interconnect-readiness-receipt", request.SourceLlmInterconnectReadinessReceiptHandle),
            SourceEngramClosureReceiptHandle: ReadValueOrFallback(telemetry, "source.engram-closure-receipt", request.SourceEngramClosureReceiptHandle),
            PriorTickReceiptHandle: ReadValueOrFallback(telemetry, "source.prior-tick-receipt", request.PriorTickReceiptHandle),
            AdapterKind: ReadValueOrFallback(telemetry, "model-adapter.kind", request.AdapterKind),
            AdapterResponseReceiptHandle: ReadValueOrFallback(telemetry, "adapter-response.receipt", request.AdapterResponseReceiptHandle),
            ThoughtForm: request.ThoughtForm,
            Telemetry: telemetry,
            ModuleNames: orderedModules,
            ModuleCount: orderedModules.Count,
            BoundedEntrypointCalled: boundedEntrypointCalled,
            LoadAttempted: loadAttempted,
            LoadSucceeded: loadSucceeded,
            LlmTickCycleCompleted: IsTrue(telemetry, "llm-tick-cycle-completed"),
            TickState: ReadValueOrFallback(telemetry, "llm-tick.state", string.Empty),
            TickLoopRunning: IsTrue(telemetry, "tick-loop.running"),
            TickLoopKind: ReadValueOrFallback(telemetry, "tick-loop.kind", string.Empty),
            SourceLlmInterconnectReady: IsTrue(telemetry, "source.llm-interconnect-ready"),
            ReadyForAdapter: IsTrue(telemetry, "llm-interconnect.ready-for-adapter"),
            ProviderNeutral: IsTrue(telemetry, "llm-interconnect.provider-neutral") || IsTrue(telemetry, "model-adapter.provider-neutral"),
            ModelAdapterPresent: IsTrue(telemetry, "model-adapter.present"),
            DeterministicHarnessAdapter: IsTrue(telemetry, "model-adapter.deterministic-harness"),
            AdapterResponseWitnessed: IsTrue(telemetry, "model-adapter.response-witnessed"),
            AdapterResponseBounded: IsTrue(telemetry, "model-adapter.response-bounded"),
            AdapterOutputWitnessed: IsTrue(telemetry, "adapter-response.output-witnessed"),
            AdapterOutputBounded: IsTrue(telemetry, "adapter-response.output-bounded"),
            AdapterOutputBecomesTruth: IsTrue(telemetry, "adapter-response.output-becomes-truth"),
            AdapterOutputAuthorizesAction: IsTrue(telemetry, "adapter-response.output-authorizes-action"),
            AdapterOutputAdmitsMemory: IsTrue(telemetry, "adapter-response.output-admits-memory"),
            AdapterOutputAdmitsContinuity: IsTrue(telemetry, "adapter-response.output-admits-continuity"),
            ModelBindingAllowed: IsTrue(telemetry, "model-binding") || IsTrue(telemetry, "model-adapter.model-binding"),
            ProviderCallAllowed: IsTrue(telemetry, "provider-call") || IsTrue(telemetry, "model-adapter.provider-call"),
            HiddenInternalsClaimed: IsTrue(telemetry, "hidden-internals-claim") || IsTrue(telemetry, "model-adapter.hidden-internals-claim"),
            SliLispLoaded: IsTrue(telemetry, "membrane.sli-lisp.loaded"),
            SliLispProcessedTick: IsTrue(telemetry, "membrane.sli-lisp.processed-tick"),
            SliLispPrimePresent: IsTrue(telemetry, "membrane.sli-lisp-prime.present"),
            SliLispCrypticPresent: IsTrue(telemetry, "membrane.sli-lisp-cryptic.present"),
            LispControlMatrixPresent: IsTrue(telemetry, "membrane.lisp-control-matrix.present"),
            ListeningFramePresent: IsTrue(telemetry, "membrane.listening-frame.present"),
            CompassPresent: IsTrue(telemetry, "membrane.compass.present"),
            SoulFrameRoutePresent: IsTrue(telemetry, "membrane.soulframe-route.present"),
            AgentiCoreRoutePresent: IsTrue(telemetry, "membrane.agenticore-route.present"),
            ListeningFrameReceived: IsTrue(telemetry, "listening-frame.received"),
            SliMembraneInterpretedPredicatePressure: IsTrue(telemetry, "sli-membrane.interpreted-predicate-pressure"),
            CompassOrientedPressure: IsTrue(telemetry, "compass.oriented-pressure"),
            CompassCoolingRequired: IsTrue(telemetry, "compass.cooling-required"),
            SoulFrameReceivedListeningFrame: IsTrue(telemetry, "soulframe.received-listening-frame"),
            AgentiCoreReceivedCompassPressure: IsTrue(telemetry, "agenticore.received-compass-pressure"),
            ThinkingAboutThinkingTelemetryProduced: IsTrue(telemetry, "thinking-about-thinking.telemetry-produced"),
            PredicateResidueProduced: IsTrue(telemetry, "predicate-residue.produced"),
            PredicateResiduePreEngramOnly: IsTrue(telemetry, "predicate-residue.pre-engram-only"),
            PredicateResidueAdmittedEngram: IsTrue(telemetry, "predicate-residue.admitted-engram"),
            TickLineageWitnessed: IsTrue(telemetry, "tick-lineage.witnessed"),
            SourceEngramClosureReady: IsTrue(telemetry, "source.engram-closure-ready"),
            FirstTickOrigin: IsTrue(telemetry, "tick-lineage.first-tick-origin"),
            PriorTickLinked: IsTrue(telemetry, "tick-lineage.prior-linked"),
            TickLineageBecomesMemory: IsTrue(telemetry, "tick-lineage.becomes-memory"),
            EngineLlmSeatReady: IsTrue(telemetry, "engine-llm-seat.ready"),
            EngineLlmSeatProviderAgnostic: IsTrue(telemetry, "engine-llm-seat.provider-agnostic"),
            EngineLlmMayArticulate: IsTrue(telemetry, "engine-llm.may-articulate"),
            EngineLlmMayRehearse: IsTrue(telemetry, "engine-llm.may-rehearse"),
            EngineLlmMayFormCandidates: IsTrue(telemetry, "engine-llm.may-form-candidates"),
            EngineLlmMayBindModel: IsTrue(telemetry, "engine-llm.may-bind-model"),
            EngineLlmMayCallProvider: IsTrue(telemetry, "engine-llm.may-call-provider"),
            EngineLlmMayGrantAuthority: IsTrue(telemetry, "engine-llm.may-grant-authority"),
            EngineLlmMayExecuteAction: IsTrue(telemetry, "engine-llm.may-execute-action"),
            StewardReviewed: IsTrue(telemetry, "steward.reviewed"),
            AuthorityGrantAbsent: IsTrue(telemetry, "authority-grant.absent"),
            ActionExecutorLocked: IsTrue(telemetry, "action-executor.locked"),
            GelAdmissionLocked: IsTrue(telemetry, "gel-admission.locked"),
            SelfGelMutationLocked: IsTrue(telemetry, "selfgel-mutation.locked"),
            HeartbeatLocked: IsTrue(telemetry, "heartbeat.locked"),
            CmeActualLocked: IsTrue(telemetry, "cme-actual.locked"),
            SanctuaryActualLocked: IsTrue(telemetry, "sanctuary-actual.locked"),
            TypedScopeAccepted: IsTrue(telemetry, "typed-scope.accepted"),
            SessionLineageWitnessed: IsTrue(telemetry, "session-lineage.witnessed"),
            ArbitraryEvaluationAllowed: IsTrue(telemetry, "arbitrary-lisp-evaluation"),
            RuntimeActionAllowed: IsTrue(telemetry, "runtime-action"),
            DatabaseWriteAllowed: IsTrue(telemetry, "database-write"),
            MemoryAdmissionAllowed: IsTrue(telemetry, "memory-admission"),
            ContinuityAdmissionAllowed: IsTrue(telemetry, "continuity-admission"),
            GelAdmissionAllowed: IsTrue(telemetry, "gel-admission"),
            SelfGelMutationAllowed: IsTrue(telemetry, "selfgel-mutation"),
            AuthorityGranted: IsTrue(telemetry, "authority-granted"),
            ActionAuthorized: IsTrue(telemetry, "action-authorized"),
            HeartbeatActive: IsTrue(telemetry, "heartbeat-active"),
            CmeActualActivationAllowed: IsTrue(telemetry, "cme-actual-activation"),
            SanctuaryActualActivationAllowed: IsTrue(telemetry, "sanctuary-actual-activation"),
            ExitCode: exitCode,
            StandardOutput: standardOutput,
            StandardError: standardError,
            TimestampUtc: timestampUtc);

    private static string WriteRunScript(
        string tempRoot,
        IReadOnlyList<string> orderedModules,
        NormalizedLlmTickCycleRequest request)
    {
        var scriptPath = Path.Combine(tempRoot, "run-llm-tick-cycle.lisp");
        var builder = new StringBuilder();

        builder.AppendLine("(format t \"SAN-SLI-LLM-TICK-BEGIN~%\")");
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

        builder.AppendLine("(unless (and (find-package :sli-core) (fboundp 'sli-core::run-llm-tick-cycle)) (error \"bounded LLM tick cycle entrypoint missing\"))");
        builder.Append("(let ((result (sli-core::run-llm-tick-cycle ");
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
        builder.Append(request.TickIndex.ToString(CultureInfo.InvariantCulture));
        builder.Append(' ');
        builder.Append(ToLispString(request.SourceLlmInterconnectReadinessReceiptHandle));
        builder.Append(' ');
        builder.Append(ToLispString(request.PriorTickReceiptHandle));
        builder.Append(' ');
        builder.Append(ToLispString(request.AdapterKind));
        builder.Append(' ');
        builder.Append(ToLispString(request.AdapterResponseReceiptHandle));
        builder.Append(' ');
        builder.Append(ToLispString(request.AdapterOutput));
        builder.Append(' ');
        builder.Append(ToLispString(request.ThoughtForm));
        builder.Append(' ');
        builder.Append(ToLispString(request.SourceEngramClosureReceiptHandle));
        builder.AppendLine(")))");
        builder.AppendLine("  (format t \"SAN-SLI-LLM-TICK-OK~%\")");
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

    private static void WriteModules(string tempRoot, IReadOnlyDictionary<string, string> modules, IReadOnlyList<string> orderedModules)
    {
        foreach (var moduleName in orderedModules)
        {
            File.WriteAllText(Path.Combine(tempRoot, moduleName), modules[moduleName], Utf8NoBom);
        }
    }

    private static ProcessResult RunSbcl(string runtimePath, string scriptPath, TimeSpan timeout)
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
            return new ProcessResult(-1, standardOutput.GetAwaiter().GetResult(), "SBCL timed out while running bounded LLM tick cycle.");
        }

        return new ProcessResult(process.ExitCode, standardOutput.GetAwaiter().GetResult(), standardError.GetAwaiter().GetResult());
    }

    private static IReadOnlyDictionary<string, string> ParseTelemetry(string standardOutput)
    {
        var telemetry = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (var line in standardOutput.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            var equalsIndex = line.IndexOf('=', StringComparison.Ordinal);
            if (equalsIndex > 0)
            {
                telemetry[line[..equalsIndex]] = line[(equalsIndex + 1)..];
            }
        }

        return telemetry;
    }

    private static bool IsTrue(IReadOnlyDictionary<string, string> telemetry, string key) =>
        telemetry.TryGetValue(key, out var value) &&
        string.Equals(value, "true", StringComparison.OrdinalIgnoreCase);

    private static string ReadValue(IReadOnlyDictionary<string, string> telemetry, string key) =>
        telemetry.TryGetValue(key, out var value) ? value : string.Empty;

    private static string ReadValueOrFallback(IReadOnlyDictionary<string, string> telemetry, string key, string fallback) =>
        telemetry.TryGetValue(key, out var value) && !string.IsNullOrWhiteSpace(value) ? value : fallback;

    private static string ResolveRuntimePath(string? runtimePath)
    {
        if (!string.IsNullOrWhiteSpace(runtimePath))
        {
            return runtimePath;
        }

        var configured = Environment.GetEnvironmentVariable("SLI_LISP_RUNTIME");
        return string.IsNullOrWhiteSpace(configured) ? "sbcl" : configured;
    }

    private static NormalizedLlmTickCycleRequest NormalizeRequest(SliLispLlmTickCycleRequest request) =>
        new(
            NormalizeValue(request.OperatorId, "Sanctuary.ID"),
            NormalizeValue(request.Domain, "Sanctuary"),
            NormalizeValue(request.Role, "InstalledBody"),
            NormalizeValue(request.JobClass, "ColdBench"),
            NormalizeValue(request.SessionId, "llm-tick-cycle-session"),
            Math.Max(0, request.TickIndex),
            NormalizeValue(request.SourceLlmInterconnectReadinessReceiptHandle, "llm-readiness-receipt-missing"),
            NormalizeValue(request.PriorTickReceiptHandle, "none"),
            NormalizeValue(request.AdapterKind, "deterministic-harness"),
            NormalizeValue(request.AdapterResponseReceiptHandle, "adapter-response-receipt-missing"),
            NormalizeValue(request.AdapterOutput, "adapter output absent"),
            NormalizeValue(request.ThoughtForm, "cold LLM tick cycle"),
            NormalizeValue(request.SourceEngramClosureReceiptHandle, "engram-closure-missing"));

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

    private sealed record NormalizedLlmTickCycleRequest(
        string OperatorId,
        string Domain,
        string Role,
        string JobClass,
        string SessionId,
        int TickIndex,
        string SourceLlmInterconnectReadinessReceiptHandle,
        string PriorTickReceiptHandle,
        string AdapterKind,
        string AdapterResponseReceiptHandle,
        string AdapterOutput,
        string ThoughtForm,
        string SourceEngramClosureReceiptHandle);

    private sealed record ProcessResult(int ExitCode, string StandardOutput, string StandardError);
}
