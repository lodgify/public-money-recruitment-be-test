using VacationRental.Data;

namespace VacationRental.Domain.Rentals
{
    public class Rentals : BaseEntity
    {
        public Rentals(int units, int preparationTime)
        {
            Units = units;
            PreparationTime = preparationTime;
        }

        public int Units { get; set; }

        public int PreparationTime { get; set; }
    }
}
