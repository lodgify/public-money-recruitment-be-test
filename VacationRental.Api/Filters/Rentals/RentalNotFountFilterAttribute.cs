using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using VacationRental.Api.Models;

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
            var rentals = context.HttpContext.RequestServices.GetService<IDictionary<int, RentalViewModel>>();
            int rentalId = (int)context.ActionArguments[_key];

            if(!rentals.ContainsKey(rentalId))
                throw new ApplicationException("Rental not found");
        }
    }
}
