using MediatR;

namespace VacationRental.Application.Queries.Rental
{
    public sealed class GetRentalByIdQuery : IRequest<RentalViewModel>
    {
        public GetRentalByIdQuery(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }
}
