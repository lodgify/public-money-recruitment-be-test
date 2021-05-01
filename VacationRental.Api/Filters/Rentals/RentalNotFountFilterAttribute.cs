using System;
using VacationRental.Api.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace VacationRental.Api.Filters.Rentals
{
    public class RentalNotFountFilterAttribute : Attribute, IActionFilter
    {
        private readonly string _key;

        public RentalNotFountFilterAttribute(string key)
        {
            _key = key;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            int rentalId = (int)context.ActionArguments[_key];
            var rentals = context.HttpContext.RequestServices.GetService<IDictionary<int, RentalViewModel>>();

            if(!rentals.ContainsKey(rentalId))
                throw new ApplicationException("Rental not found");
        }
    }
}
