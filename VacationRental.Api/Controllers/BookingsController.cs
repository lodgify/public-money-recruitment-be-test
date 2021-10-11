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

            RentalViewModel rental = _rentals[model.RentalId];
            int realNightsBlocked = rental.PreparationTimeInDays + model.Nights;
            var selectedUnit = 1;
            for (var i = 0; i < realNightsBlocked; i++)
            {
                var count = selectedUnit;
                // TODO: improve from O(n) to O(1)
                foreach (var booking in _bookings.Values
                    .Where(x=> x.RentalId == model.RentalId && x.Unit == selectedUnit))
                {
                    int bookingNightsBlocked = rental.PreparationTimeInDays + booking.Nights;
                    if ( (booking.Start <= model.Start.Date && booking.Start.AddDays(bookingNightsBlocked) > model.Start.Date)
                         || (booking.Start < model.Start.AddDays(realNightsBlocked) && booking.Start.AddDays(bookingNightsBlocked) >= model.Start.AddDays(realNightsBlocked))
                         || (booking.Start > model.Start && booking.Start.AddDays(bookingNightsBlocked) < model.Start.AddDays(realNightsBlocked)))
                    {
                        count++;
                    }
                }
                if (count > rental.Units) 
                    throw new ApplicationException("Not available");
                selectedUnit = count;

            }


            var key = new ResourceIdViewModel { Id = _bookings.Keys.Count + 1 };

            _bookings.Add(key.Id, new BookingViewModel
            {
                Id = key.Id,
                Nights = model.Nights,
                RentalId = model.RentalId,
                Start = model.Start.Date,
                Unit = selectedUnit,
            });
            return key;
        }
    }
}
