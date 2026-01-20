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

var app = builder.Build();
app.MapOpenApi();
app.MapScalarApiReference("/docs");
app.MapControllers();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.Run();