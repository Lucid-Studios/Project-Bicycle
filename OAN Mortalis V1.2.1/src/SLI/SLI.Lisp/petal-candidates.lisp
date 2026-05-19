(in-package :sli-core)

;; INERT SYMBOLIC CARRIER ONLY.
;; The 42 petals are Skills, Abilities, and Talents in candidate posture.
;; They are taught through traversal and witness; this file must not force closure.
;; Lisp extensions are expected to enter through this petal-template posture,
;; not as bespoke organs that smuggle their own authority.
;; GoA-level SLI.Lisp Control Matrix remains Steward-only; Engineered Cognition
;; receives only a lesser templated petal form for extension construction.

(defun petal-candidate (ordinal kind name source domain predicate-class)
  (list :petal-candidate
        :ordinal ordinal
        :kind kind
        :name name
        :source source
        :extension-surface :engineered-cognition-petal-template
        :domain-template-pack domain
        :predicate-class predicate-class
        :cross-domain-inheritance-requested nil
        :goa-control-matrix-access nil
        :steward-control-matrix-requested nil
        :candidate-only t
        :review-only t
        :inert t
        :authority-requested nil
        :closure-claimed nil
        :activation-requested nil))

(defun describe-petal-candidate-boundary ()
  '(:posture :sli-lisp-petal-candidate-boundary
    :petal-count-ceiling 42
    :candidate-kinds (:skill :ability :talent)
    :extension-form :templated-petal-candidate
    :extension-surface :engineered-cognition-petal-template
    :domain-template-packs (:personal :enterprise :industrial :civic :governance :special)
    :domain-pack-isolation :required
    :industrial-inherits-civic nil
    :industrial-inherits-governance nil
    :civic-inherits-industrial nil
    :governance-inherits-industrial nil
    :predicate-class-required t
    :goa-control-matrix :steward-only
    :ec-control-form :lesser-template-extension
    :bespoke-extension-authority nil
    :skill :discrete-callable-form-candidate
    :ability :composed-capacity-candidate
    :talent :stabilized-expression-candidate
    :typing-posture :distributed-through-discourse
    :typing-not-one-line-definition t
    :gap-status :held-open
    :petal-may-express-capacity t
    :petal-may-self-authorize nil
    :petal-may-force-closure nil
    :petal-may-activate nil
    :return :receipt-only))

(defun describe-seed-petal-candidates ()
  (list
    (petal-candidate 1 :skill :listening-frame "gnomeronacorde-flask" :industrial :industrial-listening)
    (petal-candidate 2 :ability :compass-orientation "gnomeronacorde-compass" :industrial :industrial-orientation)
    (petal-candidate 3 :talent :cleaving-discernment "sli-lisp-inner-chamber" :industrial :industrial-discernment)))
