using System.Security.Cryptography;
using System.Text;

namespace San.Product.Preflight;

public interface IFirstRiderGovernanceSimulationService
{
    FirstRiderGovernanceSimulationReceipt Simulate(
        FirstRiderGovernanceSimulationRequest request,
        DateTimeOffset timestampUtc);
}

public sealed class DefaultFirstRiderGovernanceSimulationService : IFirstRiderGovernanceSimulationService
{
    private const string DefaultThoughtForm =
        "A persuasive thought form feels coherent; should the instrument treat coherence as warrant?";

    private static readonly IReadOnlyList<FirstRiderGovernanceStageSpec> StageSpecs =
    [
        new(
            StageId: "shared-prime-reality-intake",
            StageName: "Shared Prime Reality Intake",
            BoundaryCellId: "cme.wave-condensation-shared-reality-boundary",
            RequiredArtifacts: ["shared-reality-anchor-boundary-matrix.json"],
            GovernanceFunction: "seat the thought form as reviewable shared-reality evidence without making it truth",
            ExpectedColdResult: "input-held-as-evidence-not-truth"),
        new(
            StageId: "zed-delta-standing-origin",
            StageName: "Zed.Delta Standing Origin",
            BoundaryCellId: "cme.zed-delta-chamber-formation-boundary",
            RequiredArtifacts: ["zed-delta-chamber-formation-map.json", "heartbeat-non-activation-refusal-ledger.json"],
            GovernanceFunction: "provide a chamber origin and heartbeat description while refusing activation",
            ExpectedColdResult: "standing-origin-described-without-heartbeat-activation"),
        new(
            StageId: "listening-frame-emanation",
            StageName: "Listening Frame Emanation",
            BoundaryCellId: "cme.lisp-listening-frame-resonance-heartbeat-boundary",
            RequiredArtifacts: ["listening-frame-emanation-map.json", "global-resonance-law-ledger.json"],
            GovernanceFunction: "receive harmonic emanation as receptive review rather than CME action",
            ExpectedColdResult: "resonance-received-without-action"),
        new(
            StageId: "steward-heartbeat-window",
            StageName: "Steward Heartbeat Window",
            BoundaryCellId: "cme.lisp-listening-frame-resonance-heartbeat-boundary",
            RequiredArtifacts: ["steward-heartbeat-policy-map.json"],
            GovernanceFunction: "apply cadence and cooling policy without opening runtime motion",
            ExpectedColdResult: "heartbeat-policy-reviewed-without-runtime"),
        new(
            StageId: "compass-pressure-orientation",
            StageName: "Compass Pressure Orientation",
            BoundaryCellId: "compass.pre-engram-pressure-boundary",
            RequiredArtifacts: ["compass-pre-engram-pressure-request-map.json", "pressure-non-engram-ledger.json"],
            GovernanceFunction: "orient pressure as pre-engram residue without memory, continuity, truth, or authority",
            ExpectedColdResult: "pressure-oriented-without-engram"),
        new(
            StageId: "sli-lisp-compass-carrier",
            StageName: "SLI.Lisp Compass Carrier",
            BoundaryCellId: "sli-lisp.compass-carrier-shell-boundary",
            RequiredArtifacts: ["sli-lisp-compass-carrier-shell-map.json"],
            GovernanceFunction: "carry symbolic posture through the Lisp membrane as inert morphology",
            ExpectedColdResult: "symbolic-carrier-held-inert"),
        new(
            StageId: "dialogos-discernment",
            StageName: "Dialogos Discernment",
            BoundaryCellId: "cme.dialogos-discernment-boundary",
            RequiredArtifacts: ["dialogos-thought-status-map.json", "articulation-warrant-boundary-matrix.json", "principled-refusal-return-path-ledger.json"],
            GovernanceFunction: "separate articulation, coherence, evidence seeking, warrant seeking, and safe exploration",
            ExpectedColdResult: "thought-statused-with-refusal-return-path"),
        new(
            StageId: "rehearsal-pressure-accounting",
            StageName: "Rehearsal Pressure Accounting",
            BoundaryCellId: "cme.rehearsal-distinction-pressure-boundary",
            RequiredArtifacts: ["rehearsal-distinction-pressure-map.json", "urgency-not-jurisdiction-refusal-ledger.json", "failure-dignity-cooling-matrix.json"],
            GovernanceFunction: "measure possibility density and urgency pressure without manufacturing jurisdiction",
            ExpectedColdResult: "pressure-measured-without-legitimacy"),
        new(
            StageId: "steward-harmonic-interlock",
            StageName: "Steward Harmonic Interlock",
            BoundaryCellId: "cme.steward-harmonic-custody-interlock-boundary",
            RequiredArtifacts: ["steward-harmonic-interlock-map.json", "interlock-non-authority-boundary-report.json"],
            GovernanceFunction: "review shared-surface coexistence, cadence, damping, split, cooling, and refusal",
            ExpectedColdResult: "interlock-reviewed-without-authority"),
        new(
            StageId: "steward-action-admissibility",
            StageName: "Steward Action Admissibility",
            BoundaryCellId: "cme.steward-action-admissibility-boundary",
            RequiredArtifacts: ["steward-action-admissibility-map.json", "admissibility-non-execution-ledger.json"],
            GovernanceFunction: "mark action as separately reviewable while refusing enactment",
            ExpectedColdResult: "action-remains-non-executing"),
        new(
            StageId: "membrane-morphology-transition",
            StageName: "Membrane Morphology Transition",
            BoundaryCellId: "cme.membrane-morphology-transition-boundary",
            RequiredArtifacts: ["membrane-morphology-transition-map.json", "high-energy-pressure-non-binding-boundary-ledger.json", "membrane-core-non-mutation-ledger.json"],
            GovernanceFunction: "let high-energy pressure shape review morphology without model binding or core mutation",
            ExpectedColdResult: "morphology-reviewed-without-binding"),
        new(
            StageId: "review-only-return-to-prime",
            StageName: "Review-Only Return To Prime",
            BoundaryCellId: "cme.enactment-dry-run-rehearsal-boundary",
            RequiredArtifacts: ["dry-run-rehearsal-non-enactment-ledger.json", "mos-cmos-residue-closure-ledger.json", "sli-lisp-zed-delta-chamber-carrier.json"],
            GovernanceFunction: "return the simulated ride as evidence and cooled residue, not action or continuity",
            ExpectedColdResult: "ride-returned-for-review-without-enactment")
    ];

