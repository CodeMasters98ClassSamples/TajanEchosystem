---
description: "Task list for Basket CRUD + Order Read Query"
---

# Tasks: Basket CRUD + Order Read Query

**Input**: plan.md, spec.md, data-model.md, contracts/openapi.yaml
**Feature Dir**: specs/1-basket-crud-order-read

## Phase 1: Setup (Shared Infrastructure)

- [ ] T001 Create project folder and solution entry for Basket service: `Tajan.Basket.Service/` (create project files, csproj)
- [ ] T002 [P] Add `Tajan.Basket.Service` to the solution and wire Directory.Packages.props package references (follow repo conventions)
- [ ] T003 Configure CI stub for the feature: add pipeline job to run unit and contract tests for `Tajan.Basket.Service` in `.github/workflows` or existing CI (add file path `/.github/workflows/basket-feature.yml`)
- [ ] T004 [P] Add sample Dockerfile and Docker compose stub for local development in `Tajan.Basket.Service/Dockerfile` and `docker-compose.feature.yml`
- [ ] T005 [P] Create a lightweight product and identity stub implementations for contract tests under `tests/stubs/ProductStub` and `tests/stubs/IdentityStub`

## Phase 2: Foundational (Blocking Prerequisites)

- [ ] T006 Create database migration project or scripts for Basket schema under `Tajan.Basket.Service/Migrations/` (include SQL create table scripts for `Baskets` and `BasketItems`)
- [ ] T007 Implement configuration and DI wiring for database and repository in `Tajan.Basket.Service/ConfigureServices.cs`
- [ ] T008 Implement health and readiness endpoints in `Tajan.Basket.Service/Controllers/HealthController.cs` and add health checks registration in `ConfigureServices.cs`
- [ ] T009 [P] Add structured logging and correlation ID middleware in `Tajan.Basket.Service/Middleware/CorrelationIdMiddleware.cs` and register it in startup
- [ ] T010 [P] Add Result Pattern helpers and shared DTO references by consuming `Tajan.Standard.*` packages or copy minimal Result types into `Tajan.Basket.Service/Common/Result.cs` if package not yet released
- [ ] T011 [P] Implement contract-test harness in `tests/contract/basket` that runs product and identity stubs and validates the API contract (`specs/1-basket-crud-order-read/contracts/openapi.yaml`)

## Phase 3: User Story 1 - Create & Add Item to Basket (Priority: P1) 🎯 MVP

**Goal**: Allow authenticated user to create a basket and add items to it.
**Independent Test**: POST /api/baskets with productId+quantity -> subsequent GET returns item and correct subtotal.

- [ ] T012 [US1] Create domain model `Tajan.Basket.Service/Domain/Models/Basket.cs` and `BasketItem.cs` reflecting `data-model.md`
- [ ] T013 [US1] Create DTOs for API in `Tajan.Basket.Service/Api/Dto/BasketDto.cs` and `AddItemRequest.cs`
- [ ] T014 [US1] Implement repository interface `Tajan.Basket.Service/Services/IBasketRepository.cs` and in-memory or EF implementation `Tajan.Basket.Service/Infrastructure/BasketRepository.cs`
- [ ] T015 [US1] Implement AddItem command handler in `Tajan.Basket.Service/Application/Commands/AddItemHandler.cs` (apply Result Pattern and validate product via Product stub)
- [ ] T016 [US1] Implement API endpoint `POST /api/baskets` in `Tajan.Basket.Service/Controllers/BasketController.cs` mapping to AddItem handler
- [ ] T017 [US1] Add unit tests for AddItem handler in `tests/unit/Basket/AddItemHandlerTests.cs`
- [ ] T018 [US1] Add integration test for AddItem + GetBasket in `tests/integration/Basket/AddAndGetBasketTests.cs` using test DB and stubs
- [ ] T019 [US1] Add contract test verifying request/response against `specs/1-basket-crud-order-read/contracts/openapi.yaml` in `tests/contract/basket/add_item_test.cs`

## Phase 4: User Story 2 - Update / Remove / Clear Basket (Priority: P1)

**Goal**: Allow users to update quantities, remove single items, and clear a basket.
**Independent Test**: Modify existing basket entries via PUT/DELETE and verify via GET.

