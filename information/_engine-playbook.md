# ENGINE PLAYBOOK

This document describes the execution lifecycle of the autonomous development engine.

It defines:

- Intent of each script
- Who executes it (Planner | Coder | Human)
- When it runs
- Why it exists

This file is READ-ONLY operational context.

---

# 1. Execution Roles

## Planner
Responsible for:
- Reading objective
- Creating master plan
- Creating mini plans (stages)
- Generating tasks
- Adjusting planning when coder fails

Planner NEVER modifies any code implementation directory.

---

## Coder
Responsible for:
- Implementing tasks
- Running validation scripts
- Creating commits
- Moving tasks to review/

Coder NEVER modifies:
- rules/
- information/
- plan/

---

## Human
Responsible for:
- Approving merges
- Evaluating task completion
- Triggering planner when blocked

---

# 2. Lifecycle Overview

objective.md
↓
bootstrap-plans.sh
↓
create-master-plan.sh
↓
create-mini-plan.sh
↓
start-stage.sh
↓
start-planner.sh
↓
create-task.sh
↓
validate-task.sh
↓
validate-stage.sh
↓
complete-stage.sh

---

# 3. Script Intent

## 3.1 Planning & Initialization

### `bootstrap-plans.sh`

* **WHO:** Planner
* **WHEN:** First run or major objective change.
* **WHY:** Bulk generates the `master-plan.md` and stage skeletons.

### `create-master-plan.sh`

* **WHO:** Planner
* **WHEN:** During initialization.
* **WHY:** Specialized script to instantiate the master plan from a template.

### `create-mini-plan.sh`

* **WHO:** Planner
* **WHEN:** Before starting a new stage.
* **WHY:** Generates a specific `mini-plan-stage-X.md` with the required metadata.

---

## 3.2 Stage Lifecycle

### `start-stage.sh`

* **WHO:** Human or Planner
* **WHEN:** Beginning a new stage.
* **WHY:** High-level orchestrator that sets the branch and marks the stage as "Active".

### `set-current-stage.sh`

* **WHO:** System (called by start-stage)
* **WHEN:** When a stage is activated.
* **WHY:** Hard-writes the active stage metadata into `context/_current-stage.md`.

### `is-stage-complete.sh`

* **WHO:** Planner
* **WHEN:** Before closing a stage.
* **WHY:** Verifies if there are any pending tasks in `backlog`, `ready`, or `in-progress`.

### `mark-stage-complete.sh`

* **WHO:** System (called by complete-stage)
* **WHEN:** Upon successful stage closure.
* **WHY:** Records the completion in `context/_last-completed-stage.md`.

### `complete-stage.sh`

* **WHO:** Planner or Coder
* **WHEN:** Stage finished and validated.
* **WHY:** Final merge of the feature branch into `development`.

---

## 3.3 Task Management (The Kanban Engine)

### `start-planner.sh`

* **WHO:** Planner
* **WHEN:** No coder task active.
* **WHY:** Entry point for the Planner to start a thinking/task-generation cycle.

### `create-task.sh`

* **WHO:** Planner
* **WHEN:** Breaking down mini-plans.
* **WHY:** Creates deterministic task files with strict `AllowedWriteTargets`.

### `move-task.sh`

* **WHO:** Coder or Planner
* **WHEN:** Task state changes.
* **WHY:** Updates the task's location and appends the execution history.

### `current-task.sh`

* **WHO:** System/Coder
* **WHEN:** Any time task context is needed.
* **WHY:** Locates the task currently in `in-progress` for the given role.

---

## 3.4 Execution & Validation

### `git-branch.sh`

* **WHO:** Coder
* **WHEN:** Starting work on a stage.
* **WHY:** Forces the environment into the correct branch defined in the Mini Plan.

### `git-commit-safe.sh`

* **WHO:** Coder
* **WHEN:** To persist task progress.
* **WHY:** Enforces semantic commit messages based on task metadata.

### `validate-task.sh`

* **WHO:** Coder
* **WHEN:** Pre-commit/Pre-review.
* **WHY:** Static analysis for Object Calisthenics and scope check.

### `validate-stage.sh`

* **WHO:** Planner/Coder
* **WHEN:** Before stage completion.
* **WHY:** Runs `dotnet build`, `dotnet test`, and global scope validation.

### `emergency-stop.sh`

* **WHO:** Planner/Human
* **WHEN:** Critical failure or loop.
* **WHY:** Hard reset of the workspace and rollback of the active task.

---

## 3.5 Observability & Context (Read-Only)

### `diff-summary.sh`

* **WHO:** Planner
* **WHEN:** Evaluating Coder progress.
* **WHY:** Generates a technical report of changes vs `development`.

### `file-tree.sh`

* **WHO:** Planner/Coder
* **WHEN:** Any time.
* **WHY:** Quick visualization of the allowed source directories.

### `task-context.sh`

* **WHO:** Coder/Planner
* **WHEN:** At the start of a prompt.
* **WHY:** Extracts metadata and objectives of the current task for AI context.

### `read-target-files.sh`

* **WHO:** Coder/Planner
* **WHEN:** During implementation/review.
* **WHY:** Reads only the content of files defined in `AllowedWriteTargets`.

---

# 4. Critical Constraints (Guardrails)

### A. Architectural Integrity (Object Calisthenics)

* **The "Else" Ban:** All logic must use Guard Clauses or Polymorphism. `else` keywords trigger a fail in `validate-task.sh`.
* **Class Density:** Files exceeding 120 lines are flagged. This forces decomposition into smaller services/Value Objects.
* **State Constraint:** Maximum 2 fields per class to prevent "God Objects".

### B. Sandbox & Scope

* **Write Restriction:** The Coder is only allowed to modify files matching the `AllowedWriteTargets` in the active task.
* **Planner Isolation:** The Planner is strictly forbidden from modifying `src/`, `test/`, or `api/`.

### C. Branch Protection

* `main` and `development` are read-only for the Coder.
* Work MUST happen in branches named `feature/stage-X-...`.
* `complete-stage.sh` is the only authorized way to merge into `development`.

### D. Contextual Continuity

* Scripts like `task-context.sh` and `read-target-files.sh` ensure the AI never hallucinates the state of the project.
* Every significant action MUST be logged in `information/_stage-registry.log`.