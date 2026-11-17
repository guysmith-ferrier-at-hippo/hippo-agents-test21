---
id: KB-0001
title: Layer Interface Decoupling and Dependency Injection in .NET 10
component: architecture-core
domain: software-design
tags:
- dependency-injection
- interfaces
- layering
- dotnet-10
status: validated
created: '2026-03-24'
updated: '2026-03-24'
---

# Layer Interface Decoupling and Dependency Injection in .NET 10

## What Happened
In establishing the Customer Greeting Service architecture, we adopted a strict layered design where each layer exposes only interfaces and contracts (DTOs / models). Concrete implementations are internal or separated so that consuming layers never reference implementation classes directly.

## Key Points
- **API Layer (`CustomerGreetingService.Api`)**: References business logic interfaces (e.g., `IGreetingBusinessLogic`). Does not instantiate or reference repository interfaces directly.
- **Business Logic Layer (`CustomerGreetingService.BusinessLogic`)**: References repository interfaces (e.g., `IGreetingRepository`). Contains domain calculation rules for customer greetings and time-of-day logic.
- **Repository Layer (`CustomerGreetingService.Repositories`)**: Implements `IGreetingRepository` using in-memory data structures initially. Exposes models and interface definitions only.
- **Composition Root**: The API project acts as the composition root, wiring implementations to interfaces using `services.AddScoped<IInterface, Implementation>()` or appropriate lifetimes via constructor injection. Dedicated configuration classes for specific purposes are registered and injected directly rather than generic `IOptions<T>`.

## Why It Matters
This pattern prevents tight coupling, enforces testability via Moq, and enables replacing the in-memory repository with a database provider in a future release with zero changes to business logic or API controllers.

## Applicability
Applies to all past, current, and future service modules in this repository.
