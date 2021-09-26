using VacationRental.Domain.Values;

namespace VacationRental.Domain.Events.Rental
{
    public class RentalUpdated
    {
        public RentalUpdated(RentalId rentalId,  int units, int preparationTimeIdDays)
        {
            RentalId = rentalId;
            Units = units;
            PreparationTimeIdDays = preparationTimeIdDays;
        }

        public RentalId RentalId { get;  }
        public int Units { get;  }
        public int PreparationTimeIdDays { get; }
    }
}
