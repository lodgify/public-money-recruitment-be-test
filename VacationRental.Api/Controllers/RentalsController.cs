using System;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;
using VacationRental.Application;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/rentals")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private IRentalService _iRentalService;

        public RentalsController(IRentalService iRentalService)
        {
            _iRentalService = iRentalService;
        }

        [HttpGet]
        [Route("{rentalId:int}")]
        public RentalViewModel Get(int rentalId)
        {
            var response = _iRentalService.GetRental(new GetRentalRequest() { rentalId = rentalId });

            if (response.Success)
            {
                return response.RentalViewModel;
            }
            else
            {
                // To don't change the signature (I like to return a Json with the error)
                throw new Exception(response.Message);
            }
        }

        [HttpPost]
        public ResourceIdViewModel Post(RentalBindingModel model)
        {
            var response = _iRentalService.AddRental(new AddRentalRequest() { PreparationTimeInDays = model.PreparationTimeInDays,Units= model.Units });

            if (response.Success)
            {
                return response.ResourceIdViewModel;
            }
            else
            {
                // To don't change the signature (I like to return a Json with the error)
                throw new Exception(response.Message);
            }
        }

        [HttpPut]
        [Route("{rentalId:int}")]
        public JsonResult Put(int rentalId, RentalBindingModel model)
        {
            UpdateRentalRequest updateRentalRequest = new UpdateRentalRequest() { Id = rentalId, Units = model.Units, PreparationTimeInDays = model.PreparationTimeInDays };

            var response = _iRentalService.UpdateRental(updateRentalRequest);

            if (response.Success)
            {
                return new JsonResult(response.Success); 
            }
            else
            {
                return new JsonResult(response);
            }

        }     
    }
}
