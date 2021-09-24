using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Application.Commands;
using VacationRental.Application.Commands.Booking;
using VacationRental.Application.Queries.Booking;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/bookings")]
    [ApiController]
    public class BookingsController : ControllerBase
    {

        private readonly IMediator _mediator;

        public BookingsController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        //[HttpGet]
        //[Route("{bookingId:int}")]
        //public BookingViewModel Get(int bookingId)
        //{
        //    if (!_bookings.ContainsKey(bookingId))
        //        throw new ApplicationException("Booking not found");

        //    return _bookings[bookingId];
        //}

        //[HttpPost]
        //public ResourceIdViewModel Post(BookingBindingModel model)
        //{
        //    if (model.Nights <= 0)
        //        throw new ApplicationException("Nigts must be positive");
        //    if (!_rentals.ContainsKey(model.RentalId))
        //        throw new ApplicationException("Rental not found");

        //    for (var i = 0; i < model.Nights; i++)
        //    {
        //        var count = 0;
        //        foreach (var booking in _bookings.Values)
        //        {
        //            if (booking.RentalId == model.RentalId
        //                && (booking.Start <= model.Start.Date && booking.Start.AddDays(booking.Nights) > model.Start.Date)
        //                || (booking.Start < model.Start.AddDays(model.Nights) && booking.Start.AddDays(booking.Nights) >= model.Start.AddDays(model.Nights))
        //                || (booking.Start > model.Start && booking.Start.AddDays(booking.Nights) < model.Start.AddDays(model.Nights)))
        //            {
        //                count++;
        //            }
        //        }
        //        if (count >= _rentals[model.RentalId].Units)
        //            throw new ApplicationException("Not available");
        //    }


        //    var key = new ResourceIdViewModel { Id = _bookings.Keys.Count + 1 };

        //    _bookings.Add(key.Id, new BookingViewModel
        //    {
        //        Id = key.Id,
        //        Nights = model.Nights,
        //        RentalId = model.RentalId,
        //        Start = model.Start.Date
        //    });

        //    return key;
        //} 


        [HttpGet]
        [Route("{bookingId:int}")]
        public async Task<BookingViewModel> Get(int bookingId)
        {
            return await _mediator.Send(new GetBookingByIdQuery(bookingId));
        }

        [HttpPost]
        public async Task<ResourceIdViewModel> Create(BookingRequest request)
        {
            return await _mediator.Send(request);
        }
    }
}
