namespace TenantRegistry.API.DTOs;

public class CreateTenantRequest
{
    public string Name { get; set; } = default!;
    public string Slug { get; set; } = default!;
    public string Country { get; set; } = default!;
    public string Timezone { get; set; } = default!;
}
