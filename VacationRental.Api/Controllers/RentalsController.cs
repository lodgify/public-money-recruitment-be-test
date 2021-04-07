using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VacationalRental.Domain.Business;
using VacationalRental.Domain.Entities;
using VacationalRental.Domain.Enums;
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
        
        private readonly IRentalService _rentalsService;

        public RentalsController(
            //IDictionary<int, RentalViewModel> rentals,
            IRentalService rentalsService)
        {
            //_rentals = rentals;
            _rentalsService = rentalsService;
        }

        [HttpGet]
        [Route("{rentalId:int}")]
        public async Task<ActionResult<RentalViewModel>> Get(int rentalId)
        {
            try
            {
                if (!await _rentalsService.RentalExists(rentalId))
                    return NotFound("Rental not found");
                //throw new ApplicationException("Rental not found");

                var rental = await _rentalsService.GetRentalById(rentalId);

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
        public async Task<ActionResult<int>> Post([FromBody] RentalBindingModel model)
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

                var rentalInfo = await _rentalsService.InsertNewRentalObtainRentalId(
                    new RentalEntity
                    {
                        Units = model.Units,
                        PreprationTimeInDays = model.PreparationTimeInDays
                    });

                //if (rentalId.Item1 == InsertNewRentalStatus.PreparationDaysHigherThanUnits)
                //    return BadRequest($"{nameof(RentalBindingModel.PreparationTimeInDays)} can´t be higher than {nameof(RentalBindingModel.Units)}");

                return rentalInfo.Item1 switch
                {
                    InsertUpdateNewRentalStatus.InsertUpdateDbNoRowsAffected => BadRequest("Booking not added, try again, if persist contact technical support"),
                    InsertUpdateNewRentalStatus.OK => Ok(new { id = rentalInfo.Item2 }),
                    _ => throw new InvalidOperationException($"{nameof(InsertUpdateNewRentalStatus)}: Not expected or implemented"),
                };
            }
            catch (Exception)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
