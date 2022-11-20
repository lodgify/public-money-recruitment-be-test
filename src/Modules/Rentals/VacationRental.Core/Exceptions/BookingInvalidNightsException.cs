using VacationRental.Shared.Abstractions.Exceptions;

namespace VacationRental.Core.Exceptions
{
    internal class BookingInvalidNightsException : DomainException
    {
        public BookingInvalidNightsException() : base($"Nights must be positive")
        {
        }
    }
}
