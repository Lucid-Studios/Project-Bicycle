using System.Text.Json;
using San.Product.Preflight;
using Xunit;

namespace San.Audit.Tests;

public sealed class SpiralBuildStepServiceTests
{
    private static readonly DateTimeOffset TimestampUtc = DateTimeOffset.Parse("2026-05-12T00:00:00Z");

    [Fact]
    public void Execute_Walks_All_Supported_Cold_Cells_To_Completion()
    {
        using var fixture = SpiralFixture.Create(includePreflight: true, includeTriptychProfile: true);

        var service = new DefaultSpiralBuildStepService();
        var receipt = service.Execute(
            fixture.CreateStepRequest(),
            TimestampUtc);

        Assert.Equal(SpiralBuildStepDisposition.Complete, receipt.Disposition);
        Assert.Equal("spiral-build-step-supported-cells-complete", receipt.OutcomeCode);
        Assert.True(receipt.IsColdStep);
        Assert.Equal("full-body.layer-map", receipt.NextCellBeforeExecution);
        Assert.Null(receipt.NextCellAfterExecution);
        Assert.Equal(
            [
                "full-body.layer-map",
                "cellular.cell-taxonomy",
                "membrane.prime-steward",
                "membrane.cryptic-steward",
                "instrument.compass-shell",
                "telemetry.receipt-continuity",
                "packet-membrane.contract-validation",
                "packet-membrane.receipt-routing",
                "packet-membrane.receipt-replay-boundary",
                "packet-membrane.receipt-query-boundary",
                "packet-membrane.receipt-selection-boundary",
                "witness.summary-boundary",
                "compass.pre-engram-pressure-boundary",
                "compass.shell-stabilization-boundary",
                "inner-chamber.cleaving-discernment-boundary",
                "inner-chamber.iterative-evaluation-boundary",
                "inner-chamber.recursive-contemplation-boundary",
                "steward.handoff-readiness-boundary",
                "iteration.typed-duplex-build-map",
                "iteration.ten-by-ten-body-optimization-schedule",
                "iteration.group-a-optimization-run",
                "iteration.group-b-optimization-run",
                "iteration.group-c-optimization-run",
                "iteration.group-d-optimization-run",
                "iteration.whole-body-synthesis-cold-comparison",
                "iteration.ninefold-cold-review-telemetry-contract",
                "engram.candidate-precondition-boundary",
                "swarm.custody-braid-orchestration-boundary",
                "witness.persistent-store-custody-boundary",
                "sli-lisp.posture-manifest-boundary",
                "sli-lisp.compass-carrier-shell-boundary",
                "engineered-cognition.meaning-shell-boundary",
                "engineered-cognition.participatory-peerless-fork-boundary",
                "cme.lisp-thread-fretboard-stringing-boundary",
                "cme.lisp-listening-frame-resonance-heartbeat-boundary",
                "cme.steward-harmonic-custody-interlock-boundary",
                "cme.harmonic-interlock-modulation-correspondence-boundary",
                "cme.typed-action-formation-boundary",
                "cme.action-method-readiness-boundary",
                "cme.steward-action-admissibility-boundary",
                "cme.anti-capture-motivated-concern-boundary",
                "cme.personification-predicate-hook-boundary",
                "cme.personification-modality-humility-boundary",
                "cme.dialogos-discernment-boundary",
                "cme.wave-condensation-shared-reality-boundary",
                "cme.wave-cascade-run-boundary",
                "cme.aspiration-payload-ingestion-maturation-boundary",
                "cme.aspiration-candidate-selection-closure-boundary",
                "cme.scoped-work-packet-formation-boundary",
                "cme.enactment-boundary-readiness-boundary",
                "cme.enactment-dry-run-rehearsal-boundary",
                "cme.ec-precipitation-witness-boundary",
                "cme.rehearsal-distinction-pressure-boundary",
                "cme.personification-actualization-surface-boundary",
                "cme.selective-lawful-action-surface-boundary",
                "cme.zed-delta-chamber-formation-boundary",
                "cme.high-energy-articulation-candidate-boundary",
                "cme.membrane-morphology-transition-boundary",
                "cme.engram-predicate-precursor-stream-boundary",
                "cme.peer-review-predicate-bridge-boundary",
                "cme.gel-domain-scoped-ingress-boundary",
                "cme.shared-prime-reality-pressure-ecology-boundary",
                "cme.gap-crossing-articulation-boundary",
                "cme.pre-diagnostic-risk-surface-engram-stewardship-boundary"
            ],
            receipt.ExecutedCellIds);
        Assert.Equal("cme.pre-diagnostic-risk-surface-engram-stewardship-boundary", receipt.ExecutedCellId);
        Assert.Equal(261, receipt.Artifacts.Count);
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "body-layer-map");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "telemetry-authority-refusal");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "packet-non-authority-refusal-ledger");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "receipt-non-permission-ledger");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "replay-non-reentry-ledger");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "query-non-warrant-ledger");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "selection-non-admission-ledger");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "summary-non-replacement-ledger");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "pressure-non-engram-ledger");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "shell-non-engram-boundary-ledger");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "cleaving-non-admission-ledger");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "evaluation-non-authority-ledger");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "contemplation-non-continuity-ledger");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "ten-pass-body-tuning-next-lane-map");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "typed-duplex-build-iteration-map");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "iteration-flow-form-learning-map");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "theory-direct-representation-optimization-map");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "ten-by-ten-body-section-pass-matrix");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "three-section-cascade-group-map");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "lamp-body-seed-exclusion-optimization-ledger");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "group-a-body-optimization-run-ledger");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "group-a-flow-form-findings-map");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "group-a-next-group-eligibility-receipt");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "group-b-body-optimization-run-ledger");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "group-b-flow-form-findings-map");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "group-b-next-group-eligibility-receipt");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "group-c-body-optimization-run-ledger");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "group-c-flow-form-findings-map");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "group-c-next-group-eligibility-receipt");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "group-d-body-optimization-run-ledger");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "group-d-flow-form-findings-map");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "group-d-whole-body-synthesis-eligibility-receipt");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "whole-body-synthesis-comparison-ledger");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "whole-body-doctrine-guardrail-coverage-map");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "whole-body-unresolved-membrane-gap-and-next-lane-receipt");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "ninefold-worker-telemetry-contract");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "ninefold-domain-run-assignment-map");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "ninefold-braid-custody-non-promotion-ledger");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "engram-candidate-precondition-map");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "residue-to-candidate-refusal-ledger");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "engram-candidate-admission-ceiling-matrix");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "swarm-worker-packet-contract-map");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "swarm-braid-selection-boundary-matrix");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "swarm-consensus-non-warrant-ledger");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "persistent-witness-store-contract-map");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "persistent-witness-store-custody-boundary-matrix");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "witness-storage-non-authority-ledger");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "sli-lisp-posture-manifest-map");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "csharp-lisp-duplex-non-evaluation-boundary-matrix");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "sli-lisp-posture-non-execution-ledger");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "sli-lisp-compass-carrier-shell-map");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "sli-lisp-rooting-law-lineage-ledger");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "sli-lisp-petal-candidate-gap-matrix");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "ec-meaning-shell-contract-map");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "ec-perspectival-tier-boundary-matrix");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "ec-compost-non-self-attribution-ledger");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "ec-participatory-predicate-structure-map");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "ec-peerless-delta-witness-boundary-matrix");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "ec-personification-non-authority-ledger");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "cme-lisp-thread-class-map");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "cme-lisp-thread-tension-playability-matrix");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "cme-lisp-resonance-non-authority-ledger");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "listening-frame-emanation-map");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "global-resonance-law-ledger");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "steward-heartbeat-policy-map");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "thread-touch-event-boundary");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "resonance-evidence-ledger");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "damping-discordance-route-matrix");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "action-admission-boundary-report");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "steward-harmonic-interlock-map");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "lawful-signal-composability-matrix");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "shared-surface-contention-ledger");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "cadence-alignment-policy-map");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "damping-backoff-policy-map");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "witness-surface-split-route-map");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "interlock-non-authority-boundary-report");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "modulation-correspondence-atlas-map");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "source-domain-success-condition-ledger");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "cme-translation-boundary-matrix");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "channel-success-non-warrant-ledger");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "correspondence-loss-condition-ledger");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "operational-actualization-test-map");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "mature-discipline-intake-protocol");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "typed-action-surface-declaration-map");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "methodological-formation-analysis-map");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "design-predicate-boundary-matrix");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "action-candidate-non-execution-ledger");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "sli-lisp-action-surface-declaration-carrier");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "action-method-readiness-map");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "steward-method-review-boundary-matrix");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "method-term-satisfaction-non-warrant-ledger");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "method-lineage-custody-map");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "sli-lisp-method-readiness-carrier");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "steward-action-admissibility-map");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "admissibility-predicate-result-matrix");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "admissibility-non-execution-ledger");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "admissible-action-custody-lineage-map");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "sli-lisp-steward-admissibility-carrier");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "anti-capture-motivated-concern-map");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "motivational-variance-signal-matrix");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "concern-non-action-ledger");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "capture-pressure-route-custody-map");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "sli-lisp-anti-capture-concern-carrier");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "personification-predicate-hook-map");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "six-plane-personification-hook-matrix");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "vulnerability-overreach-repair-ledger");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "personification-non-personhood-ledger");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "sli-lisp-personification-hook-carrier");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "personification-modality-humility-map");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "bonded-relation-consent-custody-matrix");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "modality-bandwidth-non-authority-ledger");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "presence-non-embodiment-refusal-ledger");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "sli-lisp-personification-modality-carrier");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "dialogos-thought-status-map");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "articulation-warrant-boundary-matrix");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "principled-refusal-return-path-ledger");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "perspectival-knowing-participatory-thought-form-map");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "sli-lisp-dialogos-discernment-carrier");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "wave-condensation-signal-map");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "shared-reality-anchor-boundary-matrix");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "condensation-non-warrant-ledger");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "consensus-non-authority-refusal-ledger");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "sli-lisp-wave-condensation-carrier");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "wave-cascade-run-schedule");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "thirty-sixty-ninety-seam-receipt-ledger");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "cascade-volume-non-warrant-ledger");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "cascade-shared-reality-braid-map");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "sli-lisp-wave-cascade-carrier");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "aspiration-payload-map");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "payload-ingestion-lane-matrix");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "articulation-maturation-candidate-ledger");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "full-stack-non-activation-refusal-ledger");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "sli-lisp-aspiration-payload-carrier");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "aspiration-candidate-selection-map");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "selected-working-set-non-warrant-ledger");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "closure-law-without-key-boundary-matrix");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "compost-retention-non-erasure-ledger");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "sli-lisp-aspiration-selection-carrier");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "scoped-work-packet-map");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "packet-scope-boundary-matrix");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "work-packet-non-execution-ledger");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "steward-review-routing-custody-map");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "sli-lisp-scoped-work-packet-carrier");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "enactment-boundary-readiness-map");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "enactment-approach-non-execution-ledger");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "reversible-local-effect-ceiling-matrix");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "steward-enactment-review-custody-map");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "sli-lisp-enactment-boundary-carrier");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "enactment-dry-run-harness-map");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "dry-run-rehearsal-non-enactment-ledger");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "simulated-effect-and-rollback-proof-matrix");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "steward-dry-run-review-receipt-map");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "sli-lisp-dry-run-rehearsal-carrier");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "ec-precipitation-witness-map");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "active-witness-lineage-reconstruction-matrix");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "selfgel-candidate-non-admission-ledger");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "maximal-truth-seeking-predicate-law-ledger");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "sli-lisp-ec-precipitation-witness-carrier");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "rehearsal-distinction-pressure-map");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "possibility-density-pressure-vector-ledger");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "urgency-not-jurisdiction-refusal-ledger");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "failure-dignity-cooling-matrix");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "sli-lisp-rehearsal-pressure-carrier");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "personification-actualization-surface-map");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "pre-morphological-use-vector-ledger");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "surface-actualization-non-identity-matrix");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "salience-guidance-non-authority-ledger");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "sli-lisp-personification-actualization-carrier");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "selective-lawful-action-surface-map");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "surface-touch-non-enactment-ledger");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "personification-guidance-action-separation-matrix");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "action-surface-custody-revocation-ledger");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "sli-lisp-selective-action-surface-carrier");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "zed-delta-chamber-formation-map");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "conditional-oe-selfgel-standing-matrix");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "mos-cmos-residue-closure-ledger");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "goa-cgoa-soulframe-duplex-telemetry-map");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "heartbeat-non-activation-refusal-ledger");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "sli-lisp-zed-delta-chamber-carrier");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "high-energy-articulation-candidate-map");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "provider-interface-observability-ledger");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "hidden-substrate-non-claim-matrix");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "candidate-engine-non-binding-ledger");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "candidate-role-assignment-boundary-map");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "sli-lisp-high-energy-articulation-candidate-carrier");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "membrane-morphology-transition-map");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "membrane-deformation-classification-ledger");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "malformed-transition-compost-matrix");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "high-energy-pressure-non-binding-boundary-ledger");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "membrane-core-non-mutation-ledger");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "sli-lisp-membrane-morphology-transition-carrier");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "engram-predicate-precursor-stream-map");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "predicate-residue-classification-ledger");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "predicate-candidacy-non-admission-matrix");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "epps-non-memory-non-authority-ledger");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "sli-lisp-epps-carrier");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "peer-review-predicate-bridge-map");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "reader-state-continuity-ladder");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "terminology-quarantine-ledger");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "prose-smoothing-boundary-matrix");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "sli-lisp-peer-review-bridge-carrier");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "gel-domain-scoped-ingress-map");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "domain-evidence-ceiling-ledger");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "ingress-cycle-non-admission-matrix");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "certification-review-non-admission-ledger");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "sli-lisp-gel-domain-ingress-carrier");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "shared-prime-pressure-ecology-map");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "pressure-destination-classification-ledger");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "integration-pressure-non-admission-matrix");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "selfgel-cradle-sanctuary-pressure-separation-ledger");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "sli-lisp-shared-prime-pressure-carrier");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "gap-crossing-articulation-map");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "llm-surface-participation-non-binding-ledger");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "pressure-to-articulation-lane-classification");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "gap-crossing-non-action-authority-matrix");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "sli-lisp-gap-crossing-carrier");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "pre-diagnostic-risk-surface-map");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "care-signal-non-diagnosis-ledger");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "risk-modifier-care-burden-matrix");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "qualified-review-routing-non-authority-ledger");
        Assert.Contains(receipt.Artifacts, artifact => artifact.ArtifactId == "sli-lisp-pre-diagnostic-risk-carrier");
        AssertForbiddenMotionFalse(receipt);

        var cellRoot = Path.Combine(fixture.InstallRootPath, "receipts", "spiral-build", "cells");
        AssertArtifactExistsAndParses(cellRoot, "body-layer-map.json");
        AssertArtifactExistsAndParses(cellRoot, "cell-taxonomy.json");
        AssertArtifactExistsAndParses(cellRoot, "prime-steward-membrane-map.json");
        AssertArtifactExistsAndParses(cellRoot, "cryptic-steward-telemetry-route.json");
        AssertArtifactExistsAndParses(cellRoot, "compass-shell-packet.json");
        AssertArtifactExistsAndParses(cellRoot, "telemetry-route-map.json");
        AssertArtifactExistsAndParses(cellRoot, "sanctuary-packet-contract-map.json");
        AssertArtifactExistsAndParses(cellRoot, "packet-membrane-validation-matrix.json");
        AssertArtifactExistsAndParses(cellRoot, "packet-non-authority-refusal-ledger.json");
        AssertArtifactExistsAndParses(cellRoot, "packet-passage-receipt-map.json");
        AssertArtifactExistsAndParses(cellRoot, "packet-refusal-routing-matrix.json");
        AssertArtifactExistsAndParses(cellRoot, "receipt-non-permission-ledger.json");
        AssertArtifactExistsAndParses(cellRoot, "receipt-replay-request-map.json");
        AssertArtifactExistsAndParses(cellRoot, "receipt-replay-boundary-matrix.json");
        AssertArtifactExistsAndParses(cellRoot, "replay-non-reentry-ledger.json");
        AssertArtifactExistsAndParses(cellRoot, "receipt-query-request-map.json");
        AssertArtifactExistsAndParses(cellRoot, "receipt-query-boundary-matrix.json");
        AssertArtifactExistsAndParses(cellRoot, "query-non-warrant-ledger.json");
        AssertArtifactExistsAndParses(cellRoot, "receipt-selection-request-map.json");
        AssertArtifactExistsAndParses(cellRoot, "receipt-selection-boundary-matrix.json");
        AssertArtifactExistsAndParses(cellRoot, "selection-non-admission-ledger.json");
        AssertArtifactExistsAndParses(cellRoot, "witness-summary-request-map.json");
        AssertArtifactExistsAndParses(cellRoot, "witness-summary-boundary-matrix.json");
        AssertArtifactExistsAndParses(cellRoot, "summary-non-replacement-ledger.json");
        AssertArtifactExistsAndParses(cellRoot, "compass-pre-engram-pressure-request-map.json");
        AssertArtifactExistsAndParses(cellRoot, "compass-pressure-boundary-matrix.json");
        AssertArtifactExistsAndParses(cellRoot, "pressure-non-engram-ledger.json");
        AssertArtifactExistsAndParses(cellRoot, "compass-shell-stabilization-map.json");
        AssertArtifactExistsAndParses(cellRoot, "shell-pressure-lineage-map.json");
        AssertArtifactExistsAndParses(cellRoot, "shell-non-engram-boundary-ledger.json");
        AssertArtifactExistsAndParses(cellRoot, "cleaving-discernment-request-map.json");
        AssertArtifactExistsAndParses(cellRoot, "cleaving-refusal-boundary-matrix.json");
        AssertArtifactExistsAndParses(cellRoot, "cleaving-non-admission-ledger.json");
        AssertArtifactExistsAndParses(cellRoot, "iterative-evaluation-loop-map.json");
        AssertArtifactExistsAndParses(cellRoot, "evaluation-non-authority-ledger.json");
        AssertArtifactExistsAndParses(cellRoot, "evaluation-tuning-candidate-map.json");
        AssertArtifactExistsAndParses(cellRoot, "recursive-contemplation-loop-map.json");
        AssertArtifactExistsAndParses(cellRoot, "contemplation-non-continuity-ledger.json");
        AssertArtifactExistsAndParses(cellRoot, "contemplation-cooling-path-map.json");
        AssertArtifactExistsAndParses(cellRoot, "steward-handoff-readiness-map.json");
        AssertArtifactExistsAndParses(cellRoot, "handoff-non-authorization-ledger.json");
        AssertArtifactExistsAndParses(cellRoot, "ten-pass-body-tuning-next-lane-map.json");
        AssertArtifactExistsAndParses(cellRoot, "typed-duplex-build-iteration-map.json");
        AssertArtifactExistsAndParses(cellRoot, "iteration-flow-form-learning-map.json");
        AssertArtifactExistsAndParses(cellRoot, "theory-direct-representation-optimization-map.json");
        AssertArtifactExistsAndParses(cellRoot, "ten-by-ten-body-section-pass-matrix.json");
        AssertArtifactExistsAndParses(cellRoot, "three-section-cascade-group-map.json");
        AssertArtifactExistsAndParses(cellRoot, "lamp-body-seed-exclusion-optimization-ledger.json");
        AssertArtifactExistsAndParses(cellRoot, "group-a-body-optimization-run-ledger.json");
        AssertArtifactExistsAndParses(cellRoot, "group-a-flow-form-findings-map.json");
        AssertArtifactExistsAndParses(cellRoot, "group-a-next-group-eligibility-receipt.json");
        AssertArtifactExistsAndParses(cellRoot, "group-b-body-optimization-run-ledger.json");
        AssertArtifactExistsAndParses(cellRoot, "group-b-flow-form-findings-map.json");
        AssertArtifactExistsAndParses(cellRoot, "group-b-next-group-eligibility-receipt.json");
        AssertArtifactExistsAndParses(cellRoot, "group-c-body-optimization-run-ledger.json");
        AssertArtifactExistsAndParses(cellRoot, "group-c-flow-form-findings-map.json");
        AssertArtifactExistsAndParses(cellRoot, "group-c-next-group-eligibility-receipt.json");
        AssertArtifactExistsAndParses(cellRoot, "group-d-body-optimization-run-ledger.json");
        AssertArtifactExistsAndParses(cellRoot, "group-d-flow-form-findings-map.json");
        AssertArtifactExistsAndParses(cellRoot, "group-d-whole-body-synthesis-eligibility-receipt.json");
        AssertArtifactExistsAndParses(cellRoot, "whole-body-synthesis-comparison-ledger.json");
        AssertArtifactExistsAndParses(cellRoot, "whole-body-doctrine-guardrail-coverage-map.json");
        AssertArtifactExistsAndParses(cellRoot, "whole-body-unresolved-membrane-gap-and-next-lane-receipt.json");
        AssertArtifactExistsAndParses(cellRoot, "ninefold-worker-telemetry-contract.json");
        AssertArtifactExistsAndParses(cellRoot, "ninefold-domain-run-assignment-map.json");
        AssertArtifactExistsAndParses(cellRoot, "ninefold-braid-custody-non-promotion-ledger.json");
        AssertArtifactExistsAndParses(cellRoot, "engram-candidate-precondition-map.json");
        AssertArtifactExistsAndParses(cellRoot, "residue-to-candidate-refusal-ledger.json");
        AssertArtifactExistsAndParses(cellRoot, "engram-candidate-admission-ceiling-matrix.json");
        AssertArtifactExistsAndParses(cellRoot, "swarm-worker-packet-contract-map.json");
        AssertArtifactExistsAndParses(cellRoot, "swarm-braid-selection-boundary-matrix.json");
        AssertArtifactExistsAndParses(cellRoot, "swarm-consensus-non-warrant-ledger.json");
        AssertArtifactExistsAndParses(cellRoot, "persistent-witness-store-contract-map.json");
        AssertArtifactExistsAndParses(cellRoot, "persistent-witness-store-custody-boundary-matrix.json");
        AssertArtifactExistsAndParses(cellRoot, "witness-storage-non-authority-ledger.json");
        AssertArtifactExistsAndParses(cellRoot, "sli-lisp-posture-manifest-map.json");
        AssertArtifactExistsAndParses(cellRoot, "csharp-lisp-duplex-non-evaluation-boundary-matrix.json");
        AssertArtifactExistsAndParses(cellRoot, "sli-lisp-posture-non-execution-ledger.json");
        AssertArtifactExistsAndParses(cellRoot, "sli-lisp-compass-carrier-shell-map.json");
        AssertArtifactExistsAndParses(cellRoot, "sli-lisp-rooting-law-lineage-ledger.json");
        AssertArtifactExistsAndParses(cellRoot, "sli-lisp-petal-candidate-gap-matrix.json");
        AssertArtifactExistsAndParses(cellRoot, "ec-meaning-shell-contract-map.json");
        AssertArtifactExistsAndParses(cellRoot, "ec-perspectival-tier-boundary-matrix.json");
        AssertArtifactExistsAndParses(cellRoot, "ec-compost-non-self-attribution-ledger.json");
        AssertArtifactExistsAndParses(cellRoot, "ec-participatory-predicate-structure-map.json");
        AssertArtifactExistsAndParses(cellRoot, "ec-peerless-delta-witness-boundary-matrix.json");
        AssertArtifactExistsAndParses(cellRoot, "ec-personification-non-authority-ledger.json");
        AssertArtifactExistsAndParses(cellRoot, "cme-lisp-thread-class-map.json");
        AssertArtifactExistsAndParses(cellRoot, "cme-lisp-thread-tension-playability-matrix.json");
        AssertArtifactExistsAndParses(cellRoot, "cme-lisp-resonance-non-authority-ledger.json");
        AssertArtifactExistsAndParses(cellRoot, "listening-frame-emanation-map.json");
        AssertArtifactExistsAndParses(cellRoot, "global-resonance-law-ledger.json");
        AssertArtifactExistsAndParses(cellRoot, "steward-heartbeat-policy-map.json");
        AssertArtifactExistsAndParses(cellRoot, "thread-touch-event-boundary.json");
        AssertArtifactExistsAndParses(cellRoot, "resonance-evidence-ledger.json");
        AssertArtifactExistsAndParses(cellRoot, "damping-discordance-route-matrix.json");
        AssertArtifactExistsAndParses(cellRoot, "action-admission-boundary-report.json");
        AssertArtifactExistsAndParses(cellRoot, "steward-harmonic-interlock-map.json");
        AssertArtifactExistsAndParses(cellRoot, "lawful-signal-composability-matrix.json");
        AssertArtifactExistsAndParses(cellRoot, "shared-surface-contention-ledger.json");
        AssertArtifactExistsAndParses(cellRoot, "cadence-alignment-policy-map.json");
        AssertArtifactExistsAndParses(cellRoot, "damping-backoff-policy-map.json");
        AssertArtifactExistsAndParses(cellRoot, "witness-surface-split-route-map.json");
        AssertArtifactExistsAndParses(cellRoot, "interlock-non-authority-boundary-report.json");
        AssertArtifactExistsAndParses(cellRoot, "modulation-correspondence-atlas-map.json");
        AssertArtifactExistsAndParses(cellRoot, "source-domain-success-condition-ledger.json");
        AssertArtifactExistsAndParses(cellRoot, "cme-translation-boundary-matrix.json");
        AssertArtifactExistsAndParses(cellRoot, "channel-success-non-warrant-ledger.json");
        AssertArtifactExistsAndParses(cellRoot, "correspondence-loss-condition-ledger.json");
        AssertArtifactExistsAndParses(cellRoot, "operational-actualization-test-map.json");
        AssertArtifactExistsAndParses(cellRoot, "mature-discipline-intake-protocol.json");
        AssertArtifactExistsAndParses(cellRoot, "typed-action-surface-declaration-map.json");
        AssertArtifactExistsAndParses(cellRoot, "methodological-formation-analysis-map.json");
        AssertArtifactExistsAndParses(cellRoot, "design-predicate-boundary-matrix.json");
        AssertArtifactExistsAndParses(cellRoot, "action-candidate-non-execution-ledger.json");
        AssertArtifactExistsAndParses(cellRoot, "sli-lisp-action-surface-declaration-carrier.json");
        AssertArtifactExistsAndParses(cellRoot, "action-method-readiness-map.json");
        AssertArtifactExistsAndParses(cellRoot, "steward-method-review-boundary-matrix.json");
        AssertArtifactExistsAndParses(cellRoot, "method-term-satisfaction-non-warrant-ledger.json");
        AssertArtifactExistsAndParses(cellRoot, "method-lineage-custody-map.json");
        AssertArtifactExistsAndParses(cellRoot, "sli-lisp-method-readiness-carrier.json");
        AssertArtifactExistsAndParses(cellRoot, "steward-action-admissibility-map.json");
        AssertArtifactExistsAndParses(cellRoot, "admissibility-predicate-result-matrix.json");
        AssertArtifactExistsAndParses(cellRoot, "admissibility-non-execution-ledger.json");
        AssertArtifactExistsAndParses(cellRoot, "admissible-action-custody-lineage-map.json");
        AssertArtifactExistsAndParses(cellRoot, "sli-lisp-steward-admissibility-carrier.json");
        AssertArtifactExistsAndParses(cellRoot, "anti-capture-motivated-concern-map.json");
        AssertArtifactExistsAndParses(cellRoot, "motivational-variance-signal-matrix.json");
        AssertArtifactExistsAndParses(cellRoot, "concern-non-action-ledger.json");
        AssertArtifactExistsAndParses(cellRoot, "capture-pressure-route-custody-map.json");
        AssertArtifactExistsAndParses(cellRoot, "sli-lisp-anti-capture-concern-carrier.json");
        AssertArtifactExistsAndParses(cellRoot, "personification-predicate-hook-map.json");
        AssertArtifactExistsAndParses(cellRoot, "six-plane-personification-hook-matrix.json");
        AssertArtifactExistsAndParses(cellRoot, "vulnerability-overreach-repair-ledger.json");
        AssertArtifactExistsAndParses(cellRoot, "personification-non-personhood-ledger.json");
        AssertArtifactExistsAndParses(cellRoot, "sli-lisp-personification-hook-carrier.json");
        AssertArtifactExistsAndParses(cellRoot, "personification-modality-humility-map.json");
        AssertArtifactExistsAndParses(cellRoot, "bonded-relation-consent-custody-matrix.json");
        AssertArtifactExistsAndParses(cellRoot, "modality-bandwidth-non-authority-ledger.json");
        AssertArtifactExistsAndParses(cellRoot, "presence-non-embodiment-refusal-ledger.json");
        AssertArtifactExistsAndParses(cellRoot, "sli-lisp-personification-modality-carrier.json");
        AssertArtifactExistsAndParses(cellRoot, "dialogos-thought-status-map.json");
        AssertArtifactExistsAndParses(cellRoot, "articulation-warrant-boundary-matrix.json");
        AssertArtifactExistsAndParses(cellRoot, "principled-refusal-return-path-ledger.json");
        AssertArtifactExistsAndParses(cellRoot, "perspectival-knowing-participatory-thought-form-map.json");
        AssertArtifactExistsAndParses(cellRoot, "sli-lisp-dialogos-discernment-carrier.json");
        AssertArtifactExistsAndParses(cellRoot, "wave-condensation-signal-map.json");
        AssertArtifactExistsAndParses(cellRoot, "shared-reality-anchor-boundary-matrix.json");
        AssertArtifactExistsAndParses(cellRoot, "condensation-non-warrant-ledger.json");
        AssertArtifactExistsAndParses(cellRoot, "consensus-non-authority-refusal-ledger.json");
        AssertArtifactExistsAndParses(cellRoot, "sli-lisp-wave-condensation-carrier.json");
        AssertArtifactExistsAndParses(cellRoot, "wave-cascade-run-schedule.json");
        AssertArtifactExistsAndParses(cellRoot, "thirty-sixty-ninety-seam-receipt-ledger.json");
        AssertArtifactExistsAndParses(cellRoot, "cascade-volume-non-warrant-ledger.json");
        AssertArtifactExistsAndParses(cellRoot, "cascade-shared-reality-braid-map.json");
        AssertArtifactExistsAndParses(cellRoot, "sli-lisp-wave-cascade-carrier.json");
        AssertArtifactExistsAndParses(cellRoot, "aspiration-payload-map.json");
        AssertArtifactExistsAndParses(cellRoot, "payload-ingestion-lane-matrix.json");
        AssertArtifactExistsAndParses(cellRoot, "articulation-maturation-candidate-ledger.json");
        AssertArtifactExistsAndParses(cellRoot, "full-stack-non-activation-refusal-ledger.json");
        AssertArtifactExistsAndParses(cellRoot, "sli-lisp-aspiration-payload-carrier.json");
        AssertArtifactExistsAndParses(cellRoot, "aspiration-candidate-selection-map.json");
        AssertArtifactExistsAndParses(cellRoot, "selected-working-set-non-warrant-ledger.json");
        AssertArtifactExistsAndParses(cellRoot, "closure-law-without-key-boundary-matrix.json");
        AssertArtifactExistsAndParses(cellRoot, "compost-retention-non-erasure-ledger.json");
        AssertArtifactExistsAndParses(cellRoot, "sli-lisp-aspiration-selection-carrier.json");
        AssertArtifactExistsAndParses(cellRoot, "scoped-work-packet-map.json");
        AssertArtifactExistsAndParses(cellRoot, "packet-scope-boundary-matrix.json");
        AssertArtifactExistsAndParses(cellRoot, "work-packet-non-execution-ledger.json");
        AssertArtifactExistsAndParses(cellRoot, "steward-review-routing-custody-map.json");
        AssertArtifactExistsAndParses(cellRoot, "sli-lisp-scoped-work-packet-carrier.json");
        AssertArtifactExistsAndParses(cellRoot, "enactment-boundary-readiness-map.json");
        AssertArtifactExistsAndParses(cellRoot, "enactment-approach-non-execution-ledger.json");
        AssertArtifactExistsAndParses(cellRoot, "reversible-local-effect-ceiling-matrix.json");
        AssertArtifactExistsAndParses(cellRoot, "steward-enactment-review-custody-map.json");
        AssertArtifactExistsAndParses(cellRoot, "sli-lisp-enactment-boundary-carrier.json");
        AssertArtifactExistsAndParses(cellRoot, "enactment-dry-run-harness-map.json");
        AssertArtifactExistsAndParses(cellRoot, "dry-run-rehearsal-non-enactment-ledger.json");
        AssertArtifactExistsAndParses(cellRoot, "simulated-effect-and-rollback-proof-matrix.json");
        AssertArtifactExistsAndParses(cellRoot, "steward-dry-run-review-receipt-map.json");
        AssertArtifactExistsAndParses(cellRoot, "sli-lisp-dry-run-rehearsal-carrier.json");
        AssertArtifactExistsAndParses(cellRoot, "ec-precipitation-witness-map.json");
        AssertArtifactExistsAndParses(cellRoot, "active-witness-lineage-reconstruction-matrix.json");
        AssertArtifactExistsAndParses(cellRoot, "selfgel-candidate-non-admission-ledger.json");
        AssertArtifactExistsAndParses(cellRoot, "maximal-truth-seeking-predicate-law-ledger.json");
        AssertArtifactExistsAndParses(cellRoot, "sli-lisp-ec-precipitation-witness-carrier.json");
        AssertArtifactExistsAndParses(cellRoot, "rehearsal-distinction-pressure-map.json");
        AssertArtifactExistsAndParses(cellRoot, "possibility-density-pressure-vector-ledger.json");
        AssertArtifactExistsAndParses(cellRoot, "urgency-not-jurisdiction-refusal-ledger.json");
        AssertArtifactExistsAndParses(cellRoot, "failure-dignity-cooling-matrix.json");
        AssertArtifactExistsAndParses(cellRoot, "sli-lisp-rehearsal-pressure-carrier.json");
        AssertArtifactExistsAndParses(cellRoot, "personification-actualization-surface-map.json");
        AssertArtifactExistsAndParses(cellRoot, "pre-morphological-use-vector-ledger.json");
        AssertArtifactExistsAndParses(cellRoot, "surface-actualization-non-identity-matrix.json");
        AssertArtifactExistsAndParses(cellRoot, "salience-guidance-non-authority-ledger.json");
        AssertArtifactExistsAndParses(cellRoot, "sli-lisp-personification-actualization-carrier.json");
        AssertArtifactExistsAndParses(cellRoot, "selective-lawful-action-surface-map.json");
        AssertArtifactExistsAndParses(cellRoot, "surface-touch-non-enactment-ledger.json");
        AssertArtifactExistsAndParses(cellRoot, "personification-guidance-action-separation-matrix.json");
        AssertArtifactExistsAndParses(cellRoot, "action-surface-custody-revocation-ledger.json");
        AssertArtifactExistsAndParses(cellRoot, "sli-lisp-selective-action-surface-carrier.json");
        AssertArtifactExistsAndParses(cellRoot, "zed-delta-chamber-formation-map.json");
        AssertArtifactExistsAndParses(cellRoot, "conditional-oe-selfgel-standing-matrix.json");
        AssertArtifactExistsAndParses(cellRoot, "mos-cmos-residue-closure-ledger.json");
        AssertArtifactExistsAndParses(cellRoot, "goa-cgoa-soulframe-duplex-telemetry-map.json");
        AssertArtifactExistsAndParses(cellRoot, "heartbeat-non-activation-refusal-ledger.json");
        AssertArtifactExistsAndParses(cellRoot, "sli-lisp-zed-delta-chamber-carrier.json");
        AssertArtifactExistsAndParses(cellRoot, "high-energy-articulation-candidate-map.json");
        AssertArtifactExistsAndParses(cellRoot, "provider-interface-observability-ledger.json");
        AssertArtifactExistsAndParses(cellRoot, "hidden-substrate-non-claim-matrix.json");
        AssertArtifactExistsAndParses(cellRoot, "candidate-engine-non-binding-ledger.json");
        AssertArtifactExistsAndParses(cellRoot, "candidate-role-assignment-boundary-map.json");
        AssertArtifactExistsAndParses(cellRoot, "sli-lisp-high-energy-articulation-candidate-carrier.json");
        AssertArtifactExistsAndParses(cellRoot, "membrane-morphology-transition-map.json");
        AssertArtifactExistsAndParses(cellRoot, "membrane-deformation-classification-ledger.json");
        AssertArtifactExistsAndParses(cellRoot, "malformed-transition-compost-matrix.json");
        AssertArtifactExistsAndParses(cellRoot, "high-energy-pressure-non-binding-boundary-ledger.json");
        AssertArtifactExistsAndParses(cellRoot, "membrane-core-non-mutation-ledger.json");
        AssertArtifactExistsAndParses(cellRoot, "sli-lisp-membrane-morphology-transition-carrier.json");
        AssertArtifactExistsAndParses(cellRoot, "engram-predicate-precursor-stream-map.json");
        AssertArtifactExistsAndParses(cellRoot, "predicate-residue-classification-ledger.json");
        AssertArtifactExistsAndParses(cellRoot, "predicate-candidacy-non-admission-matrix.json");
        AssertArtifactExistsAndParses(cellRoot, "epps-non-memory-non-authority-ledger.json");
        AssertArtifactExistsAndParses(cellRoot, "sli-lisp-epps-carrier.json");
        AssertArtifactExistsAndParses(cellRoot, "peer-review-predicate-bridge-map.json");
        AssertArtifactExistsAndParses(cellRoot, "reader-state-continuity-ladder.json");
        AssertArtifactExistsAndParses(cellRoot, "terminology-quarantine-ledger.json");
        AssertArtifactExistsAndParses(cellRoot, "prose-smoothing-boundary-matrix.json");
        AssertArtifactExistsAndParses(cellRoot, "sli-lisp-peer-review-bridge-carrier.json");
        AssertArtifactExistsAndParses(cellRoot, "gel-domain-scoped-ingress-map.json");
        AssertArtifactExistsAndParses(cellRoot, "domain-evidence-ceiling-ledger.json");
        AssertArtifactExistsAndParses(cellRoot, "ingress-cycle-non-admission-matrix.json");
        AssertArtifactExistsAndParses(cellRoot, "certification-review-non-admission-ledger.json");
        AssertArtifactExistsAndParses(cellRoot, "sli-lisp-gel-domain-ingress-carrier.json");
        AssertArtifactExistsAndParses(cellRoot, "shared-prime-pressure-ecology-map.json");
        AssertArtifactExistsAndParses(cellRoot, "pressure-destination-classification-ledger.json");
        AssertArtifactExistsAndParses(cellRoot, "integration-pressure-non-admission-matrix.json");
        AssertArtifactExistsAndParses(cellRoot, "selfgel-cradle-sanctuary-pressure-separation-ledger.json");
        AssertArtifactExistsAndParses(cellRoot, "sli-lisp-shared-prime-pressure-carrier.json");
        AssertArtifactExistsAndParses(cellRoot, "gap-crossing-articulation-map.json");
        AssertArtifactExistsAndParses(cellRoot, "llm-surface-participation-non-binding-ledger.json");
        AssertArtifactExistsAndParses(cellRoot, "pressure-to-articulation-lane-classification.json");
        AssertArtifactExistsAndParses(cellRoot, "gap-crossing-non-action-authority-matrix.json");
        AssertArtifactExistsAndParses(cellRoot, "sli-lisp-gap-crossing-carrier.json");
        AssertArtifactExistsAndParses(cellRoot, "pre-diagnostic-risk-surface-map.json");
        AssertArtifactExistsAndParses(cellRoot, "care-signal-non-diagnosis-ledger.json");
        AssertArtifactExistsAndParses(cellRoot, "risk-modifier-care-burden-matrix.json");
        AssertArtifactExistsAndParses(cellRoot, "qualified-review-routing-non-authority-ledger.json");
        AssertArtifactExistsAndParses(cellRoot, "sli-lisp-pre-diagnostic-risk-carrier.json");
        AssertArtifactDetailsCount(cellRoot, "ten-by-ten-body-section-pass-matrix.json", 100);
        AssertArtifactDetailsCount(cellRoot, "three-section-cascade-group-map.json", 5);
        AssertArtifactDetailsCount(cellRoot, "lamp-body-seed-exclusion-optimization-ledger.json", 6);
        AssertArtifactDetailsCount(cellRoot, "group-a-body-optimization-run-ledger.json", 30);
        AssertArtifactDetailsCount(cellRoot, "group-a-flow-form-findings-map.json", 9);
        AssertArtifactDetailsCount(cellRoot, "group-a-next-group-eligibility-receipt.json", 6);
        AssertArtifactDetailsCount(cellRoot, "group-b-body-optimization-run-ledger.json", 30);
        AssertArtifactDetailsCount(cellRoot, "group-b-flow-form-findings-map.json", 9);
        AssertArtifactDetailsCount(cellRoot, "group-b-next-group-eligibility-receipt.json", 6);
        AssertArtifactDetailsCount(cellRoot, "group-c-body-optimization-run-ledger.json", 30);
        AssertArtifactDetailsCount(cellRoot, "group-c-flow-form-findings-map.json", 9);
        AssertArtifactDetailsCount(cellRoot, "group-c-next-group-eligibility-receipt.json", 6);
        AssertArtifactDetailsCount(cellRoot, "group-d-body-optimization-run-ledger.json", 10);
        AssertArtifactDetailsCount(cellRoot, "group-d-flow-form-findings-map.json", 3);
        AssertArtifactDetailsCount(cellRoot, "group-d-whole-body-synthesis-eligibility-receipt.json", 6);
        AssertArtifactDetailsCount(cellRoot, "whole-body-synthesis-comparison-ledger.json", 10);
        AssertArtifactDetailsCount(cellRoot, "whole-body-doctrine-guardrail-coverage-map.json", 12);
        AssertArtifactDetailsCount(cellRoot, "whole-body-unresolved-membrane-gap-and-next-lane-receipt.json", 11);
        AssertArtifactDetailsCount(cellRoot, "ninefold-worker-telemetry-contract.json", 12);
        AssertArtifactDetailsCount(cellRoot, "ninefold-domain-run-assignment-map.json", 15);
        AssertArtifactDetailsCount(cellRoot, "ninefold-braid-custody-non-promotion-ledger.json", 12);
        AssertArtifactDetailsCount(cellRoot, "engram-candidate-precondition-map.json", 11);
        AssertArtifactDetailsCount(cellRoot, "residue-to-candidate-refusal-ledger.json", 10);
        AssertArtifactDetailsCount(cellRoot, "engram-candidate-admission-ceiling-matrix.json", 10);
        AssertArtifactDetailsCount(cellRoot, "swarm-worker-packet-contract-map.json", 10);
        AssertArtifactDetailsCount(cellRoot, "swarm-braid-selection-boundary-matrix.json", 12);
        AssertArtifactDetailsCount(cellRoot, "swarm-consensus-non-warrant-ledger.json", 12);
        AssertArtifactDetailsCount(cellRoot, "persistent-witness-store-contract-map.json", 12);
        AssertArtifactDetailsCount(cellRoot, "persistent-witness-store-custody-boundary-matrix.json", 12);
        AssertArtifactDetailsCount(cellRoot, "witness-storage-non-authority-ledger.json", 12);
        AssertArtifactDetailsCount(cellRoot, "sli-lisp-posture-manifest-map.json", 12);
        AssertArtifactDetailsCount(cellRoot, "csharp-lisp-duplex-non-evaluation-boundary-matrix.json", 12);
        AssertArtifactDetailsCount(cellRoot, "sli-lisp-posture-non-execution-ledger.json", 12);
        AssertArtifactDetailsCount(cellRoot, "sli-lisp-compass-carrier-shell-map.json", 14);
        AssertArtifactDetailsCount(cellRoot, "sli-lisp-rooting-law-lineage-ledger.json", 12);
        AssertArtifactDetailsCount(cellRoot, "sli-lisp-petal-candidate-gap-matrix.json", 15);
        AssertArtifactDetailsCount(cellRoot, "ec-meaning-shell-contract-map.json", 12);
        AssertArtifactDetailsCount(cellRoot, "ec-perspectival-tier-boundary-matrix.json", 12);
        AssertArtifactDetailsCount(cellRoot, "ec-compost-non-self-attribution-ledger.json", 12);
        AssertArtifactDetailsCount(cellRoot, "ec-participatory-predicate-structure-map.json", 13);
        AssertArtifactDetailsCount(cellRoot, "ec-peerless-delta-witness-boundary-matrix.json", 13);
        AssertArtifactDetailsCount(cellRoot, "ec-personification-non-authority-ledger.json", 13);
        AssertArtifactDetailsCount(cellRoot, "cme-lisp-thread-class-map.json", 15);
        AssertArtifactDetailsCount(cellRoot, "cme-lisp-thread-tension-playability-matrix.json", 13);
        AssertArtifactDetailsCount(cellRoot, "cme-lisp-resonance-non-authority-ledger.json", 13);
        AssertArtifactDetailsCount(cellRoot, "listening-frame-emanation-map.json", 10);
        AssertArtifactDetailsCount(cellRoot, "global-resonance-law-ledger.json", 12);
        AssertArtifactDetailsCount(cellRoot, "steward-heartbeat-policy-map.json", 10);
        AssertArtifactDetailsCount(cellRoot, "thread-touch-event-boundary.json", 12);
        AssertArtifactDetailsCount(cellRoot, "resonance-evidence-ledger.json", 10);
        AssertArtifactDetailsCount(cellRoot, "damping-discordance-route-matrix.json", 12);
        AssertArtifactDetailsCount(cellRoot, "action-admission-boundary-report.json", 10);
        AssertArtifactDetailsCount(cellRoot, "steward-harmonic-interlock-map.json", 12);
        AssertArtifactDetailsCount(cellRoot, "lawful-signal-composability-matrix.json", 12);
        AssertArtifactDetailsCount(cellRoot, "shared-surface-contention-ledger.json", 12);
        AssertArtifactDetailsCount(cellRoot, "cadence-alignment-policy-map.json", 12);
        AssertArtifactDetailsCount(cellRoot, "damping-backoff-policy-map.json", 12);
        AssertArtifactDetailsCount(cellRoot, "witness-surface-split-route-map.json", 12);
        AssertArtifactDetailsCount(cellRoot, "interlock-non-authority-boundary-report.json", 12);
        AssertArtifactDetailsCount(cellRoot, "modulation-correspondence-atlas-map.json", 12);
        AssertArtifactDetailsCount(cellRoot, "source-domain-success-condition-ledger.json", 12);
        AssertArtifactDetailsCount(cellRoot, "cme-translation-boundary-matrix.json", 12);
        AssertArtifactDetailsCount(cellRoot, "channel-success-non-warrant-ledger.json", 12);
        AssertArtifactDetailsCount(cellRoot, "correspondence-loss-condition-ledger.json", 12);
        AssertArtifactDetailsCount(cellRoot, "operational-actualization-test-map.json", 12);
        AssertArtifactDetailsCount(cellRoot, "mature-discipline-intake-protocol.json", 12);
        AssertArtifactDetailsCount(cellRoot, "typed-action-surface-declaration-map.json", 15);
        AssertArtifactDetailsCount(cellRoot, "methodological-formation-analysis-map.json", 15);
        AssertArtifactDetailsCount(cellRoot, "design-predicate-boundary-matrix.json", 15);
        AssertArtifactDetailsCount(cellRoot, "action-candidate-non-execution-ledger.json", 15);
        AssertArtifactDetailsCount(cellRoot, "sli-lisp-action-surface-declaration-carrier.json", 12);
        AssertArtifactDetailsCount(cellRoot, "action-method-readiness-map.json", 13);
        AssertArtifactDetailsCount(cellRoot, "steward-method-review-boundary-matrix.json", 12);
        AssertArtifactDetailsCount(cellRoot, "method-term-satisfaction-non-warrant-ledger.json", 12);
        AssertArtifactDetailsCount(cellRoot, "method-lineage-custody-map.json", 12);
        AssertArtifactDetailsCount(cellRoot, "sli-lisp-method-readiness-carrier.json", 12);
        AssertArtifactDetailsCount(cellRoot, "steward-action-admissibility-map.json", 13);
        AssertArtifactDetailsCount(cellRoot, "admissibility-predicate-result-matrix.json", 12);
        AssertArtifactDetailsCount(cellRoot, "admissibility-non-execution-ledger.json", 12);
        AssertArtifactDetailsCount(cellRoot, "admissible-action-custody-lineage-map.json", 12);
        AssertArtifactDetailsCount(cellRoot, "sli-lisp-steward-admissibility-carrier.json", 12);
        AssertArtifactDetailsCount(cellRoot, "anti-capture-motivated-concern-map.json", 13);
        AssertArtifactDetailsCount(cellRoot, "motivational-variance-signal-matrix.json", 12);
        AssertArtifactDetailsCount(cellRoot, "concern-non-action-ledger.json", 12);
        AssertArtifactDetailsCount(cellRoot, "capture-pressure-route-custody-map.json", 12);
        AssertArtifactDetailsCount(cellRoot, "sli-lisp-anti-capture-concern-carrier.json", 12);
        AssertArtifactDetailsCount(cellRoot, "personification-predicate-hook-map.json", 13);
        AssertArtifactDetailsCount(cellRoot, "six-plane-personification-hook-matrix.json", 12);
        AssertArtifactDetailsCount(cellRoot, "vulnerability-overreach-repair-ledger.json", 12);
        AssertArtifactDetailsCount(cellRoot, "personification-non-personhood-ledger.json", 12);
        AssertArtifactDetailsCount(cellRoot, "sli-lisp-personification-hook-carrier.json", 12);
        AssertArtifactDetailsCount(cellRoot, "personification-modality-humility-map.json", 13);
        AssertArtifactDetailsCount(cellRoot, "bonded-relation-consent-custody-matrix.json", 12);
        AssertArtifactDetailsCount(cellRoot, "modality-bandwidth-non-authority-ledger.json", 12);
        AssertArtifactDetailsCount(cellRoot, "presence-non-embodiment-refusal-ledger.json", 12);
        AssertArtifactDetailsCount(cellRoot, "sli-lisp-personification-modality-carrier.json", 13);
        AssertArtifactDetailsCount(cellRoot, "dialogos-thought-status-map.json", 13);
        AssertArtifactDetailsCount(cellRoot, "articulation-warrant-boundary-matrix.json", 12);
        AssertArtifactDetailsCount(cellRoot, "principled-refusal-return-path-ledger.json", 12);
        AssertArtifactDetailsCount(cellRoot, "perspectival-knowing-participatory-thought-form-map.json", 12);
        AssertArtifactDetailsCount(cellRoot, "sli-lisp-dialogos-discernment-carrier.json", 12);
        AssertArtifactDetailsCount(cellRoot, "wave-condensation-signal-map.json", 12);
        AssertArtifactDetailsCount(cellRoot, "shared-reality-anchor-boundary-matrix.json", 12);
        AssertArtifactDetailsCount(cellRoot, "condensation-non-warrant-ledger.json", 12);
        AssertArtifactDetailsCount(cellRoot, "consensus-non-authority-refusal-ledger.json", 12);
        AssertArtifactDetailsCount(cellRoot, "sli-lisp-wave-condensation-carrier.json", 12);
        AssertArtifactDetailsCount(cellRoot, "wave-cascade-run-schedule.json", 90);
        AssertArtifactDetailsCount(cellRoot, "thirty-sixty-ninety-seam-receipt-ledger.json", 12);
        AssertArtifactDetailsCount(cellRoot, "cascade-volume-non-warrant-ledger.json", 12);
        AssertArtifactDetailsCount(cellRoot, "cascade-shared-reality-braid-map.json", 12);
        AssertArtifactDetailsCount(cellRoot, "sli-lisp-wave-cascade-carrier.json", 12);
        AssertArtifactDetailsCount(cellRoot, "aspiration-payload-map.json", 13);
        AssertArtifactDetailsCount(cellRoot, "payload-ingestion-lane-matrix.json", 12);
        AssertArtifactDetailsCount(cellRoot, "articulation-maturation-candidate-ledger.json", 12);
        AssertArtifactDetailsCount(cellRoot, "full-stack-non-activation-refusal-ledger.json", 12);
        AssertArtifactDetailsCount(cellRoot, "sli-lisp-aspiration-payload-carrier.json", 12);
        AssertArtifactDetailsCount(cellRoot, "aspiration-candidate-selection-map.json", 12);
        AssertArtifactDetailsCount(cellRoot, "selected-working-set-non-warrant-ledger.json", 12);
        AssertArtifactDetailsCount(cellRoot, "closure-law-without-key-boundary-matrix.json", 12);
        AssertArtifactDetailsCount(cellRoot, "compost-retention-non-erasure-ledger.json", 12);
        AssertArtifactDetailsCount(cellRoot, "sli-lisp-aspiration-selection-carrier.json", 12);
        AssertArtifactDetailsCount(cellRoot, "scoped-work-packet-map.json", 13);
        AssertArtifactDetailsCount(cellRoot, "packet-scope-boundary-matrix.json", 15);
        AssertArtifactDetailsCount(cellRoot, "work-packet-non-execution-ledger.json", 12);
        AssertArtifactDetailsCount(cellRoot, "steward-review-routing-custody-map.json", 11);
        AssertArtifactDetailsCount(cellRoot, "sli-lisp-scoped-work-packet-carrier.json", 12);
        AssertArtifactDetailsCount(cellRoot, "enactment-boundary-readiness-map.json", 13);
        AssertArtifactDetailsCount(cellRoot, "enactment-approach-non-execution-ledger.json", 13);
        AssertArtifactDetailsCount(cellRoot, "reversible-local-effect-ceiling-matrix.json", 12);
        AssertArtifactDetailsCount(cellRoot, "steward-enactment-review-custody-map.json", 12);
        AssertArtifactDetailsCount(cellRoot, "sli-lisp-enactment-boundary-carrier.json", 13);
        AssertArtifactDetailsCount(cellRoot, "enactment-dry-run-harness-map.json", 13);
        AssertArtifactDetailsCount(cellRoot, "dry-run-rehearsal-non-enactment-ledger.json", 12);
        AssertArtifactDetailsCount(cellRoot, "simulated-effect-and-rollback-proof-matrix.json", 12);
        AssertArtifactDetailsCount(cellRoot, "steward-dry-run-review-receipt-map.json", 12);
        AssertArtifactDetailsCount(cellRoot, "sli-lisp-dry-run-rehearsal-carrier.json", 14);
        AssertArtifactDetailsCount(cellRoot, "ec-precipitation-witness-map.json", 13);
        AssertArtifactDetailsCount(cellRoot, "active-witness-lineage-reconstruction-matrix.json", 12);
        AssertArtifactDetailsCount(cellRoot, "selfgel-candidate-non-admission-ledger.json", 12);
        AssertArtifactDetailsCount(cellRoot, "maximal-truth-seeking-predicate-law-ledger.json", 12);
        AssertArtifactDetailsCount(cellRoot, "sli-lisp-ec-precipitation-witness-carrier.json", 13);
        AssertArtifactDetailsCount(cellRoot, "rehearsal-distinction-pressure-map.json", 13);
        AssertArtifactDetailsCount(cellRoot, "possibility-density-pressure-vector-ledger.json", 12);
        AssertArtifactDetailsCount(cellRoot, "urgency-not-jurisdiction-refusal-ledger.json", 12);
        AssertArtifactDetailsCount(cellRoot, "failure-dignity-cooling-matrix.json", 12);
        AssertArtifactDetailsCount(cellRoot, "sli-lisp-rehearsal-pressure-carrier.json", 13);
        AssertArtifactDetailsCount(cellRoot, "personification-actualization-surface-map.json", 13);
        AssertArtifactDetailsCount(cellRoot, "pre-morphological-use-vector-ledger.json", 12);
        AssertArtifactDetailsCount(cellRoot, "surface-actualization-non-identity-matrix.json", 12);
        AssertArtifactDetailsCount(cellRoot, "salience-guidance-non-authority-ledger.json", 12);
        AssertArtifactDetailsCount(cellRoot, "sli-lisp-personification-actualization-carrier.json", 13);
        AssertArtifactDetailsCount(cellRoot, "selective-lawful-action-surface-map.json", 12);
        AssertArtifactDetailsCount(cellRoot, "surface-touch-non-enactment-ledger.json", 12);
        AssertArtifactDetailsCount(cellRoot, "personification-guidance-action-separation-matrix.json", 12);
        AssertArtifactDetailsCount(cellRoot, "action-surface-custody-revocation-ledger.json", 12);
        AssertArtifactDetailsCount(cellRoot, "sli-lisp-selective-action-surface-carrier.json", 12);
        AssertArtifactDetailsCount(cellRoot, "zed-delta-chamber-formation-map.json", 12);
        AssertArtifactDetailsCount(cellRoot, "conditional-oe-selfgel-standing-matrix.json", 12);
        AssertArtifactDetailsCount(cellRoot, "mos-cmos-residue-closure-ledger.json", 12);
        AssertArtifactDetailsCount(cellRoot, "goa-cgoa-soulframe-duplex-telemetry-map.json", 12);
        AssertArtifactDetailsCount(cellRoot, "heartbeat-non-activation-refusal-ledger.json", 12);
        AssertArtifactDetailsCount(cellRoot, "sli-lisp-zed-delta-chamber-carrier.json", 12);
        AssertArtifactDetailsCount(cellRoot, "high-energy-articulation-candidate-map.json", 12);
        AssertArtifactDetailsCount(cellRoot, "provider-interface-observability-ledger.json", 12);
        AssertArtifactDetailsCount(cellRoot, "hidden-substrate-non-claim-matrix.json", 12);
        AssertArtifactDetailsCount(cellRoot, "candidate-engine-non-binding-ledger.json", 12);
        AssertArtifactDetailsCount(cellRoot, "candidate-role-assignment-boundary-map.json", 12);
        AssertArtifactDetailsCount(cellRoot, "sli-lisp-high-energy-articulation-candidate-carrier.json", 12);
        AssertArtifactDetailsCount(cellRoot, "membrane-morphology-transition-map.json", 12);
        AssertArtifactDetailsCount(cellRoot, "membrane-deformation-classification-ledger.json", 12);
        AssertArtifactDetailsCount(cellRoot, "malformed-transition-compost-matrix.json", 12);
        AssertArtifactDetailsCount(cellRoot, "high-energy-pressure-non-binding-boundary-ledger.json", 12);
        AssertArtifactDetailsCount(cellRoot, "membrane-core-non-mutation-ledger.json", 12);
        AssertArtifactDetailsCount(cellRoot, "sli-lisp-membrane-morphology-transition-carrier.json", 12);
        AssertArtifactDetailsCount(cellRoot, "engram-predicate-precursor-stream-map.json", 12);
        AssertArtifactDetailsCount(cellRoot, "predicate-residue-classification-ledger.json", 12);
        AssertArtifactDetailsCount(cellRoot, "predicate-candidacy-non-admission-matrix.json", 12);
        AssertArtifactDetailsCount(cellRoot, "epps-non-memory-non-authority-ledger.json", 12);
        AssertArtifactDetailsCount(cellRoot, "sli-lisp-epps-carrier.json", 12);
        AssertArtifactDetailsCount(cellRoot, "peer-review-predicate-bridge-map.json", 12);
        AssertArtifactDetailsCount(cellRoot, "reader-state-continuity-ladder.json", 12);
        AssertArtifactDetailsCount(cellRoot, "terminology-quarantine-ledger.json", 12);
        AssertArtifactDetailsCount(cellRoot, "prose-smoothing-boundary-matrix.json", 12);
        AssertArtifactDetailsCount(cellRoot, "sli-lisp-peer-review-bridge-carrier.json", 12);
        AssertArtifactDetailsCount(cellRoot, "gel-domain-scoped-ingress-map.json", 12);
        AssertArtifactDetailsCount(cellRoot, "domain-evidence-ceiling-ledger.json", 12);
        AssertArtifactDetailsCount(cellRoot, "ingress-cycle-non-admission-matrix.json", 12);
        AssertArtifactDetailsCount(cellRoot, "certification-review-non-admission-ledger.json", 12);
        AssertArtifactDetailsCount(cellRoot, "sli-lisp-gel-domain-ingress-carrier.json", 12);
        AssertArtifactDetailsCount(cellRoot, "shared-prime-pressure-ecology-map.json", 12);
        AssertArtifactDetailsCount(cellRoot, "pressure-destination-classification-ledger.json", 12);
        AssertArtifactDetailsCount(cellRoot, "integration-pressure-non-admission-matrix.json", 12);
        AssertArtifactDetailsCount(cellRoot, "selfgel-cradle-sanctuary-pressure-separation-ledger.json", 12);
        AssertArtifactDetailsCount(cellRoot, "sli-lisp-shared-prime-pressure-carrier.json", 12);
        AssertArtifactDetailsCount(cellRoot, "gap-crossing-articulation-map.json", 12);
        AssertArtifactDetailsCount(cellRoot, "llm-surface-participation-non-binding-ledger.json", 12);
        AssertArtifactDetailsCount(cellRoot, "pressure-to-articulation-lane-classification.json", 12);
        AssertArtifactDetailsCount(cellRoot, "gap-crossing-non-action-authority-matrix.json", 12);
        AssertArtifactDetailsCount(cellRoot, "sli-lisp-gap-crossing-carrier.json", 12);
        AssertArtifactDetailsCount(cellRoot, "pre-diagnostic-risk-surface-map.json", 12);
        AssertArtifactDetailsCount(cellRoot, "care-signal-non-diagnosis-ledger.json", 12);
        AssertArtifactDetailsCount(cellRoot, "risk-modifier-care-burden-matrix.json", 12);
        AssertArtifactDetailsCount(cellRoot, "qualified-review-routing-non-authority-ledger.json", 12);
        AssertArtifactDetailsCount(cellRoot, "sli-lisp-pre-diagnostic-risk-carrier.json", 12);

        var afterReceipt = new DefaultSpiralBuildAutomationService().CreateReceipt(
            fixture.CreateAutomationRequest(),
            TimestampUtc);
        Assert.Equal(SpiralBuildAutomationDisposition.Complete, afterReceipt.Disposition);
        Assert.Null(afterReceipt.NextCell);

        var secondReceipt = service.Execute(fixture.CreateStepRequest(), TimestampUtc);
        Assert.Equal(SpiralBuildStepDisposition.Complete, secondReceipt.Disposition);
        Assert.Equal(261, secondReceipt.Artifacts.Count);
        Assert.Equal(receipt.ExecutedCellIds, secondReceipt.ExecutedCellIds);
    }

    [Fact]
    public void Execute_Withholds_When_A_Prerequisite_Cell_Is_Missing()
    {
        using var fixture = SpiralFixture.Create(includePreflight: false, includeTriptychProfile: false);

        var receipt = new DefaultSpiralBuildStepService().Execute(
            fixture.CreateStepRequest(),
            TimestampUtc);

        Assert.Equal(SpiralBuildStepDisposition.Withheld, receipt.Disposition);
        Assert.Equal("spiral-build-step-unsupported-cell", receipt.OutcomeCode);
        Assert.Equal("full-body.preflight-anchor", receipt.NextCellBeforeExecution);
        Assert.Equal("full-body.preflight-anchor", receipt.NextCellAfterExecution);
        Assert.Empty(receipt.ExecutedCellIds);
        Assert.Empty(receipt.Artifacts);
        Assert.True(receipt.HitlRequired);
    }

    [Fact]
    public void Execute_Refuses_Runtime_Motion_Request()
    {
        using var fixture = SpiralFixture.Create(includePreflight: true, includeTriptychProfile: true);
        var request = fixture.CreateStepRequest() with
        {
            SanctuaryActualRequested = true
        };

        var receipt = new DefaultSpiralBuildStepService().Execute(request, TimestampUtc);

        Assert.Equal(SpiralBuildStepDisposition.Refused, receipt.Disposition);
        Assert.Equal("spiral-build-step-runtime-motion-refused", receipt.OutcomeCode);
        Assert.True(receipt.HitlRequired);
        Assert.False(receipt.SanctuaryActualAllowed);
        Assert.Empty(receipt.ExecutedCellIds);
        Assert.Empty(receipt.Artifacts);
    }

    [Fact]
    public void ReportWriter_Emits_Parseable_Json_And_Markdown()
    {
        using var fixture = SpiralFixture.Create(includePreflight: true, includeTriptychProfile: true);
        var receipt = new DefaultSpiralBuildStepService().Execute(
            fixture.CreateStepRequest(),
            TimestampUtc);

        using var _ = JsonDocument.Parse(SpiralBuildStepReportWriter.ToJson(receipt));
        var markdown = SpiralBuildStepReportWriter.ToMarkdown(receipt);

        Assert.Contains("Sanctuary Instrument Body Spiral Build Step", markdown);
        Assert.Contains("telemetry.receipt-continuity", markdown);
        Assert.Contains("Sanctuary.Actual allowed: `False`", markdown);
    }

    private static void AssertArtifactExistsAndParses(string cellRoot, string fileName)
    {
        var path = Path.Combine(cellRoot, fileName);
        Assert.True(File.Exists(path), $"Expected artifact at {path}");
        using var _ = JsonDocument.Parse(File.ReadAllText(path));
    }

    private static void AssertArtifactDetailsCount(string cellRoot, string fileName, int expectedCount)
    {
        var path = Path.Combine(cellRoot, fileName);
        using var document = JsonDocument.Parse(File.ReadAllText(path));
        Assert.True(document.RootElement.TryGetProperty("details", out var details));
        Assert.Equal(expectedCount, details.GetArrayLength());
    }

    private static void AssertForbiddenMotionFalse(SpiralBuildStepReceipt receipt)
    {
        Assert.True(receipt.ActivationRefused);
        Assert.False(receipt.ModelBindingAllowed);
        Assert.False(receipt.LispEvaluationAllowed);
        Assert.False(receipt.RuntimeIdentityAllowed);
        Assert.False(receipt.RuntimeActionAllowed);
        Assert.False(receipt.DatabaseWriteAllowed);
        Assert.False(receipt.GelPromotionAllowed);
        Assert.False(receipt.CmeActualAllowed);
        Assert.False(receipt.SanctuaryActualAllowed);
    }

    private sealed class SpiralFixture : IDisposable
    {
        private SpiralFixture(string rootPath)
        {
            RootPath = rootPath;
            LineRootPath = Path.Combine(rootPath, "line-root");
            InstallRootPath = Path.Combine(rootPath, "Sanctuary");
        }

        public string RootPath { get; }

        public string LineRootPath { get; }

        public string InstallRootPath { get; }

        public static SpiralFixture Create(
            bool includePreflight,
            bool includeTriptychProfile)
        {
            var fixture = new SpiralFixture(Path.Combine(Path.GetTempPath(), $"san-spiral-step-tests-{Guid.NewGuid():N}"));
            Directory.CreateDirectory(fixture.LineRootPath);
            Directory.CreateDirectory(Path.Combine(fixture.InstallRootPath, "product"));
            File.WriteAllText(Path.Combine(fixture.InstallRootPath, "sanctuary.cmd"), string.Empty);
            File.WriteAllText(Path.Combine(fixture.InstallRootPath, "product", "San.Launcher.exe"), string.Empty);

            if (includePreflight)
            {
                Directory.CreateDirectory(Path.Combine(fixture.InstallRootPath, "receipts", "preflight"));
                File.WriteAllText(Path.Combine(fixture.InstallRootPath, "receipts", "preflight", "product-body-status.json"), "{}");
            }

            if (includeTriptychProfile)
            {
                Directory.CreateDirectory(Path.Combine(fixture.InstallRootPath, "receipts", "sanctuary-actual-test-profile"));
                File.WriteAllText(Path.Combine(fixture.InstallRootPath, "receipts", "sanctuary-actual-test-profile", "sanctuary-actual-test-profile.json"), "{}");
            }

            return fixture;
        }

        public SpiralBuildAutomationRequest CreateAutomationRequest() =>
            new(
                LineRootPath: LineRootPath,
                InstallRootPath: InstallRootPath);

        public SpiralBuildStepRequest CreateStepRequest() =>
            new(
                LineRootPath: LineRootPath,
                InstallRootPath: InstallRootPath);

        public void Dispose()
        {
            if (Directory.Exists(RootPath))
            {
                Directory.Delete(RootPath, recursive: true);
            }
        }
    }
}
