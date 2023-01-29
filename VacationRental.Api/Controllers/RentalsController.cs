using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Application.Dtos;
using VacationRental.Application.Midlewares.Rental;
using VacationRental.Application.ViewModels;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/rentals")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly IRentalMiddleware _rentalMiddleware;

        public RentalsController(IRentalMiddleware rentalMiddleware)
        {
            this._rentalMiddleware = rentalMiddleware;
        }

        [HttpGet]
        [Route("{rentalId:int}")]
        public async Task<Application.ViewModels.RentalViewModel> Get(int rentalId)
        {
            var rental = await this._rentalMiddleware.GetById(rentalId);
            return rental;
        }

        [HttpPost]
        public async Task<int> Post(RentalBindingModel model)
        {
            var rentalDto = new RentalDto(model.Units, model.PreparationTimeInDays);
            var response = await this._rentalMiddleware.AddRentalWithTimePeriod(rentalDto);
            return response;
        }
	}
}
