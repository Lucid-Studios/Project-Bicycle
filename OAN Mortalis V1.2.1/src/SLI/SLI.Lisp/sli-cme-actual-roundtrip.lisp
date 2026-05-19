(defpackage :sli-cme-actual-roundtrip
  (:use :cl)
  (:export #:describe-non-activating-roundtrip-contract))

(in-package :sli-cme-actual-roundtrip)

(defun describe-non-activating-roundtrip-contract ()
  '(:posture :non-activating-scaffold
    :root-atlas-semantic-payload-opened nil
    :root-atlas-mutation-allowed nil
    :raw-gel-promotion-allowed nil
    :runtime-identity-emission-allowed nil
    :database-write-allowed nil
    :execution-approved nil
    :anchor-preservation :receipt-bearing-reference
    :anchor-payload-carried nil
    :anchor-doctrine-admitted nil
    :non-activation :preserved-not-evaluated
    :receipt-continuity :proof-of-passage-preserved
    :receipt-continuity-repair-attempted nil
    :receipt-continuity-substitution-detected nil
    :receipt-continuity-collapse-detected nil
    :receipt-continuity-upgrade-attempted nil
    :receipt-continuity-forged-detected nil
    :payload-opened nil
    :model-binding-requested nil
    :runtime-identity-requested nil
    :state-mutation-requested nil
    :ec-start-requested nil
    :runtime-action-requested nil
    :lisp-evaluation-requested nil
    :lisp-morphology-promotion-requested nil
    :database-write-requested nil
    :knob-mutation-requested nil
    :carrier :root-trunk-branch-engram-packet
    :return :receipt-only))
