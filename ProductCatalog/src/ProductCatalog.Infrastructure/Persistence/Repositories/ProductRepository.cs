using Dapper;
using ProductCatalog.Application.Abstractions.Persistence;
using ProductCatalog.Domain.Entities;
using ProductCatalog.Domain.Enums;
using ProductCatalog.Infrastructure.Persistence.Connections;

namespace ProductCatalog.Infrastructure.Persistence.Repositories;


public class ProductRepository : IProductRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public ProductRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task AddAsync(Product product, CancellationToken ct = default)
    {
        const string sql = @"
            INSERT INTO Products
            (ProductId, Code, Name, Category, Description, Status)
            VALUES
            (@ProductId, @Code, @Name, @Category, @Description, @Status)";

        using var connection = _connectionFactory.Create();
        await connection.QuerySingleAsync(sql, product);
    }

    public async Task<Product?> GetByIdAsync(Guid productId, CancellationToken ct = default)
    {
        const string sql = @"
            SELECT * FROM Products WHERE ProductId = @ProductId";

        using var connection = _connectionFactory.Create();

        Product? row = await connection.QuerySingleOrDefaultAsync<Product>(sql, new { ProductId = productId });

        return row;
    }


    public async Task<bool> ExistsByCodeAsync(string code)
    {
        const string sql = @"
            SELECT COUNT(*) FROM Products WHERE Code = @Code";

        using var connection = _connectionFactory.Create();
        int count = await connection.QuerySingleAsync<int>(sql, new { Code = code });
        return count > 0;
    }
}