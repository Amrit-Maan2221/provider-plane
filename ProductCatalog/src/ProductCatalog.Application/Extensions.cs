using ProductCatalog.Contracts.Dtos;

namespace ProductCatalog.Application.Exceptions;

public static class Extensions
{
    public static ProductDto ToDto(this Domain.Entities.Product product)
    {
        return new ProductDto(product.ProductId, product.Code, product.Name, product.Category, product.Description, product.Status.ToString());
    }
}