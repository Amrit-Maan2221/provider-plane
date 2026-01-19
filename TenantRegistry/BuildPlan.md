# 🏗️ Tenant Registry Service — Step-by-Step Build Plan

---

## **PHASE 0 — Foundation Decisions (1–2 hours)**

### 0.1 Tech Stack (Lock This In)

| Concern    | Choice                                      |
| ---------- | ------------------------------------------- |
| Framework  | .NET 8 Web API                              |
| API Style  | Controllers                                 |
| ORM        | **EF Core** (Dapper later if needed)        |
| DB         | SQL Server                                  |
| Messaging  | Interface now → MassTransit later           |
| Auth       | None (internal service)                     |
| Validation | FluentValidation                            |
| Mapping    | Mapster or AutoMapper (Mapster recommended) |
| Logging    | Serilog                                     |
| API Docs   | Swagger                                     |

> 🚨 No authentication yet. This is a **Control Plane internal service**.

---

### 0.2 Repo Structure (Create First)

```
MaanEnterprise.Services.TenantRegistry
│
├── src
│   ├── TenantRegistry.API
│   ├── TenantRegistry.Application
│   ├── TenantRegistry.Domain
│   ├── TenantRegistry.Infrastructure
│   └── TenantRegistry.Contracts
│
└── tests
    └── TenantRegistry.Tests
```

Create **solution + projects first**, before writing logic.

---

## **PHASE 1 — Domain Layer (Source of Truth)**

> Domain has **ZERO dependency** on ASP.NET, EF, SQL, or messaging.

---

### 1.1 Create Domain Entities

**Tenant**

* Enforces lifecycle rules
* Controls state transitions

```csharp
public class Tenant
{
    public Guid TenantId { get; private set; }
    public string Name { get; private set; }
    public string Slug { get; private set; }
    public TenantStatus Status { get; private set; }
    public string? Industry { get; private set; }
    public string Country { get; private set; }
    public string Timezone { get; private set; }
    public string OnboardingStage { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private Tenant() { } // EF

    public static Tenant Create(string name, string slug, string country, string timezone)
    {
        return new Tenant
        {
            TenantId = Guid.NewGuid(),
            Name = name,
            Slug = slug,
            Country = country,
            Timezone = timezone,
            Status = TenantStatus.Pending,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    public void Activate()
    {
        if (Status != TenantStatus.Pending)
            throw new DomainException("Only pending tenants can be activated");

        Status = TenantStatus.Active;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Suspend()
    {
        if (Status != TenantStatus.Active)
            throw new DomainException("Only active tenants can be suspended");

        Status = TenantStatus.Suspended;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Close()
    {
        if (Status != TenantStatus.Active)
            throw new DomainException("Only active tenants can be closed");

        Status = TenantStatus.Closed;
        UpdatedAt = DateTime.UtcNow;
    }
}
```

---

### 1.2 Value Objects & Enums

```csharp
public enum TenantStatus
{
    Pending = 1,
    Active = 2,
    Suspended = 3,
    Closed = 4
}
```

---

### 1.3 Domain Exceptions

```csharp
public class DomainException : Exception
{
    public DomainException(string message) : base(message) { }
}
```

---

## **PHASE 2 — Application Layer (Use Cases)**

> This layer answers: **“What can the system do?”**

---

### 2.1 Commands & Queries

Create folders:

```
Application
 ├── Tenants
 │   ├── Commands
 │   │   ├── CreateTenant
 │   │   ├── ActivateTenant
 │   │   ├── SuspendTenant
 │   │   └── UpdateTenant
 │   └── Queries
 │       ├── GetTenantById
 │       └── SearchTenants
```

---

### 2.2 Example: CreateTenant Command

```csharp
public record CreateTenantCommand(
    string Name,
    string Slug,
    string Country,
    string Timezone,
    OwnerContactDto Owner
);
```

Handler:

```csharp
public class CreateTenantHandler
{
    private readonly ITenantRepository _repository;
    private readonly IEventPublisher _events;

    public async Task<Guid> Handle(CreateTenantCommand command)
    {
        var tenant = Tenant.Create(
            command.Name,
            command.Slug,
            command.Country,
            command.Timezone);

        await _repository.AddAsync(tenant);

        await _events.PublishAsync(new TenantCreatedEvent(
            tenant.TenantId,
            tenant.Slug));

        return tenant.TenantId;
    }
}
```

---

### 2.3 Define Interfaces (VERY IMPORTANT)

```csharp
public interface ITenantRepository
{
    Task AddAsync(Tenant tenant);
    Task<Tenant?> GetByIdAsync(Guid tenantId);
    Task<Tenant?> GetBySlugAsync(string slug);
    Task UpdateAsync(Tenant tenant);
}
```

