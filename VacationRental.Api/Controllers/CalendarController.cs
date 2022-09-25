using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/calendar")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        private readonly IDictionary<int, RentalViewModel> _rentals;
        private readonly IDictionary<int, BookingViewModel> _bookings;

        public CalendarController(
            IDictionary<int, RentalViewModel> rentals,
            IDictionary<int, BookingViewModel> bookings)
        {
            _rentals = rentals;
            _bookings = bookings;
        }

        [HttpGet]
        public CalendarViewModel Get(int rentalId, DateTime start, int nights)
        {
            if (nights < 0)
                throw new ApplicationException("Nights must be positive");
            if (!_rentals.ContainsKey(rentalId))
                throw new ApplicationException("Rental not found");

            var result = new CalendarViewModel
            {
                RentalId = rentalId,
                Dates = new List<CalendarDateViewModel>()
            };

            // looks at each night from the start date
            for (var i = 0; i < nights; i++)
            {
                var date = new CalendarDateViewModel
                {
                    Date = start.Date.AddDays(i),
                    Bookings = new List<CalendarBookingViewModel>()
                };

                foreach (var booking in _bookings.Values)
                {
                    // finds the booking
                    if (booking.RentalId == rentalId
                        // checks if booking started before or on current date  AND  if booking ends after current date 
                        && booking.Start <= date.Date && booking.Start.AddDays(booking.Nights) > date.Date)
                    {
                        // if so, adds the booking ID to the calendar booking viewing model
                        date.Bookings.Add(new CalendarBookingViewModel { Id = booking.Id, isCleaning = false });
                    }
                    // does the same thing by checking if booking is in the afterward cleaning phase
                    else if (booking.RentalId == rentalId
                        && booking.Start.AddDays(booking.Nights) <= date.Date && booking.Start.AddDays(booking.Nights + _rentals[booking.RentalId].PreparationTimeInDays) > date.Date)
                    {
                        date.Bookings.Add(new CalendarBookingViewModel { Id = booking.Id, isCleaning = true });
                    }
                }
                // adds all the date bookings to the calendar viewing model
                result.Dates.Add(date);
            }

            return result;
        }
    }
}