using TenantRegistry.Domain.Enums;

namespace TenantRegistry.Application.Tenants.Queries.DTOs;

public record TenantDto(
    Guid TenantId,
    string Name,
    string Slug,
    TenantStatus Status,
    string Country,
    string Timezone
);
