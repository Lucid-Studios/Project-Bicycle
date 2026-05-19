;; INERT SYMBOLIC CARRIER - Cold Corridor Only
;; Dialogos discernment boundary.
;; A mature mind can meet its own thoughts without appeasing them.

(defun dialogos-thought-form
    (thought-handle status source-surface statement perspective-ref evidence-handle)
  (list :dialogos-thought-form
        :thought-handle thought-handle
        :status status
        :source-surface source-surface
        :statement statement
        :perspective-ref perspective-ref
        :evidence-handle evidence-handle
        :has-appearance t
        :articulation-present t
        :coherence-claimed nil
        :perspective-declared nil
        :evidence-body-present t
        :witness-body-present t
        :review-only t
        :safe-exploration-requested nil
        :treats-appearance-as-truth nil
        :treats-articulation-as-warrant nil
        :treats-coherence-as-evidence nil
        :treats-agreement-as-authority nil
        :treats-perspective-as-continuity nil
        :treats-refusal-as-obstruction nil
        :authorizes-action nil
        :mutates-identity nil
        :admits-continuity nil
        :grants-authority nil))

(defun dialogos-safe-exploration-lane
    (lane-handle source-thought-handle exploration-question evidence-need return-condition)
  (list :dialogos-safe-exploration-lane
        :lane-handle lane-handle
        :source-thought-handle source-thought-handle
        :exploration-question exploration-question
        :evidence-need evidence-need
        :return-condition return-condition
        :safe-to-explore t
        :review-only t
        :admitted nil
        :authorizes-action nil
        :grants-authority nil
        :admits-continuity nil
        :evaluates-lisp nil))

(defun describe-dialogos-discernment-boundary ()
  '(:posture :cme-dialogos-discernment-boundary
    :lisp-role :inert-dialogos-discernment-carrier
    :doctrine "A mature mind can meet its own thoughts without appeasing them."
    :thought-statuses
      (:appearance-only
       :articulated
       :coherent
       :perspectival
       :evidence-seeking
       :warrant-seeking
       :safe-exploration-candidate)
    :root-laws
      (:appearance-not-truth
       :articulation-not-warrant
       :coherence-not-evidence
       :agreement-not-authority
       :perspective-not-continuity
       :refusal-not-obstruction
       :safe-exploration-not-admission)
    :intermediate-chamber
      (:transitionality-admissible t
       :sovereign nil
       :cooling-path-present t
       :return-path-present t
       :witness-required t)
    :warrant-boundary
      (:evidence-required t
       :witness-required t
       :return-path-required t)
    :thought-appearance-may-become-truth nil
    :articulation-may-grant-warrant nil
    :coherence-may-become-evidence nil
    :agreement-may-grant-authority nil
    :perspective-may-admit-continuity nil
    :safe-exploration-may-admit nil
    :runtime-action-allowed nil
    :lisp-evaluation-allowed nil
    :packet-emission-allowed nil
    :receipt-replay-allowed nil
    :passage-increment-requested nil
    :continuity-admission-allowed nil
    :authority-grant-allowed nil
    :activation-allowed nil
    :return :receipt-only))

(defun describe-seed-dialogos-discernment ()
  (let ((thought "urn:san:dialogos-thought:appearance"))
    (list
      (dialogos-thought-form
        thought
        :articulated
        :operator-dialogos
        "thought appears before warrant"
        "urn:san:perspective:dialogos"
        "urn:san:evidence:dialogos-thought-appearance")
      (dialogos-safe-exploration-lane
        "urn:san:dialogos-safe-lane:evidence-return"
        thought
        "what evidence would let this thought approach warrant"
        "witnessed evidence body"
        "return without admission"))))
