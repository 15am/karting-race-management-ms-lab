# Architecture Overview

This document describes the Stage 4 architecture baseline for `karting-race-management`.

## Objectives
- Keep the HTTP contract stable.
- Keep application behavior provider-ready.
- Document write/read request flows and data model boundaries.

## Component view
PlantUML source: [`docs/plantuml/component-architecture.puml`](plantuml/component-architecture.puml)

This diagram shows:
- API layer controllers and middleware
- Application feature handlers and ports
- Infrastructure adapters for persistence and query operations
- SQL Server and distributed cache integration points

## Write flow
PlantUML source: [`docs/plantuml/sequence-write-flow.puml`](plantuml/sequence-write-flow.puml)

This diagram focuses on `POST /laptimes` and captures:
- Validation and domain construction
- Persistence orchestration
- Cache invalidation for race results and fastest lap keys

## Read flow
PlantUML source: [`docs/plantuml/sequence-read-flow.puml`](plantuml/sequence-read-flow.puml)

This diagram shows read-side behavior for:
- `GET /races/{raceId}/results`
- `GET /races/{raceId}/fastest-lap`
- Cache hit/miss mediation and query-port invocation

## Data model
PlantUML source: [`docs/plantuml/data-model.puml`](plantuml/data-model.puml)

This diagram represents the persistence entities and relationships:
- `DriverEntity`
- `RaceEntity`
- `LapTimeEntity`

## Assumptions
- SQL Server is the primary provider for stored-procedure-backed aggregate reads.
- In-memory provider remains available for local and integration-test workflows.
- `IDistributedCache` abstraction is mandatory for cache behavior portability.
