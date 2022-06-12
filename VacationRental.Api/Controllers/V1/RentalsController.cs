using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Domain.Models;
using VacationRental.Services.IServices;

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

            return _rentalService.Get(rentalId);

        }

        [HttpPost]
        public ResourceIdViewModel Post(RentalBindingModel model)
        {
                return _rentalService.Create(model);
        }


        [HttpPost]
        [Route("~/api/v1/vacationrental/rentals")]
        public ResourceIdViewModel VacationrentalPost(RentalBindingModel model)
        {
            return _rentalService.Create(model);
        }

        [HttpPut]
        [Route("~/api/v1/vacationrental/rentals/{id}")]
        public void Put(int id, RentalBindingModel model)
        {
            if (model.PreparationTimeInDays < 0)
                throw new ApplicationException("Preparation time must be positive");


            _rentalService.Update(id, model);

        }
    }
}
