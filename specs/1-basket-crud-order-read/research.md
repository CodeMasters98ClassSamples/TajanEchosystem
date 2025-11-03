# Research: Basket storage, read-projection, and SLA

**Feature**: Basket CRUD + Order Read Query
**Created**: 2025-11-03

## Clarification 1: Storage choice for baskets/read models

Decision: Use the existing relational database used by other services (SQL Server) and add a dedicated
`Baskets` schema/table. This keeps operational surface smaller and aligns with repository conventions
for consistency.

Rationale:
- The repository uses .NET and typical enterprise setups in this workspace use SQL Server; reusing the
  same database reduces operational overhead and simplifies transaction patterns.
- Basket data is relatively small and benefits from relational integrity for line-item calculations.

Alternatives considered:
- Dedicated NoSQL store (e.g., Redis/Document DB) — better for ephemeral low-latency access but adds
  operations and eventual consistency complexity. Consider later for high-scale performance optimization.

## Clarification 2: Read-projection strategy for Order summary

Decision: Keep the Order summary read projection in the Order service's read schema (same relational DB),
populated by domain events or by a lightweight projection update on order state changes.

Rationale:
- Minimizes cross-service coupling; the Order service owns its read models and can maintain eventual
  consistency via domain events.
- Simpler to implement for the initial iteration; separate read stores can be introduced later if load
  testing identifies a bottleneck.

Alternatives considered:
- Separate read store (e.g., dedicated read DB, Elasticsearch) — better for complex queries and analytics
  but out of scope for this small feature.

## Clarification 3: Performance SLA for basket operations

Decision: Target a conservative baseline SLA: p95 < 1 second for basket read/write operations in staging
and acceptable single-request throughput for initial rollout. This is documented as a success criterion in
the spec and can be tightened after performance tests.

Rationale:
- Matches the UX-focused success criteria in the spec and is achievable with reasonable indexing and
  caching.

---

All NEEDS CLARIFICATION items from the plan are resolved above. If you prefer a different store (e.g., Redis
for baskets), we can update the plan and data model accordingly; note this requires additional infra work
and contract tests.
