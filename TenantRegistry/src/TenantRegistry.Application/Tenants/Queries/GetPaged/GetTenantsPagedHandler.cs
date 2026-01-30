using MediatR;
using TenantRegistry.Application.Abstractions.Repositories;
using TenantRegistry.Application.Tenants.Queries.DTOs;
using TenantRegistry.Contracts.Dtos;

namespace TenantRegistry.Application.Tenants.Queries.GetTenantsPaged;

public sealed class GetTenantsPagedHandler 
    : IRequestHandler<GetTenantsPagedQuery, PagedResult<TenantDto>>
{
    private readonly ITenantReadRepository _repository;

    public GetTenantsPagedHandler(ITenantReadRepository repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<TenantDto>> Handle(
        GetTenantsPagedQuery query,
        CancellationToken ct)
    {
        var totalCount = await _repository.CountAsync(ct);

        var items = await _repository.GetPagedAsync(
            query.PageNumber,
            query.PageSize,
            ct);

        return new PagedResult<TenantDto>
        {
            Items = items,
            PageNumber = query.PageNumber,
            PageSize = query.PageSize,
            TotalCount = totalCount
        };
    }
}
