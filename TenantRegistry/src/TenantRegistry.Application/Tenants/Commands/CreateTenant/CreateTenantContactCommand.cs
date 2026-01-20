namespace TenantRegistry.Application.Tenants.Commands.CreateTenant;

public record CreateTenantContactCommand(
    string Name,
    string Email,
    string Phone,
    bool IsPrimary
);
