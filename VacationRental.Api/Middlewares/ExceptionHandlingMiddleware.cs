using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;
using VacationRental.Api.Errors;
using VacationRental.Application.Exceptions;

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

                switch (ex)
                {
                    case NotFoundException:
                        statusCode = (int)HttpStatusCode.NotFound;
                        resultMessage= ex.Message;
                        break;
                    case ValidationException:
                        var validationException = ex as ValidationException;
                        statusCode = (int)HttpStatusCode.BadRequest;
                        var validationJson = JsonConvert.SerializeObject(validationException?.Errors);
                        resultMessage = JsonConvert.SerializeObject(new CodeErrorException(statusCode, validationException?.Error.Message, validationJson));
                        break;
                    case ConflictException:
                        var conflictException = ex as ConflictException;
                        statusCode = (int)HttpStatusCode.Conflict;
                        resultMessage = JsonConvert.SerializeObject(new CodeErrorException(statusCode, conflictException?.Error.Message, null));
                        break;
                }

                context.Response.StatusCode = statusCode;

                if (string.IsNullOrEmpty(resultMessage))
                    resultMessage = JsonConvert.SerializeObject(new CodeErrorException(statusCode, ex.Message, ex.StackTrace));

                await context.Response.WriteAsync(resultMessage);
            }
        }
    }
}
