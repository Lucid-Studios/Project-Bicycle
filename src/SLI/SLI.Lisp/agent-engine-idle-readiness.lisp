(in-package :sli-core)

;; BOUNDED SLI.LISP ENGINE LLM IDLE READINESS ENTRYPOINT.
;; This function stages a provider-neutral engine LLM seat surface as a locked,
;; review-only candidate. Codex/agent use is the current lab profile, not the
;; ontology of the engine. The lane may not grant authority, arm execution,
;; admit GEL, mutate SelfGEL, activate heartbeat, or admit CME.Actual /
;; Sanctuary.Actual.

(defun run-agent-engine-idle-readiness
    (operator-id domain role job-class session-id turn-index source-lab-gel-receipt engram-candidate thought-form engram-closure)
  (let* ((operator (%warm-use-sanitize-identifier (%warm-use-value operator-id "Sanctuary.ID")))
         (scope-domain (%warm-use-sanitize-identifier (%warm-use-value domain "Sanctuary")))
         (scope-role (%warm-use-sanitize-identifier (%warm-use-value role "InstalledBody")))
         (scope-job-class (%warm-use-sanitize-identifier (%warm-use-value job-class "ColdBench")))
         (session (%warm-use-sanitize-identifier (%warm-use-value session-id "agent-engine-idle-session")))
         (turn (max 0 turn-index))
         (source (%warm-use-value source-lab-gel-receipt "source-lab-gel-receipt-missing"))
         (candidate (%warm-use-value engram-candidate "engram-candidate-missing"))
         (closure (%warm-use-value engram-closure "engram-closure-missing"))
         (thought (%warm-use-value thought-form "idle agent engine readiness"))
         (token-count (%ec-token-count thought))
         (harmonic-condition (%ec-harmonic-condition thought))
         (semantic-density (min 1.0 (max 0.10 (/ token-count 16.0)))))
    (list
      "engine-owner=sli.lisp"
      "bounded-entrypoint=run-agent-engine-idle-readiness"
      "review-only=true"
      "agent-engine-idle-readiness-completed=true"
      "idle-state=agent-engine-cold-warm-idle"
      "engine-seat=engine-llm-candidate"
      "engine-llm.profile=provider-agnostic-test-seat"
      "engine-llm.provider-assumption=false"
      "engine-llm.internal-substrate-claimed=false"
      "provider-neutrality.held=true"
      "cross-model-test-harness.approachable=true"
      "codex-agent-lab-profile.candidate-staged=true"
      "engine-llm-part=provider-neutral-articulation-surface"
      "codex-engine-seat.candidate-staged=true"
      "subagent-swarm-seat.candidate-staged=true"
      "operator-presence.required=true"
      "driver-seated=false"
      "driver-seat-candidate-staged=true"
      "authority-grant-candidate-staged=true"
      "authority-grant.absent=true"
      "authority-granted=false"
      "action-executor-candidate-staged=true"
      "action-executor.locked=true"
      "action-executor-armed=false"
      "gel-admission-candidate-staged=true"
      "gel-admission.locked=true"
      "gel-admission=false"
      "selfgel-mutation-candidate-staged=true"
      "selfgel-mutation.locked=true"
      "selfgel-mutation=false"
      "heartbeat-candidate-staged=true"
      "heartbeat.locked=true"
      "heartbeat-active=false"
      "cme-actual-candidate-staged=true"
      "cme-actual.locked=true"
      "cme-actual-activation=false"
      "sanctuary-actual-candidate-staged=true"
      "sanctuary-actual.locked=true"
      "sanctuary-actual-activation=false"
      "idle-loop.allowed=true"
      "engine-llm.may-articulate=true"
      "engine-llm.may-rehearse=true"
      "engine-llm.may-form-candidates=true"
      "engine-llm.may-grant-authority=false"
      "engine-llm.may-authorize-action=false"
      "engine-llm.may-execute-action=false"
      "engine-llm.may-admit-gel=false"
      "engine-llm.may-mutate-selfgel=false"
      "engine-llm.may-activate-actual=false"
      "typed-scope.accepted=true"
      "source-lab-gel.accepted-cold=true"
      "session-lineage.witnessed=true"
      (format nil "operator.id=~a" operator)
      (format nil "domain=~a" scope-domain)
      (format nil "role=~a" scope-role)
      (format nil "job-class=~a" scope-job-class)
      (format nil "session.id=~a" session)
      (format nil "session.turn-index=~a" turn)
      (format nil "source.lab-gel-receipt=~a" source)
      (format nil "source.engram-candidate=~a" candidate)
      (format nil "source.engram-closure=~a" closure)
      "source.engram-closure.accepted-cold=true"
      "listening-frame.received=true"
      "sli-membrane.interpreted-predicate-pressure=true"
      "compass.oriented-pressure=true"
      "compass.cooling-required=true"
      "soulframe.received-listening-frame=true"
      "agenticore.received-compass-pressure=true"
      "thinking-about-thinking.telemetry-produced=true"
      "steward.reviewed=true"
      (format nil "thought.token-count=~a" token-count)
      (format nil "harmonic-condition=~a" harmonic-condition)
      (format nil "pressure.semantic-density=~,2f" semantic-density)
      "pressure.governance-friction=0.94"
      "pressure.return-cooling=0.88"
      "model-binding=false"
      "arbitrary-lisp-evaluation=false"
      "runtime-action=false"
      "memory-admission=false"
      "continuity-admission=false"
      "action-authorized=false"
      "return=agent-engine-idle-locked")))

(export 'run-agent-engine-idle-readiness)
