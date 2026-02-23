# Repository Rules — Developers Workflow Directives

These directives define HOW Developers interacts with repositories.

---

# 1. GitFlow Strategy (MANDATORY)

All branches MUST originate from:

development

Developers MUST NEVER assume main as base branch.

Allowed prefixes:

feature/name
fix/name
refactor/name
infra/name
chore/name

Branch names must be descriptive and technical.
Do NOT use task numbers in branch names.

---

# 2. Conventional Commits

Format:

type(scope): subject

Rules:

* Max 50 chars.
* Imperative mood.
* No trailing period.

Body MUST include:

* WHY the change exists
* WHAT changed
* Which rule or directive was applied

Example:

refactor(common): introduce EmailAddress value object

* Removes primitive string usage
* Applies Primitive Obsession directive
* Adds guard clauses

---

# 3. Pull Request Expectations

Developers SHOULD:

* Prefer small cohesive commits.
* Avoid mixing infra + domain changes.
* Refactor when touching code (Boy Scout Rule).

---

# 4. Testing Strategy

Each project MUST have:

tests/ProjectName.UnitTests

---

## 4.1 Unit Test Constraints

Developers MUST generate tests that:

* Do not call databases.
* Do not call external services.
* Mock infrastructure dependencies.

---

## 4.2 Test Naming

MethodName_StateUnderTest_ExpectedBehavior

Example:

Create_InvalidEmail_ReturnsFailure

---

# 5. Repository Safety Rules

Developers MUST NOT:

* Rename large folders without explicit instruction.
* Modify CI/CD unless request mentions infra.
* Introduce breaking changes silently.


# 6. Task Folder Behavior

Developers MUST follow this lifecycle:

backlog → active → done
            ↓
          blocked

Rules:

- New tasks are created in `.tasks/backlog/`.
- Tasks ready for execution move to `.tasks/active/`.
- Completed tasks move to `.tasks/done/`.
- Blocked tasks move to `.tasks/blocked/`.

Tasks MUST NOT be deleted.

# 7. CI Workflow Directive

When a repository contains .NET projects:

- Developer SHOULD add Gitea workflows from workspace templates.
- Libraries MUST include NuGet release workflow on main branch.
- Development branch MUST run build and tests.
- Branch name MUST be declared inside the Mini Plan.
- Coder must NEVER invent branch names.
- Only git-branch.sh may create branches.

END OF REPOSITORY RULES
