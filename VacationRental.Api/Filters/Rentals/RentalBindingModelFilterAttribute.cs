using System;
using VacationRental.Api.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Filters;
using Error = VacationRental.Api.ApplicationErrors.ErrorMessages;
using Microsoft.Extensions.DependencyInjection;

namespace VacationRental.Api.Filters.Rentals
{
    public class RentalBindingModelFilterAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var request = context.ActionArguments["model"] as RentalBindingModel;
            if (request.Units <= 0)
                throw new ApplicationException(Error.RentalUnitsZero);
            if (request.PreparationTimeInDays < 0)
                throw new ApplicationException(Error.PreparationTimeLessZero);
        }
    }
}
