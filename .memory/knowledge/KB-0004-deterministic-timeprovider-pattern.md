---
id: KB-0004
title: Deterministic Temporal Operations Using .NET TimeProvider
component: core
domain: testing-and-architecture
tags:
- time-provider
- dotnet-10
- testing
- deterministic-time
status: validated
created: '2026-03-24'
updated: '2026-03-24'
depends_on:
- decisions/ADR-0005-time-provider-abstraction.md
supported_by:
- specs/hello-world/proposal.md
---

# Deterministic Temporal Operations Using .NET TimeProvider

## What Happened
In designing the hello-world endpoint and time-of-day greeting operations, direct access to static system clocks (`DateTime.UtcNow`) was evaluated. Direct static clock access prevents test deterministic control and causes test flakiness. We established `System.TimeProvider` as the required abstraction for all temporal operations across domain and API layers.

## Key Points
- **Standard Abstraction**: Use the native `System.TimeProvider` abstract class rather than custom clock interfaces or static clock methods.
- **Dependency Injection**: Register `TimeProvider.System` as a singleton in `Program.cs`. Pass `TimeProvider` to business logic services via constructor injection.
- **Timestamp Formatting**: Format UTC timestamps using ISO-8601 round-trip string representations (`O` format specifier, e.g., `yyyy-MM-ddTHH:mm:ss.fffffffZ`).
- **Test Automation**: In MSTest suites, mock `TimeProvider` or supply a fixed time provider to assert exact timestamp equality without `Thread.Sleep` calls.

## Why It Matters
This pattern eliminates flaky tests and race conditions in CI/CD pipelines. It ensures predictable greeting delivery across diverse execution environments.

## Applicability
Applies to all endpoints, services, and repositories in `CustomerGreetingService` that inspect current time or generate timestamps.