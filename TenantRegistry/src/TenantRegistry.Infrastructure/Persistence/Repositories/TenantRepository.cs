using Microsoft.EntityFrameworkCore;
using TenantRegistry.Application.Abstractions.Repositories;
using TenantRegistry.Domain.Entities;
using TenantRegistry.Infrastructure.Persistence.DbContext;

namespace TenantRegistry.Infrastructure.Persistence.Repositories;

public class TenantRepository : ITenantRepository
{
    private readonly TenantRegistryDbContext _dbContext;

    public TenantRepository(TenantRegistryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Tenant tenant, CancellationToken ct = default)
    {
        _dbContext.Tenants.Add(tenant);
        await _dbContext.SaveChangesAsync(ct);
    }

    public async Task<Tenant?> GetByIdAsync(Guid tenantId, CancellationToken ct = default)
    {
        return await _dbContext.Tenants
            .FirstOrDefaultAsync(t => t.TenantId == tenantId, ct);
    }

    public async Task<Tenant?> GetBySlugAsync(string slug, CancellationToken ct = default)
    {
        return await _dbContext.Tenants
            .FirstOrDefaultAsync(t => t.Slug == slug, ct);
    }

    public async Task SaveChangesAsync(CancellationToken ct)
    {
        await _dbContext.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Tenant tenant, CancellationToken ct = default)
    {
        _dbContext.Tenants.Update(tenant);
        await _dbContext.SaveChangesAsync(ct);
    }
}
