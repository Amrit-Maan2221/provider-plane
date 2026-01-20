namespace TenantRegistry.Application.Tenants.DTOs;

public sealed class TenantListDto
{
    public Guid TenantId { get; init; }
    public string Name { get; init; } = default!;
    public string Slug { get; init; } = default!;
    public string Country { get; init; } = default!;
    public string Status { get; init; } = default!;
    public DateTime CreatedAt { get; init; }
}
