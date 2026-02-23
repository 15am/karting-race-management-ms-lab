# TASK

## 1. Metadata
ID: Coder-TASK-0022
Stage: Stage-6
ExecutionMode: Implementation | Provisioning | Testing | Deployment
DependsOn: Coder-TASK-0021
Actor: Coder
TaskType: Work | StageValidation
Status: done
BlockedBy: None

---

## 2. Objective
Validate cache invalidation behavior in DB-backed integration scenarios

---

## 3. Scope

AllowedWriteTargets:
- api/test/**
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
Add cache invalidation integration coverage and run full build/test; include SQL-enabled execution notes for environment-dependent verification.

---

## 7. Definition Of Done
✔ Task in review/
✔ Human approval
✔ Commit created

---

## 8. Execution History (AUTO-GENERATED)
> Updated by utilities/scripts/move-task.sh

- [YYYY-MM-DD HH:mm] CREATED (backlog)

- [2026-02-23 23:02] MOVED backlog → ready | Next Stage 6 task

- [2026-02-23 23:02] MOVED ready → in-progress | Starting DB-backed cache invalidation integration coverage

- [2026-02-23 23:03] MOVED in-progress → review | DB-backed cache invalidation integration coverage added

- [2026-02-23 23:03] MOVED review → done | Completed Stage 6 cache invalidation integration task
