# MINI PLAN

## 1. Metadata
StageNumber: 8
StageName: final-readme-notes-and-closure
BranchName: feature/final-readme-notes-and-closure
Status: Completed

---

## 2. Objective
Finalize production-readiness updates and repository-level documentation by remediating frontend dependency issues, extending deployment with frontend runtime, exposing OpenAPI interaction, and completing root README.md and NOTES.md.

---

## 3. Scope

AllowedWriteTargets:
- frontend/**
- deployment/**
- api/src/Karting.Api/**
- docs/**
- README.md
- NOTES.md

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
  Description: Frontend npm dependency vulnerabilities remediated and lockfile aligned

- ID: D2
  Description: Deployment compose includes frontend service for integrated testing workflow

- ID: D3
  Description: API exposes Microsoft OpenAPI endpoint for interactive contract access

- ID: D4
  Description: Root README.md with setup, docker, bare-metal, frontend, and testing instructions

- ID: D5
  Description: Root NOTES.md with design decisions, tradeoffs, caching and stored procedure rationale, and future work

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
