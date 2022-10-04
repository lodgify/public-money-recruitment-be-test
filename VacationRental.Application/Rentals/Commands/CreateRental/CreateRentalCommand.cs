using MediatR;

namespace VacationRental.Application.Rentals.Commands.CreateRental
{
    public class CreateRentalCommand : IRequest<ResourceIdViewModel>
    {
        public int Units { get; set; }
        public int PreparationTimeInDays { get; set; }
    }
}
