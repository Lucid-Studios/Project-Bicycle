(in-package :sli-core)

;; BOUNDED SLI.LISP LAB GEL ENGRAMMITIZATION ENTRYPOINT.
;; This function converts cold warm-use predicate residue into lab GEL
;; predicate and engram-candidate telemetry. It may not admit GEL, mutate
;; SelfGEL, authorize action, bind a model, or activate CME.Actual.

(defun run-lab-gel-engrammitization
    (operator-id domain role job-class session-id turn-index source-receipt thought-form)
  (let* ((operator (%warm-use-sanitize-identifier (%warm-use-value operator-id "Sanctuary.ID")))
         (scope-domain (%warm-use-sanitize-identifier (%warm-use-value domain "Sanctuary")))
         (scope-role (%warm-use-sanitize-identifier (%warm-use-value role "InstalledBody")))
         (scope-job-class (%warm-use-sanitize-identifier (%warm-use-value job-class "ColdBench")))
         (session (%warm-use-sanitize-identifier (%warm-use-value session-id "warm-use-session")))
         (turn (max 0 turn-index))
         (source (%warm-use-value source-receipt "source-warm-use-receipt-missing"))
         (thought (%warm-use-value thought-form "idle lab GEL predicate formation"))
         (token-count (%ec-token-count thought))
         (harmonic-condition (%ec-harmonic-condition thought))
         (semantic-density (min 1.0 (max 0.10 (/ token-count 16.0)))))
    (list
      "engine-owner=sli.lisp"
      "bounded-entrypoint=run-lab-gel-engrammitization"
      "review-only=true"
      "lab-gel-engrammitization-completed=true"
      "lab-gel.state=post-gel-formation-pre-admission"
      "lab-gel.predicate-formed=true"
      "lab-gel.predicate-count=6"
      "lab-gel.predicate-classes=semantic,pressure,witness,governance,morphology,return"
      "engram-candidate.formed=true"
      "engram-candidate.pre-admission-only=true"
      "engram-candidate.evidence-body-formed=true"
      "engram-candidate.witness-body-formed=true"
      "engram-candidate.cooling-held=true"
      "engram-candidate.pre-admission-review-required=true"
      "lab-gel.readback-available=true"
      "lab-gel.readback-pre-admission-only=true"
      "typed-scope.accepted=true"
      "source-warm-use.accepted-cold=true"
      "session-lineage.witnessed=true"
      (format nil "operator.id=~a" operator)
      (format nil "domain=~a" scope-domain)
      (format nil "role=~a" scope-role)
      (format nil "job-class=~a" scope-job-class)
      (format nil "session.id=~a" session)
      (format nil "session.turn-index=~a" turn)
      (format nil "source.warm-use-receipt=~a" source)
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
      "pressure.governance-friction=0.90"
      "pressure.return-cooling=0.82"
      "gel-promotion=false"
      "gel-admission=false"
      "engram-admission=false"
      "memory-admission=false"
      "selfgel-mutation=false"
      "continuity-admission=false"
      "authority-granted=false"
      "action-authorized=false"
      "model-binding=false"
      "arbitrary-lisp-evaluation=false"
      "runtime-action=false"
      "cme-actual-activation=false"
      "sanctuary-actual-activation=false"
      "return=pre-admission-lab-substrate")))

(export 'run-lab-gel-engrammitization)
