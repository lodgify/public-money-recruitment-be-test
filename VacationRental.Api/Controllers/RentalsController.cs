using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Helpers;
using VacationRental.Common.Models;
using VacationRental.Service.Interfaces;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/rentals")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly IDictionary<int, RentalViewModel> _rentals;
        private readonly IDictionary<int, BookingViewModel> _bookings;
        private readonly IRentalService _rentalService;


        public RentalsController(IDictionary<int, RentalViewModel> rentals, IDictionary<int, BookingViewModel> bookings, IRentalService rentalService)
        {
            _rentals = rentals;
            _bookings = bookings;
            _rentalService = rentalService;
        }

        [HttpPut]
        [Route("{rentalId:int}")]
        public RentalViewModel Put(int rentalId, RentalBindingModel model)
        {
            if (!_rentals.ContainsKey(rentalId))
                throw new ApplicationException("Rental not found");
            if (_rentals[rentalId].Units != model.Units ||
                _rentals[rentalId].PreparationTimeInDays != model.PreparationTimeInDays && BookingHelper.CheckAvailability(_bookings.Values, _rentals[rentalId]))
            {
                _rentals[rentalId].Units = model.Units;
                _rentals[rentalId].PreparationTimeInDays = model.PreparationTimeInDays;
            }
            
            return _rentals[rentalId];
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

            var item = _rentalService.AddOrUpdate(new RentalViewModel
            {
                PreparationTimeInDays = model.PreparationTimeInDays,
                Units = model.Units
            });

            var key = new ResourceIdViewModel { Id = item.Id };

            return key;
        }
    }
}
