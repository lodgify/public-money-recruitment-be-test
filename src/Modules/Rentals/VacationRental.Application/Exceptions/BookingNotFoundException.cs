using VacationRental.Shared.Abstractions.Exceptions;

namespace VacationRental.Application.Exceptions
{
    internal class BookingNotFoundException : DomainException
    {
        public int BookingId { get; }

        public BookingNotFoundException(int bookingId) : base($"Booking with ID: {bookingId}' does not exist")
        {
            BookingId = bookingId;
        }
    }
}
