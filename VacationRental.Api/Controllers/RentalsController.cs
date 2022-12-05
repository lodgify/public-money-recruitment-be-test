using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;
using VacationRental.Business.BusinessLogic;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/rentals")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private RentalBusinessLogic _rentalBusinessLogic;
        public RentalsController(RentalBusinessLogic rentalBusinessLogic)
        {
            _rentalBusinessLogic = rentalBusinessLogic;
        }

        [HttpGet]
        [Route("{rentalId:int}")]
        public RentalViewModel Get(int rentalId)
        {
            return _rentalBusinessLogic.GetRental(rentalId);
        }

        [HttpPost]
        public ResourceIdViewModel Post(RentalBindingModel model)
        {
            int id = _rentalBusinessLogic.AddRental(model);
            var key = new ResourceIdViewModel { Id = id };
            return key;
        }

        [HttpPut]
        [Route("{rentalId:int}")]
        public RentalViewModel Put(int rentalId, RentalBindingModel model)
        {
            var updatedRental = _rentalBusinessLogic.UpdateRental(rentalId, model);
            return updatedRental;
        }
    }
}
