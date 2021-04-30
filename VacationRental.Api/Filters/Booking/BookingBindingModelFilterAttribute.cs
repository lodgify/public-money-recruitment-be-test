using System;
using VacationRental.Api.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;


namespace VacationRental.Api.Filters.Booking
{
    public class BookingBindingModelFilterAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var request = context.ActionArguments["model"] as BookingBindingModel;
            if(request.Nights <= 0)
                throw new ApplicationException("Nigts must be positive");
            if(request.RentalId <= 0)
                throw new ApplicationException("RentalId must be positive");
            if(request.Start < DateTime.Now.AddDays(-1))
                throw new ApplicationException($"{request.Start} have already passed. Please choose another date.");

            var rentals = context.HttpContext.RequestServices.GetService<IDictionary<int, RentalViewModel>>();
            if (!rentals.ContainsKey(request.RentalId))
                throw new ApplicationException("Rental not found");
        }
    }
}
