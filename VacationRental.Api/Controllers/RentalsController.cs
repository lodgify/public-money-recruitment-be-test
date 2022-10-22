using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Core.Interfaces;
using VacationRental.Api.Core.Models;
using VacationRental.Api.Extensions;

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
        public async Task<IActionResult> Get([Required] int rentalId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var rental = await _rentalService.GetRentalByIdAsync(rentalId);
            return rental.ToOk(e => e);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        public async Task<IActionResult> Post([Required] RentalBindingModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var rental = await _rentalService.InsertNewRentalAsync(model);
            return rental.ToOk(e => e);
        }

        [HttpPut]
        [Route("{rentalId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status304NotModified)]
        public async Task<IActionResult> Put([Required] int rentalId, [Required] RentalBindingModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var rental = await _rentalService.UpdateRentalAsync(rentalId, model);
            return rental.ToOk(e => e);
        }
    }
}
