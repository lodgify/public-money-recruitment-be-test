using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace VacationRental.Api.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ApplicationException error)
            {
                var response = context.Response;
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                await response.WriteAsync(error.Message);
            }
            catch (Exception)
            {
                var response = context.Response;
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
        }
    }
}
