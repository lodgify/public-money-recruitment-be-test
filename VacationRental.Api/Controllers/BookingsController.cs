using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.InteropServices;
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

        public BookingsController(IDictionary<int, RentalViewModel> rentals, IDictionary<int, BookingViewModel> bookings)
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
                throw new ApplicationException("Nights must be positive");
            if (!_rentals.ContainsKey(model.RentalId))
                throw new ApplicationException("Rental not found");

            // pre-declaring the days to clean this rental will save space on the below if statement
            int cleaningDays = _rentals[model.RentalId].PreparationTimeInDays;

            for (var i = 0; i < model.Nights; i++)
            {
                var unavailableRooms = 0;
                foreach (var booking in _bookings.Values)
                {
                    // the boolean logic was set up in a way that didn't require the ID's to match. Moving one parenthesis fixed the issue

                    // checks if IDs match
                    if (booking.RentalId == model.RentalId
                        // looks for clashes between the two bookings
                        && (booking.Start <= model.Start.Date && booking.Start.AddDays(booking.Nights + cleaningDays) > model.Start.Date  // 1S-2S 1E
                        || (booking.Start < model.Start.AddDays(model.Nights + cleaningDays) && booking.Start.AddDays(booking.Nights + cleaningDays) >= model.Start.AddDays(model.Nights + cleaningDays)) // 1S 2E-1E
                        || (booking.Start > model.Start && booking.Start.AddDays(booking.Nights + cleaningDays) < model.Start.AddDays(model.Nights + cleaningDays)))) // 2S 1S 1E 2E
                    {
                        // if there is a clash, that room can't be used, so increase the number of unavailable rooms
                        unavailableRooms++;
                    }
                }
                // if all rooms are unavailable?
                if (unavailableRooms >= _rentals[model.RentalId].Units)
                    throw new ApplicationException("Not available");
            }

            // add the room to the dictionary and return all of the ID "Keys"
            var key = new ResourceIdViewModel { Id = _bookings.Keys.Count + 1 };

            _bookings.Add(key.Id, new BookingViewModel
            {
                Id = key.Id,
                Nights = model.Nights,
                RentalId = model.RentalId,
                Start = model.Start.Date
            });

            return key;
        }
    }
}
