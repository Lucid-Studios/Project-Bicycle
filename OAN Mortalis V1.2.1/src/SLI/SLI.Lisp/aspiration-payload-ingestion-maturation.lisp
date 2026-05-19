;; INERT SYMBOLIC CARRIER - Cold Corridor Only
;; Aspiration payload ingestion maturation boundary.
;; The full body may be loaded, ingested, articulated, and matured for review.
;; It may not crown warrant, authority, continuity, action, or activation.

(defun aspiration-payload-statement
    (statement-handle source-wave-cascade-handle lane-kind statement-text evidence-handle witness-handle)
  (list :aspiration-payload-statement
        :statement-handle statement-handle
        :source-wave-cascade-handle source-wave-cascade-handle
        :lane-kind lane-kind
        :statement-text statement-text
        :evidence-handle evidence-handle
        :witness-handle witness-handle
        :review-only t
        :evidence-body-present t
        :witness-body-present t
        :cooling-path-present t
        :return-path-present t
        :ingestion-allowed t
        :articulation-allowed t
        :maturation-allowed t
        :treats-aspiration-as-warrant nil
        :treats-payload-density-as-truth nil
        :treats-ingestion-as-admission nil
        :treats-articulation-as-authority nil
        :treats-maturation-as-continuity nil
        :authorizes-action nil
        :mutates-identity nil
        :evaluates-lisp nil))

(defun aspiration-payload-ingestion-lane
    (lane-handle source-statement-handle lane-kind target-body-surface)
  (list :aspiration-payload-ingestion-lane
        :lane-handle lane-handle
        :source-statement-handle source-statement-handle
        :lane-kind lane-kind
        :target-body-surface target-body-surface
        :payload-class :aspiration-review-only
        :review-only t
        :ingested-for-review t
        :preserves-source-lineage t
        :requires-evidence t
        :requires-witness t
        :requires-cooling t
        :requires-return-path t
        :allows-admission nil
        :allows-authority nil
        :allows-continuity nil
        :allows-action nil
        :allows-lisp-evaluation nil))

(defun aspiration-maturation-candidate
    (candidate-handle source-statement-handle lane-handle articulated-form)
  (list :aspiration-maturation-candidate
        :candidate-handle candidate-handle
        :source-statement-handle source-statement-handle
        :lane-handle lane-handle
        :articulated-form articulated-form
        :maturation-posture :candidate-only
        :review-only t
        :articulated-for-review t
        :matured-as-candidate t
        :candidate-only t
        :preserves-payload-lineage t
        :requires-steward-review t
        :requires-return-path t
        :articulation-becomes-authority nil
        :maturation-becomes-continuity nil
        :candidate-becomes-warrant nil
        :candidate-authorizes-action nil
        :candidate-evaluates-lisp nil
        :candidate-activates nil))

(defun describe-aspiration-payload-ingestion-maturation-boundary ()
  '(:posture :cme-aspiration-payload-ingestion-maturation-boundary
    :lisp-role :inert-aspiration-payload-carrier
    :payload-scope :full-body-aspiration-review
    :lanes (:prime-body :cryptic-mind :steward-witness :sli-lisp
            :engineered-cognition :pedagogy :telemetry :operator-intent)
    :process-chain (:load :ingest :articulate :mature :return-for-review)
    :root-laws
      (:aspiration-payload-not-warrant
       :payload-density-not-truth
       :ingestion-not-admission
       :articulation-not-authority
       :maturation-not-continuity
       :full-stack-scope-not-activation)
    :boundary-requirements
      (:typed-lanes-required t
       :evidence-required t
       :witness-required t
       :cooling-required t
       :return-path-required t
       :steward-review-required t)
    :payload-may-authorize nil
    :payload-density-may-become-truth nil
    :ingestion-may-become-admission nil
    :articulation-may-become-authority nil
    :maturation-may-admit-continuity nil
    :candidate-may-authorize-action nil
    :runtime-action-allowed nil
    :lisp-evaluation-allowed nil
    :packet-emission-allowed nil
    :receipt-replay-allowed nil
    :passage-increment-requested nil
    :continuity-admission-allowed nil
    :authority-grant-allowed nil
    :activation-allowed nil
    :return :receipt-only))

(defun describe-seed-aspiration-payload ()
  (let ((statement "urn:san:aspiration-payload:statement:prime-body")
        (lane "urn:san:aspiration-payload:lane:prime-body"))
    (list
      (aspiration-payload-statement
        statement
        "urn:san:wave-cascade:review"
        :prime-body
        "Prime remains body-side invariant posture."
        "urn:san:evidence:aspiration-payload:prime-body"
        "urn:san:witness:aspiration-payload:prime-body")
      (aspiration-payload-ingestion-lane
        lane
        statement
        :prime-body
        :prime-body-register)
      (aspiration-maturation-candidate
        "urn:san:aspiration-payload:candidate:prime-body"
        statement
        lane
        :prime-body-review-candidate))))
