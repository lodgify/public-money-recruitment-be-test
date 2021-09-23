using VacationRental.Domain.Values;

namespace VacationRental.Domain.Entities
{
    public class Rental
    {
        public Rental(RentalId id, int units)
        {
            Id = id;
            Units = units;
        }

        public RentalId Id { get; }
        public int Units { get; }
    }
}
