using MediatR;
using TenantRegistry.Application.Tenants.DTOs;

namespace TenantRegistry.Application.Tenants.Queries.GetAllTenants;

public sealed record GetAllTenantsQuery
    : IRequest<IReadOnlyList<TenantListDto>>;
