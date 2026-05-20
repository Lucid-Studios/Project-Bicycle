# Project Bicycle

Project Bicycle is a standalone cold-test tool body for Project Sanctuary.

It gives researchers a small, buildable package for testing governed cognition
tool-use surfaces without treating successful motion as authority, continuity,
CME.Actual, Sanctuary.Actual, or production readiness.

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
authority gates. Arbitrary eval, model binding, action, GEL promotion, and
activation remain refused.

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
3112 passed
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
