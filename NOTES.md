# NOTES

## Approach and key design decisions

1. Vertical slices + clean boundaries
- API behavior is organized by feature slices (drivers, races, lap times, race aggregates).
- Application depends on ports/contracts; infrastructure provides adapters.

2. SQL Server first, provider-ready
- SQL Server is the active provider for current delivery.
- Read/query contracts are exposed through application ports so another SQL provider can be added later with new adapters.

3. Aggregate reads through stored procedures
- `GET /races/{raceId}/results` and `GET /races/{raceId}/fastest-lap` use SQL stored procedures.
- Ordering logic is deterministic in SQL to keep response order stable.

4. Cache invalidation and consistency
- Aggregate reads use distributed cache abstractions.
- `POST /laptimes` invalidates both aggregate cache keys for that race.

5. Runtime strategy for constrained environments
- Compose services include `apparmor:unconfined` where needed.
- SQL initialization is automated with a dedicated `sql-bootstrap` service.
- Seed data is applied automatically to support immediate frontend/API testing.

6. API discoverability
- Built-in Microsoft OpenAPI document exposed at `/openapi/v1.json`.
- Supports CLI-driven exploration through Microsoft HTTP REPL.

## Tradeoffs made

1. Frontend compose service uses Vite dev server
- Faster feedback for development and testing.
- Not a production-grade static serving approach.

2. OpenAPI document only (no UI bundle)
- Keeps dependency footprint lower and stays aligned with built-in ASP.NET capabilities.
- Requires external client/tooling for interactive browsing.

3. Startup bootstrap in compose
- Improves local developer onboarding and avoids manual SQL setup.
- Adds startup coupling between API availability and SQL bootstrap completion.

4. Stage validation adaptation
- Repository scripts have some operational caveats; practical validation used explicit `api/Karting.slnx` commands when needed.

## What I would improve with more time

1. Add production-ready frontend deployment path (multi-stage build + static server/reverse proxy).
2. Add authenticated API access and authorization policies.
3. Expand integration test matrix with SQL-backed CI services.
4. Add migration/versioning strategy for SQL scripts beyond numeric ordering.
5. Add richer API consumer experience (OpenAPI UI option and example request collections).

## Where AI assisted

AI (Codex) assisted with:

- scaffolding and iterating stage tasks/workflow artifacts
- implementing repetitive code and documentation updates
- drafting/refining Docker Compose and SQL bootstrap wiring
- generating and validating incremental endpoint smoke checks

All architectural decisions, scope choices, and final merges were user-reviewed and approved.
