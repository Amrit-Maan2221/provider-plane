using FluentValidation;
using System.Net;
using TenantRegistry.Application.Common.Exceptions;
using TenantRegistry.Domain.Exceptions;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning(ex,
                "Resource not found. Path: {Path}, Method: {Method}",
                context.Request.Path,
                context.Request.Method);

            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            await context.Response.WriteAsJsonAsync(new
            {
                error = ex.Message
            });
        }
        catch (DomainException ex)
        {
            _logger.LogWarning(ex,
                "Domain validation error. Path: {Path}, Method: {Method}",
                context.Request.Path,
                context.Request.Method);

            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await context.Response.WriteAsJsonAsync(new
            {
                error = ex.Message
            });
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning(ex,
                "Validation failed. Path: {Path}, Method: {Method}",
                context.Request.Path,
                context.Request.Method);

            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            await context.Response.WriteAsJsonAsync(new
            {
                error = ex.Errors.Select(e => e.ErrorMessage)
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Unhandled exception. Path: {Path}, Method: {Method}, TraceId: {TraceId}",
                context.Request.Path,
                context.Request.Method,
                context.TraceIdentifier);

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsJsonAsync(new
            {
                error = "An unexpected error occurred.",
                traceId = context.TraceIdentifier
            });
        }
    }
}
