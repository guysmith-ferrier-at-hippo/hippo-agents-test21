# 🧭 Product Vision Document

## 1. Product Overview

**Product Name:**  
Customer Greeting Service (`CustomerGreetingService`)

**Author / Owner:**  
Engineering Team

---

## 2. Vision Statement

Provide a reliable, standardized REST API service that generates personalized customer greetings based on customer name and time of day, simplifying integration for consuming applications across enterprise channels.

---

## 3. Problem Statement

Different client applications across the organization currently implement custom greeting logic or display static, generic greetings to users.

- **Who experiences the problem?** Consuming client applications, internal engineering teams, and end users who receive inconsistent customer greetings.
- **What is the impact of the problem today?** Inconsistent user experience, duplicate business logic across client applications, and increased maintenance overhead.
- **Why does solving this matter?** A centralized greeting service ensures consistent branding, professional communication, and rapid reuse across systems.

---

## 4. Target Users / Audiences

| Persona | Description | Key Needs / Goals |
|---|---|---|
| Consuming Client Applications | Web frontends, mobile applications, and backend services | Retrieve accurate, contextual customer greetings through simple REST calls |
| Internal Engineering Teams | Developers maintaining enterprise systems | Integrate a tested, standardized greeting service without re-implementing greeting rules |
| API Consumers | Technical integrators and testers | Discover, evaluate, and test API endpoints using an interactive OpenAPI harness |

---

## 5. Product Goals & Outcomes

| Goal | Metric / KPI | Desired Outcome |
|---|---|---|
| Standardize greeting logic | 100% centralized greeting calculation | Consuming applications discontinue duplicate greeting algorithms |
| Ensure low integration friction | OpenAPI documentation completeness | Consuming teams test and integrate API endpoints within hours |
| Support decoupled architecture | Zero leakage of internal repository logic | Interfaces and contracts strictly encapsulate layer implementations |

---

## 6. Key Features / Capabilities

- **Personalized Greeting Generation**: Generates greetings tailored to a specified customer name.
- **Time-of-Day Context**: Computes appropriate greeting variants (morning, afternoon, evening) according to the time of day.
- **REST API Endpoints**: Exposes standardized HTTP endpoints returning structured JSON responses.
- **Interactive OpenAPI Test Harness**: Provides integrated OpenAPI (Swagger) interface for interactive documentation and manual testing.
- **In-Memory Data Repository**: Provides lightweight, self-contained repository support for initial deployment phases.

---

## 7. Value Proposition

| For (user type) | Who want to (need) | Our product (solution) | Unlike (alternative) |
|---|---|---|---|
| Consuming Applications | Display contextual, professional greetings | Customer Greeting Service | Hard-coded client-side greeting logic |
| Software Developers | Integrate standardized customer greetings | Self-documenting REST API with OpenAPI | Ad-hoc internal libraries with inconsistent maintenance |

---

## 8. Constraints / Assumptions

- **Constraints**:
  - The service is an API without a custom end-user graphic interface; OpenAPI is the sole interactive harness.
  - Multi-language and locale localization is out of scope for the current phase.
  - Durable persistence is out of scope for initial deployment.
  - Unit tests must use standard Visual Studio Unit Testing (MSTest) without fluent assertion libraries.
- **Assumptions**:
  - [ASSUME-0001: In-Memory Repository Implementation as Interim Storage](assumptions/ASSUME-0001-in-memory-storage-interim.md) — Initial environments operate with in-memory repositories until permanent persistence requirements are defined.

---

## 9. Success Metrics

- **Primary metric(s)**: Successful response rate for greeting requests (100% valid schema compliance).
- **Secondary metric(s)**: 100% automated test pass rate in CI/CD pipeline prior to deployment.
- **Qualitative indicators**: Developer ease of integration via the OpenAPI test harness.

---

## 10. Stakeholders

| Role | Name / Team | Interest / Contribution |
|---|---|---|
| Product Owner / Lead | Platform Engineering Lead | Product direction, scope boundary enforcement |
| Development Team | Backend Engineering Team | Architecture, service implementation, unit testing |
| QA / Operations | DevOps & QA Engineers | Azure DevOps pipeline automation, AKS deployment validation |
| Consumers | Integration Engineering Teams | Upstream system integration and API consumption |

---

## 11. Risks & Open Questions

- **Risk**: Container restarts in Azure Kubernetes Service (AKS) clear in-memory state.
  - *Mitigation*: Ensure consumer design does not rely on persistent session state between requests.
- **Open Question**: When will downstream consumer requirements require permanent database persistence (e.g., SQL Server or PostgreSQL)?

---

## 12. References

- [Architecture Overview](architecture.md)
- [Design Guidelines](design-guidelines.md)
- [Test Strategy](testing.md)
- [Structured Specification Conventions](specification-conventions.md)
