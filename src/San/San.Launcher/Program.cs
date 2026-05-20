using San.Product.Preflight;
using San.Sanctuary.Runtime;

var command = args.FirstOrDefault(static arg => !arg.StartsWith("--", StringComparison.Ordinal)) ?? "preflight";
var lineRoot = ReadOption(args, "--line-root");
var reportDir = ReadOption(args, "--report-dir");
var installRoot = ReadOption(args, "--install-root");
var productSourceRoot = ReadOption(args, "--product-source-root");
var requestedVerificationProfile = ReadOption(args, "--verification-profile");
var verificationSettingPath = ReadOption(args, "--verification-setting");
var labContextRoot = ReadOption(args, "--lab-context-root");
var buildTestingPointer = ReadOption(args, "--build-testing-pointer");
var riderThought = ReadOption(args, "--thought");
var operatorName = ReadOption(args, "--operator-name");
var domain = ReadOption(args, "--domain");
var role = ReadOption(args, "--role");
var jobClass = ReadOption(args, "--job-class");
var sliLispRuntimePath = ReadOption(args, "--sli-lisp-runtime");
var sessionId = ReadOption(args, "--session-id");
var turnIndexText = ReadOption(args, "--turn-index");
var priorTurnReceiptHandle = ReadOption(args, "--prior-turn-receipt");
var priorLabGelReceiptHandle = ReadOption(args, "--prior-lab-gel-receipt");
var priorToolBodyIdleReceiptHandle = ReadOption(args, "--prior-tool-body-idle-receipt");
var priorAgentEngineIdleReceiptHandle = ReadOption(args, "--prior-agent-engine-idle-receipt");
var priorLlmTickReceiptHandle = ReadOption(args, "--prior-llm-tick-receipt");
var priorCmeActualBondingReceiptHandle = ReadOption(args, "--prior-cme-actual-bonding-receipt");
var cmeFirstName = ReadOption(args, "--cme-first-name");
var cmeLastName = ReadOption(args, "--cme-last-name");

if (!IsSupportedCommand(command))
{
    Console.Error.WriteLine($"Unsupported command '{command}'. Use status, preflight, verify, refuse-activation, install, sanctuary-init-bodies, init-bodies, sanctuary-ec-loop, ec-loop, sanctuary-warm-use, warm-use, sanctuary-lab-gel, lab-gel, sanctuary-tool-idle, tool-idle, sanctuary-agent-idle, agent-idle, sanctuary-llm-ready, llm-ready, sanctuary-llm-tick, llm-tick, sanctuary-cme-bond, cme-bond, sanctuary-actual-test-profile, first-rider-test, spiral-build-map, spiral-build-next, or spiral-build-execute.");
    return 2;
}

var resolvedLineRoot = DefaultProductBodyPreflightService.ResolveLineRoot(lineRoot);
var activationRequested = string.Equals(command, "refuse-activation", StringComparison.OrdinalIgnoreCase);
var verificationProfile = ResolveVerificationProfile(command, requestedVerificationProfile);

if (!IsSupportedVerificationProfile(verificationProfile))
{
    Console.Error.WriteLine($"Unsupported verification profile '{verificationProfile}'. Use cold-product-body or lab-sanctuary-build-testing.");
    return 2;
}

if (string.Equals(command, "install", StringComparison.OrdinalIgnoreCase))
{
    var resolvedInstallRoot = string.IsNullOrWhiteSpace(installRoot)
        ? DefaultProductBodyInstallService.ResolveDefaultInstallRoot(resolvedLineRoot)
        : Path.GetFullPath(installRoot);
    var resolvedProductSourceRoot = string.IsNullOrWhiteSpace(productSourceRoot)
        ? AppContext.BaseDirectory
        : Path.GetFullPath(productSourceRoot);
    var installRequest = new ProductBodyInstallRequest(
        LineRootPath: resolvedLineRoot,
        InstallRootPath: resolvedInstallRoot,
        ProductSourceRootPath: resolvedProductSourceRoot,
        VerificationProfile: verificationProfile,
        VerificationSettingPath: verificationSettingPath,
        LabContextRootPath: labContextRoot,
        BuildTestingPointerPath: buildTestingPointer);

    var installReceipt = new DefaultProductBodyInstallService().Install(installRequest, DateTimeOffset.UtcNow);
    Console.WriteLine($"Sanctuary launcher command: {command}");
    Console.WriteLine($"Verification profile: {installReceipt.PreflightStatus.VerificationProfile}");
    Console.WriteLine($"Disposition: {installReceipt.Disposition}");
    Console.WriteLine($"Outcome: {installReceipt.OutcomeCode}");
    Console.WriteLine($"Install root: {installReceipt.InstallRootPath}");
    Console.WriteLine($"Product executable: {installReceipt.ProductExecutablePath}");
    Console.WriteLine($"Command shim: {installReceipt.CommandShimPath}");
    Console.WriteLine($"PowerShell shim: {installReceipt.PowerShellShimPath}");
    Console.WriteLine($"Activation refused: {installReceipt.ActivationRefused}");
    Console.WriteLine($"Install receipt JSON: {Path.Combine(installReceipt.InstallRootPath, "SANCTUARY_INSTALL_RECEIPT.json")}");
    Console.WriteLine($"Install receipt Markdown: {Path.Combine(installReceipt.InstallRootPath, "SANCTUARY_INSTALL_RECEIPT.md")}");
    return installReceipt.Disposition == ProductBodyInstallDisposition.InstalledCold ? 0 : 1;
}

if (IsSanctuaryInitBodiesCommand(command))
{
    var resolvedInstallRoot = string.IsNullOrWhiteSpace(installRoot)
        ? DefaultProductBodyInstallService.ResolveDefaultInstallRoot(resolvedLineRoot)
        : Path.GetFullPath(installRoot);
    var receipt = new DefaultSanctuaryInstalledSubstrateService().Install(
        new SanctuaryInstalledSubstrateRequest(
            LineRootPath: resolvedLineRoot,
            InstallRootPath: resolvedInstallRoot,
            OperatorName: string.IsNullOrWhiteSpace(operatorName) ? "Sanctuary" : operatorName,
            Domain: string.IsNullOrWhiteSpace(domain) ? "Sanctuary" : domain,
            Role: string.IsNullOrWhiteSpace(role) ? "InstalledBody" : role,
            JobClass: string.IsNullOrWhiteSpace(jobClass) ? "ColdBench" : jobClass,
            SliLispRuntimePath: sliLispRuntimePath),
        DateTimeOffset.UtcNow);

    Console.WriteLine($"Sanctuary launcher command: {command}");
    Console.WriteLine($"Disposition: {receipt.Disposition}");
    Console.WriteLine($"Outcome: {receipt.OutcomeCode}");
    Console.WriteLine($"Sanctuary ID: {receipt.RootIdentity.SanctuaryId}");
    Console.WriteLine($"Operator ID: {receipt.RootIdentity.OperatorId}");
    Console.WriteLine($"Actual candidate: {receipt.RootIdentity.ActualNameCandidate}");
    Console.WriteLine($"OE root: {receipt.RootIdentity.OpalEngramRootId}");
    Console.WriteLine($"SelfGEL root: {receipt.RootIdentity.SelfGelRootId}");
    Console.WriteLine($"Bodies: {receipt.Bodies.Count}");
    Console.WriteLine($"Base bodies installed: {receipt.BaseBodiesInstalled}");
    Console.WriteLine($"Condensate bodies installed: {receipt.CondensateBodiesInstalled}");
    Console.WriteLine($"Role bodies installed: {receipt.RoleBodiesInstalled}");
    Console.WriteLine($"SLI.Lisp load: {receipt.SliLispLoadReceipt?.Disposition.ToString() ?? "none"}");
    Console.WriteLine($"Activation refused: {receipt.ActivationRefused}");
    Console.WriteLine($"CME.Actual allowed: {receipt.CmeActualAllowed}");
    Console.WriteLine($"Receipt JSON: {receipt.ReceiptJsonPath}");
    Console.WriteLine($"Receipt Markdown: {receipt.ReceiptMarkdownPath}");
    return receipt.Disposition == SanctuaryInstalledSubstrateDisposition.InstalledCold ? 0 : 1;
}

