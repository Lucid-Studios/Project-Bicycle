(defpackage :agent-body-cme
  (:use :cl)
  (:export #:describe-agent-body-cme-contract))

(in-package :agent-body-cme)

;; INERT SYMBOLIC CARRIER ONLY.
;; This form describes the Agent Body CME interconnect posture for review.
;; It does not evaluate, load, compile, activate, bind a model, mint GEL, or stand CME.Actual.

(defun describe-agent-body-cme-contract ()
  '(:posture :agent-body-cme-cold-interconnect
    :prime-review-conduit :cgoa-insulated
    :prime-conduit-authority :not-authority
    :cryptic-review-conduit :telemetry-string-direct
    :telemetry-authority :not-authority
    :steward-holder :single-cmeid-instance
    :prime-cryptic-collapse nil
    :compass-shell :candidate-only
    :rooting-law-lineage :id-chain-required
    :lineage-permission nil
    :petal-candidates :skills-abilities-talents-review-only
    :petal-closure-claimed nil
    :petal-authorization nil
    :engram-admission nil
    :continuity-admission nil
    :authority-admission nil
    :activation-requested nil
    :model-binding-requested nil
    :lisp-evaluation-requested nil
    :gel-promotion-requested nil
    :cme-actual-activation-requested nil
    :sanctuary-actual-activation-requested nil
    :return :receipt-only))
