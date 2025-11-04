# Implementation Plan: Basket CRUD + Order Read Query

**Branch**: `1-basket-crud-order-read` | **Date**: 2025-11-03 | **Spec**: ./spec.md

## Summary

Implement basket CRUD endpoints (Create, Read, Update, Delete) scoped to authenticated users, and add a
read-only order summary query in the Order domain. Deliverables include API contract(s), data models,
acceptance tests, and CI contract checks.

## Technical Context

**Language/Version**: .NET 9.0 (C#) ecosystem (repository convention).  
**Primary Dependencies**: Identity provider (`Tajan.Identity.API`), Product catalog service, Order service,
Ocelot (API gateway), central package manager (Directory.Packages.props).  
**Storage**: NEEDS CLARIFICATION: Choice of persistent store for baskets and read-models (SQL Server vs
other). Default assumption: use the same relational database as other services (SQL Server) unless
project policy requires otherwise.  
**Read model pattern**: NEEDS CLARIFICATION: read-projection store strategy for Order summary (same DB vs
separate read store). Default: keep read projection in the OrderService read schema for simplicity.  
**Testing**: xUnit / integration test framework per repository conventions; consumer-driven contract tests
for cross-service calls (Product catalog, Identity).  
**Target Platform**: Linux/containers (Docker) for staging and production.  
**Performance Goals**: NEEDS CLARIFICATION: non-functional SLAs for basket ops. Default: responsive UX (<1s p95
in staging for single-basket ops).  
**Constraints**: Must integrate with centralized Identity; must follow Result Pattern for responses; follow
Constitution principles (Clean Architecture, DDD, Observability).  
**Scale/Scope**: User-scoped baskets; read-only order summary query limited to lightweight summary data.

## Constitution Check

This change touches the following constitution principles:

 - Service Autonomy & Clean Architecture — basket functionality will be implemented inside the
   `Tajan.OrderService` (no separate Basket service) but MUST respect Clean Architecture layering within
   that project (API, Application, Domain, Infrastructure).
- Identity & Access Control — basket ownership and read/query must validate identity via `Tajan.Identity.API`.  
- Domain-Driven Design & Bounded Contexts — Order read query is a read-model inside the Order bounded
  context; Basket is a separate model.  
- Shared Standards & Central Package Management — shared DTOs (Result pattern types) should be placed in
  `Tajan.Standard.*` or consumed from a package.  
- Observability, Testing & Reliability — expose health, structured logs, and contract tests.

Compliance notes: The plan MUST include contract tests for Product lookup and Identity; any deviation must
be justified in Phase 0 research and recorded.

## Phase 0: Research (resolve clarifications)

### Identified clarifications (from Technical Context)
1. Storage choice for baskets/read models (SQL Server vs alternative).  
2. Read-projection strategy for Order summary (same DB vs separate read store).  
3. Performance SLA for basket operations (p95 SLA target).

### Phase 0 tasks (research)
- R001 Research storage options and alignment with repository standards; recommend and document decision.
- R002 Research read-projection strategies (same DB vs separate read store) and propose simplest safe
  option.
- R003 Propose a pragmatic SLA for basket operations based on project expectations and staging capability.

### Phase 0 output (research.md) — created in Phase 0 (see research.md)

## Phase 1: Design & Contracts

### Data model (see data-model.md)
- Basket
- BasketItem
- OrderSummary (read-only)

### API Contracts (see contracts/openapi.yaml)
- POST /api/baskets               # create or add item
- GET  /api/baskets/{userId}      # get basket for user (authentication required)
- PUT  /api/baskets/{userId}/item/{productId}  # update quantity
- DELETE /api/baskets/{userId}/item/{productId} # remove item
- DELETE /api/baskets/{userId}    # clear basket
- GET /api/orders/{orderId}/summary  # read-only order summary

### Quickstart (see quickstart.md)
- How to run the basket service locally and run contract tests against product/identity stubs.

### Agent context update
- Attempt to run `.specify/scripts/powershell/update-agent-context.ps1 -AgentType copilot` to update
  the agent file. If execution policy prevents running signed scripts, skip and log the exception.

## Phase 2: Implementation Planning (high level tasks)

- T001 Setup: implement basket module inside the existing `Tajan.OrderService` project (no separate Basket
  service). Wire DI and configuration within `Tajan.OrderService` and ensure the project targets .NET 9.0.
- T002 Contracts: implement OpenAPI endpoints and add contract tests (Product, Identity mocks).
- T003 Persistence: implement repository and migrations for Basket and read-model (per decision).
- T004 Handlers: implement command handlers for add/update/remove and query handlers for reads.
- T005 Tests: unit tests for domain logic; integration tests for API; contract tests for external dependencies.
- T006 Observability: add structured logging, correlation IDs, health checks, metrics.
- T007 CI: add pipeline steps to run unit, integration, and contract tests for this feature.

## Artifacts generated
- `specs/1-basket-crud-order-read/plan.md` (this file)
- `specs/1-basket-crud-order-read/research.md` (Phase 0)
- `specs/1-basket-crud-order-read/data-model.md` (Phase 1)
- `specs/1-basket-crud-order-read/contracts/openapi.yaml` (Phase 1)
- `specs/1-basket-crud-order-read/quickstart.md` (Phase 1)

## Complexity Tracking

No constitution gates violated. Any deviation from centralized Identity or shared packaging must be
justified in research.md.
