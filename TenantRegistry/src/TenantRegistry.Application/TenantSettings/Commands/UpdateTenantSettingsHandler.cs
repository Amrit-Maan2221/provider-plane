using System;
using TenantRegistry.Application.Abstractions.Repositories;
using TenantRegistry.Application.Common.Exceptions;

namespace TenantRegistry.Application.TenantSettings.Commands;

public class UpdateTenantSettingsHandler
{
    private readonly ITenantRepository _repo;

    public UpdateTenantSettingsHandler(ITenantRepository repo)
    {
        _repo = repo;
    }

    public async Task Handle(UpdateTenantSettingsCommand command, CancellationToken ct)
    {
        var tenant = await _repo.GetByIdAsync(command.TenantId, ct)
            ?? throw new NotFoundException("Tenant not found");

        tenant.ReplaceSettings(command.Settings);

        await _repo.SaveChangesAsync(ct);
    }
}
