---
title: "Implementation Tasks: Hello World Greeting Endpoint"
component: "api-layer"
domain: "customer-greeting"
tags:
  - "hello-world"
  - "tasks"
  - "dotnet-10"
  - "mstest"
created: 2026-03-24
updated: 2026-03-24
depends-on:
  - "specs/hello-world/intent.md"
supported-by:
  - "specs/hello-world/proposal.md"
---

## 1. PR 1: Solution Setup and Domain Contracts

- [x] 1.1 Create solution file in `CustomerGreetingService.sln (new)`.
- [x] 1.2 Create the business logic project in `src/CustomerGreetingService.BusinessLogic/CustomerGreetingService.BusinessLogic.csproj (new)`.
- [x] 1.3 Create `GreetingResponse` model in `src/CustomerGreetingService.BusinessLogic/Models/GreetingResponse.cs (new)`.
- [x] 1.4 Create `IGreetingBusinessLogic` interface contract in `src/CustomerGreetingService.BusinessLogic/Interfaces/IGreetingBusinessLogic.cs (new)`.
- [x] 1.5 Implement `GreetingBusinessLogic` class with constructor injection for `TimeProvider` in `src/CustomerGreetingService.BusinessLogic/Services/GreetingBusinessLogic.cs (new)`.
- [x] 1.6 Implement whitespace trimming and 50-character input validation in `src/CustomerGreetingService.BusinessLogic/Services/GreetingBusinessLogic.cs (new)`.
- [x] 1.7 Implement ASCII control character rejection throwing an `ArgumentException` in `src/CustomerGreetingService.BusinessLogic/Services/GreetingBusinessLogic.cs (new)`.

## 2. PR 1: Business Logic Unit Test Implementation

- [x] 2.1 Create MSTest project in `tests/CustomerGreetingService.BusinessLogic.UnitTests/CustomerGreetingService.BusinessLogic.UnitTests.csproj (new)`.
- [x] 2.2 Using `write-tests`, implement TC-01 and TC-14 in `tests/CustomerGreetingService.BusinessLogic.UnitTests/Services/GreetingBusinessLogicTests.cs (new)`.
- [x] 2.3 Using `write-tests`, implement TC-02 and TC-03 for personalized and Unicode names in `tests/CustomerGreetingService.BusinessLogic.UnitTests/Services/GreetingBusinessLogicTests.cs (new)`.
- [x] 2.4 Using `write-tests`, implement TC-10, TC-11, and TC-12 for empty and whitespace inputs in `tests/CustomerGreetingService.BusinessLogic.UnitTests/Services/GreetingBusinessLogicTests.cs (new)`.
- [x] 2.5 Using `write-tests`, implement TC-05, TC-06, and TC-13 for boundary length and control characters in `tests/CustomerGreetingService.BusinessLogic.UnitTests/Services/GreetingBusinessLogicTests.cs (new)`.
- [x] 2.6 Run `dotnet test` on the business logic test project and verify that all unit tests pass.

## 3. PR 2: API Layer and Swagger Configuration

- [x] 3.1 Create the API project in `src/CustomerGreetingService.Api/CustomerGreetingService.Api.csproj (new)`.
- [x] 3.2 Add project reference to `src/CustomerGreetingService.BusinessLogic/CustomerGreetingService.BusinessLogic.csproj (new)`.
- [x] 3.3 Configure `Program.cs (new)` to register `TimeProvider.System` and `IGreetingBusinessLogic` in dependency injection.
- [x] 3.4 Configure OpenAPI and Swagger UI middleware at `/swagger` in `src/CustomerGreetingService.Api/Program.cs (new)`.
- [x] 3.5 Configure RFC 7807 Problem Details service in `src/CustomerGreetingService.Api/Program.cs (new)`.
- [x] 3.6 Create `HelloWorldController` with `GET /api/v1/hello-world` in `src/CustomerGreetingService.Api/Controllers/HelloWorldController.cs (new)`.
- [x] 3.7 Add input exception handling and RFC 7807 Bad Request mapping in `src/CustomerGreetingService.Api/Controllers/HelloWorldController.cs (new)`.
- [x] 3.8 Enforce HTTP GET method restriction and return HTTP 405 for non-GET methods in `src/CustomerGreetingService.Api/Controllers/HelloWorldController.cs (new)`.

## 4. PR 2: API Controller Unit Test Implementation

- [x] 4.1 Create MSTest project in `tests/CustomerGreetingService.Api.UnitTests/CustomerGreetingService.Api.UnitTests.csproj (new)`.
- [x] 4.2 Using `write-tests`, implement controller unit tests mocking `IGreetingBusinessLogic` in `tests/CustomerGreetingService.Api.UnitTests/Controllers/HelloWorldControllerTests.cs (new)`.
- [x] 4.3 Using `write-tests`, verify controller returns 200 OK with `GreetingResponse` in `tests/CustomerGreetingService.Api.UnitTests/Controllers/HelloWorldControllerTests.cs (new)`.
- [x] 4.4 Using `write-tests`, verify controller catches `ArgumentException` and returns 400 Problem Details in `tests/CustomerGreetingService.Api.UnitTests/Controllers/HelloWorldControllerTests.cs (new)`.
- [x] 4.5 Run `dotnet test` on the API unit test project and verify that all unit tests pass.

## 5. PR 2: API Integration and Contract Test Implementation

- [x] 5.1 Create integration test project in `tests/CustomerGreetingService.ApiTests/CustomerGreetingService.ApiTests.csproj (new)`.
- [x] 5.2 Add `Microsoft.AspNetCore.Mvc.Testing` package reference to `tests/CustomerGreetingService.ApiTests/CustomerGreetingService.ApiTests.csproj (new)`.
- [x] 5.3 Using `write-tests`, implement TC-01, TC-02, and TC-03 using `WebApplicationFactory` in `tests/CustomerGreetingService.ApiTests/Controllers/HelloWorldEndpointTests.cs (new)`.
- [x] 5.4 Using `write-tests`, implement TC-05 and TC-06 for HTTP 400 Problem Details in `tests/CustomerGreetingService.ApiTests/Controllers/HelloWorldEndpointTests.cs (new)`.
- [x] 5.5 Using `write-tests`, implement TC-07, TC-08, and TC-09 for HTTP 405 Method Not Allowed in `tests/CustomerGreetingService.ApiTests/Controllers/HelloWorldEndpointTests.cs (new)`.
- [x] 5.6 Using `write-tests`, implement TC-04 to verify OpenAPI documentation endpoint in `tests/CustomerGreetingService.ApiTests/Controllers/HelloWorldEndpointTests.cs (new)`.
- [x] 5.7 Run `dotnet test` across the entire solution and verify that all integration tests pass.

## 6. PR 2: Verification and Quality Gate Inspection

- [x] 6.1 Execute `dotnet build --configuration Release` and confirm zero compiler warnings.
- [x] 6.2 Execute `dotnet test` with code coverage and confirm coverage meets or exceeds 85%.
- [x] 6.3 Verify endpoint in interactive Swagger UI at `/swagger` using local web server execution.
- [x] 6.4 Validate all remediation tasks in `.memory/specs/hello-world/security.md`.
