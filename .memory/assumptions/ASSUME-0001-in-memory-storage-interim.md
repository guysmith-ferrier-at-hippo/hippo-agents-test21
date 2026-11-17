---
id: ASSUME-0001
title: In-Memory Repository Implementation as Interim Storage
component: repository-layer
domain: data-storage
tags:
- storage
- in-memory
- repository
status: unresolved
created: '2026-03-24'
updated: '2026-03-24'
---

## Description
The service uses an in-memory repository implementation during initial development and early environments (Dev and Test). Persistent storage (such as a relational or document database) is out of scope for the initial release and will be implemented in a subsequent phase.

## Rationale
Using an in-memory repository allows rapid development and validation of customer greeting business logic and REST API contracts without external database dependencies or provisioning overhead.

## Impact If Wrong
If persistent storage is required immediately by production consumers, customer data or customized greetings will be lost upon container restart in Azure Kubernetes Service (AKS).

## Validation Method
Validate against consumer requirements prior to UAT and Production deployment. Implement a persistent repository layer when durable state requirements are finalized.
