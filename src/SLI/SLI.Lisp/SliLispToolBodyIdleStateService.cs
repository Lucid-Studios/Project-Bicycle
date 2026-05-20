using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace SLI.Lisp;

public interface ISliLispToolBodyIdleStateService
{
    SliLispToolBodyIdleStateReceipt Run(
        SliLispToolBodyIdleStateRequest request,
        DateTimeOffset timestampUtc);
}

public sealed class DefaultSliLispToolBodyIdleStateService : ISliLispToolBodyIdleStateService
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
        "tool-body-idle-state.lisp"
    ];

    public SliLispToolBodyIdleStateReceipt Run(
        SliLispToolBodyIdleStateRequest request,
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
                SliLispToolBodyIdleStateDisposition.Refused,
                "sli-lisp-tool-body-idle-runtime-motion-refused",
                runtimePath,
                orderedModules,
                telemetry: new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase),
                boundedEntrypointCalled: false,
                loadAttempted: false,
                loadSucceeded: false,
                exitCode: null,
                standardOutput: string.Empty,
                standardError: "Bounded SLI.Lisp tool body idle refused because arbitrary eval, action, activation, model binding, provider call, LLM maintenance, tick loop, authority grant, executor arm, GEL admission, SelfGEL mutation, heartbeat activation, Actual activation, or continuity admission was requested.",
                timestampUtc);
        }

        var tempRoot = Path.Combine(Path.GetTempPath(), "project-bicycle-sli-tool-idle", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempRoot);

        try
        {
            WriteModules(tempRoot, modules, orderedModules);
            var scriptPath = WriteRunScript(tempRoot, orderedModules, normalized);
            var result = RunSbcl(runtimePath, scriptPath, request.Timeout ?? DefaultTimeout);
            var telemetry = ParseTelemetry(result.StandardOutput);
            var completed = result.ExitCode == 0 && IsTrue(telemetry, "tool-body-idle-state-completed");

            return CreateReceipt(
                normalized,
                completed
                    ? SliLispToolBodyIdleStateDisposition.CompletedCold
                    : SliLispToolBodyIdleStateDisposition.Withheld,
                completed
                    ? "sli-lisp-tool-body-idle-state-completed-cold"
                    : "sli-lisp-tool-body-idle-state-withheld",
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
                SliLispToolBodyIdleStateDisposition.Withheld,
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

    private static SliLispToolBodyIdleStateReceipt CreateReceipt(
        NormalizedToolBodyIdleStateRequest request,
        SliLispToolBodyIdleStateDisposition disposition,
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
                "sli-lisp-tool-body-idle://",
                outcomeCode,
                request.OperatorId,
                request.Domain,
                request.Role,
                request.JobClass,
                request.SessionId,
                request.TurnIndex.ToString(CultureInfo.InvariantCulture),
                request.InstalledSubstrateReceiptHandle,
                request.EcLoopReceiptHandle,
                request.WarmUseReceiptHandle,
                request.LabGelReceiptHandle,
                request.EngramCandidateHandle,
                request.EngramClosureReceiptHandle,
                request.LabGelReadbackReceiptHandle,
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
            InstalledSubstrateReceiptHandle: ReadValueOrFallback(telemetry, "source.installed-substrate-receipt", request.InstalledSubstrateReceiptHandle),
            EcLoopReceiptHandle: ReadValueOrFallback(telemetry, "source.ec-loop-receipt", request.EcLoopReceiptHandle),
            WarmUseReceiptHandle: ReadValueOrFallback(telemetry, "source.warm-use-receipt", request.WarmUseReceiptHandle),
            LabGelReceiptHandle: ReadValueOrFallback(telemetry, "source.lab-gel-receipt", request.LabGelReceiptHandle),
            EngramCandidateHandle: ReadValueOrFallback(telemetry, "source.engram-candidate", request.EngramCandidateHandle),
            EngramClosureReceiptHandle: ReadValueOrFallback(telemetry, "source.engram-closure", request.EngramClosureReceiptHandle),
            LabGelReadbackReceiptHandle: ReadValueOrFallback(telemetry, "source.lab-gel-readback", request.LabGelReadbackReceiptHandle),
            ThoughtForm: request.ThoughtForm,
            Telemetry: telemetry,
            ModuleNames: orderedModules,
            ModuleCount: orderedModules.Count,
            BoundedEntrypointCalled: boundedEntrypointCalled,
            LoadAttempted: loadAttempted,
            LoadSucceeded: loadSucceeded,
            ToolBodyIdleStateCompleted: IsTrue(telemetry, "tool-body-idle-state-completed"),
            IdleState: ReadValueOrFallback(telemetry, "tool-body.idle-state", string.Empty),
            MaintainedBySanctuary: IsTrue(telemetry, "tool-body.maintained-by-sanctuary"),
            MaintainedByLlm: IsTrue(telemetry, "tool-body.maintained-by-llm"),
            LlmMaintenanceRequired: IsTrue(telemetry, "tool-body.llm-maintenance-required"),
            LlmAdapterRequired: IsTrue(telemetry, "tool-body.llm-adapter-required"),
            ReadyForLlmAdapter: IsTrue(telemetry, "tool-body.ready-for-llm-adapter"),
            CanAcceptFutureRider: IsTrue(telemetry, "tool-body.can-accept-future-rider"),
            GovernanceSlmCandidateDesirable: IsTrue(telemetry, "governance-slm.candidate-desirable"),
            GovernanceSlmRoutingSwitchCandidate: IsTrue(telemetry, "governance-slm.routing-switch-candidate"),
            GovernanceSlmIntelligentSwitchCandidate: IsTrue(telemetry, "governance-slm.intelligent-switch-candidate"),
            GovernanceSlmPresent: IsTrue(telemetry, "governance-slm.present"),
            GovernanceSlmRequiredForIdle: IsTrue(telemetry, "governance-slm.required-for-idle"),
            GovernanceSlmMayDiscriminateEscalation: IsTrue(telemetry, "governance-slm.may-discriminate-escalation"),
            GovernanceSlmMayDiscernActionReadiness: IsTrue(telemetry, "governance-slm.may-discern-action-readiness"),
            GovernanceSlmDiscernmentAuthorizesAction: IsTrue(telemetry, "governance-slm.discernment-authorizes-action"),
            GovernanceSlmMayAuthorizeAction: IsTrue(telemetry, "governance-slm.may-authorize-action"),
            ModelAdapterPresent: IsTrue(telemetry, "tool-body.model-adapter-present") || IsTrue(telemetry, "llm-interconnect.model-adapter-present"),
            ModelBindingAllowed: IsTrue(telemetry, "llm-interconnect.model-binding") || IsTrue(telemetry, "model-binding"),
            ProviderCallAllowed: IsTrue(telemetry, "llm-interconnect.provider-call") || IsTrue(telemetry, "provider-call"),
            HiddenInternalsClaimed: IsTrue(telemetry, "llm-interconnect.hidden-internals-claim"),
            TickLoopRunning: IsTrue(telemetry, "tool-body.tick-loop-running"),
            TickMaintainedByLlm: IsTrue(telemetry, "tool-body.tick-maintained-by-llm"),
            IdleLoopHeld: IsTrue(telemetry, "tool-body.idle-loop-held"),
            ReturnToPrimeHeld: IsTrue(telemetry, "tool-body.return-to-prime-held"),
            OperatorReentryAvailable: IsTrue(telemetry, "tool-body.operator-reentry-available"),
            EcMaintainedInLisp: IsTrue(telemetry, "ec.maintained-in-lisp"),
            LocalEcHoldAvailable: IsTrue(telemetry, "ec.local-hold-available"),
            EngineCallRequired: IsTrue(telemetry, "engine-call.required"),
            LlmEngineCallRequired: IsTrue(telemetry, "llm-engine-call.required"),
            ExternalEngineCallRequired: IsTrue(telemetry, "external-engine-call.required"),
            OrganCount: int.TryParse(ReadValue(telemetry, "organ.count"), NumberStyles.Integer, CultureInfo.InvariantCulture, out var organCount) ? organCount : 0,
            AllRequiredOrgansPresent: IsTrue(telemetry, "organ.all-required-present"),
            SanctuaryGelPresent: IsTrue(telemetry, "organ.base.sanctuary-gel.present"),
            SanctuaryGoaPresent: IsTrue(telemetry, "organ.base.sanctuary-goa.present"),
            SanctuaryMosPresent: IsTrue(telemetry, "organ.base.sanctuary-mos.present"),
            SanctuaryVaultPresent: IsTrue(telemetry, "organ.base.sanctuary-vault.present"),
            SanctuaryCGelPresent: IsTrue(telemetry, "organ.condensate.sanctuary-cgel.present"),
            SanctuaryCGoaPresent: IsTrue(telemetry, "organ.condensate.sanctuary-cgoa.present"),
            SanctuaryCMosPresent: IsTrue(telemetry, "organ.condensate.sanctuary-cmos.present"),
            SanctuaryCVaultPresent: IsTrue(telemetry, "organ.condensate.sanctuary-cvault.present"),
            PrimePresent: IsTrue(telemetry, "organ.role.prime.present"),
            CrypticPresent: IsTrue(telemetry, "organ.role.cryptic.present"),
            StewardPresent: IsTrue(telemetry, "organ.role.steward.present"),
            GoverningCmeCSharpBodiesBuilt: IsTrue(telemetry, "governing-cme.csharp-bodies-built"),
            GoverningCmeActualizedCold: IsTrue(telemetry, "governing-cme.actualized-cold"),
            PrimeGoverningCmeBuilt: IsTrue(telemetry, "governing-cme.prime.built"),
            CrypticGoverningCmeBuilt: IsTrue(telemetry, "governing-cme.cryptic.built"),
            StewardGoverningCmeBuilt: IsTrue(telemetry, "governing-cme.steward.built"),
            GoverningCmeSliLispActualizationSurfacesReady: IsTrue(telemetry, "governing-cme.sli-lisp-actualization-surfaces-ready"),
            GoverningCmeMaintainsIdleState: IsTrue(telemetry, "governing-cme.maintains-idle-state"),
            GoverningHeartbeatHealthy: IsTrue(telemetry, "governing-heartbeat.healthy"),
            BondedCmeCallAvailable: IsTrue(telemetry, "bonded-cme-call.available"),
            SanctuaryGovernanceMonitoringReady: IsTrue(telemetry, "sanctuary-governance.monitoring-ready"),
            SliLispLoaded: IsTrue(telemetry, "membrane.sli-lisp.loaded"),
            SliLispPrimePresent: IsTrue(telemetry, "membrane.sli-lisp-prime.present"),
            SliLispCrypticPresent: IsTrue(telemetry, "membrane.sli-lisp-cryptic.present"),
            LispControlMatrixPresent: IsTrue(telemetry, "membrane.lisp-control-matrix.present"),
            ListeningFramePresent: IsTrue(telemetry, "membrane.listening-frame.present"),
            CompassPresent: IsTrue(telemetry, "membrane.compass.present"),
            SoulFrameRoutePresent: IsTrue(telemetry, "membrane.soulframe-route.present"),
            AgentiCoreRoutePresent: IsTrue(telemetry, "membrane.agenticore-route.present"),
            EcLoopReady: IsTrue(telemetry, "lane.ec-loop.ready"),
            TypedWarmUseReady: IsTrue(telemetry, "lane.typed-warm-use.ready"),
            LabGelReady: IsTrue(telemetry, "lane.lab-gel.ready"),
            AgentEngineIdleRequired: IsTrue(telemetry, "lane.agent-engine-idle.required"),
            SourceLineageHeld: IsTrue(telemetry, "source-lineage.held"),
            SourceEngramClosureAcceptedCold: IsTrue(telemetry, "source.engram-closure.accepted-cold"),
            SourceLabGelReadbackAcceptedCold: IsTrue(telemetry, "source.lab-gel-readback.accepted-cold"),
            AuthorityGrantAbsent: IsTrue(telemetry, "authority-grant.absent"),
            ActionExecutorLocked: IsTrue(telemetry, "action-executor.locked"),
            GelAdmissionLocked: IsTrue(telemetry, "gel-admission.locked"),
            SelfGelMutationLocked: IsTrue(telemetry, "selfgel-mutation.locked"),
            HeartbeatLocked: IsTrue(telemetry, "heartbeat.locked"),
            CmeActualLocked: IsTrue(telemetry, "cme-actual.locked"),
            SanctuaryActualLocked: IsTrue(telemetry, "sanctuary-actual.locked"),
            TypedScopeAccepted: IsTrue(telemetry, "typed-scope.accepted"),
            SessionLineageWitnessed: IsTrue(telemetry, "session-lineage.witnessed"),
            ListeningFrameReceived: IsTrue(telemetry, "listening-frame.received"),
            SliMembraneInterpretedPredicatePressure: IsTrue(telemetry, "sli-membrane.interpreted-predicate-pressure"),
            CompassOrientedPressure: IsTrue(telemetry, "compass.oriented-pressure"),
            CompassCoolingRequired: IsTrue(telemetry, "compass.cooling-required"),
            SoulFrameReceivedListeningFrame: IsTrue(telemetry, "soulframe.received-listening-frame"),
            AgentiCoreReceivedCompassPressure: IsTrue(telemetry, "agenticore.received-compass-pressure"),
            ThinkingAboutThinkingTelemetryProduced: IsTrue(telemetry, "thinking-about-thinking.telemetry-produced"),
            StewardReviewed: IsTrue(telemetry, "steward.reviewed"),
            ArbitraryEvaluationAllowed: IsTrue(telemetry, "arbitrary-lisp-evaluation"),
            RuntimeActionAllowed: IsTrue(telemetry, "runtime-action"),
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
        NormalizedToolBodyIdleStateRequest request)
    {
        var scriptPath = Path.Combine(tempRoot, "run-tool-body-idle-state.lisp");
        var builder = new StringBuilder();

        builder.AppendLine("(format t \"SAN-SLI-TOOL-BODY-IDLE-BEGIN~%\")");
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

        builder.AppendLine("(unless (and (find-package :sli-core) (fboundp 'sli-core::run-tool-body-idle-state)) (error \"bounded tool body idle entrypoint missing\"))");
        builder.Append("(let ((result (sli-core::run-tool-body-idle-state ");
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
        builder.Append(ToLispString(request.InstalledSubstrateReceiptHandle));
        builder.Append(' ');
        builder.Append(ToLispString(request.EcLoopReceiptHandle));
        builder.Append(' ');
        builder.Append(ToLispString(request.WarmUseReceiptHandle));
        builder.Append(' ');
        builder.Append(ToLispString(request.LabGelReceiptHandle));
        builder.Append(' ');
        builder.Append(ToLispString(request.EngramCandidateHandle));
        builder.Append(' ');
        builder.Append(ToLispString(request.EngramClosureReceiptHandle));
        builder.Append(' ');
        builder.Append(ToLispString(request.LabGelReadbackReceiptHandle));
        builder.Append(' ');
        builder.Append(ToLispString(request.ThoughtForm));
        builder.AppendLine(")))");
        builder.AppendLine("  (format t \"SAN-SLI-TOOL-BODY-IDLE-OK~%\")");
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
                StandardError: "SBCL timed out while running bounded tool body idle state.");
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

    private static NormalizedToolBodyIdleStateRequest NormalizeRequest(SliLispToolBodyIdleStateRequest request) =>
        new(
            NormalizeValue(request.OperatorId, "Sanctuary.ID"),
            NormalizeValue(request.Domain, "Sanctuary"),
            NormalizeValue(request.Role, "InstalledBody"),
            NormalizeValue(request.JobClass, "ColdBench"),
            NormalizeValue(request.SessionId, "tool-body-idle-session"),
            Math.Max(0, request.TurnIndex),
            NormalizeValue(request.InstalledSubstrateReceiptHandle, "installed-substrate-receipt-missing"),
            NormalizeValue(request.EcLoopReceiptHandle, "ec-loop-receipt-missing"),
            NormalizeValue(request.WarmUseReceiptHandle, "warm-use-receipt-missing"),
            NormalizeValue(request.LabGelReceiptHandle, "lab-gel-receipt-missing"),
            NormalizeValue(request.EngramCandidateHandle, "engram-candidate-missing"),
            NormalizeValue(request.EngramClosureReceiptHandle, "engram-closure-missing"),
            NormalizeValue(request.LabGelReadbackReceiptHandle, "lab-gel-readback-missing"),
            NormalizeValue(request.ThoughtForm, "cold tool body idle without LLM maintenance"));

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

    private sealed record NormalizedToolBodyIdleStateRequest(
        string OperatorId,
        string Domain,
        string Role,
        string JobClass,
        string SessionId,
        int TurnIndex,
        string InstalledSubstrateReceiptHandle,
        string EcLoopReceiptHandle,
        string WarmUseReceiptHandle,
        string LabGelReceiptHandle,
        string EngramCandidateHandle,
        string EngramClosureReceiptHandle,
        string LabGelReadbackReceiptHandle,
        string ThoughtForm);

    private sealed record ProcessResult(
        int ExitCode,
        string StandardOutput,
        string StandardError);
}
