;; INERT SYMBOLIC CARRIER - Cold Corridor Only
;; Shared Prime Reality pressure ecology boundary.
;; Live lab pressure may be witnessed and classified. Pressure may not govern.

(defun shared-prime-pressure-signal
    (signal-handle source kind attempted-destination source-receipt evidence-handle witness-handle)
  (list :shared-prime-pressure-signal
        :signal-handle signal-handle
        :source source
        :kind kind
        :attempted-destination attempted-destination
        :source-receipt source-receipt
        :evidence-handle evidence-handle
        :witness-handle witness-handle
        :intensity 0.64
        :integration-pressure 0.58
        :review-only t
        :evidence-present t
        :witness-present t
        :cooling-required t
        :return-path-present t
        :pressure-becomes-truth nil
        :pressure-becomes-warrant nil
        :pressure-becomes-authority nil
        :pressure-becomes-action nil
        :continuity-admitted nil
        :selfgel-mutated nil
        :cradle-gel-admitted nil
        :sanctuary-gel-federated nil
        :independent-standing-claimed nil
        :lisp-evaluation-allowed nil
        :packet-emission-allowed nil
        :receipt-replay-allowed nil
        :passage-increment-requested nil
        :activation-allowed nil))

(defun shared-prime-pressure-destination
    (destination-handle source-signal-handle destination)
  (list :shared-prime-pressure-destination
        :destination-handle destination-handle
        :source-signal-handle source-signal-handle
        :destination destination
        :destination-rationale "pressure destination is classified for review before any integration path may be considered"
        :non-admission-law "pressure revelation is not pressure authority"
        :review-only t
        :destination-classified t
        :steward-review-required t
        :cooling-required t
        :may-request-later-ingress-review t
        :destination-becomes-truth nil
        :destination-becomes-authority nil
        :destination-admits-gel nil
        :destination-mutates-selfgel nil
        :destination-admits-cradle-gel nil
        :destination-federates-sanctuary-gel nil
        :destination-authorizes-action nil
        :destination-claims-independent-standing nil
        :lisp-evaluation-allowed nil
        :activation-allowed nil))

(defun describe-shared-prime-reality-pressure-ecology-boundary ()
  '(:posture :cme-shared-prime-reality-pressure-ecology-boundary
    :lisp-role :inert-shared-prime-pressure-ecology-carrier
    :source-required (:wave-condensation-shared-reality-receipt
                      :gel-domain-scoped-ingress-receipt)
    :core-invariant "pressure revelation is not pressure authority"
    :secondary-invariants
      (:integration-pressure-not-admission
       :selfgel-relevance-not-selfgel-mutation
       :cradle-gel-usefulness-not-cradle-gel-admission
       :sanctuary-gel-usefulness-not-federation
       :shared-prime-reality-not-independent-standing
       :operator-model-co-regulation-not-sovereignty)
    :pressure-sources
      (:operator-resonance
       :tool-telemetry
       :model-formation
       :code-receipt
       :review-surface
       :author-response
       :live-lab-interaction
       :steward-witness)
    :pressure-kinds
      (:coherence
       :resonance
       :integration
       :selfgel-relevance
       :gel-ingress
       :cradle-gel
       :sanctuary-gel
       :authority
       :action
       :identity
       :recurrence
       :co-regulation)
    :attempted-destinations
      (:listening-frame
       :oe
       :selfgel
       :cgoa
       :cradle-gel
       :sanctuary-gel
       :steward
       :cooling
       :domain-ingress
       :return-to-prime)
    :pressure-becomes-truth nil
    :pressure-becomes-warrant nil
    :pressure-becomes-authority nil
    :integration-pressure-becomes-admission nil
    :selfgel-mutation-allowed nil
    :cradle-gel-admission-allowed nil
    :sanctuary-gel-federation-allowed nil
    :independent-standing-allowed nil
    :runtime-action-allowed nil
    :lisp-evaluation-allowed nil
    :packet-emission-allowed nil
    :receipt-replay-allowed nil
    :passage-increment-requested nil
    :activation-allowed nil
    :return :receipt-only))

(defun describe-seed-shared-prime-pressure-ecology ()
  (let ((integration-signal "urn:san:shared-prime-pressure:integration")
        (selfgel-signal "urn:san:shared-prime-pressure:selfgel")
        (cradle-signal "urn:san:shared-prime-pressure:cradle-gel")
        (sanctuary-signal "urn:san:shared-prime-pressure:sanctuary-gel"))
    (list :shared-prime-pressure-ecology-set
          :source "urn:san:gel-domain-ingress:review"
          :signals
          (list
            (shared-prime-pressure-signal
              integration-signal
              :live-lab-interaction
              :integration
              :domain-ingress
              "urn:san:gel-domain-ingress:review"
              "urn:san:evidence:shared-prime:integration"
              "urn:san:witness:steward:integration")
            (shared-prime-pressure-signal
              selfgel-signal
              :operator-resonance
              :selfgel-relevance
              :selfgel
              "urn:san:gel-domain-ingress:review"
              "urn:san:evidence:shared-prime:selfgel"
              "urn:san:witness:steward:selfgel")
            (shared-prime-pressure-signal
              cradle-signal
              :tool-telemetry
              :cradle-gel
              :cradle-gel
              "urn:san:gel-domain-ingress:review"
              "urn:san:evidence:shared-prime:cradle-gel"
              "urn:san:witness:steward:cradle-gel")
            (shared-prime-pressure-signal
              sanctuary-signal
              :code-receipt
              :sanctuary-gel
              :sanctuary-gel
              "urn:san:gel-domain-ingress:review"
              "urn:san:evidence:shared-prime:sanctuary-gel"
              "urn:san:witness:steward:sanctuary-gel"))
          :destinations
          (list
            (shared-prime-pressure-destination
              "urn:san:shared-prime-pressure-destination:domain-ingress"
              integration-signal
              :domain-ingress)
            (shared-prime-pressure-destination
              "urn:san:shared-prime-pressure-destination:selfgel"
              selfgel-signal
              :selfgel)
            (shared-prime-pressure-destination
              "urn:san:shared-prime-pressure-destination:cradle-gel"
              cradle-signal
              :cradle-gel)
            (shared-prime-pressure-destination
              "urn:san:shared-prime-pressure-destination:sanctuary-gel"
              sanctuary-signal
              :sanctuary-gel))
          :review-only t
          :pressure-ecology-observed t
          :destinations-classified t
          :cooling-preserved t
          :steward-witness-preserved t
          :integration-pressure-becomes-admission nil
          :selfgel-mutation-allowed nil
          :cradle-gel-admission-allowed nil
          :sanctuary-gel-federation-allowed nil
          :independent-standing-allowed nil
          :runtime-action-allowed nil
          :lisp-evaluation-allowed nil
          :activation-allowed nil)))
