---
id: ADR-0002
title: In-Memory Repository Pattern for Interim Data Access
component: repository-layer
domain: data-storage
tags:
- in-memory
- repository-pattern
- dotnet-10
status: accepted
created: '2026-03-24'
updated: '2026-03-24'
---

# In-Memory Repository Pattern for Interim Data Access

## Context
The Customer Greeting Service requires a data access layer to retrieve greeting templates and customer preferences. However, external database infrastructure is not required during the initial development and proof-of-concept stages.

## Decision
We will implement an in-memory repository behind the `IRepository` interface abstractions for the initial development and test cycles. Persistent storage (such as Azure SQL or PostgreSQL) will be deferred to a later milestone. The repository contracts will remain clean and independent of in-memory implementation specifics to allow seamless replacement by a durable database adapter in future phases.

## Consequences
- **Positive**: Rapid local development, zero database infrastructure provisioning overhead, fast automated test execution.
- **Negative**: Data does not persist across application restarts or container pod rescheduling in AKS. Not suitable for multi-replica horizontal scaling with shared state.
- **Neutral**: Requires clear documentation and consumer expectations that state is ephemeral until permanent persistence is introduced.

## Options Considered
- **Direct Database Integration (e.g. EF Core with SQL Server)**: Rejected for phase 1 due to unnecessary infrastructure setup and management complexity before schema stabilization.
- **Local SQLite / File Storage**: Rejected because in-memory collections satisfy initial stateless greeting requirements without file system I/O.
