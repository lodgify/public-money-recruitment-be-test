using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Helpers;
using VacationRental.Common.Models;

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

            var item = new BookingViewModel
            {
                Nights = model.Nights,
                RentalId = model.RentalId,
                Start = model.Start.Date
            };
            if (!BookingHelper.CheckAvailability(item, _bookings.Values, _rentals[model.RentalId]))
            {
                throw new ApplicationException("Not available");
            }


            var key = new ResourceIdViewModel { Id = _bookings.Keys.Count + 1 };
            item.Id = key.Id;
            _bookings.Add(key.Id, item);

            return key;
        }
    }
}
