using TenantRegistry.Application.Abstractions.Messaging;
using TenantRegistry.Application.Abstractions.Repositories;
using TenantRegistry.Contracts.Events;
using TenantRegistry.Domain.Entities;

namespace TenantRegistry.Application.Tenants.Commands.CreateTenant;

public class CreateTenantHandler
{
    private readonly ITenantRepository _tenantRepository;
    private readonly IEventPublisher _eventPublisher;

    public CreateTenantHandler(
        ITenantRepository tenantRepository,
        IEventPublisher eventPublisher)
    {
        _tenantRepository = tenantRepository;
        _eventPublisher = eventPublisher;
    }

    public async Task<Guid> Handle(CreateTenantCommand command, CancellationToken ct = default)
    {
        var existingTenant = await _tenantRepository.GetBySlugAsync(command.Slug, ct);
        if (existingTenant != null)
            throw new InvalidOperationException("Tenant slug already exists");

        var tenant = Tenant.Create(
            command.Name,
            command.Slug,
            command.Country,
            command.Timezone);

        await _tenantRepository.AddAsync(tenant, ct);

        await _eventPublisher.PublishAsync(
            new TenantCreatedEvent(tenant.TenantId, tenant.Slug),
            ct);

        return tenant.TenantId;
    }
}
