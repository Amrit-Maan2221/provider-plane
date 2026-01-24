using Scalar.AspNetCore;
using ProductCatalog.Application;
using ProductCatalog.Infrastructure;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddValidatorsFromAssembly(typeof(ProductCatalog.Application.DependencyInjection).Assembly);
builder.Services.AddApplication();

var app = builder.Build();
app.MapOpenApi();
app.MapScalarApiReference("/docs");

app.UseAuthorization();


app.MapControllers();

app.Run();