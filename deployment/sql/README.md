# SQL Deployment Handoff

This directory contains SQL Server assets prepared in Stage 5 for Stage 6 implementation.

## Structure

- `schema/`: database and table scripts
- `stored-procedures/`: read-model stored procedure scripts
- `seed/`: idempotent sample data scripts for local testing

## Ordering convention

Scripts use numeric prefixes to preserve deterministic execution order:

1. `000-099` database/schema bootstrap
2. `100-199` stored procedure creation
3. `200-299` seed data

## Local sample race

Seed script `seed/200-seed-sample-data.sql` creates a demo race with this id:

- `11111111-1111-1111-1111-111111111111`

## Stage 6 targets

- Finalize table/constraint definitions.
- Implement stored procedures for race results and fastest lap.
- Wire script execution into integration/runtime validation flow.
