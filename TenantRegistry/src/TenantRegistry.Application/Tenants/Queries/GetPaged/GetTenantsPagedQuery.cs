using MediatR;
using TenantRegistry.Application.Tenants.Queries.DTOs;
using TenantRegistry.Contracts.Dtos;

namespace TenantRegistry.Application.Tenants.Queries.GetTenantsPaged;

public record GetTenantsPagedQuery(
    int PageNumber,
    int PageSize
) : IRequest<PagedResult<TenantDto>>;
