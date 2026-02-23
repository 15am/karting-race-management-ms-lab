using System.Text.Json;
using Karting.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Karting.Api.Middleware;

public sealed class GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (DomainRuleException exception)
        {
            logger.LogWarning(exception, "Domain validation failure.");
            await WriteProblemAsync(context, StatusCodes.Status400BadRequest, "Validation error", exception.Message);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Unhandled exception.");
            await WriteProblemAsync(context, StatusCodes.Status500InternalServerError, "Unexpected error", "An unexpected error occurred.");
        }
    }

    private static async Task WriteProblemAsync(HttpContext context, int statusCode, string title, string detail)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/problem+json";

        var payload = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Type = "about:blank",
            Instance = context.Request.Path,
        };

        await JsonSerializer.SerializeAsync(context.Response.Body, payload);
    }
}
