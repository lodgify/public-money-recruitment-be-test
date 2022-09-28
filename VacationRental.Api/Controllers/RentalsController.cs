using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;
using VacationRental.BusinessLogic.Services.Interfaces;
using VacationRental.BusinessObjects;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/rentals")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly IRentalsService _rentalsService;
        private readonly IMapper _mapper;

        public RentalsController(IRentalsService rentalsService,
            IMapper mapper)
        {
            _rentalsService = rentalsService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("{rentalId:int}")]
        public RentalViewModel Get(int rentalId)
        {
            var rental = _rentalsService.GetRental(rentalId);
            var rentalViewModel = _mapper.Map<RentalViewModel>(rental);

            return rentalViewModel;
        }

        [HttpPost]
        public ResourceIdViewModel Post(RentalBindingModel model)
        {
            var createRental = _mapper.Map<CreateRental>(model);
            var rentalId = _rentalsService.CreateRental(createRental);

            var key = new ResourceIdViewModel { Id = rentalId };

            return key;
        }
    }
}
