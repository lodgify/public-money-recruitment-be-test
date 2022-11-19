using VacationRental.Shared.Abstractions.Exceptions;

namespace VacationRental.Application.Exceptions
{
    internal class RentalNotFoundException : DomainException
    {
        public int RentalId { get; }

        public RentalNotFoundException(int rentalId) : base($"Rental with ID: {rentalId}' does not exist")
        {
            RentalId = rentalId;
        }
    }
}
