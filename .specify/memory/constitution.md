<!--
Sync Impact Report

Version change: TEMPLATE -> 1.0.0

Modified principles:
- [PRINCIPLE_1_NAME] -> Service Autonomy & Clean Architecture
- [PRINCIPLE_2_NAME] -> Identity & Access Control (Identity-First Security)
- [PRINCIPLE_3_NAME] -> Domain-Driven Design & Bounded Contexts
- [PRINCIPLE_4_NAME] -> Shared Standards & Central Package Management
- [PRINCIPLE_5_NAME] -> Observability, Testing & Reliability

Added sections:
- Additional Constraints & Standards
- Development Workflow & Quality Gates

Removed sections: none

Templates requiring updates:
- .specify/templates/plan-template.md: ⚠ pending — verify "Constitution Check" gates align with new principles
- .specify/templates/spec-template.md: ⚠ pending — ensure required sections reflect constitution-driven requirements
- .specify/templates/tasks-template.md: ⚠ pending — task categories and foundational tasks review

Follow-up TODOs:
- Review and update templates listed above to add explicit Constitution Check gates and references to
	package/versioning rules (owner: maintainers). These are marked as pending instead of automatic edits
	to avoid unintended template changes.
-->

# Project Constitution: Tajan Ecosystem

## Core Principles

### Service Autonomy & Clean Architecture
Every service in the Tajan ecosystem is an independently deployable unit that MUST follow
Clear/explicit boundaries and Clean Architecture principles (separation of concerns: API, Application,
Domain, Infrastructure). Services MUST not depend on runtime shared state; inter-service integration
MUST happen via well-defined contracts (HTTP/gRPC/Events). Internal layers MUST be testable in isolation.

Rationale: Enforces independent lifecycle, reduces blast radius of changes, and makes ownership and
testing boundaries explicit.

### Identity & Access Control (Identity-First Security)
Authentication and authorization are centralized responsibilities. The `Tajan.Identity.API` is the
source of truth for authentication and role management. All services MUST integrate with the identity
provider for authentication (JWT/OAuth2/OpenID Connect) and MUST perform fine-grained authorization
checks at the service boundary. Secrets, tokens, and identity configuration MUST follow secure default
practices and be rotated and audited.

Rationale: Centralizing identity reduces duplicated security logic and ensures consistent role/permission
semantics across services (login, roles, claims, refresh/rotation policies).

### Domain-Driven Design & Bounded Contexts
Each core domain (Orders, Products, Notifications, Captcha, etc.) MUST be implemented with explicit
bounded contexts and domain models. Domain invariants MUST be enforced inside the domain layer. The
Order domain is the canonical example in this repository and MUST follow DDD patterns (aggregates,
domain events, repositories, domain services). Cross-context integration MUST use anti-corruption layers
or explicit contracts to prevent model leakage.

Rationale: DDD keeps domain logic correct and focused, and prevents accidental coupling between domains.

### Shared Standards & Central Package Management
Common code that is intended for reuse across services MUST live in the `Tajan.Standard.*` projects and
be distributed via the repository's central package management (Directory.Packages.props / internal
NuGet feed or equivalent). Libraries in the standard layer MUST be stable, well-versioned, and have
backward-compatibility guarantees as described in the Versioning policy below. The team MUST use the
Result Pattern consistently for operations that return success/failure to standardize error handling.

Rationale: Avoids copy-paste, enforces a single source of truth for shared utilities, and enables safe
reuse with explicit versioning.

### Observability, Testing & Reliability
All services MUST emit structured logs, support correlation IDs, expose health and readiness endpoints,
and export metrics and traces where practical. Testing MUST include unit tests, contract/consumer-driven
tests for public APIs, and integration tests for critical flows. CI gates MUST fail on test regressions
and on contract incompatibilities. Reliability practices (timeouts, retries, circuit breakers) MUST be
applied at appropriate layers and verified by integration and load testing.

Rationale: Observability and disciplined testing reduce time to detect and fix issues in a distributed
system and raise confidence for safe releases.

## Additional Constraints & Standards

- Platform: .NET (C#) microservices following Clean Architecture; containerized via Docker; orchestrated
	per team policy.
- API Gateway: Ocelot is the supported API gateway for routing and cross-cutting concerns at the edge.
- Packaging: The repository uses a central package manager (Directory.Packages.props) — teams MUST
	publish and consume shared packages with semantic versioning.
- Error handling: Use the Result Pattern consistently for service boundaries and internal operations.
- Security: Secrets MUST be stored in a secure secret store; environment-specific configuration MUST
	not be checked into source control.
- Contracts: Cross-service API contracts MUST be explicit, versioned, and covered by contract tests.

## Development Workflow & Quality Gates

- Code Reviews: All changes must be delivered via pull requests and reviewed by at least one assigned
	reviewer (two for security-sensitive changes).
- Tests: Unit tests and static analysis MUST pass locally and in CI. Contract and integration tests MUST
	be executed in CI for PRs that touch public APIs or shared packages.
- Constitution Check: Each plan/spec must include a short "Constitution Check" section that identifies
	which principles the change touches and how it complies (see `.specify/templates/plan-template.md`).
- Releases: Service and package releases MUST include CHANGELOG entries that document semantic version
	changes and migration notes for breaking changes.

## Governance

Amendments: Amendments to this constitution are made by editing this file and submitting a Pull Request.
An amendment MUST be approved by at least two repository maintainers (or a majority of the core
maintainers if more than three exist). The PR description MUST include a migration/compatibility plan
for any breaking governance changes.

Versioning policy:
- MAJOR (X.0.0): Reserved for backward-incompatible governance changes or removal/renaming of
	principles that materially alter expectations.
- MINOR (1.Y.0): New principles or materially expanded guidance (e.g., new mandatory practices).
- PATCH (1.0.Z): Non-semantic clarifications, typos, or wording improvements.

Emergency hotfixes: If an urgent governance action is required (security incident, legal requirement), a
maintainer may fast-track an amendment with a short justification in the PR and notify the maintainers
immediately; the PR must still be recorded and archived.

Compliance review: The constitution will be reviewed at least every 6 months and upon major changes to
the architecture (e.g., adding a new core service type, changing packaging strategy). Compliance checks
are expected during Phase 0/Plan review and before releases that change cross-service contracts.

**Version**: 1.0.0 | **Ratified**: 2025-11-03 | **Last Amended**: 2025-11-03
