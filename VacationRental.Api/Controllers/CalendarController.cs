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

            RentalViewModel rental = _rentals[rentalId];

            for (var i = 0; i < nights; i++)
            {
                var date = new CalendarDateViewModel
                {
                    Date = start.Date.AddDays(i),
                    Bookings = new List<CalendarBookingViewModel>(),
                    PreparationTimes = new List<CalendarPreparationViewModel>()
                };

                foreach (var booking in _bookings.Values)
                {
                    if (booking.RentalId == rentalId)
                    {
                        int bookingBlockedNights = booking.Nights + rental.PreparationTimeInDays;
                        if (booking.Start <= date.Date && booking.Start.AddDays(bookingBlockedNights) > date.Date)
                        {
                            CalendarBookingViewModel bookingView = new CalendarBookingViewModel { Id = booking.Id, Unit = booking.Unit };
                            if (booking.Start.AddDays(booking.Nights) > date.Date)
                            {
                                date.Bookings.Add(bookingView);
                            }
                            else
                            {
                                date.PreparationTimes.Add(bookingView);
                            }                            
                        } 
                    }                        
                }

                result.Dates.Add(date);
            }

            return result;
        }
    }
}
