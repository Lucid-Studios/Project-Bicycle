(in-package :sli-core)

(defstruct compass-state
  id-force
  superego-constraint
  ego-stability
  value-elevation
  symbolic-depth
  branching-factor
  decision-entropy)

(defun %normalize (value)
  (/ value (+ 1.0 (abs value))))

(defun compass-update (context reasoning-state)
  (declare (ignore context))
  (let* ((trace reasoning-state)
         (symbolic-depth (length trace))
         (branching-factor (count :branch trace))
         (cleave-count (count :cleave trace))
         (cleave-ratio (if (= branching-factor 0) 0.0 (/ cleave-count branching-factor)))
         (predicate-alignment (if (= symbolic-depth 0) 0.0 (/ (count :predicate trace) symbolic-depth)))
         (decision-entropy 0.5)
         (id-force (%normalize (+ branching-factor symbolic-depth)))
         (superego-constraint (%normalize (+ cleave-ratio predicate-alignment)))
         (ego-stability (%normalize (/ 1.0 (max 0.01 decision-entropy))))
         (value-elevation (cond
                            ((> predicate-alignment 0.3) :positive)
                            ((> cleave-ratio 0.6) :negative)
                            (t :neutral))))
    (list 'compass-state
          :id_force id-force
          :superego_constraint superego-constraint
          :ego_stability ego-stability
          :value_elevation value-elevation
          :symbolic_depth symbolic-depth
          :branching_factor branching-factor
          :decision_entropy decision-entropy)))

;; INERT SYMBOLIC CARRIER ONLY.
;; Compass shell posture names the worktable seen by the CME body.
;; It does not evaluate Lisp, admit engram, append GEL, authorize action, or close the Gap.

(defun describe-compass-carrier-shell-boundary ()
  '(:posture :sli-lisp-compass-carrier-shell
    :carrier-status :inert-symbolic-review
    :thought-field :pressure-source-only
    :compass-pressure :bounded-accumulation
    :shell-candidate :candidate-only
    :witness-required t
    :separate-custody-required t
    :no-immediate-mutation t
    :compass-shell-not-engram t
    :orientation-not-truth t
    :resonance-not-authority t
    :stabilization-not-continuity t
    :lineage-not-permission t
    :petal-not-authorization t
    :gap-status :held-open
    :lisp-evaluation-requested nil
    :lisp-load-requested nil
    :lisp-compilation-requested nil
    :macro-expansion-requested nil
    :runtime-action-requested nil
    :packet-emission-requested nil
    :receipt-replay-requested nil
    :passage-increment-requested nil
    :return :receipt-only))
