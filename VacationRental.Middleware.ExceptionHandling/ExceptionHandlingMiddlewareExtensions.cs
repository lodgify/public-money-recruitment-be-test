using Microsoft.AspNetCore.Builder;

namespace VacationRental.Middleware.ExceptionHandling;

/// <summary>
/// Used to connect <see cref="ExceptionHandlingMiddleware"> ExceptionHandlingMiddleware </see> to an ASP Net Core Web API application.
/// </summary>
public static class ExceptionHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandling(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}
