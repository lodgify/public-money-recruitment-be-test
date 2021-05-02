using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Filters.Common;
using VacationRental.Api.Filters.Rentals;
using VacationRental.Api.Models;

namespace VacationRental.Api.Controllers
{
    #region explanation
    /*
        b) extend GET api/v1/vacationrental/calendar response by adding ~number of occupied unit~ 
        to Bookings collection (property Unit) (c)
        ------------------------------------------------------------------------------------------
        I didn't understand this part.
        Number of occupied unit during one day will always the same.
        Repeating one value in each collection item doesn't make sense.
        Bookings list for 22th april will looks like :
        {
            "date": "2021-04-22",
            "bookings": [
                {
                    "id": 1,
                    "unit": 3
                },
                {
                    "id": 2,
                    "unit": 3
                },
                {
                    "id": 3,
                    "unit": 3
                }
            ]
        }
        Instead of that I solved to provide property describes count of free units in one day.
        Since time preparation is always constant for all bookings, doesn't make sense return array.
     */
    #endregion


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
        [ZeroFilter("nights", "rentalId"), RentalNotFountFilter("rentalId")]
        public CalendarViewModel Get(int rentalId, DateTime start, int nights)
        {
            var result = new CalendarViewModel 
            {
                RentalId = rentalId,
                Dates = new List<CalendarDateViewModel>()
            };
            int units = _rentals[rentalId].Units;

            for (var i = 0; i < nights; i++)
            {
                int occupiedUnits = 0;
                var date = new CalendarDateViewModel
                {
                    Date = start.Date.AddDays(i),
                    Bookings = new List<CalendarBookingViewModel>()
                };
                var rentalBooking = _bookings.Values.Where(x => x.RentalId == rentalId).ToList();

                foreach (var booking in rentalBooking)
                {
                    DateTime bookingEnd = booking.Start.AddDays(booking.Nights);
                    DateTime bookingStart = booking.Start;

                    if (bookingStart <= date.Date && bookingEnd >= date.Date)
                    {
                        occupiedUnits += 1;
                        date.Bookings.Add(new CalendarBookingViewModel 
                        {
                            Id = booking.Id, 
                            Nights = booking.Nights
                        });
                    }
                }
                date.Unit = units - occupiedUnits;
                result.Dates.Add(date);
            }
            result.PreparationTimeInDays = _rentals[rentalId].PreparationTimeInDays;
            return result;
        }
    }
}
