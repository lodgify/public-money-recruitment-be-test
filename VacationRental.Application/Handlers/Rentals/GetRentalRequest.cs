using Flunt.Notifications;
using Flunt.Validations;
using MediatR;
using VacationRental.Application.Notifications;
using VacationRental.Domain.ViewModels;

namespace VacationRental.Application.Handlers.Rentals
{
    public class GetRentalRequest: Notifiable, IRequest<EntityResult<RentalViewModel>>
    {
        public int Id { get; set; }
        public GetRentalRequest(int id)
        {
            Id = id;

            AddNotifications(new Contract()
                .IsGreaterThan(id, 0, "Id", "Id must be greater than 0."));
        }
    }
}
