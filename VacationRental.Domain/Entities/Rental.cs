using VacationRental.Domain.Values;

namespace VacationRental.Domain.Entities
{
    public class Rental
    {
        public Rental(RentalId id, int units, int preparationTimeInDays)
        {
            Id = id;
            Units = units;
            PreparationTimeInDays = preparationTimeInDays;
        }

        public RentalId Id { get; }
        public int Units { get; }
        public int PreparationTimeInDays { get; }
    }
}
