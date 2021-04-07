using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacationalRental.Domain.Enums;
using VacationalRental.Domain.Interfaces.Services;
using VacationalRental.Domain.Models;
using VacationRental.Api.Models;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/vacationrental")]
    [ApiController]
    public class VacationRentalController : ControllerBase
    {
        private readonly IRentalService _rentalService;
        public VacationRentalController(IRentalService rentalService)
        {
            _rentalService = rentalService;
        }

        [HttpPut]
        [Route("{rentalId:int}")]
        public async Task<ActionResult<VacationalRentalViewModel>> UpdateRentals(int rentalId, [FromBody] VacationalRentalViewModel vacationalRentalViewModel)
        {
            var insertUpdateNewRentalStatus = await _rentalService.UpdateRental(
                new VacationalRentalModel
                {
                    PreparationTimeInDays = vacationalRentalViewModel.PreparationTimeInDays,
                    RentalId = rentalId,
                    Units = vacationalRentalViewModel.Units
                });

            return insertUpdateNewRentalStatus.Item1 switch
            {
                InsertUpdateNewRentalStatus.InsertUpdateDbNoRowsAffected => BadRequest("Booking not added, try again, if persist contact technical support"),
                InsertUpdateNewRentalStatus.OK => Created($"{this.Request.Scheme}://{this.Request.Host.Value}{this.Request.PathBase.Value}{Url.Action(nameof(UpdateRentals))}", new VacationalRentalViewModel { PreparationTimeInDays = insertUpdateNewRentalStatus.Item2.PreparationTimeInDays, Units = insertUpdateNewRentalStatus.Item2.Units }),
                InsertUpdateNewRentalStatus.RentalNotExists => NotFound("Rental not exists"),
                InsertUpdateNewRentalStatus.UnitsQuantityBookedAlready => BadRequest($"Can't change units quantity, they had been already booked {insertUpdateNewRentalStatus.Item2.UnitsBooked} units"),
                InsertUpdateNewRentalStatus.DatesOverlapping => BadRequest($"Dates overlapping on booked units"),
                _ => throw new InvalidOperationException($"{nameof(InsertUpdateNewRentalStatus)}: Not expected or implemented"),
            };
        }
    }
}
