# MASTER PLAN

## 1. Project Metadata
ProjectName: karting-race-management
RepositoryType: SingleRepo
ExecutionModel: StageDriven

---

## 2. Global Objective
Deliver a modular .NET 10 karting race management API with SQL Server first-class support,
provider-ready query/persistence ports, cached race aggregates, and deterministic stage-driven execution.

---

## 3. Architectural Constraints

Planner MUST follow:

- rules/00-runtime-directives.md
- rules/10-code-architecture.md
- rules/20-repository-workflow.md
- rules/90-anti-patterns.md

Technology Stack:
- .NET 10
- SQL Server
- IDistributedCache (in-memory implementation during API core stages)

---

## 4. Code Areas

Primary areas allowed for stages:

- api/src/**
- api/test/**
- plan/**
- tasks/**
- context/**

ReadOnly knowledge:

- information/**
- rules/**
- utilities/**
- frontend/**

---

## 5. Stage Strategy

Planner MUST decompose delivery into ordered stages.

Each stage MUST:

- Have a dedicated mini plan
- Define a BranchName
- Produce coder tasks
- End with validate-stage.sh

Planner MUST NOT implement code directly.

---

## 6. Stage Order (Declarative)

- Stage-1: API foundation and write endpoints
- Stage-2: Race aggregate queries with stored procedures and distributed cache
- Stage-3: Hardening, provider-readiness, and integration coverage
- Stage-4: XML compliance and architecture documentation with PlantUML
- Stage-5: Deployment runtime foundation (Docker Compose with SQL Server and Redis)
- Stage-6: SQL Server scripts, stored procedures, and DB integration hardening
- Stage-7: Minimal frontend for race results visualization
- Stage-8: Final root documentation (README and NOTES) and closure validation

Planner MAY refine stages but MUST preserve order.

---

## 7. Validation Model

A stage is considered complete when:

- All coder tasks are done
- validate-stage.sh succeeds
- complete-stage.sh merges into development
- Human promotes development → main

---

## 8. Non-Functional Guardrails

Planner MUST prioritize:

- Deterministic structure
- Small incremental stages
- Clear dependency ordering
- Minimal file surface per stage

---

## 9. Notes For Planner

Planner SHOULD:

- Generate mini plans under plan/stages/
- Use declarative language only
- Avoid narrative or long explanations
- Use task lifecycle: ready → in-progress → review → done
