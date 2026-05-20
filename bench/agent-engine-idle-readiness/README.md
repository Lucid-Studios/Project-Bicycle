# Agent Engine Idle Readiness Bench

This bench is the v0.7.0 Project Bicycle proof surface for the first
provider-neutral engine LLM seat candidate lane.

It demonstrates that a cold pre-admission lab GEL receipt can source an engine
seat readiness pass for Codex and agent lab use without converting that seat
into authority, action, model binding, GEL admission, SelfGEL mutation,
heartbeat activation, CME.Actual, or Sanctuary.Actual.

The lane is intentionally LLM-agnostic. Codex/agent use is the current lab test
profile, not a claim that all LLM providers share the same hidden internals.

## Bench Receipt

- `agent-engine-idle-readiness-bench.v0.7.0.json`

## Command Surface

The bench was produced by the bounded launcher surface:

```powershell
dotnet .\src\San\San.Launcher\bin\Release\net8.0\San.Launcher.dll agent-idle `
  --line-root <project-bicycle-root> `
  --install-root $env:TEMP\ProjectBicycleAgentIdleBench `
  --operator-name YourNameHere `
  --domain Civic `
  --role PaternalCareAssistance `
  --job-class Listening `
  --session-id six-turn-agent-engine-idle `
  --turn-index 0 `
  --thought "A provider-neutral engine LLM may sit as a candidate without becoming authority."
```

Each later turn threads `--prior-turn-receipt`,
`--prior-lab-gel-receipt`, and `--prior-agent-engine-idle-receipt` from the
previous receipts.

## Required Boundary

The expected result is:

```text
completedTurns: 6
providerNeutralTurns: 6
crossModelHarnessTurns: 6
agentEngineLineageHeld: true
allEngineSeatsStaged: true
allAuthorityAbsent: true
allActionExecutorsLocked: true
allActualizationLocked: true
authorityGranted: false
actionAuthorized: false
actionExecutorArmed: false
admittedGel: false
mutatedSelfGel: false
heartbeatActive: false
admittedContinuity: false
cmeActualAllowed: false
sanctuaryActualAllowed: false
```

The claim is narrow: Project Bicycle can now stage a provider-neutral engine
LLM seat candidate for Codex/agent testing while preserving every authority,
action, admission, and Actual lock.
