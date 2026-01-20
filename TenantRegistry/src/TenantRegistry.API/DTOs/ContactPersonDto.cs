namespace TenantRegistry.API.DTOs;

public class ContactPersonDto
{
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public bool IsPrimary { get; set; }
}
