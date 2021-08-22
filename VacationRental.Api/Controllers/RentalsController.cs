using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/rentals")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly IDictionary<int, RentalViewModel> _rentals;
        private readonly IDictionary<int, BookingViewModel> _bookings;
        private readonly IBookingValidator _bookingValidator;

        public RentalsController(
            IDictionary<int, RentalViewModel> rentals,
            IDictionary<int, BookingViewModel> bookings,
            IBookingValidator bookingValidator)
        {
            _rentals = rentals;
            _bookings = bookings;
            _bookingValidator = bookingValidator;
        }

        [HttpGet]
        [Route("{rentalId:int}")]
        public RentalViewModel Get(int rentalId)
        {
            if (!_rentals.ContainsKey(rentalId))
                throw new ApplicationException("Rental not found");

            return _rentals[rentalId];
        }

        [HttpPost]
        public ResourceIdViewModel Post(RentalBindingModel model)
        {
            var key = new ResourceIdViewModel { Id = _rentals.Keys.Count + 1 };

            _rentals.Add(key.Id, new RentalViewModel
            {
                Id = key.Id,
                Units = model.Units,
                PreparationTimeInDays = model.PreparationTimeInDays
            });

            return key;
        }

        [HttpPost]
        [Route("{rentalId:int}")]
        public RentalViewModel Post(int rentalId, RentalBindingModel model)
        {
            if (!_rentals.ContainsKey(rentalId))
                throw new ApplicationException("Rental not found");

            var key = rentalId;
            int count = 0;

            if(_bookings.Values.Where(b => b.RentalId == key).Select(b => b.Unit).Distinct().Count() > model.Units)
            {
                throw new ApplicationException("Update sets unit count below already booked amount");
            }

            if (_rentals.TryGetValue(key, out RentalViewModel rvm))
            {
                var previousBookings = _bookings.Values.Where(b => b.RentalId == key).OrderBy(b => b.Start).ToArray();
                
                for(int i = 0; i < previousBookings.Count() - 1; i++)
                {
                    if(previousBookings[i].Unit == previousBookings[i + 1].Unit &&
                        _bookingValidator.FoundMatch(previousBookings[i].Start, previousBookings[i + 1].Start, previousBookings[i].Nights + model.PreparationTimeInDays))
                    {
                        count++;
                    }
                }
            }

            if (model.Units <= count)
            {
                throw new ApplicationException("Update causes booking conflicts");
            }

            rvm.Units = model.Units;
            rvm.PreparationTimeInDays = model.PreparationTimeInDays;

            return rvm;
        }
    }
}
