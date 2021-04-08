using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VacationalRental.Domain.Enums;
using VacationalRental.Domain.Interfaces.Repositories;
using VacationalRental.Domain.Interfaces.Services;
using VacationRental.Api.Models;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/bookings")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly IRentalsRepository _rentalsRepository;
        private readonly ILogger<BookingsController> _logger;

        public BookingsController(
            IBookingService bookingService,
            IRentalsRepository rentalsRepository,
            ILogger<BookingsController> logger)
        {
            _bookingService = bookingService;
            _rentalsRepository = rentalsRepository;
            _logger = logger;
        }

        [HttpGet]
        [Route("{bookingId:int}")]
        public async Task<ActionResult<BookingViewModel>> Get(int bookingId)
        {
            try
            {
                if (!await _bookingService.BookingExists(bookingId))
                    return NotFound("Booking not found");

                var bookingEntity = await _bookingService.GetBookingById(bookingId);

                return new BookingViewModel
                {
                    Id = bookingEntity.Id,
                    RentalId = bookingEntity.RentalId,
                    Nights = bookingEntity.Nights,
                    Start = bookingEntity.Start
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(Get));
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post([FromBody] BookingBindingModel model)
        {
            try
            {
                if (model == null || !ModelState.IsValid)
                    return BadRequest(ModelState);

                if (!await _rentalsRepository.RentalExists(model.RentalId))
                    return NotFound("Rental not found");

                var bookingInfo = await _bookingService.InsertNewBooking(
                    new VacationalRental.Domain.Entities.BookingEntity
                {
                        RentalId = model.RentalId,
                        Nights = model.Nights,
                        Start = model.Start
                });

                return bookingInfo.Item1 switch
                {
                    InsertNewBookingStatus.NotAvailable => BadRequest("Not available"),
                    InsertNewBookingStatus.InsertDbNoRowsAffected => BadRequest("Booking not added, try again, if persist contact technical support"),
                    InsertNewBookingStatus.OK => Ok(new { id = bookingInfo.Item2 }),
                    _ => throw new InvalidOperationException($"{nameof(InsertNewBookingStatus)}: Not expected or implemented"),
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(Post));

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
