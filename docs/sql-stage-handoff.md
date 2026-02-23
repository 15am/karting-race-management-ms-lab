# SQL Stage Handoff (Stage 6 Baseline)

## Purpose

Stage 6 finalizes SQL Server schema and stored procedures that back race aggregate read contracts.

## Finalized assets

- `deployment/sql/schema/000-bootstrap-database.sql`
- `deployment/sql/schema/010-create-core-tables.sql`
- `deployment/sql/stored-procedures/100-create-sp-get-race-results.sql`
- `deployment/sql/stored-procedures/110-create-sp-get-fastest-lap.sql`
- `deployment/scripts/apply-sql-scripts.sh`

## Contract alignment

Stored procedures map directly to read contracts used by application query ports:

- `sp_GetRaceResults` for race ranking output
- `sp_GetFastestLap` for fastest-lap output

## Deterministic behavior guarantees

- Race results are ordered by total time, then driver name, then driver id.
- Fastest lap is ordered by lap time, then lap number, then driver name, then driver id.
- Table-level check constraints enforce positive lap number and lap time.

## Runtime application path

- Validate compose and environment with `deployment/scripts/validate-runtime.sh`.
- Start SQL Server with compose.
- Apply schema and stored procedures with `deployment/scripts/apply-sql-scripts.sh`.

## Integration test toggle

DB-backed integration tests are environment-gated to keep default test runs portable.

- `KARTING_SQL_INTEGRATION=true`
- `KARTING_SQL_CONNECTION_STRING=Server=localhost,1433;Database=KartingRaceManagement;User Id=sa;Password=<password>;TrustServerCertificate=True`
- SQL-enabled tests validate aggregate endpoint correctness and cache invalidation behavior after lap creation.
