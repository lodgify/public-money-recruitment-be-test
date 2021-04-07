using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        //private readonly IDictionary<int, RentalViewModel> _rentals;
        //private readonly IDictionary<int, BookingViewModel> _bookings;
        private readonly IBookingService _bookingService;
        private readonly IRentalsRepository _rentalsRepository;

        public BookingsController(
            //IDictionary<int, RentalViewModel> rentals,
            //IDictionary<int, BookingViewModel> bookings,
            IBookingService bookingService,
            IRentalsRepository rentalsRepository)
        {
            //_rentals = rentals;
            //_bookings = bookings;
            _bookingService = bookingService;
            _rentalsRepository = rentalsRepository;
        }

        [HttpGet]
        [Route("{bookingId:int}")]
        public async Task<ActionResult<BookingViewModel>> Get(int bookingId)
        {
            try
            {
                if (!await _bookingService.BookingExists(bookingId))
                    return NotFound("Booking not found");
                //throw new ApplicationException();

                var bookingEntity = await _bookingService.GetBookingById(bookingId);

                return new BookingViewModel
                {
                    Id = bookingEntity.Id,
                    RentalId = bookingEntity.RentalId,
                    Nights = bookingEntity.Nights,
                    Start = bookingEntity.Start
                };
            }
            catch (Exception)
            {
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

                //if (model.Nights <= 0)
                //    return BadRequest("Nigts must be positive");
                //throw new ApplicationException();

                if (!await _rentalsRepository.RentalExists(model.RentalId))
                    return NotFound("Rental not found");
                //if (!_rentals.ContainsKey(model.RentalId))
                //    return NotFound("Rental not found");
                //throw new ApplicationException();

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

                //for (var i = 0; i < model.Nights; i++)
                //{
                //    var count = 0;
                //    foreach (var booking in _bookings.Values)
                //    {
                //        if (booking.RentalId == model.RentalId
                //            && (booking.Start <= model.Start.Date && booking.Start.AddDays(booking.Nights) > model.Start.Date)
                //            || (booking.Start < model.Start.AddDays(model.Nights) && booking.Start.AddDays(booking.Nights) >= model.Start.AddDays(model.Nights))
                //            || (booking.Start > model.Start && booking.Start.AddDays(booking.Nights) < model.Start.AddDays(model.Nights)))
                //        {
                //            count++;
                //        }
                //    }
                //    if (count >= _rentals[model.RentalId].Units)
                //        return Ok("Not available");
                //    //throw new ApplicationException();
                //}


                //var key = new ResourceIdViewModel { Id = _bookings.Keys.Count + 1 };

                //_bookings.Add(key.Id, new BookingViewModel
                //{
                //    Id = key.Id,
                //    Nights = model.Nights,
                //    RentalId = model.RentalId,
                //    Start = model.Start.Date
                //});

                //return key;
            }
            catch (Exception)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
