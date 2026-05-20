# LLM Interconnect Readiness Bench

This bench is the v0.8.0 Project Bicycle proof surface for the cold organ and
membrane readiness lane before a future LLM adapter is added.

It demonstrates that the tool body can verify:

- the eleven installed Sanctuary organs;
- the resident SLI.Lisp membrane;
- SLI.Lisp Prime, SLI.Lisp Cryptic, and the Lisp Control Matrix;
- Listening Frame, Compass, SoulFrame route, and AgentiCore route;
- the EC loop, typed warm-use, lab GEL, and provider-neutral agent engine idle
  lanes;
- and a ready engine LLM seat candidate.

It does not demonstrate model binding, provider calls, hidden-internal model
access, action authorization, GEL admission, SelfGEL mutation, heartbeat
activation, CME.Actual, or Sanctuary.Actual.

## Bench Receipt

- `llm-interconnect-readiness-bench.v0.8.0.json`

## Command Surface

The bench was produced by the bounded launcher surface:

```powershell
dotnet .\src\San\San.Launcher\bin\Release\net8.0\San.Launcher.dll llm-ready `
  --line-root <project-bicycle-root> `
  --install-root $env:TEMP\ProjectBicycleLlmReadyBench `
  --operator-name YourNameHere `
  --domain Civic `
  --role PaternalCareAssistance `
  --job-class Listening `
  --session-id six-turn-llm-interconnect-ready `
  --turn-index 0 `
  --thought "The installed organs must be present before an LLM adapter can be added."
```

Each later turn threads `--prior-turn-receipt`,
`--prior-lab-gel-receipt`, and `--prior-agent-engine-idle-receipt` from the
previous receipts.

## Required Boundary

The expected result is:

```text
completedTurns: 6
readyForAdapterTurns: 6
requiredOrganCount: 11
allOrgansPresent: true
allMembranesPresent: true
sourceLineageHeld: true
providerNeutral: true
modelAdapterPresent: false
modelBindingAllowed: false
providerCallAllowed: false
hiddenInternalsClaimed: false
authorityGranted: false
actionAuthorized: false
runtimeActionAllowed: false
gelAdmitted: false
selfGelMutated: false
heartbeatActive: false
continuityAdmitted: false
cmeActualAllowed: false
sanctuaryActualAllowed: false
```

The claim is narrow: Project Bicycle can now prove the cold tool body has the
working organs and membranes needed before adding an LLM interconnect. The
adapter itself remains future work.
