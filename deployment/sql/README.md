# SQL Deployment Handoff

This directory contains SQL Server assets prepared in Stage 5 for Stage 6 implementation.

## Structure

- `schema/`: database and table scripts
- `stored-procedures/`: read-model stored procedure scripts

## Ordering convention

Scripts use numeric prefixes to preserve deterministic execution order:

1. `000-099` database/schema bootstrap
2. `100-199` stored procedure creation

## Stage 6 targets

- Finalize table/constraint definitions.
- Implement stored procedures for race results and fastest lap.
- Wire script execution into integration/runtime validation flow.
