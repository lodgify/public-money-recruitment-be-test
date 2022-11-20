using VacationRental.Shared.Abstractions.Exceptions;

namespace VacationRental.Core.Exceptions
{
    internal class RentalNotExistException : DomainException
    {
        public int RentalId { get; }

        public RentalNotExistException(int rentalId) : base($"Rental with ID: {rentalId}' does not exist")
        {
            RentalId = rentalId;
        }
    }
}
