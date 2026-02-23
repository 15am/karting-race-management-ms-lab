# SQL Stage Handoff (Stage 5 to Stage 6)

## Purpose

Stage 5 prepares SQL deployment scaffolding so Stage 6 can focus on implementation and validation.

## Prepared assets

- `deployment/sql/schema/000-bootstrap-database.sql`
- `deployment/sql/schema/010-create-core-tables.sql`
- `deployment/sql/stored-procedures/100-create-sp-get-race-results.sql`
- `deployment/sql/stored-procedures/110-create-sp-get-fastest-lap.sql`

## Contract alignment

The placeholder stored procedures map to read contracts already used by application query ports:

- `usp_GetRaceResults` for race ranking output
- `usp_GetFastestLap` for fastest-lap output

## Next stage responsibilities

- Replace placeholders with production SQL logic and indexing.
- Validate deterministic ordering and tie-breaking behavior.
- Add DB-backed integration coverage for stored procedure execution and cache behavior.
