# Test Cases: Hello World Greeting Endpoint

## 🟢 Happy Path Scenarios

### TC-01: Default Hello World Greeting Request
- **Pre-conditions**: The service is running and healthy.
- **Steps**:
  1. Send an HTTP GET request to `/api/v1/hello-world` without query parameters.
- **Expected Result**: The server returns HTTP 200 OK. The JSON body contains `message: "Hello, World!"` and a valid ISO-8601 UTC `timestamp`.

### TC-02: Parameterized Greeting with Recipient Name
- **Pre-conditions**: The service is running and healthy.
- **Steps**:
  1. Send an HTTP GET request to `/api/v1/hello-world?name=Alice`.
- **Expected Result**: The server returns HTTP 200 OK. The JSON body contains `message: "Hello, Alice!"` and a valid ISO-8601 UTC `timestamp`.

### TC-03: Parameterized Greeting with Unicode Recipient Name
- **Pre-conditions**: The service is running and healthy.
- **Steps**:
  1. Send an HTTP GET request to `/api/v1/hello-world?name=Ren%C3%A9e`.
- **Expected Result**: The server returns HTTP 200 OK. The JSON body contains `message: "Hello, Renée!"` and a valid ISO-8601 UTC `timestamp`.

### TC-04: Swagger UI OpenAPI Schema Availability
- **Pre-conditions**: The service is running in development mode.
- **Steps**:
  1. Send an HTTP GET request to `/swagger/v1/swagger.json`.
- **Expected Result**: The server returns HTTP 200 OK with valid OpenAPI JSON documentation detailing `/api/v1/hello-world`.

## 🔴 Negative Scenarios

### TC-05: Recipient Name Exceeds Fifty Characters
- **Pre-conditions**: The service is running and healthy.
- **Steps**:
  1. Send an HTTP GET request to `/api/v1/hello-world?name=ThisNameIsFarTooLongAndExceedsTheFiftyCharacterLimitNow`.
- **Expected Result**: The server returns HTTP 400 Bad Request. The response body contains RFC 7807 Problem Details describing the length violation.

### TC-06: Recipient Name Contains ASCII Control Characters
- **Pre-conditions**: The service is running and healthy.
- **Steps**:
  1. Send an HTTP GET request with a name containing an ASCII null control character (`/api/v1/hello-world?name=Alice%00Bob`).
- **Expected Result**: The server returns HTTP 400 Bad Request. The response body contains RFC 7807 Problem Details rejecting control characters.

### TC-07: Unsupported HTTP POST Request
- **Pre-conditions**: The service is running and healthy.
- **Steps**:
  1. Send an HTTP POST request to `/api/v1/hello-world` with an empty body.
- **Expected Result**: The server returns HTTP 405 Method Not Allowed. The response body contains RFC 7807 Problem Details.

### TC-08: Unsupported HTTP PUT Request
- **Pre-conditions**: The service is running and healthy.
- **Steps**:
  1. Send an HTTP PUT request to `/api/v1/hello-world` with an empty body.
- **Expected Result**: The server returns HTTP 405 Method Not Allowed. The response body contains RFC 7807 Problem Details.

### TC-09: Unsupported HTTP DELETE Request
- **Pre-conditions**: The service is running and healthy.
- **Steps**:
  1. Send an HTTP DELETE request to `/api/v1/hello-world`.
- **Expected Result**: The server returns HTTP 405 Method Not Allowed. The response body contains RFC 7807 Problem Details.

## 🟡 Edge Cases

### TC-10: Recipient Name Query Parameter Empty Fallback
- **Pre-conditions**: The service is running and healthy.
- **Steps**:
  1. Send an HTTP GET request to `/api/v1/hello-world?name=`.
- **Expected Result**: The server returns HTTP 200 OK. The JSON response contains `message: "Hello, World!"`.

### TC-11: Recipient Name Query Parameter Whitespace Only Fallback
- **Pre-conditions**: The service is running and healthy.
- **Steps**:
  1. Send an HTTP GET request to `/api/v1/hello-world?name=%20%20%20`.
- **Expected Result**: The server returns HTTP 200 OK. The JSON response contains `message: "Hello, World!"`.

### TC-12: Recipient Name with Leading and Trailing Whitespace Trimmed
- **Pre-conditions**: The service is running and healthy.
- **Steps**:
  1. Send an HTTP GET request to `/api/v1/hello-world?name=%20Bob%20`.
- **Expected Result**: The server returns HTTP 200 OK. The JSON response contains `message: "Hello, Bob!"`.

### TC-13: Recipient Name at Boundary Length of Fifty Characters
- **Pre-conditions**: The service is running and healthy.
- **Steps**:
  1. Construct a valid name string of exactly 50 characters.
  2. Send an HTTP GET request to `/api/v1/hello-world?name={50CharName}`.
- **Expected Result**: The server returns HTTP 200 OK. The JSON response contains `message: "Hello, {50CharName}!"`.

### TC-14: Deterministic Timestamp Generation
- **Pre-conditions**: The business logic service is initialized with a controlled `TimeProvider` instance set to `2026-03-24T12:00:00Z`.
- **Steps**:
  1. Invoke the business logic method `GetHelloWorldGreeting(null)`.
- **Expected Result**: The returned `GreetingResponse` contains timestamp string `"2026-03-24T12:00:00.0000000Z"`.
