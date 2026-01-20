using MediatR;

namespace TenantRegistry.Application.Tenants.Commands.CreateTenant;

public record CreateTenantCommand(
    string Name,
    string Slug,
    string Country,
    string Timezone,
    IReadOnlyList<CreateTenantContactCommand> Contacts
) : IRequest<Guid>;
