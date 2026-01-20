using TenantRegistry.Domain.Entities;

namespace TenantRegistry.Application.TenantSettings.Commands;

public record UpdateTenantSettingsCommand(
    Guid TenantId,
    IEnumerable<TenantSetting> Settings);