using System;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;
using VacationRental.Api.Services;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/vacationrental/rentals")]
    [ApiController]
    public class VacationRentalsController : ControllerBase
    {
        private readonly IRentalService _service;
        
        public VacationRentalsController(IRentalService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [HttpPost]
        public ResourceIdViewModel Post(VacationRentalBindingModel model)
        {
            var viewModel = new RentalViewModel
            {
                PreparationTimeInDays = model.PreparationTimeInDays,
                Units = model.Units
            };
            var resourceIdView = _service.Add(viewModel);
            return resourceIdView;
        }
    }
}
