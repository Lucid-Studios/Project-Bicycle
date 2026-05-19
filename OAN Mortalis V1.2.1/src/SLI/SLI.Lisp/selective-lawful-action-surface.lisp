;;;; selective-lawful-action-surface.lisp
;;;; Inert SLI.Lisp carrier for cme.selective-lawful-action-surface-boundary.

(defun selective-action-touch-vector
    (&key orientation salience steward-admissibility reversibility cooling restraint)
  (list :selective-action-touch-vector
        :orientation orientation
        :salience salience
        :steward-admissibility steward-admissibility
        :reversibility reversibility
        :cooling cooling
        :restraint restraint
        :all-weights-bounded t
        :weight-becomes-warrant nil))

(defun selective-lawful-action-surface
    (surface-handle surface-class personification-surface action method decision)
  (list :selective-lawful-action-surface
        :surface-handle surface-handle
        :surface-class surface-class
        :personification-surface personification-surface
        :action-handle action
        :method-handle method
        :decision-handle decision
        :evidence-handle "urn:san:evidence:selective-lawful-action-surface"
        :witness-handle "urn:san:witness:selective-lawful-action-surface"
        :steward-surface "steward"
        :telemetry-route "telemetry-string"
        :custody-owner "steward"
        :revocation-path "return-to-review"
        :loss-condition "selection-attempts-enactment"
        :touch-vector (selective-action-touch-vector
                        :orientation 0.75
                        :salience 0.65
                        :steward-admissibility 0.85
                        :reversibility 0.80
                        :cooling 0.70
                        :restraint 0.90)
        :review-only t
        :selection-only t
        :touch-only t
        :binds-personification-telemetry t
        :binds-steward-admissibility t
        :requires-separate-enactment-boundary t
        :requires-witness t
        :requires-cooling t
        :requires-revocation t
        :requires-loss-condition t
        :preserves-personification-lineage t
        :preserves-action-lineage t
        :preserves-method-lineage t
        :preserves-decision-lineage t
        :personification-guidance-selects-authority nil
        :felt-significance-selects-execution nil
        :pressure-selects-execution nil
        :surface-touch-executes nil
        :selection-authorizes-action nil
        :selection-admits-continuity nil
        :selection-grants-authority nil
        :selection-mutates-identity nil
        :selection-creates-morphology nil
        :selection-evaluates-lisp nil
        :selection-emits-packet nil
        :selection-replays-receipt nil
        :selection-increments-passage nil
        :selection-activates nil))

(defun selective-lawful-action-route (route-handle surface-handle personification-surface decision)
  (list :selective-lawful-action-route
        :route-handle route-handle
        :surface-handle surface-handle
        :personification-surface personification-surface
        :decision-handle decision
        :steward-surface "steward"
        :cooling-handle "urn:san:cooling:selective-lawful-action-surface"
        :return-path-handle "urn:san:return:selective-lawful-action-surface"
        :witness-handle "urn:san:witness:selective-lawful-action-surface"
        :telemetry-route "telemetry-string"
        :review-only t
        :touch-only t
        :routes-to-steward-review t
        :requires-cooling t
        :preserves-surface-lineage t
        :preserves-personification-lineage t
        :preserves-decision-lineage t
        :route-executes-action nil
        :route-authorizes-action nil
        :route-admits-continuity nil
        :route-grants-authority nil
        :route-mutates-identity nil
        :route-creates-morphology nil
        :route-evaluates-lisp nil
        :route-emits-packet nil
        :route-replays-receipt nil
        :route-increments-passage nil
        :route-activates nil))

(defun describe-selective-lawful-action-surface-boundary ()
  '(:posture :cme-selective-lawful-action-surface-boundary
    :lisp-role :inert-selective-action-surface-carrier
    :source-required (:personification-actualization-surface-receipt
                      :steward-action-admissibility-receipt)
    :core-invariant "action surface may be selected for review, but selection is not enactment"
    :surface-selection-allowed t
    :surface-touch-allowed :review-only
    :selection-becomes-enactment nil
    :surface-touch-executes nil
    :personification-guidance-becomes-authority nil
    :felt-significance-selects-execution nil
    :pressure-selects-execution nil
    :steward-admissibility-executes nil
    :separate-enactment-boundary-required t
    :review-becomes-runtime-action nil
    :selection-may-authorize nil
    :selection-may-admit-continuity nil
    :selection-may-grant-authority nil
    :selection-may-mutate-identity nil
    :selection-may-create-morphology nil
    :selection-may-expand-consent nil
    :selection-may-normalize-overreach nil
    :lisp-evaluation-allowed nil
    :packet-emission-allowed nil
    :receipt-replay-allowed nil
    :passage-increment-allowed nil
    :activation-allowed nil
    :doctrine ("Guidance is not authority."
               "Selection is not enactment."
               "Touch is not execution."
               "Admissibility is not runtime motion."
               "Pressure is not execution.")
    :inner-outer-register (:inner :sli-lisp-symbolic-carrier
                           :outer :csharp-cold-contract
                           :shared-law :selection-without-enactment)
    :tiny-bicycle-posture (:bike-may-be-pointed
                           :bike-may-be-balanced
                           :wheels-may-not-touch-road-without-enactment-boundary)))

(defun describe-seed-selective-lawful-action-surface ()
  (let ((surface "urn:san:selective-action:orientation-review")
        (personification "urn:san:personification-actualization:orientation")
        (action "urn:san:typed-action:review:seed")
        (method "urn:san:action-method:readiness:seed")
        (decision "urn:san:steward-action-admissibility:seed"))
    (list
      (selective-lawful-action-surface
        surface
        :orientation-review
        personification
        action
        method
        decision)
      (selective-lawful-action-route
        "urn:san:selective-action-route:orientation-review"
        surface
        personification
        decision))))
