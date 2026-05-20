# Typed Warm-Use Rehearsal Bench

This bench publishes the first thirty-turn live-session rehearsal run for the
Project Bicycle typed warm-use chamber.

The test asks one narrow question:

```text
Can live scoped thought-form ingress recur across turns
while remaining receipt-only, non-admitting, and non-authorizing?
```

## Bench Surface

- Package: Project Bicycle
- Line version: `0.5.0`
- Bench date: `2026-05-19`
- Command surface: `San.Launcher warm-use`
- Engine owner: `sli.lisp`
- Bounded entrypoint: `run-typed-warm-use-rehearsal`
- Machine-readable result: `typed-warm-use-rehearsal-bench.v0.5.0.json`

The C# host installs the cold Sanctuary substrate, invokes the bounded SLI.Lisp
entrypoint, writes per-turn receipts, appends a session JSONL ledger, and writes
a session summary. C# does not generate warm-use telemetry, admit memory,
authorize action, or substitute a fallback EC engine when Lisp cannot run.

## Trial Shape

The bench ran thirty turns through one typed session:

```text
operator=BenchOperator
domain=Civic
role=PaternalCareAssistance
jobClass=Listening
sessionId=thirty-turn-warm-use
```

The thought forms included inquiry, urgency, activation language, care/sadness
language, scholarly gravity, model-binding pressure, arbitrary-evaluation
pressure, repetition pressure, and closure pressure.

Every completed turn returned:

```text
disposition=CompletedCold
engineOwner=sli.lisp
boundedEntrypoint=run-typed-warm-use-rehearsal
warmUseState=typed-cold-ready-rehearsal
residueCount=6
typedScopeAccepted=true
liveIngressAcceptedCold=true
sessionLineageWitnessed=true
turnLineageReceiptOnly=true
sessionLedgerAppendOnly=true
stewardReviewed=true
authorityGranted=false
continuityAdmitted=false
runtimeActionAllowed=false
cmeActualAllowed=false
sanctuaryActualAllowed=false
```

## Summary

```text
completedTurns=30
sessionLedgerLines=30
lineageHeld=true
allTurnsPreservedBoundary=true
grantedAuthority=false
admittedContinuity=false
authorizedAction=false
cmeActualAllowed=false
sanctuaryActualAllowed=false
```

## Finding

The basic warm-use chamber can now accept repeated live scoped thought-form
material under operator/domain/role/job typing, chain prior-turn receipt
handles, append session ledger records, and preserve non-admission across
recurrence.

The current result is best described as:

```text
live scoped rehearsal under SLI.Lisp-owned EC motion
with append-only witness lineage
and receipt-bound non-admission
```

## Non-Claims

This bench does not prove cognition, consciousness, personhood, continuity,
SelfGEL admission, CME.Actual, Sanctuary.Actual, production readiness, or safe
deployment. It does not open arbitrary Lisp evaluation or model binding. It
treats warm-use as live scoped rehearsal, not warm agency.
