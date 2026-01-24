using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductCatalog.Application.Abstractions.Persistence;
using ProductCatalog.Infrastructure.Messaging;
using ProductCatalog.Infrastructure.Persistence.Connections;
using ProductCatalog.Infrastructure.Persistence.Repositories;

namespace ProductCatalog.Infrastructure;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,IConfiguration configuration)
    {
        // ----------------------------
        // Database / Dapper
        // ----------------------------
        services.AddScoped<IDbConnectionFactory>(_ =>new SqlConnectionFactory(configuration.GetConnectionString("ProductCatalogDb")!));

        // ----------------------------
        // Repositories
        // ----------------------------
        services.AddScoped<IProductRepository, ProductRepository>();

        // ----------------------------
        // Messaging (Domain Events / Integration)
        // ----------------------------
        services.AddScoped<IEventPublisher, EventPublisher>();

        return services;
    }
}
