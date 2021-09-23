using VacationRental.Application.Exceptions;
using VacationRental.Domain.Values;

namespace VacationRental.Infrastructure.Persist.Exceptions
{
    public class BookingNotFoundException : InfrastructureException
    {
        public BookingId Id { get; }
        public BookingNotFoundException(BookingId bookingId) : base($"Booking '{bookingId.Id}' isn't found")
        {

        }
    }
}
