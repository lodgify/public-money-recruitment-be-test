using VacationRental.Domain.Primitives;

namespace VacationRental.Domain.Models.Rentals
{
    public class Rental : BaseDomainModel
    {        
        public int Units { get; private set; }
        public int PreparationTimeInDays { get; private set; }

        private Rental(int units, int preparationTimeInDays) : base()
        {
            Units = units;
            PreparationTimeInDays = preparationTimeInDays;
        }

        public static Rental Create(int units, int preparationTimeInDays)
        {
            return new Rental(units, preparationTimeInDays);
        }

        public void SetUnits(int units)
        {
            Units = units;
        }

        public void SetPreparationTimeInDays(int preparationTimeInDays) 
        {
            PreparationTimeInDays = preparationTimeInDays;
        }


    }
}
