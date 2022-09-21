using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text;
using System.Text.Json;

namespace VacationRental.Middleware.ExceptionHandling;

internal class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var endpoint = context.GetEndpoint();
        var handleExceptionsOptions = endpoint?.Metadata.GetMetadata<HandleExceptionsAttribute>();
        if (handleExceptionsOptions is null)
        {
            await _next(context);
            return;
        }

        try
        {
            await _next(context);
        }
        catch (ApplicationException ex)
        {
            _logger.LogInformation(ex, $"BadRequest at {context.Request.Path}. Request details: {JsonSerializer.Serialize(await GetRequestBody(context))}");

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await context.Response.WriteAsync(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Exception at {context.Request.Path}. Request details: {JsonSerializer.Serialize(await GetRequestBody(context))}");

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        }
    }

    private async Task<string> GetRequestBody(HttpContext context)
    {
        context.Request.EnableBuffering();

        using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, true, 1024, true);
        var bodyStr = await reader.ReadToEndAsync();

        context.Request.Body.Position = 0;

        return bodyStr;
    }
}