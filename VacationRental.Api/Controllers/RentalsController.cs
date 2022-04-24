using Mapster;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using VacationRental.Api.Models;
using VacationRental.Data;
using VacationRental.Domain.Rentals;
using VacationRental.Infrastructure.DTOs;
using VacationRental.Infrastructure.Services.Interfaces;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/rentals")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly IDictionary<int, RentalViewModel> _rentals;
        private readonly IRentalService _rentalService;

        public RentalsController(IDictionary<int, RentalViewModel> rentals, IRentalService rentalService = null)
        {
            _rentals = rentals;
            _rentalService = rentalService;
        }

        [HttpGet]
        [Route("{rentalId:int}")]
        public RentalViewModel Get(int rentalId)
        {
            var rental = _rentalService.GetRental(rentalId);

            return new RentalViewModel { Id = rental.Id, PreparationTime = rental.PreparationTime, Units = rental.Units};
        }

        [HttpPost]
        public ActionResult<ResourceIdViewModel> Post(RentalCreateInputDTO model)
        {
            var rentalToCreate = model.Adapt<Rental>();

            var id = _rentalService.CreateRental(new Rental(model.Units, model.PreparationTimeInDays));

            return new ResourceIdViewModel { Id = id };
        }
    }
}
