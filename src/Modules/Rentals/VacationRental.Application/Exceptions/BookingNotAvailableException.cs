using VacationRental.Shared.Abstractions.Exceptions;

namespace VacationRental.Application.Exceptions
{
    internal class BookingNotAvailableException : DomainException
    {
        public BookingNotAvailableException() : base($"Booking not available")
        {
        }
    }
}
