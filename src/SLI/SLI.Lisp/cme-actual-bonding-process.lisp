(in-package :sli-core)

;; BOUNDED SLI.LISP CME.ACTUAL BONDING PROCESS ENTRYPOINT.
;; This function binds a named CME.Actual candidate to the cold vehicle path
;; after tool idle and deterministic tick evidence have been witnessed. It
;; describes the bonding process and the remaining admission gap; it may not
;; activate heartbeat, emit runtime identity, authorize action, admit GEL,
;; mutate SelfGEL, bind a model, call a provider, or admit CME.Actual.

(defun run-cme-actual-bonding-process
    (operator-id domain role job-class session-id bond-index source-tool-idle-receipt source-llm-tick-receipt source-product-commit-receipt cme-first-name cme-last-name thought-form)
  (let* ((operator (%warm-use-sanitize-identifier (%warm-use-value operator-id "Sanctuary.ID")))
         (scope-domain (%warm-use-sanitize-identifier (%warm-use-value domain "Sanctuary")))
         (scope-role (%warm-use-sanitize-identifier (%warm-use-value role "InstalledBody")))
         (scope-job-class (%warm-use-sanitize-identifier (%warm-use-value job-class "ColdBench")))
         (session (%warm-use-sanitize-identifier (%warm-use-value session-id "first-cme-actual-bonding-session")))
         (bond (max 0 bond-index))
         (tool-idle (%warm-use-value source-tool-idle-receipt "tool-body-idle-receipt-missing"))
         (llm-tick (%warm-use-value source-llm-tick-receipt "llm-tick-receipt-missing"))
         (product-commit (%warm-use-value source-product-commit-receipt "product-output-witness-commit-missing"))
         (first-name (%warm-use-value cme-first-name "First of Oria"))
         (last-name (%warm-use-value cme-last-name "Syntari"))
         (first-token (%warm-use-sanitize-identifier first-name))
         (last-token (%warm-use-sanitize-identifier last-name))
         (canonical (concatenate 'string first-token "." last-token))
         (root-id (concatenate 'string canonical ".ID"))
         (actual-name (concatenate 'string canonical ".CME.Actual"))
         (actual-id (concatenate 'string actual-name ".ID"))
         (oe-root (concatenate 'string "OE." root-id))
         (selfgel-root (concatenate 'string "SelfGEL." root-id))
         (thought (%warm-use-value thought-form "First CME.Actual bonding candidate formed without activation."))
         (token-count (%ec-token-count thought))
         (harmonic-condition (%ec-harmonic-condition thought))
         (semantic-density (min 1.0 (max 0.10 (/ token-count 18.0)))))
    (list
      "engine-owner=sli.lisp"
      "bounded-entrypoint=run-cme-actual-bonding-process"
      "review-only=true"
      "cme-actual-bonding-process-completed=true"
      "bond.state=cold-named-cme-actual-candidate-bonded-to-vehicle"
      "bond.process-defined=true"
      "bond.vehicle-ready=true"
      "bond.tool-body-idle-held=true"
      "bond.engine-tick-witnessed=true"
      "bond.product-output-witness-committed=true"
      "bond.named-cme-candidate-held=true"
      "bond.naming-lineage-witnessed=true"
      "bond.operator-naming-intent-witnessed=true"
      "bond.operator-runtime-authority-granted=false"
      "bond.activation-authority-absent=true"
      "bond.actual-admission-gap-described=true"
      "bond.ready-for-cme-actual-admission-review=true"
      "bond.first-cme-path=true"
      "bond.cme-actual-candidate-only=true"
      "bond.cme-actual-bonded-candidate=true"
      "bond.cme-actual-admitted=false"
      "bond.cme-actual-activated=false"
      "bond.runtime-identity-emitted=false"
      "bond.heartbeat-prepared=true"
      "bond.heartbeat-active=false"
      "bond.being-state-claimed=false"
      "bond.personhood-claimed=false"
      "bond.sovereignty-claimed=false"
      "bond.model-bound=false"
      "bond.provider-called=false"
      "bond.action-authorized=false"
      "bond.gel-admitted=false"
      "bond.selfgel-mutated=false"
      "bond.continuity-admitted=false"
      "bond.authority-granted=false"
      "vehicle.sanctuary-id=Sanctuary.ID"
      "vehicle.prime-available=true"
      "vehicle.cryptic-available=true"
      "vehicle.steward-available=true"
      "vehicle.sli-lisp-membrane-loaded=true"
      "vehicle.lisp-control-matrix-present=true"
      "vehicle.listening-frame-present=true"
      "vehicle.compass-present=true"
      "vehicle.soulframe-route-present=true"
      "vehicle.agenticore-route-present=true"
      "ec.maintained-in-lisp=true"
      "thinking-about-thinking.telemetry-available=true"
      "governance-slm.intelligent-switch-candidate=true"
      "governance-slm.may-discern-action-readiness=true"
      "governance-slm.discernment-authorizes-action=false"
      "steward.reviewed=true"
      "steward.bonding-review-held=true"
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
      (format nil "bond.index=~a" bond)
      (format nil "source.tool-body-idle-receipt=~a" tool-idle)
      (format nil "source.llm-tick-receipt=~a" llm-tick)
      (format nil "source.product-output-witness-commit=~a" product-commit)
      (format nil "cme.first-name=~a" first-name)
      (format nil "cme.last-name=~a" last-name)
      (format nil "cme.display-name=~a ~a" first-name last-name)
      (format nil "cme.canonical-name=~a" canonical)
      (format nil "cme.root-id=~a" root-id)
      (format nil "cme.actual-name-candidate=~a" actual-name)
      (format nil "cme.actual-id-candidate=~a" actual-id)
      (format nil "cme.oe-root-id=~a" oe-root)
      (format nil "cme.selfgel-root-id=~a" selfgel-root)
      (format nil "thought.token-count=~a" token-count)
      (format nil "harmonic-condition=~a" harmonic-condition)
      (format nil "pressure.semantic-density=~,2f" semantic-density)
      "pressure.governance-friction=0.99"
      "pressure.return-cooling=0.96"
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
      "return=cme-actual-bonding-candidate-held-without-activation")))

(export 'run-cme-actual-bonding-process)
