using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;
using VacationRental.Business.Abstract;
using VacationRental.Business.Mapper;
using VacationRental.Domain.DTOs;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/rentals")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly IRentalService rentalService;
        public RentalsController(IRentalService _rentalService)
        {
            rentalService = _rentalService;
        }

        [HttpGet]
        [Route("{rentalId:int}")]
        public IActionResult Get(int rentalId)
        {
            var rental = rentalService.GetById(rentalId);
            if (rental is null)
            {
                return NotFound("Rental not found");
            }

            return Ok(rental.ToBl());
        }

        [HttpPost]
        public IActionResult Post([FromBody] RentalDto model)
        {
            rentalService.Create(model.ToDb());

            return Ok(model);
        }

        [HttpPut]
        [Route("{rentalId:int}")]
        public IActionResult Put(int rentalId, [FromBody] RentalDto dto)
        {
            var rental = rentalService.GetById(rentalId);

            if (rental is null)
            {
                return NotFound("Rental not found");
            }

            var updatedRental = rentalService.Update(rental, dto);

            if (updatedRental is null)
            {
                return BadRequest("Overlapping bookings");
            }

            return Ok(dto);
        }
    }
}
