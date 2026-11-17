---
id: ADR-0001
title: Layered Architecture with Interface Encapsulation and Constructor Dependency
  Injection
component: architecture-core
domain: architecture
tags:
- layered-architecture
- dependency-injection
- csharp
- dotnet-10
status: accepted
created: '2026-03-24'
updated: '2026-03-24'
---

# Layered Architecture with Interface Encapsulation and Constructor Dependency Injection

## Context
The Customer Greeting Service requires a clean separation of concerns among the HTTP API endpoints, the core business rules for greetings, and the data access mechanisms. To maintain maintainability, prevent tight coupling, and allow independent evolution or replacement of components (such as moving from in-memory to persistent storage), layers must not depend directly on concrete implementations.

## Decision
We will organize the application into three distinct layers:
1. **API Layer**: ASP.NET Core controllers and OpenAPI test harness. Exposes HTTP endpoints.
2. **Business Logic Layer**: Domain logic, greeting rule calculations, and domain services.
3. **Repository Layer**: Data retrieval and management, initially implemented in-memory.

Each layer will expose interfaces and contract models only. Implementations must not be directly consumed or referenced by consuming layers. Dependencies between layers will be resolved exclusively through Constructor Dependency Injection via the built-in Microsoft.Extensions.DependencyInjection container in .NET 10.

## Consequences
- **Positive**: Strict layer isolation, high testability using interface mocking (Moq), and the ability to replace repository implementations without altering business logic.
- **Negative**: Requires additional boilerplate code to define interfaces, data transfer objects, and contract mappings between layers.
- **Neutral**: All developers must follow interface-driven development discipline.

## Options Considered
- **Vertical Slice Architecture**: Grouping code by feature rather than layer. Rejected because a 3-tier layered architecture fits the team's governance model and modular separation needs for interface encapsulation.
- **Minimal APIs without Layering**: Implementing endpoints and logic directly within minimal API route handlers. Rejected to prevent business logic and data access coupling in endpoint definitions.