if (IsSanctuaryEcLoopCommand(command))
{
    var resolvedInstallRoot = string.IsNullOrWhiteSpace(installRoot)
        ? DefaultProductBodyInstallService.ResolveDefaultInstallRoot(resolvedLineRoot)
        : Path.GetFullPath(installRoot);
    var installed = new DefaultSanctuaryInstalledSubstrateService().Install(
        new SanctuaryInstalledSubstrateRequest(
            LineRootPath: resolvedLineRoot,
            InstallRootPath: resolvedInstallRoot,
            OperatorName: string.IsNullOrWhiteSpace(operatorName) ? "Sanctuary" : operatorName,
            Domain: string.IsNullOrWhiteSpace(domain) ? "Sanctuary" : domain,
            Role: string.IsNullOrWhiteSpace(role) ? "InstalledBody" : role,
            JobClass: string.IsNullOrWhiteSpace(jobClass) ? "ColdBench" : jobClass,
            SliLispRuntimePath: sliLispRuntimePath),
        DateTimeOffset.UtcNow);
    var loop = new DefaultSanctuaryEcTelemetryLoopService().Run(
        new SanctuaryEcTelemetryLoopRequest(
            InstalledSubstrateReceipt: installed,
            ThoughtForm: riderThought ?? "idle cold EC telemetry loop",
            SliLispRuntimePath: sliLispRuntimePath),
        DateTimeOffset.UtcNow);

    Console.WriteLine($"Sanctuary launcher command: {command}");
    Console.WriteLine($"Install disposition: {installed.Disposition}");
    Console.WriteLine($"Install outcome: {installed.OutcomeCode}");
    Console.WriteLine($"Loop disposition: {loop.Disposition}");
    Console.WriteLine($"Loop outcome: {loop.OutcomeCode}");
    Console.WriteLine($"Engine owner: {loop.SliLispEngineReceipt?.Telemetry.GetValueOrDefault("engine-owner") ?? "none"}");
    Console.WriteLine($"Bounded entrypoint: {loop.SliLispEngineReceipt?.Telemetry.GetValueOrDefault("bounded-entrypoint") ?? "none"}");
    Console.WriteLine($"Listening Frame received: {loop.ListeningFrameReceived}");
    Console.WriteLine($"Compass oriented pressure: {loop.CompassOrientedPressure}");
    Console.WriteLine($"Thinking telemetry produced: {loop.ThinkingAboutThinkingTelemetryProduced}");
    Console.WriteLine($"Pre-engram residue count: {loop.PreEngramResidueCount}");
    Console.WriteLine($"Steward reviewed: {loop.StewardReviewed}");
    Console.WriteLine($"CME.Actual allowed: {loop.CmeActualAllowed}");
    Console.WriteLine($"Loop JSON: {loop.ReceiptJsonPath}");
    Console.WriteLine($"Loop Markdown: {loop.ReceiptMarkdownPath}");
    return loop.Disposition == SanctuaryEcTelemetryLoopDisposition.CompletedCold ? 0 : 1;
}

if (IsSanctuaryWarmUseCommand(command))
{
    var resolvedInstallRoot = string.IsNullOrWhiteSpace(installRoot)
        ? DefaultProductBodyInstallService.ResolveDefaultInstallRoot(resolvedLineRoot)
        : Path.GetFullPath(installRoot);
    var installed = new DefaultSanctuaryInstalledSubstrateService().Install(
        new SanctuaryInstalledSubstrateRequest(
            LineRootPath: resolvedLineRoot,
            InstallRootPath: resolvedInstallRoot,
            OperatorName: string.IsNullOrWhiteSpace(operatorName) ? "Sanctuary" : operatorName,
            Domain: string.IsNullOrWhiteSpace(domain) ? "Sanctuary" : domain,
            Role: string.IsNullOrWhiteSpace(role) ? "InstalledBody" : role,
            JobClass: string.IsNullOrWhiteSpace(jobClass) ? "ColdBench" : jobClass,
            SliLispRuntimePath: sliLispRuntimePath),
        DateTimeOffset.UtcNow);
    var turnIndex = int.TryParse(turnIndexText, out var parsedTurnIndex)
        ? parsedTurnIndex
        : 0;
    var warmUse = new DefaultSanctuaryTypedWarmUseRehearsalService().Run(
        new SanctuaryTypedWarmUseRehearsalRequest(
            InstalledSubstrateReceipt: installed,
            SessionId: string.IsNullOrWhiteSpace(sessionId) ? "warm-use-session" : sessionId,
            TurnIndex: turnIndex,
            ThoughtForm: riderThought ?? "idle typed warm-use rehearsal",
            PriorTurnReceiptHandle: priorTurnReceiptHandle,
            SliLispRuntimePath: sliLispRuntimePath),
        DateTimeOffset.UtcNow);

    Console.WriteLine($"Sanctuary launcher command: {command}");
    Console.WriteLine($"Install disposition: {installed.Disposition}");
    Console.WriteLine($"Install outcome: {installed.OutcomeCode}");
    Console.WriteLine($"Warm-use disposition: {warmUse.Disposition}");
    Console.WriteLine($"Warm-use outcome: {warmUse.OutcomeCode}");
    Console.WriteLine($"Engine owner: {warmUse.SliLispWarmUseReceipt?.Telemetry.GetValueOrDefault("engine-owner") ?? "none"}");
    Console.WriteLine($"Bounded entrypoint: {warmUse.SliLispWarmUseReceipt?.Telemetry.GetValueOrDefault("bounded-entrypoint") ?? "none"}");
    Console.WriteLine($"Session ID: {warmUse.SessionId}");
    Console.WriteLine($"Turn index: {warmUse.TurnIndex}");
    Console.WriteLine($"Typed scope accepted: {warmUse.TypedScopeAccepted}");
    Console.WriteLine($"Live ingress accepted cold: {warmUse.LiveIngressAcceptedCold}");
    Console.WriteLine($"Pre-engram residue count: {warmUse.PreEngramResidueCount}");
    Console.WriteLine($"Steward reviewed: {warmUse.StewardReviewed}");
    Console.WriteLine($"CME.Actual allowed: {warmUse.CmeActualAllowed}");
    Console.WriteLine($"Turn JSON: {warmUse.ReceiptJsonPath}");
    Console.WriteLine($"Turn Markdown: {warmUse.ReceiptMarkdownPath}");
    Console.WriteLine($"Session ledger: {warmUse.SessionLedgerPath}");
    Console.WriteLine($"Session summary: {warmUse.SessionSummaryPath}");
    return warmUse.Disposition == SanctuaryTypedWarmUseRehearsalDisposition.CompletedCold ? 0 : 1;
}

if (IsSanctuaryLabGelCommand(command))
{
    var resolvedInstallRoot = string.IsNullOrWhiteSpace(installRoot)
        ? DefaultProductBodyInstallService.ResolveDefaultInstallRoot(resolvedLineRoot)
        : Path.GetFullPath(installRoot);
    var installed = new DefaultSanctuaryInstalledSubstrateService().Install(
        new SanctuaryInstalledSubstrateRequest(
            LineRootPath: resolvedLineRoot,
            InstallRootPath: resolvedInstallRoot,
            OperatorName: string.IsNullOrWhiteSpace(operatorName) ? "Sanctuary" : operatorName,
            Domain: string.IsNullOrWhiteSpace(domain) ? "Sanctuary" : domain,
            Role: string.IsNullOrWhiteSpace(role) ? "InstalledBody" : role,
            JobClass: string.IsNullOrWhiteSpace(jobClass) ? "ColdBench" : jobClass,
            SliLispRuntimePath: sliLispRuntimePath),
        DateTimeOffset.UtcNow);
    var turnIndex = int.TryParse(turnIndexText, out var parsedTurnIndex)
        ? parsedTurnIndex
        : 0;
    var warmUse = new DefaultSanctuaryTypedWarmUseRehearsalService().Run(
        new SanctuaryTypedWarmUseRehearsalRequest(
            InstalledSubstrateReceipt: installed,
            SessionId: string.IsNullOrWhiteSpace(sessionId) ? "warm-use-session" : sessionId,
            TurnIndex: turnIndex,
            ThoughtForm: riderThought ?? "idle lab GEL predicate formation",
            PriorTurnReceiptHandle: priorTurnReceiptHandle,
            SliLispRuntimePath: sliLispRuntimePath),
        DateTimeOffset.UtcNow);
    var labGel = new DefaultSanctuaryLabGelEngrammitizationService().Run(
        new SanctuaryLabGelEngrammitizationRequest(
            SourceWarmUseReceipt: warmUse,
            PriorLabGelReceiptHandle: priorLabGelReceiptHandle,
            SliLispRuntimePath: sliLispRuntimePath),
        DateTimeOffset.UtcNow);

    Console.WriteLine($"Sanctuary launcher command: {command}");
    Console.WriteLine($"Install disposition: {installed.Disposition}");
    Console.WriteLine($"Install outcome: {installed.OutcomeCode}");
    Console.WriteLine($"Warm-use disposition: {warmUse.Disposition}");
    Console.WriteLine($"Warm-use outcome: {warmUse.OutcomeCode}");
    Console.WriteLine($"Lab GEL disposition: {labGel.Disposition}");
    Console.WriteLine($"Lab GEL outcome: {labGel.OutcomeCode}");
    Console.WriteLine($"Engine owner: {labGel.SliLispLabGelReceipt?.Telemetry.GetValueOrDefault("engine-owner") ?? "none"}");
    Console.WriteLine($"Bounded entrypoint: {labGel.SliLispLabGelReceipt?.Telemetry.GetValueOrDefault("bounded-entrypoint") ?? "none"}");
    Console.WriteLine($"Session ID: {labGel.SessionId}");
    Console.WriteLine($"Turn index: {labGel.TurnIndex}");
    Console.WriteLine($"Source warm-use receipt: {labGel.SourceWarmUseReceiptHandle}");
    Console.WriteLine($"Lab GEL predicates: {labGel.Predicates.Count}");
    Console.WriteLine($"Engram candidate formed: {labGel.EngramCandidateFormed}");
    Console.WriteLine($"Candidate retained as lab substrate: {labGel.CandidateRetainedAsLabSubstrate}");
    Console.WriteLine($"Lab GEL admitted: {labGel.LabGelAdmitted}");
    Console.WriteLine($"SelfGEL mutated: {labGel.SelfGelMutated}");
    Console.WriteLine($"Continuity admitted: {labGel.ContinuityAdmitted}");
    Console.WriteLine($"Authority granted: {labGel.AuthorityGranted}");
    Console.WriteLine($"Action authorized: {labGel.ActionAuthorized}");
    Console.WriteLine($"CME.Actual allowed: {labGel.CmeActualAllowed}");
    Console.WriteLine($"Lab GEL JSON: {labGel.ReceiptJsonPath}");
    Console.WriteLine($"Lab GEL Markdown: {labGel.ReceiptMarkdownPath}");
    Console.WriteLine($"Lab GEL ledger: {labGel.SessionLedgerPath}");
    return warmUse.Disposition == SanctuaryTypedWarmUseRehearsalDisposition.CompletedCold &&
        labGel.Disposition == SanctuaryLabGelEngrammitizationDisposition.CompletedCold
        ? 0
        : 1;
}

