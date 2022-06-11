using System;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;
using VacationRental.Api.Services.Interfaces;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/rentals")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly IRentalsService _rentalsService;

        public RentalsController(IRentalsService rentalsService)
        {
            _rentalsService = rentalsService;
        }

        [HttpGet("{rentalId:int}")]
        public RentalViewModel Get(int rentalId)
        {
            var rental = _rentalsService.Get(rentalId);
            if (rental == null)
                throw new ApplicationException("Rental not found");

            return rental;
        }

        [HttpPost]
        public ResourceIdViewModel Post(RentalBindingModel model)
            => _rentalsService.AddRental(model);

        [HttpPut("{rentalId:int}")]
        public RentalViewModel Post(int rentalId, RentalBindingModel model)
        {
            var rental = _rentalsService.Get(rentalId);
            if (rental == null)
                throw new ApplicationException("Rental not found");

            rental.PreparationTimeInDays = model.PreparationTimeInDays;
            rental.Units = model.Units;

            _rentalsService.Update(rentalId, rental);

            var updatedRental = _rentalsService.Get(rentalId);
            if (updatedRental == null)
                throw new ApplicationException("Rental not found");

            return updatedRental;
        }
    }
}
