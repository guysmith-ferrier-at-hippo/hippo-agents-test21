---
id: FEAT-0001-02
title: "Parameterized Hello World Greeting Specification"
status: Implemented
created: '2026-03-24'
depends-on:
  - "specs/hello-world/feature.md"
supported-by: []
---

## ADDED Requirements

### Requirement: Parameterized Recipient Greeting Delivery
When a client sends an HTTP GET request to `/api/v1/hello-world` with a valid name parameter, the system shall return an HTTP 200 response.

The response shall contain a JSON body with the personalized message and the current UTC timestamp.

#### Acceptance Criteria
- Sending `GET /api/v1/hello-world?name=Alice` returns HTTP status code 200 OK.
- The response body contains property `message` with value `"Hello, Alice!"`.
- The response body contains property `timestamp` formatted according to ISO-8601 UTC.
- Leading and trailing whitespace characters in the name parameter are trimmed.

### Requirement: Empty Name Fallback
If the client provides a name parameter containing only whitespace, then the system shall return the default greeting message "Hello, World!".

#### Acceptance Criteria
- Sending `GET /api/v1/hello-world?name=` returns message `"Hello, World!"`.
- Sending `GET /api/v1/hello-world?name=%20%20` returns message `"Hello, World!"`.
- The response returns status code HTTP 200 OK.

### Requirement: Input Length Validation
If the client provides a name parameter exceeding 50 characters, then the system shall return an HTTP 400 Bad Request response.

The response body shall follow the RFC 7807 Problem Details schema.

#### Acceptance Criteria
- Query parameters longer than 50 characters produce HTTP 400 Bad Request.
- The error response contains standard RFC 7807 fields including title, status, and detail.

### Requirement: Control Character Rejection
If the client provides a name parameter containing unescaped control characters, the system shall prevent request execution.

The system shall return an HTTP 400 Bad Request response.

#### Acceptance Criteria
- Name inputs containing control characters return HTTP 400 Bad Request.
- The system returns an RFC 7807 Problem Details error payload.
