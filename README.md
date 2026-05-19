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
- SLI.Lisp membrane source: `src/SLI/SLI.Lisp/`

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
3095 passed
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
