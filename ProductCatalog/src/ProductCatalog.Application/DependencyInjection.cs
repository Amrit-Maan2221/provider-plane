using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using ProductCatalog.Application.Behaviours;



namespace ProductCatalog.Application;
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(
                Assembly.GetExecutingAssembly()
            );
        });

        services.AddTransient(
            typeof(IPipelineBehavior<,>),
            typeof(ValidationBehaviour<,>)
        );
        return services;
    }
}
