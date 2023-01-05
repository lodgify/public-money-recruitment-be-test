using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;
using VacationRental.Api.Errors;

namespace VacationRental.Api.Middlewares
{

    public class ExceptionHandlingMiddleware : IMiddleware
    {
        public ExceptionHandlingMiddleware()
        {
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/json";
                var statusCode = (int)HttpStatusCode.InternalServerError;                
                var resultMessage = string.Empty;               

                context.Response.StatusCode = statusCode;

                if (string.IsNullOrEmpty(resultMessage))
                    resultMessage = JsonConvert.SerializeObject(new CodeErrorException(statusCode, ex.Message, ex.StackTrace));

                await context.Response.WriteAsync(resultMessage);
            }
        }
    }

}
