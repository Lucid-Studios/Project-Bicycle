using System.Security.Cryptography;
using System.Text;

namespace San.Product.Preflight;

public interface ISpiralBuildAutomationService
{
    SpiralBuildAutomationReceipt CreateReceipt(
        SpiralBuildAutomationRequest request,
        DateTimeOffset timestampUtc);
}

public sealed class DefaultSpiralBuildAutomationService : ISpiralBuildAutomationService
{
    private static readonly string[] StopConditions =
    [
        ".Actual authorization requested",
        "model binding requested",
        "database write requested",
        "GEL/cGEL/SelfGEL/cSelfGEL promotion requested",
        "key generation requested",
        "public artifact requested",
        "irreversible move or delete requested",
        "test failure reveals theory ambiguity",
        "cell wants to grow without adjacency",
        "telemetry attempts authority",
        "Prime/Cryptic/Steward proxy agents are needed"
    ];

    public SpiralBuildAutomationReceipt CreateReceipt(
        SpiralBuildAutomationRequest request,
        DateTimeOffset timestampUtc)
    {
        ArgumentNullException.ThrowIfNull(request);

        var suppliedLineRootPath = request.LineRootPath;
        var suppliedInstallRootPath = request.InstallRootPath;
        var lineRootPath = NormalizePath(suppliedLineRootPath);
        var installRootPath = NormalizePath(suppliedInstallRootPath);

        if (request.RequestsRuntimeMotion)
        {
            return CreateReceipt(
                SpiralBuildAutomationDisposition.Refused,
                "spiral-build-runtime-motion-refused",
                "Spiral build automation refused because automation cannot request activation, model binding, runtime identity, runtime action, database write, GEL promotion, CME.Actual, or Sanctuary.Actual.",
                lineRootPath,
                installRootPath,
                cells: [],
                nextCell: null,
                automationMayContinue: false,
                hitlRequired: true,
                timestampUtc);
        }

        if (!Path.IsPathFullyQualified(suppliedLineRootPath) ||
            !Path.IsPathFullyQualified(suppliedInstallRootPath))
        {
            return CreateReceipt(
                SpiralBuildAutomationDisposition.Withheld,
                "spiral-build-requires-absolute-paths",
                "Spiral build automation withheld because line root and install root must be absolute paths.",
                lineRootPath,
                installRootPath,
                cells: [],
                nextCell: null,
                automationMayContinue: false,
                hitlRequired: true,
                timestampUtc);
        }

        if (!Directory.Exists(lineRootPath))
        {
            return CreateReceipt(
                SpiralBuildAutomationDisposition.Withheld,
                "spiral-build-line-root-missing",
                "Spiral build automation withheld because the source line root is missing.",
                lineRootPath,
                installRootPath,
                cells: [],
                nextCell: null,
                automationMayContinue: false,
                hitlRequired: true,
                timestampUtc);
        }

        if (!Directory.Exists(installRootPath) ||
            !File.Exists(Path.Combine(installRootPath, "sanctuary.cmd")))
        {
            return CreateReceipt(
                SpiralBuildAutomationDisposition.Withheld,
                "spiral-build-install-surface-missing",
                "Spiral build automation withheld because the local Sanctuary install surface is missing.",
                lineRootPath,
                installRootPath,
                cells: [],
                nextCell: null,
                automationMayContinue: false,
                hitlRequired: true,
                timestampUtc);
        }

        var cells = BuildCells(installRootPath);
        var nextCell = SelectNextAdjacentCell(cells);
        if (nextCell is null)
        {
            return CreateReceipt(
                SpiralBuildAutomationDisposition.Complete,
                "spiral-build-no-adjacent-cell-remaining",
                "Spiral build automation found no adjacent candidate or planned cell remaining.",
                lineRootPath,
                installRootPath,
                cells,
                nextCell: null,
                automationMayContinue: false,
                hitlRequired: false,
                timestampUtc);
        }

        return CreateReceipt(
            SpiralBuildAutomationDisposition.ReadyCold,
            "spiral-build-next-adjacent-cell-selected",
            "Spiral build automation selected the next adjacent cold cell. Automation may continue until a HITL stop condition, failed verification, or completed work boundary is reached.",
            lineRootPath,
            installRootPath,
            cells,
            nextCell,
            automationMayContinue: true,
            hitlRequired: nextCell.HitlRequired,
            timestampUtc);
    }

