namespace TenantRegistry.Domain.Entities;

public class TenantContact
{
   public Guid Id { get; private set; }
    public string Name { get; private set; } = default!;
    public string Email { get; private set; } = default!;
    public string Phone { get; private set; } = default!;
    public bool IsPrimary { get; private set; }

    protected TenantContact() { } // EF

    public TenantContact(string name, string email, string phone, bool isPrimary)
    {
        Name = name;
        Email = email;
        Phone = phone;
        IsPrimary = isPrimary;
    }
}
