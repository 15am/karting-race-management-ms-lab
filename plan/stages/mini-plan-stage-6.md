# MINI PLAN

## 1. Metadata
StageNumber: 6
StageName: sqlserver-stored-procedures-and-db-scripts
BranchName: feature/sqlserver-stored-procedures-and-db-scripts
Status: Draft

---

## 2. Objective
Implement SQL scripts and stored procedure assets, then harden DB-backed integration validation.

---

## 3. Scope

AllowedWriteTargets:
- api/**
- deployment/**
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
  Description: SQL scripts for schema setup and stored procedure creation

- ID: D2
  Description: Runtime defaults and configuration aligned for SQL Server-backed execution

- ID: D3
  Description: Integration tests validating stored procedure-backed race aggregate endpoints

- ID: D4
  Description: Cache invalidation behavior verified in DB-backed integration scenarios

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
Stage-5 completed

---

## 8. Notes For Planner (Guardrails)

Planner MUST:

- Follow rules/00-runtime-directives.md
- Avoid narrative descriptions
- Use declarative structure only
- MUST NOT rewrite master-plan.md
