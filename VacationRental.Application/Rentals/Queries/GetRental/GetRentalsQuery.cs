using MediatR;
using VacationRental.Application.Rentals.Models;

namespace VacationRental.Application.Rentals.Queries.GetRental
{
    public class GetRentalsQuery : IRequest<RentalViewModel>
    {
        public int RentalId { get; set; }
    }
}
