# Karting Race Management

A stage-driven .NET 10 solution for managing drivers, races, lap times, and aggregate race views.

## What is included

- API (`api/`) with clean boundaries and vertical slices
- SQL Server schema and stored procedures (`deployment/sql/`)
- Distributed cache integration for race aggregate endpoints
- Frontend (`frontend/`) for race results and fastest-lap exploration
- Deployment runtime with Docker Compose (`deployment/docker-compose.yml`)

## Prerequisites

- Docker (required for full runtime and frontend workflow)
- .NET SDK 10 (required for local API build/test and local API execution)

## Quick Start (Docker Compose)

1. Create environment file:
   `cp deployment/.env.example deployment/.env`
2. Set a strong SQL password in `deployment/.env` (`SA_PASSWORD`).
3. Validate runtime:
   `deployment/scripts/validate-runtime.sh`
4. Start stack:
   `docker compose --env-file deployment/.env -f deployment/docker-compose.yml up -d`
5. Apply SQL schema and stored procedures:
   `deployment/scripts/apply-sql-scripts.sh`

### Default URLs

- Frontend: `http://localhost:4173`
- API: `http://localhost:8080`
- OpenAPI document: `http://localhost:8080/openapi/v1.json`

## Frontend workflow without local Node

Run from `frontend/`:

- Install dependencies:
  `./scripts/npm-docker.sh install`
- Start frontend dev server:
  `./scripts/npm-docker.sh run dev`
- Lint:
  `./scripts/npm-docker.sh run lint`
- Build:
  `./scripts/npm-docker.sh run build`

## API local workflow (no Docker)

- Build:
  `dotnet build api/Karting.slnx --nologo`
- Test:
  `dotnet test api/Karting.slnx --nologo`
- Run API:
  `dotnet run --project api/src/Karting.Api/Karting.Api.csproj`

## OpenAPI interaction option

The API exposes Microsoft OpenAPI output at `/openapi/v1.json`.

Example with Microsoft HTTP REPL:

1. Install once:
   `dotnet tool install --global Microsoft.dotnet-httprepl`
2. Connect:
   `httprepl http://localhost:8080 --openapi /openapi/v1.json`

## Key Documentation

- Architecture overview: `docs/architecture-overview.md`
- Deployment runtime: `docs/deployment-runtime.md`
- Frontend runtime: `docs/frontend-runtime.md`
- API OpenAPI usage: `docs/api-openapi.md`
- SQL stage handoff: `docs/sql-stage-handoff.md`
- Provider extension notes: `docs/provider-extension-notes.md`

## Objective source

The implementation objective is maintained in `information/objective.md`.
