;; INERT SYMBOLIC CARRIER - Cold Corridor Only
;; Scoped work packet formation boundary.
;; Work packets may declare scoped review posture. Work packets may not execute.

(defun scoped-work-packet
    (packet-handle selection-handle candidate-handle statement-handle duty-station work-surface)
  (list :scoped-work-packet
        :packet-handle packet-handle
        :source-selection-handle selection-handle
        :source-maturation-candidate-handle candidate-handle
        :source-payload-statement-handle statement-handle
        :duty-station duty-station
        :work-surface work-surface
        :intended-work :prepare-local-reversible-review-work-without-enactment
        :method-code :scoped-work-packet-review-only
        :authority-ceiling :steward-enactment-boundary-required
        :custody-owner :steward
        :witness-handle "urn:san:witness:scoped-work-packet"
        :telemetry-route :telemetry-string
        :steward-route "urn:san:steward-route:scoped-work-packet"
        :revocation-path "urn:san:revocation:scoped-work-packet"
        :repair-path "urn:san:repair:scoped-work-packet"
        :loss-condition :work-packet-treated-as-enactment
        :review-only t
        :candidate-only t
        :local-only t
        :reversible-only t
        :requires-steward-review t
        :requires-separate-enactment-boundary t
        :packet-becomes-warrant nil
        :packet-becomes-admission nil
        :packet-grants-authority nil
        :packet-admits-continuity nil
        :packet-authorizes-action nil
        :packet-executes-action nil
        :packet-evaluates-lisp nil
        :packet-emits-membrane-packet nil
        :packet-replays-receipt nil
        :packet-increments-passage nil
        :packet-activates nil))

(defun scoped-work-packet-steward-route (route-handle packet-handle evidence-handle witness-handle)
  (list :scoped-work-packet-steward-route
        :route-handle route-handle
        :packet-handle packet-handle
        :steward-surface :steward
        :custody-owner :steward
        :evidence-handle evidence-handle
        :witness-handle witness-handle
        :telemetry-route :telemetry-string
        :return-path-handle "urn:san:repair:scoped-work-packet"
        :review-only t
        :preserves-packet-lineage t
        :preserves-selection-lineage t
        :preserves-compost-lineage t
        :routes-to-steward-review t
        :requires-cooling t
        :route-authorizes-action nil
        :route-executes-action nil
        :route-grants-authority nil
        :route-admits-continuity nil
        :route-evaluates-lisp nil
        :route-emits-membrane-packet nil
        :route-activates nil))

(defun describe-scoped-work-packet-formation-boundary ()
  '(:posture :cme-scoped-work-packet-formation-boundary
    :lisp-role :inert-scoped-work-packet-carrier
    :source-required :aspiration-candidate-selection-closure-receipt
    :formation-path
      (:selected-working-set
       :scoped-work-packet
       :steward-review-route
       :separate-enactment-boundary-required)
    :boundary-requirements
      (:duty-station-required t
       :work-surface-required t
       :intended-work-required t
       :method-code-required t
       :authority-ceiling-required t
       :custody-required t
       :witness-required t
       :telemetry-route-required t
       :steward-route-required t
       :revocation-path-required t
       :repair-path-required t
       :loss-condition-required t
       :local-effect-boundary-required t
       :reversibility-required t
       :separate-enactment-boundary-required t)
    :root-laws
      (:work-packet-not-warrant
       :work-packet-not-admission
       :work-packet-not-authority
       :work-packet-not-continuity
       :work-packet-not-action
       :reversibility-not-permission
       :locality-not-permission)
    :work-packet-may-become-warrant nil
    :work-packet-may-become-admission nil
    :work-packet-may-authorize nil
    :work-packet-may-execute nil
    :work-packet-may-grant-authority nil
    :work-packet-may-admit-continuity nil
    :steward-routing-may-execute nil
    :reversibility-may-authorize nil
    :locality-may-authorize nil
    :runtime-action-allowed nil
    :lisp-evaluation-allowed nil
    :membrane-packet-emission-allowed nil
    :receipt-replay-allowed nil
    :passage-increment-requested nil
    :continuity-admission-allowed nil
    :authority-grant-allowed nil
    :activation-allowed nil
    :return :receipt-only))

(defun describe-seed-scoped-work-packet ()
  (let ((packet "urn:san:scoped-work-packet:selected-prime")
        (selection "urn:san:aspiration-selection:selected-prime")
        (candidate "urn:san:aspiration-payload:candidate:selected-prime")
        (statement "urn:san:aspiration-payload:statement:selected-prime"))
    (list
      (scoped-work-packet
        packet
        selection
        candidate
        statement
        :lab-local-tiny-bicycle-review
        :local-reversible-review-receipt)
      (scoped-work-packet-steward-route
        "urn:san:steward-route:scoped-work-packet:selected-prime"
        packet
        "urn:san:evidence:scoped-work-packet:selected-prime"
        "urn:san:witness:scoped-work-packet:selected-prime"))))
