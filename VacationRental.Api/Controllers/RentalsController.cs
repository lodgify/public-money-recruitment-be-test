﻿using System;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;
using VacationRental.Api.Services;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/rentals")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly IRentalService _service;

        public RentalsController(IRentalService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [HttpGet]
        [Route("{rentalId:int}")]
        public RentalViewModel Get(int rentalId)
        {
            if (!_service.IsExists(rentalId))
                throw new ApplicationException("Rental not found");

            return _service.Get(rentalId);
        }

        [HttpPost]
        public ResourceIdViewModel Post(RentalBindingModel model)
        {
            var viewModel = new RentalViewModel
            {
                Units = model.Units
            };
            var resourceIdView = _service.Add(viewModel);
            return resourceIdView;
        }
    }
}
