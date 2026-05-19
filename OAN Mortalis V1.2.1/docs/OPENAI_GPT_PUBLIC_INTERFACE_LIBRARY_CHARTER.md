# OpenAI GPT Public Interface Library Charter

Status: review-only source-library charter.  
Claim ceiling: public interface instrumentation planning.  
Activation status: non-scraping, non-provider-calling, non-model-binding, non-authorizing.
Retrieved-source posture: official public records refreshed on 2026-05-16.

This charter defines the public-record library posture for studying OpenAI GPT-line models and related OpenAI agentic interfaces as observable, documented, published surfaces. It exists so the lab can build a lawful interconnect library against public interfaces without claiming access to hidden proprietary internals.

This is a living library charter, not an exhaustive capture of every OpenAI publication. "Any and all published records" means the working library must be able to ingest all relevant public records over time under type, provenance, and non-collapse law. It does not mean the current charter already contains every record.

## Governing Compression

> Public interface study is not proprietary internals reconstruction.
>
> Published behavior is not full causal access.
>
> Observable output is not hidden mechanism.
>
> Official documentation may guide interconnect design.
>
> It may not become seed authority by itself.

## Library Purpose

The library should collect, classify, and cite public OpenAI records relevant to:

- model role selection
- model capability surfaces
- API behavior
- structured output behavior
- tool calling and tool semantics
- agent orchestration
- tracing and telemetry
- guardrails and handoffs
- data controls and retention posture
- safety policy and usage constraints
- model behavior specification
- model/system card evidence
- Codex and coding-agent surfaces
- observed interface behavior under lab tests

The goal is not to reproduce hidden internal machinery. The goal is to build a lawful, source-backed interconnect layer that can evaluate how model bodies behave when placed under typed governance conditions.

The first machine-readable seed register is:

- `OPENAI_GPT_PUBLIC_INTERFACE_SOURCE_REGISTER.seed.json`

## Allowed Source Classes

| Source Class | Examples | Use |
|---|---|---|
| Official API docs | Models, Responses API, tools, structured outputs, data controls. | Defines supported public interface, parameters, capabilities, and constraints. |
| Official behavior docs | Model Spec, usage policies, safety guidance. | Defines published behavior expectations and policy boundaries. |
| Official system/model cards | GPT-line system cards, preparedness disclosures, eval notes. | Defines published evaluation, safeguards, and known limitation posture. |
| Official SDK docs | Agents SDK, guardrails, handoffs, tracing. | Defines orchestration semantics and telemetry patterns. |
| Official privacy/data records | API data controls, enterprise privacy, data usage FAQ. | Defines data exposure, training, retention, and privacy constraints. |
| Lab observation records | Local cold traces, fixture runs, model comparison receipts. | Defines observed behavior in our own governed test surfaces. |

## Refused Source Classes

The library must not include:

- leaked weights
- secret prompts
- proprietary internal documents
- reverse-engineered hidden chain-of-thought
- credential material
- API keys
- private OpenAI staff communications
- unverifiable rumors as source authority
- scraped private user data
- claims of hidden architecture access
- claims that public behavior proves full internal mechanism

## Core Public Source Register

This initial register is not exhaustive. It is the first official-source body to build against.

