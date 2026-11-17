---
id: ADR-0005
title: Adoption of TimeProvider for Temporal Operations
component: architecture-core
domain: architecture
tags:
- time-provider
- dotnet-10
- testing
- architecture
status: accepted
created: '2026-03-24'
updated: '2026-03-24'
depends_on:
- architecture.md
- specs/hello-world/feature.md
supported_by:
- specs/hello-world/proposal.md
---

# Decision Record: ADR-0005. Adoption of TimeProvider for Temporal Operations

**Feature:** hello-world  
**Date:** 2026-03-24  
**Deciders:** Solutions Architect, Engineering Team  
**Status:** Accepted  

## Context

The Customer Greeting Service generates ISO-8601 UTC timestamps in greeting responses. Future features also calculate greetings based on time of day. Direct calls to `DateTime.UtcNow` create non-deterministic behaviors and prevent isolated unit testing. The system requires an injectable abstraction for system time across the application layers.

## Options Considered

* **Option 1:** Use the built-in .NET `System.TimeProvider` abstract class.
* **Option 2:** Define a custom interface such as `ISystemClock` or `ITimeService`.
* **Option 3:** Access static `DateTimeOffset.UtcNow` directly in business logic.

## Evaluation

### Option 1: System.TimeProvider
* **Pros:**
    * Native to .NET 10 without external dependencies.
    * Supports deterministic testing via constructor injection.
    * Standardizes time operations across all project layers.
* **Cons:**
    * Requires registration of `TimeProvider.System` in the service container.

### Option 2: Custom ISystemClock Interface
* **Pros:**
    * Full control over interface methods.
* **Cons:**
    * Introduces custom boilerplate code that duplicates native framework capabilities.

### Option 3: Static DateTimeOffset.UtcNow
* **Pros:**
    * Simple immediate implementation.
* **Cons:**
    * Cannot mock time in automated unit tests.
    * Causes flaky test runs.

## Decision

We select Option 1: Adopt `System.TimeProvider` as the standard time abstraction for all components. Register `TimeProvider.System` as a singleton in the dependency injection container. Inject `TimeProvider` into business logic classes via constructor injection.

## Consequences

### Positive
* Automated unit tests control time deterministically without thread sleep calls.
* Consistent timestamp formatting across all service endpoints.
* Follows Microsoft best practices for modern .NET development.

### Negative
* Developers must inject `TimeProvider` into every component that uses system time.
