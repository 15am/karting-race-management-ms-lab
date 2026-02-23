# TASK

## 1. Metadata
ID: Coder-TASK-XXXX
Stage: Stage-X
ExecutionMode: Implementation | Provisioning | Testing | Deployment
DependsOn: None
Actor: Coder
TaskType: Work | StageValidation
Status: backlog | ready | in-progress | review | done
BlockedBy: None

---

## 2. Objective
Short, concrete objective.

---

## 3. Scope

AllowedWriteTargets:
- deployment/** (optional)
- docs/** (very specific incremental changes)
- api/**
- frontend/**

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
Describe required tests briefly.

---

## 7. Definition Of Done
✔ Task in review/
✔ Human approval
✔ Commit created

---

## 8. Execution History (AUTO-GENERATED)
> Updated by utilities/scripts/move-task.sh

- [YYYY-MM-DD HH:mm] CREATED (backlog)