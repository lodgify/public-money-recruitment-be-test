using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using VacationRental.Api.Models;
using System;


namespace VacationRental.Api.Filters.Booking
{
    public class BookingNotFountFilterAttribute : Attribute, IActionFilter
    {
        private readonly string _key;
        public BookingNotFountFilterAttribute(string key)
        {
            _key = key;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            int bookingId = (int)context.ActionArguments[_key];
            var bookings = context.HttpContext.RequestServices.GetService<IDictionary<int, BookingViewModel>>();
            if (!bookings.ContainsKey(bookingId))
                throw new ApplicationException("Booking not found");
        }
    }
}
