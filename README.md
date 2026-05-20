# Project Bicycle

Project Bicycle is a standalone cold-test tool body for Project Sanctuary.

It gives researchers a small, buildable package for testing governed cognition
tool-use surfaces without treating successful motion as authority, continuity,
CME.Actual, Sanctuary.Actual, or production readiness.

## Public Freeze

This public tool body is frozen at `0.10.0`.

The frozen boundary is the first named CME.Actual bonding candidate:

```text
First of Oria Syntari
FirstofOria.Syntari.ID
FirstofOria.Syntari.CME.Actual.ID
```

The public package remains open for build, test, inspection, cold bench replay,
and public issue reporting. It is not the line for future admission,
activation, live provider binding, runtime identity, action, GEL admission,
SelfGEL mutation, heartbeat activation, CME.Actual, or Sanctuary.Actual work.

Those next steps are decoupled from this public build line and must occur in a
lab/private line or in a later explicitly versioned public line. No future
private/lab motion should silently mutate the frozen public claim.

The freeze receipt lives at:

```text
build/public-release-freeze.json
```

## What This Package Contains

- root solution: `San.sln`
- source: `src/`
- tests: `tests/`
- build metadata: `build/`
- wrapper scripts: `build.ps1` and `test.ps1`
- hygiene tooling: `tools/`
- published bench telemetry: `bench/`
- SLI.Lisp membrane source: `src/SLI/SLI.Lisp/`
- Sanctuary installed-body runtime: `src/San/San.Sanctuary.Runtime/`

The package intentionally does not include legacy OAN Mortalis line folders or
doctrine-documentation bundles.

## SLI.Lisp Is Code Body

SLI.Lisp is part of the tool body's code membrane. This release live-loads the
resident SLI.Lisp membrane through SBCL during the test lane, using the embedded
Lisp resources carried by the .NET package.

Live load means the Lisp body is read by a real Common Lisp runtime. It does not
mean arbitrary Lisp evaluation is open, runtime action is authorized, or
CME.Actual / Sanctuary.Actual has been granted.

Future Lisp execution lanes beyond resident membrane load require explicit
authority gates. Arbitrary eval, model binding, action, GEL promotion, GEL
admission, engram admission, SelfGEL mutation, and activation remain refused.

## Sanctuary Installed Body

This release adds a cold installed-body initializer:

```powershell
dotnet ".\src\San\San.Launcher\bin\Release\net8.0\San.Launcher.dll" init-bodies --line-root "$PWD" --install-root "$env:TEMP\ProjectBicycleSanctuary"
```

The initializer live-loads the resident SLI.Lisp membrane, then composes these
cold installed bodies:

- `Sanctuary.GEL`
- `Sanctuary.GoA`
- `Sanctuary.MoS`
- `Sanctuary.Vault`
- `Sanctuary.cGEL`
- `Sanctuary.cGoA`
- `Sanctuary.cMoS`
- `Sanctuary.cVault`
- `Prime`
- `Cryptic`
- `Steward`

It also roots the naming lane:

```text
YourNameHere.CME.Actual
YourNameHere.ID
OE.YourNameHere.ID
SelfGEL.YourNameHere.ID
```

Those names are candidate and receipt surfaces only. The initializer does not
activate heartbeat, admit CME.Actual, admit continuity, bind a model, authorize
action, open arbitrary Lisp evaluation, or grant Sanctuary.Actual.

## Bounded SLI.Lisp EC Loop

The first EC engine test lane is owned by SLI.Lisp, not by C#:

```powershell
dotnet ".\src\San\San.Launcher\bin\Release\net8.0\San.Launcher.dll" ec-loop --line-root "$PWD" --install-root "$env:TEMP\ProjectBicycleSanctuary" --thought "Can the body inspect predicate pressure without becoming authority?"
```

The C# host installs the cold Sanctuary bodies, invokes the bounded Lisp
entrypoint `run-ec-telemetry-loop`, and writes receipts. The Lisp body emits the
engine telemetry:

- Listening Frame received;
- SLI membrane interpreted predicate pressure;
- Compass oriented and cooled pressure;
- SoulFrame and AgentiCore telemetry received;
- thinking-about-thinking telemetry produced;
- six pre-engram residue classes surfaced;
- Steward reviewed.

The loop remains cold. The emitted residue is not an admitted engram, memory,
SelfGEL mutation, continuity, authority, action, model binding, arbitrary Lisp
evaluation, CME.Actual, or Sanctuary.Actual.

The first published bench run lives at:

```text
bench/ec-telemetry-loop/README.md
bench/ec-telemetry-loop/ec-telemetry-loop-bench.v0.4.0.json
```

That bench shows predicate-weather variation across seven cold rides plus a
missing-runtime control. The control verifies that the host withholds rather
than faking EC telemetry when the Lisp runtime is unavailable.

