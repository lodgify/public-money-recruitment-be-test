using MediatR;
using VacationRental.Api.Models;

namespace VacationRental.Application.Bookings.Commands.PostBooking
{
    public class PostBookingCommand : IRequest<ResourceIdViewModel>
    {
        public BookingBindingModel Model { get; set; }
    }
}