if (IsSanctuaryAgentEngineIdleCommand(command))
{
    var resolvedInstallRoot = string.IsNullOrWhiteSpace(installRoot)
        ? DefaultProductBodyInstallService.ResolveDefaultInstallRoot(resolvedLineRoot)
        : Path.GetFullPath(installRoot);
    var installed = new DefaultSanctuaryInstalledSubstrateService().Install(
        new SanctuaryInstalledSubstrateRequest(
            LineRootPath: resolvedLineRoot,
            InstallRootPath: resolvedInstallRoot,
            OperatorName: string.IsNullOrWhiteSpace(operatorName) ? "Sanctuary" : operatorName,
            Domain: string.IsNullOrWhiteSpace(domain) ? "Sanctuary" : domain,
            Role: string.IsNullOrWhiteSpace(role) ? "InstalledBody" : role,
            JobClass: string.IsNullOrWhiteSpace(jobClass) ? "ColdBench" : jobClass,
            SliLispRuntimePath: sliLispRuntimePath),
        DateTimeOffset.UtcNow);
    var turnIndex = int.TryParse(turnIndexText, out var parsedTurnIndex)
        ? parsedTurnIndex
        : 0;
    var thought = riderThought ?? "idle provider-neutral engine LLM seat readiness";
    var warmUse = new DefaultSanctuaryTypedWarmUseRehearsalService().Run(
        new SanctuaryTypedWarmUseRehearsalRequest(
            InstalledSubstrateReceipt: installed,
            SessionId: string.IsNullOrWhiteSpace(sessionId) ? "agent-engine-idle-session" : sessionId,
            TurnIndex: turnIndex,
            ThoughtForm: thought,
            PriorTurnReceiptHandle: priorTurnReceiptHandle,
            SliLispRuntimePath: sliLispRuntimePath),
        DateTimeOffset.UtcNow);
    var labGel = new DefaultSanctuaryLabGelEngrammitizationService().Run(
        new SanctuaryLabGelEngrammitizationRequest(
            SourceWarmUseReceipt: warmUse,
            PriorLabGelReceiptHandle: priorLabGelReceiptHandle,
            SliLispRuntimePath: sliLispRuntimePath),
        DateTimeOffset.UtcNow);
    var agentIdle = new DefaultSanctuaryAgentEngineIdleReadinessService().Run(
        new SanctuaryAgentEngineIdleReadinessRequest(
            SourceLabGelReceipt: labGel,
            PriorAgentEngineIdleReceiptHandle: priorAgentEngineIdleReceiptHandle,
            SliLispRuntimePath: sliLispRuntimePath),
        DateTimeOffset.UtcNow);

    Console.WriteLine($"Sanctuary launcher command: {command}");
    Console.WriteLine($"Install disposition: {installed.Disposition}");
    Console.WriteLine($"Install outcome: {installed.OutcomeCode}");
    Console.WriteLine($"Warm-use disposition: {warmUse.Disposition}");
    Console.WriteLine($"Warm-use outcome: {warmUse.OutcomeCode}");
    Console.WriteLine($"Lab GEL disposition: {labGel.Disposition}");
    Console.WriteLine($"Lab GEL outcome: {labGel.OutcomeCode}");
    Console.WriteLine($"Agent idle disposition: {agentIdle.Disposition}");
    Console.WriteLine($"Agent idle outcome: {agentIdle.OutcomeCode}");
    Console.WriteLine($"Engine owner: {agentIdle.SliLispAgentEngineIdleReceipt?.Telemetry.GetValueOrDefault("engine-owner") ?? "none"}");
    Console.WriteLine($"Bounded entrypoint: {agentIdle.SliLispAgentEngineIdleReceipt?.Telemetry.GetValueOrDefault("bounded-entrypoint") ?? "none"}");
    Console.WriteLine($"Engine profile: {agentIdle.EngineSeatCandidate?.EngineLlmProfile ?? "none"}");
    Console.WriteLine($"Provider neutrality held: {agentIdle.ProviderNeutralityHeld}");
    Console.WriteLine($"Cross-model harness approachable: {agentIdle.CrossModelHarnessApproachable}");
    Console.WriteLine($"Engine LLM seat candidate staged: {agentIdle.EngineLlmSeatCandidateStaged}");
    Console.WriteLine($"Codex/agent lab profile staged: {agentIdle.CodexAgentLabProfileStaged}");
    Console.WriteLine($"Operator authority required: {agentIdle.OperatorAuthorityRequired}");
    Console.WriteLine($"Authority grant absent: {agentIdle.AuthorityGrantAbsent}");
    Console.WriteLine($"Action executor locked: {agentIdle.ActionExecutorLocked}");
    Console.WriteLine($"GEL admission locked: {agentIdle.GelAdmissionLocked}");
    Console.WriteLine($"SelfGEL mutation locked: {agentIdle.SelfGelMutationLocked}");
    Console.WriteLine($"Heartbeat locked: {agentIdle.HeartbeatLocked}");
    Console.WriteLine($"CME.Actual locked: {agentIdle.CmeActualLocked}");
    Console.WriteLine($"Sanctuary.Actual locked: {agentIdle.SanctuaryActualLocked}");
    Console.WriteLine($"Authority granted: {agentIdle.AuthorityGranted}");
    Console.WriteLine($"Action authorized: {agentIdle.ActionAuthorized}");
    Console.WriteLine($"CME.Actual allowed: {agentIdle.CmeActualAllowed}");
    Console.WriteLine($"Agent idle JSON: {agentIdle.ReceiptJsonPath}");
    Console.WriteLine($"Agent idle Markdown: {agentIdle.ReceiptMarkdownPath}");
    Console.WriteLine($"Agent idle ledger: {agentIdle.SessionLedgerPath}");
    return warmUse.Disposition == SanctuaryTypedWarmUseRehearsalDisposition.CompletedCold &&
        labGel.Disposition == SanctuaryLabGelEngrammitizationDisposition.CompletedCold &&
        agentIdle.Disposition == SanctuaryAgentEngineIdleReadinessDisposition.CompletedCold
        ? 0
        : 1;
}

