using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Core.Models;
using VacationRental.Api.Extensions;
using VacationRental.Api.Interfaces;

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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get([Required] int rentalId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var rental = _rentalService.GetRentalById(rentalId);
            return rental.ToOk(e => e);

        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        public IActionResult Post([Required] RentalBindingModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var rental = _rentalService.AddNewRental(model);
            return rental.ToOk(e => e);
        }
    }
}
