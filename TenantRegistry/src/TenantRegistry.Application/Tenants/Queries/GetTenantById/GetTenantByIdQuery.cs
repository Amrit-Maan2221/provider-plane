using MediatR;
using TenantRegistry.Application.Tenants.Queries.DTOs;

namespace TenantRegistry.Application.Tenants.Queries.GetTenantById;

public record GetTenantByIdQuery(Guid TenantId) : IRequest<TenantDto>;
