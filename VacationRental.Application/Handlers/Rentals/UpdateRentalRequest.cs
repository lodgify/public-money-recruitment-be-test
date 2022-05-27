using Flunt.Notifications;
using Flunt.Validations;
using MediatR;
using VacationRental.Application.Notifications;
using VacationRental.Domain.ViewModels;

namespace VacationRental.Application.Handlers.Rentals
{
    public class UpdateRentalRequest: Notifiable, IRequest<EntityResult<RentalViewModel>>
    {
        public int Id { get; set; }
        public int Units { get; set; }
        public int PreparationTimeInDays { get; set; }

        public UpdateRentalRequest(int id, int units, int preparationTimeInDays)
        {
            Units = units;
            PreparationTimeInDays = preparationTimeInDays;
            Id = id;

            AddNotifications(new Contract()
                .IsGreaterThan(Units, 0, "Units", "Units must be grater than 0.")
                .IsGreaterThan(Id, 0, "Id", "Id must be grater than 0."));
        }
    }
}
