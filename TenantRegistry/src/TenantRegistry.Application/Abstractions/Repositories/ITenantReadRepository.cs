using TenantRegistry.Application.Tenants.DTOs;

namespace TenantRegistry.Application.Abstractions.Repositories;

public interface ITenantReadRepository
{
    Task<IReadOnlyList<TenantListDto>> GetAllAsync(CancellationToken ct);
}
