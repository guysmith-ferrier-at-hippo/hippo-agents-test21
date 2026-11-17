---
id: DD-0001
title: OpenAPI (Swagger UI) as Developer Interface and Interactive Test Harness
component: api-layer
domain: api-design
tags:
- openapi
- swagger
- api-design
- developer-experience
status: accepted
created: '2026-03-24'
updated: '2026-03-24'
---

# OpenAPI (Swagger UI) as Developer Interface and Interactive Test Harness

## Context
The Customer Greeting Service is a headless REST API with no custom end-user graphic interface. Developers, testers, and integration partners require a clear, discoverable method to inspect API schema contracts and test endpoint execution interactively.

## Options Considered
1. **Interactive OpenAPI (Swagger UI)**: Integrated ASP.NET Core middleware hosting interactive API documentation and test harness at `/swagger`.
2. **Static Markdown Documentation Only**: Relying strictly on repository Markdown documentation without an interactive browser testing console.
3. **Dedicated Custom Web Dashboard**: Developing a standalone web frontend.

## Evaluation
Option 1 provides an industry-standard developer experience with zero custom frontend code maintenance. Option 2 lacks interactive testing capabilities. Option 3 introduces unnecessary maintenance complexity for a purely headless service.

## Decision
We select Option 1: Provide Swagger UI via OpenAPI middleware as the sole interactive developer testing harness for the service. The interface will expose all endpoint specifications, request parameters, response schemas, and error responses.

## Consequences
- **Positive**: Self-documenting API, interactive testing capability for all stakeholders, automated synchronization with API controllers.
- **Negative**: OpenAPI endpoints must be secured or evaluated before exposure in public production environments.
