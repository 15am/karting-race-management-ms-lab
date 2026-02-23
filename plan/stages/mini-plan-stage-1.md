# MINI PLAN

## 1. Metadata
StageNumber: 1
StageName: api-core-foundation-writes
BranchName: feature/api-core-foundation-writes
Status: Completed

---

## 2. Objective
Establish solution foundation and implement write-side endpoints with domain invariants and baseline cross-cutting concerns.

---

## 3. Scope

AllowedWriteTargets:
- api/**
- docs/** (minimal)

PlannerWriteTargets:
- plan/**
- tasks/**
- context/**

ReadOnly:
- rules/**
- utilities/**
- information/**

---

## 4. Deliverables

- ID: D1
  Description: Multi-project solution in api/src and api/test with clean dependency direction

- ID: D2
  Description: Driver, Race, and LapTime domain invariants and provider-ready write interfaces

- ID: D3
  Description: POST /drivers, POST /races, POST /laptimes endpoints with validators and handlers

- ID: D4
  Description: Global exception middleware using Problem Details and structured logging for create flows

- ID: D5
  Description: Unit tests for domain, validators, and write handlers

---

## 5. Execution Strategy

Planner MUST:

- Generate planner tasks if planning refinement is required
- Generate coder tasks ONLY for this stage
- Move ONE coder task to ready/
- MUST NOT generate tasks for future stages

Coder MUST:

- Work only on one task at a time
- Commit per task

---

## 6. Validation Strategy

Stage considered valid when:

- validate-stage.sh exit code = 0
- validate-task.sh passed for all tasks
- All tasks moved to done/
- Human approves merge

---

## 7. Dependencies
None

---

## 8. Notes For Planner (Guardrails)

Planner MUST:

- Follow rules/00-runtime-directives.md
- Avoid narrative descriptions
- Use declarative structure only
- MUST NOT rewrite master-plan.md
