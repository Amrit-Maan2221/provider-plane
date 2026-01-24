using MediatR;
using ProductCatalog.Application.Abstractions.Persistence;
using ProductCatalog.Application.Exceptions;
using ProductCatalog.Application.Features.Products.Queries.GetProductById;
using ProductCatalog.Contracts.Dtos;

public class GetProductByIdHandler
    : IRequestHandler<GetProductByIdQuery, ProductDto>
{
    private readonly IProductRepository _products;

    public GetProductByIdHandler(IProductRepository products)
    {
        _products = products;
    }

    public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken ct)
    {
        var product = await _products.GetByIdAsync(request.Id, ct);

        if (product is null)
            throw new NotFoundException("Product", request.Id);

        return product.ToDto();
    }
}
