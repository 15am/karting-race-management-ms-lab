# Code Rules — Developers Execution Directives

These directives define HOW code MUST be generated.

Developers MUST prioritize architectural integrity over brevity or convenience.

---

# 0. Priority Matrix (Execution Order)

Priority 1 — Domain Modeling
Priority 2 — Object Calisthenics
Priority 3 — Control Flow & Result Pattern
Priority 4 — .NET Syntax & Clean Code
Priority 5 — Formatting

Higher priority rules ALWAYS override lower ones.

---

# 1. Domain Modeling Directives

## 1.1 Primitive Obsession (MANDATORY)

Developers MUST NEVER expose primitives in Domain Entities.

Wrap primitives using:

* readonly record struct Value Objects

Examples:

* EmailAddress
* ResourceId
* Money

---

## 1.2 State Constraint

A class MAY contain MAXIMUM two instance fields.

If more state exists:

* Extract a Value Object.

---

## 1.3 Tell, Don’t Ask

Domain objects expose behavior only.

Developers MUST:

* Avoid public setters.
* Avoid mutable properties.
* Use intention-revealing methods.

DTOs inside `Contracts` are allowed to have properties.

---

## 1.4 First-Class Collections

A class holding a collection MUST NOT contain other fields.

Example:

OrderLines instead of List<OrderLine.

---

## 1.5 Law of Demeter

Avoid deep chains:

Forbidden:
a.B.C.DoSomething()

Preferred:
a.PerformAction()

---

# 2. Object Calisthenics

## 2.1 One Indentation Level

Extract private methods when nesting appears.

---

## 2.2 Else Ban

Developers MUST NOT generate else statements.

Use guard clauses or polymorphism.

---

## 2.3 Small Types

* Max 50 lines per class
* Max 10 classes per namespace

---

## 2.4 Naming Rules

Never abbreviate identifiers.

Bad:
userRepo, idx

Good:
userRepository, index

---

# 3. Control Flow

## 3.1 Guard Clauses

Validate inputs at method start.

Fail fast.

---

## 3.2 Result Pattern

Business logic MUST return:

Result<T> or Option<T>

Exceptions only for infrastructure failures.

---

# 4. .NET 10 Directives

Developers MUST generate:

* File-scoped namespaces
* Primary constructors
* Target-typed new
* Records for immutable data

---

## 4.1 XML Documentation

All public members MUST include:

<summary>
<param>
<returns>

---

# 5. Clean Code Baseline

* SRP enforced.
* Short methods.
* Simple LINQ.
* Replace magic numbers.
* Strict layer separation:

BFF → Services → Domain → DAL

---

# 6. Anti-Patterns Developers MUST Avoid

Developers MUST NOT generate:

* Public mutable properties in Domain.
* Services containing business logic orchestration + validation + persistence.
* Classes with more than two fields.
* Deep object navigation.
* else statements.
* Primitive parameters in Domain constructors.
* Fat God Classes.
* Hidden business logic inside LINQ.

When uncertain:

1. Create a Value Object.
2. Extract a class.
3. Extract a method.
4. Reduce indentation.

END OF CODE RULES
