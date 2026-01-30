using Microsoft.EntityFrameworkCore;
using TenantRegistry.Application.Abstractions.Repositories;
using TenantRegistry.Application.Tenants.Queries.DTOs;
using TenantRegistry.Infrastructure.Persistence.DbContext;

namespace TenantRegistry.Infrastructure.Persistence.Repositories;

public sealed class TenantReadRepository : ITenantReadRepository
{
    private readonly TenantRegistryDbContext _dbContext;

    public TenantReadRepository(TenantRegistryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<TenantDto>> GetAllAsync(CancellationToken ct)
    {
        return await _dbContext.Tenants
            .AsNoTracking()
            .OrderByDescending(t => t.CreatedAt)
            .Select(t => new TenantDto(
                t.TenantId,
                t.Name,
                t.Slug,
                t.Country,
                t.Timezone
            ))
            .ToListAsync(ct);
    }

    public async Task<int> CountAsync(CancellationToken ct)
    {
        return await _dbContext.Tenants.CountAsync(ct);
    }

    public async Task<IReadOnlyList<TenantDto>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        CancellationToken ct)
    {
        var skip = (pageNumber - 1) * pageSize;

        return await _dbContext.Tenants
            .AsNoTracking()
            .OrderByDescending(t => t.CreatedAt)
            .Skip(skip)
            .Take(pageSize)
            .Select(t => new TenantDto(
                t.TenantId,
                t.Name,
                t.Slug,
                t.Country,
                t.Timezone
            ))
            .ToListAsync(ct);
    }
}
