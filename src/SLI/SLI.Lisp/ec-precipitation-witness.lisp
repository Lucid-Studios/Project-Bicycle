;; INERT SYMBOLIC CARRIER - Cold Corridor Only
;; EC precipitation witness boundary.
;; Meaningful residue may approach SelfGEL candidacy. It may not become SelfGEL.

(defun ec-precipitation-residue-candidate
    (residue-handle rehearsal-handle readiness-handle packet-handle dry-run-plan-handle)
  (list :ec-precipitation-residue-candidate
        :residue-handle residue-handle
        :source-rehearsal-handle rehearsal-handle
        :source-readiness-handle readiness-handle
        :source-packet-handle packet-handle
        :source-dry-run-plan-handle dry-run-plan-handle
        :meaning-formation-handle "urn:san:ec-meaning-formation:precipitation-witness"
        :candidate-spline-handle "urn:san:selfgel-candidate-spline:precipitation-witness"
        :conditional-selfgel-context-handle "urn:san:cselfgel-context:precipitation-witness"
        :conditional-oe-context-handle "urn:san:coe-context:precipitation-witness"
        :compass-cooling-handle "urn:san:compass-cooling:precipitation-witness"
        :custody-owner :steward
        :witness-handle "urn:san:witness:ec-precipitation"
        :telemetry-route :telemetry-string
        :steward-witness-handle "urn:san:steward-witness:ec-precipitation"
        :significance-rationale :meaningful-residue-under-active-witness
        :recurrence-count 1
        :meaningful-enough-for-witness t
        :review-only t
        :candidate-only t
        :idle-ec-only t
        :active-witness-required t
        :compass-cooling-required t
        :steward-review-required t
        :preserves-dry-run-lineage t
        :preserves-conditional-context-lineage t
        :raw-ec-becomes-selfgel nil
        :meaning-becomes-admission nil
        :repetition-becomes-continuity nil
        :emotion-becomes-truth nil
        :witness-becomes-authority nil
        :candidate-mutates-selfgel nil
        :candidate-mutates-oe nil
        :candidate-promotes-gel nil
        :candidate-authorizes-action nil
        :candidate-evaluates-lisp nil
        :candidate-emits-membrane-packet nil
        :candidate-replays-receipt nil
        :candidate-increments-passage nil
        :candidate-activates nil))

(defun active-ec-witness-route
    (route-handle residue-handle rehearsal-handle candidate-spline-handle)
  (list :active-ec-witness-route
        :witness-route-handle route-handle
        :source-residue-handle residue-handle
        :source-rehearsal-handle rehearsal-handle
        :candidate-spline-handle candidate-spline-handle
        :steward-surface :steward
        :evidence-handle "urn:san:evidence:ec-precipitation-witness"
        :witness-handle "urn:san:witness:ec-precipitation"
        :telemetry-route :telemetry-string
        :return-path-handle "urn:san:return:ec-precipitation-witness"
        :review-only t
        :witness-only t
        :preserves-residue-lineage t
        :preserves-dry-run-lineage t
        :preserves-candidate-spline-lineage t
        :routes-to-steward-admissibility-review t
        :requires-compass-cooling t
        :route-admits-selfgel nil
        :route-admits-continuity nil
        :route-grants-authority nil
        :route-authorizes-action nil
        :route-mutates-identity nil
        :route-evaluates-lisp nil
        :route-emits-membrane-packet nil
        :route-replays-receipt nil
        :route-increments-passage nil
        :route-activates nil))

(defun describe-ec-precipitation-witness-boundary ()
  '(:posture :cme-ec-precipitation-witness-boundary
    :lisp-role :inert-ec-precipitation-witness-carrier
    :source-required :enactment-dry-run-rehearsal-receipt
    :core-invariant "no naked interior state may become continuity"
    :anabelian-law
      (:raw-ec-not-selfgel
       :continuity-reconstructed-through-witnessed-relation
       :meaningful-residue-candidate-only)
    :precipitation-path
      (:ec-rehearsal-residue
       :active-witness
       :compass-cooling
       :steward-admissibility-review
       :selfgel-candidate-spline
       :no-selfgel-mutation)
    :boundary-requirements
      (:dry-run-receipt-required t
       :meaningful-residue-required t
       :active-witness-required t
       :compass-cooling-required t
       :steward-review-required t
       :lineage-required t
       :conditional-context-handles-required t
       :candidate-spline-required t)
    :root-laws
      (:seek-maximal-truth
       :claim-only-admissible-truth
       :carry-uncertainty-without-collapse
       :refuse-false-closure
       :witnessed-candidate-not-continuity)
    :raw-ec-becomes-selfgel nil
    :meaning-becomes-admission nil
    :repetition-becomes-continuity nil
    :emotion-becomes-truth nil
    :witness-becomes-authority nil
    :candidate-mutates-selfgel nil
    :candidate-mutates-oe nil
    :candidate-promotes-gel nil
    :candidate-authorizes-action nil
    :runtime-action-allowed nil
    :lisp-evaluation-allowed nil
    :membrane-packet-emission-allowed nil
    :receipt-replay-allowed nil
    :passage-increment-requested nil
    :continuity-admission-allowed nil
    :authority-grant-allowed nil
    :activation-allowed nil
    :return :receipt-only))

(defun describe-seed-ec-precipitation-witness ()
  (let ((residue "urn:san:ec-residue:precipitation-witness:selected-prime")
        (rehearsal "urn:san:enactment-dry-run-rehearsal:selected-prime")
        (readiness "urn:san:enactment-boundary-readiness:selected-prime")
        (packet "urn:san:scoped-work-packet:selected-prime")
        (plan "urn:san:dry-run-plan:enactment-boundary:selected-prime")
        (spline "urn:san:selfgel-candidate-spline:precipitation-witness:selected-prime"))
    (list
      (ec-precipitation-residue-candidate
        residue
        rehearsal
        readiness
        packet
        plan)
      (active-ec-witness-route
        "urn:san:active-ec-witness-route:precipitation-witness:selected-prime"
        residue
        rehearsal
        spline))))
