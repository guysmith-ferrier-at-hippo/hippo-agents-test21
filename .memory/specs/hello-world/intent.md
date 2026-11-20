---
title: "Intent: Hello World Greeting Endpoint"
feature: "hello-world"
created: 2026-03-24
updated: 2026-03-24
depends-on:
  - "specs/hello-world/proposal.md"
  - "specs/hello-world/default-greeting-spec.md"
  - "specs/hello-world/parameterized-greeting-spec.md"
supported-by:
  - "specs/hello-world/tasks.md"
---

# Intent: Hello World Greeting Endpoint

## Objective

Deliver an HTTP GET endpoint at `/api/v1/hello-world` that returns a structured JSON greeting. Consuming applications and integration developers use this endpoint as a lightweight mechanism to verify network connectivity and JSON deserialization.

## Boundaries & Scope Constraints

- **Untouchable:** Core architectural layers must adhere strictly to ADR-0001. Business logic must not take direct dependencies on ASP.NET Core web host types.
- **Scope Limits:** Do not add authentication, authorization handlers, persistent database connections, or time-of-day interval logic. Do not create graphic user interface screens beyond Swagger UI.

## Technical Constraints

- **Dependencies:** .NET 10, C# 13, ASP.NET Core Web API, Swashbuckle.AspNetCore (OpenAPI), MSTest, Moq. No third-party assertion libraries (ADR-0004).
- **Performance:** Endpoint latency must remain under 100 milliseconds for 95% of requests under standard test load.
- **Compatibility:** Response payload timestamps must adhere strictly to ISO-8601 UTC format. Errors must follow RFC 7807 Problem Details.

## Data Definitions & Schemas

### Database Schema Changes

- **New Tables / Collections:** None. The endpoint operates statelessly without database storage (ASSUME-0003).
- **Modified Tables / Collections:** None.
- **Data Constraints:** None.

### Code Data Structures

- **Domain Models & Entities:**
  No persistent entity required. Core domain models reside in `src/CustomerGreetingService.BusinessLogic/Models/GreetingResponse.cs (new)`.
- **DTOs (Data Transfer Objects):**
  - `GreetingResponse`:
    ```csharp
    namespace CustomerGreetingService.BusinessLogic.Models;

    public sealed class GreetingResponse
    {
        public required string Message { get; init; }
        public required string Timestamp { get; init; }
    }
    ```
- **Internal Data Flows & State Objects:**
  - `IGreetingBusinessLogic` contract in `src/CustomerGreetingService.BusinessLogic/Interfaces/IGreetingBusinessLogic.cs (new)`:
    ```csharp
    namespace CustomerGreetingService.BusinessLogic.Interfaces;

    public interface IGreetingBusinessLogic
    {
        GreetingResponse GetHelloWorldGreeting(string? name);
    }
    ```
- **Type/Schema Verification:**
  - Verified by compiler type checks in `src/CustomerGreetingService.BusinessLogic/CustomerGreetingService.BusinessLogic.csproj (new)` and `src/CustomerGreetingService.Api/CustomerGreetingService.Api.csproj (new)`.

## API & Integration Contracts

### Endpoints (REST / RPC / GraphQL)

- **Endpoint Definitions:**
  | Method | Path / Operation | Description | Changes data | Auth Required |
  | :--- | :--- | :--- | :--- | :--- |
  | `GET` | `/api/v1/hello-world` | Returns default or personalized greeting payload | No | No |
  | `POST` | `/api/v1/hello-world` | Rejected with Method Not Allowed | No | No |
  | `PUT` | `/api/v1/hello-world` | Rejected with Method Not Allowed | No | No |
  | `DELETE` | `/api/v1/hello-world` | Rejected with Method Not Allowed | No | No |

- **Rule:** A `GET` never changes data. Use `POST`, `PUT`, `PATCH` or `DELETE` for anything that changes data or records an action.
- **Headers & Query Parameters:**
  - Query Parameter `name` (optional): Recipient name string, trimmed of surrounding whitespace, maximum length 50 characters.
  - Header `Accept`: Defaults to `application/json`.
  - Header `Content-Type`: `application/json` returned on success.