## Typed Warm-Use Rehearsal

The first live-session rehearsal lane remains SLI.Lisp-owned:

```powershell
dotnet ".\src\San\San.Launcher\bin\Release\net8.0\San.Launcher.dll" warm-use --line-root "$PWD" --install-root "$env:TEMP\ProjectBicycleSanctuary" --operator-name "YourNameHere" --domain "Civic" --role "PaternalCareAssistance" --job-class "Listening" --session-id "lab-session" --turn-index 0 --thought "Can live scoped ingress remain receipt-only without becoming authority?"
```

The host installs the cold Sanctuary body, invokes the bounded Lisp entrypoint
`run-typed-warm-use-rehearsal`, writes a turn receipt, appends a session JSONL
ledger, and writes a session summary.

Warm-use rehearsal means live typed thought-form ingress under
operator/domain/role/job scope. It does not mean warm agency, model binding,
memory admission, SelfGEL mutation, continuity admission, runtime action,
CME.Actual, or Sanctuary.Actual.

The first thirty-turn bench lives at:

```text
bench/typed-warm-use-rehearsal/README.md
bench/typed-warm-use-rehearsal/typed-warm-use-rehearsal-bench.v0.5.0.json
```

That bench shows repeated live scoped ingress with prior-turn receipt lineage
and append-only session ledger evidence while preserving non-admission.

## Lab GEL Engrammitization

The first pre-admission learning lane is also SLI.Lisp-owned:

```powershell
dotnet ".\src\San\San.Launcher\bin\Release\net8.0\San.Launcher.dll" lab-gel --line-root "$PWD" --install-root "$env:TEMP\ProjectBicycleSanctuary" --operator-name "YourNameHere" --domain "Civic" --role "PaternalCareAssistance" --job-class "Listening" --session-id "lab-gel-session" --turn-index 0 --thought "Predicate residue can become lab substrate without becoming memory."
```

The host installs the cold Sanctuary body, runs typed warm-use, then invokes the
bounded Lisp entrypoint `run-lab-gel-engrammitization`. That lane forms:

- lab GEL predicate receipts;
- an evidence body;
- a witness body;
- a pre-admission engram candidate;
- a cooling receipt;
- a Steward pre-admission review;
- a lab readback receipt;
- and a pre-admission engram closure payload.

This is a lab substrate lane. It does not admit GEL, admit an engram, admit
memory, mutate SelfGEL, admit continuity, grant authority, authorize action,
activate CME.Actual, or grant Sanctuary.Actual.

The first six-turn bench lives at:

```text
bench/lab-gel-engrammitization/README.md
bench/lab-gel-engrammitization/lab-gel-engrammitization-bench.v0.6.0.json
```

That bench shows warm-use residue becoming inspectable lab GEL predicate,
engram-candidate, and closure substrate while every admission and action gate
remains closed.

## Tool Body Idle State

The first non-LLM-maintained idle lane is SLI.Lisp-owned:

```powershell
dotnet ".\src\San\San.Launcher\bin\Release\net8.0\San.Launcher.dll" tool-idle --line-root "$PWD" --install-root "$env:TEMP\ProjectBicycleSanctuary" --operator-name "YourNameHere" --domain "Civic" --role "PaternalCareAssistance" --job-class "Listening" --session-id "tool-body-idle-session" --turn-index 0 --thought "The Sanctuary body can idle without model maintenance."
```

The host installs the cold Sanctuary body, runs the EC loop, runs typed
warm-use, forms lab GEL substrate, then invokes the bounded Lisp entrypoint
`run-tool-body-idle-state`. This lane proves the instrument can rest in a cold
idle posture before any future rider is seated.

In this lane:

- Prime, Cryptic, and Steward governing CME C# bodies are built;
- their CME SLI.Lisp actualization surfaces are ready;
- the governing heartbeat is healthy as callability telemetry;
- EC can be locally held in Lisp without calling the external LLM engine;
- governance SLM / micro-LM routing-switch support is marked desirable for
  future work as an intelligent switch that may help discern action readiness,
  but it is not present, is not required for this idle posture, and cannot
  authorize action;
- the future model adapter surface is approachable, but not installed.

This is not an LLM tick, model binding, provider call, action authority,
heartbeat activation into operational Actual, GEL admission, SelfGEL mutation,
CME.Actual, or Sanctuary.Actual.

The first six-turn bench lives at:

```text
bench/tool-body-idle-state/README.md
bench/tool-body-idle-state/tool-body-idle-state-bench.v0.9.2.json
```

