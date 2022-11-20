using VacationRental.Shared.Abstractions.Exceptions;

namespace VacationRental.Core.Exceptions
{
    internal class BookingNotAvailableException : DomainException
    {
        public BookingNotAvailableException() : base($"Booking not available")
        {
        }
    }
}
