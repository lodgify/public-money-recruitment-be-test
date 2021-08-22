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
        private readonly IDictionary<int, IList<PreparationTimeModel>> _preparationTimes;
        private readonly IBookingValidator _bookingValidator;

        public BookingsController(
            IDictionary<int, RentalViewModel> rentals,
            IDictionary<int, BookingViewModel> bookings, 
            IDictionary<int, IList<PreparationTimeModel>> preparationTimes,
            IBookingValidator bookingValidator)
        {
            _rentals = rentals;
            _bookings = bookings;
            _preparationTimes = preparationTimes;
            _bookingValidator = bookingValidator;
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

            var availableUnits = Enumerable.Range(1, _rentals[model.RentalId].Units).ToList();
            var preparationDays = _rentals[model.RentalId].PreparationTimeInDays;

            for (var i = 0; i < model.Nights; i++)
            {
                var count = 0;

                foreach (var booking in _bookings.Values)
                {
                    if (booking.RentalId == model.RentalId && _bookingValidator.Validate(booking.Start, model.Start, booking.Nights + preparationDays))
                    {
                        availableUnits.Remove(booking.Unit);
                        count++;
                    }
                }
                if (count >= _rentals[model.RentalId].Units)
                    throw new ApplicationException("Not available");

            }

            var key = new ResourceIdViewModel { Id = _bookings.Keys.Count + 1 };
            var unit = availableUnits.First();

            _bookings.Add(key.Id, new BookingViewModel
            {
                Id = key.Id,
                Nights = model.Nights,
                RentalId = model.RentalId,
                Start = model.Start.Date,
                Unit = unit
            });

            var preparationTimeModel = new PreparationTimeModel
            {
                Unit = unit
            };

            if (_preparationTimes.TryGetValue(model.RentalId, out IList<PreparationTimeModel> preparationModelList))
            {
                preparationModelList.Add(preparationTimeModel);
            }
            else
            {
                _preparationTimes.Add(model.RentalId, new List<PreparationTimeModel> 
                {
                    preparationTimeModel
                });
            }

            return key;
        }
    }
}
