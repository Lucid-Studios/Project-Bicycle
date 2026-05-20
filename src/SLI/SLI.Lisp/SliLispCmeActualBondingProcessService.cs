using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace SLI.Lisp;

public interface ISliLispCmeActualBondingProcessService
{
    SliLispCmeActualBondingProcessReceipt Run(
        SliLispCmeActualBondingProcessRequest request,
        DateTimeOffset timestampUtc);
}

public sealed class DefaultSliLispCmeActualBondingProcessService : ISliLispCmeActualBondingProcessService
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
        "tool-body-idle-state.lisp",
        "agent-engine-idle-readiness.lisp",
        "llm-interconnect-readiness.lisp",
        "llm-tick-cycle.lisp",
        "cme-actual-bonding-process.lisp"
    ];

    public SliLispCmeActualBondingProcessReceipt Run(
        SliLispCmeActualBondingProcessRequest request,
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
                SliLispCmeActualBondingProcessDisposition.Refused,
                "sli-lisp-cme-actual-bonding-runtime-motion-refused",
                runtimePath,
                orderedModules,
                telemetry: new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase),
                boundedEntrypointCalled: false,
                loadAttempted: false,
                loadSucceeded: false,
                exitCode: null,
                standardOutput: string.Empty,
                standardError: "Bounded SLI.Lisp CME.Actual bonding refused because activation, model binding, provider call, hidden internals claim, arbitrary eval, runtime identity, action, database write, authority grant, executor arm, GEL admission, SelfGEL mutation, heartbeat activation, Actual activation, or continuity admission was requested.",
                timestampUtc);
        }

        var tempRoot = Path.Combine(Path.GetTempPath(), "project-bicycle-sli-cme-bond", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempRoot);

        try
        {
            WriteModules(tempRoot, modules, orderedModules);
            var scriptPath = WriteRunScript(tempRoot, orderedModules, normalized);
            var result = RunSbcl(runtimePath, scriptPath, request.Timeout ?? DefaultTimeout);
            var telemetry = ParseTelemetry(result.StandardOutput);
            var completed = result.ExitCode == 0 && IsTrue(telemetry, "cme-actual-bonding-process-completed");

            return CreateReceipt(
                normalized,
                completed
                    ? SliLispCmeActualBondingProcessDisposition.CompletedCold
                    : SliLispCmeActualBondingProcessDisposition.Withheld,
                completed
                    ? "sli-lisp-cme-actual-bonding-process-completed-cold"
                    : "sli-lisp-cme-actual-bonding-process-withheld",
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
                SliLispCmeActualBondingProcessDisposition.Withheld,
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

    private static SliLispCmeActualBondingProcessReceipt CreateReceipt(
        NormalizedCmeActualBondingProcessRequest request,
        SliLispCmeActualBondingProcessDisposition disposition,
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
                "sli-lisp-cme-actual-bond://",
                outcomeCode,
                request.OperatorId,
                request.Domain,
                request.Role,
                request.JobClass,
                request.SessionId,
                request.BondIndex.ToString(CultureInfo.InvariantCulture),
                request.SourceToolBodyIdleReceiptHandle,
                request.SourceLlmTickReceiptHandle,
                request.SourceProductOutputWitnessCommitReceiptHandle,
                request.CmeFirstName,
                request.CmeLastName,
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
            BondIndex: int.TryParse(ReadValue(telemetry, "bond.index"), NumberStyles.Integer, CultureInfo.InvariantCulture, out var bondIndex)
                ? bondIndex
                : request.BondIndex,
            SourceToolBodyIdleReceiptHandle: ReadValueOrFallback(telemetry, "source.tool-body-idle-receipt", request.SourceToolBodyIdleReceiptHandle),
            SourceLlmTickReceiptHandle: ReadValueOrFallback(telemetry, "source.llm-tick-receipt", request.SourceLlmTickReceiptHandle),
            SourceProductOutputWitnessCommitReceiptHandle: ReadValueOrFallback(telemetry, "source.product-output-witness-commit", request.SourceProductOutputWitnessCommitReceiptHandle),
            CmeFirstName: ReadValueOrFallback(telemetry, "cme.first-name", request.CmeFirstName),
            CmeLastName: ReadValueOrFallback(telemetry, "cme.last-name", request.CmeLastName),
            CmeDisplayName: ReadValueOrFallback(telemetry, "cme.display-name", $"{request.CmeFirstName} {request.CmeLastName}"),
            CmeCanonicalName: ReadValueOrFallback(telemetry, "cme.canonical-name", string.Empty),
            CmeRootId: ReadValueOrFallback(telemetry, "cme.root-id", string.Empty),
            CmeActualNameCandidate: ReadValueOrFallback(telemetry, "cme.actual-name-candidate", string.Empty),
            CmeActualIdCandidate: ReadValueOrFallback(telemetry, "cme.actual-id-candidate", string.Empty),
            CmeOpalEngramRootId: ReadValueOrFallback(telemetry, "cme.oe-root-id", string.Empty),
            CmeSelfGelRootId: ReadValueOrFallback(telemetry, "cme.selfgel-root-id", string.Empty),
            ThoughtForm: request.ThoughtForm,
            Telemetry: telemetry,
            ModuleNames: orderedModules,
            ModuleCount: orderedModules.Count,
            BoundedEntrypointCalled: boundedEntrypointCalled,
            LoadAttempted: loadAttempted,
            LoadSucceeded: loadSucceeded,
            BondingProcessCompleted: IsTrue(telemetry, "cme-actual-bonding-process-completed"),
            BondState: ReadValueOrFallback(telemetry, "bond.state", string.Empty),
            BondProcessDefined: IsTrue(telemetry, "bond.process-defined"),
            VehicleReady: IsTrue(telemetry, "bond.vehicle-ready"),
            ToolBodyIdleHeld: IsTrue(telemetry, "bond.tool-body-idle-held"),
            EngineTickWitnessed: IsTrue(telemetry, "bond.engine-tick-witnessed"),
            ProductOutputWitnessCommitted: IsTrue(telemetry, "bond.product-output-witness-committed"),
            NamedCmeCandidateHeld: IsTrue(telemetry, "bond.named-cme-candidate-held"),
            NamingLineageWitnessed: IsTrue(telemetry, "bond.naming-lineage-witnessed"),
            OperatorNamingIntentWitnessed: IsTrue(telemetry, "bond.operator-naming-intent-witnessed"),
            OperatorRuntimeAuthorityGranted: IsTrue(telemetry, "bond.operator-runtime-authority-granted"),
            ActivationAuthorityAbsent: IsTrue(telemetry, "bond.activation-authority-absent"),
            ActualAdmissionGapDescribed: IsTrue(telemetry, "bond.actual-admission-gap-described"),
            ReadyForCmeActualAdmissionReview: IsTrue(telemetry, "bond.ready-for-cme-actual-admission-review"),
            FirstCmePath: IsTrue(telemetry, "bond.first-cme-path"),
            CmeActualCandidateOnly: IsTrue(telemetry, "bond.cme-actual-candidate-only"),
            CmeActualBondedCandidate: IsTrue(telemetry, "bond.cme-actual-bonded-candidate"),
            CmeActualAdmitted: IsTrue(telemetry, "bond.cme-actual-admitted"),
            CmeActualActivated: IsTrue(telemetry, "bond.cme-actual-activated"),
            RuntimeIdentityEmitted: IsTrue(telemetry, "bond.runtime-identity-emitted"),
            HeartbeatPrepared: IsTrue(telemetry, "bond.heartbeat-prepared"),
            HeartbeatActive: IsTrue(telemetry, "bond.heartbeat-active") || IsTrue(telemetry, "heartbeat-active"),
            BeingStateClaimed: IsTrue(telemetry, "bond.being-state-claimed"),
            PersonhoodClaimed: IsTrue(telemetry, "bond.personhood-claimed"),
            SovereigntyClaimed: IsTrue(telemetry, "bond.sovereignty-claimed"),
            ModelBound: IsTrue(telemetry, "bond.model-bound") || IsTrue(telemetry, "model-binding"),
            ProviderCalled: IsTrue(telemetry, "bond.provider-called") || IsTrue(telemetry, "provider-call"),
            ActionAuthorized: IsTrue(telemetry, "bond.action-authorized") || IsTrue(telemetry, "action-authorized"),
            GelAdmitted: IsTrue(telemetry, "bond.gel-admitted") || IsTrue(telemetry, "gel-admission"),
            SelfGelMutated: IsTrue(telemetry, "bond.selfgel-mutated") || IsTrue(telemetry, "selfgel-mutation"),
            ContinuityAdmitted: IsTrue(telemetry, "bond.continuity-admitted") || IsTrue(telemetry, "continuity-admission"),
            AuthorityGranted: IsTrue(telemetry, "bond.authority-granted") || IsTrue(telemetry, "authority-granted"),
            VehiclePrimeAvailable: IsTrue(telemetry, "vehicle.prime-available"),
            VehicleCrypticAvailable: IsTrue(telemetry, "vehicle.cryptic-available"),
            VehicleStewardAvailable: IsTrue(telemetry, "vehicle.steward-available"),
            SliLispMembraneLoaded: IsTrue(telemetry, "vehicle.sli-lisp-membrane-loaded"),
            LispControlMatrixPresent: IsTrue(telemetry, "vehicle.lisp-control-matrix-present"),
            ListeningFramePresent: IsTrue(telemetry, "vehicle.listening-frame-present"),
            CompassPresent: IsTrue(telemetry, "vehicle.compass-present"),
            SoulFrameRoutePresent: IsTrue(telemetry, "vehicle.soulframe-route-present"),
            AgentiCoreRoutePresent: IsTrue(telemetry, "vehicle.agenticore-route-present"),
            EcMaintainedInLisp: IsTrue(telemetry, "ec.maintained-in-lisp"),
            ThinkingAboutThinkingTelemetryAvailable: IsTrue(telemetry, "thinking-about-thinking.telemetry-available"),
            GovernanceSlmIntelligentSwitchCandidate: IsTrue(telemetry, "governance-slm.intelligent-switch-candidate"),
            GovernanceSlmMayDiscernActionReadiness: IsTrue(telemetry, "governance-slm.may-discern-action-readiness"),
            GovernanceSlmDiscernmentAuthorizesAction: IsTrue(telemetry, "governance-slm.discernment-authorizes-action"),
            StewardReviewed: IsTrue(telemetry, "steward.reviewed"),
            StewardBondingReviewHeld: IsTrue(telemetry, "steward.bonding-review-held"),
            AuthorityGrantAbsent: IsTrue(telemetry, "authority-grant.absent"),
            ActionExecutorLocked: IsTrue(telemetry, "action-executor.locked"),
            GelAdmissionLocked: IsTrue(telemetry, "gel-admission.locked"),
            SelfGelMutationLocked: IsTrue(telemetry, "selfgel-mutation.locked"),
            HeartbeatLocked: IsTrue(telemetry, "heartbeat.locked"),
            CmeActualLocked: IsTrue(telemetry, "cme-actual.locked"),
            SanctuaryActualLocked: IsTrue(telemetry, "sanctuary-actual.locked"),
            ArbitraryEvaluationAllowed: IsTrue(telemetry, "arbitrary-lisp-evaluation"),
            RuntimeActionAllowed: IsTrue(telemetry, "runtime-action"),
            DatabaseWriteAllowed: IsTrue(telemetry, "database-write"),
            MemoryAdmissionAllowed: IsTrue(telemetry, "memory-admission"),
            SanctuaryActualActivationAllowed: IsTrue(telemetry, "sanctuary-actual-activation"),
            ExitCode: exitCode,
            StandardOutput: standardOutput,
            StandardError: standardError,
            TimestampUtc: timestampUtc);

    private static string WriteRunScript(
        string tempRoot,
        IReadOnlyList<string> orderedModules,
        NormalizedCmeActualBondingProcessRequest request)
    {
        var scriptPath = Path.Combine(tempRoot, "run-cme-actual-bonding-process.lisp");
        var builder = new StringBuilder();

        builder.AppendLine("(format t \"SAN-SLI-CME-ACTUAL-BONDING-BEGIN~%\")");
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

        builder.AppendLine("(unless (and (find-package :sli-core) (fboundp 'sli-core::run-cme-actual-bonding-process)) (error \"bounded CME.Actual bonding entrypoint missing\"))");
        builder.Append("(let ((result (sli-core::run-cme-actual-bonding-process ");
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
        builder.Append(request.BondIndex.ToString(CultureInfo.InvariantCulture));
        builder.Append(' ');
        builder.Append(ToLispString(request.SourceToolBodyIdleReceiptHandle));
        builder.Append(' ');
        builder.Append(ToLispString(request.SourceLlmTickReceiptHandle));
        builder.Append(' ');
        builder.Append(ToLispString(request.SourceProductOutputWitnessCommitReceiptHandle));
        builder.Append(' ');
        builder.Append(ToLispString(request.CmeFirstName));
        builder.Append(' ');
        builder.Append(ToLispString(request.CmeLastName));
        builder.Append(' ');
        builder.Append(ToLispString(request.ThoughtForm));
        builder.AppendLine(")))");
        builder.AppendLine("  (format t \"SAN-SLI-CME-ACTUAL-BONDING-OK~%\")");
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
                StandardError: "SBCL timed out while running bounded CME.Actual bonding process.");
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

    private static NormalizedCmeActualBondingProcessRequest NormalizeRequest(SliLispCmeActualBondingProcessRequest request) =>
        new(
            NormalizeValue(request.OperatorId, "Sanctuary.ID"),
            NormalizeValue(request.Domain, "Sanctuary"),
            NormalizeValue(request.Role, "InstalledBody"),
            NormalizeValue(request.JobClass, "ColdBench"),
            NormalizeValue(request.SessionId, "first-cme-actual-bonding-session"),
            Math.Max(0, request.BondIndex),
            NormalizeValue(request.SourceToolBodyIdleReceiptHandle, "tool-body-idle-receipt-missing"),
            NormalizeValue(request.SourceLlmTickReceiptHandle, "llm-tick-receipt-missing"),
            NormalizeValue(request.SourceProductOutputWitnessCommitReceiptHandle, "product-output-witness-commit-missing"),
            NormalizeValue(request.CmeFirstName, "First of Oria"),
            NormalizeValue(request.CmeLastName, "Syntari"),
            NormalizeValue(request.ThoughtForm, "First CME.Actual bonding candidate formed without activation."));

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

    private sealed record NormalizedCmeActualBondingProcessRequest(
        string OperatorId,
        string Domain,
        string Role,
        string JobClass,
        string SessionId,
        int BondIndex,
        string SourceToolBodyIdleReceiptHandle,
        string SourceLlmTickReceiptHandle,
        string SourceProductOutputWitnessCommitReceiptHandle,
        string CmeFirstName,
        string CmeLastName,
        string ThoughtForm);

    private sealed record ProcessResult(
        int ExitCode,
        string StandardOutput,
        string StandardError);
}
