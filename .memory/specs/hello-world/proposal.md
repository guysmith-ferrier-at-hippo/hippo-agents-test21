---
title: "Implementation Proposal: Hello World Greeting Endpoint"
component: "api-layer"
domain: "customer-greeting"
tags:
  - "hello-world"
  - "api"
  - "openapi"
  - "dotnet-10"
created: 2026-03-24
updated: 2026-03-24
depends-on:
  - "specs/hello-world/feature.md"
  - "specs/hello-world/default-greeting-spec.md"
  - "specs/hello-world/parameterized-greeting-spec.md"
supported-by:
  - "specs/hello-world/intent.md"
  - "specs/hello-world/tasks.md"
  - "specs/hello-world/test-cases.md"
  - "specs/hello-world/security.md"
---

# Implementation Proposal: Hello World Greeting Endpoint

## Intent

The Customer Greeting Service requires a lightweight connectivity endpoint. Consuming applications use this endpoint to verify network connectivity, latency, and JSON deserialization. This implementation provides the `GET /api/v1/hello-world` endpoint with default and parameterized greeting support.

## Scope

### In-Scope

- Create solution and project structure in `CustomerGreetingService.sln (new)`.
- Define the `GreetingResponse` data transfer object in `src/CustomerGreetingService.BusinessLogic/Models/GreetingResponse.cs (new)`.
- Define the `IGreetingBusinessLogic` interface contract in `src/CustomerGreetingService.BusinessLogic/Interfaces/IGreetingBusinessLogic.cs (new)`.
- Implement greeting logic and validation in `src/CustomerGreetingService.BusinessLogic/Services/GreetingBusinessLogic.cs (new)`.
- Register dependencies and OpenAPI middleware in `src/CustomerGreetingService.Api/Program.cs (new)`.
- Implement the controller endpoint in `src/CustomerGreetingService.Api/Controllers/HelloWorldController.cs (new)`.
- Support an optional `name` query parameter with whitespace trimming and a 50-character limit.
- Reject ASCII control characters with HTTP status code 400 using RFC 7807 Problem Details.
- Enforce HTTP GET method restriction and return HTTP status code 405 for other HTTP methods.
- Expose the endpoint and schema models in Swagger UI at `/swagger`.
- Implement MSTest unit tests in `tests/CustomerGreetingService.BusinessLogic.UnitTests/Services/GreetingBusinessLogicTests.cs (new)`.
- Implement MSTest unit tests in `tests/CustomerGreetingService.Api.UnitTests/Controllers/HelloWorldControllerTests.cs (new)`.
- Implement integration tests using `WebApplicationFactory` in `tests/CustomerGreetingService.ApiTests/Controllers/HelloWorldEndpointTests.cs (new)`.

### Out-of-Scope

- User authentication and authorization tokens.
- Persistent database storage or external database connections.
- Time-of-day greeting variations (reserved for future feature iterations).
- Custom graphic user interface frontend.

## Knowledge Context

### Assumptions

- [ASSUME-0001](assumptions/ASSUME-0001-in-memory-storage-interim.md): In-Memory Repository Implementation as Interim Storage.
- [ASSUME-0002](assumptions/ASSUME-0002-hello-world-connectivity-verification.md): Hello World Endpoint Serves as Baseline Connectivity Probe.
- [ASSUME-0003](assumptions/ASSUME-0003-hello-world-stateless-unauthenticated.md): Hello World Endpoint Operates Without External Persistence or Authentication.
- [ASSUME-0004](assumptions/ASSUME-0004-name-validation-control-characters.md): Unicode Recipient Names and ASCII Control Character Validation.

### Architectural Decisions (ADRs)

- [ADR-0001](decisions/ADR-0001-layered-architecture-interface-encapsulation.md): Layered Architecture with Interface Encapsulation.
- [ADR-0002](decisions/ADR-0002-in-memory-repository-pattern.md): In-Memory Repository Pattern for Interim Data Access.
- [ADR-0003](decisions/ADR-0003-aks-hosting-azure-devops.md): Hosting on Azure Kubernetes Service (AKS) Deployed via Azure DevOps.
- [ADR-0004](decisions/ADR-0004-mstest-moq-testing.md): Standardization on MSTest and Moq Without Fluent Assertion APIs.
- [ADR-0005](decisions/ADR-0005-time-provider-abstraction.md): Adoption of TimeProvider for Temporal Operations.
- [DD-0001](decisions/DD-0001-openapi-test-harness.md): OpenAPI (Swagger UI) as Developer Interface and Interactive Test Harness.

### Security Context

| Attribute | Value |
|---|---|
| Data Sensitivity | Public |
| Authentication | None (Open Endpoint) |
| Authorization | None |
| Network Ingress | HTTPS (TLS 1.3) via AKS Ingress |
| Input Validation | Query parameter length <= 50, ASCII control character rejection |

## Approach

The implementation adheres to the 3-tier layered architecture specified in ADR-0001:

1. **Business Logic Layer**:
   - `IGreetingBusinessLogic` exposes method `GreetingResponse GetHelloWorldGreeting(string? name)`.
   - `GreetingBusinessLogic` receives `TimeProvider` via constructor injection (ADR-0005).
   - If `name` is null, empty, or whitespace, the service generates the default greeting `"Hello, World!"`.
   - If `name` contains ASCII control characters (0x00-0x1F, 0x7F) or exceeds 50 characters, the service throws an `ArgumentException`.
   - The service formats the current UTC time as an ISO-8601 string (`yyyy-MM-ddTHH:mm:ss.fffffffZ`).

2. **API Layer**:
   - `HelloWorldController` handles `GET /api/v1/hello-world`.
   - The controller catches validation exceptions and returns RFC 7807 `ProblemDetails` with HTTP status 400.
   - Non-GET requests produce HTTP status 405 Method Not Allowed.
   - `Program.cs` registers `TimeProvider.System` and `IGreetingBusinessLogic` in the dependency injection container.
   - Swashbuckle OpenAPI middleware exposes Swagger UI at `/swagger` (DD-0001).

3. **Testing Strategy**:
   - Business logic unit tests verify default greeting, parameter trimming, Unicode names, and validation rules.
   - Controller unit tests verify HTTP status codes and contract mapping in isolation using Moq.
   - Integration tests execute real HTTP requests against `WebApplicationFactory<Program>` to verify end-to-end routing.
