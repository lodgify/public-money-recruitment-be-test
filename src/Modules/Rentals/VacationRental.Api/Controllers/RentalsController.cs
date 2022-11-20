using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VacationRental.Api.Models;
using VacationRental.Application.Commands;
using VacationRental.Application.Queries;
using VacationRental.Shared.Abstractions.Dispatchers;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/rentals")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly IDispatcher _dispatcher;

        public RentalsController(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        [HttpGet]
        [Route("{rentalId:int}")]
        public async Task<ActionResult<RentalViewModel>> GetAsync(int rentalId)
        {
            var rental = await _dispatcher.QueryAsync(new GetRental { Id = rentalId });

            return Ok(rental.AsViewModel());
        }

        [HttpPost]
        public async Task<ActionResult<ResourceIdViewModel>> PostAsync(RentalBindingModel model)
        {
            var id = await _dispatcher.SendAsync(new AddRental(model.Units, model.PreparationTimeInDays));

            return Ok(new ResourceIdViewModel { Id = id });
        }
    }
}
