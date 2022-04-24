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
        private readonly IRentalService _rentalService;

        public RentalsController(IRentalService rentalService)
        {
            _rentalService = rentalService;
        }

        [HttpGet]
        [Route("{rentalId:int}")]
        public ActionResult<RentalDTO> Get(int rentalId)
        {
            var rental = _rentalService.GetRental(rentalId);

            var result = rental.Adapt<RentalDTO>();

            return result;
        }

        [HttpPost]
        public ActionResult<RentalCreateOutputDTO> Post(RentalCreateInputDTO model)
        {
            var rentalToCreate = model.Adapt<Rental>();

            var id = _rentalService.CreateRental(rentalToCreate);

            var result = id.Adapt<RentalCreateOutputDTO>();

            return result;
        }
    }
}
