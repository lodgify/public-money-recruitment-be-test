using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/bookings")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IDictionary<int, RentalViewModel> _rentals;
        private readonly IDictionary<int, BookingViewModel> _bookings;

        public BookingsController(
            IDictionary<int, RentalViewModel> rentals,
            IDictionary<int, BookingViewModel> bookings)
        {
            _rentals = rentals;
            _bookings = bookings;
        }

        [HttpGet]
        [Route("{bookingId:int}")]
        public BookingViewModel Get(int bookingId)
        {
            if (!_bookings.ContainsKey(bookingId))
                throw new ApplicationException("Booking not found");

            return _bookings[bookingId];
        }

        [HttpPost]
        public ResourceIdViewModel Post(BookingBindingModel model)
        {
            if (model.Nights <= 0)
                throw new ApplicationException("Nigts must be positive");
            if (!_rentals.ContainsKey(model.RentalId))
                throw new ApplicationException("Rental not found");

            var unit = -1; // Has No Unit
            var rental = _rentals[model.RentalId];

            var rentalBookingsGroupedByUnit =
                _bookings.Values.Where(booking => booking.RentalId == model.RentalId)
                                .GroupBy(booking => booking.Unit);
            // We check before if We can Put it in an existing used Unit 
            foreach(var rentalBookingUnit in rentalBookingsGroupedByUnit)
            {
                bool bookingCanBeAddedInCurrentUnit = true;
                foreach (var booking in rentalBookingUnit)
                {
                    // We take into Consideration the preparationTime to check the Availability
                    var bookingEnd = booking.Start.AddDays(booking.Nights + rental.PreparationTimeInDays);
                    var modelEnd = model.Start.AddDays(model.Nights + rental.PreparationTimeInDays);

                    if (booking.RentalId == model.RentalId
                        && (booking.Start <= model.Start.Date && bookingEnd > model.Start.Date)
                        || (booking.Start < modelEnd && bookingEnd >= modelEnd)
                        || (booking.Start > model.Start && bookingEnd < modelEnd))
                    {
                        bookingCanBeAddedInCurrentUnit = false;
                        break;
                    }
                }
                if(bookingCanBeAddedInCurrentUnit)
                {
                    unit = rentalBookingUnit.Key;
                    break;
                }   
            }
            if(unit == -1 && rentalBookingsGroupedByUnit.Count() >= rental.Units)
            {
                throw new ApplicationException("Not available");
            }
            else if(unit == -1)
            {
                unit = rentalBookingsGroupedByUnit.Count();
            }

            var key = new ResourceIdViewModel { Id = _bookings.Keys.Count + 1 };

            _bookings.Add(key.Id, new BookingViewModel
            {
                Id = key.Id,
                Nights = model.Nights,
                RentalId = model.RentalId,
                Start = model.Start.Date,
                Unit = unit,
            });

            return key;
        }
    }
}
