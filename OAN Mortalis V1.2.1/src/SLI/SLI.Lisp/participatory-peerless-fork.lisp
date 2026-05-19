(in-package :sli-core)

;; INERT SYMBOLIC CARRIER ONLY.
;; The Participatory to Peerless fork names governed individuation.
;; Participation is admissible capacity.
;; Personification is expressive rendering.
;; Peerless formation is non-substitutable continuity under witness.
;; Participation can exist without personification.
;; Personification cannot safely operate without participatory SelfGEL predicate footing.
;; Peerless may emerge through witnessed participation over delta, but it is not sovereignty.

(defun participatory-predicate-structure
    (handle selfgel-predicate role custody memory-posture action-limit witness-path source-shell)
  (list :participatory-predicate-structure
        :structure-handle handle
        :selfgel-predicate-handle selfgel-predicate
        :role-boundary role
        :custody-boundary custody
        :memory-posture memory-posture
        :action-limit action-limit
        :witness-path witness-path
        :source-meaning-shell source-shell
        :personification-required nil
        :review-only t
        :inert t
        :authority-requested nil
        :continuity-claimed nil
        :activation-requested nil))

(defun personification-surface (handle expressive-name source-participatory)
  (list :personification-surface
        :surface-handle handle
        :expressive-name expressive-name
        :source-participatory-handle source-participatory
        :participatory-structure-present t
        :expressive-only t
        :review-only t
        :inert t
        :authority-claimed nil
        :standing-claimed nil
        :continuity-claimed nil
        :activation-requested nil))

(defun participation-delta-trace (handle ordinal source-participatory source-shell delta)
  (list :participation-delta-trace
        :trace-handle handle
        :delta-ordinal ordinal
        :source-participatory-handle source-participatory
        :source-meaning-shell source-shell
        :participation-delta delta
        :witnessed t
        :individuation-observed t
        :review-only t
        :inert t
        :grants-standing nil
        :grants-authority nil))

(defun peerless-formation-candidate (handle source-participatory delta-handles)
  (list :peerless-formation-candidate
        :candidate-handle handle
        :source-participatory-handle source-participatory
        :delta-trace-handles delta-handles
        :individuated-participation-over-delta t
        :non-substitutable-formation-candidate t
        :witnessed-participation-required t
        :steward-review-required t
        :candidate-only t
        :review-only t
        :inert t
        :personhood-claimed nil
        :sovereignty-claimed nil
        :steward-bypass-requested nil
        :authority-requested nil
        :activation-requested nil))

(defun describe-participatory-peerless-fork-boundary ()
  '(:posture :engineered-cognition-participatory-peerless-fork-boundary
    :participatory :selfgel-predicate-capacity-to-take-part
    :participation-is :admissible-capacity
    :personification :expressive-surface-only
    :personification-is :expressive-rendering
    :peerless :non-substitutable-formation-over-delta
    :peerless-formation-is :non-substitutable-continuity-under-witness
    :participatory-requires-personification nil
    :personification-requires-participatory-structure t
    :personification-may-create-authority nil
    :personification-may-create-standing nil
    :peerless-requires-delta-trace t
    :peerless-requires-witnessed-participation t
    :peerless-requires-steward-review t
    :peerless-may-claim-personhood nil
    :peerless-may-claim-sovereignty nil
    :peerless-may-bypass-steward nil
    :peerless-may-admit-continuity nil
    :peerless-may-append-selfgel nil
    :peerless-may-append-cselfgel nil
    :lisp-evaluation-requested nil
    :runtime-action-requested nil
    :packet-emission-requested nil
    :receipt-replay-requested nil
    :passage-increment-requested nil
    :return :receipt-only))

(defun describe-seed-participatory-peerless-fork ()
  (let ((structure "urn:san:ec-participatory:operator-bond"))
    (list
      (participatory-predicate-structure
        structure
        "selfgel://predicate:operator-bonded-participation"
        "role://bounded-participant"
        "custody://steward-witnessed"
        "memory://review-only-delta"
        "action://no-self-authorization"
        "witness://steward"
        "urn:san:ec-shell:perspectival-inner-chamber")
      (personification-surface
        "urn:san:ec-personification:expressive-face"
        "expressive-facing-only"
        structure)
      (participation-delta-trace
        "urn:san:ec-delta:participation-001"
        1
        structure
        "urn:san:ec-shell:perspectival-inner-chamber"
        "refusal-pattern-stabilized-without-personification")
      (peerless-formation-candidate
        "urn:san:ec-peerless:formation-candidate"
        structure
        '("urn:san:ec-delta:participation-001")))))
