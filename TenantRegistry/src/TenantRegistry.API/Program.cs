using FluentValidation;
using Scalar.AspNetCore;
using TenantRegistry.Application;
using TenantRegistry.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddOpenApi();
builder.Services.AddApplication();
builder.Services.AddValidatorsFromAssembly(typeof(TenantRegistry.Application.DependencyInjection).Assembly);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:5173" // Vite default
                // add prod domains later
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();
app.UseCors("AllowFrontend"); 
app.MapOpenApi();
app.MapScalarApiReference("/docs");
app.MapControllers();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.Run();