(in-package :sli-core)

;; INERT SYMBOLIC CARRIER ONLY.
;; Meaning shells are unfinished pre-engram bodies in Engineered Cognition.
;; They let Root, propositional, procedural, and perspectival material form
;; without becoming Self, authority, continuity, engram, or action.

(defun meaning-shell-candidate
    (shell-handle root-anchor tier predicate procedural-trace trunk branches source-petal domain predicate-class)
  (list :meaning-shell-candidate
        :shell-handle shell-handle
        :root-anchor root-anchor
        :tier tier
        :propositional-predicate predicate
        :procedural-trace procedural-trace
        :perspectival-trunk trunk
        :perspectival-branches branches
        :source-petal source-petal
        :domain-template-pack domain
        :predicate-class predicate-class
        :candidate-only t
        :review-only t
        :inert t
        :compost-allowed t
        :closure-claimed nil
        :engram-claimed nil
        :self-attribution-claimed nil
        :authority-requested nil
        :activation-requested nil))

(defun compost-disposition (compost-handle source-shell note)
  (list :compost-disposition
        :compost-handle compost-handle
        :source-shell source-shell
        :retained-near-cselfgel t
        :attributed-to-self nil
        :grants-continuity nil
        :review-only t
        :inert t
        :resolution-note note))

(defun describe-meaning-shell-boundary ()
  '(:posture :engineered-cognition-meaning-shell-boundary
    :carrier-form :unfinished-pre-engram-body
    :root-tier :essence-anchor
    :tier-1 :propositional-knowing-shell
    :tier-2-plus :procedural-knowing-shell
    :perspectival-tier :trunk-and-branch-composite
    :shell-may-form t
    :shell-may-branch t
    :shell-may-compost t
    :shell-may-nominate t
    :shell-may-become-engram nil
    :shell-may-append-selfgel nil
    :shell-may-append-cselfgel nil
    :shell-may-authorize nil
    :shell-may-activate nil
    :shell-may-evaluate-lisp nil
    :shell-may-admit-continuity nil
    :shell-may-mutate-identity nil
    :compost-may-retain-near-cselfgel t
    :compost-may-attribute-to-self nil
    :compost-may-grant-continuity nil
    :spline-outcomes (:held-open :closed-candidate :merge-to-base-candidate :new-root-candidate :composted :refused)
    :unbonded-ingress :neutral-clamp
    :domain-inheritance nil
    :packet-emission nil
    :receipt-replay nil
    :passage-increment nil
    :return :receipt-only))

(defun describe-seed-meaning-shells ()
  (list
    (meaning-shell-candidate
      "urn:san:ec-shell:root-listening"
      "root://listening-frame"
      :root
      nil
      nil
      nil
      nil
      "urn:san:sli-lisp-petal:01"
      :industrial
      :industrial-listening-root)
    (meaning-shell-candidate
      "urn:san:ec-shell:propositional-compass"
      "root://compass-orientation"
      :propositional-tier-1
      "orientation-is-not-authority"
      nil
      nil
      nil
      "urn:san:sli-lisp-petal:02"
      :industrial
      :industrial-compass-proposition)
    (meaning-shell-candidate
      "urn:san:ec-shell:procedural-cleaving"
      "root://cleaving-discernment"
      :procedural-tier-2-plus
      nil
      "listen->orient->cleave->evaluate->contemplate"
      nil
      nil
      "urn:san:sli-lisp-petal:03"
      :industrial
      :industrial-cleaving-procedure)
    (meaning-shell-candidate
      "urn:san:ec-shell:perspectival-inner-chamber"
      "root://inner-chamber"
      :perspectival-composite
      nil
      nil
      "cme-ec-action-body"
      '("root-trunk" "procedure-branch" "compost-return")
      "urn:san:sli-lisp-petal:03"
      :industrial
      :industrial-perspectival-inner-chamber)))

(defun describe-seed-compost ()
  (list
    (compost-disposition
      "urn:san:ec-compost:cleaving-attempt"
      "urn:san:ec-shell:procedural-cleaving"
      "Attempt retained as non-Self evidence for later review.")))
