# TASK

## 1. Metadata
ID: Coder-TASK-0001
Stage: Stage-1
ExecutionMode: Implementation | Provisioning | Testing | Deployment
DependsOn: None
Actor: Coder
TaskType: Work | StageValidation
Status: review
BlockedBy: None

---

## 2. Objective
Scaffold dotnet10 multi project solution and enforce dependency direction in api

---

## 3. Scope

AllowedWriteTargets:
- api/**

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
Run dotnet build and dotnet test for impacted projects and add focused test coverage for the task objective.

---

## 7. Definition Of Done
✔ Task in review/
✔ Human approval
✔ Commit created

---

## 8. Execution History (AUTO-GENERATED)
> Updated by utilities/scripts/move-task.sh

- [YYYY-MM-DD HH:mm] CREATED (backlog)
- [2026-02-23 21:24] MOVED backlog → ready | Stage 1 entry task

- [2026-02-23 21:40] MOVED ready → review | Implemented and validated
