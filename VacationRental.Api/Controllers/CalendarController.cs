using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using VacationRental.Api.Models;
using VacationRental.Domain.Aggregates.Calendars;
using VacationRental.Domain.Messages.Calendars;
using VacationRental.Domain.Models.Bookings;
using VacationRental.Domain.Models.Rentals;

namespace VacationRental.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CalendarController : ControllerBase
    {
        private readonly IDictionary<int, Rental> _rentals;
        private readonly IDictionary<int, Booking> _bookings;

        public CalendarController(
            IDictionary<int, Rental> rentals,
            IDictionary<int, Booking> bookings)
        {
            _rentals = rentals;
            _bookings = bookings;
        }

        [HttpGet]
        public CalendarDto Get(int rentalId, DateTime start, int nights)
        {
            if (nights < 0)
                throw new ApplicationException("Nights must be positive");
            if (!_rentals.ContainsKey(rentalId))
                throw new ApplicationException("Rental not found");

            var result = new CalendarDto 
            {
                RentalId = rentalId,
                Dates = new List<CalendarDate>() 
            };
             
            var rental = _rentals[rentalId];
            var endBookingDate = DateTime.Now;

            for (var i = 0; i < nights; i++)
            {
                var date = new CalendarDate
                {
                    Date = start.Date.AddDays(i),
                    Bookings = new List<CalendarBooking>(),
                    PreparationTimes = new List<PreparationTime>()
                };

                foreach (var booking in _bookings.Values)
                {
                    endBookingDate = booking.Start.AddDays(booking.Nights);

                    if (booking.RentalId == rentalId
                        && booking.Start <= date.Date && endBookingDate > date.Date)
                    {
                        date.Bookings.Add(CalendarBooking.Create(booking.Units));
                    }

                    if (booking.RentalId == rentalId
                        && endBookingDate < date.Date && endBookingDate.AddDays(rental.PreparationTimeInDays) >= date.Date)
                    {
                        date.PreparationTimes.Add(new PreparationTime
                        {
                            Unit = booking.Units
                        });
                    }
                }

                result.Dates.Add(date);
            }

            return result;
        }
    }
}
