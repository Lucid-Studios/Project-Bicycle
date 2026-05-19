;; INERT SYMBOLIC CARRIER - Cold Corridor Only
;; Enactment boundary readiness boundary.
;; Readiness may approach review. Readiness may not execute.

(defun enactment-boundary-readiness
    (readiness-handle packet-handle steward-route-handle duty-station work-surface)
  (list :enactment-boundary-readiness
        :readiness-handle readiness-handle
        :source-packet-handle packet-handle
        :source-steward-route-handle steward-route-handle
        :duty-station duty-station
        :work-surface work-surface
        :intended-work :prepare-local-reversible-review-work-without-enactment
        :method-code :scoped-work-packet-review-only
        :authority-ceiling :steward-enactment-boundary-required
        :local-effect-ceiling :receipt-only-under-tiny-bicycle-lab
        :reversibility-proof-handle "urn:san:reversibility-proof:enactment-boundary"
        :dry-run-plan-handle "urn:san:dry-run-plan:enactment-boundary"
        :custody-owner :steward
        :witness-handle "urn:san:witness:enactment-boundary-readiness"
        :telemetry-route :telemetry-string
        :steward-review-handle "urn:san:steward-review:enactment-boundary"
        :revocation-path "urn:san:revocation:enactment-boundary"
        :repair-path "urn:san:repair:enactment-boundary"
        :loss-condition :readiness-treated-as-enactment
        :review-only t
        :approach-only t
        :local-only t
        :reversible-only t
        :requires-steward-review t
        :requires-dry-run-before-execution t
        :requires-separate-action-harness t
        :readiness-becomes-warrant nil
        :readiness-becomes-admission nil
        :readiness-grants-authority nil
        :readiness-admits-continuity nil
        :readiness-authorizes-action nil
        :readiness-executes-action nil
        :approach-moves-runtime nil
        :locality-authorizes-action nil
        :reversibility-authorizes-action nil
        :steward-review-moves-runtime nil
        :readiness-evaluates-lisp nil
        :readiness-emits-membrane-packet nil
        :readiness-replays-receipt nil
        :readiness-increments-passage nil
        :readiness-activates nil))

(defun enactment-boundary-steward-review-route
    (review-route-handle readiness-handle packet-handle evidence-handle witness-handle)
  (list :enactment-boundary-steward-review-route
        :review-route-handle review-route-handle
        :readiness-handle readiness-handle
        :source-packet-handle packet-handle
        :steward-surface :steward
        :custody-owner :steward
        :evidence-handle evidence-handle
        :witness-handle witness-handle
        :telemetry-route :telemetry-string
        :return-path-handle "urn:san:repair:enactment-boundary"
        :review-only t
        :preserves-readiness-lineage t
        :preserves-packet-lineage t
        :preserves-steward-route-lineage t
        :routes-to-steward-enactment-review t
        :requires-cooling t
        :requires-separate-action-harness t
        :route-authorizes-action nil
        :route-executes-action nil
        :route-moves-runtime nil
        :route-grants-authority nil
        :route-admits-continuity nil
        :route-evaluates-lisp nil
        :route-emits-membrane-packet nil
        :route-replays-receipt nil
        :route-increments-passage nil
        :route-activates nil))

(defun describe-enactment-boundary-readiness-boundary ()
  '(:posture :cme-enactment-boundary-readiness-boundary
    :lisp-role :inert-enactment-boundary-carrier
    :source-required :scoped-work-packet-formation-receipt
    :approach-path
      (:scoped-work-packet
       :enactment-boundary-readiness
       :steward-enactment-review-route
       :separate-action-harness-required)
    :boundary-requirements
      (:scoped-work-packet-receipt-required t
       :steward-route-required t
       :duty-station-required t
       :work-surface-required t
       :intended-work-required t
       :method-code-required t
       :authority-ceiling-required t
       :local-effect-ceiling-required t
       :reversibility-proof-required t
       :dry-run-plan-required t
       :custody-required t
       :witness-required t
       :telemetry-route-required t
       :steward-review-required t
       :revocation-path-required t
       :repair-path-required t
       :loss-condition-required t
       :separate-action-harness-required t)
    :root-laws
      (:readiness-not-warrant
       :readiness-not-admission
       :readiness-not-authority
       :readiness-not-continuity
       :readiness-not-action
       :approach-not-enactment
       :locality-not-permission
       :reversibility-not-permission
       :steward-review-not-runtime-motion)
    :readiness-may-become-warrant nil
    :readiness-may-become-admission nil
    :readiness-may-authorize nil
    :readiness-may-execute nil
    :readiness-may-move-runtime nil
    :readiness-may-grant-authority nil
    :readiness-may-admit-continuity nil
    :approach-may-authorize nil
    :locality-may-authorize nil
    :reversibility-may-authorize nil
    :steward-review-may-move-runtime nil
    :dry-run-plan-may-execute nil
    :separate-action-harness-required t
    :runtime-action-allowed nil
    :lisp-evaluation-allowed nil
    :membrane-packet-emission-allowed nil
    :receipt-replay-allowed nil
    :passage-increment-requested nil
    :continuity-admission-allowed nil
    :authority-grant-allowed nil
    :activation-allowed nil
    :return :receipt-only))

(defun describe-seed-enactment-boundary-readiness ()
  (let ((readiness "urn:san:enactment-boundary-readiness:selected-prime")
        (packet "urn:san:scoped-work-packet:selected-prime")
        (route "urn:san:steward-route:scoped-work-packet:selected-prime"))
    (list
      (enactment-boundary-readiness
        readiness
        packet
        route
        :lab-local-tiny-bicycle-review
        :local-reversible-review-receipt)
      (enactment-boundary-steward-review-route
        "urn:san:steward-review:enactment-boundary:selected-prime"
        readiness
        packet
        "urn:san:evidence:enactment-boundary:selected-prime"
        "urn:san:witness:enactment-boundary-readiness:selected-prime"))))
