using System;
using System.Linq;
using VacationRental.Api.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Error = VacationRental.Api.ApplicationErrors.ErrorMessages;


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
                throw new ApplicationException(Error.BookingNightsZero);
            if(request.RentalId <= 0)
                throw new ApplicationException(Error.RentalIdLessOrZero);
            if(request.Start < DateTime.Now.AddDays(-1))
                throw new ApplicationException(Error.DateAlreadyPassed);

            var rentals = context.HttpContext.RequestServices.GetService<IDictionary<int, RentalViewModel>>();
            if (!rentals.ContainsKey(request.RentalId))
                throw new ApplicationException(Error.RentalNotFound);

            int rentalUnits = rentals[request.RentalId].Units;
            int preparationTimeInDays = rentals[request.RentalId].PreparationTimeInDays;
            
            var bookings = context.HttpContext.RequestServices.GetService<IDictionary<int, BookingViewModel>>();
            var rentalBookings = bookings.Where(x => x.Value.RentalId == request.RentalId).ToList();
            
            if (rentalUnits > rentalBookings.Count())
                return;

            int occupiedRentalUnitsCount = 0;
            foreach (var booking in rentalBookings)
            {
                DateTime requestedStart = request.Start;
                DateTime requestedEnd = request.Start.AddDays(request.Nights);
                DateTime bookingStart = booking.Value.Start;
                DateTime bookingEnd = booking.Value.Start
                    .AddDays(booking.Value.Nights)
                    .AddDays(preparationTimeInDays);

                if  (
                    !(requestedStart > bookingEnd && requestedEnd > bookingEnd) &&
                    !(requestedStart < bookingStart && requestedEnd < bookingStart)
                    )
                {
                    occupiedRentalUnitsCount += 1;
                }
            }
            if (occupiedRentalUnitsCount >= rentalUnits)
                throw new ApplicationException(Error.NoVacancy);
        }
    }
}
