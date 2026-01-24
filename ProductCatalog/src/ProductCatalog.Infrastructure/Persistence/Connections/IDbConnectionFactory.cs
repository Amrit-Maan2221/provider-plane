using System.Data;

namespace ProductCatalog.Infrastructure.Persistence.Connections;

public interface IDbConnectionFactory
{
    IDbConnection Create();
}
