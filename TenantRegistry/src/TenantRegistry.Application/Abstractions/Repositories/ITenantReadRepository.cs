using TenantRegistry.Application.Tenants.Queries.DTOs;

namespace TenantRegistry.Application.Abstractions.Repositories;

public interface ITenantReadRepository
{
    Task<IReadOnlyList<TenantDto>> GetAllAsync(CancellationToken ct);
    Task<IReadOnlyList<TenantDto>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        CancellationToken ct);
    Task<int> CountAsync(CancellationToken ct);
}
