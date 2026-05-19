# LAB_DATA_INVENTORY_SCHEMA_BOUNDARY

## Purpose

This note defines the passive Lab data inventory schema boundary in `V1.2.1`.

It exists so documented Lab data can be classified into governed inventory
posture before any later inventory evaluation, ingestion boundary, consent
capture, proof harness, startup path, runtime behavior, or `RTME` movement is
approached.

The boundary defines what may be said about data before anything touches it.

## Governing Compression

> Lab data inventory may classify documented company, personal/operator, nonprofit/society, IP/asset, witness, telemetry, and Special Case data into governed inventory posture; it does not ingest data, collect consent, expose raw content, authorize research use, train models, activate runtime, or admit RTME movement.

## Inventory Ladder

The passive ladder is:

`documented data -> metadata-only proof posture -> Lab data inventory schema -> Lab data inventory evaluation posture -> later ingestion boundary`

This ladder does not create an inventory evaluator.

It does not create ingestible data.

It does not create consent.

It does not expose raw content.

It does not activate runtime.

## First Inventory Classes

The first inventory schema may classify these metadata-only inventory classes:

- company data
- personal/operator data
- nonprofit/society data
- IP/asset data
- conversation witness data
- operational telemetry data
- Special Case/sensitive data

Each item may carry an inventory item id, data class, logical source label,
owner or steward posture, authority-to-inventory posture, sensitivity class,
consent requirement, allowed use scope, forbidden use scope, retention posture,
deletion or revocation posture, visibility posture, research-separation
posture, Special Case posture, IP/asset posture, receipt refs,
non-authority summary, and witness refs.

Each item remains metadata-only.

No item may contain raw Lab data, private examples, local manifests, private
paths, or content that exposes a source body.

## Non-Collapse Rules

This boundary locks:

- `documented != inventoried`
- `inventoried != ingestible`
- `inventory != ingestion`
- `inventory != consent`
- `owner posture != authority to use`
- `company data != public data`
- `personal data != research consent`
- `nonprofit/society data != public-benefit authority`
- `IP/asset posture != IP transfer`
- `telemetry inventory != surveillance`
- `Special Case inventory != handling permission`

## Explicit Non-Powers

This boundary does not:

- add raw Lab data
- add private research examples
- add local manifests
- add ingestion harnesses
- load data
- ingest data
- expose raw content
- collect consent
- authorize research use
- create training eligibility
- create provider visibility
- export model context
- create surveillance
- create profiles
- transfer IP
- permit Special Case handling
- activate runtime
- admit `RTME` movement

## Working Summary

`V1.2.1` now has a passive Lab data inventory schema between documented data
and any later ingestion boundary.

The schema may classify company, personal/operator, nonprofit/society,
IP/asset, witness, telemetry, and Special Case data as metadata-only governed
inventory posture.

Inventory tells Build what class of data is represented and what posture must
remain attached to it.

Inventory does not make the data ingestible, consensual, public, research
authorized, trainable, provider-visible, profile-bearing, runtime-bearing, or
movable through `RTME`.

The `LAB_DATA_INVENTORY_EVALUATION_POSTURE_BOUNDARY.md` note may read this
inventory posture for completeness, consistency, scope, and refusal conditions.
That evaluation reads inventory posture only; it does not make inventoried data
ingestible, validate raw content, collect consent, authorize use, approve
research, train models, activate runtime, or admit `RTME` movement.
