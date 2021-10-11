using System;
using System.Linq;
using System.Collections.Generic;
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


        public RentalsController(
            IDictionary<int, RentalViewModel> rentals,
            IDictionary<int, BookingViewModel> bookings)
        {
            _rentals = rentals;
            _bookings = bookings;
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

        [HttpPut]
        [Route("{rentalId:int}")]
        public RentalViewModel Put(RentalBindingModel model, int rentalId)
        {
            if (!_rentals.ContainsKey(rentalId))
                throw new ApplicationException("Rental not found");

            RentalViewModel rental = _rentals[rentalId];
            // If the preparation time is less or equal, no need to check availability.
            if (model.PreparationTimeInDays > rental.PreparationTimeInDays) 
            {
                Dictionary<int, DateTime> blockedDate = new Dictionary<int, DateTime>();
                foreach (BookingViewModel booking in _bookings.Values.Where(x => x.RentalId == rentalId))
                {
                    if (!blockedDate.ContainsKey(booking.Unit))
                    {
                        blockedDate[booking.Unit] = booking.Start.AddDays(booking.Nights + model.PreparationTimeInDays);                        
                    }else if (blockedDate[booking.Unit] >= booking.Start)
                    {
                        throw new ApplicationException("Overbooking detected");
                    }
                }
            }

            _rentals[rentalId].Units = model.Units;
            _rentals[rentalId].PreparationTimeInDays = model.PreparationTimeInDays;

            return _rentals[rentalId];
        }
    }
}
