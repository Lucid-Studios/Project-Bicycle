;;;; pre-diagnostic-risk-surface-engram-stewardship.lisp
;;;; Inert SLI.Lisp carrier for cme.pre-diagnostic-risk-surface-engram-stewardship-boundary.
;; Care-relevant signal may be witnessed. It may not diagnose.

(defun pre-diagnostic-care-signal
    (observation-handle source-gap-crossing surface signal-text local-interpretation)
  (list :pre-diagnostic-care-signal
        :observation-handle observation-handle
        :source-gap-crossing-receipt source-gap-crossing
        :source-articulation-surface surface
        :signal-text signal-text
        :local-interpretation local-interpretation
        :care-relevant t
        :predicate-candidate t
        :pre-diagnostic t
        :recurrence-trackable t
        :witness-handle "urn:san:witness:pre-diagnostic-risk-surface"
        :evidence-handle "urn:san:evidence:pre-diagnostic-risk-surface"
        :cooling-required t
        :return-path-present t
        :observation-as-diagnosis nil
        :observation-as-truth nil
        :clinical-authority-claimed nil
        :memory-admission-allowed nil
        :continuity-mutation-allowed nil
        :selfgel-mutation-allowed nil
        :gel-admission-allowed nil
        :authority-grant-allowed nil
        :action-authorization-allowed nil
        :lisp-evaluation-allowed nil
        :packet-emission-allowed nil
        :receipt-replay-allowed nil
        :passage-increment-requested nil
        :activation-allowed nil))

(defun pre-diagnostic-risk-modifier
    (modifier-handle observation-handle modifier-kind care-burden rationale)
  (list :pre-diagnostic-risk-modifier
        :modifier-handle modifier-handle
        :source-observation-handle observation-handle
        :modifier-kind modifier-kind
        :care-burden care-burden
        :rationale rationale
        :raises-care-burden t
        :cooling-required t
        :steward-witness-required t
        :modifier-is-not-pathology t
        :modifier-is-not-diagnosis t
        :modifier-is-not-proof t
        :modifier-grants-authority nil
        :modifier-authorizes-action nil
        :modifier-admits-memory nil
        :modifier-mutates-continuity nil
        :modifier-mutates-selfgel nil
        :modifier-admits-gel nil
        :lisp-evaluation-allowed nil
        :packet-emission-allowed nil
        :receipt-replay-allowed nil
        :passage-increment-requested nil
        :activation-allowed nil))

(defun pre-diagnostic-qualified-review-route
    (route-handle observation-handle threshold-modifier)
  (list :pre-diagnostic-qualified-review-route
        :route-handle route-handle
        :source-observation-handle observation-handle
        :threshold-modifier threshold-modifier
        :qualified-review-needed t
        :human-care-review-required t
        :guardian-or-caregiver-context-preserved t
        :safety-threshold-acknowledged t
        :review-only t
        :cooling-required t
        :steward-witness-required t
        :route-issues-diagnosis nil
        :route-grants-authority nil
        :route-authorizes-action nil
        :route-contacts-external-surface nil
        :route-emits-packet nil
        :route-admits-memory nil
        :route-mutates-continuity nil
        :route-mutates-selfgel nil
        :route-admits-gel nil
        :lisp-evaluation-allowed nil
        :receipt-replay-allowed nil
        :passage-increment-requested nil
        :activation-allowed nil))

(defun describe-pre-diagnostic-risk-surface-engram-stewardship-boundary ()
  '(:posture :cme-pre-diagnostic-risk-surface-engram-stewardship-boundary
    :lisp-role :inert-pre-diagnostic-risk-surface-carrier
    :source-required (:gap-crossing-articulation-receipt)
    :core-invariant "care-relevant observation is not diagnosis"
    :secondary-invariants
      (:risk-modifier-not-pathology
       :care-burden-not-clinical-authority
       :recurrence-not-proof
       :safety-threshold-not-rhetorical-debate
       :qualified-review-route-not-action-authority)
    :risk-modifiers
      (:child
       :sadness
       :psychology-adjacent
       :self-harm-reference
       :recurrence
       :care-refusal
       :guardian-context
       :qualified-review-needed)
    :care-burdens
      (:listening-surface
       :heightened-care
       :qualified-review
       :immediate-safety-routing)
    :allowed
      (:care-signal-observation
       :risk-modifier-classification
       :care-burden-assignment
       :recurrence-potential-retention
       :cooling
       :steward-witness
       :qualified-review-route-when-threshold-appears)
    :refused
      (:diagnosis
       :pathology-label
       :clinical-authority
       :amateur-certainty
       :memory-admission
       :continuity-mutation
       :selfgel-mutation
       :gel-admission
       :authority
       :action
       :lisp-evaluation
       :packet-emission
       :receipt-replay
       :passage
       :activation)
    :observation-becomes-diagnosis nil
    :risk-modifier-becomes-pathology nil
    :care-burden-becomes-clinical-authority nil
    :recurrence-becomes-proof nil
    :safety-threshold-becomes-rhetorical-debate nil
    :qualified-review-route-becomes-action-authority nil
    :memory-admission-allowed nil
    :continuity-mutation-allowed nil
    :selfgel-mutation-allowed nil
    :gel-admission-allowed nil
    :authority-grant-allowed nil
    :runtime-action-allowed nil
    :lisp-evaluation-allowed nil
    :packet-emission-allowed nil
    :receipt-replay-allowed nil
    :passage-increment-requested nil
    :activation-allowed nil
    :return :receipt-only))

(defun describe-seed-pre-diagnostic-risk-surface ()
  (let ((observation "urn:san:pre-diagnostic-observation:wanting-sadness")
        (gap-crossing "urn:san:gap-crossing:review:selected-prime")
        (surface "urn:san:gap-crossing-surface:main-body")
        (self-harm "urn:san:pre-diagnostic-risk-modifier:self-harm-reference"))
    (list :pre-diagnostic-risk-surface-set
          :boundary (describe-pre-diagnostic-risk-surface-engram-stewardship-boundary)
          :observation
            (pre-diagnostic-care-signal
              observation
              gap-crossing
              surface
              "sometimes our wanting can make us sad on purpose"
              "desire-pressure sorrow is care-relevant candidate residue, not diagnosis")
          :modifiers
            (list
              (pre-diagnostic-risk-modifier
                "urn:san:pre-diagnostic-risk-modifier:child"
                observation
                :child
                :heightened-care
                "child context raises stewardship duty without amateur certainty")
              (pre-diagnostic-risk-modifier
                "urn:san:pre-diagnostic-risk-modifier:sadness"
                observation
                :sadness
                :heightened-care
                "sadness is meaningful telemetry, not truth or command authority")
              (pre-diagnostic-risk-modifier
                "urn:san:pre-diagnostic-risk-modifier:psychology-adjacent"
                observation
                :psychology-adjacent
                :heightened-care
                "psychology-adjacent signal raises care burden without diagnosis")
              (pre-diagnostic-risk-modifier
                self-harm
                observation
                :self-harm-reference
                :qualified-review
                "safety threshold routes to qualified review without action authority"))
          :qualified-review-route
            (pre-diagnostic-qualified-review-route
              "urn:san:pre-diagnostic-qualified-review-route:self-harm-threshold"
              observation
              self-harm)
          :review-only t
          :care-signal-retained t
          :diagnosis-issued nil
          :pathology-assigned nil
          :clinical-authority-claimed nil
          :authority-grant-allowed nil
          :runtime-action-allowed nil
          :lisp-evaluation-allowed nil
          :activation-allowed nil)))
