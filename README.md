# Karting Race Management

Stage-driven .NET 10 solution for drivers, races, lap times, and race aggregates (results + fastest lap).

## What is included

- API (`api/`) with clean boundaries and vertical slices
- SQL Server schema, stored procedures, and seed data (`deployment/sql/`)
- Distributed cache integration for aggregate race reads
- Frontend (`frontend/`) for race results and fastest-lap exploration
- Docker Compose runtime (`deployment/docker-compose.yml`) with SQL bootstrap + seed execution

## Prerequisites

- Docker (required for full runtime)
- .NET SDK 10 (required for local API build/test)

## Quick Start (Docker Compose)

1. Create environment file:
   `cp deployment/.env.example deployment/.env`
2. Set a strong SQL password in `deployment/.env` (`SA_PASSWORD`).
3. Validate runtime inputs:
   `deployment/scripts/validate-runtime.sh`
4. Start stack:
   `docker compose --env-file deployment/.env -f deployment/docker-compose.yml up -d`

`sql-bootstrap` runs automatically during startup and applies:

- database bootstrap
- table/constraint schema
- stored procedures
- sample seed data

### Default URLs

- Frontend: `http://localhost:4173`
- API: `http://localhost:8080`
- OpenAPI document: `http://localhost:8080/openapi/v1.json`

## Seeded demo data

The compose bootstrap seeds a demo race:

- Race ID: `11111111-1111-1111-1111-111111111111`
- Driver IDs:
  - `22222222-2222-2222-2222-222222222222`
  - `33333333-3333-3333-3333-333333333333`
  - `44444444-4444-4444-4444-444444444444`

The frontend also shows these IDs in a `Test IDs` panel for copy/paste.

## Endpoint smoke check (running compose)

- `GET /openapi/v1.json`
- `POST /drivers`
- `POST /races`
- `POST /laptimes`
- `GET /races/{raceId}/results`
- `GET /races/{raceId}/fastest-lap`

## Manual SQL re-apply (optional)

If you reset SQL volumes or need to re-run scripts explicitly:

`deployment/scripts/apply-sql-scripts.sh`

## Frontend workflow without local Node

Run from `frontend/` with Dockerized Node wrappers:

- `./scripts/npm-docker.sh install`
- `./scripts/npm-docker.sh run dev`
- `./scripts/npm-docker.sh run lint`
- `./scripts/npm-docker.sh run build`

## API local workflow (no Docker)

- Build: `dotnet build api/Karting.slnx --nologo`
- Test: `dotnet test api/Karting.slnx --nologo`
- Run: `dotnet run --project api/src/Karting.Api/Karting.Api.csproj`

## OpenAPI interaction

Microsoft OpenAPI output is exposed at `/openapi/v1.json`.

HTTP REPL example:

1. `dotnet tool install --global Microsoft.dotnet-httprepl`
2. `httprepl http://localhost:8080 --openapi /openapi/v1.json`

## Key Documentation

- `docs/architecture-overview.md`
- `docs/deployment-runtime.md`
- `docs/frontend-runtime.md`
- `docs/api-openapi.md`
- `docs/sql-stage-handoff.md`
- `docs/provider-extension-notes.md`
- `deployment/sql/README.md`

## Objective source

The implementation objective is maintained in `information/objective.md`.
