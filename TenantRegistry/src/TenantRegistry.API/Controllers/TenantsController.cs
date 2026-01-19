using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TenantRegistry.API.DTOs;
using TenantRegistry.Application.Tenants.Commands.ActivateTenant;
using TenantRegistry.Application.Tenants.Commands.CreateTenant;
using TenantRegistry.Application.Tenants.Queries.GetTenantById;

namespace TenantRegistry.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class TenantsController : ControllerBase
{
    private readonly CreateTenantHandler _createTenantHandler;
    private readonly ActivateTenantHandler _activateTenantHandler;
    private readonly GetTenantByIdHandler _getTenantHandler;

    public TenantsController(
        CreateTenantHandler createTenantHandler,
        ActivateTenantHandler activateTenantHandler,
        GetTenantByIdHandler getTenantHandler)
    {
        _createTenantHandler = createTenantHandler;
        _activateTenantHandler = activateTenantHandler;
        _getTenantHandler = getTenantHandler;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTenantRequest request, CancellationToken ct)
    {
        var command = new CreateTenantCommand(
            request.Name,
            request.Slug,
            request.Country,
            request.Timezone);

        var tenantId = await _createTenantHandler.Handle(command, ct);

        return CreatedAtAction(nameof(GetById), new { tenantId }, new { tenantId });
    }

    [HttpGet("{tenantId}")]
    public async Task<IActionResult> GetById(Guid tenantId, CancellationToken ct)
    {
        var query = new GetTenantByIdQuery(tenantId);
        var tenant = await _getTenantHandler.Handle(query, ct);
        return Ok(tenant);
    }

    [HttpPost("{tenantId}/activate")]
    public async Task<IActionResult> Activate(Guid tenantId, CancellationToken ct)
    {
        var command = new ActivateTenantCommand(tenantId);
        await _activateTenantHandler.Handle(command, ct);
        return NoContent();
    }
}