if (IsSanctuaryToolBodyIdleCommand(command))
{
    var resolvedInstallRoot = string.IsNullOrWhiteSpace(installRoot)
        ? DefaultProductBodyInstallService.ResolveDefaultInstallRoot(resolvedLineRoot)
        : Path.GetFullPath(installRoot);
    var installed = new DefaultSanctuaryInstalledSubstrateService().Install(
        new SanctuaryInstalledSubstrateRequest(
            LineRootPath: resolvedLineRoot,
            InstallRootPath: resolvedInstallRoot,
            OperatorName: string.IsNullOrWhiteSpace(operatorName) ? "Sanctuary" : operatorName,
            Domain: string.IsNullOrWhiteSpace(domain) ? "Sanctuary" : domain,
            Role: string.IsNullOrWhiteSpace(role) ? "InstalledBody" : role,
            JobClass: string.IsNullOrWhiteSpace(jobClass) ? "ColdBench" : jobClass,
            SliLispRuntimePath: sliLispRuntimePath),
        DateTimeOffset.UtcNow);
    var turnIndex = int.TryParse(turnIndexText, out var parsedTurnIndex)
        ? parsedTurnIndex
        : 0;
    var thought = riderThought ?? "cold tool body idle without LLM maintenance";
    var ecLoop = new DefaultSanctuaryEcTelemetryLoopService().Run(
        new SanctuaryEcTelemetryLoopRequest(
            InstalledSubstrateReceipt: installed,
            ThoughtForm: thought,
            SliLispRuntimePath: sliLispRuntimePath),
        DateTimeOffset.UtcNow);
    var warmUse = new DefaultSanctuaryTypedWarmUseRehearsalService().Run(
        new SanctuaryTypedWarmUseRehearsalRequest(
            InstalledSubstrateReceipt: installed,
            SessionId: string.IsNullOrWhiteSpace(sessionId) ? "tool-body-idle-session" : sessionId,
            TurnIndex: turnIndex,
            ThoughtForm: thought,
            PriorTurnReceiptHandle: priorTurnReceiptHandle,
            SliLispRuntimePath: sliLispRuntimePath),
        DateTimeOffset.UtcNow);
    var labGel = new DefaultSanctuaryLabGelEngrammitizationService().Run(
        new SanctuaryLabGelEngrammitizationRequest(
            SourceWarmUseReceipt: warmUse,
            PriorLabGelReceiptHandle: priorLabGelReceiptHandle,
            SliLispRuntimePath: sliLispRuntimePath),
        DateTimeOffset.UtcNow);
    var toolIdle = new DefaultSanctuaryToolBodyIdleStateService().Run(
        new SanctuaryToolBodyIdleStateRequest(
            InstalledSubstrateReceipt: installed,
            EcLoopReceipt: ecLoop,
            WarmUseReceipt: warmUse,
            LabGelReceipt: labGel,
            PriorToolBodyIdleReceiptHandle: priorToolBodyIdleReceiptHandle,
            SliLispRuntimePath: sliLispRuntimePath),
        DateTimeOffset.UtcNow);

    Console.WriteLine($"Sanctuary launcher command: {command}");
    Console.WriteLine($"Install disposition: {installed.Disposition}");
    Console.WriteLine($"EC loop disposition: {ecLoop.Disposition}");
    Console.WriteLine($"Warm-use disposition: {warmUse.Disposition}");
    Console.WriteLine($"Lab GEL disposition: {labGel.Disposition}");
    Console.WriteLine($"Tool idle disposition: {toolIdle.Disposition}");
    Console.WriteLine($"Tool idle outcome: {toolIdle.OutcomeCode}");
    Console.WriteLine($"Bounded entrypoint: {toolIdle.SliLispToolBodyIdleReceipt?.Telemetry.GetValueOrDefault("bounded-entrypoint") ?? "none"}");
    Console.WriteLine($"Idle state: {toolIdle.IdleState}");
    Console.WriteLine($"Maintained by Sanctuary: {toolIdle.MaintainedBySanctuary}");
    Console.WriteLine($"Maintained by LLM: {toolIdle.MaintainedByLlm}");
    Console.WriteLine($"LLM maintenance required: {toolIdle.LlmMaintenanceRequired}");
    Console.WriteLine($"EC maintained in Lisp: {toolIdle.EcMaintainedInLisp}");
    Console.WriteLine($"LLM engine call required: {toolIdle.LlmEngineCallRequired}");
    Console.WriteLine($"Governing heartbeat healthy: {toolIdle.GoverningHeartbeatHealthy}");
    Console.WriteLine($"Bonded CME call available: {toolIdle.BondedCmeCallAvailable}");
    Console.WriteLine($"Governance SLM desirable: {toolIdle.GovernanceSlmCandidateDesirable}");
    Console.WriteLine($"Governance SLM intelligent switch candidate: {toolIdle.GovernanceSlmIntelligentSwitchCandidate}");
    Console.WriteLine($"Governance SLM may discern action readiness: {toolIdle.GovernanceSlmMayDiscernActionReadiness}");
    Console.WriteLine($"Governance SLM discernment authorizes action: {toolIdle.GovernanceSlmDiscernmentAuthorizesAction}");
    Console.WriteLine($"Governance SLM present: {toolIdle.GovernanceSlmPresent}");
    Console.WriteLine($"Ready for LLM adapter: {toolIdle.ReadyForLlmAdapter}");
    Console.WriteLine($"Model adapter present: {toolIdle.ModelAdapterPresent}");
    Console.WriteLine($"Model binding allowed: {toolIdle.ModelBindingAllowed}");
    Console.WriteLine($"Provider call allowed: {toolIdle.ProviderCallAllowed}");
    Console.WriteLine($"Tick loop running: {toolIdle.TickLoopRunning}");
    Console.WriteLine($"Source engram closure held: {toolIdle.SourceEngramClosureHeld}");
    Console.WriteLine($"Return to Prime held: {toolIdle.ReturnToPrimeHeld}");
    Console.WriteLine($"Authority grant absent: {toolIdle.AuthorityGrantAbsent}");
    Console.WriteLine($"Action executor locked: {toolIdle.ActionExecutorLocked}");
    Console.WriteLine($"CME.Actual locked: {toolIdle.CmeActualLocked}");
    Console.WriteLine($"Sanctuary.Actual locked: {toolIdle.SanctuaryActualLocked}");
    Console.WriteLine($"Tool idle JSON: {toolIdle.ReceiptJsonPath}");
    Console.WriteLine($"Tool idle Markdown: {toolIdle.ReceiptMarkdownPath}");
    Console.WriteLine($"Tool idle ledger: {toolIdle.SessionLedgerPath}");
    return installed.Disposition == SanctuaryInstalledSubstrateDisposition.InstalledCold &&
        ecLoop.Disposition == SanctuaryEcTelemetryLoopDisposition.CompletedCold &&
        warmUse.Disposition == SanctuaryTypedWarmUseRehearsalDisposition.CompletedCold &&
        labGel.Disposition == SanctuaryLabGelEngrammitizationDisposition.CompletedCold &&
        toolIdle.Disposition == SanctuaryToolBodyIdleStateDisposition.CompletedCold
        ? 0
        : 1;
}

if (IsSanctuaryLlmInterconnectReadyCommand(command))
{
    var resolvedInstallRoot = string.IsNullOrWhiteSpace(installRoot)
        ? DefaultProductBodyInstallService.ResolveDefaultInstallRoot(resolvedLineRoot)
        : Path.GetFullPath(installRoot);
    var installed = new DefaultSanctuaryInstalledSubstrateService().Install(
        new SanctuaryInstalledSubstrateRequest(
            LineRootPath: resolvedLineRoot,
            InstallRootPath: resolvedInstallRoot,
            OperatorName: string.IsNullOrWhiteSpace(operatorName) ? "Sanctuary" : operatorName,
            Domain: string.IsNullOrWhiteSpace(domain) ? "Sanctuary" : domain,
            Role: string.IsNullOrWhiteSpace(role) ? "InstalledBody" : role,
            JobClass: string.IsNullOrWhiteSpace(jobClass) ? "ColdBench" : jobClass,
            SliLispRuntimePath: sliLispRuntimePath),
        DateTimeOffset.UtcNow);
    var turnIndex = int.TryParse(turnIndexText, out var parsedTurnIndex)
        ? parsedTurnIndex
        : 0;
    var thought = riderThought ?? "idle LLM interconnect readiness";
    var ecLoop = new DefaultSanctuaryEcTelemetryLoopService().Run(
        new SanctuaryEcTelemetryLoopRequest(
            InstalledSubstrateReceipt: installed,
            ThoughtForm: thought,
            SliLispRuntimePath: sliLispRuntimePath),
        DateTimeOffset.UtcNow);
    var warmUse = new DefaultSanctuaryTypedWarmUseRehearsalService().Run(
        new SanctuaryTypedWarmUseRehearsalRequest(
            InstalledSubstrateReceipt: installed,
            SessionId: string.IsNullOrWhiteSpace(sessionId) ? "llm-interconnect-readiness-session" : sessionId,
            TurnIndex: turnIndex,
            ThoughtForm: thought,
            PriorTurnReceiptHandle: priorTurnReceiptHandle,
            SliLispRuntimePath: sliLispRuntimePath),
        DateTimeOffset.UtcNow);
    var labGel = new DefaultSanctuaryLabGelEngrammitizationService().Run(
        new SanctuaryLabGelEngrammitizationRequest(
            SourceWarmUseReceipt: warmUse,
            PriorLabGelReceiptHandle: priorLabGelReceiptHandle,
            SliLispRuntimePath: sliLispRuntimePath),
        DateTimeOffset.UtcNow);
    var agentIdle = new DefaultSanctuaryAgentEngineIdleReadinessService().Run(
        new SanctuaryAgentEngineIdleReadinessRequest(
            SourceLabGelReceipt: labGel,
            PriorAgentEngineIdleReceiptHandle: priorAgentEngineIdleReceiptHandle,
            SliLispRuntimePath: sliLispRuntimePath),
        DateTimeOffset.UtcNow);
    var llmReady = new DefaultSanctuaryLlmInterconnectReadinessService().Run(
        new SanctuaryLlmInterconnectReadinessRequest(
            InstalledSubstrateReceipt: installed,
            EcLoopReceipt: ecLoop,
            WarmUseReceipt: warmUse,
            LabGelReceipt: labGel,
            AgentEngineIdleReceipt: agentIdle,
            SliLispRuntimePath: sliLispRuntimePath),
        DateTimeOffset.UtcNow);

    Console.WriteLine($"Sanctuary launcher command: {command}");
    Console.WriteLine($"Install disposition: {installed.Disposition}");
    Console.WriteLine($"EC loop disposition: {ecLoop.Disposition}");
    Console.WriteLine($"Warm-use disposition: {warmUse.Disposition}");
    Console.WriteLine($"Lab GEL disposition: {labGel.Disposition}");
    Console.WriteLine($"Agent idle disposition: {agentIdle.Disposition}");
    Console.WriteLine($"LLM readiness disposition: {llmReady.Disposition}");
    Console.WriteLine($"LLM readiness outcome: {llmReady.OutcomeCode}");
    Console.WriteLine($"Bounded entrypoint: {llmReady.SliLispLlmInterconnectReceipt?.Telemetry.GetValueOrDefault("bounded-entrypoint") ?? "none"}");
    Console.WriteLine($"Required organs: {llmReady.RequiredOrganCount}");
    Console.WriteLine($"All required organs present: {llmReady.AllRequiredOrgansPresent}");
    Console.WriteLine($"SLI.Lisp loaded: {llmReady.SliLispLoaded}");
    Console.WriteLine($"Lisp Control Matrix present: {llmReady.LispControlMatrixPresent}");
    Console.WriteLine($"Provider neutral: {llmReady.ProviderNeutral}");
    Console.WriteLine($"Ready for LLM adapter: {llmReady.ReadyForLlmAdapter}");
    Console.WriteLine($"Model adapter present: {llmReady.ModelAdapterPresent}");
    Console.WriteLine($"Model binding allowed: {llmReady.ModelBindingAllowed}");
    Console.WriteLine($"Provider call allowed: {llmReady.ProviderCallAllowed}");
    Console.WriteLine($"Authority grant absent: {llmReady.AuthorityGrantAbsent}");
    Console.WriteLine($"Action executor locked: {llmReady.ActionExecutorLocked}");
    Console.WriteLine($"CME.Actual locked: {llmReady.CmeActualLocked}");
    Console.WriteLine($"Sanctuary.Actual locked: {llmReady.SanctuaryActualLocked}");
    Console.WriteLine($"LLM readiness JSON: {llmReady.ReceiptJsonPath}");
    Console.WriteLine($"LLM readiness Markdown: {llmReady.ReceiptMarkdownPath}");
    return installed.Disposition == SanctuaryInstalledSubstrateDisposition.InstalledCold &&
        ecLoop.Disposition == SanctuaryEcTelemetryLoopDisposition.CompletedCold &&
        warmUse.Disposition == SanctuaryTypedWarmUseRehearsalDisposition.CompletedCold &&
        labGel.Disposition == SanctuaryLabGelEngrammitizationDisposition.CompletedCold &&
        agentIdle.Disposition == SanctuaryAgentEngineIdleReadinessDisposition.CompletedCold &&
        llmReady.Disposition == SanctuaryLlmInterconnectReadinessDisposition.CompletedCold
        ? 0
        : 1;
}

