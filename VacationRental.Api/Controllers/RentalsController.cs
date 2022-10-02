using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VacationRental.Domain.VacationRental.Interfaces;
using VacationRental.Domain.VacationRental.Models;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/rentals")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly IRentalsService _rentalsService;

        public RentalsController(IRentalsService paramRentals)
        {
            _rentalsService = paramRentals;
        }

        /// <summary>
        ///  This method is responsible to return the rental information.
        /// </summary>
        /// <param name="rentalId"></param>
        /// <returns>Return a object RentalViewModel</returns>
        /// <response code="200">RentalViewModel</response>
        /// <response code="404">The rental with the parameters passed does not exist.</response> 
        /// <response code="500">Internal error from Server.</response> 
        [ProducesResponseType(typeof(RentalViewModel), 200)]
        [HttpGet]
        [Route("{rentalId:int}")]
        public async Task<RentalViewModel> Get(int rentalId)
        {
            return await _rentalsService.Get(rentalId);
        }

        /// <summary>
        ///  This method is responsible to add the rental by the hosts.
        /// </summary>
        /// <param name="rentalModel"></param>
        /// <returns>Return a object ResourceIdViewModel</returns>
        /// <response code="200">ResourceIdViewModel</response>
        /// <response code="500">Internal error from Server.</response> 
        [ProducesResponseType(typeof(ResourceIdViewModel), 200)]
        [HttpPost]
        public async Task<ResourceIdViewModel> Post(RentalBindingModel rentalModel)
        {
            return await _rentalsService.Post(rentalModel);
        }

        /// <summary>
        ///  This method is responsible to update the rental by the hosts.
        /// </summary>
        /// <param name="rentalId"></param>
        /// <param name="rentalModel"></param>
        /// <returns>Return a object ResourceIdViewModel</returns>
        /// <response code="200">ResourceIdViewModel</response>
        /// <response code="500">Internal error from Server.</response> 
        [ProducesResponseType(typeof(ResourceIdViewModel), 200)]
        [HttpPut]
        [Route("{rentalId:int}")]
        public async Task<ResourceIdViewModel> Put(int rentalId, RentalBindingModel rentalModel)
        {
            return await _rentalsService.Put(rentalId, rentalModel);
        }
    }
}
