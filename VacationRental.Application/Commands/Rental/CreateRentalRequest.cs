using MediatR;

namespace VacationRental.Application.Commands.Rental
{
    public class CreateRentalRequest : IRequest<ResourceIdResponse>
    {
        public int Units { get; set; }
        public int PreparationTimeInDays { get; set; }
    }
}
