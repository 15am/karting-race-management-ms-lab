# Deployment Runtime Foundation

## Scope

This document defines the Stage 5 deployment runtime baseline for local execution:

- API container built from `api/src/Karting.Api/Dockerfile`
- SQL Server container for upcoming database-backed stages
- Redis container for distributed cache flows
- AppArmor compatibility via `apparmor:unconfined` on all services

## Files

- `deployment/docker-compose.yml`
- `deployment/.env.example`
- `deployment/scripts/validate-runtime.sh`
- `deployment/scripts/apply-sql-scripts.sh`
- `deployment/sql/**` (Stage 6 handoff scaffolding)

## Startup Validation

1. Create an environment file:
   `cp deployment/.env.example deployment/.env`
2. Update `SA_PASSWORD` in `deployment/.env` with a strong password.
3. Validate startup configuration:
   `deployment/scripts/validate-runtime.sh`
4. Start runtime:
   `docker compose --env-file deployment/.env -f deployment/docker-compose.yml up -d`
5. Apply schema and stored procedures:
   `deployment/scripts/apply-sql-scripts.sh`

The script validates compose rendering with `docker compose config` and fails if default SQL password remains.

## Service Checks

- `sqlserver` includes a health check using `sqlcmd SELECT 1`.
- `redis` includes a health check using `redis-cli ping`.
- `api` startup waits for SQL Server and Redis healthy state through `depends_on` conditions.

## SQL runtime wiring

SQL assets are mounted read-only into the SQL Server container at `/opt/karting/sql`.

Script execution order:

1. `deployment/sql/schema/*.sql`
2. `deployment/sql/stored-procedures/*.sql`

SQL details are documented in:

- `deployment/sql/README.md`
- `docs/sql-stage-handoff.md`