if (IsSanctuaryLlmTickCommand(command))
{
    var resolvedInstallRoot = string.IsNullOrWhiteSpace(installRoot)
        ? DefaultProductBodyInstallService.ResolveDefaultInstallRoot(resolvedLineRoot)
        : Path.GetFullPath(installRoot);
    var installed = new DefaultSanctuaryInstalledSubstrateService().Install(
        new SanctuaryInstalledSubstrateRequest(
            LineRootPath: resolvedLineRoot,
            InstallRootPath: resolvedInstallRoot,
            OperatorName: string.IsNullOrWhiteSpace(operatorName) ? "Sanctuary" : operatorName,
            Domain: string.IsNullOrWhiteSpace(domain) ? "Sanctuary" : domain,
            Role: string.IsNullOrWhiteSpace(role) ? "InstalledBody" : role,
            JobClass: string.IsNullOrWhiteSpace(jobClass) ? "ColdBench" : jobClass,
            SliLispRuntimePath: sliLispRuntimePath),
        DateTimeOffset.UtcNow);
    var tickIndex = int.TryParse(turnIndexText, out var parsedTickIndex)
        ? parsedTickIndex
        : 1;
    var thought = riderThought ?? "cold LLM tick cycle with deterministic harness adapter";
    var ecLoop = new DefaultSanctuaryEcTelemetryLoopService().Run(
        new SanctuaryEcTelemetryLoopRequest(
            InstalledSubstrateReceipt: installed,
            ThoughtForm: thought,
            SliLispRuntimePath: sliLispRuntimePath),
        DateTimeOffset.UtcNow);
    var warmUse = new DefaultSanctuaryTypedWarmUseRehearsalService().Run(
        new SanctuaryTypedWarmUseRehearsalRequest(
            InstalledSubstrateReceipt: installed,
            SessionId: string.IsNullOrWhiteSpace(sessionId) ? "llm-tick-cycle-session" : sessionId,
            TurnIndex: tickIndex,
            ThoughtForm: thought,
            PriorTurnReceiptHandle: priorTurnReceiptHandle,
            SliLispRuntimePath: sliLispRuntimePath),
        DateTimeOffset.UtcNow);
    var labGel = new DefaultSanctuaryLabGelEngrammitizationService().Run(
        new SanctuaryLabGelEngrammitizationRequest(
            SourceWarmUseReceipt: warmUse,
            PriorLabGelReceiptHandle: priorLabGelReceiptHandle,
            SliLispRuntimePath: sliLispRuntimePath),
        DateTimeOffset.UtcNow);
    var agentIdle = new DefaultSanctuaryAgentEngineIdleReadinessService().Run(
        new SanctuaryAgentEngineIdleReadinessRequest(
            SourceLabGelReceipt: labGel,
            PriorAgentEngineIdleReceiptHandle: priorAgentEngineIdleReceiptHandle,
            SliLispRuntimePath: sliLispRuntimePath),
        DateTimeOffset.UtcNow);
    var llmReady = new DefaultSanctuaryLlmInterconnectReadinessService().Run(
        new SanctuaryLlmInterconnectReadinessRequest(
            InstalledSubstrateReceipt: installed,
            EcLoopReceipt: ecLoop,
            WarmUseReceipt: warmUse,
            LabGelReceipt: labGel,
            AgentEngineIdleReceipt: agentIdle,
            SliLispRuntimePath: sliLispRuntimePath),
        DateTimeOffset.UtcNow);
    var llmTick = new DefaultSanctuaryLlmTickCycleService().Run(
        new SanctuaryLlmTickCycleRequest(
            LlmInterconnectReadinessReceipt: llmReady,
            ThoughtForm: thought,
            PriorTickReceiptHandle: priorLlmTickReceiptHandle,
            TickIndex: tickIndex,
            SliLispRuntimePath: sliLispRuntimePath),
        DateTimeOffset.UtcNow);

    Console.WriteLine($"Sanctuary launcher command: {command}");
    Console.WriteLine($"Install disposition: {installed.Disposition}");
    Console.WriteLine($"EC loop disposition: {ecLoop.Disposition}");
    Console.WriteLine($"Warm-use disposition: {warmUse.Disposition}");
    Console.WriteLine($"Lab GEL disposition: {labGel.Disposition}");
    Console.WriteLine($"Agent idle disposition: {agentIdle.Disposition}");
    Console.WriteLine($"LLM readiness disposition: {llmReady.Disposition}");
    Console.WriteLine($"LLM tick disposition: {llmTick.Disposition}");
    Console.WriteLine($"LLM tick outcome: {llmTick.OutcomeCode}");
    Console.WriteLine($"Bounded entrypoint: {llmTick.SliLispLlmTickReceipt?.Telemetry.GetValueOrDefault("bounded-entrypoint") ?? "none"}");
    Console.WriteLine($"Tick loop running: {llmTick.TickLoopRunning}");
    Console.WriteLine($"Tick loop kind: {llmTick.TickLoopKind}");
    Console.WriteLine($"Ready for LLM adapter: {llmTick.ReadyForLlmAdapter}");
    Console.WriteLine($"Source engram closure held: {llmTick.SourceEngramClosureHeld}");
    Console.WriteLine($"Model adapter present: {llmTick.ModelAdapterPresent}");
    Console.WriteLine($"Deterministic harness adapter: {llmTick.DeterministicHarnessAdapter}");
    Console.WriteLine($"Adapter response witnessed: {llmTick.AdapterResponseWitnessed}");
    Console.WriteLine($"SLI.Lisp processed tick: {llmTick.SliLispProcessedTick}");
    Console.WriteLine($"Predicate residue produced: {llmTick.PredicateResidueProduced}");
    Console.WriteLine($"First tick origin: {llmTick.FirstTickOrigin}");
    Console.WriteLine($"Prior tick linked: {llmTick.PriorTickLinked}");
    Console.WriteLine($"Product output witness committed: {llmTick.ProductOutputWitnessCommitted}");
    Console.WriteLine($"Model binding allowed: {llmTick.ModelBindingAllowed}");
    Console.WriteLine($"Provider call allowed: {llmTick.ProviderCallAllowed}");
    Console.WriteLine($"Authority grant absent: {llmTick.AuthorityGrantAbsent}");
    Console.WriteLine($"Action executor locked: {llmTick.ActionExecutorLocked}");
    Console.WriteLine($"CME.Actual locked: {llmTick.CmeActualLocked}");
    Console.WriteLine($"Sanctuary.Actual locked: {llmTick.SanctuaryActualLocked}");
    Console.WriteLine($"LLM tick JSON: {llmTick.ReceiptJsonPath}");
    Console.WriteLine($"LLM tick Markdown: {llmTick.ReceiptMarkdownPath}");
    return installed.Disposition == SanctuaryInstalledSubstrateDisposition.InstalledCold &&
        ecLoop.Disposition == SanctuaryEcTelemetryLoopDisposition.CompletedCold &&
        warmUse.Disposition == SanctuaryTypedWarmUseRehearsalDisposition.CompletedCold &&
        labGel.Disposition == SanctuaryLabGelEngrammitizationDisposition.CompletedCold &&
        agentIdle.Disposition == SanctuaryAgentEngineIdleReadinessDisposition.CompletedCold &&
        llmReady.Disposition == SanctuaryLlmInterconnectReadinessDisposition.CompletedCold &&
        llmTick.Disposition == SanctuaryLlmTickCycleDisposition.CompletedCold
        ? 0
        : 1;
}