- [ ] T020 [US2] Implement UpdateItem command handler `Tajan.Basket.Service/Application/Commands/UpdateItemHandler.cs`
- [ ] T021 [US2] Implement RemoveItem command handler `Tajan.Basket.Service/Application/Commands/RemoveItemHandler.cs`
- [ ] T022 [US2] Implement ClearBasket command handler `Tajan.Basket.Service/Application/Commands/ClearBasketHandler.cs`
- [ ] T023 [US2] Implement API endpoints for update/remove/clear in `Tajan.Basket.Service/Controllers/BasketController.cs` (map routes per `contracts/openapi.yaml`)
- [ ] T024 [US2] Add unit tests for Update/Remove/Clear handlers in `tests/unit/Basket/`
- [ ] T025 [US2] Add integration tests for Update/Remove/Clear flows in `tests/integration/Basket/`

## Phase 5: User Story 3 - Read Basket (Priority: P1)

**Goal**: Expose GET /api/baskets/{userId} returning basket contents and subtotal.
**Independent Test**: GET returns correct item list and subtotal.

- [ ] T026 [US3] Implement Query handler `Tajan.Basket.Service/Application/Queries/GetBasketHandler.cs` building BasketDto
- [ ] T027 [US3] Wire GET endpoint in `Tajan.Basket.Service/Controllers/BasketController.cs` to GetBasket handler
- [ ] T028 [US3] Add unit and integration tests for GetBasket in `tests/unit` and `tests/integration` paths
- [ ] T029 [US3] Add caching strategy task (optional) to cache GET results in `Tajan.Basket.Service/Infrastructure/Caching` if performance SLA requires it

## Phase 6: User Story 4 - Order Read Query (Priority: P2)

**Goal**: Add a lightweight order summary read query to the Order service used by checkout and order history.
**Independent Test**: GET /api/orders/{orderId}/summary returns summary fields matching `data-model.md`.

- [ ] T030 [US4] Add OrderSummary DTO in `Tajan.OrderService.Application/Dtos/OrderSummaryDto.cs`
- [ ] T031 [US4] Implement Query handler `Tajan.OrderService.Application/Queries/GetOrderSummaryHandler.cs` that returns projection
- [ ] T032 [US4] Wire API endpoint `GET /api/orders/{orderId}/summary` in `Tajan.OrderService.API/Controllers/OrderController.cs`
- [ ] T033 [US4] Add unit tests for the query handler in `Tajan.OrderService.Application` tests
- [ ] T034 [US4] Add contract test validating response shape in `tests/contract/order/summary_test.cs`

## Phase 7: Polish & Cross-Cutting Concerns

- [ ] T035 [P] Add structured logging and metrics for all handlers (record success/failure counts) in `Tajan.Basket.Service/Monitoring/`
- [ ] T036 [P] Add health/readiness checks to existing service entry points and document them in `quickstart.md`
- [ ] T037 [P] Create CHANGELOG entry and release notes for `Tajan.Basket.Service` and `Tajan.OrderService` updates in `docs/CHANGELOG.md`
- [ ] T038 Add migration notes (if DB changes) in `specs/1-basket-crud-order-read/migrations/README.md`
- [ ] T039 [P] Update `.specify/templates/plan-template.md` or plan-checklist to reflect the Constitution Check items used in this plan (path: `.specify/templates/plan-template.md`) — (manual review recommended)

## Dependencies & Execution Order

- Foundation tasks T006-T011 MUST complete before user stories T012-T029 begin.
- Order read query tasks T030-T034 may run in parallel with basket implementation where teams are independent, but contract tests should be added to ensure compatibility.
- Within each user story: Tests (unit) -> Models -> Services -> Endpoints -> Integration/Contract tests.

## Parallel Opportunities

- Tasks marked [P] can run in parallel (logging, stubs, package wiring, Dockerfile, test harness, caching, monitoring work).
- Unit test creation (per handler) can be done in parallel with repository and handler implementation if interfaces are agreed upon.

## Implementation Strategy

- MVP first: Complete Phase 3 (User Story 1) end-to-end (Create/Add and Get basket). Stop and validate with contract tests and CI.
- Incrementally add Update/Remove/Clear (Phase 4) and Read Basket (Phase 5).
- Add Order summary query (Phase 6) after the basket pinned contract is stable.

## Task Counts & Summary

- Total tasks: 39
- Tasks by story/phase:
  - Setup / Foundational: 11
  - US1 (Create/Add): 8
  - US2 (Update/Remove/Clear): 6
  - US3 (Read): 4
  - US4 (Order Read Query): 5
  - Polish & Cross-cutting: 5

**Notes**: The task list is explicit about file paths and minimal implementations suitable for an LLM to act on. If you want TDD-first, I can flip the order in each story so tests are created first and marked as required.
