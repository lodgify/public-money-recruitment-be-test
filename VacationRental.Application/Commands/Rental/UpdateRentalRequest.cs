using MediatR;

namespace VacationRental.Application.Commands.Rental
{
    public class UpdateRentalRequest : IRequest
    {
        public int Id { get; set; }
        public int Units { get; set; }
        public int PreparationTimeInDays { get; set; }
    }
}
