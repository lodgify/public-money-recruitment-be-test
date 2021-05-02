using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Filters.Common;
using VacationRental.Api.Filters.Rentals;
using VacationRental.Api.Models;

namespace VacationRental.Api.Controllers
{
    /* 
        Since we don't have api/v1/~VACATIONRENTAL~/rentals endpoint and I should EXTEND something
        I'll modify `api/v1/rentals`
     */


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

        [HttpPost]
        [RentalBindingModelFilter]
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


        [HttpGet("{rentalId:int}")]
        [ZeroFilter("rentalId")] 
        [RentalNotFountFilter("rentalId")]
        public RentalViewModel Get(int rentalId)
        {
            return _rentals[rentalId];
        }

        [HttpPut("{rentalId:int}")]
        [ZeroFilter("rentalId"), RentalNotFountFilter("rentalId")]
        [RentalBindingModelFilter]
        [RentalPropertyChangedFilter("rentalId")]
        public RentalViewModel Put(int rentalId, [FromBody] RentalBindingModel model) 
        {
            var rental = _rentals[rentalId];
            rental.PreparationTimeInDays = model.PreparationTimeInDays;
            rental.Units = model.Units;

            var bookings = _bookings.Where(x => x.Value.RentalId == rental.Id).ToList();
            bookings.ForEach(booking => 
            {
                booking.Value.PreparationTimeInDays = model.PreparationTimeInDays;
            });

            return _rentals[rentalId];
        }
    }
}
