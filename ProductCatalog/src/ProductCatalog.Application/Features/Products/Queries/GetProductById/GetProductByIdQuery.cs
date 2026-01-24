using MediatR;
using ProductCatalog.Contracts.Dtos;

namespace ProductCatalog.Application.Features.Products.Queries.GetProductById;

public record GetProductByIdQuery(Guid Id)
    : IRequest<ProductDto>;
