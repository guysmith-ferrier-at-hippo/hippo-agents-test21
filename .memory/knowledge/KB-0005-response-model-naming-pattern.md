---
id: KB-0005
title: Response Model Naming Pattern
component: api-and-domain-models
domain: software-design
tags:
- response-models
- naming-conventions
- code-review
- dotnet-10
status: validated
created: '2026-03-24'
updated: '2026-03-24'
depends_on:
- architecture.md
- decisions/ADR-0001-layered-architecture-interface-encapsulation.md
supported_by:
- specs/hello-world/proposal.md
---

## What Happened

Code review identified generic model naming for method responses. The response model `GreetingResponse` was renamed to `GetHelloWorldGreetingResponse`.

## Key points

- Name response models after the calling method.
- Append the suffix `Response` to the method name.
- For example, the method `GetHelloWorldGreeting` returns `GetHelloWorldGreetingResponse`.
- Apply this rule across all service contracts and API endpoints.

## Why It Matters

This pattern prevents ambiguous model reuse. It creates clear contracts for each method. It reduces breaking changes when endpoints evolve independently.

## Applies When

Applies when you create or refactor methods that return custom data models in contracts, services, or controllers.
