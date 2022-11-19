using VacationRental.Application.DTO;
using VacationRental.Shared.Abstractions.Queries;

namespace VacationRental.Application.Queries
{
    internal class GetRental : IQuery<RentalDto>
    {
        public int Id { get; set; }
    }
}
