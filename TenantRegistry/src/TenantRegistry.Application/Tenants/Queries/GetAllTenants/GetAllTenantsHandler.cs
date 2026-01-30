using MediatR;
using TenantRegistry.Application.Abstractions.Repositories;
using TenantRegistry.Application.Tenants.Queries.DTOs;

namespace TenantRegistry.Application.Tenants.Queries.GetAllTenants;

public sealed class GetAllTenantsHandler : IRequestHandler<GetAllTenantsQuery, IReadOnlyList<TenantDto>>
{
    private readonly ITenantReadRepository _repository;

    public GetAllTenantsHandler(ITenantReadRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<TenantDto>> Handle(
        GetAllTenantsQuery query,
        CancellationToken ct)
    {
        return await _repository.GetAllAsync(ct);
    }
}
