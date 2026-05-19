;; INERT SYMBOLIC CARRIER - Cold Corridor Only
;; Enactment dry-run rehearsal boundary.
;; Rehearsal may simulate. Rehearsal may not enact.

(defun enactment-dry-run-rehearsal
    (rehearsal-handle readiness-handle packet-handle dry-run-plan-handle)
  (list :enactment-dry-run-rehearsal
        :rehearsal-handle rehearsal-handle
        :source-readiness-handle readiness-handle
        :source-packet-handle packet-handle
        :dry-run-plan-handle dry-run-plan-handle
        :duty-station :lab-local-tiny-bicycle-review
        :work-surface :local-reversible-review-receipt
        :intended-work :rehearse-local-no-op-effect-without-enactment
        :method-code :dry-run-rehearsal-review-only
        :simulated-effect-handle "urn:san:simulated-effect:dry-run-rehearsal"
        :rollback-proof-handle "urn:san:rollback-proof:dry-run-rehearsal"
        :custody-owner :steward
        :witness-handle "urn:san:witness:dry-run-rehearsal"
        :telemetry-route :telemetry-string
        :steward-review-handle "urn:san:steward-review:dry-run-rehearsal"
        :review-only t
        :simulation-only t
        :no-op-only t
        :local-only t
        :reversible-only t
        :requires-rollback-proof t
        :requires-steward-review t
        :simulation-becomes-permission nil
        :dry-run-authorizes-action nil
        :dry-run-executes-action nil
        :dry-run-moves-runtime nil
        :dry-run-writes-outside-receipt-surface nil
        :dry-run-grants-authority nil
        :dry-run-admits-continuity nil
        :dry-run-evaluates-lisp nil
        :dry-run-emits-membrane-packet nil
        :dry-run-replays-receipt nil
        :dry-run-increments-passage nil
        :dry-run-activates nil))

(defun steward-dry-run-review-route
    (review-route-handle rehearsal-handle readiness-handle packet-handle)
  (list :steward-dry-run-review-route
        :review-route-handle review-route-handle
        :rehearsal-handle rehearsal-handle
        :source-readiness-handle readiness-handle
        :source-packet-handle packet-handle
        :steward-surface :steward
        :custody-owner :steward
        :evidence-handle "urn:san:evidence:dry-run-rehearsal"
        :witness-handle "urn:san:witness:dry-run-rehearsal"
        :telemetry-route :telemetry-string
        :return-path-handle "urn:san:repair:dry-run-rehearsal"
        :review-only t
        :preserves-rehearsal-lineage t
        :preserves-readiness-lineage t
        :preserves-packet-lineage t
        :preserves-dry-run-plan-lineage t
        :routes-to-steward-dry-run-review t
        :requires-cooling t
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

(defun describe-enactment-dry-run-rehearsal-boundary ()
  '(:posture :cme-enactment-dry-run-rehearsal-boundary
    :lisp-role :inert-dry-run-rehearsal-carrier
    :source-required :enactment-boundary-readiness-receipt
    :core-invariant "dry-run rehearsal is not enactment"
    :rehearsal-path
      (:enactment-boundary-readiness
       :dry-run-rehearsal
       :simulated-effect
       :rollback-proof
       :steward-dry-run-review-route
       :separate-action-harness-still-required)
    :boundary-requirements
      (:readiness-receipt-required t
       :dry-run-plan-required t
       :simulated-effect-required t
       :rollback-proof-required t
       :no-op-required t
       :locality-required t
       :reversibility-required t
       :custody-required t
       :witness-required t
       :telemetry-route-required t
       :steward-review-required t)
    :root-laws
      (:ready-packet-may-enter-dry-run
       :dry-run-not-enactment
       :simulation-not-permission
       :reversible-local-effect-not-authorization
       :steward-dry-run-review-not-runtime-motion)
    :simulation-becomes-permission nil
    :dry-run-authorizes-action nil
    :dry-run-executes-action nil
    :dry-run-moves-runtime nil
    :dry-run-writes-outside-receipt-surface nil
    :dry-run-grants-authority nil
    :dry-run-admits-continuity nil
    :steward-dry-run-review-moves-runtime nil
    :reversible-local-effect-authorizes-action nil
    :runtime-action-allowed nil
    :lisp-evaluation-allowed nil
    :membrane-packet-emission-allowed nil
    :receipt-replay-allowed nil
    :passage-increment-requested nil
    :continuity-admission-allowed nil
    :authority-grant-allowed nil
    :activation-allowed nil
    :return :receipt-only))

(defun describe-seed-enactment-dry-run-rehearsal ()
  (let ((rehearsal "urn:san:enactment-dry-run-rehearsal:selected-prime")
        (readiness "urn:san:enactment-boundary-readiness:selected-prime")
        (packet "urn:san:scoped-work-packet:selected-prime")
        (plan "urn:san:dry-run-plan:enactment-boundary:selected-prime"))
    (list
      (enactment-dry-run-rehearsal
        rehearsal
        readiness
        packet
        plan)
      (steward-dry-run-review-route
        "urn:san:steward-review:dry-run-rehearsal:selected-prime"
        rehearsal
        readiness
        packet))))
