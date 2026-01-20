using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TenantRegistry.Application.Abstractions.Messaging;
using TenantRegistry.Application.Abstractions.Repositories;
using TenantRegistry.Infrastructure.Messaging;
using TenantRegistry.Infrastructure.Persistence.DbContext;
using TenantRegistry.Infrastructure.Persistence.Repositories;

namespace TenantRegistry.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<TenantRegistryDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("TenantRegistryDb")));

        services.AddScoped<ITenantRepository, TenantRepository>();
        services.AddScoped<ITenantReadRepository, TenantReadRepository>();
        services.AddScoped<IEventPublisher, InMemoryEventPublisher>();

        return services;
    }
}
