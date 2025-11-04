# Feature: Basket CRUD + Order Read Query

**Feature Branch**: `1-basket-crud-order-read`
**Created**: 2025-11-03
**Status**: Draft

## Summary

Allow shoppers to create and manage a shopping basket (CRUD) and add a read-only query in the Order domain to
retrieve order summaries needed by front-end checkout and reporting flows. This feature provides the
basic basket user journeys and a read query in the Order service that returns a lightweight order summary.

## User Scenarios & Testing (mandatory)

### User Story 1 - Create & Add Item to Basket (Priority: P1)
As an authenticated shopper, I want to add a product to my basket so I can continue shopping and later
checkout.

Why this priority: Core e-commerce flow; without a basket users cannot construct an order.

Independent Test:
- Call the "Add item to basket" endpoint with valid product id and quantity; verify response indicates success
  and subsequent "Get basket" contains the item with correct quantity and line totals.

Acceptance Scenarios:
1. Given an authenticated user with an empty basket, when they add product P x N, then the basket contains
   product P with quantity N and correct pricing.
2. Given the same user adds product P again, when they add it, then quantity is incremented correctly.

---

### User Story 2 - Update / Remove / Clear Basket (Priority: P1)
As a shopper, I want to update quantities, remove items, or clear my basket so I can correct mistakes and
manage my items.

Independent Test:
- Update quantity for an existing item; verify quantity and totals update. Remove item; verify item no longer
  appears. Clear basket; verify basket becomes empty.

Acceptance Scenarios:
1. Given a basket with product P, when the user sets quantity to 0, then the item is removed.
2. Given multiple items, when the user clears the basket, then the basket contains no items.

---

### User Story 3 - Read Basket (Priority: P1)
As a shopper, I want to view my current basket contents and subtotal so I can proceed to checkout.

Independent Test:
- Call the "Get basket" endpoint and verify returned items, quantities, per-line price, and subtotal.

Acceptance Scenarios:
1. Given a basket with items, when the user fetches it, then the response includes all items and an accurate subtotal.
2. Given an empty basket, when the user fetches it, then the response indicates an empty collection and zero subtotal.

---

### User Story 4 - Order Read Query (Priority: P2)
As a front-end/checkout consumer, I want a read-only order summary query in the Order service that returns a
lightweight representation of an order (id, status, total, line count, billing/shipping reference) to be used
in checkout and simple order history views.

Independent Test:
- Call the Order read query with an existing order id and verify the returned summary matches canonical order
  data (no sensitive or heavy payloads included).

Acceptance Scenarios:
1. Given an existing completed order, when the consumer requests the summary, then the service returns id,
   status, total amount, item count, and timestamps.
2. Given a non-existent order id, when requested, then the service returns a clear not-found error.

## Edge Cases

- Concurrent updates to the same basket (race conditions).
- Product becomes unavailable between add and checkout (inventory mismatch).
- Malformed product identifiers, negative quantities, or excessively large quantities.
- Anonymous vs authenticated basket semantics (assumption noted below).

## Requirements (mandatory)

### Functional Requirements
- **FR-001**: The system MUST provide endpoints or API calls to Create, Read, Update, and Delete a user's basket.
- **FR-002**: The basket MUST be scoped to a single authenticated user identity and MUST NOT expose other users' baskets.
- **FR-003**: The system MUST return clear success/failure responses with an error code and human-readable message
  for client consumption.
- **FR-004**: The system MUST validate product identifiers and quantities; invalid inputs MUST return a 4xx error
  with validation details.
- **FR-005**: The system MUST provide a read-only Order summary query that returns order id, status, total,
  item count, and timestamps without heavy payloads (no full line-item detail unless explicitly requested).
- **FR-006**: The basket endpoints MUST be idempotent where appropriate (e.g., safe repeated add operations should
  not create duplicate inconsistent state).
- **FR-007**: The system MUST include acceptance tests that cover the primary user journeys described above.

## Key Entities

- **Basket**: Owner (user id), collection of BasketItems, subtotal, currency, lastModified.
- **BasketItem**: productId, quantity, unitPrice, lineTotal.
- **OrderSummary (read-only)**: orderId, status, totalAmount, itemCount, createdAt, updatedAt, billingRef, shippingRef.

## Success Criteria (mandatory)

- **SC-001**: Users can add an item to their basket and see it listed within 1 second in 95% of successful requests
  during functional testing (measured in staging).
- **SC-002**: Core basket flows (add/update/remove/get) pass automated acceptance tests in CI with no regressions.
- **SC-003**: The Order summary read query returns correct summary data for 99% of verified existing orders in test
  datasets.
- **SC-004**: Error responses for invalid input are descriptive and include an error code and message for client handling.

## Assumptions

- Authentication is required: baskets are tied to authenticated users (the system uses the centralized Identity
  provider). If anonymous baskets are needed later, they will be added in a follow-up with migration rules.
- Pricing and inventory checks (reservations) are out of scope for this feature—this feature focuses on basket
  CRUD and an Order read summary.
- The product catalog service exists and provides product IDs and pricing; this feature calls read-only product
  lookup during validation (contract test coverage required).
 - Implementation location: The basket functionality will be implemented inside the existing `Tajan.OrderService`
   (no separate Basket service). This keeps the basket and order aggregates colocated and avoids an extra
   service layer.
 - Platform version: All services MUST target .NET 9.0 (net9.0). Update project files accordingly.

## Dependencies

- Identity service for user authentication and identity resolution.
- Product catalog for product existence and pricing validation.
- Order service for the Order summary read query (may be local if the order exists in the same service).

## Deliverables

- API endpoints for basket CRUD (contract described in tasks/implementation phase).
- Automated acceptance tests for primary flows.
- Order read-only summary query implemented and covered by contract tests.

---

**Spec created from user input:** "Add Crud for basket and add read query in Order"
