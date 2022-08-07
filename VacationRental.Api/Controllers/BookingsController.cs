using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Extensions;
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

            var rental = _rentals[model.RentalId];
            var nightsIncluingPrepration = model.Nights + rental.PreparationTimeInDays;
            var unAvailableUnits = model.Start
                                        .CreateRangeUntil(nightsIncluingPrepration)
                                        .Select(day => new {Date = day, UnitsBusy = _bookings.Values.Where(x => x.RentalId == model.RentalId && x.Start <= day && day <= x.Start.AddDays(x.Nights-1)).Count() })
                                        .Any(x => x.UnitsBusy + 1 > rental.Units);

            if(unAvailableUnits)
                throw new ApplicationException("Not available");

            var key = new ResourceIdViewModel { Id = _bookings.Keys.Count + 1 };

            // guest occupation
            _bookings.Add(key.Id, new BookingViewModel
            {
                Id = key.Id,
                Nights = model.Nights,
                RentalId = model.RentalId,
                Start = model.Start.Date
            });

            // preparation time
            if(rental.PreparationTimeInDays > 0)
            {
                _bookings.Add(key.Id + 1, new BookingViewModel
                {
                    Id = key.Id + 1,
                    Nights = rental.PreparationTimeInDays,
                    RentalId = model.RentalId,
                    Start = model.Start.Date.AddDays(model.Nights),
                    IsPreparation = true
                });
            }

            return key;
        }
    }
}
