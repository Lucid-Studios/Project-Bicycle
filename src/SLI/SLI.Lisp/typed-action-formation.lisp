;; INERT SYMBOLIC CARRIER - Cold Corridor Only
;; Typed action formation boundary.
;; Action may be declared for review. Declaration is not execution.

(defun typed-action-surface
    (handle source target intent method-code)
  (list :typed-action-surface
        :handle handle
        :source source
        :target target
        :intent intent
        :method-code method-code
        :authority-ceiling :review-only
        :custody-owner :steward
        :witness-burden :separate-custody
        :telemetry-route :steward-review
        :admissibility-predicate :declared-terms-required
        :revocation-path :required
        :loss-condition :required
        :review-only t
        :candidate-only t
        :runtime-effect-requested nil
        :continuity-effect-requested nil
        :attempts-self-authorization nil))

(defun methodological-formation-analysis
    (handle action-handle origin evidence-handle)
  (list :methodological-formation-analysis
        :handle handle
        :action-handle action-handle
        :origin origin
        :source-evidence-handle evidence-handle
        :formation-trace :declared
        :pressure-class :bounded
        :evidence-body-present t
        :witness-body-present t
        :explains-candidate t
        :authorizes-candidate nil
        :emits-packet nil
        :replays-receipt nil
        :increments-passage nil))

(defun design-predicate
    (handle action-handle predicate-code required-term)
  (list :design-predicate
        :handle handle
        :action-handle action-handle
        :predicate-code predicate-code
        :requires-term required-term
        :required-term-present t
        :review-only t
        :may-execute-itself nil
        :may-authorize-action nil
        :may-admit-continuity nil
        :may-activate-runtime nil
        :may-evaluate-lisp nil))

(defun describe-typed-action-formation-boundary ()
  '(:posture :cme-typed-action-formation-boundary
    :lisp-role :inert-action-surface-declaration-carrier
    :doctrine "Typed action may be declared for review. Declaration is not execution."
    :core-invariant "formation analysis is not authorization"
    :declaration-shape
      (:source
       :target
       :intent
       :method
       :authority-ceiling
       :custody-owner
       :witness-burden
       :telemetry-route
       :admissibility-predicate
       :revocation-path
       :loss-condition)
    :formation-origins
      (:operator-instruction
       :compass-shell
       :receipt-query
       :artifact-replay
       :memory-residue
       :tool-result
       :public-witness-pressure
       :design-inference)
    :formation-may-explain-candidate t
    :formation-may-authorize-candidate nil
    :design-predicate-may-execute-itself nil
    :design-predicate-may-authorize-action nil
    :declared-action-may-execute nil
    :summary-may-become-action nil
    :receipt-may-become-action nil
    :replay-may-become-action nil
    :query-may-become-action nil
    :runtime-action-allowed nil
    :lisp-evaluation-allowed nil
    :packet-emission-allowed nil
    :receipt-replay-allowed nil
    :passage-increment-requested nil
    :continuity-admission-allowed nil
    :authority-grant-allowed nil
    :return :receipt-only))

(defun describe-seed-typed-action-formation ()
  (let ((action "urn:san:typed-action:review:seed"))
    (list
      (typed-action-surface
        action
        :compass
        :steward
        "review candidate action surface without executing"
        :review-only-method)
      (methodological-formation-analysis
        "urn:san:formation-analysis:seed"
        action
        :design-inference
        "urn:san:evidence:typed-action-seed")
      (design-predicate
        "urn:san:design-predicate:source-target-method"
        action
        :source-target-method-required
        :source-target-method))))
