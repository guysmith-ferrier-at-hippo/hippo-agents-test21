---
id: KB-0006
title: Symmetrical Test Structure and Business Logic Placement
component: project-structure
domain: software-architecture
tags:
- solution-structure
- testing
- layering
- code-review
status: validated
created: '2026-03-24'
updated: '2026-03-24'
depends_on:
- architecture.md
- decisions/ADR-0001-layered-architecture-interface-encapsulation.md
supported_by:
- specs/hello-world/tasks.md
---

## What Happened

Code review identified redundant API test projects and unnecessary folder nesting. `CustomerGreetingService.ApiTests` was removed. Business logic classes were moved from `Services/` to the project root.

## Key points

- Maintain exactly one test project for each source project.
- Use a symmetrical structure: `Api.UnitTests`, `BusinessLogic.UnitTests`, and `Repositories.UnitTests`.
- Consolidate controller unit tests and endpoint tests inside `Api.UnitTests`.
- Place business logic classes directly in the root of the BusinessLogic project, not in a `Services` subfolder.

## Why It Matters

Redundant test projects add maintenance overhead and increase CI build duration. Symmetrical project layout provides predictable file locations and simple navigation.

## Applies When

Applies when you create or organize projects and test suites across the solution.
