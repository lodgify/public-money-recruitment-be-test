using MediatR;
using VacationRental.Application.Common.ViewModel;

namespace VacationRental.Application.Rentals.Queries.GetRental
{
    public class GetRentalQuery : IRequest<RentalViewModel>
    {
        public int RentalId { get; set; }
    }
}