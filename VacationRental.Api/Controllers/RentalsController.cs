using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;
using VacationRental.Api.Services;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/rentals")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly IDictionary<int, RentalViewModel> _rentals;
        private readonly IRentalService _service;

        public RentalsController(IDictionary<int, RentalViewModel> rentals,IRentalService service)
        {
            _rentals = rentals;
            _service = service;
        }

        [HttpGet]
        [Route("{rentalId:int}")]
        public RentalViewModel Get(int rentalId)
        {
            _service.rentalGetValidation(rentalId,_rentals);
            return _rentals[rentalId];
        }



        [HttpPost]
        public ResourceIdViewModel Post(RentalBindingModel model)
        {
            ResourceIdViewModel key = _service.addResourceIdViewModel(model,_rentals);

            return key;
        }


    }
}
