using VacationRental.Domain.Values;

namespace VacationRental.Domain.Exceptions
{
    public class NoAvailableUnitException : DomainException
    {
        public NoAvailableUnitException(RentalId rentalId) : base($"Rental '{rentalId}' doesn't have an available unit for the request period")
        {
            
        }
    }
}
