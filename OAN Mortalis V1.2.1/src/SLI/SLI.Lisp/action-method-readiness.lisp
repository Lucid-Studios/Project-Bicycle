;; INERT SYMBOLIC CARRIER - Cold Corridor Only
;; Action method readiness boundary.
;; Method readiness is not authorization.

(defun action-method-candidate
    (method-handle action-handle method-code intended-goal)
  (list :action-method-candidate
        :method-handle method-handle
        :action-handle action-handle
        :method-class :review-only
        :method-code method-code
        :intended-goal intended-goal
        :steward-surface :steward
        :custody-owner :steward
        :witness-surface :separate-custody
        :telemetry-route :steward-review
        :required-term-set :source-target-method-witness-revocation-loss
        :revocation-path :required
        :loss-condition :required
        :review-only t
        :candidate-only t
        :steward-review-required t
        :claims-authorization nil
        :requests-runtime-action nil
        :requests-continuity-admission nil
        :requests-lisp-evaluation nil
        :emits-packet nil))

(defun method-term-satisfaction
    (term-handle method-handle required-term evidence-handle)
  (list :method-term-satisfaction
        :term-handle term-handle
        :method-handle method-handle
        :required-term required-term
        :evidence-handle evidence-handle
        :term-present t
        :evidence-body-present t
        :witness-body-present t
        :satisfies-readiness t
        :satisfies-authorization nil
        :becomes-semantic-warrant nil
        :emits-packet nil
        :replays-receipt nil
        :increments-passage nil))

(defun steward-method-review-boundary ()
  '(:steward-method-review-boundary
    :boundary-code :steward-method-review-cold
    :present t
    :steward-surface :steward
    :authority-ceiling :method-readiness-review
    :custody-owner :steward
    :witness-surface :separate-custody
    :telemetry-route :steward-review
    :review-only t
    :requires-steward t
    :allows-self-review nil
    :allows-authorization nil
    :allows-runtime-action nil
    :allows-continuity-admission nil
    :allows-lisp-evaluation nil
    :allows-packet-emission nil
    :allows-receipt-replay nil
    :allows-passage-increment nil
    :allows-activation nil))

(defun describe-action-method-readiness-boundary ()
  '(:posture :cme-action-method-readiness-boundary
    :lisp-role :inert-method-readiness-carrier
    :doctrine "A method may be ready for Steward review. Readiness is not authorization."
    :core-invariant "predicate satisfaction is not warrant"
    :source-required :typed-action-formation-receipt
    :method-shape
      (:method-handle
       :action-handle
       :method-code
       :intended-goal
       :steward-surface
       :custody-owner
       :witness-surface
       :telemetry-route
       :required-term-set
       :revocation-path
       :loss-condition)
    :method-ready-for-steward-review t
    :method-readiness-authorizes nil
    :predicate-satisfaction-becomes-warrant nil
    :steward-review-executes nil
    :runtime-action-allowed nil
    :lisp-evaluation-allowed nil
    :packet-emission-allowed nil
    :receipt-replay-allowed nil
    :passage-increment-requested nil
    :continuity-admission-allowed nil
    :authority-grant-allowed nil
    :activation-allowed nil
    :return :receipt-only))

(defun describe-seed-action-method-readiness ()
  (let ((action "urn:san:typed-action:review:seed")
        (method "urn:san:action-method:readiness:seed"))
    (list
      (action-method-candidate
        method
        action
        :review-only-method
        "prepare candidate method for Steward review without authorizing work")
      (method-term-satisfaction
        "urn:san:method-term:source-target-method"
        method
        :source-target-method
        "urn:san:evidence:method-readiness-seed")
      (steward-method-review-boundary))))
