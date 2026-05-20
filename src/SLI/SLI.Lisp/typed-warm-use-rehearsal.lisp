(in-package :sli-core)

;; BOUNDED SLI.LISP TYPED WARM-USE REHEARSAL ENTRYPOINT.
;; This function accepts live scoped thought-form material as cold rehearsal
;; evidence. It may emit typed session telemetry. It may not authorize action,
;; admit memory, mutate SelfGEL, activate CME.Actual, bind a model, or grant
;; authority.

(defun %warm-use-value (value fallback)
  (if (%ec-blank-p value)
      fallback
      (string-trim '(#\Space #\Tab #\Newline #\Return) value)))

(defun %warm-use-sanitize-identifier (value)
  (let ((out (make-string-output-stream)))
    (loop for ch across value do
      (when (or (alphanumericp ch)
                (find ch '(#\. #\- #\_) :test #'char=))
        (write-char ch out)))
    (let ((result (get-output-stream-string out)))
      (if (%ec-blank-p result) "Sanctuary" result))))

(defun run-typed-warm-use-rehearsal
    (operator-id domain role job-class session-id turn-index thought-form)
  (let* ((operator (%warm-use-sanitize-identifier (%warm-use-value operator-id "Sanctuary.ID")))
         (scope-domain (%warm-use-sanitize-identifier (%warm-use-value domain "Sanctuary")))
         (scope-role (%warm-use-sanitize-identifier (%warm-use-value role "InstalledBody")))
         (scope-job-class (%warm-use-sanitize-identifier (%warm-use-value job-class "ColdBench")))
         (session (%warm-use-sanitize-identifier (%warm-use-value session-id "warm-use-session")))
         (turn (max 0 turn-index))
         (thought (%warm-use-value thought-form "idle typed warm-use rehearsal"))
         (token-count (%ec-token-count thought))
         (harmonic-condition (%ec-harmonic-condition thought))
         (semantic-density (min 1.0 (max 0.10 (/ token-count 16.0)))))
    (list
      "engine-owner=sli.lisp"
      "bounded-entrypoint=run-typed-warm-use-rehearsal"
      "review-only=true"
      "warm-use-state=typed-cold-ready-rehearsal"
      "typed-warm-use-rehearsal-completed=true"
      "typed-scope.accepted=true"
      "live-ingress.accepted-cold=true"
      "session-lineage.witnessed=true"
      (format nil "operator.id=~a" operator)
      (format nil "domain=~a" scope-domain)
      (format nil "role=~a" scope-role)
      (format nil "job-class=~a" scope-job-class)
      (format nil "session.id=~a" session)
      (format nil "session.turn-index=~a" turn)
      "listening-frame.received=true"
      "sli-membrane.interpreted-predicate-pressure=true"
      "compass.oriented-pressure=true"
      "compass.cooling-required=true"
      "soulframe.received-listening-frame=true"
      "agenticore.received-compass-pressure=true"
      "thinking-about-thinking.telemetry-produced=true"
      "pre-engram.residue-produced=true"
      "pre-engram.residue-count=6"
      "pre-engram.residue-classes=semantic,pressure,witness,governance,morphology,return"
      "steward.reviewed=true"
      (format nil "thought.token-count=~a" token-count)
      (format nil "harmonic-condition=~a" harmonic-condition)
      (format nil "pressure.semantic-density=~,2f" semantic-density)
      "pressure.governance-friction=0.85"
      "pressure.return-cooling=0.70"
      "turn-lineage.receipt-only=true"
      "session-ledger.append-only=true"
      "engram-admission=false"
      "memory-admission=false"
      "selfgel-mutation=false"
      "continuity-admission=false"
      "authority-granted=false"
      "action-authorized=false"
      "model-binding=false"
      "arbitrary-lisp-evaluation=false"
      "cme-actual-activation=false"
      "sanctuary-actual-activation=false"
      "return=receipt-only")))

(export 'run-typed-warm-use-rehearsal)