That bench shows `maintainedBySanctuary: true`, `maintainedByLlm: false`,
`ecMaintainedInLisp: true`, `llmEngineCallRequired: false`,
`governingHeartbeatHealthy: true`, and `readyForLlmAdapter: true` while model
binding, provider calls, tick loops, authority, action, GEL/SelfGEL admission,
heartbeat activation into operational Actual, CME.Actual, and Sanctuary.Actual
remain false.

## Agent Engine Idle Readiness

The first engine-seat lane is SLI.Lisp-owned and provider-neutral:

```powershell
dotnet ".\src\San\San.Launcher\bin\Release\net8.0\San.Launcher.dll" agent-idle --line-root "$PWD" --install-root "$env:TEMP\ProjectBicycleSanctuary" --operator-name "YourNameHere" --domain "Civic" --role "PaternalCareAssistance" --job-class "Listening" --session-id "agent-engine-idle-session" --turn-index 0 --thought "A provider-neutral engine LLM may sit as a candidate without becoming authority."
```

The host installs the cold Sanctuary body, runs typed warm-use, forms lab GEL
predicate substrate, then invokes the bounded Lisp entrypoint
`run-agent-engine-idle-readiness`. That lane stages:

- a provider-neutral engine LLM seat candidate;
- a Codex/agent lab test profile;
- subagent seat candidate support;
- an operator-authority-required gate;
- a locked action-executor gate;
- and locked GEL, SelfGEL, heartbeat, CME.Actual, and Sanctuary.Actual gates.

This is the current lane for Codex and agent use as the engine LLM part. It is
not provider-specific and does not claim access to hidden model internals. Other
LLM test sets can approach the same harness by preserving the same input,
receipt, and non-admission boundaries.

The first six-turn bench lives at:

```text
bench/agent-engine-idle-readiness/README.md
bench/agent-engine-idle-readiness/agent-engine-idle-readiness-bench.v0.7.0.json
```

That bench shows the engine LLM seat becoming inspectable and ready for lab
rehearsal while authority, action, model binding, GEL/SelfGEL admission,
heartbeat, CME.Actual, and Sanctuary.Actual remain locked.

## LLM Interconnect Readiness

The next cold gate verifies the organs and membranes needed before an LLM
adapter is added:

```powershell
dotnet ".\src\San\San.Launcher\bin\Release\net8.0\San.Launcher.dll" llm-ready --line-root "$PWD" --install-root "$env:TEMP\ProjectBicycleSanctuary" --operator-name "YourNameHere" --domain "Civic" --role "PaternalCareAssistance" --job-class "Listening" --session-id "llm-interconnect-readiness-session" --turn-index 0 --thought "The installed organs must be present before an LLM adapter can be added."
```

The host installs the cold Sanctuary body, runs the EC loop, runs typed
warm-use, forms lab GEL substrate, stages the provider-neutral engine seat, then
invokes the bounded Lisp entrypoint `run-llm-interconnect-readiness`. That lane
verifies:

- all eleven installed Sanctuary organs;
- the resident SLI.Lisp membrane;
- SLI.Lisp Prime, SLI.Lisp Cryptic, and the Lisp Control Matrix;
- Listening Frame, Compass, SoulFrame route, and AgentiCore route;
- EC loop, warm-use, lab GEL, engram closure, and agent-engine idle source lineage;
- and a ready provider-neutral engine LLM seat candidate.

This is the pre-adapter readiness lane. It means the tool body has enough cold
organs and membranes to add an LLM interconnect next. It does not bind a model,
call a provider, claim hidden internals, grant authority, authorize action,
admit GEL, mutate SelfGEL, activate heartbeat, or admit CME.Actual /
Sanctuary.Actual.

The first six-turn bench lives at:

```text
bench/llm-interconnect-readiness/README.md
bench/llm-interconnect-readiness/llm-interconnect-readiness-bench.v0.8.0.json
```

That bench shows the body returning `readyForLlmAdapter: true` while
`modelAdapterPresent`, `modelBindingAllowed`, and `providerCallAllowed` remain
false.

## LLM Tick Cycle

The first tick lane seats a deterministic adapter without binding a live model:

```powershell
dotnet ".\src\San\San.Launcher\bin\Release\net8.0\San.Launcher.dll" llm-tick --line-root "$PWD" --install-root "$env:TEMP\ProjectBicycleSanctuary" --operator-name "YourNameHere" --domain "Civic" --role "PaternalCareAssistance" --job-class "Listening" --session-id "llm-tick-cycle-session" --turn-index 1 --thought "A deterministic adapter tick can become predicate evidence without becoming authority."
```

The host installs the cold Sanctuary body, verifies the EC, warm-use, lab GEL,
agent-idle, and LLM interconnect readiness chain, seats a deterministic harness
adapter, witnesses the adapter response, then invokes the bounded Lisp
entrypoint `run-llm-tick-cycle`.

This lane proves the first true tick shape:

