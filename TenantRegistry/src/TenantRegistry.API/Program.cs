using FluentValidation;
using FluentValidation.AspNetCore;
using Scalar.AspNetCore;
using TenantRegistry.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddOpenApi();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateTenantRequestValidator>();


var app = builder.Build();
app.MapOpenApi();
app.MapScalarApiReference("/docs");
app.MapControllers();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.Run();