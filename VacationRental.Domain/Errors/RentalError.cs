using VacationRental.Domain.Primitives;

namespace VacationRental.Domain.Errors
{
    public sealed class RentalError : DomainError
    {
        public RentalError(string message) : base(message)
        {
        }

        public static RentalError RentalNotFound = new RentalError("Rental not found");
        public static RentalError RentalNotAvailable = new RentalError("Not available");
    }
}
