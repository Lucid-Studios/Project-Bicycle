;; INERT SYMBOLIC CARRIER - Cold Corridor Only
;; GEL domain-scoped ingress boundary.
;; Candidate meaning may approach a lawful world. It may not admit itself.

(defun gel-domain-ingress-candidate
    (candidate-handle source-epps-receipt source-bridge-receipt domain evidence-ceiling)
  (list :gel-domain-ingress-candidate
        :candidate-handle candidate-handle
        :source-epps-receipt source-epps-receipt
        :source-bridge-receipt source-bridge-receipt
        :domain domain
        :evidence-ceiling evidence-ceiling
        :post-gel-formation t
        :pre-gel-admission t
        :candidate-only t
        :review-only t
        :formed-substrate t
        :admitted-gel nil
        :admitted-memory nil
        :continuity-mutated nil
        :selfgel-mutated nil
        :authority-granted nil
        :action-authorized nil
        :lisp-evaluation-allowed nil
        :packet-emission-allowed nil
        :passage-increment-requested nil
        :activation-allowed nil))

(defun describe-gel-domain-scoped-ingress-boundary ()
  '(:posture :cme-gel-domain-scoped-ingress-boundary
    :lisp-role :inert-gel-domain-ingress-carrier
    :source-required (:engram-predicate-precursor-stream-receipt
                      :peer-review-predicate-bridge-receipt)
    :core-invariant "formed substrate is not admitted GEL"
    :secondary-invariants
      (:domain-fit-not-admission
       :evidence-ceiling-not-portable
       :recommendation-not-admission
       :governance-survivorship-not-proof
       :seed-may-transfer-warrant-does-not)
    :ingress-cycle
      (:source-event
       :telemetry-precipitation
       :epps-residue
       :bridge-synthesis
       :candidate-substrate
       :domain-classification
       :evidence-ceiling-assignment
       :cooling
       :steward-review
       :recommendation)
    :domains
      (:scholarly-review
       :engineering-telemetry
       :operator-doctrine
       :pedagogy
       :civic-governance
       :legal-compliance
       :medical-clinical
       :personification
       :security
       :military-defense-closed
       :special-case)
    :evidence-ceilings
      (:interpretive
       :operational
       :reproducible
       :regulated
       :licensed
       :clinical
       :special-case-held
       :closed)
    :boundary-requirements
      (:cold-epps-required t
       :cold-peer-review-bridge-required t
       :candidate-substrate-required t
       :domain-scope-required t
       :evidence-ceiling-required t
       :cooling-required t
       :steward-review-required t)
    :formed-substrate-becomes-admitted-gel nil
    :domain-fit-becomes-admission nil
    :evidence-ceiling-becomes-portable nil
    :recommendation-becomes-admission nil
    :governance-survivorship-becomes-proof nil
    :memory-admission-allowed nil
    :continuity-mutation-allowed nil
    :selfgel-mutation-allowed nil
    :authority-grant-allowed nil
    :runtime-action-allowed nil
    :lisp-evaluation-allowed nil
    :packet-emission-allowed nil
    :receipt-replay-allowed nil
    :passage-increment-requested nil
    :activation-allowed nil
    :return :receipt-only))

(defun describe-seed-gel-domain-scoped-ingress ()
  (list :gel-domain-scoped-ingress-set
        :source "urn:san:peer-review-bridge:review"
        :candidates
        (list
          (gel-domain-ingress-candidate
            "urn:san:gel-domain-ingress:candidate:scholarly-review"
            "urn:san:epps:review"
            "urn:san:peer-review-bridge:review"
            :scholarly-review
            :interpretive)
          (gel-domain-ingress-candidate
            "urn:san:gel-domain-ingress:candidate:special-case-held"
            "urn:san:epps:review"
            "urn:san:peer-review-bridge:review"
            :special-case
            :special-case-held))
        :review-only t
        :candidate-substrate-retained t
        :domain-scoped t
        :evidence-ceiling-assigned t
        :cooling-preserved t
        :steward-review-required t
        :recommendation-is-external-to-admission t
        :formed-substrate-becomes-admitted-gel nil
        :domain-fit-becomes-admission nil
        :evidence-ceiling-becomes-portable nil
        :recommendation-becomes-admission nil
        :memory-admission-allowed nil
        :continuity-mutation-allowed nil
        :selfgel-mutation-allowed nil
        :authority-grant-allowed nil
        :runtime-action-allowed nil
        :lisp-evaluation-allowed nil
        :activation-allowed nil))
