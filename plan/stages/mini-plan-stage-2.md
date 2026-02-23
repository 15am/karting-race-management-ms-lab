# MINI PLAN

## 1. Metadata
StageNumber: 2
StageName: api-race-aggregates-cache
BranchName: feature/api-race-aggregates-cache
Status: Completed

---

## 2. Objective
Implement read-side race aggregates via SQL Server stored procedures with distributed cache mediation.

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
  Description: Query ports for race results and fastest lap exposed by application layer

- ID: D2
  Description: SQL Server infrastructure adapters invoking stored procedures for aggregate endpoints

- ID: D3
  Description: GET /races/{raceId}/results and GET /races/{raceId}/fastest-lap handlers with cache integration

- ID: D4
  Description: Cache keys and invalidation consistency validated for lap creation and aggregate reads

- ID: D5
  Description: Unit tests covering cache hit/miss and query handler behavior with mocks

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
Stage-1 completed

---

## 8. Notes For Planner (Guardrails)

Planner MUST:

- Follow rules/00-runtime-directives.md
- Avoid narrative descriptions
- Use declarative structure only
- MUST NOT rewrite master-plan.md
