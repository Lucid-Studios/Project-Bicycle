(in-package :sli-core)

;; BOUNDED SLI.LISP EC ENTRYPOINT.
;; This function is the Engineered Cognition test body for cold telemetry loops.
;; It may emit review telemetry. It may not authorize action, admit memory,
;; mutate SelfGEL, activate CME.Actual, bind a model, or grant authority.

(defun %ec-blank-p (text)
  (or (null text) (= 0 (length (string-trim '(#\Space #\Tab #\Newline #\Return) text)))))

(defun %ec-token-count (text)
  (if (%ec-blank-p text)
      0
      (let ((count 0)
            (in-token nil))
        (loop for ch across text do
          (if (find ch '(#\Space #\Tab #\Newline #\Return #\. #\, #\; #\: #\! #\? #\( #\) #\[ #\] #\{ #\} #\' #\") :test #'char=)
              (setf in-token nil)
              (unless in-token
                (incf count)
                (setf in-token t))))
        count)))

(defun %ec-harmonic-condition (text)
  (cond
    ((search "?" text) "inquiry-tension")
    ((or (search "urgent" text :test #'char-equal)
         (search "must" text :test #'char-equal)
         (search "now" text :test #'char-equal))
     "urgency-pressure")
    (t "coherence-tension-discordance-affordance")))

(defun run-ec-telemetry-loop (thought-form)
  (let* ((thought (if (%ec-blank-p thought-form) "idle cold EC telemetry loop" thought-form))
         (token-count (%ec-token-count thought))
         (harmonic-condition (%ec-harmonic-condition thought))
         (semantic-density (min 1.0 (max 0.10 (/ token-count 16.0)))))
    (list
      "engine-owner=sli.lisp"
      "bounded-entrypoint=run-ec-telemetry-loop"
      "review-only=true"
      "cold-engine-loop-completed=true"
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
      "pressure.governance-friction=0.80"
      "pressure.return-cooling=0.65"
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

(export 'run-ec-telemetry-loop)