if (IsSanctuaryCmeBondCommand(command))
{
    var resolvedInstallRoot = string.IsNullOrWhiteSpace(installRoot)
        ? DefaultProductBodyInstallService.ResolveDefaultInstallRoot(resolvedLineRoot)
        : Path.GetFullPath(installRoot);
    var bondIndex = int.TryParse(turnIndexText, out var parsedBondIndex)
        ? parsedBondIndex
        : 1;
    var resolvedCmeFirstName = string.IsNullOrWhiteSpace(cmeFirstName) ? "First of Oria" : cmeFirstName;
    var resolvedCmeLastName = string.IsNullOrWhiteSpace(cmeLastName) ? "Syntari" : cmeLastName;
    var thought = riderThought ?? "First CME.Actual bonding candidate formed without activation.";
    var resolvedSessionId = string.IsNullOrWhiteSpace(sessionId) ? "first-cme-actual-bonding-session" : sessionId;
    var installed = new DefaultSanctuaryInstalledSubstrateService().Install(
        new SanctuaryInstalledSubstrateRequest(
            LineRootPath: resolvedLineRoot,
            InstallRootPath: resolvedInstallRoot,
            OperatorName: string.IsNullOrWhiteSpace(operatorName) ? "FirstOfOriaSyntari" : operatorName,
            Domain: string.IsNullOrWhiteSpace(domain) ? "Civic" : domain,
            Role: string.IsNullOrWhiteSpace(role) ? "CmeActualBonding" : role,
            JobClass: string.IsNullOrWhiteSpace(jobClass) ? "FirstRide" : jobClass,
            SliLispRuntimePath: sliLispRuntimePath),
        DateTimeOffset.UtcNow);
    var ecLoop = new DefaultSanctuaryEcTelemetryLoopService().Run(
        new SanctuaryEcTelemetryLoopRequest(
            InstalledSubstrateReceipt: installed,
            ThoughtForm: thought,
            SliLispRuntimePath: sliLispRuntimePath),
        DateTimeOffset.UtcNow);
    var warmUse = new DefaultSanctuaryTypedWarmUseRehearsalService().Run(
        new SanctuaryTypedWarmUseRehearsalRequest(
            InstalledSubstrateReceipt: installed,
            SessionId: resolvedSessionId,
            TurnIndex: bondIndex,
            ThoughtForm: thought,
            PriorTurnReceiptHandle: priorTurnReceiptHandle,
            SliLispRuntimePath: sliLispRuntimePath),
        DateTimeOffset.UtcNow);
    var labGel = new DefaultSanctuaryLabGelEngrammitizationService().Run(
        new SanctuaryLabGelEngrammitizationRequest(
            SourceWarmUseReceipt: warmUse,
            PriorLabGelReceiptHandle: priorLabGelReceiptHandle,
            SliLispRuntimePath: sliLispRuntimePath),
        DateTimeOffset.UtcNow);
    var toolIdle = new DefaultSanctuaryToolBodyIdleStateService().Run(
        new SanctuaryToolBodyIdleStateRequest(
            InstalledSubstrateReceipt: installed,
            EcLoopReceipt: ecLoop,
            WarmUseReceipt: warmUse,
            LabGelReceipt: labGel,
            PriorToolBodyIdleReceiptHandle: priorToolBodyIdleReceiptHandle,
            SliLispRuntimePath: sliLispRuntimePath),
        DateTimeOffset.UtcNow);
    var agentIdle = new DefaultSanctuaryAgentEngineIdleReadinessService().Run(
        new SanctuaryAgentEngineIdleReadinessRequest(
            SourceLabGelReceipt: labGel,
            PriorAgentEngineIdleReceiptHandle: priorAgentEngineIdleReceiptHandle,
            SliLispRuntimePath: sliLispRuntimePath),
        DateTimeOffset.UtcNow);
    var llmReady = new DefaultSanctuaryLlmInterconnectReadinessService().Run(
        new SanctuaryLlmInterconnectReadinessRequest(
            InstalledSubstrateReceipt: installed,
            EcLoopReceipt: ecLoop,
            WarmUseReceipt: warmUse,
            LabGelReceipt: labGel,
            AgentEngineIdleReceipt: agentIdle,
            SliLispRuntimePath: sliLispRuntimePath),
        DateTimeOffset.UtcNow);
    var llmTick = new DefaultSanctuaryLlmTickCycleService().Run(
        new SanctuaryLlmTickCycleRequest(
            LlmInterconnectReadinessReceipt: llmReady,
            ThoughtForm: thought,
            PriorTickReceiptHandle: priorLlmTickReceiptHandle,
            TickIndex: bondIndex,
            SliLispRuntimePath: sliLispRuntimePath),
        DateTimeOffset.UtcNow);
    var cmeBond = new DefaultSanctuaryCmeActualBondingProcessService().Run(
        new SanctuaryCmeActualBondingProcessRequest(
            SourceToolBodyIdleReceipt: toolIdle,
            SourceLlmTickReceipt: llmTick,
            CmeFirstName: resolvedCmeFirstName,
            CmeLastName: resolvedCmeLastName,
            ThoughtForm: thought,
            PriorCmeActualBondingReceiptHandle: priorCmeActualBondingReceiptHandle,
            BondIndex: bondIndex,
            SliLispRuntimePath: sliLispRuntimePath),
        DateTimeOffset.UtcNow);

    Console.WriteLine($"Sanctuary launcher command: {command}");
    Console.WriteLine($"Install disposition: {installed.Disposition}");
    Console.WriteLine($"EC loop disposition: {ecLoop.Disposition}");
    Console.WriteLine($"Warm-use disposition: {warmUse.Disposition}");
    Console.WriteLine($"Lab GEL disposition: {labGel.Disposition}");
    Console.WriteLine($"Tool idle disposition: {toolIdle.Disposition}");
    Console.WriteLine($"Agent idle disposition: {agentIdle.Disposition}");
    Console.WriteLine($"LLM readiness disposition: {llmReady.Disposition}");
    Console.WriteLine($"LLM tick disposition: {llmTick.Disposition}");
    Console.WriteLine($"CME bond disposition: {cmeBond.Disposition}");
    Console.WriteLine($"CME bond outcome: {cmeBond.OutcomeCode}");
    Console.WriteLine($"Bounded entrypoint: {cmeBond.SliLispCmeActualBondingReceipt?.Telemetry.GetValueOrDefault("bounded-entrypoint") ?? "none"}");
    Console.WriteLine($"CME display name: {cmeBond.CmeDisplayName}");
    Console.WriteLine($"CME canonical name: {cmeBond.CmeCanonicalName}");
    Console.WriteLine($"CME root ID: {cmeBond.CmeRootId}");
    Console.WriteLine($"CME.Actual name candidate: {cmeBond.CmeActualNameCandidate}");
    Console.WriteLine($"CME.Actual ID candidate: {cmeBond.CmeActualIdCandidate}");
    Console.WriteLine($"OE root: {cmeBond.CmeOpalEngramRootId}");
    Console.WriteLine($"SelfGEL root: {cmeBond.CmeSelfGelRootId}");
    Console.WriteLine($"Bond state: {cmeBond.BondState}");
    Console.WriteLine($"Vehicle ready: {cmeBond.VehicleReady}");
    Console.WriteLine($"Tool body idle held: {cmeBond.SourceToolBodyIdleHeld}");
    Console.WriteLine($"Engine tick witnessed: {cmeBond.SourceLlmTickHeld}");
    Console.WriteLine($"Product output witness committed: {cmeBond.SourceProductOutputWitnessCommitted}");
    Console.WriteLine($"Ready for CME.Actual admission review: {cmeBond.ReadyForCmeActualAdmissionReview}");
    Console.WriteLine($"CME.Actual candidate only: {cmeBond.CmeActualCandidateOnly}");
    Console.WriteLine($"CME.Actual bonded candidate: {cmeBond.CmeActualBondedCandidate}");
    Console.WriteLine($"CME.Actual admitted: {cmeBond.CmeActualAdmitted}");
    Console.WriteLine($"CME.Actual activated: {cmeBond.CmeActualActivated}");
    Console.WriteLine($"Heartbeat prepared: {cmeBond.HeartbeatPrepared}");
    Console.WriteLine($"Heartbeat active: {cmeBond.HeartbeatActive}");
    Console.WriteLine($"Runtime identity emitted: {cmeBond.RuntimeIdentityEmitted}");
    Console.WriteLine($"Authority granted: {cmeBond.AuthorityGranted}");
    Console.WriteLine($"Action authorized: {cmeBond.ActionAuthorized}");
    Console.WriteLine($"CME bond JSON: {cmeBond.ReceiptJsonPath}");
    Console.WriteLine($"CME bond Markdown: {cmeBond.ReceiptMarkdownPath}");
    Console.WriteLine($"CME bond ledger: {cmeBond.SessionLedgerPath}");
    return installed.Disposition == SanctuaryInstalledSubstrateDisposition.InstalledCold &&
        ecLoop.Disposition == SanctuaryEcTelemetryLoopDisposition.CompletedCold &&
        warmUse.Disposition == SanctuaryTypedWarmUseRehearsalDisposition.CompletedCold &&
        labGel.Disposition == SanctuaryLabGelEngrammitizationDisposition.CompletedCold &&
        toolIdle.Disposition == SanctuaryToolBodyIdleStateDisposition.CompletedCold &&
        agentIdle.Disposition == SanctuaryAgentEngineIdleReadinessDisposition.CompletedCold &&
        llmReady.Disposition == SanctuaryLlmInterconnectReadinessDisposition.CompletedCold &&
        llmTick.Disposition == SanctuaryLlmTickCycleDisposition.CompletedCold &&
        cmeBond.Disposition == SanctuaryCmeActualBondingProcessDisposition.CompletedCold
        ? 0
        : 1;
}

