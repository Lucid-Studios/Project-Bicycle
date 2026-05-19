;; INERT SYMBOLIC CARRIER - Cold Corridor Only
;; Steward action admissibility boundary.
;; Admissibility is not execution.

(defun steward-admissibility-predicate-result
    (predicate-handle method-handle action-handle predicate-code evidence-handle)
  (list :steward-admissibility-predicate-result
        :predicate-handle predicate-handle
        :method-handle method-handle
        :action-handle action-handle
        :predicate-code predicate-code
        :evidence-handle evidence-handle
        :evidence-body-present t
        :witness-body-present t
        :predicate-satisfied t
        :supports-admissibility t
        :grants-warrant nil
        :authorizes-execution nil
        :emits-packet nil
        :evaluates-lisp nil
        :replays-receipt nil
        :increments-passage nil
        :admits-continuity nil))

(defun steward-action-admissibility-decision
    (decision-handle method-handle action-handle steward-surface)
  (list :steward-action-admissibility-decision
        :decision-handle decision-handle
        :method-handle method-handle
        :action-handle action-handle
        :decision-class :method-prepared
        :steward-surface steward-surface
        :custody-owner :steward
        :witness-surface :separate-custody
        :telemetry-route :steward-review
        :authority-ceiling :admissibility-review
        :revocation-path :required
        :loss-condition :required
        :review-only t
        :requires-separate-enactment-boundary t
        :admissible-for-enactment-review t
        :authorizes-execution nil
        :executes-action nil
        :grants-authority nil
        :admits-continuity nil
        :activates-runtime nil
        :emits-packet nil
        :evaluates-lisp nil))

(defun describe-steward-action-admissibility-boundary ()
  '(:posture :cme-steward-action-admissibility-boundary
    :lisp-role :inert-steward-admissibility-carrier
    :doctrine "Admissibility is not execution. Steward acceptance is not runtime motion."
    :core-invariant "an admissible action remains sealed until a separate enactment boundary exists"
    :source-required :action-method-readiness-receipt
    :admissibility-shape
      (:decision-handle
       :method-handle
       :action-handle
       :steward-surface
       :custody-owner
       :witness-surface
       :telemetry-route
       :authority-ceiling
       :revocation-path
       :loss-condition)
    :admissible-for-enactment-review t
    :requires-separate-enactment-boundary t
    :admissibility-is-execution nil
    :steward-acceptance-is-runtime-motion nil
    :admissible-action-may-execute nil
    :admissibility-may-grant-authority nil
    :admissibility-may-admit-continuity nil
    :runtime-action-allowed nil
    :lisp-evaluation-allowed nil
    :packet-emission-allowed nil
    :receipt-replay-allowed nil
    :passage-increment-requested nil
    :activation-allowed nil
    :return :receipt-only))

(defun describe-seed-steward-action-admissibility ()
  (let ((action "urn:san:typed-action:review:seed")
        (method "urn:san:action-method:readiness:seed")
        (decision "urn:san:steward-action-admissibility:seed"))
    (list
      (steward-admissibility-predicate-result
        "urn:san:admissibility-predicate:method-ready"
        method
        action
        :method-ready-under-steward
        "urn:san:evidence:steward-admissibility-seed")
      (steward-action-admissibility-decision
        decision
        method
        action
        :steward))))
