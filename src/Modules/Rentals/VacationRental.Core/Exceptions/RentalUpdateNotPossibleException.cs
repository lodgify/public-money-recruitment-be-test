using VacationRental.Shared.Abstractions.Exceptions;

namespace VacationRental.Core.Exceptions
{
    internal class RentalUpdateNotPossibleException : DomainException
    {
        public int RentalId { get; }
        public int Units { get; }
        public int PreparationTimeInDays { get; }

        public RentalUpdateNotPossibleException(int rentalId, int units, int preparationTimeInDays)
            : base($"Rental with ID: '{rentalId}' is impossible to update with specified units: '{units}' and preparation time: '{preparationTimeInDays}")
        {
            RentalId = rentalId;
            Units = units;
            PreparationTimeInDays = preparationTimeInDays;
        }
    }
}