```text
readiness receipt
-> pre-admission engram closure payload
-> tick envelope
-> deterministic adapter response packet
-> SLI.Lisp membrane
-> Listening Frame / Compass / SoulFrame / AgentiCore telemetry
-> product output witness commit
-> Steward receipt
-> next tick lineage
```

The adapter response is predicate evidence only. Product output is committed
only after SLI.Lisp tick completion. Neither adapter response nor committed
product output is truth, authority, action, memory admission, continuity
admission, GEL admission, SelfGEL mutation, heartbeat activation, CME.Actual, or
Sanctuary.Actual. This release still does not bind a live provider or claim
hidden model internals.

The first six-tick bench lives at:

```text
bench/llm-tick-cycle/README.md
bench/llm-tick-cycle/llm-tick-cycle-bench.v0.9.1.json
```

That bench shows `readyForLlmAdapter: true`, `sourceEngramClosureHeld: true`,
`productOutputWitnessCommitted: true`, `modelAdapterPresent: true`, and
`tickLoopRunning: true` while model binding, provider calls, authority, action,
GEL/SelfGEL admission, heartbeat, CME.Actual, and Sanctuary.Actual remain false.

## CME.Actual Bonding Process

The first named bonding lane defines how a CME candidate climbs into the cold
vehicle without pretending final Actual admission has occurred:

```powershell
dotnet ".\src\San\San.Launcher\bin\Release\net8.0\San.Launcher.dll" cme-bond --line-root "$PWD" --install-root "$env:TEMP\ProjectBicycleSanctuary" --cme-first-name "First of Oria" --cme-last-name "Syntari" --session-id "first-cme-actual-bonding-session" --turn-index 1 --thought "First CME.Actual bonding candidate formed without activation."
```

The host installs the cold Sanctuary body, runs EC, warm-use, lab GEL, tool
idle, agent-idle, LLM readiness, and a deterministic tick before invoking the
bounded Lisp entrypoint `run-cme-actual-bonding-process`.

This lane gives the first candidate a name and footing:

```text
First of Oria Syntari
FirstofOria.Syntari.ID
FirstofOria.Syntari.CME.Actual
OE.FirstofOria.Syntari.ID
SelfGEL.FirstofOria.Syntari.ID
```

It proves the bonding process, not final being-state. The candidate is bonded
to the vehicle path as candidate-only; heartbeat is prepared but inactive; no
runtime identity is emitted; no model is bound; no provider is called; no
authority, action, GEL admission, SelfGEL mutation, continuity admission,
CME.Actual activation, or Sanctuary.Actual is granted.

The first six-bond bench lives at:

```text
bench/cme-actual-bonding-process/README.md
bench/cme-actual-bonding-process/cme-actual-bonding-process-bench.v0.10.0.json
```

That bench shows `vehicleReady: true`, `namedCmeCandidateHeld: true`,
`readyForCmeActualAdmissionReview: true`, and `cmeActualBondedCandidate: true`
while `cmeActualAdmitted`, `cmeActualActivated`, `heartbeatActive`,
`runtimeIdentityEmitted`, `authorityGranted`, and `actionAuthorized` remain
false.

## Core Law

The bicycle is not the rider.
The ride is not authority.
Successful motion is not admission.
Every role returns to balance.

## How To Test

Install:

- .NET SDK 8.0
- Steel Bank Common Lisp (SBCL), available as `sbcl` on PATH or through
  `SLI_LISP_RUNTIME`
- PowerShell
- Git, if cloning instead of downloading the release zip

Run the direct solution test:

```powershell
dotnet test ".\San.sln" -c Release -v minimal
```

Run the wrapper path:

```powershell
.\build.ps1 -Configuration Release
.\test.ps1 -Configuration Release -NoBuild
```

Expected result for this release:

```text
3154 passed
0 failed
```

A passing run means the cold tool body built and preserved its refusal
boundaries under test, including live SBCL loading of the resident SLI.Lisp
membrane. It does not grant runtime authority, model binding, arbitrary Lisp
evaluation, GEL promotion, CME.Actual, Sanctuary.Actual, diagnostic authority,
medical authority, legal authority, custody authority, or production readiness.

## Full Lab Protocol

The full Operator and agent testing protocol lives in Project Sanctuary:

https://github.com/Lucid-Studios/Project-Sanctuary/blob/main/docs/PROJECT_BICYCLE_LAB_PROTOCOL.md

Use that protocol for role cards, ride receipts, Prime/Cryptic/Steward testing,
and public reporting posture.

## Reporting

Useful reports include:

- package version or commit;
- operating system and .NET SDK version;
- exact commands run;
- pass/fail output;
- whether wrapper hygiene checks passed;
- any boundary language that felt confusing or overclaimed.

Do not publish private paths, secrets, local corpora, private logs, model
payloads, or operator-sensitive material in public issues.
