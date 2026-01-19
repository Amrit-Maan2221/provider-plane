namespace TenantRegistry.Domain.Entities;

public class TenantContact
{
    public Guid ContactId { get; private set; }
    public Guid TenantId { get; private set; }

    public string Name { get; private set; } = default!;
    public string Email { get; private set; } = default!;
    public string? Phone { get; private set; }
    public string Role { get; private set; } = default!;

    private TenantContact() { }

    public static TenantContact Create(
        Guid tenantId,
        string name,
        string email,
        string role,
        string? phone = null)
    {
        return new TenantContact
        {
            ContactId = Guid.NewGuid(),
            TenantId = tenantId,
            Name = name,
            Email = email,
            Role = role,
            Phone = phone
        };
    }
}