### Payload Contracts

- **Request Payloads:**
  No request body. Parameters supplied via optional URL query string:
  `GET /api/v1/hello-world?name={name}`

- **Response Payloads (200 OK):**
  ```json
  {
    "message": "Hello, World!",
    "timestamp": "2026-03-24T12:00:00.0000000Z"
  }
  ```
  Personalized response example:
  ```json
  {
    "message": "Hello, Alice!",
    "timestamp": "2026-03-24T12:00:00.0000000Z"
  }
  ```

- **Error Responses (400 Bad Request):**
  RFC 7807 Problem Details payload:
  ```json
  {
    "type": "https://tools.ietf.org/html/rfc7807",
    "title": "Bad Request",
    "status": 400,
    "detail": "Recipient name must not exceed 50 characters."
  }
  ```

- **Error Responses (405 Method Not Allowed):**
  RFC 7807 Problem Details payload:
  ```json
  {
    "type": "https://tools.ietf.org/html/rfc7807",
    "title": "Method Not Allowed",
    "status": 405,
    "detail": "HTTP method not supported on this route."
  }
  ```

### Webhooks & Messaging

- **Publish/Subscribe Event Schemas:** None.
- **Third-Party Integrations:** None.

## Validation Rules

### Test Sets (Input/Output Rulesets)

- Given no query parameters, when calling `GET /api/v1/hello-world`, then return 200 OK with `message="Hello, World!"`.
- Given `name="Alice"`, when calling `GET /api/v1/hello-world?name=Alice`, then return 200 OK with `message="Hello, Alice!"`.
- Given `name=""` or `name="   "`, when calling `GET /api/v1/hello-world?name=   `, then return 200 OK with `message="Hello, World!"`.
- Given `name="  Bob  "`, when calling `GET /api/v1/hello-world?name=%20Bob%20`, then return 200 OK with `message="Hello, Bob!"`.
- Given `name` containing 51 characters, when calling `GET /api/v1/hello-world`, then return 400 Bad Request with RFC 7807 Problem Details.
- Given `name` containing ASCII control character `\0`, when calling `GET /api/v1/hello-world`, then return 400 Bad Request.
- Given an HTTP POST request to `/api/v1/hello-world`, then return 405 Method Not Allowed.

### Properties (Universally Quantifiable Predicates)

- For all valid responses, the `timestamp` property must parse successfully as an ISO-8601 UTC timestamp.
- For all valid responses, the `message` property must start with the prefix `"Hello, "`.
- For all valid names with length between 1 and 50 characters, the greeting message equals `"Hello, " + name.Trim() + "!"`.

### Contracts (Preconditions, Invariants, Postconditions)

- **Precondition:** `name` must not exceed 50 characters and must not contain ASCII control characters (0x00-0x1F, 0x7F).
- **Invariant:** The service state remains unchanged across calls. No database or disk writes occur.
- **Postcondition:** The response payload contains `message` and `timestamp` formatted as camelCase JSON properties.

## Measurable Success Criteria

- **SC-001:** 100% of valid GET requests return HTTP status 200 OK.
- **SC-002:** 100% of successful responses include `message` and `timestamp` fields.
- **SC-003:** Swagger UI is accessible at `/swagger` and documents the `/api/v1/hello-world` endpoint.
- **SC-004:** Unit and integration test coverage across new components meets or exceeds 85%.
- **SC-005:** Zero linting, compiler warnings, or build errors across all projects.

## Implementation Directives

- Run `dotnet build` with zero warnings after creating each project.
- Implement tests using standard MSTest assertions (`Assert.AreEqual`, `Assert.IsNotNull`). Do not use fluent assertion libraries.
- Inject `TimeProvider` into `GreetingBusinessLogic` via constructor injection. Use `TimeProvider.System` for production runtime.
- Verify that Swagger UI at `/swagger` renders the endpoint parameters and response schemas.
