# 🏢 **Tenant Registry Service — Design Blueprint**

## 🎯 **Purpose**

The Tenant Registry is the **source of truth for all tenants** in MaanEnterprise.
Every other Control Plane and Application Plane service references it.

It manages:

* Tenant identity
* Lifecycle state
* Contacts & organization profile
* Multitenancy metadata (region, mode, etc.)
* Platform-level attributes

It does **NOT** handle subscriptions, billing, or entitlements —
it only defines **who the tenant is**.

---

## 🧠 **Core Responsibilities**

* Create / update tenant profiles
* Maintain tenant lifecycle status
* Track ownership + identifiers
* Provide tenant lookup APIs
* Emit lifecycle events

Lifecycle states:

```
Pending → Active → Suspended → Closed
```

---

## 🧩 **Domain Model**

### **Entity: Tenant**

| Field                   | Description                               |
| ----------------------- | ----------------------------------------- |
| tenant_id (GUID)        | Primary identifier shared across platform |
| name                    | Legal / organization name                 |
| slug                    | Unique code (abc-workshop)                |
| status                  | pending / active / suspended / closed     |
| industry                | optional                                  |
| country / timezone      | localization                              |
| onboarding_stage        | helps provisioning                        |
| created_at / updated_at | system tracking                           |

---

### **Entity: TenantContact**

* Owner / primary business contact

| Field                      |
| -------------------------- |
| contact_id                 |
| tenant_id                  |
| name                       |
| email                      |
| phone                      |
| role (Owner/Admin/Billing) |

---

### **Entity: TenantSettings**

Platform-level settings (not feature limits):

| Example fields                                                  |
| --------------------------------------------------------------- |
| default_locale                                                  |
| data_region                                                     |
| multitenancy_mode = shared-db / schema-per-tenant / isolated-db |
| branding_name                                                   |
| support_email                                                   |

---

## 🗄️ **Database Tables (Proposed)**

```
Tenants
TenantContacts
TenantSettings
TenantTags
```

We can start simple with:

* Soft-delete instead of hard delete

---

## 🌐 **Public API (REST)**

Base path:

```
/api/tenants
```

### **Create Tenant**

```
POST /api/tenants
```

Body:

```
name, slug, country, timezone, owner_contact
```

Returns:

```
tenant_id
status = "pending"
```

---

### **Get Tenant**

```
GET /api/tenants/{tenant_id}
```

---

### **Search Tenants**

```
GET /api/tenants?slug=abc-workshop
GET /api/tenants?status=active
```

---

### **Update Tenant Profile**

```
PUT /api/tenants/{tenant_id}
```

---

### **Change Lifecycle State**

```
POST /api/tenants/{tenant_id}/activate
POST /api/tenants/{tenant_id}/suspend
POST /api/tenants/{tenant_id}/close
```

Suspension does **not delete data** — it signals downstream services to block access.

---

### **Manage Contacts**

```
POST   /api/tenants/{tenant_id}/contacts
GET    /api/tenants/{tenant_id}/contacts
DELETE /api/tenants/{tenant_id}/contacts/{contact_id}
```

---

### **Update Tenant Settings**

```
PUT /api/tenants/{tenant_id}/settings
GET /api/tenants/{tenant_id}/settings
```

---

## 📣 **Integration Events (via message bus)**

The Tenant Registry should **publish events**, not call other services directly.

Events:

```
Tenant.Created
Tenant.Activated
Tenant.Suspended
Tenant.Closed
Tenant.Updated
```

Consumers:

* Subscription / Entitlement Service
* Billing Service
* Audit Service
* Application Provisioning Workers

This makes provisioning **asynchronous and scalable**.

---

## 🛡️ **Service Boundaries (What It MUST NOT Do)**

❌ No subscription logic
❌ No product access decisions
❌ No billing or plan validation
❌ No application-side user management

It is **identity + lifecycle only**.

---

## 🧱 **Microservice Structure (Suggested)**

```
MaanEnterprise.Services.TenantRegistry
 ├─ API
 ├─ Application (commands / handlers)
 ├─ Domain (entities, rules)
 ├─ Infrastructure (DB + messaging)
 └─ Contracts (DTOs + events)
```

---

## 🚦 **Validation Rules**

* `slug` must be globally unique
* status transitions allowed:

  ```
  pending -> active
  active -> suspended
  suspended -> active
  active -> closed
  ```
* cannot reactivate a closed tenant
* emails must be verified (future enhancement)

---

## 🧪 **MVP Scope (First Implementation)**

Phase-1 features to build:

* Create tenant
* Get tenant
* Update tenant profile
* Activate / Suspend tenant
* Manage primary contact
* Publish `Tenant.Created` and `Tenant.Activated`

This is enough to let the **Entitlement Service** and **Workshop App** attach to tenants.
