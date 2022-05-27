using Flunt.Notifications;
using Flunt.Validations;
using MediatR;
using VacationRental.Application.Notifications;
using VacationRental.Domain.ViewModels;

namespace VacationRental.Application.Handlers.Rentals
{
    public class CreateRentalRequest: Notifiable, IRequest<EntityResult<ResourceIdViewModel>>
    {
        public int Units { get; set; }
        public int PreparationTimeInDays { get; set; }

        public CreateRentalRequest(int units, int preparationTimeInDays)
        {
            Units = units;
            PreparationTimeInDays = preparationTimeInDays;

            AddNotifications(new Contract()
                .IsGreaterThan(Units, 0, "Units", "Units must be grater than 0."));
        }
    }
}
