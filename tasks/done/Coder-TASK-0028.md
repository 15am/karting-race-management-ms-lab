# TASK

## 1. Metadata
ID: Coder-TASK-0028
Stage: Stage-8
ExecutionMode: Implementation | Provisioning | Testing | Deployment
DependsOn: Coder-TASK-0027
Actor: Coder
TaskType: Work | StageValidation
Status: done
BlockedBy: None

---

## 2. Objective
Expose Microsoft OpenAPI endpoint for API interaction

---

## 3. Scope

AllowedWriteTargets:
- api/src/Karting.Api/**
- docs/**

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
Run API build and test baseline, then verify OpenAPI document endpoint is exposed and documented.

---

## 7. Definition Of Done
✔ Task in review/
✔ Human approval
✔ Commit created

---

## 8. Execution History (AUTO-GENERATED)
> Updated by utilities/scripts/move-task.sh

- [YYYY-MM-DD HH:mm] CREATED (backlog)

- [2026-02-23 23:42] MOVED backlog → ready | Next Stage 8 task

- [2026-02-23 23:42] MOVED ready → in-progress | Starting Microsoft OpenAPI exposure

- [2026-02-23 23:43] MOVED in-progress → review | Microsoft OpenAPI endpoint exposed and validated

- [2026-02-24 00:37] MOVED review → done | Human approved and merged to development
