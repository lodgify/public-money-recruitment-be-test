using Flunt.Notifications;
using Flunt.Validations;
using MediatR;
using VacationRental.Application.Notifications;
using VacationRental.Domain.ViewModels;

namespace VacationRental.Application.Handlers.Bookings
{
    public class GetBookingRequest: Notifiable, IRequest<EntityResult<BookingViewModel>>
    {
        public int Id { get; set; }
        public GetBookingRequest(int id)
        {
            Id = id;

            AddNotifications(new Contract()
                .IsGreaterThan(id, 0, "Id", "Id must be greater than 0."));
        }
    }
}
