# TASK

## 1. Metadata
ID: Planner-TASK-XXXX
Stage: Stage-X
ExecutionMode: PlanningOnly
DependsOn: None
Actor: Planner
TaskType: Feature | Fix | Refactor | Correction | Planning
Status: backlog | ready | in-progress | review | done

---

## 2. Objective
Short, concrete objective.

---

## 3. Scope
AllowedWriteTargets:
- context/** (create-only, no overwrite)
- docs/** (very specific incremental changes)
- information/**
- plan/**
- tasks/**

Read-only
- deployment/**
- rules/**
- src/**
- test/**
- utilities/**

---

## 4. Rules To Follow

Planner MUST follow:

- rules/00-runtime-directives.md
- rules/20-repository-workflow.md

---

## 5. Acceptance Criteria

- Master plan or mini plan updated coherently
- Tasks created with correct dependencies
- Scope aligned with rules/**
- No forbidden directories modified
- Planning decisions documented clearly

---

## 6. Definition Of Done
✔ Task in review/
✔ Human approval
✔ Tasks generated or updated if required

---

## 8. Execution History (AUTO-GENERATED)
> Updated by utilities/scripts/move-task.sh

- [YYYY-MM-DD HH:mm] CREATED (backlog)