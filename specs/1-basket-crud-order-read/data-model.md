# Data Model: Basket CRUD + Order Read Query

**Feature**: Basket CRUD + Order Read Query
**Created**: 2025-11-03

## Entities

### Basket
- id: GUID
- ownerId: GUID (user id)
- currency: string
- subtotal: decimal
- lastModified: datetime
- items: collection of BasketItem

### BasketItem
- id: GUID
- basketId: GUID
- productId: GUID or string (matches Product catalog)
- quantity: integer
- unitPrice: decimal
- lineTotal: decimal (unitPrice * quantity)

### OrderSummary (read-only)
- orderId: GUID
- status: enum (Created, Pending, Completed, Cancelled)
- totalAmount: decimal
- itemCount: integer
- createdAt: datetime
- updatedAt: datetime
- billingRef: string (optional)
- shippingRef: string (optional)

## Validation rules
- quantity MUST be positive integer (>= 1) for add operations; updates with 0 interpret as removal.
- productId MUST be validated against Product catalog (existence and price lookup) during basket changes.

## State transitions
- Basket: items added/updated/removed; subtotal recalculated on each write.
- OrderSummary: read-only projection derived from Order domain events.
