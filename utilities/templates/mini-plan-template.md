# MINI PLAN

## 1. Metadata
StageNumber: X
StageName: replace-with-stage-name
BranchName: feature/replace-with-stage-name
Status: Draft | Ready | Active | Completed

---

## 2. Objective
Short declarative goal of this stage.

---

## 3. Scope

AllowedWriteTargets:
- api/**
- frontend/**
- deployment/** (optional)
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
  Description: Domain model created

- ID: D2
  Description: API endpoints implemented

- ID: D3
  Description: Tests covering main flows

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
None | Describe briefly

---

## 8. Notes For Planner (Guardrails)

Planner MUST:

- Follow rules/00-runtime-directives.md
- Avoid narrative descriptions
- Use declarative structure only
- MUST NOT rewrite master-plan.md