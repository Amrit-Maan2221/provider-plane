using MediatR;
using Microsoft.AspNetCore.Mvc;
using TenantRegistry.API.DTOs;
using TenantRegistry.Application.Tenants.Commands.ActivateTenant;
using TenantRegistry.Application.Tenants.Commands.CreateTenant;
using TenantRegistry.Application.Tenants.Queries.GetAllTenants;
using TenantRegistry.Application.Tenants.Queries.GetTenantById;
using TenantRegistry.Application.Tenants.Queries.GetTenantsPaged;

namespace TenantRegistry.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TenantsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TenantsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var query = new GetAllTenantsQuery();
        var tenants = await _mediator.Send(query, ct);
        return Ok(tenants);
    }

    [HttpGet("{tenantId:guid}")]
    public async Task<IActionResult> GetById(Guid tenantId, CancellationToken ct)
    {
        var query = new GetTenantByIdQuery(tenantId);
        var tenant = await _mediator.Send(query, ct);
        return Ok(tenant);
    }

    [HttpGet("paged")]
    public async Task<IActionResult> GetPaged(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken ct = default)
    {
        var query = new GetTenantsPagedQuery(pageNumber, pageSize);
        var result = await _mediator.Send(query, ct);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateTenantRequest request,
        CancellationToken ct)
    {
        var command = new CreateTenantCommand(
            request.Name,
            request.Slug,
            request.Country,
            request.Timezone,
            request.Contacts.Select(c =>
                new CreateTenantContactCommand(
                    c.Name,
                    c.Email,
                    c.Phone,
                    c.IsPrimary
                )
            ).ToList()
        );

        var tenantId = await _mediator.Send(command, ct);

        return CreatedAtAction(
            nameof(GetById),
            new { tenantId },
            new { tenantId }
        );
    }

    // [HttpPost("{tenantId:guid}/activate")]
    // public async Task<IActionResult> Activate(Guid tenantId, CancellationToken ct)
    // {
    //     var command = new ActivateTenantCommand(tenantId);
    //     await _mediator.Send(command, ct);
    //     return NoContent();
    // }
}
