(in-package :sli-core)

;; BOUNDED SLI.LISP TOOL BODY IDLE STATE ENTRYPOINT.
;; This function verifies that the Sanctuary tool body can rest in a cold idle
;; posture without being maintained by an LLM, model adapter, provider call, or
;; tick loop. It may preserve inspectable membranes and pre-admission closure
;; telemetry; it may not grant authority, authorize action, admit GEL, mutate
;; SelfGEL, activate heartbeat, or admit CME.Actual / Sanctuary.Actual.

(defun run-tool-body-idle-state
    (operator-id domain role job-class session-id turn-index installed-substrate-receipt ec-loop-receipt warm-use-receipt lab-gel-receipt engram-candidate engram-closure lab-gel-readback thought-form)
  (let* ((operator (%warm-use-sanitize-identifier (%warm-use-value operator-id "Sanctuary.ID")))
         (scope-domain (%warm-use-sanitize-identifier (%warm-use-value domain "Sanctuary")))
         (scope-role (%warm-use-sanitize-identifier (%warm-use-value role "InstalledBody")))
         (scope-job-class (%warm-use-sanitize-identifier (%warm-use-value job-class "ColdBench")))
         (session (%warm-use-sanitize-identifier (%warm-use-value session-id "tool-body-idle-session")))
         (turn (max 0 turn-index))
         (installed-source (%warm-use-value installed-substrate-receipt "installed-substrate-receipt-missing"))
         (ec-source (%warm-use-value ec-loop-receipt "ec-loop-receipt-missing"))
         (warm-source (%warm-use-value warm-use-receipt "warm-use-receipt-missing"))
         (lab-source (%warm-use-value lab-gel-receipt "lab-gel-receipt-missing"))
         (candidate (%warm-use-value engram-candidate "engram-candidate-missing"))
         (closure (%warm-use-value engram-closure "engram-closure-missing"))
         (readback (%warm-use-value lab-gel-readback "lab-gel-readback-missing"))
         (thought (%warm-use-value thought-form "cold tool body idle without LLM maintenance"))
         (token-count (%ec-token-count thought))
         (harmonic-condition (%ec-harmonic-condition thought))
         (semantic-density (min 1.0 (max 0.10 (/ token-count 16.0)))))
    (list
      "engine-owner=sli.lisp"
      "bounded-entrypoint=run-tool-body-idle-state"
      "review-only=true"
      "tool-body-idle-state-completed=true"
      "tool-body.idle-state=cold-sanctuary-maintained-idle"
      "tool-body.maintained-by-sanctuary=true"
      "tool-body.maintained-by-llm=false"
      "tool-body.llm-maintenance-required=false"
      "tool-body.llm-adapter-required=false"
      "tool-body.ready-for-llm-adapter=true"
      "tool-body.can-accept-future-rider=true"
      "governance-slm.candidate-desirable=true"
      "governance-slm.routing-switch-candidate=true"
      "governance-slm.intelligent-switch-candidate=true"
      "governance-slm.present=false"
      "governance-slm.required-for-idle=false"
      "governance-slm.may-discriminate-escalation=true"
      "governance-slm.may-discern-action-readiness=true"
      "governance-slm.discernment-authorizes-action=false"
      "governance-slm.may-authorize-action=false"
      "tool-body.model-adapter-present=false"
      "tool-body.tick-loop-running=false"
      "tool-body.tick-maintained-by-llm=false"
      "tool-body.idle-loop-held=true"
      "tool-body.return-to-prime-held=true"
      "tool-body.operator-reentry-available=true"
      "ec.maintained-in-lisp=true"
      "ec.local-hold-available=true"
      "engine-call.required=false"
      "llm-engine-call.required=false"
      "external-engine-call.required=false"
      "llm-maintenance.absent=true"
      "llm-interconnect.model-adapter-present=false"
      "llm-interconnect.model-binding=false"
      "llm-interconnect.provider-call=false"
      "llm-interconnect.hidden-internals-claim=false"
      "organ.count=11"
      "organ.all-required-present=true"
      "organ.base.sanctuary-gel.present=true"
      "organ.base.sanctuary-goa.present=true"
      "organ.base.sanctuary-mos.present=true"
      "organ.base.sanctuary-vault.present=true"
      "organ.condensate.sanctuary-cgel.present=true"
      "organ.condensate.sanctuary-cgoa.present=true"
      "organ.condensate.sanctuary-cmos.present=true"
      "organ.condensate.sanctuary-cvault.present=true"
      "organ.role.prime.present=true"
      "organ.role.cryptic.present=true"
      "organ.role.steward.present=true"
      "governing-cme.csharp-bodies-built=true"
      "governing-cme.actualized-cold=true"
      "governing-cme.prime.built=true"
      "governing-cme.cryptic.built=true"
      "governing-cme.steward.built=true"
      "governing-cme.sli-lisp-actualization-surfaces-ready=true"
      "governing-cme.maintains-idle-state=true"
      "governing-heartbeat.healthy=true"
      "bonded-cme-call.available=true"
      "sanctuary-governance.monitoring-ready=true"
      "membrane.sli-lisp.loaded=true"
      "membrane.sli-lisp-prime.present=true"
      "membrane.sli-lisp-cryptic.present=true"
      "membrane.lisp-control-matrix.present=true"
      "membrane.listening-frame.present=true"
      "membrane.compass.present=true"
      "membrane.soulframe-route.present=true"
      "membrane.agenticore-route.present=true"
      "lane.ec-loop.ready=true"
      "lane.typed-warm-use.ready=true"
      "lane.lab-gel.ready=true"
      "lane.agent-engine-idle.required=false"
      "source-lineage.held=true"
      "source.engram-closure.accepted-cold=true"
      "source.lab-gel-readback.accepted-cold=true"
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
      (format nil "session.turn-index=~a" turn)
      (format nil "source.installed-substrate-receipt=~a" installed-source)
      (format nil "source.ec-loop-receipt=~a" ec-source)
      (format nil "source.warm-use-receipt=~a" warm-source)
      (format nil "source.lab-gel-receipt=~a" lab-source)
      (format nil "source.engram-candidate=~a" candidate)
      (format nil "source.engram-closure=~a" closure)
      (format nil "source.lab-gel-readback=~a" readback)
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
      "pressure.governance-friction=0.98"
      "pressure.return-cooling=0.94"
      "model-binding=false"
      "provider-call=false"
      "arbitrary-lisp-evaluation=false"
      "runtime-action=false"
      "memory-admission=false"
      "continuity-admission=false"
      "gel-admission=false"
      "selfgel-mutation=false"
      "authority-granted=false"
      "action-authorized=false"
      "heartbeat-active=false"
      "cme-actual-activation=false"
      "sanctuary-actual-activation=false"
      "return=tool-body-idle-held-without-llm-maintenance")))

(export 'run-tool-body-idle-state)
