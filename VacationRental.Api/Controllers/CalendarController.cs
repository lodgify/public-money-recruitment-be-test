using System;
using System.Collections.Generic;
using System.Linq;
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

            var rental = _rentals[rentalId];
            var rentalBookings = _bookings.Values.Where(b => b.RentalId == rentalId);
            var result = new CalendarViewModel 
            {
                RentalId = rentalId,
                Dates = new List<CalendarDateViewModel>() 
            };
            for (var i = 0; i < nights; i++)
            {
                var date = new CalendarDateViewModel
                {
                    Date = start.Date.AddDays(i),
                    Bookings = new List<CalendarBookingViewModel>(),
                    PreparationTimes = new List<PreparationTimeViewModel>()
                };

                foreach (var booking in rentalBookings)
                {
                    var bookingEnd = booking.Start.AddDays(booking.Nights);

                    if (booking.Start <= date.Date && bookingEnd > date.Date)
                    {
                        date.Bookings.Add(new CalendarBookingViewModel { Id = booking.Id, Unit = booking.Unit });
                    }

                    var preparationEnd = bookingEnd.AddDays(rental.PreparationTimeInDays);
                    if (bookingEnd <= date.Date && preparationEnd > date.Date)
                    {
                        date.PreparationTimes.Add(new PreparationTimeViewModel { Unit = booking.Unit });
                    }
                }

                result.Dates.Add(date);
            }
            return result;
        }
    }
}
