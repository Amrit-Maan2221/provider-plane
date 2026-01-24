using MediatR;
using ProductCatalog.Application.Abstractions.Persistence;
using ProductCatalog.Application.Exceptions;
using ProductCatalog.Domain.Entities;

namespace ProductCatalog.Application.Features.Products.Commands.CreateProduct;

public class CreateProductHandler
    : IRequestHandler<CreateProductCommand, Guid>
{
    private readonly IProductRepository _products;
    private readonly IEventPublisher _publisher;

    public CreateProductHandler(IProductRepository products,
        IEventPublisher publisher)
    {
        _products = products;
        _publisher = publisher;
    }

    public async Task<Guid> Handle(CreateProductCommand request, CancellationToken ct)
    {
        var product = new Product(
            code: request.Code,
            name:  request.Name,
            description: request.Description,
            category: request.Category
        );

        if (await _products.ExistsByCodeAsync(request.Code))
            throw new InvalidOperationException(request.Code);


        await _products.AddAsync(product, ct);

        foreach (var domainEvent in product.DomainEvents)
            await _publisher.PublishAsync(domainEvent, ct);

        product.ClearDomainEvents();

        return product.ProductId;
    }
}