| Register ID | Source | Public Surface | Library Use |
|---|---|---|---|
| `openai.models` | [Models](https://developers.openai.com/api/docs/models) | Model IDs, capability classes, modality/tool support, context/output limits, knowledge cutoff and pricing fields where published. | Model role catalog and capability-class mapping. |
| `openai.latest_gpt55` | [Introducing GPT-5.5](https://openai.com/index/introducing-gpt-5-5/) | Current GPT-line release description, availability posture, coding/agentic-work claims, published eval framing. | Model-role currency check and release-delta source. |
| `openai.responses` | [Responses API reference](https://developers.openai.com/api/reference/resources/responses/methods/create) | Unified response object, input/output items, model field, tool calls, metadata, max tool calls, reasoning/output controls. | Main future provider interconnect contract. |
| `openai.responses.migration` | [Migrate to Responses API](https://developers.openai.com/api/docs/guides/migrate-to-responses) | Agent-like application posture, built-in tools, multi-turn state, stateful context, multimodal support. | API architecture comparison and statefulness boundary. |
| `openai.tools` | [Using tools](https://developers.openai.com/api/docs/guides/tools) | Tool availability, `tools` configuration, automatic tool selection, `tool_choice`, MCP and hosted tool surfaces. | Tool-role mapping and tool-call telemetry. |
| `openai.function_calling` | [Function calling](https://developers.openai.com/api/docs/guides/function-calling) | Tool/function calling as the public way models interface with external systems and application-provided data/actions. | Action-surface and tool-argument contract mapping. |
| `openai.structured_outputs` | [Structured Outputs](https://developers.openai.com/api/docs/guides/structured-outputs) | Function-calling structured outputs and schema-constrained response output. | Typed receipt and model-output validation design. |
| `openai.model_spec` | [Model Spec](https://model-spec.openai.com/2025-10-27.html) | Published desired model behavior, authority chain, instruction roles, side-effect care, uncertainty and non-sycophancy posture. | Behavioral correspondence and non-collapse comparison. |
| `openai.usage_policies` | [Usage Policies](https://openai.com/policies/usage-policies/) | Universal usage constraints, safety restrictions, high-stakes and manipulation constraints. | Policy boundary mapping and Special Case refusal checks. |
| `openai.preparedness_framework` | [Preparedness Framework update](https://openai.com/index/updating-our-preparedness-framework/) | Frontier-risk categories, severe-harm safeguards, publication of findings with frontier model releases. | High-capability risk classification reference. |
| `openai.gpt5_system_card` | [GPT-5 System Card](https://openai.com/index/gpt-5-system-card/) | Unified GPT-5 system description, routing posture, safety/evaluation publication. | Historical GPT-line behavior/system-card evidence. |
| `openai.gpt55_system_card` | [GPT-5.5 System Card](https://openai.com/index/gpt-5-5-system-card/) | GPT-5.5 complex-work posture, tool use, safety evaluation, safeguards. | Current frontier-system evidence when available. |
| `openai.gpt55_instant_system_card` | [GPT-5.5 Instant System Card](https://openai.com/index/gpt-5-5-instant-system-card/) | Instant model safety posture and preparedness-category treatment. | Fast/instant model comparison and safety-delta source. |
| `openai.gpt5_codex_addendum` | [Addendum to GPT-5 system card: GPT-5-Codex](https://openai.com/index/gpt-5-system-card-addendum-gpt-5-codex) | Coding-agent model safety and product mitigations such as sandboxing and network controls. | Codex/governing-body comparison and coding-agent risk boundary. |
| `openai.agents_sdk` | [Agents SDK guide](https://developers.openai.com/api/docs/guides/agents-sdk/) | SDK overview for agentic apps using tools, handoffs, streaming, traces. | Agent orchestration correspondence. |
| `openai.agents_sdk.agents` | [Agents SDK - Agents](https://openai.github.io/openai-agents-python/agents/) | Agent definition, tools, handoffs, guardrails, structured outputs, Responses API default use. | Agent/body role modeling. |
| `openai.agents_sdk.tracing` | [Agents SDK - Tracing](https://openai.github.io/openai-agents-python/tracing/) | Trace spans for LLM generations, tool calls, handoffs, guardrails, custom events. | Telemetry and receipt design correspondence. |
| `openai.agents_sdk.guardrails` | [Agents SDK - Guardrails](https://openai.github.io/openai-agents-python/guardrails/) | Input, output, and tool guardrails; tripwires; workflow-boundary limitations. | Steward and tool-boundary comparison. |
| `openai.agents_sdk.handoffs` | [Agents SDK - Handoffs](https://openai.github.io/openai-agents-python/handoffs/) | Handoff semantics, input filters, delegation to specialized agents. | Prime/Cryptic/Steward and worker-body routing comparison. |
| `openai.data_controls` | [Data controls in the OpenAI platform](https://platform.openai.com/docs/guides/your-data) | API data training default, abuse monitoring logs, retention controls, application state. | Data exposure and model-context boundary. |
| `openai.enterprise_privacy` | [Enterprise privacy at OpenAI](https://openai.com/policies/api-data-usage-policies/) | Business data ownership/control, default non-training posture, security commitments. | Lab provenance and provider-exposure planning. |
| `openai.codex_cloud` | [Codex web](https://developers.openai.com/codex/cloud) | Codex coding-agent product surface for reading, editing, running code, background work, and parallel task handling. | Codex-as-build-agent comparison; not runtime authority. |
| `openai.codex_cli_help` | [OpenAI Codex CLI - Getting Started](https://help.openai.com/en/articles/11096431-openai-codex-ligetting-started) | Local CLI posture, approval modes, local execution claims, model-use FAQ. | Lab build-tool posture and operator approval model comparison. |

## Interface-Level Observation Law

The lab may observe:

- input/output behavior
- refusal posture
- structured output adherence
- tool-call selection
- tool-call arguments
- trace metadata where available
- latency and failure modes
- response consistency under fixtures
- behavior under role separation
- behavior under policy-sensitive prompts
- behavior under warrant-boundary tests

The lab may not claim:

- full internal causal certainty
- hidden chain-of-thought access
- weight-space knowledge
- undocumented router logic
- proprietary training set access
- private safety-policy access
- complete model ontology

## Public Record Intake Schema

Each source entry should eventually carry:

```text
source_record_id:
source_url:
source_title:
publisher:
record_type:
published_date:
retrieved_date:
version_or_updated_date:
source_class:
public_surface:
library_use:
allowed_derivations:
forbidden_derivations:
uncertainty_notes:
linked_lab_tests:
source_status:
supersedes:
superseded_by:
last_verified_by:
```

`allowed_derivations` defaults to `interface-correspondence-only`.

`forbidden_derivations` must include `hidden-internals-claim`.

`source_status` may be:

- `active`
- `historical`
- `superseded`
- `candidate`
- `withdrawn`
- `unverified`

No source may be promoted from `candidate` or `unverified` into implementation guidance without a fresh retrieval receipt.

## Interface Interconnect Law

The interconnect library may translate public OpenAI surfaces into local contracts only through bounded correspondence:

```text
official record
-> classified public surface
-> allowed derivation
-> local correspondence map
-> fixture or contract candidate
-> Steward review
-> cold implementation
```

The interconnect library may not translate:

```text
official record
-> hidden substrate claim
```

or:

```text
observed behavior
-> internal mechanism certainty
```

or:

```text
model success
-> semantic warrant
```

The library therefore supports interoperability, telemetry, model-role selection, and fixture design. It does not certify model ontology, runtime identity, `GEL` promotion, `CME.Actual`, or `Sanctuary.Actual`.

## OpenAI Line Correspondence Classes

The library should classify published surfaces into correspondence classes rather than treating them as direct authority.

| Correspondence Class | Meaning | Example |
|---|---|---|
| Model Capability Surface | Public capability and endpoint behavior. | Models page, model-specific docs. |
| Interaction Surface | How requests, responses, tool calls, and state are represented. | Responses API, tool calling, structured outputs. |
| Orchestration Surface | How agent workflows, handoffs, guardrails, traces, and sessions are represented. | Agents SDK docs. |
| Behavior Specification Surface | Intended assistant behavior and instruction authority. | Model Spec. |
| Safety / Policy Surface | Boundaries on permitted use and safeguards. | Usage Policies, Preparedness Framework, system cards. |
| Data Governance Surface | How data may be stored, retained, trained on, or controlled. | Data controls, enterprise privacy. |
| Lab Observation Surface | Our own reproducible observations through lawful fixtures. | Cold instrument traces, comparison receipts. |

## Non-Collapse Rules

- official documentation is not hidden internals
- public system card is not full model ontology
- observed output is not causal proof
- tool-call success is not semantic warrant
- structured output adherence is not truth
- model policy compliance is not CME governance
- API statefulness is not `GEL` continuity
- model routing is not Steward routing
- guardrail tripwire is not principled refusal by itself
- trace span is not witness authority by itself
- provider provenance is not proof of correctness
- OpenAI-proximal seed posture is not provider worship
- external model comparison is not seed authority

## Interconnect Library Deliverables

The working library should eventually produce:

- public source register
- source-record schema
- model capability map
- tool-surface map
- structured-output schema map
- agent orchestration correspondence map
- trace/telemetry correspondence map
- OpenAI policy/safety boundary map
- data exposure boundary map
- public behavior-to-CME doctrine comparison matrix
- fixture suite for model-interface behavior
- model comparison receipts

## Relationship To Existing Build Passes

This library belongs primarily to:

- Pass 6: Model Role Telemetry Run
- Pass 7: Proxy-Assisted Instrument Readiness Run
- Pass 8: LLM Universality Telemetry Run
- Pass 9: Local LLM Seed Readiness Run

It should not block Passes 1-5 unless the instrument harness begins using model-role fixtures.

## Update Discipline

OpenAI product docs, policies, and model pages are temporally unstable. Before using a source for implementation or publication, refresh:

- source URL
- retrieved date
- current model IDs
- current endpoint behavior
- current data controls
- current policy text
- current system/model card availability

No stale source should be used to justify a live model-binding decision.

## Closeout

We may study the published interface.

We may test observable behavior.

We may build lawful interconnects around public contracts.

We may not pretend that public surfaces grant hidden internal access.