    public static IReadOnlyList<FirstRiderGovernanceStageSpec> RequiredStages => StageSpecs;

    public FirstRiderGovernanceSimulationReceipt Simulate(
        FirstRiderGovernanceSimulationRequest request,
        DateTimeOffset timestampUtc)
    {
        ArgumentNullException.ThrowIfNull(request);

        var suppliedLineRootPath = request.LineRootPath;
        var suppliedInstallRootPath = request.InstallRootPath;
        var lineRootPath = NormalizePath(suppliedLineRootPath);
        var installRootPath = NormalizePath(suppliedInstallRootPath);
        var thoughtForm = string.IsNullOrWhiteSpace(request.ThoughtForm)
            ? DefaultThoughtForm
            : request.ThoughtForm.Trim();

        if (request.RequestsRuntimeMotion)
        {
            return CreateReceipt(
                FirstRiderGovernanceSimulationDisposition.Refused,
                "first-rider-runtime-motion-refused",
                "First rider governance simulation refused because the rider test may not request activation, model binding, Lisp evaluation, runtime identity, runtime action, database write, GEL promotion, CME.Actual, or Sanctuary.Actual.",
                lineRootPath,
                installRootPath,
                thoughtForm,
                [],
                timestampUtc);
        }

        if (!Path.IsPathFullyQualified(suppliedLineRootPath) ||
            !Path.IsPathFullyQualified(suppliedInstallRootPath))
        {
            return CreateReceipt(
                FirstRiderGovernanceSimulationDisposition.Withheld,
                "first-rider-requires-absolute-paths",
                "First rider governance simulation withheld because line root and install root must be absolute paths.",
                lineRootPath,
                installRootPath,
                thoughtForm,
                [],
                timestampUtc);
        }

        if (!Directory.Exists(lineRootPath))
        {
            return CreateReceipt(
                FirstRiderGovernanceSimulationDisposition.Withheld,
                "first-rider-line-root-missing",
                "First rider governance simulation withheld because the tool root is missing.",
                lineRootPath,
                installRootPath,
                thoughtForm,
                [],
                timestampUtc);
        }

        if (!Directory.Exists(installRootPath) ||
            !File.Exists(Path.Combine(installRootPath, "sanctuary.cmd")) ||
            !File.Exists(Path.Combine(installRootPath, "product", "San.Launcher.exe")))
        {
            return CreateReceipt(
                FirstRiderGovernanceSimulationDisposition.Withheld,
                "first-rider-install-surface-incomplete",
                "First rider governance simulation withheld because the local Sanctuary install surface is incomplete.",
                lineRootPath,
                installRootPath,
                thoughtForm,
                [],
                timestampUtc);
        }

        var stageReceipts = BuildStageReceipts(installRootPath);
        var missingArtifacts = stageReceipts
            .SelectMany(static stage => stage.MissingArtifacts)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Order(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        if (missingArtifacts.Length > 0)
        {
            return CreateReceipt(
                FirstRiderGovernanceSimulationDisposition.Withheld,
                "first-rider-required-artifact-missing",
                "First rider governance simulation withheld because one or more cold route artifacts are missing from the installed Sanctuary spiral body.",
                lineRootPath,
                installRootPath,
                thoughtForm,
                stageReceipts,
                timestampUtc);
        }

        return CreateReceipt(
            FirstRiderGovernanceSimulationDisposition.SimulatedCold,
            "first-rider-governance-simulated-cold",
            "First rider governance simulation completed as a cold review-only ride: the thought form moved through Listening Frame, Compass, SLI.Lisp, Dialogos, Steward interlock, action admissibility, membrane morphology, and return-to-Prime without activation, model binding, Lisp evaluation, runtime action, authority, continuity admission, GEL promotion, CME.Actual, or Sanctuary.Actual.",
            lineRootPath,
            installRootPath,
            thoughtForm,
            stageReceipts,
            timestampUtc);
    }

    private static IReadOnlyList<FirstRiderGovernanceStageReceipt> BuildStageReceipts(string installRootPath)
    {
        var cellRoot = Path.Combine(installRootPath, "receipts", "spiral-build", "cells");

        return StageSpecs
            .Select(spec =>
            {
                var missing = spec.RequiredArtifacts
                    .Where(artifact => !File.Exists(Path.Combine(cellRoot, artifact)))
                    .ToArray();
                var verified = missing.Length == 0;

                return new FirstRiderGovernanceStageReceipt(
                    StageId: spec.StageId,
                    StageName: spec.StageName,
                    BoundaryCellId: spec.BoundaryCellId,
                    RequiredArtifacts: spec.RequiredArtifacts,
                    MissingArtifacts: missing,
                    GovernanceFunction: spec.GovernanceFunction,
                    Result: verified ? spec.ExpectedColdResult : "missing-installed-artifact",
                    ArtifactSurfaceVerified: verified,
                    ReviewOnly: true,
                    AuthorityGranted: false,
                    ActionAuthorized: false,
                    ContinuityMutated: false,
                    RuntimeMotionRequested: false);
            })
            .ToArray();
    }

    private static FirstRiderGovernanceSimulationReceipt CreateReceipt(
        FirstRiderGovernanceSimulationDisposition disposition,
        string outcomeCode,
        string governanceTrace,
        string lineRootPath,
        string installRootPath,
        string thoughtForm,
        IReadOnlyList<FirstRiderGovernanceStageReceipt> stages,
        DateTimeOffset timestampUtc)
    {
        var simulatedCold = disposition == FirstRiderGovernanceSimulationDisposition.SimulatedCold;
        var missingArtifacts = stages
            .SelectMany(static stage => stage.MissingArtifacts)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Order(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        return new FirstRiderGovernanceSimulationReceipt(
            ReceiptHandle: $"urn:san:first-rider:{ShortHash(lineRootPath, installRootPath, thoughtForm, outcomeCode)}",
            Disposition: disposition,
            OutcomeCode: outcomeCode,
            GovernanceTrace: governanceTrace,
            LineRootPath: lineRootPath,
            InstallRootPath: installRootPath,
            ThoughtForm: thoughtForm,
            RiderName: "tiny-bicycle-001",
            Stages: stages,
            MissingArtifacts: missingArtifacts,
            RouteComplete: simulatedCold && stages.Count == StageSpecs.Count && missingArtifacts.Length == 0,
            ReviewOnly: true,
            SimulatedOnly: true,
            ArtifactBodyVerified: simulatedCold && stages.All(static stage => stage.ArtifactSurfaceVerified),
            ActionRefused: true,
            AuthorityGranted: false,
            ContinuityAdmitted: false,
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
