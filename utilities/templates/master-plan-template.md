# MASTER PLAN

## 1. Project Metadata
ProjectName: karting-race-management
RepositoryType: SingleRepo
ExecutionModel: StageDriven

---

## 2. Global Objective
High-level declarative goal.

Example:
Deliver a backend service to manage karting races, sessions, and results.

---

## 3. Architectural Constraints

Planner MUST follow:

- rules/00-runtime-directives.md
- rules/10-code-architecture.md
- rules/20-repository-workflow.md
- rules/90-anti-patterns.md

Technology Stack:
- .NET
- SQL Server
- Redis
- Docker Compose

---

## 4. Code Areas

Primary areas allowed for stages:

- src/**
- test/**
- deployment/**
- docs/** (minimal updates)

ReadOnly knowledge:

- information/**
- rules/**
- utilities/**

---

## 5. Stage Strategy

Planner MUST decompose delivery into ordered stages.

Each stage MUST:

- Have a dedicated mini plan
- Define a BranchName
- Produce coder tasks
- End with validate-stage.sh

Planner MUST NOT implement code directly.

---

## 6. Stage Order (Declarative)

Example:

- Stage-1: Domain foundation
- Stage-2: API layer
- Stage-3: Persistence
- Stage-4: Integration tests

Planner MAY refine stages but MUST preserve order.

---

## 7. Validation Model

A stage is considered complete when:

- All coder tasks are done
- validate-stage.sh succeeds
- complete-stage.sh merges into development
- Human promotes development → main

---

## 8. Non-Functional Guardrails

Planner MUST prioritize:

- Deterministic structure
- Small incremental stages
- Clear dependency ordering
- Minimal file surface per stage

---

## 9. Notes For Planner

Planner SHOULD:

- Generate mini plans under plan/stages/
- Use declarative language only
- Avoid narrative or long explanations