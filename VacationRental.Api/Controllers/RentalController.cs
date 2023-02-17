using Microsoft.AspNetCore.Mvc;
using Models.ViewModels;
using VacationRental.Api.Operations.RentalsOperations;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/rentals")]
    [ApiController]
    public class RentalController : ControllerBase
    {
        private readonly IRentalGetOperation _rentalGetOperation;
        private readonly IRentalCreateOperation _rentalCreateOperation;

        public RentalController(IRentalGetOperation rentalGetOperation, IRentalCreateOperation rentalCreateOperation)
        {
            _rentalGetOperation = rentalGetOperation;
            _rentalCreateOperation = rentalCreateOperation;
        }

        [HttpGet]
        [Route("{rentalId:int}")]
        public RentalViewModel Get(int rentalId)
        {
            var result = _rentalGetOperation.ExecuteAsync(rentalId);

            return result;
        }

        [HttpPost]
        public ResourceIdViewModel Post(RentalBindingModel model)
        {
            var result = _rentalCreateOperation.ExecuteAsync(model);

            return result;
        }
    }
}
