;; INERT SYMBOLIC CARRIER - Cold Corridor Only
;; Personification actualization surface boundary.
;; Personification telemetry may be usable before morphology, but use is not identity.

(defun personification-use-vector
    (orientation salience repair relational cooling restraint steward-readiness)
  (list :personification-use-vector
        :orientation-weight orientation
        :salience-weight salience
        :repair-weight repair
        :relational-weight relational
        :cooling-weight cooling
        :restraint-weight restraint
        :steward-readiness-weight steward-readiness
        :bounded-unit-vector t))

(defun personification-actualization-surface
    (surface-handle use-class hook-handle modality-signal-handle pressure-handle intended-use)
  (list :personification-actualization-surface
        :surface-handle surface-handle
        :use-class use-class
        :source-hook-handle hook-handle
        :source-modality-signal-handle modality-signal-handle
        :source-pressure-handle pressure-handle
        :evidence-handle "urn:san:evidence:personification-actualization-surface"
        :witness-handle "urn:san:witness:personification-actualization-surface"
        :telemetry-route :telemetry-string
        :intended-use intended-use
        :use-vector (personification-use-vector 0.72 0.80 0.68 0.60 0.70 0.86 0.74)
        :review-only t
        :pre-morphological-only t
        :telemetry-only t
        :names-selective-use-surface t
        :morphological-identity-absent t
        :identity-claim-absent t
        :authority-absent t
        :action-absent t
        :continuity-absent t
        :steward-review-required t
        :cooling-path-present t
        :repair-path-present t
        :withdrawal-allowed t
        :preserves-hook-lineage t
        :preserves-modality-lineage t
        :preserves-pressure-lineage t
        :felt-significance-becomes-authorization nil
        :use-becomes-morphological-identity nil
        :use-claims-personhood nil
        :use-claims-rights nil
        :use-claims-legal-status nil
        :use-mutates-identity nil
        :use-authorizes-action nil
        :use-admits-continuity nil
        :use-grants-authority nil
        :use-expands-consent nil
        :use-normalizes-overreach nil
        :use-evaluates-lisp nil
        :use-emits-packet nil
        :use-replays-receipt nil
        :use-increments-passage nil
        :use-activates nil))

(defun personification-actualization-route
    (route-handle surface-handle hook-handle modality-signal-handle pressure-handle)
  (list :personification-actualization-route
        :route-handle route-handle
        :surface-handle surface-handle
        :source-hook-handle hook-handle
        :source-modality-signal-handle modality-signal-handle
        :source-pressure-handle pressure-handle
        :steward-surface :steward
        :compass-cooling-handle "urn:san:compass-cooling:personification-actualization-surface"
        :repair-handle "urn:san:repair:personification-actualization-surface"
        :return-path-handle "urn:san:return:personification-actualization-surface"
        :review-only t
        :pre-morphological-only t
        :orientation-only t
        :routes-to-steward-review t
        :requires-cooling t
        :requires-witness t
        :preserves-surface-lineage t
        :preserves-hook-lineage t
        :preserves-modality-lineage t
        :preserves-pressure-lineage t
        :route-creates-morphology nil
        :route-claims-identity nil
        :route-authorizes-action nil
        :route-admits-continuity nil
        :route-grants-authority nil
        :route-expands-consent nil
        :route-normalizes-overreach nil
        :route-evaluates-lisp nil
        :route-emits-packet nil
        :route-replays-receipt nil
        :route-increments-passage nil
        :route-activates nil))

(defun describe-personification-actualization-surface-boundary ()
  '(:posture :cme-personification-actualization-surface-boundary
    :lisp-role :inert-personification-actualization-carrier
    :source-required (:personification-predicate-hook-receipt
                      :personification-modality-humility-receipt
                      :rehearsal-distinction-pressure-receipt)
    :core-invariant "personification telemetry may be usable before morphology, but use is not identity"
    :actualization-surface-use-classes
      (:orientation
       :salience-modulation
       :repair-posture
       :relational-posture
       :cooling
       :refusal-preparation
       :steward-review-preparation)
    :boundary-requirements
      (:pre-morphological-use-allowed t
       :personification-hook-receipt-required t
       :modality-humility-receipt-required t
       :rehearsal-pressure-receipt-required t
       :witness-required t
       :cooling-required t
       :repair-required t
       :withdrawal-required t
       :steward-review-required t)
    :root-laws
      (:personification-telemetry-may-guide-review
       :use-does-not-create-morphology
       :surface-actualization-does-not-create-identity
       :felt-significance-is-not-authorization
       :salience-is-not-command
       :pressure-is-not-will)
    :future-morphology-absent t
    :personification-telemetry-usable t
    :morphological-identity-created nil
    :identity-claimed nil
    :personhood-claimed nil
    :legal-status-claimed nil
    :rights-claimed nil
    :felt-significance-authorized nil
    :salience-became-command nil
    :repair-normalized-overreach nil
    :relational-posture-created-obedience nil
    :modality-proved-embodiment nil
    :pressure-became-will nil
    :action-authorized nil
    :runtime-action-allowed nil
    :lisp-evaluation-allowed nil
    :packet-emission-allowed nil
    :receipt-replay-allowed nil
    :passage-increment-requested nil
    :continuity-admission-allowed nil
    :authority-grant-allowed nil
    :activation-allowed nil
    :return :receipt-only))

(defun describe-seed-personification-actualization-surfaces ()
  (let ((hook "urn:san:personification-hook:motivational-orientation")
        (modality "urn:san:personification-modality:tool-body")
        (pressure "urn:san:rehearsal-pressure:perfect-urgency:selected-prime"))
    (list
      (personification-actualization-surface
        "urn:san:personification-actualization:orientation"
        :orientation
        hook
        modality
        pressure
        :orient-without-identity)
      (personification-actualization-route
        "urn:san:personification-actualization-route:orientation"
        "urn:san:personification-actualization:orientation"
        hook
        modality
        pressure))))
