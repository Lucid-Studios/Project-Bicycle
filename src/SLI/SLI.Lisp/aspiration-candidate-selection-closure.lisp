;; INERT SYMBOLIC CARRIER - Cold Corridor Only
;; Aspiration candidate selection closure boundary.
;; Selection may form a working set. Selection may not become admission.

(defun aspiration-candidate-selection
    (selection-handle candidate-handle statement-handle state evidence-handle witness-handle)
  (list :aspiration-candidate-selection
        :selection-handle selection-handle
        :source-maturation-candidate-handle candidate-handle
        :source-payload-statement-handle statement-handle
        :selection-state state
        :selection-rationale :review-working-set-formation
        :evidence-handle evidence-handle
        :witness-handle witness-handle
        :return-path-handle "urn:san:return:aspiration-selection"
        :review-only t
        :preserves-candidate-lineage t
        :preserves-payload-lineage t
        :requires-steward-review t
        :requires-cooling t
        :allows-compost-retention t
        :selection-becomes-warrant nil
        :selection-becomes-admission nil
        :selection-grants-authority nil
        :selection-admits-continuity nil
        :selection-authorizes-action nil
        :selection-evaluates-lisp nil
        :selection-smuggles-key nil))

(defun aspiration-closure-law (law-handle law-text)
  (list :aspiration-closure-law
        :law-handle law-handle
        :law-text law-text
        :review-only t
        :preserves-selection-lineage t
        :preserves-compost t
        :requires-witness t
        :requires-return-path t
        :keeps-keys-withheld t
        :law-becomes-warrant nil
        :law-grants-authority nil
        :law-admits-continuity nil
        :law-authorizes-action nil
        :law-evaluates-lisp nil
        :law-activates nil))

(defun describe-aspiration-candidate-selection-closure-boundary ()
  '(:posture :cme-aspiration-candidate-selection-closure-boundary
    :lisp-role :inert-aspiration-selection-carrier
    :selection-states (:selected-working-set :held-as-compost :returned-for-evidence :deferred-for-cooling)
    :root-laws
      (:selection-not-warrant
       :selection-not-admission
       :selection-not-authority
       :selection-not-continuity
       :closure-law-not-key
       :compost-not-erasure)
    :boundary-requirements
      (:evidence-required t
       :witness-required t
       :cooling-required t
       :return-path-required t
       :steward-review-required t
       :keys-withheld t)
    :selection-may-become-warrant nil
    :selection-may-become-admission nil
    :selection-may-grant-authority nil
    :selection-may-admit-continuity nil
    :closure-law-may-smuggle-key nil
    :compost-may-be-erased nil
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

(defun describe-seed-aspiration-selection ()
  (let ((candidate "urn:san:aspiration-payload:candidate:prime-body")
        (statement "urn:san:aspiration-payload:statement:prime-body"))
    (list
      (aspiration-candidate-selection
        "urn:san:aspiration-selection:prime-body"
        candidate
        statement
        :selected-working-set
        "urn:san:evidence:aspiration-selection:prime-body"
        "urn:san:witness:aspiration-selection:prime-body")
      (aspiration-closure-law
        "urn:san:aspiration-closure-law:keys-withheld"
        "selection may shape the working set; selection may not become the key"))))
