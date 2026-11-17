# Customer Greeting Service — Test Strategy

## Overview

The Customer Greeting Service test strategy ensures high reliability, deterministic greeting calculation, and strict interface adherence across all architectural layers. Testing encompasses unit tests using Visual Studio Unit Testing (MSTest) with Moq, integration tests via ASP.NET Core `WebApplicationFactory`, automated contract and security checks, and deployment verification in Azure Kubernetes Service (AKS) across Dev, Test, UAT, and Production environments.

## Test Environments & Access

- **Environments:**
  - **Local Development**: Developer workstations executing MSTest suites and local debugging against in-memory repository instances.
  - **CI (Azure DevOps)**: Automated container build, static analysis, unit test, and integration test execution on pull request.
  - **Dev & Test (AKS)**: Containerized deployment in development and test AKS namespaces for component and integration verification.
  - **UAT (AKS)**: Pre-production staging namespace for consumer integration verification and operational readiness testing.
  - **Production (AKS)**: Live operational cluster namespace with production health probe monitoring.
- **Access Needs:**
  - Azure DevOps service connection with permissions to push container images to Azure Container Registry (ACR) and deploy to AKS namespaces.
  - Test client certificates / API keys if authentication is enabled in future phases.
- **Data Management:**
  - Test datasets use isolated in-memory fixture initializers for each test method.
  - Tests do not share mutable static state across parallel execution threads.
- **CI/CD Integration:**
  - Pull requests trigger automated `dotnet test` with code coverage collection.
  - Pipeline gates enforce zero test failures and zero critical security vulnerabilities before publishing Helm release charts to AKS.

## Testing Approach

| Level | Scope / Objective | Tools / Frameworks | Success Criteria | Responsibilities |
|---|---|---|---|---|
| **Unit** | Business logic calculations, time-of-day interval parsing, controller action logic in strict isolation. | Visual Studio Unit Testing (MSTest), Moq (no fluent assertion APIs) | 85%+ branch coverage, 100% pass rate in CI | Devs |
| **Component (Web)** | OpenAPI test harness generation and schema definition accuracy. | Microsoft.AspNetCore.OpenApi, Swashbuckle CLI | OpenAPI document passes JSON schema validation | Devs |
| **Contract** | Verify request parameters and greeting response JSON contract compliance. | MSTest, JSON Schema Validator | Strict contract schema adherence, backward compatibility | Devs, QA |
| **Integration (Mocked)** | Interaction between API controllers and Business layer with mocked repository abstractions. | MSTest, Moq, `WebApplicationFactory` | All mocked repository edge cases handled with expected HTTP codes | Devs, QA |
| **Integration (API / Events)** | End-to-end HTTP pipeline tests against in-memory host. | `WebApplicationFactory<Program>`, `HttpClient` | All endpoints return valid status codes (200 OK, 400 Bad Request, 404 Not Found) | Devs, QA |
| **Acceptance (Functional)** | Verification of time-of-day greeting generation rules against specified business criteria. | MSTest Functional Test Suite | All functional greeting scenarios confirmed across all time intervals | QA, Devs |
| **End-to-End (E2E)** | Live service verification deployed inside AKS test namespace. | Azure DevOps Pipeline REST task / PowerShell test runner | Liveness and readiness endpoints return 200 OK; sample greeting returns expected JSON payload | QA, DevOps |
| **Performance (API)** | Benchmark API latency and throughput under concurrent request load. | k6 / Azure Load Testing | p95 latency < 100ms under standard load profile | QA, DevOps |
| **Performance (Web)** | OpenAPI Swagger UI page asset size and initial render latency. | Lighthouse CI / Network profiling | Swagger UI bundle loads within 1.5s on desktop | Devs |
| **Security** | Dependency vulnerability scanning, container image scanning, SAST. | Trivy, Microsoft Defender for DevOps, SonarQube | 0 Critical or High severity vulnerabilities | Sec, DevOps |
| **Accessibility** | Interactive OpenAPI / Swagger UI compliance with WCAG guidelines. | axe-core automated CLI | WCAG 2.1 Level AA compliant for developer test harness | Devs, QA |

## Risk & Mitigation

| Risk | Mitigation |
|---|---|
| In-memory repository state leakage across tests | Instantiate new repository instances per test method; avoid shared singleton in-memory state. |
| Time-of-day test flakiness due to system clock changes | Inject an `ISystemClock` or `TimeProvider` abstraction through constructor injection into `GreetingBusinessLogic`, enabling deterministic time mocking in unit tests. |
| Test assertion ambiguity without fluent APIs | Use explicit MSTest assertion methods (`Assert.AreEqual(expected, actual)`, `Assert.IsTrue(condition)`) with clear explanatory failure messages. |
| Ephemeral state loss in AKS pod restarts | Ensure tests validate that service logic behaves statelessly without assumption of cached session continuity across requests. |

## Reviewed by

| Name | Role | Approved | Date |
|---|---|---|---|
| Lead Test Engineer | Test Lead | [X] | 2026-03-24 |
| Solutions Architect | Tech Lead | [X] | 2026-03-24 |
| Product Owner | Product Manager | [X] | 2026-03-24 |
