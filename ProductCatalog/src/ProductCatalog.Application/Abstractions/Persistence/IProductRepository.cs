using ProductCatalog.Domain.Entities;

namespace ProductCatalog.Application.Abstractions.Persistence;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(Guid id, CancellationToken ct);
    Task AddAsync(Product product, CancellationToken ct);
    Task<bool> ExistsByCodeAsync(string code);
}
