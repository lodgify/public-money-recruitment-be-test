using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;
using VacationRental.Application.ViewModels;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/calendar")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        private readonly IDictionary<int, Models.RentalViewModel> _rentals;
        private readonly IDictionary<int, BookingViewModelOutput> _bookings;

        public CalendarController(
            IDictionary<int, Models.RentalViewModel> rentals,
            IDictionary<int, BookingViewModelOutput> bookings)
        {
            _rentals = rentals;
            _bookings = bookings;
        }

		//int rentalId, DateTime start, int nights

		[HttpGet]
        public CalendarViewModel Get(CalendarInputViewModel input)
        {
            if (input.Nights < 0)
                throw new ApplicationException("Nights must be positive");
            if (!_rentals.ContainsKey(input.RentalId))
                throw new ApplicationException("Rental not found");

            var result = new CalendarViewModel 
            {
                RentalId = input.RentalId,
                Dates = new List<CalendarDateViewModel>() 
            };
            for (var i = 0; i < input.Nights; i++)
            {
                var date = new CalendarDateViewModel
                {
                    Date = input.DateStart.Date.AddDays(i),
                    Bookings = new List<CalendarBookingViewModel>()
                };

                foreach (var booking in _bookings.Values)
                {
                    if (booking.RentalId == input.RentalId
                        && booking.Start <= date.Date && booking.Start.AddDays(booking.Nights) > date.Date)
                    {
                        date.Bookings.Add(new CalendarBookingViewModel { Id = booking.Id });
                    }
                }

                result.Dates.Add(date);
            }

            return result;
        }
    }
}
