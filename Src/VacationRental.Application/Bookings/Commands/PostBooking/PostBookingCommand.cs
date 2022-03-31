using MediatR;
using VacationRental.Application.Common.ViewModel;

namespace VacationRental.Application.Bookings.Commands.PostBooking
{
    public class PostBookingCommand : IRequest<ResourceIdViewModel>
    {
        public BookingBindingModel Model { get; set; }
    }
}