using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Features;
using VacationRental.Api.Models;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/bookings")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BookingsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{bookingId:int}")]
        public Task<BookingViewModel> Get(int bookingId)
        {
            return _mediator.Send(new GetBooking.Query { Id = bookingId });
        }

        [HttpPost]
        public Task<ResourceIdViewModel> Post(BookingBindingModel model)
        {
            CreateBooking.Command request = new CreateBooking.Command
            {
                RentalId = model.RentalId,
                Nights = model.Nights,
                Start = model.Start
            };

            return _mediator.Send(request);
        }
    }
}
