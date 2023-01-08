using VacationRental.Domain.Primitives;

namespace VacationRental.Domain.Errors
{
    public sealed class BookingError : DomainError
    {
        public BookingError(string message) : base(message)
        {
        }

        public static BookingError BookingNotFound = new BookingError("Booking not found");
    }
}