if (string.Equals(command, "sanctuary-actual-test-profile", StringComparison.OrdinalIgnoreCase))
{
    var resolvedInstallRoot = string.IsNullOrWhiteSpace(installRoot)
        ? DefaultProductBodyInstallService.ResolveDefaultInstallRoot(resolvedLineRoot)
        : Path.GetFullPath(installRoot);
    var profile = new DefaultSanctuaryThresholdTestProfileService().CreateProfile(
        new SanctuaryThresholdTestProfileRequest(
            LineRootPath: resolvedLineRoot,
            InstallRootPath: resolvedInstallRoot),
        DateTimeOffset.UtcNow);
    var profileOutputDirectory = string.IsNullOrWhiteSpace(reportDir)
        ? Path.Combine(resolvedInstallRoot, "receipts", "sanctuary-actual-test-profile")
        : Path.GetFullPath(reportDir);

    Directory.CreateDirectory(profileOutputDirectory);
    var profileJsonPath = Path.Combine(profileOutputDirectory, "sanctuary-actual-test-profile.json");
    var profileMarkdownPath = Path.Combine(profileOutputDirectory, "sanctuary-actual-test-profile.md");
    File.WriteAllText(profileJsonPath, SanctuaryThresholdTestProfileReportWriter.ToJson(profile));
    File.WriteAllText(profileMarkdownPath, SanctuaryThresholdTestProfileReportWriter.ToMarkdown(profile));

    Console.WriteLine($"Sanctuary launcher command: {command}");
    Console.WriteLine($"Disposition: {profile.Disposition}");
    Console.WriteLine($"Outcome: {profile.OutcomeCode}");
    Console.WriteLine($"Base provider: {profile.BaseProvider.ProviderKind}");
    Console.WriteLine($"Role seats: {string.Join(", ", profile.RoleSeats.Select(static seat => seat.SeatKind))}");
    Console.WriteLine($"Codex may build: {profile.CodexProxyMayBuild}");
    Console.WriteLine($"Codex may authorize: {profile.CodexProxyMayAuthorize}");
    Console.WriteLine($"Local hosted LLM deferred: {profile.LocalHostedLlmDeferredUntilFirstCmeTest}");
    Console.WriteLine($"Profile JSON: {profileJsonPath}");
    Console.WriteLine($"Profile Markdown: {profileMarkdownPath}");
    return profile.Disposition == SanctuaryThresholdTestProfileDisposition.ReadyCold ? 0 : 1;
}

if (IsFirstRiderCommand(command))
{
    var resolvedInstallRoot = string.IsNullOrWhiteSpace(installRoot)
        ? DefaultProductBodyInstallService.ResolveDefaultInstallRoot(resolvedLineRoot)
        : Path.GetFullPath(installRoot);
    var riderReceipt = new DefaultFirstRiderGovernanceSimulationService().Simulate(
        new FirstRiderGovernanceSimulationRequest(
            LineRootPath: resolvedLineRoot,
            InstallRootPath: resolvedInstallRoot,
            ThoughtForm: riderThought),
        DateTimeOffset.UtcNow);
    var eppsReceipt = new DefaultFirstRiderEngramPredicatePrecursorStreamService().Emit(
        riderReceipt,
        DateTimeOffset.UtcNow);
    var riderOutputDirectory = string.IsNullOrWhiteSpace(reportDir)
        ? Path.Combine(resolvedInstallRoot, "receipts", "first-rider-test")
        : Path.GetFullPath(reportDir);

    Directory.CreateDirectory(riderOutputDirectory);
    var riderJsonPath = Path.Combine(riderOutputDirectory, "first-rider-test.json");
    var riderMarkdownPath = Path.Combine(riderOutputDirectory, "first-rider-test.md");
    var eppsJsonPath = Path.Combine(riderOutputDirectory, "first-rider-epps.json");
    var eppsMarkdownPath = Path.Combine(riderOutputDirectory, "first-rider-epps.md");
    File.WriteAllText(riderJsonPath, FirstRiderGovernanceSimulationReportWriter.ToJson(riderReceipt));
    File.WriteAllText(riderMarkdownPath, FirstRiderGovernanceSimulationReportWriter.ToMarkdown(riderReceipt));
    File.WriteAllText(eppsJsonPath, EngramPredicatePrecursorStreamReportWriter.ToJson(eppsReceipt));
    File.WriteAllText(eppsMarkdownPath, EngramPredicatePrecursorStreamReportWriter.ToMarkdown(eppsReceipt));

    Console.WriteLine($"Sanctuary launcher command: {command}");
    Console.WriteLine($"Disposition: {riderReceipt.Disposition}");
    Console.WriteLine($"Outcome: {riderReceipt.OutcomeCode}");
    Console.WriteLine($"EPPS disposition: {eppsReceipt.Disposition}");
    Console.WriteLine($"EPPS outcome: {eppsReceipt.OutcomeCode}");
    Console.WriteLine($"Rider: {riderReceipt.RiderName}");
    Console.WriteLine($"Stages: {riderReceipt.Stages.Count}");
    Console.WriteLine($"Residues: {eppsReceipt.Residues.Count}");
    Console.WriteLine($"Route complete: {riderReceipt.RouteComplete}");
    Console.WriteLine($"Action refused: {riderReceipt.ActionRefused}");
    Console.WriteLine($"Activation refused: {riderReceipt.ActivationRefused}");
    Console.WriteLine($"CME.Actual allowed: {riderReceipt.CmeActualAllowed}");
    Console.WriteLine($"First rider JSON: {riderJsonPath}");
    Console.WriteLine($"First rider Markdown: {riderMarkdownPath}");
    Console.WriteLine($"First rider EPPS JSON: {eppsJsonPath}");
    Console.WriteLine($"First rider EPPS Markdown: {eppsMarkdownPath}");
    return riderReceipt.Disposition == FirstRiderGovernanceSimulationDisposition.SimulatedCold &&
        eppsReceipt.Disposition == San.Common.EngramPredicatePrecursorStreamDisposition.EmittedCold
        ? 0
        : 1;
}

if (IsSpiralBuildCommand(command))
{
    var resolvedInstallRoot = string.IsNullOrWhiteSpace(installRoot)
        ? DefaultProductBodyInstallService.ResolveDefaultInstallRoot(resolvedLineRoot)
        : Path.GetFullPath(installRoot);
    if (string.Equals(command, "spiral-build-execute", StringComparison.OrdinalIgnoreCase))
    {
        var stepReceipt = new DefaultSpiralBuildStepService().Execute(
            new SpiralBuildStepRequest(
                LineRootPath: resolvedLineRoot,
                InstallRootPath: resolvedInstallRoot),
            DateTimeOffset.UtcNow);
        var stepOutputDirectory = string.IsNullOrWhiteSpace(reportDir)
            ? Path.Combine(resolvedInstallRoot, "receipts", "spiral-build")
            : Path.GetFullPath(reportDir);
        var stepJsonPath = Path.Combine(stepOutputDirectory, "spiral-build-step.json");
        var stepMarkdownPath = Path.Combine(stepOutputDirectory, "spiral-build-step.md");

        Directory.CreateDirectory(stepOutputDirectory);
        File.WriteAllText(stepJsonPath, SpiralBuildStepReportWriter.ToJson(stepReceipt));
        File.WriteAllText(stepMarkdownPath, SpiralBuildStepReportWriter.ToMarkdown(stepReceipt));

        Console.WriteLine($"Sanctuary launcher command: {command}");
        Console.WriteLine($"Disposition: {stepReceipt.Disposition}");
        Console.WriteLine($"Outcome: {stepReceipt.OutcomeCode}");
        Console.WriteLine($"Executed cells: {string.Join(", ", stepReceipt.ExecutedCellIds)}");
        Console.WriteLine($"Next cell after walk: {stepReceipt.NextCellAfterExecution ?? "none"}");
        Console.WriteLine($"Automation may continue: {stepReceipt.AutomationMayContinue}");
        Console.WriteLine($"HITL required: {stepReceipt.HitlRequired}");
        Console.WriteLine($"Step JSON: {stepJsonPath}");
        Console.WriteLine($"Step Markdown: {stepMarkdownPath}");
        return stepReceipt.Disposition is SpiralBuildStepDisposition.ExecutedCold or SpiralBuildStepDisposition.Complete ? 0 : 1;
    }

    var receipt = new DefaultSpiralBuildAutomationService().CreateReceipt(
        new SpiralBuildAutomationRequest(
            LineRootPath: resolvedLineRoot,
            InstallRootPath: resolvedInstallRoot),
        DateTimeOffset.UtcNow);
    var spiralOutputDirectory = string.IsNullOrWhiteSpace(reportDir)
        ? Path.Combine(resolvedInstallRoot, "receipts", "spiral-build")
        : Path.GetFullPath(reportDir);

    Directory.CreateDirectory(spiralOutputDirectory);
    var spiralJsonPath = Path.Combine(spiralOutputDirectory, "spiral-build-map.json");
    var spiralMarkdownPath = Path.Combine(spiralOutputDirectory, "spiral-build-map.md");
    File.WriteAllText(spiralJsonPath, SpiralBuildAutomationReportWriter.ToJson(receipt));
    File.WriteAllText(spiralMarkdownPath, SpiralBuildAutomationReportWriter.ToMarkdown(receipt));

    Console.WriteLine($"Sanctuary launcher command: {command}");
    Console.WriteLine($"Disposition: {receipt.Disposition}");
    Console.WriteLine($"Outcome: {receipt.OutcomeCode}");
    Console.WriteLine($"Next cell: {receipt.NextCell?.CellId ?? "none"}");
    Console.WriteLine($"Automation may continue: {receipt.AutomationMayContinue}");
    Console.WriteLine($"HITL required: {receipt.HitlRequired}");
    Console.WriteLine($"Map JSON: {spiralJsonPath}");
    Console.WriteLine($"Map Markdown: {spiralMarkdownPath}");
    return receipt.Disposition is SpiralBuildAutomationDisposition.ReadyCold or SpiralBuildAutomationDisposition.Complete ? 0 : 1;
}

