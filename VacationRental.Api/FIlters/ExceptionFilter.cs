namespace VacationRental.Api.Filters
{
    using System.ComponentModel.DataAnnotations;
    using System.Net;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.Logging;

    public class ExceptionFilter : IExceptionFilter
    {
        public const string ErrorMessage = "An unhandled exception was thrown by the application.";

        public void OnException(ExceptionContext context)
        {
            if (context?.Exception == null)
            {
                return;
            }

            var loggerFactory = (ILoggerFactory)context.HttpContext.RequestServices?.GetService(typeof(ILoggerFactory));
            if (loggerFactory != null)
            {
                var logger = loggerFactory.CreateLogger(typeof(ExceptionFilter));
                logger.LogError(context.Exception, ErrorMessage);
            }

            var message = "Something went wrong. Please try again later.";
            var statusCode = HttpStatusCode.InternalServerError;

            if (context.Exception is ValidationException)
            {
                statusCode = HttpStatusCode.BadRequest;
                message = context.Exception.Message;
            }

            context.Result = new JsonResult(message)
            {
                StatusCode = (int)statusCode,
            };

            context.ExceptionHandled = true;
        }
    }
}
