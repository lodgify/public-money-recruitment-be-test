using LanguageExt;
using Microsoft.AspNetCore.Mvc;
using System;

namespace VacationRental.Api.Extensions
{
    public static class ControllerExtensions
    {
        public static IActionResult ToOk<TResult, TContract>(this Result<TResult> result, Func<TResult, TContract> mapper)
        {
            return result.Match<IActionResult>(obj =>
            {
                var response = mapper(obj);
                if (response == null)
                    return new NotFoundResult();

                return new OkObjectResult(response);
            }, exception =>
            {
                if (exception is ApplicationException validationException)
                {
                    if (validationException.Message.Contains("not found"))
                    {
                        return new NotFoundResult();
                    }

                    if (validationException.Message.Contains("not acceptable"))
                    {
                        return new StatusCodeResult(406);
                    }

                    return new BadRequestObjectResult(validationException.Message);
                }

                return new StatusCodeResult(500);
            });
        }
    }
}