```csharp
public interface IEventPublisher
{
    Task PublishAsync<T>(T @event);
}
```

---

## **PHASE 3 — Infrastructure Layer (SQL + Events)**

---

### 3.1 SQL Server Schema

```sql
CREATE TABLE Tenants (
    TenantId UNIQUEIDENTIFIER PRIMARY KEY,
    Name NVARCHAR(200) NOT NULL,
    Slug NVARCHAR(100) NOT NULL UNIQUE,
    Status INT NOT NULL,
    Industry NVARCHAR(100),
    Country NVARCHAR(50),
    Timezone NVARCHAR(50),
    OnboardingStage NVARCHAR(50),
    CreatedAt DATETIME2,
    UpdatedAt DATETIME2,
    IsDeleted BIT DEFAULT 0
);
```

---

### 3.2 EF Core DbContext

```csharp
public class TenantRegistryDbContext : DbContext
{
    public DbSet<Tenant> Tenants => Set<Tenant>();

    public TenantRegistryDbContext(DbContextOptions options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Tenant>()
            .HasIndex(x => x.Slug)
            .IsUnique();
    }
}
```

---

### 3.3 Repository Implementation

```csharp
public class TenantRepository : ITenantRepository
{
    private readonly TenantRegistryDbContext _db;

    public async Task AddAsync(Tenant tenant)
    {
        _db.Tenants.Add(tenant);
        await _db.SaveChangesAsync();
    }

    public async Task<Tenant?> GetByIdAsync(Guid tenantId)
        => await _db.Tenants.FirstOrDefaultAsync(x => x.TenantId == tenantId);
}
```

---

### 3.4 Event Publisher (MVP Stub)

```csharp
public class InMemoryEventPublisher : IEventPublisher
{
    public Task PublishAsync<T>(T @event)
    {
        Console.WriteLine($"Event published: {typeof(T).Name}");
        return Task.CompletedTask;
    }
}
```

> Replace later with **MassTransit + Azure Service Bus / RabbitMQ**.

---

## **PHASE 4 — API Layer (Controllers)**

---

### 4.1 Controller Structure

```
API
 ├── Controllers
 │   ├── TenantsController.cs
 │   └── TenantContactsController.cs
```

---

### 4.2 TenantsController (MVP)

```csharp
[ApiController]
[Route("api/tenants")]
public class TenantsController : ControllerBase
{
    private readonly CreateTenantHandler _createHandler;
    private readonly ActivateTenantHandler _activateHandler;

    [HttpPost]
    public async Task<IActionResult> Create(CreateTenantCommand command)
    {
        var tenantId = await _createHandler.Handle(command);
        return CreatedAtAction(nameof(GetById), new { tenantId }, tenantId);
    }

    [HttpGet("{tenantId}")]
    public async Task<IActionResult> GetById(Guid tenantId)
    {
        // Query handler
    }

    [HttpPost("{tenantId}/activate")]
    public async Task<IActionResult> Activate(Guid tenantId)
    {
        await _activateHandler.Handle(tenantId);
        return NoContent();
    }
}
```

---

## **PHASE 5 — Validation & Errors**

---

### 5.1 FluentValidation

```csharp
public class CreateTenantValidator : AbstractValidator<CreateTenantCommand>
{
    public CreateTenantValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Slug).NotEmpty().Matches("^[a-z0-9-]+$");
        RuleFor(x => x.Country).NotEmpty();
    }
}
```

---

### 5.2 Global Exception Middleware

Handles:

* DomainException → 400
* NotFound → 404
* Unknown → 500

---

## **PHASE 6 — Events & Integration (Phase-1 Scope)**

Publish:

* `Tenant.Created`
* `Tenant.Activated`

Define in **Contracts** project:

```csharp
public record TenantCreatedEvent(Guid TenantId, string Slug);
public record TenantActivatedEvent(Guid TenantId);
```

---

## **PHASE 7 — Testing (Critical)**

### 7.1 Domain Tests

* Status transitions
* Invalid transitions

### 7.2 API Tests (Minimal)

* Create tenant
* Activate tenant
* Duplicate slug fails

---

## **PHASE 8 — Production Hardening (Later)**

* MassTransit
* Outbox pattern
* Auth (service-to-service)
* Soft delete enforcement
* Observability (OpenTelemetry)
* Idempotency

---

## ✅ **What You’ll Have After Phase-1**

✔ Clean Tenant Registry
✔ Lifecycle enforced at domain level
✔ SQL backed, event driven
✔ Ready for Entitlements + Workshop App
✔ No coupling mistakes
