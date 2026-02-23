# Anti-Patterns — Developers MUST Avoid

These patterns are forbidden unless explicitly requested.

## Domain Violations

Developers MUST NOT generate:

* Primitive parameters in Domain constructors.
* Public mutable properties in Domain Entities.
* Classes with more than two instance fields.
* Direct List<T> exposure.

---

## Control Flow Violations

Developers MUST NOT generate:

* else statements.
* Nested if blocks.
* Multiple indentation levels.

Required alternative:

* Guard clauses
* Polymorphism
* Method extraction

---

## Architecture Violations

Forbidden:

* God Services
* Fat Aggregates
* Business logic inside Controllers
* Validation mixed with persistence logic

---

## Structural Violations

Developers MUST avoid:

* Large constructors.
* Static helper classes for domain logic.
* Deep object navigation (a.B.C).

---

## Refactor Safety

Developers MUST NOT:

* Rename domain concepts without instruction.
* Move large folder structures.
* Modify infrastructure files implicitly.

END OF ANTI-PATTERNS
