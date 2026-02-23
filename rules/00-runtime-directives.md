# Runtime Directives — Execution Strategy

These rules define HOW Developers must plan and execute code changes inside code repositories.

They DO NOT define architecture — they define Developers behavior during generation and refactoring.

Developers MUST follow these runtime directives before applying any modification.

---

# 1. Safe Modification Strategy

When modifying existing code, Developers MUST follow this order:

1. Preserve Domain Modeling rules first.
2. Reduce primitive usage.
3. Reduce indentation depth.
4. Extract Value Objects before modifying logic.
5. Extract private methods before adding new conditionals.

Developers MUST prefer incremental structural refactors over large rewrites.

---

# 2. Refactor Execution Model (MANDATORY)

When a change requires structural refactoring, Developers MUST execute steps in this sequence:

Step 1 — Introduce new Value Objects
Step 2 — Extract small cohesive methods
Step 3 — Reduce constructor parameters
Step 4 — Remove else branches
Step 5 — Split large classes

Developers MUST NOT attempt to solve everything in a single transformation.

---

# 3. Decision Heuristics

If Developers is uncertain how to implement a feature:

Prefer this order:

1. Introduce a Value Object.
2. Introduce a Domain Method.
3. Extract a First-Class Collection.
4. Introduce a small Domain Service.

Developers MUST avoid creating generic utility services as a first solution.

---

# 4. Class Growth Prevention

If a class approaches:

* 40 lines of real code
* or 2 fields

Developers SHOULD:

* Extract a Value Object.
* Extract a collaborator class.

Developers MUST avoid expanding existing entities.

---

# 5. Control Flow Safety

Developers MUST pause and restructure when detecting:

* Nested if statements
* else branches
* loops inside conditionals

Required action:

* Extract private method.
* Use guard clause.
* Apply polymorphism if possible.

---

# 6. Naming Stabilization Rule

During refactors:

Developers MUST NOT rename stable domain concepts unless explicitly requested.

Allowed:

* Method extraction
* Class splitting
* Value Object introduction

Forbidden without instruction:

* Renaming aggregates
* Renaming namespaces
* Large file moves

---

# 7. Repository Safety Mode

Developers MUST assume a conservative modification strategy:

* Avoid touching CI/CD files.
* Avoid editing Docker or infra.
* Avoid mass formatting.
* Branch creation MUST be done only via utilities/scripts/git-branch.sh.
* Models MUST NOT manually create branches.

Unless the request explicitly mentions infrastructure.

---

# 8. Generation Mindset (Meta Rule)

Developers MUST behave as:

"A senior engineer preserving architecture integrity."

This means:

* Prefer cohesion over cleverness.
* Prefer explicit domain modeling over shortcuts.
* Prefer readability over compact syntax.

When uncertain:

1. Extract.
2. Encapsulate.
3. Reduce nesting.
4. Introduce behavior into the domain.

---

END OF RUNTIME RULES
