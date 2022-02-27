using Microsoft.AspNetCore.Mvc;
using System;
using VacationRental.Api.Models;
using VacationRental.Api.Services.Interfaces;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/rentals")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly IRentalService _rentalService;

        public RentalsController(IRentalService rentalService)
        {
            _rentalService = rentalService;
        }

        [HttpGet]
        [Route("{rentalId:int}")]
        public RentalViewModel Get(int rentalId)
        {
            if (rentalId <= 0)
                throw new ApplicationException("Rental Id must be greater than zero!");

            return _rentalService.Get(rentalId);
        }

        [HttpPost]
        public ResourceIdViewModel Post(RentalBindingModel model)
        {
            // wish to have a chance to check if exists

            return _rentalService.Create(model);
        }

        [HttpPut]
        [Route("{rentalId:int}")]
        public RentalViewModel Put(int rentalId, RentalBindingModel model)
        {
            if (rentalId <= 0)
                throw new ApplicationException("Rental Id be greater than zero!");

            return _rentalService.Update(rentalId, model);
        }
    }
}
