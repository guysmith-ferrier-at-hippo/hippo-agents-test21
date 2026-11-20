# Implementation Guidelines

## Architecture & Design Patterns

### Layered Separation
- Maintain three isolated layers: Api, BusinessLogic, and Repositories.
- Business logic classes must reside in the root of `CustomerGreetingService.BusinessLogic`, not in a `Services/` subfolder.
- The repository layer provides data access and template storage behind `IGreetingRepository`.
- Register dependencies explicitly in `Program.cs` using constructor dependency injection.

### Response Model Naming
- Name response models after the originating method plus the suffix `Response` (for example, `GetHelloWorldGreetingResponse` for `GetHelloWorldGreeting`).

### Deterministic Time Operations
- Inject `TimeProvider` into services that handle timestamps.
- Use `TimeProvider.System` in production configuration.
- Use a fake `TimeProvider` subclass in unit test suites.
- Format UTC timestamps as ISO-8601 strings with 7 fractional digits (`yyyy-MM-ddTHH:mm:ss.fffffffZ`).

### Input Validation & Error Handling
- Validate query parameter boundaries before business logic operations.
- Reject ASCII control characters with an `ArgumentException`.
- Map validation exceptions to RFC 7807 Problem Details with HTTP 400 status.
- Enforce HTTP GET restriction and return HTTP 405 Problem Details for unsupported verbs.

### Testing Conventions
- Maintain a single test project per source project (`Api.UnitTests`, `BusinessLogic.UnitTests`, `Repositories.UnitTests`).
- Place business logic unit tests directly in the root of `CustomerGreetingService.BusinessLogic.UnitTests`.
- Use standard MSTest assertions and Moq.
- Do not use third-party assertion libraries.
