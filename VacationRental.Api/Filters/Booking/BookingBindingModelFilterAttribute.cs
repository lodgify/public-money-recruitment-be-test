using System;
using System.Linq;
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
                throw new ApplicationException($"{request.Start.ToString("dd MMMM yyyy")} already passed. Please choose another date.");

            var rentals = context.HttpContext.RequestServices.GetService<IDictionary<int, RentalViewModel>>();
            if (!rentals.ContainsKey(request.RentalId))
                throw new ApplicationException("Rental not found");

            var bookings = context.HttpContext.RequestServices.GetService<IDictionary<int, BookingViewModel>>();
            var currentRentalBookings = bookings
                .Where(x => x.Value.RentalId == request.RentalId)
                .ToList();
            
            int allRentalUnitsCount = rentals[request.RentalId].Units;
            int allRentalBookingsCount = currentRentalBookings.Count();
            int freeRental = allRentalUnitsCount - allRentalBookingsCount;

            int occupiedRentalUnitsCount = 0;
            if (freeRental == 0) 
            {
                foreach (var booking in currentRentalBookings)
                {
                    bool avaliable = false;
                    DateTime bookingStart = booking.Value.Start;
                    DateTime bookingEnd = booking.Value.Start.AddDays(booking.Value.Nights);
                    DateTime requestedStart = request.Start;
                    DateTime requestedEnd = request.Start.AddDays(request.Nights);

                    if ( bookingEnd < requestedStart || bookingStart > requestedEnd)
                    {
                        avaliable = true;
                    }

                    if (avaliable)
                        break;
                    else 
                    {
                        occupiedRentalUnitsCount += 1;
                    }
                }
                if (occupiedRentalUnitsCount == allRentalUnitsCount)
                {
                    string startDate = request.Start.ToString("dd MMMM yyyy");
                    string unitFreeDate = request.Start.AddDays(request.Nights).ToString("dd MMMM yyyy");
                    throw new ApplicationException($"No Vacancy since {startDate} till {unitFreeDate}");
                }
            }
        }
    }
}
