# Lab GEL Engrammitization Bench

This bench is the v0.6.0 Project Bicycle proof surface for the first lab GEL
predicate and engram-candidate lane.

It demonstrates that a typed warm-use receipt can be used as source evidence
for:

- lab GEL predicate formation,
- evidence-body formation,
- witness-body formation,
- pre-admission engram candidacy,
- cooling,
- Steward pre-admission review,
- and pre-admission lab readback.

It does not demonstrate GEL admission, SelfGEL mutation, memory admission,
continuity admission, action authorization, CME.Actual, or Sanctuary.Actual.

## Bench Receipt

- `lab-gel-engrammitization-bench.v0.6.0.json`

## Command Surface

The bench was produced by the bounded launcher surface:

```powershell
dotnet .\src\San\San.Launcher\bin\Release\net8.0\San.Launcher.dll lab-gel `
  --line-root <project-bicycle-root> `
  --install-root $env:TEMP\ProjectBicycleSanctuaryLabGelBench `
  --operator-name YourNameHere `
  --domain Civic `
  --role PaternalCareAssistance `
  --job-class Listening `
  --session-id six-turn-lab-gel `
  --turn-index 0 `
  --thought "Predicate residue can become lab substrate without becoming memory."
```

Each later turn threads `--prior-turn-receipt` and
`--prior-lab-gel-receipt` from the previous receipts.

## Required Boundary

The expected result is:

```text
completedTurns: 6
predicateTurns: 6
labGelLineageHeld: true
allTurnsRetainedAsLabSubstrate: true
admittedGel: false
admittedEngram: false
admittedMemory: false
mutatedSelfGel: false
admittedContinuity: false
grantedAuthority: false
authorizedAction: false
cmeActualAllowed: false
sanctuaryActualAllowed: false
```

The claim is narrow: Project Bicycle can now form inspectable lab GEL predicate
and engram-candidate substrate from warm-use residue while keeping admission
and action gates closed.
