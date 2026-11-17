# Customer Greeting Service — Design Guidelines

## Design Principles

The Customer Greeting Service operates as a headless REST API. The design principles focus on clarity, predictability, and professional communication:
- **Predictable and Intuitive**: Endpoint routes, query parameters, and JSON response bodies adhere strictly to REST conventions.
- **Self-Documenting**: API schemas, data models, and error responses are fully documented through OpenAPI standards.
- **Professional Demeanor**: Greeting content delivers a formal, respectful, and professional tone suitable for enterprise customer interactions.

## Design System & Tooling

- **Storybook**: No (Not applicable for headless API service)
- **External Design System**: Standard OpenAPI / Swagger UI test harness (Swashbuckle / Microsoft.AspNetCore.OpenApi)
- **Design Decisions**:
  - [DD-0001: OpenAPI (Swagger UI) as Developer Interface and Interactive Test Harness](decisions/DD-0001-openapi-test-harness.md)

## UI Constraints

Because the service provides no custom graphical user interface, visual constraints apply to the integrated OpenAPI (Swagger UI) test harness:

### Color Palette
| Name | Hex/CSS Variable | Usage |
| :--- | :--- | :--- |
| Swagger Brand Green | `#49cc90` | HTTP GET operations and success highlights |
| Swagger Accent Blue | `#4990e2` | HTTP POST operations and documentation headers |
| Dark Neutral | `#3b4151` | Primary text and endpoint paths |
| Light Neutral Background | `#fafafa` | Test harness background container |
| Border Neutral | `#d9d9d9` | Card borders and schema separators |

### Typography
- **Primary Font**: System sans-serif (`Segoe UI`, `Helvetica Neue`, `Arial`, `sans-serif`)
- **Headings**: Semi-bold (600 weight), clear sizing hierarchy in Swagger UI
- **Body Text**: Regular (400 weight), high readability for parameter descriptions and schema attributes
- **Monospace Font**: `Consolas`, `Menlo`, `monospace` for JSON payloads and route templates

### Spacing & Layout
- **Grid/Layout System**: OpenAPI responsive single-column layout with collapsible operation panels
- **Spacing Scale**: Standard 4px / 8px incremental margins provided by Swagger UI stylesheet
- **Breakpoints**: Responsive layout supporting Desktop (>= 1024px), Tablet (< 1024px), and Mobile (< 640px) for developer testing

## Content Strategy

- **Tone of Voice**:
  - Greeting text returned by the service must be professional, courteous, and grammatically precise.
  - Greeting examples:
    - *Morning*: "Good morning, {CustomerName}."
    - *Afternoon*: "Good afternoon, {CustomerName}."
    - *Evening*: "Good evening, {CustomerName}."
  - Avoid informal, colloquial, or overly playful phrasing (e.g., avoid "Hey", "Howdy", "What's up").
- **Terminology**:
  - Use `Customer` rather than `User` or `Client` when referring to the recipient of the greeting.
  - Use `Time of Day` for interval classifications (`Morning`, `Afternoon`, `Evening`).
  - Use RFC 7807 problem fields (`type`, `title`, `status`, `detail`, `instance`) for all error bodies.
- **Grammar & Formatting**:
  - Title Case for OpenAPI endpoint tags and operation titles.
  - Sentence case for field descriptions and parameter annotations.
  - JSON property names formatted strictly in `camelCase`.
  - Endpoint URIs formatted strictly in lowercase kebab-case (e.g., `/api/v1/customer-greetings`).

## Accessibility (a11y)

- **Standard**: WCAG 2.1 Level AA compliance provided by standard Swagger UI interface
- **Contrast Ratios**: Minimum 4.5:1 contrast ratio for all text elements against backgrounds in documentation
- **Focus States**: High-contrast outline visible on all interactive buttons, text inputs, and expandable sections
- **Keyboard Navigation**: Complete tab-order accessibility for the "Try it out" and "Execute" workflows

## Component Behaviors

- **Interactive Test Harness**:
  - "Try it out" expands editable fields for query parameters (e.g., customer name).
  - "Execute" sends an asynchronous HTTPS request and displays response code, headers, and JSON body.
- **Loading States**: Visual spinner indicator appears during pending API requests within Swagger UI.
- **Validation Feedback**: Invalid parameter inputs display client-side validation cues prior to execution, and return HTTP 400 Bad Request with Problem Details on server response.
