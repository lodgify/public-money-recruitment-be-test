using VacationRental.Shared.Abstractions.Exceptions;

namespace VacationRental.Core.Exceptions
{
    internal class BookingNotExistException : DomainException
    {
        public int BookingId { get; }

        public BookingNotExistException(int bookingId) : base($"Booking with ID: {bookingId}' does not exist")
        {
            BookingId = bookingId;
        }
    }
}
