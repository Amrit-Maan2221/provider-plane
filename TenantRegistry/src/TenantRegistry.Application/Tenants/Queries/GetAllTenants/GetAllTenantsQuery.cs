using MediatR;
using TenantRegistry.Application.Tenants.Queries.DTOs;

namespace TenantRegistry.Application.Tenants.Queries.GetAllTenants;

public sealed record GetAllTenantsQuery
    : IRequest<IReadOnlyList<TenantDto>>;
