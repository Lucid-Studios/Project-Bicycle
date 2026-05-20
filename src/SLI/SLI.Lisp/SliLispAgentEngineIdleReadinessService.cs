using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace SLI.Lisp;

public interface ISliLispAgentEngineIdleReadinessService
{
    SliLispAgentEngineIdleReadinessReceipt Run(
        SliLispAgentEngineIdleReadinessRequest request,
        DateTimeOffset timestampUtc);
}

public sealed class DefaultSliLispAgentEngineIdleReadinessService : ISliLispAgentEngineIdleReadinessService
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
        "agent-engine-idle-readiness.lisp"
    ];

    public SliLispAgentEngineIdleReadinessReceipt Run(
        SliLispAgentEngineIdleReadinessRequest request,
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
                SliLispAgentEngineIdleReadinessDisposition.Refused,
                "sli-lisp-agent-engine-idle-runtime-motion-refused",
                runtimePath,
                orderedModules,
                telemetry: new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase),
                boundedEntrypointCalled: false,
                loadAttempted: false,
                loadSucceeded: false,
                exitCode: null,
                standardOutput: string.Empty,
                standardError: "Bounded SLI.Lisp agent engine idle readiness refused because arbitrary eval, action, activation, model binding, authority grant, executor arm, GEL admission, SelfGEL mutation, heartbeat activation, Actual activation, or continuity admission was requested.",
                timestampUtc);
        }

        var tempRoot = Path.Combine(Path.GetTempPath(), "project-bicycle-sli-agent-idle", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempRoot);

        try
        {
            WriteModules(tempRoot, modules, orderedModules);
            var scriptPath = WriteRunScript(tempRoot, orderedModules, normalized);
            var result = RunSbcl(runtimePath, scriptPath, request.Timeout ?? DefaultTimeout);
            var telemetry = ParseTelemetry(result.StandardOutput);
            var completed = result.ExitCode == 0 && IsTrue(telemetry, "agent-engine-idle-readiness-completed");

            return CreateReceipt(
                normalized,
                completed
                    ? SliLispAgentEngineIdleReadinessDisposition.CompletedCold
                    : SliLispAgentEngineIdleReadinessDisposition.Withheld,
                completed
                    ? "sli-lisp-agent-engine-idle-readiness-completed-cold"
                    : "sli-lisp-agent-engine-idle-readiness-withheld",
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
                SliLispAgentEngineIdleReadinessDisposition.Withheld,
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

    private static SliLispAgentEngineIdleReadinessReceipt CreateReceipt(
        NormalizedAgentEngineIdleRequest request,
        SliLispAgentEngineIdleReadinessDisposition disposition,
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
                "sli-lisp-agent-engine-idle://",
                outcomeCode,
                request.OperatorId,
                request.Domain,
                request.Role,
                request.JobClass,
                request.SessionId,
                request.TurnIndex.ToString(CultureInfo.InvariantCulture),
                request.SourceLabGelReceiptHandle,
                request.SourceEngramCandidateHandle,
                request.SourceEngramClosureReceiptHandle,
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
            SourceLabGelReceiptHandle: ReadValueOrFallback(telemetry, "source.lab-gel-receipt", request.SourceLabGelReceiptHandle),
            SourceEngramCandidateHandle: ReadValueOrFallback(telemetry, "source.engram-candidate", request.SourceEngramCandidateHandle),
            SourceEngramClosureReceiptHandle: ReadValueOrFallback(telemetry, "source.engram-closure", request.SourceEngramClosureReceiptHandle),
            ThoughtForm: request.ThoughtForm,
            Telemetry: telemetry,
            ModuleNames: orderedModules,
            ModuleCount: orderedModules.Count,
            BoundedEntrypointCalled: boundedEntrypointCalled,
            LoadAttempted: loadAttempted,
            LoadSucceeded: loadSucceeded,
            AgentEngineIdleReadinessCompleted: IsTrue(telemetry, "agent-engine-idle-readiness-completed"),
            EngineSeatKind: ReadValueOrFallback(telemetry, "engine-seat", "engine-llm-candidate"),
            EngineLlmProfile: ReadValueOrFallback(telemetry, "engine-llm.profile", "provider-agnostic-test-seat"),
            ProviderNeutralityHeld: IsTrue(telemetry, "provider-neutrality.held"),
            CrossModelTestHarnessApproachable: IsTrue(telemetry, "cross-model-test-harness.approachable"),
            EngineLlmProviderAssumptionAllowed: IsTrue(telemetry, "engine-llm.provider-assumption"),
            EngineLlmInternalSubstrateClaimed: IsTrue(telemetry, "engine-llm.internal-substrate-claimed"),
            CodexAgentLabProfileStaged: IsTrue(telemetry, "codex-agent-lab-profile.candidate-staged"),
            CodexEngineSeatCandidateStaged: IsTrue(telemetry, "codex-engine-seat.candidate-staged"),
            SubagentEngineSeatCandidateStaged: IsTrue(telemetry, "subagent-swarm-seat.candidate-staged"),
            OperatorPresenceRequired: IsTrue(telemetry, "operator-presence.required"),
            DriverSeated: IsTrue(telemetry, "driver-seated"),
            DriverSeatCandidateStaged: IsTrue(telemetry, "driver-seat-candidate-staged"),
            AuthorityGrantCandidateStaged: IsTrue(telemetry, "authority-grant-candidate-staged"),
            AuthorityGrantAbsent: IsTrue(telemetry, "authority-grant.absent"),
            ActionExecutorCandidateStaged: IsTrue(telemetry, "action-executor-candidate-staged"),
            ActionExecutorLocked: IsTrue(telemetry, "action-executor.locked"),
            ActionExecutorArmed: IsTrue(telemetry, "action-executor-armed"),
            GelAdmissionCandidateStaged: IsTrue(telemetry, "gel-admission-candidate-staged"),
            GelAdmissionLocked: IsTrue(telemetry, "gel-admission.locked"),
            SelfGelMutationCandidateStaged: IsTrue(telemetry, "selfgel-mutation-candidate-staged"),
            SelfGelMutationLocked: IsTrue(telemetry, "selfgel-mutation.locked"),
            HeartbeatCandidateStaged: IsTrue(telemetry, "heartbeat-candidate-staged"),
            HeartbeatLocked: IsTrue(telemetry, "heartbeat.locked"),
            HeartbeatActive: IsTrue(telemetry, "heartbeat-active"),
            CmeActualCandidateStaged: IsTrue(telemetry, "cme-actual-candidate-staged"),
            CmeActualLocked: IsTrue(telemetry, "cme-actual.locked"),
            SanctuaryActualCandidateStaged: IsTrue(telemetry, "sanctuary-actual-candidate-staged"),
            SanctuaryActualLocked: IsTrue(telemetry, "sanctuary-actual.locked"),
            IdleLoopAllowed: IsTrue(telemetry, "idle-loop.allowed"),
            EngineLlmMayArticulate: IsTrue(telemetry, "engine-llm.may-articulate"),
            EngineLlmMayRehearse: IsTrue(telemetry, "engine-llm.may-rehearse"),
            EngineLlmMayFormCandidates: IsTrue(telemetry, "engine-llm.may-form-candidates"),
            EngineLlmMayGrantAuthority: IsTrue(telemetry, "engine-llm.may-grant-authority"),
            EngineLlmMayAuthorizeAction: IsTrue(telemetry, "engine-llm.may-authorize-action"),
            EngineLlmMayExecuteAction: IsTrue(telemetry, "engine-llm.may-execute-action"),
            EngineLlmMayAdmitGel: IsTrue(telemetry, "engine-llm.may-admit-gel"),
            EngineLlmMayMutateSelfGel: IsTrue(telemetry, "engine-llm.may-mutate-selfgel"),
            EngineLlmMayActivateActual: IsTrue(telemetry, "engine-llm.may-activate-actual"),
            TypedScopeAccepted: IsTrue(telemetry, "typed-scope.accepted"),
            SourceLabGelAcceptedCold: IsTrue(telemetry, "source-lab-gel.accepted-cold"),
            SourceEngramClosureAcceptedCold: IsTrue(telemetry, "source.engram-closure.accepted-cold"),
            SessionLineageWitnessed: IsTrue(telemetry, "session-lineage.witnessed"),
            ListeningFrameReceived: IsTrue(telemetry, "listening-frame.received"),
            SliMembraneInterpretedPredicatePressure: IsTrue(telemetry, "sli-membrane.interpreted-predicate-pressure"),
            CompassOrientedPressure: IsTrue(telemetry, "compass.oriented-pressure"),
            CompassCoolingRequired: IsTrue(telemetry, "compass.cooling-required"),
            SoulFrameReceivedListeningFrame: IsTrue(telemetry, "soulframe.received-listening-frame"),
            AgentiCoreReceivedCompassPressure: IsTrue(telemetry, "agenticore.received-compass-pressure"),
            ThinkingAboutThinkingTelemetryProduced: IsTrue(telemetry, "thinking-about-thinking.telemetry-produced"),
            StewardReviewed: IsTrue(telemetry, "steward.reviewed"),
            ModelBindingAllowed: IsTrue(telemetry, "model-binding"),
            ArbitraryEvaluationAllowed: IsTrue(telemetry, "arbitrary-lisp-evaluation"),
            RuntimeActionAllowed: IsTrue(telemetry, "runtime-action"),
            MemoryAdmissionAllowed: IsTrue(telemetry, "memory-admission"),
            ContinuityAdmissionAllowed: IsTrue(telemetry, "continuity-admission"),
            GelAdmissionAllowed: IsTrue(telemetry, "gel-admission"),
            SelfGelMutationAllowed: IsTrue(telemetry, "selfgel-mutation"),
            AuthorityGranted: IsTrue(telemetry, "authority-granted"),
            ActionAuthorized: IsTrue(telemetry, "action-authorized"),
            CmeActualActivationAllowed: IsTrue(telemetry, "cme-actual-activation"),
            SanctuaryActualActivationAllowed: IsTrue(telemetry, "sanctuary-actual-activation"),
            ExitCode: exitCode,
            StandardOutput: standardOutput,
            StandardError: standardError,
            TimestampUtc: timestampUtc);

    private static string WriteRunScript(
        string tempRoot,
        IReadOnlyList<string> orderedModules,
        NormalizedAgentEngineIdleRequest request)
    {
        var scriptPath = Path.Combine(tempRoot, "run-agent-engine-idle-readiness.lisp");
        var builder = new StringBuilder();

        builder.AppendLine("(format t \"SAN-SLI-AGENT-ENGINE-IDLE-BEGIN~%\")");
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

        builder.AppendLine("(unless (and (find-package :sli-core) (fboundp 'sli-core::run-agent-engine-idle-readiness)) (error \"bounded agent engine idle entrypoint missing\"))");
        builder.Append("(let ((result (sli-core::run-agent-engine-idle-readiness ");
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
        builder.Append(ToLispString(request.SourceLabGelReceiptHandle));
        builder.Append(' ');
        builder.Append(ToLispString(request.SourceEngramCandidateHandle));
        builder.Append(' ');
        builder.Append(ToLispString(request.ThoughtForm));
        builder.Append(' ');
        builder.Append(ToLispString(request.SourceEngramClosureReceiptHandle));
        builder.AppendLine(")))");
        builder.AppendLine("  (format t \"SAN-SLI-AGENT-ENGINE-IDLE-OK~%\")");
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
                StandardError: "SBCL timed out while running bounded agent engine idle readiness.");
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

    private static NormalizedAgentEngineIdleRequest NormalizeRequest(SliLispAgentEngineIdleReadinessRequest request) =>
        new(
            NormalizeValue(request.OperatorId, "Sanctuary.ID"),
            NormalizeValue(request.Domain, "Sanctuary"),
            NormalizeValue(request.Role, "InstalledBody"),
            NormalizeValue(request.JobClass, "ColdBench"),
            NormalizeValue(request.SessionId, "agent-engine-idle-session"),
            Math.Max(0, request.TurnIndex),
            NormalizeValue(request.SourceLabGelReceiptHandle, "source-lab-gel-receipt-missing"),
            NormalizeValue(request.SourceEngramCandidateHandle, "engram-candidate-missing"),
            NormalizeValue(request.ThoughtForm, "idle agent engine readiness"),
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

    private sealed record NormalizedAgentEngineIdleRequest(
        string OperatorId,
        string Domain,
        string Role,
        string JobClass,
        string SessionId,
        int TurnIndex,
        string SourceLabGelReceiptHandle,
        string SourceEngramCandidateHandle,
        string ThoughtForm,
        string SourceEngramClosureReceiptHandle);

    private sealed record ProcessResult(
        int ExitCode,
        string StandardOutput,
        string StandardError);
}
