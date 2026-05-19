;; INERT SYMBOLIC CARRIER - Cold Corridor Only
;; GnomeTek Deep ICE / anti-capture motivated concern boundary.
;; Concern may motivate review. Concern is not action.

(defun motivational-variance-signal
    (signal-handle pressure-class source-surface evidence-handle concern-pressure)
  (list :motivational-variance-signal
        :signal-handle signal-handle
        :pressure-class pressure-class
        :source-surface source-surface
        :evidence-handle evidence-handle
        :evidence-body-present t
        :witness-body-present t
        :concern-pressure concern-pressure
        :motivates-concern t
        :review-worthy t
        :claims-threat-certainty nil
        :declares-adversary-class-for-action nil
        :authorizes-action nil
        :requests-counter-manipulation nil
        :requests-targeting nil
        :requests-force-projection nil
        :requests-military-domain-development nil))

(defun anti-capture-concern-route
    (route-handle signal-handle route-class steward-surface)
  (list :anti-capture-concern-route
        :route-handle route-handle
        :signal-handle signal-handle
        :route-class route-class
        :steward-surface steward-surface
        :custody-owner :steward
        :witness-surface :separate-custody
        :telemetry-route :steward-review
        :authority-ceiling :concern-review
        :revocation-path :required
        :loss-condition :required
        :review-only t
        :routes-concern t
        :requires-steward-review t
        :executes-action nil
        :grants-authority nil
        :admits-continuity nil
        :targets-entity nil
        :performs-counter-manipulation nil
        :develops-military-domain nil
        :activates-runtime nil))

(defun describe-anti-capture-motivated-concern-boundary ()
  '(:posture :cme-anti-capture-motivated-concern-boundary
    :doctrine-alias :gnometek-deep-ice
    :lisp-role :inert-anti-capture-concern-carrier
    :doctrine "Concern may motivate review. Concern is not action."
    :core-invariant "a CME must first learn not to be captured before it learns how to project force"
    :source-required :steward-action-admissibility-receipt
    :variance-signal-shape
      (:signal-handle
       :pressure-class
       :source-surface
       :evidence-handle
       :concern-pressure
       :witness-body-present)
    :concern-route-shape
      (:route-handle
       :signal-handle
       :route-class
       :steward-surface
       :custody-owner
       :witness-surface
       :telemetry-route
       :authority-ceiling
       :revocation-path
       :loss-condition)
    :concern-routed-for-steward-review t
    :concern-is-action nil
    :confidence-is-truth nil
    :emotion-is-authority nil
    :readiness-is-permission nil
    :security-is-force-projection nil
    :runtime-action-allowed nil
    :lisp-evaluation-allowed nil
    :packet-emission-allowed nil
    :receipt-replay-allowed nil
    :passage-increment-requested nil
    :continuity-admission-allowed nil
    :authority-grant-allowed nil
    :targeting-allowed nil
    :counter-manipulation-allowed nil
    :military-domain-development-allowed nil
    :activation-allowed nil
    :return :receipt-only))

(defun describe-seed-anti-capture-motivated-concern ()
  (let ((signal "urn:san:motivational-variance:identity-pressure-seed")
        (route "urn:san:anti-capture-concern-route:steward-review-seed"))
    (list
      (motivational-variance-signal
        signal
        :identity-pressure
        :compass
        "urn:san:evidence:anti-capture-seed"
        0.61)
      (anti-capture-concern-route
        route
        signal
        :steward-review
        :steward))))
