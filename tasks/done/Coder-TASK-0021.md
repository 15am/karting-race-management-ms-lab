# TASK

## 1. Metadata
ID: Coder-TASK-0021
Stage: Stage-6
ExecutionMode: Implementation | Provisioning | Testing | Deployment
DependsOn: Coder-TASK-0020
Actor: Coder
TaskType: Work | StageValidation
Status: done
BlockedBy: None

---

## 2. Objective
Implement DB-backed integration tests for race aggregate endpoints

---

## 3. Scope

AllowedWriteTargets:
- api/test/**
- api/src/**
- docs/sql-stage-handoff.md

Read-only
- context/**
- information/**
- rules/**
- utilities/**
- tasks/**
- plan/**

---

## 4. Rules To Follow

Coder MUST follow:

- rules/00-runtime-directives.md
- rules/10-code-architecture.md
- rules/20-repository-workflow.md
- rules/90-anti-patterns.md

---

## 5. Acceptance Criteria
- Build succeeds
- Tests pass
- Only AllowedWriteTargets modified

---

## 6. Testing Strategy
Run integration tests with environment-gated SQL Server execution path and standard solution build/test for baseline coverage.

---

## 7. Definition Of Done
✔ Task in review/
✔ Human approval
✔ Commit created

---

## 8. Execution History (AUTO-GENERATED)
> Updated by utilities/scripts/move-task.sh

- [YYYY-MM-DD HH:mm] CREATED (backlog)

- [2026-02-23 22:58] MOVED backlog → ready | Next Stage 6 task

- [2026-02-23 22:58] MOVED ready → in-progress | Starting DB-backed integration tests for aggregates

- [2026-02-23 23:02] MOVED in-progress → review | DB-backed aggregate integration test scaffolded and validated

- [2026-02-23 23:03] MOVED review → done | Completed Stage 6 DB aggregate integration task