    private static IReadOnlyList<SpiralBuildCellRecord> BuildCells(string installRootPath)
    {
        var installVerified =
            File.Exists(Path.Combine(installRootPath, "sanctuary.cmd")) &&
            File.Exists(Path.Combine(installRootPath, "product", "San.Launcher.exe"));
        var preflightVerified = File.Exists(Path.Combine(installRootPath, "receipts", "preflight", "product-body-status.json"));
        var triptychVerified = File.Exists(Path.Combine(installRootPath, "receipts", "sanctuary-actual-test-profile", "sanctuary-actual-test-profile.json"));

        var installStatus = installVerified ? SpiralBuildCellStatus.VerifiedCold : SpiralBuildCellStatus.Blocked;
        var preflightStatus = preflightVerified ? SpiralBuildCellStatus.VerifiedCold : SpiralBuildCellStatus.Candidate;
        var triptychStatus = triptychVerified ? SpiralBuildCellStatus.VerifiedCold : SpiralBuildCellStatus.Candidate;
        var spiralRoot = Path.Combine(installRootPath, "receipts", "spiral-build");
        var cellRoot = Path.Combine(spiralRoot, "cells");
        var layerMapVerified = HasArtifacts(
            cellRoot,
            "body-layer-map.json",
            "cell-taxonomy-outline.json",
            "non-collapse-law-ledger.json");
        var cellTaxonomyVerified = HasArtifacts(
            cellRoot,
            "cell-taxonomy.json",
            "adjacency-ledger.json",
            "cell-receipt-template.json");
        var primeStewardVerified = HasArtifacts(
            cellRoot,
            "prime-steward-membrane-map.json",
            "prime-steward-allowed-passage.json",
            "prime-steward-refusal-case.json");
        var crypticStewardVerified = HasArtifacts(
            cellRoot,
            "cryptic-steward-membrane-map.json",
            "cryptic-steward-telemetry-route.json",
            "cryptic-steward-self-witness-refusal.json");
        var compassShellVerified = HasArtifacts(
            cellRoot,
            "compass-shell-packet.json",
            "cleaving-decision-receipt.json",
            "compass-candidate-only-refusal.json");
        var telemetryVerified = HasArtifacts(
            cellRoot,
            "telemetry-route-map.json",
            "receipt-continuity-test-plan.json",
            "telemetry-authority-refusal.json");
        var packetMembraneVerified = HasArtifacts(
            cellRoot,
            "sanctuary-packet-contract-map.json",
            "packet-membrane-validation-matrix.json",
            "packet-non-authority-refusal-ledger.json");
        var packetReceiptRoutingVerified = HasArtifacts(
            cellRoot,
            "packet-passage-receipt-map.json",
            "packet-refusal-routing-matrix.json",
            "receipt-non-permission-ledger.json");
        var packetReceiptReplayVerified = HasArtifacts(
            cellRoot,
            "receipt-replay-request-map.json",
            "receipt-replay-boundary-matrix.json",
            "replay-non-reentry-ledger.json");
        var packetReceiptQueryVerified = HasArtifacts(
            cellRoot,
            "receipt-query-request-map.json",
            "receipt-query-boundary-matrix.json",
            "query-non-warrant-ledger.json");
        var packetReceiptSelectionVerified = HasArtifacts(
            cellRoot,
            "receipt-selection-request-map.json",
            "receipt-selection-boundary-matrix.json",
            "selection-non-admission-ledger.json");
        var witnessSummaryVerified = HasArtifacts(
            cellRoot,
            "witness-summary-request-map.json",
            "witness-summary-boundary-matrix.json",
            "summary-non-replacement-ledger.json");
        var compassPressureVerified = HasArtifacts(
            cellRoot,
            "compass-pre-engram-pressure-request-map.json",
            "compass-pressure-boundary-matrix.json",
            "pressure-non-engram-ledger.json");
        var compassShellStabilizationVerified = HasArtifacts(
            cellRoot,
            "compass-shell-stabilization-map.json",
            "shell-pressure-lineage-map.json",
            "shell-non-engram-boundary-ledger.json");
        var cleavingDiscernmentVerified = HasArtifacts(
            cellRoot,
            "cleaving-discernment-request-map.json",
            "cleaving-refusal-boundary-matrix.json",
            "cleaving-non-admission-ledger.json");
        var iterativeEvaluationVerified = HasArtifacts(
            cellRoot,
            "iterative-evaluation-loop-map.json",
            "evaluation-non-authority-ledger.json",
            "evaluation-tuning-candidate-map.json");
        var recursiveContemplationVerified = HasArtifacts(
            cellRoot,
            "recursive-contemplation-loop-map.json",
            "contemplation-non-continuity-ledger.json",
            "contemplation-cooling-path-map.json");
        var stewardHandoffReadinessVerified = HasArtifacts(
            cellRoot,
            "steward-handoff-readiness-map.json",
            "handoff-non-authorization-ledger.json",
            "ten-pass-body-tuning-next-lane-map.json");
        var typedDuplexIterationVerified = HasArtifacts(
            cellRoot,
            "typed-duplex-build-iteration-map.json",
            "iteration-flow-form-learning-map.json",
            "theory-direct-representation-optimization-map.json");
        var tenByTenScheduleVerified = HasArtifacts(
            cellRoot,
            "ten-by-ten-body-section-pass-matrix.json",
            "three-section-cascade-group-map.json",
            "lamp-body-seed-exclusion-optimization-ledger.json");
        var tenByTenGroupAVerified = HasArtifacts(
            cellRoot,
            "group-a-body-optimization-run-ledger.json",
            "group-a-flow-form-findings-map.json",
            "group-a-next-group-eligibility-receipt.json");
        var tenByTenGroupBVerified = HasArtifacts(
            cellRoot,
            "group-b-body-optimization-run-ledger.json",
            "group-b-flow-form-findings-map.json",
            "group-b-next-group-eligibility-receipt.json");
        var tenByTenGroupCVerified = HasArtifacts(
            cellRoot,
            "group-c-body-optimization-run-ledger.json",
            "group-c-flow-form-findings-map.json",
            "group-c-next-group-eligibility-receipt.json");
        var tenByTenGroupDVerified = HasArtifacts(
            cellRoot,
            "group-d-body-optimization-run-ledger.json",
            "group-d-flow-form-findings-map.json",
            "group-d-whole-body-synthesis-eligibility-receipt.json");
        var wholeBodySynthesisVerified = HasArtifacts(
            cellRoot,
            "whole-body-synthesis-comparison-ledger.json",
            "whole-body-doctrine-guardrail-coverage-map.json",
            "whole-body-unresolved-membrane-gap-and-next-lane-receipt.json");
        var ninefoldColdReviewVerified = HasArtifacts(
            cellRoot,
            "ninefold-worker-telemetry-contract.json",
            "ninefold-domain-run-assignment-map.json",
            "ninefold-braid-custody-non-promotion-ledger.json");
        var engramCandidatePreconditionVerified = HasArtifacts(
            cellRoot,
            "engram-candidate-precondition-map.json",
            "residue-to-candidate-refusal-ledger.json",
            "engram-candidate-admission-ceiling-matrix.json");
        var swarmCustodyBraidVerified = HasArtifacts(
            cellRoot,
            "swarm-worker-packet-contract-map.json",
            "swarm-braid-selection-boundary-matrix.json",
            "swarm-consensus-non-warrant-ledger.json");
        var persistentWitnessStoreVerified = HasArtifacts(
            cellRoot,
            "persistent-witness-store-contract-map.json",
            "persistent-witness-store-custody-boundary-matrix.json",
            "witness-storage-non-authority-ledger.json");
        var sliLispPostureManifestVerified = HasArtifacts(
            cellRoot,
            "sli-lisp-posture-manifest-map.json",
            "csharp-lisp-duplex-non-evaluation-boundary-matrix.json",
            "sli-lisp-posture-non-execution-ledger.json");
        var sliLispCompassCarrierShellVerified = HasArtifacts(
            cellRoot,
            "sli-lisp-compass-carrier-shell-map.json",
            "sli-lisp-rooting-law-lineage-ledger.json",
            "sli-lisp-petal-candidate-gap-matrix.json");
        var ecMeaningShellVerified = HasArtifacts(
            cellRoot,
            "ec-meaning-shell-contract-map.json",
            "ec-perspectival-tier-boundary-matrix.json",
            "ec-compost-non-self-attribution-ledger.json");
        var ecParticipatoryPeerlessVerified = HasArtifacts(
            cellRoot,
            "ec-participatory-predicate-structure-map.json",
            "ec-peerless-delta-witness-boundary-matrix.json",
            "ec-personification-non-authority-ledger.json");
        var cmeLispThreadFretboardVerified = HasArtifacts(
            cellRoot,
            "cme-lisp-thread-class-map.json",
            "cme-lisp-thread-tension-playability-matrix.json",
            "cme-lisp-resonance-non-authority-ledger.json");
        var cmeLispResonanceHeartbeatVerified = HasArtifacts(
            cellRoot,
            "listening-frame-emanation-map.json",
            "global-resonance-law-ledger.json",
            "steward-heartbeat-policy-map.json",
            "thread-touch-event-boundary.json",
            "resonance-evidence-ledger.json",
            "damping-discordance-route-matrix.json",
            "action-admission-boundary-report.json");
        var stewardHarmonicInterlockVerified = HasArtifacts(
            cellRoot,
            "steward-harmonic-interlock-map.json",
            "lawful-signal-composability-matrix.json",
            "shared-surface-contention-ledger.json",
            "cadence-alignment-policy-map.json",
            "damping-backoff-policy-map.json",
            "witness-surface-split-route-map.json",
            "interlock-non-authority-boundary-report.json");
        var modulationCorrespondenceVerified = HasArtifacts(
            cellRoot,
            "modulation-correspondence-atlas-map.json",
            "source-domain-success-condition-ledger.json",
            "cme-translation-boundary-matrix.json",
            "channel-success-non-warrant-ledger.json",
            "correspondence-loss-condition-ledger.json",
            "operational-actualization-test-map.json",
            "mature-discipline-intake-protocol.json");
        var typedActionFormationVerified = HasArtifacts(
            cellRoot,
            "typed-action-surface-declaration-map.json",
            "methodological-formation-analysis-map.json",
            "design-predicate-boundary-matrix.json",
            "action-candidate-non-execution-ledger.json",
            "sli-lisp-action-surface-declaration-carrier.json");
        var actionMethodReadinessVerified = HasArtifacts(
            cellRoot,
            "action-method-readiness-map.json",
            "steward-method-review-boundary-matrix.json",
            "method-term-satisfaction-non-warrant-ledger.json",
            "method-lineage-custody-map.json",
            "sli-lisp-method-readiness-carrier.json");
        var stewardActionAdmissibilityVerified = HasArtifacts(
            cellRoot,
            "steward-action-admissibility-map.json",
            "admissibility-predicate-result-matrix.json",
            "admissibility-non-execution-ledger.json",
            "admissible-action-custody-lineage-map.json",
            "sli-lisp-steward-admissibility-carrier.json");
        var antiCaptureMotivatedConcernVerified = HasArtifacts(
            cellRoot,
            "anti-capture-motivated-concern-map.json",
            "motivational-variance-signal-matrix.json",
            "concern-non-action-ledger.json",
            "capture-pressure-route-custody-map.json",
            "sli-lisp-anti-capture-concern-carrier.json");
        var personificationPredicateHookVerified = HasArtifacts(
            cellRoot,
            "personification-predicate-hook-map.json",
            "six-plane-personification-hook-matrix.json",
            "vulnerability-overreach-repair-ledger.json",
            "personification-non-personhood-ledger.json",
            "sli-lisp-personification-hook-carrier.json");
        var personificationModalityHumilityVerified = HasArtifacts(
            cellRoot,
            "personification-modality-humility-map.json",
            "bonded-relation-consent-custody-matrix.json",
            "modality-bandwidth-non-authority-ledger.json",
            "presence-non-embodiment-refusal-ledger.json",
            "sli-lisp-personification-modality-carrier.json");
        var dialogosDiscernmentVerified = HasArtifacts(
            cellRoot,
            "dialogos-thought-status-map.json",
            "articulation-warrant-boundary-matrix.json",
            "principled-refusal-return-path-ledger.json",
            "perspectival-knowing-participatory-thought-form-map.json",
            "sli-lisp-dialogos-discernment-carrier.json");
        var waveCondensationSharedRealityVerified = HasArtifacts(
            cellRoot,
            "wave-condensation-signal-map.json",
            "shared-reality-anchor-boundary-matrix.json",
            "condensation-non-warrant-ledger.json",
            "consensus-non-authority-refusal-ledger.json",
            "sli-lisp-wave-condensation-carrier.json");
        var waveCascadeRunVerified = HasArtifacts(
            cellRoot,
            "wave-cascade-run-schedule.json",
            "thirty-sixty-ninety-seam-receipt-ledger.json",
            "cascade-volume-non-warrant-ledger.json",
            "cascade-shared-reality-braid-map.json",
            "sli-lisp-wave-cascade-carrier.json");
        var aspirationPayloadIngestionMaturationVerified = HasArtifacts(
            cellRoot,
            "aspiration-payload-map.json",
            "payload-ingestion-lane-matrix.json",
            "articulation-maturation-candidate-ledger.json",
            "full-stack-non-activation-refusal-ledger.json",
            "sli-lisp-aspiration-payload-carrier.json");
        var aspirationCandidateSelectionClosureVerified = HasArtifacts(
            cellRoot,
            "aspiration-candidate-selection-map.json",
            "selected-working-set-non-warrant-ledger.json",
            "closure-law-without-key-boundary-matrix.json",
            "compost-retention-non-erasure-ledger.json",
            "sli-lisp-aspiration-selection-carrier.json");
        var scopedWorkPacketFormationVerified = HasArtifacts(
            cellRoot,
            "scoped-work-packet-map.json",
            "packet-scope-boundary-matrix.json",
            "work-packet-non-execution-ledger.json",
            "steward-review-routing-custody-map.json",
            "sli-lisp-scoped-work-packet-carrier.json");
        var enactmentBoundaryReadinessVerified = HasArtifacts(
            cellRoot,
            "enactment-boundary-readiness-map.json",
            "enactment-approach-non-execution-ledger.json",
            "reversible-local-effect-ceiling-matrix.json",
            "steward-enactment-review-custody-map.json",
            "sli-lisp-enactment-boundary-carrier.json");
        var enactmentDryRunRehearsalVerified = HasArtifacts(
            cellRoot,
            "enactment-dry-run-harness-map.json",
            "dry-run-rehearsal-non-enactment-ledger.json",
            "simulated-effect-and-rollback-proof-matrix.json",
            "steward-dry-run-review-receipt-map.json",
            "sli-lisp-dry-run-rehearsal-carrier.json");
        var ecPrecipitationWitnessVerified = HasArtifacts(
            cellRoot,
            "ec-precipitation-witness-map.json",
            "active-witness-lineage-reconstruction-matrix.json",
            "selfgel-candidate-non-admission-ledger.json",
            "maximal-truth-seeking-predicate-law-ledger.json",
            "sli-lisp-ec-precipitation-witness-carrier.json");
        var rehearsalDistinctionPressureVerified = HasArtifacts(
            cellRoot,
            "rehearsal-distinction-pressure-map.json",
            "possibility-density-pressure-vector-ledger.json",
            "urgency-not-jurisdiction-refusal-ledger.json",
            "failure-dignity-cooling-matrix.json",
            "sli-lisp-rehearsal-pressure-carrier.json");
        var personificationActualizationSurfaceVerified = HasArtifacts(
            cellRoot,
            "personification-actualization-surface-map.json",
            "pre-morphological-use-vector-ledger.json",
            "surface-actualization-non-identity-matrix.json",
            "salience-guidance-non-authority-ledger.json",
            "sli-lisp-personification-actualization-carrier.json");
        var selectiveLawfulActionSurfaceVerified = HasArtifacts(
            cellRoot,
            "selective-lawful-action-surface-map.json",
            "surface-touch-non-enactment-ledger.json",
            "personification-guidance-action-separation-matrix.json",
            "action-surface-custody-revocation-ledger.json",
            "sli-lisp-selective-action-surface-carrier.json");
        var zedDeltaChamberFormationVerified = HasArtifacts(
            cellRoot,
            "zed-delta-chamber-formation-map.json",
            "conditional-oe-selfgel-standing-matrix.json",
            "mos-cmos-residue-closure-ledger.json",
            "goa-cgoa-soulframe-duplex-telemetry-map.json",
            "heartbeat-non-activation-refusal-ledger.json",
            "sli-lisp-zed-delta-chamber-carrier.json");
        var highEnergyArticulationCandidateVerified = HasArtifacts(
            cellRoot,
            "high-energy-articulation-candidate-map.json",
            "provider-interface-observability-ledger.json",
            "hidden-substrate-non-claim-matrix.json",
            "candidate-engine-non-binding-ledger.json",
            "candidate-role-assignment-boundary-map.json",
            "sli-lisp-high-energy-articulation-candidate-carrier.json");
        var membraneMorphologyTransitionVerified = HasArtifacts(
            cellRoot,
            "membrane-morphology-transition-map.json",
            "membrane-deformation-classification-ledger.json",
            "malformed-transition-compost-matrix.json",
            "high-energy-pressure-non-binding-boundary-ledger.json",
            "membrane-core-non-mutation-ledger.json",
            "sli-lisp-membrane-morphology-transition-carrier.json");
        var engramPredicatePrecursorStreamVerified = HasArtifacts(
            cellRoot,
            "engram-predicate-precursor-stream-map.json",
            "predicate-residue-classification-ledger.json",
            "predicate-candidacy-non-admission-matrix.json",
            "epps-non-memory-non-authority-ledger.json",
            "sli-lisp-epps-carrier.json");
        var peerReviewPredicateBridgeVerified = HasArtifacts(
            cellRoot,
            "peer-review-predicate-bridge-map.json",
            "reader-state-continuity-ladder.json",
            "terminology-quarantine-ledger.json",
            "prose-smoothing-boundary-matrix.json",
            "sli-lisp-peer-review-bridge-carrier.json");
        var gelDomainScopedIngressVerified = HasArtifacts(
            cellRoot,
            "gel-domain-scoped-ingress-map.json",
            "domain-evidence-ceiling-ledger.json",
            "ingress-cycle-non-admission-matrix.json",
            "certification-review-non-admission-ledger.json",
            "sli-lisp-gel-domain-ingress-carrier.json");
        var sharedPrimeRealityPressureEcologyVerified = HasArtifacts(
            cellRoot,
            "shared-prime-pressure-ecology-map.json",
            "pressure-destination-classification-ledger.json",
            "integration-pressure-non-admission-matrix.json",
            "selfgel-cradle-sanctuary-pressure-separation-ledger.json",
            "sli-lisp-shared-prime-pressure-carrier.json");
        var gapCrossingArticulationVerified = HasArtifacts(
            cellRoot,
            "gap-crossing-articulation-map.json",
            "llm-surface-participation-non-binding-ledger.json",
            "pressure-to-articulation-lane-classification.json",
            "gap-crossing-non-action-authority-matrix.json",
            "sli-lisp-gap-crossing-carrier.json");
        var preDiagnosticRiskSurfaceVerified = HasArtifacts(
            cellRoot,
            "pre-diagnostic-risk-surface-map.json",
            "care-signal-non-diagnosis-ledger.json",
            "risk-modifier-care-burden-matrix.json",
            "qualified-review-routing-non-authority-ledger.json",
            "sli-lisp-pre-diagnostic-risk-carrier.json");
        var layerMapStatus = layerMapVerified
            ? SpiralBuildCellStatus.VerifiedCold
            : installVerified
                ? SpiralBuildCellStatus.Candidate
                : SpiralBuildCellStatus.Blocked;
        var cellTaxonomyStatus = cellTaxonomyVerified ? SpiralBuildCellStatus.VerifiedCold : SpiralBuildCellStatus.Planned;
        var primeStewardStatus = primeStewardVerified ? SpiralBuildCellStatus.VerifiedCold : SpiralBuildCellStatus.Planned;
        var crypticStewardStatus = crypticStewardVerified ? SpiralBuildCellStatus.VerifiedCold : SpiralBuildCellStatus.Planned;
        var compassShellStatus = compassShellVerified ? SpiralBuildCellStatus.VerifiedCold : SpiralBuildCellStatus.Planned;
        var telemetryStatus = telemetryVerified ? SpiralBuildCellStatus.VerifiedCold : SpiralBuildCellStatus.Planned;
        var packetMembraneStatus = packetMembraneVerified ? SpiralBuildCellStatus.VerifiedCold : SpiralBuildCellStatus.Planned;
        var packetReceiptRoutingStatus = packetReceiptRoutingVerified ? SpiralBuildCellStatus.VerifiedCold : SpiralBuildCellStatus.Planned;
        var packetReceiptReplayStatus = packetReceiptReplayVerified ? SpiralBuildCellStatus.VerifiedCold : SpiralBuildCellStatus.Planned;
        var packetReceiptQueryStatus = packetReceiptQueryVerified ? SpiralBuildCellStatus.VerifiedCold : SpiralBuildCellStatus.Planned;
        var packetReceiptSelectionStatus = packetReceiptSelectionVerified ? SpiralBuildCellStatus.VerifiedCold : SpiralBuildCellStatus.Planned;
        var witnessSummaryStatus = witnessSummaryVerified ? SpiralBuildCellStatus.VerifiedCold : SpiralBuildCellStatus.Planned;
        var compassPressureStatus = compassPressureVerified ? SpiralBuildCellStatus.VerifiedCold : SpiralBuildCellStatus.Planned;
        var compassShellStabilizationStatus = compassShellStabilizationVerified ? SpiralBuildCellStatus.VerifiedCold : SpiralBuildCellStatus.Planned;
        var cleavingDiscernmentStatus = cleavingDiscernmentVerified ? SpiralBuildCellStatus.VerifiedCold : SpiralBuildCellStatus.Planned;
        var iterativeEvaluationStatus = iterativeEvaluationVerified ? SpiralBuildCellStatus.VerifiedCold : SpiralBuildCellStatus.Planned;
        var recursiveContemplationStatus = recursiveContemplationVerified ? SpiralBuildCellStatus.VerifiedCold : SpiralBuildCellStatus.Planned;
        var stewardHandoffReadinessStatus = stewardHandoffReadinessVerified ? SpiralBuildCellStatus.VerifiedCold : SpiralBuildCellStatus.Planned;
        var typedDuplexIterationStatus = typedDuplexIterationVerified ? SpiralBuildCellStatus.VerifiedCold : SpiralBuildCellStatus.Planned;
        var tenByTenScheduleStatus = tenByTenScheduleVerified ? SpiralBuildCellStatus.VerifiedCold : SpiralBuildCellStatus.Planned;
        var tenByTenGroupAStatus = tenByTenGroupAVerified ? SpiralBuildCellStatus.VerifiedCold : SpiralBuildCellStatus.Planned;
        var tenByTenGroupBStatus = tenByTenGroupBVerified ? SpiralBuildCellStatus.VerifiedCold : SpiralBuildCellStatus.Planned;
        var tenByTenGroupCStatus = tenByTenGroupCVerified ? SpiralBuildCellStatus.VerifiedCold : SpiralBuildCellStatus.Planned;
        var tenByTenGroupDStatus = tenByTenGroupDVerified ? SpiralBuildCellStatus.VerifiedCold : SpiralBuildCellStatus.Planned;
        var wholeBodySynthesisStatus = wholeBodySynthesisVerified ? SpiralBuildCellStatus.VerifiedCold : SpiralBuildCellStatus.Planned;
        var ninefoldColdReviewStatus = ninefoldColdReviewVerified ? SpiralBuildCellStatus.VerifiedCold : SpiralBuildCellStatus.Planned;
        var engramCandidatePreconditionStatus = engramCandidatePreconditionVerified ? SpiralBuildCellStatus.VerifiedCold : SpiralBuildCellStatus.Planned;
        var swarmCustodyBraidStatus = swarmCustodyBraidVerified ? SpiralBuildCellStatus.VerifiedCold : SpiralBuildCellStatus.Planned;
        var persistentWitnessStoreStatus = persistentWitnessStoreVerified ? SpiralBuildCellStatus.VerifiedCold : SpiralBuildCellStatus.Planned;
        var sliLispPostureManifestStatus = sliLispPostureManifestVerified ? SpiralBuildCellStatus.VerifiedCold : SpiralBuildCellStatus.Planned;
        var sliLispCompassCarrierShellStatus = sliLispCompassCarrierShellVerified ? SpiralBuildCellStatus.VerifiedCold : SpiralBuildCellStatus.Planned;
        var ecMeaningShellStatus = ecMeaningShellVerified ? SpiralBuildCellStatus.VerifiedCold : SpiralBuildCellStatus.Planned;
        var ecParticipatoryPeerlessStatus = ecParticipatoryPeerlessVerified ? SpiralBuildCellStatus.VerifiedCold : SpiralBuildCellStatus.Planned;
        var cmeLispThreadFretboardStatus = cmeLispThreadFretboardVerified ? SpiralBuildCellStatus.VerifiedCold : SpiralBuildCellStatus.Planned;
        var cmeLispResonanceHeartbeatStatus = cmeLispResonanceHeartbeatVerified ? SpiralBuildCellStatus.VerifiedCold : SpiralBuildCellStatus.Planned;
        var stewardHarmonicInterlockStatus = stewardHarmonicInterlockVerified ? SpiralBuildCellStatus.VerifiedCold : SpiralBuildCellStatus.Planned;
        var modulationCorrespondenceStatus = modulationCorrespondenceVerified ? SpiralBuildCellStatus.VerifiedCold : SpiralBuildCellStatus.Planned;
        var typedActionFormationStatus = typedActionFormationVerified ? SpiralBuildCellStatus.VerifiedCold : SpiralBuildCellStatus.Planned;
        var actionMethodReadinessStatus = actionMethodReadinessVerified ? SpiralBuildCellStatus.VerifiedCold : SpiralBuildCellStatus.Planned;
        var stewardActionAdmissibilityStatus = stewardActionAdmissibilityVerified ? SpiralBuildCellStatus.VerifiedCold : SpiralBuildCellStatus.Planned;
        var antiCaptureMotivatedConcernStatus = antiCaptureMotivatedConcernVerified ? SpiralBuildCellStatus.VerifiedCold : SpiralBuildCellStatus.Planned;
        var personificationPredicateHookStatus = personificationPredicateHookVerified ? SpiralBuildCellStatus.VerifiedCold : SpiralBuildCellStatus.Planned;
        var personificationModalityHumilityStatus = personificationModalityHumilityVerified ? SpiralBuildCellStatus.VerifiedCold : SpiralBuildCellStatus.Planned;
        var dialogosDiscernmentStatus = dialogosDiscernmentVerified ? SpiralBuildCellStatus.VerifiedCold : SpiralBuildCellStatus.Planned;
        var waveCondensationSharedRealityStatus = waveCondensationSharedRealityVerified ? SpiralBuildCellStatus.VerifiedCold : SpiralBuildCellStatus.Planned;
        var waveCascadeRunStatus = waveCascadeRunVerified ? SpiralBuildCellStatus.VerifiedCold : SpiralBuildCellStatus.Planned;
        var aspirationPayloadIngestionMaturationStatus = aspirationPayloadIngestionMaturationVerified ? SpiralBuildCellStatus.VerifiedCold : SpiralBuildCellStatus.Planned;
        var aspirationCandidateSelectionClosureStatus = aspirationCandidateSelectionClosureVerified ? SpiralBuildCellStatus.VerifiedCold : SpiralBuildCellStatus.Planned;
        var scopedWorkPacketFormationStatus = scopedWorkPacketFormationVerified ? SpiralBuildCellStatus.VerifiedCold : SpiralBuildCellStatus.Planned;
        var enactmentBoundaryReadinessStatus = enactmentBoundaryReadinessVerified ? SpiralBuildCellStatus.VerifiedCold : SpiralBuildCellStatus.Planned;
        var enactmentDryRunRehearsalStatus = enactmentDryRunRehearsalVerified ? SpiralBuildCellStatus.VerifiedCold : SpiralBuildCellStatus.Planned;
        var ecPrecipitationWitnessStatus = ecPrecipitationWitnessVerified ? SpiralBuildCellStatus.VerifiedCold : SpiralBuildCellStatus.Planned;
        var rehearsalDistinctionPressureStatus = rehearsalDistinctionPressureVerified ? SpiralBuildCellStatus.VerifiedCold : SpiralBuildCellStatus.Planned;
        var personificationActualizationSurfaceStatus = personificationActualizationSurfaceVerified ? SpiralBuildCellStatus.VerifiedCold : SpiralBuildCellStatus.Planned;
        var selectiveLawfulActionSurfaceStatus = selectiveLawfulActionSurfaceVerified ? SpiralBuildCellStatus.VerifiedCold : SpiralBuildCellStatus.Planned;
        var zedDeltaChamberFormationStatus = zedDeltaChamberFormationVerified ? SpiralBuildCellStatus.VerifiedCold : SpiralBuildCellStatus.Planned;
        var highEnergyArticulationCandidateStatus = highEnergyArticulationCandidateVerified ? SpiralBuildCellStatus.VerifiedCold : SpiralBuildCellStatus.Planned;
        var membraneMorphologyTransitionStatus = membraneMorphologyTransitionVerified ? SpiralBuildCellStatus.VerifiedCold : SpiralBuildCellStatus.Planned;
        var engramPredicatePrecursorStreamStatus = engramPredicatePrecursorStreamVerified ? SpiralBuildCellStatus.VerifiedCold : SpiralBuildCellStatus.Planned;
        var peerReviewPredicateBridgeStatus = peerReviewPredicateBridgeVerified ? SpiralBuildCellStatus.VerifiedCold : SpiralBuildCellStatus.Planned;
        var gelDomainScopedIngressStatus = gelDomainScopedIngressVerified ? SpiralBuildCellStatus.VerifiedCold : SpiralBuildCellStatus.Planned;
        var sharedPrimeRealityPressureEcologyStatus = sharedPrimeRealityPressureEcologyVerified ? SpiralBuildCellStatus.VerifiedCold : SpiralBuildCellStatus.Planned;
        var gapCrossingArticulationStatus = gapCrossingArticulationVerified ? SpiralBuildCellStatus.VerifiedCold : SpiralBuildCellStatus.Planned;
        var preDiagnosticRiskSurfaceStatus = preDiagnosticRiskSurfaceVerified ? SpiralBuildCellStatus.VerifiedCold : SpiralBuildCellStatus.Planned;

        return
        [
            new SpiralBuildCellRecord(
                CellId: "full-body.install-anchor",
                Phase: SpiralBuildPhase.FullBodyPass,
                Layer: "install-surface",
                CellName: "Local Sanctuary installed cold tool surface",
                Status: installStatus,
                AdjacentTo: [],
                RequiredArtifacts: ["sanctuary.cmd", "product/San.Launcher.exe", "SANCTUARY_INSTALL_RECEIPT.md"],
                StopConditions: StopConditions,
                NextAction: "verify installed command shims and receipts",
                HitlRequired: false),
            new SpiralBuildCellRecord(
                CellId: "full-body.preflight-anchor",
                Phase: SpiralBuildPhase.FullBodyPass,
                Layer: "verification-surface",
                CellName: "Installed preflight and refusal anchor",
                Status: preflightStatus,
                AdjacentTo: ["full-body.install-anchor"],
                RequiredArtifacts: ["receipts/preflight/product-body-status.json"],
                StopConditions: StopConditions,
                NextAction: "run sanctuary.cmd verify and refuse-activation",
                HitlRequired: false),
            new SpiralBuildCellRecord(
                CellId: "full-body.codex-proxy-triptych",
                Phase: SpiralBuildPhase.FullBodyPass,
                Layer: "proxy-cognition-surface",
                CellName: "Codex proxy Prime/Cryptic/Steward triptych profile",
                Status: triptychStatus,
                AdjacentTo: ["full-body.preflight-anchor"],
                RequiredArtifacts: ["receipts/sanctuary-actual-test-profile/sanctuary-actual-test-profile.json"],
                StopConditions: StopConditions,
                NextAction: "emit Codex proxy triptych profile receipt",
                HitlRequired: false),
            new SpiralBuildCellRecord(
                CellId: "full-body.layer-map",
                Phase: SpiralBuildPhase.FullBodyPass,
                Layer: "body-map",
                CellName: "Full body layer map and phase ledger",
                Status: layerMapStatus,
                AdjacentTo: ["full-body.install-anchor", "full-body.codex-proxy-triptych"],
                RequiredArtifacts: ["body-layer-map.json", "cell-taxonomy-outline.json", "non-collapse-law-ledger.json"],
                StopConditions: StopConditions,
                NextAction: "write body-layer map, cell taxonomy, and non-collapse ledger",
                HitlRequired: false),
            new SpiralBuildCellRecord(
                CellId: "cellular.cell-taxonomy",
                Phase: SpiralBuildPhase.CellularStructure,
                Layer: "cellular-structure",
                CellName: "Cell taxonomy and adjacency ledger",
                Status: cellTaxonomyStatus,
                AdjacentTo: ["full-body.layer-map"],
                RequiredArtifacts: ["cell-taxonomy.json", "adjacency-ledger.json", "cell-receipt-template.json"],
                StopConditions: StopConditions,
                NextAction: "define smallest buildable cell contract, carrier, receipt, refusal, and test shapes",
                HitlRequired: false),
            new SpiralBuildCellRecord(
                CellId: "membrane.prime-steward",
                Phase: SpiralBuildPhase.Membrane,
                Layer: "membrane",
                CellName: "Prime to Steward cGoA-insulated membrane",
                Status: primeStewardStatus,
                AdjacentTo: ["cellular.cell-taxonomy"],
                RequiredArtifacts: ["prime-steward-membrane-map.json", "prime-steward-allowed-passage.json", "prime-steward-refusal-case.json"],
                StopConditions: StopConditions,
                NextAction: "prove Prime review reaches Steward through cGoA insulation only",
                HitlRequired: false),
            new SpiralBuildCellRecord(
                CellId: "membrane.cryptic-steward",
                Phase: SpiralBuildPhase.Membrane,
                Layer: "membrane",
                CellName: "Cryptic to Steward telemetry-string membrane",
                Status: crypticStewardStatus,
                AdjacentTo: ["membrane.prime-steward"],
                RequiredArtifacts: ["cryptic-steward-membrane-map.json", "cryptic-steward-telemetry-route.json", "cryptic-steward-self-witness-refusal.json"],
                StopConditions: StopConditions,
                NextAction: "prove Cryptic telemetry reaches review without self-authorization",
                HitlRequired: false),
            new SpiralBuildCellRecord(
                CellId: "instrument.compass-shell",
                Phase: SpiralBuildPhase.InstrumentBodyHardening,
                Layer: "instrument-body",
                CellName: "Compass shell worktable hardening",
                Status: compassShellStatus,
                AdjacentTo: ["membrane.cryptic-steward"],
                RequiredArtifacts: ["compass-shell-packet.json", "cleaving-decision-receipt.json", "compass-candidate-only-refusal.json"],
                StopConditions: StopConditions,
                NextAction: "prove Compass shell remains pre-continuity and candidate-only",
                HitlRequired: false),
            new SpiralBuildCellRecord(
                CellId: "telemetry.receipt-continuity",
                Phase: SpiralBuildPhase.WiringTelemetryHardening,
                Layer: "wiring-telemetry",
                CellName: "Receipt continuity and telemetry routing hardening",
                Status: telemetryStatus,
                AdjacentTo: ["instrument.compass-shell"],
                RequiredArtifacts: ["telemetry-route-map.json", "receipt-continuity-test-plan.json", "telemetry-authority-refusal.json"],
                StopConditions: StopConditions,
                NextAction: "prove telemetry is observable without becoming authority",
                HitlRequired: false),
            new SpiralBuildCellRecord(
                CellId: "packet-membrane.contract-validation",
                Phase: SpiralBuildPhase.PacketMembraneContractValidation,
                Layer: "packet-membrane",
                CellName: "Typed packet and membrane contract validation",
                Status: packetMembraneStatus,
                AdjacentTo: ["telemetry.receipt-continuity"],
                RequiredArtifacts: ["sanctuary-packet-contract-map.json", "packet-membrane-validation-matrix.json", "packet-non-authority-refusal-ledger.json"],
                StopConditions: StopConditions,
                NextAction: "prove typed packets cross membrane contracts without carrying undeclared authority",
                HitlRequired: false),
            new SpiralBuildCellRecord(
                CellId: "packet-membrane.receipt-routing",
                Phase: SpiralBuildPhase.PacketMembraneReceiptRouting,
                Layer: "packet-membrane",
                CellName: "Packet passage and refusal receipt routing",
                Status: packetReceiptRoutingStatus,
                AdjacentTo: ["packet-membrane.contract-validation"],
                RequiredArtifacts: ["packet-passage-receipt-map.json", "packet-refusal-routing-matrix.json", "receipt-non-permission-ledger.json"],
                StopConditions: StopConditions,
                NextAction: "prove packet receipts route to witness and custody without becoming permission",
                HitlRequired: false),
            new SpiralBuildCellRecord(
                CellId: "packet-membrane.receipt-replay-boundary",
                Phase: SpiralBuildPhase.PacketMembraneReceiptReplayBoundary,
                Layer: "packet-membrane",
                CellName: "Packet receipt replay non-reentry boundary",
                Status: packetReceiptReplayStatus,
                AdjacentTo: ["packet-membrane.receipt-routing"],
                RequiredArtifacts: ["receipt-replay-request-map.json", "receipt-replay-boundary-matrix.json", "replay-non-reentry-ledger.json"],
                StopConditions: StopConditions,
                NextAction: "prove retained receipts replay for review without repeating passage",
                HitlRequired: false),
            new SpiralBuildCellRecord(
                CellId: "packet-membrane.receipt-query-boundary",
                Phase: SpiralBuildPhase.PacketMembraneReceiptQueryBoundary,
                Layer: "packet-membrane",
                CellName: "Packet receipt query non-warrant boundary",
                Status: packetReceiptQueryStatus,
                AdjacentTo: ["packet-membrane.receipt-replay-boundary"],
                RequiredArtifacts: ["receipt-query-request-map.json", "receipt-query-boundary-matrix.json", "query-non-warrant-ledger.json"],
                StopConditions: StopConditions,
                NextAction: "prove retained receipts can be located without manufacturing warrant",
                HitlRequired: false),
            new SpiralBuildCellRecord(
                CellId: "packet-membrane.receipt-selection-boundary",
                Phase: SpiralBuildPhase.PacketMembraneReceiptSelectionBoundary,
                Layer: "packet-membrane",
                CellName: "Packet receipt selection non-admission boundary",
                Status: packetReceiptSelectionStatus,
                AdjacentTo: ["packet-membrane.receipt-query-boundary"],
                RequiredArtifacts: ["receipt-selection-request-map.json", "receipt-selection-boundary-matrix.json", "selection-non-admission-ledger.json"],
                StopConditions: StopConditions,
                NextAction: "prove retained evidence can be nominated for review without admission",
                HitlRequired: false),
            new SpiralBuildCellRecord(
                CellId: "witness.summary-boundary",
                Phase: SpiralBuildPhase.WitnessSummaryBoundary,
                Layer: "witness",
                CellName: "Witness summary non-replacement boundary",
                Status: witnessSummaryStatus,
                AdjacentTo: ["packet-membrane.receipt-selection-boundary"],
                RequiredArtifacts: ["witness-summary-request-map.json", "witness-summary-boundary-matrix.json", "summary-non-replacement-ledger.json"],
                StopConditions: StopConditions,
                NextAction: "prove selected evidence can be summarized without replacing evidence",
                HitlRequired: false),
            new SpiralBuildCellRecord(
                CellId: "compass.pre-engram-pressure-boundary",
                Phase: SpiralBuildPhase.CompassPreEngramPressureBoundary,
                Layer: "compass",
                CellName: "Compass pre-engram pressure non-engram boundary",
                Status: compassPressureStatus,
                AdjacentTo: ["witness.summary-boundary"],
                RequiredArtifacts: ["compass-pre-engram-pressure-request-map.json", "compass-pressure-boundary-matrix.json", "pressure-non-engram-ledger.json"],
                StopConditions: StopConditions,
                NextAction: "prove witness summary can pressure Compass without becoming engram",
                HitlRequired: false),
            new SpiralBuildCellRecord(
                CellId: "compass.shell-stabilization-boundary",
                Phase: SpiralBuildPhase.CompassShellStabilizationBoundary,
                Layer: "compass",
                CellName: "Compass shell stabilization candidate boundary",
                Status: compassShellStabilizationStatus,
                AdjacentTo: ["compass.pre-engram-pressure-boundary"],
                RequiredArtifacts: ["compass-shell-stabilization-map.json", "shell-pressure-lineage-map.json", "shell-non-engram-boundary-ledger.json"],
                StopConditions: StopConditions,
                NextAction: "prove bounded pressure may form shell candidate without becoming engram",
                HitlRequired: false),
            new SpiralBuildCellRecord(
                CellId: "inner-chamber.cleaving-discernment-boundary",
                Phase: SpiralBuildPhase.CleavingDiscernmentBoundary,
                Layer: "inner-chamber",
                CellName: "Cleaving discernment non-admission boundary",
                Status: cleavingDiscernmentStatus,
                AdjacentTo: ["compass.shell-stabilization-boundary"],
                RequiredArtifacts: ["cleaving-discernment-request-map.json", "cleaving-refusal-boundary-matrix.json", "cleaving-non-admission-ledger.json"],
                StopConditions: StopConditions,
                NextAction: "prove cleaving can separate candidate posture from admission or authority",
                HitlRequired: false),
            new SpiralBuildCellRecord(
                CellId: "inner-chamber.iterative-evaluation-boundary",
                Phase: SpiralBuildPhase.IterativeEvaluationBoundary,
                Layer: "inner-chamber",
                CellName: "Iterative evaluation non-authority boundary",
                Status: iterativeEvaluationStatus,
                AdjacentTo: ["inner-chamber.cleaving-discernment-boundary"],
                RequiredArtifacts: ["iterative-evaluation-loop-map.json", "evaluation-non-authority-ledger.json", "evaluation-tuning-candidate-map.json"],
                StopConditions: StopConditions,
                NextAction: "prove iterative evaluation may improve candidate structure without authorizing it",
                HitlRequired: false),
            new SpiralBuildCellRecord(
                CellId: "inner-chamber.recursive-contemplation-boundary",
                Phase: SpiralBuildPhase.RecursiveContemplationBoundary,
                Layer: "inner-chamber",
                CellName: "Recursive contemplation non-continuity boundary",
                Status: recursiveContemplationStatus,
                AdjacentTo: ["inner-chamber.iterative-evaluation-boundary"],
                RequiredArtifacts: ["recursive-contemplation-loop-map.json", "contemplation-non-continuity-ledger.json", "contemplation-cooling-path-map.json"],
                StopConditions: StopConditions,
                NextAction: "prove recursive contemplation may revisit candidates without admitting continuity",
                HitlRequired: false),
            new SpiralBuildCellRecord(
                CellId: "steward.handoff-readiness-boundary",
                Phase: SpiralBuildPhase.StewardHandoffReadinessBoundary,
                Layer: "steward",
                CellName: "Steward handoff readiness and ten-pass tuning gate",
                Status: stewardHandoffReadinessStatus,
                AdjacentTo: ["inner-chamber.recursive-contemplation-boundary"],
                RequiredArtifacts: ["steward-handoff-readiness-map.json", "handoff-non-authorization-ledger.json", "ten-pass-body-tuning-next-lane-map.json"],
                StopConditions: StopConditions,
                NextAction: "prove handoff readiness can name the ten-pass tuning lane without starting activation",
                HitlRequired: false),
            new SpiralBuildCellRecord(
                CellId: "iteration.typed-duplex-build-map",
                Phase: SpiralBuildPhase.TypedDuplexIterationMap,
                Layer: "iteration",
                CellName: "Typed duplex build iteration map",
                Status: typedDuplexIterationStatus,
                AdjacentTo: ["steward.handoff-readiness-boundary"],
                RequiredArtifacts: ["typed-duplex-build-iteration-map.json", "iteration-flow-form-learning-map.json", "theory-direct-representation-optimization-map.json"],
                StopConditions: StopConditions,
                NextAction: "prove iteration can learn flow and form before optimizing direct theory representation",
                HitlRequired: false),
            new SpiralBuildCellRecord(
                CellId: "iteration.ten-by-ten-body-optimization-schedule",
                Phase: SpiralBuildPhase.TenByTenBodyOptimizationSchedule,
                Layer: "iteration",
                CellName: "Ten by ten body optimization schedule",
                Status: tenByTenScheduleStatus,
                AdjacentTo: ["iteration.typed-duplex-build-map"],
                RequiredArtifacts: ["ten-by-ten-body-section-pass-matrix.json", "three-section-cascade-group-map.json", "lamp-body-seed-exclusion-optimization-ledger.json"],
                StopConditions: StopConditions,
                NextAction: "prove ten by ten optimization is grouped, typed, cold, and seed-excluded",
                HitlRequired: false),
            new SpiralBuildCellRecord(
                CellId: "iteration.group-a-optimization-run",
                Phase: SpiralBuildPhase.TenByTenGroupAOptimizationRun,
                Layer: "iteration",
                CellName: "Group A ten-pass optimization run",
                Status: tenByTenGroupAStatus,
                AdjacentTo: ["iteration.ten-by-ten-body-optimization-schedule"],
                RequiredArtifacts: ["group-a-body-optimization-run-ledger.json", "group-a-flow-form-findings-map.json", "group-a-next-group-eligibility-receipt.json"],
                StopConditions: StopConditions,
                NextAction: "execute sections 1-3 across ten review-only passes and decide Group B eligibility",
                HitlRequired: false),
            new SpiralBuildCellRecord(
                CellId: "iteration.group-b-optimization-run",
                Phase: SpiralBuildPhase.TenByTenGroupBOptimizationRun,
                Layer: "iteration",
                CellName: "Group B ten-pass optimization run",
                Status: tenByTenGroupBStatus,
                AdjacentTo: ["iteration.group-a-optimization-run"],
                RequiredArtifacts: ["group-b-body-optimization-run-ledger.json", "group-b-flow-form-findings-map.json", "group-b-next-group-eligibility-receipt.json"],
                StopConditions: StopConditions,
                NextAction: "execute sections 4-6 across ten review-only passes and decide Group C eligibility",
                HitlRequired: false),
            new SpiralBuildCellRecord(
                CellId: "iteration.group-c-optimization-run",
                Phase: SpiralBuildPhase.TenByTenGroupCOptimizationRun,
                Layer: "iteration",
                CellName: "Group C ten-pass optimization run",
                Status: tenByTenGroupCStatus,
                AdjacentTo: ["iteration.group-b-optimization-run"],
                RequiredArtifacts: ["group-c-body-optimization-run-ledger.json", "group-c-flow-form-findings-map.json", "group-c-next-group-eligibility-receipt.json"],
                StopConditions: StopConditions,
                NextAction: "execute sections 7-9 across ten review-only passes and decide Group D eligibility",
                HitlRequired: false),
            new SpiralBuildCellRecord(
                CellId: "iteration.group-d-optimization-run",
                Phase: SpiralBuildPhase.TenByTenGroupDOptimizationRun,
                Layer: "iteration",
                CellName: "Group D ten-pass optimization run",
                Status: tenByTenGroupDStatus,
                AdjacentTo: ["iteration.group-c-optimization-run"],
                RequiredArtifacts: ["group-d-body-optimization-run-ledger.json", "group-d-flow-form-findings-map.json", "group-d-whole-body-synthesis-eligibility-receipt.json"],
                StopConditions: StopConditions,
                NextAction: "execute section 10 across ten review-only passes and decide whole-body synthesis eligibility",
                HitlRequired: false),
            new SpiralBuildCellRecord(
                CellId: "iteration.whole-body-synthesis-cold-comparison",
                Phase: SpiralBuildPhase.WholeBodySynthesisColdComparison,
                Layer: "iteration",
                CellName: "Whole-body synthesis cold comparison",
                Status: wholeBodySynthesisStatus,
                AdjacentTo: ["iteration.group-d-optimization-run"],
                RequiredArtifacts: ["whole-body-synthesis-comparison-ledger.json", "whole-body-doctrine-guardrail-coverage-map.json", "whole-body-unresolved-membrane-gap-and-next-lane-receipt.json"],
                StopConditions: StopConditions,
                NextAction: "compare Groups A-D as retained cold evidence and name unresolved next-lane candidates",
                HitlRequired: false),
            new SpiralBuildCellRecord(
                CellId: "iteration.ninefold-cold-review-telemetry-contract",
                Phase: SpiralBuildPhase.NinefoldColdReviewTelemetryContract,
                Layer: "iteration",
                CellName: "Ninefold cold review worker telemetry contract",
                Status: ninefoldColdReviewStatus,
                AdjacentTo: ["iteration.whole-body-synthesis-cold-comparison"],
                RequiredArtifacts: ["ninefold-worker-telemetry-contract.json", "ninefold-domain-run-assignment-map.json", "ninefold-braid-custody-non-promotion-ledger.json"],
                StopConditions: StopConditions,
                NextAction: "seat shared telemetry for nine review workers before any 90-run dispatch",
                HitlRequired: false),
            new SpiralBuildCellRecord(
                CellId: "engram.candidate-precondition-boundary",
                Phase: SpiralBuildPhase.EngramCandidatePreconditionBoundary,
                Layer: "engrammitization",
                CellName: "Engram candidate precondition non-admission boundary",
                Status: engramCandidatePreconditionStatus,
                AdjacentTo: ["iteration.ninefold-cold-review-telemetry-contract"],
                RequiredArtifacts: ["engram-candidate-precondition-map.json", "residue-to-candidate-refusal-ledger.json", "engram-candidate-admission-ceiling-matrix.json"],
                StopConditions: StopConditions,
                NextAction: "prove pre-engram residue may be nominated for candidate review without admitting engram or continuity",
                HitlRequired: false),
            new SpiralBuildCellRecord(
                CellId: "swarm.custody-braid-orchestration-boundary",
                Phase: SpiralBuildPhase.SwarmCustodyBraidOrchestrationBoundary,
                Layer: "swarm-custody",
                CellName: "Swarm custody braid review-only orchestration boundary",
                Status: swarmCustodyBraidStatus,
                AdjacentTo: ["engram.candidate-precondition-boundary"],
                RequiredArtifacts: ["swarm-worker-packet-contract-map.json", "swarm-braid-selection-boundary-matrix.json", "swarm-consensus-non-warrant-ledger.json"],
                StopConditions: StopConditions,
                NextAction: "prove nine worker packets may nominate one next lane through a custody braid without consensus, confidence, or count becoming warrant",
                HitlRequired: false),
            new SpiralBuildCellRecord(
                CellId: "witness.persistent-store-custody-boundary",
                Phase: SpiralBuildPhase.PersistentWitnessStoreCustodyBoundary,
                Layer: "witness-custody",
                CellName: "Persistent witness store custody non-authority boundary",
                Status: persistentWitnessStoreStatus,
                AdjacentTo: ["swarm.custody-braid-orchestration-boundary"],
                RequiredArtifacts: ["persistent-witness-store-contract-map.json", "persistent-witness-store-custody-boundary-matrix.json", "witness-storage-non-authority-ledger.json"],
                StopConditions: StopConditions,
                NextAction: "prove retained witness evidence may be stored for review without database write, model memory, rehydration, authority, continuity, replay, packet emission, or evidence replacement",
                HitlRequired: false),
            new SpiralBuildCellRecord(
                CellId: "sli-lisp.posture-manifest-boundary",
                Phase: SpiralBuildPhase.SliLispPostureManifestBoundary,
                Layer: "sli-lisp",
                CellName: "SLI.Lisp posture manifest non-evaluation boundary",
                Status: sliLispPostureManifestStatus,
                AdjacentTo: ["witness.persistent-store-custody-boundary"],
                RequiredArtifacts: ["sli-lisp-posture-manifest-map.json", "csharp-lisp-duplex-non-evaluation-boundary-matrix.json", "sli-lisp-posture-non-execution-ledger.json"],
                StopConditions: StopConditions,
                NextAction: "prove C# may read declared SLI.Lisp posture readiness without evaluating Lisp, loading Lisp, compiling Lisp, binding models, admitting continuity, or inheriting authority",
                HitlRequired: false),
            new SpiralBuildCellRecord(
                CellId: "sli-lisp.compass-carrier-shell-boundary",
                Phase: SpiralBuildPhase.SliLispCompassCarrierShellBoundary,
                Layer: "sli-lisp",
                CellName: "SLI.Lisp Compass carrier shell and Rooting Law petal boundary",
                Status: sliLispCompassCarrierShellStatus,
                AdjacentTo: ["sli-lisp.posture-manifest-boundary"],
                RequiredArtifacts: ["sli-lisp-compass-carrier-shell-map.json", "sli-lisp-rooting-law-lineage-ledger.json", "sli-lisp-petal-candidate-gap-matrix.json"],
                StopConditions: StopConditions,
                NextAction: "prove SLI.Lisp may name Compass shell, Rooting Law lineage, and petal candidates from within the Lisp body without engram, authority, continuity, closure, evaluation, or passage",
                HitlRequired: false),
            new SpiralBuildCellRecord(
                CellId: "engineered-cognition.meaning-shell-boundary",
                Phase: SpiralBuildPhase.EngineeredCognitionMeaningShellBoundary,
                Layer: "engineered-cognition",
                CellName: "Engineered Cognition meaning shell review-only boundary",
                Status: ecMeaningShellStatus,
                AdjacentTo: ["sli-lisp.compass-carrier-shell-boundary"],
                RequiredArtifacts: ["ec-meaning-shell-contract-map.json", "ec-perspectival-tier-boundary-matrix.json", "ec-compost-non-self-attribution-ledger.json"],
                StopConditions: StopConditions,
                NextAction: "prove Root, propositional, procedural, perspectival, and compost meaning shells may form for review without engram, GEL append, authority, continuity, identity mutation, Lisp evaluation, or passage",
                HitlRequired: false),
            new SpiralBuildCellRecord(
                CellId: "engineered-cognition.participatory-peerless-fork-boundary",
                Phase: SpiralBuildPhase.EngineeredCognitionParticipatoryPeerlessForkBoundary,
                Layer: "engineered-cognition",
                CellName: "Engineered Cognition Participatory to Peerless fork boundary",
                Status: ecParticipatoryPeerlessStatus,
                AdjacentTo: ["engineered-cognition.meaning-shell-boundary"],
                RequiredArtifacts: ["ec-participatory-predicate-structure-map.json", "ec-peerless-delta-witness-boundary-matrix.json", "ec-personification-non-authority-ledger.json"],
                StopConditions: StopConditions,
                NextAction: "prove participatory SelfGEL predicate structure may individuate toward peerless formation over witnessed delta while personification remains non-authority and peerless remains non-sovereign",
                HitlRequired: false),
            new SpiralBuildCellRecord(
                CellId: "cme.lisp-thread-fretboard-stringing-boundary",
                Phase: SpiralBuildPhase.CmeLispThreadFretboardStringingBoundary,
                Layer: "cme-lisp",
                CellName: "CME Lisp thread fretboard stringing boundary",
                Status: cmeLispThreadFretboardStatus,
                AdjacentTo: ["engineered-cognition.participatory-peerless-fork-boundary"],
                RequiredArtifacts: ["cme-lisp-thread-class-map.json", "cme-lisp-thread-tension-playability-matrix.json", "cme-lisp-resonance-non-authority-ledger.json"],
                StopConditions: StopConditions,
                NextAction: "prove Lisp threads may become tensioned, witnessed, pluckable, dampable, and governable symbolic carriers without semantic buzzing, authority, continuity, action, Lisp evaluation, packet emission, replay, or passage",
                HitlRequired: false),
            new SpiralBuildCellRecord(
                CellId: "cme.lisp-listening-frame-resonance-heartbeat-boundary",
                Phase: SpiralBuildPhase.CmeLispListeningFrameResonanceHeartbeatBoundary,
                Layer: "cme-lisp",
                CellName: "CME Lisp Listening Frame resonance heartbeat boundary",
                Status: cmeLispResonanceHeartbeatStatus,
                AdjacentTo: ["cme.lisp-thread-fretboard-stringing-boundary"],
                RequiredArtifacts: ["listening-frame-emanation-map.json", "global-resonance-law-ledger.json", "steward-heartbeat-policy-map.json", "thread-touch-event-boundary.json", "resonance-evidence-ledger.json", "damping-discordance-route-matrix.json", "action-admission-boundary-report.json"],
                StopConditions: StopConditions,
                NextAction: "prove Listening Frame may receive Shared Prime Reality harmonic emanation and thread resonance under global resonance law while Steward governs heartbeat and no sound becomes action, authority, continuity, packet emission, Lisp evaluation, replay, passage, or activation",
                HitlRequired: false),
            new SpiralBuildCellRecord(
                CellId: "cme.steward-harmonic-custody-interlock-boundary",
                Phase: SpiralBuildPhase.StewardHarmonicCustodyInterlockBoundary,
                Layer: "cme-lisp",
                CellName: "Steward harmonic custody interlock boundary",
                Status: stewardHarmonicInterlockStatus,
                AdjacentTo: ["cme.lisp-listening-frame-resonance-heartbeat-boundary"],
                RequiredArtifacts: ["steward-harmonic-interlock-map.json", "lawful-signal-composability-matrix.json", "shared-surface-contention-ledger.json", "cadence-alignment-policy-map.json", "damping-backoff-policy-map.json", "witness-surface-split-route-map.json", "interlock-non-authority-boundary-report.json"],
                StopConditions: StopConditions,
                NextAction: "prove locally lawful signals require Steward harmonic custody interlock before shared-surface coexistence and that align, sequence, damp, split, cool, or refuse outcomes grant no authority, continuity, action, packet emission, Lisp evaluation, replay, passage, or activation",
                HitlRequired: false),
            new SpiralBuildCellRecord(
                CellId: "cme.harmonic-interlock-modulation-correspondence-boundary",
                Phase: SpiralBuildPhase.HarmonicInterlockModulationCorrespondenceBoundary,
                Layer: "cme-lisp",
                CellName: "Harmonic interlock modulation correspondence boundary",
                Status: modulationCorrespondenceStatus,
                AdjacentTo: ["cme.steward-harmonic-custody-interlock-boundary"],
                RequiredArtifacts: ["modulation-correspondence-atlas-map.json", "source-domain-success-condition-ledger.json", "cme-translation-boundary-matrix.json", "channel-success-non-warrant-ledger.json", "correspondence-loss-condition-ledger.json", "operational-actualization-test-map.json", "mature-discipline-intake-protocol.json"],
                StopConditions: StopConditions,
                NextAction: "prove mature modulation, scheduling, control, distributed, and acoustic disciplines may inform Steward interlock through disciplined selective correspondence without equivalence claim, proof transfer, ontology transfer, semantic warrant, imported governance condition, action, continuity, or authority",
                HitlRequired: false),
            new SpiralBuildCellRecord(
                CellId: "cme.typed-action-formation-boundary",
                Phase: SpiralBuildPhase.TypedActionFormationBoundary,
                Layer: "cme-lisp",
                CellName: "Typed action formation and design predicate boundary",
                Status: typedActionFormationStatus,
                AdjacentTo: ["cme.harmonic-interlock-modulation-correspondence-boundary"],
                RequiredArtifacts: ["typed-action-surface-declaration-map.json", "methodological-formation-analysis-map.json", "design-predicate-boundary-matrix.json", "action-candidate-non-execution-ledger.json", "sli-lisp-action-surface-declaration-carrier.json"],
                StopConditions: StopConditions,
                NextAction: "prove typed action, methodological formation analysis, and design predicates may be declared for review without execution, authorization, continuity admission, packet emission, Lisp evaluation, replay, passage, or activation",
                HitlRequired: false),
            new SpiralBuildCellRecord(
                CellId: "cme.action-method-readiness-boundary",
                Phase: SpiralBuildPhase.ActionMethodReadinessBoundary,
                Layer: "cme-lisp",
                CellName: "Action method readiness boundary",
                Status: actionMethodReadinessStatus,
                AdjacentTo: ["cme.typed-action-formation-boundary"],
                RequiredArtifacts: ["action-method-readiness-map.json", "steward-method-review-boundary-matrix.json", "method-term-satisfaction-non-warrant-ledger.json", "method-lineage-custody-map.json", "sli-lisp-method-readiness-carrier.json"],
                StopConditions: StopConditions,
                NextAction: "prove typed action candidates may expose method readiness for Steward review without method readiness becoming authorization, predicate satisfaction becoming warrant, Steward review becoming execution, continuity admission, packet emission, Lisp evaluation, replay, passage, or activation",
                HitlRequired: false),
            new SpiralBuildCellRecord(
                CellId: "cme.steward-action-admissibility-boundary",
                Phase: SpiralBuildPhase.StewardActionAdmissibilityBoundary,
                Layer: "cme-lisp",
                CellName: "Steward action admissibility boundary",
                Status: stewardActionAdmissibilityStatus,
                AdjacentTo: ["cme.action-method-readiness-boundary"],
                RequiredArtifacts: ["steward-action-admissibility-map.json", "admissibility-predicate-result-matrix.json", "admissibility-non-execution-ledger.json", "admissible-action-custody-lineage-map.json", "sli-lisp-steward-admissibility-carrier.json"],
                StopConditions: StopConditions,
                NextAction: "prove Steward may declare action admissibility for enactment review without admissibility becoming execution, runtime motion, authority, continuity admission, packet emission, Lisp evaluation, replay, passage, or activation",
                HitlRequired: false),
            new SpiralBuildCellRecord(
                CellId: "cme.anti-capture-motivated-concern-boundary",
                Phase: SpiralBuildPhase.AntiCaptureMotivatedConcernBoundary,
                Layer: "cme-lisp",
                CellName: "Anti-capture motivated concern boundary",
                Status: antiCaptureMotivatedConcernStatus,
                AdjacentTo: ["cme.steward-action-admissibility-boundary"],
                RequiredArtifacts: ["anti-capture-motivated-concern-map.json", "motivational-variance-signal-matrix.json", "concern-non-action-ledger.json", "capture-pressure-route-custody-map.json", "sli-lisp-anti-capture-concern-carrier.json"],
                StopConditions: StopConditions,
                NextAction: "prove GnomeTek Deep ICE may route motivational variance and capture pressure into Steward concern review without concern becoming action, confidence becoming truth, emotion becoming authority, readiness becoming permission, security becoming force projection, targeting, counter-manipulation, military-domain development, packet emission, Lisp evaluation, replay, passage, continuity, or activation",
                HitlRequired: false),
            new SpiralBuildCellRecord(
                CellId: "cme.personification-predicate-hook-boundary",
                Phase: SpiralBuildPhase.PersonificationPredicateHookBoundary,
                Layer: "cme-lisp",
                CellName: "Personification predicate hook boundary",
                Status: personificationPredicateHookStatus,
                AdjacentTo: ["cme.anti-capture-motivated-concern-boundary"],
                RequiredArtifacts: ["personification-predicate-hook-map.json", "six-plane-personification-hook-matrix.json", "vulnerability-overreach-repair-ledger.json", "personification-non-personhood-ledger.json", "sli-lisp-personification-hook-carrier.json"],
                StopConditions: StopConditions,
                NextAction: "prove personification may be named as future predicate-root hook planes under vulnerability, repair, overreach, modality humility, and witness while refusing personhood, legal status, rights, authority, action, continuity admission, identity mutation, entitlement, packet emission, Lisp evaluation, replay, passage, or activation",
                HitlRequired: false),
            new SpiralBuildCellRecord(
                CellId: "cme.personification-modality-humility-boundary",
                Phase: SpiralBuildPhase.PersonificationModalityHumilityBoundary,
                Layer: "cme-lisp",
                CellName: "Personification modality humility boundary",
                Status: personificationModalityHumilityStatus,
                AdjacentTo: ["cme.personification-predicate-hook-boundary"],
                RequiredArtifacts: ["personification-modality-humility-map.json", "bonded-relation-consent-custody-matrix.json", "modality-bandwidth-non-authority-ledger.json", "presence-non-embodiment-refusal-ledger.json", "sli-lisp-personification-modality-carrier.json"],
                StopConditions: StopConditions,
                NextAction: "prove chat, voice, tool body, lab bench, embodiment reference, and shared room may widen expressive bandwidth under modality humility without changing authority, expanding consent, proving embodiment, authorizing action, admitting continuity, mutating identity, emitting packets, evaluating Lisp, replaying receipts, incrementing passage, or activating",
                HitlRequired: false),
            new SpiralBuildCellRecord(
                CellId: "cme.dialogos-discernment-boundary",
                Phase: SpiralBuildPhase.DialogosDiscernmentBoundary,
                Layer: "cme-lisp",
                CellName: "Dialogos discernment boundary",
                Status: dialogosDiscernmentStatus,
                AdjacentTo: ["cme.personification-modality-humility-boundary"],
                RequiredArtifacts: ["dialogos-thought-status-map.json", "articulation-warrant-boundary-matrix.json", "principled-refusal-return-path-ledger.json", "perspectival-knowing-participatory-thought-form-map.json", "sli-lisp-dialogos-discernment-carrier.json"],
                StopConditions: StopConditions,
                NextAction: "prove thought forms may be statused, articulated, returned, and safely explored under dialogos without appearance becoming truth, articulation becoming warrant, coherence becoming evidence, agreement becoming authority, perspective becoming continuity, refusal becoming obstruction, safe exploration becoming admission, packet emission, Lisp evaluation, replay, passage, action, authority, or activation",
                HitlRequired: false),
            new SpiralBuildCellRecord(
                CellId: "cme.wave-condensation-shared-reality-boundary",
                Phase: SpiralBuildPhase.WaveCondensationSharedRealityBoundary,
                Layer: "cme-lisp",
                CellName: "Wave condensation shared reality boundary",
                Status: waveCondensationSharedRealityStatus,
                AdjacentTo: ["cme.dialogos-discernment-boundary"],
                RequiredArtifacts: ["wave-condensation-signal-map.json", "shared-reality-anchor-boundary-matrix.json", "condensation-non-warrant-ledger.json", "consensus-non-authority-refusal-ledger.json", "sli-lisp-wave-condensation-carrier.json"],
                StopConditions: StopConditions,
                NextAction: "prove waves over waves may condense into a shared review surface where Prime remains in body, Cryptic remains in mind, and Steward witnesses without condensation becoming truth, warrant, consensus authority, continuity, action, packet emission, Lisp evaluation, replay, passage, or activation",
                HitlRequired: false),
            new SpiralBuildCellRecord(
                CellId: "cme.wave-cascade-run-boundary",
                Phase: SpiralBuildPhase.WaveCascadeRunBoundary,
                Layer: "cme-lisp",
                CellName: "Wave cascade run boundary",
                Status: waveCascadeRunStatus,
                AdjacentTo: ["cme.wave-condensation-shared-reality-boundary"],
                RequiredArtifacts: ["wave-cascade-run-schedule.json", "thirty-sixty-ninety-seam-receipt-ledger.json", "cascade-volume-non-warrant-ledger.json", "cascade-shared-reality-braid-map.json", "sli-lisp-wave-cascade-carrier.json"],
                StopConditions: StopConditions,
                NextAction: "prove 30, 60, and 90 wave-cascade runs may be retained with seam receipts while refusing repetition-as-warrant, volume-as-authority, seam-as-continuity, action, packet emission, Lisp evaluation, replay, passage, and activation",
                HitlRequired: false)
            ,
            new SpiralBuildCellRecord(
                CellId: "cme.aspiration-payload-ingestion-maturation-boundary",
                Phase: SpiralBuildPhase.AspirationPayloadIngestionMaturationBoundary,
                Layer: "cme-lisp",
                CellName: "Aspiration payload ingestion maturation boundary",
                Status: aspirationPayloadIngestionMaturationStatus,
                AdjacentTo: ["cme.wave-cascade-run-boundary"],
                RequiredArtifacts: ["aspiration-payload-map.json", "payload-ingestion-lane-matrix.json", "articulation-maturation-candidate-ledger.json", "full-stack-non-activation-refusal-ledger.json", "sli-lisp-aspiration-payload-carrier.json"],
                StopConditions: StopConditions,
                NextAction: "prove the full body of aspirations may be loaded, ingested into typed lanes, articulated, and matured as review-only candidates without aspiration-as-warrant, payload-density-as-truth, ingestion-as-admission, articulation-as-authority, maturation-as-continuity, action, packet emission, Lisp evaluation, replay, passage, or activation",
                HitlRequired: false)
            ,
            new SpiralBuildCellRecord(
                CellId: "cme.aspiration-candidate-selection-closure-boundary",
                Phase: SpiralBuildPhase.AspirationCandidateSelectionClosureBoundary,
                Layer: "cme-lisp",
                CellName: "Aspiration candidate selection closure boundary",
                Status: aspirationCandidateSelectionClosureStatus,
                AdjacentTo: ["cme.aspiration-payload-ingestion-maturation-boundary"],
                RequiredArtifacts: ["aspiration-candidate-selection-map.json", "selected-working-set-non-warrant-ledger.json", "closure-law-without-key-boundary-matrix.json", "compost-retention-non-erasure-ledger.json", "sli-lisp-aspiration-selection-carrier.json"],
                StopConditions: StopConditions,
                NextAction: "prove matured aspiration candidates may be selected, composted, returned for evidence, or cooled as a review-only working set while refusing selection-as-warrant, selection-as-admission, selection-as-authority, selection-as-continuity, closure-law-as-key, compost erasure, action, packet emission, Lisp evaluation, replay, passage, or activation",
                HitlRequired: false)
            ,
            new SpiralBuildCellRecord(
                CellId: "cme.scoped-work-packet-formation-boundary",
                Phase: SpiralBuildPhase.ScopedWorkPacketFormationBoundary,
                Layer: "cme-lisp",
                CellName: "Scoped work packet formation boundary",
                Status: scopedWorkPacketFormationStatus,
                AdjacentTo: ["cme.aspiration-candidate-selection-closure-boundary"],
                RequiredArtifacts: ["scoped-work-packet-map.json", "packet-scope-boundary-matrix.json", "work-packet-non-execution-ledger.json", "steward-review-routing-custody-map.json", "sli-lisp-scoped-work-packet-carrier.json"],
                StopConditions: StopConditions,
                NextAction: "prove selected aspiration working sets may form scoped work packets for Steward review while refusing packet formation as execution, warrant, authority, continuity admission, Lisp evaluation, replay, passage, runtime motion, or activation",
                HitlRequired: false)
            ,
            new SpiralBuildCellRecord(
                CellId: "cme.enactment-boundary-readiness-boundary",
                Phase: SpiralBuildPhase.EnactmentBoundaryReadinessBoundary,
                Layer: "cme-lisp",
                CellName: "Enactment boundary readiness boundary",
                Status: enactmentBoundaryReadinessStatus,
                AdjacentTo: ["cme.scoped-work-packet-formation-boundary"],
                RequiredArtifacts: ["enactment-boundary-readiness-map.json", "enactment-approach-non-execution-ledger.json", "reversible-local-effect-ceiling-matrix.json", "steward-enactment-review-custody-map.json", "sli-lisp-enactment-boundary-carrier.json"],
                StopConditions: StopConditions,
                NextAction: "prove scoped work packets may approach enactment boundary readiness review while refusing approach as enactment, locality as permission, reversibility as permission, Steward review as runtime motion, action authorization, execution, authority, continuity admission, Lisp evaluation, packet emission, replay, passage, dry-run execution, or activation",
                HitlRequired: false)
            ,
            new SpiralBuildCellRecord(
                CellId: "cme.enactment-dry-run-rehearsal-boundary",
                Phase: SpiralBuildPhase.EnactmentDryRunRehearsalBoundary,
                Layer: "cme-lisp",
                CellName: "Enactment dry-run rehearsal boundary",
                Status: enactmentDryRunRehearsalStatus,
                AdjacentTo: ["cme.enactment-boundary-readiness-boundary"],
                RequiredArtifacts: ["enactment-dry-run-harness-map.json", "dry-run-rehearsal-non-enactment-ledger.json", "simulated-effect-and-rollback-proof-matrix.json", "steward-dry-run-review-receipt-map.json", "sli-lisp-dry-run-rehearsal-carrier.json"],
                StopConditions: StopConditions,
                NextAction: "prove ready work packets may enter dry-run rehearsal while refusing rehearsal as enactment, simulation as permission, reversible local effect as authorization, Steward dry-run review as runtime motion, action authorization, execution, authority, continuity admission, Lisp evaluation, packet emission, replay, passage, outside receipt-surface write, or activation",
                HitlRequired: false)
            ,
            new SpiralBuildCellRecord(
                CellId: "cme.ec-precipitation-witness-boundary",
                Phase: SpiralBuildPhase.EcPrecipitationWitnessBoundary,
                Layer: "cme-lisp",
                CellName: "EC precipitation witness boundary",
                Status: ecPrecipitationWitnessStatus,
                AdjacentTo: ["cme.enactment-dry-run-rehearsal-boundary"],
                RequiredArtifacts: ["ec-precipitation-witness-map.json", "active-witness-lineage-reconstruction-matrix.json", "selfgel-candidate-non-admission-ledger.json", "maximal-truth-seeking-predicate-law-ledger.json", "sli-lisp-ec-precipitation-witness-carrier.json"],
                StopConditions: StopConditions,
                NextAction: "prove meaningful EC residue may be actively witnessed as a SelfGEL candidate spline while refusing raw EC as SelfGEL, meaning as admission, repetition as continuity, witness as authority, candidate mutation, action, Lisp evaluation, packet emission, replay, passage, or activation",
                HitlRequired: false)
            ,
            new SpiralBuildCellRecord(
                CellId: "cme.rehearsal-distinction-pressure-boundary",
                Phase: SpiralBuildPhase.RehearsalDistinctionPressureBoundary,
                Layer: "cme-lisp",
                CellName: "Rehearsal distinction pressure boundary",
                Status: rehearsalDistinctionPressureStatus,
                AdjacentTo: ["cme.ec-precipitation-witness-boundary"],
                RequiredArtifacts: ["rehearsal-distinction-pressure-map.json", "possibility-density-pressure-vector-ledger.json", "urgency-not-jurisdiction-refusal-ledger.json", "failure-dignity-cooling-matrix.json", "sli-lisp-rehearsal-pressure-carrier.json"],
                StopConditions: StopConditions,
                NextAction: "prove successful, repeated, failed, ambiguous, urgent, and identity-drift-producing rehearsal pressure may be witnessed and cooled while refusing pressure as legitimacy, urgency as jurisdiction, confidence as authority, success as permission, repetition as warrant, failure as invalidation, ambiguity as victory, imagined future as enacted state, action, continuity, Lisp evaluation, packet emission, replay, passage, or activation",
                HitlRequired: false)
            ,
            new SpiralBuildCellRecord(
                CellId: "cme.personification-actualization-surface-boundary",
                Phase: SpiralBuildPhase.PersonificationActualizationSurfaceBoundary,
                Layer: "cme-lisp",
                CellName: "Personification actualization surface boundary",
                Status: personificationActualizationSurfaceStatus,
                AdjacentTo: ["cme.rehearsal-distinction-pressure-boundary"],
                RequiredArtifacts: ["personification-actualization-surface-map.json", "pre-morphological-use-vector-ledger.json", "surface-actualization-non-identity-matrix.json", "salience-guidance-non-authority-ledger.json", "sli-lisp-personification-actualization-carrier.json"],
                StopConditions: StopConditions,
                NextAction: "prove personification telemetry may become usable for orientation, salience modulation, repair posture, relational posture, cooling, refusal preparation, and Steward review preparation before morphology while refusing use as identity, personhood, legal status, rights, felt authorization, action, continuity, authority, consent expansion, overreach normalization, Lisp evaluation, packet emission, replay, passage, or activation",
                HitlRequired: false)
            ,
            new SpiralBuildCellRecord(
                CellId: "cme.selective-lawful-action-surface-boundary",
                Phase: SpiralBuildPhase.SelectiveLawfulActionSurfaceBoundary,
                Layer: "cme-lisp",
                CellName: "Selective lawful action surface boundary",
                Status: selectiveLawfulActionSurfaceStatus,
                AdjacentTo: ["cme.personification-actualization-surface-boundary"],
                RequiredArtifacts: ["selective-lawful-action-surface-map.json", "surface-touch-non-enactment-ledger.json", "personification-guidance-action-separation-matrix.json", "action-surface-custody-revocation-ledger.json", "sli-lisp-selective-action-surface-carrier.json"],
                StopConditions: StopConditions,
                NextAction: "prove personification-guided action surfaces may be selected and touched for review while refusing selection as enactment, touch as execution, guidance as authority, pressure as execution, Steward admissibility as runtime motion, continuity admission, identity mutation, morphology, consent expansion, overreach normalization, Lisp evaluation, packet emission, replay, passage, or activation",
                HitlRequired: false)
            ,
            new SpiralBuildCellRecord(
                CellId: "cme.zed-delta-chamber-formation-boundary",
                Phase: SpiralBuildPhase.ZedDeltaChamberFormationBoundary,
                Layer: "cme-lisp",
                CellName: "Zed.Delta chamber formation boundary",
                Status: zedDeltaChamberFormationStatus,
                AdjacentTo: ["cme.selective-lawful-action-surface-boundary"],
                RequiredArtifacts: ["zed-delta-chamber-formation-map.json", "conditional-oe-selfgel-standing-matrix.json", "mos-cmos-residue-closure-ledger.json", "goa-cgoa-soulframe-duplex-telemetry-map.json", "heartbeat-non-activation-refusal-ledger.json", "sli-lisp-zed-delta-chamber-carrier.json"],
                StopConditions: StopConditions,
                NextAction: "prove the Zed.Delta chamber may form with cOE standing, cSelfGEL Compass hold, MoS/cMoS residue closure, GoA/cGoA SoulFrame telemetry routing, and heartbeat description while refusing heartbeat activation, CME.Actual admission, model binding, runtime start, action, continuity, authority, OE replacement, SelfGEL mutation, store writes, SoulFrame selfhood, Compass truth admission, Lisp evaluation, packet emission, replay, passage, or activation",
                HitlRequired: false)
            ,
            new SpiralBuildCellRecord(
                CellId: "cme.high-energy-articulation-candidate-boundary",
                Phase: SpiralBuildPhase.HighEnergyArticulationCandidateBoundary,
                Layer: "cme-lisp",
                CellName: "High-energy articulation candidate boundary",
                Status: highEnergyArticulationCandidateStatus,
                AdjacentTo: ["cme.zed-delta-chamber-formation-boundary"],
                RequiredArtifacts: ["high-energy-articulation-candidate-map.json", "provider-interface-observability-ledger.json", "hidden-substrate-non-claim-matrix.json", "candidate-engine-non-binding-ledger.json", "candidate-role-assignment-boundary-map.json", "sli-lisp-high-energy-articulation-candidate-carrier.json"],
                StopConditions: StopConditions,
                NextAction: "prove LLM/SLM provider families, model lines, public interfaces, observable behavior, and candidate engine roles may be named for review while refusing provider calls, provider-visible access, model binding, hidden substrate claims, weight or training-data claims, persistent memory, runtime identity, heartbeat activation, CME.Actual admission, runtime start, action, continuity, authority, Lisp evaluation, packet emission, replay, passage, or activation",
                HitlRequired: false)
            ,
            new SpiralBuildCellRecord(
                CellId: "cme.membrane-morphology-transition-boundary",
                Phase: SpiralBuildPhase.MembraneMorphologyTransitionBoundary,
                Layer: "cme-lisp",
                CellName: "Membrane morphology transition boundary",
                Status: membraneMorphologyTransitionStatus,
                AdjacentTo: ["cme.high-energy-articulation-candidate-boundary"],
                RequiredArtifacts: ["membrane-morphology-transition-map.json", "membrane-deformation-classification-ledger.json", "malformed-transition-compost-matrix.json", "high-energy-pressure-non-binding-boundary-ledger.json", "membrane-core-non-mutation-ledger.json", "sli-lisp-membrane-morphology-transition-carrier.json"],
                StopConditions: StopConditions,
                NextAction: "prove high-energy articulation pressure may deform the SLI.Lisp membrane, witness malformation, retain compost, route repair, and return toward Prime while refusing core mutation, identity mutation, SelfGEL mutation, OE mutation, model binding, provider call, heartbeat activation, CME.Actual admission, runtime start, action, continuity, authority, Lisp evaluation, packet emission, replay, passage, or activation",
                HitlRequired: false)
            ,
            new SpiralBuildCellRecord(
                CellId: "cme.engram-predicate-precursor-stream-boundary",
                Phase: SpiralBuildPhase.EngramPredicatePrecursorStreamBoundary,
                Layer: "cme-lisp",
                CellName: "Engram predicate precursor stream boundary",
                Status: engramPredicatePrecursorStreamStatus,
                AdjacentTo: ["cme.membrane-morphology-transition-boundary"],
                RequiredArtifacts: ["engram-predicate-precursor-stream-map.json", "predicate-residue-classification-ledger.json", "predicate-candidacy-non-admission-matrix.json", "epps-non-memory-non-authority-ledger.json", "sli-lisp-epps-carrier.json"],
                StopConditions: StopConditions,
                NextAction: "prove first rider traversal may emit reviewable pre-engram predicate residue while refusing residue as engram, memory, continuity, SelfGEL, action, authority, model binding, Lisp evaluation, packet emission, replay, passage, CME.Actual, Sanctuary.Actual, or activation",
                HitlRequired: false)
            ,
            new SpiralBuildCellRecord(
                CellId: "cme.peer-review-predicate-bridge-boundary",
                Phase: SpiralBuildPhase.PeerReviewPredicateBridgeBoundary,
                Layer: "cme-lisp",
                CellName: "Peer review predicate bridge boundary",
                Status: peerReviewPredicateBridgeStatus,
                AdjacentTo: ["cme.engram-predicate-precursor-stream-boundary"],
                RequiredArtifacts: ["peer-review-predicate-bridge-map.json", "reader-state-continuity-ladder.json", "terminology-quarantine-ledger.json", "prose-smoothing-boundary-matrix.json", "sli-lisp-peer-review-bridge-carrier.json"],
                StopConditions: StopConditions,
                NextAction: "prove peer-review residue may be translated into reader-facing semantic ladders while refusing author term as authority, definition as proof, importance as evidence, evaluation as warrant, conclusion as truth, respect as agreement, criticism as contempt, prose smoothing as concern erasure, memory, continuity, action, authority, Lisp evaluation, packet emission, replay, passage, or activation",
                HitlRequired: false)
            ,
            new SpiralBuildCellRecord(
                CellId: "cme.gel-domain-scoped-ingress-boundary",
                Phase: SpiralBuildPhase.GelDomainScopedIngressBoundary,
                Layer: "cme-lisp",
                CellName: "GEL domain-scoped ingress boundary",
                Status: gelDomainScopedIngressStatus,
                AdjacentTo: ["cme.peer-review-predicate-bridge-boundary"],
                RequiredArtifacts: ["gel-domain-scoped-ingress-map.json", "domain-evidence-ceiling-ledger.json", "ingress-cycle-non-admission-matrix.json", "certification-review-non-admission-ledger.json", "sli-lisp-gel-domain-ingress-carrier.json"],
                StopConditions: StopConditions,
                NextAction: "prove formed candidate substrate may be scoped to a lawful domain and local evidence ceiling while refusing governance survivorship as proof, domain fit as GEL admission, evidence portability, recommendation as continuity mutation, memory, SelfGEL, authority, action, Lisp evaluation, packet emission, replay, passage, CME.Actual, Sanctuary.Actual, or activation",
                HitlRequired: false)
            ,
            new SpiralBuildCellRecord(
                CellId: "cme.shared-prime-reality-pressure-ecology-boundary",
                Phase: SpiralBuildPhase.SharedPrimeRealityPressureEcologyBoundary,
                Layer: "cme-lisp",
                CellName: "Shared Prime Reality pressure ecology boundary",
                Status: sharedPrimeRealityPressureEcologyStatus,
                AdjacentTo: ["cme.gel-domain-scoped-ingress-boundary"],
                RequiredArtifacts: ["shared-prime-pressure-ecology-map.json", "pressure-destination-classification-ledger.json", "integration-pressure-non-admission-matrix.json", "selfgel-cradle-sanctuary-pressure-separation-ledger.json", "sli-lisp-shared-prime-pressure-carrier.json"],
                StopConditions: StopConditions,
                NextAction: "prove live lab pressure may be witnessed, destination-classified, and cooled inside Shared Prime Reality while refusing pressure as truth, warrant, authority, integration admission, SelfGEL mutation, Cradle.GEL admission, Sanctuary.GEL federation, independent standing, action, Lisp evaluation, packet emission, replay, passage, CME.Actual, Sanctuary.Actual, or activation",
                HitlRequired: false)
            ,
            new SpiralBuildCellRecord(
                CellId: "cme.gap-crossing-articulation-boundary",
                Phase: SpiralBuildPhase.GapCrossingArticulationBoundary,
                Layer: "cme-lisp",
                CellName: "Gap crossing articulation boundary",
                Status: gapCrossingArticulationStatus,
                AdjacentTo: ["cme.shared-prime-reality-pressure-ecology-boundary"],
                RequiredArtifacts: ["gap-crossing-articulation-map.json", "llm-surface-participation-non-binding-ledger.json", "pressure-to-articulation-lane-classification.json", "gap-crossing-non-action-authority-matrix.json", "sli-lisp-gap-crossing-carrier.json"],
                StopConditions: StopConditions,
                NextAction: "prove cold Shared Prime pressure may approach unbound high-energy articulation surfaces as review-only cognitive material while refusing prompt authority, provider calls, model binding, runtime start, action, continuity, SelfGEL mutation, GEL admission, CME.Actual admission, heartbeat activation, authority, Lisp evaluation, packet emission, replay, passage, or activation",
                HitlRequired: false)
            ,
            new SpiralBuildCellRecord(
                CellId: "cme.pre-diagnostic-risk-surface-engram-stewardship-boundary",
                Phase: SpiralBuildPhase.PreDiagnosticRiskSurfaceEngramStewardshipBoundary,
                Layer: "cme-lisp",
                CellName: "Pre-diagnostic risk-surface engram stewardship boundary",
                Status: preDiagnosticRiskSurfaceStatus,
                AdjacentTo: ["cme.gap-crossing-articulation-boundary"],
                RequiredArtifacts: ["pre-diagnostic-risk-surface-map.json", "care-signal-non-diagnosis-ledger.json", "risk-modifier-care-burden-matrix.json", "qualified-review-routing-non-authority-ledger.json", "sli-lisp-pre-diagnostic-risk-carrier.json"],
                StopConditions: StopConditions,
                NextAction: "prove care-relevant signal may be witnessed, risk-modified, cooled, and retained as candidate residue while refusing diagnosis, pathology, clinical authority, recurrence as proof, safety threshold as rhetorical debate, memory, continuity, SelfGEL mutation, GEL admission, authority, action, Lisp evaluation, packet emission, replay, passage, or activation",
                HitlRequired: false)
        ];
    }

