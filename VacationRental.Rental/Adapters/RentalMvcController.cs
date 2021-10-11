using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using VacationRental.Rental.DTOs;
using VacationRental.Rental.Services;

namespace VacationRental.Rental.Adapters
{
    [Route("api/v1/rentals")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly RentalService _service;
        private readonly ILogger<RentalsController> _logger;


        public RentalsController(RentalService service, ILogger<RentalsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        [Route("{rentalId:int}")]
        public async Task<IActionResult> Get(int rentalId, [FromQuery] GetRentalRequest request)
        {
            if (rentalId != 0 && request.Id == 0)
            {
                request.Id = rentalId;
            }
            else if (rentalId == 0 && request.Id == 0)
            {
                throw new ApplicationException("Id not Found");
            }
            var Rental = await _service.SearchAsync(request);
            return Ok(Rental);
        }

        [HttpPost]
        public async Task<IActionResult> Post(AddRentalRequest model)
        {
            var rental = await _service.AddNewAsync(model);
            return Ok(rental);
        }

        [HttpPut]
        [Route("{rentalId:int}")]
        public async Task<IActionResult> Put(UpdateRentalRequest model, int rentalId)
        {
            if (model.Id == 0 && rentalId != 0)
            {
                model.Id = rentalId;
            }
            else if (model.Id == 0 && rentalId == 0)
            {
                throw new ApplicationException("Invalid Id given");
            }
            var rental = await _service.UpdateAsync(model);
            return Ok(rental);
        }
    }
}
