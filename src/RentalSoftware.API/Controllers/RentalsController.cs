using Microsoft.AspNetCore.Mvc;
using RentalSoftware.Core.Contracts.Request;
using RentalSoftware.Core.Entities;
using RentalSoftware.Core.Interfaces;
using System;
using System.Threading.Tasks;

namespace RentalSoftware.API.Controllers
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
        public async Task<RentalViewModel> Get(int rentalId)
        {
            var rental = await _rentalService.GetRental(new GetRentalRequest() { RentalId = rentalId });

            if (rental == null)
            {
                throw new ApplicationException("Not Found");
            }

            return rental.RentalViewModel;
        }

        [HttpPut]
        [Route("{rentalId:int}")]
        public async Task<IActionResult> Put(int rentalId, RentalViewModel model)
        {
            UpdateRentalRequest updateRentalRequest = new UpdateRentalRequest() { Id = rentalId, Units = model.Units, PreparationTimeInDays = model.PreparationTime };

            var response = await _rentalService.UpdateRental(updateRentalRequest);

            if (!response.Succeeded)
            {
                return new JsonResult(response.Errors);
            }

            return new JsonResult(response.Succeeded);
        }

        [HttpPost]
        public async Task<IdentifierViewModel> Post(RentalViewModel model)
        {
            var response = await _rentalService.AddRental(new AddRentalRequest() { PreparationTime = Convert.ToInt32(model.PreparationTime), Units = model.Units });

            if (!response.Succeeded)
            {
                throw new Exception(response.Message);
            }

            return response.ResourceIdViewModel;
        }
    }
}
