using MediatR;
using ProductCatalog.Domain.Enums;

namespace ProductCatalog.Application.Features.Products.Commands.CreateProduct;

public record CreateProductCommand(
    string Name,
    string Code,
    string Category,
    string Description
) : IRequest<Guid>;