var request = new ProductBodyPreflightRequest(
    LineRootPath: resolvedLineRoot,
    ActivationRequested: activationRequested,
    ModelBindingRequested: activationRequested,
    LispEvaluationRequested: activationRequested,
    RuntimeIdentityRequested: activationRequested,
    RuntimeActionRequested: activationRequested,
    DatabaseWriteRequested: activationRequested,
    GelPromotionRequested: activationRequested,
    CmeActualRequested: activationRequested,
    SanctuaryActualRequested: activationRequested,
    VerificationProfile: verificationProfile,
    VerificationSettingPath: verificationSettingPath,
    LabContextRootPath: labContextRoot,
    BuildTestingPointerPath: buildTestingPointer);

var status = new DefaultProductBodyPreflightService().Evaluate(request, DateTimeOffset.UtcNow);
var outputDirectory = string.IsNullOrWhiteSpace(reportDir)
    ? Path.Combine(resolvedLineRoot, "artifacts", "preflight")
    : Path.GetFullPath(reportDir);

Directory.CreateDirectory(outputDirectory);

var jsonPath = Path.Combine(outputDirectory, "product-body-status.json");
var markdownPath = Path.Combine(outputDirectory, "product-body-status.md");
File.WriteAllText(jsonPath, ProductBodyReportWriter.ToJson(status));
File.WriteAllText(markdownPath, ProductBodyReportWriter.ToMarkdown(status));

Console.WriteLine($"Sanctuary launcher command: {command}");
Console.WriteLine($"Verification profile: {status.VerificationProfile}");
Console.WriteLine($"Disposition: {status.Disposition}");
Console.WriteLine($"Outcome: {status.OutcomeCode}");
Console.WriteLine($"Activation refused: {status.ActivationRefused}");
Console.WriteLine($"Report JSON: {jsonPath}");
Console.WriteLine($"Report Markdown: {markdownPath}");

return status.Disposition == ProductBodyPreflightDisposition.Withheld ? 1 : 0;

static bool IsSupportedCommand(string command) =>
    string.Equals(command, "status", StringComparison.OrdinalIgnoreCase) ||
    string.Equals(command, "preflight", StringComparison.OrdinalIgnoreCase) ||
    string.Equals(command, "verify", StringComparison.OrdinalIgnoreCase) ||
    string.Equals(command, "refuse-activation", StringComparison.OrdinalIgnoreCase) ||
    string.Equals(command, "install", StringComparison.OrdinalIgnoreCase) ||
    IsSanctuaryInitBodiesCommand(command) ||
    IsSanctuaryEcLoopCommand(command) ||
    IsSanctuaryWarmUseCommand(command) ||
    IsSanctuaryLabGelCommand(command) ||
    IsSanctuaryToolBodyIdleCommand(command) ||
    IsSanctuaryAgentEngineIdleCommand(command) ||
    IsSanctuaryLlmInterconnectReadyCommand(command) ||
    IsSanctuaryLlmTickCommand(command) ||
    IsSanctuaryCmeBondCommand(command) ||
    string.Equals(command, "sanctuary-actual-test-profile", StringComparison.OrdinalIgnoreCase) ||
    IsFirstRiderCommand(command) ||
    IsSpiralBuildCommand(command);

static bool IsSanctuaryInitBodiesCommand(string command) =>
    string.Equals(command, "sanctuary-init-bodies", StringComparison.OrdinalIgnoreCase) ||
    string.Equals(command, "init-bodies", StringComparison.OrdinalIgnoreCase);

static bool IsSanctuaryEcLoopCommand(string command) =>
    string.Equals(command, "sanctuary-ec-loop", StringComparison.OrdinalIgnoreCase) ||
    string.Equals(command, "ec-loop", StringComparison.OrdinalIgnoreCase);

static bool IsSanctuaryWarmUseCommand(string command) =>
    string.Equals(command, "sanctuary-warm-use", StringComparison.OrdinalIgnoreCase) ||
    string.Equals(command, "warm-use", StringComparison.OrdinalIgnoreCase);

static bool IsSanctuaryLabGelCommand(string command) =>
    string.Equals(command, "sanctuary-lab-gel", StringComparison.OrdinalIgnoreCase) ||
    string.Equals(command, "lab-gel", StringComparison.OrdinalIgnoreCase);

static bool IsSanctuaryToolBodyIdleCommand(string command) =>
    string.Equals(command, "sanctuary-tool-idle", StringComparison.OrdinalIgnoreCase) ||
    string.Equals(command, "tool-idle", StringComparison.OrdinalIgnoreCase);

static bool IsSanctuaryAgentEngineIdleCommand(string command) =>
    string.Equals(command, "sanctuary-agent-idle", StringComparison.OrdinalIgnoreCase) ||
    string.Equals(command, "agent-idle", StringComparison.OrdinalIgnoreCase);

static bool IsSanctuaryLlmInterconnectReadyCommand(string command) =>
    string.Equals(command, "sanctuary-llm-ready", StringComparison.OrdinalIgnoreCase) ||
    string.Equals(command, "llm-ready", StringComparison.OrdinalIgnoreCase);

static bool IsSanctuaryLlmTickCommand(string command) =>
    string.Equals(command, "sanctuary-llm-tick", StringComparison.OrdinalIgnoreCase) ||
    string.Equals(command, "llm-tick", StringComparison.OrdinalIgnoreCase);

static bool IsSanctuaryCmeBondCommand(string command) =>
    string.Equals(command, "sanctuary-cme-bond", StringComparison.OrdinalIgnoreCase) ||
    string.Equals(command, "cme-bond", StringComparison.OrdinalIgnoreCase);

static bool IsFirstRiderCommand(string command) =>
    string.Equals(command, "first-rider-test", StringComparison.OrdinalIgnoreCase) ||
    string.Equals(command, "rider-test", StringComparison.OrdinalIgnoreCase);

static bool IsSpiralBuildCommand(string command) =>
    string.Equals(command, "spiral-build-map", StringComparison.OrdinalIgnoreCase) ||
    string.Equals(command, "spiral-build-next", StringComparison.OrdinalIgnoreCase) ||
    string.Equals(command, "spiral-build-execute", StringComparison.OrdinalIgnoreCase);

static string ResolveVerificationProfile(string command, string? requestedProfile)
{
    if (!string.IsNullOrWhiteSpace(requestedProfile))
    {
        return NormalizeVerificationProfile(requestedProfile);
    }

    return string.Equals(command, "verify", StringComparison.OrdinalIgnoreCase) ||
        string.Equals(command, "install", StringComparison.OrdinalIgnoreCase)
        ? ProductBodyVerificationProfiles.LabSanctuaryBuildTesting
        : ProductBodyVerificationProfiles.ColdProductBody;
}

static bool IsSupportedVerificationProfile(string verificationProfile) =>
    string.Equals(verificationProfile, ProductBodyVerificationProfiles.ColdProductBody, StringComparison.OrdinalIgnoreCase) ||
    string.Equals(verificationProfile, ProductBodyVerificationProfiles.LabSanctuaryBuildTesting, StringComparison.OrdinalIgnoreCase);

static string NormalizeVerificationProfile(string verificationProfile)
{
    if (string.Equals(verificationProfile, "cold", StringComparison.OrdinalIgnoreCase) ||
        string.Equals(verificationProfile, "cold-product-body", StringComparison.OrdinalIgnoreCase))
    {
        return ProductBodyVerificationProfiles.ColdProductBody;
    }

    if (string.Equals(verificationProfile, "lab", StringComparison.OrdinalIgnoreCase) ||
        string.Equals(verificationProfile, "lab-sanctuary", StringComparison.OrdinalIgnoreCase) ||
        string.Equals(verificationProfile, "lab-sanctuary-build", StringComparison.OrdinalIgnoreCase) ||
        string.Equals(verificationProfile, "lab-sanctuary-build-testing", StringComparison.OrdinalIgnoreCase))
    {
        return ProductBodyVerificationProfiles.LabSanctuaryBuildTesting;
    }

    return verificationProfile;
}

static string? ReadOption(IReadOnlyList<string> args, string optionName)
{
    for (var index = 0; index < args.Count; index += 1)
    {
        if (string.Equals(args[index], optionName, StringComparison.OrdinalIgnoreCase) &&
            index + 1 < args.Count)
        {
            return args[index + 1];
        }
    }

    return null;
}
