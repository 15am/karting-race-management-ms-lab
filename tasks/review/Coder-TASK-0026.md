# TASK

## 1. Metadata
ID: Coder-TASK-0026
Stage: Stage-8
ExecutionMode: Implementation | Provisioning | Testing | Deployment
DependsOn: None
Actor: Coder
TaskType: Work | StageValidation
Status: review
BlockedBy: None

---

## 2. Objective
Remediate frontend npm vulnerabilities and refresh dependency lockfile

---

## 3. Scope

AllowedWriteTargets:
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
Run Dockerized frontend dependency audit and verify frontend lint and build succeed after remediation.

---

## 7. Definition Of Done
✔ Task in review/
✔ Human approval
✔ Commit created

---

## 8. Execution History (AUTO-GENERATED)
> Updated by utilities/scripts/move-task.sh

- [YYYY-MM-DD HH:mm] CREATED (backlog)

- [2026-02-23 23:37] MOVED backlog → ready | Stage 8 entry task

- [2026-02-23 23:37] MOVED ready → in-progress | Starting npm vulnerability remediation

- [2026-02-23 23:39] MOVED in-progress → review | Frontend audit remediated with zero high vulnerabilities
