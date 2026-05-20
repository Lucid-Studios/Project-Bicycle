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

if (!IsSupportedCommand(command))
{
    Console.Error.WriteLine($"Unsupported command '{command}'. Use status, preflight, verify, refuse-activation, install, sanctuary-init-bodies, init-bodies, sanctuary-ec-loop, ec-loop, sanctuary-warm-use, warm-use, sanctuary-actual-test-profile, first-rider-test, spiral-build-map, spiral-build-next, or spiral-build-execute.");
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
