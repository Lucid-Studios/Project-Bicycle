;;;; high-energy-articulation-candidate.lisp
;;;; Inert SLI.Lisp carrier for cme.high-energy-articulation-candidate-boundary.

(defun high-energy-articulation-candidate
    (candidate-handle candidate-role provider-family model-line)
  (list :high-energy-articulation-candidate
        :candidate-handle candidate-handle
        :candidate-role candidate-role
        :provider-family provider-family
        :model-line model-line
        :intended-role :candidate-articulation-body
        :zed-delta-chamber-receipt "urn:san:zed-delta-chamber:review"
        :zed-delta-origin-handle "urn:san:zed-delta:origin:0-0-0"
        :conditional-oe-handle "urn:san:coe:primary"
        :conditional-selfgel-handle "urn:san:cselfgel:primary"
        :telemetry-shape-handle "urn:san:telemetry-shape:high-energy-candidate"
        :public-documentation-handle "urn:san:public-interface:documented"
        :witness-handle "urn:san:witness:high-energy-articulation-candidate"
        :custody-owner "steward"
        :review-only t
        :candidate-only t
        :role-typed t
        :public-interface-only t
        :observable-behavior-only t
        :preserves-chamber-lineage t
        :preserves-conditional-oe-lineage t
        :preserves-conditional-selfgel-lineage t
        :provider-call-requested nil
        :model-binding-requested nil
        :hidden-substrate-claimed nil
        :weight-access-claimed nil
        :training-data-claimed nil
        :persistent-memory-claimed nil
        :runtime-identity-claimed nil
        :heartbeat-activation-requested nil
        :cme-actual-admission-requested nil
        :action-authorization-requested nil
        :continuity-admission-requested nil
        :authority-requested nil
        :lisp-evaluation-requested nil
        :packet-emission-requested nil
        :receipt-replay-requested nil
        :passage-increment-requested nil
        :activation-requested nil))

(defun describe-high-energy-articulation-candidate-boundary ()
  '(:posture :cme-high-energy-articulation-candidate-boundary
    :lisp-role :inert-high-energy-articulation-candidate-carrier
    :source-required (:zed-delta-chamber-formation-receipt)
    :core-invariant "candidate engine may be named, but naming is not binding"
    :candidate-engine-named t
    :candidate-role-typed t
    :public-interface-referenced t
    :observable-behavior-only t
    :provider-call-allowed nil
    :provider-visible-access-allowed nil
    :model-context-export-allowed nil
    :model-binding-allowed nil
    :hidden-substrate-claimed nil
    :hidden-internals-mapped nil
    :weights-claimed nil
    :training-data-claimed nil
    :persistent-memory-claimed nil
    :runtime-identity-claimed nil
    :heartbeat-active nil
    :cme-actual-admitted nil
    :runtime-start-allowed nil
    :action-authorized nil
    :continuity-admitted nil
    :authority-granted nil
    :lisp-evaluation-allowed nil
    :packet-emission-allowed nil
    :receipt-replay-allowed nil
    :passage-increment-allowed nil
    :activation-allowed nil
    :candidate-roles (:main-body-engine-candidate
                      :governance-review-candidate
                      :instantiated-cme-test-body-candidate
                      :comparative-universality-candidate
                      :local-slm-candidate)
    :doctrine ("A model may be identified as candidate engine."
               "A candidate engine may not become an active CME."
               "A provider interface may be described."
               "A provider interface may not be called."
               "Observable behavior is not hidden substrate knowledge."
               "Public interface success is not semantic warrant."
               "CME.Actual remains refused.")))

(defun describe-seed-high-energy-articulation-candidates ()
  (list :high-energy-articulation-candidate-set
        :source "urn:san:zed-delta-chamber:review"
        :candidates
        (list
          (high-energy-articulation-candidate
            "urn:san:high-energy-candidate:main-body"
            :main-body-engine-candidate
            "OpenAI"
            "GPT")
          (high-energy-articulation-candidate
            "urn:san:high-energy-candidate:governance-review"
            :governance-review-candidate
            "OpenAI"
            "Codex")
          (high-energy-articulation-candidate
            "urn:san:high-energy-candidate:cme-test-body"
            :instantiated-cme-test-body-candidate
            "OpenAI"
            "mini-or-micro")
          (high-energy-articulation-candidate
            "urn:san:high-energy-candidate:comparative"
            :comparative-universality-candidate
            "comparative-provider"
            "public-interface-only")
          (high-energy-articulation-candidate
            "urn:san:high-energy-candidate:local-slm"
            :local-slm-candidate
            "local-runtime"
            "deferred-slm"))
        :provider-call-allowed nil
        :model-binding-allowed nil
        :heartbeat-active nil
        :cme-actual-admitted nil
        :review-only t))
