using LanguageExt;
using Microsoft.AspNetCore.Mvc;
using System;
using VacationRental.Api.Core.Helpers.Exceptions;

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
                if(exception is NotFoundException rentalNotFoundException)
                    return new NotFoundResult();

                if (exception is UpdateFailedException rentalUpdateFailException)
                    return new StatusCodeResult(304); // Not Modified

                if (exception is NotAvailableException notAvailableException)
                    return new StatusCodeResult(409); // Conflict 

                if (exception is RentOverlappedException rentOverlappedException)
                    return new BadRequestObjectResult(rentOverlappedException.Message);

                if(exception is ApplicationException validationException)
                    return new BadRequestObjectResult(validationException.Message);

                return new StatusCodeResult(500); // Internal Server Error
            });
        }
    }
}
