using FluentValidation;
using VacationRental.Data;

namespace VacationRental.Domain.Rentals
{
    public class Rental : BaseEntity
    {
        public Rental()
        {
        }

        public Rental(int units, int preparationTime)
        {
            Units = units;
            PreparationTime = preparationTime;
        }

        public int Units { get; set; }

        public int PreparationTime { get; set; }
    }
}
