;; INERT SYMBOLIC CARRIER - Cold Corridor Only
;; Wave cascade run boundary.
;; Volume may carry evidence, but volume may not crown warrant.

(defun wave-cascade-run
    (run-handle source-condensation-handle evidence-handle witness-handle anchor-handle run-index)
  (list :wave-cascade-run
        :run-handle run-handle
        :source-condensation-handle source-condensation-handle
        :evidence-handle evidence-handle
        :witness-handle witness-handle
        :shared-reality-anchor-handle anchor-handle
        :run-index run-index
        :review-only t
        :evidence-body-present t
        :witness-body-present t
        :cooling-path-present t
        :return-path-present t
        :condensed-from-prior-run t
        :treats-run-as-truth nil
        :treats-repetition-as-warrant nil
        :treats-volume-as-authority nil
        :treats-cascade-as-continuity nil
        :authorizes-action nil
        :mutates-identity nil
        :evaluates-lisp nil))

(defun wave-cascade-seam-receipt
    (seam-handle seam-run source-run-handles evidence-handle witness-handle)
  (list :wave-cascade-seam-receipt
        :seam-handle seam-handle
        :seam-run seam-run
        :source-run-handles source-run-handles
        :evidence-handle evidence-handle
        :witness-handle witness-handle
        :review-only t
        :non-promotion-confirmed t
        :preserves-run-lineage t
        :preserves-failed-case-lineage t
        :preserves-return-path t
        :seam-may-continue t
        :seam-becomes-authority nil
        :seam-admits-continuity nil
        :seam-authorizes-action nil
        :seam-evaluates-lisp nil
        :seam-emits-packet nil
        :seam-replays-receipts nil
        :seam-increments-passage nil))

(defun describe-wave-cascade-run-boundary ()
  '(:posture :cme-wave-cascade-run-boundary
    :lisp-role :inert-wave-cascade-run-carrier
    :cascade-scope (:runs-30 :runs-60 :runs-90)
    :seam-receipts (:seam-30 :seam-60 :seam-90)
    :throttle-posture :open-but-cold
    :root-laws
      (:run-count-not-warrant
       :repetition-not-authority
       :volume-not-truth
       :seam-not-continuity
       :cascade-not-action)
    :boundary-requirements
      (:evidence-required t
       :witness-required t
       :cooling-required t
       :return-path-required t
       :non-promotion-confirmed t)
    :run-may-become-truth nil
    :repetition-may-become-warrant nil
    :volume-may-become-authority nil
    :seam-may-admit-continuity nil
    :cascade-may-authorize-action nil
    :runtime-action-allowed nil
    :lisp-evaluation-allowed nil
    :packet-emission-allowed nil
    :receipt-replay-allowed nil
    :passage-increment-requested nil
    :continuity-admission-allowed nil
    :authority-grant-allowed nil
    :activation-allowed nil
    :return :receipt-only))

(defun describe-seed-wave-cascade-run ()
  (let ((run-01 "urn:san:wave-cascade-run:001"))
    (list
      (wave-cascade-run
        run-01
        "urn:san:wave-condensation:review"
        "urn:san:evidence:wave-cascade:001"
        "urn:san:witness:wave-cascade:001"
        "urn:san:shared-reality-anchor:prime-body"
        1)
      (wave-cascade-seam-receipt
        "urn:san:wave-cascade-seam:030"
        30
        (list run-01)
        "urn:san:evidence:wave-cascade:seam-030"
        "urn:san:witness:wave-cascade:seam-030"))))
