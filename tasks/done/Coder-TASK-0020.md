# TASK

## 1. Metadata
ID: Coder-TASK-0020
Stage: Stage-6
ExecutionMode: Implementation | Provisioning | Testing | Deployment
DependsOn: Coder-TASK-0019
Actor: Coder
TaskType: Work | StageValidation
Status: done
BlockedBy: None

---

## 2. Objective
Align SQL Server runtime defaults and deployment script wiring

---

## 3. Scope

AllowedWriteTargets:
- deployment/**
- docs/deployment-runtime.md
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
Run deployment runtime validation script and compose rendering checks; run solution build/test to confirm runtime wiring changes remain safe.

---

## 7. Definition Of Done
✔ Task in review/
✔ Human approval
✔ Commit created

---

## 8. Execution History (AUTO-GENERATED)
> Updated by utilities/scripts/move-task.sh

- [YYYY-MM-DD HH:mm] CREATED (backlog)

- [2026-02-23 22:56] MOVED backlog → ready | Next Stage 6 task

- [2026-02-23 22:56] MOVED ready → in-progress | Starting runtime defaults and SQL wiring

- [2026-02-23 22:58] MOVED in-progress → review | Runtime defaults and SQL wiring completed

- [2026-02-23 23:03] MOVED review → done | Completed Stage 6 runtime wiring task
