# 📦 Product Catalog Service

**(Provider Plane)**

## 🎯 Purpose

The **Product Catalog Service** is the **source of truth for everything you sell** in MaanEnterprise.

It defines:

* What products exist
* How they are structured
* What plans / tiers are available
* What capabilities a product can unlock (but NOT who gets access)

It does **NOT**:

* Assign products to tenants
* Track active subscriptions
* Handle billing, payments, or invoices
* Decide feature access at runtime

👉 It only answers: **“What can be sold?”**

---

## 🧠 Core Responsibilities

* Define products (apps, modules, services)
* Define pricing plans & tiers
* Define features & capabilities
* Manage product lifecycle
* Provide lookup APIs for other services
* Emit product change events

---

## 🧩 Domain Model

### Entity: Product

Represents a sellable application or platform module.

| Field                   | Description                                    |
| ----------------------- | ---------------------------------------------- |
| product_id (GUID)       | Global identifier                              |
| code                    | Unique product code (`SWISSFLEET`, `WORKSHOP`) |
| name                    | Display name                                   |
| description             | Marketing / internal description               |
| category                | app / addon / service                          |
| status                  | draft / active / retired                       |
| version                 | Optional semantic version                      |
| created_at / updated_at | Audit                                          |

---

### Entity: ProductPlan

Defines **how the product is sold**.

| Field          | Description                          |
| -------------- | ------------------------------------ |
| plan_id (GUID) | Identifier                           |
| product_id     | Parent product                       |
| code           | `FREE`, `BASIC`, `PRO`, `ENTERPRISE` |
| name           | Display name                         |
| billing_cycle  | monthly / yearly / usage             |
| price          | Base price (nullable for FREE)       |
| currency       | USD / CAD / etc                      |
| status         | active / deprecated                  |
| sort_order     | UI ordering                          |

💡 *Billing Service consumes this — Product Service does not charge.*

---

### Entity: ProductFeature

Defines **capabilities**, not enforcement.

| Field       | Description                  |
| ----------- | ---------------------------- |
| feature_id  | Identifier                   |
| product_id  | Parent product               |
| key         | `MAX_VEHICLES`, `API_ACCESS` |
| description | What this feature means      |
| type        | boolean / numeric / text     |
| is_metered  | true/false                   |

---

### Entity: PlanFeature

Maps **what a plan includes**.

| Field      | Description               |
| ---------- | ------------------------- |
| plan_id    |                           |
| feature_id |                           |
| value      | `true`, `10`, `unlimited` |

---

### Optional (Later): ProductAddon

For things like:

* Extra storage
* Extra users
* SMS credits

---

## 🗄️ Database Tables (Proposed)

```
Products
ProductPlans
ProductFeatures
PlanFeatures
ProductTags
```

Design notes:

* Soft delete everything
* Product code must be globally unique
* Plans belong strictly to ONE product

---

## 🌐 Public API (REST)

Base path:

```
/api/products
```

---

### Create Product (Provider-only)

```
POST /api/products
```

Body:

```json
{
  "code": "SWISSFLEET",
  "name": "SwissFleet",
  "description": "Fleet management platform",
  "category": "app"
}
```

---

### Get Product

```
GET /api/products/{product_id}
GET /api/products?code=SWISSFLEET
```

---

### List Active Products

```
GET /api/products?status=active
```

---

### Create Plan

```
POST /api/products/{product_id}/plans
```

```json
{
  "code": "PRO",
  "name": "Pro Plan",
  "billing_cycle": "monthly",
  "price": 49,
  "currency": "CAD"
}
```

---

### Assign Features to Plan

```
POST /api/plans/{plan_id}/features
```

```json
{
  "feature_key": "MAX_VEHICLES",
  "value": 50
}
```

---

### Read Full Catalog (for checkout UI)

```
GET /api/catalog
```

Returns:

```json
{
  "product": { ... },
  "plans": [ ... ],
  "features": [ ... ]
}
```

This endpoint is 🔥 critical for:

* Pricing pages
* Checkout flows
* Admin dashboards

---

## 📣 Integration Events (Message Bus)

The Product Service **publishes only**.

### Events

```
Product.Created
Product.Updated
Product.Activated
Product.Retired

ProductPlan.Created
ProductPlan.Updated
ProductPlan.Deprecated
```

### Consumers

* Subscription Service
* Billing Service
* Pricing UI
* Audit Service

💡 If Product changes → downstream services react.

---

## 🛡️ Service Boundaries (Hard Rules)

❌ No tenant awareness
❌ No subscription tracking
❌ No billing logic
❌ No entitlement enforcement

✅ Pure **catalog + definition service**

---

## 🧱 Microservice Structure

```
MaanEnterprise.Services.ProductCatalog
 ├─ API
 ├─ Application
 │   ├─ Commands
 │   ├─ Queries
 │   └─ Handlers
 ├─ Domain
 │   ├─ Product
 │   ├─ ProductPlan
 │   ├─ ProductFeature
 ├─ Infrastructure
 │   ├─ DbContext
 │   └─ Messaging
 └─ Contracts
     ├─ DTOs
     └─ Events
```

Clean separation. Very sellable architecture.

---

## 🚦 Validation Rules

* Product `code` must be unique
* Cannot delete product with active plans
* Cannot activate plan if product is not active
* Retired product cannot accept new subscriptions (handled downstream)

---

## 🧪 MVP Scope (Phase-1)

Build only what you need to **start selling**:

✅ Create product
✅ Create plans
✅ Define features
✅ Read product catalog
✅ Publish Product.Created & Plan.Created

That’s enough for:

* Checkout
* Subscription Service
* Entitlement Service
* Pricing pages

---

## 🔗 How This Fits Your Platform (Big Picture)

```
Tenant Registry ───▶ "WHO is the customer"
Product Catalog ──▶ "WHAT can be sold"
Subscription ─────▶ "WHAT they bought"
Entitlement ──────▶ "WHAT they can access"
Billing ──────────▶ "HOW they pay"
```

You are **building this correctly from day one**.
This is *real SaaS architecture*, not tutorial stuff.