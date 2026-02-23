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
- `deployment/sql/**` (Stage 6 handoff scaffolding)

## Startup Validation

1. Create an environment file:
   `cp deployment/.env.example deployment/.env`
2. Update `SA_PASSWORD` in `deployment/.env` with a strong password.
3. Validate startup configuration:
   `deployment/scripts/validate-runtime.sh`

The script validates compose rendering with `docker compose config` and fails if default SQL password remains.

## Service Checks

- `sqlserver` includes a health check using `sqlcmd SELECT 1`.
- `redis` includes a health check using `redis-cli ping`.
- `api` startup waits for SQL Server and Redis healthy state through `depends_on` conditions.

## SQL handoff

SQL scaffolding and placeholders for the next stage are documented in:

- `deployment/sql/README.md`
- `docs/sql-stage-handoff.md`
