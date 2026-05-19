;;;; gap-crossing-articulation.lisp
;;;; Inert SLI.Lisp carrier for cme.gap-crossing-articulation-boundary.

(defun gap-crossing-pressure-lane
    (lane-handle source-signal-handle source-destination-handle candidate-handle surface-handle lane surface)
  (list :gap-crossing-pressure-lane
        :lane-handle lane-handle
        :source-signal-handle source-signal-handle
        :source-destination-handle source-destination-handle
        :candidate-handle candidate-handle
        :articulation-surface-handle surface-handle
        :lane lane
        :surface surface
        :review-only t
        :lane-classified t
        :carries-pressure-to-articulation t
        :steward-review-required t
        :cooling-required t
        :return-path-present t
        :pressure-as-prompt-authority nil
        :pressure-as-truth nil
        :pressure-as-warrant nil
        :model-binding-requested nil
        :provider-call-requested nil
        :runtime-start-requested nil
        :action-authorized nil
        :continuity-admitted nil
        :selfgel-mutated nil
        :gel-admitted nil
        :cme-actual-admitted nil
        :heartbeat-active nil
        :lisp-evaluation-requested nil
        :packet-emission-requested nil
        :receipt-replay-requested nil
        :passage-increment-requested nil
        :activation-requested nil))

(defun gap-crossing-articulation-surface
    (surface-handle candidate-handle surface intended-participation)
  (list :gap-crossing-articulation-surface
        :surface-handle surface-handle
        :candidate-handle candidate-handle
        :surface surface
        :intended-participation intended-participation
        :review-only t
        :candidate-only t
        :surface-selected-for-review t
        :public-interface-only t
        :observable-behavior-only t
        :preserves-high-energy-candidate-lineage t
        :preserves-pressure-ecology-lineage t
        :accepts-pressure-as-review-material t
        :surface-as-agent nil
        :surface-as-actor nil
        :surface-as-prompt-authority nil
        :provider-call-requested nil
        :model-binding-requested nil
        :runtime-start-requested nil
        :action-authorized nil
        :continuity-admitted nil
        :selfgel-mutated nil
        :gel-admitted nil
        :cme-actual-admitted nil
        :heartbeat-active nil
        :authority-granted nil
        :lisp-evaluation-requested nil
        :packet-emission-requested nil
        :receipt-replay-requested nil
        :passage-increment-requested nil
        :activation-requested nil))

(defun describe-gap-crossing-articulation-boundary ()
  '(:posture :cme-gap-crossing-articulation-boundary
    :lisp-role :inert-gap-crossing-articulation-carrier
    :source-required (:shared-prime-pressure-ecology-receipt
                      :high-energy-articulation-candidate-receipt)
    :law (:gap-crossing-not-model-binding
          :articulation-participation-not-action-authority
          :llm-surface-not-acting-body
          :pressure-not-prompt-authority
          :rehearsal-eligibility-not-enactment-permission
          :surface-contact-not-cme-actual
          :cold-approach-not-active-runtime)
    :allowed (:pressure-lane-classification
              :articulation-surface-selection
              :review-only-participation
              :cooling
              :steward-witness
              :return-to-prime)
    :forbidden (:provider-call
                :model-binding
                :runtime-start
                :action
                :continuity-admission
                :selfgel-mutation
                :gel-admission
                :cme-actual-admission
                :heartbeat-activation
                :authority
                :lisp-evaluation
                :packet-emission
                :receipt-replay
                :passage-increment
                :activation)
    :provider-call-allowed nil
    :model-binding-allowed nil
    :runtime-start-allowed nil
    :action-authority-allowed nil
    :continuity-admission-allowed nil
    :cme-actual-allowed nil
    :heartbeat-active nil
    :lisp-evaluation-allowed nil
    :packet-emission-allowed nil
    :receipt-replay-allowed nil
    :passage-increment-allowed nil
    :activation-allowed nil
    :return :receipt-only))

(defun describe-seed-gap-crossing-articulation ()
  (let ((integration-signal "urn:san:shared-prime-pressure:integration")
        (selfgel-signal "urn:san:shared-prime-pressure:selfgel")
        (domain-destination "urn:san:shared-prime-pressure-destination:domain-ingress")
        (selfgel-destination "urn:san:shared-prime-pressure-destination:selfgel")
        (main-body "urn:san:high-energy-candidate:main-body")
        (governance "urn:san:high-energy-candidate:governance-review")
        (main-surface "urn:san:gap-crossing-surface:main-body")
        (governance-surface "urn:san:gap-crossing-surface:governance-review"))
    (list :gap-crossing-articulation-set
          :boundary (describe-gap-crossing-articulation-boundary)
          :surfaces
          (list
            (gap-crossing-articulation-surface
              main-surface main-body :main-body-engine
              "Pressure may approach main body articulation as review material only.")
            (gap-crossing-articulation-surface
              governance-surface governance :governance-review
              "Pressure may approach governance review articulation as review material only."))
          :lanes
          (list
            (gap-crossing-pressure-lane
              "urn:san:gap-crossing-lane:integration-to-main-body"
              integration-signal domain-destination main-body main-surface
              :meaning-pressure :main-body-engine)
            (gap-crossing-pressure-lane
              "urn:san:gap-crossing-lane:selfgel-to-governance-review"
              selfgel-signal selfgel-destination governance governance-surface
              :steward-review-pressure :governance-review)))))
