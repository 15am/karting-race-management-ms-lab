# OBJECTIVES.md — AI Implementation Guide

## Purpose
This document defines the technical and architectural objectives for building a .NET 10 Web API that models a simplified karting race management system.

This file is written to be **machine-readable and AI-friendly**.  
Follow instructions strictly unless explicitly marked as optional.

---

# 1. Functional Scope

## Entities
Implement the following domain entities:

- Driver
- Race
- LapTime

Each entity must include:
- Unique identifier
- Validation rules
- Domain invariants

Suggested minimal fields:

Driver:
- Id
- Name
- CreatedAtUtc

Race:
- Id
- Name
- DateUtc
- Status

LapTime:
- Id
- RaceId
- DriverId
- LapNumber
- TimeMilliseconds
- CreatedAtUtc

---

## Required API Endpoints

POST /drivers  
POST /races  
POST /laptimes  
GET /races/{raceId}/results  
GET /races/{raceId}/fastest-lap

Rules:

- Race results must be ordered by fastest total time.
- Fastest lap must return the minimum lap time for the race.

---

# 2. Architecture Rules

## Architectural Style
Use:

- Vertical Slice Architecture
- Clean Architecture boundaries
- Pragmatic Domain Driven Design (DDD)
- Test Driven Development (TDD)

## Layer Responsibilities

Domain:
- Entities
- Value Objects
- Domain validation

Application:
- Feature handlers (one per endpoint)
- Interfaces (ports)
- Business orchestration

Infrastructure:
- EF Core
- SQL Server access
- Stored Procedures
- Distributed caching implementation
- Logging

API:
- Controllers (thin)
- Middleware
- Dependency injection

---

## Vertical Slice Structure

Organize by feature, not by technical layer.

Example:

Features/
    Drivers/Create
    Races/Create
    LapTimes/Add
    Races/GetResults
    Races/GetFastestLap

Each feature must contain:
- Request model
- Response model
- Validator
- Handler
- Endpoint mapping

---

# 3. Database Requirements

## Database Engine
Use Microsoft SQL Server.

## ORM
Use EF Core for write operations.

## Stored Procedures (MANDATORY)

Implement at least:

sp_GetRaceResults(@RaceId)  
sp_GetFastestLap(@RaceId)

Rules:

- Stored procedures perform aggregation logic.
- Results endpoint MUST use stored procedure.
- Fastest lap endpoint SHOULD use stored procedure.

---

# 4. Caching Requirements

Use distributed caching.

Preferred abstraction:
IDistributedCache

Cache Targets:
- Race Results
- Fastest Lap

Cache Keys:

race:{raceId}:results  
race:{raceId}:fastestlap

Invalidation Rules:

When POST /laptimes executes:
- Remove race:{raceId}:results
- Remove race:{raceId}:fastestlap

API instances must remain stateless.

---

# 5. Scalability Constraints

The system MUST support horizontal scaling.

Rules:

- No in-memory state required for correctness.
- Cache must be distributed-compatible.
- Database is source of truth.
- Handlers must be idempotent where possible.

---

# 6. Testing Strategy (TDD)

Unit Tests:
- Domain invariants
- Handlers
- Validators

Integration Tests:
- Endpoints
- Stored procedures
- Cache invalidation

Use xUnit or NUnit.

---

# 7. Error Handling

Requirements:

- Global exception middleware.
- RFC7807 Problem Details responses.
- Consistent HTTP status codes.

---

# 8. Logging

Use structured logging.

Log events:

- Driver created
- Race created
- Lap added
- Cache invalidated

---

# 9. Deployment Requirements

## Docker Compose (REQUIRED)

Provide docker-compose.yml including:

- API container (.NET 10)
- SQL Server container
- Redis container (distributed cache)

Containers must communicate via internal network.

Environment variables must configure:

- ConnectionStrings
- Cache settings

Execution:

docker compose up --build

---

## Bare Metal Deployment (REQUIRED)

Solution must run without Docker on:

- Linux
- macOS
- Windows

Commands must work:

dotnet restore  
dotnet build  
dotnet run

Database setup must include:

- migrations OR sql scripts
- stored procedure creation scripts

---

# 10. Nice To Have Features (IMPLEMENT IF POSSIBLE)

The following enhancements are OPTIONAL but SHOULD be implemented when feasible:

## Docker Setup
Provide a working docker-compose configuration for local execution.

## Caching Strategy
Implement distributed caching for aggregation endpoints.

## Minimal Frontend (React)
Provide a minimal React application that:

- Displays race results.
- Calls the backend API.
- Can run independently via Node.
- Uses simple UI (no heavy styling required).

Suggested structure:

frontend/
    src/
    package.json

## Tests
Include unit tests and integration tests.

## Repository Pattern
Use repository pattern only if it provides clear value.  
Avoid unnecessary abstractions.

---

# 11. Documentation Requirements

The repository MUST contain:

README.md  
NOTES.md  
OBJECTIVES.md (this file)

README.md must include:
- Project overview
- Setup instructions
- Docker execution
- Bare metal execution
- Frontend execution
- Testing instructions

NOTES.md must include:
- Design decisions
- Tradeoffs
- Caching explanation
- Stored procedure rationale
- Future improvements
- AI usage disclosure

---

# 12. Technology Stack

Backend:
- .NET 10 Web API
- EF Core
- SQL Server
- Distributed Cache (Redis-ready)
- FluentValidation (recommended)

Frontend:
- React (minimal UI)

Testing:
- xUnit or NUnit

---

# 13. Implementation Priority

Follow this order:

1. Domain Models
2. Database + Stored Procedures
3. Feature Handlers
4. API Endpoints
5. Caching
6. Tests
7. React Minimal Frontend
8. Docker Compose
9. Documentation

Do not skip stored procedures.
Do not centralize business logic in controllers.
Keep handlers small and isolated.

---

# 14. Constraints

- Follow rules/*
- Respect repository workflow
- Produce staged mini-plans