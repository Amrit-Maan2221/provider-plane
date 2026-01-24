using Microsoft.Data.SqlClient;
using System.Data;

namespace ProductCatalog.Infrastructure.Persistence.Connections;

public class SqlConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public SqlConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection Create()
    {
        return new SqlConnection(_connectionString);
    }
}
