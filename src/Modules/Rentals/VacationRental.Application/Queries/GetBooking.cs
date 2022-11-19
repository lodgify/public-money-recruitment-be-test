using VacationRental.Application.DTO;
using VacationRental.Shared.Abstractions.Queries;

namespace VacationRental.Application.Queries
{
    internal class GetBooking : IQuery<BookingDto>
    {
        public int Id { get; set; }
    }
}