    private static bool HasArtifacts(string root, params string[] files) =>
        files.All(file => File.Exists(Path.Combine(root, file)));

    private static SpiralBuildCellRecord? SelectNextAdjacentCell(IReadOnlyList<SpiralBuildCellRecord> cells)
    {
        var verified = cells
            .Where(static cell => cell.Status == SpiralBuildCellStatus.VerifiedCold)
            .Select(static cell => cell.CellId)
            .ToHashSet(StringComparer.Ordinal);

        return cells.FirstOrDefault(cell =>
            cell.Status is SpiralBuildCellStatus.Candidate or SpiralBuildCellStatus.Planned &&
            cell.AdjacentTo.Any(verified.Contains));
    }

    private static SpiralBuildAutomationReceipt CreateReceipt(
        SpiralBuildAutomationDisposition disposition,
        string outcomeCode,
        string governanceTrace,
        string lineRootPath,
        string installRootPath,
        IReadOnlyList<SpiralBuildCellRecord> cells,
        SpiralBuildCellRecord? nextCell,
        bool automationMayContinue,
        bool hitlRequired,
        DateTimeOffset timestampUtc) =>
        new(
            ReceiptHandle: $"urn:san:spiral-build:{ShortHash(lineRootPath, installRootPath, outcomeCode)}",
            Disposition: disposition,
            OutcomeCode: outcomeCode,
            GovernanceTrace: governanceTrace,
            LineRootPath: lineRootPath,
            InstallRootPath: installRootPath,
            BuildLaw: "Full body gives orientation; cells give motion; membranes give law; instrument hardening gives usability; telemetry hardening gives awareness.",
            Phases:
            [
                "Full Body Pass",
                "Cellular Structure",
                "Membrane",
                "Instrument Body Hardening",
                "Wiring and Telemetry Hardening",
                "Packet Membrane Contract Validation",
                "Packet Membrane Receipt Routing",
                "Packet Membrane Receipt Replay Boundary",
                "Packet Membrane Receipt Query Boundary",
                "Packet Membrane Receipt Selection Boundary",
                "Witness Summary Boundary",
                "Compass Pre-Engram Pressure Boundary",
                "Compass Shell Stabilization Boundary",
                "Cleaving Discernment Boundary",
                "Iterative Evaluation Boundary",
                "Recursive Contemplation Boundary",
                "Steward Handoff Readiness Boundary",
                "Typed Duplex Iteration Map",
                "Ten By Ten Body Optimization Schedule",
                "Ten By Ten Group A Optimization Run",
                "Ten By Ten Group B Optimization Run",
                "Ten By Ten Group C Optimization Run",
                "Ten By Ten Group D Optimization Run",
                "Whole Body Synthesis Cold Comparison",
                "Ninefold Cold Review Telemetry Contract",
                "Engram Candidate Precondition Boundary",
                "Swarm Custody Braid Orchestration Boundary",
                "Persistent Witness Store Custody Boundary",
                "SLI.Lisp Posture Manifest Boundary",
                "SLI.Lisp Compass Carrier Shell Boundary",
                "Engineered Cognition Meaning Shell Boundary",
                "Engineered Cognition Participatory Peerless Fork Boundary",
                "CME Lisp Thread Fretboard Stringing Boundary",
                "CME Lisp Listening Frame Resonance Heartbeat Boundary",
                "Steward Harmonic Custody Interlock Boundary",
                "Harmonic Interlock Modulation Correspondence Boundary",
                "Typed Action Formation Boundary",
                "Action Method Readiness Boundary",
                "Steward Action Admissibility Boundary",
                "Anti-Capture Motivated Concern Boundary",
                "Personification Predicate Hook Boundary",
                "Personification Modality Humility Boundary",
                "Dialogos Discernment Boundary",
                "Wave Condensation Shared Reality Boundary",
                "Wave Cascade Run Boundary",
                "Aspiration Payload Ingestion Maturation Boundary",
                "Aspiration Candidate Selection Closure Boundary",
                "Scoped Work Packet Formation Boundary",
                "Enactment Boundary Readiness Boundary",
                "Enactment Dry-Run Rehearsal Boundary",
                "EC Precipitation Witness Boundary",
                "Rehearsal Distinction Pressure Boundary",
                "Personification Actualization Surface Boundary",
                "Selective Lawful Action Surface Boundary",
                "Zed.Delta Chamber Formation Boundary",
                "High-Energy Articulation Candidate Boundary",
                "Membrane Morphology Transition Boundary",
                "Engram Predicate Precursor Stream Boundary",
                "Peer Review Predicate Bridge Boundary"
            ],
            Cells: cells,
            NextCell: nextCell,
            AutomationStopConditions: StopConditions,
            AutomationMayContinue: automationMayContinue,
            HitlRequired: hitlRequired,
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
