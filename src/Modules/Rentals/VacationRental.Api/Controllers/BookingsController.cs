using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VacationRental.Api.Models;
using VacationRental.Application.Commands;
using VacationRental.Application.Queries;
using VacationRental.Shared.Abstractions.Dispatchers;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/bookings")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IDispatcher _dispatcher;

        public BookingsController(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        [HttpGet]
        [Route("{bookingId:int}")]
        public async Task<ActionResult<BookingViewModel>> Get(int bookingId)
        {
            var booking = await _dispatcher.QueryAsync(new GetBooking { Id = bookingId });

            return Ok(booking.AsViewModel());
        }

        [HttpPost]
        public async Task<ActionResult<ResourceIdViewModel>> Post(BookingBindingModel model)
        {
            var id = await _dispatcher.SendAsync(new AddBooking(model.RentalId, model.Start, model.Nights));

            return Ok(new ResourceIdViewModel { Id = id });
        }
    }
}
