using Microsoft.EntityFrameworkCore;
using TenantRegistry.Application.Abstractions.Repositories;
using TenantRegistry.Application.Tenants.DTOs;
using TenantRegistry.Infrastructure.Persistence.DbContext;

namespace TenantRegistry.Infrastructure.Persistence.Repositories;

public sealed class TenantReadRepository : ITenantReadRepository
{
    private readonly TenantRegistryDbContext _dbContext;

    public TenantReadRepository(TenantRegistryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<TenantListDto>> GetAllAsync(CancellationToken ct)
    {
        return await _dbContext.Tenants
            .AsNoTracking()
            .OrderByDescending(t => t.CreatedAt)
            .Select(t => new TenantListDto
            {
                TenantId = t.TenantId,
                Name = t.Name,
                Slug = t.Slug,
                Country = t.Country,
                Status = t.Status.ToString(),
                CreatedAt = t.CreatedAt
            })
            .ToListAsync(ct);
    }
}
