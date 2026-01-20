using TenantRegistry.Application.Tenants.DTOs;
using TenantRegistry.Domain.Entities;

namespace TenantRegistry.Application.Abstractions.Repositories;

public interface ITenantRepository
{
    Task AddAsync(Tenant tenant, CancellationToken ct = default);
    Task<Tenant?> GetByIdAsync(Guid tenantId, CancellationToken ct = default);
    Task<Tenant?> GetBySlugAsync(string slug, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct);
    Task UpdateAsync(Tenant tenant, CancellationToken ct = default);
}
