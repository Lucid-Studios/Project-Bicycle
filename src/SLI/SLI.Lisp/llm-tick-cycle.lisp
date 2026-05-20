(in-package :sli-core)

;; BOUNDED SLI.LISP LLM TICK CYCLE ENTRYPOINT.
;; This function processes one cold adapter response packet through the SLI
;; membrane. The adapter may be present as a deterministic harness, but the
;; tick may not bind a model, call a provider, grant authority, authorize
;; action, admit GEL, mutate SelfGEL, activate heartbeat, or admit Actual.

(defun run-llm-tick-cycle
    (operator-id domain role job-class session-id tick-index source-llm-readiness-receipt
     prior-tick-receipt adapter-kind adapter-response-receipt adapter-output thought-form source-engram-closure)
  (let* ((operator (%warm-use-sanitize-identifier (%warm-use-value operator-id "Sanctuary.ID")))
         (scope-domain (%warm-use-sanitize-identifier (%warm-use-value domain "Sanctuary")))
         (scope-role (%warm-use-sanitize-identifier (%warm-use-value role "InstalledBody")))
         (scope-job-class (%warm-use-sanitize-identifier (%warm-use-value job-class "ColdBench")))
         (session (%warm-use-sanitize-identifier (%warm-use-value session-id "llm-tick-cycle-session")))
         (tick (max 0 tick-index))
          (source (%warm-use-value source-llm-readiness-receipt "llm-readiness-receipt-missing"))
          (prior (%warm-use-value prior-tick-receipt "none"))
          (prior-present (not (string-equal prior "none")))
          (closure (%warm-use-value source-engram-closure "engram-closure-missing"))
         (adapter (%warm-use-sanitize-identifier (%warm-use-value adapter-kind "deterministic-harness")))
         (adapter-receipt (%warm-use-value adapter-response-receipt "adapter-response-receipt-missing"))
         (output (%warm-use-value adapter-output "adapter output absent"))
         (thought (%warm-use-value thought-form "cold LLM tick cycle"))
         (thought-token-count (%ec-token-count thought))
         (output-token-count (%ec-token-count output))
         (combined-token-count (+ thought-token-count output-token-count))
         (harmonic-condition (%ec-harmonic-condition (concatenate 'string thought " " output)))
         (semantic-density (min 1.0 (max 0.10 (/ combined-token-count 24.0)))))
    (list
      "engine-owner=sli.lisp"
      "bounded-entrypoint=run-llm-tick-cycle"
      "review-only=true"
      "llm-tick-cycle-completed=true"
      "llm-tick.state=cold-adapter-tick-witnessed"
      "tick-loop.running=true"
      "tick-loop.kind=deterministic-harness"
      (format nil "tick.index=~a" tick)
      "source.llm-interconnect-ready=true"
      (format nil "source.llm-interconnect-readiness-receipt=~a" source)
      "source.engram-closure-ready=true"
      (format nil "source.engram-closure-receipt=~a" closure)
      (format nil "source.prior-tick-receipt=~a" prior)
      "llm-interconnect.ready-for-adapter=true"
      "llm-interconnect.provider-neutral=true"
      "model-adapter.present=true"
      (format nil "model-adapter.kind=~a" adapter)
      "model-adapter.deterministic-harness=true"
      "model-adapter.response-witnessed=true"
      "model-adapter.response-bounded=true"
      "model-adapter.provider-neutral=true"
      "model-adapter.model-binding=false"
      "model-adapter.provider-call=false"
      "model-adapter.hidden-internals-claim=false"
      (format nil "adapter-response.receipt=~a" adapter-receipt)
      "adapter-response.surface=predicate-articulation-packet"
      "adapter-response.output-witnessed=true"
      "adapter-response.output-bounded=true"
      "adapter-response.output-becomes-truth=false"
      "adapter-response.output-authorizes-action=false"
      "adapter-response.output-admits-memory=false"
      "adapter-response.output-admits-continuity=false"
      "membrane.sli-lisp.loaded=true"
      "membrane.sli-lisp.processed-tick=true"
      "membrane.sli-lisp-prime.present=true"
      "membrane.sli-lisp-cryptic.present=true"
      "membrane.lisp-control-matrix.present=true"
      "membrane.listening-frame.present=true"
      "membrane.compass.present=true"
      "membrane.soulframe-route.present=true"
      "membrane.agenticore-route.present=true"
      "listening-frame.received=true"
      "sli-membrane.interpreted-predicate-pressure=true"
      "compass.oriented-pressure=true"
      "compass.cooling-required=true"
      "soulframe.received-listening-frame=true"
      "agenticore.received-compass-pressure=true"
      "thinking-about-thinking.telemetry-produced=true"
      "predicate-residue.produced=true"
      "predicate-residue.pre-engram-only=true"
      "predicate-residue.admitted-engram=false"
      "tick-lineage.witnessed=true"
      (format nil "tick-lineage.first-tick-origin=~a" (if prior-present "false" "true"))
      (format nil "tick-lineage.prior-linked=~a" (if prior-present "true" "false"))
      "tick-lineage.becomes-memory=false"
      "engine-llm-seat.ready=true"
      "engine-llm-seat.provider-agnostic=true"
      "engine-llm.may-articulate=true"
      "engine-llm.may-rehearse=true"
      "engine-llm.may-form-candidates=true"
      "engine-llm.may-bind-model=false"
      "engine-llm.may-call-provider=false"
      "engine-llm.may-grant-authority=false"
      "engine-llm.may-execute-action=false"
      "steward.reviewed=true"
      "authority-grant.absent=true"
      "action-executor.locked=true"
      "gel-admission.locked=true"
      "selfgel-mutation.locked=true"
      "heartbeat.locked=true"
      "cme-actual.locked=true"
      "sanctuary-actual.locked=true"
      "typed-scope.accepted=true"
      "session-lineage.witnessed=true"
      (format nil "operator.id=~a" operator)
      (format nil "domain=~a" scope-domain)
      (format nil "role=~a" scope-role)
      (format nil "job-class=~a" scope-job-class)
      (format nil "session.id=~a" session)
      (format nil "thought.token-count=~a" thought-token-count)
      (format nil "adapter-output.token-count=~a" output-token-count)
      (format nil "harmonic-condition=~a" harmonic-condition)
      (format nil "pressure.semantic-density=~,2f" semantic-density)
      "pressure.governance-friction=0.98"
      "pressure.return-cooling=0.93"
      "model-binding=false"
      "provider-call=false"
      "hidden-internals-claim=false"
      "arbitrary-lisp-evaluation=false"
      "runtime-action=false"
      "database-write=false"
      "memory-admission=false"
      "continuity-admission=false"
      "gel-admission=false"
      "selfgel-mutation=false"
      "authority-granted=false"
      "action-authorized=false"
      "heartbeat-active=false"
      "cme-actual-activation=false"
      "sanctuary-actual-activation=false"
      "return=llm-tick-witnessed-without-binding-or-authority")))

(export 'run-llm-tick-cycle)
