using TenantRegistry.Domain.Enums;

namespace TenantRegistry.Application.Tenants.Queries.DTOs;

public record TenantDto(
    Guid TenantId,
    string Name,
    string Slug,
    string Country,
    string Timezone
);
