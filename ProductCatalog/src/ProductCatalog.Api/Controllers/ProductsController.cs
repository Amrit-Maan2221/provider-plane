using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Application.Features.Products.Commands.CreateProduct;
using ProductCatalog.Application.Features.Products.Queries.GetProductById;
using ProductCatalog.Contracts.Dtos;

namespace ProductCatalog.Api.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // POST /api/products
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        [FromBody] CreateProductCommand command,
        CancellationToken ct)
    {
        var productId = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = productId }, productId);
    }

    // GET /api/products/{id}
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProductDto>> GetById(
        Guid id,
        CancellationToken ct)
    {
        var product = await _mediator.Send(
            new GetProductByIdQuery(id), ct);

        return Ok(product);
    }
}
