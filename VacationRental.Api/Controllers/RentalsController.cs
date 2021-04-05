using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VacationalRental.Domain.Business;
using VacationalRental.Domain.Entities;
using VacationalRental.Domain.Interfaces.Repositories;
using VacationalRental.Domain.Interfaces.Services;
using VacationRental.Api.Models;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/rentals")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        //private readonly IDictionary<int, RentalViewModel> _rentals;
        //We call the repository and not a service because we don't have any business logic in the process
        //in this case we get a single object instantiated and not two of them
        private readonly IRentalsRepository _rentalsRepository;

        public RentalsController(
            //IDictionary<int, RentalViewModel> rentals,
            IRentalsRepository rentalsRepository)
        {
            //_rentals = rentals;
            _rentalsRepository = rentalsRepository;
        }

        [HttpGet]
        [Route("{rentalId:int}")]
        public async Task<ActionResult<RentalViewModel>> Get(int rentalId)
        {
            try
            {
                if (!await _rentalsRepository.RentalExists(rentalId))
                    return NotFound("Rental not found");
                //throw new ApplicationException("Rental not found");

                var rental = await _rentalsRepository.GetRentalById(rentalId);

                return new RentalViewModel
                {
                    Id = rental.Id,
                    Units = rental.Units
                };
            }
            catch (Exception)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(RentalBindingModel model)
        {
            try
            {
                if (model == null || !ModelState.IsValid)
                    return BadRequest(ModelState);

                //var key = new ResourceIdViewModel { Id = _rentals.Keys.Count + 1 };

                //_rentals.Add(key.Id, new RentalViewModel
                //{
                //    Id = key.Id,
                //    Units = model.Units
                //});

                //return key;

                var rentalId = await _rentalsRepository.InsertNewRentalObtainRentalId(
                    new RentalEntity
                    {
                        Units = model.Units
                    });

                if (rentalId <= 0)
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);

                return rentalId;
            }
            catch (Exception)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
