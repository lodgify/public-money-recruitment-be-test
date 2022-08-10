using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Contracts.Request;
using VacationRental.Api.Contracts.Response;
using VacationRental.Api.Interfaces;
using VacationRental.Api.Models;

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

        /// <summary>
        /// Returns a single rental by id 
        /// </summary>
        /// <param name="rentalId"></param>
        /// <response code="200">Successfully found and returned the rental </response>
        /// <response code="400">The rental does not exist in the system  </response>
        /// <response code="500">If an unexpected error happen</response>
        [ProducesResponseType(typeof(RentalViewModel), 200)]
        [ProducesResponseType(typeof(string),400)]
        [ProducesResponseType(500)]
        [HttpGet]
        [Route("{rentalId:int}")]
        public IActionResult Get(int rentalId) => Ok(_rentalService.GetRental(rentalId));


        /// <summary>
        /// Creates a new rental in the system
        /// </summary>
        /// <param name="model"></param>
        /// <response code="200">Rental was successfully created </response>
        /// <response code="400">The request did not pass validation checks </response>
        /// <response code="500">If an unexpected error happen</response>
        [ProducesResponseType(typeof(ResourceIdViewModel), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [HttpPost]
        public async Task<IActionResult> Post(RentalBindingModel model) => Ok(await _rentalService.CreateAsync(model));
    }
}