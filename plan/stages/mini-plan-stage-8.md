# MINI PLAN

## 1. Metadata
StageNumber: 8
StageName: final-readme-notes-and-closure
BranchName: feature/final-readme-notes-and-closure
Status: Draft

---

## 2. Objective
Finalize repository-level documentation with root README.md and NOTES.md after implementation stages complete.

---

## 3. Scope

AllowedWriteTargets:
- docs/**
- README.md
- NOTES.md
- deployment/** (minimal reference links only)

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
  Description: Root README.md with setup, docker, bare-metal, frontend, and testing instructions

- ID: D2
  Description: Root NOTES.md with design decisions, tradeoffs, caching and stored procedure rationale, and future work

- ID: D3
  Description: Documentation cross-links aligned with information/objective.md constraints

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
Stage-7 completed

---

## 8. Notes For Planner (Guardrails)

Planner MUST:

- Follow rules/00-runtime-directives.md
- Avoid narrative descriptions
- Use declarative structure only
- MUST NOT rewrite master-plan.md
