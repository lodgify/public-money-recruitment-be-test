using VacationRental.Domain.Values;

namespace VacationRental.Domain.Exceptions
{
    public sealed class BookingNotFoundException : EntityNotFoundException
    {
        public BookingId Id { get; }
        public BookingNotFoundException(BookingId bookingId) : base($"Booking '{bookingId.Id}' isn't found")
        {

        }
    }
}
