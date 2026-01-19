namespace TenantRegistry.Contracts.Events;

public record TenantCreatedEvent(Guid TenantId, string Slug);
