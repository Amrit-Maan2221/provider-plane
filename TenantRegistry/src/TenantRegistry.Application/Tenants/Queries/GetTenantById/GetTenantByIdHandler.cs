using MediatR;
using TenantRegistry.Application.Abstractions.Repositories;
using TenantRegistry.Application.Common.Exceptions;
using TenantRegistry.Application.Tenants.Queries.DTOs;

namespace TenantRegistry.Application.Tenants.Queries.GetTenantById;

public class GetTenantByIdHandler : IRequestHandler<GetTenantByIdQuery, TenantDto>
{
    private readonly ITenantRepository _tenantRepository; 

    public GetTenantByIdHandler(ITenantRepository tenantRepository)
    {
        _tenantRepository = tenantRepository;
    }

    public async Task<TenantDto> Handle(GetTenantByIdQuery query, CancellationToken ct = default)
    {
        var tenant = await _tenantRepository.GetByIdAsync(query.TenantId, ct);
        if (tenant == null)
            throw new NotFoundException("Tenant not found");

        return new TenantDto(
            tenant.TenantId,
            tenant.Name,
            tenant.Slug,
            tenant.Status,
            tenant.Country,
            tenant.Timezone);
    }
}
