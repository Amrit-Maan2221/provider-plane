using MediatR;
using TenantRegistry.Application.Abstractions.Messaging;
using TenantRegistry.Application.Abstractions.Repositories;
using TenantRegistry.Application.Common.Exceptions;
using TenantRegistry.Contracts.Events;

namespace TenantRegistry.Application.Tenants.Commands.ActivateTenant;

public class ActivateTenantHandler : IRequestHandler<ActivateTenantCommand, Unit>
{
    private readonly ITenantRepository _tenantRepository;
    private readonly IEventPublisher _eventPublisher;

    public ActivateTenantHandler(
        ITenantRepository tenantRepository,
        IEventPublisher eventPublisher)
    {
        _tenantRepository = tenantRepository;
        _eventPublisher = eventPublisher;
    }

    public async Task<Unit> Handle(ActivateTenantCommand command, CancellationToken ct = default)
    {
        var tenant = await _tenantRepository.GetByIdAsync(command.TenantId, ct);
        if (tenant == null)
            throw new NotFoundException("Tenant not found");

        tenant.Activate();

        await _tenantRepository.UpdateAsync(tenant, ct);

        await _eventPublisher.PublishAsync(
            new TenantActivatedEvent(command.TenantId),
            ct);

        return Unit.Value;
    }
}
